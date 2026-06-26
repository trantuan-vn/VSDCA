Imports System.IO
Imports System.Collections
Imports Sats.CommonLibrary
Imports Xceed.SmartUI.Controls
Imports Sats.WinFormsUI.Docking
Imports System.Windows.Forms
Imports Sats.AppCore


Public Class frmMain
    Public WithEvents btn As Button
#Region " Khai báo hằng, biến "

    Const c_ResourceManager = "Sats.frmMain_"
    Dim mv_dblTR_QTTY As Double = 0
    Protected WithEvents SearchGrid As GridEx
    Protected WithEvents SearchCell As Xceed.Grid.Cell
    Public mv_strSearchFilter As String
    Public hFilter As New Hashtable
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
    Private mv_arrSrFieldWidth() As Integer
    Private mv_arrSrLstTable() As String

    Private mv_strLanguage As String
    Private mv_ResourceManager As Resources.ResourceManager

    Private mv_strBranchId As String
    Private mv_strTellerId As String
    Private mv_strTellerType As String
    Private mv_intpage As Int32 = 1
    Private mv_rowpage As Int32 = 1
    Private mv_strBusDate As String
    Private mv_intRowCount As Int32 = 0

    Public mv_enuEditFormResult As SaveButtonType

    Private mv_SelectedRow As Xceed.Grid.Row

    Private Proxy As BDSChannel.BDSDelivery
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
                CountR = CountRow()
                If CountR >= (mv_intpage + 1) * mv_rowpage Then
                    mv_intpage = mv_intpage + 1
                    OnSearch(IsLocalSearch, ModuleCode & "." & ObjectName, mv_intpage)
                End If
            Case Keys.F9
                'Tương đương nhấn Double Click của dòng hiện tại
                If mv_intDblGrid = 0 Then
                    mv_intDblGrid = 1
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
        mv_ResourceManager = New Resources.ResourceManager(c_ResourceManager & UserLanguage, System.Reflection.Assembly.GetExecutingAssembly())
        LoadResource(Me)
        DoResizeForm()
        Me.TabText = mv_ResourceManager.GetString("frmMain")
        'mv_BDSDelivery = New BDSChannel.BDSDelivery
    End Sub

    Private Sub frmSearch_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        DoResizeForm()
    End Sub

    Private Sub Grid_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If Not SearchGrid.CurrentColumn Is Nothing Then
            If SearchGrid.CurrentColumn.FieldName = "__TICK" Then
                If SearchGrid.CurrentCell.Value = "X" Then
                    SearchGrid.CurrentCell.Value = String.Empty
                Else
                    SearchGrid.CurrentCell.Value = "X"
                End If
            End If
        End If
    End Sub

    Private Sub Grid_DblClick(ByVal sender As Object, ByVal e As System.EventArgs)
        Cursor = Cursors.WaitCursor
        If mv_intDblGrid = 0 Then
            mv_intDblGrid = 1

            OnQuery()
            mv_intDblGrid = 0
            Cursor = Cursors.Default
        End If
    End Sub

    Private Sub Grid_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        Try
            Select Case e.KeyCode
                Case Keys.Space
                    Cursor = Cursors.WaitCursor
                    If Not SearchGrid.Columns("__TICK") Is Nothing Then
                        If SearchGrid.CurrentColumn.FieldName = "__TICK" Then
                            If SearchGrid.CurrentCell.Value = "X" Then
                                SearchGrid.CurrentCell.Value = String.Empty
                            Else
                                SearchGrid.CurrentCell.Value = "X"
                            End If
                        End If
                    End If
                Case Keys.Enter 'Enter = Onclose de insert luon cho GD,Double_click =View 
                    Cursor = Cursors.WaitCursor
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
                    OnClose()
                Case Keys.Delete
                    OnDelete()
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

    Private Sub Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim v_strValue, v_strValueDisplay As String
        'Dim v_objResult As Object
        'Dim v_strFilterTmp As String
        'Dim v_strSearchKey As String
        'Dim v_blnSearchKeyAdded As Boolean

        Try
            If (sender Is btnSearch) Then
                OnSearch(IsLocalSearch, ModuleCode & "." & ObjectName, mv_intpage)
            ElseIf (sender Is btnExport) Then
                OnExport()
            ElseIf (sender Is btnAdd) Then
                AddSearchCriteria()
            ElseIf (sender Is btnRemove) Then
                RemoveSearchCriteria()
            ElseIf (sender Is btnRemoveAll) Then
                RemoveAllSearchCriterias()
            End If
        Catch ex As Exception
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            '             & "Error code: System error!" & vbNewLine _
            '             & "Error message: " & ex.Message, EventLogEntryType.Error)
            Throw ex
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
            'Dim i1 As Int16
            v_strValueDisplay = Trim(txtValue.Text)
            If mv_arrSrSQLRef(cboField.SelectedIndex + 1).Trim.Length > 0 Then
                v_strValue = v_strValueDisplay
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
                        v_strFilterTmp = "T."
                        v_strFilterTmp &= IIf(mv_arrSrSQLRef(cboField.SelectedIndex + 1).Trim.Length = 0, _
                            mv_arrSrFieldSrch(cboField.SelectedIndex + 1), _
                            mv_arrSrFieldSrch(cboField.SelectedIndex + 1))
                        v_strFilterTmpUpper = "REPLACE (UPPER( Trim (" & v_strFilterTmp & ")),'.','')"
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
                                    v_strFilterTmpUpper &= "UPPER ('" _
                                                  & IIf(Trim(cboOperator.SelectedValue) = "LIKE", "", "") & v_strValue _
                                                  & IIf(Trim(cboOperator.SelectedValue) = "LIKE", "", "") & "')"

                                Else
                                    If v_strValue = String.Empty Then
                                        v_strFilterTmpUpper = Replace(v_strFilterTmpUpper, "=", "")
                                        v_strFilterTmpUpper &= " IS NULL "
                                    Else
                                        v_strFilterTmpUpper &= "UPPER ('" _
                                               & IIf(Trim(cboOperator.SelectedValue) = "LIKE", "%", "") & v_strValue _
                                               & IIf(Trim(cboOperator.SelectedValue) = "LIKE", "%", "") & "')"
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
            LogError.Write("Error source: AppCore.frmSearch.AddSearchCriteria" & vbNewLine _
                         & "Error code: System error!" & vbNewLine _
                         & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
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
            LogError.Write("Error source: AppCore.frmSearch.AddSearchCriteria" & vbNewLine _
                         & "Error code: System error!" & vbNewLine _
                         & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
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
            LogError.Write("Error source: AppCore.frmSearch.AddSearchCriteria" & vbNewLine _
                         & "Error code: System error!" & vbNewLine _
                         & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
        End Try
    End Sub
#End Region

#Region "Over method"
    Public Sub InitDialog()
        'Khởi tạo kích thước form và load resource        

        SearchGrid = New GridEx(mv_strTableName, "Sats.AppCore.frmSearch_" & UserLanguage, TellerId)
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


        'Set click event for buttons
        AddHandler btnSearch.Click, AddressOf Button_Click
        AddHandler btnExport.Click, AddressOf Button_Click
        AddHandler btnAdd.Click, AddressOf Button_Click
        AddHandler btnRemove.Click, AddressOf Button_Click
        AddHandler btnRemoveAll.Click, AddressOf Button_Click

        'Set KeyDown event for Value textbox

        'Set selected index changed event for ComboBoxes
        AddHandler cboField.SelectedIndexChanged, AddressOf Combo_SelectedIndexChanged
        AddHandler cboOperator.SelectedIndexChanged, AddressOf Combo_SelectedIndexChanged

        'Thiết lập các giá trị ban đầu cho các đi?u kiện tìm kiếm
        Dim v_strCmdInquiry As String = "SELECT * FROM V_SEARCHCD WHERE 0=0 "
        Dim v_strClause As String = " UPPER(SEARCHCODE) = '" & mv_strTableName & "' ORDER BY POSITION"
        Dim v_strObjMsg As String = BuildXMLObjMsg(, , , , gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SY_SEARCHFLD, gc_ActionInquiry, v_strCmdInquiry, v_strClause, )

        'Dim v_ws As New BDSDelivery.BDSDelivery
        Proxy.Message(v_strObjMsg)

        'PrepareSearchParams(UserLanguage, v_strObjMsg, mv_strCaption, mv_strEnCaption, mv_strCmdSql, mv_strObjName, mv_strFormName, _
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

        'Load the last filter
        LoadLastSearch()

        If SearchOnInit Then
            OnSearch(IsLocalSearch, ModuleCode & "." & ObjectName)
        End If
        txtNumRow.Text = GetRowPage()
        mv_rowpage = CInt(txtNumRow.Text)
    End Sub

    Protected Overridable Function OnAddNew() As Int32
        If ShowForm(ExecuteFlag.AddNew) = Windows.Forms.DialogResult.OK Then
            OnSearch(IsLocalSearch, ModuleCode & "." & ObjectName)
        End If
    End Function

    Protected Overridable Function OnUpdate() As Int32
        'If Not (SearchGrid Is Nothing) Then
        '    mv_SelectedRow = SearchGrid.CurrentRow
        'End If
        If ShowForm(ExecuteFlag.Edit) = Windows.Forms.DialogResult.OK Then
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
            If v_res = Windows.Forms.DialogResult.OK Then
                Dim v_strFileName As String = v_dlgSave.FileName
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
                            If SearchGrid.Columns(j).Visible Then
                                v_strData &= SearchGrid.DataRows(i).Cells(j).Value & vbTab
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
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                         & "Error code: System error!" & vbNewLine _
                         & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
        End Try
    End Function

    Protected Overridable Function OnExecute() As Int32
        Dim v_strSQL, v_strObjMsg As String
        Dim v_xmlDocument As New Xml.XmlDocument, v_xmlDocumentData As New Xml.XmlDocument
        'Dim v_ws As New BDSDelivery.BDSDelivery
        Dim v_nodeList As Xml.XmlNodeList
        Dim v_strValue, v_strFLDNAME, v_strFLDCD, v_strFLDCODE, v_strTLTXCD, v_strMODCODE, v_strFLDDEFVAL As String, i, j, v_intRow As Integer

        'Căn cứ vào SEARCHCODE để lấy mã giao dịch (TLTXCD) và nạp các giá trị mặc định cho trư?ng giao dịch FLDCD.
        v_strSQL = "SELECT APPMODULES.MODCODE, SEARCH.TLTXCD, SEARCHFLD.FIELDCODE, SEARCHFLD.FLDCD FROM APPMODULES, SEARCH, SEARCHFLD " & ControlChars.CrLf _
            & "WHERE SEARCH.SEARCHCODE=SEARCHFLD.SEARCHCODE AND APPMODULES.TXCODE=SUBSTR(SEARCH.TLTXCD,1,2) AND LENGTH(SEARCH.TLTXCD)=4 AND SEARCH.SEARCHCODE='" & mv_strTableName & "'"
        v_strObjMsg = BuildXMLObjMsg(, , , , gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLTX, gc_ActionInquiry, v_strSQL)

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

        Try
            'Update mouse pointer
            'If Not SearchGrid.CurrentRow Is Nothing Then
            'If KeyColumn Is Nothing Then
            'Else
            'Value = Trim(CType(SearchGrid.CurrentRow, Xceed.Grid.DataRow).Cells(KeyColumn).Value)
            'End If
            'End If

            If Trim(txtNumRow.Text) = "" Then
                mv_rowpage = 0
            Else
                If IsNumeric(Trim(txtNumRow.Text)) Then
                    mv_rowpage = CInt(txtNumRow.Text)
                Else
                    MsgBox(mv_ResourceManager.GetString("ErrNum"), MsgBoxStyle.OkOnly)
                    txtNumRow.Focus()
                    Exit Function
                End If
            End If



            Cursor = Cursors.WaitCursor

            'Update status bar
            ssbFlag.Text = mv_ResourceManager.GetString("Searching")
            'ssbPanelExecFlag.Text = String.Empty
            mv_strSearchFilter = String.Empty

            For i = 0 To lstCondition.Items.Count - 1
                If lstCondition.GetItemChecked(i) Then
                    mv_strSearchFilter &= " AND " & hFilter(lstCondition.Items(i).ToString())
                End If
            Next i
            mv_strSearchFilter = Mid(mv_strSearchFilter, 5)

            If (pv_strIsLocal <> "") And (pv_strModule <> "") Then
                'Dim v_ws As New BDSDelivery.BDSDelivery
                Dim strRow As String
                Dim v_intFrom, v_intTo As Int32

                v_intTo = page * mv_rowpage
                v_intFrom = v_intTo + 1 - mv_rowpage

                If (mv_strSrOderByCmd <> "") And (mv_strSearchFilter <> "") Then
                    mv_strSearchFilter &= "ORDER BY " & mv_strSrOderByCmd
                End If

                If mv_strSearchFilter = "" Then
                    If mv_strSrOderByCmd <> "" Then
                        mv_strSearchFilter = " 0=0 ORDER BY " & mv_strSrOderByCmd
                    Else
                        mv_strSearchFilter = " 0 = 0 "
                    End If
                    If mv_rowpage = 0 Then
                        strRow = "SELECT * FROM ( SELECT T1.*,ROWNUM RN FROM(" & mv_strCmdSql & " AND " & mv_strSearchFilter & ")T1)" ' WHERE RN BETWEEN " & v_intFrom & " AND " & v_intTo
                    Else
                        strRow = "SELECT * FROM ( SELECT T1.*,ROWNUM RN FROM(" & mv_strCmdSql & " AND " & mv_strSearchFilter & ")T1) WHERE RN BETWEEN " & v_intFrom & " AND " & v_intTo
                    End If
                    mv_strCmdSqlTemp = mv_strCmdSql & " AND " & mv_strSearchFilter
                Else
                    strRow = "SELECT * FROM ( SELECT T1.*,ROWNUM RN FROM(SELECT * FROM (" & mv_strCmdSql & ") T WHERE  " & mv_strSearchFilter & ")T1) " 'WHERE RN BETWEEN " & v_intFrom & " AND " & v_intTo

                    mv_strCmdSqlTemp = "SELECT * FROM (" & mv_strCmdSql & ") T WHERE  " & mv_strSearchFilter
                End If

                Dim v_strObjMsg As String = BuildXMLObjMsg(, , , , pv_strIsLocal, gc_MsgTypeObj, pv_strModule, _
                                                    gc_ActionInquiry, strRow)
                Proxy.Message(v_strObjMsg)
                Me.FULLDATA = v_strObjMsg
                'Fill data into search grid
                FillDataGrid(SearchGrid, v_strObjMsg, "Sats.AppCore.frmSearch_" & UserLanguage, mv_strTableName, , v_intFrom, v_intTo, CountRow())
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
            End If

            ssbFlag.Text = mv_ResourceManager.GetString("Searched") & mv_intRowCount
            'Update mouse pointer
            Cursor = Cursors.Default
            'SetFocusGrid(Value)
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                         & "Error code: System error!" & vbNewLine _
                         & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
        End Try
    End Function

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

                            Dim v_strObjMsg As String = BuildXMLObjMsg(, BranchId, , TellerId, pv_strIsLocal, gc_MsgTypeObj, pv_strModule, gc_ActionDelete, , v_strClause)
                            'Dim v_ws As New BDSDelivery.BDSDelivery
                            Proxy.Message(v_strObjMsg)

                            'Kiểm tra thông tin và xử lý lỗi (nếu có) từ message trả v?
                            Dim v_strErrorSource As String = "", v_strErrorMessage As String = ""
                            Dim v_lngErrorCode As Long

                            GetErrorFromMessage(v_strObjMsg, v_strErrorSource, v_lngErrorCode, v_strErrorMessage)
                            If v_lngErrorCode <> 0 Then
                                'Update mouse pointer
                                Cursor = Cursors.Default
                                If v_lngErrorCode = -100009 Then
                                    MsgBox(mv_ResourceManager.GetString("DelNotSuccess"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, Me.Text)
                                Else
                                    MsgBox(v_strErrorMessage, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, Me.Text)
                                End If

                                Exit Function
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
            MsgBox(ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
        End Try
    End Function

    Protected Overridable Function GetRowPage() As Int32
        Dim v_strCmdInquiry As String
        Dim v_strRowPage As String = String.Empty
        v_strCmdInquiry = "select VARVALUE from SYSVAR where VARNAME='ROWPERPAGE'"
        Dim v_strObjMsg As String = BuildXMLObjMsg(, , , , gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SY_SEARCHFLD, gc_ActionInquiry, v_strCmdInquiry, , )
        'Dim v_ws As New BDSDelivery.BDSDelivery
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
            MessageBox.Show(ex.ToString)
            Return 0
        End Try
    End Function
#End Region

#Region "Các method khác"

    Private Sub LoadResource(ByRef pv_ctrl As Windows.Forms.Control)
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
        ssbFlag.Text = mv_ResourceManager.GetString("Searched")

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

    Public Sub EnableControl(ByVal bln As Boolean)
        Dim o_ctrl As Windows.Forms.Control
        For Each o_ctrl In Me.Controls
            If TypeOf (o_ctrl) Is GroupBox Then
                CType(o_ctrl, GroupBox).Enabled = bln
            End If
        Next
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
            LogError.Write("Error source: AppCore.frmSearch.LoadLastSearch" & vbNewLine _
                         & "Error code: System error!" & vbNewLine _
                         & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
        End Try
    End Sub

    Private Sub NewTxtValue(ByVal pv_strSqlRef As String, ByVal pv_strFldType As String, _
                           ByVal pv_strFldMask As String, ByVal pv_strDefValue As String, ByVal pv_strFldFormat As String)
        txtValue.Dispose()

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
    End Sub

    Private Sub txtValue_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtValue.KeyUp
        Select Case e.KeyCode
            Case Keys.Enter
                AddSearchCriteria()
        End Select
    End Sub

    Private Sub tmrSearch_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tmrSearch.Tick
        'OnSearch(IsLocalSearch, ModuleCode & "." & ObjectName, mv_intpage)
    End Sub
End Class
