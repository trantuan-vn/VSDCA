Imports Xceed.Grid
Imports Sats.CommonLibrary
Imports Sats.AppCore
'Imports SendFiles
'Imports ZetaCompressionLibrary
'Imports TestBase64
Imports System.IO
Imports System.Drawing
Imports System.Windows.Forms

<Serializable()> _
Public Module modCoreLib



    '----------------------------------------------------------------------------------------------
    ' + Mục đích:   ?ịnh dạng các thuộc tính chung nhất của GRID
    ' + ?ầu vào:    
    '       - pv_xGrid:         GRID cần định dạng các thuộc tính
    '       - pv_strTable:      Tên bảng dữ liệu để fill vào GRID
    '       - pv_strResource:   Tên resource dùng để định dạng GRID
    '       - pv_blnFirst:      ?ịnh dạng lần đầu; mặc định là TRUE
    ' + ?ầu ra:     GRID được định dạng
    ' + Trả v?:     N/A
    ' + Tác giả:    Trần Ki?u Minh
    ' + Ghi chú:    N/A
    '----------------------------------------------------------------------------------------------
    Public Sub _FormatGridBefore(ByRef pv_xGrid As GridControl, ByVal pv_strTellerID As String, _
                                 Optional ByVal pv_strTable As String = vbNullString, _
                                 Optional ByVal pv_strResource As String = vbNullString, _
                                 Optional ByVal pv_blnFirst As Boolean = True, _
                                 Optional ByVal pv_blnGroup As Boolean = True, _
                                 Optional ByVal pv_intFromrow As Int32 = 0, _
                                 Optional ByVal pv_intTorow As Int32 = 0, _
                                 Optional ByVal pv_intTotalrow As Int32 = 0)

        Dim m_ResourceManager As Resources.ResourceManager = Nothing
        If Len(pv_strResource) > 0 And pv_blnGroup Then
            m_ResourceManager = New Resources.ResourceManager(pv_strResource, System.Reflection.Assembly.GetExecutingAssembly())
        End If

        'Nếu lần đầu tiên tạo thì xoá trắng định dạng của Grid
        If pv_blnFirst Then pv_xGrid.Clear()

        'Không cho phép sửa dữ liệu trên GRID
        pv_xGrid.ReadOnly = True

        Dim GroupByRow1 As Xceed.Grid.GroupByRow = Nothing
        Dim ColumnManagerRow1 As Xceed.Grid.ColumnManagerRow = Nothing
        Dim VisualGridElementStyle1 As Xceed.Grid.VisualGridElementStyle
        Dim VisualGridElementStyle2 As Xceed.Grid.VisualGridElementStyle

        VisualGridElementStyle1 = New Xceed.Grid.VisualGridElementStyle
        VisualGridElementStyle2 = New Xceed.Grid.VisualGridElementStyle

        '?ịnh nghĩa định dạng cho Row dữ liệu
        '
        'VisualGridElementStyle
        '
        VisualGridElementStyle1.BackColor = System.Drawing.Color.FromArgb(CType(32, Byte), CType(1, Byte), CType(152, Byte), CType(2, Byte))
        VisualGridElementStyle2.BackColor = System.Drawing.Color.FromArgb(CType(32, Byte), CType(249, Byte), CType(190, Byte), CType(58, Byte))

        If pv_blnGroup Then
            GroupByRow1 = New Xceed.Grid.GroupByRow
            ColumnManagerRow1 = New Xceed.Grid.ColumnManagerRow
            '
            'GroupByRow1
            '
            '
            If Len(pv_strResource) > 0 Then
                GroupByRow1.NoGroupText = m_ResourceManager.GetString("GridEx_GroupByRow")
            End If

            GroupByRow1.BackColor = System.Drawing.Color.FromArgb(CType(64, Byte), CType(127, Byte), CType(123, Byte), CType(122, Byte))
            GroupByRow1.CellBackColor = System.Drawing.Color.FromArgb(CType(64, Byte), CType(216, Byte), CType(84, Byte), CType(2, Byte))
            GroupByRow1.CellFont = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
            '
            'ColumnManagerRow1
            '
            ColumnManagerRow1.BackColor = System.Drawing.Color.FromArgb(CType(64, Byte), CType(216, Byte), CType(84, Byte), CType(2, Byte))
            ColumnManagerRow1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
            ColumnManagerRow1.HorizontalAlignment = Xceed.Grid.HorizontalAlignment.Center
        End If

        pv_xGrid.RowSelectorPane.BackColor = System.Drawing.Color.FromArgb(CType(64, Byte), CType(216, Byte), CType(84, Byte), CType(2, Byte))
        pv_xGrid.RowSelectorPane.ForeColor = System.Drawing.Color.White
        pv_xGrid.SelectionBackColor = System.Drawing.Color.FromArgb(CType(96, Byte), CType(29, Byte), CType(50, Byte), CType(139, Byte))
        pv_xGrid.SelectionForeColor = System.Drawing.Color.Black

        pv_xGrid.Font = New System.Drawing.Font("Tahoma", 8.25!)
        pv_xGrid.ForeColor = System.Drawing.Color.Black
        pv_xGrid.InactiveSelectionBackColor = System.Drawing.Color.FromArgb(CType(48, Byte), CType(29, Byte), CType(50, Byte), CType(139, Byte))
        pv_xGrid.InactiveSelectionForeColor = System.Drawing.Color.Black

        pv_xGrid.DataRowTemplateStyles.Add(VisualGridElementStyle1)
        pv_xGrid.DataRowTemplateStyles.Add(VisualGridElementStyle2)

        If pv_blnFirst Then
            pv_xGrid.FixedHeaderRows.Add(GroupByRow1)
            pv_xGrid.FixedHeaderRows.Add(ColumnManagerRow1)
            If Len(pv_strResource) > 0 Then
                _FormatGridAfter(pv_xGrid, pv_strTable, pv_strResource.Substring(pv_strResource.Length - 2), pv_strTellerID)
            End If
        End If

        If Len(pv_strResource) > 0 And pv_blnGroup Then
            'Dim FooterRow As New TextRow(m_ResourceManager.GetString("GridEx.FooterRow") & pv_xGrid.DataRows.Count.ToString)
            Dim m_intToRow As Int32
            Dim FooterRow As TextRow  '(m_ResourceManager.GetString("GridEx.FooterRowFrom") & pv_intFromrow & " " & m_ResourceManager.GetString("GridEx.FooterRowTo") & m_intToRow & "   " & m_ResourceManager.GetString("GridEx.FooterRowIn") & pv_intTotalrow & " Row ")

            If (pv_intFromrow = 0) And (pv_intTorow = 0) And (pv_intTotalrow = 0) Then
                FooterRow = New TextRow(m_ResourceManager.GetString("GridEx_FooterRow") & pv_xGrid.DataRows.Count.ToString)
            Else
                If pv_intFromrow = 0 Then
                    m_intToRow = pv_intFromrow + pv_xGrid.DataRows.Count.ToString
                Else
                    m_intToRow = pv_intFromrow - 1 + pv_xGrid.DataRows.Count.ToString
                End If
                FooterRow = New TextRow(m_ResourceManager.GetString("GridEx_FooterRowFrom") & FormatNumber(pv_intFromrow, 0) & " " _
                    & m_ResourceManager.GetString("GridEx_FooterRowTo") & FormatNumber(m_intToRow, 0) & " " _
                    & m_ResourceManager.GetString("GridEx_FooterRowIn").Replace("@", FormatNumber(pv_intTotalrow, 0)))
            End If
            'FooterRow
            FooterRow.BackColor = System.Drawing.Color.FromArgb(CType(64, Byte), CType(216, Byte), CType(84, Byte), CType(2, Byte))
            FooterRow.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Italic)
            pv_xGrid.FixedFooterRows.Clear()
            pv_xGrid.FixedFooterRows.Add(FooterRow)
        End If
    End Sub

    '----------------------------------------------------------------------------------------------
    ' + Mục đích:   ?ịnh dạng các thuộc tính của GRID sau khi load dữ liệu
    ' + ?ầu vào:    
    '       - pv_xGrid:         GRID cần định dạng các thuộc tính
    '       - pv_strTable:      Tên bảng dữ liệu để fill vào GRID
    ' + ?ầu ra:     GRID được định dạng
    ' + Trả v?:     N/A
    ' + Tác giả:    Trần Ki?u Minh
    ' + Ghi chú:    N/A
    '----------------------------------------------------------------------------------------------
    Public Sub _FormatGridAfter(ByVal pv_xGrid As GridControl, _
                                ByVal pv_strTable As String, ByVal pv_strUserLanguage As String, ByVal pv_strTellerID As String)
        Try
            'Dim v_strCondition As String
            'Dim v_xmlDocument As New Xml.XmlDocument
            'Dim v_nodeList As Xml.XmlNodeList
            Dim v_intCount As Integer
            Dim v_strFieldCode As String = "", v_strFieldType As String = "", v_strFieldName As String = ""
            Dim v_strEnFieldName As String = "", v_strWidth As String = "", v_strVisible As String = ""
            Dim v_obj As New SQLEngine.SQLDataAccessLayer(pv_strTellerID)

            'Lựa ch?n các đi?u kiện tìm kiếm
            'v_strCondition = "upper(SEARCHCODE) = '" & pv_strTable & "' ORDER BY POSITION"

            'Dim v_ws As New BDSDelivery.BDSDelivery

            Dim v_strCmdInquiry As String '= "SELECT * FROM V_SEARCHCD WHERE 0=0 "
            'Dim v_strObjMsg As String = BuildXMLObjMsg(, , , pv_strTellerID, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SY_SEARCHFLD, gc_ActionInquiry, v_strCmdInquiry, v_strCondition)


            v_strCmdInquiry = "SELECT s.searchcode, s.searchtitle, s.en_searchtitle, s.searchcmdsql , (CASE WHEN s.searchcmdsql1 IS NULL THEN '' ELSE s.searchcmdsql1 END) as searchcmdsql1, s.objname, s.frmname, s.orderbycmdsql, sf.position AS POSITION, sf.fieldcode, " _
                            & " sf.fieldname, sf.en_fieldname, sf.fieldtype, sf.fieldsize, sf.mask AS MASK, sf.operator AS OPERATOR, sf.format, sf.display, sf.srch, sf.[Key] AS [KEY], " _
                            & " sf.refvalue, s.tltxcd, sf.width, sf.lookupcmdsql" _
                            & " FROM sisearch AS s INNER JOIN" _
                            & " sisearchfld AS sf ON s.searchcode = sf.searchcode" _
                            & " WHERE (s.deleted = 0) AND (s.status = 0) AND (sf.deleted = 0) AND (sf.status = 0) AND (upper(s.SEARCHCODE) = '" & pv_strTable & "') ORDER BY POSITION"

            Dim v_ds As DataSet

            v_ds = v_obj.ExecuteReturnDataSet(v_strCmdInquiry)
            'Dim v_ws As New BDSChannel.BDSDelivery
            'Dim v_lngError As Long = v_ws.Message(v_strObjMsg)
            'If v_lngError <> ERR_SYSTEM_OK Then
            '    MsgBox(IIf(pv_strUserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
            '    Exit Sub
            'End If

            'v_xmlDocument.LoadXml(v_strObjMsg)
            'v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")

            pv_xGrid.Dock = Windows.Forms.DockStyle.Fill
            'pv_xGrid.Columns.Add(New Xceed.Grid.Column("__TICK", GetType(System.String)))
            'pv_xGrid.Columns("__TICK").Visible = False
            'pv_xGrid.Columns("__TICK").Title = String.Empty
            'pv_xGrid.Columns("__TICK").Width = 20

            For v_intCount = 0 To v_ds.Tables(0).Rows.Count - 1
                With v_ds.Tables(0)
                    v_strFieldCode = .Rows(v_intCount)("FIELDCODE")
                    v_strFieldType = .Rows(v_intCount)("FIELDTYPE")
                    v_strFieldName = .Rows(v_intCount)("FIELDNAME")
                    v_strEnFieldName = .Rows(v_intCount)("EN_FIELDNAME")
                    v_strWidth = CDec(.Rows(v_intCount)("WIDTH"))
                    v_strVisible = .Rows(v_intCount)("DISPLAY")
                End With

                Select Case v_strFieldType
                    Case "D", "P"
                        pv_xGrid.Columns.Add(New Xceed.Grid.Column(v_strFieldCode, GetType(System.String)))
                    Case "N"
                        Dim v_decimalColumn As New Xceed.Grid.Column(v_strFieldCode, GetType(System.Decimal))
                        v_decimalColumn.FormatSpecifier = "#,##0.00"
                        pv_xGrid.Columns.Add(v_decimalColumn)
                    Case "I"
                        Dim v_integerColumn As New Xceed.Grid.Column(v_strFieldCode, GetType(Integer))
                        v_integerColumn.FormatSpecifier = "#,##0"
                        pv_xGrid.Columns.Add(v_integerColumn)
                    Case "L"
                        Dim v_longColumn As New Xceed.Grid.Column(v_strFieldCode, GetType(Long))
                        v_longColumn.FormatSpecifier = "#,##0"
                        pv_xGrid.Columns.Add(v_longColumn)
                    Case "C"
                        pv_xGrid.Columns.Add(New Xceed.Grid.Column(v_strFieldCode, GetType(System.String)))
                    Case "B"
                        pv_xGrid.Columns.Add(New Xceed.Grid.Column(v_strFieldCode, GetType(System.Boolean)))
                    Case "O"
                        Dim o As New Xceed.Grid.Column(v_strFieldCode, GetType(Icon))
                        o.CellViewer = New IconViewer
                        pv_xGrid.Columns.Add(o)
                End Select
                If Not v_strFieldCode Is Nothing Then
                    pv_xGrid.Columns(v_strFieldCode).Title = IIf(pv_strUserLanguage = gc_LANG_VIETNAMESE, v_strFieldName, v_strEnFieldName)
                    pv_xGrid.Columns(v_strFieldCode).Width = v_strWidth
                    pv_xGrid.Columns(v_strFieldCode).Visible = (v_strVisible = "Y")
                End If
            Next

            v_obj.CloseConnection()
            v_obj = Nothing
        Catch ex As Exception
            'LogError.Write(ex.Message & vbNewLine & ex.StackTrace, EventLogEntryType.Error)
            Throw ex
        End Try
    End Sub

    '----------------------------------------------------------------------------------------------
    ' + Mục đích:   Fill dữ liệu vào GRID
    ' + ?ầu vào:    
    '       - pv_xGrid:         GRID cần fill dữ liệu
    '       - pv_strObjMsg:     Message trả v? chứa kết quả tìm kiếm
    '       - pv_strTable:      Tên bảng
    '       - pv_strFilter:     Filter
    ' + ?ầu ra:     GRID được fill dữ liệu
    ' + Trả v?:     N/A
    ' + Tác giả:    Trần Ki?u Minh
    ' + Ghi chú:    N/A
    '----------------------------------------------------------------------------------------------
    Public Sub FillDataGrid(ByVal pv_xGrid As GridControl, _
                            ByVal pv_strTellerID As String, _
                            ByVal pv_strObjMsg As String, _
                            ByVal pv_strResource As String, _
                            Optional ByVal pv_strTable As String = "", _
                            Optional ByVal pv_strFilter As String = "", _
                            Optional ByVal pv_intFromrow As Int32 = 0, _
                            Optional ByVal pv_intTorow As Int32 = 0, _
                            Optional ByVal pv_intTotalrow As Int32 = 0, _
                            Optional ByVal ImgTool As ImageList = Nothing)

        Try
            Dim v_xmlDocument As New Xml.XmlDocument
            Dim v_nodeList As Xml.XmlNodeList
            Dim v_xColumn As Xceed.Grid.Column
            Dim v_int, v_intCount As Integer
            Dim v_strValue As String
            Dim v_strFLDNAME As String
            Dim v_strFLDTYPE As String


            v_xmlDocument.LoadXml(pv_strObjMsg)
            v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")

            pv_xGrid.DataRows.Clear()
            pv_xGrid.BeginInit()
            'If rowperpage = 0 Then
            '    rowperpage = v_nodeList.Count
            'End If

            For v_intCount = 0 To v_nodeList.Count - 1
                System.Windows.Forms.Application.DoEvents()
                'If (v_intCount >= v_nodeList.Count - rowperpage) Then
                Dim v_xDataRow As Xceed.Grid.DataRow = pv_xGrid.DataRows.AddNew()

                For Each v_xColumn In pv_xGrid.Columns
                    For v_int = 0 To v_nodeList.Item(v_intCount).ChildNodes.Count - 1
                        With v_nodeList.Item(v_intCount).ChildNodes(v_int)
                            v_strValue = .InnerText.ToString
                            v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                            v_strFLDTYPE = CStr(CType(.Attributes.GetNamedItem("fldtype"), Xml.XmlAttribute).Value)
                            If UCase(v_strFLDNAME) = UCase(Trim(v_xColumn.FieldName)) Then
                                If UCase(v_xColumn.FieldName) <> "SIGNATURE" Then
                                    If v_xColumn.DataType.Name = GetType(System.Boolean).Name Then
                                        v_xDataRow.Cells(v_xColumn.FieldName).Value = IIf(v_strValue = "0", False, True)
                                    Else
                                        Select Case v_xColumn.DataType.Name
                                            Case GetType(System.String).Name
                                                v_xDataRow.Cells(v_xColumn.FieldName).Value = IIf(v_strValue Is DBNull.Value, "", CStr(v_strValue))
                                            Case GetType(System.Decimal).Name
                                                If v_strValue = "" Then
                                                    v_strValue = 0
                                                End If
                                                'tuanta 
                                                'purpose : change . to , in value from database
                                                'bangpv: sửa sai trên form frmmaintenance với trường số ở các form đăng ký chứng khoán, tcph, tvlk
                                                'System.Threading.Thread.CurrentThread.CurrentCulture.
                                                If System.Threading.Thread.CurrentThread.CurrentCulture.IetfLanguageTag = "en-US" Then
                                                    v_xDataRow.Cells(v_xColumn.FieldName).Value = IIf(v_strValue Is DBNull.Value, 0, CDec(v_strValue))
                                                Else
                                                    v_xDataRow.Cells(v_xColumn.FieldName).Value = IIf(v_strValue Is DBNull.Value, 0, CDec(Replace(v_strValue, ".", ",")))
                                                End If
                                                'ebd bangpv 
                                                'end tuanta
                                            Case GetType(Integer).Name
                                                v_xDataRow.Cells(v_xColumn.FieldName).Value = IIf(v_strValue Is DBNull.Value, 0, CInt(v_strValue))
                                            Case GetType(Long).Name
                                                v_xDataRow.Cells(v_xColumn.FieldName).Value = IIf(v_strValue Is DBNull.Value, 0, CLng(v_strValue))
                                            Case GetType(Double).Name
                                                'tuanta 
                                                'purpose : change . to , in value from database
                                                'v_xDataRow.Cells(v_xColumn.FieldName).Value = IIf(v_strValue Is DBNull.Value, 0, CDbl(v_strValue))
                                                v_xDataRow.Cells(v_xColumn.FieldName).Value = IIf(v_strValue Is DBNull.Value, 0, CDbl(Replace(v_strValue, ".", ",")))
                                                'end tuanta
                                            Case GetType(System.DateTime).Name
                                                v_xDataRow.Cells(v_xColumn.FieldName).Value = IIf(v_strValue Is DBNull.Value, "", CDate(v_strValue).ToString("dd/MM/yyyy"))
                                            Case GetType(System.Drawing.Icon).Name
                                                v_xDataRow.Cells(v_xColumn.FieldName).Value = Icon.FromHandle(CType(ImgTool.Images(CInt(v_strValue)), Bitmap).GetHicon())
                                            Case Else
                                                v_xDataRow.Cells(v_xColumn.FieldName).Value = IIf(v_strValue Is DBNull.Value, "", v_strValue)
                                        End Select
                                    End If
                                    v_xDataRow.EndEdit()

                                End If
                            End If
                        End With
                    Next
                Next
                '  End If
            Next

            pv_xGrid.EndInit()
            _FormatGridBefore(pv_xGrid, pv_strTellerID, pv_strTable, pv_strResource, False, , pv_intFromrow, pv_intTorow, pv_intTotalrow)
        Catch ex As Exception
            'LogError.Write(ex.Message & vbNewLine & ex.StackTrace, EventLogEntryType.Error)
            Throw ex
        End Try
    End Sub

    '----------------------------------------------------------------------------------------------
    ' + Mục đích:   Phân tích xâu các ký tự điều kiện tìm kiếm
    ' + Đầu vào:    
    '       - pv_strOperator:   Xâu các ký tự điều kiện tìm kiếm
    ' + Đầu ra:
    '       - pv_arrOperator:   Mảng chứa các toán tử tìm kiếm
    ' + Trả về:     N/A
    ' + Tác giả:    Trần Kiều Minh
    ' + Ghi chú:    N/A
    '----------------------------------------------------------------------------------------------
    Public Sub AnalyzeOperator(ByVal pv_strOperator As String, ByRef pv_arrOperator() As String)
        Try
            pv_arrOperator = pv_strOperator.Split(",")

            For i As Integer = 0 To pv_arrOperator.Length - 1
                pv_arrOperator(i) = Trim(pv_arrOperator(i))
            Next
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub PrepareSearchParams(ByVal pv_strUserLanguage As String, ByVal pv_ds As DataSet, ByRef pv_strSrTitle As String, ByRef pv_strSrEnTitle As String, _
                                   ByRef pv_strSrCmd As String, ByRef pv_strSrObjName As String, _
                                   ByRef pv_strFrmName As String, ByRef pv_arrSrFieldCode() As String, _
                                   ByRef pv_arrSrFieldName() As String, ByRef pv_arrSrFieldType() As String, _
                                   ByRef pv_arrSrFieldMask() As String, ByRef pv_arrSrFieldDefValue() As String, ByRef pv_arrSrFieldOperator() As String, _
                                   ByRef pv_arrSrFieldFormat() As String, ByRef pv_arrSrFieldDisplay() As String, _
                                   ByRef pv_arrSrFieldWidth() As Integer, ByRef pv_arrSrLookupSql() As String, _
                                   ByRef pv_strKeyColumn As String, ByRef pv_strKeyFieldType As String, ByRef pv_intSearchNum As Integer, _
                                   ByRef pv_strRefColumn As String, ByRef pv_strRefFieldType As String, Optional ByRef pv_strSrOderByCmd As String = "", Optional ByRef pv_strTLTXCD As String = "", _
                                   Optional ByRef pv_strFldConditionChk As String = "")
        Dim v_strKeyValue As String, v_strSrch As String = "", v_strRefValue As String
        Dim v_strOderbycmdsql As String = "", v_strSrTitle As String = "", v_strSrEnTitle As String = ""
        Dim v_strSrCmd As String = "", v_strSrObjName As String = "", v_strFrmName As String = "", v_strSrFieldCode As String = ""
        Dim v_strSrFieldName As String = "", v_strSrEnFieldName As String = "", v_strSrFieldType As String = ""
        Dim v_strSrFieldMask As String = "", v_strSrFieldDefValue As String = ""
        Dim v_strSrFieldOperator As String = "", v_strSrFieldFormat As String = ""
        Dim v_strSrFieldDisplay As String = "", v_strSrLookupSql As String = ""
        Dim v_intSrFieldWidth As Integer, v_strLstTable As String = "", v_strFldConditionChk As String = ""

        Try
            pv_intSearchNum = 0


            ReDim pv_arrSrFieldCode(pv_ds.Tables(0).Rows.Count)
            ReDim pv_arrSrFieldName(pv_ds.Tables(0).Rows.Count)
            ReDim pv_arrSrFieldType(pv_ds.Tables(0).Rows.Count)
            ReDim pv_arrSrFieldMask(pv_ds.Tables(0).Rows.Count)
            ReDim pv_arrSrFieldDefValue(pv_ds.Tables(0).Rows.Count)
            ReDim pv_arrSrFieldOperator(pv_ds.Tables(0).Rows.Count)
            ReDim pv_arrSrFieldFormat(pv_ds.Tables(0).Rows.Count)
            ReDim pv_arrSrFieldDisplay(pv_ds.Tables(0).Rows.Count)
            ReDim pv_arrSrFieldWidth(pv_ds.Tables(0).Rows.Count)
            ReDim pv_arrSrLookupSql(pv_ds.Tables(0).Rows.Count)
            'ReDim pv_arrSrLstTable(v_nodeList.Count)

            For i As Integer = 0 To pv_ds.Tables(0).Rows.Count - 1
                With pv_ds.Tables(0)
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
                        pv_strKeyColumn = v_strSrFieldCode
                        pv_strKeyFieldType = v_strSrFieldType
                    End If
                    v_strRefValue = Trim(.Rows(i)("REFVALUE"))

                    If v_strRefValue = "Y" Then
                        pv_strRefColumn = v_strSrFieldCode
                        pv_strRefFieldType = v_strSrFieldType
                    End If
                    v_intSrFieldWidth = CInt(Trim(.Rows(i)("WIDTH")))
                    pv_strTLTXCD = Trim(.Rows(i)("TLTXCD"))
                    v_strSrLookupSql = Trim(.Rows(i)("LOOKUPCMDSQL"))
                    'Case "LST_TABLE"
                    'v_strLstTable = Trim(v_strValue)
                End With

                If v_strSrch = "Y" Then
                    pv_intSearchNum += 1

                    If pv_intSearchNum = 1 Then
                        pv_strSrTitle = v_strSrTitle
                        pv_strSrEnTitle = v_strSrEnTitle
                        pv_strSrCmd = v_strSrCmd
                        pv_strSrOderByCmd = v_strOderbycmdsql
                        pv_strSrObjName = v_strSrObjName
                        pv_strFrmName = v_strFrmName
                        pv_strFldConditionChk = v_strFldConditionChk
                    End If
                    pv_arrSrFieldCode(pv_intSearchNum) = v_strSrFieldCode
                    pv_arrSrFieldName(pv_intSearchNum) = IIf(pv_strUserLanguage = gc_LANG_VIETNAMESE, v_strSrFieldName, v_strSrEnFieldName)
                    pv_arrSrFieldType(pv_intSearchNum) = v_strSrFieldType
                    pv_arrSrFieldMask(pv_intSearchNum) = v_strSrFieldMask
                    pv_arrSrFieldDefValue(pv_intSearchNum) = v_strSrFieldDefValue
                    pv_arrSrFieldOperator(pv_intSearchNum) = v_strSrFieldOperator
                    pv_arrSrFieldFormat(pv_intSearchNum) = v_strSrFieldFormat
                    pv_arrSrFieldDisplay(pv_intSearchNum) = v_strSrFieldDisplay
                    pv_arrSrFieldWidth(pv_intSearchNum) = v_intSrFieldWidth
                    pv_arrSrLookupSql(pv_intSearchNum) = v_strSrLookupSql
                    'pv_arrSrLstTable(pv_intSearchNum) = v_strLstTable
                End If
            Next

            If pv_intSearchNum > 0 Then
                ReDim Preserve pv_arrSrFieldCode(pv_intSearchNum)
                ReDim Preserve pv_arrSrFieldName(pv_intSearchNum)
                ReDim Preserve pv_arrSrFieldType(pv_intSearchNum)
                ReDim Preserve pv_arrSrFieldMask(pv_intSearchNum)
                ReDim Preserve pv_arrSrFieldDefValue(pv_intSearchNum)
                ReDim Preserve pv_arrSrFieldOperator(pv_intSearchNum)
                ReDim Preserve pv_arrSrFieldFormat(pv_intSearchNum)
                ReDim Preserve pv_arrSrFieldDisplay(pv_intSearchNum)
                ReDim Preserve pv_arrSrFieldWidth(pv_intSearchNum)
                ReDim Preserve pv_arrSrLookupSql(pv_intSearchNum)
                'ReDim Preserve pv_arrSrLstTable(pv_intSearchNum)
            End If
        Catch ex As Exception
            'LogError.Write(ex.Message & vbNewLine & ex.StackTrace, EventLogEntryType.Error)
            Throw ex
        End Try
    End Sub

    Public Sub FillComboEx(ByVal pv_strObjMsg As String, ByRef pv_cbo As ComboBoxEx)
        Dim v_xmlDocument As New Xml.XmlDocument
        Dim v_nodeList As Xml.XmlNodeList
        Dim v_strFLDNAME, v_strValue As String
        Dim v_arrValue(), v_arrDisplay() As String
        Dim v_int As Integer

        Try
            v_int = 0

            v_xmlDocument.LoadXml(pv_strObjMsg)
            v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")

            ReDim v_arrValue(v_nodeList.Count)
            ReDim v_arrDisplay(v_nodeList.Count)

            For i As Integer = 0 To v_nodeList.Count - 1
                v_int += 1

                For j As Integer = 0 To v_nodeList.Item(i).ChildNodes.Count - 1
                    With v_nodeList.Item(i).ChildNodes(j)
                        v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                        v_strValue = .InnerText.ToString

                        Select Case Trim(v_strFLDNAME)
                            Case "VALUE"
                                v_arrValue(v_int) = Trim(v_strValue)
                            Case "DISPLAY"
                                v_arrDisplay(v_int) = Trim(v_strValue)
                        End Select
                    End With
                Next
            Next
            pv_cbo.Clears()
            For i As Integer = 1 To v_int
                pv_cbo.AddItems(v_arrDisplay(i), v_arrValue(i))
            Next
        Catch ex As Exception
            'LogError.Write(ex.Message & vbNewLine & ex.StackTrace, EventLogEntryType.Error)
            Throw ex
        End Try
    End Sub

    Public Sub FillComboExRefData(ByVal pv_strObjMsg As String, ByRef pv_cbo As ComboBoxEx, ByVal pv_strREFFLDNAME As String)
        Dim v_xmlDocument As New Xml.XmlDocument
        Dim v_nodeList As Xml.XmlNodeList
        Dim v_strREFNAME, v_strFLDNAME, v_strValue As String
        Dim v_arrRef() As Boolean, v_arrValue(), v_arrDisplay() As String
        Dim v_int As Integer

        Try
            v_int = 0

            v_xmlDocument.LoadXml(pv_strObjMsg)
            v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjDataRef")
            ReDim v_arrRef(v_nodeList.Count)
            ReDim v_arrValue(v_nodeList.Count)
            ReDim v_arrDisplay(v_nodeList.Count)

            For i As Integer = 0 To v_nodeList.Count - 1
                v_int += 1
                v_arrRef(v_int) = False
                For j As Integer = 0 To v_nodeList.Item(i).ChildNodes.Count - 1
                    With v_nodeList.Item(i).ChildNodes(j)
                        v_strREFNAME = CStr(CType(.Attributes.GetNamedItem("refname"), Xml.XmlAttribute).Value)
                        v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                        If Trim(pv_strREFFLDNAME) = Trim(v_strREFNAME) Then
                            v_arrRef(v_int) = True
                            v_strValue = .InnerText.ToString
                            Select Case Trim(v_strFLDNAME)
                                Case "VALUE"
                                    v_arrValue(v_int) = Trim(v_strValue)
                                Case "DISPLAY"
                                    v_arrDisplay(v_int) = Trim(v_strValue)
                            End Select
                        End If
                    End With
                Next
            Next
            pv_cbo.Clears()
            For i As Integer = 1 To v_int
                If v_arrRef(i) Then
                    pv_cbo.AddItems(v_arrDisplay(i), v_arrValue(i))
                End If
            Next
        Catch ex As Exception
            'LogError.Write(ex.Message & vbNewLine & ex.StackTrace, EventLogEntryType.Error)
            Throw ex
        End Try
    End Sub
End Module
