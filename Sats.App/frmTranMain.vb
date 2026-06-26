Imports Sats.AppCore
Imports Sats.CommonLibrary
Imports Sats.SQLEngine
Imports Sats.ClientCA
Imports System.IO
Imports BkavCASign

Public Class frmTranMain

    'Private mv_BDSDelivery As New BDSChannel.BDSDelivery
    Private mv_lngParentid As Long = 0
    Private mv_blnClose As Boolean = False
    Private mv_strCaption As String
    Private mv_frmTask As TaskList
    'Private v_thread As Threading.Thread
    Dim v_oProccess As ProcessForm
    Private v_strObjMsg As String
    Private mv_intType As Integer
    Private mv_strTltxcd As String
    'Private mv_strTXDATE As String


#Region " Properties "
    Public Property Tltxcd() As String
        Get
            Return mv_strTltxcd
        End Get
        Set(ByVal value As String)
            mv_strTltxcd = value
        End Set
    End Property
    'Public Property TXDATE() As String
    '    Get
    '        Return mv_strTXDATE
    '    End Get
    '    Set(ByVal value As String)
    '        mv_strTXDATE = value
    '    End Set
    'End Property
    Public Property F_Task() As TaskList
        Get
            Return mv_frmTask
        End Get
        Set(ByVal value As TaskList)
            mv_frmTask = value
        End Set
    End Property

    Public Property ParentID() As Long
        Get
            Return mv_lngParentid
        End Get
        Set(ByVal Value As Long)
            mv_lngParentid = Value
        End Set
    End Property
    Public Property CloseStatus() As Boolean
        Get
            Return mv_blnClose
        End Get
        Set(ByVal Value As Boolean)
            mv_blnClose = Value
        End Set
    End Property
    Public Property CaptionText() As String
        Get
            Return mv_strCaption
        End Get
        Set(ByVal Value As String)
            mv_strCaption = Value
        End Set
    End Property
#End Region
#Region "form functions"
    Public Sub New(ByVal pv_strLanguage As String)

        ' This call is required by the Windows Form Designer.
        MyBase.New(pv_strLanguage)
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub frmTranMain_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        'If e.CloseReason = CloseReason.MdiFormClosing Then
        ' e.Cancel = False
        ' Else
        If ModuleCode = "SY" Then
            e.Cancel = Not mv_blnClose
        End If
        'End If
    End Sub

    Private Sub frmTranMain_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'InitDialog()
        'v_oProccess = New ProcessForm(Me)
    End Sub
#End Region
#Region "protected functions"
    ' Xem giao dich
    Protected Overrides Function OnAddNew() As Int32
        OnViewTran()
    End Function
    ' Duyet giao dich
    Protected Overrides Function OnQuery() As Int32
        OnApproveTran()
    End Function
    ' Loai giao dich
    Protected Overrides Function OnUpdate() As Int32
        OnRejectTran()
    End Function
    ' Xoa giao dich
    Protected Overrides Function OnDelete(Optional ByVal pv_strIsLocal As String = "", Optional ByVal pv_strModule As String = "") As Int32
        OnDeleteTran()
    End Function
    ' In giao dich
    Protected Overrides Function OnExecute() As Int32
        'OnPrintTran()
        OnViewRptTrans()
    End Function
#End Region
#Region "private sub"
    ' Xem giao dich
    Private Sub OnViewTran()
        Try
            Dim frm As frmTransactionMaster
            Dim v_strTLTXCD, v_strModeCode, v_strCmdInquiry, _
                v_strTXCODE, v_strObjMsg, v_strTXNUM, v_strTXDATE As String
            Dim v_lngIsParent, v_lngParentID As Long
            Dim drDetail As DialogResult

            Dim v_xmlDocument As New System.Xml.XmlDocument
            Dim v_nodeList As Xml.XmlNodeList
            Dim v_strFLDNAME As String
            Dim v_strValue As String

            ' lay du lieu
            v_strTLTXCD = CType(SearchGrid.CurrentRow, Xceed.Grid.CellRow).Cells("TLTXCD").Value
            v_strTXNUM = CType(SearchGrid.CurrentRow, Xceed.Grid.CellRow).Cells("TXNUM").Value
            v_strTXDATE = CType(SearchGrid.CurrentRow, Xceed.Grid.CellRow).Cells("TXDATE").Value
            v_lngIsParent = CLng(CType(SearchGrid.CurrentRow, Xceed.Grid.CellRow).Cells("ISPARENT").Value)
            v_lngParentID = CLng(CType(SearchGrid.CurrentRow, Xceed.Grid.CellRow).Cells("AUTOID").Value)


            'mv_BDSDelivery = New BDSChannel.BDSDelivery
            'v_strTXCODE = v_strTLTXCD.Substring(0, 2)
            'v_strCmdInquiry = "SELECT MODCODE FROM APPMODULES A,TLTX B WHERE A.TXCODE='" & v_strTXCODE & "' AND SUBSTR(B.TLTXCD,0,2)=A.TXCODE AND B.TLTXCD='" & Trim(v_strTLTXCD) & "'"
            'v_strObjMsg = BuildXMLObjMsg(, , , , gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SY_SEARCHFLD, gc_ActionInquiry, v_strCmdInquiry, , )
            'mv_BDSDelivery.Message(v_strObjMsg)
            'v_xmlDocument.LoadXml(v_strObjMsg)
            'v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")

            'For i As Integer = 0 To v_nodeList.Count - 1
            '    For j As Integer = 0 To v_nodeList.Item(i).ChildNodes.Count - 1
            '        With v_nodeList.Item(i).ChildNodes(j)
            '            v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
            '            v_strValue = .InnerText.ToString
            '            Select Case Trim(v_strFLDNAME)
            '                Case "MODCODE"
            '                    v_strModeCode = Trim(v_strValue)
            '                    Exit For
            '            End Select
            '        End With
            '    Next
            'Next
            'v_strCmdInquiry = "SELECT fldcd, txdesc FROM tllogfld A WHERE A.TXNUM='" & v_strTXNUM _
            '            & "' AND a.txdate=to_date('" & v_strTXDATE & "','dd/mm/yyyy') and a.deleted = 0 and a.status = 0 order by fldcd"
            '' Tao form 
            'frm = New frmTransactionMaster
            'frm.Name = "frmTran" & v_strTXNUM & Replace(v_strTXDATE, "/", "")
            'frm.UserLanguage = m_BusLayer.AppLanguage
            'frm.ObjectName = v_strTLTXCD
            'frm.ModuleCode = v_strModeCode
            'frm.LocalObject = gc_IsLocalMsg
            'frm.BranchId = m_BusLayer.CurrentTellerProfile.BranchId
            'frm.TellerId = m_BusLayer.CurrentTellerProfile.TellerId
            'frm.IpAddress = m_BusLayer.AppIpAddress
            'frm.WsName = m_BusLayer.AppWsName
            'frm.BusDate = m_BusLayer.CurrentTellerProfile.BusDate
            'frm.MemberFilter = m_BusLayer.CurrentTellerProfile.MemberFilter
            'frm.StockFilter = m_BusLayer.CurrentTellerProfile.StockFilter
            'frm.AllMember = m_BusLayer.CurrentTellerProfile.AllMember
            'frm.AllStock = m_BusLayer.CurrentTellerProfile.AllStock

            'frm.ViewSQL = v_strCmdInquiry
            'frm.Text = Me.Text & ": " & v_strTXNUM & "; " & v_strTXDATE
            'frm.ParentID = v_lngIsParent
            'drDetail = frm.ShowDialog()

            'If drDetail = Windows.Forms.DialogResult.OK Then
            If v_lngIsParent = 2 Then
                Dim v_strObjName, v_strModCode, v_strAuthCode, v_strAuthString As String
                ' fix tam
                v_strObjName = "frmTLLOG" & v_strTXNUM & Replace(v_strTXDATE, "/", "")
                v_strModCode = "SY"
                v_strAuthCode = "NNNNYYY"
                v_strAuthString = "NNNNN"
                ' end fix
                Dim frmTranMainChild As New frmTranMain(m_BusLayer.AppLanguage)
                frmTranMainChild.Name = v_strObjName
                frmTranMainChild.BusDate = m_BusLayer.CurrentTellerProfile.BusDate
                frmTranMainChild.TableName = "TLLOG_D"
                frmTranMainChild.AutoID = v_lngParentID
                frmTranMainChild.ModuleCode = v_strModCode
                frmTranMainChild.AuthCode = v_strAuthCode
                frmTranMainChild.AuthString = v_strAuthString
                frmTranMainChild.IsLocalSearch = IIf(Me.TableName = "TLLOG", gc_IsNotLocalMsg, gc_IsInQueryNotLocalMsg)
                frmTranMainChild.SearchOnInit = False
                frmTranMainChild.BranchId = m_BusLayer.CurrentTellerProfile.BranchId
                frmTranMainChild.TellerId = m_BusLayer.CurrentTellerProfile.TellerId
                frmTranMainChild.TellerType = m_BusLayer.CurrentTellerProfile.TellerType
                frmTranMainChild.IpAddress = m_BusLayer.AppIpAddress
                frmTranMainChild.WsName = m_BusLayer.AppWsName
                frmTranMainChild.CaptionText = Me.Text & ": " & v_strTXNUM & "; " & v_strTXDATE
                frmTranMainChild.ParentID = v_lngParentID
                frmTranMainChild.DockPanel = Me.DockPanel
                frmTranMainChild.CloseStatus = True
                frmTranMainChild.MemberFilter = m_BusLayer.CurrentTellerProfile.MemberFilter
                frmTranMainChild.StockFilter = m_BusLayer.CurrentTellerProfile.StockFilter
                frmTranMainChild.AllMember = m_BusLayer.CurrentTellerProfile.AllMember
                frmTranMainChild.AllStock = m_BusLayer.CurrentTellerProfile.AllStock
                frmTranMainChild.F_Task = F_Task
                frmTranMainChild.Tltxcd = v_strTLTXCD
                'frmTranMainChild.TXDATE = v_strTXDATE
                frmTranMainChild.PassDate = m_BusLayer.CurrentTellerProfile.PassDate
                frmTranMainChild.Client = mv_oLocal
                frmTranMainChild.Proxy = pv_oProxy
                frmTranMainChild.InitDialog()
                frmTranMainChild.Show()
            End If
            'End If

        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                         & "Error code: System error!" & vbNewLine _
                                         & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Sub
    ' Duyet giao dich
    Private Sub OnApproveTran()
        Dim i As Integer
        Try
            If Not CType(MyBase.SearchGrid, GridEx).CurrentRow Is Nothing Then
                If CType(MyBase.SearchGrid, GridEx).CurrentRow.GetType.ToString = "Xceed.Grid.DataRow" Then
                    If Not CType(CType(MyBase.SearchGrid, GridEx).CurrentRow, Xceed.Grid.DataRow).Cells("AUTH_STRING").Value Is Nothing Then
                        If CType(CType(MyBase.SearchGrid, GridEx).CurrentRow, Xceed.Grid.DataRow).Cells("AUTH_STRING").Value.ToString.Substring(1, 1) = "Y" Then
                            'Hanm5 edit
                            Dim v_blnChecked As Boolean = False
                            With CType(MyBase.SearchGrid, GridEx)
                                For i = 0 To .DataRows.Count - 1
                                    With .DataRows(i)
                                        If .Cells("TXBLN").Value Then
                                            v_blnChecked = True
                                        End If
                                    End With
                                Next
                            End With

                            If Not v_blnChecked Then
                                MsgBox("Bạn chưa chọn giao dịch nào để duyệt. Đề nghị kích vào nút chọn giao dịch để duyệt", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                                Exit Sub
                            End If
                            'Hanm5 end
                            With CType(MyBase.SearchGrid, GridEx)
                                For i = 0 To .DataRows.Count - 1
                                    With .DataRows(i)
                                        If .Cells("TXBLN").Value Then
                                            Exit For
                                        End If
                                    End With
                                Next
                                If i > .DataRows.Count - 1 Then
                                    MyBase.btnView.Enabled = True
                                    MyBase.btnEdit.Enabled = True
                                    MsgBox(mv_ResourceManager.GetString("SelectTran"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                                    Exit Sub
                                End If
                            End With
                            If MsgBox("Bạn đã chắc chắn muốn duyệt giao dịch này chưa ?", MsgBoxStyle.YesNo, gc_ApplicationTitle) = MsgBoxResult.Yes Then
                                mv_intType = 1
                                SendBDS(mv_intType)
                                MyBase.mv_strGlobalAuthString = ""
                            Else
                                MyBase.btnView.Enabled = True
                            End If
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                         & "Error code: System error!" & vbNewLine _
                                         & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)

        Finally
            GC.Collect()
        End Try
    End Sub
    ' Loai giao dich
    Private Sub OnRejectTran()
        Dim i As Integer

        Try
            If Not CType(MyBase.SearchGrid, GridEx).CurrentRow Is Nothing Then
                If CType(MyBase.SearchGrid, GridEx).CurrentRow.GetType.ToString = "Xceed.Grid.DataRow" Then
                    If Not CType(CType(MyBase.SearchGrid, GridEx).CurrentRow, Xceed.Grid.DataRow).Cells("AUTH_STRING").Value Is Nothing Then
                        If CType(CType(MyBase.SearchGrid, GridEx).CurrentRow, Xceed.Grid.DataRow).Cells("AUTH_STRING").Value.ToString.Substring(2, 1) = "Y" Then
                            With CType(MyBase.SearchGrid, GridEx)
                                Dim v_blnChecked As Boolean = False
                                With CType(MyBase.SearchGrid, GridEx)
                                    For i = 0 To .DataRows.Count - 1
                                        With .DataRows(i)
                                            If .Cells("TXBLN").Value Then
                                                v_blnChecked = True
                                            End If
                                        End With
                                    Next
                                End With

                                If Not v_blnChecked Then
                                    MsgBox("Bạn chưa chọn giao dịch nào để duyệt. Đề nghị kích vào nút chọn giao dịch để duyệt", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                                    Exit Sub
                                End If

                                For i = 0 To .DataRows.Count - 1
                                    With .DataRows(i)
                                        If .Cells("TXBLN").Value Then
                                            Exit For
                                        End If
                                    End With
                                Next
                                If i > .DataRows.Count - 1 Then
                                    MyBase.btnView.Enabled = True
                                    MyBase.btnEdit.Enabled = True
                                    MsgBox(mv_ResourceManager.GetString("SelectTran"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                                    Exit Sub
                                End If
                            End With

                            If MsgBox("Bạn đã chắc chắn muốn loại giao dịch này chưa ?", MsgBoxStyle.YesNo, gc_ApplicationTitle) = MsgBoxResult.Yes Then
                                mv_intType = 2
                                SendBDS(mv_intType)
                                MyBase.mv_strGlobalAuthString = ""
                            Else
                                MyBase.btnEdit.Enabled = True
                            End If
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                         & "Error code: System error!" & vbNewLine _
                                         & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try

    End Sub
    ' Xoa giao dich
    Private Sub OnDeleteTran()
        Dim i As Integer
        Try
            If Not CType(MyBase.SearchGrid, GridEx).CurrentRow Is Nothing Then
                If CType(MyBase.SearchGrid, GridEx).CurrentRow.GetType.ToString = "Xceed.Grid.DataRow" Then
                    If Not CType(CType(MyBase.SearchGrid, GridEx).CurrentRow, Xceed.Grid.DataRow).Cells("AUTH_STRING").Value Is Nothing Then
                        If CType(CType(MyBase.SearchGrid, GridEx).CurrentRow, Xceed.Grid.DataRow).Cells("AUTH_STRING").Value.ToString.Substring(4, 1) = "Y" Then
                            With CType(MyBase.SearchGrid, GridEx)
                                'For i = 0 To .DataRows.Count - 1
                                '    With .DataRows(i)
                                '        If .Cells("TXBLN").Value Then
                                '            Exit For
                                '        End If
                                '    End With
                                'Next
                                'If i > .DataRows.Count - 1 Then
                                '    MyBase.btnDelete.Enabled = True
                                '    MsgBox(mv_ResourceManager.GetString("SelectTran"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                                '    Exit Sub
                                'End If
                                'Hanm5 edit
                                Dim v_blnChecked As Boolean = False
                                With CType(MyBase.SearchGrid, GridEx)
                                    For i = 0 To .DataRows.Count - 1
                                        With .DataRows(i)
                                            If .Cells("TXBLN").Value Then
                                                v_blnChecked = True
                                            End If
                                        End With
                                    Next
                                End With

                                If Not v_blnChecked Then
                                    MsgBox("Bạn chưa chọn giao dịch nào để duyệt. Đề nghị kích vào nút chọn giao dịch để duyệt", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                                    Exit Sub
                                End If
                                'Hanm5 end
                            End With
                            If MsgBox("Bạn đã chắc chắn muốn xóa giao dịch này chưa ?", MsgBoxStyle.YesNo, gc_ApplicationTitle) = MsgBoxResult.Yes Then
                                mv_intType = 3
                                SendBDS(mv_intType)
                                MyBase.mv_strGlobalAuthString = ""
                            End If
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                         & "Error code: System error!" & vbNewLine _
                                         & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Sub
    ' In giao dich
    Private Sub OnPrintTran()
        If MsgBox("Bạn đã chắc chắn muốn in giao dịch này chưa ?", MsgBoxStyle.YesNo, "Thông báo") = MsgBoxResult.Yes Then

        End If
    End Sub

    Public Overrides Sub InitDialog()
        Try
            MyBase.InitDialog()
            Dim intIndex, intMaxNo As Integer
            Dim strValues, strDetailText As String
            Dim v_strClause As String

            v_strClause = "SELECT a.OBJNAME || a.FLDNAME KEY, a.CAPTION FROM FLDMASTER a where a.deleted = 0 and a.status = 0 and length(a.OBJNAME) = 4"
            v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLTX, gc_ActionInquiry, v_strClause)
            Dim v_lngError As Long = Proxy.Message(v_strObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Exit Sub
            End If

            'If mv_lngParentid <> 0 Then
            '    Select Case mv_strTltxcd
            '        Case "4001"
            '            MyBase.CMDSQL = Replace(MyBase.CMDSQL, "TMP_TLLOG", "(select * from TMP_tllog where tltxcd in('4084','4002') and deleted=0 and brid='" & m_BusLayer.CurrentTellerProfile.BranchId & "' and txdate = to_date('" & mv_strTXDATE & "', 'dd/mm/yyyy'))")
            '        Case Else
            '            MyBase.CMDSQL = Replace(MyBase.CMDSQL, "TMP_TLLOG", "(select * from TMP_tllog where parentid = " & mv_lngParentid & ")")
            '    End Select
            '    MyBase.CMDSQL = Replace(MyBase.CMDSQL, "WHERE a.isparent<>0 and nvl(a.micode,'000') in (select micode from TMP_rgmi) and brid = '" & m_BusLayer.CurrentTellerProfile.BranchId & "'", "")
            'End If
            intMaxNo = m_BusLayer.CurrentTellerProfile.MaxValue
            strValues = ""
            strDetailText = ""
            If (Me.Tltxcd = "2201" Or Me.Tltxcd = "2202" Or Me.Tltxcd = "2203" Or Me.Tltxcd = "2204" _
                        Or Me.Tltxcd = "2205" Or Me.Tltxcd = "2206" Or Me.Tltxcd = "2207" Or Me.Tltxcd = "2208" _
                        Or Me.Tltxcd = "2209" Or Me.Tltxcd = "2210" Or Me.Tltxcd = "2211" Or Me.Tltxcd = "2212" Or Me.Tltxcd = "2213") Then
                For intIndex = 1 To intMaxNo
                    'strValues = strValues & vbCrLf & "LEAD(C.CAPTION," & CStr(intIndex - 1) & ",0) OVER (PARTITION BY B.TXNUM,B.TXDATE ORDER BY FLDCD) VALUE" & Format(intIndex, "00") & "_TEXT, LEAD(B.TXDESC," & CStr(intIndex - 1) & ",0) OVER (PARTITION BY B.TXNUM,B.TXDATE ORDER BY FLDCD) VALUE" & Format(intIndex, "00") & ","
                    'strDetailText = strDetailText & vbCrLf & " '#" & Format(intIndex, "00") & "' || nvl(col_desc" & Format(intIndex, "00") & ",'NULL') || 'vbCrLf' ||"
                    If intIndex = 1 Then
                        strDetailText = strDetailText & vbCrLf & " to_clob('#" & Format(intIndex, "00") & "') || nvl(col_desc" & Format(intIndex, "00") & ",nvl(col_value" & Format(intIndex, "00") & ",'NULL')) || 'vbCrLf' ||"
                    Else
                        strDetailText = strDetailText & vbCrLf & " '#" & Format(intIndex, "00") & "' || nvl(col_desc" & Format(intIndex, "00") & ",nvl(col_value" & Format(intIndex, "00") & ",'NULL')) || 'vbCrLf' ||"
                    End If
                Next
            Else
                For intIndex = 1 To intMaxNo
                    'If intIndex = 1 Then
                    'strDetailText = strDetailText & vbCrLf & " to_clob('#" & Format(intIndex, "00") & "') || nvl(col_desc" & Format(intIndex, "00") & ",nvl(col_value" & Format(intIndex, "00") & ",'NULL')) || 'vbCrLf' ||"
                    'Else
                    strDetailText = strDetailText & vbCrLf & " '#" & Format(intIndex, "00") & "' || nvl(col_desc" & Format(intIndex, "00") & ",nvl(col_value" & Format(intIndex, "00") & ",'NULL')) || 'vbCrLf' ||"
                    'End If
                Next
            End If
            strDetailText = strDetailText & " nvl(txdesc,'')"
            'For intIndex = 1 To 3
            '    MyBase.CMDSQL = Replace(MyBase.CMDSQL, "?tltxcd" & intIndex, " a.tltxcd in " _
            '                    & "( " _
            '                    & " select distinct c.tltxcd from tlauth c where c.status =0 and c.deleted =0 and " _
            '                    & " (" _
            '                    & "    ( c.authtype = 'G' and c.authid in ( select grpid from tlgrpusers d where d.tlid = '" & m_BusLayer.CurrentTellerProfile.TellerId & "' and d.status =0 and d.deleted =0 ) and c.tltype = '" & intIndex & "' and decode(c.tllimit,0,a.msgamt,c.tllimit)  >= a.msgamt ) " _
            '                    & " OR ( c.authtype = 'U' and c.authid = '" & m_BusLayer.CurrentTellerProfile.TellerId & "' and c.tltype = '" & intIndex & "' and decode(c.tllimit,0,a.msgamt,c.tllimit) >= a.msgamt )" _
            '                    & " )" _
            '                    & ")")
            'Next
            'strValues = strValues.Substring(1, strValues.Length - 2) & vbCrLf
            strDetailText = strDetailText & " DETAIL_TEXT " & vbCrLf

            'MyBase.CMDSQL = Replace(MyBase.CMDSQL, "?VALUES", strValues)
            MyBase.CMDSQL = Replace(MyBase.CMDSQL, "?DETAIL_TEXT", strDetailText)
            MyBase.CMDSQL = Replace(MyBase.CMDSQL, "?BUSDATE", m_BusLayer.CurrentTellerProfile.BusDate)
            AddHandler MyBase.SearchGrid.SelectedRowsChanged, AddressOf SearchGrid_SelectedRowsChanged
            'AddHandler MyBase.SearchGrid.Click, AddressOf SearchGrid_SelectedRowsChanged1
            Me.Text = mv_strCaption

            btnAddNew.Text = mv_ResourceManager.GetString("btnAddNew_TLLOG")
            btnView.Text = mv_ResourceManager.GetString("btnView_TLLOG")
            btnEdit.Text = mv_ResourceManager.GetString("btnEdit_TLLOG")
            btnDelete.Text = mv_ResourceManager.GetString("btnDelete_TLLOG")
            btnExecute.Text = mv_ResourceManager.GetString("btnExecute_TLLOG")
            'ssbStatus.Text = mv_ResourceManager.GetString("ssbStatus_" & mv_strTableName)           
            If ModuleCode = "SY" Then
                MyBase.EnableBtnSearch()
                'MyBase.OnSearch(MyBase.IsLocalSearch, MyBase.ModuleCode & "." & MyBase.ObjectName)
            End If
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                         & "Error code: System error!" & vbNewLine _
                                         & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try

    End Sub

    Overrides Sub Grid_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If MyBase.btnView.Enabled Or MyBase.btnDelete.Enabled Then
            MyBase.Grid_Click(sender, e)
        End If
    End Sub
    Overrides Sub Grid_DblClick(ByVal sender As Object, ByVal e As System.EventArgs)
        'OnViewTran()
    End Sub

    Sub SearchGrid_SelectedRowsChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim strDetailText, v_strTltxcd, v_strCaption As String
            Dim intMaxNo, intIndex As Integer
            If Not CType(sender, GridEx).CurrentRow Is Nothing Then
                If CType(sender, GridEx).CurrentRow.GetType.ToString = "Xceed.Grid.DataRow" Then
                    If Not CType(CType(sender, GridEx).CurrentRow, Xceed.Grid.DataRow).Cells("AUTH_STRING").Value Is Nothing Then
                        MyBase.ButtonEnable(CType(CType(sender, GridEx).CurrentRow, Xceed.Grid.DataRow).Cells("AUTH_STRING").Value)

                        v_strTltxcd = CType(CType(sender, GridEx).CurrentRow, Xceed.Grid.DataRow).Cells("TLTXCD").Value
                        strDetailText = CType(CType(sender, GridEx).CurrentRow, Xceed.Grid.DataRow).Cells("DETAIL_TEXT").Value

                        intMaxNo = m_BusLayer.CurrentTellerProfile.MaxValue
                        For intIndex = 1 To intMaxNo
                            If InStr(strDetailText, "#" & Format(intIndex, "00") & "NULL" & "vbCrLf") > 0 Then
                                strDetailText = Replace(strDetailText, "#" & Format(intIndex, "00") & "NULL" & "vbCrLf", "")
                            Else
                                strDetailText = Replace(strDetailText, "#" & Format(intIndex, "00"), getDisplay(v_strTltxcd & Format(intIndex, "00")))
                            End If
                        Next
                        strDetailText = Replace(strDetailText, "NULL", "")
                        strDetailText = Replace(strDetailText, "vbCrLf", vbCrLf)
                        strDetailText = Replace(strDetailText, "VBCRLF", vbCrLf)
                        If Not strDetailText Is Nothing Then
                            mv_frmTask.txtTaskContent.Text = strDetailText.Trim
                        Else
                            mv_frmTask.txtTaskContent.Text = ""
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                         & "Error code: System error!" & vbNewLine _
                                         & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try

    End Sub

    Private Function CheckTranLimit(ByVal strTltxcd As String, ByVal dblTranLimit As Double, ByVal v_strStatus As String) As Boolean
        Dim v_lngErrCode As Long = ERR_SYSTEM_OK
        Dim v_strErrorSource As String = "Sats.App.frmTranMain.CheckTranLimit"
        Dim v_strSQLCMD, v_strObjMsg, v_strValue, v_strFLDNAME As String
        Dim v_xmlObjDocument As New Xml.XmlDocument
        Dim v_nodeList As Xml.XmlNodeList

        Try
            Dim v_dblTranLimit As Double = 0
            v_strSQLCMD = "SELECT max(b.tllimit) tllimit, max(a.msg_amt) msg_amt FROM TLTX a, " _
                            & " ( " _
                            & " select c.tltxcd, c.tllimit from tlauth c where c.status =0 and c.deleted =0 and c.authtype = 'U' and c.authid = '" & m_BusLayer.CurrentTellerProfile.TellerId & "' and c.tltype = '" & v_strStatus & "' " _
                            & " union" _
                            & " select c.tltxcd, c.tllimit from tlauth c where c.status =0 and c.deleted =0 and c.authtype = 'G'" _
                            & " and c.authid in ( select grpid from tlgrpusers d where d.tlid = '" & m_BusLayer.CurrentTellerProfile.TellerId & "' and d.status =0 and d.deleted =0 ) and c.tltype = '" & v_strStatus & "' " _
                            & " ) b, appmodules c " _
                            & " where a.status =0 and a.deleted =0 and a.tltxcd = b.tltxcd and substr(a.tltxcd,1,2) = c.txcode and c.deleted=0 and c.status = 0" _
                            & " and a.tltxcd = '" & strTltxcd & "'"

            v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_LOOKUP, gc_ActionInquiry, v_strSQLCMD)
            Dim v_lngError As Long = Proxy.Message(v_strObjMsg)

            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Return False
            End If

            v_xmlObjDocument.LoadXml(v_strObjMsg)
            v_nodeList = v_xmlObjDocument.SelectNodes("/ObjectMessage/ObjData")
            'v_xmlDocument.SelectSingleNode("TransactMessage/fields/entry[@fldname='" & mv_strFileName & "']").InnerText = v_strXML
            If v_nodeList.Count > 0 Then
                For i = 0 To v_nodeList.Count - 1
                    For j As Integer = 0 To v_nodeList.Item(i).ChildNodes.Count - 1
                        With v_nodeList.Item(i).ChildNodes(j)
                            v_strValue = Trim(.InnerText.ToString)
                            v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                            Select Case Trim(v_strFLDNAME)
                                Case "TLLIMIT"
                                    v_dblTranLimit = CDbl(v_strValue)
                            End Select
                        End With
                    Next
                Next
            Else
                MsgBox("Bạn không còn quyền thực hiện giao dịch này", MsgBoxStyle.Information, gc_ApplicationTitle)
                Return False
            End If
            If v_dblTranLimit <> 0 Then
                If v_dblTranLimit < dblTranLimit Then
                    MsgBox("Bạn chỉ có thể thực hiện giao dịch với số lượng nhỏ hơn hoặc bằng " & v_dblTranLimit, MsgBoxStyle.Information, gc_ApplicationTitle)
                    Return False
                End If
            End If
            'ContextUtil.SetComplete()
            Return True
        Catch ex As Exception
            'ContextUtil.SetAbort()
            Throw ex
        End Try

    End Function

    Sub SendBDS1(ByVal intType As Integer)
        Dim v_lngError As Long = ERR_SYSTEM_OK
        Dim v_strTxMsg, v_strInitialTxMsg As String
        Dim v_strTLTXCD, v_strMSGAMT, v_strSICODE, v_strCOMICODE, v_strMICODE, v_strStatus, v_strOldStatus, v_strAUTOID, v_strTXDATE, v_strTXNUM, v_strBUSDATE As String
        Dim v_strPARENTID, v_strBRCODE, v_strTXNAME, v_strSTATUS_TEXT, v_strPARENT_TEXT As String
        Dim v_strTLID, v_strOFFID, v_strCHKID, v_strCFRID, v_strISPARENT As String
        Dim v_strTLNAME, v_strCHKNAME, v_strOFFNAME, v_strCFRNAME, v_strCHILDTLTXCD, v_strISBRID, v_strReason As String
        Dim v_strClause, v_strObjMsg As String
        Dim v_xmlDocument As New Xml.XmlDocument
        Dim v_intMutilTran As Integer = 0
        'bangpv
        Dim v_rowcount As Long
        Dim i As Integer
        v_oProccess = New Sats.AppCore.ProcessForm(Me)
        Try
            'v_thread = New Threading.Thread(AddressOf ShowProcessForm)
            v_strReason = ""
            If intType = 2 Then
                v_strReason = InputBox("Bạn vui lòng nhập lý do từ chối duyệt: ", "Nhập lý do từ chối duyệt", "")
                v_strReason = "Lý do từ chối duyệt là: " & v_strReason
            End If
            v_oProccess.StartProcessForm()
            ' 1. Create messages
            v_strTxMsg = ""
            With CType(MyBase.SearchGrid, GridEx)
                For i = 0 To .DataRows.Count - 1
                    With .DataRows(i)
                        If .Cells("TXBLN").Value Then
                            v_intMutilTran = v_intMutilTran + 1
                            ' kiem tra xem nguoi dung co duoc thuc hien giao dich nay, kiem tra thong tin han muc giao dich
                            v_strTLTXCD = .Cells("TLTXCD").Value
                            v_strMSGAMT = .Cells("MSGAMT").Value
                            v_strStatus = .Cells("STATUS").Value
                            v_strBUSDATE = .Cells("BUSDATE").Value
                            v_strPARENTID = .Cells("PARENTID").Value
                            v_strCHILDTLTXCD = .Cells("CHILDTLTXCD").Value
                            v_strBRCODE = .Cells("BRANCH_NAME").Value
                            v_strTXNAME = .Cells("TRAN_NAME").Value

                            v_strPARENT_TEXT = .Cells("ISPARENT_TEXT").Value
                            v_strTLID = .Cells("TLID").Value
                            v_strTLNAME = .Cells("MEMBER_STAFF").Value
                            v_strCHKID = .Cells("CHKID").Value
                            v_strCHKNAME = .Cells("MEMBER_LEADER").Value
                            v_strOFFID = .Cells("OFFID").Value
                            v_strOFFNAME = .Cells("VSD_STAFF").Value
                            v_strCFRID = .Cells("CFRID").Value
                            v_strCFRNAME = .Cells("VSD_LEADER").Value
                            v_strISPARENT = .Cells("ISPARENT").Value
                            v_strAUTOID = .Cells("AUTOID").Value
                            v_strTXDATE = .Cells("TXDATE").Value
                            v_strTXNUM = .Cells("TXNUM").Value

                            v_strSICODE = .Cells("SICODE").Value
                            v_strMICODE = .Cells("MICODE").Value
                            v_strCOMICODE = .Cells("COMICODE").Value
                            v_strISBRID = .Cells("ISBRID").Value
                            v_strOldStatus = v_strStatus
                            Select Case v_strStatus
                                Case "0"
                                    v_strCHKID = Me.TellerId
                                    v_strCHKNAME = m_BusLayer.CurrentTellerProfile.TellerName
                                    v_strSTATUS_TEXT = APPROVED_MEMBER_MANAGER_TEXT
                                    If v_strOFFID = "0" And v_strCFRID = "0" Then
                                        v_strBUSDATE = Me.BusDate
                                    End If
                                Case "1"
                                    v_strOFFID = Me.TellerId
                                    v_strOFFNAME = m_BusLayer.CurrentTellerProfile.TellerName
                                    v_strSTATUS_TEXT = APPROVED_VDS_STAFF_TEXT
                                    If v_strCFRID = "0" Then
                                        v_strBUSDATE = Me.BusDate
                                    End If
                                Case "2"
                                    v_strCFRID = Me.TellerId
                                    v_strCFRNAME = m_BusLayer.CurrentTellerProfile.TellerName
                                    v_strBUSDATE = Me.BusDate
                                    v_strSTATUS_TEXT = CONFIRMED_VSD_MANAGER_TEXT
                            End Select
                            If intType = 2 Then
                                v_strStatus = "4"
                                v_strSTATUS_TEXT = REJECTED_LOG_TEXT
                            ElseIf intType = 3 Then
                                v_strStatus = "4"
                                v_strSTATUS_TEXT = DELETED_TRANS_TEXT
                            Else
                                v_strStatus = CStr(CLng(v_strStatus) + 1)
                            End If


                            v_strClause = "select MSG_AMT from tltx where tltxcd = '" & IIf(v_strCHILDTLTXCD <> "", v_strCHILDTLTXCD, v_strTLTXCD) & "'"
                            v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLTX, gc_ActionInquiry, v_strClause)
                            v_lngError = Proxy.Message(v_strObjMsg)

                            If v_lngError <> ERR_SYSTEM_OK Then
                                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                                Exit Sub
                            End If

                            v_xmlDocument.LoadXml(v_strObjMsg)
                            If Not IsDBNull(v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='MSG_AMT']")) Then
                                v_strMSGAMT = v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='MSG_AMT']").InnerText
                            End If

                            'Tạo điện giao dịch
                            v_strTxMsg &= BuildXMLTxMsg(gc_MsgTypeTrans, gc_IsLocalMsg, IIf(intType = 3, "9002", IIf(v_strCHILDTLTXCD <> "", v_strCHILDTLTXCD, v_strTLTXCD)), _
                                        Me.BranchId, v_strTLID, Me.IpAddress, Me.WsName, _
                                        v_strStatus, IIf(InStr(v_strTLTXCD, "44") > 0, "Y", "N") _
                                        , v_strOFFID, v_strCHKID, v_strCFRID, v_strTXDATE, , v_strTXNUM, , v_strReason, v_strMSGAMT, v_strMICODE, , , Me.UserLanguage, v_strBUSDATE, v_strSICODE, _
                                        , v_strISPARENT, v_strPARENTID, v_strBRCODE, v_strTLNAME, v_strOFFNAME, v_strCHKNAME, _
                                        v_strCFRNAME, v_strTXNAME, v_strSTATUS_TEXT, v_strPARENT_TEXT, v_strAUTOID, gc_MsgTypeTrans, v_strISBRID, v_strCOMICODE, v_strReason, , , v_strOldStatus) & vbCrLf

                        End If
                    End With
                Next
            End With
            If v_intMutilTran >= 1 Then
                If v_intMutilTran >= 2 Then
                    v_strTxMsg = "<ROOT MSGTYPE=""T"" LOCAL=""Y"" LANGUAGE=""VN"" TXTYPE=""T"" BRID=""" & Me.BranchId & """ TLID=""" & TellerId & """>" & v_strTxMsg & "</ROOT>"
                End If

                ' 2. Accept transaction
                v_strInitialTxMsg = v_strTxMsg
                While True
                    v_lngError = Proxy.Message(v_strTxMsg)
                    If v_lngError <> ERR_SYSTEM_OK Then
                        Dim v_nodeList As Xml.XmlNodeList
                        Dim v_strTXNUM1, v_strTXDATE1 As String

                        'ThÃ´ng bÃ¡o lá»—i

                        v_xmlDocument.LoadXml(v_strTxMsg)
                        If InStr(v_xmlDocument.InnerXml, "ROOT") > 0 Then
                            v_nodeList = v_xmlDocument.SelectNodes("/ROOT/FAILED_MESSAGE")
                        Else
                            v_nodeList = v_xmlDocument.SelectNodes("/TransactMessage/FAILED_MESSAGE")
                        End If

                        Dim v_strValue, v_strFLDNAME, v_strCount As String
                        Dim v_strErrMsg, v_strWarningMsg As String
                        Dim v_intErr, v_intWarning As Integer

                        v_strErrMsg = ""
                        v_strWarningMsg = ""
                        v_intErr = 0
                        v_intWarning = 0

                        For i = 0 To v_nodeList.Count - 1
                            v_strTXNUM1 = CStr(CType(v_nodeList.Item(i).Attributes.GetNamedItem(gc_AtributeTXNUM), Xml.XmlAttribute).Value)
                            v_strTXDATE1 = CStr(CType(v_nodeList.Item(i).Attributes.GetNamedItem(gc_AtributeTXDATE), Xml.XmlAttribute).Value)
                            For j = 0 To v_nodeList.Item(i).ChildNodes.Count - 1
                                With v_nodeList.Item(i).ChildNodes(j)
                                    v_strValue = .InnerText.ToString
                                    v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem(gc_AtributeFLDNAME), Xml.XmlAttribute).Value)
                                    v_strCount = CStr(CType(.Attributes.GetNamedItem("COUNT"), Xml.XmlAttribute).Value)
                                    If v_strValue.Trim <> "" Then
                                        If v_strFLDNAME = "ERROR_MESSAGE" Then
                                            v_strErrMsg &= i + 1 & ". Giao dịch (" & v_strTXNUM1 & ", " & v_strTXDATE1 & ") có " & v_strCount & " lỗi :" & vbCrLf & .InnerText.ToString
                                            v_intErr += CInt(v_strCount)
                                        ElseIf v_strFLDNAME = "WARNING_MESSAGE" Then
                                            v_strWarningMsg &= i + 1 & ". Giao dịch (" & v_strTXNUM1 & ", " & v_strTXDATE1 & ") có " & v_strCount & " cảnh báo :" & vbCrLf & .InnerText.ToString
                                            v_intWarning += CInt(v_strCount)
                                        End If
                                    End If
                                End With
                            Next
                        Next

                        If v_intWarning + v_intErr > 0 Then
                            Dim frm As New frmErrMsg
                            frm.ErrMsg = v_strErrMsg
                            frm.ErrCount = v_intErr
                            frm.WarningMsg = v_strWarningMsg
                            frm.WarningCount = v_intWarning

                            v_oProccess.StopProcessForm()
                            Application.DoEvents()
                            If frm.ShowDialog() = Windows.Forms.DialogResult.Cancel Then
                                MyBase.btnView.Enabled = True
                                MyBase.btnEdit.Enabled = True
                                Exit Sub
                            Else
                                v_strTxMsg = v_strInitialTxMsg.Replace(gc_AtributeMISSING_WARNING & "=""0""", gc_AtributeMISSING_WARNING & "=""1""")
                                v_oProccess.StartProcessForm()
                            End If
                        Else
                            Exit While
                        End If
                    Else
                        Exit While
                    End If
                End While
                ' 3. Update grid

                With CType(MyBase.SearchGrid, GridEx)
                    If intType = 3 Then
                        CType(MyBase.SearchGrid, GridEx).BeginInit()
                    End If
                    v_rowcount = .DataRows.Count
                    i = 0
                    While i < v_rowcount

                        'For i = 0 To v_rowcount - 1 '.DataRows.Count - 1 (removed by bangpv)
                        With .DataRows(i)
                            If .Cells("TXBLN").Value And getSucceedTran(v_strTxMsg, .Cells("AUTOID").Value) Then
                                If intType = 3 Then
                                    CType(MyBase.SearchGrid, GridEx).DataRows.RemoveAt(i)
                                    v_rowcount = v_rowcount - 1
                                    i = i - 1
                                Else
                                    v_strTLTXCD = .Cells("TLTXCD").Value
                                    v_strMSGAMT = .Cells("MSGAMT").Value
                                    v_strStatus = .Cells("STATUS").Value
                                    v_strBUSDATE = .Cells("BUSDATE").Value
                                    v_strPARENTID = .Cells("PARENTID").Value
                                    v_strCHILDTLTXCD = .Cells("CHILDTLTXCD").Value
                                    v_strBRCODE = .Cells("BRANCH_NAME").Value
                                    v_strTXNAME = .Cells("TRAN_NAME").Value

                                    v_strPARENT_TEXT = .Cells("ISPARENT_TEXT").Value
                                    v_strTLID = .Cells("TLID").Value
                                    v_strTLNAME = .Cells("MEMBER_STAFF").Value
                                    v_strCHKID = .Cells("CHKID").Value
                                    v_strCHKNAME = .Cells("MEMBER_LEADER").Value
                                    v_strOFFID = .Cells("OFFID").Value
                                    v_strOFFNAME = .Cells("VSD_STAFF").Value
                                    v_strCFRID = .Cells("CFRID").Value
                                    v_strCFRNAME = .Cells("VSD_LEADER").Value
                                    v_strISPARENT = .Cells("ISPARENT").Value
                                    v_strAUTOID = .Cells("AUTOID").Value
                                    v_strTXDATE = .Cells("TXDATE").Value
                                    v_strTXNUM = .Cells("TXNUM").Value

                                    v_strSICODE = .Cells("SICODE").Value
                                    v_strMICODE = .Cells("MICODE").Value
                                    v_strCOMICODE = .Cells("COMICODE").Value
                                    v_strISBRID = .Cells("ISBRID").Value

                                    v_strOldStatus = v_strStatus
                                    Select Case v_strStatus
                                        Case "0"
                                            v_strCHKID = Me.TellerId
                                            v_strCHKNAME = m_BusLayer.CurrentTellerProfile.TellerName
                                            v_strSTATUS_TEXT = APPROVED_MEMBER_MANAGER_TEXT
                                            If v_strOFFID = "0" And v_strCFRID = "0" Then
                                                v_strBUSDATE = Me.BusDate
                                            End If
                                        Case "1"
                                            v_strOFFID = Me.TellerId
                                            v_strOFFNAME = m_BusLayer.CurrentTellerProfile.TellerName
                                            v_strSTATUS_TEXT = APPROVED_VDS_STAFF_TEXT
                                            If v_strCFRID = "0" Then
                                                v_strBUSDATE = Me.BusDate
                                            End If
                                        Case "2"
                                            v_strCFRID = Me.TellerId
                                            v_strCFRNAME = m_BusLayer.CurrentTellerProfile.TellerName
                                            v_strBUSDATE = Me.BusDate
                                            v_strSTATUS_TEXT = CONFIRMED_VSD_MANAGER_TEXT
                                    End Select
                                    If intType = 2 Then
                                        v_strStatus = "4"
                                        v_strSTATUS_TEXT = REJECTED_LOG_TEXT
                                    Else
                                        v_strStatus = CStr(CLng(v_strStatus) + 1)
                                    End If

                                    Dim intIcon As Integer
                                    ' update screen 
                                    .Cells("STATUS").Value = CDec(v_strStatus)
                                    .Cells("STATUS_TEXT").Value = v_strSTATUS_TEXT
                                    Select Case v_strOldStatus
                                        Case "0"
                                            .Cells("CHKID").Value = v_strCHKID
                                            .Cells("MEMBER_LEADER").Value = v_strCHKNAME
                                            If v_strOFFID = "0" And v_strCFRID = "0" Then
                                                .Cells("BUSDATE").Value = v_strBUSDATE
                                            End If
                                        Case "1"
                                            .Cells("OFFID").Value = v_strOFFID
                                            .Cells("VSD_STAFF").Value = v_strOFFNAME
                                            If v_strCFRID = "0" Then
                                                .Cells("BUSDATE").Value = v_strBUSDATE
                                            End If
                                        Case "2"
                                            .Cells("CFRID").Value = v_strCFRID
                                            .Cells("VSD_LEADER").Value = v_strCFRNAME
                                            .Cells("BUSDATE").Value = v_strBUSDATE
                                    End Select
                                    If v_strStatus = "4" Then
                                        .Cells("DETAIL_TEXT").Value = .Cells("DETAIL_TEXT").Value & "vbCrLf" & v_strReason
                                    End If
                                    .Cells("AUTH_STRING").Value = "YNNNY"
                                    MyBase.btnView.Enabled = False
                                    MyBase.btnEdit.Enabled = False
                                    intIcon = IIf(v_strStatus = "4", 3, IIf(v_strBUSDATE = Me.BusDate, 2, 1))
                                    .Cells("TXIMAGE").Value = Icon.FromHandle(CType(MyBase.imgTool1.Images(CInt(intIcon)), Bitmap).GetHicon())
                                    .Cells("TXBLN").Value = False
                                End If
                            End If
                        End With
                        i = i + 1
                        'Next
                    End While
                    If intType = 3 Then
                        CType(MyBase.SearchGrid, GridEx).EndInit()
                        _FormatGridBefore(CType(MyBase.SearchGrid, GridEx), TellerId, , , False, , , , )
                    End If
                End With
                'v_thread.Abort()
                v_oProccess.StopProcessForm()
                Application.DoEvents()
                'If InStr(v_strTxMsg, "ROOT") > 0 Then
                '    Dim v_strTranMessage As String = ""
                '    getSucceedTran(v_strTxMsg, "0", v_strTranMessage)
                '    Dim frm As New frmTranMessage
                '    frm.txtTranMessage.Text = v_strTranMessage
                '    frm.ShowDialog()
                'Else
                '    MessageBox.Show(IIf(Me.UserLanguage = "VN", "Giao dịch " & v_strTXNUM & ", " & v_strTXDATE & " thực hiện THÀNH CÔNG !", "This transaction is accepted !"))
                'End If
                ShowTranMessage(v_strTxMsg, v_lngError, intType)
                Exit Sub
            Else
                MyBase.btnView.Enabled = True
                MyBase.btnEdit.Enabled = True
            End If

        Catch ex As Exception
            v_oProccess.StopProcessForm()
            Application.DoEvents()
            MyBase.btnView.Enabled = True
            MyBase.btnEdit.Enabled = True
            Throw ex
        Finally
            v_oProccess.StopProcessForm()
            Application.DoEvents()
            v_oProccess = Nothing
            Me.Focus()
        End Try
    End Sub

    Sub SendBDS(ByVal intType As Integer)
        Dim v_lngError As Long = ERR_SYSTEM_OK
        Dim v_strTxMsg, v_strInitialTxMsg As String
        Dim v_strTLTXCD, v_strMSGAMT, v_strSICODE, v_strCOMICODE, v_strMICODE, v_strStatus, v_strOldStatus, v_strAUTOID, v_strTXDATE, v_strTXNUM, v_strBUSDATE As String
        Dim v_strPARENTID, v_strBRCODE, v_strTXNAME, v_strSTATUS_TEXT, v_strPARENT_TEXT As String
        Dim v_strTLID, v_strOFFID, v_strCHKID, v_strCFRID, v_strISPARENT As String
        Dim v_strTLNAME, v_strCHKNAME, v_strOFFNAME, v_strCFRNAME, v_strCHILDTLTXCD, v_strISBRID, v_strReason, v_strTxNote, v_strTblChk, v_strVsdBrid As String
        Dim v_strClause, v_strObjMsg As String
        Dim v_xmlDocument As New Xml.XmlDocument
        Dim v_strSignCA As String = ""
        Dim v_intMutilTran As Integer = 0
        'bangpv
        Dim v_rowcount As Long
        Dim i As Integer
        v_oProccess = New Sats.AppCore.ProcessForm(Me)
        Try
            'v_thread = New Threading.Thread(AddressOf ShowProcessForm)
            v_strReason = ""
            If intType = 2 Then
                v_strReason = InputBox("Bạn vui lòng nhập lý do từ chối duyệt: ", "Nhập lý do từ chối duyệt", "")
                v_strReason = "Lý do từ chối duyệt là: " & v_strReason
            End If
            v_oProccess.StartProcessForm()
            ' 1. Create messages
            v_strTxMsg = ""
            With CType(MyBase.SearchGrid, GridEx)
                For i = 0 To .DataRows.Count - 1
                    With .DataRows(i)
                        If .Cells("TXBLN").Value Then
                            v_intMutilTran = v_intMutilTran + 1
                            ' kiem tra xem nguoi dung co duoc thuc hien giao dich nay, kiem tra thong tin han muc giao dich
                            v_strTLTXCD = .Cells("TLTXCD").Value
                            v_strMSGAMT = .Cells("MSGAMT").Value
                            v_strStatus = .Cells("STATUS").Value
                            v_strBUSDATE = .Cells("BUSDATE").Value
                            v_strPARENTID = .Cells("PARENTID").Value
                            v_strCHILDTLTXCD = .Cells("CHILDTLTXCD").Value
                            v_strBRCODE = .Cells("BRANCH_NAME").Value
                            v_strTXNAME = .Cells("TRAN_NAME").Value

                            v_strPARENT_TEXT = .Cells("ISPARENT_TEXT").Value
                            v_strTLID = .Cells("TLID").Value
                            v_strTLNAME = .Cells("MEMBER_STAFF").Value
                            v_strCHKID = .Cells("CHKID").Value
                            v_strCHKNAME = .Cells("MEMBER_LEADER").Value
                            v_strOFFID = .Cells("OFFID").Value
                            v_strOFFNAME = .Cells("VSD_STAFF").Value
                            v_strCFRID = .Cells("CFRID").Value
                            v_strCFRNAME = .Cells("VSD_LEADER").Value
                            v_strISPARENT = .Cells("ISPARENT").Value
                            v_strAUTOID = .Cells("AUTOID").Value
                            v_strTXDATE = .Cells("TXDATE").Value
                            v_strTXNUM = .Cells("TXNUM").Value

                            v_strSICODE = .Cells("SICODE").Value
                            v_strMICODE = .Cells("MICODE").Value
                            v_strCOMICODE = .Cells("COMICODE").Value
                            v_strISBRID = .Cells("ISBRID").Value
                            v_strTxNote = .Cells("TXNOTE").Value
                            v_strOldStatus = v_strStatus
                            v_strVsdBrid = .Cells("VSD_BRID").Value
                            'bangpv: Ký số
                            v_strSignCA = IIf(.Cells("CA_STATUS").Value = "Không", "0", "1")
                            'end bangpv 
                            Select Case v_strStatus
                                Case "0"
                                    v_strCHKID = Me.TellerId
                                    v_strCHKNAME = m_BusLayer.CurrentTellerProfile.TellerName
                                    v_strSTATUS_TEXT = APPROVED_MEMBER_MANAGER_TEXT
                                    If v_strOFFID = "0" And v_strCFRID = "0" Then
                                        v_strBUSDATE = Me.BusDate
                                    End If
                                Case "1"
                                    v_strOFFID = Me.TellerId
                                    v_strOFFNAME = m_BusLayer.CurrentTellerProfile.TellerName
                                    v_strSTATUS_TEXT = APPROVED_VDS_STAFF_TEXT
                                    If v_strCFRID = "0" Then
                                        v_strBUSDATE = Me.BusDate
                                    End If
                                Case "2"
                                    v_strCFRID = Me.TellerId
                                    v_strCFRNAME = m_BusLayer.CurrentTellerProfile.TellerName
                                    v_strBUSDATE = Me.BusDate
                                    v_strSTATUS_TEXT = CONFIRMED_VSD_MANAGER_TEXT
                            End Select
                            If intType = 2 Then
                                v_strStatus = "4"
                                v_strSTATUS_TEXT = REJECTED_LOG_TEXT
                            ElseIf intType = 3 Then
                                v_strStatus = "4"
                                v_strSTATUS_TEXT = DELETED_TRANS_TEXT
                            Else
                                v_strStatus = CStr(CLng(v_strStatus) + 1)
                            End If


                            v_strClause = "select MSG_AMT, TBLCHK from tltx where tltxcd = '" & IIf(v_strCHILDTLTXCD <> "", v_strCHILDTLTXCD, v_strTLTXCD) & "'"
                            v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLTX, gc_ActionInquiry, v_strClause)
                            v_lngError = Proxy.Message(v_strObjMsg)

                            If v_lngError <> ERR_SYSTEM_OK Then
                                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                                Exit Sub
                            End If

                            v_xmlDocument.LoadXml(v_strObjMsg)
                            If Not IsDBNull(v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='MSG_AMT']")) Then
                                v_strMSGAMT = v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='MSG_AMT']").InnerText
                            End If

                            'Hanm5 them truong kiem tra check nhung bang du lieu nao
                            If Not IsDBNull(v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='TBLCHK']")) Then
                                v_strTblChk = v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='TBLCHK']").InnerText
                            End If

                            'Tạo điện giao dịch
                            'BangPV: tao dien giao dich voi giao dich ky so va ko ky so
                            If v_strSignCA = "1" And intType <> 3 Then
                                v_strTxMsg &= BuildXMLTxMsg(gc_MsgTypeTrans, gc_IsLocalMsg, IIf(intType = 3, "9002", IIf(v_strCHILDTLTXCD <> "", v_strCHILDTLTXCD, v_strTLTXCD)), _
                                        Me.BranchId, v_strTLID, Me.IpAddress, Me.WsName, _
                                        v_strStatus, IIf(InStr(v_strTLTXCD, "44") > 0, "Y", "N") _
                                        , v_strOFFID, v_strCHKID, v_strCFRID, v_strTXDATE, , v_strTXNUM, , v_strReason, v_strMSGAMT, v_strMICODE, , , Me.UserLanguage, v_strBUSDATE, v_strSICODE, _
                                        v_strTLTXCD, v_strISPARENT, v_strPARENTID, v_strBRCODE, v_strTLNAME, v_strOFFNAME, v_strCHKNAME, _
                                        v_strCFRNAME, v_strTXNAME, v_strSTATUS_TEXT, v_strPARENT_TEXT, v_strAUTOID, gc_MsgTypeTrans, v_strISBRID, v_strCOMICODE, v_strReason, , , v_strOldStatus, v_strTxNote, _
                                        v_strVsdBrid, v_strTblChk, v_strTLTXCD, , , , v_strSignCA) & vbCrLf
                            Else
                                v_strTxMsg &= BuildXMLTxMsg(gc_MsgTypeTrans, gc_IsLocalMsg, IIf(intType = 3, "9002", IIf(v_strCHILDTLTXCD <> "", v_strCHILDTLTXCD, v_strTLTXCD)), _
                                        Me.BranchId, v_strTLID, Me.IpAddress, Me.WsName, _
                                        v_strStatus, IIf(InStr(v_strTLTXCD, "44") > 0, "Y", "N") _
                                        , v_strOFFID, v_strCHKID, v_strCFRID, v_strTXDATE, , v_strTXNUM, , v_strReason, v_strMSGAMT, v_strMICODE, , , Me.UserLanguage, v_strBUSDATE, v_strSICODE, _
                                        v_strTLTXCD, v_strISPARENT, v_strPARENTID, v_strBRCODE, v_strTLNAME, v_strOFFNAME, v_strCHKNAME, _
                                        v_strCFRNAME, v_strTXNAME, v_strSTATUS_TEXT, v_strPARENT_TEXT, v_strAUTOID, gc_MsgTypeTrans, v_strISBRID, v_strCOMICODE, v_strReason, , , v_strOldStatus, v_strTxNote, _
                                        v_strVsdBrid, v_strTblChk, v_strTLTXCD) & vbCrLf
                            End If
                            

                        End If
                    End With
                Next
            End With
            If v_intMutilTran >= 1 Then
                If v_intMutilTran >= 2 Then
                    v_strTxMsg = "<ROOT MSGTYPE=""T"" LOCAL=""Y"" LANGUAGE=""VN"" TXTYPE=""T"" BRID=""" & Me.BranchId & """ BUSDATE=""" & _
                    v_strBUSDATE & """ TLID=""" & TellerId & """ STATUS=""" & v_strStatus & """ CFRNAME=""" & v_strCFRNAME & _
                    """ OFFNAME=""" & v_strOFFNAME & """ TLNAME= """ & v_strTLNAME _
                    & """>" & v_strTxMsg & "</ROOT>"
                End If

                ' 2. Accept transaction
                Client.isActive = True
                Client.Action = "Begin " & v_strSTATUS_TEXT
                Proxy.SendAction(Client)
                'BangPV: Duyet ky so
                If v_strSignCA = "1" And intType <> 3 Then
                    Proxy.MessageCA(v_strTxMsg)
                Else
                    Proxy.Message(v_strTxMsg)
                End If

                Client.Action = "End " & v_strSTATUS_TEXT
                Client.isActive = False
                Proxy.SendAction(Client)

                Dim v_nodeList As Xml.XmlNodeList
                v_xmlDocument.LoadXml(v_strTxMsg)

                'Hanm5: them phan check xem da bat dau ngay chua
                If InStr(v_xmlDocument.InnerXml, "ROOT") > 0 Then
                    v_nodeList = v_xmlDocument.SelectNodes("/ROOT/TRAN_MESSAGE")
                Else
                    v_nodeList = v_xmlDocument.SelectNodes("/TransactMessage/FAILED_MESSAGE")
                End If

                If v_nodeList.Count >= 1 Then
                    Dim v_strFldNameErr, v_strValueErr, v_strDayNotStartErr As String
                    For j As Integer = 0 To v_nodeList.Item(0).ChildNodes.Count - 1
                        With v_nodeList.Item(0).ChildNodes(j)
                            v_strFldNameErr = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                            v_strValueErr = .InnerText.ToString
                            If Trim(v_strFldNameErr) = "DAY_NOT_START" Then
                                v_strDayNotStartErr = Trim(v_strValueErr)
                            ElseIf Trim(v_strFldNameErr) = "DAY_INVALID" Then
                                v_strDayNotStartErr = Trim(v_strValueErr)
                            End If
                        End With
                    Next
                    If Not v_strDayNotStartErr Is Nothing Then
                        If Len(v_strDayNotStartErr.Trim) > 0 Then
                            If v_strFldNameErr = "DAY_NOT_START" Then
                                v_oProccess.StopProcessForm()
                                Application.DoEvents()
                                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_DAY_NOT_STARTED_VN, gc_ERR_DAY_NOT_STARTED_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                                Me.Focus()
                                Exit Sub
                            Else
                                v_oProccess.StopProcessForm()
                                Application.DoEvents()
                                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_DAY_INVALID_VN, gc_ERR_DAY_INVALID_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                                Me.Focus()
                                Exit Sub
                            End If
                        End If
                    End If
                End If


                'Hanm5 ket thuc sua


                If InStr(v_xmlDocument.InnerXml, "ROOT") > 0 Then
                    v_nodeList = v_xmlDocument.SelectNodes("/ROOT/TRAN_MESSAGE")
                Else
                    v_nodeList = v_xmlDocument.SelectNodes("/TransactMessage/TRAN_MESSAGE")
                End If

                Dim v_strFLDNAME, v_strValue, v_strSUSS_Msg As String
                If Not v_nodeList Is Nothing Then
                    For j As Integer = 0 To v_nodeList.Item(0).ChildNodes.Count - 1
                        With v_nodeList.Item(0).ChildNodes(j)
                            v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                            v_strValue = .InnerText.ToString
                            If Trim(v_strFLDNAME) = "SUCC_MSG" Then
                                v_strSUSS_Msg = Trim(v_strValue)
                            End If
                        End With
                    Next
                End If
                If intType = 3 Then
                    CType(MyBase.SearchGrid, GridEx).BeginInit()
                End If

                Dim v_oRow As Xceed.Grid.DataRow
                If v_strSUSS_Msg <> "" Then
                    For Each v_oRow In SearchGrid.DataRows
                        For i = 0 To v_strSUSS_Msg.Split("#").Count - 2
                            If v_oRow.Cells("TXNUM").GetTextToPaint.ToUpper = v_strSUSS_Msg.Split("#")(i).Split("|")(2) _
                                And v_oRow.Cells("TXDATE").GetTextToPaint = v_strSUSS_Msg.Split("#")(i).Split("|")(3) Then

                                With v_oRow
                                    If intType = 3 Then
                                        v_oRow.Visible = False
                                    Else
                                        v_strTLTXCD = .Cells("TLTXCD").Value
                                        v_strMSGAMT = .Cells("MSGAMT").Value
                                        v_strStatus = .Cells("STATUS").Value
                                        v_strBUSDATE = .Cells("BUSDATE").Value
                                        v_strPARENTID = .Cells("PARENTID").Value
                                        v_strCHILDTLTXCD = .Cells("CHILDTLTXCD").Value
                                        v_strBRCODE = .Cells("BRANCH_NAME").Value
                                        v_strTXNAME = .Cells("TRAN_NAME").Value

                                        v_strPARENT_TEXT = .Cells("ISPARENT_TEXT").Value
                                        v_strTLID = .Cells("TLID").Value
                                        v_strTLNAME = .Cells("MEMBER_STAFF").Value
                                        v_strCHKID = .Cells("CHKID").Value
                                        v_strCHKNAME = .Cells("MEMBER_LEADER").Value
                                        v_strOFFID = .Cells("OFFID").Value
                                        v_strOFFNAME = .Cells("VSD_STAFF").Value
                                        v_strCFRID = .Cells("CFRID").Value
                                        v_strCFRNAME = .Cells("VSD_LEADER").Value
                                        v_strISPARENT = .Cells("ISPARENT").Value
                                        v_strAUTOID = .Cells("AUTOID").Value
                                        v_strTXDATE = .Cells("TXDATE").Value
                                        v_strTXNUM = .Cells("TXNUM").Value

                                        v_strSICODE = .Cells("SICODE").Value
                                        v_strMICODE = .Cells("MICODE").Value
                                        v_strCOMICODE = .Cells("COMICODE").Value
                                        v_strISBRID = .Cells("ISBRID").Value
                                        v_strVsdBrid = .Cells("VSD_BRID").Value

                                        v_strOldStatus = v_strStatus
                                        Select Case v_strStatus
                                            Case "0"
                                                v_strCHKID = Me.TellerId
                                                v_strCHKNAME = m_BusLayer.CurrentTellerProfile.TellerName
                                                v_strSTATUS_TEXT = APPROVED_MEMBER_MANAGER_TEXT
                                                If v_strOFFID = "0" And v_strCFRID = "0" Then
                                                    v_strBUSDATE = Me.BusDate
                                                End If
                                            Case "1"
                                                v_strOFFID = Me.TellerId
                                                v_strOFFNAME = m_BusLayer.CurrentTellerProfile.TellerName
                                                v_strSTATUS_TEXT = APPROVED_VDS_STAFF_TEXT
                                                If v_strCFRID = "0" Then
                                                    v_strBUSDATE = Me.BusDate
                                                End If
                                            Case "2"
                                                v_strCFRID = Me.TellerId
                                                v_strCFRNAME = m_BusLayer.CurrentTellerProfile.TellerName
                                                v_strBUSDATE = Me.BusDate
                                                v_strSTATUS_TEXT = CONFIRMED_VSD_MANAGER_TEXT
                                        End Select
                                        If intType = 2 Then
                                            v_strStatus = "4"
                                            v_strSTATUS_TEXT = REJECTED_LOG_TEXT
                                        Else
                                            v_strStatus = CStr(CLng(v_strStatus) + 1)
                                        End If

                                        Dim intIcon As Integer
                                        ' update screen 
                                        .Cells("STATUS").Value = CDec(v_strStatus)
                                        .Cells("STATUS_TEXT").Value = v_strSTATUS_TEXT
                                        Select Case v_strOldStatus
                                            Case "0"
                                                .Cells("CHKID").Value = v_strCHKID
                                                .Cells("MEMBER_LEADER").Value = v_strCHKNAME
                                                If v_strOFFID = "0" And v_strCFRID = "0" Then
                                                    .Cells("BUSDATE").Value = v_strBUSDATE
                                                End If
                                            Case "1"
                                                .Cells("OFFID").Value = v_strOFFID
                                                .Cells("VSD_STAFF").Value = v_strOFFNAME
                                                If v_strCFRID = "0" Then
                                                    .Cells("BUSDATE").Value = v_strBUSDATE
                                                End If
                                            Case "2"
                                                .Cells("CFRID").Value = v_strCFRID
                                                .Cells("VSD_LEADER").Value = v_strCFRNAME
                                                .Cells("BUSDATE").Value = v_strBUSDATE
                                        End Select
                                        If v_strStatus = "4" Then
                                            .Cells("DETAIL_TEXT").Value = .Cells("DETAIL_TEXT").Value & "vbCrLf" & v_strReason
                                        End If
                                        .Cells("AUTH_STRING").Value = "YNNNY"
                                        MyBase.btnView.Enabled = False
                                        MyBase.btnEdit.Enabled = False
                                        intIcon = IIf(v_strStatus = "4", 3, IIf(v_strBUSDATE = Me.BusDate, 2, 1))
                                        .Cells("TXIMAGE").Value = Icon.FromHandle(CType(MyBase.imgTool1.Images(CInt(intIcon)), Bitmap).GetHicon())
                                        .Cells("TXBLN").Value = False
                                    End If
                                End With
                            End If
                        Next
                    Next
                End If

                If intType = 3 Then
                    CType(MyBase.SearchGrid, GridEx).EndInit()
                    _FormatGridBefore(CType(MyBase.SearchGrid, GridEx), TellerId, , , False, , , , )
                End If
                
                'bangpv
                If intType <> 3 Then
                    If v_strSUSS_Msg <> "" Then
                        For Each v_oRow In SearchGrid.DataRows
                            For i = 0 To v_strSUSS_Msg.Split("#").Count - 2
                                If v_oRow.Cells("TXNUM").GetTextToPaint.ToUpper = v_strSUSS_Msg.Split("#")(i).Split("|")(2) Then
                                    With v_oRow
                                        If .Cells("TLTXCD").Value = "1131" And .Cells("STATUS").Value = "3" Then
                                            Dim v_ServerAddress As String
                                            Dim v_ServerPort As String
                                            Dim v_Username As String
                                            Dim v_Password As String
                                            Dim v_RemotePath As String
                                            Dim v_strBrid As String
                                            Dim v_strCurrDate As String
                                            Dim v_strSystime As String

                                            v_strBrid = GetBranchId(.Cells("TXNUM").Value).Substring(0, 4)
                                            v_strCurrDate = GetCurrDate(v_strBrid)
                                            v_strSystime = GetBranchId(.Cells("TXNUM").Value).Substring(4)
                                            v_strCurrDate = v_strCurrDate.Replace("/", "") & v_strSystime

                                            Dim v_strClientDir As String = "C:\GetFileFTP\" & v_strBrid & "\" & v_strCurrDate.Replace("/", "")
                                            Dim v_strFileName As String = CType(SearchGrid.CurrentRow, Xceed.Grid.CellRow).Cells("TLTXCD").Value _
                                                                            & v_strBrid & v_strCurrDate & "_ENCRYPTED" & ".zip"
                                            'get ftp
                                            GetFTPSBV(v_ServerAddress, v_ServerPort, v_Username, v_Password, _
                                                   v_RemotePath, v_strClientDir, v_strFileName, v_strBrid)

                                            'check file

                                            Dim v_strExportPath As String = GetExportFilePath()

                                            v_lngError = _
                                            ClientBussinessCA.ExtractFile1123_1124(v_strExportPath & v_strBrid & "\" & v_strCurrDate, _
                                                                              v_strClientDir & "\" & v_strFileName, v_strBrid, v_strCurrDate, CType(SearchGrid.CurrentRow, Xceed.Grid.CellRow).Cells("TLTXCD").Value)

                                        End If
                                    End With
                                End If
                            Next
                            'End Thanglv9
                        Next
                    End If
                    If (CType(SearchGrid.CurrentRow, Xceed.Grid.CellRow).Cells("TLTXCD").Value = "1123") _
                       Or (CType(SearchGrid.CurrentRow, Xceed.Grid.CellRow).Cells("TLTXCD").Value = "1124") Then

                        Dim v_ServerAddress As String
                        Dim v_ServerPort As String
                        Dim v_Username As String
                        Dim v_Password As String
                        Dim v_RemotePath As String
                        Dim v_strBrid As String
                        Dim v_strCurrDate As String


                        v_strBrid = GetBranchId(CType(SearchGrid.CurrentRow, Xceed.Grid.CellRow).Cells("TXNUM").Value)
                        v_strCurrDate = GetCurrDate(v_strBrid)
                        v_strCurrDate = v_strCurrDate.Replace("/", "")
                        Dim v_strClientDir As String = "C:\GetFileFTP\" & v_strBrid & "\" & v_strCurrDate.Replace("/", "")
                        Dim v_strFileName As String = CType(SearchGrid.CurrentRow, Xceed.Grid.CellRow).Cells("TLTXCD").Value _
                                                        & v_strBrid & v_strCurrDate & "_ENCRYPTED" & ".zip"
                        'get ftp
                        GetFTP(v_ServerAddress, v_ServerPort, v_Username, v_Password, _
                               v_RemotePath, v_strClientDir, v_strFileName, v_strBrid)

                        'check file

                        Dim v_strExportPath As String = GetExportFilePath()

                        v_lngError = _
                        ClientBussinessCA.ExtractFile1123_1124(v_strExportPath & v_strBrid & "\" & v_strCurrDate, _
                                                          v_strClientDir & "\" & v_strFileName, v_strBrid, v_strCurrDate, CType(SearchGrid.CurrentRow, Xceed.Grid.CellRow).Cells("TLTXCD").Value)




                    End If
                    'end bangpv
                    
                    If (CType(SearchGrid.CurrentRow, Xceed.Grid.CellRow).Cells("TLTXCD").Value = "1112" _
                                    And GetBranchId(CType(SearchGrid.CurrentRow, Xceed.Grid.CellRow).Cells("TXNUM").Value) = "0002") Then
                        Try
                            Dim v_ServerAddress As String
                            Dim v_ServerPort As String
                            Dim v_Username As String
                            Dim v_Password As String
                            Dim v_RemotePath As String
                            Dim v_strBrid As String
                            Dim v_strCurrDate As String


                            v_strBrid = GetBranchId(CType(SearchGrid.CurrentRow, Xceed.Grid.CellRow).Cells("TXNUM").Value)
                            v_strCurrDate = GetCurrDate(v_strBrid)
                            v_strCurrDate = v_strCurrDate.Replace("/", "")
                            Dim v_strClientDir As String = "C:\GetFileFTP\" & v_strBrid & "\" & v_strCurrDate.Replace("/", "")
                            Dim v_strFileName As String = "1112" & v_strBrid & v_strCurrDate & "ENCRYPTED" & ".xml"
                            'get ftp
                            GetFTP(v_ServerAddress, v_ServerPort, v_Username, v_Password, _
                                   v_RemotePath, v_strClientDir, v_strFileName, v_strBrid)

                            'check file
                            Dim v_oDi As New IO.DirectoryInfo(v_strClientDir)
                            Dim v_aryFi As IO.FileInfo() = v_oDi.GetFiles("1112" & v_strBrid & v_strCurrDate & "ENCRYPTED" & ".xml")
                            Dim v_oFi As IO.FileInfo
                            Dim v_strExportPath As String = GetExportFilePath()
                            For Each v_oFi In v_aryFi
                                v_lngError = _
                                ClientBussinessCA.ExtractFile1112(v_strExportPath & v_strBrid & "\" & v_strCurrDate, _
                                                                  v_oFi.FullName, v_strBrid, v_strCurrDate)
                                If (v_lngError <> 0) Then
                                    Exit Try
                                End If
                            Next

                        Catch ex As Exception
                            v_lngError = -1
                        End Try
                    End If
                    '1132- day file CW
                    If (CType(SearchGrid.CurrentRow, Xceed.Grid.CellRow).Cells("TLTXCD").Value = "1132" _
                                    And GetBranchId(CType(SearchGrid.CurrentRow, Xceed.Grid.CellRow).Cells("TXNUM").Value) = "0002") Then
                        Try
                            Dim v_ServerAddress As String
                            Dim v_ServerPort As String
                            Dim v_Username As String
                            Dim v_Password As String
                            Dim v_RemotePath As String
                            Dim v_strBrid As String
                            Dim v_strCurrDate As String


                            v_strBrid = GetBranchId(CType(SearchGrid.CurrentRow, Xceed.Grid.CellRow).Cells("TXNUM").Value)
                            v_strCurrDate = GetCurrDate(v_strBrid)
                            v_strCurrDate = v_strCurrDate.Replace("/", "")
                            Dim v_strClientDir As String = "C:\GetFileFTP\" & v_strBrid & "\" & v_strCurrDate.Replace("/", "")
                            Dim v_strFileName As String = "1132" & v_strBrid & v_strCurrDate & "ENCRYPTED" & ".xml"
                            'get ftp
                            GetFTP(v_ServerAddress, v_ServerPort, v_Username, v_Password, _
                                   v_RemotePath, v_strClientDir, v_strFileName, v_strBrid)

                            'check file
                            Dim v_oDi As New IO.DirectoryInfo(v_strClientDir)
                            Dim v_aryFi As IO.FileInfo() = v_oDi.GetFiles("1132" & v_strBrid & v_strCurrDate & "ENCRYPTED" & ".xml")
                            Dim v_oFi As IO.FileInfo
                            Dim v_strExportPath As String = GetExportFilePath()
                            For Each v_oFi In v_aryFi
                                v_lngError = _
                                ClientBussinessCA.ExtractFile1132(v_strExportPath & v_strBrid & "\" & v_strCurrDate, _
                                                                  v_oFi.FullName, v_strBrid, v_strCurrDate)
                                If (v_lngError <> 0) Then
                                    Exit Try
                                End If
                            Next

                        Catch ex As Exception
                            v_lngError = -1
                        End Try
                    End If
                    If (CType(SearchGrid.CurrentRow, Xceed.Grid.CellRow).Cells("TLTXCD").Value = "1114") Then
                        Try
                            Dim v_ServerAddress As String
                            Dim v_ServerPort As String
                            Dim v_Username As String
                            Dim v_Password As String
                            Dim v_RemotePath As String
                            Dim v_strBrid As String
                            Dim v_strCurrDate As String


                            v_strBrid = GetBranchId(CType(SearchGrid.CurrentRow, Xceed.Grid.CellRow).Cells("TXNUM").Value)
                            v_strCurrDate = GetDate1114(CType(SearchGrid.CurrentRow, Xceed.Grid.CellRow).Cells("TXNUM").Value)
                            v_strCurrDate = v_strCurrDate.Replace("/", "")
                            Dim v_strClientDir As String = "C:\GetFileFTP\" & v_strBrid & "\" & v_strCurrDate.Replace("/", "")
                            If Not (Directory.Exists(v_strClientDir)) Then
                                Directory.CreateDirectory(v_strClientDir)
                            End If
                            Dim v_strFileName As String = "1114" & v_strBrid & v_strCurrDate & "ENCRYPTED" & ".xml"
                            'get ftp
                            GetFTP(v_ServerAddress, v_ServerPort, v_Username, v_Password, _
                                   v_RemotePath, v_strClientDir, v_strFileName, v_strBrid)

                            'check file
                            Dim v_oDi As New IO.DirectoryInfo(v_strClientDir)
                            Dim v_aryFi As IO.FileInfo() = v_oDi.GetFiles("1114" & v_strBrid & v_strCurrDate & "ENCRYPTED" & ".xml")
                            Dim v_oFi As IO.FileInfo
                            Dim v_strExportPath As String = GetExportFilePath()
                            For Each v_oFi In v_aryFi
                                v_lngError = _
                                ClientBussinessCA.ExtractFile1114(v_strExportPath & v_strBrid & "\" & v_strCurrDate, _
                                                                  v_oFi.FullName, v_strBrid, v_strCurrDate)
                                If (v_lngError <> 0) Then
                                    Exit Try
                                End If
                            Next

                        Catch ex As Exception
                            v_lngError = -1
                        End Try
                    End If
                    If (CType(SearchGrid.CurrentRow, Xceed.Grid.CellRow).Cells("TLTXCD").Value = "1150") Then
                        Try
                            Dim v_ServerAddress As String
                            Dim v_ServerPort As String
                            Dim v_Username As String
                            Dim v_Password As String
                            Dim v_RemotePath As String
                            Dim v_strBrid As String
                            Dim v_strCurrDate As String
                            Dim v_strEndFile As String
                            GetTllogInfo1150(CType(SearchGrid.CurrentRow, Xceed.Grid.CellRow).Cells("TLTXCD").Value, _
                                             CType(SearchGrid.CurrentRow, Xceed.Grid.CellRow).Cells("TXNUM").Value, v_strBrid, v_strCurrDate, v_strEndFile)
                            'v_strBrid = GetBranchId(CType(SearchGrid.CurrentRow, Xceed.Grid.CellRow).Cells("TXNUM").Value)
                            'v_strCurrDate = GetDate1114(CType(SearchGrid.CurrentRow, Xceed.Grid.CellRow).Cells("TXNUM").Value)
                            v_strCurrDate = v_strCurrDate.Replace("/", "")
                            Dim v_strClientDir As String = "C:\GetFileFTP\" & v_strBrid & "\" & v_strCurrDate.Replace("/", "")
                            If Not (Directory.Exists(v_strClientDir)) Then
                                Directory.CreateDirectory(v_strClientDir)
                            End If
                            Select Case v_strEndFile
                                Case "1"
                                    v_strEndFile = "_PT"
                                Case "2"
                                    v_strEndFile = "_TT"
                                Case "3"
                                    v_strEndFile = "_GT"
                            End Select
                            Dim v_strFileName As String = "1150" & v_strBrid & v_strCurrDate & v_strEndFile & "ENCRYPTED" & ".xml"
                            'get ftp
                            GetFTP(v_ServerAddress, v_ServerPort, v_Username, v_Password, _
                                   v_RemotePath, v_strClientDir, v_strFileName, v_strBrid)

                            'check file
                            Dim v_oDi As New IO.DirectoryInfo(v_strClientDir)
                            Dim v_aryFi As IO.FileInfo() = v_oDi.GetFiles("1150" & v_strBrid & v_strCurrDate & v_strEndFile & "ENCRYPTED" & ".xml")
                            Dim v_oFi As IO.FileInfo
                            Dim v_strExportPath As String = GetExportFilePath()
                            For Each v_oFi In v_aryFi
                                v_lngError = _
                                ClientBussinessCA.ExtractFile1114(v_strExportPath & v_strBrid & "\" & v_strCurrDate, _
                                                                  v_oFi.FullName, v_strBrid, v_strCurrDate)
                                If (v_lngError <> 0) Then
                                    Exit Try
                                End If
                            Next

                        Catch ex As Exception
                            v_lngError = -1
                        End Try
                    End If
                    'lay file tu NHCDTT
                    If (CType(SearchGrid.CurrentRow, Xceed.Grid.CellRow).Cells("TLTXCD").Value = "1125") Then

                        Dim v_ServerAddress As String
                        Dim v_ServerPort As String
                        Dim v_Username As String
                        Dim v_Password As String
                        Dim v_RemotePath As String
                        Dim v_strBrid As String
                        Dim v_strCurrDate As String


                        v_strBrid = GetBranchId(CType(SearchGrid.CurrentRow, Xceed.Grid.CellRow).Cells("TXNUM").Value)
                        v_strCurrDate = GetDate1114(CType(SearchGrid.CurrentRow, Xceed.Grid.CellRow).Cells("TXNUM").Value)
                        v_strCurrDate = v_strCurrDate.Replace("/", "")
                        Dim v_strClientDir As String = "C:\GetFileFTP\" & v_strBrid & "\" & v_strCurrDate.Replace("/", "")
                        If Not (Directory.Exists(v_strClientDir)) Then
                            Directory.CreateDirectory(v_strClientDir)
                        End If
                        Dim v_strFileName As String = "1125" & v_strBrid & v_strCurrDate & "_ENCRYPTED" & ".zip"
                        'get ftp
                        GetFTP(v_ServerAddress, v_ServerPort, v_Username, v_Password, _
                               v_RemotePath, v_strClientDir, v_strFileName, v_strBrid)

                        'check file


                        Dim v_strExportPath As String = GetExportFilePath()

                        v_lngError = ClientBussinessCA.ExtractFile1125(v_strExportPath & v_strBrid & "\" & v_strCurrDate, _
                                                          v_strClientDir & "\" & v_strFileName, v_strBrid, v_strCurrDate)



                    End If
                    'Hanm5 Them giao dich trao doi file giua SBL va NHTT
                    If CType(SearchGrid.CurrentRow, Xceed.Grid.CellRow).Cells("TLTXCD").Value = "2150" Or CType(SearchGrid.CurrentRow, Xceed.Grid.CellRow).Cells("TLTXCD").Value = "2151" Then
                        Dim v_strTempTltxcd As String = CType(SearchGrid.CurrentRow, Xceed.Grid.CellRow).Cells("TLTXCD").Value
                        Dim v_strTempTxNum As String = CType(SearchGrid.CurrentRow, Xceed.Grid.CellRow).Cells("TXNUM").Value
                        Dim v_ServerAddress As String
                        Dim v_ServerPort As String
                        Dim v_Username As String
                        Dim v_Password As String
                        Dim v_RemotePath As String
                        Dim v_strBrid As String
                        Dim v_strTradeDate As String
                        Dim v_strFileName As String

                        GetSblFptBnkTllogInfo(v_strTempTltxcd, v_strTempTxNum, v_strBrid, v_strTradeDate, v_strFileName)

                        Dim v_strClientDir As String = "C:\GetFileFTP\" & v_strTempTltxcd & "\" & v_strBrid & "\" & v_strTradeDate.Replace("/", "")
                        If Not (Directory.Exists(v_strClientDir)) Then
                            Directory.CreateDirectory(v_strClientDir)
                        End If
                        If v_strTempTltxcd = "2151" Then
                            Dim v_fiTemp As New FileInfo(v_strFileName)
                            v_strFileName = v_fiTemp.Name
                        End If
                        Dim v_fi As New FileInfo(v_strClientDir & "\" & Mid(v_strFileName, 1, v_strFileName.Length - 4) & "ENCRYPTED.xml")
                        GetFileSblFtpBnk("SBLFTPBNK", v_strBrid, v_fi.Name, v_strClientDir & "\" & v_fi.Name)

                        v_lngError = ClientBussinessCA.ExtractFile1114(v_strClientDir, v_fi.FullName, v_strBrid, v_strTradeDate)
                    End If
                End If
                'End Myvq
                'v_thread.Abort()
                v_oProccess.StopProcessForm()
                Application.DoEvents()
                ShowTranMessage(v_strTxMsg, v_lngError, intType)
                Me.Focus()
                Exit Sub
            Else
                MyBase.btnView.Enabled = True
                MyBase.btnEdit.Enabled = True
            End If

        Catch ex As Exception
            v_oProccess.StopProcessForm()
            Application.DoEvents()
            MyBase.btnView.Enabled = True
            MyBase.btnEdit.Enabled = True
            Throw ex
        Finally
            v_oProccess.StopProcessForm()
            Application.DoEvents()
            v_oProccess = Nothing
            Me.Focus()
        End Try
    End Sub

    Private Sub ShowTranMessage(ByVal pv_strMessage As String, ByVal pv_lngErr As Long, ByVal intType As Integer)
        Dim v_xmlDocument As New Xml.XmlDocument
        Dim v_nodeList As Xml.XmlNodeList = Nothing
        Dim v_strFLDNAME As String = ""
        Dim v_strValue As String = ""
        Dim v_strERR_Msg As String = ""
        Dim v_strSUSS_Msg As String = ""
        Dim v_strMsg As String

        Try
            v_xmlDocument.LoadXml(pv_strMessage)
            Dim v_attrColl As Xml.XmlAttributeCollection = v_xmlDocument.DocumentElement.Attributes

            'Xác định loại message
            If InStr(v_xmlDocument.InnerXml, "ROOT") > 0 Then
                v_nodeList = v_xmlDocument.SelectNodes("/ROOT/TRAN_MESSAGE")
            Else
                v_nodeList = v_xmlDocument.SelectNodes("/TransactMessage/TRAN_MESSAGE")
            End If

            'Lấy thông tin msg
            If v_nodeList.Count > 0 Then 'Có thông tin msg
                For i As Integer = 0 To v_nodeList.Count - 1
                    For j As Integer = 0 To v_nodeList.Item(i).ChildNodes.Count - 1
                        With v_nodeList.Item(i).ChildNodes(j)
                            v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                            v_strValue = .InnerText.ToString

                            Select Case Trim(v_strFLDNAME)
                                Case "SUCC_MSG"
                                    v_strSUSS_Msg = Trim(v_strValue)
                                Case "ERR_MSG"
                                    v_strERR_Msg = Trim(v_strValue)
                            End Select
                        End With
                    Next
                Next
            End If

            Dim v_arrTmp() As String
            Dim v_int As Integer
            Dim v_intMSGSSUC As Integer = 0
            Dim v_intMSGERR As Integer = 0
            If Trim(v_strSUSS_Msg) <> "" Then
                v_arrTmp = v_strSUSS_Msg.Split("#")
                For v_int = 0 To v_arrTmp.Length - 2
                    With v_arrTmp(v_int)
                        v_strMsg &= "     - " & .Split("|")(0) & " Giao dịch " & .Split("|")(1) & "(" & .Split("|")(2) & " - " & .Split("|")(3) & ") ''" & .Split("|")(4) & "'' - THÀNH CÔNG." & vbCrLf
                        v_intMSGSSUC += 1
                    End With
                Next
            End If

            v_strMsg &= "#"

            If Trim(v_strERR_Msg) <> "" Then
                v_arrTmp = v_strERR_Msg.Split("#")
                For v_int = 0 To v_arrTmp.Length - 2
                    With v_arrTmp(v_int)
                        v_strMsg &= "     - " & .Split("|")(0) & " Giao dịch " & .Split("|")(1) & "(" & .Split("|")(2) & " - " & .Split("|")(3) & ") ''" & .Split("|")(4) & "'' - KHÔNG THÀNH CÔNG!." & vbCrLf
                        v_intMSGERR += 1
                    End With
                Next
            End If
            If InStr(v_xmlDocument.InnerXml, "ROOT") > 0 Then
                v_nodeList = v_xmlDocument.SelectNodes("/ROOT/FAILED_MESSAGE")
            Else
                v_nodeList = v_xmlDocument.SelectNodes("/TransactMessage/FAILED_MESSAGE")
            End If
            If v_strMsg <> "" Then
                If v_intMSGSSUC + v_intMSGERR > 1 Or Not v_nodeList Is Nothing Then
                    v_strMsg = " + Có " & v_intMSGSSUC & " giao dịch thực hiện thành công, " & v_intMSGERR & " giao dịch thực hiện bị lỗi" & vbCrLf & "#" & v_strMsg
                    Dim v_frm As New frmShowTranError
                    v_frm.PublicMessage = v_strMsg

                    v_frm.ErrMsg = v_nodeList
                    v_frm.OnInit()
                    v_frm.ShowDialog()
                    Application.DoEvents()
                    If v_frm.TranMessage <> "" Then
                        ApproveWarningTran(v_frm.TranMessage, intType)
                    End If
                Else
                    If v_intMSGERR = 1 Then
                        MsgBox(v_strMsg, MsgBoxStyle.Critical, gc_ApplicationTitle)
                    Else
                        MsgBox(v_strMsg, MsgBoxStyle.Information, gc_ApplicationTitle)
                    End If
                End If
            Else
                If pv_lngErr <> ERR_SYSTEM_OK Then
                    MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub ApproveWarningTran(ByVal pv_strTran As String, ByVal intType As Integer)
        Try
            Dim v_arrTran() As String = pv_strTran.Split("$")
            Dim v_int As Integer
            Dim v_oRow As Xceed.Grid.DataRow
            Dim v_lngError As Long = ERR_SYSTEM_OK
            Dim v_strTxMsg, v_strInitialTxMsg As String
            Dim v_strTLTXCD, v_strMSGAMT, v_strSICODE, v_strCOMICODE, v_strMICODE, v_strStatus, v_strOldStatus, v_strAUTOID, v_strTXDATE, v_strTXNUM, v_strBUSDATE As String
            Dim v_strPARENTID, v_strBRCODE, v_strTXNAME, v_strSTATUS_TEXT, v_strPARENT_TEXT As String
            Dim v_strTLID, v_strOFFID, v_strCHKID, v_strCFRID, v_strISPARENT As String
            Dim v_strTLNAME, v_strCHKNAME, v_strOFFNAME, v_strCFRNAME, v_strCHILDTLTXCD, v_strISBRID, v_strReason, v_strTxNote, v_strVsdBrid, v_strTblChk As String
            Dim v_strClause, v_strObjMsg As String
            Dim v_xmlDocument As New Xml.XmlDocument
            Dim v_intMutilTran As Integer = 0
            v_oProccess.StartProcessForm()

            For Each v_oRow In SearchGrid.DataRows
                For v_int = 0 To v_arrTran.Count - 2
                    If v_oRow.Cells("TXNUM").GetTextToPaint.ToUpper = v_arrTran(v_int).Split("|")(1) _
                             And v_oRow.Cells("TXDATE").GetTextToPaint = v_arrTran(v_int).Split("|")(2) Then
                        With v_oRow
                            v_intMutilTran = v_intMutilTran + 1
                            ' kiem tra xem nguoi dung co duoc thuc hien giao dich nay, kiem tra thong tin han muc giao dich
                            v_strTLTXCD = .Cells("TLTXCD").Value
                            v_strMSGAMT = .Cells("MSGAMT").Value
                            v_strStatus = .Cells("STATUS").Value
                            v_strBUSDATE = .Cells("BUSDATE").Value
                            v_strPARENTID = .Cells("PARENTID").Value
                            v_strCHILDTLTXCD = .Cells("CHILDTLTXCD").Value
                            v_strBRCODE = .Cells("BRANCH_NAME").Value
                            v_strTXNAME = .Cells("TRAN_NAME").Value

                            v_strPARENT_TEXT = .Cells("ISPARENT_TEXT").Value
                            v_strTLID = .Cells("TLID").Value
                            v_strTLNAME = .Cells("MEMBER_STAFF").Value
                            v_strCHKID = .Cells("CHKID").Value
                            v_strCHKNAME = .Cells("MEMBER_LEADER").Value
                            v_strOFFID = .Cells("OFFID").Value
                            v_strOFFNAME = .Cells("VSD_STAFF").Value
                            v_strCFRID = .Cells("CFRID").Value
                            v_strCFRNAME = .Cells("VSD_LEADER").Value
                            v_strISPARENT = .Cells("ISPARENT").Value
                            v_strAUTOID = .Cells("AUTOID").Value
                            v_strTXDATE = .Cells("TXDATE").Value
                            v_strTXNUM = .Cells("TXNUM").Value

                            v_strSICODE = .Cells("SICODE").Value
                            v_strMICODE = .Cells("MICODE").Value
                            v_strCOMICODE = .Cells("COMICODE").Value
                            v_strISBRID = .Cells("ISBRID").Value
                            v_strOldStatus = v_strStatus
                            v_strTxNote = .Cells("TXNOTE").Value
                            v_strVsdBrid = .Cells("VSD_BRID").Value

                            Select Case v_strStatus
                                Case "0"
                                    v_strCHKID = Me.TellerId
                                    v_strCHKNAME = m_BusLayer.CurrentTellerProfile.TellerName
                                    v_strSTATUS_TEXT = APPROVED_MEMBER_MANAGER_TEXT
                                    If v_strOFFID = "0" And v_strCFRID = "0" Then
                                        v_strBUSDATE = Me.BusDate
                                    End If
                                Case "1"
                                    v_strOFFID = Me.TellerId
                                    v_strOFFNAME = m_BusLayer.CurrentTellerProfile.TellerName
                                    v_strSTATUS_TEXT = APPROVED_VDS_STAFF_TEXT
                                    If v_strCFRID = "0" Then
                                        v_strBUSDATE = Me.BusDate
                                    End If
                                Case "2"
                                    v_strCFRID = Me.TellerId
                                    v_strCFRNAME = m_BusLayer.CurrentTellerProfile.TellerName
                                    v_strBUSDATE = Me.BusDate
                                    v_strSTATUS_TEXT = CONFIRMED_VSD_MANAGER_TEXT
                            End Select
                            If intType = 2 Then
                                v_strStatus = "4"
                                v_strSTATUS_TEXT = REJECTED_LOG_TEXT
                            ElseIf intType = 3 Then
                                v_strStatus = "4"
                                v_strSTATUS_TEXT = DELETED_TRANS_TEXT
                            Else
                                v_strStatus = CStr(CLng(v_strStatus) + 1)
                            End If


                            v_strClause = "select MSG_AMT, TBLCHK from tltx where tltxcd = '" & IIf(v_strCHILDTLTXCD <> "", v_strCHILDTLTXCD, v_strTLTXCD) & "'"
                            v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLTX, gc_ActionInquiry, v_strClause)
                            v_lngError = Proxy.Message(v_strObjMsg)

                            If v_lngError <> ERR_SYSTEM_OK Then
                                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                                Exit Sub
                            End If

                            v_xmlDocument.LoadXml(v_strObjMsg)
                            If Not IsDBNull(v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='MSG_AMT']")) Then
                                v_strMSGAMT = v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='MSG_AMT']").InnerText
                            End If

                            If Not IsDBNull(v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='TBLCHK']")) Then
                                v_strTblChk = v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='TBLCHK']").InnerText
                            End If

                            'Tạo điện giao dịch
                            v_strTxMsg &= BuildXMLTxMsg(gc_MsgTypeTrans, gc_IsLocalMsg, IIf(intType = 3, "9002", IIf(v_strCHILDTLTXCD <> "", v_strCHILDTLTXCD, v_strTLTXCD)), _
                                        Me.BranchId, v_strTLID, Me.IpAddress, Me.WsName, _
                                        v_strStatus, IIf(InStr(v_strTLTXCD, "44") > 0, "Y", "N") _
                                        , v_strOFFID, v_strCHKID, v_strCFRID, v_strTXDATE, , v_strTXNUM, , v_strReason, v_strMSGAMT, v_strMICODE, , , Me.UserLanguage, v_strBUSDATE, v_strSICODE, _
                                        , v_strISPARENT, v_strPARENTID, v_strBRCODE, v_strTLNAME, v_strOFFNAME, v_strCHKNAME, _
                                        v_strCFRNAME, v_strTXNAME, v_strSTATUS_TEXT, v_strPARENT_TEXT, v_strAUTOID, gc_MsgTypeTrans, v_strISBRID, v_strCOMICODE, v_strReason, , , v_strOldStatus, v_strTxNote, v_strVsdBrid, v_strTblChk, v_strTLTXCD) & vbCrLf
                        End With
                    End If
                Next
            Next

            If v_intMutilTran >= 1 Then
                If v_intMutilTran >= 2 Then
                    v_strTxMsg = "<ROOT MSGTYPE=""T"" LOCAL=""Y"" LANGUAGE=""VN"" TXTYPE=""T"" BRID=""" & Me.BranchId & """ TLID=""" & TellerId & """>" & v_strTxMsg & "</ROOT>"
                End If

                ' 2. Accept transaction
                v_strTxMsg = v_strTxMsg.Replace(gc_AtributeMISSING_WARNING & "=""0""", gc_AtributeMISSING_WARNING & "=""1""")
                Proxy.Message(v_strTxMsg)

                Dim v_nodeList As Xml.XmlNodeList
                v_xmlDocument.LoadXml(v_strTxMsg)

                If InStr(v_xmlDocument.InnerXml, "ROOT") > 0 Then
                    v_nodeList = v_xmlDocument.SelectNodes("/ROOT/TRAN_MESSAGE")
                Else
                    v_nodeList = v_xmlDocument.SelectNodes("/TransactMessage/TRAN_MESSAGE")
                End If

                Dim v_strFLDNAME, v_strValue, v_strSUSS_Msg As String
                For j As Integer = 0 To v_nodeList.Item(0).ChildNodes.Count - 1
                    With v_nodeList.Item(0).ChildNodes(j)
                        v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                        v_strValue = .InnerText.ToString
                        If Trim(v_strFLDNAME) = "SUCC_MSG" Then
                            v_strSUSS_Msg = Trim(v_strValue)
                        End If
                    End With
                Next

                If intType = 3 Then
                    CType(MyBase.SearchGrid, GridEx).BeginInit()
                End If
                If v_strSUSS_Msg <> "" Then
                    For Each v_oRow In SearchGrid.DataRows
                        For i = 0 To v_strSUSS_Msg.Split("#").Count - 2
                            If v_oRow.Cells("TXNUM").GetTextToPaint.ToUpper = v_strSUSS_Msg.Split("#")(i).Split("|")(2) _
                                  And v_oRow.Cells("TXDATE").GetTextToPaint = v_strSUSS_Msg.Split("#")(i).Split("|")(3) Then

                                With v_oRow
                                    If intType = 3 Then
                                        CType(MyBase.SearchGrid, GridEx).DataRows.Remove(v_oRow)
                                    Else
                                        v_strTLTXCD = .Cells("TLTXCD").Value
                                        v_strMSGAMT = .Cells("MSGAMT").Value
                                        v_strStatus = .Cells("STATUS").Value
                                        v_strBUSDATE = .Cells("BUSDATE").Value
                                        v_strPARENTID = .Cells("PARENTID").Value
                                        v_strCHILDTLTXCD = .Cells("CHILDTLTXCD").Value
                                        v_strBRCODE = .Cells("BRANCH_NAME").Value
                                        v_strTXNAME = .Cells("TRAN_NAME").Value

                                        v_strPARENT_TEXT = .Cells("ISPARENT_TEXT").Value
                                        v_strTLID = .Cells("TLID").Value
                                        v_strTLNAME = .Cells("MEMBER_STAFF").Value
                                        v_strCHKID = .Cells("CHKID").Value
                                        v_strCHKNAME = .Cells("MEMBER_LEADER").Value
                                        v_strOFFID = .Cells("OFFID").Value
                                        v_strOFFNAME = .Cells("VSD_STAFF").Value
                                        v_strCFRID = .Cells("CFRID").Value
                                        v_strCFRNAME = .Cells("VSD_LEADER").Value
                                        v_strISPARENT = .Cells("ISPARENT").Value
                                        v_strAUTOID = .Cells("AUTOID").Value
                                        v_strTXDATE = .Cells("TXDATE").Value
                                        v_strTXNUM = .Cells("TXNUM").Value

                                        v_strSICODE = .Cells("SICODE").Value
                                        v_strMICODE = .Cells("MICODE").Value
                                        v_strCOMICODE = .Cells("COMICODE").Value
                                        v_strISBRID = .Cells("ISBRID").Value

                                        v_strOldStatus = v_strStatus
                                        Select Case v_strStatus
                                            Case "0"
                                                v_strCHKID = Me.TellerId
                                                v_strCHKNAME = m_BusLayer.CurrentTellerProfile.TellerName
                                                v_strSTATUS_TEXT = APPROVED_MEMBER_MANAGER_TEXT
                                                If v_strOFFID = "0" And v_strCFRID = "0" Then
                                                    v_strBUSDATE = Me.BusDate
                                                End If
                                            Case "1"
                                                v_strOFFID = Me.TellerId
                                                v_strOFFNAME = m_BusLayer.CurrentTellerProfile.TellerName
                                                v_strSTATUS_TEXT = APPROVED_VDS_STAFF_TEXT
                                                If v_strCFRID = "0" Then
                                                    v_strBUSDATE = Me.BusDate
                                                End If
                                            Case "2"
                                                v_strCFRID = Me.TellerId
                                                v_strCFRNAME = m_BusLayer.CurrentTellerProfile.TellerName
                                                v_strBUSDATE = Me.BusDate
                                                v_strSTATUS_TEXT = CONFIRMED_VSD_MANAGER_TEXT
                                        End Select
                                        If intType = 2 Then
                                            v_strStatus = "4"
                                            v_strSTATUS_TEXT = REJECTED_LOG_TEXT
                                        Else
                                            v_strStatus = CStr(CLng(v_strStatus) + 1)
                                        End If

                                        Dim intIcon As Integer
                                        ' update screen 
                                        .Cells("STATUS").Value = CDec(v_strStatus)
                                        .Cells("STATUS_TEXT").Value = v_strSTATUS_TEXT
                                        Select Case v_strOldStatus
                                            Case "0"
                                                .Cells("CHKID").Value = v_strCHKID
                                                .Cells("MEMBER_LEADER").Value = v_strCHKNAME
                                                If v_strOFFID = "0" And v_strCFRID = "0" Then
                                                    .Cells("BUSDATE").Value = v_strBUSDATE
                                                End If
                                            Case "1"
                                                .Cells("OFFID").Value = v_strOFFID
                                                .Cells("VSD_STAFF").Value = v_strOFFNAME
                                                If v_strCFRID = "0" Then
                                                    .Cells("BUSDATE").Value = v_strBUSDATE
                                                End If
                                            Case "2"
                                                .Cells("CFRID").Value = v_strCFRID
                                                .Cells("VSD_LEADER").Value = v_strCFRNAME
                                                .Cells("BUSDATE").Value = v_strBUSDATE
                                        End Select
                                        If v_strStatus = "4" Then
                                            .Cells("DETAIL_TEXT").Value = .Cells("DETAIL_TEXT").Value & "vbCrLf" & v_strReason
                                        End If
                                        .Cells("AUTH_STRING").Value = "YNNNY"
                                        MyBase.btnView.Enabled = False
                                        MyBase.btnEdit.Enabled = False
                                        intIcon = IIf(v_strStatus = "4", 3, IIf(v_strBUSDATE = Me.BusDate, 2, 1))
                                        .Cells("TXIMAGE").Value = Icon.FromHandle(CType(MyBase.imgTool1.Images(CInt(intIcon)), Bitmap).GetHicon())
                                        .Cells("TXBLN").Value = False
                                    End If
                                End With
                            End If
                        Next
                    Next
                End If

                If intType = 3 Then
                    CType(MyBase.SearchGrid, GridEx).EndInit()
                    _FormatGridBefore(CType(MyBase.SearchGrid, GridEx), TellerId, , , False, , , , )
                End If
                'v_thread.Abort()
                v_oProccess.StopProcessForm()
                Application.DoEvents()
                ShowTranMessage(v_strTxMsg, v_lngError, intType)
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Function getSucceedTran(ByVal v_strTxMsg As String, ByVal v_strAutoID As String, Optional ByRef v_strTranMessage As String = "NULL") As Boolean
        Dim v_nodeList As Xml.XmlNodeList
        Dim v_xmlDocument As New Xml.XmlDocument
        Try
            v_xmlDocument.LoadXml(v_strTxMsg)
            If InStr(v_xmlDocument.InnerXml, "ROOT") > 0 Then
                v_nodeList = v_xmlDocument.SelectNodes("/ROOT/HOST_MESSAGE")
            Else
                v_nodeList = v_xmlDocument.SelectNodes("/TransactMessage/HOST_MESSAGE")
            End If
            For i As Integer = 0 To v_nodeList.Count - 1
                If v_strTranMessage <> "NULL" Then
                    v_strTranMessage = v_strTranMessage & "Giao dịch (" _
                    & CStr(CType(v_nodeList.Item(i).Attributes.GetNamedItem(gc_AtributeTXNUM), Xml.XmlAttribute).Value) & ", " _
                    & CStr(CType(v_nodeList.Item(i).Attributes.GetNamedItem(gc_AtributeTXDATE), Xml.XmlAttribute).Value) & ") thực hiện " _
                    & IIf(CStr(CType(v_nodeList.Item(i).Attributes.GetNamedItem(gc_AtributeSTATUS), Xml.XmlAttribute).Value) = "1", "THÀNH CÔNG", "THẤT BẠI") & vbCrLf
                End If

                If CStr(CType(v_nodeList.Item(i).Attributes.GetNamedItem(gc_AtributeAUTOID), Xml.XmlAttribute).Value) = v_strAutoID _
                    And CStr(CType(v_nodeList.Item(i).Attributes.GetNamedItem(gc_AtributeSTATUS), Xml.XmlAttribute).Value) = "1" Then
                    Return True
                End If
            Next
            Return False
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function GetListRptString(ByVal pv_strTLTXCD As String) As String
        Dim v_strLstRpt As String = ""
        Try
            Dim v_strSQL As String
            Dim v_obj As New SQLDataAccessLayer(TellerId)
            Dim v_ds As DataSet
            v_strSQL = "SELECT a.LSTRPT LSTRPT FROM TLTX a where a.TLTXCD='" & pv_strTLTXCD & "'"
            v_ds = v_obj.ExecuteReturnDataSet(v_strSQL)
            v_strLstRpt = v_ds.Tables(0).Rows(0)("LSTRPT")
            v_obj.CloseConnection()
            v_obj = Nothing
            Return v_strLstRpt
        Catch ex As Exception
            Throw ex
            Return v_strLstRpt
        End Try
    End Function

    Private Sub OnViewRptTrans()
        Dim i As Integer
        Try
            If Not CType(MyBase.SearchGrid, GridEx).CurrentRow Is Nothing Then
                If CType(MyBase.SearchGrid, GridEx).CurrentRow.GetType.ToString = "Xceed.Grid.DataRow" Then
                    If Not CType(CType(MyBase.SearchGrid, GridEx).CurrentRow, Xceed.Grid.DataRow).Cells("AUTH_STRING").Value Is Nothing Then
                        Dim v_intRowCount As Integer = 0
                        Dim v_blnChecked As Boolean = False

                        Dim v_strRptId As String = ""
                        Dim v_strAuthString As String = "NNNN"
                        Dim v_strTXNUM As String = ""
                        Dim v_strTLTXCD As String = ""
                        Dim v_strAUTOID As String = ""
                        Dim v_strMICODE As String = ""
                        Dim v_strSICODE As String = ""
                        Dim v_strBRID As String = ""

                        Dim v_strCtlVal As String = ""

                        With CType(MyBase.SearchGrid, GridEx)
                            For i = 0 To .DataRows.Count - 1
                                With .DataRows(i)
                                    If .Cells("TXBLN").Value Then
                                        v_intRowCount = v_intRowCount + 1
                                        Dim v_strTemp As String = .Cells("TLTXCD").Value
                                        If v_strTLTXCD <> "" And v_strTLTXCD <> v_strTemp Then
                                            v_blnChecked = True
                                        End If
                                        v_strTLTXCD = Trim(.Cells("TLTXCD").Value)
                                        v_strAUTOID = Trim(.Cells("AUTOID").Value)
                                        v_strTXNUM = Trim(.Cells("TXNUM").Value)
                                        v_strMICODE = Trim(.Cells("MICODE").Value)
                                        v_strSICODE = Trim(.Cells("SICODE").Value)
                                        v_strBRID = Trim(.Cells("BRANCH_NAME").Value)
                                        v_strCtlVal &= v_strTXNUM & "|" & v_strMICODE & " - " & v_strSICODE & " - " _
                                                        & v_strBRID & "$"
                                    End If
                                End With
                            Next
                        End With

                        If v_strCtlVal = "" Then
                            MsgBox("Bạn chưa tích chọn giao dịch để in báo cáo !", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                            Exit Sub
                        Else
                            If v_blnChecked Then
                                MsgBox("Bạn phải chọn cùng một loại giao dịch để in báo cáo !", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                                Exit Sub
                            End If
                        End If
                        Dim frm As New Sats.AppCore.frmSelectRpt
                        
                        frm.TXNUM = v_strTXNUM
                        frm.TLTXCD = v_strTLTXCD
                        frm.AUTOID = v_strAUTOID
                        frm.BRID = m_BusLayer.CurrentTellerProfile.BranchId
                        frm.TellerID = m_BusLayer.CurrentTellerProfile.TellerId
                        frm.Proxy = Proxy
                        Dim v_strLstRpt As String = GetListRptString(v_strTLTXCD)

                        If v_strLstRpt <> "" Then
                            frm.LSTRPT = v_strLstRpt
                            frm.FillDataCombobox()
                            If frm.mv_blnBoolean = True Then
                                frm.ShowDialog()
                            Else
                                frm.Dispose()
                            End If

                            v_strRptId = frm.SELECTED
                            v_strAuthString = frm.AuthString
                            If v_strRptId <> "" And v_strCtlVal <> "" Then
                                ShowReport(v_strRptId, v_strAuthString, Mid(v_strCtlVal, 1, v_strCtlVal.Length - 1))
                            End If
                        Else
                            MsgBox("Không có báo cáo nào liên quan tới giao dịch này !", MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                            Exit Sub
                        End If
                        'End If
                    End If
                End If
            End If
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                         & "Error code: System error!" & vbNewLine _
                                         & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)

        Finally
            GC.Collect()
        End Try
    End Sub
    'bangpv
    Sub DoDelete()
        Dim v_lngError As Long = ERR_SYSTEM_OK
        Dim v_strTxMsg, v_strInitialTxMsg As String
        Dim v_strTLTXCD, v_strMSGAMT, v_strSICODE, v_strCOMICODE, v_strMICODE, v_strStatus, v_strOldStatus, v_strAUTOID, v_strTXDATE, v_strTXNUM, v_strBUSDATE As String
        Dim v_strPARENTID, v_strBRCODE, v_strTXNAME, v_strSTATUS_TEXT, v_strPARENT_TEXT As String
        Dim v_strTLID, v_strOFFID, v_strCHKID, v_strCFRID, v_strISPARENT As String
        Dim v_strTLNAME, v_strCHKNAME, v_strOFFNAME, v_strCFRNAME, v_strCHILDTLTXCD, v_strISBRID, v_strReason As String
        Try
            'v_thread = New Threading.Thread(AddressOf ShowProcessForm)
            v_oProccess.StartProcessForm()
            If Not CType(MyBase.SearchGrid, GridEx).CurrentRow Is Nothing Then
                If CType(MyBase.SearchGrid, GridEx).CurrentRow.GetType.ToString = "Xceed.Grid.DataRow" Then

                    With CType(CType(MyBase.SearchGrid, GridEx).CurrentRow, Xceed.Grid.DataRow)
                        ' kiem tra xem nguoi dung co duoc thuc hien giao dich nay, kiem tra thong tin han muc giao dich
                        v_strTLTXCD = .Cells("TLTXCD").Value
                        v_strMSGAMT = .Cells("MSGAMT").Value
                        v_strStatus = .Cells("STATUS").Value
                        v_strBUSDATE = .Cells("BUSDATE").Value
                        'If Not CheckTranLimit(v_strTLTXCD, v_strMSGAMT, CInt(v_strStatus) + 1) Then
                        '    Exit Sub
                        'End If
                        'If IsDate(v_strBUSDATE) Then
                        '    If CDate(v_strBUSDATE) > CDate(m_BusLayer.CurrentTellerProfile.BusDate) Then
                        '        MsgBox("Giao dịch này chưa đến ngày duyệt", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "Thông báo")
                        '        Exit Sub
                        '    End If
                        'End If
                        v_strSTATUS_TEXT = DELETED_TRANS_TEXT
                        v_strPARENTID = .Cells("PARENTID").Value
                        v_strCHILDTLTXCD = .Cells("CHILDTLTXCD").Value
                        v_strBRCODE = .Cells("BRANCH_NAME").Value
                        v_strTXNAME = .Cells("TRAN_NAME").Value

                        v_strPARENT_TEXT = .Cells("ISPARENT_TEXT").Value
                        v_strTLID = .Cells("TLID").Value
                        v_strTLNAME = .Cells("MEMBER_STAFF").Value
                        v_strCHKID = .Cells("CHKID").Value
                        v_strCHKNAME = .Cells("MEMBER_LEADER").Value
                        v_strOFFID = .Cells("OFFID").Value
                        v_strOFFNAME = .Cells("VSD_STAFF").Value
                        v_strCFRID = .Cells("CFRID").Value
                        v_strCFRNAME = .Cells("VSD_LEADER").Value
                        v_strISPARENT = .Cells("ISPARENT").Value
                        v_strAUTOID = .Cells("AUTOID").Value
                        v_strTXDATE = .Cells("TXDATE").Value
                        v_strTXNUM = .Cells("TXNUM").Value

                        v_strSICODE = .Cells("SICODE").Value
                        v_strMICODE = .Cells("MICODE").Value
                        v_strCOMICODE = .Cells("COMICODE").Value
                        v_strISBRID = .Cells("ISBRID").Value
                        v_strReason = ""
                        v_strOldStatus = v_strStatus
                        Select Case v_strStatus
                            Case "1"
                                MsgBox("Giao dịch này đã được duyệt ở cấp " & v_strStatus, MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "Thông báo")
                                Exit Sub
                            Case "2"
                                MsgBox("Giao dịch này đã được duyệt ở cấp " & v_strStatus, MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "Thông báo")
                                Exit Sub
                            Case "3"
                                MsgBox("Giao dịch này đã được duyệt ở cấp " & v_strStatus, MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "Thông báo")
                                Exit Sub
                        End Select
                        'If Not blnIsApprove Then
                        '    v_strStatus = "4"
                        '    v_strReason = InputBox("Bạn vui lòng nhập lý do từ chối duyệt: ", "Nhập lý do từ chối duyệt", "")
                        '    v_strReason = "Lý do từ chối duyệt là: " & v_strReason
                        '    v_strSTATUS_TEXT = REJECTED_LOG_TEXT
                        'Else
                        '    v_strStatus = CStr(CLng(v_strStatus) + 1)
                        'End If

                        v_oProccess.StartProcessForm()
                        Dim v_strClause, v_strObjMsg As String
                        Dim v_xmlDocument As New Xml.XmlDocument
                        v_strClause = "select MSG_AMT from tltx where tltxcd = '" & IIf(v_strCHILDTLTXCD <> "", v_strCHILDTLTXCD, v_strTLTXCD) & "'"
                        v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLTX, gc_ActionInquiry, v_strClause)

                        v_lngError = Proxy.Message(v_strObjMsg)
                        If v_lngError <> ERR_SYSTEM_OK Then
                            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                            Exit Sub
                        End If

                        v_xmlDocument.LoadXml(v_strObjMsg)
                        If Not IsDBNull(v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='MSG_AMT']")) Then
                            v_strMSGAMT = v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='MSG_AMT']").InnerText
                        End If

                        'Tạo điện giao dịch
                        v_strTxMsg = BuildXMLTxMsg(gc_MsgTypeTrans, gc_IsLocalMsg, "9002", _
                                    Me.BranchId, v_strTLID, Me.IpAddress, Me.WsName, _
                                    v_strStatus, IIf(InStr(v_strTLTXCD, "44") > 0, "Y", "N") _
                                    , v_strOFFID, v_strCHKID, v_strCFRID, v_strTXDATE, , v_strTXNUM, , v_strReason, v_strMSGAMT, v_strMICODE, , , Me.UserLanguage, v_strBUSDATE, v_strSICODE, _
                                    , v_strISPARENT, v_strPARENTID, v_strBRCODE, v_strTLNAME, v_strOFFNAME, v_strCHKNAME, _
                                    v_strCFRNAME, v_strTXNAME, v_strSTATUS_TEXT, v_strPARENT_TEXT, v_strAUTOID, gc_MsgTypeTrans, v_strISBRID, v_strCOMICODE, v_strReason)
                        v_strInitialTxMsg = v_strTxMsg
                        ' 2. Accept transaction
                        While True
                            v_lngError = Proxy.Message(v_strTxMsg)
                            If v_lngError <> ERR_SYSTEM_OK Then
                                'ThÃ´ng bÃ¡o lá»—i
                                Dim frm As New frmErrMsg
                                v_xmlDocument.LoadXml(v_strTxMsg)
                                frm.ErrMsg = v_xmlDocument.DocumentElement("ERROR_MESSAGE").InnerText
                                frm.ErrCount = v_xmlDocument.DocumentElement("ERROR_MESSAGE").Attributes("COUNT").Value.ToString
                                frm.WarningMsg = v_xmlDocument.DocumentElement("WARNING_MESSAGE").InnerText
                                frm.WarningCount = v_xmlDocument.DocumentElement("WARNING_MESSAGE").Attributes("COUNT").Value.ToString
                                v_oProccess.StopProcessForm()
                                Application.DoEvents()
                                If frm.ShowDialog() = Windows.Forms.DialogResult.Cancel Then
                                    MyBase.ButtonEnable(.Cells("AUTH_STRING").Value)
                                    Exit Sub
                                Else
                                    v_strTxMsg = v_strInitialTxMsg.Replace(gc_AtributeMISSING_WARNING & "=""0""", gc_AtributeMISSING_WARNING & "=""1""")
                                    v_oProccess.StartProcessForm()
                                End If
                            Else
                                '' update screen 
                                '.Cells("STATUS").Value = CDec(v_strStatus)
                                '.Cells("STATUS_TEXT").Value = v_strSTATUS_TEXT
                                'Select Case v_strOldStatus
                                '    Case "0"
                                '        .Cells("CHKID").Value = v_strCHKID
                                '        .Cells("MEMBER_LEADER").Value = v_strCHKNAME
                                '    Case "1"
                                '        .Cells("OFFID").Value = v_strOFFID
                                '        .Cells("VSD_STAFF").Value = v_strOFFNAME
                                '    Case "2"
                                '        .Cells("CFRID").Value = v_strCFRID
                                '        .Cells("VSD_LEADER").Value = v_strCFRNAME
                                '        .Cells("BUSDATE").Value = v_strBUSDATE
                                'End Select
                                'If v_strStatus = "4" Then
                                '    .Cells("DETAIL_TEXT").Value = .Cells("DETAIL_TEXT").Value & "vbCrLf" & v_strReason
                                'End If
                                '.Cells("AUTH_STRING").Value = "YNNNY"
                                'MyBase.ButtonEnable(.Cells("AUTH_STRING").Value)
                                v_oProccess.StopProcessForm()
                                Application.DoEvents()
                                MessageBox.Show(IIf(Me.UserLanguage = "VN", "Xóa giao dịch thành công !", "This transaction is deleted successful !"))
                                Exit Sub
                            End If
                        End While
                    End With
                End If
            End If
        Catch ex As Exception
            v_oProccess.StopProcessForm()
            Application.DoEvents()
            Throw ex
        Finally
            v_oProccess.StopProcessForm()
            Application.DoEvents()
        End Try
    End Sub
    'end bangpv 
    'Private Sub ShowProcessForm()
    '    Dim v_frm As New frmProcess
    '    'v_frm.Caption = "Dữ liệu đang được xử lý ..."
    '    v_frm.Caption = "Dữ liệu đang được xử lý ..."
    '    v_frm.ShowDialog()
    'End Sub
    Private Function getDisplay(ByVal v_strKey As String) As String
        Dim v_intBegin, v_intEnd As Integer
        Try
            If InStr(v_strObjMsg, v_strKey) = 0 Then
                Return String.Empty
            Else
                v_intBegin = InStr(InStr(v_strObjMsg, v_strKey), v_strObjMsg, "oldval") + 7
                v_intEnd = InStr(v_intBegin + 1, v_strObjMsg, """") - 1

                Return v_strObjMsg.Substring(v_intBegin, v_intEnd - v_intBegin) & ": "
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function CheckExitsForm(ByVal v_strName) As Boolean
        Dim o_frm As Sats.WinFormsUI.Docking.DockContent
        Dim v_blnIsExits As Boolean = False

        For Each o_frm In Me.DockPanel.Documents
            If o_frm.Name = v_strName Then
                v_blnIsExits = True
                o_frm.Activate()
                Exit For
            End If
        Next
        Return v_blnIsExits
    End Function
    Private Sub ShowReport(ByVal pv_strRptId As String, ByVal pv_strAuthString As String, ByVal pv_strCtlVal As String)
        Dim v_oProcess As New Sats.AppCore.ProcessForm(Me)
        Try
            v_oProcess.StartProcessForm()
            If Not CheckExitsForm("frm" & pv_strRptId) Then
                Dim frm = New AppCore.frmRPParameter(m_BusLayer.mv_DataSet)
                frm.Name = "frm" & pv_strRptId
                frm.UserLanguage = m_BusLayer.AppLanguage
                frm.ObjectName = pv_strRptId
                frm.ModuleCode = ModuleCode
                frm.LocalObject = gc_IsInQueryNotLocalMsg
                frm.BranchId = m_BusLayer.CurrentTellerProfile.BranchId
                frm.TellerId = m_BusLayer.CurrentTellerProfile.TellerId
                frm.IpAddress = m_BusLayer.AppIpAddress
                frm.WsName = m_BusLayer.AppWsName
                frm.BusDate = m_BusLayer.CurrentTellerProfile.BusDate
                frm.MemberFilter = m_BusLayer.CurrentTellerProfile.MemberFilter
                frm.StockFilter = m_BusLayer.CurrentTellerProfile.StockFilter
                frm.AllMember = m_BusLayer.CurrentTellerProfile.AllMember
                frm.AllStock = m_BusLayer.CurrentTellerProfile.AllStock
                frm.BRCODE = m_BusLayer.CurrentTellerProfile.BranchName
                frm.TellerName = m_BusLayer.CurrentTellerProfile.TellerName
                Threading.Thread.Sleep(100)
                frm.Auth = pv_strAuthString
                frm.ObjectType = "R"
                frm.PassDate = m_BusLayer.CurrentTellerProfile.PassDate
                frm.Proxy = Proxy
                frm.Client = Client
                frm.Show(Me.DockPanel)
                frm.AssignCtlVal(pv_strCtlVal, "TXNUM")
            End If
            v_oProcess.StopProcessForm()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub


    Public Function GetSblFptBnkTllogInfo(ByVal pv_strTltxcd As String, ByVal pv_strTxNum As String, ByRef pv_strBrid As String, ByRef pv_strTradeDate As String, ByRef pv_strFileName As String) As String
        Try
            Dim v_oXmlDocument As New XmlDocumentEx
            Dim v_strSQL As String = "SELECT a.col_value01 brid, a.col_value02 tradedate, a.col_value04 filename FROM tllog a WHERE autoid = TO_NUMBER('" + pv_strTxNum + "')"
            Dim v_strObjMessage = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLTX, gc_ActionInquiry, v_strSQL)
            Dim v_Error As Long = Proxy.Message(v_strObjMessage)
            If v_Error <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Exit Function
            End If
            v_oXmlDocument.LoadXml(v_strObjMessage)
            Dim v_xmlNodeList As Xml.XmlNodeList

            v_xmlNodeList = v_oXmlDocument.SelectNodes("/ObjectMessage/ObjData")
            Dim v_strFldValue, v_strFldName As String
            For i = 0 To v_xmlNodeList.Count - 1
                For j = 0 To v_xmlNodeList.Item(i).ChildNodes.Count - 1
                    With v_xmlNodeList.Item(i).ChildNodes(j)
                        v_strFldValue = .InnerText.ToString
                        v_strFldName = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value).ToUpper

                        Select Case v_strFldName
                            Case "BRID"
                                pv_strBrid = v_strFldValue
                            Case "TRADEDATE"
                                pv_strTradeDate = v_strFldValue
                            Case "FILENAME"
                                pv_strFileName = v_strFldValue
                        End Select
                    End With
                Next
            Next
            Return pv_strFileName
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetTllogInfo1150(ByVal pv_strTltxcd As String, ByVal pv_strTxNum As String, ByRef pv_strBrid As String, ByRef pv_strTradeDate As String, ByRef pv_strCsExpType As String) As String
        Try
            Dim v_oXmlDocument As New XmlDocumentEx
            Dim v_strSQL As String = "SELECT a.col_value01 brid, a.col_value02 tradedate, a.col_value03 CsExpType FROM tllog a WHERE autoid = TO_NUMBER('" + pv_strTxNum + "')"
            Dim v_strObjMessage = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLTX, gc_ActionInquiry, v_strSQL)
            Dim v_Error As Long = Proxy.Message(v_strObjMessage)
            If v_Error <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Exit Function
            End If
            v_oXmlDocument.LoadXml(v_strObjMessage)
            Dim v_xmlNodeList As Xml.XmlNodeList

            v_xmlNodeList = v_oXmlDocument.SelectNodes("/ObjectMessage/ObjData")
            Dim v_strFldValue, v_strFldName As String
            For i = 0 To v_xmlNodeList.Count - 1
                For j = 0 To v_xmlNodeList.Item(i).ChildNodes.Count - 1
                    With v_xmlNodeList.Item(i).ChildNodes(j)
                        v_strFldValue = .InnerText.ToString
                        v_strFldName = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value).ToUpper

                        Select Case v_strFldName
                            Case "BRID"
                                pv_strBrid = v_strFldValue
                            Case "TRADEDATE"
                                pv_strTradeDate = v_strFldValue
                            Case "CSEXPTYPE"
                                pv_strCsExpType = v_strFldValue
                        End Select
                    End With
                Next
            Next
            Return pv_strCsExpType
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    'start Myvq
    Public Function GetBranchId(ByVal pv_strTXNum As String) As String
        'start Myvq
        Dim v_oXmlDocument As New XmlDocumentEx
        'Lấy thông số FTP
        Dim v_strSQL As String = "SELECT (case when a.tltxcd ='1131' then a.brid || to_char(a.col_value04) else to_char(a.COL_VALUE01) end) VARNAME,(case when a.tltxcd ='1131' then a.brid || to_char(a.col_value04) else to_char(a.COL_VALUE01) end) VARVALUE from tllog a where  txnum = '" + pv_strTXNum + "'"
        Dim v_strObjMessage = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLTX, gc_ActionInquiry, v_strSQL)
        Dim v_Error As Long = Proxy.Message(v_strObjMessage)
        If v_Error <> ERR_SYSTEM_OK Then
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
            Exit Function
        End If
        v_oXmlDocument.LoadXml(v_strObjMessage)
        Dim v_xmlNodeList As Xml.XmlNodeList

        v_xmlNodeList = v_oXmlDocument.SelectNodes("/ObjectMessage/ObjData")
        Dim v_strValue, v_strNAME As String
        Dim v_strVarName, v_strVarValue As String

        For i = 0 To v_xmlNodeList.Count - 1
            For j = 0 To v_xmlNodeList.Item(i).ChildNodes.Count - 1
                With v_xmlNodeList.Item(i).ChildNodes(j)
                    v_strValue = .InnerText.ToString
                    v_strNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)

                    Select Case v_strNAME
                        Case "VARNAME"
                            v_strVarName = v_strValue
                        Case "VARVALUE"
                            v_strVarValue = v_strValue
                            Return v_strVarValue
                    End Select
                End With
            Next
        Next
        'end Myvq
    End Function
    Public Function GetCurrDate(ByVal pv_strBrid As String) As String
        'start Myvq
        Dim v_oXmlDocument As New XmlDocumentEx
        'Lấy thông số FTP
        Dim v_strSQL As String = "select * from sysvar where varname = 'CURRDATE' AND brid = '" + pv_strBrid + "'"
        Dim v_strObjMessage = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLTX, gc_ActionInquiry, v_strSQL)
        Dim v_Error As Long = Proxy.Message(v_strObjMessage)
        If v_Error <> ERR_SYSTEM_OK Then
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
            Exit Function
        End If
        v_oXmlDocument.LoadXml(v_strObjMessage)
        Dim v_xmlNodeList As Xml.XmlNodeList

        v_xmlNodeList = v_oXmlDocument.SelectNodes("/ObjectMessage/ObjData")
        Dim v_strValue, v_strNAME As String
        Dim v_strVarName, v_strVarValue As String

        For i = 0 To v_xmlNodeList.Count - 1
            For j = 0 To v_xmlNodeList.Item(i).ChildNodes.Count - 1
                With v_xmlNodeList.Item(i).ChildNodes(j)
                    v_strValue = .InnerText.ToString
                    v_strNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)

                    Select Case v_strNAME
                        Case "VARNAME"
                            v_strVarName = v_strValue
                        Case "VARVALUE"
                            v_strVarValue = v_strValue
                            Return v_strVarValue
                    End Select
                End With
            Next
        Next
        'end Myvq
    End Function

    Public Function GetDate1114(ByVal pv_strTXNum As String) As String
        'start Myvq
        Dim v_oXmlDocument As New XmlDocumentEx
        'Lấy thông số FTP
        Dim v_strSQL As String = "SELECT a.COL_VALUE02 VARNAME, a.COL_VALUE02 VARVALUE from tllog a where  txnum = '" + pv_strTXNum + "'"
        Dim v_strObjMessage = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLTX, gc_ActionInquiry, v_strSQL)
        Dim v_Error As Long = Proxy.Message(v_strObjMessage)
        If v_Error <> ERR_SYSTEM_OK Then
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
            Exit Function
        End If
        v_oXmlDocument.LoadXml(v_strObjMessage)
        Dim v_xmlNodeList As Xml.XmlNodeList

        v_xmlNodeList = v_oXmlDocument.SelectNodes("/ObjectMessage/ObjData")
        Dim v_strValue, v_strNAME As String
        Dim v_strVarName, v_strVarValue As String

        For i = 0 To v_xmlNodeList.Count - 1
            For j = 0 To v_xmlNodeList.Item(i).ChildNodes.Count - 1
                With v_xmlNodeList.Item(i).ChildNodes(j)
                    v_strValue = .InnerText.ToString
                    v_strNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)

                    Select Case v_strNAME
                        Case "VARNAME"
                            v_strVarName = v_strValue
                        Case "VARVALUE"
                            v_strVarValue = v_strValue
                            Return v_strVarValue
                    End Select
                End With
            Next
        Next
        'end Myvq
    End Function

    Private Sub GetFileSblFtpBnk(ByVal pv_strGrName As String, ByVal pv_strBrid As String, ByVal pv_strRemoteFileName As String, ByVal pv_strLocalFileName As String)
        Try
            Dim ServerAddress, ServerAddress1, ServerPort, Username, Password, RemotePath As String
            Dim v_strSQL As String
            Dim v_strObjMsg As String

            Dim v_xmlDocument As New XmlDocumentEx
            'Lấy thông số FTP
            v_strSQL = "SELECT a.VARNAME, a.VARVALUE FROM sysvar a where a.GRNAME='" & pv_strGrName & "' and brid ='" & pv_strBrid & "'"
            v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLTX, gc_ActionInquiry, v_strSQL)
            Dim v_lngError As Long = Proxy.Message(v_strObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Exit Sub
            End If
            v_xmlDocument.LoadXml(v_strObjMsg)
            Dim v_nodeList As Xml.XmlNodeList

            v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")
            Dim v_strValue, v_strFLDNAME As String
            Dim v_strVarName, v_strVarValue As String

            For i = 0 To v_nodeList.Count - 1
                For j = 0 To v_nodeList.Item(i).ChildNodes.Count - 1
                    With v_nodeList.Item(i).ChildNodes(j)
                        v_strValue = .InnerText.ToString
                        v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)

                        Select Case v_strFLDNAME
                            Case "VARNAME"
                                v_strVarName = v_strValue
                            Case "VARVALUE"
                                v_strVarValue = v_strValue
                        End Select
                    End With
                Next
                Select Case Trim(v_strVarName)
                    Case "ServerAddress"
                        ServerAddress = v_strVarValue
                    Case "ServerAddress1"
                        ServerAddress1 = v_strVarValue
                    Case "ServerPort"
                        ServerPort = v_strVarValue
                    Case "Username"
                        Username = v_strVarValue
                    Case "Password"
                        Password = v_strVarValue
                    Case "RemotePath"
                        RemotePath = v_strVarValue
                End Select
            Next

            'Dim v_ftpClient As New Sats.Utils.FtpClient(ServerAddress, Username, Password)
            Dim v_ftpClient As New Sats.Utils.FtpClient
            Try
                v_ftpClient.Server = ServerAddress
                v_ftpClient.Username = Username
                v_ftpClient.Password = Password
                v_ftpClient.Login()
            Catch ex As Sats.Utils.FtpClient.FtpException
                v_ftpClient.Server = ServerAddress1
                v_ftpClient.Username = Username
                v_ftpClient.Password = Password
                v_ftpClient.Login()
            End Try
            v_ftpClient.ChangeDir(RemotePath)
            v_ftpClient.Download(pv_strRemoteFileName, pv_strLocalFileName, True)
            v_ftpClient.Close()
            v_ftpClient.Dispose()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub GetFTP(ByVal v_ServerAddress As String, ByVal v_ServerPort As String, _
        ByVal v_Username As String, ByVal v_Password As String, _
        ByVal v_RemotePath As String, ByVal v_strClientDir As String, _
        ByVal v_strFileDownload As String, ByVal v_strBrid As String)
        If Not (Directory.Exists(v_strClientDir)) Then
            Directory.CreateDirectory(v_strClientDir)
        End If
        Dim v_xmlDocument As New XmlDocumentEx
        Dim v_ServerAddress1 As String
        'Lấy thông số FTP
        Dim v_strSQL = "SELECT a.VARNAME, a.VARVALUE FROM sysvar a where a.GRNAME='VSDFTPSVR' and brid ='" & v_strBrid & "'"
        v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLTX, gc_ActionInquiry, v_strSQL)
        Dim v_lngError As Long = Proxy.Message(v_strObjMsg)
        If v_lngError <> ERR_SYSTEM_OK Then
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
            Exit Sub
        End If
        v_xmlDocument.LoadXml(v_strObjMsg)
        Dim v_nodeList As Xml.XmlNodeList

        v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")
        Dim v_strValue, v_strFLDNAME As String
        Dim v_strVarName, v_strVarValue As String

        For i = 0 To v_nodeList.Count - 1
            For j = 0 To v_nodeList.Item(i).ChildNodes.Count - 1
                With v_nodeList.Item(i).ChildNodes(j)
                    v_strValue = .InnerText.ToString
                    v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)

                    Select Case v_strFLDNAME
                        Case "VARNAME"
                            v_strVarName = v_strValue
                        Case "VARVALUE"
                            v_strVarValue = v_strValue
                    End Select
                End With
            Next
            Select Case Trim(v_strVarName)
                Case "ServerAddress"
                    v_ServerAddress = v_strVarValue
                Case "ServerAddress1"
                    v_ServerAddress1 = v_strVarValue
                Case "ServerPort"
                    v_ServerPort = v_strVarValue
                Case "Username"
                    v_Username = v_strVarValue
                Case "Password"
                    v_Password = v_strVarValue
                Case "RemotePath"
                    v_RemotePath = v_strVarValue
            End Select
        Next
        Dim v_oWriter = New StreamWriter(v_strClientDir & "\" & v_strFileDownload & ".bat")
        'bangp_add_run_on_x64system
        v_oWriter.WriteLine("set " & """" & "SystemPath=%windir%\System32" & """")
        v_oWriter.WriteLine("if not " & """" & "%ProgramFiles(x86)%" & """" & "==" & """""" _
                            & " set " & """" & "SystemPath=%windir%\SysWOW64" & """")
        'end bangp
        v_oWriter.WriteLine("@ftp -i -s:""%~f0""&GOTO:EOF")
       
        v_oWriter.WriteLine("open " & v_ServerAddress)
        v_oWriter.WriteLine(v_Username)
        v_oWriter.WriteLine(v_Password)
        v_oWriter.WriteLine("lcd " & """" & v_strClientDir & """")
        v_oWriter.WriteLine("cd " & v_RemotePath)
        v_oWriter.WriteLine("binary")

        v_oWriter.WriteLine("get " & v_strFileDownload & " " & v_strFileDownload)

        v_oWriter.WriteLine("bye" & vbCrLf)
        v_oWriter.Close()
        Dim v_oProcess As Process
        v_oProcess = New Process

        v_oProcess.StartInfo.FileName = v_strClientDir & "\" & v_strFileDownload & ".bat"
        v_oProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
        v_oProcess.StartInfo.CreateNoWindow = True
        v_oProcess.Start()
        'v_oProcess.WaitForExit()
        ' System.Threading.Thread.Sleep(30 * 1000)
        v_oProcess.Close()
        'Nhận từ đầu 2

        v_oWriter = New StreamWriter(v_strClientDir & "\" & v_strFileDownload & "1.bat")

        'bangp_add_run_on_x64system
        v_oWriter.WriteLine("set " & """" & "SystemPath=%windir%\System32" & """")
        v_oWriter.WriteLine("if not " & """" & "%ProgramFiles(x86)%" & """" & "==" & """""" _
                            & " set " & """" & "SystemPath=%windir%\SysWOW64" & """")
        'end bangp
        v_oWriter.WriteLine("@ftp -i -s:""%~f0""&GOTO:EOF")
       
        v_oWriter.WriteLine("open " & v_ServerAddress1)
        v_oWriter.WriteLine(v_Username)
        v_oWriter.WriteLine(v_Password)
        v_oWriter.WriteLine("lcd " & """" & v_strClientDir & """")
        v_oWriter.WriteLine("cd " & v_RemotePath)
        v_oWriter.WriteLine("binary")

        v_oWriter.WriteLine("get " & v_strFileDownload & " " & v_strFileDownload)

        v_oWriter.WriteLine("bye" & vbCrLf)
        v_oWriter.Close()

        v_oProcess = New Process

        v_oProcess.StartInfo.FileName = v_strClientDir & "\" & v_strFileDownload & "1.bat"
        v_oProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
        v_oProcess.StartInfo.CreateNoWindow = True
        v_oProcess.Start()
        'v_oProcess.WaitForExit()
        'System.Threading.Thread.Sleep(30 * 1000)
        v_oProcess.Close()
        System.Threading.Thread.Sleep(30 * 1000)
    End Sub
    'Dung cho SGD NHNN
    Public Sub GetFTPSBV(ByVal v_ServerAddress As String, ByVal v_ServerPort As String, _
        ByVal v_Username As String, ByVal v_Password As String, _
        ByVal v_RemotePath As String, ByVal v_strClientDir As String, _
        ByVal v_strFileDownload As String, ByVal v_strBrid As String)
        If Not (Directory.Exists(v_strClientDir)) Then
            Directory.CreateDirectory(v_strClientDir)
        End If
        Dim v_xmlDocument As New XmlDocumentEx
        Dim v_ServerAddress1 As String
        'Lấy thông số FTP
        Dim v_strSQL = "SELECT a.VARNAME, a.VARVALUE FROM sysvar a where a.GRNAME='VSDFTPSBV' and brid ='" & v_strBrid & "'"
        v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLTX, gc_ActionInquiry, v_strSQL)
        Dim v_lngError As Long = Proxy.Message(v_strObjMsg)
        If v_lngError <> ERR_SYSTEM_OK Then
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
            Exit Sub
        End If
        v_xmlDocument.LoadXml(v_strObjMsg)
        Dim v_nodeList As Xml.XmlNodeList

        v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")
        Dim v_strValue, v_strFLDNAME As String
        Dim v_strVarName, v_strVarValue As String

        For i = 0 To v_nodeList.Count - 1
            For j = 0 To v_nodeList.Item(i).ChildNodes.Count - 1
                With v_nodeList.Item(i).ChildNodes(j)
                    v_strValue = .InnerText.ToString
                    v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)

                    Select Case v_strFLDNAME
                        Case "VARNAME"
                            v_strVarName = v_strValue
                        Case "VARVALUE"
                            v_strVarValue = v_strValue
                    End Select
                End With
            Next
            Select Case Trim(v_strVarName)
                Case "ServerAddress"
                    v_ServerAddress = v_strVarValue
                Case "ServerAddress1"
                    v_ServerAddress1 = v_strVarValue
                Case "ServerPort"
                    v_ServerPort = v_strVarValue
                Case "Username"
                    v_Username = v_strVarValue
                Case "Password"
                    v_Password = v_strVarValue
                Case "RemotePath"
                    v_RemotePath = v_strVarValue
            End Select
        Next
        Dim v_oWriter = New StreamWriter(v_strClientDir & "\" & v_strFileDownload & ".bat")

        'bangp_add_run_on_x64system
        v_oWriter.WriteLine("set " & """" & "SystemPath=%windir%\System32" & """")
        v_oWriter.WriteLine("if not " & """" & "%ProgramFiles(x86)%" & """" & "==" & """""" _
                            & " set " & """" & "SystemPath=%windir%\SysWOW64" & """")
        'end bangp
        v_oWriter.WriteLine("@ftp -i -s:""%~f0""&GOTO:EOF")
        v_oWriter.WriteLine("open " & v_ServerAddress)
        v_oWriter.WriteLine(v_Username)
        v_oWriter.WriteLine(v_Password)
        v_oWriter.WriteLine("lcd " & """" & v_strClientDir & """")
        v_oWriter.WriteLine("cd " & v_RemotePath)
        v_oWriter.WriteLine("binary")

        v_oWriter.WriteLine("get " & v_strFileDownload & " " & v_strFileDownload)

        v_oWriter.WriteLine("bye" & vbCrLf)
        v_oWriter.Close()
        Dim v_oProcess As Process
        v_oProcess = New Process

        v_oProcess.StartInfo.FileName = v_strClientDir & "\" & v_strFileDownload & ".bat"
        v_oProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
        v_oProcess.StartInfo.CreateNoWindow = True
        v_oProcess.Start()
        'System.Threading.Thread.Sleep(30 * 1000)
        'v_oProcess.WaitForExit()
        v_oProcess.Close()
        'Nhận từ đầu 2

        v_oWriter = New StreamWriter(v_strClientDir & "\" & v_strFileDownload & "1.bat")
        'bangp_add_run_on_x64system
        v_oWriter.WriteLine("set " & """" & "SystemPath=%windir%\System32" & """")
        v_oWriter.WriteLine("if not " & """" & "%ProgramFiles(x86)%" & """" & "==" & """""" _
                            & " set " & """" & "SystemPath=%windir%\SysWOW64" & """")
        'end bangp
        v_oWriter.WriteLine("@ftp -i -s:""%~f0""&GOTO:EOF")
        v_oWriter.WriteLine("open " & v_ServerAddress1)
        v_oWriter.WriteLine(v_Username)
        v_oWriter.WriteLine(v_Password)
        v_oWriter.WriteLine("lcd " & """" & v_strClientDir & """")
        v_oWriter.WriteLine("cd " & v_RemotePath)
        v_oWriter.WriteLine("binary")

        v_oWriter.WriteLine("get " & v_strFileDownload & " " & v_strFileDownload)

        v_oWriter.WriteLine("bye" & vbCrLf)
        v_oWriter.Close()

        v_oProcess = New Process

        v_oProcess.StartInfo.FileName = v_strClientDir & "\" & v_strFileDownload & "1.bat"
        v_oProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
        v_oProcess.StartInfo.CreateNoWindow = True
        v_oProcess.Start()
        'System.Threading.Thread.Sleep(30 * 1000)
        'v_oProcess.WaitForExit()
        v_oProcess.Close()
        System.Threading.Thread.Sleep(10 * 1000)
    End Sub

    Private Function GetExportFilePath() As String
        Try
            Dim v_strSQL As String = ""
            Dim v_xmlDocument As New Xml.XmlDocument
            Dim v_nodeList As Xml.XmlNodeList
            Dim v_strObjMsg As String
            Dim v_strFLDNAME, v_strValue, v_strFilePath As String

            v_strSQL = "SELECT VARVALUE, VARDESC, EN_VARDESC FROM SYSVAR WHERE TRIM(GRNAME) = 'SYSTEM' AND TRIM(VARNAME)='FILE_EXPORT_PATH1112' AND DELETED = 0 AND STATUS = 0"

            v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLGROUPS, gc_ActionInquiry, v_strSQL)
            Dim v_lngError As Long = Proxy.Message(v_strObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Return String.Empty
            End If

            v_xmlDocument.LoadXml(v_strObjMsg)
            v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")
            For i As Integer = 0 To v_nodeList.Count - 1
                For j As Integer = 0 To v_nodeList.Item(i).ChildNodes.Count - 1
                    With v_nodeList.Item(i).ChildNodes(j)
                        v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                        v_strValue = .InnerText.ToString
                    End With
                    Select Case Trim(v_strFLDNAME)
                        Case "VARVALUE"
                            v_strFilePath = v_strValue
                    End Select
                Next
            Next
            Return v_strFilePath
        Catch ex As Exception
            Throw ex
            Return ""
        End Try
    End Function
    'end Myvq
#End Region
    'Comment
End Class