<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmReportMaster
    Inherits Sats.AppCore.frmSearch

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        components = New System.ComponentModel.Container
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Text = "frmReportMaster"
    End Sub

    Public Sub New(ByVal pv_strLanguage As String)

        ' This call is required by the Windows Form Designer.
        MyBase.New(pv_strLanguage)
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
End Class
