<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDbConfig
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
        Me.lbCaption = New System.Windows.Forms.Label()
        Me.lblOnlineUser = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.txtOnlineDataSource = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtOnlinePassword = New System.Windows.Forms.TextBox()
        Me.lblOnlinePassword = New System.Windows.Forms.Label()
        Me.txtOnlineUser = New System.Windows.Forms.TextBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.txtInquiryDataSource = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtInquiryPassword = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtInquiryUser = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'lbCaption
        '
        Me.lbCaption.AutoSize = True
        Me.lbCaption.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbCaption.Location = New System.Drawing.Point(21, 18)
        Me.lbCaption.Name = "lbCaption"
        Me.lbCaption.Size = New System.Drawing.Size(358, 26)
        Me.lbCaption.TabIndex = 0
        Me.lbCaption.Text = "THIẾT LẬP CẤU HÌNH DATABASE"
        '
        'lblOnlineUser
        '
        Me.lblOnlineUser.AutoSize = True
        Me.lblOnlineUser.Location = New System.Drawing.Point(39, 26)
        Me.lblOnlineUser.Name = "lblOnlineUser"
        Me.lblOnlineUser.Size = New System.Drawing.Size(62, 13)
        Me.lblOnlineUser.TabIndex = 1
        Me.lblOnlineUser.Text = "Online User"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.txtOnlineDataSource)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.txtOnlinePassword)
        Me.GroupBox1.Controls.Add(Me.lblOnlinePassword)
        Me.GroupBox1.Controls.Add(Me.txtOnlineUser)
        Me.GroupBox1.Controls.Add(Me.lblOnlineUser)
        Me.GroupBox1.Location = New System.Drawing.Point(10, 47)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(387, 122)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Online User"
        '
        'txtOnlineDataSource
        '
        Me.txtOnlineDataSource.Location = New System.Drawing.Point(151, 71)
        Me.txtOnlineDataSource.Name = "txtOnlineDataSource"
        Me.txtOnlineDataSource.Size = New System.Drawing.Size(166, 20)
        Me.txtOnlineDataSource.TabIndex = 6
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(39, 78)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(97, 13)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Online DataSource"
        '
        'txtOnlinePassword
        '
        Me.txtOnlinePassword.Location = New System.Drawing.Point(151, 45)
        Me.txtOnlinePassword.Name = "txtOnlinePassword"
        Me.txtOnlinePassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtOnlinePassword.Size = New System.Drawing.Size(166, 20)
        Me.txtOnlinePassword.TabIndex = 4
        '
        'lblOnlinePassword
        '
        Me.lblOnlinePassword.AutoSize = True
        Me.lblOnlinePassword.Location = New System.Drawing.Point(39, 52)
        Me.lblOnlinePassword.Name = "lblOnlinePassword"
        Me.lblOnlinePassword.Size = New System.Drawing.Size(86, 13)
        Me.lblOnlinePassword.TabIndex = 3
        Me.lblOnlinePassword.Text = "Online Password"
        '
        'txtOnlineUser
        '
        Me.txtOnlineUser.Location = New System.Drawing.Point(151, 19)
        Me.txtOnlineUser.Name = "txtOnlineUser"
        Me.txtOnlineUser.Size = New System.Drawing.Size(166, 20)
        Me.txtOnlineUser.TabIndex = 2
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.txtInquiryDataSource)
        Me.GroupBox2.Controls.Add(Me.Label4)
        Me.GroupBox2.Controls.Add(Me.txtInquiryPassword)
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Controls.Add(Me.txtInquiryUser)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Location = New System.Drawing.Point(10, 175)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(387, 113)
        Me.GroupBox2.TabIndex = 5
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Inquiry User"
        '
        'txtInquiryDataSource
        '
        Me.txtInquiryDataSource.Location = New System.Drawing.Point(151, 73)
        Me.txtInquiryDataSource.Name = "txtInquiryDataSource"
        Me.txtInquiryDataSource.Size = New System.Drawing.Size(166, 20)
        Me.txtInquiryDataSource.TabIndex = 8
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(39, 80)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(98, 13)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "Inquiry DataSource"
        '
        'txtInquiryPassword
        '
        Me.txtInquiryPassword.Location = New System.Drawing.Point(151, 47)
        Me.txtInquiryPassword.Name = "txtInquiryPassword"
        Me.txtInquiryPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtInquiryPassword.Size = New System.Drawing.Size(166, 20)
        Me.txtInquiryPassword.TabIndex = 4
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(39, 54)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(87, 13)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Inquiry Password"
        '
        'txtInquiryUser
        '
        Me.txtInquiryUser.Location = New System.Drawing.Point(151, 21)
        Me.txtInquiryUser.Name = "txtInquiryUser"
        Me.txtInquiryUser.Size = New System.Drawing.Size(166, 20)
        Me.txtInquiryUser.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(39, 28)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(63, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Inquiry User"
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(133, 305)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(64, 23)
        Me.btnSave.TabIndex = 6
        Me.btnSave.Text = "Thực hiện"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(202, 305)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(64, 23)
        Me.btnExit.TabIndex = 7
        Me.btnExit.Text = "Thoát"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'frmDbConfig
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(408, 344)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.lbCaption)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "frmDbConfig"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Thiết lập cấu hình Database"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lbCaption As System.Windows.Forms.Label
    Friend WithEvents lblOnlineUser As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents txtOnlinePassword As System.Windows.Forms.TextBox
    Friend WithEvents lblOnlinePassword As System.Windows.Forms.Label
    Friend WithEvents txtOnlineUser As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents txtInquiryPassword As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtInquiryUser As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtOnlineDataSource As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtInquiryDataSource As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button

End Class
