<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmChangePassword
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmChangePassword))
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.lbCaption = New System.Windows.Forms.Label
        Me.txtUserName = New System.Windows.Forms.TextBox
        Me.txtOldPassword = New System.Windows.Forms.TextBox
        Me.txtNewPassword = New System.Windows.Forms.TextBox
        Me.lblUserName = New System.Windows.Forms.Label
        Me.lblOldPassword = New System.Windows.Forms.Label
        Me.lblNewPassword = New System.Windows.Forms.Label
        Me.txtReType = New System.Windows.Forms.TextBox
        Me.lblReType = New System.Windows.Forms.Label
        Me.btnOK = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.Panel1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.SteelBlue
        Me.Panel1.Controls.Add(Me.PictureBox1)
        Me.Panel1.Controls.Add(Me.lbCaption)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(415, 58)
        Me.Panel1.TabIndex = 1
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(33, 12)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(31, 33)
        Me.PictureBox1.TabIndex = 1
        Me.PictureBox1.TabStop = False
        '
        'lbCaption
        '
        Me.lbCaption.AutoSize = True
        Me.lbCaption.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbCaption.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lbCaption.Location = New System.Drawing.Point(82, 32)
        Me.lbCaption.Name = "lbCaption"
        Me.lbCaption.Size = New System.Drawing.Size(45, 13)
        Me.lbCaption.TabIndex = 0
        Me.lbCaption.Text = "Label1"
        '
        'txtUserName
        '
        Me.txtUserName.Location = New System.Drawing.Point(165, 77)
        Me.txtUserName.Name = "txtUserName"
        Me.txtUserName.ReadOnly = True
        Me.txtUserName.Size = New System.Drawing.Size(238, 20)
        Me.txtUserName.TabIndex = 2
        '
        'txtOldPassword
        '
        Me.txtOldPassword.Location = New System.Drawing.Point(165, 103)
        Me.txtOldPassword.Name = "txtOldPassword"
        Me.txtOldPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtOldPassword.Size = New System.Drawing.Size(238, 20)
        Me.txtOldPassword.TabIndex = 3
        '
        'txtNewPassword
        '
        Me.txtNewPassword.Location = New System.Drawing.Point(165, 129)
        Me.txtNewPassword.Name = "txtNewPassword"
        Me.txtNewPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtNewPassword.Size = New System.Drawing.Size(238, 20)
        Me.txtNewPassword.TabIndex = 4
        '
        'lblUserName
        '
        Me.lblUserName.AutoSize = True
        Me.lblUserName.Location = New System.Drawing.Point(33, 77)
        Me.lblUserName.Name = "lblUserName"
        Me.lblUserName.Size = New System.Drawing.Size(67, 13)
        Me.lblUserName.TabIndex = 5
        Me.lblUserName.Tag = "lblUserName"
        Me.lblUserName.Text = "lblUserName"
        '
        'lblOldPassword
        '
        Me.lblOldPassword.AutoSize = True
        Me.lblOldPassword.Location = New System.Drawing.Point(33, 106)
        Me.lblOldPassword.Name = "lblOldPassword"
        Me.lblOldPassword.Size = New System.Drawing.Size(79, 13)
        Me.lblOldPassword.TabIndex = 6
        Me.lblOldPassword.Tag = "lblOldPassword"
        Me.lblOldPassword.Text = "lblOldPassword"
        '
        'lblNewPassword
        '
        Me.lblNewPassword.AutoSize = True
        Me.lblNewPassword.Location = New System.Drawing.Point(33, 136)
        Me.lblNewPassword.Name = "lblNewPassword"
        Me.lblNewPassword.Size = New System.Drawing.Size(85, 13)
        Me.lblNewPassword.TabIndex = 7
        Me.lblNewPassword.Tag = "lblNewPassword"
        Me.lblNewPassword.Text = "lblNewPassword"
        '
        'txtReType
        '
        Me.txtReType.Location = New System.Drawing.Point(165, 155)
        Me.txtReType.Name = "txtReType"
        Me.txtReType.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtReType.Size = New System.Drawing.Size(238, 20)
        Me.txtReType.TabIndex = 8
        '
        'lblReType
        '
        Me.lblReType.AutoSize = True
        Me.lblReType.Location = New System.Drawing.Point(33, 158)
        Me.lblReType.Name = "lblReType"
        Me.lblReType.Size = New System.Drawing.Size(55, 13)
        Me.lblReType.TabIndex = 9
        Me.lblReType.Tag = "lblReType"
        Me.lblReType.Text = "lblReType"
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(219, 181)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(89, 30)
        Me.btnOK.TabIndex = 10
        Me.btnOK.Tag = "btnOK"
        Me.btnOK.Text = "btnOK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(314, 181)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(89, 30)
        Me.btnCancel.TabIndex = 11
        Me.btnCancel.Tag = "btnCancel"
        Me.btnCancel.Text = "btnCancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'frmChangePassword
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(415, 220)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.lblReType)
        Me.Controls.Add(Me.txtReType)
        Me.Controls.Add(Me.lblNewPassword)
        Me.Controls.Add(Me.lblOldPassword)
        Me.Controls.Add(Me.lblUserName)
        Me.Controls.Add(Me.txtNewPassword)
        Me.Controls.Add(Me.txtOldPassword)
        Me.Controls.Add(Me.txtUserName)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "frmChangePassword"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "frmChangePassword"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents lbCaption As System.Windows.Forms.Label
    Friend WithEvents txtUserName As System.Windows.Forms.TextBox
    Friend WithEvents txtOldPassword As System.Windows.Forms.TextBox
    Friend WithEvents txtNewPassword As System.Windows.Forms.TextBox
    Friend WithEvents lblUserName As System.Windows.Forms.Label
    Friend WithEvents lblOldPassword As System.Windows.Forms.Label
    Friend WithEvents lblNewPassword As System.Windows.Forms.Label
    Friend WithEvents txtReType As System.Windows.Forms.TextBox
    Friend WithEvents lblReType As System.Windows.Forms.Label
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
End Class
