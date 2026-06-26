<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTranAssign
    Inherits Windows.Forms.Form

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTranAssign))
        Me.lbTrans = New System.Windows.Forms.Label
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnOk = New System.Windows.Forms.Button
        Me.grbTransAssign = New System.Windows.Forms.GroupBox
        Me.txtDM3 = New Sats.AppCore.FlexMaskEditBox
        Me.chkDM3 = New System.Windows.Forms.CheckBox
        Me.txtDM2 = New Sats.AppCore.FlexMaskEditBox
        Me.chkDM2 = New System.Windows.Forms.CheckBox
        Me.txtDM1 = New Sats.AppCore.FlexMaskEditBox
        Me.chkDM1 = New System.Windows.Forms.CheckBox
        Me.txtGD = New Sats.AppCore.FlexMaskEditBox
        Me.chkGD = New System.Windows.Forms.CheckBox
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.lbCaption = New System.Windows.Forms.Label
        Me.imgList = New System.Windows.Forms.ImageList(Me.components)
        Me.stvTrans = New Xceed.SmartUI.Controls.TreeView.SmartTreeView(Me.components)
        Me.Node1 = New Xceed.SmartUI.Controls.TreeView.Node("Sats - Transaction", 2)
        Me.lbBrID = New System.Windows.Forms.Label
        Me.lstBrID = New System.Windows.Forms.ListBox
        Me.grbTransAssign.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'lbTrans
        '
        Me.lbTrans.AutoSize = True
        Me.lbTrans.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lbTrans.ForeColor = System.Drawing.Color.Firebrick
        Me.lbTrans.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lbTrans.Location = New System.Drawing.Point(285, 159)
        Me.lbTrans.Name = "lbTrans"
        Me.lbTrans.Size = New System.Drawing.Size(49, 13)
        Me.lbTrans.TabIndex = 13
        Me.lbTrans.Text = "lbTrans"
        '
        'btnCancel
        '
        Me.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnCancel.Location = New System.Drawing.Point(439, 362)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(78, 28)
        Me.btnCancel.TabIndex = 12
        Me.btnCancel.Tag = "btnCancel"
        Me.btnCancel.Text = "btnCancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnOk
        '
        Me.btnOk.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnOk.Location = New System.Drawing.Point(355, 362)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(82, 29)
        Me.btnOk.TabIndex = 11
        Me.btnOk.Tag = "btnOK1"
        Me.btnOk.Text = "btnOk"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'grbTransAssign
        '
        Me.grbTransAssign.Controls.Add(Me.txtDM3)
        Me.grbTransAssign.Controls.Add(Me.chkDM3)
        Me.grbTransAssign.Controls.Add(Me.txtDM2)
        Me.grbTransAssign.Controls.Add(Me.chkDM2)
        Me.grbTransAssign.Controls.Add(Me.txtDM1)
        Me.grbTransAssign.Controls.Add(Me.chkDM1)
        Me.grbTransAssign.Controls.Add(Me.txtGD)
        Me.grbTransAssign.Controls.Add(Me.chkGD)
        Me.grbTransAssign.Location = New System.Drawing.Point(288, 185)
        Me.grbTransAssign.Name = "grbTransAssign"
        Me.grbTransAssign.Size = New System.Drawing.Size(232, 171)
        Me.grbTransAssign.TabIndex = 10
        Me.grbTransAssign.TabStop = False
        Me.grbTransAssign.Tag = "grbTransAssign"
        Me.grbTransAssign.Text = "grbTransAssign"
        '
        'txtDM3
        '
        Me.txtDM3.FieldType = Sats.AppCore.FlexMaskEditBox._FieldType.NUMERIC
        Me.txtDM3.Location = New System.Drawing.Point(114, 140)
        Me.txtDM3.Mask = "###,###,###"
        Me.txtDM3.Name = "txtDM3"
        Me.txtDM3.PromptChar = "_"
        Me.txtDM3.Size = New System.Drawing.Size(112, 20)
        Me.txtDM3.TabIndex = 7
        Me.txtDM3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'chkDM3
        '
        Me.chkDM3.AutoSize = True
        Me.chkDM3.Location = New System.Drawing.Point(11, 142)
        Me.chkDM3.Name = "chkDM3"
        Me.chkDM3.Size = New System.Drawing.Size(67, 17)
        Me.chkDM3.TabIndex = 6
        Me.chkDM3.Tag = "chkDM1"
        Me.chkDM3.Text = "chkDM1"
        Me.chkDM3.UseVisualStyleBackColor = True
        '
        'txtDM2
        '
        Me.txtDM2.FieldType = Sats.AppCore.FlexMaskEditBox._FieldType.NUMERIC
        Me.txtDM2.Location = New System.Drawing.Point(114, 105)
        Me.txtDM2.Mask = "###,###,###"
        Me.txtDM2.Name = "txtDM2"
        Me.txtDM2.PromptChar = "_"
        Me.txtDM2.Size = New System.Drawing.Size(112, 20)
        Me.txtDM2.TabIndex = 5
        Me.txtDM2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'chkDM2
        '
        Me.chkDM2.AutoSize = True
        Me.chkDM2.Location = New System.Drawing.Point(11, 107)
        Me.chkDM2.Name = "chkDM2"
        Me.chkDM2.Size = New System.Drawing.Size(67, 17)
        Me.chkDM2.TabIndex = 4
        Me.chkDM2.Tag = "chkDM2"
        Me.chkDM2.Text = "chkDM2"
        Me.chkDM2.UseVisualStyleBackColor = True
        '
        'txtDM1
        '
        Me.txtDM1.FieldType = Sats.AppCore.FlexMaskEditBox._FieldType.NUMERIC
        Me.txtDM1.Location = New System.Drawing.Point(114, 69)
        Me.txtDM1.Mask = "###,###,###"
        Me.txtDM1.Name = "txtDM1"
        Me.txtDM1.PromptChar = "_"
        Me.txtDM1.Size = New System.Drawing.Size(112, 20)
        Me.txtDM1.TabIndex = 3
        Me.txtDM1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'chkDM1
        '
        Me.chkDM1.AutoSize = True
        Me.chkDM1.Location = New System.Drawing.Point(11, 71)
        Me.chkDM1.Name = "chkDM1"
        Me.chkDM1.Size = New System.Drawing.Size(67, 17)
        Me.chkDM1.TabIndex = 2
        Me.chkDM1.Tag = "chkDM1"
        Me.chkDM1.Text = "chkDM1"
        Me.chkDM1.UseVisualStyleBackColor = True
        '
        'txtGD
        '
        Me.txtGD.FieldType = Sats.AppCore.FlexMaskEditBox._FieldType.NUMERIC
        Me.txtGD.Location = New System.Drawing.Point(114, 34)
        Me.txtGD.Mask = "###,###,###"
        Me.txtGD.Name = "txtGD"
        Me.txtGD.PromptChar = "_"
        Me.txtGD.Size = New System.Drawing.Size(112, 20)
        Me.txtGD.TabIndex = 1
        Me.txtGD.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'chkGD
        '
        Me.chkGD.AutoSize = True
        Me.chkGD.Location = New System.Drawing.Point(11, 36)
        Me.chkGD.Name = "chkGD"
        Me.chkGD.Size = New System.Drawing.Size(60, 17)
        Me.chkGD.TabIndex = 0
        Me.chkGD.Tag = "chkGD"
        Me.chkGD.Text = "chkGD"
        Me.chkGD.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.SystemColors.Desktop
        Me.Panel1.Controls.Add(Me.lbCaption)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(534, 50)
        Me.Panel1.TabIndex = 8
        '
        'lbCaption
        '
        Me.lbCaption.AutoSize = True
        Me.lbCaption.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lbCaption.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lbCaption.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lbCaption.Location = New System.Drawing.Point(12, 19)
        Me.lbCaption.Name = "lbCaption"
        Me.lbCaption.Size = New System.Drawing.Size(60, 13)
        Me.lbCaption.TabIndex = 0
        Me.lbCaption.Text = "lbCaption"
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
        'stvTrans
        '
        Me.stvTrans.Dock = System.Windows.Forms.DockStyle.Left
        Me.stvTrans.Items.AddRange(New Xceed.SmartUI.SmartItem() {Me.Node1})
        Me.stvTrans.ItemsImageList = Me.imgList
        Me.stvTrans.Location = New System.Drawing.Point(0, 50)
        Me.stvTrans.Name = "stvTrans"
        Me.stvTrans.Size = New System.Drawing.Size(282, 350)
        Me.stvTrans.TabIndex = 14
        Me.stvTrans.Text = "stvTrans"
        '
        'Node1
        '
        Me.Node1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
        Me.Node1.ImageIndex = 2
        Me.Node1.Text = "Sats - Transaction"
        '
        'lbBrID
        '
        Me.lbBrID.AutoSize = True
        Me.lbBrID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbBrID.ForeColor = System.Drawing.Color.DarkRed
        Me.lbBrID.Location = New System.Drawing.Point(285, 71)
        Me.lbBrID.Name = "lbBrID"
        Me.lbBrID.Size = New System.Drawing.Size(42, 13)
        Me.lbBrID.TabIndex = 16
        Me.lbBrID.Tag = "lbBrID"
        Me.lbBrID.Text = "lbBrID"
        '
        'lstBrID
        '
        Me.lstBrID.FormattingEnabled = True
        Me.lstBrID.Location = New System.Drawing.Point(295, 87)
        Me.lstBrID.Name = "lstBrID"
        Me.lstBrID.Size = New System.Drawing.Size(225, 69)
        Me.lstBrID.TabIndex = 15
        Me.lstBrID.Tag = "lbBrID"
        '
        'frmTranAssign
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(534, 400)
        Me.Controls.Add(Me.lbBrID)
        Me.Controls.Add(Me.lstBrID)
        Me.Controls.Add(Me.stvTrans)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.lbTrans)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.grbTransAssign)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmTranAssign"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "frmTranAssign"
        Me.grbTransAssign.ResumeLayout(False)
        Me.grbTransAssign.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lbTrans As System.Windows.Forms.Label
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents grbTransAssign As System.Windows.Forms.GroupBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents lbCaption As System.Windows.Forms.Label
    Friend WithEvents imgList As System.Windows.Forms.ImageList
    Friend WithEvents stvTrans As Xceed.SmartUI.Controls.TreeView.SmartTreeView
    Friend WithEvents Node1 As Xceed.SmartUI.Controls.TreeView.Node
    Friend WithEvents txtDM3 As Sats.AppCore.FlexMaskEditBox
    Friend WithEvents chkDM3 As System.Windows.Forms.CheckBox
    Friend WithEvents txtDM2 As Sats.AppCore.FlexMaskEditBox
    Friend WithEvents chkDM2 As System.Windows.Forms.CheckBox
    Friend WithEvents txtDM1 As Sats.AppCore.FlexMaskEditBox
    Friend WithEvents chkDM1 As System.Windows.Forms.CheckBox
    Friend WithEvents txtGD As Sats.AppCore.FlexMaskEditBox
    Friend WithEvents chkGD As System.Windows.Forms.CheckBox
    Friend WithEvents lbBrID As System.Windows.Forms.Label
    Friend WithEvents lstBrID As System.Windows.Forms.ListBox
End Class
