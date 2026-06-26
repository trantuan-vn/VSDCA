Imports System.IO
Imports System.Collections
Imports Sats.CommonLibrary
Imports Xceed.SmartUI.Controls
Imports Sats.WinFormsUI.Docking
Imports System.Windows.Forms
Imports Microsoft.Office.Interop.Excel

Public Class frmDailyTasks
    Private mv_oProxy As BDSChannel.BDSDelivery

    Public Property Proxy() As BDSChannel.BDSDelivery
        Get
            Return mv_oProxy
        End Get
        Set(ByVal value As BDSChannel.BDSDelivery)
            mv_oProxy = value
        End Set
    End Property
#Region "Khai báo biến"
    'Private mv_BDSDelivery As BDSChannel.BDSDelivery
    Private mv_ResourceManager As Resources.ResourceManager
    Private mv_strLanguage As String
    Private mv_strTellerId As String
    Private mv_strBranchId As String
    Private mv_strGroupId As String
    Private mv_strGroupName As String
    Private mv_intExecFlag As Integer
    Private mv_strAssignType As String
    Private mv_strMIInUser As String
    Private mv_strMIOutUser As String
    Private mv_strMIInGrp As String
    Public mv_blnTransStatus As Boolean
    Public mv_blnSysErr As Boolean
    Public mv_strTellerdId As String
    Const mc_strAdmin = "Admin"

    Private hTlidInGrpFilter As New Hashtable
    Private hTellerFilter As New Hashtable
    Public mv_strBrid As String
    Public mv_strCurDate As String
    Private mv_lngAllMember As Long
    Private mv_lngAllStock As Long
    Private mv_strMemberFilter As String
    Private mv_strStockFilter As String
    Public v_intCountEnter As Integer = 0
#End Region
#Region "Khai báo thuộc tính"

    Public Property AssignType() As String
        Get
            Return mv_strAssignType
        End Get
        Set(ByVal Value As String)
            mv_strAssignType = Value
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

    Public Property GroupId() As String
        Get
            Return mv_strGroupId
        End Get
        Set(ByVal Value As String)
            mv_strGroupId = Value
        End Set
    End Property

    Public Property GroupName() As String
        Get
            Return mv_strGroupName
        End Get
        Set(ByVal Value As String)
            mv_strGroupName = Value
        End Set
    End Property

    Public Property ExeFlag() As Integer
        Get
            Return mv_intExecFlag
        End Get
        Set(ByVal Value As Integer)
            mv_intExecFlag = Value
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
    Private Sub frmDailyTasks_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'mv_BDSDelivery = New BDSChannel.BDSDelivery
            Dim v_strSQL, v_strObjMsg As String
            mv_blnTransStatus = False
            mv_blnSysErr = False
            v_strSQL = "select 1 stt, 'SETT_T3', decode (count(*) , 0,decode('" & mv_strBrid & "','0007','', 'Đã thực hiện thanh toán bù trừ đa phương T+3'),'Chưa thực hiện thanh toán bù trừ T+3' ) txmessage " _
            & " FROM tllog WHERE t_no =1 and tltxcd in ('4081','4084') and status in (1,2)  and BRID ='" & mv_strBrid & "' and busdate = to_date('" & mv_strCurDate & "','DD/MM/YYYY')" _
            & "UNION ALL " _
            & "select 2 stt, 'sett_t1', decode (count(*), 0,decode('" & mv_strBrid & "','0007','', N'Đã thực hiện thanh toán bù trừ đa phương T+1'),'Chưa thực hiện thanh toán bù trừ T+1') txmessage " _
            & " FROM tllog WHERE t_no =2 and tltxcd in ('4082','4084')and status in (1,2)  and BRID ='" & mv_strBrid & "' and busdate = to_date('" & mv_strCurDate & "','DD/MM/YYYY')" _
            & "UNION ALL " _
            & "select 3 stt, 'sett_td', decode (count(*), 0,decode('" & mv_strBrid & "','0007','', 'Đã thực hiện thanh toán trực tiếp'),'Chưa thực hiện thanh toán trực tiếp') txmessage " _
            & " FROM tllog WHERE t_no =3 and tltxcd in ('4083','4084') and status in (1,2) and BRID ='" & mv_strBrid & "' and busdate = to_date('" & mv_strCurDate & "','DD/MM/YYYY')" _
            & "UNION ALL " _
            & "select 4 stt, varvalue, decode (varvalue, '0','Chưa thực hiện kết xuất ROOM','2','Có chứng khoán tới ngày lưu ký, chú ý kết xuất Room sau khi kết thúc ngày làm việc', 'Đã thực hiện kết xuất ROOM') txmessage " _
            & " FROM SYSVAR WHERE VARNAME ='room_status' and brid not in ('0007') and BRID ='" & mv_strBrid & "' " _
            & "UNION ALL " _
            & "select 5 stt, varvalue, decode (varvalue, '0','Chưa kết xuất thanh toán T1 cho NHCĐTT', 'Đã thực hiện kết xuất thanh toán T1 cho NCĐTT') txmessage " _
            & " FROM SYSVAR WHERE VARNAME ='bank_status_t1' and brid not in ('0007') and BRID ='" & mv_strBrid & "' " _
            & "UNION ALL " _
            & "select 6 stt, varvalue, decode (varvalue, '0','Chưa kết xuất thanh toán T3 cho NHCĐTT', 'Đã thực hiện kết xuất thanh toán T3 cho NCĐTT') txmessage " _
            & " FROM SYSVAR WHERE VARNAME ='bank_status_t3' and brid not in ('0007') and BRID ='" & mv_strBrid & "' " _
            & "UNION ALL " _
            & "select 7 stt, varvalue, decode (varvalue, '0','Chưa kết xuất thanh toán trực tiếp cho NHCĐTT', 'Đã thực hiện kết xuất thanh toán TT cho NCĐTT') txmessage " _
            & " FROM SYSVAR WHERE VARNAME ='bank_status_td' and brid not in ('0007') and BRID ='" & mv_strBrid & "' " _
            & "UNION ALL " _
            & "SELECT 8 stt, '1' varvalue, DECODE (COUNT (autoid), 0,'Đã xác nhận hết các giao dịch', 'Còn giao dịch chưa xác nhận') txmessage " _
            & " FROM tllog " _
            & "WHERE status IN (1, 2) " _
            & "AND deleted = 0 " _
            & "AND brid = '" & mv_strBrid & "'" _
            & "AND (   (    busdate IS NULL " _
            & " AND txdate <= TO_DATE ('" & mv_strCurDate & "', 'dd/mm/yyyy') " _
            & ")" _
            & " OR (busdate = TO_DATE ('" & mv_strCurDate & "', 'dd/mm/yyyy')) " _
            & ") "
            'bangpv: Bo check giao dich 2053, 2054
            'v_strSQL = v_strSQL & " Union all Select 9 stt, '2053' varvalue , (case when count(a.cfr_status)>0 then N'Chưa thực hiện giao dịch 2053' else N'' end) txmessage from saissue a, tllog b where a.DEDATE=get_t_plus(to_date('" & mv_strCurDate & "','DD/MM/YYYY'),'" & mv_strBrid & "',1) and a.cfr_status =0 and a.autoid =b.autoid and b.brid ='" & mv_strBrid & "'" _
            '& "UNION ALL Select 10 stt, '2054' varvalue , (case when count(a.cfr_status)>0 then N'Chưa thực hiện giao dịch 2054' else N'' end) txmessage from saissue a, tllog b where TRANDATE=get_t_plus(to_date('" & mv_strCurDate & "','DD/MM/YYYY'),'" & mv_strBrid & "',1) and a.cfr_status <2 and a.status =1 and a.autoid =b.autoid and b.brid ='" & mv_strBrid & "'"
            'end bangpv
            'check giao dich 4110 chua xu ly 
            v_strSQL = v_strSQL & "UNION ALL  SELECT  9 stt,'1' varvalue,  decode (count(*) , 0,decode('" & mv_strBrid & "','0008', 'Đã thực hiện thanh toán TPRL',''),'Chưa thực hiện xử lý hết các giao dịch thanh toán TPRL' ) txmessage " _
                                & "FROM tllog WHERE  tltxcd in ('4110') and status in (1,2)  and BRID ='" & mv_strBrid & "' and col_value02 = '" & mv_strCurDate & "'"


            'v_strSQL = ToLiteral(v_strSQL)
            v_strObjMsg = BuildXMLObjMsg(, , , mv_strTellerdId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLTX, gc_ActionInquiry, v_strSQL)

            'Dim v_wsOutGrp As New BDSDelivery.BDSDelivery
            Dim v_lngError As Long = Proxy.Message(v_strObjMsg)

            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Exit Sub
            End If

            Dim v_xmlDocument As New Xml.XmlDocument
            Dim v_node As Xml.XmlNodeList
            Dim v_strTxMessage, v_strFLDNAME, v_strValue As String

            v_xmlDocument.LoadXml(v_strObjMsg)
            v_node = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")
            For i As Integer = 0 To v_node.Count - 1
                For j As Integer = 0 To v_node.Item(i).ChildNodes.Count - 1
                    With v_node.Item(i).ChildNodes(j)
                        v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                        v_strValue = .InnerText.ToString
                    End With
                    Select Case Trim(UCase(v_strFLDNAME))
                        Case "TXMESSAGE"
                            v_strTxMessage &= v_strValue & vbCrLf
                            If v_strValue = "Còn giao dịch chưa xác nhận" Then
                                mv_blnTransStatus = True
                            End If
                            If LSet(v_strValue, 14) = "Chưa thực hiện" Then
                                mv_blnSysErr = True
                            End If
                    End Select
                Next
            Next

            txtTranMessage.Text = v_strTxMessage
            If mv_blnTransStatus = True Then
                btnDetails.Enabled = True
            Else
                btnDetails.Enabled = False
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDetails.Click
        ' fix tam
        Dim v_strObjName, v_strModCode, v_strAuthCode, v_strAuthString As String
        v_strObjName = "NOTCONFIRMEDTASKES"
        v_strModCode = "SY"
        v_strAuthCode = "NNNNYYY"
        v_strAuthString = "NNNNYY"
        ' end fix
        Dim frm As New frmSearchMaster(m_BusLayer.AppLanguage)
        frm.Name = v_strObjName
        frm.BusDate = m_BusLayer.CurrentTellerProfile.BusDate
        frm.TableName = v_strObjName
        frm.ModuleCode = v_strModCode
        frm.AuthCode = v_strAuthCode
        frm.AuthString = v_strAuthString
        frm.IsLocalSearch = gc_IsNotLocalMsg
        frm.SearchOnInit = True
        frm.BranchId = mv_strBrid
        frm.Proxy = Proxy
        frm.Client = mv_oLocal

        frm.StockFilter = m_BusLayer.CurrentTellerProfile.StockFilter
        frm.MemberFilter = m_BusLayer.CurrentTellerProfile.MemberFilter
        frm.TellerId = m_BusLayer.CurrentTellerProfile.TellerId
        frm.TellerType = m_BusLayer.CurrentTellerProfile.TellerType
        frm.IpAddress = m_BusLayer.AppIpAddress
        frm.WsName = m_BusLayer.AppWsName


        frm.InitDialog()
        frm.CMDSQL = Replace(frm.CMDSQL, "01/01/1975", mv_strCurDate)

        'If frm.CountRowP() > 0 Then
        '            blnNotConfirmedTaskes = True
        frm.OnSearchP()
        frm.StartPosition = FormStartPosition.CenterScreen
        frm.ShowDialog()
        'End If
    End Sub

    Private Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Me.Close()
        If mv_blnSysErr = True Then
            'MsgBox("Không thể kết thúc ngày làm việc do còn các công việc trong ngày", MsgBoxStyle.Critical)
            Exit Sub
        End If
    End Sub
End Class