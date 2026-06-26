Imports System
Imports System.Collections
Imports System.Runtime.Remoting
Imports System.Runtime.Remoting.Channels
Imports System.Runtime.Remoting.Channels.Tcp
'Imports System.Runtime.Remoting.Channels.Http

Imports Sats.CommonLibrary

Public Class BDSService
    Public Sub BDSServiceStart()
        Try
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
            RemotingConfiguration.RegisterWellKnownServiceType(Type.GetType("Sats.BDSWindowsService.AuthService"), "AuthService", WellKnownObjectMode.SingleCall)
            RemotingConfiguration.RegisterWellKnownServiceType(Type.GetType("Sats.BDSWindowsService.BDSDelivery"), "BDSDelivery", WellKnownObjectMode.SingleCall)
            'Tao bang luu giu PrivateKey cua user
            hPrivateKey = New Hashtable
            hPublicKey = New Hashtable
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
End Class
