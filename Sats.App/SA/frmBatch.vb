Imports Sats.CommonLibrary
Imports Sats.AppCore.ProcessForm

Public Class frmBatch
    Private mv_ResourceManager As Resources.ResourceManager
    ' Private mv_BDSDelivery As New BDSChannel.BDSDelivery

    'Khai bao thuoc tinh Form
    Private mv_strBranchId As String
    Private mv_strLanguage As String
    Private mv_strTellerId As String
    Private mv_strBEGINOREND As String
    Private mv_strBusDate As String = String.Empty
    Private mv_blnOnInit As Boolean = False
    Protected v_oProcess As AppCore.ProcessForm
    Private mv_oProxy As BDSChannel.BDSDelivery

    Public Property Proxy() As BDSChannel.BDSDelivery
        Get
            Return mv_oProxy
        End Get
        Set(ByVal value As BDSChannel.BDSDelivery)
            mv_oProxy = value
        End Set
    End Property

#Region "Khai bao cac thuoc tinh"
    Public Property BusDate() As String
        Get
            Return mv_strBusDate
        End Get
        Set(ByVal Value As String)
            mv_strBusDate = Value
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

    Public Property UserLanguage() As String
        Get
            Return mv_strLanguage
        End Get
        Set(ByVal Value As String)
            mv_strLanguage = Value
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
    Public Property BEGINOREND() As String
        Get
            Return mv_strBEGINOREND
        End Get
        Set(ByVal Value As String)
            mv_strBEGINOREND = Value
        End Set
    End Property
#End Region

    Private Sub frmBatch_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        OnInit()
    End Sub
    Private Sub OnInit()
        mv_ResourceManager = New Resources.ResourceManager(gc_RootNamespace & ".frmBatch_" & UserLanguage, System.Reflection.Assembly.GetExecutingAssembly())
        LoadUserInterface(Me)
        'bangpv: Load form len lay dung format datetime
        System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("vi-VN")
        'end bangpv
        FillDataCombobox()
        FillDate()
        mv_blnOnInit = True
    End Sub
    Private Sub LoadUserInterface(ByRef pv_ctrl As Windows.Forms.Control)
        Dim v_ctrl As Windows.Forms.Control
        Try
            For Each v_ctrl In pv_ctrl.Controls
                If TypeOf (v_ctrl) Is Panel Then
                    LoadUserInterface(v_ctrl)
                ElseIf TypeOf (v_ctrl) Is GroupBox Then
                    CType(v_ctrl, GroupBox).Text = mv_ResourceManager.GetString(v_ctrl.Tag)
                    LoadUserInterface(v_ctrl)
                ElseIf TypeOf (v_ctrl) Is Label Then
                    CType(v_ctrl, Label).Text = mv_ResourceManager.GetString(v_ctrl.Tag)
                ElseIf TypeOf (v_ctrl) Is Button Then
                    CType(v_ctrl, Button).Text = mv_ResourceManager.GetString(v_ctrl.Tag)
                ElseIf TypeOf (v_ctrl) Is TabControl Then
                    For Each v_ctrlTmp As Control In CType(v_ctrl, TabControl).TabPages
                        CType(v_ctrlTmp, TabPage).Text = mv_ResourceManager.GetString(v_ctrlTmp.Tag)
                        LoadUserInterface(v_ctrlTmp)
                    Next
                ElseIf TypeOf (v_ctrl) Is TabPage Then
                    v_ctrl.BackColor = System.Drawing.SystemColors.InactiveCaptionText
                    CType(v_ctrl, TabPage).Text = mv_ResourceManager.GetString(v_ctrl.Tag)
                    LoadUserInterface(v_ctrl)
                End If
            Next

            'Load caption của form, label caption
            Me.Text = mv_ResourceManager.GetString("frmBatch_" & BEGINOREND)
            lbCaption.Text = mv_ResourceManager.GetString("lbCaption_" & BEGINOREND)
            lblCurrDate.Text = mv_ResourceManager.GetString("lblCurrDate_" & BEGINOREND)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub FillDataCombobox()
        Try
            Dim v_xmlDocument As New Xml.XmlDocument
            Dim v_nodeList As Xml.XmlNodeList = Nothing
            Dim v_strValue As String = "", v_strFLDNAME As String = ""
            Dim v_strCDVAL As String = "", v_strCDCONTENT As String = ""

            Dim v_strObjMsg As String
            Dim strRow As String = "SELECT BRID VALUE, BRID || ' - ' || BRNAME DISPLAY FROM BRGRP WHERE STATUS = 0 AND DELETED = 0" _
            & " and ( (brid in (SELECT distinct a.brid" _
            & " FROM tlbridauth a" _
            & " where a.deleted = 0 and a.status =0 and " _
            & " ((a.authtype = 'G' and a.authid in (select grpid from tlgrpusers where tlid = '" & Me.TellerId & "')) " _
            & " or (a.authtype= 'U' and a.authid = '" & Me.TellerId & "')) ) ) or BRID = '" & Me.BranchId & "')" _
            & " ORDER BY BRID"
            v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLGROUPS, gc_ActionInquiry, strRow)
            Dim v_lngError As Long = Proxy.Message(v_strObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Exit Sub
            End If

            v_xmlDocument.LoadXml(v_strObjMsg)
            v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")

            For i As Integer = 0 To v_nodeList.Count - 1
                For j As Integer = 0 To v_nodeList.Item(i).ChildNodes.Count - 1
                    With v_nodeList.Item(i).ChildNodes(j)
                        v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                        v_strValue = .InnerText.ToString()
                    End With
                    Select Case Trim(v_strFLDNAME)
                        Case "VALUE"
                            v_strCDVAL = Trim(v_strValue)
                        Case "DISPLAY"
                            v_strCDCONTENT = Trim(v_strValue)
                    End Select
                Next
                cboBranchId.AddItems(v_strCDCONTENT, v_strCDVAL)
            Next
        Catch ex As Exception
            MsgBox(mv_ResourceManager.GetString("Faild"), MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, Me.Text)
        End Try
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        OnSubmit()
    End Sub
    Private Sub OnSubmit()

        Dim v_strObjMsg As String
        Dim v_strObjName, v_strModCode, v_strAuthCode, v_strAuthString As String
        Dim v_strCheckEffectiveDate, v_strDueDate As String
        Dim blnNotConfirmedTaskes As Boolean = False
        Dim v_nodeList As Xml.XmlNodeList
        Dim v_strClause As String
        Dim v_xmlDocument As New Xml.XmlDocument
        Dim v_lngError As Long
        Dim v_xmlDocument_DueDate As New Xml.XmlDocument
        Dim v_strObjMsg_DueDate As String
        Dim v_lngError_DueDate As Long

        Try
            If MsgBox("Bạn đã chắc chắn muốn " & IIf(BEGINOREND = "BEGINOFDAY", "bắt đầu", "kết thúc") & " ngày không ?", MsgBoxStyle.Information + MsgBoxStyle.YesNo, "Thông báo") = MsgBoxResult.No Then
                Exit Sub
            End If
            If BEGINOREND = "BEGINOFDAY" Then
                v_strClause = "SELECT a.VARVALUE FROM sysvar a where a.GRNAME='SYSTEM' and a.VARNAME= 'BRSTATUS' and BRID='" & cboBranchId.SelectedValue.ToString & "'"
                v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLTX, gc_ActionInquiry, v_strClause)
                v_lngError = Proxy.Message(v_strObjMsg)
                If v_lngError <> ERR_SYSTEM_OK Then
                    MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                    Exit Sub
                End If
                v_xmlDocument.LoadXml(v_strObjMsg)
                If v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='VARVALUE']").InnerText <> OPERATION_INACTIVE Then
                    MsgBox(mv_ResourceManager.GetString("ErrorMsg_" & BEGINOREND), MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, mv_ResourceManager.GetString("Title"))
                    Exit Sub
                End If
                'Chi app dung voi san Cacbon
                If cboBranchId.SelectedValue.ToString = "0009" Then
                    'check Thong bao ma HN, TC den ngay giao dich
                    v_strCheckEffectiveDate = "SELECT LISTAGG(SICODE, ',') WITHIN GROUP (ORDER BY SICODE) AS SICODE_LIST FROM RGSI A WHERE STOCK_TYPE in ('1', '2') AND a.type = 5 and A.brid = '" & cboBranchId.SelectedValue.ToString & "' AND A.DELETED =0 AND A.STATUS =0 " _
                    & "AND( A.EFFECTIVE_DATE = get_t_plus(to_date('" & dtpCurrDate.Value & "','DD/MM/YYYY'),'" & cboBranchId.SelectedValue.ToString & "',0))"
                    v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLTX, gc_ActionInquiry, v_strCheckEffectiveDate)
                    v_lngError = Proxy.Message(v_strObjMsg)
                    If v_lngError <> ERR_SYSTEM_OK Then
                        MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                        Exit Sub
                    End If
                    v_xmlDocument.LoadXml(v_strObjMsg)

                    'check Thong bao ma HN, TC den ngay huy
                    v_strDueDate = "SELECT LISTAGG(SICODE, ',') WITHIN GROUP (ORDER BY SICODE) AS SICODE_LIST FROM RGSI A WHERE STOCK_TYPE in ('1', '2') AND a.type = 5 and A.brid = '" & cboBranchId.SelectedValue.ToString & "' AND A.DELETED =0 AND A.STATUS =0 " _
                    & "AND( A.due_date = get_t_plus(to_date('" & dtpCurrDate.Value & "','DD/MM/YYYY'),'" & cboBranchId.SelectedValue.ToString & "',0))"
                    v_strObjMsg_DueDate = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLTX, gc_ActionInquiry, v_strDueDate)
                    v_lngError_DueDate = Proxy.Message(v_strObjMsg_DueDate)
                    If v_lngError_DueDate <> ERR_SYSTEM_OK Then
                        MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                        Exit Sub
                    End If
                    v_xmlDocument_DueDate.LoadXml(v_strObjMsg_DueDate)

                    'Check Thong bao ma HN, TC den ngay giao dich va ngay huy
                    Dim v_sicodeList_EffectDate As String = Trim(v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='SICODE_LIST']").InnerText)
                    Dim v_sicodeList_DueDate As String = Trim(v_xmlDocument_DueDate.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='SICODE_LIST']").InnerText)

                    Dim v_message As String = ""

                    If Len(v_sicodeList_EffectDate) > 0 Then
                        v_message &= "Danh sách mã hạn ngạch, tín chỉ: '" & v_sicodeList_EffectDate & "' đến ngày giao dịch !" & vbCrLf
                    End If

                    If Len(v_sicodeList_DueDate) > 0 Then
                        v_message &= "Danh sách mã hạn ngạch, tín chỉ: '" & v_sicodeList_DueDate & "' đến ngày hủy !"
                    End If

                    If Len(Trim(v_message)) > 0 Then
                        MsgBox(v_message, MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, mv_ResourceManager.GetString("Title"))
                    End If
                End If

                ' check batch data
                v_strClause = "SELECT a.VARVALUE FROM sysvar a where a.GRNAME='SYSTEM' and a.VARNAME= 'DATA_BATCH' and BRID='" & cboBranchId.SelectedValue.ToString & "'"
                v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLTX, gc_ActionInquiry, v_strClause)
                v_lngError = Proxy.Message(v_strObjMsg)
                If v_lngError <> ERR_SYSTEM_OK Then
                    MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                    Exit Sub
                End If
                v_xmlDocument.LoadXml(v_strObjMsg)
                If v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='VARVALUE']").InnerText <> "1" Then
                    MsgBox("Chạy batch chưa tác động đến dữ liệu. Vui lòng kiểm tra lại !", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, mv_ResourceManager.GetString("Title"))
                    Exit Sub
                End If
                'BangPV
                'Bỏ check chạy Job tự động
                ' check balance job
                'v_strClause = "SELECT a.VARVALUE FROM sysvar a where a.GRNAME='SYSTEM' and a.VARNAME= 'BALANCE_JOB' and BRID='" & cboBranchId.SelectedValue.ToString & "'"
                'v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLTX, gc_ActionInquiry, v_strClause)
                'v_lngError = Proxy.Message(v_strObjMsg)
                'If v_lngError <> ERR_SYSTEM_OK Then
                '    MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                '    Exit Sub
                'End If
                'v_xmlDocument.LoadXml(v_strObjMsg)
                'If v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='VARVALUE']").InnerText <> "0" Then
                '    MsgBox("Lịch đẩy dữ liệu NĐT, TVLK chạy chưa thành công. Vui lòng kiểm tra lại", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, mv_ResourceManager.GetString("Title"))
                '    Exit Sub
                'End If
                'end BangPV
                'check room status 
                v_strClause = "SELECT a.VARVALUE FROM sysvar a where a.GRNAME='SYSTEM' and a.VARNAME= 'room_status' and BRID='" & cboBranchId.SelectedValue.ToString & "'"
                v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLTX, gc_ActionInquiry, v_strClause)
                v_lngError = Proxy.Message(v_strObjMsg)
                If v_lngError <> ERR_SYSTEM_OK Then
                    MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                    Exit Sub
                End If
                v_xmlDocument.LoadXml(v_strObjMsg)
                If v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='VARVALUE']").InnerText = "2" Then
                    MsgBox("Chưa thực hiện kết xuất Room nên không thể bắt đầu ngày giao dịch mới", MsgBoxStyle.OkOnly)
                    Exit Sub
                End If
            Else
                v_strClause = "SELECT a.VARVALUE FROM sysvar a where a.GRNAME='SYSTEM' and a.VARNAME= 'BRSTATUS' and BRID='" & cboBranchId.SelectedValue.ToString & "'"
                v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLTX, gc_ActionInquiry, v_strClause)
                v_lngError = Proxy.Message(v_strObjMsg)
                If v_lngError <> ERR_SYSTEM_OK Then
                    MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                    Exit Sub
                End If
                v_xmlDocument.LoadXml(v_strObjMsg)
                If v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='VARVALUE']").InnerText <> OPERATION_ACTIVE Then
                    MsgBox(mv_ResourceManager.GetString("ErrorMsg_" & BEGINOREND), MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, mv_ResourceManager.GetString("Title"))
                    Exit Sub
                End If
                'bangpv 

                If cboBranchId.SelectedValue.ToString = "0009" Then
                    'check chua nhan duoc dien SR=35
                    Dim v_strClause_SR35Check = "SELECT TO_CHAR(count(*)) AS CNT  FROM tllog a WHERE a.tltxcd = '4248'" _
                    & " and a.brid = '" & cboBranchId.SelectedValue.ToString & "' and a.deleted = 0 and a.status = 3" _
                    & " and TO_CHAR(txdate, 'DD/MM/YYYY') = '" & dtpCurrDate.Value.ToString("dd/MM/yyyy") & "'"
                    Dim v_strObjMsg_SR35Check = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLTX, gc_ActionInquiry, v_strClause_SR35Check)
                    v_lngError = Proxy.Message(v_strObjMsg_SR35Check)
                    If v_lngError <> ERR_SYSTEM_OK Then
                        MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                        Exit Sub
                    End If
                    Dim v_xmlDocument_SR35Check As New Xml.XmlDocument
                    v_xmlDocument_SR35Check.LoadXml(v_strObjMsg_SR35Check)

                    'Check chua hoan tat thanh toan trong ngay
                    Dim v_strClause_PaymentCheck = "SELECT COUNT(*) AS CNT FROM tllog WHERE tltxcd in ('4210') and status in (1,2)" _
                    & " and BRID ='" & cboBranchId.SelectedValue.ToString & "' and TRUNC(TO_DATE(col_value01,'DD/MM/YYYY')) = TO_DATE('" & dtpCurrDate.Value & "','DD/MM/YYYY')"
                    Dim v_strObjMsg_PaymentCheck = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLTX, gc_ActionInquiry, v_strClause_PaymentCheck)
                    v_lngError = Proxy.Message(v_strObjMsg_PaymentCheck)
                    If v_lngError <> ERR_SYSTEM_OK Then
                        MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                        Exit Sub
                    End If
                    Dim v_xmlDocument_PaymentCheck As New Xml.XmlDocument
                    v_xmlDocument_PaymentCheck.LoadXml(v_strObjMsg_PaymentCheck)

                    'Check chặn nếu Chưa kết xuất dữ liệu cuối ngày bắt buộc cho DCC (Báo cáo tổng hợp kết quả thanh toán giao dịch, Danh sách số dư sở hữu hạn ngạch, tín chỉ các-bon trên các tài khoản lưu ký, Gửi danh sách tài khoản lưu ký)
                    Dim v_strClause_DCCReportsChk = "SELECT COUNT(DISTINCT a.tltxcd) AS CNT FROM tllog a WHERE TO_CHAR(TRUNC(a.txdate),'dd/mm/yyyy') = '" & dtpCurrDate.Value & "'" _
                    & " AND a.brid = '" & cboBranchId.SelectedValue.ToString & "' AND a.deleted = 0 AND a.status = 3 AND a.tltxcd IN ('4295','4296','4297','4298')"
                    Dim v_strObjMsg_DCCReportsChk = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLTX, gc_ActionInquiry, v_strClause_DCCReportsChk)
                    v_lngError = Proxy.Message(v_strObjMsg_DCCReportsChk)
                    If v_lngError <> ERR_SYSTEM_OK Then
                        MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                        Exit Sub
                    End If
                    Dim v_xmlDocument_DCCReportsChk As New Xml.XmlDocument
                    v_xmlDocument_DCCReportsChk.LoadXml(v_strObjMsg_DCCReportsChk)

                    'Check chặn nếu Có mã TC, HN phải hủy trong ngày nhưng chưa được thực hiện
                    Dim v_strClause_HNTCCancelCheck = "SELECT LISTAGG(SICODE, ',') WITHIN GROUP (ORDER BY SICODE) AS SICODE_LIST FROM RGSI A WHERE STOCK_TYPE in ('1', '2') " _
                    & "AND a.type = 5 and A.brid = '" & cboBranchId.SelectedValue.ToString & "' AND A.DELETED =0 AND A.STATUS =0 " _
                    & "AND( A.due_date = get_t_plus(to_date('" & dtpCurrDate.Value & "','DD/MM/YYYY'),'" & cboBranchId.SelectedValue.ToString & "',0))"
                    Dim v_strObjMsg_HNTCCancelCheck = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLTX, gc_ActionInquiry, v_strClause_HNTCCancelCheck)
                    v_lngError = Proxy.Message(v_strObjMsg_HNTCCancelCheck)
                    If v_lngError <> ERR_SYSTEM_OK Then
                        MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                        Exit Sub
                    End If
                    Dim v_xmlDocument_HNTCCancelCheck As New Xml.XmlDocument
                    v_xmlDocument_HNTCCancelCheck.LoadXml(v_strObjMsg_HNTCCancelCheck)

                    Dim v_message As String = ""
                    Dim v_SR35Check As Integer = Convert.ToInt32(v_xmlDocument_SR35Check.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='CNT']").InnerText)
                    Dim v_PaymentCheck As Integer = Convert.ToInt32(v_xmlDocument_PaymentCheck.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='CNT']").InnerText)
                    Dim v_DCCReportsChk As Integer = Convert.ToInt32(v_xmlDocument_DCCReportsChk.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='CNT']").InnerText)
                    Dim v_HNTCCancelCheck = v_xmlDocument_HNTCCancelCheck.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='SICODE_LIST']").InnerText

                    If v_SR35Check < 1 Then
                        v_message &= "Chưa nhận được điện SR=35 từ HNX" & vbCrLf
                    End If

                    If v_PaymentCheck > 0 Then
                        v_message &= "Chưa hoàn tất thanh toán giao dịch hạn ngạch, tín chỉ các-bon trong ngày" & vbCrLf
                    End If

                    If v_DCCReportsChk < 4 Then
                        v_message &= "Chưa hoàn tất kết xuất dữ liệu cuối ngày bắt buộc cho DCC (GD: 4295,4296,4297,4298)" & vbCrLf
                    End If

                    If Len(v_HNTCCancelCheck) > 0 Then
                        v_message &= "Danh sách mã hạn ngạch, tín chỉ: '" & v_HNTCCancelCheck & "' phải hủy trong ngày nhưng chưa được thực hiện !"
                    End If

                    If Len(Trim(v_message)) > 0 Then
                        MsgBox(v_message, MsgBoxStyle.Exclamation + MsgBoxStyle.Critical, mv_ResourceManager.GetString("Title"))
                        Exit Sub
                    End If

                End If


                frmDailyTasks.mv_strBrid = cboBranchId.SelectedValue.ToString
                frmDailyTasks.mv_strCurDate = dtpCurrDate.Value.ToString("dd/MM/yyyy")
                frmDailyTasks.mv_strTellerdId = TellerId
                frmDailyTasks.Proxy = Proxy
                Me.Hide()
                frmDailyTasks.ShowDialog()
                'end bangpv 
                Cursor = Cursors.WaitCursor
                '' fix tam
                'v_strObjName = "NOTCONFIRMEDTASKES"
                'v_strModCode = "SY"
                'v_strAuthCode = "NNNNYYY"
                'v_strAuthString = "NNNNYY"
                '' end fix
                'Dim frm As New frmSearchMaster(m_BusLayer.AppLanguage)
                'frm.Name = v_strObjName
                'frm.BusDate = m_BusLayer.CurrentTellerProfile.BusDate
                'frm.TableName = v_strObjName
                'frm.ModuleCode = v_strModCode
                'frm.AuthCode = v_strAuthCode
                'frm.AuthString = v_strAuthString
                'frm.IsLocalSearch = gc_IsLocalMsg
                'frm.SearchOnInit = False
                'frm.BranchId = cboBranchId.SelectedValue.ToString
                'frm.TellerId = m_BusLayer.CurrentTellerProfile.TellerId
                'frm.TellerType = m_BusLayer.CurrentTellerProfile.TellerType
                'frm.IpAddress = m_BusLayer.AppIpAddress
                'frm.WsName = m_BusLayer.AppWsName
                'frm.InitDialog()
                'frm.CMDSQL = Replace(frm.CMDSQL, "?CURRDATE", dtpCurrDate.Value.ToShortDateString)

                'If frm.CountRowP() > 0 Then
                '    blnNotConfirmedTaskes = True
                '    frm.OnSearchP()
                '    frm.StartPosition = FormStartPosition.CenterScreen
                '    frm.ShowDialog()
                'End If
                'Rào tạm, mở sau- bangpv
                If frmDailyTasks.mv_blnSysErr = True Then
                    MsgBox("Không thể kết thúc ngày làm việc do còn các công việc trong ngày", MsgBoxStyle.Critical)
                    Cursor = Cursors.Default
                    Me.Show()
                    Exit Sub
                End If
                If frmDailyTasks.mv_blnTransStatus Then
                    If MsgBox(mv_ResourceManager.GetString("YesNo"), MsgBoxStyle.YesNo, mv_ResourceManager.GetString("Title")) = MsgBoxResult.No Then
                        Cursor = Cursors.Default
                        Exit Sub
                    End If
                End If
            End If

            v_oProcess = New AppCore.ProcessForm(Me.ParentForm)
            v_oProcess.ChangeCaption("Đang trong quá trình chạy Batch, vui lòng chờ đợi trong giây lát ...")
            v_oProcess.StartProcessForm()
            v_strObjMsg = BuildXMLObjMsg(dtpCurrDate.Value.ToString("dd/MM/yyyy"), cboBranchId.SelectedValue.ToString, , Me.TellerId, gc_IsNotLocalMsg, gc_MsgTypeObj, OBJNAME_SY_AUTHENTICATION, gc_ActionInquiry, , , Me.BEGINOREND, , )

            'Dim v_ws As New BDSDelivery.BDSDelivery
            v_lngError = Proxy.Message(v_strObjMsg)

            v_oProcess.StopProcessForm()
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                'Exit Sub
                'End If

                ''Kiểm tra thông tin và xử lý lỗi (nếu có) từ message trả về
                'Dim v_strErrorSource As String = "", v_strErrorMessage As String = ""
                'Dim v_lngErrorCode As Long

                'GetErrorFromMessage(v_strObjMsg, v_strErrorSource, v_lngErrorCode, v_strErrorMessage)

                'If v_lngErrorCode <> 0 Then
                'Update mouse pointer
                Cursor = Cursors.Default
                'MsgBox(v_strErrorMessage, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, Me.Text)
                Exit Sub
            Else

                If BEGINOREND = "BEGINOFDAY" Then
                    v_strClause = "select replace(pldt_strcat_agg(to_char(a.stt) || chr(9) || to_char(A.txname) || chr(9) || A.sicode  || chr(9) || to_char(a.msgamt) || chr(9) || to_char(a.dedate) ||  chr(9) || to_char(a.trandate) || chr(9) || A.reason || chr(13) || chr(10)),',','') batchmsg from " _
                                    & "( select rownum stt , a.* from " _
                                    & "(SELECT A.txname, a.sicode, a.msgamt, col_value08 dedate, col_value09 trandate, 'Đến ngày lưu ký' reason FROM TLLOG A WHERE A.BRID='" & cboBranchId.SelectedValue.ToString & "' AND A.AUTOID IN (SELECT A.AUTOID FROM SAISSUE A" _
                                    & ", (SELECT VARVALUE FROM SYSVAR WHERE varname='CURRDATE' AND BRID = '" & cboBranchId.SelectedValue.ToString & "' AND DELETED=0 AND STATUS=0) B " _
                                    & " WHERE GET_TRADE_DATE(A.DEDATE-1,'" & cboBranchId.SelectedValue.ToString & "','-') = TO_DATE(B.varvalue,'dd/mm/yyyy') " _
                                    & " AND A.DELETED=0 AND A.STATUS=0 and a.apptype='CA' )" _
                                    & " UNION ALL " _
                                    & " SELECT A.txname, a.sicode, a.msgamt, col_value06 dedate, col_value02 trandate, 'Đến ngày lưu ký' reason FROM TLLOG A WHERE A.BRID='" & cboBranchId.SelectedValue.ToString & "' AND A.AUTOID IN (SELECT A.AUTOID FROM SAISSUE A" _
                                    & ", (SELECT VARVALUE FROM SYSVAR WHERE varname='CURRDATE' AND BRID = '" & cboBranchId.SelectedValue.ToString & "' AND DELETED=0 AND STATUS=0) B " _
                                    & " WHERE GET_TRADE_DATE(A.DEDATE-1,'" & cboBranchId.SelectedValue.ToString & "','-') = TO_DATE(B.varvalue,'dd/mm/yyyy') " _
                                    & " AND A.DELETED=0 AND A.STATUS=0 and a.apptype='RG' )" _
                                    & ") a ) a "
                    v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLTX, gc_ActionInquiry, v_strClause)
                    v_lngError = Proxy.Message(v_strObjMsg)
                    If v_lngError <> ERR_SYSTEM_OK Then
                        MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                        Exit Sub
                    End If
                    v_xmlDocument.LoadXml(v_strObjMsg)
                    If Not v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='BATCHMSG']") Is Nothing Then
                        If v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='BATCHMSG']").InnerText.Trim <> "" Then
                            Dim frm As New frmBatchMsg
                            frm.BusDate = dtpCurrDate.Value.ToString("dd/MM/yyyy")
                            frm.BranchId = cboBranchId.SelectedValue.ToString
                            frm.txtErrMsg.Text = mv_ResourceManager.GetString("Success") & vbCrLf _
                            & "DANH SÁCH CÁC ĐỢT PHÁT HÀNH ĐÃ XỬ LÝ SAU KHI BẮT ĐẦU NGÀY (Chỉ tác động ROOM) : " & vbCrLf _
                            & "STT" & vbTab & "Mô tả đợt phát hành" & vbTab & "Chứng khoán" & vbTab & "Số lượng" & vbTab & "Ngày lưu ký" & vbTab & "Ngày giao dịch" & vbTab & "Lý do" & vbCrLf _
                            & v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='BATCHMSG']").InnerText
                            frm.ShowDialog()
                        Else
                            MsgBox(mv_ResourceManager.GetString("Success"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, Me.Text)
                        End If
                    Else
                        MsgBox(mv_ResourceManager.GetString("Success"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, Me.Text)
                    End If
                    m_BusLayer.CurrentTellerProfile.BusDate = dtpCurrDate.Value.ToString("dd/MM/yyyy")
                    frmMDIMain.sbrTime.Text = m_BusLayer.CurrentTellerProfile.BusDate & " - " _
                       & Format$(Now.Hour, "00") & ":" & Format$(Now.Minute, "00") & ":" & Format$(Now.Second, "00")
                Else
                    v_strClause = "select replace(pldt_strcat_agg(to_char(a.stt) || chr(9) || to_char(A.txname) || chr(9) || A.sicode  || chr(9) || to_char(a.msgamt) || chr(9) || to_char(a.dedate) ||  chr(9) || to_char(a.trandate) || chr(9) || A.reason || chr(13) || chr(10)),',','') batchmsg from " _
                                    & "( select rownum stt , a.* from " _
                                    & "(SELECT A.txname, a.sicode, a.msgamt, col_value08 dedate, col_value09 trandate, 'Đến ngày giao dịch' reason FROM TLLOG A WHERE A.BRID='" & cboBranchId.SelectedValue.ToString & "' AND A.AUTOID IN (SELECT A.AUTOID FROM SAISSUE A" _
                                    & ", (SELECT VARVALUE FROM SYSVAR WHERE varname='CURRDATE' AND BRID = '" & cboBranchId.SelectedValue.ToString & "' AND DELETED=0 AND STATUS=0) B " _
                                    & " WHERE GET_TRADE_DATE(A.TRANDATE-1,'" & cboBranchId.SelectedValue.ToString & "','-') = TO_DATE(B.varvalue,'dd/mm/yyyy') " _
                                    & " AND A.DELETED=0 AND A.STATUS=2 and a.apptype='CA' )" _
                                    & " UNION ALL " _
                                    & " SELECT A.txname, a.sicode, a.msgamt, col_value06 dedate, col_value02 trandate, 'Đến ngày giao dịch' reason FROM TLLOG A WHERE A.BRID='" & cboBranchId.SelectedValue.ToString & "' AND A.AUTOID IN (SELECT A.AUTOID FROM SAISSUE A" _
                                    & ", (SELECT VARVALUE FROM SYSVAR WHERE varname='CURRDATE' AND BRID = '" & cboBranchId.SelectedValue.ToString & "' AND DELETED=0 AND STATUS=0) B " _
                                    & " WHERE GET_TRADE_DATE(A.TRANDATE-1,'" & cboBranchId.SelectedValue.ToString & "','-') = TO_DATE(B.varvalue,'dd/mm/yyyy') " _
                                    & " AND A.DELETED=0 AND A.STATUS=2 and a.apptype='RG' )" _
                                    & " UNION ALL" _
                                    & " SELECT A.txname, a.sicode, a.msgamt, col_value08 dedate, col_value09 trandate, 'Đến ngày lưu ký' reason FROM TLLOG A WHERE A.BRID='" & cboBranchId.SelectedValue.ToString & "' AND A.AUTOID IN (SELECT A.AUTOID FROM SAISSUE A" _
                                    & ", (SELECT VARVALUE FROM SYSVAR WHERE varname='CURRDATE' AND BRID = '" & cboBranchId.SelectedValue.ToString & "' AND DELETED=0 AND STATUS=0) B " _
                                    & " WHERE GET_TRADE_DATE(A.DEDATE-1,'" & cboBranchId.SelectedValue.ToString & "','-') = TO_DATE(B.varvalue,'dd/mm/yyyy') " _
                                    & " AND A.DELETED=0 AND A.STATUS=1 and a.apptype='CA' )" _
                                    & " UNION ALL " _
                                    & " SELECT A.txname, a.sicode, a.msgamt, col_value06 dedate, col_value02 trandate, 'Đến ngày lưu ký' reason FROM TLLOG A WHERE A.BRID='" & cboBranchId.SelectedValue.ToString & "' AND A.AUTOID IN (SELECT A.AUTOID FROM SAISSUE A" _
                                    & ", (SELECT VARVALUE FROM SYSVAR WHERE varname='CURRDATE' AND BRID = '" & cboBranchId.SelectedValue.ToString & "' AND DELETED=0 AND STATUS=0) B " _
                                    & " WHERE GET_TRADE_DATE(A.DEDATE-1,'" & cboBranchId.SelectedValue.ToString & "','-') = TO_DATE(B.varvalue,'dd/mm/yyyy') " _
                                    & " AND A.DELETED=0 AND A.STATUS=1 and a.apptype='RG' )" _
                                    & ") a ) a "
                    v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLTX, gc_ActionInquiry, v_strClause)
                    v_lngError = Proxy.Message(v_strObjMsg)
                    If v_lngError <> ERR_SYSTEM_OK Then
                        MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                        Exit Sub
                    End If
                    v_xmlDocument.LoadXml(v_strObjMsg)
                    If Not v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='BATCHMSG']") Is Nothing Then
                        If v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='BATCHMSG']").InnerText.Trim <> "" Then
                            Dim frm As New frmBatchMsg
                            frm.BusDate = dtpCurrDate.Value.ToString("dd/MM/yyyy")
                            frm.BranchId = cboBranchId.SelectedValue.ToString
                            frm.txtErrMsg.Text = mv_ResourceManager.GetString("Success") & vbCrLf _
                            & "DANH SÁCH CÁC ĐỢT PHÁT HÀNH ĐÃ XỬ LÝ SAU KHI KẾT THÚC NGÀY : " & vbCrLf _
                            & "STT" & vbTab & "Mô tả đợt phát hành" & vbTab & "Chứng khoán" & vbTab & "Số lượng" & vbTab & "Ngày lưu ký" & vbTab & "Ngày giao dịch" & vbTab & "Lý do" & vbCrLf _
                            & v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='BATCHMSG']").InnerText
                            frm.ShowDialog()
                        Else
                            MsgBox(mv_ResourceManager.GetString("Success"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, Me.Text)
                        End If
                    Else
                        MsgBox(mv_ResourceManager.GetString("Success"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, Me.Text)
                    End If
                End If
            End If
            Me.Close()
            Cursor = Cursors.Default
        Catch ex As Exception
            Cursor = Cursors.Default
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                        & "Error code: System error!" & vbNewLine _
                                        & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
    Private Sub FillDate()
        Dim v_strObjMsg As String
        Dim v_nodeList As Xml.XmlNodeList
        Dim v_strClause As String
        Dim v_xmlDocument As New Xml.XmlDocument
        Dim dtIndex As Date
        Dim v_lngError As Long
        Try
            If BEGINOREND = "BEGINOFDAY" Then
                v_strClause = "SELECT GET_TRADE_DATE(to_date(a.VARVALUE,'dd/mm/yyyy')+1,'" & cboBranchId.SelectedValue.ToString & "','+') VARVALUE FROM sysvar a " _
                & " where a.GRNAME='SYSTEM' and a.VARNAME= 'CURRDATE' and BRID='" & cboBranchId.SelectedValue.ToString & "'"

                v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLTX, gc_ActionInquiry, v_strClause)
                v_lngError = Proxy.Message(v_strObjMsg)
                If v_lngError <> ERR_SYSTEM_OK Then
                    MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                    Exit Sub
                End If
                v_xmlDocument.LoadXml(v_strObjMsg)
                dtIndex = CDate(v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='VARVALUE']").InnerText)
                'While True
                '    dtIndex = DateAdd("D", 1, dtIndex)
                '    v_strClause = "SELECT count(*) COUNTNO FROM syscldr a where a.deleted=0 and a.status= 0 and SBDATE= TO_DATE('" & dtIndex.ToString("dd/MM/yyyy") & "','dd/mm/yyyy')"
                '    v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLTX, gc_ActionInquiry, v_strClause)
                '    v_lngError = Proxy.Message(v_strObjMsg)
                '    If v_lngError <> ERR_SYSTEM_OK Then
                '        MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                '        Exit Sub
                '    End If
                '    v_xmlDocument.LoadXml(v_strObjMsg)
                '    If v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='COUNTNO']").InnerText = "0" Then
                '        Exit While
                '    End If
                'End While
                dtpCurrDate.Value = dtIndex
            Else
                v_strClause = "SELECT a.VARVALUE FROM sysvar a where a.GRNAME='SYSTEM' and a.VARNAME= 'CURRDATE' and BRID='" & cboBranchId.SelectedValue.ToString & "'"
                v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLTX, gc_ActionInquiry, v_strClause)
                v_lngError = Proxy.Message(v_strObjMsg)
                If v_lngError <> ERR_SYSTEM_OK Then
                    MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                    Exit Sub
                End If
                v_xmlDocument.LoadXml(v_strObjMsg)
                dtpCurrDate.Value = CDate(v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='VARVALUE']").InnerText)
            End If
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                        & "Error code: System error!" & vbNewLine _
                                        & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Sub

    Private Sub cboBranchId_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboBranchId.SelectedIndexChanged
        If mv_blnOnInit Then
            FillDate()
        End If
    End Sub
End Class