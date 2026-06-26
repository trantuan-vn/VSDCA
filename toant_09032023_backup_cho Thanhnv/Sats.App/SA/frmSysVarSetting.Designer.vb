<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSysVarSetting
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSysVarSetting))
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.lbCaption = New System.Windows.Forms.Label
        Me.tcTabControl = New System.Windows.Forms.TabControl
        Me.tpSA = New System.Windows.Forms.TabPage
        Me.grpSA = New System.Windows.Forms.GroupBox
        Me.btnInitAll = New System.Windows.Forms.Button
        Me.btnAddOne = New System.Windows.Forms.Button
        Me.btnRemoveOne = New System.Windows.Forms.Button
        Me.btnRemoveAll = New System.Windows.Forms.Button
        Me.lstListDate = New System.Windows.Forms.ListBox
        Me.lbListDate = New System.Windows.Forms.Label
        Me.lbChooseDate = New System.Windows.Forms.Label
        Me.mcdCalendar = New System.Windows.Forms.MonthCalendar
        Me.cboBranchId = New Sats.AppCore.ComboBoxEx
        Me.lbBranchId = New System.Windows.Forms.Label
        Me.tpSYSTEM = New System.Windows.Forms.TabPage
        Me.grbSYSTEM = New System.Windows.Forms.GroupBox
        Me.lbFTPServerAddress = New System.Windows.Forms.Label
        Me.txtSearchResultPath = New System.Windows.Forms.TextBox
        Me.lbFTPPassword = New System.Windows.Forms.Label
        Me.txtFTPUserName = New System.Windows.Forms.TextBox
        Me.lbFTPServerPort = New System.Windows.Forms.Label
        Me.lbFTPUserName = New System.Windows.Forms.Label
        Me.lbSearchResultPath = New System.Windows.Forms.Label
        Me.txtFTPPassword = New System.Windows.Forms.TextBox
        Me.txtFTPServerPort = New System.Windows.Forms.TextBox
        Me.txtFTPServerAddress = New System.Windows.Forms.TextBox
        Me.tpSF = New System.Windows.Forms.TabPage
        Me.grbSF = New System.Windows.Forms.GroupBox
        Me.txtInterestRateLevel1 = New System.Windows.Forms.TextBox
        Me.lbMaxAnnualFund = New System.Windows.Forms.Label
        Me.lbInterestRateLevel2 = New System.Windows.Forms.Label
        Me.lbInterestRateLevel1 = New System.Windows.Forms.Label
        Me.txtInterestRateLevel2 = New System.Windows.Forms.TextBox
        Me.txtMaxAnnualFund = New System.Windows.Forms.TextBox
        Me.txtAnnualFundRate = New System.Windows.Forms.TextBox
        Me.lbDateLimitLevel2 = New System.Windows.Forms.Label
        Me.txtDateLimitLevel2 = New System.Windows.Forms.TextBox
        Me.lbAnnualFundRate = New System.Windows.Forms.Label
        Me.tpRA = New System.Windows.Forms.TabPage
        Me.btnOk = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.lbMaxRowSearch = New System.Windows.Forms.Label
        Me.txtMaxRowSearch = New System.Windows.Forms.TextBox
        Me.Panel1.SuspendLayout()
        Me.tcTabControl.SuspendLayout()
        Me.tpSA.SuspendLayout()
        Me.grpSA.SuspendLayout()
        Me.tpSYSTEM.SuspendLayout()
        Me.grbSYSTEM.SuspendLayout()
        Me.tpSF.SuspendLayout()
        Me.grbSF.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.SystemColors.Desktop
        Me.Panel1.Controls.Add(Me.lbCaption)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(495, 56)
        Me.Panel1.TabIndex = 5
        '
        'lbCaption
        '
        Me.lbCaption.AutoSize = True
        Me.lbCaption.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbCaption.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lbCaption.Location = New System.Drawing.Point(23, 20)
        Me.lbCaption.Name = "lbCaption"
        Me.lbCaption.Size = New System.Drawing.Size(60, 13)
        Me.lbCaption.TabIndex = 0
        Me.lbCaption.Tag = "lbCaption"
        Me.lbCaption.Text = "lbCaption"
        '
        'tcTabControl
        '
        Me.tcTabControl.Controls.Add(Me.tpSA)
        Me.tcTabControl.Controls.Add(Me.tpSYSTEM)
        Me.tcTabControl.Controls.Add(Me.tpSF)
        Me.tcTabControl.Controls.Add(Me.tpRA)
        Me.tcTabControl.Location = New System.Drawing.Point(0, 62)
        Me.tcTabControl.Name = "tcTabControl"
        Me.tcTabControl.SelectedIndex = 0
        Me.tcTabControl.Size = New System.Drawing.Size(489, 351)
        Me.tcTabControl.TabIndex = 6
        Me.tcTabControl.Tag = "tcTabControl"
        '
        'tpSA
        '
        Me.tpSA.Controls.Add(Me.grpSA)
        Me.tpSA.Location = New System.Drawing.Point(4, 22)
        Me.tpSA.Name = "tpSA"
        Me.tpSA.Padding = New System.Windows.Forms.Padding(3)
        Me.tpSA.Size = New System.Drawing.Size(481, 325)
        Me.tpSA.TabIndex = 0
        Me.tpSA.Tag = "tpSA"
        Me.tpSA.Text = "tpSA"
        Me.tpSA.UseVisualStyleBackColor = True
        '
        'grpSA
        '
        Me.grpSA.Controls.Add(Me.btnInitAll)
        Me.grpSA.Controls.Add(Me.btnAddOne)
        Me.grpSA.Controls.Add(Me.btnRemoveOne)
        Me.grpSA.Controls.Add(Me.btnRemoveAll)
        Me.grpSA.Controls.Add(Me.lstListDate)
        Me.grpSA.Controls.Add(Me.lbListDate)
        Me.grpSA.Controls.Add(Me.lbChooseDate)
        Me.grpSA.Controls.Add(Me.mcdCalendar)
        Me.grpSA.Controls.Add(Me.cboBranchId)
        Me.grpSA.Controls.Add(Me.lbBranchId)
        Me.grpSA.Location = New System.Drawing.Point(8, 6)
        Me.grpSA.Name = "grpSA"
        Me.grpSA.Size = New System.Drawing.Size(464, 313)
        Me.grpSA.TabIndex = 0
        Me.grpSA.TabStop = False
        Me.grpSA.Tag = "grpSA"
        Me.grpSA.Text = "grpSA"
        '
        'btnInitAll
        '
        Me.btnInitAll.Location = New System.Drawing.Point(362, 17)
        Me.btnInitAll.Name = "btnInitAll"
        Me.btnInitAll.Size = New System.Drawing.Size(84, 21)
        Me.btnInitAll.TabIndex = 10
        Me.btnInitAll.Tag = "btnInitAll"
        Me.btnInitAll.Text = "btnInitAll"
        Me.btnInitAll.UseVisualStyleBackColor = True
        '
        'btnAddOne
        '
        Me.btnAddOne.Font = New System.Drawing.Font("Webdings", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.btnAddOne.Location = New System.Drawing.Point(232, 183)
        Me.btnAddOne.Name = "btnAddOne"
        Me.btnAddOne.Size = New System.Drawing.Size(36, 34)
        Me.btnAddOne.TabIndex = 9
        Me.btnAddOne.Tag = "btnAddOne"
        Me.btnAddOne.Text = "4"
        Me.btnAddOne.UseVisualStyleBackColor = True
        '
        'btnRemoveOne
        '
        Me.btnRemoveOne.Font = New System.Drawing.Font("Webdings", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.btnRemoveOne.Location = New System.Drawing.Point(232, 143)
        Me.btnRemoveOne.Name = "btnRemoveOne"
        Me.btnRemoveOne.Size = New System.Drawing.Size(36, 34)
        Me.btnRemoveOne.TabIndex = 8
        Me.btnRemoveOne.Tag = "btnRemoveOne"
        Me.btnRemoveOne.Text = "3"
        Me.btnRemoveOne.UseVisualStyleBackColor = True
        '
        'btnRemoveAll
        '
        Me.btnRemoveAll.Font = New System.Drawing.Font("Webdings", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.btnRemoveAll.Location = New System.Drawing.Point(232, 103)
        Me.btnRemoveAll.Name = "btnRemoveAll"
        Me.btnRemoveAll.Size = New System.Drawing.Size(36, 34)
        Me.btnRemoveAll.TabIndex = 7
        Me.btnRemoveAll.Tag = "btnRemoveAll"
        Me.btnRemoveAll.Text = "7"
        Me.btnRemoveAll.UseVisualStyleBackColor = True
        '
        'lstListDate
        '
        Me.lstListDate.FormattingEnabled = True
        Me.lstListDate.Location = New System.Drawing.Point(287, 80)
        Me.lstListDate.Name = "lstListDate"
        Me.lstListDate.Size = New System.Drawing.Size(159, 225)
        Me.lstListDate.TabIndex = 6
        Me.lstListDate.Tag = "lstListDate"
        '
        'lbListDate
        '
        Me.lbListDate.AutoSize = True
        Me.lbListDate.Location = New System.Drawing.Point(284, 54)
        Me.lbListDate.Name = "lbListDate"
        Me.lbListDate.Size = New System.Drawing.Size(54, 13)
        Me.lbListDate.TabIndex = 5
        Me.lbListDate.Tag = "lbListDate"
        Me.lbListDate.Text = "lbListDate"
        '
        'lbChooseDate
        '
        Me.lbChooseDate.AutoSize = True
        Me.lbChooseDate.Location = New System.Drawing.Point(33, 54)
        Me.lbChooseDate.Name = "lbChooseDate"
        Me.lbChooseDate.Size = New System.Drawing.Size(74, 13)
        Me.lbChooseDate.TabIndex = 3
        Me.lbChooseDate.Tag = "lbChooseDate"
        Me.lbChooseDate.Text = "lbChooseDate"
        '
        'mcdCalendar
        '
        Me.mcdCalendar.Location = New System.Drawing.Point(36, 80)
        Me.mcdCalendar.Name = "mcdCalendar"
        Me.mcdCalendar.TabIndex = 2
        Me.mcdCalendar.Tag = "mcdCalendar"
        '
        'cboBranchId
        '
        Me.cboBranchId.DisplayMember = "DISPLAY"
        Me.cboBranchId.FormattingEnabled = True
        Me.cboBranchId.Location = New System.Drawing.Point(119, 17)
        Me.cboBranchId.Name = "cboBranchId"
        Me.cboBranchId.Size = New System.Drawing.Size(237, 21)
        Me.cboBranchId.TabIndex = 1
        Me.cboBranchId.Tag = "cboBranchId"
        Me.cboBranchId.ValueMember = "VALUE"
        '
        'lbBranchId
        '
        Me.lbBranchId.AutoSize = True
        Me.lbBranchId.Location = New System.Drawing.Point(33, 20)
        Me.lbBranchId.Name = "lbBranchId"
        Me.lbBranchId.Size = New System.Drawing.Size(58, 13)
        Me.lbBranchId.TabIndex = 0
        Me.lbBranchId.Tag = "lbBranchId"
        Me.lbBranchId.Text = "lbBranchId"
        '
        'tpSYSTEM
        '
        Me.tpSYSTEM.Controls.Add(Me.grbSYSTEM)
        Me.tpSYSTEM.Location = New System.Drawing.Point(4, 22)
        Me.tpSYSTEM.Name = "tpSYSTEM"
        Me.tpSYSTEM.Padding = New System.Windows.Forms.Padding(3)
        Me.tpSYSTEM.Size = New System.Drawing.Size(481, 325)
        Me.tpSYSTEM.TabIndex = 1
        Me.tpSYSTEM.Tag = "tpSYSTEM"
        Me.tpSYSTEM.Text = "tpSYSTEM"
        Me.tpSYSTEM.UseVisualStyleBackColor = True
        '
        'grbSYSTEM
        '
        Me.grbSYSTEM.Controls.Add(Me.txtMaxRowSearch)
        Me.grbSYSTEM.Controls.Add(Me.lbMaxRowSearch)
        Me.grbSYSTEM.Controls.Add(Me.lbFTPServerAddress)
        Me.grbSYSTEM.Controls.Add(Me.txtSearchResultPath)
        Me.grbSYSTEM.Controls.Add(Me.lbFTPPassword)
        Me.grbSYSTEM.Controls.Add(Me.txtFTPUserName)
        Me.grbSYSTEM.Controls.Add(Me.lbFTPServerPort)
        Me.grbSYSTEM.Controls.Add(Me.lbFTPUserName)
        Me.grbSYSTEM.Controls.Add(Me.lbSearchResultPath)
        Me.grbSYSTEM.Controls.Add(Me.txtFTPPassword)
        Me.grbSYSTEM.Controls.Add(Me.txtFTPServerPort)
        Me.grbSYSTEM.Controls.Add(Me.txtFTPServerAddress)
        Me.grbSYSTEM.Location = New System.Drawing.Point(8, 17)
        Me.grbSYSTEM.Name = "grbSYSTEM"
        Me.grbSYSTEM.Size = New System.Drawing.Size(467, 287)
        Me.grbSYSTEM.TabIndex = 20
        Me.grbSYSTEM.TabStop = False
        Me.grbSYSTEM.Tag = "grbSYSTEM"
        Me.grbSYSTEM.Text = "grbSYSTEM"
        '
        'lbFTPServerAddress
        '
        Me.lbFTPServerAddress.AutoSize = True
        Me.lbFTPServerAddress.Location = New System.Drawing.Point(22, 42)
        Me.lbFTPServerAddress.Name = "lbFTPServerAddress"
        Me.lbFTPServerAddress.Size = New System.Drawing.Size(104, 13)
        Me.lbFTPServerAddress.TabIndex = 10
        Me.lbFTPServerAddress.Tag = "lbFTPServerAddress"
        Me.lbFTPServerAddress.Text = "lbFTPServerAddress"
        '
        'txtSearchResultPath
        '
        Me.txtSearchResultPath.Location = New System.Drawing.Point(242, 152)
        Me.txtSearchResultPath.Name = "txtSearchResultPath"
        Me.txtSearchResultPath.Size = New System.Drawing.Size(179, 20)
        Me.txtSearchResultPath.TabIndex = 19
        Me.txtSearchResultPath.Tag = "SEARCH_RESULT_PATH"
        '
        'lbFTPPassword
        '
        Me.lbFTPPassword.AutoSize = True
        Me.lbFTPPassword.Location = New System.Drawing.Point(22, 128)
        Me.lbFTPPassword.Name = "lbFTPPassword"
        Me.lbFTPPassword.Size = New System.Drawing.Size(81, 13)
        Me.lbFTPPassword.TabIndex = 16
        Me.lbFTPPassword.Tag = "lbFTPPassword"
        Me.lbFTPPassword.Text = "lbFTPPassword"
        '
        'txtFTPUserName
        '
        Me.txtFTPUserName.Location = New System.Drawing.Point(242, 89)
        Me.txtFTPUserName.Name = "txtFTPUserName"
        Me.txtFTPUserName.Size = New System.Drawing.Size(179, 20)
        Me.txtFTPUserName.TabIndex = 15
        Me.txtFTPUserName.Tag = "FTP_USERNAME"
        '
        'lbFTPServerPort
        '
        Me.lbFTPServerPort.AutoSize = True
        Me.lbFTPServerPort.Location = New System.Drawing.Point(22, 69)
        Me.lbFTPServerPort.Name = "lbFTPServerPort"
        Me.lbFTPServerPort.Size = New System.Drawing.Size(85, 13)
        Me.lbFTPServerPort.TabIndex = 12
        Me.lbFTPServerPort.Tag = "lbFTPServerPort"
        Me.lbFTPServerPort.Text = "lbFTPServerPort"
        '
        'lbFTPUserName
        '
        Me.lbFTPUserName.AutoSize = True
        Me.lbFTPUserName.Location = New System.Drawing.Point(22, 96)
        Me.lbFTPUserName.Name = "lbFTPUserName"
        Me.lbFTPUserName.Size = New System.Drawing.Size(85, 13)
        Me.lbFTPUserName.TabIndex = 14
        Me.lbFTPUserName.Tag = "lbFTPUserName"
        Me.lbFTPUserName.Text = "lbFTPUserName"
        '
        'lbSearchResultPath
        '
        Me.lbSearchResultPath.AutoSize = True
        Me.lbSearchResultPath.Location = New System.Drawing.Point(22, 159)
        Me.lbSearchResultPath.Name = "lbSearchResultPath"
        Me.lbSearchResultPath.Size = New System.Drawing.Size(101, 13)
        Me.lbSearchResultPath.TabIndex = 18
        Me.lbSearchResultPath.Tag = "lbSearchResultPath"
        Me.lbSearchResultPath.Text = "lbSearchResultPath"
        '
        'txtFTPPassword
        '
        Me.txtFTPPassword.Location = New System.Drawing.Point(242, 121)
        Me.txtFTPPassword.Name = "txtFTPPassword"
        Me.txtFTPPassword.Size = New System.Drawing.Size(179, 20)
        Me.txtFTPPassword.TabIndex = 17
        Me.txtFTPPassword.Tag = "FTP_PASSWORD"
        '
        'txtFTPServerPort
        '
        Me.txtFTPServerPort.Location = New System.Drawing.Point(242, 62)
        Me.txtFTPServerPort.Name = "txtFTPServerPort"
        Me.txtFTPServerPort.Size = New System.Drawing.Size(179, 20)
        Me.txtFTPServerPort.TabIndex = 13
        Me.txtFTPServerPort.Tag = "FTP_SERVER_PORT"
        '
        'txtFTPServerAddress
        '
        Me.txtFTPServerAddress.Location = New System.Drawing.Point(242, 35)
        Me.txtFTPServerAddress.Name = "txtFTPServerAddress"
        Me.txtFTPServerAddress.Size = New System.Drawing.Size(179, 20)
        Me.txtFTPServerAddress.TabIndex = 11
        Me.txtFTPServerAddress.Tag = "FTP_SERVER_ADDRESS"
        '
        'tpSF
        '
        Me.tpSF.Controls.Add(Me.grbSF)
        Me.tpSF.Location = New System.Drawing.Point(4, 22)
        Me.tpSF.Name = "tpSF"
        Me.tpSF.Size = New System.Drawing.Size(481, 325)
        Me.tpSF.TabIndex = 2
        Me.tpSF.Tag = "tpSF"
        Me.tpSF.Text = "tpSF"
        Me.tpSF.UseVisualStyleBackColor = True
        '
        'grbSF
        '
        Me.grbSF.Controls.Add(Me.txtInterestRateLevel1)
        Me.grbSF.Controls.Add(Me.lbMaxAnnualFund)
        Me.grbSF.Controls.Add(Me.lbInterestRateLevel2)
        Me.grbSF.Controls.Add(Me.lbInterestRateLevel1)
        Me.grbSF.Controls.Add(Me.txtInterestRateLevel2)
        Me.grbSF.Controls.Add(Me.txtMaxAnnualFund)
        Me.grbSF.Controls.Add(Me.txtAnnualFundRate)
        Me.grbSF.Controls.Add(Me.lbDateLimitLevel2)
        Me.grbSF.Controls.Add(Me.txtDateLimitLevel2)
        Me.grbSF.Controls.Add(Me.lbAnnualFundRate)
        Me.grbSF.Location = New System.Drawing.Point(8, 11)
        Me.grbSF.Name = "grbSF"
        Me.grbSF.Size = New System.Drawing.Size(465, 265)
        Me.grbSF.TabIndex = 1
        Me.grbSF.TabStop = False
        Me.grbSF.Tag = "grbSF"
        Me.grbSF.Text = "grbSF"
        '
        'txtInterestRateLevel1
        '
        Me.txtInterestRateLevel1.Location = New System.Drawing.Point(332, 141)
        Me.txtInterestRateLevel1.Name = "txtInterestRateLevel1"
        Me.txtInterestRateLevel1.Size = New System.Drawing.Size(100, 20)
        Me.txtInterestRateLevel1.TabIndex = 9
        Me.txtInterestRateLevel1.Tag = "INTEREST_RATE_LEVEL1"
        '
        'lbMaxAnnualFund
        '
        Me.lbMaxAnnualFund.AutoSize = True
        Me.lbMaxAnnualFund.Location = New System.Drawing.Point(33, 31)
        Me.lbMaxAnnualFund.Name = "lbMaxAnnualFund"
        Me.lbMaxAnnualFund.Size = New System.Drawing.Size(92, 13)
        Me.lbMaxAnnualFund.TabIndex = 0
        Me.lbMaxAnnualFund.Tag = "lbMaxAnnualFund"
        Me.lbMaxAnnualFund.Text = "lbMaxAnnualFund"
        '
        'lbInterestRateLevel2
        '
        Me.lbInterestRateLevel2.AutoSize = True
        Me.lbInterestRateLevel2.Location = New System.Drawing.Point(33, 58)
        Me.lbInterestRateLevel2.Name = "lbInterestRateLevel2"
        Me.lbInterestRateLevel2.Size = New System.Drawing.Size(105, 13)
        Me.lbInterestRateLevel2.TabIndex = 2
        Me.lbInterestRateLevel2.Tag = "lbInterestRateLevel2"
        Me.lbInterestRateLevel2.Text = "lbInterestRateLevel2"
        '
        'lbInterestRateLevel1
        '
        Me.lbInterestRateLevel1.AutoSize = True
        Me.lbInterestRateLevel1.Location = New System.Drawing.Point(33, 148)
        Me.lbInterestRateLevel1.Name = "lbInterestRateLevel1"
        Me.lbInterestRateLevel1.Size = New System.Drawing.Size(105, 13)
        Me.lbInterestRateLevel1.TabIndex = 8
        Me.lbInterestRateLevel1.Tag = "lbInterestRateLevel1"
        Me.lbInterestRateLevel1.Text = "lbInterestRateLevel1"
        '
        'txtInterestRateLevel2
        '
        Me.txtInterestRateLevel2.Location = New System.Drawing.Point(332, 51)
        Me.txtInterestRateLevel2.Name = "txtInterestRateLevel2"
        Me.txtInterestRateLevel2.Size = New System.Drawing.Size(100, 20)
        Me.txtInterestRateLevel2.TabIndex = 3
        Me.txtInterestRateLevel2.Tag = "INTEREST_RATE_LEVEL2"
        '
        'txtMaxAnnualFund
        '
        Me.txtMaxAnnualFund.Location = New System.Drawing.Point(332, 24)
        Me.txtMaxAnnualFund.Name = "txtMaxAnnualFund"
        Me.txtMaxAnnualFund.Size = New System.Drawing.Size(100, 20)
        Me.txtMaxAnnualFund.TabIndex = 1
        Me.txtMaxAnnualFund.Tag = "MAX_ANNUAL_FUND"
        '
        'txtAnnualFundRate
        '
        Me.txtAnnualFundRate.Location = New System.Drawing.Point(332, 110)
        Me.txtAnnualFundRate.Name = "txtAnnualFundRate"
        Me.txtAnnualFundRate.Size = New System.Drawing.Size(100, 20)
        Me.txtAnnualFundRate.TabIndex = 7
        Me.txtAnnualFundRate.Tag = "ANNUAL_FUND_RATE"
        '
        'lbDateLimitLevel2
        '
        Me.lbDateLimitLevel2.AutoSize = True
        Me.lbDateLimitLevel2.Location = New System.Drawing.Point(33, 85)
        Me.lbDateLimitLevel2.Name = "lbDateLimitLevel2"
        Me.lbDateLimitLevel2.Size = New System.Drawing.Size(91, 13)
        Me.lbDateLimitLevel2.TabIndex = 4
        Me.lbDateLimitLevel2.Tag = "lbDateLimitLevel2"
        Me.lbDateLimitLevel2.Text = "lbDateLimitLevel2"
        '
        'txtDateLimitLevel2
        '
        Me.txtDateLimitLevel2.Location = New System.Drawing.Point(332, 78)
        Me.txtDateLimitLevel2.Name = "txtDateLimitLevel2"
        Me.txtDateLimitLevel2.Size = New System.Drawing.Size(100, 20)
        Me.txtDateLimitLevel2.TabIndex = 5
        Me.txtDateLimitLevel2.Tag = "DATE_LIMIT_LEVEL2"
        '
        'lbAnnualFundRate
        '
        Me.lbAnnualFundRate.AutoSize = True
        Me.lbAnnualFundRate.Location = New System.Drawing.Point(33, 117)
        Me.lbAnnualFundRate.Name = "lbAnnualFundRate"
        Me.lbAnnualFundRate.Size = New System.Drawing.Size(95, 13)
        Me.lbAnnualFundRate.TabIndex = 6
        Me.lbAnnualFundRate.Tag = "lbAnnualFundRate"
        Me.lbAnnualFundRate.Text = "lbAnnualFundRate"
        '
        'tpRA
        '
        Me.tpRA.Location = New System.Drawing.Point(4, 22)
        Me.tpRA.Name = "tpRA"
        Me.tpRA.Size = New System.Drawing.Size(481, 325)
        Me.tpRA.TabIndex = 3
        Me.tpRA.Tag = "tpRA"
        Me.tpRA.Text = "tpRA"
        Me.tpRA.UseVisualStyleBackColor = True
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(322, 419)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(75, 32)
        Me.btnOk.TabIndex = 7
        Me.btnOk.Tag = "btnOk"
        Me.btnOk.Text = "btnOk"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(410, 419)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 33)
        Me.btnCancel.TabIndex = 8
        Me.btnCancel.Tag = "btnCancel"
        Me.btnCancel.Text = "btnCancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'lbMaxRowSearch
        '
        Me.lbMaxRowSearch.AutoSize = True
        Me.lbMaxRowSearch.Location = New System.Drawing.Point(22, 192)
        Me.lbMaxRowSearch.Name = "lbMaxRowSearch"
        Me.lbMaxRowSearch.Size = New System.Drawing.Size(91, 13)
        Me.lbMaxRowSearch.TabIndex = 20
        Me.lbMaxRowSearch.Tag = "lbMaxRowSearch"
        Me.lbMaxRowSearch.Text = "lbMaxRowSearch"
        '
        'txtMaxRowSearch
        '
        Me.txtMaxRowSearch.Location = New System.Drawing.Point(242, 186)
        Me.txtMaxRowSearch.Name = "txtMaxRowSearch"
        Me.txtMaxRowSearch.Size = New System.Drawing.Size(179, 20)
        Me.txtMaxRowSearch.TabIndex = 21
        Me.txtMaxRowSearch.Tag = "MAX_ROW_SEARCH"
        '
        'frmSysVarSetting
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(495, 455)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.tcTabControl)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "frmSysVarSetting"
        Me.Text = "frmSysVarSetting"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.tcTabControl.ResumeLayout(False)
        Me.tpSA.ResumeLayout(False)
        Me.grpSA.ResumeLayout(False)
        Me.grpSA.PerformLayout()
        Me.tpSYSTEM.ResumeLayout(False)
        Me.grbSYSTEM.ResumeLayout(False)
        Me.grbSYSTEM.PerformLayout()
        Me.tpSF.ResumeLayout(False)
        Me.grbSF.ResumeLayout(False)
        Me.grbSF.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents lbCaption As System.Windows.Forms.Label
    Friend WithEvents tcTabControl As System.Windows.Forms.TabControl
    Friend WithEvents tpSA As System.Windows.Forms.TabPage
    Friend WithEvents tpSYSTEM As System.Windows.Forms.TabPage
    Friend WithEvents tpSF As System.Windows.Forms.TabPage
    Friend WithEvents tpRA As System.Windows.Forms.TabPage
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents grbSF As System.Windows.Forms.GroupBox
    Friend WithEvents txtInterestRateLevel1 As System.Windows.Forms.TextBox
    Friend WithEvents lbMaxAnnualFund As System.Windows.Forms.Label
    Friend WithEvents lbInterestRateLevel2 As System.Windows.Forms.Label
    Friend WithEvents lbInterestRateLevel1 As System.Windows.Forms.Label
    Friend WithEvents txtInterestRateLevel2 As System.Windows.Forms.TextBox
    Friend WithEvents txtMaxAnnualFund As System.Windows.Forms.TextBox
    Friend WithEvents txtAnnualFundRate As System.Windows.Forms.TextBox
    Friend WithEvents lbDateLimitLevel2 As System.Windows.Forms.Label
    Friend WithEvents txtDateLimitLevel2 As System.Windows.Forms.TextBox
    Friend WithEvents lbAnnualFundRate As System.Windows.Forms.Label
    Friend WithEvents grpSA As System.Windows.Forms.GroupBox
    Friend WithEvents lbBranchId As System.Windows.Forms.Label
    Friend WithEvents cboBranchId As Sats.AppCore.ComboBoxEx
    Friend WithEvents lbChooseDate As System.Windows.Forms.Label
    Friend WithEvents mcdCalendar As System.Windows.Forms.MonthCalendar
    Friend WithEvents lbListDate As System.Windows.Forms.Label
    Friend WithEvents lstListDate As System.Windows.Forms.ListBox
    Friend WithEvents btnAddOne As System.Windows.Forms.Button
    Friend WithEvents btnRemoveOne As System.Windows.Forms.Button
    Friend WithEvents btnRemoveAll As System.Windows.Forms.Button
    Friend WithEvents btnInitAll As System.Windows.Forms.Button
    Friend WithEvents txtSearchResultPath As System.Windows.Forms.TextBox
    Friend WithEvents lbFTPServerAddress As System.Windows.Forms.Label
    Friend WithEvents lbFTPServerPort As System.Windows.Forms.Label
    Friend WithEvents lbSearchResultPath As System.Windows.Forms.Label
    Friend WithEvents txtFTPServerPort As System.Windows.Forms.TextBox
    Friend WithEvents txtFTPServerAddress As System.Windows.Forms.TextBox
    Friend WithEvents txtFTPPassword As System.Windows.Forms.TextBox
    Friend WithEvents lbFTPUserName As System.Windows.Forms.Label
    Friend WithEvents txtFTPUserName As System.Windows.Forms.TextBox
    Friend WithEvents lbFTPPassword As System.Windows.Forms.Label
    Friend WithEvents grbSYSTEM As System.Windows.Forms.GroupBox
    Friend WithEvents txtMaxRowSearch As System.Windows.Forms.TextBox
    Friend WithEvents lbMaxRowSearch As System.Windows.Forms.Label
End Class
