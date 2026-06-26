Imports System
Imports System.Data
Imports System.Data.SqlTypes
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.ServiceModel
Imports System.ServiceModel.Description
Imports System.ServiceModel.Channels
Imports Sats.CommonLibrary

Public Class HOSTCallback
    Private mv_oHost As ServiceHost

    Public Sub CallbackStart()
        Try
            Dim v_intPort As Integer = System.Configuration.ConfigurationManager.AppSettings("LocalPort")
            Dim v_tcpAdr As New Uri("net.tcp://localhost:" & v_intPort & "/")
            Dim v_httpAdr As New Uri("http://localhost:" & v_intPort - 1 & "/")
            Dim v_baseAddress() As Uri = {v_tcpAdr, v_httpAdr}

            mv_oHost = New ServiceHost(GetType(HOSTWCFService), v_baseAddress)
            Dim tcpBinding As New NetTcpBinding("tcpBinding")

            Dim v_throttle As ServiceThrottlingBehavior
            v_throttle = mv_oHost.Description.Behaviors.Find(Of ServiceThrottlingBehavior)()
            If v_throttle Is Nothing Then
                v_throttle = New ServiceThrottlingBehavior()
                v_throttle.MaxConcurrentCalls = 1200
                v_throttle.MaxConcurrentSessions = 1200
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

            'ThoNM them sua MaxPendingChannels
            For Each ep In mv_oHost.Description.Endpoints
                Dim v_oElements As BindingElementCollection = ep.Binding.CreateBindingElements()
                Dim v_oReliableSessionElement As ReliableSessionBindingElement = v_oElements.Find(Of ReliableSessionBindingElement)()
                If Not v_oReliableSessionElement Is Nothing Then
                    v_oReliableSessionElement.MaxPendingChannels = 512
                    Dim v_oNewBinding As New CustomBinding(v_oElements)

                    v_oNewBinding.CloseTimeout = ep.Binding.CloseTimeout
                    v_oNewBinding.OpenTimeout = ep.Binding.OpenTimeout
                    v_oNewBinding.ReceiveTimeout = ep.Binding.ReceiveTimeout
                    v_oNewBinding.SendTimeout = ep.Binding.SendTimeout
                    v_oNewBinding.Name = ep.Binding.Name
                    v_oNewBinding.Namespace = ep.Binding.Namespace

                    ep.Binding = v_oNewBinding
                End If
            Next
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
        Catch ex As Exception

        End Try
    End Sub
End Class
