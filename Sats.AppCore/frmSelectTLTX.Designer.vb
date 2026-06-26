<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSelectTLTX
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSelectTLTX))
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.lbCaption = New System.Windows.Forms.Label
        Me.grbInfo = New System.Windows.Forms.GroupBox
        Me.cboTLTX = New Sats.AppCore.ComboBoxEx
        Me.lbTLTX = New System.Windows.Forms.Label
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnOk = New System.Windows.Forms.Button
        Me.Panel1.SuspendLayout()
        Me.grbInfo.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.SystemColors.Desktop
        Me.Panel1.Controls.Add(Me.lbCaption)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(458, 37)
        Me.Panel1.TabIndex = 5
        '
        'lbCaption
        '
        Me.lbCaption.AutoSize = True
        Me.lbCaption.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbCaption.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lbCaption.Location = New System.Drawing.Point(23, 13)
        Me.lbCaption.Name = "lbCaption"
        Me.lbCaption.Size = New System.Drawing.Size(403, 13)
        Me.lbCaption.TabIndex = 0
        Me.lbCaption.Tag = "lbCaption"
        Me.lbCaption.Text = "Giao dịch được chấp nhận. Bạn có muốn nhập tiếp giao dịch không ?"
        '
        'grbInfo
        '
        Me.grbInfo.Controls.Add(Me.cboTLTX)
        Me.grbInfo.Controls.Add(Me.lbTLTX)
        Me.grbInfo.Location = New System.Drawing.Point(3, 46)
        Me.grbInfo.Name = "grbInfo"
        Me.grbInfo.Size = New System.Drawing.Size(446, 60)
        Me.grbInfo.TabIndex = 6
        Me.grbInfo.TabStop = False
        Me.grbInfo.Tag = "grbInfo"
        Me.grbInfo.Text = "Các giao dịch "
        '
        'cboTLTX
        '
        Me.cboTLTX.DisplayMember = "DISPLAY"
        Me.cboTLTX.FormattingEnabled = True
        Me.cboTLTX.Location = New System.Drawing.Point(91, 21)
        Me.cboTLTX.Name = "cboTLTX"
        Me.cboTLTX.Size = New System.Drawing.Size(342, 21)
        Me.cboTLTX.TabIndex = 1
        Me.cboTLTX.Tag = "cboSysPart"
        Me.cboTLTX.ValueMember = "VALUE"
        '
        'lbTLTX
        '
        Me.lbTLTX.AutoSize = True
        Me.lbTLTX.Location = New System.Drawing.Point(12, 24)
        Me.lbTLTX.Name = "lbTLTX"
        Me.lbTLTX.Size = New System.Drawing.Size(68, 13)
        Me.lbTLTX.TabIndex = 0
        Me.lbTLTX.Tag = "lbSysPart"
        Me.lbTLTX.Text = "Mã giao dịch"
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(236, 115)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 35)
        Me.btnCancel.TabIndex = 9
        Me.btnCancel.Tag = "btnCancel"
        Me.btnCancel.Text = "Bỏ qua"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(151, 115)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(79, 35)
        Me.btnOk.TabIndex = 8
        Me.btnOk.Tag = "btnOk"
        Me.btnOk.Text = "Chấp nhận"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'frmSelectTLTX
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(458, 159)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.grbInfo)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmSelectTLTX"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Thông báo"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.grbInfo.ResumeLayout(False)
        Me.grbInfo.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents lbCaption As System.Windows.Forms.Label
    Friend WithEvents grbInfo As System.Windows.Forms.GroupBox
    Friend WithEvents cboTLTX As Sats.AppCore.ComboBoxEx
    Friend WithEvents lbTLTX As System.Windows.Forms.Label
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOk As System.Windows.Forms.Button
End Class
