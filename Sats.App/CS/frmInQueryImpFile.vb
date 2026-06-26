Imports Sats.AppCore
Imports System.IO
Imports System.Collections
Imports Sats.CommonLibrary
Imports Xceed.SmartUI.Controls
Imports Sats.WinFormsUI.Docking
Imports System.Windows.Forms

Public Class frmInQueryImpFile
    Inherits Sats.WinFormsUI.Docking.DockContent

    Private mv_oProxy As BDSChannel.BDSDelivery

    Public Property Proxy() As BDSChannel.BDSDelivery
        Get
            Return mv_oProxy
        End Get
        Set(ByVal value As BDSChannel.BDSDelivery)
            mv_oProxy = value
        End Set
    End Property
#Region "Design"
    Friend WithEvents grbSearchFilter As System.Windows.Forms.GroupBox
    Friend WithEvents pnlSearchResult As System.Windows.Forms.Panel
    Friend WithEvents grbSearchResult As System.Windows.Forms.GroupBox
    Friend WithEvents lbField As System.Windows.Forms.Label
    Friend WithEvents lbOperator As System.Windows.Forms.Label
    Friend WithEvents lbValue As System.Windows.Forms.Label
    Friend WithEvents txtValue As System.Windows.Forms.Control
    Friend WithEvents cboField As Sats.AppCore.ComboBoxEx
    Friend WithEvents grbCondition As System.Windows.Forms.GroupBox
    Friend WithEvents cboOperator As Sats.AppCore.ComboBoxEx
    Friend WithEvents lstCondition As System.Windows.Forms.CheckedListBox
    Friend WithEvents grbConditionList As System.Windows.Forms.GroupBox
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents btnRemove As System.Windows.Forms.Button
    Friend WithEvents btnRemoveAll As System.Windows.Forms.Button
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents grpFileInfo As System.Windows.Forms.GroupBox
    Friend WithEvents lblFilePath As System.Windows.Forms.Label
    Friend WithEvents cboFileType As Sats.AppCore.ComboBoxEx
    Friend WithEvents lblFileType As System.Windows.Forms.Label
    Friend WithEvents btnLoad As System.Windows.Forms.Button
    Friend WithEvents btnBrown As System.Windows.Forms.Button
    Friend WithEvents txtPath As System.Windows.Forms.TextBox
    Friend WithEvents txtNum As System.Windows.Forms.TextBox
    Friend WithEvents lblNum As System.Windows.Forms.Label
    Friend WithEvents btnNext As System.Windows.Forms.Button
    Friend WithEvents btnPre As System.Windows.Forms.Button
    Private components As System.ComponentModel.IContainer
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInQueryImpFile))
        Me.btnSearch = New System.Windows.Forms.Button
        Me.btnRemoveAll = New System.Windows.Forms.Button
        Me.btnRemove = New System.Windows.Forms.Button
        Me.btnAdd = New System.Windows.Forms.Button
        Me.grbConditionList = New System.Windows.Forms.GroupBox
        Me.lstCondition = New System.Windows.Forms.CheckedListBox
        Me.cboOperator = New Sats.AppCore.ComboBoxEx
        Me.grbCondition = New System.Windows.Forms.GroupBox
        Me.cboField = New Sats.AppCore.ComboBoxEx
        Me.txtValue = New System.Windows.Forms.Control
        Me.lbValue = New System.Windows.Forms.Label
        Me.lbOperator = New System.Windows.Forms.Label
        Me.lbField = New System.Windows.Forms.Label
        Me.grbSearchResult = New System.Windows.Forms.GroupBox
        Me.pnlSearchResult = New System.Windows.Forms.Panel
        Me.grbSearchFilter = New System.Windows.Forms.GroupBox
        Me.btnNext = New System.Windows.Forms.Button
        Me.btnPre = New System.Windows.Forms.Button
        Me.txtNum = New System.Windows.Forms.TextBox
        Me.lblNum = New System.Windows.Forms.Label
        Me.btnLoad = New System.Windows.Forms.Button
        Me.grpFileInfo = New System.Windows.Forms.GroupBox
        Me.btnBrown = New System.Windows.Forms.Button
        Me.txtPath = New System.Windows.Forms.TextBox
        Me.lblFilePath = New System.Windows.Forms.Label
        Me.cboFileType = New Sats.AppCore.ComboBoxEx
        Me.lblFileType = New System.Windows.Forms.Label
        Me.grbConditionList.SuspendLayout()
        Me.grbCondition.SuspendLayout()
        Me.grbSearchResult.SuspendLayout()
        Me.grbSearchFilter.SuspendLayout()
        Me.grpFileInfo.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnSearch
        '
        Me.btnSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSearch.Location = New System.Drawing.Point(703, 163)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(94, 28)
        Me.btnSearch.TabIndex = 13
        Me.btnSearch.Text = "btnFilter"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnRemoveAll
        '
        Me.btnRemoveAll.Font = New System.Drawing.Font("Webdings", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.btnRemoveAll.Location = New System.Drawing.Point(320, 157)
        Me.btnRemoveAll.Name = "btnRemoveAll"
        Me.btnRemoveAll.Size = New System.Drawing.Size(25, 25)
        Me.btnRemoveAll.TabIndex = 12
        Me.btnRemoveAll.Text = "7"
        Me.btnRemoveAll.UseVisualStyleBackColor = True
        '
        'btnRemove
        '
        Me.btnRemove.Font = New System.Drawing.Font("Webdings", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.btnRemove.Location = New System.Drawing.Point(320, 126)
        Me.btnRemove.Name = "btnRemove"
        Me.btnRemove.Size = New System.Drawing.Size(25, 25)
        Me.btnRemove.TabIndex = 11
        Me.btnRemove.Text = "3"
        Me.btnRemove.UseVisualStyleBackColor = True
        '
        'btnAdd
        '
        Me.btnAdd.Font = New System.Drawing.Font("Webdings", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.btnAdd.Location = New System.Drawing.Point(320, 95)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(25, 25)
        Me.btnAdd.TabIndex = 10
        Me.btnAdd.Text = "4"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'grbConditionList
        '
        Me.grbConditionList.Controls.Add(Me.lstCondition)
        Me.grbConditionList.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grbConditionList.Location = New System.Drawing.Point(365, 81)
        Me.grbConditionList.Name = "grbConditionList"
        Me.grbConditionList.Size = New System.Drawing.Size(322, 110)
        Me.grbConditionList.TabIndex = 4
        Me.grbConditionList.TabStop = False
        Me.grbConditionList.Text = "grbConditionList"
        '
        'lstCondition
        '
        Me.lstCondition.CheckOnClick = True
        Me.lstCondition.FormattingEnabled = True
        Me.lstCondition.Location = New System.Drawing.Point(12, 19)
        Me.lstCondition.Name = "lstCondition"
        Me.lstCondition.Size = New System.Drawing.Size(295, 79)
        Me.lstCondition.TabIndex = 0
        '
        'cboOperator
        '
        Me.cboOperator.DisplayMember = "DISPLAY"
        Me.cboOperator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboOperator.FormattingEnabled = True
        Me.cboOperator.Location = New System.Drawing.Point(104, 48)
        Me.cboOperator.Name = "cboOperator"
        Me.cboOperator.Size = New System.Drawing.Size(179, 21)
        Me.cboOperator.TabIndex = 7
        Me.cboOperator.ValueMember = "VALUE"
        '
        'grbCondition
        '
        Me.grbCondition.Controls.Add(Me.cboOperator)
        Me.grbCondition.Controls.Add(Me.cboField)
        Me.grbCondition.Controls.Add(Me.txtValue)
        Me.grbCondition.Controls.Add(Me.lbValue)
        Me.grbCondition.Controls.Add(Me.lbOperator)
        Me.grbCondition.Controls.Add(Me.lbField)
        Me.grbCondition.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grbCondition.Location = New System.Drawing.Point(9, 81)
        Me.grbCondition.Name = "grbCondition"
        Me.grbCondition.Size = New System.Drawing.Size(294, 110)
        Me.grbCondition.TabIndex = 3
        Me.grbCondition.TabStop = False
        Me.grbCondition.Text = "grbCondition"
        '
        'cboField
        '
        Me.cboField.DisplayMember = "DISPLAY"
        Me.cboField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboField.FormattingEnabled = True
        Me.cboField.Location = New System.Drawing.Point(104, 21)
        Me.cboField.Name = "cboField"
        Me.cboField.Size = New System.Drawing.Size(180, 21)
        Me.cboField.TabIndex = 6
        Me.cboField.ValueMember = "VALUE"
        '
        'txtValue
        '
        Me.txtValue.Location = New System.Drawing.Point(104, 76)
        Me.txtValue.Name = "txtValue"
        Me.txtValue.Size = New System.Drawing.Size(179, 20)
        Me.txtValue.TabIndex = 5
        '
        'lbValue
        '
        Me.lbValue.AutoSize = True
        Me.lbValue.Location = New System.Drawing.Point(12, 80)
        Me.lbValue.Name = "lbValue"
        Me.lbValue.Size = New System.Drawing.Size(42, 13)
        Me.lbValue.TabIndex = 2
        Me.lbValue.Text = "lbValue"
        '
        'lbOperator
        '
        Me.lbOperator.AutoSize = True
        Me.lbOperator.Location = New System.Drawing.Point(12, 51)
        Me.lbOperator.Name = "lbOperator"
        Me.lbOperator.Size = New System.Drawing.Size(56, 13)
        Me.lbOperator.TabIndex = 1
        Me.lbOperator.Text = "lbOperator"
        '
        'lbField
        '
        Me.lbField.AutoSize = True
        Me.lbField.Location = New System.Drawing.Point(12, 22)
        Me.lbField.Name = "lbField"
        Me.lbField.Size = New System.Drawing.Size(37, 13)
        Me.lbField.TabIndex = 0
        Me.lbField.Text = "lbField"
        '
        'grbSearchResult
        '
        Me.grbSearchResult.Controls.Add(Me.pnlSearchResult)
        Me.grbSearchResult.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grbSearchResult.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grbSearchResult.Location = New System.Drawing.Point(0, 201)
        Me.grbSearchResult.Name = "grbSearchResult"
        Me.grbSearchResult.Size = New System.Drawing.Size(804, 326)
        Me.grbSearchResult.TabIndex = 21
        Me.grbSearchResult.TabStop = False
        Me.grbSearchResult.Text = "grbSearchResult"
        '
        'pnlSearchResult
        '
        Me.pnlSearchResult.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlSearchResult.Location = New System.Drawing.Point(3, 16)
        Me.pnlSearchResult.Name = "pnlSearchResult"
        Me.pnlSearchResult.Size = New System.Drawing.Size(798, 307)
        Me.pnlSearchResult.TabIndex = 0
        '
        'grbSearchFilter
        '
        Me.grbSearchFilter.Controls.Add(Me.btnNext)
        Me.grbSearchFilter.Controls.Add(Me.btnPre)
        Me.grbSearchFilter.Controls.Add(Me.txtNum)
        Me.grbSearchFilter.Controls.Add(Me.lblNum)
        Me.grbSearchFilter.Controls.Add(Me.btnLoad)
        Me.grbSearchFilter.Controls.Add(Me.grpFileInfo)
        Me.grbSearchFilter.Controls.Add(Me.btnSearch)
        Me.grbSearchFilter.Controls.Add(Me.btnRemoveAll)
        Me.grbSearchFilter.Controls.Add(Me.btnRemove)
        Me.grbSearchFilter.Controls.Add(Me.btnAdd)
        Me.grbSearchFilter.Controls.Add(Me.grbConditionList)
        Me.grbSearchFilter.Controls.Add(Me.grbCondition)
        Me.grbSearchFilter.Dock = System.Windows.Forms.DockStyle.Top
        Me.grbSearchFilter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grbSearchFilter.Location = New System.Drawing.Point(0, 0)
        Me.grbSearchFilter.Name = "grbSearchFilter"
        Me.grbSearchFilter.Size = New System.Drawing.Size(804, 201)
        Me.grbSearchFilter.TabIndex = 20
        Me.grbSearchFilter.TabStop = False
        Me.grbSearchFilter.Text = "grbSearchFilter"
        '
        'btnNext
        '
        Me.btnNext.Font = New System.Drawing.Font("Webdings", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.btnNext.Location = New System.Drawing.Point(751, 129)
        Me.btnNext.Name = "btnNext"
        Me.btnNext.Size = New System.Drawing.Size(46, 29)
        Me.btnNext.TabIndex = 29
        Me.btnNext.Text = "4"
        Me.btnNext.UseVisualStyleBackColor = True
        '
        'btnPre
        '
        Me.btnPre.Font = New System.Drawing.Font("Webdings", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.btnPre.Location = New System.Drawing.Point(703, 129)
        Me.btnPre.Name = "btnPre"
        Me.btnPre.Size = New System.Drawing.Size(42, 29)
        Me.btnPre.TabIndex = 28
        Me.btnPre.Text = "3"
        Me.btnPre.UseVisualStyleBackColor = True
        '
        'txtNum
        '
        Me.txtNum.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNum.Location = New System.Drawing.Point(703, 103)
        Me.txtNum.Name = "txtNum"
        Me.txtNum.Size = New System.Drawing.Size(94, 20)
        Me.txtNum.TabIndex = 27
        Me.txtNum.Text = "100"
        '
        'lblNum
        '
        Me.lblNum.AutoSize = True
        Me.lblNum.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNum.Location = New System.Drawing.Point(700, 81)
        Me.lblNum.Name = "lblNum"
        Me.lblNum.Size = New System.Drawing.Size(39, 13)
        Me.lblNum.TabIndex = 26
        Me.lblNum.Text = "lblNum"
        '
        'btnLoad
        '
        Me.btnLoad.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLoad.Location = New System.Drawing.Point(703, 35)
        Me.btnLoad.Name = "btnLoad"
        Me.btnLoad.Size = New System.Drawing.Size(94, 28)
        Me.btnLoad.TabIndex = 25
        Me.btnLoad.Text = "btnLoad"
        Me.btnLoad.UseVisualStyleBackColor = True
        '
        'grpFileInfo
        '
        Me.grpFileInfo.Controls.Add(Me.btnBrown)
        Me.grpFileInfo.Controls.Add(Me.txtPath)
        Me.grpFileInfo.Controls.Add(Me.lblFilePath)
        Me.grpFileInfo.Controls.Add(Me.cboFileType)
        Me.grpFileInfo.Controls.Add(Me.lblFileType)
        Me.grpFileInfo.Location = New System.Drawing.Point(9, 19)
        Me.grpFileInfo.Name = "grpFileInfo"
        Me.grpFileInfo.Size = New System.Drawing.Size(678, 56)
        Me.grpFileInfo.TabIndex = 24
        Me.grpFileInfo.TabStop = False
        Me.grpFileInfo.Text = "grpFileInfo"
        '
        'btnBrown
        '
        Me.btnBrown.Location = New System.Drawing.Point(641, 19)
        Me.btnBrown.Name = "btnBrown"
        Me.btnBrown.Size = New System.Drawing.Size(30, 23)
        Me.btnBrown.TabIndex = 4
        Me.btnBrown.Tag = "btnBrown"
        Me.btnBrown.Text = "..."
        Me.btnBrown.UseVisualStyleBackColor = True
        '
        'txtPath
        '
        Me.txtPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPath.Location = New System.Drawing.Point(356, 20)
        Me.txtPath.Name = "txtPath"
        Me.txtPath.Size = New System.Drawing.Size(279, 20)
        Me.txtPath.TabIndex = 3
        '
        'lblFilePath
        '
        Me.lblFilePath.AutoSize = True
        Me.lblFilePath.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFilePath.Location = New System.Drawing.Point(247, 23)
        Me.lblFilePath.Name = "lblFilePath"
        Me.lblFilePath.Size = New System.Drawing.Size(55, 13)
        Me.lblFilePath.TabIndex = 2
        Me.lblFilePath.Tag = "lblFilePath"
        Me.lblFilePath.Text = "lblFilePath"
        '
        'cboFileType
        '
        Me.cboFileType.DisplayMember = "DISPLAY"
        Me.cboFileType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboFileType.FormattingEnabled = True
        Me.cboFileType.Location = New System.Drawing.Point(87, 20)
        Me.cboFileType.Name = "cboFileType"
        Me.cboFileType.Size = New System.Drawing.Size(154, 21)
        Me.cboFileType.TabIndex = 1
        Me.cboFileType.ValueMember = "VALUE"
        '
        'lblFileType
        '
        Me.lblFileType.AutoSize = True
        Me.lblFileType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFileType.Location = New System.Drawing.Point(12, 25)
        Me.lblFileType.Name = "lblFileType"
        Me.lblFileType.Size = New System.Drawing.Size(57, 13)
        Me.lblFileType.TabIndex = 0
        Me.lblFileType.Tag = "lblFileType"
        Me.lblFileType.Text = "lblFileType"
        '
        'frmInQueryImpFile
        '
        Me.ClientSize = New System.Drawing.Size(804, 527)
        Me.Controls.Add(Me.grbSearchResult)
        Me.Controls.Add(Me.grbSearchFilter)
        Me.KeyPreview = True
        Me.Name = "frmInQueryImpFile"
        Me.grbConditionList.ResumeLayout(False)
        Me.grbCondition.ResumeLayout(False)
        Me.grbCondition.PerformLayout()
        Me.grbSearchResult.ResumeLayout(False)
        Me.grbSearchFilter.ResumeLayout(False)
        Me.grbSearchFilter.PerformLayout()
        Me.grpFileInfo.ResumeLayout(False)
        Me.grpFileInfo.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
#End Region

#Region " Khai báo hằng, biến "

    Const c_ResourceManager = "Sats.AppCore.frmSearch_"
    Dim mv_dblTR_QTTY As Double = 0
    Protected WithEvents SearchGrid As GridEx
    Protected WithEvents SearchCell As Xceed.Grid.Cell
    Public mv_strSearchFilter As String
    Public hFilter As New Hashtable
    Private hLstTable As New Hashtable
    'Public mv_frmTransactScreen As frmTransact

    'Khai bao cac bien cho khop lenh bang tay
    Public mv_strCONFIRM_NO As String = String.Empty
    Public mv_strCUSTODYCD As String = String.Empty
    Public mv_strB_CUSTODYCD As String = String.Empty
    Public mv_strS_CUSTODYCD As String = String.Empty
    Public mv_strBORS As String = String.Empty
    Public mv_strSEC_CODE As String = String.Empty
    Public mv_intQUANTITY As Integer = 0
    Public mv_intB_QUANTITY As Integer = 0
    Public mv_intS_QUANTITY As Integer = 0
    Public mv_dblPRICE As Double = 0
    Public mv_strMATCH_DATE As String = String.Empty
    Public v_strS_ACCOUNT_NO As String = String.Empty
    Public v_strB_ACCOUNT_NO As String = String.Empty
    Public v_strS_ORDER_NO As String = String.Empty
    Public v_strB_ORDER_NO As String = String.Empty

    Private mv_strTableName As String
    Private mv_strCaption As String
    Private mv_strEnCaption As String
    Private mv_strKeyColumn As String
    Private mv_strKeyFieldType As String
    Private mv_strRefColumn As String
    Private mv_strRefFieldType As String
    Private mv_strCmdSql As String
    Private mv_strCmdSqlTemp As String

    Private mv_strTLTXCD As String
    Private mv_strSrOderByCmd As String
    Private mv_strObjName As String
    Private mv_strFormName As String
    Private mv_intSearchNum As Integer
    Private mv_strModuleCode As String
    Private mv_strIsLocalSearch As String
    Private mv_blnSearchOnInit As Boolean
    Private mv_strAuthCode As String
    Private mv_strAuthString As String
    Private mv_strIsLookup As String = "N"
    Private mv_strReturnValue As String
    Private mv_strRefValue As String
    Private mv_strReturnData As String
    Private mv_strXMLData As String
    Private mv_intDblGrid As Integer = 0
    Private mv_strIpAddress As String
    Private mv_strWsName As String

    Private mv_arrSrFieldOperator() As String                   'Danh sách các toán tử đi?u kiện
    Private mv_arrSrOperator() As String                        'Mảng các toán tử đi?u kiện
    Private mv_arrSrSQLRef() As String                          'Câu lệnh SQL liên quan
    Private mv_arrSrFieldType() As String                       'Loại dữ liệu của trư?ng
    Private mv_arrSrFieldSrch() As String                       'Tên các trư?ng làm tiêu chí để tìm kiếm
    Private mv_arrSrFieldDisp() As String                       'Tên các trư?ng sẽ hiển thị trên Combo
    Private mv_arrSrFieldMask() As String                       'Mặt nạ nhập dữ liệu
    Private mv_arrStFieldDefValue() As String                   'Giá trị mặc định
    Private mv_arrSrFieldFormat() As String                     'Định dạng dữ liệu
    Private mv_arrSrFieldDisplay() As String                    'Có hiển thị trên lưới không
    Private mv_arrSrLstTable() As String
    Private mv_arrSrFieldWidth() As Integer                     'Độ rộng hiển thị trên lưới

    Private mv_strLanguage As String
    Protected mv_ResourceManager As Resources.ResourceManager

    Private mv_strBranchId As String
    Private mv_strTellerId As String
    Private mv_strTellerType As String
    Private mv_intpage As Int32 = 1
    Private mv_rowpage As Int32 = 1
    Private mv_strBusDate As String
    Private mv_intRowCount As Int32 = 0
    Private mv_intNumRow As Integer = 100

    Public mv_enuEditFormResult As SaveButtonType

    Private mv_SelectedRow As Xceed.Grid.Row

    'Private mv_BDSDelivery As BDSChannel.BDSDelivery

    Private mv_strCondition As String
    Private mv_lngRowCount As Long
    Private mv_lngAllMember As Long
    Private mv_lngAllStock As Long
    Private mv_strMemberFilter As String
    Private mv_strStockFilter As String
    Private mv_oDataSet As DataSet

#End Region


#Region " Các thuộc tính của form "

    Public Property IpAddress() As String
        Get
            Return mv_strIpAddress
        End Get
        Set(ByVal Value As String)
            mv_strIpAddress = Value
        End Set
    End Property

    Public Property WsName() As String
        Get
            Return mv_strWsName
        End Get
        Set(ByVal Value As String)
            mv_strWsName = Value
        End Set
    End Property

    Public Property BusDate() As String
        Get
            Return mv_strBusDate
        End Get
        Set(ByVal Value As String)
            mv_strBusDate = Value
        End Set
    End Property

    Public Property RETURNDATA() As String
        Get
            Return mv_strReturnData
        End Get
        Set(ByVal Value As String)
            mv_strReturnData = Value
        End Set
    End Property

    Public Property ReturnValue() As String
        Get
            Return mv_strReturnValue
        End Get
        Set(ByVal Value As String)
            mv_strReturnValue = Value
        End Set
    End Property

    Public Property RefValue() As String
        Get
            Return mv_strRefValue
        End Get
        Set(ByVal Value As String)
            mv_strRefValue = Value
        End Set
    End Property

    Public Property IsLookup() As String
        Get
            Return mv_strIsLookup
        End Get
        Set(ByVal Value As String)
            mv_strIsLookup = Value
        End Set
    End Property

    Public Property TableName() As String
        Get
            Return mv_strTableName
        End Get
        Set(ByVal Value As String)
            mv_strTableName = Value
        End Set
    End Property

    Public Property UserLanguage() As String
        Get
            Return mv_strLanguage
        End Get
        Set(ByVal Value As String)
            mv_strLanguage = Value
        End Set
    End Property

    Public Property FormCaption() As String
        Get
            Return mv_strCaption
        End Get
        Set(ByVal Value As String)
            mv_strCaption = Value
            Me.Text = mv_strCaption
        End Set
    End Property

    Public Property KeyColumn() As String
        Get
            Return mv_strKeyColumn
        End Get
        Set(ByVal Value As String)
            mv_strKeyColumn = Value
        End Set
    End Property

    Public Property KeyFieldType() As String
        Get
            Return mv_strKeyFieldType
        End Get
        Set(ByVal Value As String)
            mv_strKeyFieldType = Value
        End Set
    End Property

    Public ReadOnly Property ObjectName() As String
        Get
            Return mv_strObjName
        End Get
    End Property

    Public ReadOnly Property MaintenanceFormName() As String
        Get
            Return mv_strFormName
        End Get
    End Property

    Public Property ModuleCode() As String
        Get
            Return mv_strModuleCode
        End Get
        Set(ByVal Value As String)
            mv_strModuleCode = Value
        End Set
    End Property

    Public Property IsLocalSearch() As String
        Get
            Return mv_strIsLocalSearch
        End Get
        Set(ByVal Value As String)
            mv_strIsLocalSearch = Value
        End Set
    End Property

    Public Property SearchOnInit() As Boolean
        Get
            Return mv_blnSearchOnInit
        End Get
        Set(ByVal Value As Boolean)
            mv_blnSearchOnInit = Value
        End Set
    End Property

    Public Property BranchId() As String
        Get
            Return mv_strBranchId
        End Get
        Set(ByVal Value As String)
            mv_strBranchId = Value
        End Set
    End Property

    Public Property TellerId() As String
        Get
            Return mv_strTellerId
        End Get
        Set(ByVal Value As String)
            mv_strTellerId = Value
        End Set
    End Property

    Public Property TellerType() As String
        Get
            Return mv_strTellerType
        End Get
        Set(ByVal Value As String)
            mv_strTellerType = Value
        End Set
    End Property

    Public ReadOnly Property ResourceManager() As Resources.ResourceManager
        Get
            Return mv_ResourceManager
        End Get
    End Property

    Public Property AuthCode() As String
        Get
            Return mv_strAuthCode
        End Get
        Set(ByVal Value As String)
            mv_strAuthCode = Value
        End Set
    End Property

    Public Property AuthString() As String
        Get
            Return mv_strAuthString
        End Get
        Set(ByVal Value As String)
            mv_strAuthString = Value
        End Set
    End Property
    Public Property CMDSQL() As String
        Get
            Return mv_strCmdSql
        End Get
        Set(ByVal Value As String)
            mv_strCmdSql = Value
        End Set
    End Property
    Public Property AllStock() As Long
        Get
            Return mv_lngAllStock
        End Get
        Set(ByVal Value As Long)
            mv_lngAllStock = Value
        End Set
    End Property
    Public Property AllMember() As Long
        Get
            Return mv_lngAllMember
        End Get
        Set(ByVal Value As Long)
            mv_lngAllMember = Value
        End Set
    End Property
    Public Property MemberFilter() As String
        Get
            Return mv_strMemberFilter
        End Get
        Set(ByVal Value As String)
            mv_strMemberFilter = Value
        End Set
    End Property
    Public Property StockFilter() As String
        Get
            Return mv_strStockFilter
        End Get
        Set(ByVal Value As String)
            mv_strStockFilter = Value
        End Set
    End Property
#End Region

#Region "Các sự kiện"

    Private Sub frmInQueryImpFile_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If Not mv_oDataSet Is Nothing Then
            mv_oDataSet.Dispose()
        End If
    End Sub

    Private Sub frmSearch_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        Dim CountR As Int32

        Select Case e.KeyCode
            Case Keys.F5
                OnSearch(IsLocalSearch, ModuleCode & "." & ObjectName, mv_intpage)
            Case Keys.F6
                If Not (SearchGrid Is Nothing) Then
                    If SearchGrid.Enabled And SearchGrid.Visible Then
                        SearchGrid.Focus()
                        If Not SearchGrid.CurrentRow Is Nothing Then
                            Dim dataRows As Xceed.Grid.Collections.ReadOnlyDataRowList = SearchGrid.GetSortedDataRows(True)
                            Dim firstTaggedDataRow As Xceed.Grid.DataRow = dataRows(0)
                            SearchGrid.CurrentRow = firstTaggedDataRow
                        End If
                    End If
                End If
            Case Keys.F7    'Prev
                mv_intpage = mv_intpage - 1
                If mv_intpage <= 0 Then
                    mv_intpage = 1
                End If
                OnSearch(IsLocalSearch, ModuleCode & "." & ObjectName, mv_intpage)
            Case Keys.F8    'Next
                'CountR = CountRow()
                CountR = mv_lngRowCount
                If CountR >= (mv_intpage + 1) * mv_rowpage Then
                    mv_intpage = mv_intpage + 1
                    OnSearch(IsLocalSearch, ModuleCode & "." & ObjectName, mv_intpage)
                End If
            Case Keys.Escape
                Me.Close()
            Case Keys.C
                If Keys.Control Then
                    If Not (SearchGrid.CurrentRow Is Nothing) Then
                        If Not (SearchGrid.CurrentRow Is SearchGrid.FixedFooterRows.Item(0)) Then
                            If mv_strKeyColumn Is Nothing Then
                                Clipboard.SetDataObject(SearchGrid.CurrentCell.Value)
                            Else
                                Clipboard.SetDataObject(CType(SearchGrid.CurrentRow, Xceed.Grid.DataRow).Cells(mv_strKeyColumn).Value)
                            End If
                        End If
                    End If
                End If
        End Select
    End Sub
    Private Sub frmSearch_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'mv_BDSDelivery = New BDSChannel.BDSDelivery
        'InitDialog()
        Combo_SelectedIndexChanged(cboField, e)
    End Sub

    Private Sub frmSearch_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        DoResizeForm()
    End Sub


    Private Sub Combo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try

            If (sender Is cboField) Then
                'Load các toán tử đi?u kiện
                AnalyzeOperator(mv_arrSrFieldOperator(cboField.SelectedIndex + 1), mv_arrSrOperator)
                cboOperator.Clears()
                For i As Integer = 1 To mv_arrSrOperator.Length
                    cboOperator.AddItems(mv_arrSrOperator(i - 1), mv_arrSrOperator(i - 1))
                Next

                If CStr(Me.cboOperator.SelectedValue).Equals("LIKE") Then
                    'Neu dieu kien tim kiem la like thi chuyen ve text box de bo dinh dang
                    NewTxtValue(mv_arrSrSQLRef(cboField.SelectedIndex + 1), mv_arrSrFieldType(cboField.SelectedIndex + 1), _
                                        String.Empty, mv_arrStFieldDefValue(cboField.SelectedIndex + 1), mv_arrSrFieldFormat(cboField.SelectedIndex + 1), mv_arrSrLstTable(cboField.SelectedIndex + 1))
                Else
                    NewTxtValue(mv_arrSrSQLRef(cboField.SelectedIndex + 1), mv_arrSrFieldType(cboField.SelectedIndex + 1), _
                                    mv_arrSrFieldMask(cboField.SelectedIndex + 1), mv_arrStFieldDefValue(cboField.SelectedIndex + 1), mv_arrSrFieldFormat(cboField.SelectedIndex + 1), mv_arrSrLstTable(cboField.SelectedIndex + 1))
                End If

            ElseIf (sender Is cboOperator) Then
                Try
                    If CStr(Me.cboOperator.SelectedValue).Equals("LIKE") Then
                        'Neu dieu kien tim kiem la like thi chuyen ve text box de bo dinh dang
                        NewTxtValue(mv_arrSrSQLRef(cboField.SelectedIndex + 1), mv_arrSrFieldType(cboField.SelectedIndex + 1), _
                                            String.Empty, mv_arrStFieldDefValue(cboField.SelectedIndex + 1), mv_arrSrFieldFormat(cboField.SelectedIndex + 1), mv_arrSrLstTable(cboField.SelectedIndex + 1))
                    Else
                        NewTxtValue(mv_arrSrSQLRef(cboField.SelectedIndex + 1), mv_arrSrFieldType(cboField.SelectedIndex + 1), _
                                        mv_arrSrFieldMask(cboField.SelectedIndex + 1), mv_arrStFieldDefValue(cboField.SelectedIndex + 1), mv_arrSrFieldFormat(cboField.SelectedIndex + 1), mv_arrSrLstTable(cboField.SelectedIndex + 1))
                    End If
                Catch ex As Exception
                    'Throw ex
                End Try

                'Khi dieu kien tim kiem la like thi bo dinh dang
            End If

        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                        & "Error code: System error!" & vbNewLine _
                        & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(m_BusLayer.AppLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Sub

    Private Sub Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim v_strValue, v_strValueDisplay As String
        'Dim v_objResult As Object
        'Dim v_strFilterTmp As String
        'Dim v_strSearchKey As String
        'Dim v_blnSearchKeyAdded As Boolean

        Try
            If (sender Is btnSearch) Then
                OnSearch(IsLocalSearch, ModuleCode & "." & ObjectName)
            ElseIf (sender Is btnAdd) Then
                AddSearchCriteria()
            ElseIf (sender Is btnRemove) Then
                RemoveSearchCriteria()
            ElseIf (sender Is btnRemoveAll) Then
                RemoveAllSearchCriterias()
            End If
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                         & "Error code: System error!" & vbNewLine _
                         & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(m_BusLayer.AppLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Sub


    Private Function CountRow() As Int32
        Try

            Dim v_xmlDocument As New Xml.XmlDocument
            Dim v_nodeList As Xml.XmlNodeList
            Dim v_int, v_intCount As Integer
            Dim v_intCOUNTROW As Int32
            Dim v_strFLDNAME, v_strVALUE As String
            'Dim v_ws As New BDSDelivery.BDSDelivery
            Dim v_strCmdInquiry As String = "select COUNT(*) COUNTROW from (" & mv_strCmdSqlTemp & ") WHERE 0=0"

            'Dim v_strObjMsg As String = BuildXMLObjMsg(, , , , gc_IsLocalMsg, gc_MsgTypeObj, "SA.ALLCODE", gc_ActionInquiry, v_strCmdInquiry)
            Dim v_strObjMsg As String = BuildXMLObjMsg(, , , , IsLocalSearch, gc_MsgTypeObj, ModuleCode & "." & ObjectName, _
                                          gc_ActionInquiry, v_strCmdInquiry)

            Proxy.Message(v_strObjMsg)

            v_xmlDocument.LoadXml(v_strObjMsg)
            v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")
            For v_intCount = 0 To v_nodeList.Count - 1
                For v_int = 0 To v_nodeList.Item(v_intCount).ChildNodes.Count - 1
                    With v_nodeList.Item(v_intCount).ChildNodes(v_int)
                        v_strFLDNAME = v_nodeList.Item(v_intCount).ChildNodes(v_int).Attributes.GetNamedItem("fldname").Value.Trim()
                        v_strVALUE = v_nodeList.Item(v_intCount).ChildNodes(v_int).Attributes.GetNamedItem("oldval").Value.Trim()

                        Select Case v_strFLDNAME
                            Case "COUNTROW"
                                v_intCOUNTROW = v_strVALUE
                        End Select
                    End With
                Next
            Next
            mv_intRowCount = v_intCOUNTROW
            Return v_intCOUNTROW
        Catch ex As Exception
            Throw ex

        End Try
    End Function


    Private Sub AddSearchCriteria()
        Try
            Dim v_strValue, v_strValueDisplay As String
            Dim v_objResult As Object
            Dim v_strFilterTmp As String
            Dim v_strFilterTmpUpper As String
            Dim v_strSearchKey As String
            Dim v_blnSearchKeyAdded As Boolean
            Dim v_strLstTable As String
            'Dim i1 As Int16
            v_strValueDisplay = Trim(txtValue.Text)
            v_strLstTable = Trim(txtValue.Tag)
            If mv_arrSrSQLRef(cboField.SelectedIndex + 1).Trim.Length > 0 Then
                'v_strValue = v_strValueDisplay
                v_strValue = CType(txtValue, ComboBoxEx).SelectedValue
            Else
                v_strValue = Trim(txtValue.Text.ToString)
            End If

            v_strValue = v_strValue.Replace("'", "''")

            If v_strValue <> String.Empty Then
                v_objResult = hFilter(mv_arrSrFieldDisp(cboField.SelectedIndex + 1) & " " _
                    & IIf(Trim(cboOperator.SelectedValue) = "LIKE", "%", "") & " " _
                    & IIf(mv_arrSrFieldType(cboField.SelectedIndex + 1) <> "N", "'", "") _
                    & v_strValueDisplay & IIf(Trim(mv_arrSrFieldType(cboField.SelectedIndex + 1)) <> "N", "'", ""))

                If (v_objResult Is Nothing) Then
                    v_blnSearchKeyAdded = False
                    v_strSearchKey = mv_arrSrFieldDisp(cboField.SelectedIndex + 1) & " " _
                        & cboOperator.SelectedValue & " " & IIf(mv_arrSrFieldType(cboField.SelectedIndex + 1) <> "N", "'", "") _
                        & v_strValueDisplay & IIf(mv_arrSrFieldType(cboField.SelectedIndex + 1) <> "N", "'", "")

                    For i As Integer = 0 To lstCondition.Items.Count - 1
                        If lstCondition.Items(i).ToString() = v_strSearchKey Then
                            v_blnSearchKeyAdded = True
                            Exit For
                        End If
                    Next

                    If Not v_blnSearchKeyAdded Then
                        If mv_arrSrFieldType(cboField.SelectedIndex + 1) = "D" Then
                            v_strFilterTmp = "TO_DATE("
                            v_strFilterTmp &= IIf(mv_arrSrSQLRef(cboField.SelectedIndex + 1).Trim.Length = 0, _
                                mv_arrSrFieldSrch(cboField.SelectedIndex + 1), _
                                mv_arrSrFieldSrch(cboField.SelectedIndex + 1)) & ",'dd/MM/yyyy')"
                        Else
                            v_strFilterTmp = ""
                            v_strFilterTmp &= IIf(mv_arrSrSQLRef(cboField.SelectedIndex + 1).Trim.Length = 0, _
                                mv_arrSrFieldSrch(cboField.SelectedIndex + 1), _
                                mv_arrSrFieldSrch(cboField.SelectedIndex + 1))
                        End If
                        v_strFilterTmpUpper = "" & v_strFilterTmp & ""
                        v_strFilterTmp &= " " & cboOperator.SelectedValue & " "
                        v_strFilterTmpUpper &= " " & cboOperator.SelectedValue & " "
                        Select Case mv_arrSrFieldType(cboField.SelectedIndex + 1)
                            Case "D"
                                v_strFilterTmp &= "TO_DATE('" & v_strValue & "', '" & gc_FORMAT_DATE & "')"
                            Case "N"

                                If IsNumeric(v_strValue) Then
                                    v_strFilterTmp &= CDbl(v_strValue)
                                Else
                                    Exit Sub
                                End If
                            Case "C"
                                v_strValue = Trim(Replace(v_strValue, ".", String.Empty))

                                If InStr(v_strValue, "%") > 0 Then
                                    v_strFilterTmpUpper &= "'" _
                                                  & IIf(Trim(cboOperator.SelectedValue) = "LIKE", "", "") & v_strValue _
                                                  & IIf(Trim(cboOperator.SelectedValue) = "LIKE", "", "") & "'"

                                Else
                                    If v_strValue = String.Empty Then
                                        v_strFilterTmpUpper = Replace(v_strFilterTmpUpper, "=", "")
                                        v_strFilterTmpUpper &= " IS NULL "
                                    Else
                                        v_strFilterTmpUpper &= "'" & IIf(Trim(cboOperator.SelectedValue) = "LIKE", "%", "") & v_strValue _
                                               & IIf(Trim(cboOperator.SelectedValue) = "LIKE", "%", "") & "'"
                                    End If
                                End If
                                v_strFilterTmp = String.Empty
                                v_strFilterTmp = v_strFilterTmpUpper
                        End Select
                        lstCondition.Items.Add(v_strSearchKey, True)
                        hFilter.Add(v_strSearchKey, v_strFilterTmp)
                    End If
                End If
            End If
            Me.btnSearch.Select()
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                        & "Error code: System error!" & vbNewLine _
                        & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(m_BusLayer.AppLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Sub

    Private Sub RemoveSearchCriteria()
        Try
            Dim v_objResult As Object

            If lstCondition.SelectedIndex <> -1 Then
                v_objResult = hFilter(lstCondition.Items(lstCondition.SelectedIndex).ToString())

                If Not (v_objResult Is Nothing) Then
                    hFilter.Remove(lstCondition.Items(lstCondition.SelectedIndex).ToString())
                    hLstTable.Remove(lstCondition.Items(lstCondition.SelectedIndex).ToString())
                    lstCondition.Items.RemoveAt(lstCondition.SelectedIndex)
                End If
            End If
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                       & "Error code: System error!" & vbNewLine _
                       & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(m_BusLayer.AppLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Sub

    Private Sub RemoveAllSearchCriterias()
        Try
            Dim v_objResult As Object
            Dim v_strValueDisplay As String

            For i As Integer = 0 To lstCondition.Items.Count - 1
                v_objResult = hFilter(lstCondition.Items(i).ToString())

                If Not (v_objResult Is Nothing) Then
                    v_strValueDisplay = lstCondition.Items(i).ToString()
                    hFilter.Remove(v_strValueDisplay)
                    hLstTable.Remove(v_strValueDisplay)
                End If
            Next
            lstCondition.Items.Clear()
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                        & "Error code: System error!" & vbNewLine _
                        & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(m_BusLayer.AppLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Sub
#End Region

#Region "Over method"
    Public Overridable Sub InitDialog()
        'Khởi tạo kích thước form và load resource
        mv_ResourceManager = New Resources.ResourceManager(c_ResourceManager & UserLanguage, System.Reflection.Assembly.GetExecutingAssembly())
        LoadResource(Me)
        DoResizeForm()
        'mv_BDSDelivery = New BDSChannel.BDSDelivery

        'Set click event for buttons
        AddHandler btnSearch.Click, AddressOf Button_Click
        AddHandler btnAdd.Click, AddressOf Button_Click
        AddHandler btnRemove.Click, AddressOf Button_Click
        AddHandler btnRemoveAll.Click, AddressOf Button_Click

        'Set enable status for toolbar buttons depend on AuthString string
        'If TellerId <> "0001" Then
        'Set enable status for toolbar buttons and other buttons depend on AuthCode string
        btnSearch.Visible = (Mid(AuthCode, 5, 1) = "Y")

        'Set selected index changed event for ComboBoxes
        AddHandler cboField.SelectedIndexChanged, AddressOf Combo_SelectedIndexChanged
        AddHandler cboOperator.SelectedIndexChanged, AddressOf Combo_SelectedIndexChanged

        cboFileType.Clears()

        cboFileType.AddItems("Cổ phiếu/Trái phiếu(HN)", 1)
        cboFileType.AddItems("Trái phiếu chuyên biệt(HN)", 2)
        cboFileType.AddItems("GD lô lớn(HCM)", 3)
        cboFileType.AddItems("DG lô thường(HCM)", 4)

        'btnView.Enabled = (Mid(AuthString, 1, 1) = "Y")
        btnLoad.Enabled = (Mid(AuthString, 2, 1) = "Y")
        'btnEdit.Enabled = (Mid(AuthString, 3, 1) = "Y")
        'btnDelete.Enabled = (Mid(AuthString, 4, 1) = "Y")
        'btnExecute.Enabled = (Mid(AuthString, 5, 1) = "Y")
        btnSearch.Enabled = (Mid(AuthString, 5, 1) = "Y")

        InitDataGrid(mv_strTableName)
        btnSearch.Enabled = False
        btnLoad.Enabled = False
    End Sub

    Private Sub InitDataGrid(ByVal pv_strObjName As String)
        Try

            SearchGrid = New GridEx(pv_strObjName, c_ResourceManager & UserLanguage, TellerId)
            Me.pnlSearchResult.Controls.Add(SearchGrid)
            SearchGrid.Dock = DockStyle.Fill

            'Thiết lập các giá trị ban đầu cho các đi?u kiện tìm kiếm
            Dim v_strCmdInquiry As String = "SELECT * FROM V_SEARCHCD WHERE 0=0 "
            Dim v_strClause As String = " UPPER(SEARCHCODE) = '" & pv_strObjName & "' ORDER BY POSITION"
            Dim v_strObjMsg As String = BuildXMLObjMsg(, , , , gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SY_SEARCHFLD, gc_ActionInquiry, v_strCmdInquiry, v_strClause, )

            'Dim v_ws As New BDSDelivery.BDSDelivery
            Dim v_lngError As Long = Proxy.Message(v_strObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(m_BusLayer.AppLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Exit Sub
            End If
            'PrepareSearchParams(UserLanguage, v_strObjMsg, mv_strCaption, mv_strEnCaption, mv_strCmdSql, pv_strObjName, mv_strFormName, _
            '    mv_arrSrFieldSrch, mv_arrSrFieldDisp, mv_arrSrFieldType, mv_arrSrFieldMask, mv_arrStFieldDefValue, _
            '    mv_arrSrFieldOperator, mv_arrSrFieldFormat, mv_arrSrFieldDisplay, mv_arrSrFieldWidth, _
            '    mv_arrSrSQLRef, mv_strKeyColumn, mv_strKeyFieldType, mv_intSearchNum, mv_strRefColumn, mv_strRefFieldType, mv_strSrOderByCmd, mv_strTLTXCD)

            cboField.Clears()

            For i As Integer = 1 To mv_intSearchNum
                cboField.AddItems(mv_arrSrFieldDisp(i), mv_arrSrFieldSrch(i))
            Next

            'Update form caption
            If UserLanguage <> "EN" Then
                FormCaption = mv_strCaption
            Else
                FormCaption = mv_strEnCaption
            End If
            Me.Text = FormCaption

        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                       & "Error code: System error!" & vbNewLine _
                       & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(m_BusLayer.AppLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Sub

    Protected Overridable Function OnSearch(Optional ByVal pv_strIsLocal As String = "", Optional ByVal pv_strModule As String = "", Optional ByVal page As Int32 = 1) As Int32
        Dim i As Integer
        Dim v_xColumn As Xceed.Grid.Column, v_strFLDNAME As String
        Dim v_thread As New Threading.Thread(AddressOf ShowFormProcess)
        v_thread.Start()
        Application.DoEvents()

        Try
            'Disable search button
            Me.btnSearch.Enabled = False

            Cursor = Cursors.WaitCursor

            'Update status bar
            mv_strSearchFilter = String.Empty

            Dim v_arrTblName() As String
            Dim v_int, v_intCount As Integer
            Dim v_strTemp As String
            mv_strCondition = ""
            Dim v_hTemp As New Hashtable
            Dim v_hTable As New Hashtable

            v_intCount = 0

            For i = 0 To lstCondition.Items.Count - 1
                If lstCondition.GetItemChecked(i) Then
                    mv_strSearchFilter &= " AND " & hFilter(lstCondition.Items(i).ToString())
                End If
            Next i

            Dim v_oSQL As New Sats.SQLEngine.SelectCommand(mv_oDataSet)
            Dim v_ds As New DataSet
            Dim v_dt As DataTable
            Dim v_strSQL As String

            mv_intpage = page

            Dim v_intFrom, v_intTo As Int32

            v_intTo = page * mv_intNumRow
            v_intFrom = v_intTo + 1 - mv_intNumRow

            v_strSQL = "SELECT * FROM IMPFILE WHERE COL0>=" & v_intFrom & " AND COL0<=" & v_intTo & " ORDER BY COL1"
            v_dt = v_oSQL.Execute(v_strSQL)

            v_ds.Tables.Add(v_dt)
            v_ds.Tables(0).TableName = "IMPFILE"
            v_oSQL = New Sats.SQLEngine.SelectCommand(v_ds)
            v_strSQL = "SELECT * FROM IMPFILE"
            If mv_strSearchFilter <> "" Then
                v_strSQL &= " WHERE "
                v_strSQL &= Mid(mv_strSearchFilter, 5, Len(mv_strSearchFilter))
            End If
            v_strSQL &= " ORDER BY COL0"
            v_dt = v_oSQL.Execute(v_strSQL)

            FillDataGridFromDataTable(v_dt, v_intFrom, v_intTo)

            'Update mouse pointer
            Cursor = Cursors.Default

            'Enable search button
            Me.btnSearch.Enabled = True
            'If Mid(AuthCode, 6, 1) = "Y" Then
            '    Me.btnSearch.Enabled = (mv_intRowCount <> 0)
            'End If
            v_ds.Dispose()
            v_dt.Dispose()
            GC.Collect()
        Catch ex As Exception
            Me.btnSearch.Enabled = True
            Cursor = Cursors.Default
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                       & "Error code: System error!" & vbNewLine _
                       & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(m_BusLayer.AppLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        Finally
            v_thread.Abort()
        End Try
    End Function

#End Region

#Region "Các method khác"
    Public Sub New(ByVal pv_strLanguage As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        UserLanguage = pv_strLanguage
        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Protected Overridable Sub LoadResource(ByRef pv_ctrl As Windows.Forms.Control)
        Dim v_ctrl As Windows.Forms.Control

        For Each v_ctrl In pv_ctrl.Controls
            If TypeOf (v_ctrl) Is Label Then
                CType(v_ctrl, Label).Text = mv_ResourceManager.GetString(v_ctrl.Name)
            ElseIf TypeOf (v_ctrl) Is GroupBox Then
                CType(v_ctrl, GroupBox).Text = mv_ResourceManager.GetString(v_ctrl.Name)
                LoadResource(v_ctrl)
            ElseIf TypeOf (v_ctrl) Is Button Then
                CType(v_ctrl, Button).Text = mv_ResourceManager.GetString(v_ctrl.Name)
            End If
        Next
    End Sub

    'Thay đổi kích thước control khi form thay đổi size
    Private Sub DoResizeForm()
        grbSearchFilter.Width = Me.Width - 18
        btnSearch.Left = grbSearchFilter.Width - btnSearch.Width - 9
        grbConditionList.Width = grbSearchFilter.Width - btnSearch.Width - grbConditionList.Left - 18
        lstCondition.Width = grbConditionList.Width - 16

        grbSearchResult.Width = grbSearchFilter.Width
        pnlSearchResult.Width = grbSearchResult.Width - 16
        grbSearchResult.Height = Me.Height - grbSearchResult.Top - 8
        pnlSearchResult.Height = grbSearchResult.Height - 32
        lblNum.Left = btnSearch.Left
        txtNum.Left = btnSearch.Left
        btnPre.Left = btnSearch.Left
        btnNext.Left = btnSearch.Left + btnSearch.Width - btnNext.Width
    End Sub

#End Region

    Private Sub LoadLastSearch()
        Try
            Dim v_strUserProfiles As String = Application.LocalUserAppDataPath & "\" & Me.BranchId & Me.TellerId & ".xml"
            Dim v_strSection As String = Me.ModuleCode & "." & Me.ObjectName
            Dim v_xmlDocument As New Xml.XmlDocument, v_nodetxData As Xml.XmlNode, v_nodeEntry As Xml.XmlNode
            Dim v_strObjMsg As String = String.Empty

            If Len(Dir(v_strUserProfiles)) = 0 Then
                'Tạo tệp tin UserProfiles
                v_strObjMsg = BuildXMLObjMsg(, , , , gc_IsLocalMsg, gc_MsgTypeObj, "USER_PROFILES:=" & Me.BranchId & "." & Me.TellerId, , , , )
                v_xmlDocument.LoadXml(v_strObjMsg)
                v_xmlDocument.Save(v_strUserProfiles)
            Else
                'Nạp tệp tin UserProfiles
                v_xmlDocument.Load(v_strUserProfiles)
                v_strObjMsg = v_xmlDocument.InnerXml
                v_nodetxData = v_xmlDocument.SelectSingleNode("ObjectMessage/ObjData[@OBJNAME='" & v_strSection & "']")

                If Not v_nodetxData Is Nothing Then
                    For i As Integer = 0 To v_nodetxData.ChildNodes.Count - 1
                        v_nodeEntry = v_nodetxData.ChildNodes(i)

                        lstCondition.Items.Add(v_nodeEntry.Attributes("DISPLAY").Value.ToString(), (v_nodeEntry.Attributes("CHECKED").Value.ToString() = "Y"))
                        hFilter.Add(v_nodeEntry.Attributes("DISPLAY").Value.ToString(), v_nodeEntry.Attributes("VALUE").Value.ToString())
                    Next i
                End If
            End If
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                       & "Error code: System error!" & vbNewLine _
                       & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(m_BusLayer.AppLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Sub

    Private Sub NewTxtValue(ByVal pv_strSqlRef As String, ByVal pv_strFldType As String, _
                           ByVal pv_strFldMask As String, ByVal pv_strDefValue As String, ByVal pv_strFldFormat As String, Optional ByVal pv_strLstTable As String = "")
        txtValue.Dispose()
        Try


            If pv_strSqlRef.Trim.Length < 1 Then
                If Trim$(mv_arrSrFieldType(cboField.SelectedIndex + 1)) = "D" Then
                    Me.txtValue = New DateTimePicker
                    CType(Me.txtValue, DateTimePicker).Format = DateTimePickerFormat.Custom
                    CType(Me.txtValue, DateTimePicker).CustomFormat = gc_FORMAT_DATE
                Else
                    If (pv_strFldMask.Trim.Length = 0) Then
                        Me.txtValue = New System.Windows.Forms.TextBox

                        Select Case pv_strFldType.Trim
                            Case "C"
                                CType(Me.txtValue, TextBox).TextAlign = HorizontalAlignment.Left
                            Case "N"
                                CType(Me.txtValue, TextBox).TextAlign = HorizontalAlignment.Right
                        End Select
                    Else
                        Me.txtValue = New FlexMaskEditBox
                        CType(Me.txtValue, FlexMaskEditBox).Mask = pv_strFldMask.Trim

                        If (pv_strFldFormat.Trim.Length > 0) Then
                            CType(Me.txtValue, FlexMaskEditBox).PromptChar = pv_strFldFormat.Trim
                        End If

                        Select Case pv_strFldType.Trim
                            Case "C"
                                CType(Me.txtValue, FlexMaskEditBox).TextAlign = HorizontalAlignment.Left
                            Case "N"
                                CType(Me.txtValue, FlexMaskEditBox).TextAlign = HorizontalAlignment.Right
                        End Select
                    End If
                End If
            Else
                Me.txtValue = New ComboBoxEx
            End If
            Me.grbCondition.Controls.Add(Me.txtValue)
            '
            'txtValue
            '
            Me.txtValue.Enabled = True
            Me.txtValue.Name = "txtValue"
            Me.txtValue.Width = cboOperator.Width
            Me.txtValue.Height = cboOperator.Height
            Me.txtValue.Left = cboOperator.Left
            Me.txtValue.Top = cboOperator.Top + cboOperator.Height + (cboOperator.Top - cboField.Top - cboField.Height)
            Me.txtValue.TabIndex = cboOperator.TabIndex + 1
            If pv_strDefValue <> "" Then
                Me.txtValue.Text = pv_strDefValue
            Else
                Me.txtValue.Text = String.Empty
            End If
            Me.txtValue.Visible = True

            'Load CSDL
            If pv_strSqlRef.Trim.Length > 0 Then
                Dim v_strObjMsg As String = BuildXMLObjMsg(, , , , gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SY_SEARCHFLD, gc_ActionInquiry, pv_strSqlRef)
                'Dim v_ws As New BDSDelivery.BDSDelivery
                Proxy.Message(v_strObjMsg)

                FillComboEx(v_strObjMsg, txtValue)
            End If
            txtValue.Tag = pv_strLstTable
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                       & "Error code: System error!" & vbNewLine _
                       & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(m_BusLayer.AppLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Sub

    Public Sub ShowAction()
        OnSearch(IsLocalSearch, ModuleCode & "." & ObjectName, mv_intpage)
    End Sub

    Private Sub txtValue_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtValue.KeyUp
        Select Case e.KeyCode
            Case Keys.Enter
                AddSearchCriteria()
        End Select
    End Sub

    Private Sub grbSearchResult_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles grbSearchResult.DoubleClick
        grbSearchFilter.Visible = Not grbSearchFilter.Visible
    End Sub

    Private Sub ShowFormProcess()
        Dim v_frm As New frmProcess
        v_frm.ShowDialog()
    End Sub

    Private Sub btnBrown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrown.Click
        Dim dgl As New OpenFileDialog

        If cboFileType.SelectedValue = 1 Or cboFileType.SelectedValue = 2 Then
            dgl.Filter = "XML(*.xml)|*.xml"
        Else
            dgl.Filter = "Text(*.txt)|*.txt"
        End If

        If dgl.ShowDialog = Windows.Forms.DialogResult.OK Then
            txtPath.Text = dgl.FileName
            btnLoad.Enabled = True
        Else
            txtPath.Text = ""
            btnLoad.Enabled = False
        End If
    End Sub

    Private Sub btnLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoad.Click
        Dim v_dt As DataTable

        If Not mv_oDataSet Is Nothing Then
            mv_oDataSet.Dispose()
        End If
        Select Case cboFileType.SelectedValue
            Case 1, 2
                mv_oDataSet = ReadFileFullXML(txtPath.Text)
                'InitDataGrid("IMPFILE")
            Case 3
                mv_oDataSet = ReadFileFullASTPT(txtPath.Text)
                'InitDataGrid("IMPFILE2")
            Case 4
                mv_oDataSet = ReadFileFullASTDT(txtPath.Text)
                'InitDataGrid("IMPFILE1")
        End Select

        If mv_oDataSet.Tables(0).Rows.Count < mv_intNumRow Then
            btnPre.Enabled = False
            btnNext.Enabled = False
        Else
            btnPre.Enabled = False
            btnNext.Enabled = True
        End If
        mv_lngRowCount = mv_oDataSet.Tables(0).Rows.Count
        If mv_lngRowCount > 0 Then
            btnSearch.Enabled = True
        Else
            btnSearch.Enabled = False
        End If
        OnSearch()
    End Sub

    Private Sub FillDataGridFromDataTable(ByVal pv_oDataTable As DataTable, Optional ByVal pv_intFormRow As Integer = 1, Optional ByVal pv_intToRow As Integer = 100)

        Dim v_strValue As String

        SearchGrid.DataRows.Clear()
        SearchGrid.BeginInit()

        For i As Integer = 0 To pv_oDataTable.Rows.Count - 1
            Dim v_xDataRow As Xceed.Grid.DataRow = SearchGrid.DataRows.AddNew()
            For j As Integer = 1 To pv_oDataTable.Columns.Count - 1
                v_strValue = pv_oDataTable(i)(j)
                With SearchGrid.Columns(j)
                    If .FieldName.ToUpper = pv_oDataTable.Columns(j).ColumnName.ToUpper Then
                        Select Case .DataType.Name
                            Case GetType(System.String).Name
                                v_xDataRow.Cells(.FieldName).Value = IIf(v_strValue Is DBNull.Value, "", CStr(v_strValue))
                            Case GetType(System.Decimal).Name
                                If v_strValue = "" Then
                                    v_strValue = 0
                                End If
                                v_xDataRow.Cells(.FieldName).Value = IIf(v_strValue Is DBNull.Value, 0, CDec(v_strValue))
                            Case GetType(Integer).Name
                                v_xDataRow.Cells(.FieldName).Value = IIf(v_strValue Is DBNull.Value, 0, CInt(v_strValue))
                            Case GetType(Long).Name
                                v_xDataRow.Cells(.FieldName).Value = IIf(v_strValue Is DBNull.Value, 0, CLng(v_strValue))
                            Case GetType(Double).Name
                                v_xDataRow.Cells(.FieldName).Value = IIf(v_strValue Is DBNull.Value, 0, CDbl(v_strValue))
                            Case GetType(System.DateTime).Name
                                v_xDataRow.Cells(.FieldName).Value = IIf(v_strValue Is DBNull.Value, "", CDate(v_strValue).ToString("dd/MM/yyyy"))
                            Case Else
                                v_xDataRow.Cells(.FieldName).Value = IIf(v_strValue Is DBNull.Value, "", v_strValue)
                        End Select
                        SearchGrid.EndInit()
                    End If
                End With
            Next
        Next
        SearchGrid.EndInit()
        _FormatGridBefore(SearchGrid, ObjectName, c_ResourceManager & UserLanguage, False, , pv_intFormRow, pv_intToRow, mv_lngRowCount)
    End Sub

    Private Sub cboFileType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboFileType.SelectedIndexChanged
        SearchGrid.Dispose()
        Select Case CType(sender, ComboBoxEx).SelectedValue
            Case 1, 2
                InitDataGrid("IMPFILE")
            Case 3
                InitDataGrid("IMPFILE2")
            Case 4
                InitDataGrid("IMPFILE1")
        End Select
    End Sub

    Private Sub txtNum_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtNum.Validating
        If Trim(txtNum.Text) = "" Then
            MsgBox(mv_ResourceManager.GetString("ErrNum"), MsgBoxStyle.OkOnly)
            txtNum.Text = 100
            txtNum.Focus()
            Me.btnSearch.Enabled = True
            Exit Sub
        Else
            If IsNumeric(Trim(txtNum.Text)) Then
                If CInt(txtNum.Text) < 0 Then
                    MsgBox(mv_ResourceManager.GetString("ErrNum"), MsgBoxStyle.OkOnly)
                    txtNum.Text = 100
                    txtNum.Focus()
                    Me.btnSearch.Enabled = True
                    Exit Sub
                End If
            Else
                MsgBox(mv_ResourceManager.GetString("ErrNum"), MsgBoxStyle.OkOnly)
                txtNum.Text = 100
                txtNum.Focus()
                Me.btnSearch.Enabled = True
                Exit Sub
            End If
        End If
        mv_intNumRow = CInt(txtNum.Text)
    End Sub

    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        If mv_lngRowCount > mv_intNumRow * mv_intpage Then
            mv_intpage += 1
            OnSearch(, , mv_intpage)
            If mv_lngRowCount <= mv_intNumRow * mv_intpage Then
                btnNext.Enabled = False
            End If
        End If
        If mv_intpage > 1 Then
            btnPre.Enabled = True
        End If
    End Sub

    Private Sub btnPre_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPre.Click
        If mv_intpage > 1 Then
            mv_intpage -= 1
            OnSearch(, , mv_intpage)
            If mv_intpage = 1 Then
                btnPre.Enabled = False
            End If
        End If
        If mv_lngRowCount > mv_intNumRow * mv_intpage Then
            btnNext.Enabled = True
        End If
    End Sub
End Class