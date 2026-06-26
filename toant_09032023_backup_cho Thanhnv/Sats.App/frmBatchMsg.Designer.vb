<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBatchMsg
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
        Me.txtErrMsg = New System.Windows.Forms.TextBox
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnExport = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'txtErrMsg
        '
        Me.txtErrMsg.Dock = System.Windows.Forms.DockStyle.Top
        Me.txtErrMsg.Location = New System.Drawing.Point(0, 0)
        Me.txtErrMsg.Multiline = True
        Me.txtErrMsg.Name = "txtErrMsg"
        Me.txtErrMsg.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtErrMsg.Size = New System.Drawing.Size(692, 319)
        Me.txtErrMsg.TabIndex = 2
        '
        'btnExit
        '
        Me.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnExit.Location = New System.Drawing.Point(346, 331)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(75, 31)
        Me.btnExit.TabIndex = 4
        Me.btnExit.Text = "Thoát"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(254, 331)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(74, 31)
        Me.btnExport.TabIndex = 3
        Me.btnExport.Text = "Kết xuất"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'frmBatchMsg
        '
        Me.AcceptButton = Me.btnExport
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnExit
        Me.ClientSize = New System.Drawing.Size(692, 373)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.btnExport)
        Me.Controls.Add(Me.txtErrMsg)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmBatchMsg"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Thông báo"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtErrMsg As System.Windows.Forms.TextBox
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
End Class
