Imports System
Imports System.IO
Imports System.ServiceModel
Imports System.Collections.Generic
Imports System.Runtime.Serialization
Imports Sats.CommonLibrary
Imports System.EnterpriseServices
Imports Sats.ServerCA
Imports BkavCASign
Imports log4net
Imports System.Security.Cryptography


<ServiceBehavior(InstanceContextMode:=InstanceContextMode.Single, _
                 ConcurrencyMode:=ConcurrencyMode.Multiple, _
                 UseSynchronizationContext:=False)> _
Public Class HOSTWCFService
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

#Region "IWCF Member"
    Public Function Connect(ByVal pv_oClient As Client) As Boolean Implements IWCF.Connect
        If Not mv_lstClients.ContainsValue(CurrentCallback) Then
            SyncLock (mv_oSyncObj)
                mv_lstClients.Add(pv_oClient, CurrentCallback)
                mv_ClientList.Add(pv_oClient)
            End SyncLock
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub Disconnect(ByVal pv_oClient As Client) Implements IWCF.Disconnect
        For Each v_oClient As Client In mv_lstClients.Keys
            If v_oClient.IPAddress = pv_oClient.IPAddress Then
                SyncLock (mv_oSyncObj)
                    mv_lstClients.Remove(v_oClient)
                    mv_ClientList.Remove(v_oClient)
                End SyncLock
                Exit Sub
            End If
        Next
    End Sub

    Public Function Login(ByVal pv_strUserName As String, Optional ByVal pv_strPassword As String = "", Optional ByVal pv_strIPAddress As String = "") As String Implements IWCF.Login
        Dim v_obj As New Branch.Branch
        Try
            HOSTChanel.AppLogger.Info("Login:" & pv_strUserName)
            Return v_obj.GetAuthorizationTicket(pv_strUserName, pv_strPassword)
        Catch ex As Exception
            Return Nothing
        Finally
            If Not v_obj Is Nothing Then
                v_obj = Nothing
            End If
        End Try
    End Function

    Public Function LoginCA(ByVal pv_strEncryptedXML As String, _
        ByRef pv_strEncryptedSessionKey As String, ByRef pv_strResult As String) As String Implements IWCF.LoginCA
        'Dim v_obj As New Branch.Branch
        'Try
        '    Return v_obj.GetAuthorizationTicket(pv_strUserName, pv_strPassword)
        'Catch ex As Exception
        '    Return Nothing
        'Finally
        '    If Not v_obj Is Nothing Then
        '        v_obj = Nothing
        '    End If
        'End Try
        Return Nothing
    End Function
    Public Function FileCA(ByRef pv_strMessage As String, ByVal pv_intBrid As String, _
                            ByVal pv_strFileName As String, ByVal pv_strCurrDate As String) As String Implements IWCF.FileCA
        'Dim v_obj As New Branch.Branch
        'Try
        '    Return v_obj.GetAuthorizationTicket(pv_strUserName, pv_strPassword)
        'Catch ex As Exception
        '    Return Nothing
        'Finally
        '    If Not v_obj Is Nothing Then
        '        v_obj = Nothing
        '    End If
        'End Try
        Return Nothing
    End Function
    'bangpv: Chuyen tu BDS Len
    Public Function AfterLoginCA(ByRef pv_strEncryptedSessionKey As String, _
          ByRef pv_strResult As String, _
          ByVal pv_strUserName As String) As String Implements IWCF.AfterLoginCA
        Try
            Dim v_oCertServer As New CertificateServer
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

            ServerBussinessCA.AfterLogin(pv_strEncryptedSessionKey, _
                        pv_strResult, v_oCertServer, pv_strUserName, mv_oSignServer, v_strVSDUsername, _
                        v_strBKAVPassword, v_strLDAPIP, v_strArrayOgarnization, v_strBKAVDomainComponent)
            Return pv_strResult
        Catch ex As Exception
            Return ERR_SYSTEM_START
        End Try
    End Function

    Public Function Send(ByRef pv_arrByte() As Byte) As Long Implements IWCF.Send
        Dim v_strErrorSource As String = "HOSTDelivery.Message(Byte)", v_strErrorMessage As String = ""
        Dim v_strResultData As String = vbNullString
        Dim v_lngErr As Long
        Dim v_objSignCA As New ServerBussinessCA
        Dim v_strTLname As String
        Dim v_obj As Object = Nothing
        Try
            'Read transaction message 

            Dim pv_strMessage As String = Decompress(pv_arrByte)
            ''xử lý đối với trường hợp có ký CA
            'Dim v_xmlRootMessage As New XmlDocumentEx
            'v_xmlRootMessage.LoadXml(pv_strMessage)
            'Dim v_attrRColl As Xml.XmlAttributeCollection = v_xmlRootMessage.DocumentElement.Attributes
            'Dim v_strSIGNCA As String = CStr(CType(v_attrRColl.GetNamedItem("SignCA"), Xml.XmlAttribute).Value)
            'If v_strSIGNCA = "1" Then
            '    pv_strMessage = CStr(CType(v_attrRColl.GetNamedItem("EncryptedXML"), Xml.XmlAttribute).Value)
            '    v_strTLname = CStr(CType(v_attrRColl.GetNamedItem("TLName"), Xml.XmlAttribute).Value)
            '    pv_strMessage = v_objSignCA.DecryptXML(pv_strMessage, v_strTLname)
            'End If

            Dim v_xmlDocumentMessage As New XmlDocumentEx
            v_xmlDocumentMessage.LoadXml(pv_strMessage)

            'HaNM5: 08/12/2020
            HOSTChanel.AppLogger.Info(pv_strMessage)

            'Get header message.

            Dim v_attrColl As Xml.XmlAttributeCollection = v_xmlDocumentMessage.DocumentElement.Attributes
            Dim v_strLOCAL As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeLOCAL), Xml.XmlAttribute).Value)
            Dim v_strMSGTYPE As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeMSGTYPE), Xml.XmlAttribute).Value)


            Select Case v_strMSGTYPE
                Case gc_MsgTypeTrans
                    Dim v_strTXTYPE As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTXTYPE), Xml.XmlAttribute).Value)
                    If Trim(v_strTXTYPE) = "I" Then
                        'Inquiry message
                        v_obj = New Host.objRouter
                    Else
                        'Transaction message
                        v_obj = New Host.txRouter
                        v_lngErr = v_obj.Transact(v_xmlDocumentMessage, mv_oSignServer)
                    End If
                    pv_strMessage = v_xmlDocumentMessage.InnerXml
                Case gc_MsgTypeObj
                    'Object message
                    If v_strLOCAL = gc_IsLocalMsg Then
                        v_obj = New Branch.Branch
                        v_obj.objTransfer(pv_strMessage)
                    Else
                        v_obj = New Host.objRouter
                        v_lngErr = v_obj.Transfer(pv_strMessage)
                    End If
            End Select
            pv_arrByte = modCommond.Compression(pv_strMessage)

            Return v_lngErr
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                & "Error code: System error!" & vbNewLine _
                & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_HOST)
            Return ERR_SYSTEM_START
        Finally
            If Not v_obj Is Nothing Then
                v_obj.Dispose()
            End If
            GC.Collect()
            GC.GetTotalMemory(False)
            GC.Collect()
        End Try
    End Function


    Public Function SendCA(ByRef pv_arrByte() As Byte, ByRef v_arrByteCA() As Byte, ByRef v_strTLLOGIDCA As String, ByRef v_strVSDSignature As String) As Long Implements IWCF.SendCA
        Dim v_strErrorSource As String = "HOSTDelivery.Message(Byte)", v_strErrorMessage As String = ""
        Dim v_strResultData As String = vbNullString
        Dim v_lngErr As Long
        Dim v_strClientSignature As String = ""
        Dim v_strTLname As String
        Dim v_obj As Object = Nothing
        Dim v_strLocalDir, v_strFileName As String
        Dim v_ds As New DataSet

        Try
            'Read transaction message 

            Dim pv_strMessage As String = Decompress(pv_arrByte)
            ''xử lý đối với trường hợp có ký CA
            Dim v_xmlRootMessage As New XmlDocumentEx
            v_xmlRootMessage.LoadXml(pv_strMessage)
            Dim v_attrRColl As Xml.XmlAttributeCollection = v_xmlRootMessage.DocumentElement.Attributes
            'lấy xml mã hóa
            pv_strMessage = CStr(CType(v_attrRColl.GetNamedItem("EncryptedXML"), Xml.XmlAttribute).Value)
            'lưu chuỗi xml mã hóa đã lấy ra 
            Dim pv_strMessageSave = pv_strMessage
            'lấy tlname truyền lên 
            ' giai ma bang HSM
            v_strTLname = CStr(CType(v_attrRColl.GetNamedItem("TLName"), Xml.XmlAttribute).Value)
            'giải mã encrypted xml để lấy về xmd gốc 
            pv_strMessage = ServerBussinessCA.DecryptXML(pv_strMessage, v_strTLname, v_strClientSignature)
            'End If


            'xử lý giao dịch trên xml gốc
            Dim v_xmlDocumentMessage As New XmlDocumentEx
            v_xmlDocumentMessage.LoadXml(pv_strMessage)

            'HaNM5: 08/12/2020
            HOSTChanel.AppLogger.Info(pv_strMessage)

            'Get header message.

            Dim v_attrColl As Xml.XmlAttributeCollection = v_xmlDocumentMessage.DocumentElement.Attributes
            Dim v_strLOCAL As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeLOCAL), Xml.XmlAttribute).Value)
            Dim v_strMSGTYPE As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeMSGTYPE), Xml.XmlAttribute).Value)
            'Dim v_strTLTXCD As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTLTXCD), Xml.XmlAttribute).Value)
            'Dim v_strTXNUM As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTXNUM), Xml.XmlAttribute).Value)

            'Select Case v_strMSGTYPE
            '    Case gc_MsgTypeTrans
            Dim v_strTXTYPE As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTXTYPE), Xml.XmlAttribute).Value)
            If Trim(v_strTXTYPE) = "I" Then
                'Inquiry message
                v_obj = New Host.objRouter
            Else
                'Transaction message
                v_obj = New Host.txRouter
                v_lngErr = v_obj.Transact(v_xmlDocumentMessage, mv_oSignServer, v_ds, v_strTLLOGIDCA)
            End If
            pv_strMessage = v_xmlDocumentMessage.InnerXml
            'ký số trên string trả về: 
            'Lấy ra tltxcd của giao dịch cha để lưu file
            Dim v_oXMLDocument As New XmlDocumentEx
            Dim v_strData As String = v_ds.GetXml()
            'ghep status 
            Dim v_oXMLElement As Xml.XmlElement = v_oXMLDocument.CreateElement("DataXML")

            Dim v_strDATAXML = v_oXMLDocument.CreateAttribute("DATAXML1")
            v_strDATAXML.Value = v_strData
            v_oXMLElement.Attributes.Append(v_strDATAXML)

            Dim v_strStatusXML = v_oXMLDocument.CreateAttribute("STATUS")
            v_strStatusXML.Value = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeSTATUS), Xml.XmlAttribute).Value)
            v_oXMLElement.Attributes.Append(v_strStatusXML)

            'Dim v_strTLName = v_oXMLDocument.CreateAttribute("TLName")
            'v_strTLName.Value = pv_strTLNAME
            'v_oXMLElement.AppendChild(v_strTLName)

            'Dim v_strSignCA = v_oXMLDocument.CreateAttribute("SignCA")
            'v_strSignCA.Value = pv_strSignCA
            'v_oXMLElement.AppendChild(v_strSignCA)

            v_oXMLDocument.AppendChild(v_oXMLElement)
            v_strData = v_oXMLDocument.InnerXml

            v_strVSDSignature = ServerBussinessCA.CombineData(v_strData, mv_oSignServer)

            pv_arrByte = modCommond.Compression(pv_strMessage)
            v_arrByteCA = modCommond.Compression(v_strData)

            Return v_lngErr
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                & "Error code: System error!" & vbNewLine _
                & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_HOST)
            Return ERR_SYSTEM_START
        Finally
            If Not v_obj Is Nothing Then
                v_obj.Dispose()
            End If
            GC.Collect()
            GC.GetTotalMemory(False)
            GC.Collect()
        End Try
    End Function
    Public Function GetTellerProfile(ByVal pv_strticket As String) As CommonLibrary.WCFTellerProfile Implements CommonLibrary.IWCF.GetTellerProfile
        Dim v_obj As New Branch.Branch
        Try
            Dim v_strBranchId As String = String.Empty
            Dim v_strTellerId As String = String.Empty

            'Dim v_str As String = FormsAuthentication.Decrypt(ticket).Name
            Dim v_strArray() As String = pv_strticket.Split("|") 'v_str.Split("|")

            'If v_strArray.Length = 3 Then
            v_strBranchId = v_strArray(0)
            v_strTellerId = v_strArray(1)
            'End If
            Return v_obj.GetTellerProfile(v_strBranchId, v_strTellerId)
        Catch ex As Exception
            Return Nothing
        Finally
            If Not v_obj Is Nothing Then
                v_obj = Nothing
            End If
        End Try
    End Function
    Public Function GetServerRptExp(ByVal pv_strRptId As String, ByVal pv_strTLName As String, ByRef pv_intServerRptExp As Integer, ByVal pv_strSiCode As String, ByRef pv_strSiCodeAllow As String) As Long Implements IWCF.GetServerRptExp
        Dim v_obj As RP.Report
        Try
            v_obj = New RP.Report
            'Kiem tra xem bao cao can in phia client hay export phia server
            pv_intServerRptExp = v_obj.GetServerRptExport(pv_strRptId, pv_strTLName, pv_strSiCode, pv_strSiCodeAllow)
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                & "Error code: System error!" & vbNewLine _
                & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_HOST)
            Return ERR_SYSTEM_START
        Finally
            If Not v_obj Is Nothing Then
                v_obj.Dispose()
            End If
            GC.Collect()
            GC.GetTotalMemory(False)
            GC.Collect()
        End Try
    End Function
    Public Function SendRpt(ByRef pv_strMessage As String, ByRef pv_arrByte() As Byte) As Long Implements CommonLibrary.IWCF.SendRpt
        Dim v_strErrorSource As String = "HOSTDelivery.Message(Byte)", v_strErrorMessage As String = ""
        Dim v_strResultData As String = vbNullString
        Dim v_lngErr As Long
        Dim v_obj As RP.Report
        Dim v_ds As DataSet
        Try
            'Read transaction message 
            Dim v_xmlDocumentMessage As New XmlDocumentEx
            v_xmlDocumentMessage.LoadXml(pv_strMessage)

            'HaNM5: 08/12/2020
            HOSTChanel.AppLogger.Info(pv_strMessage)

            'Get header message.

            Dim v_attrColl As Xml.XmlAttributeCollection = v_xmlDocumentMessage.DocumentElement.Attributes
            Dim v_strLOCAL As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeLOCAL), Xml.XmlAttribute).Value)
            Dim v_strMSGTYPE As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeMSGTYPE), Xml.XmlAttribute).Value)
            Dim v_strReportDataKey As String = String.Empty
            v_obj = New RP.Report
            v_ds = v_obj.CreateReport(v_xmlDocumentMessage, v_strReportDataKey)
            'Compress message
            pv_arrByte = ZetaCompressionLibrary.CompressionHelper.CompressDataSet(v_ds)
            pv_strMessage = v_xmlDocumentMessage.InnerXml
            Return v_lngErr
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                & "Error code: System error!" & vbNewLine _
                & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_HOST)
            Return ERR_SYSTEM_START
        Finally
            If Not v_obj Is Nothing Then
                v_obj.Dispose()
            End If
            If Not v_ds Is Nothing Then
                v_ds.Dispose()
            End If
            GC.Collect()
            GC.GetTotalMemory(False)
            GC.Collect()
        End Try
    End Function
    Public Function SendExpRpt(ByVal pv_strMessage As String) As Long Implements CommonLibrary.IWCF.SendExpRpt
        Dim v_strErrorSource As String = "HOSTDelivery.Message(Byte)", v_strErrorMessage As String = ""
        Dim v_strResultData As String = vbNullString
        Dim v_lngErr As Long
        Dim v_obj As RP.Report
        Dim v_ds As DataSet
        'Dim v_strFileName As String
        Try
            'Read transaction message 
            Dim v_xmlDocumentMessage As New XmlDocumentEx
            v_xmlDocumentMessage.LoadXml(pv_strMessage)
            Dim v_strCAKey As String = ""
            Dim v_strReportDataKey As String = ""
            'Get header message.

            Dim v_attrColl As Xml.XmlAttributeCollection = v_xmlDocumentMessage.DocumentElement.Attributes
            Dim v_strLOCAL As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeLOCAL), Xml.XmlAttribute).Value)
            Dim v_strMSGTYPE As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeMSGTYPE), Xml.XmlAttribute).Value)
            Dim v_strTLName As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTLNAME), Xml.XmlAttribute).Value)

            Dim v_strClause As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCLAUSE), Xml.XmlAttribute).Value)

            'bangpv add 20230219 
            Dim v_strClause1 As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTXTIME), Xml.XmlAttribute).Value)
            ' End bangpv add 20230219 

            Dim v_arrTemp() As String
            Dim v_strSICODE As String = ""
            'Dim v_strFieldCode As String = ""
            v_arrTemp = v_strClause.Split("#")
            Dim v_strRptID = v_arrTemp(0)
            Dim v_arrField = v_arrTemp(1).Split("$")

            For l As Integer = 0 To v_arrField.Count - 2
                'v_strSQL = Replace(v_strSQL, "[!" & v_arrField(l).Split("|")(0) & "]", v_arrField(l).Split("|")(1))
                'bangpv: CA
                If v_arrField(l).Split("|")(2) = "SICODE" Then
                    v_strSICODE = v_arrField(l).Split("|")(3)
                End If
                'end bangpv
            Next

            v_obj = New RP.Report
            v_ds = v_obj.CreateReport(v_xmlDocumentMessage, v_strReportDataKey, v_strCAKey)
            If v_strReportDataKey <> String.Empty Then
                v_ds = CoreBusiness.objMaster.ReportDataSets(v_strReportDataKey)
            End If
            'Compress message
            'pv_arrByte = ZetaCompressionLibrary.CompressionHelper.CompressDataSet(v_ds)
            '20230219: add param to function SaveRPTFile 
            v_lngErr = v_obj.SaveRPTFile(v_ds, v_strTLName, v_strRptID, v_strSICODE, v_strCAKey, v_strClause1)
            '20230219: add param to function SaveRPTFile 
            Return v_lngErr
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                & "Error code: System error!" & vbNewLine _
                & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_HOST)
            Return ERR_SYSTEM_START
        Finally
            If Not v_obj Is Nothing Then
                v_obj.Dispose()
            End If
            If Not v_ds Is Nothing Then
                v_ds.Dispose()
            End If
            GC.Collect()
            GC.GetTotalMemory(False)
            GC.Collect()
        End Try
    End Function
    'BANGpv --09032023
    Function FetchRpt(ByVal pv_strRptDataKey As String, ByVal pv_intFrom As Integer, ByVal pv_intTo As Integer, ByRef pv_arrByte() As Byte) As Long Implements CommonLibrary.IWCF.FetchRpt
        Dim v_strErrorSource As String = "HOSTDelivery.Message(Byte)", v_strErrorMessage As String = ""
        Dim v_strResultData As String = vbNullString
        Dim v_lngErr As Long
        Dim v_obj As RP.Report
        Dim v_ds As DataSet
        Try
            v_obj = New RP.Report
            v_ds = v_obj.FetchRpt(pv_strRptDataKey, pv_intFrom, pv_intTo)
            'Compress message
            pv_arrByte = ZetaCompressionLibrary.CompressionHelper.CompressDataSet(v_ds)

            Return v_lngErr
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                & "Error code: System error!" & vbNewLine _
                & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_HOST)
            Return ERR_SYSTEM_START
        Finally
            If Not v_obj Is Nothing Then
                v_obj.Dispose()
            End If
            If Not v_ds Is Nothing Then
                v_ds.Dispose()
            End If
            GC.Collect()
            GC.GetTotalMemory(False)
            GC.Collect()
        End Try
    End Function

    Public Function SendRptCA(ByRef pv_strMessage As String, ByRef pv_arrByte() As Byte, ByRef v_strVSDSignature As String, ByRef v_strCAKey As String, ByRef pv_strDataHash As String) As Long Implements CommonLibrary.IWCF.SendRptCA
        Dim v_strErrorSource As String = "HOSTDelivery.Message(Byte)", v_strErrorMessage As String = ""
        Dim v_strResultData As String = vbNullString
        Dim v_strClientSignature As String
        Dim v_lngErr As Long
        Dim v_obj As RP.Report
        Dim v_ds As DataSet

        Try
            'Read transaction message 
            Dim v_xmlRootMessage As New XmlDocumentEx
            v_xmlRootMessage.LoadXml(pv_strMessage)
            Dim v_attrRColl As Xml.XmlAttributeCollection = v_xmlRootMessage.DocumentElement.Attributes
            'lấy xml mã hóa
            pv_strMessage = CStr(CType(v_attrRColl.GetNamedItem("EncryptedXML"), Xml.XmlAttribute).Value)
            'lưu chuỗi xml mã hóa đã lấy ra 
            Dim pv_strMessageSave = pv_strMessage
            'lấy tlname truyền lên 
            ' giai ma bang HSM
            Dim v_strTLname = CStr(CType(v_attrRColl.GetNamedItem("TLName"), Xml.XmlAttribute).Value)
            'giải mã encrypted xml để lấy về xmd gốc 
            pv_strMessage = ServerBussinessCA.DecryptXML(pv_strMessage, v_strTLname, v_strClientSignature)

            Dim v_xmlDocumentMessage As New XmlDocumentEx
            v_xmlDocumentMessage.LoadXml(pv_strMessage)

            'HaNM5: 08/12/2020
            HOSTChanel.AppLogger.Info(pv_strMessage)
            'Get header message.

            Dim v_attrColl As Xml.XmlAttributeCollection = v_xmlDocumentMessage.DocumentElement.Attributes
            Dim v_strLOCAL As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeLOCAL), Xml.XmlAttribute).Value)
            Dim v_strMSGTYPE As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeMSGTYPE), Xml.XmlAttribute).Value)
            Dim v_strReportDataKey As String = String.Empty

            v_obj = New RP.Report
            v_ds = v_obj.CreateReport(v_xmlDocumentMessage, v_strReportDataKey, v_strCAKey)

            'Compress message
            pv_arrByte = ZetaCompressionLibrary.CompressionHelper.CompressDataSet(v_ds)
            'Dim pv_strResult As String = ByteArrayToStr(pv_arrByte)
            'Dim pv_strResult As String = ZetaCompressionLibrary.CompressionHelper.DecompressString(pv_arrByte)

            'ký số trên string trả về: 
            'Dim v_strData As String = v_ds.GetXml
            'bangpv: sửa về ký và lưu trên StringB64
            'Dim v_strData As String
            'If v_strReportDataKey <> String.Empty Then
            '    Dim v_arrByteTmp() As Byte = ZetaCompressionLibrary.CompressionHelper.CompressDataSet(CoreBusiness.objMaster.ReportDataSets(v_strReportDataKey))
            '    v_strData = Convert.ToBase64String(v_arrByteTmp)
            'Else
            '    v_strData = Convert.ToBase64String(pv_arrByte)
            'End If

            'v_strVSDSignature = ServerBussinessCA.CombineData(v_strData, mv_oSignServer)

            '05072021: HaNM5 sua ve ky tren chuoi Hash
            Dim v_sha = New SHA1CryptoServiceProvider
            If v_strReportDataKey <> String.Empty Then
                Dim v_arrByteTmp() As Byte = ZetaCompressionLibrary.CompressionHelper.CompressDataSet(CoreBusiness.objMaster.ReportDataSets(v_strReportDataKey))
                pv_strDataHash = Convert.ToBase64String(v_sha.ComputeHash(v_arrByteTmp))
            Else
                pv_strDataHash = Convert.ToBase64String(v_sha.ComputeHash(pv_arrByte))
            End If
            v_strVSDSignature = ServerBussinessCA.CombineData(pv_strDataHash, mv_oSignServer)

            'pv_strMessage = v_strCAKey
            pv_strMessage = v_xmlDocumentMessage.InnerXml
            Return v_lngErr
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                & "Error code: System error!" & vbNewLine _
                & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_HOST)
            Return ERR_SYSTEM_START
        Finally
            If Not v_obj Is Nothing Then
                v_obj.Dispose()
            End If
            If Not v_ds Is Nothing Then
                v_ds.Dispose()
            End If
            GC.Collect()
            GC.GetTotalMemory(False)
            GC.Collect()
        End Try
    End Function

    Public Function SaveFileRptCA(ByVal v_arrByte() As Byte, ByVal v_strVSDSignature As String, ByVal v_strClientVSDSignature As String, ByVal v_strTLName As String, ByVal v_strRptId As String, ByRef v_strLocalDir As String, ByRef v_strFileName As String, ByVal v_strStatus As String) As Long Implements CommonLibrary.IWCF.SaveFileRptCA
        Dim v_strErrorSource As String = "HOSTDelivery.Message(Byte)", v_strErrorMessage As String = ""
        Dim lngErr As Long = ERR_SYSTEM_START
        Dim v_obj As Object = Nothing

        Try
            'HaNM5: 08/12/2020
            HOSTChanel.AppLogger.Info("SaveFileRptCA")

            Dim v_strDS As String = Decompress(v_arrByte)
            v_obj = New Host.SystemAdmin
            Dim XMLDocumentMessage As New Xml.XmlDocument
            Dim dataElement As Xml.XmlElement
            Dim v_attrData, v_attrVSDSignature, v_attrClientSignature As Xml.XmlAttribute
            dataElement = XMLDocumentMessage.CreateElement("RPData")

            v_attrData = XMLDocumentMessage.CreateAttribute("DATA")
            v_attrData.Value = v_strDS 'ByteArrayToStr(v_arrByte)
            dataElement.Attributes.Append(v_attrData)

            v_attrVSDSignature = XMLDocumentMessage.CreateAttribute(gc_AtributeSignatureVSD)
            v_attrVSDSignature.Value = v_strVSDSignature
            dataElement.Attributes.Append(v_attrVSDSignature)

            v_attrClientSignature = XMLDocumentMessage.CreateAttribute(gc_AtributeSignatureClient)
            v_attrClientSignature.Value = v_strClientVSDSignature
            dataElement.Attributes.Append(v_attrClientSignature)

            XMLDocumentMessage.AppendChild(dataElement)
            'Dim pv_strMessageSave As String = "<DATA>" & ByteArrayToStr(v_arrByte) & "</DATA>" _
            '                    & "<" & gc_AtributeSignatureVSD & ">" & v_strVSDSignature & "<" & gc_AtributeSignatureVSD & ">" _
            '                    & "<" & gc_AtributeSignatureClient & ">" & v_strClientVSDSignature & "<" & gc_AtributeSignatureClient & ">"
            lngErr = v_obj.SaveFileCA(XMLDocumentMessage.InnerXml, v_strTLName, v_strRptId, v_strLocalDir, v_strFileName, v_strStatus)

            Return lngErr
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                & "Error code: System error!" & vbNewLine _
                & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_HOST)
            Return lngErr
        Finally
            GC.Collect()
            GC.GetTotalMemory(False)
            GC.Collect()
        End Try
    End Function

    Public Function SendAction(ByVal pv_oClient As CommonLibrary.Client) As Long Implements CommonLibrary.IWCF.SendAction

    End Function

    Public Sub SynSend(ByRef pv_arrByte() As Byte) Implements IWCF.SynSend

    End Sub
#End Region

    Public Function ChangePassword(ByVal pv_strUserName As String, ByVal pv_strNewPassword As String) As Long Implements CommonLibrary.IWCF.ChangePassword

    End Function
End Class