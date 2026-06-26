<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DockTreeView
    Inherits Sats.WinFormsUI.Docking.DockContent
    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DockTreeView))
        Me.tvwTransact = New Xceed.SmartUI.Controls.TreeView.SmartTreeView(Me.components)
        Me.Node1 = New Xceed.SmartUI.Controls.TreeView.Node("Sats 1.0", 2)
        Me.imgList = New System.Windows.Forms.ImageList(Me.components)
        Me.SuspendLayout()
        '
        'tvwTransact
        '
        Me.tvwTransact.BackColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.tvwTransact.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tvwTransact.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tvwTransact.Items.AddRange(New Xceed.SmartUI.SmartItem() {Me.Node1})
        Me.tvwTransact.ItemsImageList = Me.imgList
        Me.tvwTransact.Location = New System.Drawing.Point(0, 0)
        Me.tvwTransact.Name = "tvwTransact"
        Me.tvwTransact.Size = New System.Drawing.Size(214, 564)
        Me.tvwTransact.TabIndex = 1
        Me.tvwTransact.Text = "tvwTransact"
        Me.tvwTransact.UIStyle = Xceed.SmartUI.UIStyle.UIStyle.WindowsClassic
        '
        'Node1
        '
        Me.Node1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Node1.ImageIndex = 2
        Me.Node1.Text = "Sats 1.0"
        '
        'imgList
        '
        Me.imgList.ImageStream = CType(resources.GetObject("imgList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgList.TransparentColor = System.Drawing.Color.Transparent
        Me.imgList.Images.SetKeyName(0, "shell32_4.ico")
        Me.imgList.Images.SetKeyName(1, "shell32_5.ico")
        Me.imgList.Images.SetKeyName(2, "shell32_276.ico")
        Me.imgList.Images.SetKeyName(3, "MAPIR_15601.ico")
        '
        'DockTreeView
        '
        Me.ClientSize = New System.Drawing.Size(214, 564)
        Me.Controls.Add(Me.tvwTransact)
        Me.DockAreas = Sats.WinFormsUI.Docking.DockAreas.DockLeft
        Me.HideOnClose = True
        Me.KeyPreview = True
        Me.Name = "DockTreeView"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tvwTransact As Xceed.SmartUI.Controls.TreeView.SmartTreeView

    Public Sub New()

        Xceed.SmartUI.Licenser.LicenseKey = "SUN31-241NZ-YZDPW-JKBA"
        'Xceed.Grid.Licenser.LicenseKey = "GRD11-XU1NZ-BZ1HW-AKCA"
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Friend WithEvents Node1 As Xceed.SmartUI.Controls.TreeView.Node
    Friend WithEvents imgList As System.Windows.Forms.ImageList
End Class
