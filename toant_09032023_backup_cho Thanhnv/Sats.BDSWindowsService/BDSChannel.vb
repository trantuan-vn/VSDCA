Imports Sats.ServerCA
Imports Sats.CommonLibrary

Public Class BDSChannel
    Private mv_oWCF As BDSCallback

    Protected Overrides Sub OnStart(ByVal args() As String)
        ' Add code here to start your service. This method should set things
        ' in motion so your service can do its work.
        'Dim v_bds As New BDSService
        'v_bds.BDSServiceStart()
        'Threading.Thread.Sleep(30000)
        Try
            mv_oWCF = New BDSCallback
            mv_oWCF.CallbackStart()

            Dim v_intSlotId = Integer.Parse(System.Configuration.ConfigurationManager.AppSettings("HSMSlotId"))
            Dim v_strPIN = System.Configuration.ConfigurationManager.AppSettings("HSMPin")
            Dim v_strPublicKeyName = System.Configuration.ConfigurationManager.AppSettings("HSMPublicKeyName")
            Dim v_strPrivateKeyName = System.Configuration.ConfigurationManager.AppSettings("HSMPrivateKeyName")
            Dim v_strCertificateName = System.Configuration.ConfigurationManager.AppSettings("HSMCertificateName")
            '2014111 bangpv: them config duong dan cryptoki.dll 
            Dim v_strHsmDllName = System.Configuration.ConfigurationManager.AppSettings("HSMDllName")

            ServerBussinessCA.OpenHSM(mv_oSignServer, v_intSlotId, v_strPIN, _
                                      v_strPublicKeyName, v_strPrivateKeyName, _
                                      v_strCertificateName, v_strHsmDllName)
            'end bangpv 
        Catch ex As Exception
            LogError.Write("Start service - Error source: " & ex.Source & vbNewLine _
                         & "Error code: System error!" & vbNewLine _
                         & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_HOST)
        End Try
    End Sub

    Protected Overrides Sub OnStop()
        ' Add code here to perform any tear-down necessary to stop your service.
        'hPrivateKey.Clear()
        'hPrivateKey = Nothing
        'hPublicKey.Clear()
        'hPublicKey = Nothing

        If Not mv_oWCF Is Nothing Then
            mv_oWCF.CallbackStop()
            mv_oWCF = Nothing
        End If
    End Sub

End Class
