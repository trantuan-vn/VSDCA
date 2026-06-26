Imports Sats.CommonLibrary
Imports Sats.SQLEngine
Imports System.Windows.Forms

Public Class frmRPParameter
    Private mv_DataSet As DataSet
    Private mv_strAuth As String
    Private mv_intRptLimit As Integer = 0
    Private mv_dCurrDate As Date
    Private mv_strIsSignCA As String
    Public Property Auth() As String
        Get
            Return mv_strAuth
        End Get
        Set(ByVal value As String)
            mv_strAuth = value
        End Set
    End Property
    Public Property CurrDate() As Date
        Get
            Return mv_dCurrDate
        End Get
        Set(ByVal value As Date)
            mv_dCurrDate = value
        End Set
    End Property
    Public Property MaxTransferRows() As Integer
        Get
            Return mv_intMaxTransferRows
        End Get
        Set(ByVal value As Integer)
            mv_intMaxTransferRows = value
        End Set
    End Property
    Private mv_intMaxTransferRows As Integer = 10000
    Public Sub New(ByRef pv_DataSet)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        mv_DataSet = pv_DataSet
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Protected Overrides Sub OnInit()
        Me.mskTransCode.Mask = "CC999"
        MyBase.OnInit()
        lblTranCode.Text = mv_ResourceManager.GetString("RPTCODE")
        Dim v_strClause As String = "SELECT a.VARVALUE FROM sysvar a where a.GRNAME='SYSTEM' and a.VARNAME= 'RPT_MAX_TRANSFER_ROWS' and BRID='0001'"
        Dim v_strObjMsg As String = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLTX, gc_ActionInquiry, v_strClause)
        Proxy.Message(v_strObjMsg)
        Dim v_xmlDocument As New XmlDocumentEx
        v_xmlDocument.LoadXml(v_strObjMsg)

        If Not v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='VARVALUE']") Is Nothing Then
            MaxTransferRows = Convert.ToInt32(v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='VARVALUE']").InnerXml)
        End If
    End Sub

    Protected Overrides Sub LoadScreen(ByVal strTLTXCD As String)
        Dim i, j As Integer
        Dim v_objField As CFieldMaster, v_objFieldVal As CFieldVal
        Dim v_strFieldName As String = "", v_strDefName As String = "", v_strCaption As String = ""
        Dim v_strFldType As String = "", v_strFldMask As String = "", v_strFldFormat As String = ""
        Dim v_strLList As String = "", v_strLChk As String = "", v_strDefVal As String = ""
        Dim v_strAmtExp As String = "", v_strValidTag As String = "", v_strLookUp As String = ""
        Dim v_strDataType As String = "", v_strControlType As String = "", v_strChainName As String = ""
        Dim v_strLookupName As String = "", v_strPrintInfo As String = "", v_strInvName As String = ""
        Dim v_strInvFormat As String = "", v_strFldSource As String = "", v_strFldDesc As String = ""
        Dim v_strSearchCode As String = "", v_strSrModCode As String = "", v_strMemberField As String = "", v_strStockField As String = "", v_strIsDuplicated As String = ""
        Dim v_intOdrNum, v_intFldLen, v_intIndex As Integer
        Dim v_blnVisible, v_blnEnabled, v_blnMandatory As Boolean
        Dim v_blnRisk As Boolean
        Dim v_strCAKey As String
        Try
            Dim v_strSQL As String
            Dim v_obj As New SQLDataAccessLayer(TellerId)
            Dim v_ds As DataSet
            v_strSQL = "SELECT * FROM RPREPORTS WHERE RPTID='" & strTLTXCD & "'"

            v_ds = v_obj.ExecuteReturnDataSet(v_strSQL)

            If v_ds.Tables(0).Rows.Count = 0 Then
                'Neu khong tim thay ma giao dich
                MsgBox("Bạn không có quyền truy nhập báo cáo này !", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, gc_ApplicationTitle)
                mv_bolCheck = False
                Exit Sub
            End If
            For i = 0 To v_ds.Tables(0).Rows.Count - 1
                With v_ds.Tables(0)
                    If UserLanguage <> "EN" Then
                        lblTransCaption.Text = .Rows(i)("RPTTITLE")
                        Me.TabText = Trim(.Rows(i)("RPTTITLE"))
                    Else
                        lblTransCaption.Text = .Rows(i)("EN_RPTTITLE")
                        Me.TabText = Trim(.Rows(i)("EN_RPTTITLE"))
                    End If

                    'Lấy giới hạn thời gian lấy báo cáo
                    mv_intRptLimit = CInt(.Rows(i)("RPTLIMIT"))
                    'Lấy xem có phải là báo cáo ký số không
                    mv_strIsSignCA = Trim(.Rows(i)("ISSIGNCA"))
                End With
            Next

            'Lay thong tin chi tiet cac truong cua giao dich
            v_strSQL = "SELECT * FROM FLDMASTER WHERE OBJNAME = '" & strTLTXCD & "' ORDER BY ODRNUM"
            v_ds = v_obj.ExecuteReturnDataSet(v_strSQL)

            ReDim mv_arrObjFields(v_ds.Tables(0).Rows.Count)

            For i = 0 To v_ds.Tables(0).Rows.Count - 1
                With v_ds.Tables(0)
                    v_strFieldName = Trim(.Rows(i)("FLDNAME"))
                    v_strDefName = Trim(.Rows(i)("DEFNAME"))
                    If UserLanguage <> "EN" Then
                        v_strCaption = Trim(.Rows(i)("CAPTION"))
                    Else
                        v_strCaption = Trim(.Rows(i)("EN_CAPTION"))
                    End If
                    v_intOdrNum = CInt(Trim(.Rows(i)("ODRNUM")))
                    v_strFldType = Trim(.Rows(i)("FLDTYPE"))
                    v_strFldMask = Trim(.Rows(i)("FLDMASK"))
                    v_strFldFormat = Trim(.Rows(i)("FLDFORMAT"))
                    v_intFldLen = CInt(Trim(.Rows(i)("FLDLEN")))
                    v_strLList = Trim(.Rows(i)("LLIST"))
                    v_strLList = v_strLList.Replace("?BRID", Me.BranchId)
                    v_strLList = v_strLList.Replace("?TLID", Me.TellerId)
                    v_strLList = v_strLList.Replace("?BUSDATE", Me.BusDate)
                    v_strLChk = Trim(.Rows(i)("LCHK"))
                    v_strDefVal = Trim(.Rows(i)("DEFVAL"))
                    v_blnVisible = (Trim(.Rows(i)("VISIBLE")) = "Y")
                    v_blnEnabled = (Trim(.Rows(i)("DISABLE")) = "N")
                    v_blnMandatory = (Trim(.Rows(i)("MANDATORY")) = "Y")
                    v_strAmtExp = Trim(.Rows(i)("AMTEXP"))
                    v_strValidTag = Trim(.Rows(i)("VALIDTAG"))
                    v_strLookUp = Trim(.Rows(i)("LOOKUP"))
                    v_strDataType = Trim(.Rows(i)("DATATYPE"))
                    v_strControlType = Trim(.Rows(i)("CTLTYPE"))
                    v_strInvName = Trim(.Rows(i)("INVNAME"))
                    v_strInvFormat = Trim(.Rows(i)("INVFORMAT"))
                    v_strFldSource = Trim(.Rows(i)("FLDSOURCE"))
                    v_strFldDesc = Trim(.Rows(i)("FLDDESC"))
                    v_strChainName = Trim(.Rows(i)("CHAINNAME"))
                    v_strLookupName = Trim(.Rows(i)("LOOKUPNAME"))
                    v_strSearchCode = Trim(.Rows(i)("SEARCHCODE"))
                    v_strSrModCode = Trim(.Rows(i)("MODCODE"))
                    v_strPrintInfo = .Rows(i)("PRINTINFO") 'KhÃ´ng Ä‘Æ°á»£c trim vÃ¬ Ä‘á»™ dÃ i báº¯t buá»™c 10 kÃ½ tá»±
                    v_strMemberField = Trim(.Rows(i)("MEMBERFIELD"))
                    v_strStockField = Trim(.Rows(i)("STOCKFIELD"))
                    v_blnRisk = ("Y" = .Rows(i)("RISKFLD"))
                    'bangpv 
                    'thêm trường phân biệt key cho báo cáo 
                    If Trim(.Rows(i)("SRMODCODE")) = "1" Then
                        v_strCAKey = Trim(.Rows(i)("SRMODCODE"))
                    Else
                        v_strCAKey = "0"
                    End If
                    'end bangpv
                End With

                v_objField = New CFieldMaster
                With v_objField
                    .FieldName = v_strFieldName
                    .ColumnName = v_strDefName
                    .Caption = v_strCaption
                    .DisplayOrder = v_intOdrNum
                    .FieldType = v_strFldType
                    .InputMask = v_strFldMask
                    .FieldFormat = v_strFldFormat
                    .FieldLength = v_intFldLen
                    .LookupList = v_strLList
                    .LookupCheck = v_strLChk
                    .LookupName = v_strLookupName
                    If v_strDefName = "DESC" And Len(v_strDefVal) = 0 Then
                        'Xu ly cho truong Description
                        v_strDefVal = Me.lblTransCaption.Text
                    ElseIf v_strDefVal = "<$BUSDATE>" Then
                        'Lay ngay lam viec hien taSi
                        v_strDefVal = Me.BusDate
                    End If

                    .DefaultValue = v_strDefVal
                    .Visible = v_blnVisible
                    .Enabled = v_blnEnabled
                    .Mandatory = v_blnMandatory
                    .AmtExp = v_strAmtExp
                    .ValidTag = v_strValidTag
                    .LookUp = v_strLookUp
                    .DataType = v_strDataType
                    .ControlType = v_strControlType
                    .InvName = v_strInvName
                    .InvFormat = v_strInvFormat
                    .FldSource = v_strFldSource
                    .FldDesc = v_strFldDesc
                    .PrintInfo = v_strPrintInfo
                    .SearchCode = v_strSearchCode
                    .SrModCode = v_strSrModCode
                    .FieldValue = String.Empty
                    .MemberField = v_strMemberField
                    .StockField = v_strStockField
                    .RiskField = v_blnRisk
                    'bangpv 
                    .CAKey = v_strCAKey
                    'end bangpv 
                End With
                mv_arrObjFields(i) = v_objField
            Next

            ReDim Preserve mv_arrObjFields(v_ds.Tables(0).Rows.Count)

            'Lay cac quy tac kiem tra cua cac truong giao dich
            v_strSQL = "SELECT FLDNAME, VALTYPE, OPERATOR, VALEXP, VALEXP2, ERRMSG, EN_ERRMSG FROM FLDVAL " & _
                "WHERE upper(OBJNAME) = '" & strTLTXCD & "' AND DELETED=0 AND STATUS=0 ORDER BY VALTYPE, FLDNAME, ODRNUM" 'Thá»© tá»± order by lÃ  quan trá»?ng khÃ´ng sá»­a

            v_ds = v_obj.ExecuteReturnDataSet(v_strSQL)
            ReDim mv_arrObjFldVals(v_ds.Tables(0).Rows.Count)
            Dim v_strFieldVal_FldName As String = "", v_strFieldVal_ValType As String = "", v_strFieldVal_Operator As String = ""
            Dim v_strFieldVal_ValExp As String = "", v_strFieldVal_ValExp2 As String = "", v_strFieldVal_ErrMsg As String = "", v_strFieldVal_EnErrMsg As String = ""

            For i = 0 To v_ds.Tables(0).Rows.Count - 1
                With v_ds.Tables(0)
                    'Ghi nhan thuat toan kiem tra va tinh toan cho tung truong cua giao dich
                    v_strFieldVal_FldName = Trim(.Rows(i)("FLDNAME"))
                    v_strFieldVal_ValType = Trim(.Rows(i)("VALTYPE"))
                    v_strFieldVal_Operator = Trim(.Rows(i)("OPERATOR"))
                    v_strFieldVal_ValExp = Trim(.Rows(i)("VALEXP"))
                    v_strFieldVal_ValExp2 = Trim(.Rows(i)("VALEXP2"))
                    v_strFieldVal_ErrMsg = Trim(.Rows(i)("ERRMSG"))
                    v_strFieldVal_EnErrMsg = Trim(.Rows(i)("EN_ERRMSG"))
                End With
            Next

            'Xac dinh index cua mang FldMaster
            For j = 0 To mv_arrObjFields.GetLength(0) - 1 Step 1
                If Not mv_arrObjFields(j) Is Nothing Then
                    If Trim(mv_arrObjFields(j).FieldName) = Trim(v_strFieldVal_FldName) Then
                        v_intIndex = j
                    End If
                End If
                'Dieu kien xu ly
                v_objFieldVal = New CFieldVal
                With v_objFieldVal
                    .OBJNAME = strTLTXCD
                    .FLDNAME = v_strFieldVal_FldName
                    .VALTYPE = v_strFieldVal_ValType
                    .mp_OPERATOR = v_strFieldVal_Operator
                    .VALEXP = v_strFieldVal_ValExp
                    .VALEXP2 = v_strFieldVal_ValExp2
                    .ERRMSG = v_strFieldVal_ErrMsg
                    .EN_ERRMSG = v_strFieldVal_EnErrMsg
                    .IDXFLD = v_intIndex
                End With
                mv_arrObjFldVals(i) = v_objFieldVal
            Next
            ReDim Preserve mv_arrObjFldVals(v_ds.Tables(0).Rows.Count)

            v_ds.Dispose()
            v_obj.CloseConnection()
            v_obj = Nothing
            DisplayScreen()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Function VerifyRules() As Boolean
        Try
            Dim v_xmlDocument As New Xml.XmlDocument, v_dataElement As Xml.XmlElement, v_entryNode As Xml.XmlNode
            Dim v_strFLDNAME, v_strFLDVALUE As String
            Dim v_ctl As Control, v_attrFLDNAME As Xml.XmlAttribute, v_attrDATATYPE As Xml.XmlAttribute, v_objEval As New Evaluator
            Dim v_strFieldValue, strFLDNAME As String
            Dim v_intIndex As Integer
            Dim v_strErrMsg As String = ""
            Dim strCheckSQL As String = ""
            Dim v_nodeList As Xml.XmlNodeList
            Dim v_strClause, v_strObjMsg As String

            'v_strClause = "SELECT a.VARVALUE FROM sysvar a where a.GRNAME='SYSTEM' and a.VARNAME= 'BRSTATUS' and BRID='" & Me.BranchId & "'"
            'v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLTX, gc_ActionInquiry, v_strClause)
            'Proxy.Message(v_strObjMsg)
            'v_xmlDocument.LoadXml(v_strObjMsg)
            'If v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='VARVALUE']").InnerText <> OPERATION_ACTIVE Then
            '    v_oProcess.StopProcessForm()
            '    Application.DoEvents()
            '    MsgBox(mv_ResourceManager.GetString("ErrorMsg"), MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, mv_ResourceManager.GetString("Title"))
            '    mv_bolCheck = False
            '    Return False
            'End If

            'Check data control before commit          
            For Each v_control As Control In Me.pnTransDetail.Controls
                If InStr(CType(v_control, Control).Name, PREFIXED_MSKDATA) > 0 Then
                    v_intIndex = CType(v_control, Control).Tag
                    If Not mv_arrObjFields(v_intIndex) Is Nothing Then
                        If (TypeOf (v_control) Is ComboBoxEx) Then
                            v_strFieldValue = CType(v_control, ComboBox).SelectedValue
                        ElseIf (TypeOf (v_control) Is ucGridControl) Then
                            v_strFieldValue = CType(v_control, ucGridControl).GridValue
                        Else
                            v_strFieldValue = IIf(Trim(mv_arrObjFields(v_intIndex).DataType) = "D", Trim(v_control.Text).Replace("/  /", ""), Trim(v_control.Text))
                            v_strFieldValue = IIf(Trim(mv_arrObjFields(v_intIndex).DataType) = "P", Trim(v_control.Text).Replace("/  /", ""), Trim(v_control.Text))
                            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("vi-VN")
                            If (mv_arrObjFields(v_intIndex).DataType = "D" Or mv_arrObjFields(v_intIndex).DataType = "P") And mv_intRptLimit > 0 Then
                                If CDate(v_strFieldValue).AddMonths(mv_intRptLimit) < CDate(MyBase.BusDate) Then
                                    v_oProcess.StopProcessForm()
                                    Application.DoEvents()
                                    MessageBox.Show(Replace(Replace(mv_ResourceManager.GetString("ERR_INVALID_DATE_LIMIT"), "@CAPTION", mv_arrObjFields(v_intIndex).Caption), "@RPTLIMIT", mv_intRptLimit), gc_ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Information)
                                    mv_bolCheck = False
                                    v_control.Focus()
                                    Return False
                                End If
                            End If
                            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US")
                        End If
                        strFLDNAME = Mid(CType(v_control, Control).Name, Len(PREFIXED_MSKDATA) + 1)
                        If (mv_arrObjFields(v_intIndex).Mandatory = True And mv_arrObjFields(v_intIndex).Visible = True And mv_arrObjFields(v_intIndex).Enabled = True _
                                And Len(v_strFieldValue) = 0) Or Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).ForeColor = System.Drawing.Color.Red Then
                            v_oProcess.StopProcessForm()
                            Application.DoEvents()
                            MessageBox.Show(mv_arrObjFields(v_intIndex).Caption & mv_ResourceManager.GetString("ERR_INVALID_VALUE"), gc_ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Information)
                            mv_bolCheck = False
                            v_control.Focus()
                            Return False
                        End If
                    End If
                End If
            Next

            ' Duyệt mảng dữ liệu đánh mốc các điều kiện kiểm tra

            If Not CheckFldvals(mv_arrObjFldVals) Then
                Return False
            End If

            Return True
        Catch ex As Exception
            'LogError.Write(ex.Message & vbNewLine & ex.StackTrace, EventLogEntryType.Error)
            v_oProcess.StopProcessForm()
            Application.DoEvents()
            'MsgBox(ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
            'mv_bolCheck = False
            Throw ex
        End Try
    End Function

    Protected Overrides Sub OnSubmit()
        v_oProcess = New ProcessForm(Me)
        btnOK.Enabled = False
        btnCancel.Enabled = False
        Try
            'v_thread = New Threading.Thread(AddressOf ShowProcessForm)

            Dim v_strObjMsg As String = ""

            Dim v_strRptID As String
            Dim v_strErrorSource As String = "", v_strErrorMessage As String = ""
            Dim v_lngError As Long = ERR_SYSTEM_OK
            Dim v_xmlDocument As New Xml.XmlDocument
            ' Load screen if pnTransDetail.Controls.Count = 0
            If Me.pnTransDetail.Controls.Count = 0 Then
                If Len(Trim(mskTransCode.Text)) = 4 Then
                    mv_xmlDocumentInquiryData.RemoveAll()
                    LoadScreen(Trim(mskTransCode.Text))
                End If
            Else
                v_oProcess.StartProcessForm()
                v_strRptID = Trim(mskTransCode.Text)

                If Not VerifyRules() Then
                    Exit Sub
                End If
                Dim v_intindex As Integer
                Dim v_strFLDNAME, v_strFLDVALUE As String
                Dim v_strClause As String


                v_strClause = v_strRptID & "#"
                Dim v_strClause1 As String = v_strRptID & "#"
                Dim v_strDisplay As String
                Dim v_strPartition As String = ""
                Dim v_lstSICODE, v_lstMICODE As String
                v_lstSICODE = mv_strStockFilter
                v_lstMICODE = mv_strMemberFilter
                'Tao array parameter
                For Each v_control As Control In Me.pnTransDetail.Controls
                    If InStr(CType(v_control, Control).Name, PREFIXED_MSKDATA) > 0 Then
                        v_intindex = CType(v_control, Control).Tag
                        If Not mv_arrObjFields(v_intindex) Is Nothing Then
                            If TypeOf (v_control) Is ComboBoxEx Then
                                v_strFLDVALUE = CType(v_control, ComboBoxEx).SelectedValue
                                v_strDisplay = CType(v_control, ComboBoxEx).Text

                            ElseIf (TypeOf (v_control) Is ucGridControl) Then
                                v_strFLDVALUE = CType(v_control, ucGridControl).GridValue
                                v_strDisplay = v_strFLDVALUE
                            Else
                                v_strFLDVALUE = v_control.Text 'CType(v_control, Control).Text
                                v_strDisplay = v_strFLDVALUE
                            End If
                            v_strFLDNAME = Mid(CType(v_control, Control).Name, Len(PREFIXED_MSKDATA) + 1)

                            With mv_arrObjFields(v_intindex)
                                'edited by bangpv 
                                'v_strClause1 = v_strClause1 & .FieldName & "|" & v_strFLDVALUE & "|" & v_strDisplay & "|" & .DataType & "|" & .ColumnName & "$"
                                v_strClause1 = v_strClause1 & .FieldName & "|" & v_strFLDVALUE & "|" & v_strDisplay & "|" & .DataType & "|" & .ColumnName & "|" & .CAKey & "$"
                                'end 
                                If v_strFLDVALUE = "-1" And mv_arrObjFields(v_intindex).RiskField Then
                                    v_strFLDVALUE = GetListCombo(CType(v_control, ComboBoxEx), mv_arrObjFields(v_intindex).DataType)
                                Else
                                    If mv_arrObjFields(v_intindex).RiskField And v_strFLDVALUE <> "-1" Then
                                        If mv_arrObjFields(v_intindex).DataType = "N" Then
                                            v_strFLDVALUE = "(" & v_strFLDVALUE & ")"
                                        Else
                                            v_strFLDVALUE = "('" & v_strFLDVALUE & "')"
                                        End If
                                    End If
                                End If

                                If .ColumnName = "SICODE" Then
                                    If mv_arrObjFields(v_intindex).RiskField Then
                                        v_lstSICODE = v_strFLDVALUE
                                    Else
                                        v_lstSICODE = "('" & v_strFLDVALUE & "')"
                                    End If

                                ElseIf .ColumnName = "MICODE" Then
                                    If mv_arrObjFields(v_intindex).RiskField Then
                                        v_lstMICODE = v_strFLDVALUE
                                    Else
                                        v_lstMICODE = "('" & v_strFLDVALUE & "')"
                                    End If

                                End If

                                If .DataType = "P" Then
                                    v_strPartition &= v_strFLDVALUE & "|"
                                End If
                                v_strClause = v_strClause & .FieldName & "|" & v_strFLDVALUE & "|" & .ColumnName & "|" '& v_strDisplay & "|" & .CAKey & "$"
                                'bangpv
                                If v_strDisplay = "<<Tất cả>>" Then
                                    v_strClause = v_strClause & "all|" & .CAKey & "$"
                                Else
                                    If .DataType = "P" Then
                                        v_strClause = v_strClause & Replace(v_strFLDVALUE, "/", "") & "|" & .CAKey & "$"
                                    Else
                                        v_strClause = v_strClause & v_strFLDVALUE & "|" & .CAKey & "$"
                                    End If
                                End If
                                'end bangpv 
                            End With
                        End If
                    End If
                Next

                'Kiem tra ngay lay DL co vuot qua DL TimesTen hay khong?
                Dim v_lstPartition As String
                Dim v_strMinDate As String
                Dim v_strMaxDate As String
                If v_strPartition <> "" Then
                    v_lstPartition = GetPartition(v_strPartition)
                    v_strMinDate = v_lstPartition.Split("|")(0)

                    If CheckPassDate(v_strMinDate, "01/01/2000") Then
                        Me.Cursor = Cursors.Default
                        v_oProcess.StopProcessForm()
                        Application.DoEvents()
                        MsgBox(mv_ResourceManager.GetString("ErrPartition"), MsgBoxStyle.Critical, gc_ApplicationTitle)
                        Exit Sub
                    End If
                    v_strMaxDate = v_lstPartition.Split("|")(1)

                    If CheckPassDate(BusDate, v_strMaxDate) Then
                        Me.Cursor = Cursors.Default
                        v_oProcess.StopProcessForm()
                        Application.DoEvents()
                        MsgBox(mv_ResourceManager.GetString("ErrPartition"), MsgBoxStyle.Critical, gc_ApplicationTitle)
                        Exit Sub
                    End If
                End If
                'EDIT BO SUNG CLAUSE1 20230219
                v_strObjMsg = BuildXMLObjMsg(Me.BusDate, BranchId, v_strClause1, TellerId, gc_IsNotLocalMsg, gc_MsgTypeObj, OBJNAME_RP_RPREPORT, _
                gc_ActionAdhoc, v_lstSICODE & "|" & v_lstMICODE, v_strClause, "HOSTCreateReport", , v_lstPartition, , , , , mv_strIsSignCA, TellerName)

                'EDIT BO SUNG CLAUSE1 20230219

                Dim v_ds As DataSet
                'v_lngError = mv_BDSDelivery.Message(v_strObjMsg)
                Dim v_strSQL As String = "select authid, tltxcd from tlcaauth where tltxcd = '" & ObjectName _
                           & "' and authid in (select grpid from tlgrpusers where tlid ='" & TellerId & "')"

                Dim v_strObjMsg1 As String = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLGROUPS, gc_ActionInquiry, v_strSQL)
                v_lngError = Proxy.Message(v_strObjMsg1)
                'Dim v_nodeList As Xml.XmlNodeList

                If v_lngError <> ERR_SYSTEM_OK Then
                    MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                    Exit Sub
                End If
                v_xmlDocument.LoadXml(v_strObjMsg1)
                Dim v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")
                'Dim v_lstSICODEAllow As String = ""
                'Dim v_intServerRptExp As Integer = Proxy.GetServerRptExp(v_strRptID, TellerName, v_lstSICODE, v_lstSICODEAllow)
                'If v_intServerRptExp > 0 Then
                '    v_oProcess.StopProcessForm()
                '    Dim v_lstTmp As String = ""
                '    If v_lstSICODEAllow <> v_lstSICODE Then
                '        Dim v_arrSICODE() As String = v_lstSICODE.Substring(1, v_lstSICODE.Length - 2).Split(",")
                '        Dim v_arrSICODEAllow() As String = v_lstSICODEAllow.Split(",")

                '        For Each v_strTmp In v_arrSICODE
                '            If Not v_arrSICODEAllow.Contains(v_strTmp) Then
                '                v_lstTmp = v_lstTmp & "," & v_strTmp
                '            End If
                '        Next
                '    End If
                '    If v_lstTmp <> "" Then
                '        MsgBox(String.Format("Bạn không có quyền kết xuất các mã chứng khoán {0} trên máy chủ!", v_lstTmp), MsgBoxStyle.Exclamation, gc_ApplicationTitle)
                '        Exit Sub
                '    End If
                '    If MsgBox("Báo cáo sẽ được kết xuất trên máy chủ và đẩy lên máy chủ FTP. Bạn có muốn tiếp tục không?", MsgBoxStyle.OkCancel, gc_ApplicationTitle) = MsgBoxResult.Ok Then
                '        v_oProcess.StartProcessForm()
                '        v_lngError = Proxy.RptExpMessage(v_strObjMsg)
                '        If v_lngError <> ERR_SYSTEM_OK Then
                '            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                '            Exit Sub
                '        Else
                '            v_oProcess.StopProcessForm()
                '            MsgBox("Báo cáo sẽ được kết xuất thành công!", MsgBoxStyle.Information, gc_ApplicationTitle)
                '        End If
                '    Else
                '        Exit Sub
                '    End If
                'Else
                If mv_strIsSignCA = "1" And v_nodeList.Count > 0 Then
                    v_ds = Proxy.RptMessageCA(v_strObjMsg, MaxTransferRows)
                Else
                    v_ds = Proxy.RptMessage(v_strObjMsg, MaxTransferRows)
                End If
                'BangPV: Create csv file
                'Dim v_strpathfile As String = Dataset_to_CSV(v_ds, v_strRptID)
                'End BangPV
                If v_ds Is Nothing Then
                    Cursor.Current = Cursors.Default
                    v_oProcess.StopProcessForm()
                    Application.DoEvents()
                    MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                    Exit Sub
                End If
                ' ShowRptFromByte(v_arrByte)
                FillFormuleToDataSet(v_strClause1)
                If FillDataSet(v_ds, v_strRptID) Then
                    v_oProcess.ChangeCaption("Đang in báo cáo .....")
                    'edit bangpv: Truyen tham so de export bao cao
                    ShowReport(v_strRptID, v_strObjMsg, v_lstSICODE)
                    'end edit 
                Else
                    v_oProcess.StopProcessForm()
                    Application.DoEvents()
                    MsgBox("Không có dữ liệu báo cáo!", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, IIf(Me.UserLanguage = "VN", "Giao dịch", "Transaction"))
                    Exit Sub
                End If
                'End If
            End If
            v_oProcess.StopProcessForm()
            Application.DoEvents()
            Me.Cursor = Cursors.Default
        Catch ex As Exception
            v_oProcess.StopProcessForm()
            Application.DoEvents()
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                                     & "Error code: System error!" & vbNewLine _
                                                     & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
            mv_bolCheck = False
        Finally
            v_oProcess.StopProcessForm()
            Application.DoEvents()
            v_oProcess = Nothing
            btnCancel.Enabled = True
            btnOK.Enabled = True
            Me.Focus()
        End Try
    End Sub

    Private Function GetListCombo(ByVal pv_cbo As ComboBoxEx, ByVal pv_strType As String) As String
        Try
            Dim v_lst As String = ""
            Dim v_intIndex As Integer = pv_cbo.SelectedIndex
            For v_int As Integer = 0 To pv_cbo.Items.Count - 1
                pv_cbo.SelectedIndex = v_int
                If pv_cbo.SelectedValue <> "-1" Then
                    If pv_strType = "N" Then
                        v_lst &= "," & pv_cbo.SelectedValue.ToString
                    Else
                        v_lst &= ",'" & pv_cbo.SelectedValue.ToString & "'"
                    End If
                End If
            Next

            pv_cbo.SelectedIndex = v_intIndex

            If v_lst <> "" Then
                Return "(" & Mid(v_lst, 2) & ")"
            Else
                Return ""
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Sub ShowRptFromByte(ByVal pv_arrByte() As Byte)
        Dim v_frm As New frmReportView
        v_frm.arViewer.Document.Content = pv_arrByte
        v_frm.UserLanguage = UserLanguage
        v_frm.Print = ("Y" = Mid(mv_strAuth, 3, 1))
        v_frm.Show()
    End Sub

    Private Sub ShowReport(ByVal pv_strRptID As String, ByVal v_strObjMsg As String, ByVal pv_lstSICODE As String)
        Try
            Dim v_objPageSetting As New ReportSetting
            Dim v_obj As New SQLDataAccessLayer(TellerId)
            Dim v_strsql As String

            v_objPageSetting.ReportID = pv_strRptID

            If Mid(mv_strAuth, 2, 1) <> "Y" Then
                v_oProcess.StopProcessForm()
                Application.DoEvents()
                MsgBox("Bạn không có quyền xem báo cáo này!", MsgBoxStyle.Exclamation)
                Exit Sub
            End If

            Dim v_ds As DataSet

            ' v_strsql = "SELECT * FROM RPREPORTS WHERE RPTID='" & pv_strRptID & "'"
            v_strsql = "SELECT     rpttitle, en_rpttitle, rptcmdsql, objname, orderbycmdsql, rpttype, orientation, dsn, createby, createdate, modcode, rptid,CONVERT(NUMERIC(5,2), title_height) AS title_height, " _
           & "CONVERT(NUMERIC(5,2), header_height) AS header_height,CONVERT(NUMERIC(5,2), footer_height) AS footer_height, " _
           & " autoid, deleted, status, tllogtran, rpfontsize, rppapersize, ismember, rptcmdsql1, rptlimit,CONVERT(NUMERIC(5,2), datarowheight) AS datarowheight, issignca" _
           & " FROM rpreports WHERE RPTID='" & pv_strRptID & "'"

            v_ds = v_obj.ExecuteReturnDataSet(v_strsql)
            Dim v_intIsMember As Integer
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US")
            Dim i As Integer
            For i = 0 To v_ds.Tables(0).Rows.Count - 1
                With v_ds.Tables(0)
                    v_objPageSetting.Title = .Rows(i)("RPTTITLE")
                    v_objPageSetting.En_Title = .Rows(i)("EN_RPTTITLE")
                    v_objPageSetting.ReportType = .Rows(i)("RPTTYPE")
                    v_objPageSetting.Orientation = .Rows(i)("ORIENTATION")
                    v_objPageSetting.DSN = .Rows(i)("DSN")
                    v_objPageSetting.ObjectName = .Rows(i)("OBJNAME")
                    v_objPageSetting.HeaderHeight = CDbl(.Rows(i)("TITLE_HEIGHT"))
                    v_objPageSetting.HHeight = CDbl(.Rows(i)("HEADER_HEIGHT"))
                    v_objPageSetting.FHeight = CDbl(.Rows(i)("FOOTER_HEIGHT"))
                    'BangPV: Lấy thêm báo cáo có ký số hay không
                    v_objPageSetting.IsSignCA = .Rows(i)("ISSIGNCA")
                    'End BangPV
                    If .Rows(i)("RPFONTSIZE") = "" Then
                        v_objPageSetting.FontSize = "9.75"
                    Else
                        v_objPageSetting.FontSize = .Rows(i)("RPFONTSIZE")
                    End If
                    If .Rows(i)("DATAROWHEIGHT") = 0 Then
                        v_objPageSetting.DataRowHeight = 0.25
                    Else
                        v_objPageSetting.DataRowHeight = CDbl(.Rows(i)("DATAROWHEIGHT"))
                    End If
                    If .Rows(i)("RPPAPERSIZE") = "" Then
                        v_objPageSetting.PaperSize = "A4"
                    Else
                        v_objPageSetting.PaperSize = .Rows(i)("RPPAPERSIZE")
                    End If
                    v_objPageSetting.BusDate = Me.BusDate

                    v_intIsMember = .Rows(0)("ISMEMBER")
                End With
            Next
            v_objPageSetting.UserLanguage = Me.UserLanguage
            'v_objPageSetting.ReportID = Replace(Trim(CType(SearchGrid.CurrentRow, Xceed.Grid.DataRow).Cells(KeyColumn).Value), ".", String.Empty)

            If v_intIsMember = 0 Then
                v_strsql = "SELECT * FROM BRGRP WHERE BRID='" & BranchId & "'"
                v_ds = v_obj.ExecuteReturnDataSet(v_strsql)

                v_objPageSetting.CompanyName = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("CompanyName")).ToUpper
                v_objPageSetting.Address = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("Address"))
                v_objPageSetting.BranchName = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("BranchName")).ToUpper
                v_objPageSetting.WhereDate = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("WhereDate"))
            Else
                v_strsql = "SELECT * FROM RGMI WHERE MICODE<>'000'"
                v_ds = v_obj.ExecuteReturnDataSet(v_strsql)

                v_objPageSetting.MIName = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("NAME")).ToUpper
                v_objPageSetting.MIAddress = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("ADDRESS"))
                v_objPageSetting.BUSINESS_NO = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("BUSINESS_NO"))
                v_objPageSetting.MIPhone = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("PHONE"))
            End If

            'Lay cac truong bao cao
            'v_strsql = "SELECT * FROM RPFLD WHERE RPTID='" & pv_strRptID & "' ORDER BY POSITION"
            v_strsql = "SELECT AUTOID,PARENT_ID,FIELDNAME,FIELDTYPE,CAPTION,EN_CAPTION,FORMAT,DISPLAY, " _
                  & "CONVERT(NUMERIC(5,2), WIDTH) AS WIDTH,ISDATAFIELD,ISSUM,ISPARENT,ALIGN,LEV,CONVERT(NUMERIC(5,2),HEIGHT) AS HEIGHT FROM RPFLD WHERE RPTID='" & v_objPageSetting.ReportID & "' ORDER BY POSITION"
            v_ds = v_obj.ExecuteReturnDataSet(v_strsql)
            Dim v_objTitleHeader As ReportHeaderRow
            Dim v_arrTitleHeader(v_ds.Tables(0).Rows.Count - 1) As ReportHeaderRow

            For v_int As Integer = 0 To v_ds.Tables(0).Rows.Count - 1
                v_objTitleHeader = New ReportHeaderRow
                With v_ds.Tables(0)
                    v_objTitleHeader.ID = .Rows(v_int)("AUTOID")
                    v_objTitleHeader.ParentID = .Rows(v_int)("PARENT_ID")
                    v_objTitleHeader.FieldName = .Rows(v_int)("FIELDNAME")
                    v_objTitleHeader.FieldType = .Rows(v_int)("FIELDTYPE")
                    v_objTitleHeader.Caption = .Rows(v_int)("CAPTION")
                    v_objTitleHeader.En_Caption = .Rows(v_int)("EN_CAPTION")
                    v_objTitleHeader.Format = .Rows(v_int)("FORMAT")
                    v_objTitleHeader.Display = .Rows(v_int)("DISPLAY")
                    v_objTitleHeader.Width = CDbl(.Rows(v_int)("WIDTH"))
                    v_objTitleHeader.IsDataField = .Rows(v_int)("ISDATAFIELD")
                    v_objTitleHeader.IsSum = .Rows(v_int)("ISSUM")
                    v_objTitleHeader.IsParent = .Rows(v_int)("ISPARENT")
                    v_objTitleHeader.TextAlign = .Rows(v_int)("ALIGN")
                    v_objTitleHeader.Lev = .Rows(v_int)("LEV")
                    v_objTitleHeader.Height = CDbl(.Rows(v_int)("HEIGHT"))
                End With
                v_arrTitleHeader(v_int) = v_objTitleHeader
            Next

            v_objPageSetting.ReportFld = v_arrTitleHeader
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("vi-VN")
            'Lay group bao cao
            v_strsql = "SELECT * FROM RPGRP WHERE RPTID='" & pv_strRptID & "' ORDER BY POSITION"
            v_ds = v_obj.ExecuteReturnDataSet(v_strsql)
            If v_ds.Tables(0).Rows.Count > 0 Then
                Dim v_objGroup As ReportGroup
                Dim v_arrGroup(v_ds.Tables(0).Rows.Count - 1) As ReportGroup

                For v_int As Integer = 0 To v_ds.Tables(0).Rows.Count - 1
                    v_objGroup = New ReportGroup
                    With v_ds.Tables(0)
                        v_objGroup.FieldName = .Rows(v_int)("FIELDNAME")
                        v_objGroup.FldType = .Rows(v_int)("FIELDTYPE")
                        v_objGroup.Caption = .Rows(v_int)("CATION")
                        v_objGroup.En_Caption = .Rows(v_int)("EN_CATION")
                        v_objGroup.Format = .Rows(v_int)("FORMAT")
                        v_objGroup.CaptionWidth = CDbl(.Rows(v_int)("WIDTH"))
                        v_objGroup.Footer = .Rows(v_int)("GRPFOOTER")
                    End With
                    v_arrGroup(v_int) = v_objGroup
                Next
                v_objPageSetting.ReportGrp = v_arrGroup
            End If

            v_obj.CloseConnection()
            v_obj = Nothing
            Dim frm As New AppCore.frmReportView
            frm.ReportSetting = v_objPageSetting
            frm.ClientDataSet = mv_DataSet
            frm.UserLanguage = UserLanguage
            frm.Print = ("Y" = Mid(mv_strAuth, 3, 1))
            frm.Export = ("Y" = Mid(mv_strAuth, 4, 1))


            v_oProcess.StopProcessForm()
            Application.DoEvents()

            'bangpv: export report to FTP
            frm.Proxy = Proxy
            frm.ReportId = pv_strRptID
            frm.ListSicode = pv_lstSICODE
            frm.TellerName = TellerName
            frm.ShowReport(v_strObjMsg)
            'end BangPV 

            frm.Show()

        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                                     & "Error code: System error!" & vbNewLine _
                                                     & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Sub

    Private Function FillDataSet(ByVal v_strObjMsg As String, ByVal pv_strRptID As String) As Boolean
        'Dim v_ds As New DataSet("Object")
        'Dim v_strObjMsg As String
        Dim pv_xmlDoc As New XmlDocumentEx
        'Dim v_nodeList As Xml.XmlNodeList
        Dim v_XmlNode As Xml.XmlNode
        Dim v_oDataRow As DataRow
        Dim v_oDataColumn As DataColumn
        Dim v_intCountRow, v_intCountCol As Integer
        Dim v_bln As Boolean = True
        'Dim i, j As Integer

        pv_xmlDoc.LoadXml(v_strObjMsg)
        v_intCountRow = pv_xmlDoc.FirstChild.ChildNodes.Count
        If (v_intCountRow > 0) Then
            v_intCountCol = pv_xmlDoc.FirstChild.FirstChild.ChildNodes.Count

            If mv_DataSet.Tables.IndexOf(pv_strRptID) <> -1 Then
                mv_DataSet.Tables.Remove(pv_strRptID)
            End If
            mv_DataSet.Tables.Add(pv_strRptID)

            For i As Integer = 0 To v_intCountCol - 1
                v_oDataColumn = New DataColumn(pv_xmlDoc.FirstChild.FirstChild.ChildNodes(i).Attributes("fldname").InnerText)
                v_oDataColumn.ColumnName = pv_xmlDoc.FirstChild.FirstChild.ChildNodes(i).Attributes("fldname").InnerText

                Select Case pv_xmlDoc.FirstChild.FirstChild.ChildNodes(i).Attributes("fldtype").InnerText
                    Case "System.Decimal"
                        v_oDataColumn.DataType = GetType(System.Decimal)
                    Case "System.String"
                        v_oDataColumn.DataType = GetType(System.String)
                    Case "System.Double"
                        v_oDataColumn.DataType = GetType(System.Double)
                    Case "System.DateTime"
                        v_oDataColumn.DataType = GetType(System.DateTime)
                    Case Else
                        v_oDataColumn.DataType = GetType(System.String)
                End Select

                mv_DataSet.Tables(pv_strRptID).Columns.Add(v_oDataColumn)
            Next
            'End If

            v_XmlNode = pv_xmlDoc.FirstChild
            mv_DataSet.Tables(pv_strRptID).Clear()
            For j As Integer = 0 To v_intCountRow - 1

                v_oDataRow = mv_DataSet.Tables(pv_strRptID).NewRow()
                For i As Integer = 0 To v_intCountCol - 1
                    ' tuanta
                    Select Case v_XmlNode.FirstChild.ChildNodes(i).Attributes("fldtype").InnerText
                        Case "System.Decimal"
                            If v_XmlNode.ChildNodes(j).ChildNodes(i).InnerText = "" Then
                                v_oDataRow(i) = DBNull.Value
                            Else
                                v_oDataRow(i) = CType(v_XmlNode.ChildNodes(j).ChildNodes(i).InnerText, System.Decimal)
                            End If
                        Case "System.String"
                            If v_XmlNode.ChildNodes(j).ChildNodes(i).InnerText = "" Then
                                v_oDataRow(i) = DBNull.Value
                            Else
                                v_oDataRow(i) = CType(v_XmlNode.ChildNodes(j).ChildNodes(i).InnerText, System.String)
                            End If
                        Case "System.Double"
                            If v_XmlNode.ChildNodes(j).ChildNodes(i).InnerText = "" Then
                                v_oDataRow(i) = DBNull.Value
                            Else
                                v_oDataRow(i) = CType(v_XmlNode.ChildNodes(j).ChildNodes(i).InnerText, System.Double)
                            End If
                        Case "System.DateTime"
                            If v_XmlNode.ChildNodes(j).ChildNodes(i).InnerText = "" Then
                                v_oDataRow(i) = DBNull.Value
                            Else
                                v_oDataRow(i) = CType(v_XmlNode.ChildNodes(j).ChildNodes(i).InnerText, System.DateTime)
                            End If
                        Case Else
                            If v_XmlNode.ChildNodes(j).ChildNodes(i).InnerText = "" Then
                                v_oDataRow(i) = DBNull.Value
                            Else
                                v_oDataRow(i) = CType(v_XmlNode.ChildNodes(j).ChildNodes(i).InnerText, System.String)
                            End If
                    End Select
                    'v_oDataRow(i) = Trim(v_XmlNode.ChildNodes(j).ChildNodes(i).InnerText)
                    ' end tuanta
                Next
                mv_DataSet.Tables(pv_strRptID).Rows.Add(v_oDataRow)
            Next
        Else
            v_bln = False
        End If
        Return v_bln
    End Function


    Private Function FillDataSet(ByVal pv_ds As DataSet, ByVal pv_strRptID As String) As Boolean
        Try
            If pv_ds Is Nothing Then
                Return False
            End If

            If pv_ds.Tables(0).Rows.Count > 0 Then
                If mv_DataSet.Tables.IndexOf(pv_strRptID) <> -1 Then
                    mv_DataSet.Tables.Remove(pv_strRptID)
                End If
                pv_ds.Tables(0).TableName = pv_strRptID

                mv_DataSet.Tables.Add(pv_ds.Tables(0).Copy)
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Sub FillFormuleToDataSet(ByVal pv_strClause As String)
        Dim v_oDataRow As DataRow
        Dim v_oDataColumn As DataColumn
        Dim v_strRptID As String
        Dim v_arrField() As String
        Dim v_arrTemp() As String

        v_strRptID = pv_strClause.Split("#")(0)
        v_arrField = pv_strClause.Split("#")(1).Split("$")
        If mv_DataSet.Tables.IndexOf("F_" & v_strRptID) <> -1 Then
            mv_DataSet.Tables.Remove("F_" & v_strRptID)
        End If

        'If mv_DataSet.Tables.IndexOf("F_" & v_strRptID) = -1 Then
        mv_DataSet.Tables.Add("F_" & v_strRptID)

        For i As Integer = 0 To v_arrField.Count - 2
            v_arrTemp = v_arrField(i).Split("|")
            v_oDataColumn = New DataColumn("V_" & v_arrTemp(0))
            v_oDataColumn.ColumnName = "V_" & v_arrTemp(0)
            v_oDataColumn.DataType = GetType(System.String)
            mv_DataSet.Tables("F_" & v_strRptID).Columns.Add(v_oDataColumn)

            v_oDataColumn = New DataColumn("D_" & v_arrTemp(0))
            v_oDataColumn.ColumnName = "D_" & v_arrTemp(0)
            v_oDataColumn.DataType = GetType(System.String)
            mv_DataSet.Tables("F_" & v_strRptID).Columns.Add(v_oDataColumn)
        Next
        'End If

        mv_DataSet.Tables("F_" & v_strRptID).Clear()

        v_oDataRow = mv_DataSet.Tables("F_" & v_strRptID).NewRow()
        v_oDataRow = mv_DataSet.Tables("F_" & v_strRptID).NewRow()
        Dim j As Integer = 0
        For i As Integer = 0 To v_arrField.Count - 2
            v_arrTemp = v_arrField(i).Split("|")
            If v_arrTemp(3) = "N" Then
                v_oDataRow(j) = IIf(Trim(v_arrTemp(1)) = "-1", Trim(v_arrTemp(2)), Replace(FormatNumber(Trim(v_arrTemp(1)), 0), ",", "."))
            Else
                v_oDataRow(j) = IIf(Trim(v_arrTemp(1)) = "-1", Trim(v_arrTemp(2)), Trim(v_arrTemp(1)))
            End If
            j += 1
            'If v_arrTemp(3) = "N" Then
            '    v_oDataRow(j) = FormatNumber(Trim(v_arrTemp(2)), 0)
            'Else
            If v_arrTemp(4).ToUpper = "MICODE" Or v_arrTemp(4).ToUpper = "SICODE" Or _
                v_arrTemp(4).ToUpper = "TLID" Or v_arrTemp(4).ToUpper = "TLID1" Or v_arrTemp(4).ToUpper = "TLID2" Or v_arrTemp(4).ToUpper = "TLID3" Then
                v_oDataRow(j) = Trim(Mid(Trim(v_arrTemp(2)), InStr(Trim(v_arrTemp(2)), "-") + 1))
            Else
                If v_arrTemp(3) = "N" Then
                    v_oDataRow(j) = FormatNumber(Trim(v_arrTemp(2)), 0)
                Else
                    v_oDataRow(j) = Trim(v_arrTemp(2))
                End If
            End If
            'End If
            j += 1
        Next
        mv_DataSet.Tables("F_" & v_strRptID).Rows.Add(v_oDataRow)

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
    'Chỉ định giá trị cho một control
    Public Sub AssignCtlVal(ByVal pv_strCtlVal As String, ByVal pv_strFiledName As String)
        Try
            For i As Integer = 0 To mv_arrObjFields.Length - 2
                If mv_arrObjFields(i).ColumnName = pv_strFiledName Then
                    If mv_arrObjFields(i).ControlType = "C" Then
                        Dim v_arrItem() As String = pv_strCtlVal.Split("$")
                        Dim v_dt As New DataTable
                        Dim v_row As DataRow

                        v_dt.Columns.Add(New DataColumn("VALUE", System.Type.GetType("System.String")))
                        v_dt.Columns.Add(New DataColumn("DISPLAY", System.Type.GetType("System.String")))

                        For j As Integer = 0 To v_arrItem.Count - 1
                            Dim v_strTemp As String = v_arrItem(j)
                            If v_strTemp <> "" Then
                                v_row = v_dt.NewRow()
                                v_row("VALUE") = v_strTemp.Split("|")(0)
                                v_row("DISPLAY") = v_strTemp.Split("|")(0) & " - " & v_strTemp.Split("|")(1)
                                v_dt.Rows.Add(v_row)
                            End If
                        Next

                        CType(MyBase.pnTransDetail.Controls(mv_arrObjFields(i).ControlIndex), ComboBoxEx).Clears()
                        CType(MyBase.pnTransDetail.Controls(mv_arrObjFields(i).ControlIndex), ComboBoxEx).BeginUpdate()
                        CType(MyBase.pnTransDetail.Controls(mv_arrObjFields(i).ControlIndex), ComboBoxEx).DataSource = v_dt
                        CType(MyBase.pnTransDetail.Controls(mv_arrObjFields(i).ControlIndex), ComboBoxEx).DisplayMember = "DISPLAY"
                        CType(MyBase.pnTransDetail.Controls(mv_arrObjFields(i).ControlIndex), ComboBoxEx).ValueMember = "VALUE"
                        CType(MyBase.pnTransDetail.Controls(mv_arrObjFields(i).ControlIndex), ComboBoxEx).EndUpdate()

                        CType(MyBase.pnTransDetail.Controls(mv_arrObjFields(i).ControlIndex), ComboBoxEx).AddItems(mv_ResourceManager.GetString("ALL"), "-1")
                    Else
                        If mv_arrObjFields(i).SearchCode <> "" Then
                            MyBase.mv_lstIsTextChanged(i) = True And Not MyBase.mv_blnIsF5(i)
                            MyBase.pnTransDetail.Controls(mv_arrObjFields(i).ControlIndex).Text = pv_strCtlVal.Split("|")(0)
                            MyBase.mskData_Validating(MyBase.pnTransDetail.Controls(mv_arrObjFields(i).ControlIndex))
                        End If
                    End If
                End If
            Next
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
End Class