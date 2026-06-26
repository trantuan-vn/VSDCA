<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTransaction
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
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.mskTransCode = New Sats.AppCore.FlexMaskEditBox
        Me.lblTransCaption = New System.Windows.Forms.Label
        Me.lblTranCode = New System.Windows.Forms.Label
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.pnTransDetail = New System.Windows.Forms.Panel
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnOK = New System.Windows.Forms.Button
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.SystemColors.Desktop
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.mskTransCode)
        Me.Panel1.Controls.Add(Me.lblTransCaption)
        Me.Panel1.Controls.Add(Me.lblTranCode)
        Me.Panel1.Controls.Add(Me.Panel2)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(632, 53)
        Me.Panel1.TabIndex = 0
        '
        'mskTransCode
        '
        Me.mskTransCode.Location = New System.Drawing.Point(83, 18)
        Me.mskTransCode.Mask = "9999"
        Me.mskTransCode.Name = "mskTransCode"
        Me.mskTransCode.Size = New System.Drawing.Size(109, 20)
        Me.mskTransCode.TabIndex = 5
        '
        'lblTransCaption
        '
        Me.lblTransCaption.AutoSize = True
        Me.lblTransCaption.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblTransCaption.Location = New System.Drawing.Point(220, 20)
        Me.lblTransCaption.Name = "lblTransCaption"
        Me.lblTransCaption.Size = New System.Drawing.Size(80, 13)
        Me.lblTransCaption.TabIndex = 4
        Me.lblTransCaption.Text = "lblTransCaption"
        '
        'lblTranCode
        '
        Me.lblTranCode.AutoSize = True
        Me.lblTranCode.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblTranCode.Location = New System.Drawing.Point(10, 20)
        Me.lblTranCode.Name = "lblTranCode"
        Me.lblTranCode.Size = New System.Drawing.Size(54, 13)
        Me.lblTranCode.TabIndex = 2
        Me.lblTranCode.Text = "TranCode"
        '
        'Panel2
        '
        Me.Panel2.Location = New System.Drawing.Point(2, 58)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(625, 274)
        Me.Panel2.TabIndex = 1
        '
        'pnTransDetail
        '
        Me.pnTransDetail.BackColor = System.Drawing.SystemColors.Control
        Me.pnTransDetail.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnTransDetail.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnTransDetail.Location = New System.Drawing.Point(0, 53)
        Me.pnTransDetail.Name = "pnTransDetail"
        Me.pnTransDetail.Size = New System.Drawing.Size(632, 328)
        Me.pnTransDetail.TabIndex = 1
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(96, 387)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(85, 28)
        Me.btnCancel.TabIndex = 2
        Me.btnCancel.Text = "btnCancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(3, 387)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(85, 28)
        Me.btnOK.TabIndex = 2
        Me.btnOK.Text = "btnOK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'frmTransaction
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(632, 419)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.pnTransDetail)
        Me.Controls.Add(Me.Panel1)
        Me.KeyPreview = True
        Me.Name = "frmTransaction"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.TabText = "frmTransaction"
        Me.Text = "frmTransaction"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents pnTransDetail As System.Windows.Forms.Panel
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents lblTranCode As System.Windows.Forms.Label
    Friend WithEvents lblTransCaption As System.Windows.Forms.Label
    Friend WithEvents mskTransCode As Sats.AppCore.FlexMaskEditBox
End Class
