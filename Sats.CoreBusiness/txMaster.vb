Imports Sats.DataAccessLayer
Imports Sats.CommonLibrary
'Imports System.EnterpriseServices

Public Interface IBusiness
    Function txUpdate(ByVal pv_xmlDocument As Xml.XmlDocument) As Long
    Function txCheck(ByVal pv_xmlDocument As Xml.XmlDocument) As Long
    Function txMisc(ByVal pv_xmlDocument As Xml.XmlDocument) As Long
End Interface

'<JustInTimeActivation(False), _
'Transaction(TransactionOption.Disabled), _
'ObjectPooling(Enabled:=True, MinPoolSize:=30)> _
Public Class txMaster
    Implements IDisposable
    'Inherits ServicedComponent

#Region " Khai báo hằng, biến "
    Dim mv_sModule As String
    Dim mv_sAcctNo As String
    Dim mv_sFrDate As String
    Dim mv_sToDate As String
    Dim mv_sCmdMiscInquiry As String
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

    Public Property ATTR_ACCTNO() As String
        Get
            Return mv_sAcctNo
        End Get
        Set(ByVal Value As String)
            mv_sAcctNo = Value
        End Set
    End Property

    Public Property ATTR_FRDATE() As String
        Get
            Return mv_sFrDate
        End Get
        Set(ByVal Value As String)
            mv_sFrDate = Value
        End Set
    End Property

    Public Property ATTR_TODATE() As String
        Get
            Return mv_sToDate
        End Get
        Set(ByVal Value As String)
            mv_sToDate = Value
        End Set
    End Property

    Public Property ATTR_CMDMISCINQUIRY() As String
        Get
            Return mv_sCmdMiscInquiry
        End Get
        Set(ByVal Value As String)
            mv_sCmdMiscInquiry = Value
        End Set
    End Property

#End Region

#Region " Core functions - can not override "
    Public Function txCoreMiscInquiry(ByRef pv_xmlDocument As Xml.XmlDocument) As Long
        Try
            Dim v_obj As DataAccess = Nothing
            Dim v_ds As DataSet
            Dim v_strSQL As String
            Dim v_strLOCAL As String

            Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes
            If Not (CType(v_attrColl.GetNamedItem(gc_AtributeLOCAL), Xml.XmlAttribute).Value Is Nothing) Then
                v_strLOCAL = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeLOCAL), Xml.XmlAttribute).Value)
            Else
                v_strLOCAL = String.Empty
            End If

            'Create connection to DB
            If v_strLOCAL = "Y" Then
                v_obj = New DataAccess
            ElseIf v_strLOCAL = "N" Then
                v_obj = New DataAccess
                v_obj.NewDBInstance(gc_MODULE_HOST)
            End If

            '?�?c d�ữ liệu tìm kiếm
            v_strSQL = Me.ATTR_CMDMISCINQUIRY
            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

            'Create data
            Dim v_dataElement As Xml.XmlElement, v_entryNode As Xml.XmlNode
            Dim i As Integer, j As Integer
            Dim v_attrFLDNAME As Xml.XmlAttribute, v_attrFLDTYPE As Xml.XmlAttribute, v_attrOLDVAL As Xml.XmlAttribute

            If v_ds.Tables(0).Rows.Count = 0 Then
                Return ERR_SY_NO_DATAFOUND
            Else
                For i = 0 To v_ds.Tables(0).Rows.Count - 1
                    v_dataElement = pv_xmlDocument.CreateElement(Xml.XmlNodeType.Element, "ObjData", "")
                    For j = 0 To v_ds.Tables(0).Columns.Count - 1
                        'Append entry to data node
                        v_entryNode = pv_xmlDocument.CreateNode(Xml.XmlNodeType.Element, "entry", "")

                        'Add field name
                        v_attrFLDNAME = pv_xmlDocument.CreateAttribute("fldname")
                        v_attrFLDNAME.Value = v_ds.Tables(0).Columns(j).ColumnName
                        v_entryNode.Attributes.Append(v_attrFLDNAME)

                        'Add field type
                        v_attrFLDTYPE = pv_xmlDocument.CreateAttribute("fldtype")
                        v_attrFLDTYPE.Value = v_ds.Tables(0).Columns(j).DataType.ToString
                        v_entryNode.Attributes.Append(v_attrFLDTYPE)

                        'Add current value
                        v_attrOLDVAL = pv_xmlDocument.CreateAttribute("oldval")
                        If IsDBNull((v_ds.Tables(0).Rows(i)(j))) Then
                            v_attrOLDVAL.Value = ""
                        Else
                            If v_ds.Tables(0).Rows(i)(j).GetType.Name = GetType(System.DateTime).Name Then
                                v_attrOLDVAL.Value = Format(v_ds.Tables(0).Rows(i)(j), gc_FORMAT_DATE)
                            Else
                                v_attrOLDVAL.Value = CStr(v_ds.Tables(0).Rows(i)(j))
                            End If

                        End If
                        v_entryNode.Attributes.Append(v_attrOLDVAL)

                        'Set value
                        If IsDBNull((v_ds.Tables(0).Rows(i)(j))) Then
                            v_entryNode.InnerText = ""
                        Else
                            If v_ds.Tables(0).Rows(i)(j).GetType.Name = GetType(System.DateTime).Name Then
                                v_entryNode.InnerText = Format(v_ds.Tables(0).Rows(i)(j), gc_FORMAT_DATE)
                            Else
                                v_entryNode.InnerText = CStr(v_ds.Tables(0).Rows(i)(j))
                            End If
                        End If

                        v_dataElement.AppendChild(v_entryNode)
                    Next
                    pv_xmlDocument.DocumentElement.AppendChild(v_dataElement)
                Next
            End If

            v_ds.Dispose()
            ''ContextUtil.SetComplete()
        Catch ex As Exception
            'ContextUtil.SetAbort()
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            '             & "Error code: System error!" & vbNewLine _
            '             & "Error message: " & ex.Message, EventLogEntryType.Error)
            Throw ex
        End Try
    End Function

    Public Function txCoreCheck(ByRef pv_xmlDocument As Xml.XmlDocument) As Long
        Dim v_lngErrCode As Long = ERR_SYSTEM_OK
        Dim v_strErrorSource As String = "CoreBusiness.txMaster.txCoreCheck"
        Dim v_attrColl As Xml.XmlAttributeCollection = Nothing
        Dim v_strTLTXCD As String = ""
        Dim v_strUPDMODE As String = ""
        Dim v_strLOCAL As String = ""
        Dim v_strTBLNAME As String = ""
        Dim v_strFLDKEY As String = ""
        Dim v_strACCTNO As String = ""
        Dim v_strFIELD As String = ""
        Dim v_strVALUE As String = ""
        Dim v_ds As DataSet = Nothing
        Dim v_strSQL As String = String.Empty
        Dim v_lngErrNumber As Long = 0
        Dim v_obj As DataAccess = Nothing

        Try
            'Get message information
            v_attrColl = pv_xmlDocument.DocumentElement.Attributes
            If Not (CType(v_attrColl.GetNamedItem(gc_AtributeTLTXCD), Xml.XmlAttribute).Value Is Nothing) Then
                v_strTLTXCD = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTLTXCD), Xml.XmlAttribute).Value)
            Else
                v_strTLTXCD = String.Empty
            End If
            If Not (CType(v_attrColl.GetNamedItem(gc_AtributeUPDATEMODE), Xml.XmlAttribute).Value Is Nothing) Then
                v_strUPDMODE = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeUPDATEMODE), Xml.XmlAttribute).Value)
            Else
                v_strUPDMODE = String.Empty
            End If
            If Not (CType(v_attrColl.GetNamedItem(gc_AtributeLOCAL), Xml.XmlAttribute).Value Is Nothing) Then
                v_strLOCAL = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeLOCAL), Xml.XmlAttribute).Value)
            Else
                v_strLOCAL = String.Empty
            End If

            v_strTBLNAME = String.Empty
            v_strFLDKEY = String.Empty
            v_strACCTNO = String.Empty
            v_strFIELD = String.Empty
            v_strVALUE = String.Empty

            'Create connection to DB
            If v_strLOCAL = "Y" Then
                v_obj = New DataAccess
            ElseIf v_strLOCAL = "N" Then
                v_obj = New DataAccess
                v_obj.NewDBInstance(gc_MODULE_HOST)
            End If

            'Scan APPCHK entry
            Dim v_nodeData As Xml.XmlNode
            Dim v_xmlDocument As New Xml.XmlDocument

            v_nodeData = pv_xmlDocument.SelectSingleNode("/TransactMessage/appchk")

            For i As Integer = 0 To v_nodeData.ChildNodes.Count - 1
                If CStr(v_nodeData.ChildNodes(i).Attributes.GetNamedItem("apptype").Value) = ATTR_MODULE Then
                    'New account for checking
                    If v_strTBLNAME <> CStr(v_nodeData.ChildNodes(i).Attributes.GetNamedItem("tblname").Value) _
                        Or v_strFLDKEY <> CStr(v_nodeData.ChildNodes(i).Attributes.GetNamedItem("fldkey").Value) _
                        Or v_strACCTNO <> CStr(v_nodeData.ChildNodes(i).Attributes.GetNamedItem("acctno").Value) Then
                        'Get account information
                        v_strTBLNAME = CStr(v_nodeData.ChildNodes(i).Attributes.GetNamedItem("tblname").Value)
                        v_strFLDKEY = CStr(v_nodeData.ChildNodes(i).Attributes.GetNamedItem("fldkey").Value)
                        v_strACCTNO = CStr(v_nodeData.ChildNodes(i).Attributes.GetNamedItem("acctno").Value)
                        v_strSQL = "SELECT * FROM " & v_strTBLNAME _
                        & " WHERE TRIM(" & v_strFLDKEY & ") = '" & v_strACCTNO & "' "

                        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                        If v_ds.Tables(0).Rows.Count = 0 Then
                            v_lngErrNumber = ERR_SY_APPCHK_ACCTNO_NOTFOUND
                            'LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
                            '             & "Error code: System error!" & vbNewLine _
                            '             & "Error message: " & v_strSQL & "." & ControlChars.CrLf & v_strTBLNAME & "." & v_strACCTNO, EventLogEntryType.Error)
                            'Trả v? m�ã lỗi
                            'ContextUtil.SetAbort()
                            Return v_lngErrNumber
                        End If
                    End If
                    v_strFIELD = CStr(v_nodeData.ChildNodes(i).Attributes.GetNamedItem("field").Value)
                    v_strVALUE = v_nodeData.ChildNodes(i).InnerXml

                    Select Case CStr(v_nodeData.ChildNodes(i).Attributes.GetNamedItem("operand").Value)
                        Case ">>"
                            If Not (CDbl(v_ds.Tables(0).Rows(0)(v_strFIELD)) > CDbl(v_strVALUE)) Then
                                v_lngErrNumber = CDbl(v_nodeData.ChildNodes(i).Attributes.GetNamedItem("errnum").Value)
                            End If
                        Case ">="
                            If Not (CDbl(v_ds.Tables(0).Rows(0)(v_strFIELD)) >= CDbl(v_strVALUE)) Then
                                v_lngErrNumber = CDbl(v_nodeData.ChildNodes(i).Attributes.GetNamedItem("errnum").Value)
                            End If
                        Case "<<"
                            If Not (CDbl(v_ds.Tables(0).Rows(0)(v_strFIELD)) < CDbl(v_strVALUE)) Then
                                v_lngErrNumber = CDbl(v_nodeData.ChildNodes(i).Attributes.GetNamedItem("errnum").Value)
                            End If
                        Case "<="
                            If Not (CDbl(v_ds.Tables(0).Rows(0)(v_strFIELD)) <= CDbl(v_strVALUE)) Then
                                v_lngErrNumber = CDbl(v_nodeData.ChildNodes(i).Attributes.GetNamedItem("errnum").Value)
                            End If
                        Case "=="
                            If Not (CDbl(v_ds.Tables(0).Rows(0)(v_strFIELD)) = CDbl(v_strVALUE)) Then
                                v_lngErrNumber = CDbl(v_nodeData.ChildNodes(i).Attributes.GetNamedItem("errnum").Value)
                            End If
                        Case "!="
                            If Not (CDbl(v_ds.Tables(0).Rows(0)(v_strFIELD)) <> CDbl(v_strVALUE)) Then
                                v_lngErrNumber = CDbl(v_nodeData.ChildNodes(i).Attributes.GetNamedItem("errnum").Value)
                            End If
                        Case "IN"
                            If Not (InStr(1, v_strVALUE, Trim(v_ds.Tables(0).Rows(0)(v_strFIELD))) > 0) Then
                                v_lngErrNumber = CDbl(v_nodeData.ChildNodes(i).Attributes.GetNamedItem("errnum").Value)
                            End If
                        Case "NI"
                            If (InStr(1, v_strVALUE, Trim(v_ds.Tables(0).Rows(0)(v_strFIELD))) > 0) Then
                                v_lngErrNumber = CDbl(v_nodeData.ChildNodes(i).Attributes.GetNamedItem("errnum").Value)
                            End If
                        Case Else
                    End Select

                    If v_lngErrNumber <> ERR_SYSTEM_OK Then
                        'LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
                        '                 & "Error code: System error!" & vbNewLine _
                        '                 & "Error message: " & v_strTLTXCD & "." & v_strTBLNAME & "." & v_strACCTNO & "." & v_strFIELD & "." & v_strVALUE, EventLogEntryType.Error)
                        'ContextUtil.SetAbort()
                        Return v_lngErrNumber
                    End If
                End If
            Next

            'For special implementing in Business module
            v_lngErrNumber = txImpCheck(pv_xmlDocument)
            If v_lngErrNumber <> ERR_SYSTEM_OK Then
                'ContextUtil.SetAbort()
            Else
                'ContextUtil.SetComplete()
            End If
            Return v_lngErrNumber
        Catch ex As Exception
            'ContextUtil.SetAbort()
            'LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
            '             & "Error code: System error!" & vbNewLine _
            '             & "Error message: " & ex.Message, EventLogEntryType.Error)
            Throw ex
        End Try
    End Function

    Public Function txCoreMisc(ByRef pv_xmlDocument As Xml.XmlDocument) As Long
        'For special implementing in Business module
        Return txImpMisc(pv_xmlDocument)
    End Function

    Public Function txCoreUpdate(ByRef pv_xmlDocument As Xml.XmlDocument) As Long
        Dim v_strSQLTemp As String = vbNullString
        Try
            Dim v_obj As DataAccess = Nothing
            Dim v_strSQL As String = vbNullString

            Dim v_strSQLMASTER As String = vbNullString
            Dim v_strSQLTRANS As String = vbNullString
            Dim v_strTABLE As String = vbNullString
            Dim v_strFIELD As String = vbNullString

            Dim v_strTBLNAME As String = vbNullString
            Dim v_strOLDTBLNAME As String = vbNullString
            Dim v_strACFLD As String = vbNullString
            Dim v_strFLDTYPE As String = vbNullString
            Dim v_strTXCD As String = vbNullString
            Dim v_strTXTYPE As String = vbNullString
            Dim v_dblNAMT As Double = 0
            Dim v_strCAMT As String = vbNullString
            Dim v_strACCTNO As String = vbNullString
            Dim v_strFLDKEY As String = vbNullString
            Dim v_strOLDACCTNO As String = vbNullString
            'Get message information
            Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes
            Dim v_strTLTXCD, v_strTXNUM, v_strTXDATE, v_strREVERSAL, v_strUPDMODE, v_strLOCAL As String

            If Not (CType(v_attrColl.GetNamedItem(gc_AtributeTLTXCD), Xml.XmlAttribute).Value Is Nothing) Then
                v_strTLTXCD = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTLTXCD), Xml.XmlAttribute).Value)
            Else
                v_strTLTXCD = String.Empty
            End If
            If Not (CType(v_attrColl.GetNamedItem(gc_AtributeTXNUM), Xml.XmlAttribute).Value Is Nothing) Then
                v_strTXNUM = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTXNUM), Xml.XmlAttribute).Value)
            Else
                v_strTXNUM = String.Empty
            End If
            If Not (CType(v_attrColl.GetNamedItem(gc_AtributeTXDATE), Xml.XmlAttribute).Value Is Nothing) Then
                v_strTXDATE = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTXDATE), Xml.XmlAttribute).Value)
            Else
                v_strTXDATE = String.Empty
            End If
            If Not (CType(v_attrColl.GetNamedItem(gc_AtributeDELTD), Xml.XmlAttribute).Value Is Nothing) Then
                v_strREVERSAL = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeDELTD), Xml.XmlAttribute).Value)
            Else
                v_strREVERSAL = String.Empty
            End If
            If Not (CType(v_attrColl.GetNamedItem(gc_AtributeUPDATEMODE), Xml.XmlAttribute).Value Is Nothing) Then
                v_strUPDMODE = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeUPDATEMODE), Xml.XmlAttribute).Value)
            Else
                v_strUPDMODE = String.Empty
            End If
            If Not (CType(v_attrColl.GetNamedItem(gc_AtributeLOCAL), Xml.XmlAttribute).Value Is Nothing) Then
                v_strLOCAL = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeLOCAL), Xml.XmlAttribute).Value)
            Else
                v_strLOCAL = String.Empty
            End If

            'Create an instance of DataAccess class
            If v_strLOCAL = "Y" Then
                v_obj = New DataAccess
            ElseIf v_strLOCAL = "N" Then
                v_obj = New DataAccess
                v_obj.NewDBInstance(gc_MODULE_HOST)
            End If

            'Which table will be updated
            Dim v_strTBLTRAN As String = ""

            'Thực hiện cập nhật đặc biệt luôn để nếu có lỗi sẽ rollback luôn
            Dim v_lngError As Long = txImpUpdate(pv_xmlDocument)
            If v_lngError <> ERR_SYSTEM_OK Then
                'ContextUtil.SetAbort()
                Return v_lngError
            End If

            'Scan APPMAP entry
            Dim v_nodeData As Xml.XmlNode
            v_nodeData = pv_xmlDocument.SelectSingleNode("/TransactMessage/appmap")
            If Not v_nodeData Is Nothing Then
                For i As Integer = 0 To v_nodeData.ChildNodes.Count - 1
                    If CStr(v_nodeData.ChildNodes(i).Attributes.GetNamedItem("apptype").Value) = ATTR_MODULE _
                        And Len(CStr(v_nodeData.ChildNodes(i).Attributes.GetNamedItem("ofile").Value)) = 0 Then
                        v_strACCTNO = CStr(v_nodeData.ChildNodes(i).Attributes.GetNamedItem("acctno").Value)
                        'Get value
                        v_strTXCD = CStr(v_nodeData.ChildNodes(i).Attributes.GetNamedItem("apptxcd").Value)
                        v_strTXTYPE = CStr(v_nodeData.ChildNodes(i).Attributes.GetNamedItem("txtype").Value)
                        v_strFLDTYPE = CStr(v_nodeData.ChildNodes(i).Attributes.GetNamedItem("fldtype").Value)
                        v_strTBLTRAN = CStr(v_nodeData.ChildNodes(i).Attributes.GetNamedItem("tranf").Value)
                        v_strFLDKEY = CStr(v_nodeData.ChildNodes(i).Attributes.GetNamedItem("fldkey").Value)
                        v_strTBLNAME = CStr(v_nodeData.ChildNodes(i).Attributes.GetNamedItem("tblname").Value)
                        v_dblNAMT = 0
                        v_strCAMT = vbNullString
                        Select Case v_strFLDTYPE
                            Case "N"
                                v_dblNAMT = CDbl(v_nodeData.ChildNodes(i).InnerXml)
                            Case "C", "D"
                                v_strCAMT = CStr(v_nodeData.ChildNodes(i).InnerXml)
                        End Select

                        'If save to transaction file
                        If Len(v_strTBLTRAN) > 0 And v_strREVERSAL = "N" Then
                            v_strSQLTemp = "INSERT INTO " & v_strTBLTRAN & " (ACCTNO, TXNUM, TXDATE, TXCD, NAMT, CAMT, REF, DELTD) VALUES ('" _
                                    & v_strACCTNO & "','" & v_strTXNUM & "',TO_DATE('" & v_strTXDATE & "', '" & gc_FORMAT_DATE & "'),'" & v_strTXCD & "'," & v_dblNAMT & ",'" & v_strCAMT & "','','N')"
                            'Thực hiện lệnh SQL
                            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQLTemp)
                        ElseIf Len(v_strTBLTRAN) > 0 And v_strREVERSAL = "Y" Then
                            'Delete transaction
                            v_strSQLTemp = "UPDATE " & v_strTBLTRAN & " SET DELTD = 'Y' " _
                                & "WHERE TRIM(TXNUM) = '" & v_strTXNUM & "' AND TXDATE = TO_DATE('" & v_strTXDATE & "', '" & gc_FORMAT_DATE & "')"
                            'Thực hiện lệnh SQL
                            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQLTemp)
                        End If

                        'Store SQL command to update account
                        If v_strACFLD <> CStr(v_nodeData.ChildNodes(i).Attributes.GetNamedItem("acfld").InnerXml) _
                            Or v_strTBLNAME <> v_strOLDTBLNAME Then 'if update to mext account
                            'Store current SQL command
                            If Len(v_strSQLMASTER) <> 0 Then
                                'remove first character of strSQLMASTER:  ","
                                v_strSQLMASTER = Mid(v_strSQLMASTER, 2)
                                v_strSQLTemp = " UPDATE " & v_strOLDTBLNAME & " SET " & v_strSQLMASTER & " WHERE TRIM(" & v_strFLDKEY & ") = '" & v_strOLDACCTNO & "'"
                                'Thực hiện lệnh SQL
                                v_obj.ExecuteNonQuery(CommandType.Text, v_strSQLTemp)
                            End If
                            'Reset value
                            v_strSQLMASTER = vbNullString
                        End If

                        'Build expression for field
                        If v_strFIELD <> CStr(v_nodeData.ChildNodes(i).Attributes.GetNamedItem("field").InnerXml) _
                            Or Len(v_strSQLMASTER) = 0 Then
                            v_strFIELD = Trim(CStr(v_nodeData.ChildNodes(i).Attributes.GetNamedItem("field").InnerXml))
                            'Update new field
                            Select Case v_strFLDTYPE
                                Case "N"
                                    Select Case v_strTXTYPE
                                        Case "D"
                                            v_strSQLMASTER = v_strSQLMASTER & "," & v_strFIELD & " = " & v_strFIELD & IIf(v_strREVERSAL = "Y", "+", "-") & "(" & v_dblNAMT & ")"
                                        Case "C"
                                            v_strSQLMASTER = v_strSQLMASTER & "," & v_strFIELD & " = " & v_strFIELD & IIf(v_strREVERSAL = "Y", "-", "+") & "(" & v_dblNAMT & ")"
                                        Case "U"
                                            v_strSQLMASTER = v_strSQLMASTER & "," & v_strFIELD & " = " & "(" & v_dblNAMT & ")"
                                    End Select
                                Case "C"
                                    Select Case v_strFIELD
                                        Case "ORSTATUS", "STATUS", "ODSTS", "ISOTC"
                                            v_strSQLMASTER = v_strSQLMASTER & "," & IIf(v_strREVERSAL = "Y", _
                                                v_strFIELD & "=SUBSTR(P" & v_strFIELD & ",LENGTH(P" & v_strFIELD & "),1), P" & v_strFIELD & "=SUBSTR(P" & v_strFIELD & ",1,LENGTH(P" & v_strFIELD & ")-1)", _
                                                "P" & v_strFIELD & " = TRIM(P" & v_strFIELD & ") || " & v_strFIELD & ", " & v_strFIELD & " = '" & v_strCAMT & "'")
                                        Case Else
                                            v_strSQLMASTER = v_strSQLMASTER & "," & v_strFIELD & " = '" & v_strCAMT & "'"
                                    End Select
                                Case "D"
                                    v_strSQLMASTER = v_strSQLMASTER & "," & v_strFIELD & " = TO_DATE('" & v_strCAMT & "','" & gc_FORMAT_DATE & "')"
                            End Select
                        Else
                            'If update old value and field type is numeric
                            Select Case v_strFLDTYPE
                                Case "N"
                                    Select Case v_strTXTYPE
                                        Case "D"
                                            v_strSQLMASTER = v_strSQLMASTER & IIf(v_strREVERSAL = "Y", "+", "-") & "(" & v_dblNAMT & ")"
                                        Case "C"
                                            v_strSQLMASTER = v_strSQLMASTER & IIf(v_strREVERSAL = "Y", "-", "+") & "(" & v_dblNAMT & ")"
                                    End Select
                            End Select
                        End If

                        'Ghi nhận tạm th?i
                        v_strOLDACCTNO = v_strACCTNO
                        v_strACFLD = CStr(v_nodeData.ChildNodes(i).Attributes.GetNamedItem("acfld").InnerXml)
                        v_strFIELD = CStr(v_nodeData.ChildNodes(i).Attributes.GetNamedItem("field").InnerXml)
                        v_strOLDTBLNAME = v_strTBLNAME
                    End If
                Next
            End If


            'Store current SQL command
            If Len(v_strSQLMASTER) <> 0 Then
                'remove first character of strSQLMASTER:  ","
                v_strSQLMASTER = Mid(v_strSQLMASTER, 2)
                'v_strSQLTemp = " UPDATE " & v_strTBLNAME & " SET " & v_strSQLMASTER & " WHERE TRIM(ACCTNO) = '" & v_strACCTNO & "'"
                v_strSQLTemp = " UPDATE " & v_strTBLNAME & " SET " & v_strSQLMASTER & " WHERE TRIM(" & v_strFLDKEY & ") = '" & v_strACCTNO & "'"
                v_strSQL = v_strSQL & v_strSQLTemp
                'Th�ực hiện lệnh SQL
                v_obj.ExecuteNonQuery(CommandType.Text, v_strSQLTemp)
            End If
            'v_strSQL = v_strSQL & " " & v_strSQLTRANS
            'v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

            'ContextUtil.SetComplete()
            Return v_lngError
        Catch ex As Exception
            'ContextUtil.SetAbort()
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            '             & "Error code: System error!" & vbNewLine _
            '             & "Error message: " & ControlChars.CrLf & v_strSQLTemp & ControlChars.CrLf & ex.Message, EventLogEntryType.Error)
            Throw ex
        End Try

    End Function
#End Region

#Region " Implement functions - Overridable "
    Public Overridable Function txImpUpdate(ByRef pv_xmlDocument As Xml.XmlDocument) As Long
        'ContextUtil.SetComplete()
        Return 0
    End Function

    Public Overridable Function txImpCheck(ByRef pv_xmlDocument As Xml.XmlDocument) As Long
        'ContextUtil.SetComplete()
        Return 0
    End Function

    Public Overridable Function txImpMisc(ByRef pv_xmlDocument As Xml.XmlDocument) As Long
        'ContextUtil.SetComplete()
        Return 0
    End Function
#End Region

#Region " Based functions - Overridable "
    Public Overridable Function txUpdate(ByRef pv_xmlDocument As Xml.XmlDocument) As Long
        Dim v_lngErrCode As Long

        Try
            v_lngErrCode = txCoreUpdate(pv_xmlDocument)
            If v_lngErrCode <> ERR_SYSTEM_OK Then
                'ContextUtil.SetAbort()

                Dim v_strErrorSource, v_strErrorMessage As String
                v_strErrorSource = ATTR_MODULE & ".txUpdate"
                v_strErrorMessage = String.Empty
                'LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
                '             & "Error code: " & v_lngErrCode.ToString() & vbNewLine _
                '             & "Error message: " & v_strErrorMessage, EventLogEntryType.Information)
            Else
                'ContextUtil.SetComplete()
            End If

            Return v_lngErrCode
        Catch ex As Exception
            'ContextUtil.SetAbort()
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            '             & "Error code: System error!" & vbNewLine _
            '             & "Error message: " & ex.Message, EventLogEntryType.Error)
            Throw ex
        End Try
    End Function

    Public Overridable Function txCheck(ByRef pv_xmlDocument As Xml.XmlDocument) As Long
        Dim v_lngErrCode As Long

        Try
            v_lngErrCode = txCoreCheck(pv_xmlDocument)
            If v_lngErrCode <> ERR_SYSTEM_OK Then
                'ContextUtil.SetAbort()

                Dim v_strErrorSource, v_strErrorMessage As String
                v_strErrorSource = ATTR_MODULE & ".txCheck"
                v_strErrorMessage = String.Empty
                'LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
                '             & "Error code: " & v_lngErrCode.ToString() & vbNewLine _
                '             & "Error message: " & v_strErrorMessage, EventLogEntryType.Information)
                BuildXMLErrorException(pv_xmlDocument, v_strErrorSource, v_lngErrCode, v_strErrorMessage)

            Else
                'ContextUtil.SetComplete()
            End If

            Return v_lngErrCode
        Catch ex As Exception
            'ContextUtil.SetAbort()
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            '             & "Error code: System error!" & vbNewLine _
            '             & "Error message: " & ex.Message, EventLogEntryType.Error)
            Throw ex
        End Try
    End Function

    Public Overridable Function txMisc(ByRef pv_xmlDocument As Xml.XmlDocument) As Long
        Dim v_lngErrCode As Long

        Try
            v_lngErrCode = txCoreMisc(pv_xmlDocument)
            If v_lngErrCode <> ERR_SYSTEM_OK Then
                'ContextUtil.SetAbort()

                Dim v_strErrorSource, v_strErrorMessage As String
                v_strErrorSource = ATTR_MODULE & ".txMisc"
                v_strErrorMessage = String.Empty
                'LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
                '             & "Error code: " & v_lngErrCode.ToString() & vbNewLine _
                '             & "Error message: " & v_strErrorMessage, EventLogEntryType.Information)
            Else
                'ContextUtil.SetComplete()
            End If

            Return v_lngErrCode
        Catch ex As Exception
            'ContextUtil.SetAbort()
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
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
