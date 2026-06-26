<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmExportData
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmExportData))
        Me.grbInfoStocks = New System.Windows.Forms.GroupBox
        Me.cbRoom = New System.Windows.Forms.CheckBox
        Me.cbIs = New System.Windows.Forms.CheckBox
        Me.cbMi = New System.Windows.Forms.CheckBox
        Me.cbSi = New System.Windows.Forms.CheckBox
        Me.cboBranch = New Sats.AppCore.ComboBoxEx
        Me.lbBranch = New System.Windows.Forms.Label
        Me.btnOk = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.lbCaption = New System.Windows.Forms.Label
        Me.tabcotrol = New System.Windows.Forms.TabControl
        Me.tpStocks = New System.Windows.Forms.TabPage
        Me.tpBanks = New System.Windows.Forms.TabPage
        Me.grbInfoBanks = New System.Windows.Forms.GroupBox
        Me.cboBranch2 = New Sats.AppCore.ComboBoxEx
        Me.lbBranch2 = New System.Windows.Forms.Label
        Me.rbDirect = New System.Windows.Forms.RadioButton
        Me.rbMultiPartite = New System.Windows.Forms.RadioButton
        Me.lbPayMode = New System.Windows.Forms.Label
        Me.cboFrequence = New Sats.AppCore.ComboBoxEx
        Me.lbFrequence = New System.Windows.Forms.Label
        Me.dtpTransDate = New System.Windows.Forms.DateTimePicker
        Me.lbDate = New System.Windows.Forms.Label
        Me.tpNHNN = New System.Windows.Forms.TabPage
        Me.cboBranch3 = New Sats.AppCore.ComboBoxEx
        Me.lbBranch3 = New System.Windows.Forms.Label
        Me.lbFromDate = New System.Windows.Forms.Label
        Me.lbToDate = New System.Windows.Forms.Label
        Me.dtpToDate = New System.Windows.Forms.DateTimePicker
        Me.dtpFromDate = New System.Windows.Forms.DateTimePicker
        Me.cbIs1 = New System.Windows.Forms.CheckBox
        Me.cbSi1 = New System.Windows.Forms.CheckBox
        Me.cbMi1 = New System.Windows.Forms.CheckBox
        Me.cbTrans = New System.Windows.Forms.CheckBox
        Me.tpBanks2 = New System.Windows.Forms.TabPage
        Me.cboCsErrType = New Sats.AppCore.ComboBoxEx
        Me.lbCsErrType = New System.Windows.Forms.Label
        Me.cboFrequence2 = New Sats.AppCore.ComboBoxEx
        Me.lbFrequence2 = New System.Windows.Forms.Label
        Me.cboBranch4 = New Sats.AppCore.ComboBoxEx
        Me.lbBranch4 = New System.Windows.Forms.Label
        Me.dtpTransDate2 = New System.Windows.Forms.DateTimePicker
        Me.lbDate2 = New System.Windows.Forms.Label
        Me.tpBanks3 = New System.Windows.Forms.TabPage
        Me.cboBranch5 = New Sats.AppCore.ComboBoxEx
        Me.lbBranch5 = New System.Windows.Forms.Label
        Me.dtpTransDate3 = New System.Windows.Forms.DateTimePicker
        Me.lbDate3 = New System.Windows.Forms.Label
        Me.tpCW = New System.Windows.Forms.TabPage
        Me.cboBranch6 = New Sats.AppCore.ComboBoxEx
        Me.lbBranch6 = New System.Windows.Forms.Label
        Me.dtpTransDate4 = New System.Windows.Forms.DateTimePicker
        Me.lbDate4 = New System.Windows.Forms.Label
        Me.grbInfoStocks.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.tabcotrol.SuspendLayout()
        Me.tpStocks.SuspendLayout()
        Me.tpBanks.SuspendLayout()
        Me.grbInfoBanks.SuspendLayout()
        Me.tpNHNN.SuspendLayout()
        Me.tpBanks2.SuspendLayout()
        Me.tpBanks3.SuspendLayout()
        Me.tpCW.SuspendLayout()
        Me.SuspendLayout()
        '
        'grbInfoStocks
        '
        Me.grbInfoStocks.Controls.Add(Me.cbRoom)
        Me.grbInfoStocks.Controls.Add(Me.cbIs)
        Me.grbInfoStocks.Controls.Add(Me.cbMi)
        Me.grbInfoStocks.Controls.Add(Me.cbSi)
        Me.grbInfoStocks.Controls.Add(Me.cboBranch)
        Me.grbInfoStocks.Controls.Add(Me.lbBranch)
        Me.grbInfoStocks.Location = New System.Drawing.Point(3, 6)
        Me.grbInfoStocks.Name = "grbInfoStocks"
        Me.grbInfoStocks.Size = New System.Drawing.Size(531, 184)
        Me.grbInfoStocks.TabIndex = 0
        Me.grbInfoStocks.TabStop = False
        Me.grbInfoStocks.Tag = "grbInfoStocks"
        Me.grbInfoStocks.Text = "grbInfoStocks"
        '
        'cbRoom
        '
        Me.cbRoom.AutoSize = True
        Me.cbRoom.Location = New System.Drawing.Point(281, 118)
        Me.cbRoom.Name = "cbRoom"
        Me.cbRoom.Size = New System.Drawing.Size(66, 17)
        Me.cbRoom.TabIndex = 9
        Me.cbRoom.Tag = "cbRoom"
        Me.cbRoom.Text = "cbRoom"
        Me.cbRoom.UseVisualStyleBackColor = True
        '
        'cbIs
        '
        Me.cbIs.AutoSize = True
        Me.cbIs.Location = New System.Drawing.Point(281, 84)
        Me.cbIs.Name = "cbIs"
        Me.cbIs.Size = New System.Drawing.Size(46, 17)
        Me.cbIs.TabIndex = 5
        Me.cbIs.Tag = "cbIs"
        Me.cbIs.Text = "cbIs"
        Me.cbIs.UseVisualStyleBackColor = True
        '
        'cbMi
        '
        Me.cbMi.AutoSize = True
        Me.cbMi.Location = New System.Drawing.Point(48, 118)
        Me.cbMi.Name = "cbMi"
        Me.cbMi.Size = New System.Drawing.Size(49, 17)
        Me.cbMi.TabIndex = 7
        Me.cbMi.Tag = "cbMi"
        Me.cbMi.Text = "cbMi"
        Me.cbMi.UseVisualStyleBackColor = True
        '
        'cbSi
        '
        Me.cbSi.AutoSize = True
        Me.cbSi.Location = New System.Drawing.Point(48, 84)
        Me.cbSi.Name = "cbSi"
        Me.cbSi.Size = New System.Drawing.Size(47, 17)
        Me.cbSi.TabIndex = 3
        Me.cbSi.Tag = "cbSi"
        Me.cbSi.Text = "cbSi"
        Me.cbSi.UseVisualStyleBackColor = True
        '
        'cboBranch
        '
        Me.cboBranch.DisplayMember = "DISPLAY"
        Me.cboBranch.FormattingEnabled = True
        Me.cboBranch.Location = New System.Drawing.Point(129, 25)
        Me.cboBranch.Name = "cboBranch"
        Me.cboBranch.Size = New System.Drawing.Size(342, 21)
        Me.cboBranch.TabIndex = 1
        Me.cboBranch.Tag = "cboBranch"
        Me.cboBranch.ValueMember = "VALUE"
        '
        'lbBranch
        '
        Me.lbBranch.AutoSize = True
        Me.lbBranch.Location = New System.Drawing.Point(45, 28)
        Me.lbBranch.Name = "lbBranch"
        Me.lbBranch.Size = New System.Drawing.Size(49, 13)
        Me.lbBranch.TabIndex = 0
        Me.lbBranch.Tag = "lbBranch"
        Me.lbBranch.Text = "lbBranch"
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(208, 295)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(77, 33)
        Me.btnOk.TabIndex = 30
        Me.btnOk.Tag = "btnOk"
        Me.btnOk.Text = "btnOk"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(291, 295)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 33)
        Me.btnCancel.TabIndex = 32
        Me.btnCancel.Tag = "btnCancel"
        Me.btnCancel.Text = "btnCancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.SystemColors.Desktop
        Me.Panel1.Controls.Add(Me.lbCaption)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(561, 56)
        Me.Panel1.TabIndex = 3
        Me.Panel1.Tag = "Panel1"
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
        'tabcotrol
        '
        Me.tabcotrol.Controls.Add(Me.tpStocks)
        Me.tabcotrol.Controls.Add(Me.tpBanks)
        Me.tabcotrol.Controls.Add(Me.tpNHNN)
        Me.tabcotrol.Controls.Add(Me.tpBanks2)
        Me.tabcotrol.Controls.Add(Me.tpBanks3)
        Me.tabcotrol.Controls.Add(Me.tpCW)
        Me.tabcotrol.Location = New System.Drawing.Point(7, 62)
        Me.tabcotrol.Name = "tabcotrol"
        Me.tabcotrol.SelectedIndex = 0
        Me.tabcotrol.Size = New System.Drawing.Size(545, 222)
        Me.tabcotrol.TabIndex = 4
        Me.tabcotrol.Tag = "tabcotrol"
        '
        'tpStocks
        '
        Me.tpStocks.Controls.Add(Me.grbInfoStocks)
        Me.tpStocks.Location = New System.Drawing.Point(4, 22)
        Me.tpStocks.Name = "tpStocks"
        Me.tpStocks.Padding = New System.Windows.Forms.Padding(3)
        Me.tpStocks.Size = New System.Drawing.Size(537, 196)
        Me.tpStocks.TabIndex = 0
        Me.tpStocks.Tag = "tpStocks"
        Me.tpStocks.Text = "tpStocks"
        Me.tpStocks.UseVisualStyleBackColor = True
        '
        'tpBanks
        '
        Me.tpBanks.Controls.Add(Me.grbInfoBanks)
        Me.tpBanks.Location = New System.Drawing.Point(4, 22)
        Me.tpBanks.Name = "tpBanks"
        Me.tpBanks.Padding = New System.Windows.Forms.Padding(3)
        Me.tpBanks.Size = New System.Drawing.Size(537, 196)
        Me.tpBanks.TabIndex = 1
        Me.tpBanks.Tag = "tpBanks"
        Me.tpBanks.Text = "tpBanks"
        Me.tpBanks.UseVisualStyleBackColor = True
        '
        'grbInfoBanks
        '
        Me.grbInfoBanks.Controls.Add(Me.cboBranch2)
        Me.grbInfoBanks.Controls.Add(Me.lbBranch2)
        Me.grbInfoBanks.Controls.Add(Me.rbDirect)
        Me.grbInfoBanks.Controls.Add(Me.rbMultiPartite)
        Me.grbInfoBanks.Controls.Add(Me.lbPayMode)
        Me.grbInfoBanks.Controls.Add(Me.cboFrequence)
        Me.grbInfoBanks.Controls.Add(Me.lbFrequence)
        Me.grbInfoBanks.Controls.Add(Me.dtpTransDate)
        Me.grbInfoBanks.Controls.Add(Me.lbDate)
        Me.grbInfoBanks.Location = New System.Drawing.Point(6, 6)
        Me.grbInfoBanks.Name = "grbInfoBanks"
        Me.grbInfoBanks.Size = New System.Drawing.Size(525, 184)
        Me.grbInfoBanks.TabIndex = 2
        Me.grbInfoBanks.TabStop = False
        Me.grbInfoBanks.Tag = "grbInfoBanks"
        Me.grbInfoBanks.Text = "grbInfoBanks"
        '
        'cboBranch2
        '
        Me.cboBranch2.DisplayMember = "DISPLAY"
        Me.cboBranch2.FormattingEnabled = True
        Me.cboBranch2.Location = New System.Drawing.Point(158, 19)
        Me.cboBranch2.Name = "cboBranch2"
        Me.cboBranch2.Size = New System.Drawing.Size(330, 21)
        Me.cboBranch2.TabIndex = 9
        Me.cboBranch2.Tag = "cboBranch2"
        Me.cboBranch2.ValueMember = "VALUE"
        '
        'lbBranch2
        '
        Me.lbBranch2.AutoSize = True
        Me.lbBranch2.Location = New System.Drawing.Point(29, 25)
        Me.lbBranch2.Name = "lbBranch2"
        Me.lbBranch2.Size = New System.Drawing.Size(55, 13)
        Me.lbBranch2.TabIndex = 8
        Me.lbBranch2.Tag = "lbBranch2"
        Me.lbBranch2.Text = "lbBranch2"
        '
        'rbDirect
        '
        Me.rbDirect.AutoSize = True
        Me.rbDirect.Location = New System.Drawing.Point(369, 132)
        Me.rbDirect.Name = "rbDirect"
        Me.rbDirect.Size = New System.Drawing.Size(62, 17)
        Me.rbDirect.TabIndex = 17
        Me.rbDirect.Tag = "rbDirect"
        Me.rbDirect.Text = "rbDirect"
        Me.rbDirect.UseVisualStyleBackColor = True
        '
        'rbMultiPartite
        '
        Me.rbMultiPartite.AutoSize = True
        Me.rbMultiPartite.Checked = True
        Me.rbMultiPartite.Location = New System.Drawing.Point(158, 132)
        Me.rbMultiPartite.Name = "rbMultiPartite"
        Me.rbMultiPartite.Size = New System.Drawing.Size(86, 17)
        Me.rbMultiPartite.TabIndex = 15
        Me.rbMultiPartite.TabStop = True
        Me.rbMultiPartite.Tag = "rbMultiPartite"
        Me.rbMultiPartite.Text = "rbMultiPartite"
        Me.rbMultiPartite.UseVisualStyleBackColor = True
        '
        'lbPayMode
        '
        Me.lbPayMode.AutoSize = True
        Me.lbPayMode.Location = New System.Drawing.Point(29, 132)
        Me.lbPayMode.Name = "lbPayMode"
        Me.lbPayMode.Size = New System.Drawing.Size(60, 13)
        Me.lbPayMode.TabIndex = 4
        Me.lbPayMode.Tag = "lbPayMode"
        Me.lbPayMode.Text = "lbPayMode"
        '
        'cboFrequence
        '
        Me.cboFrequence.DisplayMember = "DISPLAY"
        Me.cboFrequence.FormattingEnabled = True
        Me.cboFrequence.Location = New System.Drawing.Point(158, 86)
        Me.cboFrequence.Name = "cboFrequence"
        Me.cboFrequence.Size = New System.Drawing.Size(124, 21)
        Me.cboFrequence.TabIndex = 13
        Me.cboFrequence.Tag = "cboFrequence"
        Me.cboFrequence.ValueMember = "VALUE"
        '
        'lbFrequence
        '
        Me.lbFrequence.AutoSize = True
        Me.lbFrequence.Location = New System.Drawing.Point(29, 89)
        Me.lbFrequence.Name = "lbFrequence"
        Me.lbFrequence.Size = New System.Drawing.Size(66, 13)
        Me.lbFrequence.TabIndex = 2
        Me.lbFrequence.Tag = "lbFrequence"
        Me.lbFrequence.Text = "lbFrequence"
        '
        'dtpTransDate
        '
        Me.dtpTransDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpTransDate.Location = New System.Drawing.Point(158, 53)
        Me.dtpTransDate.Name = "dtpTransDate"
        Me.dtpTransDate.Size = New System.Drawing.Size(124, 20)
        Me.dtpTransDate.TabIndex = 11
        Me.dtpTransDate.Tag = "TransDate"
        '
        'lbDate
        '
        Me.lbDate.AutoSize = True
        Me.lbDate.Location = New System.Drawing.Point(29, 57)
        Me.lbDate.Name = "lbDate"
        Me.lbDate.Size = New System.Drawing.Size(38, 13)
        Me.lbDate.TabIndex = 0
        Me.lbDate.Tag = "lbDate"
        Me.lbDate.Text = "lbDate"
        '
        'tpNHNN
        '
        Me.tpNHNN.Controls.Add(Me.cboBranch3)
        Me.tpNHNN.Controls.Add(Me.lbBranch3)
        Me.tpNHNN.Controls.Add(Me.lbFromDate)
        Me.tpNHNN.Controls.Add(Me.lbToDate)
        Me.tpNHNN.Controls.Add(Me.dtpToDate)
        Me.tpNHNN.Controls.Add(Me.dtpFromDate)
        Me.tpNHNN.Controls.Add(Me.cbIs1)
        Me.tpNHNN.Controls.Add(Me.cbSi1)
        Me.tpNHNN.Controls.Add(Me.cbMi1)
        Me.tpNHNN.Controls.Add(Me.cbTrans)
        Me.tpNHNN.Location = New System.Drawing.Point(4, 22)
        Me.tpNHNN.Name = "tpNHNN"
        Me.tpNHNN.Padding = New System.Windows.Forms.Padding(3)
        Me.tpNHNN.Size = New System.Drawing.Size(537, 196)
        Me.tpNHNN.TabIndex = 2
        Me.tpNHNN.Tag = "tpNHNN"
        Me.tpNHNN.Text = "tpNHNN"
        Me.tpNHNN.UseVisualStyleBackColor = True
        '
        'cboBranch3
        '
        Me.cboBranch3.DisplayMember = "DISPLAY"
        Me.cboBranch3.FormattingEnabled = True
        Me.cboBranch3.Location = New System.Drawing.Point(130, 26)
        Me.cboBranch3.Name = "cboBranch3"
        Me.cboBranch3.Size = New System.Drawing.Size(342, 21)
        Me.cboBranch3.TabIndex = 21
        Me.cboBranch3.Tag = "cboBranch"
        Me.cboBranch3.ValueMember = "VALUE"
        '
        'lbBranch3
        '
        Me.lbBranch3.AutoSize = True
        Me.lbBranch3.Location = New System.Drawing.Point(46, 29)
        Me.lbBranch3.Name = "lbBranch3"
        Me.lbBranch3.Size = New System.Drawing.Size(55, 13)
        Me.lbBranch3.TabIndex = 20
        Me.lbBranch3.Tag = "lbBranch3"
        Me.lbBranch3.Text = "lbBranch3"
        '
        'lbFromDate
        '
        Me.lbFromDate.AutoSize = True
        Me.lbFromDate.Location = New System.Drawing.Point(46, 76)
        Me.lbFromDate.Name = "lbFromDate"
        Me.lbFromDate.Size = New System.Drawing.Size(61, 13)
        Me.lbFromDate.TabIndex = 19
        Me.lbFromDate.Tag = "lbFromDate"
        Me.lbFromDate.Text = "lbFromDate"
        '
        'lbToDate
        '
        Me.lbToDate.AutoSize = True
        Me.lbToDate.Location = New System.Drawing.Point(289, 76)
        Me.lbToDate.Name = "lbToDate"
        Me.lbToDate.Size = New System.Drawing.Size(51, 13)
        Me.lbToDate.TabIndex = 18
        Me.lbToDate.Tag = "lbToDate"
        Me.lbToDate.Text = "lbToDate"
        '
        'dtpToDate
        '
        Me.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpToDate.Location = New System.Drawing.Point(346, 72)
        Me.dtpToDate.Name = "dtpToDate"
        Me.dtpToDate.Size = New System.Drawing.Size(124, 20)
        Me.dtpToDate.TabIndex = 17
        Me.dtpToDate.Tag = "dtpToDate"
        '
        'dtpFromDate
        '
        Me.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpFromDate.Location = New System.Drawing.Point(130, 69)
        Me.dtpFromDate.Name = "dtpFromDate"
        Me.dtpFromDate.Size = New System.Drawing.Size(124, 20)
        Me.dtpFromDate.TabIndex = 16
        Me.dtpFromDate.Tag = "dtpFromDate"
        '
        'cbIs1
        '
        Me.cbIs1.AutoSize = True
        Me.cbIs1.Location = New System.Drawing.Point(357, 151)
        Me.cbIs1.Name = "cbIs1"
        Me.cbIs1.Size = New System.Drawing.Size(52, 17)
        Me.cbIs1.TabIndex = 15
        Me.cbIs1.Tag = "cbIs1"
        Me.cbIs1.Text = "cbIs1"
        Me.cbIs1.UseVisualStyleBackColor = True
        '
        'cbSi1
        '
        Me.cbSi1.AutoSize = True
        Me.cbSi1.Location = New System.Drawing.Point(357, 117)
        Me.cbSi1.Name = "cbSi1"
        Me.cbSi1.Size = New System.Drawing.Size(53, 17)
        Me.cbSi1.TabIndex = 13
        Me.cbSi1.Tag = "cbSi1"
        Me.cbSi1.Text = "cbSi1"
        Me.cbSi1.UseVisualStyleBackColor = False
        '
        'cbMi1
        '
        Me.cbMi1.AutoSize = True
        Me.cbMi1.Location = New System.Drawing.Point(124, 151)
        Me.cbMi1.Name = "cbMi1"
        Me.cbMi1.Size = New System.Drawing.Size(55, 17)
        Me.cbMi1.TabIndex = 14
        Me.cbMi1.Tag = "cbMi1"
        Me.cbMi1.Text = "cbMi1"
        Me.cbMi1.UseVisualStyleBackColor = True
        '
        'cbTrans
        '
        Me.cbTrans.AutoSize = True
        Me.cbTrans.Location = New System.Drawing.Point(124, 117)
        Me.cbTrans.Name = "cbTrans"
        Me.cbTrans.Size = New System.Drawing.Size(65, 17)
        Me.cbTrans.TabIndex = 12
        Me.cbTrans.Tag = "cbTrans"
        Me.cbTrans.Text = "cbTrans"
        Me.cbTrans.UseVisualStyleBackColor = True
        '
        'tpBanks2
        '
        Me.tpBanks2.Controls.Add(Me.cboCsErrType)
        Me.tpBanks2.Controls.Add(Me.lbCsErrType)
        Me.tpBanks2.Controls.Add(Me.cboFrequence2)
        Me.tpBanks2.Controls.Add(Me.lbFrequence2)
        Me.tpBanks2.Controls.Add(Me.cboBranch4)
        Me.tpBanks2.Controls.Add(Me.lbBranch4)
        Me.tpBanks2.Controls.Add(Me.dtpTransDate2)
        Me.tpBanks2.Controls.Add(Me.lbDate2)
        Me.tpBanks2.Location = New System.Drawing.Point(4, 22)
        Me.tpBanks2.Name = "tpBanks2"
        Me.tpBanks2.Size = New System.Drawing.Size(537, 196)
        Me.tpBanks2.TabIndex = 3
        Me.tpBanks2.Tag = "tpBanks2"
        Me.tpBanks2.Text = "tpBanks2"
        Me.tpBanks2.UseVisualStyleBackColor = True
        '
        'cboCsErrType
        '
        Me.cboCsErrType.DisplayMember = "DISPLAY"
        Me.cboCsErrType.FormattingEnabled = True
        Me.cboCsErrType.Location = New System.Drawing.Point(140, 124)
        Me.cboCsErrType.Name = "cboCsErrType"
        Me.cboCsErrType.Size = New System.Drawing.Size(124, 21)
        Me.cboCsErrType.TabIndex = 19
        Me.cboCsErrType.Tag = "cboCsErrType"
        Me.cboCsErrType.ValueMember = "VALUE"
        '
        'lbCsErrType
        '
        Me.lbCsErrType.AutoSize = True
        Me.lbCsErrType.Location = New System.Drawing.Point(11, 127)
        Me.lbCsErrType.Name = "lbCsErrType"
        Me.lbCsErrType.Size = New System.Drawing.Size(64, 13)
        Me.lbCsErrType.TabIndex = 18
        Me.lbCsErrType.Tag = "lbCsErrType"
        Me.lbCsErrType.Text = "lbCsErrType"
        '
        'cboFrequence2
        '
        Me.cboFrequence2.DisplayMember = "DISPLAY"
        Me.cboFrequence2.FormattingEnabled = True
        Me.cboFrequence2.Location = New System.Drawing.Point(140, 97)
        Me.cboFrequence2.Name = "cboFrequence2"
        Me.cboFrequence2.Size = New System.Drawing.Size(124, 21)
        Me.cboFrequence2.TabIndex = 17
        Me.cboFrequence2.Tag = "cboFrequence2"
        Me.cboFrequence2.ValueMember = "VALUE"
        '
        'lbFrequence2
        '
        Me.lbFrequence2.AutoSize = True
        Me.lbFrequence2.Location = New System.Drawing.Point(11, 100)
        Me.lbFrequence2.Name = "lbFrequence2"
        Me.lbFrequence2.Size = New System.Drawing.Size(72, 13)
        Me.lbFrequence2.TabIndex = 16
        Me.lbFrequence2.Tag = "lbFrequence2"
        Me.lbFrequence2.Text = "lbFrequence2"
        '
        'cboBranch4
        '
        Me.cboBranch4.DisplayMember = "DISPLAY"
        Me.cboBranch4.FormattingEnabled = True
        Me.cboBranch4.Location = New System.Drawing.Point(140, 23)
        Me.cboBranch4.Name = "cboBranch4"
        Me.cboBranch4.Size = New System.Drawing.Size(330, 21)
        Me.cboBranch4.TabIndex = 14
        Me.cboBranch4.Tag = "cboBranch4"
        Me.cboBranch4.ValueMember = "VALUE"
        '
        'lbBranch4
        '
        Me.lbBranch4.AutoSize = True
        Me.lbBranch4.Location = New System.Drawing.Point(11, 29)
        Me.lbBranch4.Name = "lbBranch4"
        Me.lbBranch4.Size = New System.Drawing.Size(55, 13)
        Me.lbBranch4.TabIndex = 13
        Me.lbBranch4.Tag = "lbBranch4"
        Me.lbBranch4.Text = "lbBranch4"
        '
        'dtpTransDate2
        '
        Me.dtpTransDate2.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpTransDate2.Location = New System.Drawing.Point(140, 61)
        Me.dtpTransDate2.Name = "dtpTransDate2"
        Me.dtpTransDate2.Size = New System.Drawing.Size(124, 20)
        Me.dtpTransDate2.TabIndex = 15
        Me.dtpTransDate2.Tag = "TransDate"
        '
        'lbDate2
        '
        Me.lbDate2.AutoSize = True
        Me.lbDate2.Location = New System.Drawing.Point(11, 61)
        Me.lbDate2.Name = "lbDate2"
        Me.lbDate2.Size = New System.Drawing.Size(44, 13)
        Me.lbDate2.TabIndex = 12
        Me.lbDate2.Tag = "lbDate2"
        Me.lbDate2.Text = "lbDate2"
        '
        'tpBanks3
        '
        Me.tpBanks3.Controls.Add(Me.cboBranch5)
        Me.tpBanks3.Controls.Add(Me.lbBranch5)
        Me.tpBanks3.Controls.Add(Me.dtpTransDate3)
        Me.tpBanks3.Controls.Add(Me.lbDate3)
        Me.tpBanks3.Location = New System.Drawing.Point(4, 22)
        Me.tpBanks3.Name = "tpBanks3"
        Me.tpBanks3.Size = New System.Drawing.Size(537, 196)
        Me.tpBanks3.TabIndex = 4
        Me.tpBanks3.Tag = "tpBanks3"
        Me.tpBanks3.Text = "tpBanks3"
        Me.tpBanks3.UseVisualStyleBackColor = True
        '
        'cboBranch5
        '
        Me.cboBranch5.DisplayMember = "DISPLAY"
        Me.cboBranch5.FormattingEnabled = True
        Me.cboBranch5.Location = New System.Drawing.Point(146, 22)
        Me.cboBranch5.Name = "cboBranch5"
        Me.cboBranch5.Size = New System.Drawing.Size(330, 21)
        Me.cboBranch5.TabIndex = 18
        Me.cboBranch5.Tag = "cboBranch5"
        'Me.cboBranch5.Text = "cboBranch5"
        Me.cboBranch5.ValueMember = "VALUE"
        '
        'lbBranch5
        '
        Me.lbBranch5.AutoSize = True
        Me.lbBranch5.Location = New System.Drawing.Point(17, 28)
        Me.lbBranch5.Name = "lbBranch5"
        Me.lbBranch5.Size = New System.Drawing.Size(55, 13)
        Me.lbBranch5.TabIndex = 17
        Me.lbBranch5.Tag = "lbBranch5"
        Me.lbBranch5.Text = "lbBranch5"
        '
        'dtpTransDate3
        '
        Me.dtpTransDate3.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpTransDate3.Location = New System.Drawing.Point(146, 60)
        Me.dtpTransDate3.Name = "dtpTransDate3"
        Me.dtpTransDate3.Size = New System.Drawing.Size(124, 20)
        Me.dtpTransDate3.TabIndex = 19
        Me.dtpTransDate3.Tag = "TransDate"
        '
        'lbDate3
        '
        Me.lbDate3.AutoSize = True
        Me.lbDate3.Location = New System.Drawing.Point(17, 60)
        Me.lbDate3.Name = "lbDate3"
        Me.lbDate3.Size = New System.Drawing.Size(44, 13)
        Me.lbDate3.TabIndex = 16
        Me.lbDate3.Tag = "lbDate3"
        Me.lbDate3.Text = "lbDate3"
        '
        'tpCW
        '
        Me.tpCW.Controls.Add(Me.cboBranch6)
        Me.tpCW.Controls.Add(Me.lbBranch6)
        Me.tpCW.Controls.Add(Me.dtpTransDate4)
        Me.tpCW.Controls.Add(Me.lbDate4)
        Me.tpCW.Location = New System.Drawing.Point(4, 22)
        Me.tpCW.Name = "tpCW"
        Me.tpCW.Padding = New System.Windows.Forms.Padding(3)
        Me.tpCW.Size = New System.Drawing.Size(537, 196)
        Me.tpCW.TabIndex = 5
        Me.tpCW.Tag = "tpCW"
        Me.tpCW.Text = "tpCW"
        Me.tpCW.UseVisualStyleBackColor = True
        '
        'cboBranch6
        '
        Me.cboBranch6.DisplayMember = "DISPLAY"
        Me.cboBranch6.FormattingEnabled = True
        Me.cboBranch6.Location = New System.Drawing.Point(162, 21)
        Me.cboBranch6.Name = "cboBranch6"
        Me.cboBranch6.Size = New System.Drawing.Size(330, 21)
        Me.cboBranch6.TabIndex = 22
        Me.cboBranch6.Tag = "cboBranch6"
        Me.cboBranch6.ValueMember = "VALUE"
        '
        'lbBranch6
        '
        Me.lbBranch6.AutoSize = True
        Me.lbBranch6.Location = New System.Drawing.Point(33, 27)
        Me.lbBranch6.Name = "lbBranch6"
        Me.lbBranch6.Size = New System.Drawing.Size(55, 13)
        Me.lbBranch6.TabIndex = 21
        Me.lbBranch6.Tag = "lbBranch6"
        Me.lbBranch6.Text = "lbBranch6"
        '
        'dtpTransDate4
        '
        Me.dtpTransDate4.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpTransDate4.Location = New System.Drawing.Point(162, 59)
        Me.dtpTransDate4.Name = "dtpTransDate4"
        Me.dtpTransDate4.Size = New System.Drawing.Size(124, 20)
        Me.dtpTransDate4.TabIndex = 23
        Me.dtpTransDate4.Tag = "TransDate"
        '
        'lbDate4
        '
        Me.lbDate4.AutoSize = True
        Me.lbDate4.Location = New System.Drawing.Point(33, 59)
        Me.lbDate4.Name = "lbDate4"
        Me.lbDate4.Size = New System.Drawing.Size(44, 13)
        Me.lbDate4.TabIndex = 20
        Me.lbDate4.Tag = "lbDate4"
        Me.lbDate4.Text = "lbDate4"
        '
        'frmExportData
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(561, 340)
        Me.Controls.Add(Me.tabcotrol)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOk)
        Me.Name = "frmExportData"
        Me.Tag = "frmExportData"
        Me.Text = "frmExportData"
        Me.grbInfoStocks.ResumeLayout(False)
        Me.grbInfoStocks.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.tabcotrol.ResumeLayout(False)
        Me.tpStocks.ResumeLayout(False)
        Me.tpBanks.ResumeLayout(False)
        Me.grbInfoBanks.ResumeLayout(False)
        Me.grbInfoBanks.PerformLayout()
        Me.tpNHNN.ResumeLayout(False)
        Me.tpNHNN.PerformLayout()
        Me.tpBanks2.ResumeLayout(False)
        Me.tpBanks2.PerformLayout()
        Me.tpBanks3.ResumeLayout(False)
        Me.tpBanks3.PerformLayout()
        Me.tpCW.ResumeLayout(False)
        Me.tpCW.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grbInfoStocks As System.Windows.Forms.GroupBox
    Friend WithEvents cbRoom As System.Windows.Forms.CheckBox
    Friend WithEvents cbIs As System.Windows.Forms.CheckBox
    Friend WithEvents cbMi As System.Windows.Forms.CheckBox
    Friend WithEvents cbSi As System.Windows.Forms.CheckBox
    Friend WithEvents cboBranch As Sats.AppCore.ComboBoxEx
    Friend WithEvents lbBranch As System.Windows.Forms.Label
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents lbCaption As System.Windows.Forms.Label
    Friend WithEvents tabcotrol As System.Windows.Forms.TabControl
    Friend WithEvents tpStocks As System.Windows.Forms.TabPage
    Friend WithEvents tpBanks As System.Windows.Forms.TabPage
    Friend WithEvents dtpTransDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents lbDate As System.Windows.Forms.Label
    Friend WithEvents grbInfoBanks As System.Windows.Forms.GroupBox
    Friend WithEvents lbFrequence As System.Windows.Forms.Label
    Friend WithEvents rbDirect As System.Windows.Forms.RadioButton
    Friend WithEvents rbMultiPartite As System.Windows.Forms.RadioButton
    Friend WithEvents lbPayMode As System.Windows.Forms.Label
    Friend WithEvents cboFrequence As Sats.AppCore.ComboBoxEx
    Friend WithEvents cboBranch2 As Sats.AppCore.ComboBoxEx
    Friend WithEvents lbBranch2 As System.Windows.Forms.Label
    Friend WithEvents tpNHNN As System.Windows.Forms.TabPage
    Friend WithEvents cbIs1 As System.Windows.Forms.CheckBox
    Friend WithEvents cbSi1 As System.Windows.Forms.CheckBox
    Friend WithEvents cbMi1 As System.Windows.Forms.CheckBox
    Friend WithEvents cbTrans As System.Windows.Forms.CheckBox
    Friend WithEvents lbFromDate As System.Windows.Forms.Label
    Friend WithEvents lbToDate As System.Windows.Forms.Label
    Friend WithEvents dtpToDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtpFromDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents cboBranch3 As Sats.AppCore.ComboBoxEx
    Friend WithEvents lbBranch3 As System.Windows.Forms.Label
    Friend WithEvents tpBanks2 As System.Windows.Forms.TabPage
    Friend WithEvents cboBranch4 As Sats.AppCore.ComboBoxEx
    Friend WithEvents lbBranch4 As System.Windows.Forms.Label
    Friend WithEvents dtpTransDate2 As System.Windows.Forms.DateTimePicker
    Friend WithEvents lbDate2 As System.Windows.Forms.Label
    Friend WithEvents cboFrequence2 As Sats.AppCore.ComboBoxEx
    Friend WithEvents lbFrequence2 As System.Windows.Forms.Label
    Friend WithEvents cboCsErrType As Sats.AppCore.ComboBoxEx
    Friend WithEvents lbCsErrType As System.Windows.Forms.Label
    Friend WithEvents tpBanks3 As System.Windows.Forms.TabPage
    Friend WithEvents cboBranch5 As Sats.AppCore.ComboBoxEx
    Friend WithEvents lbBranch5 As System.Windows.Forms.Label
    Friend WithEvents dtpTransDate3 As System.Windows.Forms.DateTimePicker
    Friend WithEvents lbDate3 As System.Windows.Forms.Label
    Friend WithEvents tpCW As System.Windows.Forms.TabPage
    Friend WithEvents cboBranch6 As Sats.AppCore.ComboBoxEx
    Friend WithEvents lbBranch6 As System.Windows.Forms.Label
    Friend WithEvents dtpTransDate4 As System.Windows.Forms.DateTimePicker
    Friend WithEvents lbDate4 As System.Windows.Forms.Label
End Class
