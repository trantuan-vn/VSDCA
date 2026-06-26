Imports System
Imports System.ServiceModel
Imports Sats.CommonLibrary
Imports Sats.HOSTChannel.HOSTService
Imports Sats.ServerCA
Imports BkavCASign

Public Class HostWCFChannel
    Implements HOSTService.IWCFCallback

    Public Event User_Join(ByVal pv_Client As String)
    Public Event User_Leave(ByVal pv_Client As String)
    Public Event Refresh_Clients(ByVal pv_Client() As String)
    Public Event ReceiveMessage(ByVal pv_oMessage As String)

    Private mv_oProxy As WCFClient
    Private mv_blnIsConnected As Boolean = False
    Private mv_objInstanceContext As Object

    Public ReadOnly Property IsConnected() As Boolean
        Get
            Return mv_blnIsConnected
        End Get
    End Property

    Public ReadOnly Property Stat() As CommunicationState
        Get
            Return mv_oProxy.State
        End Get
    End Property

#Region "IWCFCallback Member"
    Public Sub Receive(ByVal pv_oMessage As String) Implements HOSTService.IWCFCallback.Receive

    End Sub

    Public Sub UserJoin(ByVal pv_oClient As CommonLibrary.Client) Implements HOSTService.IWCFCallback.UserJoin

    End Sub

    Public Sub UserLeave(ByVal pv_oClient As CommonLibrary.Client) Implements HOSTService.IWCFCallback.UserLeave

    End Sub
#End Region

#Region "Client function"
    Public Sub New()

    End Sub

    Public Sub Connect()
        Dim v_strServer As String
        Try
            If mv_oProxy Is Nothing Then
                Dim v_oContent As New InstanceContext(Me)
                mv_oProxy = New WCFClient(v_oContent)

                v_strServer = mv_oProxy.Endpoint.Address.Uri.AbsoluteUri
                'mv_oProxy.Endpoint.Address = New EndpointAddress("net.tcp://" & v_strServer & ":" & v_strserviceListenPort & v_strServicePath)
                mv_oProxy.Endpoint.Address = New EndpointAddress(v_strServer)
                mv_oProxy.Open()
                mv_blnIsConnected = True
            Else
                mv_blnIsConnected = False
            End If
        Catch ex As Exception
            mv_blnIsConnected = False
            LogError.Write("Error connect to: " & v_strServer & vbCrLf & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
        End Try
    End Sub

    Public Function Send(ByRef pv_arrByte() As Byte) As Long
        Try
            Return mv_oProxy.Send(pv_arrByte)
        Catch ex As Exception
            Return ERR_SYSTEM_START
        End Try
    End Function
    Public Function SendCA(ByRef pv_arrByte() As Byte, ByRef v_arrByteCA() As Byte, ByRef v_strTLLOGIDCA As String, ByRef v_strVSDSignature As String) As Long
        Try
            Return mv_oProxy.SendCA(pv_arrByte, v_arrByteCA, v_strTLLOGIDCA, v_strVSDSignature)
        Catch ex As Exception
            Return ERR_SYSTEM_START
        End Try
    End Function

    Public Function SendRpt(ByRef pv_strMessage As String, ByRef pv_arrByte() As Byte) As Long
        Try
            Return mv_oProxy.SendRpt(pv_strMessage, pv_arrByte)
        Catch ex As Exception
            Return ERR_SYSTEM_START
        End Try
    End Function

    Public Function SendExpRpt(ByVal pv_strMessage As String) As Long
        Try
            Return mv_oProxy.SendExpRpt(pv_strMessage)
        Catch ex As Exception
            Return ERR_SYSTEM_START
        End Try
    End Function

    Public Function GetServerRptExp(ByVal pv_strRptId As String, ByVal pv_strTLName As String, ByRef pv_intServerRptExp As Integer, ByVal pv_strSiCode As String, ByRef pv_strSiCodeAllow As String) As Long
        Try
            Return mv_oProxy.GetServerRptExp(pv_strRptId, pv_strTLName, pv_intServerRptExp, pv_strSiCode, pv_strSiCodeAllow)
        Catch ex As Exception
            Return ERR_SYSTEM_START
        End Try
    End Function


    Public Function FetchRpt(ByVal pv_strRptDataKey As String, ByVal pv_intFrom As Integer, ByVal pv_intTo As Integer, ByRef pv_arrByte() As Byte) As Long
        Try
            Return mv_oProxy.FetchRpt(pv_strRptDataKey, pv_intFrom, pv_intTo, pv_arrByte)
        Catch ex As Exception
            Return ERR_SYSTEM_START
        End Try
    End Function

    Public Function SendRptCA(ByRef pv_strMessage As String, ByRef pv_arrByte() As Byte, ByRef v_strVSDSignature As String, ByRef v_strCAKey As String, ByRef pv_strDataHash As String) As Long
        Try
            Return mv_oProxy.SendRptCA(pv_strMessage, pv_arrByte, v_strVSDSignature, v_strCAKey, pv_strDataHash)
        Catch ex As Exception
            Return ERR_SYSTEM_START
        End Try
    End Function
    'bangpv
    Public Function SaveFileRptCA(ByVal v_arrByte() As Byte, ByVal v_strVSDSignature As String, ByVal v_strClientVSDSignature As String, ByVal v_strTLName As String, ByVal v_strRptId As String, Optional ByRef v_strLocalDir As String = "", Optional ByRef v_strFileName As String = "", Optional ByVal v_strStatus As String = "") As Long
        Try
            Return mv_oProxy.SaveFileRptCA(v_arrByte, v_strVSDSignature, v_strClientVSDSignature, v_strTLName, v_strRptId, v_strLocalDir, v_strFileName, v_strStatus)
        Catch ex As Exception
            Return ERR_SYSTEM_START
        End Try
    End Function
    'bangpv: Sua after login o bds chuyển lên host
    'Public Function AfterLoginCA(ByRef pv_strEncryptedSessionKey As String, _
    '        ByRef pv_strResult As String, ByVal pv_oCertServer As CertificateServer, _
    '        ByVal pv_strUserName As String, ByVal pv_oSignServer As SignServer) As String
    '    Try
    '        Return mv_oProxy.AfterLoginCA(pv_strEncryptedSessionKey, pv_strResult, pv_oCertServer, pv_strUserName, pv_oSignServer)
    '    Catch ex As Exception
    '        Return ERR_SYSTEM_START
    '    End Try
    'End Function

    Public Function Login(ByVal pv_strUserName As String, ByVal pv_strPassword As String, ByVal pv_strIPAddress As String) As String
        Try
            Return mv_oProxy.Login(pv_strUserName, pv_strPassword, pv_strIPAddress)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Public Function AfterLoginCA(ByRef pv_strEncryptedSessionKey As String, _
        ByRef pv_strResult As String, _
        ByVal pv_strUserName As String) As String
        Try
            Return mv_oProxy.AfterLoginCA(pv_strEncryptedSessionKey, pv_strResult, pv_strUserName)
        Catch ex As Exception
            Return ERR_SYSTEM_START
        End Try
    End Function

    Public Function GetTellerProfile(ByVal pv_strTiket As String) As WCFTellerProfile
        Try
            Return mv_oProxy.GetTellerProfile(pv_strTiket)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Sub Close()
        Try
            If mv_oProxy.State = CommunicationState.Opened Then
                mv_oProxy.Close()
                mv_oProxy = Nothing
            End If
        Catch ex As Exception

        End Try
    End Sub
#End Region

    Public Function BeginReceive(ByVal pv_oMessage As String, ByVal callback As System.AsyncCallback, ByVal asyncState As Object) As System.IAsyncResult Implements HOSTService.IWCFCallback.BeginReceive

    End Function

    Public Function BeginUserJoin(ByVal pv_oClient As CommonLibrary.Client, ByVal callback As System.AsyncCallback, ByVal asyncState As Object) As System.IAsyncResult Implements HOSTService.IWCFCallback.BeginUserJoin

    End Function

    Public Function BeginUserLeave(ByVal pv_oClient As CommonLibrary.Client, ByVal callback As System.AsyncCallback, ByVal asyncState As Object) As System.IAsyncResult Implements HOSTService.IWCFCallback.BeginUserLeave

    End Function

    Public Sub EndReceive(ByVal result As System.IAsyncResult) Implements HOSTService.IWCFCallback.EndReceive

    End Sub

    Public Sub EndUserJoin(ByVal result As System.IAsyncResult) Implements HOSTService.IWCFCallback.EndUserJoin

    End Sub

    Public Sub EndUserLeave(ByVal result As System.IAsyncResult) Implements HOSTService.IWCFCallback.EndUserLeave

    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        Close()
    End Sub

    Public Function BeginReceiveAction(ByVal pv_oDataSet As System.Data.DataSet, ByVal callback As System.AsyncCallback, ByVal asyncState As Object) As System.IAsyncResult Implements HOSTService.IWCFCallback.BeginReceiveAction

    End Function

    Public Sub EndReceiveAction(ByVal result As System.IAsyncResult) Implements HOSTService.IWCFCallback.EndReceiveAction

    End Sub

    Public Sub ReceiveAction(ByVal pv_oDataSet As System.Data.DataSet) Implements HOSTService.IWCFCallback.ReceiveAction

    End Sub
End Class
