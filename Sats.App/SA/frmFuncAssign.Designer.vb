<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmFuncAssign
    Inherits Windows.Forms.Form

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmFuncAssign))
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.lbCaption = New System.Windows.Forms.Label
        Me.stvTran = New Xceed.SmartUI.Controls.TreeView.SmartTreeView(Me.components)
        Me.Node2 = New Xceed.SmartUI.Controls.TreeView.Node("Sats1.0", 2)
        Me.imgList = New System.Windows.Forms.ImageList(Me.components)
        Me.grbAccessRight = New System.Windows.Forms.GroupBox
        Me.chkExport = New System.Windows.Forms.CheckBox
        Me.chkSearch = New System.Windows.Forms.CheckBox
        Me.chkAccess = New System.Windows.Forms.CheckBox
        Me.chkView = New System.Windows.Forms.CheckBox
        Me.chkDelete = New System.Windows.Forms.CheckBox
        Me.chkEdit = New System.Windows.Forms.CheckBox
        Me.chkAddNew = New System.Windows.Forms.CheckBox
        Me.btnOk = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.Node1 = New Xceed.SmartUI.Controls.TreeView.Node("Sats1.0")
        Me.lbFunc = New System.Windows.Forms.Label
        Me.lbBrID = New System.Windows.Forms.Label
        Me.lstBrID = New System.Windows.Forms.ListBox
        Me.Panel1.SuspendLayout()
        Me.grbAccessRight.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.SystemColors.Desktop
        Me.Panel1.Controls.Add(Me.lbCaption)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(685, 64)
        Me.Panel1.TabIndex = 0
        '
        'lbCaption
        '
        Me.lbCaption.AutoSize = True
        Me.lbCaption.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbCaption.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lbCaption.Location = New System.Drawing.Point(30, 26)
        Me.lbCaption.Name = "lbCaption"
        Me.lbCaption.Size = New System.Drawing.Size(60, 13)
        Me.lbCaption.TabIndex = 0
        Me.lbCaption.Tag = "lbCaption"
        Me.lbCaption.Text = "lbCaption"
        '
        'stvTran
        '
        Me.stvTran.Dock = System.Windows.Forms.DockStyle.Left
        Me.stvTran.Items.AddRange(New Xceed.SmartUI.SmartItem() {Me.Node2})
        Me.stvTran.ItemsImageList = Me.imgList
        Me.stvTran.Location = New System.Drawing.Point(0, 64)
        Me.stvTran.Name = "stvTran"
        Me.stvTran.Size = New System.Drawing.Size(263, 396)
        Me.stvTran.TabIndex = 1
        Me.stvTran.Tag = "stvTran"
        Me.stvTran.Text = "stvTran"
        Me.stvTran.UIStyle = Xceed.SmartUI.UIStyle.UIStyle.WindowsClassic
        '
        'Node2
        '
        Me.Node2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Node2.ImageIndex = 2
        Me.Node2.Text = "Sats1.0"
        '
        'imgList
        '
        Me.imgList.ImageStream = CType(resources.GetObject("imgList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgList.TransparentColor = System.Drawing.Color.Transparent
        Me.imgList.Images.SetKeyName(0, "shell32_5.ico")
        Me.imgList.Images.SetKeyName(1, "shell32_4.ico")
        Me.imgList.Images.SetKeyName(2, "shell32_276.ico")
        Me.imgList.Images.SetKeyName(3, "MAPIR_15601.ico")
        '
        'grbAccessRight
        '
        Me.grbAccessRight.Controls.Add(Me.chkExport)
        Me.grbAccessRight.Controls.Add(Me.chkSearch)
        Me.grbAccessRight.Controls.Add(Me.chkAccess)
        Me.grbAccessRight.Controls.Add(Me.chkView)
        Me.grbAccessRight.Controls.Add(Me.chkDelete)
        Me.grbAccessRight.Controls.Add(Me.chkEdit)
        Me.grbAccessRight.Controls.Add(Me.chkAddNew)
        Me.grbAccessRight.Location = New System.Drawing.Point(269, 103)
        Me.grbAccessRight.Name = "grbAccessRight"
        Me.grbAccessRight.Size = New System.Drawing.Size(195, 345)
        Me.grbAccessRight.TabIndex = 2
        Me.grbAccessRight.TabStop = False
        Me.grbAccessRight.Tag = "grbAccessRight"
        Me.grbAccessRight.Text = "grbAccessRight"
        '
        'chkExport
        '
        Me.chkExport.AutoSize = True
        Me.chkExport.Location = New System.Drawing.Point(22, 270)
        Me.chkExport.Name = "chkExport"
        Me.chkExport.Size = New System.Drawing.Size(74, 17)
        Me.chkExport.TabIndex = 6
        Me.chkExport.Tag = "chkExport"
        Me.chkExport.Text = "chkExport"
        Me.chkExport.UseVisualStyleBackColor = True
        '
        'chkSearch
        '
        Me.chkSearch.AutoSize = True
        Me.chkSearch.Location = New System.Drawing.Point(22, 70)
        Me.chkSearch.Name = "chkSearch"
        Me.chkSearch.Size = New System.Drawing.Size(78, 17)
        Me.chkSearch.TabIndex = 5
        Me.chkSearch.Tag = "chkSearch"
        Me.chkSearch.Text = "chkSearch"
        Me.chkSearch.UseVisualStyleBackColor = True
        '
        'chkAccess
        '
        Me.chkAccess.AutoSize = True
        Me.chkAccess.Location = New System.Drawing.Point(22, 30)
        Me.chkAccess.Name = "chkAccess"
        Me.chkAccess.Size = New System.Drawing.Size(79, 17)
        Me.chkAccess.TabIndex = 4
        Me.chkAccess.Tag = "chkAccess"
        Me.chkAccess.Text = "chkAccess"
        Me.chkAccess.UseVisualStyleBackColor = True
        '
        'chkView
        '
        Me.chkView.AutoSize = True
        Me.chkView.Location = New System.Drawing.Point(22, 110)
        Me.chkView.Name = "chkView"
        Me.chkView.Size = New System.Drawing.Size(67, 17)
        Me.chkView.TabIndex = 3
        Me.chkView.Tag = "chkView"
        Me.chkView.Text = "chkView"
        Me.chkView.UseVisualStyleBackColor = True
        '
        'chkDelete
        '
        Me.chkDelete.AutoSize = True
        Me.chkDelete.Location = New System.Drawing.Point(22, 230)
        Me.chkDelete.Name = "chkDelete"
        Me.chkDelete.Size = New System.Drawing.Size(75, 17)
        Me.chkDelete.TabIndex = 2
        Me.chkDelete.Tag = "chkDelete"
        Me.chkDelete.Text = "chkDelete"
        Me.chkDelete.UseVisualStyleBackColor = True
        '
        'chkEdit
        '
        Me.chkEdit.AutoSize = True
        Me.chkEdit.Location = New System.Drawing.Point(22, 190)
        Me.chkEdit.Name = "chkEdit"
        Me.chkEdit.Size = New System.Drawing.Size(62, 17)
        Me.chkEdit.TabIndex = 1
        Me.chkEdit.Tag = "chkEdit"
        Me.chkEdit.Text = "chkEdit"
        Me.chkEdit.UseVisualStyleBackColor = True
        '
        'chkAddNew
        '
        Me.chkAddNew.AutoSize = True
        Me.chkAddNew.Location = New System.Drawing.Point(22, 150)
        Me.chkAddNew.Name = "chkAddNew"
        Me.chkAddNew.Size = New System.Drawing.Size(85, 17)
        Me.chkAddNew.TabIndex = 0
        Me.chkAddNew.Tag = "chkAddNew"
        Me.chkAddNew.Text = "chkAddNew"
        Me.chkAddNew.UseVisualStyleBackColor = True
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(478, 420)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(96, 29)
        Me.btnOk.TabIndex = 3
        Me.btnOk.Tag = "btnOk"
        Me.btnOk.Text = "btnOk"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(580, 420)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(93, 28)
        Me.btnCancel.TabIndex = 4
        Me.btnCancel.Tag = "btnCancel"
        Me.btnCancel.Text = "btnCancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'Node1
        '
        Me.Node1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Node1.Text = "Sats1.0"
        '
        'lbFunc
        '
        Me.lbFunc.AutoSize = True
        Me.lbFunc.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbFunc.ForeColor = System.Drawing.Color.DarkRed
        Me.lbFunc.Location = New System.Drawing.Point(269, 77)
        Me.lbFunc.Name = "lbFunc"
        Me.lbFunc.Size = New System.Drawing.Size(45, 13)
        Me.lbFunc.TabIndex = 5
        Me.lbFunc.Tag = "lbFunc"
        Me.lbFunc.Text = "lbFunc"
        '
        'lbBrID
        '
        Me.lbBrID.AutoSize = True
        Me.lbBrID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbBrID.ForeColor = System.Drawing.Color.DarkRed
        Me.lbBrID.Location = New System.Drawing.Point(467, 77)
        Me.lbBrID.Name = "lbBrID"
        Me.lbBrID.Size = New System.Drawing.Size(42, 13)
        Me.lbBrID.TabIndex = 11
        Me.lbBrID.Tag = "lbBrID"
        Me.lbBrID.Text = "lbBrID"
        '
        'lstBrID
        '
        Me.lstBrID.FormattingEnabled = True
        Me.lstBrID.Location = New System.Drawing.Point(470, 110)
        Me.lstBrID.Name = "lstBrID"
        Me.lstBrID.Size = New System.Drawing.Size(209, 303)
        Me.lstBrID.TabIndex = 10
        Me.lstBrID.Tag = "lbBrID"
        '
        'frmFuncAssign
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(685, 460)
        Me.Controls.Add(Me.lbBrID)
        Me.Controls.Add(Me.lstBrID)
        Me.Controls.Add(Me.lbFunc)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.grbAccessRight)
        Me.Controls.Add(Me.stvTran)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmFuncAssign"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "frmFuncAssign"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.grbAccessRight.ResumeLayout(False)
        Me.grbAccessRight.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents lbCaption As System.Windows.Forms.Label
    Friend WithEvents stvTran As Xceed.SmartUI.Controls.TreeView.SmartTreeView
    Friend WithEvents grbAccessRight As System.Windows.Forms.GroupBox
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents chkExport As System.Windows.Forms.CheckBox
    Friend WithEvents chkSearch As System.Windows.Forms.CheckBox
    Friend WithEvents chkAccess As System.Windows.Forms.CheckBox
    Friend WithEvents chkView As System.Windows.Forms.CheckBox
    Friend WithEvents chkDelete As System.Windows.Forms.CheckBox
    Friend WithEvents chkEdit As System.Windows.Forms.CheckBox
    Friend WithEvents chkAddNew As System.Windows.Forms.CheckBox
    Friend WithEvents Node1 As Xceed.SmartUI.Controls.TreeView.Node
    Friend WithEvents imgList As System.Windows.Forms.ImageList
    Friend WithEvents Node2 As Xceed.SmartUI.Controls.TreeView.Node
    Friend WithEvents lbFunc As System.Windows.Forms.Label
    Friend WithEvents lbBrID As System.Windows.Forms.Label
    Friend WithEvents lstBrID As System.Windows.Forms.ListBox
End Class
