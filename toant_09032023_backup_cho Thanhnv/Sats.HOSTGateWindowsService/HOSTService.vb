Imports System
Imports System.Runtime.Remoting
Imports System.Runtime.Remoting.Channels
Imports System.Runtime.Remoting.Channels.Tcp
Imports Sats.CommonLibrary

Public Class HOSTService
    Public Sub HOSTServiceStart()
        Dim v_intPort As Integer
        v_intPort = CInt(System.Configuration.ConfigurationManager.AppSettings("LocalPort"))

        'Hanm5 sua
        Dim props As IDictionary = New Hashtable
        props("port") = v_intPort
        props("clientConnectionLimit") = 30


        Dim v_srvProvider As BinaryServerFormatterSinkProvider = New BinaryServerFormatterSinkProvider()
        Dim v_clntProvider As BinaryClientFormatterSinkProvider = New BinaryClientFormatterSinkProvider()

        Dim v_Channel As New TcpChannel(props, v_clntProvider, v_srvProvider)
        'Hanm5 ket thuc sua

        'Dim v_Channel As New TcpChannel(v_intPort)

        ChannelServices.RegisterChannel(v_Channel, False)
        RemotingConfiguration.RegisterWellKnownServiceType(Type.GetType("Sats.HOSTWindowsService.HOSTDelivery"), "HOSTDelivery", WellKnownObjectMode.SingleCall)
    End Sub
End Class

Public Class UserUpdate

    Public Sub UserUpdateStart()
        Dim v_obj As New Host.SystemAdmin
        Try
            While True
                v_obj.UpdateUserNotUse()
                Threading.Thread.Sleep(60000)
            End While
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
End Class