<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTranMessage
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
        Me.btnOK = New System.Windows.Forms.Button
        Me.txtTranMessage = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'btnOK
        '
        Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOK.Location = New System.Drawing.Point(124, 327)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 31)
        Me.btnOK.TabIndex = 5
        Me.btnOK.Text = "&Thoát"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'txtTranMessage
        '
        Me.txtTranMessage.Dock = System.Windows.Forms.DockStyle.Top
        Me.txtTranMessage.Location = New System.Drawing.Point(0, 0)
        Me.txtTranMessage.Multiline = True
        Me.txtTranMessage.Name = "txtTranMessage"
        Me.txtTranMessage.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtTranMessage.Size = New System.Drawing.Size(350, 318)
        Me.txtTranMessage.TabIndex = 6
        '
        'frmTranMessage
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(350, 367)
        Me.Controls.Add(Me.txtTranMessage)
        Me.Controls.Add(Me.btnOK)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmTranMessage"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Thông báo kết quả thực hiện giao dịch"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents txtTranMessage As System.Windows.Forms.TextBox
End Class
