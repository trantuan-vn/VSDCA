Imports System.IO
Imports System.Collections
Imports Sats.CommonLibrary
Imports Xceed.SmartUI.Controls
Imports Sats.WinFormsUI.Docking
Imports System.Windows.Forms
Imports Microsoft.Office.Interop.Excel


Public Class frmSearch

#Region " Khai báo hằng, biến "

    Const c_ResourceManager = "Sats.AppCore.frmSearch_"
    Dim mv_dblTR_QTTY As Double = 0
    Protected WithEvents SearchGrid As GridEx
    Protected WithEvents SearchCell As Xceed.Grid.Cell
    Public mv_strSearchFilter As String
    Public hFilter As New Hashtable
    'Private hLstTable As New Hashtable
    'Public mv_frmTransactScreen As frmTransact

    'Hanm5 them 2 bien va cac thuoc tinh
    Public mv_strSICODE As String = String.Empty
    Public mv_strMICODE As String = String.Empty
    Public mv_strCOMICODE As String = String.Empty
    Public mv_intMaxRowSearch As Int32 = 0
    Public mv_intExportRowFrom As Int32
    Public mv_intExportRowTo As Int32
    'Public mv_lwLogWriter As New LogWriter
    'End Hanm5----------------------

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
    'Private mv_arrSrLstTable() As String
    Private mv_arrSrFieldWidth() As Integer                     'Độ rộng hiển thị trên lưới

    Private mv_strLanguage As String
    Protected mv_ResourceManager As System.Resources.ResourceManager

    Private mv_strBranchId As String
    Private mv_strTellerId As String
    'Added by Thanglv9 - 13/12/2012
    Private mv_strTellerName As String
    Private mv_strBRCODE As String
    'End
    Private mv_strTellerType As String
    Private mv_intpage As Int32 = 1
    Private mv_rowpage As Int32 = 1
    Private mv_strBusDate As String
    Private mv_intRowCount As Int32 = 0

    Public mv_enuEditFormResult As SaveButtonType

    Private mv_SelectedRow As Xceed.Grid.Row

    'Private mv_BDSDelivery As BDSChannel.BDSDelivery

    Private mv_strCondition As String
    Private mv_lngRowCount As Long
    Private mv_lngAllMember As Long
    Private mv_lngAllStock As Long
    Private mv_strMemberFilter As String
    Private mv_strStockFilter As String
    Private mv_strSharp As String = ""
    Private mv_strPARENT_TLTXCD As String = ""
    Private mv_strPassDate As String
    Private mv_hPartition As New Hashtable
    'Private mv_hCondition As New Hashtable
    Private mv_lngAutoID As Long
    Private mv_strTXDATE As String
    Private mv_blnCloseForm As Boolean = False
    Private mv_strVsdBrid As String = ""
    Private mv_strVsdBrid2 As String = ""
    Private mv_oProxy As BDSChannel.BDSDelivery
    Private mv_oClient As Client
    Public mv_strGlobalAuthString As String = ""
    Public mv_intRowSelectedCount As Integer = 0
#End Region


#Region " Các thuộc tính của form "
    Public Property Client() As Client
        Get
            Return mv_oClient
        End Get
        Set(ByVal value As Client)
            mv_oClient = value
        End Set
    End Property
    Public Property Proxy() As BDSChannel.BDSDelivery
        Get
            Return mv_oProxy
        End Get
        Set(ByVal value As BDSChannel.BDSDelivery)
            mv_oProxy = value
        End Set
    End Property

    Public Property MaxRowSearch() As Int32
        Get
            Return mv_intMaxRowSearch
        End Get
        Set(ByVal value As Int32)
            mv_intMaxRowSearch = value
        End Set
    End Property

    Public Property CloseFormWhenDoubleClick() As Boolean
        Get
            Return mv_blnCloseForm
        End Get
        Set(ByVal value As Boolean)
            mv_blnCloseForm = value
        End Set
    End Property


    Public Property TXDate() As String
        Get
            Return mv_strTXDATE
        End Get
        Set(ByVal value As String)
            mv_strTXDATE = value
        End Set
    End Property

    Public Property AutoID() As Long
        Get
            Return mv_lngAutoID
        End Get
        Set(ByVal value As Long)
            mv_lngAutoID = value
        End Set
    End Property

    Public Property PassDate() As String
        Get
            Return mv_strPassDate
        End Get
        Set(ByVal value As String)
            mv_strPassDate = value
        End Set
    End Property

    Public Property PARENT_TLTXCD() As String
        Get
            Return mv_strPARENT_TLTXCD
        End Get
        Set(ByVal value As String)
            mv_strPARENT_TLTXCD = value
        End Set
    End Property
    Public Property Sharp() As String
        Get
            Return mv_strSharp
        End Get
        Set(ByVal value As String)
            mv_strSharp = value
        End Set
    End Property
    Public Property SICODE() As String
        Get
            Return mv_strSICODE
        End Get
        Set(ByVal value As String)
            mv_strSICODE = value
        End Set
    End Property

    Public Property MICODE() As String
        Get
            Return mv_strMICODE
        End Get
        Set(ByVal value As String)
            mv_strMICODE = value
        End Set
    End Property

    Public Property COMICODE() As String
        Get
            Return mv_strCOMICODE
        End Get
        Set(ByVal value As String)
            mv_strCOMICODE = value
        End Set
    End Property

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
    Public Property FULLDATA() As String
        Get
            Return mv_strXMLData
        End Get
        Set(ByVal Value As String)
            mv_strXMLData = Value
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

    Public Property VsdBrid() As String
        Get
            Return mv_strVsdBrid
        End Get
        Set(ByVal Value As String)
            mv_strVsdBrid = Value
        End Set
    End Property

    Public Property VsdBrid2() As String
        Get
            Return mv_strVsdBrid2
        End Get
        Set(ByVal Value As String)
            mv_strVsdBrid2 = Value
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
    'Added by Thanglv9 - 13/12/2012
    Public Property TellerName() As String
        Get
            Return mv_strTellerName
        End Get
        Set(ByVal Value As String)
            mv_strTellerName = Value
        End Set
    End Property
    Public Property BRCODE() As String
        Get
            Return mv_strBRCODE
        End Get
        Set(ByVal Value As String)
            mv_strBRCODE = Value
        End Set
    End Property
    'End

    Public Property TellerType() As String
        Get
            Return mv_strTellerType
        End Get
        Set(ByVal Value As String)
            mv_strTellerType = Value
        End Set
    End Property

    Public ReadOnly Property ResourceManager() As System.Resources.ResourceManager
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
            Case Keys.F9
                'Tương đương nhấn Double Click của dòng hiện tại
                If mv_intDblGrid = 0 Then
                    mv_intDblGrid = 1
                    If Me.btnView.Visible = False Then
                        OnClose()
                    End If
                    OnQuery()
                    mv_intDblGrid = 0
                End If
            Case Keys.Escape
                OnClose()
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

    Overridable Sub Grid_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If Not SearchGrid.CurrentColumn Is Nothing Then
            If SearchGrid.CurrentColumn.FieldName = "TXBLN" Then
                If mv_strGlobalAuthString = "" And Mid(CType(SearchGrid.CurrentRow, Xceed.Grid.DataRow).Cells("AUTH_STRING").Value, 2, 3) <> "NNN" Then
                    mv_strGlobalAuthString = Mid(CType(SearchGrid.CurrentRow, Xceed.Grid.DataRow).Cells("AUTH_STRING").Value, 2, 3)
                    If SearchGrid.CurrentCell.Value = True Then
                        SearchGrid.CurrentCell.Value = False
                        mv_intRowSelectedCount -= 1
                        'bangpv
                        Chk_check_confirm_all.Checked = False

                        'end bangpv
                    Else
                        SearchGrid.CurrentCell.Value = True
                        mv_intRowSelectedCount += 1
                    End If
                Else
                    If Mid(CType(SearchGrid.CurrentRow, Xceed.Grid.DataRow).Cells("AUTH_STRING").Value, 2, 3) = mv_strGlobalAuthString Then
                        If SearchGrid.CurrentCell.Value = True Then
                            SearchGrid.CurrentCell.Value = False
                            mv_intRowSelectedCount -= 1
                            'bangpv
                            Chk_check_confirm_all.Checked = False

                            'end bangpv
                        Else
                            SearchGrid.CurrentCell.Value = True
                            mv_intRowSelectedCount += 1
                        End If
                    End If
                End If
            End If
            If mv_intRowSelectedCount = 0 Then
                mv_strGlobalAuthString = ""
            End If
        End If
    End Sub

    Overridable Sub Grid_DblClick(ByVal sender As Object, ByVal e As System.EventArgs)
        Cursor = Cursors.WaitCursor
        If mv_intDblGrid = 0 Then
            mv_intDblGrid = 1
            If Me.btnView.Visible = False Then
                If Me.btnExecute.Visible = False Then
                    If CloseFormWhenDoubleClick Then
                        OnClose()
                    End If
                    Exit Sub
                Else
                    OnExecute()
                    Exit Sub
                End If
            End If
            OnQuery()
            mv_intDblGrid = 0
            Cursor = Cursors.Default
        End If
    End Sub

    Private Sub Grid_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        Try
            Select Case e.KeyCode
                Case Keys.Space
                    If Not SearchGrid.Columns("TXBLN") Is Nothing Then
                        If mv_strGlobalAuthString = "" And Mid(CType(SearchGrid.CurrentRow, Xceed.Grid.DataRow).Cells("AUTH_STRING").Value, 2, 3) <> "NNN" Then
                            mv_strGlobalAuthString = Mid(CType(SearchGrid.CurrentRow, Xceed.Grid.DataRow).Cells("AUTH_STRING").Value, 2, 3)
                            If SearchGrid.CurrentColumn.FieldName = "TXBLN" Then
                                If SearchGrid.CurrentCell.Value = True Then
                                    SearchGrid.CurrentCell.Value = False
                                    mv_intRowSelectedCount -= 1
                                Else
                                    SearchGrid.CurrentCell.Value = True
                                    mv_intRowSelectedCount += 1
                                End If
                            End If
                        Else
                            If Mid(CType(SearchGrid.CurrentRow, Xceed.Grid.DataRow).Cells("AUTH_STRING").Value, 2, 3) = mv_strGlobalAuthString Then
                                If SearchGrid.CurrentColumn.FieldName = "TXBLN" Then
                                    If SearchGrid.CurrentCell.Value = True Then
                                        SearchGrid.CurrentCell.Value = False
                                        mv_intRowSelectedCount -= 1
                                    Else
                                        SearchGrid.CurrentCell.Value = True
                                        mv_intRowSelectedCount += 1
                                    End If
                                End If
                            End If
                        End If
                        If mv_intRowSelectedCount = 0 Then
                            mv_strGlobalAuthString = ""
                        End If
                    End If
                Case Keys.Enter 'Enter = Onclose de insert luon cho GD,Double_click =View 
                    'Cursor = Cursors.WaitCursor
                    'If mv_intDblGrid = 0 Then
                    '    mv_intDblGrid = 1
                    '    If Me.tbnView.Visible = False Then
                    '        If Me.tbnExecute.Visible = False Then
                    '            OnClose()
                    '            Exit Sub
                    '        Else
                    '            OnExecute()
                    '            Exit Sub
                    '        End If
                    '    End If
                    '    OnQuery()
                    '    mv_intDblGrid = 0
                    'End If
                    'OnClose()
                    'Case Keys.Delete
                    '   OnDelete()
            End Select
        Catch ex As Exception
            MsgBox(ex.ToString)
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

            'Khi dieu kien tim kiem la like thi bo dinh dang
        End If
    End Sub

    Private Sub Toolbar_Click(ByVal sender As System.Object, ByVal e As Xceed.SmartUI.SmartItemClickEventArgs)
        'Dim v_thread As New Threading.Thread(AddressOf ShowFormProcess)
        'v_thread.Start()
        'Application.DoEvents()

        Cursor = Cursors.WaitCursor
        CType(sender, Xceed.SmartUI.SmartItem).Enabled = False
        If (sender Is btnAddNew) And (btnAddNew.Visible = True) Then
            OnAddNew()
        ElseIf (sender Is btnExecute) And (btnExecute.Visible = True) Then
            OnExecute()
        ElseIf (sender Is btnView) And (btnView.Visible = True) Then
            OnQuery()
        ElseIf (sender Is btnEdit) And (btnEdit.Visible = True) Then
            OnUpdate()
        ElseIf (sender Is btnDelete) And (btnDelete.Visible = True) Then
            OnDelete(IsLocalSearch, ModuleCode & "." & ObjectName)
        ElseIf (sender Is btnExit) Then
            OnClose()
        End If
        If mv_strTableName <> "TLLOG" Then
            CType(sender, Xceed.SmartUI.SmartItem).Enabled = True
        End If

        Cursor = Cursors.Default
        'v_thread.Abort()
    End Sub

    Public Sub SelectExportType()
        Dim v_intExportType As Integer
        Try
            Dim v_frm As New frmExportParameter
            v_frm.MaxRowSearch = MaxRowSearch
            v_frm.ShowDialog()

            v_intExportType = v_frm.ExportType
            mv_intExportRowFrom = v_frm.RowFrom
            mv_intExportRowTo = v_frm.RowTo

            Select Case v_intExportType
                Case 0
                    OnExport()
                Case 1
                    OnExportFromDB(IsLocalSearch, ModuleCode & "." & ObjectName, mv_intExportRowFrom, mv_intExportRowTo)
                Case -1
                    Exit Sub
            End Select
        Catch ex As Exception
            Throw ex
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
                If CInt(Trim(txtNumRow.Text)) <= MaxRowSearch Then
                    OnSearch(IsLocalSearch, ModuleCode & "." & ObjectName)
                Else
                    MsgBox(Replace(mv_ResourceManager.GetString("MaxRowSearch"), "@MAX_ROW_SEARCH", MaxRowSearch.ToString), MsgBoxStyle.Critical, "Thông báo")
                    txtNumRow.Focus()
                    Exit Sub
                End If
            ElseIf (sender Is btnExport) Then
                SelectExportType()
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
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
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
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            ' & "Error code: System error!" & vbNewLine _
            ' & "Error message: " & ex.Message, EventLogEntryType.Error)
            Throw ex

        End Try
    End Function
    Public Function CountRowP() As Int32
        Try

            Dim v_xmlDocument As New Xml.XmlDocument
            Dim v_nodeList As Xml.XmlNodeList
            Dim v_int, v_intCount As Integer
            Dim v_intCOUNTROW As Int32
            Dim v_strFLDNAME, v_strVALUE As String
            'Dim v_ws As New BDSDelivery.BDSDelivery
            Dim v_strCmdInquiry As String = "select COUNT(*) COUNTROW from (" & mv_strCmdSql & ") WHERE 0=0"
            System.Windows.Forms.Application.DoEvents()
            'Dim v_strObjMsg As String = BuildXMLObjMsg(, , , , gc_IsLocalMsg, gc_MsgTypeObj, "SA.ALLCODE", gc_ActionInquiry, v_strCmdInquiry)
            Dim v_strObjMsg As String = BuildXMLObjMsg(Me.BusDate, Me.BranchId, , TellerId, IsLocalSearch, gc_MsgTypeObj, ModuleCode & "." & ObjectName, _
                                          gc_ActionInquiry, v_strCmdInquiry, , , , StockFilter & "|" & MemberFilter)

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
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            ' & "Error code: System error!" & vbNewLine _
            ' & "Error message: " & ex.Message, EventLogEntryType.Error)
            Throw ex

        End Try
    End Function


    Private Sub AddSearchCriteria()
        Try
            Dim v_strValue, v_strValueDisplay As String
            Dim v_strCondition As String
            Dim v_objResult As Object
            'Dim v_strFilterTmp As String
            'Dim v_strFilterTmpUpper As String
            Dim v_strSearchKey As String
            Dim v_blnSearchKeyAdded As Boolean
            'Dim v_strLstTable As String
            'Dim i1 As Int16
            v_strValueDisplay = Trim(txtValue.Text)
            'v_strLstTable = Trim(txtValue.Tag)
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
                        'If mv_arrSrFieldType(cboField.SelectedIndex + 1) = "D" Then
                        '    v_strFilterTmp = "TO_DATE(T."
                        '    v_strFilterTmp &= IIf(mv_arrSrSQLRef(cboField.SelectedIndex + 1).Trim.Length = 0, _
                        '        mv_arrSrFieldSrch(cboField.SelectedIndex + 1), _
                        '        mv_arrSrFieldSrch(cboField.SelectedIndex + 1)) & ",'dd/MM/yyyy')"
                        'Else
                        '    v_strFilterTmp = "T."
                        '    v_strFilterTmp &= IIf(mv_arrSrSQLRef(cboField.SelectedIndex + 1).Trim.Length = 0, _
                        '        mv_arrSrFieldSrch(cboField.SelectedIndex + 1), _
                        '        mv_arrSrFieldSrch(cboField.SelectedIndex + 1))
                        'End If
                        'v_strFilterTmpUpper = "UPPER(" & v_strFilterTmp & ")"
                        'v_strFilterTmp &= " " & cboOperator.SelectedValue & " "
                        'v_strFilterTmpUpper &= " " & cboOperator.SelectedValue & " "
                        'Select Case mv_arrSrFieldType(cboField.SelectedIndex + 1)
                        '    Case "D"
                        '        v_strFilterTmp &= "TO_DATE('" & v_strValue & "', '" & gc_FORMAT_DATE & "')"
                        '    Case "N"

                        '        If IsNumeric(v_strValue) Then
                        '            v_strFilterTmp &= CDbl(v_strValue)
                        '        Else
                        '            Exit Sub
                        '        End If
                        '    Case "C"
                        '        v_strValue = Trim(Replace(v_strValue, ".", String.Empty))

                        '        If InStr(v_strValue, "%") > 0 Then
                        '            v_strFilterTmpUpper &= "'" _
                        '                          & IIf(Trim(cboOperator.SelectedValue) = "LIKE", "", "") & v_strValue _
                        '                          & IIf(Trim(cboOperator.SelectedValue) = "LIKE", "", "") & "'"

                        '        Else
                        '            If v_strValue = String.Empty Then
                        '                v_strFilterTmpUpper = Replace(v_strFilterTmpUpper, "=", "")
                        '                v_strFilterTmpUpper &= " IS NULL "
                        '            Else
                        '                v_strFilterTmpUpper &= "N'" & IIf(Trim(cboOperator.SelectedValue) = "LIKE", "%", "") & v_strValue.ToUpper _
                        '                       & IIf(Trim(cboOperator.SelectedValue) = "LIKE", "%", "") & "'"
                        '            End If
                        '        End If
                        '        v_strFilterTmp = String.Empty
                        '        v_strFilterTmp = v_strFilterTmpUpper
                        'End Select
                        If mv_arrSrFieldType(cboField.SelectedIndex + 1) = "P" Then
                            mv_hPartition(v_strSearchKey) = cboOperator.SelectedValue & "|" & v_strValue
                        End If

                        v_strCondition = cboField.SelectedValue & "|" & cboOperator.SelectedValue & "|" & v_strValue & "|" & mv_arrSrFieldType(cboField.SelectedIndex + 1)
                        hFilter.Add(v_strSearchKey, v_strCondition)

                        lstCondition.Items.Add(v_strSearchKey, True)
                        'hFilter.Add(v_strSearchKey, v_strFilterTmp)
                        'If v_strLstTable <> "" Then
                        'hLstTable.Add(v_strSearchKey, v_strLstTable)
                        'End If
                    End If
                End If
            End If
            Me.btnSearch.Select()
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                                     & "Error code: System error!" & vbNewLine _
                                                     & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Sub

    Private Sub RemoveSearchCriteria()
        Try
            Dim v_objResult As Object

            If lstCondition.SelectedIndex <> -1 Then
                v_objResult = hFilter(lstCondition.Items(lstCondition.SelectedIndex).ToString())

                If Not (v_objResult Is Nothing) Then
                    hFilter.Remove(lstCondition.Items(lstCondition.SelectedIndex).ToString())
                    lstCondition.Items.RemoveAt(lstCondition.SelectedIndex)
                End If
            End If
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                                     & "Error code: System error!" & vbNewLine _
                                                     & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
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
                End If
            Next
            lstCondition.Items.Clear()
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                                     & "Error code: System error!" & vbNewLine _
                                                     & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Sub
#End Region

#Region "Over method"
    Public Overridable Sub InitDialog()
        Try
            'Khởi tạo kích thước form và load resource
            mv_ResourceManager = New System.Resources.ResourceManager(c_ResourceManager & UserLanguage, System.Reflection.Assembly.GetExecutingAssembly())
            LoadResource(Me)
            DoResizeForm()

            'mv_BDSDelivery = New BDSChannel.BDSDelivery
            MaxRowSearch = GetMaxRowSearch()
            SearchGrid = New GridEx(mv_strTableName, c_ResourceManager & UserLanguage, TellerId)
            Me.pnlSearchResult.Controls.Add(SearchGrid)
            SearchGrid.Dock = DockStyle.Fill

            'Gán sự kiên cho Grid
            AddHandler SearchGrid.DoubleClick, AddressOf Grid_DblClick
            If Me.SearchGrid.DataRowTemplate.Cells.Count >= 0 Then
                For i As Integer = 0 To Me.SearchGrid.DataRowTemplate.Cells.Count - 1
                    AddHandler SearchGrid.DataRowTemplate.Cells(i).DoubleClick, AddressOf Grid_DblClick
                    AddHandler SearchGrid.DataRowTemplate.Cells(i).Click, AddressOf Grid_Click
                Next
            End If
            AddHandler SearchGrid.DataRowTemplate.KeyUp, AddressOf Grid_KeyUp


            'Set click event for Xceed smart toolbar button
            AddHandler btnAddNew.Click, AddressOf Toolbar_Click
            AddHandler btnView.Click, AddressOf Toolbar_Click
            AddHandler btnEdit.Click, AddressOf Toolbar_Click
            AddHandler btnDelete.Click, AddressOf Toolbar_Click
            AddHandler btnExit.Click, AddressOf Toolbar_Click
            AddHandler btnExecute.Click, AddressOf Toolbar_Click

            'Set click event for buttons
            AddHandler btnSearch.Click, AddressOf Button_Click
            AddHandler btnExport.Click, AddressOf Button_Click
            AddHandler btnAdd.Click, AddressOf Button_Click
            AddHandler btnRemove.Click, AddressOf Button_Click
            AddHandler btnRemoveAll.Click, AddressOf Button_Click

            'Set KeyDown event for Value textbox




            'Set enable status for toolbar buttons depend on AuthString string
            'If TellerId <> "0001" Then
            'Set enable status for toolbar buttons and other buttons depend on AuthCode string
            btnAddNew.Visible = (Mid(AuthCode, 1, 1) = "Y")
            btnView.Visible = (Mid(AuthCode, 2, 1) = "Y")
            btnEdit.Visible = (Mid(AuthCode, 3, 1) = "Y")
            btnDelete.Visible = (Mid(AuthCode, 4, 1) = "Y")
            btnSearch.Visible = (Mid(AuthCode, 5, 1) = "Y")
            btnExport.Visible = (Mid(AuthCode, 6, 1) = "Y")
            btnExecute.Visible = (Mid(AuthCode, 7, 1) = "Y")
            txtNumRow.MaxLength = 9

            SeparatorTool2.Visible = btnAddNew.Visible Or btnView.Visible Or btnEdit.Visible Or btnDelete.Visible Or btnExecute.Visible

            btnView.Enabled = (Mid(AuthString, 1, 1) = "Y")
            btnAddNew.Enabled = (Mid(AuthString, 2, 1) = "Y")
            btnEdit.Enabled = (Mid(AuthString, 3, 1) = "Y")
            btnDelete.Enabled = (Mid(AuthString, 4, 1) = "Y")
            btnSearch.Enabled = (Mid(AuthString, 5, 1) = "Y")
            btnExport.Enabled = (Mid(AuthString, 6, 1) = "Y")
            'End If

            'btnExport.Enabled = False
            'Set selected index changed event for ComboBoxes
            AddHandler cboField.SelectedIndexChanged, AddressOf Combo_SelectedIndexChanged
            AddHandler cboOperator.SelectedIndexChanged, AddressOf Combo_SelectedIndexChanged

            'Thiết lập các giá trị ban đầu cho các đi?u kiện tìm kiếm
            Dim v_strCmdInquiry As String '= "SELECT * FROM V_SEARCHCD WHERE 0=0 "

            'v_strCmdInquiry = "SELECT s.searchcode, s.searchtitle, s.en_searchtitle, s.searchcmdsql, s.objname, s.frmname, s.orderbycmdsql, s.fldchk, sf.position AS POSITION, sf.fieldcode, " _
            '                & " sf.fieldname, sf.en_fieldname, sf.fieldtype, sf.fieldsize, sf.mask AS MASK, sf.operator AS OPERATOR, sf.format, sf.display, sf.srch, sf.[Key] AS [KEY], " _
            '                & " sf.refvalue, s.tltxcd, sf.width, sf.lookupcmdsql, sf.DEFVALUE" _
            '                & " FROM sisearch AS s INNER JOIN" _
            '                & " sisearchfld AS sf ON s.searchcode = sf.searchcode" _
            '                & " WHERE (s.deleted = 0) AND (s.status = 0) AND (sf.deleted = 0) AND (sf.status = 0) AND (upper(s.SEARCHCODE) = '" & mv_strTableName & "') ORDER BY POSITION"
            v_strCmdInquiry = "SELECT s.searchcode, s.searchtitle, s.en_searchtitle,  s.searchcmdsql , (CASE WHEN s.searchcmdsql1 IS NULL THEN '' ELSE s.searchcmdsql1 END) as searchcmdsql1, s.objname, s.frmname, s.orderbycmdsql, s.fldchk, sf.position AS POSITION, sf.fieldcode, " _
                            & " sf.fieldname, sf.en_fieldname, sf.fieldtype, sf.fieldsize, sf.mask AS MASK, sf.operator AS OPERATOR, sf.format, sf.display, sf.srch, sf.[Key] AS [KEY], " _
                            & " sf.refvalue, s.tltxcd, sf.width, sf.lookupcmdsql, sf.DEFVALUE" _
                            & " FROM sisearch AS s INNER JOIN" _
                            & " sisearchfld AS sf ON s.searchcode = sf.searchcode" _
                            & " WHERE (s.deleted = 0) AND (s.status = 0) AND (sf.deleted = 0) AND (sf.status = 0) AND (upper(s.SEARCHCODE) = '" & mv_strTableName & "') ORDER BY POSITION"
            Dim v_obj As New SQLEngine.SQLDataAccessLayer(TellerId)

            Dim v_ds As DataSet = v_obj.ExecuteReturnDataSet(v_strCmdInquiry)
            'Dim v_strClause As String = " UPPER(SEARCHCODE) = '" & mv_strTableName & "' ORDER BY POSITION"
            'Dim v_strObjMsg As String = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SY_SEARCHFLD, gc_ActionInquiry, v_strCmdInquiry, v_strClause, )

            ''Dim v_ws As New BDSDelivery.BDSDelivery
            'Dim v_lngError As Long = mv_BDSDelivery.Message(v_strObjMsg)
            'If v_lngError <> ERR_SYSTEM_OK Then
            '    MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
            '    Exit Sub
            'End If

            PrepareSearchParams(UserLanguage, v_ds, mv_strCaption, mv_strEnCaption, mv_strCmdSql, mv_strObjName, mv_strFormName, _
                mv_arrSrFieldSrch, mv_arrSrFieldDisp, mv_arrSrFieldType, mv_arrSrFieldMask, mv_arrStFieldDefValue, _
                mv_arrSrFieldOperator, mv_arrSrFieldFormat, mv_arrSrFieldDisplay, mv_arrSrFieldWidth, _
                mv_arrSrSQLRef, mv_strKeyColumn, mv_strKeyFieldType, mv_intSearchNum, mv_strRefColumn, _
                mv_strRefFieldType, mv_strSrOderByCmd, mv_strTLTXCD, mv_strFldConditionChk)

            If mv_strObjName = "RPLIST" Then
                mv_strCmdSql = mv_strCmdSql.Replace("<$MODCODE>", ModuleCode)
                mv_strCaption = mv_strCaption.Replace("@", ModuleCode)
                mv_strEnCaption = mv_strEnCaption.Replace("@", ModuleCode)
                ModuleCode = "RP"
                chkPass.Enabled = False
                chkSearch.Enabled = False
            End If

            mv_strCmdSql = mv_strCmdSql.Replace("?LANGUAGE", Me.UserLanguage)
            mv_strCmdSql = mv_strCmdSql.Replace("?MEMBERFILTER", IIf(mv_lngAllMember = 0, " NVL(a.micode,'000') in " & mv_strMemberFilter, " 1=1 "))
            mv_strCmdSql = mv_strCmdSql.Replace("?COMEMBERFILTER", IIf(mv_lngAllMember = 0, " NVL(a.comicode,'000') in " & mv_strMemberFilter, " 1=1 "))
            mv_strCmdSql = mv_strCmdSql.Replace("?STOCKFILTER", IIf(mv_lngAllStock = 0, " NVL(a.sicode,'000') in " & mv_strStockFilter, " 1=1 "))
            mv_strCmdSql = mv_strCmdSql.Replace("?BRID", mv_strBranchId)
            mv_strCmdSql = mv_strCmdSql.Replace("?TLID", mv_strTellerId)
            mv_strCmdSql = mv_strCmdSql.Replace("?TLTXCD", mv_strPARENT_TLTXCD)
            mv_strCmdSql = mv_strCmdSql.Replace("?VSD_BRID", mv_strVsdBrid)
            mv_strCmdSql = mv_strCmdSql.Replace("?VSD_2_BRID", mv_strVsdBrid2)
            'If Sharp <> "" Then
            '    Dim v_intCount, v_intSharp As Integer
            '    Dim v_intBegin, v_intEnd As Integer
            '    Dim strSharp As String
            '    v_intCount = Sharp.Substring(0, Sharp.IndexOf(Chr(1)))
            '    For v_intSharp = 1 To v_intCount
            '        strSharp = ""
            '        v_intBegin = InStr(Sharp, "#" & Format(v_intSharp, "00") & Chr(1)) + 3
            '        v_intEnd = InStr(v_intBegin + 1, Sharp, Chr(1)) - v_intBegin - 1
            '        strSharp = Sharp.Substring(v_intBegin, v_intEnd)
            '        mv_strCmdSql = mv_strCmdSql.Replace("#" & Format(v_intSharp, "00"), strSharp)
            '    Next
            'End If

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

            'Load the last filter
            LoadLastSearch()

            If SearchOnInit Then
                OnSearch(IsLocalSearch, ModuleCode & "." & ObjectName)
            End If
            txtNumRow.Text = GetRowPage()
            mv_rowpage = CInt(txtNumRow.Text)

            'ButtonEnable(False)

            'If Not mv_strTLTXCD Is Nothing Then
            '    If mv_strTLTXCD.Trim.Length = 0 Then Me.btnExecute.Visible = False
            'End If
            If Not ObjectName Is Nothing Then
                If Me.ObjectName.Trim.Length = 0 Then Me.btnView.Visible = False
            End If

            'If Me.btnExecute.Visible Then
            '    If Not SearchGrid.Columns("__TICK") Is Nothing Then
            '        SearchGrid.Columns("__TICK").Visible = True
            '        SearchGrid.ContextMenu = Me.mnuGrid
            '    End If
            'End If
            Me.btnNext.Enabled = False
            Me.btnBack.Enabled = False

            If mv_strTableName = "TLLOG" Then
                btnExit.Visible = False
                btnExit.Enabled = False
            End If

            v_obj.CloseConnection()
            v_obj = Nothing
            'bangpv test đổi định dạng form search
            'Với các form đăng ký chứng khoán, đăng ký tổ chức phát hành, đăng ký tvlk, không chuyển định dạng sang vi-VN
            If Not (mv_strObjName = "RGSI" Or mv_strObjName = "RGIS" Or mv_strObjName = "RGMI") Then
                System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("vi-VN")
            Else
                System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US")
            End If
            'end bangpv
        Catch ex As Exception
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            '                                         & "Error code: System error!" & vbNewLine _
            '                                         & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            'MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
            Throw ex
        End Try
    End Sub

    Protected Overridable Function OnAddNew() As Int32
        If ShowForm(ExecuteFlag.AddNew) = System.Windows.Forms.DialogResult.OK Then
            OnSearch(IsLocalSearch, ModuleCode & "." & ObjectName)
        End If
    End Function

    Protected Overridable Function OnUpdate() As Int32
        'If Not (SearchGrid Is Nothing) Then
        '    mv_SelectedRow = SearchGrid.CurrentRow
        'End If
        If ShowForm(ExecuteFlag.Edit) = System.Windows.Forms.DialogResult.OK Then
            'OnSearch(IsLocalSearch, ModuleCode & "." & ObjectName)
            'If Not (SearchGrid Is Nothing) Then
            '    If SearchGrid.Enabled And SearchGrid.Visible Then
            '        Dim v_strCITYPE As String = Trim(CType(mv_SelectedRow, Xceed.Grid.DataRow).Cells("ACTYPE").Value).ToString
            '        'SearchGrid.CurrentRow = SearchGrid..SelectedRows(.Controls.fin..ro
            '        'For Each v_row As Xceed.Grid.Row In SearchGrid.ro

            '        'Next
            '    End If
            'End If
        End If
    End Function

    Protected Overridable Function OnExport() As Int32
        Try
            Dim v_dlgSave As New SaveFileDialog
            v_dlgSave.Filter = "Text files (*.txt)|*.txt|Excel files (*.xls)|*.xls|All files (*.*)|*.*"

            Dim v_res As DialogResult = v_dlgSave.ShowDialog(Me)
            If v_res = System.Windows.Forms.DialogResult.OK Then
                Dim v_strFileName As String = v_dlgSave.FileName
                If v_strFileName.ToUpper.EndsWith(".XLS") Then
                    If (SearchGrid.DataRows.Count > 0) Then
                        Export2Excel(v_strFileName)
                    End If
                    MsgBox(mv_ResourceManager.GetString("ExportSuccessful"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                Else
                    Dim v_strData As String
                    Dim v_streamWriter As New StreamWriter(v_strFileName, False, System.Text.Encoding.Unicode)

                    If (SearchGrid.DataRows.Count > 0) Then
                        'Write file's header
                        v_strData = String.Empty
                        For idx As Integer = 0 To SearchGrid.Columns.Count - 1
                            If SearchGrid.Columns(idx).Visible Then
                                v_strData &= SearchGrid.Columns(idx).Title & vbTab
                            End If
                        Next
                        v_streamWriter.WriteLine(v_strData)

                        'Write data
                        For i As Integer = 0 To SearchGrid.DataRows.Count - 1
                            v_strData = String.Empty

                            For j As Integer = 0 To SearchGrid.DataRows(i).Cells.Count - 1
                                If Not SearchGrid.DataRows(i).Cells(j).Value Is Nothing And SearchGrid.DataRows(i).Cells(j).Visible = True Then
                                    If Not SearchGrid.DataRows(i).Cells(j).Value.GetType Is GetType(System.Drawing.Icon) Then
                                        v_strData &= SearchGrid.DataRows(i).Cells(j).Value & vbTab
                                    Else
                                        v_strData &= "" & vbTab
                                    End If
                                End If
                            Next
                            'Write data to the file
                            v_streamWriter.WriteLine(v_strData)
                        Next
                    Else
                        MsgBox(mv_ResourceManager.GetString("NothingToExport"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                        Exit Function
                    End If

                    'Close StreamWriter
                    v_streamWriter.Close()

                    MsgBox(mv_ResourceManager.GetString("ExportSuccessful"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                End If
            End If
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                                       & "Error code: System error!" & vbNewLine _
                                                       & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Function

    Private Sub Export2Excel(ByVal pv_strFileName As String)
        Dim oApp As Microsoft.Office.Interop.Excel.Application
        Dim oWorkbook As Microsoft.Office.Interop.Excel.Workbook
        Dim oSheet As Microsoft.Office.Interop.Excel.Worksheet
        Dim v_oProcess = New ProcessForm(Me)
        Try
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US")
            Me.Cursor = Cursors.WaitCursor
            v_oProcess.StartProcessForm()
            v_oProcess.ChangeCaption(mv_ResourceManager.GetString("Exporting"))
            System.Windows.Forms.Application.DoEvents()
            oApp = New Microsoft.Office.Interop.Excel.Application()

            oApp.UserControl = True
            oApp.DisplayAlerts = False
            oWorkbook = oApp.Workbooks.Add()
            oSheet = oWorkbook.Sheets.Add()
            oSheet.Columns.AutoFit()

            Dim intRows As Integer
            Dim intColumn As Integer
            Dim strColumnPrefix As String = ""
            Dim strColumn As String = "A"
            Dim strCell As String
            Dim intTemp As Integer = Asc(strColumn(0))
            'Headers
            For intColumn = 0 To SearchGrid.Columns.Count - 1
                If SearchGrid.Columns(intColumn).Visible Then
                    strCell = strColumnPrefix & strColumn & "1"

                    If strColumn = "Z" Then
                        If strColumnPrefix = "" Then
                            strColumnPrefix = "A"
                        Else
                            strColumnPrefix = Chr(Asc(strColumnPrefix.Chars(0)) + 1).ToString()
                        End If
                        strColumn = Chr(Asc("C") - 1).ToString()
                    End If

                    oSheet.Range(strCell).Select()
                    oSheet.Range(strCell).NumberFormat = "@"
                    oSheet.Range(strCell).Value = SearchGrid.Columns(intColumn).Title

                    strColumn = Chr(Asc(strColumn.Chars(0)) + 1).ToString()
                End If
            Next

            'Fields
            For intRows = 0 To SearchGrid.DataRows.Count - 1
                strColumnPrefix = ""
                strColumn = "A"
                For intColumn = 0 To SearchGrid.Columns.Count - 1
                    If SearchGrid.Columns(intColumn).Visible Then
                        strCell = strColumnPrefix & strColumn & (intRows + 2).ToString()
                        If strColumn = "Z" Then
                            If strColumnPrefix = "" Then
                                strColumnPrefix = "A"
                            Else
                                strColumnPrefix = Chr(Asc(strColumnPrefix.Chars(0)) + 1).ToString()
                            End If
                            strColumn = Chr(Asc("A") - 1).ToString()
                        End If
                        oSheet.Range(strCell).Select()
                        If SearchGrid.DataRows(intRows).Cells(intColumn).Value Is Nothing Then
                            oSheet.Range(strCell).Value = ""
                        Else
                            If SearchGrid.DataRows(intRows).Cells(intColumn).Value.GetType Is GetType(System.String) Then
                                oSheet.Range(strCell).NumberFormat = "@"
                            ElseIf SearchGrid.DataRows(intRows).Cells(intColumn).Value.GetType Is GetType(System.String) Then
                                oSheet.Range(strCell).NumberFormat = "dd/mm/yyyy;@"
                            End If
                            If Not SearchGrid.DataRows(intRows).Cells(intColumn).Value.GetType Is GetType(System.Drawing.Icon) Then
                                oSheet.Range(strCell).Value = SearchGrid.DataRows(intRows).Cells(intColumn).Value
                            Else
                                oSheet.Range(strCell).Value = ""
                            End If
                        End If
                        strColumn = Chr(Asc(strColumn.Chars(0)) + 1).ToString()
                    End If
                Next
            Next

            oWorkbook.SaveAs(pv_strFileName, FileFormat:=XlWindowState.xlNormal, _
             Password:="", WriteResPassword:="", ReadOnlyRecommended:=False, _
             CreateBackup:=False)
            oApp.Quit()
            oApp = Nothing
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("vi-VN")
            v_oProcess.StopProcessForm()
            System.Windows.Forms.Application.DoEvents()
            Me.Cursor = Cursors.Default
        Catch ex As Exception
            v_oProcess.StopProcessForm()
            System.Windows.Forms.Application.DoEvents()
            Me.Cursor = Cursors.Default
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("vi-VN")
            Throw ex
        End Try
    End Sub

    Protected Overridable Function OnExecute() As Int32
        Dim v_strSQL, v_strObjMsg As String
        Dim v_xmlDocument As New Xml.XmlDocument, v_xmlDocumentData As New Xml.XmlDocument
        Dim v_nodeList As Xml.XmlNodeList
        Dim v_strValue, v_strFLDNAME, v_strFLDCD, v_strFLDCODE, v_strTLTXCD, v_strMODCODE, v_strFLDDEFVAL As String, i, j, v_intRow As Integer

        'Căn cứ vào SEARCHCODE để lấy mã giao dịch (TLTXCD) và nạp các giá trị mặc định cho trư?ng giao dịch FLDCD.
        v_strSQL = "SELECT APPMODULES.MODCODE, SEARCH.TLTXCD, SEARCHFLD.FIELDCODE, SEARCHFLD.FLDCD FROM APPMODULES, SEARCH, SEARCHFLD " & ControlChars.CrLf _
            & "WHERE SEARCH.SEARCHCODE=SEARCHFLD.SEARCHCODE AND APPMODULES.TXCODE=SUBSTR(SEARCH.TLTXCD,1,2) AND LENGTH(SEARCH.TLTXCD)=4 AND SEARCH.SEARCHCODE='" & mv_strTableName & "'"
        v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLTX, gc_ActionInquiry, v_strSQL)

        Proxy.Message(v_strObjMsg)

        v_xmlDocument.LoadXml(v_strObjMsg)
        v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")
        v_strFLDDEFVAL = String.Empty
        v_strMODCODE = String.Empty
        v_strTLTXCD = String.Empty
        If Not v_nodeList.Count = 0 Then
            'Nếu đây là màn hình tra cứu cho phép thực hiện giao dịch kế tiếp
            If Not SearchGrid Is Nothing Then
                If SearchGrid.DataRows.Count > 0 Then
                    For v_intRow = 0 To SearchGrid.DataRows.Count - 1 Step 1
                        If Not SearchGrid.DataRows(v_intRow) Is Nothing Then
                            If SearchGrid.DataRows(v_intRow).Cells("__TICK").Value = "X" Then
                                'Có được đánh dấu ch?n
                                For i = 0 To v_nodeList.Count - 1
                                    v_strFLDCODE = String.Empty
                                    v_strFLDCD = String.Empty
                                    For j = 0 To v_nodeList.Item(i).ChildNodes.Count - 1
                                        With v_nodeList.Item(i).ChildNodes(j)
                                            v_strValue = .InnerText.ToString
                                            v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                                            Select Case Trim(v_strFLDNAME)
                                                Case "MODCODE"
                                                    v_strMODCODE = Trim(v_strValue)
                                                Case "TLTXCD"
                                                    v_strTLTXCD = Trim(v_strValue)
                                                Case "FIELDCODE"
                                                    v_strFLDCODE = Trim(v_strValue)
                                                Case "FLDCD"
                                                    v_strFLDCD = Trim(v_strValue)
                                            End Select
                                        End With
                                    Next
                                    'Xác định giá trị của trư?ng dữ liệu
                                    If v_strFLDCD <> "" Then
                                        v_strValue = SearchGrid.DataRows(v_intRow).Cells(v_strFLDCODE).Value
                                        v_strValue = Replace(v_strValue, ".", "")
                                        v_strFLDDEFVAL = v_strFLDDEFVAL & "[" & v_strFLDCD & "." & v_strValue & "]"
                                    End If
                                Next

                                'Nạp và thực hiện giao dịch
                                SearchGrid.DataRows(v_intRow).Cells("__TICK").Value = String.Empty
                                'SetTransactForm()
                                'If v_strMODCODE <> "" And v_strTLTXCD <> "" Then
                                'mv_frmTransactScreen.ObjectName = v_strTLTXCD
                                'mv_frmTransactScreen.ModuleCode = v_strMODCODE
                                'mv_frmTransactScreen.LocalObject = gc_IsLocalMsg
                                'mv_frmTransactScreen.BranchId = Me.BranchId
                                'mv_frmTransactScreen.TellerId = Me.TellerId
                                'mv_frmTransactScreen.IpAddress = Me.IpAddress
                                'mv_frmTransactScreen.WsName = Me.WsName
                                'mv_frmTransactScreen.BusDate = Me.BusDate
                                'mv_frmTransactScreen.DefaultValue = v_strFLDDEFVAL
                                'mv_frmTransactScreen.AutoClosedWhenOK = True
                                'mv_frmTransactScreen.ShowDialog()
                                'If mv_frmTransactScreen.CancelClick Then
                                'OnSearch(IsLocalSearch, ModuleCode & "." & ObjectName, mv_intpage)
                                'Exit Function
                                'End If
                                'mv_frmTransactScreen.Dispose()
                                'Reset lại giá trị
                                'v_strFLDDEFVAL = String.Empty
                                'End If
                            End If
                        End If
                    Next v_intRow
                End If
                'Refresh lại màn hình
                OnSearch(IsLocalSearch, ModuleCode & "." & ObjectName, mv_intpage)
            End If
        End If
    End Function

    Protected Overridable Function ShowForm(ByVal pv_intExecFlag As Integer) As Boolean
        Select Case pv_intExecFlag
            Case ExecuteFlag.AddNew
                ssbFlag.Text = mv_ResourceManager.GetString("ExecuteFlag_AddNew")
            Case ExecuteFlag.View
                ssbFlag.Text = mv_ResourceManager.GetString("ExecuteFlag_View")
            Case ExecuteFlag.Edit
                ssbFlag.Text = mv_ResourceManager.GetString("ExecuteFlag_Edit")
            Case ExecuteFlag.Delete
                ssbFlag.Text = mv_ResourceManager.GetString("ExecuteFlag_Delete")
        End Select
    End Function

    Protected Overridable Function OnSearch(Optional ByVal pv_strIsLocal As String = "", Optional ByVal pv_strModule As String = "", Optional ByVal page As Int32 = 1) As Int32
        Dim i As Integer
        Dim v_xColumn As Xceed.Grid.Column, v_strFLDNAME As String
        Dim v_oProcess As New ProcessForm(Me.ParentForm)

        Try
            'Disable search button
            'Dim v_strMethodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
            'mv_lwLogWriter.StartWriteLog(gc_MODULE_CLIENT, ModuleCode, "frmSearch", "Onsearch", _
            '                             "192.168.134.114", "Bank-Hanm5", "210", "Tuanpa", "abc")
            v_oProcess.StartProcessForm()
            Me.btnSearch.Enabled = False

            If txtNumRow.Text = "" Then
                mv_rowpage = 100
            Else
                mv_rowpage = CInt("0" & txtNumRow.Text)
            End If

            Cursor = Cursors.WaitCursor

            'Update status bar
            ssbFlag.Text = mv_ResourceManager.GetString("Searching")
            'ssbPanelExecFlag.Text = String.Empty
            mv_strSearchFilter = String.Empty

            Dim v_intCount As Integer

            v_intCount = 0

            Dim v_strCondition As String = ""
            Dim v_strPartition As String = ""

            For i = 0 To lstCondition.Items.Count - 1
                If lstCondition.GetItemChecked(i) Then
                    v_strCondition &= hFilter(lstCondition.Items(i).ToString()) & "#"

                    If Not mv_hPartition(lstCondition.Items(i).ToString()) Is Nothing And mv_strTXDATE = "" Then
                        v_strPartition &= mv_hPartition(lstCondition.Items(i).ToString()) & "$"
                    End If
                End If
            Next i

            If mv_strTXDATE <> "" Then
                v_strPartition = "=|" & mv_strTXDATE & "$"
                v_strCondition &= "TXDATE|=|" & mv_strTXDATE & "|P#"
            End If

            If (pv_strIsLocal <> "") And (pv_strModule <> "") Then
                Dim v_intFrom, v_intTo As Int32
                Dim v_strRowNum As String = ""

                v_intTo = page * mv_rowpage
                v_intFrom = v_intTo + 1 - mv_rowpage
                If mv_rowpage > 0 Then
                    v_strRowNum = v_intFrom & "|" & v_intTo
                End If

                v_strRowNum &= "|0"

                Dim v_lstPartition As String
                Dim v_strMinDate As String
                Dim v_strMaxDate As String
                If v_strPartition <> "" Then
                    v_lstPartition = GetPartition(v_strPartition)
                    If v_lstPartition = "" Then
                        Me.Cursor = Cursors.Default
                        v_oProcess.StopProcessForm()
                        System.Windows.Forms.Application.DoEvents()
                        MsgBox(mv_ResourceManager.GetString("ErrPartition"), MsgBoxStyle.Critical, gc_ApplicationTitle)
                        Me.btnNext.Enabled = True
                        Me.btnBack.Enabled = True
                        'Enable search button
                        Me.btnSearch.Enabled = True
                        Exit Function
                    End If
                    v_strMinDate = v_lstPartition.Split("|")(0)

                    If CheckPassDate(v_strMinDate, "01/01/2000") Then
                        Me.Cursor = Cursors.Default
                        v_oProcess.StopProcessForm()
                        System.Windows.Forms.Application.DoEvents()
                        MsgBox(mv_ResourceManager.GetString("ErrPartition"), MsgBoxStyle.Critical, gc_ApplicationTitle)
                        Me.btnNext.Enabled = True
                        Me.btnBack.Enabled = True
                        'Enable search button
                        Me.btnSearch.Enabled = True
                        Exit Function
                    End If
                    v_strMaxDate = v_lstPartition.Split("|")(1)

                    If CheckPassDate(BusDate, v_strMaxDate) Then
                        Me.Cursor = Cursors.Default
                        v_oProcess.StopProcessForm()
                        System.Windows.Forms.Application.DoEvents()
                        MsgBox(mv_ResourceManager.GetString("ErrPartition"), MsgBoxStyle.Critical, gc_ApplicationTitle)
                        Me.btnNext.Enabled = True
                        Me.btnBack.Enabled = True
                        'Enable search button
                        Me.btnSearch.Enabled = True
                        Exit Function
                    End If

                End If

                'Hanm5 them doan code check da nhap dieu kien thanh vien luu ky chua
                If mv_strFldConditionChk <> "" Then
                    Dim v_arrFldConditionChk() As String = mv_strFldConditionChk.Split("|")
                    Dim v_strFldErr As String = ""
                    For v_int As Integer = 0 To v_arrFldConditionChk.Count - 1
                        If mv_arrSrFieldSrch.Contains(Trim(v_arrFldConditionChk(v_int))) Then
                            If v_strCondition.IndexOf(Trim(v_arrFldConditionChk(v_int)) & "|") < 0 Then
                                v_strFldErr = v_strFldErr & "," & mv_arrSrFieldDisp(mv_arrSrFieldSrch.IndexOf(mv_arrSrFieldSrch, Trim(v_arrFldConditionChk(v_int))))
                            End If
                        End If
                    Next
                    If v_strFldErr <> "" Then
                        v_strFldErr = Mid(v_strFldErr, 2, v_strFldErr.Length - 1)
                        Me.Cursor = Cursors.Default
                        v_oProcess.StopProcessForm()
                        System.Windows.Forms.Application.DoEvents()
                        MsgBox("Với chức năng tra cứu này, bạn phải chọn tiêu chí tìm kiếm theo " & v_strFldErr, MsgBoxStyle.Critical, gc_ApplicationTitle)
                        Me.btnNext.Enabled = True
                        Me.btnBack.Enabled = True
                        'Enable search button
                        Me.btnSearch.Enabled = True
                        Exit Function
                    End If
                End If

                System.Windows.Forms.Application.DoEvents()

                Dim v_strObjMsg As String

                'Select Case v_intSearch
                '    Case 0
                '        v_strObjMsg = BuildXMLObjMsg(Me.BusDate, Me.BranchId, v_blnAll, Me.TellerId, gc_IsLocalMsg, gc_MsgTypeObj, pv_strModule, _
                '                                    gc_ActionInquiry, mv_strCmdSql, v_strCondition, v_strRowNum, True, StockFilter & "|" & MemberFilter, False)
                '    Case 1
                '        v_strObjMsg = BuildXMLObjMsg(Me.BusDate, Me.BranchId, v_blnAll, Me.TellerId, gc_IsLocalMsg, gc_MsgTypeObj, pv_strModule, _
                '                                    gc_ActionInquiry, mv_strCmdSql, v_strCondition, v_strRowNum, True, StockFilter & "|" & MemberFilter, True)
                '    Case 2
                'bangpv: them de check database firewall 
                'v_lngError = mv_BDSDelivery.Message(v_strObjMsg)
                Dim v_strSQL As String = "select authid, tltxcd from tlcaauth where tltxcd = '" & ObjectName _
                           & "' and authid in (select grpid from tlgrpusers where tlid ='" & TellerId & "')"

                Dim v_strObjMsg1 As String = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLGROUPS, gc_ActionInquiry, v_strSQL)
                Dim v_lngError As Long = Proxy.Message(v_strObjMsg1)
                If v_lngError <> ERR_SYSTEM_OK Then
                    Me.Cursor = Cursors.Default
                    v_oProcess.StopProcessForm()
                    System.Windows.Forms.Application.DoEvents()
                    Me.btnNext.Enabled = True
                    Me.btnBack.Enabled = True
                    'Enable search button
                    Me.btnSearch.Enabled = True
                    MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                    Exit Function
                End If
                'end bangpv 
                v_strObjMsg = BuildXMLObjMsg(Me.BusDate, Me.BranchId, , Me.TellerId, IsLocalSearch, gc_MsgTypeObj, pv_strModule, _
                                        gc_ActionInquiry, mv_strCmdSql, v_strCondition, v_strRowNum, AutoID, StockFilter & "|" & MemberFilter, _
                                        v_lstPartition, , Me.VsdBrid, Me.VsdBrid2)
                'End Select

                Client.isActive = True
                Client.Action = "Begin Search: " & Me.Text
                Proxy.SendAction(Client)
                v_lngError = Proxy.Message(v_strObjMsg)
                Client.Action = "End Search: " & Me.Text
                Client.isActive = False
                Proxy.SendAction(Client)

                If v_lngError <> ERR_SYSTEM_OK Then
                    Me.Cursor = Cursors.Default
                    v_oProcess.StopProcessForm()
                    System.Windows.Forms.Application.DoEvents()
                    Me.btnNext.Enabled = True
                    Me.btnBack.Enabled = True
                    'Enable search button
                    Me.btnSearch.Enabled = True
                    MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                    Exit Function
                End If

                ''Kiểm tra thông tin và xử lý lỗi (nếu có) từ message trả về
                'Dim v_strErrorSource As String = "", v_strErrorMessage As String = ""
                'Dim v_lngErrorCode As Long

                'GetErrorFromMessage(v_strObjMsg, v_strErrorSource, v_lngErrorCode, v_strErrorMessage)
                'If v_lngErrorCode <> 0 Then
                '    'Update mouse pointer
                '    Cursor = Cursors.Default
                '    v_thread.Abort()
                '    btnSearch.Enabled = True
                '    MsgBox(mv_ResourceManager.GetString("StoresErr"), MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, Me.Text)
                '    Exit Function
                'End If

                Dim v_xml As New XmlDocumentEx
                v_xml.LoadXml(v_strObjMsg)

                Dim v_nodeList As Xml.XmlNodeList

                v_nodeList = v_xml.SelectNodes("/ObjectMessage/SysSearchStatusMsg")
                Dim v_strSearchStatus As String = ""
                If v_nodeList.Count > 0 Then
                    Dim v_strSearchFldName, v_strSearchValue As String
                    For j As Integer = 0 To v_nodeList.Item(0).ChildNodes.Count - 1
                        With v_nodeList.Item(0).ChildNodes(j)
                            v_strSearchFldName = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                            v_strSearchValue = .InnerText.ToString
                            If Trim(v_strSearchFldName) = "SYS_SEARCH_STATUS" Then
                                v_strSearchStatus = v_nodeList.Item(0).ChildNodes(j).InnerText
                            End If
                        End With
                    Next
                End If

                If v_strSearchStatus = "0" Then
                    v_oProcess.StopProcessForm()
                    MsgBox(mv_ResourceManager.GetString("SysSearchStatus"), MsgBoxStyle.Critical, "Thông báo")
                    Me.btnSearch.Enabled = True
                    Cursor = Cursors.Default
                    v_oProcess.StopProcessForm()
                    Exit Function
                Else
                    Dim v_attrColl As Xml.XmlAttributeCollection = v_xml.DocumentElement.Attributes
                    mv_lngRowCount = CLng(CType(v_attrColl.GetNamedItem(gc_AtributeFUNCNAME), Xml.XmlAttribute).Value)
                    ssbFlag.Text = mv_ResourceManager.GetString("Searched") & mv_lngRowCount
                    Me.FULLDATA = v_strObjMsg

                    'bangpv test đổi định dạng form search
                    'System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("vi-VN")
                    'end bangpv
                    If ObjectName = "TLLOG" Then
                        mv_strGlobalAuthString = ""
                    End If

                    'Format data in search grid
                    For Each v_xColumn In SearchGrid.Columns
                        v_strFLDNAME = UCase(Trim(v_xColumn.FieldName))
                        For i = 0 To mv_arrSrFieldSrch.GetLength(0) - 1
                            If UCase(mv_arrSrFieldSrch(i)) = v_strFLDNAME Then
                                v_xColumn.FormatSpecifier = mv_arrSrFieldFormat(i)
                                Exit For
                            End If
                        Next
                    Next

                    'Fill data into search grid
                    'FillDataGrid(SearchGrid, v_strObjMsg, c_ResourceManager & UserLanguage, mv_strTableName, , v_intFrom, v_intTo, mv_lngRowCount)
                    FillDataGrid(SearchGrid, TellerId, v_strObjMsg, c_ResourceManager & UserLanguage, mv_strTableName, , v_intFrom, v_intTo, mv_lngRowCount, imgTool1)
                End If
            End If

            'Update mouse pointer
            Cursor = Cursors.Default

            Me.btnNext.Enabled = True
            Me.btnBack.Enabled = True
            'Enable search button
            Me.btnSearch.Enabled = True
            'If Mid(AuthCode, 6, 1) = "Y" Then
            '    Me.btnSearch.Enabled = (mv_intRowCount <> 0)
            'End If
            'If v_oProcess.Status Then
            v_oProcess.StopProcessForm()
            System.Windows.Forms.Application.DoEvents()
            'End If
            GC.Collect()
        Catch ex As Exception
            Me.btnSearch.Enabled = True
            Cursor = Cursors.Default
            v_oProcess.StopProcessForm()
            System.Windows.Forms.Application.DoEvents()
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                                     & "Error code: System error!" & vbNewLine _
                                                     & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        Finally
        End Try
    End Function

    Protected Overridable Function OnExportFromDB(Optional ByVal pv_strIsLocal As String = "", Optional ByVal pv_strModule As String = "", Optional ByVal pv_intExportRowFrom As Int32 = 1, Optional ByVal pv_intExportRowTo As Int32 = 1) As Int32
        Dim i As Integer
        Dim v_xColumn As Xceed.Grid.Column, v_strFLDNAME As String
        Dim v_oProcess As New ProcessForm(Me.ParentForm)

        Try
            'Disable search button

            v_oProcess.StartProcessForm()
            Me.btnSearch.Enabled = False

            Cursor = Cursors.WaitCursor

            'Update status bar
            ssbFlag.Text = mv_ResourceManager.GetString("Searching")
            'ssbPanelExecFlag.Text = String.Empty
            mv_strSearchFilter = String.Empty

            Dim v_intCount As Integer

            v_intCount = 0

            Dim v_strCondition As String = ""
            Dim v_strPartition As String = ""

            For i = 0 To lstCondition.Items.Count - 1
                If lstCondition.GetItemChecked(i) Then
                    v_strCondition &= hFilter(lstCondition.Items(i).ToString()) & "#"

                    If Not mv_hPartition(lstCondition.Items(i).ToString()) Is Nothing And mv_strTXDATE = "" Then
                        v_strPartition &= mv_hPartition(lstCondition.Items(i).ToString()) & "$"
                    End If
                End If
            Next i

            If mv_strTXDATE <> "" Then
                v_strPartition = "=|" & mv_strTXDATE & "$"
                v_strCondition &= "TXDATE|=|" & mv_strTXDATE & "|P#"
            End If

            If (pv_strIsLocal <> "") And (pv_strModule <> "") Then
                Dim v_strRowNum As String = ""
                v_strRowNum = pv_intExportRowFrom & "|" & pv_intExportRowTo & "|1"

                Dim v_lstPartition As String
                Dim v_strMinDate As String
                Dim v_strMaxDate As String
                If v_strPartition <> "" Then
                    v_lstPartition = GetPartition(v_strPartition)
                    If v_lstPartition = "" Then
                        Me.Cursor = Cursors.Default
                        v_oProcess.StopProcessForm()
                        System.Windows.Forms.Application.DoEvents()
                        MsgBox(mv_ResourceManager.GetString("ErrPartition"), MsgBoxStyle.Critical, gc_ApplicationTitle)
                        Me.btnNext.Enabled = True
                        Me.btnBack.Enabled = True
                        'Enable search button
                        Me.btnSearch.Enabled = True
                        Exit Function
                    End If
                    v_strMinDate = v_lstPartition.Split("|")(0)

                    If CheckPassDate(v_strMinDate, "01/01/2000") Then
                        Me.Cursor = Cursors.Default
                        v_oProcess.StopProcessForm()
                        System.Windows.Forms.Application.DoEvents()
                        MsgBox(mv_ResourceManager.GetString("ErrPartition"), MsgBoxStyle.Critical, gc_ApplicationTitle)
                        Me.btnNext.Enabled = True
                        Me.btnBack.Enabled = True
                        'Enable search button
                        Me.btnSearch.Enabled = True
                        Exit Function
                    End If
                    v_strMaxDate = v_lstPartition.Split("|")(1)

                    If CheckPassDate(BusDate, v_strMaxDate) Then
                        Me.Cursor = Cursors.Default
                        v_oProcess.StopProcessForm()
                        System.Windows.Forms.Application.DoEvents()
                        MsgBox(mv_ResourceManager.GetString("ErrPartition"), MsgBoxStyle.Critical, gc_ApplicationTitle)
                        Me.btnNext.Enabled = True
                        Me.btnBack.Enabled = True
                        'Enable search button
                        Me.btnSearch.Enabled = True
                        Exit Function
                    End If

                End If

                'Hanm5 them doan code check da nhap dieu kien thanh vien luu ky chua
                If mv_strFldConditionChk <> "" Then
                    Dim v_arrFldConditionChk() As String = mv_strFldConditionChk.Split("|")
                    Dim v_strFldErr As String = ""
                    For v_int As Integer = 0 To v_arrFldConditionChk.Count - 1
                        If mv_arrSrFieldSrch.Contains(Trim(v_arrFldConditionChk(v_int))) Then
                            If v_strCondition.IndexOf(Trim(v_arrFldConditionChk(v_int)) & "|") < 0 Then
                                v_strFldErr = v_strFldErr & "," & mv_arrSrFieldDisp(mv_arrSrFieldSrch.IndexOf(mv_arrSrFieldSrch, Trim(v_arrFldConditionChk(v_int))))
                            End If
                        End If
                    Next
                    If v_strFldErr <> "" Then
                        v_strFldErr = Mid(v_strFldErr, 2, v_strFldErr.Length - 1)
                        Me.Cursor = Cursors.Default
                        v_oProcess.StopProcessForm()
                        System.Windows.Forms.Application.DoEvents()
                        MsgBox("Với chức năng tra cứu này, bạn phải chọn tiêu chí tìm kiếm theo " & v_strFldErr, MsgBoxStyle.Critical, gc_ApplicationTitle)
                        Me.btnNext.Enabled = True
                        Me.btnBack.Enabled = True
                        'Enable search button
                        Me.btnSearch.Enabled = True
                        Exit Function
                    End If
                End If

                System.Windows.Forms.Application.DoEvents()
                Dim v_strObjMsg As String
                v_strObjMsg = BuildXMLObjMsg(Me.BusDate, Me.BranchId, , Me.TellerId, IsLocalSearch, gc_MsgTypeObj, pv_strModule, _
                                        gc_ActionInquiry, mv_strCmdSql, v_strCondition, v_strRowNum, AutoID, StockFilter & "|" & MemberFilter, _
                                        v_lstPartition, , Me.VsdBrid, Me.VsdBrid2)
                'End Select

                Dim v_lngError As Long = Proxy.Message(v_strObjMsg)
                If v_lngError <> ERR_SYSTEM_OK Then
                    Me.Cursor = Cursors.Default
                    v_oProcess.StopProcessForm()
                    System.Windows.Forms.Application.DoEvents()
                    Me.btnNext.Enabled = True
                    Me.btnBack.Enabled = True
                    'Enable search button
                    Me.btnSearch.Enabled = True
                    MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                    Exit Function
                End If

                Dim v_xml As New XmlDocumentEx
                v_xml.LoadXml(v_strObjMsg)

                Dim v_nodeList As Xml.XmlNodeList

                v_nodeList = v_xml.SelectNodes("/ObjectMessage/SysSearchStatusMsg")
                Dim v_strSearchStatus As String = ""
                If v_nodeList.Count > 0 Then
                    Dim v_strSearchFldName, v_strSearchValue As String
                    For j As Integer = 0 To v_nodeList.Item(0).ChildNodes.Count - 1
                        With v_nodeList.Item(0).ChildNodes(j)
                            v_strSearchFldName = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                            v_strSearchValue = .InnerText.ToString
                            If Trim(v_strSearchFldName) = "SYS_SEARCH_STATUS" Then
                                v_strSearchStatus = v_nodeList.Item(0).ChildNodes(j).InnerText
                            End If
                        End With
                    Next
                End If

                If v_strSearchStatus = "0" Then
                    v_oProcess.StopProcessForm()
                    MsgBox(mv_ResourceManager.GetString("SysSearchStatus"), MsgBoxStyle.Critical, "Thông báo")
                    Me.btnSearch.Enabled = True
                    Cursor = Cursors.Default
                    System.Windows.Forms.Application.DoEvents()
                    GC.Collect()
                    Exit Function
                Else
                    v_nodeList = v_xml.SelectNodes("/ObjectMessage/ExportMsg")
                    Dim v_strExpResult As String = ""
                    If v_nodeList.Count > 0 Then
                        Dim v_strExpFldName, v_strExpValue As String
                        For j As Integer = 0 To v_nodeList.Item(0).ChildNodes.Count - 1
                            With v_nodeList.Item(0).ChildNodes(j)
                                v_strExpFldName = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                                v_strExpValue = .InnerText.ToString
                                If Trim(v_strExpFldName) = "EXPORT_RESULT" Then
                                    v_strExpResult = v_nodeList.Item(0).ChildNodes(j).InnerText
                                End If
                            End With
                        Next
                    End If

                    If v_strExpResult <> "" Then
                        'Clear SearchGrid
                        SearchGrid.DataRows.Clear()
                        Dim v_arrExpResult() As String = v_strExpResult.Split("|")
                        Dim v_frmFTPClient As New frmFTPClient
                        v_frmFTPClient.ServerAddress = v_arrExpResult(2)
                        v_frmFTPClient.ServerPort = v_arrExpResult(3)
                        v_frmFTPClient.UserName = v_arrExpResult(4)
                        v_frmFTPClient.Password = v_arrExpResult(5)
                        v_frmFTPClient.lbMessage.Text = "Kết quả kết xuất được lưu tại địa chỉ " & v_arrExpResult(1) & " trên máy chủ"
                        v_frmFTPClient.RemotePath = Replace(v_arrExpResult(1), v_arrExpResult(0), "")
                        v_oProcess.StopProcessForm()
                        v_frmFTPClient.ShowDialog()
                    End If
                End If
            End If
            'Update mouse pointer
            Cursor = Cursors.Default

            Me.btnNext.Enabled = True
            Me.btnBack.Enabled = True
            'Enable search button
            Me.btnSearch.Enabled = True
            v_oProcess.StopProcessForm()
            System.Windows.Forms.Application.DoEvents()
            GC.Collect()
        Catch ex As Exception
            Me.btnSearch.Enabled = True
            Cursor = Cursors.Default
            v_oProcess.StopProcessForm()
            System.Windows.Forms.Application.DoEvents()
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                                     & "Error code: System error!" & vbNewLine _
                                                     & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        Finally

        End Try
    End Function

    Private Function GetPartition(ByVal pv_lstDate As String) As String
        Dim v_strDate As String
        Dim v_strOperator As String
        Dim v_strMaxDate, v_strMinDate As String
        Dim v_arrDate() As String = pv_lstDate.Split("$")
        v_strMaxDate = BusDate
        v_strMinDate = "01/01/2000"
        For i As Integer = 0 To v_arrDate.Length - 2
            v_strOperator = v_arrDate(i).Split("|")(0)
            v_strDate = v_arrDate(i).Split("|")(1)
            Select Case v_strOperator
                Case ">", ">="
                    If Not CheckPassDate(v_strDate, v_strMinDate) Then
                        v_strMinDate = v_strDate
                    End If
                Case "<", "<="
                    If CheckPassDate(v_strDate, v_strMaxDate) Then
                        v_strMaxDate = v_strDate
                    End If
                Case "="
                    If CheckPassDate(v_strDate, v_strMaxDate) Then
                        Return v_strDate & "|" & v_strMaxDate
                    Else
                        Return v_strMaxDate & "|" & v_strDate
                    End If
            End Select

            If Not CheckPassDate(v_strMinDate, v_strMaxDate) Then
                Return ""
            End If
        Next
        Return v_strMinDate & "|" & v_strMaxDate
    End Function

    Public Sub OnSearchP()
        OnSearch(IsLocalSearch, ModuleCode & "." & ObjectName)
    End Sub

    Protected Overridable Function OnQuery() As Int32
        Try
            Dim v_strView As String
            If Len(Trim(AuthString)) > 0 Then
                v_strView = Mid(Trim(AuthString), 1, 1)
                If v_strView = "Y" Then
                    ShowForm(ExecuteFlag.View)
                Else
                    Return ERR_SYSTEM_OK
                End If
            End If
            Return 1
        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Public Overridable Sub OnClose()
        If Me.IsLookup = "Y" Then
            'Nếu là form search dùng để lookup thì trả v? giá trị tìm kiếm
            If SearchGrid.DataRows.Count > 0 Then
                If Not SearchGrid.CurrentRow Is Nothing Then
                    If Not (SearchGrid.CurrentRow Is SearchGrid.FixedFooterRows.Item(0)) Then
                        ReturnValue = CType(SearchGrid.CurrentRow, Xceed.Grid.DataRow).Cells(mv_strKeyColumn).Value
                        If Len(mv_strRefColumn) > 0 Then
                            RefValue = CType(SearchGrid.CurrentRow, Xceed.Grid.DataRow).Cells(mv_strRefColumn).Value
                        Else
                            RefValue = String.Empty
                        End If
                        'If Not CType(SearchGrid.CurrentRow, Xceed.Grid.DataRow).Cells(gc_AtributeMICODE).Value Is Nothing Then
                        '    MICODE = CType(SearchGrid.CurrentRow, Xceed.Grid.DataRow).Cells(gc_AtributeMICODE).Value
                        'End If

                        'If Not CType(SearchGrid.CurrentRow, Xceed.Grid.DataRow).Cells(gc_AtributeSICODE).Value Is Nothing Then
                        '    SICODE = CType(SearchGrid.CurrentRow, Xceed.Grid.DataRow).Cells(gc_AtributeSICODE).Value
                        'End If

                        'If Not CType(SearchGrid.CurrentRow, Xceed.Grid.DataRow).Cells(gc_AtributeCOMICODE).Value Is Nothing Then
                        '    COMICODE = CType(SearchGrid.CurrentRow, Xceed.Grid.DataRow).Cells(gc_AtributeCOMICODE).Value
                        'End If

                    End If
                End If
            End If
        Else
            If Me.TableName = "TRADING_RESULT" And Me.ModuleCode = "SA" Then
                'Nếu là form search dùng để tim kiem trong trading_result thì trả v? giá trị tìm kiếm
                If SearchGrid.DataRows.Count > 0 Then
                    If Not SearchGrid.CurrentRow Is Nothing Then
                        If Not (SearchGrid.CurrentRow Is SearchGrid.FixedFooterRows.Item(0)) Then
                            mv_strCONFIRM_NO = CType(SearchGrid.CurrentRow, Xceed.Grid.DataRow).Cells("CONFIRM_NO").Value
                            mv_strB_CUSTODYCD = CType(SearchGrid.CurrentRow, Xceed.Grid.DataRow).Cells("B_ACCOUNT_NO").Value
                            mv_strS_CUSTODYCD = CType(SearchGrid.CurrentRow, Xceed.Grid.DataRow).Cells("S_ACCOUNT_NO").Value
                            mv_strSEC_CODE = CType(SearchGrid.CurrentRow, Xceed.Grid.DataRow).Cells("SEC_CODE").Value
                            mv_intQUANTITY = CType(SearchGrid.CurrentRow, Xceed.Grid.DataRow).Cells("QUANTITY").Value
                            mv_intB_QUANTITY = CType(SearchGrid.CurrentRow, Xceed.Grid.DataRow).Cells("MATCHED_BQTTY").Value
                            mv_intS_QUANTITY = CType(SearchGrid.CurrentRow, Xceed.Grid.DataRow).Cells("MATCHED_SQTTY").Value
                            mv_dblPRICE = CType(SearchGrid.CurrentRow, Xceed.Grid.DataRow).Cells("PRICE").Value
                            mv_strMATCH_DATE = CType(SearchGrid.CurrentRow, Xceed.Grid.DataRow).Cells("TRADING_DATE").Value
                            v_strS_ACCOUNT_NO = CType(SearchGrid.CurrentRow, Xceed.Grid.DataRow).Cells("S_ACCOUNT_NO").Value
                            v_strB_ACCOUNT_NO = CType(SearchGrid.CurrentRow, Xceed.Grid.DataRow).Cells("B_ACCOUNT_NO").Value
                            v_strS_ORDER_NO = CType(SearchGrid.CurrentRow, Xceed.Grid.DataRow).Cells("S_ORDER_NO").Value
                            v_strB_ORDER_NO = CType(SearchGrid.CurrentRow, Xceed.Grid.DataRow).Cells("B_ORDER_NO").Value
                        End If
                    End If
                End If
            Else
                'Ghi nhận lại đi?u kiện tìm kiếm lần cuối cùng
                'SaveLastSearch()
            End If
        End If
        Me.Close()
    End Sub


    Protected Overridable Function OnDelete(Optional ByVal pv_strIsLocal As String = "", Optional ByVal pv_strModule As String = "") As Int32
        Dim v_strKeyFieldName, v_strKeyFieldValue As String
        Dim v_strClause As String = ""

        Try
            If (SearchGrid.CurrentRow Is Nothing) Then
                Cursor = Cursors.Default
                MsgBox(mv_ResourceManager.GetString("NotSelected"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, Me.Text)
                Exit Function
            End If
            If MsgBox(mv_ResourceManager.GetString("DelConfirm"), MsgBoxStyle.Question + MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.Yes Then
                'Update mouse pointer
                Cursor = Cursors.WaitCursor

                If (pv_strIsLocal <> "") And (pv_strModule <> "") Then
                    If Not (SearchGrid.CurrentRow Is Nothing) Then
                        If Not (SearchGrid.CurrentRow Is SearchGrid.FixedFooterRows.Item(0)) Then
                            v_strKeyFieldName = CType(SearchGrid.CurrentRow, Xceed.Grid.DataRow).Cells(mv_strKeyColumn).FieldName
                            v_strKeyFieldValue = CType(SearchGrid.CurrentRow, Xceed.Grid.DataRow).Cells(mv_strKeyColumn).Value
                            v_strKeyFieldValue = Replace(v_strKeyFieldValue, ".", "")
                            Select Case KeyFieldType
                                Case "D"
                                    v_strClause = v_strKeyFieldName & " = TO_DATE('" & v_strKeyFieldValue & "', '" & gc_FORMAT_DATE & "')"
                                Case "N"
                                    v_strClause = v_strKeyFieldName & " = " & v_strKeyFieldValue
                                Case "C"
                                    v_strClause = v_strKeyFieldName & " = '" & v_strKeyFieldValue & "'"
                            End Select

                            Dim v_strObjMsg As String = BuildXMLObjMsg(, BranchId, , TellerId, gc_IsNotLocalMsg, gc_MsgTypeObj, pv_strModule, gc_ActionDelete, , v_strClause)
                            'Dim v_ws As New BDSDelivery.BDSDelivery

                            Proxy.Message(v_strObjMsg)

                            'Kiểm tra thông tin và xử lý lỗi (nếu có) từ message trả v?
                            Dim v_strErrorSource As String = "", v_strErrorMessage As String = ""
                            Dim v_lngErrorCode As Long

                            GetErrorFromMessage(v_strObjMsg, v_strErrorSource, v_lngErrorCode, v_strErrorMessage)
                            If v_lngErrorCode <> 0 Then
                                'Update mouse pointer
                                Cursor = Cursors.Default
                                Select Case v_lngErrorCode
                                    Case ERR_SA_GRP_HAS_CHILD
                                        MsgBox(mv_ResourceManager.GetString("RR_SA_GRP_HAS_CHILD"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, Me.Text)
                                        Exit Function
                                    Case ERR_RG_IS_HAS_SI
                                        MsgBox(mv_ResourceManager.GetString("ERR_RG_IS_HAS_SI"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, Me.Text)
                                        Exit Function
                                    Case ERR_SA_TL_HAS_CHILD
                                        MsgBox(mv_ResourceManager.GetString("ERR_SA_TL_HAS_CHILD"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, Me.Text)
                                        Exit Function
                                    Case ERR_SA_BR_HAS_USER
                                        MsgBox(mv_ResourceManager.GetString("ERR_SA_BR_HAS_USER"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, Me.Text)
                                        Exit Function
                                    Case ERR_RG_SI_HAS_TRANS
                                        MsgBox(mv_ResourceManager.GetString("ERR_RG_SI_HAS_TRANS"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, Me.Text)
                                        Exit Function
                                    Case ERR_RG_MI_HAS_TRANS
                                        MsgBox(mv_ResourceManager.GetString("ERR_RG_MI_HAS_TRANS"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, Me.Text)
                                        Exit Function
                                    Case Else
                                        MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                                        Exit Function
                                End Select
                            End If

                            'Remove dòng dữ liệu đã xoá kh?i grid
                            SearchGrid.CurrentRow.Remove()
                        Else
                            'Update mouse pointer
                            Cursor = Cursors.Default
                            MsgBox(mv_ResourceManager.GetString("Footer"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, Me.Text)
                            Exit Function
                        End If
                    Else
                        'Update mouse pointer
                        Cursor = Cursors.Default
                        MsgBox(mv_ResourceManager.GetString("NotSelected"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, Me.Text)
                        Exit Function
                    End If
                End If

                'Đồng bộ lại thông tin
                'OnSearch(IsLocalSearch, ModuleCode & "." & ObjectName)

                'Update mouse pointer
                Cursor = Cursors.Default
                MsgBox(mv_ResourceManager.GetString("DelSuccess"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, Me.Text)
            End If
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                                     & "Error code: System error!" & vbNewLine _
                                                     & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Function

    Protected Function GetMaxRowSearch() As Int32
        Dim v_strCmdInquiry As String
        Dim v_strMaxRowSearch As String = String.Empty
        v_strCmdInquiry = "select VARVALUE from SYSVAR where VARNAME='MAX_ROW_SEARCH'"
        Dim v_strObjMsg As String = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLGROUPS, gc_ActionInquiry, v_strCmdInquiry, , )

        Proxy.Message(v_strObjMsg)

        Dim v_xmlDocument As New System.Xml.XmlDocument
        Dim v_nodeList As Xml.XmlNodeList
        Dim v_strFLDNAME As String
        Dim v_strValue As String
        Dim MaxRowSearch As Int32
        Try
            v_xmlDocument.LoadXml(v_strObjMsg)
            v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")

            For i As Integer = 0 To v_nodeList.Count - 1
                For j As Integer = 0 To v_nodeList.Item(i).ChildNodes.Count - 1
                    With v_nodeList.Item(i).ChildNodes(j)
                        v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                        v_strValue = .InnerText.ToString
                        Select Case Trim(v_strFLDNAME)
                            Case "VARVALUE"
                                v_strMaxRowSearch = Trim(v_strValue)
                                Exit For
                        End Select
                    End With
                Next
            Next
            MaxRowSearch = CInt(v_strMaxRowSearch)
            Return MaxRowSearch
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                                     & "Error code: System error!" & vbNewLine _
                                                     & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            Return 0
        End Try
    End Function

    Protected Overridable Function GetRowPage() As Int32
        Dim v_strCmdInquiry As String
        Dim v_strRowPage As String = String.Empty
        v_strCmdInquiry = "select VARVALUE from SYSVAR where VARNAME='ROWPERPAGE'"
        Dim v_strObjMsg As String = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SY_SEARCHFLD, gc_ActionInquiry, v_strCmdInquiry, , )

        Proxy.Message(v_strObjMsg)

        Dim v_xmlDocument As New System.Xml.XmlDocument
        Dim v_nodeList As Xml.XmlNodeList
        Dim v_strFLDNAME As String
        Dim v_strValue As String
        Dim RowPage As Int32
        Try
            v_xmlDocument.LoadXml(v_strObjMsg)
            v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")

            For i As Integer = 0 To v_nodeList.Count - 1
                For j As Integer = 0 To v_nodeList.Item(i).ChildNodes.Count - 1
                    With v_nodeList.Item(i).ChildNodes(j)
                        v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                        v_strValue = .InnerText.ToString
                        Select Case Trim(v_strFLDNAME)
                            Case "VARVALUE"
                                v_strRowPage = Trim(v_strValue)
                                Exit For
                        End Select
                    End With
                Next
            Next
            RowPage = CInt(v_strRowPage)
            Return RowPage
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                                     & "Error code: System error!" & vbNewLine _
                                                     & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            Return 0
        End Try
    End Function
#End Region

#Region "Các method khác"

    Public Sub ButtonEnable(ByVal Auth_String As String)
        btnAddNew.Enabled = (Mid(Auth_String, 1, 1) = "Y")
        btnView.Enabled = (Mid(Auth_String, 2, 1) = "Y")
        btnEdit.Enabled = (Mid(Auth_String, 3, 1) = "Y")
        btnDelete.Enabled = (Mid(Auth_String, 4, 1) = "Y")
        btnExecute.Enabled = (Mid(Auth_String, 5, 1) = "Y")
    End Sub

    Protected Overridable Sub LoadResource(ByRef pv_ctrl As System.Windows.Forms.Control)
        Dim v_ctrl As System.Windows.Forms.Control

        For Each v_ctrl In pv_ctrl.Controls
            If TypeOf (v_ctrl) Is System.Windows.Forms.Label Then
                CType(v_ctrl, System.Windows.Forms.Label).Text = mv_ResourceManager.GetString(v_ctrl.Name)
            ElseIf TypeOf (v_ctrl) Is System.Windows.Forms.GroupBox Then
                CType(v_ctrl, System.Windows.Forms.GroupBox).Text = mv_ResourceManager.GetString(v_ctrl.Name)
                LoadResource(v_ctrl)
            ElseIf TypeOf (v_ctrl) Is System.Windows.Forms.Button Then
                CType(v_ctrl, System.Windows.Forms.Button).Text = mv_ResourceManager.GetString(v_ctrl.Name)
            End If
        Next

        If mv_strTableName = "TLLOG" Then
            btnAddNew.Text = mv_ResourceManager.GetString("btnAddNew_" & mv_strTableName)
            btnView.Text = mv_ResourceManager.GetString("btnView_" & mv_strTableName)
            btnEdit.Text = mv_ResourceManager.GetString("btnEdit_" & mv_strTableName)
            btnDelete.Text = mv_ResourceManager.GetString("btnDelete_" & mv_strTableName)
            btnExecute.Text = mv_ResourceManager.GetString("btnExecute_" & mv_strTableName)
            ssbStatus.Text = mv_ResourceManager.GetString("ssbStatus_" & mv_strTableName)
        Else
            ssbStatus.Text = mv_ResourceManager.GetString("ssbStatus")
        End If
        btnExit.Text = mv_ResourceManager.GetString("btnExit")
        'btnBack.Text = mv_ResourceManager.GetString("btnBack")
        'btnNext.Text = mv_ResourceManager.GetString("btnNext")
        ssbFlag.Text = mv_ResourceManager.GetString("Searched")
        chkSearch.Text = Replace(mv_ResourceManager.GetString("chkSearch"), "$", PassDate)
        chkPass.Text = mv_ResourceManager.GetString("chkPass")

    End Sub

    'Thay đổi kích thước control khi form thay đổi size
    Private Sub DoResizeForm()
        grbSearchFilter.Width = Me.Width - 18
        btnSearch.Left = grbSearchFilter.Width - btnSearch.Width - 9
        btnExport.Left = btnSearch.Left
        grbConditionList.Width = grbSearchFilter.Width - btnSearch.Width - grbConditionList.Left - 18
        lstCondition.Width = grbConditionList.Width - 16

        grbSearchResult.Width = grbSearchFilter.Width
        pnlSearchResult.Width = grbSearchResult.Width - 16
        grbSearchResult.Height = Me.Height - grbSearchResult.Top - sbrStatus.Height - 8
        pnlSearchResult.Height = grbSearchResult.Height - 32
        'btnNext.Top = grbSearchResult.Height + grbSearchResult.Top + 8
        'btnBack.Top = grbSearchResult.Height + grbSearchResult.Top + 8
        txtNumRow.Left = btnSearch.Left
        lbNumRow.Left = btnSearch.Left
    End Sub

#End Region

    Private Sub LoadLastSearch()
        Try
            Dim v_strUserProfiles As String = System.Windows.Forms.Application.LocalUserAppDataPath & "\" & Me.BranchId & Me.TellerId & ".xml"
            Dim v_strSection As String = Me.ModuleCode & "." & Me.ObjectName
            Dim v_xmlDocument As New Xml.XmlDocument, v_nodetxData As Xml.XmlNode, v_nodeEntry As Xml.XmlNode
            Dim v_strObjMsg As String = String.Empty

            If Len(Dir(v_strUserProfiles)) = 0 Then
                'Tạo tệp tin UserProfiles
                v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, "USER_PROFILES:=" & Me.BranchId & "." & Me.TellerId, , , , )
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
            'LogError.Write("Error source: AppCore.frmSearch.LoadLastSearch" & vbNewLine _
            '             & "Error code: System error!" & vbNewLine _
            '             & "Error message: " & ex.Message, EventLogEntryType.Error)
            MsgBox(ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
        End Try
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
            '    Dim v_strCommandSQL As String
            '    v_strCommandSQL = pv_strSqlRef
            '    If InStr(v_strCommandSQL.ToUpper.Replace(" ", ""), "FROMRGSI") > 0 Then
            '        v_strCommandSQL = "select a.value, a.display from ( " & v_strCommandSQL & ") a, " _
            '                        & " (" _
            '                        & " select sicode from tlstockauth a where a.authtype = 'U' and a.authid = '" & Me.TellerId & "' " _
            '                        & " union " _
            '                        & " select sicode from tlstockauth a , (select grpid from tlgrpusers a where a.TLID = '" & Me.TellerId & "' and deleted=0 and status = 0) b " _
            '                        & " where a.authtype = 'G' and a.authid = b.grpid " _
            '                        & " ) b " _
            '                        & " where a.value = b.sicode "
            '    End If
            '    If InStr(v_strCommandSQL.ToUpper.Replace(" ", ""), "FROMRGMI") > 0 Then
            '        v_strCommandSQL = "select a.value, a.display from ( " & v_strCommandSQL & ") a, " _
            '                        & " (" _
            '                        & " select micode from tlmemauth a where a.authtype = 'U' and a.authid = '" & Me.TellerId & "' " _
            '                        & " union " _
            '                        & " select micode from tlmemauth a , (select grpid from tlgrpusers a where a.TLID = '" & Me.TellerId & "' and deleted=0 and status = 0) b " _
            '                        & " where a.authtype = 'G' and a.authid = b.grpid " _
            '                        & " ) b " _
            '                        & " where a.value = b.micode "
            '    End If
            '    If InStr(v_strCommandSQL.ToUpper.Replace(" ", ""), "FROMBRGRP") > 0 Then
            '        v_strCommandSQL = "select a.value, a.display from ( " & v_strCommandSQL & ") a, " _
            '                        & " (" _
            '                        & " select brid from tlbridauth a where a.authtype = 'U' and a.authid = '" & Me.TellerId & "' " _
            '                        & " union " _
            '                        & " select brid from tlbridauth a , (select grpid from tlgrpusers a where a.TLID = '" & Me.TellerId & "' and deleted=0 and status = 0) b " _
            '                        & " where a.authtype = 'G' and a.authid = b.grpid " _
            '                        & " ) b " _
            '                        & " where a.value = b.brid "
            '    End If
            '    Dim v_strObjMsg As String = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SY_SEARCHFLD, gc_ActionInquiry, v_strCommandSQL)
            '    'Dim v_ws As New BDSDelivery.BDSDelivery
            '    mv_BDSDelivery.Message(v_strObjMsg)

            'FillComboEx(v_strObjMsg, txtValue)

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

        Me.grbCondition.Controls.Add(Me.txtValue)
        'txtValue.Tag = pv_strLstTable
    End Sub

    Public Sub ShowAction()
        OnSearch(IsLocalSearch, ModuleCode & "." & ObjectName, mv_intpage)
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As Xceed.SmartUI.SmartItemClickEventArgs) Handles btnBack.Click
        mv_intpage = mv_intpage - 1
        If mv_intpage <= 0 Then
            mv_intpage = 1
            btnBack.Enabled = False
        Else
            OnSearch(IsLocalSearch, ModuleCode & "." & ObjectName, mv_intpage)
            btnNext.Enabled = True
        End If
    End Sub

    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As Xceed.SmartUI.SmartItemClickEventArgs) Handles btnNext.Click
        Dim CountR As Int32 = mv_lngRowCount 'CountRow()
        If CountR >= (mv_intpage) * mv_rowpage Then
            mv_intpage = mv_intpage + 1
            OnSearch(IsLocalSearch, ModuleCode & "." & ObjectName, mv_intpage)
            btnBack.Enabled = True
        Else
            btnNext.Enabled = False
        End If
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


    'Private Sub ShowFormProcess()
    '    Dim v_frm As New frmProcess
    '    v_frm.ShowDialog()
    'End Sub

    Private Sub txtNumRow_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtNumRow.Validating
        If Trim(txtNumRow.Text) = "" Then
            MsgBox(mv_ResourceManager.GetString("ErrNum"), MsgBoxStyle.OkOnly)
            txtNumRow.Text = 100
            txtNumRow.Focus()
            Me.btnSearch.Enabled = True
            Exit Sub
        Else
            If IsNumeric(Trim(txtNumRow.Text)) Then
                If CInt(txtNumRow.Text) <= 0 Then
                    MsgBox(mv_ResourceManager.GetString("ErrNum"), MsgBoxStyle.OkOnly)
                    txtNumRow.Text = 100
                    txtNumRow.Focus()
                    Me.btnSearch.Enabled = True
                    Exit Sub
                End If
            Else
                MsgBox(mv_ResourceManager.GetString("ErrNum"), MsgBoxStyle.OkOnly)
                txtNumRow.Text = 100
                txtNumRow.Focus()
                Me.btnSearch.Enabled = True
                Exit Sub
            End If
        End If
    End Sub

    Private Sub chkPass_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkPass.CheckedChanged
        chkSearch.Enabled = Not chkPass.Checked
    End Sub

    Public Sub EnableBtnSearch()
        btnSearch.Enabled = True
        btnExport.Enabled = True
    End Sub

    'Private Sub txtNumRow_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtNumRow.Leave
    '    Dim v_strNumRow As String = txtNumRow.Text
    '    Dim v_intNumRow As Int32
    '    If Trim(v_strNumRow) <> "" Then
    '        If IsNumeric(v_strNumRow) Then
    '            v_intNumRow = CInt(v_strNumRow)
    '            If v_intNumRow > MaxRowSearch Then
    '                MsgBox(mv_ResourceManager.GetString("MaxRowSearch"), MsgBoxStyle.OkOnly)
    '                btnSearch.Enabled = False
    '                Exit Sub
    '            Else
    '                btnSearch.Enabled = mv_blnStatusSearch
    '            End If
    '        Else
    '            MsgBox(mv_ResourceManager.GetString("ErrRowNumFormat"), MsgBoxStyle.OkOnly)
    '            btnSearch.Enabled = mv_blnStatusSearch
    '            Exit Sub
    '        End If
    '    Else
    '        MsgBox(mv_ResourceManager.GetString("ErrRowNumFormat"), MsgBoxStyle.OkOnly)
    '        btnSearch.Enabled = mv_blnStatusSearch
    '    End If
    'End Sub

    Private Sub Chk_check_confirm_all_Click(ByVal sender As System.Object, ByVal e As Xceed.SmartUI.SmartItemClickEventArgs) Handles Chk_check_confirm_all.Click
        Try
            If Chk_check_confirm_all.Checked Then
                For i = 0 To SearchGrid.DataRows.Count - 1
                    'SearchGrid.DataRows.Item(i)
                    If mv_strGlobalAuthString = "" And Mid(SearchGrid.DataRows.Item(i).Cells("AUTH_STRING").Value, 2, 3) <> "NNN" Then
                        mv_strGlobalAuthString = Mid(SearchGrid.DataRows.Item(i).Cells("AUTH_STRING").Value, 2, 3)
                        If SearchGrid.DataRows.Item(i).Cells("TLTXCD").Value = "6039" _
                                    Or SearchGrid.DataRows.Item(i).Cells("TLTXCD").Value = "2012" _
                                    Or SearchGrid.DataRows.Item(i).Cells("TLTXCD").Value = "3206" _
                                    Or SearchGrid.DataRows.Item(i).Cells("TLTXCD").Value = "3203" _
                                    Or SearchGrid.DataRows.Item(i).Cells("TLTXCD").Value = "2117" _
                                    Or SearchGrid.DataRows.Item(i).Cells("TLTXCD").Value = "3035" Then
                            SearchGrid.DataRows.Item(i).Cells("TXBLN").Value = True
                            mv_intRowSelectedCount += 1
                        End If

                    Else
                        If Mid(SearchGrid.DataRows.Item(i).Cells("AUTH_STRING").Value, 2, 3) = mv_strGlobalAuthString Then

                            If SearchGrid.DataRows.Item(i).Cells("TLTXCD").Value = "6039" _
                                    Or SearchGrid.DataRows.Item(i).Cells("TLTXCD").Value = "2012" _
                                    Or SearchGrid.DataRows.Item(i).Cells("TLTXCD").Value = "3206" _
                                    Or SearchGrid.DataRows.Item(i).Cells("TLTXCD").Value = "3203" _
                                    Or SearchGrid.DataRows.Item(i).Cells("TLTXCD").Value = "2117" _
                                    Or SearchGrid.DataRows.Item(i).Cells("TLTXCD").Value = "3035" Then
                                SearchGrid.DataRows.Item(i).Cells("TXBLN").Value = True
                                mv_intRowSelectedCount += 1
                            End If
                        End If
                    End If


                Next
                'MsgBox("chon tat ca", MsgBoxStyle.Information)
            Else
                For i = 0 To SearchGrid.DataRows.Count - 1
                    'SearchGrid.DataRows.Item(i)
                    If mv_strGlobalAuthString = "" And Mid(SearchGrid.DataRows.Item(i).Cells("AUTH_STRING").Value, 2, 3) <> "NNN" Then
                        mv_strGlobalAuthString = Mid(SearchGrid.DataRows.Item(i).Cells("AUTH_STRING").Value, 2, 3)

                        SearchGrid.DataRows.Item(i).Cells("TXBLN").Value = False

                        mv_intRowSelectedCount -= 1

                    Else
                        If Mid(SearchGrid.DataRows.Item(i).Cells("AUTH_STRING").Value, 2, 3) = mv_strGlobalAuthString Then

                            SearchGrid.DataRows.Item(i).Cells("TXBLN").Value = False
                            mv_intRowSelectedCount -= 1

                        End If
                    End If


                Next
                'MsgBox("Bo chon tat ca", MsgBoxStyle.Information)

            End If
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                         & "Error code: System error!" & vbNewLine _
                                         & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Sub
End Class
