Imports System.Windows.Forms
Imports Xceed.SmartUI.Controls
Imports Sats.CommonLibrary
Imports System.Collections

Public Class frmLookUp
    Inherits System.Windows.Forms.Form
    Public v_dtgLookupData As New GridEx
    Private mv_blnKey As Boolean = True

#Region " Declare constant and variables "
    Const c_ResourceManager = "Sats.AppCore.frmLookup_"
    Const WIDTH_GRID_LOOKUP = 550

    Const SEARCH_OPTION_BEGIN = "BeginWith"
    Const SEARCH_OPTION_CONTAINS = "Contains"

    Private mv_blnAutoClosed As Boolean = False
    Private mv_blnAcceptedClose As Boolean = True
    Private mv_strCaption As String
    Private mv_strSQLCommand As String
    Private mv_strReturnData As String
    Private mv_strReturnDisplay As String
    Private mv_strLanguage As String
    Private mv_ResourceManager As Resources.ResourceManager
    Private mv_strXMLData As String
    Private mv_strTellerID As String
    Private mv_oProxy As BDSChannel.BDSDelivery
#End Region
#Region " Properties "
    Public Property Proxy() As BDSChannel.BDSDelivery
        Get
            Return mv_oProxy
        End Get
        Set(ByVal value As BDSChannel.BDSDelivery)
            mv_oProxy = value
        End Set
    End Property

    Public Property AcceptedClose() As Boolean
        Get
            Return mv_blnAcceptedClose
        End Get
        Set(ByVal Value As Boolean)
            mv_blnAcceptedClose = Value
        End Set
    End Property

    Public Property AutoClosed() As Boolean
        Get
            Return mv_blnAutoClosed
        End Get
        Set(ByVal Value As Boolean)
            mv_blnAutoClosed = Value
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

    Public Property CAPTION() As String
        Get
            Return mv_strCaption
        End Get
        Set(ByVal Value As String)
            mv_strCaption = Value
        End Set
    End Property

    Public Property SQLCMD() As String
        Get
            Return mv_strSQLCommand
        End Get
        Set(ByVal Value As String)
            mv_strSQLCommand = Value
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

    Public Property RETURNDISPLAY() As String
        Get
            Return mv_strReturnDisplay
        End Get
        Set(ByVal value As String)
            mv_strReturnDisplay = value
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

    Public Property TellerID() As String
        Get
            Return mv_strTellerID
        End Get
        Set(ByVal value As String)
            mv_strTellerID = value
        End Set
    End Property
#End Region
#Region " Other methods "
    Protected Overridable Sub InitDialog()
        'Khởi tạo kích thước form và load resource
        mv_ResourceManager = New Resources.ResourceManager(c_ResourceManager & UserLanguage, System.Reflection.Assembly.GetExecutingAssembly())
        LoadResource(Me)

        'Thiết lập các thuộc tính ban đầu cho form
        DoResizeForm()

        'Load search option
        With cboSearchOption
            .Clears()
            .AddItems(mv_ResourceManager.GetString(SEARCH_OPTION_BEGIN), SEARCH_OPTION_BEGIN)
            .AddItems(mv_ResourceManager.GetString(SEARCH_OPTION_CONTAINS), SEARCH_OPTION_CONTAINS)
            .SelectedIndex = 0
        End With

        'Nạp dữ liệu hiển thị thông tin tra cứu
        If Len(Trim(SQLCMD)) > 0 Then
            LoadLookupData()
        End If

        'Set focus on txtSearch control
        Me.ActiveControl = Me.txtSearch
    End Sub

    Private Sub LoadLookupData()
        Dim v_nodeList As Xml.XmlNodeList
        Dim v_strValue As String, v_strFLDNAME As String
        Dim v_strFLDTYPE As String, v_strTEXT As String = ""

        Try
            'Create message to inquiry object fields
            Dim v_strObjMsg As String
            Dim v_xmlDocument As New Xml.XmlDocument

            'Dim v_ws As New BDSChannel.BDSDelivery

            'Lấy thông tin chung về giao dịch
            v_strObjMsg = BuildXMLObjMsg(, , , TellerID, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_LOOKUP, gc_ActionInquiry, SQLCMD, "")
            Dim v_lngError As Long = Proxy.Message(v_strObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Exit Sub
            End If
            'Lưu trữ danh sách tìm kiếm trả về
            FULLDATA = v_strObjMsg

            v_xmlDocument.LoadXml(v_strObjMsg)
            v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")
            Dim i, j As Integer

            'Hiển thị toàn bộ nội dung của dữ liệu tìm kiếm trả về
            If v_nodeList.Count > 0 Then
                v_dtgLookupData.Dock = DockStyle.Fill
                Dim v_cmrHeader As New Xceed.Grid.ColumnManagerRow
                v_cmrHeader.BackColor = System.Drawing.Color.FromArgb(CType(64, Byte), CType(216, Byte), CType(84, Byte), CType(2, Byte))
                v_cmrHeader.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
                v_dtgLookupData.FixedHeaderRows.Add(v_cmrHeader)
                'Tạo Header của Grid
                For j = 0 To v_nodeList.Item(0).ChildNodes.Count - 1
                    With v_nodeList.Item(0).ChildNodes(j)
                        v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                        v_strFLDTYPE = CStr(CType(.Attributes.GetNamedItem("fldtype"), Xml.XmlAttribute).Value)
                        v_dtgLookupData.Columns.Add(New Xceed.Grid.Column(v_strFLDNAME, GetType(System.String)))
                        v_dtgLookupData.Columns(v_strFLDNAME).Title = UCase(v_strFLDNAME)
                        v_dtgLookupData.Columns(v_strFLDNAME).HorizontalAlignment = Xceed.Grid.HorizontalAlignment.Default
                        Select Case Trim(v_strFLDNAME)
                            Case "VALUE"
                                v_dtgLookupData.Columns(v_strFLDNAME).Width = 0
                            Case "VALUECD"
                                v_dtgLookupData.Columns(v_strFLDNAME).Width = 2 * WIDTH_GRID_LOOKUP / 10
                            Case "DESCRIPTION"
                                v_dtgLookupData.Columns(v_strFLDNAME).Width = 3 * WIDTH_GRID_LOOKUP / 10
                            Case "DISPLAY"
                                If UserLanguage = "EN" Then
                                    v_dtgLookupData.Columns(v_strFLDNAME).Width = 0
                                Else
                                    v_dtgLookupData.Columns(v_strFLDNAME).Width = 5 * WIDTH_GRID_LOOKUP / 10
                                End If
                            Case "EN_DISPLAY"
                                If UserLanguage <> "EN" Then
                                    v_dtgLookupData.Columns(v_strFLDNAME).Width = 0
                                Else
                                    v_dtgLookupData.Columns(v_strFLDNAME).Width = 5 * WIDTH_GRID_LOOKUP / 10
                                End If
                            Case Else
                                v_dtgLookupData.Columns(v_strFLDNAME).Width = 0
                        End Select

                    End With
                Next
                If v_dtgLookupData.DataRowTemplate.Cells.Count >= 0 Then
                    'Bắt sự kiện Double Click và KeyUp
                    For i = 0 To v_dtgLookupData.DataRowTemplate.Cells.Count - 1
                        AddHandler v_dtgLookupData.DataRowTemplate.Cells(i).DoubleClick, AddressOf Grid_DblClick
                    Next
                End If
                AddHandler v_dtgLookupData.DataRowTemplate.KeyUp, AddressOf Grid_KeyUp

                'Điền thông tin tra cứu
                v_dtgLookupData.DataRows.Clear()
                v_dtgLookupData.BeginInit()
                For i = 0 To v_nodeList.Count - 1
                    'Tạo row dữ liệu
                    Dim v_xDataRow As Xceed.Grid.DataRow = v_dtgLookupData.DataRows.AddNew()
                    For j = 0 To v_nodeList.Item(i).ChildNodes.Count - 1
                        With v_nodeList.Item(i).ChildNodes(j)
                            v_strValue = Trim(.InnerText)
                            v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                            v_xDataRow.Cells(v_strFLDNAME).Value = v_strValue
                            Select Case Trim(v_strFLDNAME)
                                Case "VALUE"
                                    v_strTEXT = v_strValue
                                Case "DESCRIPTION"
                                    v_strTEXT = v_strTEXT & ControlChars.Tab & v_strValue
                            End Select
                        End With
                    Next
                    'Dùng để trả về giá trị RETURNDATA cho form lookup
                    v_xDataRow.Cells("VALUE").Value = v_strTEXT
                    v_xDataRow.EndEdit()
                Next
                v_dtgLookupData.EndInit()
                Me.pnLookup.Controls.Add(v_dtgLookupData)

                'Tự động đóng màn hình nếu chỉ có 01 bản ghi và AutoClosed=True
                If v_nodeList.Count = 1 And AutoClosed Then
                    OnAccept()
                End If
                pnLookup.Select()
            End If

        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                                     & "Error code: System error!" & vbNewLine _
                                                     & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
            mv_blnKey = False
        End Try
    End Sub

    Private Sub DoResizeForm()

    End Sub

    Private Sub Grid_DblClick(ByVal sender As Object, ByVal e As System.EventArgs)
        OnAccept()
    End Sub

    Private Sub Grid_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        Try
            Select Case e.KeyCode
                Case Keys.Enter
                    OnAccept()
            End Select
        Catch ex As Exception
            MsgBox(ex.ToString)
            mv_blnKey = False
        End Try
    End Sub

    Private Sub OnClose()
        Me.Close()
    End Sub

    Private Sub OnAccept()
        If v_dtgLookupData.DataRows.Count > 0 Then
            RETURNDATA = CType(v_dtgLookupData.CurrentRow, Xceed.Grid.DataRow).Cells("VALUE").Value
            RETURNDISPLAY = CType(v_dtgLookupData.CurrentRow, Xceed.Grid.DataRow).Cells("DISPLAY").Value
        End If
        Me.Close()
    End Sub


    Private Sub OnSearch()
        Dim v_blnItemFound As Boolean = False
        Dim v_intIndex As Int16, v_strText As String = Trim(txtSearch.Text)
        Dim v_intOldIndex As Integer = v_dtgLookupData.DataRows.IndexOf(v_dtgLookupData.CurrentRow)
        Dim v_strValue As String
        v_intOldIndex = IIf(v_intOldIndex = 0, -1, v_intOldIndex)
        If (v_strText.Length > 0) Then
            Select Case cboSearchOption.SelectedValue
                Case SEARCH_OPTION_BEGIN
                    For v_intIndex = v_intOldIndex + 1 To v_dtgLookupData.DataRows.Count - 1
                        v_strValue = CType(v_dtgLookupData.DataRows(v_intIndex), Xceed.Grid.DataRow).Cells("VALUECD").Value.ToString().ToUpper()

                        If (v_strValue.IndexOf(v_strText.ToUpper) = 0) Then
                            v_dtgLookupData.CurrentRow = v_dtgLookupData.DataRows.Item(v_intIndex)
                            v_dtgLookupData.SelectedRows.Clear()
                            v_dtgLookupData.SelectedRows.Add(v_dtgLookupData.CurrentRow)
                            For i As Integer = 0 To v_dtgLookupData.DataRows.IndexOf(v_dtgLookupData.CurrentRow) - v_intOldIndex - 1
                                v_dtgLookupData.Scroll(Xceed.Grid.ScrollDirection.Down)
                            Next i
                            v_blnItemFound = True
                            Exit For
                        End If
                    Next v_intIndex
                Case SEARCH_OPTION_CONTAINS
                    For v_intIndex = v_intOldIndex + 1 To v_dtgLookupData.DataRows.Count - 1
                        If InStr(UCase(CType(v_dtgLookupData.DataRows(v_intIndex), Xceed.Grid.DataRow).Cells("VALUE").Value), UCase(v_strText)) > 0 _
                            Or InStr(UCase(CType(v_dtgLookupData.DataRows(v_intIndex), Xceed.Grid.DataRow).Cells("DESCRIPTION").Value), UCase(v_strText)) > 0 Then
                            v_dtgLookupData.CurrentRow = v_dtgLookupData.DataRows.Item(v_intIndex)
                            v_dtgLookupData.SelectedRows.Clear()
                            v_dtgLookupData.SelectedRows.Add(v_dtgLookupData.CurrentRow)
                            For i As Integer = 0 To v_dtgLookupData.DataRows.IndexOf(v_dtgLookupData.CurrentRow) - v_intOldIndex - 1
                                v_dtgLookupData.Scroll(Xceed.Grid.ScrollDirection.Down)
                            Next
                            v_blnItemFound = True
                            Exit For
                        End If
                    Next v_intIndex
            End Select

            If (Not v_blnItemFound) Then
                If (MessageBox.Show(mv_ResourceManager.GetString("SearchConfirm"), gc_ApplicationTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes) Then
                    'Move to the top of list
                    v_dtgLookupData.Scroll(Xceed.Grid.ScrollDirection.TopPage)
                    v_dtgLookupData.CurrentRow = v_dtgLookupData.DataRows.Item(0)
                    v_dtgLookupData.SelectedRows.Clear()
                    v_dtgLookupData.SelectedRows.Add(v_dtgLookupData.CurrentRow)
                Else
                    mv_blnKey = False
                End If
            End If
        End If
    End Sub

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

        Me.Text = mv_ResourceManager.GetString("frmLookUp")
    End Sub
#End Region

#Region " Form events "
    Private Sub frmLookUp_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        InitDialog()
    End Sub

    Private Sub frmLookUp_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        DoResizeForm()
    End Sub

    Private Sub btnCANCEL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        OnClose()
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        OnAccept()
    End Sub

    Private Sub txtSearch_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSearch.GotFocus
        txtSearch.SelectionStart = 0
        txtSearch.SelectionLength = Len(Trim(txtSearch.Text))
    End Sub

    Private Sub frmLookUp_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        Select Case e.KeyCode
            Case Keys.Escape
                OnClose()
            Case Keys.Enter
                If mv_blnKey Then
                    If Me.ActiveControl.Name = "txtSearch" Then
                        If Len(Trim(CType(Me.ActiveControl, TextBox).Text)) > 0 Then
                            OnSearch()
                        End If
                    End If
                Else
                    mv_blnKey = True
                End If

        End Select
    End Sub
#End Region
   
    'Private Sub txtSearch_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtSearch.KeyUp
    '    Select Case e.KeyCode
    '        Case Keys.Escape
    '            OnClose()
    '        Case Keys.Enter

    '            If Len(Trim(CType(Me.ActiveControl, TextBox).Text)) > 0 Then
    '                OnSearch()
    '            End If

    '    End Select
    'End Sub
End Class