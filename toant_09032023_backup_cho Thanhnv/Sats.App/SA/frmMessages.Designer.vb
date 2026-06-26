<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMessages
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMessages))
        Me.stbTool = New Xceed.SmartUI.Controls.ToolBar.SmartToolBar(Me.components)
        Me.btnBack = New Xceed.SmartUI.Controls.ToolBar.Tool(6)
        Me.btnNext = New Xceed.SmartUI.Controls.ToolBar.Tool(5)
        Me.SeparatorTool3 = New Xceed.SmartUI.Controls.ToolBar.SeparatorTool
        Me.btnAddNew = New Xceed.SmartUI.Controls.ToolBar.Tool("btnAddNew", 0)
        Me.btnView = New Xceed.SmartUI.Controls.ToolBar.Tool("btnView", 3)
        Me.btnEdit = New Xceed.SmartUI.Controls.ToolBar.Tool("btnEdit", 7)
        Me.btnDelete = New Xceed.SmartUI.Controls.ToolBar.Tool("btnDelete", 2)
        Me.btnExecute = New Xceed.SmartUI.Controls.ToolBar.Tool("btnExecute", 8)
        Me.SeparatorTool2 = New Xceed.SmartUI.Controls.ToolBar.SeparatorTool
        Me.btnExit = New Xceed.SmartUI.Controls.ToolBar.Tool("btnExit", 1)
        Me.ImgTool = New System.Windows.Forms.ImageList(Me.components)
        Me.spcMain = New System.Windows.Forms.SplitContainer
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.spcMain.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.SuspendLayout()
        '
        'stbTool
        '
        Me.stbTool.Items.AddRange(New Xceed.SmartUI.SmartItem() {Me.btnBack, Me.btnNext, Me.SeparatorTool3, Me.btnAddNew, Me.btnView, Me.btnEdit, Me.btnDelete, Me.btnExecute, Me.SeparatorTool2, Me.btnExit})
        Me.stbTool.ItemsImageList = Me.ImgTool
        Me.stbTool.Location = New System.Drawing.Point(0, 0)
        Me.stbTool.Name = "stbTool"
        Me.stbTool.Padding = New System.Windows.Forms.Padding(2)
        Me.stbTool.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.stbTool.Size = New System.Drawing.Size(971, 34)
        Me.stbTool.TabIndex = 11
        Me.stbTool.Text = "stbTool"
        Me.stbTool.UIStyle = Xceed.SmartUI.UIStyle.UIStyle.WindowsClassic
        '
        'btnBack
        '
        Me.btnBack.ImageIndex = 6
        Me.btnBack.Shortcut = System.Windows.Forms.Shortcut.F7
        Me.btnBack.ShowShortcut = False
        Me.btnBack.ToolTipText = "F7"
        '
        'btnNext
        '
        Me.btnNext.ImageIndex = 5
        Me.btnNext.Shortcut = System.Windows.Forms.Shortcut.F8
        Me.btnNext.ShowShortcut = False
        Me.btnNext.ToolTipText = "F8"
        '
        'btnAddNew
        '
        Me.btnAddNew.ImageIndex = 0
        Me.btnAddNew.Shortcut = System.Windows.Forms.Shortcut.CtrlN
        Me.btnAddNew.ShowShortcut = False
        Me.btnAddNew.Text = "btnAddNew"
        Me.btnAddNew.ToolTipText = "Ctrl+N"
        '
        'btnView
        '
        Me.btnView.ImageIndex = 3
        Me.btnView.Shortcut = System.Windows.Forms.Shortcut.CtrlV
        Me.btnView.ShowShortcut = False
        Me.btnView.Text = "btnView"
        Me.btnView.ToolTipText = "Ctrl+V"
        '
        'btnEdit
        '
        Me.btnEdit.ImageIndex = 7
        Me.btnEdit.Shortcut = System.Windows.Forms.Shortcut.CtrlE
        Me.btnEdit.ShowShortcut = False
        Me.btnEdit.Text = "btnEdit"
        Me.btnEdit.ToolTipText = "Ctrl+S"
        '
        'btnDelete
        '
        Me.btnDelete.ImageIndex = 2
        Me.btnDelete.Shortcut = System.Windows.Forms.Shortcut.CtrlD
        Me.btnDelete.ShowShortcut = False
        Me.btnDelete.Text = "btnDelete"
        Me.btnDelete.ToolTipText = "Ctrl+X"
        '
        'btnExecute
        '
        Me.btnExecute.ImageIndex = 8
        Me.btnExecute.Shortcut = System.Windows.Forms.Shortcut.CtrlP
        Me.btnExecute.Text = "btnExecute"
        Me.btnExecute.ToolTipText = "Ctrl+P"
        '
        'btnExit
        '
        Me.btnExit.ImageIndex = 1
        Me.btnExit.Text = "btnExit"
        '
        'ImgTool
        '
        Me.ImgTool.ImageStream = CType(resources.GetObject("ImgTool.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImgTool.TransparentColor = System.Drawing.Color.Transparent
        Me.ImgTool.Images.SetKeyName(0, "")
        Me.ImgTool.Images.SetKeyName(1, "")
        Me.ImgTool.Images.SetKeyName(2, "")
        Me.ImgTool.Images.SetKeyName(3, "")
        Me.ImgTool.Images.SetKeyName(4, "")
        Me.ImgTool.Images.SetKeyName(5, "")
        Me.ImgTool.Images.SetKeyName(6, "")
        Me.ImgTool.Images.SetKeyName(7, "")
        Me.ImgTool.Images.SetKeyName(8, "Hardware Devices.bmp")
        '
        'spcMain
        '
        Me.spcMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.spcMain.Location = New System.Drawing.Point(0, 34)
        Me.spcMain.Name = "spcMain"
        '
        'spcMain.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.SplitContainer1)
        Me.spcMain.Size = New System.Drawing.Size(971, 524)
        Me.spcMain.SplitterDistance = 353
        Me.spcMain.TabIndex = 12
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.SplitContainer1.Size = New System.Drawing.Size(614, 524)
        Me.SplitContainer1.SplitterDistance = 204
        Me.SplitContainer1.TabIndex = 0
        '
        'frmMessages
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(971, 558)
        Me.Controls.Add(Me.spcMain)
        Me.Controls.Add(Me.stbTool)
        Me.KeyPreview = True
        Me.Name = "frmMessages"
        Me.Text = "Giao dịch chờ xác nhận"
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.spcMain.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents stbTool As Xceed.SmartUI.Controls.ToolBar.SmartToolBar
    Friend WithEvents btnBack As Xceed.SmartUI.Controls.ToolBar.Tool
    Friend WithEvents btnNext As Xceed.SmartUI.Controls.ToolBar.Tool
    Friend WithEvents SeparatorTool3 As Xceed.SmartUI.Controls.ToolBar.SeparatorTool
    Protected WithEvents btnAddNew As Xceed.SmartUI.Controls.ToolBar.Tool
    Protected WithEvents btnView As Xceed.SmartUI.Controls.ToolBar.Tool
    Protected WithEvents btnEdit As Xceed.SmartUI.Controls.ToolBar.Tool
    Protected WithEvents btnDelete As Xceed.SmartUI.Controls.ToolBar.Tool
    Protected WithEvents btnExecute As Xceed.SmartUI.Controls.ToolBar.Tool
    Friend WithEvents SeparatorTool2 As Xceed.SmartUI.Controls.ToolBar.SeparatorTool
    Protected WithEvents btnExit As Xceed.SmartUI.Controls.ToolBar.Tool
    Friend WithEvents ImgTool As System.Windows.Forms.ImageList
    Friend WithEvents spcMain As System.Windows.Forms.SplitContainer
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
End Class
