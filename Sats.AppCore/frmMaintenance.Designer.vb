<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMaintenance
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
        Me.lbCaption = New System.Windows.Forms.Label
        Me.btnOk = New System.Windows.Forms.Button
        Me.btnApply = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.SystemColors.Desktop
        Me.Panel1.Controls.Add(Me.lbCaption)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(790, 42)
        Me.Panel1.TabIndex = 2
        Me.Panel1.Tag = "lbCaption"
        '
        'lbCaption
        '
        Me.lbCaption.AutoSize = True
        Me.lbCaption.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbCaption.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lbCaption.Location = New System.Drawing.Point(12, 14)
        Me.lbCaption.Name = "lbCaption"
        Me.lbCaption.Size = New System.Drawing.Size(60, 13)
        Me.lbCaption.TabIndex = 0
        Me.lbCaption.Tag = "lbCaption"
        Me.lbCaption.Text = "lbCaption"
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(529, 476)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(80, 25)
        Me.btnOk.TabIndex = 1
        Me.btnOk.Tag = "btnOK"
        Me.btnOk.Text = "btnOk"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'btnApply
        '
        Me.btnApply.Location = New System.Drawing.Point(615, 476)
        Me.btnApply.Name = "btnApply"
        Me.btnApply.Size = New System.Drawing.Size(80, 25)
        Me.btnApply.TabIndex = 2
        Me.btnApply.Tag = "btnApply"
        Me.btnApply.Text = "btnApply"
        Me.btnApply.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(701, 476)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(80, 25)
        Me.btnCancel.TabIndex = 3
        Me.btnCancel.Tag = "btnCancel"
        Me.btnCancel.Text = "btnCancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'frmMaintenance
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(790, 509)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnApply)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.Panel1)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmMaintenance"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Protected WithEvents Panel1 As System.Windows.Forms.Panel
    Protected WithEvents lbCaption As System.Windows.Forms.Label
    Protected WithEvents btnOk As System.Windows.Forms.Button
    Protected WithEvents btnApply As System.Windows.Forms.Button
    Protected WithEvents btnCancel As System.Windows.Forms.Button
End Class
