<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUpLoadUpdateFile
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
        Me.chlFile = New System.Windows.Forms.CheckedListBox
        Me.txtPath = New System.Windows.Forms.TextBox
        Me.lblPath = New System.Windows.Forms.Label
        Me.lblListFile = New System.Windows.Forms.Label
        Me.btnPath = New System.Windows.Forms.Button
        Me.btnOK = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.lblProcess = New System.Windows.Forms.Label
        Me.prc = New System.Windows.Forms.ProgressBar
        Me.btnReport = New System.Windows.Forms.Button
        Me.Button1 = New System.Windows.Forms.Button
        Me.lblServer = New System.Windows.Forms.Label
        Me.txtServer = New System.Windows.Forms.TextBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.SuspendLayout()
        '
        'chlFile
        '
        Me.chlFile.FormattingEnabled = True
        Me.chlFile.Location = New System.Drawing.Point(12, 107)
        Me.chlFile.Name = "chlFile"
        Me.chlFile.Size = New System.Drawing.Size(269, 229)
        Me.chlFile.TabIndex = 0
        '
        'txtPath
        '
        Me.txtPath.Location = New System.Drawing.Point(82, 60)
        Me.txtPath.Name = "txtPath"
        Me.txtPath.ReadOnly = True
        Me.txtPath.Size = New System.Drawing.Size(235, 20)
        Me.txtPath.TabIndex = 1
        '
        'lblPath
        '
        Me.lblPath.AutoSize = True
        Me.lblPath.Location = New System.Drawing.Point(11, 64)
        Me.lblPath.Name = "lblPath"
        Me.lblPath.Size = New System.Drawing.Size(60, 13)
        Me.lblPath.TabIndex = 2
        Me.lblPath.Text = "Đường dẫn"
        '
        'lblListFile
        '
        Me.lblListFile.AutoSize = True
        Me.lblListFile.Location = New System.Drawing.Point(9, 91)
        Me.lblListFile.Name = "lblListFile"
        Me.lblListFile.Size = New System.Drawing.Size(91, 13)
        Me.lblListFile.TabIndex = 3
        Me.lblListFile.Text = "Danh sách tệp tin"
        '
        'btnPath
        '
        Me.btnPath.Location = New System.Drawing.Point(323, 57)
        Me.btnPath.Name = "btnPath"
        Me.btnPath.Size = New System.Drawing.Size(36, 26)
        Me.btnPath.TabIndex = 4
        Me.btnPath.Text = "..."
        Me.btnPath.UseVisualStyleBackColor = True
        '
        'btnOK
        '
        Me.btnOK.Enabled = False
        Me.btnOK.Location = New System.Drawing.Point(287, 240)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 28)
        Me.btnOK.TabIndex = 5
        Me.btnOK.Text = "&Chấp nhận"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(287, 308)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 28)
        Me.btnCancel.TabIndex = 6
        Me.btnCancel.Text = "&Thoát"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'lblProcess
        '
        Me.lblProcess.AutoSize = True
        Me.lblProcess.Location = New System.Drawing.Point(12, 339)
        Me.lblProcess.Name = "lblProcess"
        Me.lblProcess.Size = New System.Drawing.Size(55, 13)
        Me.lblProcess.TabIndex = 7
        Me.lblProcess.Text = "lblProcess"
        '
        'prc
        '
        Me.prc.Location = New System.Drawing.Point(12, 355)
        Me.prc.Name = "prc"
        Me.prc.Size = New System.Drawing.Size(350, 23)
        Me.prc.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.prc.TabIndex = 8
        '
        'btnReport
        '
        Me.btnReport.Enabled = False
        Me.btnReport.Location = New System.Drawing.Point(287, 274)
        Me.btnReport.Name = "btnReport"
        Me.btnReport.Size = New System.Drawing.Size(75, 28)
        Me.btnReport.TabIndex = 9
        Me.btnReport.Text = "Cập nhật &BC"
        Me.btnReport.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(323, 9)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(36, 26)
        Me.Button1.TabIndex = 12
        Me.Button1.Text = "..."
        Me.Button1.UseVisualStyleBackColor = True
        '
        'lblServer
        '
        Me.lblServer.AutoSize = True
        Me.lblServer.Location = New System.Drawing.Point(11, 16)
        Me.lblServer.Name = "lblServer"
        Me.lblServer.Size = New System.Drawing.Size(38, 13)
        Me.lblServer.TabIndex = 11
        Me.lblServer.Text = "Server"
        '
        'txtServer
        '
        Me.txtServer.Location = New System.Drawing.Point(82, 12)
        Me.txtServer.Name = "txtServer"
        Me.txtServer.ReadOnly = True
        Me.txtServer.Size = New System.Drawing.Size(235, 20)
        Me.txtServer.TabIndex = 10
        '
        'GroupBox1
        '
        Me.GroupBox1.Location = New System.Drawing.Point(20, 41)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(325, 4)
        Me.GroupBox1.TabIndex = 13
        Me.GroupBox1.TabStop = False
        '
        'frmUpLoadUpdateFile
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(368, 386)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.lblServer)
        Me.Controls.Add(Me.txtServer)
        Me.Controls.Add(Me.btnReport)
        Me.Controls.Add(Me.prc)
        Me.Controls.Add(Me.lblProcess)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.btnPath)
        Me.Controls.Add(Me.lblListFile)
        Me.Controls.Add(Me.lblPath)
        Me.Controls.Add(Me.txtPath)
        Me.Controls.Add(Me.chlFile)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmUpLoadUpdateFile"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Cập nhật file lên server"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents chlFile As System.Windows.Forms.CheckedListBox
    Friend WithEvents txtPath As System.Windows.Forms.TextBox
    Friend WithEvents lblPath As System.Windows.Forms.Label
    Friend WithEvents lblListFile As System.Windows.Forms.Label
    Friend WithEvents btnPath As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents lblProcess As System.Windows.Forms.Label
    Friend WithEvents prc As System.Windows.Forms.ProgressBar
    Friend WithEvents btnReport As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents lblServer As System.Windows.Forms.Label
    Friend WithEvents txtServer As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox

End Class
