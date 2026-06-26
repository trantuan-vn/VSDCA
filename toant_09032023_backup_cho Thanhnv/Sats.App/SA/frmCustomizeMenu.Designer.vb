<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCustomizeMenu
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCustomizeMenu))
        Me.tvwTransact = New Xceed.SmartUI.Controls.TreeView.SmartTreeView(Me.components)
        Me.Node1 = New Xceed.SmartUI.Controls.TreeView.Node("Sats 1.0", 2)
        Me.imgList = New System.Windows.Forms.ImageList(Me.components)
        Me.stvFavmenu = New Xceed.SmartUI.Controls.TreeView.SmartTreeView(Me.components)
        Me.mnuContent = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.cmdAdd = New System.Windows.Forms.ToolStripMenuItem
        Me.cmdEdit = New System.Windows.Forms.ToolStripMenuItem
        Me.cmdDel = New System.Windows.Forms.ToolStripMenuItem
        Me.Node2 = New Xceed.SmartUI.Controls.TreeView.Node("Sats 1.0", 2)
        Me.btnAdd = New System.Windows.Forms.Button
        Me.btnLoadDefault = New System.Windows.Forms.Button
        Me.btnReLoad = New System.Windows.Forms.Button
        Me.btnSave = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.mnuContent.SuspendLayout()
        Me.SuspendLayout()
        '
        'tvwTransact
        '
        Me.tvwTransact.BackColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.tvwTransact.Dock = System.Windows.Forms.DockStyle.Left
        Me.tvwTransact.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tvwTransact.Items.AddRange(New Xceed.SmartUI.SmartItem() {Me.Node1})
        Me.tvwTransact.ItemsImageList = Me.imgList
        Me.tvwTransact.Location = New System.Drawing.Point(0, 0)
        Me.tvwTransact.Name = "tvwTransact"
        Me.tvwTransact.Size = New System.Drawing.Size(315, 531)
        Me.tvwTransact.TabIndex = 2
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
        'stvFavmenu
        '
        Me.stvFavmenu.BackColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.stvFavmenu.ContextMenuStrip = Me.mnuContent
        Me.stvFavmenu.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.stvFavmenu.Items.AddRange(New Xceed.SmartUI.SmartItem() {Me.Node2})
        Me.stvFavmenu.ItemsImageList = Me.imgList
        Me.stvFavmenu.Location = New System.Drawing.Point(385, 0)
        Me.stvFavmenu.Name = "stvFavmenu"
        Me.stvFavmenu.Size = New System.Drawing.Size(351, 485)
        Me.stvFavmenu.TabIndex = 3
        Me.stvFavmenu.Text = "stvFavmenu"
        Me.stvFavmenu.UIStyle = Xceed.SmartUI.UIStyle.UIStyle.WindowsClassic
        '
        'mnuContent
        '
        Me.mnuContent.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmdAdd, Me.cmdEdit, Me.cmdDel})
        Me.mnuContent.Name = "mnuContent"
        Me.mnuContent.Size = New System.Drawing.Size(131, 70)
        '
        'cmdAdd
        '
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.Size = New System.Drawing.Size(130, 22)
        Me.cmdAdd.Text = "Thêm mới"
        '
        'cmdEdit
        '
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.Size = New System.Drawing.Size(130, 22)
        Me.cmdEdit.Text = "Sửa"
        '
        'cmdDel
        '
        Me.cmdDel.Name = "cmdDel"
        Me.cmdDel.Size = New System.Drawing.Size(130, 22)
        Me.cmdDel.Text = "Xóa"
        '
        'Node2
        '
        Me.Node2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Node2.ImageIndex = 2
        Me.Node2.Text = "Sats 1.0"
        '
        'btnAdd
        '
        Me.btnAdd.Font = New System.Drawing.Font("Webdings", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.btnAdd.Location = New System.Drawing.Point(336, 179)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(30, 30)
        Me.btnAdd.TabIndex = 9
        Me.btnAdd.Tag = "btnAdd"
        Me.btnAdd.Text = "4"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'btnLoadDefault
        '
        Me.btnLoadDefault.Location = New System.Drawing.Point(385, 491)
        Me.btnLoadDefault.Name = "btnLoadDefault"
        Me.btnLoadDefault.Size = New System.Drawing.Size(79, 28)
        Me.btnLoadDefault.TabIndex = 12
        Me.btnLoadDefault.Text = "&Mặc định"
        Me.btnLoadDefault.UseVisualStyleBackColor = True
        '
        'btnReLoad
        '
        Me.btnReLoad.Location = New System.Drawing.Point(472, 491)
        Me.btnReLoad.Name = "btnReLoad"
        Me.btnReLoad.Size = New System.Drawing.Size(79, 28)
        Me.btnReLoad.TabIndex = 13
        Me.btnReLoad.Text = "&Làm lại"
        Me.btnReLoad.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(559, 491)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(79, 28)
        Me.btnSave.TabIndex = 14
        Me.btnSave.Text = "&Chấp nhận"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(646, 491)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(79, 28)
        Me.btnCancel.TabIndex = 15
        Me.btnCancel.Text = "&Thoát"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'frmCustomizeMenu
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(735, 531)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.btnReLoad)
        Me.Controls.Add(Me.btnLoadDefault)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.stvFavmenu)
        Me.Controls.Add(Me.tvwTransact)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmCustomizeMenu"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Nghiệp vụ thường dùng"
        Me.mnuContent.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tvwTransact As Xceed.SmartUI.Controls.TreeView.SmartTreeView
    Friend WithEvents Node1 As Xceed.SmartUI.Controls.TreeView.Node
    Friend WithEvents stvFavmenu As Xceed.SmartUI.Controls.TreeView.SmartTreeView
    Friend WithEvents Node2 As Xceed.SmartUI.Controls.TreeView.Node
    Friend WithEvents imgList As System.Windows.Forms.ImageList
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents mnuContent As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents cmdAdd As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmdEdit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmdDel As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnLoadDefault As System.Windows.Forms.Button
    Friend WithEvents btnReLoad As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
End Class
