<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSelectVsdBrid
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSelectVsdBrid))
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnOk = New System.Windows.Forms.Button
        Me.grbInfo = New System.Windows.Forms.GroupBox
        Me.cboVsdBrid = New Sats.AppCore.ComboBoxEx
        Me.lbVsdBrid = New System.Windows.Forms.Label
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.lbCaption = New System.Windows.Forms.Label
        Me.grbInfo.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(240, 122)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 35)
        Me.btnCancel.TabIndex = 13
        Me.btnCancel.Tag = "btnCancel"
        Me.btnCancel.Text = "Bỏ qua"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(155, 122)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(79, 35)
        Me.btnOk.TabIndex = 12
        Me.btnOk.Tag = "btnOk"
        Me.btnOk.Text = "Chấp nhận"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'grbInfo
        '
        Me.grbInfo.Controls.Add(Me.cboVsdBrid)
        Me.grbInfo.Controls.Add(Me.lbVsdBrid)
        Me.grbInfo.Location = New System.Drawing.Point(7, 53)
        Me.grbInfo.Name = "grbInfo"
        Me.grbInfo.Size = New System.Drawing.Size(446, 60)
        Me.grbInfo.TabIndex = 11
        Me.grbInfo.TabStop = False
        Me.grbInfo.Tag = "grbInfo"
        Me.grbInfo.Text = "Nơi nhận chứng từ gốc"
        '
        'cboVsdBrid
        '
        Me.cboVsdBrid.DisplayMember = "DISPLAY"
        Me.cboVsdBrid.FormattingEnabled = True
        Me.cboVsdBrid.Location = New System.Drawing.Point(91, 21)
        Me.cboVsdBrid.Name = "cboVsdBrid"
        Me.cboVsdBrid.Size = New System.Drawing.Size(342, 21)
        Me.cboVsdBrid.TabIndex = 1
        Me.cboVsdBrid.Tag = "cboVsdBrid"
        Me.cboVsdBrid.ValueMember = "VALUE"
        '
        'lbVsdBrid
        '
        Me.lbVsdBrid.AutoSize = True
        Me.lbVsdBrid.Location = New System.Drawing.Point(12, 24)
        Me.lbVsdBrid.Name = "lbVsdBrid"
        Me.lbVsdBrid.Size = New System.Drawing.Size(55, 13)
        Me.lbVsdBrid.TabIndex = 0
        Me.lbVsdBrid.Tag = "lbVsdBrid"
        Me.lbVsdBrid.Text = "Chi nhánh"
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.SystemColors.Desktop
        Me.Panel1.Controls.Add(Me.lbCaption)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(453, 47)
        Me.Panel1.TabIndex = 10
        '
        'lbCaption
        '
        Me.lbCaption.AutoSize = True
        Me.lbCaption.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbCaption.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lbCaption.Location = New System.Drawing.Point(23, 13)
        Me.lbCaption.Name = "lbCaption"
        Me.lbCaption.Size = New System.Drawing.Size(253, 13)
        Me.lbCaption.TabIndex = 0
        Me.lbCaption.Tag = "lbCaption"
        Me.lbCaption.Text = "Chọn nơi nhận chứng từ gốc của giao dịch."
        '
        'frmSelectVsdBrid
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(453, 163)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.grbInfo)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "frmSelectVsdBrid"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Nơi nhận chứng từ gốc của giao dịch"
        Me.grbInfo.ResumeLayout(False)
        Me.grbInfo.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents grbInfo As System.Windows.Forms.GroupBox
    Friend WithEvents cboVsdBrid As Sats.AppCore.ComboBoxEx
    Friend WithEvents lbVsdBrid As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents lbCaption As System.Windows.Forms.Label
End Class
