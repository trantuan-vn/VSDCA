<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmErrMsg
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
        Me.btnExport = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.tbMessages = New System.Windows.Forms.TabControl
        Me.tbErrors = New System.Windows.Forms.TabPage
        Me.tpWarnings = New System.Windows.Forms.TabPage
        Me.txtErrMsg = New System.Windows.Forms.TextBox
        Me.btnOK = New System.Windows.Forms.Button
        Me.txtWarningMsg = New System.Windows.Forms.TextBox
        Me.tbMessages.SuspendLayout()
        Me.tbErrors.SuspendLayout()
        Me.tpWarnings.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(43, 370)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(74, 31)
        Me.btnExport.TabIndex = 1
        Me.btnExport.Text = "Kết xuất"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnExit.Location = New System.Drawing.Point(214, 370)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(75, 31)
        Me.btnExit.TabIndex = 2
        Me.btnExit.Text = "Thoát"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'tbMessages
        '
        Me.tbMessages.Controls.Add(Me.tbErrors)
        Me.tbMessages.Controls.Add(Me.tpWarnings)
        Me.tbMessages.Dock = System.Windows.Forms.DockStyle.Top
        Me.tbMessages.Location = New System.Drawing.Point(0, 0)
        Me.tbMessages.Name = "tbMessages"
        Me.tbMessages.SelectedIndex = 0
        Me.tbMessages.Size = New System.Drawing.Size(331, 351)
        Me.tbMessages.TabIndex = 3
        '
        'tbErrors
        '
        Me.tbErrors.Controls.Add(Me.txtErrMsg)
        Me.tbErrors.Location = New System.Drawing.Point(4, 22)
        Me.tbErrors.Name = "tbErrors"
        Me.tbErrors.Padding = New System.Windows.Forms.Padding(3)
        Me.tbErrors.Size = New System.Drawing.Size(323, 325)
        Me.tbErrors.TabIndex = 0
        Me.tbErrors.Text = "0 lỗi"
        Me.tbErrors.UseVisualStyleBackColor = True
        '
        'tpWarnings
        '
        Me.tpWarnings.Controls.Add(Me.txtWarningMsg)
        Me.tpWarnings.Location = New System.Drawing.Point(4, 22)
        Me.tpWarnings.Name = "tpWarnings"
        Me.tpWarnings.Padding = New System.Windows.Forms.Padding(3)
        Me.tpWarnings.Size = New System.Drawing.Size(323, 325)
        Me.tpWarnings.TabIndex = 1
        Me.tpWarnings.Text = "0 cảnh báo"
        Me.tpWarnings.UseVisualStyleBackColor = True
        '
        'txtErrMsg
        '
        Me.txtErrMsg.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtErrMsg.Location = New System.Drawing.Point(3, 3)
        Me.txtErrMsg.Multiline = True
        Me.txtErrMsg.Name = "txtErrMsg"
        Me.txtErrMsg.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtErrMsg.Size = New System.Drawing.Size(317, 319)
        Me.txtErrMsg.TabIndex = 1
        '
        'btnOK
        '
        Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOK.Location = New System.Drawing.Point(127, 370)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 31)
        Me.btnOK.TabIndex = 4
        Me.btnOK.Text = "Chấp nhận"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'txtWarningMsg
        '
        Me.txtWarningMsg.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtWarningMsg.Location = New System.Drawing.Point(3, 3)
        Me.txtWarningMsg.Multiline = True
        Me.txtWarningMsg.Name = "txtWarningMsg"
        Me.txtWarningMsg.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtWarningMsg.Size = New System.Drawing.Size(317, 319)
        Me.txtWarningMsg.TabIndex = 2
        '
        'frmErrMsg
        '
        Me.AcceptButton = Me.btnExport
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnExit
        Me.ClientSize = New System.Drawing.Size(331, 413)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.tbMessages)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.btnExport)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmErrMsg"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Danh sách thông báo : "
        Me.tbMessages.ResumeLayout(False)
        Me.tbErrors.ResumeLayout(False)
        Me.tbErrors.PerformLayout()
        Me.tpWarnings.ResumeLayout(False)
        Me.tpWarnings.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents tbMessages As System.Windows.Forms.TabControl
    Friend WithEvents tbErrors As System.Windows.Forms.TabPage
    Friend WithEvents txtErrMsg As System.Windows.Forms.TextBox
    Friend WithEvents tpWarnings As System.Windows.Forms.TabPage
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents txtWarningMsg As System.Windows.Forms.TextBox
End Class
