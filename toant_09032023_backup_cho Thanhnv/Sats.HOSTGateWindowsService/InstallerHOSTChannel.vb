Imports System.ComponentModel
Imports System.Configuration.Install
Imports System.ServiceProcess

Public Class InstallerHOSTChannel
    Private mv_ServerInstall As ServiceInstaller
    Private mv_ServerProcessInstall As ServiceProcessInstaller

    Public Sub New()
        MyBase.New()

        'This call is required by the Component Designer.
        InitializeComponent()

        'Add initialization code after the call to InitializeComponent
        mv_ServerInstall = New ServiceInstaller
        mv_ServerProcessInstall = New ServiceProcessInstaller

        mv_ServerProcessInstall.Account = ServiceAccount.LocalSystem
        mv_ServerInstall.ServiceName = "HOSTGateChannel"
        mv_ServerInstall.DisplayName = "VSDS_HOSTGateChannel"
        mv_ServerInstall.StartType = ServiceStartMode.Automatic

        Installers.Add(mv_ServerInstall)
        Installers.Add(mv_ServerProcessInstall)
    End Sub

End Class
