Imports System.Windows.Forms
Imports Sats.CommonLibrary
Imports System.Data.OleDb
Imports Sats.SQLEngine
Imports System.ComponentModel
Imports Sats.ClientCA
Imports BkavCASign
Imports System.IO
Imports System.Security.Cryptography

Public Class frmTransaction


#Region " Declare constant and variables "
    Const c_ResourceManager = "Sats.AppCore.frmTransaction_"
    Protected mv_xmlDocumentInquiryData As New Xml.XmlDocument
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
    Protected mv_bolCheck As Boolean = True
    Private mv_Check As Boolean = True
    Protected mv_ResourceManager As Resources.ResourceManager
    Protected mv_strMemberFilter As String
    Protected mv_strStockFilter As String
    Private mv_lngAllMember As Long
    Private mv_lngAllStock As Long
    Private mv_strSignCA As String

    Private mv_strVsdBrid As String
    Private mv_strVsdBrid2 As String
    Private mv_strKeyDecrypt3079 = "&/?@*>:>"

    Protected mv_arrObjFields() As CFieldMaster
    Protected mv_arrObjCoFields() As CFieldMaster
    Protected mv_arrObjFldVals() As CFieldVal
    Protected mv_arrObjCoFldVals() As CFieldVal

    Private mv_strMSGAMT As String
    Private mv_strMICODE As String
    Private mv_strCOMICODE As String

    Private mv_strSICODE As String
    Private mv_strBACKDATE As String
    Private mv_strCHKID As String
    Private mv_strOFFID As String
    Private mv_strCFRID As String
    Private mv_strCOTLTXCD As String = ""
    Private mv_strCHILDTLTXCD As String = ""
    Private mv_strFileName As String = ""
    Private mv_strFileName1 As String = ""
    Private mv_strRange As String = ""
    Private mv_strTXNUM As String = ""
    Private mv_strTXDATE As String = ""
    Private mv_strISPARENT As String = ""
    Private mv_intISBRID As Integer = 1
    Private mv_intVisible_Child As Integer = 1
    Private mv_strTxNote As String
    Private mv_strTblChk As String

    Private mv_strFILETLTXCD As String = ""
    Private RemotePath As String
    Private mv_strBrid As String = ""
    Private mv_strCurrDate As String = ""
    Private mv_strDate1114 As String = ""
    Private mv_strFileNameCA As String = ""
    Private mv_strFileNameCA1 As String = ""
    Private mv_intCSEXPType As Integer

    Private mv_ds As New DataSet

    Const CONTROL_TOP = 10
    Const CONTROL_LEFT = 10
    Const CONTROL_GAP = 2
    Const CONTROL_HEIGHT = 23
    Const CONTROL_HEIGHT_GRID = 500
    Const ALL_WIDTH = 900
    Const WIDTH_PERCHAR = 8
    Const WIDTH_LABLE = 100
    Const PANEL_TOP = 54
    Const PANEL_HEIGHT = 100

    Protected Const PREFIXED_MSKDATA = "mskData"

    Private mv_isAutoClosedWhenOK As Boolean = False

    Public mv_frmSearchScreen As frmSearch

    'Protected mv_BDSDelivery As BDSChannel.BDSDelivery
    Private mv_viewsql As String = ""
    Private mv_IsParent As Long = -1
    Private mv_fail_index As Long
    ' quick search 
    Dim mv_lngListIndex As Long
    Dim mv_intChar As Integer

    'Private v_thread As Threading.Thread
    Private mv_intErrCount As Integer
    Private mv_strPassDate As String
    Public mv_lstIsTextChanged() As Boolean
    Public mv_blnIsF5() As Boolean
    Protected v_oProcess As ProcessForm
    Private mv_strObjType As String = "T"
    Private mv_oProxy As BDSChannel.BDSDelivery
    Private mv_oClient As Client
#End Region
#Region " Properties "

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

    Public Property PassDate() As String
        Get
            Return mv_strPassDate
        End Get
        Set(ByVal value As String)
            mv_strPassDate = value
        End Set
    End Property

    Public Property ParentID() As String
        Get
            Return mv_IsParent
        End Get
        Set(ByVal Value As String)
            mv_IsParent = Value
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

    Public Property ObjectType() As String
        Get
            Return mv_strObjType
        End Get
        Set(ByVal value As String)
            mv_strObjType = value
        End Set
    End Property

    Public Property VSDBRID() As String
        Get
            Return mv_strVsdBrid
        End Get
        Set(ByVal value As String)
            mv_strVsdBrid = value
        End Set
    End Property
    Public Property VSDBRID2() As String
        Get
            Return mv_strVsdBrid2
        End Get
        Set(ByVal value As String)
            mv_strVsdBrid2 = value
        End Set
    End Property

#End Region

#Region "Khoi tao Form giao dich"
    Protected Overridable Sub OnInit()
        Try
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

            
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                                     & "Error code: System error!" & vbNewLine _
                                                     & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
            mv_bolCheck = False
        End Try

    End Sub


    Protected Overridable Sub LoadScreen(ByVal strTLTXCD As String)

        Dim i, j As Integer
        Dim v_objField As CFieldMaster, v_objFieldVal As CFieldVal
        Dim v_strFieldName As String = "", v_strDefName As String = "", v_strCaption As String = ""
        Dim v_strFldType As String = "", v_strFldMask As String = "", v_strFldFormat As String = ""
        Dim v_strLList As String = "", v_strLChk As String = "", v_strDefVal As String = ""
        Dim v_strAmtExp As String = "", v_strValidTag As String = "", v_strLookUp As String = ""
        Dim v_strDataType As String = "", v_strControlType As String = "", v_strChainName As String = ""
        Dim v_strLookupName As String = "", v_strPrintInfo As String = "", v_strInvName As String = ""
        Dim v_strInvFormat As String = "", v_strFldSource As String = "", v_strFldDesc As String = ""
        Dim v_strSearchCode As String = "", v_strSrModCode As String = "", v_strMemberField As String = ""
        Dim v_strStockField As String = "", v_strIsDuplicated As String = ""
        Dim v_intOdrNum, v_intFldLen, v_intIndex, v_intMaxFldLen As Integer
        Dim v_blnVisible, v_blnEnabled, v_blnMandatory As Boolean
        Try
            Dim v_strSQL As String
            Dim v_obj As New SQLDataAccessLayer(TellerId)
            Dim v_ds As DataSet
            v_strSQL = "SELECT a.* FROM TLTX a where a.TLTXCD='" & strTLTXCD & "'"
            v_ds = v_obj.ExecuteReturnDataSet(v_strSQL)
            'bangpv
            Dim v_xmlDocument As New System.Xml.XmlDocument
            Dim v_nodeList As Xml.XmlNodeList
            Dim v_strObjMsg As String

            Dim v_strCmdInquiry = "SELECT c.modcode FROM TLTX a, " _
               & " ( " _
               & " select c.tltxcd from tlauth c where c.status =0 and c.deleted =0 and c.authtype = 'U' and c.authid = '" & TellerId & "' and c.tltype = '0' " _
               & " and c.brid ='" & BranchId & "'" _
               & " union" _
               & " select c.tltxcd from tlauth c where c.status =0 and c.deleted =0 and c.authtype = 'G' and c.brid ='" & BranchId & "'" _
               & " and c.authid in ( select grpid from tlgrpusers d where d.tlid = '" & TellerId & "' and d.status =0 and d.deleted =0 ) and c.tltype = '0'" _
               & " ) b, appmodules c " _
               & " where a.status =0 and a.deleted =0 and a.tltxcd = b.tltxcd and substr(a.tltxcd,1,2) = c.txcode and c.deleted=0 and c.status = 0 " _
               & " and a.TLTXCD='" & strTLTXCD & "'"

            v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SY_SEARCHFLD, gc_ActionInquiry, v_strCmdInquiry, , )

            Dim v_lngError As Long = Proxy.Message(v_strObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(gc_ERR_GET_MSG_VN, MsgBoxStyle.Critical, gc_ApplicationTitle)
                Exit Sub
            End If

            v_xmlDocument.LoadXml(v_strObjMsg)
            v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")
            If v_nodeList.Count = 0 Then
                'Neu khong tim thay ma giao dich
                'v_oProcess.StopProcessForm()
                'Application.DoEvents()
                MessageBox.Show("Bạn không có quyền truy nhập giao dịch này !", gc_ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Information)
                'v_blnKey = False
                Me.Cursor = Cursors.Default
                Exit Sub
            End If
            'end bangpv



            If v_ds.Tables(0).Rows.Count = 0 Then
                'Neu khong tim thay ma giao dich
                MessageBox.Show("Bạn không có quyền truy nhập giao dịch này !", gc_ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Information)
                mv_bolCheck = False
                Exit Sub
            End If
            For i = 0 To v_ds.Tables(0).Rows.Count - 1
                With v_ds.Tables(0)
                    If UserLanguage <> "EN" Then
                        lblTransCaption.Text = .Rows(i)("TXDESC")
                        Me.TabText = Trim(.Rows(i)("TXDESC"))
                    Else
                        lblTransCaption.Text = .Rows(i)("EN_TXDESC")
                        Me.TabText = Trim(.Rows(i)("EN_TXDESC"))
                    End If
                    mv_strCHKID = Trim(.Rows(i)("CHKID"))
                    mv_strOFFID = Trim(.Rows(i)("OFFID"))
                    mv_strCFRID = Trim(.Rows(i)("CFRID"))
                    mv_strMSGAMT = Trim(.Rows(i)("MSG_AMT"))
                    mv_strMICODE = Trim(.Rows(i)("MICODE"))
                    mv_strCOMICODE = Trim(.Rows(i)("COMICODE"))
                    mv_strBACKDATE = Trim(.Rows(i)("BUSDATE"))
                    mv_strSICODE = Trim(.Rows(i)("SICODE"))
                    mv_strCOTLTXCD = Trim(.Rows(i)("COTLTXCD"))
                    mv_strFileName = Mid(Trim(.Rows(i)("FILENAME")), 1, 2)
                    mv_strFileName1 = Mid(Trim(.Rows(i)("FILENAME")), 4, 2)
                    mv_strRange = Trim(.Rows(i)("RANGE"))
                    mv_strTXNUM = Trim(.Rows(i)("TXNUM"))
                    mv_strTXDATE = Trim(.Rows(i)("TXDATE"))
                    mv_strISPARENT = Trim(.Rows(i)("ISPARENT"))
                    mv_strCHILDTLTXCD = Trim(.Rows(i)("CHILDTLTXCD"))
                    mv_intISBRID = CInt(.Rows(i)("ISBRID"))
                    mv_intVisible_Child = CInt(.Rows(i)("VISIBLE_CHILD"))
                    mv_strTxNote = Trim(.Rows(i)("TXNOTE"))
                    mv_strTblChk = Trim(.Rows(i)("TBLCHK"))
                    mv_strSignCA = Trim(.Rows(i)("ISSIGNCA"))
                End With
            Next
            
            'Hanm5 : Start - Lay thong tin phan cap duyet giao dich theo User hoac Group

            v_strSQL = "select authid, authtype, chkid, offid, cfrid from tltxuserauth where tltxcd = '" & ObjectName & "'"
            v_ds = v_obj.ExecuteReturnDataSet(v_strSQL)
            If v_ds.Tables(0).Rows.Count > 0 Then
                Dim v_strCHKID As String = "0"
                Dim v_strOFFID As String = "0"
                Dim v_strCFRID As String = "0"
                Dim v_strCHKID_final As String = "1"
                Dim v_strOFFID_final As String = "1"
                Dim v_strCFRID_final As String = "1"

                Dim v_strAuthType As String
                For i = 0 To v_ds.Tables(0).Rows.Count - 1
                    With v_ds.Tables(0)
                        v_strCHKID = Trim(.Rows(i)("CHKID"))
                        v_strOFFID = Trim(.Rows(i)("OFFID"))
                        v_strCFRID = Trim(.Rows(i)("CFRID"))
                        v_strAuthType = Trim(.Rows(i)("AUTHTYPE"))
                    End With

                    If v_strAuthType = "U" Then
                        mv_strCHKID = v_strCHKID
                        mv_strOFFID = v_strOFFID
                        mv_strCFRID = v_strCFRID
                        Exit For
                    Else
                        If CInt(v_strCHKID_final) > CInt(v_strCHKID) Then
                            v_strCHKID_final = v_strCHKID
                        End If
                        If CInt(v_strOFFID_final) > CInt(v_strOFFID) Then
                            v_strOFFID_final = v_strOFFID
                        End If
                        If CInt(v_strCFRID_final) > CInt(v_strCFRID) Then
                            v_strCFRID_final = v_strCFRID
                        End If
                    End If
                    mv_strCHKID = v_strCHKID_final
                    mv_strOFFID = v_strOFFID_final
                    mv_strCFRID = v_strCFRID_final
                Next
            End If

            'Hanm5 : End - Lay thong tin phan cap duyet giao dich theo User hoac Group

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
                    v_intMaxFldLen = Trim(.Rows(i)("MAXFLDLEN"))
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
                    .MaxFldLen = v_intMaxFldLen
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

                'Xac dinh index cua mang FldMaster
                For j = 0 To mv_arrObjFields.GetLength(0) - 1 Step 1
                    If Not mv_arrObjFields(j) Is Nothing Then
                        If Trim(mv_arrObjFields(j).FieldName) = Trim(v_strFieldVal_FldName) Then
                            v_intIndex = j
                            Exit For
                        End If
                    End If
                Next
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
            ' Lay thong tin giao dich lo
            If mv_strCOTLTXCD <> "" Then
                'Lay thong tin chi tiet cac truong cua giao dich
                v_strSQL = "SELECT * FROM FLDMASTER WHERE OBJNAME = '" & mv_strCOTLTXCD & "' and upper(isimported) = 'Y' and deleted = 0 and status = 0 ORDER BY COODRNUM"
                v_ds = v_obj.ExecuteReturnDataSet(v_strSQL)
                ReDim mv_arrObjCoFields(v_ds.Tables(0).Rows.Count)

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
                        v_strSrModCode = Trim(.Rows(i)("SRMODCODE"))
                        v_strPrintInfo = .Rows(i)("PRINTINFO")
                        v_strMemberField = Trim(.Rows(i)("MEMBERFIELD"))
                        v_strStockField = Trim(.Rows(i)("STOCKFIELD"))
                        v_strIsDuplicated = Trim(.Rows(i)("ISDUPLICATED"))
                        v_intMaxFldLen = Trim(.Rows(i)("MAXFLDLEN"))
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
                            'Lay ngay lam viec hien tai
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
                        .IsDuplicated = v_strIsDuplicated
                        .MaxFldLen = v_intMaxFldLen
                    End With
                    mv_arrObjCoFields(i) = v_objField
                Next
                ReDim Preserve mv_arrObjCoFields(v_ds.Tables(0).Rows.Count)

                'Lay cac quy tac kiem tra cua cac truong giao dich
                v_strSQL = "SELECT FLDNAME, VALTYPE, OPERATOR, VALEXP, VALEXP2, ERRMSG, EN_ERRMSG FROM FLDVAL " & _
                    "WHERE upper(OBJNAME) = '" & mv_strCOTLTXCD & "' ORDER BY VALTYPE, FLDNAME, ODRNUM"

                v_ds = v_obj.ExecuteReturnDataSet(v_strSQL)
                ReDim mv_arrObjCoFldVals(v_ds.Tables(0).Rows.Count)
                v_strFieldVal_FldName = ""
                v_strFieldVal_ValType = ""
                v_strFieldVal_Operator = ""
                v_strFieldVal_ValExp = ""
                v_strFieldVal_ValExp2 = ""
                v_strFieldVal_ErrMsg = ""
                v_strFieldVal_EnErrMsg = ""

                For i = 0 To v_ds.Tables(0).Rows.Count - 1
                    With v_ds.Tables(0)
                        v_strFieldVal_FldName = Trim(.Rows(i)("FLDNAME"))
                        v_strFieldVal_ValType = Trim(.Rows(i)("VALTYPE"))
                        v_strFieldVal_Operator = Trim(.Rows(i)("OPERATOR"))
                        v_strFieldVal_ValExp = Trim(.Rows(i)("VALEXP"))
                        v_strFieldVal_ValExp2 = Trim(.Rows(i)("VALEXP2"))
                        v_strFieldVal_ErrMsg = Trim(.Rows(i)("ERRMSG"))
                        v_strFieldVal_EnErrMsg = Trim(.Rows(i)("EN_ERRMSG"))
                    End With

                    'Xac dinh index cua mang FldMaster
                    For j = 0 To mv_arrObjCoFields.GetLength(0) - 1 Step 1
                        If Not mv_arrObjCoFields(j) Is Nothing Then
                            If Trim(mv_arrObjCoFields(j).FieldName) = Trim(v_strFieldVal_FldName) Then
                                v_intIndex = j
                            End If
                        End If
                    Next

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
                    mv_arrObjCoFldVals(i) = v_objFieldVal
                Next
                ReDim Preserve mv_arrObjCoFldVals(v_ds.Tables(0).Rows.Count)
            End If
            v_ds.Dispose()
            v_obj.CloseConnection()
            v_obj = Nothing
            DisplayScreen()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub


    Protected Sub DisplayScreen()
        Dim v_intIndex, v_intCount, v_intPosition, v_intTop, v_intLeft, v_intWidth, v_intLastTop As Integer
        Dim v_lblCaption, v_lblDesc As Label
        Dim v_mskData As FlexMaskEditBox = Nothing
        Dim v_txtData As TextBox = Nothing
        Dim v_cboData As ComboBox = Nothing
        Dim v_gridControl As ucGridControl = Nothing

        Dim v_strCmdSQL As String, v_strObjMsg As String

        Try
            Me.pnTransDetail.Controls.Clear()
            Dim v_ds As DataSet
            'Dim v_obj As New SQLDataAccessLayer(TellerId)
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
                ReDim mv_lstIsTextChanged(v_intCount)
                ReDim mv_blnIsF5(v_intCount)

                v_intLastTop = 0
                v_intPosition = 0
                v_intTop = CONTROL_TOP

                For v_intIndex = 0 To v_intCount - 1 Step 1
                    If Not mv_arrObjFields(v_intIndex) Is Nothing Then
                        v_lblCaption = New Label
                        v_lblDesc = New Label
                        v_lblCaption.Visible = mv_arrObjFields(v_intIndex).Visible
                        v_lblDesc.Visible = False

                        'v_intTop = CONTROL_TOP + v_intPosition * (CONTROL_HEIGHT + CONTROL_GAP)
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

                        If mv_arrObjFields(v_intIndex).FldSource = "BOLD" Then
                            v_lblCaption.Font = New Drawing.Font(v_lblCaption.Font, Drawing.FontStyle.Bold)
                        End If

                        mv_lstIsTextChanged(v_intIndex) = False
                        mv_blnIsF5(v_intIndex) = False
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
                                v_txtData.WordWrap = True
                                v_txtData.Multiline = True
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

                                If mv_arrObjFields(v_intIndex).Enabled Then
                                    AddHandler v_txtData.GotFocus, AddressOf mskData_GotFocus
                                    AddHandler v_txtData.Validating, AddressOf mskData_Validating
                                    AddHandler v_txtData.TextChanged, AddressOf mskData_TextChanged
                                End If
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

                                If mv_arrObjFields(v_intIndex).Enabled Then
                                    AddHandler v_mskData.GotFocus, AddressOf mskData_GotFocus
                                    AddHandler v_mskData.Validating, AddressOf mskData_Validating
                                    AddHandler v_mskData.TextChanged, AddressOf mskData_TextChanged
                                End If

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
                                v_cboData.DropDownWidth = 400

                                If mv_arrObjFields(v_intIndex).Enabled Then
                                    AddHandler v_cboData.SelectedIndexChanged, AddressOf cboData_SelectedIndexChanged
                                    AddHandler v_cboData.Validating, AddressOf v_cboData_Validating
                                    AddHandler v_cboData.TextChanged, AddressOf mskData_TextChanged
                                End If

                                v_lblDesc.Top = v_intTop
                                v_intLeft = v_cboData.Left + v_cboData.Width + CONTROL_GAP
                                If v_intLeft >= ALL_WIDTH Then
                                    v_intWidth = 0
                                Else
                                    v_intWidth = ALL_WIDTH - v_intLeft
                                End If
                                'Lấy dữ liệu cho ComboBox
                                v_strCmdSQL = mv_arrObjFields(v_intIndex).LookupList
                                If InStr(v_strCmdSQL, "#") > 0 Then
                                    For Each v_ctrl In Me.pnTransDetail.Controls
                                        If TypeOf (v_ctrl) Is ComboBoxEx Then
                                            v_strCmdSQL = Replace(v_strCmdSQL, "#" & Mid(CType(v_ctrl, ComboBoxEx).Name, Len(PREFIXED_MSKDATA) + 1), CType(v_ctrl, ComboBoxEx).SelectedValue)
                                        End If
                                    Next
                                End If
                                Dim v_obj As New SQLDataAccessLayer(TellerId)
                                v_ds = v_obj.ExecuteReturnDataSet(v_strCmdSQL)
                                v_cboData.BeginUpdate()
                                v_cboData.DataSource = v_ds.Tables(0)
                                v_cboData.DisplayMember = "DISPLAY"
                                v_cboData.ValueMember = "VALUE"
                                v_cboData.EndUpdate()
                                v_obj.CloseConnection()
                                v_obj = Nothing

                                If ObjectType = "R" Then
                                    If mv_arrObjFields(v_intIndex).RiskField Then
                                        CType(v_cboData, ComboBoxEx).AddItems(mv_ResourceManager.GetString("ALL"), "-1")
                                    End If
                                End If
                            Case "G"
                                v_gridControl = New ucGridControl
                                v_gridControl.TableName = mv_arrObjFields(v_intIndex).LookupList
                                v_gridControl.Proxy = Me.Proxy
                                v_gridControl.UserLanguage = Me.UserLanguage
                                v_gridControl.TellerId = Me.TellerId
                                v_gridControl.BusDate = Me.BusDate
                                v_gridControl.VsdBrid = Me.VSDBRID
                                v_gridControl.VsdBrid2 = Me.VSDBRID2
                                v_gridControl.BranchId = Me.BranchId
                                v_gridControl.StockFilter = Me.StockFilter
                                v_gridControl.MemberFilter = Me.MemberFilter
                                v_gridControl.AllMember = Me.AllMember
                                v_gridControl.AllStock = Me.AllStock
                                v_gridControl.TLTXCD = Me.mv_strObjectName

                                v_gridControl.OnInit()

                                v_gridControl.Visible = mv_arrObjFields(v_intIndex).Visible
                                v_gridControl.Top = v_intTop
                                v_intLeft = CONTROL_LEFT + CONTROL_GAP + v_intLabelWidth
                                v_gridControl.Left = v_intLeft
                                v_intWidth = mv_arrObjFields(v_intIndex).FieldLength * WIDTH_PERCHAR
                                If ALL_WIDTH < v_intLeft + v_intWidth Then
                                    v_intWidth = ALL_WIDTH - v_intLeft
                                End If
                                v_gridControl.Width = v_intWidth
                                v_gridControl.Tag = v_intIndex
                                v_gridControl.Enabled = mv_arrObjFields(v_intIndex).Enabled
                                v_gridControl.Name = PREFIXED_MSKDATA & Trim(mv_arrObjFields(v_intIndex).FieldName)

                                If mv_arrObjFields(v_intIndex).Enabled Then
                                    'Add handler
                                End If

                                v_lblDesc.Top = v_intTop
                                v_intLeft = v_gridControl.Left + v_gridControl.Width + CONTROL_GAP
                                If v_intLeft >= ALL_WIDTH Then
                                    v_intWidth = 0
                                Else
                                    v_intWidth = ALL_WIDTH - v_intLeft
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
                            Case "G"
                                Me.pnTransDetail.Controls.Add(v_gridControl)
                                mv_arrObjFields(v_intIndex).ControlIndex = Me.pnTransDetail.Controls.IndexOf(v_gridControl)
                        End Select

                        'Tính toán vị trí hiển thị nếu control là  visible
                        If mv_arrObjFields(v_intIndex).Visible Then
                            v_intPosition = v_intPosition + 1
                            If Trim(mv_arrObjFields(v_intIndex).ControlType) = "G" Then
                                v_intTop = v_intTop + (CONTROL_HEIGHT_GRID + CONTROL_GAP)
                            Else
                                v_intTop = v_intTop + (CONTROL_HEIGHT + CONTROL_GAP)
                            End If
                            v_intLastTop = v_intTop
                        End If

                    End If
                Next

                If v_intLastTop + CONTROL_HEIGHT + 24 + btnCancel.Height + PANEL_TOP > Me.Height Then
                    pnTransDetail.AutoScroll = True
                    pnTransDetail.Height = Me.Height - PANEL_TOP - btnCancel.Height - 16
                    Me.btnCancel.Top = Me.pnTransDetail.Height + Me.Panel1.Height + 8
                    Me.btnOK.Top = Me.btnCancel.Top
                Else
                    Me.pnTransDetail.Height = v_intLastTop + CONTROL_HEIGHT + 8
                    Me.btnCancel.Top = Me.pnTransDetail.Height + Me.Panel1.Height + 8
                    Me.btnOK.Top = Me.btnCancel.Top
                End If
                Me.pnTransDetail.Controls(mv_arrObjFields(0).ControlIndex).Select()
            Else
                Me.pnTransDetail.Visible = False
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub LoadResource(ByRef pv_ctrl As Windows.Forms.Control)
        Dim v_ctrl As Windows.Forms.Control
        Try
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
        Catch ex As Exception
            Throw ex
        End Try
        
    End Sub
#End Region

#Region "Cac ham, thu tuc khac"
    Private Sub FormResize()
        Me.pnTransDetail.Width = Me.Width
        Me.btnOK.Top = Me.btnCancel.Top

        Me.btnOK.Left = 8
        Me.btnCancel.Left = Me.btnOK.Left + Me.btnCancel.Width + 10

        If mv_viewsql <> "" Then
            If mv_IsParent = 2 Then
                Me.btnOK.Text = "Chi tiết ..."
            Else
                Me.btnCancel.Left = CInt(Me.Width / 2) - CONTROL_TOP * 3
                Me.btnOK.Visible = False
            End If
        End If
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
            'bangpv test đổi định dạng form search
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("vi-VN")
            'end bangpv
            'tuannta
            'purpose : if branchid is code of opened fund market (0008), v_intDecimal will be 2
            'If BranchId = "0008" Then
             '   v_intDecimal = 2
            'End If
            'end tuanta

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
        Dim pv_ctrl As TextBox
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
                                            ' tuanta 
                                            pv_ctrl = CType(ctl, TextBox)
                                            pv_ctrl.Text = Replace(pv_ctrl.Text, ".", ",")
                                            FormatNumericTextbox(pv_ctrl)
                                            'FormatNumericTextbox(CType(ctl, TextBox))
                                            ' end tuanta
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

    Public Sub FillLookupData(ByVal v_strFLDNAME As String, ByVal v_strVALUE As String, ByVal pv_ds As DataSet, Optional ByVal v_strFieldKey As String = "VALUE")
        Try
            Dim ctl As Control
            Dim v_strLookupName As String, i, v_intCount As Integer
            Dim v_strLookupValue As String
            Dim pv_ctrl As TextBox
            v_intCount = mv_arrObjFields.GetLength(0)
            Dim v_oRow As DataRow
            Dim v_oDataColumn(1) As DataColumn
            v_oDataColumn(0) = pv_ds.Tables(0).Columns("VALUE")
            pv_ds.Tables(0).PrimaryKey = v_oDataColumn
            v_oRow = pv_ds.Tables(0).Rows.Find(v_strVALUE)
            If v_intCount > 0 Then
                'Nạp dữ liệu Lookup cho các control đã khai báo
                For i = 0 To v_intCount - 1 Step 1
                    If Not mv_arrObjFields(i) Is Nothing Then
                        If Trim(mv_arrObjFields(i).LookupName).Length > 0 Then
                            If Mid(Trim(mv_arrObjFields(i).LookupName), 1, 2) = v_strFLDNAME Then
                                v_strLookupName = Mid(Trim(mv_arrObjFields(i).LookupName), 3)
                                v_strLookupValue = v_oRow(v_strLookupName)
                                ctl = Me.pnTransDetail.Controls(mv_arrObjFields(i).ControlIndex)
                                If TypeOf (ctl) Is ComboBoxEx Then
                                    CType(ctl, ComboBoxEx).SelectedValue = v_strLookupValue
                                Else
                                    ctl.Text = v_strLookupValue
                                End If

                                If mv_arrObjFields(i).DataType = "N" And Len(mv_arrObjFields(i).FieldFormat) > 0 Then
                                    ' tuanta 
                                    pv_ctrl = CType(ctl, TextBox)
                                    pv_ctrl.Text = Replace(pv_ctrl.Text, ".", ",")
                                    FormatNumericTextbox(pv_ctrl)
                                    'FormatNumericTextbox(CType(ctl, TextBox))
                                    'end tuanta
                                End If
                            End If
                        End If
                    End If
                Next
            End If
        Catch ex As Exception

        End Try
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
        Dim v_strMsgErr As String
        Try
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
                        mv_blnIsF5(v_intIndex) = True
                        'Tra cá»©u thÃ´ng tin
                        If Len(mv_arrObjFields(v_intIndex).SearchCode) > 0 Then
                            'Gá»?i hÃ m Ä‘á»ƒ thiáº¿t láº­p mÃ n hÃ¬nh tÃ¬m kiáº¿m
                            'Kiem tra xem co phai la lookup cua ben nhan hay khong?
                            If Not Mid(mv_arrObjFields(v_intIndex).SearchCode, 1, 3) = "CO_" Then
                                Dim v_strSearchCode As String = mv_arrObjFields(v_intIndex).SearchCode.ToUpper
                                Select Case v_strSearchCode
                                    Case "CAFUNC"
                                        Dim v_frm As New frmFunction
                                        v_frm.UserLanguage = mv_strLanguage
                                        v_frm.TellerID = TellerId
                                        v_frm.Proxy = Proxy
                                        v_frm.ShowDialog()
                                        ActiveControl.Text = v_frm.ReturnValue
                                    Case "FOREIGNNO"
                                        Dim v_frm As New frmForeignNoReg
                                        v_frm.UserLanguage = mv_strLanguage
                                        v_frm.TellerId = TellerId
                                        Dim v_strCardNo As String = ""
                                        Dim v_strCardDate As String = ""
                                        Dim v_strIaType As String = ""

                                        If ObjectName = "2016" Then
                                            v_strCardNo = GetValueControlByName("mskData07")
                                            v_strCardDate = GetValueControlByName("mskData08")
                                            v_strIaType = ""
                                        ElseIf ObjectName = "2031" Then
                                            v_strCardNo = GetValueControlByName("mskData06")
                                            v_strCardDate = GetValueControlByName("mskData07")
                                            v_strIaType = "4"
                                        ElseIf ObjectName = "2034" Then
                                            v_strCardNo = GetValueControlByName("mskData06")
                                            v_strCardDate = GetValueControlByName("mskData07")
                                            v_strIaType = "6"
                                        End If
                                        v_frm.CardNo = v_strCardNo
                                        v_frm.CardDate = v_strCardDate
                                        v_frm.IaType = v_strIaType
                                        v_frm.Proxy = Proxy
                                        v_frm.ShowDialog()
                                        ActiveControl.Text = v_frm.ReturnValue
                                    Case Else

                                        SetLookUpDataForm()
                                        mv_frmSearchScreen = New frmSearch(Me.mv_strLanguage)
                                        mv_frmSearchScreen.TableName = mv_arrObjFields(v_intIndex).SearchCode
                                        mv_frmSearchScreen.ModuleCode = mv_arrObjFields(v_intIndex).SrModCode
                                        mv_frmSearchScreen.AuthCode = "NNNNYYNNNN" 'Chá»‰ cho phÃ©p thá»±c hiá»‡n Close vÃ  View. TÃ­nh nÄƒng nÃ y cáº§n nÃ¢ng cáº¥p Ä‘á»ƒ kiá»ƒm tra quyá»?n
                                        mv_frmSearchScreen.AuthString = "NNNNYY" 'Chá»‰ cho phÃ©p thá»±c hiá»‡n Close vÃ  View. TÃ­nh nÄƒng nÃ y cáº§n nÃ¢ng cáº¥p Ä‘á»ƒ kiá»ƒm tra quyá»?n
                                        mv_frmSearchScreen.IsLocalSearch = Me.LocalObject
                                        mv_frmSearchScreen.IsLookup = "Y"
                                        mv_frmSearchScreen.SearchOnInit = False
                                        mv_frmSearchScreen.BranchId = Me.BranchId
                                        mv_frmSearchScreen.TellerId = Me.TellerId
                                        mv_frmSearchScreen.MemberFilter = mv_strMemberFilter
                                        mv_frmSearchScreen.StockFilter = mv_strStockFilter
                                        mv_frmSearchScreen.AllMember = mv_lngAllMember
                                        mv_frmSearchScreen.AllStock = mv_lngAllStock
                                        mv_frmSearchScreen.PassDate = PassDate
                                        mv_frmSearchScreen.BusDate = BusDate
                                        mv_frmSearchScreen.CloseFormWhenDoubleClick = True

                                        v_intCount = mv_arrObjFields.GetLength(0) - 1
                                        strSharp = "" & v_intCount & Chr(1)
                                        For v_intSharp = 0 To v_intCount - 1 Step 1
                                            v_strSharp = ""
                                            If mv_arrObjFields(v_intSharp).Visible = True Then
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
                                            End If
                                        Next
                                        mv_frmSearchScreen.Sharp = strSharp
                                        mv_frmSearchScreen.PARENT_TLTXCD = Trim(mskTransCode.Text)
                                        mv_frmSearchScreen.Proxy = Proxy
                                        mv_frmSearchScreen.Client = Client
                                        mv_frmSearchScreen.InitDialog()

                                        Dim v_strCmdSql As String
                                        v_strCmdSql = mv_frmSearchScreen.CMDSQL

                                        If strSharp <> "" Then
                                            Dim v_intBegin, v_intEnd As Integer
                                            v_intCount = strSharp.Substring(0, strSharp.IndexOf(Chr(1)))
                                            For v_intSharp = 1 To v_intCount
                                                v_strSharp = ""
                                                v_intBegin = InStr(strSharp, "#" & Format(v_intSharp, "00") & Chr(1)) + 3
                                                v_intEnd = InStr(v_intBegin + 1, strSharp, Chr(1)) - v_intBegin - 1
                                                v_strSharp = strSharp.Substring(v_intBegin, v_intEnd)

                                                If Trim(v_strSharp) = "" And InStr(v_strCmdSql, "#" & Format(v_intSharp, "00"), CompareMethod.Text) > 0 Then
                                                    v_strMsgErr = "Bạn chưa nhập " & mv_arrObjFields(v_intSharp).Caption
                                                    Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Visible = True
                                                    Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Text = v_strMsgErr
                                                    Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).ForeColor = System.Drawing.Color.Red
                                                    Exit Sub
                                                End If

                                                v_strCmdSql = v_strCmdSql.Replace("#" & Format(v_intSharp, "00"), v_strSharp)
                                            Next
                                        End If
                                        v_strCmdSql = v_strCmdSql.Replace("?TLID", mv_strTellerId)
                                        v_strCmdSql = v_strCmdSql.Replace("?TLTXCD", ObjectName)
                                        mv_frmSearchScreen.CMDSQL = v_strCmdSql

                                        mv_frmSearchScreen.ShowDialog()

                                        If Not mv_frmSearchScreen.ReturnValue Is Nothing Then
                                            Me.ActiveControl.Text = mv_frmSearchScreen.ReturnValue
                                            If Len(mv_frmSearchScreen.RefValue) > 0 Then
                                                ctl = Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex)
                                                ctl.Top = Me.ActiveControl.Top
                                                ctl.ForeColor = Drawing.Color.Blue
                                                ctl.Text = mv_frmSearchScreen.RefValue
                                                ctl.Visible = True
                                            End If
                                            'Náº¡p cÃ¡c giÃ¡ trá»‹ tÆ°Æ¡ng á»©ng cho cÃ¡c trÆ°á»?ng khÃ¡c
                                            strFLDNAME = Mid(ActiveControl.Name, Len(PREFIXED_MSKDATA) + 1)
                                            FillLookupData(strFLDNAME, mv_frmSearchScreen.ReturnValue, mv_frmSearchScreen.FULLDATA, mv_frmSearchScreen.KeyColumn)
                                            mv_frmSearchScreen.Dispose()
                                        End If
                                End Select
                            End If
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
                                If (InStr(strSQL.ToUpper, "RGSI") > 0) Then
                                    strSQL = "select * from (" & strSQL & ") where brid ='" & Me.BranchId & "' and deleted=0 and status=0"
                                End If
                                v_intCount = mv_arrObjFields.GetLength(0) - 1
                                For v_intSharp = 0 To v_intCount - 1 Step 1
                                    v_strSharp = ""
                                    If mv_arrObjFields(v_intSharp).Visible = True Then
                                        Select Case Trim(mv_arrObjFields(v_intSharp).ControlType)
                                            Case "T"
                                                v_strSharp = CType(Me.pnTransDetail.Controls(mv_arrObjFields(v_intSharp).ControlIndex), TextBox).Text
                                            Case "M"
                                                v_strSharp = CType(Me.pnTransDetail.Controls(mv_arrObjFields(v_intSharp).ControlIndex), FlexMaskEditBox).Text
                                                v_strSharp = IIf(Trim(mv_arrObjFields(v_intSharp).DataType) = "D", v_strSharp.Replace("/  /", ""), v_strSharp)
                                            Case "C"
                                                v_strSharp = CType(Me.pnTransDetail.Controls(mv_arrObjFields(v_intSharp).ControlIndex), ComboBoxEx).SelectedValue.ToString
                                        End Select
                                        If Trim(v_strSharp) = "" And InStr(strSQL, "#" & Format(v_intSharp, "00"), CompareMethod.Text) > 0 Then
                                            v_strMsgErr = "Bạn chưa nhập " & mv_arrObjFields(v_intSharp).Caption
                                            Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Visible = True
                                            Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Text = v_strMsgErr
                                            Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).ForeColor = System.Drawing.Color.Red
                                            Exit Sub
                                        End If
                                        strSQL = strSQL.Replace("#" & Format(v_intSharp + 1, "00"), v_strSharp.Trim)
                                    End If
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
                                v_ofdOpenFile.Filter = "Excel(*.xls)|*.xls|File XML(*.xml)|*.xml|File Text(*.txt)|*.txt|All files (*.*)|*.*"
                                v_ofdOpenFile.ShowDialog()
                                Me.ActiveControl.Text = v_ofdOpenFile.FileName
                            End If
                        End If
                    ElseIf Me.ActiveControl.Name = "mskTransCode" Then
                        'Hiá»ƒn thá»‹ danh sÃ¡ch giao dá»‹ch cá»§a nhÃ³m nghiá»‡p vá»¥ tÆ°Æ¡ng á»©ng
                        Dim frm As New frmLookUp(UserLanguage)

                        frm.SQLCMD = "SELECT a.TLTXCD VALUE, a.TLTXCD VALUECD, a.TLTXCD || '- ' || a.TXDESC DISPLAY, a.EN_TXDESC EN_DISPLAY, c.modname DESCRIPTION FROM TLTX a, " _
                                & " ( " _
                                & " select c.tltxcd from tlauth c where c.status =0 and c.deleted =0 and c.authtype = 'U' and c.authid = '" & mv_strTellerId & "' and c.tltype = '0' " _
                                & " union" _
                                & " select c.tltxcd from tlauth c where c.status =0 and c.deleted =0 and c.authtype = 'G'" _
                                & " and c.authid in ( select grpid from tlgrpusers d where d.tlid = '" & mv_strTellerId & "' and d.status =0 and d.deleted =0 ) and c.tltype = '0'" _
                                & " ) b, appmodules c " _
                                & " where a.status =0 and a.deleted =0 and a.tltxcd = b.tltxcd and substr(a.tltxcd,1,2) = c.txcode and c.deleted=0 and c.status = 0 "
                        frm.TellerID = TellerId
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
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                                     & "Error code: System error!" & vbNewLine _
                                                     & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
            mv_bolCheck = False
        End Try
    End Sub

    Private Sub frmTransaction_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'mv_BDSDelivery = New BDSChannel.BDSDelivery
        OnInit()
    End Sub
    Private Function getHSMSignature(ByVal v_strExportFilePath As String, ByVal mv_strBrid As String, ByVal mv_strCurrDate As String) As String
        Dim v_Prefix As String
        Dim v_strCurrDate As String = mv_strCurrDate.Replace("/", "")
        Dim v_strPath As String = _
            v_strExportFilePath & v_strCurrDate
        Dim v_fStream As System.IO.FileStream
        Try
            Select Case mv_strBrid
                Case "0001"
                    v_Prefix = "LISTED"
                Case "0003"
                    v_Prefix = "UPCOM"
                Case "0004"
                    v_Prefix = "BOND"
                Case "0005"
                    v_Prefix = "USDBOND"
                Case "0006"
                    v_Prefix = "BILL_VND"
                    'bangpv add TPRL 
                Case "0008"
                    v_Prefix = "HNX_BOND_CORP"
                Case "0009"
                    v_Prefix = "HNX_CACBON"
            End Select
            'add TPRL 
            Dim v_dtCurrDate As DateTime = DateTime.ParseExact(mv_strCurrDate, "dd/MM/yyyy", Globalization.CultureInfo.InvariantCulture)
            Dim v_strCurrDateTPRL As String = v_dtCurrDate.ToString("yyyyMMdd", Globalization.CultureInfo.InvariantCulture)
            'end TPRL 
            Dim v_strCurrDateCacbon As String = v_dtCurrDate.ToString("yyyyMMdd", Globalization.CultureInfo.InvariantCulture)


            Dim v_strFullEncyptedFileName As String = v_strPath & "\" & v_Prefix & v_strCurrDate & ".zip"
            'TPRL
            If mv_strBrid = "0008" Then
                v_strFullEncyptedFileName = v_strPath & "\" & v_Prefix & "_" & v_strCurrDateTPRL & ".zip"
            End If
            'TPRL
            'Cacbon
            If mv_strBrid = "0009" Then
                v_strFullEncyptedFileName = v_strPath & "\" & v_Prefix & "_" & v_strCurrDateCacbon & ".zip"
            End If
            'Cacbon
            v_fStream = System.IO.File.OpenRead(v_strFullEncyptedFileName)
            Dim len As Long = v_fStream.Length

            Dim v_arrSource As Byte() = New Byte(len - 1) {}
            Dim intErr As Long = v_fStream.Read(v_arrSource, 0, len)
            v_fStream.Close()
            v_fStream = Nothing
            'ký số 
            Dim v_strData As String = Convert.ToBase64String(v_arrSource)
            mv_oProxy.FileCA(v_strData, mv_strBrid, "9999", mv_strCurrDate)
            Return v_strData
            'Convert.FromBase64String(v_strDataFileServer)

        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                                     & "Error code: System error!getHSMSignature" & vbNewLine _
                                                     & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
            Return ""
        End Try
    End Function


    Protected Overridable Sub OnSubmit()
        Dim v_strTXNUM As String
        Dim v_strTXDATE As String
        Dim v_blnchk As Boolean = False
        Dim v_nodeList As Xml.XmlNodeList
        v_oProcess = New ProcessForm(Me.ParentForm)
        Try
            'Hanm5 sửa thêm phần chọn nơi nhận chứng từ gốc
            If mv_strISPARENT = 2 And mv_strCHKID = 1 Then
                Dim o_frm As New frmSelectVsdBrid
                o_frm.TellerID = TellerId
                o_frm.BRID = Me.BranchId
                o_frm.Proxy = Proxy
                If Not o_frm.OnInit() Then
                    o_frm.ShowDialog()
                End If
                If o_frm.SELECTED <> "" Then
                    mv_strVsdBrid = o_frm.SELECTED
                    VSDBRID = mv_strVsdBrid
                    Application.DoEvents()
                Else
                    o_frm.Dispose()
                    Exit Sub
                End If
                o_frm.Dispose()
            End If
            'Hanm5 kết thúc sửa
            v_oProcess.StartProcessForm()
            Dim v_strTxMsg As String = "", v_strInitialTxMsg As String = ""

            Dim v_strTLTXCD As String
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
                'v_thread.Start()
                v_strTLTXCD = Trim(mskTransCode.Text)
                ' 1. Check FLDVAL
                v_oProcess.ChangeCaption("Kiểm tra tính hợp lệ của dữ liệu ...")
                Application.DoEvents()
                LogError.Write("Begin VerifyRules onSubmit: " & Format(Now, "HH:mm:ss.fff"), EventLogEntryType.Information, gc_MODULE_CLIENT)
                If Not VerifyRules(v_strTxMsg, v_strTLTXCD) Then
                    v_oProcess.StopProcessForm()
                    Application.DoEvents()
                    Exit Sub
                End If
                LogError.Write("End VerifyRules onSubmit: " & Format(Now, "HH:mm:ss.fff"), EventLogEntryType.Information, gc_MODULE_CLIENT)
                ' 2. Accept transaction
                v_strInitialTxMsg = v_strTxMsg
                While True
                    If v_oProcess.Status Then
                        v_oProcess.StartProcessForm()
                    End If
                    v_oProcess.ChangeCaption("Đẩy dữ liệu lên máy chủ xử lý ...")
                    Application.DoEvents()

                    'Client.isActive = True
                    'Client.Action = "Begin Transact: " & lblTransCaption.Text
                    'Proxy.SendAction(Client)
                    ' bangpv: nếu là giao dịch ký số thì dùng MessageCA
                    'Lấy phân quyền giao dịch xem có ký số không
                    'BangPV: Lấy thông tin phân cấp ký số giao dịch
                    'End BangPV
                    Dim v_strSQL As String = "select authid, tltxcd from tlcaauth where tltxcd = '" & ObjectName _
                            & "' and authid in (select grpid from tlgrpusers where tlid ='" & TellerId & "')"

                    Dim v_strObjMsg As String = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLGROUPS, gc_ActionInquiry, v_strSQL)

                    v_lngError = Proxy.Message(v_strObjMsg)
                    'Dim v_nodeList As Xml.XmlNodeList

                    If v_lngError <> ERR_SYSTEM_OK Then
                        MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                        Exit Sub
                    End If
                    v_xmlDocument.LoadXml(v_strObjMsg)
                    v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")
                    'Hanm5: Them giao dich lay gia TP cho HOSE
                    If v_strTLTXCD = "2130" And mv_strBrid = "0002" Then
                        Dim v_strImportFilePath As String = GetImportFilePath()
                        Dim v_strTradeDate As String = GetValueControlByName("mskData01")
                        Dim v_strPriceType As String = GetValueControlByName("mskData03")
                        Dim v_strEncryptedFile As String = ClientBussinessCA.EncryptFile2130(v_strImportFilePath, mv_strBrid, v_strTradeDate, Proxy.mv_oSessionKey, v_strPriceType)

                        If (v_strEncryptedFile & "" = "") Then
                            MsgBox("Chưa có file để chuyển", MsgBoxStyle.Critical)
                            Exit Sub
                        End If
                        'Bangpv: add price type for hsx 
                        SendFileFTP(v_strImportFilePath & mv_strBrid, "2130" & mv_strBrid & v_strTradeDate.Replace("/", "") & "_" & v_strPriceType & ".xml", mv_strBrid)
                    End If

                    'Hanm5: Them 2 giao dich 2150 va 2151 phuc vu trao doi file giua SBL va NHTT
                    If v_strTLTXCD = "2150" Or v_strTLTXCD = "2151" Then
                        Dim v_strBrid As String = GetValueControlByName("mskData01")
                        Dim v_strTradeDate As String = GetValueControlByName("mskData02")
                        Dim v_strFileName As String = GetValueControlByName("mskData04")

                        Dim v_strEncryptedFile As String = ClientBussinessCA.EncryptFileSblFtpBnk(v_strFileName, mv_strBrid, v_strTradeDate, Proxy.mv_oSessionKey, v_strTLTXCD)

                        If (v_strEncryptedFile & "" = "") Then
                            MsgBox("Chưa có file để chuyển", MsgBoxStyle.Critical)
                            Exit Sub
                        End If
                        SendFileSblFtpBnk("SBLFTPBNK", v_strBrid, v_strEncryptedFile)
                    End If
                    'start Myvq
                    If (v_strTLTXCD = "1113") And mv_strBrid = "0002" Then

                        Dim v_strImportFilePath As String = _
                            GetImportFilePath()
                        Dim v_strEncryptedFile As String = _
                            ClientBussinessCA.EncryptFile1113(v_strImportFilePath, _
                                    mv_strBrid, mv_strCurrDate, Proxy.mv_oSessionKey)

                        If (v_strEncryptedFile Is Nothing) Or (v_strEncryptedFile = "") Then
                            MsgBox("Chưa có file để chuyển", MsgBoxStyle.Critical)
                            Exit Sub
                        End If

                        SendFileFTP(v_strImportFilePath & mv_strBrid, _
                                "1113" & mv_strBrid & mv_strCurrDate.Replace("/", "") & ".xml", mv_strBrid)

                        'Extract                           
                        'ClientBussinessCA.ExtractFile("C:\Extract", v_strEncryptedFile)

                    End If

                    If (v_strTLTXCD = "1112") Then
                        'Try
                        Dim v_strExportFilePath As String = _
                            GetExportFilePath()
                        'Lấy chữ ký số hsm. 
                        Dim v_strVSDSignature As String
                        If mv_strBrid <> "0002" Then
                            v_strVSDSignature = getHSMSignature(v_strExportFilePath, mv_strBrid, mv_strCurrDate)
                        End If
                        Dim v_strEncryptedFile As String = _
                            ClientBussinessCA.EncryptFile1112(v_strExportFilePath, _
                                    mv_strBrid, mv_strCurrDate, Proxy.mv_oSessionKey, v_strVSDSignature)

                        If (v_strEncryptedFile Is Nothing) Or (v_strEncryptedFile = "") Then
                            MsgBox("Không có file đúng định dạng trong thư mục: """ & v_strExportFilePath & mv_strCurrDate.Replace("/", "") & """", _
                                   MsgBoxStyle.Exclamation, "Thông báo")
                            Exit Sub
                        End If
                        If mv_strBrid = "0002" Then
                            SendFileFTP(v_strExportFilePath & mv_strCurrDate.Replace("/", ""), _
                                        "1112" & mv_strBrid & mv_strCurrDate.Replace("/", "") & ".xml", mv_strBrid)
                        Else
                            SendFileFTP(v_strExportFilePath & mv_strCurrDate.Replace("/", ""), _
                            Replace(v_strEncryptedFile, v_strExportFilePath & mv_strCurrDate.Replace("/", "") & "\", ""), mv_strBrid)
                            v_strObjMsg = BuildXMLObjMsg(BusDate, mv_strBrid, , Me.TellerId, gc_IsNotLocalMsg, gc_MsgTypeObj, OBJNAME_SY_AUTHENTICATION, gc_ActionInquiry, _
                                                         , RemotePath & ":" & Replace(v_strEncryptedFile, v_strExportFilePath & mv_strCurrDate.Replace("/", "") & "\", ""), "SendFTPtoHNX", , )
                            v_lngError = Proxy.Message(v_strObjMsg)

                            If v_lngError <> ERR_SYSTEM_OK Then
                                MsgBox("Đẩy file lên máy chủ HNX(hoặc HSX) không thành công", MsgBoxStyle.Critical, gc_ApplicationTitle)
                                Cursor = Cursors.Default

                            Else
                                MsgBox("Đẩy file lên máy chủ HNX(hoặc HSX) thành công", MsgBoxStyle.Exclamation, gc_ApplicationTitle)

                            End If
                        End If

                    End If
                    '1132
                    If (v_strTLTXCD = "1132") Then
                        'Try
                        Dim v_strExportFilePath As String = _
                            GetExportFilePath()
                        'Lấy chữ ký số hsm. 
                        Dim v_strVSDSignature As String
                        If mv_strBrid <> "0002" Then
                            v_strVSDSignature = getHSMSignature(v_strExportFilePath, mv_strBrid, mv_strCurrDate)
                        End If
                        Dim v_strEncryptedFile As String = _
                            ClientBussinessCA.EncryptFile1132(v_strExportFilePath, _
                                    mv_strBrid, mv_strCurrDate, Proxy.mv_oSessionKey, v_strVSDSignature)

                        If (v_strEncryptedFile Is Nothing) Or (v_strEncryptedFile = "") Then
                            MsgBox("Không có file đúng định dạng trong thư mục: """ & v_strExportFilePath & mv_strCurrDate.Replace("/", "") & """", _
                                   MsgBoxStyle.Exclamation, "Thông báo")
                            Exit Sub
                        End If
                        If mv_strBrid = "0002" Then
                            SendFileFTP(v_strExportFilePath & mv_strCurrDate.Replace("/", ""), _
                                        "1132" & mv_strBrid & mv_strCurrDate.Replace("/", "") & ".xml", mv_strBrid)
                        Else
                            SendFileFTP(v_strExportFilePath & mv_strCurrDate.Replace("/", ""), _
                            Replace(v_strEncryptedFile, v_strExportFilePath & mv_strCurrDate.Replace("/", "") & "\", ""), mv_strBrid)
                            v_strObjMsg = BuildXMLObjMsg(BusDate, mv_strBrid, , Me.TellerId, gc_IsNotLocalMsg, gc_MsgTypeObj, OBJNAME_SY_AUTHENTICATION, gc_ActionInquiry, _
                                                         , RemotePath & ":" & Replace(v_strEncryptedFile, v_strExportFilePath & mv_strCurrDate.Replace("/", "") & "\", ""), "SendFTPtoHNX", , )
                            v_lngError = Proxy.Message(v_strObjMsg)

                            If v_lngError <> ERR_SYSTEM_OK Then
                                MsgBox("Đẩy file lên máy chủ HNX(hoặc HSX) không thành công", MsgBoxStyle.Critical, gc_ApplicationTitle)
                                Cursor = Cursors.Default

                            Else
                                MsgBox("Đẩy file lên máy chủ HNX(hoặc HSX) thành công", MsgBoxStyle.Exclamation, gc_ApplicationTitle)

                            End If
                        End If

                    End If
                    If (v_strTLTXCD = "1129") Then
                        'Try
                        Dim v_strExportFilePath As String = _
                            GetExportFilePath()
                        Dim v_strEncryptedFile As String = _
                            ClientBussinessCA.EncryptFile1129_1130(mv_strFileNameCA, mv_strFileNameCA1, _
                                    mv_strBrid, mv_strCurrDate, Proxy.mv_oSessionKey, "1129")

                        If (v_strEncryptedFile Is Nothing) Or (v_strEncryptedFile = "") Then
                            MsgBox("Không có file đúng định dạng trong thư mục: """ & v_strExportFilePath & mv_strCurrDate.Replace("/", "") & """", _
                                   MsgBoxStyle.Exclamation, "Thông báo")
                            Exit Sub
                        End If

                        SendFileFTP(System.AppDomain.CurrentDomain.BaseDirectory & "\Log", _
                                       Replace(v_strEncryptedFile, System.AppDomain.CurrentDomain.BaseDirectory & "\Log\", ""), mv_strBrid)
                        v_strObjMsg = BuildXMLObjMsg(BusDate, mv_strBrid, , Me.TellerId, gc_IsNotLocalMsg, gc_MsgTypeObj, OBJNAME_SY_AUTHENTICATION, gc_ActionInquiry, _
                                                     , RemotePath & ":" & Replace(v_strEncryptedFile, System.AppDomain.CurrentDomain.BaseDirectory & "\Log\", ""), "SendFTPtoHNX", , )
                        v_lngError = Proxy.Message(v_strObjMsg)

                        If v_lngError <> ERR_SYSTEM_OK Then
                            MsgBox("Đẩy file lên máy chủ HNX(hoặc HSX) không thành công", MsgBoxStyle.Critical, gc_ApplicationTitle)
                            Cursor = Cursors.Default

                        Else
                            MsgBox("Đẩy file lên máy chủ HNX(hoặc HSX) thành công", MsgBoxStyle.Exclamation, gc_ApplicationTitle)

                        End If


                    End If

                    If (v_strTLTXCD = "1114") Then
                        'Try
                        Dim v_strExportFilePath As String = _
                            GetExportFilePath()
                        Dim v_strEncryptedFile As String = _
                            ClientBussinessCA.EncryptFile1114(v_strExportFilePath, _
                                    mv_strBrid, mv_strDate1114, Proxy.mv_oSessionKey, v_strTLTXCD)

                        If (v_strEncryptedFile Is Nothing) Or (v_strEncryptedFile = "") Then
                            MsgBox("Không có file đúng định dạng trong thư mục: """ & v_strExportFilePath & mv_strDate1114.Replace("/", "") & """", _
                                   MsgBoxStyle.Exclamation, "Thông báo")
                            Exit Sub
                        End If

                        SendFileFTP(v_strExportFilePath & mv_strDate1114.Replace("/", ""), _
                                    v_strTLTXCD & mv_strBrid & mv_strDate1114.Replace("/", "") & ".xml", mv_strBrid)

                    End If
                    If (v_strTLTXCD = "1150") Then
                        'Try
                        Dim v_strExportFilePath As String = _
                            GetExportFilePath()
                        Dim v_strEncryptedFile As String = _
                            ClientBussinessCA.EncryptFile1114(v_strExportFilePath, _
                                    mv_strBrid, mv_strDate1114, Proxy.mv_oSessionKey, v_strTLTXCD, mv_intCSEXPType, mv_strBusDate)

                        If (v_strEncryptedFile Is Nothing) Or (v_strEncryptedFile = "") Then
                            MsgBox("Không có file đúng định dạng trong thư mục: """ & v_strExportFilePath & mv_strDate1114.Replace("/", "") & """", _
                                   MsgBoxStyle.Exclamation, "Thông báo")
                            Exit Sub
                        End If

                        SendFileFTP(v_strExportFilePath & mv_strDate1114.Replace("/", ""), _
                                    Replace(v_strEncryptedFile, v_strExportFilePath & mv_strDate1114.Replace("/", "") & "\", ""), mv_strBrid)

                    End If


                    If (v_strTLTXCD = "1125") Then
                        'Try

                        Dim v_strEncryptedFile As String = _
                            ClientBussinessCA.EncryptFile1114(mv_strFileNameCA, _
                                    mv_strBrid, mv_strDate1114, Proxy.mv_oSessionKey, v_strTLTXCD)

                        If (v_strEncryptedFile Is Nothing) Or (v_strEncryptedFile = "") Then
                            MsgBox("Không có file đúng định dạng trong thư mục: """ & mv_strFileNameCA & mv_strDate1114.Replace("/", "") & """", _
                                   MsgBoxStyle.Exclamation, "Thông báo")
                            Exit Sub
                        End If

                        SendFileFTP("C:\ExportFile\Log", _
                                      Replace(v_strEncryptedFile, "C:\ExportFile\Log\", ""), mv_strBrid)

                    End If


                    'end Myvq
                    'bangpv: Giao dich chuyen file cho so 
                    If (v_strTLTXCD = "1123") Then
                        Dim v_strExportFilePath As String = GetExportFilePath()
                        Dim v_strEncryptedFile As String = _
                            ClientBussinessCA.EncryptFile1123(mv_strFileNameCA, mv_strFileNameCA1, _
                                    mv_strBrid, mv_strCurrDate, Proxy.mv_oSessionKey)

                        If (v_strEncryptedFile Is Nothing) Or (v_strEncryptedFile = "") Then
                            MsgBox("Không có file đúng định dạng trong thư mục: """ & v_strExportFilePath & mv_strCurrDate.Replace("/", "") & """", _
                                   MsgBoxStyle.Exclamation, "Thông báo")
                            Exit Sub
                        End If
                        SendFileFTP(System.AppDomain.CurrentDomain.BaseDirectory & "\Log", _
                                       Replace(v_strEncryptedFile, System.AppDomain.CurrentDomain.BaseDirectory & "\Log\", ""), mv_strBrid)
                    End If

                    If (v_strTLTXCD = "1124") Then

                        Dim v_strExportFilePath As String = _
                            GetExportFilePath()
                        Dim v_strEncryptedFile As String = _
                            ClientBussinessCA.EncryptFile1124(mv_strFileNameCA, mv_strFileNameCA1, _
                                    mv_strBrid, mv_strCurrDate, Proxy.mv_oSessionKey)

                        If (v_strEncryptedFile Is Nothing) Or (v_strEncryptedFile = "") Then
                            MsgBox("Không có file đúng định dạng trong thư mục: """ & v_strExportFilePath & mv_strCurrDate.Replace("/", "") & """", _
                                   MsgBoxStyle.Exclamation, "Thông báo")
                            Exit Sub
                        End If

                        SendFileFTP(System.AppDomain.CurrentDomain.BaseDirectory & "\Log", _
                                       Replace(v_strEncryptedFile, System.AppDomain.CurrentDomain.BaseDirectory & "\Log\", ""), mv_strBrid)


                    End If

                    'end bangpv
                    'Added by Thanglv9 - 13/04/2013
                    '1131
                    If (v_strTLTXCD = "1131") Then
                        Dim v_strSysTime = DateTime.Now.ToString("HH:mm:ss").Replace(":", "")
                        Dim v_strExportFilePath As String = _
                            GetExportFilePath()
                        Dim v_strEncryptedFile As String = _
                            ClientBussinessCA.EncryptFileNHNN(mv_strFileNameCA, _
                                    mv_strBranchId, mv_strCurrDate, v_strSysTime, Proxy.mv_oSessionKey)

                        If (v_strEncryptedFile Is Nothing) Or (v_strEncryptedFile = "") Then
                            MsgBox("Không có file đúng định dạng trong thư mục: """ & v_strExportFilePath & mv_strCurrDate.Replace("/", "") & """", _
                                   MsgBoxStyle.Exclamation, "Thông báo")
                            Exit Sub
                        End If

                        SendFileFTPSBV(System.AppDomain.CurrentDomain.BaseDirectory & "\Log", _
                                       Replace(v_strEncryptedFile, System.AppDomain.CurrentDomain.BaseDirectory & "\Log\", ""), mv_strBranchId)

                        Dim pv_xmlDocument As New Xml.XmlDocument

                        pv_xmlDocument.LoadXml(v_strTxMsg)

                        pv_xmlDocument.DocumentElement.Attributes(gc_AtributeStateFile).Value = v_strSysTime

                        v_strTxMsg = pv_xmlDocument.InnerXml


                    End If
                    'End Thanglv9

                    If mv_strSignCA = "1" And v_nodeList.Count > 0 Then
                        v_lngError = Proxy.MessageCA(v_strTxMsg)
                    Else
                        v_lngError = Proxy.Message(v_strTxMsg)
                    End If

                    'If Not Client Is Nothing Then
                    '    Client.Action = "End Transact: " & lblTransCaption.Text
                    '    Client.isActive = False
                    '    Proxy.SendAction(Client)
                    'End If

                    If v_lngError <> ERR_SYSTEM_OK Then
                        v_oProcess.StopProcessForm()
                        Application.DoEvents()
                        Dim frm As New frmErrMsg
                        v_xmlDocument.LoadXml(v_strTxMsg)
                        v_nodeList = v_xmlDocument.SelectNodes("/TransactMessage/FAILED_MESSAGE")
                        Dim v_strValue, v_strFLDNAME, v_strCount As String
                        Dim v_strErrMsg, v_strWarningMsg As String
                        Dim v_intErr, v_intWarning As Integer

                        v_strErrMsg = ""
                        v_strWarningMsg = ""
                        v_intErr = 0
                        v_intWarning = 0

                        For i = 0 To v_nodeList.Count - 1
                            v_strTXNUM = CStr(CType(v_nodeList.Item(i).Attributes.GetNamedItem(gc_AtributeTXNUM), Xml.XmlAttribute).Value)
                            v_strTXDATE = CStr(CType(v_nodeList.Item(i).Attributes.GetNamedItem(gc_AtributeTXDATE), Xml.XmlAttribute).Value)
                            For j = 0 To v_nodeList.Item(i).ChildNodes.Count - 1
                                With v_nodeList.Item(i).ChildNodes(j)
                                    v_strValue = .InnerText.ToString
                                    v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem(gc_AtributeFLDNAME), Xml.XmlAttribute).Value)
                                    v_strCount = CStr(CType(.Attributes.GetNamedItem("COUNT"), Xml.XmlAttribute).Value)
                                    If v_strValue.Trim <> "" Then
                                        If v_strFLDNAME = "ERROR_MESSAGE" Then
                                            If v_strTLTXCD = "1109" Or v_strTLTXCD = "1110" Or v_strTLTXCD = "1127" Or v_strTLTXCD = "1128" Or v_strTLTXCD = "3079" Then
                                                v_strErrMsg &= .InnerText.ToString
                                                v_intErr += CInt(v_strCount)
                                            Else
                                                v_strErrMsg &= i + 1 & ". Giao dịch (" & v_strTXNUM & ", " & v_strTXDATE & ") có " & v_strCount & " lỗi :" & vbCrLf & .InnerText.ToString
                                                v_intErr += CInt(v_strCount)
                                            End If

                                        ElseIf v_strFLDNAME = "WARNING_MESSAGE" Then
                                            v_strWarningMsg &= i + 1 & ". Giao dịch (" & v_strTXNUM & ", " & v_strTXDATE & ") có " & v_strCount & " cảnh báo :" & vbCrLf & .InnerText.ToString
                                            v_intWarning += CInt(v_strCount)
                                        End If
                                    End If
                                End With
                            Next
                        Next

                        If v_intWarning + v_intErr > 0 Then
                            frm.ErrMsg = v_strErrMsg
                            'bangpv
                            If (v_strTLTXCD = "1109" Or v_strTLTXCD = "1110" Or v_strTLTXCD = "1127" Or v_strTLTXCD = "1128" Or v_strTLTXCD = "3079") _
                                                                                        And (InStr(v_strErrMsg, "Dữ liệu hợp lệ") > 0 Or InStr(v_strErrMsg, "Giao dịch thành công") > 0) Then
                                frm.ErrCount = 0
                                If v_strTLTXCD = "3079" Then
                                    v_blnchk = True
                                End If
                            Else
                                frm.ErrCount = v_intErr
                            End If

                            'end bangpv 
                            frm.WarningMsg = v_strWarningMsg
                            frm.WarningCount = v_intWarning

                            'Threading.Thread.Sleep(200)
                            If frm.ShowDialog() = Windows.Forms.DialogResult.Cancel Then
                                mv_bolCheck = False
                                If v_blnchk = True Then
                                    ResetScreen()
                                End If
                                Exit Sub
                            Else
                                v_strTxMsg = v_strInitialTxMsg.Replace(gc_AtributeMISSING_WARNING & "=""0""", gc_AtributeMISSING_WARNING & "=""1""")
                            End If
                        Else
                            'v_oProcess.StopProcessForm()
                            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                            Exit Sub
                        End If
                    Else
                        v_oProcess.StopProcessForm()
                        Application.DoEvents()
                        'HaNM5 bo sung loai bo giao dich 4056 va 4062 khong hien giao dich con ra
                        If mv_strISPARENT = 2 And mv_intVisible_Child = 0 And v_strTLTXCD <> "4056" And v_strTLTXCD <> "4062" Then ' nhap cha
                            Application.DoEvents()
                            'Threading.Thread.Sleep(3000)
                            Dim frm As New frmSelectTLTX
                            frm.TLTXCD = mv_strCHILDTLTXCD
                            frm.TellerID = TellerId
                            frm.Proxy = Proxy
                            frm.BRID = Me.BranchId
                            If Not frm.OnInit() Then
                                frm.ShowDialog()
                            End If
                            If frm.SELECTED <> "" Then
                                mv_bolCheck = False
                                v_oProcess.StartProcessForm()
                                ResetScreen()
                                mskTransCode.Text = frm.SELECTED
                                v_xmlDocument.LoadXml(v_strTxMsg)
                                v_strTXNUM = v_xmlDocument.DocumentElement.Attributes(gc_AtributeTXNUM).Value.ToString
                                LoadScreen(frm.SELECTED)
                                Me.pnTransDetail.Controls(mv_arrObjFields(0).ControlIndex).Select()
                                ActiveControl.Text = v_strTXNUM
                                ObjectName = frm.SELECTED
                                SelectNextControl(ActiveControl, True, True, True, True)
                                'v_thread.Abort()
                                'BằngPV - Lấy file kết quả giao dịch với giao dịch 4001
                                'Ca đã có giao dịch lấy file về nên ko dùng đoạn này nữa 
                                'If v_strTLTXCD = "4001" And Me.BranchId <> "0002" Then
                                '    GetFileFTP(Me.BranchId)
                                'End If
                                'BằngPV: Thay đổi giải pháp, với HNX vẫn dùng get file ftp
                                'End BằngPV
                                v_oProcess.StopProcessForm()
                                Application.DoEvents()
                            End If
                            frm.Dispose()
                        ElseIf mv_strCOTLTXCD = "" And mv_intVisible_Child = 0 Then ' nhap gd le hoac con
                            v_oProcess.StopProcessForm()
                            Application.DoEvents()
                            If MsgBox(IIf(Me.UserLanguage = "VN", "Giao dịch được chấp nhận. Bạn có muốn giữ nguyên màn hình không ?", "This transaction is accepted !"), MsgBoxStyle.YesNo, "Thông báo") = MsgBoxResult.Yes Then
                                mv_bolCheck = False
                            Else
                                mv_bolCheck = False
                                ResetScreen()
                            End If
                        Else ' nhap file
                            v_oProcess.StopProcessForm()
                            Application.DoEvents()
                            MsgBox(IIf(Me.UserLanguage = "VN", "Giao dịch được chấp nhận.", "This transaction is accepted !"), MsgBoxStyle.OkOnly, "Thông báo")
                            mv_bolCheck = False
                            ResetScreen()
                        End If
                        Exit Sub
                    End If
                End While
            End If
            LogError.Write("End onSubmit: " & Format(Now, "HH:mm:ss.fff"), EventLogEntryType.Information, gc_MODULE_CLIENT)
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
            Me.ParentForm.Focus()
        End Try
    End Sub
    Public Property AutoClosedWhenOK() As Boolean
        Get
            Return mv_isAutoClosedWhenOK
        End Get
        Set(ByVal Value As Boolean)
            mv_isAutoClosedWhenOK = Value
        End Set
    End Property
    Private Sub GetFileFTP(ByVal v_strBrId As String)
        Dim ServerAddress As String
        Dim ServerPort As String
        Dim Username As String
        Dim Password As String
        Dim RemotePath As String
        Dim v_strClause As String
        Dim v_strSQL As String
        Dim v_strObjMsg As String
        Dim v_xmlDocument As New XmlDocumentEx
        Dim v_strTxDate As String
        Dim v_dtIndex As Date
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
        v_strTxDate = CStr(Format(v_dtIndex, "ddMMyyyy"))

        'Lấy thông số FTP
        v_strSQL = "SELECT a.VARNAME, a.VARVALUE FROM sysvar a where a.GRNAME='GETFTP' and BRID='" & BranchId & "'"
        v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLTX, gc_ActionInquiry, v_strSQL)
        v_lngError = Proxy.Message(v_strObjMsg)
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
        v_strClause = ServerAddress & ":" & ServerPort & ":" & Username & ":" & Password & ":" & RemotePath
        v_strObjMsg = BuildXMLObjMsg(v_strTxDate, v_strBrId, , Me.TellerId, gc_IsNotLocalMsg, gc_MsgTypeObj, OBJNAME_SY_AUTHENTICATION, gc_ActionInquiry, , v_strClause, "GETFTP", , )

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
            'MsgBox(mv_ResourceManager.GetString("Success"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, Me.Text)
            Exit Sub
        End If
    End Sub
    Private Sub cboData_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim v_obj As New SQLDataAccessLayer(TellerId)
        'Dim v_ds As DataSet
        Try
            '    Dim strFLDNAME As String, v_intIndex As Integer
            '    Dim v_strSQLCMD As String
            '    Dim v_strFieldValue As String
            '    Dim v_intSharp1, v_intCount As Integer
            '    Dim v_cboData As ComboBoxEx

            '    If InStr(CType(sender, Control).Name, PREFIXED_MSKDATA) > 0 Then
            '        v_intIndex = CType(sender, Control).Tag
            '        If Not mv_arrObjFields(v_intIndex) Is Nothing Then
            '            v_strFieldValue = CType(sender, ComboBoxEx).SelectedValue
            '            strFLDNAME = Mid(CType(sender, Control).Name, Len(PREFIXED_MSKDATA) + 1)
            '            If mv_arrObjFields(v_intIndex).Mandatory = True And InStr(Me.ActiveControl.Name, PREFIXED_MSKDATA) > 0 _
            '                    And (Len(v_strFieldValue) = 0) Then
            '                Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Visible = True
            '                Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Text = mv_ResourceManager.GetString("ERR_LOOKUP_VALUE")
            '                Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).ForeColor = System.Drawing.Color.Red
            '                Exit Sub
            '            End If

            '            If v_strFieldValue <> "" Then
            '                v_strSQLCMD = mv_arrObjFields(v_intIndex).LookupList
            '                v_ds = v_obj.ExecuteReturnDataSet(v_strSQLCMD)
            '                FillLookupData(strFLDNAME, v_strFieldValue, v_ds)
            '            Else
            '                Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Visible = True
            '                Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Text = mv_ResourceManager.GetString("ERR_LOOKUP_VALUE")
            '                Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).ForeColor = System.Drawing.Color.Red
            '                Exit Sub
            '            End If

            '            ' load lai cac combox co #
            '            v_intCount = mv_arrObjFields.GetLength(0) - 1
            '            For v_intSharp1 = 0 To v_intCount - 1 Step 1
            '                If Trim(mv_arrObjFields(v_intSharp1).ControlType) = "C" And v_intSharp1 <> v_intIndex Then
            '                    v_strSQLCMD = mv_arrObjFields(v_intSharp1).LookupList
            '                    If InStr(v_strSQLCMD, "#" & strFLDNAME) > 0 Then
            '                        v_strSQLCMD = v_strSQLCMD.Replace("#" & strFLDNAME, v_strFieldValue)
            '                        v_ds = v_obj.ExecuteReturnDataSet(v_strSQLCMD)

            '                        v_cboData = CType(Me.pnTransDetail.Controls(mv_arrObjFields(v_intSharp1).ControlIndex), ComboBoxEx)
            '                        v_cboData.Text = ""
            '                        v_cboData.BeginUpdate()
            '                        v_cboData.DataSource = v_ds.Tables(0)
            '                        v_cboData.DisplayMember = "DISPLAY"
            '                        v_cboData.ValueMember = "VALUE"
            '                        v_cboData.EndUpdate()
            '                    End If
            '                End If
            '            Next
            '        End If
            '    End If

            Dim v_intIndex As Integer
            v_intIndex = CType(sender, Control).Tag
            mv_lstIsTextChanged(v_intIndex) = True
            Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Text = ""
            Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).ForeColor = System.Drawing.Color.Blue
            For i As Integer = 0 To mv_arrObjFields.Length - 2
                If mv_arrObjFields(i).InvName <> "" Then
                    If mv_arrObjFields(i).InvName.Split("|").Contains(mv_arrObjFields(v_intIndex).FieldName) Then
                        mv_lstIsTextChanged(i) = True And Not mv_blnIsF5(i)
                        mskData_Validating(Me.pnTransDetail.Controls(mv_arrObjFields(i).ControlIndex))
                    End If
                End If
            Next
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                                     & "Error code: System error!" & vbNewLine _
                                                     & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
            mv_bolCheck = False
        Finally
            'v_obj.CloseConnection()
            'v_obj = Nothing
            'v_ds = Nothing
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

    Public Overridable Sub mskData_Validating(ByVal sender As Object, Optional ByVal e As System.ComponentModel.CancelEventArgs = Nothing)
        Dim v_obj As SQLDataAccessLayer
        Dim v_ds As DataSet
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
            Dim v_strMsgErr As String
            Dim v_lngError As Long
            Dim v_bolCheck As Boolean = False

            'If InStr(Me.ActiveControl.Name, PREFIXED_MSKDATA) > 0 Then
            If InStr(CType(sender, Control).Name, PREFIXED_MSKDATA) > 0 And mv_Check Then
                If mv_Check Then
                    v_intIndex = CType(sender, Control).Tag
                    If Not mv_arrObjFields(v_intIndex) Is Nothing Then
                        v_strFieldValue = Trim(CType(sender, Control).Text)
                        v_strFieldValue = IIf(Trim(mv_arrObjFields(v_intIndex).DataType) = "D" Or Trim(mv_arrObjFields(v_intIndex).DataType) = "P", v_strFieldValue.Replace("/  /", ""), v_strFieldValue)
                        'v_strFieldValue = IIf(Trim(mv_arrObjFields(v_intIndex).DataType) = "P", v_strFieldValue.Replace("/  /", ""), v_strFieldValue)
                        strFLDNAME = Mid(CType(sender, Control).Name, Len(PREFIXED_MSKDATA) + 1)

                        'Thông báo phải nhập dữ liệu
                        If mv_arrObjFields(v_intIndex).Mandatory = True And Len(v_strFieldValue) = 0 Then
                            Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Visible = True
                            Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Text = mv_ResourceManager.GetString("ERR_NULL_VALUE")
                            Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).ForeColor = System.Drawing.Color.Red
                            Exit Sub
                        Else
                            Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).ForeColor = System.Drawing.Color.Blue
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
                                    ElseIf CDbl(v_strFieldValue) < 0 And InStr(mv_arrObjFields(v_intIndex).ColumnName, "_N_") = 0 Then
                                        Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Visible = True
                                        Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Text = mv_ResourceManager.GetString("ERR_NUMERIC_VALUE")
                                        Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).ForeColor = System.Drawing.Color.Red
                                        Exit Sub
                                    Else
                                        If Trim(mv_arrObjFields(v_intIndex).ControlType) = "T" Then
                                            FormatNumericTextbox(CType(sender, TextBox))
                                        End If
                                        Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).ForeColor = System.Drawing.Color.Blue
                                    End If
                                Case "D"
                                    If Not IsDateValue(v_strFieldValue) Then
                                        Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Visible = True
                                        Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Text = mv_ResourceManager.GetString("ERR_DATE_VALUE")
                                        Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).ForeColor = System.Drawing.Color.Red
                                        Exit Sub
                                    Else
                                        Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).ForeColor = System.Drawing.Color.Blue
                                    End If
                            End Select

                            ' kiem tra vung du lieu trong file excel co hop le khong 
                            If strFLDNAME = mv_strRange Then
                                If v_strFieldValue.Length < 8 Then
                                    Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Visible = True
                                    Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Text = mv_ResourceManager.GetString("ERR_RANGE_VALUE1")
                                    Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).ForeColor = System.Drawing.Color.Red
                                    Exit Sub
                                Else
                                    Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).ForeColor = System.Drawing.Color.Blue
                                End If

                                v_strFieldValue = v_strFieldValue & " "

                                If v_strFieldValue.Substring(0, 1) = " " _
                                    Or (v_strFieldValue.Substring(0, 1) <> " " And v_strFieldValue.Substring(1, 1) = " ") _
                                    Or (v_strFieldValue.Substring(0, 1) <> " " And v_strFieldValue.Substring(1, 1) >= "A" And v_strFieldValue.Substring(1, 1) <= "Z" And v_strFieldValue.Substring(2, 1) = " ") Then
                                    Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Visible = True
                                    Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Text = mv_ResourceManager.GetString("ERR_RANGE_VALUE1")
                                    Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).ForeColor = System.Drawing.Color.Red
                                    Exit Sub
                                Else
                                    Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).ForeColor = System.Drawing.Color.Blue
                                End If
                                If v_strFieldValue.Substring(6, 1) = " " _
                                    Or (v_strFieldValue.Substring(6, 1) <> " " And v_strFieldValue.Substring(7, 1) = " ") _
                                    Or (v_strFieldValue.Substring(6, 1) <> " " And v_strFieldValue.Substring(7, 1) >= "A" And v_strFieldValue.Substring(7, 1) <= "Z" And v_strFieldValue.Substring(8, 1) = " ") Then
                                    Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Visible = True
                                    Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Text = mv_ResourceManager.GetString("ERR_RANGE_VALUE1")
                                    Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).ForeColor = System.Drawing.Color.Red
                                    Exit Sub
                                Else
                                    Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).ForeColor = System.Drawing.Color.Blue
                                End If

                                Dim v_intStart, v_intFinish As Integer
                                v_intStart = Asc(v_strFieldValue.Substring(0, 1)) _
                                            + IIf(v_strFieldValue.Substring(1, 1) <= "Z" _
                                                  And v_strFieldValue.Substring(1, 1) >= "A" _
                                                  , Asc(v_strFieldValue.Substring(1, 1)) - Asc("A") _
                                                  , 0)
                                'HoaLX3 - Added: Kiem tra neu so cot trong file excel lon hon 25 cot (tu A ~ AA tro di) nhung phai nho hon 50 cot (BA)
                                If (v_strFieldValue.Substring(6, 1) > "A" And v_strFieldValue.Substring(7, 1) >= "A") Then
                                    Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Visible = True
                                    Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Text = mv_ResourceManager.GetString("ERR_RANGE_VALUE")
                                    Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).ForeColor = System.Drawing.Color.Red
                                    Exit Sub
                                ElseIf (v_strFieldValue.Substring(6, 1) = "A" And v_strFieldValue.Substring(7, 1) >= "A") Then
                                    v_intFinish = Asc("Z") + (Asc(v_strFieldValue.Substring(7, 1)) - Asc(v_strFieldValue.Substring(6, 1))) + 1
                                Else
                                    v_intFinish = Asc(v_strFieldValue.Substring(6, 1)) _
                                            + IIf(v_strFieldValue.Substring(7, 1) <= "Z" _
                                                  And v_strFieldValue.Substring(7, 1) >= "A" _
                                                  , Asc(v_strFieldValue.Substring(7, 1)) - Asc("A") _
                                                  , 0)

                                End If

                                If Math.Abs(v_intFinish - v_intStart) + 1 <> mv_arrObjCoFields.Length - 1 Then
                                    Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Visible = True
                                    Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Text = mv_ResourceManager.GetString("ERR_RANGE_VALUE")
                                    Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).ForeColor = System.Drawing.Color.Red
                                    Exit Sub
                                Else
                                    Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).ForeColor = System.Drawing.Color.Blue
                                End If
                            End If
                        End If

                        'Fill du lieu lookup từ HOST
                        If Len(mv_arrObjFields(v_intIndex).SearchCode) > 0 Then
                            If mv_lstIsTextChanged(v_intIndex) Then
                                If v_obj Is Nothing Then
                                    v_obj = New SQLDataAccessLayer(TellerId)
                                End If

                                Dim ctlCheck As Control
                                ctlCheck = Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).ControlIndex)

                                'Xoa DL o nhung con trol lien quan
                                v_intCount = mv_arrObjFields.GetLength(0) - 1
                                For v_intSharp = 0 To v_intCount - 1 Step 1
                                    If mv_arrObjFields(v_intSharp).Visible = True _
                                        And Mid(mv_arrObjFields(v_intSharp).LookupName, 1, 2) = mv_arrObjFields(v_intIndex).FieldName Then
                                        Select Case Trim(mv_arrObjFields(v_intSharp).ControlType)
                                            Case "T", "M"
                                                Me.pnTransDetail.Controls(mv_arrObjFields(v_intSharp).ControlIndex).Text = ""
                                            Case "C"
                                                CType(Me.pnTransDetail.Controls(mv_arrObjFields(v_intSharp).ControlIndex), ComboBoxEx).SelectedIndex = 0
                                        End Select
                                    End If
                                Next

                                If Not (mv_arrObjFields(v_intIndex).Mandatory = False And ctlCheck.Text.Trim.Length = 0) Then
                                    'Kiểm tra dữ liệu nhập vào có đúng không?
                                    Dim v_strSEARCHSQL As String = "", v_strSEARCHCODE As String, v_strRefValue As String = ""
                                    Dim v_strFIELDCODE As String = "", v_strKeyVal As String, v_strKeyName As String = "", v_strOBJNAME As String = ""
                                    Dim v_xmlDocument As New Xml.XmlDocument
                                    v_strSEARCHCODE = mv_arrObjFields(v_intIndex).SearchCode
                                    'bangpv: rao lai, chua hieu vi sao replace doan nay
                                    v_strKeyVal = v_strFieldValue 'Replace(v_strFieldValue, ".", "")
                                    'end bangpv 
                                    'Lấy KeyName
                                    v_strSQLCMD = "SELECT s.searchcode, s.searchtitle, s.en_searchtitle, s.searchcmdsql,s.searchcmdsql1, s.objname, s.frmname, s.orderbycmdsql, sf.position AS POSITION, sf.fieldcode, " _
                                                & " sf.fieldname, sf.en_fieldname, sf.fieldtype, sf.fieldsize, sf.mask AS MASK, sf.operator AS OPERATOR, sf.format, sf.display, sf.srch, sf.[Key] AS [KEY], " _
                                                & " sf.refvalue, s.tltxcd, sf.width, sf.lookupcmdsql, sf.DEFVALUE" _
                                                & " FROM sisearch AS s INNER JOIN" _
                                                & " sisearchfld AS sf ON s.searchcode = sf.searchcode" _
                                                & " WHERE (s.deleted = 0) AND (s.status = 0) AND (sf.deleted = 0) AND (sf.status = 0) AND (upper(s.SEARCHCODE) = '" & v_strSEARCHCODE & "')" _
                                                & " AND ((sf.refvalue='Y') OR (sf.[key]='Y')) ORDER BY POSITION"

                                    v_ds = v_obj.ExecuteReturnDataSet(v_strSQLCMD)
                                    If v_ds.Tables(0).Rows.Count > 0 Then
                                        For i As Integer = 0 To v_ds.Tables(0).Rows.Count - 1
                                            With v_ds.Tables(0)
                                                If .Rows(i)("KEY") = "Y" Then
                                                    v_strKeyName = .Rows(i)("FIELDCODE")
                                                End If
                                                If .Rows(i)("REFVALUE") = "Y" Then
                                                    v_strFIELDCODE = .Rows(i)("FIELDCODE")
                                                End If
                                                v_strSEARCHSQL = .Rows(i)("SEARCHCMDSQL") & .Rows(i)("SEARCHCMDSQL1")
                                            End With
                                        Next
                                        v_strSEARCHSQL = v_strSEARCHSQL.Replace("?LANGUAGE", Me.UserLanguage)
                                        v_strSEARCHSQL = v_strSEARCHSQL.Replace("?COMEMBERFILTER", IIf(mv_lngAllMember = 0, " NVL(a.comicode,'000') in " & mv_strMemberFilter, " 1=1 "))
                                        v_strSEARCHSQL = v_strSEARCHSQL.Replace("?BRID", mv_strBranchId)
                                        v_strSEARCHSQL = v_strSEARCHSQL.Replace("?TLID", mv_strTellerId)
                                        v_strSEARCHSQL = v_strSEARCHSQL.Replace("?TLTXCD", Trim(mskTransCode.Text))
                                        'them rang buoc cac control
                                        v_intCount = mv_arrObjFields.GetLength(0) - 1
                                        For v_intSharp = 0 To v_intCount - 1 Step 1
                                            v_strSharp = ""
                                            If mv_arrObjFields(v_intSharp).Visible = True Then
                                                Select Case Trim(mv_arrObjFields(v_intSharp).ControlType)
                                                    Case "T"
                                                        v_strSharp = CType(Me.pnTransDetail.Controls(mv_arrObjFields(v_intSharp).ControlIndex), TextBox).Text
                                                    Case "M"
                                                        v_strSharp = CType(Me.pnTransDetail.Controls(mv_arrObjFields(v_intSharp).ControlIndex), FlexMaskEditBox).Text
                                                        v_strSharp = IIf(Trim(mv_arrObjFields(v_intSharp).DataType) = "D", v_strSharp.Replace("/  /", ""), v_strSharp)
                                                    Case "C"
                                                        If Not CType(Me.pnTransDetail.Controls(mv_arrObjFields(v_intSharp).ControlIndex), ComboBoxEx).SelectedValue Is Nothing Then
                                                            v_strSharp = CType(Me.pnTransDetail.Controls(mv_arrObjFields(v_intSharp).ControlIndex), ComboBoxEx).SelectedValue.ToString
                                                        End If
                                                End Select
                                                If Trim(v_strSharp) = "" And InStr(v_strSEARCHSQL, "#" & Format(v_intSharp + 1, "00"), CompareMethod.Text) > 0 Then
                                                    v_strMsgErr = "Bạn chưa nhập " & mv_arrObjFields(v_intSharp).Caption
                                                    Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Visible = True
                                                    Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Text = v_strMsgErr
                                                    Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).ForeColor = System.Drawing.Color.Red
                                                    Exit Sub
                                                Else
                                                    Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).ForeColor = System.Drawing.Color.Blue
                                                End If
                                                v_strSEARCHSQL = v_strSEARCHSQL.Replace("#" & Format(v_intSharp + 1, "00"), v_strSharp.Trim)
                                            End If
                                        Next
                                        v_strSQLCMD = "SELECT * FROM (" & v_strSEARCHSQL & ") WHERE " & v_strKeyName & " = '" & v_strKeyVal & "'"
                                        v_strObjMsg = BuildXMLObjMsg(Me.BusDate, Me.BranchId, , Me.TellerId, Me.LocalObject, gc_MsgTypeObj, "TR." & v_strSEARCHCODE, _
                                                            gc_ActionInquiry, v_strSQLCMD, , , , StockFilter & "|" & MemberFilter)

                                        v_lngError = Proxy.Message(v_strObjMsg)
                                        Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Text = ""
                                        If v_lngError <> ERR_SYSTEM_OK Then
                                            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                                            Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Visible = True
                                            Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Text = mv_ResourceManager.GetString("ERR_LOOKUP_VALUE")
                                            Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).ForeColor = System.Drawing.Color.Red
                                            Exit Sub
                                        Else
                                            Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).ForeColor = System.Drawing.Color.Blue
                                        End If

                                        v_xmlDocument.LoadXml(v_strObjMsg)
                                        v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")
                                        If v_nodeList.Count > 0 Then
                                            'Nạp giá trị tương ứng cho các trường khác
                                            mv_lstIsTextChanged(v_intIndex) = False
                                            FillLookupData(strFLDNAME, v_strFieldValue, v_strObjMsg, v_strKeyName)
                                            'Fill Refval
                                            If v_strFIELDCODE <> "" Then
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
                                            Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Visible = True
                                            Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Text = mv_ResourceManager.GetString("ERR_LOOKUP_VALUE")
                                            Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).ForeColor = System.Drawing.Color.Red
                                            Exit Sub
                                        End If
                                    End If
                                End If
                            End If
                        ElseIf mv_arrObjFields(v_intIndex).LookUp = "Y" Then
                            If mv_lstIsTextChanged(v_intIndex) Then
                                If mv_arrObjFields(v_intIndex).LookupList.Length > 0 Then
                                    Dim ctlCheck As Control
                                    ctlCheck = Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).ControlIndex)
                                    If Not (mv_arrObjFields(v_intIndex).Mandatory = False And ctlCheck.Text.Trim.Length = 0) Then
                                        'Chá»‰ kiá»ƒm tra náº¿u cÃ³ yÃªu cáº§u nháº­p má»›i kiá»ƒm tra
                                        'Fill DL lookup á»Ÿ BDS 
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
                                            If mv_arrObjFields(v_intSharp).Visible = True Then
                                                Select Case Trim(mv_arrObjFields(v_intSharp).ControlType)
                                                    Case "T"
                                                        v_strSharp = CType(Me.pnTransDetail.Controls(mv_arrObjFields(v_intSharp).ControlIndex), TextBox).Text
                                                    Case "M"
                                                        v_strSharp = CType(Me.pnTransDetail.Controls(mv_arrObjFields(v_intSharp).ControlIndex), FlexMaskEditBox).Text
                                                        v_strSharp = IIf(Trim(mv_arrObjFields(v_intSharp).DataType) = "D", v_strSharp.Replace("/  /", ""), v_strSharp)
                                                    Case "C"
                                                        v_strSharp = CType(Me.pnTransDetail.Controls(mv_arrObjFields(v_intSharp).ControlIndex), ComboBoxEx).SelectedValue.ToString
                                                End Select
                                                If Trim(v_strSharp) = "" And InStr(v_strSQLCMD, "#" & Format(v_intSharp + 1, "00"), CompareMethod.Text) > 0 Then
                                                    v_strMsgErr = "Bạn chưa nhập " & mv_arrObjFields(v_intSharp).Caption
                                                    Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Visible = True
                                                    Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Text = v_strMsgErr
                                                    Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).ForeColor = System.Drawing.Color.Red
                                                    Exit Sub
                                                Else
                                                    Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).ForeColor = System.Drawing.Color.Blue
                                                End If
                                                v_strSQLCMD = v_strSQLCMD.Replace("#" & Format(v_intSharp + 1, "00"), v_strSharp.Trim)
                                            End If
                                        Next

                                        'Create message to inquiry object fields

                                        Dim v_xmlDocument As New Xml.XmlDocument
                                        'Láº¥y thÃ´ng tin chung vá»? giao dá»‹ch
                                        v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_LOOKUP, gc_ActionInquiry, v_strSQLCMD, "")

                                        v_lngError = Proxy.Message(v_strObjMsg)
                                        If v_lngError <> ERR_SYSTEM_OK Then
                                            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                                            Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Visible = True
                                            Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Text = mv_ResourceManager.GetString("ERR_LOOKUP_VALUE")
                                            Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).ForeColor = System.Drawing.Color.Red
                                            Exit Sub
                                        Else
                                            Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).ForeColor = System.Drawing.Color.Blue
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
                                                            For k As Integer = 0 To v_nodeList.Item(i).ChildNodes.Count - 1 Step 1
                                                                With v_nodeList.Item(i).ChildNodes(k)
                                                                    If CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value) = "DISPLAY" Then
                                                                        v_strDisplay = Trim(.InnerText.ToString)
                                                                    End If
                                                                End With
                                                            Next
                                                            Dim ctl As Control
                                                            ctl = Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex)
                                                            ctl.Top = sender.Top
                                                            ctl.Text = v_strDisplay
                                                            ctl.Visible = True
                                                            ctl.ForeColor = System.Drawing.Color.Blue
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
                            Else
                                If Not IO.File.Exists(v_strFieldValue) Then
                                    Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Visible = True
                                    Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Text = mv_ResourceManager.GetString("ERR_INVALID_EXISTED_VALUE")
                                    Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).ForeColor = System.Drawing.Color.Red
                                    Exit Sub
                                Else
                                    Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).ForeColor = System.Drawing.Color.Blue
                                End If
                            End If
                        End If
                        ' load lai cac combox co #
                        For v_intSharp1 = 0 To v_intCount - 1 Step 1
                            If Trim(mv_arrObjFields(v_intSharp1).ControlType) = "C" And v_intSharp1 <> v_intIndex Then
                                v_strSQLCMD = mv_arrObjFields(v_intSharp1).LookupList
                                If InStr(v_strSQLCMD, "#") > 0 Then
                                    v_intCount = mv_arrObjFields.GetLength(0) - 1
                                    For v_intSharp = 0 To v_intCount - 1 Step 1
                                        v_strSharp = ""
                                        If mv_arrObjFields(v_intSharp).Visible = True Then
                                            Select Case Trim(mv_arrObjFields(v_intSharp).ControlType)
                                                Case "T"
                                                    v_strSharp = CType(Me.pnTransDetail.Controls(mv_arrObjFields(v_intSharp).ControlIndex), TextBox).Text
                                                Case "M"
                                                    v_strSharp = CType(Me.pnTransDetail.Controls(mv_arrObjFields(v_intSharp).ControlIndex), FlexMaskEditBox).Text
                                                    v_strSharp = IIf(Trim(mv_arrObjFields(v_intSharp).DataType) = "D", v_strSharp.Replace("/  /", ""), v_strSharp)
                                                Case "C"
                                                    v_strSharp = CType(Me.pnTransDetail.Controls(mv_arrObjFields(v_intSharp).ControlIndex), ComboBoxEx).SelectedValue.ToString
                                            End Select
                                            If Trim(v_strSharp) = "" And InStr(v_strSQLCMD, "#" & Format(v_intSharp + 1, "00"), CompareMethod.Text) > 0 Then
                                                v_strMsgErr = "Bạn chưa nhập " & mv_arrObjFields(v_intSharp).Caption
                                                Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Visible = True
                                                Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Text = v_strMsgErr
                                                Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).ForeColor = System.Drawing.Color.Red
                                                Exit Sub
                                            Else
                                                Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).ForeColor = System.Drawing.Color.Blue
                                            End If
                                            v_strSQLCMD = v_strSQLCMD.Replace("#" & Format(v_intSharp + 1, "00"), v_strSharp.Trim)
                                        End If
                                    Next
                                    If mv_arrObjFields(v_intSharp1).MemberField <> "" And mv_lngAllMember = 0 Then
                                        v_strSQLCMD = "select * from (" & v_strSQLCMD & ") where " & mv_arrObjFields(v_intSharp1).MemberField & " in " & mv_strMemberFilter
                                    End If
                                    If mv_arrObjFields(v_intSharp1).StockField <> "" And mv_lngAllStock = 0 Then
                                        v_strSQLCMD = "select * from (" & v_strSQLCMD & ") where " & mv_arrObjFields(v_intSharp1).StockField & " in " & mv_strStockFilter
                                    End If

                                    v_strObjMsg = BuildXMLObjMsg(, BranchId, , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_LOOKUP, gc_ActionInquiry, v_strSQLCMD)

                                    v_lngError = Proxy.Message(v_strObjMsg)
                                    If v_lngError <> ERR_SYSTEM_OK Then
                                        MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                                        Exit Sub
                                    End If
                                    v_cboData = CType(Me.pnTransDetail.Controls(mv_arrObjFields(v_intSharp1).ControlIndex), ComboBoxEx)
                                    FillComboEx(v_strObjMsg, v_cboData)
                                End If
                            End If
                        Next

                    End If
                End If
            End If
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                                     & "Error code: System error!" & vbNewLine _
                                                     & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
            mv_bolCheck = False
        Finally
            If Not v_obj Is Nothing Then
                v_obj.CloseConnection()
            End If
            v_obj = Nothing
        End Try
    End Sub


    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If mv_IsParent = -1 Then
            OnSubmit()
        Else
            Me.DialogResult = Windows.Forms.DialogResult.OK
            OnClose()
        End If
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
                        v_strCtrValue = CType(CType(v_ctr, ComboBoxEx).SelectedValue, String)
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
    Private Function GetSignatureClient(ByVal v_strFileName As String, ByRef v_strClientSignature As String) As Long

        Dim v_strData As String
        Dim v_xmlDocumentMessage As New XmlDocumentEx
        Dim v_attrColl As Xml.XmlAttributeCollection
        Dim v_lngErr As Long = ERR_SYSTEM_START
        Try
            If Not File.Exists(v_strFileName) Then
                MsgBox("File dữ liệu không tồn tại hoặc không đúng", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                Return v_lngErr
                Exit Function
            End If
            Dim v_Stream As New System.IO.StreamReader(v_strFileName)
            v_strData = v_Stream.ReadToEnd
            v_strClientSignature = ClientBussinessCA.CombineData(v_strData)
            v_xmlDocumentMessage.LoadXml(v_strClientSignature)
            v_attrColl = v_xmlDocumentMessage.DocumentElement.Attributes
            v_strClientSignature = CStr(CType(v_attrColl.GetNamedItem("SignatureXML"), Xml.XmlAttribute).Value)
            v_lngErr = ERR_SYSTEM_OK
            v_Stream.Close()
            Return v_lngErr
            'Ký số lên chuỗi string
        Catch ex As Exception
            'LogError.Write(ex.Message & vbNewLine & ex.StackTrace, EventLogEntryType.Error)
            MsgBox(ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
            Return v_lngErr
            Throw ex
        End Try

    End Function
    Private Function GetFile3079(ByVal v_strFileName As String, ByRef v_strClientSignature As String) As Long

        Dim v_strData As String
        Dim v_strSQL As String
        Dim v_strObjMsg As String
        Dim v_xmlDocument As New XmlDocumentEx
        Dim v_lngErr As Long = ERR_SYSTEM_START
        Dim v_strISencrypt As String
        
        Try
            If Not File.Exists(v_strFileName) Then
                MsgBox("File dữ liệu không tồn tại hoặc không đúng", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                Return v_lngErr
                Exit Function
            End If
            'Lấy tham so ma hoa hay ko ma hoa
            v_strSQL = "SELECT a.VARVALUE FROM sysvar a where a.GRNAME='SYSTEM' and a.VARNAME= 'ENCRYPT_SBV_FILE_IMPORT'"
            v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLTX, gc_ActionInquiry, v_strSQL)
            Dim v_lngError As Long = Proxy.Message(v_strObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Exit Function
            End If
            v_xmlDocument.LoadXml(v_strObjMsg)
            v_strISencrypt = CStr(v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='VARVALUE']").InnerText)
            Dim uniEncoding As New System.Text.UnicodeEncoding
            'Dim v_Stream As New System.IO.StreamReader(v_strFileName, uniEncoding)
            Dim v_Stream As New System.IO.StreamReader(v_strFileName)
            v_Stream.DiscardBufferedData()
            v_strData = v_Stream.ReadToEnd
            If v_strISencrypt = "Y" Then
                v_strClientSignature = Decrypt_3079(v_strData, mv_strKeyDecrypt3079)
            Else
                v_strClientSignature = v_strData
            End If
            v_lngErr = ERR_SYSTEM_OK
            v_Stream.Close()
            Return v_lngErr
            'Ký số lên chuỗi string
        Catch ex As Exception
            'LogError.Write(ex.Message & vbNewLine & ex.StackTrace, EventLogEntryType.Error)
            MsgBox(ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
            Return v_lngErr
            Throw ex
        End Try
    End Function

    Private Function Decrypt_3079(ByVal strText As String, ByVal sDecrKey _
               As String) As String
        Dim byKey() As Byte = {}
        Dim IV() As Byte = {&H12, &H34, &H56, &H78, &H90, &HAB, &HCD, &HEF}
        Dim inputByteArray(strText.Length) As Byte

        Try

            byKey = System.Text.Encoding.UTF8.GetBytes(Microsoft.VisualBasic.Left(sDecrKey, 8))
            Dim des As New DESCryptoServiceProvider
            inputByteArray = Convert.FromBase64String(strText)
            Dim ms As New MemoryStream
            Dim cs As New CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write)

            cs.Write(inputByteArray, 0, inputByteArray.Length)
            cs.FlushFinalBlock()
            Dim encoding As System.Text.Encoding = System.Text.Encoding.UTF8

            Return encoding.GetString(ms.ToArray())

        Catch ex As Exception
            Return ex.Message
        End Try

    End Function


    Private Function VerifyRules(ByRef v_strTxMsg As String, ByRef v_strTLTXCD As String) As Boolean
        Try
            Dim v_xmlDocument As New Xml.XmlDocument, v_dataElement As Xml.XmlElement, v_entryNode As Xml.XmlNode
            Dim v_strFLDNAME, v_strFLDVALUE As String
            Dim v_ctl As Control, v_attrFLDNAME As Xml.XmlAttribute, v_attrDATATYPE As Xml.XmlAttribute, v_objEval As New Evaluator
            Dim v_strFieldValue, strFLDNAME As String
            Dim v_intIndex As Integer
            Dim v_strMSGAMT, v_strMSGAMTTran, v_strCOMICODE, v_strMICODE, v_strFileName, v_strFileName1, v_strRange, v_strTXNUM, v_strTXDATE, v_strBUSDATE As String
            Dim v_strTxNote As String = ""
            Dim v_strVsdBrid As String = ""
            Dim v_strSICODE As String = ""
            Dim v_strErrMsg As String = ""
            Dim strCheckSQL As String = ""
            Dim v_strFileNameCA As String = ""
            Dim v_strFileNameCA1 As String = ""
            Dim v_strSignatureClient As String = ""
            Dim strInsertFields, strInsertValues As String
            Dim v_nodeList As Xml.XmlNodeList
            Dim v_strClause, v_strObjMsg As String

            v_strClause = "SELECT a.VARVALUE FROM sysvar a where a.GRNAME='SYSTEM' and a.VARNAME= 'BRSTATUS' and BRID='" & Me.BranchId & "'"
            v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLTX, gc_ActionInquiry, v_strClause)
            Dim v_lngError As Long = Proxy.Message(v_strObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Return False
            End If

            v_xmlDocument.LoadXml(v_strObjMsg)
            If v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='VARVALUE']").InnerText <> OPERATION_ACTIVE Then
                v_oProcess.StopProcessForm()
                Application.DoEvents()
                MsgBox(mv_ResourceManager.GetString("ErrorMsg"), MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, mv_ResourceManager.GetString("Title"))
                mv_bolCheck = False
                Return False
            End If

            'Check data control before commit
            v_strMSGAMT = "0"
            strInsertFields = ""
            strInsertValues = ""
            v_strFLDNAME = ""
            For Each v_control As Control In Me.pnTransDetail.Controls
                If InStr(CType(v_control, Control).Name, PREFIXED_MSKDATA) > 0 Then
                    v_intIndex = CType(v_control, Control).Tag
                    If Not mv_arrObjFields(v_intIndex) Is Nothing Then
                        If (TypeOf (v_control) Is ComboBoxEx) Then
                            v_strFieldValue = Trim(CType(v_control, ComboBox).SelectedValue)
                            'Hanm
                            If (v_strTLTXCD = "2130" Or v_strTLTXCD = "1150") And mv_arrObjFields(v_intIndex).ColumnName = "BRID" Then
                                GetCurrDate(v_strFieldValue)
                            End If
                            If v_strTLTXCD = "1150" And mv_arrObjFields(v_intIndex).ColumnName = "CSEXP_TYPE" Then
                                mv_intCSEXPType = CInt(v_strFieldValue)
                            End If
                            'start Myvq  
                            If (v_strTLTXCD = "1113" Or v_strTLTXCD = "1112" Or v_strTLTXCD = "1132" Or v_strTLTXCD = "1114" Or v_strTLTXCD = "1125" Or v_strTLTXCD = "1123" _
                                                                Or v_strTLTXCD = "1124" Or v_strTLTXCD = "1129" Or v_strTLTXCD = "1130") Then
                                GetCurrDate(v_strFieldValue)
                            End If

                            'end Myvq
                        ElseIf (TypeOf (v_control) Is ucGridControl) Then
                            v_strFieldValue = CType(v_control, ucGridControl).GridValue
                        Else
                            v_strFieldValue = IIf(Trim(mv_arrObjFields(v_intIndex).DataType) = "D" Or Trim(mv_arrObjFields(v_intIndex).DataType) = "P", Trim(v_control.Text).Replace("/  /", ""), Trim(v_control.Text))
                            'start Myvq
                            If (v_strTLTXCD = "1114" Or v_strTLTXCD = "1125" Or v_strTLTXCD = "1150") Then
                                mv_strDate1114 = v_strFieldValue
                            End If
                            'end Myvq
                            'Hanm5 them 3 dong
                            If mv_arrObjFields(v_intIndex).ColumnName = "SICODE" Then
                                v_strFieldValue = v_strFieldValue.Trim.Replace(" ", "_")
                            End If
                            ' tuanta 
                            'Added by Thanglv9 13/04/2013
                            If (v_strTLTXCD = "1131") Then
                                GetCurrDate(mv_strBranchId)
                            End If
                            'End Thanglv9
                            ' purpose : deleted
                            If Trim(mv_arrObjFields(v_intIndex).DataType) = "N" Then
                                v_strFieldValue = Replace(v_strFieldValue, ".", "")
                                v_strFieldValue = Replace(v_strFieldValue, ",", ".")
                            End If
                            ' end tuanta
                            'bangpv
                            If v_strTLTXCD = "1109" Or v_strTLTXCD = "1110" Or v_strTLTXCD = "1127" Or _
                                             ((v_strTLTXCD = "1122" Or v_strTLTXCD = "1111" Or v_strTLTXCD = "1116" Or v_strTLTXCD = "3079") And _
                                                                                mv_arrObjFields(v_intIndex).ColumnName = "FileName") Then
                                v_strFileNameCA = v_strFieldValue
                            End If
                            If (v_strTLTXCD = "1123" Or v_strTLTXCD = "1124" Or v_strTLTXCD = "1129") And mv_arrObjFields(v_intIndex).ColumnName = "FileName1" Then
                                mv_strFileNameCA1 = v_strFieldValue
                            End If
                            If (v_strTLTXCD = "1123" Or v_strTLTXCD = "1124" Or v_strTLTXCD = "1125" Or v_strTLTXCD = "1129") And mv_arrObjFields(v_intIndex).ColumnName = "FileName" Then
                                mv_strFileNameCA = v_strFieldValue
                            End If

                            If v_strTLTXCD = "1128" Then
                                v_strFileNameCA = v_strFieldValue
                            End If
                            'Added by Thanglv9 - 13/04/2013
                            If v_strTLTXCD = "1131" And mv_arrObjFields(v_intIndex).ColumnName = "FileName" Then
                                mv_strFileNameCA = v_strFieldValue
                            End If
                            'End Thanglv9
                            'End(bangpv)                            
                            'End Hanm5
                            'v_strFieldValue = IIf(Trim(mv_arrObjFields(v_intIndex).DataType) = "P", Trim(v_control.Text).Replace("/  /", ""), Trim(v_control.Text))
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
                        Select Case mv_arrObjFields(v_intIndex).DataType
                            Case "N"
                                v_strFieldValue = v_strFieldValue.Replace(",", "")
                            Case "D"
                                v_strFieldValue = v_strFieldValue
                            Case Else
                                v_strFieldValue = v_strFieldValue.Replace("'", "''")
                        End Select

                        strInsertFields &= ",COL_VALUE" & strFLDNAME & ", COL_TYPE" & strFLDNAME & ",COL_DESC" & strFLDNAME
                        strInsertValues &= ",'" & v_strFieldValue & "','" & mv_arrObjFields(v_intIndex).DataType & "','" & Trim(v_control.Text).Replace("/  /", "").Replace("'", "''") & "'"

                        Select Case strFLDNAME
                            Case mv_strMSGAMT
                                v_strMSGAMT = v_strFieldValue
                            Case mv_strFileName
                                v_strFileName = v_strFieldValue
                            Case mv_strFileName1
                                v_strFileName1 = v_strFieldValue
                            Case mv_strRange
                                v_strRange = v_strFieldValue
                            Case mv_strTXNUM
                                v_strTXNUM = v_strFieldValue
                            Case mv_strTXDATE
                                v_strTXDATE = v_strFieldValue
                            Case mv_strBACKDATE
                                v_strBUSDATE = v_strFieldValue
                            Case mv_strSICODE
                                v_strSICODE = v_strFieldValue
                            Case mv_strMICODE
                                v_strMICODE = v_strFieldValue
                            Case mv_strCOMICODE
                                v_strCOMICODE = v_strFieldValue
                            Case mv_strTxNote
                                v_strTxNote = v_strFieldValue
                        End Select
                    End If
                End If
            Next

            ' kiem tra xem nguoi dung co duoc thuc hien giao dich nay, kiem tra thong tin han muc giao dich
            If Not CheckTranLimit(v_strTLTXCD, v_strMSGAMT, v_strMSGAMTTran) Then
                Return False
            End If
            ' Duyệt mảng dữ liệu đánh mốc các điều kiện kiểm tra

            If Not CheckFldvals(mv_arrObjFldVals) Then
                Return False
            End If

            'Lay thong so tao giao dich
            Dim v_strParentID, v_strBRCODE, v_strTLNAME, v_strTXNAME, v_strSTATUS_TEXT, v_strPARENT_TEXT As String
            ' ma giao dich cha
            v_strParentID = "0"
            If mv_strTXNUM <> "" And mv_strTXDATE <> "" Then
                v_strClause = "select AUTOID, SICODE, MICODE, BUSDATE, COMICODE from TLLOG where txdate = TO_DATE('" & v_strTXDATE & "','dd/mm/yyyy') and txnum = '" & v_strTXNUM & "'"
                'v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLTX, gc_ActionInquiry, v_strClause)
                v_strObjMsg = BuildXMLObjMsg(Me.BusDate, Me.BranchId, , Me.TellerId, gc_IsNotLocalMsg, gc_MsgTypeObj, "TR.TLLOG", _
                                        gc_ActionInquiry, v_strClause, , , , StockFilter & "|" & MemberFilter)
                v_lngError = Proxy.Message(v_strObjMsg)
                If v_lngError <> ERR_SYSTEM_OK Then
                    MsgBox(IIf(UserLanguage = gc_LANG_ENGLISH, gc_ERR_GET_MSG_EN, gc_ERR_GET_MSG_VN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                    Return False
                End If

                v_xmlDocument.LoadXml(v_strObjMsg)
                If Not IsDBNull(v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='AUTOID']")) Then
                    v_strParentID = v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='AUTOID']").InnerText
                End If
                If (Not IsDBNull(v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='SICODE']"))) And v_strSICODE = "" Then
                    v_strSICODE = v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='SICODE']").InnerText
                End If
                If Not IsDBNull(v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='MICODE']")) Then
                    v_strMICODE = v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='MICODE']").InnerText
                End If
                If Not IsDBNull(v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='COMICODE']")) Then
                    v_strCOMICODE = v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='COMICODE']").InnerText
                End If
                If Not IsDBNull(v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='BUSDATE']")) Then
                    v_strBUSDATE = v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='BUSDATE']").InnerText
                End If
            End If
            'ma chi nhanh
            v_strBRCODE = Me.BRCODE
            ' ten nguoi nhap
            v_strTLNAME = Me.TellerName
            ' ten giao dich
            v_strTXNAME = Me.TabText
            'ten trang thai
            v_strSTATUS_TEXT = LOG_MEMBER_STAFF_TEXT
            'Nơi nhận chứng gốc
            v_strVsdBrid = Me.VSDBRID

            v_strPARENT_TEXT = ""
            v_strClause = "select CDCONTENT from allcode where cdname = 'ISPARENT' and CDTYPE = 'SY' and CDVALNO = " & mv_strISPARENT
            v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLTX, gc_ActionInquiry, v_strClause)
            v_lngError = Proxy.Message(v_strObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Return False
            End If

            v_xmlDocument.LoadXml(v_strObjMsg)
            If Not IsDBNull(v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='CDCONTENT']")) Then
                v_strPARENT_TEXT = v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='CDCONTENT']").InnerText
            End If
            'BangPV
            'Lấy dữ liệu từ file chọn  khi thực hiện giao dịch 1109, ký số và lấy chữ ký gửi lên server
            If v_strTLTXCD = "1109" Or v_strTLTXCD = "1127" Then
                v_lngError = GetSignatureClient(v_strFileNameCA, v_strSignatureClient)
                If v_lngError = ERR_SYSTEM_START Then
                    Exit Function
                End If
            ElseIf v_strTLTXCD = "1110" Or v_strTLTXCD = "1128" Then
                v_lngError = GetSignatureClient(v_strFileNameCA, v_strSignatureClient)
                If v_lngError = ERR_SYSTEM_START Then
                    Exit Function
                End If
            ElseIf (v_strTLTXCD = "1111" Or v_strTLTXCD = "1116" Or v_strTLTXCD = "1122") Then
                v_lngError = GetSignatureClient(v_strFileNameCA, v_strSignatureClient)
                If v_lngError = ERR_SYSTEM_START Then
                    Exit Function
                End If
            End If
            'lay du lieu tu file cua NHNN tai giao dich 3079 de xu ly 3047
            If v_strTLTXCD = "3079" Then
                v_lngError = GetFile3079(v_strFileNameCA, v_strSignatureClient)
            End If
            'end bangpv 

            'Tạo điện giao dịch
            v_strTxMsg = BuildXMLTxMsg(gc_MsgTypeTrans, Me.LocalObject, IIf(mv_strCOTLTXCD <> "", mv_strCOTLTXCD, v_strTLTXCD), _
                        Me.BranchId, Me.TellerId, Me.IpAddress, Me.WsName, _
                        TransactStatus.LOG_MEMBER_STAFF, IIf(InStr(v_strTLTXCD, "44") > 0, "Y", "N"), _
                        mv_strOFFID, mv_strCHKID, mv_strCFRID, Me.BusDate, , , , , mv_strMSGAMT, v_strMICODE, , , Me.UserLanguage, , v_strSICODE, _
                        mv_strCHILDTLTXCD, mv_strISPARENT, v_strParentID, v_strBRCODE, v_strTLNAME, , , , v_strTXNAME, v_strSTATUS_TEXT, v_strPARENT_TEXT, _
                        v_strParentID, gc_MsgTypeTrans, mv_intISBRID.ToString, v_strCOMICODE, , , , "0", v_strTxNote, _
                        v_strVsdBrid, mv_strTblChk, v_strTLTXCD, v_strTXNUM, v_strTXDATE, v_strBUSDATE, mv_strSignCA, v_strFileNameCA, v_strSignatureClient)
            v_xmlDocument.LoadXml(v_strTxMsg)

            If mv_strCOTLTXCD <> "" Then
                'v_strSQLList = ""
                strInsertFields = ""
                strInsertValues = ""
                If mv_strTXNUM <> "" And mv_strTXDATE <> "" Then
                    strInsertFields = ",COL_VALUE" & mv_strTXNUM & ", COL_TYPE" & mv_strTXNUM & ", COL_DESC" & mv_strTXNUM _
                    & ",COL_VALUE" & mv_strTXDATE & ", COL_TYPE" & mv_strTXDATE & ", COL_DESC" & mv_strTXDATE
                    If Trim(mv_strBACKDATE) <> "" Then
                        strInsertFields &= ",COL_VALUE" & mv_strBACKDATE & ", COL_TYPE" & mv_strBACKDATE & ", COL_DESC" & mv_strBACKDATE
                    End If
                    strInsertValues = ",'" & v_strTXNUM & "'" & " COL_VALUE" & mv_strTXNUM _
                    & ", 'C'" & " COL_TYPE" & mv_strTXNUM _
                    & ", '" & v_strTXNUM & "'" & " COL_DESC" & mv_strTXNUM _
                    & ", '" & v_strTXDATE & "'" & " COL_VALUE" & mv_strTXDATE _
                    & ", 'D'" & " COL_TYPE" & mv_strTXDATE _
                    & ", '" & v_strTXDATE & "'" & " COL_DESC" & mv_strTXDATE
                    If Trim(mv_strBACKDATE) <> "" Then
                        strInsertValues &= ", '" & v_strBUSDATE.Replace("/  /", "").Trim() & "'" & " COL_VALUE" & mv_strBACKDATE _
                                        & ", 'D'" & " COL_TYPE" & mv_strBACKDATE _
                                        & ", '" & v_strBUSDATE.Replace("/  /", "").Trim() & "'" & " COL_DESC" & mv_strBACKDATE
                    End If
                End If
                'strCheckSQL = v_strSQLList.Replace("?TXFIELDS", strInsertFields).Replace("?TXVALUES", strInsertValues)

                Select Case v_strTLTXCD
                    Case "4003", "4005", "4006", "4007", "4008", "4020"
                        ' tuanta 30092009, sua toc do phan nhan dl gd
                        ''If Not CheckChildTranRulesDataSet(v_xmlDocument, v_strFileName, "", strInsertFields, strInsertValues, v_strErrMsg, v_strTXNUM, v_strTXDATE) Then
                        ''    Dim frm As New frmErrMsg
                        ''    frm.ErrMsg = v_strErrMsg
                        ''    frm.ErrCount = mv_intErrCount
                        ''    frm.WarningCount = 0
                        ''    frm.WarningMsg = ""
                        ''    'v_thread.Abort()
                        ''    frm.ShowDialog()
                        ''    mv_bolCheck = False
                        ''    Return False
                        ''End If
                        ' end tuanta
                        'If Not CheckChildTranRulesDataSet(v_xmlDocument, v_strFileName1, "", strInsertFields, strInsertValues, v_strErrMsg, v_strTXNUM, v_strTXDATE) Then
                        '    Dim frm As New frmErrMsg
                        '    frm.ErrMsg = "Các lỗi từ danh sách khớp lệnh TP/CP" & vbCrLf & v_strErrMsg
                        '    frm.ErrCount = mv_intErrCount
                        '    frm.WarningCount = 0
                        '    frm.WarningMsg = ""
                        '    v_thread.Abort()
                        '    frm.ShowDialog()
                        '    mv_bolCheck = False
                        '    Return False
                        'End If
                    Case "4004"
                        ' tuanta 30092009, sua toc do phan nhan dl gd
                        ''If Not CheckChildTranRulesDataSet(v_xmlDocument, v_strFileName, "D", strInsertFields, strInsertValues, v_strErrMsg, v_strTXNUM, v_strTXDATE) Then
                        ''    v_oProcess.StopProcessForm()
                        ''    Application.DoEvents()
                        ''    Dim frm As New frmErrMsg
                        ''    frm.ErrMsg = "Các lỗi từ danh sách khớp lệnh thông thường" & vbCrLf & v_strErrMsg
                        ''    frm.ErrCount = mv_intErrCount
                        ''    frm.WarningCount = 0
                        ''    frm.WarningMsg = ""
                        ''    'v_thread.Abort()
                        ''    frm.ShowDialog()
                        ''    mv_bolCheck = False
                        ''    Return False
                        ''End If
                        ''If Not CheckChildTranRulesDataSet(v_xmlDocument, v_strFileName1, "P", strInsertFields, strInsertValues, v_strErrMsg, v_strTXNUM, v_strTXDATE) Then
                        ''    v_oProcess.StopProcessForm()
                        ''    Application.DoEvents()
                        ''    Dim frm As New frmErrMsg
                        ''    frm.ErrMsg = "Các lỗi từ danh sách khớp lệnh thỏa thuận" & vbCrLf & v_strErrMsg
                        ''    frm.ErrCount = mv_intErrCount
                        ''    frm.WarningCount = 0
                        ''    frm.WarningMsg = ""
                        ''    'v_thread.Abort()
                        ''    frm.ShowDialog()
                        ''    mv_bolCheck = False
                        ''    Return False
                        ''End If
                        ' end tuanta
                        'Case "4005"
                        '    If Not CheckChildTranRulesDataSet(v_xmlDocument, v_strFileName, "", strInsertFields, strInsertValues, v_strErrMsg, v_strTXNUM, v_strTXDATE) Then
                        '        Dim frm As New frmErrMsg
                        '        frm.ErrMsg = v_strErrMsg
                        '        frm.ErrCount = mv_intErrCount
                        '        frm.WarningCount = 0
                        '        frm.WarningMsg = ""
                        '        v_thread.Abort()
                        '        frm.ShowDialog()
                        '        mv_bolCheck = False
                        '        Return False
                        '    End If
                    Case "2133", "2136", "2139", "2142", "2145", "2163", "2166", "2169", "2172", "2148", "4109", "4209"
                    Case Else
                        If Not CheckChildTranRules(v_xmlDocument, v_strFileName, v_strRange, strInsertFields, strInsertValues, v_strErrMsg, v_strTXNUM, v_strTXDATE) Then
                            v_oProcess.StopProcessForm()
                            Application.DoEvents()
                            Dim frm As New frmErrMsg
                            frm.ErrMsg = v_strErrMsg
                            frm.ErrCount = mv_intErrCount
                            frm.WarningCount = 0
                            frm.WarningMsg = ""
                            'v_thread.Abort()
                            frm.ShowDialog()
                            mv_bolCheck = False
                            Return False
                        End If

                End Select
            Else
                v_dataElement = v_xmlDocument.CreateElement(Xml.XmlNodeType.Element, "fields", "")
                strCheckSQL = "INSERT INTO TMP_TXFIELDS(AUTOID, TLTXCD" & strInsertFields & ", REAL_ROW) " _
                        & " SELECT seq_tmp_txfields.nextval,'" & v_strTLTXCD & "'" & strInsertValues & " , 1 REAL_ROW FROM DUAL"
                'Append entry to data node
                v_entryNode = v_xmlDocument.CreateNode(Xml.XmlNodeType.Element, "entry", "")
                'Add field name
                v_attrFLDNAME = v_xmlDocument.CreateAttribute(gc_AtributeFLDNAME)
                v_attrFLDNAME.Value = "INSERT_ROW"
                v_entryNode.Attributes.Append(v_attrFLDNAME)
                'Add field type
                v_attrDATATYPE = v_xmlDocument.CreateAttribute(gc_AtributeFLDTYPE)
                v_attrDATATYPE.Value = "C"
                v_entryNode.Attributes.Append(v_attrDATATYPE)
                'Set value
                v_entryNode.InnerText = strCheckSQL
                v_dataElement.AppendChild(v_entryNode)
                v_xmlDocument.DocumentElement.AppendChild(v_dataElement)
            End If

            v_strTxMsg = v_xmlDocument.InnerXml
            Return True
        Catch ex As Exception
            'LogError.Write(ex.Message & vbNewLine & ex.StackTrace, EventLogEntryType.Error)
            'v_thread.Abort()
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
        v_oProcess = Nothing
        Me.Close()
    End Sub
    Protected Overridable Sub SetLookUpDataForm()

    End Sub

    Public Overridable Sub v_cboData_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
        Dim v_obj As SQLDataAccessLayer
        Dim v_ds As DataSet
        Try
            Dim strFLDNAME As String, v_intIndex As Integer
            Dim v_strSQLCMD As String
            Dim v_strFieldValue As String
            Dim v_intSharp1, v_intCount As Integer
            Dim v_cboData As ComboBoxEx

            If InStr(CType(sender, Control).Name, PREFIXED_MSKDATA) > 0 Then
                v_intIndex = CType(sender, Control).Tag
                If mv_lstIsTextChanged(v_intIndex) Then
                    mv_lstIsTextChanged(v_intIndex) = False
                    If v_obj Is Nothing Then
                        v_obj = New SQLDataAccessLayer(TellerId)
                    End If

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

                        If v_strFieldValue <> "" Then
                            v_strSQLCMD = mv_arrObjFields(v_intIndex).LookupList
                            v_ds = v_obj.ExecuteReturnDataSet(v_strSQLCMD)
                            FillLookupData(strFLDNAME, v_strFieldValue, v_ds)
                        Else
                            Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Visible = True
                            Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Text = mv_ResourceManager.GetString("ERR_LOOKUP_VALUE")
                            Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).ForeColor = System.Drawing.Color.Red
                            Exit Sub
                        End If

                        ' load lai cac combox co #
                        v_intCount = mv_arrObjFields.GetLength(0) - 1
                        For v_intSharp1 = 0 To v_intCount - 1 Step 1
                            If Trim(mv_arrObjFields(v_intSharp1).ControlType) = "C" And v_intSharp1 <> v_intIndex Then
                                v_strSQLCMD = mv_arrObjFields(v_intSharp1).LookupList
                                If InStr(v_strSQLCMD, "#" & strFLDNAME) > 0 Then
                                    v_strSQLCMD = v_strSQLCMD.Replace("#" & strFLDNAME, v_strFieldValue)
                                    v_ds = v_obj.ExecuteReturnDataSet(v_strSQLCMD)

                                    v_cboData = CType(Me.pnTransDetail.Controls(mv_arrObjFields(v_intSharp1).ControlIndex), ComboBoxEx)

                                    v_cboData.BeginUpdate()
                                    v_cboData.DataSource = v_ds.Tables(0)
                                    v_cboData.DisplayMember = "DISPLAY"
                                    v_cboData.ValueMember = "VALUE"
                                    v_cboData.EndUpdate()

                                    If ObjectType = "R" Then
                                        If mv_arrObjFields(v_intSharp1).RiskField Then
                                            v_cboData.AddItems(mv_ResourceManager.GetString("ALL"), "-1")
                                        End If
                                    End If

                                    v_cboData.SelectedIndex = 0
                                End If
                            End If
                        Next
                    End If
                End If
            End If
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                                     & "Error code: System error!" & vbNewLine _
                                                     & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
            mv_bolCheck = False
        Finally
            If Not v_obj Is Nothing Then
                v_obj.CloseConnection()
            End If
            v_obj = Nothing
            If Not v_ds Is Nothing Then
                v_ds = Nothing
            End If
        End Try
    End Sub

    'Private Sub ExecFldval(ByVal v_xmlDocument As Xml.XmlDocument)
    '    Dim v_intCount, v_intIndex, i As Integer
    '    Dim v_strVALEXP, v_strVALEXP2 As String
    '    Dim v_dtVALEXP, v_dtVALEXP2 As Date
    '    Dim v_objEval As New Evaluator

    '    'Duyá»‡t máº£ng dá»¯ liá»‡u danh má»¥c cÃ¡c Ä‘iá»?u kiá»‡n kiá»ƒm tra
    '    v_intCount = mv_arrObjFldVals.GetLength(0)
    '    If v_intCount > 0 Then
    '        For i = 0 To v_intCount - 1 Step 1
    '            If Not mv_arrObjFldVals(i) Is Nothing Then
    '                'Xá»­ lÃ½ theo tham sá»‘ Ä‘Ã£ cÃ i Ä‘áº·t
    '                With mv_arrObjFldVals(i)
    '                    'XÃ¡c Ä‘inh control index
    '                    v_intIndex = mv_arrObjFields(.IDXFLD).ControlIndex
    '                    'Thá»±c hiá»‡n xá»­ lÃ½ cho tá»«ng phÃ©p toÃ¡n
    '                    If .VALTYPE = "E" Then
    '                        'Náº¿u trÆ°á»?ng cÃ³ kiá»ƒu dá»¯ liá»‡u lÃ  sá»‘
    '                        If mv_arrObjFields(.IDXFLD).DataType <> "D" Then
    '                            Select Case .mp_OPERATOR
    '                                Case "EX"
    '                                    v_strVALEXP = BuildAMTEXP(.VALEXP)
    '                                    If mv_arrObjFields(.IDXFLD).DataType = "N" Then
    '                                        Me.pnTransDetail.Controls(v_intIndex).Text = Math.Round(v_objEval.Eval(v_strVALEXP), 0)
    '                                        FormatNumericTextbox(CType(Me.pnTransDetail.Controls(v_intIndex), TextBox))
    '                                    Else
    '                                        Me.pnTransDetail.Controls(v_intIndex).Text = v_objEval.Eval(v_strVALEXP)
    '                                    End If
    '                                Case "MA"
    '                                    v_strVALEXP = BuildAMTEXP(.VALEXP)
    '                                    v_strVALEXP2 = BuildAMTEXP(.VALEXP2)
    '                                    If mv_arrObjFields(.IDXFLD).DataType = "N" Then
    '                                        Me.pnTransDetail.Controls(v_intIndex).Text = Math.Round(GetMax(v_objEval.Eval(v_strVALEXP), v_objEval.Eval(v_strVALEXP2)), 0)
    '                                        FormatNumericTextbox(CType(Me.pnTransDetail.Controls(v_intIndex), TextBox))
    '                                    Else
    '                                        Me.pnTransDetail.Controls(v_intIndex).Text = GetMax(v_objEval.Eval(v_strVALEXP), v_objEval.Eval(v_strVALEXP2)).ToString
    '                                    End If
    '                                    'Me.pnTransDetail.Controls(v_intIndex).Text = GetMax(v_objEval.Eval(v_strVALEXP), v_objEval.Eval(v_strVALEXP2)).ToString
    '                            End Select
    '                            'Náº¿u trÆ°á»?ng cÃ³ kiá»ƒu dá»¯ liá»‡u lÃ  ngÃ y thÃ¡ng
    '                        ElseIf mv_arrObjFields(.IDXFLD).DataType = "D" Then
    '                            Select Case .mp_OPERATOR
    '                                Case "DF"
    '                                    If .VALEXP = "00" Then
    '                                        v_dtVALEXP = DDMMYYYY_SystemDate(Me.pnTransDetail.Controls(0).Text)
    '                                    End If
    '                                    v_dtVALEXP2 = DDMMYYYY_SystemDate(BuildAMTEXP(.VALEXP2))
    '                                    Me.pnTransDetail.Controls(v_intIndex).Text = DateDiff(DateInterval.Day, v_dtVALEXP, v_dtVALEXP2).ToString
    '                            End Select
    '                        End If
    '                    End If
    '                End With
    '            End If
    '        Next
    '    End If
    'End Sub

    'Public Sub GetInventory(ByVal v_strFldSource As String, ByVal v_strFldDesc As String, ByVal v_strInvName As String, ByVal v_strInvFormat As String)
    '    Try
    '        Dim v_strModule As String = "", v_strValue As String = "", v_strFldName As String = ""
    '        Dim v_ctlmskData As FlexMaskEditBox, v_ctrl As Control
    '        Dim v_strTemp, v_strINVReturn As String, v_intPos As Integer
    '        v_strModule = Mid(v_strFldSource, 1, 2)
    '        v_strFldSource = Mid(v_strFldSource, 3)

    '        If Not mv_arrObjFields Is Nothing Then
    '            'XÃ¡c Ä‘á»‹nh giÃ¡ trá»‹ cá»§a cÃ¡c trÆ°á»?ng dÃ¹ng Ä‘á»ƒ gá»™p vá»›i Inventory Name
    '            For Each v_ctrl In Me.pnTransDetail.Controls
    '                If TypeOf (v_ctrl) Is FlexMaskEditBox Then
    '                    v_ctlmskData = CType(v_ctrl, FlexMaskEditBox)
    '                    v_strFldName = Mid(v_ctlmskData.Name, Len(PREFIXED_MSKDATA) + 1)
    '                    If InStr(v_strFldSource, v_strFldName & "@") > 0 Then
    '                        If Len(v_ctlmskData.Text) > 0 Then
    '                            v_strValue = v_strValue & v_ctlmskData.Text
    '                        Else
    '                            'Náº¿u chÆ°a cÃ³ giÃ¡ trá»‹ thÃ¬ khÃ´ng táº¡o Inventory ná»¯a
    '                            Exit Sub
    '                        End If

    '                    End If
    '                End If
    '            Next

    '            'Náº¿u cÃ³ láº¥y Inventory thÃ¬ pháº£i xÃ¡c Ä‘á»‹nh giÃ¡ trá»‹ cá»§a trÆ°á»?ng nguá»“n
    '            If Len(v_strInvName) > 0 Then
    '                'Cáº¥u trÃºc cá»§a 01 inventoy lÃ : PhÃ¢n há»‡ + MÃ£ chi nhÃ¡nh + TÃªn Inventory + GiÃ¡ trá»‹ trÆ°á»?ng nguá»“n
    '                v_strValue = v_strModule & Me.BranchId & v_strInvName & v_strValue
    '                'v_strValue = v_strModule & Me.BranchId & v_strInvName

    '                'Gá»?i hÃ m Ä‘á»ƒ xÃ¡c Ä‘á»‹nh Inventory
    '                Dim v_strClause, v_strObjMsg As String
    '                Dim v_xmlDocument As New Xml.XmlDocument
    '                'Dim v_ws As New BDSDelivery.BDSDelivery
    '                'Láº¥y thÃ´ng tin chung vá»? giao dá»‹ch
    '                v_strClause = v_strValue
    '                v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SY_AUTHENTICATION, gc_ActionInquiry, , v_strClause, "GetInventory")

    '                Dim v_lngError As Long = mv_BDSDelivery.Message(v_strObjMsg)
    '                If v_lngError <> ERR_SYSTEM_OK Then
    '                    MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
    '                    Exit Sub
    '                End If

    '                'Láº¥y giÃ¡ trá»‹ tráº£ vá»? (tráº£ táº¡i má»‡nh Ä‘á»? Clause luÃ´n)
    '                v_xmlDocument.LoadXml(v_strObjMsg)
    '                Dim v_attrColl As Xml.XmlAttributeCollection = v_xmlDocument.DocumentElement.Attributes
    '                v_strValue = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCLAUSE), Xml.XmlAttribute).Value)

    '                'Táº¡o theo cáº¥u trÃºc InvFormat
    '                v_intPos = 1
    '                v_strINVReturn = String.Empty
    '                While v_intPos < Len(v_strInvFormat)
    '                    v_strTemp = Mid(v_strInvFormat, v_intPos, 2)
    '                    If v_strTemp = "BR" Then
    '                        v_strINVReturn = v_strINVReturn & Me.BranchId
    '                    Else
    '                        'Láº¥y giÃ¡ trá»‹ hiá»‡n táº¡i cá»§a trÆ°á»?ng nÃ o Ä‘Ã³
    '                        For Each v_ctrl In Me.pnTransDetail.Controls
    '                            If TypeOf (v_ctrl) Is FlexMaskEditBox Then
    '                                v_ctlmskData = CType(v_ctrl, FlexMaskEditBox)
    '                                v_strFldName = Mid(v_ctlmskData.Name, Len(PREFIXED_MSKDATA) + 1)
    '                                If Trim(v_strTemp) = Trim(v_strFldName) Then
    '                                    v_strINVReturn = v_strINVReturn & CType(v_ctrl, FlexMaskEditBox).Text
    '                                    Exit For
    '                                End If
    '                            End If
    '                        Next
    '                    End If
    '                    v_intPos = v_intPos + 2
    '                End While

    '                'Ä?áº·t Inventory cho trÆ°á»?ng Ä‘Ã­ch: 
    '                For Each v_ctrl In Me.pnTransDetail.Controls
    '                    If TypeOf (v_ctrl) Is FlexMaskEditBox Then
    '                        v_ctlmskData = CType(v_ctrl, FlexMaskEditBox)
    '                        v_strFldName = Mid(v_ctlmskData.Name, Len(PREFIXED_MSKDATA) + 1)
    '                        If Trim(v_strFldDesc) = Trim(v_strFldName) Then
    '                            v_strTemp = Strings.Right(gc_FORMAT_ODAUTOID & v_strValue, CType(v_ctrl, FlexMaskEditBox).MaxLength - Len(v_strINVReturn))
    '                            CType(v_ctrl, FlexMaskEditBox).Text = v_strINVReturn & v_strTemp
    '                            Exit Sub
    '                        End If
    '                    End If
    '                Next
    '            End If
    '        End If

    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Sub

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

        v_strObjMsg = BuildXMLObjMsg(, , , , gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SY_SEARCHFLD, gc_ActionInquiry, strViewSQL, , )
        Proxy.Message(v_strObjMsg)
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

    Private Function CheckChildTranRules1(ByVal v_xmlDocument As Xml.XmlDocument, _
                                         ByVal strFileName As String, _
                                         ByVal strRange As String, _
                                         ByVal v_strFieldsSQL As String, _
                                         ByVal v_strValuesSQL As String, _
                                         ByRef v_strErrMsg As String, _
                                         ByVal strTXNUM As String, _
                                         ByVal strTXDATE As String) As Boolean
        Dim xlApp As Microsoft.Office.Interop.Excel.Application
        Dim xlBook As Microsoft.Office.Interop.Excel.Workbook
        Dim xlSheet As Microsoft.Office.Interop.Excel.Worksheet
        Dim xlData As Microsoft.Office.Interop.Excel.Range
        Dim arrValue As System.Array
        Dim v_dataElement As Xml.XmlElement, v_entryNode As Xml.XmlNode

        Dim strErrMsg As String = ""
        Dim intIndex, intCount, intVIndex, intVCount, i, j, intRealRow As Integer
        Dim v_strSQLCMD, v_strFULLDATA, v_strObjMsg, v_strValue, v_strSEARCHCODE, _
            v_strFIELDCODE, v_strFLDNAME, v_strKeyName, v_strKeyVal, v_strSEARCHSQL As String
        Dim v_nodeList As Xml.XmlNodeList
        Dim arrCoFields(), arrDisplayCoFields() As String
        Dim strConDuplicated, strRowDuplicated As String
        Dim strInsertFields, strInsertValues As String
        Dim dblTranLimit As Double
        Dim v_xmlObjDocument As New Xml.XmlDocument
        Dim strChildMsgAmt As String
        Dim strRows As String = ""
        Dim v_attrFLDNAME As Xml.XmlAttribute, v_attrDATATYPE As Xml.XmlAttribute

        Dim v_intSharp, v_intCount As Integer
        Dim v_strSharp As String
        Dim v_strMsgErr As String
        Dim v_lngError As Long
        Dim v_blnEmptyRow As Boolean = False
        Dim v_obj As SQLDataAccessLayer
        Dim v_ds As DataSet
        Try
            ' kiem tra xem nguoi dung co duoc thuc hien giao dich nay khong, lay thong tin han muc giao dich
            If Not CheckTranLimit(mv_strCOTLTXCD, 0, dblTranLimit, strChildMsgAmt) Then
                Return False
            End If
            ' load data
            xlApp = New Microsoft.Office.Interop.Excel.Application
            xlBook = xlApp.Workbooks.Open(strFileName)
            xlSheet = CType(xlBook.Worksheets(1), Microsoft.Office.Interop.Excel.Worksheet)
            xlData = xlSheet.Range(strRange.Insert(6, ":").Replace(" ", ""))
            arrValue = CType(xlData.Value, Array)
            If strRange.Substring(1, 1) <= "Z" And strRange.Substring(1, 1) >= "A" Then
                intRealRow = CInt(Trim(strRange.Substring(2, 4))) - 1
            Else
                intRealRow = CInt(Trim(strRange.Substring(1, 5))) - 1
            End If

            'Hanm5 sửa: check trong file import có dòng dữ liệu trắng hay không.
            mv_intErrCount = 0
            For i = 1 To arrValue.GetLength(0)
                For j = 1 To arrValue.GetLength(1)
                    If Not arrValue.GetValue(i, j) Is Nothing Then
                        v_blnEmptyRow = True
                    End If
                Next
                If v_blnEmptyRow = False Then
                    strErrMsg = strErrMsg & "---oOo---" & vbCrLf & "Dòng " & i + intRealRow & " không có dữ liệu" & vbCrLf
                    mv_intErrCount = mv_intErrCount + 1
                End If
                v_blnEmptyRow = False
            Next

            If Trim(strErrMsg) <> "" Then
                v_strErrMsg = strErrMsg
                Return False
            End If
            'Kết thúc : Hanm5 sửa

            ' lay cac tieu chi filter tu mv_arrObjCoFields
            intCount = mv_arrObjCoFields.GetLength(0) - 1
            ReDim arrCoFields(intCount)
            ReDim arrDisplayCoFields(intCount)
            v_obj = New SQLDataAccessLayer(TellerId)
            For intIndex = 0 To intCount - 1
                If mv_arrObjCoFields(intIndex).LookupList <> "" Then
                    v_strSQLCMD = mv_arrObjCoFields(intIndex).LookupList
                    If InStr(v_strSQLCMD, "#", CompareMethod.Text) = 0 Then
                        v_ds = v_obj.ExecuteReturnDataSet(v_strSQLCMD)
                        For i = 0 To v_ds.Tables(0).Rows.Count - 1
                            With v_ds.Tables(0)
                                arrCoFields(intIndex) = arrCoFields(intIndex) & Chr(1) & Trim(.Rows(i)("VALUE"))
                                'arrDisplayCoFields(intIndex) = arrDisplayCoFields(intIndex) & Chr(1) & Trim(.Rows(i)("VALUE"))
                                arrDisplayCoFields(intIndex) = arrDisplayCoFields(intIndex) & Chr(1) & Trim(.Rows(i)("DISPLAY"))
                            End With
                        Next i
                    End If
                    '    ElseIf mv_arrObjCoFields(intIndex).SearchCode <> "" Then
                    '        'ElseIf mv_arrObjCoFields(intIndex).SearchCode <> "" And Not (InStr(mv_arrObjCoFields(intIndex).SearchCode, "#") > 0) Then
                    '        v_strSEARCHCODE = mv_arrObjCoFields(intIndex).SearchCode
                    '        v_strSQLCMD = "SELECT A.FIELDCODE KEYNAME,B.SEARCHCMDSQL, C.FIELDCODE " _
                    '        & " FROM SISEARCHFLD A,SISEARCH B, (select * from SISEARCHFLD where REFVALUE ='Y') C " & ControlChars.CrLf _
                    '        & " WHERE B.SEARCHCODE = A.SEARCHCODE " & ControlChars.CrLf _
                    '        & " AND A.KEY ='Y' AND B.SEARCHCODE ='" & v_strSEARCHCODE & "'" _
                    '        & " AND B.SEARCHCODE = C.SEARCHCODE(+)"
                    '        v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_LOOKUP, gc_ActionInquiry, v_strSQLCMD)

                    '        v_lngError = mv_BDSDelivery.Message(v_strObjMsg)
                    '        If v_lngError <> ERR_SYSTEM_OK Then
                    '            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                    '            Return False
                    '        End If

                    '        v_xmlObjDocument.LoadXml(v_strObjMsg)
                    '        v_nodeList = v_xmlObjDocument.SelectNodes("/ObjectMessage/ObjData")
                    '        v_strKeyName = ""
                    '        v_strSEARCHSQL = ""
                    '        v_strFIELDCODE = ""
                    '        If v_nodeList.Count > 0 Then
                    '            For i = 0 To v_nodeList.Count - 1
                    '                For j = 0 To v_nodeList.Item(i).ChildNodes.Count - 1 Step 1
                    '                    With v_nodeList.Item(i).ChildNodes(j)
                    '                        v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                    '                        v_strValue = Trim(.InnerText)
                    '                        Select Case v_strFLDNAME
                    '                            Case "KEYNAME"
                    '                                v_strKeyName = Trim(v_strValue)
                    '                            Case "SEARCHCMDSQL"
                    '                                v_strSEARCHSQL = Trim(v_strValue)
                    '                            Case "FIELDCODE"
                    '                                v_strFIELDCODE = Trim(v_strValue)
                    '                        End Select
                    '                    End With
                    '                Next
                    '            Next
                    '            v_strSEARCHSQL = v_strSEARCHSQL.Replace("?LANGUAGE", Me.UserLanguage)
                    '            v_strSEARCHSQL = v_strSEARCHSQL.Replace("?MEMBERFILTER", IIf(mv_lngAllMember = 0, " NVL(a.micode,'000') in " & mv_strMemberFilter, " 1=1 "))
                    '            v_strSEARCHSQL = v_strSEARCHSQL.Replace("?COMEMBERFILTER", IIf(mv_lngAllMember = 0, " NVL(a.comicode,'000') in " & mv_strMemberFilter, " 1=1 "))
                    '            v_strSEARCHSQL = v_strSEARCHSQL.Replace("?STOCKFILTER", IIf(mv_lngAllStock = 0, " NVL(a.sicode,'000') in " & mv_strStockFilter, " 1=1 "))
                    '            v_strSEARCHSQL = v_strSEARCHSQL.Replace("?BRID", mv_strBranchId)
                    '            v_strSEARCHSQL = v_strSEARCHSQL.Replace("?TLID", mv_strTellerId)

                    '            v_intCount = mv_arrObjFields.GetLength(0) - 1
                    '            For v_intSharp = 0 To v_intCount - 1 Step 1
                    '                v_strSharp = ""
                    '                'Hanm5 Sửa
                    '                If mv_arrObjFields(v_intSharp).Visible = True Then
                    '                    Select Case Trim(mv_arrObjFields(v_intSharp).ControlType)
                    '                        Case "T"
                    '                            v_strSharp = CType(Me.pnTransDetail.Controls(mv_arrObjFields(v_intSharp).ControlIndex), TextBox).Text
                    '                        Case "M"
                    '                            v_strSharp = CType(Me.pnTransDetail.Controls(mv_arrObjFields(v_intSharp).ControlIndex), FlexMaskEditBox).Text
                    '                            v_strSharp = IIf(Trim(mv_arrObjFields(v_intSharp).DataType) = "D", v_strSharp.Replace("/  /", ""), v_strSharp)
                    '                        Case "C"
                    '                            v_strSharp = CType(Me.pnTransDetail.Controls(mv_arrObjFields(v_intSharp).ControlIndex), ComboBoxEx).SelectedValue.ToString
                    '                    End Select
                    '                    If Trim(v_strSharp) = "" And InStr(v_strSEARCHSQL, "#" & Format(v_intSharp + 1, "00"), CompareMethod.Text) > 0 Then
                    '                        v_strMsgErr = "Bạn chưa nhập " & mv_arrObjFields(v_intSharp).Caption
                    '                        Me.pnTransDetail.Controls(mv_arrObjFields(intIndex).LabelIndex).Visible = True
                    '                        Me.pnTransDetail.Controls(mv_arrObjFields(intIndex).LabelIndex).Text = v_strMsgErr
                    '                        Me.pnTransDetail.Controls(mv_arrObjFields(intIndex).LabelIndex).ForeColor = System.Drawing.Color.Red
                    '                        Exit Function
                    '                    End If
                    '                    v_strSEARCHSQL = v_strSEARCHSQL.Replace("#" & Format(v_intSharp + 1, "00"), v_strSharp.Trim)
                    '                End If
                    '            Next
                    '            'Kiá»ƒm tra thÃ´ng tin cá»§a nháº­p vÃ o cÃ³ Ä‘Ãºng khÃ´ng?
                    '            v_strSQLCMD = "SELECT * FROM (" & v_strSEARCHSQL & ")"
                    '            If Not (InStr(v_strSEARCHSQL, "#")) > 0 Then
                    '                'v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsNotLocalMsg, gc_MsgTypeObj, OBJNAME_SA_LOOKUP, gc_ActionInquiry, v_strSQLCMD, "")
                    '                v_strObjMsg = BuildXMLObjMsg(Me.BusDate, Me.BranchId, , Me.TellerId, gc_IsNotLocalMsg, gc_MsgTypeObj, "TR." & v_strSEARCHCODE, _
                    '                                            gc_ActionInquiry, v_strSQLCMD, , , True, StockFilter & "|" & MemberFilter)

                    '                v_lngError = mv_BDSDelivery.Message(v_strObjMsg)
                    '                If v_lngError <> ERR_SYSTEM_OK Then
                    '                    MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                    '                    Return 0
                    '                End If

                    '                'LÆ°u trá»¯ danh sÃ¡ch tÃ¬m kiáº¿m tráº£ vá»?
                    '                v_strFULLDATA = v_strObjMsg
                    '                v_xmlObjDocument.LoadXml(v_strObjMsg)
                    '                v_nodeList = v_xmlObjDocument.SelectNodes("/ObjectMessage/ObjData")
                    '                'Kiá»ƒm tra xem giÃ¡ trá»‹ co há»£p lá»‡ khÃ´ng
                    '                For i = 0 To v_nodeList.Count - 1
                    '                    For j = 0 To v_nodeList.Item(i).ChildNodes.Count - 1 Step 1
                    '                        With v_nodeList.Item(i).ChildNodes(j)
                    '                            Select Case CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                    '                                Case v_strKeyName
                    '                                    arrCoFields(intIndex) = arrCoFields(intIndex) & Chr(1) & Trim(.InnerText.ToString)
                    '                                    arrDisplayCoFields(intIndex) = arrDisplayCoFields(intIndex) & Chr(1) & Trim(.InnerText.ToString)
                    '                                    If v_strKeyName = v_strFIELDCODE Then
                    '                                        arrDisplayCoFields(intIndex) = arrDisplayCoFields(intIndex) & Chr(1) & Trim(.InnerText.ToString)
                    '                                    End If
                    '                                Case v_strFIELDCODE
                    '                                    arrDisplayCoFields(intIndex) = arrDisplayCoFields(intIndex) & Chr(1) & Trim(.InnerText.ToString)
                    '                            End Select
                    '                        End With
                    '                    Next j
                    '                Next i
                    '            Else
                    '                arrDisplayCoFields(intIndex) = "#"
                    '            End If
                    '        End If
                End If
                arrCoFields(intIndex) = arrCoFields(intIndex) & Chr(1)
                arrDisplayCoFields(intIndex) = arrDisplayCoFields(intIndex) & Chr(1)
            Next
            Dim strDisplayTmp, strValueTmp As String

            strConDuplicated = ""
            strRows = ""

            v_dataElement = v_xmlDocument.CreateElement(Xml.XmlNodeType.Element, "fields", "")
            mv_intErrCount = 0
            For i = 1 To arrValue.GetLength(0)
                strErrMsg = strErrMsg & "---oOo---" & vbCrLf & "Dòng " & i + intRealRow & " : " & vbCrLf
                strRowDuplicated = ""
                strInsertFields = ""
                strInsertValues = ""

                For j = 1 To arrValue.GetLength(1)
                    strDisplayTmp = ""
                    strValueTmp = ""
                    If mv_arrObjCoFields(j - 1).DataType = "N" Then
                        If mv_arrObjCoFields(j - 1).Mandatory And arrValue(i, j) Is Nothing Then
                            strErrMsg = strErrMsg & "Cột """ & mv_arrObjCoFields(j - 1).Caption.Replace(":", "") & """" & " của dòng " & i + intRealRow & " : " & mv_ResourceManager.GetString("ERR_NULL_VALUE") & vbCrLf
                            mv_intErrCount = mv_intErrCount + 1
                        End If
                        If Not arrValue(i, j) Is Nothing Then
                            If arrValue(i, j).GetType.ToString = "System.Double" Then
                                If arrCoFields(j - 1) <> Chr(1) Then
                                    If InStr(arrCoFields(j - 1), Chr(1) & Trim(arrValue(i, j) & Chr(1))) = 0 Then
                                        strErrMsg = strErrMsg & "Cột """ & mv_arrObjCoFields(j - 1).Caption.Replace(":", "") & """" & " của dòng " & i + intRealRow & " : " & mv_ResourceManager.GetString("ERR_LOOKUP_VALUE") & vbCrLf
                                        mv_intErrCount = mv_intErrCount + 1
                                    End If
                                End If
                                If mv_arrObjCoFields(j - 1).FieldName = strChildMsgAmt Then
                                    If dblTranLimit <> 0 And dblTranLimit < arrValue(i, j) Then
                                        strErrMsg = strErrMsg & "Cột """ & mv_arrObjCoFields(j - 1).Caption.Replace(":", "") & """" & " của dòng " & i + intRealRow & " : " & mv_ResourceManager.GetString("ERR_LIMIT_VALUE") & dblTranLimit & vbCrLf
                                        mv_intErrCount = mv_intErrCount + 1
                                    End If

                                End If

                                If (mv_arrObjCoFields(j - 1).SearchCode.Length > 0 And arrDisplayCoFields(j - 1) <> "#" & Chr(1)) _
                                            Or mv_arrObjCoFields(j - 1).LookupList.Length > 0 Then
                                    Dim strTmpDisplay As String
                                    Dim itmp, jtmp As Integer
                                    strTmpDisplay = Chr(1) & Trim(arrValue(i, j)) & Chr(1)
                                    If InStr(1, arrDisplayCoFields(j - 1), strTmpDisplay) > 0 Then
                                        itmp = InStr(1, arrDisplayCoFields(j - 1), strTmpDisplay) + Len(strTmpDisplay)
                                        jtmp = InStr(itmp, arrDisplayCoFields(j - 1), Chr(1))
                                        strDisplayTmp = Mid(arrDisplayCoFields(j - 1), itmp, jtmp - itmp)
                                    End If
                                Else
                                    strDisplayTmp = Trim(arrValue(i, j))
                                End If
                                strValueTmp = arrValue(i, j).ToString
                            Else
                                strErrMsg = strErrMsg & "Cột """ & mv_arrObjCoFields(j - 1).Caption.Replace(":", "") & """" & " của dòng " & i + intRealRow & " : " & mv_ResourceManager.GetString("ERR_NUMERIC_VALUE") & vbCrLf
                                mv_intErrCount = mv_intErrCount + 1
                            End If
                        End If
                    ElseIf mv_arrObjCoFields(j - 1).DataType = "C" Then
                        If mv_arrObjCoFields(j - 1).Mandatory And arrValue(i, j) Is Nothing Then
                            strErrMsg = strErrMsg & "Cột """ & mv_arrObjCoFields(j - 1).Caption.Replace(":", "") & """" & " của dòng " & i + intRealRow & " : " & mv_ResourceManager.GetString("ERR_NULL_VALUE") & vbCrLf
                            mv_intErrCount = mv_intErrCount + 1
                        End If
                        If Not arrValue(i, j) Is Nothing Then
                            If arrValue(i, j).GetType.ToString = "System.String" Then
                                If arrCoFields(j - 1) <> Chr(1) Then
                                    If InStr(arrCoFields(j - 1), Chr(1) & Trim(arrValue(i, j) & Chr(1))) = 0 Then
                                        strErrMsg = strErrMsg & "Cột """ & mv_arrObjCoFields(j - 1).Caption.Replace(":", "") & """" & " của dòng " & i + intRealRow & " : " & mv_ResourceManager.GetString("ERR_LOOKUP_VALUE") & vbCrLf
                                        mv_intErrCount = mv_intErrCount + 1
                                    End If
                                End If

                                If (mv_arrObjCoFields(j - 1).SearchCode.Length > 0 And arrDisplayCoFields(j - 1) <> "#" & Chr(1)) _
                                            Or mv_arrObjCoFields(j - 1).LookupList.Length > 0 Then
                                    Dim strTmpDisplay As String
                                    Dim itmp, jtmp As Integer
                                    strTmpDisplay = Chr(1) & Trim(arrValue(i, j)) & Chr(1)
                                    If InStr(1, arrDisplayCoFields(j - 1), strTmpDisplay) > 0 Then
                                        itmp = InStr(1, arrDisplayCoFields(j - 1), strTmpDisplay) + Len(strTmpDisplay)
                                        jtmp = InStr(itmp, arrDisplayCoFields(j - 1), Chr(1))
                                        strDisplayTmp = Mid(arrDisplayCoFields(j - 1), itmp, jtmp - itmp)
                                    End If
                                Else
                                    strDisplayTmp = Trim(arrValue(i, j))

                                End If

                                strValueTmp = arrValue(i, j).ToString.Replace("'", "''")
                                strDisplayTmp = Replace(strDisplayTmp, "'", "''")
                            Else
                                strErrMsg = strErrMsg & "Cột """ & mv_arrObjCoFields(j - 1).Caption.Replace(":", "") & """" & " của dòng " & i + intRealRow & " : " & mv_ResourceManager.GetString("ERR_NUMERIC_VALUE") & vbCrLf
                                mv_intErrCount = mv_intErrCount + 1
                            End If
                        End If
                    ElseIf mv_arrObjCoFields(j - 1).DataType = "D" Then
                        If mv_arrObjCoFields(j - 1).Mandatory And arrValue(i, j) Is Nothing Then
                            strErrMsg = strErrMsg & "Cột """ & mv_arrObjCoFields(j - 1).Caption.Replace(":", "") & """" & " của dòng " & i + intRealRow & " : " & mv_ResourceManager.GetString("ERR_NULL_VALUE") & vbCrLf
                            mv_intErrCount = mv_intErrCount + 1
                        End If
                        If Not arrValue(i, j) Is Nothing Then
                            If arrValue(i, j).GetType.ToString = "System.DateTime" Then
                                If arrCoFields(j - 1) <> Chr(1) Then
                                    If InStr(arrCoFields(j - 1), Chr(1) & Trim(arrValue(i, j) & Chr(1))) = 0 Then
                                        strErrMsg = strErrMsg & "Cột """ & mv_arrObjCoFields(j - 1).Caption.Replace(":", "") & """" & " của dòng " & i + intRealRow & " : " & mv_ResourceManager.GetString("ERR_LOOKUP_VALUE") & vbCrLf
                                        mv_intErrCount = mv_intErrCount + 1
                                    End If
                                End If

                                If (mv_arrObjCoFields(j - 1).SearchCode.Length > 0 And arrDisplayCoFields(j - 1) <> "#" & Chr(1)) _
                                        Or mv_arrObjCoFields(j - 1).LookupList.Length > 0 Then
                                    Dim strTmpDisplay As String
                                    Dim itmp, jtmp As Integer
                                    strTmpDisplay = Chr(1) & Trim(arrValue(i, j)) & Chr(1)
                                    If InStr(1, arrDisplayCoFields(j - 1), strTmpDisplay) > 0 Then
                                        itmp = InStr(1, arrDisplayCoFields(j - 1), strTmpDisplay) + Len(strTmpDisplay)
                                        jtmp = InStr(itmp, arrDisplayCoFields(j - 1), Chr(1))
                                        strDisplayTmp = Mid(arrDisplayCoFields(j - 1), itmp, jtmp - itmp)
                                    End If
                                Else
                                    strDisplayTmp = Trim(arrValue(i, j))
                                End If

                                strValueTmp = CDate(arrValue(i, j)).ToString("dd/MM/yyyy")
                            Else
                                strErrMsg = strErrMsg & "Cột """ & mv_arrObjCoFields(j - 1).Caption.Replace(":", "") & """" & " của dòng " & i + intRealRow & " : " & mv_ResourceManager.GetString("ERR_NUMERIC_VALUE") & vbCrLf
                                mv_intErrCount = mv_intErrCount + 1
                            End If
                        End If
                    End If
                    If mv_arrObjCoFields(j - 1).IsDuplicated = "Y" Then
                        'modified by bangpv: loai bo dau '
                        strRowDuplicated = strRowDuplicated & arrValue(i, j)
                    End If
                    strInsertFields = strInsertFields & ",COL_VALUE" & mv_arrObjCoFields(j - 1).FieldName _
                                        & ", COL_TYPE" & mv_arrObjCoFields(j - 1).FieldName _
                                        & ", COL_DESC" & mv_arrObjCoFields(j - 1).FieldName
                    'strInsertValues = strInsertValues & ",'" & strValueTmp & "'" & " COL_VALUE" & mv_arrObjCoFields(j - 1).FieldName & ",'" _
                    '                    & mv_arrObjCoFields(j - 1).DataType & "'" & " COL_TYPE" & mv_arrObjCoFields(j - 1).FieldName & ",'" _
                    '                    & strDisplayTmp & "'" & " COL_DESC" & mv_arrObjCoFields(j - 1).FieldName
                    'BằngPV sửa 28/04/2009: sửa lỗi dấu ' trong file Excel
                    strInsertValues = strInsertValues & ",'" & strValueTmp & "'" & " COL_VALUE" & mv_arrObjCoFields(j - 1).FieldName & ",'" _
                                        & mv_arrObjCoFields(j - 1).DataType & "'" & " COL_TYPE" & mv_arrObjCoFields(j - 1).FieldName & ",'" _
                                        & strDisplayTmp & "'" & " COL_DESC" & mv_arrObjCoFields(j - 1).FieldName
                    'BangPV
                Next
                If strRowDuplicated <> "" Then
                    strConDuplicated = strConDuplicated & Chr(1) & i + intRealRow & Chr(1) & strRowDuplicated & Chr(1)
                End If
                strErrMsg = strErrMsg & "@" & i + intRealRow & vbCrLf & "---oOo---" & vbCrLf
                strRows = strRows & "UNION SELECT '" & mv_strCOTLTXCD & "' TLTXCD " & v_strValuesSQL & " " & strInsertValues & " , " & i + intRealRow & " as REAL_ROW FROM DUAL " & vbCrLf
                If i Mod gc_IMP_TRAN_UNIT = 0 Then
                    'Append entry to data node
                    v_entryNode = v_xmlDocument.CreateNode(Xml.XmlNodeType.Element, "entry", "")
                    'Add field name
                    v_attrFLDNAME = v_xmlDocument.CreateAttribute(gc_AtributeFLDNAME)
                    v_attrFLDNAME.Value = "INSERT_ROW"
                    v_entryNode.Attributes.Append(v_attrFLDNAME)
                    'Add field type
                    v_attrDATATYPE = v_xmlDocument.CreateAttribute(gc_AtributeFLDTYPE)
                    v_attrDATATYPE.Value = "C"
                    v_entryNode.Attributes.Append(v_attrDATATYPE)
                    'Set value
                    strRows = "INSERT INTO TMP_TXFIELDS(AUTOID, TLTXCD " & v_strFieldsSQL & " " & strInsertFields & ", REAL_ROW ) " _
                        & " SELECT seq_tmp_txfields.nextval , a.* from ( " & strRows.Substring(6) & " ) a "

                    v_entryNode.InnerText = strRows
                    v_dataElement.AppendChild(v_entryNode)
                    strRows = ""
                End If
            Next
            If strRows <> "" Then
                v_entryNode = v_xmlDocument.CreateNode(Xml.XmlNodeType.Element, "entry", "")
                'Add field name
                v_attrFLDNAME = v_xmlDocument.CreateAttribute(gc_AtributeFLDNAME)
                v_attrFLDNAME.Value = "INSERT_ROW"
                v_entryNode.Attributes.Append(v_attrFLDNAME)
                'Add field type
                v_attrDATATYPE = v_xmlDocument.CreateAttribute(gc_AtributeFLDTYPE)
                v_attrDATATYPE.Value = "C"
                v_entryNode.Attributes.Append(v_attrDATATYPE)
                'Set value
                strRows = "INSERT INTO TMP_TXFIELDS(AUTOID, TLTXCD " & v_strFieldsSQL & " " & strInsertFields & ", REAL_ROW ) " _
                    & " SELECT seq_tmp_txfields.nextval , a.* from (" & strRows.Substring(6) & ") a "

                v_entryNode.InnerText = strRows
                v_dataElement.AppendChild(v_entryNode)

                'v_xmlDocument.DocumentElement.AppendChild(v_dataElement)
            End If

            If arrValue.GetLength(0) > 0 Then
                v_xmlDocument.DocumentElement.AppendChild(v_dataElement)
            End If
            ' lay tieu chi filter tu mv_arrObjCoFldvals
            CheckFldvals(mv_arrObjCoFldVals, arrValue, strErrMsg, intRealRow)

            ' liet ke dong trung nhau 
            If strConDuplicated <> "" Then
                Dim intPos, intSplit, intTimes As Integer
                Dim strCheck, strMsg, strChecMsg As String

                For i = 1 To arrValue.GetLength(0)
                    strCheck = ""
                    strChecMsg = ""
                    For j = 1 To arrValue.GetLength(1)
                        If mv_arrObjCoFields(j - 1).IsDuplicated = "Y" Then
                            strCheck = strCheck & arrValue(i, j)
                            strChecMsg = strChecMsg & ";" & mv_arrObjCoFields(j - 1).Caption & " " & arrValue(i, j)
                        End If
                    Next
                    strCheck = Chr(1) & strCheck & Chr(1)
                    intPos = 1
                    strMsg = ""
                    intTimes = 0
                    While True
                        intPos = InStr(intPos, strConDuplicated, strCheck)
                        If intPos <> 0 Then
                            intSplit = InStrRev(strConDuplicated, Chr(1), intPos - 2)
                            strMsg = strMsg & "," & strConDuplicated.Substring(intSplit, intPos - intSplit - 1)
                            intTimes = intTimes + 1
                        Else
                            Exit While
                        End If
                        intPos = intPos + 1
                    End While
                    If strMsg <> "" And intTimes >= 2 Then
                        strErrMsg = strErrMsg & "Các dòng (" & strMsg.Substring(1) & ") có giá trị trùng nhau ( " & strChecMsg.Substring(1) & ")" & vbCrLf
                        mv_intErrCount = mv_intErrCount + 1
                    End If
                    strConDuplicated = strConDuplicated.Replace(strCheck, "")
                Next
            End If

            For i = 1 To arrValue.GetLength(0)
                strErrMsg = strErrMsg.Replace("---oOo---" & vbCrLf & "Dòng " & i + intRealRow & " : " & vbCrLf & "@" & i + intRealRow & vbCrLf & "---oOo---" & vbCrLf, "")
                strErrMsg = strErrMsg.Replace("@" & i + intRealRow & vbCrLf, "")
            Next

            If Trim(strErrMsg) <> "" Then
                v_strErrMsg = strErrMsg
                ' thong bao loi
                Return False
            Else
                v_xmlDocument.DocumentElement.Attributes(gc_AtributeMSGAMT).InnerXml = strChildMsgAmt
                Return True
            End If

        Catch ex As Exception
            Throw ex
        Finally
            If Not v_obj Is Nothing Then
                v_obj.CloseConnection()
            End If
            v_obj = Nothing
            If Not v_ds Is Nothing Then
                v_ds.Dispose()
            End If
            xlBook.Close(SaveChanges:=False)
            xlApp.Quit()
            'cleaning up
            xlSheet = Nothing
            xlBook = Nothing
            xlApp = Nothing
            GC.Collect()
            GC.WaitForPendingFinalizers()
            GC.Collect()
            GC.WaitForPendingFinalizers()
        End Try
    End Function

    Private Function CheckChildTranRules(ByVal v_xmlDocument As Xml.XmlDocument, _
                                       ByVal strFileName As String, _
                                       ByVal strRange As String, _
                                       ByVal v_strFieldsSQL As String, _
                                       ByVal v_strValuesSQL As String, _
                                       ByRef v_strErrMsg As String, _
                                       ByVal strTXNUM As String, _
                                       ByVal strTXDATE As String) As Boolean
        Dim xlApp As Microsoft.Office.Interop.Excel.Application
        Dim xlBook As Microsoft.Office.Interop.Excel.Workbook
        Dim xlSheet As Microsoft.Office.Interop.Excel.Worksheet
        Dim xlData As Microsoft.Office.Interop.Excel.Range
        Dim arrValue As System.Array
        Dim v_dataElement As Xml.XmlElement, v_entryNode As Xml.XmlNode

        Dim strErrMsg As String = ""
        Dim intIndex, intCount, intVIndex, intVCount, i, j, intRealRow As Integer
        Dim v_strSQLCMD, v_strFULLDATA, v_strObjMsg, v_strValue, v_strSEARCHCODE, _
            v_strFIELDCODE, v_strFLDNAME, v_strKeyName, v_strKeyVal, v_strSEARCHSQL As String
        Dim v_nodeList As Xml.XmlNodeList
        Dim arrCoFields(), arrDisplayCoFields() As String
        Dim strConDuplicated, strRowDuplicated As String
        Dim strInsertFields, strInsertValues As String
        Dim dblTranLimit As Double
        Dim v_xmlObjDocument As New Xml.XmlDocument
        Dim strChildMsgAmt As String
        Dim strRows As String = ""
        Dim v_attrFLDNAME As Xml.XmlAttribute, v_attrDATATYPE As Xml.XmlAttribute

        Dim v_intSharp, v_intCount As Integer
        Dim v_strSharp As String
        Dim v_strMsgErr As String
        Dim v_lngError As Long
        Dim v_blnEmptyRow As Boolean = False
        Dim v_obj As SQLDataAccessLayer
        Dim v_ds As DataSet
        Try
            ' kiem tra xem nguoi dung co duoc thuc hien giao dich nay khong, lay thong tin han muc giao dich
            If Not CheckTranLimit(mv_strCOTLTXCD, 0, dblTranLimit, strChildMsgAmt) Then
                Return False
            End If
            ' load data
            'chuyển sang en-us để đọc định dạng file excel
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US")
            xlApp = New Microsoft.Office.Interop.Excel.Application
            xlBook = xlApp.Workbooks.Open(strFileName)
            xlSheet = CType(xlBook.Worksheets(1), Microsoft.Office.Interop.Excel.Worksheet)
            xlData = xlSheet.Range(strRange.Insert(6, ":").Replace(" ", ""))
            arrValue = CType(xlData.Value, Array)
            If strRange.Substring(1, 1) <= "Z" And strRange.Substring(1, 1) >= "A" Then
                intRealRow = CInt(Trim(strRange.Substring(2, 4))) - 1
            Else
                intRealRow = CInt(Trim(strRange.Substring(1, 5))) - 1
            End If

            'Hanm5 sửa: check trong file import có dòng dữ liệu trắng hay không.
            mv_intErrCount = 0
            For i = 1 To arrValue.GetLength(0)
                For j = 1 To arrValue.GetLength(1)
                    If Not arrValue.GetValue(i, j) Is Nothing Then
                        v_blnEmptyRow = True
                    End If
                Next
                If v_blnEmptyRow = False Then
                    strErrMsg = strErrMsg & "---oOo---" & vbCrLf & "Dòng " & i + intRealRow & " không có dữ liệu" & vbCrLf
                    mv_intErrCount = mv_intErrCount + 1
                End If
                v_blnEmptyRow = False
            Next

            If Trim(strErrMsg) <> "" Then
                v_strErrMsg = strErrMsg
                Return False
            End If
            'Kết thúc : Hanm5 sửa

            ' lay cac tieu chi filter tu mv_arrObjCoFields
            intCount = mv_arrObjCoFields.GetLength(0) - 1
            ReDim arrCoFields(intCount)
            ReDim arrDisplayCoFields(intCount)
            v_obj = New SQLDataAccessLayer(TellerId)
            For intIndex = 0 To intCount - 1
                If mv_arrObjCoFields(intIndex).LookupList <> "" And mv_arrObjCoFields(intIndex).Mandatory = True Then
                    v_strSQLCMD = mv_arrObjCoFields(intIndex).LookupList
                    If InStr(v_strSQLCMD, "#", CompareMethod.Text) = 0 Then
                        v_ds = v_obj.ExecuteReturnDataSet(v_strSQLCMD)
                        For i = 0 To v_ds.Tables(0).Rows.Count - 1
                            With v_ds.Tables(0)
                                arrCoFields(intIndex) = arrCoFields(intIndex) & Chr(1) & Trim(.Rows(i)("VALUE"))
                                arrDisplayCoFields(intIndex) = arrDisplayCoFields(intIndex) & Chr(1) & Trim(.Rows(i)("DISPLAY"))
                            End With
                        Next i
                    End If
                End If
                arrCoFields(intIndex) = arrCoFields(intIndex) & Chr(1)
                arrDisplayCoFields(intIndex) = arrDisplayCoFields(intIndex) & Chr(1)
            Next
            Dim strDisplayTmp, strValueTmp As String
            Dim v_strCellValue As String
            strConDuplicated = ""
            strRows = ""

            v_dataElement = v_xmlDocument.CreateElement(Xml.XmlNodeType.Element, "fields", "")
            mv_intErrCount = 0
            For i = 1 To arrValue.GetLength(0)
                strErrMsg = strErrMsg & "---oOo---" & vbCrLf & "Dòng " & i + intRealRow & " : " & vbCrLf
                strRowDuplicated = ""
                strInsertFields = ""
                strInsertValues = ""

                For j = 1 To arrValue.GetLength(1)
                    strDisplayTmp = ""
                    strValueTmp = ""
                    If Trim(gf_CorrectStringField(arrValue(i, j))) <> "" Then
                        If arrValue(i, j).GetType.Name = "DateTime" Then
                            v_strCellValue = Trim(gf_CorrectStringField(Format(arrValue(i, j), "dd/MM/yyyy")))
                        Else
                            v_strCellValue = Trim(gf_CorrectStringField(arrValue(i, j)))
                        End If
                    Else
                        v_strCellValue = Trim(gf_CorrectStringField(arrValue(i, j)))
                    End If
                    If v_strCellValue <> "" Then
                        v_strCellValue = Replace(v_strCellValue, "'", "''")
                        If Len(v_strCellValue.ToString) > mv_arrObjCoFields(j - 1).MaxFldLen Then
                            strErrMsg = strErrMsg & "Cột """ & mv_arrObjCoFields(j - 1).Caption.Replace(":", "") & """" & " của dòng " _
                                                  & i + intRealRow & " : " & Replace(mv_ResourceManager.GetString("ERR_OVER_MAXFLDLEN"), "@MAXFLDLEN", mv_arrObjCoFields(j - 1).MaxFldLen.ToString) & vbCrLf
                            mv_intErrCount = mv_intErrCount + 1
                        End If
                    End If
                    If mv_arrObjCoFields(j - 1).Mandatory And v_strCellValue = "" Then
                        strErrMsg = strErrMsg & "Cột """ & mv_arrObjCoFields(j - 1).Caption.Replace(":", "") & """" & " của dòng " & i + intRealRow & " : " & mv_ResourceManager.GetString("ERR_NULL_VALUE") & vbCrLf
                        mv_intErrCount = mv_intErrCount + 1
                    ElseIf mv_arrObjCoFields(j - 1).DataType = "N" Then
                        If Not IsNumeric(v_strCellValue) Then
                            strErrMsg = strErrMsg & "Cột """ & mv_arrObjCoFields(j - 1).Caption.Replace(":", "") & """" & " của dòng " & i + intRealRow & " : " & mv_ResourceManager.GetString("ERR_NUMERIC_VALUE") & vbCrLf
                            mv_intErrCount = mv_intErrCount + 1
                        Else
                            If CDbl(v_strCellValue) < 0 And InStr(mv_arrObjCoFields(j - 1).ColumnName, "_N_") = 0 Then
                                strErrMsg = strErrMsg & "Cột """ & mv_arrObjCoFields(j - 1).Caption.Replace(":", "") & """" & " của dòng " & i + intRealRow & " : " & mv_ResourceManager.GetString("ERR_NUMERIC_VALUE") & vbCrLf
                                mv_intErrCount = mv_intErrCount + 1
                            End If
                            If arrCoFields(j - 1) <> Chr(1) Then
                                If InStr(arrCoFields(j - 1), Chr(1) & Trim(v_strCellValue & Chr(1))) = 0 Then
                                    strErrMsg = strErrMsg & "Cột """ & mv_arrObjCoFields(j - 1).Caption.Replace(":", "") & """" & " của dòng " & i + intRealRow & " : " & mv_ResourceManager.GetString("ERR_LOOKUP_VALUE") & vbCrLf
                                    mv_intErrCount = mv_intErrCount + 1
                                End If
                            End If
                            If mv_arrObjCoFields(j - 1).FieldName = strChildMsgAmt Then
                                If dblTranLimit <> 0 And dblTranLimit < CDbl(v_strCellValue) Then
                                    strErrMsg = strErrMsg & "Cột """ & mv_arrObjCoFields(j - 1).Caption.Replace(":", "") & """" & " của dòng " & i + intRealRow & " : " & mv_ResourceManager.GetString("ERR_LIMIT_VALUE") & dblTranLimit & vbCrLf
                                    mv_intErrCount = mv_intErrCount + 1
                                End If
                            End If

                            If (mv_arrObjCoFields(j - 1).SearchCode.Length > 0 And arrDisplayCoFields(j - 1) <> "#" & Chr(1)) _
                                        Or mv_arrObjCoFields(j - 1).LookupList.Length > 0 Then
                                Dim strTmpDisplay As String
                                Dim itmp, jtmp As Integer
                                strTmpDisplay = Chr(1) & Trim(v_strCellValue) & Chr(1)
                                If InStr(1, arrDisplayCoFields(j - 1), strTmpDisplay) > 0 Then
                                    itmp = InStr(1, arrDisplayCoFields(j - 1), strTmpDisplay) + Len(strTmpDisplay)
                                    jtmp = InStr(itmp, arrDisplayCoFields(j - 1), Chr(1))
                                    strDisplayTmp = Mid(arrDisplayCoFields(j - 1), itmp, jtmp - itmp)
                                End If
                            Else
                                strDisplayTmp = Trim(v_strCellValue)
                            End If
                            strValueTmp = v_strCellValue
                        End If

                    ElseIf mv_arrObjCoFields(j - 1).DataType = "C" Then
                        If arrCoFields(j - 1) <> Chr(1) Then
                            If InStr(arrCoFields(j - 1), Chr(1) & Trim(v_strCellValue) & Chr(1)) = 0 Then
                                strErrMsg = strErrMsg & "Cột """ & mv_arrObjCoFields(j - 1).Caption.Replace(":", "") & """" & " của dòng " & i + intRealRow & " : " & mv_ResourceManager.GetString("ERR_LOOKUP_VALUE") & vbCrLf
                                mv_intErrCount = mv_intErrCount + 1
                            End If
                        End If

                        If (mv_arrObjCoFields(j - 1).SearchCode.Length > 0 And arrDisplayCoFields(j - 1) <> "#" & Chr(1)) _
                                            Or mv_arrObjCoFields(j - 1).LookupList.Length > 0 Then
                            Dim strTmpDisplay As String
                            Dim itmp, jtmp As Integer
                            strTmpDisplay = Chr(1) & Trim(v_strCellValue) & Chr(1)
                            If InStr(1, arrDisplayCoFields(j - 1), strTmpDisplay) > 0 Then
                                itmp = InStr(1, arrDisplayCoFields(j - 1), strTmpDisplay) + Len(strTmpDisplay)
                                jtmp = InStr(itmp, arrDisplayCoFields(j - 1), Chr(1))
                                strDisplayTmp = Mid(arrDisplayCoFields(j - 1), itmp, jtmp - itmp)
                            End If
                        Else
                            strDisplayTmp = Trim(v_strCellValue)

                        End If

                        strValueTmp = v_strCellValue
                        strDisplayTmp = strDisplayTmp

                    ElseIf mv_arrObjCoFields(j - 1).DataType = "D" Then
                        If mv_arrObjCoFields(j - 1).Mandatory = True And Not IsDateValue(v_strCellValue) Then
                            strErrMsg = strErrMsg & "Cột """ & mv_arrObjCoFields(j - 1).Caption.Replace(":", "") & """" & " của dòng " & i + intRealRow & " : " & mv_ResourceManager.GetString("ERR_DATE_VALUE") & vbCrLf
                            mv_intErrCount = mv_intErrCount + 1
                        Else

                            If arrCoFields(j - 1) <> Chr(1) Then
                                If InStr(arrCoFields(j - 1), Chr(1) & Trim(v_strCellValue & Chr(1))) = 0 Then
                                    strErrMsg = strErrMsg & "Cột """ & mv_arrObjCoFields(j - 1).Caption.Replace(":", "") & """" & " của dòng " & i + intRealRow & " : " & mv_ResourceManager.GetString("ERR_LOOKUP_VALUE") & vbCrLf
                                    mv_intErrCount = mv_intErrCount + 1
                                End If
                            End If

                            If (mv_arrObjCoFields(j - 1).SearchCode.Length > 0 And arrDisplayCoFields(j - 1) <> "#" & Chr(1)) _
                                    Or mv_arrObjCoFields(j - 1).LookupList.Length > 0 Then
                                Dim strTmpDisplay As String
                                Dim itmp, jtmp As Integer
                                strTmpDisplay = Chr(1) & Trim(v_strCellValue) & Chr(1)
                                If InStr(1, arrDisplayCoFields(j - 1), strTmpDisplay) > 0 Then
                                    itmp = InStr(1, arrDisplayCoFields(j - 1), strTmpDisplay) + Len(strTmpDisplay)
                                    jtmp = InStr(itmp, arrDisplayCoFields(j - 1), Chr(1))
                                    strDisplayTmp = Mid(arrDisplayCoFields(j - 1), itmp, jtmp - itmp)
                                End If
                            Else
                                strDisplayTmp = Trim(v_strCellValue)
                            End If
                            strValueTmp = v_strCellValue
                        End If
                    End If

                    If mv_arrObjCoFields(j - 1).IsDuplicated = "Y" Then
                        'modified by bangpv: loai bo dau '
                        strRowDuplicated = strRowDuplicated & v_strCellValue
                    End If
                    strInsertFields = strInsertFields & ",COL_VALUE" & mv_arrObjCoFields(j - 1).FieldName _
                                        & ", COL_TYPE" & mv_arrObjCoFields(j - 1).FieldName _
                                        & ", COL_DESC" & mv_arrObjCoFields(j - 1).FieldName
                    'strInsertValues = strInsertValues & ",'" & strValueTmp & "'" & " COL_VALUE" & mv_arrObjCoFields(j - 1).FieldName & ",'" _
                    '                    & mv_arrObjCoFields(j - 1).DataType & "'" & " COL_TYPE" & mv_arrObjCoFields(j - 1).FieldName & ",'" _
                    '                    & strDisplayTmp & "'" & " COL_DESC" & mv_arrObjCoFields(j - 1).FieldName
                    'BằngPV sửa 28/04/2009: sửa lỗi dấu ' trong file Excel
                    strInsertValues = strInsertValues & ",'" & strValueTmp & "'" & " COL_VALUE" & mv_arrObjCoFields(j - 1).FieldName & ",'" _
                                        & mv_arrObjCoFields(j - 1).DataType & "'" & " COL_TYPE" & mv_arrObjCoFields(j - 1).FieldName & ",'" _
                                        & strDisplayTmp & "'" & " COL_DESC" & mv_arrObjCoFields(j - 1).FieldName
                    'BangPV
                Next
                If strRowDuplicated <> "" Then
                    strConDuplicated = strConDuplicated & Chr(1) & "@" & i + intRealRow & Chr(1) & strRowDuplicated & Chr(1)
                End If
                strErrMsg = strErrMsg & "@" & i + intRealRow & vbCrLf & "---oOo---" & vbCrLf
                strRows = strRows & "UNION SELECT '" & mv_strCOTLTXCD & "' TLTXCD " & v_strValuesSQL & " " & strInsertValues & " , " & i + intRealRow & " as REAL_ROW FROM DUAL " & vbCrLf
                If i Mod gc_IMP_TRAN_UNIT = 0 Then
                    'Append entry to data node
                    v_entryNode = v_xmlDocument.CreateNode(Xml.XmlNodeType.Element, "entry", "")
                    'Add field name
                    v_attrFLDNAME = v_xmlDocument.CreateAttribute(gc_AtributeFLDNAME)
                    v_attrFLDNAME.Value = "INSERT_ROW"
                    v_entryNode.Attributes.Append(v_attrFLDNAME)
                    'Add field type
                    v_attrDATATYPE = v_xmlDocument.CreateAttribute(gc_AtributeFLDTYPE)
                    v_attrDATATYPE.Value = "C"
                    v_entryNode.Attributes.Append(v_attrDATATYPE)
                    'Set value
                    strRows = "INSERT INTO TMP_TXFIELDS(AUTOID, TLTXCD " & v_strFieldsSQL & " " & strInsertFields & ", REAL_ROW ) " _
                        & " SELECT seq_tmp_txfields.nextval , a.* from ( " & strRows.Substring(6) & " ) a "

                    v_entryNode.InnerText = strRows
                    v_dataElement.AppendChild(v_entryNode)
                    strRows = ""
                End If
            Next
            If strRows <> "" Then
                v_entryNode = v_xmlDocument.CreateNode(Xml.XmlNodeType.Element, "entry", "")
                'Add field name
                v_attrFLDNAME = v_xmlDocument.CreateAttribute(gc_AtributeFLDNAME)
                v_attrFLDNAME.Value = "INSERT_ROW"
                v_entryNode.Attributes.Append(v_attrFLDNAME)
                'Add field type
                v_attrDATATYPE = v_xmlDocument.CreateAttribute(gc_AtributeFLDTYPE)
                v_attrDATATYPE.Value = "C"
                v_entryNode.Attributes.Append(v_attrDATATYPE)
                'Set value
                strRows = "INSERT INTO TMP_TXFIELDS(AUTOID, TLTXCD " & v_strFieldsSQL & " " & strInsertFields & ", REAL_ROW ) " _
                    & " SELECT seq_tmp_txfields.nextval , a.* from (" & strRows.Substring(6) & ") a "

                v_entryNode.InnerText = strRows
                v_dataElement.AppendChild(v_entryNode)

                'v_xmlDocument.DocumentElement.AppendChild(v_dataElement)
            End If

            If arrValue.GetLength(0) > 0 Then
                v_xmlDocument.DocumentElement.AppendChild(v_dataElement)
            End If
            ' lay tieu chi filter tu mv_arrObjCoFldvals
            CheckFldvals(mv_arrObjCoFldVals, arrValue, strErrMsg, intRealRow)

            ' liet ke dong trung nhau 
            If strConDuplicated <> "" Then
                Dim intPos, intSplit, intTimes As Integer
                Dim strCheck, strMsg, strChecMsg As String

                For i = 1 To arrValue.GetLength(0)
                    strCheck = ""
                    strChecMsg = ""
                    For j = 1 To arrValue.GetLength(1)
                        If mv_arrObjCoFields(j - 1).IsDuplicated = "Y" Then
                            v_strCellValue = Trim(gf_CorrectStringField(arrValue(i, j)))
                            If v_strCellValue <> "" Then
                                v_strCellValue = Replace(v_strCellValue, "'", "''")
                            End If
                            strCheck = strCheck & v_strCellValue
                            strChecMsg = strChecMsg & ";" & mv_arrObjCoFields(j - 1).Caption & " " & v_strCellValue
                        End If
                    Next
                    strCheck = Chr(1) & strCheck & Chr(1)
                    intPos = 1
                    strMsg = ""
                    intTimes = 0
                    While True
                        intPos = InStr(intPos, strConDuplicated, strCheck)
                        If intPos <> 0 Then
                            intSplit = InStrRev(strConDuplicated, Chr(1) & "@", intPos - 2)
                            strMsg = strMsg & "," & strConDuplicated.Substring(intSplit + 1, intPos - intSplit - 1)
                            intTimes = intTimes + 1
                        Else
                            Exit While
                        End If
                        intPos = intPos + 1
                    End While
                    If strMsg <> "" And intTimes >= 2 Then
                        strErrMsg = strErrMsg & "Các dòng (" & strMsg.Substring(1) & ") có giá trị trùng nhau ( " & strChecMsg.Substring(1) & ")" & vbCrLf
                        mv_intErrCount = mv_intErrCount + 1
                    End If
                    strConDuplicated = strConDuplicated.Replace(strCheck, "")
                Next
            End If

            For i = 1 To arrValue.GetLength(0)
                strErrMsg = strErrMsg.Replace("---oOo---" & vbCrLf & "Dòng " & i + intRealRow & " : " & vbCrLf & "@" & i + intRealRow & vbCrLf & "---oOo---" & vbCrLf, "")
                strErrMsg = strErrMsg.Replace("@" & i + intRealRow & vbCrLf, "")
            Next

            If Trim(strErrMsg) <> "" Then
                v_strErrMsg = strErrMsg
                ' thong bao loi
                Return False
            Else
                v_xmlDocument.DocumentElement.Attributes(gc_AtributeMSGAMT).InnerXml = strChildMsgAmt
                Return True
            End If

        Catch ex As Exception
            Throw ex
        Finally
            If Not v_obj Is Nothing Then
                v_obj.CloseConnection()
            End If
            v_obj = Nothing
            If Not v_ds Is Nothing Then
                v_ds.Dispose()
            End If
            xlBook.Close(SaveChanges:=False)
            xlApp.Quit()
            'cleaning up
            'bangpv
            arrValue = Nothing
            'end bangpv 
            xlSheet = Nothing
            xlBook = Nothing
            xlApp = Nothing
            'chuyển về định dạng thông thường của form giao dịch 
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("vi-VN")
            GC.Collect()
            GC.WaitForPendingFinalizers()
            GC.Collect()
            GC.WaitForPendingFinalizers()
        End Try
    End Function


    Private Function CheckChildTranRulesDataSet(ByVal v_xmlDocument As Xml.XmlDocument, _
                                         ByVal strFileName As String, _
                                         ByVal strType As String, _
                                         ByVal v_strFieldsSQL As String, _
                                         ByVal v_strValuesSQL As String, _
                                         ByRef v_strErrMsg As String, _
                                         ByVal strTXNUM As String, _
                                         ByVal strTXDATE As String) As Boolean

        Dim v_dataElement As Xml.XmlElement, v_entryNode As Xml.XmlNode

        Dim strErrMsg As String = ""
        Dim intIndex, intCount, intVIndex, intVCount, i, j, intRealRow As Integer
        Dim v_strSQLCMD, v_strFULLDATA, v_strObjMsg, v_strValue, v_strSEARCHCODE, _
            v_strFIELDCODE, v_strFLDNAME, v_strKeyName, v_strKeyVal, v_strSEARCHSQL As String
        Dim v_nodeList As Xml.XmlNodeList
        Dim arrCoFields(), arrDisplayCoFields() As String
        Dim strConDuplicated, strRowDuplicated As String
        Dim strInsertFields, strInsertValues As String
        Dim dblTranLimit As Double
        Dim v_xmlObjDocument As New Xml.XmlDocument
        Dim strChildMsgAmt As String
        Dim strRows As String = ""
        Dim v_attrFLDNAME As Xml.XmlAttribute, v_attrDATATYPE As Xml.XmlAttribute

        Dim v_intSharp, v_intCount As Integer
        Dim v_strSharp As String
        Dim v_strMsgErr As String
        Dim v_lngError As Long
        Dim v_obj As SQLDataAccessLayer
        Dim v_ds As DataSet
        Try
            ' kiem tra xem nguoi dung co duoc thuc hien giao dich nay khong, lay thong tin han muc giao dich
            If Not CheckTranLimit(mv_strCOTLTXCD, 0, dblTranLimit, strChildMsgAmt) Then
                Return False
            End If

            ' lay cac tieu chi filter tu mv_arrObjCoFields
            intCount = mv_arrObjCoFields.GetLength(0) - 1
            ReDim arrCoFields(intCount)
            ReDim arrDisplayCoFields(intCount)
            v_obj = New SQLDataAccessLayer(TellerId)
            For intIndex = 0 To intCount - 1
                If mv_arrObjCoFields(intIndex).LookupList <> "" Then
                    v_strSQLCMD = mv_arrObjCoFields(intIndex).LookupList
                    If InStr(v_strSQLCMD, "#", CompareMethod.Text) = 0 Then
                        v_ds = v_obj.ExecuteReturnDataSet(v_strSQLCMD)
                        For i = 0 To v_ds.Tables(0).Rows.Count - 1
                            With v_ds.Tables(0)
                                arrCoFields(intIndex) = arrCoFields(intIndex) & Chr(1) & Trim(.Rows(i)("VALUE"))
                                'arrDisplayCoFields(intIndex) = arrDisplayCoFields(intIndex) & Chr(1) & Trim(.Rows(i)("VALUE"))
                                arrDisplayCoFields(intIndex) = arrDisplayCoFields(intIndex) & Chr(1) & Trim(.Rows(i)("DISPLAY"))
                            End With
                        Next i
                    End If

                    'ElseIf mv_arrObjCoFields(intIndex).SearchCode <> "" Then
                    '    'ElseIf mv_arrObjCoFields(intIndex).SearchCode <> "" And Not (InStr(mv_arrObjCoFields(intIndex).SearchCode, "#") > 0) Then
                    '    v_strSEARCHCODE = mv_arrObjCoFields(intIndex).SearchCode
                    '    v_strSQLCMD = "SELECT A.FIELDCODE KEYNAME,B.SEARCHCMDSQL, C.FIELDCODE " _
                    '    & " FROM SISEARCHFLD A,SISEARCH B, (select * from SISEARCHFLD where REFVALUE ='Y') C " & ControlChars.CrLf _
                    '    & " WHERE B.SEARCHCODE = A.SEARCHCODE " & ControlChars.CrLf _
                    '    & " AND A.KEY ='Y' AND B.SEARCHCODE ='" & v_strSEARCHCODE & "'" _
                    '    & " AND B.SEARCHCODE = C.SEARCHCODE(+)"
                    '    v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_LOOKUP, gc_ActionInquiry, v_strSQLCMD)

                    '    v_lngError = mv_BDSDelivery.Message(v_strObjMsg)
                    '    If v_lngError <> ERR_SYSTEM_OK Then
                    '        MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                    '        Return False
                    '    End If

                    '    v_xmlObjDocument.LoadXml(v_strObjMsg)
                    '    v_nodeList = v_xmlObjDocument.SelectNodes("/ObjectMessage/ObjData")
                    '    v_strKeyName = ""
                    '    v_strSEARCHSQL = ""
                    '    v_strFIELDCODE = ""
                    '    If v_nodeList.Count > 0 Then
                    '        For i = 0 To v_nodeList.Count - 1
                    '            For j = 0 To v_nodeList.Item(i).ChildNodes.Count - 1 Step 1
                    '                With v_nodeList.Item(i).ChildNodes(j)
                    '                    v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                    '                    v_strValue = Trim(.InnerText)
                    '                    Select Case v_strFLDNAME
                    '                        Case "KEYNAME"
                    '                            v_strKeyName = Trim(v_strValue)
                    '                        Case "SEARCHCMDSQL"
                    '                            v_strSEARCHSQL = Trim(v_strValue)
                    '                        Case "FIELDCODE"
                    '                            v_strFIELDCODE = Trim(v_strValue)
                    '                    End Select
                    '                End With
                    '            Next
                    '        Next
                    '        v_strSEARCHSQL = v_strSEARCHSQL.Replace("?LANGUAGE", Me.UserLanguage)
                    '        v_strSEARCHSQL = v_strSEARCHSQL.Replace("?MEMBERFILTER", IIf(mv_lngAllMember = 0, " NVL(a.micode,'000') in " & mv_strMemberFilter, " 1=1 "))
                    '        v_strSEARCHSQL = v_strSEARCHSQL.Replace("?COMEMBERFILTER", IIf(mv_lngAllMember = 0, " NVL(a.comicode,'000') in " & mv_strMemberFilter, " 1=1 "))
                    '        v_strSEARCHSQL = v_strSEARCHSQL.Replace("?STOCKFILTER", IIf(mv_lngAllStock = 0, " NVL(a.sicode,'000') in " & mv_strStockFilter, " 1=1 "))
                    '        v_strSEARCHSQL = v_strSEARCHSQL.Replace("?BRID", mv_strBranchId)
                    '        v_strSEARCHSQL = v_strSEARCHSQL.Replace("?TLID", mv_strTellerId)

                    '        v_intCount = mv_arrObjFields.GetLength(0) - 1
                    '        For v_intSharp = 0 To v_intCount - 1 Step 1
                    '            v_strSharp = ""
                    '            If mv_arrObjFields(v_intSharp).Visible = True Then
                    '                Select Case Trim(mv_arrObjFields(v_intSharp).ControlType)
                    '                    Case "T"
                    '                        v_strSharp = CType(Me.pnTransDetail.Controls(mv_arrObjFields(v_intSharp).ControlIndex), TextBox).Text
                    '                    Case "M"
                    '                        v_strSharp = CType(Me.pnTransDetail.Controls(mv_arrObjFields(v_intSharp).ControlIndex), FlexMaskEditBox).Text
                    '                        v_strSharp = IIf(Trim(mv_arrObjFields(v_intSharp).DataType) = "D", v_strSharp.Replace("/  /", ""), v_strSharp)
                    '                    Case "C"
                    '                        v_strSharp = CType(Me.pnTransDetail.Controls(mv_arrObjFields(v_intSharp).ControlIndex), ComboBoxEx).SelectedValue.ToString
                    '                End Select
                    '                If Trim(v_strSharp) = "" And InStr(v_strSEARCHSQL, "#" & Format(v_intSharp + 1, "00"), CompareMethod.Text) > 0 Then
                    '                    v_strMsgErr = "Bạn chưa nhập " & mv_arrObjFields(v_intSharp).Caption
                    '                    Me.pnTransDetail.Controls(mv_arrObjFields(intIndex).LabelIndex).Visible = True
                    '                    Me.pnTransDetail.Controls(mv_arrObjFields(intIndex).LabelIndex).Text = v_strMsgErr
                    '                    Me.pnTransDetail.Controls(mv_arrObjFields(intIndex).LabelIndex).ForeColor = System.Drawing.Color.Red
                    '                    Exit Function
                    '                End If
                    '                v_strSEARCHSQL = v_strSEARCHSQL.Replace("#" & Format(v_intSharp + 1, "00"), v_strSharp.Trim)
                    '            End If
                    '        Next
                    '        'Kiá»ƒm tra thÃ´ng tin cá»§a nháº­p vÃ o cÃ³ Ä‘Ãºng khÃ´ng?
                    '        v_strSQLCMD = "SELECT * FROM (" & v_strSEARCHSQL & ")"
                    '        If Not (InStr(v_strSEARCHSQL, "#")) > 0 Then
                    '            'v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsNotLocalMsg, gc_MsgTypeObj, OBJNAME_SA_LOOKUP, gc_ActionInquiry, v_strSQLCMD, "")
                    '            v_strObjMsg = BuildXMLObjMsg(Me.BusDate, Me.BranchId, , Me.TellerId, gc_IsNotLocalMsg, gc_MsgTypeObj, "TR." & v_strSEARCHCODE, _
                    '                                        gc_ActionInquiry, v_strSQLCMD, , , True, StockFilter & "|" & MemberFilter)

                    '            v_lngError = mv_BDSDelivery.Message(v_strObjMsg)
                    '            If v_lngError <> ERR_SYSTEM_OK Then
                    '                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                    '                Return False
                    '            End If

                    '            'LÆ°u trá»¯ danh sÃ¡ch tÃ¬m kiáº¿m tráº£ vá»?
                    '            v_strFULLDATA = v_strObjMsg
                    '            v_xmlObjDocument.LoadXml(v_strObjMsg)
                    '            v_nodeList = v_xmlObjDocument.SelectNodes("/ObjectMessage/ObjData")
                    '            'Kiá»ƒm tra xem giÃ¡ trá»‹ co há»£p lá»‡ khÃ´ng
                    '            For i = 0 To v_nodeList.Count - 1
                    '                For j = 0 To v_nodeList.Item(i).ChildNodes.Count - 1 Step 1
                    '                    With v_nodeList.Item(i).ChildNodes(j)
                    '                        Select Case CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                    '                            Case v_strKeyName
                    '                                arrCoFields(intIndex) = arrCoFields(intIndex) & Chr(1) & Trim(.InnerText.ToString)
                    '                                arrDisplayCoFields(intIndex) = arrDisplayCoFields(intIndex) & Chr(1) & Trim(.InnerText.ToString)
                    '                                If v_strKeyName = v_strFIELDCODE Then
                    '                                    arrDisplayCoFields(intIndex) = arrDisplayCoFields(intIndex) & Chr(1) & Trim(.InnerText.ToString)
                    '                                End If
                    '                            Case v_strFIELDCODE
                    '                                arrDisplayCoFields(intIndex) = arrDisplayCoFields(intIndex) & Chr(1) & Trim(.InnerText.ToString)
                    '                        End Select
                    '                    End With
                    '                Next j
                    '            Next i
                    '        Else
                    '            arrDisplayCoFields(intIndex) = "#"
                    '        End If
                    '    End If
                End If
                arrCoFields(intIndex) = arrCoFields(intIndex) & Chr(1)
                arrDisplayCoFields(intIndex) = arrDisplayCoFields(intIndex) & Chr(1)
            Next
            Dim strDisplayTmp, strValueTmp As String

            strConDuplicated = ""
            strRows = ""

            v_dataElement = v_xmlDocument.CreateElement(Xml.XmlNodeType.Element, "fields", "")

            'For i = 1 To arrValue.GetLength(0)
            Dim v_dt As DataTable
            'bangpv: 
            Dim strErrMsgTmp
            'end bangpv
            v_dt = GetDataTableFromFile(strFileName, strType)
            mv_intErrCount = 0

            For i = 0 To v_dt.Rows.Count - 1
                'bangpv: Bỏ đoạn này 
                'strErrMsg = strErrMsg & "---oOo---" & vbCrLf & "Dòng " & i + intRealRow & " : " & vbCrLf
                'end bangpv
                strRowDuplicated = ""
                strInsertFields = ""
                strInsertValues = ""
                strErrMsgTmp = ""
                'For j = 1 To arrValue.GetLength(1)
                For j = 0 To v_dt.Columns.Count - 1
                    strDisplayTmp = ""
                    strValueTmp = ""
                    'If mv_arrObjCoFields(j - 1).DataType = "N" Then
                    '    If mv_arrObjCoFields(j - 1).Mandatory And arrValue(i, j) Is Nothing Then
                    '        strErrMsg = strErrMsg & "Cột """ & mv_arrObjCoFields(j - 1).Caption.Replace(":", "") & """" & " của dòng " & i + intRealRow & " : " & mv_ResourceManager.GetString("ERR_NULL_VALUE") & vbCrLf
                    '    End If
                    '    If Not arrValue(i, j) Is Nothing Then
                    '        If arrValue(i, j).GetType.ToString = "System.Double" Then
                    '            If arrCoFields(j - 1) <> Chr(1) Then
                    '                If InStr(arrCoFields(j - 1), Chr(1) & Trim(arrValue(i, j) & Chr(1))) = 0 Then
                    '                    strErrMsg = strErrMsg & "Cột """ & mv_arrObjCoFields(j - 1).Caption.Replace(":", "") & """" & " của dòng " & i + intRealRow & " : " & mv_ResourceManager.GetString("ERR_LOOKUP_VALUE") & vbCrLf
                    '                End If
                    '            End If
                    '            If mv_arrObjCoFields(j - 1).FieldName = strChildMsgAmt Then
                    '                If dblTranLimit <> 0 And dblTranLimit < arrValue(i, j) Then
                    '                    strErrMsg = strErrMsg & "Cột """ & mv_arrObjCoFields(j - 1).Caption.Replace(":", "") & """" & " của dòng " & i + intRealRow & " : " & mv_ResourceManager.GetString("ERR_LIMIT_VALUE") & dblTranLimit & vbCrLf
                    '                End If

                    '            End If

                    '            If mv_arrObjCoFields(j - 1).SearchCode.Length > 0 Or mv_arrObjCoFields(j - 1).LookupList.Length > 0 Then
                    '                Dim strTmpDisplay As String
                    '                Dim itmp, jtmp As Integer
                    '                strTmpDisplay = Chr(1) & Trim(arrValue(i, j)) & Chr(1)
                    '                If InStr(1, arrDisplayCoFields(j - 1), strTmpDisplay) > 0 Then
                    '                    itmp = InStr(1, arrDisplayCoFields(j - 1), strTmpDisplay) + Len(strTmpDisplay)
                    '                    jtmp = InStr(itmp, arrDisplayCoFields(j - 1), Chr(1))
                    '                    strDisplayTmp = Mid(arrDisplayCoFields(j - 1), itmp, jtmp - itmp)
                    '                End If
                    '            Else
                    '                strDisplayTmp = Trim(arrValue(i, j))
                    '            End If
                    '            strValueTmp = arrValue(i, j).ToString
                    '        Else
                    '            strErrMsg = strErrMsg & "Cột """ & mv_arrObjCoFields(j - 1).Caption.Replace(":", "") & """" & " của dòng " & i + intRealRow & " : " & mv_ResourceManager.GetString("ERR_NUMERIC_VALUE") & vbCrLf
                    '        End If
                    '    End If
                    'ElseIf mv_arrObjCoFields(j - 1).DataType = "C" Then
                    ' bangpv: replace : strerrmsg --> strerrmsgtmp
                    If mv_arrObjCoFields(j).Mandatory And v_dt.Rows(i)(j) Is Nothing Then
                        strErrMsgTmp = strErrMsgTmp & "Cột """ & mv_arrObjCoFields(j).Caption.Replace(":", "") & """" & " của dòng " & i + intRealRow & " : " & mv_ResourceManager.GetString("ERR_NULL_VALUE") & vbCrLf
                        mv_intErrCount = mv_intErrCount + 1
                    End If
                    If Not v_dt.Rows(i)(j) Is Nothing Then
                        'If arrValue(i, j).GetType.ToString = "System.String" Then
                        If arrCoFields(j) <> Chr(1) Then
                            If InStr(arrCoFields(j), Chr(1) & Trim(v_dt.Rows(i)(j) & Chr(1))) = 0 Then
                                strErrMsgTmp = strErrMsgTmp & "Cột """ & mv_arrObjCoFields(j).Caption.Replace(":", "") & """" & " của dòng " & i + intRealRow & " : " & mv_ResourceManager.GetString("ERR_LOOKUP_VALUE") & vbCrLf
                                mv_intErrCount = mv_intErrCount + 1
                            End If
                        End If

                        If (mv_arrObjCoFields(j).SearchCode.Length > 0 And arrDisplayCoFields(j) <> "#" & Chr(1)) _
                                Or mv_arrObjCoFields(j).LookupList.Length > 0 Then
                            Dim strTmpDisplay As String
                            Dim itmp, jtmp As Integer
                            strTmpDisplay = Chr(1) & Trim(v_dt.Rows(i)(j)) & Chr(1)
                            If InStr(1, arrDisplayCoFields(j), strTmpDisplay) > 0 Then
                                itmp = InStr(1, arrDisplayCoFields(j), strTmpDisplay) + Len(strTmpDisplay)
                                jtmp = InStr(itmp, arrDisplayCoFields(j), Chr(1))
                                strDisplayTmp = Mid(arrDisplayCoFields(j), itmp, jtmp - itmp)
                            End If
                        Else
                            strDisplayTmp = Trim(v_dt.Rows(i)(j))
                        End If

                        strValueTmp = v_dt.Rows(i)(j).ToString.Replace("'", "''")
                        strDisplayTmp = Replace(strDisplayTmp, "'", "''")
                        'Else
                        '   strErrMsg = strErrMsg & "Cột """ & mv_arrObjCoFields(j - 1).Caption.Replace(":", "") & """" & " của dòng " & i + intRealRow & " : " & mv_ResourceManager.GetString("ERR_NUMERIC_VALUE") & vbCrLf
                        'End If
                    End If
                    'ElseIf mv_arrObjCoFields(j - 1).DataType = "D" Then
                    '    If mv_arrObjCoFields(j - 1).Mandatory And arrValue(i, j) Is Nothing Then
                    '        strErrMsg = strErrMsg & "Cột """ & mv_arrObjCoFields(j - 1).Caption.Replace(":", "") & """" & " của dòng " & i + intRealRow & " : " & mv_ResourceManager.GetString("ERR_NULL_VALUE") & vbCrLf
                    '    End If
                    '    If Not arrValue(i, j) Is Nothing Then
                    '        If arrValue(i, j).GetType.ToString = "System.DateTime" Then
                    '            If arrCoFields(j - 1) <> Chr(1) Then
                    '                If InStr(arrCoFields(j - 1), Chr(1) & Trim(arrValue(i, j) & Chr(1))) = 0 Then
                    '                    strErrMsg = strErrMsg & "Cột """ & mv_arrObjCoFields(j - 1).Caption.Replace(":", "") & """" & " của dòng " & i + intRealRow & " : " & mv_ResourceManager.GetString("ERR_LOOKUP_VALUE") & vbCrLf
                    '                End If
                    '            End If

                    '            If mv_arrObjCoFields(j - 1).SearchCode.Length > 0 Or mv_arrObjCoFields(j - 1).LookupList.Length > 0 Then
                    '                Dim strTmpDisplay As String
                    '                Dim itmp, jtmp As Integer
                    '                strTmpDisplay = Chr(1) & Trim(arrValue(i, j)) & Chr(1)
                    '                If InStr(1, arrDisplayCoFields(j - 1), strTmpDisplay) > 0 Then
                    '                    itmp = InStr(1, arrDisplayCoFields(j - 1), strTmpDisplay) + Len(strTmpDisplay)
                    '                    jtmp = InStr(itmp, arrDisplayCoFields(j - 1), Chr(1))
                    '                    strDisplayTmp = Mid(arrDisplayCoFields(j - 1), itmp, jtmp - itmp)
                    '                End If
                    '            Else
                    '                strDisplayTmp = Trim(arrValue(i, j))
                    '            End If

                    '            strValueTmp = CDate(arrValue(i, j)).ToShortDateString
                    '        Else
                    '            strErrMsg = strErrMsg & "Cột """ & mv_arrObjCoFields(j - 1).Caption.Replace(":", "") & """" & " của dòng " & i + intRealRow & " : " & mv_ResourceManager.GetString("ERR_NUMERIC_VALUE") & vbCrLf
                    '        End If
                    '    End If
                    'End If
                    If mv_arrObjCoFields(j).IsDuplicated = "Y" Then
                        strRowDuplicated = strRowDuplicated & v_dt.Rows(i)(j) 'arrValue(i, j)
                    End If
                    strInsertFields = strInsertFields & ",COL_VALUE" & mv_arrObjCoFields(j).FieldName _
                                        & ", COL_TYPE" & mv_arrObjCoFields(j).FieldName _
                                        & ", COL_DESC" & mv_arrObjCoFields(j).FieldName
                    strInsertValues = strInsertValues & ",'" & strValueTmp & "'" & " COL_VALUE" & mv_arrObjCoFields(j).FieldName & ",'" _
                                        & mv_arrObjCoFields(j).DataType & "'" & " COL_TYPE" & mv_arrObjCoFields(j).FieldName & ",'" _
                                        & strDisplayTmp & "'" & " COL_DESC" & mv_arrObjCoFields(j).FieldName
                Next
                If strRowDuplicated <> "" Then
                    strConDuplicated = strConDuplicated & Chr(1) & i + intRealRow & Chr(1) & strRowDuplicated & Chr(1)
                End If
                'BằngPV: Bỏ
                'strErrMsg = strErrMsg & "@" & i + intRealRow & vbCrLf & "---oOo---" & vbCrLf
                'End BằngPV
                strRows = strRows & "UNION SELECT '" & mv_strCOTLTXCD & "' TLTXCD " & v_strValuesSQL & " " & strInsertValues & ", " & i + intRealRow & " as REAL_ROW FROM DUAL " & vbCrLf
                If (i + 1) Mod gc_IMP_TRAN_UNIT = 0 Then
                    'Append entry to data node
                    v_entryNode = v_xmlDocument.CreateNode(Xml.XmlNodeType.Element, "entry", "")
                    'Add field name
                    v_attrFLDNAME = v_xmlDocument.CreateAttribute(gc_AtributeFLDNAME)
                    v_attrFLDNAME.Value = "INSERT_ROW"
                    v_entryNode.Attributes.Append(v_attrFLDNAME)
                    'Add field type
                    v_attrDATATYPE = v_xmlDocument.CreateAttribute(gc_AtributeFLDTYPE)
                    v_attrDATATYPE.Value = "C"
                    v_entryNode.Attributes.Append(v_attrDATATYPE)
                    'Set value
                    strRows = "INSERT INTO TMP_TXFIELDS(AUTOID, TLTXCD " & v_strFieldsSQL & " " & strInsertFields & ", REAL_ROW ) " _
                        & " SELECT seq_tmp_txfields.nextval , a.* from (" & strRows.Substring(6) & ") a "

                    v_entryNode.InnerText = strRows
                    v_dataElement.AppendChild(v_entryNode)
                    strRows = ""
                End If
            Next
            If strRows <> "" Then
                v_entryNode = v_xmlDocument.CreateNode(Xml.XmlNodeType.Element, "entry", "")
                'Add field name
                v_attrFLDNAME = v_xmlDocument.CreateAttribute(gc_AtributeFLDNAME)
                v_attrFLDNAME.Value = "INSERT_ROW"
                v_entryNode.Attributes.Append(v_attrFLDNAME)
                'Add field type
                v_attrDATATYPE = v_xmlDocument.CreateAttribute(gc_AtributeFLDTYPE)
                v_attrDATATYPE.Value = "C"
                v_entryNode.Attributes.Append(v_attrDATATYPE)
                'Set value
                strRows = "INSERT INTO TMP_TXFIELDS(AUTOID, TLTXCD " & v_strFieldsSQL & " " & strInsertFields & ", REAL_ROW ) " _
                    & " SELECT seq_tmp_txfields.nextval , a.* from (" & strRows.Substring(6) & ") a "

                v_entryNode.InnerText = strRows
                v_dataElement.AppendChild(v_entryNode)

                'v_xmlDocument.DocumentElement.AppendChild(v_dataElement)
            End If
            If v_dt.Rows.Count > 0 Then
                v_xmlDocument.DocumentElement.AppendChild(v_dataElement)
            End If
            ' lay tieu chi filter tu mv_arrObjCoFldvals
            'CheckFldvals(mv_arrObjCoFldVals, arrValue, strErrMsg, intRealRow)

            ' liet ke dong trung nhau 
            If strConDuplicated <> "" Then
                Dim intPos, intSplit, intTimes As Integer
                Dim strCheck, strMsg, strChecMsg As String

                'For i = 1 To arrValue.GetLength(0)
                For i = 0 To v_dt.Rows.Count - 1
                    strCheck = ""
                    strChecMsg = ""
                    'For j = 1 To arrValue.GetLength(1)
                    For j = 0 To v_dt.Columns.Count - 1
                        If mv_arrObjCoFields(j).IsDuplicated = "Y" Then
                            strCheck = strCheck & v_dt.Rows(i)(j)
                            strChecMsg = strChecMsg & ";" & mv_arrObjCoFields(j).Caption & " " & v_dt.Rows(i)(j)
                        End If
                    Next
                    strCheck = Chr(1) & strCheck & Chr(1)
                    intPos = 1
                    strMsg = ""
                    intTimes = 0
                    While True
                        intPos = InStr(intPos, strConDuplicated, strCheck)
                        If intPos <> 0 Then
                            intSplit = InStrRev(strConDuplicated, Chr(1), intPos - 2)
                            strMsg = strMsg & "," & strConDuplicated.Substring(intSplit, intPos - intSplit - 1)
                            intTimes = intTimes + 1
                        Else
                            Exit While
                        End If
                        intPos = intPos + 1
                    End While
                    If strMsg <> "" And intTimes >= 2 Then
                        strErrMsgTmp = strErrMsgTmp & "Các dòng (" & strMsg.Substring(1) & ") có giá trị trùng nhau ( " & strChecMsg.Substring(1) & ")" & vbCrLf
                        mv_intErrCount = mv_intErrCount + 1
                    End If
                    strConDuplicated = strConDuplicated.Replace(strCheck, "")
                    If strErrMsgTmp <> "" Then
                        strErrMsg = strErrMsg & strErrMsgTmp
                    End If
                Next
            End If

            'For i = 1 To arrValue.GetLength(0)
            'BằngPV
            'For i = 0 To v_dt.Rows.Count - 1
            '    strErrMsg = strErrMsg.Replace("---oOo---" & vbCrLf & "Dòng " & i + intRealRow & " : " & vbCrLf & "@" & i + intRealRow & vbCrLf & "---oOo---" & vbCrLf, "")
            '    strErrMsg = strErrMsg.Replace("@" & i + intRealRow & vbCrLf, "")
            'Next
            'end bangpv
            If strErrMsg <> "" Then
                v_strErrMsg = strErrMsg
                ' thong bao loi
                Return False
            Else
                v_xmlDocument.DocumentElement.Attributes(gc_AtributeMSGAMT).InnerXml = strChildMsgAmt
                Return True
            End If

        Catch ex As Exception
            Throw ex
        Finally
            If Not v_obj Is Nothing Then
                v_obj.CloseConnection()
            End If
            v_obj = Nothing
            If Not v_ds Is Nothing Then
                v_ds.Dispose()
            End If
            GC.Collect()
            GC.WaitForPendingFinalizers()
            GC.Collect()
            GC.WaitForPendingFinalizers()
        End Try
    End Function

    Protected Function CheckFldvals(ByVal arrObjFldVals() As CFieldVal, Optional ByRef arrValue As Array = Nothing, Optional ByRef strErrMsg As String = Nothing, Optional ByVal intRealRow As Integer = -1) As Boolean
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
                                            Application.DoEvents()
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
                                                        Application.DoEvents()
                                                        MsgBox(.EN_ERRMSG)

                                                    Else
                                                        v_oProcess.StopProcessForm()
                                                        Application.DoEvents()
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
                                                        Application.DoEvents()
                                                        MsgBox(.EN_ERRMSG)
                                                    Else
                                                        v_oProcess.StopProcessForm()
                                                        Application.DoEvents()
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
                                                        Application.DoEvents()
                                                        MsgBox(.EN_ERRMSG)
                                                    Else
                                                        v_oProcess.StopProcessForm()
                                                        Application.DoEvents()
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
                                                        Application.DoEvents()
                                                        MsgBox(.EN_ERRMSG)
                                                    Else
                                                        v_oProcess.StopProcessForm()
                                                        Application.DoEvents()
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
                                                        Application.DoEvents()
                                                        MsgBox(.EN_ERRMSG)
                                                    Else
                                                        v_oProcess.StopProcessForm()
                                                        Application.DoEvents()
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
                                                        Application.DoEvents()
                                                        MsgBox(.EN_ERRMSG)
                                                    Else
                                                        v_oProcess.StopProcessForm()
                                                        Application.DoEvents()
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
                                            Application.DoEvents()
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
                                                        Application.DoEvents()
                                                        MsgBox(.EN_ERRMSG)
                                                    Else
                                                        v_oProcess.StopProcessForm()
                                                        Application.DoEvents()
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
                                                        Application.DoEvents()
                                                        MsgBox(.EN_ERRMSG)
                                                    Else
                                                        v_oProcess.StopProcessForm()
                                                        Application.DoEvents()
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
                                                        Application.DoEvents()
                                                        MsgBox(.EN_ERRMSG)
                                                    Else
                                                        v_oProcess.StopProcessForm()
                                                        Application.DoEvents()
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
                                                        Application.DoEvents()
                                                        MsgBox(.EN_ERRMSG)
                                                    Else
                                                        v_oProcess.StopProcessForm()
                                                        Application.DoEvents()
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
                                                        Application.DoEvents()
                                                        MsgBox(.EN_ERRMSG)
                                                    Else
                                                        v_oProcess.StopProcessForm()
                                                        Application.DoEvents()
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
                                                        Application.DoEvents()
                                                        MsgBox(.EN_ERRMSG)
                                                    Else
                                                        v_oProcess.StopProcessForm()
                                                        Application.DoEvents()
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
                                        v_strFLDVALUE = Trim(gf_CorrectStringField(arrValue(intVIndext, v_intIndex)))
                                        If mv_arrObjCoFields(.IDXFLD).DataType <> "D" Then
                                            If Not gf_Numberic(v_strFLDVALUE) Then
                                                strTmp = strTmp & "Cột """ & mv_arrObjCoFields(.IDXFLD).Caption.Replace(":", "") & """" & " của dòng " & intVIndext + intRealRow & " : " & mv_ResourceManager.GetString("ERR_INVALID_NUMERIC_NUMBER") & vbCrLf
                                            Else
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
                                            End If
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

    'Protected Function CheckFldvals(ByVal pv_intControlIndex As Integer) As Boolean
    '    Dim v_intCount, v_intIndex As Integer
    '    Dim v_strVALEXP, v_strVALEXP2, v_strFLDVALUE As String
    '    Dim v_dtFLDVALUE, v_dtVALEXP, v_dtVALEXP2 As Date
    '    Dim v_objEval As New Evaluator
    '    Dim strTmp As String = ""
    '    Dim v_oFldVal As CFieldVal
    '    Try
    '        v_intCount = mv_arrObjFldVals.Count - 1
    '        'xac dinh cong thuc check dl
    '        For i = 0 To v_intCount
    '            If mv_arrObjFldVals(i).IDXFLD = pv_intControlIndex Then
    '                v_oFldVal = mv_arrObjFldVals(i)
    '                Exit For
    '            End If
    '        Next

    '        If Not v_oFldVal Is Nothing Then
    '            With v_oFldVal
    '                'Xác định index
    '                v_intIndex = mv_arrObjFields(.IDXFLD).ControlIndex
    '                'Thực hiện xác lập cho từng phép toán
    '                If .VALTYPE = "E" Then
    '                    'Nếu trường dữ liệu có kiểu là số
    '                    If mv_arrObjFields(.IDXFLD).DataType <> "D" Then
    '                        Select Case .mp_OPERATOR
    '                            Case "MA"
    '                                v_strVALEXP = BuildAMTEXP(.VALEXP)
    '                                v_strVALEXP2 = BuildAMTEXP(.VALEXP2)
    '                                Me.pnTransDetail.Controls(v_intIndex).Text = GetMax(v_objEval.Eval(v_strVALEXP), v_objEval.Eval(v_strVALEXP2)).ToString
    '                            Case "MI"
    '                                v_strVALEXP = BuildAMTEXP(.VALEXP)
    '                                v_strVALEXP2 = BuildAMTEXP(.VALEXP2)
    '                                Me.pnTransDetail.Controls(v_intIndex).Text = GetMin(v_objEval.Eval(v_strVALEXP), v_objEval.Eval(v_strVALEXP2)).ToString
    '                            Case "&&"
    '                                v_strVALEXP = BuildCONCAT(.VALEXP)
    '                                Me.pnTransDetail.Controls(v_intIndex).Text = v_strVALEXP
    '                            Case "EX"
    '                                v_strVALEXP = BuildAMTEXP(.VALEXP)
    '                                If mv_arrObjFields(.IDXFLD).DataType = "N" Then
    '                                    Me.pnTransDetail.Controls(v_intIndex).Text = Math.Round(v_objEval.Eval(v_strVALEXP), 0)
    '                                Else
    '                                    Me.pnTransDetail.Controls(v_intIndex).Text = v_objEval.Eval(v_strVALEXP)
    '                                End If
    '                        End Select
    '                        'Nếu trường dữ liệu có kiểu là ngày tháng
    '                    ElseIf mv_arrObjFields(.IDXFLD).DataType = "D" Then
    '                        Select Case .mp_OPERATOR
    '                            Case "MA"
    '                                v_dtVALEXP = DDMMYYYY_SystemDate(BuildAMTEXP(.VALEXP))
    '                                v_dtVALEXP2 = DDMMYYYY_SystemDate(BuildAMTEXP(.VALEXP2))
    '                                If v_dtVALEXP > v_dtVALEXP2 Then
    '                                    Me.pnTransDetail.Controls(v_intIndex).Text = v_dtVALEXP
    '                                Else
    '                                    Me.pnTransDetail.Controls(v_intIndex).Text = v_dtVALEXP2
    '                                End If
    '                            Case "MI"
    '                                v_dtVALEXP = DDMMYYYY_SystemDate(BuildAMTEXP(.VALEXP))
    '                                v_dtVALEXP2 = DDMMYYYY_SystemDate(BuildAMTEXP(.VALEXP2))
    '                                If v_dtVALEXP > v_dtVALEXP2 Then
    '                                    Me.pnTransDetail.Controls(v_intIndex).Text = v_dtVALEXP2
    '                                Else
    '                                    Me.pnTransDetail.Controls(v_intIndex).Text = v_dtVALEXP
    '                                End If
    '                        End Select
    '                    End If

    '                ElseIf .VALTYPE = "V" Then
    '                    If TypeOf (Me.pnTransDetail.Controls(v_intIndex)) Is ComboBoxEx Then
    '                        v_strFLDVALUE = CType(Me.pnTransDetail.Controls(v_intIndex), ComboBoxEx).SelectedValue
    '                    Else
    '                        v_strFLDVALUE = Me.pnTransDetail.Controls(v_intIndex).Text
    '                    End If

    '                    If mv_arrObjFields(.IDXFLD).DataType <> "D" Then
    '                        If Not gf_Numberic(v_strFLDVALUE) Then
    '                            v_oProcess.StopProcessForm()
    '                            MessageBox.Show(mv_ResourceManager.GetString("ERR_INVALID_NUMERIC_NUMBER"))
    '                            mv_bolCheck = False
    '                            Return False
    '                        End If
    '                        v_strFLDVALUE = CDbl(v_strFLDVALUE).ToString
    '                        Select Case .mp_OPERATOR
    '                            Case ">>"
    '                                v_strVALEXP = BuildAMTEXP(.VALEXP)
    '                                If Not CDbl(v_strFLDVALUE) > v_objEval.Eval(v_strVALEXP) Then
    '                                    If Me.UserLanguage = "EN" Then
    '                                        v_oProcess.StopProcessForm()
    '                                        MsgBox(.EN_ERRMSG)

    '                                    Else
    '                                        v_oProcess.StopProcessForm()
    '                                        MsgBox(.ERRMSG)

    '                                    End If
    '                                    mv_bolCheck = False
    '                                    Return False
    '                                End If
    '                            Case ">="
    '                                v_strVALEXP = BuildAMTEXP(.VALEXP)
    '                                If Not CDbl(v_strFLDVALUE) >= v_objEval.Eval(v_strVALEXP) Then
    '                                    If Me.UserLanguage = "EN" Then
    '                                        v_oProcess.StopProcessForm()
    '                                        MsgBox(.EN_ERRMSG)
    '                                    Else
    '                                        v_oProcess.StopProcessForm()
    '                                        MsgBox(.ERRMSG)
    '                                    End If
    '                                    mv_bolCheck = False
    '                                    Return False
    '                                End If
    '                            Case "<<"
    '                                v_strVALEXP = BuildAMTEXP(.VALEXP)
    '                                If Not CDbl(v_strFLDVALUE) < v_objEval.Eval(v_strVALEXP) Then
    '                                    If Me.UserLanguage = "EN" Then
    '                                        v_oProcess.StopProcessForm()
    '                                        MsgBox(.EN_ERRMSG)
    '                                    Else
    '                                        v_oProcess.StopProcessForm()
    '                                        MsgBox(.ERRMSG)
    '                                    End If
    '                                    mv_bolCheck = False
    '                                    Return False
    '                                End If
    '                            Case "<="
    '                                v_strVALEXP = BuildAMTEXP(.VALEXP)
    '                                If Not CDbl(v_strFLDVALUE) <= v_objEval.Eval(v_strVALEXP) Then
    '                                    If Me.UserLanguage = "EN" Then
    '                                        v_oProcess.StopProcessForm()
    '                                        MsgBox(.EN_ERRMSG)
    '                                    Else
    '                                        v_oProcess.StopProcessForm()
    '                                        MsgBox(.ERRMSG)
    '                                    End If
    '                                    mv_bolCheck = False
    '                                    Return False
    '                                End If
    '                            Case "=="
    '                                v_strVALEXP = BuildAMTEXP(.VALEXP)
    '                                If Not CDbl(v_strFLDVALUE) = v_objEval.Eval(v_strVALEXP) Then
    '                                    If Me.UserLanguage = "EN" Then
    '                                        v_oProcess.StopProcessForm()
    '                                        MsgBox(.EN_ERRMSG)
    '                                    Else
    '                                        v_oProcess.StopProcessForm()
    '                                        MsgBox(.ERRMSG)
    '                                    End If
    '                                    mv_bolCheck = False
    '                                    Return False
    '                                End If
    '                            Case "<>"
    '                                v_strVALEXP = BuildAMTEXP(.VALEXP)
    '                                If Not CDbl(v_strFLDVALUE) <> v_objEval.Eval(v_strVALEXP) Then
    '                                    If Me.UserLanguage = "EN" Then
    '                                        v_oProcess.StopProcessForm()
    '                                        MsgBox(.EN_ERRMSG)
    '                                    Else
    '                                        v_oProcess.StopProcessForm()
    '                                        MsgBox(.ERRMSG)
    '                                    End If
    '                                    mv_bolCheck = False
    '                                    Return False
    '                                End If
    '                            Case "IN"
    '                            Case "NI"
    '                        End Select
    '                    ElseIf mv_arrObjFields(.IDXFLD).DataType = "D" Then
    '                        If Not IsDateValue(v_strFLDVALUE) Then
    '                            v_oProcess.StopProcessForm()
    '                            MessageBox.Show(mv_ResourceManager.GetString("ERR_DATE_VALUE"), gc_ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Information)
    '                            mv_bolCheck = False
    '                            Return False
    '                        Else
    '                            v_dtFLDVALUE = DDMMYYYY_SystemDate(v_strFLDVALUE)
    '                        End If
    '                        Select Case .mp_OPERATOR
    '                            Case ">>"
    '                                v_dtVALEXP = DDMMYYYY_SystemDate(BuildAMTEXP(.VALEXP))
    '                                If Not v_dtFLDVALUE > v_dtVALEXP Then
    '                                    If Me.UserLanguage = "EN" Then
    '                                        v_oProcess.StopProcessForm()
    '                                        MsgBox(.EN_ERRMSG)
    '                                    Else
    '                                        v_oProcess.StopProcessForm()
    '                                        MsgBox(.ERRMSG)
    '                                    End If
    '                                    mv_bolCheck = False
    '                                    Return False
    '                                End If
    '                            Case ">="
    '                                v_dtVALEXP = DDMMYYYY_SystemDate(BuildAMTEXP(.VALEXP))
    '                                If v_dtFLDVALUE >= v_dtVALEXP Then
    '                                    If Me.UserLanguage = "EN" Then
    '                                        v_oProcess.StopProcessForm()
    '                                        MsgBox(.EN_ERRMSG)
    '                                    Else
    '                                        v_oProcess.StopProcessForm()
    '                                        MsgBox(.ERRMSG)
    '                                    End If
    '                                    mv_bolCheck = False
    '                                    Return False
    '                                End If
    '                            Case "<<"
    '                                v_dtVALEXP = DDMMYYYY_SystemDate(BuildAMTEXP(.VALEXP))
    '                                If Not v_dtFLDVALUE < v_dtVALEXP Then
    '                                    If Me.UserLanguage = "EN" Then
    '                                        v_oProcess.StopProcessForm()
    '                                        MsgBox(.EN_ERRMSG)
    '                                    Else
    '                                        v_oProcess.StopProcessForm()
    '                                        MsgBox(.ERRMSG)
    '                                    End If
    '                                    mv_bolCheck = False
    '                                    Return False
    '                                End If
    '                            Case "<="
    '                                v_dtVALEXP = DDMMYYYY_SystemDate(BuildAMTEXP(.VALEXP))
    '                                If Not v_dtFLDVALUE <= v_dtVALEXP Then
    '                                    If Me.UserLanguage = "EN" Then
    '                                        v_oProcess.StopProcessForm()
    '                                        MsgBox(.EN_ERRMSG)
    '                                    Else
    '                                        v_oProcess.StopProcessForm()
    '                                        MsgBox(.ERRMSG)
    '                                    End If
    '                                    mv_bolCheck = False
    '                                    Return False
    '                                End If
    '                            Case "=="
    '                                v_dtVALEXP = DDMMYYYY_SystemDate(BuildAMTEXP(.VALEXP))
    '                                If Not v_dtFLDVALUE = v_dtVALEXP Then
    '                                    If Me.UserLanguage = "EN" Then
    '                                        v_oProcess.StopProcessForm()
    '                                        MsgBox(.EN_ERRMSG)
    '                                    Else
    '                                        v_oProcess.StopProcessForm()
    '                                        MsgBox(.ERRMSG)
    '                                    End If
    '                                    mv_bolCheck = False
    '                                    Return False
    '                                End If
    '                            Case "<>"
    '                                v_dtVALEXP = DDMMYYYY_SystemDate(BuildAMTEXP(.VALEXP))
    '                                If Not v_dtFLDVALUE <> v_dtVALEXP Then
    '                                    If Me.UserLanguage = "EN" Then
    '                                        v_oProcess.StopProcessForm()
    '                                        MsgBox(.EN_ERRMSG)
    '                                    Else
    '                                        v_oProcess.StopProcessForm()
    '                                        MsgBox(.ERRMSG)
    '                                    End If
    '                                    mv_bolCheck = False
    '                                    Return False
    '                                End If
    '                            Case "IN"
    '                            Case "NI"
    '                        End Select
    '                    End If
    '                End If

    '                'Thực hiện xác lập cho từng phép toán
    '                If .VALTYPE = "E" Then
    '                    'Nếu trường dữ liệu có kiểu là số
    '                    If mv_arrObjCoFields(.IDXFLD).DataType <> "D" Then
    '                        Select Case .mp_OPERATOR
    '                            Case "MA"
    '                                v_strVALEXP = BuildAMTEXP(.VALEXP, arrValue, intVIndext)
    '                                v_strVALEXP2 = BuildAMTEXP(.VALEXP2, arrValue, intVIndext)
    '                                arrValue(intVIndext, v_intIndex) = GetMax(v_objEval.Eval(v_strVALEXP), v_objEval.Eval(v_strVALEXP2)).ToString
    '                            Case "MI"
    '                                v_strVALEXP = BuildAMTEXP(.VALEXP, arrValue, intVIndext)
    '                                v_strVALEXP2 = BuildAMTEXP(.VALEXP2, arrValue, intVIndext)
    '                                arrValue(intVIndext, v_intIndex) = GetMin(v_objEval.Eval(v_strVALEXP), v_objEval.Eval(v_strVALEXP2)).ToString
    '                            Case "&&"
    '                                v_strVALEXP = BuildCONCAT(.VALEXP, arrValue, intVIndext)
    '                                arrValue(intVIndext, v_intIndex) = v_strVALEXP
    '                            Case "EX"
    '                                v_strVALEXP = BuildAMTEXP(.VALEXP, arrValue, intVIndext)
    '                                If mv_arrObjCoFields(.IDXFLD).DataType = "N" Then
    '                                    arrValue(intVIndext, v_intIndex) = Math.Round(v_objEval.Eval(v_strVALEXP), 0)
    '                                Else
    '                                    arrValue(intVIndext, v_intIndex) = v_objEval.Eval(v_strVALEXP)
    '                                End If
    '                        End Select
    '                        'Nếu trường dữ liệu có kiểu là ngày tháng
    '                    ElseIf mv_arrObjCoFields(.IDXFLD).DataType = "D" Then
    '                        Select Case .mp_OPERATOR
    '                            Case "MA"
    '                                v_dtVALEXP = DDMMYYYY_SystemDate(BuildAMTEXP(.VALEXP, arrValue, intVIndext))
    '                                v_dtVALEXP2 = DDMMYYYY_SystemDate(BuildAMTEXP(.VALEXP2, arrValue, intVIndext))
    '                                If v_dtVALEXP > v_dtVALEXP2 Then
    '                                    arrValue(intVIndext, v_intIndex) = v_dtVALEXP
    '                                Else
    '                                    arrValue(intVIndext, v_intIndex) = v_dtVALEXP2
    '                                End If
    '                            Case "MI"
    '                                v_dtVALEXP = DDMMYYYY_SystemDate(BuildAMTEXP(.VALEXP, arrValue, intVIndext))
    '                                v_dtVALEXP2 = DDMMYYYY_SystemDate(BuildAMTEXP(.VALEXP2, arrValue, intVIndext))
    '                                If v_dtVALEXP > v_dtVALEXP2 Then
    '                                    arrValue(intVIndext, v_intIndex) = v_dtVALEXP2
    '                                Else
    '                                    arrValue(intVIndext, v_intIndex) = v_dtVALEXP
    '                                End If
    '                        End Select
    '                    End If

    '                ElseIf .VALTYPE = "V" Then
    '                    v_strFLDVALUE = arrValue(intVIndext, v_intIndex)
    '                    If mv_arrObjCoFields(.IDXFLD).DataType <> "D" Then
    '                        If Not gf_Numberic(v_strFLDVALUE) Then
    '                            strTmp = strTmp & "Cột """ & mv_arrObjCoFields(.IDXFLD).Caption.Replace(":", "") & """" & " của dòng " & intVIndext + intRealRow & " : " & mv_ResourceManager.GetString("ERR_INVALID_NUMERIC_NUMBER") & vbCrLf
    '                        End If
    '                        v_strFLDVALUE = CDbl(v_strFLDVALUE).ToString
    '                        Select Case .mp_OPERATOR
    '                            Case ">>"
    '                                v_strVALEXP = BuildAMTEXP(.VALEXP, arrValue, intVIndext)
    '                                If Not CDbl(v_strFLDVALUE) > v_objEval.Eval(v_strVALEXP) Then
    '                                    If Me.UserLanguage = "EN" Then
    '                                        strTmp = strTmp & "Cột """ & mv_arrObjCoFields(.IDXFLD).Caption.Replace(":", "") & """" & " của dòng " & intVIndext + intRealRow & " : " & .EN_ERRMSG & vbCrLf
    '                                        mv_intErrCount = mv_intErrCount + 1
    '                                    Else
    '                                        strTmp = strTmp & "Cột """ & mv_arrObjCoFields(.IDXFLD).Caption.Replace(":", "") & """" & " của dòng " & intVIndext + intRealRow & " : " & .ERRMSG & vbCrLf
    '                                        mv_intErrCount = mv_intErrCount + 1
    '                                    End If
    '                                End If
    '                            Case ">="
    '                                v_strVALEXP = BuildAMTEXP(.VALEXP, arrValue, intVIndext)
    '                                If Not CDbl(v_strFLDVALUE) >= v_objEval.Eval(v_strVALEXP) Then
    '                                    If Me.UserLanguage = "EN" Then
    '                                        strTmp = strTmp & "Cột """ & mv_arrObjCoFields(.IDXFLD).Caption.Replace(":", "") & """" & " của dòng " & intVIndext + intRealRow & " : " & .EN_ERRMSG & vbCrLf
    '                                        mv_intErrCount = mv_intErrCount + 1
    '                                    Else
    '                                        strTmp = strTmp & "Cột """ & mv_arrObjCoFields(.IDXFLD).Caption.Replace(":", "") & """" & " của dòng " & intVIndext + intRealRow & " : " & .ERRMSG & vbCrLf
    '                                        mv_intErrCount = mv_intErrCount + 1
    '                                    End If
    '                                End If
    '                            Case "<<"
    '                                v_strVALEXP = BuildAMTEXP(.VALEXP, arrValue, intVIndext)
    '                                If Not CDbl(v_strFLDVALUE) < v_objEval.Eval(v_strVALEXP) Then
    '                                    If Me.UserLanguage = "EN" Then
    '                                        strTmp = strTmp & "Cột """ & mv_arrObjCoFields(.IDXFLD).Caption.Replace(":", "") & """" & " của dòng " & intVIndext + intRealRow & " : " & .EN_ERRMSG & vbCrLf
    '                                        mv_intErrCount = mv_intErrCount + 1
    '                                    Else
    '                                        strTmp = strTmp & "Cột """ & mv_arrObjCoFields(.IDXFLD).Caption.Replace(":", "") & """" & " của dòng " & intVIndext + intRealRow & " : " & .ERRMSG & vbCrLf
    '                                        mv_intErrCount = mv_intErrCount + 1
    '                                    End If
    '                                End If
    '                            Case "<="
    '                                v_strVALEXP = BuildAMTEXP(.VALEXP, arrValue, intVIndext)
    '                                If Not CDbl(v_strFLDVALUE) <= v_objEval.Eval(v_strVALEXP) Then
    '                                    If Me.UserLanguage = "EN" Then
    '                                        strTmp = strTmp & "Cột """ & mv_arrObjCoFields(.IDXFLD).Caption.Replace(":", "") & """" & " của dòng " & intVIndext + intRealRow & " : " & .EN_ERRMSG & vbCrLf
    '                                        mv_intErrCount = mv_intErrCount + 1
    '                                    Else
    '                                        strTmp = strTmp & "Cột """ & mv_arrObjCoFields(.IDXFLD).Caption.Replace(":", "") & """" & " của dòng " & intVIndext + intRealRow & " : " & .ERRMSG & vbCrLf
    '                                        mv_intErrCount = mv_intErrCount + 1
    '                                    End If
    '                                End If
    '                            Case "=="
    '                                v_strVALEXP = BuildAMTEXP(.VALEXP, arrValue, intVIndext)
    '                                If Not CDbl(v_strFLDVALUE) = v_objEval.Eval(v_strVALEXP) Then
    '                                    If Me.UserLanguage = "EN" Then
    '                                        strTmp = strTmp & "Cột """ & mv_arrObjCoFields(.IDXFLD).Caption.Replace(":", "") & """" & " của dòng " & intVIndext + intRealRow & " : " & .EN_ERRMSG & vbCrLf
    '                                        mv_intErrCount = mv_intErrCount + 1
    '                                    Else
    '                                        strTmp = strTmp & "Cột """ & mv_arrObjCoFields(.IDXFLD).Caption.Replace(":", "") & """" & " của dòng " & intVIndext + intRealRow & " : " & .ERRMSG & vbCrLf
    '                                        mv_intErrCount = mv_intErrCount + 1
    '                                    End If
    '                                End If
    '                            Case "<>"
    '                                v_strVALEXP = BuildAMTEXP(.VALEXP, arrValue, intVIndext)
    '                                If Not CDbl(v_strFLDVALUE) <> v_objEval.Eval(v_strVALEXP) Then
    '                                    If Me.UserLanguage = "EN" Then
    '                                        strTmp = strTmp & "Cột """ & mv_arrObjCoFields(.IDXFLD).Caption.Replace(":", "") & """" & " của dòng " & intVIndext + intRealRow & " : " & .EN_ERRMSG & vbCrLf
    '                                        mv_intErrCount = mv_intErrCount + 1
    '                                    Else
    '                                        strTmp = strTmp & "Cột """ & mv_arrObjCoFields(.IDXFLD).Caption.Replace(":", "") & """" & " của dòng " & intVIndext + intRealRow & " : " & .ERRMSG & vbCrLf
    '                                        mv_intErrCount = mv_intErrCount + 1
    '                                    End If
    '                                End If
    '                            Case "IN"
    '                            Case "NI"
    '                        End Select
    '                    ElseIf mv_arrObjCoFields(.IDXFLD).DataType = "D" Then
    '                        If Not IsDateValue(v_strFLDVALUE) Then
    '                            strTmp = strTmp & "Cột """ & mv_arrObjCoFields(.IDXFLD).Caption.Replace(":", "") & """" & " của dòng " & intVIndext + intRealRow & " : " & mv_ResourceManager.GetString("ERR_DATE_VALUE") & vbCrLf
    '                            mv_intErrCount = mv_intErrCount + 1
    '                        Else
    '                            v_dtFLDVALUE = DDMMYYYY_SystemDate(v_strFLDVALUE)
    '                        End If
    '                        Select Case .mp_OPERATOR
    '                            Case ">>"
    '                                v_dtVALEXP = DDMMYYYY_SystemDate(BuildAMTEXP(.VALEXP, arrValue, intVIndext))
    '                                If Not v_dtFLDVALUE > v_dtVALEXP Then
    '                                    If Me.UserLanguage = "EN" Then
    '                                        strTmp = strTmp & "Cột """ & mv_arrObjCoFields(.IDXFLD).Caption.Replace(":", "") & """" & " của dòng " & intVIndext + intRealRow & " : " & .EN_ERRMSG & vbCrLf
    '                                        mv_intErrCount = mv_intErrCount + 1
    '                                    Else
    '                                        strTmp = strTmp & "Cột """ & mv_arrObjCoFields(.IDXFLD).Caption.Replace(":", "") & """" & " của dòng " & intVIndext + intRealRow & " : " & .ERRMSG & vbCrLf
    '                                        mv_intErrCount = mv_intErrCount + 1
    '                                    End If
    '                                End If
    '                            Case ">="
    '                                v_dtVALEXP = DDMMYYYY_SystemDate(BuildAMTEXP(.VALEXP, arrValue, intVIndext))
    '                                If v_dtFLDVALUE >= v_dtVALEXP Then
    '                                    If Me.UserLanguage = "EN" Then
    '                                        strTmp = strTmp & "Cột """ & mv_arrObjCoFields(.IDXFLD).Caption.Replace(":", "") & """" & " của dòng " & intVIndext + intRealRow & " : " & .EN_ERRMSG & vbCrLf
    '                                        mv_intErrCount = mv_intErrCount + 1
    '                                    Else
    '                                        strTmp = strTmp & "Cột """ & mv_arrObjCoFields(.IDXFLD).Caption.Replace(":", "") & """" & " của dòng " & intVIndext + intRealRow & " : " & .ERRMSG & vbCrLf
    '                                        mv_intErrCount = mv_intErrCount + 1
    '                                    End If
    '                                End If
    '                            Case "<<"
    '                                v_dtVALEXP = DDMMYYYY_SystemDate(BuildAMTEXP(.VALEXP, arrValue, intVIndext))
    '                                If Not v_dtFLDVALUE < v_dtVALEXP Then
    '                                    If Me.UserLanguage = "EN" Then
    '                                        strTmp = strTmp & "Cột """ & mv_arrObjCoFields(.IDXFLD).Caption.Replace(":", "") & """" & " của dòng " & intVIndext + intRealRow & " : " & .EN_ERRMSG & vbCrLf
    '                                        mv_intErrCount = mv_intErrCount + 1
    '                                    Else
    '                                        strTmp = strTmp & "Cột """ & mv_arrObjCoFields(.IDXFLD).Caption.Replace(":", "") & """" & " của dòng " & intVIndext + intRealRow & " : " & .ERRMSG & vbCrLf
    '                                        mv_intErrCount = mv_intErrCount + 1
    '                                    End If
    '                                End If
    '                            Case "<="
    '                                v_dtVALEXP = DDMMYYYY_SystemDate(BuildAMTEXP(.VALEXP, arrValue, intVIndext))
    '                                If Not v_dtFLDVALUE <= v_dtVALEXP Then
    '                                    If Me.UserLanguage = "EN" Then
    '                                        strTmp = strTmp & "Cột """ & mv_arrObjCoFields(.IDXFLD).Caption.Replace(":", "") & """" & " của dòng " & intVIndext + intRealRow & " : " & .EN_ERRMSG & vbCrLf
    '                                        mv_intErrCount = mv_intErrCount + 1
    '                                    Else
    '                                        strTmp = strTmp & "Cột """ & mv_arrObjCoFields(.IDXFLD).Caption.Replace(":", "") & """" & " của dòng " & intVIndext + intRealRow & " : " & .ERRMSG & vbCrLf
    '                                        mv_intErrCount = mv_intErrCount + 1
    '                                    End If
    '                                End If
    '                            Case "=="
    '                                v_dtVALEXP = DDMMYYYY_SystemDate(BuildAMTEXP(.VALEXP, arrValue, intVIndext))
    '                                If Not v_dtFLDVALUE = v_dtVALEXP Then
    '                                    If Me.UserLanguage = "EN" Then
    '                                        strTmp = strTmp & "Cột """ & mv_arrObjCoFields(.IDXFLD).Caption.Replace(":", "") & """" & " của dòng " & intVIndext + intRealRow & " : " & .EN_ERRMSG & vbCrLf
    '                                        mv_intErrCount = mv_intErrCount + 1
    '                                    Else
    '                                        strTmp = strTmp & "Cột """ & mv_arrObjCoFields(.IDXFLD).Caption.Replace(":", "") & """" & " của dòng " & intVIndext + intRealRow & " : " & .ERRMSG & vbCrLf
    '                                        mv_intErrCount = mv_intErrCount + 1
    '                                    End If
    '                                End If
    '                            Case "<>"
    '                                v_dtVALEXP = DDMMYYYY_SystemDate(BuildAMTEXP(.VALEXP, arrValue, intVIndext))
    '                                If Not v_dtFLDVALUE <> v_dtVALEXP Then
    '                                    If Me.UserLanguage = "EN" Then
    '                                        strTmp = strTmp & "Cột """ & mv_arrObjCoFields(.IDXFLD).Caption.Replace(":", "") & """" & " của dòng " & intVIndext + intRealRow & " : " & .EN_ERRMSG & vbCrLf
    '                                        mv_intErrCount = mv_intErrCount + 1
    '                                    Else
    '                                        strTmp = strTmp & "Cột """ & mv_arrObjCoFields(.IDXFLD).Caption.Replace(":", "") & """" & " của dòng " & intVIndext + intRealRow & " : " & .ERRMSG & vbCrLf
    '                                        mv_intErrCount = mv_intErrCount + 1
    '                                    End If
    '                                End If
    '                            Case "IN"
    '                            Case "NI"
    '                        End Select
    '                    End If
    '                End If
    '            End With
    '        End If
    '        Next
    '        If strTmp <> "" Then
    '            strErrMsg = strErrMsg.Replace("@" & intVIndext + intRealRow & vbCrLf, strTmp)
    '        End If
    '            Next
    '        End If
    '        End If

    '        Return True

    '    Catch ex As Exception
    '        Throw ex
    '    End Try

    'End Function

    Public Function CheckTranLimit(ByVal strTltxcd As String, ByVal dblTranLimit As Double, ByRef v_dblTranLimit As Double, Optional ByRef v_strMSGAMT As String = "") As Boolean
        Dim v_lngErrCode As Long = ERR_SYSTEM_OK
        Dim v_strErrorSource As String = "AppCore.frmTransaction.CheckTrans"
        Dim v_strSQLCMD, v_strObjMsg, v_strValue, v_strFLDNAME As String
        Dim v_xmlObjDocument As New Xml.XmlDocument
        Dim v_nodeList As Xml.XmlNodeList
        Dim v_dblMinTranLimit As Double

        Try
            v_dblTranLimit = 0
            v_strSQLCMD = "SELECT max(b.tllimit) max_tllimit, min(b.tllimit) min_tllimit,  max(a.msg_amt) msg_amt FROM TLTX a, " _
                            & " ( " _
                            & " select c.tltxcd, c.tllimit from tlauth c where c.status =0 and c.deleted =0 and c.authtype = 'U' and c.authid = '" & mv_strTellerId & "' and c.tltype = '0' " _
                            & " union" _
                            & " select c.tltxcd, c.tllimit from tlauth c where c.status =0 and c.deleted =0 and c.authtype = 'G'" _
                            & " and c.authid in ( select grpid from tlgrpusers d where d.tlid = '" & mv_strTellerId & "' and d.status =0 and d.deleted =0 ) and c.tltype = '0'" _
                            & " ) b, appmodules c " _
                            & " where a.status =0 and a.deleted =0 and a.tltxcd = b.tltxcd and substr(a.tltxcd,1,2) = c.txcode and c.deleted=0 and c.status = 0" _
                            & " and a.tltxcd = '" & strTltxcd & "'"

            v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_LOOKUP, gc_ActionInquiry, v_strSQLCMD)
            v_lngErrCode = Proxy.Message(v_strObjMsg)
            If v_lngErrCode <> ERR_SYSTEM_OK Then
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
                                Case "MAX_TLLIMIT"
                                    v_dblTranLimit = CDbl(v_strValue)
                                Case "MIN_TLLIMIT"
                                    v_dblMinTranLimit = CDbl(v_strValue)
                                Case "MSG_AMT"
                                    v_strMSGAMT = v_strValue
                            End Select
                        End With
                    Next
                Next
            Else
                v_oProcess.StopProcessForm()
                Application.DoEvents()
                MsgBox("Bạn không còn quyền thực hiện giao dịch này", MsgBoxStyle.Information, "Thông báo")
                mv_bolCheck = False
                Return False
            End If
            If v_dblMinTranLimit <> 0 Then
                If v_dblTranLimit < dblTranLimit Then
                    v_oProcess.StopProcessForm()
                    Application.DoEvents()
                    MsgBox("Bạn chỉ có thể thực hiện giao dịch với số lượng nhỏ hơn hoặc bằng " & v_dblTranLimit, MsgBoxStyle.Information, "Thông báo")
                    mv_bolCheck = False
                    Return False
                End If
            Else
                v_dblTranLimit = v_dblMinTranLimit
            End If
            'ContextUtil.SetComplete()
            Return True
        Catch ex As Exception
            'ContextUtil.SetAbort()
            Throw ex
        End Try

    End Function

    Private Sub mskData_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim v_intIndex As Integer
        v_intIndex = CType(sender, Control).Tag
        mv_lstIsTextChanged(v_intIndex) = True And Not mv_blnIsF5(v_intIndex)
        mv_blnIsF5(v_intIndex) = False
        Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Text = ""
        Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).ForeColor = System.Drawing.Color.Blue
        For i As Integer = 0 To mv_arrObjFields.Length - 2
            If mv_arrObjFields(i).InvName <> "" Then
                If mv_arrObjFields(i).InvName.Split("|").Contains(mv_arrObjFields(v_intIndex).FieldName) Then
                    mv_lstIsTextChanged(i) = True And Not mv_blnIsF5(i)
                    mskData_Validating(Me.pnTransDetail.Controls(mv_arrObjFields(i).ControlIndex))
                End If
            End If
        Next
    End Sub

    Private Sub mskData_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim v_intIndex As Integer
        v_intIndex = CType(sender, Control).Tag
        mv_lstIsTextChanged(v_intIndex) = True And Not mv_blnIsF5(v_intIndex)
        mv_blnIsF5(v_intIndex) = False
        Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).Text = ""
        Me.pnTransDetail.Controls(mv_arrObjFields(v_intIndex).LabelIndex).ForeColor = System.Drawing.Color.Blue
        For i As Integer = 0 To mv_arrObjFields.Length - 2
            If mv_arrObjFields(i).InvName <> "" Then
                If mv_arrObjFields(i).InvName.Split("|").Contains(mv_arrObjFields(v_intIndex).FieldName) Then
                    mv_lstIsTextChanged(i) = True And Not mv_blnIsF5(i)
                    mskData_Validating(Me.pnTransDetail.Controls(mv_arrObjFields(i).ControlIndex))
                End If
            End If
        Next
    End Sub
    'start Myvq
    Private Function GetExportFilePath() As String
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
    Private Function GetImportFilePath() As String
        Try
            Dim v_strSQL As String = ""
            Dim v_xmlDocument As New Xml.XmlDocument
            Dim v_nodeList As Xml.XmlNodeList
            Dim v_strObjMsg As String
            Dim v_strFLDNAME, v_strValue, v_strFilePath As String

            v_strSQL = "SELECT VARVALUE, VARDESC, EN_VARDESC FROM SYSVAR WHERE TRIM(GRNAME) = 'SYSTEM' AND TRIM(VARNAME)='FILE_IMPORT_PATH' AND DELETED = 0 AND STATUS = 0"

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
    Public Sub GetCurrDate(ByVal pv_strBrid As String)
        'start Myvq
        mv_strBrid = pv_strBrid
        Dim v_oXmlDocument As New XmlDocumentEx
        'Lấy thông số FTP
        Dim v_strSQL As String = "select a.varname varname, a.varvalue varvalue from sysvar a where grname = 'SYSTEM' and brid = '" + mv_strBrid + "' and varname = 'CURRDATE'"
        Dim v_strObjMessage = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLTX, gc_ActionInquiry, v_strSQL)
        Dim v_Error As Long = Proxy.Message(v_strObjMessage)
        If v_Error <> ERR_SYSTEM_OK Then
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
            Exit Sub
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
                            mv_strCurrDate = v_strValue
                    End Select
                End With
            Next
        Next
        'end Myvq
    End Sub

    Private Sub SendFileSblFtpBnk(ByVal pv_strGrName As String, ByVal pv_strBrid As String, ByVal pv_strFileName As String)
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
            v_ftpClient.Upload(pv_strFileName, True)
            v_ftpClient.Close()
            v_ftpClient.Dispose()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Function SendFileFTP(ByVal FilePath As String, ByVal FileName As String, ByVal BrID As String) As Boolean
        Try
            Dim ServerAddress, ServerAddress1 As String
            Dim ServerPort, ServerPort1 As String
            Dim Username As String
            Dim Password As String

            Dim v_strSQL As String
            Dim v_strObjMsg As String

            Dim v_xmlDocument As New XmlDocumentEx
            'Lấy thông số FTP
            v_strSQL = "SELECT a.VARNAME, a.VARVALUE FROM sysvar a where a.GRNAME='VSDFTPSVR' and brid ='" & BrID & "'"
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
                        'bangpv 
                    Case "ServerAddress1"
                        ServerAddress1 = v_strVarValue
                        'end bangpv 
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

            Dim v_oWriter As System.IO.StreamWriter
            ' gửi đi ở server ftp đầu 1 
            If CheckExistFile(FilePath, FileName.Split(".")(0) & ".bat") Then
                File.Delete(FilePath & "\" & FileName.Split(".")(0) & ".bat")
            End If

            v_oWriter = New StreamWriter(FilePath & "\" & FileName.Split(".")(0) & ".bat")
            'bangp_add_run_on_x64system
            v_oWriter.WriteLine("set " & """" & "SystemPath=%windir%\System32" & """")
            v_oWriter.WriteLine("if not " & """" & "%ProgramFiles(x86)%" & """" & "==" & """""" _
                                & " set " & """" & "SystemPath=%windir%\SysWOW64" & """")
            'end bangp
            v_oWriter.WriteLine("@ftp -i -s:""%~f0""&GOTO:EOF")
            v_oWriter.WriteLine("open " & ServerAddress)
            v_oWriter.WriteLine(Username)
            v_oWriter.WriteLine(Password)
            If Mid(FilePath, FilePath.Length, 1) = "\" Then
                v_oWriter.WriteLine("lcd " & """" & Mid(FilePath, 1, FilePath.Length - 1) & """")
            Else
                v_oWriter.WriteLine("lcd " & """" & FilePath & """")
            End If

            v_oWriter.WriteLine("cd " & RemotePath)
            v_oWriter.WriteLine("del " & FileName)
            v_oWriter.WriteLine("binary")
            v_oWriter.WriteLine("put " & FileName & " " & FileName)
            'day them file ssk da ma hoa voi giao dich 1123 va 1131
            If InStr(FileName, "1123") > 0 Then
                v_oWriter.WriteLine("put " & Replace(FileName, ".enc", ".dat") & " " & Replace(FileName, ".enc", ".dat"))
            End If
            'Added by Thanglv 14/04/2013
            If InStr(FileName, "1131") > 0 Then
                v_oWriter.WriteLine("put " & Replace(FileName, ".enc", ".dat") & " " & Replace(FileName, ".enc", ".dat"))
            End If
            'End Thanglv9
            v_oWriter.WriteLine("bye" & vbCrLf)

            v_oWriter.Close()

            Dim v_oProcess As Process
            v_oProcess = New Process

            v_oProcess.StartInfo.FileName = FilePath & "\" & FileName.Split(".")(0) & ".bat"
            v_oProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
            v_oProcess.StartInfo.CreateNoWindow = True
            v_oProcess.Start()
            'System.Threading.Thread.Sleep(10 * 1000)
            'v_oProcess.WaitForExit()
            v_oProcess.Close()
            'Gửi đi ở FTP đầu 2
            If CheckExistFile(FilePath, FileName.Split(".")(0) & "1.bat") Then
                File.Delete(FilePath & "\" & FileName.Split(".")(0) & "1.bat")
            End If

            v_oWriter = New StreamWriter(FilePath & "\" & FileName.Split(".")(0) & "1.bat")
            'bangp_add_run_on_x64system
            v_oWriter.WriteLine("set " & """" & "SystemPath=%windir%\System32" & """")
            v_oWriter.WriteLine("if not " & """" & "%ProgramFiles(x86)%" & """" & "==" & """""" _
                                & " set " & """" & "SystemPath=%windir%\SysWOW64" & """")
            'end bangp
            v_oWriter.WriteLine("@ftp -i -s:""%~f0""&GOTO:EOF")
            v_oWriter.WriteLine("open " & ServerAddress1)
            v_oWriter.WriteLine(Username)
            v_oWriter.WriteLine(Password)
            If Mid(FilePath, FilePath.Length, 1) = "\" Then
                v_oWriter.WriteLine("lcd " & """" & Mid(FilePath, 1, FilePath.Length - 1) & """")
            Else
                v_oWriter.WriteLine("lcd " & """" & FilePath & """")
            End If
            v_oWriter.WriteLine("cd " & RemotePath)
            v_oWriter.WriteLine("del " & FileName)
            v_oWriter.WriteLine("binary")
            v_oWriter.WriteLine("put " & FileName & " " & FileName)
            'day them file ssk da ma hoa voi giao dich 1123
            If InStr(FileName, "1123") > 0 Then
                v_oWriter.WriteLine("put " & Replace(FileName, ".enc", ".dat") & " " & Replace(FileName, ".enc", ".dat"))
            End If
            'Added by Thanglv 14/04/2013
            If InStr(FileName, "1131") > 0 Then
                v_oWriter.WriteLine("put " & Replace(FileName, ".enc", ".dat") & " " & Replace(FileName, ".enc", ".dat"))
            End If
            'End Thanglv9
            v_oWriter.WriteLine("bye" & vbCrLf)

            v_oWriter.Close()

            'Dim v_oProcess As Process
            v_oProcess = New Process

            v_oProcess.StartInfo.FileName = FilePath & "\" & FileName.Split(".")(0) & "1.bat"
            v_oProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
            v_oProcess.StartInfo.CreateNoWindow = True
            v_oProcess.Start()
            'System.Threading.Thread.Sleep(10 * 1000)
            'v_oProcess.WaitForExit()
            v_oProcess.Close()
            'MsgBox("Đẩy file lên máy chủ VSD thành công", MsgBoxStyle.Exclamation, "Thông báo")
            System.Threading.Thread.Sleep(30 * 1000)
            If CheckExistFile(FilePath, FileName.Split(".")(0) & ".bat") Then
                File.Delete(FilePath & "\" & FileName.Split(".")(0) & ".bat")
            End If
        Catch ex As Exception
            Return False
            Throw ex
        End Try
    End Function

    'Dung cho SGD NHNN
    Private Function SendFileFTPSBV(ByVal FilePath As String, ByVal FileName As String, ByVal BrID As String) As Boolean
        Try
            Dim ServerAddress, ServerAddress1 As String
            Dim ServerPort, ServerPort1 As String
            Dim Username As String
            Dim Password As String

            Dim v_strSQL As String
            Dim v_strObjMsg As String

            Dim v_xmlDocument As New XmlDocumentEx
            'Lấy thông số FTP
            v_strSQL = "SELECT a.VARNAME, a.VARVALUE FROM sysvar a where a.GRNAME='VSDFTPSBV' and brid ='" & BrID & "'"
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
                        'bangpv 
                    Case "ServerAddress1"
                        ServerAddress1 = v_strVarValue
                        'end bangpv 
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

            Dim v_oWriter As System.IO.StreamWriter
            ' gửi đi ở server ftp đầu 1 
            If CheckExistFile(FilePath, FileName.Split(".")(0) & ".bat") Then
                File.Delete(FilePath & "\" & FileName.Split(".")(0) & ".bat")
            End If

            v_oWriter = New StreamWriter(FilePath & "\" & FileName.Split(".")(0) & ".bat")
            'bangpv_add_run_on_x64system
            v_oWriter.WriteLine("set " & """" & "SystemPath=%windir%\System32" & """")
            v_oWriter.WriteLine("if not " & """" & "%ProgramFiles(x86)%" & """" & "==" & """""" _
                                & " set " & """" & "SystemPath=%windir%\SysWOW64" & """")
            'end bangpv
            v_oWriter.WriteLine("@ftp -i -s:""%~f0""&GOTO:EOF")
            v_oWriter.WriteLine("open " & ServerAddress)
            v_oWriter.WriteLine(Username)
            v_oWriter.WriteLine(Password)
            If Mid(FilePath, FilePath.Length, 1) = "\" Then
                v_oWriter.WriteLine("lcd " & """" & Mid(FilePath, 1, FilePath.Length - 1) & """")
            Else
                v_oWriter.WriteLine("lcd " & """" & FilePath & """")
            End If

            v_oWriter.WriteLine("cd " & RemotePath)
            v_oWriter.WriteLine("del " & FileName)
            v_oWriter.WriteLine("binary")
            v_oWriter.WriteLine("put " & FileName & " " & FileName)
            'day them file ssk da ma hoa voi giao dich 1123 va 1131
            If InStr(FileName, "1123") > 0 Then
                v_oWriter.WriteLine("put " & Replace(FileName, ".enc", ".dat") & " " & Replace(FileName, ".enc", ".dat"))
            End If
            'Added by Thanglv 14/04/2013
            If InStr(FileName, "1131") > 0 Then
                v_oWriter.WriteLine("put " & Replace(FileName, ".enc", ".dat") & " " & Replace(FileName, ".enc", ".dat"))
            End If
            'End Thanglv9
            v_oWriter.WriteLine("bye" & vbCrLf)

            v_oWriter.Close()

            Dim v_oProcess As Process
            v_oProcess = New Process

            v_oProcess.StartInfo.FileName = FilePath & "\" & FileName.Split(".")(0) & ".bat"
            v_oProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
            v_oProcess.StartInfo.CreateNoWindow = True
            v_oProcess.Start()
            'v_oProcess.WaitForExit()
            'System.Threading.Thread.Sleep(2 * 1000)
            v_oProcess.Close()
            'Gửi đi ở FTP đầu 2
            If CheckExistFile(FilePath, FileName.Split(".")(0) & "1.bat") Then
                File.Delete(FilePath & "\" & FileName.Split(".")(0) & "1.bat")
            End If

            v_oWriter = New StreamWriter(FilePath & "\" & FileName.Split(".")(0) & "1.bat")
            'bangp_add_run_on_x64system
            v_oWriter.WriteLine("set " & """" & "SystemPath=%windir%\System32" & """")
            v_oWriter.WriteLine("if not " & """" & "%ProgramFiles(x86)%" & """" & "==" & """""" _
                                & " set " & """" & "SystemPath=%windir%\SysWOW64" & """")
            'end bangpv
            v_oWriter.WriteLine("@ftp -i -s:""%~f0""&GOTO:EOF")
            v_oWriter.WriteLine("open " & ServerAddress1)
            v_oWriter.WriteLine(Username)
            v_oWriter.WriteLine(Password)
            If Mid(FilePath, FilePath.Length, 1) = "\" Then
                v_oWriter.WriteLine("lcd " & """" & Mid(FilePath, 1, FilePath.Length - 1) & """")
            Else
                v_oWriter.WriteLine("lcd " & """" & FilePath & """")
            End If
            v_oWriter.WriteLine("cd " & RemotePath)
            v_oWriter.WriteLine("del " & FileName)
            v_oWriter.WriteLine("binary")
            v_oWriter.WriteLine("put " & FileName & " " & FileName)
            'day them file ssk da ma hoa voi giao dich 1123
            If InStr(FileName, "1123") > 0 Then
                v_oWriter.WriteLine("put " & Replace(FileName, ".enc", ".dat") & " " & Replace(FileName, ".enc", ".dat"))
            End If
            'Added by Thanglv 14/04/2013
            If InStr(FileName, "1131") > 0 Then
                v_oWriter.WriteLine("put " & Replace(FileName, ".enc", ".dat") & " " & Replace(FileName, ".enc", ".dat"))
            End If
            'End Thanglv9
            v_oWriter.WriteLine("bye" & vbCrLf)

            v_oWriter.Close()

            'Dim v_oProcess As Process
            v_oProcess = New Process

            v_oProcess.StartInfo.FileName = FilePath & "\" & FileName.Split(".")(0) & "1.bat"
            v_oProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
            v_oProcess.StartInfo.CreateNoWindow = True
            v_oProcess.Start()
            'System.Threading.Thread.Sleep(2 * 1000)
            'v_oProcess.WaitForExit()
            v_oProcess.Close()
            'MsgBox("Đẩy file lên máy chủ VSD thành công", MsgBoxStyle.Exclamation, "Thông báo")
            System.Threading.Thread.Sleep(30 * 1000)
            If CheckExistFile(FilePath, FileName.Split(".")(0) & ".bat") Then
                File.Delete(FilePath & "\" & FileName.Split(".")(0) & ".bat")
            End If
        Catch ex As Exception
            Return False
            Throw ex
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

    Private Sub lblTranCode_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblTranCode.Click

    End Sub
End Class