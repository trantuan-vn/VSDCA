<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRGIS
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRGIS))
        Me.GrbGroups = New System.Windows.Forms.GroupBox
        Me.cbIsOperCate = New Sats.AppCore.ComboBoxEx
        Me.lblOperCate = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.DateTimePicker2 = New System.Windows.Forms.DateTimePicker
        Me.DateTimePicker1 = New System.Windows.Forms.DateTimePicker
        Me.txtPhone = New System.Windows.Forms.TextBox
        Me.dtpBussinessDate = New System.Windows.Forms.DateTimePicker
        Me.txtBussinessIssue = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.lbBussinessDate = New System.Windows.Forms.Label
        Me.txtBussinessNo = New System.Windows.Forms.TextBox
        Me.lbBussinessNo = New System.Windows.Forms.Label
        Me.txtFax = New System.Windows.Forms.TextBox
        Me.dtpLicenseDate = New System.Windows.Forms.DateTimePicker
        Me.dtpFoundationDate = New System.Windows.Forms.DateTimePicker
        Me.txtCapitalRule = New System.Windows.Forms.TextBox
        Me.txtCode = New Sats.AppCore.FlexMaskEditBox
        Me.cbIsType = New Sats.AppCore.ComboBoxEx
        Me.txtNote = New System.Windows.Forms.TextBox
        Me.lbNote = New System.Windows.Forms.Label
        Me.lbCapitalRule = New System.Windows.Forms.Label
        Me.txtLicenseIssuer = New System.Windows.Forms.TextBox
        Me.lbShortName = New System.Windows.Forms.Label
        Me.txtShortName = New System.Windows.Forms.TextBox
        Me.lbLicenseIssuer = New System.Windows.Forms.Label
        Me.lbLicenseDate = New System.Windows.Forms.Label
        Me.txtLicenseNo = New System.Windows.Forms.TextBox
        Me.lbLicenseNo = New System.Windows.Forms.Label
        Me.txtFoundationIssuers = New System.Windows.Forms.TextBox
        Me.lblFoundationIssuers = New System.Windows.Forms.Label
        Me.lbFoundationDate = New System.Windows.Forms.Label
        Me.txtFoundationNo = New System.Windows.Forms.TextBox
        Me.lbFoundationNo = New System.Windows.Forms.Label
        Me.txtBankName = New System.Windows.Forms.TextBox
        Me.lbBankName = New System.Windows.Forms.Label
        Me.txtBankAccount = New System.Windows.Forms.TextBox
        Me.lbBankAccount = New System.Windows.Forms.Label
        Me.txtBusinessField = New System.Windows.Forms.TextBox
        Me.lbBusinessField = New System.Windows.Forms.Label
        Me.lbFax = New System.Windows.Forms.Label
        Me.lbPhone = New System.Windows.Forms.Label
        Me.txtAddress = New System.Windows.Forms.TextBox
        Me.lbAddress = New System.Windows.Forms.Label
        Me.txtIsName = New System.Windows.Forms.TextBox
        Me.lbIsName = New System.Windows.Forms.Label
        Me.lbCode = New System.Windows.Forms.Label
        Me.lbIsType = New System.Windows.Forms.Label
        Me.Panel1.SuspendLayout()
        Me.GrbGroups.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Size = New System.Drawing.Size(628, 42)
        Me.Panel1.TabIndex = 53
        '
        'lbCaption
        '
        Me.lbCaption.BackColor = System.Drawing.SystemColors.Desktop
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(456, 384)
        Me.btnOk.TabIndex = 101
        '
        'btnApply
        '
        Me.btnApply.Enabled = False
        Me.btnApply.Location = New System.Drawing.Point(370, 384)
        Me.btnApply.TabIndex = 100
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(542, 384)
        Me.btnCancel.TabIndex = 102
        '
        'GrbGroups
        '
        Me.GrbGroups.Controls.Add(Me.cbIsOperCate)
        Me.GrbGroups.Controls.Add(Me.lblOperCate)
        Me.GrbGroups.Controls.Add(Me.Label3)
        Me.GrbGroups.Controls.Add(Me.Label2)
        Me.GrbGroups.Controls.Add(Me.DateTimePicker2)
        Me.GrbGroups.Controls.Add(Me.DateTimePicker1)
        Me.GrbGroups.Controls.Add(Me.txtPhone)
        Me.GrbGroups.Controls.Add(Me.dtpBussinessDate)
        Me.GrbGroups.Controls.Add(Me.txtBussinessIssue)
        Me.GrbGroups.Controls.Add(Me.Label1)
        Me.GrbGroups.Controls.Add(Me.lbBussinessDate)
        Me.GrbGroups.Controls.Add(Me.txtBussinessNo)
        Me.GrbGroups.Controls.Add(Me.lbBussinessNo)
        Me.GrbGroups.Controls.Add(Me.txtFax)
        Me.GrbGroups.Controls.Add(Me.dtpLicenseDate)
        Me.GrbGroups.Controls.Add(Me.dtpFoundationDate)
        Me.GrbGroups.Controls.Add(Me.txtCapitalRule)
        Me.GrbGroups.Controls.Add(Me.txtCode)
        Me.GrbGroups.Controls.Add(Me.cbIsType)
        Me.GrbGroups.Controls.Add(Me.txtNote)
        Me.GrbGroups.Controls.Add(Me.lbNote)
        Me.GrbGroups.Controls.Add(Me.lbCapitalRule)
        Me.GrbGroups.Controls.Add(Me.txtLicenseIssuer)
        Me.GrbGroups.Controls.Add(Me.lbShortName)
        Me.GrbGroups.Controls.Add(Me.txtShortName)
        Me.GrbGroups.Controls.Add(Me.lbLicenseIssuer)
        Me.GrbGroups.Controls.Add(Me.lbLicenseDate)
        Me.GrbGroups.Controls.Add(Me.txtLicenseNo)
        Me.GrbGroups.Controls.Add(Me.lbLicenseNo)
        Me.GrbGroups.Controls.Add(Me.txtFoundationIssuers)
        Me.GrbGroups.Controls.Add(Me.lblFoundationIssuers)
        Me.GrbGroups.Controls.Add(Me.lbFoundationDate)
        Me.GrbGroups.Controls.Add(Me.txtFoundationNo)
        Me.GrbGroups.Controls.Add(Me.lbFoundationNo)
        Me.GrbGroups.Controls.Add(Me.txtBankName)
        Me.GrbGroups.Controls.Add(Me.lbBankName)
        Me.GrbGroups.Controls.Add(Me.txtBankAccount)
        Me.GrbGroups.Controls.Add(Me.lbBankAccount)
        Me.GrbGroups.Controls.Add(Me.txtBusinessField)
        Me.GrbGroups.Controls.Add(Me.lbBusinessField)
        Me.GrbGroups.Controls.Add(Me.lbFax)
        Me.GrbGroups.Controls.Add(Me.lbPhone)
        Me.GrbGroups.Controls.Add(Me.txtAddress)
        Me.GrbGroups.Controls.Add(Me.lbAddress)
        Me.GrbGroups.Controls.Add(Me.txtIsName)
        Me.GrbGroups.Controls.Add(Me.lbIsName)
        Me.GrbGroups.Controls.Add(Me.lbCode)
        Me.GrbGroups.Controls.Add(Me.lbIsType)
        Me.GrbGroups.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GrbGroups.Location = New System.Drawing.Point(3, 50)
        Me.GrbGroups.Margin = New System.Windows.Forms.Padding(5)
        Me.GrbGroups.Name = "GrbGroups"
        Me.GrbGroups.Size = New System.Drawing.Size(619, 337)
        Me.GrbGroups.TabIndex = 0
        Me.GrbGroups.TabStop = False
        Me.GrbGroups.Tag = "grbGroups"
        Me.GrbGroups.Text = "grbGroups"
        '
        'cbIsOperCate
        '
        Me.cbIsOperCate.DisplayMember = "DISPLAY"
        Me.cbIsOperCate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbIsOperCate.FormattingEnabled = True
        Me.cbIsOperCate.Location = New System.Drawing.Point(87, 126)
        Me.cbIsOperCate.Name = "cbIsOperCate"
        Me.cbIsOperCate.Size = New System.Drawing.Size(178, 21)
        Me.cbIsOperCate.TabIndex = 59
        Me.cbIsOperCate.Tag = "OPER_CATEG"
        Me.cbIsOperCate.ValueMember = "VALUE"
        '
        'lblOperCate
        '
        Me.lblOperCate.AutoSize = True
        Me.lblOperCate.Location = New System.Drawing.Point(6, 131)
        Me.lblOperCate.Name = "lblOperCate"
        Me.lblOperCate.Size = New System.Drawing.Size(62, 13)
        Me.lblOperCate.TabIndex = 58
        Me.lblOperCate.Tag = "IS_OPER_CATE"
        Me.lblOperCate.Text = "lblOperCate"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(190, 265)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(71, 13)
        Me.Label3.TabIndex = 57
        Me.Label3.Tag = "IS_CANCEL_DATE"
        Me.Label3.Text = "lbCancelDate"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(5, 266)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(63, 13)
        Me.Label2.TabIndex = 56
        Me.Label2.Tag = "IS_ISSUE_DATE"
        Me.Label2.Text = "lbIssueDate"
        '
        'DateTimePicker2
        '
        Me.DateTimePicker2.Checked = False
        Me.DateTimePicker2.CustomFormat = "dd/MM/yyyy"
        Me.DateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimePicker2.Location = New System.Drawing.Point(88, 258)
        Me.DateTimePicker2.Name = "DateTimePicker2"
        Me.DateTimePicker2.ShowCheckBox = True
        Me.DateTimePicker2.Size = New System.Drawing.Size(100, 20)
        Me.DateTimePicker2.TabIndex = 55
        Me.DateTimePicker2.Tag = "ISSUE_DATE"
        '
        'DateTimePicker1
        '
        Me.DateTimePicker1.Checked = False
        Me.DateTimePicker1.CustomFormat = "dd/MM/yyyy"
        Me.DateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimePicker1.Location = New System.Drawing.Point(267, 259)
        Me.DateTimePicker1.Name = "DateTimePicker1"
        Me.DateTimePicker1.ShowCheckBox = True
        Me.DateTimePicker1.Size = New System.Drawing.Size(100, 20)
        Me.DateTimePicker1.TabIndex = 54
        Me.DateTimePicker1.Tag = "CANCEL_DATE"
        '
        'txtPhone
        '
        Me.txtPhone.Location = New System.Drawing.Point(87, 102)
        Me.txtPhone.Name = "txtPhone"
        Me.txtPhone.Size = New System.Drawing.Size(100, 20)
        Me.txtPhone.TabIndex = 6
        Me.txtPhone.Tag = "PHONE"
        '
        'dtpBussinessDate
        '
        Me.dtpBussinessDate.Checked = False
        Me.dtpBussinessDate.CustomFormat = "dd/MM/yyyy"
        Me.dtpBussinessDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpBussinessDate.Location = New System.Drawing.Point(266, 178)
        Me.dtpBussinessDate.Name = "dtpBussinessDate"
        Me.dtpBussinessDate.ShowCheckBox = True
        Me.dtpBussinessDate.Size = New System.Drawing.Size(101, 20)
        Me.dtpBussinessDate.TabIndex = 12
        Me.dtpBussinessDate.Tag = "BUSSINESS_DATE"
        '
        'txtBussinessIssue
        '
        Me.txtBussinessIssue.Location = New System.Drawing.Point(455, 185)
        Me.txtBussinessIssue.Name = "txtBussinessIssue"
        Me.txtBussinessIssue.Size = New System.Drawing.Size(153, 20)
        Me.txtBussinessIssue.TabIndex = 13
        Me.txtBussinessIssue.Tag = "BUSSINESS_ISSUERS"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(370, 188)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(95, 13)
        Me.Label1.TabIndex = 53
        Me.Label1.Tag = "IS_BUSSINESS_ISSUERS"
        Me.Label1.Text = "lbBussinessIssuers"
        '
        'lbBussinessDate
        '
        Me.lbBussinessDate.AutoSize = True
        Me.lbBussinessDate.Location = New System.Drawing.Point(190, 185)
        Me.lbBussinessDate.Name = "lbBussinessDate"
        Me.lbBussinessDate.Size = New System.Drawing.Size(85, 13)
        Me.lbBussinessDate.TabIndex = 52
        Me.lbBussinessDate.Tag = "IS_BUSSINESS_DATE"
        Me.lbBussinessDate.Text = "lbBussinessDate"
        '
        'txtBussinessNo
        '
        Me.txtBussinessNo.Location = New System.Drawing.Point(87, 181)
        Me.txtBussinessNo.Name = "txtBussinessNo"
        Me.txtBussinessNo.Size = New System.Drawing.Size(100, 20)
        Me.txtBussinessNo.TabIndex = 11
        Me.txtBussinessNo.Tag = "BUSSINESS_NO"
        '
        'lbBussinessNo
        '
        Me.lbBussinessNo.AutoSize = True
        Me.lbBussinessNo.Location = New System.Drawing.Point(7, 184)
        Me.lbBussinessNo.Name = "lbBussinessNo"
        Me.lbBussinessNo.Size = New System.Drawing.Size(76, 13)
        Me.lbBussinessNo.TabIndex = 51
        Me.lbBussinessNo.Tag = "IS_BUSSINESS_NO"
        Me.lbBussinessNo.Text = "lbBussinessNo"
        '
        'txtFax
        '
        Me.txtFax.Location = New System.Drawing.Point(264, 102)
        Me.txtFax.Name = "txtFax"
        Me.txtFax.Size = New System.Drawing.Size(100, 20)
        Me.txtFax.TabIndex = 7
        Me.txtFax.Tag = "FAX"
        '
        'dtpLicenseDate
        '
        Me.dtpLicenseDate.Checked = False
        Me.dtpLicenseDate.CustomFormat = "dd/MM/yyyy"
        Me.dtpLicenseDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpLicenseDate.Location = New System.Drawing.Point(267, 230)
        Me.dtpLicenseDate.Name = "dtpLicenseDate"
        Me.dtpLicenseDate.ShowCheckBox = True
        Me.dtpLicenseDate.Size = New System.Drawing.Size(100, 20)
        Me.dtpLicenseDate.TabIndex = 18
        Me.dtpLicenseDate.Tag = "LICENSE_DATE"
        '
        'dtpFoundationDate
        '
        Me.dtpFoundationDate.Checked = False
        Me.dtpFoundationDate.CustomFormat = "dd/MM/yyyy"
        Me.dtpFoundationDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpFoundationDate.Location = New System.Drawing.Point(266, 204)
        Me.dtpFoundationDate.Name = "dtpFoundationDate"
        Me.dtpFoundationDate.ShowCheckBox = True
        Me.dtpFoundationDate.Size = New System.Drawing.Size(101, 20)
        Me.dtpFoundationDate.TabIndex = 15
        Me.dtpFoundationDate.Tag = "FOUNDATION_DATE"
        '
        'txtCapitalRule
        '
        Me.txtCapitalRule.Location = New System.Drawing.Point(84, 285)
        Me.txtCapitalRule.Name = "txtCapitalRule"
        Me.txtCapitalRule.Size = New System.Drawing.Size(181, 20)
        Me.txtCapitalRule.TabIndex = 20
        Me.txtCapitalRule.Tag = "CAPITAL_RULE"
        '
        'txtCode
        '
        Me.txtCode.Location = New System.Drawing.Point(360, 23)
        Me.txtCode.Name = "txtCode"
        Me.txtCode.Size = New System.Drawing.Size(246, 20)
        Me.txtCode.TabIndex = 2
        Me.txtCode.Tag = "ISCODE"
        '
        'cbIsType
        '
        Me.cbIsType.DisplayMember = "DISPLAY"
        Me.cbIsType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbIsType.FormattingEnabled = True
        Me.cbIsType.Location = New System.Drawing.Point(87, 23)
        Me.cbIsType.Name = "cbIsType"
        Me.cbIsType.Size = New System.Drawing.Size(203, 21)
        Me.cbIsType.TabIndex = 1
        Me.cbIsType.Tag = "ISTYPE"
        Me.cbIsType.ValueMember = "VALUE"
        '
        'txtNote
        '
        Me.txtNote.Location = New System.Drawing.Point(84, 311)
        Me.txtNote.Name = "txtNote"
        Me.txtNote.Size = New System.Drawing.Size(522, 20)
        Me.txtNote.TabIndex = 21
        Me.txtNote.Tag = "NOTE"
        '
        'lbNote
        '
        Me.lbNote.AutoSize = True
        Me.lbNote.Location = New System.Drawing.Point(7, 318)
        Me.lbNote.Name = "lbNote"
        Me.lbNote.Size = New System.Drawing.Size(38, 13)
        Me.lbNote.TabIndex = 47
        Me.lbNote.Tag = "IS_NOTE"
        Me.lbNote.Text = "lbNote"
        '
        'lbCapitalRule
        '
        Me.lbCapitalRule.AutoSize = True
        Me.lbCapitalRule.Location = New System.Drawing.Point(6, 292)
        Me.lbCapitalRule.Name = "lbCapitalRule"
        Me.lbCapitalRule.Size = New System.Drawing.Size(69, 13)
        Me.lbCapitalRule.TabIndex = 46
        Me.lbCapitalRule.Tag = "IS_CAPITAL_RULE"
        Me.lbCapitalRule.Text = "lbCapitalRule"
        '
        'txtLicenseIssuer
        '
        Me.txtLicenseIssuer.Location = New System.Drawing.Point(457, 237)
        Me.txtLicenseIssuer.Name = "txtLicenseIssuer"
        Me.txtLicenseIssuer.Size = New System.Drawing.Size(152, 20)
        Me.txtLicenseIssuer.TabIndex = 19
        Me.txtLicenseIssuer.Tag = "LICENSE_ISSUER"
        '
        'lbShortName
        '
        Me.lbShortName.AutoSize = True
        Me.lbShortName.Location = New System.Drawing.Point(296, 53)
        Me.lbShortName.Name = "lbShortName"
        Me.lbShortName.Size = New System.Drawing.Size(68, 13)
        Me.lbShortName.TabIndex = 32
        Me.lbShortName.Tag = "IS_SHORT_NAME"
        Me.lbShortName.Text = "lbShortName"
        '
        'txtShortName
        '
        Me.txtShortName.Location = New System.Drawing.Point(87, 50)
        Me.txtShortName.Name = "txtShortName"
        Me.txtShortName.Size = New System.Drawing.Size(203, 20)
        Me.txtShortName.TabIndex = 3
        Me.txtShortName.Tag = "SHORT_NAME"
        '
        'lbLicenseIssuer
        '
        Me.lbLicenseIssuer.AutoSize = True
        Me.lbLicenseIssuer.Location = New System.Drawing.Point(370, 240)
        Me.lbLicenseIssuer.Name = "lbLicenseIssuer"
        Me.lbLicenseIssuer.Size = New System.Drawing.Size(80, 13)
        Me.lbLicenseIssuer.TabIndex = 45
        Me.lbLicenseIssuer.Tag = "IS_LICENSE_ISSUER"
        Me.lbLicenseIssuer.Text = "lbLicenseIssuer"
        '
        'lbLicenseDate
        '
        Me.lbLicenseDate.AutoSize = True
        Me.lbLicenseDate.Location = New System.Drawing.Point(190, 236)
        Me.lbLicenseDate.Name = "lbLicenseDate"
        Me.lbLicenseDate.Size = New System.Drawing.Size(75, 13)
        Me.lbLicenseDate.TabIndex = 44
        Me.lbLicenseDate.Tag = "IS_LICENSE_DATE"
        Me.lbLicenseDate.Text = "lbLicenseDate"
        '
        'txtLicenseNo
        '
        Me.txtLicenseNo.Location = New System.Drawing.Point(87, 230)
        Me.txtLicenseNo.Name = "txtLicenseNo"
        Me.txtLicenseNo.Size = New System.Drawing.Size(100, 20)
        Me.txtLicenseNo.TabIndex = 17
        Me.txtLicenseNo.Tag = "LICENSE_NO"
        '
        'lbLicenseNo
        '
        Me.lbLicenseNo.AutoSize = True
        Me.lbLicenseNo.Location = New System.Drawing.Point(6, 240)
        Me.lbLicenseNo.Name = "lbLicenseNo"
        Me.lbLicenseNo.Size = New System.Drawing.Size(66, 13)
        Me.lbLicenseNo.TabIndex = 43
        Me.lbLicenseNo.Tag = "IS_LICENSE_NO"
        Me.lbLicenseNo.Text = "lbLicenseNo"
        '
        'txtFoundationIssuers
        '
        Me.txtFoundationIssuers.Location = New System.Drawing.Point(456, 211)
        Me.txtFoundationIssuers.Name = "txtFoundationIssuers"
        Me.txtFoundationIssuers.Size = New System.Drawing.Size(153, 20)
        Me.txtFoundationIssuers.TabIndex = 16
        Me.txtFoundationIssuers.Tag = "FOUNDATION_ISSUERS"
        '
        'lblFoundationIssuers
        '
        Me.lblFoundationIssuers.AutoSize = True
        Me.lblFoundationIssuers.Location = New System.Drawing.Point(370, 214)
        Me.lblFoundationIssuers.Name = "lblFoundationIssuers"
        Me.lblFoundationIssuers.Size = New System.Drawing.Size(101, 13)
        Me.lblFoundationIssuers.TabIndex = 42
        Me.lblFoundationIssuers.Tag = "IS_FOUNDATION_ISSUERS"
        Me.lblFoundationIssuers.Text = "lbFoundationIssuers"
        '
        'lbFoundationDate
        '
        Me.lbFoundationDate.AutoSize = True
        Me.lbFoundationDate.Location = New System.Drawing.Point(190, 210)
        Me.lbFoundationDate.Name = "lbFoundationDate"
        Me.lbFoundationDate.Size = New System.Drawing.Size(91, 13)
        Me.lbFoundationDate.TabIndex = 41
        Me.lbFoundationDate.Tag = "IS_FOUNDATION_DATE"
        Me.lbFoundationDate.Text = "lbFoundationDate"
        '
        'txtFoundationNo
        '
        Me.txtFoundationNo.Location = New System.Drawing.Point(87, 207)
        Me.txtFoundationNo.Name = "txtFoundationNo"
        Me.txtFoundationNo.Size = New System.Drawing.Size(100, 20)
        Me.txtFoundationNo.TabIndex = 14
        Me.txtFoundationNo.Tag = "FOUNDATION_NO"
        '
        'lbFoundationNo
        '
        Me.lbFoundationNo.AutoSize = True
        Me.lbFoundationNo.Location = New System.Drawing.Point(7, 210)
        Me.lbFoundationNo.Name = "lbFoundationNo"
        Me.lbFoundationNo.Size = New System.Drawing.Size(82, 13)
        Me.lbFoundationNo.TabIndex = 40
        Me.lbFoundationNo.Tag = "IS_FOUNDATION_NO"
        Me.lbFoundationNo.Text = "lbFoundationNo"
        '
        'txtBankName
        '
        Me.txtBankName.Location = New System.Drawing.Point(264, 153)
        Me.txtBankName.Name = "txtBankName"
        Me.txtBankName.Size = New System.Drawing.Size(344, 20)
        Me.txtBankName.TabIndex = 10
        Me.txtBankName.Tag = "BANK_NAME"
        '
        'lbBankName
        '
        Me.lbBankName.AutoSize = True
        Me.lbBankName.Location = New System.Drawing.Point(190, 160)
        Me.lbBankName.Name = "lbBankName"
        Me.lbBankName.Size = New System.Drawing.Size(68, 13)
        Me.lbBankName.TabIndex = 39
        Me.lbBankName.Tag = "IS_BANK_NAME"
        Me.lbBankName.Text = "lbBankName"
        '
        'txtBankAccount
        '
        Me.txtBankAccount.Location = New System.Drawing.Point(87, 155)
        Me.txtBankAccount.Name = "txtBankAccount"
        Me.txtBankAccount.Size = New System.Drawing.Size(100, 20)
        Me.txtBankAccount.TabIndex = 9
        Me.txtBankAccount.Tag = "BANK_ACCOUNT"
        '
        'lbBankAccount
        '
        Me.lbBankAccount.AutoSize = True
        Me.lbBankAccount.Location = New System.Drawing.Point(7, 160)
        Me.lbBankAccount.Name = "lbBankAccount"
        Me.lbBankAccount.Size = New System.Drawing.Size(80, 13)
        Me.lbBankAccount.TabIndex = 38
        Me.lbBankAccount.Tag = "IS_BANK_ACCOUNT"
        Me.lbBankAccount.Text = "lbBankAccount"
        '
        'txtBusinessField
        '
        Me.txtBusinessField.Location = New System.Drawing.Point(456, 102)
        Me.txtBusinessField.Name = "txtBusinessField"
        Me.txtBusinessField.Size = New System.Drawing.Size(153, 20)
        Me.txtBusinessField.TabIndex = 8
        Me.txtBusinessField.Tag = "BUSINESS_FIELD"
        '
        'lbBusinessField
        '
        Me.lbBusinessField.AutoSize = True
        Me.lbBusinessField.Location = New System.Drawing.Point(370, 108)
        Me.lbBusinessField.Name = "lbBusinessField"
        Me.lbBusinessField.Size = New System.Drawing.Size(79, 13)
        Me.lbBusinessField.TabIndex = 37
        Me.lbBusinessField.Tag = "IS_BUSINESS_FIELD"
        Me.lbBusinessField.Text = "lbBusinessField"
        '
        'lbFax
        '
        Me.lbFax.AutoSize = True
        Me.lbFax.Location = New System.Drawing.Point(192, 105)
        Me.lbFax.Name = "lbFax"
        Me.lbFax.Size = New System.Drawing.Size(32, 13)
        Me.lbFax.TabIndex = 36
        Me.lbFax.Tag = "IS_FAX"
        Me.lbFax.Text = "lbFax"
        '
        'lbPhone
        '
        Me.lbPhone.AutoSize = True
        Me.lbPhone.Location = New System.Drawing.Point(7, 105)
        Me.lbPhone.Name = "lbPhone"
        Me.lbPhone.Size = New System.Drawing.Size(46, 13)
        Me.lbPhone.TabIndex = 35
        Me.lbPhone.Tag = "IS_PHONE"
        Me.lbPhone.Text = "lbPhone"
        '
        'txtAddress
        '
        Me.txtAddress.Location = New System.Drawing.Point(87, 76)
        Me.txtAddress.Name = "txtAddress"
        Me.txtAddress.Size = New System.Drawing.Size(521, 20)
        Me.txtAddress.TabIndex = 5
        Me.txtAddress.Tag = "ADDRESS"
        '
        'lbAddress
        '
        Me.lbAddress.AutoSize = True
        Me.lbAddress.Location = New System.Drawing.Point(7, 79)
        Me.lbAddress.Name = "lbAddress"
        Me.lbAddress.Size = New System.Drawing.Size(53, 13)
        Me.lbAddress.TabIndex = 34
        Me.lbAddress.Tag = "IS_ADDRESS"
        Me.lbAddress.Text = "lbAddress"
        '
        'txtIsName
        '
        Me.txtIsName.Location = New System.Drawing.Point(360, 49)
        Me.txtIsName.Name = "txtIsName"
        Me.txtIsName.Size = New System.Drawing.Size(248, 20)
        Me.txtIsName.TabIndex = 4
        Me.txtIsName.Tag = "ISNAME"
        '
        'lbIsName
        '
        Me.lbIsName.AutoSize = True
        Me.lbIsName.Location = New System.Drawing.Point(7, 53)
        Me.lbIsName.Name = "lbIsName"
        Me.lbIsName.Size = New System.Drawing.Size(51, 13)
        Me.lbIsName.TabIndex = 33
        Me.lbIsName.Tag = "IS_NAME"
        Me.lbIsName.Text = "lbIsName"
        '
        'lbCode
        '
        Me.lbCode.AutoSize = True
        Me.lbCode.Location = New System.Drawing.Point(296, 26)
        Me.lbCode.Name = "lbCode"
        Me.lbCode.Size = New System.Drawing.Size(40, 13)
        Me.lbCode.TabIndex = 31
        Me.lbCode.Tag = "IS_CODE"
        Me.lbCode.Text = "lbCode"
        '
        'lbIsType
        '
        Me.lbIsType.AutoEllipsis = True
        Me.lbIsType.AutoSize = True
        Me.lbIsType.Location = New System.Drawing.Point(7, 26)
        Me.lbIsType.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.lbIsType.Name = "lbIsType"
        Me.lbIsType.Size = New System.Drawing.Size(47, 13)
        Me.lbIsType.TabIndex = 30
        Me.lbIsType.Tag = "IS_TYPE"
        Me.lbIsType.Text = "lbIsType"
        '
        'frmRGIS
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.ClientSize = New System.Drawing.Size(628, 421)
        Me.Controls.Add(Me.GrbGroups)
        Me.Name = "frmRGIS"
        Me.Controls.SetChildIndex(Me.btnOk, 0)
        Me.Controls.SetChildIndex(Me.btnCancel, 0)
        Me.Controls.SetChildIndex(Me.btnApply, 0)
        Me.Controls.SetChildIndex(Me.GrbGroups, 0)
        Me.Controls.SetChildIndex(Me.Panel1, 0)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.GrbGroups.ResumeLayout(False)
        Me.GrbGroups.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GrbGroups As System.Windows.Forms.GroupBox
    Friend WithEvents lbIsType As System.Windows.Forms.Label
    Friend WithEvents lbCode As System.Windows.Forms.Label
    Friend WithEvents lbShortName As System.Windows.Forms.Label
    Friend WithEvents txtIsName As System.Windows.Forms.TextBox
    Friend WithEvents lbIsName As System.Windows.Forms.Label
    Friend WithEvents txtShortName As System.Windows.Forms.TextBox
    Friend WithEvents lbBusinessField As System.Windows.Forms.Label
    Friend WithEvents lbFax As System.Windows.Forms.Label
    Friend WithEvents lbPhone As System.Windows.Forms.Label
    Friend WithEvents txtAddress As System.Windows.Forms.TextBox
    Friend WithEvents lbAddress As System.Windows.Forms.Label
    Friend WithEvents txtBankAccount As System.Windows.Forms.TextBox
    Friend WithEvents lbBankAccount As System.Windows.Forms.Label
    Friend WithEvents txtBusinessField As System.Windows.Forms.TextBox
    Friend WithEvents txtBankName As System.Windows.Forms.TextBox
    Friend WithEvents lbBankName As System.Windows.Forms.Label
    Friend WithEvents lbFoundationDate As System.Windows.Forms.Label
    Friend WithEvents txtFoundationNo As System.Windows.Forms.TextBox
    Friend WithEvents lbFoundationNo As System.Windows.Forms.Label
    Friend WithEvents txtFoundationIssuers As System.Windows.Forms.TextBox
    Friend WithEvents lblFoundationIssuers As System.Windows.Forms.Label
    Friend WithEvents txtLicenseNo As System.Windows.Forms.TextBox
    Friend WithEvents lbLicenseNo As System.Windows.Forms.Label
    Friend WithEvents txtLicenseIssuer As System.Windows.Forms.TextBox
    Friend WithEvents lbLicenseIssuer As System.Windows.Forms.Label
    Friend WithEvents lbLicenseDate As System.Windows.Forms.Label
    Friend WithEvents lbCapitalRule As System.Windows.Forms.Label
    Friend WithEvents txtNote As System.Windows.Forms.TextBox
    Friend WithEvents lbNote As System.Windows.Forms.Label
    Friend WithEvents cbIsType As Sats.AppCore.ComboBoxEx
    Friend WithEvents txtCapitalRule As System.Windows.Forms.TextBox
    Friend WithEvents dtpFoundationDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtpLicenseDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents txtCode As Sats.AppCore.FlexMaskEditBox
    Friend WithEvents txtFax As System.Windows.Forms.TextBox
    Friend WithEvents dtpBussinessDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents txtBussinessIssue As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lbBussinessDate As System.Windows.Forms.Label
    Friend WithEvents txtBussinessNo As System.Windows.Forms.TextBox
    Friend WithEvents lbBussinessNo As System.Windows.Forms.Label
    Friend WithEvents txtPhone As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents DateTimePicker2 As System.Windows.Forms.DateTimePicker
    Friend WithEvents DateTimePicker1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lblOperCate As System.Windows.Forms.Label
    Friend WithEvents cbIsOperCate As Sats.AppCore.ComboBoxEx

End Class
