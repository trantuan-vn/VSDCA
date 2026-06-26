Imports System
Imports System.ServiceModel
Imports System.Collections.Generic
Imports System.Runtime.Serialization
Imports Sats.CommonLibrary
Imports Sats.ActiveDirectory
Imports Sats.HOSTChannel
Imports Sats.HOSTReportChannel
Imports System.EnterpriseServices
Imports Sats.ServerCA
Imports BkavCASign
Imports System.IO

<ServiceBehavior(InstanceContextMode:=InstanceContextMode.Single, _
                 ConcurrencyMode:=ConcurrencyMode.Multiple, _
                 UseSynchronizationContext:=False)> _
Public Class BDSWCFService
    Implements IWCF

    Private mv_lstClients As New Dictionary(Of Client, IWCFCallback)
    Private mv_ClientList As New List(Of Client)
    Private mv_oSyncObj As New Object
    'Private mv_lwLogWriter As New LogWriter

    Public ReadOnly Property CurrentCallback() As IWCFCallback
        Get
            Return OperationContext.Current.GetCallbackChannel(Of IWCFCallback)()
        End Get
    End Property

#Region "IWCF"

    Private Function SearchClientsByName(ByVal pv_oClient As Client) As Boolean
        For Each v_oClient In mv_lstClients.Keys
            If v_oClient.IPAddress = pv_oClient.IPAddress And v_oClient.TLNAME = pv_oClient.TLNAME Then
                Return True
            End If
        Next
        Return False
    End Function

    Private Sub Remove(ByRef pv_oClient As Client)
        For Each v_oClient In mv_lstClients.Keys
            If v_oClient.IPAddress = pv_oClient.IPAddress And v_oClient.TLNAME = pv_oClient.TLNAME Then
                pv_oClient.LoginTime = v_oClient.LoginTime
                mv_lstClients.Remove(v_oClient)
                Exit For
            End If
        Next

        For i As Integer = 0 To mv_ClientList.Count - 1
            If mv_ClientList(i).IPAddress = pv_oClient.IPAddress And mv_ClientList(i).TLNAME = pv_oClient.TLNAME Then
                mv_ClientList.RemoveAt(i)
                Exit For
            End If
        Next
    End Sub

    Public Function Connect(ByVal pv_oClient As Client) As Boolean Implements IWCF.Connect
        If Not mv_lstClients.ContainsValue(CurrentCallback) And Not SearchClientsByName(pv_oClient) Then
            SyncLock (mv_oSyncObj)
                pv_oClient.LoginTime = Now.ToUniversalTime
                pv_oClient.Action = Now.ToUniversalTime & ": Login"
                mv_lstClients.Add(pv_oClient, CurrentCallback)
                mv_ClientList.Add(pv_oClient)
            End SyncLock
        Else
            SyncLock (mv_oSyncObj)
                Remove(pv_oClient)
                pv_oClient.Action = Now.ToUniversalTime & ": Reconnect"
                mv_lstClients.Add(pv_oClient, CurrentCallback)
                mv_ClientList.Add(pv_oClient)
            End SyncLock
        End If

        Try
            Dim v_oCallBack As IWCFCallback
            v_oCallBack = GetAdmistrator()
            If Not v_oCallBack Is Nothing Then
                Dim v_ds As DataSet
                v_ds = CreateMessage(pv_oClient)
                v_oCallBack.ReceiveAction(v_ds)
            End If
        Catch ex As Exception

        End Try
          
    End Function

    Public Sub Disconnect(ByVal pv_oClient As Client) Implements IWCF.Disconnect
        If pv_oClient.IPAddress <> "Administrator" And pv_oClient.TLNAME <> "Administrator" Then
            For i As Integer = 0 To mv_ClientList.Count - 1
                If pv_oClient.TLNAME = mv_ClientList(i).TLNAME And pv_oClient.IPAddress = mv_ClientList(i).IPAddress Then
                    pv_oClient.LoginTime = mv_ClientList(i).LoginTime
                    pv_oClient.Action = Now.ToUniversalTime & ": Logout"
                    Exit For
                End If
            Next
            Try
                Dim v_oCallBack As IWCFCallback
                v_oCallBack = GetAdmistrator()
                If Not v_oCallBack Is Nothing Then
                    Dim v_ds As DataSet
                    v_ds = CreateMessage(pv_oClient)
                    v_oCallBack.ReceiveAction(v_ds)
                End If
            Catch ex As Exception

            End Try
        End If
        For Each v_oClient As Client In mv_lstClients.Keys
            If v_oClient.IPAddress = pv_oClient.IPAddress And v_oClient.TLNAME = pv_oClient.TLNAME Then
                SyncLock (mv_oSyncObj)
                    mv_lstClients.Remove(v_oClient)
                    mv_ClientList.Remove(v_oClient)
                End SyncLock
                Exit For
            End If
        Next
    End Sub

    Public Function Login(ByVal pv_strUserName As String, Optional ByVal pv_strPassword As String = "", Optional ByVal pv_strIPAddress As String = "") As String Implements IWCF.Login
        Dim v_strAuthType As String = System.Configuration.ConfigurationManager.AppSettings("AuthType").ToString
        Dim v_eLoginResult As Utility.LoginResult = Utility.LoginResult.LOGIN_OK
        Dim v_strTicket As String = ""

        If v_strAuthType = gc_AUTHORIZATION_MODE_LDAP Then
            v_eLoginResult = ADManager.Login(pv_strUserName, DecryptString(pv_strUserName, pv_strPassword), pv_strIPAddress)
        End If

        If v_eLoginResult = Utility.LoginResult.LOGIN_OK Then
            Dim v_oWCF As New HostWCFChannel
            Try
                v_oWCF.Connect()
                If v_strAuthType = gc_AUTHORIZATION_MODE_LDAP Then
                    v_strTicket = v_oWCF.Login(pv_strUserName, "", "")
                Else
                    v_strTicket = v_oWCF.Login(pv_strUserName, pv_strPassword, pv_strIPAddress)
                End If
                Return v_strTicket
            Catch ex As Exception
                Throw ex
                Return v_strTicket
            Finally
                If Not v_oWCF Is Nothing Then
                    v_oWCF.Close()
                    v_oWCF = Nothing
                End If
            End Try
        Else
            Return Nothing
        End If
    End Function

    Public Function LoginCA(ByVal pv_strEncryptedXML As String, _
        ByRef pv_strEncryptedSessionKey As String, ByRef pv_strResult As String) As String Implements CommonLibrary.IWCF.LoginCA
        ' ServerBussinessCA.testEncrypt()
        'Certificate data
        Dim v_strUserName = ""
        Dim v_strPassword = ""
        Dim v_strIPAddress = ""
        Dim v_oCertServer As New CertificateServer

        'Open HSM
        'ServerBussinessCA.OpenHSM(mv_oSignServer)
        Try
            'Get config for connecting BKAV LDAP
            Dim v_strVSDUsername As String = _
                        System.Configuration.ConfigurationManager.AppSettings("VSDUsername").ToString
            Dim v_strBKAVPassword As String = _
                        System.Configuration.ConfigurationManager.AppSettings("BKAVPassword").ToString
            Dim v_strLDAPIP As String = _
                        System.Configuration.ConfigurationManager.AppSettings("LDAPIP").ToString
            Dim v_strArrayOgarnization As String = _
                        System.Configuration.ConfigurationManager.AppSettings("ArrayOgarnization").ToString
            Dim v_strBKAVDomainComponent As String = _
                System.Configuration.ConfigurationManager.AppSettings("BKAVDomainComponent").ToString

            Dim v_strOrigXML = ServerBussinessCA.DecryptXML(pv_strEncryptedXML, _
                        pv_strEncryptedSessionKey, v_oCertServer, mv_oSignServer, _
                        v_strVSDUsername, v_strBKAVPassword, v_strLDAPIP, v_strArrayOgarnization, _
                        v_strBKAVDomainComponent)
            If (v_strOrigXML.StartsWith(ServerBussinessCA.HEADER_ERROR)) Then
                pv_strResult = v_strOrigXML
                Return pv_strResult
            End If

            'Get infomation
            pv_strResult = ServerBussinessCA.GetOrigXMLInfo(v_strOrigXML, _
                        v_strUserName, v_strPassword, v_strIPAddress)
            If (v_strOrigXML.StartsWith(ServerBussinessCA.HEADER_ERROR)) Then
                pv_strResult = v_strOrigXML
                Return pv_strResult
            End If


            Dim v_strAuthType As String = System.Configuration.ConfigurationManager.AppSettings("AuthType").ToString
            Dim v_eLoginResult As Utility.LoginResult = Utility.LoginResult.LOGIN_OK
            Dim v_strTicket As String = ""

            If v_strAuthType = gc_AUTHORIZATION_MODE_LDAP Then
                'v_eLoginResult = ADManager.Login(pv_strUserName, DecryptString(pv_strUserName, pv_strPassword), pv_strIPAddress)
                v_eLoginResult = ADManager.Login(v_strUserName, DecryptString(v_strUserName, v_strPassword), v_strIPAddress)
            End If

            If v_eLoginResult = Utility.LoginResult.LOGIN_OK Then
                Dim v_oWCF As New HostWCFChannel

                v_oWCF.Connect()
                If v_strAuthType = gc_AUTHORIZATION_MODE_LDAP Then
                    'v_strTicket = v_oWCF.Login(v_strUserName, "", "")
                    pv_strResult = v_oWCF.Login(v_strUserName, "", "")
                Else
                    'v_strTicket = v_oWCF.Login(v_strUserName, v_strPassword, v_strIPAddress)
                    pv_strResult = v_oWCF.Login(v_strUserName, v_strPassword, v_strIPAddress)
                End If
                'Return v_strTicket

                If Not v_oWCF Is Nothing Then
                    v_oWCF.Close()
                    v_oWCF = Nothing
                End If


                'After login
                'Đoạn này phải sửa để chuyển xử lý db lên host BDS_DATA

                AfterLoginCA(pv_strEncryptedSessionKey, _
                            pv_strResult, v_strUserName)

                'ServerBussinessCA.AfterLogin(pv_strEncryptedSessionKey, _
                '            pv_strResult, v_oCertServer, v_strUserName, mv_oSignServer)
                Return pv_strResult
            Else
                Return Nothing
            End If
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                & "Error code: System error!" & vbNewLine _
                & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_HOST)
            Return ERR_SYSTEM_START
        End Try
    End Function
    'Public Function Login(ByVal pv_strUserName As String, Optional ByVal pv_strPassword As String = "", Optional ByVal pv_strIPAddress As String = "") As String Implements IWCF.Login
    '    Dim v_obj As New Branch.Branch
    '    Try
    '        Return v_obj.GetAuthorizationTicket(pv_strUserName, pv_strPassword)
    '    Catch ex As Exception
    '        Return Nothing
    '    Finally
    '        If Not v_obj Is Nothing Then
    '            v_obj = Nothing
    '        End If
    '    End Try
    'End Function

    Public Function AfterLoginCA(ByRef pv_strEncryptedSessionKey As String, _
        ByRef pv_strResult As String, _
        ByVal pv_strUserName As String) As String Implements IWCF.AfterLoginCA
        Try
            Dim v_oWCF As New HostWCFChannel

            v_oWCF.Connect()
            v_oWCF.AfterLoginCA(pv_strEncryptedSessionKey, _
                        pv_strResult, pv_strUserName)
            If Not v_oWCF Is Nothing Then
                v_oWCF.Close()
                v_oWCF = Nothing
            End If
            Return pv_strResult

        Catch ex As Exception
            Return ERR_SYSTEM_START
        End Try
    End Function
    Public Function FileCA(ByRef pv_strMessage As String, ByVal pv_strBrid As String, _
                            ByVal pv_strFileName As String, ByVal pv_strCurrDate As String) As String Implements IWCF.FileCA
        Try
            Dim v_strReturn As String
            If pv_strFileName = "9999" Then
                'Convert.FromBase64String(v_strDataFileServer)
                'Dim v_arrSource As Byte()
                'v_arrSource = Convert.FromBase64String(pv_strMessage)
                v_strReturn = ServerBussinessCA.SignByte(pv_strMessage, mv_oSignServer)

            Else
                v_strReturn = ServerBussinessCA.EncryptXML(pv_strMessage)
                'Đoạn này phải xử lý để chuyển lên host xử lý đoạn get database BDS_DATA

                Dim v_strFilePath = ServerBussinessCA.GetFileCAPath(pv_strBrid)
                v_strFilePath = v_strFilePath & pv_strBrid & "\" & pv_strCurrDate & "\"
                Dim v_strFileName = v_strFilePath & "ENCRYPTED_" & pv_strFileName & ".xml"

                Dim v_xmlEncryptedXML As Xml.XmlDocument = New Xml.XmlDocument
                v_xmlEncryptedXML.LoadXml(v_strReturn)
                If Not Directory.Exists(v_strFilePath) Then
                    Directory.CreateDirectory(v_strFilePath)
                End If
                'xxx
                If ServerBussinessCA.CheckExistFile(v_strFilePath, v_strFileName) Then
                    File.Delete(v_strFileName)
                    v_xmlEncryptedXML.Save(v_strFileName)
                Else
                    v_xmlEncryptedXML.Save(v_strFileName)
                End If
            End If
            pv_strMessage = v_strReturn
            Return pv_strMessage
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
             & "Error code: System error!" & vbNewLine _
             & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_HOST)
            Throw ex
        Finally

        End Try
    End Function
    'Public Function AfterLoginCA(ByRef pv_strEncryptedSessionKey As String, _
    '    ByRef pv_strResult As String, ByVal pv_oCertServer As CertificateServer, _
    '    ByVal pv_strUserName As String, ByVal pv_oSignServer As SignServer) As String Implements IWCF.AfterLoginCA
    '    Try
    '        Return mv_oProxy.AfterLoginCA(pv_strEncryptedSessionKey, pv_strResult, pv_oCertServer, pv_strUserName, pv_oSignServer)
    '    Catch ex As Exception
    '        Return ERR_SYSTEM_START
    '    End Try
    'End Function
    Public Function ChangePassword(ByVal pv_strUserName As String, ByVal pv_strNewPassword As String) As Long Implements CommonLibrary.IWCF.ChangePassword
        Try
            Dim v_oUser As New ADUser
            v_oUser = ADManager.Instance.LoadUser(pv_strUserName)
            v_oUser.SetPassword(DecryptString(pv_strUserName, pv_strNewPassword))
            Return ERR_SYSTEM_OK
        Catch ex As Exception
            Return -1
        End Try
    End Function

    Public Function Send(ByRef pv_arrByte() As Byte) As Long Implements IWCF.Send
        Dim v_oWCF As New HostWCFChannel
        Try
            v_oWCF.Connect()
            Return v_oWCF.Send(pv_arrByte)
        Catch ex As Exception
            Throw ex
        Finally
            If Not v_oWCF Is Nothing Then
                v_oWCF.Close()
                v_oWCF = Nothing
            End If
        End Try
    End Function

    Public Function SendCA(ByRef pv_arrByte() As Byte, ByRef v_arrByteCA() As Byte, ByRef v_strTLLOGIDCA As String, ByRef v_strVSDSignature As String) As Long Implements IWCF.SendCA
        Dim v_oWCF As New HostWCFChannel
        Try
            v_oWCF.Connect()
            Return v_oWCF.SendCA(pv_arrByte, v_arrByteCA, v_strTLLOGIDCA, v_strVSDSignature)
        Catch ex As Exception
            Throw ex
        Finally
            If Not v_oWCF Is Nothing Then
                v_oWCF.Close()
                v_oWCF = Nothing
            End If
        End Try
    End Function

    'Function FetchRpt(ByVal pv_strRptDataKey As String, ByVal pv_intFrom As Integer, ByVal pv_intTo As Integer, ByRef pv_arrByte() As Byte) As Long Implements IWCF.FetchRpt
    '    Dim v_oWCF As New HostWCFChannel
    '    Try
    '        v_oWCF.Connect()
    '        Return v_oWCF.FetchRpt(pv_strRptDataKey, pv_intFrom, pv_intTo, pv_arrByte)
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If Not v_oWCF Is Nothing Then
    '            v_oWCF.Close()
    '            v_oWCF = Nothing
    '        End If
    '    End Try
    'End Function
    Function FetchRpt(ByVal pv_strRptDataKey As String, ByVal pv_intFrom As Integer, ByVal pv_intTo As Integer, ByRef pv_arrByte() As Byte) As Long Implements IWCF.FetchRpt
        Dim v_oWCF As New HostReportWCFChannel
        Try
            v_oWCF.Connect()
            Return v_oWCF.FetchRpt(pv_strRptDataKey, pv_intFrom, pv_intTo, pv_arrByte)
        Catch ex As Exception
            Throw ex
        Finally
            If Not v_oWCF Is Nothing Then
                v_oWCF.Close()
                v_oWCF = Nothing
            End If
        End Try
    End Function

    'Public Function SendRpt(ByRef pv_strMessage As String, ByRef pv_arrByte() As Byte) As Long Implements CommonLibrary.IWCF.SendRpt
    '    Dim v_oWCF As New HostWCFChannel
    '    Try
    '        v_oWCF.Connect()
    '        Return v_oWCF.SendRpt(pv_strMessage, pv_arrByte)
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If Not v_oWCF Is Nothing Then
    '            v_oWCF.Close()
    '            v_oWCF = Nothing
    '        End If
    '    End Try
    'End Function
    Public Function SendRpt(ByRef pv_strMessage As String, ByRef pv_arrByte() As Byte) As Long Implements CommonLibrary.IWCF.SendRpt
        Dim v_oWCF As New HostReportWCFChannel
        Try
            v_oWCF.Connect()
            Return v_oWCF.SendRpt(pv_strMessage, pv_arrByte)
        Catch ex As Exception
            Throw ex
        Finally
            If Not v_oWCF Is Nothing Then
                v_oWCF.Close()
                v_oWCF = Nothing
            End If
        End Try
    End Function
    Public Function GetServerRptExp(ByVal pv_strRptId As String, ByVal pv_strTLName As String, ByRef pv_intServerRptExp As Integer, ByVal pv_strSiCode As String, ByRef pv_strSiCodeAllow As String) As Long Implements CommonLibrary.IWCF.GetServerRptExp
        Dim v_oWCF As New HostWCFChannel
        Try
            'Dim pv_arrByte() As Byte

            v_oWCF.Connect()
            Return v_oWCF.GetServerRptExp(pv_strRptId, pv_strTLName, pv_intServerRptExp, pv_strSiCode, pv_strSiCodeAllow)
        Catch ex As Exception
            Throw ex
        Finally
            If Not v_oWCF Is Nothing Then
                v_oWCF.Close()
                v_oWCF = Nothing
            End If
        End Try
    End Function
    Public Function SendExpRpt(ByVal pv_strMessage As String) As Long Implements CommonLibrary.IWCF.SendExpRpt
        Dim v_oWCF As New HostReportWCFChannel
        Try
            'Dim pv_arrByte() As Byte

            v_oWCF.Connect()
            Return v_oWCF.SendExpRpt(pv_strMessage)
        Catch ex As Exception
            Throw ex
        Finally
            If Not v_oWCF Is Nothing Then
                v_oWCF.Close()
                v_oWCF = Nothing
            End If
        End Try
    End Function
    'Public Function SendRptCA(ByRef pv_strMessage As String, ByRef pv_arrByte() As Byte, ByRef v_strVSDSignature As String, ByRef v_strCAKey As String, ByRef pv_strDataHash As String) As Long Implements CommonLibrary.IWCF.SendRptCA
    '    Dim v_oWCF As New HostWCFChannel
    '    Try
    '        v_oWCF.Connect()
    '        Return v_oWCF.SendRptCA(pv_strMessage, pv_arrByte, v_strVSDSignature, v_strCAKey, pv_strDataHash)
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If Not v_oWCF Is Nothing Then
    '            v_oWCF.Close()
    '            v_oWCF = Nothing
    '        End If
    '    End Try
    'End Function
    Public Function SendRptCA(ByRef pv_strMessage As String, ByRef pv_arrByte() As Byte, ByRef v_strVSDSignature As String, ByRef v_strCAKey As String, ByRef pv_strDataHash As String) As Long Implements CommonLibrary.IWCF.SendRptCA
        Dim v_oWCF As New HostReportWCFChannel
        Try
            v_oWCF.Connect()
            Return v_oWCF.SendRptCA(pv_strMessage, pv_arrByte, v_strVSDSignature, v_strCAKey, pv_strDataHash)
        Catch ex As Exception
            Throw ex
        Finally
            If Not v_oWCF Is Nothing Then
                v_oWCF.Close()
                v_oWCF = Nothing
            End If
        End Try
    End Function
    'Public Function SaveFileRptCA(ByVal pv_arrByte() As Byte, ByVal v_strVSDSignature As String, ByVal v_strClientVSDSignature As String, ByVal v_strTLName As String, ByVal v_strRptId As String, ByRef v_strLocalDir As String, ByRef v_strFileName As String, ByVal v_strStatus As String) As Long Implements CommonLibrary.IWCF.SaveFileRptCA
    '    Dim v_oWCF As New HostWCFChannel
    '    Try
    '        v_oWCF.Connect()
    '        Return v_oWCF.SaveFileRptCA(pv_arrByte, v_strVSDSignature, v_strClientVSDSignature, v_strTLName, v_strRptId, v_strLocalDir, v_strFileName, v_strStatus)
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If Not v_oWCF Is Nothing Then
    '            v_oWCF.Close()
    '            v_oWCF = Nothing
    '        End If
    '    End Try
    'End Function

    Public Function SaveFileRptCA(ByVal pv_arrByte() As Byte, ByVal v_strVSDSignature As String, ByVal v_strClientVSDSignature As String, ByVal v_strTLName As String, ByVal v_strRptId As String, ByRef v_strLocalDir As String, ByRef v_strFileName As String, ByVal v_strStatus As String) As Long Implements CommonLibrary.IWCF.SaveFileRptCA
        Dim v_oWCF As New HostReportWCFChannel
        Try
            v_oWCF.Connect()
            Return v_oWCF.SaveFileRptCA(pv_arrByte, v_strVSDSignature, v_strClientVSDSignature, v_strTLName, v_strRptId, v_strLocalDir, v_strFileName, v_strStatus)
        Catch ex As Exception
            Throw ex
        Finally
            If Not v_oWCF Is Nothing Then
                v_oWCF.Close()
                v_oWCF = Nothing
            End If
        End Try
    End Function

    Public Function SendAction(ByVal pv_oClient As CommonLibrary.Client) As Long Implements CommonLibrary.IWCF.SendAction
        For i As Integer = 0 To mv_ClientList.Count - 1
            If pv_oClient.TLNAME = mv_ClientList(i).TLNAME And pv_oClient.IPAddress = mv_ClientList(i).IPAddress Then
                pv_oClient.LoginTime = mv_ClientList(i).LoginTime
                pv_oClient.Action = Now.ToUniversalTime & ": " & pv_oClient.Action
                mv_ClientList.RemoveAt(i)
                mv_ClientList.Add(pv_oClient)
                Exit For
            End If
        Next

        Try
            Dim v_oCallBack As IWCFCallback
            v_oCallBack = GetAdmistrator()
            If Not v_oCallBack Is Nothing Then
                Dim v_ds As DataSet
                v_ds = CreateMessage(pv_oClient)
                v_oCallBack.ReceiveAction(v_ds)
            End If
        Catch ex As Exception

        End Try
    End Function

    Public Sub SynSend(ByRef pv_arrByte() As Byte) Implements IWCF.SynSend

    End Sub

    Public Function GetTellerProfile(ByVal pv_strUserName As String) As CommonLibrary.WCFTellerProfile Implements CommonLibrary.IWCF.GetTellerProfile
        Dim v_oWCF As New HostWCFChannel
        Try
            v_oWCF.Connect()
            Return v_oWCF.GetTellerProfile(pv_strUserName)
        Catch ex As Exception
            Throw ex
            Return Nothing
        Finally
            If Not v_oWCF Is Nothing Then
                v_oWCF.Close()
                v_oWCF = Nothing
            End If
        End Try
    End Function


    Private Function GetAdmistrator() As IWCFCallback
        For Each v_oClient As Client In mv_lstClients.Keys
            If v_oClient.TLNAME = "Administrator" Then
                Return mv_lstClients(v_oClient)
            End If
        Next
        Return Nothing
    End Function

    Private Function CreateMessage(ByVal pv_oClient As Client) As DataSet
        Dim v_ds As DataSet
        Try
            v_ds = New DataSet
            Dim v_dt As DataTable
            Dim v_oRow As DataRow
            Dim v_oColumn As DataColumn

            v_dt = v_ds.Tables.Add

            v_oColumn = New DataColumn("IPAddress")
            v_oColumn.ColumnName = "IPAddress"
            v_oColumn.DataType = GetType(System.String)
            v_dt.Columns.Add(v_oColumn)

            v_oColumn = New DataColumn("WSName")
            v_oColumn.ColumnName = "WSName"
            v_oColumn.DataType = GetType(System.String)
            v_dt.Columns.Add(v_oColumn)

            v_oColumn = New DataColumn("TLName")
            v_oColumn.ColumnName = "TLName"
            v_oColumn.DataType = GetType(System.String)
            v_dt.Columns.Add(v_oColumn)

            v_oColumn = New DataColumn("LoginTime")
            v_oColumn.ColumnName = "LoginTime"
            v_oColumn.DataType = GetType(System.DateTime)
            v_dt.Columns.Add(v_oColumn)

            v_oColumn = New DataColumn("Action")
            v_oColumn.ColumnName = "Action"
            v_oColumn.DataType = GetType(System.String)
            v_dt.Columns.Add(v_oColumn)

            v_oColumn = New DataColumn("isActive")
            v_oColumn.ColumnName = "isActive"
            v_oColumn.DataType = GetType(System.String)
            v_dt.Columns.Add(v_oColumn)

            v_oColumn = New DataColumn("isNotActive")
            v_oColumn.ColumnName = "isNotActive"
            v_oColumn.DataType = GetType(System.String)
            v_dt.Columns.Add(v_oColumn)

            Dim v_intActive As Integer = GetActive(True)
            Dim v_intNotActive As Integer = GetActive(False)

            If pv_oClient.IPAddress = "Administrator" And pv_oClient.TLNAME = "Administrator" Then
                For i As Integer = 0 To mv_ClientList.Count - 1
                    v_oRow = v_dt.NewRow
                    v_oRow("IPAddress") = mv_ClientList(i).IPAddress
                    v_oRow("WSName") = mv_ClientList(i).WSName
                    v_oRow("TLName") = mv_ClientList(i).TLNAME
                    v_oRow("LoginTime") = mv_ClientList(i).LoginTime
                    v_oRow("Action") = mv_ClientList(i).Action
                    v_oRow("isActive") = v_intActive
                    v_oRow("isNotActive") = v_intNotActive
                    v_dt.Rows.Add(v_oRow)
                Next
            Else
                v_oRow = v_dt.NewRow
                v_oRow("IPAddress") = pv_oClient.IPAddress
                v_oRow("WSName") = pv_oClient.WSName
                v_oRow("TLName") = pv_oClient.TLNAME
                v_oRow("LoginTime") = pv_oClient.LoginTime
                v_oRow("Action") = pv_oClient.Action
                v_oRow("isActive") = v_intActive
                v_oRow("isNotActive") = v_intNotActive
                v_dt.Rows.Add(v_oRow)
            End If
            Return v_ds
        Catch ex As Exception
            Return Nothing
        Finally
            If Not v_ds Is Nothing Then
                v_ds.Dispose()
            End If
        End Try
    End Function

    Private Function GetActive(ByVal pv_bln As Boolean) As Integer
        Dim v_intCount As Integer = 0
        For i As Integer = 0 To mv_ClientList.Count - 1
            If mv_ClientList(i).isActive = pv_bln Then
                v_intCount += 1
            End If
        Next
        Return v_intCount
    End Function
#End Region

End Class