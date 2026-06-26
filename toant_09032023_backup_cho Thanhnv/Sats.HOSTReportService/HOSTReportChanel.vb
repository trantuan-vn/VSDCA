Imports System.ComponentModel
Imports Sats.ServerCA
Imports Sats.CommonLibrary
Imports System.Reflection
Imports log4net
Imports log4net.Config

Public Class HOSTReportChanel
    Private WithEvents v_wb As BackgroundWorker

    Private v_oWCF As HOSTCallback
    Public Shared ReadOnly AppLogger As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Protected Overrides Sub OnStart(ByVal args() As String)
        ' Add code here to start your service. This method should set things
        ' in motion so your service can do its work.
        'Dim v_host As New HOSTService
        'v_host.HOSTServiceStart()
        'v_wb = New BackgroundWorker
        'v_wb.RunWorkerAsync()
        ' Threading.Thread.Sleep(10000)
        Try
            'XmlConfigurator.Configure(New System.IO.FileInfo("log4net.config"))
            AppLogger.Info("Service started....")
            v_oWCF = New HOSTCallback
            v_oWCF.CallbackStart()

            'Lay tham so database
            Dim asb As Assembly
            Dim assamblyName As AssemblyName = AssemblyName.GetAssemblyName(AppDomain.CurrentDomain.BaseDirectory & "Sats.DBConfig.dll")
            Dim myDomain As AppDomain = AppDomain.CreateDomain("HOST")
            asb = myDomain.Load(assamblyName)
            Dim dbConfig As IDBConfig = CType(asb.CreateInstance("Sats.DBConfig.DBConfig"), IDBConfig)
            GlobalDataConfig.HOST_DBCONFIG = dbConfig.GetHostConfig()
            GlobalDataConfig.INQUERY_DBCONFIG = dbConfig.GetInQueryConfig()
            AppDomain.Unload(myDomain)

            'connect HSM
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
            'ServerBussinessCA.OpenHSM(mv_oSignServer, v_intSlotId, v_strPIN, _
            '                          v_strPublicKeyName, v_strPrivateKeyName, _
            '                          v_strCertificateName)
        Catch ex As Exception
            LogError.Write("Start service - Error source: " & ex.Source & vbNewLine _
                         & "Error code: System error!" & vbNewLine _
                         & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_HOST)
        End Try
    End Sub

    Protected Overrides Sub OnStop()
        ' Add code here to perform any tear-down necessary to stop your service.
        If Not v_oWCF Is Nothing Then
            v_oWCF.CallbackStop()
            v_oWCF = Nothing
        End If
    End Sub

    Private Sub bgw_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles v_wb.DoWork
        Dim v_obj As New UserUpdate
        v_obj.UserUpdateStart()
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
