<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmReportView
    Inherits System.Windows.Forms.Form

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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmReportView))
        Me.arViewer = New DataDynamics.ActiveReports.Viewer.Viewer
        Me.imlTool = New System.Windows.Forms.ImageList(Me.components)
        Me.PrintDialog1 = New System.Windows.Forms.PrintDialog
        Me.SuspendLayout()
        '
        'arViewer
        '
        Me.arViewer.BackColor = System.Drawing.SystemColors.Control
        Me.arViewer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.arViewer.Document = New DataDynamics.ActiveReports.Document.Document("ARNet Document")
        Me.arViewer.Location = New System.Drawing.Point(0, 0)
        Me.arViewer.Name = "arViewer"
        Me.arViewer.ReportViewer.CurrentPage = 0
        Me.arViewer.ReportViewer.MultiplePageCols = 3
        Me.arViewer.ReportViewer.MultiplePageRows = 2
        Me.arViewer.ReportViewer.RepositionPage = True
        Me.arViewer.ReportViewer.ViewType = DataDynamics.ActiveReports.Viewer.ViewType.Normal
        Me.arViewer.Size = New System.Drawing.Size(763, 518)
        Me.arViewer.TabIndex = 0
        Me.arViewer.TableOfContents.Text = "Table Of Contents"
        Me.arViewer.TableOfContents.Width = 200
        Me.arViewer.TabTitleLength = 35
        Me.arViewer.Toolbar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        '
        'imlTool
        '
        Me.imlTool.ImageStream = CType(resources.GetObject("imlTool.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imlTool.TransparentColor = System.Drawing.Color.Transparent
        Me.imlTool.Images.SetKeyName(0, "setupapi_50.ico")
        Me.imlTool.Images.SetKeyName(1, "EXCEL_257.ico")
        '
        'PrintDialog1
        '
        Me.PrintDialog1.AllowCurrentPage = Global.Sats.AppCore.My.MySettings.Default.CrrPg
        Me.PrintDialog1.AllowSelection = Global.Sats.AppCore.My.MySettings.Default.sltn
        Me.PrintDialog1.AllowSomePages = True
        Me.PrintDialog1.PrintToFile = True
        Me.PrintDialog1.UseEXDialog = True
        '
        'frmReportView
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(763, 518)
        Me.Controls.Add(Me.arViewer)
        Me.Name = "frmReportView"
        Me.Text = "frmReportView"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents arViewer As DataDynamics.ActiveReports.Viewer.Viewer
    Friend WithEvents imlTool As System.Windows.Forms.ImageList
    Friend WithEvents PrintDialog1 As System.Windows.Forms.PrintDialog
End Class
