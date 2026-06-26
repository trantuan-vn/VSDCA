Imports System.Net
Imports System.Threading
Imports Microsoft.Win32
Imports Sats.CommonLibrary
Imports System.Runtime.InteropServices
Imports Sats.ClientCA

Public Structure SYSTEMTIME
    Public wYear As UInt16
    Public wMonth As UInt16
    Public wDayOfWeek As UInt16
    Public wDay As UInt16
    Public wHour As UInt16
    Public wMinute As UInt16
    Public wSecond As UInt16
    Public wMilliseconds As UInt16
End Structure

Public Enum BusLayerResult
    None = 0
    Success = 1
    ServiceFailure = 2
    UnknownFailure = 3
    ConnectionFailure = 4
    AuthenticationFailure = 5

    'myvq start 24/12/2010
    UnpluggedUSB = 6
    WrongCertificate = 7
    'myvq end 24/12/2010



End Enum

Public Class CBusLayer
    'Private Const c_wsTimeout As Integer = 300000000 '30000 seconds

    Private mv_strLanguage As String = String.Empty
    Private mv_strIpAddress As String = String.Empty
    Private mv_strWsName As String = String.Empty
    Private mv_strTicket As String = Nothing    

    Public CurrentTellerProfile As New CTellerProfile
    Public st As New SYSTEMTIME    

    Public mv_DataSet As DataSet

    <DllImport("kernel32.dll")> _
    Public Shared Sub GetLocalTime(ByRef lpSystemTime As SYSTEMTIME)
    End Sub

    <DllImport("kernel32.dll")> _
    Public Shared Sub SetLocalTime(ByRef lpSystemTime As SYSTEMTIME)
    End Sub

    Public Sub New()
        '1. Lấy thông số ngôn ngữ của ứng dụng từ Registry
        Try
            Dim v_strLang As String = String.Empty
            Dim v_regKey As RegistryKey = Registry.CurrentUser.OpenSubKey(gc_RegistryKey)

            If Not v_regKey Is Nothing Then
                v_strLang = CType(v_regKey.GetValue(gc_REG_LANG), String)

                Select Case v_strLang
                    Case CommonLibrary.gc_LANG_VIETNAMESE
                        mv_strLanguage = CommonLibrary.gc_LANG_VIETNAMESE
                    Case CommonLibrary.gc_LANG_ENGLISH
                        mv_strLanguage = CommonLibrary.gc_LANG_ENGLISH
                    Case Else
                        mv_strLanguage = CommonLibrary.gc_LANG_VIETNAMESE
                End Select
            Else
                mv_strLanguage = CommonLibrary.gc_LANG_VIETNAMESE
            End If
        Catch ex As Exception
            CommonLibrary.LogError.Write(ex.Message & vbCrLf & ex.StackTrace, EventLogEntryType.Error, gc_MODULE_CLIENT)
        End Try
        mv_strWsName = System.Net.Dns.GetHostName
        mv_strIpAddress = GetIPAddress()

        '2. Khởi tạo thông tin chung cho NSD
        CurrentTellerProfile.BranchId = "0000"
        CurrentTellerProfile.TellerId = "0000"
        CurrentTellerProfile.BranchName = String.Empty
        CurrentTellerProfile.Description = String.Empty
        CurrentTellerProfile.Password = String.Empty
        CurrentTellerProfile.TellerType = "0"
        CurrentTellerProfile.TellerGroup = "00"
        CurrentTellerProfile.TellerLevel = 0
        CurrentTellerProfile.TellerName = String.Empty
        CurrentTellerProfile.TellerPrinterName = String.Empty
        CurrentTellerProfile.TellerTitle = String.Empty
        CurrentTellerProfile.LoginTime = String.Empty
        'CurrentTellerProfile.IPAddress = mv_strIpAddress


        mv_DataSet = New DataSet
        mv_oLocal = New Client
        mv_oLocal.IPAddress = GetIPAddress()
        mv_oLocal.WSName = AppWsName
    End Sub

    Public Function ChangePassword(ByVal pv_strUserName As String, ByVal pv_strNewPassword As String) As Long
        Try
            'mv_wsAuth.ChangePassword(pv_strUserName, EncryptString(pv_strUserName, pv_strNewPassword))
            Return pv_oProxy.ChangePassword(pv_strUserName, EncryptString(pv_strUserName, pv_strNewPassword))
        Catch ex As Exception
            Return -1
        End Try
    End Function

    'Public Function Login1(ByVal pv_strUserName As String, ByVal pv_strPassword As String) As BusLayerResult
    '    'Lưu thông tin của NSD hiện thời
    '    CurrentTellerProfile.TellerName = pv_strUserName
    '    CurrentTellerProfile.Password = EncryptString(pv_strUserName, pv_strPassword)

    '    'Try to get a ticket
    '    Dim ticketResult As BusLayerResult = GetAuthorizationTicket()
    '    If ticketResult = BusLayerResult.Success Then
    '        Dim newTellerProfile As CTellerProfile

    '        'Lấy thông tin NSD
    '        Try
    '            newTellerProfile = mv_wsAuth.GetTellerProfile(mv_strTicket)
    '            If newTellerProfile Is Nothing Then
    '                Return BusLayerResult.AuthenticationFailure
    '            End If

    '            'Lưu lại thông tin NSD
    '            CurrentTellerProfile.BranchId = newTellerProfile.BranchId
    '            CurrentTellerProfile.BranchName = newTellerProfile.BranchName
    '            CurrentTellerProfile.Description = newTellerProfile.Description
    '            CurrentTellerProfile.TellerType = newTellerProfile.TellerType
    '            CurrentTellerProfile.TellerGroup = newTellerProfile.TellerGroup
    '            CurrentTellerProfile.TellerId = newTellerProfile.TellerId
    '            CurrentTellerProfile.TellerLevel = newTellerProfile.TellerLevel
    '            CurrentTellerProfile.TellerName = newTellerProfile.TellerName
    '            CurrentTellerProfile.TellerPrinterName = newTellerProfile.TellerPrinterName
    '            CurrentTellerProfile.TellerTitle = newTellerProfile.TellerTitle
    '            CurrentTellerProfile.BusDate = newTellerProfile.BusDate
    '            CurrentTellerProfile.Interval = newTellerProfile.Interval
    '            CurrentTellerProfile.LoginTime = newTellerProfile.LoginTime
    '            CurrentTellerProfile.MemberFilter = newTellerProfile.MemberFilter
    '            CurrentTellerProfile.StockFilter = newTellerProfile.StockFilter
    '            CurrentTellerProfile.AllMember = newTellerProfile.AllMember
    '            CurrentTellerProfile.AllStock = newTellerProfile.AllStock
    '            CurrentTellerProfile.MaxValue = newTellerProfile.MaxValue
    '            CurrentTellerProfile.PassDate = newTellerProfile.PassDate

    '            CurrentTellerProfile.VsdBrid = newTellerProfile.VsdBrid
    '            'GetLocalTime(st)

    '            'st.wHour = Convert.ToUInt16(Strings.Left(CurrentTellerProfile.LoginTime, 2))
    '            'st.wMinute = Convert.ToUInt16(Strings.Mid(CurrentTellerProfile.LoginTime, 4, 2))
    '            'st.wSecond = Convert.ToUInt16(Strings.Right(CurrentTellerProfile.LoginTime, 2))

    '            'SetLocalTime(st)
    '            'mv_DataSet.Tables.Clear()
    '            'mv_DataSet.Tables.Add(GetDataTable("RG015"))

    '            'Cap nhat thong tin user dang nhap vao he thong
    '            'Dim v_ws As New BDSChannel.BDSDelivery
    '            'Dim v_strObjMsg As String

    '            'v_strObjMsg = BuildXMLObjMsg(newTellerProfile.BusDate, newTellerProfile.BranchId, newTellerProfile.LoginTime, newTellerProfile.TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SY_AUTHENTICATION, gc_ActionInquiry, , Me.AppWsName & "|" & Me.AppIpAddress, "WriteSessionIn")
    '            'Dim v_lngErr As Long = v_ws.Message(v_strObjMsg)
    '            'If v_lngErr <> ERR_SYSTEM_OK Then
    '            '    Return BusLayerResult.UnknownFailure
    '            'End If

    '        Catch ex As Exception
    '            Return HandleException(ex)
    '        End Try
    '    End If

    '    Return ticketResult
    'End Function

    Public Sub BusSystemMessage(ByRef pv_strObjMsg As String)
        pv_oProxy.Message(pv_strObjMsg)
    End Sub

#Region "Properties"
    Public ReadOnly Property AppLanguage() As String
        Get
            Return mv_strLanguage
        End Get
    End Property

    Public ReadOnly Property AppIpAddress() As String
        Get
            Return mv_strIpAddress
        End Get
    End Property

    Public ReadOnly Property AppWsName() As String
        Get
            Return mv_strWsName
        End Get
    End Property
#End Region

#Region " Auth Service "
    'Private Function GetAuthorizationTicket() As BusLayerResult
    '    Try
    '        With CurrentTellerProfile
    '            mv_strTicket = mv_wsAuth.GetAuthorizationTicket(.TellerName, .Password)
    '        End With
    '    Catch ex As Exception
    '        mv_strTicket = Nothing
    '        Return HandleException(ex)
    '    End Try

    '    If mv_strTicket Is Nothing Then
    '        'Username/password failed
    '        Return BusLayerResult.AuthenticationFailure
    '    ElseIf mv_strTicket = "UserInUse" Then
    '        Return BusLayerResult.ServiceFailure
    '    End If

    '    Return BusLayerResult.Success
    'End Function
#End Region

#Region " Helper Functions "
    Private Function HandleException(ByVal ex As Exception) As BusLayerResult
        CommonLibrary.LogError.Write(ex.Message & vbNewLine & ex.StackTrace, EventLogEntryType.Error, gc_MODULE_CLIENT)

        If ex.GetType() Is GetType(WebException) Then
            Return BusLayerResult.ConnectionFailure
        Else
            Return BusLayerResult.UnknownFailure
        End If
    End Function

    Private Function GetIPAddress() As String
        Try
            Dim sHostName As String = System.Net.Dns.GetHostName()
            Dim ipE As System.Net.IPHostEntry = System.Net.Dns.GetHostByName(sHostName)
            Dim IpA() As System.Net.IPAddress = ipE.AddressList
            Dim sAddr As String

            sAddr = IpA(0).ToString

            Return sAddr
        Catch ex As Exception
            Throw ex
        End Try
    End Function
#End Region


#Region "WCF"
    Public Function Login(ByVal pv_strUserName As String, ByVal pv_strPassword As String, ByVal pv_strIPAddress As String, Optional ByVal pv_intCheckCA As Integer = 0) As BusLayerResult
        Try
            pv_oProxy = New BDSChannel.BDSDelivery
            pv_oProxy.Connect()
            Dim test As String
            CurrentTellerProfile.TellerName = pv_strUserName
            CurrentTellerProfile.Password = EncryptString(pv_strUserName, pv_strPassword)

            Dim ticketResult As String

            'If pv_intCheckCA = 10 Then
            'ticketResult = pv_oProxy.Login(pv_strUserName, CurrentTellerProfile.Password, pv_strIPAddress)
            'Else
            'rao tam de test ca
            'ticketResult = pv_oProxy.LoginCA(pv_strUserName, CurrentTellerProfile.Password, pv_strIPAddress)
            'End If

            ticketResult = pv_oProxy.LoginCA(pv_strUserName, CurrentTellerProfile.Password, pv_strIPAddress)

            ' pv_oProxy.SendAction(mv_oLocal)

            If ticketResult <> "" Then
                Dim newTellerProfile As WCFTellerProfile

                'Lấy thông tin NSD
                Try
                    If (ticketResult.StartsWith("CLIENT_ERROR_BKAV_")) Then
                        ClientBussinessCA.closeUSB()
                        Return BusLayerResult.UnpluggedUSB
                    End If
                    If (ticketResult.StartsWith("SERVER_ERROR_BKAV_") And Not ticketResult.StartsWith("SERVER_ERROR_BKAV_VSD_USER")) Then
                        ClientBussinessCA.closeUSB()
                        Return BusLayerResult.WrongCertificate
                    End If
                    newTellerProfile = pv_oProxy.GetTellerProfile(ticketResult)
                    If newTellerProfile Is Nothing Then
                        ClientBussinessCA.closeUSB()
                        Return BusLayerResult.AuthenticationFailure
                    End If

                    'Lưu lại thông tin NSD
                    CurrentTellerProfile.BranchId = newTellerProfile.BranchId
                    CurrentTellerProfile.BranchName = newTellerProfile.BranchName
                    CurrentTellerProfile.Description = newTellerProfile.Description
                    CurrentTellerProfile.TellerType = newTellerProfile.TellerType
                    CurrentTellerProfile.TellerGroup = newTellerProfile.TellerGroup
                    CurrentTellerProfile.TellerId = newTellerProfile.TellerID
                    CurrentTellerProfile.TellerLevel = newTellerProfile.TellerLevel
                    CurrentTellerProfile.TellerName = newTellerProfile.TellerName
                    CurrentTellerProfile.TellerPrinterName = newTellerProfile.TellerPrinterName
                    CurrentTellerProfile.TellerTitle = newTellerProfile.TellerTitle
                    CurrentTellerProfile.BusDate = newTellerProfile.BusDate
                    CurrentTellerProfile.Interval = newTellerProfile.Interval
                    CurrentTellerProfile.LoginTime = newTellerProfile.LoginTime
                    CurrentTellerProfile.MemberFilter = newTellerProfile.MemberFilter
                    CurrentTellerProfile.StockFilter = newTellerProfile.StockFilter
                    CurrentTellerProfile.AllMember = newTellerProfile.AllMember
                    CurrentTellerProfile.AllStock = newTellerProfile.AllStock
                    CurrentTellerProfile.MaxValue = newTellerProfile.MaxValue
                    CurrentTellerProfile.PassDate = newTellerProfile.PassDate

                    CurrentTellerProfile.VsdBrid = newTellerProfile.VsdBrid
                    CurrentTellerProfile.VsdBrid2 = newTellerProfile.VsdBrid2
                    mv_DataSet.Tables.Clear()
                    mv_oLocal.TLNAME = newTellerProfile.TellerName
                    pv_oProxy.UpdateInfoClient(mv_oLocal)

                Catch ex As Exception
                    Return HandleException(ex)
                End Try
                Return BusLayerResult.Success
            Else
                ClientBussinessCA.closeUSB()
                Return BusLayerResult.AuthenticationFailure
            End If
        Catch ex As Exception
            ClientBussinessCA.closeUSB()
            Return BusLayerResult.AuthenticationFailure
        Finally
        End Try
    End Function
#End Region
End Class
