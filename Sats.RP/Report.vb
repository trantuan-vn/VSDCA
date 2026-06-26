Imports Sats.CommonLibrary
Imports Sats.DataAccessLayer
'Imports System.EnterpriseServices
Imports Sats.ServerCA
Imports System.Collections.Generic
Imports System.IO
'Add to export PDF on server
Imports DataDynamics.ActiveReports
Imports DataDynamics.ActiveReports.Export
Imports DataDynamics.ActiveReports.Export.Pdf
'End Add to export PDF on server 
'<JustInTimeActivation(False), _
'Transaction(TransactionOption.Supported), _
'ObjectPooling(Enabled:=True, MinPoolSize:=30)> _
Public Class Report
    Inherits CoreBusiness.objMaster
    Implements CoreBusiness.IMaster, IDisposable
    '20230219 Add for PDF Export
    Private mv_DataSet As New DataSet


    
    Public Property ClientDataSet() As DataSet
        Get
            Return mv_DataSet
        End Get
        Set(ByVal value As DataSet)
            mv_DataSet = value
        End Set
    End Property
    '20230219 Add for PDF Export
    Public Sub New()
        ATTR_TABLE = "RPPARAMETERS"
        'ContextUtil.SetComplete()
    End Sub
    Public Function Add(ByRef pv_xmlDocument As XmlDocumentEx) As Long Implements CoreBusiness.IMaster.Add

    End Function

    Public Function Adhoc(ByRef pv_xmlDocument As XmlDocumentEx) As Long Implements CoreBusiness.IMaster.Adhoc
        Dim v_lngErrCode As Long = ERR_SYSTEM_OK
        Dim v_strFunction As String
        Try
            Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes
            v_strFunction = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeFUNCNAME), Xml.XmlAttribute).Value)

            Select Case Trim(v_strFunction)
                'Case "PageSetting"
                '    v_lngErrCode = ReportSetting(pv_xmlDocument)
                'Case "CreateReport"
                '    v_lngErrCode = CreateReportOver(pv_xmlDocument)
                Case "HOSTCreateReport"
                    v_lngErrCode = HOSTCreateReportOver(pv_xmlDocument)
                Case "ExportViewNetBank"
                    v_lngErrCode = ExportViewNetBank(pv_xmlDocument)
                Case "ExportViewNetBank2"
                    v_lngErrCode = ExportViewNetBank2(pv_xmlDocument)
                Case "ExportViewNetBank3"
                    v_lngErrCode = ExportViewNetBank3(pv_xmlDocument)
                Case "CheckviewNetBank"
                    v_lngErrCode = CheckViewNetBank(pv_xmlDocument)
            End Select
            Return v_lngErrCode
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function Delete(ByRef pv_xmlDocument As XmlDocumentEx) As Long Implements CoreBusiness.IMaster.Delete

    End Function

    Public Function Edit(ByRef pv_xmlDocument As XmlDocumentEx) As Long Implements CoreBusiness.IMaster.Edit

    End Function

    Public Function Inquiry(ByRef pv_xmlDocument As XmlDocumentEx) As Long Implements CoreBusiness.IMaster.Inquiry
        Dim v_lngErrCode As Long

        Try
            v_lngErrCode = CoreInquiry(pv_xmlDocument)
            'If v_lngErrCode <> 0 Then
            '    Dim v_strErrorSource, v_strErrorMessage As String

            '    v_strErrorSource = "RP.Inquiry"
            '    v_strErrorMessage = String.Empty

            '    LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
            '                 & "Error code: " & v_lngErrCode.ToString() & vbNewLine _
            '                 & "Error message: " & v_strErrorMessage, EventLogEntryType.Information)
            '    BuildXMLErrorException(pv_xmlDocument, v_strErrorSource, v_lngErrCode, v_strErrorMessage)
            'End If

            'ContextUtil.SetComplete()
            Return v_lngErrCode
        Catch ex As Exception
            'ContextUtil.SetAbort()
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            '             & "Error code: System error!" & vbNewLine _
            '             & "Error message: " & ex.Message, EventLogEntryType.Error)
            Throw ex
        End Try
    End Function


#Region "Lay thong tin Report"

    'Private Function ReportSetting(ByRef pv_xmlDocument As XmlDocumentEx) As Long
    '    Dim v_lngErr As Long = ERR_SYSTEM_OK

    '    Try
    '        Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes

    '        Dim v_strSQL As String = ""
    '        Dim v_strObjMsg As String
    '        Dim v_strClause As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCLAUSE), Xml.XmlAttribute).Value)
    '        v_strObjMsg = pv_xmlDocument.InnerXml()

    '        'Lay thong ti chung cua bao cao
    '        v_strSQL = "SELECT * FROM RPREPORTS WHERE RPTID = '" & v_strClause & "'  AND status=0 AND deleted=0"
    '        Dim v_obj As New TTDataAccess
    '        Dim v_ds As DataSet
    '        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

    '        'Dim v_strObjName As String
    '        'v_strObjName = v_ds.Tables(0).Rows(0)("OBJNAME").ToString

    '        BuildXMLRptData(v_ds, v_strObjMsg, , "PageSetting")

    '        'Lay thong tin cac truong bao cao
    '        v_strSQL = "SELECT * FROM RPFLD WHERE RPTID = '" & v_strClause & "'  AND status=0 AND deleted=0 ORDER BY POSITION"
    '        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
    '        BuildXMLRptData(v_ds, v_strObjMsg, , "RPTFLD")

    '        'Lay thong tin nhom
    '        v_strSQL = "SELECT a.cation, a.rptid, a.POSITION, a.width, a.fieldname, a.fieldtype," _
    '                    & " a.format, a.en_cation, a.GRPFOOTER FROM rpgrp a" _
    '                    & " WHERE a.rptid='" & v_strClause & "' AND a.status=0 AND a.deleted=0" _
    '                    & " ORDER BY a.POSITION"
    '        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
    '        BuildXMLRptData(v_ds, v_strObjMsg, , "GRPFLD")

    '        pv_xmlDocument.LoadXml(v_strObjMsg)

    '        v_obj.Dispose()
    '        GC.Collect()
    '        Return v_lngErr
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Function


    'Private Function CreateReport(ByRef pv_xmlDocument As XmlDocumentEx) As Long
    '    Dim v_lngErr As Long = ERR_SYSTEM_OK
    '    Dim v_obj As New TTDataAccess
    '    Dim tr2 As TextWriterTraceListener
    '    Dim v_trace_status, v_trace_path As String
    '    Try
    '        Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes

    '        v_trace_status = "0"
    '        v_trace_path = "C:\log_report_sql_data.txt"

    '        v_obj.GetSysVar("SYSTEM", "TRACE_STATUS", v_trace_status)

    '        If v_trace_status = "1" Then
    '            tr2 = New TextWriterTraceListener(System.IO.File.AppendText(v_trace_path))
    '            Trace.Listeners.Add(tr2)
    '        End If

    '        Dim v_strSQL, v_strSQLTmp As String
    '        Dim v_strObjMsg As String
    '        Dim v_strClause As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCLAUSE), Xml.XmlAttribute).Value)
    '        Dim v_strTellerID As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTLID), Xml.XmlAttribute).Value)
    '        Dim v_arrFilter As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCMDINQUIRY), Xml.XmlAttribute).Value)
    '        Dim v_strBRID As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeBRID), Xml.XmlAttribute).Value)
    '        Dim v_strCurdate As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTXDATE), Xml.XmlAttribute).Value)

    '        v_strObjMsg = pv_xmlDocument.InnerXml()


    '        Dim v_ds, v_ds1 As DataSet
    '        Dim v_trace As DataSet
    '        Dim v_lstBrID As String
    '        Dim v_blnSearch As Boolean = False

    '        Dim v_arrTemp() As String
    '        Dim v_strRptID As String
    '        Dim v_arrField() As String
    '        Dim v_strFieldCode As String = ""
    '        v_arrTemp = v_strClause.Split("#")
    '        v_strRptID = v_arrTemp(0)
    '        v_arrField = v_arrTemp(1).Split("$")

    '        'Kiem tra xem DL co dc lay tu qua khu hay ko?
    '        v_strSQL = "SELECT FLDNAME FROM FLDMASTER WHERE OBJNAME='" & v_strRptID & "' AND LOADALL='Y' AND DELETED=0 AND STATUS=0"
    '        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

    '        If v_ds.Tables(0).Rows.Count > 0 Then
    '            v_strFieldCode = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(0)(0)))
    '        End If

    '        If v_strFieldCode <> "" Then
    '            For i As Integer = 0 To v_arrField.Count - 2
    '                If v_strFieldCode = v_arrField(i).Split("|")(0) Then
    '                    If v_strCurdate <> v_arrField(i).Split("|")(1) Then
    '                        v_blnSearch = True
    '                    End If
    '                    Exit For
    '                End If
    '            Next
    '        End If

    '        'Lay phan quyen chi nhanh
    '        v_strSQL = "SELECT DISTINCT b.brid FROM brgrp b, tlbridauth a" _
    '                    & " WHERE ((a.AUTHID = '" & v_strTellerID & "' AND a.authtype = 'U')" _
    '                    & " OR (a.AUTHID IN (SELECT grpid FROM tlgrpusers WHERE tlid = '" & v_strTellerID & "')" _
    '                    & " AND a.authtype = 'G')) AND b.deleted = 0 AND b.status = 0" _
    '                    & " AND b.brid = a.brid AND b.deleted=0 AND b.status=0"
    '        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

    '        v_lstBrID = "('" & v_strBRID & "'"
    '        For i = 0 To v_ds.Tables(0).Rows.Count - 1
    '            v_lstBrID &= ",'" & v_ds.Tables(0).Rows(i)(0) & "'"
    '        Next
    '        v_lstBrID &= ")"

    '        'Lay TVLK dc phan quyen cho user vao bang TT_TMP_RGMI
    '        v_strSQL = "INSERT INTO tt_tmp_rgmi " _
    '                    & " SELECT DISTINCT m.* FROM rgmi m, tlmemauth a" _
    '                    & " WHERE ((a.AUTHID = '" & v_strTellerID & "' AND a.authtype = 'U')" _
    '                    & " OR (a.AUTHID IN (SELECT grpid FROM tlgrpusers WHERE tlid = '" & v_strTellerID & "')" _
    '                    & " AND a.authtype = 'G')) AND m.deleted = 0 AND m.status = 0" _
    '                    & " AND m.micode = a.micode AND m.deleted=0 AND m.status=0"
    '        v_obj.ExecuteNonQueryTran(CommandType.Text, v_strSQL)

    '        v_strSQL = "INSERT INTO TT_TMP_RGMI(AUTOID, MICODE) SELECT '0' AUTOID, '000' MICODE FROM DUAL"
    '        v_obj.ExecuteNonQueryTran(CommandType.Text, v_strSQL)

    '        'Lay chung khoan dc phan quyen cho user vao bang TT_TMP_RGSI
    '        v_strSQL = "INSERT INTO tt_tmp_rgsi(iscode, sicode, stock_name, part_value, TYPE," _
    '                        & "stock_type, interest_rate, interest_period, " _
    '                        & "bond_period, deleted, exchange_rate, note, " _
    '                        & "bond_term, release_series, release_mode, isin, " _
    '                        & "rarate, tmpid, paidmethod, npaiddate1, npaiddate2, " _
    '                        & "npaiddate3, npaiddate4, int_release_mode) " _
    '                        & " SELECT distinct m.iscode, m.sicode, m.stock_name, m.part_value, m.TYPE," _
    '                        & "m.stock_type, m.interest_rate, m.interest_period," _
    '                        & "m.bond_period, m.deleted, m.exchange_rate, m.note, " _
    '                        & "m.bond_term, m.release_series, m.release_mode, m.isin, " _
    '                        & "m.rarate, m.tmpid, m.paidmethod, m.npaiddate1, m.npaiddate2," _
    '                        & "m.npaiddate3, m.npaiddate4, m.int_release_mode FROM rgsi m, tlstockauth a" _
    '                        & " WHERE ((a.AUTHID = '" & v_strTellerID & "' AND a.authtype = 'U')" _
    '                        & " OR (a.AUTHID IN (SELECT grpid FROM tlgrpusers WHERE tlid = '" & v_strTellerID & "')" _
    '                        & " AND a.authtype = 'G')) AND m.deleted = 0 " _
    '                        & " AND m.sicode = a.sicode AND m.deleted=0 AND m.BRID IN " & v_lstBrID
    '        v_obj.ExecuteNonQueryTran(CommandType.Text, v_strSQL)

    '        v_strSQL = "INSERT INTO TT_TMP_RGSI(AUTOID, SICODE) SELECT 0 AUTOID, '000' SICODE FROM DUAL"
    '        v_obj.ExecuteNonQueryTran(CommandType.Text, v_strSQL)

    '        Dim v_lstSICODE, v_lstMICODE As String

    '        v_lstMICODE = v_arrFilter.Split("|")(1)
    '        v_lstSICODE = v_arrFilter.Split("|")(0)

    '        'Xu ly DL bang TRAN, TLLOG
    '        Dim v_strMainSQL As String
    '        v_strSQL = "SELECT RPTCMDSQL, TLLOGTRAN FROM RPREPORTS WHERE RPTID = '" & v_strRptID & "' AND DELETED=0 AND STATUS=0"
    '        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

    '        Dim v_strTran As String = "" 'IA|MA|CA|MF|RA|RG|SF|TLLOG
    '        Dim v_lstTran() As String

    '        If v_ds.Tables(0).Rows.Count > 0 Then
    '            v_strTran = v_ds.Tables(0).Rows(0)("TLLOGTRAN").ToUpper
    '            v_strMainSQL = v_ds.Tables(0).Rows(0)("RPTCMDSQL")
    '        End If

    '        If InStr(v_strTran, "TLLOG") > 0 Then
    '            v_strSQL = "INSERT INTO TT_TMP_TLLOG" _
    '                    & " SELECT a.* FROM tllog a, (SELECT DISTINCT tltxcd, tllimit, tltype" _
    '                    & " FROM (SELECT tltxcd, DECODE (MIN (tllimit), 0, 0, MAX (tllimit)) tllimit, tltype FROM tlauth" _
    '                    & " WHERE (authtype = 'U' AND AUTHID = '" & v_strTellerID & "') OR (AUTHID IN (SELECT grpid FROM tlgrpusers WHERE tlid = '" & v_strTellerID & "')" _
    '                    & " AND authtype = 'G') GROUP BY tltxcd, tltype)) b" _
    '                    & " WHERE(a.status = to_number(b.tltype) And DECODE(b.tllimit, 0, 0, NVL(a.msgamt, 0)) <= b.tllimit)" _
    '                    & " AND a.tltxcd = b.tltxcd AND a.deleted = 0 AND nvl(a.sicode,'000') IN " & v_lstSICODE _
    '                    & " AND (nvl(a.micode,'000') IN " & v_lstMICODE & " OR a.comicode IN " & v_lstMICODE & ")" _
    '                    & " AND a.brid IN " & v_lstBrID

    '            v_obj.ExecuteNonQueryTran(CommandType.Text, v_strSQL)

    '            'Neu tim kiem qua khu
    '            If v_blnSearch Then
    '                v_strSQL = "INSERT INTO TT_TMP_TLLOG" _
    '                    & " SELECT a.* FROM tllogall a, (SELECT DISTINCT tltxcd, tllimit, tltype" _
    '                    & " FROM (SELECT tltxcd, DECODE (MIN (tllimit), 0, 0, MAX (tllimit)) tllimit, tltype FROM tlauth" _
    '                    & " WHERE (authtype = 'U' AND AUTHID = '" & v_strTellerID & "') OR (AUTHID IN (SELECT grpid FROM tlgrpusers WHERE tlid = '" & v_strTellerID & "')" _
    '                    & " AND authtype = 'G') GROUP BY tltxcd, tltype)) b" _
    '                    & " WHERE(a.status = to_number(b.tltype) And DECODE(b.tllimit, 0, 0, NVL(a.msgamt, 0)) <= b.tllimit)" _
    '                    & " AND a.tltxcd = b.tltxcd AND a.deleted = 0 AND nvl(a.sicode,'000') IN " & v_lstSICODE _
    '                    & " AND (nvl(a.micode,'000') IN " & v_lstMICODE & " OR a.comicode IN " & v_lstMICODE & ")" _
    '                    & " AND a.brid IN " & v_lstBrID

    '                v_obj.ExecuteNonQueryTran(CommandType.Text, v_strSQL)
    '            End If
    '        End If

    '        If Trim(v_strTran) <> "" Then
    '            v_lstTran = v_strTran.Split("|")
    '            For i = 0 To v_lstTran.Length - 1
    '                v_strSQL = ""
    '                'Xu ly bang TRAN
    '                Select Case Trim(v_lstTran(i))
    '                    Case "CA", "IA"
    '                        v_strSQL = "INSERT INTO TT_TMP_" & v_lstTran(i) & "TRAN" _
    '                                    & " SELECT * FROM " & v_lstTran(i) & "TRAN a" _
    '                                    & " WHERE substr(a.acctno,1,3) IN " & v_lstMICODE _
    '                                    & " AND SUBSTR (a.acctno, INSTR (a.acctno, '.', 1, 2) + 1," _
    '                                    & " INSTR (a.acctno, '.', 1, 3)- INSTR (a.acctno, '.', 1, 2)- 1) IN " & v_lstSICODE _
    '                                    & " AND a.brid IN " & v_lstBrID & " AND a.deleted=0"
    '                    Case "MA"
    '                        v_strSQL = "INSERT INTO TT_TMP_MATRAN" _
    '                                    & " SELECT * FROM MATRAN a" _
    '                                    & " WHERE substr(a.acctno,1,3) IN " & v_lstMICODE _
    '                                    & " AND SUBSTR (a.acctno, INSTR (a.acctno, '.', 1, 2) + 1) IN " & v_lstSICODE _
    '                                    & " AND a.brid IN " & v_lstBrID & " AND a.deleted=0"
    '                    Case "SF", "MF"
    '                        v_strSQL = "INSERT INTO TT_TMP_" & v_lstTran(i) & "TRAN" _
    '                                    & " SELECT * FROM " & v_lstTran(i) & "TRAN A WHERE substr(a.acctno,1,3) IN " & v_lstMICODE _
    '                                    & " AND a.brid IN " & v_lstBrID & " AND a.deleted=0"
    '                    Case "RA"
    '                        v_strSQL = "INSERT INTO TT_TMP_RATRAN" _
    '                                    & " SELECT * FROM RATRAN A WHERE A.acctno IN " & v_lstSICODE _
    '                                    & " AND a.brid IN " & v_lstBrID & " AND a.deleted=0"
    '                    Case "RG"
    '                        v_strSQL = "INSERT INTO TT_TMP_RGTRAN" _
    '                                    & " SELECT * FROM RGTRAN A WHERE a.brid IN " & v_lstBrID & " AND a.deleted=0"
    '                End Select
    '                If v_strSQL <> "" Then
    '                    v_obj.ExecuteNonQueryTran(CommandType.Text, v_strSQL)
    '                End If

    '                'Xu ly bang MAST
    '                Select Case Trim(v_lstTran(i))
    '                    Case "CA", "IA"
    '                        v_strSQL = "INSERT INTO TT_TMP_" & v_lstTran(i) & "MAST" _
    '                                    & " SELECT * FROM " & v_lstTran(i) & "MAST a" _
    '                                    & " WHERE substr(a." & v_lstTran(i) & "acctno,1,3) IN " & v_lstMICODE _
    '                                    & " AND SUBSTR (a." & v_lstTran(i) & "acctno, INSTR (a." & v_lstTran(i) & "acctno, '.', 1, 2) + 1," _
    '                                    & " INSTR (a." & v_lstTran(i) & "acctno, '.', 1, 3)- INSTR (a." & v_lstTran(i) & "acctno, '.', 1, 2)- 1) IN " & v_lstSICODE _
    '                                    & " AND a.brid IN " & v_lstBrID & " AND a.deleted=0"
    '                    Case "MA"
    '                        v_strSQL = "INSERT INTO TT_TMP_MAMAST" _
    '                                    & " SELECT * FROM MAMAST a" _
    '                                    & " WHERE substr(a.maacctno,1,3) IN " & v_lstMICODE _
    '                                    & " AND SUBSTR (a.maacctno, INSTR (a.maacctno, '.', 1, 2) + 1) IN " & v_lstSICODE _
    '                                    & " AND a.brid IN " & v_lstBrID & " AND a.deleted=0"
    '                    Case "SF", "MF"
    '                        v_strSQL = "INSERT INTO TT_TMP_" & v_lstTran(i) & "MAST" _
    '                                    & " SELECT * FROM " & v_lstTran(i) & "MAST A WHERE substr(a." & v_lstTran(i) & "acctno,1,3) IN " & v_lstMICODE _
    '                                    & " AND a.brid IN " & v_lstBrID & " AND a.deleted=0"
    '                    Case "RA"
    '                        v_strSQL = "INSERT INTO TT_TMP_RAMAST" _
    '                                    & " SELECT * FROM RAMAST A WHERE A." & v_lstTran(i) & "acctno IN " & v_lstSICODE _
    '                                    & " AND a.brid IN " & v_lstBrID & " AND a.deleted=0"
    '                End Select
    '                If v_strSQL <> "" Then
    '                    v_obj.ExecuteNonQueryTran(CommandType.Text, v_strSQL)
    '                End If

    '                'Neu tra cuu qua khu
    '                If v_blnSearch Then
    '                    Select Case Trim(v_lstTran(i)).ToUpper
    '                        Case "CA", "IA"
    '                            v_strSQL = "INSERT INTO TT_TMP_" & v_lstTran(i) & "TRAN" _
    '                                        & " SELECT * FROM " & v_lstTran(i) & "TRANA a" _
    '                                        & " WHERE substr(a.acctno,1,3) IN " & v_lstMICODE _
    '                                        & " AND SUBSTR (a.acctno, INSTR (a.acctno, '.', 1, 2) + 1," _
    '                                        & " INSTR (a.acctno, '.', 1, 3)- INSTR (a.acctno, '.', 1, 2)- 1) IN " & v_lstSICODE _
    '                                        & " AND a.brid IN " & v_lstBrID & " AND a.deleted=0"
    '                        Case "MA"
    '                            v_strSQL = "INSERT INTO TT_TMP_MATRAN" _
    '                                        & " SELECT * FROM MATRANA a" _
    '                                        & " WHERE substr(a.acctno,1,3) IN " & v_lstMICODE _
    '                                        & " AND SUBSTR (a.acctno, INSTR (a.acctno, '.', 1, 2) + 1) IN " & v_lstSICODE _
    '                                        & " AND a.brid IN " & v_lstBrID & " AND a.deleted=0"
    '                        Case "SF", "MF"
    '                            v_strSQL = "INSERT INTO TT_TMP_" & v_lstTran(i) & "TRAN" _
    '                                        & " SELECT * FROM " & v_lstTran(i) & "TRANA A WHERE substr(a.acctno,1,3) IN " & v_lstMICODE _
    '                                        & " AND a.brid IN " & v_lstBrID & " AND a.deleted=0"
    '                        Case "RA"
    '                            v_strSQL = "INSERT INTO TT_TMP_RATRAN" _
    '                                        & " SELECT * FROM RATRANA A WHERE A.acctno IN " & v_lstSICODE _
    '                                        & " AND a.brid IN " & v_lstBrID & " AND a.deleted=0"
    '                        Case "RG"
    '                            v_strSQL = "INSERT INTO TT_TMP_RGTRAN" _
    '                                        & " SELECT * FROM RGTRANA A WHERE a.brid IN " & v_lstBrID & " AND a.deleted=0"
    '                    End Select
    '                    If v_strSQL <> "" Then
    '                        v_obj.ExecuteNonQueryTran(CommandType.Text, v_strSQL)
    '                    End If
    '                End If
    '            Next
    '        End If


    '        v_strSQL = "SELECT * FROM RPSTORES WHERE RPTID='" & v_strRptID & "' AND DELETED=0 AND STATUS=0 ORDER BY ODRNUM"
    '        v_ds1 = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

    '        Dim v_int As Integer

    '        For v_int = 0 To v_ds1.Tables(0).Rows.Count - 1

    '            v_strSQL = v_ds1.Tables(0).Rows(v_int)("CMDSQL")

    '            If Not CheckTranTable(v_strSQL) Then
    '                v_lngErr = 1
    '                Exit For
    '            End If


    '            For i As Integer = 0 To v_arrField.Count - 2
    '                v_strSQL = Replace(v_strSQL, "[!" & v_arrField(i).Split("|")(0) & "]", v_arrField(i).Split("|")(1))
    '            Next

    '            If v_trace_status = "1" And v_ds1.Tables(0).Rows(v_int)("TRACE") = 1 Then
    '                Trace.WriteLine("[RP] " & DateTime.Now & " :" & vbCrLf & " -o- Câu lệnh thứ #" & v_ds1.Tables(0).Rows(v_int)("ODRNUM") & "-o-" & vbCrLf & v_strSQL & vbCrLf)
    '                If InStr(v_strSQL.ToUpper, "SELECT") > 0 Then
    '                    Trace.WriteLine("-o- Dữ liệu câu lệnh thứ #" & v_ds1.Tables(0).Rows(v_int)("ODRNUM") & "-o-")
    '                    v_strSQLTmp = v_strSQL.Substring(InStr(v_strSQL.ToUpper, "SELECT") - 1)
    '                    v_trace = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
    '                    Trace.WriteLine(v_trace.GetXml & vbCrLf)
    '                End If
    '            End If

    '            v_obj.ExecuteNonQueryTran(CommandType.Text, v_strSQL)

    '        Next

    '        If v_lngErr = ERR_SYSTEM_OK Then
    '            'v_strSQL = "SELECT RPTCMDSQL FROM RPREPORTS WHERE RPTID='" & v_strRptID & "'"
    '            'v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

    '            'If v_ds.Tables(0).Rows.Count > 0 Then
    '            v_strSQL = v_strMainSQL
    '            'End If

    '            If Not CheckTranTable(v_strSQL) Then
    '                v_lngErr = 1
    '            Else
    '                For i As Integer = 0 To v_arrField.Count - 2
    '                    v_strSQL = Replace(v_strSQL, "[!" & v_arrField(i).Split("|")(0) & "]", v_arrField(i).Split("|")(1))
    '                Next

    '                If v_strSQL <> "" Then
    '                    v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
    '                    If v_trace_status = "1" Then
    '                        Trace.WriteLine("[RP] " & DateTime.Now & " :" & vbCrLf & " -o- Ket qua:-o-" & vbCrLf & v_strSQL & vbCrLf)
    '                        'Trace.WriteLine(v_ds.GetXml & vbCrLf)
    '                    End If
    '                End If
    '                BuildXMLObjData(v_ds, v_strObjMsg)
    '            End If
    '        End If
    '        pv_xmlDocument.LoadXml(v_strObjMsg)

    '        'If v_lngErr <> 0 Then
    '        '    Dim v_strErrorSource, v_strErrorMessage As String

    '        '    v_strErrorSource = "SISTORES"
    '        '    v_strErrorMessage = String.Empty

    '        '    LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
    '        '                 & "Error code: " & v_lngErr.ToString() & vbNewLine _
    '        '                 & "Error message: " & v_strErrorMessage, EventLogEntryType.Information)
    '        '    BuildXMLErrorException(pv_xmlDocument, v_strErrorSource, v_lngErr, v_strErrorMessage)
    '        'End If

    '        ContextUtil.SetComplete()
    '        Return v_lngErr
    '    Catch ex As Exception
    '        Throw ex
    '        ContextUtil.SetAbort()
    '        v_obj.Rollback()
    '    Finally
    '        If v_trace_status = "1" Then
    '            tr2.Close()
    '            tr2.Dispose()
    '        End If
    '        v_obj.Rollback()
    '        v_obj.Dispose()
    '        GC.Collect()
    '    End Try
    'End Function


    Private Function CheckTranTable(ByVal v_strSQL As String) As Boolean
        Try
            Dim v_strTran As String

            Dim v_int As Integer
            Dim v_strTmp As String
            v_strSQL = v_strSQL.ToUpper
            Dim v_intStart, v_intEnd As String
            Do
                v_intStart = InStr(v_strSQL, "FROM", CompareMethod.Text)
                v_intEnd = 0
                If v_intStart > 0 Then
                    v_strSQL = Mid(v_strSQL, v_intStart)

                    v_intEnd = GetEndSQL(v_strSQL)
                    If v_intEnd <> 0 Then
                        v_strTmp = Mid(v_strSQL, 1, v_intEnd)
                    Else
                        v_strTmp = v_strSQL
                    End If

                    While (InStr(v_strTmp, "TRAN", CompareMethod.Text) > 0) Or (InStr(v_strTmp, "TLLOG", CompareMethod.Text) > 0)
                        v_int = InStr(v_strTmp, "TRAN", CompareMethod.Text)
                        If v_int > 0 Then
                            v_strTran = Mid(v_strTmp, v_int - 9, 13)
                            If Mid(v_strTran, 1, 6) <> "TT_TMP" Then
                                Return False
                            End If

                            v_strTmp = Replace(v_strTmp, "TRAN", "")
                        End If
                        v_int = InStr(v_strTmp, "TLLOG", CompareMethod.Text)

                        If v_int > 0 Then
                            v_strTran = Mid(v_strTmp, v_int - 7, 11)

                            If Mid(v_strTran, 1, 6) <> "TT_TMP" Then
                                Return False
                            End If
                            v_strTmp = Replace(v_strTmp, "TLLOG", "")
                        End If
                    End While
                    If v_intEnd > 0 Then
                        v_strSQL = Mid(v_strSQL, v_intEnd)
                    End If
                End If
            Loop Until v_intEnd = 0 Or v_strSQL = ""

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function


    Private Function GetEndSQL(ByVal pv_strSQL As String) As Integer
        Try
            Dim v_intEnd As Integer = 0
            Dim v_int As Integer

            v_int = InStr(pv_strSQL, "(", CompareMethod.Text)
            If v_int > 0 Then
                v_intEnd = v_int
            End If

            v_int = InStr(pv_strSQL, ")", CompareMethod.Text)
            If v_int > 0 And v_intEnd > 0 And v_intEnd > v_int Then
                v_intEnd = v_int
            End If

            v_int = InStr(pv_strSQL, "WHERE", CompareMethod.Text)
            If v_int > 0 And v_intEnd > 0 And v_intEnd > v_int Then
                v_intEnd = v_int
            End If

            v_int = InStr(pv_strSQL, "ORDER", CompareMethod.Text)
            If v_int > 0 And v_intEnd > 0 And v_intEnd > v_int Then
                v_intEnd = v_int
            End If

            v_int = InStr(pv_strSQL, "SELECT", CompareMethod.Text)
            If v_int > 0 And v_intEnd > 0 And v_intEnd > v_int Then
                v_intEnd = v_int
            End If

            Return v_intEnd
        Catch ex As Exception
            Return 0
        End Try
    End Function

    'Private Function HOSTCreateReport(ByRef pv_xmlDocument As XmlDocumentEx) As Long
    '    Dim v_lngErr As Long = ERR_SYSTEM_OK
    '    Dim v_obj As New DataAccess
    '    Dim tr2 As TextWriterTraceListener
    '    Dim v_trace_status, v_trace_path As String
    '    Try
    '        Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes

    '        v_trace_status = "0"
    '        v_trace_path = "C:\log_report_sql_data.txt"

    '        v_obj.NewDBInstance(gc_MODULE_HOST)
    '        v_obj.BeginTran()

    '        v_obj.GetSysVar("SYSTEM", "TRACE_STATUS", v_trace_status)

    '        If v_trace_status = "1" Then
    '            tr2 = New TextWriterTraceListener(System.IO.File.AppendText(v_trace_path))
    '            Trace.Listeners.Add(tr2)
    '        End If

    '        Dim v_strSQL, v_strSQLTmp As String
    '        Dim v_strObjMsg As String
    '        Dim v_strClause As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCLAUSE), Xml.XmlAttribute).Value)
    '        Dim v_strTellerID As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTLID), Xml.XmlAttribute).Value)
    '        Dim v_arrFilter As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCMDINQUIRY), Xml.XmlAttribute).Value)
    '        Dim v_strBRID As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeBRID), Xml.XmlAttribute).Value)
    '        Dim v_strCurdate As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTXDATE), Xml.XmlAttribute).Value)

    '        v_strObjMsg = pv_xmlDocument.InnerXml()


    '        Dim v_ds, v_ds1 As DataSet
    '        Dim v_trace As DataSet
    '        Dim v_lstBrID As String
    '        Dim v_blnSearch As Boolean = False

    '        Dim v_arrTemp() As String
    '        Dim v_strRptID As String
    '        Dim v_arrField() As String
    '        Dim v_strFieldCode As String = ""
    '        v_arrTemp = v_strClause.Split("#")
    '        v_strRptID = v_arrTemp(0)
    '        v_arrField = v_arrTemp(1).Split("$")

    '        'Lay phan quyen chi nhanh
    '        v_strSQL = "SELECT DISTINCT b.brid FROM brgrp b, tlbridauth a" _
    '                    & " WHERE ((a.AUTHID = '" & v_strTellerID & "' AND a.authtype = 'U')" _
    '                    & " OR (a.AUTHID IN (SELECT grpid FROM tlgrpusers WHERE tlid = '" & v_strTellerID & "')" _
    '                    & " AND a.authtype = 'G')) AND b.deleted = 0 AND b.status = 0" _
    '                    & " AND b.brid = a.brid AND b.deleted=0 AND b.status=0"
    '        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

    '        v_lstBrID = "('" & v_strBRID & "'"
    '        For i = 0 To v_ds.Tables(0).Rows.Count - 1
    '            v_lstBrID &= ",'" & v_ds.Tables(0).Rows(i)(0) & "'"
    '        Next
    '        v_lstBrID &= ")"

    '        'Lay TVLK dc phan quyen cho user vao bang TT_TMP_RGMI
    '        v_strSQL = "INSERT INTO tmp_rgmi " _
    '                    & " SELECT DISTINCT m.* FROM rgmi m, tlmemauth a" _
    '                    & " WHERE ((a.AUTHID = '" & v_strTellerID & "' AND a.authtype = 'U')" _
    '                    & " OR (a.AUTHID IN (SELECT grpid FROM tlgrpusers WHERE tlid = '" & v_strTellerID & "')" _
    '                    & " AND a.authtype = 'G')) AND m.deleted = 0 AND m.status = 0" _
    '                    & " AND m.micode = a.micode AND m.deleted=0 AND m.status=0"
    '        v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

    '        v_strSQL = "INSERT INTO TMP_RGMI(AUTOID, MICODE) SELECT '0' AUTOID, '000' MICODE FROM DUAL"
    '        v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

    '        'Lay chung khoan dc phan quyen cho user vao bang TT_TMP_RGSI
    '        v_strSQL = "INSERT INTO tmp_rgsi(iscode, sicode, stock_name, part_value, TYPE," _
    '                        & "stock_type, interest_rate, interest_period, " _
    '                        & "bond_period, deleted, exchange_rate, note, " _
    '                        & "bond_term, release_series, release_mode, isin, " _
    '                        & "rarate, tmpid, paidmethod, npaiddate1, npaiddate2, " _
    '                        & "npaiddate3, npaiddate4, int_release_mode) " _
    '                        & " SELECT distinct m.iscode, m.sicode, m.stock_name, m.part_value, m.TYPE," _
    '                        & "m.stock_type, m.interest_rate, m.interest_period," _
    '                        & "m.bond_period, m.deleted, m.exchange_rate, m.note, " _
    '                        & "m.bond_term, m.release_series, m.release_mode, m.isin, " _
    '                        & "m.rarate, m.tmpid, m.paidmethod, m.npaiddate1, m.npaiddate2," _
    '                        & "m.npaiddate3, m.npaiddate4, m.int_release_mode FROM rgsi m, tlstockauth a" _
    '                        & " WHERE ((a.AUTHID = '" & v_strTellerID & "' AND a.authtype = 'U')" _
    '                        & " OR (a.AUTHID IN (SELECT grpid FROM tlgrpusers WHERE tlid = '" & v_strTellerID & "')" _
    '                        & " AND a.authtype = 'G')) AND m.deleted = 0 " _
    '                        & " AND m.sicode = a.sicode AND m.deleted=0 AND m.BRID IN " & v_lstBrID
    '        v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

    '        v_strSQL = "INSERT INTO TMP_RGSI(AUTOID, SICODE) SELECT 0 AUTOID, '000' SICODE FROM DUAL"
    '        v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

    '        Dim v_lstSICODE, v_lstMICODE As String

    '        v_lstMICODE = v_arrFilter.Split("|")(1)
    '        v_lstSICODE = v_arrFilter.Split("|")(0)

    '        'Xu ly DL bang TRAN, TLLOG
    '        Dim v_strMainSQL As String
    '        v_strSQL = "SELECT RPTCMDSQL, TLLOGTRAN FROM RPREPORTS WHERE RPTID = '" & v_strRptID & "' AND DELETED=0 AND STATUS=0"
    '        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

    '        Dim v_strTran As String = "" 'IA|MA|CA|MF|RA|RG|SF|TLLOG
    '        Dim v_lstTran() As String

    '        If v_ds.Tables(0).Rows.Count > 0 Then
    '            v_strTran = v_ds.Tables(0).Rows(0)("TLLOGTRAN").ToUpper
    '            v_strMainSQL = v_ds.Tables(0).Rows(0)("RPTCMDSQL")
    '        End If

    '        If InStr(v_strTran, "TLLOG") > 0 Then
    '            v_strSQL = "INSERT INTO TMP_TLLOG" _
    '                & " SELECT a.* FROM tllogall a, (SELECT DISTINCT tltxcd, tllimit, tltype" _
    '                & " FROM (SELECT tltxcd, DECODE (MIN (tllimit), 0, 0, MAX (tllimit)) tllimit, tltype FROM tlauth" _
    '                & " WHERE (authtype = 'U' AND AUTHID = '" & v_strTellerID & "') OR (AUTHID IN (SELECT grpid FROM tlgrpusers WHERE tlid = '" & v_strTellerID & "')" _
    '                & " AND authtype = 'G') GROUP BY tltxcd, tltype)) b" _
    '                & " WHERE(a.status = to_number(b.tltype) And DECODE(b.tllimit, 0, 0, NVL(a.msgamt, 0)) <= b.tllimit)" _
    '                & " AND a.tltxcd = b.tltxcd AND a.deleted = 0 AND nvl(a.sicode,'000') IN " & v_lstSICODE _
    '                & " AND (nvl(a.micode,'000') IN " & v_lstMICODE & " OR a.comicode IN " & v_lstMICODE & ")" _
    '                & " AND a.brid IN " & v_lstBrID

    '            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
    '        End If

    '        If Trim(v_strTran) <> "" Then
    '            v_lstTran = v_strTran.Split("|")
    '            For i = 0 To v_lstTran.Length - 1
    '                'Xu ly bang MAST
    '                Select Case Trim(v_lstTran(i))
    '                    Case "CA", "IA"
    '                        v_strSQL = "INSERT INTO TMP_" & v_lstTran(i) & "MAST" _
    '                                    & " SELECT * FROM " & v_lstTran(i) & "MAST a" _
    '                                    & " WHERE substr(a." & v_lstTran(i) & "acctno,1,3) IN " & v_lstMICODE _
    '                                    & " AND SUBSTR (a." & v_lstTran(i) & "acctno, INSTR (a." & v_lstTran(i) & "acctno, '.', 1, 2) + 1," _
    '                                    & " INSTR (a." & v_lstTran(i) & "acctno, '.', 1, 3)- INSTR (a." & v_lstTran(i) & "acctno, '.', 1, 2)- 1) IN " & v_lstSICODE _
    '                                    & " AND a.brid IN " & v_lstBrID & " AND a.deleted=0"
    '                    Case "MA"
    '                        v_strSQL = "INSERT INTO TMP_MAMAST" _
    '                                    & " SELECT * FROM MAMAST a" _
    '                                    & " WHERE substr(a.maacctno,1,3) IN " & v_lstMICODE _
    '                                    & " AND SUBSTR (a.maacctno, INSTR (a.maacctno, '.', 1, 2) + 1) IN " & v_lstSICODE _
    '                                    & " AND a.brid IN " & v_lstBrID & " AND a.deleted=0"
    '                    Case "SF", "MF"
    '                        v_strSQL = "INSERT INTO TMP_" & v_lstTran(i) & "MAST" _
    '                                    & " SELECT * FROM " & v_lstTran(i) & "MAST A WHERE substr(a." & v_lstTran(i) & "acctno,1,3) IN " & v_lstMICODE _
    '                                    & " AND a.brid IN " & v_lstBrID & " AND a.deleted=0"
    '                    Case "RA"
    '                        v_strSQL = "INSERT INTO TMP_RAMAST" _
    '                                    & " SELECT * FROM RAMAST A WHERE A." & v_lstTran(i) & "acctno IN " & v_lstSICODE _
    '                                    & " AND a.brid IN " & v_lstBrID & " AND a.deleted=0"
    '                End Select
    '                If v_strSQL <> "" Then
    '                    v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
    '                End If


    '                Select Case Trim(v_lstTran(i)).ToUpper
    '                    Case "CA", "IA"
    '                        v_strSQL = "INSERT INTO TMP_" & v_lstTran(i) & "TRAN" _
    '                                    & " SELECT * FROM " & v_lstTran(i) & "TRANA a" _
    '                                    & " WHERE substr(a.acctno,1,3) IN " & v_lstMICODE _
    '                                    & " AND SUBSTR (a.acctno, INSTR (a.acctno, '.', 1, 2) + 1," _
    '                                    & " INSTR (a.acctno, '.', 1, 3)- INSTR (a.acctno, '.', 1, 2)- 1) IN " & v_lstSICODE _
    '                                    & " AND a.brid IN " & v_lstBrID & " AND a.deleted=0"
    '                    Case "MA"
    '                        v_strSQL = "INSERT INTO TMP_MATRAN" _
    '                                    & " SELECT * FROM MATRANA a" _
    '                                    & " WHERE substr(a.acctno,1,3) IN " & v_lstMICODE _
    '                                    & " AND SUBSTR (a.acctno, INSTR (a.acctno, '.', 1, 2) + 1) IN " & v_lstSICODE _
    '                                    & " AND a.brid IN " & v_lstBrID & " AND a.deleted=0"
    '                    Case "SF", "MF"
    '                        v_strSQL = "INSERT INTO TMP_" & v_lstTran(i) & "TRAN" _
    '                                    & " SELECT * FROM " & v_lstTran(i) & "TRANA A WHERE substr(a.acctno,1,3) IN " & v_lstMICODE _
    '                                    & " AND a.brid IN " & v_lstBrID & " AND a.deleted=0"
    '                    Case "RA"
    '                        v_strSQL = "INSERT INTO TMP_RATRAN" _
    '                                    & " SELECT * FROM RATRANA A WHERE A.acctno IN " & v_lstSICODE _
    '                                    & " AND a.brid IN " & v_lstBrID & " AND a.deleted=0"
    '                    Case "RG"
    '                        v_strSQL = "INSERT INTO TMP_RGTRAN" _
    '                                    & " SELECT * FROM RGTRANA A WHERE a.brid IN " & v_lstBrID & " AND a.deleted=0"
    '                End Select
    '                If v_strSQL <> "" Then
    '                    v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
    '                End If
    '            Next
    '        End If

    '        v_strSQL = "SELECT * FROM RPSTORES WHERE RPTID='" & v_strRptID & "' AND DELETED=0 AND STATUS=0 ORDER BY ODRNUM"
    '        v_ds1 = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

    '        Dim v_int As Integer

    '        For v_int = 0 To v_ds1.Tables(0).Rows.Count - 1

    '            v_strSQL = Replace(v_ds1.Tables(0).Rows(v_int)("CMDSQL").ToString.ToUpper, "TT_", "")

    '            If Not CheckTranTable(v_strSQL) Then
    '                v_lngErr = 1
    '                Exit For
    '            End If


    '            For i As Integer = 0 To v_arrField.Count - 2
    '                v_strSQL = Replace(v_strSQL, "[!" & v_arrField(i).Split("|")(0) & "]", v_arrField(i).Split("|")(1))
    '            Next

    '            If v_trace_status = "1" And v_ds1.Tables(0).Rows(v_int)("TRACE") = 1 Then
    '                Trace.WriteLine("[RP] " & DateTime.Now & " :" & vbCrLf & " -o- Câu lệnh thứ #" & v_ds1.Tables(0).Rows(v_int)("ODRNUM") & "-o-" & vbCrLf & v_strSQL & vbCrLf)
    '                If InStr(v_strSQL.ToUpper, "SELECT") > 0 Then
    '                    Trace.WriteLine("-o- Dữ liệu câu lệnh thứ #" & v_ds1.Tables(0).Rows(v_int)("ODRNUM") & "-o-")
    '                    v_strSQLTmp = v_strSQL.Substring(InStr(v_strSQL.ToUpper, "SELECT") - 1)
    '                    v_trace = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
    '                    Trace.WriteLine(v_trace.GetXml & vbCrLf)
    '                End If
    '            End If

    '            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

    '        Next

    '        If v_lngErr = ERR_SYSTEM_OK Then
    '            'v_strSQL = "SELECT RPTCMDSQL FROM RPREPORTS WHERE RPTID='" & v_strRptID & "'"
    '            'v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

    '            'If v_ds.Tables(0).Rows.Count > 0 Then
    '            v_strSQL = Replace(v_strMainSQL.ToUpper, "TT_", "")
    '            'End If

    '            If Not CheckTranTable(v_strSQL) Then
    '                v_lngErr = 1
    '            Else
    '                For i As Integer = 0 To v_arrField.Count - 2
    '                    v_strSQL = Replace(v_strSQL, "[!" & v_arrField(i).Split("|")(0) & "]", v_arrField(i).Split("|")(1))
    '                Next

    '                If v_strSQL <> "" Then
    '                    v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
    '                    If v_trace_status = "1" Then
    '                        Trace.WriteLine("[RP] " & DateTime.Now & " :" & vbCrLf & " -o- Ket qua:-o-" & vbCrLf & v_strSQL & vbCrLf)
    '                        'Trace.WriteLine(v_ds.GetXml & vbCrLf)
    '                    End If
    '                End If
    '                BuildXMLObjData(v_ds, v_strObjMsg)
    '            End If
    '        End If
    '        pv_xmlDocument.LoadXml(v_strObjMsg)

    '        'If v_lngErr <> 0 Then
    '        '    Dim v_strErrorSource, v_strErrorMessage As String

    '        '    v_strErrorSource = "SISTORES"
    '        '    v_strErrorMessage = String.Empty

    '        '    LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
    '        '                 & "Error code: " & v_lngErr.ToString() & vbNewLine _
    '        '                 & "Error message: " & v_strErrorMessage, EventLogEntryType.Information)
    '        '    BuildXMLErrorException(pv_xmlDocument, v_strErrorSource, v_lngErr, v_strErrorMessage)
    '        'End If
    '        v_obj.Commit()
    '        ContextUtil.SetComplete()
    '        Return v_lngErr
    '    Catch ex As Exception
    '        Throw ex
    '        ContextUtil.SetAbort()
    '        v_obj.Rollback()
    '    Finally
    '        If v_trace_status = "1" Then
    '            tr2.Close()
    '            tr2.Dispose()
    '        End If
    '        v_obj.Commit()
    '        v_obj.Dispose()
    '        GC.Collect()
    '    End Try
    'End Function

    'Private Function CreateReportOver(ByRef pv_xmlDocument As XmlDocumentEx) As Long
    '    Dim v_lngErr As Long = ERR_SYSTEM_OK
    '    Dim v_obj As New TTDataAccess
    '    Dim tr2 As TextWriterTraceListener
    '    Dim v_trace_status, v_trace_path As String
    '    Dim v_blnSearch As Boolean
    '    Dim v_strRptID As String

    '    Try
    '        Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes
    '        Dim v_strSQL, v_strSQLTmp As String
    '        Dim v_strObjMsg As String
    '        Dim v_strClause As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCLAUSE), Xml.XmlAttribute).Value)
    '        Dim v_strTellerID As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTLID), Xml.XmlAttribute).Value)
    '        Dim v_arrFilter As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCMDINQUIRY), Xml.XmlAttribute).Value)
    '        Dim v_strBRID As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeBRID), Xml.XmlAttribute).Value)
    '        Dim v_strCurdate As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTXDATE), Xml.XmlAttribute).Value)
    '        If Not (v_attrColl.GetNamedItem(gc_AtributeAUTOID) Is Nothing) Then
    '            v_blnSearch = CBool(CType(v_attrColl.GetNamedItem(gc_AtributeAUTOID), Xml.XmlAttribute).Value)
    '        Else
    '            v_blnSearch = False
    '        End If
    '        Dim v_strPartition As String
    '        If Not (v_attrColl.GetNamedItem(gc_AtributeTXNUM) Is Nothing) Then
    '            v_strPartition = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTXNUM), Xml.XmlAttribute).Value)
    '        Else
    '            v_strPartition = ""
    '        End If


    '        v_trace_status = "0"
    '        'v_trace_path = "C:\log_report_sql_data.txt"           

    '        v_strObjMsg = pv_xmlDocument.InnerXml()


    '        v_obj.GetSysVar("SYSTEM", "TRACE_STATUS", v_trace_status)
    '        Dim v_strCurrDate As String
    '        If v_trace_status = "1" Then
    '            v_obj.GetSysVar("SYSTEM", "TRACE_PATH", v_trace_path)
    '            Dim v_strTLName As String = v_obj.GetValue("TLPROFILES", "TLNAME", "TLID='" & v_strTellerID & "'")

    '            v_strCurrDate = Replace(v_strCurdate, "/", "_")
    '            If v_trace_path = "" Then
    '                Dim v_app As New ApplicationServices.ApplicationBase
    '                v_trace_path = v_app.Info.DirectoryPath & "\Log\" & v_strCurrDate
    '            Else
    '                v_trace_path = v_trace_path & v_strCurrDate
    '            End If

    '            If Not System.IO.Directory.Exists(v_trace_path) Then
    '                System.IO.Directory.CreateDirectory(v_trace_path)
    '            End If

    '            v_trace_path &= "\log_report_br" & v_strBRID & "_" & v_strTLName & ".txt"

    '            tr2 = New TextWriterTraceListener(System.IO.File.AppendText(v_trace_path))
    '            Trace.Listeners.Add(tr2)
    '            Trace.WriteLine("[Bắt đầu: RP - " & v_strRptID & "] " & DateTime.Now & vbCrLf)
    '        End If

    '        Dim v_ds, v_ds1 As DataSet
    '        Dim v_trace As DataSet
    '        Dim v_lstBrID As String

    '        Dim v_arrTemp() As String

    '        Dim v_arrField() As String
    '        Dim v_strFieldCode As String = ""
    '        v_arrTemp = v_strClause.Split("#")
    '        v_strRptID = v_arrTemp(0)
    '        v_arrField = v_arrTemp(1).Split("$")

    '        Dim v_strMinDate, v_strMaxDate As String
    '        v_strMinDate = v_strCurdate
    '        v_strMaxDate = v_strCurdate
    '        If v_strPartition <> "" Then
    '            v_strMinDate = v_strPartition.Split("|")(0)
    '            v_strMaxDate = v_strPartition.Split("|")(1)
    '        End If
    '        'Xu ly DL bang TRAN, TLLOG
    '        Dim v_strMainSQL As String
    '        v_strSQL = "SELECT RPTCMDSQL, TLLOGTRAN FROM RPREPORTS WHERE RPTID = '" & v_strRptID & "' AND DELETED=0 AND STATUS=0"
    '        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

    '        Dim v_strTran As String = "" 'IA|MA|CA|MF|RA|RG|SF|TLLOG
    '        Dim v_lstTran() As String

    '        If v_ds.Tables(0).Rows.Count > 0 Then
    '            v_strTran = v_ds.Tables(0).Rows(0)("TLLOGTRAN").ToUpper
    '            v_strMainSQL = v_ds.Tables(0).Rows(0)("RPTCMDSQL")
    '        End If

    '        If v_strTran <> "" Then
    '            v_lstTran = v_strTran.Split("|")
    '            v_strTran = ""
    '            For i As Integer = 0 To v_lstTran.Length - 1
    '                Select Case v_lstTran(i)
    '                    Case "TLLOG"
    '                        v_strTran &= ",'TLLOG'"
    '                        If v_blnSearch Then
    '                            v_strTran &= ",'TLLOGALL'"
    '                        End If
    '                    Case "CA", "IA", "MA", "RA", "SF"
    '                        v_strTran &= ",'" & v_lstTran(i) & "TRAN'"
    '                        v_strTran &= ",'" & v_lstTran(i) & "MAST'"
    '                        If v_blnSearch Then
    '                            v_strTran &= ",'" & v_lstTran(i) & "TRANA'"
    '                        End If
    '                    Case "RG", "MF"
    '                        v_strTran &= ",'" & v_lstTran(i) & "TRAN'"
    '                        If v_blnSearch Then
    '                            v_strTran &= ",'" & v_lstTran(i) & "TRANA'"
    '                        End If
    '                End Select
    '            Next
    '            v_strTran = "(" & Mid(v_strTran, 2) & ")"
    '        End If
    '        Dim v_lstSICODE, v_lstMICODE As String

    '        v_lstMICODE = v_arrFilter.Split("|")(1)
    '        v_lstSICODE = v_arrFilter.Split("|")(0)

    '        'Lay phan quyen chi nhanh
    '        v_strSQL = "SELECT DISTINCT brid brid  FROM " _
    '                    & " (SELECT b.brid FROM tlbridauth a, brgrp b" _
    '                    & " WHERE ((AUTHID = '" & v_strTellerID & "' AND authtype = 'U') OR (AUTHID IN (SELECT a.grpid" _
    '                    & " FROM tlgrpusers a WHERE a.tlid = '" & v_strTellerID & "') AND authtype = 'G'))" _
    '                    & " And a.brid = b.brid And a.deleted = 0 And a.status = 0" _
    '                    & " And b.deleted = 0 And b.status = 0" _
    '                    & " UNION " _
    '                    & " SELECT b.brid FROM tlprofiles a, brgrp b" _
    '                    & " WHERE a.brid = b.brid AND a.tlid = '" & v_strTellerID & "'" _
    '                    & " AND a.deleted = 0 AND a.status = 0 AND b.deleted = 0 AND b.status = 0)"

    '        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

    '        v_lstBrID = ""
    '        For i = 0 To v_ds.Tables(0).Rows.Count - 1
    '            v_lstBrID &= ",'" & v_ds.Tables(0).Rows(i)(0) & "'"
    '        Next
    '        v_lstBrID = "(" & Mid(v_lstBrID, 2) & ")"


    '        'Lay TVLK dc phan quyen cho user vao bang TT_TMP_RGMI
    '        v_strSQL = "INSERT INTO tt_tmp_rgmi " _
    '                       & " SELECT DISTINCT m.* FROM rgmi m" _
    '                       & " WHERE m.deleted = 0 AND m.status = 0" _
    '                       & " AND m.micode IN " & v_lstMICODE & " AND m.deleted=0 AND m.status=0"
    '        v_obj.ExecuteNonQueryTran(CommandType.Text, v_strSQL)

    '        v_strSQL = "INSERT INTO TT_TMP_RGMI(AUTOID, MICODE) SELECT '0' AUTOID, '000' MICODE FROM DUAL"
    '        v_obj.ExecuteNonQueryTran(CommandType.Text, v_strSQL)

    '        'Lay chung khoan dc phan quyen cho user vao bang TT_TMP_RGSI
    '        v_strSQL = "INSERT INTO tt_tmp_rgsi(iscode, sicode, stock_name, part_value, TYPE," _
    '                        & "stock_type, interest_rate, interest_period, " _
    '                        & "bond_period, deleted, exchange_rate, note, " _
    '                        & "bond_term, release_series, release_mode, isin, " _
    '                        & "rarate, tmpid, paidmethod, npaiddate1, npaiddate2, " _
    '                        & "npaiddate3, npaiddate4, int_release_mode, brid, status) " _
    '                        & " SELECT distinct m.iscode, m.sicode, m.stock_name, m.part_value, m.TYPE," _
    '                        & "m.stock_type, m.interest_rate, m.interest_period," _
    '                        & "m.bond_period, m.deleted, m.exchange_rate, m.note, " _
    '                        & "m.bond_term, m.release_series, m.release_mode, m.isin, " _
    '                        & "m.rarate, m.tmpid, m.paidmethod, m.npaiddate1, m.npaiddate2," _
    '                        & "m.npaiddate3, m.npaiddate4, m.int_release_mode, m.brid, m.status FROM rgsi m" _
    '                        & " WHERE m.deleted = 0 AND m.sicode IN " & v_lstSICODE _
    '                        & " AND m.deleted=0 AND m.BRID IN " & v_lstBrID
    '        v_obj.ExecuteNonQueryTran(CommandType.Text, v_strSQL)

    '        v_strSQL = "INSERT INTO TT_TMP_RGSI(AUTOID, SICODE) SELECT 0 AUTOID, '000' SICODE FROM DUAL"
    '        v_obj.ExecuteNonQueryTran(CommandType.Text, v_strSQL)

    '        'Lay cau xu ly chung
    '        Dim v_hSQL As New Hashtable
    '        Dim v_hORD As New Hashtable
    '        If v_strTran <> "" Then
    '            v_strSQL = "SELECT * FROM RPSTORES WHERE RPTID IS NULL AND OVERWRITE IN " & v_strTran & " ORDER BY ODRNUM"

    '            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
    '            Dim v_strOverWrite As String
    '            If v_ds.Tables(0).Rows.Count > 0 Then
    '                For i As Integer = 0 To v_ds.Tables(0).Rows.Count - 1
    '                    v_strOverWrite = v_ds.Tables(0).Rows(i)("OVERWRITE")
    '                    v_hORD(i) = v_strOverWrite
    '                    v_hSQL(v_strOverWrite) = v_ds.Tables(0).Rows(i)("TRACE") & v_ds.Tables(0).Rows(i)("CMDSQL") & " " & v_ds.Tables(0).Rows(i)("CMDSQL1")
    '                Next
    '            End If

    '            'Lay cau xu ly chung da dc viet lai
    '            v_strSQL = "SELECT * FROM RPSTORES WHERE RPTID='" & v_strRptID & "' AND OVERWRITE IN " & v_strTran & " ORDER BY ODRNUM"
    '            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

    '            If v_ds.Tables(0).Rows.Count > 0 Then
    '                For i As Integer = 0 To v_ds.Tables(0).Rows.Count - 1
    '                    v_strOverWrite = v_ds.Tables(0).Rows(i)("OVERWRITE")
    '                    If Not v_hSQL(v_strOverWrite) Is Nothing Then
    '                        v_hSQL(v_strOverWrite) = v_ds.Tables(0).Rows(i)("TRACE") & v_ds.Tables(0).Rows(i)("CMDSQL") & " " & v_ds.Tables(0).Rows(i)("CMDSQL1")
    '                    End If
    '                Next
    '            End If

    '            'Thuc hien cau xu ly chung
    '            For i As Integer = 0 To v_hORD.Count - 1
    '                v_strOverWrite = v_hORD(i)
    '                v_strSQL = Mid(v_hSQL(v_strOverWrite).ToString, 2)

    '                v_strSQL = Replace(v_strSQL, "[!BRID]", v_strBRID)
    '                v_strSQL = Replace(v_strSQL, "[!TLID]", v_strTellerID)
    '                v_strSQL = Replace(v_strSQL, "[!LBRID]", v_lstBrID)
    '                v_strSQL = Replace(v_strSQL, "[!MINDATE]", "TO_DATE('" & v_strMinDate & "','dd/mm/yyyy')")
    '                v_strSQL = Replace(v_strSQL, "[!MAXDATE]", "TO_DATE('" & v_strMaxDate & "','dd/mm/yyyy')")

    '                v_strSQL = Replace(v_strSQL, "[!TLLOGALL]", "TLLOGALL")
    '                v_strSQL = Replace(v_strSQL, "[!CATRANA]", "CATRANA")
    '                v_strSQL = Replace(v_strSQL, "[!IATRANA]", "IATRANA")
    '                v_strSQL = Replace(v_strSQL, "[!MATRANA]", "MATRANA")
    '                v_strSQL = Replace(v_strSQL, "[!SFTRANA]", "SFTRANA")
    '                v_strSQL = Replace(v_strSQL, "[!MFTRANA]", "MFTRANA")
    '                v_strSQL = Replace(v_strSQL, "[!RATRANA]", "RATRANA")
    '                v_strSQL = Replace(v_strSQL, "[!RGTRANA]", "RGTRANA")

    '                For j As Integer = 0 To v_arrField.Count - 2
    '                    v_strSQL = Replace(v_strSQL, "[!" & v_arrField(j).Split("|")(0) & "]", v_arrField(j).Split("|")(1))
    '                Next

    '                If v_trace_status = "1" And Mid(v_hSQL(v_strOverWrite).ToString, 1, 1) = "1" Then
    '                    Trace.WriteLine("[RP - " & v_strRptID & "] " & DateTime.Now & " :" & vbCrLf & " -o- Câu lệnh thứ xử lý chung thứ #" & i + 1 & "-o-" & vbCrLf & v_strSQL & vbCrLf)
    '                    If InStr(v_strSQL.ToUpper, "SELECT") > 0 Then
    '                        Trace.WriteLine("-o- Dữ liệu câu lệnh xử lý chung thứ #" & i + 1 & "-o-")
    '                        v_strSQLTmp = v_strSQL.Substring(InStr(v_strSQL.ToUpper, "SELECT") - 1)
    '                        v_trace = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
    '                        Trace.WriteLine(v_trace.GetXml & vbCrLf)
    '                    End If
    '                End If

    '                v_obj.ExecuteNonQueryTran(CommandType.Text, v_strSQL)
    '            Next
    '        End If

    '        v_strSQL = "SELECT * FROM RPSTORES WHERE RPTID='" & v_strRptID & "' AND OVERWRITE IS NULL AND DELETED=0 AND STATUS=0 ORDER BY ODRNUM"
    '        v_ds1 = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

    '        Dim v_int As Integer

    '        For v_int = 0 To v_ds1.Tables(0).Rows.Count - 1

    '            v_strSQL = v_ds1.Tables(0).Rows(v_int)("CMDSQL").ToString.ToUpper & " " & v_ds1.Tables(0).Rows(v_int)("CMDSQL1").ToString.ToUpper

    '            If Not CheckTranTable(v_strSQL) Then
    '                v_lngErr = 1
    '                Exit For
    '            End If

    '            For i As Integer = 0 To v_arrField.Count - 2
    '                v_strSQL = Replace(v_strSQL, "[!" & v_arrField(i).Split("|")(0) & "]", v_arrField(i).Split("|")(1))
    '            Next

    '            v_strSQL = Replace(v_strSQL, "[!BRID]", v_strBRID)
    '            v_strSQL = Replace(v_strSQL, "[!TLID]", v_strTellerID)
    '            v_strSQL = Replace(v_strSQL, "[!LBRID]", v_lstBrID)
    '            v_strSQL = Replace(v_strSQL, "[!MINDATE]", "TO_DATE('" & v_strMinDate & "','dd/mm/yyyy')")
    '            v_strSQL = Replace(v_strSQL, "[!MAXDATE]", "TO_DATE('" & v_strMaxDate & "','dd/mm/yyyy')")

    '            If v_trace_status = "1" And v_ds1.Tables(0).Rows(v_int)("TRACE") = 1 Then
    '                Trace.WriteLine("[RP - " & v_strRptID & "] " & DateTime.Now & " :" & vbCrLf & " -o- Câu lệnh thứ #" & v_ds1.Tables(0).Rows(v_int)("ODRNUM") & "-o-" & vbCrLf & v_strSQL & vbCrLf)
    '                If InStr(v_strSQL.ToUpper, "SELECT") > 0 Then
    '                    Trace.WriteLine("-o- Dữ liệu câu lệnh thứ #" & v_ds1.Tables(0).Rows(v_int)("ODRNUM") & "-o-")
    '                    v_strSQLTmp = v_strSQL.Substring(InStr(v_strSQL.ToUpper, "SELECT") - 1)
    '                    v_trace = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
    '                    Trace.WriteLine(v_trace.GetXml & vbCrLf)
    '                End If
    '            End If

    '            v_obj.ExecuteNonQueryTran(CommandType.Text, v_strSQL)

    '        Next

    '        If v_lngErr = ERR_SYSTEM_OK Then

    '            v_strSQL = v_strMainSQL

    '            If Not CheckTranTable(v_strSQL) Then
    '                v_lngErr = 1
    '            Else
    '                For i As Integer = 0 To v_arrField.Count - 2
    '                    v_strSQL = Replace(v_strSQL, "[!" & v_arrField(i).Split("|")(0) & "]", v_arrField(i).Split("|")(1))
    '                Next

    '                If v_strSQL <> "" Then
    '                    v_strSQL = Replace(v_strSQL, "[!BRID]", v_strBRID)
    '                    v_strSQL = Replace(v_strSQL, "[!TLID]", v_strTellerID)
    '                    v_strSQL = Replace(v_strSQL, "[!LBRID]", v_lstBrID)
    '                    v_strSQL = Replace(v_strSQL, "[!MINDATE]", "TO_DATE('" & v_strMinDate & "','dd/mm/yyyy')")
    '                    v_strSQL = Replace(v_strSQL, "[!MAXDATE]", "TO_DATE('" & v_strMaxDate & "','dd/mm/yyyy')")

    '                    v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
    '                    If v_trace_status = "1" Then
    '                        Trace.WriteLine("[RP - " & v_strRptID & "] " & DateTime.Now & " :" & vbCrLf & " -o- Ket qua:-o-" & vbCrLf & v_strSQL & vbCrLf)
    '                        'Trace.WriteLine(v_ds.GetXml & vbCrLf)
    '                    End If
    '                End If
    '                BuildXMLObjData(v_ds, v_strObjMsg)
    '            End If
    '        End If
    '        pv_xmlDocument.LoadXml(v_strObjMsg)

    '        'If v_lngErr <> 0 Then
    '        '    Dim v_strErrorSource, v_strErrorMessage As String

    '        '    v_strErrorSource = "SISTORES"
    '        '    v_strErrorMessage = String.Empty

    '        '    LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
    '        '                 & "Error code: " & v_lngErr.ToString() & vbNewLine _
    '        '                 & "Error message: " & v_strErrorMessage, EventLogEntryType.Information)
    '        '    BuildXMLErrorException(pv_xmlDocument, v_strErrorSource, v_lngErr, v_strErrorMessage)
    '        'End If

    '        ContextUtil.SetComplete()
    '        Return v_lngErr
    '    Catch ex As Exception
    '        Throw ex
    '        ContextUtil.SetAbort()
    '        v_obj.Rollback()
    '    Finally
    '        If v_trace_status = "1" Then
    '            Trace.WriteLine("[Kết thúc: RP - " & v_strRptID & "] " & DateTime.Now & vbCrLf)
    '            tr2.Close()
    '            tr2.Dispose()
    '        End If
    '        v_obj.Rollback()
    '        v_obj.Dispose()
    '        GC.Collect()
    '    End Try
    'End Function
    'bangpv 
    Private Function CheckViewNetBank(ByRef pv_xmlDocument As XmlDocumentEx) As Long
        Dim v_lngErr As Long = ERR_SYSTEM_OK
        Dim v_obj As New DataAccess
        Try
            Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes
            Dim v_strSQL As String
            Dim v_strObjMsg As String
            Dim v_strClause As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCLAUSE), Xml.XmlAttribute).Value)
            Dim v_strTellerID As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTLID), Xml.XmlAttribute).Value)
            'Dim v_arrFilter As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCMDINQUIRY), Xml.XmlAttribute).Value)
            Dim v_strBRID As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeBRID), Xml.XmlAttribute).Value)
            Dim v_strCurdate As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTXDATE), Xml.XmlAttribute).Value)
            Dim v_strPartition As String
            Dim v_ds As DataSet
            If Not (v_attrColl.GetNamedItem(gc_AtributeTXNUM) Is Nothing) Then
                v_strPartition = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTXNUM), Xml.XmlAttribute).Value)
            Else
                v_strPartition = ""
            End If
            v_strPartition = GetListPartition(v_strPartition)
            v_obj.NewDBInstance(gc_MODULE_INQUERY)
            v_obj.BeginTran()
            v_strPartition = Replace(v_strPartition, "|", "")
            v_strObjMsg = pv_xmlDocument.InnerXml()
            'lay tu TLLOG 
            v_strSQL = "select count (*) status from tllog where tltxcd ='4084' and t_no =" & CInt(v_strClause) & " and txdate = " & "to_date('" & v_strCurdate & "', 'DD/MM/YYYY')"
            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            BuildXMLObjData(v_ds, v_strObjMsg)
            pv_xmlDocument.LoadXml(v_strObjMsg)

            v_obj.Commit()
            'ContextUtil.SetComplete()
            Return v_lngErr
        Catch ex As Exception
            Throw ex
            'ContextUtil.SetAbort()
            v_obj.Rollback()
        Finally
            v_obj.Dispose()
            GC.Collect()
        End Try
    End Function
    Private Function ExportViewNetBank(ByRef pv_xmlDocument As XmlDocumentEx) As Long
        Dim v_lngErr As Long = ERR_SYSTEM_OK
        Dim v_obj As New DataAccess
        Try
            Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes
            Dim v_strSQL As String
            Dim v_strObjMsg As String
            Dim v_strClause As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCLAUSE), Xml.XmlAttribute).Value)
            Dim v_strTellerID As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTLID), Xml.XmlAttribute).Value)
            'Dim v_arrFilter As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCMDINQUIRY), Xml.XmlAttribute).Value)
            Dim v_strBRID As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeBRID), Xml.XmlAttribute).Value)
            Dim v_strCurdate As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTXDATE), Xml.XmlAttribute).Value)
            Dim v_strPartition As String
            Dim v_ds As DataSet
            If Not (v_attrColl.GetNamedItem(gc_AtributeTXNUM) Is Nothing) Then
                v_strPartition = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTXNUM), Xml.XmlAttribute).Value)
            Else
                v_strPartition = ""
            End If
            Dim v_strStockType = "(" & IIf(v_strBRID = "1004", "1", IIf(v_strBRID = "1001", "2,3,4", "1,2,3,4")) & ")"

            v_strBRID = "(" & IIf(v_strBRID = "1004", "'0001','0002','0004','0006'", IIf(v_strBRID = "1001", "'0001','0002','0003'", IIf(v_strBRID = "1000", "'0001','0002'", "'" & v_strBRID & "'"))) & ")"

            v_strPartition = GetListPartition(v_strPartition)
            v_obj.NewDBInstance(gc_MODULE_INQUERY)
            v_obj.BeginTran() '
            v_strPartition = Replace(v_strPartition, "|", "")
            v_strObjMsg = pv_xmlDocument.InnerXml()
            'lay tu TLLOG 
            v_strSQL = "INSERT INTO TMP_TLLOG SELECT a.* FROM tllog a where a.brid in " & v_strBRID _
                    & " and a.tltxcd in ('4081','4082','4083','4084')" _
                    & " and  a.deleted=0 and a.txdate = to_date('" & v_strCurdate & "', 'DD/MM/YYYY')"
            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
            'Lay tu TLLOGALL_ALL voi du lieu qua khu
            v_strSQL = "INSERT INTO TMP_TLLOG SELECT a.* FROM [!TLLOGALL] a WHERE " _
                        & " a.brid in " & v_strBRID & "  and a.tltxcd in ('4081','4082','4083','4084')" _
                    & " and a.deleted=0 and a.txdate = to_date('" & v_strCurdate & "', 'DD/MM/YYYY')"
            v_strSQL = Replace(v_strSQL, "[!TLLOGALL]", "TLLOGALL_ALL PARTITION(" & v_strPartition & ")")
            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
            'Lay du lieu vao bang TMP_RGSI
            v_strSQL = "INSERT INTO tmp_rgsi(iscode, sicode, stock_name, part_value, TYPE," _
                            & "stock_type, interest_rate, interest_period, " _
                            & "bond_period, deleted, exchange_rate, note, " _
                            & "bond_term, release_series, release_mode, isin, " _
                            & "rarate, tmpid, paidmethod, npaiddate1, npaiddate2, " _
                            & "npaiddate3, npaiddate4, int_release_mode, brid, status, issuer_date, due_date,vsdbrid) " _
                            & " SELECT distinct m.iscode, m.sicode, m.stock_name, m.part_value, m.TYPE," _
                            & "m.stock_type, m.interest_rate, m.interest_period," _
                            & "m.bond_period, m.deleted, m.exchange_rate, m.note, " _
                            & "m.bond_term, m.release_series, m.release_mode, m.isin, " _
                            & "m.rarate, m.tmpid, m.paidmethod, m.npaiddate1, m.npaiddate2," _
                            & "m.npaiddate3, m.npaiddate4, m.int_release_mode, m.brid, m.status, m.issuer_date, m.due_date,m.vsdbrid FROM rgsi m" _
                            & " WHERE m.status=0 and  m.deleted=0 AND m.BRID in" & v_strBRID & " and m.type in  " & v_strStockType
            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
            ' Lay giao dich mua
            v_strSQL = "INSERT INTO tmp_tllog1" _
                    & "(autoid, txnum, txdate,busdate, brid, micode, comicode, msgamt, sicode,  " _
                    & "col_value01,col_value02, col_value03, col_value04, col_value05, col_value06," _
                    & "col_value07, col_value08,col_value09)" _
                    & "SELECT a.autoid, a.txnum, a.txdate,trunc(b.busdate) busdate, a.brid, substr(a.col_value10,1,3), a.col_value14, a.msgamt, a.sicode," _
                    & "CASE WHEN substr(a.col_value10,4,1) IN ('P','A','E') THEN " _
                    & "to_char(a.msgamt) " _
                    & "ELSE '0' " _
                    & "END MUATD, '0', " _
                    & "CASE WHEN substr(a.col_value10,4,1) IN ('C','B') THEN " _
                    & "to_char(a.msgamt) " _
                    & "ELSE '0' " _
                    & "END BANMGTN, '0', " _
                    & "CASE WHEN substr(a.col_value10,4,1) = 'F' THEN " _
                    & "to_char(a.msgamt) " _
                    & "ELSE '0' " _
                    & "END BANMGNN, '0', to_char(a.msgamt), '0', TO_CHAR(TO_NUMBER(a.col_value08)/1000) " _
                    & "FROM tmp_tllog a, tmp_tllog b where a.parentid=b.autoid  " _
                    & " and a.sicode in (select sicode from tmp_rgsi) " _
                    & " and a.t_no = " & CInt(v_strClause) _
                    & " and decode(a.t_no,3,a.col_value40,'1') = '1'" ' Them de chi lay truong hop thieu ck voi giao dich truc tiep
            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
            'Lay giao dich ban
            v_strSQL = "INSERT INTO tmp_tllog1" _
                    & "(autoid, txnum, txdate,  busdate, brid, micode, comicode, msgamt, sicode," _
                    & "col_value01,col_value02, col_value03, col_value04, col_value05, col_value06," _
                    & "col_value07, col_value08,col_value09) " _
                    & "SELECT a.autoid, a.txnum, a.txdate, trunc(b.busdate) busdate, a.brid, substr(a.col_value11,1,3), a.col_value15, a.msgamt, a.sicode," _
                    & "'0',CASE WHEN substr(a.col_value11,4,1) IN ('P','A','E') THEN " _
                    & "to_char(a.msgamt) " _
                    & "ELSE '0' " _
                    & "END MUATD, " _
                    & "'0', CASE WHEN substr(a.col_value11,4,1) IN ('C','B') THEN " _
                    & "to_char(a.msgamt) " _
                    & "ELSE '0' " _
                    & "END BANMGTN, " _
                    & "'0', CASE WHEN substr(a.col_value11,4,1) = 'F' THEN " _
                    & "to_char(a.msgamt) " _
                    & "ELSE '0' " _
                    & "END BANMGNN, '0', to_char(a.msgamt), TO_CHAR(TO_NUMBER(a.col_value08)/1000) " _
                    & "FROM tmp_tllog a, tmp_tllog b where a.parentid=b.autoid " _
                    & " and a.sicode in (select sicode from tmp_rgsi) " _
                    & " and a.t_no = " & CInt(v_strClause) _
                    & " and decode(a.t_no,3,a.col_value40,'1') = '1'" ' Them de chi lay truong hop thieu ck voi giao dich truc tiep
            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

            'Lay thong tin ra de export 
            'Nếu là thanh toán đa  phương 
            If CInt(v_strClause) <> 3 Then
                v_strSQL = "SELECT txdate, busdate, miname, micode,code_trade, muatd, bantd, " _
                    & "CASE WHEN muatd>bantd THEN  (muatd-bantd) ELSE 0  END DUOCNHANTD," _
                    & "CASE WHEN muatd<bantd THEN  (bantd-muatd) ELSE 0  END PHAIGIAOTD," _
                    & "muamgtn, banmgtn," _
                    & "CASE WHEN muamgtn>banmgtn THEN (muamgtn-banmgtn) ELSE 0 END DUOCNHANMGTN," _
                    & "CASE WHEN muamgtn<banmgtn THEN (banmgtn-muamgtn) ELSE 0 END PHAIGIAOMGTN," _
                    & "muamgnn, banmgnn, " _
                    & "CASE WHEN muamgnn>banmgnn THEN (muamgnn-banmgnn) ELSE 0 END DUOCNHANMGNN," _
                    & "CASE WHEN muamgnn<banmgnn THEN (banmgnn-muamgnn) ELSE 0 END PHAIGIAOMGNN," _
                    & "slmua, slban, " _
                    & "CASE WHEN slmua>slban THEN (slmua-slban) ELSE 0 END DUOCNHAN," _
                    & "CASE WHEN slmua<slban THEN (slban-slmua) ELSE 0 END PHAIGIAO,LCK " _
                    & " FROM " _
                    & "(SELECT   a.txdate,a.busdate, b.name miname, a.micode code_trade,b.micode, SUM (round(to_number(a.col_value01)*to_number(a.col_value09),3)) muatd, " _
                    & "SUM (round(to_number(a.col_value02)*to_number(a.col_value09),3)) bantd, SUM (round(to_number(a.col_value03)*to_number(a.col_value09),3)) muamgtn," _
                    & "SUM (round(to_number(a.col_value04)*to_number(a.col_value09),3)) banmgtn, SUM (round(to_number(a.col_value05)*to_number(a.col_value09),3)) muamgnn," _
                    & "SUM (round(to_number(a.col_value06)*to_number(a.col_value09),3)) banmgnn, SUM (round(to_number(a.col_value07)*to_number(a.col_value09),3)) slmua," _
                    & "SUM (round(to_number(a.col_value08)*to_number(a.col_value09),3)) slban, '' LCK " _
                    & "FROM tmp_tllog1 a , rgmi b" _
                    & " where(a.micode = b.code_trade and b.deleted =0 )  " _
                    & "GROUP BY  a.micode, b.micode,  b.name, a.txdate, a.busdate) order by micode"
                'nếu là thanh toán trực tiếp
            Else
                v_strSQL = "SELECT txdate, busdate, miname, micode,code_trade, muatd, bantd, " _
                    & " muatd DUOCNHANTD," _
                    & " /*bantd*/ 0  PHAIGIAOTD," _
                    & " muamgtn, banmgtn," _
                    & " muamgtn  DUOCNHANMGTN," _
                    & " /*banmgtn*/ 0 PHAIGIAOMGTN," _
                    & " muamgnn, round(banmgnn,1), " _
                    & " muamgnn DUOCNHANMGNN," _
                    & " /*banmgnn*/ 0 PHAIGIAOMGNN," _
                    & " slmua, slban, " _
                    & " slmua DUOCNHAN," _
                    & " /*slban */ 0 PHAIGIAO,LCK " _
                    & " FROM " _
                    & "(SELECT   a.txdate,max(a.busdate) busdate, b.name miname, a.micode code_trade,b.micode, SUM (round(to_number(a.col_value01)*to_number(a.col_value09),3)) muatd, " _
                    & "SUM (round(to_number(a.col_value02)*to_number(a.col_value09),3)) bantd, SUM (round(to_number(a.col_value03)*to_number(a.col_value09),3)) muamgtn," _
                    & "SUM (round(to_number(a.col_value04)*to_number(a.col_value09),3)) banmgtn, SUM (round(to_number(a.col_value05)*to_number(a.col_value09),3)) muamgnn," _
                    & "SUM (round(to_number(a.col_value06)*to_number(a.col_value09),3)) banmgnn, SUM (round(to_number(a.col_value07)*to_number(a.col_value09),3)) slmua," _
                    & "SUM (round(to_number(a.col_value08)*to_number(a.col_value09),3)) slban, '' LCK " _
                    & "FROM tmp_tllog1 a , rgmi b" _
                    & " where(a.micode = b.code_trade and b.deleted =0 )  " _
                    & "GROUP BY  a.micode, b.micode,  b.name, a.txdate) " _
                    & "where slmua>0 order by micode"
            End If

            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            BuildXMLObjData(v_ds, v_strObjMsg)
            pv_xmlDocument.LoadXml(v_strObjMsg)

            v_obj.Commit()
            'ContextUtil.SetComplete()
            Return v_lngErr
        Catch ex As Exception
            Throw ex
            'ContextUtil.SetAbort()
            v_obj.Rollback()
        Finally
            v_obj.Dispose()
            GC.Collect()
        End Try
    End Function
    'end bangpv 
    'bangpv 20150211
    Private Function ExportViewNetBank2(ByRef pv_xmlDocument As XmlDocumentEx) As Long
        Dim v_lngErr As Long = ERR_SYSTEM_OK
        Dim v_obj As New DataAccess
        Try
            Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes
            Dim v_strSQL As String
            Dim v_strObjMsg As String
            Dim v_strClause As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCLAUSE), Xml.XmlAttribute).Value)
            Dim v_strCSerrType As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCMDINQUIRY), Xml.XmlAttribute).Value)
            Dim v_strTellerID As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTLID), Xml.XmlAttribute).Value)
            'Dim v_arrFilter As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCMDINQUIRY), Xml.XmlAttribute).Value)
            Dim v_strBRID As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeBRID), Xml.XmlAttribute).Value)
            Dim v_strCurdate As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTXDATE), Xml.XmlAttribute).Value)
            Dim v_strPartition As String
            Dim v_ds As DataSet
            If Not (v_attrColl.GetNamedItem(gc_AtributeTXNUM) Is Nothing) Then
                v_strPartition = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTXNUM), Xml.XmlAttribute).Value)
            Else
                v_strPartition = ""
            End If
            Dim v_strStockType = "(" & IIf(v_strBRID = "1004", "1", IIf(v_strBRID = "1001", "2,3,4", "1,2,3,4")) & ")"

            v_strBRID = "(" & IIf(v_strBRID = "1004", "'0001','0002','0004','0006'", IIf(v_strBRID = "1001", "'0001','0002','0003'", IIf(v_strBRID = "1000", "'0001','0002'", "'" & v_strBRID & "'"))) & ")"

            v_strPartition = GetListPartition(v_strPartition)
            v_obj.NewDBInstance(gc_MODULE_INQUERY)
            v_obj.BeginTran() '
            v_strPartition = Replace(v_strPartition, "|", "")
            v_strObjMsg = pv_xmlDocument.InnerXml()
            'lay tu TLLOG 
            v_strSQL = "INSERT INTO TMP_TLLOG SELECT a.* FROM tllog a where a.brid in " & v_strBRID _
                    & " and a.tltxcd in ('4081','4082','4083','4084')" _
                    & " and  a.deleted=0 and a.txdate = to_date('" & v_strCurdate & "', 'DD/MM/YYYY') and a.status <3"
            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
            'Lay tu TLLOGALL_ALL voi du lieu qua khu
            v_strSQL = "INSERT INTO TMP_TLLOG SELECT a.* FROM [!TLLOGALL] a WHERE " _
                        & " a.brid in " & v_strBRID & "  and a.tltxcd in ('4081','4082','4083','4084')" _
                    & " and a.deleted=0 and a.txdate = to_date('" & v_strCurdate & "', 'DD/MM/YYYY')"
            v_strSQL = Replace(v_strSQL, "[!TLLOGALL]", "TLLOGALL_ALL PARTITION(" & v_strPartition & ")")
            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

            'bangpv: lay thong tin gia cua quyen 
            v_strSQL = " INSERT INTO tmp_rgsi1 (sicode, effective_date, part_value, type,brid) " _
                           & "SELECT a.sicode, a.msgdate,b.CLOSE_price - b.price DELTA,a.type ,a.brid " _
                           & "FROM " _
                           & " ( " _
                           & "SELECT DISTINCT a.sicode, a.msgdate, get_t_plus(a.msgdate,a.brid,-2) tc_date,get_t_plus(a.msgdate,a.brid,-1) tc_1_date, b.type,b.brid " _
                           & " FROM tllog a, rgsi b  WHERE a.tltxcd IN ('6050','6054','6055','6056') AND a.status =3  " _
                           & " AND (msgdate =get_t_plus(to_date('" & v_strCurdate & "', 'DD/MM/YYYY'),a.brid,decode(b.type,1,2,3)) " _
                           & " OR msgdate =get_t_plus(to_date('" & v_strCurdate & "', 'DD/MM/YYYY'),a.brid,decode(b.type,1,1,4)))" _
                           & " AND a.brid in " & v_strBRID & " AND a.brid = b.brid AND a.sicode = b.sicode AND b.status =0 AND b.deleted =0 ) a " _
                           & " , rgsi_info b WHERE a.sicode = b.sicode  AND a.tc_date= b.tradedate "
            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
            'Lay du lieu vao bang TMP_RGSI
            v_strSQL = "INSERT INTO tmp_rgsi(iscode, sicode, stock_name, part_value, TYPE," _
                            & "stock_type, interest_rate, interest_period, " _
                            & "bond_period, deleted, exchange_rate, note, " _
                            & "bond_term, release_series, release_mode, isin, " _
                            & "rarate, tmpid, paidmethod, npaiddate1, npaiddate2, " _
                            & "npaiddate3, npaiddate4, int_release_mode, brid, status, issuer_date, due_date,vsdbrid) " _
                            & " SELECT distinct m.iscode, m.sicode, m.stock_name, m.part_value, m.TYPE," _
                            & "m.stock_type, m.interest_rate, m.interest_period," _
                            & "m.bond_period, m.deleted, m.exchange_rate, m.note, " _
                            & "m.bond_term, m.release_series, m.release_mode, m.isin, " _
                            & "m.rarate, m.tmpid, m.paidmethod, m.npaiddate1, m.npaiddate2," _
                            & "m.npaiddate3, m.npaiddate4, m.int_release_mode, m.brid, m.status, m.issuer_date, m.due_date,m.vsdbrid FROM rgsi m" _
                            & " WHERE m.status=0 and  m.deleted=0 AND m.BRID in" & v_strBRID & " and m.type in  " & v_strStockType
            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
            '@2: LAY CHENH LECH GIA
            ' Lay giao dich mua
            v_strSQL = "INSERT INTO tmp_tllog1" _
                    & "(autoid, txnum, txdate, busdate, brid, micode, comicode, msgamt, sicode,  " _
                    & "col_value01,col_value02, col_value03, col_value04, col_value05, col_value06," _
                    & "col_value07, col_value08,col_value09)" _
                    & "SELECT a.autoid, a.txnum, a.txdate, trunc(b.busdate) busdate, a.brid, substr(a.col_value10,1,3), a.col_value14, a.msgamt, a.sicode," _
                    & "CASE WHEN substr(a.col_value10,4,1) IN ('P','A','E') THEN " _
                    & "to_char(a.msgamt) " _
                    & "ELSE '0' " _
                    & "END MUATD, '0', " _
                    & "CASE WHEN substr(a.col_value10,4,1) IN ('C','B') THEN " _
                    & "to_char(a.msgamt) " _
                    & "ELSE '0' " _
                    & "END BANMGTN, '0', " _
                    & "CASE WHEN substr(a.col_value10,4,1) = 'F' THEN " _
                    & "to_char(a.msgamt) " _
                    & "ELSE '0' " _
                    & "END BANMGNN, '0', to_char(a.msgamt), '0', TO_CHAR((-1*(CASE WHEN nvl(d.part_value,0)<0 THEN 0 ELSE nvl(d.part_value,0) end))/1000) " _
                    & "FROM tmp_tllog a, tmp_tllog b, " _
                    & " (SELECT TO_DATE(A.COL_VALUE03,'dd/MM/yyyy') TXDATE , B.COL_VALUE05 CONFIRMNO FROM TLLOG A , TLLOG B " _
                    & " WHERE(A.AUTOID = B.PARENTID And A.STATUS = B.STATUS And A.DELETED = B.DELETED) " _
                    & " AND A.STATUS =2 AND A.DELETED =0 AND A.TLTXCD ='4034') C, " _
                    & " (SELECT effective_date msgdate, sicode, get_t_plus(effective_date,brid,decode(TYPE,2,-3,-1)) effective_date, " _
                    & "  get_t_plus(effective_date,brid,decode(TYPE,2,-4,-2)) effective_date1 , part_value, TYPE FROM tmp_rgsi1) d " _
                    & " where a.parentid=b.autoid  " _
                    & " and a.t_no = 3" _
                    & " and a.col_value40 = '" & v_strCSerrType & "'" _
                    & " and a.sicode in (select sicode from tmp_rgsi) " _
                    & " AND A.COL_VALUE04= C.CONFIRMNO AND A.TXDATE = C.TXDATE" _
                    & " AND ( a.txdate = d.effective_date OR A.TXDATE = d.effective_date1) " _
                    & " AND A.SICODE =D.SICODE "
            
            '& " and a.col_value04 in (select col_value05 from tllog where tltxcd ='4035' and status =2 and deleted =0)" ' Them de chi lay truong hop thieu ck voi giao dich truc tiep
            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
            'Lay giao dich ban
            v_strSQL = "INSERT INTO tmp_tllog1" _
                    & "(autoid, txnum, txdate,  busdate, brid, micode, comicode, msgamt, sicode," _
                    & "col_value01,col_value02, col_value03, col_value04, col_value05, col_value06," _
                    & "col_value07, col_value08,col_value09) " _
                    & "SELECT a.autoid, a.txnum, a.txdate,  trunc(b.busdate) busdate, a.brid, substr(a.col_value11,1,3), a.col_value15, a.msgamt, a.sicode," _
                    & "'0',CASE WHEN substr(a.col_value11,4,1) IN ('P','A','E') THEN " _
                    & "to_char(a.msgamt) " _
                    & "ELSE '0' " _
                    & "END MUATD, " _
                    & "'0', CASE WHEN substr(a.col_value11,4,1) IN ('C','B') THEN " _
                    & "to_char(a.msgamt) " _
                    & "ELSE '0' " _
                    & "END BANMGTN, " _
                    & "'0', CASE WHEN substr(a.col_value11,4,1) = 'F' THEN " _
                    & "to_char(a.msgamt) " _
                    & "ELSE '0' " _
                    & "END BANMGNN, '0', to_char(a.msgamt), TO_CHAR((-1*(CASE WHEN nvl(d.part_value,0)<0 THEN 0 ELSE nvl(d.part_value,0) end))/1000) " _
                    & "FROM tmp_tllog a, tmp_tllog b, " _
                    & " (SELECT TO_DATE(A.COL_VALUE03,'dd/MM/yyyy') TXDATE , B.COL_VALUE05 CONFIRMNO FROM TLLOG A , TLLOG B " _
                    & " WHERE(A.AUTOID = B.PARENTID And A.STATUS = B.STATUS And A.DELETED = B.DELETED) " _
                    & " AND A.STATUS =2 AND A.DELETED =0 AND A.TLTXCD ='4034') C,  " _
                    & " (SELECT effective_date msgdate, sicode, get_t_plus(effective_date,brid,decode(TYPE,2,-3,-1)) effective_date, " _
                    & "  get_t_plus(effective_date,brid,decode(TYPE,2,-4,-2)) effective_date1 , part_value, TYPE FROM tmp_rgsi1) d " _
                    & " where a.parentid=b.autoid  " _
                    & " and a.t_no = 3" _
                    & " and a.col_value40 = '" & v_strCSerrType & "'" _
                    & " and a.sicode in (select sicode from tmp_rgsi) " _
                    & " AND A.COL_VALUE04= C.CONFIRMNO AND A.TXDATE = C.TXDATE " _
                    & " AND ( a.txdate = d.effective_date OR A.TXDATE = d.effective_date1) " _
                    & " AND A.SICODE =D.SICODE "

            '& " and a.col_value04 in (select col_value05 from tllog where tltxcd ='4035' and status =2 and deleted =0)" ' Them de chi lay truong hop thieu ck voi giao dich truc tiep
            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
            'END @2: KET THUC
            ' Lay giao dich mua
            v_strSQL = "INSERT INTO tmp_tllog1" _
                    & "(autoid, txnum, txdate, busdate, brid, micode, comicode, msgamt, sicode,  " _
                    & "col_value01,col_value02, col_value03, col_value04, col_value05, col_value06," _
                    & "col_value07, col_value08,col_value09)" _
                    & "SELECT a.autoid, a.txnum, a.txdate, trunc(b.busdate) busdate, a.brid, substr(a.col_value10,1,3), a.col_value14, a.msgamt, a.sicode," _
                    & "CASE WHEN substr(a.col_value10,4,1) IN ('P','A','E') THEN " _
                    & "to_char(a.msgamt) " _
                    & "ELSE '0' " _
                    & "END MUATD, '0', " _
                    & "CASE WHEN substr(a.col_value10,4,1) IN ('C','B') THEN " _
                    & "to_char(a.msgamt) " _
                    & "ELSE '0' " _
                    & "END BANMGTN, '0', " _
                    & "CASE WHEN substr(a.col_value10,4,1) = 'F' THEN " _
                    & "to_char(a.msgamt) " _
                    & "ELSE '0' " _
                    & "END BANMGNN, '0', to_char(a.msgamt), '0', TO_CHAR(TO_NUMBER(a.col_value08)/1000) " _
                    & "FROM tmp_tllog a, tmp_tllog b, " _
                    & " (SELECT TO_DATE(A.COL_VALUE03,'dd/MM/yyyy') TXDATE , B.COL_VALUE05 CONFIRMNO FROM TLLOG A , TLLOG B " _
                    & " WHERE(A.AUTOID = B.PARENTID And A.STATUS = B.STATUS And A.DELETED = B.DELETED) " _
                    & " AND A.STATUS =2 AND A.DELETED =0 AND A.TLTXCD ='4034') C " _
                    & " where a.parentid=b.autoid  " _
                    & " and a.t_no = 3" _
                    & " and a.col_value40 = '" & v_strCSerrType & "'" _
                    & " and a.sicode in (select sicode from tmp_rgsi) " _
                    & " AND A.COL_VALUE04= C.CONFIRMNO AND A.TXDATE = C.TXDATE"
            '& " and a.col_value04 in (select col_value05 from tllog where tltxcd ='4035' and status =2 and deleted =0)" ' Them de chi lay truong hop thieu ck voi giao dich truc tiep
            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
            'Lay giao dich ban
            v_strSQL = "INSERT INTO tmp_tllog1" _
                    & "(autoid, txnum, txdate,  busdate, brid, micode, comicode, msgamt, sicode," _
                    & "col_value01,col_value02, col_value03, col_value04, col_value05, col_value06," _
                    & "col_value07, col_value08,col_value09) " _
                    & "SELECT a.autoid, a.txnum, a.txdate,  trunc(b.busdate) busdate, a.brid, substr(a.col_value11,1,3), a.col_value15, a.msgamt, a.sicode," _
                    & "'0',CASE WHEN substr(a.col_value11,4,1) IN ('P','A','E') THEN " _
                    & "to_char(a.msgamt) " _
                    & "ELSE '0' " _
                    & "END MUATD, " _
                    & "'0', CASE WHEN substr(a.col_value11,4,1) IN ('C','B') THEN " _
                    & "to_char(a.msgamt) " _
                    & "ELSE '0' " _
                    & "END BANMGTN, " _
                    & "'0', CASE WHEN substr(a.col_value11,4,1) = 'F' THEN " _
                    & "to_char(a.msgamt) " _
                    & "ELSE '0' " _
                    & "END BANMGNN, '0', to_char(a.msgamt), TO_CHAR(TO_NUMBER(a.col_value08)/1000) " _
                    & "FROM tmp_tllog a, tmp_tllog b, " _
                    & " (SELECT TO_DATE(A.COL_VALUE03,'dd/MM/yyyy') TXDATE , B.COL_VALUE05 CONFIRMNO FROM TLLOG A , TLLOG B " _
                    & " WHERE(A.AUTOID = B.PARENTID And A.STATUS = B.STATUS And A.DELETED = B.DELETED) " _
                    & " AND A.STATUS =2 AND A.DELETED =0 AND A.TLTXCD ='4034') C " _
                    & " where a.parentid=b.autoid  " _
                    & " and a.t_no = 3" _
                    & " and a.col_value40 = '" & v_strCSerrType & "'" _
                    & " and a.sicode in (select sicode from tmp_rgsi) " _
                    & " AND A.COL_VALUE04= C.CONFIRMNO AND A.TXDATE = C.TXDATE"
            '& " and a.col_value04 in (select col_value05 from tllog where tltxcd ='4035' and status =2 and deleted =0)" ' Them de chi lay truong hop thieu ck voi giao dich truc tiep
            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
            'Lay thong tin ra de export 
            'Nếu là thanh toán đa  phương 
            If CInt(v_strClause) <> 3 Then
                v_strSQL = "SELECT txdate, busdate, miname, micode,code_trade, muatd, bantd, " _
                    & "CASE WHEN muatd>bantd THEN  (muatd-bantd) ELSE 0  END DUOCNHANTD," _
                    & "CASE WHEN muatd<bantd THEN  (bantd-muatd) ELSE 0  END PHAIGIAOTD," _
                    & "muamgtn, banmgtn," _
                    & "CASE WHEN muamgtn>banmgtn THEN (muamgtn-banmgtn) ELSE 0 END DUOCNHANMGTN," _
                    & "CASE WHEN muamgtn<banmgtn THEN (banmgtn-muamgtn) ELSE 0 END PHAIGIAOMGTN," _
                    & "muamgnn, banmgnn, " _
                    & "CASE WHEN muamgnn>banmgnn THEN (muamgnn-banmgnn) ELSE 0 END DUOCNHANMGNN," _
                    & "CASE WHEN muamgnn<banmgnn THEN (banmgnn-muamgnn) ELSE 0 END PHAIGIAOMGNN," _
                    & "slmua, slban, " _
                    & "CASE WHEN slmua>slban THEN (slmua-slban) ELSE 0 END DUOCNHAN," _
                    & "CASE WHEN slmua<slban THEN (slban-slmua) ELSE 0 END PHAIGIAO,LCK " _
                    & " FROM " _
                    & "(SELECT txdate,miname,busdate, code_Trade, micode, sum(muatd) muatd, sum(bantd) bantd, sum(muamgtn) muamgtn, sum(banmgtn) banmgtn, " _
                    & " sum(muamgnn) muamgnn, sum(banmgnn) banmgnn, sum(slmua) slmua, sum(slban) slban, max(lck) lck " _
                    & " FROM " _
                    & "(SELECT   a.txdate,a.busdate, CASE WHEN to_number(b.micode)>=800 THEN 'Tổng Công ty Lưu ký và Bù trừ Chứng khoán Việt nam' ELSE to_char(b.name) END miname, " _
                    & "CASE WHEN to_number(b.micode)>=800 THEN '800' ELSE to_char(a.micode) END code_trade, " _
                    & " CASE WHEN to_number(b.micode)>=800 THEN '800' ELSE to_char(b.micode) END micode, " _
                    & "SUM (round(to_number(a.col_value01)*to_number(a.col_value09),3)) muatd, " _
                    & "SUM (round(to_number(a.col_value02)*to_number(a.col_value09),3)) bantd, SUM (round(to_number(a.col_value03)*to_number(a.col_value09),3)) muamgtn," _
                    & "SUM (round(to_number(a.col_value04)*to_number(a.col_value09),3)) banmgtn, SUM (round(to_number(a.col_value05)*to_number(a.col_value09),3)) muamgnn," _
                    & "SUM (round(to_number(a.col_value06)*to_number(a.col_value09),3)) banmgnn, SUM (round(to_number(a.col_value07)*to_number(a.col_value09),3)) slmua," _
                    & "SUM (round(to_number(a.col_value08)*to_number(a.col_value09),3)) slban, '' LCK " _
                    & "FROM tmp_tllog1 a , rgmi b" _
                    & " where(a.micode = b.code_trade and b.deleted =0 )  " _
                    & "GROUP BY  a.micode, b.micode,  b.name, a.txdate, a.busdate) " _
                    & "GROUP BY busdate, code_trade, micode,miname,txdate,busdate) " _
                    & "order by micode"
                'v_strSQL = "SELECT txdate, busdate, miname, micode,code_trade, muatd, bantd, " _
                '    & "CASE WHEN muatd>bantd THEN  (muatd-bantd) ELSE 0  END DUOCNHANTD," _
                '    & "CASE WHEN muatd<bantd THEN  (bantd-muatd) ELSE 0  END PHAIGIAOTD," _
                '    & "muamgtn, banmgtn," _
                '    & "CASE WHEN muamgtn>banmgtn THEN (muamgtn-banmgtn) ELSE 0 END DUOCNHANMGTN," _
                '    & "CASE WHEN muamgtn<banmgtn THEN (banmgtn-muamgtn) ELSE 0 END PHAIGIAOMGTN," _
                '    & "muamgnn, banmgnn, " _
                '    & "CASE WHEN muamgnn>banmgnn THEN (muamgnn-banmgnn) ELSE 0 END DUOCNHANMGNN," _
                '    & "CASE WHEN muamgnn<banmgnn THEN (banmgnn-muamgnn) ELSE 0 END PHAIGIAOMGNN," _
                '    & "slmua, slban, " _
                '    & "CASE WHEN slmua>slban THEN (slmua-slban) ELSE 0 END DUOCNHAN," _
                '    & "CASE WHEN slmua<slban THEN (slban-slmua) ELSE 0 END PHAIGIAO,LCK " _
                '    & " FROM " _
                '    & "(SELECT   a.txdate,a.busdate, b.name miname, a.micode code_trade,b.micode, SUM (round(to_number(a.col_value01)*to_number(a.col_value09),3)) muatd, " _
                '    & "SUM (round(to_number(a.col_value02)*to_number(a.col_value09),3)) bantd, SUM (round(to_number(a.col_value03)*to_number(a.col_value09),3)) muamgtn," _
                '    & "SUM (round(to_number(a.col_value04)*to_number(a.col_value09),3)) banmgtn, SUM (round(to_number(a.col_value05)*to_number(a.col_value09),3)) muamgnn," _
                '    & "SUM (round(to_number(a.col_value06)*to_number(a.col_value09),3)) banmgnn, SUM (round(to_number(a.col_value07)*to_number(a.col_value09),3)) slmua," _
                '    & "SUM (round(to_number(a.col_value08)*to_number(a.col_value09),3)) slban, '' LCK " _
                '    & "FROM tmp_tllog1 a , rgmi b" _
                '    & " where(a.micode = b.code_trade and b.deleted =0 )  " _
                '    & "GROUP BY  a.micode, b.micode,  b.name, a.txdate, a.busdate) order by micode"
                'nếu là thanh toán trực tiếp
            Else
                v_strSQL = "SELECT txdate, busdate, miname, micode,code_trade, muatd, bantd, " _
                    & " muatd DUOCNHANTD," _
                    & " /*bantd*/ 0  PHAIGIAOTD," _
                    & " muamgtn, banmgtn," _
                    & " muamgtn  DUOCNHANMGTN," _
                    & " /*banmgtn*/ 0 PHAIGIAOMGTN," _
                    & " muamgnn, round(banmgnn,1), " _
                    & " muamgnn DUOCNHANMGNN," _
                    & " /*banmgnn*/ 0 PHAIGIAOMGNN," _
                    & " slmua, slban, " _
                    & " slmua DUOCNHAN," _
                    & " /*slban */ 0 PHAIGIAO,LCK " _
                    & " FROM " _
                    & "(SELECT miname,txdate,busdate, code_Trade, micode, sum(muatd) muatd, sum(bantd) bantd, sum(muamgtn) muamgtn, sum(banmgtn) banmgtn, " _
                    & " sum(muamgnn) muamgnn, sum(banmgnn) banmgnn, sum(slmua) slmua, sum(slban) slban, max(lck) lck " _
                    & " FROM " _
                    & "(SELECT   a.txdate,max(a.busdate) busdate,CASE WHEN to_number(b.micode)>=800 THEN 'Tổng Công ty Lưu ký và Bù trừ Chứng khoán Việt nam' ELSE to_char(b.name) END miname, " _
                    & "CASE WHEN to_number(b.micode)>=800 THEN '800' ELSE to_char(a.micode) END code_trade, " _
                    & " CASE WHEN to_number(b.micode)>=800 THEN '800' ELSE to_char(b.micode) END micode, " _
                    & "SUM (round(to_number(a.col_value01)*to_number(a.col_value09),3)) muatd, " _
                    & "SUM (round(to_number(a.col_value02)*to_number(a.col_value09),3)) bantd, SUM (round(to_number(a.col_value03)*to_number(a.col_value09),3)) muamgtn," _
                    & "SUM (round(to_number(a.col_value04)*to_number(a.col_value09),3)) banmgtn, SUM (round(to_number(a.col_value05)*to_number(a.col_value09),3)) muamgnn," _
                    & "SUM (round(to_number(a.col_value06)*to_number(a.col_value09),3)) banmgnn, SUM (round(to_number(a.col_value07)*to_number(a.col_value09),3)) slmua," _
                    & "SUM (round(to_number(a.col_value08)*to_number(a.col_value09),3)) slban, '' LCK " _
                    & "FROM tmp_tllog1 a , rgmi b" _
                    & " where(a.micode = b.code_trade and b.deleted =0 )  " _
                    & "GROUP BY  a.micode, b.micode,  b.name, a.txdate) " _
                    & "GROUP BY busdate, code_trade, micode,miname,txdate) " _
                    & "where slmua>0 order by micode"
                'v_strSQL = "SELECT txdate, busdate, miname, micode,code_trade, muatd, bantd, " _
                '    & " muatd DUOCNHANTD," _
                '    & " bantd PHAIGIAOTD," _
                '    & " muamgtn, banmgtn," _
                '    & " muamgtn  DUOCNHANMGTN," _
                '    & " banmgtn PHAIGIAOMGTN," _
                '    & " muamgnn, round(banmgnn,1), " _
                '    & " muamgnn DUOCNHANMGNN," _
                '    & " banmgnn PHAIGIAOMGNN," _
                '    & " slmua, slban, " _
                '    & " slmua DUOCNHAN," _
                '    & " slban  PHAIGIAO,LCK " _
                '    & " FROM " _
                '    & "(SELECT   a.txdate,a.busdate, b.name miname, a.micode code_trade,b.micode, SUM (round(to_number(a.col_value01)*to_number(a.col_value09),3)) muatd, " _
                '    & "SUM (round(to_number(a.col_value02)*to_number(a.col_value09),3)) bantd, SUM (round(to_number(a.col_value03)*to_number(a.col_value09),3)) muamgtn," _
                '    & "SUM (round(to_number(a.col_value04)*to_number(a.col_value09),3)) banmgtn, SUM (round(to_number(a.col_value05)*to_number(a.col_value09),3)) muamgnn," _
                '    & "SUM (round(to_number(a.col_value06)*to_number(a.col_value09),3)) banmgnn, SUM (round(to_number(a.col_value07)*to_number(a.col_value09),3)) slmua," _
                '    & "SUM (round(to_number(a.col_value08)*to_number(a.col_value09),3)) slban, '' LCK " _
                '    & "FROM tmp_tllog1 a , rgmi b" _
                '    & " where(a.micode = b.code_trade and b.deleted =0 )  " _
                '    & "GROUP BY  a.micode, b.micode,  b.name, a.txdate, a.busdate) order by micode"
            End If

            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            BuildXMLObjData(v_ds, v_strObjMsg)
            pv_xmlDocument.LoadXml(v_strObjMsg)

            v_obj.Commit()
            'ContextUtil.SetComplete()
            Return v_lngErr
        Catch ex As Exception
            Throw ex
            'ContextUtil.SetAbort()
            v_obj.Rollback()
        Finally
            v_obj.Dispose()
            GC.Collect()
        End Try
    End Function
    'end bangpv 
    Private Function ExportViewNetBank3(ByRef pv_xmlDocument As XmlDocumentEx) As Long
        Dim v_lngErr As Long = ERR_SYSTEM_OK
        Dim v_obj As New DataAccess
        Try
            Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes
            Dim v_strSQL As String
            Dim v_strObjMsg As String
            'Dim v_strClause As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCLAUSE), Xml.XmlAttribute).Value)
            ' Dim v_strCSerrType As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCMDINQUIRY), Xml.XmlAttribute).Value)
            Dim v_strTellerID As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTLID), Xml.XmlAttribute).Value)
            'Dim v_arrFilter As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCMDINQUIRY), Xml.XmlAttribute).Value)
            Dim v_strBRID As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeBRID), Xml.XmlAttribute).Value)
            Dim v_strCurdate As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTXDATE), Xml.XmlAttribute).Value)
            Dim v_strPartition As String
            Dim v_ds As DataSet
            If Not (v_attrColl.GetNamedItem(gc_AtributeTXNUM) Is Nothing) Then
                v_strPartition = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTXNUM), Xml.XmlAttribute).Value)
            Else
                v_strPartition = ""
            End If
            Dim v_strStockType = "(" & IIf(v_strBRID = "1004", "1", IIf(v_strBRID = "1001", "2,3,4", "1,2,3,4")) & ")"

            v_strBRID = "(" & IIf(v_strBRID = "1004", "'0001','0002','0004','0006'", IIf(v_strBRID = "1001", "'0001','0002','0003'", IIf(v_strBRID = "1000", "'0001','0002'", "'" & v_strBRID & "'"))) & ")"

            v_strPartition = GetListPartition(v_strPartition)
            v_obj.NewDBInstance(gc_MODULE_INQUERY)
            v_obj.BeginTran() '
            v_strPartition = Replace(v_strPartition, "|", "")
            v_strObjMsg = pv_xmlDocument.InnerXml()
            'lay tu TLLOG 
            v_strSQL = "INSERT INTO TMP_TLLOG SELECT a.* FROM tllog a where a.brid in " & v_strBRID _
                    & " and a.tltxcd in ('4081','4082','4083','4084')" _
                    & " and   a.txdate = to_date('" & v_strCurdate & "', 'DD/MM/YYYY') and a.status in (2,3,4)"
            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
            'Lay tu TLLOGALL_ALL voi du lieu qua khu
            ' v_strSQL = "INSERT INTO TMP_TLLOG SELECT a.* FROM [!TLLOGALL] a WHERE " _
            '            & " a.brid in " & v_strBRID & "  and a.tltxcd in ('4081','4082','4083','4084')" _
            '       & " and  a.txdate = to_date('" & v_strCurdate & "', 'DD/MM/YYYY') and  a.status in (2,3,4)"
            'v_strSQL = Replace(v_strSQL, "[!TLLOGALL]", "TLLOGALL_ALL PARTITION(" & v_strPartition & ")")
            'v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
            'Lay du lieu vao bang TMP_RGSI
            v_strSQL = "INSERT INTO tmp_rgsi(iscode, sicode, stock_name, part_value, TYPE," _
                            & "stock_type, interest_rate, interest_period, " _
                            & "bond_period, deleted, exchange_rate, note, " _
                            & "bond_term, release_series, release_mode, isin, " _
                            & "rarate, tmpid, paidmethod, npaiddate1, npaiddate2, " _
                            & "npaiddate3, npaiddate4, int_release_mode, brid, status, issuer_date, due_date,vsdbrid) " _
                            & " SELECT distinct m.iscode, m.sicode, m.stock_name, m.part_value, m.TYPE," _
                            & "m.stock_type, m.interest_rate, m.interest_period," _
                            & "m.bond_period, m.deleted, m.exchange_rate, m.note, " _
                            & "m.bond_term, m.release_series, m.release_mode, m.isin, " _
                            & "m.rarate, m.tmpid, m.paidmethod, m.npaiddate1, m.npaiddate2," _
                            & "m.npaiddate3, m.npaiddate4, m.int_release_mode, m.brid, m.status, m.issuer_date, m.due_date,m.vsdbrid FROM rgsi m" _
                            & " WHERE m.status=0 and  m.deleted=0 AND m.BRID in" & v_strBRID & " and m.type in  " & v_strStockType
            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
            v_strSQL = " INSERT INTO tmp_rgsi1 (sicode, effective_date, part_value, type,brid) " _
                          & "SELECT a.sicode, a.msgdate,b.CLOSE_price - b.price DELTA,a.type ,a.brid " _
                          & "FROM " _
                          & " ( " _
                          & "SELECT DISTINCT a.sicode, a.msgdate, get_t_plus(a.msgdate,a.brid,-2) tc_date,get_t_plus(a.msgdate,a.brid,-1) tc_1_date, b.type,b.brid " _
                          & " FROM tllog a, rgsi b  WHERE a.tltxcd IN ('6050','6054','6055','6056') AND a.status =3  " _
                          & " AND (msgdate =get_t_plus(to_date('" & v_strCurdate & "', 'DD/MM/YYYY'),a.brid,decode(b.type,1,2,3)) " _
                          & " OR msgdate =get_t_plus(to_date('" & v_strCurdate & "', 'DD/MM/YYYY'),a.brid,decode(b.type,1,1,4)))" _
                          & " AND a.brid in " & v_strBRID & " AND a.brid = b.brid AND a.sicode = b.sicode AND b.status =0 AND b.deleted =0 ) a " _
                          & " , rgsi_info b WHERE a.sicode = b.sicode  AND a.tc_date= b.tradedate "
            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
            '@2: Lay giao dich thanh toan nhung bi anh huong gia lui quyen nên phải giải tỏa
            ' Lay giao dich mua
            v_strSQL = "INSERT INTO tmp_tllog1" _
                    & "(autoid, txnum, txdate, busdate, brid, micode, comicode, msgamt, sicode,  " _
                    & "col_value01,col_value02, col_value03, col_value04, col_value05, col_value06," _
                    & "col_value07, col_value08,col_value09)" _
                    & "SELECT a.autoid, a.txnum, a.txdate, trunc(b.busdate) busdate, a.brid, substr(a.col_value10,1,3), a.col_value14, a.msgamt, a.sicode," _
                    & "CASE WHEN substr(a.col_value10,4,1) IN ('P','A','E') THEN " _
                    & "to_char(a.msgamt) " _
                    & "ELSE '0' " _
                    & "END MUATD, '0', " _
                    & "CASE WHEN substr(a.col_value10,4,1) IN ('C','B') THEN " _
                    & "to_char(a.msgamt) " _
                    & "ELSE '0' " _
                    & "END BANMGTN, '0', " _
                    & "CASE WHEN substr(a.col_value10,4,1) = 'F' THEN " _
                    & "to_char(a.msgamt) " _
                    & "ELSE '0' " _
                    & "END BANMGNN, '0', to_char(a.msgamt), '0', TO_CHAR(((CASE WHEN nvl(d.part_value,0)<0 THEN 0 ELSE nvl(d.part_value,0) end))/1000) " _
                    & "FROM tmp_tllog a, tmp_tllog b, " _
                    & " (SELECT effective_date msgdate, sicode, get_t_plus(effective_date,brid,decode(TYPE,2,-3,-1)) effective_date, " _
                    & "  get_t_plus(effective_date,brid,decode(TYPE,2,-4,-2)) effective_date1 , part_value, TYPE FROM tmp_rgsi1) d " _
                    & " where a.parentid=b.autoid  " _
                    & " and a.t_no = 3" _
                    & " and a.col_value40 = '1'" _
                    & " and a.sicode in (select sicode from tmp_rgsi) " _
                    & " and b.status =3 " _
                    & " AND ( a.txdate = d.effective_date OR A.TXDATE = d.effective_date1) " _
                    & " AND A.SICODE =D.SICODE and trunc(b.busdate)> d.msgdate "

           
            '& " and a.col_value04 in (select col_value05 from tllog where tltxcd ='4035' and status =2 and deleted =0)" ' Them de chi lay truong hop thieu ck voi giao dich truc tiep
            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
            'Lay giao dich ban
            v_strSQL = "INSERT INTO tmp_tllog1" _
                    & "(autoid, txnum, txdate,  busdate, brid, micode, comicode, msgamt, sicode," _
                    & "col_value01,col_value02, col_value03, col_value04, col_value05, col_value06," _
                    & "col_value07, col_value08,col_value09) " _
                    & "SELECT a.autoid, a.txnum, a.txdate,  trunc(b.busdate) busdate, a.brid, substr(a.col_value11,1,3), a.col_value15, a.msgamt, a.sicode," _
                    & "'0',CASE WHEN substr(a.col_value11,4,1) IN ('P','A','E') THEN " _
                    & "to_char(a.msgamt) " _
                    & "ELSE '0' " _
                    & "END MUATD, " _
                    & "'0', CASE WHEN substr(a.col_value11,4,1) IN ('C','B') THEN " _
                    & "to_char(a.msgamt) " _
                    & "ELSE '0' " _
                    & "END BANMGTN, " _
                    & "'0', CASE WHEN substr(a.col_value11,4,1) = 'F' THEN " _
                    & "to_char(a.msgamt) " _
                    & "ELSE '0' " _
                    & "END BANMGNN, '0', to_char(a.msgamt), TO_CHAR(((CASE WHEN nvl(d.part_value,0)<0 THEN 0 ELSE nvl(d.part_value,0) end))/1000) " _
                    & "FROM tmp_tllog a, tmp_tllog b, " _
                    & " (SELECT effective_date msgdate, sicode, get_t_plus(effective_date,brid,decode(TYPE,2,-3,-1)) effective_date, " _
                    & "  get_t_plus(effective_date,brid,decode(TYPE,2,-4,-2)) effective_date1 , part_value, TYPE FROM tmp_rgsi1) d " _
                    & " where a.parentid=b.autoid  " _
                    & " and a.t_no = 3" _
                    & " and a.col_value40 = '1'" _
                    & " and a.sicode in (select sicode from tmp_rgsi) " _
                    & " AND ( a.txdate = d.effective_date OR A.TXDATE = d.effective_date1) " _
                    & " AND A.SICODE =D.SICODE and  trunc(b.busdate)> d.msgdate" _
                    & " AND b.status =3"

            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
            'end @2
            ' Lay giao dich mua
            v_strSQL = "INSERT INTO tmp_tllog1" _
                    & "(autoid, txnum, txdate, busdate, brid, micode, comicode, msgamt, sicode,  " _
                    & "col_value01,col_value02, col_value03, col_value04, col_value05, col_value06," _
                    & "col_value07, col_value08,col_value09)" _
                    & "SELECT a.autoid, a.txnum, a.txdate,trunc(b.busdate) busdate, a.brid, substr(a.col_value10,1,3), a.col_value14, a.msgamt, a.sicode," _
                    & "CASE WHEN substr(a.col_value10,4,1) IN ('P','A','E') THEN " _
                    & "to_char(a.msgamt) " _
                    & "ELSE '0' " _
                    & "END MUATD, '0', " _
                    & "CASE WHEN substr(a.col_value10,4,1) IN ('C','B') THEN " _
                    & "to_char(a.msgamt) " _
                    & "ELSE '0' " _
                    & "END BANMGTN, '0', " _
                    & "CASE WHEN substr(a.col_value10,4,1) = 'F' THEN " _
                    & "to_char(a.msgamt) " _
                    & "ELSE '0' " _
                    & "END BANMGNN, '0', to_char(a.msgamt), '0', TO_CHAR(TO_NUMBER(a.col_value08)/1000) " _
                    & "FROM tmp_tllog a, tmp_tllog b, tllog c  where a.parentid=b.autoid  " _
                    & " and a.t_no = 3" _
                    & " and a.col_value40='1'" _
                    & " and a.sicode in (select sicode from tmp_rgsi) " _
                    & " and a.col_value04= c.col_value04 and a.txdate =to_date(c.col_value06,'DD/MM/YYYY') " _
                    & " and c.tltxcd ='4038' and c.status =3 and c.deleted =0"
            '& " and a.col_value04 in (select col_value04 from tllog where tltxcd ='4038' and status =3 and deleted =0)" ' Them de chi lay truong hop thieu ck voi giao dich truc tiep
            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
            'Lay giao dich ban
            v_strSQL = "INSERT INTO tmp_tllog1" _
                    & "(autoid, txnum, txdate,busdate , brid, micode, comicode, msgamt, sicode," _
                    & "col_value01,col_value02, col_value03, col_value04, col_value05, col_value06," _
                    & "col_value07, col_value08,col_value09) " _
                    & "SELECT a.autoid, a.txnum, a.txdate, trunc(b.busdate) busdate, a.brid, substr(a.col_value11,1,3), a.col_value15, a.msgamt, a.sicode," _
                    & "'0',CASE WHEN substr(a.col_value11,4,1) IN ('P','A','E') THEN " _
                    & "to_char(a.msgamt) " _
                    & "ELSE '0' " _
                    & "END MUATD, " _
                    & "'0', CASE WHEN substr(a.col_value11,4,1) IN ('C','B') THEN " _
                    & "to_char(a.msgamt) " _
                    & "ELSE '0' " _
                    & "END BANMGTN, " _
                    & "'0', CASE WHEN substr(a.col_value11,4,1) = 'F' THEN " _
                    & "to_char(a.msgamt) " _
                    & "ELSE '0' " _
                    & "END BANMGNN, '0', to_char(a.msgamt), TO_CHAR(TO_NUMBER(a.col_value08)/1000) " _
                    & "FROM tmp_tllog a, tmp_tllog b, tllog c  where a.parentid=b.autoid " _
                    & " and a.t_no = 3" _
                    & " and a.col_value40='1'" _
                    & " and a.sicode in (select sicode from tmp_rgsi) " _
                    & " and a.col_value04= c.col_value04 and a.txdate =to_date(c.col_value06,'DD/MM/YYYY') " _
                    & " and c.tltxcd ='4038' and c.status =3 and c.deleted =0"
            '& " and a.col_value04 in (select col_value04 from tllog where tltxcd ='4038' and status =3 and deleted =0)" ' Them de chi lay truong hop thieu ck voi giao dich truc tiep
            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
            Dim v_strClause = "3"
            'Lay thong tin ra de export 
            'Nếu là thanh toán đa  phương 
            If CInt(v_strClause) <> 3 Then
                v_strSQL = "SELECT txdate, busdate, miname, micode,code_trade, muatd, bantd, " _
                    & "CASE WHEN muatd>bantd THEN  (muatd-bantd) ELSE 0  END DUOCNHANTD," _
                    & "CASE WHEN muatd<bantd THEN  (bantd-muatd) ELSE 0  END PHAIGIAOTD," _
                    & "muamgtn, banmgtn," _
                    & "CASE WHEN muamgtn>banmgtn THEN (muamgtn-banmgtn) ELSE 0 END DUOCNHANMGTN," _
                    & "CASE WHEN muamgtn<banmgtn THEN (banmgtn-muamgtn) ELSE 0 END PHAIGIAOMGTN," _
                    & "muamgnn, banmgnn, " _
                    & "CASE WHEN muamgnn>banmgnn THEN (muamgnn-banmgnn) ELSE 0 END DUOCNHANMGNN," _
                    & "CASE WHEN muamgnn<banmgnn THEN (banmgnn-muamgnn) ELSE 0 END PHAIGIAOMGNN," _
                    & "slmua, slban, " _
                    & "CASE WHEN slmua>slban THEN (slmua-slban) ELSE 0 END DUOCNHAN," _
                    & "CASE WHEN slmua<slban THEN (slban-slmua) ELSE 0 END PHAIGIAO,LCK " _
                    & " FROM " _
                    & "(SELECT txdate,miname,busdate, code_Trade, micode, sum(muatd) muatd, sum(bantd) bantd, sum(muamgtn) muamgtn, sum(banmgtn) banmgtn, " _
                    & " sum(muamgnn) muamgnn, sum(banmgnn) banmgnn, sum(slmua) slmua, sum(slban) slban, max(lck) lck " _
                    & " FROM " _
                    & "(SELECT   a.txdate,a.busdate, CASE WHEN to_number(b.micode)>=800 THEN 'Tổng Công ty Lưu ký và Bù trừ Chứng khoán Việt nam' ELSE to_char(b.name) END miname, " _
                    & "CASE WHEN to_number(b.micode)>=800 THEN '800' ELSE to_char(a.micode) END code_trade, " _
                    & " CASE WHEN to_number(b.micode)>=800 THEN '800' ELSE to_char(b.micode) END micode, " _
                    & "SUM (round(to_number(a.col_value01)*to_number(a.col_value09),3)) muatd, " _
                    & "SUM (round(to_number(a.col_value02)*to_number(a.col_value09),3)) bantd, SUM (round(to_number(a.col_value03)*to_number(a.col_value09),3)) muamgtn," _
                    & "SUM (round(to_number(a.col_value04)*to_number(a.col_value09),3)) banmgtn, SUM (round(to_number(a.col_value05)*to_number(a.col_value09),3)) muamgnn," _
                    & "SUM (round(to_number(a.col_value06)*to_number(a.col_value09),3)) banmgnn, SUM (round(to_number(a.col_value07)*to_number(a.col_value09),3)) slmua," _
                    & "SUM (round(to_number(a.col_value08)*to_number(a.col_value09),3)) slban, '' LCK " _
                    & "FROM tmp_tllog1 a , rgmi b" _
                    & " where(a.micode = b.code_trade and b.deleted =0 )  " _
                    & "GROUP BY  a.micode, b.micode,  b.name, a.txdate, a.busdate) " _
                    & "GROUP BY busdate, code_trade, micode,miname,txdate,busdate) " _
                    & "order by micode"
                'v_strSQL = "SELECT txdate, busdate, miname, micode,code_trade, muatd, bantd, " _
                '    & "CASE WHEN muatd>bantd THEN  (muatd-bantd) ELSE 0  END DUOCNHANTD," _
                '    & "CASE WHEN muatd<bantd THEN  (bantd-muatd) ELSE 0  END PHAIGIAOTD," _
                '    & "muamgtn, banmgtn," _
                '    & "CASE WHEN muamgtn>banmgtn THEN (muamgtn-banmgtn) ELSE 0 END DUOCNHANMGTN," _
                '    & "CASE WHEN muamgtn<banmgtn THEN (banmgtn-muamgtn) ELSE 0 END PHAIGIAOMGTN," _
                '    & "muamgnn, banmgnn, " _
                '    & "CASE WHEN muamgnn>banmgnn THEN (muamgnn-banmgnn) ELSE 0 END DUOCNHANMGNN," _
                '    & "CASE WHEN muamgnn<banmgnn THEN (banmgnn-muamgnn) ELSE 0 END PHAIGIAOMGNN," _
                '    & "slmua, slban, " _
                '    & "CASE WHEN slmua>slban THEN (slmua-slban) ELSE 0 END DUOCNHAN," _
                '    & "CASE WHEN slmua<slban THEN (slban-slmua) ELSE 0 END PHAIGIAO,LCK " _
                '    & " FROM " _
                '    & "(SELECT   a.txdate,a.busdate, b.name miname, a.micode code_trade,b.micode, SUM (round(to_number(a.col_value01)*to_number(a.col_value09),3)) muatd, " _
                '    & "SUM (round(to_number(a.col_value02)*to_number(a.col_value09),3)) bantd, SUM (round(to_number(a.col_value03)*to_number(a.col_value09),3)) muamgtn," _
                '    & "SUM (round(to_number(a.col_value04)*to_number(a.col_value09),3)) banmgtn, SUM (round(to_number(a.col_value05)*to_number(a.col_value09),3)) muamgnn," _
                '    & "SUM (round(to_number(a.col_value06)*to_number(a.col_value09),3)) banmgnn, SUM (round(to_number(a.col_value07)*to_number(a.col_value09),3)) slmua," _
                '    & "SUM (round(to_number(a.col_value08)*to_number(a.col_value09),3)) slban, '' LCK " _
                '    & "FROM tmp_tllog1 a , rgmi b" _
                '    & " where(a.micode = b.code_trade and b.deleted =0 )  " _
                '    & "GROUP BY  a.micode, b.micode,  b.name, a.txdate, a.busdate) order by micode"
                'nếu là thanh toán trực tiếp
            Else
                v_strSQL = "SELECT txdate, busdate, miname, micode,code_trade, muatd, bantd, " _
                    & " muatd DUOCNHANTD," _
                    & " /*bantd*/ 0  PHAIGIAOTD," _
                    & " muamgtn, banmgtn," _
                    & " muamgtn  DUOCNHANMGTN," _
                    & " /*banmgtn*/ 0 PHAIGIAOMGTN," _
                    & " muamgnn, round(banmgnn,1), " _
                    & " muamgnn DUOCNHANMGNN," _
                    & " /*banmgnn*/ 0 PHAIGIAOMGNN," _
                    & " slmua, slban, " _
                    & " slmua DUOCNHAN," _
                    & " /*slban */ 0 PHAIGIAO,LCK " _
                    & " FROM " _
                    & "(SELECT miname,txdate,busdate, code_Trade, micode, sum(muatd) muatd, sum(bantd) bantd, sum(muamgtn) muamgtn, sum(banmgtn) banmgtn, " _
                    & " sum(muamgnn) muamgnn, sum(banmgnn) banmgnn, sum(slmua) slmua, sum(slban) slban, max(lck) lck " _
                    & " FROM " _
                    & "(SELECT   a.txdate,max(a.busdate) busdate,CASE WHEN to_number(b.micode)>=800 THEN 'Tổng Công ty Lưu ký và Bù trừ Chứng khoán Việt nam' ELSE to_char(b.name) END miname, " _
                    & "CASE WHEN to_number(b.micode)>=800 THEN '800' ELSE to_char(a.micode) END code_trade, " _
                    & " CASE WHEN to_number(b.micode)>=800 THEN '800' ELSE to_char(b.micode) END micode, " _
                    & "SUM (round(to_number(a.col_value01)*to_number(a.col_value09),3)) muatd, " _
                    & "SUM (round(to_number(a.col_value02)*to_number(a.col_value09),3)) bantd, SUM (round(to_number(a.col_value03)*to_number(a.col_value09),3)) muamgtn," _
                    & "SUM (round(to_number(a.col_value04)*to_number(a.col_value09),3)) banmgtn, SUM (round(to_number(a.col_value05)*to_number(a.col_value09),3)) muamgnn," _
                    & "SUM (round(to_number(a.col_value06)*to_number(a.col_value09),3)) banmgnn, SUM (round(to_number(a.col_value07)*to_number(a.col_value09),3)) slmua," _
                    & "SUM (round(to_number(a.col_value08)*to_number(a.col_value09),3)) slban, '' LCK " _
                    & "FROM tmp_tllog1 a , rgmi b" _
                    & " where(a.micode = b.code_trade and b.deleted =0 )  " _
                    & "GROUP BY  a.micode, b.micode,  b.name, a.txdate) " _
                    & "GROUP BY busdate, code_trade, micode,miname,txdate) " _
                    & "where slmua>0 order by micode"
                'v_strSQL = "SELECT txdate, busdate, miname, micode,code_trade, muatd, bantd, " _
                '    & " muatd DUOCNHANTD," _
                '    & " /*bantd*/ 0 PHAIGIAOTD," _
                '    & " muamgtn, banmgtn," _
                '    & " muamgtn  DUOCNHANMGTN," _
                '    & " /*banmgtn*/ 0 PHAIGIAOMGTN," _
                '    & " muamgnn, round(banmgnn,1), " _
                '    & " muamgnn DUOCNHANMGNN," _
                '    & " /*banmgnn*/ 0 PHAIGIAOMGNN," _
                '    & " slmua, slban, " _
                '    & " slmua DUOCNHAN," _
                '    & " /*slban*/ 0  PHAIGIAO,LCK " _
                '    & " FROM " _
                '    & "(SELECT   a.txdate,a.busdate, b.name miname, a.micode code_trade,b.micode, SUM (round(to_number(a.col_value01)*to_number(a.col_value09),3)) muatd, " _
                '    & "SUM (round(to_number(a.col_value02)*to_number(a.col_value09),3)) bantd, SUM (round(to_number(a.col_value03)*to_number(a.col_value09),3)) muamgtn," _
                '    & "SUM (round(to_number(a.col_value04)*to_number(a.col_value09),3)) banmgtn, SUM (round(to_number(a.col_value05)*to_number(a.col_value09),3)) muamgnn," _
                '    & "SUM (round(to_number(a.col_value06)*to_number(a.col_value09),3)) banmgnn, SUM (round(to_number(a.col_value07)*to_number(a.col_value09),3)) slmua," _
                '    & "SUM (round(to_number(a.col_value08)*to_number(a.col_value09),3)) slban, '' LCK " _
                '    & "FROM tmp_tllog1 a , rgmi b" _
                '    & " where(a.micode = b.code_trade and b.deleted =0 )  " _
                '    & "GROUP BY  a.micode, b.micode,  b.name, a.txdate, a.busdate) " _
                '    & " where slmua >0 order by micode"
            End If

            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            BuildXMLObjData(v_ds, v_strObjMsg)
            pv_xmlDocument.LoadXml(v_strObjMsg)

            v_obj.Commit()
            'ContextUtil.SetComplete()
            Return v_lngErr
        Catch ex As Exception
            Throw ex
            'ContextUtil.SetAbort()
            v_obj.Rollback()
        Finally
            v_obj.Dispose()
            GC.Collect()
        End Try
    End Function
    'end bangpv 20150211
    Private Function HOSTCreateReportOver(ByRef pv_xmlDocument As XmlDocumentEx) As Long
        Dim v_lngErr As Long = ERR_SYSTEM_OK
        Dim v_obj As New DataAccess
        Dim tr2 As TextWriterTraceListener
        Dim v_trace_status, v_trace_path As String
        Dim v_strRptID As String
        Try
            Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes
            Dim v_strSQL, v_strSQLTmp As String
            Dim v_strObjMsg As String
            Dim v_strClause As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCLAUSE), Xml.XmlAttribute).Value)
            Dim v_strTellerID As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTLID), Xml.XmlAttribute).Value)
            Dim v_arrFilter As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCMDINQUIRY), Xml.XmlAttribute).Value)
            Dim v_strBRID As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeBRID), Xml.XmlAttribute).Value)
            Dim v_strCurdate As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTXDATE), Xml.XmlAttribute).Value)
            Dim v_strTLName As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTLNAME), Xml.XmlAttribute).Value)
            Dim v_strIsSignCA As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeSIGNCA), Xml.XmlAttribute).Value)
            Dim v_strPartition As String
            If Not (v_attrColl.GetNamedItem(gc_AtributeTXNUM) Is Nothing) Then
                v_strPartition = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTXNUM), Xml.XmlAttribute).Value)
            Else
                v_strPartition = ""
            End If
            v_trace_status = "0"

            v_obj.NewDBInstance(gc_MODULE_INQUERY)
            v_obj.BeginTran()

            v_strObjMsg = pv_xmlDocument.InnerXml()

            v_obj.GetSysVar("SYSTEM", "TRACE_STATUS", v_trace_status)
            Dim v_strDirCurrDate As String
            If v_trace_status = "1" Then
                v_obj.GetSysVar("SYSTEM", "TRACE_PATH", v_trace_path)
                'Dim v_strTLName As String = v_obj.GetValue("TLPROFILES", "TLNAME", "TLID='" & v_strTellerID & "'")

                v_strDirCurrDate = Replace(v_strCurdate, "/", "_")
                If v_trace_path = "" Then
                    Dim v_app As New ApplicationServices.ApplicationBase
                    v_trace_path = v_app.Info.DirectoryPath & "\Log\" & v_strDirCurrDate
                Else
                    v_trace_path = v_trace_path & v_strDirCurrDate
                End If

                If Not System.IO.Directory.Exists(v_trace_path) Then
                    System.IO.Directory.CreateDirectory(v_trace_path)
                End If

                v_trace_path &= "\log_report_br" & v_strBRID & "_" & v_strTLName & ".txt"

                tr2 = New TextWriterTraceListener(System.IO.File.AppendText(v_trace_path))
                Trace.Listeners.Add(tr2)
                Trace.WriteLine("[Bắt đầu: RP - " & v_strRptID & "] " & DateTime.Now & vbCrLf)
            End If

            Dim v_ds, v_ds1 As DataSet
            Dim v_trace As DataSet
            Dim v_lstBrID As String
            Dim v_blnSearch As Boolean = False
            Dim v_blnLoadAll As Boolean = False
            Dim v_strMinDate, v_strMaxDate As String

            Dim v_arrTemp() As String

            Dim v_arrField() As String
            Dim v_strFieldCode As String = ""
            v_arrTemp = v_strClause.Split("#")
            v_strRptID = v_arrTemp(0)
            v_arrField = v_arrTemp(1).Split("$")

            'Xu ly DL bang TRAN, TLLOG
            Dim v_strMainSQL As String
            v_strSQL = "SELECT (RPTCMDSQL || nvl(RPTCMDSQL1,'')) RPTCMDSQL, TLLOGTRAN FROM RPREPORTS WHERE RPTID = '" & v_strRptID & "' AND DELETED=0 AND STATUS=0"
            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

            Dim v_strTran As String = "" 'IA|MA|CA|MF|RA|RG|SF|TLLOG
            Dim v_lstTran() As String

            If v_ds.Tables(0).Rows.Count > 0 Then
                v_strTran = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("TLLOGTRAN")).ToUpper
                v_strMainSQL = v_ds.Tables(0).Rows(0)("RPTCMDSQL")
            End If

            v_strMinDate = v_strCurdate
            v_strMaxDate = v_strCurdate
            If v_strPartition <> "" Then
                v_strMinDate = v_strPartition.Split("|")(0)
                v_strMaxDate = v_strPartition.Split("|")(1)
            End If
            v_blnSearch = CheckPassDate(v_strMaxDate, v_strCurdate)
            v_blnLoadAll = CheckPassDate(v_strMinDate, v_strCurdate)


            If v_strTran <> "" Then
                v_lstTran = v_strTran.Split("|")
                v_strTran = ""
                For i As Integer = 0 To v_lstTran.Length - 1
                    Select Case v_lstTran(i)
                        Case "TLLOG"
                            v_strTran &= ",'TLLOG'"
                            If v_blnLoadAll Then
                                v_strTran &= ",'TLLOGALL'"
                            End If

                        Case "CA", "IA", "MA", "RA", "SF"
                            v_strTran &= ",'" & v_lstTran(i) & "TRAN'"
                            v_strTran &= ",'" & v_lstTran(i) & "MAST'"
                            If v_blnLoadAll Then
                                v_strTran &= ",'" & v_lstTran(i) & "TRANA'"
                            End If
                        Case "RG", "MF"
                            v_strTran &= ",'" & v_lstTran(i) & "TRAN'"
                            If v_blnLoadAll Then
                                v_strTran &= ",'" & v_lstTran(i) & "TRANA'"
                            End If
                    End Select
                Next
                v_strTran = "(" & Mid(v_strTran, 2) & ")"
            End If

            Dim v_lstSICODE, v_lstMICODE As String

            v_lstMICODE = v_arrFilter.Split("|")(1)
            v_lstSICODE = v_arrFilter.Split("|")(0)

            'Lay phan quyen chi nhanh
            v_strSQL = "SELECT DISTINCT brid brid  FROM " _
                       & " (SELECT b.brid FROM tlbridauth a, brgrp b" _
                       & " WHERE ((AUTHID = '" & v_strTellerID & "' AND authtype = 'U') OR (AUTHID IN (SELECT a.grpid" _
                       & " FROM tlgrpusers a WHERE a.tlid = '" & v_strTellerID & "') AND authtype = 'G'))" _
                       & " And a.brid = b.brid And a.deleted = 0 And a.status = 0" _
                       & " And b.deleted = 0 And b.status = 0" _
                       & " UNION " _
                       & " SELECT b.brid FROM tlprofiles a, brgrp b" _
                       & " WHERE a.brid = b.brid AND a.tlid = '" & v_strTellerID & "'" _
                       & " AND a.deleted = 0 AND a.status = 0 AND b.deleted = 0 AND b.status = 0)"

            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

            v_lstBrID = ""
            For i = 0 To v_ds.Tables(0).Rows.Count - 1
                v_lstBrID &= ",'" & v_ds.Tables(0).Rows(i)(0) & "'"
            Next
            v_lstBrID = "(" & Mid(v_lstBrID, 2) & ")"

            'Lay TVLK dc phan quyen cho user vao bang TT_TMP_RGMI
            'v_strSQL = "INSERT INTO tmp_rgmi " _
            '            & " SELECT DISTINCT m.* FROM rgmi m, tlmemauth a" _
            '            & " WHERE ((a.AUTHID = '" & v_strTellerID & "' AND a.authtype = 'U')" _
            '            & " OR (a.AUTHID IN (SELECT grpid FROM tlgrpusers WHERE tlid = '" & v_strTellerID & "')" _
            '            & " AND a.authtype = 'G')) AND m.deleted = 0 AND m.status = 0" _
            '            & " AND m.micode = a.micode AND m.deleted=0 AND m.status=0"
            v_strSQL = "INSERT INTO tmp_rgmi " _
                       & " SELECT DISTINCT m.* FROM rgmi m WHERE m.deleted=0 AND m.status=0" _
                       & " AND m.MICODE IN " & v_lstMICODE
            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
            'bangpv: Bỏ lấy chưa lưu ký
            'v_strSQL = "INSERT INTO TMP_RGMI(AUTOID, MICODE) SELECT '0' AUTOID, '000' MICODE FROM DUAL"
            'v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
            'end bangpv

            ''Lay chung khoan dc phan quyen cho user vao bang TT_TMP_RGSI
            'v_strSQL = "INSERT INTO tmp_rgsi(iscode, sicode, stock_name, part_value, TYPE," _
            '            & "stock_type, interest_rate, interest_period, " _
            '            & "bond_period, deleted, exchange_rate, note, " _
            '            & "bond_term, release_series, release_mode, isin, " _
            '            & "rarate, tmpid, paidmethod, npaiddate1, npaiddate2, " _
            '            & "npaiddate3, npaiddate4, int_release_mode, brid, status, issuer_date, due_date) " _
            '            & " SELECT distinct m.iscode, m.sicode, m.stock_name, m.part_value, m.TYPE," _
            '            & "m.stock_type, m.interest_rate, m.interest_period," _
            '            & "m.bond_period, m.deleted, m.exchange_rate, m.note, " _
            '            & "m.bond_term, m.release_series, m.release_mode, m.isin, " _
            '            & "m.rarate, m.tmpid, m.paidmethod, m.npaiddate1, m.npaiddate2," _
            '            & "m.npaiddate3, m.npaiddate4, m.int_release_mode, m.brid, m.status, m.issuer_date, m.due_date FROM rgsi m" _
            '            & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_lstSICODE
            'v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
            'Lay chung khoan dc phan quyen cho user vao bang TT_TMP_RGSI
            ' ngay : 05092009 
            ' muc dich : tach chuoi v_lstSICODE khac phuc loi 
            '               ORA-01795: maximum number of expressions in a list is 1000
            'Dim v_strCut, v_strCut1, v_strCut2, v_strCut3, v_strCut4, v_strCut5 As String
            'chia lam 5 khoi

            'Dim v_iCut As Integer
            'v_iCut = (v_lstSICODE.Length \ 5)
            'v_iCut = InStr(v_iCut, v_lstSICODE, ",")
            'If v_iCut > 0 Then
            '    v_strCut1 = v_lstSICODE.Substring(0, v_iCut - 1) & ")"
            '    v_strSQL = "INSERT INTO tmp_rgsi( iscode, sicode, stock_name, part_value, type," _
            '                            & " status, stock_type, interest_rate, interest_period, " _
            '                            & " bond_period, due_date, issuer_date, effective_date, " _
            '                            & " deleted, exchange_date, exchange_rate, note, bond_term, " _
            '                            & "release_series, release_mode, isin, brid, rarate, " _
            '                            & "tmpid, paidmethod, npaiddate1, npaiddate2, npaiddate3, " _
            '                            & "npaiddate4, int_release_mode, currency_code, public_date, " _
            '                            & "stock_id, listed_qtty, bond_period_unit, sbv_sicode, vsdbrid) " _
            '                            & " SELECT distinct m.iscode, m.sicode, m.stock_name, m.part_value, m.type," _
            '                            & "m.status, m.stock_type, m.interest_rate, m.interest_period, " _
            '                            & "m.bond_period, m.due_date, m.issuer_date, m.effective_date, " _
            '                            & "m.deleted, m.exchange_date, m.exchange_rate, m.note, m.bond_term, " _
            '                            & "m.release_series, m.release_mode, m.isin, m.brid, m.rarate, " _
            '                            & "m.tmpid, m.paidmethod, m.npaiddate1, m.npaiddate2, m.npaiddate3, " _
            '                            & "m.npaiddate4, m.int_release_mode, m.currency_code, m.public_date, " _
            '                            & "m.stock_id, m.listed_qtty, m.bond_period_unit, m.sbv_sicode,m.vsdbrid FROM rgsi m" _
            '    & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_strCut1
            '    v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

            '    v_strCut = "(" & v_lstSICODE.Substring(v_iCut)
            '    v_iCut = InStr(v_iCut, v_strCut, ",")
            '    If v_iCut > 0 Then
            '        v_strCut2 = v_strCut.Substring(0, v_iCut - 1) & ")"
            '        v_strSQL = "INSERT INTO tmp_rgsi( iscode, sicode, stock_name, part_value, type," _
            '                            & " status, stock_type, interest_rate, interest_period, " _
            '                            & " bond_period, due_date, issuer_date, effective_date, " _
            '                            & " deleted, exchange_date, exchange_rate, note, bond_term, " _
            '                            & "release_series, release_mode, isin, brid, rarate, " _
            '                            & "tmpid, paidmethod, npaiddate1, npaiddate2, npaiddate3, " _
            '                            & "npaiddate4, int_release_mode, currency_code, public_date, " _
            '                            & "stock_id, listed_qtty, bond_period_unit, sbv_sicode, vsdbrid) " _
            '                            & " SELECT distinct m.iscode, m.sicode, m.stock_name, m.part_value, m.type," _
            '                            & "m.status, m.stock_type, m.interest_rate, m.interest_period, " _
            '                            & "m.bond_period, m.due_date, m.issuer_date, m.effective_date, " _
            '                            & "m.deleted, m.exchange_date, m.exchange_rate, m.note, m.bond_term, " _
            '                            & "m.release_series, m.release_mode, m.isin, m.brid, m.rarate, " _
            '                            & "m.tmpid, m.paidmethod, m.npaiddate1, m.npaiddate2, m.npaiddate3, " _
            '                            & "m.npaiddate4, m.int_release_mode, m.currency_code, m.public_date, " _
            '                            & "m.stock_id, m.listed_qtty, m.bond_period_unit, m.sbv_sicode,m.vsdbrid FROM rgsi m" _
            '              & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_strCut2
            '        v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

            '        v_strCut = "(" & v_strCut.Substring(v_iCut)
            '        v_iCut = InStr(v_iCut, v_strCut, ",")
            '        '3
            '        If v_iCut > 0 Then
            '            v_strCut3 = v_strCut.Substring(0, v_iCut - 1) & ")"
            '            v_strSQL = "INSERT INTO tmp_rgsi( iscode, sicode, stock_name, part_value, type," _
            '                            & " status, stock_type, interest_rate, interest_period, " _
            '                            & " bond_period, due_date, issuer_date, effective_date, " _
            '                            & " deleted, exchange_date, exchange_rate, note, bond_term, " _
            '                            & "release_series, release_mode, isin, brid, rarate, " _
            '                            & "tmpid, paidmethod, npaiddate1, npaiddate2, npaiddate3, " _
            '                            & "npaiddate4, int_release_mode, currency_code, public_date, " _
            '                            & "stock_id, listed_qtty, bond_period_unit, sbv_sicode, vsdbrid) " _
            '                            & " SELECT distinct m.iscode, m.sicode, m.stock_name, m.part_value, m.type," _
            '                            & "m.status, m.stock_type, m.interest_rate, m.interest_period, " _
            '                            & "m.bond_period, m.due_date, m.issuer_date, m.effective_date, " _
            '                            & "m.deleted, m.exchange_date, m.exchange_rate, m.note, m.bond_term, " _
            '                            & "m.release_series, m.release_mode, m.isin, m.brid, m.rarate, " _
            '                            & "m.tmpid, m.paidmethod, m.npaiddate1, m.npaiddate2, m.npaiddate3, " _
            '                            & "m.npaiddate4, m.int_release_mode, m.currency_code, m.public_date, " _
            '                            & "m.stock_id, m.listed_qtty, m.bond_period_unit, m.sbv_sicode,m.vsdbrid FROM rgsi m" _
            '                            & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_strCut3
            '            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

            '            v_strCut = "(" & v_strCut.Substring(v_iCut)
            '            v_iCut = InStr(v_iCut, v_strCut, ",")
            '            If v_iCut > 0 Then
            '                v_strCut4 = v_strCut.Substring(0, v_iCut - 1) & ")"
            '                v_strSQL = "INSERT INTO tmp_rgsi( iscode, sicode, stock_name, part_value, type," _
            '                            & " status, stock_type, interest_rate, interest_period, " _
            '                            & " bond_period, due_date, issuer_date, effective_date, " _
            '                            & " deleted, exchange_date, exchange_rate, note, bond_term, " _
            '                            & "release_series, release_mode, isin, brid, rarate, " _
            '                            & "tmpid, paidmethod, npaiddate1, npaiddate2, npaiddate3, " _
            '                            & "npaiddate4, int_release_mode, currency_code, public_date, " _
            '                            & "stock_id, listed_qtty, bond_period_unit, sbv_sicode, vsdbrid) " _
            '                            & " SELECT distinct m.iscode, m.sicode, m.stock_name, m.part_value, m.type," _
            '                            & "m.status, m.stock_type, m.interest_rate, m.interest_period, " _
            '                            & "m.bond_period, m.due_date, m.issuer_date, m.effective_date, " _
            '                            & "m.deleted, m.exchange_date, m.exchange_rate, m.note, m.bond_term, " _
            '                            & "m.release_series, m.release_mode, m.isin, m.brid, m.rarate, " _
            '                            & "m.tmpid, m.paidmethod, m.npaiddate1, m.npaiddate2, m.npaiddate3, " _
            '                            & "m.npaiddate4, m.int_release_mode, m.currency_code, m.public_date, " _
            '                            & "m.stock_id, m.listed_qtty, m.bond_period_unit, m.sbv_sicode,m.vsdbrid FROM rgsi m" _
            '                        & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_strCut4
            '                v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

            '                v_strCut5 = "(" & v_strCut.Substring(v_iCut)

            '                v_strSQL = "INSERT INTO tmp_rgsi( iscode, sicode, stock_name, part_value, type," _
            '                            & " status, stock_type, interest_rate, interest_period, " _
            '                            & " bond_period, due_date, issuer_date, effective_date, " _
            '                            & " deleted, exchange_date, exchange_rate, note, bond_term, " _
            '                            & "release_series, release_mode, isin, brid, rarate, " _
            '                            & "tmpid, paidmethod, npaiddate1, npaiddate2, npaiddate3, " _
            '                            & "npaiddate4, int_release_mode, currency_code, public_date, " _
            '                            & "stock_id, listed_qtty, bond_period_unit, sbv_sicode, vsdbrid) " _
            '                            & " SELECT distinct m.iscode, m.sicode, m.stock_name, m.part_value, m.type," _
            '                            & "m.status, m.stock_type, m.interest_rate, m.interest_period, " _
            '                            & "m.bond_period, m.due_date, m.issuer_date, m.effective_date, " _
            '                            & "m.deleted, m.exchange_date, m.exchange_rate, m.note, m.bond_term, " _
            '                            & "m.release_series, m.release_mode, m.isin, m.brid, m.rarate, " _
            '                            & "m.tmpid, m.paidmethod, m.npaiddate1, m.npaiddate2, m.npaiddate3, " _
            '                            & "m.npaiddate4, m.int_release_mode, m.currency_code, m.public_date, " _
            '                            & "m.stock_id, m.listed_qtty, m.bond_period_unit, m.sbv_sicode,m.vsdbrid FROM rgsi m" _
            '                        & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_strCut5
            '                v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

            '            Else
            '                v_strSQL = "INSERT INTO tmp_rgsi( iscode, sicode, stock_name, part_value, type," _
            '                            & " status, stock_type, interest_rate, interest_period, " _
            '                            & " bond_period, due_date, issuer_date, effective_date, " _
            '                            & " deleted, exchange_date, exchange_rate, note, bond_term, " _
            '                            & "release_series, release_mode, isin, brid, rarate, " _
            '                            & "tmpid, paidmethod, npaiddate1, npaiddate2, npaiddate3, " _
            '                            & "npaiddate4, int_release_mode, currency_code, public_date, " _
            '                            & "stock_id, listed_qtty, bond_period_unit, sbv_sicode, vsdbrid) " _
            '                            & " SELECT distinct m.iscode, m.sicode, m.stock_name, m.part_value, m.type," _
            '                            & "m.status, m.stock_type, m.interest_rate, m.interest_period, " _
            '                            & "m.bond_period, m.due_date, m.issuer_date, m.effective_date, " _
            '                            & "m.deleted, m.exchange_date, m.exchange_rate, m.note, m.bond_term, " _
            '                            & "m.release_series, m.release_mode, m.isin, m.brid, m.rarate, " _
            '                            & "m.tmpid, m.paidmethod, m.npaiddate1, m.npaiddate2, m.npaiddate3, " _
            '                            & "m.npaiddate4, m.int_release_mode, m.currency_code, m.public_date, " _
            '                            & "m.stock_id, m.listed_qtty, m.bond_period_unit, m.sbv_sicode,m.vsdbrid FROM rgsi m" _
            '                          & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_strCut
            '                v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
            '            End If
            '        End If
            '    End If
            'Else
            '    v_strSQL = "INSERT INTO tmp_rgsi( iscode, sicode, stock_name, part_value, type," _
            '                            & " status, stock_type, interest_rate, interest_period, " _
            '                            & " bond_period, due_date, issuer_date, effective_date, " _
            '                            & " deleted, exchange_date, exchange_rate, note, bond_term, " _
            '                            & "release_series, release_mode, isin, brid, rarate, " _
            '                            & "tmpid, paidmethod, npaiddate1, npaiddate2, npaiddate3, " _
            '                            & "npaiddate4, int_release_mode, currency_code, public_date, " _
            '                            & "stock_id, listed_qtty, bond_period_unit, sbv_sicode, vsdbrid) " _
            '                            & " SELECT distinct m.iscode, m.sicode, m.stock_name, m.part_value, m.type," _
            '                            & "m.status, m.stock_type, m.interest_rate, m.interest_period, " _
            '                            & "m.bond_period, m.due_date, m.issuer_date, m.effective_date, " _
            '                            & "m.deleted, m.exchange_date, m.exchange_rate, m.note, m.bond_term, " _
            '                            & "m.release_series, m.release_mode, m.isin, m.brid, m.rarate, " _
            '                            & "m.tmpid, m.paidmethod, m.npaiddate1, m.npaiddate2, m.npaiddate3, " _
            '                            & "m.npaiddate4, m.int_release_mode, m.currency_code, m.public_date, " _
            '                            & "m.stock_id, m.listed_qtty, m.bond_period_unit, m.sbv_sicode,m.vsdbrid FROM rgsi m" _
            '                  & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_lstSICODE
            '    v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

            'End If
            'HaNM5-28/10/2020-Sua loi tren 1000 ma
            Dim v_arrSICODE() As String = v_lstSICODE.Replace("(", "").Replace(")", "").Split(",")
            Dim v_intStart As Integer = 0
            Dim v_intEnd As Integer = 0
            Dim v_intOffSet As Integer = 990
            Dim v_strSiCodeList As String = ""
            Dim v_intSiCodeCount As Integer = v_arrSICODE.Length
            While v_intStart < v_intSiCodeCount
                v_strSiCodeList = ""
                If v_intStart + v_intOffSet >= v_intSiCodeCount Then
                    v_intEnd = v_intSiCodeCount - 1
                Else
                    v_intEnd = v_intStart + v_intOffSet - 1
                End If
                For i As Integer = v_intStart To v_intEnd
                    v_strSiCodeList = v_strSiCodeList & "," & v_arrSICODE(i)
                Next
                If v_strSiCodeList.Length > 0 Then
                    v_strSiCodeList = "(" & v_strSiCodeList.Substring(1) & ")"
                    v_strSQL = "INSERT INTO tmp_rgsi(iscode, sicode, stock_name, part_value, TYPE," _
                                      & "stock_type, interest_rate, interest_period, " _
                                      & "bond_period, deleted, exchange_rate, note, " _
                                      & "bond_term, release_series, release_mode, isin, " _
                                      & "rarate, tmpid, paidmethod, npaiddate1, npaiddate2, " _
                                      & "npaiddate3, npaiddate4, int_release_mode, brid, status, public_date, currency_code,vsdbrid) " _
                                      & " SELECT distinct m.iscode, m.sicode, m.stock_name, m.part_value, m.TYPE," _
                                      & "m.stock_type, m.interest_rate, m.interest_period," _
                                      & "m.bond_period, m.deleted, m.exchange_rate, m.note, " _
                                      & "m.bond_term, m.release_series, m.release_mode, m.isin, " _
                                      & "m.rarate, m.tmpid, m.paidmethod, m.npaiddate1, m.npaiddate2," _
                                      & "m.npaiddate3, m.npaiddate4, m.int_release_mode, m.brid, m.status, m.public_date, m.currency_code,m.vsdbrid FROM rgsi m" _
                                      & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_strSiCodeList
                    v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                End If
                v_intStart = v_intStart + v_intOffSet
            End While
            'End HaNM5 sua
            ' tuanta

            v_strSQL = "INSERT INTO TMP_RGSI(AUTOID, SICODE) SELECT 0 AUTOID, '000' SICODE FROM DUAL"
            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

            'Lay cau xu ly chung
            Dim v_hSQL As New Hashtable
            Dim v_hORD As New Hashtable
            If v_strTran <> "" Then
                v_strSQL = "SELECT * FROM RPSTORES WHERE RPTID IS NULL AND OVERWRITE IN " & v_strTran & " ORDER BY ODRNUM"

                v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                Dim v_strOverWrite As String
                If v_ds.Tables(0).Rows.Count > 0 Then
                    For i As Integer = 0 To v_ds.Tables(0).Rows.Count - 1
                        v_strOverWrite = v_ds.Tables(0).Rows(i)("OVERWRITE")
                        v_hORD(i) = v_strOverWrite
                        v_hSQL(v_strOverWrite) = v_ds.Tables(0).Rows(i)("TRACE") & v_ds.Tables(0).Rows(i)("CMDSQL") & " " & v_ds.Tables(0).Rows(i)("CMDSQL1")
                    Next
                End If

                'Lay cau xu ly chung da dc viet lai
                v_strSQL = "SELECT * FROM RPSTORES WHERE RPTID='" & v_strRptID & "' AND OVERWRITE IN " & v_strTran & " ORDER BY ODRNUM"
                v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

                If v_ds.Tables(0).Rows.Count > 0 Then
                    For i As Integer = 0 To v_ds.Tables(0).Rows.Count - 1
                        v_strOverWrite = v_ds.Tables(0).Rows(i)("OVERWRITE")
                        If Not v_hSQL(v_strOverWrite) Is Nothing Then
                            v_hSQL(v_strOverWrite) = v_ds.Tables(0).Rows(i)("TRACE") & v_ds.Tables(0).Rows(i)("CMDSQL") & " " & v_ds.Tables(0).Rows(i)("CMDSQL1")
                        End If
                    Next
                End If

                'Thuc hien cau xu ly chung
                'Dim v_strPartitionSQL As String
                'Dim v_lstPartition As String
                For i As Integer = 0 To v_hORD.Count - 1
                    v_strOverWrite = v_hORD(i)
                    v_strSQL = Mid(v_hSQL(v_strOverWrite), 2)
                    'v_strPartitionSQL = ""

                    v_strSQL = Replace(v_strSQL, "[!BRID]", v_strBRID)
                    v_strSQL = Replace(v_strSQL, "[!TLID]", v_strTellerID)
                    v_strSQL = Replace(v_strSQL, "[!LBRID]", v_lstBrID)
                    v_strSQL = Replace(v_strSQL, "[!MINDATE]", "TO_DATE('" & v_strMinDate & "','dd/mm/yyyy')")
                    v_strSQL = Replace(v_strSQL, "[!MAXDATE]", "TO_DATE('" & v_strMaxDate & "','dd/mm/yyyy')")
                    'BangPV: Bỏ tt_ do ko dùng timesten nữa
                    v_strSQL = Replace(v_strSQL, "TT_", "")
                    v_strSQL = Replace(v_strSQL, "tt_", "")
                    v_strSQL = Replace(v_strSQL, "tT_", "")
                    v_strSQL = Replace(v_strSQL, "Tt_", "")
                    'end bangpv
                    v_strSQL = Replace(v_strSQL, "[!TLLOGALL]", "TLLOGALL_ALL")
                    v_strSQL = Replace(v_strSQL, "[!CATRANA]", "CATRANA_ALL")
                    v_strSQL = Replace(v_strSQL, "[!IATRANA]", "IATRANA_ALL")
                    v_strSQL = Replace(v_strSQL, "[!MATRANA]", "MATRANA_ALL")
                    v_strSQL = Replace(v_strSQL, "[!SFTRANA]", "SFTRANA_ALL")
                    v_strSQL = Replace(v_strSQL, "[!MFTRANA]", "MFTRANA_ALL")
                    v_strSQL = Replace(v_strSQL, "[!RATRANA]", "RATRANA_ALL")
                    v_strSQL = Replace(v_strSQL, "[!RGTRANA]", "RGTRANA_ALL")

                    'If Right(v_strOverWrite, 3) = "ALL" Or Right(v_strOverWrite, 1) = "A" Then
                    'If Mid(v_strOverWrite, 1, 5).ToUpper = "TLLOG" Then
                    '    v_lstPartition = GetListPartition(v_strPartition)
                    'Else
                    '    v_lstPartition = GetListPartition(v_strMinDate & "|" & v_strCurdate)
                    'End If

                    'For j As Integer = 0 To v_lstPartition.Split("|").Length - 2
                    '    If v_strPartitionSQL <> "" Then
                    '        v_strPartitionSQL &= " UNION ALL "
                    '    End If

                    '    v_strPartitionSQL &= v_strSQL.Substring(InStr(v_strSQL.ToUpper, "SELECT") - 1)
                    '    v_strPartitionSQL = Replace(v_strPartitionSQL, "[!TLLOGALL]", "TLLOGALL_ALL PARTITION(" & v_lstPartition.Split("|")(j) & ")")
                    '    v_strPartitionSQL = Replace(v_strPartitionSQL, "[!CATRANA]", "CATRANA_ALL PARTITION(" & v_lstPartition.Split("|")(j) & ")")
                    '    v_strPartitionSQL = Replace(v_strPartitionSQL, "[!IATRANA]", "IATRANA_ALL PARTITION(" & v_lstPartition.Split("|")(j) & ")")
                    '    v_strPartitionSQL = Replace(v_strPartitionSQL, "[!MATRANA]", "MATRANA_ALL PARTITION(" & v_lstPartition.Split("|")(j) & ")")
                    '    v_strPartitionSQL = Replace(v_strPartitionSQL, "[!SFTRANA]", "SFTRANA_ALL PARTITION(" & v_lstPartition.Split("|")(j) & ")")
                    '    v_strPartitionSQL = Replace(v_strPartitionSQL, "[!MFTRANA]", "MFTRANA_ALL PARTITION(" & v_lstPartition.Split("|")(j) & ")")
                    '    v_strPartitionSQL = Replace(v_strPartitionSQL, "[!RATRANA]", "RATRANA_ALL PARTITION(" & v_lstPartition.Split("|")(j) & ")")
                    '    v_strPartitionSQL = Replace(v_strPartitionSQL, "[!RGTRANA]", "RGTRANA_ALL PARTITION(" & v_lstPartition.Split("|")(j) & ")")
                    'Next

                    '    v_strSQL = Mid(v_strSQL, 1, InStr(v_strSQL.ToUpper, "SELECT") - 1)
                    '    v_strSQL &= v_strPartitionSQL
                    'End If

                    For l As Integer = 0 To v_arrField.Count - 2
                        v_strSQL = Replace(v_strSQL, "[!" & v_arrField(l).Split("|")(0) & "]", v_arrField(l).Split("|")(1))
                    Next

                    If v_trace_status = "1" And Mid(v_hSQL(v_strOverWrite), 1, 1) = "1" Then
                        Trace.WriteLine("[RP] " & DateTime.Now & " :" & vbCrLf & " -o- Câu lệnh thứ xử lý chung thứ #" & i + 1 & "-o-" & vbCrLf & v_strSQL & vbCrLf)
                        If InStr(v_strSQL.ToUpper, "SELECT") > 0 Then
                            Trace.WriteLine("-o- Dữ liệu câu lệnh xử lý chung thứ #" & i + 1 & "-o-")
                            v_strSQLTmp = v_strSQL.Substring(InStr(v_strSQL.ToUpper, "SELECT") - 1)
                            v_trace = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                            Trace.WriteLine(v_trace.GetXml & vbCrLf)
                        End If
                    End If
                    'v_obj.ExecuteNonQuery(CommandType.Text, v_strPartitionSQL)
                    v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                Next
            End If

            v_strSQL = "SELECT * FROM RPSTORES WHERE RPTID='" & v_strRptID & "' AND OVERWRITE IS NULL AND DELETED=0 AND STATUS=0 ORDER BY ODRNUM"
            v_ds1 = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

            Dim v_int As Integer

            For v_int = 0 To v_ds1.Tables(0).Rows.Count - 1

                v_strSQL = v_ds1.Tables(0).Rows(v_int)("CMDSQL").ToString.ToUpper & " " & v_ds1.Tables(0).Rows(v_int)("CMDSQL1").ToString.ToUpper

                'If Not CheckTranTable(v_strSQL) Then
                '    v_lngErr = 1
                '    Exit For
                'End If

                For i As Integer = 0 To v_arrField.Count - 2
                    v_strSQL = Replace(v_strSQL, "[!" & v_arrField(i).Split("|")(0) & "]", v_arrField(i).Split("|")(1))
                Next

                v_strSQL = Replace(v_strSQL, "[!BRID]", v_strBRID)
                v_strSQL = Replace(v_strSQL, "[!TLID]", v_strTellerID)
                v_strSQL = Replace(v_strSQL, "[!LBRID]", v_lstBrID)
                v_strSQL = Replace(v_strSQL, "[!MINDATE]", "TO_DATE('" & v_strMinDate & "','dd/mm/yyyy')")
                v_strSQL = Replace(v_strSQL, "[!MAXDATE]", "TO_DATE('" & v_strMaxDate & "','dd/mm/yyyy')")
                'BangPV: Bỏ tt_ do ko dùng timesten nữa
                v_strSQL = Replace(v_strSQL, "TT_", "")
                v_strSQL = Replace(v_strSQL, "tt_", "")
                v_strSQL = Replace(v_strSQL, "tT_", "")
                v_strSQL = Replace(v_strSQL, "Tt_", "")
                'end BangPV

                If v_trace_status = "1" And v_ds1.Tables(0).Rows(v_int)("TRACE") = 1 Then
                    Trace.WriteLine("[RP] " & DateTime.Now & " :" & vbCrLf & " -o- Câu lệnh thứ #" & v_ds1.Tables(0).Rows(v_int)("ODRNUM") & "-o-" & vbCrLf & v_strSQL & vbCrLf)
                    If InStr(v_strSQL.ToUpper, "SELECT") > 0 Then
                        Trace.WriteLine("-o- Dữ liệu câu lệnh thứ #" & v_ds1.Tables(0).Rows(v_int)("ODRNUM") & "-o-")
                        v_strSQLTmp = v_strSQL.Substring(InStr(v_strSQL.ToUpper, "SELECT") - 1)
                        v_trace = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                        Trace.WriteLine(v_trace.GetXml & vbCrLf)
                    End If
                End If

                v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

            Next

            If v_lngErr = ERR_SYSTEM_OK Then

                v_strSQL = v_strMainSQL

                'If Not CheckTranTable(v_strSQL) Then
                '    v_lngErr = 1
                'Else
                For i As Integer = 0 To v_arrField.Count - 2
                    v_strSQL = Replace(v_strSQL, "[!" & v_arrField(i).Split("|")(0) & "]", v_arrField(i).Split("|")(1))
                Next
                ' purpose : phi luu ky
                If InStr(v_strSQL, "[!MFNO_01]", CompareMethod.Text) Then
                    v_strSQLTmp = "select fomula from mftype where deleted=0 and status=0 and mfno='01'"
                    v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                    If v_ds.Tables(0).Rows.Count > 0 Then
                        v_strSQL = Replace(v_strSQL, "[!MFNO_01]", v_ds.Tables(0).Rows(0)(0))
                    Else
                        v_strSQL = Replace(v_strSQL, "[!MFNO_01]", "0")
                    End If
                End If
                ' end phi luu ky
                If v_strSQL <> "" Then
                    v_strSQL = Replace(v_strSQL, "[!BRID]", v_strBRID)
                    v_strSQL = Replace(v_strSQL, "[!TLID]", v_strTellerID)
                    v_strSQL = Replace(v_strSQL, "[!LBRID]", v_lstBrID)
                    v_strSQL = Replace(v_strSQL, "[!MINDATE]", "TO_DATE('" & v_strMinDate & "','dd/mm/yyyy')")
                    v_strSQL = Replace(v_strSQL, "[!MAXDATE]", "TO_DATE('" & v_strMaxDate & "','dd/mm/yyyy')")

                    'BangPV: Bỏ tt_ do ko dùng timesten nữa
                    v_strSQL = Replace(v_strSQL, "TT_", "")
                    v_strSQL = Replace(v_strSQL, "tt_", "")
                    v_strSQL = Replace(v_strSQL, "tT_", "")
                    v_strSQL = Replace(v_strSQL, "Tt_", "")
                    'end BangPV

                    v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                    If v_trace_status = "1" Then
                        Trace.WriteLine("[RP] " & DateTime.Now & " :" & vbCrLf & " -o- Ket qua:-o-" & vbCrLf & v_strSQL & vbCrLf)
                        'Trace.WriteLine(v_ds.GetXml & vbCrLf)
                    End If
                End If
                BuildXMLObjData(v_ds, v_strObjMsg)
            End If
            'End If
            pv_xmlDocument.LoadXml(v_strObjMsg)

            v_obj.Commit()
            'ContextUtil.SetComplete()
            Return v_lngErr
        Catch ex As Exception
            Throw ex
            'ContextUtil.SetAbort()
            v_obj.Commit()
        Finally
            If v_trace_status = "1" Then
                Trace.WriteLine("[Kết thúc: RP - " & v_strRptID & "] " & DateTime.Now & vbCrLf)
                tr2.Close()
                tr2.Dispose()
            End If
            v_obj.Dispose()
            GC.Collect()
        End Try
    End Function

    Function FetchRpt(ByVal pv_strRptDataKey As String, ByVal pv_intFrom As Integer, ByVal pv_intTo As Integer) As DataSet
        Try
            Dim v_ds As DataSet = New DataSet
            If ReportDataSets.ContainsKey(pv_strRptDataKey) Then
                Dim v_intRowCount = ReportDataSets(pv_strRptDataKey).Tables(0).Rows.Count
                v_ds = ReportDataSets(pv_strRptDataKey).Clone()
                For i As Integer = pv_intFrom To pv_intTo - 1
                    If i <= v_intRowCount - 1 Then
                        v_ds.Tables(0).ImportRow(ReportDataSets(pv_strRptDataKey).Tables(0).Rows(i))
                    End If
                Next
                If v_intRowCount <= pv_intTo Then
                    ReportDataSets(pv_strRptDataKey).Dispose()
                    ReportDataSets.Remove(pv_strRptDataKey)
                End If
            End If
            Return v_ds
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetServerRptExport(ByVal v_strRptId As String, ByVal pv_strTLName As String, ByVal pv_strSiCode As String, ByRef pv_strSiCodeAllow As String) As Integer
        Dim v_ds As DataSet
        Dim v_obj As New DataAccess
        Dim v_intRptExport As Integer = 0
        Dim v_lstRptId As String
        Dim v_strSiCodeAllow As String = ""
        Try
            v_obj.NewDBInstance(gc_MODULE_INQUERY)
            Dim v_strSQL As String = "select count(1) from tlprofiles a, tlgroups b, tlgrpusers c " _
                & " where a.tlname = '" & pv_strTLName & "' and a.deleted = 0 and a.status = 0" _
                & " and a.tlid = c.tlid " _
                & " and b.grpid = c.grpid and b.server_rpt_exp = 1" _
                & " and c.deleted = 0 and c.status = 0 and b.deleted = 0 and b.status = 0"
            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            v_intRptExport = v_ds.Tables(0).Rows(0)(0)
            If v_intRptExport > 0 Then
                v_strSQL = "select varvalue from sysvar where grname = 'EXPORT' and varname = 'REPORT_LIST'"
                v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                v_lstRptId = v_ds.Tables(0).Rows(0)(0).ToString()
                '20250507 mo cho tat ca cac ma co the len server
                'If v_lstRptId <> String.Empty Then
                '    If v_lstRptId.Split(",").Contains(v_strRptId) Then
                '        v_strSQL = "select sicode from rgsi where allow_svr_exp = 1 and deleted = 0 and status = 0 and sicode in " & pv_strSiCode
                '        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                '        If v_ds.Tables(0).Rows.Count > 0 Then
                '            For i As Integer = 0 To v_ds.Tables(0).Rows.Count - 1
                '                v_strSiCodeAllow = v_strSiCodeAllow & "'" & v_ds.Tables(0).Rows(i)(0).ToString() & "',"
                '            Next
                '        End If
                '        pv_strSiCodeAllow = v_strSiCodeAllow
                '    Else
                '        pv_strSiCodeAllow = pv_strSiCode
                '    End If
                'Else
                '    pv_strSiCodeAllow = pv_strSiCode
                'End If
                pv_strSiCodeAllow = pv_strSiCode
            End If
            Return v_intRptExport
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetVsdBrid(ByVal pv_strTlName As String) As String
        Dim v_ds As DataSet
        Dim v_obj As New DataAccess
        Try
            v_obj.NewDBInstance(gc_MODULE_INQUERY)
            Dim v_strSQL As String = "select decode(a.vsd_brid,'01','HN','03','HN','HCM') from tlprofiles a " _
                & " where a.tlname = '" & pv_strTlName & "' and a.deleted = 0 and a.status = 0"
            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            Return v_ds.Tables(0).Rows(0)(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Public Function CreateReport(ByRef pv_xmlDocument As XmlDocumentEx, ByRef pv_strReportDataKey As String, Optional ByRef v_strCAKey As String = "") As DataSet
        Dim v_lngErr As Long = ERR_SYSTEM_OK
        Dim v_obj As New DataAccess
        Dim tr2 As TextWriterTraceListener
        Dim v_t3 As DataSet
        Dim v_trace_status, v_trace_path As String
        Dim v_strRptID As String
        Try
            Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes
            Dim v_strSQL, v_strSQLTmp As String
            Dim v_strObjMsg As String
            Dim v_strClause As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCLAUSE), Xml.XmlAttribute).Value)
            Dim v_strTellerID As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTLID), Xml.XmlAttribute).Value)
            Dim v_arrFilter As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCMDINQUIRY), Xml.XmlAttribute).Value)
            Dim v_strBRID As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeBRID), Xml.XmlAttribute).Value)
            Dim v_strCurdate As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTXDATE), Xml.XmlAttribute).Value)
            Dim v_strTLName As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTLNAME), Xml.XmlAttribute).Value)
            Dim v_strIsSignCA As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeSIGNCA), Xml.XmlAttribute).Value)
            Dim v_strPartition As String

            Dim pv_strMessage As String
            If Not (v_attrColl.GetNamedItem(gc_AtributeTXNUM) Is Nothing) Then
                v_strPartition = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTXNUM), Xml.XmlAttribute).Value)
            Else
                v_strPartition = ""
            End If
            v_trace_status = "0"

            v_obj.NewDBInstance(gc_MODULE_INQUERY)
            v_obj.BeginTran()

            v_strObjMsg = pv_xmlDocument.InnerXml()
            'bangpv
            ' Purpose : get from  date T-3 to date T
            Dim v_strT_T_3 As String
            v_strSQL = "select to_char(GET_T_PLUS(to_date('" & v_strCurdate & "', 'dd/mm/yyyy'),'" & v_strBRID & "',-1),'dd/mm/yyyy') txdate from dual" _
                    & " union select to_char(GET_T_PLUS(to_date('" & v_strCurdate & "', 'dd/mm/yyyy'),'" & v_strBRID & "',-2),'dd/mm/yyyy') txdate from dual" _
                    & " union select to_char(GET_T_PLUS(to_date('" & v_strCurdate & "', 'dd/mm/yyyy'),'" & v_strBRID & "',-3),'dd/mm/yyyy') txdate from dual"
            v_t3 = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            v_strT_T_3 = "to_date('" & v_strCurdate & "', 'dd/mm/yyyy'),"
            For i = 0 To v_t3.Tables(0).Rows.Count - 1
                v_strT_T_3 = v_strT_T_3 & "to_date('" & v_t3.Tables(0).Rows(i)("txdate") & "', 'dd/mm/yyyy'),"
            Next
            v_strT_T_3 = "(" & Left(v_strT_T_3, v_strT_T_3.Length - 1) & ")"
            ' end 
            'end bangpv
            v_obj.GetSysVar("SYSTEM", "TRACE_STATUS", v_trace_status)
            Dim v_strDirCurrDate As String
            If v_trace_status = "1" Then
                v_obj.GetSysVar("SYSTEM", "TRACE_PATH", v_trace_path)
                'Dim v_strTLName As String = v_obj.GetValue("TLPROFILES", "TLNAME", "TLID='" & v_strTellerID & "'")

                v_strDirCurrDate = Replace(v_strCurdate, "/", "_")
                If v_trace_path = "" Then
                    Dim v_app As New ApplicationServices.ApplicationBase
                    v_trace_path = v_app.Info.DirectoryPath & "\Log\" & v_strDirCurrDate
                Else
                    v_trace_path = v_trace_path & v_strDirCurrDate
                End If

                If Not System.IO.Directory.Exists(v_trace_path) Then
                    System.IO.Directory.CreateDirectory(v_trace_path)
                End If

                v_trace_path &= "\log_report_br" & v_strBRID & "_" & v_strTLName & ".txt"

                tr2 = New TextWriterTraceListener(System.IO.File.AppendText(v_trace_path))
                Trace.Listeners.Add(tr2)
                Trace.WriteLine("[Bắt đầu: RP - " & v_strRptID & "] " & DateTime.Now & vbCrLf)
            End If

            Dim v_ds, v_ds1 As DataSet
            Dim v_trace As DataSet
            Dim v_lstBrID As String
            Dim v_blnSearch As Boolean = False
            Dim v_blnLoadAll As Boolean = False
            Dim v_strMinDate, v_strMaxDate As String

            Dim v_arrTemp() As String

            Dim v_arrField() As String
            Dim v_strFieldCode As String = ""
            v_arrTemp = v_strClause.Split("#")
            v_strRptID = v_arrTemp(0)
            v_arrField = v_arrTemp(1).Split("$")

            'Xu ly DL bang TRAN, TLLOG
            Dim v_strMainSQL As String
            v_strSQL = "SELECT (RPTCMDSQL || nvl(RPTCMDSQL1,'')) RPTCMDSQL , TLLOGTRAN FROM RPREPORTS WHERE RPTID = '" & v_strRptID & "' AND DELETED=0 AND STATUS=0"
            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

            Dim v_strTran As String = "" 'IA|MA|CA|MF|RA|RG|SF|TLLOG
            Dim v_lstTran() As String

            If v_ds.Tables(0).Rows.Count > 0 Then
                v_strTran = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("TLLOGTRAN")).ToUpper
                v_strMainSQL = v_ds.Tables(0).Rows(0)("RPTCMDSQL")
            End If

            v_strMinDate = v_strCurdate
            v_strMaxDate = v_strCurdate
            If v_strPartition <> "" Then
                v_strMinDate = v_strPartition.Split("|")(0)
                v_strMaxDate = v_strPartition.Split("|")(1)
            End If
            v_blnSearch = CheckPassDate(v_strMaxDate, v_strCurdate)
            v_blnLoadAll = CheckPassDate(v_strMinDate, v_strCurdate)


            If v_strTran <> "" Then
                v_lstTran = v_strTran.Split("|")
                v_strTran = ""
                For i As Integer = 0 To v_lstTran.Length - 1
                    Select Case v_lstTran(i)
                        Case "TLLOG"
                            v_strTran &= ",'TLLOG'"
                            If v_blnLoadAll Then
                                v_strTran &= ",'TLLOGALL'"
                            End If

                        Case "CA", "IA", "MA", "RA", "SF"
                            v_strTran &= ",'" & v_lstTran(i) & "TRAN'"
                            v_strTran &= ",'" & v_lstTran(i) & "MAST'"
                            If v_blnLoadAll Then
                                v_strTran &= ",'" & v_lstTran(i) & "TRANA'"
                            End If
                        Case "RG", "MF", "CS"
                            v_strTran &= ",'" & v_lstTran(i) & "TRAN'"
                            If v_blnLoadAll Then
                                v_strTran &= ",'" & v_lstTran(i) & "TRANA'"
                            End If
                    End Select
                Next
                v_strTran = "(" & Mid(v_strTran, 2) & ")"
            End If

            Dim v_lstSICODE, v_lstMICODE As String

            v_lstMICODE = v_arrFilter.Split("|")(1)
            v_lstSICODE = v_arrFilter.Split("|")(0)

            'Lay phan quyen chi nhanh
            v_strSQL = "SELECT DISTINCT brid brid  FROM " _
                       & " (SELECT b.brid FROM tlbridauth a, brgrp b" _
                       & " WHERE ((AUTHID = '" & v_strTellerID & "' AND authtype = 'U') OR (AUTHID IN (SELECT a.grpid" _
                       & " FROM tlgrpusers a WHERE a.tlid = '" & v_strTellerID & "') AND authtype = 'G'))" _
                       & " And a.brid = b.brid And a.deleted = 0 And a.status = 0" _
                       & " And b.deleted = 0 And b.status = 0" _
                       & " UNION " _
                       & " SELECT b.brid FROM tlprofiles a, brgrp b" _
                       & " WHERE a.brid = b.brid AND a.tlid = '" & v_strTellerID & "'" _
                       & " AND a.deleted = 0 AND a.status = 0 AND b.deleted = 0 AND b.status = 0)"

            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

            v_lstBrID = ""
            For i = 0 To v_ds.Tables(0).Rows.Count - 1
                v_lstBrID &= ",'" & v_ds.Tables(0).Rows(i)(0) & "'"
            Next
            v_lstBrID = "(" & Mid(v_lstBrID, 2) & ")"

            v_strSQL = "INSERT INTO tmp_rgmi " _
                       & " SELECT DISTINCT m.* FROM rgmi m WHERE m.deleted=0 AND m.status=0" _
                       & " AND m.MICODE IN " & v_lstMICODE
            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
            'bangpv: Bỏ lấy chưa lưu ký
            'v_strSQL = "INSERT INTO TMP_RGMI(AUTOID, MICODE) SELECT '0' AUTOID, '000' MICODE FROM DUAL"
            'v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
            'end bangpv
            ''Lay chung khoan dc phan quyen cho user vao bang TT_TMP_RGSI
            'v_strSQL = "INSERT INTO tmp_rgsi(iscode, sicode, stock_name, part_value, TYPE," _
            '            & "stock_type, interest_rate, interest_period, " _
            '            & "bond_period, deleted, exchange_rate, note, " _
            '            & "bond_term, release_series, release_mode, isin, " _
            '            & "rarate, tmpid, paidmethod, npaiddate1, npaiddate2, " _
            '            & "npaiddate3, npaiddate4, int_release_mode, brid, status, issuer_date, due_date) " _
            '            & " SELECT distinct m.iscode, m.sicode, m.stock_name, m.part_value, m.TYPE," _
            '            & "m.stock_type, m.interest_rate, m.interest_period," _
            '            & "m.bond_period, m.deleted, m.exchange_rate, m.note, " _
            '            & "m.bond_term, m.release_series, m.release_mode, m.isin, " _
            '            & "m.rarate, m.tmpid, m.paidmethod, m.npaiddate1, m.npaiddate2," _
            '            & "m.npaiddate3, m.npaiddate4, m.int_release_mode, m.brid, m.status, m.issuer_date, m.due_date FROM rgsi m" _
            '            & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_lstSICODE
            'v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
            'Dim v_strCut, v_strCut1, v_strCut2, v_strCut3, v_strCut4, v_strCut5 As String
            'chia lam 5 khoi

            'Dim v_iCut As Integer
            'v_iCut = (v_lstSICODE.Length \ 5)
            'v_iCut = InStr(v_iCut, v_lstSICODE, ",")
            'If v_iCut > 0 Then
            '    v_strCut1 = v_lstSICODE.Substring(0, v_iCut - 1) & ")"
            '    v_strSQL = "INSERT INTO tmp_rgsi( iscode, sicode, stock_name, part_value, type," _
            '                            & " status, stock_type, interest_rate, interest_period, " _
            '                            & " bond_period, due_date, issuer_date, effective_date, " _
            '                            & " deleted, exchange_date, exchange_rate, note, bond_term, " _
            '                            & "release_series, release_mode, isin, brid, rarate, " _
            '                            & "tmpid, paidmethod, npaiddate1, npaiddate2, npaiddate3, " _
            '                            & "npaiddate4, int_release_mode, currency_code, public_date, " _
            '                            & "stock_id, listed_qtty, bond_period_unit, sbv_sicode, vsdbrid) " _
            '                            & " SELECT distinct m.iscode, m.sicode, m.stock_name, m.part_value, m.type," _
            '                            & "m.status, m.stock_type, m.interest_rate, m.interest_period, " _
            '                            & "m.bond_period, m.due_date, m.issuer_date, m.effective_date, " _
            '                            & "m.deleted, m.exchange_date, m.exchange_rate, m.note, m.bond_term, " _
            '                            & "m.release_series, m.release_mode, m.isin, m.brid, m.rarate, " _
            '                            & "m.tmpid, m.paidmethod, m.npaiddate1, m.npaiddate2, m.npaiddate3, " _
            '                            & "m.npaiddate4, m.int_release_mode, m.currency_code, m.public_date, " _
            '                            & "m.stock_id, m.listed_qtty, m.bond_period_unit, m.sbv_sicode,m.vsdbrid FROM rgsi m" _
            '                            & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_strCut1
            '    v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

            '    v_strCut = "(" & v_lstSICODE.Substring(v_iCut)
            '    v_iCut = InStr(v_iCut, v_strCut, ",")
            '    If v_iCut > 0 Then
            '        v_strCut2 = v_strCut.Substring(0, v_iCut - 1) & ")"
            '        v_strSQL = "INSERT INTO tmp_rgsi( iscode, sicode, stock_name, part_value, type," _
            '                            & " status, stock_type, interest_rate, interest_period, " _
            '                            & " bond_period, due_date, issuer_date, effective_date, " _
            '                            & " deleted, exchange_date, exchange_rate, note, bond_term, " _
            '                            & "release_series, release_mode, isin, brid, rarate, " _
            '                            & "tmpid, paidmethod, npaiddate1, npaiddate2, npaiddate3, " _
            '                            & "npaiddate4, int_release_mode, currency_code, public_date, " _
            '                            & "stock_id, listed_qtty, bond_period_unit, sbv_sicode, vsdbrid) " _
            '                            & " SELECT distinct m.iscode, m.sicode, m.stock_name, m.part_value, m.type," _
            '                            & "m.status, m.stock_type, m.interest_rate, m.interest_period, " _
            '                            & "m.bond_period, m.due_date, m.issuer_date, m.effective_date, " _
            '                            & "m.deleted, m.exchange_date, m.exchange_rate, m.note, m.bond_term, " _
            '                            & "m.release_series, m.release_mode, m.isin, m.brid, m.rarate, " _
            '                            & "m.tmpid, m.paidmethod, m.npaiddate1, m.npaiddate2, m.npaiddate3, " _
            '                            & "m.npaiddate4, m.int_release_mode, m.currency_code, m.public_date, " _
            '                            & "m.stock_id, m.listed_qtty, m.bond_period_unit, m.sbv_sicode,m.vsdbrid FROM rgsi m" _
            '              & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_strCut2
            '        v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

            '        v_strCut = "(" & v_strCut.Substring(v_iCut)
            '        v_iCut = InStr(v_iCut, v_strCut, ",")
            '        '3
            '        If v_iCut > 0 Then
            '            v_strCut3 = v_strCut.Substring(0, v_iCut - 1) & ")"
            '            v_strSQL = "INSERT INTO tmp_rgsi( iscode, sicode, stock_name, part_value, type," _
            '                            & " status, stock_type, interest_rate, interest_period, " _
            '                            & " bond_period, due_date, issuer_date, effective_date, " _
            '                            & " deleted, exchange_date, exchange_rate, note, bond_term, " _
            '                            & "release_series, release_mode, isin, brid, rarate, " _
            '                            & "tmpid, paidmethod, npaiddate1, npaiddate2, npaiddate3, " _
            '                            & "npaiddate4, int_release_mode, currency_code, public_date, " _
            '                            & "stock_id, listed_qtty, bond_period_unit, sbv_sicode, vsdbrid) " _
            '                            & " SELECT distinct m.iscode, m.sicode, m.stock_name, m.part_value, m.type," _
            '                            & "m.status, m.stock_type, m.interest_rate, m.interest_period, " _
            '                            & "m.bond_period, m.due_date, m.issuer_date, m.effective_date, " _
            '                            & "m.deleted, m.exchange_date, m.exchange_rate, m.note, m.bond_term, " _
            '                            & "m.release_series, m.release_mode, m.isin, m.brid, m.rarate, " _
            '                            & "m.tmpid, m.paidmethod, m.npaiddate1, m.npaiddate2, m.npaiddate3, " _
            '                            & "m.npaiddate4, m.int_release_mode, m.currency_code, m.public_date, " _
            '                            & "m.stock_id, m.listed_qtty, m.bond_period_unit, m.sbv_sicode,m.vsdbrid FROM rgsi m" _
            '                    & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_strCut3
            '            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

            '            v_strCut = "(" & v_strCut.Substring(v_iCut)
            '            v_iCut = InStr(v_iCut, v_strCut, ",")
            '            If v_iCut > 0 Then
            '                v_strCut4 = v_strCut.Substring(0, v_iCut - 1) & ")"
            '                v_strSQL = "INSERT INTO tmp_rgsi( iscode, sicode, stock_name, part_value, type," _
            '                            & " status, stock_type, interest_rate, interest_period, " _
            '                            & " bond_period, due_date, issuer_date, effective_date, " _
            '                            & " deleted, exchange_date, exchange_rate, note, bond_term, " _
            '                            & "release_series, release_mode, isin, brid, rarate, " _
            '                            & "tmpid, paidmethod, npaiddate1, npaiddate2, npaiddate3, " _
            '                            & "npaiddate4, int_release_mode, currency_code, public_date, " _
            '                            & "stock_id, listed_qtty, bond_period_unit, sbv_sicode, vsdbrid) " _
            '                            & " SELECT distinct m.iscode, m.sicode, m.stock_name, m.part_value, m.type," _
            '                            & "m.status, m.stock_type, m.interest_rate, m.interest_period, " _
            '                            & "m.bond_period, m.due_date, m.issuer_date, m.effective_date, " _
            '                            & "m.deleted, m.exchange_date, m.exchange_rate, m.note, m.bond_term, " _
            '                            & "m.release_series, m.release_mode, m.isin, m.brid, m.rarate, " _
            '                            & "m.tmpid, m.paidmethod, m.npaiddate1, m.npaiddate2, m.npaiddate3, " _
            '                            & "m.npaiddate4, m.int_release_mode, m.currency_code, m.public_date, " _
            '                            & "m.stock_id, m.listed_qtty, m.bond_period_unit, m.sbv_sicode,m.vsdbrid FROM rgsi m" _
            '                        & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_strCut4
            '                v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

            '                v_strCut5 = "(" & v_strCut.Substring(v_iCut)

            '                v_strSQL = "INSERT INTO tmp_rgsi( iscode, sicode, stock_name, part_value, type," _
            '                            & " status, stock_type, interest_rate, interest_period, " _
            '                            & " bond_period, due_date, issuer_date, effective_date, " _
            '                            & " deleted, exchange_date, exchange_rate, note, bond_term, " _
            '                            & "release_series, release_mode, isin, brid, rarate, " _
            '                            & "tmpid, paidmethod, npaiddate1, npaiddate2, npaiddate3, " _
            '                            & "npaiddate4, int_release_mode, currency_code, public_date, " _
            '                            & "stock_id, listed_qtty, bond_period_unit, sbv_sicode, vsdbrid) " _
            '                            & " SELECT distinct m.iscode, m.sicode, m.stock_name, m.part_value, m.type," _
            '                            & "m.status, m.stock_type, m.interest_rate, m.interest_period, " _
            '                            & "m.bond_period, m.due_date, m.issuer_date, m.effective_date, " _
            '                            & "m.deleted, m.exchange_date, m.exchange_rate, m.note, m.bond_term, " _
            '                            & "m.release_series, m.release_mode, m.isin, m.brid, m.rarate, " _
            '                            & "m.tmpid, m.paidmethod, m.npaiddate1, m.npaiddate2, m.npaiddate3, " _
            '                            & "m.npaiddate4, m.int_release_mode, m.currency_code, m.public_date, " _
            '                            & "m.stock_id, m.listed_qtty, m.bond_period_unit, m.sbv_sicode,m.vsdbrid FROM rgsi m" _
            '                        & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_strCut5
            '                v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

            '            Else
            '                v_strSQL = "INSERT INTO tmp_rgsi( iscode, sicode, stock_name, part_value, type," _
            '                            & " status, stock_type, interest_rate, interest_period, " _
            '                            & " bond_period, due_date, issuer_date, effective_date, " _
            '                            & " deleted, exchange_date, exchange_rate, note, bond_term, " _
            '                            & "release_series, release_mode, isin, brid, rarate, " _
            '                            & "tmpid, paidmethod, npaiddate1, npaiddate2, npaiddate3, " _
            '                            & "npaiddate4, int_release_mode, currency_code, public_date, " _
            '                            & "stock_id, listed_qtty, bond_period_unit, sbv_sicode, vsdbrid) " _
            '                            & " SELECT distinct m.iscode, m.sicode, m.stock_name, m.part_value, m.type," _
            '                            & "m.status, m.stock_type, m.interest_rate, m.interest_period, " _
            '                            & "m.bond_period, m.due_date, m.issuer_date, m.effective_date, " _
            '                            & "m.deleted, m.exchange_date, m.exchange_rate, m.note, m.bond_term, " _
            '                            & "m.release_series, m.release_mode, m.isin, m.brid, m.rarate, " _
            '                            & "m.tmpid, m.paidmethod, m.npaiddate1, m.npaiddate2, m.npaiddate3, " _
            '                            & "m.npaiddate4, m.int_release_mode, m.currency_code, m.public_date, " _
            '                            & "m.stock_id, m.listed_qtty, m.bond_period_unit, m.sbv_sicode,m.vsdbrid FROM rgsi m" _
            '                          & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_strCut
            '                v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
            '            End If
            '        End If
            '    End If
            'Else
            '    v_strSQL = "INSERT INTO tmp_rgsi( iscode, sicode, stock_name, part_value, type," _
            '                            & " status, stock_type, interest_rate, interest_period, " _
            '                            & " bond_period, due_date, issuer_date, effective_date, " _
            '                            & " deleted, exchange_date, exchange_rate, note, bond_term, " _
            '                            & "release_series, release_mode, isin, brid, rarate, " _
            '                            & "tmpid, paidmethod, npaiddate1, npaiddate2, npaiddate3, " _
            '                            & "npaiddate4, int_release_mode, currency_code, public_date, " _
            '                            & "stock_id, listed_qtty, bond_period_unit, sbv_sicode, vsdbrid) " _
            '                            & " SELECT distinct m.iscode, m.sicode, m.stock_name, m.part_value, m.type," _
            '                            & "m.status, m.stock_type, m.interest_rate, m.interest_period, " _
            '                            & "m.bond_period, m.due_date, m.issuer_date, m.effective_date, " _
            '                            & "m.deleted, m.exchange_date, m.exchange_rate, m.note, m.bond_term, " _
            '                            & "m.release_series, m.release_mode, m.isin, m.brid, m.rarate, " _
            '                            & "m.tmpid, m.paidmethod, m.npaiddate1, m.npaiddate2, m.npaiddate3, " _
            '                            & "m.npaiddate4, m.int_release_mode, m.currency_code, m.public_date, " _
            '                            & "m.stock_id, m.listed_qtty, m.bond_period_unit, m.sbv_sicode,m.vsdbrid FROM rgsi m" _
            '                  & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_lstSICODE
            '    v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
            'End If
            'HaNM5-28/10/2020-Sua loi tren 1000 ma
            Dim v_arrSICODE() As String = v_lstSICODE.Replace("(", "").Replace(")", "").Split(",")
            Dim v_intStart As Integer = 0
            Dim v_intEnd As Integer = 0
            Dim v_intOffSet As Integer = 990
            Dim v_strSiCodeList As String = ""
            Dim v_intSiCodeCount As Integer = v_arrSICODE.Length
            While v_intStart < v_intSiCodeCount
                v_strSiCodeList = ""
                If v_intStart + v_intOffSet >= v_intSiCodeCount Then
                    v_intEnd = v_intSiCodeCount - 1
                Else
                    v_intEnd = v_intStart + v_intOffSet - 1
                End If
                For i As Integer = v_intStart To v_intEnd
                    v_strSiCodeList = v_strSiCodeList & "," & v_arrSICODE(i)
                Next
                If v_strSiCodeList.Length > 0 Then
                    v_strSiCodeList = "(" & v_strSiCodeList.Substring(1) & ")"
                    v_strSQL = "INSERT INTO tmp_rgsi(iscode, sicode, stock_name, part_value, TYPE," _
                                      & "stock_type, interest_rate, interest_period, " _
                                      & "bond_period, deleted, exchange_rate, note, " _
                                      & "bond_term, release_series, release_mode, isin, " _
                                      & "rarate, tmpid, paidmethod, npaiddate1, npaiddate2, " _
                                      & "npaiddate3, npaiddate4, int_release_mode, brid, status, public_date, currency_code,vsdbrid) " _
                                      & " SELECT distinct m.iscode, m.sicode, m.stock_name, m.part_value, m.TYPE," _
                                      & "m.stock_type, m.interest_rate, m.interest_period," _
                                      & "m.bond_period, m.deleted, m.exchange_rate, m.note, " _
                                      & "m.bond_term, m.release_series, m.release_mode, m.isin, " _
                                      & "m.rarate, m.tmpid, m.paidmethod, m.npaiddate1, m.npaiddate2," _
                                      & "m.npaiddate3, m.npaiddate4, m.int_release_mode, m.brid, m.status, m.public_date, m.currency_code,m.vsdbrid FROM rgsi m" _
                                      & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_strSiCodeList
                    v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                End If
                v_intStart = v_intStart + v_intOffSet
            End While
            'End HaNM5 sua
            ' tuanta
            v_strSQL = "INSERT INTO TMP_RGSI(AUTOID, SICODE) SELECT 0 AUTOID, '000' SICODE FROM DUAL"
            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

            'Lay cau xu ly chung
            Dim v_hSQL As New Hashtable
            Dim v_hORD As New Hashtable
            If v_strTran <> "" Then
                v_strSQL = "SELECT * FROM RPSTORES WHERE RPTID IS NULL AND OVERWRITE IN " & v_strTran & " ORDER BY ODRNUM"

                v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                Dim v_strOverWrite As String
                If v_ds.Tables(0).Rows.Count > 0 Then
                    For i As Integer = 0 To v_ds.Tables(0).Rows.Count - 1
                        v_strOverWrite = v_ds.Tables(0).Rows(i)("OVERWRITE")
                        v_hORD(i) = v_strOverWrite
                        v_hSQL(v_strOverWrite) = v_ds.Tables(0).Rows(i)("TRACE") & v_ds.Tables(0).Rows(i)("CMDSQL") & " " & v_ds.Tables(0).Rows(i)("CMDSQL1")
                    Next
                End If

                'Lay cau xu ly chung da dc viet lai
                v_strSQL = "SELECT * FROM RPSTORES WHERE RPTID='" & v_strRptID & "' AND OVERWRITE IN " & v_strTran & " ORDER BY ODRNUM"
                v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

                If v_ds.Tables(0).Rows.Count > 0 Then
                    For i As Integer = 0 To v_ds.Tables(0).Rows.Count - 1
                        v_strOverWrite = v_ds.Tables(0).Rows(i)("OVERWRITE")
                        If Not v_hSQL(v_strOverWrite) Is Nothing Then
                            v_hSQL(v_strOverWrite) = v_ds.Tables(0).Rows(i)("TRACE") & v_ds.Tables(0).Rows(i)("CMDSQL") & " " & v_ds.Tables(0).Rows(i)("CMDSQL1")
                        End If
                    Next
                End If

                'Thuc hien cau xu ly chung
                For i As Integer = 0 To v_hORD.Count - 1
                    v_strOverWrite = v_hORD(i)
                    v_strSQL = Mid(v_hSQL(v_strOverWrite), 2)
                    'v_strPartitionSQL = ""

                    v_strSQL = Replace(v_strSQL, "[!BRID]", v_strBRID)
                    v_strSQL = Replace(v_strSQL, "[!TLID]", v_strTellerID)
                    v_strSQL = Replace(v_strSQL, "[!LBRID]", v_lstBrID)
                    v_strSQL = Replace(v_strSQL, "[!T_T_3]", v_strT_T_3)
                    v_strSQL = Replace(v_strSQL, "[!MINDATE]", "TO_DATE('" & v_strMinDate & "','dd/mm/yyyy')")
                    v_strSQL = Replace(v_strSQL, "[!MAXDATE]", "TO_DATE('" & v_strMaxDate & "','dd/mm/yyyy')")
                    'BangPV: Bỏ tt_ do ko dùng timesten nữa
                    v_strSQL = Replace(v_strSQL, "TT_", "")
                    v_strSQL = Replace(v_strSQL, "tt_", "")
                    v_strSQL = Replace(v_strSQL, "tT_", "")
                    v_strSQL = Replace(v_strSQL, "Tt_", "")
                    'end BangPV
                    v_strSQL = Replace(v_strSQL, "[!TLLOGALL]", "TLLOGALL_ALL")
                    v_strSQL = Replace(v_strSQL, "[!CATRANA]", "CATRANA_ALL")
                    v_strSQL = Replace(v_strSQL, "[!IATRANA]", "IATRANA_ALL")
                    v_strSQL = Replace(v_strSQL, "[!MATRANA]", "MATRANA_ALL")
                    v_strSQL = Replace(v_strSQL, "[!SFTRANA]", "SFTRANA_ALL")
                    v_strSQL = Replace(v_strSQL, "[!MFTRANA]", "MFTRANA_ALL")
                    v_strSQL = Replace(v_strSQL, "[!RATRANA]", "RATRANA_ALL")
                    v_strSQL = Replace(v_strSQL, "[!RGTRANA]", "RGTRANA_ALL")

                    For l As Integer = 0 To v_arrField.Count - 2
                        v_strSQL = Replace(v_strSQL, "[!" & v_arrField(l).Split("|")(0) & "]", v_arrField(l).Split("|")(1))
                        'bangpv: CA
                        'If v_strIsSignCA = "1" Then
                        '    If v_arrField(l).Split("|")(4) = "1" Then
                        '        v_strCAKey = v_strCAKey & v_arrField(l).Split("|")(0) & "@" & v_arrField(l).Split("|")(3) & "$"
                        '    End If
                        'End If
                        'end bangpv
                    Next

                    If v_trace_status = "1" And Mid(v_hSQL(v_strOverWrite), 1, 1) = "1" Then
                        Trace.WriteLine("[RP] " & DateTime.Now & " :" & vbCrLf & " -o- Câu lệnh thứ xử lý chung thứ #" & i + 1 & "-o-" & vbCrLf & v_strSQL & vbCrLf)
                        If InStr(v_strSQL.ToUpper, "SELECT") > 0 Then
                            Trace.WriteLine("-o- Dữ liệu câu lệnh xử lý chung thứ #" & i + 1 & "-o-")
                            v_strSQLTmp = v_strSQL.Substring(InStr(v_strSQL.ToUpper, "SELECT") - 1)
                            v_trace = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                            Trace.WriteLine(v_trace.GetXml & vbCrLf)
                        End If
                    End If
                    'v_obj.ExecuteNonQuery(CommandType.Text, v_strPartitionSQL)
                    v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                Next
            End If

            v_strSQL = "SELECT * FROM RPSTORES WHERE RPTID='" & v_strRptID & "' AND OVERWRITE IS NULL AND DELETED=0 AND STATUS=0 ORDER BY ODRNUM"
            v_ds1 = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

            Dim v_int As Integer

            For v_int = 0 To v_ds1.Tables(0).Rows.Count - 1

                v_strSQL = v_ds1.Tables(0).Rows(v_int)("CMDSQL").ToString & " " & v_ds1.Tables(0).Rows(v_int)("CMDSQL1").ToString

                'If Not CheckTranTable(v_strSQL) Then
                '    v_lngErr = 1
                '    Exit For
                'End If

                For i As Integer = 0 To v_arrField.Count - 2
                    v_strSQL = Replace(v_strSQL, "[!" & v_arrField(i).Split("|")(0) & "]", v_arrField(i).Split("|")(1))
                    'bangpv: CA
                    'If v_strIsSignCA = "1" Then
                    '    If v_arrField(i).Split("|")(4) = "1" Then
                    '        v_strCAKey = v_strCAKey & v_arrField(i).Split("|")(0) & "@" & v_arrField(i).Split("|")(3) & "$"
                    '    End If
                    'End If
                    'end bangpv
                Next
                ' purpose : phi luu ky
                If InStr(v_strSQL, "[!MFNO_01]", CompareMethod.Text) Then
                    v_strSQLTmp = "select fomula from mftype where deleted=0 and status=0 and mfno='01'"
                    v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                    If v_ds.Tables(0).Rows.Count > 0 Then
                        v_strSQL = Replace(v_strSQL, "[!MFNO_01]", v_ds.Tables(0).Rows(0)(0))
                    Else
                        v_strSQL = Replace(v_strSQL, "[!MFNO_01]", "0")
                    End If
                End If
                ' end phi luu ky
                v_strSQL = Replace(v_strSQL, "[!BRID]", v_strBRID)
                v_strSQL = Replace(v_strSQL, "[!TLID]", v_strTellerID)
                v_strSQL = Replace(v_strSQL, "[!LBRID]", v_lstBrID)
                v_strSQL = Replace(v_strSQL, "[!T_T_3]", v_strT_T_3)
                v_strSQL = Replace(v_strSQL, "[!MINDATE]", "TO_DATE('" & v_strMinDate & "','dd/mm/yyyy')")
                v_strSQL = Replace(v_strSQL, "[!MAXDATE]", "TO_DATE('" & v_strMaxDate & "','dd/mm/yyyy')")

                'BangPV: Bỏ tt_ do ko dùng timesten nữa
                v_strSQL = Replace(v_strSQL, "TT_", "")
                v_strSQL = Replace(v_strSQL, "tt_", "")
                v_strSQL = Replace(v_strSQL, "tT_", "")
                v_strSQL = Replace(v_strSQL, "Tt_", "")
                'end bangpv

                If v_trace_status = "1" And v_ds1.Tables(0).Rows(v_int)("TRACE") = 1 Then
                    Trace.WriteLine("[RP] " & DateTime.Now & " :" & vbCrLf & " -o- Câu lệnh thứ #" & v_ds1.Tables(0).Rows(v_int)("ODRNUM") & "-o-" & vbCrLf & v_strSQL & vbCrLf)
                    If InStr(v_strSQL.ToUpper, "SELECT") > 0 Then
                        Trace.WriteLine("-o- Dữ liệu câu lệnh thứ #" & v_ds1.Tables(0).Rows(v_int)("ODRNUM") & "-o-")
                        v_strSQLTmp = v_strSQL.Substring(InStr(v_strSQL.ToUpper, "SELECT") - 1)
                        v_trace = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                        Trace.WriteLine(v_trace.GetXml & vbCrLf)
                    End If
                End If

                v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

            Next

            If v_lngErr = ERR_SYSTEM_OK Then
                v_strSQL = v_strMainSQL

                'If Not CheckTranTable(v_strSQL) Then
                '    v_lngErr = 1
                'Else
                '    For i As Integer = 0 To v_arrField.Count - 2
                '        v_strSQL = Replace(v_strSQL, "[!" & v_arrField(i).Split("|")(0) & "]", v_arrField(i).Split("|")(1))
                '    Next

                '    If v_strSQL <> "" Then
                '        v_strSQL = Replace(v_strSQL, "[!BRID]", v_strBRID)
                '        v_strSQL = Replace(v_strSQL, "[!TLID]", v_strTellerID)
                '        v_strSQL = Replace(v_strSQL, "[!LBRID]", v_lstBrID)
                '        v_strSQL = Replace(v_strSQL, "[!MINDATE]", "TO_DATE('" & v_strMinDate & "','dd/mm/yyyy')")
                '        v_strSQL = Replace(v_strSQL, "[!MAXDATE]", "TO_DATE('" & v_strMaxDate & "','dd/mm/yyyy')")

                '        v_strSQL = Replace(v_strSQL, "TT_", "")
                '        v_strSQL = Replace(v_strSQL, "tt_", "")
                '        v_strSQL = Replace(v_strSQL, "tT_", "")
                '        v_strSQL = Replace(v_strSQL, "Tt_", "")

                '        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                '        If v_trace_status = "1" Then
                '            Trace.WriteLine("[RP] " & DateTime.Now & " :" & vbCrLf & " -o- Ket qua:-o-" & vbCrLf & v_strSQL & vbCrLf)
                '            'Trace.WriteLine(v_ds.GetXml & vbCrLf)
                '        End If
                '    End If
                'End If

                For i As Integer = 0 To v_arrField.Count - 2
                    v_strSQL = Replace(v_strSQL, "[!" & v_arrField(i).Split("|")(0) & "]", v_arrField(i).Split("|")(1))

                    'bangpv: CA
                    'If v_strIsSignCA = "1" Then
                    '    If v_arrField(i).Split("|")(4) = "1" Then
                    '        v_strCAKey = v_strCAKey & v_arrField(i).Split("|")(0) & "@" & v_arrField(i).Split("|")(3) & "$"
                    '    End If
                    'End If
                    'end bangpv
                Next
                ' purpose : phi luu ky
                If InStr(v_strSQL, "[!MFNO_01]", CompareMethod.Text) Then
                    v_strSQLTmp = "select fomula from mftype where deleted=0 and status=0 and mfno='01'"
                    v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                    If v_ds.Tables(0).Rows.Count > 0 Then
                        v_strSQL = Replace(v_strSQL, "[!MFNO_01]", v_ds.Tables(0).Rows(0)(0))
                    Else
                        v_strSQL = Replace(v_strSQL, "[!MFNO_01]", "0")
                    End If
                End If
                v_strSQLTmp = "select fomula from mftype where deleted=0 and status=0 and mfno='01'"
                v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                ' end phi luu ky
                'HaNM5 sua 23/12/2020
                Dim v_intMaxTransferRows As Integer = 10000
                Dim v_strMaxTransferRows As String = ""
                v_obj.GetSysVar("SYSTEM", "RPT_MAX_TRANSFER_ROWS", v_strMaxTransferRows)
                If v_strMaxTransferRows <> "" Then
                    v_intMaxTransferRows = Convert.ToInt32(v_strMaxTransferRows)
                End If
                If v_strSQL <> "" Then
                    v_strSQL = Replace(v_strSQL, "[!BRID]", v_strBRID)
                    v_strSQL = Replace(v_strSQL, "[!TLID]", v_strTellerID)
                    v_strSQL = Replace(v_strSQL, "[!LBRID]", v_lstBrID)
                    v_strSQL = Replace(v_strSQL, "[!T_T_3]", v_strT_T_3)
                    v_strSQL = Replace(v_strSQL, "[!MINDATE]", "TO_DATE('" & v_strMinDate & "','dd/mm/yyyy')")
                    v_strSQL = Replace(v_strSQL, "[!MAXDATE]", "TO_DATE('" & v_strMaxDate & "','dd/mm/yyyy')")

                    v_strSQL = Replace(v_strSQL, "TT_", "")
                    v_strSQL = Replace(v_strSQL, "tt_", "")
                    v_strSQL = Replace(v_strSQL, "tT_", "")
                    v_strSQL = Replace(v_strSQL, "Tt_", "")

                    'v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

                    'HaNM5 sua 23/12/2020
                    pv_strReportDataKey = String.Empty
                    ReportExecuteId += 1
                    Dim v_strReportDataKey As String = v_strTellerID & "_" & v_strRptID & "_" & ReportExecuteId & "_" & DateTime.Now
                    Dim v_dsTemp As DataSet = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                    Dim v_lngRowCount As Long = v_dsTemp.Tables(0).Rows.Count
                    If v_lngRowCount <= v_intMaxTransferRows Then
                        v_ds = v_dsTemp.Copy()
                        v_dsTemp.Dispose()
                    Else
                        ReportDataSets.Add(v_strReportDataKey, v_dsTemp)
                        pv_strReportDataKey = v_strReportDataKey
                        v_ds = v_dsTemp.Clone()
                        For i As Integer = 0 To v_intMaxTransferRows - 1
                            v_ds.Tables(0).ImportRow(v_dsTemp.Tables(0).Rows(i))
                        Next

                        Dim v_attr1 As Xml.XmlAttribute = pv_xmlDocument.CreateAttribute(gc_AtributeRptDataKey)
                        v_attr1.Value = v_strReportDataKey
                        pv_xmlDocument.DocumentElement.Attributes.Append(v_attr1)

                        Dim v_attr2 = pv_xmlDocument.CreateAttribute(gc_AtributeRptDataRowCount)
                        v_attr2.Value = v_lngRowCount
                        pv_xmlDocument.DocumentElement.Attributes.Append(v_attr2)
                    End If
                    'HaNM5 sua 23/12/2020

                    If v_trace_status = "1" Then
                        Trace.WriteLine("[RP] " & DateTime.Now & " :" & vbCrLf & " -o- Ket qua:-o-" & vbCrLf & v_strSQL & vbCrLf)
                        'Trace.WriteLine(v_ds.GetXml & vbCrLf)
                    End If
                End If
            End If

            v_obj.Commit()

            'cập nhật vào tlsession 
            If v_strIsSignCA = "1" Then
                pv_strMessage = pv_xmlDocument.InnerXml
                'Dim v_array() As Byte = modCommond.Compression(pv_strMessage)
                For l As Integer = 0 To v_arrField.Count - 2
                    'v_strSQL = Replace(v_strSQL, "[!" & v_arrField(l).Split("|")(0) & "]", v_arrField(l).Split("|")(1))
                    'bangpv: CA
                    If v_arrField(l).Split("|")(4) = "1" Then
                        v_strCAKey = v_strCAKey & v_arrField(l).Split("|")(0) & "@" & v_arrField(l).Split("|")(3) & "$"
                    End If
                    'end bangpv
                Next
                v_strCAKey = Replace(v_strCAKey, "'", "")
                'pv_strMessage = ByteArrayToStr(v_array)
                InsertTLSession(pv_strMessage, v_strTLName, v_strTellerID, v_strRptID, v_strBRID, v_strCurdate, v_strCAKey)
                'v_array = StrToByteArray(pv_strMessage)
                'pv_strMessage = Decompress(v_array)

            End If
            Return v_ds

        Catch ex As Exception
            Throw ex
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                & "Error code: System error!" & vbNewLine _
                & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_HOST)
            v_obj.Commit()
        Finally
            If v_trace_status = "1" Then
                Trace.WriteLine("[Kết thúc: RP - " & v_strRptID & "] " & DateTime.Now & vbCrLf)
                tr2.Close()
                tr2.Dispose()
            End If
            v_obj.Dispose()
            GC.Collect()
        End Try
    End Function
    Public Shared Function GetSessionKey(ByVal pv_strUserName As String, ByRef pv_strSessionKey As String, ByVal pv_strCurdate As String) As Long
        Try
            Dim v_strInquiry = "select tlid from tlprofiles where tlname = '" + pv_strUserName + "'"
            Dim v_oDataAccess = New DataAccessLayer.DataAccess
            Dim v_oDataSet = v_oDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strInquiry)
            If (v_oDataSet.Tables(0).Rows.Count = 0) Then
                v_oDataAccess = Nothing
                Return ""
            ElseIf (v_oDataSet.Tables(0).Rows.Count > 1) Then
                v_oDataAccess = Nothing
                Return -1
            End If

            Dim v_intTLID As Integer = v_oDataSet.Tables(0).Rows(0).Item("TLID")
            v_strInquiry = "select sessionkey from tlsession where 1 = 1"
            v_strInquiry = v_strInquiry + "and tlid =" + v_intTLID.ToString + " "
            v_strInquiry = v_strInquiry + "and type = 1 "
            v_strInquiry = v_strInquiry + "and txdate = to_date('" & pv_strCurdate & "', 'dd/mm/yyyy')"

            v_oDataSet = v_oDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strInquiry)
            If (v_oDataSet.Tables(0).Rows.Count = 0) Then
                v_oDataAccess = Nothing
                Return ""
            ElseIf (v_oDataSet.Tables(0).Rows.Count = 1) Then

                Dim v_strReturn As String = v_oDataSet.Tables(0).Rows(0).Item("SESSIONKEY")
                v_oDataAccess = Nothing
                Return 0
            Else
                Return -1
            End If
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                & "Error code: System error!" & vbNewLine _
                & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_HOST)
            Return -1

        End Try

    End Function
    Public Function InsertTLSession(ByVal pv_strMessage As String, ByVal v_strTLName As String, ByVal v_strTLID As String, _
ByVal v_strRptID As String, ByVal v_strBrid As String, ByVal v_strCurDate As String, ByVal v_strCAKey As String) As Long
        Try
            'Load session key

            Dim v_lngErr As Long = -1
            Dim v_strSessionKey As String
            Dim v_lngCount As Long
            v_lngCount = Len(pv_strMessage) / 1000
            'Dim v_arrByte() As Byte = Compression(pv_strMessage)

            v_lngErr = GetSessionKey(v_strTLName, v_strSessionKey, v_strCurDate)
            If v_lngErr = ERR_SYSTEM_START Then
                Return -1
                Exit Function
            End If
            Dim v_strSQL As String
            Dim v_oDataAccess = New DataAccessLayer.DataAccess
            If v_strCAKey = "" Then
                v_strSQL = "select count(*) count_ from tlsession where tllogid ='" & v_strRptID & "' and tlid ='" & v_strTLID _
            & "' and txdate = to_date('" & v_strCurDate & "', 'dd/mm/yyyy')"
            Else
                v_strSQL = "select count(*) count_ from tlsession where tllogid ='" & v_strRptID & "' and tlid ='" & v_strTLID _
            & "' and txdate = to_date('" & v_strCurDate & "', 'dd/mm/yyyy') and METADIGITALSIGN0='" & v_strCAKey & "'"
            End If

            Dim v_oDataSet = v_oDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            If (v_oDataSet.Tables(0).Rows(0).Item("count_") = 0) Then
                v_oDataAccess.BeginTran()
                'v_strSQL = "insert into tlsession (autoid, tlid, txdate, tllogid, sessionkey, type, rpparameter,rpparameter1,rpparameter2,METADIGITALSIGN0 )" _
                '& " values(seq_tlsession.nextval, '" & v_strTLID & "', to_date('" & v_strCurDate & "', 'dd/mm/yyyy'), '" & v_strRptID & "', '" _
                '& v_strSessionKey & "', 3, '" & Replace(v_strMessage, "'", "''") & "','" & Replace(v_strMessage1, "'", "''") & "','" & Replace(v_strMessage2, "'", "''") _
                '& "','" & v_strCAKey & "')"
                v_strSQL = "insert into tlsession (autoid, tlid, txdate, tllogid, sessionkey, type, crpparameter,METADIGITALSIGN0 )" _
                & " values(seq_tlsession.nextval, '" & v_strTLID & "', to_date('" & v_strCurDate & "', 'dd/mm/yyyy'), '" & v_strRptID & "', '" _
                & v_strSessionKey & "', 3, :TEXT_DATA,'" & v_strCAKey & "')"
                v_lngErr = v_oDataAccess.ExecuteNonQuery(v_strSQL, "TEXT_DATA", pv_strMessage, CommandType.Text)

                If v_lngErr = ERR_SYSTEM_START Then
                    v_oDataAccess.Rollback()
                Else
                    v_oDataAccess.Commit()

                End If
            ElseIf (v_oDataSet.Tables(0).Rows.Count = 1) Then
                v_oDataAccess.BeginTran()
                'v_strSQL = "update tlsession set rpparameter ='" & Replace(v_strMessage, "'", "''") & "', rpparameter1= '" & Replace(v_strMessage1, "'", "''") & "'" _
                '& ",rpparameter2='" & Replace(v_strMessage2, "'", "''") & "' where tllogid ='" & v_strRptID & "' and tlid ='" & v_strTLID _
                '& "' and txdate = to_date('" & v_strCurDate & "', 'dd/mm/yyyy')"
                ' v_oDataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
                If v_strCAKey = "" Then
                    v_strSQL = "update tlsession set crpparameter = :TEXT_DATA  where tllogid ='" & v_strRptID & "' and tlid ='" & v_strTLID _
                    & "' and txdate = to_date('" & v_strCurDate & "', 'dd/mm/yyyy')"

                Else

                    v_strSQL = "update tlsession set crpparameter = :TEXT_DATA  where tllogid ='" & v_strRptID & "' and tlid ='" & v_strTLID _
                    & "' and txdate = to_date('" & v_strCurDate & "', 'dd/mm/yyyy') and METADIGITALSIGN0='" & v_strCAKey & "'"

                End If
                v_lngErr = v_oDataAccess.ExecuteNonQuery(v_strSQL, "TEXT_DATA", pv_strMessage, CommandType.Text)

                If v_lngErr = ERR_SYSTEM_START Then
                    v_oDataAccess.Rollback()
                Else
                    v_oDataAccess.Commit()
                End If
            End If
            'insert vào bảng báo cáo 
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                & "Error code: System error!" & vbNewLine _
                & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_HOST)
            Return -1
        End Try
    End Function
    Public Function CreateReportCheck(ByRef pv_xmlDocument As XmlDocumentEx) As DataSet
        Dim v_lngErr As Long = ERR_SYSTEM_OK
        Dim v_obj As New DataAccess
        Dim tr2 As TextWriterTraceListener
        Dim v_t3 As DataSet
        Dim v_trace_status, v_trace_path As String
        Dim v_strRptID As String
        Try
            Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes
            Dim v_strSQL, v_strSQLTmp As String
            Dim v_strObjMsg As String
            Dim v_strClause As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCLAUSE), Xml.XmlAttribute).Value)
            Dim v_strTellerID As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTLID), Xml.XmlAttribute).Value)
            Dim v_arrFilter As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCMDINQUIRY), Xml.XmlAttribute).Value)
            Dim v_strBRID As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeBRID), Xml.XmlAttribute).Value)
            Dim v_strCurdate As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTXDATE), Xml.XmlAttribute).Value)
            Dim v_strTLName As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTLNAME), Xml.XmlAttribute).Value)
            Dim v_strIsSignCA As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeSIGNCA), Xml.XmlAttribute).Value)
            Dim v_strPartition As String

            Dim pv_strMessage As String
            If Not (v_attrColl.GetNamedItem(gc_AtributeTXNUM) Is Nothing) Then
                v_strPartition = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTXNUM), Xml.XmlAttribute).Value)
            Else
                v_strPartition = ""
            End If
            v_trace_status = "0"

            v_obj.NewDBInstance(gc_MODULE_INQUERY)
            v_obj.BeginTran()

            v_strObjMsg = pv_xmlDocument.InnerXml()
            'bangpv
            ' Purpose : get from  date T-3 to date T
            Dim v_strT_T_3 As String
            v_strSQL = "select to_char(GET_T_PLUS(to_date('" & v_strCurdate & "', 'dd/mm/yyyy'),'" & v_strBRID & "',-1),'dd/mm/yyyy') txdate from dual" _
                    & " union select to_char(GET_T_PLUS(to_date('" & v_strCurdate & "', 'dd/mm/yyyy'),'" & v_strBRID & "',-2),'dd/mm/yyyy') txdate from dual" _
                    & " union select to_char(GET_T_PLUS(to_date('" & v_strCurdate & "', 'dd/mm/yyyy'),'" & v_strBRID & "',-3),'dd/mm/yyyy') txdate from dual"
            v_t3 = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            v_strT_T_3 = "to_date('" & v_strCurdate & "', 'dd/mm/yyyy'),"
            For i = 0 To v_t3.Tables(0).Rows.Count - 1
                v_strT_T_3 = v_strT_T_3 & "to_date('" & v_t3.Tables(0).Rows(i)("txdate") & "', 'dd/mm/yyyy'),"
            Next
            v_strT_T_3 = "(" & Left(v_strT_T_3, v_strT_T_3.Length - 1) & ")"
            ' end 
            'end bangpv
            v_obj.GetSysVar("SYSTEM", "TRACE_STATUS", v_trace_status)
            Dim v_strDirCurrDate As String
            v_trace_status = "0"
            If v_trace_status = "1" Then
                v_obj.GetSysVar("SYSTEM", "TRACE_PATH", v_trace_path)
                'Dim v_strTLName As String = v_obj.GetValue("TLPROFILES", "TLNAME", "TLID='" & v_strTellerID & "'")

                v_strDirCurrDate = Replace(v_strCurdate, "/", "_")
                If v_trace_path = "" Then
                    Dim v_app As New ApplicationServices.ApplicationBase
                    v_trace_path = v_app.Info.DirectoryPath & "\Log\" & v_strDirCurrDate
                Else
                    v_trace_path = v_trace_path & v_strDirCurrDate
                End If

                If Not System.IO.Directory.Exists(v_trace_path) Then
                    System.IO.Directory.CreateDirectory(v_trace_path)
                End If

                v_trace_path &= "\log_report_br" & v_strBRID & "_" & v_strTLName & ".txt"

                tr2 = New TextWriterTraceListener(System.IO.File.AppendText(v_trace_path))
                Trace.Listeners.Add(tr2)
                Trace.WriteLine("[Bắt đầu: RP - " & v_strRptID & "] " & DateTime.Now & vbCrLf)
            End If

            Dim v_ds, v_ds1 As DataSet
            Dim v_trace As DataSet
            Dim v_lstBrID As String
            Dim v_blnSearch As Boolean = False
            Dim v_blnLoadAll As Boolean = False
            Dim v_strMinDate, v_strMaxDate As String

            Dim v_arrTemp() As String

            Dim v_arrField() As String
            Dim v_strFieldCode As String = ""
            v_arrTemp = v_strClause.Split("#")
            v_strRptID = v_arrTemp(0)
            v_arrField = v_arrTemp(1).Split("$")

            'Xu ly DL bang TRAN, TLLOG
            Dim v_strMainSQL As String
            v_strSQL = "SELECT (RPTCMDSQL || nvl(RPTCMDSQL1,'')) RPTCMDSQL , TLLOGTRAN FROM RPREPORTS WHERE RPTID = '" & v_strRptID & "' AND DELETED=0 AND STATUS=0"
            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

            Dim v_strTran As String = "" 'IA|MA|CA|MF|RA|RG|SF|TLLOG
            Dim v_lstTran() As String

            If v_ds.Tables(0).Rows.Count > 0 Then
                v_strTran = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("TLLOGTRAN")).ToUpper
                v_strMainSQL = v_ds.Tables(0).Rows(0)("RPTCMDSQL")
            End If

            v_strMinDate = v_strCurdate
            v_strMaxDate = v_strCurdate
            If v_strPartition <> "" Then
                v_strMinDate = v_strPartition.Split("|")(0)
                v_strMaxDate = v_strPartition.Split("|")(1)
            End If
            v_blnSearch = CheckPassDate(v_strMaxDate, v_strCurdate)
            v_blnLoadAll = CheckPassDate(v_strMinDate, v_strCurdate)


            If v_strTran <> "" Then
                v_lstTran = v_strTran.Split("|")
                v_strTran = ""
                For i As Integer = 0 To v_lstTran.Length - 1
                    Select Case v_lstTran(i)
                        Case "TLLOG"
                            v_strTran &= ",'TLLOG'"
                            If v_blnLoadAll Then
                                v_strTran &= ",'TLLOGALL'"
                            End If

                        Case "CA", "IA", "MA", "RA", "SF"
                            v_strTran &= ",'" & v_lstTran(i) & "TRAN'"
                            v_strTran &= ",'" & v_lstTran(i) & "MAST'"
                            If v_blnLoadAll Then
                                v_strTran &= ",'" & v_lstTran(i) & "TRANA'"
                            End If
                        Case "RG", "MF", "CS"
                            v_strTran &= ",'" & v_lstTran(i) & "TRAN'"
                            If v_blnLoadAll Then
                                v_strTran &= ",'" & v_lstTran(i) & "TRANA'"
                            End If
                    End Select
                Next
                v_strTran = "(" & Mid(v_strTran, 2) & ")"
            End If

            Dim v_lstSICODE, v_lstMICODE As String

            v_lstMICODE = v_arrFilter.Split("|")(1)
            v_lstSICODE = v_arrFilter.Split("|")(0)

            'Lay phan quyen chi nhanh
            v_strSQL = "SELECT DISTINCT brid brid  FROM " _
                       & " (SELECT b.brid FROM tlbridauth a, brgrp b" _
                       & " WHERE ((AUTHID = '" & v_strTellerID & "' AND authtype = 'U') OR (AUTHID IN (SELECT a.grpid" _
                       & " FROM tlgrpusers a WHERE a.tlid = '" & v_strTellerID & "') AND authtype = 'G'))" _
                       & " And a.brid = b.brid And a.deleted = 0 And a.status = 0" _
                       & " And b.deleted = 0 And b.status = 0" _
                       & " UNION " _
                       & " SELECT b.brid FROM tlprofiles a, brgrp b" _
                       & " WHERE a.brid = b.brid AND a.tlid = '" & v_strTellerID & "'" _
                       & " AND a.deleted = 0 AND a.status = 0 AND b.deleted = 0 AND b.status = 0)"

            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

            v_lstBrID = ""
            For i = 0 To v_ds.Tables(0).Rows.Count - 1
                v_lstBrID &= ",'" & v_ds.Tables(0).Rows(i)(0) & "'"
            Next
            v_lstBrID = "(" & Mid(v_lstBrID, 2) & ")"

            v_strSQL = "INSERT INTO tmp_rgmi " _
                       & " SELECT DISTINCT m.* FROM rgmi m WHERE m.deleted=0 AND m.status=0" _
                       & " AND m.MICODE IN " & v_lstMICODE
            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
            'bangpv: Bỏ lấy chưa lưu ký
            'v_strSQL = "INSERT INTO TMP_RGMI(AUTOID, MICODE) SELECT '0' AUTOID, '000' MICODE FROM DUAL"
            'v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
            'end bangpv
            ''Lay chung khoan dc phan quyen cho user vao bang TT_TMP_RGSI
            'v_strSQL = "INSERT INTO tmp_rgsi(iscode, sicode, stock_name, part_value, TYPE," _
            '            & "stock_type, interest_rate, interest_period, " _
            '            & "bond_period, deleted, exchange_rate, note, " _
            '            & "bond_term, release_series, release_mode, isin, " _
            '            & "rarate, tmpid, paidmethod, npaiddate1, npaiddate2, " _
            '            & "npaiddate3, npaiddate4, int_release_mode, brid, status, issuer_date, due_date) " _
            '            & " SELECT distinct m.iscode, m.sicode, m.stock_name, m.part_value, m.TYPE," _
            '            & "m.stock_type, m.interest_rate, m.interest_period," _
            '            & "m.bond_period, m.deleted, m.exchange_rate, m.note, " _
            '            & "m.bond_term, m.release_series, m.release_mode, m.isin, " _
            '            & "m.rarate, m.tmpid, m.paidmethod, m.npaiddate1, m.npaiddate2," _
            '            & "m.npaiddate3, m.npaiddate4, m.int_release_mode, m.brid, m.status, m.issuer_date, m.due_date FROM rgsi m" _
            '            & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_lstSICODE
            'v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
            'Dim v_strCut, v_strCut1, v_strCut2, v_strCut3, v_strCut4, v_strCut5 As String
            'chia lam 5 khoi

            'Dim v_iCut As Integer
            'v_iCut = (v_lstSICODE.Length \ 5)
            'v_iCut = InStr(v_iCut, v_lstSICODE, ",")
            'If v_iCut > 0 Then
            '    v_strCut1 = v_lstSICODE.Substring(0, v_iCut - 1) & ")"
            '    v_strSQL = "INSERT INTO tmp_rgsi( iscode, sicode, stock_name, part_value, type," _
            '                            & " status, stock_type, interest_rate, interest_period, " _
            '                            & " bond_period, due_date, issuer_date, effective_date, " _
            '                            & " deleted, exchange_date, exchange_rate, note, bond_term, " _
            '                            & "release_series, release_mode, isin, brid, rarate, " _
            '                            & "tmpid, paidmethod, npaiddate1, npaiddate2, npaiddate3, " _
            '                            & "npaiddate4, int_release_mode, currency_code, public_date, " _
            '                            & "stock_id, listed_qtty, bond_period_unit, sbv_sicode, vsdbrid) " _
            '                            & " SELECT distinct m.iscode, m.sicode, m.stock_name, m.part_value, m.type," _
            '                            & "m.status, m.stock_type, m.interest_rate, m.interest_period, " _
            '                            & "m.bond_period, m.due_date, m.issuer_date, m.effective_date, " _
            '                            & "m.deleted, m.exchange_date, m.exchange_rate, m.note, m.bond_term, " _
            '                            & "m.release_series, m.release_mode, m.isin, m.brid, m.rarate, " _
            '                            & "m.tmpid, m.paidmethod, m.npaiddate1, m.npaiddate2, m.npaiddate3, " _
            '                            & "m.npaiddate4, m.int_release_mode, m.currency_code, m.public_date, " _
            '                            & "m.stock_id, m.listed_qtty, m.bond_period_unit, m.sbv_sicode,m.vsdbrid FROM rgsi m" _
            '    & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_strCut1
            '    v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

            '    v_strCut = "(" & v_lstSICODE.Substring(v_iCut)
            '    v_iCut = InStr(v_iCut, v_strCut, ",")
            '    If v_iCut > 0 Then
            '        v_strCut2 = v_strCut.Substring(0, v_iCut - 1) & ")"
            '        v_strSQL = "INSERT INTO tmp_rgsi( iscode, sicode, stock_name, part_value, type," _
            '                            & " status, stock_type, interest_rate, interest_period, " _
            '                            & " bond_period, due_date, issuer_date, effective_date, " _
            '                            & " deleted, exchange_date, exchange_rate, note, bond_term, " _
            '                            & "release_series, release_mode, isin, brid, rarate, " _
            '                            & "tmpid, paidmethod, npaiddate1, npaiddate2, npaiddate3, " _
            '                            & "npaiddate4, int_release_mode, currency_code, public_date, " _
            '                            & "stock_id, listed_qtty, bond_period_unit, sbv_sicode, vsdbrid) " _
            '                            & " SELECT distinct m.iscode, m.sicode, m.stock_name, m.part_value, m.type," _
            '                            & "m.status, m.stock_type, m.interest_rate, m.interest_period, " _
            '                            & "m.bond_period, m.due_date, m.issuer_date, m.effective_date, " _
            '                            & "m.deleted, m.exchange_date, m.exchange_rate, m.note, m.bond_term, " _
            '                            & "m.release_series, m.release_mode, m.isin, m.brid, m.rarate, " _
            '                            & "m.tmpid, m.paidmethod, m.npaiddate1, m.npaiddate2, m.npaiddate3, " _
            '                            & "m.npaiddate4, m.int_release_mode, m.currency_code, m.public_date, " _
            '                            & "m.stock_id, m.listed_qtty, m.bond_period_unit, m.sbv_sicode,m.vsdbrid FROM rgsi m" _
            '              & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_strCut2
            '        v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

            '        v_strCut = "(" & v_strCut.Substring(v_iCut)
            '        v_iCut = InStr(v_iCut, v_strCut, ",")
            '        '3
            '        If v_iCut > 0 Then
            '            v_strCut3 = v_strCut.Substring(0, v_iCut - 1) & ")"
            '            v_strSQL = "INSERT INTO tmp_rgsi( iscode, sicode, stock_name, part_value, type," _
            '                            & " status, stock_type, interest_rate, interest_period, " _
            '                            & " bond_period, due_date, issuer_date, effective_date, " _
            '                            & " deleted, exchange_date, exchange_rate, note, bond_term, " _
            '                            & "release_series, release_mode, isin, brid, rarate, " _
            '                            & "tmpid, paidmethod, npaiddate1, npaiddate2, npaiddate3, " _
            '                            & "npaiddate4, int_release_mode, currency_code, public_date, " _
            '                            & "stock_id, listed_qtty, bond_period_unit, sbv_sicode, vsdbrid) " _
            '                            & " SELECT distinct m.iscode, m.sicode, m.stock_name, m.part_value, m.type," _
            '                            & "m.status, m.stock_type, m.interest_rate, m.interest_period, " _
            '                            & "m.bond_period, m.due_date, m.issuer_date, m.effective_date, " _
            '                            & "m.deleted, m.exchange_date, m.exchange_rate, m.note, m.bond_term, " _
            '                            & "m.release_series, m.release_mode, m.isin, m.brid, m.rarate, " _
            '                            & "m.tmpid, m.paidmethod, m.npaiddate1, m.npaiddate2, m.npaiddate3, " _
            '                            & "m.npaiddate4, m.int_release_mode, m.currency_code, m.public_date, " _
            '                            & "m.stock_id, m.listed_qtty, m.bond_period_unit, m.sbv_sicode,m.vsdbrid FROM rgsi m" _
            '                    & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_strCut3
            '            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

            '            v_strCut = "(" & v_strCut.Substring(v_iCut)
            '            v_iCut = InStr(v_iCut, v_strCut, ",")
            '            If v_iCut > 0 Then
            '                v_strCut4 = v_strCut.Substring(0, v_iCut - 1) & ")"
            '                v_strSQL = "INSERT INTO tmp_rgsi( iscode, sicode, stock_name, part_value, type," _
            '                            & " status, stock_type, interest_rate, interest_period, " _
            '                            & " bond_period, due_date, issuer_date, effective_date, " _
            '                            & " deleted, exchange_date, exchange_rate, note, bond_term, " _
            '                            & "release_series, release_mode, isin, brid, rarate, " _
            '                            & "tmpid, paidmethod, npaiddate1, npaiddate2, npaiddate3, " _
            '                            & "npaiddate4, int_release_mode, currency_code, public_date, " _
            '                            & "stock_id, listed_qtty, bond_period_unit, sbv_sicode, vsdbrid) " _
            '                            & " SELECT distinct m.iscode, m.sicode, m.stock_name, m.part_value, m.type," _
            '                            & "m.status, m.stock_type, m.interest_rate, m.interest_period, " _
            '                            & "m.bond_period, m.due_date, m.issuer_date, m.effective_date, " _
            '                            & "m.deleted, m.exchange_date, m.exchange_rate, m.note, m.bond_term, " _
            '                            & "m.release_series, m.release_mode, m.isin, m.brid, m.rarate, " _
            '                            & "m.tmpid, m.paidmethod, m.npaiddate1, m.npaiddate2, m.npaiddate3, " _
            '                            & "m.npaiddate4, m.int_release_mode, m.currency_code, m.public_date, " _
            '                            & "m.stock_id, m.listed_qtty, m.bond_period_unit, m.sbv_sicode,m.vsdbrid FROM rgsi m" _
            '                        & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_strCut4
            '                v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

            '                v_strCut5 = "(" & v_strCut.Substring(v_iCut)

            '                v_strSQL = "INSERT INTO tmp_rgsi( iscode, sicode, stock_name, part_value, type," _
            '                            & " status, stock_type, interest_rate, interest_period, " _
            '                            & " bond_period, due_date, issuer_date, effective_date, " _
            '                            & " deleted, exchange_date, exchange_rate, note, bond_term, " _
            '                            & "release_series, release_mode, isin, brid, rarate, " _
            '                            & "tmpid, paidmethod, npaiddate1, npaiddate2, npaiddate3, " _
            '                            & "npaiddate4, int_release_mode, currency_code, public_date, " _
            '                            & "stock_id, listed_qtty, bond_period_unit, sbv_sicode, vsdbrid) " _
            '                            & " SELECT distinct m.iscode, m.sicode, m.stock_name, m.part_value, m.type," _
            '                            & "m.status, m.stock_type, m.interest_rate, m.interest_period, " _
            '                            & "m.bond_period, m.due_date, m.issuer_date, m.effective_date, " _
            '                            & "m.deleted, m.exchange_date, m.exchange_rate, m.note, m.bond_term, " _
            '                            & "m.release_series, m.release_mode, m.isin, m.brid, m.rarate, " _
            '                            & "m.tmpid, m.paidmethod, m.npaiddate1, m.npaiddate2, m.npaiddate3, " _
            '                            & "m.npaiddate4, m.int_release_mode, m.currency_code, m.public_date, " _
            '                            & "m.stock_id, m.listed_qtty, m.bond_period_unit, m.sbv_sicode,m.vsdbrid FROM rgsi m" _
            '                        & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_strCut5
            '                v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

            '            Else
            '                v_strSQL = "INSERT INTO tmp_rgsi( iscode, sicode, stock_name, part_value, type," _
            '                            & " status, stock_type, interest_rate, interest_period, " _
            '                            & " bond_period, due_date, issuer_date, effective_date, " _
            '                            & " deleted, exchange_date, exchange_rate, note, bond_term, " _
            '                            & "release_series, release_mode, isin, brid, rarate, " _
            '                            & "tmpid, paidmethod, npaiddate1, npaiddate2, npaiddate3, " _
            '                            & "npaiddate4, int_release_mode, currency_code, public_date, " _
            '                            & "stock_id, listed_qtty, bond_period_unit, sbv_sicode, vsdbrid) " _
            '                            & " SELECT distinct m.iscode, m.sicode, m.stock_name, m.part_value, m.type," _
            '                            & "m.status, m.stock_type, m.interest_rate, m.interest_period, " _
            '                            & "m.bond_period, m.due_date, m.issuer_date, m.effective_date, " _
            '                            & "m.deleted, m.exchange_date, m.exchange_rate, m.note, m.bond_term, " _
            '                            & "m.release_series, m.release_mode, m.isin, m.brid, m.rarate, " _
            '                            & "m.tmpid, m.paidmethod, m.npaiddate1, m.npaiddate2, m.npaiddate3, " _
            '                            & "m.npaiddate4, m.int_release_mode, m.currency_code, m.public_date, " _
            '                            & "m.stock_id, m.listed_qtty, m.bond_period_unit, m.sbv_sicode,m.vsdbrid FROM rgsi m" _
            '                          & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_strCut
            '                v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
            '            End If
            '        End If
            '    End If
            'Else
            '    v_strSQL = "INSERT INTO tmp_rgsi( iscode, sicode, stock_name, part_value, type," _
            '                            & " status, stock_type, interest_rate, interest_period, " _
            '                            & " bond_period, due_date, issuer_date, effective_date, " _
            '                            & " deleted, exchange_date, exchange_rate, note, bond_term, " _
            '                            & "release_series, release_mode, isin, brid, rarate, " _
            '                            & "tmpid, paidmethod, npaiddate1, npaiddate2, npaiddate3, " _
            '                            & "npaiddate4, int_release_mode, currency_code, public_date, " _
            '                            & "stock_id, listed_qtty, bond_period_unit, sbv_sicode, vsdbrid) " _
            '                            & " SELECT distinct m.iscode, m.sicode, m.stock_name, m.part_value, m.type," _
            '                            & "m.status, m.stock_type, m.interest_rate, m.interest_period, " _
            '                            & "m.bond_period, m.due_date, m.issuer_date, m.effective_date, " _
            '                            & "m.deleted, m.exchange_date, m.exchange_rate, m.note, m.bond_term, " _
            '                            & "m.release_series, m.release_mode, m.isin, m.brid, m.rarate, " _
            '                            & "m.tmpid, m.paidmethod, m.npaiddate1, m.npaiddate2, m.npaiddate3, " _
            '                            & "m.npaiddate4, m.int_release_mode, m.currency_code, m.public_date, " _
            '                            & "m.stock_id, m.listed_qtty, m.bond_period_unit, m.sbv_sicode,m.vsdbrid FROM rgsi m" _
            '                  & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_lstSICODE
            '    v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

            'End If
            'HaNM5-28/10/2020-Sua loi tren 1000 ma
            Dim v_arrSICODE() As String = v_lstSICODE.Replace("(", "").Replace(")", "").Split(",")
            Dim v_intStart As Integer = 0
            Dim v_intEnd As Integer = 0
            Dim v_intOffSet As Integer = 990
            Dim v_strSiCodeList As String = ""
            Dim v_intSiCodeCount As Integer = v_arrSICODE.Length
            While v_intStart < v_intSiCodeCount
                v_strSiCodeList = ""
                If v_intStart + v_intOffSet >= v_intSiCodeCount Then
                    v_intEnd = v_intSiCodeCount - 1
                Else
                    v_intEnd = v_intStart + v_intOffSet - 1
                End If
                For i As Integer = v_intStart To v_intEnd
                    v_strSiCodeList = v_strSiCodeList & "," & v_arrSICODE(i)
                Next
                If v_strSiCodeList.Length > 0 Then
                    v_strSiCodeList = "(" & v_strSiCodeList.Substring(1) & ")"
                    v_strSQL = "INSERT INTO tmp_rgsi(iscode, sicode, stock_name, part_value, TYPE," _
                                      & "stock_type, interest_rate, interest_period, " _
                                      & "bond_period, deleted, exchange_rate, note, " _
                                      & "bond_term, release_series, release_mode, isin, " _
                                      & "rarate, tmpid, paidmethod, npaiddate1, npaiddate2, " _
                                      & "npaiddate3, npaiddate4, int_release_mode, brid, status, public_date, currency_code,vsdbrid) " _
                                      & " SELECT distinct m.iscode, m.sicode, m.stock_name, m.part_value, m.TYPE," _
                                      & "m.stock_type, m.interest_rate, m.interest_period," _
                                      & "m.bond_period, m.deleted, m.exchange_rate, m.note, " _
                                      & "m.bond_term, m.release_series, m.release_mode, m.isin, " _
                                      & "m.rarate, m.tmpid, m.paidmethod, m.npaiddate1, m.npaiddate2," _
                                      & "m.npaiddate3, m.npaiddate4, m.int_release_mode, m.brid, m.status, m.public_date, m.currency_code,m.vsdbrid FROM rgsi m" _
                                      & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_strSiCodeList
                    v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                End If
                v_intStart = v_intStart + v_intOffSet
            End While
            'End HaNM5 sua
            ' tuanta
            v_strSQL = "INSERT INTO TMP_RGSI(AUTOID, SICODE) SELECT 0 AUTOID, '000' SICODE FROM DUAL"
            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

            'Lay cau xu ly chung
            Dim v_hSQL As New Hashtable
            Dim v_hORD As New Hashtable
            If v_strTran <> "" Then
                v_strSQL = "SELECT * FROM RPSTORES WHERE RPTID IS NULL AND OVERWRITE IN " & v_strTran & " ORDER BY ODRNUM"

                v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                Dim v_strOverWrite As String
                If v_ds.Tables(0).Rows.Count > 0 Then
                    For i As Integer = 0 To v_ds.Tables(0).Rows.Count - 1
                        v_strOverWrite = v_ds.Tables(0).Rows(i)("OVERWRITE")
                        v_hORD(i) = v_strOverWrite
                        v_hSQL(v_strOverWrite) = v_ds.Tables(0).Rows(i)("TRACE") & v_ds.Tables(0).Rows(i)("CMDSQL") & " " & v_ds.Tables(0).Rows(i)("CMDSQL1")
                    Next
                End If

                'Lay cau xu ly chung da dc viet lai
                v_strSQL = "SELECT * FROM RPSTORES WHERE RPTID='" & v_strRptID & "' AND OVERWRITE IN " & v_strTran & " ORDER BY ODRNUM"
                v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

                If v_ds.Tables(0).Rows.Count > 0 Then
                    For i As Integer = 0 To v_ds.Tables(0).Rows.Count - 1
                        v_strOverWrite = v_ds.Tables(0).Rows(i)("OVERWRITE")
                        If Not v_hSQL(v_strOverWrite) Is Nothing Then
                            v_hSQL(v_strOverWrite) = v_ds.Tables(0).Rows(i)("TRACE") & v_ds.Tables(0).Rows(i)("CMDSQL") & " " & v_ds.Tables(0).Rows(i)("CMDSQL1")
                        End If
                    Next
                End If

                'Thuc hien cau xu ly chung
                For i As Integer = 0 To v_hORD.Count - 1
                    v_strOverWrite = v_hORD(i)
                    v_strSQL = Mid(v_hSQL(v_strOverWrite), 2)
                    'v_strPartitionSQL = ""

                    v_strSQL = Replace(v_strSQL, "[!BRID]", v_strBRID)
                    v_strSQL = Replace(v_strSQL, "[!TLID]", v_strTellerID)
                    v_strSQL = Replace(v_strSQL, "[!LBRID]", v_lstBrID)
                    v_strSQL = Replace(v_strSQL, "[!T_T_3]", v_strT_T_3)
                    v_strSQL = Replace(v_strSQL, "[!MINDATE]", "TO_DATE('" & v_strMinDate & "','dd/mm/yyyy')")
                    v_strSQL = Replace(v_strSQL, "[!MAXDATE]", "TO_DATE('" & v_strMaxDate & "','dd/mm/yyyy')")
                    'BangPV: Bỏ tt_ do ko dùng timesten nữa
                    v_strSQL = Replace(v_strSQL, "TT_", "")
                    v_strSQL = Replace(v_strSQL, "tt_", "")
                    v_strSQL = Replace(v_strSQL, "tT_", "")
                    v_strSQL = Replace(v_strSQL, "Tt_", "")
                    'end BangPV
                    v_strSQL = Replace(v_strSQL, "[!TLLOGALL]", "TLLOGALL_ALL")
                    v_strSQL = Replace(v_strSQL, "[!CATRANA]", "CATRANA_ALL")
                    v_strSQL = Replace(v_strSQL, "[!IATRANA]", "IATRANA_ALL")
                    v_strSQL = Replace(v_strSQL, "[!MATRANA]", "MATRANA_ALL")
                    v_strSQL = Replace(v_strSQL, "[!SFTRANA]", "SFTRANA_ALL")
                    v_strSQL = Replace(v_strSQL, "[!MFTRANA]", "MFTRANA_ALL")
                    v_strSQL = Replace(v_strSQL, "[!RATRANA]", "RATRANA_ALL")
                    v_strSQL = Replace(v_strSQL, "[!RGTRANA]", "RGTRANA_ALL")

                    For l As Integer = 0 To v_arrField.Count - 2
                        v_strSQL = Replace(v_strSQL, "[!" & v_arrField(l).Split("|")(0) & "]", v_arrField(l).Split("|")(1))

                    Next

                    If v_trace_status = "1" And Mid(v_hSQL(v_strOverWrite), 1, 1) = "1" Then
                        Trace.WriteLine("[RP] " & DateTime.Now & " :" & vbCrLf & " -o- Câu lệnh thứ xử lý chung thứ #" & i + 1 & "-o-" & vbCrLf & v_strSQL & vbCrLf)
                        If InStr(v_strSQL.ToUpper, "SELECT") > 0 Then
                            Trace.WriteLine("-o- Dữ liệu câu lệnh xử lý chung thứ #" & i + 1 & "-o-")
                            v_strSQLTmp = v_strSQL.Substring(InStr(v_strSQL.ToUpper, "SELECT") - 1)
                            v_trace = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                            Trace.WriteLine(v_trace.GetXml & vbCrLf)
                        End If
                    End If
                    'v_obj.ExecuteNonQuery(CommandType.Text, v_strPartitionSQL)
                    v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                Next
            End If

            v_strSQL = "SELECT * FROM RPSTORES WHERE RPTID='" & v_strRptID & "' AND OVERWRITE IS NULL AND DELETED=0 AND STATUS=0 ORDER BY ODRNUM"
            v_ds1 = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

            Dim v_int As Integer

            For v_int = 0 To v_ds1.Tables(0).Rows.Count - 1

                v_strSQL = v_ds1.Tables(0).Rows(v_int)("CMDSQL").ToString & " " & v_ds1.Tables(0).Rows(v_int)("CMDSQL1").ToString

                'If Not CheckTranTable(v_strSQL) Then
                '    v_lngErr = 1
                '    Exit For
                'End If

                For i As Integer = 0 To v_arrField.Count - 2
                    v_strSQL = Replace(v_strSQL, "[!" & v_arrField(i).Split("|")(0) & "]", v_arrField(i).Split("|")(1))
                Next
                ' purpose : phi luu ky
                If InStr(v_strSQL, "[!MFNO_01]", CompareMethod.Text) Then
                    v_strSQLTmp = "select fomula from mftype where deleted=0 and status=0 and mfno='01'"
                    v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                    If v_ds.Tables(0).Rows.Count > 0 Then
                        v_strSQL = Replace(v_strSQL, "[!MFNO_01]", v_ds.Tables(0).Rows(0)(0))
                    Else
                        v_strSQL = Replace(v_strSQL, "[!MFNO_01]", "0")
                    End If
                End If
                ' end phi luu ky
                v_strSQL = Replace(v_strSQL, "[!BRID]", v_strBRID)
                v_strSQL = Replace(v_strSQL, "[!TLID]", v_strTellerID)
                v_strSQL = Replace(v_strSQL, "[!LBRID]", v_lstBrID)
                v_strSQL = Replace(v_strSQL, "[!T_T_3]", v_strT_T_3)
                v_strSQL = Replace(v_strSQL, "[!MINDATE]", "TO_DATE('" & v_strMinDate & "','dd/mm/yyyy')")
                v_strSQL = Replace(v_strSQL, "[!MAXDATE]", "TO_DATE('" & v_strMaxDate & "','dd/mm/yyyy')")

                'BangPV: Bỏ tt_ do ko dùng timesten nữa
                v_strSQL = Replace(v_strSQL, "TT_", "")
                v_strSQL = Replace(v_strSQL, "tt_", "")
                v_strSQL = Replace(v_strSQL, "tT_", "")
                v_strSQL = Replace(v_strSQL, "Tt_", "")
                'end bangpv

                If v_trace_status = "1" And v_ds1.Tables(0).Rows(v_int)("TRACE") = 1 Then
                    Trace.WriteLine("[RP] " & DateTime.Now & " :" & vbCrLf & " -o- Câu lệnh thứ #" & v_ds1.Tables(0).Rows(v_int)("ODRNUM") & "-o-" & vbCrLf & v_strSQL & vbCrLf)
                    If InStr(v_strSQL.ToUpper, "SELECT") > 0 Then
                        Trace.WriteLine("-o- Dữ liệu câu lệnh thứ #" & v_ds1.Tables(0).Rows(v_int)("ODRNUM") & "-o-")
                        v_strSQLTmp = v_strSQL.Substring(InStr(v_strSQL.ToUpper, "SELECT") - 1)
                        v_trace = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                        Trace.WriteLine(v_trace.GetXml & vbCrLf)
                    End If
                End If

                v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

            Next

            If v_lngErr = ERR_SYSTEM_OK Then
                v_strSQL = v_strMainSQL

                'If Not CheckTranTable(v_strSQL) Then
                '    v_lngErr = 1
                'Else
                '    For i As Integer = 0 To v_arrField.Count - 2
                '        v_strSQL = Replace(v_strSQL, "[!" & v_arrField(i).Split("|")(0) & "]", v_arrField(i).Split("|")(1))
                '    Next

                '    If v_strSQL <> "" Then
                '        v_strSQL = Replace(v_strSQL, "[!BRID]", v_strBRID)
                '        v_strSQL = Replace(v_strSQL, "[!TLID]", v_strTellerID)
                '        v_strSQL = Replace(v_strSQL, "[!LBRID]", v_lstBrID)
                '        v_strSQL = Replace(v_strSQL, "[!MINDATE]", "TO_DATE('" & v_strMinDate & "','dd/mm/yyyy')")
                '        v_strSQL = Replace(v_strSQL, "[!MAXDATE]", "TO_DATE('" & v_strMaxDate & "','dd/mm/yyyy')")

                '        v_strSQL = Replace(v_strSQL, "TT_", "")
                '        v_strSQL = Replace(v_strSQL, "tt_", "")
                '        v_strSQL = Replace(v_strSQL, "tT_", "")
                '        v_strSQL = Replace(v_strSQL, "Tt_", "")

                '        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                '        If v_trace_status = "1" Then
                '            Trace.WriteLine("[RP] " & DateTime.Now & " :" & vbCrLf & " -o- Ket qua:-o-" & vbCrLf & v_strSQL & vbCrLf)
                '            'Trace.WriteLine(v_ds.GetXml & vbCrLf)
                '        End If
                '    End If
                'End If

                For i As Integer = 0 To v_arrField.Count - 2
                    v_strSQL = Replace(v_strSQL, "[!" & v_arrField(i).Split("|")(0) & "]", v_arrField(i).Split("|")(1))
                Next
                ' purpose : phi luu ky
                If InStr(v_strSQL, "[!MFNO_01]", CompareMethod.Text) Then
                    v_strSQLTmp = "select fomula from mftype where deleted=0 and status=0 and mfno='01'"
                    v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                    If v_ds.Tables(0).Rows.Count > 0 Then
                        v_strSQL = Replace(v_strSQL, "[!MFNO_01]", v_ds.Tables(0).Rows(0)(0))
                    Else
                        v_strSQL = Replace(v_strSQL, "[!MFNO_01]", "0")
                    End If
                End If
                ' end phi luu ky
                If v_strSQL <> "" Then
                    v_strSQL = Replace(v_strSQL, "[!BRID]", v_strBRID)
                    v_strSQL = Replace(v_strSQL, "[!TLID]", v_strTellerID)
                    v_strSQL = Replace(v_strSQL, "[!LBRID]", v_lstBrID)
                    v_strSQL = Replace(v_strSQL, "[!T_T_3]", v_strT_T_3)
                    v_strSQL = Replace(v_strSQL, "[!MINDATE]", "TO_DATE('" & v_strMinDate & "','dd/mm/yyyy')")
                    v_strSQL = Replace(v_strSQL, "[!MAXDATE]", "TO_DATE('" & v_strMaxDate & "','dd/mm/yyyy')")

                    v_strSQL = Replace(v_strSQL, "TT_", "")
                    v_strSQL = Replace(v_strSQL, "tt_", "")
                    v_strSQL = Replace(v_strSQL, "tT_", "")
                    v_strSQL = Replace(v_strSQL, "Tt_", "")

                    v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                    If v_trace_status = "1" Then
                        Trace.WriteLine("[RP] " & DateTime.Now & " :" & vbCrLf & " -o- Ket qua:-o-" & vbCrLf & v_strSQL & vbCrLf)
                        'Trace.WriteLine(v_ds.GetXml & vbCrLf)
                    End If
                End If

            End If

            v_obj.Commit()

            'cập nhật vào tlsession 
            'If v_strIsSignCA = "1" Then
            '    pv_strMessage = pv_xmlDocument.InnerXml
            '    'Dim v_array() As Byte = modCommond.Compression(pv_strMessage)

            '    'pv_strMessage = ByteArrayToStr(v_array)
            '    InsertTLSession(Replace(pv_strMessage, "'", "''"), v_strTLName, v_strTellerID, v_strRptID, v_strBRID, v_strCurdate)
            '    'v_array = StrToByteArray(pv_strMessage)
            '    'pv_strMessage = Decompress(v_array)

            'End If
            Return v_ds

        Catch ex As Exception
            Throw ex
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                & "Error code: System error!" & vbNewLine _
                & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_HOST)
            v_obj.Commit()
        Finally
            If v_trace_status = "1" Then
                Trace.WriteLine("[Kết thúc: RP - " & v_strRptID & "] " & DateTime.Now & vbCrLf)
                tr2.Close()
                tr2.Dispose()
            End If
            v_obj.Dispose()
            GC.Collect()
        End Try
    End Function
    'Public Function GetReports(ByRef pv_xmlDocument As XmlDocumentEx) As Byte()
    '    Try
    '        Dim v_ds As DataSet
    '        Dim v_objPageSetting As New ReportSetting
    '        Dim v_strSQL As String
    '        Dim v_obj As New DataAccess

    '        Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes
    '        Dim v_strObjMsg As String
    '        Dim v_strClause As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCLAUSE), Xml.XmlAttribute).Value)
    '        Dim v_strTellerID As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTLID), Xml.XmlAttribute).Value)
    '        Dim v_arrFilter As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCMDINQUIRY), Xml.XmlAttribute).Value)
    '        Dim v_strBRID As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeBRID), Xml.XmlAttribute).Value)
    '        Dim v_strCurdate As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTXDATE), Xml.XmlAttribute).Value)
    '        'Dim v_strBUSDATE As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeBUSDATE), Xml.XmlAttribute).Value)
    '        Dim v_strUserLang As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeLANGUAGE), Xml.XmlAttribute).Value)
    '        Dim v_strRptID As String

    '        Dim v_arrField() As String
    '        Dim v_strFieldCode As String = ""
    '        Dim v_arrTemp() As String
    '        v_arrTemp = v_strClause.Split("#")
    '        v_strRptID = v_arrTemp(0)
    '        v_arrField = v_arrTemp(1).Split("$")

    '        v_obj.NewDBInstance(gc_MODULE_INQUERY)

    '        v_strSQL = "SELECT * FROM RPREPORTS WHERE RPTID='" & v_strRptID & "'"
    '        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

    '        Dim i As Integer
    '        For i = 0 To v_ds.Tables(0).Rows.Count - 1
    '            With v_ds.Tables(0)
    '                v_objPageSetting.Title = .Rows(i)("RPTTITLE")
    '                v_objPageSetting.En_Title = .Rows(i)("EN_RPTTITLE")
    '                v_objPageSetting.ReportType = .Rows(i)("RPTTYPE")
    '                v_objPageSetting.Orientation = .Rows(i)("ORIENTATION")
    '                v_objPageSetting.DSN = .Rows(i)("DSN")
    '                v_objPageSetting.ObjectName = .Rows(i)("OBJNAME")
    '                v_objPageSetting.HeaderHeight = CDbl(.Rows(i)("TITLE_HEIGHT"))
    '                v_objPageSetting.HHeight = CDbl(.Rows(i)("HEADER_HEIGHT"))
    '                v_objPageSetting.FHeight = CDbl(.Rows(i)("FOOTER_HEIGHT"))
    '                If gf_CorrectStringField(.Rows(i)("RPFONTSIZE")) = "" Then
    '                    v_objPageSetting.FontSize = "9.75"
    '                Else
    '                    v_objPageSetting.FontSize = .Rows(i)("RPFONTSIZE")
    '                End If

    '                If gf_CorrectStringField(.Rows(i)("RPPAPERSIZE")) = "" Then
    '                    v_objPageSetting.PaperSize = "A4"
    '                Else
    '                    v_objPageSetting.PaperSize = .Rows(i)("RPPAPERSIZE")
    '                End If
    '                v_objPageSetting.BusDate = v_strCurdate
    '            End With
    '        Next
    '        v_objPageSetting.UserLanguage = v_strUserLang
    '        'v_objPageSetting.ReportID = Replace(Trim(CType(SearchGrid.CurrentRow, Xceed.Grid.DataRow).Cells(KeyColumn).Value), ".", String.Empty)

    '        'Lay cac truong bao cao
    '        v_strSQL = "SELECT * FROM RPFLD WHERE RPTID='" & v_strRptID & "' ORDER BY POSITION"
    '        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
    '        Dim v_objTitleHeader As ReportHeaderRow
    '        Dim v_arrTitleHeader(v_ds.Tables(0).Rows.Count - 1) As ReportHeaderRow

    '        For v_int As Integer = 0 To v_ds.Tables(0).Rows.Count - 1
    '            v_objTitleHeader = New ReportHeaderRow
    '            With v_ds.Tables(0)
    '                v_objTitleHeader.ID = .Rows(v_int)("AUTOID")
    '                v_objTitleHeader.ParentID = .Rows(v_int)("PARENT_ID")
    '                v_objTitleHeader.FieldName = gf_CorrectStringField(.Rows(v_int)("FIELDNAME"))
    '                v_objTitleHeader.FieldType = gf_CorrectStringField(.Rows(v_int)("FIELDTYPE"))
    '                v_objTitleHeader.Caption = gf_CorrectStringField(.Rows(v_int)("CAPTION"))
    '                v_objTitleHeader.En_Caption = gf_CorrectStringField(.Rows(v_int)("EN_CAPTION"))
    '                v_objTitleHeader.Format = gf_CorrectStringField(.Rows(v_int)("FORMAT"))
    '                v_objTitleHeader.Display = gf_CorrectStringField(.Rows(v_int)("DISPLAY"))
    '                v_objTitleHeader.Width = CDbl(.Rows(v_int)("WIDTH"))
    '                v_objTitleHeader.IsDataField = gf_CorrectStringField(.Rows(v_int)("ISDATAFIELD"))
    '                v_objTitleHeader.IsSum = gf_CorrectStringField(.Rows(v_int)("ISSUM"))
    '                v_objTitleHeader.IsParent = gf_CorrectStringField(.Rows(v_int)("ISPARENT"))
    '                v_objTitleHeader.TextAlign = gf_CorrectStringField(.Rows(v_int)("ALIGN"))
    '                v_objTitleHeader.Lev = .Rows(v_int)("LEV")
    '                v_objTitleHeader.Height = CDbl(.Rows(v_int)("HEIGHT"))
    '            End With
    '            v_arrTitleHeader(v_int) = v_objTitleHeader
    '        Next

    '        v_objPageSetting.ReportFld = v_arrTitleHeader

    '        'Lay group bao cao
    '        v_strSQL = "SELECT * FROM RPGRP WHERE RPTID='" & v_strRptID & "' ORDER BY POSITION"
    '        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
    '        If v_ds.Tables(0).Rows.Count > 0 Then
    '            Dim v_objGroup As ReportGroup
    '            Dim v_arrGroup(v_ds.Tables(0).Rows.Count - 1) As ReportGroup

    '            For v_int As Integer = 0 To v_ds.Tables(0).Rows.Count - 1
    '                v_objGroup = New ReportGroup
    '                With v_ds.Tables(0)
    '                    v_objGroup.FieldName = gf_CorrectStringField(.Rows(v_int)("FIELDNAME"))
    '                    v_objGroup.FldType = gf_CorrectStringField(.Rows(v_int)("FIELDTYPE"))
    '                    v_objGroup.Caption = gf_CorrectStringField(.Rows(v_int)("CATION"))
    '                    v_objGroup.En_Caption = gf_CorrectStringField(.Rows(v_int)("EN_CATION"))
    '                    v_objGroup.Format = gf_CorrectStringField(.Rows(v_int)("FORMAT"))
    '                    v_objGroup.CaptionWidth = CDbl(.Rows(v_int)("WIDTH"))
    '                    v_objGroup.Footer = gf_CorrectStringField(.Rows(v_int)("GRPFOOTER"))
    '                End With
    '                v_arrGroup(v_int) = v_objGroup
    '            Next
    '            v_objPageSetting.ReportGrp = v_arrGroup
    '        End If

    '        v_obj.Dispose()
    '        v_obj = Nothing

    '        v_ds = FillData(pv_xmlDocument)

    '        Dim v_rpt As New rptReports(v_objPageSetting, v_ds)
    '        v_rpt.DataSource = v_ds
    '        v_rpt.DataMember = v_ds.Tables(0).TableName
    '        v_rpt.SetLicense("RGN,RGN Warez Group,DD-APN-30-C01339,944SHS949SWOM49HSHSF")
    '        v_rpt.Run(True)

    '        Return v_rpt.Document.Content
    '        ContextUtil.SetComplete()
    '    Catch ex As Exception
    '        ContextUtil.SetAbort()
    '        Throw ex
    '    End Try
    'End Function
    '20230219 Add for export PDF on server 
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

    Public Function InitExportReport(ByVal pv_ds As DataSet, ByVal v_strTLName As String, _
                               ByVal v_strRptID As String, ByVal v_strSicode As String, _
                               ByVal v_strCAKey As String, ByVal v_strClause As String, _
                               ByVal v_strFile As String) As Long
        Try
            Dim v_objPageSetting As New ReportSetting
            Dim v_strsql As String
            Dim v_obj As New DataAccess
            v_objPageSetting.ReportID = v_strRptID
            Dim v_StrBusdate As String
            Dim v_ds As DataSet
            Dim v_lngErrCode = v_obj.GetSysVar("SYSTEM", "CURRDATE", v_StrBusdate)
            FillFormuleToDataSet(v_strClause)
            FillDataSet(pv_ds, v_strRptID)
            ' v_strsql = "SELECT * FROM RPREPORTS WHERE RPTID='" & pv_strRptID & "'"
            v_strsql = "SELECT     rpttitle, en_rpttitle, rptcmdsql, objname, orderbycmdsql, rpttype, orientation, dsn, createby, createdate, modcode, rptid,to_number( title_height) AS title_height, " _
           & "to_number(header_height) AS header_height,to_number(footer_height) AS footer_height, " _
           & " autoid, deleted, status, tllogtran, rpfontsize, rppapersize, ismember, rptcmdsql1, rptlimit,to_number( datarowheight) AS datarowheight, issignca" _
           & " FROM rpreports WHERE RPTID='" & v_strRptID & "'"

            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strsql)
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
                    v_objPageSetting.BusDate = v_StrBusdate

                    v_intIsMember = .Rows(0)("ISMEMBER")
                End With
            Next
            v_objPageSetting.UserLanguage = gc_LANG_VIETNAMESE
            'v_objPageSetting.ReportID = Replace(Trim(CType(SearchGrid.CurrentRow, Xceed.Grid.DataRow).Cells(KeyColumn).Value), ".", String.Empty)

            If v_intIsMember = 0 Then
                v_strsql = "SELECT * FROM BRGRP WHERE BRID='0001'"
                v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strsql)

                v_objPageSetting.CompanyName = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("CompanyName")).ToUpper
                v_objPageSetting.Address = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("Address"))
                v_objPageSetting.BranchName = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("BranchName")).ToUpper
                v_objPageSetting.WhereDate = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("WhereDate"))
            Else
                v_strsql = "SELECT * FROM RGMI WHERE MICODE<>'000'"
                v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strsql)

                v_objPageSetting.MIName = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("NAME")).ToUpper
                v_objPageSetting.MIAddress = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("ADDRESS"))
                v_objPageSetting.BUSINESS_NO = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("BUSINESS_NO"))
                v_objPageSetting.MIPhone = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("PHONE"))
            End If

            'Lay cac truong bao cao
            'v_strsql = "SELECT * FROM RPFLD WHERE RPTID='" & pv_strRptID & "' ORDER BY POSITION"
            v_strsql = "SELECT AUTOID,PARENT_ID,FIELDNAME,FIELDTYPE,CAPTION,EN_CAPTION,FORMAT,DISPLAY, " _
                  & "to_number( WIDTH) AS WIDTH,ISDATAFIELD,ISSUM,ISPARENT,ALIGN,LEV,to_number(HEIGHT) AS HEIGHT FROM RPFLD WHERE RPTID='" & v_objPageSetting.ReportID & "' ORDER BY POSITION"
            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strsql)
            Dim v_objTitleHeader As ReportHeaderRow
            Dim v_arrTitleHeader(v_ds.Tables(0).Rows.Count - 1) As ReportHeaderRow

            For v_int As Integer = 0 To v_ds.Tables(0).Rows.Count - 1
                v_objTitleHeader = New ReportHeaderRow
                With v_ds.Tables(0)
                    v_objTitleHeader.ID = .Rows(v_int)("AUTOID")
                    v_objTitleHeader.ParentID = .Rows(v_int)("PARENT_ID")
                    v_objTitleHeader.FieldName = IIf(IsDBNull(.Rows(v_int)("FIELDNAME")), "", .Rows(v_int)("FIELDNAME"))
                    v_objTitleHeader.FieldType = .Rows(v_int)("FIELDTYPE")
                    v_objTitleHeader.Caption = .Rows(v_int)("CAPTION")
                    v_objTitleHeader.En_Caption = "" ' .Rows(v_int)("EN_CAPTION")
                    v_objTitleHeader.Format = IIf(IsDBNull(.Rows(v_int)("FORMAT")), "", .Rows(v_int)("FORMAT"))
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
            v_strsql = "SELECT * FROM RPGRP WHERE RPTID='" & v_strRptID & "' ORDER BY POSITION"
            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strsql)
            If v_ds.Tables(0).Rows.Count > 0 Then
                Dim v_objGroup As ReportGroup
                Dim v_arrGroup(v_ds.Tables(0).Rows.Count - 1) As ReportGroup

                For v_int As Integer = 0 To v_ds.Tables(0).Rows.Count - 1
                    v_objGroup = New ReportGroup
                    With v_ds.Tables(0)
                        v_objGroup.FieldName = .Rows(v_int)("FIELDNAME")
                        v_objGroup.FldType = .Rows(v_int)("FIELDTYPE")
                        v_objGroup.Caption = .Rows(v_int)("CATION")
                        v_objGroup.En_Caption = IIf(IsDBNull(.Rows(v_int)("EN_CATION")), "", .Rows(v_int)("EN_CATION"))
                        v_objGroup.Format = IIf(IsDBNull(.Rows(v_int)("FORMAT")), "", .Rows(v_int)("FORMAT"))
                        v_objGroup.CaptionWidth = CDbl(.Rows(v_int)("WIDTH"))
                        v_objGroup.Footer = .Rows(v_int)("GRPFOOTER")
                    End With
                    v_arrGroup(v_int) = v_objGroup
                Next
                v_objPageSetting.ReportGrp = v_arrGroup
            End If

            v_obj.Dispose()
            v_obj = Nothing
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("vi-VN")
            Dim v_oRpt As DataDynamics.ActiveReports.ActiveReport3 = Nothing
            Select Case v_objPageSetting.ReportType
                Case 1
                    v_oRpt = New rptReports(v_objPageSetting, mv_DataSet)
                Case 2
                    v_oRpt = New rptReportType2(v_objPageSetting, mv_DataSet)
                    'v_oRpt = New rptTextReport(mv_ReportSetting)
                Case 3
                    v_oRpt = New rptReportType3(v_objPageSetting, mv_DataSet)


                Case 4
                    v_oRpt = New rptReportType4(v_objPageSetting, mv_DataSet)

            End Select

            Dim v_oExport As New Object
            v_oExport = New PdfExport
            v_strFile = v_strFile & ".pdf"
            Select v_objPageSetting.ReportType
                Case 1
                    v_oRpt = New rptReportsExport(v_objPageSetting, mv_DataSet)
                    v_oRpt.SetLicense("RGN,RGN Warez Group,DD-APN-30-C01339,944SHS949SWOM49HSHSF")

                    If Not ClientDataSet Is Nothing Then
                        If ClientDataSet.Tables.IndexOf(v_objPageSetting.ReportID) >= 0 Then
                            v_oRpt.DataSource = ClientDataSet 'GetDataSet()
                            v_oRpt.DataMember = v_objPageSetting.ReportID '"RptData"
                        End If
                    End If
                    v_oRpt.Run(True)

                    v_oExport.Export(v_oRpt.Document, v_strFile)
                    v_oRpt.Dispose()
                Case 3
                    v_oRpt = New rptReportType3Export(v_objPageSetting, mv_DataSet)
                    v_oRpt.SetLicense("RGN,RGN Warez Group,DD-APN-30-C01339,944SHS949SWOM49HSHSF")

                    If Not ClientDataSet Is Nothing Then
                        If ClientDataSet.Tables.IndexOf(v_objPageSetting.ReportID) >= 0 Then
                            v_oRpt.DataSource = ClientDataSet 'GetDataSet()
                            v_oRpt.DataMember = v_objPageSetting.ReportID '"RptData"
                        End If
                    End If
                    v_oRpt.Run(True)

                    v_oExport.Export(v_oRpt.Document, v_strFile)

                    v_oRpt.Dispose()
            End Select


        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                                     & "Error code: System error!" & vbNewLine _
                                                     & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            'MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
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

    'End 20230219 Add for export PDF on server 

    Public Function SaveRPTFile(ByVal pv_ds As DataSet, ByVal v_strTLName As String, _
                               ByVal v_strRptID As String, ByVal v_strSicode As String, _
                               ByVal v_strCAKey As String, ByVal v_strClause As String) As Long
        Dim v_oWriter As System.IO.StreamWriter
        Dim v_DataAccess As New DataAccess
        Dim v_strLocalDir As String
        Dim v_strFileName As String
        Dim v_zipEngine As New ZipEngine
        Try
            Dim v_strAppPath As String = System.AppDomain.CurrentDomain.BaseDirectory
            'Doan nay dung de tro toi thu muc share chung 
            'Dim v_intErr As Integer = v_DataAccess.GetSysVar("CA", "APP_LOCATION", v_strAppPath)
            'v_DataAccess.GetSysVar("CA", "VSD_CA", v_strLocalDir)
            'v_strLocalDir = v_strLocalDir & "\Server"
            'bangpv_ save file to ftpserver 
            

            Dim v_strSQL = "select to_char(to_date(varvalue,'dd/mm/yyyy'),'YYYYMMDD') date_, to_char(to_date(varvalue,'dd/mm/yyyy'),'YYYYMMDD')||''''||to_char(sysdate,'hh24miss') time from sysvar where varname ='CURRDATE' and brid ='0008'"
            Dim v_trace As System.Data.DataSet = v_DataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            Dim v_strTime As String = v_trace.Tables(0).Rows(0)("time")
            v_strLocalDir = v_trace.Tables(0).Rows(0)("date_")
            'Gan header tieng Viet
            v_strSQL = "SELECT * FROM rpfld WHERE rptid ='" & v_strRptID & "' and deleted =0 AND status =0 and ISPARENT <>'Y' ORDER BY position "
            v_trace = v_DataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)


            Dim v_arrFieldName(v_trace.Tables(0).Rows.Count) As String
            Dim v_arrFieldCaption(v_trace.Tables(0).Rows.Count) As String
            For v_int = 0 To v_trace.Tables(0).Rows.Count - 1
                With v_trace.Tables(0)
                    v_arrFieldName(v_int) = UCase(.Rows(v_int)("FIELDNAME")) 'UCase(pv_ReportSetting.ReportFld(v_int).FieldName)
                    v_arrFieldCaption(v_int) = UCase(.Rows(v_int)("CAPTION")) 'UCase(pv_ReportSetting.ReportFld(v_int).Caption)
                End With
            Next
            v_strSQL = "SELECT * FROM rpgrp WHERE rptid ='" & v_strRptID & "' and deleted =0 AND status =0 "
            v_trace = v_DataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

            Dim v_arrGroupName(v_trace.Tables(0).Rows.Count) As String
            Dim v_arrGroupCation(v_trace.Tables(0).Rows.Count) As String
            For v_int = 0 To v_trace.Tables(0).Rows.Count - 1
                With v_trace.Tables(0)
                    v_arrGroupName(v_int) = UCase(.Rows(v_int)("FIELDNAME"))
                    v_arrGroupCation(v_int) = UCase(.Rows(v_int)("CATION"))
                End With
            Next
            If Not Directory.Exists(v_strAppPath & "\Log") Then
                Directory.CreateDirectory(v_strAppPath & "\Log")
            End If

            v_strAppPath = v_strAppPath & "\Log\" & v_strTLName & "\" & v_strLocalDir
            If Not Directory.Exists(v_strAppPath) Then
                Directory.CreateDirectory(v_strAppPath)
            End If
            v_strFileName = v_strTLName & "'" & v_strRptID & "'" & v_strSicode & "'" & v_strCAKey & "'" & v_strTime
            '20230219 Add Export PDF 
            If File.Exists(v_strAppPath & "\" & v_strFileName & ".pdf") Then
                File.Delete(v_strAppPath & "\" & v_strFileName & ".pdf")
            End If
            InitExportReport(pv_ds, v_strTLName, v_strRptID, v_strSicode, v_strCAKey, v_strClause, v_strAppPath & "\" & v_strFileName)

            'End 20230219
            Dim v_strFieldName As String
            Dim v_intCount As Integer = pv_ds.Tables(0).Columns.Count
            Dim v_strRemoveColumn As String = ""
            For i As Integer = 0 To v_intCount - 1
                If i < pv_ds.Tables(0).Columns.Count - 1 Then
                    v_strFieldName = Trim(pv_ds.Tables(0).Columns(i).ColumnName)
                    If v_arrFieldName.Contains(UCase(v_strFieldName)) Then
                        pv_ds.Tables(0).Columns(i).ColumnName = v_arrFieldCaption(v_arrFieldName.IndexOf(v_arrFieldName, UCase(v_strFieldName))) & "(" & i & ")"
                    ElseIf Not (v_arrGroupName.Contains(UCase(v_strFieldName)) Or v_arrGroupCation.Contains(UCase(v_strFieldName))) Then
                        v_strRemoveColumn &= v_strFieldName & "|"

                    Else
                        Select Case v_strFieldName
                            Case "MICODE"
                                pv_ds.Tables(0).Columns(i).ColumnName = "Mã TVLK"
                            Case "SICODE"
                                pv_ds.Tables(0).Columns(i).ColumnName = "Mã CK"
                            Case "IICODE"
                                pv_ds.Tables(0).Columns(i).ColumnName = "Mã PIN NĐT"
                            Case "IINAME"
                                pv_ds.Tables(0).Columns(i).ColumnName = "NĐT"
                            Case "MINAME"
                                pv_ds.Tables(0).Columns(i).ColumnName = "Tên TVLK"
                            Case "MI_NAME"
                                pv_ds.Tables(0).Columns(i).ColumnName = "Tên TVLK"
                            Case "STOCK_NAME"
                                pv_ds.Tables(0).Columns(i).ColumnName = "Tên CK"
                            Case "STOCKNAME"
                                pv_ds.Tables(0).Columns(i).ColumnName = "Tên CK"
                        End Select
                    End If
                End If
            Next
            If v_strRemoveColumn <> "" Then
                Dim v_arrRemoveColumn() As String = v_strRemoveColumn.Split("|")
                For j As Integer = 0 To v_arrRemoveColumn.Count - 1
                    If Trim(v_arrRemoveColumn(j)) <> "" Then
                        pv_ds.Tables(0).Columns.Remove(Trim(v_arrRemoveColumn(j)))
                    End If
                Next
            End If

          
            'Xóa file cũ nếu đã tạo 1 lần:
            If File.Exists(v_strAppPath & "\" & v_strTLName & "'" & v_strRptID & "'" & v_strSicode & "'" & v_strCAKey & "'" & v_strTime & "1.bat") Then
                File.Delete(v_strAppPath & "\" & v_strTLName & "'" & v_strRptID & "'" & v_strSicode & "'" & v_strCAKey & "'" & v_strTime & "1.bat")
            End If
            If File.Exists(v_strAppPath & "\" & v_strTLName & "'" & v_strRptID & "'" & v_strSicode & "'" & v_strCAKey & "'" & v_strTime & "2.bat") Then
                File.Delete(v_strAppPath & "\" & v_strTLName & "'" & v_strRptID & "'" & v_strSicode & "'" & v_strCAKey & "'" & v_strTime & "2.bat")
            End If

            
            If File.Exists(v_strAppPath & "\" & v_strFileName & ".xls") Then
                File.Delete(v_strAppPath & "\" & v_strFileName & ".xls")
            End If

            pv_ds.WriteXml(v_strAppPath & "\" & v_strFileName & ".xls")

            'add export pdf to server 
            Dim v_strPDF As String = v_strAppPath & "\" & v_strFileName & ".pdf"
            Dim v_strXLS As String = v_strAppPath & "\" & v_strFileName & ".xls"
            'tao file zip
            'If v_zipEngine.Zip2FileNotDel(v_strPDF, v_strXLS, _
            '                              v_strAppPath & "\" & v_strFileName & ".zip") = "" Then
            'Return ""
            'Exit Function
            'End If

            'end add export pdf to server 
            ' Đẩy file lên FTP server

            Dim v_strBrid As String = GetVsdBrid(v_strTLName)
            If v_strBrid = "HN" Then
                UploadToFTPServer("RPTFTPS01", v_strAppPath, v_strFileName, v_strTLName, v_strLocalDir)
            Else
                UploadToFTPServer("RPTFTPS02", v_strAppPath, v_strFileName, v_strTLName, v_strLocalDir)
            End If
            Return 0
        Catch ex As Exception
            LogError.Write(ex.Message, EventLogEntryType.Error, gc_MODULE_HOST)
            Return -1
        End Try
        GC.Collect()
    End Function
    Public Function UploadToFTPServer(ByVal pv_FTPSERVER As String, ByVal pv_strAppPath As String, ByVal pv_strFileName As String, ByVal pv_strTLName As String, ByVal pv_strLocalDir As String)
        Dim v_DataAccess As New DataAccess

        Try
            Dim v_strServerAddress, v_strServerPort, v_strUsername, v_strPassword, v_strRemotePath, v_strRootPath As String
            v_DataAccess.GetSysVar(pv_FTPSERVER, "ServerAddress", "0001", v_strServerAddress)
            v_DataAccess.GetSysVar(pv_FTPSERVER, "ServerPort", "0001", v_strServerPort)
            v_DataAccess.GetSysVar(pv_FTPSERVER, "Username", "0001", v_strUsername)
            v_DataAccess.GetSysVar(pv_FTPSERVER, "Password", "0001", v_strPassword)
            v_DataAccess.GetSysVar(pv_FTPSERVER, "RemotePath", "0001", v_strRemotePath)
            v_DataAccess.GetSysVar(pv_FTPSERVER, "RootPath", "0001", v_strRootPath)
            
            Dim v_oWriter As System.IO.StreamWriter
            If pv_FTPSERVER = "RPTFTPS01" Then
                v_oWriter = New StreamWriter(pv_strAppPath & "\" & pv_strFileName & "1.bat")
            Else
                v_oWriter = New StreamWriter(pv_strAppPath & "\" & pv_strFileName & "2.bat")
            End If

            v_oWriter.WriteLine("@ftp -i -s:""%~f0""&GOTO:EOF")
            v_oWriter.WriteLine("open " & v_strServerAddress)
            v_oWriter.WriteLine(v_strUsername)
            v_oWriter.WriteLine(v_strPassword)
            v_oWriter.WriteLine("lcd " & """" & pv_strAppPath & """")
            v_oWriter.WriteLine("cd " & v_strRemotePath)
            v_oWriter.WriteLine("mkdir " & pv_strTLName)
            v_oWriter.WriteLine("cd " & pv_strTLName)
            v_oWriter.WriteLine("mkdir " & pv_strLocalDir)
            v_oWriter.WriteLine("cd " & pv_strLocalDir)
            v_oWriter.WriteLine("binary")
            '20250507 edit: zip to xls
            '20230219 edit: xls to zip
            v_oWriter.WriteLine("put " & pv_strFileName & ".xls" & " " & pv_strFileName & ".xls")
            '20230219 edit: xls to zip
            '20250507 edit: zip to xls end 
            v_oWriter.WriteLine("bye" & vbCrLf)

            v_oWriter.Close()

            Dim v_oProcess As Process
            v_oProcess = New Process

            If pv_FTPSERVER = "RPTFTPS01" Then
                v_oProcess.StartInfo.FileName = pv_strAppPath & "\" & pv_strFileName & "1.bat"
            Else
                v_oProcess.StartInfo.FileName = pv_strAppPath & "\" & pv_strFileName & "2.bat"
            End If

            v_oProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
            v_oProcess.StartInfo.CreateNoWindow = True
            v_oProcess.Start()
            v_oProcess.Close()
            Return 0
        Catch ex As Exception
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
