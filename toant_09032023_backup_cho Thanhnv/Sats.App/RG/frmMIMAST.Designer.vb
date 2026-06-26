<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRGMI
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRGMI))
        Me.grbInfor = New System.Windows.Forms.GroupBox
        Me.cboNationality = New Sats.AppCore.ComboBoxEx
        Me.lbNationality = New System.Windows.Forms.Label
        Me.cboIsMember = New Sats.AppCore.ComboBoxEx
        Me.lbIsMember = New System.Windows.Forms.Label
        Me.fmebMiCode = New Sats.AppCore.FlexMaskEditBox
        Me.txtCapitalRule = New System.Windows.Forms.TextBox
        Me.txtCapital = New System.Windows.Forms.TextBox
        Me.cbCoMICODE = New Sats.AppCore.ComboBoxEx
        Me.cboBorfFlag = New Sats.AppCore.ComboBoxEx
        Me.cboSTATUS = New Sats.AppCore.ComboBoxEx
        Me.txtCodeTrade = New Sats.AppCore.FlexMaskEditBox
        Me.txtRetrievalDate = New System.Windows.Forms.DateTimePicker
        Me.txtFirmDate = New System.Windows.Forms.DateTimePicker
        Me.txtOperationDate = New System.Windows.Forms.DateTimePicker
        Me.txtBusinessDate = New System.Windows.Forms.DateTimePicker
        Me.txtFoundationDate = New System.Windows.Forms.DateTimePicker
        Me.cboType = New Sats.AppCore.ComboBoxEx
        Me.lbID = New System.Windows.Forms.Label
        Me.lbType1 = New System.Windows.Forms.Label
        Me.lbType2 = New System.Windows.Forms.Label
        Me.txtFax = New System.Windows.Forms.TextBox
        Me.txtNotes = New System.Windows.Forms.TextBox
        Me.txtFirmIssuer = New System.Windows.Forms.TextBox
        Me.txtOperationIssuer = New System.Windows.Forms.TextBox
        Me.txtBusinessIssuer = New System.Windows.Forms.TextBox
        Me.txtFoundationIssuer = New System.Windows.Forms.TextBox
        Me.txtBankBranch = New System.Windows.Forms.TextBox
        Me.txtBankName = New System.Windows.Forms.TextBox
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.txtRetrievalNo = New System.Windows.Forms.TextBox
        Me.txtFirmNo = New System.Windows.Forms.TextBox
        Me.txtOperationNo = New System.Windows.Forms.TextBox
        Me.txtBusinessNo = New System.Windows.Forms.TextBox
        Me.txtRepName = New System.Windows.Forms.TextBox
        Me.txtFoundationNo = New System.Windows.Forms.TextBox
        Me.txtBankAccount = New System.Windows.Forms.TextBox
        Me.txtPhone = New System.Windows.Forms.TextBox
        Me.txtNameGD = New System.Windows.Forms.TextBox
        Me.lbName = New System.Windows.Forms.Label
        Me.lbNameGD = New System.Windows.Forms.Label
        Me.txtName = New System.Windows.Forms.TextBox
        Me.lbCode = New System.Windows.Forms.Label
        Me.lbPhone = New System.Windows.Forms.Label
        Me.txtAddress = New System.Windows.Forms.TextBox
        Me.lbAddress = New System.Windows.Forms.Label
        Me.lbFax = New System.Windows.Forms.Label
        Me.lbGDBy = New System.Windows.Forms.Label
        Me.lbCapital = New System.Windows.Forms.Label
        Me.lbCapitalRule = New System.Windows.Forms.Label
        Me.lbStatus = New System.Windows.Forms.Label
        Me.lbBankName = New System.Windows.Forms.Label
        Me.lbBankBranch = New System.Windows.Forms.Label
        Me.lbBankAccount = New System.Windows.Forms.Label
        Me.lbFoundationNo = New System.Windows.Forms.Label
        Me.lbFoundationDate = New System.Windows.Forms.Label
        Me.lbFoundationIssuer = New System.Windows.Forms.Label
        Me.lbBusinessNo = New System.Windows.Forms.Label
        Me.lbBusinessDate = New System.Windows.Forms.Label
        Me.lbBusinessIssuer = New System.Windows.Forms.Label
        Me.lbOperationNo = New System.Windows.Forms.Label
        Me.lbOperationDate = New System.Windows.Forms.Label
        Me.lbOperationIssuer = New System.Windows.Forms.Label
        Me.lbFirmNo = New System.Windows.Forms.Label
        Me.lbFirmDate = New System.Windows.Forms.Label
        Me.lbFirmIssuer = New System.Windows.Forms.Label
        Me.lbRetrievalNo = New System.Windows.Forms.Label
        Me.lbRetrievalDate = New System.Windows.Forms.Label
        Me.lbRepName = New System.Windows.Forms.Label
        Me.lbRepID = New System.Windows.Forms.Label
        Me.lbNotes = New System.Windows.Forms.Label
        Me.Panel1.SuspendLayout()
        Me.grbInfor.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Size = New System.Drawing.Size(779, 42)
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(604, 435)
        Me.btnOk.Size = New System.Drawing.Size(80, 35)
        Me.btnOk.TabIndex = 101
        Me.btnOk.Tag = "btnOk"
        '
        'btnApply
        '
        Me.btnApply.Location = New System.Drawing.Point(518, 435)
        Me.btnApply.Size = New System.Drawing.Size(80, 35)
        Me.btnApply.TabIndex = 100
        Me.btnApply.Visible = False
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(690, 435)
        Me.btnCancel.Size = New System.Drawing.Size(80, 35)
        Me.btnCancel.TabIndex = 102
        '
        'grbInfor
        '
        Me.grbInfor.Controls.Add(Me.cboNationality)
        Me.grbInfor.Controls.Add(Me.lbNationality)
        Me.grbInfor.Controls.Add(Me.cboIsMember)
        Me.grbInfor.Controls.Add(Me.lbIsMember)
        Me.grbInfor.Controls.Add(Me.fmebMiCode)
        Me.grbInfor.Controls.Add(Me.txtCapitalRule)
        Me.grbInfor.Controls.Add(Me.txtCapital)
        Me.grbInfor.Controls.Add(Me.cbCoMICODE)
        Me.grbInfor.Controls.Add(Me.cboBorfFlag)
        Me.grbInfor.Controls.Add(Me.cboSTATUS)
        Me.grbInfor.Controls.Add(Me.txtCodeTrade)
        Me.grbInfor.Controls.Add(Me.txtRetrievalDate)
        Me.grbInfor.Controls.Add(Me.txtFirmDate)
        Me.grbInfor.Controls.Add(Me.txtOperationDate)
        Me.grbInfor.Controls.Add(Me.txtBusinessDate)
        Me.grbInfor.Controls.Add(Me.txtFoundationDate)
        Me.grbInfor.Controls.Add(Me.cboType)
        Me.grbInfor.Controls.Add(Me.lbID)
        Me.grbInfor.Controls.Add(Me.lbType1)
        Me.grbInfor.Controls.Add(Me.lbType2)
        Me.grbInfor.Controls.Add(Me.txtFax)
        Me.grbInfor.Controls.Add(Me.txtNotes)
        Me.grbInfor.Controls.Add(Me.txtFirmIssuer)
        Me.grbInfor.Controls.Add(Me.txtOperationIssuer)
        Me.grbInfor.Controls.Add(Me.txtBusinessIssuer)
        Me.grbInfor.Controls.Add(Me.txtFoundationIssuer)
        Me.grbInfor.Controls.Add(Me.txtBankBranch)
        Me.grbInfor.Controls.Add(Me.txtBankName)
        Me.grbInfor.Controls.Add(Me.TextBox1)
        Me.grbInfor.Controls.Add(Me.txtRetrievalNo)
        Me.grbInfor.Controls.Add(Me.txtFirmNo)
        Me.grbInfor.Controls.Add(Me.txtOperationNo)
        Me.grbInfor.Controls.Add(Me.txtBusinessNo)
        Me.grbInfor.Controls.Add(Me.txtRepName)
        Me.grbInfor.Controls.Add(Me.txtFoundationNo)
        Me.grbInfor.Controls.Add(Me.txtBankAccount)
        Me.grbInfor.Controls.Add(Me.txtPhone)
        Me.grbInfor.Controls.Add(Me.txtNameGD)
        Me.grbInfor.Controls.Add(Me.lbName)
        Me.grbInfor.Controls.Add(Me.lbNameGD)
        Me.grbInfor.Controls.Add(Me.txtName)
        Me.grbInfor.Controls.Add(Me.lbCode)
        Me.grbInfor.Controls.Add(Me.lbPhone)
        Me.grbInfor.Controls.Add(Me.txtAddress)
        Me.grbInfor.Controls.Add(Me.lbAddress)
        Me.grbInfor.Controls.Add(Me.lbFax)
        Me.grbInfor.Controls.Add(Me.lbGDBy)
        Me.grbInfor.Controls.Add(Me.lbCapital)
        Me.grbInfor.Controls.Add(Me.lbCapitalRule)
        Me.grbInfor.Controls.Add(Me.lbStatus)
        Me.grbInfor.Controls.Add(Me.lbBankName)
        Me.grbInfor.Controls.Add(Me.lbBankBranch)
        Me.grbInfor.Controls.Add(Me.lbBankAccount)
        Me.grbInfor.Controls.Add(Me.lbFoundationNo)
        Me.grbInfor.Controls.Add(Me.lbFoundationDate)
        Me.grbInfor.Controls.Add(Me.lbFoundationIssuer)
        Me.grbInfor.Controls.Add(Me.lbBusinessNo)
        Me.grbInfor.Controls.Add(Me.lbBusinessDate)
        Me.grbInfor.Controls.Add(Me.lbBusinessIssuer)
        Me.grbInfor.Controls.Add(Me.lbOperationNo)
        Me.grbInfor.Controls.Add(Me.lbOperationDate)
        Me.grbInfor.Controls.Add(Me.lbOperationIssuer)
        Me.grbInfor.Controls.Add(Me.lbFirmNo)
        Me.grbInfor.Controls.Add(Me.lbFirmDate)
        Me.grbInfor.Controls.Add(Me.lbFirmIssuer)
        Me.grbInfor.Controls.Add(Me.lbRetrievalNo)
        Me.grbInfor.Controls.Add(Me.lbRetrievalDate)
        Me.grbInfor.Controls.Add(Me.lbRepName)
        Me.grbInfor.Controls.Add(Me.lbRepID)
        Me.grbInfor.Controls.Add(Me.lbNotes)
        Me.grbInfor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grbInfor.Location = New System.Drawing.Point(11, 48)
        Me.grbInfor.Name = "grbInfor"
        Me.grbInfor.Size = New System.Drawing.Size(758, 381)
        Me.grbInfor.TabIndex = 0
        Me.grbInfor.TabStop = False
        Me.grbInfor.Tag = "grbInfor"
        Me.grbInfor.Text = "grbInfor"
        '
        'cboNationality
        '
        Me.cboNationality.DisplayMember = "DISPLAY"
        Me.cboNationality.FormattingEnabled = True
        Me.cboNationality.Location = New System.Drawing.Point(535, 51)
        Me.cboNationality.Name = "cboNationality"
        Me.cboNationality.Size = New System.Drawing.Size(207, 21)
        Me.cboNationality.TabIndex = 6
        Me.cboNationality.Tag = "NATIONALITY"
        Me.cboNationality.ValueMember = "VALUE"
        '
        'lbNationality
        '
        Me.lbNationality.AutoSize = True
        Me.lbNationality.Location = New System.Drawing.Point(457, 54)
        Me.lbNationality.Name = "lbNationality"
        Me.lbNationality.Size = New System.Drawing.Size(64, 13)
        Me.lbNationality.TabIndex = 112
        Me.lbNationality.Tag = "lbNationality"
        Me.lbNationality.Text = "lbNationality"
        '
        'cboIsMember
        '
        Me.cboIsMember.DisplayMember = "DISPLAY"
        Me.cboIsMember.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboIsMember.FormattingEnabled = True
        Me.cboIsMember.Location = New System.Drawing.Point(535, 110)
        Me.cboIsMember.Name = "cboIsMember"
        Me.cboIsMember.Size = New System.Drawing.Size(208, 21)
        Me.cboIsMember.TabIndex = 12
        Me.cboIsMember.Tag = "ISMEMBER"
        Me.cboIsMember.ValueMember = "VALUE"
        '
        'lbIsMember
        '
        Me.lbIsMember.AutoSize = True
        Me.lbIsMember.Location = New System.Drawing.Point(456, 113)
        Me.lbIsMember.Name = "lbIsMember"
        Me.lbIsMember.Size = New System.Drawing.Size(61, 13)
        Me.lbIsMember.TabIndex = 111
        Me.lbIsMember.Tag = "lbIsMember"
        Me.lbIsMember.Text = "lbIsMember"
        '
        'fmebMiCode
        '
        Me.fmebMiCode.Location = New System.Drawing.Point(124, 24)
        Me.fmebMiCode.Name = "fmebMiCode"
        Me.fmebMiCode.Size = New System.Drawing.Size(54, 20)
        Me.fmebMiCode.TabIndex = 1
        Me.fmebMiCode.Tag = "MICODE"
        '
        'txtCapitalRule
        '
        Me.txtCapitalRule.Location = New System.Drawing.Point(340, 139)
        Me.txtCapitalRule.Name = "txtCapitalRule"
        Me.txtCapitalRule.Size = New System.Drawing.Size(108, 20)
        Me.txtCapitalRule.TabIndex = 14
        Me.txtCapitalRule.Tag = "CAPITAL_RULE"
        '
        'txtCapital
        '
        Me.txtCapital.Location = New System.Drawing.Point(125, 138)
        Me.txtCapital.Name = "txtCapital"
        Me.txtCapital.Size = New System.Drawing.Size(113, 20)
        Me.txtCapital.TabIndex = 13
        Me.txtCapital.Tag = "CAPITAL"
        '
        'cbCoMICODE
        '
        Me.cbCoMICODE.DisplayMember = "DISPLAY"
        Me.cbCoMICODE.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbCoMICODE.FormattingEnabled = True
        Me.cbCoMICODE.Location = New System.Drawing.Point(666, 79)
        Me.cbCoMICODE.Name = "cbCoMICODE"
        Me.cbCoMICODE.Size = New System.Drawing.Size(77, 21)
        Me.cbCoMICODE.TabIndex = 9
        Me.cbCoMICODE.Tag = "CO_MICODE"
        Me.cbCoMICODE.ValueMember = "VALUE"
        '
        'cboBorfFlag
        '
        Me.cboBorfFlag.DisplayMember = "DISPLAY"
        Me.cboBorfFlag.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBorfFlag.FormattingEnabled = True
        Me.cboBorfFlag.Location = New System.Drawing.Point(535, 23)
        Me.cboBorfFlag.Name = "cboBorfFlag"
        Me.cboBorfFlag.Size = New System.Drawing.Size(208, 21)
        Me.cboBorfFlag.TabIndex = 4
        Me.cboBorfFlag.Tag = "BORF_FLAG"
        Me.cboBorfFlag.ValueMember = "VALUE"
        '
        'cboSTATUS
        '
        Me.cboSTATUS.DisplayMember = "DISPLAY"
        Me.cboSTATUS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSTATUS.FormattingEnabled = True
        Me.cboSTATUS.Location = New System.Drawing.Point(535, 140)
        Me.cboSTATUS.Name = "cboSTATUS"
        Me.cboSTATUS.Size = New System.Drawing.Size(208, 21)
        Me.cboSTATUS.TabIndex = 15
        Me.cboSTATUS.Tag = "STATUS"
        Me.cboSTATUS.ValueMember = "VALUE"
        '
        'txtCodeTrade
        '
        Me.txtCodeTrade.Location = New System.Drawing.Point(230, 24)
        Me.txtCodeTrade.Name = "txtCodeTrade"
        Me.txtCodeTrade.Size = New System.Drawing.Size(53, 20)
        Me.txtCodeTrade.TabIndex = 2
        Me.txtCodeTrade.Tag = "CODE_TRADE"
        '
        'txtRetrievalDate
        '
        Me.txtRetrievalDate.Checked = False
        Me.txtRetrievalDate.CustomFormat = "dd/MM/yyyy"
        Me.txtRetrievalDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.txtRetrievalDate.Location = New System.Drawing.Point(339, 318)
        Me.txtRetrievalDate.Name = "txtRetrievalDate"
        Me.txtRetrievalDate.ShowCheckBox = True
        Me.txtRetrievalDate.Size = New System.Drawing.Size(109, 20)
        Me.txtRetrievalDate.TabIndex = 32
        Me.txtRetrievalDate.Tag = "RETRIEVAL_DATE"
        '
        'txtFirmDate
        '
        Me.txtFirmDate.Checked = False
        Me.txtFirmDate.CustomFormat = "dd/MM/yyyy"
        Me.txtFirmDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.txtFirmDate.Location = New System.Drawing.Point(339, 287)
        Me.txtFirmDate.Name = "txtFirmDate"
        Me.txtFirmDate.ShowCheckBox = True
        Me.txtFirmDate.Size = New System.Drawing.Size(109, 20)
        Me.txtFirmDate.TabIndex = 29
        Me.txtFirmDate.Tag = "FIRM_DATE"
        '
        'txtOperationDate
        '
        Me.txtOperationDate.Checked = False
        Me.txtOperationDate.CustomFormat = "dd/MM/yyyy"
        Me.txtOperationDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.txtOperationDate.Location = New System.Drawing.Point(339, 259)
        Me.txtOperationDate.Name = "txtOperationDate"
        Me.txtOperationDate.ShowCheckBox = True
        Me.txtOperationDate.Size = New System.Drawing.Size(109, 20)
        Me.txtOperationDate.TabIndex = 26
        Me.txtOperationDate.Tag = "OPERATION_DATE"
        '
        'txtBusinessDate
        '
        Me.txtBusinessDate.Checked = False
        Me.txtBusinessDate.CustomFormat = "dd/MM/yyyy"
        Me.txtBusinessDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.txtBusinessDate.Location = New System.Drawing.Point(340, 229)
        Me.txtBusinessDate.Name = "txtBusinessDate"
        Me.txtBusinessDate.ShowCheckBox = True
        Me.txtBusinessDate.Size = New System.Drawing.Size(109, 20)
        Me.txtBusinessDate.TabIndex = 23
        Me.txtBusinessDate.Tag = "BUSINESS_DATE"
        '
        'txtFoundationDate
        '
        Me.txtFoundationDate.Checked = False
        Me.txtFoundationDate.CustomFormat = "dd/MM/yyyy"
        Me.txtFoundationDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.txtFoundationDate.Location = New System.Drawing.Point(340, 198)
        Me.txtFoundationDate.Name = "txtFoundationDate"
        Me.txtFoundationDate.ShowCheckBox = True
        Me.txtFoundationDate.Size = New System.Drawing.Size(109, 20)
        Me.txtFoundationDate.TabIndex = 20
        Me.txtFoundationDate.Tag = "FOUNDATION_DATE"
        '
        'cboType
        '
        Me.cboType.DisplayMember = "DISPLAY"
        Me.cboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboType.FormattingEnabled = True
        Me.cboType.Location = New System.Drawing.Point(362, 23)
        Me.cboType.Name = "cboType"
        Me.cboType.Size = New System.Drawing.Size(87, 21)
        Me.cboType.TabIndex = 3
        Me.cboType.Tag = "TYPE"
        Me.cboType.ValueMember = "VALUE"
        '
        'lbID
        '
        Me.lbID.AutoSize = True
        Me.lbID.Location = New System.Drawing.Point(8, 26)
        Me.lbID.Name = "lbID"
        Me.lbID.Size = New System.Drawing.Size(26, 13)
        Me.lbID.TabIndex = 76
        Me.lbID.Tag = "lbID"
        Me.lbID.Text = "lbID"
        '
        'lbType1
        '
        Me.lbType1.AutoSize = True
        Me.lbType1.Location = New System.Drawing.Point(289, 26)
        Me.lbType1.Name = "lbType1"
        Me.lbType1.Size = New System.Drawing.Size(45, 13)
        Me.lbType1.TabIndex = 79
        Me.lbType1.Tag = "lbType1"
        Me.lbType1.Text = "lbType1"
        '
        'lbType2
        '
        Me.lbType2.AutoSize = True
        Me.lbType2.Location = New System.Drawing.Point(457, 24)
        Me.lbType2.Name = "lbType2"
        Me.lbType2.Size = New System.Drawing.Size(45, 13)
        Me.lbType2.TabIndex = 81
        Me.lbType2.Tag = "lbType2"
        Me.lbType2.Text = "lbType2"
        '
        'txtFax
        '
        Me.txtFax.Location = New System.Drawing.Point(339, 109)
        Me.txtFax.Name = "txtFax"
        Me.txtFax.Size = New System.Drawing.Size(108, 20)
        Me.txtFax.TabIndex = 11
        Me.txtFax.Tag = "FAX"
        '
        'txtNotes
        '
        Me.txtNotes.Location = New System.Drawing.Point(535, 318)
        Me.txtNotes.Multiline = True
        Me.txtNotes.Name = "txtNotes"
        Me.txtNotes.Size = New System.Drawing.Size(207, 47)
        Me.txtNotes.TabIndex = 35
        Me.txtNotes.Tag = "NOTES"
        '
        'txtFirmIssuer
        '
        Me.txtFirmIssuer.Location = New System.Drawing.Point(535, 288)
        Me.txtFirmIssuer.Name = "txtFirmIssuer"
        Me.txtFirmIssuer.Size = New System.Drawing.Size(208, 20)
        Me.txtFirmIssuer.TabIndex = 30
        Me.txtFirmIssuer.Tag = "FIRM_ISSUER"
        '
        'txtOperationIssuer
        '
        Me.txtOperationIssuer.Location = New System.Drawing.Point(534, 258)
        Me.txtOperationIssuer.Name = "txtOperationIssuer"
        Me.txtOperationIssuer.Size = New System.Drawing.Size(208, 20)
        Me.txtOperationIssuer.TabIndex = 27
        Me.txtOperationIssuer.Tag = "OPERATION_ISSUER"
        '
        'txtBusinessIssuer
        '
        Me.txtBusinessIssuer.Location = New System.Drawing.Point(534, 229)
        Me.txtBusinessIssuer.Name = "txtBusinessIssuer"
        Me.txtBusinessIssuer.Size = New System.Drawing.Size(208, 20)
        Me.txtBusinessIssuer.TabIndex = 24
        Me.txtBusinessIssuer.Tag = "BUSINESS_ISSUER"
        '
        'txtFoundationIssuer
        '
        Me.txtFoundationIssuer.Location = New System.Drawing.Point(535, 201)
        Me.txtFoundationIssuer.Name = "txtFoundationIssuer"
        Me.txtFoundationIssuer.Size = New System.Drawing.Size(208, 20)
        Me.txtFoundationIssuer.TabIndex = 21
        Me.txtFoundationIssuer.Tag = "FOUNDATION_ISSUER"
        '
        'txtBankBranch
        '
        Me.txtBankBranch.Location = New System.Drawing.Point(535, 171)
        Me.txtBankBranch.Name = "txtBankBranch"
        Me.txtBankBranch.Size = New System.Drawing.Size(208, 20)
        Me.txtBankBranch.TabIndex = 18
        Me.txtBankBranch.Tag = "BANK_BRANCH"
        '
        'txtBankName
        '
        Me.txtBankName.Location = New System.Drawing.Point(340, 169)
        Me.txtBankName.Name = "txtBankName"
        Me.txtBankName.Size = New System.Drawing.Size(108, 20)
        Me.txtBankName.TabIndex = 17
        Me.txtBankName.Tag = "BANK_NAME"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(125, 350)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(113, 20)
        Me.TextBox1.TabIndex = 33
        Me.TextBox1.Tag = "REP_NAME"
        '
        'txtRetrievalNo
        '
        Me.txtRetrievalNo.Location = New System.Drawing.Point(125, 320)
        Me.txtRetrievalNo.Name = "txtRetrievalNo"
        Me.txtRetrievalNo.Size = New System.Drawing.Size(113, 20)
        Me.txtRetrievalNo.TabIndex = 31
        Me.txtRetrievalNo.Tag = "RETRIEVAL_NO"
        '
        'txtFirmNo
        '
        Me.txtFirmNo.Location = New System.Drawing.Point(124, 289)
        Me.txtFirmNo.Name = "txtFirmNo"
        Me.txtFirmNo.Size = New System.Drawing.Size(113, 20)
        Me.txtFirmNo.TabIndex = 28
        Me.txtFirmNo.Tag = "FIRM_NO"
        '
        'txtOperationNo
        '
        Me.txtOperationNo.Location = New System.Drawing.Point(125, 258)
        Me.txtOperationNo.Name = "txtOperationNo"
        Me.txtOperationNo.Size = New System.Drawing.Size(112, 20)
        Me.txtOperationNo.TabIndex = 25
        Me.txtOperationNo.Tag = "OPERATION_NO"
        '
        'txtBusinessNo
        '
        Me.txtBusinessNo.Location = New System.Drawing.Point(125, 227)
        Me.txtBusinessNo.Name = "txtBusinessNo"
        Me.txtBusinessNo.Size = New System.Drawing.Size(112, 20)
        Me.txtBusinessNo.TabIndex = 22
        Me.txtBusinessNo.Tag = "BUSINESS_NO"
        '
        'txtRepName
        '
        Me.txtRepName.Location = New System.Drawing.Point(339, 350)
        Me.txtRepName.Name = "txtRepName"
        Me.txtRepName.Size = New System.Drawing.Size(108, 20)
        Me.txtRepName.TabIndex = 34
        Me.txtRepName.Tag = "REP_ID"
        '
        'txtFoundationNo
        '
        Me.txtFoundationNo.Location = New System.Drawing.Point(125, 197)
        Me.txtFoundationNo.Name = "txtFoundationNo"
        Me.txtFoundationNo.Size = New System.Drawing.Size(112, 20)
        Me.txtFoundationNo.TabIndex = 19
        Me.txtFoundationNo.Tag = "FOUNDATION_NO"
        '
        'txtBankAccount
        '
        Me.txtBankAccount.Location = New System.Drawing.Point(125, 168)
        Me.txtBankAccount.Name = "txtBankAccount"
        Me.txtBankAccount.Size = New System.Drawing.Size(112, 20)
        Me.txtBankAccount.TabIndex = 16
        Me.txtBankAccount.Tag = "BANK_ACCOUNT"
        '
        'txtPhone
        '
        Me.txtPhone.Location = New System.Drawing.Point(125, 109)
        Me.txtPhone.Name = "txtPhone"
        Me.txtPhone.Size = New System.Drawing.Size(112, 20)
        Me.txtPhone.TabIndex = 10
        Me.txtPhone.Tag = "PHONE"
        '
        'txtNameGD
        '
        Me.txtNameGD.Location = New System.Drawing.Point(535, 80)
        Me.txtNameGD.Name = "txtNameGD"
        Me.txtNameGD.Size = New System.Drawing.Size(76, 20)
        Me.txtNameGD.TabIndex = 8
        Me.txtNameGD.Tag = "SHORT_NAME"
        '
        'lbName
        '
        Me.lbName.AutoSize = True
        Me.lbName.Location = New System.Drawing.Point(8, 54)
        Me.lbName.Name = "lbName"
        Me.lbName.Size = New System.Drawing.Size(43, 13)
        Me.lbName.TabIndex = 82
        Me.lbName.Tag = "lbName"
        Me.lbName.Text = "lbName"
        '
        'lbNameGD
        '
        Me.lbNameGD.AutoSize = True
        Me.lbNameGD.Location = New System.Drawing.Point(457, 82)
        Me.lbNameGD.Name = "lbNameGD"
        Me.lbNameGD.Size = New System.Drawing.Size(59, 13)
        Me.lbNameGD.TabIndex = 83
        Me.lbNameGD.Tag = "lbNameGD"
        Me.lbNameGD.Text = "lbNameGD"
        '
        'txtName
        '
        Me.txtName.Location = New System.Drawing.Point(124, 52)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(323, 20)
        Me.txtName.TabIndex = 5
        Me.txtName.Tag = "NAME"
        '
        'lbCode
        '
        Me.lbCode.AutoSize = True
        Me.lbCode.Location = New System.Drawing.Point(184, 26)
        Me.lbCode.Name = "lbCode"
        Me.lbCode.Size = New System.Drawing.Size(40, 13)
        Me.lbCode.TabIndex = 77
        Me.lbCode.Tag = "lbCode"
        Me.lbCode.Text = "lbCode"
        '
        'lbPhone
        '
        Me.lbPhone.AutoSize = True
        Me.lbPhone.Location = New System.Drawing.Point(9, 111)
        Me.lbPhone.Name = "lbPhone"
        Me.lbPhone.Size = New System.Drawing.Size(46, 13)
        Me.lbPhone.TabIndex = 86
        Me.lbPhone.Tag = "lbPhone"
        Me.lbPhone.Text = "lbPhone"
        '
        'txtAddress
        '
        Me.txtAddress.Location = New System.Drawing.Point(125, 80)
        Me.txtAddress.Name = "txtAddress"
        Me.txtAddress.Size = New System.Drawing.Size(323, 20)
        Me.txtAddress.TabIndex = 7
        Me.txtAddress.Tag = "ADDRESS"
        '
        'lbAddress
        '
        Me.lbAddress.AutoSize = True
        Me.lbAddress.Location = New System.Drawing.Point(8, 81)
        Me.lbAddress.Name = "lbAddress"
        Me.lbAddress.Size = New System.Drawing.Size(53, 13)
        Me.lbAddress.TabIndex = 85
        Me.lbAddress.Tag = "lbAddress"
        Me.lbAddress.Text = "lbAddress"
        '
        'lbFax
        '
        Me.lbFax.AutoSize = True
        Me.lbFax.Location = New System.Drawing.Point(255, 112)
        Me.lbFax.Name = "lbFax"
        Me.lbFax.Size = New System.Drawing.Size(32, 13)
        Me.lbFax.TabIndex = 87
        Me.lbFax.Tag = "lbFax"
        Me.lbFax.Text = "lbFax"
        '
        'lbGDBy
        '
        Me.lbGDBy.AutoSize = True
        Me.lbGDBy.Location = New System.Drawing.Point(617, 82)
        Me.lbGDBy.Name = "lbGDBy"
        Me.lbGDBy.Size = New System.Drawing.Size(43, 13)
        Me.lbGDBy.TabIndex = 84
        Me.lbGDBy.Tag = "lbGDBy"
        Me.lbGDBy.Text = "lbGDBy"
        '
        'lbCapital
        '
        Me.lbCapital.AutoSize = True
        Me.lbCapital.Location = New System.Drawing.Point(9, 142)
        Me.lbCapital.Name = "lbCapital"
        Me.lbCapital.Size = New System.Drawing.Size(47, 13)
        Me.lbCapital.TabIndex = 88
        Me.lbCapital.Tag = "lbCapital"
        Me.lbCapital.Text = "lbCapital"
        '
        'lbCapitalRule
        '
        Me.lbCapitalRule.AutoSize = True
        Me.lbCapitalRule.Location = New System.Drawing.Point(255, 143)
        Me.lbCapitalRule.Name = "lbCapitalRule"
        Me.lbCapitalRule.Size = New System.Drawing.Size(69, 13)
        Me.lbCapitalRule.TabIndex = 89
        Me.lbCapitalRule.Tag = "lbCapitalRule"
        Me.lbCapitalRule.Text = "lbCapitalRule"
        '
        'lbStatus
        '
        Me.lbStatus.AutoSize = True
        Me.lbStatus.Location = New System.Drawing.Point(457, 144)
        Me.lbStatus.Name = "lbStatus"
        Me.lbStatus.Size = New System.Drawing.Size(45, 13)
        Me.lbStatus.TabIndex = 90
        Me.lbStatus.Tag = "lbStatus"
        Me.lbStatus.Text = "lbStatus"
        '
        'lbBankName
        '
        Me.lbBankName.AutoSize = True
        Me.lbBankName.Location = New System.Drawing.Point(255, 174)
        Me.lbBankName.Name = "lbBankName"
        Me.lbBankName.Size = New System.Drawing.Size(68, 13)
        Me.lbBankName.TabIndex = 92
        Me.lbBankName.Tag = "lbBankName"
        Me.lbBankName.Text = "lbBankName"
        '
        'lbBankBranch
        '
        Me.lbBankBranch.AutoSize = True
        Me.lbBankBranch.Location = New System.Drawing.Point(456, 176)
        Me.lbBankBranch.Name = "lbBankBranch"
        Me.lbBankBranch.Size = New System.Drawing.Size(74, 13)
        Me.lbBankBranch.TabIndex = 93
        Me.lbBankBranch.Tag = "lbBankBranch"
        Me.lbBankBranch.Text = "lbBankBranch"
        '
        'lbBankAccount
        '
        Me.lbBankAccount.AutoSize = True
        Me.lbBankAccount.Location = New System.Drawing.Point(9, 171)
        Me.lbBankAccount.Name = "lbBankAccount"
        Me.lbBankAccount.Size = New System.Drawing.Size(80, 13)
        Me.lbBankAccount.TabIndex = 91
        Me.lbBankAccount.Tag = "lbBankAccount"
        Me.lbBankAccount.Text = "lbBankAccount"
        '
        'lbFoundationNo
        '
        Me.lbFoundationNo.AutoSize = True
        Me.lbFoundationNo.Location = New System.Drawing.Point(9, 200)
        Me.lbFoundationNo.Name = "lbFoundationNo"
        Me.lbFoundationNo.Size = New System.Drawing.Size(82, 13)
        Me.lbFoundationNo.TabIndex = 94
        Me.lbFoundationNo.Tag = "lbFoundationNo"
        Me.lbFoundationNo.Text = "lbFoundationNo"
        '
        'lbFoundationDate
        '
        Me.lbFoundationDate.AutoSize = True
        Me.lbFoundationDate.Location = New System.Drawing.Point(255, 201)
        Me.lbFoundationDate.Name = "lbFoundationDate"
        Me.lbFoundationDate.Size = New System.Drawing.Size(91, 13)
        Me.lbFoundationDate.TabIndex = 95
        Me.lbFoundationDate.Tag = "lbFoundationDate"
        Me.lbFoundationDate.Text = "lbFoundationDate"
        '
        'lbFoundationIssuer
        '
        Me.lbFoundationIssuer.AutoSize = True
        Me.lbFoundationIssuer.Location = New System.Drawing.Point(456, 203)
        Me.lbFoundationIssuer.Name = "lbFoundationIssuer"
        Me.lbFoundationIssuer.Size = New System.Drawing.Size(96, 13)
        Me.lbFoundationIssuer.TabIndex = 96
        Me.lbFoundationIssuer.Tag = "lbFoundationIssuer"
        Me.lbFoundationIssuer.Text = "lbFoundationIssuer"
        '
        'lbBusinessNo
        '
        Me.lbBusinessNo.AutoSize = True
        Me.lbBusinessNo.Location = New System.Drawing.Point(9, 229)
        Me.lbBusinessNo.Name = "lbBusinessNo"
        Me.lbBusinessNo.Size = New System.Drawing.Size(71, 13)
        Me.lbBusinessNo.TabIndex = 97
        Me.lbBusinessNo.Tag = "lbBusinessNo"
        Me.lbBusinessNo.Text = "lbBusinessNo"
        '
        'lbBusinessDate
        '
        Me.lbBusinessDate.AutoSize = True
        Me.lbBusinessDate.Location = New System.Drawing.Point(255, 230)
        Me.lbBusinessDate.Name = "lbBusinessDate"
        Me.lbBusinessDate.Size = New System.Drawing.Size(80, 13)
        Me.lbBusinessDate.TabIndex = 98
        Me.lbBusinessDate.Tag = "lbBusinessDate"
        Me.lbBusinessDate.Text = "lbBusinessDate"
        '
        'lbBusinessIssuer
        '
        Me.lbBusinessIssuer.AutoSize = True
        Me.lbBusinessIssuer.Location = New System.Drawing.Point(455, 233)
        Me.lbBusinessIssuer.Name = "lbBusinessIssuer"
        Me.lbBusinessIssuer.Size = New System.Drawing.Size(85, 13)
        Me.lbBusinessIssuer.TabIndex = 99
        Me.lbBusinessIssuer.Tag = "lbBusinessIssuer"
        Me.lbBusinessIssuer.Text = "lbBusinessIssuer"
        '
        'lbOperationNo
        '
        Me.lbOperationNo.AutoSize = True
        Me.lbOperationNo.Location = New System.Drawing.Point(9, 259)
        Me.lbOperationNo.Name = "lbOperationNo"
        Me.lbOperationNo.Size = New System.Drawing.Size(75, 13)
        Me.lbOperationNo.TabIndex = 100
        Me.lbOperationNo.Tag = "lbOperationNo"
        Me.lbOperationNo.Text = "lbOperationNo"
        '
        'lbOperationDate
        '
        Me.lbOperationDate.AutoSize = True
        Me.lbOperationDate.Location = New System.Drawing.Point(255, 258)
        Me.lbOperationDate.Name = "lbOperationDate"
        Me.lbOperationDate.Size = New System.Drawing.Size(84, 13)
        Me.lbOperationDate.TabIndex = 101
        Me.lbOperationDate.Tag = "lbOperationDate"
        Me.lbOperationDate.Text = "lbOperationDate"
        '
        'lbOperationIssuer
        '
        Me.lbOperationIssuer.AutoSize = True
        Me.lbOperationIssuer.Location = New System.Drawing.Point(455, 263)
        Me.lbOperationIssuer.Name = "lbOperationIssuer"
        Me.lbOperationIssuer.Size = New System.Drawing.Size(89, 13)
        Me.lbOperationIssuer.TabIndex = 102
        Me.lbOperationIssuer.Tag = "lbOperationIssuer"
        Me.lbOperationIssuer.Text = "lbOperationIssuer"
        '
        'lbFirmNo
        '
        Me.lbFirmNo.AutoSize = True
        Me.lbFirmNo.Location = New System.Drawing.Point(9, 291)
        Me.lbFirmNo.Name = "lbFirmNo"
        Me.lbFirmNo.Size = New System.Drawing.Size(48, 13)
        Me.lbFirmNo.TabIndex = 103
        Me.lbFirmNo.Tag = "lbFirmNo"
        Me.lbFirmNo.Text = "lbFirmNo"
        '
        'lbFirmDate
        '
        Me.lbFirmDate.AutoSize = True
        Me.lbFirmDate.Location = New System.Drawing.Point(255, 293)
        Me.lbFirmDate.Name = "lbFirmDate"
        Me.lbFirmDate.Size = New System.Drawing.Size(57, 13)
        Me.lbFirmDate.TabIndex = 104
        Me.lbFirmDate.Tag = "lbFirmDate"
        Me.lbFirmDate.Text = "lbFirmDate"
        '
        'lbFirmIssuer
        '
        Me.lbFirmIssuer.AutoSize = True
        Me.lbFirmIssuer.Location = New System.Drawing.Point(457, 288)
        Me.lbFirmIssuer.Name = "lbFirmIssuer"
        Me.lbFirmIssuer.Size = New System.Drawing.Size(62, 13)
        Me.lbFirmIssuer.TabIndex = 105
        Me.lbFirmIssuer.Tag = "lbFirmIssuer"
        Me.lbFirmIssuer.Text = "lbFirmIssuer"
        '
        'lbRetrievalNo
        '
        Me.lbRetrievalNo.AutoSize = True
        Me.lbRetrievalNo.Location = New System.Drawing.Point(9, 323)
        Me.lbRetrievalNo.Name = "lbRetrievalNo"
        Me.lbRetrievalNo.Size = New System.Drawing.Size(71, 13)
        Me.lbRetrievalNo.TabIndex = 106
        Me.lbRetrievalNo.Tag = "lbRetrievalNo"
        Me.lbRetrievalNo.Text = "lbRetrievalNo"
        '
        'lbRetrievalDate
        '
        Me.lbRetrievalDate.AutoSize = True
        Me.lbRetrievalDate.Location = New System.Drawing.Point(255, 326)
        Me.lbRetrievalDate.Name = "lbRetrievalDate"
        Me.lbRetrievalDate.Size = New System.Drawing.Size(80, 13)
        Me.lbRetrievalDate.TabIndex = 107
        Me.lbRetrievalDate.Tag = "lbRetrievalDate"
        Me.lbRetrievalDate.Text = "lbRetrievalDate"
        '
        'lbRepName
        '
        Me.lbRepName.AutoSize = True
        Me.lbRepName.Location = New System.Drawing.Point(9, 353)
        Me.lbRepName.Name = "lbRepName"
        Me.lbRepName.Size = New System.Drawing.Size(63, 13)
        Me.lbRepName.TabIndex = 108
        Me.lbRepName.Tag = "lbRepName"
        Me.lbRepName.Text = "lbRepName"
        '
        'lbRepID
        '
        Me.lbRepID.AutoSize = True
        Me.lbRepID.Location = New System.Drawing.Point(255, 352)
        Me.lbRepID.Name = "lbRepID"
        Me.lbRepID.Size = New System.Drawing.Size(46, 13)
        Me.lbRepID.TabIndex = 109
        Me.lbRepID.Tag = "lbRepID"
        Me.lbRepID.Text = "lbRepID"
        '
        'lbNotes
        '
        Me.lbNotes.AutoSize = True
        Me.lbNotes.Location = New System.Drawing.Point(459, 321)
        Me.lbNotes.Name = "lbNotes"
        Me.lbNotes.Size = New System.Drawing.Size(43, 13)
        Me.lbNotes.TabIndex = 110
        Me.lbNotes.Tag = "lbNotes"
        Me.lbNotes.Text = "lbNotes"
        '
        'frmRGMI
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.ClientSize = New System.Drawing.Size(779, 475)
        Me.Controls.Add(Me.grbInfor)
        Me.Name = "frmRGMI"
        Me.Controls.SetChildIndex(Me.btnApply, 0)
        Me.Controls.SetChildIndex(Me.btnCancel, 0)
        Me.Controls.SetChildIndex(Me.btnOk, 0)
        Me.Controls.SetChildIndex(Me.grbInfor, 0)
        Me.Controls.SetChildIndex(Me.Panel1, 0)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.grbInfor.ResumeLayout(False)
        Me.grbInfor.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
    Friend WithEvents grbInfor As System.Windows.Forms.GroupBox
    Friend WithEvents lbID As System.Windows.Forms.Label
    Friend WithEvents lbType1 As System.Windows.Forms.Label
    Friend WithEvents lbType2 As System.Windows.Forms.Label
    Friend WithEvents lbName As System.Windows.Forms.Label
    Friend WithEvents lbNameGD As System.Windows.Forms.Label
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents lbCode As System.Windows.Forms.Label
    Friend WithEvents lbPhone As System.Windows.Forms.Label
    Friend WithEvents txtAddress As System.Windows.Forms.TextBox
    Friend WithEvents lbAddress As System.Windows.Forms.Label
    Friend WithEvents lbFax As System.Windows.Forms.Label
    Friend WithEvents lbGDBy As System.Windows.Forms.Label
    Friend WithEvents lbCapital As System.Windows.Forms.Label
    Friend WithEvents lbCapitalRule As System.Windows.Forms.Label
    Friend WithEvents lbStatus As System.Windows.Forms.Label
    Friend WithEvents lbBankName As System.Windows.Forms.Label
    Friend WithEvents lbBankBranch As System.Windows.Forms.Label
    Friend WithEvents lbBankAccount As System.Windows.Forms.Label
    Friend WithEvents lbFoundationNo As System.Windows.Forms.Label
    Friend WithEvents lbFoundationDate As System.Windows.Forms.Label
    Friend WithEvents lbFoundationIssuer As System.Windows.Forms.Label
    Friend WithEvents lbBusinessNo As System.Windows.Forms.Label
    Friend WithEvents lbBusinessDate As System.Windows.Forms.Label
    Friend WithEvents lbBusinessIssuer As System.Windows.Forms.Label
    Friend WithEvents lbOperationNo As System.Windows.Forms.Label
    Friend WithEvents lbOperationDate As System.Windows.Forms.Label
    Friend WithEvents lbOperationIssuer As System.Windows.Forms.Label
    Friend WithEvents lbFirmNo As System.Windows.Forms.Label
    Friend WithEvents lbFirmDate As System.Windows.Forms.Label
    Friend WithEvents lbFirmIssuer As System.Windows.Forms.Label
    Friend WithEvents lbRetrievalNo As System.Windows.Forms.Label
    Friend WithEvents lbRetrievalDate As System.Windows.Forms.Label
    Friend WithEvents lbRepName As System.Windows.Forms.Label
    Friend WithEvents lbRepID As System.Windows.Forms.Label
    Friend WithEvents lbNotes As System.Windows.Forms.Label
    Friend WithEvents cboType As Sats.AppCore.ComboBoxEx
    Friend WithEvents txtNameGD As System.Windows.Forms.TextBox
    Friend WithEvents txtPhone As System.Windows.Forms.TextBox
    Friend WithEvents txtFax As System.Windows.Forms.TextBox
    Friend WithEvents txtBankName As System.Windows.Forms.TextBox
    Friend WithEvents txtBankBranch As System.Windows.Forms.TextBox
    Friend WithEvents txtFoundationNo As System.Windows.Forms.TextBox
    Friend WithEvents txtBusinessNo As System.Windows.Forms.TextBox
    Friend WithEvents txtOperationNo As System.Windows.Forms.TextBox
    Friend WithEvents txtFirmNo As System.Windows.Forms.TextBox
    Friend WithEvents txtRetrievalNo As System.Windows.Forms.TextBox
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents txtBankAccount As System.Windows.Forms.TextBox
    Friend WithEvents txtRepName As System.Windows.Forms.TextBox
    Friend WithEvents txtFoundationIssuer As System.Windows.Forms.TextBox
    Friend WithEvents txtBusinessIssuer As System.Windows.Forms.TextBox
    Friend WithEvents txtOperationIssuer As System.Windows.Forms.TextBox
    Friend WithEvents txtFirmIssuer As System.Windows.Forms.TextBox
    Friend WithEvents txtNotes As System.Windows.Forms.TextBox
    Friend WithEvents txtRetrievalDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents txtFirmDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents txtOperationDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents txtBusinessDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents txtFoundationDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents cboBorfFlag As Sats.AppCore.ComboBoxEx
    Friend WithEvents txtCodeTrade As Sats.AppCore.FlexMaskEditBox
    Friend WithEvents cboSTATUS As Sats.AppCore.ComboBoxEx
    Friend WithEvents cbCoMICODE As Sats.AppCore.ComboBoxEx
    Friend WithEvents txtCapitalRule As System.Windows.Forms.TextBox
    Friend WithEvents txtCapital As System.Windows.Forms.TextBox
    Friend WithEvents fmebMiCode As Sats.AppCore.FlexMaskEditBox
    Friend WithEvents lbIsMember As System.Windows.Forms.Label
    Friend WithEvents cboIsMember As Sats.AppCore.ComboBoxEx
    Friend WithEvents lbNationality As System.Windows.Forms.Label
    Friend WithEvents cboNationality As Sats.AppCore.ComboBoxEx
End Class
