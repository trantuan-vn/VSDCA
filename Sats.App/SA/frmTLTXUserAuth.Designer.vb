<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTLTXUserAuth
    Inherits Sats.AppCore.frmMaintenance

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTLTXUserAuth))
        Me.grbInfo = New System.Windows.Forms.GroupBox
        Me.lbNote = New System.Windows.Forms.Label
        Me.txtAuthName = New System.Windows.Forms.TextBox
        Me.txtAuthId = New System.Windows.Forms.TextBox
        Me.lbAuthType = New System.Windows.Forms.Label
        Me.cboAuthType = New Sats.AppCore.ComboBoxEx
        Me.lbAuthName = New System.Windows.Forms.Label
        Me.cboCfrId = New Sats.AppCore.ComboBoxEx
        Me.lbCfrId = New System.Windows.Forms.Label
        Me.cboOffId = New Sats.AppCore.ComboBoxEx
        Me.lbOffId = New System.Windows.Forms.Label
        Me.cboChkId = New Sats.AppCore.ComboBoxEx
        Me.lbChkId = New System.Windows.Forms.Label
        Me.cboTLTX = New Sats.AppCore.ComboBoxEx
        Me.lbTltx = New System.Windows.Forms.Label
        Me.cboModCode = New Sats.AppCore.ComboBoxEx
        Me.lbModCode = New System.Windows.Forms.Label
        Me.lbAuthId = New System.Windows.Forms.Label
        Me.Panel1.SuspendLayout()
        Me.grbInfo.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Size = New System.Drawing.Size(526, 42)
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(180, 309)
        Me.btnOk.Size = New System.Drawing.Size(80, 34)
        '
        'btnApply
        '
        Me.btnApply.Location = New System.Drawing.Point(206, 404)
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(266, 309)
        Me.btnCancel.Size = New System.Drawing.Size(80, 34)
        '
        'grbInfo
        '
        Me.grbInfo.Controls.Add(Me.lbNote)
        Me.grbInfo.Controls.Add(Me.txtAuthName)
        Me.grbInfo.Controls.Add(Me.txtAuthId)
        Me.grbInfo.Controls.Add(Me.lbAuthType)
        Me.grbInfo.Controls.Add(Me.cboAuthType)
        Me.grbInfo.Controls.Add(Me.lbAuthName)
        Me.grbInfo.Controls.Add(Me.cboCfrId)
        Me.grbInfo.Controls.Add(Me.lbCfrId)
        Me.grbInfo.Controls.Add(Me.cboOffId)
        Me.grbInfo.Controls.Add(Me.lbOffId)
        Me.grbInfo.Controls.Add(Me.cboChkId)
        Me.grbInfo.Controls.Add(Me.lbChkId)
        Me.grbInfo.Controls.Add(Me.cboTLTX)
        Me.grbInfo.Controls.Add(Me.lbTltx)
        Me.grbInfo.Controls.Add(Me.cboModCode)
        Me.grbInfo.Controls.Add(Me.lbModCode)
        Me.grbInfo.Controls.Add(Me.lbAuthId)
        Me.grbInfo.Location = New System.Drawing.Point(12, 58)
        Me.grbInfo.Name = "grbInfo"
        Me.grbInfo.Size = New System.Drawing.Size(502, 245)
        Me.grbInfo.TabIndex = 4
        Me.grbInfo.TabStop = False
        Me.grbInfo.Tag = "grbInfo"
        Me.grbInfo.Text = "grbInfo"
        '
        'lbNote
        '
        Me.lbNote.AutoSize = True
        Me.lbNote.Location = New System.Drawing.Point(270, 50)
        Me.lbNote.Name = "lbNote"
        Me.lbNote.Size = New System.Drawing.Size(38, 13)
        Me.lbNote.TabIndex = 21
        Me.lbNote.Tag = "lbNote"
        Me.lbNote.Text = "lbNote"
        '
        'txtAuthName
        '
        Me.txtAuthName.Enabled = False
        Me.txtAuthName.Location = New System.Drawing.Point(163, 72)
        Me.txtAuthName.Name = "txtAuthName"
        Me.txtAuthName.Size = New System.Drawing.Size(220, 20)
        Me.txtAuthName.TabIndex = 20
        Me.txtAuthName.Tag = "AUTHNAME"
        '
        'txtAuthId
        '
        Me.txtAuthId.Location = New System.Drawing.Point(163, 45)
        Me.txtAuthId.Name = "txtAuthId"
        Me.txtAuthId.Size = New System.Drawing.Size(100, 20)
        Me.txtAuthId.TabIndex = 19
        Me.txtAuthId.Tag = "AUTHID"
        '
        'lbAuthType
        '
        Me.lbAuthType.AutoSize = True
        Me.lbAuthType.Location = New System.Drawing.Point(6, 22)
        Me.lbAuthType.Name = "lbAuthType"
        Me.lbAuthType.Size = New System.Drawing.Size(61, 13)
        Me.lbAuthType.TabIndex = 18
        Me.lbAuthType.Tag = "lbAuthType"
        Me.lbAuthType.Text = "lbAuthType"
        '
        'cboAuthType
        '
        Me.cboAuthType.DisplayMember = "DISPLAY"
        Me.cboAuthType.FormattingEnabled = True
        Me.cboAuthType.Location = New System.Drawing.Point(163, 19)
        Me.cboAuthType.Name = "cboAuthType"
        Me.cboAuthType.Size = New System.Drawing.Size(220, 21)
        Me.cboAuthType.TabIndex = 17
        Me.cboAuthType.Tag = "AUTHTYPE"
        Me.cboAuthType.ValueMember = "VALUE"
        '
        'lbAuthName
        '
        Me.lbAuthName.AutoSize = True
        Me.lbAuthName.Location = New System.Drawing.Point(6, 75)
        Me.lbAuthName.Name = "lbAuthName"
        Me.lbAuthName.Size = New System.Drawing.Size(65, 13)
        Me.lbAuthName.TabIndex = 15
        Me.lbAuthName.Tag = "lbAuthName"
        Me.lbAuthName.Text = "lbAuthName"
        '
        'cboCfrId
        '
        Me.cboCfrId.DisplayMember = "DISPLAY"
        Me.cboCfrId.FormattingEnabled = True
        Me.cboCfrId.Location = New System.Drawing.Point(163, 215)
        Me.cboCfrId.Name = "cboCfrId"
        Me.cboCfrId.Size = New System.Drawing.Size(155, 21)
        Me.cboCfrId.TabIndex = 11
        Me.cboCfrId.Tag = "CFRID"
        Me.cboCfrId.ValueMember = "VALUE"
        '
        'lbCfrId
        '
        Me.lbCfrId.AutoSize = True
        Me.lbCfrId.Location = New System.Drawing.Point(5, 218)
        Me.lbCfrId.Name = "lbCfrId"
        Me.lbCfrId.Size = New System.Drawing.Size(37, 13)
        Me.lbCfrId.TabIndex = 10
        Me.lbCfrId.Tag = "lbCfrId"
        Me.lbCfrId.Text = "lbCfrId"
        '
        'cboOffId
        '
        Me.cboOffId.DisplayMember = "DISPLAY"
        Me.cboOffId.FormattingEnabled = True
        Me.cboOffId.Location = New System.Drawing.Point(163, 185)
        Me.cboOffId.Name = "cboOffId"
        Me.cboOffId.Size = New System.Drawing.Size(155, 21)
        Me.cboOffId.TabIndex = 9
        Me.cboOffId.Tag = "OFFID"
        Me.cboOffId.ValueMember = "VALUE"
        '
        'lbOffId
        '
        Me.lbOffId.AutoSize = True
        Me.lbOffId.Location = New System.Drawing.Point(6, 188)
        Me.lbOffId.Name = "lbOffId"
        Me.lbOffId.Size = New System.Drawing.Size(38, 13)
        Me.lbOffId.TabIndex = 8
        Me.lbOffId.Tag = "lbOffId"
        Me.lbOffId.Text = "lbOffId"
        '
        'cboChkId
        '
        Me.cboChkId.DisplayMember = "DISPLAY"
        Me.cboChkId.FormattingEnabled = True
        Me.cboChkId.Location = New System.Drawing.Point(163, 156)
        Me.cboChkId.Name = "cboChkId"
        Me.cboChkId.Size = New System.Drawing.Size(155, 21)
        Me.cboChkId.TabIndex = 7
        Me.cboChkId.Tag = "CHKID"
        Me.cboChkId.ValueMember = "VALUE"
        '
        'lbChkId
        '
        Me.lbChkId.AutoSize = True
        Me.lbChkId.Location = New System.Drawing.Point(6, 159)
        Me.lbChkId.Name = "lbChkId"
        Me.lbChkId.Size = New System.Drawing.Size(43, 13)
        Me.lbChkId.TabIndex = 6
        Me.lbChkId.Tag = "lbChkId"
        Me.lbChkId.Text = "lbChkId"
        '
        'cboTLTX
        '
        Me.cboTLTX.DisplayMember = "DISPLAY"
        Me.cboTLTX.FormattingEnabled = True
        Me.cboTLTX.Location = New System.Drawing.Point(163, 127)
        Me.cboTLTX.Name = "cboTLTX"
        Me.cboTLTX.Size = New System.Drawing.Size(333, 21)
        Me.cboTLTX.TabIndex = 5
        Me.cboTLTX.Tag = "TLTXCD"
        Me.cboTLTX.ValueMember = "VALUE"
        '
        'lbTltx
        '
        Me.lbTltx.AutoSize = True
        Me.lbTltx.Location = New System.Drawing.Point(5, 130)
        Me.lbTltx.Name = "lbTltx"
        Me.lbTltx.Size = New System.Drawing.Size(32, 13)
        Me.lbTltx.TabIndex = 4
        Me.lbTltx.Tag = "lbTltx"
        Me.lbTltx.Text = "lbTltx"
        '
        'cboModCode
        '
        Me.cboModCode.DisplayMember = "DISPLAY"
        Me.cboModCode.FormattingEnabled = True
        Me.cboModCode.Location = New System.Drawing.Point(163, 98)
        Me.cboModCode.Name = "cboModCode"
        Me.cboModCode.Size = New System.Drawing.Size(333, 21)
        Me.cboModCode.TabIndex = 3
        Me.cboModCode.Tag = "MODCODE"
        Me.cboModCode.ValueMember = "VALUE"
        '
        'lbModCode
        '
        Me.lbModCode.AutoSize = True
        Me.lbModCode.Location = New System.Drawing.Point(6, 101)
        Me.lbModCode.Name = "lbModCode"
        Me.lbModCode.Size = New System.Drawing.Size(61, 13)
        Me.lbModCode.TabIndex = 2
        Me.lbModCode.Tag = "lbModCode"
        Me.lbModCode.Text = "lbModCode"
        '
        'lbAuthId
        '
        Me.lbAuthId.AutoSize = True
        Me.lbAuthId.Location = New System.Drawing.Point(6, 48)
        Me.lbAuthId.Name = "lbAuthId"
        Me.lbAuthId.Size = New System.Drawing.Size(46, 13)
        Me.lbAuthId.TabIndex = 0
        Me.lbAuthId.Tag = "lbAuthId"
        Me.lbAuthId.Text = "lbAuthId"
        '
        'frmTLTXUserAuth
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.ClientSize = New System.Drawing.Size(526, 351)
        Me.Controls.Add(Me.grbInfo)
        Me.Name = "frmTLTXUserAuth"
        Me.Controls.SetChildIndex(Me.btnOk, 0)
        Me.Controls.SetChildIndex(Me.btnCancel, 0)
        Me.Controls.SetChildIndex(Me.btnApply, 0)
        Me.Controls.SetChildIndex(Me.grbInfo, 0)
        Me.Controls.SetChildIndex(Me.Panel1, 0)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.grbInfo.ResumeLayout(False)
        Me.grbInfo.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grbInfo As System.Windows.Forms.GroupBox
    Friend WithEvents lbAuthId As System.Windows.Forms.Label
    Friend WithEvents cboModCode As Sats.AppCore.ComboBoxEx
    Friend WithEvents lbModCode As System.Windows.Forms.Label
    Friend WithEvents cboCfrId As Sats.AppCore.ComboBoxEx
    Friend WithEvents lbCfrId As System.Windows.Forms.Label
    Friend WithEvents cboOffId As Sats.AppCore.ComboBoxEx
    Friend WithEvents lbOffId As System.Windows.Forms.Label
    Friend WithEvents cboChkId As Sats.AppCore.ComboBoxEx
    Friend WithEvents lbChkId As System.Windows.Forms.Label
    Friend WithEvents cboTLTX As Sats.AppCore.ComboBoxEx
    Friend WithEvents lbTltx As System.Windows.Forms.Label
    Friend WithEvents lbAuthName As System.Windows.Forms.Label
    Friend WithEvents lbAuthType As System.Windows.Forms.Label
    Friend WithEvents cboAuthType As Sats.AppCore.ComboBoxEx
    Friend WithEvents txtAuthId As System.Windows.Forms.TextBox
    Friend WithEvents lbNote As System.Windows.Forms.Label
    Friend WithEvents txtAuthName As System.Windows.Forms.TextBox

End Class
