Imports System
Imports System.ServiceModel
Imports System.ServiceModel.Description
Imports System.ServiceModel.Channels
Imports Sats.Interface4Gate
Imports Sats.CommonLibrary

Public Class HOSTCallback
    Private mv_oHost As ServiceHost
    Private mv_oGate As ServiceHost

    Public Sub CallbackStart()
        Try
            ''Mo cong core
            'Dim v_baseUri As New Uri(System.Configuration.ConfigurationManager.AppSettings("LocalServiceUri"))
            'Dim v_binding As Binding
            'Dim v_serviceUri = New Uri(v_baseUri, "HOST")

            'v_binding = Sats.Utils.CommonUtils.CreateTcpBinding(True)

            'Dim v_throttle As New ServiceThrottlingBehavior()
            'v_throttle.MaxConcurrentCalls = Integer.MaxValue
            'v_throttle.MaxConcurrentInstances = Integer.MaxValue
            'v_throttle.MaxConcurrentSessions = Integer.MaxValue

            'mv_oHost = New ServiceHost(GetType(HOSTWCFService))
            'Dim v_debug = mv_oHost.Description.Behaviors.Find(Of ServiceDebugBehavior)()
            'If Not v_debug Is Nothing Then
            '    v_debug.IncludeExceptionDetailInFaults = True
            'End If
            'mv_oHost.Description.Behaviors.Add(v_throttle)
            'mv_oHost.AddServiceEndpoint(GetType(IWCF), v_binding, v_serviceUri)
            'mv_oHost.Open()

            'Mo cong gate
            Dim v_baseGateUri As New Uri(System.Configuration.ConfigurationManager.AppSettings("GateServiceUri"))
            Dim v_gateBinding As Binding
            Dim v_gateServiceUri = New Uri(v_baseGateUri, "GATE")

            v_gateBinding = Sats.Utils.CommonUtils.CreateTcpBinding(True)

            Dim v_gateThrottle As New ServiceThrottlingBehavior()
            v_gateThrottle.MaxConcurrentCalls = Integer.MaxValue
            v_gateThrottle.MaxConcurrentInstances = Integer.MaxValue
            v_gateThrottle.MaxConcurrentSessions = Integer.MaxValue

            mv_oGate = New ServiceHost(GetType(GateController))
            Dim v_gateDebug = mv_oGate.Description.Behaviors.Find(Of ServiceDebugBehavior)()
            If Not v_gateDebug Is Nothing Then
                v_gateDebug.IncludeExceptionDetailInFaults = True
            End If
            mv_oGate.Description.Behaviors.Add(v_gateThrottle)
            mv_oGate.AddServiceEndpoint(GetType(IWCF4Gate), v_gateBinding, v_gateServiceUri)
            mv_oGate.Open()


            'Mo cong core
            Dim v_intPort As Integer = System.Configuration.ConfigurationManager.AppSettings("LocalPort")
            Dim v_tcpAdr As New Uri("net.tcp://localhost:" & v_intPort & "/")
            Dim v_httpAdr As New Uri("http://localhost:" & v_intPort - 1 & "/")
            Dim v_baseAddress() As Uri = {v_tcpAdr}

            mv_oHost = New ServiceHost(GetType(HOSTWCFService), v_baseAddress)

            Dim tcpBinding As Binding = Sats.Utils.CommonUtils.CreateTcpBinding(True) 'New NetTcpBinding("tcpBinding")

            Dim v_throttle As ServiceThrottlingBehavior
            v_throttle = mv_oHost.Description.Behaviors.Find(Of ServiceThrottlingBehavior)()
            If v_throttle Is Nothing Then
                v_throttle = New ServiceThrottlingBehavior()
                v_throttle.MaxConcurrentCalls = 300
                v_throttle.MaxConcurrentSessions = 300
                mv_oHost.Description.Behaviors.Add(v_throttle)
            End If
            'tcpBinding.ReceiveTimeout = New TimeSpan(20, 0, 0)
            'tcpBinding.ReliableSession.Enabled = True
            'tcpBinding.ReliableSession.InactivityTimeout = New TimeSpan(20, 0, 10)

            mv_oHost.AddServiceEndpoint(GetType(IWCF), tcpBinding, "host")

            Dim v_Behavior = New ServiceMetadataBehavior()
            mv_oHost.Description.Behaviors.Add(v_Behavior)

            mv_oHost.AddServiceEndpoint(GetType(IMetadataExchange), _
                                        MetadataExchangeBindings.CreateMexTcpBinding(), _
                                        "net.tcp://localhost:" & v_intPort - 2 & "/mex")

            ''ThoNM them sua MaxPendingChannels
            'For Each ep In mv_oHost.Description.Endpoints
            '    Dim v_oElements As BindingElementCollection = ep.Binding.CreateBindingElements()
            '    Dim v_oReliableSessionElement As ReliableSessionBindingElement = v_oElements.Find(Of ReliableSessionBindingElement)()
            '    If Not v_oReliableSessionElement Is Nothing Then
            '        v_oReliableSessionElement.MaxPendingChannels = 128
            '        Dim v_oNewBinding As New CustomBinding(v_oElements)

            '        v_oNewBinding.CloseTimeout = ep.Binding.CloseTimeout
            '        v_oNewBinding.OpenTimeout = ep.Binding.OpenTimeout
            '        v_oNewBinding.ReceiveTimeout = ep.Binding.ReceiveTimeout
            '        v_oNewBinding.SendTimeout = ep.Binding.SendTimeout
            '        v_oNewBinding.Name = ep.Binding.Name
            '        v_oNewBinding.Namespace = ep.Binding.Namespace

            '        ep.Binding = v_oNewBinding
            '    End If
            'Next
            'End ThoNM
            mv_oHost.Open()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub CallbackStop()
        Try
            If Not mv_oHost Is Nothing Then
                mv_oHost.Close()
                mv_oHost = Nothing
            End If

            If Not mv_oGate Is Nothing Then
                mv_oGate.Close()
                mv_oGate = Nothing
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class
