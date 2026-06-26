Imports Sats.CommonLibrary
Imports System.IO
Imports System.Windows.Forms
Imports Sats.AppCore
Imports Sats.ClientCA

Public Class frmExportData
    Private mv_ResourceManager As Resources.ResourceManager
    Private mv_strLanguage As String
    Private mv_strTellerId As String
    Private mv_strBranchId As String
    Private mv_strBranchId2 As String
    Private mv_strBranchId3 As String
    Private mv_strFilePath As String
    Private mv_strBusDate As String
    Private mv_strFileName As String = ""

    Private Const FORMAT_STOCK_CODE As String = "        " '8 ki tu trang

    'Private mv_BDSDelivery As New BDSChannel.BDSDelivery
    Private v_blnExpSi, v_blnExpIs, v_blnExpMi, v_blnExpRoom As Boolean
    Private v_blnMultiPartite, v_blnDirect As Boolean
    Private v_intFrequence As Integer
    Private v_strFilePath As String
    'Private v_strFileNHNN As String = "CDS_TRANS_NHNN" & v_dCurrentDate_NHNN & "_" & DateTime.Now.ToString("HH:mm:ss").Replace(":", "") & ".xml"
    Private v_dCurrentDate As String = CStr(Format(Now.Date, "ddMMyyyy"))
    Private v_dCurrentDate_NHNN As String = CStr(Format(Now.Date, "ddMMyyyy"))
    Private v_strFileNHNN As String = "CDS_TRANS_NHNN" & v_dCurrentDate_NHNN & "_" & DateTime.Now.ToString("HH:mm:ss").Replace(":", "") & ".xml"
    Private v_dtIndex As Date
    Private v_dtIndex_NHNN As Date
    Private mv_oProxy As BDSChannel.BDSDelivery
    Private mv_xftpFTPEngine As New FTPEngine
    Private mv_xZipEngine As New ZipEngine
    Dim v_oProcess As New ProcessForm(Me)

    'myvq
    Private mv_strRespond As String = ""
    Private Const HEADER_ENCRYPTED As String = "ENCRYPTED_"

    Public Property Proxy() As BDSChannel.BDSDelivery
        Get
            Return mv_oProxy
        End Get
        Set(ByVal value As BDSChannel.BDSDelivery)
            mv_oProxy = value
        End Set
    End Property

#Region "Khai bao cac thuong tinh"
    Public Property FilePath() As String
        Get
            Return mv_strFilePath
        End Get
        Set(ByVal value As String)
            mv_strFilePath = value
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
        Set(ByVal Value As String)
            mv_strBranchId = Value
        End Set
    End Property
    Public Property BranchId2() As String
        Get
            Return mv_strBranchId2
        End Get
        Set(ByVal Value As String)
            mv_strBranchId2 = Value
        End Set
    End Property
    Public Property BranchId3() As String
        Get
            Return mv_strBranchId3
        End Get
        Set(ByVal Value As String)
            mv_strBranchId3 = Value
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
#End Region
#Region "Method"
    Public Function Uni2TCVN(ByVal str$) As String
        Dim i As Integer, arrABC() As String, sUni$, ABC$, UNI$
        ABC = "¸,µ,¶,·,¹,¨,¾,»,¼,½,Æ,©,Ê,Ç,È,É,Ë,Ð,Ì,Î,Ï,Ñ,ª,Õ,Ò,Ó,Ô,Ö,Ý,×,Ø,Ü,Þ,ã,ß,á,â,ä,«,è,å,æ,ç,é,¬,í,ê,ë,ì,î,ó,ï,ñ,ò,ô,­,ø,õ,ö,÷," _
        & "ù,ý,ú,û,ü,þ,®,¸,µ,¶,·,¹,¡,¾,»,¼,½,Æ,¢,Ê,Ç,È,É,Ë,Ð,Ì,Î,Ï,Ñ,£,Õ,Ò,Ó,Ô,Ö,Ý,×,Ø,Ü,Þ,ã,ß,á,â,ä,¤,è,å,æ,ç,é,¥,í,ê,ë,ì,î,ó,ï,ñ,ò,ô,¦,ø,õ,ö,÷,ù,ý,ú,û,ü,þ,§"
        UNI = "áàảãạăắằẳẵặâấầẩẫậéèẻẽẹêếềểễệíìỉĩịóòỏõọôốồổỗộơớờởỡợúùủũụưứừửữựýỳỷỹỵđáàảãạĂắằẳẵặÂấầẩẫậéèẻẽẹÊếềểễệíìỉĩịóòỏõọÔốồổỗộƠớờởỡợúùủũụƯứừửữựýỳỷỹỵĐ"
        'arrUNI = Split(UNI, ",")
        arrABC = Split(ABC, ",")
        For i = 1 To Len(str$)
            If InStr(UNI, Mid(str$, i, 1)) > 0 Then
                sUni = sUni & arrABC(InStr(UNI, Mid(str$, i, 1)) - 1)
            Else
                sUni = sUni & Mid(str$, i, 1)
            End If
        Next
        Uni2TCVN = sUni
    End Function
    Private Sub OnInit()
        Dim v_strSQL As String = ""
        Dim v_xmlDocument As New Xml.XmlDocument
        Dim v_strObjMsg As String
        'Dim dtIndex As Date
        Try
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("vi-VN")
            mv_ResourceManager = New Resources.ResourceManager(gc_RootNamespace & ".frmExportData_" & UserLanguage, System.Reflection.Assembly.GetExecutingAssembly())
            LoadUserInterface(Me)
            FillDataCombobox()
            'bangpv
            'Lấy ngày hệ thống
            v_strSQL = "SELECT a.VARVALUE FROM sysvar a where a.GRNAME='SYSTEM' and a.VARNAME= 'CURRDATE' and BRID='" & BranchId & "'"
            v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLTX, gc_ActionInquiry, v_strSQL)
            Dim v_lngError As Long = Proxy.Message(v_strObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Exit Sub
            End If
            v_xmlDocument.LoadXml(v_strObjMsg)
            v_dtIndex = CDate(v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='VARVALUE']").InnerText)
            dtpTransDate4.Value = v_dtIndex
            v_dCurrentDate = CStr(Format(v_dtIndex, "ddMMyyyy"))
            'end bangpv 
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                          & "Error code: System error!" & vbNewLine _
                                          & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
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
                ElseIf TypeOf (v_ctrl) Is TabControl Then
                    For Each v_ctrlTmp As Control In CType(v_ctrl, TabControl).TabPages
                        CType(v_ctrlTmp, TabPage).Text = mv_ResourceManager.GetString(v_ctrlTmp.Tag)
                    Next
                    LoadUserInterface(v_ctrl)
                ElseIf TypeOf (v_ctrl) Is TabPage Then
                    v_ctrl.BackColor = System.Drawing.SystemColors.InactiveCaptionText
                    CType(v_ctrl, TabPage).Text = mv_ResourceManager.GetString(v_ctrl.Tag)
                    LoadUserInterface(v_ctrl)
                ElseIf TypeOf (v_ctrl) Is Label Then
                    CType(v_ctrl, Label).Text = mv_ResourceManager.GetString(v_ctrl.Tag)
                ElseIf TypeOf (v_ctrl) Is Button Then
                    CType(v_ctrl, Button).Text = mv_ResourceManager.GetString(v_ctrl.Tag)
                ElseIf TypeOf (v_ctrl) Is CheckBox Then
                    CType(v_ctrl, CheckBox).Text = mv_ResourceManager.GetString(v_ctrl.Tag)
                ElseIf TypeOf (v_ctrl) Is RadioButton Then
                    CType(v_ctrl, RadioButton).Text = mv_ResourceManager.GetString(v_ctrl.Tag)
                End If
            Next

            'Load caption của form, label caption
            Me.Text = mv_ResourceManager.GetString("frmExportData")
            lbCaption.Text = mv_ResourceManager.GetString("lbCaption")
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
            Dim strRow As String = "SELECT BRID VALUE, BRID || ' - ' || BRNAME DISPLAY FROM BRGRP WHERE STATUS = 0 AND DELETED = 0 ORDER BY BRID"
            'Dim strRow As String = "select cdval value, cdval ||'_'||cdcontent display from  allcode where cdtype = 'CS' and cdname='BRID' order by value"
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
                cboBranch.AddItems(v_strCDCONTENT, v_strCDVAL)
                'cboBranch2.AddItems(v_strCDCONTENT, v_strCDVAL)
                cboBranch3.AddItems(v_strCDCONTENT, v_strCDVAL)
                'Them cho Cover warrant 
                If v_strCDVAL = "0002" Then
                    cboBranch6.AddItems(v_strCDCONTENT, v_strCDVAL)
                End If
            Next
            'code rieng voi cboBranch2 va cbo branch4 
            strRow = "select cdval value, cdval ||'_'||cdcontent display from  allcode where cdtype = 'CS' and cdname='BRID' order by value"
            v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLGROUPS, gc_ActionInquiry, strRow)
            v_lngError = Proxy.Message(v_strObjMsg)
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
                cboBranch5.AddItems(v_strCDCONTENT, v_strCDVAL)
                cboBranch4.AddItems(v_strCDCONTENT, v_strCDVAL)
                cboBranch2.AddItems(v_strCDCONTENT, v_strCDVAL)
            Next
            'cboBranch2.AddItems("1000_TH HNX_HOSE", "1000")
            'cboBranch2.AddItems("1004_TP", "1004")
            'cboBranch2.AddItems("1001_CP", "1001")
            cboFrequence.AddItems("T + 1", 2)
            cboFrequence.AddItems("T + 2", 1)
            cboFrequence2.AddItems("Sáng", 1)
            cboFrequence2.AddItems("Chiều", 2)
            cboCsErrType.AddItems("Thiếu CK", 1)
            cboCsErrType.AddItems("Thiếu tiền", 2)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub GetFileExpPath()
        Try

        Catch ex As Exception

        End Try
    End Sub
    Private Function GetXMLCDS_TRANS_NHNN() As Xml.XmlDocument
        Try
            Dim v_strSQL As String = ""
            Dim v_xmlDocument As New Xml.XmlDocument
            Dim v_strObjMsg As String
            Dim v_strFromDate As String 'v_strTxDate = Format(dtpTransDate.Value, "dd/MM/yyyy")
            Dim v_strToDate As String
            v_strFromDate = Format(dtpFromDate.Value, "dd/MM/yyyy")
            v_strToDate = Format(dtpToDate.Value, "dd/MM/yyyy")
            ' Edited by Thanglv9 - 18/10/2012.Them ma GTCG tu NHNN va ma TV tu NHNN
            v_strSQL = "select a.bankcodevsd,a.bankcodenhnn, a.seccodevsd,b.sbv_sicode seccodenhnn, a.trans_type, a.quantity*b.part_value quantity, a.trancode, a.type from (" _
                & "select a.* from ( " _
                & " select substr(a.tradeacctno,-3) bankcodevsd,a.sbv_micode bankcodenhnn, b.sicode seccodevsd,decode (b.operator,'-',2,1) trans_type, " _
                & " b.namt quantity, b.txnum trancode , decode(b.operator,'-',18,3) type  " _
                & "from rgiiia a, iatran b where a.iicode =b.iicode and a.deleted =0 and a.status =0 and b.deleted=0 and b.status =0 " _
                & " and b.tltxcd ='3048' and b.micode = '501' and a.tradeacctno <> '501A990012' and a.tradeacctno like '501%' and brid ='" & BranchId3 & "'" _
                & "and txdate >= to_date('" & v_strFromDate & " 00:00:00','DD/MM/YYYY hh24:mi:ss') and txdate <= to_date('" & v_strToDate & " 23:59:00','DD/MM/YYYY hh24:mi:ss') " _
                        & "union all " _
                & "select substr(a.tradeacctno,-3) bankcodevsd,a.sbv_micode bankcodenhnn, b.sicode seccodevsd,decode (b.operator,'-',2,1) trans_type, " _
                & "b.namt quantity, b.txnum trancode , decode(b.operator,'-',18,3) type  " _
                & "from rgiiia a, passed.iatrana_all b where a.iicode =b.iicode and a.deleted =0 and a.status =0 and b.deleted=0 and b.status =0 " _
                & "and b.tltxcd ='3048' and b.micode = '501' and a.tradeacctno <> '501A990012' and a.tradeacctno like '501%' and brid ='" & BranchId3 & "'" _
                & "and txdate >= to_date('" & v_strFromDate & " 00:00:00','DD/MM/YYYY hh24:mi:ss') and txdate <= to_date('" & v_strToDate & " 23:59:00','DD/MM/YYYY hh24:mi:ss')  ) a"

            ' Added by Thanglv9 - 29/11/2012
            v_strSQL = v_strSQL & " Union all select a.* from ( " _
               & " select substr(a.tradeacctno,-3) bankcodevsd,a.sbv_micode bankcodenhnn, b.sicode seccodevsd,decode (b.operator,'-',2,1) trans_type, " _
               & " b.namt quantity, b.txnum trancode , decode(b.operator,'-',18,3) type  " _
               & "from rgiiia a, iatran b where a.iicode =b.iicode and a.deleted =0 and a.status =0 and b.deleted=0 and b.status =0 " _
               & " and b.tltxcd ='3002' and b.micode = '501' and b.micode <> '000' and a.tradeacctno <> '501A990012' and a.tradeacctno like '501%' and brid ='" & BranchId3 & "'" _
               & "and txdate >= to_date('" & v_strFromDate & " 00:00:00','DD/MM/YYYY hh24:mi:ss') and txdate <= to_date('" & v_strToDate & " 23:59:00','DD/MM/YYYY hh24:mi:ss') " _
                       & "union all " _
               & "select substr(a.tradeacctno,-3) bankcodevsd,a.sbv_micode bankcodenhnn, b.sicode seccodevsd,decode (b.operator,'-',2,1) trans_type, " _
               & "b.namt quantity, b.txnum trancode , decode(b.operator,'-',18,3) type  " _
               & "from rgiiia a, passed.iatrana_all b where a.iicode =b.iicode and a.deleted =0 and a.status =0 and b.deleted=0 and b.status =0 " _
               & "and b.tltxcd ='3002' and b.micode = '501' and b.micode <> '000' and a.tradeacctno <> '501A990012' and a.tradeacctno like '501%' and brid ='" & BranchId3 & "'" _
               & "and txdate >= to_date('" & v_strFromDate & " 00:00:00','DD/MM/YYYY hh24:mi:ss') and txdate <= to_date('" & v_strToDate & " 23:59:00','DD/MM/YYYY hh24:mi:ss')  ) a "

            v_strSQL = v_strSQL & " Union all select a.* from ( " _
               & " select substr(a.tradeacctno,-3) bankcodevsd,a.sbv_micode bankcodenhnn, b.sicode seccodevsd,decode (b.operator,'-',1,2) trans_type, " _
               & " b.namt quantity, b.txnum trancode , decode(b.operator,'-',3,18) type  " _
               & "from rgiiia a, iatran b where a.iicode =b.iicode and a.deleted =0 and a.status =0 and b.deleted=0 and b.status =0 " _
               & " and b.tltxcd ='3002' and b.micode = '501' and b.micode <> '000' and a.tradeacctno <> '501A990012' and a.tradeacctno like '501%' and brid ='" & BranchId3 & "'" _
               & "and txdate >= to_date('" & v_strFromDate & " 00:00:00','DD/MM/YYYY hh24:mi:ss') and txdate <= to_date('" & v_strToDate & " 23:59:00','DD/MM/YYYY hh24:mi:ss') " _
                       & "union all " _
               & "select substr(a.tradeacctno,-3) bankcodevsd,a.sbv_micode bankcodenhnn, b.sicode seccodevsd,decode (b.operator,'-',1,2) trans_type, " _
               & "b.namt quantity, b.txnum trancode , decode(b.operator,'-',3,18) type  " _
               & "from rgiiia a, passed.iatrana_all b where a.iicode =b.iicode and a.deleted =0 and a.status =0 and b.deleted=0 and b.status =0 " _
               & "and b.tltxcd ='3002' and b.micode = '501' and b.micode <> '000' and a.tradeacctno <> '501A990012' and a.tradeacctno like '501%' and brid ='" & BranchId3 & "'" _
               & "and txdate >= to_date('" & v_strFromDate & " 00:00:00','DD/MM/YYYY hh24:mi:ss') and txdate <= to_date('" & v_strToDate & " 23:59:00','DD/MM/YYYY hh24:mi:ss')  ) a "

            v_strSQL = v_strSQL & " Union all select a.* from ( " _
               & " select substr(a.tradeacctno,-3) bankcodevsd,a.sbv_micode bankcodenhnn, b.sicode seccodevsd,decode (b.operator,'-',2,1) trans_type, " _
               & " b.namt quantity, b.txnum trancode , decode(b.operator,'-',18,3) type  " _
               & "from rgiiia a, iatran b where a.iicode =b.iicode and a.deleted =0 and a.status =0 and b.deleted=0 and b.status =0 " _
               & " and b.tltxcd ='3052' and b.micode = '501' and b.micode <> '000' and a.tradeacctno <> '501A990012' and a.tradeacctno like '501%' and brid ='" & BranchId3 & "'" _
               & "and txdate >= to_date('" & v_strFromDate & " 00:00:00','DD/MM/YYYY hh24:mi:ss') and txdate <= to_date('" & v_strToDate & " 23:59:00','DD/MM/YYYY hh24:mi:ss') " _
                       & "union all " _
               & "select substr(a.tradeacctno,-3) bankcodevsd,a.sbv_micode bankcodenhnn, b.sicode seccodevsd,decode (b.operator,'-',2,1) trans_type, " _
               & "b.namt quantity, b.txnum trancode , decode(b.operator,'-',18,3) type  " _
               & "from rgiiia a, passed.iatrana_all b where a.iicode =b.iicode and a.deleted =0 and a.status =0 and b.deleted=0 and b.status =0 " _
               & "and b.tltxcd ='3052' and b.micode = '501' and b.micode <> '000' and a.tradeacctno <> '501A990012' and a.tradeacctno like '501%' and brid ='" & BranchId3 & "'" _
               & "and txdate >= to_date('" & v_strFromDate & " 00:00:00','DD/MM/YYYY hh24:mi:ss') and txdate <= to_date('" & v_strToDate & " 23:59:00','DD/MM/YYYY hh24:mi:ss')  ) a "

            v_strSQL = v_strSQL & " Union all select a.* from ( " _
               & " select substr(a.tradeacctno,-3) bankcodevsd,a.sbv_micode bankcodenhnn, b.sicode seccodevsd,decode (b.operator,'-',1,2) trans_type, " _
               & " b.namt quantity, b.txnum trancode , decode(b.operator,'-',3,18) type  " _
               & "from rgiiia a, iatran b where a.iicode =b.iicode and a.deleted =0 and a.status =0 and b.deleted=0 and b.status =0 " _
               & " and b.tltxcd ='3052' and b.micode = '501' and b.micode <> '000' and a.tradeacctno <> '501A990012' and a.tradeacctno like '501%' and brid ='" & BranchId3 & "'" _
               & "and txdate >= to_date('" & v_strFromDate & " 00:00:00','DD/MM/YYYY hh24:mi:ss') and txdate <= to_date('" & v_strToDate & " 23:59:00','DD/MM/YYYY hh24:mi:ss') " _
                       & "union all " _
               & "select substr(a.tradeacctno,-3) bankcodevsd,a.sbv_micode bankcodenhnn, b.sicode seccodevsd,decode (b.operator,'-',1,2) trans_type, " _
               & "b.namt quantity, b.txnum trancode , decode(b.operator,'-',3,18) type  " _
               & "from rgiiia a, passed.iatrana_all b where a.iicode =b.iicode and a.deleted =0 and a.status =0 and b.deleted=0 and b.status =0 " _
               & "and b.tltxcd ='3052' and b.micode = '501' and b.micode <> '000' and a.tradeacctno <> '501A990012' and a.tradeacctno like '501%' and brid ='" & BranchId3 & "'" _
               & "and txdate >= to_date('" & v_strFromDate & " 00:00:00','DD/MM/YYYY hh24:mi:ss') and txdate <= to_date('" & v_strToDate & " 23:59:00','DD/MM/YYYY hh24:mi:ss')  ) a "

            v_strSQL = v_strSQL & " Union all select a.* from ( " _
               & " select substr(a.tradeacctno,-3) bankcodevsd,a.sbv_micode bankcodenhnn, b.sicode seccodevsd,decode (b.operator,'-',2,1) trans_type, " _
               & " b.namt quantity, b.txnum trancode , decode(b.operator,'-',18,3) type  " _
               & "from rgiiia a, iatran b where a.iicode =b.iicode and a.deleted =0 and a.status =0 and b.deleted=0 and b.status =0 " _
               & " and b.tltxcd ='3148' and b.micode = '501' and b.micode <> '000' and a.tradeacctno <> '501A990012' and a.tradeacctno like '501%' and brid ='" & BranchId3 & "'" _
               & "and txdate >= to_date('" & v_strFromDate & " 00:00:00','DD/MM/YYYY hh24:mi:ss') and txdate <= to_date('" & v_strToDate & " 23:59:00','DD/MM/YYYY hh24:mi:ss') " _
                       & "union all " _
               & "select substr(a.tradeacctno,-3) bankcodevsd,a.sbv_micode bankcodenhnn, b.sicode seccodevsd,decode (b.operator,'-',2,1) trans_type, " _
               & "b.namt quantity, b.txnum trancode , decode(b.operator,'-',18,3) type  " _
               & "from rgiiia a, passed.iatrana_all b where a.iicode =b.iicode and a.deleted =0 and a.status =0 and b.deleted=0 and b.status =0 " _
               & "and b.tltxcd ='3148' and b.micode = '501' and b.micode <> '000' and a.tradeacctno <> '501A990012' and a.tradeacctno like '501%' and brid ='" & BranchId3 & "'" _
               & "and txdate >= to_date('" & v_strFromDate & " 00:00:00','DD/MM/YYYY hh24:mi:ss') and txdate <= to_date('" & v_strToDate & " 23:59:00','DD/MM/YYYY hh24:mi:ss')  ) a "

            v_strSQL = v_strSQL & " Union all select a.* from ( " _
               & " select substr(a.tradeacctno,-3) bankcodevsd,a.sbv_micode bankcodenhnn, b.sicode seccodevsd,decode (b.operator,'-',1,2) trans_type, " _
               & " b.namt quantity, b.txnum trancode , decode(b.operator,'-',3,18) type  " _
               & "from rgiiia a, iatran b where a.iicode =b.iicode and a.deleted =0 and a.status =0 and b.deleted=0 and b.status =0 " _
               & " and b.tltxcd ='3148' and b.micode = '501' and b.micode <> '000' and a.tradeacctno <> '501A990012' and a.tradeacctno like '501%' and brid ='" & BranchId3 & "'" _
               & "and txdate >= to_date('" & v_strFromDate & " 00:00:00','DD/MM/YYYY hh24:mi:ss') and txdate <= to_date('" & v_strToDate & " 23:59:00','DD/MM/YYYY hh24:mi:ss') " _
                       & "union all " _
               & "select substr(a.tradeacctno,-3) bankcodevsd,a.sbv_micode bankcodenhnn, b.sicode seccodevsd,decode (b.operator,'-',1,2) trans_type, " _
               & "b.namt quantity, b.txnum trancode , decode(b.operator,'-',3,18) type  " _
               & "from rgiiia a, passed.iatrana_all b where a.iicode =b.iicode and a.deleted =0 and a.status =0 and b.deleted=0 and b.status =0 " _
               & "and b.tltxcd ='3148' and b.micode = '501' and b.micode <> '000' and a.tradeacctno <> '501A990012' and a.tradeacctno like '501%' and brid ='" & BranchId3 & "'" _
               & "and txdate >= to_date('" & v_strFromDate & " 00:00:00','DD/MM/YYYY hh24:mi:ss') and txdate <= to_date('" & v_strToDate & " 23:59:00','DD/MM/YYYY hh24:mi:ss')  ) a "

            v_strSQL = v_strSQL & " Union all select a.* from ( " _
               & " select substr(a.tradeacctno,-3) bankcodevsd,a.sbv_micode bankcodenhnn, b.sicode seccodevsd,decode (b.operator,'-',1,2) trans_type, " _
               & " b.namt quantity, b.txnum trancode , decode(b.operator,'-',3,18) type  " _
               & "from rgiiia a, iatran b where a.iicode =b.iicode and a.deleted =0 and a.status =0 and b.deleted=0 and b.status =0 " _
               & " and b.tltxcd ='3048' and b.micode = '501' and a.tradeacctno <> '501A990012' and a.tradeacctno like '501%' and brid ='" & BranchId3 & "'" _
               & "and txdate >= to_date('" & v_strFromDate & " 00:00:00','DD/MM/YYYY hh24:mi:ss') and txdate <= to_date('" & v_strToDate & " 23:59:00','DD/MM/YYYY hh24:mi:ss') " _
                       & "union all " _
               & "select substr(a.tradeacctno,-3) bankcodevsd,a.sbv_micode bankcodenhnn, b.sicode seccodevsd,decode (b.operator,'-',1,2) trans_type, " _
               & "b.namt quantity, b.txnum trancode , decode(b.operator,'-',3,18) type  " _
               & "from rgiiia a, passed.iatrana_all b where a.iicode =b.iicode and a.deleted =0 and a.status =0 and b.deleted=0 and b.status =0 " _
               & "and b.tltxcd ='3048' and b.micode = '501' and a.tradeacctno <> '501A990012' and a.tradeacctno like '501%' and brid ='" & BranchId3 & "'" _
               & "and txdate >= to_date('" & v_strFromDate & " 00:00:00','DD/MM/YYYY hh24:mi:ss') and txdate <= to_date('" & v_strToDate & " 23:59:00','DD/MM/YYYY hh24:mi:ss')  ) a " _
               & " ) a, rgsi b where a.seccodevsd = b.sicode and b.status =0 and b.deleted =0 and b.type = 1 and a.trans_type = 1 and a.trancode not in (select distinct trancode from cds_tranlog_nhnn where deleted = 0 and status = 0)"
            'bangpv: lay ten file day len server
            'v_strFileNHNN = "CDS_TRANS_NHNN" & v_dCurrentDate_NHNN & "_" & DateTime.Now.ToString("HH:mm:ss").Replace(":", "") & ".xml"
            v_strObjMsg = BuildXMLObjMsg(Now.Date, m_BusLayer.CurrentTellerProfile.BranchId, Now.Date, _
                    m_BusLayer.CurrentTellerProfile.TellerId, CommonLibrary.gc_IsLocalMsg, CommonLibrary.gc_MsgTypeObj, _
                    OBJNAME_SY_AUTHENTICATION, CommonLibrary.gc_ActionInquiry, v_strSQL, "CDS_TRANS", "GetExportData", _
                    "", "", "", "", "", "", "", "", "", "", "", "", "", v_strFileNHNN)

            Dim v_strObjMsgTemp As String = v_strObjMsg
            v_strObjMsgTemp = Replace(v_strObjMsgTemp, " />", ">")
            Dim v_strXMLHeader As String = "<?xml version=""1.0"" standalone=""yes"" ?> "
            Dim v_strRootOpen = "<SATS_CDS_TRANS xmlns=""http://tempuri.org/SATS_CDS_TRANS.xsd"">"
            Dim v_strRootClose = "</SATS_CDS_TRANS>"

            Dim v_lngError As Long = Proxy.Message(v_strObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Return Nothing
            End If
            'code chuoi' 

            v_strObjMsg = Replace(v_strObjMsg, v_strObjMsgTemp, v_strRootOpen)
            v_strObjMsg = Replace(v_strObjMsg, "</ObjectMessage>", v_strRootClose)
            If Mid(v_strObjMsg, 1, Len("<ObjectMessage")) = "<ObjectMessage" Then
                'v_strObjMsg = v_strRootOpen & v_strRootClose
                'MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, "Không tồn tại dữ liệu hoặc dữ liệu đã được kết suất trong khoảng thời gian này!", "Không tồn tại dữ liệu hoặc dữ liệu đã được kết suất trong khoảng thời gian này!"), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Return Nothing
            End If
            v_strObjMsg = v_strXMLHeader & vbCrLf & v_strObjMsg
            v_xmlDocument.LoadXml(v_strObjMsg)

            Return v_xmlDocument

        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Private Function GetXMLMIData_NHNN() As Xml.XmlDocument
        Try
            Dim v_strSQL As String = ""
            Dim v_xmlDocument As New Xml.XmlDocument
            Dim v_strObjMsg As String

            ' Edited by Thanglv9 - 18/10/2012.Sua thanh BANKCODEVSD va them BANKCODENHNN theo cau truc file xml moi
            v_strSQL = "select substr(b.tradeacctno,-3) BANKCODEVSD,b.sbv_micode BANKCODENHNN,a.full_name BANKNAME from rgii a, rgiiia b " _
                    & " where a.iicode = b.iicode and b.tradeacctno like '501%'and b.tradeacctno <> '501A990012' " _
                    & "and a.status =0 and b.status =0 and a.deleted =0 and b.deleted =0"

            v_strObjMsg = BuildXMLObjMsg(Now.Date, m_BusLayer.CurrentTellerProfile.BranchId, Now.Date, _
                    m_BusLayer.CurrentTellerProfile.TellerId, CommonLibrary.gc_IsLocalMsg, CommonLibrary.gc_MsgTypeObj, _
                    OBJNAME_SY_AUTHENTICATION, CommonLibrary.gc_ActionInquiry, v_strSQL, "MEMBERS", "GetExportData")

            Dim v_strObjMsgTemp As String = v_strObjMsg
            v_strObjMsgTemp = Replace(v_strObjMsgTemp, " />", ">")
            Dim v_strXMLHeader As String = "<?xml version=""1.0"" standalone=""yes"" ?> "
            Dim v_strRootOpen = "<SATS_MEMBERS xmlns=""http://tempuri.org/SATS_MEMBERS.xsd"">"
            Dim v_strRootClose = "</SATS_MEMBERS>"

            Dim v_lngError As Long = Proxy.Message(v_strObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Return Nothing
            End If

            v_strObjMsg = Replace(v_strObjMsg, v_strObjMsgTemp, v_strRootOpen)
            v_strObjMsg = Replace(v_strObjMsg, "</ObjectMessage>", v_strRootClose)
            v_strObjMsg = v_strXMLHeader & vbCrLf & v_strObjMsg
            v_xmlDocument.LoadXml(v_strObjMsg)

            Return v_xmlDocument

        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Private Function GetXMLSIData_NHNN() As Xml.XmlDocument
        Try
            Dim v_strSQL As String = ""
            Dim v_xmlDocument As New Xml.XmlDocument
            Dim v_strObjMsg As String

            ' Edited by Thanglv9 - 18/10/2012.Sua thanh seccodevsd va them seccodenhnn theo cau truc file xml moi
            v_strSQL = "SELECT a.stock_type sectype, a.interest_period interestperiod," _
                         & "a.bond_period secperiod,DECODE (TO_CHAR (a.issuer_date, 'dd/mm/yyyy'),'01/01/1753', NULL,TO_CHAR (a.issuer_date, 'dd/mm/yyyy')) issuedate, DECODE (TO_CHAR (a.due_date, 'dd/mm/yyyy'),'01/01/1753', NULL,TO_CHAR (a.due_date, 'dd/mm/yyyy')) duedate," _
                         & "a.INTEREST_RATE INTERESTRATE,a.iscode issuercodevsd,b.ktp_issuer_code issuercodenhnn,a.sicode seccodevsd,a.sbv_sicode seccodenhnn,a.isin, " _
                         & "a.part_value, a.bond_period_unit,a.INT_RELEASE_MODE ,a.INTEREST_TYPE ,a.INTEREST_RATE_TYPE,a.INTEREST_PERIOD_ASSUME,DECODE (TO_CHAR (a.npaiddate1, 'dd/mm/yyyy'),'01/01/1753', NULL,TO_CHAR (a.npaiddate1, 'dd/mm/yyyy')) NPAIDDATE1 " _
                         & " FROM rgsi a,rgis_map_ktp b  where a.TYPE = 1 AND a.iscode = b.iscode" _
                         & " AND a.deleted = 0 AND a.status = 0 and b.deleted = 0 and b.status = 0" _
                         & " and a.BRID ='" & BranchId3 & "'"

            v_strObjMsg = BuildXMLObjMsg(Now.Date, m_BusLayer.CurrentTellerProfile.BranchId, Now.Date, _
                    m_BusLayer.CurrentTellerProfile.TellerId, CommonLibrary.gc_IsLocalMsg, CommonLibrary.gc_MsgTypeObj, _
                    OBJNAME_SY_AUTHENTICATION, CommonLibrary.gc_ActionInquiry, v_strSQL, "STOCKS", "GetExportData")

            Dim v_strObjMsgTemp As String = v_strObjMsg
            v_strObjMsgTemp = Replace(v_strObjMsgTemp, " />", ">")
            Dim v_strXMLHeader As String = "<?xml version=""1.0"" standalone=""yes"" ?> "
            Dim v_strRootOpen = "<SATS_STOCKS xmlns=""http://tempuri.org/SATS_STOCKS.xsd"">"
            Dim v_strRootClose = "</SATS_STOCKS>"

            Dim v_lngError As Long = Proxy.Message(v_strObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Return Nothing
            End If

            v_strObjMsg = Replace(v_strObjMsg, v_strObjMsgTemp, v_strRootOpen)
            v_strObjMsg = Replace(v_strObjMsg, "</ObjectMessage>", v_strRootClose)
            If Mid(v_strObjMsg, 1, Len("<ObjectMessage")) = "<ObjectMessage" Then
                'v_strObjMsg = v_strRootOpen & v_strRootClose
                'MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, "Không tồn tại dữ liệu hoặc dữ liệu đã được kết suất trong khoảng thời gian này!", "Không tồn tại dữ liệu hoặc dữ liệu đã được kết suất trong khoảng thời gian này!"), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Return Nothing
            End If
            v_strObjMsg = v_strXMLHeader & vbCrLf & v_strObjMsg
            v_xmlDocument.LoadXml(v_strObjMsg)

            Return v_xmlDocument

        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Private Function GetXMLISData_NHNN() As Xml.XmlDocument
        Try
            Dim v_strSQL As String = ""
            Dim v_xmlDocument As New Xml.XmlDocument
            Dim v_strObjMsg As String

            ' Edited by Thanglv9 - 18/10/2012.Sua thanh ISSUERCODEVSD theo cau truc file xml moi
            v_strSQL = "select distinct a.short_name ISSUERNAME, a.iscode ISSUERCODEVSD from rgis a, rgsi b where a.deleted =0 and a.status =0 and" _
                        & " a.iscode=b.iscode and b.type =1 and b.status =0 and b.deleted=0 and b.brid ='" & BranchId3 & "'"

            v_strObjMsg = BuildXMLObjMsg(Now.Date, m_BusLayer.CurrentTellerProfile.BranchId, Now.Date, _
                    m_BusLayer.CurrentTellerProfile.TellerId, CommonLibrary.gc_IsLocalMsg, CommonLibrary.gc_MsgTypeObj, _
                    OBJNAME_SY_AUTHENTICATION, CommonLibrary.gc_ActionInquiry, v_strSQL, "ISSUERS", "GetExportData")

            Dim v_strObjMsgTemp As String = v_strObjMsg
            v_strObjMsgTemp = Replace(v_strObjMsgTemp, " />", ">")
            Dim v_strXMLHeader As String = "<?xml version=""1.0"" standalone=""yes"" ?> "
            Dim v_strRootOpen = "<SATS_ISSUERS xmlns=""http://tempuri.org/SATS_ISSUERS.xsd"">"
            Dim v_strRootClose = "</SATS_ISSUERS>"

            Dim v_lngError As Long = Proxy.Message(v_strObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Return Nothing
            End If

            v_strObjMsg = Replace(v_strObjMsg, v_strObjMsgTemp, v_strRootOpen)
            v_strObjMsg = Replace(v_strObjMsg, "</ObjectMessage>", v_strRootClose)
            v_strObjMsg = v_strXMLHeader & vbCrLf & v_strObjMsg
            v_xmlDocument.LoadXml(v_strObjMsg)

            Return v_xmlDocument

        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Private Function GetXMLISData() As Xml.XmlDocument
        Try
            Dim v_strSQL As String = ""
            Dim v_xmlDocument As New Xml.XmlDocument
            Dim v_strObjMsg As String

            v_strSQL = "SELECT decode(issuer_id, null, AUTOID, issuer_id) ISSUER_ID, ISNAME NAME, SHORT_NAME, ISCODE CODE, ISTYPE TYPE, ADDRESS, PHONE, FAX, '' TELEX," _
                        & "CAPITAL_RULE CAPITAL1, CAPITAL_RULE CAPITAL2, DELETED, NCOUPONDATEPERIOD FROM RGIS WHERE DELETED = 0 AND STATUS = 0 " _
                        & " AND ISCODE IN (select ISCODE FROM RGSI WHERE STATUS =0 AND BRID ='" & BranchId & "')"

            v_strObjMsg = BuildXMLObjMsg(Now.Date, m_BusLayer.CurrentTellerProfile.BranchId, Now.Date, _
                    m_BusLayer.CurrentTellerProfile.TellerId, CommonLibrary.gc_IsLocalMsg, CommonLibrary.gc_MsgTypeObj, _
                    OBJNAME_SY_AUTHENTICATION, CommonLibrary.gc_ActionInquiry, v_strSQL, "ISSUERS", "GetExportData")

            Dim v_strObjMsgTemp As String = v_strObjMsg
            v_strObjMsgTemp = Replace(v_strObjMsgTemp, " />", ">")
            Dim v_strXMLHeader As String = "<?xml version=""1.0"" standalone=""yes"" ?> "
            Dim v_strRootOpen = "<SATS_ISSUERS xmlns=""http://tempuri.org/SATS_ISSUERS.xsd"">"
            Dim v_strRootClose = "</SATS_ISSUERS>"

            Dim v_lngError As Long = Proxy.Message(v_strObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Return Nothing
            End If
            'Start Myvq
            'mv_strRespond = mv_oProxy.FileCA(v_strObjMsg, mv_strBranchId, mv_strFileName, v_dCurrentDate)
            'mv_strFileName = ""
            'End Myvq

            If v_strObjMsgTemp = Replace(v_strObjMsg, " />", ">") Then
                v_strObjMsg = v_strRootOpen & vbCrLf & v_strRootClose
                v_strObjMsg = v_strXMLHeader & vbCrLf & v_strObjMsg
            Else
                v_strObjMsg = Replace(v_strObjMsg, v_strObjMsgTemp, v_strRootOpen)
                v_strObjMsg = Replace(v_strObjMsg, "</ObjectMessage>", v_strRootClose)
                v_strObjMsg = v_strXMLHeader & vbCrLf & v_strObjMsg
            End If
		v_xmlDocument.LoadXml(v_strObjMsg)
            Return v_xmlDocument

        Catch ex As Exception
            Throw ex
        End Try
    End Function
    
    Private Function GetXMLSIData() As Xml.XmlDocument
        Try
            Dim v_strSQL As String = ""
            Dim v_xmlDocument As New Xml.XmlDocument
            Dim v_strObjMsg As String
            If BranchId = "0006" Or BranchId = "0004" Or BranchId = "0005" Then
                v_strSQL = "select decode (b.issuer_id, null, b.autoid, b.issuer_id) ISSUER_ID, /*a.autoid*/ decode(a.stock_id, null,a.autoid,a.stock_id) STOCK_ID, " _
                        & " a.sicode CODE, a.stock_name NAME, a.sicode SHORT_NAME, a.type TYPE,  " _
                        & "to_char(a.ISSUER_DATE,'dd/MM/yyyy') ACTION_DATE, a.part_value PAR_VALUE, '' BASIC_PRICE, '' ISSUING_PRICE, c.TOTAL_QTTY, " _
                        & "to_char(a.DUE_DATE,'dd/MM/yyyy') DUE_DATE, 0 CONTROL_STATUS, 1 STATUS, " _
                        & "to_char(a.ISSUER_DATE,'dd/MM/yyyy') ISSUE_DATE, a.STOCK_TYPE, r.balance CURRENT_ROOM_QTTY, nvl(a.bond_period_unit,'') bond_period_unit, " _
                        & "decode(a.brid,'0001',2,'0002',2,3) STATUS_LISTING, '' TIME_ISSUE, a.STOCK_TYPE STOCKS_TYPE, nvl(d.balance,0) CURRENT_HOLDING_QTTY, a.rarate FOREIGN_RATE, " _
                        & "a.EFFECTIVE_DATE LISTING_DATE, a.DELETED, Decode (a.brid,'0001',2,'0002',2,'0003',2,3) FLOOR_CODE, decode(a.brid,'0006',null,a.INTEREST_RATE) INTEREST_RATE, a.bond_period BOND_PERIOD, decode(a.brid,'0006',null,a.interest_period) interest_period, nvl(INT_RELEASE_MODE,0) paidmethod, " _
                        & "to_char(a.npaiddate1,'dd/MM/yyyy') NPAIDDATE1, to_char(a.npaiddate2,'dd/MM/yyyy') NPAIDDATE2, to_char(a.npaiddate3,'dd/MM/yyyy') NPAIDDATE3, to_char(a.npaiddate4,'dd/MM/yyyy') NPAIDDATE4, '' NCOUPONDATEPERIOD,a.INTEREST_TYPE,a.INTEREST_RATE_TYPE,a.INTEREST_PERIOD_ASSUME " _
                        & "from rgsi a, rgis b,  " _
                        & "( select substr(a.maacctno, 12, length(a.maacctno)- 11) sicode, sum(a.balance) TOTAL_QTTY from mamast a " _
                        & "where  a.deleted = 0 and a.status = 0 group by substr(a.maacctno, 12, length(a.maacctno)- 11) ) c, " _
                        & "ramast r,  " _
                        & " (select  SUBSTR (maacctno, 12, LENGTH (maacctno) - 11) sicode, sum(balance) balance " _
                        & " from mamast " _
                        & " WHERE deleted = 0 " _
                        & " AND status = 0 and substr(maacctno,0,3)='000' and substr(maacctno,9,1) in ('3','5','9') " _
                        & " group by SUBSTR (maacctno, 12, LENGTH (maacctno) - 11))        d " _
                        & "where a.iscode = b.iscode and a.sicode = d.sicode(+) " _
                        & "and a.sicode = c.sicode and a.sicode = r.sicode(+) and r.deleted=0 and r.status = 0 " _
                        & "and a.deleted = 0 and a.status = 0 " _
                        & "and b.deleted = 0 and b.status = 0 " _
                        & "and a.brid = '" & BranchId & "'"

            Else
                v_strSQL = "select decode (b.issuer_id, null, b.autoid, b.issuer_id) ISSUER_ID, /*a.autoid*/ decode(a.stock_id, null,a.autoid,a.stock_id) STOCK_ID, " _
                    & " a.sicode CODE, a.stock_name NAME, a.sicode SHORT_NAME, a.type TYPE,  " _
                    & "to_char(a.ISSUER_DATE,'dd/MM/yyyy') ACTION_DATE, a.part_value PAR_VALUE, '' BASIC_PRICE, '' ISSUING_PRICE, c.TOTAL_QTTY, " _
                    & "to_char(a.DUE_DATE,'dd/MM/yyyy') DUE_DATE, 0 CONTROL_STATUS, 1 STATUS, " _
                    & "to_char(a.ISSUER_DATE,'dd/MM/yyyy') ISSUE_DATE, a.STOCK_TYPE, r.balance CURRENT_ROOM_QTTY, " _
                    & "decode(a.brid,'0001',2,'0002',2,3) STATUS_LISTING, '' TIME_ISSUE, a.STOCK_TYPE STOCKS_TYPE, nvl(d.balance,0) CURRENT_HOLDING_QTTY, a.rarate FOREIGN_RATE, " _
                    & "a.EFFECTIVE_DATE LISTING_DATE, a.DELETED, Decode (a.brid,'0001',2,'0002',2,'0003',2,3) FLOOR_CODE, a.INTEREST_RATE, a.bond_period BOND_PERIOD, a.interest_period, nvl(INT_RELEASE_MODE,0) paidmethod, " _
                    & "to_char(a.npaiddate1,'dd/MM/yyyy') NPAIDDATE1, to_char(a.npaiddate2,'dd/MM/yyyy') NPAIDDATE2, to_char(a.npaiddate3,'dd/MM/yyyy') NPAIDDATE3, to_char(a.npaiddate4,'dd/MM/yyyy') NPAIDDATE4, '' NCOUPONDATEPERIOD " _
                    & "from rgsi a, rgis b,  " _
                    & "( select substr(a.maacctno, 12, length(a.maacctno)- 11) sicode, sum(a.balance) TOTAL_QTTY from mamast a " _
                    & "where  a.deleted = 0 and a.status = 0 group by substr(a.maacctno, 12, length(a.maacctno)- 11) ) c, " _
                    & "ramast r,  " _
                    & " (select  SUBSTR (maacctno, 12, LENGTH (maacctno) - 11) sicode, sum(balance) balance " _
                    & " from mamast " _
                    & " WHERE deleted = 0 " _
                    & " AND status = 0 and substr(maacctno,0,3)='000' and substr(maacctno,9,1) in ('3','5','9') " _
                    & " group by SUBSTR (maacctno, 12, LENGTH (maacctno) - 11))        d " _
                    & "where a.iscode = b.iscode and a.sicode = d.sicode(+) " _
                    & "and a.sicode = c.sicode and a.sicode = r.sicode(+) and r.deleted=0 and r.status = 0 " _
                    & "and a.deleted = 0 and a.status = 0 " _
                    & "and b.deleted = 0 and b.status = 0 " _
                    & "and a.brid = '" & BranchId & "'"

            End If
            
            v_strObjMsg = BuildXMLObjMsg(Now.Date, m_BusLayer.CurrentTellerProfile.BranchId, Now.Date, _
                    m_BusLayer.CurrentTellerProfile.TellerId, CommonLibrary.gc_IsLocalMsg, CommonLibrary.gc_MsgTypeObj, _
                    OBJNAME_SY_AUTHENTICATION, CommonLibrary.gc_ActionInquiry, v_strSQL, "STOCKS", "GetExportData")

            Dim v_strObjMsgTemp As String = v_strObjMsg
            v_strObjMsgTemp = Replace(v_strObjMsgTemp, " />", ">")
            Dim v_strXMLHeader As String = "<?xml version=""1.0"" standalone=""yes"" ?> "
            Dim v_strRootOpen = "<SATS_STOCKS xmlns=""http://tempuri.org/SATS_STOCKS.xsd"">"
            Dim v_strRootClose = "</SATS_STOCKS>"

            Dim v_lngError As Long = Proxy.Message(v_strObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Return Nothing
            End If

            'Start Myvq
            'mv_strRespond = mv_oProxy.FileCA(v_strObjMsg, mv_strBranchId, mv_strFileName, v_dCurrentDate)
            'mv_strFileName = ""
            'End Myvq
            If v_strObjMsgTemp = Replace(v_strObjMsg, " />", ">") Then
                v_strObjMsg = v_strRootOpen & vbCrLf & v_strRootClose
                v_strObjMsg = v_strXMLHeader & vbCrLf & v_strObjMsg
            Else
                v_strObjMsg = Replace(v_strObjMsg, v_strObjMsgTemp, v_strRootOpen)
                v_strObjMsg = Replace(v_strObjMsg, "</ObjectMessage>", v_strRootClose)
                v_strObjMsg = v_strXMLHeader & vbCrLf & v_strObjMsg
            End If
            
            v_xmlDocument.LoadXml(v_strObjMsg)

            Return v_xmlDocument

        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Private Function GetXMLMIData() As Xml.XmlDocument
        Try
            Dim v_strSQL As String = ""
            Dim v_xmlDocument As New Xml.XmlDocument
            Dim v_strObjMsg As String


            v_strSQL = "SELECT decode(a.member_id,null,A.AUTOID,member_id) MEMBER_ID, A.MICODE CODE, A.NAME, A.SHORT_NAME, A.TYPE, A.BANK_ACCOUNT ACCOUNT_NO, decode(A.STATUS, 0,1,1,2) STATUS, " _
                    & "decode (A.BORF_FLAG,5,2,1) DORF_FLAG, A.BORF_FLAG, A.DELETED, '' N, A.CODE_TRADE, A.ADDRESS, A.PHONE, A.FAX, '' TELEX, A.CAPITAL, A.CAPITAL_RULE, " _
                    & " 0 FUND_AMOUNT FROM RGMI A WHERE DELETED =0 AND STATUS =0  and micode <>'000' "

            v_strObjMsg = BuildXMLObjMsg(Now.Date, m_BusLayer.CurrentTellerProfile.BranchId, Now.Date, _
                    m_BusLayer.CurrentTellerProfile.TellerId, CommonLibrary.gc_IsLocalMsg, CommonLibrary.gc_MsgTypeObj, _
                    OBJNAME_SY_AUTHENTICATION, CommonLibrary.gc_ActionInquiry, v_strSQL, "MEMBERS", "GetExportData")

            Dim v_strObjMsgTemp As String = v_strObjMsg
            v_strObjMsgTemp = Replace(v_strObjMsgTemp, " />", ">")
            Dim v_strXMLHeader As String = "<?xml version=""1.0"" standalone=""yes"" ?> "
            Dim v_strRootOpen = "<SATS_MEMBERS xmlns=""http://tempuri.org/SATS_MEMBERS.xsd"">"
            Dim v_strRootClose = "</SATS_MEMBERS>"

            Dim v_lngError As Long = Proxy.Message(v_strObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Return Nothing
            End If

            'Start Myvq
            'mv_strRespond = mv_oProxy.FileCA(v_strObjMsg, mv_strBranchId, mv_strFileName, v_dCurrentDate)
            'mv_strFileName = ""
            'End Myvq

            v_strObjMsg = Replace(v_strObjMsg, v_strObjMsgTemp, v_strRootOpen)
            v_strObjMsg = Replace(v_strObjMsg, "</ObjectMessage>", v_strRootClose)
            v_strObjMsg = v_strXMLHeader & vbCrLf & v_strObjMsg
            v_xmlDocument.LoadXml(v_strObjMsg)

            Return v_xmlDocument

        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Private Function GetXMLCWData() As Xml.XmlDocument
        Try
            Dim v_strSQL As String = ""
            Dim v_xmlDocument As New Xml.XmlDocument
            Dim v_strObjMsg As String

            v_strSQL = " WITH tmp_rgsi AS (SELECT a.*, SUBSTR(a.sicode,2,3) code  FROM rgsi a WHERE TYPE =4 AND status =0 AND deleted =0), " _
                 & "  tmp_rgsi1 AS (SELECT b.* " _
                 & "                          FROM tmp_Rgsi a, rgsi b " _
                 & "                         WHERE b.sicode = a.code AND  b.status =0 AND b.deleted =0) " _
                 & "         SELECT   DISTINCT  a.micode MemberCode,a.tradeacctno, a.business_no RegisterNo,a.business_date RegisterDate, " _
                 & "        a.sicode CWName , NVL(a.balance,0) CWBalance, NVL(b.sicode, a.code) BaseStockName  ,NVL(b.balance,0) BaseStockBalance         " _
                 & "        FROM (SELECT a.*, c.NAME, c.business_no, c.business_date  " _
                 & "                FROM  " _
                 & "                (  " _
                 & "                    select a.iaacctno, b.tradeacctno, b.sicode, b.micode, b.iicode, a.typeno, a.odrno, a.balance, b.status, a.deleted, a.debit, " _
                 & "                    a.credit, a.autoid, b.brid,a.tmpid, a.issuertype, a.date_created , b.code from " _
                 & "                    (select b.sicode, b.brid, b.status, b.code, b.iscode, y.iicode iicode, z.micode micode, w.tradeacctno " _
                 & "                    from tmp_rgsi b, rgis x, rgii y, rgmi z, (select distinct micode, iicode, tradeacctno from rgiiia) w " _
                 & "                    where(b.iscode = x.iscode And x.bussiness_no = y.cardno And x.bussiness_date = y.carddate)" _
                 & "                    and x.bussiness_no = z.business_no and x.bussiness_date = z.business_date and z.micode = w.micode and y.iicode = w.iicode) b " _
                 & "                    left join iamast a " _
                 & "                    on (a.sicode = b.sicode and a.brid = b.brid and decode(a.status,5,b.status, a.status) = b.status and a.micode = b.micode and a.micode <> '000' " _
                 & "                    and a.typeno in ('012111','012191') and a.deleted = 0 and a.status in (0,1,4,5)) " _
                 & "             ) a, RGMI c " _
                 & "        WHERE a.micode = c.micode  " _
                 & "   ) a  " _
                 & "     LEFT JOIN ( " _
                 & "     SELECT a.*, c.name " _
                 & "         FROM  " _
                 & "        ( " _
                 & "           select a.* from iamast a, tmp_rgsi1 b  " _
                 & "           where a.deleted = 0 and a.status =0  and a.typeno in ('012111','012191') " _
                 & "          AND a.sicode = b.sicode  AND micode <>'000' " _
                 & "       ) a, RGMI c  " _
                 & "     WHERE a.micode = c.micode " _
                 & "    ) b  " _
                 & "       ON (  a.code =b.sicode and a.status = b.status AND a.micode = b.micode  AND a.tradeacctno = b.tradeacctno) " _
                 & "  WHERE (NVL(a.balance,0) > 0 or NVL(b.balance,0) > 0) "

            v_strObjMsg = BuildXMLObjMsg(Now.Date, m_BusLayer.CurrentTellerProfile.BranchId, Now.Date, _
                    m_BusLayer.CurrentTellerProfile.TellerId, CommonLibrary.gc_IsLocalMsg, CommonLibrary.gc_MsgTypeObj, _
                    OBJNAME_SY_AUTHENTICATION, CommonLibrary.gc_ActionInquiry, v_strSQL, "Broker", "GetExportData")

            Dim v_strObjMsgTemp As String = v_strObjMsg
            v_strObjMsgTemp = Replace(v_strObjMsgTemp, " />", ">")
            Dim v_strXMLHeader As String = "<?xml version=""1.0"" encoding=""UTF-8"" ?> "
            Dim v_strRootOpen = "<CoverWarrant xmlns=""http://tempuri.org/CoverWarrant.xsd"">"
            Dim v_strRootClose = "</CoverWarrant>"

            Dim v_lngError As Long = Proxy.Message(v_strObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Return Nothing
            End If

            'Start Myvq
            'mv_strRespond = mv_oProxy.FileCA(v_strObjMsg, mv_strBranchId, mv_strFileName, v_dCurrentDate)
            'mv_strFileName = ""
            'End Myvq

            v_strObjMsg = Replace(v_strObjMsg, v_strObjMsgTemp, v_strRootOpen)
            v_strObjMsg = Replace(v_strObjMsg, "</ObjectMessage>", v_strRootClose)
            v_strObjMsg = v_strXMLHeader & vbCrLf & v_strObjMsg
            v_xmlDocument.LoadXml(v_strObjMsg)

            Return v_xmlDocument

        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Private Function GetXMLRoomData() As Xml.XmlDocument
        Try
            Dim v_strSQL As String = ""
            Dim v_xmlDocument As New Xml.XmlDocument
            Dim v_strObjMsg As String

            
            v_strSQL = "SELECT r.AUTOID FRGROOM_ID, r.SICODE CODE, " _
                        & "(case when (nvl(c.listed_qtty,0) = 0 or  nvl(c.listed_qtty,0) >=nvl(r.balance,0)) then " _
                        & "(case when NVL (r.balance, 0)>=0 then NVL (r.balance, 0) else 0 end) " _
                        & " else c.listed_qtty end " _
                        & " ) CURRENT_ROOM_QTTY, " _
                        & "nvl(C.TOTAL_QTTY,0) TOTAL_QTTY, nvl(C.CURRENT_QTTY,0) CURRENT_QTTY, " _
                        & "nvl(D.BALANCE,0) CURRENT_HOLDING_QTTY, C.STOCK_ID, '" & v_dtIndex & "' FRGROOM_DATE " _
                        & "FROM RAMAST r, ( " _
                        & "select b.sicode, b.listed_qtty , b.stock_id, (b.total_qtty+nvl(a.msgamt,0)) total_qtty, (nvl(a.current_qtty,0)+b.current_qtty ) current_qtty from " _
                        & " (SELECT b.sicode, sum(b.msgamt) msgamt, b.brid, ROUND(sum(b.msgamt) * max(c.rarate) * 0.01) current_qtty " _
                        & " FROM saissue a, tllog b, rgsi c WHERE a.autoid = b.autoid " _
                        & "            AND a.dedate = get_trade_date(TO_DATE ('" & v_dCurrentDate & "', 'dd/mm/yyyy') + 1, '" & BranchId & "', '+')" _
                        & " AND b.status = 3  AND b.deleted = 0 and c.type<>1  and c.deleted =0  AND b.brid = '" & BranchId & "'  and b.brid = c.brid " _
                        & "       AND b.sicode = c.sicode group by b.sicode, b.brid) a " _
                        & "      RIGHT OUTER JOIN " _
                        & "(SELECT B.SICODE, max(b.listed_qtty) listed_qtty, decode(max(stock_id),null,MAX(B.AUTOID),max(b.stock_id)) STOCK_ID,SUM(nvl(A.BALANCE,0)) TOTAL_QTTY,  " _
                        & "round(SUM(nvl(A.BALANCE,0))* MAX(B.RARATE)/100) CURRENT_QTTY FROM  RGSI B left join MAMAST A on (a.sicode = b.sicode) " _
                        & "WHERE( b.EFFECTIVE_DATE <=get_t_plus(to_date('" & v_dCurrentDate & "','DD/MM/YYYY'),'" & BranchId & "',1) " _
                        & "And nvl(a.deleted,0)=0 and nvl(a.status,0) =0  and B.DELETED = 0 AND B.STATUS=0 and b.brid ='" & BranchId & "') " _
                        & "GROUP BY B.SICODE) b on a.sicode= b.sicode ) C, " _
                        & " (select   sicode, sum(balance) balance " _
                        & " from mamast WHERE deleted = 0 " _
                        & " AND status = 0 and micode='000' and substr(maacctno,9,1) in ('3','5','9') " _
                        & " group by sicode )      d " _
                        & "WHERE r.DELETED = 0 AND r.STATUS = 0" _
                        & " AND r.BRID = '" & BranchId & "' " _
                        & " and r.sicode = c.sicode(+)" _
                        & " and r.sicode = d.sicode(+) " _
                        & " and r.sicode not in (select sicode from rgsi where brid ='" & BranchId & "' and EFFECTIVE_DATE >  get_t_plus(to_date('" & v_dCurrentDate & "','DD/MM/YYYY'),'" & BranchId & "',1) and status = 0 and deleted = 0)"
            'bangpv
            'new sql
            v_strSQL = "with tmp1 as " _
                        & "(SELECT c.sicode, SUM (b.msgamt) msgamt,c.brid, " _
                        & "       /*ROUND*/ (SUM (b.msgamt) * MAX (c.rarate) * 0.01) current_qtty " _
                        & "                 FROM saissue a, tllog b, rgsi c " _
                        & "                WHERE a.autoid = b.autoid " _
                        & "                      AND a.dedate = get_trade_date (TO_DATE ('" & v_dCurrentDate & "','dd/mm/yyyy') + 1, '0001','+') " _
                        & "                      AND b.status = 3 " _
                        & "                      AND b.deleted = 0 " _
                        & "                      and c.status =0 /*AND c.TYPE <> 1*/ " _
                        & "                      AND c.deleted = 0 " _
                        & "                      AND c.brid not in ('0004','0005','0006') " _
                        & "                      AND c.brid = '" & BranchId & "' " _
                        & "                      /*AND b.brid = c.brid */" _
                        & "                      AND c.sicode =(case when b.tltxcd ='6043' and lpad (b.col_value05,2,'0') in ('08','09')" _
                        & "                     then b.cosicode else b.sicode end)   " _
                        & "               GROUP BY c.sicode, c.brid), " _
                        & " tmp2 as " _
                        & " (SELECT a.SICODE, SUM (A.BALANCE) BALANCE " _
                        & "                     FROM MAMAST A " _
                        & "                    WHERE (    A.DELETED = 0 " _
                        & "                           AND A.STATUS = 0 " _
                        & "                          AND a.brid = '" & BranchId & "') " _
                        & "                   GROUP BY a.SICODE), " _
                        & " tmp3 as " _
                        & "    (SELECT B.SICODE, " _
                        & "                          MAX (b.listed_qtty) listed_qtty, " _
                        & "                          DECODE (MAX (stock_id), " _
                        & "                             NULL, MAX (B.AUTOID), " _
                        & "                             MAX (b.stock_id)) " _
                        & "                             STOCK_ID, " _
                        & "                          SUM (NVL (A.BALANCE, 0)) TOTAL_QTTY, " _
                        & "                          /*ROUND*/ (SUM (NVL (A.BALANCE, 0)) * MAX (B.RARATE) / 100 " _
                        & "                          ) " _
                        & "                             CURRENT_QTTY " _
                        & "                     FROM    RGSI B " _
                        & "                          LEFT JOIN " _
                        & "                             tmp2 A " _
                        & "                          ON (a.sicode = b.sicode) " _
                        & "                    WHERE (b.EFFECTIVE_DATE <= " _
                        & "                             get_t_plus (TO_DATE ('" & v_dCurrentDate & "', 'DD/MM/YYYY'),'0001', 1) " _
                        & "                           AND B.DELETED = 0 " _
                        & "                           AND B.STATUS = 0 " _
                        & "                           AND b.brid = '" & BranchId & "') " _
                        & "                   GROUP BY B.SICODE), " _
                        & " tmp4 as " _
                        & " (SELECT b.sicode," _
                        & "               b.listed_qtty," _
                        & "               b.stock_id," _
                        & "               (b.total_qtty + NVL (a.msgamt, 0)) total_qtty," _
                        & "               floor(NVL (a.current_qtty, 0) + b.current_qtty) current_qtty" _
                        & "          FROM    tmp1 a" _
                        & "               RIGHT OUTER JOIN" _
                        & "                  tmp3 b" _
                        & "               ON a.sicode = b.sicode)      " _
                        & " select a.* from ( " _
                        & " SELECT r.AUTOID FRGROOM_ID," _
                        & "       r.SICODE CODE," _
                        & "       (CASE " _
                        & "           WHEN (NVL (c.listed_qtty, 0) = 0" _
                        & "                 OR NVL (c.listed_qtty, 0) >= NVL (r.balance, 0))" _
                        & "           THEN" _
                        & "              (CASE" _
                        & "                  WHEN NVL (r.balance, 0) >= 0 THEN NVL (r.balance, 0) " _
                        & "                  ELSE 0 " _
                        & "               END) " _
                        & "           ELSE " _
                        & "              c.listed_qtty " _
                        & "        END)" _
                        & "          CURRENT_ROOM_QTTY," _
                        & "       NVL (C.TOTAL_QTTY, 0)  TOTAL_QTTY, " _
                        & "       NVL (C.CURRENT_QTTY, 0) CURRENT_QTTY, " _
                        & "       NVL (D.BALANCE, 0) CURRENT_HOLDING_QTTY, C.STOCK_ID, '" & v_dtIndex & "' FRGROOM_DATE " _
                        & "  FROM RAMAST r, " _
                        & "       tmp4 C," _
                        & "       (SELECT sicode, SUM (balance) balance " _
                        & "          FROM mamast" _
                        & "         WHERE     deleted = 0 " _
                        & "               AND status = 0 " _
                        & "               AND micode = '000' " _
                        & "               AND SUBSTR (maacctno, 9, 1) IN ('3', '5', '9') " _
                        & "        GROUP BY sicode) d" _
                        & " WHERE     r.DELETED = 0 " _
                        & "       AND r.STATUS = 0 " _
                        & "       AND r.BRID = '" & BranchId & "' " _
                        & "       AND r.sicode = c.sicode(+) " _
                        & "       AND r.sicode = d.sicode(+) ) a, rgsi b " _
                        & " where a.CODE = b.sicode  and b.status =0 and b.brid ='" & BranchId & "' and b.deleted =0 " _
                        & "  and b.EFFECTIVE_DATE <=get_t_plus (TO_DATE ('" & v_dCurrentDate & "', 'DD/MM/YYYY'),'" & BranchId & "', 1 )"
            'end bangpv 
            v_strObjMsg = BuildXMLObjMsg(Now.Date, m_BusLayer.CurrentTellerProfile.BranchId, Now.Date, _
                    m_BusLayer.CurrentTellerProfile.TellerId, CommonLibrary.gc_IsLocalMsg, CommonLibrary.gc_MsgTypeObj, _
                    OBJNAME_SY_AUTHENTICATION, CommonLibrary.gc_ActionInquiry, v_strSQL, "FOREIGN_ROOM", "GetExportData")

            Dim v_strObjMsgTemp As String = v_strObjMsg
            v_strObjMsgTemp = Replace(v_strObjMsgTemp, " />", ">")
            Dim v_strXMLHeader As String = "<?xml version=""1.0"" standalone=""yes"" ?> "
            Dim v_strRootOpen = "<SATS_FOREIGN_ROOM xmlns=""http://tempuri.org/SATS_FOREIGN_ROOM.xsd"">"
            Dim v_strRootClose = "</SATS_FOREIGN_ROOM>"

            Dim v_lngError As Long = Proxy.Message(v_strObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Return Nothing
            Else
                'Start Myvq
                'mv_strRespond = mv_oProxy.FileCA(v_strObjMsg, mv_strBranchId, mv_strFileName, v_dCurrentDate)
                'mv_strFileName = ""
                'End Myvq
                'xu ly voi truong hop khong co du lieu 
                If v_strObjMsgTemp = Replace(v_strObjMsg, " />", ">") Then
                    v_strObjMsg = v_strRootOpen & vbCrLf & v_strRootClose
                    v_strObjMsg = v_strXMLHeader & vbCrLf & v_strObjMsg
                Else
                    v_strObjMsg = Replace(v_strObjMsg, v_strObjMsgTemp, v_strRootOpen)
                    v_strObjMsg = Replace(v_strObjMsg, "</ObjectMessage>", v_strRootClose)
                    v_strObjMsg = v_strXMLHeader & vbCrLf & v_strObjMsg
                End If

                v_xmlDocument.LoadXml(v_strObjMsg)

                Return v_xmlDocument

                v_strObjMsg = BuildXMLObjMsg(, BranchId, , Me.TellerId, gc_IsNotLocalMsg, gc_MsgTypeObj, OBJNAME_SY_AUTHENTICATION, gc_ActionInquiry, , , "UPDATE_ROOM_STATUS", , )
                'Dim v_lngError As String = Proxy.Message(v_strObjMsg)
                v_lngError = Proxy.Message(v_strObjMsg)
                If v_lngError <> ERR_SYSTEM_OK Then
                    MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                    Cursor = Cursors.Default
                    'MsgBox(v_strErrorMessage, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, Me.Text)
                    Return Nothing
                Else
                    'MsgBox(mv_ResourceManager.GetString("AddnewSuccessful"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                    Cursor = Cursors.Default
                End If
                '                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                '               Exit Sub

            End If

           
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Sub ExportRoomToTextFile(ByRef pv_blnChecked As Boolean, ByRef pv_strFilePath As String, ByRef pv_strFileName As String)
        Try
            Dim v_strSQL As String = ""
            Dim v_xmlDocument As New Xml.XmlDocument
            Dim v_nodeList As Xml.XmlNodeList
            Dim v_strObjMsg As String
            'Dim v_fTextFile As System.IO.File
            Dim v_swStreamWriter As System.IO.StreamWriter
            Dim v_strLine, v_strFLDNAME, v_strValue As String
            Dim v_strFileName As String = pv_strFilePath & pv_strFileName & v_dCurrentDate & ".txt"
            mv_strFileName = pv_strFileName & v_dCurrentDate & ".txt"
            Dim v_intAns As Integer
            ' Dim dtIndex As Date
            ''bangpv
            ''Lấy ngày hệ thống

            'v_strSQL = "SELECT a.VARVALUE FROM sysvar a where a.GRNAME='SYSTEM' and a.VARNAME= 'CURRDATE' and BRID='" & BranchId & "'"
            'v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLTX, gc_ActionInquiry, v_strSQL)
            'Dim v_lngError As Long = mv_BDSDelivery.Message(v_strObjMsg)
            'If v_lngError <> ERR_SYSTEM_OK Then
            '    MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
            '    Exit Sub
            'End If
            'v_xmlDocument.LoadXml(v_strObjMsg)
            'dtIndex = CDate(v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='VARVALUE']").InnerText)
            ''end bangpv 
            If CheckExistFile(pv_strFilePath, v_strFileName) Then
                v_intAns = MsgBox(Replace(mv_ResourceManager.GetString("FileExist"), "@", v_strFileName), MsgBoxStyle.Information + MsgBoxStyle.YesNo, gc_ApplicationTitle)
                If v_intAns = vbYes Then
                    v_swStreamWriter = File.CreateText(v_strFileName)
                Else
                    Exit Sub
                End If
            Else
                v_swStreamWriter = File.CreateText(v_strFileName)
            End If
            'sql cu
            v_strSQL = "SELECT r.AUTOID FRGROOM_ID, r.SICODE CODE, " _
                    & "decode(r.sicode, 'MKP',0,(case when (nvl(c.listed_qtty,0) = 0 or  nvl(c.listed_qtty,0) >=nvl(r.balance,0)) then " _
                    & "(case when NVL (r.balance, 0)>=0 then NVL (r.balance, 0) else 0 end) " _
                    & " else c.listed_qtty end)) CURRENT_ROOM_QTTY, " _
                    & "C.TOTAL_QTTY, " _
                    & "(case when (nvl(c.listed_qtty,0) = 0 or  nvl(c.listed_qtty,0) >=nvl(C.CURRENT_QTTY,0)) then " _
                    & "nvl(C.CURRENT_QTTY,0)  else c.listed_qtty end) CURRENT_QTTY, " _
                    & "nvl((C.TOTAL_QTTY - r.BALANCE),0) CURRENT_HOLDING_QTTY, C.STOCK_ID, '" & v_dtIndex & "' FRGROOM_DATE " _
                    & "FROM RAMAST r, (" _
                    & " select b.sicode,b.listed_qtty, b.stock_id, (b.total_qtty+nvl(a.msgamt,0)) total_qtty, (nvl(a.current_qtty,0)+b.current_qtty ) current_qtty from " _
                    & " (SELECT b.sicode, sum(b.msgamt) msgamt, b.brid, ROUND(sum(b.msgamt) * max(c.rarate) * 0.01) current_qtty " _
                    & " FROM saissue a, tllog b, rgsi c WHERE a.autoid = b.autoid " _
                    & "            AND a.dedate = get_trade_date(TO_DATE ('" & v_dCurrentDate & "', 'dd/mm/yyyy') + 1, '" & BranchId & "', '+')" _
                    & " AND b.status = 3  AND b.deleted = 0   and c.deleted =0  AND b.brid = '" & BranchId & "'  and b.brid = c.brid " _
                    & "       AND b.sicode = c.sicode group by b.sicode, b.brid) a " _
                    & "      RIGHT OUTER JOIN " _
                    & "( SELECT B.SICODE, max(b.listed_qtty) listed_qtty,MAX(B.AUTOID) STOCK_ID,SUM(A.BALANCE) TOTAL_QTTY,  round(SUM(A.BALANCE)* MAX(B.RARATE)/100) CURRENT_QTTY FROM MAMAST A, RGSI B " _
                    & "WHERE(A.DELETED = 0 And A.STATUS = 0 And B.SICODE = SUBSTR(A.MAACCTNO, 12, LENGTH(A.MAACCTNO) - 11) And B.DELETED = 0 AND B.STATUS=0 and a.brid = b.brid and  b.brid ='" & BranchId & "') " _
                    & "GROUP BY B.SICODE) b on a.sicode = b.sicode  ) C " _
                    & "WHERE r.DELETED = 0 AND r.STATUS = 0" _
                    & " AND r.BRID = '" & BranchId & "' " _
                    & " and r.sicode = c.sicode" _
                    & " and r.sicode not in (select sicode from rgsi where EFFECTIVE_DATE > get_t_plus(to_date('" & v_dCurrentDate & "','DD/MM/YYYY'),'" & BranchId & "',1) and status = 0 and deleted = 0)"
            'bangpv: sua tam loi ngay 27/01/2014
            'v_strSQL = "SELECT * FROM ROOM27012014"
            'Sql moi
            v_strSQL = "with tmp1 as " _
                   & "(SELECT c.sicode,SUM (b.msgamt) msgamt, c.brid, /*round*/ (SUM (b.msgamt) * MAX (c.rarate) * 0.01) current_qtty" _
                   & "    FROM (SELECT *FROM saissue a WHERE a.dedate = get_trade_date (TO_DATE ('" & v_dCurrentDate & "','DD/MM/YYYY')+ 1,'" & BranchId & "','+' )) a," _
                   & "                         tllog b, rgsi c " _
                   & "       WHERE a.autoid = b.autoid   And b.status = 3 and c.status =0 And b.deleted = 0 And c.deleted = 0 AND c.brid = '" & BranchId & "'" _
                   & "   /*AND b.brid = c.brid*/    " _
                   & "                     and c.sicode =(case when b.tltxcd ='6043' and lpad (b.col_value05,2,'0') in ('08','09')" _
                   & "                     then b.cosicode else b.sicode end)   " _
                   & "GROUP BY c.sicode, c.brid), " _
                   & "   tmp2 as " _
                   & "  (SELECT a.SICODE, SUM (A.BALANCE) BALANCE " _
                   & "      FROM MAMAST A WHERE (    A.DELETED = 0 AND A.STATUS = 0 AND a.brid = '" & BranchId & "') GROUP BY a.SICODE), " _
                   & " tmp3 as (" _
                   & " SELECT B.SICODE, MAX (b.listed_qtty) listed_qtty, MAX (B.AUTOID) STOCK_ID," _
                   & "        SUM (A.BALANCE) TOTAL_QTTY, /*round*/ (SUM (A.BALANCE) * MAX (B.RARATE) / 100) CURRENT_QTTY" _
                   & "     FROM tmp2 A, RGSI B" _
                   & " WHERE B.SICODE = a.sicode AND B.DELETED = 0 AND B.STATUS = 0 AND b.brid = '" & BranchId & "' " _
                   & "      and EFFECTIVE_DATE <=get_t_plus (TO_DATE ('" & v_dCurrentDate & "', 'DD/MM/YYYY'),'" & BranchId & "', 1)  GROUP BY B.SICODE)," _
                   & "tmp4 as (" _
                   & " SELECT b.sicode,b.listed_qtty, b.stock_id,(b.total_qtty + NVL (a.msgamt, 0)) total_qtty," _
                   & "               floor(NVL (a.current_qtty, 0) + b.current_qtty) current_qtty " _
                   & "   FROM    tmp1 a " _
                   & " RIGHT OUTER JOIN tmp3 b ON a.sicode = b.sicode ) " _
                   & " SELECT r.AUTOID FRGROOM_ID,r.SICODE CODE, " _
                   & "      DECODE (r.sicode, 'MKP',0, " _
                   & "         (CASE WHEN (NVL (c.listed_qtty, 0) = 0 OR NVL (c.listed_qtty, 0) >= NVL (r.balance, 0)) " _
                   & "                   THEN " _
                   & "                      (CASE WHEN NVL (r.balance, 0) >= 0 THEN NVL (r.balance, 0)ELSE 0 END) " _
                   & "                   ELSE c.listed_qtty  END)) CURRENT_ROOM_QTTY, C.TOTAL_QTTY, " _
                   & "       (CASE  WHEN (NVL (c.listed_qtty, 0) = 0 ) " _
                   & "           then nvl(C.CURRENT_QTTY, 0) " _
                   & "           when   NVL (c.listed_qtty, 0) >= NVL (C.CURRENT_QTTY, 0)  " _
                   & "           THEN  NVL (C.CURRENT_QTTY, 0)/* c.listed_qtty*/  ELSE c.listed_qtty  END) CURRENT_QTTY, NVL ( (C.TOTAL_QTTY - r.BALANCE), 0) CURRENT_HOLDING_QTTY, " _
                   & " C.STOCK_ID,  '" & v_dtIndex & "' FRGROOM_DATE " _
                   & "  FROM RAMAST r, tmp4 C " _
                   & "WHERE     r.DELETED = 0  AND r.STATUS = 0  AND r.BRID = '" & BranchId & "'  AND r.sicode = c.sicode"
            'end bangpv
            v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLGROUPS, gc_ActionInquiry, v_strSQL)

            Dim v_lngError As Long = Proxy.Message(v_strObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Exit Sub
            Else
                ''start Myvq            
                'v_strObjMsg = ClientBussinessCA.DeCombineData(v_strObjMsg, v_strObjMsg, mv_strRespond)
                ''end Myvq
                v_xmlDocument.LoadXml(v_strObjMsg)
                v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")

                Dim v_strRoomDate As String = ""
                Dim v_strSiCode As String = ""
                Dim v_strTotalQtty As String = ""
                Dim v_strAdjustQtty As String = ""

                For i As Integer = 0 To v_nodeList.Count - 1
                    For j As Integer = 0 To v_nodeList.Item(i).ChildNodes.Count - 1
                        With v_nodeList.Item(i).ChildNodes(j)
                            v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                            v_strValue = .InnerText.ToString
                        End With
                        Select Case Trim(v_strFLDNAME)
                            Case "FRGROOM_DATE"
                                v_strRoomDate = v_strValue
                            Case "CODE"
                                v_strSiCode = v_strValue
                            Case "CURRENT_QTTY"
                                v_strTotalQtty = v_strValue
                            Case "CURRENT_ROOM_QTTY"
                                v_strAdjustQtty = v_strValue
                        End Select
                    Next
                    'Ghi ra file
                    v_strSiCode = Mid(v_strSiCode + FORMAT_STOCK_CODE, 1, Len(FORMAT_STOCK_CODE))
                    v_strTotalQtty = Format(CLng(v_strTotalQtty), "0000000000000#")
                    If CLng(v_strAdjustQtty) >= 0 Then
                        v_strAdjustQtty = Format(CLng(v_strAdjustQtty), "00000000000000")
                    Else
                        v_strAdjustQtty = Format(CLng(v_strAdjustQtty), "0000000000000")
                    End If
                    v_strLine = v_strRoomDate & v_strSiCode & v_strTotalQtty & v_strAdjustQtty
                    v_swStreamWriter.WriteLine(v_strLine)
                Next
                v_swStreamWriter.Close()

                'Start Myvq
                'Dim v_strFileToString As String = ClientBussinessCA.FileToString(v_strFileName)
                'mv_strRespond = mv_oProxy.FileCA(v_strFileToString, mv_strBranchId, mv_strFileName, v_dCurrentDate)
                'mv_strFileName = ""

                'If Not (mv_strRespond Is Nothing) And (mv_strRespond <> "") Then
                '    Dim v_strFileNameEncrypted = pv_strFilePath & HEADER_ENCRYPTED & pv_strFileName & v_dCurrentDate & ".txt" & ".xml"
                '    Dim v_xmlEncryptedXML As Xml.XmlDocument = New Xml.XmlDocument
                '    v_xmlEncryptedXML.LoadXml(mv_strRespond)
                '    If CheckExistFile(pv_strFilePath & "", v_strFileNameEncrypted) Then
                '        v_intAns = MsgBox(Replace(mv_ResourceManager.GetString("FileExist"), "@", v_strFileName), MsgBoxStyle.Information + MsgBoxStyle.YesNo, gc_ApplicationTitle)
                '        If v_intAns = vbYes Then
                '            File.Delete(v_strFileNameEncrypted)
                '            v_xmlEncryptedXML.Save(v_strFileNameEncrypted)
                '        Else
                '            Exit Sub
                '        End If
                '    Else
                '        v_xmlEncryptedXML.Save(v_strFileNameEncrypted)
                '    End If
                '    mv_strRespond = ""
                'End If
                'end Myvq

                v_strObjMsg = BuildXMLObjMsg(, BranchId, , Me.TellerId, gc_IsNotLocalMsg, gc_MsgTypeObj, OBJNAME_SY_AUTHENTICATION, gc_ActionInquiry, , , "UPDATE_ROOM_STATUS", , )
                'Dim v_lngError As String = mv_BDSDelivery.Message(v_strObjMsg)
                v_lngError = Proxy.Message(v_strObjMsg)
                If v_lngError <> ERR_SYSTEM_OK Then
                    MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                    Cursor = Cursors.Default
                    'MsgBox(v_strErrorMessage, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, Me.Text)
                    Exit Sub
                Else
                    'MsgBox(mv_ResourceManager.GetString("AddnewSuccessful"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                    Cursor = Cursors.Default
                End If
                '                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                '               Exit Sub

            End If

           
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub ExportViewNetBank(ByVal pv_blnMultiPartite As Boolean, _
                                  ByVal pv_blnDirect As Boolean, _
                                  ByVal pv_intFrequence As Integer, _
                                  ByVal pv_dTransDate As Date, _
                                  ByVal pv_strFilePath As String, _
                                  ByVal pv_strFileName As String)
        Try
            Dim v_strSQL As String = ""
            Dim v_xmlDocument As New Xml.XmlDocument
            Dim v_nodeList As Xml.XmlNodeList
            Dim v_strObjMsg As String
            'Dim v_fTextFile As System.IO.File
            'Dim v_swStreamWriter As System.IO.StreamWriter

            Dim v_strLine, v_strFLDNAME, v_strValue As String
            Dim v_strFileName As String = ""
            Dim v_intAns As Integer
            Dim v_strTransdate As String
            If pv_blnMultiPartite Then
                v_strFileName = pv_strFilePath & pv_strFileName & "_" & cboBranch2.Text & "_DP" & Replace(Replace(cboFrequence.Text, "+", ""), " ", "") & "_" & Format(pv_dTransDate, "ddMMyyyy") & ".txt"
            Else
                v_strFileName = pv_strFilePath & pv_strFileName & "_" & cboBranch2.Text & "_TT_" & Format(pv_dTransDate, "ddMMyyyy") & "_PT.txt"
            End If
            Dim v_swStreamWriter As New StreamWriter(v_strFileName, False, System.Text.Encoding.Default)
            'If CheckExistFile(pv_strFilePath, v_strFileName) Then
            '    v_intAns = MsgBox(Replace(mv_ResourceManager.GetString("FileExist"), "@", v_strFileName), MsgBoxStyle.Information + MsgBoxStyle.YesNo, gc_ApplicationTitle)
            '    If v_intAns = vbYes Then
            '        Dim v_swStreamWriter As New StreamWriter(v_strFileName, False, System.Text.Encoding.ASCII)
            '        'v_swStreamWriter = File.CreateText(v_strFileName)
            '        'v_swStreamWriter = File.CreateText(v_strFileName)
            '    Else
            '        Exit Sub
            '    End If
            'Else
            '    ' Dim v_swStreamWriter As New StreamWriter(v_strFileName, True, System.Text.Encoding.ASCII)
            '    'v_swStreamWriter = File.CreateText(v_strFileName)
            '    Dim v_swStreamWriter As New StreamWriter(v_strFileName, False, System.Text.Encoding.ASCII)
            'End If
            v_strTransdate = GetPartition(CStr(pv_dTransDate))
            Dim v_lngError As Long
            'bangpv :
            'Kiểm tra xem có đúng ngày chọn là ngày giao dịch cần đẩy thanh toán không
            'v_strObjMsg = BuildXMLObjMsg(pv_dTransDate, BranchId, , TellerId, gc_IsNotLocalMsg, gc_MsgTypeObj, OBJNAME_RP_RPREPORT, _
            '                     gc_ActionAdhoc, , pv_intFrequence, "CheckviewNetBank", , v_strTransdate)

            'Dim v_lngError As Long = mv_BDSDelivery.Message(v_strObjMsg)

            'If v_lngError <> ERR_SYSTEM_OK Then
            '    MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
            '    Exit Sub
            'End If
            'v_xmlDocument.LoadXml(v_strObjMsg)
            'v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")
            'Dim v_strstatus As String
            'For i As Integer = 0 To v_nodeList.Count - 1
            '    For j As Integer = 0 To v_nodeList.Item(i).ChildNodes.Count - 1
            '        With v_nodeList.Item(i).ChildNodes(j)
            '            v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
            '            v_strValue = .InnerText.ToString
            '        End With
            '        Select Case Trim(v_strFLDNAME)
            '            Case "STATUS"
            '                v_strstatus = v_strValue
            '        End Select
            '    Next
            'Next

            'If v_strstatus = "0" Then
            '    MsgBox("Ngày giao dịch không đúng hoặc đã được thanh toán rồi", MsgBoxStyle.Critical, gc_ApplicationTitle)
            '    Exit Sub
            'End If

            'end bangpv
            v_strObjMsg = BuildXMLObjMsg(pv_dTransDate, LSet(cboBranch2.Text, 4), , TellerId, gc_IsNotLocalMsg, gc_MsgTypeObj, OBJNAME_RP_RPREPORT, _
                                 gc_ActionAdhoc, , pv_intFrequence, "ExportViewNetBank", , v_strTransdate)


            'v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLGROUPS, gc_ActionInquiry, v_strSQL)

            v_lngError = Proxy.Message(v_strObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Exit Sub
            Else

               
                v_xmlDocument.LoadXml(v_strObjMsg)
                v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")

                Dim v_strTxDate As String = ""
                Dim v_strBusDate As String = ""
                Dim v_strMiName As String = ""
                Dim v_strMiCode As String = ""
                Dim v_strBidDealing As String = ""
                Dim v_strBidBrockerageDomestic As String = ""
                Dim v_strBidBrockerageForeign As String = ""
                Dim v_strOfferDealing As String = ""
                Dim v_strOfferBrockerageDomestic As String = ""
                Dim v_strOfferBrockerageForeign As String = ""
                Dim v_strBid As String = ""
                Dim v_strOffer As String = ""


                For i As Integer = 0 To v_nodeList.Count - 1
                    For j As Integer = 0 To v_nodeList.Item(i).ChildNodes.Count - 1
                        With v_nodeList.Item(i).ChildNodes(j)
                            v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                            v_strValue = .InnerText.ToString
                        End With
                        Select Case Trim(v_strFLDNAME)
                            Case "TXDATE"
                                v_strTxDate = v_strValue
                            Case "MINAME"
                                v_strMiName = v_strValue
                            Case "CODE_TRADE"
                                v_strMiCode = v_strValue
                            Case "DUOCNHANTD"
                                v_strBidDealing = v_strValue
                            Case "DUOCNHANMGTN"
                                v_strBidBrockerageDomestic = v_strValue
                            Case "DUOCNHANMGNN"
                                v_strBidBrockerageForeign = v_strValue
                            Case "PHAIGIAOTD"
                                v_strOfferDealing = v_strValue
                            Case "PHAIGIAOMGTN"
                                v_strOfferBrockerageDomestic = v_strValue
                            Case "PHAIGIAOMGNN"
                                v_strOfferBrockerageForeign = v_strValue
                            Case "DUOCNHAN"
                                v_strBid = v_strValue
                            Case "PHAIGIAO"
                                v_strOffer = v_strValue
                            Case "BUSDATE"
                                If pv_blnMultiPartite Then
                                    v_strBusDate = ""
                                Else
                                    'v_strBusDate = v_strValue
                                    'v_strBusDate = CStr(Format(CDate(v_dCurrentDate), "DD/MM/YYYY"))
                                    v_strBusDate = Mid(v_dCurrentDate, 1, 2) & "/" & Mid(v_dCurrentDate, 3, 2) & "/" & Mid(v_dCurrentDate, 5)
                                End If
                        End Select
                    Next
                    'Ghi ra file
                    v_strLine = v_strTxDate & "|" _
                                & Uni2TCVN(v_strMiName) & "|" _
                                & v_strMiCode & "|" _
                                & v_strBidDealing & "|" _
                                & v_strBidBrockerageDomestic & "|" _
                                & v_strBidBrockerageForeign & "|" _
                                & v_strOfferDealing & "|" _
                                & v_strOfferBrockerageDomestic & "|" _
                                & v_strOfferBrockerageForeign & "|" _
                                & v_strOffer & "|" _
                                & v_strBid & "|" _
                                & v_strBusDate & "|"

                    v_swStreamWriter.WriteLine(v_strLine)
                Next
                v_swStreamWriter.Close()

                v_strObjMsg = BuildXMLObjMsg(, LSet(cboBranch2.Text, 4), , Me.TellerId, gc_IsNotLocalMsg, gc_MsgTypeObj, OBJNAME_SY_AUTHENTICATION, gc_ActionInquiry, , pv_intFrequence, "UPDATE_BANK_STATUS", , )
                'Dim v_lngError As String = mv_BDSDelivery.Message(v_strObjMsg)
                v_lngError = Proxy.Message(v_strObjMsg)
                If v_lngError <> ERR_SYSTEM_OK Then
                    MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)

                    Cursor = Cursors.Default
                    'MsgBox(v_strErrorMessage, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, Me.Text)
                    Exit Sub
                End If
                '                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                '               Exit Sub
            End If
            Cursor = Cursors.Default
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    'bangpv 20150211
    Private Sub ExportViewNetBank2(ByVal pv_intFrequence As Integer, _
                                  ByVal pv_dTransDate As Date, _
                                  ByVal pv_strFilePath As String, _
                                  ByVal pv_strFileName As String, _
                                  ByVal pv_intCsErrType As Integer)
        Try
            Dim v_strSQL As String = ""
            Dim v_xmlDocument As New Xml.XmlDocument
            Dim v_nodeList As Xml.XmlNodeList
            Dim v_strObjMsg As String
            'Dim v_fTextFile As System.IO.File
            'Dim v_swStreamWriter As System.IO.StreamWriter

            Dim v_strLine, v_strFLDNAME, v_strValue As String
            Dim v_strFileName As String = ""
            Dim v_intAns As Integer
            Dim v_strTransdate As String

            v_strFileName = pv_strFilePath & pv_strFileName & "_" & cboBranch4.Text & "_TT_" & Format(pv_dTransDate, "ddMMyyyy") & "_" _
                            & cboCsErrType.SelectedValue & "_" & IIf(cboFrequence2.SelectedValue = 1, "S", "C") & "_" & v_dCurrentDate & "_TT.txt"

            Dim v_swStreamWriter As New StreamWriter(v_strFileName, False, System.Text.Encoding.Default)

            v_strTransdate = GetPartition(CStr(pv_dTransDate))
            Dim v_lngError As Long

            v_strObjMsg = BuildXMLObjMsg(pv_dTransDate, LSet(cboBranch4.Text, 4), , TellerId, gc_IsNotLocalMsg, gc_MsgTypeObj, OBJNAME_RP_RPREPORT, _
                                 gc_ActionAdhoc, pv_intCsErrType, pv_intFrequence, "ExportViewNetBank2", , v_strTransdate)


            v_lngError = Proxy.Message(v_strObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Exit Sub
            Else


                v_xmlDocument.LoadXml(v_strObjMsg)
                v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")

                Dim v_strTxDate As String = ""
                Dim v_strBusDate As String = ""
                Dim v_strMiName As String = ""
                Dim v_strMiCode As String = ""
                Dim v_strBidDealing As String = ""
                Dim v_strBidBrockerageDomestic As String = ""
                Dim v_strBidBrockerageForeign As String = ""
                Dim v_strOfferDealing As String = ""
                Dim v_strOfferBrockerageDomestic As String = ""
                Dim v_strOfferBrockerageForeign As String = ""
                Dim v_strBid As String = ""
                Dim v_strOffer As String = ""


                For i As Integer = 0 To v_nodeList.Count - 1
                    For j As Integer = 0 To v_nodeList.Item(i).ChildNodes.Count - 1
                        With v_nodeList.Item(i).ChildNodes(j)
                            v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                            v_strValue = .InnerText.ToString
                        End With
                        Select Case Trim(v_strFLDNAME)
                            Case "TXDATE"
                                v_strTxDate = v_strValue
                            Case "MINAME"
                                v_strMiName = v_strValue
                            Case "CODE_TRADE"
                                v_strMiCode = v_strValue
                            Case "DUOCNHANTD"
                                v_strBidDealing = v_strValue
                            Case "DUOCNHANMGTN"
                                v_strBidBrockerageDomestic = v_strValue
                            Case "DUOCNHANMGNN"
                                v_strBidBrockerageForeign = v_strValue
                            Case "PHAIGIAOTD"
                                v_strOfferDealing = v_strValue
                            Case "PHAIGIAOMGTN"
                                v_strOfferBrockerageDomestic = v_strValue
                            Case "PHAIGIAOMGNN"
                                v_strOfferBrockerageForeign = v_strValue
                            Case "DUOCNHAN"
                                v_strBid = v_strValue
                            Case "PHAIGIAO"
                                v_strOffer = v_strValue
                            Case "BUSDATE"
                                'If pv_blnMultiPartite Then
                                'v_strBusDate = ""
                                'Else
                                v_strBusDate = v_strValue
                                'End If
                        End Select
                    Next
                    'Ghi ra file
                    v_strLine = v_strTxDate & "|" _
                                & Uni2TCVN(v_strMiName) & "|" _
                                & v_strMiCode & "|" _
                                & v_strBidDealing & "|" _
                                & v_strBidBrockerageDomestic & "|" _
                                & v_strBidBrockerageForeign & "|" _
                                & v_strOfferDealing & "|" _
                                & v_strOfferBrockerageDomestic & "|" _
                                & v_strOfferBrockerageForeign & "|" _
                                & v_strOffer & "|" _
                                & v_strBid & "|" _
                                & v_strBusDate & "|"

                    v_swStreamWriter.WriteLine(v_strLine)
                Next
                v_swStreamWriter.Close()

                v_strObjMsg = BuildXMLObjMsg(, LSet(cboBranch2.Text, 4), , Me.TellerId, gc_IsNotLocalMsg, gc_MsgTypeObj, OBJNAME_SY_AUTHENTICATION, gc_ActionInquiry, , pv_intFrequence, "UPDATE_BANK_STATUS", , )
                'Dim v_lngError As String = mv_BDSDelivery.Message(v_strObjMsg)
                v_lngError = Proxy.Message(v_strObjMsg)
                If v_lngError <> ERR_SYSTEM_OK Then
                    MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)

                    Cursor = Cursors.Default
                    'MsgBox(v_strErrorMessage, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, Me.Text)
                    Exit Sub
                End If
                '                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                '               Exit Sub
            End If
            Cursor = Cursors.Default
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub ExportViewNetBank3(ByVal pv_dTransDate As Date, _
                                ByVal pv_strFilePath As String, _
                                ByVal pv_strFileName As String)
        Try
            Dim v_strSQL As String = ""
            Dim v_xmlDocument As New Xml.XmlDocument
            Dim v_nodeList As Xml.XmlNodeList
            Dim v_strObjMsg As String
            'Dim v_fTextFile As System.IO.File
            'Dim v_swStreamWriter As System.IO.StreamWriter

            Dim v_strLine, v_strFLDNAME, v_strValue As String
            Dim v_strFileName As String = ""
            Dim v_intAns As Integer
            Dim v_strTransdate As String

            v_strFileName = pv_strFilePath & pv_strFileName & "_" & cboBranch5.Text & "_TT_" & Format(pv_dTransDate, "ddMMyyyy") & "_GT.txt"

            Dim v_swStreamWriter As New StreamWriter(v_strFileName, False, System.Text.Encoding.Default)

            v_strTransdate = GetPartition(CStr(pv_dTransDate))
            Dim v_lngError As Long

            v_strObjMsg = BuildXMLObjMsg(pv_dTransDate, LSet(cboBranch5.Text, 4), , TellerId, gc_IsNotLocalMsg, gc_MsgTypeObj, OBJNAME_RP_RPREPORT, _
                                 gc_ActionAdhoc, , , "ExportViewNetBank3", , v_strTransdate)


            v_lngError = Proxy.Message(v_strObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Exit Sub
            Else


                v_xmlDocument.LoadXml(v_strObjMsg)
                v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")

                Dim v_strTxDate As String = ""
                Dim v_strBusDate As String = ""
                Dim v_strMiName As String = ""
                Dim v_strMiCode As String = ""
                Dim v_strBidDealing As String = ""
                Dim v_strBidBrockerageDomestic As String = ""
                Dim v_strBidBrockerageForeign As String = ""
                Dim v_strOfferDealing As String = ""
                Dim v_strOfferBrockerageDomestic As String = ""
                Dim v_strOfferBrockerageForeign As String = ""
                Dim v_strBid As String = ""
                Dim v_strOffer As String = ""


                For i As Integer = 0 To v_nodeList.Count - 1
                    For j As Integer = 0 To v_nodeList.Item(i).ChildNodes.Count - 1
                        With v_nodeList.Item(i).ChildNodes(j)
                            v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                            v_strValue = .InnerText.ToString
                        End With
                        Select Case Trim(v_strFLDNAME)
                            Case "TXDATE"
                                v_strTxDate = v_strValue
                            Case "MINAME"
                                v_strMiName = v_strValue
                            Case "CODE_TRADE"
                                v_strMiCode = v_strValue
                            Case "DUOCNHANTD"
                                v_strBidDealing = v_strValue
                            Case "DUOCNHANMGTN"
                                v_strBidBrockerageDomestic = v_strValue
                            Case "DUOCNHANMGNN"
                                v_strBidBrockerageForeign = v_strValue
                            Case "PHAIGIAOTD"
                                v_strOfferDealing = v_strValue
                            Case "PHAIGIAOMGTN"
                                v_strOfferBrockerageDomestic = v_strValue
                            Case "PHAIGIAOMGNN"
                                v_strOfferBrockerageForeign = v_strValue
                            Case "DUOCNHAN"
                                v_strBid = v_strValue
                            Case "PHAIGIAO"
                                v_strOffer = v_strValue
                            Case "BUSDATE"
                                'If pv_blnMultiPartite Then
                                'v_strBusDate = ""
                                'Else
                                v_strBusDate = v_strValue
                                'End If
                        End Select
                    Next
                    'Ghi ra file
                    v_strLine = v_strTxDate & "|" _
                                & Uni2TCVN(v_strMiName) & "|" _
                                & v_strMiCode & "|" _
                                & v_strBidDealing & "|" _
                                & v_strBidBrockerageDomestic & "|" _
                                & v_strBidBrockerageForeign & "|" _
                                & v_strOfferDealing & "|" _
                                & v_strOfferBrockerageDomestic & "|" _
                                & v_strOfferBrockerageForeign & "|" _
                                & v_strOffer & "|" _
                                & v_strBid & "|" _
                                & v_strBusDate & "|"

                    v_swStreamWriter.WriteLine(v_strLine)
                Next
                v_swStreamWriter.Close()

                'v_strObjMsg = BuildXMLObjMsg(, LSet(cboBranch2.Text, 4), , Me.TellerId, gc_IsNotLocalMsg, gc_MsgTypeObj, OBJNAME_SY_AUTHENTICATION, gc_ActionInquiry, , pv_intFrequence, "UPDATE_BANK_STATUS", , )
                'Dim v_lngError As String = mv_BDSDelivery.Message(v_strObjMsg)
                'v_lngError = Proxy.Message(v_strObjMsg)
                'If v_lngError <> ERR_SYSTEM_OK Then
                '    MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)

                '    Cursor = Cursors.Default
                '    'MsgBox(v_strErrorMessage, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, Me.Text)
                '    Exit Sub
                'End If
                '                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                '               Exit Sub
            End If
            Cursor = Cursors.Default
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    'end bangpv 20150211
    Private Sub ExportToXMLFile(ByRef pv_blnChecked As Boolean, ByRef pv_strFilePath As String, ByRef pv_strFileName As String, ByVal v_Prefix As String, Optional ByRef v_blnTRANS As Integer = 0, Optional ByRef v_blnIS As Integer = 0)
        Dim v_xmlDocument As New Xml.XmlDocument
        Dim v_intAns As Integer
        Dim v_StrSQL As String
        Dim v_strObjMSG As String
        Dim v_lngError As Long
        mv_strFileName = v_Prefix & "_" & pv_strFileName & v_dCurrentDate.ToString & ".xml"
        Try
            If pv_blnChecked Then
                Select Case Trim(pv_strFileName)
                    Case "Broker"
                        v_xmlDocument = GetXMLCWData()
                    Case "ISSUERS"
                        v_xmlDocument = GetXMLISData()
                        'Dim v_strFileName As String = pv_strFilePath & pv_strFileName & v_dCurrentDate.ToString & ".xml"
                        'If CheckExistFile(pv_strFilePath, v_strFileName) Then
                        '    v_intAns = MsgBox(Replace(mv_ResourceManager.GetString("FileExist"), "@", v_strFileName), MsgBoxStyle.Information + MsgBoxStyle.YesNo, gc_ApplicationTitle)
                        '    If v_intAns = vbYes Then
                        '        v_xmlDocument.Save(v_strFileName)
                        '    Else
                        '        Exit Sub
                        '    End If
                        'Else
                        '    v_xmlDocument.Save(v_strFileName)
                        'End If
                    Case "STOCKS"
                        v_xmlDocument = GetXMLSIData()
                        'Dim v_strFileName As String = pv_strFilePath & pv_strFileName & v_dCurrentDate.ToString & ".xml"
                        'If CheckExistFile(pv_strFilePath, v_strFileName) Then
                        '    v_intAns = MsgBox(Replace(mv_ResourceManager.GetString("FileExist"), "@", v_strFileName), MsgBoxStyle.Information + MsgBoxStyle.YesNo, gc_ApplicationTitle)
                        '    If v_intAns = vbYes Then
                        '        v_xmlDocument.Save(v_strFileName)
                        '    Else
                        '        Exit Sub
                        '    End If
                        'Else
                        '    v_xmlDocument.Save(v_strFileName)
                        'End If
                    Case "MEMBERS"
                        v_xmlDocument = GetXMLMIData()
                        'Dim v_strFileName As String = pv_strFilePath & pv_strFileName & v_dCurrentDate.ToString & ".xml"
                        'If CheckExistFile(pv_strFilePath, v_strFileName) Then
                        '    v_intAns = MsgBox(Replace(mv_ResourceManager.GetString("FileExist"), "@", v_strFileName), MsgBoxStyle.Information + MsgBoxStyle.YesNo, gc_ApplicationTitle)
                        '    If v_intAns = vbYes Then
                        '        v_xmlDocument.Save(v_strFileName)
                        '    Else
                        '        Exit Sub
                        '    End If
                        'Else
                        '    v_xmlDocument.Save(v_strFileName)
                        'End If
                    Case "FOREIGN_ROOM"
                        v_xmlDocument = GetXMLRoomData()
                        'Dim v_strFileName As String = pv_strFilePath & pv_strFileName & v_dCurrentDate.ToString & ".xml"
                        'If CheckExistFile(pv_strFilePath, v_strFileName) Then
                        '    v_intAns = MsgBox(Replace(mv_ResourceManager.GetString("FileExist"), "@", v_strFileName), MsgBoxStyle.Information + MsgBoxStyle.YesNo, gc_ApplicationTitle)
                        '    If v_intAns = vbYes Then
                        '        v_xmlDocument.Save(v_strFileName)
                        '    Else
                        '        Exit Sub
                        '    End If
                        'Else
                        '    v_xmlDocument.Save(v_strFileName)
                        'End If
                    Case "ISSUERS_NHNN"
                        v_StrSQL = "SELECT a.VARVALUE FROM sysvar a where a.GRNAME='SYSTEM' and a.VARNAME= 'CURRDATE' and BRID='" & BranchId3 & "'"
                        v_strObjMSG = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLTX, gc_ActionInquiry, v_StrSQL)

                        v_lngError = Proxy.Message(v_strObjMSG)
                        If v_lngError <> ERR_SYSTEM_OK Then
                            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                            Exit Sub
                        End If
                        v_xmlDocument.LoadXml(v_strObjMSG)
                        v_dtIndex_NHNN = CDate(v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='VARVALUE']").InnerText)
                        v_dCurrentDate_NHNN = CStr(Format(v_dtIndex, "ddMMyyyy"))
                        v_xmlDocument = GetXMLISData_NHNN()

                    Case "STOCKS_NHNN"
                        v_StrSQL = "SELECT a.VARVALUE FROM sysvar a where a.GRNAME='SYSTEM' and a.VARNAME= 'CURRDATE' and BRID='" & BranchId3 & "'"
                        v_strObjMSG = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLTX, gc_ActionInquiry, v_StrSQL)

                        v_lngError = Proxy.Message(v_strObjMSG)
                        If v_lngError <> ERR_SYSTEM_OK Then
                            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                            Exit Sub
                        End If
                        v_xmlDocument.LoadXml(v_strObjMSG)
                        v_dtIndex_NHNN = CDate(v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='VARVALUE']").InnerText)
                        v_dCurrentDate_NHNN = CStr(Format(v_dtIndex, "ddMMyyyy"))
                        v_xmlDocument = GetXMLSIData_NHNN()
                    Case "MEMBERS_NHNN"
                        v_StrSQL = "SELECT a.VARVALUE FROM sysvar a where a.GRNAME='SYSTEM' and a.VARNAME= 'CURRDATE' and BRID='" & BranchId3 & "'"
                        v_strObjMSG = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLTX, gc_ActionInquiry, v_StrSQL)

                        v_lngError = Proxy.Message(v_strObjMSG)
                        If v_lngError <> ERR_SYSTEM_OK Then
                            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                            Exit Sub
                        End If
                        v_xmlDocument.LoadXml(v_strObjMSG)
                        v_dtIndex_NHNN = CDate(v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='VARVALUE']").InnerText)
                        v_dCurrentDate_NHNN = CStr(Format(v_dtIndex, "ddMMyyyy"))
                        v_xmlDocument = GetXMLMIData_NHNN()
                    Case "CDS_TRANS_NHNN"
                        v_StrSQL = "SELECT a.VARVALUE FROM sysvar a where a.GRNAME='SYSTEM' and a.VARNAME= 'CURRDATE' and BRID='" & BranchId3 & "'"
                        v_strObjMSG = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLTX, gc_ActionInquiry, v_StrSQL)

                        v_lngError = Proxy.Message(v_strObjMSG)
                        If v_lngError <> ERR_SYSTEM_OK Then
                            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                            Exit Sub
                        End If
                        v_xmlDocument.LoadXml(v_strObjMSG)
                        v_dtIndex_NHNN = CDate(v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='VARVALUE']").InnerText)
                        v_dCurrentDate_NHNN = CStr(Format(v_dtIndex, "ddMMyyyy"))
                        v_xmlDocument = GetXMLCDS_TRANS_NHNN()

                End Select
            Else
                Exit Sub
            End If
            'If pv_strFileName = "ISSUERS_NHNN" Or pv_strFileName = "STOCKS_NHNN" Or pv_strFileName = "MEMBERS_NHNN" Or pv_strFileName = "CDS_TRANS_NHNN" Then
            '    pv_strFileName = BranchId3 & "_" & pv_strFileName & "_" & v_dCurrentDate_NHNN
            '    pv_strFileName = pv_strFileName & "_" & v_dCurrentDate_NHNN
            'End If
            If Not v_xmlDocument Is Nothing Then
                v_blnTRANS = 0
                v_blnIS = 0
                Dim v_strFileName As String
                If pv_strFileName = "ISSUERS_NHNN" Or pv_strFileName = "STOCKS_NHNN" Or pv_strFileName = "MEMBERS_NHNN" Or pv_strFileName = "CDS_TRANS_NHNN" Then
                    'bangpv
                    'v_strFileName = pv_strFilePath & pv_strFileName & v_dCurrentDate_NHNN & "_" & DateTime.Now.ToString("HH:mm:ss").Replace(":", "") & ".xml"
                    'end bangpv
                    If pv_strFileName = "CDS_TRANS_NHNN" Then
                        v_strFileName = pv_strFilePath & v_strFileNHNN
                    Else
                        v_strFileName = pv_strFilePath & pv_strFileName & v_dCurrentDate_NHNN & "_" & DateTime.Now.ToString("HH:mm:ss").Replace(":", "") & ".xml"
                    End If
                ElseIf pv_strFileName = "Broker" Then
                    v_strFileName = pv_strFilePath & v_Prefix & v_dCurrentDate.ToString & ".xml"
                Else
                    v_strFileName = pv_strFilePath & v_Prefix & "_" & pv_strFileName & v_dCurrentDate.ToString & ".xml"
                End If
                'v_strFileName = pv_strFilePath & v_Prefix & "_" & pv_strFileName & v_dCurrentDate.ToString & ".xml"
                If CheckExistFile(pv_strFilePath, v_strFileName) Then
                    v_intAns = MsgBox(Replace(mv_ResourceManager.GetString("FileExist"), "@", v_strFileName), MsgBoxStyle.Information + MsgBoxStyle.YesNo, gc_ApplicationTitle)
                    If v_intAns = vbYes Then
                        File.Delete(v_strFileName)
                        v_xmlDocument.Save(v_strFileName)
                    Else
                        Exit Sub
                    End If
                Else
                    v_xmlDocument.Save(v_strFileName)
                End If

                'start Myvq
                'Dim v_strFileToString As String = ClientBussinessCA.FileToString(v_strFileName)
                'mv_strRespond = Proxy.FileCA(v_strFileToString, mv_strBranchId, mv_strFileName, v_dCurrentDate)
                'mv_strFileName = ""
                'If Not (mv_strRespond Is Nothing) And (mv_strRespond <> "") Then
                '    Dim v_strFileNameEncrypted = pv_strFilePath & HEADER_ENCRYPTED & v_Prefix & "_" & pv_strFileName & v_dCurrentDate.ToString & ".xml" & ".xml"
                '    Dim v_xmlEncryptedXML As Xml.XmlDocument = New Xml.XmlDocument
                '    v_xmlEncryptedXML.LoadXml(mv_strRespond)
                '    If CheckExistFile(pv_strFilePath & "", v_strFileNameEncrypted) Then
                '        v_intAns = MsgBox(Replace(mv_ResourceManager.GetString("FileExist"), "@", v_strFileName), MsgBoxStyle.Information + MsgBoxStyle.YesNo, gc_ApplicationTitle)
                '        If v_intAns = vbYes Then
                '            File.Delete(v_strFileNameEncrypted)
                '            v_xmlEncryptedXML.Save(v_strFileNameEncrypted)
                '        Else
                '            Exit Sub
                '        End If
                '    Else
                '        v_xmlEncryptedXML.Save(v_strFileNameEncrypted)
                '    End If
                '    mv_strRespond = ""
                'End If
                'end Myvq
            Else
                v_blnTRANS = 1
                v_blnIS = 1
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Function GetFilePath() As String
        Try
            Dim v_strSQL As String = ""
            Dim v_xmlDocument As New Xml.XmlDocument
            Dim v_nodeList As Xml.XmlNodeList
            Dim v_strObjMsg As String
            Dim v_strFLDNAME, v_strValue, v_strFilePath As String

            v_strSQL = "SELECT VARVALUE, VARDESC, EN_VARDESC FROM SYSVAR WHERE TRIM(GRNAME) = 'SYSTEM' AND TRIM(VARNAME)='FILE_EXPORT_PATH' AND DELETED = 0 AND STATUS = 0"

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

    Private Function CheckExistFile(ByVal pv_strDirPath As String, ByVal pv_strFileName As String)
        Dim v_blnCheck As Boolean = False
        Try
            For Each item As String In Directory.GetFiles(pv_strDirPath)
                If Trim(item) = Trim(pv_strFileName) Then
                    v_blnCheck = True
                End If
            Next
            Return v_blnCheck
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Private Sub OnSubmit()
        Cursor = Cursors.WaitCursor
        Dim v_strObjMsg As String
        Dim v_strTxDate As Date
        Dim v_lngError As Long
        Dim v_prefix As String
        Dim v_xmlDocument As New XmlDocumentEx
        Dim v_strSQL As String
        Dim v_intStatus As Integer
        Dim v_intBrStatus As Integer

        Try
            v_strFilePath = GetFilePath()
            'bangpv
            'Lấy ngày hệ thống
            v_strSQL = "SELECT a.VARVALUE FROM sysvar a where a.GRNAME='SYSTEM' and a.VARNAME= 'CURRDATE' and BRID='" & BranchId & "'"
            v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLTX, gc_ActionInquiry, v_strSQL)
            v_lngError = Proxy.Message(v_strObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Exit Sub
            End If
            v_xmlDocument.LoadXml(v_strObjMsg)
            v_dtIndex = CDate(v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='VARVALUE']").InnerText)
            v_dCurrentDate = Replace(v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='VARVALUE']").InnerText, "/", "") 'CStr(Format(v_dtIndex, "ddMMyyyy"))
            'end bangpv 
            Select Case BranchId
                Case "0001"
                    v_prefix = "LISTED"
                Case "0003"
                    v_prefix = "UPCOM"
                Case "0004"
                    v_prefix = "BOND"
                Case "0005"
                    v_prefix = "USDBOND"
                Case "0006"
                    v_prefix = "BILL_VND"
            End Select
            If tpStocks.Focus Then
                v_blnExpIs = cbIs.Checked
                v_blnExpSi = cbSi.Checked
                v_blnExpMi = cbMi.Checked
                v_blnExpRoom = cbRoom.Checked
                'BangPV: Kiem tra xem co chung khoan nao sap toi ngay giao dich khong
                'v_strSQL = "select count(*) Status from (SELECT a.autoid, a.txnum, to_char(a.txdate,'dd/mm/yyyy') txdate, to_char(a.busdate,'dd/mm/yyyy') busdate,  a.tltxcd ,a.txname tran_name ," _
                '            & "a.BRCODE branch_name, a.micode, a.isparent , a.parent_text isparent_text, a.parentid , " _
                '            & "a.tlname member_staff, a.offname vsd_staff,  a.chkname member_leader, a.cfrname vsd_leader, " _
                '            & "a.status, a.status_text, a.ipaddress, a.wsname, a.sicode, a.msgamt, a.tlid, a.chkid, a.offid, a.cfrid, a.childtltxcd, a.isbrid, a.comicode " _
                '            & "FROM tllog a, saissue  b  " _
                '            & "WHERE b.deleted = 0 AND a.autoid = b.autoid and a.brid ='" & BranchId & "' " _
                '            & " and b.dedate=get_t_plus(to_date('" & v_dCurrentDate & "','DD/MM/YYYY'),'" & BranchId & "',1))"
                'v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLTX, gc_ActionInquiry, v_strSQL)
                'v_lngError = Proxy.Message(v_strObjMsg)
                'If v_lngError <> ERR_SYSTEM_OK Then
                '    MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                '    Exit Sub
                'End If
                'v_xmlDocument.LoadXml(v_strObjMsg)
                'v_intStatus = CInt(v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='STATUS']").InnerText)

                'v_strSQL = "SELECT a.VARVALUE FROM sysvar a where a.GRNAME='SYSTEM' and a.VARNAME= 'BRSTATUS' and BRID='" & BranchId & "'"
                'v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLTX, gc_ActionInquiry, v_strSQL)
                'v_lngError = Proxy.Message(v_strObjMsg)
                'If v_lngError <> ERR_SYSTEM_OK Then
                '    MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                '    Exit Sub
                'End If
                'v_xmlDocument.LoadXml(v_strObjMsg)
                'v_intBrStatus = CInt(v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='VARVALUE']").InnerText)
                'If v_intBrStatus = CInt(OPERATION_ACTIVE) And v_intStatus > 0 Then
                '    v_strObjMsg = BuildXMLObjMsg(, BranchId, , Me.TellerId, gc_IsNotLocalMsg, gc_MsgTypeObj, OBJNAME_SY_AUTHENTICATION, gc_ActionInquiry, , , "UPDATE_ROOM_STATUS1", , )
                '    v_lngError = Proxy.Message(v_strObjMsg)
                '    If v_lngError <> ERR_SYSTEM_OK Then
                '        MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                '        Cursor = Cursors.Default
                '        'MsgBox(v_strErrorMessage, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, Me.Text)
                '        Exit Sub
                '    Else
                '        MsgBox("Không thể đẩy Room khi chưa kết thúc ngày giao dịch", MsgBoxStyle.Critical)
                '        Cursor = Cursors.Default
                '        Exit Sub
                '    End If
                'End If
                'BangPV bỏ ngày 02/06/2010: Chuyển ngày hiệu lực room lên 
                'End BangPV
                If v_blnExpIs = False And v_blnExpMi = False And v_blnExpSi = False And v_blnExpRoom = False Then
                    MsgBox(mv_ResourceManager.GetString("NotSelectValue"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                    Cursor = Cursors.Default
                    Exit Sub

                Else
                    v_strFilePath = v_strFilePath & v_dCurrentDate & "\"
                    If Not Directory.Exists(v_strFilePath) Then
                        Directory.CreateDirectory(v_strFilePath)
                    End If
                    ExportToXMLFile(v_blnExpIs, v_strFilePath, "ISSUERS", v_prefix)
                    ExportToXMLFile(v_blnExpSi, v_strFilePath, "STOCKS", v_prefix)
                    ExportToXMLFile(v_blnExpMi, v_strFilePath, "MEMBERS", v_prefix)

                    If BranchId = "0002" Then
                        ExportRoomToTextFile(v_blnExpRoom, v_strFilePath, "FOREIGN_ROOM")
                    Else
                        ExportToXMLFile(v_blnExpRoom, v_strFilePath, "FOREIGN_ROOM", v_prefix)
                    End If
                    If BranchId = "0002" Then
                        'If SendFileFTP(v_strFilePath, "FOREIGN_ROOM" & v_dCurrentDate & ".txt", BranchId) Then
                        '    If v_blnExpRoom Then
                        '        '        'File.Delete(v_strFilePath & "FOREIGN_ROOM" & v_dCurrentDate & ".txt")
                        '    End If
                        'End If
                    Else
                        mv_xZipEngine.ZipFileNotDel(v_strFilePath, v_prefix & v_dCurrentDate & ".zip")
                        If v_blnExpSi Then
                            File.Delete(v_strFilePath & v_prefix & "_STOCKS" & v_dCurrentDate & ".xml")
                        End If
                        If v_blnExpIs Then
                            File.Delete(v_strFilePath & v_prefix & "_ISSUERS" & v_dCurrentDate & ".xml")
                        End If
                        If v_blnExpMi Then
                            File.Delete(v_strFilePath & v_prefix & "_MEMBERS" & v_dCurrentDate & ".xml")
                        End If
                        If v_blnExpRoom Then
                            File.Delete(v_strFilePath & v_prefix & "_FOREIGN_ROOM" & v_dCurrentDate & ".xml")
                        End If
                        'If SendFileFTP(v_strFilePath, v_prefix & v_dCurrentDate & ".zip", BranchId) Then
                        '    'File.Delete(v_strFilePath & v_prefix & v_dCurrentDate & ".zip")
                        'End If
                    End If

                    v_strObjMsg = BuildXMLObjMsg(, BranchId, , Me.TellerId, gc_IsNotLocalMsg, gc_MsgTypeObj, OBJNAME_SY_AUTHENTICATION, gc_ActionInquiry, , , "UPDATE_ROOM_STATUS2", , )
                    v_lngError = Proxy.Message(v_strObjMsg)
                    If v_lngError <> ERR_SYSTEM_OK Then
                        MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                        Cursor = Cursors.Default
                        'MsgBox(v_strErrorMessage, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, Me.Text)
                        Exit Sub
                    Else
                        MsgBox(mv_ResourceManager.GetString("AddnewSuccessful"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                    End If
                End If
                Cursor = Cursors.Default
            ElseIf tpNHNN.Focus Then 'Ket xuat ngan hang nha nuoc
                v_blnExpIs = cbIs1.Checked
                v_blnExpSi = cbSi1.Checked
                v_blnExpMi = cbMi1.Checked
                v_blnExpRoom = cbTrans.Checked
                If v_blnExpIs = False And v_blnExpMi = False And v_blnExpSi = False And v_blnExpRoom = False Then
                    MsgBox(mv_ResourceManager.GetString("NotSelectValue"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                    Cursor = Cursors.Default
                    Exit Sub

                Else
                    Dim v_dDate_NHNN As String
                    Dim v_blnTRANS As Integer = 0
                    Dim v_blnSI As Integer = 0
                    v_dDate_NHNN = CStr(Format(dtpFromDate.Value, "ddMMyyyy")) & "_" & CStr(Format(dtpToDate.Value, "ddMMyyyy"))
                    v_strFilePath = v_strFilePath & v_dDate_NHNN & "\"
                    If Not Directory.Exists(v_strFilePath) Then
                        Directory.CreateDirectory(v_strFilePath)
                    End If
                    If v_blnExpIs Then
                        ExportToXMLFile(v_blnExpIs, v_strFilePath, "ISSUERS_NHNN", v_prefix)
                    End If
                    If v_blnExpSi Then
                        ExportToXMLFile(v_blnExpSi, v_strFilePath, "STOCKS_NHNN", v_prefix, v_blnSI)
                    End If
                    If v_blnExpMi Then
                        ExportToXMLFile(v_blnExpMi, v_strFilePath, "MEMBERS_NHNN", v_prefix)
                    End If
                    If v_blnExpRoom Then
                        ExportToXMLFile(v_blnExpRoom, v_strFilePath, "CDS_TRANS_NHNN", v_prefix, v_blnTRANS)
                    End If

                    v_strObjMsg = BuildXMLObjMsg(, BranchId, , Me.TellerId, gc_IsNotLocalMsg, gc_MsgTypeObj, OBJNAME_SY_AUTHENTICATION, gc_ActionInquiry, , , "UPDATE_ROOM_STATUS", , )
                    v_lngError = Proxy.Message(v_strObjMsg)
                    If v_lngError <> ERR_SYSTEM_OK Then
                        MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                        Cursor = Cursors.Default
                        'MsgBox(v_strErrorMessage, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, Me.Text)
                        Exit Sub
                    Else
                        If v_blnTRANS = 0 And v_blnSI = 0 Then
                            MsgBox(mv_ResourceManager.GetString("AddnewSuccessful"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                        Else
                            If v_blnTRANS = 1 And v_blnSI = 0 Then
                                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, "Kết xuất dữ liệu Giao dịch NHNN không thành công : không tồn tại dữ liệu hoặc dữ liệu đã được kết xuất trong khoảng thời gian này!", "Kết xuất dữ liệu Giao dịch NHNN không thành công : không tồn tại dữ liệu hoặc dữ liệu đã được kết xuất trong khoảng thời gian này!"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                            ElseIf v_blnTRANS = 0 And v_blnSI = 1 Then
                                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, "Kết xuất dữ liệu Thông tin TP không thành công : không tồn tại dữ liệu!", "Kết xuất dữ liệu Thông tin TP không thành công : không tồn tại dữ liệu!"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                            Else
                                Dim v_strError As String = "-Kết xuất dữ liệu Giao dịch NHNN không thành công : không tồn tại dữ liệu hoặc dữ liệu đã được kết xuất trong khoảng thời gian này!" & vbCrLf _
                                                            & "-Kết xuất dữ liệu Thông tin TP không thành công : không tồn tại dữ liệu!"
                                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, v_strError, v_strError), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                            End If
                        End If
                    End If

                    Cursor = Cursors.Default
                End If
            ElseIf tpBanks2.Focus Then
                'v_blnDirect = rbDirect.Checked
                'v_blnMultiPartite = rbMultiPartite.Checked
                v_strTxDate = Format(dtpTransDate2.Value, "dd/MM/yyyy")

                v_intFrequence = CInt(cboFrequence2.SelectedValue)
                Dim v_intCsErrType As Integer = CInt(cboCsErrType.SelectedValue)
                'end bangpv 

                v_strFilePath = v_strFilePath & Format(v_strTxDate, "ddMMyyyy") & "\"
                If Not Directory.Exists(v_strFilePath) Then
                    Directory.CreateDirectory(v_strFilePath)
                End If

                ExportViewNetBank2(v_intFrequence, v_strTxDate, v_strFilePath, "NHCDTT", v_intCsErrType)

                If v_lngError <> ERR_SYSTEM_OK Then
                    MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)

                    'Update mouse pointer
                    Cursor = Cursors.Default

                    Exit Sub
                Else
                    MsgBox(mv_ResourceManager.GetString("AddnewSuccessful"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                    Cursor = Cursors.Default
                End If
            ElseIf tpBanks3.Focus Then
                'v_blnDirect = rbDirect.Checked
                'v_blnMultiPartite = rbMultiPartite.Checked
                v_strTxDate = Format(dtpTransDate3.Value, "dd/MM/yyyy")

                ' v_intFrequence = CInt(cboFrequence2.SelectedValue)
                Dim v_intCsErrType As Integer = CInt(cboCsErrType.SelectedValue)
                'end bangpv 

                v_strFilePath = v_strFilePath & Format(v_strTxDate, "ddMMyyyy") & "\"
                If Not Directory.Exists(v_strFilePath) Then
                    Directory.CreateDirectory(v_strFilePath)
                End If

                ExportViewNetBank3(v_strTxDate, v_strFilePath, "NHCDTT")

                If v_lngError <> ERR_SYSTEM_OK Then
                    MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)

                    'Update mouse pointer
                    Cursor = Cursors.Default

                    Exit Sub
                Else
                    MsgBox(mv_ResourceManager.GetString("AddnewSuccessful"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                    Cursor = Cursors.Default
                End If
            ElseIf tpCW.Focus Then 'Ket xuat cover warrant 
                'v_blnDirect = rbDirect.Checked
                'v_blnMultiPartite = rbMultiPartite.Checked
                v_strSQL = "SELECT a.VARVALUE FROM sysvar a where a.GRNAME='SYSTEM' and a.VARNAME= 'CURRDATE' and BRID='0002'"
                v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLTX, gc_ActionInquiry, v_strSQL)
                v_lngError = Proxy.Message(v_strObjMsg)
                If v_lngError <> ERR_SYSTEM_OK Then
                    MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                    Exit Sub
                End If
                v_xmlDocument.LoadXml(v_strObjMsg)
                v_dtIndex = CDate(v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='VARVALUE']").InnerText)
                v_dCurrentDate = Replace(v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='VARVALUE']").InnerText, "/", "") 'CStr(Format(v_dtIndex, "ddMMyyyy"))
                v_strTxDate = Format(v_dtIndex, "dd/MM/yyyy")

                ' v_intFrequence = CInt(cboFrequence2.SelectedValue)

                'end bangpv 

                v_strFilePath = v_strFilePath & Format(v_strTxDate, "ddMMyyyy") & "\"
                If Not Directory.Exists(v_strFilePath) Then
                    Directory.CreateDirectory(v_strFilePath)
                End If
                ExportToXMLFile(True, v_strFilePath, "Broker", "CWBalance")


                If v_lngError <> ERR_SYSTEM_OK Then
                    MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)

                    'Update mouse pointer
                    Cursor = Cursors.Default

                    Exit Sub
                Else
                    MsgBox(mv_ResourceManager.GetString("AddnewSuccessful"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                    Cursor = Cursors.Default
                End If
            Else 'Ket xuat ngan hang chi dinh thanh toan
                v_blnDirect = rbDirect.Checked
                v_blnMultiPartite = rbMultiPartite.Checked
                v_strTxDate = Format(dtpTransDate.Value, "dd/MM/yyyy")
                'bangpv: lay t_no cua Thanh toan bu tru/truc tiep 
                If rbDirect.Checked = True Then
                    v_intFrequence = 3
                Else
                    v_intFrequence = CInt(cboFrequence.SelectedValue)
                End If
                'end bangpv 

                v_strFilePath = v_strFilePath & Format(v_strTxDate, "ddMMyyyy") & "\"
                If Not Directory.Exists(v_strFilePath) Then
                    Directory.CreateDirectory(v_strFilePath)
                End If

                ExportViewNetBank(v_blnMultiPartite, v_blnDirect, v_intFrequence, v_strTxDate, v_strFilePath, "NHCDTT")
                'v_strObjMsg = BuildXMLObjMsg(, BranchId, , Me.TellerId, gc_IsNotLocalMsg, gc_MsgTypeObj, OBJNAME_SY_AUTHENTICATION, gc_ActionInquiry, , , "UPDATE_BANK_STATUS", , )
                ''Dim v_lngError As String = mv_BDSDelivery.Message(v_strObjMsg)
                'v_lngError = mv_BDSDelivery.Message(v_strObjMsg)
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
                    MsgBox(mv_ResourceManager.GetString("AddnewSuccessful"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                    Cursor = Cursors.Default
                End If
            End If
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                          & "Error code: System error!" & vbNewLine _
                                          & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
            Cursor = Cursors.Default
        End Try
    End Sub
    Private Function SendFileFTP(ByVal FilePath As String, ByVal FileName As String, ByVal BrID As String) As Boolean
        Try
            Dim ServerAddress As String
            Dim ServerPort As String
            Dim Username As String
            Dim Password As String
            Dim RemotePath As String
            Dim v_strSQL As String
            Dim v_strObjMsg As String

            Dim v_xmlDocument As New XmlDocumentEx
            'Lấy thông số FTP
            v_strSQL = "SELECT a.VARNAME, a.VARVALUE FROM sysvar a where a.GRNAME='VSDFTPSVR'"
            v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLTX, gc_ActionInquiry, v_strSQL)
            Dim v_lngError As Long = Proxy.Message(v_strObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Exit Function
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
            'end bangpv 
            'If mv_xftpFTPEngine.Connect(ServerAddress, ServerPort, Username, Password) Then
            '    v_oProcess.StartProcessForm()
            '    v_oProcess.ChangeCaption("Đang gửi file")
            '    Dim v_blnSendFileStatus As Boolean = False
            '    v_blnSendFileStatus = mv_xftpFTPEngine.SendFile(RemotePath & "\" & FileName, FilePath & "\" & FileName)
            '    If v_blnSendFileStatus Then
            '        mv_xftpFTPEngine.Disconnect()
            '        v_oProcess.StopProcessForm()
            '        MsgBox("File đã được gửi tới " & RemotePath, MsgBoxStyle.OkOnly, "Thông báo gửi file FTP")
            '        Me.Close()
            '    Else
            '        v_oProcess.StopProcessForm()
            '        MsgBox("Lỗi trong quá trình gửi file lên máy chủ FTP, Vui lòng kiểm tra lại kết nối!", MsgBoxStyle.OkOnly, "Thông báo gửi file FTP")
            '        Return False
            '        Exit Function
            '    End If
            '    Return True
            'Else
            '    MsgBox("Không kết nối được tới máy chủ FTP, vui lòng kiểm tra lại cấu hình máy chủ FTP", MsgBoxStyle.Critical, "Lỗi kết nối máy chủ FTP")
            '    Return False
            '    Exit Function
            'End If

            'Dim v_oFTPClient As Utilities.FTP.FTPclient
            'v_oFTPClient = New Utilities.FTP.FTPclient(ServerAddress, Username, Password)

            'Dim v_blnUpload As Boolean = v_oFTPClient.Upload(FilePath & "\" & FileName, RemotePath & "\" & FileName)

            'If Not v_blnUpload Then
            '    MsgBox("Đẩy file lên máy chủ không thành công", MsgBoxStyle.Exclamation, "Thông báo")
            'End If


            Dim v_oWriter As System.IO.StreamWriter

            If CheckExistFile(FilePath, FileName.Split(".")(0) & ".bat") Then
                File.Delete(FilePath & "\" & FileName.Split(".")(0) & ".bat")
            End If

            v_oWriter = New StreamWriter(FilePath & "\" & FileName.Split(".")(0) & ".bat")
            v_oWriter.WriteLine("@ftp -i -s:""%~f0""&GOTO:EOF")
            v_oWriter.WriteLine("open " & ServerAddress)
            v_oWriter.WriteLine(Username)
            v_oWriter.WriteLine(Password)
            If Mid(FilePath, FilePath.Length, 1) = "\" Then
                v_oWriter.WriteLine("lcd " & Mid(FilePath, 1, FilePath.Length - 1))
            Else
                v_oWriter.WriteLine("lcd " & FilePath)
            End If

            v_oWriter.WriteLine("cd " & RemotePath)
            v_oWriter.WriteLine("binary")
            v_oWriter.WriteLine("put " & FileName & " " & FileName)
            v_oWriter.WriteLine("bye" & vbCrLf)

            v_oWriter.Close()

            'If Mid(FilePath, FilePath.Length, 1) = "\" Then
            '    FilePath = Mid(FilePath, 1, FilePath.Length - 1)
            'End If

            Dim v_oProcess As Process
            v_oProcess = New Process

            v_oProcess.StartInfo.FileName = FilePath & "\" & FileName.Split(".")(0) & ".bat"
            v_oProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
            v_oProcess.StartInfo.CreateNoWindow = True
            v_oProcess.Start()
            'v_oProcess.WaitForExit()
            v_oProcess.Close()

            MsgBox("Đẩy file lên máy chủ VSD thành công", MsgBoxStyle.Exclamation, "Thông báo")
            If CheckExistFile(FilePath, FileName.Split(".")(0) & ".bat") Then
                File.Delete(FilePath & "\" & FileName.Split(".")(0) & ".bat")
            End If
            v_strObjMsg = BuildXMLObjMsg(BusDate, BrID, , Me.TellerId, gc_IsNotLocalMsg, gc_MsgTypeObj, OBJNAME_SY_AUTHENTICATION, gc_ActionInquiry, , RemotePath & ":" & FileName, "SendFTPtoHNX", , )
            v_lngError = Proxy.Message(v_strObjMsg)

            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox("Đẩy file lên máy chủ HNX(hoặc HSX) không thành công", MsgBoxStyle.Critical, gc_ApplicationTitle)
                Cursor = Cursors.Default
                Return False
            Else
                MsgBox("Đẩy file lên máy chủ HNX(hoặc HSX) thành công", MsgBoxStyle.Exclamation, gc_ApplicationTitle)
                Return True
            End If
        Catch ex As Exception
            Return False
            Throw ex
        End Try
    End Function
    Private Sub OnClose()
        Me.Close()
    End Sub
#End Region

    Private Sub frmExportData_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        OnInit()
        BranchId = Trim(cboBranch.SelectedValue)
        BranchId2 = Trim(cboBranch2.SelectedValue)
        BranchId3 = Trim(cboBranch2.SelectedValue)
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        OnClose()
    End Sub

    Private Sub cboBranch_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboBranch.SelectedIndexChanged
        If Trim(cboBranch.SelectedValue.ToString) = "0002" Then
            cbSi.Enabled = False
            cbIs.Enabled = False
            cbMi.Enabled = False
        Else
            cbMi.Enabled = True
            cbSi.Enabled = True
            cbIs.Enabled = True
        End If
        BranchId = Trim(cboBranch.SelectedValue.ToString)
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        OnSubmit()
        'OnClose()
    End Sub

    Private Sub cboBranch2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboBranch2.SelectedIndexChanged
        'If Trim(cboBranch2.SelectedValue.ToString) = "0002" Then
        '    rbDirect.Enabled = True
        'Else
        '    rbDirect.Enabled = True
        'End If
        BranchId2 = Trim(cboBranch2.SelectedValue.ToString)
    End Sub
    Private Sub cboBranch3_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboBranch3.SelectedIndexChanged
        'If Trim(cboBranch2.SelectedValue.ToString) = "0002" Then
        '    rbDirect.Enabled = True
        'Else
        '    rbDirect.Enabled = True
        'End If
        BranchId3 = Trim(cboBranch3.SelectedValue.ToString)
    End Sub

    Private Sub rbDirect_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbDirect.CheckedChanged
        If rbDirect.Checked Then
            cboFrequence.Clears()
            cboFrequence.Enabled = False
        Else
            cboFrequence.Enabled = True
            cboFrequence.AddItems("T + 1", 2)
            cboFrequence.AddItems("T + 3", 1)
        End If
    End Sub
    Private Function GetPartition(ByVal pv_lstDate As String) As String
        Dim v_strMaxDate, v_strMinDate As String
        Dim v_arrDate() As String = pv_lstDate.Split("|")
        v_strMinDate = v_arrDate(0)
        v_strMaxDate = v_arrDate(0)

        If v_arrDate.Length > 1 Then
            v_strMinDate = v_arrDate(0)
            v_strMaxDate = v_arrDate(0)
            For i As Integer = 1 To v_arrDate.Length - 2
                If CheckPassDate(v_arrDate(i), v_strMinDate) Then
                    v_strMinDate = v_arrDate(i)
                End If
                If Not CheckPassDate(v_arrDate(i), v_strMaxDate) Then
                    v_strMaxDate = v_arrDate(i)
                End If
            Next
        End If
        Return v_strMinDate & "|" & v_strMaxDate
    End Function


End Class