<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLogin
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer
    Private resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmLogin))
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmLogin))
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.lbCaption = New System.Windows.Forms.Label
        Me.lbUserName = New System.Windows.Forms.Label
        Me.lbPassword = New System.Windows.Forms.Label
        Me.txtUserName = New System.Windows.Forms.TextBox
        Me.txtPassword = New System.Windows.Forms.TextBox
        Me.cbRemPass = New System.Windows.Forms.CheckBox
        Me.btnOk = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.lblBRID = New System.Windows.Forms.Label
        Me.btnBRID = New System.Windows.Forms.Button
        Me.cboBRID = New Sats.AppCore.ComboBoxEx
        Me.Panel1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.SteelBlue
        Me.Panel1.Controls.Add(Me.PictureBox1)
        Me.Panel1.Controls.Add(Me.lbCaption)
        Me.Panel1.Location = New System.Drawing.Point(-2, -6)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(588, 71)
        Me.Panel1.TabIndex = 0
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(33, 18)
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
        Me.lbCaption.Location = New System.Drawing.Point(131, 33)
        Me.lbCaption.Name = "lbCaption"
        Me.lbCaption.Size = New System.Drawing.Size(45, 13)
        Me.lbCaption.TabIndex = 0
        Me.lbCaption.Text = "Label1"
        '
        'lbUserName
        '
        Me.lbUserName.AutoSize = True
        Me.lbUserName.Location = New System.Drawing.Point(12, 74)
        Me.lbUserName.Name = "lbUserName"
        Me.lbUserName.Size = New System.Drawing.Size(65, 13)
        Me.lbUserName.TabIndex = 1
        Me.lbUserName.Text = "lbUserName"
        '
        'lbPassword
        '
        Me.lbPassword.AutoSize = True
        Me.lbPassword.Location = New System.Drawing.Point(12, 100)
        Me.lbPassword.Name = "lbPassword"
        Me.lbPassword.Size = New System.Drawing.Size(61, 13)
        Me.lbPassword.TabIndex = 2
        Me.lbPassword.Text = "lbPassword"
        '
        'txtUserName
        '
        Me.txtUserName.Location = New System.Drawing.Point(123, 71)
        Me.txtUserName.Name = "txtUserName"
        Me.txtUserName.Size = New System.Drawing.Size(207, 20)
        Me.txtUserName.TabIndex = 3
        '
        'txtPassword
        '
        Me.txtPassword.Location = New System.Drawing.Point(123, 97)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPassword.Size = New System.Drawing.Size(207, 20)
        Me.txtPassword.TabIndex = 4
        '
        'cbRemPass
        '
        Me.cbRemPass.AutoSize = True
        Me.cbRemPass.Location = New System.Drawing.Point(123, 123)
        Me.cbRemPass.Name = "cbRemPass"
        Me.cbRemPass.Size = New System.Drawing.Size(83, 17)
        Me.cbRemPass.TabIndex = 5
        Me.cbRemPass.Text = "cbRemPass"
        Me.cbRemPass.UseVisualStyleBackColor = True
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(156, 146)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(86, 27)
        Me.btnOk.TabIndex = 6
        Me.btnOk.Text = "btnOk"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(248, 146)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(82, 27)
        Me.btnCancel.TabIndex = 30
        Me.btnCancel.Text = "btnCancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'lblBRID
        '
        Me.lblBRID.AutoSize = True
        Me.lblBRID.Location = New System.Drawing.Point(12, 124)
        Me.lblBRID.Name = "lblBRID"
        Me.lblBRID.Size = New System.Drawing.Size(43, 13)
        Me.lblBRID.TabIndex = 8
        Me.lblBRID.Tag = "lblBRID"
        Me.lblBRID.Text = "lblBRID"
        Me.lblBRID.Visible = False
        '
        'btnBRID
        '
        Me.btnBRID.Location = New System.Drawing.Point(156, 146)
        Me.btnBRID.Name = "btnBRID"
        Me.btnBRID.Size = New System.Drawing.Size(86, 27)
        Me.btnBRID.TabIndex = 8
        Me.btnBRID.Tag = "btnBRID"
        Me.btnBRID.Text = "btnBRID"
        Me.btnBRID.UseVisualStyleBackColor = True
        Me.btnBRID.Visible = False
        '
        'cboBRID
        '
        Me.cboBRID.DisplayMember = "DISPLAY"
        Me.cboBRID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBRID.FormattingEnabled = True
        Me.cboBRID.Location = New System.Drawing.Point(123, 123)
        Me.cboBRID.Name = "cboBRID"
        Me.cboBRID.Size = New System.Drawing.Size(207, 21)
        Me.cboBRID.TabIndex = 7
        Me.cboBRID.ValueMember = "VALUE"
        Me.cboBRID.Visible = False
        '
        'frmLogin
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(339, 179)
        Me.Controls.Add(Me.cboBRID)
        Me.Controls.Add(Me.btnBRID)
        Me.Controls.Add(Me.lblBRID)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.cbRemPass)
        Me.Controls.Add(Me.txtPassword)
        Me.Controls.Add(Me.txtUserName)
        Me.Controls.Add(Me.lbPassword)
        Me.Controls.Add(Me.lbUserName)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmLogin"
        Me.ShowInTaskbar = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Login"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents lbCaption As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents lbUserName As System.Windows.Forms.Label
    Friend WithEvents lbPassword As System.Windows.Forms.Label
    Friend WithEvents txtUserName As System.Windows.Forms.TextBox
    Friend WithEvents txtPassword As System.Windows.Forms.TextBox
    Friend WithEvents cbRemPass As System.Windows.Forms.CheckBox
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents lblBRID As System.Windows.Forms.Label
    Friend WithEvents btnBRID As System.Windows.Forms.Button
    Friend WithEvents cboBRID As Sats.AppCore.ComboBoxEx

End Class
