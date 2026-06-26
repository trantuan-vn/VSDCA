Imports Sats.CommonLibrary
Imports Sats.CoreBusiness
Imports Sats.DataAccessLayer
Imports Sats.CA
Imports Sats.CS
Imports Sats.DE
Imports Sats.MF
Imports Sats.SF
'Imports System.EnterpriseServices
Imports System.IO
Imports System.Xml
Imports Sats.ServerCA
Imports BkavCASign
Imports System.Globalization

'Imports BkavCASign



'<Transaction(TransactionOption.Disabled), _
'ConstructionEnabled([Default]:="Sats.txRouter"), _
'ObjectPooling(Enabled:=True, MinPoolSize:=30)> _
Public Class txRouter
    Implements IDisposable
    'Inherits ServicedComponent

    Private mv_strTLTXErr As String = ""
    Private mv_strTLTXSucc As String = ""
    'Private mv_lwLogWriter As New LogWriter
    Public mv_strIpAddress As String
    Public mv_strWsName As String
    Public mv_strTellerId As String
    Public mv_strTellerName As String
    'Public mv_oSignServer As SignServer
    'Public Function Transact(ByRef pv_xmlDocument As Xml.XmlDocument) As Long
    '    Dim v_lngErrCode As Long = ERR_SYSTEM_OK
    '    Dim v_strErrorSource As String = "Host.txRouter.Transact", v_strErrorMessage As String = ""
    '    Dim v_trace_status As String = "0", v_trace_path As String '= "C:\log_sql_data.txt"
    '    Dim tr2 As TextWriterTraceListener
    '    Dim v_TranDataAccess As New DataAccess
    '    Dim v_nodeList As Xml.XmlNodeList
    '    Dim intIndex As Integer

    '    Try

    '        v_TranDataAccess.NewDBInstance(gc_MODULE_HOST)
    '        'Kiem tra tinh hop le cua message

    '        'Chỉ cho phép chạy Batch nếu HOSTATUS là ACTIVE
    '        Dim v_strSYSVAR As String = ""
    '        Dim v_strBRID As String = pv_xmlDocument.DocumentElement.Attributes(gc_AtributeBRID).Value.ToString

    '        v_lngErrCode = v_TranDataAccess.GetSysVar("SYSTEM", "BRSTATUS", v_strBRID, v_strSYSVAR)
    '        v_lngErrCode = v_TranDataAccess.GetSysVar("SYSTEM", "TRACE_STATUS", v_trace_status)
    '        v_lngErrCode = v_TranDataAccess.GetSysVar("SYSTEM", "TRACE_PATH", v_trace_path)
    '        If v_lngErrCode <> ERR_SYSTEM_OK Then
    '            LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
    '                         & "Error code: " & v_lngErrCode.ToString() & vbNewLine _
    '                         & "Error message: GetSysVar(""SYSTEM"", ""BRSTATUS"", v_strBRID, v_strSYSVAR) is failed" & v_strErrorMessage, EventLogEntryType.Information, gc_MODULE_HOST)
    '            ContextUtil.SetAbort()
    '            Return v_lngErrCode
    '        End If

    '        If v_strSYSVAR <> OPERATION_ACTIVE Then
    '            LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
    '                         & "Error code: " & v_lngErrCode.ToString() & vbNewLine _
    '                         & "Error message: HOSTATUS IS NOT ACTIVE", EventLogEntryType.Information, gc_MODULE_HOST)
    '            ContextUtil.SetAbort()
    '            Return ERR_SYSTEM_START
    '        End If
    '        If InStr(pv_xmlDocument.InnerXml, "ROOT") > 0 Then
    '            v_nodeList = pv_xmlDocument.SelectNodes("/ROOT/TransactMessage")
    '        Else
    '            v_nodeList = pv_xmlDocument.SelectNodes("/TransactMessage")
    '        End If

    '        For intIndex = 0 To v_nodeList.Count - 1
    '            v_lngErrCode = ExcuteTran(pv_xmlDocument, v_TranDataAccess, v_nodeList.Item(intIndex), tr2, v_trace_status, v_trace_path)
    '            GetTranErr(v_nodeList.Item(intIndex), v_lngErrCode)
    '        Next

    '        If v_trace_status = "1" Then
    '            Trace.Flush()
    '        End If

    '        WriteTranErr(pv_xmlDocument)

    '        ContextUtil.SetComplete()
    '        Return v_lngErrCode
    '    Catch ex As Exception
    '        ContextUtil.SetAbort()
    '        Throw ex
    '    Finally
    '        If v_trace_status = "1" Then
    '            tr2.Close()
    '            tr2.Dispose()
    '        End If
    '        v_TranDataAccess.Dispose()
    '    End Try
    'End Function
    'start Myvq
    Public mv_strFullFileName = ""
    'end Myvq

    Public Function Transact(ByRef pv_xmlDocument As Xml.XmlDocument, Optional ByVal mv_oSignServer As SignServer = Nothing, _
                             Optional ByRef v_ReturnDS As DataSet = Nothing, Optional ByRef v_strTLLOGIDCA As String = "") As Long
        Dim v_lngErrCode As Long = ERR_SYSTEM_OK
        Dim v_strErrorSource As String = "Host.txRouter.Transact", v_strErrorMessage As String = ""
        Dim v_nodeList As Xml.XmlNodeList
        Dim intIndex As Integer
        Dim v_obj As New DataAccess
        Dim v_strCurrDate As String
        Try
            v_obj.NewDBInstance(gc_MODULE_HOST)

            'Chỉ cho phép chạy Batch nếu HOSTATUS là ACTIVE
            Dim v_strSYSVAR As String = ""
            Dim v_strBRID As String = pv_xmlDocument.DocumentElement.Attributes(gc_AtributeBRID).Value.ToString
            Dim v_strBusDate As String = ""
            If Not pv_xmlDocument.DocumentElement.Attributes(gc_AtributeBUSDATE) Is Nothing Then
                v_strBusDate = pv_xmlDocument.DocumentElement.Attributes(gc_AtributeBUSDATE).Value.ToString
            End If
            v_lngErrCode = v_obj.GetSysVar("SYSTEM", "BRSTATUS", v_strBRID, v_strSYSVAR)
            v_lngErrCode = v_obj.GetSysVar("SYSTEM", "CURRDATE", v_strBRID, v_strCurrDate)
            If v_lngErrCode <> ERR_SYSTEM_OK Then
                LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
                             & "Error code: " & v_lngErrCode.ToString() & vbNewLine _
                             & "Error message: GetSysVar(""SYSTEM"", ""BRSTATUS"", v_strBRID, v_strSYSVAR) is failed" & v_strErrorMessage, EventLogEntryType.Information, gc_MODULE_HOST)
                'ContextUtil.SetAbort()
                Return v_lngErrCode
            End If
            If v_strBusDate <> "" Then
                If v_strBusDate <> v_strCurrDate Then
                    LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
                                 & "Error code: " & v_lngErrCode.ToString() & vbNewLine _
                                 & "Error message: DAY IS INVALID", EventLogEntryType.Information, gc_MODULE_HOST)
                    'ContextUtil.SetAbort()
                    WriteTranErrDayInvalid(pv_xmlDocument, 1)
                    Return ERR_SYSTEM_START
                End If
            End If
            If v_strSYSVAR <> OPERATION_ACTIVE Then
                LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
                             & "Error code: " & v_lngErrCode.ToString() & vbNewLine _
                             & "Error message: HOSTATUS IS NOT ACTIVE", EventLogEntryType.Information, gc_MODULE_HOST)
                'ContextUtil.SetAbort()
                WriteTranErrDayInvalid(pv_xmlDocument, 0)
                Return ERR_SYSTEM_START
            End If

            'v_obj.Dispose()

            If InStr(pv_xmlDocument.InnerXml, "ROOT") > 0 Then
                v_nodeList = pv_xmlDocument.SelectNodes("/ROOT/TransactMessage")
            Else
                v_nodeList = pv_xmlDocument.SelectNodes("/TransactMessage")
            End If

            For intIndex = 0 To v_nodeList.Count - 1
                'v_lngErrCode = CheckHOST(v_nodeList.Item(intIndex), pv_xmlDocument)
                'bangpv: Truyen them doi tuong signserver
                v_lngErrCode = CheckHOST(v_nodeList.Item(intIndex), pv_xmlDocument, mv_oSignServer)
                If v_lngErrCode = ERR_SYSTEM_OK Then
                    v_lngErrCode = ExcuteTran(v_nodeList.Item(intIndex), v_ReturnDS, v_strTLLOGIDCA)
                End If
                GetTranErr(v_nodeList.Item(intIndex), v_lngErrCode)
            Next

            WriteTranErr(pv_xmlDocument)
            'start Myvq
            WriteEncryptedFile(pv_xmlDocument)
            'End Myvq
            'ContextUtil.SetComplete()
            Return v_lngErrCode
        Catch ex As Exception
            'ContextUtil.SetAbort()
            Throw ex
        Finally
            If Not v_obj Is Nothing Then
                v_obj.Dispose()
            End If
            GC.Collect()
        End Try
    End Function

    'Private Function ExcuteTran(ByRef pv_xmlDocument As Xml.XmlDocument, ByRef v_TranDataAccess As DataAccess, ByRef v_node As Xml.XmlNode, ByRef tr2 As TextWriterTraceListener, ByRef v_trace_status As String, ByRef v_trace_path As String) As Long
    '    Dim blnTran As Boolean = False
    '    Dim v_trace As DataSet
    '    Dim v_strMFNO As String
    '    Dim v_ds, v_replace As DataSet
    '    Dim v_blnStockType As Boolean = False
    '    Dim v_strAUTOID As String
    '    Dim v_strSTATUS As String = "0"
    '    Dim v_strTXNUM As String
    '    Dim v_strTXDATE As String
    '    Dim v_lngErr As Long = ERR_SYSTEM_OK
    '    Try
    '        With v_node
    '            v_TranDataAccess.BeginTran()
    '            blnTran = True
    '            'Get message header
    '            Dim v_strBRID As String = .Attributes(gc_AtributeBRID).Value.ToString
    '            v_strSTATUS = .Attributes(gc_AtributeSTATUS).Value.ToString
    '            Dim v_strSTATUS_TEXT As String = .Attributes(gc_AtributeSTATUSTEXT).Value.ToString
    '            v_strAUTOID = .Attributes(gc_AtributeAUTOID).Value.ToString
    '            v_strTXNUM = .Attributes(gc_AtributeTXNUM).Value.ToString
    '            v_strTXDATE = .Attributes(gc_AtributeTXDATE).Value.ToString
    '            Dim v_strBUSDATE As String = .Attributes(gc_AtributeBUSDATE).Value.ToString
    '            Dim v_strTLTXCD As String = .Attributes(gc_AtributeTLTXCD).Value.ToString
    '            Dim v_strTXDESC As String = .Attributes(gc_AtributeTXDESC).Value.ToString
    '            Dim v_strBRCODE As String = .Attributes(gc_AtributeBRCODE).Value.ToString
    '            Dim v_strTLID As String = .Attributes(gc_AtributeTLID).Value.ToString
    '            Dim v_strCHKID As String = .Attributes(gc_AtributeCHKID).Value.ToString
    '            Dim v_strOFFID As String = .Attributes(gc_AtributeOFFID).Value.ToString
    '            Dim v_strCFRID As String = .Attributes(gc_AtributeCFRID).Value.ToString
    '            Dim v_strTLNAME As String = .Attributes(gc_AtributeTLNAME).Value.ToString
    '            Dim v_strCHKNAME As String = .Attributes(gc_AtributeCHKNAME).Value.ToString
    '            Dim v_strOFFNAME As String = .Attributes(gc_AtributeOFFNAME).Value.ToString
    '            Dim v_strCFRNAME As String = .Attributes(gc_AtributeCFRNAME).Value.ToString
    '            Dim v_strIPADDRESS As String = .Attributes(gc_AtributeIPADDRESS).Value.ToString
    '            Dim v_strWSNAME As String = .Attributes(gc_AtributeWSNAME).Value.ToString
    '            Dim v_strSICODE As String = .Attributes(gc_AtributeSICODE).Value.ToString
    '            Dim v_strMICODE As String = .Attributes(gc_AtributeMICODE).Value.ToString
    '            Dim v_strCOMICODE As String = .Attributes(gc_AtributeCOMICODE).Value.ToString
    '            Dim v_strMSGAMT As String = .Attributes(gc_AtributeMSGAMT).Value.ToString
    '            Dim v_strISPARENT As String = .Attributes(gc_AtributeISPARENT).Value.ToString
    '            Dim v_strPARENTID As String = .Attributes(gc_AtributePARENTID).Value.ToString
    '            Dim v_strPARENT_TEXT As String = .Attributes(gc_AtributePARENT_TEXT).Value.ToString
    '            Dim v_strTXNAME As String = .Attributes(gc_AtributeTXNAME).Value.ToString
    '            Dim v_strCHILDTLTXCD As String = .Attributes(gc_AtributeCHILDTLTXCD).Value.ToString
    '            Dim v_strISBRID As String = .Attributes(gc_AtributeISBRID).Value.ToString
    '            Dim v_strREASON As String = .Attributes(gc_AtributeREASON).Value.ToString

    '            Dim v_strSYSVAR As String = ""
    '            Dim v_strCurrDate1 As String
    '            If v_trace_status = "1" Then
    '                v_TranDataAccess.GetSysVar("SYSTEM", "CURRDATE", v_strBRID, v_strCurrDate1)
    '                Dim v_strTLName1 As String = v_strCFRNAME
    '                Dim v_trace_path1 As String = ""
    '                If v_strTLName1 = "" Then
    '                    v_strTLName1 = v_strOFFNAME
    '                End If
    '                If v_strTLName1 = "" Then
    '                    v_strTLName1 = v_strCHKNAME
    '                End If
    '                If v_strTLName1 = "" Then
    '                    v_strTLName1 = v_strTLNAME
    '                End If
    '                v_strCurrDate1 = Replace(v_strCurrDate1, "/", "_")
    '                If v_trace_path = "" Then
    '                    Dim v_app As New ApplicationServices.ApplicationBase
    '                    v_trace_path1 = v_app.Info.DirectoryPath & "\Log\" & v_strCurrDate1
    '                Else
    '                    v_trace_path1 = v_trace_path & v_strCurrDate1
    '                End If

    '                If Not System.IO.Directory.Exists(v_trace_path) Then
    '                    System.IO.Directory.CreateDirectory(v_trace_path1)
    '                End If

    '                v_trace_path1 &= "\log_dml_br" & v_strBRID & "_" & v_strTLName1 & ".txt"

    '                tr2 = New TextWriterTraceListener(System.IO.File.AppendText(v_trace_path1))
    '                Trace.Listeners.Add(tr2)
    '            End If
    '            'Nếu ngày trong branch khác với ngày hệ thống thì báo lỗi yêu cầu thoát ra vào lại hệ thống
    '            Dim v_strCURRDATE As String = ""
    '            Dim v_bCmd As New BusinessCommand

    '            Dim v_strSQL, v_strReplaceSQL, v_strSQLTmp, v_strFieldsSQL, v_strValuesSQL As String

    '            Dim intCountNo As Integer = 0

    '            'Add by thonm
    '            'Lay cong thuc tinh toan quyen thay the vao SQL
    '            'Su dung voi GD 6004, 6010
    '            Dim v_strCA As String = ""
    '            Dim v_strTN As String = ""
    '            Dim v_strPL As String = ""
    '            Dim v_strLCP As String = ""
    '            Select Case v_strTLTXCD
    '                Case "6038"
    '                    v_strSQL = "SELECT A.COL_VALUE10, A.COL_VALUE12, A.COL_VALUE13, A.COL_VALUE07 FROM TLLOG A, TLLOG B" _
    '                                    & " WHERE A.TXNUM = B.COL_VALUE01 AND B.AUTOID='" & v_strAUTOID & "'"
    '                    v_ds = v_TranDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
    '                    If v_ds.Tables(0).Rows.Count > 0 Then
    '                        v_strCA = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE10"))
    '                        v_strTN = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE12"))
    '                        v_strPL = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE13"))
    '                        v_strLCP = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE07"))
    '                    Else
    '                        v_strSQL = "SELECT A.COL_VALUE10, A.COL_VALUE12, A.COL_VALUE13, A.COL_VALUE07 FROM TLLOGALL A, TLLOG B" _
    '                                    & " WHERE A.TXNUM = B.COL_VALUE01 AND B.AUTOID='" & v_strAUTOID & "'"
    '                        v_ds = v_TranDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
    '                        If v_ds.Tables(0).Rows.Count > 0 Then
    '                            v_strCA = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE10"))
    '                            v_strTN = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE12"))
    '                            v_strPL = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE13"))
    '                            v_strLCP = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE07"))
    '                        End If
    '                    End If
    '                Case "6034"
    '                    v_strSQL = "SELECT A.COL_VALUE07, A.COL_VALUE08, A.COL_VALUE09, A.COL_VALUE10 FROM TLLOG A" _
    '                                        & " WHERE A.PARENTID='" & v_strAUTOID & "'"
    '                    v_ds = v_TranDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
    '                    If v_ds.Tables(0).Rows.Count > 0 Then
    '                        v_strCA = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE07"))
    '                        v_strTN = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE08"))
    '                        v_strPL = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE09"))
    '                        v_strLCP = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE10"))
    '                    End If
    '            End Select

    '            'Lay loai hinh phi GD
    '            v_strSQL = "SELECT MFNO FROM TLTX WHERE TLTXCD='" & v_strTLTXCD & "' AND DELETED=0 AND STATUS=0"
    '            v_ds = v_TranDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
    '            If v_ds.Tables(0).Rows.Count > 0 Then
    '                v_strMFNO = gf_CorrectStringField(v_ds.Tables(0).Rows(0)(0))
    '            End If
    '            'end thonm    

    '            v_strSQL = "select ordnum, trace, DMLSQL, DMLSQL1 from appdml where status = 0 and deleted = 0 and ((tltxcd = '" & v_strTLTXCD & "') or (tltxcd is null)) and instr(txstatus,'" & v_strSTATUS & "')>0"
    '            If v_strSTATUS.Trim() = CStr(TransactStatus.LOG_MEMBER_STAFF) Then
    '                If v_strCHKID = gc_TRANSACTION_ZERO Then
    '                    v_strSTATUS = TransactStatus.APPROVED_MEMBER_MANAGER
    '                    v_strSTATUS_TEXT = APPROVED_MEMBER_MANAGER_TEXT
    '                    v_strCHKID = v_strTLID
    '                    v_strCHKNAME = v_strTLNAME
    '                    v_strSQL = v_strSQL & " union select ordnum, trace, DMLSQL, DMLSQL1 from appdml where status = 0 and deleted = 0 and tltxcd = '" & v_strTLTXCD & "' and instr(txstatus,'" & v_strSTATUS & "')>0"
    '                    If v_strOFFID = gc_TRANSACTION_ZERO Then
    '                        v_strSTATUS = TransactStatus.APPROVED_VDS_STAFF
    '                        v_strOFFID = v_strCHKID
    '                        v_strOFFNAME = v_strCHKNAME
    '                        v_strSTATUS_TEXT = APPROVED_VDS_STAFF_TEXT
    '                        v_strSQL = v_strSQL & " union select ordnum, trace, DMLSQL, DMLSQL1 from appdml where status = 0 and deleted = 0 and tltxcd = '" & v_strTLTXCD & "' and instr(txstatus,'" & v_strSTATUS & "')>0"
    '                        If v_strCFRID = gc_TRANSACTION_ZERO Then
    '                            v_strSTATUS = TransactStatus.CONFIRMED_VSD_MANAGER
    '                            v_strCFRID = v_strOFFID
    '                            v_strCFRNAME = v_strOFFNAME
    '                            v_strSTATUS_TEXT = CONFIRMED_VSD_MANAGER_TEXT
    '                            v_strSQL = v_strSQL & " union select ordnum, trace, DMLSQL, DMLSQL1 from appdml where status = 0 and deleted = 0 and tltxcd = '" & v_strTLTXCD & "' and instr(txstatus,'" & v_strSTATUS & "')>0"
    '                        End If
    '                    End If
    '                End If
    '            End If
    '            If v_strSTATUS.Trim() = CStr(TransactStatus.APPROVED_MEMBER_MANAGER) Then
    '                If v_strOFFID = gc_TRANSACTION_ZERO Then
    '                    v_strSTATUS = TransactStatus.APPROVED_VDS_STAFF
    '                    v_strOFFID = v_strCHKID
    '                    v_strOFFNAME = v_strCHKNAME
    '                    v_strSTATUS_TEXT = APPROVED_VDS_STAFF_TEXT
    '                    v_strSQL = v_strSQL & " union select ordnum, trace, DMLSQL, DMLSQL1 from appdml where status = 0 and deleted = 0 and tltxcd = '" & v_strTLTXCD & "' and instr(txstatus,'" & v_strSTATUS & "')>0"
    '                    If v_strCFRID = gc_TRANSACTION_ZERO Then
    '                        v_strSTATUS = TransactStatus.CONFIRMED_VSD_MANAGER
    '                        v_strCFRID = v_strOFFID
    '                        v_strCFRNAME = v_strOFFNAME
    '                        v_strSTATUS_TEXT = CONFIRMED_VSD_MANAGER_TEXT
    '                        v_strSQL = v_strSQL & " union select ordnum, trace, DMLSQL, DMLSQL1 from appdml where status = 0 and deleted = 0 and tltxcd = '" & v_strTLTXCD & "' and instr(txstatus,'" & v_strSTATUS & "')>0"
    '                    End If
    '                End If
    '            End If
    '            If v_strSTATUS.Trim() = CStr(TransactStatus.APPROVED_VDS_STAFF) Then
    '                If v_strCFRID = gc_TRANSACTION_ZERO Then
    '                    v_strSTATUS = TransactStatus.CONFIRMED_VSD_MANAGER
    '                    v_strCFRID = v_strOFFID
    '                    v_strCFRNAME = v_strOFFNAME
    '                    v_strSTATUS_TEXT = CONFIRMED_VSD_MANAGER_TEXT
    '                    v_strSQL = v_strSQL & " union select ordnum, trace, DMLSQL, DMLSQL1 from appdml where status = 0 and deleted = 0 and tltxcd = '" & v_strTLTXCD & "' and instr(txstatus,'" & v_strSTATUS & "')>0"
    '                End If
    '            End If

    '            v_strReplaceSQL = "select count(*) from (" & v_strSQL & " ) where DMLSQL like '%?STOCK_TYPE%' OR DMLSQL1 like '%?STOCK_TYPE%'"
    '            v_replace = v_TranDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strReplaceSQL)
    '            If v_replace.Tables(0).Rows(0)(0) > 0 Then
    '                v_blnStockType = True
    '            End If

    '            v_strSQL = "select * from (" & v_strSQL & " ) order by ordnum"

    '            v_bCmd.ExecuteUser = v_strTLID
    '            v_bCmd.SQLCommand = v_strSQL
    '            v_ds = v_TranDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_bCmd.SQLCommand)
    '            If v_ds.Tables(0).Rows.Count > 0 Then
    '                v_strReplaceSQL = "select max(fldname) from fldmaster where objname = '" & v_strTLTXCD & "' and deleted=0 and status=0"
    '                v_bCmd.SQLCommand = v_strReplaceSQL
    '                v_replace = v_TranDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_bCmd.SQLCommand)
    '                If v_replace.Tables(0).Rows.Count > 0 Then
    '                    intCountNo = v_replace.Tables(0).Rows(0)(0)
    '                End If
    '            End If

    '            If v_trace_status = "1" Then
    '                Trace.WriteLine("[Bắt đầu: APPDML - " & v_strTLTXCD & "] " & DateTime.Now & vbCrLf)
    '            End If
    '            ' thay the cac tham so can thiet 
    '            If v_blnStockType Then
    '                For v_intStockType As Integer = 1 To 6
    '                    For i As Integer = 0 To v_ds.Tables(0).Rows.Count - 1
    '                        v_strSQL = v_ds.Tables(0).Rows(i)("DMLSQL") & " " & gf_CorrectStringField(v_ds.Tables(0).Rows(i)("DMLSQL1"))
    '                        ' thay the cac transaction message 
    '                        ' nhom trang thai giao dich
    '                        v_strSQL = Replace(v_strSQL, "?STATUS_TEXT", "'" & v_strSTATUS_TEXT & "'")
    '                        v_strSQL = Replace(v_strSQL, "?STATUS", v_strSTATUS)
    '                        ' nhom ma giao dich
    '                        v_strSQL = Replace(v_strSQL, "?AUTOID", v_strAUTOID)
    '                        v_strSQL = Replace(v_strSQL, "?TXNUM", "'" & v_strTXNUM & "'")
    '                        v_strSQL = Replace(v_strSQL, "?TXNAME", "'" & v_strTXNAME & "'")
    '                        v_strSQL = Replace(v_strSQL, "?TXDATE", "TO_DATE('" & v_strTXDATE & "','dd/mm/yyyy')")

    '                        'v_strSQL = Replace(v_strSQL, "?BUSDATE", IIf(IsDate(v_strBUSDATE), "TO_DATE('" & v_strBUSDATE & "','dd/mm/yyyy')", "NULL"))
    '                        v_strSQL = Replace(v_strSQL, "?BUSDATE", "TO_DATE('" & v_strBUSDATE & "','dd/mm/yyyy')")

    '                        ' nhom loai giao dich
    '                        v_strSQL = Replace(v_strSQL, "?TLTXCD", "'" & v_strTLTXCD & "'")
    '                        v_strSQL = Replace(v_strSQL, "?TXDESC", "'" & v_strTXDESC & "'")
    '                        ' nhom chi nhanh
    '                        v_strSQL = Replace(v_strSQL, "?BRID", "'" & v_strBRID & "'")
    '                        v_strSQL = Replace(v_strSQL, "?BRCODE", "'" & v_strBRCODE & "'")
    '                        ' nhom user
    '                        v_strSQL = Replace(v_strSQL, "?TLID", "'" & v_strTLID & "'")
    '                        v_strSQL = Replace(v_strSQL, "?OFFID", "'" & v_strOFFID & "'")
    '                        v_strSQL = Replace(v_strSQL, "?CFRID", "'" & v_strCFRID & "'")
    '                        v_strSQL = Replace(v_strSQL, "?CHKID", "'" & v_strCHKID & "'")
    '                        v_strSQL = Replace(v_strSQL, "?TLNAME", "'" & v_strTLNAME & "'")
    '                        v_strSQL = Replace(v_strSQL, "?OFFNAME", "'" & v_strOFFNAME & "'")
    '                        v_strSQL = Replace(v_strSQL, "?CHKNAME", "'" & v_strCHKNAME & "'")
    '                        v_strSQL = Replace(v_strSQL, "?CFRNAME", "'" & v_strCFRNAME & "'")
    '                        ' nhom dia chi tao giao dich
    '                        v_strSQL = Replace(v_strSQL, "?IPADDRESS", "'" & v_strIPADDRESS & "'")
    '                        v_strSQL = Replace(v_strSQL, "?WSNAME", "'" & v_strWSNAME & "'")
    '                        ' nhom quyen
    '                        v_strSQL = Replace(v_strSQL, "?SICODE", "'" & v_strSICODE & "'")
    '                        v_strSQL = Replace(v_strSQL, "?MICODE", "'" & v_strMICODE & "'")
    '                        v_strSQL = Replace(v_strSQL, "?COMICODE", "'" & v_strCOMICODE & "'")
    '                        v_strSQL = Replace(v_strSQL, "?MSGAMT", IIf(v_strMSGAMT <> "", "TO_NUMBER(COL_VALUE" & v_strMSGAMT & ")", "0"))
    '                        ' nhom giao dich cha-con
    '                        v_strSQL = Replace(v_strSQL, "?ISPARENT", v_strISPARENT)
    '                        v_strSQL = Replace(v_strSQL, "?PARENTID", v_strPARENTID)
    '                        v_strSQL = Replace(v_strSQL, "?PARENT_TEXT", "'" & v_strPARENT_TEXT & "'")
    '                        v_strSQL = Replace(v_strSQL, "?CHILDTLTXCD", "'" & v_strCHILDTLTXCD & "'")
    '                        v_strSQL = Replace(v_strSQL, "?STOCK_TYPE", "'" & v_intStockType & "'")
    '                        v_strSQL = Replace(v_strSQL, "?ISBRID", v_strISBRID)
    '                        v_strSQL = Replace(v_strSQL, "?REASON", v_strREASON)
    '                        'Add by thonm
    '                        'Thay the cong thuc tinh toan quyen
    '                        v_strSQL = Replace(v_strSQL, "?CA", v_strCA)
    '                        v_strSQL = Replace(v_strSQL, "?TN", v_strTN)
    '                        v_strSQL = Replace(v_strSQL, "?PL", v_strPL)
    '                        v_strSQL = Replace(v_strSQL, "?LCP", v_strLCP)
    '                        'End thonm

    '                        If v_trace_status = "1" And v_ds.Tables(0).Rows(i)("TRACE") = 1 Then
    '                            Trace.WriteLine("[APPDML - " & v_strTLTXCD & "] " & DateTime.Now & " :" & vbCrLf & " -o- Câu lệnh thứ #" & v_ds.Tables(0).Rows(i)("ORDNUM") & "-o-" & vbCrLf & v_strSQL & vbCrLf)
    '                            If InStr(v_strSQL.ToUpper, "SELECT") > 0 Then
    '                                Trace.WriteLine("-o- Dữ liệu câu lệnh thứ #" & v_ds.Tables(0).Rows(i)("ORDNUM") & "-o-")
    '                                v_strSQLTmp = v_strSQL.Substring(InStr(v_strSQL.ToUpper, "SELECT") - 1)
    '                                v_trace = v_TranDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
    '                                Trace.WriteLine(v_trace.GetXml & vbCrLf)
    '                            End If
    '                        End If

    '                        v_strSQLTmp = ""
    '                        If InStr(v_strSQL, "?TMP_TXFIELDS") > 0 Then
    '                            Dim v_nodelist1 As Xml.XmlNodeList
    '                            v_nodelist1 = pv_xmlDocument.SelectNodes("/TransactMessage/fields")
    '                            If v_nodelist1.Count > 0 Then
    '                                For ii As Integer = 0 To v_nodelist1.Count - 1
    '                                    For j As Integer = 0 To v_nodelist1.Item(ii).ChildNodes.Count - 1
    '                                        With v_nodelist1.Item(ii).ChildNodes(j)
    '                                            v_strSQLTmp = .InnerText
    '                                            v_TranDataAccess.ExecuteNonQuery(CommandType.Text, v_strSQLTmp)
    '                                        End With
    '                                    Next
    '                                Next
    '                            Else
    '                                v_strFieldsSQL = ""
    '                                v_strValuesSQL = ""
    '                                For j As Integer = 1 To intCountNo
    '                                    v_strFieldsSQL = v_strFieldsSQL & ", COL_VALUE" & String.Format("{0:00}", j) _
    '                                    & ", COL_TYPE" & String.Format("{0:00}", j) _
    '                                    & ", COL_DESC" & String.Format("{0:00}", j)
    '                                Next
    '                                v_strSQLTmp = "INSERT INTO TMP_TXFIELDS(AUTOID, TLTXCD " & v_strFieldsSQL & ") " _
    '                                & " SELECT SEQ_TMP_TXFIELDS.NEXTVAL, TLTXCD " & v_strFieldsSQL & " FROM " _
    '                                & " ( SELECT " _
    '                                & "'" & v_strTLTXCD & "' TLTXCD" & v_strFieldsSQL _
    '                                & " FROM  TLLOG " _
    '                                & " WHERE deleted =0 and " & IIf(v_strISPARENT = "2", "PARENTID = ", "AUTOID = ") & v_strAUTOID _
    '                                & " ) "
    '                                v_strSQL = v_strSQLTmp
    '                                v_TranDataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
    '                            End If
    '                        ElseIf InStr(v_strSQL, "?TLLOG") > 0 Then
    '                            v_strFieldsSQL = ""
    '                            For j As Integer = 1 To intCountNo
    '                                v_strFieldsSQL = v_strFieldsSQL & ", COL_VALUE" & String.Format("{0:00}", j) _
    '                                & ", COL_TYPE" & String.Format("{0:00}", j) _
    '                                & ", COL_DESC" & String.Format("{0:00}", j)
    '                            Next
    '                            v_strSQL = v_strSQL.Replace("?TLLOG", v_strFieldsSQL)
    '                            v_strFieldsSQL = ""
    '                            For j As Integer = 1 To intCountNo
    '                                v_strFieldsSQL = v_strFieldsSQL & ", a.COL_VALUE" & String.Format("{0:00}", j) _
    '                                & ", a.COL_TYPE" & String.Format("{0:00}", j) _
    '                                & ", a.COL_DESC" & String.Format("{0:00}", j)
    '                            Next
    '                            v_strSQL = v_strSQL.Replace("?VALUES", v_strFieldsSQL)
    '                            v_TranDataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)

    '                            'Add by thonm
    '                        ElseIf InStr(v_strSQL, "?MFTOTX") > 0 Then
    '                            'Tinh phi giao dich
    '                            Dim v_arrMFNO() As String
    '                            Dim v_int As Integer
    '                            Dim v_strFomula As String
    '                            Dim v_dsMF As DataSet

    '                            If Trim(v_strMFNO) <> "" Then
    '                                v_arrMFNO = v_strMFNO.Split("|")

    '                                For v_int = 1 To v_arrMFNO.Length - 1
    '                                    'Lay cong thuc tinh phi
    '                                    v_strSQL = "SELECT FOMULA FROM MFTYPE " _
    '                                                  & " WHERE MFNO='" & v_arrMFNO(v_int) & "' AND BRID='" & v_strBRID & "' AND DELETED=0 AND STATUS=0"
    '                                    v_dsMF = v_TranDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
    '                                    'Neu ton tai cong thuc tinh phi
    '                                    If v_dsMF.Tables(0).Rows.Count > 0 Then
    '                                        v_strFomula = v_dsMF.Tables(0).Rows(0)(0)

    '                                        v_strSQL = "INSERT INTO TMP_TLLOG(autoid, txnum, txdate, col_value01, col_value02)" _
    '                                                      & " SELECT autoid, txnum, txdate, MICODE || '." & v_arrMFNO(v_int) & "'," & v_strFomula & " FROM" _
    '                                                      & " (SELECT a.autoid,a.txnum,a.txdate, b.type T, nvl(a.msgamt,0) X,a.MICODE" _
    '                                                      & " FROM tllog a, RGSI b WHERE a.sicode=b.sicode" _
    '                                                      & " AND a.deleted =0 AND " & IIf(v_strISPARENT = "2", "A.PARENTID = ", "A.AUTOID = ") & v_strAUTOID & ")"

    '                                        'Ghi file log
    '                                        If v_trace_status = "1" And v_ds.Tables(0).Rows(i)("TRACE") = 1 Then
    '                                            Trace.WriteLine("APPDML - " & v_strTLTXCD & "-o- Câu lệnh thứ :" & v_ds.Tables(0).Rows(i)("ORDNUM") & "-o- Dữ liệu sau khi được tính phí-o-")
    '                                            v_strSQLTmp = v_strSQL.Substring(InStr(v_strSQL.ToUpper, "SELECT") - 1)
    '                                            v_trace = v_TranDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
    '                                            Trace.WriteLine(v_trace.GetXml & vbCrLf)
    '                                        End If
    '                                        v_TranDataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
    '                                    End If
    '                                Next
    '                            End If

    '                        ElseIf InStr(v_strSQL, "?PHTTQ") > 0 Then
    '                            Dim v_TmpDS As DataSet
    '                            Dim v_strMsgDate As String
    '                            Dim v_strCATYPE As String
    '                            v_strSQL = "SELECT COL_VALUE04, COL_VALUE05, COL_VALUE07 FROM TLLOG WHERE AUTOID = " & v_strAUTOID
    '                            v_TmpDS = v_TranDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
    '                            If v_TmpDS.Tables(0).Rows.Count > 0 Then
    '                                If CInt(v_TmpDS.Tables(0).Rows(0)("COL_VALUE07")) = 0 Then
    '                                    v_strCATYPE = v_TmpDS.Tables(0).Rows(0)("COL_VALUE05")
    '                                    v_strMsgDate = v_TmpDS.Tables(0).Rows(0)("COL_VALUE04")

    '                                    v_strSQL = "INSERT INTO CATRAN(AUTOID, TXNUM, TXDATE, ACCTNO, NAMT, BRID, OPERATOR, TLTXCD, MSGDATE)" _
    '                                                & " SELECT SEQ_CATRAN.NEXTVAL, '" & v_strTXNUM & "', TO_DATE('" & v_strTXDATE & "','dd/mm/yyyy'), CAACCTNO," _
    '                                                & " BALANCE, BRID,'-','" & v_strTLTXCD & "', MSGDATE FROM CAMAST WHERE SUBSTR(CAACCTNO,1,3) = '000' AND GET_TOKEN(CAACCTNO,3,'.') = '" & v_strSICODE & "'" _
    '                                                & " AND GET_TOKEN(CAACCTNO,6,'.') = '05' AND GET_TOKEN(CAACCTNO,7,'.') = LPAD(" & v_strCATYPE & ",2,'0') AND TO_CHAR(MSGDATE,'dd/mm/yyyy') = '" & v_strMsgDate & "'"

    '                                    v_TranDataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)

    '                                    v_strSQL = "DELETE FROM CAMAST WHERE SUBSTR(CAACCTNO,1,3) = '000' AND GET_TOKEN(CAACCTNO,3,'.') = '" & v_strSICODE & "'" _
    '                                                & " AND GET_TOKEN(CAACCTNO,6,'.') = '05' AND GET_TOKEN(CAACCTNO,7,'.') = LPAD(" & v_strCATYPE & ",2,'0') AND TO_CHAR(MSGDATE,'dd/mm/yyyy') = '" & v_strMsgDate & "'"

    '                                    v_TranDataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
    '                                End If
    '                            End If
    '                        Else
    '                            'end thonm
    '                            ' thuc hien cau lenh
    '                            v_TranDataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
    '                        End If
    '                        ' end tuanta 09/12/2008
    '                    Next
    '                Next
    '            Else
    '                For i As Integer = 0 To v_ds.Tables(0).Rows.Count - 1
    '                    v_strSQL = v_ds.Tables(0).Rows(i)("DMLSQL") & " " & gf_CorrectStringField(v_ds.Tables(0).Rows(i)("DMLSQL1"))
    '                    ' thay the cac transaction message 
    '                    ' nhom trang thai giao dich
    '                    v_strSQL = Replace(v_strSQL, "?STATUS_TEXT", "'" & v_strSTATUS_TEXT & "'")
    '                    v_strSQL = Replace(v_strSQL, "?STATUS", v_strSTATUS)
    '                    ' nhom ma giao dich
    '                    v_strSQL = Replace(v_strSQL, "?AUTOID", v_strAUTOID)
    '                    v_strSQL = Replace(v_strSQL, "?TXNUM", "'" & v_strTXNUM & "'")
    '                    v_strSQL = Replace(v_strSQL, "?TXNAME", "'" & v_strTXNAME & "'")
    '                    v_strSQL = Replace(v_strSQL, "?TXDATE", "TO_DATE('" & v_strTXDATE & "','dd/mm/yyyy')")

    '                    'v_strSQL = Replace(v_strSQL, "?BUSDATE", IIf(IsDate(v_strBUSDATE), "TO_DATE('" & v_strBUSDATE & "','dd/mm/yyyy')", "NULL"))
    '                    v_strSQL = Replace(v_strSQL, "?BUSDATE", "TO_DATE('" & v_strBUSDATE & "','dd/mm/yyyy')")


    '                    ' nhom loai giao dich
    '                    v_strSQL = Replace(v_strSQL, "?TLTXCD", "'" & v_strTLTXCD & "'")
    '                    v_strSQL = Replace(v_strSQL, "?TXDESC", "'" & v_strTXDESC & "'")
    '                    ' nhom chi nhanh
    '                    v_strSQL = Replace(v_strSQL, "?BRID", "'" & v_strBRID & "'")
    '                    v_strSQL = Replace(v_strSQL, "?BRCODE", "'" & v_strBRCODE & "'")
    '                    ' nhom user
    '                    v_strSQL = Replace(v_strSQL, "?TLID", "'" & v_strTLID & "'")
    '                    v_strSQL = Replace(v_strSQL, "?OFFID", "'" & v_strOFFID & "'")
    '                    v_strSQL = Replace(v_strSQL, "?CFRID", "'" & v_strCFRID & "'")
    '                    v_strSQL = Replace(v_strSQL, "?CHKID", "'" & v_strCHKID & "'")
    '                    v_strSQL = Replace(v_strSQL, "?TLNAME", "'" & v_strTLNAME & "'")
    '                    v_strSQL = Replace(v_strSQL, "?OFFNAME", "'" & v_strOFFNAME & "'")
    '                    v_strSQL = Replace(v_strSQL, "?CHKNAME", "'" & v_strCHKNAME & "'")
    '                    v_strSQL = Replace(v_strSQL, "?CFRNAME", "'" & v_strCFRNAME & "'")
    '                    ' nhom dia chi tao giao dich
    '                    v_strSQL = Replace(v_strSQL, "?IPADDRESS", "'" & v_strIPADDRESS & "'")
    '                    v_strSQL = Replace(v_strSQL, "?WSNAME", "'" & v_strWSNAME & "'")
    '                    ' nhom quyen
    '                    v_strSQL = Replace(v_strSQL, "?SICODE", "'" & v_strSICODE & "'")
    '                    v_strSQL = Replace(v_strSQL, "?MICODE", "'" & v_strMICODE & "'")
    '                    v_strSQL = Replace(v_strSQL, "?COMICODE", "'" & v_strCOMICODE & "'")
    '                    v_strSQL = Replace(v_strSQL, "?MSGAMT", IIf(v_strMSGAMT <> "", "TO_NUMBER(a.COL_VALUE" & v_strMSGAMT & ")", "0"))
    '                    ' nhom giao dich cha-con
    '                    v_strSQL = Replace(v_strSQL, "?ISPARENT", v_strISPARENT)
    '                    v_strSQL = Replace(v_strSQL, "?PARENTID", v_strPARENTID)
    '                    v_strSQL = Replace(v_strSQL, "?PARENT_TEXT", "'" & v_strPARENT_TEXT & "'")
    '                    v_strSQL = Replace(v_strSQL, "?CHILDTLTXCD", "'" & v_strCHILDTLTXCD & "'")
    '                    v_strSQL = Replace(v_strSQL, "?ISBRID", v_strISBRID)
    '                    v_strSQL = Replace(v_strSQL, "?REASON", v_strREASON)
    '                    'Add by thonm
    '                    'Thay the cong thuc tinh toan quyen
    '                    v_strSQL = Replace(v_strSQL, "?CA", v_strCA)
    '                    v_strSQL = Replace(v_strSQL, "?TN", v_strTN)
    '                    v_strSQL = Replace(v_strSQL, "?PL", v_strPL)
    '                    v_strSQL = Replace(v_strSQL, "?LCP", v_strLCP)
    '                    'End thonm

    '                    If v_trace_status = "1" And v_ds.Tables(0).Rows(i)("TRACE") = 1 Then
    '                        Trace.WriteLine("[APPDML - " & v_strTLTXCD & "] " & DateTime.Now & " :" & vbCrLf & " -o- Câu lệnh thứ #" & v_ds.Tables(0).Rows(i)("ORDNUM") & "-o-" & vbCrLf & v_strSQL & vbCrLf)
    '                        If InStr(v_strSQL.ToUpper, "SELECT") > 0 Then
    '                            Trace.WriteLine("-o- Dữ liệu câu lệnh thứ #" & v_ds.Tables(0).Rows(i)("ORDNUM") & "-o-")
    '                            v_strSQLTmp = v_strSQL.Substring(InStr(v_strSQL.ToUpper, "SELECT") - 1)
    '                            v_trace = v_TranDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
    '                            Trace.WriteLine(v_trace.GetXml & vbCrLf)
    '                        End If
    '                    End If

    '                    v_strSQLTmp = ""
    '                    If InStr(v_strSQL, "?TMP_TXFIELDS") > 0 Then
    '                        Dim v_nodelist1 As Xml.XmlNodeList
    '                        v_nodelist1 = pv_xmlDocument.SelectNodes("/TransactMessage/fields")
    '                        If v_nodelist1.Count > 0 Then
    '                            For ii As Integer = 0 To v_nodelist1.Count - 1
    '                                For j As Integer = 0 To v_nodelist1.Item(ii).ChildNodes.Count - 1
    '                                    With v_nodelist1.Item(ii).ChildNodes(j)
    '                                        v_strSQLTmp = .InnerText
    '                                        v_TranDataAccess.ExecuteNonQuery(CommandType.Text, v_strSQLTmp)
    '                                    End With
    '                                Next
    '                            Next
    '                        Else
    '                            v_strFieldsSQL = ""
    '                            For j As Integer = 1 To intCountNo
    '                                v_strFieldsSQL = v_strFieldsSQL & ", COL_VALUE" & String.Format("{0:00}", j) _
    '                                & ", COL_TYPE" & String.Format("{0:00}", j) _
    '                                & ", COL_DESC" & String.Format("{0:00}", j)
    '                            Next
    '                            v_strSQLTmp = "INSERT INTO TMP_TXFIELDS(AUTOID, TLTXCD " & v_strFieldsSQL & ") " _
    '                            & " SELECT SEQ_TMP_TXFIELDS.NEXTVAL, TLTXCD " & v_strFieldsSQL & " FROM " _
    '                            & " ( SELECT " _
    '                            & "'" & v_strTLTXCD & "' TLTXCD" & v_strFieldsSQL _
    '                            & " FROM  TLLOG " _
    '                            & " WHERE deleted =0 and " & IIf(v_strISPARENT = "2", "PARENTID = ", "AUTOID = ") & v_strAUTOID _
    '                            & " )"
    '                            v_strSQL = v_strSQLTmp
    '                            v_TranDataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
    '                        End If
    '                    ElseIf InStr(v_strSQL, "?TLLOG") > 0 Then
    '                        v_strFieldsSQL = ""
    '                        For j As Integer = 1 To intCountNo
    '                            v_strFieldsSQL = v_strFieldsSQL & ", COL_VALUE" & String.Format("{0:00}", j) _
    '                            & ", COL_TYPE" & String.Format("{0:00}", j) _
    '                            & ", COL_DESC" & String.Format("{0:00}", j)
    '                        Next
    '                        v_strSQL = v_strSQL.Replace("?TLLOG", v_strFieldsSQL)
    '                        v_strFieldsSQL = ""
    '                        For j As Integer = 1 To intCountNo
    '                            v_strFieldsSQL = v_strFieldsSQL & ", a.COL_VALUE" & String.Format("{0:00}", j) _
    '                            & ", a.COL_TYPE" & String.Format("{0:00}", j) _
    '                            & ", a.COL_DESC" & String.Format("{0:00}", j)
    '                        Next
    '                        v_strSQL = v_strSQL.Replace("?VALUES", v_strFieldsSQL)
    '                        v_TranDataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)

    '                        'add by thonm
    '                    ElseIf InStr(v_strSQL, "?MFTOTX") > 0 Then
    '                        'Tinh phi giao dich
    '                        Dim v_arrMFNO() As String
    '                        Dim v_int As Integer
    '                        Dim v_strFomula As String
    '                        Dim v_dsMF As DataSet

    '                        If Trim(v_strMFNO) <> "" Then
    '                            v_arrMFNO = v_strMFNO.Split("|")

    '                            For v_int = 1 To v_arrMFNO.Length - 1
    '                                'Lay cong thuc tinh phi
    '                                v_strSQL = "SELECT FOMULA FROM MFTYPE " _
    '                                              & " WHERE MFNO='" & v_arrMFNO(v_int) & "' AND BRID='" & v_strBRID & "' AND DELETED=0 AND STATUS=0"
    '                                v_dsMF = v_TranDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
    '                                'Neu ton tai cong thuc tinh phi
    '                                If v_dsMF.Tables(0).Rows.Count > 0 Then
    '                                    v_strFomula = v_dsMF.Tables(0).Rows(0)(0)

    '                                    v_strSQL = "INSERT INTO TMP_TLLOG(autoid, txnum, txdate, col_value01, col_value02)" _
    '                                                  & " SELECT autoid, txnum, txdate, MICODE || '." & v_arrMFNO(v_int) & "'," & v_strFomula & " FROM" _
    '                                                  & " (SELECT a.autoid,a.txnum,a.txdate, b.type T, nvl(a.msgamt,0) X, a.MICODE" _
    '                                                  & " FROM tllog a, RGSI b WHERE a.sicode=b.sicode" _
    '                                                  & " AND a.deleted =0 AND " & IIf(v_strISPARENT = "2", "A.PARENTID = ", "A.AUTOID = ") & v_strAUTOID & ")"

    '                                    'Ghi file log
    '                                    If v_trace_status = "1" And v_ds.Tables(0).Rows(i)("TRACE") = 1 Then
    '                                        Trace.WriteLine("APPDML - " & v_strTLTXCD & "-o- Câu lệnh thứ :" & v_ds.Tables(0).Rows(i)("ORDNUM") & "-o- Dữ liệu sau khi được tính phí-o-")
    '                                        v_strSQLTmp = v_strSQL.Substring(InStr(v_strSQL.ToUpper, "SELECT") - 1)
    '                                        v_trace = v_TranDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
    '                                        Trace.WriteLine(v_trace.GetXml & vbCrLf)
    '                                    End If
    '                                    v_TranDataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
    '                                End If
    '                            Next
    '                        End If

    '                    ElseIf InStr(v_strSQL, "?PHTTQ") > 0 Then
    '                        Dim v_TmpDS As DataSet
    '                        Dim v_strMsgDate As String
    '                        Dim v_strCATYPE As String
    '                        v_strSQL = "SELECT COL_VALUE04, COL_VALUE05, COL_VALUE07 FROM TMP_TLLOG"
    '                        v_TmpDS = v_TranDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
    '                        If v_TmpDS.Tables(0).Rows.Count > 0 Then
    '                            If CInt(v_TmpDS.Tables(0).Rows(0)("COL_VALUE07")) = 0 Then
    '                                v_strCATYPE = v_TmpDS.Tables(0).Rows(0)("COL_VALUE05")
    '                                v_strMsgDate = v_TmpDS.Tables(0).Rows(0)("COL_VALUE04")

    '                                v_strSQL = "INSERT INTO CATRAN(AUTOID, TXNUM, TXDATE, ACCTNO, NAMT, BRID, OPERATOR, TLTXCD, MSGDATE)" _
    '                                            & " SELECT SEQ_CATRAN.NEXTVAL, '" & v_strTXNUM & "', TO_DATE('" & v_strTXDATE & "','dd/mm/yyyy'), CAACCTNO," _
    '                                            & " BALANCE, BRID,'-','" & v_strTLTXCD & "', MSGDATE FROM CAMAST WHERE SUBSTR(CAACCTNO,1,3) = '000' AND GET_TOKEN(CAACCTNO,3,'.') = '" & v_strSICODE & "'" _
    '                                            & " AND GET_TOKEN(CAACCTNO,6,'.') = '05' AND GET_TOKEN(CAACCTNO,7,'.') = LPAD(" & v_strCATYPE & ",2,'0')  AND TO_CHAR(MSGDATE,'dd/mm/yyyy') = '" & v_strMsgDate & "'"

    '                                v_TranDataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)

    '                                v_strSQL = "DELETE FROM CAMAST WHERE SUBSTR(CAACCTNO,1,3) = '000' AND GET_TOKEN(CAACCTNO,3,'.') = '" & v_strSICODE & "'" _
    '                                            & " AND GET_TOKEN(CAACCTNO,6,'.') = '05' AND GET_TOKEN(CAACCTNO,7,'.') = LPAD(" & v_strCATYPE & ",2,'0')  AND TO_CHAR(MSGDATE,'dd/mm/yyyy') = '" & v_strMsgDate & "'"

    '                                v_TranDataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
    '                            End If
    '                        End If
    '                    Else
    '                        ' end thonm
    '                        ' thuc hien cau lenh
    '                        v_TranDataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
    '                    End If
    '                    '' end tuanta 09/12/2008
    '                Next

    '                If v_trace_status = "1" Then
    '                    Trace.WriteLine("[Kết thúc: APPDML - " & v_strTLTXCD & "] " & DateTime.Now & vbCrLf)
    '                    tr2.Close()
    '                    tr2.Dispose()
    '                End If
    '            End If
    '            ' attache txnum
    '            If v_strISPARENT = 2 Then
    '                v_strSQLTmp = "select txnum from tmp_tllog_result where rownum=1"
    '                v_trace = v_TranDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
    '                If v_trace.Tables(0).Rows.Count > 0 Then
    '                    .Attributes(gc_AtributeTXNUM).InnerText = v_trace.Tables(0).Rows(0)(0).ToString
    '                End If
    '            End If
    '            v_TranDataAccess.Commit()
    '            blnTran = False
    '            If v_strSTATUS <> 0 Then
    '                Dim v_attrAUTOID, v_attrTXNUM, v_attrTXDATE, v_attrSTATUS As Xml.XmlAttribute
    '                Dim v_dataElement As Xml.XmlElement

    '                v_dataElement = pv_xmlDocument.CreateElement(Xml.XmlNodeType.Element, "HOST_MESSAGE", "")
    '                v_attrTXNUM = pv_xmlDocument.CreateAttribute(gc_AtributeTXNUM)
    '                v_attrTXNUM.Value = v_strTXNUM
    '                v_dataElement.Attributes.Append(v_attrTXNUM)

    '                v_attrTXDATE = pv_xmlDocument.CreateAttribute(gc_AtributeTXDATE)
    '                v_attrTXDATE.Value = v_strTXDATE
    '                v_dataElement.Attributes.Append(v_attrTXDATE)

    '                v_attrAUTOID = pv_xmlDocument.CreateAttribute(gc_AtributeAUTOID)
    '                v_attrAUTOID.Value = v_strAUTOID
    '                v_dataElement.Attributes.Append(v_attrAUTOID)

    '                v_attrSTATUS = pv_xmlDocument.CreateAttribute(gc_AtributeSTATUS)
    '                v_attrSTATUS.Value = "1"
    '                v_dataElement.Attributes.Append(v_attrSTATUS)

    '                pv_xmlDocument.DocumentElement.AppendChild(v_dataElement)
    '            End If
    '        End With

    '        Return v_lngErr
    '    Catch ex As Exception
    '        If blnTran Then
    '            v_TranDataAccess.Rollback()
    '        End If

    '        ex.Source = "Host.txRouter.Transact"
    '        LogError.Write("Error source: " & ex.Source & vbNewLine _
    '                     & "Error code: System error!" & vbNewLine _
    '                     & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_HOST)
    '        If v_strSTATUS <> "0" Then
    '            Dim v_attrAUTOID, v_attrTXNUM, v_attrTXDATE, v_attrSTATUS As Xml.XmlAttribute
    '            Dim v_dataElement As Xml.XmlElement

    '            v_dataElement = pv_xmlDocument.CreateElement(Xml.XmlNodeType.Element, "HOST_MESSAGE", "")
    '            v_attrTXNUM = pv_xmlDocument.CreateAttribute(gc_AtributeTXNUM)
    '            v_attrTXNUM.Value = v_strTXNUM
    '            v_dataElement.Attributes.Append(v_attrTXNUM)

    '            v_attrTXDATE = pv_xmlDocument.CreateAttribute(gc_AtributeTXDATE)
    '            v_attrTXDATE.Value = v_strTXDATE
    '            v_dataElement.Attributes.Append(v_attrTXDATE)

    '            v_attrAUTOID = pv_xmlDocument.CreateAttribute(gc_AtributeAUTOID)
    '            v_attrAUTOID.Value = v_strAUTOID
    '            v_dataElement.Attributes.Append(v_attrAUTOID)

    '            v_attrSTATUS = pv_xmlDocument.CreateAttribute(gc_AtributeSTATUS)
    '            v_attrSTATUS.Value = "0"
    '            v_dataElement.Attributes.Append(v_attrSTATUS)

    '            pv_xmlDocument.DocumentElement.AppendChild(v_dataElement)
    '        End If
    '        Return ERR_SYSTEM_START
    '    End Try
    'End Function

    Private Function ExcuteTran(ByRef v_node As Xml.XmlNode, Optional ByRef v_ReturnDS As DataSet = Nothing, Optional ByRef v_strCATLLOGID As String = "") As Long
        Dim blnTran As Boolean = False
        Dim v_trace As DataSet
        Dim v_strMFNO As String
        Dim v_ds, v_replace As DataSet
        Dim v_blnStockType As Boolean = False
        Dim v_strAUTOID As String
        Dim v_strSTATUS As String = "0"
        Dim v_strTXNUM As String
        Dim v_strTXDATE As String
        Dim v_lngErr As Long = ERR_SYSTEM_OK
        Dim v_obj As New DataAccess
        Dim tr2 As TextWriterTraceListener
        Dim v_trace_status, v_trace_path As String
        'bangpv
        'Dim v_strCATLLOGID As String
        Dim v_nodeList As Xml.XmlNodeList
        Dim v_strPARENT_TLTXCD As String = v_node.Attributes(gc_AtributePARENT_TLTXCD).Value.ToString
        Dim v_strOLDSTATUS As String = v_node.Attributes(gc_AtributeOLDSTATUS).Value.ToString
        Dim v_strIsSignCA As String = v_node.Attributes(gc_AtributeSIGNCA).Value.ToString
        'Dim v_strSignatureClient As String = v_node.Attributes(gc_AtributeSignatureClient).Value.ToString
        Try

            v_obj.NewDBInstance(gc_MODULE_HOST)
            With v_node
                v_obj.BeginTran()
                blnTran = True
                'Get message header
                Dim v_strBRID As String = .Attributes(gc_AtributeBRID).Value.ToString
                v_strSTATUS = .Attributes(gc_AtributeSTATUS).Value.ToString
                'bangpv: voi CA
                Dim v_strCAStatus As String = v_strSTATUS
                Dim v_strSTATUS_TEXT As String = .Attributes(gc_AtributeSTATUSTEXT).Value.ToString
                v_strAUTOID = .Attributes(gc_AtributeAUTOID).Value.ToString
                v_strTXNUM = .Attributes(gc_AtributeTXNUM).Value.ToString
                v_strTXDATE = .Attributes(gc_AtributeTXDATE).Value.ToString
                Dim v_strBUSDATE As String = .Attributes(gc_AtributeBUSDATE).Value.ToString
                Dim v_strTLTXCD As String = .Attributes(gc_AtributeTLTXCD).Value.ToString
                Dim v_strTXDESC As String = .Attributes(gc_AtributeTXDESC).Value.ToString
                Dim v_strBRCODE As String = .Attributes(gc_AtributeBRCODE).Value.ToString
                Dim v_strTLID As String = .Attributes(gc_AtributeTLID).Value.ToString
                Dim v_strCHKID As String = .Attributes(gc_AtributeCHKID).Value.ToString
                Dim v_strOFFID As String = .Attributes(gc_AtributeOFFID).Value.ToString
                Dim v_strCFRID As String = .Attributes(gc_AtributeCFRID).Value.ToString
                Dim v_strTLNAME As String = .Attributes(gc_AtributeTLNAME).Value.ToString
                Dim v_strCHKNAME As String = .Attributes(gc_AtributeCHKNAME).Value.ToString
                Dim v_strOFFNAME As String = .Attributes(gc_AtributeOFFNAME).Value.ToString
                Dim v_strCFRNAME As String = .Attributes(gc_AtributeCFRNAME).Value.ToString
                Dim v_strIPADDRESS As String = .Attributes(gc_AtributeIPADDRESS).Value.ToString
                Dim v_strWSNAME As String = .Attributes(gc_AtributeWSNAME).Value.ToString
                Dim v_strSICODE As String = .Attributes(gc_AtributeSICODE).Value.ToString
                Dim v_strMICODE As String = .Attributes(gc_AtributeMICODE).Value.ToString
                Dim v_strCOMICODE As String = .Attributes(gc_AtributeCOMICODE).Value.ToString
                Dim v_strMSGAMT As String = .Attributes(gc_AtributeMSGAMT).Value.ToString
                Dim v_strISPARENT As String = .Attributes(gc_AtributeISPARENT).Value.ToString
                Dim v_strPARENTID As String = .Attributes(gc_AtributePARENTID).Value.ToString
                Dim v_strPARENT_TEXT As String = .Attributes(gc_AtributePARENT_TEXT).Value.ToString
                Dim v_strTXNAME As String = .Attributes(gc_AtributeTXNAME).Value.ToString
                Dim v_strCHILDTLTXCD As String = .Attributes(gc_AtributeCHILDTLTXCD).Value.ToString
                Dim v_strISBRID As String = .Attributes(gc_AtributeISBRID).Value.ToString
                Dim v_strREASON As String = .Attributes(gc_AtributeREASON).Value.ToString
                'hanm5
                Dim v_strTXNOTE As String = .Attributes(gc_AtributeTXNOTE).Value.ToString
                Dim v_strVsdBrid As String = .Attributes(gc_AtributeVSDBRID).Value.ToString
                Dim v_strVsdBrid2 As String '= .Attributes(gc_AtributeVSDBRID2).Value.ToString
                Dim v_strTblChk As String = .Attributes(gc_AtributeTBLCHK).Value.ToString
                Dim v_strStateFile As String = .Attributes(gc_AtributeStateFile).Value.ToString

                Dim v_strSYSVAR As String = ""
                Dim v_strCurrDate1 As String

                'start Myvq
                'Dim v_strEncryptedFile As String = ""
                'Dim v_strBranchId As String = ""
                'Dim v_strBridCurrdate As String = ""
                'Try
                '    v_strBranchId = .Attributes("BRANCH_ID").Value.ToString
                '    v_strBridCurrdate = .Attributes("CURR_DATE").Value.ToString
                '    v_strEncryptedFile = .Attributes("ENCRYPTED_FILE").Value.ToString
                'Catch ex As Exception
                'End Try
                'end Myvq
                '27/02/2014: Lay lai phan cap giao dich
                If v_strSTATUS.Trim() = CStr(TransactStatus.LOG_MEMBER_STAFF) Then
                    GetTltxUserAuth(v_strPARENT_TLTXCD, v_strTLID, v_strCHKID, v_strOFFID, v_strCFRID, v_obj)
                End If
                'Hanm5 Ket thuc ngay 27/02/2014 
                mv_strIpAddress = v_strIPADDRESS
                mv_strWsName = v_strWSNAME
                mv_strTellerName = v_strCFRNAME
                If mv_strTellerName = "" Then
                    mv_strTellerName = v_strOFFNAME
                    mv_strTellerId = v_strOFFID
                End If
                If mv_strTellerName = "" Then
                    mv_strTellerName = v_strCHKNAME
                    mv_strTellerId = v_strCHKID
                End If
                If mv_strTellerName = "" Then
                    mv_strTellerName = v_strTLNAME
                    mv_strTellerId = v_strTLID
                End If

                v_obj.GetSysVar("SYSTEM", "TRACE_STATUS", v_trace_status)
                v_obj.GetSysVar("SYSTEM", "TRACE_PATH", v_trace_path)
                v_obj.GetSysVar("SYSTEM", "CURRDATE", v_strBRID, v_strCurrDate1)

                Dim v_strSQL, v_strReplaceSQL, v_strSQLTmp, v_strFieldsSQL, v_strValuesSQL As String

                ' date : 14/10/2008
                ' Purpose : get from  date T-3 to date T
                Dim v_strT_T_3 As String
                v_strSQL = "select to_char(GET_T_PLUS(to_date('" & v_strCurrDate1 & "', 'dd/mm/yyyy'),'" & v_strBRID & "',-1),'dd/mm/yyyy') txdate from dual" _
                        & " union select to_char(GET_T_PLUS(to_date('" & v_strCurrDate1 & "', 'dd/mm/yyyy'),'" & v_strBRID & "',-2),'dd/mm/yyyy') txdate from dual" _
                        & " union select to_char(GET_T_PLUS(to_date('" & v_strCurrDate1 & "', 'dd/mm/yyyy'),'" & v_strBRID & "',-3),'dd/mm/yyyy') txdate from dual"
                v_trace = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

                v_strT_T_3 = "to_date('" & v_strCurrDate1 & "', 'dd/mm/yyyy'),"
                For i = 0 To v_trace.Tables(0).Rows.Count - 1
                    v_strT_T_3 = v_strT_T_3 & "to_date('" & v_trace.Tables(0).Rows(i)("txdate") & "', 'dd/mm/yyyy'),"
                Next
                v_strT_T_3 = "(" & Left(v_strT_T_3, v_strT_T_3.Length - 1) & ")"
                ' end 
                ' date : 14/10/2010
                ' Purpose : get from  hour from sysdate
                v_strSQL = "select to_char(sysdate, 'hh24:mi:ss') time from dual"
                v_trace = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                Dim v_strTime As String = v_trace.Tables(0).Rows(0)("time")
                'end


                If v_trace_status = "1" Then
                    Dim v_strTLName1 As String = v_strCFRNAME
                    Dim v_trace_path1 As String = ""
                    If v_strTLName1 = "" Then
                        v_strTLName1 = v_strOFFNAME
                    End If
                    If v_strTLName1 = "" Then
                        v_strTLName1 = v_strCHKNAME
                    End If
                    If v_strTLName1 = "" Then
                        v_strTLName1 = v_strTLNAME
                    End If
                    v_strCurrDate1 = Replace(v_strCurrDate1, "/", "_")
                    If v_trace_path = "" Then
                        Dim v_app As New ApplicationServices.ApplicationBase
                        v_trace_path1 = v_app.Info.DirectoryPath & "\Log\" & v_strCurrDate1
                    Else
                        v_trace_path1 = v_trace_path & v_strCurrDate1
                    End If

                    If Not System.IO.Directory.Exists(v_trace_path) Then
                        System.IO.Directory.CreateDirectory(v_trace_path1)
                    End If

                    v_trace_path1 &= "\log_dml_br" & v_strBRID & "_" & v_strTLName1 & ".txt"

                    tr2 = New TextWriterTraceListener(System.IO.File.AppendText(v_trace_path1))
                    Trace.Listeners.Add(tr2)
                End If
                'Nếu ngày trong branch khác với ngày hệ thống thì báo lỗi yêu cầu thoát ra vào lại hệ thống
                Dim v_strCURRDATE As String = ""
                Dim v_bCmd As New BusinessCommand

                Dim intCountNo As Integer = 0

                'Add by thonm
                'Lay cong thuc tinh toan quyen thay the vao SQL
                'Su dung voi GD 6004, 6010
                Dim v_strCA As String = ""
                Dim v_strTN As String = ""
                Dim v_strPL As String = ""
                Dim v_strLCP As String = ""
                Select Case v_strTLTXCD
                    Case "6038"
                        v_strSQL = "SELECT A.COL_VALUE10, A.COL_VALUE12, A.COL_VALUE13, A.COL_VALUE07 FROM TLLOG A, TLLOG B" _
                                        & " WHERE A.TXNUM = B.COL_VALUE01 AND B.AUTOID='" & v_strAUTOID & "'"
                        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                        'Wtite Log
                        'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "ExcuteTran", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Excute SQL command: " & v_strSQL)

                        If v_ds.Tables(0).Rows.Count > 0 Then
                            v_strCA = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE10"))
                            v_strTN = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE12"))
                            v_strPL = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE13"))
                            v_strLCP = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE07"))
                        Else
                            v_strSQL = "SELECT A.COL_VALUE10, A.COL_VALUE12, A.COL_VALUE13, A.COL_VALUE07 FROM TLLOGALL A, TLLOG B" _
                                        & " WHERE A.TXNUM = B.COL_VALUE01 AND B.AUTOID='" & v_strAUTOID & "'"
                            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                            'Wtite Log
                            'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "ExcuteTran", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Excute SQL command: " & v_strSQL)

                            If v_ds.Tables(0).Rows.Count > 0 Then
                                v_strCA = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE10"))
                                v_strTN = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE12"))
                                v_strPL = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE13"))
                                v_strLCP = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE07"))
                            End If
                        End If
                    Case "6034"
                        v_strSQL = "SELECT A.COL_VALUE07, A.COL_VALUE08, A.COL_VALUE09, A.COL_VALUE10 FROM TLLOG A" _
                                            & " WHERE A.PARENTID='" & v_strAUTOID & "'"
                        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                        'Wtite Log
                        'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "ExcuteTran", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Excute SQL command: " & v_strSQL)

                        If v_ds.Tables(0).Rows.Count > 0 Then
                            v_strCA = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE07"))
                            v_strTN = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE08"))
                            v_strPL = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE09"))
                            v_strLCP = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE10"))
                        End If
                        'bangpv: theem voi quyen trai phieu chuyen doi
                    Case "6065"
                        v_strSQL = "SELECT A.COL_VALUE07, A.COL_VALUE08, A.COL_VALUE09, A.COL_VALUE10 FROM TLLOG A" _
                                            & " WHERE A.PARENTID='" & v_strAUTOID & "'"
                        v_strSQL = "SELECT A.COL_VALUE10 COL_VALUE07, A.COL_VALUE12 COL_VALUE08, A.COL_VALUE13 COL_VALUE09, A.COL_VALUE07 COL_VALUE10 " _
                                    & " FROM TLLOG A WHERE autoid =(SELECT to_number(col_value01) FROM tllog WHERE autoid =" & v_strAUTOID & ")"
                        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                        'Wtite Log
                        'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "ExcuteTran", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Excute SQL command: " & v_strSQL)

                        If v_ds.Tables(0).Rows.Count > 0 Then
                            v_strCA = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE07"))
                            v_strTN = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE08"))
                            v_strPL = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE09"))
                            v_strLCP = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE10"))
                        End If
                        'end bangpv 
                End Select

                'Lay loai hinh phi GD
                v_strSQL = "SELECT MFNO FROM TLTX WHERE TLTXCD='" & v_strTLTXCD & "' AND DELETED=0 AND STATUS=0"
                v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                'Wtite Log
                'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "ExcuteTran", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Excute SQL command: " & v_strSQL)

                If v_ds.Tables(0).Rows.Count > 0 Then
                    v_strMFNO = gf_CorrectStringField(v_ds.Tables(0).Rows(0)(0))
                End If
                'end thonm    

                v_strSQL = "select ordnum, trace, DMLSQL, DMLSQL1 from appdml where status = 0 and deleted = 0 and ((tltxcd = '" & v_strTLTXCD & "') or (tltxcd is null)) and instr(txstatus,'" & v_strSTATUS & "')>0"
                If v_strSTATUS.Trim() = CStr(TransactStatus.LOG_MEMBER_STAFF) Then
                    If v_strCHKID = gc_TRANSACTION_ZERO Then
                        v_strSTATUS = TransactStatus.APPROVED_MEMBER_MANAGER
                        v_strSTATUS_TEXT = APPROVED_MEMBER_MANAGER_TEXT
                        v_strCHKID = v_strTLID
                        v_strCHKNAME = v_strTLNAME
                        v_strSQL = v_strSQL & " union select ordnum, trace, DMLSQL, DMLSQL1 from appdml where status = 0 and deleted = 0 and tltxcd = '" & v_strTLTXCD & "' and instr(txstatus,'" & v_strSTATUS & "')>0"
                        If v_strOFFID = gc_TRANSACTION_ZERO Then
                            v_strSTATUS = TransactStatus.APPROVED_VDS_STAFF
                            v_strOFFID = v_strCHKID
                            v_strOFFNAME = v_strCHKNAME
                            v_strSTATUS_TEXT = APPROVED_VDS_STAFF_TEXT
                            v_strSQL = v_strSQL & " union select ordnum, trace, DMLSQL, DMLSQL1 from appdml where status = 0 and deleted = 0 and tltxcd = '" & v_strTLTXCD & "' and instr(txstatus,'" & v_strSTATUS & "')>0"
                            If v_strCFRID = gc_TRANSACTION_ZERO Then
                                v_strSTATUS = TransactStatus.CONFIRMED_VSD_MANAGER
                                v_strCFRID = v_strOFFID
                                v_strCFRNAME = v_strOFFNAME
                                v_strSTATUS_TEXT = CONFIRMED_VSD_MANAGER_TEXT
                                v_strBUSDATE = v_strTXDATE
                                v_strSQL = v_strSQL & " union select ordnum, trace, DMLSQL, DMLSQL1 from appdml where status = 0 and deleted = 0 and tltxcd = '" & v_strTLTXCD & "' and instr(txstatus,'" & v_strSTATUS & "')>0"
                            End If
                        End If
                    End If
                End If
                If v_strSTATUS.Trim() = CStr(TransactStatus.APPROVED_MEMBER_MANAGER) Then
                    If v_strOFFID = gc_TRANSACTION_ZERO Then
                        v_strSTATUS = TransactStatus.APPROVED_VDS_STAFF
                        v_strOFFID = v_strCHKID
                        v_strOFFNAME = v_strCHKNAME
                        v_strSTATUS_TEXT = APPROVED_VDS_STAFF_TEXT
                        v_strSQL = v_strSQL & " union select ordnum, trace, DMLSQL, DMLSQL1 from appdml where status = 0 and deleted = 0 and tltxcd = '" & v_strTLTXCD & "' and instr(txstatus,'" & v_strSTATUS & "')>0"
                        If v_strCFRID = gc_TRANSACTION_ZERO Then
                            v_strSTATUS = TransactStatus.CONFIRMED_VSD_MANAGER
                            v_strCFRID = v_strOFFID
                            v_strCFRNAME = v_strOFFNAME
                            v_strSTATUS_TEXT = CONFIRMED_VSD_MANAGER_TEXT
                            v_strSQL = v_strSQL & " union select ordnum, trace, DMLSQL, DMLSQL1 from appdml where status = 0 and deleted = 0 and tltxcd = '" & v_strTLTXCD & "' and instr(txstatus,'" & v_strSTATUS & "')>0"
                        End If
                    End If
                End If
                If v_strSTATUS.Trim() = CStr(TransactStatus.APPROVED_VDS_STAFF) Then
                    If v_strCFRID = gc_TRANSACTION_ZERO Then
                        v_strSTATUS = TransactStatus.CONFIRMED_VSD_MANAGER
                        v_strCFRID = v_strOFFID
                        v_strCFRNAME = v_strOFFNAME
                        v_strSTATUS_TEXT = CONFIRMED_VSD_MANAGER_TEXT
                        v_strSQL = v_strSQL & " union select ordnum, trace, DMLSQL, DMLSQL1 from appdml where status = 0 and deleted = 0 and tltxcd = '" & v_strTLTXCD & "' and instr(txstatus,'" & v_strSTATUS & "')>0"
                    End If
                End If

                'v_strReplaceSQL = "select count(*) from (" & v_strSQL & " ) where DMLSQL like '%?STOCK_TYPE%' OR DMLSQL1 like '%?STOCK_TYPE%'"
                'v_replace = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strReplaceSQL)
                ''Wtite Log
                ''mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "ExcuteTran", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Excute SQL command: " & v_strReplaceSQL)

                'If v_replace.Tables(0).Rows(0)(0) > 0 Then
                '    v_blnStockType = True
                'End If

                v_strSQL = "select * from (" & v_strSQL & " ) order by ordnum"

                v_bCmd.ExecuteUser = v_strTLID
                v_bCmd.SQLCommand = v_strSQL
                v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_bCmd.SQLCommand)
                'Wtite Log
                'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "ExcuteTran", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Excute SQL command: " & v_strSQL)

                If v_ds.Tables(0).Rows.Count > 0 Then
                    v_strReplaceSQL = "select max(fldname) from fldmaster where objname = '" & v_strTLTXCD & "' and deleted=0 and status=0"
                    v_bCmd.SQLCommand = v_strReplaceSQL
                    v_replace = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_bCmd.SQLCommand)
                    If v_replace.Tables(0).Rows.Count > 0 Then
                        intCountNo = v_replace.Tables(0).Rows(0)(0)
                    End If
                End If

                If v_trace_status = "1" Then
                    Trace.WriteLine("[Bắt đầu: APPDML - " & v_strTLTXCD & "] " & DateTime.Now & vbCrLf)
                End If
                ' thay the cac tham so can thiet 
                'If v_blnStockType Then
                '    For v_intStockType As Integer = 1 To 6
                '        For i As Integer = 0 To v_ds.Tables(0).Rows.Count - 1
                '            v_strSQL = v_ds.Tables(0).Rows(i)("DMLSQL") & " " & gf_CorrectStringField(v_ds.Tables(0).Rows(i)("DMLSQL1"))
                '            ' thay the cac transaction message 
                '            ' nhom trang thai giao dich
                '            v_strSQL = Replace(v_strSQL, "?STATUS_TEXT", "'" & v_strSTATUS_TEXT & "'")
                '            v_strSQL = Replace(v_strSQL, "?STATUS", v_strSTATUS)
                '            ' nhom ma giao dich
                '            v_strSQL = Replace(v_strSQL, "?AUTOID", v_strAUTOID)
                '            v_strSQL = Replace(v_strSQL, "?TXNUM", "'" & v_strTXNUM & "'")
                '            v_strSQL = Replace(v_strSQL, "?TXNAME", "'" & v_strTXNAME & "'")
                '            v_strSQL = Replace(v_strSQL, "?TXDATE", "TO_DATE('" & v_strTXDATE & "','dd/mm/yyyy')")

                '            'v_strSQL = Replace(v_strSQL, "?BUSDATE", IIf(IsDate(v_strBUSDATE), "TO_DATE('" & v_strBUSDATE & "','dd/mm/yyyy')", "NULL"))
                '            v_strSQL = Replace(v_strSQL, "?BUSDATE", "TO_DATE('" & v_strBUSDATE & "','dd/mm/yyyy')")

                '            ' nhom loai giao dich
                '            v_strSQL = Replace(v_strSQL, "?TLTXCD", "'" & v_strTLTXCD & "'")
                '            v_strSQL = Replace(v_strSQL, "?TXDESC", "'" & v_strTXDESC & "'")
                '            ' nhom chi nhanh
                '            v_strSQL = Replace(v_strSQL, "?BRID", "'" & v_strBRID & "'")
                '            v_strSQL = Replace(v_strSQL, "?BRCODE", "'" & v_strBRCODE & "'")
                '            ' nhom user
                '            v_strSQL = Replace(v_strSQL, "?TLID", "'" & v_strTLID & "'")
                '            v_strSQL = Replace(v_strSQL, "?OFFID", "'" & v_strOFFID & "'")
                '            v_strSQL = Replace(v_strSQL, "?CFRID", "'" & v_strCFRID & "'")
                '            v_strSQL = Replace(v_strSQL, "?CHKID", "'" & v_strCHKID & "'")
                '            v_strSQL = Replace(v_strSQL, "?TLNAME", "'" & v_strTLNAME & "'")
                '            v_strSQL = Replace(v_strSQL, "?OFFNAME", "'" & v_strOFFNAME & "'")
                '            v_strSQL = Replace(v_strSQL, "?CHKNAME", "'" & v_strCHKNAME & "'")
                '            v_strSQL = Replace(v_strSQL, "?CFRNAME", "'" & v_strCFRNAME & "'")
                '            ' nhom dia chi tao giao dich
                '            v_strSQL = Replace(v_strSQL, "?IPADDRESS", "'" & v_strIPADDRESS & "'")
                '            v_strSQL = Replace(v_strSQL, "?WSNAME", "'" & v_strWSNAME & "'")
                '            ' nhom quyen
                '            v_strSQL = Replace(v_strSQL, "?SICODE", "'" & IIf(v_strSICODE = "", "000", v_strSICODE) & "'")
                '            v_strSQL = Replace(v_strSQL, "?MICODE", "'" & IIf(v_strMICODE = "", "000", v_strMICODE) & "'")
                '            v_strSQL = Replace(v_strSQL, "?COMICODE", "'" & IIf(v_strCOMICODE = "", "000", v_strCOMICODE) & "'")
                '            v_strSQL = Replace(v_strSQL, "?MSGAMT", IIf(v_strMSGAMT <> "", "TO_NUMBER(COL_VALUE" & v_strMSGAMT & ")", "0"))
                '            ' nhom giao dich cha-con
                '            v_strSQL = Replace(v_strSQL, "?ISPARENT", v_strISPARENT)
                '            v_strSQL = Replace(v_strSQL, "?PARENTID", v_strPARENTID)
                '            v_strSQL = Replace(v_strSQL, "?PARENT_TEXT", "'" & v_strPARENT_TEXT & "'")
                '            v_strSQL = Replace(v_strSQL, "?CHILDTLTXCD", "'" & v_strCHILDTLTXCD & "'")
                '            v_strSQL = Replace(v_strSQL, "?PARENT_TLTXCD", "'" & v_strCHILDTLTXCD & "'")
                '            v_strSQL = Replace(v_strSQL, "?STOCK_TYPE", "'" & v_intStockType & "'")
                '            v_strSQL = Replace(v_strSQL, "?ISBRID", v_strISBRID)
                '            v_strSQL = Replace(v_strSQL, "?REASON", v_strREASON)
                '            v_strSQL = Replace(v_strSQL, "?TXNOTE", "'" & v_strTXNOTE & "'")
                '            v_strSQL = Replace(v_strSQL, "?VSD_BRID", "'" & v_strVsdBrid & "'")
                '            v_strSQL = Replace(v_strSQL, "?VSD_2_BRID", "'" & v_strVsdBrid2 & "'")
                '            v_strSQL = Replace(v_strSQL, "?T_T_3", v_strT_T_3)
                '            'Add by thonm
                '            'Thay the cong thuc tinh toan quyen
                '            v_strSQL = Replace(v_strSQL, "?CA", v_strCA)
                '            v_strSQL = Replace(v_strSQL, "?TN", v_strTN)
                '            v_strSQL = Replace(v_strSQL, "?PL", v_strPL)
                '            v_strSQL = Replace(v_strSQL, "?LCP", v_strLCP)
                '            'End thonm

                '            If v_trace_status = "1" And v_ds.Tables(0).Rows(i)("TRACE") = 1 Then
                '                Trace.WriteLine("[APPDML - " & v_strTLTXCD & "] " & DateTime.Now & " :" & vbCrLf & " -o- Câu lệnh thứ #" & v_ds.Tables(0).Rows(i)("ORDNUM") & "-o-" & vbCrLf & v_strSQL & vbCrLf)
                '                If InStr(v_strSQL.ToUpper, "SELECT") > 0 Then
                '                    Trace.WriteLine("-o- Dữ liệu câu lệnh thứ #" & v_ds.Tables(0).Rows(i)("ORDNUM") & "-o-")
                '                    v_strSQLTmp = v_strSQL.Substring(InStr(v_strSQL.ToUpper, "SELECT") - 1)
                '                    v_trace = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                '                    Trace.WriteLine(v_trace.GetXml & vbCrLf)
                '                End If
                '            End If

                '            v_strSQLTmp = ""
                '            If InStr(v_strSQL, "?TMP_TXFIELDS") > 0 Then
                '                Dim v_nodelist1 As Xml.XmlNodeList
                '                v_nodelist1 = v_node.SelectNodes("/TransactMessage/fields")
                '                If v_nodelist1.Count > 0 Then
                '                    For ii As Integer = 0 To v_nodelist1.Count - 1
                '                        For j As Integer = 0 To v_nodelist1.Item(ii).ChildNodes.Count - 1
                '                            With v_nodelist1.Item(ii).ChildNodes(j)
                '                                v_strSQLTmp = .InnerText
                '                                v_obj.ExecuteNonQuery(CommandType.Text, v_strSQLTmp)
                '                            End With
                '                        Next
                '                    Next
                '                Else
                '                    v_strFieldsSQL = ""
                '                    v_strValuesSQL = ""
                '                    For j As Integer = 1 To intCountNo
                '                        v_strFieldsSQL = v_strFieldsSQL & ", COL_VALUE" & String.Format("{0:00}", j) _
                '                        & ", COL_TYPE" & String.Format("{0:00}", j) _
                '                        & ", COL_DESC" & String.Format("{0:00}", j)
                '                    Next
                '                    v_strSQLTmp = "INSERT INTO TMP_TXFIELDS(AUTOID, TLTXCD " & v_strFieldsSQL & ", real_row) " _
                '                    & " SELECT SEQ_TMP_TXFIELDS.NEXTVAL, TLTXCD " & v_strFieldsSQL & ", rownum FROM " _
                '                    & " ( SELECT " _
                '                    & "'" & v_strTLTXCD & "' TLTXCD" & v_strFieldsSQL _
                '                    & " FROM  TLLOG " _
                '                    & " WHERE deleted =0 and " & IIf(v_strISPARENT = "2", "PARENTID = ", "AUTOID = ") & v_strAUTOID _
                '                    & " ) "
                '                    v_strSQL = v_strSQLTmp
                '                    v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                '                    'Wtite Log
                '                    'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "ExcuteTran", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Excute SQL command: " & v_strSQL)

                '                End If
                '            ElseIf InStr(v_strSQL, "?TLLOG") > 0 Then
                '                v_strFieldsSQL = ""
                '                For j As Integer = 1 To intCountNo
                '                    v_strFieldsSQL = v_strFieldsSQL & ", COL_VALUE" & String.Format("{0:00}", j) _
                '                    & ", COL_TYPE" & String.Format("{0:00}", j) _
                '                    & ", COL_DESC" & String.Format("{0:00}", j)
                '                Next
                '                v_strSQL = v_strSQL.Replace("?TLLOG", v_strFieldsSQL)
                '                v_strFieldsSQL = ""
                '                For j As Integer = 1 To intCountNo
                '                    v_strFieldsSQL = v_strFieldsSQL & ", a.COL_VALUE" & String.Format("{0:00}", j) _
                '                    & ", a.COL_TYPE" & String.Format("{0:00}", j) _
                '                    & ", a.COL_DESC" & String.Format("{0:00}", j)
                '                Next
                '                v_strSQL = v_strSQL.Replace("?VALUES", v_strFieldsSQL)
                '                v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                '                'Wtite Log
                '                'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "ExcuteTran", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Excute SQL command: " & v_strSQL)

                '                'Add by thonm
                '            ElseIf InStr(v_strSQL, "?MFTOTX") > 0 Then
                '                'Tinh phi giao dich
                '                Dim v_arrMFNO() As String
                '                Dim v_int As Integer
                '                Dim v_strFomula As String
                '                Dim v_dsMF As DataSet

                '                If Trim(v_strMFNO) <> "" Then
                '                    v_arrMFNO = v_strMFNO.Split("|")

                '                    For v_int = 1 To v_arrMFNO.Length - 1
                '                        'Lay cong thuc tinh phi
                '                        v_strSQL = "SELECT FOMULA FROM MFTYPE " _
                '                                      & " WHERE MFNO='" & v_arrMFNO(v_int) & "' AND DELETED=0 AND STATUS=0"
                '                        v_dsMF = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                '                        'Wtite Log
                '                        'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "ExcuteTran", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Excute SQL command: " & v_strSQL)


                '                        'Neu ton tai cong thuc tinh phi
                '                        If v_dsMF.Tables(0).Rows.Count > 0 Then
                '                            v_strFomula = v_dsMF.Tables(0).Rows(0)(0)

                '                            v_strSQL = "INSERT INTO TMP_TLLOG(autoid, txnum, txdate, col_value01, col_value02, col_value03,msgamt, col_value04, col_value05, col_value06, col_value07)" _
                '                                          & " SELECT autoid, txnum, D txdate, M || '." & v_arrMFNO(v_int) & "'," & v_strFomula & " , '" & v_strFomula & "' , X, M micode, '" & v_arrMFNO(v_int) & "', N typeno, S sicode  FROM" _
                '                                          & " (SELECT a.autoid,a.txnum, a.txdate D, b.type T, " & IIf(v_strTLTXCD = "4084", "a.balance", "nvl(a.namt,0)") & " X,a.MICODE M, a.TYPENO N, a.SICODE S, a.brid B " _
                '                                          & " FROM " & IIf(v_strTLTXCD = "4084", "cstran", "matran") & " a, RGSI b WHERE a.sicode=b.sicode" _
                '                                          & " AND b.deleted=0 and b.status=0 AND a.txnum = lpad(" & v_strAUTOID & ",10,'0') " _
                '                                          & IIf(v_strTLTXCD = "4084", " and ((a.tltxcd in ('4081','4082') and a.step='22') or (a.tltxcd='4083' and a.step='1')) ", " and a.OPERATOR='-'") & " )"
                '                            'Ghi file log
                '                            If v_trace_status = "1" And v_ds.Tables(0).Rows(i)("TRACE") = 1 Then
                '                                Trace.WriteLine("APPDML - " & v_strTLTXCD & "-o- Câu lệnh thứ :" & v_ds.Tables(0).Rows(i)("ORDNUM") & "-o- Dữ liệu sau khi được tính phí-o-")
                '                                v_strSQLTmp = v_strSQL.Substring(InStr(v_strSQL.ToUpper, "SELECT") - 1)
                '                                v_trace = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                '                                Trace.WriteLine(v_trace.GetXml & vbCrLf)
                '                            End If
                '                            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                '                            'Wtite Log
                '                            'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "ExcuteTran", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Excute SQL command: " & v_strSQL)

                '                        End If
                '                    Next
                '                End If

                '            ElseIf InStr(v_strSQL, "?PHTTQ") > 0 Then
                '                Dim v_TmpDS As DataSet
                '                Dim v_strMsgDate As String
                '                Dim v_strCATYPE As String
                '                v_strSQL = "SELECT COL_VALUE04, COL_VALUE05, COL_VALUE07 FROM TLLOG WHERE AUTOID = " & v_strAUTOID
                '                v_TmpDS = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                '                'Wtite Log
                '                'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "ExcuteTran", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Excute SQL command: " & v_strSQL)

                '                If v_TmpDS.Tables(0).Rows.Count > 0 Then
                '                    If CInt(v_TmpDS.Tables(0).Rows(0)("COL_VALUE07")) = 0 Then
                '                        v_strCATYPE = v_TmpDS.Tables(0).Rows(0)("COL_VALUE05")
                '                        v_strMsgDate = v_TmpDS.Tables(0).Rows(0)("COL_VALUE04")

                '                        v_strSQL = "INSERT INTO CATRAN(AUTOID, TXNUM, TXDATE, ACCTNO, NAMT, BRID, OPERATOR, TLTXCD, MSGDATE)" _
                '                                    & " SELECT SEQ_CATRAN.NEXTVAL, '" & v_strTXNUM & "', TO_DATE('" & v_strTXDATE & "','dd/mm/yyyy'), CAACCTNO," _
                '                                    & " BALANCE, BRID,'-','" & v_strTLTXCD & "', MSGDATE FROM CAMAST WHERE SUBSTR(CAACCTNO,1,3) = '000' AND GET_TOKEN(CAACCTNO,3,'.') = '" & v_strSICODE & "'" _
                '                                    & " AND GET_TOKEN(CAACCTNO,6,'.') = '05' AND GET_TOKEN(CAACCTNO,7,'.') = LPAD(" & v_strCATYPE & ",2,'0') AND TO_CHAR(MSGDATE,'dd/mm/yyyy') = '" & v_strMsgDate & "'"

                '                        v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

                '                        v_strSQL = "DELETE FROM CAMAST WHERE SUBSTR(CAACCTNO,1,3) = '000' AND GET_TOKEN(CAACCTNO,3,'.') = '" & v_strSICODE & "'" _
                '                                    & " AND GET_TOKEN(CAACCTNO,6,'.') = '05' AND GET_TOKEN(CAACCTNO,7,'.') = LPAD(" & v_strCATYPE & ",2,'0') AND TO_CHAR(MSGDATE,'dd/mm/yyyy') = '" & v_strMsgDate & "'"

                '                        v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                '                        'Wtite Log
                '                        'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "ExcuteTran", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Excute SQL command: " & v_strSQL)

                '                    End If
                '                End If
                '            Else
                '                'end thonm
                '                ' thuc hien cau lenh
                '                v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                '                'Wtite Log
                '                'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "ExcuteTran", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Excute SQL command: " & v_strSQL)

                '            End If
                '            ' end tuanta 09/12/2008
                '        Next
                '    Next
                'Else
                For i As Integer = 0 To v_ds.Tables(0).Rows.Count - 1
                    v_strSQL = v_ds.Tables(0).Rows(i)("DMLSQL") & " " & gf_CorrectStringField(v_ds.Tables(0).Rows(i)("DMLSQL1"))
                    ' thay the cac transaction message 
                    ' nhom trang thai giao dich
                    v_strSQL = Replace(v_strSQL, "?STATE_FILE", "'" & v_strStateFile & "'")
                    v_strSQL = Replace(v_strSQL, "?STATUS_TEXT", "'" & v_strSTATUS_TEXT & "'")
                    v_strSQL = Replace(v_strSQL, "?STATUS", v_strSTATUS)
                    ' nhom ma giao dich
                    v_strSQL = Replace(v_strSQL, "?AUTOID", v_strAUTOID)
                    v_strSQL = Replace(v_strSQL, "?TXNUM", "'" & v_strTXNUM & "'")
                    v_strSQL = Replace(v_strSQL, "?TXNAME", "'" & v_strTXNAME & "'")
                    v_strSQL = Replace(v_strSQL, "?TXDATE", "TO_DATE('" & v_strTXDATE & "','dd/mm/yyyy')")

                    'v_strSQL = Replace(v_strSQL, "?BUSDATE", IIf(IsDate(v_strBUSDATE), "TO_DATE('" & v_strBUSDATE & "','dd/mm/yyyy')", "NULL"))
                    If v_strBUSDATE <> "" Then
                        v_strSQL = Replace(v_strSQL, "?BUSDATE", "TO_DATE('" & v_strBUSDATE & " " & v_strTime & "','dd/mm/yyyy hh24:mi:ss')")
                    Else
                        v_strSQL = Replace(v_strSQL, "?BUSDATE", IIf(IsDate(v_strBUSDATE), "TO_DATE('" & v_strBUSDATE & "','dd/mm/yyyy')", "NULL"))
                    End If
                    ' nhom loai giao dich
                    v_strSQL = Replace(v_strSQL, "?TLTXCD", "'" & v_strTLTXCD & "'")
                    v_strSQL = Replace(v_strSQL, "?TXDESC", "'" & v_strTXDESC & "'")
                    ' nhom chi nhanh
                    v_strSQL = Replace(v_strSQL, "?BRID", "'" & v_strBRID & "'")
                    v_strSQL = Replace(v_strSQL, "?BRCODE", "'" & v_strBRCODE & "'")
                    ' nhom user
                    v_strSQL = Replace(v_strSQL, "?TLID", "'" & v_strTLID & "'")
                    v_strSQL = Replace(v_strSQL, "?OFFID", "'" & v_strOFFID & "'")
                    v_strSQL = Replace(v_strSQL, "?CFRID", "'" & v_strCFRID & "'")
                    v_strSQL = Replace(v_strSQL, "?CHKID", "'" & v_strCHKID & "'")
                    v_strSQL = Replace(v_strSQL, "?TLNAME", "'" & v_strTLNAME & "'")
                    v_strSQL = Replace(v_strSQL, "?OFFNAME", "'" & v_strOFFNAME & "'")
                    v_strSQL = Replace(v_strSQL, "?CHKNAME", "'" & v_strCHKNAME & "'")
                    v_strSQL = Replace(v_strSQL, "?CFRNAME", "'" & v_strCFRNAME & "'")
                    ' nhom dia chi tao giao dich
                    v_strSQL = Replace(v_strSQL, "?IPADDRESS", "'" & v_strIPADDRESS & "'")
                    v_strSQL = Replace(v_strSQL, "?WSNAME", "'" & v_strWSNAME & "'")
                    ' nhom quyen
                    v_strSQL = Replace(v_strSQL, "?SICODE", "'" & IIf(v_strSICODE = "", "000", v_strSICODE) & "'")
                    v_strSQL = Replace(v_strSQL, "?MICODE", "'" & IIf(v_strMICODE = "", "000", v_strMICODE) & "'")
                    v_strSQL = Replace(v_strSQL, "?COMICODE", "'" & IIf(v_strCOMICODE = "", "000", v_strCOMICODE) & "'")
                    v_strSQL = Replace(v_strSQL, "?MSGAMT", IIf(v_strMSGAMT <> "", "TO_NUMBER(a.COL_VALUE" & v_strMSGAMT & ")", "0"))
                    ' nhom giao dich cha-con
                    v_strSQL = Replace(v_strSQL, "?ISPARENT", v_strISPARENT)
                    v_strSQL = Replace(v_strSQL, "?PARENTID", v_strPARENTID)
                    v_strSQL = Replace(v_strSQL, "?PARENT_TEXT", "'" & v_strPARENT_TEXT & "'")
                    v_strSQL = Replace(v_strSQL, "?CHILDTLTXCD", "'" & v_strCHILDTLTXCD & "'")
                    v_strSQL = Replace(v_strSQL, "?PARENT_TLTXCD", "'" & v_strCHILDTLTXCD & "'")
                    v_strSQL = Replace(v_strSQL, "?ISBRID", v_strISBRID)
                    v_strSQL = Replace(v_strSQL, "?REASON", v_strREASON)
                    v_strSQL = Replace(v_strSQL, "?TXNOTE", "'" & v_strTXNOTE & "'")
                    v_strSQL = Replace(v_strSQL, "?VSD_BRID", "'" & v_strVsdBrid & "'")
                    v_strSQL = Replace(v_strSQL, "?VSD_2_BRID", "'" & v_strVsdBrid2 & "'")
                    v_strSQL = Replace(v_strSQL, "?T_T_3", v_strT_T_3)
                    'Add by thonm
                    'Thay the cong thuc tinh toan quyen
                    v_strSQL = Replace(v_strSQL, "?CA", v_strCA)
                    v_strSQL = Replace(v_strSQL, "?TN", v_strTN)
                    v_strSQL = Replace(v_strSQL, "?PL", v_strPL)
                    v_strSQL = Replace(v_strSQL, "?LCP", v_strLCP)
                    'End thonm

                    If v_trace_status = "1" And v_ds.Tables(0).Rows(i)("TRACE") = 1 Then
                        Trace.WriteLine("[APPDML - " & v_strTLTXCD & "] " & DateTime.Now & " :" & vbCrLf & " -o- Câu lệnh thứ #" & v_ds.Tables(0).Rows(i)("ORDNUM") & "-o-" & vbCrLf & v_strSQL & vbCrLf)
                        If InStr(v_strSQL.ToUpper, "SELECT") > 0 Then
                            Trace.WriteLine("-o- Dữ liệu câu lệnh thứ #" & v_ds.Tables(0).Rows(i)("ORDNUM") & "-o-")
                            v_strSQLTmp = v_strSQL.Substring(InStr(v_strSQL.ToUpper, "SELECT") - 1)
                            v_trace = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                            Trace.WriteLine(v_trace.GetXml & vbCrLf)
                        End If
                    End If

                    v_strSQLTmp = ""
                    If InStr(v_strSQL, "?TMP_TXFIELDS") > 0 Then
                        Dim v_nodelist1 As Xml.XmlNodeList
                        v_nodelist1 = v_node.SelectNodes("/TransactMessage/fields")
                        If v_nodelist1.Count > 0 Then
                            For ii As Integer = 0 To v_nodelist1.Count - 1
                                For j As Integer = 0 To v_nodelist1.Item(ii).ChildNodes.Count - 1
                                    With v_nodelist1.Item(ii).ChildNodes(j)
                                        v_strSQLTmp = .InnerText
                                        v_obj.ExecuteNonQuery(CommandType.Text, v_strSQLTmp)
                                        'Wtite Log
                                        'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "ExcuteTran", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Excute SQL command: " & v_strSQLTmp)
                                    End With
                                Next
                            Next
                        Else
                            v_strFieldsSQL = ""
                            For j As Integer = 1 To intCountNo
                                v_strFieldsSQL = v_strFieldsSQL & ", COL_VALUE" & String.Format("{0:00}", j) _
                                & ", COL_TYPE" & String.Format("{0:00}", j) _
                                & ", COL_DESC" & String.Format("{0:00}", j)
                            Next
                            v_strSQLTmp = "INSERT INTO TMP_TXFIELDS(AUTOID, TLTXCD " & v_strFieldsSQL & ", real_row) " _
                            & " SELECT SEQ_TMP_TXFIELDS.NEXTVAL, TLTXCD " & v_strFieldsSQL & ", rownum FROM " _
                            & " ( SELECT " _
                            & "'" & v_strTLTXCD & "' TLTXCD" & v_strFieldsSQL _
                            & " FROM  TLLOG " _
                            & " WHERE deleted =0 and " & IIf(v_strISPARENT = "2", "PARENTID = ", "AUTOID = ") & v_strAUTOID _
                            & " )"
                            v_strSQL = v_strSQLTmp
                            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                            'Wtite Log
                            'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "ExcuteTran", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Excute SQL command: " & v_strSQL)
                        End If
                    ElseIf InStr(v_strSQL, "?TLLOG") > 0 Then
                        v_strFieldsSQL = ""
                        For j As Integer = 1 To intCountNo
                            v_strFieldsSQL = v_strFieldsSQL & ", COL_VALUE" & String.Format("{0:00}", j) _
                            & ", COL_TYPE" & String.Format("{0:00}", j) _
                            & ", COL_DESC" & String.Format("{0:00}", j)
                        Next
                        v_strSQL = v_strSQL.Replace("?TLLOG", v_strFieldsSQL)
                        v_strFieldsSQL = ""
                        For j As Integer = 1 To intCountNo
                            v_strFieldsSQL = v_strFieldsSQL & ", a.COL_VALUE" & String.Format("{0:00}", j) _
                            & ", a.COL_TYPE" & String.Format("{0:00}", j) _
                            & ", a.COL_DESC" & String.Format("{0:00}", j)
                        Next
                        v_strSQL = v_strSQL.Replace("?VALUES", v_strFieldsSQL)
                        v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                        'Wtite Log
                        'bangpv: Với giao dịch ký số thì lấy dữ liệu ra để lưu 
                        If v_strIsSignCA = "1" Then
                            If v_strISPARENT = 2 Or v_strISPARENT = 1 Then
                                v_strSQLTmp = "select txnum from tmp_tllog_result where rownum=1"
                                v_trace = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)

                                If v_trace.Tables(0).Rows.Count > 0 Then

                                    v_strSQLTmp = "select autoid, txdate, txnum, brid, tlid, tltxcd, ipaddress, wsname, micode, msgamt, isparent, parentid, brcode, tlname, txname, parent_text, sicode, childtltxcd, msgdate," _
                                                & "col_value01, col_value02, col_value03, col_value04, col_value05, col_value06, col_value07, col_value08, col_value09, col_value10," _
                                                & "col_value11, col_value12, col_value13, col_value14, col_value15, col_value16, col_value17, col_value18, col_value19, col_value20," _
                                                & "col_value21, col_value22, col_value23, col_value24, col_value25, col_value26, col_value27, col_value28, col_value29, col_value30," _
                                                & "isbrid, txnote, vsd_brid from tllog where autoid =to_number('" & v_trace.Tables(0).Rows(0)(0).ToString & "')  union all " _
                                                & "select autoid, txdate, txnum, brid, tlid, tltxcd, ipaddress, wsname, micode, msgamt, isparent, parentid, brcode, tlname, txname, parent_text, sicode, childtltxcd, msgdate," _
                                                & "col_value01, col_value02, col_value03, col_value04, col_value05, col_value06, col_value07, col_value08, col_value09, col_value10, " _
                                                & "col_value11, col_value12, col_value13, col_value14, col_value15, col_value16, col_value17, col_value18, col_value19, col_value20, " _
                                                & "col_value21, col_value22, col_value23, col_value24, col_value25, col_value26, col_value27, col_value28, col_value29, col_value30, " _
                                                & "isbrid, txnote, vsd_brid from tllog where parentid  =to_number('" & v_trace.Tables(0).Rows(0)(0).ToString & "')"
                                    ' tra ve dataset de gui ve client
                                    v_ReturnDS = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                                End If
                            Else
                                v_strSQLTmp = "select autoid, txdate, txnum, brid, tlid, tltxcd, ipaddress, wsname, micode, msgamt, isparent, parentid, brcode, tlname, txname, parent_text, sicode, childtltxcd, msgdate," _
                                            & "col_value01, col_value02, col_value03, col_value04, col_value05, col_value06, col_value07, col_value08, col_value09, col_value10," _
                                            & "col_value11, col_value12, col_value13, col_value14, col_value15, col_value16, col_value17, col_value18, col_value19, col_value20," _
                                            & "col_value21, col_value22, col_value23, col_value24, col_value25, col_value26, col_value27, col_value28, col_value29, col_value30," _
                                            & "isbrid, txnote, vsd_brid from tllog where autoid =to_number('" & v_strPARENTID & "')  union all " _
                                            & "select autoid, txdate ,txnum, brid, tlid, tltxcd, ipaddress, wsname, micode, msgamt, isparent, parentid, brcode, tlname, txname, parent_text, sicode, childtltxcd, msgdate," _
                                            & "col_value01, col_value02, col_value03, col_value04, col_value05, col_value06, col_value07, col_value08, col_value09, col_value10, " _
                                            & "col_value11, col_value12, col_value13, col_value14, col_value15, col_value16, col_value17, col_value18, col_value19, col_value20, " _
                                            & "col_value21, col_value22, col_value23, col_value24, col_value25, col_value26, col_value27, col_value28, col_value29, col_value30, " _
                                            & "isbrid, txnote, vsd_brid from tllog where parentid  =to_number('" & v_strPARENTID & "')"
                                v_ReturnDS = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)

                                'Dim dc As DataColumn
                                'For Each dc In v_ReturnDS.Tables(0).Columns
                                '    dc.ColumnMapping = MappingType.Attribute
                                'Next

                                'Dim v_strtmp As String = v_ReturnDS.GetXml()
                                'v_ReturnDS.WriteXml("c:\Authors.xml")

                                'end bangpv 
                            End If
                        End If
                        'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "ExcuteTran", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Excute SQL command: " & v_strSQL)

                        'add by thonm
                    ElseIf InStr(v_strSQL, "?MFTOTX") > 0 Then
                        'Tinh phi giao dich
                        Dim v_arrMFNO() As String
                        Dim v_int As Integer
                        Dim v_strFomula As String
                        Dim v_dsMF As DataSet
                        Dim v_count As Integer = 0
                        Dim v_strSicode3138 As String = "-1"

                        If Trim(v_strMFNO) <> "" Then
                            v_arrMFNO = v_strMFNO.Split("|")

                            For v_int = 1 To v_arrMFNO.Length - 1
                                'Lay cong thuc tinh phi
                                v_strSQL = "SELECT FOMULA FROM MFTYPE " _
                                              & " WHERE MFNO='" & v_arrMFNO(v_int) & "' AND DELETED=0 AND STATUS=0"
                                v_dsMF = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                                'Wtite Log
                                'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "ExcuteTran", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Excute SQL command: " & v_strSQL)

                                'Neu ton tai cong thuc tinh phi
                                If v_dsMF.Tables(0).Rows.Count > 0 Then
                                    v_strFomula = v_dsMF.Tables(0).Rows(0)(0)
                                    Select Case v_arrMFNO(v_int)
                                        Case "02", "08" '"02": '3023','3024','3038','3039' ; "08": '3086','3087','3092','3093','7021','7022','7027','7028'
                                            If v_strTLTXCD = "3138" Then
                                                v_strSQLTmp = "select min(col_value04) sicode from tllog where parentid =" & v_strAUTOID & " and deleted = 0"
                                                v_dsMF = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                                                v_count = v_dsMF.Tables(0).Rows.Count
                                                If v_count > 0 Then
                                                    If v_count > 1 Then
                                                        v_strSicode3138 = "0"
                                                    Else
                                                        If v_dsMF.Tables(0).Rows(0)(0).ToString = "-1" Then
                                                            v_strSicode3138 = v_dsMF.Tables(0).Rows(0)(0).ToString
                                                        Else
                                                            v_strSicode3138 = "0"
                                                        End If
                                                    End If
                                                End If
                                            End If
                                            v_strSQL = "SELECT mffor FROM TLTX " _
                                              & " WHERE TLTXCD='" & v_strTLTXCD & "' AND '" & v_strSicode3138 & "' = '-1' and DELETED=0 AND STATUS=0"
                                            v_dsMF = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                                            If v_dsMF.Tables(0).Rows.Count > 0 Then
                                                Dim v_strMFFOR As String = v_dsMF.Tables(0).Rows(0)(0)
                                                If v_strMFFOR.Substring(0, 1) = "1" Then
                                                    v_strSQL = "INSERT INTO TMP_TLLOG(txnum, col_value01, col_value02, col_value03,msgamt, col_value04, col_value05, col_value06, col_value07)" _
                                                                  & " SELECT txnum, M || '." & v_arrMFNO(v_int) & "', " & v_strFomula & " , '" _
                                                                  & v_strFomula & "' , X, M micode, '" & v_arrMFNO(v_int) & "', N typeno, S sicode  FROM" _
                                                                  & " (SELECT nvl(c.col_value10,0) R, a.txnum, TO_DATE('" & v_strBUSDATE & "','dd/mm/yyyy') D, b.type T, nvl(a.namt,0) X,a.MICODE M, a.TYPENO N, a.SICODE S, a.brid B " _
                                                                  & " FROM matran a, RGSI b, tllog c WHERE a.sicode=b.sicode and c.autoid=" & v_strAUTOID _
                                                                  & " AND b.deleted=0 and b.status=0 AND a.txnum = lpad(" & v_strAUTOID & ",10,'0') and a.OPERATOR='-')" _
                                                                  & " where " & v_strFomula & " >0 "
                                                    v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                                                End If
                                                If v_strMFFOR.Substring(1, 1) = "1" Then
                                                    v_strSQL = "INSERT INTO TMP_TLLOG(txnum, col_value01, col_value02, col_value03,msgamt, col_value04, col_value05, col_value06, col_value07)" _
                                                                  & " SELECT txnum, M || '." & v_arrMFNO(v_int) & "', " & v_strFomula & " , '" _
                                                                  & v_strFomula & "' , X, M micode, '" & v_arrMFNO(v_int) & "', N typeno, S sicode  FROM" _
                                                                  & " (SELECT nvl(c.col_value10,0) R , a.txnum, TO_DATE('" & v_strBUSDATE & "','dd/mm/yyyy') D, b.type T, nvl(a.namt,0) X,a.MICODE M, a.TYPENO N, a.SICODE S, a.brid B " _
                                                                  & " FROM matran a, RGSI b, tllog c WHERE a.sicode=b.sicode and c.autoid=" & v_strAUTOID _
                                                                  & " AND b.deleted=0 and b.status=0 AND a.txnum = lpad(" & v_strAUTOID & ",10,'0') and a.OPERATOR='+')" _
                                                                  & " where " & v_strFomula & " >0 "
                                                    v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                                                End If
                                                'Else
                                                'v_strSQL = v_strTLTXCD
                                                'v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                                            End If
                                        Case "03" '4081','4082','4083','4084'
                                            v_strSQL = "INSERT INTO TMP_TLLOG(txnum, col_value01, col_value02, col_value03,msgamt, col_value04, col_value05, col_value06, col_value07)" _
                                                          & " SELECT '" & v_strTXNUM & "', M || '." & v_arrMFNO(v_int) & "', " & v_strFomula & " , '" _
                                                          & v_strFomula & "' , H, M micode, '" & v_arrMFNO(v_int) & "', N typeno, S sicode  FROM" _
                                                          & " (select TO_DATE('" & v_strBUSDATE & "','dd/mm/yyyy') D, NULL N, micode M , sicode S, sum(col_value09) H, brid B from tllog " _
                                                          & " where deleted=0 and parentid = " & v_strAUTOID _
                                                          & " group by micode, sicode,brid) " _
                                                          & " where " & v_strFomula & " >0 "
                                            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                                        Case "07" '4008','4009'
                                            If v_strTLTXCD = "4009" Then
                                                'bangpv: 20160117: sua voi dieu kien tinh phi =1 thi moi tinh phi
                                                v_strSQL = "INSERT INTO TMP_TLLOG(txnum, col_value01, col_value02, col_value03,msgamt, col_value04, col_value05, col_value06, col_value07, col_value08)" _
                                                              & " SELECT '" & v_strTXNUM & "', M || '." & v_arrMFNO(v_int) & "', " & v_strFomula & " , '" _
                                                              & v_strFomula & "' , G, M micode, '" & v_arrMFNO(v_int) & "', N typeno, S sicode, 'Các số hiệu lệnh: ' || col_value08  FROM" _
                                                              & " ( select pldt_strcat_agg(to_char(col_value08)) col_value08, D,N,M ,S,B, count(*) G from " _
                                                              & " (select decode(col_value04,'1','SHLM-' || col_value06, 'SHLB-' || col_value07) col_value08, TO_DATE('" & v_strBUSDATE & "','dd/mm/yyyy') D, NULL N, micode M , NULL S, brid B from tllog " _
                                                              & " where col_value16='1' and deleted=0 and parentid = " & v_strAUTOID _
                                                              & " group by decode(col_value04,'1','SHLM-' || col_value06, 'SHLB-' || col_value07), micode,brid) " _
                                                              & " group by D,N,M ,S,B) " _
                                                              & " where " & v_strFomula & " >0 "

                                                v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                                                'ElseIf v_strTLTXCD = "4015" Then
                                                '    v_strSQL = "INSERT INTO TMP_TLLOG(txnum, col_value01, col_value02, col_value03,msgamt, col_value04, col_value05, col_value06, col_value07, col_value08)" _
                                                '                & " SELECT '" & v_strTXNUM & "', M || '." & v_arrMFNO(v_int) & "', " & v_strFomula & " , '" _
                                                '                & v_strFomula & "' , G, M micode, '" & v_arrMFNO(v_int) & "', N typeno, S sicode, 'Các số hiệu lệnh: ' || col_value08  FROM" _
                                                '                & " ( select pldt_strcat_agg(to_char(col_value08)) col_value08, D,N,M ,S,B, count(*) G from " _
                                                '                & " ( select 'SHLB-' || a.col_value17  col_value08 , TO_DATE('" & v_strBUSDATE & "','dd/mm/yyyy') D, NULL N, a.micode M , NULL S, a.brid B from tllog a, rgmi b where a.parentid  =  " & v_strAUTOID _
                                                '                & " and b.deleted = 0 and b.status =0 and b.micode = a.micode and (b.code_trade = substr(a.col_value11,1,3) or b.code_trade = a.col_value15) " _
                                                '                & " group by a.col_value17, a.micode, a.brid " _
                                                '                & " union all " _
                                                '                & " select 'SHLM-' || a.col_value16  col_value08   , TO_DATE('" & v_strBUSDATE & "','dd/mm/yyyy') D, NULL N, a.micode M , NULL S, a.brid B  from tllog a, rgmi b where a.parentid  =  " & v_strAUTOID _
                                                '                & " and b.deleted = 0 and b.status =0 and b.micode = a.micode and (b.code_trade = substr(a.col_value10,1,3) or b.code_trade = a.col_value14) " _
                                                '                & " and not exists (select 1 from tllog c, rgmi d where c.parentid  =  " & v_strAUTOID _
                                                '                & " and d.deleted = 0 and d.status =0 and d.micode = c.micode and (d.code_trade = substr(c.col_value11,1,3) or d.code_trade = c.col_value15) and  a.autoid = c.autoid ) " _
                                                '                & " group by a.col_value16, a.micode, a.brid ) " _
                                                '                & " group by D,N,M ,S,B) " _
                                                '                & " where " & v_strFomula & " >0 "
                                                '    v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                                            End If
                                        Case "04", "05" ' 2001 2002 6043
                                            If v_strTLTXCD = "6044" Then
                                                v_strSQL = "INSERT INTO TMP_TLLOG(txnum, col_value01, col_value02, col_value03,msgamt, col_value04, col_value05, col_value06, col_value07)" _
                                                              & " SELECT '" & v_strTXNUM & "', M || '." & v_arrMFNO(v_int) & "', " & v_strFomula & " , '" _
                                                              & v_strFomula & "' , Q, M micode, '" & v_arrMFNO(v_int) & "', N typeno, S sicode  FROM" _
                                                              & " ( select TO_DATE('" & v_strBUSDATE & "','dd/mm/yyyy') D, NULL N, '000' M , a.SICODE S, a.brid B, 1 J, " _
                                                              & " to_number(a.col_value10) Q, b.type T from tllog a, rgsi b " _
                                                              & " where a.autoid = " & v_strAUTOID & " and b.deleted=0 and b.status=0 and a.sicode=b.sicode ) " _
                                                              & " where " & v_strFomula & " >0 "
                                                v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                                            Else
                                                v_strSQL = "INSERT INTO TMP_TLLOG(txnum, col_value01, col_value02, col_value03,msgamt, col_value04, col_value05, col_value06, col_value07)" _
                                                              & " SELECT '" & v_strTXNUM & "', M || '." & v_arrMFNO(v_int) & "', " & v_strFomula & " , '" _
                                                              & v_strFomula & "' , Q, M micode, '" & v_arrMFNO(v_int) & "', N typeno, S sicode  FROM" _
                                                              & " ( select TO_DATE('" & v_strBUSDATE & "','dd/mm/yyyy') D, NULL N, '000' M , a.SICODE S, a.brid B, to_number(a.col_value07) J " _
                                                              & " , (to_number(a.col_value09)+to_number(a.col_value10)+to_number(a.col_value11)+to_number(a.col_value12)+to_number(a.col_value13)+to_number(a.col_value14)) Q, b.type T, b.PART_VALUE P from tllog a, rgsi b " _
                                                              & " where a.autoid = " & v_strAUTOID & " and b.deleted=0 and b.status=0 and a.sicode=b.sicode ) " _
                                                              & " where " & v_strFomula & " >0 "
                                                v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                                            End If
                                        Case "06" ' 6043,'6044'
                                            v_strSQL = "INSERT INTO TMP_TLLOG(txnum, col_value01, col_value02, col_value03,msgamt, col_value04, col_value05, col_value06, col_value07)" _
                                                          & " SELECT '" & v_strTXNUM & "', M || '." & v_arrMFNO(v_int) & "', " & v_strFomula & " , '" _
                                                          & v_strFomula & "' , I, M micode, '" & v_arrMFNO(v_int) & "', N typeno, S sicode  FROM" _
                                                          & " ( select TO_DATE('" & v_strBUSDATE & "','dd/mm/yyyy') D, NULL N, '000' M , max(SICODE) S, max(brid) B " _
                                                          & " , count(*) I from (select b.iicode, max(b.sicode) sicode, max(b.brid) brid from tllog a, camast b " _
                                                          & " where a.autoid = " & v_strAUTOID & " and a.sicode = b.sicode" _
                                                          & " And a.brid = b.brid And a.msgdate = b.msgdate" _
                                                          & " AND b.rtype = '01' AND b.deleted=0 AND b.status=0  " _
                                                          & " group by b.iicode) ) where " & v_strFomula & " >0 "
                                            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                                        Case "10" ' 6043,'6044'
                                            v_strSQL = "INSERT INTO TMP_TLLOG(txnum, col_value01, col_value02, col_value03,msgamt, col_value04, col_value05, col_value06, col_value07)" _
                                                          & " SELECT '" & v_strTXNUM & "', M || '." & v_arrMFNO(v_int) & "', " & v_strFomula & " , '" _
                                                          & v_strFomula & "' , U, M micode, '" & v_arrMFNO(v_int) & "', N typeno, S sicode  FROM" _
                                                          & " ( select MAX(c.type) T,MAX(c.STOCK_TYPE) Z , TO_DATE('" & v_strBUSDATE & "','dd/mm/yyyy') D, NULL N, '000' M , max(a.SICODE) S, max(a.brid) B " _
                                                          & " , sum(b.balance) U from tllog a, camast b, rgsi c " _
                                                          & " where a.autoid = " & v_strAUTOID & " and a.sicode = b.sicode" _
                                                          & " And a.brid = b.brid And a.msgdate = b.msgdate" _
                                                          & " AND b.rtype = '05' AND b.deleted=0 AND b.status=0 " _
                                                          & " and a.sicode = c.sicode AND c.deleted=0 AND c.status=0) " _
                                                          & " where " & v_strFomula & " >0 "
                                            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                                        Case "11"
                                            If v_strTLTXCD = "4032" Then
                                                v_strSQL = "INSERT INTO TMP_TLLOG(txnum, col_value01, col_value02, col_value03,msgamt, col_value04, col_value05, col_value06, col_value07, col_value08)" _
                                                              & " SELECT '" & v_strTXNUM & "', M || '." & v_arrMFNO(v_int) & "', " & v_strFomula & " , '" _
                                                              & v_strFomula & "' , G, M micode, '" & v_arrMFNO(v_int) & "', N typeno, S sicode, 'Các số hiệu lệnh: ' || col_value08  FROM" _
                                                              & " ( select pldt_strcat_agg(to_char(col_value08)) col_value08, D,N,M ,S,B, count(*) G from " _
                                                              & " (select decode(col_value04,'2','SHLM-' || col_value06, 'SHLB-' || col_value07) col_value08, TO_DATE('" & v_strBUSDATE & "','dd/mm/yyyy') D, NULL N, micode M , NULL S, brid B from tllog " _
                                                              & " where deleted=0 and parentid = " & v_strAUTOID _
                                                              & " group by decode(col_value04,'2','SHLM-' || col_value06, 'SHLB-' || col_value07), micode,brid) " _
                                                              & " group by D,N,M ,S,B) " _
                                                              & " where " & v_strFomula & " >0 "
                                                v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                                                'ElseIf v_strTLTXCD = "4015" Then
                                                '    v_strSQL = "INSERT INTO TMP_TLLOG(txnum, col_value01, col_value02, col_value03,msgamt, col_value04, col_value05, col_value06, col_value07, col_value08)" _
                                                '                & " SELECT '" & v_strTXNUM & "', M || '." & v_arrMFNO(v_int) & "', " & v_strFomula & " , '" _
                                                '                & v_strFomula & "' , G, M micode, '" & v_arrMFNO(v_int) & "', N typeno, S sicode, 'Các số hiệu lệnh: ' || col_value08  FROM" _
                                                '                & " ( select pldt_strcat_agg(to_char(col_value08)) col_value08, D,N,M ,S,B, count(*) G from " _
                                                '                & " ( select 'SHLB-' || a.col_value17  col_value08 , TO_DATE('" & v_strBUSDATE & "','dd/mm/yyyy') D, NULL N, a.micode M , NULL S, a.brid B from tllog a, rgmi b where a.parentid  =  " & v_strAUTOID _
                                                '                & " and b.deleted = 0 and b.status =0 and b.micode = a.micode and (b.code_trade = substr(a.col_value11,1,3) or b.code_trade = a.col_value15) " _
                                                '                & " group by a.col_value17, a.micode, a.brid " _
                                                '                & " union all " _
                                                '                & " select 'SHLM-' || a.col_value16  col_value08   , TO_DATE('" & v_strBUSDATE & "','dd/mm/yyyy') D, NULL N, a.micode M , NULL S, a.brid B  from tllog a, rgmi b where a.parentid  =  " & v_strAUTOID _
                                                '                & " and b.deleted = 0 and b.status =0 and b.micode = a.micode and (b.code_trade = substr(a.col_value10,1,3) or b.code_trade = a.col_value14) " _
                                                '                & " and not exists (select 1 from tllog c, rgmi d where c.parentid  =  " & v_strAUTOID _
                                                '                & " and d.deleted = 0 and d.status =0 and d.micode = c.micode and (d.code_trade = substr(c.col_value11,1,3) or d.code_trade = c.col_value15) and  a.autoid = c.autoid ) " _
                                                '                & " group by a.col_value16, a.micode, a.brid ) " _
                                                '                & " group by D,N,M ,S,B) " _
                                                '                & " where " & v_strFomula & " >0 "
                                                '    v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                                            End If
                                    End Select


                                    'v_strSQL = "INSERT INTO TMP_TLLOG(autoid, txnum, txdate, col_value01, col_value02, col_value03,msgamt, col_value04, col_value05, col_value06, col_value07)" _
                                    '              & " SELECT autoid, txnum, D txdate, M || '." & v_arrMFNO(v_int) & "'," & v_strFomula & " , '" & v_strFomula & "' , X, M micode, '" & v_arrMFNO(v_int) & "', N typeno, S sicode  FROM" _
                                    '              & " (SELECT a.autoid,a.txnum, a.txdate D, b.type T, " & IIf(v_strTLTXCD = "4084", "a.balance", "nvl(a.namt,0)") & " X,a.MICODE M, a.TYPENO N, a.SICODE S, a.brid B " _
                                    '              & " FROM " & IIf(v_strTLTXCD = "4084", "cstran", "matran") & " a, RGSI b WHERE a.sicode=b.sicode" _
                                    '              & " AND b.deleted=0 and b.status=0 AND a.txnum = lpad(" & v_strAUTOID & ",10,'0') " _
                                    '              & IIf(v_strTLTXCD = "4084", " and ((a.tltxcd in ('4081','4082') and a.step='22') or (a.tltxcd='4083' and a.step='1'))", " and a.OPERATOR='-'") & " )"

                                    'Ghi file log
                                    If v_trace_status = "1" And v_ds.Tables(0).Rows(i)("TRACE") = 1 Then
                                        Trace.WriteLine("APPDML - " & v_strTLTXCD & "-o- Câu lệnh thứ :" & v_ds.Tables(0).Rows(i)("ORDNUM") & "-o- Dữ liệu sau khi được tính phí-o-")
                                        v_strSQLTmp = v_strSQL.Substring(InStr(v_strSQL.ToUpper, "SELECT") - 1)
                                        v_trace = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                                        Trace.WriteLine(v_trace.GetXml & vbCrLf)
                                    End If

                                    'Wtite Log
                                    'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "ExcuteTran", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Excute SQL command: " & v_strSQL)

                                End If
                            Next
                        End If

                    ElseIf InStr(v_strSQL, "?PHTTQ") > 0 Then
                        Dim v_TmpDS As DataSet
                        Dim v_strMsgDate As String
                        Dim v_strCATYPE As String
                        v_strSQL = "SELECT COL_VALUE04, COL_VALUE05, COL_VALUE07 FROM TMP_TLLOG"
                        v_TmpDS = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                        'Wtite Log
                        'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "ExcuteTran", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Excute SQL command: " & v_strSQL)

                        If v_TmpDS.Tables(0).Rows.Count > 0 Then
                            If CInt(v_TmpDS.Tables(0).Rows(0)("COL_VALUE07")) = 0 Then
                                v_strCATYPE = v_TmpDS.Tables(0).Rows(0)("COL_VALUE05")
                                v_strMsgDate = v_TmpDS.Tables(0).Rows(0)("COL_VALUE04")
                                'bangpv sua: them voi quyen trai phieu chuyen doi va hoan doi co phieu, thay vi lay sicode, phai lay cosicode 
                                'v_strSQL = "INSERT INTO CATRAN(AUTOID, TXNUM, TXDATE, ACCTNO, NAMT, BRID, OPERATOR, TLTXCD, MSGDATE)" _
                                '           & " SELECT SEQ_CATRAN.NEXTVAL, '" & v_strTXNUM & "', TO_DATE('" & v_strTXDATE & "','dd/mm/yyyy'), CAACCTNO," _
                                '          & " BALANCE, BRID,'-','" & v_strTLTXCD & "', MSGDATE FROM CAMAST WHERE SUBSTR(CAACCTNO,1,3) = '000' AND GET_TOKEN(CAACCTNO,3,'.') = '" & v_strSICODE & "'" _
                                '         & " AND GET_TOKEN(CAACCTNO,6,'.') = '05' AND GET_TOKEN(CAACCTNO,7,'.') = LPAD(" & v_strCATYPE & ",2,'0')  AND TO_CHAR(MSGDATE,'dd/mm/yyyy') = '" & v_strMsgDate & "'"
                                v_strSQL = "INSERT INTO CATRAN(AUTOID, TXNUM, TXDATE, ACCTNO, NAMT, BRID, OPERATOR, TLTXCD, MSGDATE, " _
                                           & " sicode, micode, typeno, odrno, rtype, cano, cosicode,iicode)" _
                                           & " SELECT SEQ_CATRAN.NEXTVAL, '" & v_strTXNUM & "', TO_DATE('" & v_strTXDATE & "','dd/mm/yyyy'), CAACCTNO," _
                                           & " BALANCE, BRID,'-','" & v_strTLTXCD & "', MSGDATE, sicode, micode, typeno, odrno, rtype, cano, cosicode,iicode " _
                                           & " FROM CAMAST WHERE MICODE = '000' AND decode(cano, '09', cosicode, '08', cosicode,SICODE) = '" & v_strSICODE & "'" _
                                           & " AND /*GET_TOKEN(CAACCTNO,6,'.')*/ rtype = '05' AND /*GET_TOKEN(CAACCTNO,7,'.')*/ CANO = LPAD(" & v_strCATYPE & ",2,'0')  AND MSGDATE = to_date('" & v_strMsgDate & "','dd/mm/yyyy')"

                                'end bangpv sua
                                v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                                'Wtite Log
                                'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "ExcuteTran", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Excute SQL command: " & v_strSQL)
                                'bangpv sua: them voi quyen trai phieu chuyen doi va hoan doi co phieu, thay vi lay sicode, phai lay cosicode 
                                'v_strSQL = "DELETE FROM CAMAST WHERE SUBSTR(CAACCTNO,1,3) = '000' AND GET_TOKEN(CAACCTNO,3,'.') = '" & v_strSICODE & "'"
                                '            & " AND GET_TOKEN(CAACCTNO,6,'.') = '05' AND GET_TOKEN(CAACCTNO,7,'.') = LPAD(" & v_strCATYPE & ",2,'0')  AND TO_CHAR(MSGDATE,'dd/mm/yyyy') = '" & v_strMsgDate & "'"
                                v_strSQL = "DELETE FROM CAMAST WHERE MICODE = '000' AND decode(cano, '09', cosicode, '08', cosicode,SICODE) = '" & v_strSICODE & "'" _
                                            & " AND /*GET_TOKEN(CAACCTNO,6,'.')*/ rtype = '05' AND /*GET_TOKEN(CAACCTNO,7,'.')*/ CANO = LPAD(" & v_strCATYPE & ",2,'0')  AND MSGDATE = to_date('" & v_strMsgDate & "','dd/mm/yyyy')"
                                'end bangpv sua
                                v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

                                'Wtite Log
                                'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "ExcuteTran", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Excute SQL command: " & v_strSQL)
                            End If
                        End If
                    Else
                        ' end thonm
                        ' thuc hien cau lenh
                        v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                        'Wtite Log
                        'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "ExcuteTran", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Excute SQL command: " & v_strSQL)

                    End If
                    '' end tuanta 09/12/2008
                Next
                'End If
                ' attache txnum


                If v_strISPARENT = 2 Then
                    v_strSQLTmp = "select txnum from tmp_tllog_result where rownum=1"
                    v_trace = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                    'Wtite Log
                    'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "ExcuteTran", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Excute SQL command: " & v_strSQL)

                    If v_trace.Tables(0).Rows.Count > 0 Then
                        .Attributes(gc_AtributeTXNUM).InnerText = v_trace.Tables(0).Rows(0)(0).ToString
                    End If
                End If


                'ThoNM add
                If v_strTXNUM.Equals("") Then
                    v_strSQLTmp = "select txnum from tmp_tllog_result where rownum=1"
                    v_trace = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                    'Wtite Log
                    'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "ExcuteTran", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Excute SQL command: " & v_strSQL)

                    If v_trace.Tables(0).Rows.Count > 0 Then
                        .Attributes(gc_AtributeTXNUM).InnerText = v_trace.Tables(0).Rows(0)(0).ToString
                    End If
                End If

                'bằngpv: Xử lý với trường hợp có ký CA 
                ' Voi truong hop nhap giao dich
                If v_strIsSignCA = "1" And v_strCAStatus = "0" Then
                    v_strSQLTmp = "select autoid from tmp_tllog_result where rownum=1"
                    v_trace = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)

                    If v_strISPARENT = 2 Or v_strISPARENT = 1 Then
                        v_strCATLLOGID = v_trace.Tables(0).Rows(0)(0).ToString
                        v_strSQLTmp = "insert into tlsession(autoid, tlid, txdate, tllogid,  type," _
                                & "metadigitalsign0) values (seq_tlsession.nextval, '" & v_strTLID & "',TO_DATE('" & v_strTXDATE & "','dd/mm/yyyy'),'" & v_strCATLLOGID & "', " _
                                & "2, '')"
                        v_strCATLLOGID = v_strCATLLOGID & "'" & v_strTLTXCD
                    Else
                        v_strCATLLOGID = v_strPARENTID
                        v_strSQLTmp = "select tltxcd from tltx where deleted =0 and status =0 and childtltxcd ='" & v_strTLTXCD & "'"
                        v_trace = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                        v_strSQLTmp = "update tlsession set txdate =TO_DATE('" & v_strTXDATE & "','dd/mm/yyyy') where tlid ='" & v_strTLID & "'" _
                                        & " and tllogid = '" & v_strCATLLOGID & "'"
                        v_strCATLLOGID = v_strCATLLOGID & "'" & v_trace.Tables(0).Rows(0)(0).ToString
                    End If

                    'insert into tlsession 


                    v_obj.ExecuteNonQuery(CommandType.Text, v_strSQLTmp)
                ElseIf v_strIsSignCA = "1" And v_strCAStatus <> "0" Then
                    'Lay du lieu goc de luu 
                    v_strSQLTmp = "select autoid, txdate, txnum, brid, tlid, tltxcd, ipaddress, wsname, micode, msgamt, isparent, parentid, brcode, tlname, txname, parent_text, sicode, childtltxcd, msgdate," _
                                            & "col_value01, col_value02, col_value03, col_value04, col_value05, col_value06, col_value07, col_value08, col_value09, col_value10," _
                                            & "col_value11, col_value12, col_value13, col_value14, col_value15, col_value16, col_value17, col_value18, col_value19, col_value20," _
                                            & "col_value21, col_value22, col_value23, col_value24, col_value25, col_value26, col_value27, col_value28, col_value29, col_value30," _
                                            & "isbrid, txnote, vsd_brid from tllog where autoid =to_number('" & v_strAUTOID & "')  union all " _
                                            & "select autoid, txdate ,txnum, brid, tlid, tltxcd, ipaddress, wsname, micode, msgamt, isparent, parentid, brcode, tlname, txname, parent_text, sicode, childtltxcd, msgdate," _
                                            & "col_value01, col_value02, col_value03, col_value04, col_value05, col_value06, col_value07, col_value08, col_value09, col_value10, " _
                                            & "col_value11, col_value12, col_value13, col_value14, col_value15, col_value16, col_value17, col_value18, col_value19, col_value20, " _
                                            & "col_value21, col_value22, col_value23, col_value24, col_value25, col_value26, col_value27, col_value28, col_value29, col_value30, " _
                                            & "isbrid, txnote, vsd_brid from tllog where parentid  =to_number('" & v_strAUTOID & "')"
                    v_ReturnDS = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                    ' Luu vao tlsession
                    v_strCATLLOGID = v_strAUTOID
                    Dim v_strTLIDTmp As String
                    v_strTLIDTmp = v_strCFRID
                    If v_strTLIDTmp = "" Then
                        v_strTLIDTmp = v_strOFFID
                    End If
                    If v_strTLIDTmp = "" Then
                        v_strTLIDTmp = v_strCHKID
                    End If
                    'If v_strCAStatus = "1" Then
                    '    v_strTLIDTmp = v_strCHKID
                    'ElseIf v_strCAStatus = "2" Then
                    '    v_strTLIDTmp = v_strOFFID
                    'Else
                    '    v_strTLIDTmp = v_strCFRID
                    'End If
                    v_strSQLTmp = "select tltxcd from tltx where isparent= 2 and deleted =0 and status =0 and childtltxcd ='" & v_strTLTXCD & "'" _
                    & " Union all select tltxcd from tltx where isparent= 1 and deleted =0 and status =0 and tltxcd ='" & v_strTLTXCD & "'"
                    v_trace = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)

                    v_strSQLTmp = "insert into tlsession(autoid, tlid, txdate, tllogid,  type," _
                                & "metadigitalsign0) values (seq_tlsession.nextval, '" & v_strTLIDTmp & "',TO_DATE('" & v_strTXDATE & "','dd/mm/yyyy'),'" & v_strCATLLOGID & "', " _
                                & "2, '')"

                    v_strCATLLOGID = v_strCATLLOGID & "'" & v_trace.Tables(0).Rows(0)(0).ToString
                End If
                If v_trace_status = "1" Then
                    Trace.WriteLine("[Kết thúc: APPDML - " & v_strTLTXCD & "] " & DateTime.Now & vbCrLf)
                    tr2.Close()
                    tr2.Dispose()
                End If

                'Hanm5 them cau check cac bang so du co bi am khong?
                If v_strTblChk <> "" Then
                    Dim v_arrTblChk() As String = v_strTblChk.Split("|")
                    Dim v_strTblChkSql As String = ""
                    Dim v_dsTblChk As DataSet
                    Dim v_strTblChkErr As String = ""
                    Dim v_strTemp As String
                    For i As Integer = 0 To v_arrTblChk.Count - 1
                        Select Case v_arrTblChk(i)
                            Case "IA"
                                v_strTblChkSql = "SELECT COUNT(AUTOID) FROM IAMAST WHERE BRID = '" & v_strBRID & "' AND BALANCE < 0"
                            Case "MA"
                                v_strTblChkSql = "SELECT COUNT(AUTOID) FROM MAMAST WHERE BRID = '" & v_strBRID & "' AND BALANCE < 0"
                            Case "CA"
                                v_strTblChkSql = "SELECT COUNT(AUTOID) FROM CAMAST WHERE BRID = '" & v_strBRID & "' AND BALANCE < 0"
                            Case "RA"
                                v_strTblChkSql = "SELECT COUNT(AUTOID) FROM RAMAST WHERE BRID = '" & v_strBRID & "' AND BALANCE < 0"
                            Case "SF"
                                v_strTblChkSql = "SELECT COUNT(AUTOID) FROM SFMAST WHERE BRID = '" & v_strBRID & "' AND BALANCE < 0"
                            Case "MF"
                                v_strTblChkSql = "SELECT COUNT(AUTOID) FROM MFMAST WHERE BRID = '" & v_strBRID & "' AND BALANCE < 0"
                        End Select
                        v_dsTblChk = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strTblChkSql)
                        'Wtite Log
                        'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "ExcuteTran", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Excute SQL command: " & v_strSQL)

                        If v_dsTblChk.Tables(0).Rows.Count > 0 Then
                            If CInt(v_dsTblChk.Tables(0).Rows(0)(0)) > 0 Then
                                v_strTblChkErr = v_strTblChkErr & "," & v_arrTblChk(i)
                            End If
                        End If
                    Next
                    If v_strTblChkErr <> "" Then
                        v_strTblChkErr = Mid(v_strTblChkErr, 2, Len(v_strTblChkErr) - 1)
                        If blnTran Then
                            v_obj.Rollback()
                            blnTran = False
                        End If
                        If v_trace_status = "1" Then
                            tr2.Close()
                            tr2.Dispose()
                        End If
                        Return ERR_SYSTEM_START
                    End If
                End If
                'Hanm5: end
                'bangpv: test .zip
                
                'Dim f As New IO.FileInfo(v_strAppPath & "data\" & v_strBRID & "_" & v_strTxdate1 & ".xml")
                'end bangpv
                'Start Myvq
                If (v_strSTATUS.Trim = "3") And (v_strTLTXCD.Trim = "1112") Then

                    v_strSQL = "SELECT A.COL_VALUE01 FROM TLLOG A" _
                                        & " WHERE A.TXNUM='" & v_strTXNUM & "'"
                    v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                    'Wtite Log
                    'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "ExcuteTran", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Excute SQL command: " & v_strSQL)

                    If v_ds.Tables(0).Rows.Count > 0 Then
                        Dim v_strBranchId1 As String = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE01"))
                        v_strSQL = "SELECT varvalue COL_VALUE01 from SYSVAR where varname ='CURRDATE' and brid =" + v_strBranchId1
                        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                        Dim v_strBridCurrdate1 As String = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE01"))
                        v_lngErr = SaveFile(v_strBranchId1, v_strBridCurrdate1, "1112")
                    End If

                End If
                '1132
                If (v_strSTATUS.Trim = "3") And (v_strTLTXCD.Trim = "1132") Then

                    v_strSQL = "SELECT A.COL_VALUE01 FROM TLLOG A" _
                                        & " WHERE A.TXNUM='" & v_strTXNUM & "'"
                    v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                    'Wtite Log
                    'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "ExcuteTran", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Excute SQL command: " & v_strSQL)

                    If v_ds.Tables(0).Rows.Count > 0 Then
                        Dim v_strBranchId1 As String = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE01"))
                        v_strSQL = "SELECT varvalue COL_VALUE01 from SYSVAR where varname ='CURRDATE' and brid =" + v_strBranchId1
                        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                        Dim v_strBridCurrdate1 As String = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE01"))
                        v_lngErr = SaveFile(v_strBranchId1, v_strBridCurrdate1, "1132")
                    End If

                End If

                If (v_strSTATUS.Trim = "3") And (v_strTLTXCD.Trim = "1114" Or v_strTLTXCD.Trim = "1125" Or v_strTLTXCD = "1123" Or v_strTLTXCD = "1124" Or v_strTLTXCD = "1150") Then

                    v_strSQL = "SELECT A.COL_VALUE01, A.COL_VALUE02 FROM TLLOG A" _
                                        & " WHERE A.TXNUM='" & v_strTXNUM & "'"
                    v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                    'Wtite Log
                    'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "ExcuteTran", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Excute SQL command: " & v_strSQL)

                    If v_ds.Tables(0).Rows.Count > 0 Then
                        Dim v_strBranchId1 As String = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE01"))
                        Dim v_strBridCurrdate1 As String = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE02"))
                        v_lngErr = SaveFile(v_strBranchId1, v_strBridCurrdate1, v_strTLTXCD.Trim)
                    End If

                End If
                If (v_strSTATUS.Trim = "3") And (v_strTLTXCD.Trim = "1150") Then

                    v_strSQL = "SELECT A.COL_VALUE01, A.COL_VALUE02, a.COL_VALUE03 FROM TLLOG A" _
                                        & " WHERE A.TXNUM='" & v_strTXNUM & "'"
                    v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                    'Wtite Log
                    'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "ExcuteTran", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Excute SQL command: " & v_strSQL)

                    If v_ds.Tables(0).Rows.Count > 0 Then
                        Dim v_strBranchId1 As String = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE01"))
                        Dim v_strBridCurrdate1 As String = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE02"))
                        Dim v_strCsType As String = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE03"))
                        v_lngErr = SaveFile(v_strBranchId1 & "|" & v_strCsType, v_strBridCurrdate1, v_strTLTXCD.Trim)
                    End If

                End If

                'Hanm5: Them phan lay file tien gui tu SBL sang NHTT
                If v_strSTATUS.Trim = "3" And (v_strTLTXCD.Trim = "2150" Or v_strTLTXCD = "2151") Then
                    v_strSQL = "SELECT A.COL_VALUE01, A.COL_VALUE02, A.COL_VALUE04 FROM TLLOG A" _
                                        & " WHERE A.TXNUM='" & v_strTXNUM & "'"
                    v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                    If v_ds.Tables(0).Rows.Count > 0 Then
                        Dim v_strBranchId1 As String = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE01"))
                        Dim v_strBridCurrdate1 As String = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE02"))
                        Dim v_strFileName As String = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE04"))
                        v_lngErr = SaveFileSblFtpBnk(v_strBranchId1, v_strBridCurrdate1, v_strFileName.Trim, v_strTLTXCD.Trim)
                    End If
                End If
                'Hanm5: Them phan lay gia CK
                If v_strSTATUS.Trim = "3" And v_strTLTXCD.Trim = "2130" Then
                    v_strSQL = "SELECT a.col_value01, a.col_value02, a.col_value03 FROM TLLOG A WHERE A.TXNUM = '" & v_strTXNUM & "'"
                    v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                    If v_ds.Tables(0).Rows.Count > 0 Then
                        'Lay thi truong
                        Dim v_strTradeDate1 As String = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE01")).Replace("/", "")
                        Dim v_strBranchId1 As String = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE02"))
                        Dim v_str2130Type As String = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE03"))
                        LogError.Write("EXECUTE_TRAN - Error source: " & "2130:" & vbNewLine _
                         & "Error code: System error!" & vbNewLine _
                         & "Error message: " & "v_strTradeDate1=" & v_strTradeDate1 & ",v_strBranchId1=" & v_strBranchId1 & ",v_str2130Type=" & v_str2130Type, EventLogEntryType.Information, gc_MODULE_HOST)
                        If v_strBranchId1 = "0002" Then
                            Dim v_strRootPath As String = ""
                            Dim v_strRemotePath As String = ""
                            v_obj.GetSysVar("VSDFTPSVR", "RootPath", v_strBranchId1, v_strRootPath)
                            v_obj.GetSysVar("VSDFTPSVR", "RemotePath", v_strBranchId1, v_strRemotePath)
                            LogError.Write("EXECUTE_TRAN - Error source: " & "2130:" & vbNewLine _
                                           & "Error code: System error!" & vbNewLine _
                                           & "Error message: " & "v_strRootPath=" & v_strRootPath & ",v_strRemotePath=" & v_strRemotePath, EventLogEntryType.Information, gc_MODULE_HOST)
                            v_lngErr = ServerBussinessCA.ExtractFile2130(System.AppDomain.CurrentDomain.BaseDirectory & "price\", _
                                    v_strRootPath & "\" & v_strRemotePath & "\" & "2130" & v_strBranchId1 & v_strTradeDate1 & ".xml", v_strBranchId1)
                        Else
                            Dim v_strAppPath As String = System.AppDomain.CurrentDomain.BaseDirectory
                            Dim v_strTradeDate2 As String = DateTime.ParseExact(v_strTradeDate1, "ddMMyyyy", Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd", Globalization.CultureInfo.InvariantCulture)
                            LogError.Write("EXECUTE_2130_1: " & "v_strAppPath=" & v_strAppPath & vbNewLine _
                                                            & "v_strTradeDate2=" & v_strTradeDate2 & vbNewLine _
                                                            & "v_str2130Type=" & v_str2130Type, _
                                           EventLogEntryType.Information, gc_MODULE_HOST)
                            'Neu la lay gia dong cua
                            If v_str2130Type = "1" Then
                                'Lay gia CK
                                LogError.Write("EXECUTE_2130_2: " & "v_strAppPath=" & v_strAppPath & vbNewLine _
                                                            & "v_strTradeDate2=" & v_strTradeDate2 & vbNewLine _
                                                            & "v_str2130Type=" & v_str2130Type, _
                                           EventLogEntryType.Information, gc_MODULE_HOST)

                                v_lngErr = GetFtpFromHNX(v_obj, "HNX_CL_PR", v_strBranchId1, "HNX_CLOSE_PRICE", v_strTradeDate2)
                                If v_lngErr <> ERR_SYSTEM_OK Then
                                    Return v_lngErr
                                    Exit Function
                                End If
                                System.Threading.Thread.Sleep(30 * 1000)

                                If (Not ServerBussinessCA.DecryptPrice_HNX(v_strAppPath & "price\" & "HNX_CLOSE_PRICE_" & v_strTradeDate2, "HNX_CLOSE_PRICE_" & v_strTradeDate2)) Then
                                    Return ERR_SYSTEM_START
                                    Exit Function
                                End If
                                If File.Exists(v_strAppPath & "price\" & "HNX_CLOSE_PRICE_" & v_strTradeDate2 & ".zip") Then
                                    Dim mv_xzipEngine As New ZipEngine
                                    Dim f1 As New IO.FileInfo(v_strAppPath & "price\" & "HNX_CLOSE_PRICE_" & v_strTradeDate2 & ".xml")
                                    If f1.Exists Then
                                        f1.Delete()
                                    End If

                                    mv_xzipEngine.UnzipFile(v_strAppPath & "price\", "HNX_CLOSE_PRICE_" & v_strTradeDate2 & ".zip", "HNX_CLOSE_PRICE_" & v_strTradeDate2 & ".xml")
                                    v_obj.Commit()
                                    blnTran = False
                                    Return ERR_SYSTEM_OK
                                Else
                                    Return ERR_SYSTEM_START
                                    Exit Function
                                End If
                            ElseIf v_str2130Type = "2" Then
                                'Lay gia CK
                                v_lngErr = GetFtpFromHNX(v_obj, "HNX_PRICE", v_strBranchId1, "HNX_STOCKS_PRICE", v_strTradeDate2)
                                If v_lngErr <> ERR_SYSTEM_OK Then
                                    Return v_lngErr
                                    Exit Function
                                End If
                                If (Not ServerBussinessCA.DecryptPrice_HNX(v_strAppPath & "price\" & "HNX_STOCKS_PRICE_" & v_strTradeDate2, "HNX_STOCKS_PRICE_" & v_strTradeDate2)) Then
                                    Return ERR_SYSTEM_START
                                    Exit Function
                                End If
                                If File.Exists(v_strAppPath & "price\" & "HNX_STOCKS_PRICE_" & v_strTradeDate2 & ".zip") Then
                                    Dim mv_xzipEngine As New ZipEngine
                                    Dim f As New IO.FileInfo(v_strAppPath & "price\" & "HNX_STOCKS_PRICE_" & v_strTradeDate2 & ".xml")
                                    If f.Exists Then
                                        f.Delete()
                                    End If
                                    mv_xzipEngine.UnzipFile(v_strAppPath & "price\", "HNX_STOCKS_PRICE_" & v_strTradeDate2 & ".zip", "HNX_STOCKS_PRICE_" & v_strTradeDate2 & ".xml")
                                    v_obj.Commit()
                                    blnTran = False
                                    Return ERR_SYSTEM_OK
                                Else
                                    Return ERR_SYSTEM_START
                                    Exit Function
                                End If
                            ElseIf v_str2130Type = "3" Then
                                'Lay chi so CK
                                v_lngErr = GetFtpFromHNX(v_obj, "HNX_IDX", v_strBranchId1, "HNX_IDX_STOCKS", v_strTradeDate2)
                                If v_lngErr <> ERR_SYSTEM_OK Then
                                    Return v_lngErr
                                    Exit Function
                                End If
                                If (Not ServerBussinessCA.DecryptPrice_HNX(v_strAppPath & "price\" & "HNX_IDX_STOCKS_" & v_strTradeDate2, "HNX_IDX_STOCKS_" & v_strTradeDate2)) Then
                                    Return ERR_SYSTEM_START
                                    Exit Function
                                End If
                                If File.Exists(v_strAppPath & "price\" & "HNX_IDX_STOCKS_" & v_strTradeDate2 & ".zip") Then
                                    Dim mv_xzipEngine As New ZipEngine
                                    Dim f As New IO.FileInfo(v_strAppPath & "price\" & "HNX_IDX_STOCKS_" & v_strTradeDate2 & ".xml")
                                    If f.Exists Then
                                        f.Delete()
                                    End If
                                    mv_xzipEngine.UnzipFile(v_strAppPath & "price\", "HNX_IDX_STOCKS_" & v_strTradeDate2 & ".zip", "HNX_IDX_STOCKS_" & v_strTradeDate2 & ".xml")
                                    v_obj.Commit()
                                    blnTran = False
                                    Return ERR_SYSTEM_OK
                                Else
                                    Return ERR_SYSTEM_START
                                    Exit Function
                                End If
                            ElseIf v_str2130Type = "4" Then
                                'Lay gia TP
                                v_lngErr = GetFtpFromHNX(v_obj, "HNX_BOND", v_strBranchId1, "HNX_BOND_PRICE_YC", v_strTradeDate2)
                                If v_lngErr <> ERR_SYSTEM_OK Then
                                    Return v_lngErr
                                    Exit Function
                                End If
                                If (Not ServerBussinessCA.DecryptPrice_HNX(v_strAppPath & "price\" & "HNX_BOND_PRICE_YC_" & v_strTradeDate2, "HNX_BOND_PRICE_YC_" & v_strTradeDate2)) Then
                                    Return ERR_SYSTEM_START
                                    Exit Function
                                End If
                                If File.Exists(v_strAppPath & "price\" & "HNX_BOND_PRICE_YC_" & v_strTradeDate2 & ".zip") Then
                                    Dim mv_xzipEngine As New ZipEngine
                                    Dim f As New IO.FileInfo(v_strAppPath & "price\" & "HNX_BOND_PRICE_YC_" & v_strTradeDate2 & ".xml")
                                    If f.Exists Then
                                        f.Delete()
                                    End If
                                    mv_xzipEngine.UnzipFile(v_strAppPath & "price\", "HNX_BOND_PRICE_YC_" & v_strTradeDate2 & ".zip", "HNX_BOND_PRICE_YC_" & v_strTradeDate2 & ".xml")
                                    v_obj.Commit()
                                    blnTran = False
                                    Return ERR_SYSTEM_OK

                                Else
                                    Return ERR_SYSTEM_START
                                    Exit Function
                                End If
                                'Rổ trái phiếu chuyển đổi 
                            ElseIf v_str2130Type = "5" Then
                                v_lngErr = GetFtpFromHNX(v_obj, "HNXBONDBAS", v_strBranchId1, "HNX_BOND_BASKET", v_strTradeDate2)
                                If v_lngErr <> ERR_SYSTEM_OK Then
                                    Return v_lngErr
                                    Exit Function
                                End If
                                If (Not ServerBussinessCA.DecryptPrice_HNX(v_strAppPath & "price\" & "HNX_BOND_BASKET_" & v_strTradeDate2, "HNX_BOND_BASKET_" & v_strTradeDate2)) Then
                                    Return ERR_SYSTEM_START
                                    Exit Function
                                End If
                                If File.Exists(v_strAppPath & "price\" & "HNX_BOND_BASKET_" & v_strTradeDate2 & ".zip") Then
                                    Dim mv_xzipEngine As New ZipEngine
                                    Dim f As New IO.FileInfo(v_strAppPath & "price\" & "HNX_BOND_BASKET_" & v_strTradeDate2 & ".xml")
                                    If f.Exists Then
                                        f.Delete()
                                    End If
                                    mv_xzipEngine.UnzipFile(v_strAppPath & "price\", "HNX_BOND_BASKET_" & v_strTradeDate2 & ".zip", "HNX_BOND_BASKET_" & v_strTradeDate2 & ".xml")
                                    v_obj.Commit()
                                    blnTran = False
                                    Return ERR_SYSTEM_OK

                                Else
                                    Return ERR_SYSTEM_START
                                    Exit Function
                                End If
                                'chỉ số đóng cửa trái phiếu 
                            ElseIf v_str2130Type = "6" Then
                                v_lngErr = GetFtpFromHNX(v_obj, "HNXBONDIDX", v_strBranchId1, "HNX_BOND_INDEX", v_strTradeDate2)
                                If v_lngErr <> ERR_SYSTEM_OK Then
                                    Return v_lngErr
                                    Exit Function
                                End If
                                If (Not ServerBussinessCA.DecryptPrice_HNX(v_strAppPath & "price\" & "HNX_BOND_INDEX_" & v_strTradeDate2, "HNX_BOND_INDEX_" & v_strTradeDate2)) Then
                                    Return ERR_SYSTEM_START
                                    Exit Function
                                End If
                                If File.Exists(v_strAppPath & "price\" & "HNX_BOND_INDEX_" & v_strTradeDate2 & ".zip") Then
                                    Dim mv_xzipEngine As New ZipEngine
                                    Dim f As New IO.FileInfo(v_strAppPath & "price\" & "HNX_BOND_INDEX_" & v_strTradeDate2 & ".xml")
                                    If f.Exists Then
                                        f.Delete()
                                    End If
                                    mv_xzipEngine.UnzipFile(v_strAppPath & "price\", "HNX_BOND_INDEX_" & v_strTradeDate2 & ".zip", "HNX_BOND_INDEX_" & v_strTradeDate2 & ".xml")
                                    v_obj.Commit()
                                    blnTran = False
                                    Return ERR_SYSTEM_OK

                                Else
                                    Return ERR_SYSTEM_START
                                    Exit Function
                                End If
                            ElseIf v_str2130Type = "7" Then
                                v_lngErr = GetFtpFromHNX(v_obj, "HNXIDXCCP", v_strBranchId1, "HNX_INDEX", v_strTradeDate2)
                                If v_lngErr <> ERR_SYSTEM_OK Then
                                    Return v_lngErr
                                    Exit Function
                                End If
                                If (Not ServerBussinessCA.DecryptPrice_HNX(v_strAppPath & "price\" & "HNX_INDEX_" & v_strTradeDate2, "HNX_INDEX_" & v_strTradeDate2)) Then
                                    Return ERR_SYSTEM_START
                                    Exit Function
                                End If
                                If File.Exists(v_strAppPath & "price\" & "HNX_INDEX_" & v_strTradeDate2 & ".zip") Then
                                    Dim mv_xzipEngine As New ZipEngine
                                    Dim f As New IO.FileInfo(v_strAppPath & "price\" & "HNX_INDEX_" & v_strTradeDate2 & ".xml")
                                    If f.Exists Then
                                        f.Delete()
                                    End If
                                    mv_xzipEngine.UnzipFile(v_strAppPath & "price\", "HNX_INDEX_" & v_strTradeDate2 & ".zip", "HNX_INDEX_" & v_strTradeDate2 & ".xml")
                                    v_obj.Commit()
                                    blnTran = False
                                    Return ERR_SYSTEM_OK
                                Else
                                    Return ERR_SYSTEM_START
                                    Exit Function
                                End If
                            ElseIf v_str2130Type = "8" Then
                                v_lngErr = GetFtpFromHNX(v_obj, "HNX_BYC", v_strBranchId1, "HNX_BOND_YC_DATA", v_strTradeDate2)
                                If v_lngErr <> ERR_SYSTEM_OK Then
                                    Return v_lngErr
                                    Exit Function
                                End If
                                If (Not ServerBussinessCA.DecryptPrice_HNX(v_strAppPath & "price\" & "HNX_BOND_YC_DATA_" & v_strTradeDate2, "HNX_BOND_YC_DATA_" & v_strTradeDate2)) Then
                                    Return ERR_SYSTEM_START
                                    Exit Function
                                End If
                                If File.Exists(v_strAppPath & "price\" & "HNX_BOND_YC_DATA_" & v_strTradeDate2 & ".zip") Then
                                    Dim mv_xzipEngine As New ZipEngine
                                    Dim f As New IO.FileInfo(v_strAppPath & "price\" & "HNX_BOND_YC_DATA_" & v_strTradeDate2 & ".xml")
                                    If f.Exists Then
                                        f.Delete()
                                    End If
                                    mv_xzipEngine.UnzipFile(v_strAppPath & "price\", "HNX_BOND_YC_DATA_" & v_strTradeDate2 & ".zip", "HNX_BOND_YC_DATA_" & v_strTradeDate2 & ".xml")
                                    v_obj.Commit()
                                    blnTran = False
                                    Return ERR_SYSTEM_OK
                                Else
                                    Return ERR_SYSTEM_START
                                    Exit Function
                                End If

                            End If
                        End If
                    End If
                End If
                If (v_strSTATUS.Trim = "3") And (v_strTLTXCD.Trim = "1113") Then

                    v_strSQL = "SELECT A.COL_VALUE01 FROM TLLOG A" _
                                        & " WHERE A.TXNUM='" & v_strTXNUM & "'"
                    v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                    'Wtite Log
                    'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "ExcuteTran", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Excute SQL command: " & v_strSQL)

                    If v_ds.Tables(0).Rows.Count > 0 Then
                        Dim v_strBranchId1 As String = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE01"))
                        v_strSQL = "SELECT varvalue COL_VALUE01 from SYSVAR where varname ='CURRDATE' and brid =" + v_strBranchId1
                        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                        Dim v_strBridCurrdate1 As String = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE01"))
                        v_strBridCurrdate1 = v_strBridCurrdate1.Replace("/", "")
                        'v_lngErr = ExtractFile(v_strBranchId1, v_strBridCurrdate1)
                        v_strSQL = "SELECT varvalue COL_VALUE01 FROM sysvar a where a.GRNAME like 'VSDFTPSVR' and varname = 'RootPath' and brid='" + v_strBranchId1 + "'"
                        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                        Dim v_strRootPath As String = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE01"))

                        v_strSQL = "SELECT varvalue COL_VALUE01 FROM sysvar a where a.GRNAME like 'VSDFTPSVR' and varname = 'RemotePath' and brid='" + v_strBranchId1 + "'"
                        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                        Dim v_strRemotePath As String = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE01"))
                        Dim v_lstFileName As List(Of String) = New List(Of String)
                        If v_strBranchId1 = "0002" Then
                            v_lngErr = ServerBussinessCA.ExtractFile1113(System.AppDomain.CurrentDomain.BaseDirectory & "data\", _
                                    v_strRootPath & "\" & v_strRemotePath & "\" & "1113" & v_strBranchId1 & v_strBridCurrdate1 & ".xml", v_strBranchId1, v_lstFileName)
                            'Read file to database
                            Dim v_strSettUpdType As String = ""
                            v_obj.GetSysVar("SYSTEM", "SETT_UPD_TYPE", v_strBranchId1, v_strSettUpdType)
                            If v_strSettUpdType = "3.0" Or v_strSettUpdType = "4.3" Then
                                For Each v_strFile In v_lstFileName
                                    Dim v_strArgs As String = ""
                                    Dim v_intResult As Integer = ERR_SYSTEM_START
                                    If v_strFile.ToUpper().Contains("ASTDL") Then
                                        v_strArgs = """" & v_strFile & """ TXFIELDS_ASTDL 0000000000 " & v_strTXDATE & " " & v_strBUSDATE
                                    ElseIf v_strFile.ToUpper().Contains("ASTPT") Then
                                        v_strArgs = """" & v_strFile & """ TXFIELDS_ASTPT 0000000000 " & v_strTXDATE & " " & v_strBUSDATE
                                    End If
                                    If v_strArgs <> "" Then
                                        v_intResult = RunExternalProgram("ReadHoseDataTool.exe", v_strArgs)
                                    End If
                                    If v_intResult <> ERR_SYSTEM_OK Then
                                        v_obj.Rollback()
                                        Return v_intResult
                                        Exit Function
                                    End If
                                Next
                            End If
                        Else
                            'lấy file từ ftp server về 
                            Dim v_DataAccess As New DataAccess
                            Dim v_strServerAddress, v_strServerAddress1, v_strServerPort, v_strUsername, v_strPassword As String
                            v_DataAccess.GetSysVar("EXPORT", "ServerAddress", v_strBRID, v_strServerAddress)
                            v_DataAccess.GetSysVar("EXPORT", "ServerAddress1", v_strBRID, v_strServerAddress1)
                            v_DataAccess.GetSysVar("EXPORT", "ServerPort", v_strBRID, v_strServerPort)
                            v_DataAccess.GetSysVar("EXPORT", "Username", v_strBRID, v_strUsername)
                            v_DataAccess.GetSysVar("EXPORT", "Password", v_strBRID, v_strPassword)
                            v_DataAccess.GetSysVar("EXPORT", "RemotePath", v_strBRID, v_strRemotePath)
                            v_DataAccess.GetSysVar("VSDFTPSVR", "RootPath", v_strBRID, v_strRootPath)
                            Dim v_oWriter As System.IO.StreamWriter
                            Dim v_strAppPath As String = System.AppDomain.CurrentDomain.BaseDirectory
                            If File.Exists(v_strAppPath & "data\" & v_strBRID & "_" & v_strTXDATE & ".bat") Then
                                File.Delete(v_strAppPath & "data\" & v_strBRID & "_" & v_strTXDATE & ".bat")
                            End If
                            Dim v_Prefix As String
                            Dim v_strTxdate1 As String = Replace(v_strTXDATE, "/", "")
                            Select Case v_strBRID
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
                            End Select

                            v_oWriter = New StreamWriter(v_strAppPath & "data\" & v_strBRID & "_" & v_strTxdate1 & ".bat")
                            v_oWriter.WriteLine("@ftp -i -s:""%~f0""&GOTO:EOF")
                            v_oWriter.WriteLine("open " & v_strServerAddress)
                            v_oWriter.WriteLine(v_strUsername)
                            v_oWriter.WriteLine(v_strPassword)
                            v_oWriter.WriteLine("lcd " & """" & v_strAppPath & "data" & """")
                            v_oWriter.WriteLine("cd " & v_strRemotePath)
                            v_oWriter.WriteLine("binary")

                            v_oWriter.WriteLine("get " & v_Prefix & "_TRADING_RESULT" & v_strTxdate1 & ".zip " & v_Prefix & "_TRADING_RESULT" & v_strTxdate1 & ".zip")

                            v_oWriter.WriteLine("bye" & vbCrLf)
                            v_oWriter.Close()
                            Dim v_oProcess As Process
                            v_oProcess = New Process

                            v_oProcess.StartInfo.FileName = v_strAppPath & "data\" & v_strBRID & "_" & v_strTxdate1 & ".bat" 'v_strAppPath & "data\" & v_strBrid & "_" & v_strTxdate & ".bat"
                            v_oProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
                            v_oProcess.StartInfo.CreateNoWindow = True
                            v_oProcess.Start()
                            'v_oProcess.WaitForExit()
                            v_oProcess.Close()
                            ' Lấy từ đầu 2 
                            v_oWriter = New StreamWriter(v_strAppPath & "data\" & v_strBRID & "_" & v_strTxdate1 & "1.bat")
                            v_oWriter.WriteLine("@ftp -i -s:""%~f0""&GOTO:EOF")
                            v_oWriter.WriteLine("open " & v_strServerAddress1)
                            v_oWriter.WriteLine(v_strUsername)
                            v_oWriter.WriteLine(v_strPassword)
                            v_oWriter.WriteLine("lcd " & """" & v_strAppPath & "data" & """")
                            v_oWriter.WriteLine("cd " & v_strRemotePath)
                            v_oWriter.WriteLine("binary")

                            v_oWriter.WriteLine("get " & v_Prefix & "_TRADING_RESULT" & v_strTxdate1 & ".zip " & v_Prefix & "_TRADING_RESULT" & v_strTxdate1 & ".zip")

                            v_oWriter.WriteLine("bye" & vbCrLf)
                            v_oWriter.Close()
                            v_oProcess = New Process

                            v_oProcess.StartInfo.FileName = v_strAppPath & "data\" & v_strBRID & "_" & v_strTxdate1 & "1.bat" 'v_strAppPath & "data\" & v_strBrid & "_" & v_strTxdate & ".bat"
                            v_oProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
                            v_oProcess.StartInfo.CreateNoWindow = True
                            v_oProcess.Start()
                            'v_oProcess.WaitForExit()
                            v_oProcess.Close()
                            System.Threading.Thread.Sleep(30 * 1000)

                            If Not ServerBussinessCA.DecryptTrade_HNX(v_strAppPath & "data\" & v_Prefix & "_TRADING_RESULT" & v_strTxdate1, v_Prefix & "_TRADING_RESULT" & v_strTxdate1) Then
                                Return -1
                                Exit Function

                            End If
                            If File.Exists(v_strAppPath & "data\" & v_Prefix & "_TRADING_RESULT" & v_strTxdate1 & ".zip") Then
                                Dim mv_xzipEngine As New ZipEngine
                                mv_xzipEngine.UnzipFile(v_strAppPath & "data", v_Prefix & "_TRADING_RESULT" & v_strTxdate1 & ".zip", v_Prefix & "_TRADING_RESULT" & v_strTxdate1 & ".xml")
                                Dim f As New IO.FileInfo(v_strAppPath & "data\" & v_strBRID & "_" & v_strTxdate1 & ".xml")
                                If f.Exists Then
                                    f.Delete()
                                End If
                                Rename(v_strAppPath & "data\" & v_Prefix & "_TRADING_RESULT" & v_strTxdate1 & ".xml", v_strAppPath & "data\" & v_strBRID & "_" & v_strTxdate1 & ".xml")

                                v_obj.Commit()
                                blnTran = False
                                Return 0

                            Else
                                Return -1

                                Exit Function
                            End If
                        End If
                    End If

                End If
                'Added by Thanglv9 - 13/04/2013
                If (v_strSTATUS.Trim = "3") And (v_strTLTXCD.Trim = "1131") Then

                    v_strSQL = "SELECT A.BRID COL_VALUE01, A.COL_VALUE02, a.col_value04 FROM TLLOG A" _
                                        & " WHERE A.TXNUM='" & v_strTXNUM & "'"
                    v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                    'Wtite Log
                    'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "ExcuteTran", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Excute SQL command: " & v_strSQL)

                    If v_ds.Tables(0).Rows.Count > 0 Then
                        Dim v_strBranchId1 As String = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE01"))
                        Dim v_strBridCurrdate1 As String = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE02")) & gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE04"))

                        v_lngErr = SaveFile(v_strBranchId1, v_strBridCurrdate1, v_strTLTXCD.Trim)
                    End If

                End If
                'End Thanglv9
                'BangPV 1130
                If (v_strSTATUS.Trim = "3") And (v_strTLTXCD.Trim = "1130") Then

                    v_strSQL = "SELECT A.COL_VALUE01 FROM TLLOG A" _
                                        & " WHERE A.TXNUM='" & v_strTXNUM & "'"
                    v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                    'Wtite Log
                    'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "ExcuteTran", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Excute SQL command: " & v_strSQL)

                    If v_ds.Tables(0).Rows.Count > 0 Then
                        Dim v_strBranchId1 As String = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE01"))
                        v_strSQL = "SELECT varvalue COL_VALUE01, to_char(to_date(varvalue,'DD/MM/YYYY'),'Q') V_QUARTER,to_char(to_date(varvalue,'DD/MM/YYYY'),'YYYY') V_YEAR from SYSVAR where varname ='CURRDATE' and brid =" + v_strBranchId1
                        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                        Dim v_strBridCurrdate1 As String = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE01"))
                        'lay nam, quy
                        Dim v_strQuarter As String = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("V_QUARTER"))
                        Dim v_strYear As String = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("V_YEAR"))

                        v_strBridCurrdate1 = v_strBridCurrdate1.Replace("/", "")
                        'v_lngErr = ExtractFile(v_strBranchId1, v_strBridCurrdate1)
                        v_strSQL = "SELECT varvalue COL_VALUE01 FROM sysvar a where a.GRNAME like 'VSDFTPSVR' and varname = 'RootPath' and brid='" + v_strBranchId1 + "'"
                        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                        Dim v_strRootPath As String = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE01"))

                        v_strSQL = "SELECT varvalue COL_VALUE01 FROM sysvar a where a.GRNAME like 'VSDFTPSVR' and varname = 'RemotePath' and brid='" + v_strBranchId1 + "'"
                        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                        Dim v_strRemotePath As String = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE01"))
                        'lấy file từ ftp server về 
                        Dim v_DataAccess As New DataAccess
                        Dim v_strServerAddress, v_strServerAddress1, v_strServerPort, v_strUsername, v_strPassword As String
                        v_DataAccess.GetSysVar("EXPORT", "ServerAddress", v_strBRID, v_strServerAddress)
                        v_DataAccess.GetSysVar("EXPORT", "ServerAddress1", v_strBRID, v_strServerAddress1)
                        v_DataAccess.GetSysVar("EXPORT", "ServerPort", v_strBRID, v_strServerPort)
                        v_DataAccess.GetSysVar("EXPORT", "Username", v_strBRID, v_strUsername)
                        v_DataAccess.GetSysVar("EXPORT", "Password", v_strBRID, v_strPassword)
                        v_DataAccess.GetSysVar("EXPORT", "RemotePath", v_strBRID, v_strRemotePath)
                        v_DataAccess.GetSysVar("VSDFTPSVR", "RootPath", v_strBRID, v_strRootPath)
                        Dim v_oWriter As System.IO.StreamWriter
                        Dim v_strAppPath As String = System.AppDomain.CurrentDomain.BaseDirectory
                        If File.Exists(v_strAppPath & "data\" & v_strBRID & "_" & v_strTXDATE & ".bat") Then
                            File.Delete(v_strAppPath & "data\" & v_strBRID & "_" & v_strTXDATE & ".bat")
                        End If
                        'Dim v_Prefix As String
                        Dim v_strTxdate1 As String = Replace(v_strTXDATE, "/", "")
                        ''Select Case v_strBRID
                        ''    Case "0001"
                        ''        v_Prefix = "LISTED"
                        ''    Case "0003"
                        ''        v_Prefix = "UPCOM"
                        ''    Case "0004"
                        ''        v_Prefix = "BOND"
                        ''    Case "0005"
                        ''        v_Prefix = "USDBOND"
                        ''    Case "0006"
                        ''        v_Prefix = "BILL_VND"
                        'End Select

                        v_oWriter = New StreamWriter(v_strAppPath & "data\" & v_strBRID & "_" & v_strTxdate1 & ".bat")
                        v_oWriter.WriteLine("@ftp -i -s:""%~f0""&GOTO:EOF")
                        v_oWriter.WriteLine("open " & v_strServerAddress)
                        v_oWriter.WriteLine(v_strUsername)
                        v_oWriter.WriteLine(v_strPassword)
                        v_oWriter.WriteLine("lcd " & """" & v_strAppPath & "data" & """")
                        v_oWriter.WriteLine("cd " & v_strRemotePath)
                        v_oWriter.WriteLine("binary")

                        v_oWriter.WriteLine("get " & "HNX_LTD_SHD_" & v_strYear & "_" & v_strQuarter & ".zip " & " HNX_LTD_SHD_" & v_strYear & "_" & v_strQuarter & ".zip")

                        v_oWriter.WriteLine("bye" & vbCrLf)
                        v_oWriter.Close()
                        Dim v_oProcess As Process
                        v_oProcess = New Process

                        v_oProcess.StartInfo.FileName = v_strAppPath & "data\" & v_strBRID & "_" & v_strTxdate1 & ".bat" 'v_strAppPath & "data\" & v_strBrid & "_" & v_strTxdate & ".bat"
                        v_oProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
                        v_oProcess.StartInfo.CreateNoWindow = True
                        v_oProcess.Start()
                        'v_oProcess.WaitForExit()
                        v_oProcess.Close()
                        ' Lấy từ đầu 2 
                        v_oWriter = New StreamWriter(v_strAppPath & "data\" & v_strBRID & "_" & v_strTxdate1 & "1.bat")
                        v_oWriter.WriteLine("@ftp -i -s:""%~f0""&GOTO:EOF")
                        v_oWriter.WriteLine("open " & v_strServerAddress1)
                        v_oWriter.WriteLine(v_strUsername)
                        v_oWriter.WriteLine(v_strPassword)
                        v_oWriter.WriteLine("lcd " & """" & v_strAppPath & "data" & """")
                        v_oWriter.WriteLine("cd " & v_strRemotePath)
                        v_oWriter.WriteLine("binary")

                        v_oWriter.WriteLine("get " & "HNX_LTD_SHD_" & v_strYear & "_" & v_strQuarter & ".zip " & " HNX_LTD_SHD_" & v_strYear & "_" & v_strQuarter & ".zip")

                        v_oWriter.WriteLine("bye" & vbCrLf)
                        v_oWriter.Close()
                        v_oProcess = New Process

                        v_oProcess.StartInfo.FileName = v_strAppPath & "data\" & v_strBRID & "_" & v_strTxdate1 & "1.bat" 'v_strAppPath & "data\" & v_strBrid & "_" & v_strTxdate & ".bat"
                        v_oProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
                        v_oProcess.StartInfo.CreateNoWindow = True
                        v_oProcess.Start()
                        'v_oProcess.WaitForExit()
                        v_oProcess.Close()
                        System.Threading.Thread.Sleep(30 * 1000)

                        If Not ServerBussinessCA.DecryptFreeFloat_HNX(v_strAppPath & "data\" & "HNX_LTD_SHD_" & v_strYear & "_" & v_strQuarter, "HNX_LTD_SHD_" & v_strYear & "_" & v_strQuarter) Then
                            Return -1
                            Exit Function

                        End If
                        If File.Exists(v_strAppPath & "data\" & "HNX_LTD_SHD_" & v_strYear & "_" & v_strQuarter & ".zip") Then
                            Dim mv_xzipEngine As New ZipEngine
                            mv_xzipEngine.UnzipFiles(v_strAppPath & "data", "HNX_LTD_SHD_" & v_strYear & "_" & v_strQuarter & ".zip")
                            Dim f As New IO.FileInfo(v_strAppPath & "data\" & v_strBRID & "_" & v_strTxdate1 & ".xml")
                            If f.Exists Then
                                f.Delete()
                            End If
                            'Rename(v_strAppPath & "data\" & v_Prefix & "_TRADING_RESULT" & v_strTxdate1 & ".xml", v_strAppPath & "data\" & v_strBRID & "_" & v_strTxdate1 & ".xml")

                            v_obj.Commit()
                            blnTran = False
                            Return 0

                        Else
                            Return -1

                            Exit Function
                        End If
                    End If

                End If


                'End Myvq
                If v_lngErr = ERR_SYSTEM_START Then
                    v_obj.Rollback()
                    Exit Function
                End If
                v_obj.Commit()
                blnTran = False
                ''Wtite Log
                'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "ExcuteTran", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Excute SQL command: Commit")
            End With

            Return v_lngErr
        Catch ex As Exception
            'Wtite Log
            'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "ExcuteTran", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Error: " & ex.ToString)
            If blnTran Then
                v_obj.Rollback()
            End If
            If v_trace_status = "1" Then
                'Trace.WriteLine("[Kết thúc: APPDML - " & v_strTLTXCD & "] " & DateTime.Now & vbCrLf)
                tr2.Close()
                tr2.Dispose()
            End If
            ex.Source = "Host.txRouter.Transact"
            LogError.Write("EXECUTE_TRAN - Error source: " & ex.Source & vbNewLine _
                         & "Error code: System error!" & vbNewLine _
                         & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_HOST)
            Return ERR_SYSTEM_START
        Finally
            'WriteLog
            'mv_lwLogWriter.StopWriteLog(gc_MODULE_HOST, "Host", "txRouter", "ExcuteTran", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "")

            If InStr("4001|4081|4082|4083|4003|4004|4005|4006|4007|4020", v_strPARENT_TLTXCD) > 0 And v_strPARENT_TLTXCD <> "" And v_strOLDSTATUS = "0" Then
                v_nodeList = v_node.SelectNodes("/TransactMessage/fields")
                If v_nodeList.Count > 0 Then
                    For i = 0 To v_nodeList.Count - 1
                        v_node.RemoveChild(v_nodeList.Item(i))
                    Next
                End If
            End If
            If Not v_ds Is Nothing Then
                v_ds.Dispose()
            End If
            If Not v_trace Is Nothing Then
                v_trace.Dispose()
            End If
            If Not v_replace Is Nothing Then
                v_replace.Dispose()
            End If
            If Not v_obj Is Nothing Then
                v_obj.Dispose()
            End If
        End Try
    End Function

    Function RunExternalProgram(ByVal fileName As String, ByVal args As String) As Integer
        Try
            Dim returnValue As String = "0"
            Dim info As ProcessStartInfo = New ProcessStartInfo(fileName)
            info.UseShellExecute = False
            info.Arguments = args
            info.RedirectStandardInput = True
            info.RedirectStandardOutput = True
            info.CreateNoWindow = True

            Dim process As Process = process.Start(info)

            Dim sr As StreamReader = process.StandardOutput
            returnValue = sr.ReadToEnd().ToString()
            Dim strLines() As String = returnValue.Split(New String() {Environment.NewLine}, StringSplitOptions.None)
            If strLines.Length > 0 Then
                For Each line As String In strLines
                    If line = "OK" Then
                        Return ERR_SYSTEM_OK
                    End If
                Next    
                LogError.Write("RunExternalProgram - Error message: " & returnValue, EventLogEntryType.Error, gc_MODULE_HOST)
                Return ERR_SYSTEM_START
            Else
                Return ERR_SYSTEM_START
            End If
        Catch ex As Exception
            LogError.Write("GetFtpFromHNX - Error source: " & ex.Source & vbNewLine _
                         & "Error code: System error!" & vbNewLine _
                         & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_HOST)
            Return ERR_SYSTEM_START
        End Try
    End Function

    Function GetFtpFromHNX(ByRef pv_obj As DataAccess, ByVal pv_strGrName As String, ByVal pv_strBrid As String, _
                          ByVal pv_strFileName As String, ByVal pv_strTradeDate As String)
        Dim v_strServerAddress, v_strServerPort, v_strUsername, v_strPassword, v_strRemotePath As String
        Dim v_strSql As String = ""
        Dim v_ds As DataSet
        Try
            v_strSql = "SELECT max(ServerAddress) ServerAddress, max(ServerPort) ServerPort, " _
                        & " max(Username) Username, max(Password) Password, max(RemotePath) RemotePath " _
                        & " FROM " _
                        & " (SELECT decode(varname, 'ServerAddress', varvalue, '') ServerAddress, " _
                        & " decode(varname, 'ServerPort', varvalue, '') ServerPort, " _
                        & " decode(varname, 'Username', varvalue, '') Username, " _
                        & " decode(varname, 'Password', varvalue, '') Password, " _
                        & " decode(varname, 'RemotePath', varvalue, '') RemotePath " _
                        & " FROM sysvar WHERE grname = '" & pv_strGrName & "' AND brid = '" & pv_strBrid & "')"
            v_ds = pv_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSql)
            If v_ds.Tables(0).Rows.Count > 0 Then
                v_strServerAddress = v_ds.Tables(0).Rows(0)("ServerAddress").ToString()
                v_strServerPort = v_ds.Tables(0).Rows(0)("ServerPort").ToString()
                v_strUsername = v_ds.Tables(0).Rows(0)("Username").ToString()
                v_strPassword = v_ds.Tables(0).Rows(0)("Password").ToString()
                v_strRemotePath = v_ds.Tables(0).Rows(0)("RemotePath").ToString()
            Else
                Return ERR_SYSTEM_START
            End If
            LogError.Write("EXECUTE_2130_3: " & "v_strServerAddress=" & v_strServerAddress & vbNewLine _
                                                            & "v_strServerPort=" & v_strServerPort & vbNewLine _
                                                            & "v_strUsername=" & v_strUsername & vbNewLine _
                                                            & "v_strPassword=" & v_strPassword & vbNewLine _
                                                            & "v_strRemotePath=" & v_strRemotePath, _
                                           EventLogEntryType.Information, gc_MODULE_HOST)
            Dim v_oWriter As System.IO.StreamWriter
            Dim v_strAppPath As String = System.AppDomain.CurrentDomain.BaseDirectory
            If File.Exists(v_strAppPath & "price\" & pv_strGrName & "_" & pv_strBrid & "_" & pv_strTradeDate & ".bat") Then
                File.Delete(v_strAppPath & "price\" & pv_strGrName & "_" & pv_strBrid & "_" & pv_strTradeDate & ".bat")
            End If

            LogError.Write("EXECUTE_2130_4: " & "Bat File Name=" & v_strAppPath & "price\" & pv_strGrName & "_" & pv_strBrid & "_PRICE_" & pv_strTradeDate & ".bat", _
                                           EventLogEntryType.Information, gc_MODULE_HOST)
            'Lay file ve
            v_oWriter = New StreamWriter(v_strAppPath & "price\" & pv_strGrName & "_" & pv_strBrid & "_PRICE_" & pv_strTradeDate & ".bat")
            v_oWriter.WriteLine("@ftp -i -s:""%~f0""&GOTO:EOF")
            v_oWriter.WriteLine("open " & v_strServerAddress)
            v_oWriter.WriteLine(v_strUsername)
            v_oWriter.WriteLine(v_strPassword)
            v_oWriter.WriteLine("lcd " & """" & v_strAppPath & "price" & """")
            v_oWriter.WriteLine("cd " & v_strRemotePath)
            v_oWriter.WriteLine("binary")
            v_oWriter.WriteLine("get " & pv_strFileName & "_" & pv_strTradeDate & ".zip " & pv_strFileName & "_" & pv_strTradeDate & ".zip")
            v_oWriter.WriteLine("bye" & vbCrLf)
            v_oWriter.Close()

            Dim v_oProcess As New Process
            v_oProcess.StartInfo.FileName = v_strAppPath & "price\" & pv_strGrName & "_" & pv_strBrid & "_PRICE_" & pv_strTradeDate & ".bat"
            v_oProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
            v_oProcess.StartInfo.CreateNoWindow = True
            v_oProcess.Start()
            v_oProcess.WaitForExit(10 * 1000)
            v_oProcess.Close()

            Return ERR_SYSTEM_OK
        Catch ex As Exception
            LogError.Write("GetFtpFromHNX - Error source: " & ex.Source & vbNewLine _
                         & "Error code: System error!" & vbNewLine _
                         & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_HOST)
            Return ERR_SYSTEM_START
        End Try
    End Function
    Private Function GetTransCA(ByRef v_node As Xml.XmlNode, ByRef v_trace As DataSet, ByRef v_strISPARENT As String) As Long
        Dim blnTran As Boolean = False

        Dim v_strMFNO As String
        Dim v_ds, v_replace As DataSet
        Dim v_blnStockType As Boolean = False
        Dim v_strAUTOID As String
        Dim v_strSTATUS As String = "0"
        Dim v_strTXNUM As String
        Dim v_strTXDATE As String
        Dim v_lngErr As Long = ERR_SYSTEM_OK
        Dim v_obj As New DataAccess
        Dim tr2 As TextWriterTraceListener
        Dim v_trace_status, v_trace_path As String
        'bangpv

        Dim v_nodeList As Xml.XmlNodeList
        Dim v_strPARENT_TLTXCD As String = v_node.Attributes(gc_AtributePARENT_TLTXCD).Value.ToString
        Dim v_strOLDSTATUS As String = v_node.Attributes(gc_AtributeOLDSTATUS).Value.ToString
        Dim v_strIsSignCA As String = v_node.Attributes(gc_AtributeSIGNCA).Value.ToString
        'Dim v_strSignatureClient As String = v_node.Attributes(gc_AtributeSignatureClient).Value.ToString
        Try

            v_obj.NewDBInstance(gc_MODULE_HOST)
            With v_node
                v_obj.BeginTran()
                blnTran = True
                'Get message header
                Dim v_strBRID As String = .Attributes(gc_AtributeBRID).Value.ToString
                v_strSTATUS = .Attributes(gc_AtributeSTATUS).Value.ToString
                Dim v_strSTATUS_TEXT As String = .Attributes(gc_AtributeSTATUSTEXT).Value.ToString
                v_strAUTOID = .Attributes(gc_AtributeAUTOID).Value.ToString
                v_strTXNUM = .Attributes(gc_AtributeTXNUM).Value.ToString
                v_strTXDATE = .Attributes(gc_AtributeTXDATE).Value.ToString
                Dim v_strBUSDATE As String = .Attributes(gc_AtributeBUSDATE).Value.ToString
                Dim v_strTLTXCD As String = .Attributes(gc_AtributeTLTXCD).Value.ToString
                Dim v_strTXDESC As String = .Attributes(gc_AtributeTXDESC).Value.ToString
                Dim v_strBRCODE As String = .Attributes(gc_AtributeBRCODE).Value.ToString
                Dim v_strTLID As String = .Attributes(gc_AtributeTLID).Value.ToString
                Dim v_strCHKID As String = .Attributes(gc_AtributeCHKID).Value.ToString
                Dim v_strOFFID As String = .Attributes(gc_AtributeOFFID).Value.ToString
                Dim v_strCFRID As String = .Attributes(gc_AtributeCFRID).Value.ToString
                Dim v_strTLNAME As String = .Attributes(gc_AtributeTLNAME).Value.ToString
                Dim v_strCHKNAME As String = .Attributes(gc_AtributeCHKNAME).Value.ToString
                Dim v_strOFFNAME As String = .Attributes(gc_AtributeOFFNAME).Value.ToString
                Dim v_strCFRNAME As String = .Attributes(gc_AtributeCFRNAME).Value.ToString
                Dim v_strIPADDRESS As String = .Attributes(gc_AtributeIPADDRESS).Value.ToString
                Dim v_strWSNAME As String = .Attributes(gc_AtributeWSNAME).Value.ToString
                Dim v_strSICODE As String = .Attributes(gc_AtributeSICODE).Value.ToString
                Dim v_strMICODE As String = .Attributes(gc_AtributeMICODE).Value.ToString
                Dim v_strCOMICODE As String = .Attributes(gc_AtributeCOMICODE).Value.ToString
                Dim v_strMSGAMT As String = .Attributes(gc_AtributeMSGAMT).Value.ToString
                v_strISPARENT = .Attributes(gc_AtributeISPARENT).Value.ToString
                Dim v_strPARENTID As String = .Attributes(gc_AtributePARENTID).Value.ToString
                Dim v_strPARENT_TEXT As String = .Attributes(gc_AtributePARENT_TEXT).Value.ToString
                Dim v_strTXNAME As String = .Attributes(gc_AtributeTXNAME).Value.ToString
                Dim v_strCHILDTLTXCD As String = .Attributes(gc_AtributeCHILDTLTXCD).Value.ToString
                Dim v_strISBRID As String = .Attributes(gc_AtributeISBRID).Value.ToString
                Dim v_strREASON As String = .Attributes(gc_AtributeREASON).Value.ToString
                'hanm5
                Dim v_strTXNOTE As String = .Attributes(gc_AtributeTXNOTE).Value.ToString
                Dim v_strVsdBrid As String = .Attributes(gc_AtributeVSDBRID).Value.ToString
                Dim v_strVsdBrid2 As String '= .Attributes(gc_AtributeVSDBRID2).Value.ToString
                Dim v_strTblChk As String = .Attributes(gc_AtributeTBLCHK).Value.ToString

                Dim v_strSYSVAR As String = ""
                Dim v_strCurrDate1 As String

                mv_strIpAddress = v_strIPADDRESS
                mv_strWsName = v_strWSNAME
                mv_strTellerName = v_strCFRNAME
                If mv_strTellerName = "" Then
                    mv_strTellerName = v_strOFFNAME
                    mv_strTellerId = v_strOFFID
                End If
                If mv_strTellerName = "" Then
                    mv_strTellerName = v_strCHKNAME
                    mv_strTellerId = v_strCHKID
                End If
                If mv_strTellerName = "" Then
                    mv_strTellerName = v_strTLNAME
                    mv_strTellerId = v_strTLID
                End If

                v_obj.GetSysVar("SYSTEM", "TRACE_STATUS", v_trace_status)
                v_obj.GetSysVar("SYSTEM", "TRACE_PATH", v_trace_path)
                v_obj.GetSysVar("SYSTEM", "CURRDATE", v_strBRID, v_strCurrDate1)

                Dim v_strSQL, v_strReplaceSQL, v_strSQLTmp, v_strFieldsSQL, v_strValuesSQL As String

                ' date : 14/10/2008
                ' Purpose : get from  date T-3 to date T
                Dim v_strT_T_3 As String
                v_strSQL = "select to_char(GET_T_PLUS(to_date('" & v_strCurrDate1 & "', 'dd/mm/yyyy'),'" & v_strBRID & "',-1),'dd/mm/yyyy') txdate from dual" _
                        & " union select to_char(GET_T_PLUS(to_date('" & v_strCurrDate1 & "', 'dd/mm/yyyy'),'" & v_strBRID & "',-2),'dd/mm/yyyy') txdate from dual" _
                        & " union select to_char(GET_T_PLUS(to_date('" & v_strCurrDate1 & "', 'dd/mm/yyyy'),'" & v_strBRID & "',-3),'dd/mm/yyyy') txdate from dual"
                v_trace = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

                v_strT_T_3 = "to_date('" & v_strCurrDate1 & "', 'dd/mm/yyyy'),"
                For i = 0 To v_trace.Tables(0).Rows.Count - 1
                    v_strT_T_3 = v_strT_T_3 & "to_date('" & v_trace.Tables(0).Rows(i)("txdate") & "', 'dd/mm/yyyy'),"
                Next
                v_strT_T_3 = "(" & Left(v_strT_T_3, v_strT_T_3.Length - 1) & ")"
                ' end 
                ' date : 14/10/2010
                ' Purpose : get from  hour from sysdate
                v_strSQL = "select to_char(sysdate, 'hh24:mi:ss') time from dual"
                v_trace = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                Dim v_strTime As String = v_trace.Tables(0).Rows(0)("time")
                'end


                If v_trace_status = "1" Then
                    Dim v_strTLName1 As String = v_strCFRNAME
                    Dim v_trace_path1 As String = ""
                    If v_strTLName1 = "" Then
                        v_strTLName1 = v_strOFFNAME
                    End If
                    If v_strTLName1 = "" Then
                        v_strTLName1 = v_strCHKNAME
                    End If
                    If v_strTLName1 = "" Then
                        v_strTLName1 = v_strTLNAME
                    End If
                    v_strCurrDate1 = Replace(v_strCurrDate1, "/", "_")
                    If v_trace_path = "" Then
                        Dim v_app As New ApplicationServices.ApplicationBase
                        v_trace_path1 = v_app.Info.DirectoryPath & "\Log\" & v_strCurrDate1
                    Else
                        v_trace_path1 = v_trace_path & v_strCurrDate1
                    End If

                    If Not System.IO.Directory.Exists(v_trace_path) Then
                        System.IO.Directory.CreateDirectory(v_trace_path1)
                    End If

                    v_trace_path1 &= "\log_dml_br" & v_strBRID & "_" & v_strTLName1 & ".txt"

                    tr2 = New TextWriterTraceListener(System.IO.File.AppendText(v_trace_path1))
                    Trace.Listeners.Add(tr2)
                End If
                'Nếu ngày trong branch khác với ngày hệ thống thì báo lỗi yêu cầu thoát ra vào lại hệ thống
                Dim v_strCURRDATE As String = ""
                Dim v_bCmd As New BusinessCommand

                Dim intCountNo As Integer = 0

                'Add by thonm
                'Lay cong thuc tinh toan quyen thay the vao SQL
                'Su dung voi GD 6004, 6010
                Dim v_strCA As String = ""
                Dim v_strTN As String = ""
                Dim v_strPL As String = ""
                Dim v_strLCP As String = ""
                Select Case v_strTLTXCD
                    Case "6038"
                        v_strSQL = "SELECT A.COL_VALUE10, A.COL_VALUE12, A.COL_VALUE13, A.COL_VALUE07 FROM TLLOG A, TLLOG B" _
                                        & " WHERE A.TXNUM = B.COL_VALUE01 AND B.AUTOID='" & v_strAUTOID & "'"
                        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                        'Wtite Log
                        'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "ExcuteTran", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Excute SQL command: " & v_strSQL)

                        If v_ds.Tables(0).Rows.Count > 0 Then
                            v_strCA = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE10"))
                            v_strTN = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE12"))
                            v_strPL = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE13"))
                            v_strLCP = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE07"))
                        Else
                            v_strSQL = "SELECT A.COL_VALUE10, A.COL_VALUE12, A.COL_VALUE13, A.COL_VALUE07 FROM TLLOGALL A, TLLOG B" _
                                        & " WHERE A.TXNUM = B.COL_VALUE01 AND B.AUTOID='" & v_strAUTOID & "'"
                            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                            'Wtite Log
                            'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "ExcuteTran", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Excute SQL command: " & v_strSQL)

                            If v_ds.Tables(0).Rows.Count > 0 Then
                                v_strCA = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE10"))
                                v_strTN = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE12"))
                                v_strPL = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE13"))
                                v_strLCP = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE07"))
                            End If
                        End If
                    Case "6034"
                        v_strSQL = "SELECT A.COL_VALUE07, A.COL_VALUE08, A.COL_VALUE09, A.COL_VALUE10 FROM TLLOG A" _
                                            & " WHERE A.PARENTID='" & v_strAUTOID & "'"
                        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                        'Wtite Log
                        'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "ExcuteTran", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Excute SQL command: " & v_strSQL)

                        If v_ds.Tables(0).Rows.Count > 0 Then
                            v_strCA = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE07"))
                            v_strTN = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE08"))
                            v_strPL = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE09"))
                            v_strLCP = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE10"))
                        End If
                End Select

                'Lay loai hinh phi GD
                v_strSQL = "SELECT MFNO FROM TLTX WHERE TLTXCD='" & v_strTLTXCD & "' AND DELETED=0 AND STATUS=0"
                v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                'Wtite Log
                'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "ExcuteTran", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Excute SQL command: " & v_strSQL)

                If v_ds.Tables(0).Rows.Count > 0 Then
                    v_strMFNO = gf_CorrectStringField(v_ds.Tables(0).Rows(0)(0))
                End If
                'end thonm    
                v_strSQL = "select ordnum, trace, DMLSQL, DMLSQL1 from appdml where status = 0 and deleted = 0 and ((tltxcd = '" & v_strTLTXCD & "') or (tltxcd is null)) and instr(txstatus,'" & v_strSTATUS & "')>0"


                v_strSQL = "select * from (" & v_strSQL & " ) order by ordnum"

                v_bCmd.ExecuteUser = v_strTLID
                v_bCmd.SQLCommand = v_strSQL
                v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_bCmd.SQLCommand)
                'Wtite Log
                'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "ExcuteTran", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Excute SQL command: " & v_strSQL)

                If v_ds.Tables(0).Rows.Count > 0 Then
                    v_strReplaceSQL = "select max(fldname) from fldmaster where objname = '" & v_strTLTXCD & "' and deleted=0 and status=0"
                    v_bCmd.SQLCommand = v_strReplaceSQL
                    v_replace = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_bCmd.SQLCommand)
                    If v_replace.Tables(0).Rows.Count > 0 Then
                        intCountNo = v_replace.Tables(0).Rows(0)(0)
                    End If
                End If

                If v_trace_status = "1" Then
                    Trace.WriteLine("[Bắt đầu: APPDML - " & v_strTLTXCD & "] " & DateTime.Now & vbCrLf)
                End If
                ' thay the cac tham so can thiet 
                For i As Integer = 0 To v_ds.Tables(0).Rows.Count - 1
                    v_strSQL = v_ds.Tables(0).Rows(i)("DMLSQL") & " " & gf_CorrectStringField(v_ds.Tables(0).Rows(i)("DMLSQL1"))
                    ' thay the cac transaction message 
                    ' nhom trang thai giao dich
                    v_strSQL = Replace(v_strSQL, "?STATUS_TEXT", "'" & v_strSTATUS_TEXT & "'")
                    v_strSQL = Replace(v_strSQL, "?STATUS", v_strSTATUS)
                    ' nhom ma giao dich
                    v_strSQL = Replace(v_strSQL, "?AUTOID", v_strAUTOID)
                    v_strSQL = Replace(v_strSQL, "?TXNUM", "'" & v_strTXNUM & "'")
                    v_strSQL = Replace(v_strSQL, "?TXNAME", "'" & v_strTXNAME & "'")
                    v_strSQL = Replace(v_strSQL, "?TXDATE", "TO_DATE('" & v_strTXDATE & "','dd/mm/yyyy')")

                    'v_strSQL = Replace(v_strSQL, "?BUSDATE", IIf(IsDate(v_strBUSDATE), "TO_DATE('" & v_strBUSDATE & "','dd/mm/yyyy')", "NULL"))
                    If v_strBUSDATE <> "" Then
                        v_strSQL = Replace(v_strSQL, "?BUSDATE", "TO_DATE('" & v_strBUSDATE & " " & v_strTime & "','dd/mm/yyyy hh24:mi:ss')")
                    Else
                        v_strSQL = Replace(v_strSQL, "?BUSDATE", IIf(IsDate(v_strBUSDATE), "TO_DATE('" & v_strBUSDATE & "','dd/mm/yyyy')", "NULL"))
                    End If
                    ' nhom loai giao dich
                    v_strSQL = Replace(v_strSQL, "?TLTXCD", "'" & v_strTLTXCD & "'")
                    v_strSQL = Replace(v_strSQL, "?TXDESC", "'" & v_strTXDESC & "'")
                    ' nhom chi nhanh
                    v_strSQL = Replace(v_strSQL, "?BRID", "'" & v_strBRID & "'")
                    v_strSQL = Replace(v_strSQL, "?BRCODE", "'" & v_strBRCODE & "'")
                    ' nhom user
                    v_strSQL = Replace(v_strSQL, "?TLID", "'" & v_strTLID & "'")
                    v_strSQL = Replace(v_strSQL, "?OFFID", "'" & v_strOFFID & "'")
                    v_strSQL = Replace(v_strSQL, "?CFRID", "'" & v_strCFRID & "'")
                    v_strSQL = Replace(v_strSQL, "?CHKID", "'" & v_strCHKID & "'")
                    v_strSQL = Replace(v_strSQL, "?TLNAME", "'" & v_strTLNAME & "'")
                    v_strSQL = Replace(v_strSQL, "?OFFNAME", "'" & v_strOFFNAME & "'")
                    v_strSQL = Replace(v_strSQL, "?CHKNAME", "'" & v_strCHKNAME & "'")
                    v_strSQL = Replace(v_strSQL, "?CFRNAME", "'" & v_strCFRNAME & "'")
                    ' nhom dia chi tao giao dich
                    v_strSQL = Replace(v_strSQL, "?IPADDRESS", "'" & v_strIPADDRESS & "'")
                    v_strSQL = Replace(v_strSQL, "?WSNAME", "'" & v_strWSNAME & "'")
                    ' nhom quyen
                    v_strSQL = Replace(v_strSQL, "?SICODE", "'" & IIf(v_strSICODE = "", "000", v_strSICODE) & "'")
                    v_strSQL = Replace(v_strSQL, "?MICODE", "'" & IIf(v_strMICODE = "", "000", v_strMICODE) & "'")
                    v_strSQL = Replace(v_strSQL, "?COMICODE", "'" & IIf(v_strCOMICODE = "", "000", v_strCOMICODE) & "'")
                    v_strSQL = Replace(v_strSQL, "?MSGAMT", IIf(v_strMSGAMT <> "", "TO_NUMBER(a.COL_VALUE" & v_strMSGAMT & ")", "0"))
                    ' nhom giao dich cha-con
                    v_strSQL = Replace(v_strSQL, "?ISPARENT", v_strISPARENT)
                    v_strSQL = Replace(v_strSQL, "?PARENTID", v_strPARENTID)
                    v_strSQL = Replace(v_strSQL, "?PARENT_TEXT", "'" & v_strPARENT_TEXT & "'")
                    v_strSQL = Replace(v_strSQL, "?CHILDTLTXCD", "'" & v_strCHILDTLTXCD & "'")
                    v_strSQL = Replace(v_strSQL, "?PARENT_TLTXCD", "'" & v_strCHILDTLTXCD & "'")
                    v_strSQL = Replace(v_strSQL, "?ISBRID", v_strISBRID)
                    v_strSQL = Replace(v_strSQL, "?REASON", v_strREASON)
                    v_strSQL = Replace(v_strSQL, "?TXNOTE", "'" & v_strTXNOTE & "'")
                    v_strSQL = Replace(v_strSQL, "?VSD_BRID", "'" & v_strVsdBrid & "'")
                    v_strSQL = Replace(v_strSQL, "?VSD_2_BRID", "'" & v_strVsdBrid2 & "'")
                    v_strSQL = Replace(v_strSQL, "?T_T_3", v_strT_T_3)
                    'Add by thonm
                    'Thay the cong thuc tinh toan quyen
                    v_strSQL = Replace(v_strSQL, "?CA", v_strCA)
                    v_strSQL = Replace(v_strSQL, "?TN", v_strTN)
                    v_strSQL = Replace(v_strSQL, "?PL", v_strPL)
                    v_strSQL = Replace(v_strSQL, "?LCP", v_strLCP)
                    'End thonm

                    If v_trace_status = "1" And v_ds.Tables(0).Rows(i)("TRACE") = 1 Then
                        Trace.WriteLine("[APPDML - " & v_strTLTXCD & "] " & DateTime.Now & " :" & vbCrLf & " -o- Câu lệnh thứ #" & v_ds.Tables(0).Rows(i)("ORDNUM") & "-o-" & vbCrLf & v_strSQL & vbCrLf)
                        If InStr(v_strSQL.ToUpper, "SELECT") > 0 Then
                            Trace.WriteLine("-o- Dữ liệu câu lệnh thứ #" & v_ds.Tables(0).Rows(i)("ORDNUM") & "-o-")
                            v_strSQLTmp = v_strSQL.Substring(InStr(v_strSQL.ToUpper, "SELECT") - 1)
                            v_trace = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                            Trace.WriteLine(v_trace.GetXml & vbCrLf)
                        End If
                    End If

                    v_strSQLTmp = ""
                    If InStr(v_strSQL, "?TMP_TXFIELDS") > 0 Then
                        Dim v_nodelist1 As Xml.XmlNodeList
                        v_nodelist1 = v_node.SelectNodes("/TransactMessage/fields")
                        If v_nodelist1.Count > 0 Then
                            For ii As Integer = 0 To v_nodelist1.Count - 1
                                For j As Integer = 0 To v_nodelist1.Item(ii).ChildNodes.Count - 1
                                    With v_nodelist1.Item(ii).ChildNodes(j)
                                        v_strSQLTmp = .InnerText
                                        v_obj.ExecuteNonQuery(CommandType.Text, v_strSQLTmp)
                                        'Wtite Log
                                        'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "ExcuteTran", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Excute SQL command: " & v_strSQLTmp)
                                    End With
                                Next
                            Next
                        Else
                            v_strFieldsSQL = ""
                            For j As Integer = 1 To intCountNo
                                v_strFieldsSQL = v_strFieldsSQL & ", COL_VALUE" & String.Format("{0:00}", j) _
                                & ", COL_TYPE" & String.Format("{0:00}", j) _
                                & ", COL_DESC" & String.Format("{0:00}", j)
                            Next
                            v_strSQLTmp = "INSERT INTO TMP_TXFIELDS(AUTOID, TLTXCD " & v_strFieldsSQL & ", real_row) " _
                            & " SELECT SEQ_TMP_TXFIELDS.NEXTVAL, TLTXCD " & v_strFieldsSQL & ", rownum FROM " _
                            & " ( SELECT " _
                            & "'" & v_strTLTXCD & "' TLTXCD" & v_strFieldsSQL _
                            & " FROM  TLLOG " _
                            & " WHERE deleted =0 and " & IIf(v_strISPARENT = "2", "PARENTID = ", "AUTOID = ") & v_strAUTOID _
                            & " )"
                            v_strSQL = v_strSQLTmp
                            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                            'Wtite Log
                            'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "ExcuteTran", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Excute SQL command: " & v_strSQL)
                        End If
                    ElseIf InStr(v_strSQL, "?TLLOG") > 0 Then
                        v_strFieldsSQL = ""
                        For j As Integer = 1 To intCountNo
                            v_strFieldsSQL = v_strFieldsSQL & ", COL_VALUE" & String.Format("{0:00}", j) _
                            & ", COL_TYPE" & String.Format("{0:00}", j) _
                            & ", COL_DESC" & String.Format("{0:00}", j)
                        Next
                        v_strSQL = v_strSQL.Replace("?TLLOG", v_strFieldsSQL)
                        v_strFieldsSQL = ""
                        For j As Integer = 1 To intCountNo
                            v_strFieldsSQL = v_strFieldsSQL & ", a.COL_VALUE" & String.Format("{0:00}", j) _
                            & ", a.COL_TYPE" & String.Format("{0:00}", j) _
                            & ", a.COL_DESC" & String.Format("{0:00}", j)
                        Next
                        v_strSQL = v_strSQL.Replace("?VALUES", v_strFieldsSQL)
                        v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                        'Wtite Log
                        'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "ExcuteTran", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Excute SQL command: " & v_strSQL)

                    Else
                        ' end thonm
                        ' thuc hien cau lenh
                        v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                        'Wtite Log
                        'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "ExcuteTran", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Excute SQL command: " & v_strSQL)

                    End If
                    '' end tuanta 09/12/2008
                Next
                'End If
                ' attache txnum
                'lay ra cau sql lay du lieu tu tllog ra o day --> sinh vao dataset 

                If v_strISPARENT = 2 Then
                    v_strSQLTmp = "select autoid,col_value01,...  from tmp_tllog1"
                    v_trace = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                Else
                    v_strSQLTmp = "select parentid,col_value01,...  from tmp_tllog1"
                    v_trace = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                End If
                'bằngpv: Xử lý với trường hợp có ký CA 



                'Hanm5 them cau check cac bang so du co bi am khong?

                blnTran = False
                ''Wtite Log
                'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "ExcuteTran", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Excute SQL command: Commit")
            End With

            Return v_lngErr
        Catch ex As Exception
            'Wtite Log
            'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "ExcuteTran", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Error: " & ex.ToString)
            If blnTran Then
                v_obj.Rollback()
            End If
            If v_trace_status = "1" Then
                'Trace.WriteLine("[Kết thúc: APPDML - " & v_strTLTXCD & "] " & DateTime.Now & vbCrLf)
                tr2.Close()
                tr2.Dispose()
            End If
            ex.Source = "Host.txRouter.Transact"
            LogError.Write("EXECUTE_TRAN - Error source: " & ex.Source & vbNewLine _
                         & "Error code: System error!" & vbNewLine _
                         & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_HOST)
            Return ERR_SYSTEM_START
        Finally
            'WriteLog
            'mv_lwLogWriter.StopWriteLog(gc_MODULE_HOST, "Host", "txRouter", "ExcuteTran", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "")

            If InStr("4001|4081|4082|4083|4003|4004|4005|4006|4007|4020", v_strPARENT_TLTXCD) > 0 And v_strPARENT_TLTXCD <> "" And v_strOLDSTATUS = "0" Then
                v_nodeList = v_node.SelectNodes("/TransactMessage/fields")
                If v_nodeList.Count > 0 Then
                    For i = 0 To v_nodeList.Count - 1
                        v_node.RemoveChild(v_nodeList.Item(i))
                    Next
                End If
            End If
            If Not v_ds Is Nothing Then
                v_ds.Dispose()
            End If
            If Not v_trace Is Nothing Then
                v_trace.Dispose()
            End If
            If Not v_replace Is Nothing Then
                v_replace.Dispose()
            End If
            If Not v_obj Is Nothing Then
                v_obj.Dispose()
            End If
        End Try
    End Function

    Private Sub GetTranErr(ByVal v_node As Xml.XmlNode, ByVal v_lngErr As Long)
        Try
            With v_node
                Dim v_strBRID As String = .Attributes(gc_AtributeBRID).Value.ToString
                Dim v_strSTATUS As String = .Attributes(gc_AtributeSTATUS).Value.ToString
                Dim v_strSTATUS_TEXT As String = .Attributes(gc_AtributeSTATUSTEXT).Value.ToString
                Dim v_strTXNUM As String = .Attributes(gc_AtributeTXNUM).Value.ToString
                Dim v_strTXDATE As String = .Attributes(gc_AtributeTXDATE).Value.ToString
                Dim v_strTLTXCD As String = .Attributes(gc_AtributeTLTXCD).Value.ToString
                Dim v_strTXDESC As String = .Attributes(gc_AtributeTXDESC).Value.ToString

                If v_lngErr = ERR_SYSTEM_OK Then
                    mv_strTLTXSucc &= v_strSTATUS_TEXT & "|" & v_strTLTXCD & "|" & v_strTXNUM & "|" & v_strTXDATE & "|" & v_strTXDESC & "#"
                Else
                    mv_strTLTXErr &= v_strSTATUS_TEXT & "|" & v_strTLTXCD & "|" & v_strTXNUM & "|" & v_strTXDATE & "|" & v_strTXDESC & "#"
                End If

            End With

        Catch ex As Exception
            'ex.Source = "Host.txRouter.GetTranErr"
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            '             & "Error code: System error!" & vbNewLine _
            '             & "Error message: " & ex.Message, EventLogEntryType.Error)
            Throw ex
        End Try
    End Sub

    Private Sub WriteTranErr(ByRef pv_xmlDocument As Xml.XmlDocument)
        Dim dataElement As Xml.XmlElement
        Dim entryNode As Xml.XmlNode
        Dim v_attrFLDNAME, v_attrOLDVAL As Xml.XmlAttribute

        dataElement = pv_xmlDocument.CreateElement(Xml.XmlNodeType.Element, "TRAN_MESSAGE", "")

        entryNode = pv_xmlDocument.CreateNode(Xml.XmlNodeType.Element, "Entry", "")
        'Add field name
        v_attrFLDNAME = pv_xmlDocument.CreateAttribute("fldname")
        v_attrFLDNAME.Value = "SUCC_MSG"
        entryNode.Attributes.Append(v_attrFLDNAME)
        'Add current value
        v_attrOLDVAL = pv_xmlDocument.CreateAttribute("msg")
        If mv_strTLTXSucc.Length > 0 Then
            v_attrOLDVAL.Value = mv_strTLTXSucc
        End If
        entryNode.Attributes.Append(v_attrOLDVAL)
        'Set value
        If mv_strTLTXSucc.Length > 0 Then
            entryNode.InnerText = mv_strTLTXSucc
        End If

        dataElement.AppendChild(entryNode)

        entryNode = pv_xmlDocument.CreateNode(Xml.XmlNodeType.Element, "Entry", "")
        'Add field name
        v_attrFLDNAME = pv_xmlDocument.CreateAttribute("fldname")
        v_attrFLDNAME.Value = "ERR_MSG"
        entryNode.Attributes.Append(v_attrFLDNAME)
        'Add current value
        v_attrOLDVAL = pv_xmlDocument.CreateAttribute("msg")
        If mv_strTLTXErr.Length > 0 Then
            v_attrOLDVAL.Value = mv_strTLTXErr
        End If
        entryNode.Attributes.Append(v_attrOLDVAL)
        'Set value
        If mv_strTLTXErr.Length > 0 Then
            entryNode.InnerText = mv_strTLTXErr
        End If

        dataElement.AppendChild(entryNode)
        pv_xmlDocument.DocumentElement.AppendChild(dataElement)
    End Sub
    'start Myvq
    Private Sub WriteEncryptedFile(ByRef pv_xmlDocument As Xml.XmlDocument)
        If (mv_strFullFileName Is Nothing) Or (mv_strFullFileName = "") Then
            Exit Sub
        End If
        Try
            Dim dataElement As Xml.XmlElement
            Dim entryNode As Xml.XmlNode
            Dim v_attrFLDNAME, v_attrOLDVAL As Xml.XmlAttribute

            dataElement = pv_xmlDocument.CreateElement(Xml.XmlNodeType.Element, "ENCRYPTEDFILE", "")

            entryNode = pv_xmlDocument.CreateNode(Xml.XmlNodeType.Element, "Entry", "")
            'Add field name
            v_attrFLDNAME = pv_xmlDocument.CreateAttribute("ENCRYPTED_FILE")
            v_attrFLDNAME.Value = ServerBussinessCA.EncryptFile(mv_strTellerName, mv_strFullFileName)
            entryNode.Attributes.Append(v_attrFLDNAME)
            'Add current value
            v_attrOLDVAL = pv_xmlDocument.CreateAttribute("FULL_FILE_NAME")
            v_attrOLDVAL.Value = mv_strFullFileName
            entryNode.Attributes.Append(v_attrOLDVAL)

            dataElement.AppendChild(entryNode)

            pv_xmlDocument.DocumentElement.AppendChild(dataElement)

            mv_strFullFileName = ""
        Catch ex As Exception
            mv_strFullFileName = ""
        End Try
    End Sub
    'end Myvq

    Private Sub WriteTranErrDayInvalid(ByRef pv_xmlDocument As Xml.XmlDocument, ByVal pv_intErrType As Integer)
        Dim dataElement As Xml.XmlElement
        Dim entryNode As Xml.XmlNode
        Dim v_attrFLDNAME, v_attrOLDVAL As Xml.XmlAttribute
        Try
            Select Case pv_intErrType
                Case 0
                    dataElement = pv_xmlDocument.CreateElement(Xml.XmlNodeType.Element, "FAILED_MESSAGE", "")

                    entryNode = pv_xmlDocument.CreateNode(Xml.XmlNodeType.Element, "Entry", "")
                    'Add field name
                    v_attrFLDNAME = pv_xmlDocument.CreateAttribute("fldname")
                    v_attrFLDNAME.Value = "DAY_NOT_START"
                    entryNode.Attributes.Append(v_attrFLDNAME)
                    'Add current value
                    v_attrOLDVAL = pv_xmlDocument.CreateAttribute("msg")

                    v_attrOLDVAL.Value = "DAY_NOT_START"
                    entryNode.Attributes.Append(v_attrOLDVAL)
                    'Set value
                    entryNode.InnerText = "Day is not started"

                    dataElement.AppendChild(entryNode)

                    pv_xmlDocument.DocumentElement.AppendChild(dataElement)
                Case 1
                    dataElement = pv_xmlDocument.CreateElement(Xml.XmlNodeType.Element, "FAILED_MESSAGE", "")

                    entryNode = pv_xmlDocument.CreateNode(Xml.XmlNodeType.Element, "Entry", "")
                    'Add field name
                    v_attrFLDNAME = pv_xmlDocument.CreateAttribute("fldname")
                    v_attrFLDNAME.Value = "DAY_INVALID"
                    entryNode.Attributes.Append(v_attrFLDNAME)
                    'Add current value
                    v_attrOLDVAL = pv_xmlDocument.CreateAttribute("msg")

                    v_attrOLDVAL.Value = "DAY_INVALID"
                    entryNode.Attributes.Append(v_attrOLDVAL)
                    'Set value
                    entryNode.InnerText = "Day is invalid"

                    dataElement.AppendChild(entryNode)

                    pv_xmlDocument.DocumentElement.AppendChild(dataElement)
            End Select
        Catch ex As Exception
            Throw ex
        End Try
    End Sub


    Public Function Batch(ByVal v_strAPPTYPE As String, ByVal v_strBCHMDL As String, Optional ByVal v_strBCHFillter As String = "", Optional ByRef v_intMaxRow As Integer = 0) As Long
        Dim v_lngErrCode As Long = ERR_SYSTEM_OK
        Dim v_strErrorSource As String = "Host.txRouter.Batch"
        Dim v_objBatch As CoreBusiness.Batch = Nothing
        Dim v_objTxRouter As New Host.txRouter
        Try
            'Chỉ cho phép chạy Batch nếu HOSTATUS là INACTIVE
            Dim v_strSYSVAR As String = "", v_DataAccess As New DataAccess
            v_DataAccess.NewDBInstance(gc_MODULE_HOST)
            v_lngErrCode = v_DataAccess.GetSysVar("SYSTEM", "HOSTATUS", v_strSYSVAR)
            If v_lngErrCode <> ERR_SYSTEM_OK Then
                'ContextUtil.SetAbort()
                Return v_lngErrCode
            End If
            If v_strSYSVAR <> OPERATION_INACTIVE Then
                'ContextUtil.SetAbort()
                Return ERR_SA_HOST_OPERATION_STILLACTIVE
            End If

            'Xử lý chung cho các Object kế thừa từ objMaster
            Select Case v_strAPPTYPE
                Case SUB_SYSTEM_DE
                    v_objBatch = New Sats.DE.Batch
                Case SUB_SYSTEM_CA
                    'v_objBatch = New Sats.CA.Batch
                Case SUB_SYSTEM_MF
                    v_objBatch = New Sats.MF.Batch
                Case SUB_SYSTEM_CS
                    v_objBatch = New Sats.CS.Batch
                Case SUB_SYSTEM_SF
                    v_objBatch = New Sats.CS.Batch
            End Select

            If Not v_objBatch Is Nothing Then
                'Tạo giao dịch xử lý trong Batch
                v_lngErrCode = v_objBatch.ExecuteRouter(v_strBCHMDL, v_strBCHFillter, v_intMaxRow)
                If v_lngErrCode <> ERR_SYSTEM_OK Then
                    'ContextUtil.SetAbort()
                    Return v_lngErrCode
                End If
                'Thực hiện giao dịch Batch
                v_lngErrCode = v_objTxRouter.ExecuteBatchName(v_strBCHMDL)
                If v_lngErrCode <> ERR_SYSTEM_OK Then
                    'ContextUtil.SetAbort()
                    Return v_lngErrCode
                End If
            End If
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
    'Start Myvq
    Public Function SaveFile(ByVal pv_strBrId As String, ByVal pv_strCurrDate As String, _
                             ByVal pv_strTltxcd As String) As Long
        'Check file
        Dim v_strCurrDate As String = pv_strCurrDate.Replace("/", "")
        Dim v_strFilePath As String
        'Add Thanglv 11/09/2013
        If pv_strTltxcd = "1131" Then
            v_strFilePath = ServerBussinessCA.GetFileCAPathSBV(pv_strBrId)
        ElseIf pv_strTltxcd = "1150" Then
            v_strFilePath = ServerBussinessCA.GetFileCAPath(pv_strBrId.Split("|")(0))
        Else
            v_strFilePath = ServerBussinessCA.GetFileCAPath(pv_strBrId)
        End If
        'Dim v_strFilePath = ServerBussinessCA.GetFileCAPath(pv_strBrId)
        'End Thanglv
        v_strFilePath = v_strFilePath & "\"
        If Not (Directory.Exists(v_strFilePath)) Then
            Return 0
            Exit Function
        End If

        'Get all files
        Try
            If pv_strTltxcd = "1123" Or pv_strTltxcd = "1124" Or pv_strTltxcd = "1125" Or pv_strTltxcd = "1131" Then
                'get file
                Dim v_DataAccess As New DataAccess
                Dim v_strRemotePath, v_strRootPath As String
                Dim v_strServerAddress, v_strServerAddress1, v_strServerPort, v_strUsername, v_strPassword As String
                If pv_strTltxcd <> "1131" Then
                    v_DataAccess.GetSysVar("VSDFTPSVR", "ServerAddress", pv_strBrId, v_strServerAddress)
                    v_DataAccess.GetSysVar("VSDFTPSVR", "ServerAddress1", pv_strBrId, v_strServerAddress1)
                    v_DataAccess.GetSysVar("VSDFTPSVR", "ServerPort", pv_strBrId, v_strServerPort)
                    v_DataAccess.GetSysVar("VSDFTPSVR", "Username", pv_strBrId, v_strUsername)
                    v_DataAccess.GetSysVar("VSDFTPSVR", "Password", pv_strBrId, v_strPassword)
                    v_DataAccess.GetSysVar("VSDFTPSVR", "RemotePath", pv_strBrId, v_strRemotePath)
                    v_DataAccess.GetSysVar("VSDFTPSVR", "RootPath", pv_strBrId, v_strRootPath)
                Else
                    v_DataAccess.GetSysVar("VSDFTPSBV", "ServerAddress", pv_strBrId, v_strServerAddress)
                    v_DataAccess.GetSysVar("VSDFTPSBV", "ServerAddress1", pv_strBrId, v_strServerAddress1)
                    v_DataAccess.GetSysVar("VSDFTPSBV", "ServerPort", pv_strBrId, v_strServerPort)
                    v_DataAccess.GetSysVar("VSDFTPSBV", "Username", pv_strBrId, v_strUsername)
                    v_DataAccess.GetSysVar("VSDFTPSBV", "Password", pv_strBrId, v_strPassword)
                    v_DataAccess.GetSysVar("VSDFTPSBV", "RemotePath", pv_strBrId, v_strRemotePath)
                    v_DataAccess.GetSysVar("VSDFTPSBV", "RootPath", pv_strBrId, v_strRootPath)
                End If

                Dim v_oWriter As System.IO.StreamWriter
                Dim v_strAppPath As String = System.AppDomain.CurrentDomain.BaseDirectory
                If File.Exists(v_strAppPath & "data\" & pv_strTltxcd & pv_strBrId & "_" & v_strCurrDate & ".bat") Then
                    File.Delete(v_strAppPath & "data\" & pv_strTltxcd & pv_strBrId & "_" & v_strCurrDate & ".bat")
                End If
                Dim v_Prefix As String
                Dim v_strTxdate1 As String = Replace(v_strCurrDate, "/", "")
                Select Case pv_strBrId
                    Case "0001"
                        v_Prefix = "LISTED"
                    Case "0003"
                        v_Prefix = "UPCOM"
                    Case "0004"
                        v_Prefix = "BOND"
                    Case "0005"
                        v_Prefix = "USDBOND"
                    Case "0006"
                        v_Prefix = "BONDTP"
                End Select

                v_oWriter = New StreamWriter(v_strAppPath & "data\" & pv_strBrId & "_" & v_strTxdate1 & ".bat")
                v_oWriter.WriteLine("@ftp -i -s:""%~f0""&GOTO:EOF")
                v_oWriter.WriteLine("open " & v_strServerAddress)
                v_oWriter.WriteLine(v_strUsername)
                v_oWriter.WriteLine(v_strPassword)
                v_oWriter.WriteLine("lcd " & """" & v_strAppPath & "data" & """")
                v_oWriter.WriteLine("cd " & v_strRemotePath)
                v_oWriter.WriteLine("binary")


                v_oWriter.WriteLine("get " & pv_strTltxcd & pv_strBrId & v_strTxdate1 & "_enc.zip " & pv_strTltxcd & pv_strBrId & v_strTxdate1 & "_enc.zip")

                v_oWriter.WriteLine("bye" & vbCrLf)
                v_oWriter.Close()
                Dim v_oProcess As Process
                v_oProcess = New Process

                v_oProcess.StartInfo.FileName = v_strAppPath & "data\" & pv_strBrId & "_" & v_strTxdate1 & ".bat"
                v_oProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
                v_oProcess.StartInfo.CreateNoWindow = True
                v_oProcess.Start()
                'v_oProcess.WaitForExit()
                v_oProcess.Close()
                ' Lấy từ đầu 2 
                v_oWriter = New StreamWriter(v_strAppPath & "data\" & pv_strBrId & "_" & v_strTxdate1 & "1.bat")
                v_oWriter.WriteLine("@ftp -i -s:""%~f0""&GOTO:EOF")
                v_oWriter.WriteLine("open " & v_strServerAddress1)
                v_oWriter.WriteLine(v_strUsername)
                v_oWriter.WriteLine(v_strPassword)
                v_oWriter.WriteLine("lcd " & """" & v_strAppPath & "data" & """")
                v_oWriter.WriteLine("cd " & v_strRemotePath)
                v_oWriter.WriteLine("binary")

                v_oWriter.WriteLine("get " & pv_strTltxcd & pv_strBrId & v_strTxdate1 & "_enc.zip " & pv_strTltxcd & pv_strBrId & v_strTxdate1 & "_enc.zip")

                v_oWriter.WriteLine("bye" & vbCrLf)
                v_oWriter.Close()
                v_oProcess = New Process

                v_oProcess.StartInfo.FileName = v_strAppPath & "data\" & pv_strBrId & "_" & v_strTxdate1 & "1.bat" 'v_strAppPath & "data\" & v_strBrid & "_" & v_strTxdate & ".bat"
                v_oProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
                v_oProcess.StartInfo.CreateNoWindow = True
                v_oProcess.Start()
                'v_oProcess.WaitForExit()
                v_oProcess.Close()
                System.Threading.Thread.Sleep(30 * 1000)

                ServerBussinessCA.ExtractFile1123(v_strFilePath, v_strCurrDate, pv_strBrId, pv_strTltxcd, mv_strTellerName)
                'send lai ftp server
            ElseIf pv_strTltxcd = "1150" Then
                Dim v_oDi As New IO.DirectoryInfo(v_strFilePath)
                Dim v_strEndFile As String
                Dim pv_strCsType As String = pv_strBrId.Split("|")(1)
                pv_strBrId = pv_strBrId.Split("|")(0)
                Select Case pv_strCsType
                    Case "1"
                        v_strEndFile = "_PT"
                    Case "2"
                        v_strEndFile = "_TT"
                    Case "3"
                        v_strEndFile = "_GT"
                End Select
                Dim v_aryFi As IO.FileInfo() = v_oDi.GetFiles(pv_strTltxcd & pv_strBrId & v_strCurrDate & v_strEndFile & ".xml")
                Dim v_oFi As IO.FileInfo
                Dim v_strNewFilePath As String = v_strFilePath & pv_strBrId & "\"
                For Each v_oFi In v_aryFi
                    ServerBussinessCA.ExtractAndSaveFile(v_strFilePath, v_oFi.FullName, _
                                                      mv_strTellerName, pv_strBrId, _
                                                      v_strCurrDate, pv_strTltxcd)
                Next

                mv_strFullFileName = v_strNewFilePath & v_strCurrDate & ".zip"
            Else

                Dim v_oDi As New IO.DirectoryInfo(v_strFilePath)
                Dim v_aryFi As IO.FileInfo() = v_oDi.GetFiles(pv_strTltxcd & pv_strBrId & v_strCurrDate & ".xml")
                Dim v_oFi As IO.FileInfo
                Dim v_strNewFilePath As String = v_strFilePath & pv_strBrId & "\"
                For Each v_oFi In v_aryFi
                    ServerBussinessCA.ExtractAndSaveFile(v_strFilePath, v_oFi.FullName, _
                                                      mv_strTellerName, pv_strBrId, _
                                                      v_strCurrDate, pv_strTltxcd)
                Next

                mv_strFullFileName = v_strNewFilePath & v_strCurrDate & ".zip"
            End If
            'End If
        Catch Ex As Exception
            Return -1
            LogError.Write("SaveFile - Error source: " & Ex.Source & vbNewLine _
                     & "Error code: System error!" & vbNewLine _
                     & "Error message: " & Ex.Message, EventLogEntryType.Error, gc_MODULE_HOST)
        End Try
    End Function

    'Hanm5: Phuc vu trao doi file giua SBL va NHTT
    Public Function SaveFileSblFtpBnk(ByVal pv_strBrId As String, ByVal pv_strCurrDate As String, ByVal pv_strFileName As String, ByVal pv_strTltxcd As String) As Long
        Dim v_strCurrDate As String = pv_strCurrDate.Replace("/", "")
        Dim v_strFilePath As String = ""
        Dim v_strFileName As String = ""
        v_strFilePath = "\" & pv_strTltxcd & "\" & pv_strBrId & "\" & v_strCurrDate & "\"
        If Not Directory.Exists(v_strFilePath) Then
            Directory.CreateDirectory(v_strFilePath)
        End If
        Try
            Dim v_obj As New DataAccess
            Dim v_strServerAddress, v_strServerAddress1, v_strServerPort, v_strUsername, v_strPassword, v_strRemotePath, v_strRootPath, v_strCAEnc As String
            v_obj.GetSysVar("SBLFTPBNK", "ServerAddress", pv_strBrId, v_strServerAddress)
            v_obj.GetSysVar("SBLFTPBNK", "ServerAddress1", pv_strBrId, v_strServerAddress1)
            v_obj.GetSysVar("SBLFTPBNK", "ServerPort", pv_strBrId, v_strServerPort)
            v_obj.GetSysVar("SBLFTPBNK", "Username", pv_strBrId, v_strUsername)
            v_obj.GetSysVar("SBLFTPBNK", "Password", pv_strBrId, v_strPassword)
            v_obj.GetSysVar("SBLFTPBNK", "RemotePath", pv_strBrId, v_strRemotePath)
            v_obj.GetSysVar("SBLFTPBNK", "RootPath", pv_strBrId, v_strRootPath)
            v_obj.GetSysVar("SBLFTPBNK", "CaEncrypt", pv_strBrId, v_strCAEnc)

            If pv_strTltxcd = "2150" Then
                v_strFileName = Mid(pv_strFileName, 1, pv_strFileName.Length - 3) & "xml"
            ElseIf pv_strTltxcd = "2151" Then
                Dim v_fiTmp As New FileInfo(Mid(pv_strFileName, 1, pv_strFileName.Length - 3) & "xml")
                v_strFileName = v_fiTmp.Name
            End If
            'Dim v_ftpClient As New Sats.Utils.FtpClient(v_strServerAddress, v_strUsername, v_strPassword)
            Dim v_ftpClient As New Sats.Utils.FtpClient
            Try
                v_ftpClient.Server = v_strServerAddress
                v_ftpClient.Username = v_strUsername
                v_ftpClient.Password = v_strPassword
                v_ftpClient.Login()
            Catch ex As Sats.Utils.FtpClient.FtpException
                v_ftpClient.Server = v_strServerAddress1
                v_ftpClient.Username = v_strUsername
                v_ftpClient.Password = v_strPassword
                v_ftpClient.Login()
            End Try

            v_ftpClient.ChangeDir(v_strRemotePath)
            v_ftpClient.Download(v_strFileName, v_strFilePath & v_strFileName, True)
            Dim v_di As New DirectoryInfo(v_strFilePath)
            Dim v_strFullFileName As String = v_di.FullName & "\" & v_strFileName
            ServerBussinessCA.ExtractAndSaveFile(v_strFilePath, v_strFullFileName, mv_strTellerName, pv_strBrId, v_strCurrDate, pv_strTltxcd)
            If v_strCAEnc = "N" Then
                v_ftpClient.Upload(Mid(v_strFullFileName, 1, v_strFullFileName.Length - 4) & ".txt")
            Else
                v_ftpClient.Upload(Mid(v_strFullFileName, 1, v_strFullFileName.Length - 4) & "ENCRYPTED.xml")
            End If
            'v_ftpClient.Upload(Mid(v_strFullFileName, 1, v_strFullFileName.Length - 4) & "ENCRYPTED.xml")
            If pv_strTltxcd = "2151" Then
                v_ftpClient.Upload(Mid(v_strFullFileName, 1, v_strFullFileName.Length - 4) & ".txt")
            End If
            v_ftpClient.Close()
            v_ftpClient.Dispose()
        Catch Ex As Exception
            LogError.Write("SaveFileSblFtpBnk - Error source: " & Ex.Source & vbNewLine _
                     & "Error code: System error!" & vbNewLine _
                     & "Error message: " & Ex.Message, EventLogEntryType.Error, gc_MODULE_HOST)
            Return -1
        End Try
    End Function

    Public Function SendFTPtoHNX(ByVal pv_strFilePath As String, ByVal pv_strFileName As String, ByVal pv_strBrid As String) As Long
        Dim v_lngErrCode As Long = ERR_SYSTEM_OK
        Dim v_strServerAddress As String
        Dim v_strServerPort As String
        Dim v_strUsername As String
        Dim v_strPassword As String
        Dim v_strRemotePath As String
        Dim v_xmlDocument As New XmlDocumentEx
        Dim v_strSQL As String, v_DataAccess As New DataAccess

        Try
            v_DataAccess.GetSysVar("EXPORT", "ServerAddress", pv_strBrid, v_strServerAddress)
            v_DataAccess.GetSysVar("EXPORT", "ServerPort", pv_strBrid, v_strServerPort)
            v_DataAccess.GetSysVar("EXPORT", "Username", pv_strBrid, v_strUsername)
            v_DataAccess.GetSysVar("EXPORT", "Password", pv_strBrid, v_strPassword)
            v_DataAccess.GetSysVar("EXPORT", "RemotePath", pv_strBrid, v_strRemotePath)

            Dim v_oWriter As System.IO.StreamWriter

            If File.Exists(pv_strFilePath & "\" & Mid(pv_strFileName, 1, pv_strFileName.Length - 4) & ".bat") Then
                File.Delete(pv_strFilePath & "\" & Mid(pv_strFileName, 1, pv_strFileName.Length - 4) & ".bat")
            End If

            v_oWriter = New StreamWriter(pv_strFilePath & Mid(pv_strFileName, 1, pv_strFileName.Length - 4) & ".bat")
            v_oWriter.WriteLine("@ftp -i -s:""%~f0""&GOTO:EOF")
            v_oWriter.WriteLine("open " & v_strServerAddress)
            v_oWriter.WriteLine(v_strUsername)
            v_oWriter.WriteLine(v_strPassword)
            v_oWriter.WriteLine("lcd " & pv_strFilePath.Substring(0, pv_strFilePath.Length - 1))
            v_oWriter.WriteLine("cd " & v_strRemotePath)
            v_oWriter.WriteLine("binary")
            v_oWriter.WriteLine("put " & pv_strFileName & " " & pv_strFileName)
            v_oWriter.WriteLine("bye" & vbCrLf)

            v_oWriter.Close()

            Dim v_oProcess As Process
            v_oProcess = New Process

            v_oProcess.StartInfo.FileName = pv_strFilePath & Mid(pv_strFileName, 1, pv_strFileName.Length - 4) & ".bat"
            v_oProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
            v_oProcess.StartInfo.CreateNoWindow = True
            v_oProcess.Start()
            Threading.Thread.Sleep(3000)
            v_oProcess.Close()

            'If File.Exists(v_strLocalDir & "\" & Mid(v_strLocalFileName, 1, v_strLocalFileName.Length - 4) & ".bat") Then
            '    File.Delete(v_strLocalDir & "\" & Mid(v_strLocalFileName, 1, v_strLocalFileName.Length - 4) & ".bat")
            'End If
            Return 0
        Catch ex As Exception
            LogError.Write(ex.Message, EventLogEntryType.Error, gc_MODULE_HOST)
            Return -1
        Finally
            v_DataAccess.Dispose()
        End Try
    End Function

    Public Function SaveFile(ByVal pv_strBranchId As String, _
                                ByVal pv_strBridCurrdate As String, _
                                ByVal pv_strTellerName As String, _
                                ByVal pv_strEncryptedFile As String) As Long
        Try
            'get Path
            Dim v_strBridCurrDate As String = pv_strBridCurrdate.Replace("/", "")
            Dim v_strPath = "C:\Import\" & pv_strBranchId & "\" & v_strBridCurrDate
            Dim v_strOrigFile As String = ServerBussinessCA.DecryptXML(pv_strEncryptedFile, pv_strTellerName)
            If (v_strOrigFile Is Nothing) Or (v_strOrigFile = "") Then
                Return -1
                Exit Function
            End If

            Dim v_oDocument As New Xml.XmlDocument
            v_oDocument.LoadXml(v_strOrigFile)
            Dim v_attrRColl As Xml.XmlAttributeCollection = v_oDocument.DocumentElement.Attributes
            Dim v_attr As Xml.XmlAttribute
            For Each v_attr In v_attrRColl
                ServerBussinessCA.SaveFile(v_strPath & "\" & v_attr.Name, v_attr.Value)
            Next
            'Return ServerBussinessCA.SaveFile(v_strPath & "\" & pv_strBranchId & _
            'v_strBridCurrDate & ".zip", v_strOrigFile)

        Catch ex As Exception
            Return -1
        End Try
    End Function

    Public Function ExtractFile(ByVal pv_strBranchId As String, _
                                ByVal pv_strBridCurrdate As String) As Long
        Try
            Dim v_strAppPath As String
            v_strAppPath = System.AppDomain.CurrentDomain.BaseDirectory

            Dim v_strBridCurrDate = pv_strBridCurrdate.Replace("/", "")
            Dim v_strOrigPath = "C:\Import\" & pv_strBranchId & "\" & v_strBridCurrDate
            'Dim v_strFullName = v_strOrigPath & "\" & pv_strBranchId & v_strBridCurrDate & ".zip"

            'If Not (File.Exists(v_strFullName)) Then
            'Return -1
            'Exit Function
            'End If

            'unzip file
            'Dim mv_xzipEngine As New ZipEngine
            'mv_xzipEngine.UnzipFile(v_strOrigPath & "\", pv_strBranchId & v_strBridCurrDate & ".zip", "*")

            'rename
            Dim v_Prefix As String = ""
            Select Case pv_strBranchId
                Case "0001"
                    v_Prefix = "LISTED"
                Case "0003"
                    v_Prefix = "UPCOM"
                Case "0004"
                    v_Prefix = "BOND"
                Case "0005"
                    v_Prefix = "USDBOND"
            End Select
            If (pv_strBranchId = "0002") Then
                Rename(v_strOrigPath & "\" & "astdl" & pv_strBridCurrdate & ".txt", _
                       v_strOrigPath & "\" & pv_strBranchId & "_ASTDL_" & pv_strBridCurrdate & ".txt")
                Rename(v_strOrigPath & "\" & "astpt" & pv_strBridCurrdate & ".txt", _
                       v_strOrigPath & "\" & pv_strBranchId & "_ASTPT_" & pv_strBridCurrdate & ".txt")
                File.Move(v_strOrigPath & "\" & pv_strBranchId & "_ASTDL_" & pv_strBridCurrdate & ".txt", _
                        v_strAppPath & "data\" & pv_strBranchId & "_ASTDL_" & pv_strBridCurrdate & ".txt")
                File.Move(v_strOrigPath & "\" & pv_strBranchId & "_ASTPT_" & pv_strBridCurrdate & ".txt", _
                v_strAppPath & "data\" & pv_strBranchId & "_ASTPT_" & pv_strBridCurrdate & ".txt")
                Return 0
            ElseIf File.Exists(v_strOrigPath & "\" & v_Prefix & "_TRADING_RESULT" & pv_strBridCurrdate & ".zip") Then
                'mv_xzipEngine.UnzipFile(v_strOrigPath & "\", v_Prefix & "_TRADING_RESULT" & pv_strBridCurrdate & ".zip", _
                '          v_Prefix & "_TRADING_RESULT" & pv_strBridCurrdate & ".xml")
                Dim f As New IO.FileInfo(v_strOrigPath & "\" & pv_strBranchId & "_" & pv_strBridCurrdate & ".xml")
                If f.Exists Then
                    f.Delete()
                End If
                Rename(v_strOrigPath & "\" & v_Prefix & "_TRADING_RESULT" & pv_strBridCurrdate & ".xml", _
                       v_strOrigPath & "\" & pv_strBranchId & "_" & pv_strBridCurrdate & ".xml")
                File.Move(v_strOrigPath & "\" & pv_strBranchId & "_" & pv_strBridCurrdate & ".xml", _
                       v_strAppPath & "data\" & pv_strBranchId & "_" & pv_strBridCurrdate & ".xml")
                Return 0
            End If
        Catch ex As Exception
            Return -1
            LogError.Write("ExtractFile - Error source: " & ex.Source & vbNewLine _
                     & "Error code: System error!" & vbNewLine _
                     & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_HOST)
        End Try
    End Function


    'End Myvq

#Region " Private methods "
    'Hàm này thực hiện một bước Batch
    'v_strBatchName là tên bước chạy Batch
    Private Function ExecuteBatchName(ByVal v_strBatchName As String) As Long
        Dim v_lngErrCode As Long = ERR_SYSTEM_OK
        Dim v_strErrorSource As String = "CoreBusiness.Batch.ExecuteBatchName." & v_strBatchName
        Dim v_strSQL As String, v_ds As DataSet, v_obj As New DataAccess
        Dim v_strTxNum As String = "", v_strTxDate As String = ""
        Try
            v_obj.NewDBInstance(gc_MODULE_HOST)
            'Lấy danh sách các giao dịch trong Batch theo BatchName có trạng thái là Logged để thực hiện
            v_strSQL = "SELECT TXDATE, TXNUM FROM TLLOG WHERE DELTD<>'Y' AND TXSTATUS='" & CStr(TransactStatus1.Logged) & "' AND BATCHNAME='" & v_strBatchName & "'"
            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            If v_ds.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To v_ds.Tables(0).Rows.Count - 1 Step 1
                    'Lấy TxDate và TxNum
                    v_strTxDate = Format(gf_CorrectDateField(v_ds.Tables(0).Rows(i)("TXDATE")), gc_FORMAT_DATE)
                    v_strTxNum = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(i)("TXNUM")))
                    'Thực hiện giao dịch
                    v_lngErrCode = ExecuteTxMessage(v_strTxDate, v_strTxNum)
                    If v_lngErrCode <> ERR_SYSTEM_OK Then
                        'ContextUtil.SetAbort()
                        Return v_lngErrCode
                    End If
                Next
            End If
            'ContextUtil.SetComplete()
            Return v_lngErrCode

        Catch ex As Exception
            'ContextUtil.SetAbort()
            'LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
            '             & "Error code: System error!" & vbNewLine _
            '             & "Error message: " & v_strTxDate & "." & v_strTxNum, EventLogEntryType.Error)
            Throw ex
        End Try
    End Function

    'Hàm này thực hiện chạy lại một giao dịch
    'v_strTxDate là ngày giao dịch, v_strTxNum là số giao dịch
    Private Function ExecuteTxMessage(ByVal v_strTxDate As String, ByVal v_strTxNum As String) As Long
        Dim v_lngErrCode As Long = ERR_SYSTEM_OK
        Dim v_strErrorSource As String = "CoreBusiness.Batch.ExecuteBatchName." & v_strTxDate & "." & v_strTxNum, v_strErrorMessage As String = ""
        Dim v_obj As New DataAccess
        Dim v_strTxMsg As String, v_xmlDocument As New Xml.XmlDocument, v_objMessageLog As New MessageLog
        v_objMessageLog.NewDBInstance(gc_MODULE_HOST)
        Try
            v_obj.NewDBInstance(gc_MODULE_HOST)
            'Lấy nội dung chi tiết của giao dịch
            v_strTxMsg = BuildXMLObjMsg(v_strTxDate, , , , gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SY_AUTHENTICATION, gc_ActionInquiry, , , "GetMessage", , v_strTxNum)
            v_xmlDocument.LoadXml(v_strTxMsg)
            v_lngErrCode = v_objMessageLog.TransDetail(v_xmlDocument)
            If v_lngErrCode <> ERR_SYSTEM_OK Then
                'ContextUtil.SetAbort()
                Return v_lngErrCode
            End If

            'Thực hiện giao dịch. ?ối với giao dịch Batch thì thực hiện luôn không cần quan tâm đến số lần Submit
            Dim v_strTXTYPE As String = Trim(v_xmlDocument.DocumentElement.Attributes(gc_AtributeTXTYPE).Value.ToString)
            Dim v_intSTATUS As Integer = CInt(v_xmlDocument.DocumentElement.Attributes(gc_AtributeSTATUS).Value.ToString)
            Dim v_strDELTD As String = Trim(v_xmlDocument.DocumentElement.Attributes(gc_AtributeDELTD).Value.ToString)
            Dim v_blnReversal As Boolean = IIf(v_strDELTD = "Y", True, False), v_blnApproval As Boolean = False

            If v_lngErrCode <> ERR_SYSTEM_OK Then
                'Lỗi trả v?
                v_strErrorMessage = v_strErrorSource & ".Step: HostAppCheck"
                LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
                             & "Error code: System error!" & vbNewLine _
                             & "Error message: " & v_strTxDate & "." & v_strTxNum, EventLogEntryType.Error, gc_MODULE_HOST)
                'ContextUtil.SetAbort()
                Return v_lngErrCode
            Else
                'Giao dịch trong Batch không quan tâm đến duyệt giao dịch
                v_xmlDocument.DocumentElement.Attributes(gc_AtributeSTATUS).InnerXml = TransactStatus1.Completed

                'Tạo bộ phép toán cập nhật phân hệ nghiệp vụ
                v_lngErrCode = GenAPPMAP(v_xmlDocument)
                If v_lngErrCode <> ERR_SYSTEM_OK Then
                    v_strErrorMessage = v_strErrorSource & ".Step: GenAPPMAP"
                    LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
                                 & "Error code: System error!" & vbNewLine _
                                 & "Error message: " & v_strTxDate & "." & v_strTxNum, EventLogEntryType.Error, gc_MODULE_HOST)
                    'ContextUtil.SetAbort()
                    Return v_lngErrCode
                End If
                'Cập nhật GL và TLLOG
                v_lngErrCode = HostTransUpdate(v_xmlDocument)
                If v_lngErrCode <> ERR_SYSTEM_OK Then
                    v_strErrorMessage = v_strErrorSource & ".Step: HostTransUpdate"
                    LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
                                 & "Error code: System error!" & vbNewLine _
                                 & "Error message: " & v_strTxDate & "." & v_strTxNum, EventLogEntryType.Error, gc_MODULE_HOST)
                    'ContextUtil.SetAbort()
                    Return v_lngErrCode
                End If
                'Cập nhật phân hệ nghiệp vụ
                v_lngErrCode = HostAppUpdate(v_xmlDocument)
                If v_lngErrCode <> ERR_SYSTEM_OK Then
                    v_strErrorMessage = v_strErrorSource & ".Step: HostAppUpdate"
                    LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
                                 & "Error code: System error!" & vbNewLine _
                                 & "Error message: " & v_strTxDate & "." & v_strTxNum, EventLogEntryType.Error, gc_MODULE_HOST)
                    'ContextUtil.SetAbort()
                    Return v_lngErrCode
                End If
            End If
            'ContextUtil.SetComplete()
            Return v_lngErrCode

        Catch ex As Exception
            'ContextUtil.SetAbort()
            'LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
            '             & "Error code: " & v_strErrorMessage & vbNewLine _
            '             & "Error message: " & ex.Message, EventLogEntryType.Error)
            Throw ex
        End Try
    End Function



    Private Function BuildAMTEXP(ByVal pv_xmlDocument As Xml.XmlDocument, ByVal strAMTEXP As String) As String
        Try
            Dim v_strEvaluator, v_strElemenent As String
            Dim v_lngIndex As Long
            Dim v_nodetxData As Xml.XmlNode
            Dim v_strFEEAMT As String = pv_xmlDocument.DocumentElement.Attributes(gc_AtributeFEEAMT).Value.ToString
            Dim v_strVATAMT As String = pv_xmlDocument.DocumentElement.Attributes(gc_AtributeVATAMT).Value.ToString

            v_strEvaluator = vbNullString
            v_lngIndex = 1

            While v_lngIndex < Len(strAMTEXP)
                'Get 02 charatacters in AMTEXP
                v_strElemenent = Mid$(strAMTEXP, v_lngIndex, 2)
                Select Case v_strElemenent
                    Case "++", "--", "**", "//", "((", "))"
                        'Operand
                        v_strEvaluator = v_strEvaluator & Left$(v_strElemenent, 1)
                    Case Else
                        'Operator
                        v_nodetxData = pv_xmlDocument.SelectSingleNode("TransactMessage/fields/entry[@fldname='" & v_strElemenent & "']")
                        v_strEvaluator = v_strEvaluator & v_nodetxData.InnerText
                End Select
                v_lngIndex = v_lngIndex + 2
            End While
            'ContextUtil.SetComplete()
            Return v_strEvaluator
        Catch ex As Exception
            'ContextUtil.SetAbort()
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            '             & "Error code: System error!" & vbNewLine _
            '             & "Error message: " & ex.Message, EventLogEntryType.Error)
            Throw ex
        End Try
    End Function

    Private Function GenAPPCHK(ByRef pv_xmlDocument As Xml.XmlDocument) As Long
        Dim v_lngErrCode As Long = ERR_SYSTEM_OK
        Dim v_strErrorSource As String = "Host.txRouter.GenAPPCHK"
        Dim v_strSQL As String, v_ds As DataSet, v_obj As New DataAccess

        Try
            v_obj.NewDBInstance(gc_MODULE_HOST)
            Dim v_objEval As New Evaluator, v_nodetxData As Xml.XmlNode
            Dim i As Integer, v_strACFLD As String, v_strAMTEXP As String, v_strVALUE As String = "", v_strACCTNO As String
            'Dim EntrySUBTXNO, EntryDORC, EntryACCTNO, EntryCCYCD As String
            'Dim EntryAMOUNT As Double

            Dim v_nodeGenChecking As Xml.XmlNode
            v_nodeGenChecking = pv_xmlDocument.SelectSingleNode("/TransactMessage/appchk")
            'Không tạo APPCHK nếu đã có
            If Not v_nodeGenChecking Is Nothing Then Return ERR_SYSTEM_OK

            'Create Appchk Node
            Dim v_appchkElement As Xml.XmlElement, v_entryNode As Xml.XmlNode
            v_appchkElement = pv_xmlDocument.CreateElement(Xml.XmlNodeType.Element, "appchk", "")


            Dim v_strTLTXCD As String = pv_xmlDocument.DocumentElement.Attributes(gc_AtributeTLTXCD).Value.ToString
            Dim v_strTXDATE As String = pv_xmlDocument.DocumentElement.Attributes(gc_AtributeTXDATE).Value.ToString

            'Get check rules
            v_strSQL = "SELECT TLTXCD, APPTYPE, RULECD, ACFLD, AMTEXP, FIELD, OPERAND, ERRNUM, ERRMSG, TBLNAME, FLDKEY " _
                & "FROM V_APPCHK_BY_TLTXCD APP WHERE APP.TLTXCD = '" & v_strTLTXCD & "' " _
                & "ORDER BY APPTYPE, TBLNAME, FLDKEY, FIELD, RULECD" 'Please do not change ORDER BY
            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            If v_ds.Tables(0).Rows.Count > 0 Then
                For i = 0 To v_ds.Tables(0).Rows.Count - 1 Step 1
                    'Lấy trư?ng số tài khoản
                    v_strACFLD = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(i)("ACFLD")))
                    v_nodetxData = pv_xmlDocument.SelectSingleNode("TransactMessage/fields/entry[@fldname='" & v_strACFLD & "']")
                    v_strACCTNO = v_nodetxData.InnerText

                    'Xác định biểu thức số h?c kiểm tra
                    v_strAMTEXP = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(i)("AMTEXP")))
                    If Len(v_strAMTEXP) > 0 Then
                        If Left(v_strAMTEXP, 1) = "@" Then
                            v_strVALUE = Mid(v_strAMTEXP, 2)
                        ElseIf Left(v_strAMTEXP, 1) = "$" Then
                            'Get field code: Sử dụng trong trư?ng hợp trư?ng có giá trị ký tự sẽ không áp dụng phép toán được
                            v_strVALUE = Mid(v_strAMTEXP, 2)
                            'Get field value
                            v_nodetxData = pv_xmlDocument.SelectSingleNode("TransactMessage/fields/entry[@fldname='" & v_strVALUE & "']")
                            v_strVALUE = v_nodetxData.InnerText
                        ElseIf v_strAMTEXP = "<$BUSDATE>" Then
                            'Get business date
                            v_strVALUE = v_strTXDATE
                        Else
                            'Armethic expression
                            v_strAMTEXP = BuildAMTEXP(pv_xmlDocument, v_strAMTEXP)
                            v_strVALUE = v_objEval.Eval(v_strAMTEXP).ToString
                        End If
                    End If

                    'Create appchk entry
                    v_entryNode = pv_xmlDocument.CreateNode(Xml.XmlNodeType.Element, "entry", "")

                    Dim v_attrAPPTYPE As Xml.XmlAttribute = pv_xmlDocument.CreateAttribute("apptype")
                    v_attrAPPTYPE.Value = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(i)("APPTYPE")))
                    v_entryNode.Attributes.Append(v_attrAPPTYPE)

                    Dim v_attrRULECD As Xml.XmlAttribute = pv_xmlDocument.CreateAttribute("rulecd")
                    v_attrRULECD.Value = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(i)("RULECD")))
                    v_entryNode.Attributes.Append(v_attrRULECD)

                    Dim v_attrACFLD As Xml.XmlAttribute = pv_xmlDocument.CreateAttribute("acfld")
                    v_attrACFLD.Value = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(i)("ACFLD")))
                    v_entryNode.Attributes.Append(v_attrACFLD)

                    Dim v_attrAMTEXP As Xml.XmlAttribute = pv_xmlDocument.CreateAttribute("amtexp")
                    v_attrAMTEXP.Value = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(i)("AMTEXP")))
                    v_entryNode.Attributes.Append(v_attrAMTEXP)

                    Dim v_attrFIELD As Xml.XmlAttribute = pv_xmlDocument.CreateAttribute("field")
                    v_attrFIELD.Value = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(i)("FIELD")))
                    v_entryNode.Attributes.Append(v_attrFIELD)

                    Dim v_attrOPERAND As Xml.XmlAttribute = pv_xmlDocument.CreateAttribute("operand")
                    v_attrOPERAND.Value = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(i)("OPERAND")))
                    v_entryNode.Attributes.Append(v_attrOPERAND)

                    Dim v_attrERRNUM As Xml.XmlAttribute = pv_xmlDocument.CreateAttribute("errnum")
                    v_attrERRNUM.Value = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(i)("ERRNUM")))
                    v_entryNode.Attributes.Append(v_attrERRNUM)

                    Dim v_attrERRMSG As Xml.XmlAttribute = pv_xmlDocument.CreateAttribute("errmsg")
                    v_attrERRMSG.Value = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(i)("ERRMSG")))
                    v_entryNode.Attributes.Append(v_attrERRMSG)

                    Dim v_attrTBLNAME As Xml.XmlAttribute = pv_xmlDocument.CreateAttribute("tblname")
                    v_attrTBLNAME.Value = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(i)("TBLNAME")))
                    v_entryNode.Attributes.Append(v_attrTBLNAME)

                    Dim v_attrFLDKEY As Xml.XmlAttribute = pv_xmlDocument.CreateAttribute("fldkey")
                    v_attrFLDKEY.Value = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(i)("FLDKEY")))
                    v_entryNode.Attributes.Append(v_attrFLDKEY)

                    Dim v_attrACCTNO As Xml.XmlAttribute = pv_xmlDocument.CreateAttribute("acctno")
                    v_attrACCTNO.Value = v_strACCTNO
                    v_entryNode.Attributes.Append(v_attrACCTNO)

                    v_entryNode.InnerText = v_strVALUE
                    v_appchkElement.AppendChild(v_entryNode)
                Next
            End If

            If Not (v_ds Is Nothing) Then
                v_ds.Dispose()
            End If

            pv_xmlDocument.DocumentElement.AppendChild(v_appchkElement)
            'ContextUtil.SetComplete()
            Return v_lngErrCode
        Catch ex As Exception
            'ContextUtil.SetAbort()
            'ex.Source = "Host.txRouter.GenAPPCHK"
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            '             & "Error code: System error!" & vbNewLine _
            '             & "Error message: " & ex.Message, EventLogEntryType.Error)
            Throw ex
        End Try
    End Function

    Private Function GenAPPMAP(ByRef pv_xmlDocument As Xml.XmlDocument) As Long
        Dim v_lngErrCode As Long = ERR_SYSTEM_OK
        Dim v_strErrorSource As String = "Host.txRouter.GenAPPMAP"
        Dim v_strSQL As String, v_ds As DataSet, v_obj As New DataAccess

        Try
            v_obj.NewDBInstance(gc_MODULE_HOST)
            Dim v_objEval As New Evaluator, v_nodetxData As Xml.XmlNode
            Dim i As Integer, v_strACFLD As String, v_strACFLDREF As String, v_strAMTEXP As String, v_strVALUE As String = "", v_strACCTNO As String


            Dim v_nodeGenChecking As Xml.XmlNode
            v_nodeGenChecking = pv_xmlDocument.SelectSingleNode("/TransactMessage/appmap")
            'Không tạo APPMAP nếu đã có
            If Not v_nodeGenChecking Is Nothing Then Return ERR_SYSTEM_OK

            'Get message information
            Dim v_strTLTXCD As String = pv_xmlDocument.DocumentElement.Attributes(gc_AtributeTLTXCD).Value.ToString
            Dim v_strTXDATE As String = pv_xmlDocument.DocumentElement.Attributes(gc_AtributeTXDATE).Value.ToString

            'Create Appmap Node
            Dim v_appmapElement As Xml.XmlElement, v_entryNode As Xml.XmlNode

            v_strSQL = "SELECT TLTXCD, APPTYPE, TBLNAME, APPTXCD, ACFLD, AMTEXP, FLDKEY, COND, ACFLDREF, TXTYPE, FIELD, FLDTYPE, TRANF, OFILE, OFILEACT " _
                & "FROM V_APPMAP_BY_TLTXCD APP WHERE APP.TLTXCD = '" & v_strTLTXCD & "' " _
                & " ORDER BY APPTYPE, ACFLD, TBLNAME, FIELD, FLDKEY, APPTXCD" 'Please do not change ORDER BY
            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            If v_ds.Tables(0).Rows.Count > 0 Then
                v_appmapElement = pv_xmlDocument.CreateElement(Xml.XmlNodeType.Element, "appmap", "")
                For i = 0 To v_ds.Tables(0).Rows.Count - 1 Step 1
                    'Lấy trư?ng số tài khoản
                    v_strACFLD = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(i)("ACFLD")))
                    v_nodetxData = pv_xmlDocument.SelectSingleNode("TransactMessage/fields/entry[@fldname='" & v_strACFLD & "']")
                    v_strACCTNO = v_nodetxData.InnerText

                    'Xác định trư?ng tài khoản đối ứng
                    v_strACFLDREF = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(i)("ACFLDREF")))
                    If Len(v_strACFLDREF) <> 0 Then
                        v_nodetxData = pv_xmlDocument.SelectSingleNode("TransactMessage/fields/entry[@fldname='" & v_strACFLDREF & "']")
                        v_strACFLDREF = v_nodetxData.InnerText
                    End If

                    'Xác định biểu thức số h?c kiểm tra
                    v_strAMTEXP = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(i)("AMTEXP")))
                    If Len(v_strAMTEXP) > 0 Then
                        If Left(v_strAMTEXP, 1) = "@" Then
                            v_strVALUE = Mid(v_strAMTEXP, 2)
                        ElseIf Left(v_strAMTEXP, 1) = "$" Then
                            'Get field code
                            v_strVALUE = Mid(v_strAMTEXP, 2)
                            'Get field value
                            v_nodetxData = pv_xmlDocument.SelectSingleNode("TransactMessage/fields/entry[@fldname='" & v_strVALUE & "']")
                            v_strVALUE = v_nodetxData.InnerText
                        ElseIf v_strAMTEXP = "<$BUSDATE>" Then
                            'Get business date
                            v_strVALUE = v_strTXDATE
                        Else

                            If Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(i)("TXTYPE"))) = "U" Then
                                'Neu la update thi lay gia tri 
                                v_strAMTEXP = BuildAMTEXP(pv_xmlDocument, v_strAMTEXP)
                                v_strVALUE = v_strAMTEXP
                            Else
                                'Credit or Debit Armethic expression
                                v_strAMTEXP = BuildAMTEXP(pv_xmlDocument, v_strAMTEXP)
                                v_strVALUE = v_objEval.Eval(v_strAMTEXP).ToString
                            End If

                        End If
                    End If

                    'Create appmap entry
                    v_entryNode = pv_xmlDocument.CreateNode(Xml.XmlNodeType.Element, "entry", "")

                    Dim v_attrAPPTYPE As Xml.XmlAttribute = pv_xmlDocument.CreateAttribute("apptype")
                    v_attrAPPTYPE.Value = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(i)("APPTYPE")))
                    v_entryNode.Attributes.Append(v_attrAPPTYPE)

                    Dim v_attrTBLNAME As Xml.XmlAttribute = pv_xmlDocument.CreateAttribute("tblname")
                    v_attrTBLNAME.Value = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(i)("TBLNAME")))
                    v_entryNode.Attributes.Append(v_attrTBLNAME)

                    Dim v_attrAPPTXCD As Xml.XmlAttribute = pv_xmlDocument.CreateAttribute("apptxcd")
                    v_attrAPPTXCD.Value = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(i)("APPTXCD")))
                    v_entryNode.Attributes.Append(v_attrAPPTXCD)

                    Dim v_attrAMTEXP As Xml.XmlAttribute = pv_xmlDocument.CreateAttribute("amtexp")
                    v_attrAMTEXP.Value = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(i)("AMTEXP")))
                    v_entryNode.Attributes.Append(v_attrAMTEXP)

                    Dim v_attrACFLD As Xml.XmlAttribute = pv_xmlDocument.CreateAttribute("acfld")
                    v_attrACFLD.Value = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(i)("ACFLD")))
                    v_entryNode.Attributes.Append(v_attrACFLD)

                    Dim v_attrFLDKEY As Xml.XmlAttribute = pv_xmlDocument.CreateAttribute("fldkey")
                    v_attrFLDKEY.Value = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(i)("FLDKEY")))
                    v_entryNode.Attributes.Append(v_attrFLDKEY)

                    Dim v_attrACFLDREF As Xml.XmlAttribute = pv_xmlDocument.CreateAttribute("acfldref")
                    v_attrACFLDREF.Value = v_strACFLDREF
                    v_entryNode.Attributes.Append(v_attrACFLDREF)

                    Dim v_attrCOND As Xml.XmlAttribute = pv_xmlDocument.CreateAttribute("cond")
                    v_attrCOND.Value = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(i)("COND")))
                    v_entryNode.Attributes.Append(v_attrCOND)

                    Dim v_attrTXTYPE As Xml.XmlAttribute = pv_xmlDocument.CreateAttribute("txtype")
                    v_attrTXTYPE.Value = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(i)("TXTYPE")))
                    v_entryNode.Attributes.Append(v_attrTXTYPE)

                    Dim v_attrFIELD As Xml.XmlAttribute = pv_xmlDocument.CreateAttribute("field")
                    v_attrFIELD.Value = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(i)("FIELD")))
                    v_entryNode.Attributes.Append(v_attrFIELD)

                    Dim v_attrFLDTYPE As Xml.XmlAttribute = pv_xmlDocument.CreateAttribute("fldtype")
                    v_attrFLDTYPE.Value = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(i)("FLDTYPE")))
                    v_entryNode.Attributes.Append(v_attrFLDTYPE)

                    Dim v_attrTRANF As Xml.XmlAttribute = pv_xmlDocument.CreateAttribute("tranf")
                    v_attrTRANF.Value = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(i)("TRANF")))
                    v_entryNode.Attributes.Append(v_attrTRANF)

                    Dim v_attrOFILE As Xml.XmlAttribute = pv_xmlDocument.CreateAttribute("ofile")
                    v_attrOFILE.Value = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(i)("OFILE")))
                    v_entryNode.Attributes.Append(v_attrOFILE)

                    Dim v_attrOFILEACT As Xml.XmlAttribute = pv_xmlDocument.CreateAttribute("ofileact")
                    v_attrOFILEACT.Value = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(i)("OFILEACT")))
                    v_entryNode.Attributes.Append(v_attrOFILEACT)

                    Dim v_attrACCTNO As Xml.XmlAttribute = pv_xmlDocument.CreateAttribute("acctno")
                    v_attrACCTNO.Value = v_strACCTNO
                    v_entryNode.Attributes.Append(v_attrACCTNO)

                    v_entryNode.InnerText = v_strVALUE
                    v_appmapElement.AppendChild(v_entryNode)
                Next

                pv_xmlDocument.DocumentElement.AppendChild(v_appmapElement)
            End If

            If Not (v_ds Is Nothing) Then
                v_ds.Dispose()
            End If
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


    'Hàm này được g?i để thực hiện kiểm tra các phân hệ nghiệp vụ
    'Biến vào:  pv_xmlDocument là xmlDocument đã được GenAPPCHK
    Private Function HostAppCheck(ByRef pv_xmlDocument As Xml.XmlDocument) As Long
        Dim v_lngErrCode As Long = ERR_SYSTEM_OK
        Dim v_strErrorSource As String = "Host.txRouter.HostAppCheck", v_strErrorMessage As String = ""
        Dim v_DataAccess As New DataAccess, v_ds As DataSet, v_strSQL As String
        Try
            Dim i As Integer
            Dim v_strMODULE As String = ""
            Dim v_appMODULE As Sats.CoreBusiness.txMaster = Nothing
            Dim v_strTLTXCD As String = pv_xmlDocument.DocumentElement.Attributes(gc_AtributeTLTXCD).Value.ToString

            v_DataAccess.NewDBInstance(gc_MODULE_HOST)
            'Xác định các phân hệ nghiệp vụ để kiểm tra
            v_strSQL = "SELECT DISTINCT APPTYPE FROM APPMAP WHERE TLTXCD ='" & v_strTLTXCD & "'"
            v_ds = v_DataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

            If v_ds.Tables(0).Rows.Count > 0 Then
                For i = 0 To v_ds.Tables(0).Rows.Count - 1
                    'Xác định tên phân hệ nghiệp vụ
                    v_strMODULE = Trim(v_ds.Tables(0).Rows(i)(0))
                    Select Case v_strMODULE
                        Case SUB_SYSTEM_DE
                            v_appMODULE = New Sats.DE.Trans
                        Case SUB_SYSTEM_CA
                            v_appMODULE = New Sats.CA.Trans
                        Case SUB_SYSTEM_MF
                            v_appMODULE = New Sats.MF.Trans
                        Case SUB_SYSTEM_CS
                            v_appMODULE = New Sats.CS.Trans
                        Case SUB_SYSTEM_SF
                            v_appMODULE = New Sats.CS.Trans
                    End Select
                    'Kiểm tra luôn
                    If Not v_appMODULE Is Nothing Then
                        v_lngErrCode = v_appMODULE.txCheck(pv_xmlDocument)
                        If v_lngErrCode <> ERR_SYSTEM_OK And v_lngErrCode <> ERR_SA_CHECKER1_OVR And v_lngErrCode <> ERR_SA_CHECKER2_OVR Then
                            v_strErrorMessage = v_strErrorSource & ".Step: " & v_strMODULE & ".txCheck"
                            'Trả v? mã lỗi
                            'ContextUtil.SetAbort()
                            Return v_lngErrCode
                        End If
                    End If
                Next
            End If

            'ContextUtil.SetComplete()
            Return v_lngErrCode
        Catch ex As Exception
            'ContextUtil.SetAbort()
            ex.Source = "Host.txRouter.HostAppCheck"
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            '             & "Error code: " & v_strErrorMessage & vbNewLine _
            '             & "Error message: " & ex.Message, EventLogEntryType.Error)
            Throw ex
        Finally
            If Not (v_DataAccess Is Nothing) Then
                v_DataAccess.Dispose()
            End If
            If Not v_ds Is Nothing Then
                v_ds.Dispose()
            End If
        End Try
    End Function

    'Hàm này được g?i để thực hiện cập nhật các phân hệ nghiệp vụ
    'Biến vào:  pv_xmlDocument là xmlDocument đã được GenAPPMAP
    Private Function HostAppUpdate(ByRef pv_xmlDocument As Xml.XmlDocument) As Long
        Dim v_lngErrCode As Long = ERR_SYSTEM_OK
        Dim v_strErrorSource As String = "Host.txRouter.HostAppUpdate", v_strErrorMessage As String = ""
        Try
            Dim v_ds As DataSet = Nothing
            Dim v_strSQL As String = ""
            Dim i As Integer
            Dim v_strMODULE As String = ""
            Dim v_appMODULE As CoreBusiness.txMaster = Nothing
            Dim v_strTLTXCD As String = pv_xmlDocument.DocumentElement.Attributes(gc_AtributeTLTXCD).Value.ToString
            Dim v_obj As New DataAccess
            v_obj.NewDBInstance(gc_MODULE_HOST)
            'Xác định các phân hệ nghiệp vụ để kiểm tra
            v_strSQL = "SELECT DISTINCT APPTYPE FROM APPMAP WHERE TLTXCD ='" & v_strTLTXCD & "'"
            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            If Not (v_obj Is Nothing) Then
                v_obj.Dispose()
            End If

            If v_ds.Tables(0).Rows.Count > 0 Then
                For i = 0 To v_ds.Tables(0).Rows.Count - 1
                    'Xác định tên phân hệ nghiệp vụ
                    v_strMODULE = Trim(v_ds.Tables(0).Rows(i)(0))
                    Select Case v_strMODULE
                        Case SUB_SYSTEM_DE
                            v_appMODULE = New Sats.DE.Trans
                        Case SUB_SYSTEM_CA
                            v_appMODULE = New Sats.CA.Trans
                        Case SUB_SYSTEM_MF
                            v_appMODULE = New Sats.MF.Trans
                        Case SUB_SYSTEM_CS
                            v_appMODULE = New Sats.CS.Trans
                        Case SUB_SYSTEM_SF
                            v_appMODULE = New Sats.SF.Trans
                    End Select
                    'Kiểm tra luôn
                    If Not v_appMODULE Is Nothing Then
                        v_lngErrCode = v_appMODULE.txUpdate(pv_xmlDocument)
                        If v_lngErrCode <> ERR_SYSTEM_OK Then
                            v_strErrorMessage = v_strErrorSource & ".Step: " & v_strMODULE & ".HostAppUpdate"
                            'Trả v? mã lỗi
                            'ContextUtil.SetAbort()
                            Return v_lngErrCode
                        End If
                    End If
                Next
            End If

            'ContextUtil.SetComplete()
            Return v_lngErrCode
        Catch ex As Exception
            'ContextUtil.SetAbort()
            'LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
            '             & "Error code: " & v_strErrorMessage & vbNewLine _
            '             & "Error message: " & ex.Message, EventLogEntryType.Error)
            Throw ex
        End Try
    End Function

    'Hàm này được g?i để thực hiện cập nhật GL và TLLOG
    'Biến vào:  pv_xmlDocument là xmlDocument đã có POSTMAP
    Private Function HostTransUpdate(ByRef pv_xmlDocument As Xml.XmlDocument) As Long
        Dim v_lngErrCode As Long = ERR_SYSTEM_OK
        Dim v_strErrorSource As String = "Host.txRouter.HostTransUpdate"
        Dim v_DataAccess As New DataAccess
        'Dim i As Integer, v_strMODULE As String, v_appMODULE As CoreBusiness.txMaster
        Dim v_strTXDATE As String = pv_xmlDocument.DocumentElement.Attributes(gc_AtributeTXDATE).Value.ToString
        Dim v_strTXNUM As String = pv_xmlDocument.DocumentElement.Attributes(gc_AtributeTXNUM).Value.ToString
        Dim v_strTLTXCD As String = pv_xmlDocument.DocumentElement.Attributes(gc_AtributeTLTXCD).Value.ToString
        Dim v_strDELTD As String = pv_xmlDocument.DocumentElement.Attributes(gc_AtributeDELTD).Value.ToString
        Dim v_strBATCHNAME As String = pv_xmlDocument.DocumentElement.Attributes(gc_AtributeBATCHNAME).Value.ToString
        Dim v_blnReversal As Boolean = IIf(v_strDELTD = "Y", True, False)
        Try
            'Cập nhật TLLOG
            Dim v_objMsgLog As New MessageLog
            v_objMsgLog.NewDBInstance(gc_MODULE_HOST)
            v_DataAccess.NewDBInstance(gc_MODULE_HOST)
            If Not v_blnReversal Then
                'Kiểm tra nếu là giao dịch chạy batch thì chỉ cập nhật lại trạng thái
                If v_strBATCHNAME.Trim <> DAILY_TRANSACTION Then
                    v_lngErrCode = v_objMsgLog.TransUpdateStatus(pv_xmlDocument)
                Else
                    v_lngErrCode = v_objMsgLog.TransLog(pv_xmlDocument)
                End If
                If v_lngErrCode <> ERR_SYSTEM_OK Then
                    'ContextUtil.SetAbort()
                    Return v_lngErrCode
                End If
            Else
                v_lngErrCode = v_objMsgLog.TransDelete(pv_xmlDocument)
                If v_lngErrCode <> ERR_SYSTEM_OK Then
                    'ContextUtil.SetAbort()
                    Return v_lngErrCode
                End If
            End If

            If Not (v_DataAccess Is Nothing) Then
                v_DataAccess.Dispose()
            End If

            'ContextUtil.SetComplete()
            Return v_lngErrCode
        Catch ex As Exception
            'ContextUtil.SetAbort()
            'LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
            '             & "Error code: System error!" & vbNewLine _
            '             & "Error message: " & v_strTLTXCD & "." & v_strTXDATE & "." & v_strTXNUM, EventLogEntryType.Error)
            Throw ex
        End Try
    End Function

    'Public Function CheckHOST(ByRef v_xmlDocument As Xml.XmlDocument) As Long
    '    Dim v_strErrorSource As String = "Branch.Branch.CheckBDS", v_strErrorMessage As String = ""
    '    Dim v_lngError As Long = ERR_SYSTEM_OK
    '    Dim v_strTellerId, v_strSQL, v_strTLTXCD, v_strCOTLTXCD As String
    '    Dim v_strSQLTmp, v_strReplaceSQL, v_strFieldsSQL, v_strValuesSQL As String
    '    Dim intCountNo As Integer

    '    Dim v_trace As DataSet
    '    Dim v_trace_status, v_trace_path As String
    '    Dim tr2 As New TextWriterTraceListener
    '    Dim v_CheckTTDataAccess As New DataAccess
    '    Dim v_nodeList As Xml.XmlNodeList
    '    Dim v_nodelist1 As Xml.XmlNodeList

    '    Try

    '        Dim v_bCmd As New BusinessCommand
    '        Dim v_ds, v_Msg, v_replace As DataSet
    '        Dim v_strErrMsg As String = ""
    '        Dim v_strWarningMsg As String = ""
    '        Dim v_intErrCount As Integer = 0
    '        Dim v_intWarningCount As Integer = 0
    '        Dim i, j, intIndex As Integer

    '        v_CheckTTDataAccess.NewDBInstance(gc_MODULE_HOST)

    '        v_trace_status = "0"
    '        Dim v_strBRID1 As String = v_xmlDocument.DocumentElement.Attributes(gc_AtributeBRID).Value.ToString
    '        Dim v_strCurrDate As String
    '        v_CheckTTDataAccess.GetSysVar("SYSTEM", "TRACE_STATUS", v_trace_status)
    '        v_CheckTTDataAccess.GetSysVar("SYSTEM", "CURRDATE", v_strBRID1, v_strCurrDate)
    '        v_CheckTTDataAccess.GetSysVar("SYSTEM", "TRACE_PATH", v_trace_path)

    '        If InStr(v_xmlDocument.InnerXml, "ROOT") > 0 Then
    '            v_nodeList = v_xmlDocument.SelectNodes("/ROOT/TransactMessage")
    '        Else
    '            v_nodeList = v_xmlDocument.SelectNodes("/TransactMessage")
    '        End If

    '        For intIndex = 0 To v_nodeList.Count - 1
    '            v_CheckTTDataAccess.BeginTran()
    '            With v_nodeList.Item(intIndex)
    '                v_intErrCount = 0
    '                v_intWarningCount = 0
    '                v_strErrMsg = ""
    '                v_strWarningMsg = ""
    '                v_strTLTXCD = .Attributes(gc_AtributeTLTXCD).InnerXml
    '                Dim v_strSTATUS As String = .Attributes(gc_AtributeSTATUS).Value.ToString
    '                Dim v_strSTATUS_TEXT As String = .Attributes(gc_AtributeSTATUSTEXT).Value.ToString
    '                Dim v_strAUTOID As String = .Attributes(gc_AtributeAUTOID).Value.ToString
    '                Dim v_strTXNUM As String = .Attributes(gc_AtributeTXNUM).Value.ToString
    '                Dim v_strTXDATE As String = .Attributes(gc_AtributeTXDATE).Value.ToString
    '                Dim v_strBUSDATE As String = .Attributes(gc_AtributeBUSDATE).Value.ToString
    '                Dim v_strTXDESC As String = .Attributes(gc_AtributeTXDESC).Value.ToString
    '                Dim v_strBRID As String = .Attributes(gc_AtributeBRID).Value.ToString
    '                Dim v_strBRCODE As String = .Attributes(gc_AtributeBRCODE).Value.ToString
    '                Dim v_strTLID As String = .Attributes(gc_AtributeTLID).Value.ToString
    '                Dim v_strCHKID As String = .Attributes(gc_AtributeCHKID).Value.ToString
    '                Dim v_strOFFID As String = .Attributes(gc_AtributeOFFID).Value.ToString
    '                Dim v_strCFRID As String = .Attributes(gc_AtributeCFRID).Value.ToString
    '                Dim v_strTLNAME As String = .Attributes(gc_AtributeTLNAME).Value.ToString
    '                Dim v_strCHKNAME As String = .Attributes(gc_AtributeCHKNAME).Value.ToString
    '                Dim v_strOFFNAME As String = .Attributes(gc_AtributeOFFNAME).Value.ToString
    '                Dim v_strCFRNAME As String = .Attributes(gc_AtributeCFRNAME).Value.ToString
    '                Dim v_strIPADDRESS As String = .Attributes(gc_AtributeIPADDRESS).Value.ToString
    '                Dim v_strWSNAME As String = .Attributes(gc_AtributeWSNAME).Value.ToString
    '                Dim v_strSICODE As String = .Attributes(gc_AtributeSICODE).Value.ToString
    '                Dim v_strMICODE As String = .Attributes(gc_AtributeMICODE).Value.ToString
    '                Dim v_strCOMICODE As String = .Attributes(gc_AtributeCOMICODE).Value.ToString
    '                Dim v_strMSGAMT As String = .Attributes(gc_AtributeMSGAMT).Value.ToString
    '                Dim v_strISPARENT As String = .Attributes(gc_AtributeISPARENT).Value.ToString
    '                Dim v_strPARENTID As String = .Attributes(gc_AtributePARENTID).Value.ToString
    '                Dim v_strPARENT_TEXT As String = .Attributes(gc_AtributePARENT_TEXT).Value.ToString
    '                Dim v_strTXNAME As String = .Attributes(gc_AtributeTXNAME).Value.ToString
    '                Dim v_strCHILDTLTXCD As String = .Attributes(gc_AtributeCHILDTLTXCD).Value.ToString
    '                Dim v_strISBRID As String = .Attributes(gc_AtributeISBRID).Value.ToString
    '                Dim v_strMISSING_WARNING As String = .Attributes(gc_AtributeMISSING_WARNING).Value.ToString
    '                Dim v_strOLDSTATUS As String = .Attributes(gc_AtributeOLDSTATUS).Value.ToString

    '                If v_trace_status = "1" Then
    '                    Dim v_strTLName1 As String = v_strCFRNAME
    '                    Dim v_trace_path1 As String = ""
    '                    If v_strTLName1 = "" Then
    '                        v_strTLName1 = v_strOFFNAME
    '                    End If
    '                    If v_strTLName1 = "" Then
    '                        v_strTLName1 = v_strCHKNAME
    '                    End If
    '                    If v_strTLName1 = "" Then
    '                        v_strTLName1 = v_strTLNAME
    '                    End If
    '                    v_strCurrDate = Replace(v_strCurrDate, "/", "_")

    '                    If v_trace_path = "" Then
    '                        Dim v_app As New ApplicationServices.ApplicationBase
    '                        v_trace_path1 = v_app.Info.DirectoryPath & "\Log\" & v_strCurrDate
    '                    Else
    '                        v_trace_path1 = v_trace_path & v_strCurrDate
    '                    End If

    '                    If Not System.IO.Directory.Exists(v_trace_path1) Then
    '                        System.IO.Directory.CreateDirectory(v_trace_path1)
    '                    End If

    '                    v_trace_path1 &= "\log_chk_br" & v_strBRID & "_" & v_strTLName1 & ".txt"

    '                    tr2 = New TextWriterTraceListener(System.IO.File.AppendText(v_trace_path1))
    '                    Trace.Listeners.Add(tr2)
    '                    Trace.WriteLine("[Bắt đầu: APPCHK - " & v_strTLTXCD & "] " & DateTime.Now & vbCrLf)
    '                End If

    '                'v_strSQL = "select ORDNUM, trace, type, CHECKSQL || NVL(CHECKSQL1,'') as CHECKSQL from appchk where status = 0 and deleted = 0 and ((tltxcd = '" & v_strTLTXCD & "') or (tltxcd is null)) and instr(txstatus,'" & v_strSTATUS & "')>0 order by ordnum"
    '                v_strSQL = "select ORDNUM, trace, type, CHECKSQL , CHECKSQL1 from appchk where status = 0 and deleted = 0 and ((tltxcd = '" & v_strTLTXCD & "') or (tltxcd is null)) and instr(txstatus,'" & v_strSTATUS & "')>0 order by ordnum"
    '                v_bCmd.SQLCommand = v_strSQL
    '                v_ds = v_CheckTTDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_bCmd.SQLCommand)
    '                If v_ds.Tables(0).Rows.Count > 0 Then
    '                    ' insert data
    '                    v_nodelist1 = v_xmlDocument.SelectNodes("/TransactMessage/fields")
    '                    If v_nodelist1.Count > 0 Then
    '                        For i = 0 To v_nodelist1.Count - 1
    '                            For j = 0 To v_nodelist1.Item(i).ChildNodes.Count - 1
    '                                With v_nodelist1.Item(i).ChildNodes(j)
    '                                    v_strSQLTmp = .InnerText

    '                                    v_strSQLTmp = Replace(v_strSQLTmp, "TT_", "")
    '                                    v_strSQLTmp = Replace(v_strSQLTmp, "tt_", "")
    '                                    v_strSQLTmp = Replace(v_strSQLTmp, "tT_", "")
    '                                    v_strSQLTmp = Replace(v_strSQLTmp, "Tt_", "")

    '                                    v_CheckTTDataAccess.ExecuteNonQuery(CommandType.Text, v_strSQLTmp)
    '                                End With
    '                            Next
    '                        Next
    '                    Else
    '                        v_strReplaceSQL = "select max(fldname) from fldmaster where objname = '" & v_strTLTXCD & "' and deleted=0 and status=0"
    '                        v_bCmd.SQLCommand = v_strReplaceSQL
    '                        v_replace = v_CheckTTDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_bCmd.SQLCommand)
    '                        If v_replace.Tables(0).Rows.Count > 0 Then
    '                            intCountNo = v_replace.Tables(0).Rows(0)(0)
    '                        End If
    '                        v_strFieldsSQL = ""
    '                        v_strValuesSQL = ""
    '                        For j = 1 To intCountNo
    '                            v_strFieldsSQL = v_strFieldsSQL & ", COL_VALUE" & String.Format("{0:00}", j) _
    '                            & ", COL_TYPE" & String.Format("{0:00}", j) _
    '                            & ", COL_DESC" & String.Format("{0:00}", j)
    '                        Next
    '                        v_strSQLTmp = "INSERT INTO TMP_TXFIELDS(AUTOID, TLTXCD " & v_strFieldsSQL & ") " _
    '                        & " SELECT SEQ_TMP_TXFIELDS.NEXTVAL, TLTXCD " & v_strFieldsSQL & " FROM " _
    '                        & " ( SELECT " _
    '                        & " '" & v_strTLTXCD & "' TLTXCD" & v_strFieldsSQL _
    '                        & " FROM  TLLOG " _
    '                        & " WHERE deleted =0 and " & IIf(v_strISPARENT = "2", "PARENTID = ", "AUTOID = ") & v_strAUTOID _
    '                        & " )"

    '                        v_strSQLTmp = Replace(v_strSQLTmp, "TT_", "")
    '                        v_strSQLTmp = Replace(v_strSQLTmp, "tt_", "")
    '                        v_strSQLTmp = Replace(v_strSQLTmp, "tT_", "")
    '                        v_strSQLTmp = Replace(v_strSQLTmp, "Tt_", "")
    '                        v_CheckTTDataAccess.ExecuteNonQuery(CommandType.Text, v_strSQLTmp)
    '                    End If


    '                    ' Kiem tra thong tin tren timesten
    '                    For i = 0 To v_ds.Tables(0).Rows.Count - 1
    '                        v_strSQL = v_ds.Tables(0).Rows(i)("CHECKSQL") & " " & gf_CorrectStringField(v_ds.Tables(0).Rows(i)("CHECKSQL1"))
    '                        ' thay the cac transaction message 
    '                        ' nhom trang thai giao dich
    '                        v_strSQL = Replace(v_strSQL, "?STATUS_TEXT", "'" & v_strSTATUS_TEXT & "'")
    '                        v_strSQL = Replace(v_strSQL, "?STATUS", v_strSTATUS)
    '                        v_strSQL = Replace(v_strSQL, "?OLDSTATUS", v_strOLDSTATUS)
    '                        ' nhom ma giao dich
    '                        v_strSQL = Replace(v_strSQL, "?AUTOID", v_strAUTOID)
    '                        v_strSQL = Replace(v_strSQL, "?TXNUM", "'" & v_strTXNUM & "'")
    '                        v_strSQL = Replace(v_strSQL, "?TXNAME", "'" & v_strTXNAME & "'")
    '                        v_strSQL = Replace(v_strSQL, "?TXDATE", "TO_DATE('" & v_strTXDATE & "','dd/mm/yyyy')")
    '                        'v_strSQL = Replace(v_strSQL, "?BUSDATE", IIf(IsDate(v_strBUSDATE), "TO_DATE('" & v_strBUSDATE & "','dd/mm/yyyy')", "NULL"))
    '                        v_strSQL = Replace(v_strSQL, "?BUSDATE", "TO_DATE('" & v_strBUSDATE & "','dd/mm/yyyy')")
    '                        ' nhom loai giao dich
    '                        v_strSQL = Replace(v_strSQL, "?TLTXCD", "'" & v_strTLTXCD & "'")
    '                        v_strSQL = Replace(v_strSQL, "?TXDESC", "'" & v_strTXDESC & "'")
    '                        ' nhom chi nhanh
    '                        v_strSQL = Replace(v_strSQL, "?BRID", "'" & v_strBRID & "'")
    '                        v_strSQL = Replace(v_strSQL, "?BRCODE", "'" & v_strBRCODE & "'")
    '                        ' nhom user
    '                        v_strSQL = Replace(v_strSQL, "?TLID", "'" & v_strTLID & "'")
    '                        v_strSQL = Replace(v_strSQL, "?OFFID", "'" & v_strOFFID & "'")
    '                        v_strSQL = Replace(v_strSQL, "?CFRID", "'" & v_strCFRID & "'")
    '                        v_strSQL = Replace(v_strSQL, "?CHKID", "'" & v_strCHKID & "'")
    '                        v_strSQL = Replace(v_strSQL, "?TLNAME", "'" & v_strTLNAME & "'")
    '                        v_strSQL = Replace(v_strSQL, "?OFFNAME", "'" & v_strOFFNAME & "'")
    '                        v_strSQL = Replace(v_strSQL, "?CHKNAME", "'" & v_strCHKNAME & "'")
    '                        v_strSQL = Replace(v_strSQL, "?CFRNAME", "'" & v_strCFRNAME & "'")
    '                        ' nhom dia chi tao giao dich
    '                        v_strSQL = Replace(v_strSQL, "?IPADDRESS", "'" & v_strIPADDRESS & "'")
    '                        v_strSQL = Replace(v_strSQL, "?WSNAME", "'" & v_strWSNAME & "'")
    '                        ' nhom quyen
    '                        v_strSQL = Replace(v_strSQL, "?SICODE", "'" & v_strSICODE & "'")
    '                        v_strSQL = Replace(v_strSQL, "?MICODE", "'" & v_strMICODE & "'")
    '                        v_strSQL = Replace(v_strSQL, "?COMICODE", "'" & v_strCOMICODE & "'")
    '                        v_strSQL = Replace(v_strSQL, "?MSGAMT", IIf(v_strMSGAMT <> "", "TO_NUMBER(COL_VALUE" & v_strMSGAMT & ")", "0"))
    '                        ' nhom giao dich cha-con
    '                        v_strSQL = Replace(v_strSQL, "?ISPARENT", v_strISPARENT)
    '                        v_strSQL = Replace(v_strSQL, "?PARENTID", v_strPARENTID)
    '                        v_strSQL = Replace(v_strSQL, "?PARENT_TEXT", "'" & v_strPARENT_TEXT & "'")
    '                        v_strSQL = Replace(v_strSQL, "?CHILDTLTXCD", "'" & v_strCHILDTLTXCD & "'")
    '                        v_strSQL = Replace(v_strSQL, "?ISBRID", v_strISBRID)

    '                        If v_trace_status = "1" And v_ds.Tables(0).Rows(i)("TRACE") = 1 Then
    '                            Trace.WriteLine("[APPCHK - " & v_strTLTXCD & "] " & DateTime.Now & " :" & vbCrLf & " -o- Câu lệnh thứ #" & v_ds.Tables(0).Rows(i)("ORDNUM") & "-o-" & vbCrLf & v_strSQL & vbCrLf)
    '                            If InStr(v_strSQL.ToUpper, "SELECT") > 0 Then
    '                                Trace.WriteLine("-o- Dữ liệu câu lệnh thứ #" & v_ds.Tables(0).Rows(i)("ORDNUM") & "-o-")
    '                                v_strSQLTmp = v_strSQL.Substring(InStr(v_strSQL.ToUpper, "SELECT") - 1)

    '                                v_strSQLTmp = Replace(v_strSQLTmp, "TT_", "")
    '                                v_strSQLTmp = Replace(v_strSQLTmp, "tt_", "")
    '                                v_strSQLTmp = Replace(v_strSQLTmp, "tT_", "")
    '                                v_strSQLTmp = Replace(v_strSQLTmp, "Tt_", "")
    '                                v_trace = v_CheckTTDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
    '                                Trace.WriteLine(v_trace.GetXml & vbCrLf)
    '                            End If
    '                        End If

    '                        If gf_CorrectStringField(v_ds.Tables(0).Rows(i)("TYPE")) = "C" Then
    '                            If InStr(v_strSQL, "?CHECK_DUPLICATION") > 0 Then
    '                                If v_strISPARENT = "0" Then
    '                                    v_strSQLTmp = "select fldname from fldmaster where status = 0 and deleted = 0 " _
    '                                    & " and objname = '" & v_strTLTXCD & "' and ISDUPLICATED='Y' order by fldname"
    '                                    v_trace = v_CheckTTDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
    '                                    Dim v_strFileterDup As String = ""
    '                                    For intDup As Integer = 0 To v_trace.Tables(0).Rows.Count - 1
    '                                        v_strFileterDup = v_strFileterDup & " and nvl(a.col_value" & v_trace.Tables(0).Rows(intDup)(0).ToString & ",'#') " _
    '                                        & " = nvl(b.col_value" & v_trace.Tables(0).Rows(intDup)(0).ToString & ",'#') " & vbCrLf
    '                                    Next
    '                                    If v_strFileterDup <> "" Then
    '                                        v_strSQL = "select N'Giao dá»‹ch nÃ y trÃ¹ng vá»›i giao dá»‹ch (' || a.txnum  || ', ' " _
    '                                        & " || to_char(a.txdate,'dd/mm/yyyy') || ')',b.REAL_ROW " _
    '                                        & " from tllog a, TMP_txfields b" _
    '                                        & " where a.parentid =" & v_strAUTOID _
    '                                        & v_strFileterDup
    '                                    Else
    '                                        v_strSQL = "select N'' from dual where 1=0"
    '                                    End If
    '                                Else
    '                                    v_strSQL = "select N'' from dual where 1=0"
    '                                End If
    '                            End If

    '                            v_strSQL = Replace(v_strSQL, "TT_", "")
    '                            v_strSQL = Replace(v_strSQL, "tt_", "")
    '                            v_strSQL = Replace(v_strSQL, "tT_", "")
    '                            v_strSQL = Replace(v_strSQL, "Tt_", "")
    '                            v_Msg = v_CheckTTDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

    '                            For j = 0 To v_Msg.Tables(0).Rows.Count - 1
    '                                If gf_CorrectStringField(v_Msg.Tables(0).Rows(j)(0)).Trim <> "" Then
    '                                    If v_Msg.Tables(0).Columns.Count >= 3 Then
    '                                        If gf_CorrectNumericField(v_Msg.Tables(0).Rows(j)(2)) = "2" Then
    '                                            If v_strMISSING_WARNING = "0" Then
    '                                                v_intWarningCount = v_intWarningCount + 1
    '                                                v_strWarningMsg = v_strWarningMsg & "Dòng " & v_Msg.Tables(0).Rows(j)(1) & ": "
    '                                                v_strWarningMsg = v_strWarningMsg & v_Msg.Tables(0).Rows(j)(0) & vbCrLf
    '                                            End If
    '                                        Else
    '                                            v_intErrCount = v_intErrCount + 1
    '                                            v_strErrMsg = v_strErrMsg & "Dòng " & v_Msg.Tables(0).Rows(j)(1) & ": "
    '                                            v_strErrMsg = v_strErrMsg & v_Msg.Tables(0).Rows(j)(0) & vbCrLf
    '                                        End If
    '                                    Else
    '                                        v_intErrCount = v_intErrCount + 1
    '                                        If v_Msg.Tables(0).Columns.Count >= 2 Then
    '                                            v_strErrMsg = v_strErrMsg & "Dòng " & v_Msg.Tables(0).Rows(j)(1) & ": "
    '                                        End If
    '                                        v_strErrMsg = v_strErrMsg & v_Msg.Tables(0).Rows(j)(0) & vbCrLf
    '                                    End If
    '                                End If
    '                            Next
    '                        Else
    '                            v_strSQL = Replace(v_strSQL, "TT_", "")
    '                            v_strSQL = Replace(v_strSQL, "tt_", "")
    '                            v_strSQL = Replace(v_strSQL, "tT_", "")
    '                            v_strSQL = Replace(v_strSQL, "Tt_", "")

    '                            v_CheckTTDataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
    '                        End If
    '                    Next
    '                End If
    '                If v_intErrCount + v_intWarningCount > 0 Then
    '                    Dim v_attrFLDNAME, v_attrCount, v_attrTXNUM, v_attrTXDATE As Xml.XmlAttribute
    '                    Dim v_dataElement As Xml.XmlElement
    '                    Dim v_entryNode As Xml.XmlNode

    '                    v_dataElement = v_xmlDocument.CreateElement(Xml.XmlNodeType.Element, "FAILED_MESSAGE", "")
    '                    v_attrTXNUM = v_xmlDocument.CreateAttribute(gc_AtributeTXNUM)
    '                    v_attrTXNUM.Value = v_strTXNUM
    '                    v_dataElement.Attributes.Append(v_attrTXNUM)
    '                    v_attrTXDATE = v_xmlDocument.CreateAttribute(gc_AtributeTXDATE)
    '                    v_attrTXDATE.Value = v_strTXDATE
    '                    v_dataElement.Attributes.Append(v_attrTXDATE)

    '                    ' 1. error
    '                    v_entryNode = v_xmlDocument.CreateNode(Xml.XmlNodeType.Element, "entry", "")
    '                    'Add field name
    '                    v_attrFLDNAME = v_xmlDocument.CreateAttribute(gc_AtributeFLDNAME)
    '                    v_attrFLDNAME.Value = "ERROR_MESSAGE"
    '                    v_entryNode.Attributes.Append(v_attrFLDNAME)
    '                    'Add Err count
    '                    v_attrCount = v_xmlDocument.CreateAttribute("COUNT")
    '                    v_attrCount.Value = v_intErrCount.ToString
    '                    v_entryNode.Attributes.Append(v_attrCount)
    '                    'Set value
    '                    v_entryNode.InnerText = v_strErrMsg
    '                    v_dataElement.AppendChild(v_entryNode)

    '                    ' 2. warning
    '                    v_entryNode = v_xmlDocument.CreateNode(Xml.XmlNodeType.Element, "entry", "")
    '                    'Add field name
    '                    v_attrFLDNAME = v_xmlDocument.CreateAttribute(gc_AtributeFLDNAME)
    '                    v_attrFLDNAME.Value = "WARNING_MESSAGE"
    '                    v_entryNode.Attributes.Append(v_attrFLDNAME)
    '                    'Add Err count
    '                    v_attrCount = v_xmlDocument.CreateAttribute("COUNT")
    '                    v_attrCount.Value = v_intWarningCount.ToString
    '                    v_entryNode.Attributes.Append(v_attrCount)
    '                    'Set value
    '                    v_entryNode.InnerText = v_strWarningMsg
    '                    v_dataElement.AppendChild(v_entryNode)

    '                    v_xmlDocument.DocumentElement.AppendChild(v_dataElement)
    '                    v_lngError = ERR_SYSTEM_START
    '                End If
    '                If v_trace_status = "1" Then
    '                    Trace.WriteLine("[Kết thúc: APPCHK - " & v_strTLTXCD & "] " & DateTime.Now & vbCrLf)
    '                    tr2.Close()
    '                    tr2.Dispose()
    '                End If
    '            End With
    '            v_CheckTTDataAccess.Commit()
    '        Next
    '        ContextUtil.SetComplete()
    '        Return v_lngError
    '    Catch ex As Exception
    '        ContextUtil.SetAbort()
    '        If v_trace_status = "1" Then
    '            tr2.Close()
    '            tr2.Dispose()
    '        End If
    '        v_CheckTTDataAccess.Rollback()
    '        Throw ex
    '    Finally
    '        v_CheckTTDataAccess.Dispose()
    '    End Try
    'End Function
    Public Sub GetTltxUserAuth(ByVal pv_strTltxcd As String, ByVal pv_strTellerId As String, _
                                   ByRef pv_strCHKID As String, ByRef pv_strOFFID As String, _
                                   ByRef pv_strCFRID As String, ByRef pv_obj As DataAccess)
        Try
            Dim v_strSQL As String
            Dim v_ds As DataSet
            Dim v_strCHKID1, v_strOFFID1, v_strCFRID1 As String
            v_strSQL = "SELECT chkid, offid, cfrid FROM tltx WHERE tltxcd = '" & pv_strTltxcd & "' AND status = 0 AND deleted = 0"
            v_ds = pv_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            If v_ds.Tables(0).Rows.Count > 0 Then
                With v_ds.Tables(0).Rows(0)
                    v_strCHKID1 = gf_CorrectStringField(.Item("CHKID"))
                    v_strOFFID1 = gf_CorrectStringField(.Item("OFFID"))
                    v_strCFRID1 = gf_CorrectStringField(.Item("CFRID"))
                End With
            End If
            v_strSQL = "SELECT * FROM (select authid, authtype, chkid, offid, cfrid, tltxcd from tltxuserauth where authid = '" & pv_strTellerId & "'" _
                & " and authtype = 'U' and deleted = 0 and status = 0 " _
                & " union " _
                & " select authid, authtype, chkid, offid, cfrid, tltxcd from tltxuserauth a where deleted = 0 and status = 0 and authtype = 'G'" _
                & " and authid in (select distinct grpid from tlgrpusers where tlid = '" & pv_strTellerId & "' and deleted = 0 and status = 0 )" _
                & " ) WHERE tltxcd = '" & pv_strTltxcd & "'"
            v_ds = pv_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

            If v_ds.Tables(0).Rows.Count > 0 Then
                Dim v_strTmpCHKID As String = "1"
                Dim v_strTmpOFFID As String = "1"
                Dim v_strTmpCFRID As String = "1"
                Dim v_strCHKID_final As String = "1"
                Dim v_strOFFID_final As String = "1"
                Dim v_strCFRID_final As String = "1"

                Dim v_strAuthType As String
                For i = 0 To v_ds.Tables(0).Rows.Count - 1
                    With v_ds.Tables(0)
                        v_strTmpCHKID = Trim(.Rows(i)("CHKID"))
                        v_strTmpOFFID = Trim(.Rows(i)("OFFID"))
                        v_strTmpCFRID = Trim(.Rows(i)("CFRID"))
                        v_strAuthType = Trim(.Rows(i)("AUTHTYPE"))
                    End With

                    If v_strAuthType = "U" Then
                        v_strCHKID1 = v_strTmpCHKID
                        v_strOFFID1 = v_strTmpOFFID
                        v_strCFRID1 = v_strTmpCFRID
                        Exit For
                    Else
                        If CInt(v_strCHKID_final) > CInt(v_strTmpCHKID) Then
                            v_strCHKID_final = v_strTmpCHKID
                        End If
                        If CInt(v_strOFFID_final) > CInt(v_strTmpOFFID) Then
                            v_strOFFID_final = v_strTmpOFFID
                        End If
                        If CInt(v_strCFRID_final) > CInt(v_strTmpCFRID) Then
                            v_strCFRID_final = v_strTmpCFRID
                        End If
                    End If
                    v_strCHKID1 = v_strCHKID_final
                    v_strOFFID1 = v_strOFFID_final
                    v_strCFRID1 = v_strCFRID_final
                Next
            End If
            If (pv_strCHKID = "0" And v_strCHKID1 = "1") Or (pv_strCHKID = "1" And v_strCHKID1 = "0") Then
                pv_strCHKID = v_strCHKID1
            End If
            If (pv_strOFFID = "0" And v_strOFFID1 = "1") Or (pv_strOFFID = "1" And v_strOFFID1 = "0") Then
                pv_strOFFID = v_strOFFID1
            End If
            If (pv_strCFRID = "0" And v_strCFRID1 = "1") Or (pv_strCFRID = "1" And v_strCFRID1 = "0") Then
                pv_strCHKID = v_strCHKID1
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Function CheckHOST(ByRef pv_oNode As Xml.XmlNode, ByRef pv_oXml As XmlDocumentEx, Optional ByVal mv_oSignServer As SignServer = Nothing) As Long
        Dim v_strErrorSource As String = "Branch.Branch.CheckBDS", v_strErrorMessage As String = ""
        Dim v_lngError As Long = ERR_SYSTEM_OK
        Dim v_strTellerId, v_strSQL, v_strTLTXCD, v_strCOTLTXCD As String
        Dim v_strSQLTmp, v_strReplaceSQL, v_strFieldsSQL, v_strValuesSQL As String
        Dim intCountNo As Integer
        Dim v_strTXNUM, v_strTXDATE As String
        Dim v_trace As DataSet
        Dim v_trace_status, v_trace_path As String
        Dim tr2 As New TextWriterTraceListener
        Dim v_obj As New DataAccess
        Dim v_nodeList As Xml.XmlNodeList
        Dim blnTran As Boolean = False
        Dim v_strPARENT_TLTXCD As String = pv_oNode.Attributes(gc_AtributePARENT_TLTXCD).Value.ToString
        Dim v_strPARENT_TXDATE As String = pv_oNode.Attributes(gc_AtributePARENT_TXDATE).Value.ToString
        Dim v_strOLDSTATUS As String = pv_oNode.Attributes(gc_AtributeOLDSTATUS).Value.ToString
        Dim v_strCHILD_TLTXCD As String = pv_oNode.Attributes(gc_AtributeCHILDTLTXCD).Value.ToString
        Dim v_strFileNameCA As String = pv_oNode.Attributes(gc_AtributeFileNameCACHK).Value.ToString
        Dim v_strSignatureCA As String = pv_oNode.Attributes(gc_AtributeSignatureCACHK).Value.ToString

        Dim v_ds, v_Msg, v_replace As DataSet

        Try

            Dim v_bCmd As New BusinessCommand
            Dim v_strErrMsg As String = ""
            Dim v_strWarningMsg As String = ""
            Dim v_intErrCount As Integer = 0
            Dim v_intWarningCount As Integer = 0
            Dim i, j, intIndex As Integer

            v_obj.NewDBInstance(gc_MODULE_HOST)

            v_trace_status = "0"

            Dim v_strBRID1 As String = pv_oNode.Attributes(gc_AtributeBRID).Value.ToString
            Dim v_strCurrDate As String
            v_obj.GetSysVar("SYSTEM", "TRACE_STATUS", v_trace_status)
            v_obj.GetSysVar("SYSTEM", "CURRDATE", v_strBRID1, v_strCurrDate)
            v_obj.GetSysVar("SYSTEM", "TRACE_PATH", v_trace_path)
            ' date : 14/10/2008
            ' Purpose : get from  date T-3 to date T
            Dim v_strT_T_3 As String
            v_strSQL = "select to_char(GET_T_PLUS(to_date('" & v_strCurrDate & "', 'dd/mm/yyyy'),'" & v_strBRID1 & "',-1),'dd/mm/yyyy') txdate from dual" _
                    & " union select to_char(GET_T_PLUS(to_date('" & v_strCurrDate & "', 'dd/mm/yyyy'),'" & v_strBRID1 & "',-2),'dd/mm/yyyy') txdate from dual" _
                    & " union select to_char(GET_T_PLUS(to_date('" & v_strCurrDate & "', 'dd/mm/yyyy'),'" & v_strBRID1 & "',-3),'dd/mm/yyyy') txdate from dual"
            v_trace = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            v_strT_T_3 = "to_date('" & v_strCurrDate & "', 'dd/mm/yyyy'),"
            For i = 0 To v_trace.Tables(0).Rows.Count - 1
                v_strT_T_3 = v_strT_T_3 & "to_date('" & v_trace.Tables(0).Rows(i)("txdate") & "', 'dd/mm/yyyy'),"
            Next
            v_strT_T_3 = "(" & Left(v_strT_T_3, v_strT_T_3.Length - 1) & ")"
            ' end 
            ' nhan dl tren host
            Dim v_dsError As New DataSet
            Dim v_dsWarning As New DataSet


            Dim v_strPARENT_TXNUM As String = pv_oNode.Attributes(gc_AtributePARENT_TXNUM).Value.ToString

            Dim v_strPARENT_BUSDATE As String = pv_oNode.Attributes(gc_AtributePARENT_BUSDATE).Value.ToString
            v_strTLTXCD = pv_oNode.Attributes(gc_AtributeTLTXCD).InnerXml
            Dim v_strAppPath As String
            v_strAppPath = System.AppDomain.CurrentDomain.BaseDirectory
            'bangpv: thêm các GD 2163,2166,2169 cho nhận giá HNX cho CCP 
            If InStr("1127|1128|1122|1116|1111|1110|1109|4003|4004|4005|4006|4007|4020|3079|2133|2136|2139|2142|2145|2148|2163|2166|2169|2172|1113", v_strPARENT_TLTXCD) > 0 And v_strPARENT_TLTXCD <> "" Then
                Select Case v_strPARENT_TLTXCD
                    Case "4003", "4005", "4006", "4007", "4020"
                        If Not HOSTReadFileXML(v_strAppPath & "data\" & v_strBRID1 & "_" & v_strCurrDate.Replace("/", "") & ".xml", pv_oNode, pv_oXml, v_strTLTXCD, v_strPARENT_TXNUM, v_strPARENT_TXDATE, v_strPARENT_BUSDATE, v_strErrMsg, v_intErrCount) Then
                            v_lngError = ERR_SYSTEM_START
                        End If
                    Case "4004"
                        Dim v_strSettUpdType As String = ""
                        v_obj.GetSysVar("SYSTEM", "SETT_UPD_TYPE", v_strBRID1, v_strSettUpdType)
                        If v_strSettUpdType = "1.0" Then
                            If Not HOSTReadFileASTDT_1(v_strAppPath & "data\" & v_strBRID1 & "_ASTDL_" & v_strCurrDate.Replace("/", "") & ".txt", pv_oNode, pv_oXml, v_strTLTXCD, v_strPARENT_TXNUM, v_strPARENT_TXDATE, v_strPARENT_BUSDATE, v_strErrMsg, v_intErrCount) Then
                                v_lngError = ERR_SYSTEM_START
                            End If
                            If Not HOSTReadFileASTPT_1(v_strAppPath & "data\" & v_strBRID1 & "_ASTPT_" & v_strCurrDate.Replace("/", "") & ".txt", pv_oNode, pv_oXml, v_strTLTXCD, v_strPARENT_TXNUM, v_strPARENT_TXDATE, v_strPARENT_BUSDATE, v_strErrMsg, v_intErrCount) Then
                                v_lngError = ERR_SYSTEM_START
                            End If
                        ElseIf v_strSettUpdType = "2.0" Then
                            If Not HOSTReadFileASTDT_2(v_strAppPath & "data\" & v_strBRID1 & "_ASTDL_" & v_strCurrDate.Replace("/", "") & ".txt", pv_oNode, pv_oXml, v_strTLTXCD, v_strPARENT_TXNUM, v_strPARENT_TXDATE, v_strPARENT_BUSDATE, v_strErrMsg, v_intErrCount) Then
                                v_lngError = ERR_SYSTEM_START
                            End If
                            If Not HOSTReadFileASTPT_2(v_strAppPath & "data\" & v_strBRID1 & "_ASTPT_" & v_strCurrDate.Replace("/", "") & ".txt", pv_oNode, pv_oXml, v_strTLTXCD, v_strPARENT_TXNUM, v_strPARENT_TXDATE, v_strPARENT_BUSDATE, v_strErrMsg, v_intErrCount) Then
                                v_lngError = ERR_SYSTEM_START
                            End If
                        ElseIf v_strSettUpdType = "3.0" Then
                            If Not HOSTReadFileASTDT_3(v_strAppPath & "data\" & v_strBRID1 & "_ASTDL_" & v_strCurrDate.Replace("/", "") & ".txt", pv_oNode, pv_oXml, v_strTLTXCD, v_strPARENT_TXNUM, v_strPARENT_TXDATE, v_strPARENT_BUSDATE, v_strErrMsg, v_intErrCount) Then
                                v_lngError = ERR_SYSTEM_START
                            End If
                            If Not HOSTReadFileASTPT_3(v_strAppPath & "data\" & v_strBRID1 & "_ASTPT_" & v_strCurrDate.Replace("/", "") & ".txt", pv_oNode, pv_oXml, v_strTLTXCD, v_strPARENT_TXNUM, v_strPARENT_TXDATE, v_strPARENT_BUSDATE, v_strErrMsg, v_intErrCount) Then
                                v_lngError = ERR_SYSTEM_START
                            End If
                        ElseIf v_strSettUpdType = "4.1" Then
                            If Not HOSTReadFileASTDT_41(v_strAppPath & "data\" & v_strBRID1 & "_ASTDL_" & v_strCurrDate.Replace("/", "") & ".txt", pv_oNode, pv_oXml, v_strTLTXCD, v_strPARENT_TXNUM, v_strPARENT_TXDATE, v_strPARENT_BUSDATE, v_strErrMsg, v_intErrCount) Then
                                v_lngError = ERR_SYSTEM_START
                            End If
                            If Not HOSTReadFileASTPT_41(v_strAppPath & "data\" & v_strBRID1 & "_ASTPT_" & v_strCurrDate.Replace("/", "") & ".txt", pv_oNode, pv_oXml, v_strTLTXCD, v_strPARENT_TXNUM, v_strPARENT_TXDATE, v_strPARENT_BUSDATE, v_strErrMsg, v_intErrCount) Then
                                v_lngError = ERR_SYSTEM_START
                            End If
                        ElseIf v_strSettUpdType = "4.2" Then
                            If Not HOSTReadFileASTDT_42(v_strAppPath & "data\" & v_strBRID1 & "_ASTDL_" & v_strCurrDate.Replace("/", "") & ".txt", pv_oNode, pv_oXml, v_strTLTXCD, v_strPARENT_TXNUM, v_strPARENT_TXDATE, v_strPARENT_BUSDATE, v_strErrMsg, v_intErrCount) Then
                                v_lngError = ERR_SYSTEM_START
                            End If
                            If Not HOSTReadFileASTPT_42(v_strAppPath & "data\" & v_strBRID1 & "_ASTPT_" & v_strCurrDate.Replace("/", "") & ".txt", pv_oNode, pv_oXml, v_strTLTXCD, v_strPARENT_TXNUM, v_strPARENT_TXDATE, v_strPARENT_BUSDATE, v_strErrMsg, v_intErrCount) Then
                                v_lngError = ERR_SYSTEM_START
                            End If
                        ElseIf v_strSettUpdType = "4.3" Then
                            If Not HOSTReadFileASTDT_43(v_strAppPath & "data\" & v_strBRID1 & "_ASTDL_" & v_strCurrDate.Replace("/", "") & ".txt", pv_oNode, pv_oXml, v_strTLTXCD, v_strPARENT_TXNUM, v_strPARENT_TXDATE, v_strPARENT_BUSDATE, v_strErrMsg, v_intErrCount) Then
                                v_lngError = ERR_SYSTEM_START
                            End If
                            If Not HOSTReadFileASTPT_43(v_strAppPath & "data\" & v_strBRID1 & "_ASTPT_" & v_strCurrDate.Replace("/", "") & ".txt", pv_oNode, pv_oXml, v_strTLTXCD, v_strPARENT_TXNUM, v_strPARENT_TXDATE, v_strPARENT_BUSDATE, v_strErrMsg, v_intErrCount) Then
                                v_lngError = ERR_SYSTEM_START
                            End If
                        End If
                        'bangpv: đối chứng báo cáo 
                    Case "1109"
                        If Not HostCheckReports(v_strFileNameCA, v_strSignatureCA, v_strErrMsg, v_intErrCount, pv_oNode.Attributes(gc_AtributeTLNAME).Value.ToString, _
                                                pv_oNode.Attributes(gc_AtributeTLID).Value.ToString, mv_oSignServer) Then
                            v_lngError = ERR_SYSTEM_START
                        End If
                    Case "1110"
                        If Not HostCheckTransCA(v_strFileNameCA, v_strSignatureCA, v_strErrMsg, v_intErrCount, pv_oNode.Attributes(gc_AtributeTLNAME).Value.ToString, _
                                                pv_oNode.Attributes(gc_AtributeTLID).Value.ToString, mv_oSignServer) Then
                            v_lngError = ERR_SYSTEM_START
                        End If
                        'bangpv: Ket xuat bao cao
                    Case "1127"
                        If Not HostExportReports(v_strFileNameCA, v_strSignatureCA, v_strErrMsg, v_intErrCount, pv_oNode.Attributes(gc_AtributeTLNAME).Value.ToString, _
                                                pv_oNode.Attributes(gc_AtributeTLID).Value.ToString, mv_oSignServer) Then
                            v_lngError = ERR_SYSTEM_START
                        End If
                        'Ket xuat giao dich
                    Case "1128"
                        If Not HostExportTransCA(v_strFileNameCA, v_strSignatureCA, v_strErrMsg, v_intErrCount, pv_oNode.Attributes(gc_AtributeTLNAME).Value.ToString, _
                                                pv_oNode.Attributes(gc_AtributeTLID).Value.ToString, mv_oSignServer) Then
                            v_lngError = ERR_SYSTEM_START
                        End If
                        'end bangpv
                    Case "1111"
                        If pv_oNode.Attributes(gc_AtributeSTATUS).Value.ToString = "0" Then
                            If Not HostCheckConfirmRPT(v_strFileNameCA, v_strSignatureCA, v_strErrMsg, v_intErrCount, pv_oNode.Attributes(gc_AtributeTLNAME).Value.ToString, _
                                                    pv_oNode.Attributes(gc_AtributeTLID).Value.ToString, mv_oSignServer) Then
                                v_lngError = ERR_SYSTEM_START
                            End If
                        End If
                    Case "1116"
                        If pv_oNode.Attributes(gc_AtributeSTATUS).Value.ToString = "0" Then
                            If Not HostCheckConfirmRPT(v_strFileNameCA, v_strSignatureCA, v_strErrMsg, v_intErrCount, pv_oNode.Attributes(gc_AtributeTLNAME).Value.ToString, _
                                                    pv_oNode.Attributes(gc_AtributeTLID).Value.ToString, mv_oSignServer) Then
                                v_lngError = ERR_SYSTEM_START
                            End If
                        End If
                    Case "1122"
                        If pv_oNode.Attributes(gc_AtributeSTATUS).Value.ToString = "0" Then
                            If Not HostCheckConfirmRPT(v_strFileNameCA, v_strSignatureCA, v_strErrMsg, v_intErrCount, pv_oNode.Attributes(gc_AtributeTLNAME).Value.ToString, _
                                                    pv_oNode.Attributes(gc_AtributeTLID).Value.ToString, mv_oSignServer) Then
                                v_lngError = ERR_SYSTEM_START
                            End If
                        End If
                    Case "3079"
                        'Dim v_strMicode As String = pv_oNode.Attributes(gc_AtributeMICODE).Value.ToString
                        'Dim v_strCoMicode As String = pv_oNode.Attributes(gc_AtributeCOMICODE).Value.ToString
                        'Dim v_strTradeacctno As String = pv_oNode.Attributes(gc_AtributeTXNOTE).Value.ToString
                        'Added by Thanglv9 - 19/03/2013
                        'Them v_strWarningMsg,v_intWarningCount
                        'If Not Host_insert_3079_3197(pv_oNode, v_strSignatureCA, v_strErrMsg, v_intErrCount, v_strWarningMsg, v_intWarningCount) Then
                        'v_lngError = ERR_SYSTEM_START
                        'End If
                        If Not Host_insert_3079_SBV(pv_oNode, v_strSignatureCA, v_strErrMsg, v_intErrCount, v_strWarningMsg, v_intWarningCount) Then
                            v_lngError = ERR_SYSTEM_START
                        End If
                        'Nhan gia CK,Tp,HNX
                    Case "2133"
                        v_strSQL = "SELECT col_value01 FROM tllog WHERE txnum = '" & v_strPARENT_TXNUM & "'"
                        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                        Dim v_strTradeDate As String = ""
                        If v_ds.Tables(0).Rows.Count > 0 Then
                            v_strTradeDate = gf_CorrectStringField(v_ds.Tables(0).Rows(0)(0)).Replace("/", "")
                        End If
                        If Not HOSTReadFileStockPrice(v_strAppPath & "price\" & "HNX_STOCKS_PRICE_" & _
                                                      DateTime.ParseExact(v_strTradeDate.Replace("/", ""), "ddMMyyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd", CultureInfo.InvariantCulture) & ".xml", _
                                                      pv_oNode, pv_oXml, v_strTLTXCD, v_strPARENT_TXNUM, v_strPARENT_TXDATE, v_strPARENT_BUSDATE, v_strErrMsg, v_intErrCount, "") Then
                            v_lngError = ERR_SYSTEM_START
                        End If
                    Case "2136"
                        v_strSQL = "SELECT col_value01 FROM tllog WHERE txnum = '" & v_strPARENT_TXNUM & "'"
                        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                        Dim v_strTradeDate As String = ""
                        If v_ds.Tables(0).Rows.Count > 0 Then
                            v_strTradeDate = gf_CorrectStringField(v_ds.Tables(0).Rows(0)(0)).Replace("/", "")
                        End If
                        If Not HOSTReadFileStockPrice(v_strAppPath & "price\" & "HNX_IDX_STOCKS_" & _
                                                      DateTime.ParseExact(v_strTradeDate.Replace("/", ""), "ddMMyyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd", CultureInfo.InvariantCulture) & ".xml", _
                                                      pv_oNode, pv_oXml, v_strTLTXCD, v_strPARENT_TXNUM, v_strPARENT_TXDATE, v_strPARENT_BUSDATE, v_strErrMsg, v_intErrCount, "") Then
                            v_lngError = ERR_SYSTEM_START
                        End If
                    Case "2139"
                        v_strSQL = "SELECT col_value01 FROM tllog WHERE txnum = '" & v_strPARENT_TXNUM & "'"
                        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                        Dim v_strTradeDate As String = ""
                        If v_ds.Tables(0).Rows.Count > 0 Then
                            v_strTradeDate = gf_CorrectStringField(v_ds.Tables(0).Rows(0)(0)).Replace("/", "")
                        End If
                        If Not HOSTReadFileStockPrice(v_strAppPath & "price\" & "HNX_BOND_PRICE_YC_" & _
                                                      DateTime.ParseExact(v_strTradeDate.Replace("/", ""), "ddMMyyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd", CultureInfo.InvariantCulture) & ".xml", _
                                                      pv_oNode, pv_oXml, v_strTLTXCD, v_strPARENT_TXNUM, v_strPARENT_TXDATE, v_strPARENT_BUSDATE, v_strErrMsg, v_intErrCount, "") Then
                            v_lngError = ERR_SYSTEM_START
                        End If
                    Case "2142"
                        v_strSQL = "SELECT col_value01, col_value03 FROM tllog WHERE txnum = '" & v_strPARENT_TXNUM & "'"
                        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                        Dim v_strTradeDate As String = ""
                        Dim v_strPriceType As String = ""
                        If v_ds.Tables(0).Rows.Count > 0 Then
                            v_strTradeDate = gf_CorrectStringField(v_ds.Tables(0).Rows(0)(0)).Replace("/", "")
                            v_strPriceType = gf_CorrectStringField(v_ds.Tables(0).Rows(0)(1))
                        End If
                        If v_strPriceType = "1" Then
                            If Not HOSTReadFileStockPrice(v_strAppPath & "price\" & v_strTradeDate.Replace("/", "") & "_CLOSE_PRICE_OF_LISTED_STOCKS.txt", _
                                                      pv_oNode, pv_oXml, v_strTLTXCD, v_strPARENT_TXNUM, v_strPARENT_TXDATE, v_strPARENT_BUSDATE, v_strErrMsg, v_intErrCount, v_strPriceType) Then
                                v_lngError = ERR_SYSTEM_START
                            End If
                        Else
                            If Not HOSTReadFileStockPrice(v_strAppPath & "price\" & v_strTradeDate.Replace("/", "") & "_REFER_PRICE_OF_LISTED_STOCKS.txt", _
                                                      pv_oNode, pv_oXml, v_strTLTXCD, v_strPARENT_TXNUM, v_strPARENT_TXDATE, v_strPARENT_BUSDATE, v_strErrMsg, v_intErrCount, v_strPriceType) Then
                                v_lngError = ERR_SYSTEM_START
                            End If
                        End If
                        'bangpv: add 2148
                    Case "2148"
                        v_strSQL = "SELECT col_value01, col_value03 FROM tllog WHERE txnum = '" & v_strPARENT_TXNUM & "'"
                        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                        Dim v_strTradeDate As String = ""
                        Dim v_strPriceType As String = ""
                        If v_ds.Tables(0).Rows.Count > 0 Then
                            v_strTradeDate = gf_CorrectStringField(v_ds.Tables(0).Rows(0)(0)).Replace("/", "")
                            v_strPriceType = gf_CorrectStringField(v_ds.Tables(0).Rows(0)(1))
                        End If

                        If Not HOSTReadFileStockPrice(v_strAppPath & "price\" & v_strTradeDate.Replace("/", "") & "_FSP_VN30.txt", _
                                                  pv_oNode, pv_oXml, v_strTLTXCD, v_strPARENT_TXNUM, v_strPARENT_TXDATE, v_strPARENT_BUSDATE, v_strErrMsg, v_intErrCount, v_strPriceType) Then
                            v_lngError = ERR_SYSTEM_START
                        End If
                        'bangpv: add 2148
                    Case "2145"
                        v_strSQL = "SELECT col_value01 FROM tllog WHERE txnum = '" & v_strPARENT_TXNUM & "'"
                        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                        Dim v_strTradeDate As String = ""
                        If v_ds.Tables(0).Rows.Count > 0 Then
                            v_strTradeDate = gf_CorrectStringField(v_ds.Tables(0).Rows(0)(0)).Replace("/", "")
                        End If
                        If Not HOSTReadFileStockPrice(v_strAppPath & "price\" & "HNX_CLOSE_PRICE_" & _
                                                      DateTime.ParseExact(v_strTradeDate.Replace("/", ""), "ddMMyyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd", CultureInfo.InvariantCulture) & ".xml", _
                                                      pv_oNode, pv_oXml, v_strTLTXCD, v_strPARENT_TXNUM, v_strPARENT_TXDATE, v_strPARENT_BUSDATE, v_strErrMsg, v_intErrCount, "") Then
                            v_lngError = ERR_SYSTEM_START
                        End If
                        'bangpv: các GD phục vụ CCP 
                    Case "2163"
                        v_strSQL = "SELECT col_value01 FROM tllog WHERE txnum = '" & v_strPARENT_TXNUM & "'"
                        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                        Dim v_strTradeDate As String = ""
                        If v_ds.Tables(0).Rows.Count > 0 Then
                            v_strTradeDate = gf_CorrectStringField(v_ds.Tables(0).Rows(0)(0)).Replace("/", "")
                        End If
                        If Not HOSTReadFileStockPrice(v_strAppPath & "price\" & "HNX_BOND_BASKET_" & _
                                                      DateTime.ParseExact(v_strTradeDate.Replace("/", ""), "ddMMyyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd", CultureInfo.InvariantCulture) & ".xml", _
                                                      pv_oNode, pv_oXml, v_strTLTXCD, v_strPARENT_TXNUM, v_strPARENT_TXDATE, v_strPARENT_BUSDATE, v_strErrMsg, v_intErrCount, "") Then
                            v_lngError = ERR_SYSTEM_START
                        End If
                    Case "2166"
                        v_strSQL = "SELECT col_value01 FROM tllog WHERE txnum = '" & v_strPARENT_TXNUM & "'"
                        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                        Dim v_strTradeDate As String = ""
                        If v_ds.Tables(0).Rows.Count > 0 Then
                            v_strTradeDate = gf_CorrectStringField(v_ds.Tables(0).Rows(0)(0)).Replace("/", "")
                        End If
                        If Not HOSTReadFileStockPrice(v_strAppPath & "price\" & "HNX_BOND_INDEX_" & _
                                                      DateTime.ParseExact(v_strTradeDate.Replace("/", ""), "ddMMyyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd", CultureInfo.InvariantCulture) & ".xml", _
                                                      pv_oNode, pv_oXml, v_strTLTXCD, v_strPARENT_TXNUM, v_strPARENT_TXDATE, v_strPARENT_BUSDATE, v_strErrMsg, v_intErrCount, "") Then
                            v_lngError = ERR_SYSTEM_START
                        End If
                    Case "2169"
                        v_strSQL = "SELECT col_value01 FROM tllog WHERE txnum = '" & v_strPARENT_TXNUM & "'"
                        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                        Dim v_strTradeDate As String = ""
                        If v_ds.Tables(0).Rows.Count > 0 Then
                            v_strTradeDate = gf_CorrectStringField(v_ds.Tables(0).Rows(0)(0)).Replace("/", "")
                        End If
                        If Not HOSTReadFileStockPrice(v_strAppPath & "price\" & "HNX_INDEX_" & _
                                                      DateTime.ParseExact(v_strTradeDate.Replace("/", ""), "ddMMyyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd", CultureInfo.InvariantCulture) & ".xml", _
                                                      pv_oNode, pv_oXml, v_strTLTXCD, v_strPARENT_TXNUM, v_strPARENT_TXDATE, v_strPARENT_BUSDATE, v_strErrMsg, v_intErrCount, "") Then
                            v_lngError = ERR_SYSTEM_START
                        End If
                    Case "2172"
                        v_strSQL = "SELECT col_value01 FROM tllog WHERE txnum = '" & v_strPARENT_TXNUM & "'"
                        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                        Dim v_strTradeDate As String = ""
                        If v_ds.Tables(0).Rows.Count > 0 Then
                            v_strTradeDate = gf_CorrectStringField(v_ds.Tables(0).Rows(0)(0)).Replace("/", "")
                        End If
                        If Not HOSTReadFileStockPrice(v_strAppPath & "price\" & "HNX_BOND_YC_DATA_" & _
                                                      DateTime.ParseExact(v_strTradeDate.Replace("/", ""), "ddMMyyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd", CultureInfo.InvariantCulture) & ".xml", _
                                                      pv_oNode, pv_oXml, v_strTLTXCD, v_strPARENT_TXNUM, v_strPARENT_TXDATE, v_strPARENT_BUSDATE, v_strErrMsg, v_intErrCount, "") Then
                            v_lngError = ERR_SYSTEM_START
                        End If
                        'end bangpv 
                        'Add by Thanglv 28/05/2014
                    Case "1113"
                        'Dim v_strMicode As String = pv_oNode.Attributes(gc_AtributeMICODE).Value.ToString
                        'Dim v_strCoMicode As String = pv_oNode.Attributes(gc_AtributeCOMICODE).Value.ToString
                        'Dim v_strTradeacctno As String = pv_oNode.Attributes(gc_AtributeTXNOTE).Value.ToString
                        'Added by Thanglv9 - 19/03/2013
                        'Them v_strWarningMsg,v_intWarningCount
                        If v_strBRID1 <> "0002" Then
                            If Not Host_check_file_1113(pv_oNode, v_strSignatureCA, v_strErrMsg, v_intErrCount) Then
                                v_lngError = ERR_SYSTEM_START
                            End If
                        End If
                End Select
            End If

            'Add by ThangLV - 20180816 - check nhan gia 2130
            If v_strTLTXCD = "2130" Then
                If Not HostCheckftpfromHNX_2130(pv_oNode, v_strSignatureCA, v_strErrMsg, v_intErrCount) Then
                    v_lngError = ERR_SYSTEM_START
                End If
            End If

            'end tuanta
            If v_intErrCount = 0 Then
                v_obj.BeginTran()
                blnTran = True
                With pv_oNode
                    v_intErrCount = 0
                    v_intWarningCount = 0
                    v_strErrMsg = ""
                    v_strWarningMsg = ""

                    Dim v_strSTATUS As String = .Attributes(gc_AtributeSTATUS).Value.ToString
                    Dim v_strSTATUS_TEXT As String = .Attributes(gc_AtributeSTATUSTEXT).Value.ToString
                    Dim v_strAUTOID As String = .Attributes(gc_AtributeAUTOID).Value.ToString
                    v_strTXNUM = .Attributes(gc_AtributeTXNUM).Value.ToString
                    v_strTXDATE = .Attributes(gc_AtributeTXDATE).Value.ToString
                    Dim v_strBUSDATE As String = .Attributes(gc_AtributeBUSDATE).Value.ToString
                    Dim v_strTXDESC As String = .Attributes(gc_AtributeTXDESC).Value.ToString
                    Dim v_strBRID As String = .Attributes(gc_AtributeBRID).Value.ToString
                    Dim v_strBRCODE As String = .Attributes(gc_AtributeBRCODE).Value.ToString
                    Dim v_strTLID As String = .Attributes(gc_AtributeTLID).Value.ToString
                    Dim v_strCHKID As String = .Attributes(gc_AtributeCHKID).Value.ToString
                    Dim v_strOFFID As String = .Attributes(gc_AtributeOFFID).Value.ToString
                    Dim v_strCFRID As String = .Attributes(gc_AtributeCFRID).Value.ToString
                    Dim v_strTLNAME As String = .Attributes(gc_AtributeTLNAME).Value.ToString
                    Dim v_strCHKNAME As String = .Attributes(gc_AtributeCHKNAME).Value.ToString
                    Dim v_strOFFNAME As String = .Attributes(gc_AtributeOFFNAME).Value.ToString
                    Dim v_strCFRNAME As String = .Attributes(gc_AtributeCFRNAME).Value.ToString
                    Dim v_strIPADDRESS As String = .Attributes(gc_AtributeIPADDRESS).Value.ToString
                    Dim v_strWSNAME As String = .Attributes(gc_AtributeWSNAME).Value.ToString
                    Dim v_strSICODE As String = .Attributes(gc_AtributeSICODE).Value.ToString
                    Dim v_strMICODE As String = .Attributes(gc_AtributeMICODE).Value.ToString
                    Dim v_strCOMICODE As String = .Attributes(gc_AtributeCOMICODE).Value.ToString
                    Dim v_strMSGAMT As String = .Attributes(gc_AtributeMSGAMT).Value.ToString
                    Dim v_strISPARENT As String = .Attributes(gc_AtributeISPARENT).Value.ToString
                    Dim v_strPARENTID As String = .Attributes(gc_AtributePARENTID).Value.ToString
                    Dim v_strPARENT_TEXT As String = .Attributes(gc_AtributePARENT_TEXT).Value.ToString
                    Dim v_strTXNAME As String = .Attributes(gc_AtributeTXNAME).Value.ToString
                    Dim v_strCHILDTLTXCD As String = .Attributes(gc_AtributeCHILDTLTXCD).Value.ToString
                    Dim v_strISBRID As String = .Attributes(gc_AtributeISBRID).Value.ToString
                    Dim v_strMISSING_WARNING As String = .Attributes(gc_AtributeMISSING_WARNING).Value.ToString
                    '27/02/2014: Lay lai phan cap giao dich
                    GetTltxUserAuth(v_strPARENT_TLTXCD, v_strTLID, v_strCHKID, v_strOFFID, v_strCFRID, v_obj)
                    'Hanm5 Ket thuc ngay 27/02/2014
                    mv_strIpAddress = v_strIPADDRESS
                    mv_strWsName = v_strWSNAME
                    mv_strTellerName = v_strCFRNAME
                    If mv_strTellerName = "" Then
                        mv_strTellerName = v_strOFFNAME
                        mv_strTellerId = v_strOFFID
                    End If
                    If mv_strTellerName = "" Then
                        mv_strTellerName = v_strCHKNAME
                        mv_strTellerId = v_strCHKID
                    End If
                    If mv_strTellerName = "" Then
                        mv_strTellerName = v_strTLNAME
                        mv_strTellerId = v_strTLID
                    End If

                    'Wtite Log
                    'mv_lwLogWriter.StartWriteLog(gc_MODULE_HOST, "Host", "txRouter", "CheckHOST", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "")

                    If v_trace_status = "1" Then
                        Dim v_strTLName1 As String = v_strCFRNAME
                        Dim v_trace_path1 As String = ""
                        If v_strTLName1 = "" Then
                            v_strTLName1 = v_strOFFNAME
                        End If
                        If v_strTLName1 = "" Then
                            v_strTLName1 = v_strCHKNAME
                        End If
                        If v_strTLName1 = "" Then
                            v_strTLName1 = v_strTLNAME
                        End If
                        v_strCurrDate = Replace(v_strCurrDate, "/", "_")

                        If v_trace_path = "" Then
                            Dim v_app As New ApplicationServices.ApplicationBase
                            v_trace_path1 = v_app.Info.DirectoryPath & "\Log\" & v_strCurrDate
                        Else
                            v_trace_path1 = v_trace_path & v_strCurrDate
                        End If

                        If Not System.IO.Directory.Exists(v_trace_path1) Then
                            System.IO.Directory.CreateDirectory(v_trace_path1)
                        End If

                        v_trace_path1 &= "\log_chk_br" & v_strBRID & "_" & v_strTLName1 & ".txt"

                        tr2 = New TextWriterTraceListener(System.IO.File.AppendText(v_trace_path1))
                        Trace.Listeners.Add(tr2)
                        Trace.WriteLine("[Bắt đầu: APPCHK - " & v_strTLTXCD & "] " & DateTime.Now & vbCrLf)
                    End If

                    'v_strSQL = "select ORDNUM, trace, type, CHECKSQL || NVL(CHECKSQL1,'') as CHECKSQL from appchk where status = 0 and deleted = 0 and ((tltxcd = '" & v_strTLTXCD & "') or (tltxcd is null)) and instr(txstatus,'" & v_strSTATUS & "')>0 order by ordnum"
                    'v_strSQL = "select ORDNUM, trace, type, CHECKSQL , CHECKSQL1 from appchk where status = 0 and deleted = 0 and ((tltxcd = '" & v_strTLTXCD & "') or (tltxcd is null)) and instr(txstatus,'" & v_strSTATUS & "')>0 order by ordnum"
                    v_strSQL = "select * from appchk where status = 0 and deleted = 0 " _
                    & " and ((tltxcd = '" & v_strTLTXCD & "') or (tltxcd is null)) " _
                    & " and instr(txstatus,'" & v_strSTATUS & "')>0 " _
                    & IIf(v_strMISSING_WARNING = "0", "", " and eorw=1 ") _
                    & " order by ordnum"
                    v_bCmd.SQLCommand = v_strSQL
                    v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_bCmd.SQLCommand)
                    'Wtite Log
                    'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "CheckHOST", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Excute SQL command:" & v_strSQL)

                    If v_ds.Tables(0).Rows.Count > 0 Then
                        ' insert data
                        v_nodeList = pv_oNode.SelectNodes("/TransactMessage/fields")
                        If v_nodeList.Count > 0 Then
                            For i = 0 To v_nodeList.Count - 1
                                For j = 0 To v_nodeList.Item(i).ChildNodes.Count - 1
                                    With v_nodeList.Item(i).ChildNodes(j)
                                        v_strSQLTmp = .InnerText

                                        v_strSQLTmp = Replace(v_strSQLTmp, "TT_", "")
                                        v_strSQLTmp = Replace(v_strSQLTmp, "tt_", "")
                                        v_strSQLTmp = Replace(v_strSQLTmp, "tT_", "")
                                        v_strSQLTmp = Replace(v_strSQLTmp, "Tt_", "")

                                        v_obj.ExecuteNonQuery(CommandType.Text, v_strSQLTmp)
                                        'Wtite Log
                                        'bangpv: test err vsd
                                        ' v_obj.ExecuteNonQuery(CommandType.Text, "insert into tmp_txf select * from tmp_txfields")
                                        'end bangpv 
                                        'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "CheckHOST", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Excute SQL command:" & v_strSQLTmp)
                                    End With
                                Next
                            Next
                        Else
                            v_strReplaceSQL = "select max(fldname) from fldmaster where objname = '" & v_strTLTXCD & "' and deleted=0 and status=0"
                            v_bCmd.SQLCommand = v_strReplaceSQL
                            v_replace = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_bCmd.SQLCommand)
                            'Wtite Log
                            'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "CheckHOST", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Excute SQL command:" & v_strReplaceSQL)
                            If v_replace.Tables(0).Rows.Count > 0 Then
                                intCountNo = v_replace.Tables(0).Rows(0)(0)
                            End If
                            v_strFieldsSQL = ""
                            v_strValuesSQL = ""
                            For j = 1 To intCountNo
                                v_strFieldsSQL = v_strFieldsSQL & ", COL_VALUE" & String.Format("{0:00}", j) _
                                & ", COL_TYPE" & String.Format("{0:00}", j) _
                                & ", COL_DESC" & String.Format("{0:00}", j)
                            Next
                            v_strSQLTmp = "INSERT INTO TMP_TXFIELDS(AUTOID, TLTXCD " & v_strFieldsSQL & ",real_row) " _
                            & " SELECT SEQ_TMP_TXFIELDS.NEXTVAL, TLTXCD " & v_strFieldsSQL & ", rownum FROM " _
                            & " ( SELECT " _
                            & " '" & v_strTLTXCD & "' TLTXCD" & v_strFieldsSQL _
                            & " FROM  TLLOG " _
                            & " WHERE deleted =0 and " & IIf(v_strISPARENT = "2", "PARENTID = ", "AUTOID = ") & v_strAUTOID _
                            & " )"

                            v_strSQLTmp = Replace(v_strSQLTmp, "TT_", "")
                            v_strSQLTmp = Replace(v_strSQLTmp, "tt_", "")
                            v_strSQLTmp = Replace(v_strSQLTmp, "tT_", "")
                            v_strSQLTmp = Replace(v_strSQLTmp, "Tt_", "")
                            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQLTmp)
                            'bangpv: test err vsd
                            'v_obj.ExecuteNonQuery(CommandType.Text, "insert into tmp_txf select * from tmp_txfields")
                            'end bangpv 
                            'Wtite Log
                            'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "CheckHOST", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Excute SQL command:" & v_strSQLTmp)
                        End If


                        ' Kiem tra thong tin tren timesten
                        For i = 0 To v_ds.Tables(0).Rows.Count - 1
                            v_strSQL = v_ds.Tables(0).Rows(i)("CHECKSQL") & " " & gf_CorrectStringField(v_ds.Tables(0).Rows(i)("CHECKSQL1"))
                            ' thay the cac transaction message 
                            ' nhom trang thai giao dich
                            v_strSQL = Replace(v_strSQL, "?STATUS_TEXT", "'" & v_strSTATUS_TEXT & "'")
                            v_strSQL = Replace(v_strSQL, "?STATUS", v_strSTATUS)
                            v_strSQL = Replace(v_strSQL, "?OLDSTATUS", v_strOLDSTATUS)
                            ' nhom ma giao dich
                            v_strSQL = Replace(v_strSQL, "?AUTOID", v_strAUTOID)
                            v_strSQL = Replace(v_strSQL, "?TXNUM", "'" & v_strTXNUM & "'")
                            v_strSQL = Replace(v_strSQL, "?TXNAME", "'" & v_strTXNAME & "'")
                            v_strSQL = Replace(v_strSQL, "?TXDATE", "TO_DATE('" & v_strTXDATE & "','dd/mm/yyyy')")
                            'v_strSQL = Replace(v_strSQL, "?BUSDATE", IIf(IsDate(v_strBUSDATE), "TO_DATE('" & v_strBUSDATE & "','dd/mm/yyyy')", "NULL"))
                            v_strSQL = Replace(v_strSQL, "?BUSDATE", "TO_DATE('" & v_strBUSDATE & "','dd/mm/yyyy')")
                            ' nhom loai giao dich
                            v_strSQL = Replace(v_strSQL, "?TLTXCD", "'" & v_strTLTXCD & "'")
                            v_strSQL = Replace(v_strSQL, "?TXDESC", "'" & v_strTXDESC & "'")
                            ' nhom chi nhanh
                            v_strSQL = Replace(v_strSQL, "?BRID", "'" & v_strBRID & "'")
                            v_strSQL = Replace(v_strSQL, "?BRCODE", "'" & v_strBRCODE & "'")
                            ' nhom user
                            v_strSQL = Replace(v_strSQL, "?TLID", "'" & v_strTLID & "'")
                            v_strSQL = Replace(v_strSQL, "?OFFID", "'" & v_strOFFID & "'")
                            v_strSQL = Replace(v_strSQL, "?CFRID", "'" & v_strCFRID & "'")
                            v_strSQL = Replace(v_strSQL, "?CHKID", "'" & v_strCHKID & "'")
                            v_strSQL = Replace(v_strSQL, "?TLNAME", "'" & v_strTLNAME & "'")
                            v_strSQL = Replace(v_strSQL, "?OFFNAME", "'" & v_strOFFNAME & "'")
                            v_strSQL = Replace(v_strSQL, "?CHKNAME", "'" & v_strCHKNAME & "'")
                            v_strSQL = Replace(v_strSQL, "?CFRNAME", "'" & v_strCFRNAME & "'")
                            ' nhom dia chi tao giao dich
                            v_strSQL = Replace(v_strSQL, "?IPADDRESS", "'" & v_strIPADDRESS & "'")
                            v_strSQL = Replace(v_strSQL, "?WSNAME", "'" & v_strWSNAME & "'")
                            ' nhom quyen
                            v_strSQL = Replace(v_strSQL, "?SICODE", "'" & IIf(v_strSICODE = "", "000", v_strSICODE) & "'")
                            v_strSQL = Replace(v_strSQL, "?MICODE", "'" & IIf(v_strMICODE = "", "000", v_strMICODE) & "'")
                            v_strSQL = Replace(v_strSQL, "?COMICODE", "'" & IIf(v_strCOMICODE = "", "000", v_strCOMICODE) & "'")
                            v_strSQL = Replace(v_strSQL, "?MSGAMT", IIf(v_strMSGAMT <> "", "TO_NUMBER(COL_VALUE" & v_strMSGAMT & ")", "0"))
                            ' nhom giao dich cha-con
                            v_strSQL = Replace(v_strSQL, "?ISPARENT", v_strISPARENT)
                            v_strSQL = Replace(v_strSQL, "?PARENTID", v_strPARENTID)
                            v_strSQL = Replace(v_strSQL, "?PARENT_TEXT", "'" & v_strPARENT_TEXT & "'")
                            v_strSQL = Replace(v_strSQL, "?CHILDTLTXCD", "'" & v_strCHILDTLTXCD & "'")
                            v_strSQL = Replace(v_strSQL, "?ISBRID", v_strISBRID)
                            v_strSQL = Replace(v_strSQL, "?T_T_3", v_strT_T_3)

                            If v_trace_status = "1" And v_ds.Tables(0).Rows(i)("TRACE") = 1 Then
                                Trace.WriteLine("[APPCHK - " & v_strTLTXCD & "] " & DateTime.Now & " :" & vbCrLf & " -o- Câu lệnh thứ #" & v_ds.Tables(0).Rows(i)("ORDNUM") & "-o-" & vbCrLf & v_strSQL & vbCrLf)
                                If InStr(v_strSQL.ToUpper, "SELECT") > 0 Then
                                    Trace.WriteLine("-o- Dữ liệu câu lệnh thứ #" & v_ds.Tables(0).Rows(i)("ORDNUM") & "-o-")
                                    v_strSQLTmp = v_strSQL.Substring(InStr(v_strSQL.ToUpper, "SELECT") - 1)

                                    v_strSQLTmp = Replace(v_strSQLTmp, "TT_", "")
                                    v_strSQLTmp = Replace(v_strSQLTmp, "tt_", "")
                                    v_strSQLTmp = Replace(v_strSQLTmp, "tT_", "")
                                    v_strSQLTmp = Replace(v_strSQLTmp, "Tt_", "")
                                    v_trace = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                                    'Wtite Log
                                    'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "CheckHOST", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Excute SQL command:" & v_strSQLTmp)

                                    Trace.WriteLine(v_trace.GetXml & vbCrLf)
                                End If
                            End If

                            If gf_CorrectStringField(v_ds.Tables(0).Rows(i)("TYPE")) = "C" Then
                                If InStr(v_strSQL, "?CHECK_DUPLICATION") > 0 Then
                                    If v_strISPARENT = "0" Then
                                        v_strSQLTmp = "select fldname from fldmaster where status = 0 and deleted = 0 " _
                                        & " and objname = '" & v_strTLTXCD & "' and ISDUPLICATED='Y' order by fldname"
                                        v_trace = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                                        'Wtite Log
                                        'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "CheckHOST", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Excute SQL command:" & v_strSQLTmp)

                                        Dim v_strFileterDup As String = ""
                                        For intDup As Integer = 0 To v_trace.Tables(0).Rows.Count - 1
                                            v_strFileterDup = v_strFileterDup & " and nvl(a.col_value" & v_trace.Tables(0).Rows(intDup)(0).ToString & ",'#') " _
                                            & " = nvl(b.col_value" & v_trace.Tables(0).Rows(intDup)(0).ToString & ",'#') " & vbCrLf
                                        Next
                                        If v_strFileterDup <> "" Then
                                            v_strSQL = "select * from (select 'Giao dịch này trùng với giao dịch (' || a.txnum  || ', ' " _
                                            & " || to_char(a.txdate,'dd/mm/yyyy') || ')' ERRMSG ,  b.real_row+0, '1' TYPE  " _
                                            & " from tllog a, tmp_txfields b" _
                                            & " where a.parentid =" & v_strAUTOID _
                                            & v_strFileterDup _
                                            & ") where  not ERRMSG is null"
                                        Else
                                            v_strSQL = "select N'' from dual where 1=0"
                                        End If
                                    Else
                                        v_strSQL = "select N'' from dual where 1=0"
                                    End If
                                End If
                                'BangPV 20141120: Check cac giao dich khong duoc phep khi da co giao dich chuyen san 1101 da duyet
                                'If gf_CorrectStringField(v_ds.Tables(0).Rows(i)("TYPE")) = "C" Then
                                If InStr(v_strSQL, "?CHECK_SICODE_PENDING") > 0 Then
                                    If v_strISPARENT = "2" Then
                                        v_strSQLTmp = "select sicode from tltx where " _
                                        & "  tltxcd = '" & v_strTLTXCD & "' and deleted =0 and sicode is not null"
                                        v_trace = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                                        'Wtite Log
                                        'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "CheckHOST", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Excute SQL command:" & v_strSQLTmp)
                                        If v_trace.Tables(0).Rows.Count > 0 Then

                                            v_strSQL = "SELECT * FROM (" _
                                                        & "SELECT CASE WHEN b.sicode is null " _
                                                        & "        THEN '' " _
                                                        & "        ELSE 'Mã chứng khoán '|| nvl(b.sicode,'_')|| 'đang chờ hiệu lực chuyển sàn, không thể thực hiện giao dịch'  " _
                                                        & "        END errmsg, a.real_row , '1' Type              " _
                                                        & "  FROM tmp_txfields A " _
                                                        & "LEFT JOIN  " _
                                                        & "(SELECT * FROM tllog WHERE tltxcd ='1101' and status =3 AND brid =" & "'" & v_strBRID & "'" & "  AND deleted =0)  b" _
                                                        & " ON (a.col_value" & gf_CorrectStringField(v_trace.Tables(0).Rows(0)("SICODE")) & " = b.sicode ) " _
                                                        & " ) where length(errmsg)>0"

                                        Else
                                            v_strSQL = "select N'' from dual where 1=0"
                                        End If

                                    Else
                                        v_strSQL = "select N'' from dual where 1=0"
                                    End If
                                End If
                                'end BangPV 20141120 
                                v_strSQL = Replace(v_strSQL, "TT_", "")
                                v_strSQL = Replace(v_strSQL, "tt_", "")
                                v_strSQL = Replace(v_strSQL, "tT_", "")
                                v_strSQL = Replace(v_strSQL, "Tt_", "")
                                v_Msg = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                                'Wtite Log
                                'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "CheckHOST", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Excute SQL command:" & v_strSQL)

                                If InStr("4001|4081|4082|4083|4003|4004|4005|4006|4007|4020", v_strPARENT_TLTXCD) > 0 And v_strPARENT_TLTXCD <> "" Then
                                    If gf_CorrectNumericField(v_ds.Tables(0).Rows(i)("EORW")) = 1 Then
                                        If v_Msg.Tables(0).Rows.Count > 0 Then
                                            v_dsError.Merge(v_Msg)
                                            v_intErrCount = v_intErrCount + v_Msg.Tables(0).Rows.Count
                                        End If
                                    Else
                                        If v_Msg.Tables(0).Rows.Count > 0 Then
                                            v_dsWarning.Merge(v_Msg)
                                            v_intWarningCount = v_intWarningCount + v_Msg.Tables(0).Rows.Count
                                        End If
                                    End If
                                Else
                                    For j = 0 To v_Msg.Tables(0).Rows.Count - 1
                                        If gf_CorrectStringField(v_Msg.Tables(0).Rows(j)(0)).Trim <> "" Then
                                            If v_Msg.Tables(0).Columns.Count >= 3 Then
                                                If gf_CorrectNumericField(v_Msg.Tables(0).Rows(j)(2)) = "2" Then
                                                    If v_strMISSING_WARNING = "0" Then
                                                        v_intWarningCount = v_intWarningCount + 1
                                                        v_strWarningMsg = v_strWarningMsg & "Dòng " & v_Msg.Tables(0).Rows(j)(1) & ": "
                                                        v_strWarningMsg = v_strWarningMsg & v_Msg.Tables(0).Rows(j)(0) & vbCrLf
                                                    End If
                                                Else
                                                    v_intErrCount = v_intErrCount + 1
                                                    v_strErrMsg = v_strErrMsg & "Dòng " & v_Msg.Tables(0).Rows(j)(1) & ": "
                                                    v_strErrMsg = v_strErrMsg & v_Msg.Tables(0).Rows(j)(0) & vbCrLf
                                                End If
                                            Else
                                                v_intErrCount = v_intErrCount + 1
                                                If v_Msg.Tables(0).Columns.Count >= 2 Then
                                                    v_strErrMsg = v_strErrMsg & "Dòng " & v_Msg.Tables(0).Rows(j)(1) & ": "
                                                End If
                                                v_strErrMsg = v_strErrMsg & v_Msg.Tables(0).Rows(j)(0) & vbCrLf
                                            End If
                                        End If
                                    Next
                                End If
                                v_Msg.Dispose()
                            Else
                                v_strSQL = Replace(v_strSQL, "TT_", "")
                                v_strSQL = Replace(v_strSQL, "tt_", "")
                                v_strSQL = Replace(v_strSQL, "tT_", "")
                                v_strSQL = Replace(v_strSQL, "Tt_", "")

                                v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                                'Wtite Log
                                'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "CheckHOST", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Excute SQL command:" & v_strSQL)

                            End If
                        Next
                    End If
                    If v_trace_status = "1" Then
                        Trace.WriteLine("[Kết thúc: APPCHK - " & v_strTLTXCD & "] " & DateTime.Now & vbCrLf)
                        tr2.Close()
                        tr2.Dispose()
                    End If
                End With
                v_obj.Commit()
                blnTran = False
                'Wtite Log
                'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "CheckHOST", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Excute SQL command: Commit")
            Else
                v_Msg = v_obj.ExecuteSQLReturnDataset(CommandType.Text, "select '" & v_strErrMsg.Replace("'", "''") & "' errmsg From dual")
                v_dsError.Merge(v_Msg)
            End If

            If v_intErrCount + v_intWarningCount > 0 Then
                Dim v_attrFLDNAME, v_attrCount, v_attrTXNUM, v_attrTXDATE, v_attrTLTXCD As Xml.XmlAttribute
                Dim v_dataElement As Xml.XmlElement
                Dim v_entryNode As Xml.XmlNode

                v_dataElement = pv_oXml.CreateElement(Xml.XmlNodeType.Element, "FAILED_MESSAGE", "")
                v_attrTLTXCD = pv_oXml.CreateAttribute(gc_AtributeTLTXCD)
                v_attrTLTXCD.Value = v_strTLTXCD
                v_dataElement.Attributes.Append(v_attrTLTXCD)

                v_attrTXNUM = pv_oXml.CreateAttribute(gc_AtributeTXNUM)
                v_attrTXNUM.Value = v_strTXNUM
                v_dataElement.Attributes.Append(v_attrTXNUM)

                v_attrTXDATE = pv_oXml.CreateAttribute(gc_AtributeTXDATE)
                v_attrTXDATE.Value = v_strTXDATE
                v_dataElement.Attributes.Append(v_attrTXDATE)

                ' 1. error
                v_entryNode = pv_oXml.CreateNode(Xml.XmlNodeType.Element, "entry", "")
                'Add field name
                v_attrFLDNAME = pv_oXml.CreateAttribute(gc_AtributeFLDNAME)
                v_attrFLDNAME.Value = "ERROR_MESSAGE"
                v_entryNode.Attributes.Append(v_attrFLDNAME)
                'Add Err count
                v_attrCount = pv_oXml.CreateAttribute("COUNT")
                v_attrCount.Value = v_intErrCount.ToString
                v_entryNode.Attributes.Append(v_attrCount)
                'Set value
                If InStr("4001|4081|4082|4083|4003|4004|4005|4006|4007|4020", v_strPARENT_TLTXCD) > 0 And v_strPARENT_TLTXCD <> "" And v_intErrCount > 0 Then
                    Dim v_strLogFile As String = v_strAppPath & "log\error\" & v_strPARENT_TLTXCD & "_" & v_strPARENT_TXDATE.Replace("/", "") & "_" & (New Random).Next & ".xls"
                    v_entryNode.InnerText = "Vui lòng xem lỗi trên máy chủ HOST : " & v_strLogFile
                    v_dataElement.AppendChild(v_entryNode)
                    If Not System.IO.Directory.Exists(v_strAppPath & "log\error") Then
                        System.IO.Directory.CreateDirectory(v_strAppPath & "log\error")
                    End If

                    v_dsError.WriteXml(v_strLogFile)
                Else
                    v_entryNode.InnerText = v_strErrMsg
                    v_dataElement.AppendChild(v_entryNode)
                End If


                ' 2. warning
                v_entryNode = pv_oXml.CreateNode(Xml.XmlNodeType.Element, "entry", "")
                'Add field name
                v_attrFLDNAME = pv_oXml.CreateAttribute(gc_AtributeFLDNAME)
                v_attrFLDNAME.Value = "WARNING_MESSAGE"
                v_entryNode.Attributes.Append(v_attrFLDNAME)
                'Add Err count
                v_attrCount = pv_oXml.CreateAttribute("COUNT")
                v_attrCount.Value = v_intWarningCount.ToString
                v_entryNode.Attributes.Append(v_attrCount)
                'Set value
                If InStr("4001|4081|4082|4083|4003|4004|4005|4006|4007|4020", v_strPARENT_TLTXCD) > 0 And v_strPARENT_TLTXCD <> "" And v_intWarningCount > 0 Then
                    Dim v_strLogFile As String = v_strAppPath & "log\warning\" & v_strPARENT_TLTXCD & "_" & v_strPARENT_TXDATE.Replace("/", "") & "_" & (New Random).Next & ".xls"
                    v_entryNode.InnerText = "Vui lòng xem cảnh bảo trên máy chủ HOST : " & v_strLogFile
                    v_dataElement.AppendChild(v_entryNode)
                    If Not System.IO.Directory.Exists(v_strAppPath & "log\warning") Then
                        System.IO.Directory.CreateDirectory(v_strAppPath & "log\warning")
                    End If
                    v_dsWarning.WriteXml(v_strLogFile)
                Else
                    v_entryNode.InnerText = v_strWarningMsg
                    v_dataElement.AppendChild(v_entryNode)
                End If

                pv_oXml.DocumentElement.AppendChild(v_dataElement)
                v_lngError = ERR_SYSTEM_START

                If InStr("4001|4081|4082|4083|4003|4004|4005|4006|4007|4020", v_strPARENT_TLTXCD) > 0 And v_strPARENT_TLTXCD <> "" And v_strOLDSTATUS = "0" Then
                    v_nodeList = pv_oNode.SelectNodes("/TransactMessage/fields")
                    If v_nodeList.Count > 0 Then
                        For i = 0 To v_nodeList.Count - 1
                            pv_oNode.RemoveChild(v_nodeList.Item(i))
                        Next
                    End If
                End If
            End If

            'ContextUtil.SetComplete()

            Return v_lngError
        Catch ex As Exception
            'Wtite Log
            'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "CheckHOST", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Error: " & ex.ToString)
            'ContextUtil.SetAbort()
            If v_trace_status = "1" Then
                tr2.Close()
                tr2.Dispose()
            End If
            If blnTran Then v_obj.Commit()
            If InStr("4001|4081|4082|4083|4003|4004|4005|4006|4007|4020", v_strPARENT_TLTXCD) > 0 And v_strPARENT_TLTXCD <> "" And v_strOLDSTATUS = "0" Then
                v_nodeList = pv_oNode.SelectNodes("/TransactMessage/fields")
                If v_nodeList.Count > 0 Then
                    For i = 0 To v_nodeList.Count - 1
                        pv_oNode.RemoveChild(v_nodeList.Item(i))
                    Next
                End If
            End If
            v_lngError = ERR_SYSTEM_START
            LogError.Write("CHECK_HOST - Error source: " & ex.Source & vbNewLine _
                         & "Error code: System error!" & vbNewLine _
                         & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_HOST)

            Return v_lngError
            'Throw ex
        Finally
            'Wtite Log
            'mv_lwLogWriter.StopWriteLog(gc_MODULE_HOST, "Host", "txRouter", "CheckHOST", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "")
            v_obj.Dispose()
            If Not v_trace Is Nothing Then
                v_trace.Dispose()
            End If
            If Not v_Msg Is Nothing Then
                v_Msg.Dispose()
            End If
            If Not v_replace Is Nothing Then
                v_replace.Dispose()
            End If
            If Not tr2 Is Nothing Then
                tr2.Close()
                tr2.Dispose()
            End If
        End Try
    End Function
    Private Function HostCheckReports(ByVal v_strFileNameCA As String, ByVal v_strSignatureCA As String, ByRef v_strErrMsg As String, _
                                      ByRef v_intErrCount As Integer, ByVal v_strTLName As String, ByVal v_strTLID As String, ByVal mv_oSignServer As SignServer) As Boolean
        Try
            'Get config for connecting BKAV LDAP
            'Dim v_abc As BkavCASign.CertificateServer
            v_intErrCount = 0
            'Dim v_intCHKSrv As Integer = 0\ Dim v_dataAccess As New DataAccess
            Dim v_dataAccess As New DataAccess
            Dim v_strVSDUsername As String = _
                        System.Configuration.ConfigurationManager.AppSettings("VSDUsername").ToString
            Dim v_strBKAVPassword As String = _
                        System.Configuration.ConfigurationManager.AppSettings("BKAVPassword").ToString
            Dim v_strLDAPIP As String = _
                        System.Configuration.ConfigurationManager.AppSettings("LDAPIP").ToString
            Dim v_strArrayOgarnization As String = _
                        System.Configuration.ConfigurationManager.AppSettings("ArrayOgarnization").ToString
            Dim v_strBKAVDomainComponent As String = _
                System.Configuration.ConfigurationManager.AppSettings("BKAVDomainComponent").ToString

            'Tách Đường dẫn từ client lên để lấy file trên server
            Dim v_arrFileName() As String
            v_arrFileName = v_strFileNameCA.Split("\")
            Dim i As Integer = v_arrFileName.Length
            v_strFileNameCA = v_arrFileName(i - 1)

            v_arrFileName = v_strFileNameCA.Split("'")
            Dim v_strDateCreated As String = v_arrFileName(3)
            Dim v_strRptId As String = v_arrFileName(1)
            Dim v_strCAKey As String = v_arrFileName(2)
            Dim v_strTLName1 = v_arrFileName(0)
            Dim v_strAppPath As String = System.AppDomain.CurrentDomain.BaseDirectory
            'Them lay file tu FTP Server về

            Dim v_strLocalDir = v_strAppPath & "\Log\" & v_strDateCreated
            Dim v_strServerFile As String = v_strAppPath & "\Log\" & v_strDateCreated & "\" & v_strFileNameCA
            ' Đẩy file lên FTP server
            If Not File.Exists(v_strServerFile) Then
                Dim v_strServerAddress, v_strServerPort, v_strUsername, v_strPassword, v_strRemotePath, v_strRootPath As String
                v_dataAccess.GetSysVar("FILEFTPSVR", "ServerAddress", "0001", v_strServerAddress)
                v_dataAccess.GetSysVar("FILEFTPSVR", "ServerPort", "0001", v_strServerPort)
                v_dataAccess.GetSysVar("FILEFTPSVR", "Username", "0001", v_strUsername)
                v_dataAccess.GetSysVar("FILEFTPSVR", "Password", "0001", v_strPassword)
                v_dataAccess.GetSysVar("FILEFTPSVR", "RemotePath", "0001", v_strRemotePath)
                v_dataAccess.GetSysVar("FILEFTPSVR", "RootPath", "0001", v_strRootPath)

                Dim v_oWriter As StreamWriter
                v_oWriter = New StreamWriter(v_strLocalDir & "\" & Replace(v_strFileNameCA, ".dat", ".bat"))
                v_oWriter.WriteLine("@ftp -i -s:""%~f0""&GOTO:EOF")
                v_oWriter.WriteLine("open " & v_strServerAddress)
                v_oWriter.WriteLine(v_strUsername)
                v_oWriter.WriteLine(v_strPassword)
                v_oWriter.WriteLine("lcd " & """" & v_strLocalDir & """")
                v_oWriter.WriteLine("cd " & v_strRemotePath)
                v_oWriter.WriteLine("binary")
                v_oWriter.WriteLine("get " & v_strFileNameCA & " " & v_strFileNameCA)
                v_oWriter.WriteLine("bye" & vbCrLf)

                v_oWriter.Close()

                Dim v_oProcess As Process
                v_oProcess = New Process

                v_oProcess.StartInfo.FileName = v_strLocalDir & "\" & Replace(v_strFileNameCA, ".dat", ".bat")
                v_oProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
                v_oProcess.StartInfo.CreateNoWindow = True
                v_oProcess.Start()
                'v_oProcess.WaitForExit()
                v_oProcess.Close()
                System.Threading.Thread.Sleep(3 * 1000)
                'end Them lay file tu FTP Server về
                'v_dataAccess.GetSysVar("CA", "APP_LOCATION", v_strAppPath)


                If Not File.Exists(v_strServerFile) Then
                    v_intErrCount = 1
                    v_strErrMsg = "File dữ liệu ở client và server không khớp"
                    Exit Function
                End If
            End If
            Dim v_Stream As New System.IO.StreamReader(v_strServerFile)
            Dim v_strData As String = v_Stream.ReadToEnd
            Dim v_strSQL As String

            If ServerBussinessCA.Check_Signature_client(v_strData, v_strSignatureCA, v_strVSDUsername, v_strBKAVPassword, _
                                                                             v_strLDAPIP, v_strArrayOgarnization, v_strBKAVDomainComponent, v_strTLName) Then
                'If v_intCHKSrv = 0 Then
                '    v_intErrCount = 1
                '    v_strErrMsg = "Dữ liệu hợp lệ"
                '    Exit Function
                'Else
                '    v_intErrCount = 1
                '    v_strErrMsg = "Dữ liệu trong database và dữ liệu lưu ở file server không khớp"
                '    Exit Function
                'End If

                ' lấy dữ liệu gốc
                Dim v_xmlDocumentMessage As New XmlDocumentEx

                v_strTLName = v_strTLName1
                v_xmlDocumentMessage.LoadXml(v_strData)
                Dim v_attrColl As Xml.XmlAttributeCollection = v_xmlDocumentMessage.DocumentElement.Attributes
                Dim v_strDataFileServer As String = CStr(CType(v_attrColl.GetNamedItem("DATA"), Xml.XmlAttribute).Value)
                v_strDataFileServer = ServerBussinessCA.DecryptXML(v_strDataFileServer, v_strTLName)
                'lấy xml gốc tạo báo cáo 
                'bangpv: sửa lại lấy từ strB64 về arr --> chuyển về dataset 
                Dim v_arrDataServer As Byte() = Convert.FromBase64String(v_strDataFileServer)
                Dim v_dsFileServer As New DataSet
                v_dsFileServer = ZetaCompressionLibrary.CompressionHelper.DecompressDataSet(v_arrDataServer)
                'end bangpv
                'Lấy dữ liệu báo cáo dựa vào parameter lưu trong db
                'get tlid 
                v_strSQL = "select tlid from tlprofiles where tlname ='" & v_strTLName & "'"
                Dim v_ds As DataSet = v_dataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                v_strTLID = v_ds.Tables(0).Rows(0).Item("tlid")
                'v_ds = Nothing
                If v_strCAKey = "" Then
                    v_strSQL = "select crpparameter  from tlsession where tlid ='" & v_strTLID & "' and tllogid ='" & v_strRptId & "' and txdate = to_date('" _
            & v_strDateCreated & "','YYYY/MM/DD') and type=3"
                Else
                    v_strSQL = "select crpparameter  from tlsession where tlid ='" & v_strTLID & "' and tllogid ='" & v_strRptId & "' and txdate = to_date('" _
            & v_strDateCreated & "','YYYY/MM/DD') and type=3 and (METADIGITALSIGN0='" & v_strCAKey & "')"
                End If

                'v_ds = v_dataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                Dim v_strMessage As String = v_dataAccess.ExecuteSQLReturnString(CommandType.Text, v_strSQL)
                'v_ds = Nothing
                Dim v_obj As RP.Report
                v_obj = New RP.Report

                v_xmlDocumentMessage.LoadXml(v_strMessage)
                v_ds = v_obj.CreateReportCheck(v_xmlDocumentMessage)

                'compare dataset 


                'end compare dataset 
                'Dim pv_arrByte As Byte()
                'pv_arrByte = ZetaCompressionLibrary.CompressionHelper.CompressDataSet(v_ds)
                'Chuyển về str
                'v_strData = v_ds.GetXml() 'ByteArrayToStr(pv_arrByte)
                'Lay ra dataset de so sanh 

                'Dim v_dsData As New DataSet

                'Dim v_reader As New System.IO.StringReader(v_strDataFileServer)
                'v_dsFileServer.ReadXml(v_reader)
                'Dim v_reader1 As New System.IO.StringReader(v_strData)
                'v_dsData.ReadXml(v_reader1)
                Dim v_oDataRow1 = v_dsFileServer.Tables(0).AsEnumerable
                'v_dsFileServer = Nothing
                Dim v_oDataRow2 = v_ds.Tables(0).AsEnumerable

                Dim v_oResult As IEnumerable(Of DataRow) = v_oDataRow1.Intersect(v_oDataRow2, DataRowComparer.Default)
                Dim v_arrResult As DataRow() = v_oResult.ToArray

                For i = 0 To v_arrResult.Length - 1
                    Dim v_oRow = v_arrResult(i)
                    v_dsFileServer.Tables(0).Rows.Remove(v_oRow)
                Next

                'For Each DataRow In v_oResult

                'Next

                Dim v_strTMP As String = v_dsFileServer.GetXml
                'Ký số với dữ liệu trả về

                'v_strData = ServerBussinessCA.CombineData(v_strData, mv_oSignServer)
                v_strData = Convert.ToBase64String(ZetaCompressionLibrary.CompressionHelper.CompressDataSet(v_ds))
                'If ServerBussinessCA.Check_VSD_Signature(v_strData, v_strDataFileServer, mv_oSignServer) Then
                If v_dsFileServer.Tables(0).Rows.Count = 0 Then
                    v_intErrCount = 1
                    v_strErrMsg = "Dữ liệu hợp lệ"
                Else
                    v_intErrCount = 1
                    v_strErrMsg = "Dữ liệu trong database và dữ liệu lưu ở file server không khớp: " & vbCrLf & v_strTMP

                End If
                'verify với dữ liệu lưu ở file 
            Else
                'thông báo kiểm tra dữ liệu không thành công 
                v_intErrCount = 1
                v_strErrMsg = "File dữ liệu ở client và server không khớp"
            End If
            'If blnChk = True Then
            '    Return True

            'Else
            '    Return False
            'End If



        Catch ex As Exception
            v_strErrMsg = "File dữ liệu không hợp lệ!. Chi tiết: " & ex.Message
            v_intErrCount += 1
            Return False
        Finally
            GC.Collect()
        End Try
    End Function
    Private Function HostExportReports(ByVal v_strFileNameCA As String, ByVal v_strSignatureCA As String, ByRef v_strErrMsg As String, _
                                      ByRef v_intErrCount As Integer, ByVal v_strTLName As String, ByVal v_strTLID As String, ByVal mv_oSignServer As SignServer) As Boolean
        Try
            'Get config for connecting BKAV LDAP
            Dim v_dataAccess As New DataAccess
            'Dim v_abc As BkavCASign.CertificateServer
            v_intErrCount = 0
            'Dim v_intCHKSrv As Integer = 0
            Dim v_strVSDUsername As String = _
                        System.Configuration.ConfigurationManager.AppSettings("VSDUsername").ToString
            Dim v_strBKAVPassword As String = _
                        System.Configuration.ConfigurationManager.AppSettings("BKAVPassword").ToString
            Dim v_strLDAPIP As String = _
                        System.Configuration.ConfigurationManager.AppSettings("LDAPIP").ToString
            Dim v_strArrayOgarnization As String = _
                        System.Configuration.ConfigurationManager.AppSettings("ArrayOgarnization").ToString
            Dim v_strBKAVDomainComponent As String = _
                System.Configuration.ConfigurationManager.AppSettings("BKAVDomainComponent").ToString

            'Tách Đường dẫn từ client lên để lấy file trên server
            Dim v_arrFileName() As String
            v_arrFileName = v_strFileNameCA.Split("\")
            Dim i As Integer = v_arrFileName.Length
            v_strFileNameCA = v_arrFileName(i - 1)

            v_arrFileName = v_strFileNameCA.Split("'")
            Dim v_strDateCreated As String = v_arrFileName(3)
            Dim v_strRptId As String = v_arrFileName(1)
            Dim v_strCAKey As String = v_arrFileName(2)
            Dim v_strTLName1 = v_arrFileName(0)
            Dim v_strAppPath As String = System.AppDomain.CurrentDomain.BaseDirectory

            'Them lay file tu FTP Server về

            Dim v_strLocalDir = v_strAppPath & "\Log\" & v_strDateCreated
            ' Đẩy file lên FTP server
            Dim v_strServerAddress, v_strServerPort, v_strUsername, v_strPassword, v_strRemotePath, v_strRootPath As String
            v_dataAccess.GetSysVar("FILEFTPSVR", "ServerAddress", "0001", v_strServerAddress)
            v_dataAccess.GetSysVar("FILEFTPSVR", "ServerPort", "0001", v_strServerPort)
            v_dataAccess.GetSysVar("FILEFTPSVR", "Username", "0001", v_strUsername)
            v_dataAccess.GetSysVar("FILEFTPSVR", "Password", "0001", v_strPassword)
            v_dataAccess.GetSysVar("FILEFTPSVR", "RemotePath", "0001", v_strRemotePath)
            v_dataAccess.GetSysVar("FILEFTPSVR", "RootPath", "0001", v_strRootPath)

            Dim v_oWriter As StreamWriter
            v_oWriter = New StreamWriter(v_strLocalDir & "\" & Replace(v_strFileNameCA, ".dat", ".bat"))
            v_oWriter.WriteLine("@ftp -i -s:""%~f0""&GOTO:EOF")
            v_oWriter.WriteLine("open " & v_strServerAddress)
            v_oWriter.WriteLine(v_strUsername)
            v_oWriter.WriteLine(v_strPassword)
            v_oWriter.WriteLine("lcd " & """" & v_strLocalDir & """")
            v_oWriter.WriteLine("cd " & v_strRemotePath)
            v_oWriter.WriteLine("binary")
            v_oWriter.WriteLine("get " & v_strFileNameCA & " " & v_strFileNameCA)
            v_oWriter.WriteLine("bye" & vbCrLf)

            v_oWriter.Close()

            Dim v_oProcess As Process
            v_oProcess = New Process

            v_oProcess.StartInfo.FileName = v_strLocalDir & "\" & Replace(v_strFileNameCA, ".dat", ".bat")
            v_oProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
            v_oProcess.StartInfo.CreateNoWindow = True
            v_oProcess.Start()
            'v_oProcess.WaitForExit()
            v_oProcess.Close()
            System.Threading.Thread.Sleep(3 * 1000)
            'end Them lay file tu FTP Server về
            'v_dataAccess.GetSysVar("CA", "APP_LOCATION", v_strAppPath)
            Dim v_strServerFile As String = v_strAppPath & "\Log\" & v_strDateCreated & "\" & v_strFileNameCA

            If Not File.Exists(v_strServerFile) Then
                v_intErrCount = 1
                v_strErrMsg = "File dữ liệu ở client và server không khớp"
                Exit Function
            End If

            Dim v_Stream As New System.IO.StreamReader(v_strServerFile)
            Dim v_strData As String = v_Stream.ReadToEnd
            Dim v_strSQL As String



            ' lấy dữ liệu gốc
            Dim v_xmlDocumentMessage As New XmlDocumentEx

            v_strTLName = v_strTLName1
            v_xmlDocumentMessage.LoadXml(v_strData)
            Dim v_attrColl As Xml.XmlAttributeCollection = v_xmlDocumentMessage.DocumentElement.Attributes
            Dim v_strDataFileServer As String = CStr(CType(v_attrColl.GetNamedItem("DATA"), Xml.XmlAttribute).Value)
            'Lay chu ky cua VSD
            Dim v_strVSDSignal As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeSignatureVSD), Xml.XmlAttribute).Value)

            'Lay chu ky cua Nguoi dung
            'gc_AtributeSignatureClient
            Dim v_strUserSignal As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeSignatureClient), Xml.XmlAttribute).Value)
            v_strDataFileServer = ServerBussinessCA.DecryptXML_Exports(v_strDataFileServer, v_strTLName, v_strDateCreated)
            'lấy xml gốc tạo báo cáo 
            'bangpv: sửa lại lấy từ strB64 về arr --> chuyển về dataset 
            Dim v_arrDataServer As Byte() = Convert.FromBase64String(v_strDataFileServer)
            Dim v_dsFileServer As New DataSet
            v_dsFileServer = ZetaCompressionLibrary.CompressionHelper.DecompressDataSet(v_arrDataServer)
            v_strData = v_dsFileServer.GetXml
            'Ghi ra file
            Dim XMLDocumentMessage As New XmlDocumentEx
            Dim dataElement As Xml.XmlElement
            Dim v_attrData, v_attrVSDSignature, v_attrClientSignature As Xml.XmlAttribute
            dataElement = XMLDocumentMessage.CreateElement("RPData")

            v_attrData = XMLDocumentMessage.CreateAttribute("DATA")
            v_attrData.Value = v_strData 'ByteArrayToStr(v_arrByte)
            dataElement.Attributes.Append(v_attrData)

            v_attrVSDSignature = XMLDocumentMessage.CreateAttribute(gc_AtributeSignatureVSD)
            v_attrVSDSignature.Value = v_strVSDSignal
            dataElement.Attributes.Append(v_attrVSDSignature)

            v_attrClientSignature = XMLDocumentMessage.CreateAttribute(gc_AtributeSignatureClient)
            v_attrClientSignature.Value = v_strUserSignal
            dataElement.Attributes.Append(v_attrClientSignature)

            XMLDocumentMessage.AppendChild(dataElement)
            'Dim v_oWriter As System.IO.StreamWriter
            'Dim v_strAppPath As String = System.AppDomain.CurrentDomain.BaseDirectory

            'v_DataAccess.GetSysVar("CA", "VSD_CA", v_strLocalDir)
            'v_strLocalDir = v_strLocalDir & "\Server"

            v_strSQL = "select to_char(to_date(varvalue,'dd/mm/yyyy'),'YYYYMMDD') date_, to_char(to_date(varvalue,'dd/mm/yyyy'),'YYYYMMDD')||''''||to_char(sysdate,'hh24miss') time from sysvar where varname ='CURRDATE' and brid ='0001'"
            Dim v_trace As System.Data.DataSet = v_dataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            Dim v_strTime As String = v_trace.Tables(0).Rows(0)("time")
            v_strLocalDir = v_trace.Tables(0).Rows(0)("date_")

            Dim f As New IO.DirectoryInfo(v_strAppPath & "\Log")
            If Not f.Exists Then
                Directory.CreateDirectory(v_strAppPath & "\Log")
            End If

            v_strAppPath = v_strAppPath & "\Log\" & v_strLocalDir

            f = New IO.DirectoryInfo(v_strAppPath)
            If Not f.Exists Then
                Directory.CreateDirectory(v_strAppPath)
            End If
            Dim v_strFileName = v_strTLName & "'Export_RPT_" & v_strTime & "'"
            If File.Exists(v_strAppPath & "\" & v_strFileName & ".xml") Then
                File.Delete(v_strAppPath & "\" & v_strFileName & ".xml")
            End If
            'end bangpv
            'bangpv: Luu rieng chu ky va data
            Dim v_strFileName_data = v_strTLName & "'Export_RPT_DATA_" & v_strTime & "'"
            If File.Exists(v_strAppPath & "\" & v_strFileName_data & ".xml") Then
                File.Delete(v_strAppPath & "\" & v_strFileName_data & ".xml")
            End If
            v_oWriter = New StreamWriter(v_strAppPath & "\" & v_strFileName_data & ".xml")
            v_oWriter.WriteLine(v_strData)
            v_oWriter.Close()
            'end bangpv 
            v_oWriter = New StreamWriter(v_strAppPath & "\" & v_strFileName & ".xml")
            v_oWriter.WriteLine(XMLDocumentMessage.InnerXml)
            v_oWriter.Close()


            v_intErrCount = 1
            v_strErrMsg = "Dữ liệu hợp lệ" & vbCrLf & "Đã kết xuất file tại: " & v_strAppPath & "\" & v_strFileName & ".xml"


        Catch ex As Exception
            v_strErrMsg = "File dữ liệu không hợp lệ!. Chi tiết: " & ex.Message
            v_intErrCount += 1
            Return False
        Finally
            GC.Collect()
        End Try
    End Function
    '3079
    Private Function Host_insert_3079_3197(ByVal pv_oNode As Xml.XmlNode, ByVal v_strXMLData As String, ByRef v_strErrMsg As String, _
                                     ByRef v_intErrCount As Integer, ByRef v_strWarning As String, ByRef v_intWarningCount As Integer) As Boolean
        Dim blnTran As Boolean = False
        'Dim v_trace As DataSet
        ' Dim v_strMFNO As String
        Dim v_ds As DataSet
        Dim v_blnStockType As Boolean = False
        Dim v_strAUTOID As String
        Dim v_strSTATUS As String = "0"
        Dim v_strTXNUM As String
        Dim v_strTXDATE As String
        Dim v_lngErr As Long = ERR_SYSTEM_OK
        Dim v_obj As New DataAccess
        'Dim tr2 As TextWriterTraceListener
        Dim v_trace_status, v_trace_path As String
        Try
            'Get config for connecting BKAV LDAP
            With pv_oNode
                v_obj.BeginTran()
                blnTran = True
                'Get message header
                Dim v_strBRID As String = .Attributes(gc_AtributeBRID).Value.ToString
                v_strSTATUS = .Attributes(gc_AtributeSTATUS).Value.ToString
                'bangpv: voi CA
                Dim v_strCAStatus As String = v_strSTATUS
                Dim v_strSTATUS_TEXT As String = .Attributes(gc_AtributeSTATUSTEXT).Value.ToString
                v_strAUTOID = .Attributes(gc_AtributeAUTOID).Value.ToString
                v_strTXNUM = .Attributes(gc_AtributeTXNUM).Value.ToString
                v_strTXDATE = .Attributes(gc_AtributeTXDATE).Value.ToString
                Dim v_strBUSDATE As String = .Attributes(gc_AtributeBUSDATE).Value.ToString
                Dim v_strTLTXCD As String = .Attributes(gc_AtributeTLTXCD).Value.ToString
                Dim v_strTXDESC As String = .Attributes(gc_AtributeTXDESC).Value.ToString
                Dim v_strBRCODE As String = .Attributes(gc_AtributeBRCODE).Value.ToString
                Dim v_strTLID As String = .Attributes(gc_AtributeTLID).Value.ToString
                Dim v_strCHKID As String = .Attributes(gc_AtributeCHKID).Value.ToString
                Dim v_strOFFID As String = .Attributes(gc_AtributeOFFID).Value.ToString
                Dim v_strCFRID As String = .Attributes(gc_AtributeCFRID).Value.ToString
                Dim v_strTLNAME As String = .Attributes(gc_AtributeTLNAME).Value.ToString
                Dim v_strCHKNAME As String = .Attributes(gc_AtributeCHKNAME).Value.ToString
                Dim v_strOFFNAME As String = .Attributes(gc_AtributeOFFNAME).Value.ToString
                Dim v_strCFRNAME As String = .Attributes(gc_AtributeCFRNAME).Value.ToString
                Dim v_strIPADDRESS As String = .Attributes(gc_AtributeIPADDRESS).Value.ToString
                Dim v_strWSNAME As String = .Attributes(gc_AtributeWSNAME).Value.ToString
                Dim v_strSICODE As String = .Attributes(gc_AtributeSICODE).Value.ToString
                Dim v_strMICODE As String = .Attributes(gc_AtributeMICODE).Value.ToString
                Dim v_strCOMICODE As String '= .Attributes(gc_AtributeCOMICODE).Value.ToString
                Dim v_strMSGAMT As String = .Attributes(gc_AtributeMSGAMT).Value.ToString
                Dim v_strISPARENT As String = .Attributes(gc_AtributeISPARENT).Value.ToString
                Dim v_strPARENTID As String = .Attributes(gc_AtributePARENTID).Value.ToString
                Dim v_strPARENT_TEXT As String = .Attributes(gc_AtributePARENT_TEXT).Value.ToString
                Dim v_strTXNAME As String = .Attributes(gc_AtributeTXNAME).Value.ToString
                Dim v_strCHILDTLTXCD As String = .Attributes(gc_AtributeCHILDTLTXCD).Value.ToString
                Dim v_strISBRID As String = .Attributes(gc_AtributeISBRID).Value.ToString
                Dim v_strREASON As String = .Attributes(gc_AtributeREASON).Value.ToString
                'hanm5
                Dim v_strTXNOTE As String = .Attributes(gc_AtributeTXNOTE).Value.ToString
                Dim v_strVsdBrid As String = .Attributes(gc_AtributeVSDBRID).Value.ToString
                'Dim v_strVsdBrid2 As String = .Attributes(gc_AtributeVSDBRID2).Value.ToString
                Dim v_strTblChk As String = .Attributes(gc_AtributeTBLCHK).Value.ToString

                Dim v_strSYSVAR As String = ""
                Dim v_strCurrDate1 As String

                'Added by Thanglv9 -19/03/2013
                Dim v_strTradeacctno As String = ""
                Dim v_strSbvSicode As String = ""
                'End

                mv_strIpAddress = v_strIPADDRESS
                mv_strWsName = v_strWSNAME
                mv_strTellerName = v_strCFRNAME
                If mv_strTellerName = "" Then
                    mv_strTellerName = v_strOFFNAME
                    mv_strTellerId = v_strOFFID
                End If
                If mv_strTellerName = "" Then
                    mv_strTellerName = v_strCHKNAME
                    mv_strTellerId = v_strCHKID
                End If
                If mv_strTellerName = "" Then
                    mv_strTellerName = v_strTLNAME
                    mv_strTellerId = v_strTLID
                End If



                v_obj.GetSysVar("SYSTEM", "TRACE_STATUS", v_trace_status)
                v_obj.GetSysVar("SYSTEM", "TRACE_PATH", v_trace_path)
                v_obj.GetSysVar("SYSTEM", "CURRDATE", v_strBRID, v_strCurrDate1)
                'Dim xmlReader As New XmlTextReader(v_strXMLData)

                If v_strSTATUS.Trim() = CStr(TransactStatus.LOG_MEMBER_STAFF) Then
                    If v_strCHKID = gc_TRANSACTION_ZERO Then
                        v_strSTATUS = TransactStatus.APPROVED_MEMBER_MANAGER
                        v_strSTATUS_TEXT = APPROVED_MEMBER_MANAGER_TEXT
                        v_strCHKID = v_strTLID
                        v_strCHKNAME = v_strTLNAME
                        'v_strSQL = v_strSQL & " union select ordnum, trace, DMLSQL, DMLSQL1 from appdml where status = 0 and deleted = 0 and tltxcd = '" & v_strTLTXCD & "' and instr(txstatus,'" & v_strSTATUS & "')>0"
                        If v_strOFFID = gc_TRANSACTION_ZERO Then
                            v_strSTATUS = TransactStatus.APPROVED_VDS_STAFF
                            v_strOFFID = v_strCHKID
                            v_strOFFNAME = v_strCHKNAME
                            v_strSTATUS_TEXT = APPROVED_VDS_STAFF_TEXT
                            'v_strSQL = v_strSQL & " union select ordnum, trace, DMLSQL, DMLSQL1 from appdml where status = 0 and deleted = 0 and tltxcd = '" & v_strTLTXCD & "' and instr(txstatus,'" & v_strSTATUS & "')>0"
                            If v_strCFRID = gc_TRANSACTION_ZERO Then
                                v_strSTATUS = TransactStatus.CONFIRMED_VSD_MANAGER
                                v_strCFRID = v_strOFFID
                                v_strCFRNAME = v_strOFFNAME
                                v_strSTATUS_TEXT = CONFIRMED_VSD_MANAGER_TEXT
                                v_strBUSDATE = v_strTXDATE
                                'v_strSQL = v_strSQL & " union select ordnum, trace, DMLSQL, DMLSQL1 from appdml where status = 0 and deleted = 0 and tltxcd = '" & v_strTLTXCD & "' and instr(txstatus,'" & v_strSTATUS & "')>0"
                            End If
                        End If
                    End If
                End If
                If v_strSTATUS.Trim() = CStr(TransactStatus.APPROVED_MEMBER_MANAGER) Then
                    If v_strOFFID = gc_TRANSACTION_ZERO Then
                        v_strSTATUS = TransactStatus.APPROVED_VDS_STAFF
                        v_strOFFID = v_strCHKID
                        v_strOFFNAME = v_strCHKNAME
                        v_strSTATUS_TEXT = APPROVED_VDS_STAFF_TEXT
                        'v_strSQL = v_strSQL & " union select ordnum, trace, DMLSQL, DMLSQL1 from appdml where status = 0 and deleted = 0 and tltxcd = '" & v_strTLTXCD & "' and instr(txstatus,'" & v_strSTATUS & "')>0"
                        If v_strCFRID = gc_TRANSACTION_ZERO Then
                            v_strSTATUS = TransactStatus.CONFIRMED_VSD_MANAGER
                            v_strCFRID = v_strOFFID
                            v_strCFRNAME = v_strOFFNAME
                            v_strSTATUS_TEXT = CONFIRMED_VSD_MANAGER_TEXT
                            'v_strSQL = v_strSQL & " union select ordnum, trace, DMLSQL, DMLSQL1 from appdml where status = 0 and deleted = 0 and tltxcd = '" & v_strTLTXCD & "' and instr(txstatus,'" & v_strSTATUS & "')>0"
                        End If
                    End If
                End If
                If v_strSTATUS.Trim() = CStr(TransactStatus.APPROVED_VDS_STAFF) Then
                    If v_strCFRID = gc_TRANSACTION_ZERO Then
                        v_strSTATUS = TransactStatus.CONFIRMED_VSD_MANAGER
                        v_strCFRID = v_strOFFID
                        v_strCFRNAME = v_strOFFNAME
                        v_strSTATUS_TEXT = CONFIRMED_VSD_MANAGER_TEXT
                        'v_strSQL = v_strSQL & " union select ordnum, trace, DMLSQL, DMLSQL1 from appdml where status = 0 and deleted = 0 and tltxcd = '" & v_strTLTXCD & "' and instr(txstatus,'" & v_strSTATUS & "')>0"
                    End If
                End If

                Dim v_xmlDocument As New XmlDocumentEx
                v_xmlDocument.LoadXml(v_strXMLData)
                Dim v_nodeList As Xml.XmlNodeList
                v_nodeList = v_xmlDocument.SelectNodes("/KTP_CDS_TRANS/CDS_TRANS")
                'Dim v_reader1 As New System.IO.StringReader(v_strXMLData)
                'v_ds.ReadXml(v_reader1)
                'Dim xmlNodeRdr As New XmlNodeReader(v_xmlDocument)
                'v_ds.ReadXml(xmlNodeRdr)
                'duyet danh sach trong file xml 

                Dim v_strSBVMiCode, v_strQuantity, v_strNum As String
                'Add by Thanglv9 - 18/01/2013
                'Dinh nghia bien Menh gia
                Dim v_PartValue As Double
                'Dinh nghia SL chung khoan
                Dim v_QttySCode As Double
                Dim v_TotalQttySCode As Double = 0
                'End

                Dim v_strSQL, v_strSQLTmp As String
                'Added by Thanglv9 - 19/03/2013
                ''Tinh tong so luong CK
                'For i = 0 To v_nodeList.Count - 1
                '    v_strSICODE = v_nodeList.Item(i).ChildNodes(0).InnerText.ToString
                '    v_strQuantity = v_nodeList.Item(i).ChildNodes(2).InnerText.ToString
                '    'Lay Menh Gia cua Chung Khoan
                '    v_strSQL = "select a.part_value from rgsi a where a.sicode = '" & v_strSICODE & "'"
                '    v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                '    v_PartValue = v_ds.Tables(0).Rows(0)("part_value")
                '    v_TotalQttySCode = v_TotalQttySCode + (Convert.ToDouble(v_strQuantity) / v_PartValue)
                'Next

                'Tao giao dich 3197
                'Lấy autoid cho giao dịch cha
                v_strSQL = "select seq_tllog.NEXTVAL AUTOID from dual"
                v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                v_strAUTOID = v_ds.Tables(0).Rows(0)("AUTOID")

                v_strSQL = "insert into tllog (autoid, txnum, txdate, brid, tlid, offid, cfrid," _
                                & "chkid, tltxcd, off_line, busdate, txdesc, ipaddress," _
                                & "wsname, status, batchname, micode, deleted, msgamt," _
                                & "isparent, parentid, brcode, tlname, offname, cfrname," _
                                & "chkname, txname, status_text, parent_text," _
                                & "tmpid, childtltxcd, t_no, msgdate, col_value01, col_type01, col_desc01, col_value02, col_type02, col_desc02" _
                                & ", col_value04, col_type04, col_desc04," _
                                & "vsd_brid)"
                v_strSQL = v_strSQL & " select " & v_strAUTOID & ", GENERATE_TLLOG_CODE(" & v_strAUTOID & "), to_date('" & v_strTXDATE & "','DD/MM/YYYY')," _
                            & "'" & v_strBRID & "', '" & v_strTLID & "','" & v_strOFFID & "','" & v_strCFRID & "','" & v_strCHKID & "','3197',null, null, '" & v_strTXDESC & "','" _
                            & v_strIPADDRESS & "','" & v_strWSNAME & "','" & v_strSTATUS & "', null, '" & v_strMICODE & "',0," & v_QttySCode.ToString & ",2,0,'" & v_strBRCODE & "','" _
                            & v_strTLNAME & "','" & v_strOFFNAME & "','" & v_strCFRNAME & "','" & v_strCHKNAME & "','" & v_strTXNAME & "','" & v_strSTATUS_TEXT & "','Giao dịch lẻ'," _
                            & "null,'3198',null,null,'3079','C','3079','" _
                            & v_strTXDATE & "','D','" & v_strTXDATE & "','" _
                            & v_strMICODE & "','C','" & v_strMICODE & "','" _
                            & v_strVsdBrid & "' from dual"
                v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                'End
                Dim v_strPaAutoid As String = ""
                Dim v_strPaTXNUM As String = ""
                Dim v_intNumCount As Integer = 0
                Dim v_strDeleted_log As Integer = 0
                Dim v_strStatus_log As Integer = 0
                v_intWarningCount = 0
                For i = 0 To v_nodeList.Count - 1
                    'Tao giao dich con 3198
                    v_strSbvSicode = v_nodeList.Item(i).ChildNodes(0).InnerText.ToString
                    v_strSBVMiCode = v_nodeList.Item(i).ChildNodes(1).InnerText.ToString
                    v_strQuantity = v_nodeList.Item(i).ChildNodes(2).InnerText.ToString
                    v_strNum = v_nodeList.Item(i).ChildNodes(3).InnerText.ToString
                    'Kiem tra GD da duoc nhan trong lan gui truoc hay chua
                    v_strSQL = "select * from cds_tranlog_vsd where deleted = 0 and status = 0 and num_operation = to_number('" & v_strNum & "')"
                    v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                    If v_ds.Tables(0).Rows.Count > 0 Then
                        v_intNumCount = v_intNumCount + 1
                    Else
                        'Added by Thanglv9 - 18/01/2013
                        'Kiem tra Sicode ton tai
                        v_strSQL = "select distinct sicode from rgsi a where a.sbv_sicode = '" & v_strSbvSicode & "' and a.brid = '" & v_strBRID & "' and a.deleted = 0 and a.status in (0,4) "
                        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                        If v_ds.Tables(0).Rows.Count = 0 Then
                            v_intErrCount = 1
                            v_strErrMsg = "Không tồn tại Mã GTCG :" & v_strSbvSicode
                            v_obj.Rollback()
                            Return False
                            Exit Function
                        End If
                        v_strSICODE = v_ds.Tables(0).Rows(0)("SICODE")
                        'Lay Menh Gia cua Chung Khoan
                        v_strSQL = "select a.part_value from rgsi a where a.sicode = '" & v_strSICODE & "' and a.deleted = 0 and a.status in (0,4)"
                        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                        v_PartValue = v_ds.Tables(0).Rows(0)("part_value")
                        'Tinh SL Chung khoan
                        v_QttySCode = Convert.ToDouble(v_strQuantity) / v_PartValue
                        'End

                        'Kiem tra CK da den ngay dao han hay chua
                        'Da dao han cap nhat vao tllog voi deleted = 1
                        'Thanglv9 06/05/2014
                        v_strSQLTmp = "select distinct sicode from rgsi a where a.sbv_sicode = '" & v_strSbvSicode & "' and a.brid = '" & v_strBRID & "' and a.deleted = 0 and a.status in (0,4) and due_date <= to_date('" & v_strTXDATE & "','DD/MM/YYYY')"
                        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                        If v_ds.Tables(0).Rows.Count > 0 Then
                            v_strDeleted_log = 1
                            'v_strSTATUS = 4
                        Else
                            v_strDeleted_log = 0
                        End If
                        ' Sinh 3198
                        'Lay thong tin nha dau tu ben chuyen 
                        v_strSQLTmp = "select a.tradeacctno,a.iicode,b.full_name , b.cardno ,to_char(b.carddate,'DD/MM/YYYY') carddate,b.cardtype      from rgiiia a, rgii b " _
                                    & "where a.micode ='501' and a.sbv_micode ='" & v_strSBVMiCode & "' and a.iicode = b.iicode and a.status =0 and a.deleted =0"
                        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                        'Dim v_strTradeacctno As String = ""
                        Dim v_strFull_name As String = ""
                        Dim v_strCardNo As String = ""
                        Dim v_strCardDate As String = ""
                        Dim v_strCardType As String = ""
                        Dim v_strIICODE As String = ""
                        If v_ds.Tables(0).Rows.Count > 0 Then
                            v_strTradeacctno = v_ds.Tables(0).Rows(0)("TRADEACCTNO")
                            v_strFull_name = v_ds.Tables(0).Rows(0)("FULL_NAME")
                            v_strCardNo = v_ds.Tables(0).Rows(0)("CARDNO")
                            v_strCardDate = v_ds.Tables(0).Rows(0)("CARDDATE")
                            v_strCardType = v_ds.Tables(0).Rows(0)("CARDTYPE")
                            v_strIICODE = v_ds.Tables(0).Rows(0)("IICODE")
                        Else
                            v_intErrCount = 1
                            v_strErrMsg = "Không có NĐT nào có mã thành viên tại sở là :" & v_strSBVMiCode
                            v_obj.Rollback()
                            Return False
                            Exit Function
                        End If

                        'Edited by Thanglv9 - 19/03/2013
                        'Lay autoid 3198
                        v_strSQL = "select seq_tllog.NEXTVAL AUTOID,GENERATE_TLLOG_CODE(seq_tllog.currval) TXNUM from dual"
                        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                        v_strPaAutoid = v_ds.Tables(0).Rows(0)("AUTOID")
                        v_strPaTXNUM = v_ds.Tables(0).Rows(0)("TXNUM")
                        'End
                        'Lay thong tin NDT ben nhan
                        v_strSQLTmp = "select a.micode,a.tradeacctno,a.iicode,b.full_name , b.cardno ,to_char(b.carddate,'DD/MM/YYYY') carddate,b.cardtype      from rgiiia a, rgii b " _
                                    & "where a.micode <>'501' and a.sbv_micode ='" & v_strSBVMiCode & "' and a.iicode = b.iicode and a.status =0 and a.deleted =0"
                        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                        Dim v_strTradeacctno1 As String = ""
                        Dim v_strFull_name1 As String = ""
                        Dim v_strCardNo1 As String = ""
                        Dim v_strCardDate1 As String = ""
                        Dim v_strCardType1 As String = ""
                        If v_ds.Tables(0).Rows.Count > 0 Then
                            If v_ds.Tables(0).Rows.Count > 1 Then
                                v_strTradeacctno1 = ""
                                v_strFull_name1 = v_ds.Tables(0).Rows(0)("FULL_NAME")
                                v_strCardNo1 = v_ds.Tables(0).Rows(0)("CARDNO")
                                v_strCardDate1 = v_ds.Tables(0).Rows(0)("CARDDATE")
                                v_strCardType1 = v_ds.Tables(0).Rows(0)("CARDTYPE")
                                v_strCOMICODE = ""
                                v_intWarningCount = v_intWarningCount + 1
                                v_strWarning = v_strWarning & "Dòng " & v_intWarningCount & ": (Số chứng từ :" & v_strPaTXNUM & ") - Thành viên có mã :" & v_strSBVMiCode & " có nhiều tài khoản tại nhiều TVLK!" & vbCrLf
                            Else
                                v_strTradeacctno1 = v_ds.Tables(0).Rows(0)("TRADEACCTNO")
                                v_strFull_name1 = v_ds.Tables(0).Rows(0)("FULL_NAME")
                                v_strCardNo1 = v_ds.Tables(0).Rows(0)("CARDNO")
                                v_strCardDate1 = v_ds.Tables(0).Rows(0)("CARDDATE")
                                v_strCardType1 = v_ds.Tables(0).Rows(0)("CARDTYPE")
                                v_strCOMICODE = v_ds.Tables(0).Rows(0)("MICODE")
                            End If
                        Else
                            v_strTradeacctno1 = ""
                            v_strFull_name1 = ""
                            v_strCardNo1 = ""
                            v_strCardDate1 = ""
                            v_strCardType1 = ""
                            v_strCOMICODE = ""
                            v_intWarningCount = v_intWarningCount + 1
                            v_strWarning = v_strWarning & "Dòng " & v_intWarningCount & ": (Số chứng từ :" & v_strPaTXNUM & ") - Thành viên có mã :" & v_strSBVMiCode & " chưa đăng ký tài khoản tại TVLK nào!" & vbCrLf
                        End If
                        v_strSQL = "insert into tllog (autoid, txnum, txdate, brid, tlid, offid, cfrid," _
                                    & "chkid, tltxcd, off_line, busdate, txdesc, ipaddress," _
                                    & "wsname, status, batchname, micode, deleted, msgamt," _
                                    & "isparent, parentid, brcode, tlname, offname, cfrname," _
                                    & "chkname, txname, status_text, parent_text, sicode," _
                                    & "tmpid, childtltxcd, t_no, msgdate, col_value01, col_type01, col_desc01, col_value02, col_type02, col_desc02" _
                                    & ", col_value03, col_type03, col_desc03, col_value04, col_type04, col_desc04, col_value05, col_type05, col_desc05," _
                                    & "col_value06, col_type06, col_desc06, col_value07, col_type07, col_desc07, col_value08, col_type08, col_desc08, " _
                                    & "col_value09, col_type09, col_desc09,col_value13, col_type13, col_desc13,col_value14, col_type14, col_desc14 " _
                                    & ",col_value15, col_type15, col_desc15,col_value16, col_type16, col_desc16,col_value17, col_type17, col_desc17 " _
                                    & ",col_value18, col_type18, col_desc18,col_value23, col_type23, col_desc23,col_value21,col_type21,col_desc21 " _
                                    & ", col_value22,col_type22,col_desc22, vsd_brid,comicode,col_value30,col_type30,col_desc30 )"
                        v_strSQL = v_strSQL & " select " & v_strPaAutoid & ",'" & v_strPaTXNUM & "', to_date('" & v_strTXDATE & "','DD/MM/YYYY')," _
                                    & "'" & v_strBRID & "', '" & v_strTLID & "','" & v_strOFFID & "','" & v_strCFRID & "','" & v_strCHKID & "','3198',null, null, '" & v_strTXDESC & "','" _
                                    & v_strIPADDRESS & "','" & v_strWSNAME & "','" & v_strSTATUS & "', null, '" & v_strMICODE & "'," & v_strDeleted_log & "," & v_QttySCode.ToString & ",0," & v_strAUTOID & ",'" & v_strBRCODE & "','" _
                                    & v_strTLNAME & "','" & v_strOFFNAME & "','" & v_strCFRNAME & "','" & v_strCHKNAME & "','" & v_strTXNAME & "','" & v_strSTATUS_TEXT & "','Giao dịch con'," _
                                    & "'" & v_strSICODE & "',null,null,null,null,GENERATE_TLLOG_CODE(" & v_strAUTOID & ") col_value01,'C',GENERATE_TLLOG_CODE(" & v_strAUTOID & ") col_desc01,'" _
                                    & v_strTXDATE & "','D','" & v_strTXDATE & "',null,'D',NULL," _
                                    & "'1','N','1 - CK phổ thông','" & v_strTradeacctno & "','C','" & v_strTradeacctno & "','" _
                                    & v_strFull_name & "','C','" & v_strFull_name & "','" & v_strCardType & "', 'N','" & v_strCardType & "','" & v_strCardNo & "','C','" & v_strCardNo & "','" _
                                & v_strCardDate & "','D','" & v_strCardDate & "','" & v_strTradeacctno1 & "','C','" & v_strTradeacctno1 & "','" & v_strFull_name1 & "','C','" _
                                & v_strFull_name1 & "','" & v_strCardType1 & "','N','" _
                                & v_strCardType1 & "','" & v_strCardNo1 & "','C','" & v_strCardNo1 & "','" & v_strCardDate1 & "','D','" & v_strCardDate1 & "','" _
                                & v_QttySCode & "','N','" & v_QttySCode & "','3079','C','3079','" & v_strSICODE & "','C','" & v_strSICODE & "','" _
                                & v_strCOMICODE & "','C','" & v_strCOMICODE & "','" & v_strVsdBrid & "','" & v_strCOMICODE & "','" _
                                & v_strNum & "','N','" & v_strNum _
                                & "' from dual"
                        v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                    End If
                Next
                If v_intNumCount = v_nodeList.Count Then
                    v_intErrCount = 1
                    v_strErrMsg = "Tất cả các giao dịch trong File được tồn tại và được xử lý!"
                    v_obj.Rollback()
                    Return False
                    Exit Function
                End If
            End With
            v_obj.Commit()
            v_intErrCount = 1
            v_strErrMsg = "Giao dịch thành công"


        Catch ex As Exception
            v_strErrMsg = "File dữ liệu không hợp lệ!. Chi tiết: " & ex.Message
            v_intErrCount += 1
            v_obj.Rollback()
            Return False
        Finally
            GC.Collect()
        End Try
    End Function
    Private Function Host_insert_3079_SBV(ByVal pv_oNode As Xml.XmlNode, ByVal v_strXMLData As String, ByRef v_strErrMsg As String, _
                                     ByRef v_intErrCount As Integer, ByRef v_strWarning As String, ByRef v_intWarningCount As Integer) As Boolean
        Dim blnTran As Boolean = False
        'Dim v_trace As DataSet
        ' Dim v_strMFNO As String
        Dim v_ds As DataSet
        Dim v_blnStockType As Boolean = False
        Dim v_strAUTOID As String
        Dim v_strSTATUS As String = "0"
        Dim v_strTXNUM As String
        Dim v_strTXDATE As String
        Dim v_lngErr As Long = ERR_SYSTEM_OK
        Dim v_obj As New DataAccess
        'Dim tr2 As TextWriterTraceListener
        Dim v_trace_status, v_trace_path As String
        Try
            'Get config for connecting BKAV LDAP
            With pv_oNode
                v_obj.BeginTran()
                blnTran = True
                'Get message header
                Dim v_strBRID As String = .Attributes(gc_AtributeBRID).Value.ToString
                v_strSTATUS = .Attributes(gc_AtributeSTATUS).Value.ToString
                'bangpv: voi CA
                Dim v_strCAStatus As String = v_strSTATUS
                Dim v_strSTATUS_TEXT As String = .Attributes(gc_AtributeSTATUSTEXT).Value.ToString
                v_strAUTOID = .Attributes(gc_AtributeAUTOID).Value.ToString
                v_strTXNUM = .Attributes(gc_AtributeTXNUM).Value.ToString
                v_strTXDATE = .Attributes(gc_AtributeTXDATE).Value.ToString
                Dim v_strBUSDATE As String = .Attributes(gc_AtributeBUSDATE).Value.ToString
                Dim v_strTLTXCD As String = .Attributes(gc_AtributeTLTXCD).Value.ToString
                Dim v_strTXDESC As String = .Attributes(gc_AtributeTXDESC).Value.ToString
                Dim v_strBRCODE As String = .Attributes(gc_AtributeBRCODE).Value.ToString
                Dim v_strTLID As String = .Attributes(gc_AtributeTLID).Value.ToString
                Dim v_strCHKID As String = .Attributes(gc_AtributeCHKID).Value.ToString
                Dim v_strOFFID As String = .Attributes(gc_AtributeOFFID).Value.ToString
                Dim v_strCFRID As String = .Attributes(gc_AtributeCFRID).Value.ToString
                Dim v_strTLNAME As String = .Attributes(gc_AtributeTLNAME).Value.ToString
                Dim v_strCHKNAME As String = .Attributes(gc_AtributeCHKNAME).Value.ToString
                Dim v_strOFFNAME As String = .Attributes(gc_AtributeOFFNAME).Value.ToString
                Dim v_strCFRNAME As String = .Attributes(gc_AtributeCFRNAME).Value.ToString
                Dim v_strIPADDRESS As String = .Attributes(gc_AtributeIPADDRESS).Value.ToString
                Dim v_strWSNAME As String = .Attributes(gc_AtributeWSNAME).Value.ToString
                Dim v_strSICODE As String = .Attributes(gc_AtributeSICODE).Value.ToString
                Dim v_strMICODE As String = .Attributes(gc_AtributeMICODE).Value.ToString
                Dim v_strCOMICODE As String '= .Attributes(gc_AtributeCOMICODE).Value.ToString
                Dim v_strMSGAMT As String = .Attributes(gc_AtributeMSGAMT).Value.ToString
                Dim v_strISPARENT As String = .Attributes(gc_AtributeISPARENT).Value.ToString
                Dim v_strPARENTID As String = .Attributes(gc_AtributePARENTID).Value.ToString
                Dim v_strPARENT_TEXT As String = .Attributes(gc_AtributePARENT_TEXT).Value.ToString
                Dim v_strTXNAME As String = .Attributes(gc_AtributeTXNAME).Value.ToString
                Dim v_strCHILDTLTXCD As String = .Attributes(gc_AtributeCHILDTLTXCD).Value.ToString
                Dim v_strISBRID As String = .Attributes(gc_AtributeISBRID).Value.ToString
                Dim v_strREASON As String = .Attributes(gc_AtributeREASON).Value.ToString
                'hanm5
                Dim v_strTXNOTE As String = .Attributes(gc_AtributeTXNOTE).Value.ToString
                Dim v_strVsdBrid As String = .Attributes(gc_AtributeVSDBRID).Value.ToString
                'Dim v_strVsdBrid2 As String = .Attributes(gc_AtributeVSDBRID2).Value.ToString
                Dim v_strTblChk As String = .Attributes(gc_AtributeTBLCHK).Value.ToString

                Dim v_strSYSVAR As String = ""
                Dim v_strCurrDate1 As String

                'Added by Thanglv9 -19/03/2013
                Dim v_strTradeacctno As String = ""
                Dim v_strSbvSicode As String = ""
                Dim v_strFILENAMECA As String = Path.GetFileName(.Attributes(gc_AtributeFileNameCACHK).Value.ToString)
                'End

                mv_strIpAddress = v_strIPADDRESS
                mv_strWsName = v_strWSNAME
                mv_strTellerName = v_strCFRNAME
                If mv_strTellerName = "" Then
                    mv_strTellerName = v_strOFFNAME
                    mv_strTellerId = v_strOFFID
                End If
                If mv_strTellerName = "" Then
                    mv_strTellerName = v_strCHKNAME
                    mv_strTellerId = v_strCHKID
                End If
                If mv_strTellerName = "" Then
                    mv_strTellerName = v_strTLNAME
                    mv_strTellerId = v_strTLID
                End If



                v_obj.GetSysVar("SYSTEM", "TRACE_STATUS", v_trace_status)
                v_obj.GetSysVar("SYSTEM", "TRACE_PATH", v_trace_path)
                v_obj.GetSysVar("SYSTEM", "CURRDATE", v_strBRID, v_strCurrDate1)
                'Dim xmlReader As New XmlTextReader(v_strXMLData)

                If v_strSTATUS.Trim() = CStr(TransactStatus.LOG_MEMBER_STAFF) Then
                    If v_strCHKID = gc_TRANSACTION_ZERO Then
                        v_strSTATUS = TransactStatus.APPROVED_MEMBER_MANAGER
                        v_strSTATUS_TEXT = APPROVED_MEMBER_MANAGER_TEXT
                        v_strCHKID = v_strTLID
                        v_strCHKNAME = v_strTLNAME
                        'v_strSQL = v_strSQL & " union select ordnum, trace, DMLSQL, DMLSQL1 from appdml where status = 0 and deleted = 0 and tltxcd = '" & v_strTLTXCD & "' and instr(txstatus,'" & v_strSTATUS & "')>0"
                        If v_strOFFID = gc_TRANSACTION_ZERO Then
                            v_strSTATUS = TransactStatus.APPROVED_VDS_STAFF
                            v_strOFFID = v_strCHKID
                            v_strOFFNAME = v_strCHKNAME
                            v_strSTATUS_TEXT = APPROVED_VDS_STAFF_TEXT
                            'v_strSQL = v_strSQL & " union select ordnum, trace, DMLSQL, DMLSQL1 from appdml where status = 0 and deleted = 0 and tltxcd = '" & v_strTLTXCD & "' and instr(txstatus,'" & v_strSTATUS & "')>0"
                            If v_strCFRID = gc_TRANSACTION_ZERO Then
                                v_strSTATUS = TransactStatus.CONFIRMED_VSD_MANAGER
                                v_strCFRID = v_strOFFID
                                v_strCFRNAME = v_strOFFNAME
                                v_strSTATUS_TEXT = CONFIRMED_VSD_MANAGER_TEXT
                                v_strBUSDATE = v_strTXDATE
                                'v_strSQL = v_strSQL & " union select ordnum, trace, DMLSQL, DMLSQL1 from appdml where status = 0 and deleted = 0 and tltxcd = '" & v_strTLTXCD & "' and instr(txstatus,'" & v_strSTATUS & "')>0"
                            End If
                        End If
                    End If
                End If
                If v_strSTATUS.Trim() = CStr(TransactStatus.APPROVED_MEMBER_MANAGER) Then
                    If v_strOFFID = gc_TRANSACTION_ZERO Then
                        v_strSTATUS = TransactStatus.APPROVED_VDS_STAFF
                        v_strOFFID = v_strCHKID
                        v_strOFFNAME = v_strCHKNAME
                        v_strSTATUS_TEXT = APPROVED_VDS_STAFF_TEXT
                        'v_strSQL = v_strSQL & " union select ordnum, trace, DMLSQL, DMLSQL1 from appdml where status = 0 and deleted = 0 and tltxcd = '" & v_strTLTXCD & "' and instr(txstatus,'" & v_strSTATUS & "')>0"
                        If v_strCFRID = gc_TRANSACTION_ZERO Then
                            v_strSTATUS = TransactStatus.CONFIRMED_VSD_MANAGER
                            v_strCFRID = v_strOFFID
                            v_strCFRNAME = v_strOFFNAME
                            v_strSTATUS_TEXT = CONFIRMED_VSD_MANAGER_TEXT
                            'v_strSQL = v_strSQL & " union select ordnum, trace, DMLSQL, DMLSQL1 from appdml where status = 0 and deleted = 0 and tltxcd = '" & v_strTLTXCD & "' and instr(txstatus,'" & v_strSTATUS & "')>0"
                        End If
                    End If
                End If
                If v_strSTATUS.Trim() = CStr(TransactStatus.APPROVED_VDS_STAFF) Then
                    If v_strCFRID = gc_TRANSACTION_ZERO Then
                        v_strSTATUS = TransactStatus.CONFIRMED_VSD_MANAGER
                        v_strCFRID = v_strOFFID
                        v_strCFRNAME = v_strOFFNAME
                        v_strSTATUS_TEXT = CONFIRMED_VSD_MANAGER_TEXT
                        'v_strSQL = v_strSQL & " union select ordnum, trace, DMLSQL, DMLSQL1 from appdml where status = 0 and deleted = 0 and tltxcd = '" & v_strTLTXCD & "' and instr(txstatus,'" & v_strSTATUS & "')>0"
                    End If
                End If

                Dim v_xmlDocument As New XmlDocumentEx
                v_xmlDocument.LoadXml(v_strXMLData)
                Dim v_nodeList As Xml.XmlNodeList
                If InStr(1, v_strFILENAMECA, "KTP_CDS_TRANS", CompareMethod.Text) > 0 Then
                    v_nodeList = v_xmlDocument.SelectNodes("/KTP_CDS_TRANS/CDS_TRANS")
                ElseIf InStr(1, v_strFILENAMECA, "SBV_TRANS", CompareMethod.Text) > 0 Then
                    v_nodeList = v_xmlDocument.SelectNodes("/SBV_TRANS/CSD_TRANS")
                End If

                Dim v_strSBVMiCode As String = ""
                Dim v_strQuantity As String = ""
                Dim v_strNum As String = ""
                Dim v_strCODE As String = ""
                Dim v_strSBVComicode As String = ""
                Dim v_TLTXCD As String = ""
                Dim v_CHILDTLTXCD As String = ""
                Dim v_TltxName As String = ""
                Dim v_ChildtxName As String = ""
                Dim v_TransferType As String = ""
                Dim v_Type As String = ""
                'Add by Thanglv9 - 18/01/2013
                'Dinh nghia bien Menh gia
                Dim v_PartValue As Double
                Dim checkResult As Double
                'Dinh nghia SL chung khoan
                Dim v_QttySCode As Double
                Dim v_TotalQttySCode As Double = 0
                Dim v_ParamInt As Double = 100000
                'End

                If InStr(1, v_strFILENAMECA, "KTP_CDS_TRANS", CompareMethod.Text) > 0 Then
                    v_TLTXCD = "3197"
                    v_CHILDTLTXCD = "3198"
                    v_Type = "1"
                ElseIf InStr(1, v_strFILENAMECA, "SBV_TRANS", CompareMethod.Text) > 0 Then
                    v_TLTXCD = "3186"
                    v_CHILDTLTXCD = "3187"
                    v_Type = "2"
                End If

                Dim v_strSQL, v_strSQLTmp As String

                v_strSQL = "select txdesc TXNAME from tltx where deleted = 0 and status = 0 and tltxcd = '" & v_TLTXCD & "'"
                v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                v_TltxName = v_ds.Tables(0).Rows(0)("TXNAME")

                v_strSQL = "select txdesc TXNAME from tltx where deleted = 0 and status = 0 and tltxcd = '" & v_CHILDTLTXCD & "'"
                v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                v_ChildtxName = v_ds.Tables(0).Rows(0)("TXNAME")

                'Tao giao dich cha
                'Lấy autoid cho giao dịch cha
                v_strSQL = "select seq_tllog.NEXTVAL AUTOID from dual"
                v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                v_strAUTOID = v_ds.Tables(0).Rows(0)("AUTOID")

                v_strSQL = "insert into tllog (autoid, txnum, txdate, brid, tlid, offid, cfrid," _
                                & "chkid, tltxcd, off_line, busdate, txdesc, ipaddress," _
                                & "wsname, status, batchname, micode, deleted, msgamt," _
                                & "isparent, parentid, brcode, tlname, offname, cfrname," _
                                & "chkname, txname, status_text, parent_text," _
                                & "tmpid, childtltxcd, t_no, msgdate, col_value01, col_type01, col_desc01, col_value02, col_type02, col_desc02" _
                                & ", col_value04, col_type04, col_desc04," _
                                & "vsd_brid)"
                v_strSQL = v_strSQL & " select " & v_strAUTOID & ", GENERATE_TLLOG_CODE(" & v_strAUTOID & "), to_date('" & v_strTXDATE & "','DD/MM/YYYY')," _
                            & "'" & v_strBRID & "', '" & v_strTLID & "','" & v_strOFFID & "','" & v_strCFRID & "','" & v_strCHKID & "','" & v_TLTXCD & "',null, null, '" & v_strTXDESC & "','" _
                            & v_strIPADDRESS & "','" & v_strWSNAME & "','" & v_strSTATUS & "', null, '" & v_strMICODE & "',0," & v_QttySCode.ToString & ",2,0,'" & v_strBRCODE & "','" _
                            & v_strTLNAME & "','" & v_strOFFNAME & "','" & v_strCFRNAME & "','" & v_strCHKNAME & "','" & v_TltxName & "','" & v_strSTATUS_TEXT & "','Giao dịch lẻ'," _
                            & "null,'" & v_CHILDTLTXCD & "',null,null,'3079','C','3079','" _
                            & v_strTXDATE & "','D','" & v_strTXDATE & "','" _
                            & v_strMICODE & "','C','" & v_strMICODE & "','" _
                            & v_strVsdBrid & "' from dual"
                v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                'End
                Dim v_strPaAutoid As String = ""
                Dim v_strPaTXNUM As String = ""
                Dim v_intNumCount As Integer = 0
                Dim v_strDeleted_log As Integer = 0
                Dim v_strStatus_log As Integer = 0
                v_intWarningCount = 0
                v_intErrCount = 0
                For i = 0 To v_nodeList.Count - 1
                    'Tao giao dich con 
                    If v_TLTXCD = "3197" Then
                        v_strSbvSicode = v_nodeList.Item(i).ChildNodes(0).InnerText.ToString
                        v_strSBVMiCode = v_nodeList.Item(i).ChildNodes(1).InnerText.ToString
                        v_strQuantity = v_nodeList.Item(i).ChildNodes(2).InnerText.ToString
                        v_strNum = v_nodeList.Item(i).ChildNodes(3).InnerText.ToString
                    ElseIf v_TLTXCD = "3186" Then
                        v_strSbvSicode = v_nodeList.Item(i).ChildNodes(1).InnerText.ToString
                        v_strSBVMiCode = v_nodeList.Item(i).ChildNodes(2).InnerText.ToString
                        v_strSBVComicode = v_nodeList.Item(i).ChildNodes(3).InnerText.ToString
                        v_strQuantity = v_nodeList.Item(i).ChildNodes(4).InnerText.ToString
                        v_TransferType = v_nodeList.Item(i).ChildNodes(5).InnerText.ToString
                        v_strCODE = v_nodeList.Item(i).ChildNodes(6).InnerText.ToString
                        v_strNum = v_nodeList.Item(i).ChildNodes(7).InnerText.ToString
                        If v_strSBVMiCode = v_strSBVComicode Then
                            v_intErrCount = v_intErrCount + 1
                            v_strErrMsg = v_strErrMsg & "Dòng " & i + 1 & ": Thành viên lưu ký bên chuyển (" & v_strSBVMiCode & ") trùng với Thành viên lưu ký bên nhận (" & v_strSBVComicode & ") ." & vbCrLf
                            'v_obj.Rollback()
                            'Return False
                            'Exit Function
                        End If
                    End If
                    'Kiem tra GD da duoc nhan trong lan gui truoc hay chua
                    v_strSQL = "select * from cds_tranlog_vsd where deleted = 0 and status = 0 and num_operation = '" & v_strNum & "' and trans_type = '" & v_Type & "'"
                    v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                    If v_ds.Tables(0).Rows.Count > 0 Then
                        v_intNumCount = v_intNumCount + 1
                    Else
                        checkResult = Convert.ToDouble(v_strQuantity) Mod v_ParamInt
                        If checkResult > 0 Then
                            v_intErrCount = v_intErrCount + 1
                            v_strErrMsg = v_strErrMsg & "Dòng " & i + 1 & ": Số lượng (" & v_strQuantity & ") không chia hết cho (" & v_ParamInt & ")." & vbCrLf
                        End If
                        'Added by Thanglv9 - 18/01/2013
                        'Kiem tra Sicode ton tai
                        v_strSQL = "select distinct sicode from rgsi a where a.sbv_sicode = '" & v_strSbvSicode & "' and a.brid = '" & v_strBRID & "' and a.deleted = 0 and a.status in (0,4) "
                        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                        If v_ds.Tables(0).Rows.Count = 0 Then
                            v_intErrCount = v_intErrCount + 1
                            v_strErrMsg = v_strErrMsg & "Dòng " & i + 1 & ": Không tồn tại Mã GTCG :" & v_strSbvSicode & "." & vbCrLf
                            'v_obj.Rollback()
                            'Return False
                            'Exit Function
                        Else
                            v_strSICODE = v_ds.Tables(0).Rows(0)("SICODE")
                            'Lay Menh Gia cua Chung Khoan
                            v_strSQL = "select a.part_value from rgsi a where a.sicode = '" & v_strSICODE & "' and a.deleted = 0 and a.status in (0,4)"
                            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                            v_PartValue = v_ds.Tables(0).Rows(0)("part_value")
                            'Tinh SL Chung khoan
                            v_QttySCode = Convert.ToDouble(v_strQuantity) / v_PartValue
                            'End

                            'Kiem tra CK da den ngay dao han hay chua
                            'Da dao han cap nhat vao tllog voi deleted = 1
                            'Thanglv9 06/05/2014
                            v_strSQLTmp = "select distinct sicode from rgsi a where a.sbv_sicode = '" & v_strSbvSicode & "' and a.brid = '" & v_strBRID & "' and a.deleted = 0 and a.status in (0,4) and due_date <= to_date('" & v_strTXDATE & "','DD/MM/YYYY')"
                            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                            If v_ds.Tables(0).Rows.Count > 0 Then
                                v_strDeleted_log = 1
                                'v_strSTATUS = 4
                            Else
                                v_strDeleted_log = 0
                            End If
                        End If

                        ' Sinh giao dich con
                        'Lay thong tin nha dau tu ben chuyen 
                        v_strSQLTmp = "select a.tradeacctno,a.iicode,b.full_name , b.cardno ,to_char(b.carddate,'DD/MM/YYYY') carddate,b.cardtype      from rgiiia a, rgii b " _
                                    & "where a.micode ='501' and a.sbv_micode ='" & v_strSBVMiCode & "' and a.iicode = b.iicode and a.status =0 and a.deleted =0"
                        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                        'Dim v_strTradeacctno As String = ""
                        Dim v_strFull_name As String = ""
                        Dim v_strCardNo As String = ""
                        Dim v_strCardDate As String = ""
                        Dim v_strCardType As String = ""
                        Dim v_strIICODE As String = ""

                        If v_ds.Tables(0).Rows.Count > 0 Then
                            v_strTradeacctno = v_ds.Tables(0).Rows(0)("TRADEACCTNO")
                            v_strFull_name = v_ds.Tables(0).Rows(0)("FULL_NAME")
                            v_strCardNo = v_ds.Tables(0).Rows(0)("CARDNO")
                            v_strCardDate = v_ds.Tables(0).Rows(0)("CARDDATE")
                            v_strCardType = v_ds.Tables(0).Rows(0)("CARDTYPE")
                            v_strIICODE = v_ds.Tables(0).Rows(0)("IICODE")
                        Else
                            v_intErrCount = v_intErrCount + 1
                            v_strErrMsg = v_strErrMsg & "Dòng " & i + 1 & ": Không có NĐT nào có mã thành viên tại sở là :" & v_strSBVMiCode & "." & vbCrLf
                            'v_obj.Rollback()
                            'Return False
                            'Exit Function
                        End If

                        'Edited by Thanglv9 - 19/03/2013
                        'Lay autoid 
                        v_strSQL = "select seq_tllog.NEXTVAL AUTOID,GENERATE_TLLOG_CODE(seq_tllog.currval) TXNUM from dual"
                        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                        v_strPaAutoid = v_ds.Tables(0).Rows(0)("AUTOID")
                        v_strPaTXNUM = v_ds.Tables(0).Rows(0)("TXNUM")
                        'End
                        'Lay thong tin NDT ben nhan
                        If v_TLTXCD = "3197" Then
                            v_strSQLTmp = "select a.micode,a.tradeacctno,a.iicode,b.full_name , b.cardno ,to_char(b.carddate,'DD/MM/YYYY') carddate,b.cardtype      from rgiiia a, rgii b " _
                                        & "where a.micode <>'501' and a.sbv_micode ='" & v_strSBVMiCode & "' and a.iicode = b.iicode and a.status =0 and a.deleted =0"
                        ElseIf v_TLTXCD = "3186" Then
                            v_strSQLTmp = "select a.micode,a.tradeacctno,a.iicode,b.full_name , b.cardno ,to_char(b.carddate,'DD/MM/YYYY') carddate,b.cardtype      from rgiiia a, rgii b " _
                                        & "where a.micode ='501' and a.sbv_micode ='" & v_strSBVComicode & "' and a.iicode = b.iicode and a.status =0 and a.deleted =0"
                        End If
                        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                        Dim v_strTradeacctno1 As String = ""
                        Dim v_strFull_name1 As String = ""
                        Dim v_strCardNo1 As String = ""
                        Dim v_strCardDate1 As String = ""
                        Dim v_strCardType1 As String = ""
                        If v_ds.Tables(0).Rows.Count > 0 Then
                            If v_ds.Tables(0).Rows.Count > 1 Then
                                v_strTradeacctno1 = ""
                                v_strFull_name1 = v_ds.Tables(0).Rows(0)("FULL_NAME")
                                v_strCardNo1 = v_ds.Tables(0).Rows(0)("CARDNO")
                                v_strCardDate1 = v_ds.Tables(0).Rows(0)("CARDDATE")
                                v_strCardType1 = v_ds.Tables(0).Rows(0)("CARDTYPE")
                                v_strCOMICODE = ""
                                v_intWarningCount = v_intWarningCount + 1
                                v_strWarning = v_strWarning & "Dòng " & v_intWarningCount & ": (Số chứng từ :" & v_strPaTXNUM & ") - Thành viên có mã :" & v_strSBVMiCode & " có nhiều tài khoản tại nhiều TVLK!" & vbCrLf
                            Else
                                v_strTradeacctno1 = v_ds.Tables(0).Rows(0)("TRADEACCTNO")
                                v_strFull_name1 = v_ds.Tables(0).Rows(0)("FULL_NAME")
                                v_strCardNo1 = v_ds.Tables(0).Rows(0)("CARDNO")
                                v_strCardDate1 = v_ds.Tables(0).Rows(0)("CARDDATE")
                                v_strCardType1 = v_ds.Tables(0).Rows(0)("CARDTYPE")
                                v_strCOMICODE = v_ds.Tables(0).Rows(0)("MICODE")
                            End If
                        Else
                            If v_TLTXCD = "3197" Then
                                v_strTradeacctno1 = ""
                                v_strFull_name1 = ""
                                v_strCardNo1 = ""
                                v_strCardDate1 = ""
                                v_strCardType1 = ""
                                v_strCOMICODE = ""
                                v_intWarningCount = v_intWarningCount + 1
                                v_strWarning = v_strWarning & "Dòng " & v_intWarningCount & ": (Số chứng từ :" & v_strPaTXNUM & ") - Thành viên có mã :" & v_strSBVMiCode & " chưa đăng ký tài khoản tại TVLK nào!" & vbCrLf
                            ElseIf v_TLTXCD = "3186" Then
                                v_intErrCount = v_intErrCount + 1
                                v_strErrMsg = v_strErrMsg & "Dòng " & i + 1 & ": Không có NĐT nào có mã thành viên tại sở là :" & v_strSBVComicode & "." & vbCrLf
                                'v_obj.Rollback()
                                'Return False
                                'Exit Function
                            End If
                        End If

                        v_strSQL = "insert into tllog (autoid, txnum, txdate, brid, tlid, offid, cfrid," _
                                        & "chkid, tltxcd, off_line, busdate, txdesc, ipaddress," _
                                        & "wsname, status, batchname, micode, deleted, msgamt," _
                                        & "isparent, parentid, brcode, tlname, offname, cfrname," _
                                        & "chkname, txname, status_text, parent_text, sicode," _
                                        & "tmpid, childtltxcd, t_no, msgdate, col_value01, col_type01, col_desc01, col_value02, col_type02, col_desc02" _
                                        & ", col_value03, col_type03, col_desc03, col_value04, col_type04, col_desc04, col_value05, col_type05, col_desc05," _
                                        & "col_value06, col_type06, col_desc06, col_value07, col_type07, col_desc07, col_value08, col_type08, col_desc08, " _
                                        & "col_value09, col_type09, col_desc09,col_value13, col_type13, col_desc13,col_value14, col_type14, col_desc14 " _
                                        & ",col_value15, col_type15, col_desc15,col_value16, col_type16, col_desc16,col_value17, col_type17, col_desc17 " _
                                        & ",col_value18, col_type18, col_desc18,col_value23, col_type23, col_desc23,col_value21,col_type21,col_desc21 " _
                                        & ", col_value22,col_type22,col_desc22, vsd_brid,comicode,col_value30,col_type30,col_desc30 ,col_value31,col_type31,col_desc31 ,col_value29,col_type29,col_desc29 ,col_value32,col_type32,col_desc32)"
                        v_strSQL = v_strSQL & " select " & v_strPaAutoid & ",'" & v_strPaTXNUM & "', to_date('" & v_strTXDATE & "','DD/MM/YYYY')," _
                                    & "'" & v_strBRID & "', '" & v_strTLID & "','" & v_strOFFID & "','" & v_strCFRID & "','" & v_strCHKID & "','" & v_CHILDTLTXCD & "',null, null, '" & v_strTXDESC & "','" _
                                    & v_strIPADDRESS & "','" & v_strWSNAME & "','" & v_strSTATUS & "', null, '" & v_strMICODE & "'," & v_strDeleted_log & "," & v_QttySCode.ToString & ",0," & v_strAUTOID & ",'" & v_strBRCODE & "','" _
                                    & v_strTLNAME & "','" & v_strOFFNAME & "','" & v_strCFRNAME & "','" & v_strCHKNAME & "','" & v_ChildtxName & "','" & v_strSTATUS_TEXT & "','Giao dịch con'," _
                                    & "'" & v_strSICODE & "',null,null,null,null,GENERATE_TLLOG_CODE(" & v_strAUTOID & ") col_value01,'C',GENERATE_TLLOG_CODE(" & v_strAUTOID & ") col_desc01,'" _
                                    & v_strTXDATE & "','D','" & v_strTXDATE & "',null,'D',NULL," _
                                    & "'1','N','1 - CK phổ thông','" & v_strTradeacctno & "','C','" & v_strTradeacctno & "','" _
                                    & v_strFull_name & "','C','" & v_strFull_name & "','" & v_strCardType & "', 'N','" & v_strCardType & "','" & v_strCardNo & "','C','" & v_strCardNo & "','" _
                                & v_strCardDate & "','D','" & v_strCardDate & "','" & v_strTradeacctno1 & "','C','" & v_strTradeacctno1 & "','" & v_strFull_name1 & "','C','" _
                                & v_strFull_name1 & "','" & v_strCardType1 & "','N','" _
                                & v_strCardType1 & "','" & v_strCardNo1 & "','C','" & v_strCardNo1 & "','" & v_strCardDate1 & "','D','" & v_strCardDate1 & "','" _
                                & v_QttySCode & "','N','" & v_QttySCode & "','3079','C','3079','" & v_strSICODE & "','C','" & v_strSICODE & "','" _
                                & v_strCOMICODE & "','C','" & v_strCOMICODE & "','" & v_strVsdBrid & "','" & v_strCOMICODE & "','" _
                                & v_strNum & "','N','" & v_strNum & "','" _
                                & v_TransferType & "','C','" & v_TransferType & "','" _
                                & v_TransferType & "','C','" & v_TransferType & "','" _
                                & v_strCODE & "','N','" & v_strCODE _
                                & "' from dual"
                        v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                    End If
                Next
                If v_intNumCount = v_nodeList.Count Then
                    v_intErrCount = v_intErrCount + 1
                    v_strErrMsg = "Tất cả các giao dịch trong File được tồn tại và được xử lý!"
                    v_obj.Rollback()
                    Return False
                    Exit Function
                Else
                    If v_intErrCount > 0 Then
                        v_obj.Rollback()
                        Return False
                        Exit Function
                    End If
                End If
            End With
            v_obj.Commit()
            v_intErrCount = 1
            v_strErrMsg = "Giao dịch thành công"


        Catch ex As Exception
            v_strErrMsg = "File dữ liệu không hợp lệ!. Chi tiết: " & ex.Message
            v_intErrCount += 1
            v_obj.Rollback()
            Return False
        Finally
            GC.Collect()
        End Try
    End Function
    '1113
    Private Function Host_check_file_1113(ByVal pv_oNode As Xml.XmlNode, ByVal v_strXMLData As String, ByRef v_strErrMsg As String, _
                                     ByRef v_intErrCount As Integer) As Boolean
        Dim blnTran As Boolean = False
        'Dim v_trace As DataSet
        ' Dim v_strMFNO As String
        Dim v_ds As DataSet
        Dim v_blnStockType As Boolean = False
        Dim v_strAUTOID As String
        Dim v_strSTATUS As String = "0"
        Dim v_strTXNUM As String
        Dim v_strTXDATE As String
        Dim v_lngErr As Long = ERR_SYSTEM_OK
        Dim v_obj As New DataAccess
        'Dim tr2 As TextWriterTraceListener
        Try
            'Get config for connecting BKAV LDAP
            With pv_oNode
                v_obj.BeginTran()
                blnTran = True
                'Get message header
                Dim v_strBRID As String = .Attributes(gc_AtributeBRID).Value.ToString
                v_strSTATUS = .Attributes(gc_AtributeSTATUS).Value.ToString
                'bangpv: voi CA
                Dim v_strCAStatus As String = v_strSTATUS
                Dim v_strSTATUS_TEXT As String = .Attributes(gc_AtributeSTATUSTEXT).Value.ToString
                v_strAUTOID = .Attributes(gc_AtributeAUTOID).Value.ToString
                v_strTXNUM = .Attributes(gc_AtributeTXNUM).Value.ToString
                v_strTXDATE = .Attributes(gc_AtributeTXDATE).Value.ToString

                Dim v_strSQL As String
                'v_strSQL = "SELECT A.COL_VALUE01 FROM TLLOG A" _
                '& " WHERE A.TXNUM='" & v_strTXNUM & "'"
                'v_strSQL = Split(pv_oNode.InnerText.ToLower.ToString, "Select").ToString
                'v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                'Wtite Log
                'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "Host", "txRouter", "ExcuteTran", mv_strIpAddress, mv_strWsName, mv_strTellerId, mv_strTellerName, "Excute SQL command: " & v_strSQL)

                'If v_ds.Tables(0).Rows.Count > 0 Then
                Dim v_strBranchId1 As String = v_strBRID
                v_strSQL = "SELECT varvalue COL_VALUE01 from SYSVAR where varname ='CURRDATE' and brid =" + v_strBranchId1
                v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                Dim v_strBridCurrdate1 As String = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE01"))
                v_strBridCurrdate1 = v_strBridCurrdate1.Replace("/", "")
                'v_lngErr = ExtractFile(v_strBranchId1, v_strBridCurrdate1)
                v_strSQL = "SELECT varvalue COL_VALUE01 FROM sysvar a where a.GRNAME like 'VSDFTPSVR' and varname = 'RootPath' and brid='" + v_strBranchId1 + "'"
                v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                Dim v_strRootPath As String = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE01"))

                v_strSQL = "SELECT varvalue COL_VALUE01 FROM sysvar a where a.GRNAME like 'VSDFTPSVR' and varname = 'RemotePath' and brid='" + v_strBranchId1 + "'"
                v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                Dim v_strRemotePath As String = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE01"))
                Dim v_lstFileName As List(Of String) = New List(Of String)
                If v_strBranchId1 = "0002" Then
                    v_lngErr = ServerBussinessCA.ExtractFile1113(System.AppDomain.CurrentDomain.BaseDirectory & "tmp\", _
                            v_strRootPath & "\" & v_strRemotePath & "\" & "1113" & v_strBranchId1 & v_strBridCurrdate1 & ".xml", v_strBranchId1, v_lstFileName)
                Else
                    'lấy file từ ftp server về 
                    Dim v_DataAccess As New DataAccess
                    Dim v_strServerAddress, v_strServerAddress1, v_strServerPort, v_strUsername, v_strPassword As String
                    v_DataAccess.GetSysVar("EXPORT", "ServerAddress", v_strBRID, v_strServerAddress)
                    v_DataAccess.GetSysVar("EXPORT", "ServerAddress1", v_strBRID, v_strServerAddress1)
                    v_DataAccess.GetSysVar("EXPORT", "ServerPort", v_strBRID, v_strServerPort)
                    v_DataAccess.GetSysVar("EXPORT", "Username", v_strBRID, v_strUsername)
                    v_DataAccess.GetSysVar("EXPORT", "Password", v_strBRID, v_strPassword)
                    v_DataAccess.GetSysVar("EXPORT", "RemotePath", v_strBRID, v_strRemotePath)
                    v_DataAccess.GetSysVar("VSDFTPSVR", "RootPath", v_strBRID, v_strRootPath)
                    Dim v_oWriter As System.IO.StreamWriter
                    Dim v_strAppPath As String = System.AppDomain.CurrentDomain.BaseDirectory
                    If File.Exists(v_strAppPath & "tmp\" & v_strBRID & "_" & v_strTXDATE & ".bat") Then
                        File.Delete(v_strAppPath & "tmp\" & v_strBRID & "_" & v_strTXDATE & ".bat")
                    End If
                    Dim v_Prefix As String
                    Dim v_strTxdate1 As String = Replace(v_strTXDATE, "/", "")
                    Select Case v_strBRID
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
                    End Select

                    v_oWriter = New StreamWriter(v_strAppPath & "tmp\" & v_strBRID & "_" & v_strTxdate1 & ".bat")
                    v_oWriter.WriteLine("@ftp -i -s:""%~f0""&GOTO:EOF")
                    v_oWriter.WriteLine("open " & v_strServerAddress)
                    v_oWriter.WriteLine(v_strUsername)
                    v_oWriter.WriteLine(v_strPassword)
                    v_oWriter.WriteLine("lcd " & """" & v_strAppPath & "tmp" & """")
                    v_oWriter.WriteLine("cd " & v_strRemotePath)
                    v_oWriter.WriteLine("binary")

                    v_oWriter.WriteLine("get " & v_Prefix & "_TRADING_RESULT" & v_strTxdate1 & ".zip " & v_Prefix & "_TRADING_RESULT" & v_strTxdate1 & ".zip")

                    v_oWriter.WriteLine("bye" & vbCrLf)
                    v_oWriter.Close()
                    Dim v_oProcess As Process
                    v_oProcess = New Process

                    v_oProcess.StartInfo.FileName = v_strAppPath & "tmp\" & v_strBRID & "_" & v_strTxdate1 & ".bat" 'v_strAppPath & "tmp\" & v_strBrid & "_" & v_strTxdate & ".bat"
                    v_oProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
                    v_oProcess.StartInfo.CreateNoWindow = True
                    v_oProcess.Start()
                    'v_oProcess.WaitForExit()
                    v_oProcess.Close()
                    ' Lấy từ đầu 2 
                    v_oWriter = New StreamWriter(v_strAppPath & "tmp\" & v_strBRID & "_" & v_strTxdate1 & "1.bat")
                    v_oWriter.WriteLine("@ftp -i -s:""%~f0""&GOTO:EOF")
                    v_oWriter.WriteLine("open " & v_strServerAddress1)
                    v_oWriter.WriteLine(v_strUsername)
                    v_oWriter.WriteLine(v_strPassword)
                    v_oWriter.WriteLine("lcd " & """" & v_strAppPath & "tmp" & """")
                    v_oWriter.WriteLine("cd " & v_strRemotePath)
                    v_oWriter.WriteLine("binary")

                    v_oWriter.WriteLine("get " & v_Prefix & "_TRADING_RESULT" & v_strTxdate1 & ".zip " & v_Prefix & "_TRADING_RESULT" & v_strTxdate1 & ".zip")

                    v_oWriter.WriteLine("bye" & vbCrLf)
                    v_oWriter.Close()
                    v_oProcess = New Process

                    v_oProcess.StartInfo.FileName = v_strAppPath & "tmp\" & v_strBRID & "_" & v_strTxdate1 & "1.bat" 'v_strAppPath & "tmp\" & v_strBrid & "_" & v_strTxdate & ".bat"
                    v_oProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
                    v_oProcess.StartInfo.CreateNoWindow = True
                    v_oProcess.Start()
                    'v_oProcess.WaitForExit()
                    v_oProcess.Close()
                    System.Threading.Thread.Sleep(30 * 1000)

                    'If Not ServerBussinessCA.DecryptTrade_HNX(v_strAppPath & "tmp\" & v_Prefix & "_TRADING_RESULT" & v_strTxdate1, v_Prefix & "_TRADING_RESULT" & v_strTxdate1) Then
                    'Return False
                    'Exit Function
                    'End If
                    If Not File.Exists(v_strAppPath & "tmp\" & v_Prefix & "_TRADING_RESULT" & v_strTxdate1 & ".zip") Then
                        v_intErrCount = 1
                        v_strErrMsg = "Chưa có file kết quả giao dịch"
                        'Exit Function
                    Else
                        v_intErrCount = 0
                        File.Delete(v_strAppPath & "tmp\" & v_Prefix & "_TRADING_RESULT" & v_strTxdate1 & ".zip")
                        Return True
                    End If
                End If
                'End If
            End With
            'v_intErrCount = 0
            'Return True
            'v_strErrMsg = "Giao dịch thành công"
        Catch ex As Exception
            v_strErrMsg = "File dữ liệu không hợp lệ!. Chi tiết: " & ex.Message
            'v_strErrMsg = ex.Message
            v_intErrCount += 1
            v_obj.Rollback()
            Return False
        Finally
            GC.Collect()
        End Try
    End Function
    Private Function HostCheckftpfromHNX_2130(ByVal pv_oNode As Xml.XmlNode, ByVal v_strXMLData As String, ByRef v_strErrMsg As String, ByRef v_intErrCount As Integer) As Boolean
        Dim blnTran As Boolean = False
        'Dim v_trace As DataSet
        ' Dim v_strMFNO As String
        Dim pv_strGrName As String = "", pv_strBrid As String = "", pv_strFileName As String = "", pv_strTradeDate As String = ""
        Dim v_ds As DataSet
        Dim v_strServerAddress, v_strServerPort, v_strUsername, v_strPassword, v_strRemotePath As String
        Dim v_strSql As String = ""
        Dim v_lngErr As Long = ERR_SYSTEM_OK
        Dim v_obj As New DataAccess
        Dim v_strSQLTEMP As String = ""
        Dim v_strSTATUS = pv_oNode.Attributes(gc_AtributeSTATUS).Value.ToString()
        Dim v_strTXNUM = pv_oNode.Attributes(gc_AtributeTXNUM).Value.ToString()
        Dim v_nodeList As Xml.XmlNodeList
        Dim v_dsServer As DataSet
        Dim v_strSqlServer As String = ""
        'Dim v_ds As DataSet
        'Dim tr2 As TextWriterTraceListener
        Try
            'Get config for connecting BKAV LDAP
            With pv_oNode
                v_obj.BeginTran()
                blnTran = True
                If v_strSTATUS = "0" Then
                    v_nodeList = pv_oNode.SelectNodes("/TransactMessage/fields")
                    For i = 0 To v_nodeList.Count - 1
                        For j = 0 To v_nodeList.Item(i).ChildNodes.Count - 1
                            With v_nodeList.Item(i).ChildNodes(j)
                                v_strSQLTEMP = .InnerText

                                v_strSQLTEMP = Replace(v_strSQLTEMP, "TT_", "")
                                v_strSQLTEMP = Replace(v_strSQLTEMP, "tt_", "")
                                v_strSQLTEMP = Replace(v_strSQLTEMP, "tT_", "")
                                v_strSQLTEMP = Replace(v_strSQLTEMP, "Tt_", "")

                                v_obj.ExecuteNonQuery(CommandType.Text, v_strSQLTEMP)
                            End With
                        Next
                    Next
                    v_strSql = "SELECT a.col_value01, a.col_value02, a.col_value03 FROM TMP_TXFIELDS A"
                    v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSql)
                    v_strSQLTEMP = "delete TMP_TXFIELDS"
                    v_obj.ExecuteNonQuery(CommandType.Text, v_strSQLTEMP)
                Else
                    v_strSql = "SELECT a.col_value01, a.col_value02, a.col_value03 FROM tllog A where txnum ='" & v_strTXNUM & "'"
                    v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSql)
                End If
                'End If
                If v_ds.Tables(0).Rows.Count > 0 Then
                    pv_strTradeDate = DateTime.ParseExact(gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE01")).Replace("/", ""), "ddMMyyyy", Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd", Globalization.CultureInfo.InvariantCulture)
                    pv_strBrid = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE02"))
                    Dim v_str2130Type As String = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("COL_VALUE03"))
                    If pv_strBrid <> "0002" And InStr("5,6,7,8", v_str2130Type) > 0 Then
                        Select Case v_str2130Type
                            Case "5"
                                pv_strGrName = "HNX_BOND"
                                pv_strFileName = "HNX_BOND_BASKET"
                            Case "6"
                                pv_strGrName = "HNX_BOND"
                                pv_strFileName = "HNX_BOND_INDEX"
                            Case "7"
                                pv_strGrName = "HNX_IDX"
                                pv_strFileName = "HNX_INDEX"
                            Case "8"
                                pv_strGrName = "HNX_BYC"
                                pv_strFileName = "HNX_BOND_YC_DATA"
                        End Select
                        v_strSqlServer = "SELECT max(ServerAddress) ServerAddress, max(ServerPort) ServerPort, " _
                                & " max(Username) Username, max(Password) Password, max(RemotePath) RemotePath " _
                                & " FROM " _
                                & " (SELECT decode(varname, 'ServerAddress', varvalue, '') ServerAddress, " _
                                & " decode(varname, 'ServerPort', varvalue, '') ServerPort, " _
                                & " decode(varname, 'Username', varvalue, '') Username, " _
                                & " decode(varname, 'Password', varvalue, '') Password, " _
                                & " decode(varname, 'RemotePath', varvalue, '') RemotePath " _
                                & " FROM sysvar WHERE grname = '" & pv_strGrName & "' AND brid = '" & pv_strBrid & "')"
                        v_dsServer = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSqlServer)
                        If v_dsServer.Tables(0).Rows.Count > 0 Then
                            v_strServerAddress = v_dsServer.Tables(0).Rows(0)("ServerAddress").ToString()
                            v_strServerPort = v_dsServer.Tables(0).Rows(0)("ServerPort").ToString()
                            v_strUsername = v_dsServer.Tables(0).Rows(0)("Username").ToString()
                            v_strPassword = v_dsServer.Tables(0).Rows(0)("Password").ToString()
                            v_strRemotePath = v_dsServer.Tables(0).Rows(0)("RemotePath").ToString()

                            Dim v_oWriter As System.IO.StreamWriter
                            Dim v_strAppPath As String = System.AppDomain.CurrentDomain.BaseDirectory
                            If File.Exists(v_strAppPath & "tmp\" & pv_strGrName & "_" & pv_strBrid & "_" & pv_strTradeDate & ".bat") Then
                                File.Delete(v_strAppPath & "tmp\" & pv_strGrName & "_" & pv_strBrid & "_" & pv_strTradeDate & ".bat")
                            End If

                            'Lay file ve
                            v_oWriter = New StreamWriter(v_strAppPath & "tmp\" & pv_strGrName & "_" & pv_strBrid & "_PRICE_" & pv_strTradeDate & ".bat")
                            v_oWriter.WriteLine("@ftp -i -s:""%~f0""&GOTO:EOF")
                            v_oWriter.WriteLine("open " & v_strServerAddress)
                            v_oWriter.WriteLine(v_strUsername)
                            v_oWriter.WriteLine(v_strPassword)
                            v_oWriter.WriteLine("lcd " & """" & v_strAppPath & "tmp" & """")
                            v_oWriter.WriteLine("cd " & v_strRemotePath)
                            v_oWriter.WriteLine("binary")
                            v_oWriter.WriteLine("get " & pv_strFileName & "_" & pv_strTradeDate & ".zip " & pv_strFileName & "_" & pv_strTradeDate & ".zip")
                            v_oWriter.WriteLine("bye" & vbCrLf)
                            v_oWriter.Close()

                            Dim v_oProcess As New Process
                            v_oProcess.StartInfo.FileName = v_strAppPath & "tmp\" & pv_strGrName & "_" & pv_strBrid & "_PRICE_" & pv_strTradeDate & ".bat"
                            v_oProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
                            v_oProcess.StartInfo.CreateNoWindow = True
                            v_oProcess.Start()
                            v_oProcess.WaitForExit(10 * 1000)
                            v_oProcess.Close()
                            If Not File.Exists(v_strAppPath & "tmp\" & pv_strFileName & "_" & pv_strTradeDate & ".zip ") Then
                                v_intErrCount = 1
                                v_strErrMsg = "Chưa có file dữ liệu từ HNX"
                                'Exit Function
                            Else
                                v_intErrCount = 0
                                File.Delete(v_strAppPath & "tmp\" & pv_strFileName & "_" & pv_strTradeDate & ".zip ")
                                Return True
                            End If
                        Else
                            v_intErrCount = 1
                            v_strErrMsg = "Không có thông tin FTP server"
                        End If
                    Else
                        v_intErrCount = 0
                        Return True
                    End If
                Else
                    v_intErrCount = 1
                    v_strErrMsg = "Không có thông tin giao dịch"
                End If
            End With
            'v_intErrCount = 0
            'Return True
            'v_strErrMsg = "Giao dịch thành công"
        Catch ex As Exception
            'v_strErrMsg = "Không lấy được file!. Chi tiết: " & ex.Message
            'v_strErrMsg = ex.Message
            v_strErrMsg = "Giao dịch không thành công !. Chi tiết: " & ex.Message
            v_intErrCount += 1
            v_obj.Rollback()
            Return False
        Finally
            GC.Collect()
        End Try
    End Function

    Private Function HostCheckConfirmRPT(ByVal v_strFileNameCA As String, ByVal v_strSignatureCA As String, ByRef v_strErrMsg As String, _
                                      ByRef v_intErrCount As Integer, ByVal v_strTLName As String, ByVal v_strTLID As String, ByVal mv_oSignServer As SignServer) As Boolean
        Try
            'Get config for connecting BKAV LDAP
            'Dim v_abc As BkavCASign.CertificateServer
            v_intErrCount = 0
            'Dim v_intCHKSrv As Integer = 0
            Dim v_dataAccess As New DataAccess
            Dim v_strVSDUsername As String = _
                        System.Configuration.ConfigurationManager.AppSettings("VSDUsername").ToString
            Dim v_strBKAVPassword As String = _
                        System.Configuration.ConfigurationManager.AppSettings("BKAVPassword").ToString
            Dim v_strLDAPIP As String = _
                        System.Configuration.ConfigurationManager.AppSettings("LDAPIP").ToString
            Dim v_strArrayOgarnization As String = _
                        System.Configuration.ConfigurationManager.AppSettings("ArrayOgarnization").ToString
            Dim v_strBKAVDomainComponent As String = _
                System.Configuration.ConfigurationManager.AppSettings("BKAVDomainComponent").ToString

            'Tách Đường dẫn từ client lên để lấy file trên server
            Dim v_arrFileName() As String
            v_arrFileName = v_strFileNameCA.Split("\")
            Dim i As Integer = v_arrFileName.Length
            v_strFileNameCA = v_arrFileName(i - 1)

            v_arrFileName = v_strFileNameCA.Split("'")
            Dim v_strDateCreated As String = v_arrFileName(3)
            'Dim v_strTLLOGID As String = v_arrFileName(1)
            'Dim v_strTLName1 As String = v_arrFileName(0)p
            'v_DataAccess.GetSysVar("CA", "APP_LOCATION", v_strAppPath)
            Dim v_strAppPath As String = System.AppDomain.CurrentDomain.BaseDirectory
            'Them lay file tu FTP Server về

            Dim v_strLocalDir = v_strAppPath & "\Log\" & v_strDateCreated
            ' Đẩy file lên FTP server
            Dim v_strServerAddress, v_strServerPort, v_strUsername, v_strPassword, v_strRemotePath, v_strRootPath As String
            v_dataAccess.GetSysVar("FILEFTPSVR", "ServerAddress", "0001", v_strServerAddress)
            v_dataAccess.GetSysVar("FILEFTPSVR", "ServerPort", "0001", v_strServerPort)
            v_dataAccess.GetSysVar("FILEFTPSVR", "Username", "0001", v_strUsername)
            v_dataAccess.GetSysVar("FILEFTPSVR", "Password", "0001", v_strPassword)
            v_dataAccess.GetSysVar("FILEFTPSVR", "RemotePath", "0001", v_strRemotePath)
            v_dataAccess.GetSysVar("FILEFTPSVR", "RootPath", "0001", v_strRootPath)

            Dim v_oWriter As StreamWriter
            v_oWriter = New StreamWriter(v_strLocalDir & "\" & Replace(v_strFileNameCA, ".dat", ".bat"))
            v_oWriter.WriteLine("@ftp -i -s:""%~f0""&GOTO:EOF")
            v_oWriter.WriteLine("open " & v_strServerAddress)
            v_oWriter.WriteLine(v_strUsername)
            v_oWriter.WriteLine(v_strPassword)
            v_oWriter.WriteLine("lcd " & """" & v_strLocalDir & """")
            v_oWriter.WriteLine("cd " & v_strRemotePath)
            v_oWriter.WriteLine("binary")
            v_oWriter.WriteLine("get " & v_strFileNameCA & " " & v_strFileNameCA)
            v_oWriter.WriteLine("bye" & vbCrLf)

            v_oWriter.Close()

            Dim v_oProcess As Process
            v_oProcess = New Process

            v_oProcess.StartInfo.FileName = v_strLocalDir & "\" & Replace(v_strFileNameCA, ".dat", ".bat")
            v_oProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
            v_oProcess.StartInfo.CreateNoWindow = True
            v_oProcess.Start()
            'v_oProcess.WaitForExit()
            v_oProcess.Close()
            System.Threading.Thread.Sleep(3 * 1000)
            'end Them lay file tu FTP Server về
            ' v_dataAccess.GetSysVar("CA", "APP_LOCATION", v_strAppPath)
            Dim v_strServerFile As String = v_strAppPath & "\Log\" & v_strDateCreated & "\" & v_strFileNameCA

            If Not File.Exists(v_strServerFile) Then
                v_intErrCount = 1
                v_strErrMsg = "File dữ liệu ở client và server không khớp"
                Exit Function
            End If

            Dim v_Stream As New System.IO.StreamReader(v_strServerFile)
            Dim v_strData As String = v_Stream.ReadToEnd
            Dim v_strSQL As String

            If ServerBussinessCA.Check_Signature_client(v_strData, v_strSignatureCA, v_strVSDUsername, v_strBKAVPassword, _
                                                                         v_strLDAPIP, v_strArrayOgarnization, v_strBKAVDomainComponent, v_strTLName) Then


                v_intErrCount = 0
                v_strErrMsg = ""
                Return True
                'verify với dữ liệu lưu ở file 
            Else
                'thông báo kiểm tra dữ liệu không thành công 
                v_intErrCount = 1
                v_strErrMsg = "File dữ liệu ở client và server không khớp"
                Return False
            End If
            'If blnChk = True Then
            '    Return True

            'Else
            '    Return False
            'End If



        Catch ex As Exception
            v_strErrMsg = "File dữ liệu không hợp lệ!. Chi tiết: " & ex.Message
            v_intErrCount += 1
            Return False
        End Try
    End Function

    Private Function HostCheckTransCA(ByVal v_strFileNameCA As String, ByVal v_strSignatureCA As String, ByRef v_strErrMsg As String, _
                                          ByRef v_intErrCount As Integer, ByVal v_strTLName As String, ByVal v_strTLID As String, ByVal mv_oSignServer As SignServer) As Boolean
        Dim v_lngErrCode As Long = ERR_SYSTEM_OK
        Dim v_strErrorSource As String = "Host.txRouter.Transact", v_strErrorMessage As String = ""
        'Dim v_nodeList As Xml.XmlNodeList
        'Dim intIndex As Integer
        Dim v_obj As New DataAccess
        'Dim v_strCurrDate As String
        Try


            'Get config for connecting BKAV LDAP
            'Dim v_abc As BkavCASign.CertificateServer
            v_intErrCount = 0
            'Dim v_intCHKSrv As Integer = 0
            Dim v_strVSDUsername As String = _
                        System.Configuration.ConfigurationManager.AppSettings("VSDUsername").ToString
            Dim v_strBKAVPassword As String = _
                        System.Configuration.ConfigurationManager.AppSettings("BKAVPassword").ToString
            Dim v_strLDAPIP As String = _
                        System.Configuration.ConfigurationManager.AppSettings("LDAPIP").ToString
            Dim v_strArrayOgarnization As String = _
                        System.Configuration.ConfigurationManager.AppSettings("ArrayOgarnization").ToString
            Dim v_strBKAVDomainComponent As String = _
                System.Configuration.ConfigurationManager.AppSettings("BKAVDomainComponent").ToString

            'Tách Đường dẫn từ client lên để lấy file trên server
            Dim v_arrFileName() As String
            v_arrFileName = v_strFileNameCA.Split("\")
            Dim i As Integer = v_arrFileName.Length
            v_strFileNameCA = v_arrFileName(i - 1)
            'Dim v_strServerFile As String = "C:\Server\" & v_strFileNameCA
            v_arrFileName = v_strFileNameCA.Split("'")
            Dim v_strDateCreated As String = v_arrFileName(3)
            Dim v_strTLLOGID As String = v_arrFileName(1)
            Dim v_strTLName1 As String = v_arrFileName(0)
            Dim v_strAppPath As String = System.AppDomain.CurrentDomain.BaseDirectory

            'Them lay file tu FTP Server về
            Dim v_dataAccess As New DataAccess
            'Them lay file tu FTP Server về

            Dim v_strLocalDir = v_strAppPath & "\Log\" & v_strDateCreated
            ' Đẩy file lên FTP server
            Dim v_strServerAddress, v_strServerPort, v_strUsername, v_strPassword, v_strRemotePath, v_strRootPath As String
            v_dataAccess.GetSysVar("FILEFTPSVR", "ServerAddress", "0001", v_strServerAddress)
            v_dataAccess.GetSysVar("FILEFTPSVR", "ServerPort", "0001", v_strServerPort)
            v_dataAccess.GetSysVar("FILEFTPSVR", "Username", "0001", v_strUsername)
            v_dataAccess.GetSysVar("FILEFTPSVR", "Password", "0001", v_strPassword)
            v_dataAccess.GetSysVar("FILEFTPSVR", "RemotePath", "0001", v_strRemotePath)
            v_dataAccess.GetSysVar("FILEFTPSVR", "RootPath", "0001", v_strRootPath)

            Dim v_oWriter As StreamWriter
            v_oWriter = New StreamWriter(v_strLocalDir & "\" & Replace(v_strFileNameCA, ".dat", ".bat"))
            v_oWriter.WriteLine("@ftp -i -s:""%~f0""&GOTO:EOF")
            v_oWriter.WriteLine("open " & v_strServerAddress)
            v_oWriter.WriteLine(v_strUsername)
            v_oWriter.WriteLine(v_strPassword)
            v_oWriter.WriteLine("lcd " & """" & v_strLocalDir & """")
            v_oWriter.WriteLine("cd " & v_strRemotePath)
            v_oWriter.WriteLine("binary")
            v_oWriter.WriteLine("get " & v_strFileNameCA & " " & v_strFileNameCA)
            v_oWriter.WriteLine("bye" & vbCrLf)

            v_oWriter.Close()

            Dim v_oProcess As Process
            v_oProcess = New Process

            v_oProcess.StartInfo.FileName = v_strLocalDir & "\" & Replace(v_strFileNameCA, ".dat", ".bat")
            v_oProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
            v_oProcess.StartInfo.CreateNoWindow = True
            v_oProcess.Start()
            'v_oProcess.WaitForExit()
            v_oProcess.Close()
            System.Threading.Thread.Sleep(3 * 1000)
            'end Them lay file tu FTP Server về
            'v_obj.GetSysVar("CA", "APP_LOCATION", v_strAppPath) 'System.AppDomain.CurrentDomain.BaseDirectory
            Dim v_strServerFile As String = v_strAppPath & "\Log\" & v_strDateCreated & "\" & v_strFileNameCA

            If Not File.Exists(v_strServerFile) Then
                v_intErrCount = 1
                v_strErrMsg = "File dữ liệu ở client và server không khớp"
                Exit Function
            End If

            Dim v_Stream As New System.IO.StreamReader(v_strServerFile)
            Dim v_strData As String = v_Stream.ReadToEnd
            Dim v_strSQL As String

            If ServerBussinessCA.Check_Signature_client(v_strData, v_strSignatureCA, v_strVSDUsername, v_strBKAVPassword, _
                                                                             v_strLDAPIP, v_strArrayOgarnization, v_strBKAVDomainComponent, v_strTLName) Then

                ' lấy dữ liệu gốc
                'lấy xml gốc của giao dịch
                v_strTLName = v_strTLName1
                'get tlid                 
                Dim v_xmlDocumentMessage As New XmlDocumentEx
                v_xmlDocumentMessage.LoadXml(v_strData)
                Dim v_attrColl As Xml.XmlAttributeCollection = v_xmlDocumentMessage.DocumentElement.Attributes
                Dim v_strDataFileServer As String = CStr(CType(v_attrColl.GetNamedItem("DATA"), Xml.XmlAttribute).Value)
                v_strDataFileServer = ServerBussinessCA.DecryptXML(v_strDataFileServer, v_strTLName)
                'Tach status 
                v_xmlDocumentMessage = New XmlDocumentEx
                v_xmlDocumentMessage.LoadXml(v_strDataFileServer)
                v_attrColl = v_xmlDocumentMessage.DocumentElement.Attributes
                v_strDataFileServer = CStr(CType(v_attrColl.GetNamedItem("DATAXML1"), Xml.XmlAttribute).Value)
                'end tach status
                'Lấy dữ liệu từ trong File để tạo thành Dataset chứa dữ liệu để đối chứng

                'v_obj.Dispose()
                Dim v_dsFileServer As New DataSet
                Dim v_dsData As New DataSet
                'lay du lieu trong database
                v_strSQL = "select autoid, txdate, txnum, brid, tlid, tltxcd, ipaddress, wsname, micode, msgamt, isparent, parentid, brcode, tlname, txname, parent_text, sicode, childtltxcd, msgdate," _
                           & "col_value01, col_value02, col_value03, col_value04, col_value05, col_value06, col_value07, col_value08, col_value09, col_value10," _
                           & "col_value11, col_value12, col_value13, col_value14, col_value15, col_value16, col_value17, col_value18, col_value19, col_value20," _
                           & "col_value21, col_value22, col_value23, col_value24, col_value25, col_value26, col_value27, col_value28, col_value29, col_value30," _
                           & "isbrid, txnote, vsd_brid from tllog where status <=3 and autoid =to_number('" & v_strTLLOGID & "')  union all " _
                           & "select autoid, txdate, txnum, brid, tlid, tltxcd, ipaddress, wsname, micode, msgamt, isparent, parentid, brcode, tlname, txname, parent_text, sicode, childtltxcd, msgdate," _
                           & "col_value01, col_value02, col_value03, col_value04, col_value05, col_value06, col_value07, col_value08, col_value09, col_value10, " _
                           & "col_value11, col_value12, col_value13, col_value14, col_value15, col_value16, col_value17, col_value18, col_value19, col_value20, " _
                           & "col_value21, col_value22, col_value23, col_value24, col_value25, col_value26, col_value27, col_value28, col_value29, col_value30, " _
                           & "isbrid, txnote, vsd_brid from tllog where status <=3 and parentid  =to_number('" & v_strTLLOGID & "')" _
                           & " union all " _
                           & "select autoid, txdate, txnum, brid, tlid, tltxcd, ipaddress, wsname, micode, msgamt, isparent, parentid, brcode, tlname, txname, parent_text, sicode, childtltxcd, msgdate," _
                           & "col_value01, col_value02, col_value03, col_value04, col_value05, col_value06, col_value07, col_value08, col_value09, col_value10," _
                           & "col_value11, col_value12, col_value13, col_value14, col_value15, col_value16, col_value17, col_value18, col_value19, col_value20," _
                           & "col_value21, col_value22, col_value23, col_value24, col_value25, col_value26, col_value27, col_value28, col_value29, col_value30," _
                           & "isbrid, txnote, vsd_brid from passed.tllogall_all where status =3 and autoid =to_number('" & v_strTLLOGID & "')  union all " _
                           & "select autoid, txdate, txnum, brid, tlid, tltxcd, ipaddress, wsname, micode, msgamt, isparent, parentid, brcode, tlname, txname, parent_text, sicode, childtltxcd, msgdate," _
                           & "col_value01, col_value02, col_value03, col_value04, col_value05, col_value06, col_value07, col_value08, col_value09, col_value10, " _
                           & "col_value11, col_value12, col_value13, col_value14, col_value15, col_value16, col_value17, col_value18, col_value19, col_value20, " _
                           & "col_value21, col_value22, col_value23, col_value24, col_value25, col_value26, col_value27, col_value28, col_value29, col_value30, " _
                           & "isbrid, txnote, vsd_brid from passed.tllogall_all where status =3 and parentid  =to_number('" & v_strTLLOGID & "')"
                Dim v_ds As DataSet = v_dataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                v_strData = v_ds.GetXml()
                v_ds = Nothing

                Dim v_reader As New System.IO.StringReader(v_strDataFileServer)
                v_dsFileServer.ReadXml(v_reader)
                Dim v_reader1 As New System.IO.StringReader(v_strData)
                v_dsData.ReadXml(v_reader1)

                Dim v_oDataRow1 = v_dsFileServer.Tables(0).AsEnumerable
                'v_dsFileServer = Nothing
                Dim v_oDataRow2 = v_dsData.Tables(0).AsEnumerable

                Dim v_oResult As IEnumerable(Of DataRow) = v_oDataRow1.Intersect(v_oDataRow2, DataRowComparer.Default)
                Dim v_arrResult As DataRow() = v_oResult.ToArray

                For i = 0 To v_arrResult.Length - 1
                    Dim v_oRow = v_arrResult(i)
                    v_dsFileServer.Tables(0).Rows.Remove(v_oRow)
                Next

                'For Each DataRow In v_oResult
                Dim v_strTMP As String = ""
                'Next
                If v_dsFileServer.Tables(0).Rows.Count >= 1 Then
                    v_strTMP = v_dsFileServer.GetXml
                End If
                'Lấy tên các trường 
                'Ký số với dữ liệu trả về

                'v_strData = ServerBussinessCA.CombineData(v_strData, mv_oSignServer)


                If ServerBussinessCA.Check_VSD_Signature(v_strData, v_strDataFileServer, mv_oSignServer) Or v_strTMP = "" Then
                    v_intErrCount = 1
                    v_strErrMsg = "Dữ liệu hợp lệ"
                Else
                    v_intErrCount = 1
                    v_strErrMsg = "Dữ liệu trong database và dữ liệu lưu ở file server không khớp: " & vbCrLf & v_strTMP

                End If
                'verify với dữ liệu lưu ở file 
            Else
                'thông báo kiểm tra dữ liệu không thành công 
                v_intErrCount = 1
                v_strErrMsg = "File dữ liệu ở client và server không khớp"
            End If
            'If blnChk = True Then
            '    Return True

            'Else
            '    Return False
            'End If



        Catch ex As Exception
            v_strErrMsg = "File dữ liệu không hợp lệ!. Chi tiết: " & ex.Message
            v_intErrCount += 1
            Return False
        End Try
    End Function
    Private Function HostExportTransCA(ByVal v_strFileNameCA As String, ByVal v_strSignatureCA As String, ByRef v_strErrMsg As String, _
                                          ByRef v_intErrCount As Integer, ByVal v_strTLName As String, ByVal v_strTLID As String, ByVal mv_oSignServer As SignServer) As Boolean
        Dim v_lngErrCode As Long = ERR_SYSTEM_OK
        Dim v_strErrorSource As String = "Host.txRouter.Transact", v_strErrorMessage As String = ""
        'Dim v_nodeList As Xml.XmlNodeList
        'Dim intIndex As Integer
        Dim v_obj As New DataAccess
        'Dim v_strCurrDate As String
        Try


            'Get config for connecting BKAV LDAP
            'Dim v_abc As BkavCASign.CertificateServer
            v_intErrCount = 0

            Dim v_dataAccess As New DataAccess
            'Dim v_intCHKSrv As Integer = 0
            Dim v_strVSDUsername As String = _
                        System.Configuration.ConfigurationManager.AppSettings("VSDUsername").ToString
            Dim v_strBKAVPassword As String = _
                        System.Configuration.ConfigurationManager.AppSettings("BKAVPassword").ToString
            Dim v_strLDAPIP As String = _
                        System.Configuration.ConfigurationManager.AppSettings("LDAPIP").ToString
            Dim v_strArrayOgarnization As String = _
                        System.Configuration.ConfigurationManager.AppSettings("ArrayOgarnization").ToString
            Dim v_strBKAVDomainComponent As String = _
                System.Configuration.ConfigurationManager.AppSettings("BKAVDomainComponent").ToString
            Dim v_strAppPath As String = System.AppDomain.CurrentDomain.BaseDirectory


            'v_dataAccess.GetSysVar("CA", "APP_LOCATION", v_strAppPath)
            'Tách Đường dẫn từ client lên để lấy file trên server
            Dim v_arrFileName() As String
            v_arrFileName = v_strFileNameCA.Split("\")
            Dim i As Integer = v_arrFileName.Length
            v_strFileNameCA = v_arrFileName(i - 1)
            'Dim v_strServerFile As String = "C:\Server\" & v_strFileNameCA
            v_arrFileName = v_strFileNameCA.Split("'")
            Dim v_strDateCreated As String = v_arrFileName(3)
            Dim v_strTLLOGID As String = v_arrFileName(1)
            Dim v_strTLName1 As String = v_arrFileName(0)
            'System.AppDomain.CurrentDomain.BaseDirectory
            'Them lay file tu FTP Server về

            Dim v_strLocalDir = v_strAppPath & "\Log\" & v_strDateCreated
            ' Đẩy file lên FTP server
            Dim v_strServerAddress, v_strServerPort, v_strUsername, v_strPassword, v_strRemotePath, v_strRootPath As String
            v_dataAccess.GetSysVar("FILEFTPSVR", "ServerAddress", "0001", v_strServerAddress)
            v_dataAccess.GetSysVar("FILEFTPSVR", "ServerPort", "0001", v_strServerPort)
            v_dataAccess.GetSysVar("FILEFTPSVR", "Username", "0001", v_strUsername)
            v_dataAccess.GetSysVar("FILEFTPSVR", "Password", "0001", v_strPassword)
            v_dataAccess.GetSysVar("FILEFTPSVR", "RemotePath", "0001", v_strRemotePath)
            v_dataAccess.GetSysVar("FILEFTPSVR", "RootPath", "0001", v_strRootPath)

            Dim v_oWriter As StreamWriter
            v_oWriter = New StreamWriter(v_strLocalDir & "\" & Replace(v_strFileNameCA, ".dat", ".bat"))
            v_oWriter.WriteLine("@ftp -i -s:""%~f0""&GOTO:EOF")
            v_oWriter.WriteLine("open " & v_strServerAddress)
            v_oWriter.WriteLine(v_strUsername)
            v_oWriter.WriteLine(v_strPassword)
            v_oWriter.WriteLine("lcd " & """" & v_strLocalDir & """")
            v_oWriter.WriteLine("cd " & v_strRemotePath)
            v_oWriter.WriteLine("binary")
            v_oWriter.WriteLine("get " & v_strFileNameCA & " " & v_strFileNameCA)
            v_oWriter.WriteLine("bye" & vbCrLf)

            v_oWriter.Close()

            Dim v_oProcess As Process
            v_oProcess = New Process

            v_oProcess.StartInfo.FileName = v_strLocalDir & "\" & Replace(v_strFileNameCA, ".dat", ".bat")
            v_oProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
            v_oProcess.StartInfo.CreateNoWindow = True
            v_oProcess.Start()
            'v_oProcess.WaitForExit()
            v_oProcess.Close()
            System.Threading.Thread.Sleep(3 * 1000)
            'end Them lay file tu FTP Server về

            Dim v_strServerFile As String = v_strAppPath & "\Log\" & v_strDateCreated & "\" & v_strFileNameCA

            If Not File.Exists(v_strServerFile) Then
                v_intErrCount = 1
                v_strErrMsg = "File dữ liệu ở client và server không khớp"
                Exit Function
            End If

            Dim v_Stream As New System.IO.StreamReader(v_strServerFile)
            Dim v_strData As String = v_Stream.ReadToEnd
            Dim v_strSQL As String


            'lấy xml gốc của giao dịch
            v_strTLName = v_strTLName1
            'get tlid                 
            Dim v_xmlDocumentMessage As New XmlDocumentEx
            v_xmlDocumentMessage.LoadXml(v_strData)
            Dim v_attrColl As Xml.XmlAttributeCollection = v_xmlDocumentMessage.DocumentElement.Attributes
            Dim v_strDataFileServer As String = CStr(CType(v_attrColl.GetNamedItem("DATA"), Xml.XmlAttribute).Value)
            v_strDataFileServer = ServerBussinessCA.DecryptXML(v_strDataFileServer, v_strTLName)
            'Lay chu ky cua VSD
            Dim v_strVSDSignal As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeSignatureVSD), Xml.XmlAttribute).Value)

            'Lay chu ky cua Nguoi dung
            'gc_AtributeSignatureClient
            Dim v_strUserSignal As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeSignatureClient), Xml.XmlAttribute).Value)
            'Tach status 
            v_xmlDocumentMessage = New XmlDocumentEx
            v_xmlDocumentMessage.LoadXml(v_strDataFileServer)
            v_attrColl = v_xmlDocumentMessage.DocumentElement.Attributes
            v_strDataFileServer = CStr(CType(v_attrColl.GetNamedItem("DATAXML1"), Xml.XmlAttribute).Value)
            'end tach status

            'Lấy dữ liệu từ trong File để tạo thành Dataset chứa dữ liệu để đối chứng

            'v_obj.Dispose()
            Dim XMLDocumentMessage As New XmlDocumentEx
            Dim dataElement As Xml.XmlElement
            Dim v_attrData, v_attrVSDSignature, v_attrClientSignature As Xml.XmlAttribute
            dataElement = XMLDocumentMessage.CreateElement("RPData")

            v_attrData = XMLDocumentMessage.CreateAttribute("DATA")
            v_attrData.Value = v_strDataFileServer 'ByteArrayToStr(v_arrByte)
            dataElement.Attributes.Append(v_attrData)

            v_attrVSDSignature = XMLDocumentMessage.CreateAttribute(gc_AtributeSignatureVSD)
            v_attrVSDSignature.Value = v_strVSDSignal
            dataElement.Attributes.Append(v_attrVSDSignature)

            v_attrClientSignature = XMLDocumentMessage.CreateAttribute(gc_AtributeSignatureClient)
            v_attrClientSignature.Value = v_strUserSignal
            dataElement.Attributes.Append(v_attrClientSignature)

            XMLDocumentMessage.AppendChild(dataElement)
            'Dim v_oWriter As System.IO.StreamWriter
            'Dim v_strAppPath As String = System.AppDomain.CurrentDomain.BaseDirectory

            'v_DataAccess.GetSysVar("CA", "VSD_CA", v_strLocalDir)
            'v_strLocalDir = v_strLocalDir & "\Server"

            v_strSQL = "select to_char(to_date(varvalue,'dd/mm/yyyy'),'YYYYMMDD') date_, to_char(to_date(varvalue,'dd/mm/yyyy'),'YYYYMMDD')||''''||to_char(sysdate,'hh24miss') time from sysvar where varname ='CURRDATE' and brid ='0001'"
            Dim v_trace As System.Data.DataSet = v_dataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            Dim v_strTime As String = v_trace.Tables(0).Rows(0)("time")
            v_strLocalDir = v_trace.Tables(0).Rows(0)("date_")

            Dim f As New IO.DirectoryInfo(v_strAppPath & "\Log")
            If Not f.Exists Then
                Directory.CreateDirectory(v_strAppPath & "\Log")
            End If

            v_strAppPath = v_strAppPath & "\Log\" & v_strLocalDir

            f = New IO.DirectoryInfo(v_strAppPath)
            If Not f.Exists Then
                Directory.CreateDirectory(v_strAppPath)
            End If
            Dim v_strFileName = v_strTLName & "_Export_TRANS_" & v_strTime & ""
            If File.Exists(v_strAppPath & "\" & v_strFileName & ".xml") Then
                File.Delete(v_strAppPath & "\" & v_strFileName & ".xml")
            End If
            'end bangpv
            v_oWriter = New StreamWriter(v_strAppPath & "\" & v_strFileName & ".xml")
            v_oWriter.WriteLine(XMLDocumentMessage.InnerXml)
            v_oWriter.Close()




            v_intErrCount = 1
            v_strErrMsg = "Dữ liệu hợp lệ" & vbCrLf & "Đã kết xuất file tại: " & v_strAppPath & "\" & v_strFileName & ".xml"




        Catch ex As Exception
            v_strErrMsg = "File dữ liệu không hợp lệ!. Chi tiết: " & ex.Message
            v_intErrCount += 1
            Return False
        End Try
    End Function

    'Backup truoc khi sua phan nhan du lieu Hose su dung bang tam (15/03/2021)
    Private Function HOSTReadFileASTDT_1(ByVal pv_strFile As String, ByRef pv_oNode As Xml.XmlNode, ByRef pv_oXml As XmlDocumentEx, ByVal v_strTLTXCD As String, ByVal v_strPARENT_TXNUM As String, ByVal v_strPARENT_TXDATE As String, ByVal v_strPARENT_BUSDATE As String, ByRef v_strErrMsg As String, ByRef v_intErrCount As Integer) As Boolean
        Try
            Dim v_Stream As New System.IO.StreamReader(pv_strFile)
            Dim v_strLine As String
            Dim v_strConfirmNo, v_strMatch_Date, v_strMatch_Time, v_strSec_Code, v_strSET_TYPE As String
            Dim v_strQty, v_strPrice, v_strB_ACC_NO, v_strS_ACC_NO, v_strBlock_Tran As String
            Dim v_strB_CODE_TRADE, v_strS_CODE_TRADE, v_strB_ORDER_NO, v_strS_ORDER_NO, v_strB_PC_PLAG, v_strS_PC_PLAG As String
            Dim v_entryNode As Xml.XmlNode
            Dim v_attrFLDNAME As Xml.XmlAttribute, v_attrDATATYPE As Xml.XmlAttribute
            Dim v_strStatus As String

            Dim strRows As String = ""

            v_strBlock_Tran = "0"
            v_strSET_TYPE = "3"

            Dim strInsertFields, strInsertValues As String
            strInsertFields = ",COL_VALUE01, COL_TYPE01, COL_DESC01,COL_VALUE02, COL_TYPE02 , COL_DESC02,COL_VALUE03, COL_TYPE03 , COL_DESC03"
            strInsertValues = ",'" & v_strPARENT_TXNUM & "'" & " COL_VALUE01" _
            & ", 'C'" & " COL_TYPE01" _
            & ", '" & v_strPARENT_TXNUM & "'" & " COL_DESC01" _
            & ", '" & v_strPARENT_TXDATE & "'" & " COL_VALUE02" _
            & ", 'D'" & " COL_TYPE02" _
            & ", '" & v_strPARENT_TXDATE & "'" & " COL_DESC02" _
            & ", '" & v_strPARENT_BUSDATE & "'" & " COL_VALUE03" _
            & ", 'D'" & " COL_TYPE03" _
            & ", '" & v_strPARENT_BUSDATE & "'" & " COL_DESC03"

            Dim v_strFieldsSQL, v_strValuesSQL As String
            Dim v_int As Integer = 0

            Dim v_dataElement As Xml.XmlElement
            v_dataElement = pv_oXml.CreateElement(Xml.XmlNodeType.Element, "fields", "")
            v_strFieldsSQL = ""
            v_strFieldsSQL &= ",COL_VALUE04,COL_TYPE04,COL_DESC04"
            v_strFieldsSQL &= ",COL_VALUE05,COL_TYPE05,COL_DESC05"
            v_strFieldsSQL &= ",COL_VALUE06,COL_TYPE06,COL_DESC06"
            v_strFieldsSQL &= ",COL_VALUE07,COL_TYPE07,COL_DESC07"
            v_strFieldsSQL &= ",COL_VALUE08,COL_TYPE08,COL_DESC08"
            v_strFieldsSQL &= ",COL_VALUE09,COL_TYPE09,COL_DESC09"
            v_strFieldsSQL &= ",COL_VALUE10,COL_TYPE10,COL_DESC10"
            v_strFieldsSQL &= ",COL_VALUE11,COL_TYPE11,COL_DESC11"
            v_strFieldsSQL &= ",COL_VALUE14,COL_TYPE14,COL_DESC14"
            v_strFieldsSQL &= ",COL_VALUE15,COL_TYPE15,COL_DESC15"
            v_strFieldsSQL &= ",COL_VALUE16,COL_TYPE16,COL_DESC16"
            v_strFieldsSQL &= ",COL_VALUE17,COL_TYPE17,COL_DESC17"
            v_strFieldsSQL &= ",COL_VALUE18,COL_TYPE18,COL_DESC18"
            v_strFieldsSQL &= ",COL_VALUE19,COL_TYPE19,COL_DESC19"
            v_strFieldsSQL &= ",COL_VALUE12,COL_TYPE12,COL_DESC12"
            v_strFieldsSQL &= ",COL_VALUE13,COL_TYPE13,COL_DESC13"

            While Not v_Stream.EndOfStream
                v_strValuesSQL = ""

                v_strLine = v_Stream.ReadLine
                If v_strLine <> "" Then
                    v_strStatus = Trim(Mid(v_strLine, 90, 2))
                    If (v_strStatus <> "XC") And (v_strStatus <> "XS") And (v_strStatus <> "") Then
                    Else
                        v_strConfirmNo = String.Format("{0:000000}", CInt(Mid(v_strLine, 1, 6)))

                        v_strValuesSQL &= ",'" & v_strConfirmNo.Trim & "' COL_VALUE04,'C' COL_TYPE04,'" & v_strConfirmNo.Trim & "' COL_DESC04"

                        v_strMatch_Time = Mid(v_strLine, 55, 8)

                        v_strValuesSQL &= ",'" & v_strMatch_Time.Trim & "' COL_VALUE05,'C' COL_TYPE05,'" & v_strMatch_Time.Trim & "' COL_DESC05"

                        v_strMatch_Date = Mid(v_strLine, 63, 10)

                        v_strValuesSQL &= ",'" & v_strMatch_Date & "' COL_VALUE06,'D' COL_TYPE06,'" & v_strMatch_Date & "' COL_DESC06"

                        v_strSec_Code = Mid(v_strLine, 92, 8)

                        v_strValuesSQL &= ",'" & v_strSec_Code.Trim & "' COL_VALUE07,'C' COL_TYPE07,'" & v_strSec_Code.Trim & "' COL_DESC07"


                        v_strPrice = CStr(CDbl(Mid(v_strLine, 108, 9)) * 1000)

                        v_strValuesSQL &= ",'" & v_strPrice.Trim & "' COL_VALUE08,'N' COL_TYPE08,'" & v_strPrice.Trim & "' COL_DESC08"

                        v_strQty = Mid(v_strLine, 100, 8)

                        v_strValuesSQL &= ",'" & v_strQty.Trim & "' COL_VALUE09,'N' COL_TYPE09,'" & v_strQty.Trim & "' COL_DESC09"

                        v_strB_ACC_NO = Mid(v_strLine, 117, 10)

                        v_strValuesSQL &= ",'" & v_strB_ACC_NO.Trim & "' COL_VALUE10,'C' COL_TYPE10,'" & v_strB_ACC_NO.Trim & "' COL_DESC10"

                        v_strS_ACC_NO = Mid(v_strLine, 127, 10)

                        v_strValuesSQL &= ",'" & v_strS_ACC_NO.Trim & "' COL_VALUE11,'C' COL_TYPE11,'" & v_strS_ACC_NO.Trim & "' COL_DESC11"

                        v_strB_CODE_TRADE = Mid(v_strLine, 83, 3)

                        v_strValuesSQL &= ",'" & v_strB_CODE_TRADE.Trim & "' COL_VALUE14,'C' COL_TYPE14,'" & v_strB_CODE_TRADE.Trim & "' COL_DESC14"

                        v_strS_CODE_TRADE = Mid(v_strLine, 86, 3)

                        v_strValuesSQL &= ",'" & v_strS_CODE_TRADE.Trim & "' COL_VALUE15,'C' COL_TYPE15,'" & v_strS_CODE_TRADE.Trim & "' COL_DESC15"

                        v_strB_ORDER_NO = Mid(v_strLine, 7, 8)

                        v_strValuesSQL &= ",'" & v_strB_ORDER_NO.Trim & "' COL_VALUE16,'C' COL_TYPE16,'" & v_strB_ORDER_NO.Trim & "' COL_DESC16"

                        v_strS_ORDER_NO = Mid(v_strLine, 25, 8)

                        v_strValuesSQL &= ",'" & v_strS_ORDER_NO.Trim & "' COL_VALUE17,'C' COL_TYPE17,'" & v_strS_ORDER_NO.Trim & "' COL_DESC17"

                        v_strB_PC_PLAG = Mid(v_strLine, 81, 1)

                        v_strValuesSQL &= ",'" & v_strB_PC_PLAG.Trim & "' COL_VALUE18,'C' COL_TYPE18,'" & v_strB_PC_PLAG.Trim & "' COL_DESC18"

                        v_strS_PC_PLAG = Mid(v_strLine, 82, 1)

                        v_strValuesSQL &= ",'" & v_strS_PC_PLAG.Trim & "' COL_VALUE19,'C' COL_TYPE19,'" & v_strS_PC_PLAG.Trim & "' COL_DESC19"


                        v_strValuesSQL &= ",'" & v_strSET_TYPE.Trim & "' COL_VALUE12,'N' COL_TYPE12,'" & v_strSET_TYPE.Trim & "' COL_DESC12"


                        v_strValuesSQL &= ",'" & v_strBlock_Tran.Trim & "' COL_VALUE13,'N' COL_TYPE13,'" & v_strBlock_Tran.Trim & "' COL_DESC13"
                        strRows = strRows & "UNION ALL SELECT '" & v_strTLTXCD & "' TLTXCD " & v_strValuesSQL & " " & strInsertValues & ", " & v_int + 1 & " as REAL_ROW FROM DUAL " & vbCrLf

                        If (v_int + 1) Mod gc_IMP_TRAN_UNIT = 0 Then
                            'Append entry to data node
                            'err 2
                            'bangpv
                            v_strErrMsg = "Loi tai: 'err 2: If (v_int + 1) Mod gc_IMP_TRAN_UNIT = 0 Then (dong:" & v_int & ")"
                            'end bangpv

                            v_entryNode = pv_oXml.CreateNode(Xml.XmlNodeType.Element, "entry", "")
                            'Add field name
                            v_attrFLDNAME = pv_oXml.CreateAttribute(gc_AtributeFLDNAME)
                            v_attrFLDNAME.Value = "INSERT_ROW"
                            v_entryNode.Attributes.Append(v_attrFLDNAME)
                            'Add field type
                            v_attrDATATYPE = pv_oXml.CreateAttribute(gc_AtributeFLDTYPE)
                            v_attrDATATYPE.Value = "C"
                            v_entryNode.Attributes.Append(v_attrDATATYPE)
                            'Set value
                            strRows = "INSERT INTO TMP_TXFIELDS(AUTOID, TLTXCD " & v_strFieldsSQL & " " & strInsertFields & ", REAL_ROW ) " _
                                & " SELECT seq_tmp_txfields.nextval , a.* from (" & strRows.Substring(10) & ") a "
                            v_entryNode.InnerText = strRows
                            v_dataElement.AppendChild(v_entryNode)
                            strRows = ""
                        End If
                        v_int = v_int + 1
                    End If
                End If
            End While
            If strRows <> "" Then
                'bangpv: check loi 
                v_strErrMsg = "'err 3:  If strRows <> "" Then (dong: " & v_int & ")"
                'end bangpv
                v_entryNode = pv_oXml.CreateNode(Xml.XmlNodeType.Element, "entry", "")
                'Add field name
                v_attrFLDNAME = pv_oXml.CreateAttribute(gc_AtributeFLDNAME)
                v_attrFLDNAME.Value = "INSERT_ROW"
                v_entryNode.Attributes.Append(v_attrFLDNAME)
                'Add field type
                v_attrDATATYPE = pv_oXml.CreateAttribute(gc_AtributeFLDTYPE)
                v_attrDATATYPE.Value = "C"
                v_entryNode.Attributes.Append(v_attrDATATYPE)
                'Set value
                strRows = "INSERT INTO TMP_TXFIELDS(AUTOID, TLTXCD " & v_strFieldsSQL & " " & strInsertFields & ", REAL_ROW ) " _
                    & " SELECT seq_tmp_txfields.nextval , a.* from (" & strRows.Substring(10) & ") a "
                v_entryNode.InnerText = strRows
                v_dataElement.AppendChild(v_entryNode)
            End If
            If v_int > 0 Then
                pv_oNode.AppendChild(v_dataElement)
            End If

            If Not v_Stream Is Nothing Then
                v_Stream.Dispose()
            End If
            Return True
        Catch ex As Exception
            v_strErrMsg = "File dữ liệu không hợp lệ!. Chi tiết: " & ex.Message
            v_intErrCount += 1
            Return False
        End Try
    End Function

    Private Function HOSTReadFileASTDT_41(ByVal pv_strFile As String, ByRef pv_oNode As Xml.XmlNode, ByRef pv_oXml As XmlDocumentEx, ByVal v_strTLTXCD As String, ByVal v_strPARENT_TXNUM As String, ByVal v_strPARENT_TXDATE As String, ByVal v_strPARENT_BUSDATE As String, ByRef v_strErrMsg As String, ByRef v_intErrCount As Integer) As Boolean
        Try
            Dim v_Stream As New System.IO.StreamReader(pv_strFile)
            Dim v_strLine As String
            Dim v_strConfirmNo, v_strMatch_Date, v_strMatch_Time, v_strSec_Code, v_strSET_TYPE As String
            Dim v_strQty, v_strPrice, v_strB_ACC_NO, v_strS_ACC_NO, v_strBlock_Tran As String
            Dim v_strB_CODE_TRADE, v_strS_CODE_TRADE, v_strB_ORDER_NO, v_strS_ORDER_NO, v_strB_PC_PLAG, v_strS_PC_PLAG As String
            Dim v_entryNode As Xml.XmlNode
            Dim v_attrFLDNAME As Xml.XmlAttribute, v_attrDATATYPE As Xml.XmlAttribute
            Dim v_strStatus As String

            Dim strRows As String = ""

            v_strBlock_Tran = "0"
            v_strSET_TYPE = "3"

            Dim strInsertFields, strInsertValues As String
            strInsertFields = ",COL_VALUE01, COL_TYPE01, COL_DESC01,COL_VALUE02, COL_TYPE02 , COL_DESC02,COL_VALUE03, COL_TYPE03 , COL_DESC03"
            strInsertValues = ",'" & v_strPARENT_TXNUM & "'" & " COL_VALUE01" _
            & ", 'C'" & " COL_TYPE01" _
            & ", '" & v_strPARENT_TXNUM & "'" & " COL_DESC01" _
            & ", '" & v_strPARENT_TXDATE & "'" & " COL_VALUE02" _
            & ", 'D'" & " COL_TYPE02" _
            & ", '" & v_strPARENT_TXDATE & "'" & " COL_DESC02" _
            & ", '" & v_strPARENT_BUSDATE & "'" & " COL_VALUE03" _
            & ", 'D'" & " COL_TYPE03" _
            & ", '" & v_strPARENT_BUSDATE & "'" & " COL_DESC03"

            Dim v_strFieldsSQL, v_strValuesSQL As String
            Dim v_int As Integer = 0

            Dim v_dataElement As Xml.XmlElement
            v_dataElement = pv_oXml.CreateElement(Xml.XmlNodeType.Element, "fields", "")
            v_strFieldsSQL = ""
            v_strFieldsSQL &= ",COL_VALUE04,COL_TYPE04,COL_DESC04"
            v_strFieldsSQL &= ",COL_VALUE05,COL_TYPE05,COL_DESC05"
            v_strFieldsSQL &= ",COL_VALUE06,COL_TYPE06,COL_DESC06"
            v_strFieldsSQL &= ",COL_VALUE07,COL_TYPE07,COL_DESC07"
            v_strFieldsSQL &= ",COL_VALUE08,COL_TYPE08,COL_DESC08"
            v_strFieldsSQL &= ",COL_VALUE09,COL_TYPE09,COL_DESC09"
            v_strFieldsSQL &= ",COL_VALUE10,COL_TYPE10,COL_DESC10"
            v_strFieldsSQL &= ",COL_VALUE11,COL_TYPE11,COL_DESC11"
            v_strFieldsSQL &= ",COL_VALUE14,COL_TYPE14,COL_DESC14"
            v_strFieldsSQL &= ",COL_VALUE15,COL_TYPE15,COL_DESC15"
            v_strFieldsSQL &= ",COL_VALUE16,COL_TYPE16,COL_DESC16"
            v_strFieldsSQL &= ",COL_VALUE17,COL_TYPE17,COL_DESC17"
            v_strFieldsSQL &= ",COL_VALUE18,COL_TYPE18,COL_DESC18"
            v_strFieldsSQL &= ",COL_VALUE19,COL_TYPE19,COL_DESC19"
            v_strFieldsSQL &= ",COL_VALUE12,COL_TYPE12,COL_DESC12"
            v_strFieldsSQL &= ",COL_VALUE13,COL_TYPE13,COL_DESC13"

            While Not v_Stream.EndOfStream
                v_strValuesSQL = ""

                v_strLine = v_Stream.ReadLine
                If v_strLine <> "" Then
                    'v_strStatus = Trim(Mid(v_strLine, 90, 2))
                    v_strStatus = Trim(Mid(v_strLine, 96, 2))
                    If (v_strStatus <> "XC") And (v_strStatus <> "XS") And (v_strStatus <> "") Then
                    Else
                        'v_strConfirmNo = String.Format("{0:000000}", CInt(Mid(v_strLine, 1, 6)))
                        v_strConfirmNo = String.Format("{0:000000000000}", CLng(Mid(v_strLine, 1, 12)))
                        v_strValuesSQL &= ",'" & v_strConfirmNo.Trim & "' COL_VALUE04,'C' COL_TYPE04,'" & v_strConfirmNo.Trim & "' COL_DESC04"

                        'v_strMatch_Time = Mid(v_strLine, 55, 8)
                        v_strMatch_Time = Mid(v_strLine, 61, 8)
                        v_strValuesSQL &= ",'" & v_strMatch_Time.Trim & "' COL_VALUE05,'C' COL_TYPE05,'" & v_strMatch_Time.Trim & "' COL_DESC05"

                        'v_strMatch_Date = Mid(v_strLine, 63, 10)
                        v_strMatch_Date = Mid(v_strLine, 69, 10)
                        v_strValuesSQL &= ",'" & v_strMatch_Date & "' COL_VALUE06,'D' COL_TYPE06,'" & v_strMatch_Date & "' COL_DESC06"

                        'v_strSec_Code = Mid(v_strLine, 92, 8)
                        v_strSec_Code = Mid(v_strLine, 98, 8)
                        v_strValuesSQL &= ",'" & v_strSec_Code.Trim & "' COL_VALUE07,'C' COL_TYPE07,'" & v_strSec_Code.Trim & "' COL_DESC07"


                        'v_strPrice = CStr(CDbl(Mid(v_strLine, 108, 9)) * 1000)
                        v_strPrice = CStr(CDbl(Mid(v_strLine, 114, 9)) * 1000)

                        v_strValuesSQL &= ",'" & v_strPrice.Trim & "' COL_VALUE08,'N' COL_TYPE08,'" & v_strPrice.Trim & "' COL_DESC08"

                        'v_strQty = Mid(v_strLine, 100, 8)
                        v_strQty = Mid(v_strLine, 106, 8)
                        v_strValuesSQL &= ",'" & v_strQty.Trim & "' COL_VALUE09,'N' COL_TYPE09,'" & v_strQty.Trim & "' COL_DESC09"

                        'v_strB_ACC_NO = Mid(v_strLine, 117, 10)
                        v_strB_ACC_NO = Mid(v_strLine, 123, 10)
                        v_strValuesSQL &= ",'" & v_strB_ACC_NO.Trim & "' COL_VALUE10,'C' COL_TYPE10,'" & v_strB_ACC_NO.Trim & "' COL_DESC10"

                        'v_strS_ACC_NO = Mid(v_strLine, 127, 10)
                        v_strS_ACC_NO = Mid(v_strLine, 133, 10)
                        v_strValuesSQL &= ",'" & v_strS_ACC_NO.Trim & "' COL_VALUE11,'C' COL_TYPE11,'" & v_strS_ACC_NO.Trim & "' COL_DESC11"

                        'v_strB_CODE_TRADE = Mid(v_strLine, 83, 3)
                        v_strB_CODE_TRADE = Mid(v_strLine, 89, 3)
                        v_strValuesSQL &= ",'" & v_strB_CODE_TRADE.Trim & "' COL_VALUE14,'C' COL_TYPE14,'" & v_strB_CODE_TRADE.Trim & "' COL_DESC14"

                        'v_strS_CODE_TRADE = Mid(v_strLine, 86, 3)
                        v_strS_CODE_TRADE = Mid(v_strLine, 92, 3)
                        v_strValuesSQL &= ",'" & v_strS_CODE_TRADE.Trim & "' COL_VALUE15,'C' COL_TYPE15,'" & v_strS_CODE_TRADE.Trim & "' COL_DESC15"

                        'v_strB_ORDER_NO = Mid(v_strLine, 7, 8)
                        v_strB_ORDER_NO = Mid(v_strLine, 13, 8)
                        v_strValuesSQL &= ",'" & v_strB_ORDER_NO.Trim & "' COL_VALUE16,'C' COL_TYPE16,'" & v_strB_ORDER_NO.Trim & "' COL_DESC16"

                        'v_strS_ORDER_NO = Mid(v_strLine, 25, 8)
                        v_strS_ORDER_NO = Mid(v_strLine, 31, 8)
                        v_strValuesSQL &= ",'" & v_strS_ORDER_NO.Trim & "' COL_VALUE17,'C' COL_TYPE17,'" & v_strS_ORDER_NO.Trim & "' COL_DESC17"

                        'v_strB_PC_PLAG = Mid(v_strLine, 81, 1)
                        v_strB_PC_PLAG = Mid(v_strLine, 87, 1)
                        v_strValuesSQL &= ",'" & v_strB_PC_PLAG.Trim & "' COL_VALUE18,'C' COL_TYPE18,'" & v_strB_PC_PLAG.Trim & "' COL_DESC18"

                        'v_strS_PC_PLAG = Mid(v_strLine, 82, 1)
                        v_strS_PC_PLAG = Mid(v_strLine, 88, 1)
                        v_strValuesSQL &= ",'" & v_strS_PC_PLAG.Trim & "' COL_VALUE19,'C' COL_TYPE19,'" & v_strS_PC_PLAG.Trim & "' COL_DESC19"


                        v_strValuesSQL &= ",'" & v_strSET_TYPE.Trim & "' COL_VALUE12,'N' COL_TYPE12,'" & v_strSET_TYPE.Trim & "' COL_DESC12"


                        v_strValuesSQL &= ",'" & v_strBlock_Tran.Trim & "' COL_VALUE13,'N' COL_TYPE13,'" & v_strBlock_Tran.Trim & "' COL_DESC13"
                        strRows = strRows & "UNION ALL SELECT '" & v_strTLTXCD & "' TLTXCD " & v_strValuesSQL & " " & strInsertValues & ", " & v_int + 1 & " as REAL_ROW FROM DUAL " & vbCrLf

                        If (v_int + 1) Mod gc_IMP_TRAN_UNIT = 0 Then
                            'Append entry to data node
                            'err 2
                            'bangpv
                            v_strErrMsg = "Loi tai: 'err 2: If (v_int + 1) Mod gc_IMP_TRAN_UNIT = 0 Then (dong:" & v_int & ")"
                            'end bangpv

                            v_entryNode = pv_oXml.CreateNode(Xml.XmlNodeType.Element, "entry", "")
                            'Add field name
                            v_attrFLDNAME = pv_oXml.CreateAttribute(gc_AtributeFLDNAME)
                            v_attrFLDNAME.Value = "INSERT_ROW"
                            v_entryNode.Attributes.Append(v_attrFLDNAME)
                            'Add field type
                            v_attrDATATYPE = pv_oXml.CreateAttribute(gc_AtributeFLDTYPE)
                            v_attrDATATYPE.Value = "C"
                            v_entryNode.Attributes.Append(v_attrDATATYPE)
                            'Set value
                            strRows = "INSERT INTO TMP_TXFIELDS(AUTOID, TLTXCD " & v_strFieldsSQL & " " & strInsertFields & ", REAL_ROW ) " _
                                & " SELECT seq_tmp_txfields.nextval , a.* from (" & strRows.Substring(10) & ") a "
                            v_entryNode.InnerText = strRows
                            v_dataElement.AppendChild(v_entryNode)
                            strRows = ""
                        End If
                        v_int = v_int + 1
                    End If
                End If
            End While
            If strRows <> "" Then
                'bangpv: check loi 
                v_strErrMsg = "'err 3:  If strRows <> "" Then (dong: " & v_int & ")"
                'end bangpv
                v_entryNode = pv_oXml.CreateNode(Xml.XmlNodeType.Element, "entry", "")
                'Add field name
                v_attrFLDNAME = pv_oXml.CreateAttribute(gc_AtributeFLDNAME)
                v_attrFLDNAME.Value = "INSERT_ROW"
                v_entryNode.Attributes.Append(v_attrFLDNAME)
                'Add field type
                v_attrDATATYPE = pv_oXml.CreateAttribute(gc_AtributeFLDTYPE)
                v_attrDATATYPE.Value = "C"
                v_entryNode.Attributes.Append(v_attrDATATYPE)
                'Set value
                strRows = "INSERT INTO TMP_TXFIELDS(AUTOID, TLTXCD " & v_strFieldsSQL & " " & strInsertFields & ", REAL_ROW ) " _
                    & " SELECT seq_tmp_txfields.nextval , a.* from (" & strRows.Substring(10) & ") a "
                v_entryNode.InnerText = strRows
                v_dataElement.AppendChild(v_entryNode)
            End If
            If v_int > 0 Then
                pv_oNode.AppendChild(v_dataElement)
            End If

            If Not v_Stream Is Nothing Then
                v_Stream.Dispose()
            End If
            Return True
        Catch ex As Exception
            v_strErrMsg = "File dữ liệu không hợp lệ!. Chi tiết: " & ex.Message
            v_intErrCount += 1
            Return False
        End Try
    End Function
    Private Function HOSTReadFileASTDT_3(ByVal pv_strFile As String, ByRef pv_oNode As Xml.XmlNode, ByRef pv_oXml As XmlDocumentEx, ByVal v_strTLTXCD As String, ByVal v_strPARENT_TXNUM As String, ByVal v_strPARENT_TXDATE As String, ByVal v_strPARENT_BUSDATE As String, ByRef v_strErrMsg As String, ByRef v_intErrCount As Integer) As Boolean
        Try
            Dim v_dataElement As Xml.XmlElement
            Dim v_entryNode As Xml.XmlNode
            Dim v_attrFLDNAME As Xml.XmlAttribute, v_attrDATATYPE As Xml.XmlAttribute

            Dim strRows As String = ""
            Dim v_obj As New DataAccess
            v_obj.NewDBInstance(gc_MODULE_HOST)
            Dim v_strSql = "UPDATE txfields_astdl SET COL_VALUE01='" & v_strPARENT_TXNUM & "', COL_DESC01='" & v_strPARENT_TXNUM & "', " _
                            & "COL_VALUE02='" & v_strPARENT_TXDATE & "', COL_DESC02='" & v_strPARENT_TXDATE & "', " _
                            & "COL_VALUE03='" & v_strPARENT_BUSDATE & "', COL_DESC03='" & v_strPARENT_BUSDATE & "'"

            v_obj.ExecuteNonQuery(CommandType.Text, v_strSql)
            Dim v_ds As DataSet = v_obj.ExecuteSQLReturnDataset(CommandType.Text, "SELECT COUNT(*) FROM txfields_astdl")
            Dim v_int As Integer = CInt(v_ds.Tables(0).Rows(0)(0).ToString())

            v_dataElement = pv_oXml.CreateElement(Xml.XmlNodeType.Element, "fields", "")
            If v_int > 0 Then
                v_entryNode = pv_oXml.CreateNode(Xml.XmlNodeType.Element, "entry", "")
                'Add field name
                v_attrFLDNAME = pv_oXml.CreateAttribute(gc_AtributeFLDNAME)
                v_attrFLDNAME.Value = "INSERT_ROW"
                v_entryNode.Attributes.Append(v_attrFLDNAME)
                'Add field type
                v_attrDATATYPE = pv_oXml.CreateAttribute(gc_AtributeFLDTYPE)
                v_attrDATATYPE.Value = "C"
                v_entryNode.Attributes.Append(v_attrDATATYPE)
                'Set value
                strRows = "INSERT INTO TMP_TXFIELDS SELECT seq_tmp_txfields.nextval, a.* from txfields_astdl a "
                v_entryNode.InnerText = strRows
                v_dataElement.AppendChild(v_entryNode)
                pv_oNode.AppendChild(v_dataElement)
            End If
            Return True
        Catch ex As Exception
            v_strErrMsg = "File dữ liệu không hợp lệ!. Chi tiết: " & ex.Message
            v_intErrCount += 1
            Return False
        End Try
    End Function

    Private Function HOSTReadFileASTDT_43(ByVal pv_strFile As String, ByRef pv_oNode As Xml.XmlNode, ByRef pv_oXml As XmlDocumentEx, ByVal v_strTLTXCD As String, ByVal v_strPARENT_TXNUM As String, ByVal v_strPARENT_TXDATE As String, ByVal v_strPARENT_BUSDATE As String, ByRef v_strErrMsg As String, ByRef v_intErrCount As Integer) As Boolean
        Try
            Dim v_dataElement As Xml.XmlElement
            Dim v_entryNode As Xml.XmlNode
            Dim v_attrFLDNAME As Xml.XmlAttribute, v_attrDATATYPE As Xml.XmlAttribute

            Dim strRows As String = ""
            Dim v_obj As New DataAccess
            v_obj.NewDBInstance(gc_MODULE_HOST)
            Dim v_strSql = "UPDATE txfields_astdl SET COL_VALUE01='" & v_strPARENT_TXNUM & "', COL_DESC01='" & v_strPARENT_TXNUM & "', " _
                            & "COL_VALUE02='" & v_strPARENT_TXDATE & "', COL_DESC02='" & v_strPARENT_TXDATE & "', " _
                            & "COL_VALUE03='" & v_strPARENT_BUSDATE & "', COL_DESC03='" & v_strPARENT_BUSDATE & "'"

            v_obj.ExecuteNonQuery(CommandType.Text, v_strSql)
            Dim v_ds As DataSet = v_obj.ExecuteSQLReturnDataset(CommandType.Text, "SELECT COUNT(*) FROM txfields_astdl")
            Dim v_int As Integer = CInt(v_ds.Tables(0).Rows(0)(0).ToString())

            v_dataElement = pv_oXml.CreateElement(Xml.XmlNodeType.Element, "fields", "")
            If v_int > 0 Then
                v_entryNode = pv_oXml.CreateNode(Xml.XmlNodeType.Element, "entry", "")
                'Add field name
                v_attrFLDNAME = pv_oXml.CreateAttribute(gc_AtributeFLDNAME)
                v_attrFLDNAME.Value = "INSERT_ROW"
                v_entryNode.Attributes.Append(v_attrFLDNAME)
                'Add field type
                v_attrDATATYPE = pv_oXml.CreateAttribute(gc_AtributeFLDTYPE)
                v_attrDATATYPE.Value = "C"
                v_entryNode.Attributes.Append(v_attrDATATYPE)
                'Set value
                strRows = "INSERT INTO TMP_TXFIELDS SELECT seq_tmp_txfields.nextval, a.* from txfields_astdl a "
                v_entryNode.InnerText = strRows
                v_dataElement.AppendChild(v_entryNode)
                pv_oNode.AppendChild(v_dataElement)
            End If
            Return True
        Catch ex As Exception
            v_strErrMsg = "File dữ liệu không hợp lệ!. Chi tiết: " & ex.Message
            v_intErrCount += 1
            Return False
        End Try
    End Function

    'Backup truoc khi sua chuc nang nhan su lieu su dung Tool ngoai (23/05/2021)
    Private Function HOSTReadFileASTDT_2(ByVal pv_strFile As String, ByRef pv_oNode As Xml.XmlNode, ByRef pv_oXml As XmlDocumentEx, ByVal v_strTLTXCD As String, ByVal v_strPARENT_TXNUM As String, ByVal v_strPARENT_TXDATE As String, ByVal v_strPARENT_BUSDATE As String, ByRef v_strErrMsg As String, ByRef v_intErrCount As Integer) As Boolean
        Try
            Dim v_Stream As New System.IO.StreamReader(pv_strFile)
            Dim v_strLine As String
            Dim v_strConfirmNo, v_strMatch_Date, v_strMatch_Time, v_strSec_Code, v_strSET_TYPE As String
            Dim v_strQty, v_strPrice, v_strB_ACC_NO, v_strS_ACC_NO, v_strBlock_Tran As String
            Dim v_strB_CODE_TRADE, v_strS_CODE_TRADE, v_strB_ORDER_NO, v_strS_ORDER_NO, v_strB_PC_PLAG, v_strS_PC_PLAG As String
            Dim v_entryNode As Xml.XmlNode
            Dim v_attrFLDNAME As Xml.XmlAttribute, v_attrDATATYPE As Xml.XmlAttribute
            Dim v_strStatus As String
            Dim strRows As String = ""
            Dim v_dataElement As Xml.XmlElement
            v_dataElement = pv_oXml.CreateElement(Xml.XmlNodeType.Element, "fields", "")

            v_strBlock_Tran = "0"
            v_strSET_TYPE = "3"
            Dim v_obj As New DataAccess
            v_obj.NewDBInstance(gc_MODULE_HOST)
            Dim v_strSettUpdAstdlStatus As String
            v_obj.GetSysVar("SYSTEM", "SETT_UPD_ASTDL_STATUS", "0002", v_strSettUpdAstdlStatus)
            If v_strSettUpdAstdlStatus = "3" Then
                v_entryNode = pv_oXml.CreateNode(Xml.XmlNodeType.Element, "entry", "")
                'Add field name
                v_attrFLDNAME = pv_oXml.CreateAttribute(gc_AtributeFLDNAME)
                v_attrFLDNAME.Value = "INSERT_ROW"
                v_entryNode.Attributes.Append(v_attrFLDNAME)
                'Add field type
                v_attrDATATYPE = pv_oXml.CreateAttribute(gc_AtributeFLDTYPE)
                v_attrDATATYPE.Value = "C"
                v_entryNode.Attributes.Append(v_attrDATATYPE)
                'Set value
                strRows = "INSERT INTO TMP_TXFIELDS SELECT seq_tmp_txfields.nextval, a.* from txfields_astdl a "
                v_entryNode.InnerText = strRows
                v_dataElement.AppendChild(v_entryNode)

                pv_oNode.AppendChild(v_dataElement)

                Return True
            End If

            v_obj.ExecuteNonQuery(CommandType.Text, "UPDATE sysvar SET varvalue = '1' WHERE grname = 'SYSTEM' AND varname = 'SETT_UPD_ASTDL_STATUS' AND brid = '0002'")
            v_obj.ExecuteNonQuery(CommandType.Text, "TRUNCATE TABLE txfields_astdl")
            Dim v_ds As DataSet = v_obj.ExecuteSQLReturnDataset(CommandType.Text, "SELECT * FROM txfields_astdl WHERE 0=1")

            Dim v_int As Integer = 0
            Dim v_intBatchSize As Integer = 0
            Dim v_strTmp As String = "0"
            v_obj.GetSysVar("SYSTEM", "SETT_UPD_BATCH_SIZE", "0002", v_strTmp)
            If v_strTmp = "" Or v_strTmp = "0" Then
                v_intBatchSize = 5000
            Else
                v_intBatchSize = CInt(v_strTmp)
            End If
            Dim v_intOffSet As Integer = 0

            While Not v_Stream.EndOfStream
                Dim v_row As DataRow = v_ds.Tables(0).NewRow()
                v_row("COL_VALUE01") = v_strPARENT_TXNUM
                v_row("COL_TYPE01") = "C"
                v_row("COL_DESC01") = v_strPARENT_TXNUM
                v_row("COL_VALUE02") = v_strPARENT_TXDATE
                v_row("COL_TYPE02") = "D"
                v_row("COL_DESC02") = v_strPARENT_TXDATE
                v_row("COL_VALUE03") = v_strPARENT_BUSDATE
                v_row("COL_TYPE03") = "D"
                v_row("COL_DESC03") = v_strPARENT_BUSDATE

                v_strLine = v_Stream.ReadLine
                If v_strLine <> "" Then
                    v_strStatus = Trim(Mid(v_strLine, 90, 2))
                    If (v_strStatus <> "XC") And (v_strStatus <> "XS") And (v_strStatus <> "") Then
                    Else
                        v_strConfirmNo = String.Format("{0:000000}", CInt(Mid(v_strLine, 1, 6)))
                        v_row("COL_VALUE04") = v_strConfirmNo.Trim
                        v_row("COL_TYPE04") = "C"
                        v_row("COL_DESC04") = v_strConfirmNo.Trim

                        v_strMatch_Time = Mid(v_strLine, 55, 8)
                        v_row("COL_VALUE05") = v_strMatch_Time.Trim
                        v_row("COL_TYPE05") = "C"
                        v_row("COL_DESC05") = v_strMatch_Time.Trim

                        v_strMatch_Date = Mid(v_strLine, 63, 10)
                        v_row("COL_VALUE06") = v_strMatch_Date
                        v_row("COL_TYPE06") = "C"
                        v_row("COL_DESC06") = v_strMatch_Date

                        v_strSec_Code = Mid(v_strLine, 92, 8)
                        v_row("COL_VALUE07") = v_strSec_Code.Trim
                        v_row("COL_TYPE07") = "C"
                        v_row("COL_DESC07") = v_strSec_Code.Trim

                        v_strPrice = CStr(CDbl(Mid(v_strLine, 108, 9)) * 1000)
                        v_row("COL_VALUE08") = v_strPrice.Trim
                        v_row("COL_TYPE08") = "N"
                        v_row("COL_DESC08") = v_strPrice.Trim

                        v_strQty = Mid(v_strLine, 100, 8)
                        v_row("COL_VALUE09") = v_strQty.Trim
                        v_row("COL_TYPE09") = "N"
                        v_row("COL_DESC09") = v_strQty.Trim

                        v_strB_ACC_NO = Mid(v_strLine, 117, 10)
                        v_row("COL_VALUE10") = v_strB_ACC_NO.Trim
                        v_row("COL_TYPE10") = "C"
                        v_row("COL_DESC10") = v_strB_ACC_NO.Trim

                        v_strS_ACC_NO = Mid(v_strLine, 127, 10)
                        v_row("COL_VALUE11") = v_strS_ACC_NO.Trim
                        v_row("COL_TYPE11") = "C"
                        v_row("COL_DESC11") = v_strS_ACC_NO.Trim

                        v_strB_CODE_TRADE = Mid(v_strLine, 83, 3)
                        v_row("COL_VALUE14") = v_strB_CODE_TRADE.Trim
                        v_row("COL_TYPE14") = "C"
                        v_row("COL_DESC14") = v_strB_CODE_TRADE.Trim

                        v_strS_CODE_TRADE = Mid(v_strLine, 86, 3)
                        v_row("COL_VALUE15") = v_strS_CODE_TRADE.Trim
                        v_row("COL_TYPE15") = "C"
                        v_row("COL_DESC15") = v_strS_CODE_TRADE.Trim

                        v_strB_ORDER_NO = Mid(v_strLine, 7, 8)
                        v_row("COL_VALUE16") = v_strB_ORDER_NO.Trim
                        v_row("COL_TYPE16") = "C"
                        v_row("COL_DESC16") = v_strB_ORDER_NO.Trim

                        v_strS_ORDER_NO = Mid(v_strLine, 25, 8)
                        v_row("COL_VALUE17") = v_strS_ORDER_NO.Trim
                        v_row("COL_TYPE17") = "C"
                        v_row("COL_DESC17") = v_strS_ORDER_NO.Trim

                        v_strB_PC_PLAG = Mid(v_strLine, 81, 1)
                        v_row("COL_VALUE18") = v_strB_PC_PLAG.Trim
                        v_row("COL_TYPE18") = "C"
                        v_row("COL_DESC18") = v_strB_PC_PLAG.Trim

                        v_strS_PC_PLAG = Mid(v_strLine, 82, 1)
                        v_row("COL_VALUE19") = v_strS_PC_PLAG.Trim
                        v_row("COL_TYPE19") = "C"
                        v_row("COL_DESC19") = v_strS_PC_PLAG.Trim

                        v_row("COL_VALUE12") = v_strSET_TYPE.Trim
                        v_row("COL_TYPE12") = "N"
                        v_row("COL_DESC12") = v_strSET_TYPE.Trim

                        v_row("COL_VALUE13") = v_strBlock_Tran.Trim
                        v_row("COL_TYPE13") = "N"
                        v_row("COL_DESC13") = v_strBlock_Tran.Trim

                        v_row("TLTXCD") = v_strTLTXCD
                        v_row("REAL_ROW") = v_int

                        v_ds.Tables(0).Rows.Add(v_row)
                        v_int = v_int + 1
                        v_intOffSet = v_intOffSet + 1
                        If v_intOffSet = v_intBatchSize Then
                            If v_obj.SaveUsingOracleBulkCopy("txfields_astdl", v_ds.Tables(0)) Then
                                v_intOffSet = 0
                                v_ds.Tables(0).Rows.Clear()
                            Else
                                Throw New Exception("Error in SaveUsingOracleBulkCopy...")
                            End If
                        End If
                    End If
                End If
            End While
            If v_ds.Tables(0).Rows.Count > 0 Then
                If v_obj.SaveUsingOracleBulkCopy("txfields_astdl", v_ds.Tables(0)) Then
                    v_intOffSet = 0
                    v_ds.Tables(0).Rows.Clear()
                Else
                    Throw New Exception("Error in SaveUsingOracleBulkCopy...")
                End If
            End If
            If v_int > 0 Then
                v_entryNode = pv_oXml.CreateNode(Xml.XmlNodeType.Element, "entry", "")
                'Add field name
                v_attrFLDNAME = pv_oXml.CreateAttribute(gc_AtributeFLDNAME)
                v_attrFLDNAME.Value = "INSERT_ROW"
                v_entryNode.Attributes.Append(v_attrFLDNAME)
                'Add field type
                v_attrDATATYPE = pv_oXml.CreateAttribute(gc_AtributeFLDTYPE)
                v_attrDATATYPE.Value = "C"
                v_entryNode.Attributes.Append(v_attrDATATYPE)
                'Set value
                strRows = "INSERT INTO TMP_TXFIELDS SELECT seq_tmp_txfields.nextval, a.* from txfields_astdl a "
                v_entryNode.InnerText = strRows
                v_dataElement.AppendChild(v_entryNode)

                pv_oNode.AppendChild(v_dataElement)
            End If

            If Not v_Stream Is Nothing Then
                v_Stream.Dispose()
            End If
            v_obj.ExecuteNonQuery(CommandType.Text, "UPDATE sysvar SET varvalue = '3' WHERE grname = 'SYSTEM' AND varname = 'SETT_UPD_ASTDL_STATUS' AND brid = '0002'")
            Return True
        Catch ex As Exception
            v_strErrMsg = "File dữ liệu không hợp lệ!. Chi tiết: " & ex.Message
            v_intErrCount += 1
            Return False
        End Try
    End Function

    Private Function HOSTReadFileASTDT_42(ByVal pv_strFile As String, ByRef pv_oNode As Xml.XmlNode, ByRef pv_oXml As XmlDocumentEx, ByVal v_strTLTXCD As String, ByVal v_strPARENT_TXNUM As String, ByVal v_strPARENT_TXDATE As String, ByVal v_strPARENT_BUSDATE As String, ByRef v_strErrMsg As String, ByRef v_intErrCount As Integer) As Boolean
        Try
            Dim v_Stream As New System.IO.StreamReader(pv_strFile)
            Dim v_strLine As String
            Dim v_strConfirmNo, v_strMatch_Date, v_strMatch_Time, v_strSec_Code, v_strSET_TYPE As String
            Dim v_strQty, v_strPrice, v_strB_ACC_NO, v_strS_ACC_NO, v_strBlock_Tran As String
            Dim v_strB_CODE_TRADE, v_strS_CODE_TRADE, v_strB_ORDER_NO, v_strS_ORDER_NO, v_strB_PC_PLAG, v_strS_PC_PLAG As String
            Dim v_entryNode As Xml.XmlNode
            Dim v_attrFLDNAME As Xml.XmlAttribute, v_attrDATATYPE As Xml.XmlAttribute
            Dim v_strStatus As String
            Dim strRows As String = ""
            Dim v_dataElement As Xml.XmlElement
            v_dataElement = pv_oXml.CreateElement(Xml.XmlNodeType.Element, "fields", "")

            v_strBlock_Tran = "0"
            v_strSET_TYPE = "3"
            Dim v_obj As New DataAccess
            v_obj.NewDBInstance(gc_MODULE_HOST)
            Dim v_strSettUpdAstdlStatus As String
            v_obj.GetSysVar("SYSTEM", "SETT_UPD_ASTDL_STATUS", "0002", v_strSettUpdAstdlStatus)
            If v_strSettUpdAstdlStatus = "3" Then
                v_entryNode = pv_oXml.CreateNode(Xml.XmlNodeType.Element, "entry", "")
                'Add field name
                v_attrFLDNAME = pv_oXml.CreateAttribute(gc_AtributeFLDNAME)
                v_attrFLDNAME.Value = "INSERT_ROW"
                v_entryNode.Attributes.Append(v_attrFLDNAME)
                'Add field type
                v_attrDATATYPE = pv_oXml.CreateAttribute(gc_AtributeFLDTYPE)
                v_attrDATATYPE.Value = "C"
                v_entryNode.Attributes.Append(v_attrDATATYPE)
                'Set value
                strRows = "INSERT INTO TMP_TXFIELDS SELECT seq_tmp_txfields.nextval, a.* from txfields_astdl a "
                v_entryNode.InnerText = strRows
                v_dataElement.AppendChild(v_entryNode)

                pv_oNode.AppendChild(v_dataElement)
                Return True
            End If
            v_obj.ExecuteNonQuery(CommandType.Text, "UPDATE sysvar SET varvalue = '1' WHERE grname = 'SYSTEM' AND varname = 'SETT_UPD_ASTDL_STATUS' AND brid = '0002'")
            v_obj.ExecuteNonQuery(CommandType.Text, "TRUNCATE TABLE txfields_astdl")
            Dim v_ds As DataSet = v_obj.ExecuteSQLReturnDataset(CommandType.Text, "SELECT * FROM txfields_astdl WHERE 0=1")

            Dim v_int As Integer = 0
            Dim v_intBatchSize As Integer = 0
            Dim v_strTmp As String = "0"
            v_obj.GetSysVar("SYSTEM", "SETT_UPD_BATCH_SIZE", "0002", v_strTmp)
            If v_strTmp = "" Or v_strTmp = "0" Then
                v_intBatchSize = 5000
            Else
                v_intBatchSize = CInt(v_strTmp)
            End If
            Dim v_intOffSet As Integer = 0

            While Not v_Stream.EndOfStream
                Dim v_row As DataRow = v_ds.Tables(0).NewRow()
                v_row("COL_VALUE01") = v_strPARENT_TXNUM
                v_row("COL_TYPE01") = "C"
                v_row("COL_DESC01") = v_strPARENT_TXNUM
                v_row("COL_VALUE02") = v_strPARENT_TXDATE
                v_row("COL_TYPE02") = "D"
                v_row("COL_DESC02") = v_strPARENT_TXDATE
                v_row("COL_VALUE03") = v_strPARENT_BUSDATE
                v_row("COL_TYPE03") = "D"
                v_row("COL_DESC03") = v_strPARENT_BUSDATE

                v_strLine = v_Stream.ReadLine
                If v_strLine <> "" Then
                    'v_strStatus = Trim(Mid(v_strLine, 90, 2))
                    v_strStatus = Trim(Mid(v_strLine, 96, 2))
                    If (v_strStatus <> "XC") And (v_strStatus <> "XS") And (v_strStatus <> "") Then
                    Else
                        'v_strConfirmNo = String.Format("{0:000000}", CInt(Mid(v_strLine, 1, 6)))
                        v_strConfirmNo = String.Format("{0:000000000000}", CLng(Mid(v_strLine, 1, 12)))
                        v_row("COL_VALUE04") = v_strConfirmNo.Trim
                        v_row("COL_TYPE04") = "C"
                        v_row("COL_DESC04") = v_strConfirmNo.Trim

                        'v_strMatch_Time = Mid(v_strLine, 55, 8)
                        v_strMatch_Time = Mid(v_strLine, 61, 8)
                        v_row("COL_VALUE05") = v_strMatch_Time.Trim
                        v_row("COL_TYPE05") = "C"
                        v_row("COL_DESC05") = v_strMatch_Time.Trim

                        'v_strMatch_Date = Mid(v_strLine, 63, 10)
                        v_strMatch_Date = Mid(v_strLine, 69, 10)
                        v_row("COL_VALUE06") = v_strMatch_Date
                        v_row("COL_TYPE06") = "C"
                        v_row("COL_DESC06") = v_strMatch_Date

                        'v_strSec_Code = Mid(v_strLine, 92, 8)
                        v_strSec_Code = Mid(v_strLine, 98, 8)
                        v_row("COL_VALUE07") = v_strSec_Code.Trim
                        v_row("COL_TYPE07") = "C"
                        v_row("COL_DESC07") = v_strSec_Code.Trim

                        'v_strPrice = CStr(CDbl(Mid(v_strLine, 108, 9)) * 1000)
                        v_strPrice = CStr(CDbl(Mid(v_strLine, 114, 9)) * 1000)
                        v_row("COL_VALUE08") = v_strPrice.Trim
                        v_row("COL_TYPE08") = "N"
                        v_row("COL_DESC08") = v_strPrice.Trim

                        'v_strQty = Mid(v_strLine, 100, 8)
                        v_strQty = Mid(v_strLine, 106, 8)
                        v_row("COL_VALUE09") = v_strQty.Trim
                        v_row("COL_TYPE09") = "N"
                        v_row("COL_DESC09") = v_strQty.Trim

                        'v_strB_ACC_NO = Mid(v_strLine, 117, 10)
                        v_strB_ACC_NO = Mid(v_strLine, 123, 10)
                        v_row("COL_VALUE10") = v_strB_ACC_NO.Trim
                        v_row("COL_TYPE10") = "C"
                        v_row("COL_DESC10") = v_strB_ACC_NO.Trim

                        'v_strS_ACC_NO = Mid(v_strLine, 127, 10)
                        v_strS_ACC_NO = Mid(v_strLine, 133, 10)
                        v_row("COL_VALUE11") = v_strS_ACC_NO.Trim
                        v_row("COL_TYPE11") = "C"
                        v_row("COL_DESC11") = v_strS_ACC_NO.Trim

                        'v_strB_CODE_TRADE = Mid(v_strLine, 83, 3)
                        v_strB_CODE_TRADE = Mid(v_strLine, 89, 3)
                        v_row("COL_VALUE14") = v_strB_CODE_TRADE.Trim
                        v_row("COL_TYPE14") = "C"
                        v_row("COL_DESC14") = v_strB_CODE_TRADE.Trim

                        'v_strS_CODE_TRADE = Mid(v_strLine, 86, 3)
                        v_strS_CODE_TRADE = Mid(v_strLine, 92, 3)
                        v_row("COL_VALUE15") = v_strS_CODE_TRADE.Trim
                        v_row("COL_TYPE15") = "C"
                        v_row("COL_DESC15") = v_strS_CODE_TRADE.Trim

                        'v_strB_ORDER_NO = Mid(v_strLine, 7, 8)
                        v_strB_ORDER_NO = Mid(v_strLine, 13, 8)
                        v_row("COL_VALUE16") = v_strB_ORDER_NO.Trim
                        v_row("COL_TYPE16") = "C"
                        v_row("COL_DESC16") = v_strB_ORDER_NO.Trim

                        'v_strS_ORDER_NO = Mid(v_strLine, 25, 8)
                        v_strS_ORDER_NO = Mid(v_strLine, 31, 8)
                        v_row("COL_VALUE17") = v_strS_ORDER_NO.Trim
                        v_row("COL_TYPE17") = "C"
                        v_row("COL_DESC17") = v_strS_ORDER_NO.Trim

                        'v_strB_PC_PLAG = Mid(v_strLine, 81, 1)
                        v_strB_PC_PLAG = Mid(v_strLine, 87, 1)
                        v_row("COL_VALUE18") = v_strB_PC_PLAG.Trim
                        v_row("COL_TYPE18") = "C"
                        v_row("COL_DESC18") = v_strB_PC_PLAG.Trim

                        'v_strS_PC_PLAG = Mid(v_strLine, 82, 1)
                        v_strS_PC_PLAG = Mid(v_strLine, 88, 1)
                        v_row("COL_VALUE19") = v_strS_PC_PLAG.Trim
                        v_row("COL_TYPE19") = "C"
                        v_row("COL_DESC19") = v_strS_PC_PLAG.Trim

                        v_row("COL_VALUE12") = v_strSET_TYPE.Trim
                        v_row("COL_TYPE12") = "N"
                        v_row("COL_DESC12") = v_strSET_TYPE.Trim

                        v_row("COL_VALUE13") = v_strBlock_Tran.Trim
                        v_row("COL_TYPE13") = "N"
                        v_row("COL_DESC13") = v_strBlock_Tran.Trim

                        v_row("TLTXCD") = v_strTLTXCD
                        v_row("REAL_ROW") = v_int

                        v_ds.Tables(0).Rows.Add(v_row)
                        v_int = v_int + 1
                        v_intOffSet = v_intOffSet + 1
                        If v_intOffSet = v_intBatchSize Then
                            If v_obj.SaveUsingOracleBulkCopy("txfields_astdl", v_ds.Tables(0)) Then
                                v_intOffSet = 0
                                v_ds.Tables(0).Rows.Clear()
                            Else
                                Throw New Exception("Error in SaveUsingOracleBulkCopy...")
                            End If
                        End If
                    End If
                End If
            End While
            If v_ds.Tables(0).Rows.Count > 0 Then
                If v_obj.SaveUsingOracleBulkCopy("txfields_astdl", v_ds.Tables(0)) Then
                    v_intOffSet = 0
                    v_ds.Tables(0).Rows.Clear()
                Else
                    Throw New Exception("Error in SaveUsingOracleBulkCopy...")
                End If
            End If
            If v_int > 0 Then
                v_entryNode = pv_oXml.CreateNode(Xml.XmlNodeType.Element, "entry", "")
                'Add field name
                v_attrFLDNAME = pv_oXml.CreateAttribute(gc_AtributeFLDNAME)
                v_attrFLDNAME.Value = "INSERT_ROW"
                v_entryNode.Attributes.Append(v_attrFLDNAME)
                'Add field type
                v_attrDATATYPE = pv_oXml.CreateAttribute(gc_AtributeFLDTYPE)
                v_attrDATATYPE.Value = "C"
                v_entryNode.Attributes.Append(v_attrDATATYPE)
                'Set value
                strRows = "INSERT INTO TMP_TXFIELDS SELECT seq_tmp_txfields.nextval, a.* from txfields_astdl a "
                v_entryNode.InnerText = strRows
                v_dataElement.AppendChild(v_entryNode)

                pv_oNode.AppendChild(v_dataElement)
            End If

            If Not v_Stream Is Nothing Then
                v_Stream.Dispose()
            End If
            v_obj.ExecuteNonQuery(CommandType.Text, "UPDATE sysvar SET varvalue = '3' WHERE grname = 'SYSTEM' AND varname = 'SETT_UPD_ASTDL_STATUS' AND brid = '0002'")
            Return True
        Catch ex As Exception
            v_strErrMsg = "File dữ liệu không hợp lệ!. Chi tiết: " & ex.Message
            v_intErrCount += 1
            Return False
        End Try
    End Function

    'Backup truoc khi sua phan nhan du lieu Hose su dung bang tam (15/03/2021)
    Public Function HOSTReadFileASTPT_1(ByVal pv_strFile As String, ByRef pv_oNode As Xml.XmlNode, ByRef pv_oXml As XmlDocumentEx, ByVal v_strTLTXCD As String, ByVal v_strPARENT_TXNUM As String, ByVal v_strPARENT_TXDATE As String, ByVal v_strPARENT_BUSDATE As String, ByRef v_strErrMsg As String, ByRef v_intErrCount As Integer) As Boolean
        Try
            Dim v_Stream As New System.IO.StreamReader(pv_strFile)
            Dim v_strLine As String
            Dim strRows As String = ""
            Dim v_strConfirmNo, v_strMatch_Date, v_strMatch_Time, v_strSec_Code, v_strSET_TYPE As String
            Dim v_strQty, v_strPrice, v_strB_ACC_NO, v_strS_ACC_NO, v_strBlock_Tran As String
            Dim v_strB_CODE_TRADE, v_strS_CODE_TRADE, v_strB_ORDER_NO, v_strS_ORDER_NO, v_strB_PC_PLAG, v_strS_PC_PLAG As String
            Dim v_entryNode As Xml.XmlNode
            Dim v_attrFLDNAME As Xml.XmlAttribute, v_attrDATATYPE As Xml.XmlAttribute
            Dim v_strStatus As String


            v_strBlock_Tran = "1"
            v_strSET_TYPE = "3"

            Dim strInsertFields, strInsertValues As String
            strInsertFields = ",COL_VALUE01, COL_TYPE01, COL_DESC01,COL_VALUE02, COL_TYPE02 , COL_DESC02,COL_VALUE03, COL_TYPE03 , COL_DESC03"
            strInsertValues = ",'" & v_strPARENT_TXNUM & "'" & " COL_VALUE01" _
            & ", 'C'" & " COL_TYPE01" _
            & ", '" & v_strPARENT_TXNUM & "'" & " COL_DESC01" _
            & ", '" & v_strPARENT_TXDATE & "'" & " COL_VALUE02" _
            & ", 'D'" & " COL_TYPE02" _
            & ", '" & v_strPARENT_TXDATE & "'" & " COL_DESC02" _
            & ", '" & v_strPARENT_BUSDATE & "'" & " COL_VALUE03" _
            & ", 'D'" & " COL_TYPE03" _
            & ", '" & v_strPARENT_BUSDATE & "'" & " COL_DESC03"

            Dim v_strFieldsSQL, v_strValuesSQL As String
            Dim v_int As Integer = 0

            Dim v_dataElement As Xml.XmlElement
            v_dataElement = pv_oXml.CreateElement(Xml.XmlNodeType.Element, "fields", "")
            v_strFieldsSQL = ""
            v_strFieldsSQL &= ",COL_VALUE04,COL_TYPE04,COL_DESC04"
            v_strFieldsSQL &= ",COL_VALUE05,COL_TYPE05,COL_DESC05"
            v_strFieldsSQL &= ",COL_VALUE06,COL_TYPE06,COL_DESC06"
            v_strFieldsSQL &= ",COL_VALUE07,COL_TYPE07,COL_DESC07"
            v_strFieldsSQL &= ",COL_VALUE08,COL_TYPE08,COL_DESC08"
            v_strFieldsSQL &= ",COL_VALUE09,COL_TYPE09,COL_DESC09"
            v_strFieldsSQL &= ",COL_VALUE18,COL_TYPE18,COL_DESC18"
            v_strFieldsSQL &= ",COL_VALUE19,COL_TYPE19,COL_DESC19"
            v_strFieldsSQL &= ",COL_VALUE10,COL_TYPE10,COL_DESC10"
            v_strFieldsSQL &= ",COL_VALUE11,COL_TYPE11,COL_DESC11"
            v_strFieldsSQL &= ",COL_VALUE14,COL_TYPE14,COL_DESC14"
            v_strFieldsSQL &= ",COL_VALUE15,COL_TYPE15,COL_DESC15"
            v_strFieldsSQL &= ",COL_VALUE16,COL_TYPE16,COL_DESC16"
            v_strFieldsSQL &= ",COL_VALUE17,COL_TYPE17,COL_DESC17"
            v_strFieldsSQL &= ",COL_VALUE12,COL_TYPE12,COL_DESC12"
            v_strFieldsSQL &= ",COL_VALUE13,COL_TYPE13,COL_DESC13"

            While Not v_Stream.EndOfStream
                v_strLine = v_Stream.ReadLine
                v_strValuesSQL = ""


                'err 1
                If v_strLine <> "" Then
                    v_strStatus = Trim(Mid(v_strLine, 40, 2))
                    If (v_strStatus <> "XC") And (v_strStatus <> "XS") And (v_strStatus <> "") Then
                    Else
                        'bangpv: check loi
                        v_strErrMsg = "Loi tai: 'err 1: If v_strLine <> "" Then (dong:" & v_int & ")"
                        'end bangpv
                        v_strConfirmNo = String.Format("{0:900000}", CInt(Mid(v_strLine, 1, 6)))

                        v_strValuesSQL &= ",'" & v_strConfirmNo.Trim & "' COL_VALUE04,'C' COL_TYPE04,'" & v_strConfirmNo.Trim & "' COL_DESC04"

                        v_strMatch_Time = Mid(v_strLine, 7, 8)

                        v_strValuesSQL &= ",'" & v_strMatch_Time.Trim & "' COL_VALUE05,'C' COL_TYPE05,'" & v_strMatch_Time.Trim & "' COL_DESC05"

                        v_strMatch_Date = Mid(v_strLine, 15, 10)

                        v_strValuesSQL &= ",'" & v_strMatch_Date & "' COL_VALUE06,'D' COL_TYPE06,'" & v_strMatch_Date & "' COL_DESC06"

                        v_strSec_Code = Mid(v_strLine, 42, 8)

                        v_strValuesSQL &= ",'" & v_strSec_Code.Trim & "' COL_VALUE07,'C' COL_TYPE07,'" & v_strSec_Code.Trim & "' COL_DESC07"

                        v_strPrice = CStr(CDbl(Mid(v_strLine, 50, 13)) * 1000)

                        v_strValuesSQL &= ",'" & v_strPrice.Trim & "' COL_VALUE08,'N' COL_TYPE08,'" & v_strPrice.Trim & "' COL_DESC08"

                        If CDbl("0" & Mid(v_strLine, 96, 8).Trim) > 0 Then
                            v_strQty = Mid(v_strLine, 96, 8)
                            v_strB_PC_PLAG = "P"
                        ElseIf CDbl("0" & Mid(v_strLine, 104, 8).Trim) > 0 Then
                            v_strQty = Mid(v_strLine, 104, 8)
                            v_strB_PC_PLAG = "C"
                        ElseIf CDbl("0" & Mid(v_strLine, 112, 8).Trim) > 0 Then
                            v_strQty = Mid(v_strLine, 112, 8)
                            v_strB_PC_PLAG = "M"
                        ElseIf CDbl("0" & Mid(v_strLine, 120, 8).Trim) > 0 Then
                            v_strQty = Mid(v_strLine, 120, 8)
                            v_strB_PC_PLAG = "F"
                        End If

                        v_strValuesSQL &= ",'" & v_strQty.Trim & "' COL_VALUE09,'N' COL_TYPE09,'" & v_strQty.Trim & "' COL_DESC09"

                        v_strValuesSQL &= ",'" & v_strB_PC_PLAG.Trim & "' COL_VALUE18,'C' COL_TYPE18,'" & v_strB_PC_PLAG.Trim & "' COL_DESC18"


                        If CDbl("0" & Mid(v_strLine, 160, 8).Trim) > 0 Then
                            v_strS_PC_PLAG = "P"
                        ElseIf CDbl("0" & Mid(v_strLine, 168, 8).Trim) > 0 Then
                            v_strS_PC_PLAG = "C"
                        ElseIf CDbl("0" & Mid(v_strLine, 176, 8).Trim) > 0 Then
                            v_strS_PC_PLAG = "M"
                        ElseIf CDbl("0" & Mid(v_strLine, 184, 8).Trim) > 0 Then
                            v_strS_PC_PLAG = "F"
                        End If

                        v_strValuesSQL &= ",'" & v_strS_PC_PLAG.Trim & "' COL_VALUE19,'C' COL_TYPE19,'" & v_strS_PC_PLAG.Trim & "' COL_DESC19"

                        v_strB_ACC_NO = Mid(v_strLine, 63, 10)

                        v_strValuesSQL &= ",'" & v_strB_ACC_NO.Trim & "' COL_VALUE10,'C' COL_TYPE10,'" & v_strB_ACC_NO.Trim & "' COL_DESC10"

                        v_strS_ACC_NO = Mid(v_strLine, 73, 10)

                        v_strValuesSQL &= ",'" & v_strS_ACC_NO.Trim & "' COL_VALUE11,'C' COL_TYPE11,'" & v_strS_ACC_NO.Trim & "' COL_DESC11"

                        v_strB_CODE_TRADE = Mid(v_strLine, 33, 3)

                        v_strValuesSQL &= ",'" & v_strB_CODE_TRADE & "' COL_VALUE14,'C' COL_TYPE14,'" & v_strB_CODE_TRADE & "' COL_DESC14"

                        v_strS_CODE_TRADE = Mid(v_strLine, 36, 3)

                        v_strValuesSQL &= ",'" & v_strS_CODE_TRADE & "' COL_VALUE15,'C' COL_TYPE15,'" & v_strS_CODE_TRADE & "' COL_DESC15"

                        v_strB_ORDER_NO = v_strConfirmNo

                        v_strValuesSQL &= ",'" & v_strConfirmNo.Trim & "' COL_VALUE16,'C' COL_TYPE16,'" & v_strConfirmNo.Trim & "' COL_DESC16"

                        v_strS_ORDER_NO = v_strConfirmNo

                        v_strValuesSQL &= ",'" & v_strConfirmNo.Trim & "' COL_VALUE17,'C' COL_TYPE17,'" & v_strConfirmNo.Trim & "' COL_DESC17"


                        v_strValuesSQL &= ",'" & v_strSET_TYPE.Trim & "' COL_VALUE12,'N' COL_TYPE12,'" & v_strSET_TYPE.Trim & "' COL_DESC12"


                        v_strValuesSQL &= ",'" & v_strBlock_Tran.Trim & "' COL_VALUE13,'N' COL_TYPE13,'" & v_strBlock_Tran.Trim & "' COL_DESC13"

                        strRows = strRows & "UNION ALL SELECT '" & v_strTLTXCD & "' TLTXCD " & v_strValuesSQL & " " & strInsertValues & ", " & v_int + 1 & " as REAL_ROW FROM DUAL " & vbCrLf
                        'err 2
                        If (v_int + 1) Mod gc_IMP_TRAN_UNIT = 0 Then
                            'bangpv: check loi
                            v_strErrMsg = "Loi tai: 'err 2: If (v_int + 1) Mod gc_IMP_TRAN_UNIT = 0 Then (dong:" & v_int & ")"
                            'end bangpv
                            'Append entry to data node
                            v_entryNode = pv_oXml.CreateNode(Xml.XmlNodeType.Element, "entry", "")
                            'Add field name
                            v_attrFLDNAME = pv_oXml.CreateAttribute(gc_AtributeFLDNAME)
                            v_attrFLDNAME.Value = "INSERT_ROW"
                            v_entryNode.Attributes.Append(v_attrFLDNAME)
                            'Add field type
                            v_attrDATATYPE = pv_oXml.CreateAttribute(gc_AtributeFLDTYPE)
                            v_attrDATATYPE.Value = "C"
                            v_entryNode.Attributes.Append(v_attrDATATYPE)
                            'Set value
                            strRows = "INSERT INTO TMP_TXFIELDS(AUTOID, TLTXCD " & v_strFieldsSQL & " " & strInsertFields & ", REAL_ROW ) " _
                                & " SELECT seq_tmp_txfields.nextval , a.* from (" & strRows.Substring(10) & ") a "
                            v_entryNode.InnerText = strRows
                            v_dataElement.AppendChild(v_entryNode)
                            strRows = ""
                        End If
                        v_int = v_int + 1
                    End If
                End If
            End While
            'err 3
            If strRows <> "" Then
                'bangpv: check loi 
                v_strErrMsg = "'err 3:  If strRows <> "" Then (dong: " & v_int & ")"
                'end bangpv
                v_entryNode = pv_oXml.CreateNode(Xml.XmlNodeType.Element, "entry", "")
                'Add field name
                v_attrFLDNAME = pv_oXml.CreateAttribute(gc_AtributeFLDNAME)
                v_attrFLDNAME.Value = "INSERT_ROW"
                v_entryNode.Attributes.Append(v_attrFLDNAME)
                'Add field type
                v_attrDATATYPE = pv_oXml.CreateAttribute(gc_AtributeFLDTYPE)
                v_attrDATATYPE.Value = "C"
                v_entryNode.Attributes.Append(v_attrDATATYPE)
                'Set value
                strRows = "INSERT INTO TMP_TXFIELDS(AUTOID, TLTXCD " & v_strFieldsSQL & " " & strInsertFields & ", REAL_ROW ) " _
                    & " SELECT seq_tmp_txfields.nextval , a.* from (" & strRows.Substring(10) & ") a "
                v_entryNode.InnerText = strRows
                v_dataElement.AppendChild(v_entryNode)
            End If
            If v_int > 0 Then
                pv_oNode.AppendChild(v_dataElement)
            End If
            If Not v_Stream Is Nothing Then
                v_Stream.Dispose()
            End If
            Return True
        Catch ex As Exception
            v_strErrMsg = "File dữ liệu không hợp lệ!. Chi tiết: " & ex.Message & v_strErrMsg
            v_intErrCount += 1
            Return False
        End Try
    End Function
    Public Function HOSTReadFileASTPT_41(ByVal pv_strFile As String, ByRef pv_oNode As Xml.XmlNode, ByRef pv_oXml As XmlDocumentEx, ByVal v_strTLTXCD As String, ByVal v_strPARENT_TXNUM As String, ByVal v_strPARENT_TXDATE As String, ByVal v_strPARENT_BUSDATE As String, ByRef v_strErrMsg As String, ByRef v_intErrCount As Integer) As Boolean
        Try
            Dim v_Stream As New System.IO.StreamReader(pv_strFile)
            Dim v_strLine As String
            Dim strRows As String = ""
            Dim v_strConfirmNo, v_strMatch_Date, v_strMatch_Time, v_strSec_Code, v_strSET_TYPE As String
            Dim v_strQty, v_strPrice, v_strB_ACC_NO, v_strS_ACC_NO, v_strBlock_Tran As String
            Dim v_strB_CODE_TRADE, v_strS_CODE_TRADE, v_strB_ORDER_NO, v_strS_ORDER_NO, v_strB_PC_PLAG, v_strS_PC_PLAG As String
            Dim v_entryNode As Xml.XmlNode
            Dim v_attrFLDNAME As Xml.XmlAttribute, v_attrDATATYPE As Xml.XmlAttribute
            Dim v_strStatus As String


            v_strBlock_Tran = "1"
            v_strSET_TYPE = "3"

            Dim strInsertFields, strInsertValues As String
            strInsertFields = ",COL_VALUE01, COL_TYPE01, COL_DESC01,COL_VALUE02, COL_TYPE02 , COL_DESC02,COL_VALUE03, COL_TYPE03 , COL_DESC03"
            strInsertValues = ",'" & v_strPARENT_TXNUM & "'" & " COL_VALUE01" _
            & ", 'C'" & " COL_TYPE01" _
            & ", '" & v_strPARENT_TXNUM & "'" & " COL_DESC01" _
            & ", '" & v_strPARENT_TXDATE & "'" & " COL_VALUE02" _
            & ", 'D'" & " COL_TYPE02" _
            & ", '" & v_strPARENT_TXDATE & "'" & " COL_DESC02" _
            & ", '" & v_strPARENT_BUSDATE & "'" & " COL_VALUE03" _
            & ", 'D'" & " COL_TYPE03" _
            & ", '" & v_strPARENT_BUSDATE & "'" & " COL_DESC03"

            Dim v_strFieldsSQL, v_strValuesSQL As String
            Dim v_int As Integer = 0

            Dim v_dataElement As Xml.XmlElement
            v_dataElement = pv_oXml.CreateElement(Xml.XmlNodeType.Element, "fields", "")
            v_strFieldsSQL = ""
            v_strFieldsSQL &= ",COL_VALUE04,COL_TYPE04,COL_DESC04"
            v_strFieldsSQL &= ",COL_VALUE05,COL_TYPE05,COL_DESC05"
            v_strFieldsSQL &= ",COL_VALUE06,COL_TYPE06,COL_DESC06"
            v_strFieldsSQL &= ",COL_VALUE07,COL_TYPE07,COL_DESC07"
            v_strFieldsSQL &= ",COL_VALUE08,COL_TYPE08,COL_DESC08"
            v_strFieldsSQL &= ",COL_VALUE09,COL_TYPE09,COL_DESC09"
            v_strFieldsSQL &= ",COL_VALUE18,COL_TYPE18,COL_DESC18"
            v_strFieldsSQL &= ",COL_VALUE19,COL_TYPE19,COL_DESC19"
            v_strFieldsSQL &= ",COL_VALUE10,COL_TYPE10,COL_DESC10"
            v_strFieldsSQL &= ",COL_VALUE11,COL_TYPE11,COL_DESC11"
            v_strFieldsSQL &= ",COL_VALUE14,COL_TYPE14,COL_DESC14"
            v_strFieldsSQL &= ",COL_VALUE15,COL_TYPE15,COL_DESC15"
            v_strFieldsSQL &= ",COL_VALUE16,COL_TYPE16,COL_DESC16"
            v_strFieldsSQL &= ",COL_VALUE17,COL_TYPE17,COL_DESC17"
            v_strFieldsSQL &= ",COL_VALUE12,COL_TYPE12,COL_DESC12"
            v_strFieldsSQL &= ",COL_VALUE13,COL_TYPE13,COL_DESC13"

            While Not v_Stream.EndOfStream
                v_strLine = v_Stream.ReadLine
                v_strValuesSQL = ""
                'err 1
                If v_strLine <> "" Then
                    'v_strStatus = Trim(Mid(v_strLine, 40, 2))
                    v_strStatus = Trim(Mid(v_strLine, 46, 2))
                    If (v_strStatus <> "XC") And (v_strStatus <> "XS") And (v_strStatus <> "") Then
                    Else
                        'bangpv: check loi
                        v_strErrMsg = "Loi tai: 'err 1: If v_strLine <> "" Then (dong:" & v_int & ")"
                        'end bangpv
                        'v_strConfirmNo = String.Format("{0:900000}", CInt(Mid(v_strLine, 1, 6)))
                        v_strConfirmNo = String.Format("{0:900000000000}", CLng(Mid(v_strLine, 1, 12)))

                        v_strValuesSQL &= ",'" & v_strConfirmNo.Trim & "' COL_VALUE04,'C' COL_TYPE04,'" & v_strConfirmNo.Trim & "' COL_DESC04"

                        'v_strMatch_Time = Mid(v_strLine, 7, 8)
                        v_strMatch_Time = Mid(v_strLine, 13, 8)
                        v_strValuesSQL &= ",'" & v_strMatch_Time.Trim & "' COL_VALUE05,'C' COL_TYPE05,'" & v_strMatch_Time.Trim & "' COL_DESC05"

                        'v_strMatch_Date = Mid(v_strLine, 15, 10)
                        v_strMatch_Date = Mid(v_strLine, 21, 10)
                        v_strValuesSQL &= ",'" & v_strMatch_Date & "' COL_VALUE06,'D' COL_TYPE06,'" & v_strMatch_Date & "' COL_DESC06"

                        'v_strSec_Code = Mid(v_strLine, 42, 8)
                        v_strSec_Code = Mid(v_strLine, 48, 8)
                        v_strValuesSQL &= ",'" & v_strSec_Code.Trim & "' COL_VALUE07,'C' COL_TYPE07,'" & v_strSec_Code.Trim & "' COL_DESC07"

                        'v_strPrice = CStr(CDbl(Mid(v_strLine, 50, 13)) * 1000)
                        v_strPrice = CStr(CDbl(Mid(v_strLine, 56, 13)) * 1000)
                        v_strValuesSQL &= ",'" & v_strPrice.Trim & "' COL_VALUE08,'N' COL_TYPE08,'" & v_strPrice.Trim & "' COL_DESC08"

                        'If CDbl("0" & Mid(v_strLine, 96, 8).Trim) > 0 Then
                        '    v_strQty = Mid(v_strLine, 96, 8)
                        '    v_strB_PC_PLAG = "P"
                        'ElseIf CDbl("0" & Mid(v_strLine, 104, 8).Trim) > 0 Then
                        '    v_strQty = Mid(v_strLine, 104, 8)
                        '    v_strB_PC_PLAG = "C"
                        'ElseIf CDbl("0" & Mid(v_strLine, 112, 8).Trim) > 0 Then
                        '    v_strQty = Mid(v_strLine, 112, 8)
                        '    v_strB_PC_PLAG = "M"
                        'ElseIf CDbl("0" & Mid(v_strLine, 120, 8).Trim) > 0 Then
                        '    v_strQty = Mid(v_strLine, 120, 8)
                        '    v_strB_PC_PLAG = "F"
                        'End If
                        If CDbl("0" & Mid(v_strLine, 102, 8).Trim) > 0 Then
                            v_strQty = Mid(v_strLine, 102, 8)
                            v_strB_PC_PLAG = "P"
                        ElseIf CDbl("0" & Mid(v_strLine, 110, 8).Trim) > 0 Then
                            v_strQty = Mid(v_strLine, 110, 8)
                            v_strB_PC_PLAG = "C"
                        ElseIf CDbl("0" & Mid(v_strLine, 118, 8).Trim) > 0 Then
                            v_strQty = Mid(v_strLine, 118, 8)
                            v_strB_PC_PLAG = "M"
                        ElseIf CDbl("0" & Mid(v_strLine, 126, 8).Trim) > 0 Then
                            v_strQty = Mid(v_strLine, 126, 8)
                            v_strB_PC_PLAG = "F"
                        End If

                        v_strValuesSQL &= ",'" & v_strQty.Trim & "' COL_VALUE09,'N' COL_TYPE09,'" & v_strQty.Trim & "' COL_DESC09"

                        v_strValuesSQL &= ",'" & v_strB_PC_PLAG.Trim & "' COL_VALUE18,'C' COL_TYPE18,'" & v_strB_PC_PLAG.Trim & "' COL_DESC18"


                        'If CDbl("0" & Mid(v_strLine, 160, 8).Trim) > 0 Then
                        '    v_strS_PC_PLAG = "P"
                        'ElseIf CDbl("0" & Mid(v_strLine, 168, 8).Trim) > 0 Then
                        '    v_strS_PC_PLAG = "C"
                        'ElseIf CDbl("0" & Mid(v_strLine, 176, 8).Trim) > 0 Then
                        '    v_strS_PC_PLAG = "M"
                        'ElseIf CDbl("0" & Mid(v_strLine, 184, 8).Trim) > 0 Then
                        '    v_strS_PC_PLAG = "F"
                        'End If
                        If CDbl("0" & Mid(v_strLine, 166, 8).Trim) > 0 Then
                            v_strS_PC_PLAG = "P"
                        ElseIf CDbl("0" & Mid(v_strLine, 174, 8).Trim) > 0 Then
                            v_strS_PC_PLAG = "C"
                        ElseIf CDbl("0" & Mid(v_strLine, 182, 8).Trim) > 0 Then
                            v_strS_PC_PLAG = "M"
                        ElseIf CDbl("0" & Mid(v_strLine, 190, 8).Trim) > 0 Then
                            v_strS_PC_PLAG = "F"
                        End If

                        v_strValuesSQL &= ",'" & v_strS_PC_PLAG.Trim & "' COL_VALUE19,'C' COL_TYPE19,'" & v_strS_PC_PLAG.Trim & "' COL_DESC19"

                        'v_strB_ACC_NO = Mid(v_strLine, 63, 10)
                        v_strB_ACC_NO = Mid(v_strLine, 69, 10)
                        v_strValuesSQL &= ",'" & v_strB_ACC_NO.Trim & "' COL_VALUE10,'C' COL_TYPE10,'" & v_strB_ACC_NO.Trim & "' COL_DESC10"

                        'v_strS_ACC_NO = Mid(v_strLine, 73, 10)
                        v_strS_ACC_NO = Mid(v_strLine, 79, 10)
                        v_strValuesSQL &= ",'" & v_strS_ACC_NO.Trim & "' COL_VALUE11,'C' COL_TYPE11,'" & v_strS_ACC_NO.Trim & "' COL_DESC11"

                        'v_strB_CODE_TRADE = Mid(v_strLine, 33, 3)
                        v_strB_CODE_TRADE = Mid(v_strLine, 39, 3)
                        v_strValuesSQL &= ",'" & v_strB_CODE_TRADE & "' COL_VALUE14,'C' COL_TYPE14,'" & v_strB_CODE_TRADE & "' COL_DESC14"

                        'v_strS_CODE_TRADE = Mid(v_strLine, 36, 3)
                        v_strS_CODE_TRADE = Mid(v_strLine, 42, 3)
                        v_strValuesSQL &= ",'" & v_strS_CODE_TRADE & "' COL_VALUE15,'C' COL_TYPE15,'" & v_strS_CODE_TRADE & "' COL_DESC15"

                        v_strB_ORDER_NO = v_strConfirmNo

                        v_strValuesSQL &= ",'" & v_strConfirmNo.Trim & "' COL_VALUE16,'C' COL_TYPE16,'" & v_strConfirmNo.Trim & "' COL_DESC16"

                        v_strS_ORDER_NO = v_strConfirmNo

                        v_strValuesSQL &= ",'" & v_strConfirmNo.Trim & "' COL_VALUE17,'C' COL_TYPE17,'" & v_strConfirmNo.Trim & "' COL_DESC17"


                        v_strValuesSQL &= ",'" & v_strSET_TYPE.Trim & "' COL_VALUE12,'N' COL_TYPE12,'" & v_strSET_TYPE.Trim & "' COL_DESC12"


                        v_strValuesSQL &= ",'" & v_strBlock_Tran.Trim & "' COL_VALUE13,'N' COL_TYPE13,'" & v_strBlock_Tran.Trim & "' COL_DESC13"

                        strRows = strRows & "UNION ALL SELECT '" & v_strTLTXCD & "' TLTXCD " & v_strValuesSQL & " " & strInsertValues & ", " & v_int + 1 & " as REAL_ROW FROM DUAL " & vbCrLf
                        'err 2
                        If (v_int + 1) Mod gc_IMP_TRAN_UNIT = 0 Then
                            'bangpv: check loi
                            v_strErrMsg = "Loi tai: 'err 2: If (v_int + 1) Mod gc_IMP_TRAN_UNIT = 0 Then (dong:" & v_int & ")"
                            'end bangpv
                            'Append entry to data node
                            v_entryNode = pv_oXml.CreateNode(Xml.XmlNodeType.Element, "entry", "")
                            'Add field name
                            v_attrFLDNAME = pv_oXml.CreateAttribute(gc_AtributeFLDNAME)
                            v_attrFLDNAME.Value = "INSERT_ROW"
                            v_entryNode.Attributes.Append(v_attrFLDNAME)
                            'Add field type
                            v_attrDATATYPE = pv_oXml.CreateAttribute(gc_AtributeFLDTYPE)
                            v_attrDATATYPE.Value = "C"
                            v_entryNode.Attributes.Append(v_attrDATATYPE)
                            'Set value
                            strRows = "INSERT INTO TMP_TXFIELDS(AUTOID, TLTXCD " & v_strFieldsSQL & " " & strInsertFields & ", REAL_ROW ) " _
                                & " SELECT seq_tmp_txfields.nextval , a.* from (" & strRows.Substring(10) & ") a "
                            v_entryNode.InnerText = strRows
                            v_dataElement.AppendChild(v_entryNode)
                            strRows = ""
                        End If
                        v_int = v_int + 1
                    End If
                End If
            End While
            'err 3
            If strRows <> "" Then
                'bangpv: check loi 
                v_strErrMsg = "'err 3:  If strRows <> "" Then (dong: " & v_int & ")"
                'end bangpv
                v_entryNode = pv_oXml.CreateNode(Xml.XmlNodeType.Element, "entry", "")
                'Add field name
                v_attrFLDNAME = pv_oXml.CreateAttribute(gc_AtributeFLDNAME)
                v_attrFLDNAME.Value = "INSERT_ROW"
                v_entryNode.Attributes.Append(v_attrFLDNAME)
                'Add field type
                v_attrDATATYPE = pv_oXml.CreateAttribute(gc_AtributeFLDTYPE)
                v_attrDATATYPE.Value = "C"
                v_entryNode.Attributes.Append(v_attrDATATYPE)
                'Set value
                strRows = "INSERT INTO TMP_TXFIELDS(AUTOID, TLTXCD " & v_strFieldsSQL & " " & strInsertFields & ", REAL_ROW ) " _
                    & " SELECT seq_tmp_txfields.nextval , a.* from (" & strRows.Substring(10) & ") a "
                v_entryNode.InnerText = strRows
                v_dataElement.AppendChild(v_entryNode)
            End If
            If v_int > 0 Then
                pv_oNode.AppendChild(v_dataElement)
            End If
            If Not v_Stream Is Nothing Then
                v_Stream.Dispose()
            End If
            Return True
        Catch ex As Exception
            v_strErrMsg = "File dữ liệu không hợp lệ!. Chi tiết: " & ex.Message & v_strErrMsg
            v_intErrCount += 1
            Return False
        End Try
    End Function
    Public Function HOSTReadFileASTPT_43(ByVal pv_strFile As String, ByRef pv_oNode As Xml.XmlNode, ByRef pv_oXml As XmlDocumentEx, ByVal v_strTLTXCD As String, ByVal v_strPARENT_TXNUM As String, ByVal v_strPARENT_TXDATE As String, ByVal v_strPARENT_BUSDATE As String, ByRef v_strErrMsg As String, ByRef v_intErrCount As Integer) As Boolean
        Try
            Dim strRows As String = ""
            Dim v_entryNode As Xml.XmlNode
            Dim v_attrFLDNAME As Xml.XmlAttribute, v_attrDATATYPE As Xml.XmlAttribute

            Dim v_obj As New DataAccess
            v_obj.NewDBInstance(gc_MODULE_HOST)
            Dim v_strSql = "UPDATE txfields_astpt SET COL_VALUE01='" & v_strPARENT_TXNUM & "', COL_DESC01='" & v_strPARENT_TXNUM & "', " _
                            & "COL_VALUE02='" & v_strPARENT_TXDATE & "', COL_DESC02='" & v_strPARENT_TXDATE & "', " _
                            & "COL_VALUE03='" & v_strPARENT_BUSDATE & "', COL_DESC03='" & v_strPARENT_BUSDATE & "'"

            v_obj.ExecuteNonQuery(CommandType.Text, v_strSql)
            Dim v_ds As DataSet = v_obj.ExecuteSQLReturnDataset(CommandType.Text, "SELECT COUNT(*) FROM txfields_astpt")
            Dim v_int As Integer = CInt(v_ds.Tables(0).Rows(0)(0).ToString())

            Dim v_dataElement As Xml.XmlElement
            v_dataElement = pv_oXml.CreateElement(Xml.XmlNodeType.Element, "fields", "")


            If v_int > 0 Then
                v_entryNode = pv_oXml.CreateNode(Xml.XmlNodeType.Element, "entry", "")
                'Add field name
                v_attrFLDNAME = pv_oXml.CreateAttribute(gc_AtributeFLDNAME)
                v_attrFLDNAME.Value = "INSERT_ROW"
                v_entryNode.Attributes.Append(v_attrFLDNAME)
                'Add field type
                v_attrDATATYPE = pv_oXml.CreateAttribute(gc_AtributeFLDTYPE)
                v_attrDATATYPE.Value = "C"
                v_entryNode.Attributes.Append(v_attrDATATYPE)
                'Set value
                strRows = "INSERT INTO TMP_TXFIELDS SELECT seq_tmp_txfields.nextval, a.* from txfields_astpt a "
                v_entryNode.InnerText = strRows
                v_dataElement.AppendChild(v_entryNode)
                pv_oNode.AppendChild(v_dataElement)
            End If
            Return True
        Catch ex As Exception
            v_strErrMsg = "File dữ liệu không hợp lệ!. Chi tiết: " & ex.Message & v_strErrMsg
            v_intErrCount += 1
            Return False
        End Try
    End Function

    Public Function HOSTReadFileASTPT_3(ByVal pv_strFile As String, ByRef pv_oNode As Xml.XmlNode, ByRef pv_oXml As XmlDocumentEx, ByVal v_strTLTXCD As String, ByVal v_strPARENT_TXNUM As String, ByVal v_strPARENT_TXDATE As String, ByVal v_strPARENT_BUSDATE As String, ByRef v_strErrMsg As String, ByRef v_intErrCount As Integer) As Boolean
        Try
            Dim strRows As String = ""
            Dim v_entryNode As Xml.XmlNode
            Dim v_attrFLDNAME As Xml.XmlAttribute, v_attrDATATYPE As Xml.XmlAttribute

            Dim v_obj As New DataAccess
            v_obj.NewDBInstance(gc_MODULE_HOST)
            Dim v_strSql = "UPDATE txfields_astpt SET COL_VALUE01='" & v_strPARENT_TXNUM & "', COL_DESC01='" & v_strPARENT_TXNUM & "', " _
                            & "COL_VALUE02='" & v_strPARENT_TXDATE & "', COL_DESC02='" & v_strPARENT_TXDATE & "', " _
                            & "COL_VALUE03='" & v_strPARENT_BUSDATE & "', COL_DESC03='" & v_strPARENT_BUSDATE & "'"

            v_obj.ExecuteNonQuery(CommandType.Text, v_strSql)
            Dim v_ds As DataSet = v_obj.ExecuteSQLReturnDataset(CommandType.Text, "SELECT COUNT(*) FROM txfields_astpt")
            Dim v_int As Integer = CInt(v_ds.Tables(0).Rows(0)(0).ToString())

            Dim v_dataElement As Xml.XmlElement
            v_dataElement = pv_oXml.CreateElement(Xml.XmlNodeType.Element, "fields", "")


            If v_int > 0 Then
                v_entryNode = pv_oXml.CreateNode(Xml.XmlNodeType.Element, "entry", "")
                'Add field name
                v_attrFLDNAME = pv_oXml.CreateAttribute(gc_AtributeFLDNAME)
                v_attrFLDNAME.Value = "INSERT_ROW"
                v_entryNode.Attributes.Append(v_attrFLDNAME)
                'Add field type
                v_attrDATATYPE = pv_oXml.CreateAttribute(gc_AtributeFLDTYPE)
                v_attrDATATYPE.Value = "C"
                v_entryNode.Attributes.Append(v_attrDATATYPE)
                'Set value
                strRows = "INSERT INTO TMP_TXFIELDS SELECT seq_tmp_txfields.nextval, a.* from txfields_astpt a "
                v_entryNode.InnerText = strRows
                v_dataElement.AppendChild(v_entryNode)
                pv_oNode.AppendChild(v_dataElement)
            End If
            Return True
        Catch ex As Exception
            v_strErrMsg = "File dữ liệu không hợp lệ!. Chi tiết: " & ex.Message & v_strErrMsg
            v_intErrCount += 1
            Return False
        End Try
    End Function

    'Backup truoc khi sua phan nhan du lieu Hose su dung cong cu ben ngoai
    Public Function HOSTReadFileASTPT_2(ByVal pv_strFile As String, ByRef pv_oNode As Xml.XmlNode, ByRef pv_oXml As XmlDocumentEx, ByVal v_strTLTXCD As String, ByVal v_strPARENT_TXNUM As String, ByVal v_strPARENT_TXDATE As String, ByVal v_strPARENT_BUSDATE As String, ByRef v_strErrMsg As String, ByRef v_intErrCount As Integer) As Boolean
        Try
            Dim v_Stream As New System.IO.StreamReader(pv_strFile)
            Dim v_strLine As String
            Dim strRows As String = ""
            Dim v_strConfirmNo, v_strMatch_Date, v_strMatch_Time, v_strSec_Code, v_strSET_TYPE As String
            Dim v_strQty, v_strPrice, v_strB_ACC_NO, v_strS_ACC_NO, v_strBlock_Tran As String
            Dim v_strB_CODE_TRADE, v_strS_CODE_TRADE, v_strB_ORDER_NO, v_strS_ORDER_NO, v_strB_PC_PLAG, v_strS_PC_PLAG As String
            Dim v_entryNode As Xml.XmlNode
            Dim v_attrFLDNAME As Xml.XmlAttribute, v_attrDATATYPE As Xml.XmlAttribute
            Dim v_strStatus As String
            Dim v_dataElement As Xml.XmlElement
            v_dataElement = pv_oXml.CreateElement(Xml.XmlNodeType.Element, "fields", "")


            v_strBlock_Tran = "1"
            v_strSET_TYPE = "3"
            Dim v_obj As New DataAccess
            v_obj.NewDBInstance(gc_MODULE_HOST)
            Dim v_strSettUpdAstptStatus As String
            v_obj.GetSysVar("SYSTEM", "SETT_UPD_ASTPT_STATUS", "0002", v_strSettUpdAstptStatus)
            If v_strSettUpdAstptStatus = "3" Then
                v_entryNode = pv_oXml.CreateNode(Xml.XmlNodeType.Element, "entry", "")
                'Add field name
                v_attrFLDNAME = pv_oXml.CreateAttribute(gc_AtributeFLDNAME)
                v_attrFLDNAME.Value = "INSERT_ROW"
                v_entryNode.Attributes.Append(v_attrFLDNAME)
                'Add field type
                v_attrDATATYPE = pv_oXml.CreateAttribute(gc_AtributeFLDTYPE)
                v_attrDATATYPE.Value = "C"
                v_entryNode.Attributes.Append(v_attrDATATYPE)
                'Set value
                strRows = "INSERT INTO TMP_TXFIELDS SELECT seq_tmp_txfields.nextval, a.* from txfields_astpt a "
                v_entryNode.InnerText = strRows
                v_dataElement.AppendChild(v_entryNode)

                pv_oNode.AppendChild(v_dataElement)
                Return True
            End If
            v_obj.ExecuteNonQuery(CommandType.Text, "UPDATE sysvar SET varvalue = '1' WHERE grname = 'SYSTEM' AND varname = 'SETT_UPD_ASTPT_STATUS' AND brid = '0002'")
            v_obj.ExecuteNonQuery(CommandType.Text, "TRUNCATE TABLE txfields_astpt")
            Dim v_ds As DataSet = v_obj.ExecuteSQLReturnDataset(CommandType.Text, "SELECT * FROM txfields_astpt WHERE 0=1")

            Dim v_int As Integer = 0
            Dim v_intBatchSize As Integer = 0
            Dim v_strTmp As String = "0"
            v_obj.GetSysVar("SYSTEM", "SETT_UPD_BATCH_SIZE", "0002", v_strTmp)
            If v_strTmp = "" Or v_strTmp = "0" Then
                v_intBatchSize = 5000
            Else
                v_intBatchSize = CInt(v_strTmp)
            End If

            Dim v_intOffSet As Integer = 0

            While Not v_Stream.EndOfStream
                v_strLine = v_Stream.ReadLine
                'err 1
                If v_strLine <> "" Then
                    v_strStatus = Trim(Mid(v_strLine, 40, 2))
                    If (v_strStatus <> "XC") And (v_strStatus <> "XS") And (v_strStatus <> "") Then
                    Else
                        Dim v_row As DataRow = v_ds.Tables(0).NewRow()
                        v_row("COL_VALUE01") = v_strPARENT_TXNUM
                        v_row("COL_TYPE01") = "C"
                        v_row("COL_DESC01") = v_strPARENT_TXNUM
                        v_row("COL_VALUE02") = v_strPARENT_TXDATE
                        v_row("COL_TYPE02") = "D"
                        v_row("COL_DESC02") = v_strPARENT_TXDATE
                        v_row("COL_VALUE03") = v_strPARENT_BUSDATE
                        v_row("COL_TYPE03") = "D"
                        v_row("COL_DESC03") = v_strPARENT_BUSDATE
                        'bangpv: check loi
                        v_strErrMsg = "Loi tai: 'err 1: If v_strLine <> "" Then (dong:" & v_int & ")"
                        'end bangpv
                        v_strConfirmNo = String.Format("{0:900000}", CInt(Mid(v_strLine, 1, 6)))
                        v_row("COL_VALUE04") = v_strConfirmNo.Trim
                        v_row("COL_TYPE04") = "C"
                        v_row("COL_DESC04") = v_strConfirmNo.Trim

                        v_strMatch_Time = Mid(v_strLine, 7, 8)
                        v_row("COL_VALUE05") = v_strMatch_Time.Trim
                        v_row("COL_TYPE05") = "C"
                        v_row("COL_DESC05") = v_strMatch_Time.Trim

                        v_strMatch_Date = Mid(v_strLine, 15, 10)
                        v_row("COL_VALUE06") = v_strMatch_Date.Trim
                        v_row("COL_TYPE06") = "D"
                        v_row("COL_DESC06") = v_strMatch_Date.Trim

                        v_strSec_Code = Mid(v_strLine, 42, 8)
                        v_row("COL_VALUE07") = v_strSec_Code.Trim
                        v_row("COL_TYPE07") = "C"
                        v_row("COL_DESC07") = v_strSec_Code.Trim

                        v_strPrice = CStr(CDbl(Mid(v_strLine, 50, 13)) * 1000)
                        v_row("COL_VALUE08") = v_strPrice.Trim
                        v_row("COL_TYPE08") = "N"
                        v_row("COL_DESC08") = v_strPrice.Trim

                        If CDbl("0" & Mid(v_strLine, 96, 8).Trim) > 0 Then
                            v_strQty = Mid(v_strLine, 96, 8)
                            v_strB_PC_PLAG = "P"
                        ElseIf CDbl("0" & Mid(v_strLine, 104, 8).Trim) > 0 Then
                            v_strQty = Mid(v_strLine, 104, 8)
                            v_strB_PC_PLAG = "C"
                        ElseIf CDbl("0" & Mid(v_strLine, 112, 8).Trim) > 0 Then
                            v_strQty = Mid(v_strLine, 112, 8)
                            v_strB_PC_PLAG = "M"
                        ElseIf CDbl("0" & Mid(v_strLine, 120, 8).Trim) > 0 Then
                            v_strQty = Mid(v_strLine, 120, 8)
                            v_strB_PC_PLAG = "F"
                        End If

                        v_row("COL_VALUE09") = v_strQty.Trim
                        v_row("COL_TYPE09") = "N"
                        v_row("COL_DESC09") = v_strQty.Trim

                        v_row("COL_VALUE18") = v_strB_PC_PLAG.Trim
                        v_row("COL_TYPE18") = "C"
                        v_row("COL_DESC18") = v_strB_PC_PLAG.Trim

                        If CDbl("0" & Mid(v_strLine, 160, 8).Trim) > 0 Then
                            v_strS_PC_PLAG = "P"
                        ElseIf CDbl("0" & Mid(v_strLine, 168, 8).Trim) > 0 Then
                            v_strS_PC_PLAG = "C"
                        ElseIf CDbl("0" & Mid(v_strLine, 176, 8).Trim) > 0 Then
                            v_strS_PC_PLAG = "M"
                        ElseIf CDbl("0" & Mid(v_strLine, 184, 8).Trim) > 0 Then
                            v_strS_PC_PLAG = "F"
                        End If
                        v_row("COL_VALUE19") = v_strS_PC_PLAG.Trim
                        v_row("COL_TYPE19") = "C"
                        v_row("COL_DESC19") = v_strS_PC_PLAG.Trim

                        v_strB_ACC_NO = Mid(v_strLine, 63, 10)
                        v_row("COL_VALUE10") = v_strB_ACC_NO.Trim
                        v_row("COL_TYPE10") = "C"
                        v_row("COL_DESC10") = v_strB_ACC_NO.Trim

                        v_strS_ACC_NO = Mid(v_strLine, 73, 10)
                        v_row("COL_VALUE11") = v_strS_ACC_NO.Trim
                        v_row("COL_TYPE11") = "C"
                        v_row("COL_DESC11") = v_strS_ACC_NO.Trim

                        v_strB_CODE_TRADE = Mid(v_strLine, 33, 3)
                        v_row("COL_VALUE14") = v_strB_CODE_TRADE.Trim
                        v_row("COL_TYPE14") = "C"
                        v_row("COL_DESC14") = v_strB_CODE_TRADE.Trim

                        v_strS_CODE_TRADE = Mid(v_strLine, 36, 3)
                        v_row("COL_VALUE15") = v_strS_CODE_TRADE.Trim
                        v_row("COL_TYPE15") = "C"
                        v_row("COL_DESC15") = v_strS_CODE_TRADE.Trim


                        v_strB_ORDER_NO = v_strConfirmNo
                        v_row("COL_VALUE16") = v_strB_ORDER_NO.Trim
                        v_row("COL_TYPE16") = "C"
                        v_row("COL_DESC16") = v_strB_ORDER_NO.Trim

                        v_strS_ORDER_NO = v_strConfirmNo
                        v_row("COL_VALUE17") = v_strS_ORDER_NO.Trim
                        v_row("COL_TYPE17") = "C"
                        v_row("COL_DESC17") = v_strS_ORDER_NO.Trim

                        v_row("COL_VALUE12") = v_strSET_TYPE.Trim
                        v_row("COL_TYPE12") = "N"
                        v_row("COL_DESC12") = v_strSET_TYPE.Trim

                        v_row("COL_VALUE13") = v_strBlock_Tran.Trim
                        v_row("COL_TYPE13") = "N"
                        v_row("COL_DESC13") = v_strBlock_Tran.Trim

                        v_row("TLTXCD") = v_strTLTXCD
                        v_row("REAL_ROW") = v_int

                        v_ds.Tables(0).Rows.Add(v_row)
                        v_int = v_int + 1
                        v_intOffSet = v_intOffSet + 1
                        If v_intOffSet = v_intBatchSize Then
                            If v_obj.SaveUsingOracleBulkCopy("txfields_astpt", v_ds.Tables(0)) Then
                                v_intOffSet = 0
                                v_ds.Tables(0).Rows.Clear()
                            Else
                                Throw New Exception("Error in SaveUsingOracleBulkCopy...")
                            End If
                        End If
                    End If
                End If
            End While
            'err 3
            If v_ds.Tables(0).Rows.Count > 0 Then
                If v_obj.SaveUsingOracleBulkCopy("txfields_astpt", v_ds.Tables(0)) Then
                    v_intOffSet = 0
                    v_ds.Tables(0).Rows.Clear()
                Else
                    Throw New Exception("Error in SaveUsingOracleBulkCopy...")
                End If
            End If
            If v_int > 0 Then
                'bangpv: check loi 
                v_strErrMsg = "'err 3:  If v_ds.Tables(0).Rows.Count > 0 Then (dong: " & v_int & ")"
                'end bangpv
                v_entryNode = pv_oXml.CreateNode(Xml.XmlNodeType.Element, "entry", "")
                'Add field name
                v_attrFLDNAME = pv_oXml.CreateAttribute(gc_AtributeFLDNAME)
                v_attrFLDNAME.Value = "INSERT_ROW"
                v_entryNode.Attributes.Append(v_attrFLDNAME)
                'Add field type
                v_attrDATATYPE = pv_oXml.CreateAttribute(gc_AtributeFLDTYPE)
                v_attrDATATYPE.Value = "C"
                v_entryNode.Attributes.Append(v_attrDATATYPE)
                'Set value
                strRows = "INSERT INTO TMP_TXFIELDS SELECT seq_tmp_txfields.nextval, a.* from txfields_astpt a "
                v_entryNode.InnerText = strRows
                v_dataElement.AppendChild(v_entryNode)

                pv_oNode.AppendChild(v_dataElement)
            End If
            If Not v_Stream Is Nothing Then
                v_Stream.Dispose()
            End If
            v_obj.ExecuteNonQuery(CommandType.Text, "UPDATE sysvar SET varvalue = '3' WHERE grname = 'SYSTEM' AND varname = 'SETT_UPD_ASTPT_STATUS' AND brid = '0002'")
            Return True
        Catch ex As Exception
            v_strErrMsg = "File dữ liệu không hợp lệ!. Chi tiết: " & ex.Message & v_strErrMsg
            v_intErrCount += 1
            Return False
        End Try
    End Function
    Public Function HOSTReadFileASTPT_42(ByVal pv_strFile As String, ByRef pv_oNode As Xml.XmlNode, ByRef pv_oXml As XmlDocumentEx, ByVal v_strTLTXCD As String, ByVal v_strPARENT_TXNUM As String, ByVal v_strPARENT_TXDATE As String, ByVal v_strPARENT_BUSDATE As String, ByRef v_strErrMsg As String, ByRef v_intErrCount As Integer) As Boolean
        Try
            Dim v_Stream As New System.IO.StreamReader(pv_strFile)
            Dim v_strLine As String
            Dim strRows As String = ""
            Dim v_strConfirmNo, v_strMatch_Date, v_strMatch_Time, v_strSec_Code, v_strSET_TYPE As String
            Dim v_strQty, v_strPrice, v_strB_ACC_NO, v_strS_ACC_NO, v_strBlock_Tran As String
            Dim v_strB_CODE_TRADE, v_strS_CODE_TRADE, v_strB_ORDER_NO, v_strS_ORDER_NO, v_strB_PC_PLAG, v_strS_PC_PLAG As String
            Dim v_entryNode As Xml.XmlNode
            Dim v_attrFLDNAME As Xml.XmlAttribute, v_attrDATATYPE As Xml.XmlAttribute
            Dim v_strStatus As String
            Dim v_dataElement As Xml.XmlElement
            v_dataElement = pv_oXml.CreateElement(Xml.XmlNodeType.Element, "fields", "")

            v_strBlock_Tran = "1"
            v_strSET_TYPE = "3"
            Dim v_obj As New DataAccess
            v_obj.NewDBInstance(gc_MODULE_HOST)
            Dim v_strSettUpdAstptStatus As String
            v_obj.GetSysVar("SYSTEM", "SETT_UPD_ASTPT_STATUS", "0002", v_strSettUpdAstptStatus)
            If v_strSettUpdAstptStatus = "3" Then
                v_entryNode = pv_oXml.CreateNode(Xml.XmlNodeType.Element, "entry", "")
                'Add field name
                v_attrFLDNAME = pv_oXml.CreateAttribute(gc_AtributeFLDNAME)
                v_attrFLDNAME.Value = "INSERT_ROW"
                v_entryNode.Attributes.Append(v_attrFLDNAME)
                'Add field type
                v_attrDATATYPE = pv_oXml.CreateAttribute(gc_AtributeFLDTYPE)
                v_attrDATATYPE.Value = "C"
                v_entryNode.Attributes.Append(v_attrDATATYPE)
                'Set value
                strRows = "INSERT INTO TMP_TXFIELDS SELECT seq_tmp_txfields.nextval, a.* from txfields_astpt a "
                v_entryNode.InnerText = strRows
                v_dataElement.AppendChild(v_entryNode)

                pv_oNode.AppendChild(v_dataElement)
                Return True
            End If
            v_obj.ExecuteNonQuery(CommandType.Text, "UPDATE sysvar SET varvalue = '1' WHERE grname = 'SYSTEM' AND varname = 'SETT_UPD_ASTPT_STATUS' AND brid = '0002'")
            v_obj.ExecuteNonQuery(CommandType.Text, "TRUNCATE TABLE txfields_astpt")
            Dim v_ds As DataSet = v_obj.ExecuteSQLReturnDataset(CommandType.Text, "SELECT * FROM txfields_astpt WHERE 0=1")

            Dim v_int As Integer = 0
            Dim v_intBatchSize As Integer = 0
            Dim v_strTmp As String = "0"
            v_obj.GetSysVar("SYSTEM", "SETT_UPD_BATCH_SIZE", "0002", v_strTmp)
            If v_strTmp = "" Or v_strTmp = "0" Then
                v_intBatchSize = 5000
            Else
                v_intBatchSize = CInt(v_strTmp)
            End If

            Dim v_intOffSet As Integer = 0

            While Not v_Stream.EndOfStream
                v_strLine = v_Stream.ReadLine
                'err 1
                If v_strLine <> "" Then
                    'v_strStatus = Trim(Mid(v_strLine, 40, 2))
                    v_strStatus = Trim(Mid(v_strLine, 46, 2))
                    If (v_strStatus <> "XC") And (v_strStatus <> "XS") And (v_strStatus <> "") Then
                    Else
                        Dim v_row As DataRow = v_ds.Tables(0).NewRow()
                        v_row("COL_VALUE01") = v_strPARENT_TXNUM
                        v_row("COL_TYPE01") = "C"
                        v_row("COL_DESC01") = v_strPARENT_TXNUM
                        v_row("COL_VALUE02") = v_strPARENT_TXDATE
                        v_row("COL_TYPE02") = "D"
                        v_row("COL_DESC02") = v_strPARENT_TXDATE
                        v_row("COL_VALUE03") = v_strPARENT_BUSDATE
                        v_row("COL_TYPE03") = "D"
                        v_row("COL_DESC03") = v_strPARENT_BUSDATE
                        'bangpv: check loi
                        v_strErrMsg = "Loi tai: 'err 1: If v_strLine <> "" Then (dong:" & v_int & ")"
                        'end bangpv
                        'v_strConfirmNo = String.Format("{0:900000}", CInt(Mid(v_strLine, 1, 6)))
                        v_strConfirmNo = String.Format("{0:900000000000}", CLng(Mid(v_strLine, 1, 12)))
                        v_row("COL_VALUE04") = v_strConfirmNo.Trim
                        v_row("COL_TYPE04") = "C"
                        v_row("COL_DESC04") = v_strConfirmNo.Trim

                        'v_strMatch_Time = Mid(v_strLine, 7, 8)
                        v_strMatch_Time = Mid(v_strLine, 13, 8)
                        v_row("COL_VALUE05") = v_strMatch_Time.Trim
                        v_row("COL_TYPE05") = "C"
                        v_row("COL_DESC05") = v_strMatch_Time.Trim

                        'v_strMatch_Date = Mid(v_strLine, 15, 10)
                        v_strMatch_Date = Mid(v_strLine, 21, 10)
                        v_row("COL_VALUE06") = v_strMatch_Date.Trim
                        v_row("COL_TYPE06") = "D"
                        v_row("COL_DESC06") = v_strMatch_Date.Trim

                        'v_strSec_Code = Mid(v_strLine, 42, 8)
                        v_strSec_Code = Mid(v_strLine, 48, 8)
                        v_row("COL_VALUE07") = v_strSec_Code.Trim
                        v_row("COL_TYPE07") = "C"
                        v_row("COL_DESC07") = v_strSec_Code.Trim

                        'v_strPrice = CStr(CDbl(Mid(v_strLine, 50, 13)) * 1000)
                        v_strPrice = CStr(CDbl(Mid(v_strLine, 56, 13)) * 1000)
                        v_row("COL_VALUE08") = v_strPrice.Trim
                        v_row("COL_TYPE08") = "N"
                        v_row("COL_DESC08") = v_strPrice.Trim

                        'If CDbl("0" & Mid(v_strLine, 96, 8).Trim) > 0 Then
                        '    v_strQty = Mid(v_strLine, 96, 8)
                        '    v_strB_PC_PLAG = "P"
                        'ElseIf CDbl("0" & Mid(v_strLine, 104, 8).Trim) > 0 Then
                        '    v_strQty = Mid(v_strLine, 104, 8)
                        '    v_strB_PC_PLAG = "C"
                        'ElseIf CDbl("0" & Mid(v_strLine, 112, 8).Trim) > 0 Then
                        '    v_strQty = Mid(v_strLine, 112, 8)
                        '    v_strB_PC_PLAG = "M"
                        'ElseIf CDbl("0" & Mid(v_strLine, 120, 8).Trim) > 0 Then
                        '    v_strQty = Mid(v_strLine, 120, 8)
                        '    v_strB_PC_PLAG = "F"
                        'End If
                        If CDbl("0" & Mid(v_strLine, 102, 8).Trim) > 0 Then
                            v_strQty = Mid(v_strLine, 102, 8)
                            v_strB_PC_PLAG = "P"
                        ElseIf CDbl("0" & Mid(v_strLine, 110, 8).Trim) > 0 Then
                            v_strQty = Mid(v_strLine, 110, 8)
                            v_strB_PC_PLAG = "C"
                        ElseIf CDbl("0" & Mid(v_strLine, 118, 8).Trim) > 0 Then
                            v_strQty = Mid(v_strLine, 118, 8)
                            v_strB_PC_PLAG = "M"
                        ElseIf CDbl("0" & Mid(v_strLine, 126, 8).Trim) > 0 Then
                            v_strQty = Mid(v_strLine, 126, 8)
                            v_strB_PC_PLAG = "F"
                        End If

                        v_row("COL_VALUE09") = v_strQty.Trim
                        v_row("COL_TYPE09") = "N"
                        v_row("COL_DESC09") = v_strQty.Trim

                        v_row("COL_VALUE18") = v_strB_PC_PLAG.Trim
                        v_row("COL_TYPE18") = "C"
                        v_row("COL_DESC18") = v_strB_PC_PLAG.Trim

                        'If CDbl("0" & Mid(v_strLine, 160, 8).Trim) > 0 Then
                        '    v_strS_PC_PLAG = "P"
                        'ElseIf CDbl("0" & Mid(v_strLine, 168, 8).Trim) > 0 Then
                        '    v_strS_PC_PLAG = "C"
                        'ElseIf CDbl("0" & Mid(v_strLine, 176, 8).Trim) > 0 Then
                        '    v_strS_PC_PLAG = "M"
                        'ElseIf CDbl("0" & Mid(v_strLine, 184, 8).Trim) > 0 Then
                        '    v_strS_PC_PLAG = "F"
                        'End If

                        If CDbl("0" & Mid(v_strLine, 166, 8).Trim) > 0 Then
                            v_strS_PC_PLAG = "P"
                        ElseIf CDbl("0" & Mid(v_strLine, 174, 8).Trim) > 0 Then
                            v_strS_PC_PLAG = "C"
                        ElseIf CDbl("0" & Mid(v_strLine, 182, 8).Trim) > 0 Then
                            v_strS_PC_PLAG = "M"
                        ElseIf CDbl("0" & Mid(v_strLine, 190, 8).Trim) > 0 Then
                            v_strS_PC_PLAG = "F"
                        End If
                        v_row("COL_VALUE19") = v_strS_PC_PLAG.Trim
                        v_row("COL_TYPE19") = "C"
                        v_row("COL_DESC19") = v_strS_PC_PLAG.Trim

                        'v_strB_ACC_NO = Mid(v_strLine, 63, 10)
                        v_strB_ACC_NO = Mid(v_strLine, 69, 10)
                        v_row("COL_VALUE10") = v_strB_ACC_NO.Trim
                        v_row("COL_TYPE10") = "C"
                        v_row("COL_DESC10") = v_strB_ACC_NO.Trim

                        'v_strS_ACC_NO = Mid(v_strLine, 73, 10)
                        v_strS_ACC_NO = Mid(v_strLine, 79, 10)
                        v_row("COL_VALUE11") = v_strS_ACC_NO.Trim
                        v_row("COL_TYPE11") = "C"
                        v_row("COL_DESC11") = v_strS_ACC_NO.Trim

                        'v_strB_CODE_TRADE = Mid(v_strLine, 33, 3)
                        v_strB_CODE_TRADE = Mid(v_strLine, 39, 3)
                        v_row("COL_VALUE14") = v_strB_CODE_TRADE.Trim
                        v_row("COL_TYPE14") = "C"
                        v_row("COL_DESC14") = v_strB_CODE_TRADE.Trim

                        'v_strS_CODE_TRADE = Mid(v_strLine, 36, 3)
                        v_strS_CODE_TRADE = Mid(v_strLine, 42, 3)
                        v_row("COL_VALUE15") = v_strS_CODE_TRADE.Trim
                        v_row("COL_TYPE15") = "C"
                        v_row("COL_DESC15") = v_strS_CODE_TRADE.Trim


                        v_strB_ORDER_NO = v_strConfirmNo
                        v_row("COL_VALUE16") = v_strB_ORDER_NO.Trim
                        v_row("COL_TYPE16") = "C"
                        v_row("COL_DESC16") = v_strB_ORDER_NO.Trim

                        v_strS_ORDER_NO = v_strConfirmNo
                        v_row("COL_VALUE17") = v_strS_ORDER_NO.Trim
                        v_row("COL_TYPE17") = "C"
                        v_row("COL_DESC17") = v_strS_ORDER_NO.Trim

                        v_row("COL_VALUE12") = v_strSET_TYPE.Trim
                        v_row("COL_TYPE12") = "N"
                        v_row("COL_DESC12") = v_strSET_TYPE.Trim

                        v_row("COL_VALUE13") = v_strBlock_Tran.Trim
                        v_row("COL_TYPE13") = "N"
                        v_row("COL_DESC13") = v_strBlock_Tran.Trim

                        v_row("TLTXCD") = v_strTLTXCD
                        v_row("REAL_ROW") = v_int

                        v_ds.Tables(0).Rows.Add(v_row)
                        v_int = v_int + 1
                        v_intOffSet = v_intOffSet + 1
                        If v_intOffSet = v_intBatchSize Then
                            If v_obj.SaveUsingOracleBulkCopy("txfields_astpt", v_ds.Tables(0)) Then
                                v_intOffSet = 0
                                v_ds.Tables(0).Rows.Clear()
                            Else
                                Throw New Exception("Error in SaveUsingOracleBulkCopy...")
                            End If
                        End If
                    End If
                End If
            End While
            'err 3
            If v_ds.Tables(0).Rows.Count > 0 Then
                If v_obj.SaveUsingOracleBulkCopy("txfields_astpt", v_ds.Tables(0)) Then
                    v_intOffSet = 0
                    v_ds.Tables(0).Rows.Clear()
                Else
                    Throw New Exception("Error in SaveUsingOracleBulkCopy...")
                End If
            End If
            If v_int > 0 Then
                v_entryNode = pv_oXml.CreateNode(Xml.XmlNodeType.Element, "entry", "")
                'Add field name
                v_attrFLDNAME = pv_oXml.CreateAttribute(gc_AtributeFLDNAME)
                v_attrFLDNAME.Value = "INSERT_ROW"
                v_entryNode.Attributes.Append(v_attrFLDNAME)
                'Add field type
                v_attrDATATYPE = pv_oXml.CreateAttribute(gc_AtributeFLDTYPE)
                v_attrDATATYPE.Value = "C"
                v_entryNode.Attributes.Append(v_attrDATATYPE)
                'Set value
                strRows = "INSERT INTO TMP_TXFIELDS SELECT seq_tmp_txfields.nextval, a.* from txfields_astpt a "
                v_entryNode.InnerText = strRows
                v_dataElement.AppendChild(v_entryNode)

                pv_oNode.AppendChild(v_dataElement)
            End If
            If Not v_Stream Is Nothing Then
                v_Stream.Dispose()
            End If
            v_obj.ExecuteNonQuery(CommandType.Text, "UPDATE sysvar SET varvalue = '3' WHERE grname = 'SYSTEM' AND varname = 'SETT_UPD_ASTPT_STATUS' AND brid = '0002'")
            Return True
        Catch ex As Exception
            v_strErrMsg = "File dữ liệu không hợp lệ!. Chi tiết: " & ex.Message & v_strErrMsg
            v_intErrCount += 1
            Return False
        End Try
    End Function

    Public Function HOSTReadFileXML(ByVal pv_strFile As String, ByRef pv_oNode As Xml.XmlNode, ByRef pv_oXml As XmlDocumentEx, ByVal v_strTLTXCD As String, ByVal v_strPARENT_TXNUM As String, ByVal v_strPARENT_TXDATE As String, ByVal v_strPARENT_BUSDATE As String, ByRef v_strErrMsg As String, ByRef v_intErrCount As Integer) As Boolean
        Dim v_obj As New DataAccess
        Try
            'Dim v_oDocument As New Xml.XmlDocument
            'v_oDocument.Load(pv_strFile)
            'v_oDocument.InnerXml = v_oDocument.InnerXml.Replace(" xmlns=""http://tempuri.org/SATS_TRADING_RESULT.xsd""", "")
            Dim v_entryNode As Xml.XmlNode
            Dim v_attrFLDNAME As Xml.XmlAttribute, v_attrDATATYPE As Xml.XmlAttribute

            Dim xmlReader As New XmlTextReader(pv_strFile)

            Dim strRows As String = ""

            v_obj.NewDBInstance(gc_MODULE_HOST)

            'Dim v_nodeList As Xml.XmlNodeList
            'v_nodeList = v_oDocument.SelectNodes("/SATS_TRADING_RESULT/TRADING_RESULT")

            Dim strInsertFields, strInsertValues As String
            strInsertFields = ",COL_VALUE01, COL_TYPE01, COL_DESC01,COL_VALUE02, COL_TYPE02 , COL_DESC02,COL_VALUE03, COL_TYPE03 , COL_DESC03"
            strInsertValues = ",'" & v_strPARENT_TXNUM & "'" & " COL_VALUE01" _
            & ", 'C'" & " COL_TYPE01" _
            & ", '" & v_strPARENT_TXNUM & "'" & " COL_DESC01" _
            & ", '" & v_strPARENT_TXDATE & "'" & " COL_VALUE02" _
            & ", 'D'" & " COL_TYPE02" _
            & ", '" & v_strPARENT_TXDATE & "'" & " COL_DESC02" _
            & ", '" & v_strPARENT_BUSDATE & "'" & " COL_VALUE03" _
            & ", 'D'" & " COL_TYPE03" _
            & ", '" & v_strPARENT_BUSDATE & "'" & " COL_DESC03"

            Dim v_dataElement As Xml.XmlElement
            v_dataElement = pv_oXml.CreateElement(Xml.XmlNodeType.Element, "fields", "")
            Dim v_int As Integer

            Dim v_strField, v_strValue As String
            Dim v_strFieldsSQL, v_strValuesSQL As String

            v_int = 0
            While Not xmlReader.EOF
                'err 1
                If xmlReader.MoveToContent = XmlNodeType.Element And xmlReader.Name = "TRADING_RESULT" Then
                    'bangpv
                    v_strErrMsg = "Loi tai: 'err 1   If xmlReader.MoveToContent = XmlNodeType.Element And xmlReader.Name = ""TRADING_RESULT"" Then (dòng:" & v_int & ")"

                    'end bangpv
                    v_strFieldsSQL = ""
                    v_strValuesSQL = ""
                    xmlReader.Read()
                    If xmlReader.MoveToContent = XmlNodeType.Element And xmlReader.Name = "CONFIRM_NO" Then
                        v_strValue = xmlReader.ReadString
                        v_strFieldsSQL &= ",COL_VALUE04,COL_TYPE04,COL_DESC04"
                        v_strValuesSQL &= ",'" & v_strValue.Trim & "' COL_VALUE04,'C' COL_TYPE04,'" & v_strValue.Trim & "' COL_DESC04"
                    End If
                    xmlReader.Read()
                    If xmlReader.MoveToContent = XmlNodeType.Element And xmlReader.Name = "B_ORDER_NO" Then
                        v_strValue = xmlReader.ReadString
                        v_strFieldsSQL &= ",COL_VALUE16,COL_TYPE16,COL_DESC16"
                        v_strValuesSQL &= ",'" & v_strValue.Trim & "' COL_VALUE16,'C' COL_TYPE16,'" & v_strValue.Trim & "' COL_DESC16"
                    End If

                    xmlReader.Read()
                    If xmlReader.MoveToContent = XmlNodeType.Element And xmlReader.Name = "B_ORDER_DATE" Then
                        v_strValue = xmlReader.ReadString
                    End If

                    xmlReader.Read()
                    If xmlReader.MoveToContent = XmlNodeType.Element And xmlReader.Name = "S_ORDER_NO" Then
                        v_strValue = xmlReader.ReadString
                        v_strFieldsSQL &= ",COL_VALUE17,COL_TYPE17,COL_DESC17"
                        v_strValuesSQL &= ",'" & v_strValue.Trim & "' COL_VALUE17,'C' COL_TYPE17,'" & v_strValue.Trim & "' COL_DESC17"
                    End If

                    xmlReader.Read()
                    If xmlReader.MoveToContent = XmlNodeType.Element And xmlReader.Name = "S_ORDER_DATE" Then
                        v_strValue = xmlReader.ReadString
                    End If

                    xmlReader.Read()
                    If xmlReader.MoveToContent = XmlNodeType.Element And xmlReader.Name = "B_NEXT_CNFRM" Then
                        v_strValue = xmlReader.ReadString
                    End If

                    xmlReader.Read()
                    If xmlReader.MoveToContent = XmlNodeType.Element And xmlReader.Name = "S_NEXT_CNFRM" Then
                        v_strValue = xmlReader.ReadString
                    End If

                    xmlReader.Read()
                    If xmlReader.MoveToContent = XmlNodeType.Element And xmlReader.Name = "MATCH_TIME" Then
                        v_strValue = xmlReader.ReadString
                        v_strFieldsSQL &= ",COL_VALUE05,COL_TYPE05,COL_DESC05"
                        v_strValuesSQL &= ",'" & v_strValue.Trim & "' COL_VALUE05,'C' COL_TYPE05,'" & v_strValue.Trim & "' COL_DESC05"
                    End If

                    xmlReader.Read()
                    If xmlReader.MoveToContent = XmlNodeType.Element And xmlReader.Name = "MATCH_DATE" Then
                        v_strValue = xmlReader.ReadString
                        v_strFieldsSQL &= ",COL_VALUE06,COL_TYPE06,COL_DESC06"
                        v_strValuesSQL &= ",'" & Mid(v_strValue, 9, 2) & "/" & Mid(v_strValue, 6, 2) & "/" & Mid(v_strValue, 1, 4) & "' COL_VALUE06,'D' COL_TYPE06,'" & Mid(v_strValue, 9, 2) & "/" & Mid(v_strValue, 6, 2) & "/" & Mid(v_strValue, 1, 4) & "' COL_DESC06"
                    End If

                    xmlReader.Read()
                    If xmlReader.MoveToContent = XmlNodeType.Element And xmlReader.Name = "B_TRADING_ID" Then
                        v_strValue = xmlReader.ReadString
                    End If

                    xmlReader.Read()
                    If xmlReader.MoveToContent = XmlNodeType.Element And xmlReader.Name = "S_TRADING_ID" Then
                        v_strValue = xmlReader.ReadString
                    End If

                    xmlReader.Read()
                    If xmlReader.MoveToContent = XmlNodeType.Element And xmlReader.Name = "B_PC_FLAG" Then
                        v_strValue = xmlReader.ReadString
                        v_strFieldsSQL &= ",COL_VALUE18,COL_TYPE18,COL_DESC18"
                        v_strValuesSQL &= ",'" & v_strValue.Trim & "' COL_VALUE18,'C' COL_TYPE18,'" & v_strValue.Trim & "' COL_DESC18"
                    End If

                    xmlReader.Read()
                    If xmlReader.MoveToContent = XmlNodeType.Element And xmlReader.Name = "S_PC_FLAG" Then
                        v_strValue = xmlReader.ReadString
                        v_strFieldsSQL &= ",COL_VALUE19,COL_TYPE19,COL_DESC19"
                        v_strValuesSQL &= ",'" & v_strValue.Trim & "' COL_VALUE19,'C' COL_TYPE19,'" & v_strValue.Trim & "' COL_DESC19"
                    End If

                    xmlReader.Read()
                    If xmlReader.MoveToContent = XmlNodeType.Element And xmlReader.Name = "B_CODE_TRADE" Then
                        v_strValue = xmlReader.ReadString
                        v_strFieldsSQL &= ",COL_VALUE14,COL_TYPE14,COL_DESC14"
                        v_strValuesSQL &= ",'" & IIf(v_strValue.Length = 2, "0" & v_strValue, v_strValue) & "' COL_VALUE14,'C' COL_TYPE14,'" & IIf(v_strValue.Length = 2, "0" & v_strValue, v_strValue) & "' COL_DESC14"
                    End If

                    xmlReader.Read()
                    If xmlReader.MoveToContent = XmlNodeType.Element And xmlReader.Name = "S_CODE_TRADE" Then
                        v_strValue = xmlReader.ReadString
                        v_strFieldsSQL &= ",COL_VALUE15,COL_TYPE15,COL_DESC15"
                        v_strValuesSQL &= ",'" & IIf(v_strValue.Length = 2, "0" & v_strValue, v_strValue) & "' COL_VALUE15,'C' COL_TYPE15,'" & IIf(v_strValue.Length = 2, "0" & v_strValue, v_strValue) & "' COL_DESC15"
                    End If

                    xmlReader.Read()
                    If xmlReader.MoveToContent = XmlNodeType.Element And xmlReader.Name = "STATUS" Then
                        v_strValue = xmlReader.ReadString
                    End If

                    xmlReader.Read()
                    If xmlReader.MoveToContent = XmlNodeType.Element And xmlReader.Name = "SEC_CODE" Then
                        v_strValue = xmlReader.ReadString
                        v_strFieldsSQL &= ",COL_VALUE07,COL_TYPE07,COL_DESC07"
                        v_strValuesSQL &= ",'" & v_strValue.Trim & "' COL_VALUE07,'C' COL_TYPE07,'" & v_strValue.Trim & "' COL_DESC07"
                    End If

                    xmlReader.Read()
                    If xmlReader.MoveToContent = XmlNodeType.Element And xmlReader.Name = "QUANTITY" Then
                        v_strValue = xmlReader.ReadString
                        v_strFieldsSQL &= ",COL_VALUE09,COL_TYPE09,COL_DESC09"
                        v_strValuesSQL &= ",'" & v_strValue.Trim & "' COL_VALUE09,'N' COL_TYPE09,'" & v_strValue.Trim & "' COL_DESC09"
                    End If

                    xmlReader.Read()
                    If xmlReader.MoveToContent = XmlNodeType.Element And xmlReader.Name = "PRICE" Then
                        v_strValue = xmlReader.ReadString
                        v_strFieldsSQL &= ",COL_VALUE08,COL_TYPE08,COL_DESC08"
                        v_strValuesSQL &= ",'" & v_strValue.Trim & "' COL_VALUE08,'N' COL_TYPE08,'" & v_strValue.Trim & "' COL_DESC08"
                    End If

                    xmlReader.Read()
                    If xmlReader.MoveToContent = XmlNodeType.Element And xmlReader.Name = "B_ACCOUNT_NO" Then
                        v_strValue = xmlReader.ReadString
                        v_strFieldsSQL &= ",COL_VALUE10,COL_TYPE10,COL_DESC10"
                        v_strValuesSQL &= ",'" & v_strValue.Trim & "' COL_VALUE10,'C' COL_TYPE10,'" & v_strValue.Trim & "' COL_DESC10"
                    End If

                    xmlReader.Read()
                    If xmlReader.MoveToContent = XmlNodeType.Element And xmlReader.Name = "S_ACCOUNT_NO" Then
                        v_strValue = xmlReader.ReadString
                        v_strFieldsSQL &= ",COL_VALUE11,COL_TYPE11,COL_DESC11"
                        v_strValuesSQL &= ",'" & v_strValue.Trim & "' COL_VALUE11,'C' COL_TYPE11,'" & v_strValue.Trim & "' COL_DESC11"
                    End If

                    xmlReader.Read()
                    If xmlReader.MoveToContent = XmlNodeType.Element And xmlReader.Name = "SETT_TYPE" Then
                        v_strValue = xmlReader.ReadString
                        v_strFieldsSQL &= ",COL_VALUE12,COL_TYPE12,COL_DESC12"
                        v_strValuesSQL &= ",'" & v_strValue.Trim & "' COL_VALUE12,'N' COL_TYPE12,'" & v_strValue.Trim & "' COL_DESC12"
                    End If

                    xmlReader.Read()
                    If xmlReader.MoveToContent = XmlNodeType.Element And xmlReader.Name = "SETT_DAY" Then
                        v_strValue = xmlReader.ReadString
                    End If

                    xmlReader.Read()
                    If xmlReader.MoveToContent = XmlNodeType.Element And xmlReader.Name = "TRADING_DATE" Then
                        v_strValue = xmlReader.ReadString
                    End If

                    xmlReader.Read()
                    If xmlReader.MoveToContent = XmlNodeType.Element And xmlReader.Name = "BLOCK_TRANS" Then
                        v_strValue = xmlReader.ReadString
                        v_strFieldsSQL &= ",COL_VALUE13,COL_TYPE13,COL_DESC13"
                        v_strValuesSQL &= ",'" & v_strValue.Trim & "' COL_VALUE13,'N' COL_TYPE13,'" & v_strValue.Trim & "' COL_DESC13"
                    End If
                    ' xu ly tung dong
                    strRows = strRows & "UNION ALL SELECT '" & v_strTLTXCD & "' TLTXCD " & v_strValuesSQL & " " & strInsertValues & ", " & v_int + 1 & " as REAL_ROW FROM DUAL " & vbCrLf
                    If (v_int + 1) Mod gc_IMP_TRAN_UNIT = 0 Then
                        'Append entry to data node
                        'err 2
                        'bangpv
                        v_strErrMsg = "Loi tai: 'err 2: If (v_int + 1) Mod gc_IMP_TRAN_UNIT = 0 Then (dong:" & v_int & ")"
                        'end bangpv

                        v_entryNode = pv_oXml.CreateNode(Xml.XmlNodeType.Element, "entry", "")
                        'Add field name
                        v_attrFLDNAME = pv_oXml.CreateAttribute(gc_AtributeFLDNAME)
                        v_attrFLDNAME.Value = "INSERT_ROW"
                        v_entryNode.Attributes.Append(v_attrFLDNAME)
                        'Add field type
                        v_attrDATATYPE = pv_oXml.CreateAttribute(gc_AtributeFLDTYPE)
                        v_attrDATATYPE.Value = "C"
                        v_entryNode.Attributes.Append(v_attrDATATYPE)
                        'Set value
                        strRows = "INSERT INTO TMP_TXFIELDS(AUTOID, TLTXCD " & v_strFieldsSQL & " " & strInsertFields & ", REAL_ROW ) " _
                            & " SELECT seq_tmp_txfields.nextval , a.* from (" & strRows.Substring(10) & ") a "
                        v_entryNode.InnerText = strRows
                        v_dataElement.AppendChild(v_entryNode)
                        strRows = ""
                    End If
                    v_int = v_int + 1
                End If
                xmlReader.Read()
            End While

            If strRows <> "" Then
                'bangpv: check loi 
                v_strErrMsg = "'err 3:  If strRows <> "" Then (dong: " & v_int & ")"
                'end bangpv
                v_entryNode = pv_oXml.CreateNode(Xml.XmlNodeType.Element, "entry", "")
                'Add field name
                v_attrFLDNAME = pv_oXml.CreateAttribute(gc_AtributeFLDNAME)
                v_attrFLDNAME.Value = "INSERT_ROW"
                v_entryNode.Attributes.Append(v_attrFLDNAME)
                'Add field type
                v_attrDATATYPE = pv_oXml.CreateAttribute(gc_AtributeFLDTYPE)
                v_attrDATATYPE.Value = "C"
                v_entryNode.Attributes.Append(v_attrDATATYPE)
                'Set value
                strRows = "INSERT INTO TMP_TXFIELDS(AUTOID, TLTXCD " & v_strFieldsSQL & " " & strInsertFields & ", REAL_ROW ) " _
                    & " SELECT seq_tmp_txfields.nextval , a.* from (" & strRows.Substring(10) & ") a "
                v_entryNode.InnerText = strRows
                v_dataElement.AppendChild(v_entryNode)
            End If

            If v_int > 0 Then
                pv_oNode.AppendChild(v_dataElement)
            End If

            xmlReader.Close()


            Return True
        Catch ex As Exception
            v_strErrMsg = "File dữ liệu không hợp lệ!. Chi tiết: " & ex.Message & v_strErrMsg
            v_intErrCount += 1
            Return False
        Finally
            If Not v_obj Is Nothing Then
                v_obj.Dispose()
            End If
        End Try
    End Function
    Public Function HOSTReadFileStockPrice(ByVal pv_strFile As String, _
                                           ByRef pv_oNode As Xml.XmlNode, _
                                           ByRef pv_oXml As XmlDocumentEx, _
                                           ByVal v_strTLTXCD As String, _
                                           ByVal v_strPARENT_TXNUM As String, _
                                           ByVal v_strPARENT_TXDATE As String, _
                                           ByVal v_strPARENT_BUSDATE As String, _
                                           ByRef v_strErrMsg As String, _
                                           ByRef v_intErrCount As Integer, _
                                           ByVal pv_strPriceType As String) As Boolean
        Dim v_ds As DataSet
        Try
            Dim v_entryNode As Xml.XmlNode
            Dim v_dataElement As Xml.XmlElement
            v_dataElement = pv_oXml.CreateElement(Xml.XmlNodeType.Element, "fields", "")

            Dim v_attrFLDNAME As Xml.XmlAttribute, v_attrDATATYPE As Xml.XmlAttribute
            Dim strRows As String = ""
            Dim strInsertFields, strInsertValues As String
            Dim v_strFieldsSQL, v_strValuesSQL As String
            If v_strTLTXCD = "2132" Then
                strInsertFields = ",COL_VALUE01, COL_TYPE01, COL_DESC01, COL_VALUE02, COL_TYPE02, COL_DESC02"
                strInsertValues = ",'" & v_strPARENT_TXNUM & "'" & " COL_VALUE01" _
                & ", 'C'" & " COL_TYPE01" _
                & ", '" & v_strPARENT_TXNUM & "'" & " COL_DESC01" _
                & ", '" & v_strPARENT_TXDATE & "'" & " COL_VALUE02" _
                & ", 'D'" & " COL_TYPE02" _
                & ", '" & v_strPARENT_TXDATE & "'" & " COL_DESC02 "

                Dim xmlReader As New XmlTextReader(pv_strFile)
                v_ds = New DataSet
                v_ds.ReadXml(xmlReader)
                If v_ds.Tables.Count > 0 Then
                    If v_ds.Tables(0).Rows.Count > 0 Then
                        v_strFieldsSQL &= ",COL_VALUE03, COL_TYPE03, COL_DESC03, COL_VALUE04, COL_TYPE04, COL_DESC04, COL_VALUE05, COL_TYPE05, COL_DESC05 " _
                                                & ",COL_VALUE06, COL_TYPE06, COL_DESC06, COL_VALUE07, COL_TYPE07, COL_DESC07, COL_VALUE08, COL_TYPE08, COL_DESC08 " _
                                                & ",COL_VALUE09, COL_TYPE09, COL_DESC09 "
                        For i As Integer = 0 To v_ds.Tables(0).Rows.Count - 1
                            With v_ds.Tables(0).Rows(i)
                                v_strValuesSQL = ",'" & Trim(.Item("TRADING_DATE").ToString()) & "' COL_VALUE03, 'C' COL_TYPE03, '" & Trim(.Item("TRADING_DATE").ToString()) & "' COL_DESC03" _
                                                & ",'" & Trim(.Item("SYMBOL").ToString()) & "' COL_VALUE04, 'C' COL_TYPE04, '" & Trim(.Item("SYMBOL").ToString()) & "' COL_DESC04" _
                                                & ",'" & Trim(.Item("CEILING_PRICE").ToString()) & "' COL_VALUE05, 'C' COL_TYPE05, '" & Trim(.Item("CEILING_PRICE").ToString()) & "' COL_DESC05" _
                                                & ",'" & Trim(.Item("FLOOR_PRICE").ToString()) & "' COL_VALUE06, 'C' COL_TYPE06, '" & Trim(.Item("FLOOR_PRICE").ToString()) & "' COL_DESC06" _
                                                & ",'" & Trim(.Item("REFER_PRICE").ToString()) & "' COL_VALUE07, 'C' COL_TYPE07, '" & Trim(.Item("REFER_PRICE").ToString()) & "' COL_DESC07" _
                                                & ",'" & Trim(.Item("IN_MARKET").ToString()) & "' COL_VALUE08, 'C' COL_TYPE08, '" & Trim(.Item("IN_MARKET").ToString()) & "' COL_DESC08" _
                                                & ",'" & "Gia CK HNX" & "' COL_VALUE09, 'C' COL_TYPE09, '" & "Gia CK HNX" & "' COL_DESC09"
                            End With
                            strRows = strRows & "UNION ALL SELECT '" & v_strTLTXCD & "' TLTXCD " & v_strValuesSQL & " " & strInsertValues & ", " & i + 1 & " as REAL_ROW FROM DUAL " & vbCrLf
                        Next

                        v_entryNode = pv_oXml.CreateNode(Xml.XmlNodeType.Element, "entry", "")
                        'Add field name
                        v_attrFLDNAME = pv_oXml.CreateAttribute(gc_AtributeFLDNAME)
                        v_attrFLDNAME.Value = "INSERT_ROW"
                        v_entryNode.Attributes.Append(v_attrFLDNAME)
                        'Add field type
                        v_attrDATATYPE = pv_oXml.CreateAttribute(gc_AtributeFLDTYPE)
                        v_attrDATATYPE.Value = "C"
                        v_entryNode.Attributes.Append(v_attrDATATYPE)
                        'Set value
                        strRows = "INSERT INTO TMP_TXFIELDS(AUTOID, TLTXCD " & v_strFieldsSQL & " " & strInsertFields & ", REAL_ROW ) " _
                            & " SELECT seq_tmp_txfields.nextval , a.* from (" & strRows.Substring(10) & ") a "
                        v_entryNode.InnerText = strRows
                        v_dataElement.AppendChild(v_entryNode)
                        strRows = ""
                        pv_oNode.AppendChild(v_dataElement)
                    Else
                        v_strErrMsg = "File dữ liệu không hợp lệ!(Không có dòng dữ liệu nào)"
                        v_intErrCount += 1
                        Return False
                    End If
                Else
                    v_strErrMsg = "File dữ liệu không hợp lệ!(Không load được file giá vào Dataset)"
                    v_intErrCount += 1
                    Return False
                End If
                v_ds.Dispose()
            ElseIf v_strTLTXCD = "2135" Then
                strInsertFields = ",COL_VALUE01, COL_TYPE01, COL_DESC01, COL_VALUE02, COL_TYPE02, COL_DESC02"
                strInsertValues = ",'" & v_strPARENT_TXNUM & "'" & " COL_VALUE01" _
                & ", 'C'" & " COL_TYPE01" _
                & ", '" & v_strPARENT_TXNUM & "'" & " COL_DESC01" _
                & ", '" & v_strPARENT_TXDATE & "'" & " COL_VALUE02" _
                & ", 'D'" & " COL_TYPE02" _
                & ", '" & v_strPARENT_TXDATE & "'" & " COL_DESC02 "

                Dim xmlReader As New XmlTextReader(pv_strFile)
                v_ds = New DataSet
                v_ds.ReadXml(xmlReader)
                If v_ds.Tables.Count > 0 Then
                    If v_ds.Tables(0).Rows.Count > 0 Then
                        v_strFieldsSQL &= ",COL_VALUE03, COL_TYPE03, COL_DESC03, COL_VALUE04, COL_TYPE04, COL_DESC04, COL_VALUE05, COL_TYPE05, COL_DESC05 " _
                                                & ",COL_VALUE06, COL_TYPE06, COL_DESC06 "
                        For i As Integer = 0 To v_ds.Tables(0).Rows.Count - 1
                            With v_ds.Tables(0).Rows(i)
                                v_strValuesSQL = ",'" & Trim(.Item("TRADING_DATE").ToString()) & "' COL_VALUE03, 'C' COL_TYPE03, '" & Trim(.Item("TRADING_DATE").ToString()) & "' COL_DESC03" _
                                                & ",'" & Trim(.Item("SYMBOL").ToString()) & "' COL_VALUE04, 'C' COL_TYPE04, '" & Trim(.Item("SYMBOL").ToString()) & "' COL_DESC04" _
                                                & ",'" & Trim(.Item("IN_IDX_LIST").ToString()) & "' COL_VALUE05, 'C' COL_TYPE05, '" & Trim(.Item("IN_IDX_LIST").ToString()) & "' COL_DESC05" _
                                                & ",'" & "Thông tin chỉ số CK HNX" & "' COL_VALUE06, 'C' COL_TYPE06, '" & "Thông tin chỉ số CK HNX" & "' COL_DESC06"
                            End With
                            strRows = strRows & "UNION ALL SELECT '" & v_strTLTXCD & "' TLTXCD " & v_strValuesSQL & " " & strInsertValues & ", " & i + 1 & " as REAL_ROW FROM DUAL " & vbCrLf
                        Next
                        v_entryNode = pv_oXml.CreateNode(Xml.XmlNodeType.Element, "entry", "")
                        'Add field name
                        v_attrFLDNAME = pv_oXml.CreateAttribute(gc_AtributeFLDNAME)
                        v_attrFLDNAME.Value = "INSERT_ROW"
                        v_entryNode.Attributes.Append(v_attrFLDNAME)
                        'Add field type
                        v_attrDATATYPE = pv_oXml.CreateAttribute(gc_AtributeFLDTYPE)
                        v_attrDATATYPE.Value = "C"
                        v_entryNode.Attributes.Append(v_attrDATATYPE)
                        'Set value
                        strRows = "INSERT INTO TMP_TXFIELDS(AUTOID, TLTXCD " & v_strFieldsSQL & " " & strInsertFields & ", REAL_ROW ) " _
                            & " SELECT seq_tmp_txfields.nextval , a.* from (" & strRows.Substring(10) & ") a "
                        v_entryNode.InnerText = strRows
                        v_dataElement.AppendChild(v_entryNode)
                        strRows = ""
                        pv_oNode.AppendChild(v_dataElement)
                    Else
                        v_strErrMsg = "File dữ liệu không hợp lệ!(Không có dòng dữ liệu nào)"
                        v_intErrCount += 1
                        Return False
                    End If
                    v_ds.Dispose()
                Else
                    v_strErrMsg = "File dữ liệu không hợp lệ!(Không load được file giá vào Dataset)"
                    v_intErrCount += 1
                    Return False
                End If
            ElseIf v_strTLTXCD = "2138" Then
                strInsertFields = ",COL_VALUE01, COL_TYPE01, COL_DESC01, COL_VALUE02, COL_TYPE02, COL_DESC02"
                strInsertValues = ",'" & v_strPARENT_TXNUM & "'" & " COL_VALUE01" _
                & ", 'C'" & " COL_TYPE01" _
                & ", '" & v_strPARENT_TXNUM & "'" & " COL_DESC01" _
                & ", '" & v_strPARENT_TXDATE & "'" & " COL_VALUE02" _
                & ", 'D'" & " COL_TYPE02" _
                & ", '" & v_strPARENT_TXDATE & "'" & " COL_DESC02 "

                Dim xmlReader As New XmlTextReader(pv_strFile)
                v_ds = New DataSet
                v_ds.ReadXml(xmlReader)
                If v_ds.Tables.Count > 0 Then
                    If v_ds.Tables(0).Rows.Count > 0 Then
                        v_strFieldsSQL &= ",COL_VALUE03, COL_TYPE03, COL_DESC03, COL_VALUE04, COL_TYPE04, COL_DESC04, COL_VALUE05, COL_TYPE05, COL_DESC05 " _
                                                & ",COL_VALUE06, COL_TYPE06, COL_DESC06, COL_VALUE07, COL_TYPE07, COL_DESC07, COL_VALUE08, COL_TYPE08, COL_DESC08 " _
                                                & ",COL_VALUE09, COL_TYPE09, COL_DESC09, COL_VALUE10, COL_TYPE10, COL_DESC10  "
                        For i As Integer = 0 To v_ds.Tables(0).Rows.Count - 1
                            With v_ds.Tables(0).Rows(i)
                                v_strValuesSQL = ",'" & Trim(.Item("TRADING_DATE").ToString()) & "' COL_VALUE03, 'C' COL_TYPE03, '" & Trim(.Item("TRADING_DATE").ToString()) & "' COL_DESC03" _
                                                & ",'" & Trim(.Item("SYMBOL").ToString()) & "' COL_VALUE04, 'C' COL_TYPE04, '" & Trim(.Item("SYMBOL").ToString()) & "' COL_DESC04" _
                                                & ",'" & Trim(.Item("PRICE").ToString()) & "' COL_VALUE05, 'C' COL_TYPE05, '" & Trim(.Item("PRICE").ToString()) & "' COL_DESC05" _
                                                & ",'" & Trim(.Item("YIELD_CURVE").ToString()) & "' COL_VALUE06, 'C' COL_TYPE06, '" & Trim(.Item("YIELD_CURVE").ToString()) & "' COL_DESC06" _
                                                & ",'" & Trim(.Item("TERM_REMAIN").ToString()) & "' COL_VALUE07, 'C' COL_TYPE07, '" & Trim(.Item("TERM_REMAIN").ToString()) & "' COL_DESC07" _
                                                & ",'" & Trim(.Item("TERM_UNIT").ToString()) & "' COL_VALUE08, 'C' COL_TYPE08, '" & Trim(.Item("TERM_UNIT").ToString()) & "' COL_DESC08" _
                                                & ",'" & Trim(.Item("TERM_REMAIN_YEAR").ToString()) & "' COL_VALUE09, 'C' COL_TYPE09, '" & Trim(.Item("TERM_REMAIN_YEAR").ToString()) & "' COL_DESC09" _
                                                & ",'" & "Gia TP HNX" & "' COL_VALUE10, 'C' COL_TYPE10, '" & "Gia TP HNX" & "' COL_DESC10"
                            End With
                            strRows = strRows & "UNION ALL SELECT '" & v_strTLTXCD & "' TLTXCD " & v_strValuesSQL & " " & strInsertValues & ", " & i + 1 & " as REAL_ROW FROM DUAL " & vbCrLf
                        Next
                        v_entryNode = pv_oXml.CreateNode(Xml.XmlNodeType.Element, "entry", "")
                        'Add field name
                        v_attrFLDNAME = pv_oXml.CreateAttribute(gc_AtributeFLDNAME)
                        v_attrFLDNAME.Value = "INSERT_ROW"
                        v_entryNode.Attributes.Append(v_attrFLDNAME)
                        'Add field type
                        v_attrDATATYPE = pv_oXml.CreateAttribute(gc_AtributeFLDTYPE)
                        v_attrDATATYPE.Value = "C"
                        v_entryNode.Attributes.Append(v_attrDATATYPE)
                        'Set value
                        strRows = "INSERT INTO TMP_TXFIELDS(AUTOID, TLTXCD " & v_strFieldsSQL & " " & strInsertFields & ", REAL_ROW ) " _
                            & " SELECT seq_tmp_txfields.nextval , a.* from (" & strRows.Substring(10) & ") a "
                        v_entryNode.InnerText = strRows
                        v_dataElement.AppendChild(v_entryNode)
                        strRows = ""
                        pv_oNode.AppendChild(v_dataElement)
                    Else
                        v_strErrMsg = "File dữ liệu không hợp lệ!(Không có dòng dữ liệu nào)"
                        v_intErrCount += 1
                        Return False
                    End If
                    v_ds.Dispose()
                Else
                    v_strErrMsg = "File dữ liệu không hợp lệ!(Không load được file giá vào Dataset)"
                    v_intErrCount += 1
                    Return False
                End If
                'bangpv: thêm GD 2162, 2165, 2168, 2171 cho CCP 
            ElseIf v_strTLTXCD = "2162" Then
                strInsertFields = ",COL_VALUE01, COL_TYPE01, COL_DESC01, COL_VALUE02, COL_TYPE02, COL_DESC02"
                strInsertValues = ",'" & v_strPARENT_TXNUM & "'" & " COL_VALUE01" _
                & ", 'C'" & " COL_TYPE01" _
                & ", '" & v_strPARENT_TXNUM & "'" & " COL_DESC01" _
                & ", '" & v_strPARENT_TXDATE & "'" & " COL_VALUE02" _
                & ", 'D'" & " COL_TYPE02" _
                & ", '" & v_strPARENT_TXDATE & "'" & " COL_DESC02 "

                Dim xmlReader As New XmlTextReader(pv_strFile)
                v_ds = New DataSet
                v_ds.ReadXml(xmlReader)
                If v_ds.Tables.Count > 0 Then
                    If v_ds.Tables(0).Rows.Count > 0 Then
                        v_strFieldsSQL &= ",COL_VALUE03, COL_TYPE03, COL_DESC03, COL_VALUE04, COL_TYPE04, COL_DESC04, COL_VALUE05, COL_TYPE05, COL_DESC05 " _
                                                & ",COL_VALUE06, COL_TYPE06, COL_DESC06, COL_VALUE07, COL_TYPE07, COL_DESC07 " _
                                                & ",COL_VALUE10, COL_TYPE10, COL_DESC10  " _
                                                & ",COL_VALUE08, COL_TYPE08, COL_DESC08 " _
                                                & ",COL_VALUE09, COL_TYPE09, COL_DESC09 "
                        For i As Integer = 0 To v_ds.Tables(0).Rows.Count - 1
                            With v_ds.Tables(0).Rows(i)
                                v_strValuesSQL = ",'" & Trim(.Item("DESIGNED_DATE").ToString()) & "' COL_VALUE03, 'C' COL_TYPE03, '" & Trim(.Item("DESIGNED_DATE").ToString()) & "' COL_DESC03" _
                                                & ",'" & Trim(.Item("BOND_CODE").ToString()) & "' COL_VALUE04, 'C' COL_TYPE04, '" & Trim(.Item("BOND_CODE").ToString()) & "' COL_DESC04" _
                                                & ",'" & Trim(.Item("NOMINAL_RATE").ToString()) & "' COL_VALUE05, 'C' COL_TYPE05, '" & Trim(.Item("NOMINAL_RATE").ToString()) & "' COL_DESC05" _
                                                & ",'" & Trim(.Item("MATURITY_DATE").ToString()) & "' COL_VALUE06, 'C' COL_TYPE06, '" & Trim(.Item("MATURITY_DATE").ToString()) & "' COL_DESC06" _
                                                & ",'" & Trim(.Item("CF").ToString()) & "' COL_VALUE07, 'C' COL_TYPE07, '" & Trim(.Item("CF").ToString()) & "' COL_DESC07" _
                                                & ",'" & "Gia TP HNX" & "' COL_VALUE10, 'C' COL_TYPE10, '" & "Gia TP HNX" & "' COL_DESC10" _
                                                & ",'" & Trim(.Item("AI").ToString()) & "' COL_VALUE08, 'C' COL_TYPE08, '" & Trim(.Item("AI").ToString()) & "' COL_DESC08" _
                                                & ",'" & Trim(.Item("DERIVATIVE_CODE").ToString()) & "' COL_VALUE09, 'C' COL_TYPE09, '" & Trim(.Item("DERIVATIVE_CODE").ToString()) & "' COL_DESC09"
                            End With
                            strRows = strRows & "UNION ALL SELECT '" & v_strTLTXCD & "' TLTXCD " & v_strValuesSQL & " " & strInsertValues & ", " & i + 1 & " as REAL_ROW FROM DUAL " & vbCrLf
                        Next
                        v_entryNode = pv_oXml.CreateNode(Xml.XmlNodeType.Element, "entry", "")
                        'Add field name
                        v_attrFLDNAME = pv_oXml.CreateAttribute(gc_AtributeFLDNAME)
                        v_attrFLDNAME.Value = "INSERT_ROW"
                        v_entryNode.Attributes.Append(v_attrFLDNAME)
                        'Add field type
                        v_attrDATATYPE = pv_oXml.CreateAttribute(gc_AtributeFLDTYPE)
                        v_attrDATATYPE.Value = "C"
                        v_entryNode.Attributes.Append(v_attrDATATYPE)
                        'Set value
                        strRows = "INSERT INTO TMP_TXFIELDS(AUTOID, TLTXCD " & v_strFieldsSQL & " " & strInsertFields & ", REAL_ROW ) " _
                            & " SELECT seq_tmp_txfields.nextval , a.* from (" & strRows.Substring(10) & ") a "
                        v_entryNode.InnerText = strRows
                        v_dataElement.AppendChild(v_entryNode)
                        strRows = ""
                        pv_oNode.AppendChild(v_dataElement)
                    Else
                        v_strErrMsg = "File dữ liệu không hợp lệ!(Không có dòng dữ liệu nào)"
                        v_intErrCount += 1
                        Return False
                    End If
                    v_ds.Dispose()
                Else
                    v_strErrMsg = "File dữ liệu không hợp lệ!(Không load được file giá vào Dataset)"
                    v_intErrCount += 1
                    Return False
                End If
                '2165
            ElseIf v_strTLTXCD = "2165" Then
                strInsertFields = ",COL_VALUE01, COL_TYPE01, COL_DESC01, COL_VALUE02, COL_TYPE02, COL_DESC02"
                strInsertValues = ",'" & v_strPARENT_TXNUM & "'" & " COL_VALUE01" _
                & ", 'C'" & " COL_TYPE01" _
                & ", '" & v_strPARENT_TXNUM & "'" & " COL_DESC01" _
                & ", '" & v_strPARENT_TXDATE & "'" & " COL_VALUE02" _
                & ", 'D'" & " COL_TYPE02" _
                & ", '" & v_strPARENT_TXDATE & "'" & " COL_DESC02 "

                Dim xmlReader As New XmlTextReader(pv_strFile)
                v_ds = New DataSet
                v_ds.ReadXml(xmlReader)
                If v_ds.Tables.Count > 0 Then
                    If v_ds.Tables(0).Rows.Count > 0 Then
                        v_strFieldsSQL &= ",COL_VALUE03, COL_TYPE03, COL_DESC03, COL_VALUE04, COL_TYPE04, COL_DESC04, COL_VALUE05, COL_TYPE05, COL_DESC05 " _
                                                & ",COL_VALUE06, COL_TYPE06, COL_DESC06, COL_VALUE07, COL_TYPE07, COL_DESC07 " _
                                                & ", COL_VALUE08, COL_TYPE08, COL_DESC08, COL_VALUE09, COL_TYPE09, COL_DESC09"
                        For i As Integer = 0 To v_ds.Tables(0).Rows.Count - 1
                            With v_ds.Tables(0).Rows(i)
                                v_strValuesSQL = ",'" & Trim(.Item("TRADING_DATE").ToString()) & "' COL_VALUE03, 'C' COL_TYPE03, '" & Trim(.Item("TRADING_DATE").ToString()) & "' COL_DESC03" _
                                                & ",'" & Trim(.Item("BONDINDEX_CODE").ToString()) & "' COL_VALUE04, 'C' COL_TYPE04, '" & Trim(.Item("BONDINDEX_CODE").ToString()) & "' COL_DESC04" _
                                                & ",'" & Trim(.Item("CLEAN_INDEX").ToString()) & "' COL_VALUE05, 'C' COL_TYPE05, '" & Trim(.Item("CLEAN_INDEX").ToString()) & "' COL_DESC05" _
                                                & ",'" & Trim(.Item("MC").ToString()) & "' COL_VALUE06, 'C' COL_TYPE06, '" & Trim(.Item("MC").ToString()) & "' COL_DESC06" _
                                                & ",'" & "Gia chi so dong cua TP HNX" & "' COL_VALUE07, 'C' COL_TYPE07, '" & "Gia chi so dong cua TP HNX" & "' COL_DESC07" _
                                                & ",'" & Trim(.Item("DIRTY_INDEX").ToString()) & "' COL_VALUE08, 'C' COL_TYPE08, '" & Trim(.Item("DIRTY_INDEX").ToString()) & "' COL_DESC08" _
                                                & ",'" & Trim(.Item("TOTAL_INCOM_INDEX").ToString()) & "' COL_VALUE09, 'C' COL_TYPE09, '" & Trim(.Item("TOTAL_INCOM_INDEX").ToString()) & "' COL_DESC09"

                            End With
                            strRows = strRows & "UNION ALL SELECT '" & v_strTLTXCD & "' TLTXCD " & v_strValuesSQL & " " & strInsertValues & ", " & i + 1 & " as REAL_ROW FROM DUAL " & vbCrLf
                        Next
                        v_entryNode = pv_oXml.CreateNode(Xml.XmlNodeType.Element, "entry", "")
                        'Add field name
                        v_attrFLDNAME = pv_oXml.CreateAttribute(gc_AtributeFLDNAME)
                        v_attrFLDNAME.Value = "INSERT_ROW"
                        v_entryNode.Attributes.Append(v_attrFLDNAME)
                        'Add field type
                        v_attrDATATYPE = pv_oXml.CreateAttribute(gc_AtributeFLDTYPE)
                        v_attrDATATYPE.Value = "C"
                        v_entryNode.Attributes.Append(v_attrDATATYPE)
                        'Set value
                        strRows = "INSERT INTO TMP_TXFIELDS(AUTOID, TLTXCD " & v_strFieldsSQL & " " & strInsertFields & ", REAL_ROW ) " _
                            & " SELECT seq_tmp_txfields.nextval , a.* from (" & strRows.Substring(10) & ") a "
                        v_entryNode.InnerText = strRows
                        v_dataElement.AppendChild(v_entryNode)
                        strRows = ""
                        pv_oNode.AppendChild(v_dataElement)
                    Else
                        v_strErrMsg = "File dữ liệu không hợp lệ!(Không có dòng dữ liệu nào)"
                        v_intErrCount += 1
                        Return False
                    End If
                    v_ds.Dispose()
                Else
                    v_strErrMsg = "File dữ liệu không hợp lệ!(Không load được file giá vào Dataset)"
                    v_intErrCount += 1
                    Return False
                End If
                '2168
            ElseIf v_strTLTXCD = "2168" Then
                strInsertFields = ",COL_VALUE01, COL_TYPE01, COL_DESC01, COL_VALUE02, COL_TYPE02, COL_DESC02"
                strInsertValues = ",'" & v_strPARENT_TXNUM & "'" & " COL_VALUE01" _
                & ", 'C'" & " COL_TYPE01" _
                & ", '" & v_strPARENT_TXNUM & "'" & " COL_DESC01" _
                & ", '" & v_strPARENT_TXDATE & "'" & " COL_VALUE02" _
                & ", 'D'" & " COL_TYPE02" _
                & ", '" & v_strPARENT_TXDATE & "'" & " COL_DESC02 "

                Dim xmlReader As New XmlTextReader(pv_strFile)
                v_ds = New DataSet
                v_ds.ReadXml(xmlReader)
                If v_ds.Tables.Count > 0 Then
                    If v_ds.Tables(0).Rows.Count > 0 Then
                        v_strFieldsSQL &= ",COL_VALUE03, COL_TYPE03, COL_DESC03, COL_VALUE04, COL_TYPE04, COL_DESC04, COL_VALUE05, COL_TYPE05, COL_DESC05 " _
                                                & ", COL_VALUE07, COL_TYPE07, COL_DESC07 "
                        For i As Integer = 0 To v_ds.Tables(0).Rows.Count - 1
                            With v_ds.Tables(0).Rows(i)
                                v_strValuesSQL = ",'" & Trim(.Item("TRADING_DATE").ToString()) & "' COL_VALUE03, 'C' COL_TYPE03, '" & Trim(.Item("TRADING_DATE").ToString()) & "' COL_DESC03" _
                                                & ",'" & Trim(.Item("INDEX_CODE").ToString()) & "' COL_VALUE04, 'C' COL_TYPE04, '" & Trim(.Item("INDEX_CODE").ToString()) & "' COL_DESC04" _
                                                & ",'" & Trim(.Item("CLOSE_INDEX").ToString()) & "' COL_VALUE05, 'C' COL_TYPE05, '" & Trim(.Item("CLOSE_INDEX").ToString()) & "' COL_DESC05" _
                                                & ",'" & "Gia chi so dong cua TP HNX" & "' COL_VALUE07, 'C' COL_TYPE07, '" & "Gia chi so dong cua TP HNX" & "' COL_DESC07"
                            End With
                            strRows = strRows & "UNION ALL SELECT '" & v_strTLTXCD & "' TLTXCD " & v_strValuesSQL & " " & strInsertValues & ", " & i + 1 & " as REAL_ROW FROM DUAL " & vbCrLf
                        Next
                        v_entryNode = pv_oXml.CreateNode(Xml.XmlNodeType.Element, "entry", "")
                        'Add field name
                        v_attrFLDNAME = pv_oXml.CreateAttribute(gc_AtributeFLDNAME)
                        v_attrFLDNAME.Value = "INSERT_ROW"
                        v_entryNode.Attributes.Append(v_attrFLDNAME)
                        'Add field type
                        v_attrDATATYPE = pv_oXml.CreateAttribute(gc_AtributeFLDTYPE)
                        v_attrDATATYPE.Value = "C"
                        v_entryNode.Attributes.Append(v_attrDATATYPE)
                        'Set value
                        strRows = "INSERT INTO TMP_TXFIELDS(AUTOID, TLTXCD " & v_strFieldsSQL & " " & strInsertFields & ", REAL_ROW ) " _
                            & " SELECT seq_tmp_txfields.nextval , a.* from (" & strRows.Substring(10) & ") a "
                        v_entryNode.InnerText = strRows
                        v_dataElement.AppendChild(v_entryNode)
                        strRows = ""
                        pv_oNode.AppendChild(v_dataElement)
                    Else
                        v_strErrMsg = "File dữ liệu không hợp lệ!(Không có dòng dữ liệu nào)"
                        v_intErrCount += 1
                        Return False
                    End If
                    v_ds.Dispose()
                Else
                    v_strErrMsg = "File dữ liệu không hợp lệ!(Không load được file giá vào Dataset)"
                    v_intErrCount += 1
                    Return False
                End If
                '2171
            ElseIf v_strTLTXCD = "2171" Then
                strInsertFields = ",COL_VALUE01, COL_TYPE01, COL_DESC01, COL_VALUE02, COL_TYPE02, COL_DESC02"
                strInsertValues = ",'" & v_strPARENT_TXNUM & "'" & " COL_VALUE01" _
                & ", 'C'" & " COL_TYPE01" _
                & ", '" & v_strPARENT_TXNUM & "'" & " COL_DESC01" _
                & ", '" & v_strPARENT_TXDATE & "'" & " COL_VALUE02" _
                & ", 'D'" & " COL_TYPE02" _
                & ", '" & v_strPARENT_TXDATE & "'" & " COL_DESC02 "

                Dim xmlReader As New XmlTextReader(pv_strFile)
                v_ds = New DataSet
                v_ds.ReadXml(xmlReader)
                If v_ds.Tables.Count > 0 Then
                    If v_ds.Tables(0).Rows.Count > 0 Then
                        v_strFieldsSQL &= ",COL_VALUE03, COL_TYPE03, COL_DESC03, COL_VALUE04, COL_TYPE04, COL_DESC04, COL_VALUE05, COL_TYPE05, COL_DESC05 " _
                                                & ", COL_VALUE06, COL_TYPE06, COL_DESC06 , COL_VALUE07, COL_TYPE07, COL_DESC07 "
                        For i As Integer = 0 To v_ds.Tables(0).Rows.Count - 1
                            With v_ds.Tables(0).Rows(i)
                                v_strValuesSQL = ",'" & Trim(.Item("TRADING_DATE").ToString()) & "' COL_VALUE03, 'C' COL_TYPE03, '" & Trim(.Item("TRADING_DATE").ToString()) & "' COL_DESC03" _
                                                & ",'" & Trim(.Item("BOND_TERM").ToString()) & "' COL_VALUE04, 'C' COL_TYPE04, '" & Trim(.Item("BOND_TERM").ToString()) & "' COL_DESC04" _
                                                & ",'" & Trim(.Item("SPOT_RATE").ToString()) & "' COL_VALUE05, 'C' COL_TYPE05, '" & Trim(.Item("SPOT_RATE").ToString()) & "' COL_DESC05" _
                                                & ",'" & Trim(.Item("PAR_YIELD").ToString()) & "' COL_VALUE06, 'C' COL_TYPE06, '" & Trim(.Item("PAR_YIELD").ToString()) & "' COL_DESC06" _
                                                & ",'" & "Gia chi so dong cua TP HNX" & "' COL_VALUE07, 'C' COL_TYPE07, '" & "Gia chi so dong cua TP HNX" & "' COL_DESC07"
                            End With
                            strRows = strRows & "UNION ALL SELECT '" & v_strTLTXCD & "' TLTXCD " & v_strValuesSQL & " " & strInsertValues & ", " & i + 1 & " as REAL_ROW FROM DUAL " & vbCrLf
                        Next
                        v_entryNode = pv_oXml.CreateNode(Xml.XmlNodeType.Element, "entry", "")
                        'Add field name
                        v_attrFLDNAME = pv_oXml.CreateAttribute(gc_AtributeFLDNAME)
                        v_attrFLDNAME.Value = "INSERT_ROW"
                        v_entryNode.Attributes.Append(v_attrFLDNAME)
                        'Add field type
                        v_attrDATATYPE = pv_oXml.CreateAttribute(gc_AtributeFLDTYPE)
                        v_attrDATATYPE.Value = "C"
                        v_entryNode.Attributes.Append(v_attrDATATYPE)
                        'Set value
                        strRows = "INSERT INTO TMP_TXFIELDS(AUTOID, TLTXCD " & v_strFieldsSQL & " " & strInsertFields & ", REAL_ROW ) " _
                            & " SELECT seq_tmp_txfields.nextval , a.* from (" & strRows.Substring(10) & ") a "
                        v_entryNode.InnerText = strRows
                        v_dataElement.AppendChild(v_entryNode)
                        strRows = ""
                        pv_oNode.AppendChild(v_dataElement)
                    Else
                        v_strErrMsg = "File dữ liệu không hợp lệ!(Không có dòng dữ liệu nào)"
                        v_intErrCount += 1
                        Return False
                    End If
                    v_ds.Dispose()
                Else
                    v_strErrMsg = "File dữ liệu không hợp lệ!(Không load được file giá vào Dataset)"
                    v_intErrCount += 1
                    Return False
                End If
                'end bangpv 
            ElseIf v_strTLTXCD = "2141" Then
                strInsertFields = ",COL_VALUE01, COL_TYPE01, COL_DESC01, COL_VALUE02, COL_TYPE02, COL_DESC02"
                strInsertValues = ",'" & v_strPARENT_TXNUM & "'" & " COL_VALUE01" _
                & ", 'C'" & " COL_TYPE01" _
                & ", '" & v_strPARENT_TXNUM & "'" & " COL_DESC01" _
                & ", '" & v_strPARENT_TXDATE & "'" & " COL_VALUE02" _
                & ", 'D'" & " COL_TYPE02" _
                & ", '" & v_strPARENT_TXDATE & "'" & " COL_DESC02 "

                v_strFieldsSQL = ",COL_VALUE03, COL_TYPE03, COL_DESC03, COL_VALUE04, COL_TYPE04, COL_DESC04, COL_VALUE05, COL_TYPE05, COL_DESC05 " _
                                                & ",COL_VALUE06, COL_TYPE06, COL_DESC06, COL_VALUE07, COL_TYPE07, COL_DESC07, COL_VALUE08, COL_TYPE08, COL_DESC08 " _
                                                & ",COL_VALUE09, COL_TYPE09, COL_DESC09 "
                Dim v_Stream As New System.IO.StreamReader(pv_strFile)
                Dim v_strLine As String
                Dim v_strTradeDate, v_strSymbol, v_strCeilPrice, v_strFloorPrice, v_strPrice, v_strInList As String
                Dim v_intCount As Integer = 0
                If pv_strPriceType = "1" Then
                    Dim v_strOutStanding As String
                    'bangpv: sửa thêm trường giá trị thị trường của CP
                    'v_strFieldsSQL = v_strFieldsSQL & ",COL_VALUE10, COL_TYPE10, COL_DESC10 "
                    'end bangpv 
                    While Not v_Stream.EndOfStream
                        v_strValuesSQL = ""
                        v_strLine = Trim(v_Stream.ReadLine) & "   "
                        If v_strLine <> "" Then
                            v_strTradeDate = Mid(v_strLine, 1, 10).Trim
                            v_strSymbol = Mid(v_strLine, 11, 8).Trim
                            'v_strCeilPrice = Mid(v_strLine, 19, 9).Trim
                            'v_strFloorPrice = Mid(v_strLine, 28, 9).Trim
                            'v_strPrice = Mid(v_strLine, 37, 9).Trim
                            'v_strInList = Mid(v_strLine, 46).Trim
                            v_strCeilPrice = ""
                            v_strFloorPrice = ""
                            v_strPrice = Mid(v_strLine, 19).Trim
                            'bangpv: sửa thêm trường giá trị thị trường của CP
                            'v_strOutStanding = Mid(v_strLine, 25).Trim
                            'end bangpv 
                            v_strInList = ""

                            v_strValuesSQL = ",'" & v_strTradeDate & "' COL_VALUE03, 'C' COL_TYPE03, '" & v_strTradeDate & "' COL_DESC03" _
                                            & ",'" & v_strSymbol & "' COL_VALUE04, 'C' COL_TYPE04, '" & v_strSymbol & "' COL_DESC04" _
                                            & ",'" & v_strCeilPrice & "' COL_VALUE05, 'C' COL_TYPE05, '" & v_strCeilPrice & "' COL_DESC05" _
                                            & ",'" & v_strFloorPrice & "' COL_VALUE06, 'C' COL_TYPE06, '" & v_strFloorPrice & "' COL_DESC06" _
                                            & ",'" & v_strPrice & "' COL_VALUE07, 'C' COL_TYPE07, '" & v_strPrice & "' COL_DESC07" _
                                            & ",'" & v_strInList & "' COL_VALUE08, 'C' COL_TYPE08, '" & v_strInList & "' COL_DESC08" _
                                            & ",'" & "Gia CP HSX" & "' COL_VALUE09, 'C' COL_TYPE09, '" & "Gia CP HSX" & "' COL_DESC09"

                            strRows = strRows & "UNION ALL SELECT '" & v_strTLTXCD & "' TLTXCD " & v_strValuesSQL & " " & strInsertValues & ", " & v_intCount + 1 & " as REAL_ROW FROM DUAL " & vbCrLf
                            v_intCount += 1
                        End If
                    End While
                
                Else
                    While Not v_Stream.EndOfStream
                        v_strValuesSQL = ""
                        v_strLine = Trim(v_Stream.ReadLine) & "   "
                        If v_strLine <> "" Then
                            v_strTradeDate = Mid(v_strLine, 1, 10).Trim
                            v_strSymbol = Mid(v_strLine, 11, 8).Trim
                            'v_strCeilPrice = Mid(v_strLine, 19, 9).Trim
                            'v_strFloorPrice = Mid(v_strLine, 28, 9).Trim
                            'v_strPrice = Mid(v_strLine, 37, 9).Trim
                            'v_strInList = Mid(v_strLine, 46).Trim
                            v_strCeilPrice = ""
                            v_strFloorPrice = ""
                            v_strPrice = Mid(v_strLine, 19, 17).Trim
                            v_strInList = Mid(v_strLine, 36).Trim

                            v_strValuesSQL = ",'" & v_strTradeDate & "' COL_VALUE03, 'C' COL_TYPE03, '" & v_strTradeDate & "' COL_DESC03" _
                                            & ",'" & v_strSymbol & "' COL_VALUE04, 'C' COL_TYPE04, '" & v_strSymbol & "' COL_DESC04" _
                                            & ",'" & v_strCeilPrice & "' COL_VALUE05, 'C' COL_TYPE05, '" & v_strCeilPrice & "' COL_DESC05" _
                                            & ",'" & v_strFloorPrice & "' COL_VALUE06, 'C' COL_TYPE06, '" & v_strFloorPrice & "' COL_DESC06" _
                                            & ",'" & v_strPrice & "' COL_VALUE07, 'C' COL_TYPE07, '" & v_strPrice & "' COL_DESC07" _
                                            & ",'" & v_strInList & "' COL_VALUE08, 'C' COL_TYPE08, '" & v_strInList & "' COL_DESC08" _
                                            & ",'" & "Gia CP HSX" & "' COL_VALUE09, 'C' COL_TYPE09, '" & "Gia CP HSX" & "' COL_DESC09"
                            strRows = strRows & "UNION ALL SELECT '" & v_strTLTXCD & "' TLTXCD " & v_strValuesSQL & " " & strInsertValues & ", " & v_intCount + 1 & " as REAL_ROW FROM DUAL " & vbCrLf
                            v_intCount += 1
                        End If
                    End While
                End If
                If v_intCount > 0 Then
                    v_entryNode = pv_oXml.CreateNode(Xml.XmlNodeType.Element, "entry", "")
                    'Add field name
                    v_attrFLDNAME = pv_oXml.CreateAttribute(gc_AtributeFLDNAME)
                    v_attrFLDNAME.Value = "INSERT_ROW"
                    v_entryNode.Attributes.Append(v_attrFLDNAME)
                    'Add field type
                    v_attrDATATYPE = pv_oXml.CreateAttribute(gc_AtributeFLDTYPE)
                    v_attrDATATYPE.Value = "C"
                    v_entryNode.Attributes.Append(v_attrDATATYPE)
                    'Set value
                    strRows = "INSERT INTO TMP_TXFIELDS(AUTOID, TLTXCD " & v_strFieldsSQL & " " & strInsertFields & ", REAL_ROW ) " _
                        & " SELECT seq_tmp_txfields.nextval , a.* from (" & strRows.Substring(10) & ") a "
                    v_entryNode.InnerText = strRows
                    v_dataElement.AppendChild(v_entryNode)
                    strRows = ""
                    pv_oNode.AppendChild(v_dataElement)
                Else
                    v_strErrMsg = "File dữ liệu không hợp lệ!(Không có dòng dữ liệu nào)"
                    v_intErrCount += 1
                    Return False
                End If
                'BangPV: Add 2147 
            ElseIf v_strTLTXCD = "2147" Then
                strInsertFields = ",COL_VALUE01, COL_TYPE01, COL_DESC01, COL_VALUE02, COL_TYPE02, COL_DESC02"
                strInsertValues = ",'" & v_strPARENT_TXNUM & "'" & " COL_VALUE01" _
                & ", 'C'" & " COL_TYPE01" _
                & ", '" & v_strPARENT_TXNUM & "'" & " COL_DESC01" _
                & ", '" & v_strPARENT_TXDATE & "'" & " COL_VALUE02" _
                & ", 'D'" & " COL_TYPE02" _
                & ", '" & v_strPARENT_TXDATE & "'" & " COL_DESC02 "

                v_strFieldsSQL = ",COL_VALUE03, COL_TYPE03, COL_DESC03, COL_VALUE04, COL_TYPE04, COL_DESC04, COL_VALUE05, COL_TYPE05, COL_DESC05 " _
                                                & ",COL_VALUE06, COL_TYPE06, COL_DESC06, COL_VALUE07, COL_TYPE07, COL_DESC07, COL_VALUE08, COL_TYPE08, COL_DESC08 " 
                Dim v_Stream As New System.IO.StreamReader(pv_strFile)
                Dim v_strLine As String
                Dim v_strTradeDate, v_strSymbol, v_strValue, v_strTime, v_strOrderNo As String
                Dim v_intCount As Integer = 0

                Dim v_strOutStanding As String
                'bangpv: sửa thêm trường giá trị thị trường của CP
                'v_strFieldsSQL = v_strFieldsSQL & ",COL_VALUE10, COL_TYPE10, COL_DESC10 "
                'end bangpv 
                While Not v_Stream.EndOfStream
                    v_strValuesSQL = ""
                    v_strLine = RTrim(v_Stream.ReadLine) & "  1 "

                    If v_strLine <> "" Then


                        v_strOrderNo = Mid(v_strLine, 1, 4).Trim
                        v_strTradeDate = Mid(v_strLine, 5, 10).Trim
                        v_strSymbol = Mid(v_strLine, 15, 15)

                        v_strValue = Mid(v_strLine, 30, 9).Trim
                        v_strTime = Mid(v_strLine, 39, 7).Trim


                        v_strValuesSQL = ",'" & v_strOrderNo & "' COL_VALUE03, 'C' COL_TYPE03, '" & v_strOrderNo & "' COL_DESC03" _
                                        & ",'" & v_strTradeDate & "' COL_VALUE04, 'C' COL_TYPE04, '" & v_strTradeDate & "' COL_DESC04" _
                                        & ",'" & v_strSymbol & "' COL_VALUE05, 'C' COL_TYPE05, '" & v_strSymbol & "' COL_DESC05" _
                                        & ",'" & v_strValue & "' COL_VALUE06, 'C' COL_TYPE06, '" & v_strValue & "' COL_DESC06" _
                                        & ",'" & v_strTime & "' COL_VALUE07, 'C' COL_TYPE07, '" & v_strTime & "' COL_DESC07" _
                                        & ",'" & "Gia chi so VN30 HSX" & "' COL_VALUE08, 'C' COL_TYPE08, '" & "Gia chi so VN30 HSX" & "' COL_DESC08"

                        strRows = strRows & "UNION ALL SELECT '" & v_strTLTXCD & "' TLTXCD " & v_strValuesSQL & " " & strInsertValues & ", " & v_intCount + 1 & " as REAL_ROW FROM DUAL " & vbCrLf
                        v_intCount += 1
                    End If
                End While


                If v_intCount > 0 Then
                    v_entryNode = pv_oXml.CreateNode(Xml.XmlNodeType.Element, "entry", "")
                    'Add field name
                    v_attrFLDNAME = pv_oXml.CreateAttribute(gc_AtributeFLDNAME)
                    v_attrFLDNAME.Value = "INSERT_ROW"
                    v_entryNode.Attributes.Append(v_attrFLDNAME)
                    'Add field type
                    v_attrDATATYPE = pv_oXml.CreateAttribute(gc_AtributeFLDTYPE)
                    v_attrDATATYPE.Value = "C"
                    v_entryNode.Attributes.Append(v_attrDATATYPE)
                    'Set value
                    strRows = "INSERT INTO TMP_TXFIELDS(AUTOID, TLTXCD " & v_strFieldsSQL & " " & strInsertFields & ", REAL_ROW ) " _
                        & " SELECT seq_tmp_txfields.nextval , a.* from (" & strRows.Substring(10) & ") a "
                    v_entryNode.InnerText = strRows
                    v_dataElement.AppendChild(v_entryNode)
                    strRows = ""
                    pv_oNode.AppendChild(v_dataElement)
                Else
                    v_strErrMsg = "File dữ liệu không hợp lệ!(Không có dòng dữ liệu nào)"
                    v_intErrCount += 1
                    Return False
                End If
                'end : Add 2147
            ElseIf v_strTLTXCD = "2144" Then
                strInsertFields = ",COL_VALUE01, COL_TYPE01, COL_DESC01, COL_VALUE02, COL_TYPE02, COL_DESC02"
                strInsertValues = ",'" & v_strPARENT_TXNUM & "'" & " COL_VALUE01" _
                & ", 'C'" & " COL_TYPE01" _
                & ", '" & v_strPARENT_TXNUM & "'" & " COL_DESC01" _
                & ", '" & v_strPARENT_TXDATE & "'" & " COL_VALUE02" _
                & ", 'D'" & " COL_TYPE02" _
                & ", '" & v_strPARENT_TXDATE & "'" & " COL_DESC02 "

                Dim xmlReader As New XmlTextReader(pv_strFile)
                v_ds = New DataSet
                v_ds.ReadXml(xmlReader)
                If v_ds.Tables.Count > 0 Then
                    If v_ds.Tables(0).Rows.Count > 0 Then
                        v_strFieldsSQL &= ",COL_VALUE03, COL_TYPE03, COL_DESC03, COL_VALUE04, COL_TYPE04, COL_DESC04, COL_VALUE05, COL_TYPE05, COL_DESC05 " _
                                                & ",COL_VALUE06, COL_TYPE06, COL_DESC06 "
                        For i As Integer = 0 To v_ds.Tables(0).Rows.Count - 1
                            With v_ds.Tables(0).Rows(i)
                                v_strValuesSQL = ",'" & Trim(.Item("STOCK_CODE").ToString()) & "' COL_VALUE03, 'C' COL_TYPE03, '" & Trim(.Item("STOCK_CODE").ToString()) & "' COL_DESC03" _
                                                & ",'" & Trim(.Item("CLOSE_PRICE").ToString()) & "' COL_VALUE04, 'C' COL_TYPE04, '" & Trim(.Item("CLOSE_PRICE").ToString()) & "' COL_DESC04" _
                                                & ",'" & Trim(.Item("TIME").ToString()) & "' COL_VALUE05, 'C' COL_TYPE05, '" & Trim(.Item("TIME").ToString()) & "' COL_DESC05" _
                                                & ",'" & "Thông tin gia dong cua CK HNX" & "' COL_VALUE06, 'C' COL_TYPE06, '" & "Thông tin gia dong cua CK HNX" & "' COL_DESC06"
                            End With
                            strRows = strRows & "UNION ALL SELECT '" & v_strTLTXCD & "' TLTXCD " & v_strValuesSQL & " " & strInsertValues & ", " & i + 1 & " as REAL_ROW FROM DUAL " & vbCrLf
                        Next
                        v_entryNode = pv_oXml.CreateNode(Xml.XmlNodeType.Element, "entry", "")
                        'Add field name
                        v_attrFLDNAME = pv_oXml.CreateAttribute(gc_AtributeFLDNAME)
                        v_attrFLDNAME.Value = "INSERT_ROW"
                        v_entryNode.Attributes.Append(v_attrFLDNAME)
                        'Add field type
                        v_attrDATATYPE = pv_oXml.CreateAttribute(gc_AtributeFLDTYPE)
                        v_attrDATATYPE.Value = "C"
                        v_entryNode.Attributes.Append(v_attrDATATYPE)
                        'Set value
                        strRows = "INSERT INTO TMP_TXFIELDS(AUTOID, TLTXCD " & v_strFieldsSQL & " " & strInsertFields & ", REAL_ROW ) " _
                            & " SELECT seq_tmp_txfields.nextval , a.* from (" & strRows.Substring(10) & ") a "
                        v_entryNode.InnerText = strRows
                        v_dataElement.AppendChild(v_entryNode)
                        strRows = ""
                        pv_oNode.AppendChild(v_dataElement)
                    Else
                        v_strErrMsg = "File dữ liệu không hợp lệ!(Không có dòng dữ liệu nào)"
                        v_intErrCount += 1
                        Return False
                    End If
                    v_ds.Dispose()
                Else
                    v_strErrMsg = "File dữ liệu không hợp lệ!(Không load được file giá vào Dataset)"
                    v_intErrCount += 1
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            v_strErrMsg = "File dữ liệu không hợp lệ!. Chi tiết: " & ex.Message & v_strErrMsg
            v_intErrCount += 1
            Return False
        Finally
            GC.Collect()
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
