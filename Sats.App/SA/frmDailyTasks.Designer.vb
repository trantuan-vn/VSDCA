<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDailyTasks
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
        Me.txtTranMessage = New System.Windows.Forms.TextBox
        Me.btnOK = New System.Windows.Forms.Button
        Me.btnDetails = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'txtTranMessage
        '
        Me.txtTranMessage.Dock = System.Windows.Forms.DockStyle.Top
        Me.txtTranMessage.Location = New System.Drawing.Point(0, 0)
        Me.txtTranMessage.Multiline = True
        Me.txtTranMessage.Name = "txtTranMessage"
        Me.txtTranMessage.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtTranMessage.Size = New System.Drawing.Size(368, 318)
        Me.txtTranMessage.TabIndex = 8
        '
        'btnOK
        '
        Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOK.Location = New System.Drawing.Point(201, 324)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(104, 42)
        Me.btnOK.TabIndex = 7
        Me.btnOK.Text = "&Thoát"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnDetails
        '
        Me.btnDetails.Location = New System.Drawing.Point(56, 324)
        Me.btnDetails.Name = "btnDetails"
        Me.btnDetails.Size = New System.Drawing.Size(111, 42)
        Me.btnDetails.TabIndex = 9
        Me.btnDetails.Text = "&Chi tiết GD chưa XN"
        Me.btnDetails.UseVisualStyleBackColor = True
        '
        'frmDailyTasks
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(368, 375)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnDetails)
        Me.Controls.Add(Me.txtTranMessage)
        Me.Controls.Add(Me.btnOK)
        Me.MaximizeBox = False
        Me.Name = "frmDailyTasks"
        Me.ShowInTaskbar = False
        Me.Text = "Các công việc trong ngày"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtTranMessage As System.Windows.Forms.TextBox
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnDetails As System.Windows.Forms.Button
End Class
