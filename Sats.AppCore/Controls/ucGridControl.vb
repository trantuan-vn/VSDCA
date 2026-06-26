Imports System.Windows.Forms
Imports Sats.CommonLibrary
Public Class ucGridControl
    Protected mv_ResourceManager As System.Resources.ResourceManager
    Const c_ResourceManager = "Sats.AppCore.frmSearch_"
    Public mv_gridSearch As GridEx
    Public mv_strTableName As String
    Public mv_oProxy As BDSChannel.BDSDelivery
    Public mv_strUserLanguage As String
    Public mv_strTellerId As String
    Public mv_strBusDate As String
    Public mv_strVsdBrid As String
    Public mv_strVsdBrid2 As String
    Public mv_strBranchId As String
    Public mv_strStockFilter As String
    Public mv_strMemberFilter As String
    Public mv_lngAllMember As String
    Public mv_lngAllStock As String
    Public mv_strTLTXCD As String
    Public mv_strGridValue As String

    Private mv_strFldConditionChk As String                     'List cac truong can co tieu chi tim kiem
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
    Private mv_arrSrFieldWidth() As Integer                     'Độ rộng hiển thị trên lưới
    Private mv_intSearchNum As Integer

    Private mv_strKeyColumn As String
    Private mv_strKeyFieldType As String
    Private mv_strRefColumn As String
    Private mv_strRefFieldType As String

    Private mv_dtDataTable As DataTable
    Private mv_strFinalCmdSql As String

    Private mnuGrid As ContextMenu
    Private mniSelectAll As MenuItem
    Private mniDeSelectAll As MenuItem

#Region "Thuoc tinh"
    Public Property Proxy() As BDSChannel.BDSDelivery
        Get
            Return mv_oProxy
        End Get
        Set(ByVal value As BDSChannel.BDSDelivery)
            mv_oProxy = value
        End Set
    End Property

    Public Property SearchGrid() As GridEx
        Get
            Return mv_gridSearch
        End Get
        Set(ByVal value As GridEx)
            mv_gridSearch = value
        End Set
    End Property

    Public Property TableName() As String
        Get
            Return mv_strTableName
        End Get
        Set(ByVal value As String)
            mv_strTableName = value
        End Set
    End Property

    Public Property UserLanguage() As String
        Get
            Return mv_strUserLanguage
        End Get
        Set(ByVal value As String)
            mv_strUserLanguage = value
        End Set
    End Property

    Public Property TellerId() As String
        Get
            Return mv_strTellerId
        End Get
        Set(ByVal value As String)
            mv_strTellerId = value
        End Set
    End Property

    Public Property BusDate() As String
        Get
            Return mv_strBusDate
        End Get
        Set(ByVal value As String)
            mv_strBusDate = value
        End Set
    End Property
    Public Property BranchId() As String
        Get
            Return mv_strBranchId
        End Get
        Set(ByVal value As String)
            mv_strBranchId = value
        End Set
    End Property

    Public Property VsdBrid() As String
        Get
            Return mv_strVsdBrid
        End Get
        Set(ByVal value As String)
            mv_strVsdBrid = value
        End Set
    End Property

    Public Property VsdBrid2() As String
        Get
            Return mv_strVsdBrid2
        End Get
        Set(ByVal value As String)
            mv_strVsdBrid2 = value
        End Set
    End Property

    Public Property StockFilter() As String
        Get
            Return mv_strStockFilter
        End Get
        Set(ByVal value As String)
            mv_strStockFilter = value
        End Set
    End Property

    Public Property MemberFilter() As String
        Get
            Return mv_strMemberFilter
        End Get
        Set(ByVal value As String)
            mv_strMemberFilter = value
        End Set
    End Property

    Public Property AllMember() As Long
        Get
            Return mv_lngAllMember
        End Get
        Set(ByVal value As Long)
            mv_lngAllMember = value
        End Set
    End Property

    Public Property AllStock() As Long
        Get
            Return mv_lngAllStock
        End Get
        Set(ByVal value As Long)
            mv_lngAllStock = value
        End Set
    End Property

    Public Property TLTXCD() As String
        Get
            Return mv_strTLTXCD
        End Get
        Set(ByVal value As String)
            mv_strTLTXCD = value
        End Set
    End Property

    Public Property GridValue() As String
        Get
            Return mv_strGridValue
        End Get
        Set(ByVal value As String)
            mv_strGridValue = value
        End Set
    End Property
#End Region
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Public Sub OnInit()
        Try
            mv_ResourceManager = New System.Resources.ResourceManager(c_ResourceManager & UserLanguage, System.Reflection.Assembly.GetExecutingAssembly())
            SearchGrid = New GridEx(mv_strTableName, c_ResourceManager & UserLanguage, TellerId)
            Me.pnlSearch.Controls.Add(SearchGrid)
            SearchGrid.Dock = DockStyle.Fill

            If Me.SearchGrid.DataRowTemplate.Cells.Count >= 0 Then
                For i As Integer = 0 To Me.SearchGrid.DataRowTemplate.Cells.Count - 1
                    AddHandler SearchGrid.DataRowTemplate.Cells(i).Click, AddressOf Grid_MouseClick
                Next
            End If

            InitFilter()
            cboField.Clears()

            If mv_intSearchNum > 0 Then
                For i As Integer = 1 To mv_intSearchNum
                    cboField.AddItems(mv_arrSrFieldDisp(i), mv_arrSrFieldSrch(i))
                Next
                cboField.Refresh()
            End If
            AddHandler cboField.SelectedIndexChanged, AddressOf Combo_SelectedIndexChanged
            AddHandler cboOperator.SelectedIndexChanged, AddressOf Combo_SelectedIndexChanged

            Application.DoEvents()

            btnFilter.Left = gbFilter.Right - 28
            btnFilter.Width = 24

            InitContextMenu()
            AddHandler SearchGrid.MouseClick, AddressOf SearchGrid_MouseClick

            RefreshDataSource()

            'Set vi tri cho nut Filter
            Me.txtValue.Width = cboField.Width
            Me.txtValue.Height = cboField.Height
            Me.txtValue.Left = lblValue.Left + lblValue.Width + 24
            Me.txtValue.Top = cboOperator.Top

            Me.btnFilter.Left = Me.txtValue.Left + Me.txtValue.Width + 24
            Me.btnFilter.Top = Me.txtValue.Top
            Me.btnFilter.Width = 80
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub ucGridControl_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Combo_SelectedIndexChanged(cboField, e)
    End Sub

    Public Sub InitFilter()
        Dim v_strCmdInquiry As String
        Dim v_obj As New SQLEngine.SQLDataAccessLayer(TellerId)
        Try
            v_strCmdInquiry = "SELECT s.searchcode, s.searchtitle, s.en_searchtitle,  s.searchcmdsql , (CASE WHEN s.searchcmdsql1 IS NULL THEN '' ELSE s.searchcmdsql1 END) as searchcmdsql1, s.objname, s.frmname, s.orderbycmdsql, s.fldchk, sf.position AS POSITION, sf.fieldcode, " _
                            & " sf.fieldname, sf.en_fieldname, sf.fieldtype, sf.fieldsize, sf.mask AS MASK, sf.operator AS OPERATOR, sf.format, sf.display, sf.srch, sf.[Key] AS [KEY], " _
                            & " sf.refvalue, s.tltxcd, sf.width, sf.lookupcmdsql, sf.DEFVALUE" _
                            & " FROM sisearch AS s INNER JOIN" _
                            & " sisearchfld AS sf ON s.searchcode = sf.searchcode" _
                            & " WHERE (s.deleted = 0) AND (s.status = 0) AND (sf.deleted = 0) AND (sf.status = 0) AND (upper(s.SEARCHCODE) = '" & mv_strTableName & "') ORDER BY POSITION"
            Dim mv_ds As DataSet = v_obj.ExecuteReturnDataSet(v_strCmdInquiry)

            Dim v_strKeyValue As String, v_strSrch As String = "", v_strRefValue As String
            Dim v_strOderbycmdsql As String = "", v_strSrTitle As String = "", v_strSrEnTitle As String = ""
            Dim v_strSrCmd As String = "", v_strSrObjName As String = "", v_strFrmName As String = "", v_strSrFieldCode As String = ""
            Dim v_strSrFieldName As String = "", v_strSrEnFieldName As String = "", v_strSrFieldType As String = ""
            Dim v_strSrFieldMask As String = "", v_strSrFieldDefValue As String = ""
            Dim v_strSrFieldOperator As String = "", v_strSrFieldFormat As String = ""
            Dim v_strSrFieldDisplay As String = "", v_strSrLookupSql As String = ""
            Dim v_intSrFieldWidth As Integer, v_strLstTable As String = "", v_strFldConditionChk As String = ""


            mv_intSearchNum = 0
            ReDim mv_arrSrFieldSrch(mv_ds.Tables(0).Rows.Count)
            ReDim mv_arrSrFieldDisp(mv_ds.Tables(0).Rows.Count)
            ReDim mv_arrSrFieldType(mv_ds.Tables(0).Rows.Count)
            ReDim mv_arrSrFieldMask(mv_ds.Tables(0).Rows.Count)
            ReDim mv_arrStFieldDefValue(mv_ds.Tables(0).Rows.Count)
            ReDim mv_arrSrFieldOperator(mv_ds.Tables(0).Rows.Count)
            ReDim mv_arrSrFieldFormat(mv_ds.Tables(0).Rows.Count)
            ReDim mv_arrSrFieldDisplay(mv_ds.Tables(0).Rows.Count)
            ReDim mv_arrSrFieldWidth(mv_ds.Tables(0).Rows.Count)
            ReDim mv_arrSrSQLRef(mv_ds.Tables(0).Rows.Count)

            For i As Integer = 0 To mv_ds.Tables(0).Rows.Count - 1
                With mv_ds.Tables(0)
                    v_strSrch = Trim(.Rows(i)("SRCH"))
                    v_strSrTitle = Trim(.Rows(i)("SEARCHTITLE"))
                    v_strSrEnTitle = Trim(.Rows(i)("EN_SEARCHTITLE"))
                    v_strSrCmd = Trim(.Rows(i)("SEARCHCMDSQL")) + Trim(.Rows(i)("SEARCHCMDSQL1"))
                    v_strSrObjName = Trim(.Rows(i)("OBJNAME"))
                    v_strFrmName = Trim(.Rows(i)("FRMNAME"))
                    v_strSrFieldCode = Trim(.Rows(i)("FIELDCODE"))
                    v_strSrFieldName = Trim(.Rows(i)("FIELDNAME"))
                    v_strSrEnFieldName = Trim(.Rows(i)("EN_FIELDNAME"))
                    v_strSrFieldType = Trim(.Rows(i)("FIELDTYPE"))
                    v_strSrFieldMask = Trim(.Rows(i)("MASK"))
                    v_strSrFieldDefValue = Trim(.Rows(i)("DEFVALUE"))
                    v_strOderbycmdsql = Trim(.Rows(i)("ORDERBYCMDSQL"))
                    v_strSrFieldOperator = Trim(.Rows(i)("OPERATOR"))
                    v_strSrFieldFormat = Trim(.Rows(i)("FORMAT"))
                    v_strSrFieldDisplay = Trim(.Rows(i)("DISPLAY"))
                    v_strKeyValue = Trim(.Rows(i)("KEY"))
                    v_strFldConditionChk = Trim(.Rows(i)("FLDCHK"))
                    If v_strKeyValue = "Y" Then
                        mv_strKeyColumn = v_strSrFieldCode
                        mv_strKeyFieldType = v_strSrFieldType
                    End If
                    v_strRefValue = Trim(.Rows(i)("REFVALUE"))

                    If v_strRefValue = "Y" Then
                        mv_strRefColumn = v_strSrFieldCode
                        mv_strRefFieldType = v_strSrFieldType
                    End If
                    v_intSrFieldWidth = CInt(Trim(.Rows(i)("WIDTH")))
                    mv_strTLTXCD = Trim(.Rows(i)("TLTXCD"))
                    v_strSrLookupSql = Trim(.Rows(i)("LOOKUPCMDSQL"))
                End With

                If v_strSrch = "Y" Then
                    mv_intSearchNum += 1

                    mv_arrSrFieldSrch(mv_intSearchNum) = v_strSrFieldCode
                    mv_arrSrFieldDisp(mv_intSearchNum) = IIf(mv_strUserLanguage = gc_LANG_VIETNAMESE, v_strSrFieldName, v_strSrEnFieldName)
                    mv_arrSrFieldType(mv_intSearchNum) = v_strSrFieldType
                    mv_arrSrFieldMask(mv_intSearchNum) = v_strSrFieldMask
                    mv_arrStFieldDefValue(mv_intSearchNum) = v_strSrFieldDefValue
                    mv_arrSrFieldOperator(mv_intSearchNum) = v_strSrFieldOperator
                    mv_arrSrFieldFormat(mv_intSearchNum) = v_strSrFieldFormat
                    mv_arrSrFieldDisplay(mv_intSearchNum) = v_strSrFieldDisplay
                    mv_arrSrFieldWidth(mv_intSearchNum) = v_intSrFieldWidth
                    mv_arrSrSQLRef(mv_intSearchNum) = v_strSrLookupSql
                End If
            Next

            If mv_intSearchNum > 0 Then
                ReDim Preserve mv_arrSrFieldSrch(mv_intSearchNum)
                ReDim Preserve mv_arrSrFieldDisp(mv_intSearchNum)
                ReDim Preserve mv_arrSrFieldType(mv_intSearchNum)
                ReDim Preserve mv_arrSrFieldMask(mv_intSearchNum)
                ReDim Preserve mv_arrStFieldDefValue(mv_intSearchNum)
                ReDim Preserve mv_arrSrFieldOperator(mv_intSearchNum)
                ReDim Preserve mv_arrSrFieldFormat(mv_intSearchNum)
                ReDim Preserve mv_arrSrFieldDisplay(mv_intSearchNum)
                ReDim Preserve mv_arrSrFieldWidth(mv_intSearchNum)
                ReDim Preserve mv_arrSrSQLRef(mv_intSearchNum)
            End If

        Catch ex As Exception
            Throw ex
        Finally
            If Not v_obj Is Nothing Then
                v_obj.CloseConnection()
                v_obj = Nothing
            End If
        End Try
    End Sub

    Private Sub Combo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
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
                                    String.Empty, mv_arrStFieldDefValue(cboField.SelectedIndex + 1), mv_arrSrFieldFormat(cboField.SelectedIndex + 1))
            Else
                NewTxtValue(mv_arrSrSQLRef(cboField.SelectedIndex + 1), mv_arrSrFieldType(cboField.SelectedIndex + 1), _
                                mv_arrSrFieldMask(cboField.SelectedIndex + 1), mv_arrStFieldDefValue(cboField.SelectedIndex + 1), mv_arrSrFieldFormat(cboField.SelectedIndex + 1))
            End If

        ElseIf (sender Is cboOperator) Then
            Try
                If CStr(Me.cboOperator.SelectedValue).Equals("LIKE") Then
                    'Neu dieu kien tim kiem la like thi chuyen ve text box de bo dinh dang
                    NewTxtValue(mv_arrSrSQLRef(cboField.SelectedIndex + 1), mv_arrSrFieldType(cboField.SelectedIndex + 1), _
                                        String.Empty, mv_arrStFieldDefValue(cboField.SelectedIndex + 1), mv_arrSrFieldFormat(cboField.SelectedIndex + 1))
                Else
                    NewTxtValue(mv_arrSrSQLRef(cboField.SelectedIndex + 1), mv_arrSrFieldType(cboField.SelectedIndex + 1), _
                                    mv_arrSrFieldMask(cboField.SelectedIndex + 1), mv_arrStFieldDefValue(cboField.SelectedIndex + 1), mv_arrSrFieldFormat(cboField.SelectedIndex + 1))
                End If
            Catch ex As Exception
                'Throw ex
            End Try
        End If
    End Sub

    Private Sub NewTxtValue(ByVal pv_strSqlRef As String, ByVal pv_strFldType As String, _
                           ByVal pv_strFldMask As String, ByVal pv_strDefValue As String, ByVal pv_strFldFormat As String)
        txtValue.Dispose()

        If pv_strSqlRef.Trim.Length < 1 Then
            If Trim$(mv_arrSrFieldType(cboField.SelectedIndex + 1)) = "D" Or Trim$(mv_arrSrFieldType(cboField.SelectedIndex + 1)) = "P" Then
                Me.txtValue = New DateTimePicker
                CType(Me.txtValue, DateTimePicker).Format = DateTimePickerFormat.Custom
                CType(Me.txtValue, DateTimePicker).CustomFormat = gc_FORMAT_DATE
            Else
                If (pv_strFldMask.Trim.Length = 0) Then
                    Me.txtValue = New System.Windows.Forms.TextBox

                    Select Case pv_strFldType.Trim
                        Case "C"
                            CType(Me.txtValue, System.Windows.Forms.TextBox).TextAlign = HorizontalAlignment.Left
                        Case "N"
                            CType(Me.txtValue, System.Windows.Forms.TextBox).TextAlign = HorizontalAlignment.Right
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
        'Load CSDL
        If pv_strSqlRef.Trim.Length > 0 Then
            CType(txtValue, ComboBoxEx).DropDownStyle = ComboBoxStyle.DropDown
            CType(txtValue, ComboBoxEx).DropDownWidth = 400

            Dim v_ds As DataSet
            Dim v_obj As New SQLEngine.SQLDataAccessLayer(TellerId)
            v_ds = v_obj.ExecuteReturnDataSet(pv_strSqlRef)

            CType(txtValue, ComboBoxEx).BeginUpdate()
            CType(txtValue, ComboBoxEx).DataSource = v_ds.Tables(0)
            CType(txtValue, ComboBoxEx).DisplayMember = "DISPLAY"
            CType(txtValue, ComboBoxEx).ValueMember = "VALUE"
            CType(txtValue, ComboBoxEx).EndUpdate()
            v_ds.Dispose()
            v_obj.CloseConnection()
            v_obj = Nothing
        End If

        'txtValue
        '
        Me.txtValue.Enabled = True
        Me.txtValue.Name = "txtValue"
        Me.txtValue.Width = cboField.Width
        Me.txtValue.Height = cboField.Height
        Me.txtValue.Left = lblValue.Left + lblValue.Width + 24
        Me.txtValue.Top = cboOperator.Top
        Me.txtValue.TabIndex = cboOperator.TabIndex + 1
        If pv_strDefValue <> "" Then
            Me.txtValue.Text = pv_strDefValue
        Else
            Me.txtValue.Text = String.Empty
        End If
        Me.txtValue.Visible = True

        Me.gbFilter.Controls.Add(Me.txtValue)
        Me.btnFilter.Left = Me.txtValue.Left + Me.txtValue.Width + 24
        Me.btnFilter.Top = Me.txtValue.Top
        Me.btnFilter.Width = 80
    End Sub

    Private Sub RefreshDataSource(ByVal pv_strFilter As String)
        Dim v_strCmdSql As String = mv_strFinalCmdSql
        Try
            If pv_strFilter <> String.Empty Then
                v_strCmdSql = "SELECT * FROM ( " & v_strCmdSql & " ) WHERE " & pv_strFilter
            End If
            Dim v_strObjMsg As String = BuildXMLObjMsg(, BranchId, , TellerId, gc_IsNotLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLGROUPS, gc_ActionInquiry, v_strCmdSql, , , , IIf(mv_lngAllStock = 0, "('000')", "(" & mv_strStockFilter & ")") & "|" & IIf(mv_lngAllMember = 0, "('000')", "(" & mv_strMemberFilter & ")"))

            Dim v_lngError As Long = Proxy.Message(v_strObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Exit Sub
            End If

            FillDataGrid(SearchGrid, TellerId, v_strObjMsg, c_ResourceManager & UserLanguage, mv_strTableName, , 0, 0, 0, Nothing)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub RefreshDataSource()
        Dim v_strCmdInquiry As String
        Dim v_strCmdSql As String
        Dim v_obj As New SQLEngine.SQLDataAccessLayer(TellerId)
        Try
            'v_strCmdInquiry = "SELECT s.searchcmdsql || (CASE WHEN s.searchcmdsql1 IS NULL THEN '' ELSE s.searchcmdsql1 END) as searchcmdsql " _
            '                & " FROM sisearch AS s WHERE s.deleted = 0 AND s.status = 0 AND upper(s.SEARCHCODE) = '" & mv_strTableName & "'"

            v_strCmdInquiry = "SELECT s.searchcmdsql as searchcmdsql " _
                            & " FROM sisearch AS s WHERE s.deleted = 0 AND s.status = 0 AND upper(s.SEARCHCODE) = '" & mv_strTableName & "'"


            Dim v_ds As DataSet = v_obj.ExecuteReturnDataSet(v_strCmdInquiry)
            If v_ds.Tables.Count > 0 Then
                If v_ds.Tables(0).Rows.Count > 0 Then
                    v_strCmdSql = v_ds.Tables(0).Rows(0)("SEARCHCMDSQL")

                    v_strCmdSql = v_strCmdSql.Replace("?LANGUAGE", Me.UserLanguage)
                    v_strCmdSql = v_strCmdSql.Replace("?MEMBERFILTER", IIf(mv_lngAllMember = 0, " NVL(a.micode,'000') in " & mv_strMemberFilter, " 1=1 "))
                    v_strCmdSql = v_strCmdSql.Replace("?COMEMBERFILTER", IIf(mv_lngAllMember = 0, " NVL(a.comicode,'000') in " & mv_strMemberFilter, " 1=1 "))
                    v_strCmdSql = v_strCmdSql.Replace("?STOCKFILTER", IIf(mv_lngAllStock = 0, " NVL(a.sicode,'000') in " & mv_strStockFilter, " 1=1 "))
                    v_strCmdSql = v_strCmdSql.Replace("?BRID", mv_strBranchId)
                    v_strCmdSql = v_strCmdSql.Replace("?TLID", mv_strTellerId)
                    v_strCmdSql = v_strCmdSql.Replace("?TLTXCD", mv_strTLTXCD)
                    v_strCmdSql = v_strCmdSql.Replace("?VSD_BRID", mv_strVsdBrid)
                    v_strCmdSql = v_strCmdSql.Replace("?VSD_2_BRID", mv_strVsdBrid2)

                    

                    mv_strFinalCmdSql = v_strCmdSql

                    'Dim v_strObjMsg As String = BuildXMLObjMsg(, BranchId, , TellerId, gc_IsNotLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLGROUPS, gc_ActionInquiry, v_strCmdSql, , , , IIf(mv_lngAllStock = 0, "('000')", "(" & mv_strStockFilter & ")") & "|" & IIf(mv_lngAllMember = 0, "('000')", "(" & mv_strMemberFilter & ")"))
                    Dim v_strObjMsg As String = BuildXMLObjMsg(, BranchId, , TellerId, gc_IsNotLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLGROUPS, gc_ActionInquiry, v_strCmdSql, , , , mv_strStockFilter & "|" & mv_strMemberFilter)

                    Dim v_lngError As Long = Proxy.Message(v_strObjMsg)
                    If v_lngError <> ERR_SYSTEM_OK Then
                        MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                        Exit Sub
                    End If

                    FillDataGrid(SearchGrid, TellerId, v_strObjMsg, c_ResourceManager & UserLanguage, mv_strTableName, , 0, 0, 0, Nothing)
                End If
            End If
        Catch ex As Exception
            Throw ex
        Finally
            If Not v_obj Is Nothing Then
                v_obj.CloseConnection()
                v_obj = Nothing
            End If
        End Try
    End Sub

    Public Sub Grid_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If Not SearchGrid.CurrentColumn Is Nothing Then
            If SearchGrid.CurrentColumn.FieldName = "TXBLN" Then
                If SearchGrid.CurrentCell.Value = True Then
                    SearchGrid.CurrentCell.Value = False
                Else
                    SearchGrid.CurrentCell.Value = True
                End If
                GridValue = GetGridValue()
            End If
        End If
    End Sub

    Public Sub Grid_MouseClick(ByVal sender As Object, ByVal e As MouseEventArgs)
        If (e.Button = Windows.Forms.MouseButtons.Left) Then
            If Not SearchGrid.CurrentColumn Is Nothing Then
                If SearchGrid.CurrentColumn.FieldName = "TXBLN" Then
                    If SearchGrid.CurrentCell.Value = True Then
                        SearchGrid.CurrentCell.Value = False
                    Else
                        SearchGrid.CurrentCell.Value = True
                    End If
                    GridValue = GetGridValue()
                End If
            End If
        End If
    End Sub

    Public Function GetGridValue() As String
        Dim v_strKeyValue As String
        Dim v_strGridValue As String = ""
        Try
            With CType(SearchGrid, GridEx)
                For i = 0 To .DataRows.Count - 1
                    With .DataRows(i)
                        If .Cells("TXBLN").Value Then
                            v_strKeyValue = .Cells("KEYVALUE").Value
                            v_strGridValue = v_strGridValue & "," & v_strKeyValue
                        End If
                    End With
                Next
            End With
            If v_strGridValue.Length > 0 Then
                v_strGridValue = Mid(v_strGridValue, 2)
            End If
            Return v_strGridValue
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Sub InitContextMenu()
        Try
            mnuGrid = New ContextMenu()
            mniSelectAll = New MenuItem()
            mniSelectAll.Index = 0
            mniSelectAll.Text = "Chọn tất cả"
            AddHandler mniSelectAll.Click, AddressOf ContextMenu_Click

            mniDeSelectAll = New MenuItem()
            mniDeSelectAll.Index = 1
            mniDeSelectAll.Text = "Bỏ chọn tất cả"
            AddHandler mniDeSelectAll.Click, AddressOf ContextMenu_Click

            mnuGrid.MenuItems.AddRange(New MenuItem() {Me.mniSelectAll, Me.mniDeSelectAll})
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub SearchGrid_MouseClick(ByVal sender As Object, ByVal e As MouseEventArgs)
        If (e.Button = Windows.Forms.MouseButtons.Right) Then
            mnuGrid.Show(SearchGrid, New Drawing.Point(e.X, e.Y))
        End If
    End Sub

    Private Sub ContextMenu_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim objCurMenuItem As MenuItem = CType(sender, MenuItem)
            If objCurMenuItem.Index = 0 Then
                For Each dr As Xceed.Grid.DataRow In SearchGrid.DataRows
                    dr.Cells("TXBLN").Value = True
                Next
            ElseIf objCurMenuItem.Index = 1 Then
                For Each dr As Xceed.Grid.DataRow In SearchGrid.DataRows
                    dr.Cells("TXBLN").Value = False
                Next
            End If

            GridValue = GetGridValue()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub btnFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFilter.Click
        Try
            Dim v_strValue As String
            Dim v_strOperator As String
            Dim v_strField As String
            Dim v_strExpression As String

            If mv_arrSrSQLRef(cboField.SelectedIndex + 1).Trim.Length > 0 Then
                v_strValue = CType(txtValue, ComboBoxEx).SelectedValue
            Else
                v_strValue = Trim(txtValue.Text.ToString)
            End If

            v_strValue = v_strValue.Replace("'", "''")
            v_strOperator = cboOperator.SelectedValue.ToString
            v_strField = cboField.SelectedValue.ToString

            If v_strValue <> String.Empty Then
                If v_strField <> String.Empty And v_strOperator <> String.Empty Then
                    If v_strOperator = "LIKE" Then
                        v_strExpression = v_strField & " " & v_strOperator & " '%" & v_strValue & "%'"
                    Else
                        If mv_arrSrFieldType(cboField.SelectedIndex + 1) = "D" Then
                            v_strExpression = v_strField & " " & v_strOperator & "TO_DATE('" & v_strValue & "', 'DD/MM/YYYY')"
                        Else
                            v_strExpression = v_strField & " " & v_strOperator & " " & IIf(mv_arrSrFieldType(cboField.SelectedIndex + 1) <> "N", "'", "") & v_strValue & IIf(mv_arrSrFieldType(cboField.SelectedIndex + 1) <> "N", "'", "")
                        End If
                    End If
                    RefreshDataSource(v_strExpression)
                End If
            Else
                RefreshDataSource()
            End If
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                                     & "Error code: System error!" & vbNewLine _
                                                     & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Sub
End Class
