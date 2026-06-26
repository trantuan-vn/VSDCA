Imports Sats.CommonLibrary

Public Class HOSTDelivery
    Private mv_obj As IReomoteLib

    Public Sub New()
        Try
            Dim v_intPort As Integer
            Dim v_strServer As String
            Dim v_strChannel As String

            v_intPort = System.Configuration.ConfigurationManager.AppSettings("RemotePort")
            v_strServer = System.Configuration.ConfigurationManager.AppSettings("RemoteServer")
            v_strChannel = System.Configuration.ConfigurationManager.AppSettings("RemoteChannel")

            If v_strServer = "" Then v_strServer = "localhost"
            If v_intPort = 0 Then v_intPort = 8200
            If v_strChannel = "" Then v_strChannel = "tcp"

            mv_obj = CType(Activator.GetObject(GetType(IReomoteLib), v_strChannel & "://" & v_strServer & ":" & v_intPort & "/HOSTDelivery"), IReomoteLib)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function Message(ByRef pv_strMessage As String) As Long
        Return mv_obj.Message(pv_strMessage)
    End Function

    Public Function Message(ByRef pv_arrByteMessage() As Byte) As Long
        Return mv_obj.Message(pv_arrByteMessage)
    End Function

    Public Function GetAuthorizationTicket(ByVal pv_strUserName As String, ByVal pv_strPassword As String) As String
        Return mv_obj.GetAuthorizationTicket(pv_strUserName, pv_strPassword)
    End Function

    Public Function GetTellerProfile(ByVal ticket As String) As CTellerProfile
        Return mv_obj.GetTellerProfile(ticket)
    End Function

    Public Function RptMessage(ByRef pv_arrByteMessage() As Byte) As Long
        Return mv_obj.RptMessage(pv_arrByteMessage)
    End Function

    Public Function LoadInterface(ByRef pv_arrByteMessage() As Byte) As Long
        Return mv_obj.LoadInterface(pv_arrByteMessage)
    End Function
End Class
