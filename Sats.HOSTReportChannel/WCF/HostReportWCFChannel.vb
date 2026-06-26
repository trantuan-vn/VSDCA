Imports System
Imports System.ServiceModel
Imports BkavCASign
Imports Sats.HOSTReportChannel.HOSTReportService
Imports Sats.CommonLibrary

Public Class HostReportWCFChannel
    Implements IReportWCFCallback

    Public Event User_Join(ByVal pv_Client As String)
    Public Event User_Leave(ByVal pv_Client As String)
    Public Event Refresh_Clients(ByVal pv_Client() As String)
    Public Event ReceiveMessage(ByVal pv_oMessage As String)

    Private mv_oProxy As ReportWCFClient
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
    Public Sub Receive(ByVal pv_oMessage As String) Implements IReportWCFCallback.Receive

    End Sub

    Public Sub UserJoin(ByVal pv_oClient As Sats.CommonLibrary.Client) Implements IReportWCFCallback.UserJoin

    End Sub

    Public Sub UserLeave(ByVal pv_oClient As Sats.CommonLibrary.Client) Implements IReportWCFCallback.UserLeave

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
                mv_oProxy = New ReportWCFClient(v_oContent)

                v_strServer = mv_oProxy.Endpoint.Address.Uri.AbsoluteUri
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

    Public Function SendRpt(ByRef pv_strMessage As String, ByRef pv_arrByte() As Byte) As Long
        Try
            Return mv_oProxy.SendRpt(pv_strMessage, pv_arrByte)
        Catch ex As Exception
            Return ERR_SYSTEM_START
        End Try
    End Function
    Public Function SendExpRpt(ByRef pv_strMessage As String) As Long
        Try
            Return mv_oProxy.SendExpRpt(pv_strMessage)
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

    Public Function BeginReceive(ByVal pv_oMessage As String, ByVal callback As System.AsyncCallback, ByVal asyncState As Object) As System.IAsyncResult Implements IReportWCFCallback.BeginReceive

    End Function

    Public Function BeginUserJoin(ByVal pv_oClient As Sats.CommonLibrary.Client, ByVal callback As System.AsyncCallback, ByVal asyncState As Object) As System.IAsyncResult Implements IReportWCFCallback.BeginUserJoin

    End Function

    Public Function BeginUserLeave(ByVal pv_oClient As Sats.CommonLibrary.Client, ByVal callback As System.AsyncCallback, ByVal asyncState As Object) As System.IAsyncResult Implements IReportWCFCallback.BeginUserLeave

    End Function

    Public Sub EndReceive(ByVal result As System.IAsyncResult) Implements IReportWCFCallback.EndReceive

    End Sub

    Public Sub EndUserJoin(ByVal result As System.IAsyncResult) Implements IReportWCFCallback.EndUserJoin

    End Sub

    Public Sub EndUserLeave(ByVal result As System.IAsyncResult) Implements IReportWCFCallback.EndUserLeave

    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        Close()
    End Sub

    Public Function BeginReceiveAction(ByVal pv_oDataSet As System.Data.DataSet, ByVal callback As System.AsyncCallback, ByVal asyncState As Object) As System.IAsyncResult Implements IReportWCFCallback.BeginReceiveAction

    End Function

    Public Sub EndReceiveAction(ByVal result As System.IAsyncResult) Implements IReportWCFCallback.EndReceiveAction

    End Sub

    Public Sub ReceiveAction(ByVal pv_oDataSet As System.Data.DataSet) Implements IReportWCFCallback.ReceiveAction

    End Sub
End Class
