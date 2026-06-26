<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmReportAssign
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmReportAssign))
        Me.chkExport = New System.Windows.Forms.CheckBox
        Me.Node1 = New Xceed.SmartUI.Controls.TreeView.Node("Sats1.0")
        Me.lbFunc = New System.Windows.Forms.Label
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnOk = New System.Windows.Forms.Button
        Me.chkView = New System.Windows.Forms.CheckBox
        Me.lbCaption = New System.Windows.Forms.Label
        Me.grbAccessRight = New System.Windows.Forms.GroupBox
        Me.chkPrint = New System.Windows.Forms.CheckBox
        Me.chkCreate = New System.Windows.Forms.CheckBox
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.imgList = New System.Windows.Forms.ImageList(Me.components)
        Me.stvTran = New Xceed.SmartUI.Controls.TreeView.SmartTreeView(Me.components)
        Me.Node2 = New Xceed.SmartUI.Controls.TreeView.Node("Sats1.0", 2)
        Me.grbAccessRight.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'chkExport
        '
        Me.chkExport.AutoSize = True
        Me.chkExport.Location = New System.Drawing.Point(12, 134)
        Me.chkExport.Name = "chkExport"
        Me.chkExport.Size = New System.Drawing.Size(74, 17)
        Me.chkExport.TabIndex = 6
        Me.chkExport.Tag = "chkExport"
        Me.chkExport.Text = "chkExport"
        Me.chkExport.UseVisualStyleBackColor = True
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
        Me.lbFunc.Location = New System.Drawing.Point(269, 78)
        Me.lbFunc.Name = "lbFunc"
        Me.lbFunc.Size = New System.Drawing.Size(45, 13)
        Me.lbFunc.TabIndex = 11
        Me.lbFunc.Tag = "lbFunc"
        Me.lbFunc.Text = "lbFunc"
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(364, 269)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(89, 28)
        Me.btnCancel.TabIndex = 10
        Me.btnCancel.Tag = "btnCancel"
        Me.btnCancel.Text = "btnCancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(269, 268)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(89, 29)
        Me.btnOk.TabIndex = 9
        Me.btnOk.Tag = "btnOk"
        Me.btnOk.Text = "btnOk"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'chkView
        '
        Me.chkView.AutoSize = True
        Me.chkView.Location = New System.Drawing.Point(12, 68)
        Me.chkView.Name = "chkView"
        Me.chkView.Size = New System.Drawing.Size(67, 17)
        Me.chkView.TabIndex = 1
        Me.chkView.Tag = "chkView"
        Me.chkView.Text = "chkView"
        Me.chkView.UseVisualStyleBackColor = True
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
        'grbAccessRight
        '
        Me.grbAccessRight.Controls.Add(Me.chkPrint)
        Me.grbAccessRight.Controls.Add(Me.chkExport)
        Me.grbAccessRight.Controls.Add(Me.chkCreate)
        Me.grbAccessRight.Controls.Add(Me.chkView)
        Me.grbAccessRight.Location = New System.Drawing.Point(269, 103)
        Me.grbAccessRight.Name = "grbAccessRight"
        Me.grbAccessRight.Size = New System.Drawing.Size(184, 159)
        Me.grbAccessRight.TabIndex = 8
        Me.grbAccessRight.TabStop = False
        Me.grbAccessRight.Tag = "grbAccessRight"
        Me.grbAccessRight.Text = "grbAccessRight"
        '
        'chkPrint
        '
        Me.chkPrint.AutoSize = True
        Me.chkPrint.Location = New System.Drawing.Point(12, 101)
        Me.chkPrint.Name = "chkPrint"
        Me.chkPrint.Size = New System.Drawing.Size(65, 17)
        Me.chkPrint.TabIndex = 7
        Me.chkPrint.Tag = "chkPrint"
        Me.chkPrint.Text = "chkPrint"
        Me.chkPrint.UseVisualStyleBackColor = True
        '
        'chkCreate
        '
        Me.chkCreate.AutoSize = True
        Me.chkCreate.Location = New System.Drawing.Point(12, 35)
        Me.chkCreate.Name = "chkCreate"
        Me.chkCreate.Size = New System.Drawing.Size(75, 17)
        Me.chkCreate.TabIndex = 3
        Me.chkCreate.Tag = "chkCreate"
        Me.chkCreate.Text = "chkCreate"
        Me.chkCreate.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.SystemColors.Desktop
        Me.Panel1.Controls.Add(Me.lbCaption)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(464, 64)
        Me.Panel1.TabIndex = 6
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
        'stvTran
        '
        Me.stvTran.Dock = System.Windows.Forms.DockStyle.Left
        Me.stvTran.Items.AddRange(New Xceed.SmartUI.SmartItem() {Me.Node2})
        Me.stvTran.ItemsImageList = Me.imgList
        Me.stvTran.Location = New System.Drawing.Point(0, 64)
        Me.stvTran.Name = "stvTran"
        Me.stvTran.Size = New System.Drawing.Size(263, 388)
        Me.stvTran.TabIndex = 12
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
        'frmReportAssign
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(464, 452)
        Me.Controls.Add(Me.stvTran)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.lbFunc)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.grbAccessRight)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmReportAssign"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "frmReportAssign"
        Me.grbAccessRight.ResumeLayout(False)
        Me.grbAccessRight.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents chkExport As System.Windows.Forms.CheckBox
    Friend WithEvents Node1 As Xceed.SmartUI.Controls.TreeView.Node
    Friend WithEvents lbFunc As System.Windows.Forms.Label
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents chkView As System.Windows.Forms.CheckBox
    Friend WithEvents lbCaption As System.Windows.Forms.Label
    Friend WithEvents grbAccessRight As System.Windows.Forms.GroupBox
    Friend WithEvents chkCreate As System.Windows.Forms.CheckBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents imgList As System.Windows.Forms.ImageList
    Friend WithEvents stvTran As Xceed.SmartUI.Controls.TreeView.SmartTreeView
    Friend WithEvents Node2 As Xceed.SmartUI.Controls.TreeView.Node
    Friend WithEvents chkPrint As System.Windows.Forms.CheckBox
End Class
