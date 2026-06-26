<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmFTPClient
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
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnOk = New System.Windows.Forms.Button
        Me.grbInfo = New System.Windows.Forms.GroupBox
        Me.lbNote = New System.Windows.Forms.Label
        Me.lbMessage = New System.Windows.Forms.Label
        Me.btnBrowse = New System.Windows.Forms.Button
        Me.txtFileLocation = New System.Windows.Forms.TextBox
        Me.lbFileLocation = New System.Windows.Forms.Label
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.lbCaption = New System.Windows.Forms.Label
        Me.grbInfo.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(274, 218)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 35)
        Me.btnCancel.TabIndex = 11
        Me.btnCancel.Tag = "btnCancel"
        Me.btnCancel.Text = "Thoát"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(189, 218)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(79, 35)
        Me.btnOk.TabIndex = 10
        Me.btnOk.Tag = "btnOk"
        Me.btnOk.Text = "Lưu file"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'grbInfo
        '
        Me.grbInfo.Controls.Add(Me.lbNote)
        Me.grbInfo.Controls.Add(Me.lbMessage)
        Me.grbInfo.Controls.Add(Me.btnBrowse)
        Me.grbInfo.Controls.Add(Me.txtFileLocation)
        Me.grbInfo.Controls.Add(Me.lbFileLocation)
        Me.grbInfo.Location = New System.Drawing.Point(12, 58)
        Me.grbInfo.Name = "grbInfo"
        Me.grbInfo.Size = New System.Drawing.Size(510, 154)
        Me.grbInfo.TabIndex = 9
        Me.grbInfo.TabStop = False
        Me.grbInfo.Text = "Kết quả tìm kiếm"
        '
        'lbNote
        '
        Me.lbNote.AutoSize = True
        Me.lbNote.Location = New System.Drawing.Point(38, 63)
        Me.lbNote.Name = "lbNote"
        Me.lbNote.Size = New System.Drawing.Size(436, 13)
        Me.lbNote.TabIndex = 8
        Me.lbNote.Tag = "lbFileLocation"
        Me.lbNote.Text = "Bạn có thể xem file trên máy chủ hoặc chọn đường dẫn để lưu file kết quả tìm kiếm" & _
            " về máy"
        '
        'lbMessage
        '
        Me.lbMessage.Location = New System.Drawing.Point(38, 15)
        Me.lbMessage.Name = "lbMessage"
        Me.lbMessage.Size = New System.Drawing.Size(421, 47)
        Me.lbMessage.TabIndex = 7
        Me.lbMessage.Tag = "lbMessage"
        Me.lbMessage.Text = "lbMessage"
        '
        'btnBrowse
        '
        Me.btnBrowse.Location = New System.Drawing.Point(384, 113)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(75, 23)
        Me.btnBrowse.TabIndex = 6
        Me.btnBrowse.Tag = "btnBrowse"
        Me.btnBrowse.Text = "Browse"
        Me.btnBrowse.UseVisualStyleBackColor = True
        '
        'txtFileLocation
        '
        Me.txtFileLocation.Location = New System.Drawing.Point(117, 87)
        Me.txtFileLocation.Name = "txtFileLocation"
        Me.txtFileLocation.Size = New System.Drawing.Size(342, 20)
        Me.txtFileLocation.TabIndex = 5
        Me.txtFileLocation.Tag = "txtFileLocation"
        '
        'lbFileLocation
        '
        Me.lbFileLocation.AutoSize = True
        Me.lbFileLocation.Location = New System.Drawing.Point(38, 90)
        Me.lbFileLocation.Name = "lbFileLocation"
        Me.lbFileLocation.Size = New System.Drawing.Size(60, 13)
        Me.lbFileLocation.TabIndex = 4
        Me.lbFileLocation.Tag = "lbFileLocation"
        Me.lbFileLocation.Text = "Đường dẫn"
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.SystemColors.Desktop
        Me.Panel1.Controls.Add(Me.lbCaption)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(541, 56)
        Me.Panel1.TabIndex = 8
        '
        'lbCaption
        '
        Me.lbCaption.AutoSize = True
        Me.lbCaption.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbCaption.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lbCaption.Location = New System.Drawing.Point(23, 20)
        Me.lbCaption.Name = "lbCaption"
        Me.lbCaption.Size = New System.Drawing.Size(207, 13)
        Me.lbCaption.TabIndex = 0
        Me.lbCaption.Tag = "lbCaption"
        Me.lbCaption.Text = "Thông báo kết quả tìm kiếm dữ liệu"
        '
        'frmFTPClient
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(541, 259)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.grbInfo)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "frmFTPClient"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Thông báo kết quả tìm kiếm dữ liệu"
        Me.grbInfo.ResumeLayout(False)
        Me.grbInfo.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents grbInfo As System.Windows.Forms.GroupBox
    Friend WithEvents btnBrowse As System.Windows.Forms.Button
    Friend WithEvents txtFileLocation As System.Windows.Forms.TextBox
    Friend WithEvents lbFileLocation As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents lbCaption As System.Windows.Forms.Label
    Friend WithEvents lbNote As System.Windows.Forms.Label
    Friend WithEvents lbMessage As System.Windows.Forms.Label
End Class
