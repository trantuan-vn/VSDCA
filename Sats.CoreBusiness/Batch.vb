Imports SATS.CommonLibrary
Imports SATS.CoreBusiness
Imports SATS.DataAccessLayer
'Imports System.EnterpriseServices
'<JustInTimeActivation(False), _
'Transaction(TransactionOption.Disabled), _
'ObjectPooling(Enabled:=True, MinPoolSize:=30)> _
Public Class Batch
    Implements IDisposable
    'Inherits ServicedComponent

#Region " Khai báo hằng, biến "
    Dim mv_sModule As String
    Dim mv_sTable As String
#End Region

#Region " Các thuộc tính của lớp "
    Public Property ATTR_MODULE() As String
        Get
            Return mv_sModule
        End Get
        Set(ByVal Value As String)
            mv_sModule = Value
        End Set
    End Property

    Public Property ATTR_TABLE() As String
        Get
            Return mv_sTable
        End Get
        Set(ByVal Value As String)
            mv_sTable = Value
        End Set
    End Property
#End Region

#Region " Các hàm xử lý Batch "

    'Hàm này khởi tạo message cho bước chạy Batch
    Overridable Function BuildBatchTxMsg(ByRef v_xmlDocument As Xml.XmlDocument, ByVal v_strDetailBatchName As String) As Long
        Dim v_lngErrCode As Long = ERR_SYSTEM_OK
        Dim v_strErrorSource As String = "CoreBusiness.Batch.BuildBatchTxMsg"
        Dim v_obj As New DataAccess
        Dim v_strTxMsg As String, v_strTxNum As String, v_strTxDate As String = ""
        Try
            v_obj.NewDBInstance(gc_MODULE_HOST)
            'Lấy số chứng từ
            v_strTxNum = BATCH_PREFIXED & Right(gc_FORMAT_BATCHTXNUM & CStr(v_obj.GetIDValue("BATCHTXNUM")), Len(gc_FORMAT_BATCHTXNUM) - Len(BATCH_PREFIXED))
            'Lấy ngày làm việc hiện tại
            v_lngErrCode = v_obj.GetSysVar("SYSTEM", "CURRDATE", v_strTxDate)
            If v_lngErrCode <> ERR_SYSTEM_OK Then
                'ContextUtil.SetAbort()
                Return v_lngErrCode
            End If

            'Tạo message trả v�?
            v_strTxMsg = BuildXMLTxMsg(gc_MsgTypeTrans, gc_IsLocalMsg, , "0000", "0000", "HOST", "HOST")
            v_xmlDocument.LoadXml(v_strTxMsg)
            v_xmlDocument.DocumentElement.Attributes(gc_AtributeTXNUM).Value = v_strTxNum
            v_xmlDocument.DocumentElement.Attributes(gc_AtributeTXDATE).Value = v_strTxDate
            v_xmlDocument.DocumentElement.Attributes(gc_AtributeBATCHNAME).Value = v_strDetailBatchName
            v_xmlDocument.DocumentElement.Attributes(gc_AtributeSTATUS).Value = CStr(TransactStatus.LOG_MEMBER_STAFF)
            'ContextUtil.SetComplete()
            Return v_lngErrCode
        Catch ex As Exception
            'ContextUtil.SetAbort()
            'LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
            '             & "Error code: System error!" & vbNewLine _
            '             & "Error message: " & ex.Message, EventLogEntryType.Error)
            Throw ex
        End Try
    End Function

    'Lấy giá trị biến SDE.DAYS
    Public Function GetSDE_DAYS(ByVal pv_strLastICCFDate As String, ByVal pv_strMONTHDAY As String, ByRef pv_intDAYS As Integer) As Long
        Dim v_lngErrCode As Long = ERR_SYSTEM_OK
        Dim v_strErrorSource As String = ATTR_MODULE & ".Batch.GetSDE_DAYS"
        Dim v_strSYSVAR As String = "", v_obj As New DataAccess
        Try
            v_obj.NewDBInstance(gc_MODULE_HOST)
            Dim v_dtCURRDATE, v_dtCMPDATE, v_dtBOMDATE As Date, v_intCMPDAY, v_intCURRDAY As Integer
            v_lngErrCode = v_obj.GetSysVar("SYSTEM", "CURRDATE", v_strSYSVAR)
            If v_lngErrCode <> ERR_SYSTEM_OK Then
                'ContextUtil.SetAbort()
                Return v_lngErrCode
            End If
            v_dtCURRDATE = DDMMYYYY_SystemDate(v_strSYSVAR)
            v_dtCMPDATE = DDMMYYYY_SystemDate(pv_strLastICCFDate)
            v_dtBOMDATE = DDMMYYYY_SystemDate("01/" & v_dtCMPDATE.Month & "/" & v_dtCMPDATE.Year)
            Select Case pv_strMONTHDAY
                Case "A"
                    'Số ngày thực tế
                    pv_intDAYS = DateDiff(DateInterval.Day, v_dtCMPDATE, v_dtCURRDATE)
                Case "M"
                    'Làm tròn tháng 30 ngày
                    If DateDiff(DateInterval.Month, v_dtCMPDATE, v_dtCURRDATE) > 0 Then
                        'Nếu khác tháng. Lấy số ngày trong các tháng nằm giữa CMPDATE và CURRDATE
                        pv_intDAYS = (DateDiff(DateInterval.Month, v_dtCMPDATE, v_dtCURRDATE) - 1) * 30
                        'Làm tròn tháng chỉ có 30 ngày
                        v_intCMPDAY = IIf(30 - v_dtCMPDATE.Day > 0, 30 - v_dtCMPDATE.Day, 0)
                        v_intCURRDAY = IIf(v_dtCURRDATE.Day > 30, 30, v_dtCURRDATE.Day)
                        pv_intDAYS = pv_intDAYS + v_intCMPDAY + v_intCURRDAY
                    Else
                        pv_intDAYS = DateDiff(DateInterval.Day, v_dtCMPDATE, v_dtCURRDATE)
                        If pv_intDAYS > 30 Then pv_intDAYS = 30
                    End If
                Case "E"
                    'Làm tròn tháng 30 ngày, nhưng nếu có tháng 2 thì phải xác định chính xác số ngày của tháng 2 đó. 
                    'Giải thuật là nếu có tháng 2 nằm trong khoảng th�?i gian xử lý thì xác định số ngày của tháng 2 đó

                    'Làm tròn tháng 30 ngày (giống với MONTHDAY=M)
                    If DateDiff(DateInterval.Month, v_dtCMPDATE, v_dtCURRDATE) > 0 Then
                        'Nếu khác tháng. Lấy số ngày trong các tháng nằm giữa CMPDATE và CURRDATE
                        pv_intDAYS = (DateDiff(DateInterval.Month, v_dtCMPDATE, v_dtCURRDATE) - 1) * 30
                        'Làm tròn tháng chỉ có 30 ngày
                        v_intCMPDAY = IIf(30 - v_dtCMPDATE.Day > 0, 30 - v_dtCMPDATE.Day, 0)
                        v_intCURRDAY = IIf(v_dtCURRDATE.Day > 30, 30, v_dtCURRDATE.Day)
                        pv_intDAYS = pv_intDAYS + v_intCMPDAY + v_intCURRDAY
                    Else
                        pv_intDAYS = DateDiff(DateInterval.Day, v_dtCMPDATE, v_dtCURRDATE)
                        If pv_intDAYS > 30 Then pv_intDAYS = 30
                    End If

                    'Duyệt từ v_dtCMPDATE đến v_dtCURRDATE có bao nhiêu tháng 2
                    v_intCMPDAY = 0
                    While v_dtCMPDATE < v_dtCURRDATE
                        If v_dtCMPDATE.Month = 2 Then
                            v_intCMPDAY = v_intCMPDAY + 30 - Date.DaysInMonth(v_dtCMPDATE.Year, 2)
                        End If
                        v_dtCMPDATE = DateAdd(DateInterval.Month, 1, v_dtCMPDATE)
                    End While
                    'Xử lý nếu ngày hiện tại là cuối tháng 2
                    If v_dtCURRDATE.Month = 2 And v_dtCURRDATE.Day >= Date.DaysInMonth(v_dtCURRDATE.Year, 2) Then
                        v_intCMPDAY = v_intCMPDAY + 30 - Date.DaysInMonth(v_dtCURRDATE.Year, 2)
                    End If
                    'Số ngày tính được sẽ bằng số tròn 30 ngày trừ đi số ngày tháng 2 bị bù
                    pv_intDAYS = pv_intDAYS - v_intCMPDAY
            End Select
            'ContextUtil.SetComplete()
            Return v_lngErrCode
        Catch ex As Exception
            'ContextUtil.SetAbort()
            Throw ex
        End Try
    End Function

    Public Function GetSDEFormulaValue(ByVal pv_strSDEName As String, ByRef pv_strValue As String) As Long
        Dim v_lngErrCode As Long = ERR_SYSTEM_OK
        Dim v_strSYSVAR As String = "", v_obj As New DataAccess, v_dtREFDATE As Date
        Try
            v_obj.NewDBInstance(gc_MODULE_HOST)
            v_lngErrCode = v_obj.GetSysVar("SYSTEM", "CURRDATE", v_strSYSVAR)
            If v_lngErrCode <> ERR_SYSTEM_OK Then
                'ContextUtil.SetAbort()
                Return v_lngErrCode
            End If
            v_dtREFDATE = DDMMYYYY_SystemDate(v_strSYSVAR)
            Select Case pv_strSDEName
                Case "MONTHDAY"
                    'pv_strValue là giá trị được ch�?n
                    If pv_strValue = "A" Then
                        'Lấy số ngày thực tế trong tháng
                        pv_strValue = CStr(Date.DaysInMonth(v_dtREFDATE.Year, v_dtREFDATE.Month))
                    ElseIf pv_strValue = "M" Then
                        'Lấy tròn 30 ngày
                        pv_strValue = "30"
                    ElseIf pv_strValue = "E" Then
                        'Lấy tròn 30 ngày trừ tháng 2 lấy theo đúng ngày thực tế
                        If GetDateValue(v_strSYSVAR, "M") <> 2 Then
                            pv_strValue = "30"
                        Else
                            pv_strValue = CStr(Date.DaysInMonth(v_dtREFDATE.Year, 2))
                        End If
                    End If
                Case "YEARDAY"
                    'pv_strValue là giá trị được ch�?n
                    If pv_strValue = "A" Then
                        'Lấy số ngày thực tế trong năm
                        If Date.DaysInMonth(v_dtREFDATE.Year, 2) = 28 Then
                            pv_strValue = "365"
                        Else
                            pv_strValue = "366"
                        End If
                    ElseIf pv_strValue = "M" Then
                        'Lấy tháng tròn 30 ngày -> 1 năm 360 ngày
                        pv_strValue = "360"
                    ElseIf pv_strValue = "E" Then
                        'Lấy tròn 30 ngày trừ tháng 2 lấy theo đúng ngày thực tế
                        pv_strValue = CStr(360 - Date.DaysInMonth(v_dtREFDATE.Year, 2))
                    End If
                Case "RATE"
                    'pv_strValue là giá trị mã lãi suất thả nổi

                Case Else
                    'Lấy trong bảng SYSVAR SYSTEM + DEFINED
                    Dim v_strSys As String = "", v_strDef As String = ""
                    'v_lngErrCode = v_obj.GetSysVar("SYSTEM", pv_strSDEName, pv_strValue)
                    'If v_lngErrCode <> ERR_SYSTEM_OK Then
                    '    ContextUtil.SetAbort()
                    '    Return v_lngErrCode
                    'End If

                    'Khai VARNAME trong sysvar khong duoc trung ten
                    v_lngErrCode = v_obj.GetSysVar("SYSTEM", pv_strSDEName, v_strSys)
                    If v_strSys Is Nothing Then
                        v_lngErrCode = v_obj.GetSysVar("DEFINED", pv_strSDEName, v_strDef)
                        If v_lngErrCode <> ERR_SYSTEM_OK Then
                            'ContextUtil.SetAbort()
                            Return v_lngErrCode
                        End If
                    End If

                    pv_strValue = IIf(v_strSys Is Nothing, "", v_strSys) & IIf(v_strDef Is Nothing, "", v_strDef)
            End Select
            'ContextUtil.SetComplete()
            Return v_lngErrCode
        Catch ex As Exception
            'ContextUtil.SetAbort()
            Throw ex
        End Try
    End Function

    'Hàm này tính toán biểu thức số h�?c trên cơ sở mảng ICCFTX
    Public Function BuildICCFTXEXP(ByVal pv_arrICCFTX() As ICCFTX, ByVal strAMTEXP As String) As String
        Try
            Dim v_strEvaluator, v_strElemenent As String, v_lngIndex, i As Long
            v_strEvaluator = vbNullString
            v_lngIndex = 1

            If Mid(strAMTEXP, 1, 1) = "@" Then 'Lấy trực tiếp giá trị sau ký tự @
                Return Mid(strAMTEXP, 2)
            ElseIf Mid(strAMTEXP, 1) = "$" Then 'Lấy giá trị của trư�?ng
                strAMTEXP = Mid(strAMTEXP, 2)
                '�?�?c thông tin trong mảng pv_arrICCFTX để lấy giá trị
                If pv_arrICCFTX.GetLength(0) > 0 Then
                    For i = 0 To pv_arrICCFTX.GetLength(0) - 1 Step 1
                        If Not pv_arrICCFTX(i) Is Nothing Then
                            If pv_arrICCFTX(i).TXCD = strAMTEXP Then
                                'ContextUtil.SetComplete()
                                Return pv_arrICCFTX(i).VALNUMBER
                            End If
                        End If
                    Next
                End If
            Else 'Biểu thức số h�?c
                While v_lngIndex < Len(strAMTEXP)
                    'Get 02 charatacters in AMTEXP
                    v_strElemenent = Mid$(strAMTEXP, v_lngIndex, 2)
                    Select Case v_strElemenent
                        Case "++", "--", "**", "//", "((", "))"
                            'Operand: Toán tử
                            v_strEvaluator = v_strEvaluator & Left$(v_strElemenent, 1)
                        Case Else
                            'Operator: Toán hạng - đ�?c thông tin trong mảng pv_arrICCFTX để lấy giá trị
                            If pv_arrICCFTX.GetLength(0) > 0 Then
                                For i = 0 To pv_arrICCFTX.GetLength(0) - 1 Step 1
                                    If Not pv_arrICCFTX(i) Is Nothing Then
                                        If pv_arrICCFTX(i).TXCD = v_strElemenent Then
                                            v_strEvaluator = v_strEvaluator & pv_arrICCFTX(i).VALNUMBER
                                            Exit For
                                        End If
                                    End If
                                Next
                            End If
                    End Select
                    v_lngIndex = v_lngIndex + 2
                End While
                'ContextUtil.SetComplete()
                Return v_strEvaluator
            End If
            Return String.Empty
        Catch ex As Exception
            'ContextUtil.SetAbort()
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            '             & "Error code: System error!" & vbNewLine _
            '             & "Error message: " & ex.Message, EventLogEntryType.Error)
            Throw ex
        End Try
    End Function

    'Hàm thực hiện kiểm tra đi�?u kiện
    Public Function CheckCompareExpression(ByVal pv_arrICCFTX() As ICCFTX, _
        ByVal pv_strCMPCD As String, ByVal pv_strOPERAND As String, ByVal pv_strCMPEXP As String) As Boolean
        Try
            Dim i As Integer, v_strValue As String = String.Empty
            For i = 0 To pv_arrICCFTX.GetLength(0) - 1 Step 1
            Next
            'ContextUtil.SetComplete()
            Return False
        Catch ex As Exception
            'ContextUtil.SetAbort()
            Throw ex
        End Try
    End Function

    'Hàm lấy giá trị của ICCFTX theo REFNAME
    Public Function GetICCFTXValueByREFNAME(ByVal pv_arrICCFTX() As ICCFTX, ByVal pv_strREFNAME As String) As String
        Try
            Dim i As Integer, v_strValue As String = String.Empty
            For i = 0 To pv_arrICCFTX.GetLength(0) - 1 Step 1
                If Not pv_arrICCFTX(i) Is Nothing Then
                    If Trim(pv_arrICCFTX(i).REFNAME) = Trim(pv_strREFNAME) Then
                        If pv_arrICCFTX(i).DATATYPE = "N" Then
                            v_strValue = CStr(pv_arrICCFTX(i).VALNUMBER)
                        Else
                            v_strValue = pv_arrICCFTX(i).VALCHAR
                        End If
                        Return v_strValue
                    End If
                End If
            Next
            'ContextUtil.SetComplete()
            Return v_strValue
        Catch ex As Exception
            'ContextUtil.SetAbort()
            Throw ex
        End Try
    End Function

    'Hàm lấy giá trị của ICCFTX theo TXCD
    Public Function GetICCFTXValueByTXCD(ByVal pv_arrICCFTX() As ICCFTX, ByVal pv_strTXCD As String) As String
        Try
            Dim i As Integer, v_strValue As String = String.Empty
            For i = 0 To pv_arrICCFTX.GetLength(0) - 1 Step 1
                If Not pv_arrICCFTX(i) Is Nothing Then
                    If Trim(pv_arrICCFTX(i).TXCD) = Trim(pv_strTXCD) Then
                        If pv_arrICCFTX(i).DATATYPE = "N" Then
                            v_strValue = CStr(pv_arrICCFTX(i).VALNUMBER)
                        Else
                            v_strValue = pv_arrICCFTX(i).VALCHAR
                        End If
                        Return v_strValue
                    End If
                End If
            Next
            'ContextUtil.SetComplete()
            Return v_strValue
        Catch ex As Exception
            'ContextUtil.SetAbort()
            Throw ex
        End Try
    End Function

    Overridable Function ICCFCalculate(ByVal v_strBATCHNAME As String, Optional ByVal v_strBCHFillter As String = "", Optional ByRef v_intMaxRow As Integer = 0) As Long
        Dim v_lngErrCode As Long = ERR_SYSTEM_OK
        Dim v_strErrorSource As String = ATTR_MODULE & ".Batch.ICCFCalculate." & v_strBATCHNAME
        Dim v_strSQL As String = "", v_ds, v_dsIRRATE, v_dsMAST, v_dsICCFTX, v_dsTIER, v_dsCHECK As DataSet
        Dim v_obj As New DataAccess, v_objMessageLog As New MessageLog
        v_objMessageLog.NewDBInstance(gc_MODULE_HOST)
        Dim v_xmlDocument As New Xml.XmlDocument
        Dim v_arrICCFTX() As ICCFTX

        Try
            v_obj.NewDBInstance(gc_MODULE_HOST)
            'Lấy tham số ngày làm việc
            Dim v_strCURRDATE As String = "", v_strPREVDATE As String = "", v_strNEXTDATE As String = ""
            Dim v_strPERIOD As String, v_strICCFLASTDATE As String
            Dim v_intCURRMONTH, v_intCURRYEAR, v_intNEXTMONTH, v_intNEXTYEAR As Integer

            v_lngErrCode = v_obj.GetSysVar("SYSTEM", "PREVDATE", v_strPREVDATE)
            If v_lngErrCode <> ERR_SYSTEM_OK Then
                'ContextUtil.SetAbort()
                Return v_lngErrCode
            End If

            v_lngErrCode = v_obj.GetSysVar("SYSTEM", "CURRDATE", v_strCURRDATE)
            If v_lngErrCode <> ERR_SYSTEM_OK Then
                'ContextUtil.SetAbort()
                Return v_lngErrCode
            End If

            v_lngErrCode = v_obj.GetSysVar("SYSTEM", "NEXTDATE", v_strNEXTDATE)
            If v_lngErrCode <> ERR_SYSTEM_OK Then
                'ContextUtil.SetAbort()
                Return v_lngErrCode
            End If

            v_strICCFLASTDATE = v_strPREVDATE

            'L�?c các tài khoản để tính lãi
            If GetDateValue(v_strCURRDATE, "Y") <> GetDateValue(v_strNEXTDATE, "Y") Then
                'EOY. Ngày cuối năm
                v_strPERIOD = "('D','M','Y')"
            ElseIf GetDateValue(v_strCURRDATE, "M") <> GetDateValue(v_strNEXTDATE, "M") Then
                'EOM. Ngày cuối tháng
                v_strPERIOD = "('D','M')"
            Else
                'EOD. Cuối ngày
                v_strPERIOD = "('D')"
            End If

            '�?ối với loại PERIOD=S, xác định chính xác thì PERIODDAY phải nằm trong khoảng CURRDATE & NEXTDATE
            v_intCURRMONTH = GetDateValue(v_strCURRDATE, "M")
            v_intCURRYEAR = GetDateValue(v_strCURRDATE, "Y")
            v_intNEXTMONTH = GetDateValue(v_strNEXTDATE, "M")
            v_intNEXTYEAR = GetDateValue(v_strNEXTDATE, "Y")

            'Thuật toán: 
            Dim v_strCRITERIA, v_strACTYPE, v_strEVENTCODE, v_strTLTXCD, v_strFLDTXCD, _
                v_strRULETYPE, v_strMONTHDAY, v_strYEARDAY, v_strGLREF, v_strICTYPE, v_strICRATECD As String
            Dim v_dblMINBAL, v_dblMAXBAL As Double
            'Dim v_strICRULE As String
            Dim i, v_intVALUE As Integer
            Dim intIndexICCFBAL, intIndexICCFRATE, intIndexINTBAL, intIndexINTAMT As Integer
            Dim dblBALANCE, dblBASEDRATE, dblRATE, dblFRAMT, dblTOAMT, dblDELTA As Double
            Dim dblICBASEDRATE, dblICBASEDRATEMIN, dblICBASEDRATEMAX As Double
            Dim v_strVALEXP As String = ""
            Dim v_strVALUE As String = ""
            Dim v_strCMPCD, v_strOPERAND, v_strCMPEXP As String, v_blnRuleAllow, v_blnMinMax As Boolean, v_dblMinMaxValue As Double
            Dim v_objEval As New Evaluator

            'Lấy giá trị cực đại trả v�? trong phân trang
            v_strSQL = "SELECT COUNT(*) MAXROW FROM  " & ATTR_MODULE & "MAST"
            v_dsMAST = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            v_intMaxRow = v_dsMAST.Tables(0).Rows(0)("MAXROW")

            '1.Lấy các event đến hạn xử lý của phân hệ nghiệp vụ
            'Căn cứ vào ngày hiện tại để l�?c lấy các event thực hiện daily/monthly/yearly hoặc ngày xác định
            'Nếu là ngày xác định thì kiểm tra đã đến ngày đó chưa
            v_strSQL = "SELECT ICDEF.*, ENV.CRITERIA, ENV.LSTODR, ENV.TLTXCD, ENV.FLDTXCD, ENV.ACFLD, ENV.FLDKEY, ENV.TBLNAME " & ControlChars.CrLf _
                & "FROM ICCFTYPEDEF ICDEF, APPEVENTS ENV " & ControlChars.CrLf _
                & "WHERE ICDEF.MODCODE = ENV.MODCODE And ICDEF.EVENTCODE = ENV.EVENTCODE " & ControlChars.CrLf _
                & "AND TRIM(ICDEF.MODCODE)='" & ATTR_MODULE & "' AND ICDEF.DELTD<> 'Y' " & ControlChars.CrLf _
                & "AND ((ICDEF.PERIOD IN " & v_strPERIOD & ") " & ControlChars.CrLf _
                & "OR (ICDEF.PERIOD='S' AND TO_DATE(ICDEF.PERIODDAY || '/" & v_intCURRMONTH & "/" & v_intCURRYEAR & "','" & gc_FORMAT_DATE & "')>=" & ControlChars.CrLf _
                & "TO_DATE('" & v_strCURRDATE & "','" & gc_FORMAT_DATE & "') " & ControlChars.CrLf _
                & "AND TO_DATE(ICDEF.PERIODDAY || '/" & v_intCURRMONTH & "/" & v_intCURRYEAR & "','" & gc_FORMAT_DATE & "')<" & ControlChars.CrLf _
                & "TO_DATE('" & v_strNEXTDATE & "','" & gc_FORMAT_DATE & "'))) " & ControlChars.CrLf _
                & "AND TRIM(ENV.BCHMDL) = '" & v_strBATCHNAME & "'" & ControlChars.CrLf _
                & "ORDER BY ICDEF.MODCODE, ENV.LSTODR, ENV.EVENTCODE"
            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            If v_ds.Tables(0).Rows.Count > 0 Then
                For intEventCount As Integer = 0 To v_ds.Tables(0).Rows.Count - 1
                    '2. Với mỗi event, l�?c lấy các tài khoản thoả mãn yêu cầu xử lý
                    v_strCRITERIA = gf_CorrectStringField(v_ds.Tables(0).Rows(intEventCount)("CRITERIA"))
                    v_strEVENTCODE = gf_CorrectStringField(v_ds.Tables(0).Rows(intEventCount)("EVENTCODE"))
                    v_strRULETYPE = gf_CorrectStringField(v_ds.Tables(0).Rows(intEventCount)("RULETYPE"))
                    v_strTLTXCD = gf_CorrectStringField(v_ds.Tables(0).Rows(intEventCount)("TLTXCD"))
                    v_strFLDTXCD = gf_CorrectStringField(v_ds.Tables(0).Rows(intEventCount)("FLDTXCD"))
                    v_strMONTHDAY = gf_CorrectStringField(v_ds.Tables(0).Rows(intEventCount)("MONTHDAY"))
                    v_strYEARDAY = gf_CorrectStringField(v_ds.Tables(0).Rows(intEventCount)("YEARDAY"))
                    v_strACTYPE = gf_CorrectStringField(v_ds.Tables(0).Rows(intEventCount)("ACTYPE"))
                    v_strGLREF = gf_CorrectStringField(v_ds.Tables(0).Rows(intEventCount)("GLACCTNO"))
                    v_strICTYPE = gf_CorrectStringField(v_ds.Tables(0).Rows(intEventCount)("ICTYPE"))
                    v_strICRATECD = gf_CorrectStringField(v_ds.Tables(0).Rows(intEventCount)("ICRATECD"))
                    'Khoảng giá trị max, min cho giá trị tính lãi
                    v_dblMINBAL = gf_CorrectNumericField(v_ds.Tables(0).Rows(intEventCount)("MINVAL"))
                    v_dblMAXBAL = gf_CorrectNumericField(v_ds.Tables(0).Rows(intEventCount)("MAXVAL"))

                    'Xác định các giá trị lãi suất cơ bản của ICCF
                    dblICBASEDRATEMIN = -1   'Không giới hạn
                    dblICBASEDRATEMAX = -1   'Không giới hạn
                    If v_strICTYPE = "F" Then
                        'Mức cố định
                        dblICBASEDRATE = gf_CorrectNumericField(v_ds.Tables(0).Rows(intEventCount)("ICFLAT")).ToString
                    ElseIf v_strICTYPE = "P" Then
                        If v_strICRATECD = "S" Then
                            'Lãi suất cố định
                            dblICBASEDRATE = gf_CorrectNumericField(v_ds.Tables(0).Rows(intEventCount)("ICRATE"))
                        ElseIf v_strICRATECD = "F" Then
                            'Lãi suất đi�?u chỉnh (cộng thêm vào lãi suất cơ sở)
                            dblICBASEDRATE = gf_CorrectNumericField(v_ds.Tables(0).Rows(intEventCount)("ICRATE"))
                            'Lãi suất thả nổi
                            v_strVALUE = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(intEventCount)("ICRATEID")))
                            v_strSQL = "SELECT * FROM IRRATE WHERE STATUS='Y' AND RATEID='" & v_strVALUE & "'"
                            v_dsIRRATE = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                            If v_dsIRRATE.Tables(0).Rows.Count > 0 Then
                                dblICBASEDRATE = dblICBASEDRATE + gf_CorrectNumericField(v_dsIRRATE.Tables(0).Rows(0)("RATE"))
                                dblICBASEDRATEMIN = gf_CorrectNumericField(v_dsIRRATE.Tables(0).Rows(0)("FLRRATE"))
                                dblICBASEDRATEMAX = gf_CorrectNumericField(v_dsIRRATE.Tables(0).Rows(0)("CELRATE"))
                            Else
                                'Lãi suất thả nổi hết hiệu lực 
                                dblICBASEDRATE = 0
                            End If
                        End If
                    End If


                    'L�?c giá trị theo phân trang và bộ Fillter khai báo
                    Select Case ATTR_MODULE
                        Case SUB_SYSTEM_CA
                            'Phân hệ SE phải lấy thêm thông tin v�? chứng khoán
                            v_strSQL = "SELECT (CASE WHEN TYP.LINETIED='Y' THEN 0 ELSE 1 END ) * AF.TRADERATE TRADERATE , (CASE WHEN TYP.LINETIED='Y' THEN 0 ELSE 1 END ) *  AF.DEPORATE DEPORATE, (CASE WHEN TYP.LINETIED='Y' THEN 0 ELSE 1 END ) *  AF.MISCRATE MISCRATE,AF.BRATIO,(CASE WHEN TYP.LINETIED='Y' THEN 0 ELSE 1 END ) * AF.FEEBASE FEEBASE, INF.PARVALUE, SEINF.DEPOFEEUNIT, SEINF.DEPOFEELOT, SEINF.CURRPRICE, AF.MISCRATE, MST.* " & ControlChars.CrLf _
                                & "FROM AFMAST AF, AFTYPE TYP,(SELECT MOD.* FROM (SELECT ROWNUM INDEXROW," & ATTR_MODULE & "MAST.* FROM " & ATTR_MODULE & "MAST) MOD  WHERE 0=0  " & v_strBCHFillter & "  ) MST, SECURITIES_INFO SEINF, SBSECURITIES INF " & ControlChars.CrLf _
                                & "WHERE AF.ACCTNO=MST.AFACCTNO AND MST.ACTYPE='" & v_strACTYPE & "' " & ControlChars.CrLf _
                                & "AND MST.CODEID=SEINF.CODEID AND INF.CODEID=SEINF.CODEID AND AF.ACTYPE=TYP.ACTYPE"
                        Case Else
                            v_strSQL = "SELECT (CASE WHEN TYP.LINETIED='Y' THEN 0 ELSE 1 END ) * AF.TRADERATE TRADERATE , (CASE WHEN TYP.LINETIED='Y' THEN 0 ELSE 1 END ) *  AF.DEPORATE DEPORATE, (CASE WHEN TYP.LINETIED='Y' THEN 0 ELSE 1 END ) *  AF.MISCRATE MISCRATE,AF.BRATIO,(CASE WHEN TYP.LINETIED='Y' THEN 0 ELSE 1 END ) * AF.FEEBASE FEEBASE, MST.* " & ControlChars.CrLf _
                                & "FROM AFMAST AF,AFTYPE TYP ,(SELECT MOD.* FROM (SELECT ROWNUM INDEXROW," & ATTR_MODULE & "MAST.* FROM " & ATTR_MODULE & "MAST) MOD  WHERE 0=0 " & v_strBCHFillter & "  ) MST WHERE AF.ACCTNO=MST.AFACCTNO AND MST.ACTYPE='" & v_strACTYPE & "' AND AF.ACTYPE=TYP.ACTYPE "
                    End Select
                    If Len(Trim(v_strCRITERIA)) > 0 Then
                        v_strSQL = v_strSQL & " AND " & v_strCRITERIA
                    End If
                    v_dsMAST = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

                    If v_dsMAST.Tables(0).Rows.Count > 0 Then
                        '3. Với mỗi tài khoản thực hiện tính toán theo công thức ICCF
                        '3.1 Lấy danh sách các phép toán của Event
                        v_strSQL = "SELECT MST.* FROM ICCFTX MST WHERE MODCODE='" & ATTR_MODULE & "' AND EVENTCODE='" & v_strEVENTCODE & "'"
                        v_dsICCFTX = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                        If v_dsICCFTX.Tables(0).Rows.Count > 0 Then
                            'Dựng thông tin bảng ICCFTX
                            ReDim v_arrICCFTX(v_dsICCFTX.Tables(0).Rows.Count - 1)
                            For intTXCount As Integer = 0 To v_dsICCFTX.Tables(0).Rows.Count - 1 Step 1
                                Dim v_objICCFTX As New ICCFTX
                                v_objICCFTX.ICRULE = v_strEVENTCODE
                                v_objICCFTX.TXCD = Trim(gf_CorrectStringField(v_dsICCFTX.Tables(0).Rows(intTXCount)("TXCD")))
                                v_objICCFTX.SORU = Trim(gf_CorrectStringField(v_dsICCFTX.Tables(0).Rows(intTXCount)("SORU")))
                                v_objICCFTX.DATATYPE = Trim(gf_CorrectStringField(v_dsICCFTX.Tables(0).Rows(intTXCount)("DATATYPE")))
                                v_objICCFTX.REFNAME = Trim(gf_CorrectStringField(v_dsICCFTX.Tables(0).Rows(intTXCount)("REFNAME")))
                                v_objICCFTX.AMTEXP = Trim(gf_CorrectStringField(v_dsICCFTX.Tables(0).Rows(intTXCount)("AMTEXP")))
                                v_objICCFTX.CMPCD = Trim(gf_CorrectStringField(v_dsICCFTX.Tables(0).Rows(intTXCount)("CMPCD")))
                                v_objICCFTX.OPERAND = Trim(gf_CorrectStringField(v_dsICCFTX.Tables(0).Rows(intTXCount)("OPERAND")))
                                v_objICCFTX.CMPEXP = Trim(gf_CorrectStringField(v_dsICCFTX.Tables(0).Rows(intTXCount)("CMPEXP")))
                                v_arrICCFTX(intTXCount) = v_objICCFTX
                            Next

                            '3.2 Xử lý phép toán cho từng tài khoản
                            For intMastCount As Integer = 0 To v_dsMAST.Tables(0).Rows.Count - 1
#If DEBUG Then
                                'Log lại tài khoản để tiện debug
                                'LogError.Write("Source: " & v_strErrorSource & vbNewLine _
                                '    & "Error message: " & ATTR_MODULE & "." & v_strACTYPE & "." & v_strEVENTCODE & "." & intMastCount, EventLogEntryType.Information)
#End If
                                v_strSQL = "SELECT COUNT(*) FROM EXAFMAST WHERE STATUS='A' " & _
                                    "AND EVENTCODE='" & v_strEVENTCODE & "' " & _
                                    "AND AFACCTNO='" & Trim(gf_CorrectStringField(v_dsMAST.Tables(0).Rows(intMastCount)("AFACCTNO"))) & "'"
                                v_dsCHECK = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

                                If v_dsCHECK.Tables(0).Rows(0)(0) = 0 Then
                                    '4. Tạo giao dịch tương ứng
                                    '4.1 Tính toán các giá trị cho mảng ICCFTX
                                    intIndexICCFBAL = -1
                                    intIndexICCFRATE = -1
                                    intIndexINTBAL = -1
                                    dblBASEDRATE = 0
                                    For i = 0 To v_arrICCFTX.GetLength(0) - 1 Step 1
                                        If Not v_arrICCFTX(i) Is Nothing Then
                                            '4.1.1 Kiểm tra đi�?u kiện để thực hiện Rule có thoả mãn không
                                            v_blnRuleAllow = False
                                            v_blnMinMax = False 'Mặc định là không so sánh
                                            v_dblMinMaxValue = 0
                                            v_strCMPCD = v_arrICCFTX(i).CMPCD
                                            v_strOPERAND = v_arrICCFTX(i).OPERAND
                                            v_strCMPEXP = v_arrICCFTX(i).CMPEXP

                                            'Ghi nhận vị trí trư�?ng BALANCE/RATE
                                            If v_arrICCFTX(i).REFNAME = "ICCFBAL" Then intIndexICCFBAL = i
                                            If v_arrICCFTX(i).REFNAME = "ICCFRATE" Then intIndexICCFRATE = i
                                            'Ghi nhận vị trí trư�?ng INTAMT
                                            If v_arrICCFTX(i).REFNAME = "INTAMT" Then intIndexINTAMT = i
                                            'Ghi nhận vị trí trư�?ng biểu thức số trả v�?
                                            If v_arrICCFTX(i).TXCD = v_strFLDTXCD Then
                                                intIndexINTBAL = i
                                            End If

                                            If Len(v_strCMPCD) = 0 Or Len(v_strOPERAND) = 0 Or Len(v_strCMPEXP) = 0 Then
                                                v_blnRuleAllow = True
                                            Else
                                                v_strVALUE = BuildICCFTXEXP(v_arrICCFTX, v_strCMPCD)
                                                v_strCMPCD = v_strVALUE
                                                v_strVALUE = BuildICCFTXEXP(v_arrICCFTX, v_strCMPEXP)
                                                v_strCMPEXP = v_strVALUE
                                                'Kiểm tra
                                                v_blnRuleAllow = ICCFEvaluateChecking(v_strOPERAND, v_strCMPCD, v_strCMPEXP, v_blnMinMax, v_dblMinMaxValue)
                                            End If

                                            '4.1.2 Tính toán giá trị cho phần tử mảng v_arrICCFTX
                                            If Not v_blnRuleAllow Then
                                                'Nếu không thoả mãn đi�?u kiện lựa ch�?n thì lấy giá trị mặc định
                                                If v_arrICCFTX(i).DATATYPE = "N" Then
                                                    v_arrICCFTX(i).VALNUMBER = 0
                                                Else
                                                    v_arrICCFTX(i).VALCHAR = String.Empty
                                                End If
                                            Else
                                                If v_arrICCFTX(i).SORU = "S" Then
                                                    v_strVALUE = vbNullString
                                                    If v_arrICCFTX(i).REFNAME = "DAYS" Then
                                                        'Nếu REFNAME là DAYS, thì sẽ so sánh giữa ngày hiện tại và giá trị được chỉ định
                                                        'v_strVALEXP = Trim(Mid(v_arrICCFTX(i).AMTEXP, 2))
                                                        'v_strVALUE = Format(gf_CorrectDateField(v_dsMAST.Tables(0).Rows(intMastCount)(v_strVALEXP)), gc_FORMAT_DATE)
                                                        v_strVALUE = Format(DDMMYYYY_SystemDate(v_strICCFLASTDATE), gc_FORMAT_DATE)
                                                        v_lngErrCode = GetSDE_DAYS(v_strVALUE, v_strMONTHDAY, v_intVALUE)
                                                        v_strVALUE = v_intVALUE.ToString
                                                    Else
                                                        'Nếu là SDE.System Element Definition thì lấy giá trị luôn
                                                        '�?ối với SDE thì không cần quan tâm đến CMPCD. 
                                                        'Biểu thức đi�?u kiện chỉ sử dụng cho UDE.User Element Definition
                                                        v_strVALEXP = Trim(Mid(v_arrICCFTX(i).AMTEXP, 2))
                                                        If Len(v_arrICCFTX(i).AMTEXP) >= 2 Then
                                                            Select Case Mid(v_arrICCFTX(i).AMTEXP, 1, 1)
                                                                Case "$"
                                                                    If v_strVALEXP = "PERIOD" Then
                                                                        v_strVALUE = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(intEventCount)("PERIOD")))
                                                                    ElseIf v_strVALEXP = "RULETYPE" Then
                                                                        v_strVALUE = v_strRULETYPE
                                                                    ElseIf v_strVALEXP = "EVENTCODE" Then
                                                                        v_strVALUE = v_strEVENTCODE
                                                                    ElseIf v_strVALEXP = "GLREF" Then
                                                                        v_strVALUE = v_strGLREF
                                                                    ElseIf v_strVALEXP = "RATE" Then
                                                                        v_strVALUE = dblICBASEDRATE.ToString
                                                                    Else
                                                                        If v_strVALEXP = "MONTHDAY" Then
                                                                            'Lấy giá trị được xác định trong ICCFMAP
                                                                            v_strVALUE = v_strMONTHDAY
                                                                        ElseIf v_strVALEXP = "YEARDAY" Then
                                                                            'Lấy giá trị được xác định trong ICCFMAP
                                                                            v_strVALUE = v_strYEARDAY
                                                                        End If
                                                                        'Lấy dữ liệu SDE
                                                                        v_lngErrCode = GetSDEFormulaValue(v_strVALEXP, v_strVALUE)
                                                                        If v_lngErrCode <> ERR_SYSTEM_OK Then
                                                                            'ContextUtil.SetAbort()
                                                                            Return v_lngErrCode
                                                                        End If
                                                                    End If
                                                                Case "@"
                                                                    'Lấy dữ liệu trực tiếp
                                                                    v_strVALUE = v_strVALEXP
                                                                Case "#"
                                                                    'Lấy dữ liệu từ thông tin tài khoản
                                                                    If v_arrICCFTX(i).DATATYPE = "D" Then
                                                                        v_strVALUE = Format(gf_CorrectDateField(v_dsMAST.Tables(0).Rows(intMastCount)(v_strVALEXP)), gc_FORMAT_DATE)
                                                                    ElseIf v_arrICCFTX(i).DATATYPE = "N" Then
                                                                        v_strVALUE = Trim(gf_CorrectNumericField(v_dsMAST.Tables(0).Rows(intMastCount)(v_strVALEXP)))
                                                                    ElseIf v_arrICCFTX(i).DATATYPE = "C" Then
                                                                        v_strVALUE = Trim(gf_CorrectStringField(v_dsMAST.Tables(0).Rows(intMastCount)(v_strVALEXP)))
                                                                    End If
                                                                Case Else
                                                            End Select
                                                        End If
                                                    End If
                                                    'Gán giá trị trả v�?
                                                    If v_arrICCFTX(i).DATATYPE = "N" Then
                                                        v_arrICCFTX(i).VALNUMBER = Math.Round(CDbl(v_strVALUE), 4)
                                                    Else
                                                        v_arrICCFTX(i).VALCHAR = v_strVALUE
                                                    End If

                                                ElseIf v_arrICCFTX(i).SORU = "U" Then
                                                    'Biểu thức số h�?c: Chỉ sử dụng cho UDE
                                                    If v_blnMinMax Then
                                                        'Nếu là phép toán so sánh lấy trực tiếp giá trị
                                                        If v_arrICCFTX(i).DATATYPE = "N" Then
                                                            v_arrICCFTX(i).VALNUMBER = Math.Round(v_dblMinMaxValue, 4)
                                                        End If
                                                    Else
                                                        If v_arrICCFTX(i).DATATYPE = "N" Then
                                                            v_strVALEXP = BuildICCFTXEXP(v_arrICCFTX, v_arrICCFTX(i).AMTEXP)
                                                            v_arrICCFTX(i).VALNUMBER = Math.Round(CDbl(v_objEval.Eval(v_strVALEXP).ToString), 4)
                                                        End If
                                                    End If
                                                End If

                                            End If 'Của If Not v_blnRuleAllow Then

                                        End If 'Của If Not v_arrICCFTX(i) Is Nothing Then
                                    Next 'Của For intMastCount As Integer = 0 To v_dsMAST.Tables(0).Rows.Count - 1

                                    '4.2 Nạp giao dịch tương ứng trên cơ sở thông tin của mảng ICCFTX
                                    'Phân loại RULETYPE (Fixed/Tier/Cluster) để có cách xử lý tương ứng
                                    'Trư�?ng số dư được sử dụng để MAP là trư�?ng có AMTEXP=$ICCFBAL
                                    'Tạo giao dịch ICCF: Các giao dịch sinh ra từ ICCF khi khai báo FLDMASTER sẽ để DEFNAME=REFNAME của ICCFTX

                                    'Lấy các Tier của trư�?ng số để tính toán nếu có
                                    If Not (intIndexICCFBAL = -1 Or intIndexICCFRATE = -1 Or intIndexINTBAL = -1) Then
                                        v_strSQL = "SELECT MST.* FROM ICCFTIER MST WHERE MODCODE='" & ATTR_MODULE & "' AND ACTYPE='" & v_strACTYPE & "' AND EVENTCODE='" & v_strEVENTCODE & "'"
                                        v_dsTIER = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                                        If v_dsTIER.Tables(0).Rows.Count > 0 Then
                                            'Có định nghĩa các tier xử lý
                                            dblBALANCE = v_arrICCFTX(intIndexICCFBAL).VALNUMBER     'Số dư tính toán của tài khoản
                                            dblBASEDRATE = v_arrICCFTX(intIndexICCFRATE).VALNUMBER  'Lãi suất cơ sở
                                            For intTierCount As Integer = 0 To v_dsTIER.Tables(0).Rows.Count - 1 Step 1
                                                'Duyệt từng Tier để xử lý 
                                                dblFRAMT = gf_CorrectNumericField(v_dsTIER.Tables(0).Rows(intTierCount)("FRAMT"))
                                                dblTOAMT = gf_CorrectNumericField(v_dsTIER.Tables(0).Rows(intTierCount)("TOAMT"))
                                                dblDELTA = gf_CorrectNumericField(v_dsTIER.Tables(0).Rows(intTierCount)("DELTA"))
                                                Select Case v_strRULETYPE
                                                    Case "F"    'Fixed
                                                        'Lấy toàn bộ số dư
                                                        v_arrICCFTX(intIndexICCFBAL).VALNUMBER = Math.Round(dblBALANCE, 4)
                                                        v_arrICCFTX(intIndexICCFRATE).VALNUMBER = dblBASEDRATE + dblDELTA
                                                        'Tính toán lại thông tin lãi
                                                        v_strVALEXP = BuildICCFTXEXP(v_arrICCFTX, v_arrICCFTX(intIndexINTBAL).AMTEXP)
                                                        v_arrICCFTX(intIndexINTBAL).VALNUMBER = Math.Round(CDbl(v_objEval.Eval(v_strVALEXP).ToString), 4)
                                                        If v_strICTYPE = "P" And v_strICRATECD = "F" Then
                                                            'Nếu là lãi suất thả nổi phải kiểm tra lãi suất sàn và trần
                                                            If v_arrICCFTX(intIndexICCFRATE).VALNUMBER < dblICBASEDRATEMIN And dblICBASEDRATEMIN > 0 Then
                                                                v_arrICCFTX(intIndexICCFRATE).VALNUMBER = Math.Round(dblICBASEDRATEMIN, 4)
                                                            ElseIf v_arrICCFTX(intIndexICCFRATE).VALNUMBER > dblICBASEDRATEMAX And dblICBASEDRATEMAX > 0 Then
                                                                v_arrICCFTX(intIndexICCFRATE).VALNUMBER = Math.Round(dblICBASEDRATEMAX, 4)
                                                            End If
                                                        End If
                                                        'Tạo giao dịch
                                                        'Với kiểu fixed thì chỉ tính lãi một lần, theo lãi suất của tier đầu tiên là cộng 0

                                                        ICCFBuildTransact(v_arrICCFTX, v_strTLTXCD, v_strFLDTXCD, v_strBATCHNAME, v_dblMINBAL, v_dblMAXBAL)
                                                        Exit For
                                                    Case "T"    'Tier
                                                        'Chỉ có một mức lãi suất chung cho số dư tính lãi
                                                        dblRATE = 0
                                                        If dblFRAMT <= 0 And dblBALANCE <= dblTOAMT Then  'Không xác định mức sàn cho số dư
                                                            dblRATE = dblBASEDRATE + dblDELTA
                                                        ElseIf dblFRAMT < dblBALANCE And dblTOAMT <= 0 Then   'Không xác định mức trần cho số dư
                                                            dblRATE = dblBASEDRATE + dblDELTA
                                                        ElseIf dblFRAMT <= 0 And dblTOAMT <= 0 Then   'Không xác định mức trần/sàn cho số dư
                                                            dblRATE = dblBASEDRATE + dblDELTA
                                                        Else    'Xử lý cho dblFRAMT > dblTOAMT > 0 
                                                            If dblTOAMT >= dblBALANCE And dblBALANCE > dblFRAMT Then
                                                                dblRATE = dblBASEDRATE + dblDELTA
                                                            End If
                                                        End If
                                                        If dblRATE > 0 Then
                                                            v_arrICCFTX(intIndexICCFRATE).VALNUMBER = dblRATE
                                                            v_arrICCFTX(intIndexICCFBAL).VALNUMBER = Math.Round(dblBALANCE, 4)
                                                            If v_strICTYPE = "P" And v_strICRATECD = "F" Then
                                                                'Nếu là lãi suất thả nổi phải kiểm tra lãi suất sàn và trần
                                                                If v_arrICCFTX(intIndexICCFRATE).VALNUMBER < dblICBASEDRATEMIN And dblICBASEDRATEMIN > 0 Then
                                                                    v_arrICCFTX(intIndexICCFRATE).VALNUMBER = Math.Round(dblICBASEDRATEMIN, 4)
                                                                ElseIf v_arrICCFTX(intIndexICCFRATE).VALNUMBER > dblICBASEDRATEMAX And dblICBASEDRATEMAX > 0 Then
                                                                    v_arrICCFTX(intIndexICCFRATE).VALNUMBER = Math.Round(dblICBASEDRATEMAX, 4)
                                                                End If
                                                            End If
                                                            'Tính toán lại thông tin lãi
                                                            v_strVALEXP = BuildICCFTXEXP(v_arrICCFTX, v_arrICCFTX(intIndexINTBAL).AMTEXP)
                                                            v_arrICCFTX(intIndexINTBAL).VALNUMBER = Math.Round(CDbl(v_objEval.Eval(v_strVALEXP).ToString), 4)
                                                            'Tạo giao dịch
                                                            ICCFBuildTransact(v_arrICCFTX, v_strTLTXCD, v_strFLDTXCD, v_strBATCHNAME, v_dblMINBAL, v_dblMAXBAL)
                                                        End If
                                                    Case "C"    'Cluster
                                                        'Mỗi Tier có một mức lãi suất khác nhau
                                                        dblRATE = dblBASEDRATE + dblDELTA
                                                        If dblFRAMT <= 0 Then   'Không xác định mức sàn cho số dư
                                                            If dblTOAMT >= dblBALANCE Then
                                                                v_arrICCFTX(intIndexICCFBAL).VALNUMBER = Math.Round(dblBALANCE, 4)
                                                            Else
                                                                v_arrICCFTX(intIndexICCFBAL).VALNUMBER = Math.Round(dblTOAMT, 4)
                                                            End If
                                                        ElseIf dblTOAMT <= 0 Then   'Không xác định mức trần cho số dư
                                                            If dblBALANCE > dblFRAMT Then
                                                                v_arrICCFTX(intIndexICCFBAL).VALNUMBER = Math.Round(dblBALANCE - dblFRAMT, 4)
                                                            Else
                                                                v_arrICCFTX(intIndexICCFBAL).VALNUMBER = 0
                                                            End If
                                                        Else    'Xử lý cho dblFRAMT > dblTOAMT > 0 
                                                            If dblTOAMT >= dblBALANCE And dblBALANCE > dblFRAMT Then
                                                                v_arrICCFTX(intIndexICCFBAL).VALNUMBER = Math.Round(dblBALANCE - dblFRAMT, 4)
                                                            Else
                                                                'v_arrICCFTX(intIndexICCFBAL).VALNUMBER = dblTOAMT - dblFRAMT
                                                                '----------------------------
                                                                'Sửa thay dòng code bị đóng ở trên
                                                                'Vi trư�?ng hợp trên chưa tính cho trư�?ng hợp dblBALANCE <= dblFRAMT
                                                                If dblBALANCE <= dblFRAMT Then
                                                                    v_arrICCFTX(intIndexICCFBAL).VALNUMBER = 0
                                                                Else
                                                                    v_arrICCFTX(intIndexICCFBAL).VALNUMBER = Math.Round(dblTOAMT - dblFRAMT, 4)
                                                                End If
                                                                '------------------------------
                                                            End If
                                                        End If
                                                        If dblRATE > 0 And v_arrICCFTX(intIndexICCFBAL).VALNUMBER > 0 Then
                                                            v_arrICCFTX(intIndexICCFRATE).VALNUMBER = dblRATE
                                                            If v_strICTYPE = "P" And v_strICRATECD = "F" Then
                                                                'Nếu là lãi suất thả nổi phải kiểm tra lãi suất sàn và trần
                                                                If v_arrICCFTX(intIndexICCFRATE).VALNUMBER < dblICBASEDRATEMIN And dblICBASEDRATEMIN > 0 Then
                                                                    v_arrICCFTX(intIndexICCFRATE).VALNUMBER = dblICBASEDRATEMIN
                                                                ElseIf v_arrICCFTX(intIndexICCFRATE).VALNUMBER > dblICBASEDRATEMAX And dblICBASEDRATEMAX > 0 Then
                                                                    v_arrICCFTX(intIndexICCFRATE).VALNUMBER = dblICBASEDRATEMAX
                                                                End If
                                                            End If
                                                            'Tính toán lại thông tin lãi
                                                            v_strVALEXP = BuildICCFTXEXP(v_arrICCFTX, v_arrICCFTX(intIndexINTBAL).AMTEXP)
                                                            v_arrICCFTX(intIndexINTBAL).VALNUMBER = Math.Round(CDbl(v_objEval.Eval(v_strVALEXP).ToString), 4)
                                                            'Tạo giao dịch
                                                            ICCFBuildTransact(v_arrICCFTX, v_strTLTXCD, v_strFLDTXCD, v_strBATCHNAME, v_dblMINBAL, v_dblMAXBAL)
                                                        End If
                                                End Select
                                            Next
                                        Else
                                            If v_strICTYPE = "P" And v_strICRATECD = "F" Then
                                                'Nếu là lãi suất thả nổi phải kiểm tra lãi suất sàn và trần
                                                If v_arrICCFTX(intIndexICCFRATE).VALNUMBER < dblICBASEDRATEMIN And dblICBASEDRATEMIN > 0 Then
                                                    v_arrICCFTX(intIndexICCFRATE).VALNUMBER = dblICBASEDRATEMIN
                                                ElseIf v_arrICCFTX(intIndexICCFRATE).VALNUMBER > dblICBASEDRATEMAX And dblICBASEDRATEMAX > 0 Then
                                                    v_arrICCFTX(intIndexICCFRATE).VALNUMBER = dblICBASEDRATEMAX
                                                End If
                                            End If
                                            'Không định nghĩa các tier xử lý
                                            ICCFBuildTransact(v_arrICCFTX, v_strTLTXCD, v_strFLDTXCD, v_strBATCHNAME, v_dblMINBAL, v_dblMAXBAL)
                                        End If 'Của If v_dsTIER.Tables(0).Rows.Count > 0 Then
                                    End If 'Của If Not (intIndexICCFBAL = -1 Or intIndexICCFRATE = -1 Or intIndexINTBAL = -1) Then
                                End If ' If Not v_dsCHECK.Tables(0).Rows(0)(0) = 0 Then
                            Next 'Của For intMastCount As Integer = 0 To v_dsMAST.Tables(0).Rows.Count - 1
                        End If 'Của If v_dsICCFTX.Tables(0).Rows.Count > 0 Then

                    End If 'Của If v_dsMAST.Tables(0).Rows.Count > 0 Then

                Next 'Của For intEventCount As Integer = 0 To v_ds.Tables(0).Rows.Count - 1
            End If 'Của If v_ds.Tables(0).Rows.Count > 0 Then


            'ContextUtil.SetComplete()
            Return v_lngErrCode
        Catch ex As Exception
            'ContextUtil.SetAbort()
            'LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
            '    & "Error message: " & v_strSQL & ControlChars.CrLf & ex.Message, EventLogEntryType.Error)
            Throw ex
        End Try

    End Function

    'Hàm này xây dựng để thực hiện kiểm tra phần tử ICCF có thoả mãn đi�?u kiện không
    Private Function ICCFEvaluateChecking(ByVal v_strOPERAND As String, ByVal v_strCMPCD As String, ByVal v_strCMPEXP As String, _
        ByRef v_blnMinMax As Boolean, ByRef v_dblMinMaxValue As Double) As Boolean
        Dim v_objEval As New Evaluator, v_blnRuleAllow As Boolean = False
        Select Case v_strOPERAND
            Case ">>"
                If CDbl(v_objEval.Eval(v_strCMPCD).ToString) > CDbl(v_objEval.Eval(v_strCMPEXP).ToString) Then
                    v_blnRuleAllow = True
                End If
            Case ">="
                If CDbl(v_objEval.Eval(v_strCMPCD).ToString) >= CDbl(v_objEval.Eval(v_strCMPEXP).ToString) Then
                    v_blnRuleAllow = True
                End If
            Case "<<"
                If CDbl(v_objEval.Eval(v_strCMPCD).ToString) < CDbl(v_objEval.Eval(v_strCMPEXP).ToString) Then

                End If
            Case "<="
                If CDbl(v_objEval.Eval(v_strCMPCD).ToString) <= CDbl(v_objEval.Eval(v_strCMPEXP).ToString) Then
                    v_blnRuleAllow = True
                End If
            Case "=="
                If CDbl(v_objEval.Eval(v_strCMPCD).ToString) = CDbl(v_objEval.Eval(v_strCMPEXP).ToString) Then
                    v_blnRuleAllow = True
                End If
            Case "<>"
                If CDbl(v_objEval.Eval(v_strCMPCD).ToString) <> CDbl(v_objEval.Eval(v_strCMPEXP).ToString) Then
                    v_blnRuleAllow = True
                End If
            Case "IN"
                If InStr(v_strCMPEXP, v_strCMPCD) > 0 Then
                    v_blnRuleAllow = True
                End If
            Case "NI"
                If Not InStr(v_strCMPEXP, v_strCMPCD) > 0 Then
                    v_blnRuleAllow = True
                End If
            Case "MI"
                'Trư�?ng hợp này trả v�? giá trị MIN và Allow
                v_blnRuleAllow = True
                v_blnMinMax = True
                If CDbl(v_objEval.Eval(v_strCMPCD).ToString) > CDbl(v_objEval.Eval(v_strCMPEXP).ToString) Then
                    v_dblMinMaxValue = CDbl(v_objEval.Eval(v_strCMPEXP).ToString)
                Else
                    v_dblMinMaxValue = CDbl(v_objEval.Eval(v_strCMPCD).ToString)
                End If
            Case "MA"
                'Trư�?ng hợp này trả v�? giá trị MAX và Allow
                v_blnRuleAllow = True
                v_blnMinMax = True
                If Not CDbl(v_objEval.Eval(v_strCMPCD).ToString) > CDbl(v_objEval.Eval(v_strCMPEXP).ToString) Then
                    v_dblMinMaxValue = CDbl(v_objEval.Eval(v_strCMPEXP).ToString)
                Else
                    v_dblMinMaxValue = CDbl(v_objEval.Eval(v_strCMPCD).ToString)
                End If
            Case Else
        End Select
        Return v_blnRuleAllow
    End Function

    'Hàm này xây dựng giao dịch theo luật ICCF căn cứ thông tin của mảng ICCFTX
    Private Function ICCFBuildTransact(ByVal v_arrICCFTX() As ICCFTX, _
        ByVal v_strTLTXCD As String, ByVal v_strFLDTXCD As String, ByVal v_strBATCHNAME As String, ByVal v_dblMINBAL As Double, ByVal v_dblMAXBAL As Double) As Long
        Dim v_lngErrCode As Long = ERR_SYSTEM_OK
        Dim v_strErrorSource As String = ATTR_MODULE & ".Batch.ICCFBuildTransact." & v_strBATCHNAME & "." & v_strTLTXCD
        Dim v_strSQL, v_strVALUE, v_strDEFNAME, v_strFLDNAME, v_strFLDTYPE, v_strFLDACCT, v_strDESC, v_strINTAMT As String, v_dsTLLOG As DataSet
        Dim v_dblINTAMT As Double
        Dim v_obj As New DataAccess, v_objMessageLog As New MessageLog
        v_objMessageLog.NewDBInstance(gc_MODULE_HOST)
        Dim v_xmlDocument As New Xml.XmlDocument, v_dataElement As Xml.XmlElement, v_entryNode As Xml.XmlNode
        Dim v_attrFLDNAME, v_attrDATATYPE As Xml.XmlAttribute
        Dim v_strACCTNO As String
        Try
            v_strVALUE = GetICCFTXValueByTXCD(v_arrICCFTX, v_strFLDTXCD)
            'Nếu có trư�?ng INTAMT thì so sánh giá trị trư�?ng này với giá trị max, min.
            '   Nếu lớn hơn Max thì lấy max
            '   Nếu nh�? hơn Min thì lấy Min
            v_strINTAMT = GetICCFTXValueByREFNAME(v_arrICCFTX, "INTAMT")
            If v_strINTAMT <> String.Empty Then
                If IsNumeric(v_strINTAMT) Then
                    v_dblINTAMT = CDbl(v_strINTAMT)
                    If v_dblINTAMT < v_dblMINBAL Then v_dblINTAMT = v_dblMINBAL
                    If v_dblINTAMT > v_dblMAXBAL Then v_dblINTAMT = v_dblMAXBAL
                    For i As Integer = 0 To v_arrICCFTX.GetLength(0) - 1 Step 1
                        If Not v_arrICCFTX(i) Is Nothing Then
                            If Trim(v_arrICCFTX(i).REFNAME) = Trim("INTAMT") Then
                                If v_arrICCFTX(i).DATATYPE = "N" Then
                                    v_arrICCFTX(i).VALNUMBER = v_dblINTAMT
                                Else
                                    v_arrICCFTX(i).VALCHAR = v_strINTAMT
                                End If
                            End If
                        End If
                    Next
                End If
            End If

            If IsNumeric(v_strVALUE) Then
                If CDbl(v_strVALUE) > 0 Then
                    'Tạo connection đến HOST
                    v_obj.NewDBInstance(gc_MODULE_HOST)
                    v_lngErrCode = BuildBatchTxMsg(v_xmlDocument, v_strBATCHNAME)
                    v_xmlDocument.DocumentElement.Attributes(gc_AtributeTLTXCD).Value = v_strTLTXCD

                    v_strSQL = "SELECT FLDMASTER.FLDNAME, FLDMASTER.FLDTYPE, FLDMASTER.DEFNAME, TLTX.EN_TXDESC TLTXDESC, TLTX.MSG_ACCT  " & ControlChars.CrLf _
                        & "FROM FLDMASTER, TLTX WHERE TRIM(TLTX.TLTXCD)=TRIM(OBJNAME) AND TRIM(OBJNAME)='" & v_strTLTXCD & "' ORDER BY ODRNUM"
                    v_dsTLLOG = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                    If v_dsTLLOG.Tables(0).Rows.Count > 0 Then
                        v_dataElement = v_xmlDocument.CreateElement(Xml.XmlNodeType.Element, "fields", "")
                        'Tạo phần nội dung của giao dịch
                        For j As Integer = 0 To v_dsTLLOG.Tables(0).Rows.Count - 1 Step 1
                            v_strDEFNAME = Trim(gf_CorrectStringField(v_dsTLLOG.Tables(0).Rows(j)("DEFNAME")))
                            v_strFLDNAME = Trim(gf_CorrectStringField(v_dsTLLOG.Tables(0).Rows(j)("FLDNAME")))
                            v_strFLDTYPE = Trim(gf_CorrectStringField(v_dsTLLOG.Tables(0).Rows(j)("FLDTYPE")))
                            v_strFLDACCT = Trim(gf_CorrectStringField(v_dsTLLOG.Tables(0).Rows(j)("MSG_ACCT")))
                            v_strDESC = Trim(gf_CorrectStringField(v_dsTLLOG.Tables(0).Rows(j)("TLTXDESC")))
                            v_entryNode = v_xmlDocument.CreateNode(Xml.XmlNodeType.Element, "entry", "")
                            'Add field name
                            v_attrFLDNAME = v_xmlDocument.CreateAttribute(gc_AtributeFLDNAME)
                            v_attrFLDNAME.Value = v_strFLDNAME
                            v_entryNode.Attributes.Append(v_attrFLDNAME)

                            'Add field type
                            v_attrDATATYPE = v_xmlDocument.CreateAttribute(gc_AtributeFLDTYPE)
                            v_attrDATATYPE.Value = v_strFLDTYPE
                            v_entryNode.Attributes.Append(v_attrDATATYPE)

                            'Set value
                            v_entryNode.InnerText = GetICCFTXValueByREFNAME(v_arrICCFTX, v_strDEFNAME)
                            If Len(Trim(v_entryNode.InnerText)) = 0 And v_strFLDNAME = "DESC" Then
                                v_entryNode.InnerText = v_strDESC
                            End If

                            v_dataElement.AppendChild(v_entryNode)
                            If v_strFLDNAME = v_strFLDACCT Then
                                v_strACCTNO = GetICCFTXValueByREFNAME(v_arrICCFTX, v_strDEFNAME)
                                'Modified by MinhTK, 17-Apr-07: Khach hang cua chi nhanh nao thi GD phai thuoc chi nhanh do
                                v_xmlDocument.DocumentElement.Attributes(gc_AtributeBRID).Value = v_strACCTNO.Substring(0, 4)
                                'End of modified by MinhTK, 7-Apr-07
                            End If
                        Next
                        v_xmlDocument.DocumentElement.AppendChild(v_dataElement)

                        'Xu ly dac biet cho phan he CI,SE
                        Select Case v_strTLTXCD
                            Case "1160", "1161" ', "1166" 'Lai cong don, lai thau chi,khau tru phi
                                v_lngErrCode = GenCIIntTrans(v_xmlDocument)
                                'Case "2260"
                                '    v_lngErrCode = GenSECostPrice(v_xmlDocument)
                                'Case "2261" 'So du luu ky cong don
                                '    v_lngErrCode = GenSEDepositoryBalance(v_xmlDocument)
                            Case Else 'Neu khong
                                'Ghi nhận giao dịch vào TLLOG
                                v_lngErrCode = v_objMessageLog.TransLog(v_xmlDocument)
                        End Select
                        'Tra loi
                        If v_lngErrCode <> ERR_SYSTEM_OK Then
                            ''ContextUtil.SetAbort()
                            Return v_lngErrCode
                        End If
                    End If
                End If 'Của If CDbl(v_strVALUE) > 0 Then
            End If 'Của If IsNumeric(v_strVALUE) Then
            'ContextUtil.SetComplete()
            Return v_lngErrCode
        Catch ex As Exception
            'ContextUtil.SetAbort()
            'LogError.Write("Error source: " & v_strErrorSource & vbNewLine & "Error code: System error!" & vbNewLine & "Error message: " & ex.Message, EventLogEntryType.Error)
            Throw ex
        End Try
    End Function

    'Hàm này cần được các phân hệ nghiệp vụ override để implement cụ thể
    Overridable Function ExecuteRouter(ByVal v_strBCHMDL As String, Optional ByVal v_strBCHFillter As String = "", Optional ByRef v_intMaxRow As Integer = 0) As Long
        Return ERR_SYSTEM_OK
        'ContextUtil.SetComplete()
    End Function

    'Hàm này thực hiện tình ra ngày cách ngày pv_dtFromday là pv_intNum theo loại lịch pv_strCarType
    'pv_strCarType có giá trị truy�?n vào là "B" hoặc "N"
    Private Function DateCalculate(ByVal pv_intNum As Integer, ByVal pv_dtFromday As Date, ByVal pv_strCarType As String) As Date
        Dim v_lngErrCode As Long = ERR_SYSTEM_OK
        Dim v_strSQL As String, v_intWITHHOLIDAY, v_intWITHOUTHOLIDAY As Integer, v_dtSBDATE As Date
        Dim v_obj As New DataAccess, v_ds As New DataSet
        v_obj.NewDBInstance(gc_MODULE_HOST)
        v_strSQL = "select SUM(CASE WHEN CLR1.HOLIDAY='Y' THEN 0 ELSE 1 END) WITHHOLIDAY,SUM(CASE WHEN CLR1.HOLIDAY='Y' THEN 1 ELSE 1 END) WITHOUTHOLIDAY,MAX(SBDATE) SBDATE from SBCLDR CLR1 where CLR1.CLDRTYPE='000' AND CLR1.SBDATE>= '" & pv_dtFromday & "' AND CLR1.SBDATE< TO_DATE('" & pv_dtFromday & "','" & gc_FORMAT_DATE & "') +" & pv_intNum
        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
        If v_ds.Tables(0).Rows.Count > 0 Then
            v_intWITHHOLIDAY = gf_CorrectNumericField(v_ds.Tables(0).Rows(0)("WITHHOLIDAY"))
            v_intWITHOUTHOLIDAY = gf_CorrectNumericField(v_ds.Tables(0).Rows(0)("WITHOUTHOLIDAY"))
            v_dtSBDATE = gf_CorrectDateField(v_ds.Tables(0).Rows(0)("SBDATE"))
            If pv_dtFromday = "N" Then
                Return v_dtSBDATE
            Else
                If v_intWITHHOLIDAY < pv_intNum Then
                    Return DateCalculate(pv_intNum - v_intWITHHOLIDAY, v_dtSBDATE, "B")
                Else
                    Return v_dtSBDATE
                End If
            End If
        End If
        Return DDMMYYYY_SystemDate(gc_NULL_DATE)
    End Function

    Private Function GenCIIntTrans(ByRef pv_xmlDocument As Xml.XmlDocument) As Long
        Dim v_lngErrCode As Long = ERR_SYSTEM_OK
        Dim v_strErrorSource As String = "CoreBusiness.Batch.GenCIIntTrans"

        Try
            Dim v_nodeList As Xml.XmlNodeList
            Dim v_strSQL, v_strFLDCD, v_strFLDTYPE, v_strVALUE As String, v_dblVALUE As Double, i As Integer
            Dim v_strACCTNO As String = ""
            Dim v_strINTTYPE As String = ""
            Dim v_strFRDATE As String = ""
            Dim v_strTODATE As String = ""
            Dim v_strICCFCD As String = ""

            Dim v_dblBALANCE, v_dblINTAMT, v_dblRATE As Double
            Dim v_obj As New DataAccess
            v_obj.NewDBInstance(gc_MODULE_HOST)
            '�?�?c nội dung giao dịch tính lãi cộng dồn: 1160
            v_nodeList = pv_xmlDocument.SelectNodes("/TransactMessage/fields/entry")
            For i = 0 To v_nodeList.Count - 1
                With v_nodeList.Item(i)
                    v_strFLDCD = Trim(.Attributes(gc_AtributeFLDNAME).Value.ToString)
                    v_strFLDTYPE = Trim(.Attributes(gc_AtributeFLDTYPE).Value.ToString)
                    If v_strFLDTYPE = "N" Then
                        v_strVALUE = vbNullString
                        v_dblVALUE = IIf(IsNumeric(.InnerText), CDbl(.InnerText), 0)
                    Else
                        v_strVALUE = Trim(.InnerText)
                        v_dblVALUE = 0
                    End If

                    Select Case v_strFLDCD
                        Case "03" 'ACCTNO
                            v_strACCTNO = v_strVALUE
                        Case "05" 'FRDATE
                            v_strFRDATE = v_strVALUE
                        Case "06" 'TODATE
                            v_strTODATE = v_strVALUE
                        Case "10" 'BALANCE
                            v_dblBALANCE = v_dblVALUE
                        Case "11" 'INTAMT
                            v_dblINTAMT = Math.Round(v_dblVALUE, 4)
                        Case "12" 'RATE
                            v_dblRATE = v_dblVALUE
                        Case "04" 'ICRULE
                            v_strICCFCD = v_strVALUE
                        Case "07" 'INTTYPE
                            v_strINTTYPE = v_strVALUE
                    End Select
                End With
            Next

            'Tạo phiếu tính lãi
            v_strSQL = "INSERT INTO CIINTTRAN (AUTOID, ACCTNO, INTTYPE, FRDATE, TODATE, ICRULE, IRRATE, INTBAL, INTAMT) " & ControlChars.CrLf _
                & "VALUES (SEQ_CIINTTRAN.NEXTVAL,'" & v_strACCTNO & "','" & v_strINTTYPE & "', " & ControlChars.CrLf _
                & "TO_DATE('" & v_strFRDATE & "', '" & gc_FORMAT_DATE & "'), " & ControlChars.CrLf _
                & "TO_DATE('" & v_strTODATE & "', '" & gc_FORMAT_DATE & "'), " & ControlChars.CrLf _
                & "'" & v_strICCFCD & "'," & v_dblRATE & "," & v_dblBALANCE & "," & v_dblINTAMT & ")"
            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
            'Cap nhat lai lai
            Select Case v_strINTTYPE
                Case "CR"
                    v_strSQL = "UPDATE CIMAST SET CRINTACR=CRINTACR+" & v_dblINTAMT & " WHERE ACCTNO='" & v_strACCTNO & "'"
                    v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                Case "OD"
                    v_strSQL = "UPDATE CIMAST SET ODINTACR=ODINTACR+" & v_dblINTAMT & " WHERE ACCTNO='" & v_strACCTNO & "'"
                    v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
            End Select
            'ContextUtil.SetComplete()
            Return v_lngErrCode
        Catch ex As Exception
            ''ContextUtil.SetAbort()
            'LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
            '             & "Error code: System error!" & vbNewLine _
            '             & "Error message: " & ex.Message, EventLogEntryType.Error)
            Throw ex
        End Try
    End Function

    Private Function GenSEDepositoryBalance(ByRef pv_xmlDocument As Xml.XmlDocument) As Long
        Dim v_lngErrCode As Long = ERR_SYSTEM_OK
        Dim v_strErrorSource As String = "CoreBusiness.Batch.GenSEDepositoryBalance"

        Try
            Dim v_nodeList As Xml.XmlNodeList
            Dim v_strSQL, v_strFLDCD, v_strFLDTYPE, v_strVALUE As String, v_dblVALUE As Double, i As Integer
            Dim v_strACCTNO As String = ""
            Dim v_strTXDATE As String = ""
            Dim v_dblDAYS, v_dblQTTY As Double

            Dim v_obj As New DataAccess

            v_obj.NewDBInstance(gc_MODULE_HOST)
            v_strTXDATE = pv_xmlDocument.DocumentElement.Attributes(gc_AtributeTXDATE).Value
            '�?�?c nội dung giao dịch so du luu ky cộng dồn: 2261
            v_nodeList = pv_xmlDocument.SelectNodes("/TransactMessage/fields/entry")
            For i = 0 To v_nodeList.Count - 1
                With v_nodeList.Item(i)
                    v_strFLDCD = Trim(.Attributes(gc_AtributeFLDNAME).Value.ToString)
                    v_strFLDTYPE = Trim(.Attributes(gc_AtributeFLDTYPE).Value.ToString)
                    If v_strFLDTYPE = "N" Then
                        v_strVALUE = vbNullString
                        v_dblVALUE = IIf(IsNumeric(.InnerText), CDbl(.InnerText), 0)
                    Else
                        v_strVALUE = Trim(.InnerText)
                        v_dblVALUE = 0
                    End If

                    Select Case v_strFLDCD
                        Case "03" 'ACCTNO
                            v_strACCTNO = v_strVALUE
                        Case "04" 'DAYS
                            v_dblDAYS = v_dblVALUE
                        Case "10" 'QTTY
                            v_dblQTTY = v_dblVALUE
                    End Select
                End With
            Next

            'Ghi lai thay doi so du luu ky cong don
            v_strSQL = "INSERT INTO SEDEPOBAL (AUTOID, ACCTNO, TXDATE, DAYS, QTTY, DELTD) " & ControlChars.CrLf _
                & "VALUES (SEQ_SEDEPOBAL.NEXTVAL,'" & v_strACCTNO & "', " & ControlChars.CrLf _
                & "TO_DATE('" & v_strTXDATE & "', '" & gc_FORMAT_DATE & "'), " & ControlChars.CrLf _
                & v_dblDAYS & "," & v_dblQTTY & ",'N')"
            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

            'Cap nhat so du lu ky cong don
            v_strSQL = "UPDATE SEMAST SET TBALDEPO=TBALDEPO+" & v_dblQTTY * v_dblDAYS & ControlChars.CrLf _
                & " ,LASTDATE=TO_DATE('" & v_strTXDATE & "', '" & gc_FORMAT_DATE & "'),TBALDT=TO_DATE('" & v_strTXDATE & "', '" & gc_FORMAT_DATE & "')" & ControlChars.CrLf _
                & " WHERE ACCTNO='" & v_strACCTNO & "'"
            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

            'ContextUtil.SetComplete()
            Return v_lngErrCode
        Catch ex As Exception
            'ContextUtil.SetAbort()
            'LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
            '             & "Error code: System error!" & vbNewLine _
            '             & "Error message: " & ex.Message, EventLogEntryType.Error)
            Throw ex
        End Try
    End Function

    Private Function GenSECostPrice(ByRef pv_xmlDocument As Xml.XmlDocument) As Long
        Dim v_lngErrCode As Long = ERR_SYSTEM_OK
        Dim v_strErrorSource As String = "CoreBusiness.Batch.GenSECostPrice"

        Try
            Dim v_nodeList As Xml.XmlNodeList
            Dim v_strSQL, v_strFLDCD, v_strFLDTYPE, v_strVALUE As String, v_dblVALUE As Double, i As Integer
            Dim v_strACCTNO As String = ""
            Dim v_strTXDATE As String = ""

            Dim v_dblCOSTPRICE, v_dblPREVCOSTPRICE, v_dblDCRAMT, v_dblDCRQTTY As Double

            Dim v_obj As New DataAccess

            v_obj.NewDBInstance(gc_MODULE_HOST)
            v_strTXDATE = pv_xmlDocument.DocumentElement.Attributes(gc_AtributeTXDATE).Value
            '�?�?c nội dung giao dịch tính gia von: 2260
            v_nodeList = pv_xmlDocument.SelectNodes("/TransactMessage/fields/entry")
            For i = 0 To v_nodeList.Count - 1
                With v_nodeList.Item(i)
                    v_strFLDCD = Trim(.Attributes(gc_AtributeFLDNAME).Value.ToString)
                    v_strFLDTYPE = Trim(.Attributes(gc_AtributeFLDTYPE).Value.ToString)
                    If v_strFLDTYPE = "N" Then
                        v_strVALUE = vbNullString
                        v_dblVALUE = IIf(IsNumeric(.InnerText), CDbl(.InnerText), 0)
                    Else
                        v_strVALUE = Trim(.InnerText)
                        v_dblVALUE = 0
                    End If

                    Select Case v_strFLDCD
                        Case "03" 'ACCTNO
                            v_strACCTNO = v_strVALUE
                        Case "09" 'OLDCOST
                            v_dblPREVCOSTPRICE = v_dblVALUE
                        Case "10" 'COSTPRICE
                            v_dblCOSTPRICE = v_dblVALUE
                        Case "12" 'DCRQTTY
                            v_dblDCRQTTY = v_dblVALUE
                        Case "13" 'DCRAMT
                            v_dblDCRAMT = v_dblVALUE
                    End Select
                End With
            Next

            'Ghi nhan gia von thay doi
            v_strSQL = "INSERT INTO SECOSTPRICE (AUTOID, ACCTNO, TXDATE, COSTPRICE, PREVCOSTPRICE, DCRAMT, DCRQTTY, DELTD) " & ControlChars.CrLf _
                & "VALUES (SEQ_SECOSTPRICE.NEXTVAL,'" & v_strACCTNO & "', " & ControlChars.CrLf _
                & "TO_DATE('" & v_strTXDATE & "', '" & gc_FORMAT_DATE & "'), " & ControlChars.CrLf _
                & v_dblCOSTPRICE & "," & v_dblPREVCOSTPRICE & "," & v_dblDCRAMT & "," & v_dblDCRQTTY & ",'N')"
            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

            'Cap nhat thong tin gia von
            'v_strSQL = "UPDATE SEMAST SET DCRAMT=0,DCRQTTY=0,PREVQTTY=PREVQTTY+" & v_dblDCRQTTY & ",COSTPRICE=" & v_dblCOSTPRICE & "," & ControlChars.CrLf _
            '    & " LASTDATE=TO_DATE('" & v_strTXDATE & "', '" & gc_FORMAT_DATE & "'),COSTDT=TO_DATE('" & v_strTXDATE & "', '" & gc_FORMAT_DATE & "')" & ControlChars.CrLf _
            '    & " WHERE ACCTNO='" & v_strACCTNO & "'"
            v_strSQL = "UPDATE SEMAST SET DCRAMT=0,DCRQTTY=0,PREVQTTY=TRADE+MORTAGE+MARGIN+SECURED+BLOCKED+WITHDRAW,COSTPRICE=" & v_dblCOSTPRICE & "," & ControlChars.CrLf _
            & " LASTDATE=TO_DATE('" & v_strTXDATE & "', '" & gc_FORMAT_DATE & "'),COSTDT=TO_DATE('" & v_strTXDATE & "', '" & gc_FORMAT_DATE & "')" & ControlChars.CrLf _
            & " WHERE ACCTNO='" & v_strACCTNO & "'"
            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

            'ContextUtil.SetComplete()
            Return v_lngErrCode
        Catch ex As Exception
            'ContextUtil.SetAbort()
            'LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
            '             & "Error code: System error!" & vbNewLine _
            '             & "Error message: " & ex.Message, EventLogEntryType.Error)
            Throw ex
        End Try
    End Function
#End Region
#Region " IDisposable Support "
    Private disposedValue As Boolean = False        ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: free other state (managed objects).
            End If

            ' TODO: free your own state (unmanaged objects).
            ' TODO: set large fields to null.
        End If
        Me.disposedValue = True
    End Sub
    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

#End Region
End Class
