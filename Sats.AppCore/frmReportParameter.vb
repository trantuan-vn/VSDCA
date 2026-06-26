Imports System.Windows.Forms
Imports Sats.CommonLibrary
Imports System.Data
Imports System.Data.SqlServerCe

Public Class frmReportParameter

#Region " Declare constant and variables "
    Const c_ResourceManager = "Sats.AppCore.frmReportParameter_"
    Private mv_xmlDocumentInquiryData As New Xml.XmlDocument
    Private mv_strModuleCode As String
    Private mv_strLanguage As String
    Private mv_strLocalObject As String
    Private mv_strBranchId As String
    Private mv_strTellerId As String
    Private mv_strBRCODE As String
    Private mv_strTellerName As String
    Private mv_strTellerType As String
    Private mv_strIpAddress As String
    Private mv_strWsName As String
    Private mv_strBusDate As String = String.Empty
    Private mv_strObjectName As String
    Private mv_strTxBusDate As String
    Private mv_strCustName As String
    Private mv_bolCheck As Boolean = True
    Private mv_Check As Boolean = True
    Private mv_ResourceManager As Resources.ResourceManager
    Private mv_strMemberFilter As String
    Private mv_strStockFilter As String
    Private mv_lngAllMember As Long
    Private mv_lngAllStock As Long


    Private mv_arrObjFields() As CFieldMaster
    Private mv_arrObjCoFields() As CFieldMaster
    Private mv_arrObjFldVals() As CFieldVal
    Private mv_arrObjCoFldVals() As CFieldVal

    'Private mv_strMSGAMT As String
    'Private mv_strMICODE As String
    'Private mv_strCOMICODE As String

    'Private mv_strSICODE As String
    'Private mv_strBACKDATE As String
    'Private mv_strCHKID As String
    'Private mv_strOFFID As String
    'Private mv_strCFRID As String
    'Private mv_strCOTLTXCD As String = ""
    'Private mv_strCHILDTLTXCD As String = ""
    'Private mv_strFileName As String = ""
    'Private mv_strFileName1 As String = ""
    'Private mv_strRange As String = ""
    'Private mv_strTXNUM As String = ""
    'Private mv_strTXDATE As String = ""
    'Private mv_strISPARENT As String = ""
    'Private mv_intISBRID As Integer = 1

    Private mv_ds As New DataSet

    Const CONTROL_TOP = 10
    Const CONTROL_LEFT = 10
    Const CONTROL_GAP = 2
    Const CONTROL_HEIGHT = 23
    Const ALL_WIDTH = 700
    Const WIDTH_PERCHAR = 7
    Const WIDTH_LABLE = 100
    Const PANEL_TOP = 54
    Const PANEL_HEIGHT = 100

    Const PREFIXED_MSKDATA = "mskData"

    Private mv_isAutoClosedWhenOK As Boolean = False

    Public mv_frmSearchScreen As frmSearch

    Private mv_BDSDelivery As BDSChannel.BDSDelivery
    Private mv_viewsql As String = ""
    Private mv_fail_index As Long
    ' quick search 
    Dim mv_lngListIndex As Long
    Dim mv_intChar As Integer

    'Hanm5 them 2 bien truyen len form search
    Private mv_strSearchSICODE As String = String.Empty
    Private mv_strSearchMICODE As String = String.Empty
    'Hanm5 end-----------------------------------

    'Private v_frmProcess As frmProcess
    'Private Delegate Function DelageteForm()
    'Private mv_Dela As DelageteForm
    'Private bgw As New BackgroundWorker

    'Private v_thread As Threading.Thread
    Private mv_intErrCount As Integer
    Private mv_DataSet As DataSet
    Private mv_strAuth As String
    Private mv_strPassDate As String
    Private v_oProcess As ProcessForm

#End Region
#Region " Properties "

    Public Property PassDate() As String
        Get
            Return mv_strPassDate
        End Get
        Set(ByVal value As String)
            mv_strPassDate = value
        End Set
    End Property

    Public Property ViewSQL() As String
        Get
            Return mv_viewsql
        End Get
        Set(ByVal Value As String)
            mv_viewsql = Value
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

    Public Property ModuleCode() As String
        Get
            Return mv_strModuleCode
        End Get
        Set(ByVal Value As String)
            mv_strModuleCode = Value
        End Set
    End Property

    Public Property ObjectName() As String
        Get
            Return mv_strObjectName
        End Get
        Set(ByVal Value As String)
            mv_strObjectName = Value
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
    Public Property BRCODE() As String
        Get
            Return mv_strBRCODE
        End Get
        Set(ByVal Value As String)
            mv_strBRCODE = Value
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
    Public Property TellerName() As String
        Get
            Return mv_strTellerName
        End Get
        Set(ByVal Value As String)
            mv_strTellerName = Value
        End Set
    End Property
    Public Property LocalObject() As String
        Get
            Return mv_strLocalObject
        End Get
        Set(ByVal Value As String)
            mv_strLocalObject = Value
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
    Public Property UserLanguage() As String
        Get
            Return mv_strLanguage
        End Get
        Set(ByVal Value As String)
            mv_strLanguage = Value
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

    Public Property Auth() As String
        Get
            Return mv_strAuth
        End Get
        Set(ByVal value As String)
            mv_strAuth = value
        End Set
    End Property
#End Region

#Region "Khoi tao Form giao dich"
    Protected Overridable Sub OnInit()

        mv_ResourceManager = New Resources.ResourceManager(c_ResourceManager & UserLanguage, System.Reflection.Assembly.GetExecutingAssembly())
        mskTransCode.Text = mv_strObjectName

        LoadResource(Me)

        If Me.ObjectName.Length > 0 Then
            LoadScreen(Me.ObjectName.ToString)
        End If

        If mv_viewsql <> "" Then
            LoadView(mv_viewsql)
        End If

        FormResize()

    End Sub

    Public Sub LoadScreen(ByVal strRptID As String, Optional ByVal v_strXML As String = vbNullString)

        Dim v_nodeList As Xml.XmlNodeList
        Dim v_strValue, v_strFLDNAME As String
        Dim i, j As Integer
        Dim v_objField As CFieldMaster, v_objFieldVal As CFieldVal
        Dim v_strFieldName As String = "", v_strDefName As String = "", v_strCaption As String = ""
        Dim v_strFldType As String = "", v_strFldMask As String = "", v_strFldFormat As String = ""
        Dim v_strLList As String = "", v_strLChk As String = "", v_strDefVal As String = ""
        Dim v_strAmtExp As String = "", v_strValidTag As String = "", v_strLookUp As String = ""
        Dim v_strDataType As String = "", v_strControlType As String = "", v_strChainName As String = ""
        Dim v_strLookupName As String = "", v_strPrintInfo As String = "", v_strInvName As String = ""
        Dim v_strInvFormat As String = "", v_strFldSource As String = "", v_strFldDesc As String = "", v_strRiskfld As String = ""
        Dim v_strSearchCode As String = "", v_strSrModCode As String = "", v_strMemberField As String = "", v_strStockField As String = "", v_strIsDuplicated As String = ""
        Dim v_intOdrNum, v_intFldLen, v_intIndex As Integer
        Dim v_blnVisible, v_blnEnabled, v_blnMandatory As Boolean

        Try
            'Create message to inquiry object fields
            Dim v_strClause, v_strObjMsg As String
            Dim v_xmlDocument As New Xml.XmlDocument, v_xmlDocumentData As New Xml.XmlDocument
            'Dim v_ws As New BDSDelivery.BDSDelivery
            If Len(v_strXML) > 0 Then
                v_xmlDocumentData.LoadXml(v_strXML)
            End If

            'Lay thong tin bao cao
            v_strClause = "SELECT * FROM RPREPORTS WHERE RPTID='" & strRptID & "'"
            'End If
            v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLTX, gc_ActionInquiry, v_strClause)

            Dim v_lngError As Long = mv_BDSDelivery.Message(v_strObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Exit Sub
            End If

            v_xmlDocument.LoadXml(v_strObjMsg)
            v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")
            If v_nodeList.Count = 0 Then
                'Neu khong tim thay ma giao dich
                MessageBox.Show("Không tìm thấy báo cáo này!", gc_ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Information)
                mv_bolCheck = False
                'ResetScreen()
                Exit Sub
            End If
            For i = 0 To v_nodeList.Count - 1
                For j = 0 To v_nodeList.Item(i).ChildNodes.Count - 1
                    With v_nodeList.Item(i).ChildNodes(j)
                        v_strValue = .InnerText.ToString
                        v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                        Select Case Trim(v_strFLDNAME)
                            Case "RPTTITLE"
                                If UserLanguage <> "EN" Then
                                    lblTransCaption.Text = Trim(v_strValue)
                                    Me.Text = Trim(v_strValue)
                                End If
                            Case "EN_RPTTITLE"
                                If UserLanguage = "EN" Then
                                    lblTransCaption.Text = Trim(v_strValue)
                                    Me.Text = Trim(v_strValue)
                                End If
                        End Select
                    End With
                Next
            Next

            'Lay thong tin chi tiet cac truong cua giao dich
            v_strClause = "upper(OBJNAME) = '" & strRptID & "' and deleted =0 and status =0 ORDER BY ODRNUM"
            v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_FLDMASTER, gc_ActionInquiry, , v_strClause)
            v_lngError = mv_BDSDelivery.Message(v_strObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Exit Sub
            End If

            v_xmlDocument.LoadXml(v_strObjMsg)
            v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")

            ReDim mv_arrObjFields(v_nodeList.Count)

            For i = 0 To v_nodeList.Count - 1
                For j = 0 To v_nodeList.Item(i).ChildNodes.Count - 1
                    With v_nodeList.Item(i).ChildNodes(j)
                        v_strValue = .InnerText.ToString
                        v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)

                        Select Case Trim(v_strFLDNAME)
                            Case "FLDNAME"
                                v_strFieldName = Trim(v_strValue)
                            Case "DEFNAME"
                                v_strDefName = Trim(v_strValue)
                            Case "CAPTION"
                                If UserLanguage <> "EN" Then
                                    v_strCaption = Trim(v_strValue)
                                End If
                            Case "EN_CAPTION"
                                If UserLanguage = "EN" Then
                                    v_strCaption = Trim(v_strValue)
                                End If
                            Case "ODRNUM"
                                v_intOdrNum = CInt(Trim(v_strValue))
                            Case "FLDTYPE"
                                v_strFldType = Trim(v_strValue)
                            Case "FLDMASK"
                                v_strFldMask = Trim(v_strValue)
                            Case "FLDFORMAT"
                                v_strFldFormat = Trim(v_strValue)
                            Case "FLDLEN"
                                v_intFldLen = CInt(Trim(v_strValue))
                            Case "LLIST"
                                v_strLList = Trim(v_strValue)
                                'Xá»­ lÃ½ cÃ¡c biáº¿n há»‡ thá»‘ng
                                v_strLList = v_strLList.Replace("<$BRID>", Me.BranchId)
                                v_strLList = v_strLList.Replace("<$TLID>", Me.TellerId)
                                v_strLList = v_strLList.Replace("<$BUSDATE>", Me.BusDate)
                            Case "LCHK"
                                v_strLChk = Trim(v_strValue)
                            Case "DEFVAL"
                                v_strDefVal = Trim(v_strValue)
                            Case "VISIBLE"
                                v_blnVisible = (Trim(v_strValue) = "Y")
                            Case "DISABLE"
                                v_blnEnabled = (Trim(v_strValue) = "N")
                            Case "MANDATORY"
                                v_blnMandatory = (Trim(v_strValue) = "Y")
                            Case "AMTEXP"
                                v_strAmtExp = Trim(v_strValue)
                            Case "VALIDTAG"
                                v_strValidTag = Trim(v_strValue)
                            Case "LOOKUP"
                                v_strLookUp = Trim(v_strValue)
                            Case "DATATYPE"
                                v_strDataType = Trim(v_strValue)
                            Case "CTLTYPE"
                                v_strControlType = Trim(v_strValue)
                            Case "INVNAME"
                                v_strInvName = Trim(v_strValue)
                            Case "INVFORMAT"
                                v_strInvFormat = Trim(v_strValue)
                            Case "FLDSOURCE"
                                v_strFldSource = Trim(v_strValue)
                            Case "FLDDESC"
                                v_strFldDesc = Trim(v_strValue)
                            Case "CHAINNAME"
                                v_strChainName = Trim(v_strValue)
                            Case "LOOKUPNAME"
                                v_strLookupName = Trim(v_strValue)
                            Case "SEARCHCODE"
                                v_strSearchCode = Trim(v_strValue)
                                'Case "SRMODCODE"
                                ' v_strSrModCode = Trim(v_strValue)
                            Case "MODCODE"
                                v_strSrModCode = Trim(v_strValue)
                            Case "PRINTINFO"
                                v_strPrintInfo = v_strValue 'KhÃ´ng Ä‘Æ°á»£c trim vÃ¬ Ä‘á»™ dÃ i báº¯t buá»™c 10 kÃ½ tá»±
                            Case "MEMBERFIELD"
                                v_strMemberField = Trim(v_strValue)
                            Case "STOCKFIELD"
                                v_strStockField = Trim(v_strValue)
                            Case "RISKFLD"
                                v_strRiskfld = ("Y" = Trim(v_strValue).ToUpper)
                        End Select
                    End With
                Next

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
                    .RiskField = v_strRiskfld
                End With
                mv_arrObjFields(i) = v_objField
            Next
            ReDim Preserve mv_arrObjFields(v_nodeList.Count)

            'Lay cac quy tac kiem tra cua cac truong giao dich
            v_strClause = "SELECT FLDNAME, VALTYPE, OPERATOR, VALEXP, VALEXP2, ERRMSG, EN_ERRMSG FROM FLDVAL " & _
                "WHERE upper(OBJNAME) = '" & strRptID & "' AND DELETED=0 AND STATUS=0 ORDER BY VALTYPE, FLDNAME, ODRNUM" 'Thá»© tá»± order by lÃ  quan trá»?ng khÃ´ng sá»­a

            v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_FLDVAL, gc_ActionInquiry, v_strClause)

            v_lngError = mv_BDSDelivery.Message(v_strObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Exit Sub
            End If

            v_xmlDocument.LoadXml(v_strObjMsg)
            v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")

            ReDim mv_arrObjFldVals(v_nodeList.Count)
            Dim v_strFieldVal_FldName As String = "", v_strFieldVal_ValType As String = "", v_strFieldVal_Operator As String = ""
            Dim v_strFieldVal_ValExp As String = "", v_strFieldVal_ValExp2 As String = "", v_strFieldVal_ErrMsg As String = "", v_strFieldVal_EnErrMsg As String = ""

            For i = 0 To v_nodeList.Count - 1
                For j = 0 To v_nodeList.Item(i).ChildNodes.Count - 1
                    With v_nodeList.Item(i).ChildNodes(j)
                        'Ghi nhan thuat toan kiem tra va tinh toan cho tung truong cua giao dich
                        v_strValue = .InnerText.ToString
                        v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                        Select Case Trim(v_strFLDNAME)
                            Case "FLDNAME"
                                v_strFieldVal_FldName = Trim(v_strValue)
                            Case "VALTYPE"
                                v_strFieldVal_ValType = Trim(v_strValue)
                            Case "OPERATOR"
                                v_strFieldVal_Operator = Trim(v_strValue)
                            Case "VALEXP"
                                v_strFieldVal_ValExp = Trim(v_strValue)
                            Case "VALEXP2"
                                v_strFieldVal_ValExp2 = Trim(v_strValue)
                            Case "ERRMSG"
                                v_strFieldVal_ErrMsg = Trim(v_strValue)
                            Case "EN_ERRMSG"
                                v_strFieldVal_EnErrMsg = Trim(v_strValue)
                        End Select
                    End With
                Next

                'Xac dinh index cua mang FldMaster
                For j = 0 To mv_arrObjFields.GetLength(0) - 1 Step 1
                    If Not mv_arrObjFields(j) Is Nothing Then
                        If Trim(mv_arrObjFields(j).FieldName) = Trim(v_strFieldVal_FldName) Then
                            v_intIndex = j
                        End If
                    End If
                Next

                'Dieu kien xu ly
                v_objFieldVal = New CFieldVal
                With v_objFieldVal
                    .OBJNAME = strRptID
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
            ReDim Preserve mv_arrObjFldVals(v_nodeList.Count)
            'Hien thi thong tin giao dich len man hinh
            DisplayScreen()

        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                                     & "Error code: System error!" & vbNewLine _
                                                     & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Sub


    Private Sub DisplayScreen()
        Dim v_intIndex, v_intCount, v_intPosition, v_intTop, v_intLeft, v_intWidth, v_intLastTop As Integer
        Dim v_lblCaption, v_lblDesc As Label
        Dim v_mskData As FlexMaskEditBox = Nothing
        Dim v_txtData As TextBox = Nothing
        Dim v_cboData As ComboBox = Nothing

        'Dim v_ws As New BDSDelivery.BDSDelivery
        Dim v_strCmdSQL As String, v_strObjMsg As String

        Try
            'Xoa man hinh cu
            Me.pnTransDetail.Controls.Clear()
            'Tao man hinh moi
            v_intCount = mv_arrObjFields.GetLength(0)
            If v_intCount > 0 Then
                Me.pnTransDetail.Visible = True
                Me.pnTransDetail.Top = PANEL_TOP

                Dim v_intLabelWidth As Integer = 0

                For v_intIndex = 0 To v_intCount - 1 Step 1
                    If Not mv_arrObjFields(v_intIndex) Is Nothing Then
                        If v_intLabelWidth < mv_arrObjFields(v_intIndex).Caption.Length Then
                            v_intLabelWidth = mv_arrObjFields(v_intIndex).Caption.Length
                        End If
                    End If
                Next
                v_intLabelWidth = v_intLabelWidth * WIDTH_PERCHAR - v_intLabelWidth

                'Hiện thị nội dung các trường trong giao dịch
                For v_intIndex = 0 To v_intCount - 1 Step 1
                    If Not mv_arrObjFields(v_intIndex) Is Nothing Then
                        v_lblCaption = New Label
                        v_lblDesc = New Label
                        v_lblCaption.Visible = mv_arrObjFields(v_intIndex).Visible
                        v_lblDesc.Visible = False

                        v_intTop = CONTROL_TOP + v_intPosition * (CONTROL_HEIGHT + CONTROL_GAP)
                        v_lblCaption.Top = v_intTop
                        v_lblCaption.Left = CONTROL_LEFT
                        If mv_arrObjFields(v_intIndex).Mandatory Then
                            v_lblCaption.ForeColor = System.Drawing.Color.Red
                        Else
                            v_lblCaption.ForeColor = System.Drawing.Color.Blue
                        End If
                        v_lblCaption.Tag = mv_arrObjFields(v_intIndex).ValidTag
                        v_lblCaption.Text = mv_arrObjFields(v_intIndex).Caption
                        v_lblCaption.RightToLeft = Windows.Forms.RightToLeft.Yes
                        v_lblCaption.Name = Trim(mv_arrObjFields(v_intIndex).FieldName)
                        v_lblCaption.Width = v_intLabelWidth

                        Select Case Trim(mv_arrObjFields(v_intIndex).ControlType)
                            Case "T" 'TextBox
                                v_txtData = New TextBox
                                v_txtData.Visible = mv_arrObjFields(v_intIndex).Visible
                                v_txtData.Top = v_intTop
                                v_intLeft = CONTROL_LEFT + CONTROL_GAP + v_intLabelWidth
                                v_txtData.Left = v_intLeft
                                v_intWidth = mv_arrObjFields(v_intIndex).FieldLength * WIDTH_PERCHAR
                                If ALL_WIDTH < v_intLeft + v_intWidth Then
                                    v_intWidth = ALL_WIDTH - v_intLeft
                                End If
                                v_txtData.MaxLength = mv_arrObjFields(v_intIndex).FieldLength
                                v_txtData.Width = v_intWidth
                                v_txtData.Tag = v_intIndex  'Lưu lại chỉ số của mảng để lấy các thông tin tương ứng đến trường
                                v_txtData.Enabled = mv_arrObjFields(v_intIndex).Enabled
                                v_txtData.Name = PREFIXED_MSKDATA & Trim(mv_arrObjFields(v_intIndex).FieldName)
                                If mv_arrObjFields(v_intIndex).LookUp = "Y" Then
                                    If Len(mv_arrObjFields(v_intIndex).LookupList) > 0 Then
                                        v_txtData.BackColor = System.Drawing.Color.Khaki
                                    Else
                                        v_txtData.BackColor = System.Drawing.Color.Lavender
                                    End If
                                End If
                                If Len(Trim(mv_arrObjFields(v_intIndex).FieldValue)) > 0 Then
                                    v_txtData.Text = Trim(mv_arrObjFields(v_intIndex).FieldValue)
                                Else
                                    If Len(Trim(mv_arrObjFields(v_intIndex).DefaultValue)) > 0 Then
                                        If mv_arrObjFields(v_intIndex).DataType = "N" Then
                                            v_txtData.Text = Trim(mv_arrObjFields(v_intIndex).DefaultValue)
                                        Else
                                            v_txtData.Text = Trim(mv_arrObjFields(v_intIndex).DefaultValue)
                                        End If
                                    Else
                                        If Trim(mv_arrObjFields(v_intIndex).DataType) = "N" Then
                                            v_txtData.Text = "0"
                                        ElseIf Trim(mv_arrObjFields(v_intIndex).DataType) = "D" Then
                                            v_txtData.Text = Me.BusDate
                                        End If
                                    End If
                                End If
                                If Trim(mv_arrObjFields(v_intIndex).DataType) = "N" Then
                                    FormatNumericTextbox(CType(v_txtData, TextBox))
                                End If
                                AddHandler v_txtData.GotFocus, AddressOf mskData_GotFocus
                                AddHandler v_txtData.Validating, AddressOf mskData_Validating
                                v_lblDesc.Top = v_intTop
                                v_intLeft = v_txtData.Left + v_txtData.Width + CONTROL_GAP
                                If v_intLeft >= ALL_WIDTH Then
                                    v_intWidth = 0
                                Else
                                    v_intWidth = ALL_WIDTH - v_intLeft
                                End If
                            Case "M" 'FlexMaskedEdit
                                v_mskData = New FlexMaskEditBox
                                v_mskData.Visible = mv_arrObjFields(v_intIndex).Visible
                                v_mskData.Top = v_intTop
                                v_intLeft = CONTROL_LEFT + CONTROL_GAP + v_intLabelWidth
                                v_mskData.Left = v_intLeft
                                v_intWidth = mv_arrObjFields(v_intIndex).FieldLength * WIDTH_PERCHAR
                                If ALL_WIDTH < v_intLeft + v_intWidth Then
                                    v_intWidth = ALL_WIDTH - v_intLeft
                                End If
                                v_mskData.Width = v_intWidth
                                v_mskData.Tag = v_intIndex
                                v_mskData.PromptChar = "_"
                                'v_mskData.SetFormatString = "C"

                                If Trim(mv_arrObjFields(v_intIndex).DataType) = "N" Then
                                    v_mskData.MaskCharInclude = False
                                    v_mskData.FieldType = FlexMaskEditBox._FieldType.NUMERIC
                                ElseIf Trim(mv_arrObjFields(v_intIndex).DataType) = "C" Then
                                    v_mskData.MaskCharInclude = False
                                    v_mskData.FieldType = FlexMaskEditBox._FieldType.ALFA
                                Else
                                    v_mskData.MaskCharInclude = True
                                    v_mskData.FieldType = FlexMaskEditBox._FieldType.DATE_
                                End If
                                v_mskData.Mask = mv_arrObjFields(v_intIndex).InputMask
                                v_mskData.MaxLength = mv_arrObjFields(v_intIndex).FieldLength
                                v_mskData.Enabled = mv_arrObjFields(v_intIndex).Enabled
                                v_mskData.Name = PREFIXED_MSKDATA & Trim(mv_arrObjFields(v_intIndex).FieldName)
                                If Len(mv_arrObjFields(v_intIndex).SearchCode) > 0 Then
                                    v_mskData.BackColor = System.Drawing.Color.GreenYellow
                                ElseIf Len(mv_arrObjFields(v_intIndex).LookupList) > 0 Then
                                    v_mskData.BackColor = System.Drawing.Color.Khaki
                                End If

                                If Len(Trim(mv_arrObjFields(v_intIndex).FieldValue)) > 0 Then
                                    v_mskData.Text = Trim(mv_arrObjFields(v_intIndex).FieldValue)
                                Else
                                    If Len(Trim(mv_arrObjFields(v_intIndex).DefaultValue)) > 0 Then
                                        v_mskData.Text = Trim(mv_arrObjFields(v_intIndex).DefaultValue)
                                    Else
                                        If Trim(mv_arrObjFields(v_intIndex).DataType) = "N" Then
                                            v_mskData.Text = "0"
                                        ElseIf Trim(mv_arrObjFields(v_intIndex).DataType) = "D" Then
                                            v_mskData.Text = Me.BusDate
                                        End If
                                    End If
                                End If
                                AddHandler v_mskData.GotFocus, AddressOf mskData_GotFocus
                                AddHandler v_mskData.Validating, AddressOf mskData_Validating

                                v_lblDesc.Top = v_intTop
                                v_intLeft = v_mskData.Left + v_mskData.Width + CONTROL_GAP
                                If v_intLeft >= ALL_WIDTH Then
                                    v_intWidth = 0
                                Else
                                    v_intWidth = ALL_WIDTH - v_intLeft
                                End If
                            Case "C" 'ComboBox
                                v_cboData = New ComboBoxEx
                                v_cboData.Visible = mv_arrObjFields(v_intIndex).Visible
                                v_cboData.Top = v_intTop
                                v_intLeft = CONTROL_LEFT + CONTROL_GAP + v_intLabelWidth
                                v_cboData.Left = v_intLeft
                                v_intWidth = mv_arrObjFields(v_intIndex).FieldLength * WIDTH_PERCHAR
                                If ALL_WIDTH < v_intLeft + v_intWidth Then
                                    v_intWidth = ALL_WIDTH - v_intLeft
                                End If
                                v_cboData.Width = v_intWidth
                                v_cboData.Tag = v_intIndex
                                v_cboData.Enabled = mv_arrObjFields(v_intIndex).Enabled
                                v_cboData.Name = PREFIXED_MSKDATA & Trim(mv_arrObjFields(v_intIndex).FieldName)
                                v_cboData.DropDownStyle = ComboBoxStyle.DropDown
                                AddHandler v_cboData.SelectedIndexChanged, AddressOf cboData_SelectedIndexChanged
                                AddHandler v_cboData.Validating, AddressOf v_cboData_Validating

                                v_lblDesc.Top = v_intTop
                                v_intLeft = v_cboData.Left + v_cboData.Width + CONTROL_GAP
                                If v_intLeft >= ALL_WIDTH Then
                                    v_intWidth = 0
                                Else
                                    v_intWidth = ALL_WIDTH - v_intLeft
                                End If
                                'Lấy dữ liệu cho ComboBox
                                v_strCmdSQL = mv_arrObjFields(v_intIndex).LookupList
                                If Not (InStr(v_strCmdSQL, "#") > 0) Then
                                    If mv_arrObjFields(v_intIndex).MemberField <> "" And mv_lngAllMember = 0 Then
                                        v_strCmdSQL = "select * from (" & v_strCmdSQL & ") where " & mv_arrObjFields(v_intIndex).MemberField & " in " & mv_strMemberFilter
                                    End If
                                    If mv_arrObjFields(v_intIndex).StockField <> "" And mv_lngAllStock = 0 Then
                                        v_strCmdSQL = "select * from (" & v_strCmdSQL & ") where " & mv_arrObjFields(v_intIndex).StockField & " in " & mv_strStockFilter
                                    End If
                                    'Dim v_strSQL As String
                                    'If mv_arrObjFields(v_intIndex).RiskField Then
                                    '    v_strSQL = "SELECT VALUE VALUE, DISPLAY DISPLAY FROM(SELECT '-1' VALUE, N'" & mv_ResourceManager.GetString("ALL") & "' DISPLAY FROM DUAL" _
                                    '                & " UNION" _
                                    '                & " SELECT VALUE, DISPLAY FROM (" & v_strCmdSQL & ")) ORDER BY 1"

                                    'Else
                                    '    v_strSQL = v_strCmdSQL
                                    'End If

                                    v_strObjMsg = BuildXMLObjMsg(, BranchId, , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_LOOKUP, gc_ActionInquiry, v_strCmdSQL)

                                    mv_BDSDelivery.Message(v_strObjMsg)

                                    FillComboEx(v_strObjMsg, v_cboData)
                                    If mv_arrObjFields(v_intIndex).RiskField Then
                                        CType(v_cboData, ComboBoxEx).AddItems(mv_ResourceManager.GetString("ALL"), "-1")
                                    End If

                                End If
                        End Select
                        v_lblDesc.Tag = mv_arrObjFields(v_intIndex).LookupCheck
                        v_lblDesc.Left = v_intLeft
                        v_lblDesc.Width = v_intWidth
                        v_lblDesc.Text = ""
                        v_lblDesc.Name = Trim(mv_arrObjFields(v_intIndex).FieldName)

                        Me.pnTransDetail.Controls.Add(v_lblCaption)
                        Me.pnTransDetail.Controls.Add(v_lblDesc)
                        mv_arrObjFields(v_intIndex).LabelIndex = Me.pnTransDetail.Controls.IndexOf(v_lblDesc)
                        Select Case Trim(mv_arrObjFields(v_intIndex).ControlType)
                            Case "T"
                                Me.pnTransDetail.Controls.Add(v_txtData)
                                mv_arrObjFields(v_intIndex).ControlIndex = Me.pnTransDetail.Controls.IndexOf(v_txtData)
                            Case "M"
                                Me.pnTransDetail.Controls.Add(v_mskData)
                                mv_arrObjFields(v_intIndex).ControlIndex = Me.pnTransDetail.Controls.IndexOf(v_mskData)
                            Case "C"
                                Me.pnTransDetail.Controls.Add(v_cboData)
                                mv_arrObjFields(v_intIndex).ControlIndex = Me.pnTransDetail.Controls.IndexOf(v_cboData)
                                'Đặt giá trị mặc định cho combobox
                                If Len(Trim(mv_arrObjFields(v_intIndex).FieldValue)) > 0 Then
                                    CType(Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).ControlIndex), ComboBoxEx).SelectedValue = Trim(mv_arrObjFields(v_intIndex).FieldValue)
                                    'Else
                                    '   CType(Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).ControlIndex), ComboBoxEx).SelectedValue = Trim(mv_arrObjFields(v_intIndex).DefaultValue)
                                End If
                        End Select

                        'Tính toán vị trí hiển thị nếu control là  visible
                        If mv_arrObjFields(v_intIndex).Visible Then
                            v_intPosition = v_intPosition + 1
                            v_intLastTop = v_intTop
                        End If

                    End If
                Next

                Me.pnTransDetail.Height = v_intLastTop + CONTROL_HEIGHT + 8
                Me.btnCancel.Top = Me.pnTransDetail.Height + Me.Panel1.Height + 8
                Me.btnOK.Top = Me.btnCancel.Top
                Me.pnTransDetail.Controls(mv_arrObjFields(0).ControlIndex).Select()

            Else
                Me.pnTransDetail.Visible = False
            End If

        Catch ex As Exception
            'LogError.Write(ex.Message & vbNewLine & ex.StackTrace, EventLogEntryType.Error)
            'MsgBox(ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
            'mv_bolCheck = False
            Throw ex
        End Try
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
            ElseIf TypeOf (v_ctrl) Is Panel Then
                LoadResource(v_ctrl)
            End If
        Next

    End Sub
#End Region

#Region "Cac ham, thu tuc khac"
    Private Sub FormResize()
        Me.pnTransDetail.Width = Me.Width
        Me.btnOK.Top = Me.btnCancel.Top

        Me.btnOK.Left = 8
        Me.btnCancel.Left = Me.btnOK.Left + Me.btnCancel.Width + 10
        Me.Height = btnCancel.Top + btnCancel.Height + CONTROL_TOP * 4
    End Sub

    Private Sub FormatNumericTextbox(ByVal pv_ctrl As TextBox)
        Try
            Dim v_strFormat As String
            Dim v_intDecimal As String
            Dim v_intIndex As Integer
            v_intIndex = CType(pv_ctrl, TextBox).Tag
            v_strFormat = mv_arrObjFields(v_intIndex).FieldFormat
            If (v_strFormat.Length > 0) Then
                If (v_strFormat.IndexOf(".") <> -1) Then
                    v_intDecimal = Mid(v_strFormat, v_strFormat.IndexOf(".") + 2).Length()
                Else
                    v_intDecimal = 0
                End If
            Else
                v_intDecimal = 0
            End If

            If IsNumeric(pv_ctrl.Text) Then
                If FormatNumber(pv_ctrl.Text, v_intDecimal) = Math.Round(CDbl(pv_ctrl.Text)) Then
                    pv_ctrl.Text = FormatNumber(Math.Floor(CDbl(pv_ctrl.Text)), v_intDecimal)
                Else
                    pv_ctrl.Text = FormatNumber(pv_ctrl.Text, v_intDecimal)
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub FillLookupData(ByVal v_strFLDNAME As String, ByVal v_strVALUE As String, ByVal v_strFULLDATA As String, Optional ByVal v_strFieldKey As String = "VALUE")
        Dim v_xmlDocument As New Xml.XmlDocument, v_nodeList As Xml.XmlNodeList, ctl As Control
        Dim v_strLookupName As String, i, j, v_intNodeIndex, v_intCount As Integer
        Dim v_strLookupValue As String
        v_xmlDocument.LoadXml(v_strFULLDATA)
        v_intCount = mv_arrObjFields.GetLength(0)
        If v_intCount > 0 Then
            'Xác định Node chứa dữ liệu
            v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")
            For i = 0 To v_nodeList.Count - 1
                For j = 0 To v_nodeList.Item(i).ChildNodes.Count - 1
                    With v_nodeList.Item(i).ChildNodes(j)
                        'If "VALUE" = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value) _
                        If v_strFieldKey = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value) _
                            And v_strVALUE = Trim(.InnerText.ToString) Then
                            v_intNodeIndex = i
                            Exit For
                        End If
                    End With
                Next
            Next
            'Nạp dữ liệu Lookup cho các control đã khai báo
            For i = 0 To v_intCount - 1 Step 1
                If Not mv_arrObjFields(i) Is Nothing Then
                    If Trim(mv_arrObjFields(i).LookupName).Length > 0 Then
                        'Náº¿u cÃ³ tham sá»‘ láº¥y giÃ¡ trá»‹
                        If Mid(Trim(mv_arrObjFields(i).LookupName), 1, 2) = v_strFLDNAME Then
                            'Vá»‹ trÃ­ tá»« thá»© 3 trá»Ÿ Ä‘i lÃ  tÃªn trÆ°á»?ng
                            v_strLookupName = Mid(Trim(mv_arrObjFields(i).LookupName), 3)
                            For j = 0 To v_nodeList.Item(v_intNodeIndex).ChildNodes.Count - 1
                                With v_nodeList.Item(v_intNodeIndex).ChildNodes(j)

                                    'Hanm5 them 4 dong
                                    If CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value) = "SICODE" Then
                                        mv_strSearchSICODE = Trim(.InnerText.ToString)
                                    ElseIf CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value) = "MICODE" Then
                                        mv_strSearchMICODE = Trim(.InnerText.ToString)
                                    End If
                                    'End Hanm5-----------------------------
                                    If v_strLookupName = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value) Then
                                        'GÃ¡n giÃ¡ trá»‹ cho contol tÆ°Æ¡ng á»©ng
                                        v_strLookupValue = Trim(.InnerText.ToString)
                                        ctl = Me.pnTransDetail.Controls(mv_arrObjFields(i).ControlIndex)
                                        If TypeOf (ctl) Is ComboBoxEx Then
                                            CType(ctl, ComboBoxEx).SelectedValue = v_strLookupValue
                                        Else
                                            ctl.Text = v_strLookupValue
                                        End If

                                        If mv_arrObjFields(i).DataType = "N" And Len(mv_arrObjFields(i).FieldFormat) > 0 Then
                                            FormatNumericTextbox(CType(ctl, TextBox))
                                        End If
                                    End If
                                End With
                            Next
                        End If
                    End If
                End If
            Next
        End If
        'v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")

    End Sub

#End Region

#Region "Các sự kiện"
    'Private Sub mskTransCode_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
    '    Select Case e.KeyCode
    '        Case Keys.Enter
    '            'ShowTransact()
    '        Case Keys.F5
    '            Dim frm As New Sats.AppCore.frmLookUp(mv_strLanguage)
    '            Dim v_intPos As Integer ', ctl As Control
    '            frm.SQLCMD = "SELECT TLTXCD VALUECD, TLTXCD VALUE, TXDESC DISPLAY, EN_TXDESC EN_DISPLAY, EN_TXDESC DESCRIPTION FROM TLTX ORDER BY VALUE"
    '            frm.ShowDialog()
    '            If Not frm.RETURNDATA Is Nothing Then
    '                v_intPos = InStr(frm.RETURNDATA, vbTab)
    '                If v_intPos > 0 Then
    '                    Me.mskTransCode.Text = Mid(frm.RETURNDATA, 1, v_intPos - 1)
    '                    Me.mskTransCode.Select()
    '                End If
    '                frm.Dispose()
    '            End If
    '    End Select
    'End Sub

    Private Sub frmTransaction_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        Dim v_intPos As Int16, ctl As Control, strFLDNAME As String, v_intIndex As Integer
        Dim strSQL, strSharp As String
        Dim v_intSharp, v_intCount As Integer
        Dim v_strSharp As String

        Select Case e.KeyCode
            Case Keys.Escape
                If mv_bolCheck Then
                    If MsgBox("Bạn có muốn thoát khỏi chức năng này không ?", MsgBoxStyle.Information + MsgBoxStyle.OkCancel, "Thông báo") = MsgBoxResult.Ok Then
                        OnClose()
                    End If
                Else
                    mv_bolCheck = True
                End If
            Case Keys.F5
                If Me.ActiveControl.Name = PREFIXED_MSKDATA & "BUSDATE" Then
                    Exit Sub
                End If
                If InStr(Me.ActiveControl.Name, PREFIXED_MSKDATA) > 0 Then
                    v_intIndex = Me.ActiveControl.Tag
                    'Tra cá»©u thÃ´ng tin
                    If Len(mv_arrObjFields(v_intIndex).SearchCode) > 0 Then
                        'Gá»?i hÃ m Ä‘á»ƒ thiáº¿t láº­p mÃ n hÃ¬nh tÃ¬m kiáº¿m
                        Dim v_strSearchCode As String = mv_arrObjFields(v_intIndex).SearchCode.ToUpper
                        Select Case v_strSearchCode
                            Case "CAFUNC"
                                Dim v_frm As New frmFunction
                                v_frm.UserLanguage = mv_strLanguage
                                v_frm.ShowDialog()
                                ActiveControl.Text = v_frm.ReturnValue
                            Case "FOREIGNNO"
                                Dim v_frm As New frmForeignNoReg
                                v_frm.UserLanguage = mv_strLanguage
                                Dim v_intIaType As Integer = 0
                                Dim v_strIaType As String = GetValueControlByName("mskData07")
                                If v_strIaType = "" Then
                                    v_intIaType = 0
                                Else
                                    v_intIaType = CInt(v_strIaType)
                                End If
                                v_frm.IaType = v_intIaType
                                v_frm.ShowDialog()
                                ActiveControl.Text = v_frm.ReturnValue

                            Case Else

                                SetLookUpDataForm()
                                mv_frmSearchScreen = New frmSearch(Me.mv_strLanguage)
                                mv_frmSearchScreen.TableName = mv_arrObjFields(v_intIndex).SearchCode
                                mv_frmSearchScreen.ModuleCode = mv_arrObjFields(v_intIndex).SrModCode
                                mv_frmSearchScreen.AuthCode = "NNNNYYNNNN" 'Chá»‰ cho phÃ©p thá»±c hiá»‡n Close vÃ  View. TÃ­nh nÄƒng nÃ y cáº§n nÃ¢ng cáº¥p Ä‘á»ƒ kiá»ƒm tra quyá»?n
                                mv_frmSearchScreen.AuthString = "NNNNYY" 'Chá»‰ cho phÃ©p thá»±c hiá»‡n Close vÃ  View. TÃ­nh nÄƒng nÃ y cáº§n nÃ¢ng cáº¥p Ä‘á»ƒ kiá»ƒm tra quyá»?n
                                mv_frmSearchScreen.IsLocalSearch = gc_IsLocalMsg
                                mv_frmSearchScreen.IsLookup = "Y"
                                mv_frmSearchScreen.SearchOnInit = False
                                mv_frmSearchScreen.BranchId = Me.BranchId
                                mv_frmSearchScreen.TellerId = Me.TellerId
                                'Hanm5 them 2 dong
                                mv_frmSearchScreen.SICODE = mv_strSearchSICODE
                                mv_frmSearchScreen.MICODE = mv_strSearchMICODE
                                '----------------
                                mv_frmSearchScreen.MemberFilter = mv_strMemberFilter
                                mv_frmSearchScreen.StockFilter = mv_strStockFilter
                                mv_frmSearchScreen.AllMember = mv_lngAllMember
                                mv_frmSearchScreen.AllStock = mv_lngAllStock
                                mv_frmSearchScreen.PassDate = PassDate
                                mv_frmSearchScreen.BusDate = BusDate

                                v_intCount = mv_arrObjFields.GetLength(0) - 1
                                strSharp = "" & v_intCount & Chr(1)
                                For v_intSharp = 0 To v_intCount - 1 Step 1
                                    v_strSharp = ""
                                    Select Case Trim(mv_arrObjFields(v_intSharp).ControlType)
                                        Case "T"
                                            v_strSharp = CType(Me.pnTransDetail.Controls(mv_arrObjFields(v_intSharp).ControlIndex), TextBox).Text
                                        Case "M"
                                            v_strSharp = CType(Me.pnTransDetail.Controls(mv_arrObjFields(v_intSharp).ControlIndex), FlexMaskEditBox).Text
                                            v_strSharp = IIf(Trim(mv_arrObjFields(v_intSharp).DataType) = "D", v_strSharp.Replace("/  /", ""), v_strSharp)
                                        Case "C"
                                            v_strSharp = CType(Me.pnTransDetail.Controls(mv_arrObjFields(v_intSharp).ControlIndex), ComboBoxEx).SelectedValue.ToString
                                    End Select
                                    strSharp = strSharp & "#" & Format(v_intSharp + 1, "00") & Chr(1) & v_strSharp.Trim & Chr(1)
                                Next
                                mv_frmSearchScreen.Sharp = strSharp
                                mv_frmSearchScreen.InitDialog()
                                mv_frmSearchScreen.ShowDialog()

                                If Not mv_frmSearchScreen.ReturnValue Is Nothing Then
                                    Me.ActiveControl.Text = mv_frmSearchScreen.ReturnValue
                                    If Len(mv_frmSearchScreen.RefValue) > 0 Then
                                        ctl = Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex)
                                        ctl.Top = Me.ActiveControl.Top
                                        ctl.Text = mv_frmSearchScreen.RefValue
                                        ctl.Visible = True
                                    End If
                                    'Náº¡p cÃ¡c giÃ¡ trá»‹ tÆ°Æ¡ng á»©ng cho cÃ¡c trÆ°á»?ng khÃ¡c
                                    strFLDNAME = Mid(ActiveControl.Name, Len(PREFIXED_MSKDATA) + 1)
                                    FillLookupData(strFLDNAME, mv_frmSearchScreen.ReturnValue, mv_frmSearchScreen.FULLDATA, mv_frmSearchScreen.KeyColumn)
                                    mv_frmSearchScreen.Dispose()
                                End If
                        End Select
                    ElseIf mv_arrObjFields(v_intIndex).LookUp = "Y" Then
                        If mv_arrObjFields(v_intIndex).LookupList.Length > 0 Then
                            Dim frm As New frmLookUp(UserLanguage)
                            strSQL = mv_arrObjFields(v_intIndex).LookupList
                            If mv_arrObjFields(v_intIndex).MemberField <> "" And mv_lngAllMember = 0 Then
                                strSQL = "select * from (" & strSQL & ") where " & mv_arrObjFields(v_intIndex).MemberField & " in " & mv_strMemberFilter
                            End If
                            If mv_arrObjFields(v_intIndex).StockField <> "" And mv_lngAllStock = 0 Then
                                strSQL = "select * from (" & strSQL & ") where " & mv_arrObjFields(v_intIndex).StockField & " in " & mv_strStockFilter
                            End If
                            v_intCount = mv_arrObjFields.GetLength(0) - 1
                            For v_intSharp = 0 To v_intCount - 1 Step 1
                                v_strSharp = ""
                                Select Case Trim(mv_arrObjFields(v_intSharp).ControlType)
                                    Case "T"
                                        v_strSharp = CType(Me.pnTransDetail.Controls(mv_arrObjFields(v_intSharp).ControlIndex), TextBox).Text
                                    Case "M"
                                        v_strSharp = CType(Me.pnTransDetail.Controls(mv_arrObjFields(v_intSharp).ControlIndex), FlexMaskEditBox).Text
                                        v_strSharp = IIf(Trim(mv_arrObjFields(v_intSharp).DataType) = "D", v_strSharp.Replace("/  /", ""), v_strSharp)
                                    Case "C"
                                        v_strSharp = CType(Me.pnTransDetail.Controls(mv_arrObjFields(v_intSharp).ControlIndex), ComboBoxEx).SelectedValue.ToString
                                End Select
                                strSQL = strSQL.Replace("#" & Format(v_intSharp + 1, "00"), v_strSharp.Trim)
                            Next

                            frm.SQLCMD = strSQL
                            frm.ShowDialog()
                            If Not frm.RETURNDATA Is Nothing Then
                                v_intPos = InStr(frm.RETURNDATA, vbTab)
                                If v_intPos > 0 Then
                                    Me.ActiveControl.Text = Mid(frm.RETURNDATA, 1, v_intPos - 1)
                                    ctl = Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex)
                                    ctl.Top = Me.ActiveControl.Top
                                    ctl.Text = Mid(frm.RETURNDATA, v_intPos + 1)
                                    ctl.Visible = True
                                    'Náº¡p cÃ¡c giÃ¡ trá»‹ tÆ°Æ¡ng á»©ng cho cÃ¡c trÆ°á»?ng khÃ¡c
                                    strFLDNAME = Mid(ActiveControl.Name, Len(PREFIXED_MSKDATA) + 1)
                                    FillLookupData(strFLDNAME, Mid(frm.RETURNDATA, 1, v_intPos - 1), frm.FULLDATA)
                                End If
                                frm.Dispose()
                            End If
                        Else
                            Dim v_ofdOpenFile As New OpenFileDialog
                            v_ofdOpenFile.Filter = "Excel(*.xls)|*.xls|File XML(*.xml)|*.xml|File Text(*.txt)|*.txt"
                            v_ofdOpenFile.ShowDialog()
                            Me.ActiveControl.Text = v_ofdOpenFile.FileName
                        End If
                    End If
                ElseIf Me.ActiveControl.Name = "mskTransCode" Then
                    'Hiá»ƒn thá»‹ danh sÃ¡ch giao dá»‹ch cá»§a nhÃ³m nghiá»‡p vá»¥ tÆ°Æ¡ng á»©ng
                    Dim frm As New frmLookUp(UserLanguage)

                    frm.SQLCMD = "SELECT a.RPTID VALUE, a.RPTID VALUECD, a.RPTID || '- ' || a.RPTTITTLE DISPLAY, c.modcode DESCRIPTION FROM RPREPORTS a"
                    frm.ShowDialog()
                    v_intPos = InStr(frm.RETURNDATA, vbTab)
                    If v_intPos > 0 Then
                        Me.ActiveControl.Text = Mid(frm.RETURNDATA, 1, v_intPos - 1)
                        'Chuyá»ƒn Ä‘áº¿n control káº¿ tiáº¿p
                        'SelectNextControl(ActiveControl, True, True, True, True)
                        e.Handled = True
                    End If
                    frm.Dispose()
                End If
            Case Keys.Enter
                If mv_bolCheck Then
                    If Me.ActiveControl.Name = "mskTransCode" Then
                        'Náº¡p mÃ n hÃ¬nh má»›i
                        If Len(Trim(mskTransCode.Text)) = 4 Then
                            'mv_strTxDesc = String.Empty
                            LoadScreen(Trim(mskTransCode.Text))
                            'Chuyá»ƒn Ä‘áº¿n control káº¿ tiáº¿p
                            'SendKeys.Send("{Tab}")
                            e.Handled = True
                        End If
                    ElseIf InStr(CType(Me.ActiveControl, Control).Name, PREFIXED_MSKDATA) > 0 Then
                        'Náº¿u lÃ  cÃ¡c trÆ°á»?ng cá»§a giao dá»‹ch thÃ¬ chuyá»ƒn Ä‘áº¿n control káº¿ tiáº¿p
                        'SendKeys.Send("{Tab}")
                        SelectNextControl(ActiveControl, True, True, True, True)
                        e.Handled = True
                    Else

                    End If
                Else
                    mv_bolCheck = True
                End If
        End Select
    End Sub

    Private Sub frmTransaction_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        mv_BDSDelivery = New BDSChannel.BDSDelivery
        OnInit()
    End Sub

    Private Sub OnSubmit()
        v_oProcess = New ProcessForm(Me)
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

                            Else
                                v_strFLDVALUE = v_control.Text 'CType(v_control, Control).Text
                                v_strDisplay = v_strFLDVALUE
                            End If
                            v_strFLDNAME = Mid(CType(v_control, Control).Name, Len(PREFIXED_MSKDATA) + 1)

                            With mv_arrObjFields(v_intindex)
                                v_strClause1 = v_strClause1 & .FieldName & "|" & v_strFLDVALUE & "|" & v_strDisplay & "$"

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
                                v_strClause = v_strClause & .FieldName & "|" & v_strFLDVALUE & "|" & .ColumnName & "$"
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
                        MsgBox(mv_ResourceManager.GetString("ErrPartition"), MsgBoxStyle.Critical, gc_ApplicationTitle)
                        Exit Sub
                    End If
                    v_strMaxDate = v_lstPartition.Split("|")(1)

                    If CheckPassDate(BusDate, v_strMaxDate) Then
                        Me.Cursor = Cursors.Default
                        v_oProcess.StopProcessForm()
                        MsgBox(mv_ResourceManager.GetString("ErrPartition"), MsgBoxStyle.Critical, gc_ApplicationTitle)
                        Exit Sub
                    End If
                End If
                
                v_strObjMsg = BuildXMLObjMsg(Me.BusDate, BranchId, , TellerId, gc_IsNotLocalMsg, gc_MsgTypeObj, OBJNAME_RP_RPREPORT, _
                                 gc_ActionAdhoc, v_lstSICODE & "|" & v_lstMICODE, v_strClause, "HOSTCreateReport", , v_lstPartition)

                v_lngError = mv_BDSDelivery.Message(v_strObjMsg)

                If v_lngError <> ERR_SYSTEM_OK Then
                    Cursor.Current = Cursors.Default
                    v_oProcess.StopProcessForm()
                    MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                    Exit Sub
                End If

                FillFormuleToDataSet(v_strClause1)
                v_oProcess.StopProcessForm()
                If FillDataSet(v_strObjMsg, v_strRptID) Then
                    'If MsgBox("Bạn có muốn lưu lại hay không", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, gc_ApplicationTitle) = MsgBoxResult.Yes Then
                    '    WriteDataTableToFile(v_strRptID)
                    'End If
                    'ResetScreen()
                    ShowReport(v_strRptID)
                Else
                    MsgBox("Không có dữ liệu báo cáo!", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, IIf(Me.UserLanguage = "VN", "Giao dịch", "Transaction"))
                    Exit Sub
                End If

                If v_lngError <> ERR_SYSTEM_OK Then
                    MsgBox("Lỗi tạo file báo cáo. Nêu khởi động lại chương trình bạn sẽ phải tạo lại báo cáo", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, IIf(Me.UserLanguage = "VN", "Giao dịch", "Transaction"))
                    Exit Sub
                End If
                'MessageBox.Show(IIf(Me.UserLanguage = "VN", "Tạo báo cáo thành công!", "This transaction is accepted !"))
                'Me.Close()
            End If
            Me.Cursor = Cursors.Default
        Catch ex As Exception
            v_oProcess.StopProcessForm()
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                                     & "Error code: System error!" & vbNewLine _
                                                     & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
            mv_bolCheck = False
        Finally
            v_oProcess.StopProcessForm()
            v_oProcess = Nothing
        End Try
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

    Private Sub cboData_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            Dim strFLDNAME As String, v_intIndex As Integer
            Dim v_strSQLCMD, v_strFULLDATA As String
            Dim v_strFieldValue As String

            Dim v_strFULLNAME As String
            v_strFULLNAME = ""
            Dim v_xmlDocument As New Xml.XmlDocument
            'Dim i, j As Integer

            If InStr(CType(sender, Control).Name, PREFIXED_MSKDATA) > 0 And (TypeOf (sender) Is ComboBoxEx) Then
                v_intIndex = CType(sender, Control).Tag
                If Not mv_arrObjFields(v_intIndex) Is Nothing Then
                    v_strFieldValue = CType(sender, ComboBoxEx).SelectedValue
                    strFLDNAME = Mid(CType(sender, Control).Name, Len(PREFIXED_MSKDATA) + 1)
                    'Tra cá»©u thÃ´ng tin
                    If mv_arrObjFields(v_intIndex).LookUp = "Y" And InStr(Me.ActiveControl.Name, PREFIXED_MSKDATA) > 0 Then
                        v_strSQLCMD = mv_arrObjFields(v_intIndex).LookupList
                        'Create message to inquiry object fields
                        Dim v_strObjMsg As String
                        'Dim v_ws As New BDSDelivery.BDSDelivery
                        'Láº¥y thÃ´ng tin chung vá»? giao dá»‹ch
                        v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_LOOKUP, gc_ActionInquiry, v_strSQLCMD, "")

                        Dim v_lngError As Long = mv_BDSDelivery.Message(v_strObjMsg)
                        If v_lngError <> ERR_SYSTEM_OK Then
                            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                            Exit Sub
                        End If
                        'LÆ°u trá»¯ danh sÃ¡ch tÃ¬m kiáº¿m tráº£ vá»?
                        v_strFULLDATA = v_strObjMsg
                        'Hiá»ƒn thá»‹ dá»¯ liá»‡u tá»« Lookup
                        FillLookupData(strFLDNAME, v_strFieldValue, v_strFULLDATA)
                    End If
                End If
                'If CType(Me.ActiveControl, ComboBoxEx).SelectedValue.Trim.Length > 0 Then
                '    v_strSQLCMD = "SELECT FULLNAME FROM ISSUERS ISS,SBSECURITIES SB WHERE ISS.SHORTNAME=SB.SYMBOL AND SB.CODEID='" & CType(Me.ActiveControl, ComboBoxEx).SelectedValue & "'"
                '    'Create message to inquiry object fields
                '    Dim v_strObjMsg As String
                '    Dim v_ws As New BDSDelivery.BDSDelivery
                '    'Láº¥y thÃ´ng tin chung vá»? giao dá»‹ch
                '    v_strObjMsg = BuildXMLObjMsg(, , , , gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_LOOKUP, gc_ActionInquiry, v_strSQLCMD, "")
                '    v_ws.Message(v_strObjMsg)
                '    'LÆ°u trá»¯ danh sÃ¡ch tÃ¬m kiáº¿m tráº£ vá»?
                '    v_strFULLDATA = v_strObjMsg
                '    v_xmlDocument.LoadXml(v_strFULLDATA)

                '    v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")
                '    For i = 0 To v_nodeList.Count - 1
                '        For j = 0 To v_nodeList.Item(i).ChildNodes.Count - 1
                '            With v_nodeList.Item(i).ChildNodes(j)
                '                v_strValue = .InnerText.ToString
                '                v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                '                Select Case Trim(v_strFLDNAME)
                '                    Case "FULLNAME"
                '                        v_strFULLNAME = v_strValue
                '                End Select
                '            End With
                '        Next
                '    Next
                '    'If v_strFULLNAME.Trim.Length > 0 Then
                '    v_intIndex = Me.ActiveControl.Tag
                '    ctl = Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex)
                '    ctl.Top = Me.ActiveControl.Top
                '    ctl.Text = v_strFULLNAME
                '    ctl.Visible = True
                '    'End If
                'End If
            End If
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                                     & "Error code: System error!" & vbNewLine _
                                                     & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
            mv_bolCheck = False
        End Try
    End Sub

    Public Overridable Sub mskData_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim v_mskctrl As FlexMaskEditBox
            Dim v_txtctrl As TextBox
            If (TypeOf (sender) Is FlexMaskEditBox) Then
                v_mskctrl = CType(sender, FlexMaskEditBox)
                v_mskctrl.SelectionStart = 0
                v_mskctrl.SelectionLength = Len(v_mskctrl.Text)
            ElseIf (TypeOf (sender) Is TextBox) Then
                v_txtctrl = CType(sender, TextBox)
                v_txtctrl.SelectionStart = 0
                v_txtctrl.SelectionLength = Len(v_txtctrl.Text)
            End If
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                                     & "Error code: System error!" & vbNewLine _
                                                     & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
            mv_bolCheck = False
        End Try
    End Sub

    Public Overridable Sub mskData_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
        Try
            Dim strFLDNAME As String, v_intIndex As Integer
            Dim v_strSQLCMD, v_strFULLDATA As String
            Dim v_nodeList As Xml.XmlNodeList
            Dim v_strDataType As String, v_strValue As String
            Dim v_strDisplay As String = "", v_strFLDNAME As String
            Dim v_strFieldValue As String
            Dim v_strFldSource, v_strFldDesc As String
            Dim v_intSharp, v_intSharp1, v_intCount As Integer
            Dim v_strSharp As String
            Dim v_cboData As ComboBoxEx
            Dim v_strObjMsg As String
            Dim v_lngError As Long

            Dim v_bolCheck As Boolean = False
            'If InStr(Me.ActiveControl.Name, PREFIXED_MSKDATA) > 0 Then
            If InStr(CType(sender, Control).Name, PREFIXED_MSKDATA) > 0 And mv_Check Then
                v_intIndex = CType(sender, Control).Tag
                If Not mv_arrObjFields(v_intIndex) Is Nothing Then
                    v_strFieldValue = Trim(CType(sender, Control).Text)
                    v_strFieldValue = IIf(Trim(mv_arrObjFields(v_intIndex).DataType) = "D", v_strFieldValue.Replace("/  /", ""), v_strFieldValue)
                    strFLDNAME = Mid(CType(sender, Control).Name, Len(PREFIXED_MSKDATA) + 1)

                    'Thông báo phải nhập dữ liệu
                    If mv_arrObjFields(v_intIndex).Mandatory = True And Len(v_strFieldValue) = 0 Then
                        Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Visible = True
                        Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Text = mv_ResourceManager.GetString("ERR_NULL_VALUE")
                        Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).ForeColor = System.Drawing.Color.Red
                        Exit Sub
                    End If

                    If Len(v_strFieldValue) > 0 Then
                        v_strDataType = Trim(mv_arrObjFields(v_intIndex).DataType)
                        Select Case v_strDataType
                            Case "N"
                                If Not IsNumeric(v_strFieldValue) Then
                                    Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Visible = True
                                    Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Text = mv_ResourceManager.GetString("ERR_NUMERIC_VALUE")
                                    Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).ForeColor = System.Drawing.Color.Red
                                    Exit Sub
                                Else
                                    If Trim(mv_arrObjFields(v_intIndex).ControlType) = "T" Then
                                        FormatNumericTextbox(CType(sender, TextBox))
                                    End If
                                End If
                            Case "D"
                                If Not IsDateValue(v_strFieldValue) Then
                                    Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Visible = True
                                    Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Text = mv_ResourceManager.GetString("ERR_DATE_VALUE")
                                    Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).ForeColor = System.Drawing.Color.Red
                                    Exit Sub
                                End If
                        End Select
                    End If
                    Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Text = ""
                    Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).ForeColor = System.Drawing.Color.Blue
                    'Fill du lieu lookup từ HOST

                    If Len(mv_arrObjFields(v_intIndex).SearchCode) > 0 Then
                        Dim ctlCheck As Control
                        ctlCheck = Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).ControlIndex)
                        If Not (mv_arrObjFields(v_intIndex).Mandatory = False And ctlCheck.Text.Trim.Length = 0) Then
                            'Kiểm tra dữ liệu nhập vào có đúng không?

                            Dim v_strSEARCHSQL As String = "", v_strSEARCHCODE As String, v_strRefValue As String = ""
                            Dim v_strFIELDCODE As String = "", v_strKeyVal As String, v_strKeyName As String = "", v_strOBJNAME As String = ""
                            Dim v_xmlDocument As New Xml.XmlDocument
                            'Dim v_ws As New BDSDelivery.BDSDelivery
                            v_strSEARCHCODE = mv_arrObjFields(v_intIndex).SearchCode
                            v_strKeyVal = Replace(v_strFieldValue, ".", "")
                            'Lấy KeyName
                            v_strSQLCMD = "SELECT SISEARCHFLD.FIELDCODE KEYNAME,SISEARCH.SEARCHCMDSQL FROM SISEARCHFLD,SISEARCH " & ControlChars.CrLf _
                            & " WHERE SISEARCH.SEARCHCODE = SISEARCHFLD.SEARCHCODE " & ControlChars.CrLf _
                            & " AND SISEARCHFLD.KEY ='Y' AND SISEARCHFLD.SEARCHCODE ='" & v_strSEARCHCODE & "'"
                            v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_LOOKUP, gc_ActionInquiry, v_strSQLCMD)

                            v_lngError = mv_BDSDelivery.Message(v_strObjMsg)
                            If v_lngError <> ERR_SYSTEM_OK Then
                                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                                Exit Sub
                            End If

                            v_xmlDocument.LoadXml(v_strObjMsg)
                            v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")
                            If v_nodeList.Count > 0 Then
                                For i As Integer = 0 To v_nodeList.Count - 1
                                    For j As Integer = 0 To v_nodeList.Item(i).ChildNodes.Count - 1 Step 1
                                        With v_nodeList.Item(i).ChildNodes(j)
                                            v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                                            v_strValue = Trim(.InnerText)
                                            Select Case v_strFLDNAME
                                                Case "KEYNAME"
                                                    v_strKeyName = Trim(v_strValue)
                                                Case "SEARCHCMDSQL"
                                                    v_strSEARCHSQL = Trim(v_strValue)
                                            End Select
                                        End With
                                    Next
                                Next
                                v_strSEARCHSQL = v_strSEARCHSQL.Replace("?LANGUAGE", Me.UserLanguage)
                                v_strSEARCHSQL = v_strSEARCHSQL.Replace("?MEMBERFILTER", IIf(mv_lngAllMember = 0, " NVL(a.micode,'000') in " & mv_strMemberFilter, " 1=1 "))
                                v_strSEARCHSQL = v_strSEARCHSQL.Replace("?COMEMBERFILTER", IIf(mv_lngAllMember = 0, " NVL(a.comicode,'000') in " & mv_strMemberFilter, " 1=1 "))
                                v_strSEARCHSQL = v_strSEARCHSQL.Replace("?STOCKFILTER", IIf(mv_lngAllStock = 0, " NVL(a.sicode,'000') in " & mv_strStockFilter, " 1=1 "))
                                v_strSEARCHSQL = v_strSEARCHSQL.Replace("?BRID", mv_strBranchId)
                                v_strSEARCHSQL = v_strSEARCHSQL.Replace("?TLID", mv_strTellerId)
                                'them rang buoc cac control

                                v_intCount = mv_arrObjFields.GetLength(0) - 1
                                For v_intSharp = 0 To v_intCount - 1 Step 1
                                    v_strSharp = ""
                                    Select Case Trim(mv_arrObjFields(v_intSharp).ControlType)
                                        Case "T"
                                            v_strSharp = CType(Me.pnTransDetail.Controls(mv_arrObjFields(v_intSharp).ControlIndex), TextBox).Text
                                        Case "M"
                                            v_strSharp = CType(Me.pnTransDetail.Controls(mv_arrObjFields(v_intSharp).ControlIndex), FlexMaskEditBox).Text
                                            v_strSharp = IIf(Trim(mv_arrObjFields(v_intSharp).DataType) = "D", v_strSharp.Replace("/  /", ""), v_strSharp)
                                        Case "C"
                                            v_strSharp = CType(Me.pnTransDetail.Controls(mv_arrObjFields(v_intSharp).ControlIndex), ComboBoxEx).SelectedValue.ToString
                                    End Select
                                    v_strSEARCHSQL = v_strSEARCHSQL.Replace("#" & Format(v_intSharp + 1, "00"), v_strSharp.Trim)
                                Next
                                'Kiá»ƒm tra thÃ´ng tin cá»§a nháº­p vÃ o cÃ³ Ä‘Ãºng khÃ´ng?
                                v_strSQLCMD = "SELECT * FROM (" & v_strSEARCHSQL & ") WHERE " & v_strKeyName & " = '" & v_strKeyVal & "'"
                                'v_strObjMsg = BuildXMLObjMsg(, , , , gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_LOOKUP, gc_ActionInquiry, v_strSQLCMD)
                                v_strObjMsg = BuildXMLObjMsg(Me.BusDate, Me.BranchId, , Me.TellerId, gc_IsNotLocalMsg, gc_MsgTypeObj, "TR." & v_strSEARCHCODE, _
                                                    gc_ActionInquiry, v_strSQLCMD, , , True, StockFilter & "|" & MemberFilter)

                                v_lngError = mv_BDSDelivery.Message(v_strObjMsg)
                                If v_lngError <> ERR_SYSTEM_OK Then
                                    MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                                    Exit Sub
                                End If

                                v_xmlDocument.LoadXml(v_strObjMsg)
                                v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")
                                If v_nodeList.Count > 0 Then
                                    'Nạp giá trị tương ứng cho các trường khác
                                    FillLookupData(strFLDNAME, v_strFieldValue, v_strObjMsg, v_strKeyName)
                                    'Fill Refval
                                    v_strSQLCMD = "SELECT SEARCHCMDSQL,SE.SEARCHCODE,SEFLD.FIELDCODE, SISEARCHFLD.FIELDCODE KEYNAME FROM SISEARCH SE,SISEARCHFLD SEFLD,SISEARCHFLD WHERE SE.SEARCHCODE=SEFLD.SEARCHCODE" & ControlChars.CrLf _
                                                & "AND SE.SEARCHCODE=SISEARCHFLD.SEARCHCODE AND SE.SEARCHCODE='" & v_strSEARCHCODE & "' AND SEFLD.REFVALUE='Y' AND SISEARCHFLD.KEY='Y'"
                                    v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_LOOKUP, gc_ActionInquiry, v_strSQLCMD, "")

                                    v_lngError = mv_BDSDelivery.Message(v_strObjMsg)
                                    If v_lngError <> ERR_SYSTEM_OK Then
                                        MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                                        Exit Sub
                                    End If

                                    v_xmlDocument.LoadXml(v_strObjMsg)
                                    v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")
                                    If v_nodeList.Count > 0 Then
                                        For i As Integer = 0 To v_nodeList.Count - 1
                                            For j As Integer = 0 To v_nodeList.Item(i).ChildNodes.Count - 1 Step 1
                                                With v_nodeList.Item(i).ChildNodes(j)
                                                    v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                                                    v_strValue = Trim(.InnerText)
                                                    Select Case v_strFLDNAME
                                                        Case "FIELDCODE"
                                                            v_strFIELDCODE = Trim(v_strValue)
                                                    End Select
                                                End With
                                            Next
                                        Next
                                        'v_strSQLCMD = "SELECT " & v_strFIELDCODE & " FROM  (" & v_strSEARCHSQL & ") WHERE REPLACE(" & v_strKeyName & ",'.','')='" & v_strKeyVal & "'"
                                        v_strSQLCMD = "SELECT " & v_strFIELDCODE & " FROM  (" & v_strSEARCHSQL & ") WHERE " & v_strKeyName & "='" & v_strKeyVal & "'"
                                        v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_LOOKUP, gc_ActionInquiry, v_strSQLCMD, "")

                                        v_lngError = mv_BDSDelivery.Message(v_strObjMsg)
                                        If v_lngError <> ERR_SYSTEM_OK Then
                                            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                                            Exit Sub
                                        End If

                                        v_xmlDocument.LoadXml(v_strObjMsg)
                                        v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")
                                        If v_nodeList.Count > 0 Then
                                            For i As Integer = 0 To v_nodeList.Count - 1
                                                For j As Integer = 0 To v_nodeList.Item(i).ChildNodes.Count - 1 Step 1
                                                    With v_nodeList.Item(i).ChildNodes(j)
                                                        v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                                                        v_strValue = Trim(.InnerText)
                                                        Select Case v_strFLDNAME
                                                            Case v_strFIELDCODE
                                                                v_strRefValue = v_strValue
                                                        End Select
                                                    End With
                                                Next
                                            Next
                                            If Len(v_strRefValue) > 0 Then
                                                'Fill Refval 
                                                Dim ctl As Control
                                                ctl = Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex)
                                                ctl.Top = sender.Top
                                                ctl.Text = v_strRefValue
                                                ctl.ForeColor = System.Drawing.Color.Blue
                                                ctl.Visible = True
                                            End If
                                        End If
                                    End If
                                Else
                                    'Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Visible = True
                                    'Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Text = mv_ResourceManager.GetString("ERR_LOOKUP_VALUE")
                                    'Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).ForeColor = System.Drawing.Color.Red
                                    'Exit Sub
                                End If
                            End If
                        End If
                    ElseIf mv_arrObjFields(v_intIndex).LookUp = "Y" Then
                        'If mv_arrObjFields(v_intIndex).LookupList.Length > 0 Then
                        '    Dim ctlCheck As Control
                        '    ctlCheck = Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).ControlIndex)
                        '    If Not (mv_arrObjFields(v_intIndex).Mandatory = False And ctlCheck.Text.Trim.Length = 0) Then
                        '        'Chá»‰ kiá»ƒm tra náº¿u cÃ³ yÃªu cáº§u nháº­p má»›i kiá»ƒm tra
                        '        'Fill DL lookup á»Ÿ BDS 
                        '        v_strSQLCMD = mv_arrObjFields(v_intIndex).LookupList
                        '        If mv_arrObjFields(v_intIndex).MemberField <> "" And mv_lngAllMember = 0 Then
                        '            v_strSQLCMD = "select * from (" & v_strSQLCMD & ") where " & mv_arrObjFields(v_intIndex).MemberField & " in " & mv_strMemberFilter
                        '        End If
                        '        If mv_arrObjFields(v_intIndex).StockField <> "" And mv_lngAllStock = 0 Then
                        '            v_strSQLCMD = "select * from (" & v_strSQLCMD & ") where " & mv_arrObjFields(v_intIndex).StockField & " in " & mv_strStockFilter
                        '        End If
                        '        'them rang buoc cac control

                        '        v_intCount = mv_arrObjFields.GetLength(0) - 1
                        '        For v_intSharp = 0 To v_intCount - 1 Step 1
                        '            v_strSharp = ""
                        '            Select Case Trim(mv_arrObjFields(v_intSharp).ControlType)
                        '                Case "T"
                        '                    v_strSharp = CType(Me.pnTransDetail.Controls(mv_arrObjFields(v_intSharp).ControlIndex), TextBox).Text
                        '                Case "M"
                        '                    v_strSharp = CType(Me.pnTransDetail.Controls(mv_arrObjFields(v_intSharp).ControlIndex), FlexMaskEditBox).Text
                        '                    v_strSharp = IIf(Trim(mv_arrObjFields(v_intSharp).DataType) = "D", v_strSharp.Replace("/  /", ""), v_strSharp)
                        '                Case "C"
                        '                    v_strSharp = CType(Me.pnTransDetail.Controls(mv_arrObjFields(v_intSharp).ControlIndex), ComboBoxEx).SelectedValue.ToString
                        '            End Select
                        '            v_strSQLCMD = v_strSQLCMD.Replace("#" & Format(v_intSharp + 1, "00"), v_strSharp.Trim)
                        '        Next

                        '        'Create message to inquiry object fields

                        '        Dim v_xmlDocument As New Xml.XmlDocument
                        '        'Láº¥y thÃ´ng tin chung vá»? giao dá»‹ch
                        '        v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_LOOKUP, gc_ActionInquiry, v_strSQLCMD, "")

                        '        v_lngError = mv_BDSDelivery.Message(v_strObjMsg)
                        '        If v_lngError <> ERR_SYSTEM_OK Then
                        '            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                        '            Exit Sub
                        '        End If

                        '        'LÆ°u trá»¯ danh sÃ¡ch tÃ¬m kiáº¿m tráº£ vá»?
                        '        v_strFULLDATA = v_strObjMsg
                        '        v_xmlDocument.LoadXml(v_strObjMsg)
                        '        v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")
                        '        'Kiá»ƒm tra xem giÃ¡ trá»‹ co há»£p lá»‡ khÃ´ng
                        '        For i As Integer = 0 To v_nodeList.Count - 1
                        '            For j As Integer = 0 To v_nodeList.Item(i).ChildNodes.Count - 1 Step 1
                        '                With v_nodeList.Item(i).ChildNodes(j)
                        '                    If CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value) = "VALUE" Then
                        '                        v_strValue = Trim(.InnerText.ToString)
                        '                        If v_strFieldValue = v_strValue Then
                        '                            For k As Integer = 0 To v_nodeList.Item(i).ChildNodes.Count - 1 Step 1
                        '                                With v_nodeList.Item(i).ChildNodes(k)
                        '                                    If CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value) = "DISPLAY" Then
                        '                                        v_strDisplay = Trim(.InnerText.ToString)
                        '                                    End If
                        '                                End With
                        '                            Next
                        'Dim ctl As Control
                        'ctl = Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex)
                        'ctl.Top = sender.Top
                        'ctl.Text = v_strDisplay
                        'ctl.Visible = True
                        'ctl.ForeColor = System.Drawing.Color.Blue
                        '                            v_bolCheck = True
                        '                            Exit For
                        '                        End If
                        '                    End If
                        '                End With
                        '            Next
                        '        Next
                        '        If v_bolCheck = True Then
                        '            'Hiá»ƒn thá»‹ dá»¯ liá»‡u tá»« Lookup
                        '            FillLookupData(strFLDNAME, v_strFieldValue, v_strFULLDATA)
                        '        Else
                        '            'ThÃ´ng bÃ¡o lá»—i
                        '            Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Visible = True
                        '            Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Text = mv_ResourceManager.GetString("ERR_LOOKUP_VALUE")
                        '            Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).ForeColor = System.Drawing.Color.Red
                        '            Exit Sub
                        '        End If
                        '    End If
                        'Else
                        '    If Not IO.File.Exists(v_strFieldValue) Then
                        '        Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Visible = True
                        '        Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Text = mv_ResourceManager.GetString("ERR_INVALID_EXISTED_VALUE")
                        '        Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).ForeColor = System.Drawing.Color.Red
                        '        Exit Sub
                        '    End If
                        'End If
                    End If
                    'TrÆ°á»?ng Inventory
                    If Len(mv_arrObjFields(v_intIndex).InvName) > 0 Then
                        v_strFldSource = mv_arrObjFields(v_intIndex).FldSource
                        v_strFldDesc = mv_arrObjFields(v_intIndex).FldDesc
                        'Them
                        Dim v_ctrl As Control, v_ctlmskData As FlexMaskEditBox

                        For Each v_ctrl In Me.pnTransDetail.Controls
                            If TypeOf (v_ctrl) Is FlexMaskEditBox Then
                                v_ctlmskData = CType(v_ctrl, FlexMaskEditBox)
                                v_strFLDNAME = Mid(v_ctlmskData.Name, Len(PREFIXED_MSKDATA) + 1)
                                If Trim(v_strFldDesc) = Trim(v_strFLDNAME) Then
                                    'v_strTemp = Strings.Right(gc_FORMAT_ODAUTOID & v_strValue, CType(v_ctrl, FlexMaskEditBox).MaxLength - Len(v_strINVReturn))
                                    If Len(CType(v_ctrl, FlexMaskEditBox).Text) = 0 Then
                                        GetInventory(v_strFldSource, v_strFldDesc, mv_arrObjFields(v_intIndex).InvName, mv_arrObjFields(v_intIndex).InvFormat)
                                        Exit For
                                    End If
                                End If
                            End If
                        Next
                    End If

                    ' load lai cac combox co #

                    For v_intSharp1 = 0 To v_intCount - 1 Step 1
                        If Trim(mv_arrObjFields(v_intSharp1).ControlType) = "C" And v_intSharp1 <> v_intIndex Then
                            v_strSQLCMD = mv_arrObjFields(v_intSharp1).LookupList
                            If InStr(v_strSQLCMD, "#") > 0 Then
                                v_intCount = mv_arrObjFields.GetLength(0) - 1
                                For v_intSharp = 0 To v_intCount - 1 Step 1
                                    v_strSharp = ""
                                    Select Case Trim(mv_arrObjFields(v_intSharp).ControlType)
                                        Case "T"
                                            v_strSharp = CType(Me.pnTransDetail.Controls(mv_arrObjFields(v_intSharp).ControlIndex), TextBox).Text
                                        Case "M"
                                            v_strSharp = CType(Me.pnTransDetail.Controls(mv_arrObjFields(v_intSharp).ControlIndex), FlexMaskEditBox).Text
                                            v_strSharp = IIf(Trim(mv_arrObjFields(v_intSharp).DataType) = "D", v_strSharp.Replace("/  /", ""), v_strSharp)
                                        Case "C"
                                            v_strSharp = CType(Me.pnTransDetail.Controls(mv_arrObjFields(v_intSharp).ControlIndex), ComboBoxEx).SelectedValue.ToString
                                    End Select
                                    v_strSQLCMD = v_strSQLCMD.Replace("#" & Format(v_intSharp + 1, "00"), v_strSharp.Trim)
                                Next
                                If mv_arrObjFields(v_intSharp1).MemberField <> "" And mv_lngAllMember = 0 Then
                                    v_strSQLCMD = "select * from (" & v_strSQLCMD & ") where " & mv_arrObjFields(v_intSharp1).MemberField & " in " & mv_strMemberFilter
                                End If
                                If mv_arrObjFields(v_intSharp1).StockField <> "" And mv_lngAllStock = 0 Then
                                    v_strSQLCMD = "select * from (" & v_strSQLCMD & ") where " & mv_arrObjFields(v_intSharp1).StockField & " in " & mv_strStockFilter
                                End If

                                v_strObjMsg = BuildXMLObjMsg(, BranchId, , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_LOOKUP, gc_ActionInquiry, v_strSQLCMD)

                                v_lngError = mv_BDSDelivery.Message(v_strObjMsg)
                                If v_lngError <> ERR_SYSTEM_OK Then
                                    MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                                    Exit Sub
                                End If
                                v_cboData = CType(Me.pnTransDetail.Controls(mv_arrObjFields(v_intSharp1).ControlIndex), ComboBoxEx)
                                FillComboEx(v_strObjMsg, v_cboData)
                            End If
                        End If
                    Next

                End If
            End If

        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                                     & "Error code: System error!" & vbNewLine _
                                                     & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
            mv_bolCheck = False
        End Try
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        OnSubmit()
    End Sub
#End Region

#Region "Kiểm tra dữ liệu"

    Private Function BuildAMTEXP(ByVal strAMTEXP As String, Optional ByVal arrValue As Array = Nothing, Optional ByVal intVIndex As Integer = -1) As String
        Try
            Dim v_strEvaluator As String, v_strElemenent As String, v_strValue As String = ""
            Dim v_lngIndex As Long, v_ctl As Control
            Dim i As Integer

            If arrValue Is Nothing Then
                v_strEvaluator = vbNullString
                v_lngIndex = 1
                If InStr(strAMTEXP, "@") > 0 Then
                    Return Mid(strAMTEXP, 2)
                End If

                While v_lngIndex < Len(strAMTEXP)
                    'Get 02 charatacters in AMTEXP
                    v_strElemenent = Mid$(strAMTEXP, v_lngIndex, 2)
                    Select Case v_strElemenent
                        Case "++", "--", "**", "//", "((", "))"
                            'Operand
                            v_strEvaluator = v_strEvaluator & Mid(v_strElemenent, 1, 1)
                        Case Else
                            'Operator
                            For Each v_ctl In Me.pnTransDetail.Controls
                                'If InStr(v_ctl.Name, PREFIXED_MSKDATA & v_strElemenent) > 0 And TypeOf (v_ctl) Is FlexMaskEditBox Then
                                If InStr(v_ctl.Name, PREFIXED_MSKDATA & v_strElemenent) > 0 Then
                                    If TypeOf (v_ctl) Is ComboBoxEx Then
                                        v_strValue = CType(v_ctl, ComboBoxEx).SelectedValue
                                    Else
                                        v_strValue = Replace(v_ctl.Text, ",", "")
                                    End If
                                    Exit For
                                End If
                            Next
                            v_strEvaluator = v_strEvaluator & v_strValue
                    End Select
                    v_lngIndex = v_lngIndex + 2
                End While

            Else
                v_strEvaluator = vbNullString
                v_lngIndex = 1
                If InStr(strAMTEXP, "@") > 0 Then
                    Return Mid(strAMTEXP, 2)
                End If

                While v_lngIndex < Len(strAMTEXP)
                    'Get 02 charatacters in AMTEXP
                    v_strElemenent = Mid$(strAMTEXP, v_lngIndex, 2)
                    Select Case v_strElemenent
                        'Case "FF"
                        '    'Fee amount
                        '    v_strEvaluator = v_strEvaluator & v_strFEEAMT
                        'Case "VV"
                        '    'VAT amount
                        '    v_strEvaluator = v_strEvaluator & v_strVATAMT

                        Case "++", "--", "**", "//", "((", "))"
                            'Operand
                            v_strEvaluator = v_strEvaluator & Mid(v_strElemenent, 1, 1)
                        Case Else
                            For i = 0 To mv_arrObjCoFields.Length - 1
                                If mv_arrObjCoFields(i).FieldName = v_strElemenent Then
                                    Exit For
                                End If
                            Next
                            'Operator
                            v_strEvaluator = v_strEvaluator & CStr(arrValue(intVIndex, i + 1)).Replace(",", "")
                    End Select
                    v_lngIndex = v_lngIndex + 2
                End While
            End If

            Return v_strEvaluator

        Catch ex As Exception
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            '             & "Error code: System error!" & vbNewLine _
            '             & "Error message: " & ex.Message, EventLogEntryType.Error)
            Throw ex
        End Try
    End Function

    Private Function BuildCONCAT(ByVal strAMTEXP As String, Optional ByVal arrValue As Array = Nothing, Optional ByVal intVIndex As Integer = -1) As String
        Try
            Dim v_strEvaluator As String = "", v_strElemenent As String = "", v_strValue As String = ""
            Dim v_lngIndex As Long, v_ctl As Control
            Dim i As Integer

            v_strEvaluator = vbNullString
            v_lngIndex = 1

            If arrValue Is Nothing Then
                While v_lngIndex < Len(strAMTEXP)
                    'Get 02 charatacters in AMTEXP
                    v_strElemenent = Mid$(strAMTEXP, v_lngIndex, 2)
                    Select Case v_strElemenent
                        Case "&&"
                            'Operand
                            'v_strEvaluator = v_strEvaluator & Mid(v_strElemenent, 1, 1)
                        Case Else
                            'Operator
                            For Each v_ctl In Me.pnTransDetail.Controls
                                'If InStr(v_ctl.Name, PREFIXED_MSKDATA & v_strElemenent) > 0 And TypeOf (v_ctl) Is FlexMaskEditBox Then
                                If InStr(v_ctl.Name, PREFIXED_MSKDATA & v_strElemenent) > 0 Then
                                    If TypeOf (v_ctl) Is ComboBoxEx Then
                                        v_strValue = CType(v_ctl, ComboBoxEx).SelectedValue
                                    Else
                                        v_strValue = v_ctl.Text
                                    End If
                                    Exit For
                                End If
                            Next
                            v_strEvaluator = v_strEvaluator & v_strValue
                    End Select
                    v_lngIndex = v_lngIndex + 2
                End While
            Else
                While v_lngIndex < Len(strAMTEXP)
                    'Get 02 charatacters in AMTEXP
                    v_strElemenent = Mid$(strAMTEXP, v_lngIndex, 2)
                    Select Case v_strElemenent
                        Case "&&"
                            'Operand
                            'v_strEvaluator = v_strEvaluator & Mid(v_strElemenent, 1, 1)
                        Case Else
                            For i = 0 To mv_arrObjCoFields.Length - 1
                                If mv_arrObjCoFields(i).FieldName = v_strElemenent Then
                                    Exit For
                                End If
                            Next
                            'Operator
                            v_strEvaluator = v_strEvaluator & CStr(arrValue(intVIndex, i + 1)).Replace(",", "")
                    End Select
                    v_lngIndex = v_lngIndex + 2
                End While
            End If

            Return v_strEvaluator
        Catch ex As Exception
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            '             & "Error code: System error!" & vbNewLine _
            '             & "Error message: " & ex.Message, EventLogEntryType.Error)
            Throw ex
        End Try
    End Function
    'Hanm5 them ham nay

    Private Function GetValueControlByName(ByVal pv_strCtrName As String) As String
        Dim v_strCtrValue As String = ""
        Try
            For Each v_ctr As Control In Me.pnTransDetail.Controls
                If CType(v_ctr, Control).Name = pv_strCtrName Then
                    If TypeOf v_ctr Is TextBox Then
                        v_strCtrValue = CType(CType(v_ctr, TextBox).Text, String)
                    ElseIf TypeOf v_ctr Is ComboBoxEx Then
                        v_strCtrValue = CType(CType(v_ctr, ComboBoxEx).ValueMember, String)
                    ElseIf TypeOf v_ctr Is FlexMaskEditBox Then
                        v_strCtrValue = CType(CType(v_ctr, FlexMaskEditBox).Text, String)
                    End If
                End If
            Next
            Return v_strCtrValue
        Catch ex As Exception
            'LogError.Write(ex.Message & vbNewLine & ex.StackTrace, EventLogEntryType.Error)
            'MsgBox(ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
            Throw ex
        End Try
    End Function

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

            v_strClause = "SELECT a.VARVALUE FROM sysvar a where a.GRNAME='SYSTEM' and a.VARNAME= 'BRSTATUS' and BRID='" & Me.BranchId & "'"
            v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLTX, gc_ActionInquiry, v_strClause)
            mv_BDSDelivery.Message(v_strObjMsg)
            v_xmlDocument.LoadXml(v_strObjMsg)
            If v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='VARVALUE']").InnerText <> OPERATION_ACTIVE Then
                v_oProcess.StopProcessForm()
                MsgBox(mv_ResourceManager.GetString("ErrorMsg"), MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, mv_ResourceManager.GetString("Title"))
                mv_bolCheck = False
                Return False
            End If

            'Check data control before commit          
            For Each v_control As Control In Me.pnTransDetail.Controls
                If InStr(CType(v_control, Control).Name, PREFIXED_MSKDATA) > 0 Then
                    v_intIndex = CType(v_control, Control).Tag
                    If Not mv_arrObjFields(v_intIndex) Is Nothing Then
                        If (TypeOf (v_control) Is ComboBoxEx) Then
                            v_strFieldValue = CType(v_control, ComboBox).SelectedValue
                        Else
                            v_strFieldValue = IIf(Trim(mv_arrObjFields(v_intIndex).DataType) = "D", Trim(v_control.Text).Replace("/  /", ""), Trim(v_control.Text))
                        End If
                        strFLDNAME = Mid(CType(v_control, Control).Name, Len(PREFIXED_MSKDATA) + 1)
                        If (mv_arrObjFields(v_intIndex).Mandatory = True And mv_arrObjFields(v_intIndex).Visible = True And mv_arrObjFields(v_intIndex).Enabled = True _
                                And Len(v_strFieldValue) = 0) Or Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).ForeColor = System.Drawing.Color.Red Then
                            v_oProcess.StopProcessForm()
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
            'MsgBox(ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
            'mv_bolCheck = False
            Throw ex
        End Try
    End Function
#End Region

    Private Sub mskTransCode_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles mskTransCode.GotFocus
        mskTransCode.SelectionStart = 0
        mskTransCode.SelectionLength = Len(Trim(mskTransCode.Text))
    End Sub
    Private Sub ResetScreen(Optional ByVal pv_ResetPostmap As Boolean = True)
        Me.pnTransDetail.Controls.Clear()
        Me.mskTransCode.Enabled = True
        Me.mskTransCode.Text = String.Empty
        Me.pnTransDetail.Visible = False
        mv_arrObjFields = Nothing
        lblTransCaption.Text = vbNullString
        Me.btnOK.Top = PANEL_TOP
        Me.btnCancel.Top = PANEL_TOP
        'Ä?áº·t láº¡i Ä‘á»™ cao cá»§a mÃ n hÃ¬nh
        Me.Height = Me.btnOK.Top + Me.btnOK.Height + CONTROL_GAP * 20
        Me.ActiveControl = Me.mskTransCode
    End Sub

    Private Sub OnClose()
        mv_Check = False
        Me.Close()
    End Sub
    Protected Overridable Sub SetLookUpDataForm()

    End Sub
    Public Overridable Sub v_cboData_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
        Try
            Dim strFLDNAME As String, v_intIndex As Integer
            Dim v_strSQLCMD, v_strFULLDATA As String
            Dim v_nodeList As Xml.XmlNodeList
            Dim v_strValue As String, v_strDisplay As String = "", v_strFLDNAME As String
            Dim v_strFieldValue As String
            Dim v_strFldSource, v_strFldDesc As String

            Dim v_bolCheck As Boolean = False

            Dim v_intSharp, v_intSharp1, v_intCount As Integer
            Dim v_strSharp, v_strObjMsg As String
            Dim v_cboData As ComboBoxEx
            Dim v_lngError As Long


            'If InStr(Me.ActiveControl.Name, PREFIXED_MSKDATA) > 0 Then
            If InStr(CType(sender, Control).Name, PREFIXED_MSKDATA) > 0 And mv_Check Then
                v_intIndex = CType(sender, Control).Tag
                If Not mv_arrObjFields(v_intIndex) Is Nothing Then
                    v_strFieldValue = CType(sender, ComboBoxEx).SelectedValue
                    strFLDNAME = Mid(CType(sender, Control).Name, Len(PREFIXED_MSKDATA) + 1)
                    If mv_arrObjFields(v_intIndex).Mandatory = True And InStr(Me.ActiveControl.Name, PREFIXED_MSKDATA) > 0 _
                            And (Len(v_strFieldValue) = 0) Then
                        Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Visible = True
                        Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Text = mv_ResourceManager.GetString("ERR_LOOKUP_VALUE")
                        Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).ForeColor = System.Drawing.Color.Red
                        Exit Sub
                    End If
                    Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Text = ""
                    Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).ForeColor = System.Drawing.Color.Blue
                    Dim ctlCheck As Control
                    ctlCheck = Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).ControlIndex)
                    If Not (mv_arrObjFields(v_intIndex).Mandatory = False And ctlCheck.Text.Trim.Length = 0) Then
                        'Chá»‰ kiá»ƒm tra náº¿u cÃ³ yÃªu cáº§u nháº­p má»›i kiá»ƒm tra
                        'Fill DL lookup á»Ÿ BDS
                        If Not (v_strFieldValue = "-1" And mv_arrObjFields(v_intIndex).RiskField) Then
                            v_strSQLCMD = mv_arrObjFields(v_intIndex).LookupList
                            If mv_arrObjFields(v_intIndex).MemberField <> "" And mv_lngAllMember = 0 Then
                                v_strSQLCMD = "select * from (" & v_strSQLCMD & ") where " & mv_arrObjFields(v_intIndex).MemberField & " in " & mv_strMemberFilter
                            End If
                            If mv_arrObjFields(v_intIndex).StockField <> "" And mv_lngAllStock = 0 Then
                                v_strSQLCMD = "select * from (" & v_strSQLCMD & ") where " & mv_arrObjFields(v_intIndex).StockField & " in " & mv_strStockFilter
                            End If
                            'them rang buoc cac control

                            v_intCount = mv_arrObjFields.GetLength(0) - 1
                            For v_intSharp = 0 To v_intCount - 1 Step 1
                                v_strSharp = ""
                                Select Case Trim(mv_arrObjFields(v_intSharp).ControlType)
                                    Case "T"
                                        v_strSharp = CType(Me.pnTransDetail.Controls(mv_arrObjFields(v_intSharp).ControlIndex), TextBox).Text
                                    Case "M"
                                        v_strSharp = CType(Me.pnTransDetail.Controls(mv_arrObjFields(v_intSharp).ControlIndex), FlexMaskEditBox).Text
                                        v_strSharp = IIf(Trim(mv_arrObjFields(v_intSharp).DataType) = "D", v_strSharp.Replace("/  /", ""), v_strSharp)
                                    Case "C"
                                        v_strSharp = CType(Me.pnTransDetail.Controls(mv_arrObjFields(v_intSharp).ControlIndex), ComboBoxEx).SelectedValue.ToString
                                End Select
                                v_strSQLCMD = v_strSQLCMD.Replace("#" & Format(v_intSharp + 1, "00"), v_strSharp.Trim)
                            Next
                            'Create message to inquiry object fields

                            Dim v_xmlDocument As New Xml.XmlDocument
                            'Láº¥y thÃ´ng tin chung vá»? giao dá»‹ch
                            v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_LOOKUP, gc_ActionInquiry, v_strSQLCMD, "")

                            v_lngError = mv_BDSDelivery.Message(v_strObjMsg)
                            If v_lngError <> ERR_SYSTEM_OK Then
                                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                                Exit Sub
                            End If

                            'LÆ°u trá»¯ danh sÃ¡ch tÃ¬m kiáº¿m tráº£ vá»?
                            v_strFULLDATA = v_strObjMsg
                            v_xmlDocument.LoadXml(v_strObjMsg)
                            v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")
                            'Kiá»ƒm tra xem giÃ¡ trá»‹ co há»£p lá»‡ khÃ´ng
                            For i As Integer = 0 To v_nodeList.Count - 1
                                For j As Integer = 0 To v_nodeList.Item(i).ChildNodes.Count - 1 Step 1
                                    With v_nodeList.Item(i).ChildNodes(j)
                                        If CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value) = "VALUE" Then
                                            v_strValue = Trim(.InnerText.ToString)
                                            If v_strFieldValue = v_strValue Then
                                                v_bolCheck = True
                                                Exit For
                                            End If
                                        End If
                                    End With
                                Next
                            Next

                            If v_bolCheck = True Then
                                'Hiá»ƒn thá»‹ dá»¯ liá»‡u tá»« Lookup
                                FillLookupData(strFLDNAME, v_strFieldValue, v_strFULLDATA)
                            Else
                                'ThÃ´ng bÃ¡o lá»—i
                                Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Visible = True
                                Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Text = mv_ResourceManager.GetString("ERR_LOOKUP_VALUE")
                                Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).ForeColor = System.Drawing.Color.Red
                                Exit Sub
                            End If
                        End If
                    End If
                    End If
                    '----------------------------------

                    'TrÆ°á»?ng Inventory
                    If Len(mv_arrObjFields(v_intIndex).InvName) > 0 Then
                        v_strFldSource = mv_arrObjFields(v_intIndex).FldSource
                        v_strFldDesc = mv_arrObjFields(v_intIndex).FldDesc
                        'Them
                        Dim v_ctrl As Control, v_ctlmskData As FlexMaskEditBox

                        For Each v_ctrl In Me.pnTransDetail.Controls
                            If TypeOf (v_ctrl) Is FlexMaskEditBox Then
                                v_ctlmskData = CType(v_ctrl, FlexMaskEditBox)
                                v_strFLDNAME = Mid(v_ctlmskData.Name, Len(PREFIXED_MSKDATA) + 1)
                                If Trim(v_strFldDesc) = Trim(v_strFLDNAME) Then
                                    'v_strTemp = Strings.Right(gc_FORMAT_ODAUTOID & v_strValue, CType(v_ctrl, FlexMaskEditBox).MaxLength - Len(v_strINVReturn))
                                    If Len(CType(v_ctrl, FlexMaskEditBox).Text) = 0 Then
                                        GetInventory(v_strFldSource, v_strFldDesc, mv_arrObjFields(v_intIndex).InvName, mv_arrObjFields(v_intIndex).InvFormat)
                                        Exit For
                                    End If
                                End If
                            End If
                        Next
                    End If
                    ' load lai cac combox co #
                    v_intCount = mv_arrObjFields.GetLength(0) - 1
                    For v_intSharp1 = 0 To v_intCount - 1 Step 1
                        If Trim(mv_arrObjFields(v_intSharp1).ControlType) = "C" And v_intSharp1 <> v_intIndex Then
                            v_strSQLCMD = mv_arrObjFields(v_intSharp1).LookupList
                            If InStr(v_strSQLCMD, "#") > 0 Then
                                For v_intSharp = 0 To v_intCount - 1 Step 1
                                    v_strSharp = ""
                                    Select Case Trim(mv_arrObjFields(v_intSharp).ControlType)
                                        Case "T"
                                            v_strSharp = CType(Me.pnTransDetail.Controls(mv_arrObjFields(v_intSharp).ControlIndex), TextBox).Text
                                        Case "M"
                                            v_strSharp = CType(Me.pnTransDetail.Controls(mv_arrObjFields(v_intSharp).ControlIndex), FlexMaskEditBox).Text
                                            v_strSharp = IIf(Trim(mv_arrObjFields(v_intSharp).DataType) = "D", v_strSharp.Replace("/  /", ""), v_strSharp)
                                        Case "C"
                                            v_strSharp = CType(Me.pnTransDetail.Controls(mv_arrObjFields(v_intSharp).ControlIndex), ComboBoxEx).SelectedValue.ToString
                                    End Select
                                    v_strSQLCMD = v_strSQLCMD.Replace("#" & Format(v_intSharp + 1, "00"), v_strSharp.Trim)
                                Next
                                If mv_arrObjFields(v_intSharp1).MemberField <> "" And mv_lngAllMember = 0 Then
                                    v_strSQLCMD = "select * from (" & v_strSQLCMD & ") where " & mv_arrObjFields(v_intSharp1).MemberField & " in " & mv_strMemberFilter
                                End If
                                If mv_arrObjFields(v_intSharp1).StockField <> "" And mv_lngAllStock = 0 Then
                                    v_strSQLCMD = "select * from (" & v_strSQLCMD & ") where " & mv_arrObjFields(v_intSharp1).StockField & " in " & mv_strStockFilter
                                End If

                                v_strObjMsg = BuildXMLObjMsg(, BranchId, , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_LOOKUP, gc_ActionInquiry, v_strSQLCMD)

                            v_lngError = mv_BDSDelivery.Message(v_strObjMsg)
                            If v_lngError <> ERR_SYSTEM_OK Then
                                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                                Exit Sub
                            End If
                            v_cboData = CType(Me.pnTransDetail.Controls(mv_arrObjFields(v_intSharp1).ControlIndex), ComboBoxEx)
                            FillComboEx(v_strObjMsg, v_cboData)
                        End If
                    End If
                Next
            End If


                'Dim v_xmlATXDocument As New Xml.XmlDocument
                'ExecFldval(v_xmlATXDocument)
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                                     & "Error code: System error!" & vbNewLine _
                                                     & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
            mv_bolCheck = False
        End Try
    End Sub

    Private Sub ExecFldval(ByVal v_xmlDocument As Xml.XmlDocument)
        Dim v_intCount, v_intIndex, i As Integer
        Dim v_strVALEXP, v_strVALEXP2 As String
        Dim v_dtVALEXP, v_dtVALEXP2 As Date
        Dim v_objEval As New Evaluator

        'Duyá»‡t máº£ng dá»¯ liá»‡u danh má»¥c cÃ¡c Ä‘iá»?u kiá»‡n kiá»ƒm tra
        v_intCount = mv_arrObjFldVals.GetLength(0)
        If v_intCount > 0 Then
            For i = 0 To v_intCount - 1 Step 1
                If Not mv_arrObjFldVals(i) Is Nothing Then
                    'Xá»­ lÃ½ theo tham sá»‘ Ä‘Ã£ cÃ i Ä‘áº·t
                    With mv_arrObjFldVals(i)
                        'XÃ¡c Ä‘inh control index
                        v_intIndex = mv_arrObjFields(.IDXFLD).ControlIndex
                        'Thá»±c hiá»‡n xá»­ lÃ½ cho tá»«ng phÃ©p toÃ¡n
                        If .VALTYPE = "E" Then
                            'Náº¿u trÆ°á»?ng cÃ³ kiá»ƒu dá»¯ liá»‡u lÃ  sá»‘
                            If mv_arrObjFields(.IDXFLD).DataType <> "D" Then
                                Select Case .mp_OPERATOR
                                    Case "EX"
                                        v_strVALEXP = BuildAMTEXP(.VALEXP)
                                        If mv_arrObjFields(.IDXFLD).DataType = "N" Then
                                            Me.pnTransDetail.Controls(v_intIndex).Text = Math.Round(v_objEval.Eval(v_strVALEXP), 0)
                                            FormatNumericTextbox(CType(Me.pnTransDetail.Controls(v_intIndex), TextBox))
                                        Else
                                            Me.pnTransDetail.Controls(v_intIndex).Text = v_objEval.Eval(v_strVALEXP)
                                        End If
                                    Case "MA"
                                        v_strVALEXP = BuildAMTEXP(.VALEXP)
                                        v_strVALEXP2 = BuildAMTEXP(.VALEXP2)
                                        If mv_arrObjFields(.IDXFLD).DataType = "N" Then
                                            Me.pnTransDetail.Controls(v_intIndex).Text = Math.Round(GetMax(v_objEval.Eval(v_strVALEXP), v_objEval.Eval(v_strVALEXP2)), 0)
                                            FormatNumericTextbox(CType(Me.pnTransDetail.Controls(v_intIndex), TextBox))
                                        Else
                                            Me.pnTransDetail.Controls(v_intIndex).Text = GetMax(v_objEval.Eval(v_strVALEXP), v_objEval.Eval(v_strVALEXP2)).ToString
                                        End If
                                        'Me.pnTransDetail.Controls(v_intIndex).Text = GetMax(v_objEval.Eval(v_strVALEXP), v_objEval.Eval(v_strVALEXP2)).ToString
                                End Select
                                'Náº¿u trÆ°á»?ng cÃ³ kiá»ƒu dá»¯ liá»‡u lÃ  ngÃ y thÃ¡ng
                            ElseIf mv_arrObjFields(.IDXFLD).DataType = "D" Then
                                Select Case .mp_OPERATOR
                                    Case "DF"
                                        If .VALEXP = "00" Then
                                            v_dtVALEXP = DDMMYYYY_SystemDate(Me.pnTransDetail.Controls(0).Text)
                                        End If
                                        v_dtVALEXP2 = DDMMYYYY_SystemDate(BuildAMTEXP(.VALEXP2))
                                        Me.pnTransDetail.Controls(v_intIndex).Text = DateDiff(DateInterval.Day, v_dtVALEXP, v_dtVALEXP2).ToString
                                End Select
                            End If
                        End If
                    End With
                End If
            Next
        End If
    End Sub

    Public Sub GetInventory(ByVal v_strFldSource As String, ByVal v_strFldDesc As String, ByVal v_strInvName As String, ByVal v_strInvFormat As String)
        Try
            Dim v_strModule As String = "", v_strValue As String = "", v_strFldName As String = ""
            Dim v_ctlmskData As FlexMaskEditBox, v_ctrl As Control
            Dim v_strTemp, v_strINVReturn As String, v_intPos As Integer
            v_strModule = Mid(v_strFldSource, 1, 2)
            v_strFldSource = Mid(v_strFldSource, 3)

            If Not mv_arrObjFields Is Nothing Then
                'XÃ¡c Ä‘á»‹nh giÃ¡ trá»‹ cá»§a cÃ¡c trÆ°á»?ng dÃ¹ng Ä‘á»ƒ gá»™p vá»›i Inventory Name
                For Each v_ctrl In Me.pnTransDetail.Controls
                    If TypeOf (v_ctrl) Is FlexMaskEditBox Then
                        v_ctlmskData = CType(v_ctrl, FlexMaskEditBox)
                        v_strFldName = Mid(v_ctlmskData.Name, Len(PREFIXED_MSKDATA) + 1)
                        If InStr(v_strFldSource, v_strFldName & "@") > 0 Then
                            If Len(v_ctlmskData.Text) > 0 Then
                                v_strValue = v_strValue & v_ctlmskData.Text
                            Else
                                'Náº¿u chÆ°a cÃ³ giÃ¡ trá»‹ thÃ¬ khÃ´ng táº¡o Inventory ná»¯a
                                Exit Sub
                            End If

                        End If
                    End If
                Next

                'Náº¿u cÃ³ láº¥y Inventory thÃ¬ pháº£i xÃ¡c Ä‘á»‹nh giÃ¡ trá»‹ cá»§a trÆ°á»?ng nguá»“n
                If Len(v_strInvName) > 0 Then
                    'Cáº¥u trÃºc cá»§a 01 inventoy lÃ : PhÃ¢n há»‡ + MÃ£ chi nhÃ¡nh + TÃªn Inventory + GiÃ¡ trá»‹ trÆ°á»?ng nguá»“n
                    v_strValue = v_strModule & Me.BranchId & v_strInvName & v_strValue
                    'v_strValue = v_strModule & Me.BranchId & v_strInvName

                    'Gá»?i hÃ m Ä‘á»ƒ xÃ¡c Ä‘á»‹nh Inventory
                    Dim v_strClause, v_strObjMsg As String
                    Dim v_xmlDocument As New Xml.XmlDocument
                    'Dim v_ws As New BDSDelivery.BDSDelivery
                    'Láº¥y thÃ´ng tin chung vá»? giao dá»‹ch
                    v_strClause = v_strValue
                    v_strObjMsg = BuildXMLObjMsg(, , , , gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SY_AUTHENTICATION, gc_ActionInquiry, , v_strClause, "GetInventory")

                    mv_BDSDelivery.Message(v_strObjMsg)

                    'Láº¥y giÃ¡ trá»‹ tráº£ vá»? (tráº£ táº¡i má»‡nh Ä‘á»? Clause luÃ´n)
                    v_xmlDocument.LoadXml(v_strObjMsg)
                    Dim v_attrColl As Xml.XmlAttributeCollection = v_xmlDocument.DocumentElement.Attributes
                    v_strValue = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCLAUSE), Xml.XmlAttribute).Value)

                    'Táº¡o theo cáº¥u trÃºc InvFormat
                    v_intPos = 1
                    v_strINVReturn = String.Empty
                    While v_intPos < Len(v_strInvFormat)
                        v_strTemp = Mid(v_strInvFormat, v_intPos, 2)
                        If v_strTemp = "BR" Then
                            v_strINVReturn = v_strINVReturn & Me.BranchId
                        Else
                            'Láº¥y giÃ¡ trá»‹ hiá»‡n táº¡i cá»§a trÆ°á»?ng nÃ o Ä‘Ã³
                            For Each v_ctrl In Me.pnTransDetail.Controls
                                If TypeOf (v_ctrl) Is FlexMaskEditBox Then
                                    v_ctlmskData = CType(v_ctrl, FlexMaskEditBox)
                                    v_strFldName = Mid(v_ctlmskData.Name, Len(PREFIXED_MSKDATA) + 1)
                                    If Trim(v_strTemp) = Trim(v_strFldName) Then
                                        v_strINVReturn = v_strINVReturn & CType(v_ctrl, FlexMaskEditBox).Text
                                        Exit For
                                    End If
                                End If
                            Next
                        End If
                        v_intPos = v_intPos + 2
                    End While

                    'Ä?áº·t Inventory cho trÆ°á»?ng Ä‘Ã­ch: 
                    For Each v_ctrl In Me.pnTransDetail.Controls
                        If TypeOf (v_ctrl) Is FlexMaskEditBox Then
                            v_ctlmskData = CType(v_ctrl, FlexMaskEditBox)
                            v_strFldName = Mid(v_ctlmskData.Name, Len(PREFIXED_MSKDATA) + 1)
                            If Trim(v_strFldDesc) = Trim(v_strFldName) Then
                                v_strTemp = Strings.Right(gc_FORMAT_ODAUTOID & v_strValue, CType(v_ctrl, FlexMaskEditBox).MaxLength - Len(v_strINVReturn))
                                CType(v_ctrl, FlexMaskEditBox).Text = v_strINVReturn & v_strTemp
                                Exit Sub
                            End If
                        End If
                    Next
                End If
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        OnClose()
    End Sub

    Private Sub LoadView(ByVal strViewSQL As String)
        Dim v_xmlDocument As New System.Xml.XmlDocument
        Dim v_nodeList As Xml.XmlNodeList
        Dim v_strFLDNAME, v_strFLDCD, v_strTXDESC As String
        Dim v_strValue, v_strObjMsg As String
        Dim ctl As Control
        Dim lngWidth As Long = 0

        v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SY_SEARCHFLD, gc_ActionInquiry, strViewSQL, , )
        Dim v_lngError As Long = mv_BDSDelivery.Message(v_strObjMsg)
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
                    v_strValue = .InnerText.ToString
                    Select Case Trim(v_strFLDNAME)
                        Case "FLDCD"
                            v_strFLDCD = Trim(v_strValue)
                        Case "TXDESC"
                            v_strTXDESC = Trim(v_strValue)
                    End Select
                End With
            Next
            ctl = Me.pnTransDetail.Controls(mv_arrObjFields(i).ControlIndex)
            If lngWidth < ctl.Left + ctl.Width Then
                lngWidth = ctl.Left + ctl.Width
            End If
            ctl.Text = v_strTXDESC
            ctl.Enabled = False
        Next
        Me.btnCancel.Text = "Thoát"
        Me.Width = lngWidth + CONTROL_TOP * 2
        Me.mskTransCode.Enabled = False
    End Sub

    Private Function CheckFldvals(ByVal arrObjFldVals() As CFieldVal, Optional ByRef arrValue As Array = Nothing, Optional ByRef strErrMsg As String = Nothing, Optional ByVal intRealRow As Integer = -1) As Boolean
        Dim v_intCount, v_intIndex As Integer
        Dim v_strVALEXP, v_strVALEXP2, v_strFLDVALUE As String
        Dim v_dtFLDVALUE, v_dtVALEXP, v_dtVALEXP2 As Date
        Dim v_objEval As New Evaluator
        Dim strTmp As String = ""
        Try
            If arrValue Is Nothing Then
                'Duyệt mảng dữ liệu đánh mốc các điều kiện kiểm tra
                v_intCount = arrObjFldVals.GetLength(0)
                If v_intCount > 0 Then
                    For i = 0 To v_intCount - 1 Step 1
                        If Not arrObjFldVals(i) Is Nothing Then
                            'Xác lập theo tham số đã cài đặt
                            With arrObjFldVals(i)
                                'Xác định index
                                v_intIndex = mv_arrObjFields(.IDXFLD).ControlIndex
                                'Thực hiện xác lập cho từng phép toán
                                If .VALTYPE = "E" Then
                                    'Nếu trường dữ liệu có kiểu là số
                                    If mv_arrObjFields(.IDXFLD).DataType <> "D" Then
                                        Select Case .mp_OPERATOR
                                            Case "MA"
                                                v_strVALEXP = BuildAMTEXP(.VALEXP)
                                                v_strVALEXP2 = BuildAMTEXP(.VALEXP2)
                                                Me.pnTransDetail.Controls(v_intIndex).Text = GetMax(v_objEval.Eval(v_strVALEXP), v_objEval.Eval(v_strVALEXP2)).ToString
                                            Case "MI"
                                                v_strVALEXP = BuildAMTEXP(.VALEXP)
                                                v_strVALEXP2 = BuildAMTEXP(.VALEXP2)
                                                Me.pnTransDetail.Controls(v_intIndex).Text = GetMin(v_objEval.Eval(v_strVALEXP), v_objEval.Eval(v_strVALEXP2)).ToString
                                            Case "&&"
                                                v_strVALEXP = BuildCONCAT(.VALEXP)
                                                Me.pnTransDetail.Controls(v_intIndex).Text = v_strVALEXP
                                            Case "EX"
                                                v_strVALEXP = BuildAMTEXP(.VALEXP)
                                                If mv_arrObjFields(.IDXFLD).DataType = "N" Then
                                                    Me.pnTransDetail.Controls(v_intIndex).Text = Math.Round(v_objEval.Eval(v_strVALEXP), 0)
                                                Else
                                                    Me.pnTransDetail.Controls(v_intIndex).Text = v_objEval.Eval(v_strVALEXP)
                                                End If
                                        End Select
                                        'Nếu trường dữ liệu có kiểu là ngày tháng
                                    ElseIf mv_arrObjFields(.IDXFLD).DataType = "D" Then
                                        Select Case .mp_OPERATOR
                                            Case "MA"
                                                v_dtVALEXP = DDMMYYYY_SystemDate(BuildAMTEXP(.VALEXP))
                                                v_dtVALEXP2 = DDMMYYYY_SystemDate(BuildAMTEXP(.VALEXP2))
                                                If v_dtVALEXP > v_dtVALEXP2 Then
                                                    Me.pnTransDetail.Controls(v_intIndex).Text = v_dtVALEXP
                                                Else
                                                    Me.pnTransDetail.Controls(v_intIndex).Text = v_dtVALEXP2
                                                End If
                                            Case "MI"
                                                v_dtVALEXP = DDMMYYYY_SystemDate(BuildAMTEXP(.VALEXP))
                                                v_dtVALEXP2 = DDMMYYYY_SystemDate(BuildAMTEXP(.VALEXP2))
                                                If v_dtVALEXP > v_dtVALEXP2 Then
                                                    Me.pnTransDetail.Controls(v_intIndex).Text = v_dtVALEXP2
                                                Else
                                                    Me.pnTransDetail.Controls(v_intIndex).Text = v_dtVALEXP
                                                End If
                                        End Select
                                    End If

                                ElseIf .VALTYPE = "V" Then
                                    If TypeOf (Me.pnTransDetail.Controls(v_intIndex)) Is ComboBoxEx Then
                                        v_strFLDVALUE = CType(Me.pnTransDetail.Controls(v_intIndex), ComboBoxEx).SelectedValue
                                    Else
                                        v_strFLDVALUE = Me.pnTransDetail.Controls(v_intIndex).Text
                                    End If

                                    If mv_arrObjFields(.IDXFLD).DataType <> "D" Then
                                        If Not gf_Numberic(v_strFLDVALUE) Then
                                            v_oProcess.StopProcessForm()
                                            MessageBox.Show(mv_ResourceManager.GetString("ERR_INVALID_NUMERIC_NUMBER"))
                                            mv_bolCheck = False
                                            Return False
                                        End If
                                        v_strFLDVALUE = CDbl(v_strFLDVALUE).ToString
                                        Select Case .mp_OPERATOR
                                            Case ">>"
                                                v_strVALEXP = BuildAMTEXP(.VALEXP)
                                                If Not CDbl(v_strFLDVALUE) > v_objEval.Eval(v_strVALEXP) Then
                                                    If Me.UserLanguage = "EN" Then
                                                        v_oProcess.StopProcessForm()
                                                        MsgBox(.EN_ERRMSG)

                                                    Else
                                                        v_oProcess.StopProcessForm()
                                                        MsgBox(.ERRMSG)

                                                    End If
                                                    mv_bolCheck = False
                                                    Return False
                                                End If
                                            Case ">="
                                                v_strVALEXP = BuildAMTEXP(.VALEXP)
                                                If Not CDbl(v_strFLDVALUE) >= v_objEval.Eval(v_strVALEXP) Then
                                                    If Me.UserLanguage = "EN" Then
                                                        v_oProcess.StopProcessForm()
                                                        MsgBox(.EN_ERRMSG)
                                                    Else
                                                        v_oProcess.StopProcessForm()
                                                        MsgBox(.ERRMSG)
                                                    End If
                                                    mv_bolCheck = False
                                                    Return False
                                                End If
                                            Case "<<"
                                                v_strVALEXP = BuildAMTEXP(.VALEXP)
                                                If Not CDbl(v_strFLDVALUE) < v_objEval.Eval(v_strVALEXP) Then
                                                    If Me.UserLanguage = "EN" Then
                                                        v_oProcess.StopProcessForm()
                                                        MsgBox(.EN_ERRMSG)
                                                    Else
                                                        v_oProcess.StopProcessForm()
                                                        MsgBox(.ERRMSG)
                                                    End If
                                                    mv_bolCheck = False
                                                    Return False
                                                End If
                                            Case "<="
                                                v_strVALEXP = BuildAMTEXP(.VALEXP)
                                                If Not CDbl(v_strFLDVALUE) <= v_objEval.Eval(v_strVALEXP) Then
                                                    If Me.UserLanguage = "EN" Then
                                                        v_oProcess.StopProcessForm()
                                                        MsgBox(.EN_ERRMSG)
                                                    Else
                                                        v_oProcess.StopProcessForm()
                                                        MsgBox(.ERRMSG)
                                                    End If
                                                    mv_bolCheck = False
                                                    Return False
                                                End If
                                            Case "=="
                                                v_strVALEXP = BuildAMTEXP(.VALEXP)
                                                If Not CDbl(v_strFLDVALUE) = v_objEval.Eval(v_strVALEXP) Then
                                                    If Me.UserLanguage = "EN" Then
                                                        v_oProcess.StopProcessForm()
                                                        MsgBox(.EN_ERRMSG)
                                                    Else
                                                        v_oProcess.StopProcessForm()
                                                        MsgBox(.ERRMSG)
                                                    End If
                                                    mv_bolCheck = False
                                                    Return False
                                                End If
                                            Case "<>"
                                                v_strVALEXP = BuildAMTEXP(.VALEXP)
                                                If Not CDbl(v_strFLDVALUE) <> v_objEval.Eval(v_strVALEXP) Then
                                                    If Me.UserLanguage = "EN" Then
                                                        v_oProcess.StopProcessForm()
                                                        MsgBox(.EN_ERRMSG)
                                                    Else
                                                        v_oProcess.StopProcessForm()
                                                        MsgBox(.ERRMSG)
                                                    End If
                                                    mv_bolCheck = False
                                                    Return False
                                                End If
                                            Case "IN"
                                            Case "NI"
                                        End Select
                                    ElseIf mv_arrObjFields(.IDXFLD).DataType = "D" Then
                                        If Not IsDateValue(v_strFLDVALUE) Then
                                            v_oProcess.StopProcessForm()
                                            MessageBox.Show(mv_ResourceManager.GetString("ERR_DATE_VALUE"), gc_ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Information)
                                            mv_bolCheck = False
                                            Return False
                                        Else
                                            v_dtFLDVALUE = DDMMYYYY_SystemDate(v_strFLDVALUE)
                                        End If
                                        Select Case .mp_OPERATOR
                                            Case ">>"
                                                v_dtVALEXP = DDMMYYYY_SystemDate(BuildAMTEXP(.VALEXP))
                                                If Not v_dtFLDVALUE > v_dtVALEXP Then
                                                    If Me.UserLanguage = "EN" Then
                                                        v_oProcess.StopProcessForm()
                                                        MsgBox(.EN_ERRMSG)
                                                    Else
                                                        v_oProcess.StopProcessForm()
                                                        MsgBox(.ERRMSG)
                                                    End If
                                                    mv_bolCheck = False
                                                    Return False
                                                End If
                                            Case ">="
                                                v_dtVALEXP = DDMMYYYY_SystemDate(BuildAMTEXP(.VALEXP))
                                                If v_dtFLDVALUE >= v_dtVALEXP Then
                                                    If Me.UserLanguage = "EN" Then
                                                        v_oProcess.StopProcessForm()
                                                        MsgBox(.EN_ERRMSG)
                                                    Else
                                                        v_oProcess.StopProcessForm()
                                                        MsgBox(.ERRMSG)
                                                    End If
                                                    mv_bolCheck = False
                                                    Return False
                                                End If
                                            Case "<<"
                                                v_dtVALEXP = DDMMYYYY_SystemDate(BuildAMTEXP(.VALEXP))
                                                If Not v_dtFLDVALUE < v_dtVALEXP Then
                                                    If Me.UserLanguage = "EN" Then
                                                        v_oProcess.StopProcessForm()
                                                        MsgBox(.EN_ERRMSG)
                                                    Else
                                                        v_oProcess.StopProcessForm()
                                                        MsgBox(.ERRMSG)
                                                    End If
                                                    mv_bolCheck = False
                                                    Return False
                                                End If
                                            Case "<="
                                                v_dtVALEXP = DDMMYYYY_SystemDate(BuildAMTEXP(.VALEXP))
                                                If Not v_dtFLDVALUE <= v_dtVALEXP Then
                                                    If Me.UserLanguage = "EN" Then
                                                        v_oProcess.StopProcessForm()
                                                        MsgBox(.EN_ERRMSG)
                                                    Else
                                                        v_oProcess.StopProcessForm()
                                                        MsgBox(.ERRMSG)
                                                    End If
                                                    mv_bolCheck = False
                                                    Return False
                                                End If
                                            Case "=="
                                                v_dtVALEXP = DDMMYYYY_SystemDate(BuildAMTEXP(.VALEXP))
                                                If Not v_dtFLDVALUE = v_dtVALEXP Then
                                                    If Me.UserLanguage = "EN" Then
                                                        v_oProcess.StopProcessForm()
                                                        MsgBox(.EN_ERRMSG)
                                                    Else
                                                        v_oProcess.StopProcessForm()
                                                        MsgBox(.ERRMSG)
                                                    End If
                                                    mv_bolCheck = False
                                                    Return False
                                                End If
                                            Case "<>"
                                                v_dtVALEXP = DDMMYYYY_SystemDate(BuildAMTEXP(.VALEXP))
                                                If Not v_dtFLDVALUE <> v_dtVALEXP Then
                                                    If Me.UserLanguage = "EN" Then
                                                        v_oProcess.StopProcessForm()
                                                        MsgBox(.EN_ERRMSG)
                                                    Else
                                                        v_oProcess.StopProcessForm()
                                                        MsgBox(.ERRMSG)
                                                    End If
                                                    mv_bolCheck = False
                                                    Return False
                                                End If
                                            Case "IN"
                                            Case "NI"
                                        End Select
                                    End If
                                End If
                            End With
                        End If
                    Next
                End If
            Else
                Dim intVIndext, intVCount As Integer
                intVCount = arrValue.GetLength(0)
                v_intCount = arrObjFldVals.GetLength(0)
                If v_intCount > 0 Then
                    For intVIndext = 1 To intVCount
                        'Duyệt mảng dữ liệu đánh mốc các điều kiện kiểm tra
                        strTmp = ""
                        For i = 0 To v_intCount - 1 Step 1
                            If Not arrObjFldVals(i) Is Nothing Then
                                'Xác lập theo tham số đã cài đặt
                                With arrObjFldVals(i)
                                    'Xác định index
                                    v_intIndex = .IDXFLD + 1
                                    'Thực hiện xác lập cho từng phép toán
                                    If .VALTYPE = "E" Then
                                        'Nếu trường dữ liệu có kiểu là số
                                        If mv_arrObjCoFields(.IDXFLD).DataType <> "D" Then
                                            Select Case .mp_OPERATOR
                                                Case "MA"
                                                    v_strVALEXP = BuildAMTEXP(.VALEXP, arrValue, intVIndext)
                                                    v_strVALEXP2 = BuildAMTEXP(.VALEXP2, arrValue, intVIndext)
                                                    arrValue(intVIndext, v_intIndex) = GetMax(v_objEval.Eval(v_strVALEXP), v_objEval.Eval(v_strVALEXP2)).ToString
                                                Case "MI"
                                                    v_strVALEXP = BuildAMTEXP(.VALEXP, arrValue, intVIndext)
                                                    v_strVALEXP2 = BuildAMTEXP(.VALEXP2, arrValue, intVIndext)
                                                    arrValue(intVIndext, v_intIndex) = GetMin(v_objEval.Eval(v_strVALEXP), v_objEval.Eval(v_strVALEXP2)).ToString
                                                Case "&&"
                                                    v_strVALEXP = BuildCONCAT(.VALEXP, arrValue, intVIndext)
                                                    arrValue(intVIndext, v_intIndex) = v_strVALEXP
                                                Case "EX"
                                                    v_strVALEXP = BuildAMTEXP(.VALEXP, arrValue, intVIndext)
                                                    If mv_arrObjCoFields(.IDXFLD).DataType = "N" Then
                                                        arrValue(intVIndext, v_intIndex) = Math.Round(v_objEval.Eval(v_strVALEXP), 0)
                                                    Else
                                                        arrValue(intVIndext, v_intIndex) = v_objEval.Eval(v_strVALEXP)
                                                    End If
                                            End Select
                                            'Nếu trường dữ liệu có kiểu là ngày tháng
                                        ElseIf mv_arrObjCoFields(.IDXFLD).DataType = "D" Then
                                            Select Case .mp_OPERATOR
                                                Case "MA"
                                                    v_dtVALEXP = DDMMYYYY_SystemDate(BuildAMTEXP(.VALEXP, arrValue, intVIndext))
                                                    v_dtVALEXP2 = DDMMYYYY_SystemDate(BuildAMTEXP(.VALEXP2, arrValue, intVIndext))
                                                    If v_dtVALEXP > v_dtVALEXP2 Then
                                                        arrValue(intVIndext, v_intIndex) = v_dtVALEXP
                                                    Else
                                                        arrValue(intVIndext, v_intIndex) = v_dtVALEXP2
                                                    End If
                                                Case "MI"
                                                    v_dtVALEXP = DDMMYYYY_SystemDate(BuildAMTEXP(.VALEXP, arrValue, intVIndext))
                                                    v_dtVALEXP2 = DDMMYYYY_SystemDate(BuildAMTEXP(.VALEXP2, arrValue, intVIndext))
                                                    If v_dtVALEXP > v_dtVALEXP2 Then
                                                        arrValue(intVIndext, v_intIndex) = v_dtVALEXP2
                                                    Else
                                                        arrValue(intVIndext, v_intIndex) = v_dtVALEXP
                                                    End If
                                            End Select
                                        End If

                                    ElseIf .VALTYPE = "V" Then
                                        v_strFLDVALUE = arrValue(intVIndext, v_intIndex)
                                        If mv_arrObjCoFields(.IDXFLD).DataType <> "D" Then
                                            If Not gf_Numberic(v_strFLDVALUE) Then
                                                strTmp = strTmp & "Cột """ & mv_arrObjCoFields(.IDXFLD).Caption.Replace(":", "") & """" & " của dòng " & intVIndext + intRealRow & " : " & mv_ResourceManager.GetString("ERR_INVALID_NUMERIC_NUMBER") & vbCrLf
                                            End If
                                            v_strFLDVALUE = CDbl(v_strFLDVALUE).ToString
                                            Select Case .mp_OPERATOR
                                                Case ">>"
                                                    v_strVALEXP = BuildAMTEXP(.VALEXP, arrValue, intVIndext)
                                                    If Not CDbl(v_strFLDVALUE) > v_objEval.Eval(v_strVALEXP) Then
                                                        If Me.UserLanguage = "EN" Then
                                                            strTmp = strTmp & "Cột """ & mv_arrObjCoFields(.IDXFLD).Caption.Replace(":", "") & """" & " của dòng " & intVIndext + intRealRow & " : " & .EN_ERRMSG & vbCrLf
                                                            mv_intErrCount = mv_intErrCount + 1
                                                        Else
                                                            strTmp = strTmp & "Cột """ & mv_arrObjCoFields(.IDXFLD).Caption.Replace(":", "") & """" & " của dòng " & intVIndext + intRealRow & " : " & .ERRMSG & vbCrLf
                                                            mv_intErrCount = mv_intErrCount + 1
                                                        End If
                                                    End If
                                                Case ">="
                                                    v_strVALEXP = BuildAMTEXP(.VALEXP, arrValue, intVIndext)
                                                    If Not CDbl(v_strFLDVALUE) >= v_objEval.Eval(v_strVALEXP) Then
                                                        If Me.UserLanguage = "EN" Then
                                                            strTmp = strTmp & "Cột """ & mv_arrObjCoFields(.IDXFLD).Caption.Replace(":", "") & """" & " của dòng " & intVIndext + intRealRow & " : " & .EN_ERRMSG & vbCrLf
                                                            mv_intErrCount = mv_intErrCount + 1
                                                        Else
                                                            strTmp = strTmp & "Cột """ & mv_arrObjCoFields(.IDXFLD).Caption.Replace(":", "") & """" & " của dòng " & intVIndext + intRealRow & " : " & .ERRMSG & vbCrLf
                                                            mv_intErrCount = mv_intErrCount + 1
                                                        End If
                                                    End If
                                                Case "<<"
                                                    v_strVALEXP = BuildAMTEXP(.VALEXP, arrValue, intVIndext)
                                                    If Not CDbl(v_strFLDVALUE) < v_objEval.Eval(v_strVALEXP) Then
                                                        If Me.UserLanguage = "EN" Then
                                                            strTmp = strTmp & "Cột """ & mv_arrObjCoFields(.IDXFLD).Caption.Replace(":", "") & """" & " của dòng " & intVIndext + intRealRow & " : " & .EN_ERRMSG & vbCrLf
                                                            mv_intErrCount = mv_intErrCount + 1
                                                        Else
                                                            strTmp = strTmp & "Cột """ & mv_arrObjCoFields(.IDXFLD).Caption.Replace(":", "") & """" & " của dòng " & intVIndext + intRealRow & " : " & .ERRMSG & vbCrLf
                                                            mv_intErrCount = mv_intErrCount + 1
                                                        End If
                                                    End If
                                                Case "<="
                                                    v_strVALEXP = BuildAMTEXP(.VALEXP, arrValue, intVIndext)
                                                    If Not CDbl(v_strFLDVALUE) <= v_objEval.Eval(v_strVALEXP) Then
                                                        If Me.UserLanguage = "EN" Then
                                                            strTmp = strTmp & "Cột """ & mv_arrObjCoFields(.IDXFLD).Caption.Replace(":", "") & """" & " của dòng " & intVIndext + intRealRow & " : " & .EN_ERRMSG & vbCrLf
                                                            mv_intErrCount = mv_intErrCount + 1
                                                        Else
                                                            strTmp = strTmp & "Cột """ & mv_arrObjCoFields(.IDXFLD).Caption.Replace(":", "") & """" & " của dòng " & intVIndext + intRealRow & " : " & .ERRMSG & vbCrLf
                                                            mv_intErrCount = mv_intErrCount + 1
                                                        End If
                                                    End If
                                                Case "=="
                                                    v_strVALEXP = BuildAMTEXP(.VALEXP, arrValue, intVIndext)
                                                    If Not CDbl(v_strFLDVALUE) = v_objEval.Eval(v_strVALEXP) Then
                                                        If Me.UserLanguage = "EN" Then
                                                            strTmp = strTmp & "Cột """ & mv_arrObjCoFields(.IDXFLD).Caption.Replace(":", "") & """" & " của dòng " & intVIndext + intRealRow & " : " & .EN_ERRMSG & vbCrLf
                                                            mv_intErrCount = mv_intErrCount + 1
                                                        Else
                                                            strTmp = strTmp & "Cột """ & mv_arrObjCoFields(.IDXFLD).Caption.Replace(":", "") & """" & " của dòng " & intVIndext + intRealRow & " : " & .ERRMSG & vbCrLf
                                                            mv_intErrCount = mv_intErrCount + 1
                                                        End If
                                                    End If
                                                Case "<>"
                                                    v_strVALEXP = BuildAMTEXP(.VALEXP, arrValue, intVIndext)
                                                    If Not CDbl(v_strFLDVALUE) <> v_objEval.Eval(v_strVALEXP) Then
                                                        If Me.UserLanguage = "EN" Then
                                                            strTmp = strTmp & "Cột """ & mv_arrObjCoFields(.IDXFLD).Caption.Replace(":", "") & """" & " của dòng " & intVIndext + intRealRow & " : " & .EN_ERRMSG & vbCrLf
                                                            mv_intErrCount = mv_intErrCount + 1
                                                        Else
                                                            strTmp = strTmp & "Cột """ & mv_arrObjCoFields(.IDXFLD).Caption.Replace(":", "") & """" & " của dòng " & intVIndext + intRealRow & " : " & .ERRMSG & vbCrLf
                                                            mv_intErrCount = mv_intErrCount + 1
                                                        End If
                                                    End If
                                                Case "IN"
                                                Case "NI"
                                            End Select
                                        ElseIf mv_arrObjCoFields(.IDXFLD).DataType = "D" Then
                                            If Not IsDateValue(v_strFLDVALUE) Then
                                                strTmp = strTmp & "Cột """ & mv_arrObjCoFields(.IDXFLD).Caption.Replace(":", "") & """" & " của dòng " & intVIndext + intRealRow & " : " & mv_ResourceManager.GetString("ERR_DATE_VALUE") & vbCrLf
                                                mv_intErrCount = mv_intErrCount + 1
                                            Else
                                                v_dtFLDVALUE = DDMMYYYY_SystemDate(v_strFLDVALUE)
                                            End If
                                            Select Case .mp_OPERATOR
                                                Case ">>"
                                                    v_dtVALEXP = DDMMYYYY_SystemDate(BuildAMTEXP(.VALEXP, arrValue, intVIndext))
                                                    If Not v_dtFLDVALUE > v_dtVALEXP Then
                                                        If Me.UserLanguage = "EN" Then
                                                            strTmp = strTmp & "Cột """ & mv_arrObjCoFields(.IDXFLD).Caption.Replace(":", "") & """" & " của dòng " & intVIndext + intRealRow & " : " & .EN_ERRMSG & vbCrLf
                                                            mv_intErrCount = mv_intErrCount + 1
                                                        Else
                                                            strTmp = strTmp & "Cột """ & mv_arrObjCoFields(.IDXFLD).Caption.Replace(":", "") & """" & " của dòng " & intVIndext + intRealRow & " : " & .ERRMSG & vbCrLf
                                                            mv_intErrCount = mv_intErrCount + 1
                                                        End If
                                                    End If
                                                Case ">="
                                                    v_dtVALEXP = DDMMYYYY_SystemDate(BuildAMTEXP(.VALEXP, arrValue, intVIndext))
                                                    If v_dtFLDVALUE >= v_dtVALEXP Then
                                                        If Me.UserLanguage = "EN" Then
                                                            strTmp = strTmp & "Cột """ & mv_arrObjCoFields(.IDXFLD).Caption.Replace(":", "") & """" & " của dòng " & intVIndext + intRealRow & " : " & .EN_ERRMSG & vbCrLf
                                                            mv_intErrCount = mv_intErrCount + 1
                                                        Else
                                                            strTmp = strTmp & "Cột """ & mv_arrObjCoFields(.IDXFLD).Caption.Replace(":", "") & """" & " của dòng " & intVIndext + intRealRow & " : " & .ERRMSG & vbCrLf
                                                            mv_intErrCount = mv_intErrCount + 1
                                                        End If
                                                    End If
                                                Case "<<"
                                                    v_dtVALEXP = DDMMYYYY_SystemDate(BuildAMTEXP(.VALEXP, arrValue, intVIndext))
                                                    If Not v_dtFLDVALUE < v_dtVALEXP Then
                                                        If Me.UserLanguage = "EN" Then
                                                            strTmp = strTmp & "Cột """ & mv_arrObjCoFields(.IDXFLD).Caption.Replace(":", "") & """" & " của dòng " & intVIndext + intRealRow & " : " & .EN_ERRMSG & vbCrLf
                                                            mv_intErrCount = mv_intErrCount + 1
                                                        Else
                                                            strTmp = strTmp & "Cột """ & mv_arrObjCoFields(.IDXFLD).Caption.Replace(":", "") & """" & " của dòng " & intVIndext + intRealRow & " : " & .ERRMSG & vbCrLf
                                                            mv_intErrCount = mv_intErrCount + 1
                                                        End If
                                                    End If
                                                Case "<="
                                                    v_dtVALEXP = DDMMYYYY_SystemDate(BuildAMTEXP(.VALEXP, arrValue, intVIndext))
                                                    If Not v_dtFLDVALUE <= v_dtVALEXP Then
                                                        If Me.UserLanguage = "EN" Then
                                                            strTmp = strTmp & "Cột """ & mv_arrObjCoFields(.IDXFLD).Caption.Replace(":", "") & """" & " của dòng " & intVIndext + intRealRow & " : " & .EN_ERRMSG & vbCrLf
                                                            mv_intErrCount = mv_intErrCount + 1
                                                        Else
                                                            strTmp = strTmp & "Cột """ & mv_arrObjCoFields(.IDXFLD).Caption.Replace(":", "") & """" & " của dòng " & intVIndext + intRealRow & " : " & .ERRMSG & vbCrLf
                                                            mv_intErrCount = mv_intErrCount + 1
                                                        End If
                                                    End If
                                                Case "=="
                                                    v_dtVALEXP = DDMMYYYY_SystemDate(BuildAMTEXP(.VALEXP, arrValue, intVIndext))
                                                    If Not v_dtFLDVALUE = v_dtVALEXP Then
                                                        If Me.UserLanguage = "EN" Then
                                                            strTmp = strTmp & "Cột """ & mv_arrObjCoFields(.IDXFLD).Caption.Replace(":", "") & """" & " của dòng " & intVIndext + intRealRow & " : " & .EN_ERRMSG & vbCrLf
                                                            mv_intErrCount = mv_intErrCount + 1
                                                        Else
                                                            strTmp = strTmp & "Cột """ & mv_arrObjCoFields(.IDXFLD).Caption.Replace(":", "") & """" & " của dòng " & intVIndext + intRealRow & " : " & .ERRMSG & vbCrLf
                                                            mv_intErrCount = mv_intErrCount + 1
                                                        End If
                                                    End If
                                                Case "<>"
                                                    v_dtVALEXP = DDMMYYYY_SystemDate(BuildAMTEXP(.VALEXP, arrValue, intVIndext))
                                                    If Not v_dtFLDVALUE <> v_dtVALEXP Then
                                                        If Me.UserLanguage = "EN" Then
                                                            strTmp = strTmp & "Cột """ & mv_arrObjCoFields(.IDXFLD).Caption.Replace(":", "") & """" & " của dòng " & intVIndext + intRealRow & " : " & .EN_ERRMSG & vbCrLf
                                                            mv_intErrCount = mv_intErrCount + 1
                                                        Else
                                                            strTmp = strTmp & "Cột """ & mv_arrObjCoFields(.IDXFLD).Caption.Replace(":", "") & """" & " của dòng " & intVIndext + intRealRow & " : " & .ERRMSG & vbCrLf
                                                            mv_intErrCount = mv_intErrCount + 1
                                                        End If
                                                    End If
                                                Case "IN"
                                                Case "NI"
                                            End Select
                                        End If
                                    End If
                                End With
                            End If
                        Next
                        If strTmp <> "" Then
                            strErrMsg = strErrMsg.Replace("@" & intVIndext + intRealRow & vbCrLf, strTmp)
                        End If
                    Next
                End If
            End If

            Return True

        Catch ex As Exception
            Throw ex
        End Try

    End Function

    'Public Function CheckTranLimit(ByVal strTltxcd As String, ByVal dblTranLimit As Double, ByRef v_dblTranLimit As Double, Optional ByRef v_strMSGAMT As String = "") As Boolean
    '    Dim v_lngErrCode As Long = ERR_SYSTEM_OK
    '    Dim v_strErrorSource As String = "AppCore.frmTransaction.CheckTrans"
    '    Dim v_strSQLCMD, v_strObjMsg, v_strValue, v_strFLDNAME As String
    '    Dim v_xmlObjDocument As New Xml.XmlDocument
    '    Dim v_nodeList As Xml.XmlNodeList
    '    Dim v_dblMinTranLimit As Double

    '    Try
    '        v_dblTranLimit = 0
    '        v_strSQLCMD = "SELECT max(b.tllimit) max_tllimit, min(b.tllimit) min_tllimit,  max(a.msg_amt) msg_amt FROM TLTX a, " _
    '                        & " ( " _
    '                        & " select c.tltxcd, c.tllimit from tlauth c where c.status =0 and c.deleted =0 and c.authtype = 'U' and c.authid = '" & mv_strTellerId & "' and c.tltype = '0' " _
    '                        & " union" _
    '                        & " select c.tltxcd, c.tllimit from tlauth c where c.status =0 and c.deleted =0 and c.authtype = 'G'" _
    '                        & " and c.authid in ( select grpid from tlgrpusers d where d.tlid = '" & mv_strTellerId & "' and d.status =0 and d.deleted =0 ) and c.tltype = '0'" _
    '                        & " ) b, appmodules c " _
    '                        & " where a.status =0 and a.deleted =0 and a.tltxcd = b.tltxcd and substr(a.tltxcd,1,2) = c.txcode and c.deleted=0 and c.status = 0" _
    '                        & " and a.tltxcd = '" & strTltxcd & "'"

    '        v_strObjMsg = BuildXMLObjMsg(, , , , gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_LOOKUP, gc_ActionInquiry, v_strSQLCMD)
    '        mv_BDSDelivery.Message(v_strObjMsg)
    '        v_xmlObjDocument.LoadXml(v_strObjMsg)
    '        v_nodeList = v_xmlObjDocument.SelectNodes("/ObjectMessage/ObjData")
    '        'v_xmlDocument.SelectSingleNode("TransactMessage/fields/entry[@fldname='" & mv_strFileName & "']").InnerText = v_strXML
    '        If v_nodeList.Count > 0 Then
    '            For i = 0 To v_nodeList.Count - 1
    '                For j As Integer = 0 To v_nodeList.Item(i).ChildNodes.Count - 1
    '                    With v_nodeList.Item(i).ChildNodes(j)
    '                        v_strValue = Trim(.InnerText.ToString)
    '                        v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
    '                        Select Case Trim(v_strFLDNAME)
    '                            Case "MAX_TLLIMIT"
    '                                v_dblTranLimit = CDbl(v_strValue)
    '                            Case "MIN_TLLIMIT"
    '                                v_dblMinTranLimit = CDbl(v_strValue)
    '                            Case "MSG_AMT"
    '                                v_strMSGAMT = v_strValue
    '                        End Select
    '                    End With
    '                Next
    '            Next
    '        Else
    '            v_thread.Abort()
    '            MsgBox("Bạn không còn quyền thực hiện giao dịch này", MsgBoxStyle.Information, "Thông báo")
    '            mv_bolCheck = False
    '            Return False
    '        End If
    '        If v_dblMinTranLimit <> 0 Then
    '            If v_dblTranLimit < dblTranLimit Then
    '                v_thread.Abort()
    '                MsgBox("Bạn chỉ có thể thực hiện giao dịch với số lượng nhỏ hơn hoặc bằng " & v_dblTranLimit, MsgBoxStyle.Information, "Thông báo")
    '                mv_bolCheck = False
    '                Return False
    '            End If
    '        Else
    '            v_dblTranLimit = v_dblMinTranLimit
    '        End If
    '        'ContextUtil.SetComplete()
    '        Return True
    '    Catch ex As Exception
    '        'ContextUtil.SetAbort()
    '        Throw ex
    '    End Try

    'End Function

    'Private Sub ShowProcessForm()
    '    Dim v_frm As New frmProcess
    '    'v_frm.Caption = "Dữ liệu đang được xử lý ..."
    '    v_frm.lbl.Text = "Dữ liệu đang được xử lý ..."
    '    v_frm.ShowDialog()
    'End Sub


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
        Dim j As Integer = 0
        For i As Integer = 0 To v_arrField.Count - 2
            v_arrTemp = v_arrField(i).Split("|")
            v_oDataRow(j) = IIf(Trim(v_arrTemp(1)) = "-1", Trim(v_arrTemp(2)), Trim(v_arrTemp(1)))
            j += 1
            v_oDataRow(j) = Trim(v_arrTemp(2))
            j += 1
        Next
        mv_DataSet.Tables("F_" & v_strRptID).Rows.Add(v_oDataRow)

    End Sub

    Private Function WriteDataTableToFile(ByVal pv_strRptID As String) As Long
        Dim v_strFile As String
        Dim v_stream As System.IO.StreamWriter
        Dim v_ds As New DataSet("DS_" & pv_strRptID)
        Try
            v_strFile = Application.LocalUserAppDataPath & "\" & pv_strRptID & ".xml"
            v_stream = New System.IO.StreamWriter(v_strFile)

            v_ds = mv_DataSet.Copy

            For i As Integer = 0 To v_ds.Tables.Count - 1
                If (v_ds.Tables(i).TableName <> pv_strRptID) And (v_ds.Tables(i).TableName <> "F_" & pv_strRptID) Then
                    v_ds.Tables.RemoveAt(i)
                End If
            Next
            v_ds.WriteXml(v_stream, XmlWriteMode.WriteSchema)

            v_stream.Close()
            v_stream.Dispose()

            System.IO.File.SetLastWriteTime(v_strFile, CDate(BusDate))
            Return 0
        Catch ex As Exception
            Return 1
        End Try
    End Function

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
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

    Private Sub ShowReport(ByVal pv_strRptID As String)
        Dim v_thread As New Threading.Thread(AddressOf ShowFormProcess)
        Try

            v_thread.Start()
            Application.DoEvents()

            Dim v_strObjMsg As String
            Dim v_xmlDocument As New XmlDocumentEx
            Dim v_nodeList As Xml.XmlNodeList
            Dim v_objPageSetting As New ReportSetting
            Dim v_ws As New BDSChannel.BDSDelivery

            v_objPageSetting.ReportID = pv_strRptID

            If Mid(mv_strAuth, 2, 1) <> "Y" Then
                v_thread.Abort()
                MsgBox("Bạn không có quyền xem báo cáo này!", MsgBoxStyle.Exclamation)
                Exit Sub
            End If

            v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_RP_RPREPORT, gc_ActionAdhoc, , v_objPageSetting.ReportID, "PageSetting")
            Dim v_lngError As Long = v_ws.Message(v_strObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Exit Sub
            End If

            v_xmlDocument.LoadXml(v_strObjMsg)
            v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/PageSetting")

            Dim i As Integer
            Dim v_strValue As String
            Dim v_strFldName As String

            For i = 0 To v_nodeList.Item(0).ChildNodes.Count - 1
                With v_nodeList.Item(0).ChildNodes(i)
                    v_strValue = .InnerText.ToString
                    v_strFldName = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                    Select Case Trim(v_strFldName)
                        Case "RPTTITLE"
                            v_objPageSetting.Title = v_strValue
                        Case "EN_RPTTITLE"
                            v_objPageSetting.En_Title = v_strValue
                        Case "RPTCMDSQL"
                            v_objPageSetting.SQLCommand = v_strValue
                        Case "RPTTYPE"
                            v_objPageSetting.ReportType = v_strValue
                        Case "ORIENTATION"
                            v_objPageSetting.Orientation = v_strValue
                        Case "DSN"
                            v_objPageSetting.DSN = v_strValue
                        Case "OBJNAME"
                            v_objPageSetting.ObjectName = v_strValue
                        Case "CREATEBY"
                            v_objPageSetting.CreateBy = v_strValue
                        Case "CREATEDATE"
                            v_objPageSetting.CreateDate = v_strValue
                        Case "REPORTPARAM"
                            v_objPageSetting.Param = v_strValue
                        Case "TITLE_HEIGHT"
                            v_objPageSetting.HeaderHeight = v_strValue
                        Case "HEADER_HEIGHT"
                            v_objPageSetting.HHeight = v_strValue
                        Case "FOOTER_HEIGHT"
                            v_objPageSetting.FHeight = v_strValue
                        Case "RPFONTSIZE"
                            If v_strValue.Trim = "" Then
                                v_strValue = "9.75"
                            End If
                            v_objPageSetting.FontSize = v_strValue
                        Case "RPPAPERSIZE"
                            If v_strValue.Trim = "" Then
                                v_strValue = "A4"
                            End If
                            v_objPageSetting.PaperSize = v_strValue
                    End Select
                End With
            Next
            v_objPageSetting.UserLanguage = Me.UserLanguage
            'v_objPageSetting.ReportID = Replace(Trim(CType(SearchGrid.CurrentRow, Xceed.Grid.DataRow).Cells(KeyColumn).Value), ".", String.Empty)

            'Lay cac truong bao cao
            v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/RPTFLD")
            Dim v_objTitleHeader As ReportHeaderRow
            Dim v_arrTitleHeader(v_nodeList.Count - 1) As ReportHeaderRow

            For v_int As Integer = 0 To v_nodeList.Count - 1
                v_objTitleHeader = New ReportHeaderRow
                For i = 0 To v_nodeList.Item(v_int).ChildNodes.Count - 1

                    With v_nodeList.Item(v_int).ChildNodes(i)
                        v_strValue = .InnerText.ToString
                        v_strFldName = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                        Select Case Trim(v_strFldName)
                            Case "AUTOID"
                                v_objTitleHeader.ID = v_strValue
                            Case "PARENT_ID"
                                v_objTitleHeader.ParentID = v_strValue
                            Case "FIELDNAME"
                                v_objTitleHeader.FieldName = v_strValue
                            Case "FIELDTYPE"
                                v_objTitleHeader.FieldType = v_strValue
                            Case "CAPTION"
                                v_objTitleHeader.Caption = v_strValue
                            Case "EN_CAPTION"
                                v_objTitleHeader.En_Caption = v_strValue
                            Case "FORMAT"
                                v_objTitleHeader.Format = v_strValue
                            Case "DISPLAY"
                                v_objTitleHeader.Display = v_strValue
                            Case "WIDTH"
                                v_objTitleHeader.Width = v_strValue
                            Case "ISDATAFIELD"
                                v_objTitleHeader.IsDataField = v_strValue
                            Case "ISSUM"
                                v_objTitleHeader.IsSum = v_strValue
                            Case "ISPARENT"
                                v_objTitleHeader.IsParent = v_strValue
                            Case "ALIGN"
                                v_objTitleHeader.TextAlign = v_strValue
                            Case "LEV"
                                v_objTitleHeader.Lev = v_strValue
                            Case "HEIGHT"
                                v_objTitleHeader.Height = v_strValue
                        End Select
                    End With
                Next
                v_arrTitleHeader(v_int) = v_objTitleHeader
            Next

            v_objPageSetting.ReportFld = v_arrTitleHeader

            'Lay group bao cao
            v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/GRPFLD")
            If v_nodeList.Count > 0 Then
                Dim v_objGroup As ReportGroup
                Dim v_arrGroup(v_nodeList.Count - 1) As ReportGroup

                For v_int As Integer = 0 To v_nodeList.Count - 1
                    v_objGroup = New ReportGroup
                    For i = 0 To v_nodeList.Item(v_int).ChildNodes.Count - 1

                        With v_nodeList.Item(v_int).ChildNodes(i)
                            v_strValue = .InnerText.ToString
                            v_strFldName = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                            Select Case Trim(v_strFldName)
                                Case "FIELDNAME"
                                    v_objGroup.FieldName = v_strValue
                                Case "FIELDTYPE"
                                    v_objGroup.FldType = v_strValue
                                Case "CATION"
                                    v_objGroup.Caption = v_strValue
                                Case "EN_CATION"
                                    v_objGroup.En_Caption = v_strValue
                                Case "FORMAT"
                                    v_objGroup.Format = v_strValue
                                Case "WIDTH"
                                    v_objGroup.CaptionWidth = v_strValue
                                Case "GRPFOOTER"
                                    v_objGroup.Footer = v_strValue
                            End Select
                        End With
                    Next
                    v_arrGroup(v_int) = v_objGroup
                Next

                v_objPageSetting.ReportGrp = v_arrGroup
            End If

            Dim frm As New AppCore.frmReportView
            frm.ReportSetting = v_objPageSetting
            frm.ClientDataSet = mv_DataSet
            frm.UserLanguage = UserLanguage
            frm.Print = ("Y" = Mid(mv_strAuth, 3, 1))
            frm.ShowReport()

            frm.Show()
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                                     & "Error code: System error!" & vbNewLine _
                                                     & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        Finally
            v_thread.Abort()
        End Try
    End Sub

    Public Sub ShowFormProcess()
        Dim v_frm As New Sats.AppCore.frmProcess
        v_frm.ShowDialog()
    End Sub

    Public Sub New(ByRef pv_DataSet)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        mv_DataSet = pv_DataSet
        ' Add any initialization after the InitializeComponent() call.

    End Sub
End Class