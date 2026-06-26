<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRGII
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRGII))
        Me.tctRGII = New System.Windows.Forms.TabControl
        Me.tpPersonal = New System.Windows.Forms.TabPage
        Me.txtForeignNo = New System.Windows.Forms.TextBox
        Me.lbForeignNo = New System.Windows.Forms.Label
        Me.cboCurrentNationaly = New Sats.AppCore.ComboBoxEx
        Me.dtpBirthDate = New System.Windows.Forms.DateTimePicker
        Me.cboOriginalNationaly = New Sats.AppCore.ComboBoxEx
        Me.txtMobiphone = New System.Windows.Forms.TextBox
        Me.lbMobiphone = New System.Windows.Forms.Label
        Me.cboSex = New Sats.AppCore.ComboBoxEx
        Me.lbSex = New System.Windows.Forms.Label
        Me.txtNote = New System.Windows.Forms.TextBox
        Me.lbDesc = New System.Windows.Forms.Label
        Me.txtEmail = New System.Windows.Forms.TextBox
        Me.lbEmail = New System.Windows.Forms.Label
        Me.txtFax = New System.Windows.Forms.TextBox
        Me.lbFax = New System.Windows.Forms.Label
        Me.txtPhone = New System.Windows.Forms.TextBox
        Me.lbPhone = New System.Windows.Forms.Label
        Me.txtPost = New System.Windows.Forms.TextBox
        Me.lbFunction = New System.Windows.Forms.Label
        Me.txtOccupation = New System.Windows.Forms.TextBox
        Me.lbCareer = New System.Windows.Forms.Label
        Me.txtEducationLevel = New System.Windows.Forms.TextBox
        Me.lbEduLevel = New System.Windows.Forms.Label
        Me.txtAddress = New System.Windows.Forms.TextBox
        Me.lbAddress = New System.Windows.Forms.Label
        Me.txtNation = New System.Windows.Forms.TextBox
        Me.lbNation = New System.Windows.Forms.Label
        Me.lbCurrentNationaly = New System.Windows.Forms.Label
        Me.lbOriginNationaly = New System.Windows.Forms.Label
        Me.txtCardIssue = New System.Windows.Forms.TextBox
        Me.lbCardIssue = New System.Windows.Forms.Label
        Me.dtpCardDate = New System.Windows.Forms.DateTimePicker
        Me.lbCardDate = New System.Windows.Forms.Label
        Me.txtCardNo = New System.Windows.Forms.TextBox
        Me.lbCardNo = New System.Windows.Forms.Label
        Me.cboCardType = New Sats.AppCore.ComboBoxEx
        Me.lbCardType = New System.Windows.Forms.Label
        Me.txtBirthPlace = New System.Windows.Forms.TextBox
        Me.lbBirthPlace = New System.Windows.Forms.Label
        Me.lbBirthDay = New System.Windows.Forms.Label
        Me.txtFullName = New System.Windows.Forms.TextBox
        Me.lbFullName = New System.Windows.Forms.Label
        Me.cboType = New Sats.AppCore.ComboBoxEx
        Me.lbType = New System.Windows.Forms.Label
        Me.tpOrganization = New System.Windows.Forms.TabPage
        Me.txtOrgForeignNo = New System.Windows.Forms.TextBox
        Me.lbOrgForeignNo = New System.Windows.Forms.Label
        Me.lstOrgRepPerson = New System.Windows.Forms.ListBox
        Me.dtpFoundationDate = New System.Windows.Forms.DateTimePicker
        Me.lbFoundationDate = New System.Windows.Forms.Label
        Me.txtFoudationNo = New System.Windows.Forms.TextBox
        Me.lbFoundationNo = New System.Windows.Forms.Label
        Me.lbOrgDesc = New System.Windows.Forms.Label
        Me.txtOrgDesc = New System.Windows.Forms.TextBox
        Me.dtpOrgCardDate = New System.Windows.Forms.DateTimePicker
        Me.lbShortName = New System.Windows.Forms.Label
        Me.txtOrgShortName = New System.Windows.Forms.TextBox
        Me.txtOrgTransName = New System.Windows.Forms.TextBox
        Me.lbRGIIRep = New System.Windows.Forms.Label
        Me.txtFoundationIssue = New System.Windows.Forms.TextBox
        Me.lbFoundationIssue = New System.Windows.Forms.Label
        Me.lbOrgCardDate = New System.Windows.Forms.Label
        Me.txtOrgCardNo = New System.Windows.Forms.TextBox
        Me.lbOrgCardNo = New System.Windows.Forms.Label
        Me.txtOrgFieldBusiness = New System.Windows.Forms.TextBox
        Me.lbFieldBusiness = New System.Windows.Forms.Label
        Me.txtOrgFax = New System.Windows.Forms.TextBox
        Me.lbOrgFax = New System.Windows.Forms.Label
        Me.txtOrgPhone = New System.Windows.Forms.TextBox
        Me.lbOrgPhone = New System.Windows.Forms.Label
        Me.txtOrgAddress = New System.Windows.Forms.TextBox
        Me.lbOrgAddress = New System.Windows.Forms.Label
        Me.lbTransName = New System.Windows.Forms.Label
        Me.txtOrgFullName = New System.Windows.Forms.TextBox
        Me.lbOrgName = New System.Windows.Forms.Label
        Me.cboOrgType = New Sats.AppCore.ComboBoxEx
        Me.lbType2 = New System.Windows.Forms.Label
        Me.Panel1.SuspendLayout()
        Me.tctRGII.SuspendLayout()
        Me.tpPersonal.SuspendLayout()
        Me.tpOrganization.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Size = New System.Drawing.Size(706, 42)
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(524, 358)
        Me.btnOk.TabIndex = 47
        '
        'btnApply
        '
        Me.btnApply.Location = New System.Drawing.Point(434, 358)
        Me.btnApply.TabIndex = 45
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(614, 358)
        Me.btnCancel.TabIndex = 49
        '
        'tctRGII
        '
        Me.tctRGII.Controls.Add(Me.tpPersonal)
        Me.tctRGII.Controls.Add(Me.tpOrganization)
        Me.tctRGII.Location = New System.Drawing.Point(12, 48)
        Me.tctRGII.Name = "tctRGII"
        Me.tctRGII.RightToLeftLayout = True
        Me.tctRGII.SelectedIndex = 0
        Me.tctRGII.Size = New System.Drawing.Size(682, 304)
        Me.tctRGII.TabIndex = 4
        Me.tctRGII.Tag = "tctRGII"
        '
        'tpPersonal
        '
        Me.tpPersonal.Controls.Add(Me.txtForeignNo)
        Me.tpPersonal.Controls.Add(Me.lbForeignNo)
        Me.tpPersonal.Controls.Add(Me.cboCurrentNationaly)
        Me.tpPersonal.Controls.Add(Me.dtpBirthDate)
        Me.tpPersonal.Controls.Add(Me.cboOriginalNationaly)
        Me.tpPersonal.Controls.Add(Me.txtMobiphone)
        Me.tpPersonal.Controls.Add(Me.lbMobiphone)
        Me.tpPersonal.Controls.Add(Me.cboSex)
        Me.tpPersonal.Controls.Add(Me.lbSex)
        Me.tpPersonal.Controls.Add(Me.txtNote)
        Me.tpPersonal.Controls.Add(Me.lbDesc)
        Me.tpPersonal.Controls.Add(Me.txtEmail)
        Me.tpPersonal.Controls.Add(Me.lbEmail)
        Me.tpPersonal.Controls.Add(Me.txtFax)
        Me.tpPersonal.Controls.Add(Me.lbFax)
        Me.tpPersonal.Controls.Add(Me.txtPhone)
        Me.tpPersonal.Controls.Add(Me.lbPhone)
        Me.tpPersonal.Controls.Add(Me.txtPost)
        Me.tpPersonal.Controls.Add(Me.lbFunction)
        Me.tpPersonal.Controls.Add(Me.txtOccupation)
        Me.tpPersonal.Controls.Add(Me.lbCareer)
        Me.tpPersonal.Controls.Add(Me.txtEducationLevel)
        Me.tpPersonal.Controls.Add(Me.lbEduLevel)
        Me.tpPersonal.Controls.Add(Me.txtAddress)
        Me.tpPersonal.Controls.Add(Me.lbAddress)
        Me.tpPersonal.Controls.Add(Me.txtNation)
        Me.tpPersonal.Controls.Add(Me.lbNation)
        Me.tpPersonal.Controls.Add(Me.lbCurrentNationaly)
        Me.tpPersonal.Controls.Add(Me.lbOriginNationaly)
        Me.tpPersonal.Controls.Add(Me.txtCardIssue)
        Me.tpPersonal.Controls.Add(Me.lbCardIssue)
        Me.tpPersonal.Controls.Add(Me.dtpCardDate)
        Me.tpPersonal.Controls.Add(Me.lbCardDate)
        Me.tpPersonal.Controls.Add(Me.txtCardNo)
        Me.tpPersonal.Controls.Add(Me.lbCardNo)
        Me.tpPersonal.Controls.Add(Me.cboCardType)
        Me.tpPersonal.Controls.Add(Me.lbCardType)
        Me.tpPersonal.Controls.Add(Me.txtBirthPlace)
        Me.tpPersonal.Controls.Add(Me.lbBirthPlace)
        Me.tpPersonal.Controls.Add(Me.lbBirthDay)
        Me.tpPersonal.Controls.Add(Me.txtFullName)
        Me.tpPersonal.Controls.Add(Me.lbFullName)
        Me.tpPersonal.Controls.Add(Me.cboType)
        Me.tpPersonal.Controls.Add(Me.lbType)
        Me.tpPersonal.Location = New System.Drawing.Point(4, 22)
        Me.tpPersonal.Name = "tpPersonal"
        Me.tpPersonal.Padding = New System.Windows.Forms.Padding(3)
        Me.tpPersonal.Size = New System.Drawing.Size(674, 278)
        Me.tpPersonal.TabIndex = 0
        Me.tpPersonal.Tag = "tpPersonal"
        Me.tpPersonal.Text = "tpPersonal"
        Me.tpPersonal.UseVisualStyleBackColor = True
        '
        'txtForeignNo
        '
        Me.txtForeignNo.Location = New System.Drawing.Point(537, 228)
        Me.txtForeignNo.Name = "txtForeignNo"
        Me.txtForeignNo.Size = New System.Drawing.Size(124, 20)
        Me.txtForeignNo.TabIndex = 106
        Me.txtForeignNo.Tag = "FOREIGNNO"
        '
        'lbForeignNo
        '
        Me.lbForeignNo.AutoSize = True
        Me.lbForeignNo.Location = New System.Drawing.Point(448, 233)
        Me.lbForeignNo.Name = "lbForeignNo"
        Me.lbForeignNo.Size = New System.Drawing.Size(64, 13)
        Me.lbForeignNo.TabIndex = 105
        Me.lbForeignNo.Tag = "lbForeignNo"
        Me.lbForeignNo.Text = "lbForeignNo"
        '
        'cboCurrentNationaly
        '
        Me.cboCurrentNationaly.DisplayMember = "DISPLAY"
        Me.cboCurrentNationaly.FormattingEnabled = True
        Me.cboCurrentNationaly.Location = New System.Drawing.Point(316, 121)
        Me.cboCurrentNationaly.Name = "cboCurrentNationaly"
        Me.cboCurrentNationaly.Size = New System.Drawing.Size(127, 21)
        Me.cboCurrentNationaly.TabIndex = 23
        Me.cboCurrentNationaly.Tag = "CURRENT_NATIONALY"
        Me.cboCurrentNationaly.ValueMember = "VALUE"
        '
        'dtpBirthDate
        '
        Me.dtpBirthDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpBirthDate.Location = New System.Drawing.Point(90, 66)
        Me.dtpBirthDate.Name = "dtpBirthDate"
        Me.dtpBirthDate.ShowCheckBox = True
        Me.dtpBirthDate.Size = New System.Drawing.Size(128, 20)
        Me.dtpBirthDate.TabIndex = 10
        Me.dtpBirthDate.Tag = "BIRTH_DATE"
        Me.dtpBirthDate.Value = New Date(2008, 4, 24, 0, 0, 0, 0)
        '
        'cboOriginalNationaly
        '
        Me.cboOriginalNationaly.DisplayMember = "DISPLAY"
        Me.cboOriginalNationaly.FormattingEnabled = True
        Me.cboOriginalNationaly.Location = New System.Drawing.Point(90, 119)
        Me.cboOriginalNationaly.Name = "cboOriginalNationaly"
        Me.cboOriginalNationaly.Size = New System.Drawing.Size(127, 21)
        Me.cboOriginalNationaly.TabIndex = 21
        Me.cboOriginalNationaly.Tag = "ORIGINAL_NATIONALY"
        Me.cboOriginalNationaly.ValueMember = "VALUE"
        '
        'txtMobiphone
        '
        Me.txtMobiphone.Location = New System.Drawing.Point(317, 202)
        Me.txtMobiphone.Name = "txtMobiphone"
        Me.txtMobiphone.Size = New System.Drawing.Size(127, 20)
        Me.txtMobiphone.TabIndex = 37
        Me.txtMobiphone.Tag = "MOBILEPHONE"
        '
        'lbMobiphone
        '
        Me.lbMobiphone.AutoSize = True
        Me.lbMobiphone.Location = New System.Drawing.Point(224, 204)
        Me.lbMobiphone.Name = "lbMobiphone"
        Me.lbMobiphone.Size = New System.Drawing.Size(68, 13)
        Me.lbMobiphone.TabIndex = 50
        Me.lbMobiphone.Tag = "lbMobiphone"
        Me.lbMobiphone.Text = "lbMobiphone"
        '
        'cboSex
        '
        Me.cboSex.DisplayMember = "DISPLAY"
        Me.cboSex.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSex.FormattingEnabled = True
        Me.cboSex.Location = New System.Drawing.Point(316, 40)
        Me.cboSex.Name = "cboSex"
        Me.cboSex.Size = New System.Drawing.Size(128, 21)
        Me.cboSex.TabIndex = 6
        Me.cboSex.Tag = "SEX"
        Me.cboSex.ValueMember = "VALUE"
        '
        'lbSex
        '
        Me.lbSex.AutoSize = True
        Me.lbSex.Location = New System.Drawing.Point(224, 41)
        Me.lbSex.Name = "lbSex"
        Me.lbSex.Size = New System.Drawing.Size(33, 13)
        Me.lbSex.TabIndex = 48
        Me.lbSex.Tag = "lbSex"
        Me.lbSex.Text = "lbSex"
        '
        'txtNote
        '
        Me.txtNote.Location = New System.Drawing.Point(90, 255)
        Me.txtNote.Name = "txtNote"
        Me.txtNote.Size = New System.Drawing.Size(571, 20)
        Me.txtNote.TabIndex = 43
        Me.txtNote.Tag = "NOTE"
        '
        'lbDesc
        '
        Me.lbDesc.AutoSize = True
        Me.lbDesc.Location = New System.Drawing.Point(6, 255)
        Me.lbDesc.Name = "lbDesc"
        Me.lbDesc.Size = New System.Drawing.Size(40, 13)
        Me.lbDesc.TabIndex = 46
        Me.lbDesc.Tag = "lbDesc"
        Me.lbDesc.Text = "lbDesc"
        '
        'txtEmail
        '
        Me.txtEmail.Location = New System.Drawing.Point(90, 229)
        Me.txtEmail.Name = "txtEmail"
        Me.txtEmail.Size = New System.Drawing.Size(354, 20)
        Me.txtEmail.TabIndex = 41
        Me.txtEmail.Tag = "EMAIL"
        '
        'lbEmail
        '
        Me.lbEmail.AutoSize = True
        Me.lbEmail.Location = New System.Drawing.Point(6, 232)
        Me.lbEmail.Name = "lbEmail"
        Me.lbEmail.Size = New System.Drawing.Size(40, 13)
        Me.lbEmail.TabIndex = 44
        Me.lbEmail.Tag = "lbEmail"
        Me.lbEmail.Text = "lbEmail"
        '
        'txtFax
        '
        Me.txtFax.Location = New System.Drawing.Point(537, 200)
        Me.txtFax.Name = "txtFax"
        Me.txtFax.Size = New System.Drawing.Size(125, 20)
        Me.txtFax.TabIndex = 39
        Me.txtFax.Tag = "FAX"
        '
        'lbFax
        '
        Me.lbFax.AutoSize = True
        Me.lbFax.Location = New System.Drawing.Point(449, 205)
        Me.lbFax.Name = "lbFax"
        Me.lbFax.Size = New System.Drawing.Size(32, 13)
        Me.lbFax.TabIndex = 42
        Me.lbFax.Tag = "lbFax"
        Me.lbFax.Text = "lbFax"
        '
        'txtPhone
        '
        Me.txtPhone.Location = New System.Drawing.Point(90, 201)
        Me.txtPhone.Name = "txtPhone"
        Me.txtPhone.Size = New System.Drawing.Size(128, 20)
        Me.txtPhone.TabIndex = 35
        Me.txtPhone.Tag = "PHONE"
        '
        'lbPhone
        '
        Me.lbPhone.AutoSize = True
        Me.lbPhone.Location = New System.Drawing.Point(6, 205)
        Me.lbPhone.Name = "lbPhone"
        Me.lbPhone.Size = New System.Drawing.Size(46, 13)
        Me.lbPhone.TabIndex = 40
        Me.lbPhone.Tag = "lbPhone"
        Me.lbPhone.Text = "lbPhone"
        '
        'txtPost
        '
        Me.txtPost.Location = New System.Drawing.Point(537, 174)
        Me.txtPost.Name = "txtPost"
        Me.txtPost.Size = New System.Drawing.Size(124, 20)
        Me.txtPost.TabIndex = 33
        Me.txtPost.Tag = "POST"
        '
        'lbFunction
        '
        Me.lbFunction.AutoSize = True
        Me.lbFunction.Location = New System.Drawing.Point(448, 178)
        Me.lbFunction.Name = "lbFunction"
        Me.lbFunction.Size = New System.Drawing.Size(56, 13)
        Me.lbFunction.TabIndex = 38
        Me.lbFunction.Tag = "lbFunction"
        Me.lbFunction.Text = "lbFunction"
        '
        'txtOccupation
        '
        Me.txtOccupation.Location = New System.Drawing.Point(317, 174)
        Me.txtOccupation.Name = "txtOccupation"
        Me.txtOccupation.Size = New System.Drawing.Size(127, 20)
        Me.txtOccupation.TabIndex = 31
        Me.txtOccupation.Tag = "OCCUPATION"
        '
        'lbCareer
        '
        Me.lbCareer.AutoSize = True
        Me.lbCareer.Location = New System.Drawing.Point(224, 178)
        Me.lbCareer.Name = "lbCareer"
        Me.lbCareer.Size = New System.Drawing.Size(46, 13)
        Me.lbCareer.TabIndex = 36
        Me.lbCareer.Tag = "lbCareer"
        Me.lbCareer.Text = "lbCareer"
        '
        'txtEducationLevel
        '
        Me.txtEducationLevel.Location = New System.Drawing.Point(90, 174)
        Me.txtEducationLevel.Name = "txtEducationLevel"
        Me.txtEducationLevel.Size = New System.Drawing.Size(128, 20)
        Me.txtEducationLevel.TabIndex = 29
        Me.txtEducationLevel.Tag = "EDUCATION_LEVEL"
        '
        'lbEduLevel
        '
        Me.lbEduLevel.AutoSize = True
        Me.lbEduLevel.Location = New System.Drawing.Point(6, 178)
        Me.lbEduLevel.Name = "lbEduLevel"
        Me.lbEduLevel.Size = New System.Drawing.Size(60, 13)
        Me.lbEduLevel.TabIndex = 34
        Me.lbEduLevel.Tag = "lbEduLevel"
        Me.lbEduLevel.Text = "lbEduLevel"
        '
        'txtAddress
        '
        Me.txtAddress.Location = New System.Drawing.Point(90, 147)
        Me.txtAddress.Name = "txtAddress"
        Me.txtAddress.Size = New System.Drawing.Size(572, 20)
        Me.txtAddress.TabIndex = 27
        Me.txtAddress.Tag = "ADDRESS"
        '
        'lbAddress
        '
        Me.lbAddress.AutoSize = True
        Me.lbAddress.Location = New System.Drawing.Point(6, 150)
        Me.lbAddress.Name = "lbAddress"
        Me.lbAddress.Size = New System.Drawing.Size(53, 13)
        Me.lbAddress.TabIndex = 32
        Me.lbAddress.Tag = "lbAddress"
        Me.lbAddress.Text = "lbAddress"
        '
        'txtNation
        '
        Me.txtNation.Location = New System.Drawing.Point(537, 120)
        Me.txtNation.Name = "txtNation"
        Me.txtNation.Size = New System.Drawing.Size(125, 20)
        Me.txtNation.TabIndex = 25
        Me.txtNation.Tag = "NATION"
        '
        'lbNation
        '
        Me.lbNation.AutoSize = True
        Me.lbNation.Location = New System.Drawing.Point(450, 123)
        Me.lbNation.Name = "lbNation"
        Me.lbNation.Size = New System.Drawing.Size(46, 13)
        Me.lbNation.TabIndex = 30
        Me.lbNation.Tag = "lbNation"
        Me.lbNation.Text = "lbNation"
        '
        'lbCurrentNationaly
        '
        Me.lbCurrentNationaly.AutoSize = True
        Me.lbCurrentNationaly.Location = New System.Drawing.Point(224, 127)
        Me.lbCurrentNationaly.Name = "lbCurrentNationaly"
        Me.lbCurrentNationaly.Size = New System.Drawing.Size(93, 13)
        Me.lbCurrentNationaly.TabIndex = 28
        Me.lbCurrentNationaly.Tag = "lbCurrentNationaly"
        Me.lbCurrentNationaly.Text = "lbCurrentNationaly"
        '
        'lbOriginNationaly
        '
        Me.lbOriginNationaly.AutoSize = True
        Me.lbOriginNationaly.Location = New System.Drawing.Point(6, 124)
        Me.lbOriginNationaly.Name = "lbOriginNationaly"
        Me.lbOriginNationaly.Size = New System.Drawing.Size(86, 13)
        Me.lbOriginNationaly.TabIndex = 26
        Me.lbOriginNationaly.Tag = "lbOriginNationaly"
        Me.lbOriginNationaly.Text = "lbOriginNationaly"
        '
        'txtCardIssue
        '
        Me.txtCardIssue.Location = New System.Drawing.Point(537, 93)
        Me.txtCardIssue.Name = "txtCardIssue"
        Me.txtCardIssue.Size = New System.Drawing.Size(125, 20)
        Me.txtCardIssue.TabIndex = 19
        Me.txtCardIssue.Tag = "CARDISSUE"
        '
        'lbCardIssue
        '
        Me.lbCardIssue.AutoSize = True
        Me.lbCardIssue.Location = New System.Drawing.Point(450, 96)
        Me.lbCardIssue.Name = "lbCardIssue"
        Me.lbCardIssue.Size = New System.Drawing.Size(62, 13)
        Me.lbCardIssue.TabIndex = 24
        Me.lbCardIssue.Tag = "lbCardIssue"
        Me.lbCardIssue.Text = "lbCardIssue"
        '
        'dtpCardDate
        '
        Me.dtpCardDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpCardDate.Location = New System.Drawing.Point(317, 94)
        Me.dtpCardDate.Name = "dtpCardDate"
        Me.dtpCardDate.ShowCheckBox = True
        Me.dtpCardDate.Size = New System.Drawing.Size(127, 20)
        Me.dtpCardDate.TabIndex = 17
        Me.dtpCardDate.Tag = "CARDDATE"
        Me.dtpCardDate.Value = New Date(2008, 4, 24, 0, 0, 0, 0)
        '
        'lbCardDate
        '
        Me.lbCardDate.AutoSize = True
        Me.lbCardDate.Location = New System.Drawing.Point(224, 96)
        Me.lbCardDate.Name = "lbCardDate"
        Me.lbCardDate.Size = New System.Drawing.Size(60, 13)
        Me.lbCardDate.TabIndex = 22
        Me.lbCardDate.Tag = "lbCardDate"
        Me.lbCardDate.Text = "lbCardDate"
        '
        'txtCardNo
        '
        Me.txtCardNo.Location = New System.Drawing.Point(90, 93)
        Me.txtCardNo.Name = "txtCardNo"
        Me.txtCardNo.Size = New System.Drawing.Size(128, 20)
        Me.txtCardNo.TabIndex = 15
        Me.txtCardNo.Tag = "CARDNO"
        '
        'lbCardNo
        '
        Me.lbCardNo.AutoSize = True
        Me.lbCardNo.Location = New System.Drawing.Point(6, 96)
        Me.lbCardNo.Name = "lbCardNo"
        Me.lbCardNo.Size = New System.Drawing.Size(51, 13)
        Me.lbCardNo.TabIndex = 20
        Me.lbCardNo.Tag = "lbCardNo"
        Me.lbCardNo.Text = "lbCardNo"
        '
        'cboCardType
        '
        Me.cboCardType.DisplayMember = "DISPLAY"
        Me.cboCardType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboCardType.FormattingEnabled = True
        Me.cboCardType.Location = New System.Drawing.Point(537, 65)
        Me.cboCardType.Name = "cboCardType"
        Me.cboCardType.Size = New System.Drawing.Size(125, 21)
        Me.cboCardType.TabIndex = 13
        Me.cboCardType.Tag = "CARDTYPE"
        Me.cboCardType.ValueMember = "VALUE"
        '
        'lbCardType
        '
        Me.lbCardType.AutoSize = True
        Me.lbCardType.Location = New System.Drawing.Point(451, 67)
        Me.lbCardType.Name = "lbCardType"
        Me.lbCardType.Size = New System.Drawing.Size(61, 13)
        Me.lbCardType.TabIndex = 18
        Me.lbCardType.Tag = "lbCardType"
        Me.lbCardType.Text = "lbCardType"
        '
        'txtBirthPlace
        '
        Me.txtBirthPlace.Location = New System.Drawing.Point(317, 67)
        Me.txtBirthPlace.Name = "txtBirthPlace"
        Me.txtBirthPlace.Size = New System.Drawing.Size(127, 20)
        Me.txtBirthPlace.TabIndex = 11
        Me.txtBirthPlace.Tag = "BIRTH_PLACE"
        '
        'lbBirthPlace
        '
        Me.lbBirthPlace.AutoSize = True
        Me.lbBirthPlace.Location = New System.Drawing.Point(224, 68)
        Me.lbBirthPlace.Name = "lbBirthPlace"
        Me.lbBirthPlace.Size = New System.Drawing.Size(63, 13)
        Me.lbBirthPlace.TabIndex = 16
        Me.lbBirthPlace.Tag = "lbBirthPlace"
        Me.lbBirthPlace.Text = "lbBirthPlace"
        '
        'lbBirthDay
        '
        Me.lbBirthDay.AutoSize = True
        Me.lbBirthDay.Location = New System.Drawing.Point(6, 65)
        Me.lbBirthDay.Name = "lbBirthDay"
        Me.lbBirthDay.Size = New System.Drawing.Size(55, 13)
        Me.lbBirthDay.TabIndex = 14
        Me.lbBirthDay.Tag = "lbBirthDay"
        Me.lbBirthDay.Text = "lbBirthDay"
        '
        'txtFullName
        '
        Me.txtFullName.Location = New System.Drawing.Point(90, 12)
        Me.txtFullName.Name = "txtFullName"
        Me.txtFullName.Size = New System.Drawing.Size(354, 20)
        Me.txtFullName.TabIndex = 1
        Me.txtFullName.Tag = "FULL_NAME"
        '
        'lbFullName
        '
        Me.lbFullName.AutoSize = True
        Me.lbFullName.Location = New System.Drawing.Point(6, 15)
        Me.lbFullName.Name = "lbFullName"
        Me.lbFullName.Size = New System.Drawing.Size(59, 13)
        Me.lbFullName.TabIndex = 10
        Me.lbFullName.Tag = "lbFullName"
        Me.lbFullName.Text = "lbFullName"
        '
        'cboType
        '
        Me.cboType.DisplayMember = "DISPLAY"
        Me.cboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboType.FormattingEnabled = True
        Me.cboType.Location = New System.Drawing.Point(90, 38)
        Me.cboType.Name = "cboType"
        Me.cboType.Size = New System.Drawing.Size(128, 21)
        Me.cboType.TabIndex = 5
        Me.cboType.Tag = "TYPE"
        Me.cboType.ValueMember = "VALUE"
        '
        'lbType
        '
        Me.lbType.AutoSize = True
        Me.lbType.Location = New System.Drawing.Point(6, 41)
        Me.lbType.Name = "lbType"
        Me.lbType.Size = New System.Drawing.Size(39, 13)
        Me.lbType.TabIndex = 2
        Me.lbType.Tag = "lbType"
        Me.lbType.Text = "lbType"
        '
        'tpOrganization
        '
        Me.tpOrganization.Controls.Add(Me.txtOrgForeignNo)
        Me.tpOrganization.Controls.Add(Me.lbOrgForeignNo)
        Me.tpOrganization.Controls.Add(Me.lstOrgRepPerson)
        Me.tpOrganization.Controls.Add(Me.dtpFoundationDate)
        Me.tpOrganization.Controls.Add(Me.lbFoundationDate)
        Me.tpOrganization.Controls.Add(Me.txtFoudationNo)
        Me.tpOrganization.Controls.Add(Me.lbFoundationNo)
        Me.tpOrganization.Controls.Add(Me.lbOrgDesc)
        Me.tpOrganization.Controls.Add(Me.txtOrgDesc)
        Me.tpOrganization.Controls.Add(Me.dtpOrgCardDate)
        Me.tpOrganization.Controls.Add(Me.lbShortName)
        Me.tpOrganization.Controls.Add(Me.txtOrgShortName)
        Me.tpOrganization.Controls.Add(Me.txtOrgTransName)
        Me.tpOrganization.Controls.Add(Me.lbRGIIRep)
        Me.tpOrganization.Controls.Add(Me.txtFoundationIssue)
        Me.tpOrganization.Controls.Add(Me.lbFoundationIssue)
        Me.tpOrganization.Controls.Add(Me.lbOrgCardDate)
        Me.tpOrganization.Controls.Add(Me.txtOrgCardNo)
        Me.tpOrganization.Controls.Add(Me.lbOrgCardNo)
        Me.tpOrganization.Controls.Add(Me.txtOrgFieldBusiness)
        Me.tpOrganization.Controls.Add(Me.lbFieldBusiness)
        Me.tpOrganization.Controls.Add(Me.txtOrgFax)
        Me.tpOrganization.Controls.Add(Me.lbOrgFax)
        Me.tpOrganization.Controls.Add(Me.txtOrgPhone)
        Me.tpOrganization.Controls.Add(Me.lbOrgPhone)
        Me.tpOrganization.Controls.Add(Me.txtOrgAddress)
        Me.tpOrganization.Controls.Add(Me.lbOrgAddress)
        Me.tpOrganization.Controls.Add(Me.lbTransName)
        Me.tpOrganization.Controls.Add(Me.txtOrgFullName)
        Me.tpOrganization.Controls.Add(Me.lbOrgName)
        Me.tpOrganization.Controls.Add(Me.cboOrgType)
        Me.tpOrganization.Controls.Add(Me.lbType2)
        Me.tpOrganization.Location = New System.Drawing.Point(4, 22)
        Me.tpOrganization.Name = "tpOrganization"
        Me.tpOrganization.Padding = New System.Windows.Forms.Padding(3)
        Me.tpOrganization.Size = New System.Drawing.Size(674, 278)
        Me.tpOrganization.TabIndex = 1
        Me.tpOrganization.Tag = "tpOrganization"
        Me.tpOrganization.Text = "tpOrganization"
        Me.tpOrganization.UseVisualStyleBackColor = True
        '
        'txtOrgForeignNo
        '
        Me.txtOrgForeignNo.Location = New System.Drawing.Point(508, 121)
        Me.txtOrgForeignNo.Name = "txtOrgForeignNo"
        Me.txtOrgForeignNo.Size = New System.Drawing.Size(160, 20)
        Me.txtOrgForeignNo.TabIndex = 86
        Me.txtOrgForeignNo.Tag = "FOREIGNNO"
        '
        'lbOrgForeignNo
        '
        Me.lbOrgForeignNo.AutoSize = True
        Me.lbOrgForeignNo.Location = New System.Drawing.Point(430, 127)
        Me.lbOrgForeignNo.Name = "lbOrgForeignNo"
        Me.lbOrgForeignNo.Size = New System.Drawing.Size(81, 13)
        Me.lbOrgForeignNo.TabIndex = 110
        Me.lbOrgForeignNo.Tag = "lbOrgForeignNo"
        Me.lbOrgForeignNo.Text = "lbOrgForeignNo"
        '
        'lstOrgRepPerson
        '
        Me.lstOrgRepPerson.FormattingEnabled = True
        Me.lstOrgRepPerson.HorizontalScrollbar = True
        Me.lstOrgRepPerson.Location = New System.Drawing.Point(99, 174)
        Me.lstOrgRepPerson.Name = "lstOrgRepPerson"
        Me.lstOrgRepPerson.Size = New System.Drawing.Size(323, 69)
        Me.lstOrgRepPerson.TabIndex = 109
        Me.lstOrgRepPerson.Tag = "REPNO"
        '
        'dtpFoundationDate
        '
        Me.dtpFoundationDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpFoundationDate.Location = New System.Drawing.Point(298, 148)
        Me.dtpFoundationDate.Name = "dtpFoundationDate"
        Me.dtpFoundationDate.ShowCheckBox = True
        Me.dtpFoundationDate.Size = New System.Drawing.Size(124, 20)
        Me.dtpFoundationDate.TabIndex = 89
        Me.dtpFoundationDate.Tag = "BIRTH_DATE"
        '
        'lbFoundationDate
        '
        Me.lbFoundationDate.AutoSize = True
        Me.lbFoundationDate.Location = New System.Drawing.Point(214, 151)
        Me.lbFoundationDate.Name = "lbFoundationDate"
        Me.lbFoundationDate.Size = New System.Drawing.Size(91, 13)
        Me.lbFoundationDate.TabIndex = 107
        Me.lbFoundationDate.Tag = "lbFoundationDate"
        Me.lbFoundationDate.Text = "lbFoundationDate"
        '
        'txtFoudationNo
        '
        Me.txtFoudationNo.Location = New System.Drawing.Point(99, 148)
        Me.txtFoudationNo.Name = "txtFoudationNo"
        Me.txtFoudationNo.Size = New System.Drawing.Size(109, 20)
        Me.txtFoudationNo.TabIndex = 87
        Me.txtFoudationNo.Tag = "MOBILEPHONE"
        '
        'lbFoundationNo
        '
        Me.lbFoundationNo.AutoSize = True
        Me.lbFoundationNo.Location = New System.Drawing.Point(6, 151)
        Me.lbFoundationNo.Name = "lbFoundationNo"
        Me.lbFoundationNo.Size = New System.Drawing.Size(82, 13)
        Me.lbFoundationNo.TabIndex = 105
        Me.lbFoundationNo.Tag = "lbFoundationNo"
        Me.lbFoundationNo.Text = "lbFoundationNo"
        '
        'lbOrgDesc
        '
        Me.lbOrgDesc.AutoSize = True
        Me.lbOrgDesc.Location = New System.Drawing.Point(9, 252)
        Me.lbOrgDesc.Name = "lbOrgDesc"
        Me.lbOrgDesc.Size = New System.Drawing.Size(40, 13)
        Me.lbOrgDesc.TabIndex = 101
        Me.lbOrgDesc.Tag = "lbDesc"
        Me.lbOrgDesc.Text = "lbDesc"
        '
        'txtOrgDesc
        '
        Me.txtOrgDesc.Location = New System.Drawing.Point(99, 249)
        Me.txtOrgDesc.Name = "txtOrgDesc"
        Me.txtOrgDesc.Size = New System.Drawing.Size(613, 20)
        Me.txtOrgDesc.TabIndex = 95
        Me.txtOrgDesc.Tag = "NOTE"
        '
        'dtpOrgCardDate
        '
        Me.dtpOrgCardDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpOrgCardDate.Location = New System.Drawing.Point(298, 121)
        Me.dtpOrgCardDate.Name = "dtpOrgCardDate"
        Me.dtpOrgCardDate.ShowCheckBox = True
        Me.dtpOrgCardDate.Size = New System.Drawing.Size(124, 20)
        Me.dtpOrgCardDate.TabIndex = 85
        Me.dtpOrgCardDate.Tag = "CARDDATE"
        '
        'lbShortName
        '
        Me.lbShortName.AutoSize = True
        Me.lbShortName.Location = New System.Drawing.Point(214, 45)
        Me.lbShortName.Name = "lbShortName"
        Me.lbShortName.Size = New System.Drawing.Size(68, 13)
        Me.lbShortName.TabIndex = 97
        Me.lbShortName.Tag = "lbShortName"
        Me.lbShortName.Text = "lbShortName"
        '
        'txtOrgShortName
        '
        Me.txtOrgShortName.Location = New System.Drawing.Point(298, 40)
        Me.txtOrgShortName.Name = "txtOrgShortName"
        Me.txtOrgShortName.Size = New System.Drawing.Size(124, 20)
        Me.txtOrgShortName.TabIndex = 69
        Me.txtOrgShortName.Tag = "LAST_NAME"
        '
        'txtOrgTransName
        '
        Me.txtOrgTransName.Location = New System.Drawing.Point(99, 40)
        Me.txtOrgTransName.Name = "txtOrgTransName"
        Me.txtOrgTransName.Size = New System.Drawing.Size(109, 20)
        Me.txtOrgTransName.TabIndex = 67
        Me.txtOrgTransName.Tag = "FIRST_NAME"
        '
        'lbRGIIRep
        '
        Me.lbRGIIRep.AutoSize = True
        Me.lbRGIIRep.Location = New System.Drawing.Point(6, 174)
        Me.lbRGIIRep.Name = "lbRGIIRep"
        Me.lbRGIIRep.Size = New System.Drawing.Size(57, 13)
        Me.lbRGIIRep.TabIndex = 93
        Me.lbRGIIRep.Tag = "lbRGIIRep"
        Me.lbRGIIRep.Text = "lbRGIIRep"
        '
        'txtFoundationIssue
        '
        Me.txtFoundationIssue.Location = New System.Drawing.Point(508, 147)
        Me.txtFoundationIssue.Name = "txtFoundationIssue"
        Me.txtFoundationIssue.Size = New System.Drawing.Size(160, 20)
        Me.txtFoundationIssue.TabIndex = 91
        Me.txtFoundationIssue.Tag = "BIRTH_PLACE"
        '
        'lbFoundationIssue
        '
        Me.lbFoundationIssue.AutoSize = True
        Me.lbFoundationIssue.Location = New System.Drawing.Point(428, 151)
        Me.lbFoundationIssue.Name = "lbFoundationIssue"
        Me.lbFoundationIssue.Size = New System.Drawing.Size(93, 13)
        Me.lbFoundationIssue.TabIndex = 85
        Me.lbFoundationIssue.Tag = "lbFoundationIssue"
        Me.lbFoundationIssue.Text = "lbFoundationIssue"
        '
        'lbOrgCardDate
        '
        Me.lbOrgCardDate.AutoSize = True
        Me.lbOrgCardDate.Location = New System.Drawing.Point(214, 125)
        Me.lbOrgCardDate.Name = "lbOrgCardDate"
        Me.lbOrgCardDate.Size = New System.Drawing.Size(77, 13)
        Me.lbOrgCardDate.TabIndex = 83
        Me.lbOrgCardDate.Tag = "lbOrgCardDate"
        Me.lbOrgCardDate.Text = "lbOrgCardDate"
        '
        'txtOrgCardNo
        '
        Me.txtOrgCardNo.Location = New System.Drawing.Point(99, 122)
        Me.txtOrgCardNo.Name = "txtOrgCardNo"
        Me.txtOrgCardNo.Size = New System.Drawing.Size(109, 20)
        Me.txtOrgCardNo.TabIndex = 83
        Me.txtOrgCardNo.Tag = "CARDNO"
        '
        'lbOrgCardNo
        '
        Me.lbOrgCardNo.AutoSize = True
        Me.lbOrgCardNo.Location = New System.Drawing.Point(7, 123)
        Me.lbOrgCardNo.Name = "lbOrgCardNo"
        Me.lbOrgCardNo.Size = New System.Drawing.Size(68, 13)
        Me.lbOrgCardNo.TabIndex = 81
        Me.lbOrgCardNo.Tag = "lbOrgCardNo"
        Me.lbOrgCardNo.Text = "lbOrgCardNo"
        '
        'txtOrgFieldBusiness
        '
        Me.txtOrgFieldBusiness.Location = New System.Drawing.Point(99, 95)
        Me.txtOrgFieldBusiness.Name = "txtOrgFieldBusiness"
        Me.txtOrgFieldBusiness.Size = New System.Drawing.Size(183, 20)
        Me.txtOrgFieldBusiness.TabIndex = 77
        Me.txtOrgFieldBusiness.Tag = "OCCUPATION"
        '
        'lbFieldBusiness
        '
        Me.lbFieldBusiness.AutoSize = True
        Me.lbFieldBusiness.Location = New System.Drawing.Point(7, 97)
        Me.lbFieldBusiness.Name = "lbFieldBusiness"
        Me.lbFieldBusiness.Size = New System.Drawing.Size(79, 13)
        Me.lbFieldBusiness.TabIndex = 79
        Me.lbFieldBusiness.Tag = "lbFieldBusiness"
        Me.lbFieldBusiness.Text = "lbFieldBusiness"
        '
        'txtOrgFax
        '
        Me.txtOrgFax.Location = New System.Drawing.Point(573, 96)
        Me.txtOrgFax.Name = "txtOrgFax"
        Me.txtOrgFax.Size = New System.Drawing.Size(95, 20)
        Me.txtOrgFax.TabIndex = 81
        Me.txtOrgFax.Tag = "FAX"
        '
        'lbOrgFax
        '
        Me.lbOrgFax.AutoSize = True
        Me.lbOrgFax.Location = New System.Drawing.Point(505, 100)
        Me.lbOrgFax.Name = "lbOrgFax"
        Me.lbOrgFax.Size = New System.Drawing.Size(49, 13)
        Me.lbOrgFax.TabIndex = 77
        Me.lbOrgFax.Tag = "lbFax"
        Me.lbOrgFax.Text = "lbOrgFax"
        '
        'txtOrgPhone
        '
        Me.txtOrgPhone.Location = New System.Drawing.Point(368, 96)
        Me.txtOrgPhone.Name = "txtOrgPhone"
        Me.txtOrgPhone.Size = New System.Drawing.Size(99, 20)
        Me.txtOrgPhone.TabIndex = 79
        Me.txtOrgPhone.Tag = "PHONE"
        '
        'lbOrgPhone
        '
        Me.lbOrgPhone.AutoSize = True
        Me.lbOrgPhone.Location = New System.Drawing.Point(295, 100)
        Me.lbOrgPhone.Name = "lbOrgPhone"
        Me.lbOrgPhone.Size = New System.Drawing.Size(63, 13)
        Me.lbOrgPhone.TabIndex = 75
        Me.lbOrgPhone.Tag = "lbPhone"
        Me.lbOrgPhone.Text = "lbOrgPhone"
        '
        'txtOrgAddress
        '
        Me.txtOrgAddress.Location = New System.Drawing.Point(99, 68)
        Me.txtOrgAddress.Name = "txtOrgAddress"
        Me.txtOrgAddress.Size = New System.Drawing.Size(323, 20)
        Me.txtOrgAddress.TabIndex = 73
        Me.txtOrgAddress.Tag = "ADDRESS"
        '
        'lbOrgAddress
        '
        Me.lbOrgAddress.AutoSize = True
        Me.lbOrgAddress.Location = New System.Drawing.Point(7, 70)
        Me.lbOrgAddress.Name = "lbOrgAddress"
        Me.lbOrgAddress.Size = New System.Drawing.Size(70, 13)
        Me.lbOrgAddress.TabIndex = 73
        Me.lbOrgAddress.Tag = "lbOrgAddress"
        Me.lbOrgAddress.Text = "lbOrgAddress"
        '
        'lbTransName
        '
        Me.lbTransName.AutoSize = True
        Me.lbTransName.Location = New System.Drawing.Point(7, 42)
        Me.lbTransName.Name = "lbTransName"
        Me.lbTransName.Size = New System.Drawing.Size(70, 13)
        Me.lbTransName.TabIndex = 61
        Me.lbTransName.Tag = "lbTransName"
        Me.lbTransName.Text = "lbTransName"
        '
        'txtOrgFullName
        '
        Me.txtOrgFullName.Location = New System.Drawing.Point(99, 14)
        Me.txtOrgFullName.Name = "txtOrgFullName"
        Me.txtOrgFullName.Size = New System.Drawing.Size(323, 20)
        Me.txtOrgFullName.TabIndex = 63
        Me.txtOrgFullName.Tag = "FULL_NAME"
        '
        'lbOrgName
        '
        Me.lbOrgName.AutoSize = True
        Me.lbOrgName.Location = New System.Drawing.Point(6, 17)
        Me.lbOrgName.Name = "lbOrgName"
        Me.lbOrgName.Size = New System.Drawing.Size(60, 13)
        Me.lbOrgName.TabIndex = 57
        Me.lbOrgName.Tag = "lbOrgName"
        Me.lbOrgName.Text = "lbOrgName"
        '
        'cboOrgType
        '
        Me.cboOrgType.DisplayMember = "DISPLAY"
        Me.cboOrgType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboOrgType.FormattingEnabled = True
        Me.cboOrgType.Location = New System.Drawing.Point(508, 12)
        Me.cboOrgType.Name = "cboOrgType"
        Me.cboOrgType.Size = New System.Drawing.Size(160, 21)
        Me.cboOrgType.TabIndex = 65
        Me.cboOrgType.Tag = "TYPE"
        Me.cboOrgType.ValueMember = "VALUE"
        '
        'lbType2
        '
        Me.lbType2.AutoSize = True
        Me.lbType2.Location = New System.Drawing.Point(428, 17)
        Me.lbType2.Name = "lbType2"
        Me.lbType2.Size = New System.Drawing.Size(39, 13)
        Me.lbType2.TabIndex = 49
        Me.lbType2.Tag = "lbType"
        Me.lbType2.Text = "lbType"
        '
        'frmRGII
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.ClientSize = New System.Drawing.Size(706, 389)
        Me.Controls.Add(Me.tctRGII)
        Me.Name = "frmRGII"
        Me.Controls.SetChildIndex(Me.btnOk, 0)
        Me.Controls.SetChildIndex(Me.btnCancel, 0)
        Me.Controls.SetChildIndex(Me.btnApply, 0)
        Me.Controls.SetChildIndex(Me.tctRGII, 0)
        Me.Controls.SetChildIndex(Me.Panel1, 0)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.tctRGII.ResumeLayout(False)
        Me.tpPersonal.ResumeLayout(False)
        Me.tpPersonal.PerformLayout()
        Me.tpOrganization.ResumeLayout(False)
        Me.tpOrganization.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tctRGII As System.Windows.Forms.TabControl
    Friend WithEvents tpOrganization As System.Windows.Forms.TabPage
    Friend WithEvents lbType As System.Windows.Forms.Label
    Friend WithEvents cboType As Sats.AppCore.ComboBoxEx
    Friend WithEvents txtFullName As System.Windows.Forms.TextBox
    Friend WithEvents lbFullName As System.Windows.Forms.Label
    Friend WithEvents lbBirthPlace As System.Windows.Forms.Label
    Friend WithEvents lbBirthDay As System.Windows.Forms.Label
    Friend WithEvents lbOriginNationaly As System.Windows.Forms.Label
    Friend WithEvents txtCardIssue As System.Windows.Forms.TextBox
    Friend WithEvents lbCardIssue As System.Windows.Forms.Label
    Friend WithEvents dtpCardDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents lbCardDate As System.Windows.Forms.Label
    Friend WithEvents txtCardNo As System.Windows.Forms.TextBox
    Friend WithEvents lbCardNo As System.Windows.Forms.Label
    Friend WithEvents cboCardType As Sats.AppCore.ComboBoxEx
    Friend WithEvents lbCardType As System.Windows.Forms.Label
    Friend WithEvents txtBirthPlace As System.Windows.Forms.TextBox
    Friend WithEvents txtNation As System.Windows.Forms.TextBox
    Friend WithEvents lbNation As System.Windows.Forms.Label
    Friend WithEvents lbCurrentNationaly As System.Windows.Forms.Label
    Friend WithEvents lbEduLevel As System.Windows.Forms.Label
    Friend WithEvents txtAddress As System.Windows.Forms.TextBox
    Friend WithEvents lbAddress As System.Windows.Forms.Label
    Friend WithEvents txtEmail As System.Windows.Forms.TextBox
    Friend WithEvents lbEmail As System.Windows.Forms.Label
    Friend WithEvents txtFax As System.Windows.Forms.TextBox
    Friend WithEvents lbFax As System.Windows.Forms.Label
    Friend WithEvents txtPhone As System.Windows.Forms.TextBox
    Friend WithEvents lbPhone As System.Windows.Forms.Label
    Friend WithEvents txtPost As System.Windows.Forms.TextBox
    Friend WithEvents lbFunction As System.Windows.Forms.Label
    Friend WithEvents txtOccupation As System.Windows.Forms.TextBox
    Friend WithEvents lbCareer As System.Windows.Forms.Label
    Friend WithEvents txtEducationLevel As System.Windows.Forms.TextBox
    Friend WithEvents txtNote As System.Windows.Forms.TextBox
    Friend WithEvents lbDesc As System.Windows.Forms.Label
    Friend WithEvents lbRGIIRep As System.Windows.Forms.Label
    Friend WithEvents txtFoundationIssue As System.Windows.Forms.TextBox
    Friend WithEvents lbFoundationIssue As System.Windows.Forms.Label
    Friend WithEvents lbOrgCardDate As System.Windows.Forms.Label
    Friend WithEvents txtOrgCardNo As System.Windows.Forms.TextBox
    Friend WithEvents lbOrgCardNo As System.Windows.Forms.Label
    Friend WithEvents txtOrgFieldBusiness As System.Windows.Forms.TextBox
    Friend WithEvents lbFieldBusiness As System.Windows.Forms.Label
    Friend WithEvents txtOrgFax As System.Windows.Forms.TextBox
    Friend WithEvents lbOrgFax As System.Windows.Forms.Label
    Friend WithEvents txtOrgPhone As System.Windows.Forms.TextBox
    Friend WithEvents lbOrgPhone As System.Windows.Forms.Label
    Friend WithEvents txtOrgAddress As System.Windows.Forms.TextBox
    Friend WithEvents lbOrgAddress As System.Windows.Forms.Label
    Friend WithEvents lbTransName As System.Windows.Forms.Label
    Friend WithEvents txtOrgFullName As System.Windows.Forms.TextBox
    Friend WithEvents lbOrgName As System.Windows.Forms.Label
    Friend WithEvents cboOrgType As Sats.AppCore.ComboBoxEx
    Friend WithEvents lbType2 As System.Windows.Forms.Label
    Friend WithEvents cboSex As Sats.AppCore.ComboBoxEx
    Friend WithEvents lbSex As System.Windows.Forms.Label
    Friend WithEvents lbShortName As System.Windows.Forms.Label
    Friend WithEvents txtOrgShortName As System.Windows.Forms.TextBox
    Friend WithEvents txtOrgTransName As System.Windows.Forms.TextBox
    Friend WithEvents dtpOrgCardDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents lbOrgDesc As System.Windows.Forms.Label
    Friend WithEvents txtOrgDesc As System.Windows.Forms.TextBox
    Friend WithEvents txtMobiphone As System.Windows.Forms.TextBox
    Friend WithEvents lbMobiphone As System.Windows.Forms.Label
    Friend WithEvents dtpBirthDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtpFoundationDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents lbFoundationDate As System.Windows.Forms.Label
    Friend WithEvents txtFoudationNo As System.Windows.Forms.TextBox
    Friend WithEvents lbFoundationNo As System.Windows.Forms.Label
    Friend WithEvents tpPersonal As System.Windows.Forms.TabPage
    Friend WithEvents lstOrgRepPerson As System.Windows.Forms.ListBox
    Friend WithEvents cboCurrentNationaly As Sats.AppCore.ComboBoxEx
    Friend WithEvents cboOriginalNationaly As Sats.AppCore.ComboBoxEx
    Friend WithEvents txtForeignNo As System.Windows.Forms.TextBox
    Friend WithEvents lbForeignNo As System.Windows.Forms.Label
    Friend WithEvents txtOrgForeignNo As System.Windows.Forms.TextBox
    Friend WithEvents lbOrgForeignNo As System.Windows.Forms.Label

End Class
