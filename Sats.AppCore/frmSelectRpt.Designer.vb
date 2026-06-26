<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSelectRpt
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSelectRpt))
        Me.btnCancel = New System.Windows.Forms.Button
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.lbCaption = New System.Windows.Forms.Label
        Me.cboRptId = New Sats.AppCore.ComboBoxEx
        Me.btnOk = New System.Windows.Forms.Button
        Me.grbInfo = New System.Windows.Forms.GroupBox
        Me.lbRptId = New System.Windows.Forms.Label
        Me.Panel1.SuspendLayout()
        Me.grbInfo.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(240, 137)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 35)
        Me.btnCancel.TabIndex = 17
        Me.btnCancel.Tag = "btnCancel"
        Me.btnCancel.Text = "Thoát"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.SystemColors.Desktop
        Me.Panel1.Controls.Add(Me.lbCaption)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(465, 47)
        Me.Panel1.TabIndex = 14
        '
        'lbCaption
        '
        Me.lbCaption.AutoSize = True
        Me.lbCaption.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbCaption.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lbCaption.Location = New System.Drawing.Point(23, 13)
        Me.lbCaption.Name = "lbCaption"
        Me.lbCaption.Size = New System.Drawing.Size(231, 13)
        Me.lbCaption.TabIndex = 0
        Me.lbCaption.Tag = "lbCaption"
        Me.lbCaption.Text = "Chọn In báo cáo liên quan tới giao dịch"
        '
        'cboRptId
        '
        Me.cboRptId.DisplayMember = "DISPLAY"
        Me.cboRptId.FormattingEnabled = True
        Me.cboRptId.Location = New System.Drawing.Point(91, 21)
        Me.cboRptId.Name = "cboRptId"
        Me.cboRptId.Size = New System.Drawing.Size(342, 21)
        Me.cboRptId.TabIndex = 1
        Me.cboRptId.Tag = "cboRptId"
        Me.cboRptId.ValueMember = "VALUE"
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(155, 137)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(79, 35)
        Me.btnOk.TabIndex = 16
        Me.btnOk.Tag = "btnOk"
        Me.btnOk.Text = "Chấp nhận"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'grbInfo
        '
        Me.grbInfo.Controls.Add(Me.cboRptId)
        Me.grbInfo.Controls.Add(Me.lbRptId)
        Me.grbInfo.Location = New System.Drawing.Point(7, 68)
        Me.grbInfo.Name = "grbInfo"
        Me.grbInfo.Size = New System.Drawing.Size(446, 60)
        Me.grbInfo.TabIndex = 15
        Me.grbInfo.TabStop = False
        Me.grbInfo.Tag = "grbInfo"
        Me.grbInfo.Text = "Danh sách báo cáo"
        '
        'lbRptId
        '
        Me.lbRptId.AutoSize = True
        Me.lbRptId.Location = New System.Drawing.Point(12, 24)
        Me.lbRptId.Name = "lbRptId"
        Me.lbRptId.Size = New System.Drawing.Size(68, 13)
        Me.lbRptId.TabIndex = 0
        Me.lbRptId.Tag = "lbRptId"
        Me.lbRptId.Text = "Tên báo cáo"
        '
        'frmSelectRpt
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(465, 180)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.grbInfo)
        Me.Name = "frmSelectRpt"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Chọn in báo cáo liên quan tới giao dịch"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.grbInfo.ResumeLayout(False)
        Me.grbInfo.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents lbCaption As System.Windows.Forms.Label
    Friend WithEvents cboRptId As Sats.AppCore.ComboBoxEx
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents grbInfo As System.Windows.Forms.GroupBox
    Friend WithEvents lbRptId As System.Windows.Forms.Label
End Class
