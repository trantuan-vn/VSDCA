Imports Sats.DataAccessLayer
Imports Sats.CommonLibrary
'Imports System.EnterpriseServices
Imports Sats.CommonLibrary.ZipEngine
Imports System.Configuration

Public Interface IMaster
    Function Add(ByRef pv_xmlDocument As XmlDocumentEx) As Long
    Function Edit(ByRef pv_xmlDocument As XmlDocumentEx) As Long
    Function Delete(ByRef pv_xmlDocument As XmlDocumentEx) As Long
    Function Inquiry(ByRef pv_xmlDocument As XmlDocumentEx) As Long
    Function Adhoc(ByRef pv_xmlDocument As XmlDocumentEx) As Long
End Interface

'<JustInTimeActivation(False), _
'Transaction(TransactionOption.Disabled), _
'ObjectPooling(Enabled:=True, MinPoolSize:=30)> _
Public Class objMaster
    Implements IDisposable
    'Inherits ServicedComponent

    Dim mv_sTABLE As String

    Dim mv_strIPAddress As String = ""
    Dim mv_strWsName As String = ""
    Dim mv_strTellerId As String = ""
    Dim mv_strTellerName As String = ""
    Public mv_arrTableName() As String = {"IA", "MA", "CA", "SF", "CS", "MF", "TLLOG", "RG"}
    Public Shared ReportDataSets As New Dictionary(Of String, DataSet)
    Public Shared ReportExecuteId As Integer = 0
#Region "Property"
    Public Property ATTR_TABLE() As String
        Get
            Return mv_sTABLE
        End Get
        Set(ByVal Value As String)
            mv_sTABLE = Value
        End Set
    End Property
#End Region

#Region "Core Functions"
    Public Function CoreAdd(ByRef pv_xmlDocument As XmlDocumentEx) As Long
        Dim v_lngErrorCode As Long
        Dim v_strSYSVAR As String = "", v_DataAccess As New DataAccess
        Dim v_strBRID As String = ""
        Dim v_obj As New DataAccess
        Try
            Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes
            If Not (v_attrColl.GetNamedItem(gc_AtributeBRID) Is Nothing) Then
                v_strBRID = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeBRID), Xml.XmlAttribute).Value)
            End If
            'Check HOST Active
            v_DataAccess.NewDBInstance(gc_MODULE_HOST)
            v_lngErrorCode = v_DataAccess.GetSysVar("SYSTEM", "BRSTATUS", v_strBRID, v_strSYSVAR)
            If v_lngErrorCode <> ERR_SYSTEM_OK Then
                'ContextUtil.SetAbort()
                Return v_lngErrorCode
            End If
            If v_strSYSVAR <> OPERATION_ACTIVE Then
                'ContextUtil.SetAbort()
                Return ERR_SY_HOST_OPERATION_ISINACTIVE
            End If

            v_lngErrorCode = CheckBeforeAdd(pv_xmlDocument)

            If v_lngErrorCode <> 0 Then
                'ContextUtil.SetAbort()
                Return v_lngErrorCode
                Exit Function
            End If


            Dim v_strClause As String
            Dim v_strLocal As String
            Dim v_strAutoId As String

            If Not (v_attrColl.GetNamedItem(gc_AtributeCLAUSE) Is Nothing) Then
                v_strClause = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCLAUSE), Xml.XmlAttribute).Value)
            Else
                v_strClause = String.Empty
            End If
            If Not (v_attrColl.GetNamedItem(gc_AtributeLOCAL) Is Nothing) Then
                v_strLocal = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeLOCAL), Xml.XmlAttribute).Value)
            Else
                v_strLocal = String.Empty
            End If
            If Not (v_attrColl.GetNamedItem(gc_AtributeAUTOID) Is Nothing) Then
                v_strAutoId = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeAUTOID), Xml.XmlAttribute).Value)
            Else
                v_strAutoId = String.Empty
            End If

            'Inquiry data

            'If v_strLocal = "Y" Then
            'v_obj.NewDBInstance(gc_MODULE_BDS)
            'ElseIf v_strLocal = "N" Then
            v_obj.NewDBInstance(gc_MODULE_HOST)
            'End If

            Dim v_strSQL As String = "INSERT INTO " & ATTR_TABLE
            Dim v_strListOfFields As String = vbNullString
            Dim v_strListOfValues As String = vbNullString
            Dim v_strSignature As String = String.Empty
            Dim v_strCustID As String = String.Empty


            Dim v_decID As Decimal
            If (v_strAutoId = gc_AutoIdUsed) Then
                v_decID = v_obj.GetIDValue(ATTR_TABLE)
                v_strListOfFields = "(AUTOID"
                v_strListOfValues = "(" & v_decID
            End If

            'Cập nhật vào CSDL
            Dim v_nodeList As Xml.XmlNodeList, i As Integer
            Dim v_strNewValue As String
            Dim v_strFLDNAME As String
            Dim v_strFLDTYPE As String

            v_nodeList = pv_xmlDocument.SelectNodes("/ObjectMessage/ObjData")


            For i = 0 To v_nodeList.Item(0).ChildNodes.Count - 1
                With v_nodeList.Item(0).ChildNodes(i)
                    v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                    v_strFLDTYPE = CStr(CType(.Attributes.GetNamedItem("fldtype"), Xml.XmlAttribute).Value)

                    'If v_strFLDNAME = "AUTOID" Then
                    ' v_strNewValue = v_decID
                    ' Else
                    v_strNewValue = .InnerText.ToString
                    ' End If

                    If Len(v_strNewValue) > 0 Then
                        If Len(v_strListOfFields) = 0 Then
                            v_strListOfFields = "(" & v_strFLDNAME
                            Select Case v_strFLDTYPE
                                Case "System.String"
                                    v_strListOfValues = "('" & Replace(v_strNewValue, "'", "''") & "'"
                                Case "System.Date"
                                    v_strListOfValues = "('" & v_strNewValue & "'"
                                Case Else
                                    v_strListOfValues = "(" & v_strNewValue
                            End Select
                        Else
                            v_strListOfFields = v_strListOfFields & "," & v_strFLDNAME
                            Select Case v_strFLDTYPE
                                Case "System.String"
                                    v_strListOfValues = v_strListOfValues & ",'" & Replace(v_strNewValue, "'", "''") & "'"
                                Case "System.DateTime"
                                    v_strListOfValues = v_strListOfValues & ",TO_DATE('" & v_strNewValue & "', '" & gc_FORMAT_DATE & "')"
                                Case GetType(Double).Name
                                    v_strListOfValues = v_strListOfValues & "," & Replace(v_strNewValue, ",", "")
                                Case Else
                                    v_strListOfValues = v_strListOfValues & "," & v_strNewValue
                            End Select
                        End If
                    End If
                End With
            Next

            If Len(v_strListOfFields) <> 0 Then
                v_strListOfFields = v_strListOfFields & ")"
                v_strListOfValues = v_strListOfValues & ")"
                v_strSQL = v_strSQL & " " & v_strListOfFields & " VALUES " & v_strListOfValues
                v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
            End If
            'ContextUtil.SetComplete()
            Return 0
        Catch ex As Exception
            Throw ex
        Finally
            If Not v_obj Is Nothing Then
                v_obj.Dispose()
            End If
            If Not v_DataAccess Is Nothing Then
                v_DataAccess.Dispose()
            End If
        End Try
    End Function
    'Added by Thanglv9 - 13/12/2012 - Insert vao TLLOG 
    Public Function CoreAddtoLOG(ByRef pv_xmlDocument As XmlDocumentEx) As Long
        Dim v_lngErrorCode As Long
        Dim v_strSYSVAR As String = "", v_DataAccess As New DataAccess
        Dim v_strBRID As String = ""
        Dim v_obj As New DataAccess
        Dim v_ds As DataSet
        Try
            Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes
            If Not (v_attrColl.GetNamedItem(gc_AtributeBRID) Is Nothing) Then
                v_strBRID = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeBRID), Xml.XmlAttribute).Value)
            End If
            'Check HOST Active
            v_DataAccess.NewDBInstance(gc_MODULE_HOST)
            v_lngErrorCode = v_DataAccess.GetSysVar("SYSTEM", "BRSTATUS", v_strBRID, v_strSYSVAR)
            If v_lngErrorCode <> ERR_SYSTEM_OK Then
                'ContextUtil.SetAbort()
                Return v_lngErrorCode
            End If
            If v_strSYSVAR <> OPERATION_ACTIVE Then
                'ContextUtil.SetAbort()
                Return ERR_SY_HOST_OPERATION_ISINACTIVE
            End If

            Dim v_strClause As String
            Dim v_strLocal As String
            Dim v_strAutoId As String

            If Not (v_attrColl.GetNamedItem(gc_AtributeCLAUSE) Is Nothing) Then
                v_strClause = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCLAUSE), Xml.XmlAttribute).Value)
            Else
                v_strClause = String.Empty
            End If
            If Not (v_attrColl.GetNamedItem(gc_AtributeLOCAL) Is Nothing) Then
                v_strLocal = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeLOCAL), Xml.XmlAttribute).Value)
            Else
                v_strLocal = String.Empty
            End If
            If Not (v_attrColl.GetNamedItem(gc_AtributeAUTOID) Is Nothing) Then
                v_strAutoId = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeAUTOID), Xml.XmlAttribute).Value)
            Else
                v_strAutoId = String.Empty
            End If

            Dim v_strTXDATE As String
            If Not (v_attrColl.GetNamedItem(gc_AtributeTXDATE) Is Nothing) Then
                v_strTXDATE = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTXDATE), Xml.XmlAttribute).Value)
            Else
                v_strTXDATE = String.Empty
            End If
            Dim v_strBUSDATE As String
            If Not (v_attrColl.GetNamedItem(gc_AtributeBUSDATE) Is Nothing) Then
                v_strBUSDATE = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeBUSDATE), Xml.XmlAttribute).Value)
            Else
                v_strBUSDATE = String.Empty
            End If

            Dim v_strBRCODE As String
            If Not (v_attrColl.GetNamedItem(gc_AtributeBRCODE) Is Nothing) Then
                v_strBRCODE = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeBRCODE), Xml.XmlAttribute).Value)
            Else
                v_strBRCODE = String.Empty
            End If
            Dim v_strTLID As String
            If Not (v_attrColl.GetNamedItem(gc_AtributeTLID) Is Nothing) Then
                v_strTLID = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTLID), Xml.XmlAttribute).Value)
            Else
                v_strTLID = String.Empty
            End If
            Dim v_strTLNAME As String
            If Not (v_attrColl.GetNamedItem(gc_AtributeTLNAME) Is Nothing) Then
                v_strTLNAME = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTLNAME), Xml.XmlAttribute).Value)
            Else
                v_strTLNAME = String.Empty
            End If
            Dim v_strIPADDRESS As String
            If Not (v_attrColl.GetNamedItem(gc_AtributeIPADDRESS) Is Nothing) Then
                v_strIPADDRESS = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeIPADDRESS), Xml.XmlAttribute).Value)
            Else
                v_strIPADDRESS = String.Empty
            End If
            Dim v_strWSNAME As String
            If Not (v_attrColl.GetNamedItem(gc_AtributeWSNAME) Is Nothing) Then
                v_strWSNAME = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeWSNAME), Xml.XmlAttribute).Value)
            Else
                v_strWSNAME = String.Empty
            End If
            Dim v_strTXNAME As String
            If Not (v_attrColl.GetNamedItem(gc_AtributeTXNAME) Is Nothing) Then
                v_strTXNAME = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTXNAME), Xml.XmlAttribute).Value)
            Else
                v_strTXNAME = String.Empty
            End If
            Dim v_strVsdBrid As String
            If Not (v_attrColl.GetNamedItem(gc_AtributeVSDBRID) Is Nothing) Then
                v_strVsdBrid = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeVSDBRID), Xml.XmlAttribute).Value)
            Else
                v_strVsdBrid = String.Empty
            End If
            Dim v_strObjectName As String
            If Not (v_attrColl.GetNamedItem(gc_AtributeOBJNAME) Is Nothing) Then
                v_strObjectName = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeOBJNAME), Xml.XmlAttribute).Value)
            Else
                v_strObjectName = String.Empty
            End If

            v_obj.NewDBInstance(gc_MODULE_HOST)


            Dim v_strSQL As String = "INSERT INTO TLLOG"
            Dim v_strListOfFields As String = vbNullString
            Dim v_strListOfValues As String = vbNullString
            Dim v_strSignature As String = String.Empty
            Dim v_strCustID As String = String.Empty
            Dim v_strTLTXCD As String

            If v_strObjectName = "RG.RGIS" Then
                v_strTLTXCD = "2080"
            ElseIf v_strObjectName = "RG.RGSI" Then
                v_strTLTXCD = "2081"
            ElseIf v_strObjectName = "RG.RGMI" Then
                v_strTLTXCD = "2083"
            Else
                v_strTLTXCD = ""
            End If

            Dim v_dsTLTX As DataSet
            Dim v_strSQLParentText As String = "SELECT a.* from allcode a where a.cdname = 'ISPARENT' and a.CDTYPE = 'SY' and a.CDVALNO = (SELECT ISPARENT FROM TLTX where TLTXCD='" & v_strTLTXCD & "')"
            v_dsTLTX = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLParentText)
            Dim v_strParentText As String = v_dsTLTX.Tables(0).Rows(0)("CDCONTENT").ToString
            Dim v_strISPARENT As String = v_dsTLTX.Tables(0).Rows(0)("CDVALNO").ToString

            Dim v_autoID As String
            Dim v_strSQLID As String
            'If (v_strAutoId = gc_AutoIdUsed) Then
            v_strSQLID = "select seq_tllog.NEXTVAL AUTOID from dual"
            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLID)
            v_autoID = v_ds.Tables(0).Rows(0)("AUTOID")
            v_strListOfFields = "(autoid, txnum, txdate, brid, tlid, offid, cfrid," _
                                & "chkid, tltxcd, off_line, busdate, txdesc, ipaddress," _
                                & "wsname, status, batchname, micode, deleted, msgamt,isparent,parentid,brcode," _
                                & "tlname,offname,cfrname,chkname,txname,status_text,parent_text,sicode,tmpid,childtltxcd,t_no,msgdate,"
            v_strListOfValues = "(" & v_autoID & ", GENERATE_TLLOG_CODE(" & v_autoID & "), to_date('" & v_strTXDATE & "','DD/MM/YYYY')," _
                            & "'" & v_strBRID & "', '" & v_strTLID & "','" & v_strTLID & "','" & v_strTLID & "','" & v_strTLID & "','" & v_strTLTXCD & "',null, to_date('" & v_strBUSDATE & "','DD/MM/YYYY'), null," _
                            & "'" & v_strIPADDRESS & "', '" & v_strWSNAME & "'," & CStr(TransactStatus.CONFIRMED_VSD_MANAGER) & ",null,'000',0,0," & v_strISPARENT & ",0,'" & v_strBRCODE & "','" & v_strTLNAME & "','" & v_strTLNAME & "','" & v_strTLNAME & "','" & v_strTLNAME & "'," _
                            & "'" & v_strTXNAME & "','" & CONFIRMED_VSD_MANAGER_TEXT & "','" & v_strParentText & "',null,null,null,null,null,"
            'End If

            'Cập nhật vào CSDL
            Dim v_nodeList As Xml.XmlNodeList, i As Integer
            Dim v_strNewValue As String
            Dim v_strFLDNAME As String
            Dim v_strFLDTYPE As String
            Dim v_strCBO As String = ""
            Dim v_strLLIST As String = ""
            Dim v_str_coldesc As String = ""

            v_nodeList = pv_xmlDocument.SelectNodes("/ObjectMessage/ObjData")


            For i = 0 To v_nodeList.Item(0).ChildNodes.Count - 1
                With v_nodeList.Item(0).ChildNodes(i)
                    v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                    v_strFLDTYPE = CStr(CType(.Attributes.GetNamedItem("fldtype"), Xml.XmlAttribute).Value)

                    v_strCBO = "select llist from fldmaster where llist is not null and objname = '" & v_strTLTXCD & "' and defname = " _
                                & "(select defname from fldmaster where objname = '" & v_strObjectName & "' and fldname = '" & v_strFLDNAME & "' and deleted = 0) and deleted = 0"
                    v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strCBO)
                    If v_ds.Tables(0).Rows.Count > 0 Then
                        v_strLLIST = v_ds.Tables(0).Rows(0)("LLIST")
                        v_strLLIST = Replace(v_strLLIST, "+", "||")
                    Else
                        v_strLLIST = ""
                    End If
                    'If v_strFLDNAME = "AUTOID" Then
                    ' v_strNewValue = v_decID
                    ' Else
                    v_strNewValue = .InnerText.ToString
                    ' End If

                    If Len(v_strNewValue) > 0 Then
                        If i < 9 Then
                            v_strListOfFields = v_strListOfFields & "col_value0" & (i + 1) & ",col_type0" & (i + 1) & ",col_desc0" & (i + 1) & ","
                        Else
                            v_strListOfFields = v_strListOfFields & "col_value" & (i + 1) & ",col_type" & (i + 1) & ",col_desc" & (i + 1) & ","
                        End If
                        If Len(v_strLLIST) > 0 Then
                            v_strCBO = "select a.display from (" & v_strLLIST & ") a where a.value ='" & v_strNewValue & "'"
                            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strCBO)
                            If v_ds.Tables(0).Rows.Count > 0 Then
                                v_str_coldesc = Convert.ToString(v_ds.Tables(0).Rows(0)("DISPLAY"))
                            Else
                                v_str_coldesc = v_strNewValue
                            End If
                        Else
                            v_str_coldesc = v_strNewValue
                        End If
                        Select Case v_strFLDTYPE
                            Case "System.String"
                                v_strListOfValues = v_strListOfValues & "'" & Replace(v_strNewValue, "'", "''") & "','C','" & Replace(v_str_coldesc, "'", "''") & "',"
                            Case "System.DateTime"
                                v_strListOfValues = v_strListOfValues & "TO_DATE('" & v_strNewValue & "', '" & gc_FORMAT_DATE & "'),'D',TO_DATE('" & v_str_coldesc & "', '" & gc_FORMAT_DATE & "'),"
                            Case GetType(Double).Name
                                If Len(v_strLLIST) > 0 Then
                                    v_strListOfValues = v_strListOfValues & Replace(v_strNewValue, ",", "") & ",'N','" & Replace(v_str_coldesc, ",", "") & "',"
                                Else
                                    v_strListOfValues = v_strListOfValues & Replace(v_strNewValue, ",", "") & ",'N'," & Replace(v_str_coldesc, ",", "") & ","
                                End If
                            Case Else
                                If Len(v_strLLIST) > 0 Then
                                    v_strListOfValues = v_strListOfValues & v_strNewValue & ",null,'" & v_str_coldesc & "',"
                                Else
                                    v_strListOfValues = v_strListOfValues & v_strNewValue & ",null," & v_str_coldesc & ","
                                End If
                        End Select
                    Else
                        If i < 9 Then
                            v_strListOfFields = v_strListOfFields & "col_value0" & (i + 1) & ",col_type0" & (i + 1) & ",col_desc0" & (i + 1) & ","
                        Else
                            v_strListOfFields = v_strListOfFields & "col_value" & (i + 1) & ",col_type" & (i + 1) & ",col_desc" & (i + 1) & ","
                        End If
                        v_strListOfValues = v_strListOfValues & "null,null,null,"
                    End If
                End With
            Next
            '"(" & v_autoID & ", GENERATE_TLLOG_CODE(" & v_autoID & ")
            If Len(v_strListOfFields) <> 0 Then
                v_strListOfFields = v_strListOfFields & "isbrid,vsd_brid)"
                v_strListOfValues = v_strListOfValues & "'1','" & v_strVsdBrid & "')"
                v_strSQL = v_strSQL & " " & v_strListOfFields & " VALUES " & v_strListOfValues
                v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

                If v_strTLTXCD = "2081" Then
                    'Sinh 2181
                    v_strSQLID = "select seq_tllog.NEXTVAL AUTOID from dual"
                    v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLID)
                    Dim v_autoID1 As String = v_ds.Tables(0).Rows(0)("AUTOID")

                    v_strSQL = Replace(v_strSQL, "'2081'", "'2181'")
                    v_strSQL = Replace(v_strSQL, "(" & v_autoID & ", GENERATE_TLLOG_CODE(" & v_autoID & ")", "(" & v_autoID1 & ", GENERATE_TLLOG_CODE(" & v_autoID1 & ")")

                    v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                    'Sinh 2182
                    v_strSQLID = "select seq_tllog.NEXTVAL AUTOID from dual"
                    v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLID)
                    v_autoID = v_ds.Tables(0).Rows(0)("AUTOID")

                    v_strSQL = Replace(v_strSQL, "'2181'", "'2182'")
                    v_strSQL = Replace(v_strSQL, "(" & v_autoID1 & ", GENERATE_TLLOG_CODE(" & v_autoID1 & ")", "(" & v_autoID & ", GENERATE_TLLOG_CODE(" & v_autoID & ")")

                    v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

                End If
            End If
            'ContextUtil.SetComplete()
            Return 0
        Catch ex As Exception
            Throw ex
            LogError.Write("CoreAddToLOG - Error source: " & ex.Source & vbNewLine _
                         & "Error code: System error!" & vbNewLine _
                         & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_HOST)
        Finally
            If Not v_obj Is Nothing Then
                v_obj.Dispose()
            End If
            If Not v_DataAccess Is Nothing Then
                v_DataAccess.Dispose()
            End If
        End Try
    End Function
    'End

    Public Function CoreEdit(ByRef pv_xmlDocument As XmlDocumentEx) As Long
        Dim v_lngErrorCode As Long
        Dim v_strSYSVAR As String = "", v_DataAccess As New DataAccess
        Dim v_strBRID As String = ""
        Dim v_obj As New DataAccess
        Try
            Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes
            If Not (v_attrColl.GetNamedItem(gc_AtributeBRID) Is Nothing) Then
                v_strBRID = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeBRID), Xml.XmlAttribute).Value)
            End If
            'Check HOST Active
            v_DataAccess.NewDBInstance(gc_MODULE_HOST)
            v_lngErrorCode = v_DataAccess.GetSysVar("SYSTEM", "BRSTATUS", v_strBRID, v_strSYSVAR)
            If v_lngErrorCode <> ERR_SYSTEM_OK Then
                'ContextUtil.SetAbort()
                Return v_lngErrorCode
            End If
            If v_strSYSVAR <> OPERATION_ACTIVE Then
                'ContextUtil.SetAbort()
                Return ERR_SY_HOST_OPERATION_ISINACTIVE
            End If

            v_lngErrorCode = CheckBeforeEdit(pv_xmlDocument)
            If v_lngErrorCode <> 0 Then
                'ContextUtil.SetAbort()
                Return v_lngErrorCode
                Exit Function
            End If


            Dim v_strClause As String
            Dim v_strLocal As String

            If Not (v_attrColl.GetNamedItem(gc_AtributeCLAUSE) Is Nothing) Then
                v_strClause = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCLAUSE), Xml.XmlAttribute).Value)
            Else
                v_strClause = String.Empty
            End If
            If Not (v_attrColl.GetNamedItem(gc_AtributeLOCAL) Is Nothing) Then
                v_strLocal = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeLOCAL), Xml.XmlAttribute).Value)
            Else
                v_strLocal = String.Empty
            End If

            'Update data

            'If v_strLocal = "Y" Then
            '    v_obj.NewDBInstance(gc_MODULE_BDS)
            'ElseIf v_strLocal = "N" Then
            v_obj.NewDBInstance(gc_MODULE_HOST)
            'End If

            Dim v_strSQL As String = "UPDATE " & ATTR_TABLE & " SET ", v_strUPD As String = vbNullString, v_strUPDTMP As String = vbNullString

            Dim v_nodeList As Xml.XmlNodeList, i As Integer
            Dim v_strNewValue As String, v_strOldValue As String, v_strFLDNAME As String, v_strFLDTYPE As String

            v_nodeList = pv_xmlDocument.SelectNodes("/ObjectMessage/ObjData")

            For i = 0 To v_nodeList.Item(0).ChildNodes.Count - 1
                With v_nodeList.Item(0).ChildNodes(i)
                    v_strOldValue = CStr(CType(v_nodeList.Item(0).ChildNodes(i).Attributes.GetNamedItem("oldval"), Xml.XmlAttribute).Value)
                    v_strNewValue = .InnerText.ToString
                    If Trim(v_strOldValue) <> Trim(v_strNewValue) Then
                        v_strFLDNAME = CStr(CType(v_nodeList.Item(0).ChildNodes(i).Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                        v_strFLDTYPE = CStr(CType(v_nodeList.Item(0).ChildNodes(i).Attributes.GetNamedItem("fldtype"), Xml.XmlAttribute).Value)

                        Select Case v_strFLDTYPE
                            Case "System.String"
                                v_strUPDTMP = v_strFLDNAME & " = '" & Replace(v_strNewValue, "'", "''") & "'"
                            Case "System.DateTime"
                                v_strUPDTMP = v_strFLDNAME & " = TO_DATE('" & v_strNewValue & "', '" & gc_FORMAT_DATE & "')"
                            Case GetType(Double).Name
                                v_strUPDTMP = v_strFLDNAME & "=" & Replace(v_strNewValue, ",", "")
                            Case Else
                                v_strUPDTMP = v_strFLDNAME & "=" & v_strNewValue
                        End Select

                        If Len(v_strUPD) = 0 Then
                            v_strUPD = v_strUPDTMP
                        Else
                            v_strUPD = v_strUPD & ", " & v_strUPDTMP
                        End If
                    Else
                        v_strFLDNAME = CStr(CType(v_nodeList.Item(0).ChildNodes(i).Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                        v_strFLDTYPE = CStr(CType(v_nodeList.Item(0).ChildNodes(i).Attributes.GetNamedItem("fldtype"), Xml.XmlAttribute).Value)
                    End If
                End With
            Next

            If Len(v_strUPD) <> 0 Then
                v_strSQL = v_strSQL & v_strUPD & " WHERE 0=0 AND " & v_strClause
                v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
            End If
            'ContextUtil.SetComplete()
            'v_obj.Dispose()
            Return 0

        Catch ex As Exception
            Throw ex
        Finally
            If Not v_obj Is Nothing Then
                v_obj.Dispose()
            End If
            If Not v_DataAccess Is Nothing Then
                v_DataAccess.Dispose()
            End If
        End Try
    End Function
    'Added by Thanglv9 - 14/12/2012
    Public Function CoreInSertToLogAfterEdit(ByRef pv_xmlDocument As XmlDocumentEx) As Long
        Dim v_lngErrorCode As Long
        Dim v_strSYSVAR As String = "", v_DataAccess As New DataAccess
        Dim v_strBRID As String = ""
        Dim v_obj As New DataAccess
        Try
            Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes
            If Not (v_attrColl.GetNamedItem(gc_AtributeBRID) Is Nothing) Then
                v_strBRID = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeBRID), Xml.XmlAttribute).Value)
            End If
            'Check HOST Active
            v_DataAccess.NewDBInstance(gc_MODULE_HOST)
            v_lngErrorCode = v_DataAccess.GetSysVar("SYSTEM", "BRSTATUS", v_strBRID, v_strSYSVAR)
            If v_lngErrorCode <> ERR_SYSTEM_OK Then
                'ContextUtil.SetAbort()
                Return v_lngErrorCode
            End If
            If v_strSYSVAR <> OPERATION_ACTIVE Then
                'ContextUtil.SetAbort()
                Return ERR_SY_HOST_OPERATION_ISINACTIVE
            End If

            Dim v_strClause As String
            Dim v_strLocal As String

            If Not (v_attrColl.GetNamedItem(gc_AtributeCLAUSE) Is Nothing) Then
                v_strClause = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCLAUSE), Xml.XmlAttribute).Value)
            Else
                v_strClause = String.Empty
            End If
            If Not (v_attrColl.GetNamedItem(gc_AtributeLOCAL) Is Nothing) Then
                v_strLocal = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeLOCAL), Xml.XmlAttribute).Value)
            Else
                v_strLocal = String.Empty
            End If

            Dim v_strTXDATE As String
            If Not (v_attrColl.GetNamedItem(gc_AtributeTXDATE) Is Nothing) Then
                v_strTXDATE = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTXDATE), Xml.XmlAttribute).Value)
            Else
                v_strTXDATE = String.Empty
            End If
            Dim v_strBUSDATE As String
            If Not (v_attrColl.GetNamedItem(gc_AtributeBUSDATE) Is Nothing) Then
                v_strBUSDATE = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeBUSDATE), Xml.XmlAttribute).Value)
            Else
                v_strBUSDATE = String.Empty
            End If

            Dim v_strBRCODE As String
            If Not (v_attrColl.GetNamedItem(gc_AtributeBRCODE) Is Nothing) Then
                v_strBRCODE = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeBRCODE), Xml.XmlAttribute).Value)
            Else
                v_strBRCODE = String.Empty
            End If
            Dim v_strTLID As String
            If Not (v_attrColl.GetNamedItem(gc_AtributeTLID) Is Nothing) Then
                v_strTLID = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTLID), Xml.XmlAttribute).Value)
            Else
                v_strTLID = String.Empty
            End If
            Dim v_strTLNAME As String
            If Not (v_attrColl.GetNamedItem(gc_AtributeTLNAME) Is Nothing) Then
                v_strTLNAME = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTLNAME), Xml.XmlAttribute).Value)
            Else
                v_strTLNAME = String.Empty
            End If
            Dim v_strIPADDRESS As String
            If Not (v_attrColl.GetNamedItem(gc_AtributeIPADDRESS) Is Nothing) Then
                v_strIPADDRESS = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeIPADDRESS), Xml.XmlAttribute).Value)
            Else
                v_strIPADDRESS = String.Empty
            End If
            Dim v_strWSNAME As String
            If Not (v_attrColl.GetNamedItem(gc_AtributeWSNAME) Is Nothing) Then
                v_strWSNAME = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeWSNAME), Xml.XmlAttribute).Value)
            Else
                v_strWSNAME = String.Empty
            End If
            Dim v_strTXNAME As String
            If Not (v_attrColl.GetNamedItem(gc_AtributeTXNAME) Is Nothing) Then
                v_strTXNAME = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTXNAME), Xml.XmlAttribute).Value)
            Else
                v_strTXNAME = String.Empty
            End If
            Dim v_strVsdBrid As String
            If Not (v_attrColl.GetNamedItem(gc_AtributeVSDBRID) Is Nothing) Then
                v_strVsdBrid = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeVSDBRID), Xml.XmlAttribute).Value)
            Else
                v_strVsdBrid = String.Empty
            End If
            Dim v_strObjectName As String
            If Not (v_attrColl.GetNamedItem(gc_AtributeOBJNAME) Is Nothing) Then
                v_strObjectName = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeOBJNAME), Xml.XmlAttribute).Value)
            Else
                v_strObjectName = String.Empty
            End If

            'Update data

            'If v_strLocal = "Y" Then
            '    v_obj.NewDBInstance(gc_MODULE_BDS)
            'ElseIf v_strLocal = "N" Then
            v_obj.NewDBInstance(gc_MODULE_HOST)
            'End If

            Dim v_strSQL As String, v_strUPD As String = vbNullString, v_strUPDTMP As String = vbNullString
            Dim v_strParentField As String = vbNullString
            Dim v_strParentValues As String = vbNullString
            Dim v_strChildField As String = vbNullString
            Dim v_strChildValues1 As String = vbNullString
            Dim v_strChildValues2 As String = vbNullString
            Dim v_autoIDParent As String
            Dim v_autoIDChild As String
            Dim v_strSQLID As String
            v_strSQLID = "select seq_tllog.NEXTVAL AUTOID from dual"
            Dim v_strTLTXCD As String
            Dim v_strChildTltxcd As String
            Dim v_strParentText As String
            Dim v_strParentTextChild As String
            Dim v_strISPARENT As String
            Dim v_strIsParentChild As String

            If v_strObjectName = "RG.RGIS" Then
                v_strTLTXCD = "2084"
                v_strChildTltxcd = "2085"
            ElseIf v_strObjectName = "RG.RGSI" Then
                v_strTLTXCD = "2086"
                v_strChildTltxcd = "2087"
            ElseIf v_strObjectName = "RG.RGII" Then
                v_strTLTXCD = "2088"
                v_strChildTltxcd = "2089"
            ElseIf v_strObjectName = "RG.RGMI" Then
                v_strTLTXCD = "2090"
                v_strChildTltxcd = "2091"
            Else
                v_strTLTXCD = ""
                v_strChildTltxcd = ""
            End If

            Dim v_dsTLTX As DataSet
            v_strSQL = "SELECT a.* from allcode a where a.cdname = 'ISPARENT' and a.CDTYPE = 'SY' and a.CDVALNO = (SELECT ISPARENT FROM TLTX where TLTXCD='" & v_strTLTXCD & "')"
            v_dsTLTX = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            v_strParentText = v_dsTLTX.Tables(0).Rows(0)("CDCONTENT").ToString
            v_strISPARENT = v_dsTLTX.Tables(0).Rows(0)("CDVALNO").ToString

            v_strSQL = "SELECT a.* from allcode a where a.cdname = 'ISPARENT' and a.CDTYPE = 'SY' and a.CDVALNO = (SELECT ISPARENT FROM TLTX where TLTXCD='" & v_strChildTltxcd & "')"
            v_dsTLTX = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            v_strParentTextChild = v_dsTLTX.Tables(0).Rows(0)("CDCONTENT").ToString
            v_strIsParentChild = v_dsTLTX.Tables(0).Rows(0)("CDVALNO").ToString

            'Insert giao dich cha vao TLLOG
            Dim v_dsParent As DataSet = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLID)
            v_autoIDParent = v_dsParent.Tables(0).Rows(0)("AUTOID")
            v_strParentField = "(autoid, txnum, txdate, brid, tlid, offid, cfrid," _
                                & "chkid, tltxcd, off_line, busdate, txdesc, ipaddress," _
                                & "wsname, status, batchname, micode, deleted, msgamt,isparent,parentid,brcode," _
                                & "tlname,offname,cfrname,chkname,txname,status_text,parent_text,sicode,tmpid,childtltxcd,t_no,msgdate,isbrid,vsd_brid,col_value08)"
            v_strParentValues = "(" & v_autoIDParent & ", GENERATE_TLLOG_CODE(" & v_autoIDParent & "), to_date('" & v_strTXDATE & "','DD/MM/YYYY')," _
                            & "'" & v_strBRID & "', '" & v_strTLID & "','" & v_strTLID & "','" & v_strTLID & "','" & v_strTLID & "','" & v_strTLTXCD & "',null, to_date('" & v_strBUSDATE & "','DD/MM/YYYY'), null," _
                            & "'" & v_strIPADDRESS & "', '" & v_strWSNAME & "'," & CStr(TransactStatus.CONFIRMED_VSD_MANAGER) & ",null,'000',0,0," & v_strISPARENT & ",0,'" & v_strBRCODE & "','" & v_strTLNAME & "','" & v_strTLNAME & "','" & v_strTLNAME & "','" & v_strTLNAME & "'," _
                            & "'" & v_strTXNAME & "','" & CONFIRMED_VSD_MANAGER_TEXT & "','" & v_strParentText & "',null,null,'" & v_strChildTltxcd & "',null,null,1,'" & v_strVsdBrid & "','" & v_strTXNAME & "')"
            v_strSQL = "INSERT INTO TLLOG" & " " & v_strParentField & " VALUES " & v_strParentValues
            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
            v_strSQL = ""
            'End

            'Old Values
            Dim v_dsChild1 As DataSet = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLID)
            v_autoIDChild = v_dsChild1.Tables(0).Rows(0)("AUTOID")
            v_strChildField = "(autoid, txnum, txdate, brid, tlid, offid, cfrid," _
                                & "chkid, tltxcd, off_line, busdate, txdesc, ipaddress," _
                                & "wsname, status, batchname, micode, deleted, msgamt,isparent,parentid,brcode," _
                                & "tlname,offname,cfrname,chkname,txname,status_text,parent_text,sicode,tmpid,childtltxcd,t_no,msgdate,col_value01,col_type01,col_desc01,"
            v_strChildValues1 = "(" & v_autoIDChild & ", GENERATE_TLLOG_CODE(" & v_autoIDChild & "), to_date('" & v_strTXDATE & "','DD/MM/YYYY')," _
                            & "'" & v_strBRID & "', '" & v_strTLID & "','" & v_strTLID & "','" & v_strTLID & "','" & v_strTLID & "','" & v_strChildTltxcd & "',null, to_date('" & v_strBUSDATE & "','DD/MM/YYYY'), null," _
                            & "'" & v_strIPADDRESS & "', '" & v_strWSNAME & "'," & CStr(TransactStatus.CONFIRMED_VSD_MANAGER) & ",null,'000',0,0," & v_strIsParentChild & "," & v_autoIDParent & ",'" & v_strBRCODE & "','" & v_strTLNAME & "','" & v_strTLNAME & "','" & v_strTLNAME & "','" & v_strTLNAME & "'," _
                            & "'" & v_strTXNAME & "','" & CONFIRMED_VSD_MANAGER_TEXT & "','" & v_strParentTextChild & "',null,null,null,null,null,GENERATE_TLLOG_CODE(" & v_autoIDParent & "),'C',GENERATE_TLLOG_CODE(" & v_autoIDParent & "),"
            'End

            Dim v_nodeList As Xml.XmlNodeList, i As Integer
            Dim v_strNewValue As String, v_strOldValue As String, v_strFLDNAME As String, v_strFLDTYPE As String
            Dim v_strCBO As String = ""
            Dim v_strLLIST As String = ""
            Dim v_str_coldesc As String = ""
            Dim v_str_coldesc_new As String = ""
            Dim v_ds As DataSet

            v_nodeList = pv_xmlDocument.SelectNodes("/ObjectMessage/ObjData")

            For i = 0 To v_nodeList.Item(0).ChildNodes.Count - 1
                With v_nodeList.Item(0).ChildNodes(i)
                    v_strFLDNAME = CStr(CType(v_nodeList.Item(0).ChildNodes(i).Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                    v_strFLDTYPE = CStr(CType(v_nodeList.Item(0).ChildNodes(i).Attributes.GetNamedItem("fldtype"), Xml.XmlAttribute).Value)
                    v_strCBO = "select llist from fldmaster where llist is not null and objname = '" & v_strChildTltxcd & "' and defname = " _
                                & "(select defname from fldmaster where objname = '" & v_strObjectName & "' and fldname = '" & v_strFLDNAME & "' and deleted = 0) and deleted = 0"
                    v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strCBO)
                    If v_ds.Tables(0).Rows.Count > 0 Then
                        v_strLLIST = v_ds.Tables(0).Rows(0)("LLIST")
                        v_strLLIST = Replace(v_strLLIST, "+", "||")
                    Else
                        v_strLLIST = ""
                    End If

                    'Old Values
                    v_strOldValue = CStr(CType(v_nodeList.Item(0).ChildNodes(i).Attributes.GetNamedItem("oldval"), Xml.XmlAttribute).Value)
                    If i < 8 Then
                        v_strChildField = v_strChildField & "col_value0" & (i + 2) & ",col_type0" & (i + 2) & ",col_desc0" & (i + 2) & ","
                    Else
                        v_strChildField = v_strChildField & "col_value" & (i + 2) & ",col_type" & (i + 2) & ",col_desc" & (i + 2) & ","
                    End If
                    If Len(v_strOldValue) > 0 Then
                        If Len(v_strLLIST) > 0 Then
                            v_strCBO = "select a.display from (" & v_strLLIST & ") a where a.value ='" & v_strOldValue & "'"
                            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strCBO)
                            If v_ds.Tables(0).Rows.Count > 0 Then
                                v_str_coldesc = Convert.ToString(v_ds.Tables(0).Rows(0)("DISPLAY"))
                            Else
                                v_str_coldesc = v_strOldValue
                            End If
                        Else
                            v_str_coldesc = v_strOldValue
                        End If
                        Select Case v_strFLDTYPE
                            Case "System.String"
                                v_strChildValues1 = v_strChildValues1 & "'" & Replace(v_strOldValue, "'", "''") & "','C','" & Replace(v_str_coldesc, "'", "''") & "',"
                            Case "System.DateTime"
                                v_strChildValues1 = v_strChildValues1 & "TO_DATE('" & v_strOldValue & "', '" & gc_FORMAT_DATE & "'),'D',TO_DATE('" & v_str_coldesc & "', '" & gc_FORMAT_DATE & "'),"
                            Case GetType(Double).Name
                                If Len(v_strLLIST) > 0 Then
                                    v_strChildValues1 = v_strChildValues1 & Replace(v_strOldValue, ",", "") & ",'N','" & Replace(v_str_coldesc, ",", "") & "',"
                                Else
                                    v_strChildValues1 = v_strChildValues1 & Replace(v_strOldValue, ",", "") & ",'N'," & Replace(v_str_coldesc, ",", "") & ","
                                End If
                            Case Else
                                If Len(v_strLLIST) > 0 Then
                                    v_strChildValues1 = v_strChildValues1 & v_strOldValue & ",null,'" & v_str_coldesc & "',"
                                Else
                                    v_strChildValues1 = v_strChildValues1 & v_strOldValue & ",null," & v_str_coldesc & ","
                                End If
                        End Select
                    Else
                        v_strChildValues1 = v_strChildValues1 & "null,null,null,"
                    End If
                    'End
                    'New Values
                    v_strNewValue = .InnerText.ToString
                    If Len(v_strNewValue) > 0 Then
                        If Trim(v_strOldValue) <> Trim(v_strNewValue) Then
                            If Len(v_strLLIST) > 0 Then
                                v_strCBO = "select a.display from (" & v_strLLIST & ") a where a.value ='" & v_strNewValue & "'"
                                v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strCBO)
                                If v_ds.Tables(0).Rows.Count > 0 Then
                                    v_str_coldesc_new = Convert.ToString(v_ds.Tables(0).Rows(0)("DISPLAY"))
                                Else
                                    v_str_coldesc_new = v_strNewValue
                                End If
                            Else
                                v_str_coldesc_new = v_strNewValue
                            End If
                            Select Case v_strFLDTYPE
                                Case "System.String"
                                    v_strChildValues2 = v_strChildValues2 & "'" & Replace(v_strNewValue, "'", "''") & "','C','" & Replace(v_str_coldesc_new, "'", "''") & "',"
                                Case "System.DateTime"
                                    v_strChildValues2 = v_strChildValues2 & "TO_DATE('" & v_strNewValue & "', '" & gc_FORMAT_DATE & "'),'D',TO_DATE('" & v_str_coldesc_new & "', '" & gc_FORMAT_DATE & "'),"
                                Case GetType(Double).Name
                                    If Len(v_strLLIST) > 0 Then
                                        v_strChildValues2 = v_strChildValues2 & Replace(v_strNewValue, ",", "") & ",'N','" & Replace(v_str_coldesc_new, ",", "") & "',"
                                    Else
                                        v_strChildValues2 = v_strChildValues2 & Replace(v_strNewValue, ",", "") & ",'N'," & Replace(v_str_coldesc_new, ",", "") & ","
                                    End If
                                Case Else
                                    If Len(v_strLLIST) > 0 Then
                                        v_strChildValues2 = v_strChildValues2 & v_strNewValue & ",null,'" & v_str_coldesc_new & "',"
                                    Else
                                        v_strChildValues2 = v_strChildValues2 & v_strNewValue & ",null," & v_str_coldesc_new & ","
                                    End If
                            End Select
                        Else
                            If Len(v_strOldValue) > 0 Then
                                If Len(v_strLLIST) > 0 Then
                                    v_strCBO = "select a.display from (" & v_strLLIST & ") a where a.value ='" & v_strOldValue & "'"
                                    v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strCBO)
                                    If v_ds.Tables(0).Rows.Count > 0 Then
                                        v_str_coldesc_new = Convert.ToString(v_ds.Tables(0).Rows(0)("DISPLAY"))
                                    Else
                                        v_str_coldesc_new = v_strOldValue
                                    End If
                                Else
                                    v_str_coldesc_new = v_strOldValue
                                End If
                                Select Case v_strFLDTYPE
                                    Case "System.String"
                                        v_strChildValues2 = v_strChildValues2 & "'" & Replace(v_strOldValue, "'", "''") & "','C','" & Replace(v_str_coldesc_new, "'", "''") & "',"
                                    Case "System.DateTime"
                                        v_strChildValues2 = v_strChildValues2 & "TO_DATE('" & v_strOldValue & "', '" & gc_FORMAT_DATE & "'),'D',TO_DATE('" & v_str_coldesc_new & "', '" & gc_FORMAT_DATE & "'),"
                                    Case GetType(Double).Name
                                        If Len(v_strLLIST) > 0 Then
                                            v_strChildValues2 = v_strChildValues2 & Replace(v_strOldValue, ",", "") & ",'N','" & Replace(v_str_coldesc_new, ",", "") & "',"
                                        Else
                                            v_strChildValues2 = v_strChildValues2 & Replace(v_strOldValue, ",", "") & ",'N'," & Replace(v_str_coldesc_new, ",", "") & ","
                                        End If
                                    Case Else
                                        If Len(v_strLLIST) > 0 Then
                                            v_strChildValues2 = v_strChildValues2 & v_strOldValue & ",null,'" & v_str_coldesc_new & "',"
                                        Else
                                            v_strChildValues2 = v_strChildValues2 & v_strOldValue & ",null," & v_str_coldesc_new & ","
                                        End If
                                End Select
                            Else
                                v_strChildValues2 = v_strChildValues2 & "null,null,null,"
                            End If
                        End If
                    Else
                        v_strChildValues2 = v_strChildValues2 & "null,null,null,"
                    End If
                    'End
                End With
            Next

            'Old Values
            v_strChildField = v_strChildField & "isbrid,vsd_brid,col_value40,col_type40,col_desc40)"
            v_strChildValues1 = v_strChildValues1 & "'1','" & v_strVsdBrid & "','Giao dịch gốc','C','Giao dịch gốc')"
            v_strSQL = "INSERT INTO TLLOG" & " " & v_strChildField & " VALUES " & v_strChildValues1
            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
            v_strSQL = ""
            'End

            'New Values
            Dim v_dsChild2 As DataSet = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLID)
            v_autoIDChild = v_dsChild2.Tables(0).Rows(0)("AUTOID")
            v_strChildValues2 = "(" & v_autoIDChild & ", GENERATE_TLLOG_CODE(" & v_autoIDChild & "), to_date('" & v_strTXDATE & "','DD/MM/YYYY')," _
                            & "'" & v_strBRID & "', '" & v_strTLID & "','" & v_strTLID & "','" & v_strTLID & "','" & v_strTLID & "','" & v_strChildTltxcd & "',null, to_date('" & v_strBUSDATE & "','DD/MM/YYYY'), null," _
                            & "'" & v_strIPADDRESS & "', '" & v_strWSNAME & "'," & CStr(TransactStatus.CONFIRMED_VSD_MANAGER) & ",null,'000',0,0," & v_strIsParentChild & "," & v_autoIDParent & ",'" & v_strBRCODE & "','" & v_strTLNAME & "','" & v_strTLNAME & "','" & v_strTLNAME & "','" & v_strTLNAME & "'," _
                            & "'" & v_strTXNAME & "','" & CONFIRMED_VSD_MANAGER_TEXT & "','" & v_strParentTextChild & "',null,null,null,null,null,GENERATE_TLLOG_CODE(" & v_autoIDParent & "),'C',GENERATE_TLLOG_CODE(" & v_autoIDParent & ")," & v_strChildValues2
            v_strChildValues2 = v_strChildValues2 & "'1','" & v_strVsdBrid & "','Giao dịch sau khi sửa','C','Giao dịch sau khi sửa')"
            v_strSQL = "INSERT INTO TLLOG" & " " & v_strChildField & " VALUES " & v_strChildValues2
            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
            'End

            'ContextUtil.SetComplete()
            'v_obj.Dispose()
            Return 0

        Catch ex As Exception
            Throw ex
        Finally
            If Not v_obj Is Nothing Then
                v_obj.Dispose()
            End If
            If Not v_DataAccess Is Nothing Then
                v_DataAccess.Dispose()
            End If
        End Try
    End Function
    'End
    Public Function CoreDelete(ByRef pv_xmlDocument As XmlDocumentEx) As Long
        Dim v_lngErrorCode As Long
        Dim v_strSYSVAR As String = "", v_DataAccess As New DataAccess
        Dim v_strBRID As String = ""
        Dim v_obj As DataAccess = Nothing
        Try
            Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes
            If Not (v_attrColl.GetNamedItem(gc_AtributeBRID) Is Nothing) Then
                v_strBRID = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeBRID), Xml.XmlAttribute).Value)
            End If

            'Check HOST Active
            v_DataAccess.NewDBInstance(gc_MODULE_HOST)
            v_lngErrorCode = v_DataAccess.GetSysVar("SYSTEM", "BRSTATUS", v_strBRID, v_strSYSVAR)
            If v_lngErrorCode <> ERR_SYSTEM_OK Then
                'ContextUtil.SetAbort()
                Return v_lngErrorCode
            End If
            If v_strSYSVAR <> OPERATION_ACTIVE Then
                'ContextUtil.SetAbort()
                Return ERR_SY_HOST_OPERATION_ISINACTIVE
            End If

            v_lngErrorCode = CheckBeforeDelete(pv_xmlDocument)
            If v_lngErrorCode <> 0 Then
                'ContextUtil.SetAbort()
                Return v_lngErrorCode
                Exit Function
            End If


            Dim v_strClause As String
            Dim v_strLocal As String
            Dim v_strSICODE As String

            If Not (v_attrColl.GetNamedItem(gc_AtributeCLAUSE) Is Nothing) Then
                v_strClause = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCLAUSE), Xml.XmlAttribute).Value)
            Else
                v_strClause = String.Empty
            End If
            If Not (v_attrColl.GetNamedItem(gc_AtributeLOCAL) Is Nothing) Then
                v_strLocal = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeLOCAL), Xml.XmlAttribute).Value)
            Else
                v_strLocal = String.Empty
            End If

            'Delete data
            If v_strLocal = "Y" Then
                v_obj = New DataAccess
            ElseIf v_strLocal = "N" Then
                v_obj = New DataAccess
                v_obj.NewDBInstance(gc_MODULE_HOST)
            End If

            Dim v_strSQL As String

            If v_obj.CheckExitsField(ATTR_TABLE) Then
                v_strSQL = "UPDATE " & ATTR_TABLE & " SET DELETED='1' WHERE 0=0 AND " & v_strClause
            Else
                v_strSQL = "DELETE FROM " & ATTR_TABLE & " WHERE 0=0 AND " & v_strClause
            End If

            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
            'ContextUtil.SetComplete()
            Return 0
        Catch ex As Exception
            Throw ex
        Finally
            If Not v_DataAccess Is Nothing Then
                v_DataAccess.Dispose()
            End If
            If Not v_obj Is Nothing Then
                v_obj.Dispose()
            End If
        End Try
    End Function
    'Added by Thanglv9 - 14/12/2012
    Public Function CoreDeleteToLogAfterDel(ByRef pv_xmlDocument As XmlDocumentEx) As Long
        Dim v_lngErrorCode As Long
        Dim v_strSYSVAR As String = "", v_DataAccess As New DataAccess
        Dim v_strBRID As String = ""
        Dim v_obj As New DataAccess
        Try
            Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes
            If Not (v_attrColl.GetNamedItem(gc_AtributeBRID) Is Nothing) Then
                v_strBRID = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeBRID), Xml.XmlAttribute).Value)
            End If
            'Check HOST Active
            v_DataAccess.NewDBInstance(gc_MODULE_HOST)
            v_lngErrorCode = v_DataAccess.GetSysVar("SYSTEM", "BRSTATUS", v_strBRID, v_strSYSVAR)
            If v_lngErrorCode <> ERR_SYSTEM_OK Then
                'ContextUtil.SetAbort()
                Return v_lngErrorCode
            End If
            If v_strSYSVAR <> OPERATION_ACTIVE Then
                'ContextUtil.SetAbort()
                Return ERR_SY_HOST_OPERATION_ISINACTIVE
            End If

            Dim v_strClause As String
            Dim v_strLocal As String

            If Not (v_attrColl.GetNamedItem(gc_AtributeCLAUSE) Is Nothing) Then
                v_strClause = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCLAUSE), Xml.XmlAttribute).Value)
            Else
                v_strClause = String.Empty
            End If
            If Not (v_attrColl.GetNamedItem(gc_AtributeLOCAL) Is Nothing) Then
                v_strLocal = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeLOCAL), Xml.XmlAttribute).Value)
            Else
                v_strLocal = String.Empty
            End If

            Dim v_strTXDATE As String
            If Not (v_attrColl.GetNamedItem(gc_AtributeTXDATE) Is Nothing) Then
                v_strTXDATE = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTXDATE), Xml.XmlAttribute).Value)
            Else
                v_strTXDATE = String.Empty
            End If
            Dim v_strBUSDATE As String
            If Not (v_attrColl.GetNamedItem(gc_AtributeBUSDATE) Is Nothing) Then
                v_strBUSDATE = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeBUSDATE), Xml.XmlAttribute).Value)
            Else
                v_strBUSDATE = String.Empty
            End If

            Dim v_strBRCODE As String
            If Not (v_attrColl.GetNamedItem(gc_AtributeBRCODE) Is Nothing) Then
                v_strBRCODE = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeBRCODE), Xml.XmlAttribute).Value)
            Else
                v_strBRCODE = String.Empty
            End If
            Dim v_strTLID As String
            If Not (v_attrColl.GetNamedItem(gc_AtributeTLID) Is Nothing) Then
                v_strTLID = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTLID), Xml.XmlAttribute).Value)
            Else
                v_strTLID = String.Empty
            End If
            Dim v_strTLNAME As String
            If Not (v_attrColl.GetNamedItem(gc_AtributeTLNAME) Is Nothing) Then
                v_strTLNAME = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTLNAME), Xml.XmlAttribute).Value)
            Else
                v_strTLNAME = String.Empty
            End If
            Dim v_strIPADDRESS As String
            If Not (v_attrColl.GetNamedItem(gc_AtributeIPADDRESS) Is Nothing) Then
                v_strIPADDRESS = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeIPADDRESS), Xml.XmlAttribute).Value)
            Else
                v_strIPADDRESS = String.Empty
            End If
            Dim v_strWSNAME As String
            If Not (v_attrColl.GetNamedItem(gc_AtributeWSNAME) Is Nothing) Then
                v_strWSNAME = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeWSNAME), Xml.XmlAttribute).Value)
            Else
                v_strWSNAME = String.Empty
            End If
            Dim v_strTXNAME As String
            If Not (v_attrColl.GetNamedItem(gc_AtributeTXNAME) Is Nothing) Then
                v_strTXNAME = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTXNAME), Xml.XmlAttribute).Value)
            Else
                v_strTXNAME = String.Empty
            End If
            Dim v_strVsdBrid As String
            If Not (v_attrColl.GetNamedItem(gc_AtributeVSDBRID) Is Nothing) Then
                v_strVsdBrid = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeVSDBRID), Xml.XmlAttribute).Value)
            Else
                v_strVsdBrid = String.Empty
            End If
            Dim v_strObjectName As String
            If Not (v_attrColl.GetNamedItem(gc_AtributeOBJNAME) Is Nothing) Then
                v_strObjectName = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeOBJNAME), Xml.XmlAttribute).Value)
            Else
                v_strObjectName = String.Empty
            End If

            'Update data

            v_obj.NewDBInstance(gc_MODULE_HOST)
            'End If

            Dim v_strSQL As String, v_strUPD As String = vbNullString, v_strUPDTMP As String = vbNullString
            Dim v_strTLTXCD As String
            Dim v_strSicode As String
            

            If v_strObjectName = "RG.RGIS" Then
                v_strTLTXCD = "2080"
            ElseIf v_strObjectName = "RG.RGSI" Then
                v_strTLTXCD = "2081"
            ElseIf v_strObjectName = "RG.RGMI" Then
                v_strTLTXCD = "2083"
            Else
                v_strTLTXCD = ""
            End If

            Dim v_dsSicode As DataSet
            v_strSQL = "select distinct sicode from " & ATTR_TABLE & " WHERE 1=1 AND " & v_strClause
            v_dsSicode = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            v_strSicode = v_dsSicode.Tables(0).Rows(0)("SICODE").ToString

            v_strSQL = "UPDATE tllog SET status='4' WHERE 0=0 AND tltxcd ='" & v_strTLTXCD & "' AND col_value03 ='" & v_strSicode & "'"
            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
            v_strSQL = ""
            'End
            Return 0

        Catch ex As Exception
            Throw ex
        Finally
            If Not v_obj Is Nothing Then
                v_obj.Dispose()
            End If
            If Not v_DataAccess Is Nothing Then
                v_DataAccess.Dispose()
            End If
        End Try
    End Function

    Public Function CoreInquiry(ByRef pv_xmlDocument As XmlDocumentEx) As Long
        Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes

        Dim v_obj As New DataAccess
        Dim v_ds As DataSet

        Dim v_strSysSearchStatus As Integer = 1

        Dim v_strMaxRowSearch As String = "0"
        Dim v_strLocal As String, v_strClause As String
        Dim v_lngError As Long = ERR_SYSTEM_OK
        Try
            If Not (v_attrColl.GetNamedItem(gc_AtributeLOCAL) Is Nothing) Then
                v_strLocal = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeLOCAL), Xml.XmlAttribute).Value)
            Else
                v_strLocal = String.Empty
            End If

            If Not (v_attrColl.GetNamedItem(gc_AtributeFUNCNAME) Is Nothing) Then
                v_strClause = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeFUNCNAME), Xml.XmlAttribute).Value)
            Else
                v_strClause = String.Empty
            End If

            Dim v_intInqueryType As Integer = 0
            If v_strClause <> "" Then
                Dim v_arrClause() As String = v_strClause.Split("|")
                v_intInqueryType = CInt(v_arrClause(2))
            End If

            v_obj.GetSysVar("SYSTEM", "SYS_SEARCH_STATUS", v_strSysSearchStatus)
            v_obj.GetSysVar("SYSTEM", "MAX_ROW_SEARCH", v_strMaxRowSearch)

            If v_strSysSearchStatus <> "0" Then
                If v_intInqueryType = 0 Then
                    If v_strLocal = gc_IsLocalMsg Then
                        v_ds = CoreBaseInquery(pv_xmlDocument)
                    Else
                        v_ds = CoreHostInquery(pv_xmlDocument)
                    End If
                    'Tao DL de tra ve Client
                    Dim v_dataElement As Xml.XmlElement, v_entryNode As Xml.XmlNode

                    Dim v_attrFLDNAME As Xml.XmlAttribute, v_attrFLDTYPE As Xml.XmlAttribute, v_attrOLDVAL As Xml.XmlAttribute

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
                                If v_ds.Tables(0).Rows(i)(j).GetType.Name = GetType(System.DateTime).Name _
                                    Or v_ds.Tables(0).Rows(i)(j).GetType.Name = GetType(System.String).Name Then
                                    v_attrOLDVAL.Value = ""
                                Else
                                    v_attrOLDVAL.Value = "0"
                                End If

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
                Else
                    Dim v_strFileExportPath As String = CoreHostExport(pv_xmlDocument)

                    'Tao DL de tra ve Client
                    Dim v_dataElement As Xml.XmlElement, v_entryNode As Xml.XmlNode
                    Dim v_attrFLDNAME As Xml.XmlAttribute, v_attrFLDTYPE As Xml.XmlAttribute, v_attrOLDVAL As Xml.XmlAttribute

                    v_dataElement = pv_xmlDocument.CreateElement(Xml.XmlNodeType.Element, "ExportMsg", "")

                    'Append entry to data node
                    v_entryNode = pv_xmlDocument.CreateNode(Xml.XmlNodeType.Element, "Entry", "")
                    v_attrFLDNAME = pv_xmlDocument.CreateAttribute("fldname")
                    v_attrFLDNAME.Value = "EXPORT_RESULT"
                    v_entryNode.Attributes.Append(v_attrFLDNAME)

                    v_attrOLDVAL = pv_xmlDocument.CreateAttribute("msg")
                    v_attrOLDVAL.Value = "EXPORT_RESULT"
                    v_entryNode.Attributes.Append(v_attrOLDVAL)

                    v_entryNode.InnerText = v_strFileExportPath
                    v_dataElement.AppendChild(v_entryNode)
                    pv_xmlDocument.DocumentElement.AppendChild(v_dataElement)

                End If
            Else
                Dim v_dataElement As Xml.XmlElement, v_entryNode As Xml.XmlNode
                Dim v_attrFLDNAME As Xml.XmlAttribute, v_attrFLDTYPE As Xml.XmlAttribute, v_attrOLDVAL As Xml.XmlAttribute

                v_dataElement = pv_xmlDocument.CreateElement(Xml.XmlNodeType.Element, "SysSearchStatusMsg", "")

                'Append entry to data node
                v_entryNode = pv_xmlDocument.CreateNode(Xml.XmlNodeType.Element, "Entry", "")
                v_attrFLDNAME = pv_xmlDocument.CreateAttribute("fldname")
                v_attrFLDNAME.Value = "SYS_SEARCH_STATUS"
                v_entryNode.Attributes.Append(v_attrFLDNAME)

                v_attrOLDVAL = pv_xmlDocument.CreateAttribute("msg")
                v_attrOLDVAL.Value = "SYS_SEARCH_STATUS"
                v_entryNode.Attributes.Append(v_attrOLDVAL)

                v_entryNode.InnerText = "0"
                v_dataElement.AppendChild(v_entryNode)
                pv_xmlDocument.DocumentElement.AppendChild(v_dataElement)
            End If

            If v_lngError <> 0 Then
                Dim v_strErrorSource, v_strErrorMessage As String
                v_strErrorSource = "SISTORES"
                v_strErrorMessage = String.Empty
                BuildXMLErrorException(pv_xmlDocument, v_strErrorSource, v_lngError, v_strErrorMessage)
            End If

            Return v_lngError
            'ContextUtil.SetComplete()
        Catch ex As Exception
            'ContextUtil.SetAbort()
            Throw ex
        Finally
            If Not v_obj Is Nothing Then
                v_obj.Dispose()
            End If
            If Not v_ds Is Nothing Then
                v_ds.Dispose()
            End If
            GC.Collect()
        End Try
    End Function

    Public Function GetSearch4STP(ByVal pv_xmlDocument As XmlDocumentEx) As DataSet
        Return CoreHostInquery(pv_xmlDocument)
    End Function

    Private Function DecodeSQL(ByVal pv_strSQL As String) As String
        Dim v_strTmpSql, v_strTmpSql1 As String
        Dim v_intStart, v_intEnd As Integer

        Try
            While InStr(pv_strSQL, "[#", CompareMethod.Text) > 0
                v_strTmpSql1 = "1 = 1"
                v_intStart = InStr(1, pv_strSQL, "[#")
                v_intEnd = InStr(1, pv_strSQL, "#]")
                v_strTmpSql = Mid(pv_strSQL, v_intStart, v_intEnd - v_intStart + 2)
                If InStr(v_strTmpSql, "[!", CompareMethod.Text) = 0 Then
                    v_strTmpSql1 = Replace(v_strTmpSql, "[#", "")
                    v_strTmpSql1 = Replace(v_strTmpSql1, "#]", "")
                End If
                pv_strSQL = Replace(pv_strSQL, v_strTmpSql, v_strTmpSql1)
            End While
            pv_strSQL = Replace(pv_strSQL, "[#", "")
            pv_strSQL = Replace(pv_strSQL, "#]", "")

            Return pv_strSQL
        Catch ex As Exception
            Throw ex
        End Try
    End Function

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
                            If Mid(v_strTran, 1, 6) <> "TMP" Then
                                Return False
                            End If

                            v_strTmp = Replace(v_strTmp, "TRAN", "")
                        End If
                        v_int = InStr(v_strTmp, "TLLOG", CompareMethod.Text)

                        If v_int > 0 Then
                            v_strTran = Mid(v_strTmp, v_int - 7, 11)

                            If Mid(v_strTran, 1, 6) <> "TMP" Then
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

    'Public Function CoreInquiryOver(ByRef pv_xmlDocument As XmlDocumentEx) As Long
    '    Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes

    '    Dim v_strClause As String
    '    Dim v_strLocal As String
    '    Dim v_strObjName As String
    '    Dim v_strCmdInquiry As String
    '    Dim v_strCondition As String
    '    Dim v_strBRID As String
    '    Dim v_obj As New TTDataAccess
    '    'Dim v_obj As Object = Nothing
    '    Dim v_arrField() As String

    '    Dim v_ds As DataSet
    '    Dim v_strSQL, v_strSQLTmp As String
    '    Dim v_bln As Boolean
    '    Dim tr2 As TextWriterTraceListener
    '    Dim v_trace_status, v_trace_path As String
    '    Dim v_lstBrID As String
    '    Dim v_ds1 As DataSet
    '    Dim v_trace As DataSet
    '    Dim v_lngError As Long = ERR_SYSTEM_OK

    '    Try
    '        If Not (v_attrColl.GetNamedItem(gc_AtributeCMDINQUIRY) Is Nothing) Then
    '            v_strCmdInquiry = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCMDINQUIRY), Xml.XmlAttribute).Value)
    '        Else
    '            v_strCmdInquiry = String.Empty
    '        End If

    '        If Not (v_attrColl.GetNamedItem(gc_AtributeFUNCNAME) Is Nothing) Then
    '            v_strClause = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeFUNCNAME), Xml.XmlAttribute).Value)
    '        Else
    '            v_strClause = String.Empty
    '        End If

    '        If Not (v_attrColl.GetNamedItem(gc_AtributeLOCAL) Is Nothing) Then
    '            v_strLocal = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeLOCAL), Xml.XmlAttribute).Value)
    '        Else
    '            v_strLocal = String.Empty
    '        End If
    '        If Not (v_attrColl.GetNamedItem(gc_AtributeCLAUSE) Is Nothing) Then
    '            v_strCondition = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCLAUSE), Xml.XmlAttribute).Value)
    '        Else
    '            v_strCondition = String.Empty
    '        End If

    '        If Not (v_attrColl.GetNamedItem(gc_AtributeOBJNAME) Is Nothing) Then
    '            v_strObjName = Mid(CStr(CType(v_attrColl.GetNamedItem(gc_AtributeOBJNAME), Xml.XmlAttribute).Value), 4)
    '        Else
    '            v_strObjName = ATTR_TABLE
    '        End If

    '        If Not (v_attrColl.GetNamedItem(gc_AtributeAUTOID) Is Nothing) Then
    '            v_bln = CBool(CType(v_attrColl.GetNamedItem(gc_AtributeAUTOID), Xml.XmlAttribute).Value)
    '        Else
    '            v_bln = False
    '        End If
    '        Dim v_blnAll As Boolean
    '        If Not (v_attrColl.GetNamedItem(gc_AtributeTXTIME) Is Nothing) Then
    '            v_blnAll = CBool(CType(v_attrColl.GetNamedItem(gc_AtributeTXTIME), Xml.XmlAttribute).Value)
    '        Else
    '            v_blnAll = True
    '        End If

    '        'Neu su dung qua form frmsearch
    '        If v_bln Then
    '            v_strBRID = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeBRID), Xml.XmlAttribute).Value)
    '            Dim v_strTellerID As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTLID), Xml.XmlAttribute).Value)
    '            Dim v_arrFilter As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTXNUM), Xml.XmlAttribute).Value)
    '            Dim v_blnSearch As Boolean = CBool(CType(v_attrColl.GetNamedItem(gc_AtributeREFERENCE), Xml.XmlAttribute).Value)
    '            Dim v_strCurrDate As String

    '            v_trace_status = "0"
    '            'v_trace_path = "C:\log_sisearch_sql_data.txt"
    '            v_obj.GetSysVar("SYSTEM", "TRACE_STATUS", v_trace_status)

    '            If v_trace_status = "1" Then
    '                v_obj.GetSysVar("SYSTEM", "CURRDATE", v_strBRID, v_strCurrDate)
    '                v_obj.GetSysVar("SYSTEM", "TRACE_PATH", v_trace_path)
    '                Dim v_strTLName As String = v_obj.GetValue("TLPROFILES", "TLNAME", "TLID='" & v_strTellerID & "'")
    '                v_strCurrDate = Replace(v_strCurrDate, "/", "_")
    '                If v_trace_path = "" Then
    '                    Dim v_app As New ApplicationServices.ApplicationBase
    '                    v_trace_path = v_app.Info.DirectoryPath & "\Log\" & v_strCurrDate
    '                Else
    '                    v_trace_path = v_trace_path & v_strCurrDate
    '                End If

    '                If Not System.IO.Directory.Exists(v_trace_path) Then
    '                    System.IO.Directory.CreateDirectory(v_trace_path)
    '                End If

    '                v_trace_path &= "\log_search_br" & v_strBRID & "_" & v_strTLName & ".txt"

    '                tr2 = New TextWriterTraceListener(System.IO.File.AppendText(v_trace_path))
    '                Trace.Listeners.Add(tr2)
    '                Trace.WriteLine("[Bắt đầu: SI - " & v_strObjName & "] " & DateTime.Now & vbCrLf)
    '            End If


    '            'Xu ly DL bang TRAN, TLLOG
    '            v_strSQL = "SELECT TLLOGTRAN, LOADALL FROM SISEARCH WHERE SEARCHCODE = '" & v_strObjName & "'"
    '            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

    '            Dim v_strTran As String = "" 'IA|MA|CA|MF|RA|RG|SF|TLLOG
    '            Dim v_lstTran() As String
    '            Dim v_blnLoadAll As Boolean = False

    '            If v_ds.Tables(0).Rows.Count > 0 Then
    '                v_strTran = v_ds.Tables(0).Rows(0)("TLLOGTRAN").ToUpper
    '                If v_ds.Tables(0).Rows(0)("LOADALL").ToUpper = "Y" Then
    '                    v_blnLoadAll = True
    '                End If
    '            End If

    '            v_blnSearch = v_blnSearch Or v_blnLoadAll

    '            Dim v_strTranWhere As String = ""
    '            Dim v_strTllogWhere As String = ""
    '            Dim v_strCAWhere As String = ""
    '            v_strCAWhere = " AND [#MSGDATE = TO_DATE('[!V_MSGDATE]','" & gc_FORMAT_DATE & "')#]"

    '            v_strTranWhere = " AND [#TXDATE [!O_TXDATE] TO_DATE('[!V_TXDATE]','" & gc_FORMAT_DATE & "')#] AND [#TXNUM [!O_TXNUM] '[!V_TXNUM]'#]"
    '            If v_blnSearch Then
    '                v_strTllogWhere = " AND [#TXDATE [!O_TXDATE] TO_DATE('[!V_TXDATE]','" & gc_FORMAT_DATE & "')#] AND [#TXNUM [!O_TXNUM] '[!V_TXNUM]'#]"
    '                v_strTllogWhere &= " AND [#BUSDATE [!O_TXDATE] TO_DATE('[!V_BUSDATE]','" & gc_FORMAT_DATE & "')#] AND [#TLTXCD [!O_TLTXCD] '[!V_TLTXCD]'#]"
    '            End If

    '            If v_strTran <> "" Then
    '                v_lstTran = v_strTran.Split("|")
    '                v_strTran = ""
    '                For i As Integer = 0 To v_lstTran.Length - 1
    '                    Select Case v_lstTran(i)
    '                        Case "TLLOG"
    '                            If v_blnAll Then
    '                                v_strTran &= ",'TLLOG'"
    '                            End If
    '                            If v_blnSearch Then
    '                                v_strTran &= ",'TLLOGALL'"
    '                            End If
    '                        Case "CA", "IA", "MA", "RA", "SF"
    '                            v_strTran &= ",'" & v_lstTran(i) & "TRAN'"
    '                            If v_lstTran(i) <> "CA" Then
    '                                v_strTran &= ",'" & v_lstTran(i) & "MAST'"
    '                            End If
    '                            If v_blnSearch Then
    '                                v_strTran &= ",'" & v_lstTran(i) & "TRANA'"
    '                            End If
    '                        Case "RG", "MF"
    '                            v_strTran &= ",'" & v_lstTran(i) & "TRAN'"
    '                            If v_blnSearch Then
    '                                v_strTran &= ",'" & v_lstTran(i) & "TRANA'"
    '                            End If
    '                    End Select
    '                Next
    '                v_strTran = "(" & Mid(v_strTran, 2) & ")"
    '            End If

    '            Dim v_lstSICODE, v_lstMICODE As String

    '            v_lstMICODE = v_arrFilter.Split("|")(1)
    '            v_lstSICODE = v_arrFilter.Split("|")(0)

    '            Dim v_intCount As Integer = -1

    '            If v_strCondition <> "" Then
    '                v_arrField = v_strCondition.Split("#")
    '                v_intCount = v_arrField.Length - 2
    '            End If
    '            Dim v_hCondition As New Hashtable
    '            Dim v_strhKey, v_strhValue As String
    '            Dim v_blnReplace As Boolean
    '            If v_intCount >= 0 Then
    '                For j = 0 To v_intCount
    '                    v_strhKey = v_arrField(j).Split("|")(0).ToUpper
    '                    Select Case v_strhKey
    '                        Case "SICODE"
    '                            v_lstSICODE = "('" & v_arrField(j).Split("|")(2) & "')"
    '                        Case "MICODE"
    '                            v_lstMICODE = "('" & v_arrField(j).Split("|")(2) & "')"
    '                    End Select
    '                Next
    '            End If
    '            'Lay phan quyen chi nhanh
    '            v_strSQL = "SELECT DISTINCT brid brid  FROM " _
    '                   & " (SELECT b.brid FROM tlbridauth a, brgrp b" _
    '                   & " WHERE ((AUTHID = '" & v_strTellerID & "' AND authtype = 'U') OR (AUTHID IN (SELECT a.grpid" _
    '                   & " FROM tlgrpusers a WHERE a.tlid = '" & v_strTellerID & "') AND authtype = 'G'))" _
    '                   & " And a.brid = b.brid And a.deleted = 0 And a.status = 0" _
    '                   & " And b.deleted = 0 And b.status = 0" _
    '                   & " UNION " _
    '                   & " SELECT b.brid FROM tlprofiles a, brgrp b" _
    '                   & " WHERE a.brid = b.brid AND a.tlid = '" & v_strTellerID & "'" _
    '                   & " AND a.deleted = 0 AND a.status = 0 AND b.deleted = 0 AND b.status = 0)"

    '            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

    '            v_lstBrID = ""
    '            For i = 0 To v_ds.Tables(0).Rows.Count - 1
    '                v_lstBrID &= ",'" & v_ds.Tables(0).Rows(i)(0) & "'"
    '            Next
    '            v_lstBrID = "(" & Mid(v_lstBrID, 2) & ")"

    '            'Lay TVLK dc phan quyen cho user vao bang TMP_RGMI
    '            'v_strSQL = "INSERT INTO TMP_rgmi " _
    '            '            & " SELECT DISTINCT m.* FROM rgmi m, tlmemauth a" _
    '            '            & " WHERE ((a.AUTHID = '" & v_strTellerID & "' AND a.authtype = 'U')" _
    '            '            & " OR (a.AUTHID IN (SELECT grpid FROM tlgrpusers WHERE tlid = '" & v_strTellerID & "')" _
    '            '            & " AND a.authtype = 'G')) AND m.deleted = 0 AND m.status = 0" _
    '            '            & " AND m.micode = a.micode AND m.deleted=0 AND m.status=0"
    '            v_strSQL = "INSERT INTO TMP_rgmi " _
    '                        & " SELECT DISTINCT m.* FROM rgmi m" _
    '                        & " WHERE m.deleted = 0 AND m.status = 0" _
    '                        & " AND m.micode IN " & v_lstMICODE & " AND m.deleted=0 AND m.status=0"
    '            v_obj.ExecuteNonQueryTran(CommandType.Text, v_strSQL)

    '            v_strSQL = "INSERT INTO TMP_RGMI(AUTOID, MICODE) SELECT '0' AUTOID, '000' MICODE FROM DUAL"
    '            v_obj.ExecuteNonQueryTran(CommandType.Text, v_strSQL)

    '            'Lay chung khoan dc phan quyen cho user vao bang TMP_RGSI
    '            'v_strSQL = "INSERT INTO TMP_rgsi(iscode, sicode, stock_name, part_value, TYPE," _
    '            '            & "stock_type, interest_rate, interest_period, " _
    '            '            & "bond_period, deleted, exchange_rate, note, " _
    '            '            & "bond_term, release_series, release_mode, isin, " _
    '            '            & "rarate, tmpid, paidmethod, npaiddate1, npaiddate2, " _
    '            '            & "npaiddate3, npaiddate4, int_release_mode) " _
    '            '            & " SELECT distinct m.iscode, m.sicode, m.stock_name, m.part_value, m.TYPE," _
    '            '            & "m.stock_type, m.interest_rate, m.interest_period," _
    '            '            & "m.bond_period, m.deleted, m.exchange_rate, m.note, " _
    '            '            & "m.bond_term, m.release_series, m.release_mode, m.isin, " _
    '            '            & "m.rarate, m.tmpid, m.paidmethod, m.npaiddate1, m.npaiddate2," _
    '            '            & "m.npaiddate3, m.npaiddate4, m.int_release_mode FROM rgsi m, tlstockauth a" _
    '            '            & " WHERE ((a.AUTHID = '" & v_strTellerID & "' AND a.authtype = 'U')" _
    '            '            & " OR (a.AUTHID IN (SELECT grpid FROM tlgrpusers WHERE tlid = '" & v_strTellerID & "')" _
    '            '            & " AND a.authtype = 'G')) AND m.deleted = 0 " _
    '            '            & " AND m.sicode = a.sicode AND m.deleted=0 AND m.BRID IN " & v_lstBrID
    '            v_strSQL = "INSERT INTO TMP_rgsi(iscode, sicode, stock_name, part_value, TYPE," _
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
    '            v_obj.ExecuteNonQueryTran(CommandType.Text, v_strSQL)
    '            ' tuanta
    '            v_strSQL = "INSERT INTO TMP_RGSI(AUTOID, SICODE) SELECT 0 AUTOID, '000' SICODE FROM DUAL"
    '            v_obj.ExecuteNonQueryTran(CommandType.Text, v_strSQL)


    '            'Lay cau xu ly chung
    '            Dim v_hSQL As New Hashtable
    '            Dim v_hORD As New Hashtable
    '            If v_strTran <> "" Then
    '                v_strSQL = "SELECT * FROM SISTORES WHERE SEARCHCODE IS NULL AND OVERWRITE IN " & v_strTran & " ORDER BY ODRNUM"

    '                v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
    '                Dim v_strOverWrite As String
    '                If v_ds.Tables(0).Rows.Count > 0 Then
    '                    For i As Integer = 0 To v_ds.Tables(0).Rows.Count - 1
    '                        v_strOverWrite = v_ds.Tables(0).Rows(i)("OVERWRITE")
    '                        v_hORD(i) = v_strOverWrite
    '                        v_hSQL(v_strOverWrite) = v_ds.Tables(0).Rows(i)("TRACE") & v_ds.Tables(0).Rows(i)("CMDSQL") & " " & v_ds.Tables(0).Rows(i)("CMDSQL1")
    '                    Next
    '                End If

    '                'Lay cau xu ly chung da dc viet lai
    '                v_strSQL = "SELECT * FROM SISTORES WHERE SEARCHCODE='" & v_strObjName & "' AND OVERWRITE IN " & v_strTran & " ORDER BY ODRNUM"
    '                v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

    '                If v_ds.Tables(0).Rows.Count > 0 Then
    '                    For i As Integer = 0 To v_ds.Tables(0).Rows.Count - 1
    '                        v_strOverWrite = v_ds.Tables(0).Rows(i)("OVERWRITE")
    '                        If Not v_hSQL(v_strOverWrite) Is Nothing Then
    '                            v_hSQL(v_strOverWrite) = v_ds.Tables(0).Rows(i)("TRACE") & v_ds.Tables(0).Rows(i)("CMDSQL") & " " & v_ds.Tables(0).Rows(i)("CMDSQL1")
    '                        End If
    '                    Next
    '                End If


    '                'Thuc hien cau xu ly chung
    '                For i As Integer = 0 To v_hORD.Count - 1
    '                    v_strOverWrite = v_hORD(i)

    '                    v_strSQL = Mid(v_hSQL(v_strOverWrite).ToString, 2)

    '                    v_strSQL = Replace(v_strSQL, "[!BRID]", v_strBRID)
    '                    v_strSQL = Replace(v_strSQL, "[!TLID]", v_strTellerID)
    '                    v_strSQL = Replace(v_strSQL, "[!LBRID]", v_lstBrID)
    '                    'v_strSQL = Replace(v_strSQL, "[!LSICODE]", v_lstSICODE)
    '                    'v_strSQL = Replace(v_strSQL, "[!LMICODE]", v_lstMICODE)

    '                    v_strSQL = Replace(v_strSQL, "[!TLLOGALL]", "TLLOGALL")
    '                    v_strSQL = Replace(v_strSQL, "[!CATRANA]", "CATRANA")
    '                    v_strSQL = Replace(v_strSQL, "[!IATRANA]", "IATRANA")
    '                    v_strSQL = Replace(v_strSQL, "[!MATRANA]", "MATRANA")
    '                    v_strSQL = Replace(v_strSQL, "[!SFTRANA]", "SFTRANA")
    '                    v_strSQL = Replace(v_strSQL, "[!MFTRANA]", "MFTRANA")
    '                    v_strSQL = Replace(v_strSQL, "[!RATRANA]", "RATRANA")
    '                    v_strSQL = Replace(v_strSQL, "[!RGTRANA]", "RGTRANA")

    '                    If Mid(v_strOverWrite, 1, 5) = "TLLOG" Then
    '                        v_strSQL = Mid(v_strSQL, 1, InStr(v_strSQL.ToUpper, "SELECT") - 1) _
    '                                   & " SELECT * FROM (" & v_strSQL.Substring(InStr(v_strSQL.ToUpper, "SELECT") - 1) & ")" _
    '                                   & " WHERE 1=1 " & v_strTllogWhere
    '                    ElseIf Mid(v_strOverWrite, 3, 4) = "TRAN" Then
    '                        Select Case Mid(v_strOverWrite, 1, 6)
    '                            Case "CATRAN", "CAMAST"
    '                                v_strSQL = v_strSQL & v_strCAWhere
    '                            Case Else
    '                                v_strSQL = v_strSQL & v_strTranWhere
    '                        End Select
    '                    End If

    '                    If v_intCount >= 0 Then
    '                        For j = 0 To v_intCount
    '                            Select Case Mid(v_strOverWrite, 1, 2)
    '                                Case "IA", "MA", "RA", "SF"
    '                                    If v_arrField(j).Split("|")(0) = "TXDATE" Then
    '                                        v_strSQL = Replace(v_strSQL, "[!O_TXDATE]", ">")
    '                                    Else
    '                                        v_strSQL = Replace(v_strSQL, "[!O_" & v_arrField(j).Split("|")(0) & "]", v_arrField(j).Split("|")(1))
    '                                    End If
    '                                Case Else
    '                                    v_strSQL = Replace(v_strSQL, "[!O_" & v_arrField(j).Split("|")(0) & "]", v_arrField(j).Split("|")(1))
    '                            End Select
    '                            v_strSQL = Replace(v_strSQL, "[!V_" & v_arrField(j).Split("|")(0) & "]", v_arrField(j).Split("|")(2))
    '                        Next
    '                    End If
    '                    If Mid(v_strOverWrite, 1, 5) <> "TLLOG" Then
    '                        v_strSQL = Replace(v_strSQL, "[!O_TXDATE]", ">")
    '                        v_strSQL = Replace(v_strSQL, "[!V_TXDATE]", Replace(v_strCurrDate, "_", "/"))
    '                    Else
    '                        v_strSQL = Replace(v_strSQL, "[!O_TXDATE]", "=")
    '                        v_strSQL = Replace(v_strSQL, "[!V_TXDATE]", Replace(v_strCurrDate, "_", "/"))
    '                    End If

    '                    v_strSQL = DecodeSQL(v_strSQL)
    '                    If v_trace_status = "1" And Mid(v_hSQL(v_strOverWrite).ToString, 1, 1) = "1" Then
    '                        Trace.WriteLine("[SI - " & v_strObjName & "] " & DateTime.Now & " :" & vbCrLf & " -o- Câu lệnh thứ xử lý chung thứ #" & i + 1 & "-o-" & vbCrLf & v_strSQL & vbCrLf)
    '                        If InStr(v_strSQL.ToUpper, "SELECT") > 0 Then
    '                            Trace.WriteLine("-o- Dữ liệu câu lệnh xử lý chung thứ #" & i + 1 & "-o-")
    '                            v_strSQLTmp = v_strSQL.Substring(InStr(v_strSQL.ToUpper, "SELECT") - 1)
    '                            v_trace = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
    '                            Trace.WriteLine(v_trace.GetXml & vbCrLf)
    '                        End If
    '                    End If
    '                    v_obj.ExecuteNonQueryTran(CommandType.Text, v_strSQL)
    '                Next
    '            End If

    '            v_strSQL = "SELECT * FROM SISTORES WHERE SEARCHCODE='" & v_strObjName & "'  AND OVERWRITE IS NULL AND DELETED=0 AND STATUS=0 ORDER BY ODRNUM"
    '            v_ds1 = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

    '            'Su ly cau lay SQL trong SISTORES
    '            For i = 0 To v_ds1.Tables(0).Rows.Count - 1
    '                v_strSQL = v_ds1.Tables(0).Rows(i)("CMDSQL").ToString.ToUpper & " " & v_ds1.Tables(0).Rows(i)("CMDSQL1").ToString.ToUpper
    '                v_blnReplace = False

    '                If v_intCount >= 0 Then
    '                    For j = 0 To v_intCount
    '                        v_strhKey = v_arrField(j).Split("|")(0)
    '                        v_strhValue = "[!V_" & v_arrField(j).Split("|")(0) & "]"
    '                        If InStr(v_strSQL, v_strhValue, CompareMethod.Text) > 0 Then
    '                            v_strSQL = Replace(v_strSQL, "[!O_" & v_arrField(j).Split("|")(0) & "]", v_arrField(j).Split("|")(1))
    '                            v_strSQL = Replace(v_strSQL, "[!V_" & v_arrField(j).Split("|")(0) & "]", v_arrField(j).Split("|")(2))

    '                            If v_hCondition(v_strhKey) Is Nothing Then
    '                                v_hCondition.Add(v_strhKey, v_strhValue)
    '                            End If
    '                            v_blnReplace = True
    '                        End If
    '                    Next
    '                End If

    '                If (Not v_blnReplace And v_ds1.Tables(0).Rows(i)("SITYPE") = 0) Or v_blnReplace Then
    '                    v_strSQL = DecodeSQL(v_strSQL)
    '                    'Neu khong su dung bang TMP thi thong bao loi
    '                    If Not CheckTranTable(v_strSQL) Then
    '                        v_lngError = 1
    '                        Exit For
    '                    End If

    '                    v_strSQL = Replace(v_strSQL, "[!BRID]", v_strBRID)
    '                    v_strSQL = Replace(v_strSQL, "[!TLID]", v_strTellerID)
    '                    v_strSQL = Replace(v_strSQL, "[!LBRID]", v_lstBrID)
    '                    'v_strSQL = Replace(v_strSQL, "[!LSICODE]", v_lstSICODE)
    '                    'v_strSQL = Replace(v_strSQL, "[!LMICODE]", v_lstMICODE)

    '                    If v_trace_status = "1" And v_ds1.Tables(0).Rows(i)("TRACE") = 1 Then
    '                        Trace.WriteLine("[SI - " & v_strObjName & "] " & DateTime.Now & " :" & vbCrLf & " -o- Câu lệnh thứ #" & v_ds1.Tables(0).Rows(i)("ODRNUM") & "-o-" & vbCrLf & v_strSQL & vbCrLf)
    '                        If InStr(v_strSQL.ToUpper, "SELECT") > 0 Then
    '                            Trace.WriteLine("-o- Dữ liệu câu lệnh thứ #" & v_ds1.Tables(0).Rows(i)("ODRNUM") & "-o-")
    '                            v_strSQLTmp = v_strSQL.Substring(InStr(v_strSQL.ToUpper, "SELECT") - 1)
    '                            v_trace = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
    '                            Trace.WriteLine(v_trace.GetXml & vbCrLf)
    '                        End If
    '                    End If

    '                    v_obj.ExecuteNonQueryTran(CommandType.Text, v_strSQL)
    '                End If
    '            Next

    '            If v_lngError = ERR_SYSTEM_OK Then
    '                v_strSQL = v_strCmdInquiry

    '                If Not CheckTranTable(v_strSQL) Then
    '                    v_lngError = 1
    '                Else
    '                    If v_intCount >= 0 Then
    '                        For i = 0 To v_intCount
    '                            v_strhKey = v_arrField(i).Split("|")(0)
    '                            v_strhValue = "[!V_" & v_arrField(i).Split("|")(0) & "]"
    '                            If InStr(v_strSQL, v_strhValue, CompareMethod.Text) > 0 Then
    '                                v_strSQL = Replace(v_strSQL, "[!O_" & v_arrField(i).Split("|")(0) & "]", v_arrField(i).Split("|")(1))
    '                                v_strSQL = Replace(v_strSQL, "[!V_" & v_arrField(i).Split("|")(0) & "]", v_arrField(i).Split("|")(2))

    '                                If v_hCondition(v_strhKey) Is Nothing Then
    '                                    v_hCondition.Add(v_strhKey, v_strhValue)
    '                                End If
    '                            End If
    '                        Next
    '                    End If

    '                    v_strSQL = Replace(v_strSQL, "[!V_TXDATE]", Replace(v_strCurrDate, "_", "/"))

    '                    v_strSQL = DecodeSQL(v_strSQL)
    '                    Dim v_lngRowCount As Long

    '                    Dim v_strWhere As String = ""
    '                    Dim v_strTmp, v_strTmpField, v_strValue As String
    '                    If v_strCondition <> "" Then
    '                        v_arrField = v_strCondition.Split("#")
    '                        v_intCount = v_arrField.Length - 2

    '                        For i = 0 To v_arrField.Length - 2
    '                            If v_hCondition(v_arrField(i).Split("|")(0)) Is Nothing Then
    '                                'v_strWhere &= " AND " & v_arrField(i).Split("|")(0) & " " & v_arrField(i).Split("|")(1) & " "
    '                                v_strValue = v_arrField(i).Split("|")(2)
    '                                Select Case v_arrField(i).Split("|")(3)
    '                                    Case "D", "P"
    '                                        v_strTmpField = "TO_DATE(" & v_arrField(i).Split("|")(0) & ",'" & gc_FORMAT_DATE & "') " & v_arrField(i).Split("|")(1) & " "
    '                                        v_strTmp = "TO_DATE('" & v_strValue & "', '" & gc_FORMAT_DATE & "')"
    '                                    Case "N"
    '                                        v_strTmpField = v_arrField(i).Split("|")(0) & " " & v_arrField(i).Split("|")(1) & " "
    '                                        If IsNumeric(v_strValue) Then
    '                                            v_strTmp = CDbl(v_strValue)
    '                                        Else
    '                                            v_strTmp = 0
    '                                        End If
    '                                    Case Else
    '                                        v_strTmpField = v_arrField(i).Split("|")(0) & " " & v_arrField(i).Split("|")(1) & " "

    '                                        v_strValue = Trim(Replace(v_strValue, ".", String.Empty))
    '                                        If v_arrField(i).Split("|")(1) = "LIKE" Then
    '                                            v_strTmp = "N'%" & ToLiteral(v_strValue) & "%'"
    '                                        Else
    '                                            v_strTmp = "N'" & ToLiteral(v_strValue) & "'"
    '                                        End If
    '                                End Select

    '                                v_strWhere &= " AND " & v_strTmpField & v_strTmp
    '                            End If
    '                        Next
    '                    End If

    '                    If Trim(v_strWhere) <> "" Then
    '                        v_strSQL = "SELECT * FROM (" & v_strSQL & ") WHERE 1=1 " & v_strWhere
    '                    End If

    '                    If v_strSQL <> "" Then
    '                        'Tinh so ban ghi
    '                        v_strSQLTmp = "SELECT COUNT(1) FROM (" & v_strSQL & ")"
    '                        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
    '                        v_lngRowCount = v_ds.Tables(0).Rows(0)(0)

    '                        If Not (v_attrColl.GetNamedItem(gc_AtributeFUNCNAME) Is Nothing) Then
    '                            pv_xmlDocument.DocumentElement.Attributes(gc_AtributeFUNCNAME).InnerXml = v_lngRowCount
    '                        Else
    '                            Dim v_attr As Xml.XmlAttribute
    '                            v_attr = pv_xmlDocument.CreateAttribute(gc_AtributeFUNCNAME)
    '                            v_attr.Value = v_lngRowCount
    '                            Dim dataElement As Xml.XmlElement
    '                            dataElement = pv_xmlDocument.DocumentElement
    '                            dataElement.Attributes.Append(v_attr)
    '                        End If

    '                        'Lay DL loc theo dong
    '                        Dim v_intFrom, v_intTo As Integer


    '                        If v_strClause <> "" Then
    '                            v_intFrom = CInt(v_strClause.Split("|")(0))
    '                            v_intTo = CInt(v_strClause.Split("|")(1))

    '                            v_strSQLTmp = "SELECT * FROM (SELECT *,rownum rn FROM (" & v_strSQL & ")) WHERE RN BETWEEN " & v_intFrom & " AND " & v_intTo
    '                        Else
    '                            v_strSQLTmp = v_strSQL
    '                        End If

    '                        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)

    '                        If v_trace_status = "1" Then
    '                            Trace.WriteLine("[SI - " & v_strObjName & "] " & DateTime.Now & " :" & vbCrLf & " -o- Ket qua:-o-" & vbCrLf & v_strSQL & vbCrLf)
    '                            'Trace.WriteLine(v_ds.GetXml & vbCrLf)
    '                        End If
    '                    End If
    '                End If
    '            End If
    '            'Lay dl khong qua form frmsearch
    '        Else
    '            If Trim(v_strCmdInquiry) <> "" Then
    '                v_strSQL = v_strCmdInquiry
    '            Else
    '                v_strSQL = "SELECT * FROM " & v_strObjName & " WHERE 0=0"
    '            End If

    '            If Trim(v_strCondition) <> "" Then
    '                v_strSQL = v_strSQL & " AND " & v_strCondition
    '            End If

    '            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
    '        End If

    '        'Tao DL de tra ve Client
    '        Dim v_dataElement As Xml.XmlElement, v_entryNode As Xml.XmlNode

    '        Dim v_attrFLDNAME As Xml.XmlAttribute, v_attrFLDTYPE As Xml.XmlAttribute, v_attrOLDVAL As Xml.XmlAttribute

    '        For i = 0 To v_ds.Tables(0).Rows.Count - 1
    '            v_dataElement = pv_xmlDocument.CreateElement(Xml.XmlNodeType.Element, "ObjData", "")
    '            For j = 0 To v_ds.Tables(0).Columns.Count - 1
    '                'Append entry to data node
    '                v_entryNode = pv_xmlDocument.CreateNode(Xml.XmlNodeType.Element, "entry", "")

    '                'Add field name
    '                v_attrFLDNAME = pv_xmlDocument.CreateAttribute("fldname")
    '                v_attrFLDNAME.Value = v_ds.Tables(0).Columns(j).ColumnName
    '                v_entryNode.Attributes.Append(v_attrFLDNAME)

    '                'Add field type
    '                v_attrFLDTYPE = pv_xmlDocument.CreateAttribute("fldtype")
    '                v_attrFLDTYPE.Value = v_ds.Tables(0).Columns(j).DataType.ToString
    '                v_entryNode.Attributes.Append(v_attrFLDTYPE)

    '                'Add current value
    '                v_attrOLDVAL = pv_xmlDocument.CreateAttribute("oldval")
    '                If IsDBNull((v_ds.Tables(0).Rows(i)(j))) Then
    '                    If v_ds.Tables(0).Rows(i)(j).GetType.Name = GetType(System.DateTime).Name _
    '                        Or v_ds.Tables(0).Rows(i)(j).GetType.Name = GetType(System.String).Name Then
    '                        v_attrOLDVAL.Value = ""
    '                    Else
    '                        v_attrOLDVAL.Value = "0"
    '                    End If

    '                Else
    '                    If v_ds.Tables(0).Rows(i)(j).GetType.Name = GetType(System.DateTime).Name Then
    '                        v_attrOLDVAL.Value = Format(v_ds.Tables(0).Rows(i)(j), gc_FORMAT_DATE)
    '                    Else
    '                        v_attrOLDVAL.Value = CStr(v_ds.Tables(0).Rows(i)(j))
    '                    End If
    '                End If
    '                v_entryNode.Attributes.Append(v_attrOLDVAL)

    '                'Set value
    '                If IsDBNull((v_ds.Tables(0).Rows(i)(j))) Then
    '                    v_entryNode.InnerText = ""
    '                Else
    '                    If v_ds.Tables(0).Rows(i)(j).GetType.Name = GetType(System.DateTime).Name Then
    '                        v_entryNode.InnerText = Format(v_ds.Tables(0).Rows(i)(j), gc_FORMAT_DATE)
    '                    Else
    '                        v_entryNode.InnerText = CStr(v_ds.Tables(0).Rows(i)(j))
    '                    End If
    '                End If

    '                v_dataElement.AppendChild(v_entryNode)
    '            Next
    '            pv_xmlDocument.DocumentElement.AppendChild(v_dataElement)
    '        Next
    '        'If v_lngError <> 0 Then
    '        '    Dim v_strErrorSource, v_strErrorMessage As String

    '        '    v_strErrorSource = "SISTORES"
    '        '    v_strErrorMessage = String.Empty

    '        '    LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
    '        '                 & "Error code: " & v_lngError.ToString() & vbNewLine _
    '        '                 & "Error message: " & v_strErrorMessage, EventLogEntryType.Information)
    '        '    BuildXMLErrorException(pv_xmlDocument, v_strErrorSource, v_lngError, v_strErrorMessage)
    '        'End If
    '        Return v_lngError
    '    Catch ex As Exception
    '        v_obj.Rollback()
    '        'LogError.Write("Error source: " & ex.Source & vbNewLine _
    '        '             & "Error code: System error!" & vbNewLine _
    '        '             & "Error message: " & ex.Message & vbCrLf & v_strSQL, EventLogEntryType.Error)
    '        Throw ex
    '    Finally
    '        If v_trace_status = "1" Then
    '            Trace.WriteLine("[Kết thúc: SI - " & v_strObjName & "] " & DateTime.Now & vbCrLf)
    '            tr2.Close()
    '            tr2.Dispose()
    '        End If
    '        v_obj.Rollback()
    '        v_obj.Dispose()
    '        GC.Collect()
    '    End Try
    'End Function

    'Public Function HOSTCoreInquiryOver(ByRef pv_xmlDocument As XmlDocumentEx) As Long
    '    Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes

    '    Dim v_strClause As String
    '    Dim v_strLocal As String
    '    Dim v_strObjName As String
    '    Dim v_strCmdInquiry As String
    '    Dim v_strCondition As String
    '    Dim v_strBRID As String
    '    Dim v_obj As New DataAccess
    '    'Dim v_obj As Object = Nothing
    '    Dim v_arrField() As String

    '    Dim v_ds As DataSet
    '    Dim v_strSQL, v_strSQLTmp As String
    '    Dim v_bln As Boolean
    '    Dim tr2 As TextWriterTraceListener
    '    Dim v_trace_status, v_trace_path As String
    '    Dim v_lstBrID As String
    '    Dim v_ds1 As DataSet
    '    Dim v_trace As DataSet
    '    Dim v_lngError As Long = ERR_SYSTEM_OK
    '    Try
    '        If Not (v_attrColl.GetNamedItem(gc_AtributeCMDINQUIRY) Is Nothing) Then
    '            v_strCmdInquiry = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCMDINQUIRY), Xml.XmlAttribute).Value)
    '        Else
    '            v_strCmdInquiry = String.Empty
    '        End If

    '        If Not (v_attrColl.GetNamedItem(gc_AtributeFUNCNAME) Is Nothing) Then
    '            v_strClause = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeFUNCNAME), Xml.XmlAttribute).Value)
    '        Else
    '            v_strClause = String.Empty
    '        End If

    '        If Not (v_attrColl.GetNamedItem(gc_AtributeLOCAL) Is Nothing) Then
    '            v_strLocal = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeLOCAL), Xml.XmlAttribute).Value)
    '        Else
    '            v_strLocal = String.Empty
    '        End If
    '        If Not (v_attrColl.GetNamedItem(gc_AtributeCLAUSE) Is Nothing) Then
    '            v_strCondition = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCLAUSE), Xml.XmlAttribute).Value)
    '        Else
    '            v_strCondition = String.Empty
    '        End If

    '        If Not (v_attrColl.GetNamedItem(gc_AtributeOBJNAME) Is Nothing) Then
    '            v_strObjName = Mid(CStr(CType(v_attrColl.GetNamedItem(gc_AtributeOBJNAME), Xml.XmlAttribute).Value), 4)
    '        Else
    '            v_strObjName = ATTR_TABLE
    '        End If
    '        Dim v_strPartition As String
    '        If Not (v_attrColl.GetNamedItem(gc_AtributeREFERENCE) Is Nothing) Then
    '            v_strPartition = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeREFERENCE), Xml.XmlAttribute).Value)
    '        Else
    '            v_strPartition = ""
    '        End If

    '        v_strBRID = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeBRID), Xml.XmlAttribute).Value)
    '        Dim v_strTellerID As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTLID), Xml.XmlAttribute).Value)
    '        Dim v_arrFilter As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTXNUM), Xml.XmlAttribute).Value)
    '        Dim v_blnSearch As Boolean = True
    '        Dim v_strCurrDate As String
    '        Dim v_strMaxDate As String
    '        v_trace_status = "0"
    '        'v_trace_path = "C:\log_sisearch_sql_data.txt"

    '        v_obj.NewDBInstance(gc_MODULE_HOST)
    '        v_obj.BeginTran()

    '        v_obj.GetSysVar("SYSTEM", "TRACE_STATUS", v_trace_status)

    '        If v_trace_status = "1" Then
    '            v_obj.GetSysVar("SYSTEM", "CURRDATE", v_strBRID, v_strCurrDate)
    '            v_obj.GetSysVar("SYSTEM", "TRACE_PATH", v_trace_path)
    '            Dim v_strTLName As String = v_obj.GetValue("TLPROFILES", "TLNAME", "TLID='" & v_strTellerID & "'")
    '            v_strCurrDate = Replace(v_strCurrDate, "/", "_")
    '            If v_trace_path = "" Then
    '                Dim v_app As New ApplicationServices.ApplicationBase
    '                v_trace_path = v_app.Info.DirectoryPath & "\Log\" & v_strCurrDate
    '            Else
    '                v_trace_path = v_trace_path & v_strCurrDate
    '            End If

    '            If Not System.IO.Directory.Exists(v_trace_path) Then
    '                System.IO.Directory.CreateDirectory(v_trace_path)
    '            End If

    '            v_trace_path &= "\log_search_br" & v_strBRID & "_" & v_strTLName & ".txt"

    '            tr2 = New TextWriterTraceListener(System.IO.File.AppendText(v_trace_path))
    '            Trace.Listeners.Add(tr2)
    '            Trace.WriteLine("[Bắt đầu: SI - " & v_strObjName & "] " & DateTime.Now & vbCrLf)
    '        End If

    '        'Xu ly DL bang TRAN, TLLOG
    '        v_strSQL = "SELECT TLLOGTRAN, LOADALL FROM SISEARCH WHERE SEARCHCODE = '" & v_strObjName & "'"
    '        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

    '        Dim v_strTran As String = "" 'IA|MA|CA|MF|RA|RG|SF|TLLOG
    '        Dim v_lstTran() As String
    '        Dim v_blnLoadAll As Boolean = False

    '        If v_ds.Tables(0).Rows.Count > 0 Then
    '            v_strTran = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("TLLOGTRAN")).ToUpper
    '        End If

    '        v_strMaxDate = v_strPartition.Split("|")(1)
    '        v_blnSearch = CheckPassDate(v_strMaxDate, Replace(v_strCurrDate, "_", "/"))

    '        Dim v_strTranWhere As String = ""
    '        Dim v_strTllogWhere As String = ""
    '        Dim v_strCAWhere As String = ""
    '        v_strCAWhere = " AND [#TXDATE [!O_MSGDATE] TO_DATE('[!V_MSGDATE]','" & gc_FORMAT_DATE & "')#]"
    '        v_strTranWhere = " AND [#TXDATE [!O_TXDATE] TO_DATE('[!V_TXDATE]','" & gc_FORMAT_DATE & "')#] AND [#TXNUM [!O_TXNUM] '[!V_TXNUM]'#]"

    '        'If v_blnSearch Then
    '        v_strTllogWhere = " AND [#TXDATE [!O_TXDATE] TO_DATE('[!V_TXDATE]','" & gc_FORMAT_DATE & "')#] AND [#TXNUM [!O_TXNUM] '[!V_TXNUM]'#]"
    '        v_strTllogWhere &= " AND [#BUSDATE [!O_TXDATE] TO_DATE('[!V_BUSDATE]','" & gc_FORMAT_DATE & "')#] AND [#TLTXCD [!O_TLTXCD] '[!V_TLTXCD]'#]"
    '        'End If

    '        If v_strTran <> "" Then
    '            v_lstTran = v_strTran.Split("|")
    '            v_strTran = ""
    '            For i As Integer = 0 To v_lstTran.Length - 1
    '                Select Case v_lstTran(i)
    '                    Case "TLLOG"
    '                        If Not v_blnSearch Then
    '                            v_strTran &= ",'TLLOG'"
    '                        End If
    '                        v_strTran &= ",'TLLOGALL'"
    '                    Case "CA", "IA", "MA", "RA", "SF"
    '                        v_strTran &= ",'" & v_lstTran(i) & "TRAN'"
    '                        If v_lstTran(i) <> "CA" Then
    '                            v_strTran &= ",'" & v_lstTran(i) & "MAST'"
    '                        End If
    '                        v_strTran &= ",'" & v_lstTran(i) & "TRANA'"
    '                    Case "RG", "MF"
    '                        v_strTran &= ",'" & v_lstTran(i) & "TRAN'"
    '                        v_strTran &= ",'" & v_lstTran(i) & "TRANA'"
    '                End Select
    '            Next
    '            v_strTran = "(" & Mid(v_strTran, 2) & ")"
    '        End If

    '        v_strPartition = GetListPartition(v_strPartition)

    '        Dim v_lstSICODE, v_lstMICODE As String

    '        v_lstMICODE = v_arrFilter.Split("|")(1)
    '        v_lstSICODE = v_arrFilter.Split("|")(0)
    '        Dim v_intCount As Integer = -1

    '        If v_strCondition <> "" Then
    '            v_arrField = v_strCondition.Split("#")
    '            v_intCount = v_arrField.Length - 2
    '        End If
    '        Dim v_hCondition As New Hashtable
    '        Dim v_strhKey, v_strhValue As String
    '        Dim v_blnReplace As Boolean

    '        If v_intCount >= 0 Then
    '            For j = 0 To v_intCount
    '                v_strhKey = v_arrField(j).Split("|")(0).ToUpper
    '                Select Case v_strhKey
    '                    Case "SICODE"
    '                        v_lstSICODE = "('" & v_arrField(j).Split("|")(2) & "')"
    '                    Case "MICODE"
    '                        v_lstMICODE = "('" & v_arrField(j).Split("|")(2) & "')"
    '                End Select
    '            Next
    '        End If
    '        'Lay phan quyen chi nhanh
    '        v_strSQL = "SELECT DISTINCT brid brid  FROM " _
    '                   & " (SELECT b.brid FROM tlbridauth a, brgrp b" _
    '                   & " WHERE ((AUTHID = '" & v_strTellerID & "' AND authtype = 'U') OR (AUTHID IN (SELECT a.grpid" _
    '                   & " FROM tlgrpusers a WHERE a.tlid = '" & v_strTellerID & "') AND authtype = 'G'))" _
    '                   & " And a.brid = b.brid And a.deleted = 0 And a.status = 0" _
    '                   & " And b.deleted = 0 And b.status = 0" _
    '                   & " UNION " _
    '                   & " SELECT b.brid FROM tlprofiles a, brgrp b" _
    '                   & " WHERE a.brid = b.brid AND a.tlid = '" & v_strTellerID & "'" _
    '                   & " AND a.deleted = 0 AND a.status = 0 AND b.deleted = 0 AND b.status = 0)"

    '        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

    '        v_lstBrID = ""
    '        For i = 0 To v_ds.Tables(0).Rows.Count - 1
    '            v_lstBrID &= ",'" & v_ds.Tables(0).Rows(i)(0) & "'"
    '        Next
    '        v_lstBrID = "(" & Mid(v_lstBrID, 2) & ")"

    '        'Lay TVLK dc phan quyen cho user vao bang TMP_RGMI
    '        'v_strSQL = "INSERT INTO tmp_rgmi " _
    '        '            & " SELECT DISTINCT m.* FROM rgmi m, tlmemauth a" _
    '        '            & " WHERE ((a.AUTHID = '" & v_strTellerID & "' AND a.authtype = 'U')" _
    '        '            & " OR (a.AUTHID IN (SELECT grpid FROM tlgrpusers WHERE tlid = '" & v_strTellerID & "')" _
    '        '            & " AND a.authtype = 'G')) AND m.deleted = 0 AND m.status = 0" _
    '        '            & " AND m.micode = a.micode AND m.deleted=0 AND m.status=0"
    '        v_strSQL = "INSERT INTO tmp_rgmi " _
    '                   & " SELECT DISTINCT m.* FROM rgmi m WHERE m.deleted=0 AND m.status=0" _
    '                   & " AND m.MICODE IN " & v_lstMICODE

    '        v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

    '        v_strSQL = "INSERT INTO TMP_RGMI(AUTOID, MICODE) SELECT '0' AUTOID, '000' MICODE FROM DUAL"
    '        v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

    '        'Lay chung khoan dc phan quyen cho user vao bang TMP_RGSI
    '        'v_strSQL = "INSERT INTO tmp_rgsi(iscode, sicode, stock_name, part_value, TYPE," _
    '        '            & "stock_type, interest_rate, interest_period, " _
    '        '            & "bond_period, deleted, exchange_rate, note, " _
    '        '            & "bond_term, release_series, release_mode, isin, " _
    '        '            & "rarate, tmpid, paidmethod, npaiddate1, npaiddate2, " _
    '        '            & "npaiddate3, npaiddate4, int_release_mode) " _
    '        '            & " SELECT distinct m.iscode, m.sicode, m.stock_name, m.part_value, m.TYPE," _
    '        '            & "m.stock_type, m.interest_rate, m.interest_period," _
    '        '            & "m.bond_period, m.deleted, m.exchange_rate, m.note, " _
    '        '            & "m.bond_term, m.release_series, m.release_mode, m.isin, " _
    '        '            & "m.rarate, m.tmpid, m.paidmethod, m.npaiddate1, m.npaiddate2," _
    '        '            & "m.npaiddate3, m.npaiddate4, m.int_release_mode FROM rgsi m, tlstockauth a" _
    '        '            & " WHERE ((a.AUTHID = '" & v_strTellerID & "' AND a.authtype = 'U')" _
    '        '            & " OR (a.AUTHID IN (SELECT grpid FROM tlgrpusers WHERE tlid = '" & v_strTellerID & "')" _
    '        '            & " AND a.authtype = 'G')) AND m.deleted = 0 " _
    '        '            & " AND m.sicode = a.sicode AND m.deleted=0 AND m.BRID IN " & v_lstBrID
    '        v_strSQL = "INSERT INTO tmp_rgsi(iscode, sicode, stock_name, part_value, TYPE," _
    '                    & "stock_type, interest_rate, interest_period, " _
    '                    & "bond_period, deleted, exchange_rate, note, " _
    '                    & "bond_term, release_series, release_mode, isin, " _
    '                    & "rarate, tmpid, paidmethod, npaiddate1, npaiddate2, " _
    '                    & "npaiddate3, npaiddate4, int_release_mode, brid, status, issuer_date, due_date) " _
    '                    & " SELECT distinct m.iscode, m.sicode, m.stock_name, m.part_value, m.TYPE," _
    '                    & "m.stock_type, m.interest_rate, m.interest_period," _
    '                    & "m.bond_period, m.deleted, m.exchange_rate, m.note, " _
    '                    & "m.bond_term, m.release_series, m.release_mode, m.isin, " _
    '                    & "m.rarate, m.tmpid, m.paidmethod, m.npaiddate1, m.npaiddate2," _
    '                    & "m.npaiddate3, m.npaiddate4, m.int_release_mode, m.brid, m.status, m.issuer_date, m.due_date FROM rgsi m" _
    '                    & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_lstSICODE
    '        v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
    '        ' tuanta
    '        v_strSQL = "INSERT INTO TMP_RGSI(AUTOID, SICODE) SELECT 0 AUTOID, '000' SICODE FROM DUAL"
    '        v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

    '        'Lay cau xu ly chung
    '        Dim v_hSQL As New Hashtable
    '        Dim v_hORD As New Hashtable
    '        If v_strTran <> "" Then
    '            v_strSQL = "SELECT * FROM SISTORES WHERE SEARCHCODE IS NULL AND OVERWRITE IN " & v_strTran & " ORDER BY ODRNUM"

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
    '            v_strSQL = "SELECT * FROM SISTORES WHERE SEARCHCODE='" & v_strObjName & "' AND OVERWRITE IN " & v_strTran & " ORDER BY ODRNUM"
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
    '            Dim v_strPartitionSQL As String
    '            For i As Integer = 0 To v_hORD.Count - 1
    '                v_strOverWrite = v_hORD(i)
    '                v_strSQL = Mid(v_hSQL(v_strOverWrite), 2)
    '                v_strPartitionSQL = ""
    '                v_strSQL = Replace(v_strSQL, "[!BRID]", v_strBRID)
    '                v_strSQL = Replace(v_strSQL, "[!TLID]", v_strTellerID)
    '                v_strSQL = Replace(v_strSQL, "[!LBRID]", v_lstBrID)
    '                'v_strSQL = Replace(v_strSQL, "[!LSICODE]", v_lstSICODE)
    '                'v_strSQL = Replace(v_strSQL, "[!LMICODE]", v_lstMICODE)

    '                v_strSQL = Replace(v_strSQL, "TT_", "")
    '                v_strSQL = Replace(v_strSQL, "tt_", "")
    '                v_strSQL = Replace(v_strSQL, "tT_", "")
    '                v_strSQL = Replace(v_strSQL, "Tt_", "")

    '                If Right(v_strOverWrite, 3) = "ALL" Or Right(v_strOverWrite, 1) = "A" Then
    '                    For j As Integer = 0 To v_strPartition.Split("|").Length - 2
    '                        If v_strPartitionSQL <> "" Then
    '                            v_strPartitionSQL &= " UNION ALL "
    '                        End If

    '                        v_strPartitionSQL &= v_strSQL.Substring(InStr(v_strSQL.ToUpper, "SELECT") - 1)
    '                        v_strPartitionSQL = Replace(v_strPartitionSQL, "[!TLLOGALL]", "TLLOGALL_ALL PARTITION(" & v_strPartition.Split("|")(j) & ")")
    '                        v_strPartitionSQL = Replace(v_strPartitionSQL, "[!CATRANA]", "CATRANA_ALL PARTITION(" & v_strPartition.Split("|")(j) & ")")
    '                        v_strPartitionSQL = Replace(v_strPartitionSQL, "[!IATRANA]", "IATRANA_ALL PARTITION(" & v_strPartition.Split("|")(j) & ")")
    '                        v_strPartitionSQL = Replace(v_strPartitionSQL, "[!MATRANA]", "MATRANA_ALL PARTITION(" & v_strPartition.Split("|")(j) & ")")
    '                        v_strPartitionSQL = Replace(v_strPartitionSQL, "[!SFTRANA]", "SFTRANA_ALL PARTITION(" & v_strPartition.Split("|")(j) & ")")
    '                        v_strPartitionSQL = Replace(v_strPartitionSQL, "[!MFTRANA]", "MFTRANA_ALL PARTITION(" & v_strPartition.Split("|")(j) & ")")
    '                        v_strPartitionSQL = Replace(v_strPartitionSQL, "[!RATRANA]", "RATRANA_ALL PARTITION(" & v_strPartition.Split("|")(j) & ")")
    '                        v_strPartitionSQL = Replace(v_strPartitionSQL, "[!RGTRANA]", "RGTRANA_ALL PARTITION(" & v_strPartition.Split("|")(j) & ")")
    '                        If Mid(v_strOverWrite, 3) = "TRANA" Then
    '                            If Mid(v_strOverWrite, 1, 2) = "CA" Then
    '                                v_strPartitionSQL &= v_strCAWhere
    '                            Else
    '                                v_strPartitionSQL &= v_strTranWhere
    '                            End If
    '                        End If
    '                    Next

    '                    v_strSQL = Mid(v_strSQL, 1, InStr(v_strSQL.ToUpper, "SELECT") - 1)
    '                    If v_strOverWrite = "TLLOGALL" Then
    '                        v_strPartitionSQL = " SELECT * FROM (" & v_strPartitionSQL & ") WHERE 1=1" & v_strTllogWhere
    '                    End If
    '                    v_strSQL &= v_strPartitionSQL

    '                    If v_intCount >= 0 Then
    '                        For j = 0 To v_intCount
    '                            Select Case Mid(v_strOverWrite, 1, 2)
    '                                Case "IA", "MA", "RA", "SF"
    '                                    If v_arrField(j).Split("|")(0) = "TXDATE" Then
    '                                        v_strSQL = Replace(v_strSQL, "[!O_TXDATE]", ">")
    '                                    Else
    '                                        v_strSQL = Replace(v_strSQL, "[!O_" & v_arrField(j).Split("|")(0) & "]", v_arrField(j).Split("|")(1))
    '                                    End If
    '                                Case Else
    '                                    v_strSQL = Replace(v_strSQL, "[!O_" & v_arrField(j).Split("|")(0) & "]", v_arrField(j).Split("|")(1))
    '                            End Select
    '                            v_strSQL = Replace(v_strSQL, "[!V_" & v_arrField(j).Split("|")(0) & "]", v_arrField(j).Split("|")(2))

    '                        Next
    '                    End If
    '                End If

    '                'If v_strOverWrite = "CAMAST" Then
    '                '    v_strSQL &= v_strCAWhere
    '                'End If
    '                v_strSQL = DecodeSQL(v_strSQL)

    '                If v_trace_status = "1" And Mid(v_hSQL(v_strOverWrite).ToString, 1, 1) = "1" Then
    '                    Trace.WriteLine("[SI - " & v_strObjName & "] " & DateTime.Now & " :" & vbCrLf & " -o- Câu lệnh thứ xử lý chung thứ #" & i + 1 & "-o-" & vbCrLf & v_strSQL & vbCrLf)
    '                    If InStr(v_strSQL.ToUpper, "SELECT") > 0 Then
    '                        Trace.WriteLine("-o- Dữ liệu câu lệnh xử lý chung thứ #" & i + 1 & "-o-")
    '                        v_strSQLTmp = v_strSQL.Substring(InStr(v_strSQL.ToUpper, "SELECT") - 1)
    '                        v_trace = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
    '                        Trace.WriteLine(v_trace.GetXml & vbCrLf)
    '                    End If
    '                End If
    '                v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
    '            Next
    '        End If

    '        v_strSQL = "SELECT * FROM SISTORES WHERE SEARCHCODE='" & v_strObjName & "' AND OVERWRITE IS NULL AND DELETED=0 AND STATUS=0 ORDER BY ODRNUM"
    '        v_ds1 = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

    '        'Su ly cau lay SQL trong SISTORES


    '        For i = 0 To v_ds1.Tables(0).Rows.Count - 1
    '            v_strSQL = v_ds1.Tables(0).Rows(i)("CMDSQL").ToString.ToUpper & " " & v_ds1.Tables(0).Rows(i)("CMDSQL1").ToString.ToUpper
    '            v_blnReplace = False

    '            If v_intCount >= 0 Then
    '                For j = 0 To v_intCount
    '                    v_strhKey = v_arrField(j).Split("|")(0)
    '                    v_strhValue = "[!V_" & v_arrField(j).Split("|")(0) & "]"
    '                    If InStr(v_strSQL, v_strhValue, CompareMethod.Text) > 0 Then
    '                        v_strSQL = Replace(v_strSQL, "[!O_" & v_arrField(j).Split("|")(0) & "]", v_arrField(j).Split("|")(1))
    '                        v_strSQL = Replace(v_strSQL, "[!V_" & v_arrField(j).Split("|")(0) & "]", v_arrField(j).Split("|")(2))

    '                        If v_hCondition(v_strhKey) Is Nothing Then
    '                            v_hCondition.Add(v_strhKey, v_strhValue)
    '                        End If
    '                        v_blnReplace = True
    '                    End If
    '                Next
    '            End If

    '            If (Not v_blnReplace And v_ds1.Tables(0).Rows(i)("SITYPE") = 0) Or v_blnReplace Then
    '                v_strSQL = DecodeSQL(v_strSQL)
    '                'Neu khong su dung bang TMP thi thong bao loi
    '                If Not CheckTranTable(v_strSQL) Then
    '                    v_lngError = 1
    '                    Exit For
    '                End If

    '                v_strSQL = Replace(v_strSQL, "[!BRID]", v_strBRID)
    '                v_strSQL = Replace(v_strSQL, "[!TLID]", v_strTellerID)
    '                v_strSQL = Replace(v_strSQL, "[!LBRID]", v_lstBrID)
    '                'v_strSQL = Replace(v_strSQL, "[!LSICODE]", v_lstSICODE)
    '                'v_strSQL = Replace(v_strSQL, "[!LMICODE]", v_lstMICODE)

    '                v_strSQL = Replace(v_strSQL, "TT_", "")
    '                v_strSQL = Replace(v_strSQL, "tt_", "")
    '                v_strSQL = Replace(v_strSQL, "tT_", "")
    '                v_strSQL = Replace(v_strSQL, "Tt_", "")

    '                If v_trace_status = "1" And v_ds1.Tables(0).Rows(i)("TRACE") = 1 Then
    '                    Trace.WriteLine("[SI - " & v_strObjName & "] " & DateTime.Now & " :" & vbCrLf & " -o- Câu lệnh thứ #" & v_ds1.Tables(0).Rows(i)("ODRNUM") & "-o-" & vbCrLf & v_strSQL & vbCrLf)
    '                    If InStr(v_strSQL.ToUpper, "SELECT") > 0 Then
    '                        Trace.WriteLine("-o- Dữ liệu câu lệnh thứ #" & v_ds1.Tables(0).Rows(i)("ODRNUM") & "-o-")
    '                        v_strSQLTmp = v_strSQL.Substring(InStr(v_strSQL.ToUpper, "SELECT") - 1)
    '                        v_trace = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
    '                        Trace.WriteLine(v_trace.GetXml & vbCrLf)
    '                    End If
    '                End If

    '                v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
    '            End If
    '        Next

    '        If v_lngError = ERR_SYSTEM_OK Then
    '            v_strSQL = v_strCmdInquiry

    '            If Not CheckTranTable(v_strSQL) Then
    '                v_lngError = 1
    '            Else
    '                If v_intCount >= 0 Then
    '                    For i = 0 To v_intCount
    '                        v_strhKey = v_arrField(i).Split("|")(0)
    '                        v_strhValue = "[!V_" & v_arrField(i).Split("|")(0) & "]"
    '                        If InStr(v_strSQL, v_strhValue, CompareMethod.Text) > 0 Then
    '                            v_strSQL = Replace(v_strSQL, "[!O_" & v_arrField(i).Split("|")(0) & "]", v_arrField(i).Split("|")(1))
    '                            v_strSQL = Replace(v_strSQL, "[!V_" & v_arrField(i).Split("|")(0) & "]", v_arrField(i).Split("|")(2))

    '                            If v_hCondition(v_strhKey) Is Nothing Then
    '                                v_hCondition.Add(v_strhKey, v_strhValue)
    '                            End If
    '                        End If
    '                    Next
    '                End If

    '                v_strSQL = DecodeSQL(v_strSQL)
    '                v_strSQL = Replace(v_strSQL, "TT_", "")
    '                v_strSQL = Replace(v_strSQL, "tt_", "")
    '                v_strSQL = Replace(v_strSQL, "tT_", "")
    '                v_strSQL = Replace(v_strSQL, "Tt_", "")
    '                Dim v_lngRowCount As Long

    '                Dim v_strWhere As String = ""
    '                Dim v_strTmp, v_strTmpField, v_strValue As String
    '                If v_strCondition <> "" Then
    '                    v_arrField = v_strCondition.Split("#")
    '                    v_intCount = v_arrField.Length - 2

    '                    For i = 0 To v_arrField.Length - 2
    '                        If v_hCondition(v_arrField(i).Split("|")(0)) Is Nothing Then
    '                            'v_strWhere &= " AND " & v_arrField(i).Split("|")(0) & " " & v_arrField(i).Split("|")(1) & " "
    '                            v_strValue = v_arrField(i).Split("|")(2)
    '                            Select Case v_arrField(i).Split("|")(3)
    '                                Case "D", "P"
    '                                    v_strTmpField = "TO_DATE(" & v_arrField(i).Split("|")(0) & ",'" & gc_FORMAT_DATE & "') " & v_arrField(i).Split("|")(1) & " "
    '                                    v_strTmp = "TO_DATE('" & v_strValue & "', '" & gc_FORMAT_DATE & "')"
    '                                Case "N"
    '                                    v_strTmpField = v_arrField(i).Split("|")(0) & " " & v_arrField(i).Split("|")(1) & " "
    '                                    If IsNumeric(v_strValue) Then
    '                                        v_strTmp = CDbl(v_strValue)
    '                                    Else
    '                                        v_strTmp = 0
    '                                    End If
    '                                Case Else
    '                                    v_strTmpField = v_arrField(i).Split("|")(0) & " " & v_arrField(i).Split("|")(1) & " "

    '                                    v_strValue = Trim(Replace(v_strValue, ".", String.Empty))
    '                                    If v_arrField(i).Split("|")(1) = "LIKE" Then
    '                                        v_strTmp = "N'%" & v_strValue & "%'"
    '                                    Else
    '                                        v_strTmp = "N'" & v_strValue & "'"
    '                                    End If
    '                            End Select

    '                            v_strWhere &= " AND " & v_strTmpField & v_strTmp
    '                        End If
    '                    Next
    '                End If

    '                If Trim(v_strWhere) <> "" Then
    '                    v_strSQL = "SELECT * FROM (" & v_strSQL & ") WHERE 1=1 " & v_strWhere
    '                End If

    '                If v_strSQL <> "" Then
    '                    'Tinh so ban ghi
    '                    v_strSQLTmp = "SELECT COUNT(1) FROM (" & v_strSQL & ")"
    '                    v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
    '                    v_lngRowCount = v_ds.Tables(0).Rows(0)(0)

    '                    If Not (v_attrColl.GetNamedItem(gc_AtributeFUNCNAME) Is Nothing) Then
    '                        pv_xmlDocument.DocumentElement.Attributes(gc_AtributeFUNCNAME).InnerXml = v_lngRowCount
    '                    Else
    '                        Dim v_attr As Xml.XmlAttribute
    '                        v_attr = pv_xmlDocument.CreateAttribute(gc_AtributeFUNCNAME)
    '                        v_attr.Value = v_lngRowCount
    '                        Dim dataElement As Xml.XmlElement
    '                        dataElement = pv_xmlDocument.DocumentElement
    '                        dataElement.Attributes.Append(v_attr)
    '                    End If

    '                    'Lay DL loc theo dong
    '                    Dim v_intFrom, v_intTo As Integer


    '                    If v_strClause <> "" Then
    '                        v_intFrom = CInt(v_strClause.Split("|")(0))
    '                        v_intTo = CInt(v_strClause.Split("|")(1))

    '                        v_strSQLTmp = "SELECT * FROM (SELECT T.*,rownum rn FROM (" & v_strSQL & ") T) WHERE RN BETWEEN " & v_intFrom & " AND " & v_intTo
    '                    Else
    '                        v_strSQLTmp = v_strSQL
    '                    End If

    '                    'v_strSQLTmp = Replace(v_strSQLTmp, "N'", "'")
    '                    v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)

    '                    If v_trace_status = "1" Then
    '                        Trace.WriteLine("[SI - " & v_strObjName & "] " & DateTime.Now & " :" & vbCrLf & " -o- Ket qua:-o-" & vbCrLf & v_strSQL & vbCrLf)
    '                        'Trace.WriteLine(v_ds.GetXml & vbCrLf)
    '                    End If
    '                End If
    '            End If
    '        End If
    '        'Tao DL de tra ve Client
    '        Dim v_dataElement As Xml.XmlElement, v_entryNode As Xml.XmlNode

    '        Dim v_attrFLDNAME As Xml.XmlAttribute, v_attrFLDTYPE As Xml.XmlAttribute, v_attrOLDVAL As Xml.XmlAttribute

    '        For i = 0 To v_ds.Tables(0).Rows.Count - 1
    '            v_dataElement = pv_xmlDocument.CreateElement(Xml.XmlNodeType.Element, "ObjData", "")
    '            For j = 0 To v_ds.Tables(0).Columns.Count - 1
    '                'Append entry to data node
    '                v_entryNode = pv_xmlDocument.CreateNode(Xml.XmlNodeType.Element, "entry", "")

    '                'Add field name
    '                v_attrFLDNAME = pv_xmlDocument.CreateAttribute("fldname")
    '                v_attrFLDNAME.Value = v_ds.Tables(0).Columns(j).ColumnName
    '                v_entryNode.Attributes.Append(v_attrFLDNAME)

    '                'Add field type
    '                v_attrFLDTYPE = pv_xmlDocument.CreateAttribute("fldtype")
    '                v_attrFLDTYPE.Value = v_ds.Tables(0).Columns(j).DataType.ToString
    '                v_entryNode.Attributes.Append(v_attrFLDTYPE)

    '                'Add current value
    '                v_attrOLDVAL = pv_xmlDocument.CreateAttribute("oldval")
    '                If IsDBNull((v_ds.Tables(0).Rows(i)(j))) Then
    '                    If v_ds.Tables(0).Rows(i)(j).GetType.Name = GetType(System.DateTime).Name _
    '                        Or v_ds.Tables(0).Rows(i)(j).GetType.Name = GetType(System.String).Name Then
    '                        v_attrOLDVAL.Value = ""
    '                    Else
    '                        v_attrOLDVAL.Value = "0"
    '                    End If

    '                Else
    '                    If v_ds.Tables(0).Rows(i)(j).GetType.Name = GetType(System.DateTime).Name Then
    '                        v_attrOLDVAL.Value = Format(v_ds.Tables(0).Rows(i)(j), gc_FORMAT_DATE)
    '                    Else
    '                        v_attrOLDVAL.Value = CStr(v_ds.Tables(0).Rows(i)(j))
    '                    End If
    '                End If
    '                v_entryNode.Attributes.Append(v_attrOLDVAL)

    '                'Set value
    '                If IsDBNull((v_ds.Tables(0).Rows(i)(j))) Then
    '                    v_entryNode.InnerText = ""
    '                Else
    '                    If v_ds.Tables(0).Rows(i)(j).GetType.Name = GetType(System.DateTime).Name Then
    '                        v_entryNode.InnerText = Format(v_ds.Tables(0).Rows(i)(j), gc_FORMAT_DATE)
    '                    Else
    '                        v_entryNode.InnerText = CStr(v_ds.Tables(0).Rows(i)(j))
    '                    End If
    '                End If

    '                v_dataElement.AppendChild(v_entryNode)
    '            Next
    '            pv_xmlDocument.DocumentElement.AppendChild(v_dataElement)
    '        Next

    '        'If v_lngError <> 0 Then
    '        '    Dim v_strErrorSource, v_strErrorMessage As String

    '        '    v_strErrorSource = "SISTORES"
    '        '    v_strErrorMessage = String.Empty

    '        '    LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
    '        '                 & "Error code: " & v_lngError.ToString() & vbNewLine _
    '        '                 & "Error message: " & v_strErrorMessage, EventLogEntryType.Information)
    '        '    BuildXMLErrorException(pv_xmlDocument, v_strErrorSource, v_lngError, v_strErrorMessage)
    '        'End If
    '        v_obj.Commit()
    '        Return v_lngError
    '    Catch ex As Exception
    '        v_obj.Rollback()
    '        'LogError.Write("Error source: " & ex.Source & vbNewLine _
    '        '             & "Error code: System error!" & vbNewLine _
    '        '             & "Error message: " & ex.Message & vbCrLf & v_strSQL, EventLogEntryType.Error)
    '        Throw ex
    '    Finally
    '        If v_trace_status = "1" Then
    '            Trace.WriteLine("[Kết thúc: SI - " & v_strObjName & "] " & DateTime.Now & vbCrLf)
    '            tr2.Close()
    '            tr2.Dispose()
    '        End If
    '        v_obj.Dispose()
    '        GC.Collect()
    '    End Try
    'End Function

    Private Function CoreBaseInquery(ByRef pv_xmlDocument As XmlDocumentEx) As DataSet
        Dim v_obj As New DataAccess
        Dim v_ds As DataSet
        Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes

        Dim v_strObjName As String
        Dim v_strCmdInquiry As String
        Dim v_strCondition As String
        Dim v_strSQL As String
        Try
            If Not (v_attrColl.GetNamedItem(gc_AtributeCMDINQUIRY) Is Nothing) Then
                v_strCmdInquiry = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCMDINQUIRY), Xml.XmlAttribute).Value)
            Else
                v_strCmdInquiry = String.Empty
            End If

            If Not (v_attrColl.GetNamedItem(gc_AtributeCLAUSE) Is Nothing) Then
                v_strCondition = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCLAUSE), Xml.XmlAttribute).Value)
            Else
                v_strCondition = String.Empty
            End If

            If Not (v_attrColl.GetNamedItem(gc_AtributeOBJNAME) Is Nothing) Then
                v_strObjName = Mid(CStr(CType(v_attrColl.GetNamedItem(gc_AtributeOBJNAME), Xml.XmlAttribute).Value), 4)
            Else
                v_strObjName = ATTR_TABLE
            End If

            v_obj.NewDBInstance(gc_MODULE_HOST)

            If Trim(v_strCmdInquiry) <> "" Then
                v_strSQL = v_strCmdInquiry
            Else
                v_strSQL = "SELECT * FROM " & v_strObjName & " WHERE 0=0"
            End If

            If Trim(v_strCondition) <> "" Then
                v_strSQL = v_strSQL & " AND " & v_strCondition
            End If

            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            Return v_ds
        Catch ex As Exception
            Throw ex
        Finally
            If Not v_obj Is Nothing Then
                v_obj.Dispose()
            End If
            If Not v_ds Is Nothing Then
                v_ds.Dispose()
            End If
        End Try
    End Function

    Private Function BaseReplaceString(ByVal pv_strSql As String, ByVal pv_strTableName As String, ByVal pv_strDbLink As String) As String
        Dim v_strSql As String = pv_strSql
        Try
            v_strSql = Replace(v_strSql, " " & UCase(pv_strTableName) & " ", " " & UCase(pv_strTableName) & pv_strDbLink & " ")
            v_strSql = Replace(v_strSql, " " & LCase(pv_strTableName) & " ", " " & UCase(pv_strTableName) & pv_strDbLink & " ")
            v_strSql = Replace(v_strSql, " " & UCase(Mid(pv_strTableName, 1, 1)) & LCase(Mid(pv_strTableName, 2)) & " ", " " & UCase(pv_strTableName) & pv_strDbLink & " ")
            Return v_strSql
        Catch ex As Exception
            Throw ex
            Return pv_strSql
        End Try
    End Function

    Private Function ReplaceSQLDBLink(ByVal pv_strSqlCmd As String, ByVal pv_strDbLink As String) As String
        Dim v_strSqlCmd As String = pv_strSqlCmd
        Dim v_strTableName As String = ""
        Try
            For i As Integer = 0 To mv_arrTableName.Length - 1
                If mv_arrTableName(i) = "TLLOG" Then
                    v_strTableName = "TLLOG"
                    v_strSqlCmd = BaseReplaceString(v_strSqlCmd, v_strTableName, pv_strDbLink)
                ElseIf mv_arrTableName(i) = "RG" Then
                    v_strTableName = "RGII"
                    v_strSqlCmd = BaseReplaceString(v_strSqlCmd, v_strTableName, pv_strDbLink)
                    v_strTableName = "RGIIIA"
                    v_strSqlCmd = BaseReplaceString(v_strSqlCmd, v_strTableName, pv_strDbLink)
                    v_strTableName = "RGIIREP"
                    v_strSqlCmd = BaseReplaceString(v_strSqlCmd, v_strTableName, pv_strDbLink)
                    v_strTableName = "RGIIINFO"
                    v_strSqlCmd = BaseReplaceString(v_strSqlCmd, v_strTableName, pv_strDbLink)
                    v_strTableName = "RGIITRAN"
                    v_strSqlCmd = BaseReplaceString(v_strSqlCmd, v_strTableName, pv_strDbLink)
                    v_strTableName = "RGIS"
                    v_strSqlCmd = BaseReplaceString(v_strSqlCmd, v_strTableName, pv_strDbLink)
                Else
                    v_strTableName = mv_arrTableName(i) & "MAST"
                    v_strSqlCmd = BaseReplaceString(v_strSqlCmd, v_strTableName, pv_strDbLink)
                    v_strTableName = mv_arrTableName(i) & "TRAN"
                    v_strSqlCmd = BaseReplaceString(v_strSqlCmd, v_strTableName, pv_strDbLink)
                End If
            Next
            Return v_strSqlCmd
        Catch ex As Exception
            Throw ex
            Return pv_strSqlCmd
        End Try
    End Function
    Private Function CoreHostInquery2(ByRef pv_xmlDocument As XmlDocumentEx) As DataSet
        Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes

        Dim v_strClause As String
        Dim v_strLocal As String
        Dim v_strObjName As String
        Dim v_strCmdInquiry As String
        Dim v_strCondition As String
        Dim v_strBRID As String
        Dim v_strVsdBrid As String
        Dim v_obj As New DataAccess
        Dim v_arrField() As String

        Dim v_ds As DataSet
        Dim v_strSQL, v_strSQLTmp As String
        Dim tr2 As TextWriterTraceListener
        Dim v_trace_status, v_trace_path As String
        Dim v_lstBrID As String
        Dim v_ds1 As DataSet
        Dim v_trace As DataSet
        Dim v_lngError As Long = ERR_SYSTEM_OK
        Dim blnTran As Boolean = False
        Dim v_strDBLink As String = ""

        Try
            If Not (v_attrColl.GetNamedItem(gc_AtributeCMDINQUIRY) Is Nothing) Then
                v_strCmdInquiry = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCMDINQUIRY), Xml.XmlAttribute).Value)
            Else
                v_strCmdInquiry = String.Empty
            End If

            If Not (v_attrColl.GetNamedItem(gc_AtributeFUNCNAME) Is Nothing) Then
                v_strClause = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeFUNCNAME), Xml.XmlAttribute).Value)
            Else
                v_strClause = String.Empty
            End If

            If Not (v_attrColl.GetNamedItem(gc_AtributeLOCAL) Is Nothing) Then
                v_strLocal = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeLOCAL), Xml.XmlAttribute).Value)
            Else
                v_strLocal = String.Empty
            End If
            If Not (v_attrColl.GetNamedItem(gc_AtributeCLAUSE) Is Nothing) Then
                v_strCondition = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCLAUSE), Xml.XmlAttribute).Value)
            Else
                v_strCondition = String.Empty
            End If

            If Not (v_attrColl.GetNamedItem(gc_AtributeOBJNAME) Is Nothing) Then
                v_strObjName = Mid(CStr(CType(v_attrColl.GetNamedItem(gc_AtributeOBJNAME), Xml.XmlAttribute).Value), 4)
            Else
                v_strObjName = ATTR_TABLE
            End If
            Dim v_strPartition As String
            If Not (v_attrColl.GetNamedItem(gc_AtributeREFERENCE) Is Nothing) Then
                v_strPartition = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeREFERENCE), Xml.XmlAttribute).Value)
            Else
                v_strPartition = ""
            End If

            Dim v_strAutoID As String
            If Not (v_attrColl.GetNamedItem(gc_AtributeAUTOID) Is Nothing) Then
                v_strAutoID = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeAUTOID), Xml.XmlAttribute).Value)
                If Not IsNumeric(v_strAutoID) Then
                    v_strAutoID = "0"
                End If
            Else
                v_strAutoID = "0"
            End If

            v_strBRID = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeBRID), Xml.XmlAttribute).Value)
            If Not (v_attrColl.GetNamedItem(gc_AtributeVSDBRID) Is Nothing) Then
                v_strVsdBrid = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeVSDBRID), Xml.XmlAttribute).Value)
            Else
                v_strVsdBrid = ""
            End If

            Dim v_strTellerID As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTLID), Xml.XmlAttribute).Value)
            Dim v_strTLName As String = v_obj.GetValue("TLPROFILES", "TLNAME", "TLID='" & v_strTellerID & "'")
            mv_strTellerId = v_strTellerID
            mv_strTellerName = v_strTLName

            Dim v_arrFilter As String
            v_arrFilter = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTXNUM), Xml.XmlAttribute).Value)

            Dim v_blnSearch As Boolean = True
            Dim v_strCurrDate As String
            Dim v_strMaxDate, v_strMinDate As String
            v_trace_status = "0"

            'If v_strLocal = gc_IsInQueryNotLocalMsg Then
            '    v_obj.NewDBInstance(gc_MODULE_INQUERY)
            'Else
            '    v_obj.NewDBInstance(gc_MODULE_HOST)
            'End If

            'blnTran = True
            'v_obj.BeginTran()

            v_obj.GetSysVar("SYSTEM", "TRACE_STATUS", v_trace_status)
            v_obj.GetSysVar("SYSTEM", "CURRDATE", v_strBRID, v_strCurrDate)
            v_obj.GetSysVar("SYSTEM", "DATABASE_LINK_HOST", v_strBRID, v_strDBLink)

            If v_obj.ModuleHost = v_obj.ModuleInquery Then
                v_strDBLink = ""
            Else
                v_strDBLink = "@" & v_strDBLink
            End If

            ' date : 14/10/2008
            ' Purpose : get from  date T-3 to date T
            Dim v_strT_T_3 As String
            v_strSQL = "select to_char(GET_T_PLUS(to_date('" & v_strCurrDate & "', 'dd/mm/yyyy'),'" & v_strBRID & "',-1),'dd/mm/yyyy') txdate from dual" _
                    & " union select to_char(GET_T_PLUS(to_date('" & v_strCurrDate & "', 'dd/mm/yyyy'),'" & v_strBRID & "',-2),'dd/mm/yyyy') txdate from dual" _
                    & " union select to_char(GET_T_PLUS(to_date('" & v_strCurrDate & "', 'dd/mm/yyyy'),'" & v_strBRID & "',-3),'dd/mm/yyyy') txdate from dual"
            v_trace = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            v_strT_T_3 = "to_date('" & v_strCurrDate & "', 'dd/mm/yyyy'),"
            For i = 0 To v_trace.Tables(0).Rows.Count - 1
                v_strT_T_3 = v_strT_T_3 & "to_date('" & v_trace.Tables(0).Rows(i)("txdate") & "', 'dd/mm/yyyy'),"
            Next
            v_strT_T_3 = "(" & Left(v_strT_T_3, v_strT_T_3.Length - 1) & ")"
            ' end 

            If v_trace_status = "1" Then
                v_obj.GetSysVar("SYSTEM", "TRACE_PATH", v_trace_path)
                v_strCurrDate = Replace(v_strCurrDate, "/", "_")
                If v_trace_path = "" Then
                    Dim v_app As New ApplicationServices.ApplicationBase
                    v_trace_path = v_app.Info.DirectoryPath & "\Log\" & v_strCurrDate
                Else
                    v_trace_path = v_trace_path & v_strCurrDate
                End If

                If Not System.IO.Directory.Exists(v_trace_path) Then
                    System.IO.Directory.CreateDirectory(v_trace_path)
                End If

                v_trace_path &= "\log_search_br" & v_strBRID & "_" & v_strTLName & ".txt"

                tr2 = New TextWriterTraceListener(System.IO.File.AppendText(v_trace_path))
                Trace.Listeners.Add(tr2)
                Trace.WriteLine("[Bắt đầu: SI - " & v_strObjName & "] " & DateTime.Now & vbCrLf)
            End If

            'Xu ly DL bang TRAN, TLLOG
            v_strSQL = "SELECT TLLOGTRAN FROM SISEARCH WHERE SEARCHCODE = '" & v_strObjName & "' and deleted=0"
            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

            Dim v_strTran As String = "" 'IA|MA|CA|MF|RA|RG|SF|TLLOG
            Dim v_lstTran() As String
            Dim v_blnLoadAll As Boolean = False

            If v_ds.Tables(0).Rows.Count > 0 Then
                v_strTran = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("TLLOGTRAN")).ToUpper
            End If

            v_strMinDate = Replace(v_strCurrDate, "_", "/")
            v_strMaxDate = Replace(v_strCurrDate, "_", "/")
            If v_strPartition <> "" Then
                v_strMinDate = v_strPartition.Split("|")(0)
                v_strMaxDate = v_strPartition.Split("|")(1)
            End If
            v_blnSearch = CheckPassDate(v_strMaxDate, Replace(v_strCurrDate, "_", "/"))
            v_blnLoadAll = CheckPassDate(v_strMinDate, Replace(v_strCurrDate, "_", "/"))


            If CheckPassDate(v_strMinDate, Replace(v_strCurrDate, "_", "/")) Then
                v_obj.NewDBInstance(gc_MODULE_INQUERY)
            Else
                v_obj.NewDBInstance(gc_MODULE_HOST)
                v_strDBLink = ""
            End If

            blnTran = True
            v_obj.BeginTran()

            Dim v_strTllogWhere As String = ""
            If v_blnLoadAll Then
                v_strTllogWhere = " AND [#TXDATE [!O_TXDATE] TO_DATE('[!V_TXDATE]','" & gc_FORMAT_DATE & "')#] AND [#TXNUM [!O_TXNUM] '[!V_TXNUM]'#]"
                v_strTllogWhere &= " AND [#BUSDATE [!O_TXDATE] TO_DATE('[!V_BUSDATE]','" & gc_FORMAT_DATE & "')#] AND [#TLTXCD [!O_TLTXCD] '[!V_TLTXCD]'#]"
            End If

            If v_strTran <> "" Then
                v_lstTran = v_strTran.Split("|")
                v_strTran = ""
                For i As Integer = 0 To v_lstTran.Length - 1
                    Select Case v_lstTran(i)
                        Case "TLLOG"
                            v_strTran &= ",'TLLOG'"

                            If v_blnLoadAll And v_strLocal = gc_IsInQueryNotLocalMsg Then
                                v_strTran &= ",'TLLOGALL'"
                            End If
                        Case "IA", "MA", "RA", "SF"
                            v_strTran &= ",'" & v_lstTran(i) & "TRAN'"
                            v_strTran &= ",'" & v_lstTran(i) & "MAST'"

                            If v_blnLoadAll Then
                                v_strTran &= ",'" & v_lstTran(i) & "TRANA'"
                            End If
                        Case "CA"
                            v_strTran &= ",'" & v_lstTran(i) & "TRAN'"
                            If v_strPartition <> "" Then
                                v_strTran &= ",'" & v_lstTran(i) & "TRANA'"
                                v_blnLoadAll = True
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
            Dim v_intCount As Integer = -1

            If v_strCondition <> "" Then
                v_arrField = v_strCondition.Split("#")
                v_intCount = v_arrField.Length - 2
            End If
            Dim v_hCondition As New Hashtable
            Dim v_strhKey, v_strhValue As String
            Dim v_blnReplace As Boolean

            If v_intCount >= 0 Then
                For j = 0 To v_intCount
                    v_strhKey = v_arrField(j).Split("|")(0).ToUpper
                    Select Case v_strhKey
                        Case "SICODE"
                            v_lstSICODE = "('" & v_arrField(j).Split("|")(2) & "')"
                        Case "MICODE"
                            v_lstMICODE = "('" & v_arrField(j).Split("|")(2) & "')"
                    End Select
                Next
            End If
            'Lay phan quyen chi nhanh
            If v_strDBLink <> "" Then
                v_strSQL = "SELECT DISTINCT brid brid  FROM " _
                       & " (SELECT b.brid FROM tlbridauth " & v_strDBLink & " a, brgrp" & v_strDBLink & " b" _
                       & " WHERE ((AUTHID = '" & v_strTellerID & "' AND authtype = 'U') OR (AUTHID IN (SELECT a.grpid" _
                       & " FROM tlgrpusers" & v_strDBLink & " a WHERE a.tlid = '" & v_strTellerID & "') AND authtype = 'G'))" _
                       & " And a.brid = b.brid And a.deleted = 0 And a.status = 0" _
                       & " And b.deleted = 0 And b.status = 0" _
                       & " UNION " _
                       & " SELECT b.brid FROM tlprofiles" & v_strDBLink & " a, brgrp" & v_strDBLink & " b" _
                       & " WHERE a.brid = b.brid AND a.tlid = '" & v_strTellerID & "'" _
                       & " AND a.deleted = 0 AND a.status = 0 AND b.deleted = 0 AND b.status = 0)"

            Else
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
            End If

            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

            v_lstBrID = ""
            For i = 0 To v_ds.Tables(0).Rows.Count - 1
                v_lstBrID &= ",'" & v_ds.Tables(0).Rows(i)(0) & "'"
            Next
            v_lstBrID = "(" & Mid(v_lstBrID, 2) & ")"

            'Lay TVLK dc phan quyen cho user vao bang TMP_RGMI
            If v_strDBLink <> "" Then
                v_strSQL = "INSERT INTO tmp_rgmi " _
                       & " SELECT DISTINCT m.* FROM rgmi" & v_strDBLink & " m WHERE m.deleted=0 AND m.status=0" _
                       & " AND m.MICODE IN " & v_lstMICODE
            Else
                v_strSQL = "INSERT INTO tmp_rgmi " _
                       & " SELECT DISTINCT m.* FROM rgmi m WHERE m.deleted=0 AND m.status=0" _
                       & " AND m.MICODE IN " & v_lstMICODE
            End If

            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

            'Lay chung khoan dc phan quyen cho user vao bang TMP_RGSI
            ' ngay : 05092009 
            ' muc dich : tach chuoi v_lstSICODE khac phuc loi 
            '               ORA-01795: maximum number of expressions in a list is 1000
            Dim v_strCut, v_strCut1, v_strCut2, v_strCut3 As String
            Dim v_iCut As Integer
            v_iCut = (v_lstSICODE.Length \ 3)
            v_iCut = InStr(v_iCut, v_lstSICODE, ",")
            If v_iCut > 0 Then
                v_strCut1 = v_lstSICODE.Substring(0, v_iCut - 1) & ")"
                If v_strDBLink <> "" Then
                    v_strSQL = "INSERT INTO tmp_rgsi SELECT m.* FROM rgsi" & v_strDBLink & " m" _
                                & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_strCut1
                Else
                    v_strSQL = "INSERT INTO tmp_rgsi SELECT m.* FROM rgsi m" _
                                & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_strCut1
                End If
                v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                'Write Log
                'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "CoreBusiness", "objMaster", "CoreHostInquery", "", "", mv_strTellerId, mv_strTellerName, "Excute SQL Command: " & v_strSQL)

                v_strCut = "(" & v_lstSICODE.Substring(v_iCut)
                v_iCut = InStr(v_iCut, v_strCut, ",")
                If v_iCut > 0 Then
                    v_strCut2 = v_strCut.Substring(0, v_iCut - 1) & ")"
                    If v_strDBLink <> "" Then
                        v_strSQL = "INSERT INTO tmp_rgsi SELECT m.* FROM rgsi" & v_strDBLink & " m" _
                                    & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_strCut2
                    Else
                        v_strSQL = "INSERT INTO tmp_rgsi SELECT m.* FROM rgsi m" _
                                    & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_strCut2
                    End If
                    v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                    'Write Log
                    'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "CoreBusiness", "objMaster", "CoreHostInquery", "", "", mv_strTellerId, mv_strTellerName, "Excute SQL Command: " & v_strSQL)

                    v_strCut3 = "(" & v_strCut.Substring(v_iCut)
                    If v_strDBLink <> "" Then
                        v_strSQL = "INSERT INTO tmp_rgsi SELECT m.* FROM rgsi" & v_strDBLink & " m" _
                                    & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_strCut3
                    Else
                        v_strSQL = "INSERT INTO tmp_rgsi SELECT m.* FROM rgsi m" _
                                    & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_strCut3
                    End If
                    v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                Else
                    If v_strDBLink <> "" Then
                        v_strSQL = "INSERT INTO tmp_rgsi SELECT m.* FROM rgsi" & v_strDBLink & " m" _
                                    & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_strCut
                    Else
                        v_strSQL = "INSERT INTO tmp_rgsi SELECT m.* FROM rgsi m" _
                                    & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_strCut
                    End If
                    v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                End If
            Else
                If v_strDBLink <> "" Then
                    v_strSQL = "INSERT INTO tmp_rgsi SELECT m.* FROM rgsi" & v_strDBLink & " m" _
                                & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_lstSICODE
                Else
                    v_strSQL = "INSERT INTO tmp_rgsi SELECT m.* FROM rgsi m" _
                                & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_lstSICODE
                End If
                v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
            End If
            ' tuanta
            v_strSQL = "INSERT INTO TMP_RGSI(AUTOID, SICODE) SELECT 0 AUTOID, '000' SICODE FROM DUAL"
            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

            'Lay cau xu ly chung
            Dim v_hSQL As New Hashtable
            Dim v_hORD As New Hashtable
            If v_strTran <> "" Then
                v_strSQL = "SELECT * FROM SISTORES WHERE deleted =0 and SEARCHCODE IS NULL AND OVERWRITE IN " & v_strTran & " ORDER BY ODRNUM"
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
                v_strSQL = "SELECT * FROM SISTORES WHERE deleted=0 and SEARCHCODE='" & v_strObjName & "' AND OVERWRITE IN " & v_strTran & " ORDER BY ODRNUM"
                v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                'Write Log
                'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "CoreBusiness", "objMaster", "CoreHostInquery", "", "", mv_strTellerId, mv_strTellerName, "Excute SQL Command: " & v_strSQL)

                If v_ds.Tables(0).Rows.Count > 0 Then
                    For i As Integer = 0 To v_ds.Tables(0).Rows.Count - 1
                        v_strOverWrite = v_ds.Tables(0).Rows(i)("OVERWRITE")
                        If Not v_hSQL(v_strOverWrite) Is Nothing Then
                            v_hSQL(v_strOverWrite) = v_ds.Tables(0).Rows(i)("TRACE") & v_ds.Tables(0).Rows(i)("CMDSQL") & " " & v_ds.Tables(0).Rows(i)("CMDSQL1")
                        End If
                    Next
                End If

                'Thuc hien cau xu ly chung
                Dim v_strPartitionSQL As String
                'Dim v_lstPartition As String
                For i As Integer = 0 To v_hORD.Count - 1
                    v_strOverWrite = v_hORD(i)
                    v_strSQL = Mid(v_hSQL(v_strOverWrite), 2)
                    'v_strPartitionSQL = ""
                    v_strSQL = Replace(v_strSQL, "[!BRID]", v_strBRID)
                    v_strSQL = Replace(v_strSQL, "[!VSD_BRID]", v_strVsdBrid)
                    v_strSQL = Replace(v_strSQL, "[!AUTOID]", v_strAutoID)
                    v_strSQL = Replace(v_strSQL, "[!TLID]", v_strTellerID)
                    v_strSQL = Replace(v_strSQL, "[!LBRID]", v_lstBrID)
                    v_strSQL = Replace(v_strSQL, "[!LMICODE]", v_lstMICODE)
                    v_strSQL = Replace(v_strSQL, "[!T_T_3]", v_strT_T_3)
                    v_strSQL = Replace(v_strSQL, "[!MINDATE]", "TO_DATE('" & v_strMinDate & "','dd/mm/yyyy')")
                    v_strSQL = Replace(v_strSQL, "[!MAXDATE]", "TO_DATE('" & v_strMaxDate & "','dd/mm/yyyy')")

                    v_strSQL = Replace(v_strSQL, "TT_", "")
                    v_strSQL = Replace(v_strSQL, "tt_", "")
                    v_strSQL = Replace(v_strSQL, "tT_", "")
                    v_strSQL = Replace(v_strSQL, "Tt_", "")

                    v_strSQL = Replace(v_strSQL, "[!TLLOGALL]", "TLLOGALL_ALL")
                    v_strSQL = Replace(v_strSQL, "[!CATRANA]", "CATRANA_ALL")
                    v_strSQL = Replace(v_strSQL, "[!IATRANA]", "IATRANA_ALL")
                    v_strSQL = Replace(v_strSQL, "[!MATRANA]", "MATRANA_ALL")
                    v_strSQL = Replace(v_strSQL, "[!SFTRANA]", "SFTRANA_ALL")
                    v_strSQL = Replace(v_strSQL, "[!MFTRANA]", "MFTRANA_ALL")
                    v_strSQL = Replace(v_strSQL, "[!RATRANA]", "RATRANA_ALL")
                    v_strSQL = Replace(v_strSQL, "[!RGTRANA]", "RGTRANA_ALL")

                    If Right(v_strOverWrite, 3) = "ALL" Or Right(v_strOverWrite, 1) = "A" Then
                        If v_strOverWrite = "TLLOGALL" Then
                            v_strPartitionSQL = v_strSQL.Substring(InStr(v_strSQL.ToUpper, "SELECT") - 1)
                            v_strSQL = Mid(v_strSQL, 1, InStr(v_strSQL.ToUpper, "SELECT") - 1)
                            v_strPartitionSQL = " SELECT * FROM (" & v_strPartitionSQL & ") WHERE 1=1" & v_strTllogWhere
                            v_strSQL &= v_strPartitionSQL
                        End If

                        If v_intCount >= 0 Then
                            For j = 0 To v_intCount
                                v_strSQL = Replace(v_strSQL, "[!O_" & v_arrField(j).Split("|")(0) & "]", v_arrField(j).Split("|")(1))
                                v_strSQL = Replace(v_strSQL, "[!V_" & v_arrField(j).Split("|")(0) & "]", v_arrField(j).Split("|")(2))
                            Next
                        End If
                    End If

                    v_strSQL = Replace(v_strSQL, "[!V_TXDATE]", Replace(v_strCurrDate, "_", "/"))
                    v_strSQL = DecodeSQL(v_strSQL)

                    'Thay the cau lenh bang DB Link
                    If v_strDBLink <> "" Then
                        v_strSQL = ReplaceSQLDBLink(v_strSQL, v_strDBLink)
                    End If

                    If v_trace_status = "1" And Mid(v_hSQL(v_strOverWrite).ToString, 1, 1) = "1" Then
                        Trace.WriteLine("[SI - " & v_strObjName & "] " & DateTime.Now & " :" & vbCrLf & " -o- Câu lệnh thứ xử lý chung thứ #" & i + 1 & "-o-" & vbCrLf & v_strSQL & vbCrLf)
                        If InStr(v_strSQL.ToUpper, "SELECT") > 0 Then
                            Trace.WriteLine("-o- Dữ liệu câu lệnh xử lý chung thứ #" & i + 1 & "-o-")
                            v_strSQLTmp = v_strSQL.Substring(InStr(v_strSQL.ToUpper, "SELECT") - 1)
                            v_trace = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                            Trace.WriteLine(v_trace.GetXml & vbCrLf)
                        End If
                    End If
                    v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                Next
            End If

            v_strSQL = "SELECT * FROM SISTORES WHERE SEARCHCODE='" & v_strObjName & "' AND OVERWRITE IS NULL AND DELETED=0 AND STATUS=0 ORDER BY ODRNUM"
            v_ds1 = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

            'Su ly cau lay SQL trong SISTORES
            For i = 0 To v_ds1.Tables(0).Rows.Count - 1
                v_strSQL = v_ds1.Tables(0).Rows(i)("CMDSQL").ToString.ToUpper & " " & v_ds1.Tables(0).Rows(i)("CMDSQL1").ToString.ToUpper
                v_blnReplace = False

                If v_intCount >= 0 Then
                    For j = 0 To v_intCount
                        v_strhKey = v_arrField(j).Split("|")(0)
                        v_strhValue = "[!V_" & v_arrField(j).Split("|")(0) & "]"
                        If InStr(v_strSQL, v_strhValue, CompareMethod.Text) > 0 Then
                            v_strSQL = Replace(v_strSQL, "[!O_" & v_arrField(j).Split("|")(0) & "]", v_arrField(j).Split("|")(1))
                            v_strSQL = Replace(v_strSQL, "[!V_" & v_arrField(j).Split("|")(0) & "]", v_arrField(j).Split("|")(2))

                            If v_hCondition(v_strhKey) Is Nothing Then
                                v_hCondition.Add(v_strhKey, v_strhValue)
                            End If
                            v_blnReplace = True
                        End If
                    Next
                End If

                If (Not v_blnReplace And v_ds1.Tables(0).Rows(i)("SITYPE") = 0) Or v_blnReplace Then
                    v_strSQL = DecodeSQL(v_strSQL)
                    'Neu khong su dung bang TMP thi thong bao loi
                    'If Not CheckTranTable(v_strSQL) Then
                    '    v_lngError = 1
                    '    Exit For
                    'End If

                    v_strSQL = Replace(v_strSQL, "[!BRID]", v_strBRID)
                    v_strSQL = Replace(v_strSQL, "[!VSD_BRID]", v_strVsdBrid)
                    v_strSQL = Replace(v_strSQL, "[!AUTOID]", v_strAutoID)
                    v_strSQL = Replace(v_strSQL, "[!TLID]", v_strTellerID)
                    v_strSQL = Replace(v_strSQL, "[!LBRID]", v_lstBrID)
                    v_strSQL = Replace(v_strSQL, "[!LMICODE]", v_lstMICODE)
                    v_strSQL = Replace(v_strSQL, "[!T_T_3]", v_strT_T_3)
                    v_strSQL = Replace(v_strSQL, "[!MINDATE]", "TO_DATE('" & v_strMinDate & "','dd/mm/yyyy')")
                    v_strSQL = Replace(v_strSQL, "[!MAXDATE]", "TO_DATE('" & v_strMaxDate & "','dd/mm/yyyy')")

                    v_strSQL = Replace(v_strSQL, "TT_", "")
                    v_strSQL = Replace(v_strSQL, "tt_", "")
                    v_strSQL = Replace(v_strSQL, "tT_", "")
                    v_strSQL = Replace(v_strSQL, "Tt_", "")

                    'Thay the cau lenh bang DB Link
                    If v_strDBLink <> "" Then
                        v_strSQL = ReplaceSQLDBLink(v_strSQL, v_strDBLink)
                    End If

                    If v_trace_status = "1" And v_ds1.Tables(0).Rows(i)("TRACE") = 1 Then
                        Trace.WriteLine("[SI - " & v_strObjName & "] " & DateTime.Now & " :" & vbCrLf & " -o- Câu lệnh thứ #" & v_ds1.Tables(0).Rows(i)("ODRNUM") & "-o-" & vbCrLf & v_strSQL & vbCrLf)
                        If InStr(v_strSQL.ToUpper, "SELECT") > 0 Then
                            Trace.WriteLine("-o- Dữ liệu câu lệnh thứ #" & v_ds1.Tables(0).Rows(i)("ODRNUM") & "-o-")
                            v_strSQLTmp = v_strSQL.Substring(InStr(v_strSQL.ToUpper, "SELECT") - 1)
                            v_trace = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                            Trace.WriteLine(v_trace.GetXml & vbCrLf)
                        End If
                    End If
                    v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                End If
            Next

            If v_lngError = ERR_SYSTEM_OK Then
                v_strSQL = v_strCmdInquiry

                'If Not CheckTranTable(v_strSQL) Then
                '    v_lngError = 1
                'Else
                If v_intCount >= 0 Then
                    For i = 0 To v_intCount
                        v_strhKey = v_arrField(i).Split("|")(0)
                        v_strhValue = "[!V_" & v_arrField(i).Split("|")(0) & "]"
                        If InStr(v_strSQL, v_strhValue, CompareMethod.Text) > 0 Then
                            v_strSQL = Replace(v_strSQL, "[!O_" & v_arrField(i).Split("|")(0) & "]", v_arrField(i).Split("|")(1))
                            v_strSQL = Replace(v_strSQL, "[!V_" & v_arrField(i).Split("|")(0) & "]", v_arrField(i).Split("|")(2))

                            If v_hCondition(v_strhKey) Is Nothing Then
                                v_hCondition.Add(v_strhKey, v_strhValue)
                            End If
                        End If
                    Next
                End If

                v_strSQL = DecodeSQL(v_strSQL)
                v_strSQL = Replace(v_strSQL, "TT_", "")
                v_strSQL = Replace(v_strSQL, "tt_", "")
                v_strSQL = Replace(v_strSQL, "tT_", "")
                v_strSQL = Replace(v_strSQL, "Tt_", "")
                Dim v_lngRowCount As Long

                Dim v_strWhere As String = ""
                'purpose : filter_group_x
                Dim v_strFILTERGROUP(10) As String
                Dim v_arrGroupID() As String
                For i = 1 To 9
                    v_strFILTERGROUP(i) = ""
                Next
                'end filter_group_x

                Dim v_strTmp, v_strTmpField, v_strTmpField1, v_strValue As String
                If v_strCondition <> "" Then
                    'purpose : filter_group_x
                    v_strSQLTmp = "select FIELDCODE,groupid from sisearchfld where not groupid is null and deleted=0 and SEARCHCODE='" & v_strObjName & "'"
                    v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                    'end filter_group_x

                    v_arrField = v_strCondition.Split("#")
                    v_intCount = v_arrField.Length - 2

                    For i = 0 To v_arrField.Length - 2
                        If v_hCondition(v_arrField(i).Split("|")(0)) Is Nothing Then
                            'v_strWhere &= " AND " & v_arrField(i).Split("|")(0) & " " & v_arrField(i).Split("|")(1) & " "
                            v_strValue = v_arrField(i).Split("|")(2)
                            Select Case v_arrField(i).Split("|")(3)
                                Case "D", "P"
                                    v_strTmpField = "TO_DATE(" & v_arrField(i).Split("|")(0) & ",'" & gc_FORMAT_DATE & "') " & v_arrField(i).Split("|")(1) & " "
                                    v_strTmpField1 = v_arrField(i).Split("|")(0) & " " & v_arrField(i).Split("|")(1) & " "
                                    v_strTmp = "TO_DATE('" & v_strValue & "', '" & gc_FORMAT_DATE & "')"
                                Case "N"
                                    v_strTmpField = v_arrField(i).Split("|")(0) & " " & v_arrField(i).Split("|")(1) & " "
                                    v_strTmpField1 = v_strTmpField
                                    If IsNumeric(v_strValue) Then
                                        v_strTmp = CDbl(v_strValue)
                                    Else
                                        v_strTmp = 0
                                    End If
                                Case Else
                                    v_strTmpField = "UPPER(" & v_arrField(i).Split("|")(0) & ") " & v_arrField(i).Split("|")(1) & " "
                                    v_strTmpField1 = v_strTmpField
                                    v_strValue = Trim(Replace(v_strValue, ".", String.Empty))
                                    If v_arrField(i).Split("|")(1) = "LIKE" Then
                                        v_strTmp = "UPPER(N'%" & v_strValue & "%')"
                                    Else
                                        v_strTmp = "UPPER(N'" & v_strValue & "')"
                                    End If
                            End Select
                            'purpose : filter_group_x
                            If v_ds.Tables(0).Rows.Count > 0 Then
                                For j = 0 To v_ds.Tables(0).Rows.Count - 1
                                    If v_arrField(i).Split("|")(0) = v_ds.Tables(0).Rows(j)("FIELDCODE") Then
                                        v_arrGroupID = v_ds.Tables(0).Rows(j)("groupid").ToString.Split("|")
                                        For t = 0 To v_arrGroupID.Length - 1
                                            v_strFILTERGROUP(CInt(v_arrGroupID(t).Substring(1, 1))) &= "AND " _
                                                & v_strTmpField1.Replace(v_ds.Tables(0).Rows(j)("FIELDCODE"), _
                                                                        v_arrGroupID(t).Substring(0, 1) _
                                                                        & "." _
                                                                        & v_ds.Tables(0).Rows(j)("FIELDCODE")) _
                                                & v_strTmp.Replace("N'", "'")
                                        Next
                                    End If
                                Next
                            End If
                            'end filter_group_x
                            v_strWhere &= " AND " & v_strTmpField & v_strTmp
                        End If
                    Next
                End If
                'purpose : filter_group_x
                Dim v_strForcedGroupArr() As String

                v_strSQLTmp = "select forced_group from sisearch where deleted=0 and SEARCHCODE='" & v_strObjName & "'"
                v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                If Not IsDBNull(v_ds.Tables(0).Rows(0)("forced_group")) Then
                    v_strForcedGroupArr = v_ds.Tables(0).Rows(0)("forced_group").ToString.Split("|")
                End If

                For i = 1 To 9
                    If Not IsDBNull(v_ds.Tables(0).Rows(0)("forced_group")) Then
                        For j = 0 To v_strForcedGroupArr.Length - 1
                            If v_strForcedGroupArr(j) = i Then
                                v_strFILTERGROUP(i) = IIf(v_strFILTERGROUP(i).Trim = "", " and 1=0 ", v_strFILTERGROUP(i))
                            End If
                        Next
                    End If
                    v_strSQL = Replace(v_strSQL, "?FITLER_GROUP_" & i, v_strFILTERGROUP(i))
                Next
                'end filter_group_x
                If Trim(v_strWhere) <> "" Then
                    v_strSQL = "SELECT * FROM (" & v_strSQL & ") WHERE 1=1 " & v_strWhere
                End If
                v_strSQL = Replace(v_strSQL, "[!V_TXDATE]", Replace(v_strCurrDate, "_", "/"))
                v_strSQL = Replace(v_strSQL, "[!MINDATE]", "TO_DATE('" & v_strMinDate & "','dd/mm/yyyy')")
                v_strSQL = Replace(v_strSQL, "[!MAXDATE]", "TO_DATE('" & v_strMaxDate & "','dd/mm/yyyy')")
                v_strSQL = Replace(v_strSQL, "[!BRID]", v_strBRID)
                v_strSQL = Replace(v_strSQL, "[!T_T_3]", v_strT_T_3)
                v_strSQL = Replace(v_strSQL, "[!LMICODE]", v_lstMICODE)
                v_strSQL = Replace(v_strSQL, "[!LBRID]", v_lstBrID)

                'Thay the cau lenh bang DB Link
                If v_strDBLink <> "" Then
                    v_strSQL = ReplaceSQLDBLink(v_strSQL, v_strDBLink)
                End If

                If v_strSQL <> "" Then
                    'Tinh so ban ghi
                    v_strSQLTmp = "SELECT COUNT(1) FROM (" & v_strSQL & ")"
                    v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)

                    v_lngRowCount = v_ds.Tables(0).Rows(0)(0)

                    If Not (v_attrColl.GetNamedItem(gc_AtributeFUNCNAME) Is Nothing) Then
                        pv_xmlDocument.DocumentElement.Attributes(gc_AtributeFUNCNAME).InnerXml = v_lngRowCount
                    Else
                        Dim v_attr As Xml.XmlAttribute
                        v_attr = pv_xmlDocument.CreateAttribute(gc_AtributeFUNCNAME)
                        v_attr.Value = v_lngRowCount
                        Dim dataElement As Xml.XmlElement
                        dataElement = pv_xmlDocument.DocumentElement
                        dataElement.Attributes.Append(v_attr)
                    End If

                    'Lay DL loc theo dong
                    Dim v_intFrom, v_intTo As Integer
                    If v_strClause <> "" Then
                        v_intFrom = CInt(v_strClause.Split("|")(0))
                        v_intTo = CInt(v_strClause.Split("|")(1))

                        v_strSQLTmp = "SELECT * FROM (SELECT T.*,rownum rn FROM (" & v_strSQL & ") T) WHERE RN BETWEEN " & v_intFrom & " AND " & v_intTo
                    Else
                        v_strSQLTmp = v_strSQL
                    End If

                    If v_trace_status = "1" Then
                        Trace.WriteLine("[SI - " & v_strObjName & "] " & DateTime.Now & " :" & vbCrLf & " -o- Ket qua:-o-" & vbCrLf & v_strSQL & vbCrLf)
                    End If

                    v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                End If
                'End If
            End If

            v_obj.Commit()
            Return v_ds
        Catch ex As Exception
            If blnTran Then v_obj.Commit()
            Throw ex
        Finally
            If v_trace_status = "1" Then
                Trace.WriteLine("[Kết thúc: SI - " & v_strObjName & "] " & DateTime.Now & vbCrLf)
                tr2.Close()
                tr2.Dispose()
            End If
            v_obj.Dispose()
            If Not v_ds Is Nothing Then
                v_ds.Dispose()
            End If
            If Not v_ds1 Is Nothing Then
                v_ds1.Dispose()
            End If
            GC.Collect()
        End Try
    End Function
    Private Function CoreHostInquery(ByRef pv_xmlDocument As XmlDocumentEx) As DataSet
        Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes

        Dim v_strClause As String
        Dim v_strLocal As String
        Dim v_strObjName As String
        Dim v_strCmdInquiry As String
        Dim v_strCondition As String
        Dim v_strBRID As String
        Dim v_strVsdBrid As String
        Dim v_obj As New DataAccess
        Dim v_arrField() As String

        Dim v_ds As DataSet
        Dim v_strSQL, v_strSQLTmp As String
        Dim tr2 As TextWriterTraceListener
        Dim v_trace_status, v_trace_path As String
        Dim v_lstBrID As String
        Dim v_ds1 As DataSet
        Dim v_trace As DataSet
        Dim v_lngError As Long = ERR_SYSTEM_OK
        Dim blnTran As Boolean = False

        Try
            If Not (v_attrColl.GetNamedItem(gc_AtributeCMDINQUIRY) Is Nothing) Then
                v_strCmdInquiry = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCMDINQUIRY), Xml.XmlAttribute).Value)
            Else
                v_strCmdInquiry = String.Empty
            End If

            If Not (v_attrColl.GetNamedItem(gc_AtributeFUNCNAME) Is Nothing) Then
                v_strClause = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeFUNCNAME), Xml.XmlAttribute).Value)
            Else
                v_strClause = String.Empty
            End If

            If Not (v_attrColl.GetNamedItem(gc_AtributeLOCAL) Is Nothing) Then
                v_strLocal = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeLOCAL), Xml.XmlAttribute).Value)
            Else
                v_strLocal = String.Empty
            End If
            If Not (v_attrColl.GetNamedItem(gc_AtributeCLAUSE) Is Nothing) Then
                v_strCondition = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCLAUSE), Xml.XmlAttribute).Value)
            Else
                v_strCondition = String.Empty
            End If

            If Not (v_attrColl.GetNamedItem(gc_AtributeOBJNAME) Is Nothing) Then
                v_strObjName = Mid(CStr(CType(v_attrColl.GetNamedItem(gc_AtributeOBJNAME), Xml.XmlAttribute).Value), 4)
            Else
                v_strObjName = ATTR_TABLE
            End If
            Dim v_strPartition As String
            If Not (v_attrColl.GetNamedItem(gc_AtributeREFERENCE) Is Nothing) Then
                v_strPartition = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeREFERENCE), Xml.XmlAttribute).Value)
            Else
                v_strPartition = ""
            End If

            Dim v_strAutoID As String
            If Not (v_attrColl.GetNamedItem(gc_AtributeAUTOID) Is Nothing) Then
                v_strAutoID = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeAUTOID), Xml.XmlAttribute).Value)
                If Not IsNumeric(v_strAutoID) Then
                    v_strAutoID = "0"
                End If
            Else
                v_strAutoID = "0"
            End If

            v_strBRID = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeBRID), Xml.XmlAttribute).Value)
            If Not (v_attrColl.GetNamedItem(gc_AtributeVSDBRID) Is Nothing) Then
                v_strVsdBrid = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeVSDBRID), Xml.XmlAttribute).Value)
            Else
                v_strVsdBrid = ""
            End If

            Dim v_strTellerID As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTLID), Xml.XmlAttribute).Value)
            Dim v_strTLName As String = v_obj.GetValue("TLPROFILES", "TLNAME", "TLID='" & v_strTellerID & "'")
            mv_strTellerId = v_strTellerID
            mv_strTellerName = v_strTLName

            Dim v_arrFilter As String
            v_arrFilter = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTXNUM), Xml.XmlAttribute).Value)

            Dim v_blnSearch As Boolean = True
            Dim v_strCurrDate As String
            Dim v_strMaxDate, v_strMinDate As String
            v_trace_status = "0"

            If v_strLocal = gc_IsInQueryNotLocalMsg Then
                v_obj.NewDBInstance(gc_MODULE_INQUERY)
            Else
                v_obj.NewDBInstance(gc_MODULE_HOST)
            End If

            blnTran = True
            v_obj.BeginTran()

            v_obj.GetSysVar("SYSTEM", "TRACE_STATUS", v_trace_status)
            v_obj.GetSysVar("SYSTEM", "CURRDATE", v_strBRID, v_strCurrDate)
            ' date : 14/10/2008
            ' Purpose : get from  date T-3 to date T
            Dim v_strT_T_3 As String
            v_strSQL = "select to_char(GET_T_PLUS(to_date('" & v_strCurrDate & "', 'dd/mm/yyyy'),'" & v_strBRID & "',-1),'dd/mm/yyyy') txdate from dual" _
                    & " union select to_char(GET_T_PLUS(to_date('" & v_strCurrDate & "', 'dd/mm/yyyy'),'" & v_strBRID & "',-2),'dd/mm/yyyy') txdate from dual" _
                    & " union select to_char(GET_T_PLUS(to_date('" & v_strCurrDate & "', 'dd/mm/yyyy'),'" & v_strBRID & "',-3),'dd/mm/yyyy') txdate from dual"
            v_trace = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            v_strT_T_3 = "to_date('" & v_strCurrDate & "', 'dd/mm/yyyy'),"
            For i = 0 To v_trace.Tables(0).Rows.Count - 1
                v_strT_T_3 = v_strT_T_3 & "to_date('" & v_trace.Tables(0).Rows(i)("txdate") & "', 'dd/mm/yyyy'),"
            Next
            v_strT_T_3 = "(" & Left(v_strT_T_3, v_strT_T_3.Length - 1) & ")"
            ' end 

            If v_trace_status = "1" Then
                v_obj.GetSysVar("SYSTEM", "TRACE_PATH", v_trace_path)
                v_strCurrDate = Replace(v_strCurrDate, "/", "_")
                If v_trace_path = "" Then
                    Dim v_app As New ApplicationServices.ApplicationBase
                    v_trace_path = v_app.Info.DirectoryPath & "\Log\" & v_strCurrDate
                Else
                    v_trace_path = v_trace_path & v_strCurrDate
                End If

                If Not System.IO.Directory.Exists(v_trace_path) Then
                    System.IO.Directory.CreateDirectory(v_trace_path)
                End If

                v_trace_path &= "\log_search_br" & v_strBRID & "_" & v_strTLName & ".txt"

                tr2 = New TextWriterTraceListener(System.IO.File.AppendText(v_trace_path))
                Trace.Listeners.Add(tr2)
                Trace.WriteLine("[Bắt đầu: SI - " & v_strObjName & "] " & DateTime.Now & vbCrLf)
            End If

            'Xu ly DL bang TRAN, TLLOG
            v_strSQL = "SELECT TLLOGTRAN FROM SISEARCH WHERE SEARCHCODE = '" & v_strObjName & "' and deleted=0"
            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

            Dim v_strTran As String = "" 'IA|MA|CA|MF|RA|RG|SF|TLLOG
            Dim v_lstTran() As String
            Dim v_blnLoadAll As Boolean = False

            If v_ds.Tables(0).Rows.Count > 0 Then
                v_strTran = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("TLLOGTRAN")).ToUpper
            End If

            v_strMinDate = Replace(v_strCurrDate, "_", "/")
            v_strMaxDate = Replace(v_strCurrDate, "_", "/")
            If v_strPartition <> "" Then
                v_strMinDate = v_strPartition.Split("|")(0)
                v_strMaxDate = v_strPartition.Split("|")(1)
            End If
            v_blnSearch = CheckPassDate(v_strMaxDate, Replace(v_strCurrDate, "_", "/"))
            v_blnLoadAll = CheckPassDate(v_strMinDate, Replace(v_strCurrDate, "_", "/"))


            Dim v_strTllogWhere As String = ""
            If v_blnLoadAll Then
                v_strTllogWhere = " AND [#TXDATE [!O_TXDATE] TO_DATE('[!V_TXDATE]','" & gc_FORMAT_DATE & "')#] AND [#TXNUM [!O_TXNUM] '[!V_TXNUM]'#]"
                v_strTllogWhere &= " AND [#BUSDATE [!O_TXDATE] TO_DATE('[!V_BUSDATE]','" & gc_FORMAT_DATE & "')#] AND [#TLTXCD [!O_TLTXCD] '[!V_TLTXCD]'#]"
            End If

            If v_strTran <> "" Then
                v_lstTran = v_strTran.Split("|")
                v_strTran = ""
                For i As Integer = 0 To v_lstTran.Length - 1
                    Select Case v_lstTran(i)
                        Case "TLLOG"
                            v_strTran &= ",'TLLOG'"

                            If v_blnLoadAll And v_strLocal = gc_IsInQueryNotLocalMsg Then
                                v_strTran &= ",'TLLOGALL'"
                            End If
                        Case "IA", "MA", "RA", "SF"
                            v_strTran &= ",'" & v_lstTran(i) & "TRAN'"
                            v_strTran &= ",'" & v_lstTran(i) & "MAST'"

                            If v_blnLoadAll Then
                                v_strTran &= ",'" & v_lstTran(i) & "TRANA'"
                            End If
                        Case "CA"
                            v_strTran &= ",'" & v_lstTran(i) & "TRAN'"
                            If v_strPartition <> "" Then
                                v_strTran &= ",'" & v_lstTran(i) & "TRANA'"
                                v_blnLoadAll = True
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
            Dim v_intCount As Integer = -1

            If v_strCondition <> "" Then
                v_arrField = v_strCondition.Split("#")
                v_intCount = v_arrField.Length - 2
            End If
            Dim v_hCondition As New Hashtable
            Dim v_strhKey, v_strhValue As String
            Dim v_blnReplace As Boolean

            'If v_intCount >= 0 Then
            '    For j = 0 To v_intCount
            '        v_strhKey = v_arrField(j).Split("|")(0).ToUpper
            '        Select Case v_strhKey
            '            Case "SICODE"
            '                v_lstSICODE = "('" & v_arrField(j).Split("|")(2) & "')"
            '            Case "MICODE"
            '                v_lstMICODE = "('" & v_arrField(j).Split("|")(2) & "')"
            '        End Select
            '    Next
            'End If
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

            'Lay TVLK dc phan quyen cho user vao bang TMP_RGMI
            v_strSQL = "INSERT INTO tmp_rgmi " _
                       & " SELECT DISTINCT m.* FROM rgmi m WHERE m.deleted=0 AND m.status=0" _
                       & " AND m.MICODE IN " & v_lstMICODE

            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

            'v_strSQL = "INSERT INTO TMP_RGMI(AUTOID, MICODE) SELECT '0' AUTOID, '000' MICODE FROM DUAL"
            'v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

            'Lay chung khoan dc phan quyen cho user vao bang TMP_RGSI
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
            '    v_strSQL = "INSERT INTO tmp_rgsi(iscode, sicode, stock_name, part_value, TYPE," _
            '    & "stock_type, interest_rate, interest_period, " _
            '    & "bond_period, deleted, exchange_rate, note, " _
            '    & "bond_term, release_series, release_mode, isin, " _
            '    & "rarate, tmpid, paidmethod, npaiddate1, npaiddate2, " _
            '    & "npaiddate3, npaiddate4, int_release_mode, brid, status, public_date, currency_code,vsdbrid) " _
            '    & " SELECT distinct m.iscode, m.sicode, m.stock_name, m.part_value, m.TYPE," _
            '    & "m.stock_type, m.interest_rate, m.interest_period," _
            '    & "m.bond_period, m.deleted, m.exchange_rate, m.note, " _
            '    & "m.bond_term, m.release_series, m.release_mode, m.isin, " _
            '    & "m.rarate, m.tmpid, m.paidmethod, m.npaiddate1, m.npaiddate2," _
            '    & "m.npaiddate3, m.npaiddate4, m.int_release_mode, m.brid, m.status, m.public_date, m.currency_code,m.vsdbrid FROM rgsi m" _
            '    & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_strCut1
            '    v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

            '    v_strCut = "(" & v_lstSICODE.Substring(v_iCut)
            '    v_iCut = InStr(v_iCut, v_strCut, ",")
            '    If v_iCut > 0 Then
            '        v_strCut2 = v_strCut.Substring(0, v_iCut - 1) & ")"
            '        v_strSQL = "INSERT INTO tmp_rgsi(iscode, sicode, stock_name, part_value, TYPE," _
            '              & "stock_type, interest_rate, interest_period, " _
            '              & "bond_period, deleted, exchange_rate, note, " _
            '              & "bond_term, release_series, release_mode, isin, " _
            '              & "rarate, tmpid, paidmethod, npaiddate1, npaiddate2, " _
            '              & "npaiddate3, npaiddate4, int_release_mode, brid, status, public_date, currency_code,vsdbrid) " _
            '              & " SELECT distinct m.iscode, m.sicode, m.stock_name, m.part_value, m.TYPE," _
            '              & "m.stock_type, m.interest_rate, m.interest_period," _
            '              & "m.bond_period, m.deleted, m.exchange_rate, m.note, " _
            '              & "m.bond_term, m.release_series, m.release_mode, m.isin, " _
            '              & "m.rarate, m.tmpid, m.paidmethod, m.npaiddate1, m.npaiddate2," _
            '              & "m.npaiddate3, m.npaiddate4, m.int_release_mode, m.brid, m.status, m.public_date, m.currency_code,m.vsdbrid FROM rgsi m" _
            '              & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_strCut2
            '        v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

            '        v_strCut = "(" & v_strCut.Substring(v_iCut)
            '        v_iCut = InStr(v_iCut, v_strCut, ",")
            '        '3
            '        If v_iCut > 0 Then
            '            v_strCut3 = v_strCut.Substring(0, v_iCut - 1) & ")"
            '            v_strSQL = "INSERT INTO tmp_rgsi(iscode, sicode, stock_name, part_value, TYPE," _
            '                    & "stock_type, interest_rate, interest_period, " _
            '                    & "bond_period, deleted, exchange_rate, note, " _
            '                    & "bond_term, release_series, release_mode, isin, " _
            '                    & "rarate, tmpid, paidmethod, npaiddate1, npaiddate2, " _
            '                    & "npaiddate3, npaiddate4, int_release_mode, brid, status, public_date, currency_code,vsdbrid) " _
            '                    & " SELECT distinct m.iscode, m.sicode, m.stock_name, m.part_value, m.TYPE," _
            '                    & "m.stock_type, m.interest_rate, m.interest_period," _
            '                    & "m.bond_period, m.deleted, m.exchange_rate, m.note, " _
            '                    & "m.bond_term, m.release_series, m.release_mode, m.isin, " _
            '                    & "m.rarate, m.tmpid, m.paidmethod, m.npaiddate1, m.npaiddate2," _
            '                    & "m.npaiddate3, m.npaiddate4, m.int_release_mode, m.brid, m.status, m.public_date, m.currency_code,m.vsdbrid FROM rgsi m" _
            '                    & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_strCut3
            '            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

            '            v_strCut = "(" & v_strCut.Substring(v_iCut)
            '            v_iCut = InStr(v_iCut, v_strCut, ",")
            '            If v_iCut > 0 Then
            '                v_strCut4 = v_strCut.Substring(0, v_iCut - 1) & ")"
            '                v_strSQL = "INSERT INTO tmp_rgsi(iscode, sicode, stock_name, part_value, TYPE," _
            '                        & "stock_type, interest_rate, interest_period, " _
            '                        & "bond_period, deleted, exchange_rate, note, " _
            '                        & "bond_term, release_series, release_mode, isin, " _
            '                        & "rarate, tmpid, paidmethod, npaiddate1, npaiddate2, " _
            '                        & "npaiddate3, npaiddate4, int_release_mode, brid, status, public_date, currency_code,vsdbrid) " _
            '                        & " SELECT distinct m.iscode, m.sicode, m.stock_name, m.part_value, m.TYPE," _
            '                        & "m.stock_type, m.interest_rate, m.interest_period," _
            '                        & "m.bond_period, m.deleted, m.exchange_rate, m.note, " _
            '                        & "m.bond_term, m.release_series, m.release_mode, m.isin, " _
            '                        & "m.rarate, m.tmpid, m.paidmethod, m.npaiddate1, m.npaiddate2," _
            '                        & "m.npaiddate3, m.npaiddate4, m.int_release_mode, m.brid, m.status, m.public_date, m.currency_code,m.vsdbrid FROM rgsi m" _
            '                        & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_strCut4
            '                v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

            '                v_strCut5 = "(" & v_strCut.Substring(v_iCut)

            '                v_strSQL = "INSERT INTO tmp_rgsi(iscode, sicode, stock_name, part_value, TYPE," _
            '                        & "stock_type, interest_rate, interest_period, " _
            '                        & "bond_period, deleted, exchange_rate, note, " _
            '                        & "bond_term, release_series, release_mode, isin, " _
            '                        & "rarate, tmpid, paidmethod, npaiddate1, npaiddate2, " _
            '                        & "npaiddate3, npaiddate4, int_release_mode, brid, status, public_date, currency_code,vsdbrid) " _
            '                        & " SELECT distinct m.iscode, m.sicode, m.stock_name, m.part_value, m.TYPE," _
            '                        & "m.stock_type, m.interest_rate, m.interest_period," _
            '                        & "m.bond_period, m.deleted, m.exchange_rate, m.note, " _
            '                        & "m.bond_term, m.release_series, m.release_mode, m.isin, " _
            '                        & "m.rarate, m.tmpid, m.paidmethod, m.npaiddate1, m.npaiddate2," _
            '                        & "m.npaiddate3, m.npaiddate4, m.int_release_mode, m.brid, m.status, m.public_date, m.currency_code,m.vsdbrid FROM rgsi m" _
            '                        & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_strCut5
            '                v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

            '            Else
            '                v_strSQL = "INSERT INTO tmp_rgsi(iscode, sicode, stock_name, part_value, TYPE," _
            '                          & "stock_type, interest_rate, interest_period, " _
            '                          & "bond_period, deleted, exchange_rate, note, " _
            '                          & "bond_term, release_series, release_mode, isin, " _
            '                          & "rarate, tmpid, paidmethod, npaiddate1, npaiddate2, " _
            '                          & "npaiddate3, npaiddate4, int_release_mode, brid, status, public_date, currency_code,vsdbrid) " _
            '                          & " SELECT distinct m.iscode, m.sicode, m.stock_name, m.part_value, m.TYPE," _
            '                          & "m.stock_type, m.interest_rate, m.interest_period," _
            '                          & "m.bond_period, m.deleted, m.exchange_rate, m.note, " _
            '                          & "m.bond_term, m.release_series, m.release_mode, m.isin, " _
            '                          & "m.rarate, m.tmpid, m.paidmethod, m.npaiddate1, m.npaiddate2," _
            '                          & "m.npaiddate3, m.npaiddate4, m.int_release_mode, m.brid, m.status, m.public_date, m.currency_code,m.vsdbrid FROM rgsi m" _
            '                          & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_strCut
            '                v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
            '            End If
            '        End If
            '        End If
            '    Else
            '        v_strSQL = "INSERT INTO tmp_rgsi(iscode, sicode, stock_name, part_value, TYPE," _
            '                      & "stock_type, interest_rate, interest_period, " _
            '                      & "bond_period, deleted, exchange_rate, note, " _
            '                      & "bond_term, release_series, release_mode, isin, " _
            '                      & "rarate, tmpid, paidmethod, npaiddate1, npaiddate2, " _
            '                      & "npaiddate3, npaiddate4, int_release_mode, brid, status, public_date, currency_code,vsdbrid) " _
            '                      & " SELECT distinct m.iscode, m.sicode, m.stock_name, m.part_value, m.TYPE," _
            '                      & "m.stock_type, m.interest_rate, m.interest_period," _
            '                      & "m.bond_period, m.deleted, m.exchange_rate, m.note, " _
            '                      & "m.bond_term, m.release_series, m.release_mode, m.isin, " _
            '                      & "m.rarate, m.tmpid, m.paidmethod, m.npaiddate1, m.npaiddate2," _
            '                      & "m.npaiddate3, m.npaiddate4, m.int_release_mode, m.brid, m.status, m.public_date, m.currency_code,m.vsdbrid FROM rgsi m" _
            '                      & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_lstSICODE
            '        v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

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
                v_strSQL = "SELECT * FROM SISTORES WHERE deleted =0 and SEARCHCODE IS NULL AND OVERWRITE IN " & v_strTran & " ORDER BY ODRNUM"

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
                v_strSQL = "SELECT * FROM SISTORES WHERE deleted=0 and SEARCHCODE='" & v_strObjName & "' AND OVERWRITE IN " & v_strTran & " ORDER BY ODRNUM"
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
                Dim v_strPartitionSQL As String
                'Dim v_lstPartition As String
                For i As Integer = 0 To v_hORD.Count - 1
                    v_strOverWrite = v_hORD(i)
                    v_strSQL = Mid(v_hSQL(v_strOverWrite), 2)
                    'v_strPartitionSQL = ""
                    v_strSQL = Replace(v_strSQL, "[!BRID]", v_strBRID)
                    v_strSQL = Replace(v_strSQL, "[!VSD_BRID]", v_strVsdBrid)
                    v_strSQL = Replace(v_strSQL, "[!AUTOID]", v_strAutoID)
                    v_strSQL = Replace(v_strSQL, "[!TLID]", v_strTellerID)
                    v_strSQL = Replace(v_strSQL, "[!LBRID]", v_lstBrID)
                    v_strSQL = Replace(v_strSQL, "[!LMICODE]", v_lstMICODE)
                    v_strSQL = Replace(v_strSQL, "[!T_T_3]", v_strT_T_3)
                    v_strSQL = Replace(v_strSQL, "[!MINDATE]", "TO_DATE('" & v_strMinDate & "','dd/mm/yyyy')")
                    v_strSQL = Replace(v_strSQL, "[!MAXDATE]", "TO_DATE('" & v_strMaxDate & "','dd/mm/yyyy')")

                    v_strSQL = Replace(v_strSQL, "TT_", "")
                    v_strSQL = Replace(v_strSQL, "tt_", "")
                    v_strSQL = Replace(v_strSQL, "tT_", "")
                    v_strSQL = Replace(v_strSQL, "Tt_", "")

                    v_strSQL = Replace(v_strSQL, "[!TLLOGALL]", "TLLOGALL_ALL")
                    v_strSQL = Replace(v_strSQL, "[!CATRANA]", "CATRANA_ALL")
                    v_strSQL = Replace(v_strSQL, "[!IATRANA]", "IATRANA_ALL")
                    v_strSQL = Replace(v_strSQL, "[!MATRANA]", "MATRANA_ALL")
                    v_strSQL = Replace(v_strSQL, "[!SFTRANA]", "SFTRANA_ALL")
                    v_strSQL = Replace(v_strSQL, "[!MFTRANA]", "MFTRANA_ALL")
                    v_strSQL = Replace(v_strSQL, "[!RATRANA]", "RATRANA_ALL")
                    v_strSQL = Replace(v_strSQL, "[!RGTRANA]", "RGTRANA_ALL")

                    If Right(v_strOverWrite, 3) = "ALL" Or Right(v_strOverWrite, 1) = "A" Then
                        'If Mid(v_strOverWrite, 1, 5).ToUpper = "TLLOG" Then
                        '    v_lstPartition = GetListPartition(v_strPartition)
                        'Else
                        '    v_lstPartition = GetListPartition(v_strMinDate & "|" & Replace(v_strCurrDate, "_", "/"))
                        'End If

                        'For j As Integer = 0 To v_lstPartition.Split("|").Length - 2
                        '    If v_strPartitionSQL <> "" Then
                        '        v_strPartitionSQL &= " UNION ALL "
                        '    End If

                        '    v_strPartitionSQL &= v_strSQL.Substring(InStr(v_strSQL.ToUpper, "SELECT") - 1)
                        'v_strPartitionSQL = Replace(v_strPartitionSQL, "[!TLLOGALL]", "TLLOGALL_ALL PARTITION(" & v_lstPartition.Split("|")(j) & ")")
                        'v_strPartitionSQL = Replace(v_strPartitionSQL, "[!CATRANA]", "CATRANA_ALL PARTITION(" & v_lstPartition.Split("|")(j) & ")")
                        'v_strPartitionSQL = Replace(v_strPartitionSQL, "[!IATRANA]", "IATRANA_ALL PARTITION(" & v_lstPartition.Split("|")(j) & ")")
                        'v_strPartitionSQL = Replace(v_strPartitionSQL, "[!MATRANA]", "MATRANA_ALL PARTITION(" & v_lstPartition.Split("|")(j) & ")")
                        'v_strPartitionSQL = Replace(v_strPartitionSQL, "[!SFTRANA]", "SFTRANA_ALL PARTITION(" & v_lstPartition.Split("|")(j) & ")")
                        'v_strPartitionSQL = Replace(v_strPartitionSQL, "[!MFTRANA]", "MFTRANA_ALL PARTITION(" & v_lstPartition.Split("|")(j) & ")")
                        'v_strPartitionSQL = Replace(v_strPartitionSQL, "[!RATRANA]", "RATRANA_ALL PARTITION(" & v_lstPartition.Split("|")(j) & ")")
                        'v_strPartitionSQL = Replace(v_strPartitionSQL, "[!RGTRANA]", "RGTRANA_ALL PARTITION(" & v_lstPartition.Split("|")(j) & ")")
                        'Next
                        If v_strOverWrite = "TLLOGALL" Then
                            v_strPartitionSQL = v_strSQL.Substring(InStr(v_strSQL.ToUpper, "SELECT") - 1)
                            v_strSQL = Mid(v_strSQL, 1, InStr(v_strSQL.ToUpper, "SELECT") - 1)
                            v_strPartitionSQL = " SELECT * FROM (" & v_strPartitionSQL & ") WHERE 1=1" & v_strTllogWhere
                            v_strSQL &= v_strPartitionSQL
                        End If

                        If v_intCount >= 0 Then
                            For j = 0 To v_intCount
                                v_strSQL = Replace(v_strSQL, "[!O_" & v_arrField(j).Split("|")(0) & "]", v_arrField(j).Split("|")(1))
                                v_strSQL = Replace(v_strSQL, "[!V_" & v_arrField(j).Split("|")(0) & "]", v_arrField(j).Split("|")(2))
                            Next
                        End If
                    End If

                    v_strSQL = Replace(v_strSQL, "[!V_TXDATE]", Replace(v_strCurrDate, "_", "/"))
                    v_strSQL = DecodeSQL(v_strSQL)

                    If v_trace_status = "1" And Mid(v_hSQL(v_strOverWrite).ToString, 1, 1) = "1" Then
                        Trace.WriteLine("[SI - " & v_strObjName & "] " & DateTime.Now & " :" & vbCrLf & " -o- Câu lệnh thứ xử lý chung thứ #" & i + 1 & "-o-" & vbCrLf & v_strSQL & vbCrLf)
                        If InStr(v_strSQL.ToUpper, "SELECT") > 0 Then
                            Trace.WriteLine("-o- Dữ liệu câu lệnh xử lý chung thứ #" & i + 1 & "-o-")
                            v_strSQLTmp = v_strSQL.Substring(InStr(v_strSQL.ToUpper, "SELECT") - 1)
                            v_trace = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                            Trace.WriteLine(v_trace.GetXml & vbCrLf)
                        End If
                    End If
                    v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

                Next
            End If

            v_strSQL = "SELECT * FROM SISTORES WHERE SEARCHCODE='" & v_strObjName & "' AND OVERWRITE IS NULL AND DELETED=0 AND STATUS=0 ORDER BY ODRNUM"
            v_ds1 = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

            'Su ly cau lay SQL trong SISTORES
            For i = 0 To v_ds1.Tables(0).Rows.Count - 1
                v_strSQL = v_ds1.Tables(0).Rows(i)("CMDSQL").ToString.ToUpper & " " & v_ds1.Tables(0).Rows(i)("CMDSQL1").ToString.ToUpper
                v_blnReplace = False

                If v_intCount >= 0 Then
                    For j = 0 To v_intCount
                        v_strhKey = v_arrField(j).Split("|")(0)
                        v_strhValue = "[!V_" & v_arrField(j).Split("|")(0) & "]"
                        If InStr(v_strSQL, v_strhValue, CompareMethod.Text) > 0 Then
                            v_strSQL = Replace(v_strSQL, "[!O_" & v_arrField(j).Split("|")(0) & "]", v_arrField(j).Split("|")(1))
                            v_strSQL = Replace(v_strSQL, "[!V_" & v_arrField(j).Split("|")(0) & "]", v_arrField(j).Split("|")(2))

                            If v_hCondition(v_strhKey) Is Nothing Then
                                v_hCondition.Add(v_strhKey, v_strhValue)
                            End If
                            v_blnReplace = True
                        End If
                    Next
                End If

                If (Not v_blnReplace And v_ds1.Tables(0).Rows(i)("SITYPE") = 0) Or v_blnReplace Then
                    v_strSQL = DecodeSQL(v_strSQL)
                    'Neu khong su dung bang TMP thi thong bao loi
                    'If Not CheckTranTable(v_strSQL) Then
                    '    v_lngError = 1
                    '    Exit For
                    'End If

                    v_strSQL = Replace(v_strSQL, "[!BRID]", v_strBRID)
                    v_strSQL = Replace(v_strSQL, "[!VSD_BRID]", v_strVsdBrid)
                    v_strSQL = Replace(v_strSQL, "[!AUTOID]", v_strAutoID)
                    v_strSQL = Replace(v_strSQL, "[!TLID]", v_strTellerID)
                    v_strSQL = Replace(v_strSQL, "[!LBRID]", v_lstBrID)
                    v_strSQL = Replace(v_strSQL, "[!LMICODE]", v_lstMICODE)
                    v_strSQL = Replace(v_strSQL, "[!T_T_3]", v_strT_T_3)
                    v_strSQL = Replace(v_strSQL, "[!MINDATE]", "TO_DATE('" & v_strMinDate & "','dd/mm/yyyy')")
                    v_strSQL = Replace(v_strSQL, "[!MAXDATE]", "TO_DATE('" & v_strMaxDate & "','dd/mm/yyyy')")

                    v_strSQL = Replace(v_strSQL, "TT_", "")
                    v_strSQL = Replace(v_strSQL, "tt_", "")
                    v_strSQL = Replace(v_strSQL, "tT_", "")
                    v_strSQL = Replace(v_strSQL, "Tt_", "")

                    If v_trace_status = "1" And v_ds1.Tables(0).Rows(i)("TRACE") = 1 Then
                        Trace.WriteLine("[SI - " & v_strObjName & "] " & DateTime.Now & " :" & vbCrLf & " -o- Câu lệnh thứ #" & v_ds1.Tables(0).Rows(i)("ODRNUM") & "-o-" & vbCrLf & v_strSQL & vbCrLf)
                        If InStr(v_strSQL.ToUpper, "SELECT") > 0 Then
                            Trace.WriteLine("-o- Dữ liệu câu lệnh thứ #" & v_ds1.Tables(0).Rows(i)("ODRNUM") & "-o-")
                            v_strSQLTmp = v_strSQL.Substring(InStr(v_strSQL.ToUpper, "SELECT") - 1)
                            v_trace = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                            Trace.WriteLine(v_trace.GetXml & vbCrLf)
                        End If
                    End If

                    v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

                End If
            Next

            If v_lngError = ERR_SYSTEM_OK Then
                v_strSQL = v_strCmdInquiry

                'If Not CheckTranTable(v_strSQL) Then
                '    v_lngError = 1
                'Else
                If v_intCount >= 0 Then
                    For i = 0 To v_intCount
                        v_strhKey = v_arrField(i).Split("|")(0)
                        v_strhValue = "[!V_" & v_arrField(i).Split("|")(0) & "]"
                        If InStr(v_strSQL, v_strhValue, CompareMethod.Text) > 0 Then
                            v_strSQL = Replace(v_strSQL, "[!O_" & v_arrField(i).Split("|")(0) & "]", v_arrField(i).Split("|")(1))
                            v_strSQL = Replace(v_strSQL, "[!V_" & v_arrField(i).Split("|")(0) & "]", v_arrField(i).Split("|")(2))

                            If v_hCondition(v_strhKey) Is Nothing Then
                                v_hCondition.Add(v_strhKey, v_strhValue)
                            End If
                        End If
                    Next
                End If

                v_strSQL = DecodeSQL(v_strSQL)
                v_strSQL = Replace(v_strSQL, "TT_", "")
                v_strSQL = Replace(v_strSQL, "tt_", "")
                v_strSQL = Replace(v_strSQL, "tT_", "")
                v_strSQL = Replace(v_strSQL, "Tt_", "")
                Dim v_lngRowCount As Long

                Dim v_strWhere As String = ""
                'purpose : filter_group_x
                Dim v_strFILTERGROUP(10) As String
                Dim v_arrGroupID() As String
                For i = 1 To 9
                    v_strFILTERGROUP(i) = ""
                Next
                'end filter_group_x

                Dim v_strTmp, v_strTmpField, v_strTmpField1, v_strValue As String
                If v_strCondition <> "" Then
                    'purpose : filter_group_x
                    v_strSQLTmp = "select FIELDCODE,groupid from sisearchfld where not groupid is null and deleted=0 and SEARCHCODE='" & v_strObjName & "'"
                    v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                    'end filter_group_x

                    v_arrField = v_strCondition.Split("#")
                    v_intCount = v_arrField.Length - 2

                    For i = 0 To v_arrField.Length - 2
                        If v_hCondition(v_arrField(i).Split("|")(0)) Is Nothing Then
                            'v_strWhere &= " AND " & v_arrField(i).Split("|")(0) & " " & v_arrField(i).Split("|")(1) & " "
                            v_strValue = v_arrField(i).Split("|")(2)
                            Select Case v_arrField(i).Split("|")(3)
                                Case "D", "P"
                                    v_strTmpField = "TO_DATE(" & v_arrField(i).Split("|")(0) & ",'" & gc_FORMAT_DATE & "') " & v_arrField(i).Split("|")(1) & " "
                                    v_strTmpField1 = v_arrField(i).Split("|")(0) & " " & v_arrField(i).Split("|")(1) & " "
                                    v_strTmp = "TO_DATE('" & v_strValue & "', '" & gc_FORMAT_DATE & "')"
                                Case "N"
                                    v_strTmpField = v_arrField(i).Split("|")(0) & " " & v_arrField(i).Split("|")(1) & " "
                                    v_strTmpField1 = v_strTmpField
                                    If IsNumeric(v_strValue) Then
                                        v_strTmp = CDbl(v_strValue)
                                    Else
                                        v_strTmp = 0
                                    End If
                                Case Else
                                    v_strTmpField = "UPPER(" & v_arrField(i).Split("|")(0) & ") " & v_arrField(i).Split("|")(1) & " "
                                    v_strTmpField1 = v_strTmpField
                                    'bangpv 28/08/2012: rao lai, khong hieu y nghia cua doan nay
                                    'v_strValue = Trim(Replace(v_strValue, ".", String.Empty))
                                    'end bangpv
                                    If v_arrField(i).Split("|")(1) = "LIKE" Then
                                        v_strTmp = "UPPER(N'%" & v_strValue & "%')"
                                    Else
                                        v_strTmp = "UPPER(N'" & v_strValue & "')"
                                    End If
                            End Select
                            'purpose : filter_group_x
                            If v_ds.Tables(0).Rows.Count > 0 Then
                                For j = 0 To v_ds.Tables(0).Rows.Count - 1
                                    If v_arrField(i).Split("|")(0) = v_ds.Tables(0).Rows(j)("FIELDCODE") Then
                                        v_arrGroupID = v_ds.Tables(0).Rows(j)("groupid").ToString.Split("|")
                                        For t = 0 To v_arrGroupID.Length - 1
                                            v_strFILTERGROUP(CInt(v_arrGroupID(t).Substring(1, 1))) &= "AND " _
                                                & v_strTmpField1.Replace("UPPER(", "").Replace(")", "").Replace(v_ds.Tables(0).Rows(j)("FIELDCODE"), _
                                                                        v_arrGroupID(t).Substring(0, 1) _
                                                                        & "." _
                                                                        & v_ds.Tables(0).Rows(j)("FIELDCODE")) _
                                                & IIf("DPN".IndexOf(v_arrField(i).Split("|")(3)) >= 0, v_strTmp, v_strTmp.Replace("UPPER(", "").Replace(")", ""))
                                            'bangpv: sua lai de bo dieu kien upper cua ca filter ben trong 
                                        Next
                                    End If
                                Next
                            End If
                            'end filter_group_x
                            v_strWhere &= " AND " & v_strTmpField & v_strTmp
                        End If
                    Next
                End If
                'purpose : filter_group_x
                Dim v_strForcedGroupArr() As String

                v_strSQLTmp = "select forced_group from sisearch where deleted=0 and SEARCHCODE='" & v_strObjName & "'"
                v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                If Not IsDBNull(v_ds.Tables(0).Rows(0)("forced_group")) Then
                    v_strForcedGroupArr = v_ds.Tables(0).Rows(0)("forced_group").ToString.Split("|")
                End If

                For i = 1 To 9
                    If Not IsDBNull(v_ds.Tables(0).Rows(0)("forced_group")) Then
                        For j = 0 To v_strForcedGroupArr.Length - 1
                            If v_strForcedGroupArr(j) = i Then
                                v_strFILTERGROUP(i) = IIf(v_strFILTERGROUP(i).Trim = "", " and 1=0 ", v_strFILTERGROUP(i))
                            End If
                        Next
                    End If
                    v_strSQL = Replace(v_strSQL, "?FITLER_GROUP_" & i, v_strFILTERGROUP(i))
                Next
                ' end filter_group_x
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
                If Trim(v_strWhere) <> "" Then
                    v_strSQL = "SELECT * FROM (" & v_strSQL & ") WHERE 1=1 " & v_strWhere
                End If
                v_strSQL = Replace(v_strSQL, "[!V_TXDATE]", Replace(v_strCurrDate, "_", "/"))
                v_strSQL = Replace(v_strSQL, "[!MINDATE]", "TO_DATE('" & v_strMinDate & "','dd/mm/yyyy')")
                v_strSQL = Replace(v_strSQL, "[!MAXDATE]", "TO_DATE('" & v_strMaxDate & "','dd/mm/yyyy')")
                v_strSQL = Replace(v_strSQL, "[!BRID]", v_strBRID)
                v_strSQL = Replace(v_strSQL, "[!TLID]", v_strTellerID)
                v_strSQL = Replace(v_strSQL, "[!T_T_3]", v_strT_T_3)
                v_strSQL = Replace(v_strSQL, "[!LMICODE]", v_lstMICODE)
                v_strSQL = Replace(v_strSQL, "[!LBRID]", v_lstBrID)

                If v_strSQL <> "" Then
                    'Tinh so ban ghi
                    v_strSQLTmp = "SELECT COUNT(1) FROM (" & v_strSQL & ")"
                    v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)

                    v_lngRowCount = v_ds.Tables(0).Rows(0)(0)

                    If Not (v_attrColl.GetNamedItem(gc_AtributeFUNCNAME) Is Nothing) Then
                        pv_xmlDocument.DocumentElement.Attributes(gc_AtributeFUNCNAME).InnerXml = v_lngRowCount
                    Else
                        Dim v_attr As Xml.XmlAttribute
                        v_attr = pv_xmlDocument.CreateAttribute(gc_AtributeFUNCNAME)
                        v_attr.Value = v_lngRowCount
                        Dim dataElement As Xml.XmlElement
                        dataElement = pv_xmlDocument.DocumentElement
                        dataElement.Attributes.Append(v_attr)
                    End If

                    'Lay DL loc theo dong
                    Dim v_intFrom, v_intTo As Integer

                    If v_strClause <> "" Then
                        v_intFrom = CInt(v_strClause.Split("|")(0))
                        v_intTo = CInt(v_strClause.Split("|")(1))

                        v_strSQLTmp = "SELECT * FROM (SELECT T.*,rownum rn FROM (" & v_strSQL & ") T) WHERE RN BETWEEN " & v_intFrom & " AND " & v_intTo
                    Else
                        v_strSQLTmp = v_strSQL
                    End If

                    If v_trace_status = "1" Then
                        Trace.WriteLine("[SI - " & v_strObjName & "] " & DateTime.Now & " :" & vbCrLf & " -o- Ket qua:-o-" & vbCrLf & v_strSQL & vbCrLf)
                    End If

                    v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                End If
                'End If
            End If

            v_obj.Commit()
            blnTran = False
            Return v_ds
        Catch ex As Exception
            If blnTran Then v_obj.Commit()
            Throw ex
        Finally
            If v_trace_status = "1" Then
                Trace.WriteLine("[Kết thúc: SI - " & v_strObjName & "] " & DateTime.Now & vbCrLf)
                tr2.Close()
                tr2.Dispose()
            End If
            v_obj.Dispose()
            If Not v_ds Is Nothing Then
                v_ds.Dispose()
            End If
            If Not v_ds1 Is Nothing Then
                v_ds1.Dispose()
            End If
            GC.Collect()
        End Try
    End Function

    Private Function CoreHostExport(ByRef pv_xmlDocument As XmlDocumentEx) As String
        Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes

        Dim v_strClause As String
        Dim v_strLocal As String
        Dim v_strObjName As String
        Dim v_strCmdInquiry As String
        Dim v_strCondition As String
        Dim v_strBRID As String
        Dim v_strVsdBrid As String
        Dim v_obj As New DataAccess
        Dim v_arrField() As String

        Dim v_ds As DataSet
        Dim v_strSQL, v_strSQLTmp As String
        Dim tr2 As TextWriterTraceListener
        Dim v_trace_status, v_trace_path As String
        Dim v_strSearchResultRootPath, v_strSearchResultPath As String
        Dim v_strServerAddress, v_strServerPort, v_strUserName, v_strPassword As String
        Dim v_lstBrID As String
        Dim v_ds1 As DataSet
        Dim v_trace As DataSet
        Dim v_lngError As Long = ERR_SYSTEM_OK
        Try
            If Not (v_attrColl.GetNamedItem(gc_AtributeCMDINQUIRY) Is Nothing) Then
                v_strCmdInquiry = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCMDINQUIRY), Xml.XmlAttribute).Value)
            Else
                v_strCmdInquiry = String.Empty
            End If

            If Not (v_attrColl.GetNamedItem(gc_AtributeFUNCNAME) Is Nothing) Then
                v_strClause = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeFUNCNAME), Xml.XmlAttribute).Value)
            Else
                v_strClause = String.Empty
            End If

            If Not (v_attrColl.GetNamedItem(gc_AtributeLOCAL) Is Nothing) Then
                v_strLocal = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeLOCAL), Xml.XmlAttribute).Value)
            Else
                v_strLocal = String.Empty
            End If
            If Not (v_attrColl.GetNamedItem(gc_AtributeCLAUSE) Is Nothing) Then
                v_strCondition = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCLAUSE), Xml.XmlAttribute).Value)
            Else
                v_strCondition = String.Empty
            End If

            If Not (v_attrColl.GetNamedItem(gc_AtributeOBJNAME) Is Nothing) Then
                v_strObjName = Mid(CStr(CType(v_attrColl.GetNamedItem(gc_AtributeOBJNAME), Xml.XmlAttribute).Value), 4)
            Else
                v_strObjName = ATTR_TABLE
            End If
            Dim v_strPartition As String
            If Not (v_attrColl.GetNamedItem(gc_AtributeREFERENCE) Is Nothing) Then
                v_strPartition = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeREFERENCE), Xml.XmlAttribute).Value)
            Else
                v_strPartition = ""
            End If

            Dim v_strAutoID As String
            If Not (v_attrColl.GetNamedItem(gc_AtributeAUTOID) Is Nothing) Then
                v_strAutoID = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeAUTOID), Xml.XmlAttribute).Value)
                If Not IsNumeric(v_strAutoID) Then
                    v_strAutoID = "0"
                End If
            Else
                v_strAutoID = "0"
            End If

            v_strBRID = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeBRID), Xml.XmlAttribute).Value)
            If Not (v_attrColl.GetNamedItem(gc_AtributeVSDBRID) Is Nothing) Then
                v_strVsdBrid = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeVSDBRID), Xml.XmlAttribute).Value)
            Else
                v_strVsdBrid = ""
            End If

            Dim v_strTellerID As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTLID), Xml.XmlAttribute).Value)
            Dim v_strTLName As String = v_obj.GetValue("TLPROFILES", "TLNAME", "TLID='" & v_strTellerID & "'")
            mv_strTellerId = v_strTellerID
            mv_strTellerName = v_strTLName

            Dim v_arrFilter As String
            v_arrFilter = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTXNUM), Xml.XmlAttribute).Value)

            'Write Log
            'mv_lwLogWriter.StartWriteLog(gc_MODULE_HOST, "CoreBusiness", "objMaster", "CoreHostExport", "", "", mv_strTellerId, mv_strTellerName, "")

            Dim v_blnSearch As Boolean = True
            Dim v_strCurrDate As String
            Dim v_strMaxDate, v_strMinDate As String
            v_trace_status = "0"

            If v_strLocal = gc_IsInQueryNotLocalMsg Then
                v_obj.NewDBInstance(gc_MODULE_INQUERY)
            Else
                v_obj.NewDBInstance(gc_MODULE_HOST)
            End If

            v_obj.BeginTran()

            v_obj.GetSysVar("SYSTEM", "TRACE_STATUS", v_trace_status)
            v_obj.GetSysVar("SYSTEM", "CURRDATE", v_strBRID, v_strCurrDate)

            v_obj.GetSysVar("SYSTEM", "FTP_SERVER_ADDRESS", v_strServerAddress)
            v_obj.GetSysVar("SYSTEM", "FTP_SERVER_PORT", v_strServerPort)
            v_obj.GetSysVar("SYSTEM", "FTP_USERNAME", v_strUserName)
            v_obj.GetSysVar("SYSTEM", "FTP_PASSWORD", v_strPassword)
            v_obj.GetSysVar("SYSTEM", "SEARCH_RESULT_PATH", v_strSearchResultRootPath)


            v_strCurrDate = Replace(v_strCurrDate, "/", "_")
            If v_strSearchResultRootPath = "" Then
                Dim v_app As New ApplicationServices.ApplicationBase
                v_strSearchResultRootPath = v_app.Info.DirectoryPath & "\VSD_SRC_RESULT\"
            End If

            v_strSearchResultPath = v_strCurrDate

            If Not System.IO.Directory.Exists(v_strSearchResultRootPath & v_strCurrDate) Then
                System.IO.Directory.CreateDirectory(v_strSearchResultRootPath & v_strCurrDate)
            End If

            v_strSearchResultPath &= "\Ket_Qua_Tim_Kiem_" & v_strBRID & "_" & v_strTLName & "_" & Replace(Replace(Now.ToShortTimeString, ":", "_"), " ", "_") & ".xls"

            ' date : 14/10/2008
            ' Purpose : get from  date T-3 to date T
            Dim v_strT_T_3 As String
            v_strSQL = "select to_char(GET_T_PLUS(to_date('" & v_strCurrDate & "', 'dd/mm/yyyy'),'" & v_strBRID & "',-1),'dd/mm/yyyy') txdate from dual" _
                    & " union select to_char(GET_T_PLUS(to_date('" & v_strCurrDate & "', 'dd/mm/yyyy'),'" & v_strBRID & "',-2),'dd/mm/yyyy') txdate from dual" _
                    & " union select to_char(GET_T_PLUS(to_date('" & v_strCurrDate & "', 'dd/mm/yyyy'),'" & v_strBRID & "',-3),'dd/mm/yyyy') txdate from dual"
            v_trace = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            v_strT_T_3 = "to_date('" & v_strCurrDate & "', 'dd/mm/yyyy'),"
            For i = 0 To v_trace.Tables(0).Rows.Count - 1
                v_strT_T_3 = v_strT_T_3 & "to_date('" & v_trace.Tables(0).Rows(i)("txdate") & "', 'dd/mm/yyyy'),"
            Next
            v_strT_T_3 = "(" & Left(v_strT_T_3, v_strT_T_3.Length - 1) & ")"
            ' end 

            If v_trace_status = "1" Then
                v_obj.GetSysVar("SYSTEM", "TRACE_PATH", v_trace_path)
                v_strCurrDate = Replace(v_strCurrDate, "/", "_")
                If v_trace_path = "" Then
                    Dim v_app As New ApplicationServices.ApplicationBase
                    v_trace_path = v_app.Info.DirectoryPath & "\Log\" & v_strCurrDate
                Else
                    v_trace_path = v_trace_path & v_strCurrDate
                End If

                If Not System.IO.Directory.Exists(v_trace_path) Then
                    System.IO.Directory.CreateDirectory(v_trace_path)
                End If

                v_trace_path &= "\log_search_br" & v_strBRID & "_" & v_strTLName & ".txt"

                tr2 = New TextWriterTraceListener(System.IO.File.AppendText(v_trace_path))
                Trace.Listeners.Add(tr2)
                Trace.WriteLine("[Bắt đầu: SI - " & v_strObjName & "] " & DateTime.Now & vbCrLf)
            End If

            'Xu ly DL bang TRAN, TLLOG
            v_strSQL = "SELECT TLLOGTRAN FROM SISEARCH WHERE SEARCHCODE = '" & v_strObjName & "' and deleted=0"
            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

            'Write Log
            'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "CoreBusiness", "objMaster", "CoreHostInquery", "", "", mv_strTellerId, mv_strTellerName, "Excute SQL Command: " & v_strSQL)

            Dim v_strTran As String = "" 'IA|MA|CA|MF|RA|RG|SF|TLLOG
            Dim v_lstTran() As String
            Dim v_blnLoadAll As Boolean = False

            If v_ds.Tables(0).Rows.Count > 0 Then
                v_strTran = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("TLLOGTRAN")).ToUpper
            End If

            v_strMinDate = Replace(v_strCurrDate, "_", "/")
            v_strMaxDate = Replace(v_strCurrDate, "_", "/")
            If v_strPartition <> "" Then
                v_strMinDate = v_strPartition.Split("|")(0)
                v_strMaxDate = v_strPartition.Split("|")(1)
            End If
            v_blnSearch = CheckPassDate(v_strMaxDate, Replace(v_strCurrDate, "_", "/"))
            v_blnLoadAll = CheckPassDate(v_strMinDate, Replace(v_strCurrDate, "_", "/"))


            Dim v_strTllogWhere As String = ""
            If v_blnLoadAll Then
                v_strTllogWhere = " AND [#TXDATE [!O_TXDATE] TO_DATE('[!V_TXDATE]','" & gc_FORMAT_DATE & "')#] AND [#TXNUM [!O_TXNUM] '[!V_TXNUM]'#]"
                v_strTllogWhere &= " AND [#BUSDATE [!O_TXDATE] TO_DATE('[!V_BUSDATE]','" & gc_FORMAT_DATE & "')#] AND [#TLTXCD [!O_TLTXCD] '[!V_TLTXCD]'#]"
            End If

            If v_strTran <> "" Then
                v_lstTran = v_strTran.Split("|")
                v_strTran = ""
                For i As Integer = 0 To v_lstTran.Length - 1
                    Select Case v_lstTran(i)
                        Case "TLLOG"
                            v_strTran &= ",'TLLOG'"

                            If v_blnLoadAll And v_strLocal = gc_IsInQueryNotLocalMsg Then
                                v_strTran &= ",'TLLOGALL'"
                            End If
                        Case "IA", "MA", "RA", "SF"
                            v_strTran &= ",'" & v_lstTran(i) & "TRAN'"
                            v_strTran &= ",'" & v_lstTran(i) & "MAST'"

                            If v_blnLoadAll Then
                                v_strTran &= ",'" & v_lstTran(i) & "TRANA'"
                            End If
                        Case "CA"
                            v_strTran &= ",'" & v_lstTran(i) & "TRAN'"
                            If v_strPartition <> "" Then
                                v_strTran &= ",'" & v_lstTran(i) & "TRANA'"
                                v_blnLoadAll = True
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
            Dim v_intCount As Integer = -1

            If v_strCondition <> "" Then
                v_arrField = v_strCondition.Split("#")
                v_intCount = v_arrField.Length - 2
            End If
            Dim v_hCondition As New Hashtable
            Dim v_strhKey, v_strhValue As String
            Dim v_blnReplace As Boolean

            If v_intCount >= 0 Then
                For j = 0 To v_intCount
                    v_strhKey = v_arrField(j).Split("|")(0).ToUpper
                    Select Case v_strhKey
                        Case "SICODE"
                            v_lstSICODE = "('" & v_arrField(j).Split("|")(2) & "')"
                        Case "MICODE"
                            v_lstMICODE = "('" & v_arrField(j).Split("|")(2) & "')"
                    End Select
                Next
            End If
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
            'Write Log
            'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "CoreBusiness", "objMaster", "CoreHostInquery", "", "", mv_strTellerId, mv_strTellerName, "Excute SQL Command: " & v_strSQL)

            v_lstBrID = ""
            For i = 0 To v_ds.Tables(0).Rows.Count - 1
                v_lstBrID &= ",'" & v_ds.Tables(0).Rows(i)(0) & "'"
            Next
            v_lstBrID = "(" & Mid(v_lstBrID, 2) & ")"

            'Lay TVLK dc phan quyen cho user vao bang TMP_RGMI
            v_strSQL = "INSERT INTO tmp_rgmi " _
                       & " SELECT DISTINCT m.* FROM rgmi m WHERE m.deleted=0 AND m.status=0" _
                       & " AND m.MICODE IN " & v_lstMICODE

            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
            'Write Log
            'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "CoreBusiness", "objMaster", "CoreHostInquery", "", "", mv_strTellerId, mv_strTellerName, "Excute SQL Command: " & v_strSQL)

            'v_strSQL = "INSERT INTO TMP_RGMI(AUTOID, MICODE) SELECT '0' AUTOID, '000' MICODE FROM DUAL"
            'v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

            'Lay chung khoan dc phan quyen cho user vao bang TMP_RGSI
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
            '    v_strSQL = "INSERT INTO tmp_rgsi(iscode, sicode, stock_name, part_value, TYPE," _
            '    & "stock_type, interest_rate, interest_period, " _
            '    & "bond_period, deleted, exchange_rate, note, " _
            '    & "bond_term, release_series, release_mode, isin, " _
            '    & "rarate, tmpid, paidmethod, npaiddate1, npaiddate2, " _
            '    & "npaiddate3, npaiddate4, int_release_mode, brid, status, public_date, currency_code,vsdbrid) " _
            '    & " SELECT distinct m.iscode, m.sicode, m.stock_name, m.part_value, m.TYPE," _
            '    & "m.stock_type, m.interest_rate, m.interest_period," _
            '    & "m.bond_period, m.deleted, m.exchange_rate, m.note, " _
            '    & "m.bond_term, m.release_series, m.release_mode, m.isin, " _
            '    & "m.rarate, m.tmpid, m.paidmethod, m.npaiddate1, m.npaiddate2," _
            '    & "m.npaiddate3, m.npaiddate4, m.int_release_mode, m.brid, m.status, m.public_date, m.currency_code,m.vsdbrid FROM rgsi m" _
            '    & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_strCut1
            '    v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
            '    'Write Log
            '    'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "CoreBusiness", "objMaster", "CoreHostInquery", "", "", mv_strTellerId, mv_strTellerName, "Excute SQL Command: " & v_strSQL)

            '    v_strCut = "(" & v_lstSICODE.Substring(v_iCut)
            '    v_iCut = InStr(v_iCut, v_strCut, ",")
            '    If v_iCut > 0 Then
            '        v_strCut2 = v_strCut.Substring(0, v_iCut - 1) & ")"
            '        v_strSQL = "INSERT INTO tmp_rgsi(iscode, sicode, stock_name, part_value, TYPE," _
            '              & "stock_type, interest_rate, interest_period, " _
            '              & "bond_period, deleted, exchange_rate, note, " _
            '              & "bond_term, release_series, release_mode, isin, " _
            '              & "rarate, tmpid, paidmethod, npaiddate1, npaiddate2, " _
            '              & "npaiddate3, npaiddate4, int_release_mode, brid, status, public_date, currency_code,vsdbrid) " _
            '              & " SELECT distinct m.iscode, m.sicode, m.stock_name, m.part_value, m.TYPE," _
            '              & "m.stock_type, m.interest_rate, m.interest_period," _
            '              & "m.bond_period, m.deleted, m.exchange_rate, m.note, " _
            '              & "m.bond_term, m.release_series, m.release_mode, m.isin, " _
            '              & "m.rarate, m.tmpid, m.paidmethod, m.npaiddate1, m.npaiddate2," _
            '              & "m.npaiddate3, m.npaiddate4, m.int_release_mode, m.brid, m.status, m.public_date, m.currency_code,m.vsdbrid FROM rgsi m" _
            '              & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_strCut2
            '        v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

            '        v_strCut = "(" & v_strCut.Substring(v_iCut)
            '        v_iCut = InStr(v_iCut, v_strCut, ",")
            '        '3
            '        If v_iCut > 0 Then
            '            v_strCut3 = v_strCut.Substring(0, v_iCut - 1) & ")"
            '            v_strSQL = "INSERT INTO tmp_rgsi(iscode, sicode, stock_name, part_value, TYPE," _
            '                    & "stock_type, interest_rate, interest_period, " _
            '                    & "bond_period, deleted, exchange_rate, note, " _
            '                    & "bond_term, release_series, release_mode, isin, " _
            '                    & "rarate, tmpid, paidmethod, npaiddate1, npaiddate2, " _
            '                    & "npaiddate3, npaiddate4, int_release_mode, brid, status, public_date, currency_code,vsdbrid) " _
            '                    & " SELECT distinct m.iscode, m.sicode, m.stock_name, m.part_value, m.TYPE," _
            '                    & "m.stock_type, m.interest_rate, m.interest_period," _
            '                    & "m.bond_period, m.deleted, m.exchange_rate, m.note, " _
            '                    & "m.bond_term, m.release_series, m.release_mode, m.isin, " _
            '                    & "m.rarate, m.tmpid, m.paidmethod, m.npaiddate1, m.npaiddate2," _
            '                    & "m.npaiddate3, m.npaiddate4, m.int_release_mode, m.brid, m.status, m.public_date, m.currency_code,m.vsdbrid FROM rgsi m" _
            '                    & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_strCut3
            '            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

            '            v_strCut = "(" & v_strCut.Substring(v_iCut)
            '            v_iCut = InStr(v_iCut, v_strCut, ",")
            '            If v_iCut > 0 Then
            '                v_strCut4 = v_strCut.Substring(0, v_iCut - 1) & ")"
            '                v_strSQL = "INSERT INTO tmp_rgsi(iscode, sicode, stock_name, part_value, TYPE," _
            '                        & "stock_type, interest_rate, interest_period, " _
            '                        & "bond_period, deleted, exchange_rate, note, " _
            '                        & "bond_term, release_series, release_mode, isin, " _
            '                        & "rarate, tmpid, paidmethod, npaiddate1, npaiddate2, " _
            '                        & "npaiddate3, npaiddate4, int_release_mode, brid, status, public_date, currency_code,vsdbrid) " _
            '                        & " SELECT distinct m.iscode, m.sicode, m.stock_name, m.part_value, m.TYPE," _
            '                        & "m.stock_type, m.interest_rate, m.interest_period," _
            '                        & "m.bond_period, m.deleted, m.exchange_rate, m.note, " _
            '                        & "m.bond_term, m.release_series, m.release_mode, m.isin, " _
            '                        & "m.rarate, m.tmpid, m.paidmethod, m.npaiddate1, m.npaiddate2," _
            '                        & "m.npaiddate3, m.npaiddate4, m.int_release_mode, m.brid, m.status, m.public_date, m.currency_code,m.vsdbrid FROM rgsi m" _
            '                        & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_strCut4
            '                v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

            '                v_strCut5 = "(" & v_strCut.Substring(v_iCut)

            '                v_strSQL = "INSERT INTO tmp_rgsi(iscode, sicode, stock_name, part_value, TYPE," _
            '                        & "stock_type, interest_rate, interest_period, " _
            '                        & "bond_period, deleted, exchange_rate, note, " _
            '                        & "bond_term, release_series, release_mode, isin, " _
            '                        & "rarate, tmpid, paidmethod, npaiddate1, npaiddate2, " _
            '                        & "npaiddate3, npaiddate4, int_release_mode, brid, status, public_date, currency_code,vsdbrid) " _
            '                        & " SELECT distinct m.iscode, m.sicode, m.stock_name, m.part_value, m.TYPE," _
            '                        & "m.stock_type, m.interest_rate, m.interest_period," _
            '                        & "m.bond_period, m.deleted, m.exchange_rate, m.note, " _
            '                        & "m.bond_term, m.release_series, m.release_mode, m.isin, " _
            '                        & "m.rarate, m.tmpid, m.paidmethod, m.npaiddate1, m.npaiddate2," _
            '                        & "m.npaiddate3, m.npaiddate4, m.int_release_mode, m.brid, m.status, m.public_date, m.currency_code,m.vsdbrid FROM rgsi m" _
            '                        & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_strCut5
            '                v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

            '            Else
            '                v_strSQL = "INSERT INTO tmp_rgsi(iscode, sicode, stock_name, part_value, TYPE," _
            '                          & "stock_type, interest_rate, interest_period, " _
            '                          & "bond_period, deleted, exchange_rate, note, " _
            '                          & "bond_term, release_series, release_mode, isin, " _
            '                          & "rarate, tmpid, paidmethod, npaiddate1, npaiddate2, " _
            '                          & "npaiddate3, npaiddate4, int_release_mode, brid, status, public_date, currency_code,vsdbrid) " _
            '                          & " SELECT distinct m.iscode, m.sicode, m.stock_name, m.part_value, m.TYPE," _
            '                          & "m.stock_type, m.interest_rate, m.interest_period," _
            '                          & "m.bond_period, m.deleted, m.exchange_rate, m.note, " _
            '                          & "m.bond_term, m.release_series, m.release_mode, m.isin, " _
            '                          & "m.rarate, m.tmpid, m.paidmethod, m.npaiddate1, m.npaiddate2," _
            '                          & "m.npaiddate3, m.npaiddate4, m.int_release_mode, m.brid, m.status, m.public_date, m.currency_code,m.vsdbrid FROM rgsi m" _
            '                          & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_strCut
            '                v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
            '            End If
            '        End If
            '    End If
            'Else
            '    v_strSQL = "INSERT INTO tmp_rgsi(iscode, sicode, stock_name, part_value, TYPE," _
            '                  & "stock_type, interest_rate, interest_period, " _
            '                  & "bond_period, deleted, exchange_rate, note, " _
            '                  & "bond_term, release_series, release_mode, isin, " _
            '                  & "rarate, tmpid, paidmethod, npaiddate1, npaiddate2, " _
            '                  & "npaiddate3, npaiddate4, int_release_mode, brid, status, public_date, currency_code,vsdbrid) " _
            '                  & " SELECT distinct m.iscode, m.sicode, m.stock_name, m.part_value, m.TYPE," _
            '                  & "m.stock_type, m.interest_rate, m.interest_period," _
            '                  & "m.bond_period, m.deleted, m.exchange_rate, m.note, " _
            '                  & "m.bond_term, m.release_series, m.release_mode, m.isin, " _
            '                  & "m.rarate, m.tmpid, m.paidmethod, m.npaiddate1, m.npaiddate2," _
            '                  & "m.npaiddate3, m.npaiddate4, m.int_release_mode, m.brid, m.status, m.public_date, m.currency_code,m.vsdbrid FROM rgsi m" _
            '                  & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_lstSICODE
            '    v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
            '    'Write Log
            '    'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "CoreBusiness", "objMaster", "CoreHostInquery", "", "", mv_strTellerId, mv_strTellerName, "Excute SQL Command: " & v_strSQL)

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
            'Write Log
            'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "CoreBusiness", "objMaster", "CoreHostInquery", "", "", mv_strTellerId, mv_strTellerName, "Excute SQL Command: " & v_strSQL)


            'Lay cau xu ly chung
            Dim v_hSQL As New Hashtable
            Dim v_hORD As New Hashtable
            If v_strTran <> "" Then
                v_strSQL = "SELECT * FROM SISTORES WHERE deleted =0 and SEARCHCODE IS NULL AND OVERWRITE IN " & v_strTran & " ORDER BY ODRNUM"

                v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                'Write Log
                'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "CoreBusiness", "objMaster", "CoreHostInquery", "", "", mv_strTellerId, mv_strTellerName, "Excute SQL Command: " & v_strSQL)

                Dim v_strOverWrite As String
                If v_ds.Tables(0).Rows.Count > 0 Then
                    For i As Integer = 0 To v_ds.Tables(0).Rows.Count - 1
                        v_strOverWrite = v_ds.Tables(0).Rows(i)("OVERWRITE")
                        v_hORD(i) = v_strOverWrite
                        v_hSQL(v_strOverWrite) = v_ds.Tables(0).Rows(i)("TRACE") & v_ds.Tables(0).Rows(i)("CMDSQL") & " " & v_ds.Tables(0).Rows(i)("CMDSQL1")
                    Next
                End If

                'Lay cau xu ly chung da dc viet lai
                v_strSQL = "SELECT * FROM SISTORES WHERE deleted=0 and SEARCHCODE='" & v_strObjName & "' AND OVERWRITE IN " & v_strTran & " ORDER BY ODRNUM"
                v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                'Write Log
                'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "CoreBusiness", "objMaster", "CoreHostInquery", "", "", mv_strTellerId, mv_strTellerName, "Excute SQL Command: " & v_strSQL)

                If v_ds.Tables(0).Rows.Count > 0 Then
                    For i As Integer = 0 To v_ds.Tables(0).Rows.Count - 1
                        v_strOverWrite = v_ds.Tables(0).Rows(i)("OVERWRITE")
                        If Not v_hSQL(v_strOverWrite) Is Nothing Then
                            v_hSQL(v_strOverWrite) = v_ds.Tables(0).Rows(i)("TRACE") & v_ds.Tables(0).Rows(i)("CMDSQL") & " " & v_ds.Tables(0).Rows(i)("CMDSQL1")
                        End If
                    Next
                End If

                'Thuc hien cau xu ly chung
                Dim v_strPartitionSQL As String
                'Dim v_lstPartition As String
                For i As Integer = 0 To v_hORD.Count - 1
                    v_strOverWrite = v_hORD(i)
                    v_strSQL = Mid(v_hSQL(v_strOverWrite), 2)
                    'v_strPartitionSQL = ""
                    v_strSQL = Replace(v_strSQL, "[!BRID]", v_strBRID)
                    v_strSQL = Replace(v_strSQL, "[!VSD_BRID]", v_strVsdBrid)
                    v_strSQL = Replace(v_strSQL, "[!AUTOID]", v_strAutoID)
                    v_strSQL = Replace(v_strSQL, "[!TLID]", v_strTellerID)
                    v_strSQL = Replace(v_strSQL, "[!LBRID]", v_lstBrID)
                    v_strSQL = Replace(v_strSQL, "[!LMICODE]", v_lstMICODE)
                    v_strSQL = Replace(v_strSQL, "[!T_T_3]", v_strT_T_3)
                    v_strSQL = Replace(v_strSQL, "[!MINDATE]", "TO_DATE('" & v_strMinDate & "','dd/mm/yyyy')")
                    v_strSQL = Replace(v_strSQL, "[!MAXDATE]", "TO_DATE('" & v_strMaxDate & "','dd/mm/yyyy')")

                    v_strSQL = Replace(v_strSQL, "TT_", "")
                    v_strSQL = Replace(v_strSQL, "tt_", "")
                    v_strSQL = Replace(v_strSQL, "tT_", "")
                    v_strSQL = Replace(v_strSQL, "Tt_", "")

                    v_strSQL = Replace(v_strSQL, "[!TLLOGALL]", "TLLOGALL_ALL")
                    v_strSQL = Replace(v_strSQL, "[!CATRANA]", "CATRANA_ALL")
                    v_strSQL = Replace(v_strSQL, "[!IATRANA]", "IATRANA_ALL")
                    v_strSQL = Replace(v_strSQL, "[!MATRANA]", "MATRANA_ALL")
                    v_strSQL = Replace(v_strSQL, "[!SFTRANA]", "SFTRANA_ALL")
                    v_strSQL = Replace(v_strSQL, "[!MFTRANA]", "MFTRANA_ALL")
                    v_strSQL = Replace(v_strSQL, "[!RATRANA]", "RATRANA_ALL")
                    v_strSQL = Replace(v_strSQL, "[!RGTRANA]", "RGTRANA_ALL")

                    If Right(v_strOverWrite, 3) = "ALL" Or Right(v_strOverWrite, 1) = "A" Then
                        'If Mid(v_strOverWrite, 1, 5).ToUpper = "TLLOG" Then
                        '    v_lstPartition = GetListPartition(v_strPartition)
                        'Else
                        '    v_lstPartition = GetListPartition(v_strMinDate & "|" & Replace(v_strCurrDate, "_", "/"))
                        'End If

                        'For j As Integer = 0 To v_lstPartition.Split("|").Length - 2
                        '    If v_strPartitionSQL <> "" Then
                        '        v_strPartitionSQL &= " UNION ALL "
                        '    End If

                        '    v_strPartitionSQL &= v_strSQL.Substring(InStr(v_strSQL.ToUpper, "SELECT") - 1)
                        'v_strPartitionSQL = Replace(v_strPartitionSQL, "[!TLLOGALL]", "TLLOGALL_ALL PARTITION(" & v_lstPartition.Split("|")(j) & ")")
                        'v_strPartitionSQL = Replace(v_strPartitionSQL, "[!CATRANA]", "CATRANA_ALL PARTITION(" & v_lstPartition.Split("|")(j) & ")")
                        'v_strPartitionSQL = Replace(v_strPartitionSQL, "[!IATRANA]", "IATRANA_ALL PARTITION(" & v_lstPartition.Split("|")(j) & ")")
                        'v_strPartitionSQL = Replace(v_strPartitionSQL, "[!MATRANA]", "MATRANA_ALL PARTITION(" & v_lstPartition.Split("|")(j) & ")")
                        'v_strPartitionSQL = Replace(v_strPartitionSQL, "[!SFTRANA]", "SFTRANA_ALL PARTITION(" & v_lstPartition.Split("|")(j) & ")")
                        'v_strPartitionSQL = Replace(v_strPartitionSQL, "[!MFTRANA]", "MFTRANA_ALL PARTITION(" & v_lstPartition.Split("|")(j) & ")")
                        'v_strPartitionSQL = Replace(v_strPartitionSQL, "[!RATRANA]", "RATRANA_ALL PARTITION(" & v_lstPartition.Split("|")(j) & ")")
                        'v_strPartitionSQL = Replace(v_strPartitionSQL, "[!RGTRANA]", "RGTRANA_ALL PARTITION(" & v_lstPartition.Split("|")(j) & ")")
                        'Next
                        If v_strOverWrite = "TLLOGALL" Then
                            v_strPartitionSQL = v_strSQL.Substring(InStr(v_strSQL.ToUpper, "SELECT") - 1)
                            v_strSQL = Mid(v_strSQL, 1, InStr(v_strSQL.ToUpper, "SELECT") - 1)
                            v_strPartitionSQL = " SELECT * FROM (" & v_strPartitionSQL & ") WHERE 1=1" & v_strTllogWhere
                            v_strSQL &= v_strPartitionSQL
                        End If

                        If v_intCount >= 0 Then
                            For j = 0 To v_intCount
                                v_strSQL = Replace(v_strSQL, "[!O_" & v_arrField(j).Split("|")(0) & "]", v_arrField(j).Split("|")(1))
                                v_strSQL = Replace(v_strSQL, "[!V_" & v_arrField(j).Split("|")(0) & "]", v_arrField(j).Split("|")(2))
                            Next
                        End If
                    End If

                    v_strSQL = Replace(v_strSQL, "[!V_TXDATE]", Replace(v_strCurrDate, "_", "/"))
                    v_strSQL = DecodeSQL(v_strSQL)

                    If v_trace_status = "1" And Mid(v_hSQL(v_strOverWrite).ToString, 1, 1) = "1" Then
                        Trace.WriteLine("[SI - " & v_strObjName & "] " & DateTime.Now & " :" & vbCrLf & " -o- Câu lệnh thứ xử lý chung thứ #" & i + 1 & "-o-" & vbCrLf & v_strSQL & vbCrLf)
                        If InStr(v_strSQL.ToUpper, "SELECT") > 0 Then
                            Trace.WriteLine("-o- Dữ liệu câu lệnh xử lý chung thứ #" & i + 1 & "-o-")
                            v_strSQLTmp = v_strSQL.Substring(InStr(v_strSQL.ToUpper, "SELECT") - 1)
                            v_trace = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                            Trace.WriteLine(v_trace.GetXml & vbCrLf)
                        End If
                    End If
                    v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                    'Write Log
                    'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "CoreBusiness", "objMaster", "CoreHostInquery", "", "", mv_strTellerId, mv_strTellerName, "Excute SQL Command: " & v_strSQL)

                Next
            End If

            v_strSQL = "SELECT * FROM SISTORES WHERE SEARCHCODE='" & v_strObjName & "' AND OVERWRITE IS NULL AND DELETED=0 AND STATUS=0 ORDER BY ODRNUM"
            v_ds1 = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            'Write Log
            'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "CoreBusiness", "objMaster", "CoreHostInquery", "", "", mv_strTellerId, mv_strTellerName, "Excute SQL Command: " & v_strSQL)

            'Su ly cau lay SQL trong SISTORES
            For i = 0 To v_ds1.Tables(0).Rows.Count - 1
                v_strSQL = v_ds1.Tables(0).Rows(i)("CMDSQL").ToString.ToUpper & " " & v_ds1.Tables(0).Rows(i)("CMDSQL1").ToString.ToUpper
                v_blnReplace = False

                If v_intCount >= 0 Then
                    For j = 0 To v_intCount
                        v_strhKey = v_arrField(j).Split("|")(0)
                        v_strhValue = "[!V_" & v_arrField(j).Split("|")(0) & "]"
                        If InStr(v_strSQL, v_strhValue, CompareMethod.Text) > 0 Then
                            v_strSQL = Replace(v_strSQL, "[!O_" & v_arrField(j).Split("|")(0) & "]", v_arrField(j).Split("|")(1))
                            v_strSQL = Replace(v_strSQL, "[!V_" & v_arrField(j).Split("|")(0) & "]", v_arrField(j).Split("|")(2))

                            If v_hCondition(v_strhKey) Is Nothing Then
                                v_hCondition.Add(v_strhKey, v_strhValue)
                            End If
                            v_blnReplace = True
                        End If
                    Next
                End If

                If (Not v_blnReplace And v_ds1.Tables(0).Rows(i)("SITYPE") = 0) Or v_blnReplace Then
                    v_strSQL = DecodeSQL(v_strSQL)
                    'Neu khong su dung bang TMP thi thong bao loi
                    'If Not CheckTranTable(v_strSQL) Then
                    '    v_lngError = 1
                    '    Exit For
                    'End If

                    v_strSQL = Replace(v_strSQL, "[!BRID]", v_strBRID)
                    v_strSQL = Replace(v_strSQL, "[!VSD_BRID]", v_strVsdBrid)
                    v_strSQL = Replace(v_strSQL, "[!AUTOID]", v_strAutoID)
                    v_strSQL = Replace(v_strSQL, "[!TLID]", v_strTellerID)
                    v_strSQL = Replace(v_strSQL, "[!LBRID]", v_lstBrID)
                    v_strSQL = Replace(v_strSQL, "[!LMICODE]", v_lstMICODE)
                    v_strSQL = Replace(v_strSQL, "[!T_T_3]", v_strT_T_3)
                    v_strSQL = Replace(v_strSQL, "[!MINDATE]", "TO_DATE('" & v_strMinDate & "','dd/mm/yyyy')")
                    v_strSQL = Replace(v_strSQL, "[!MAXDATE]", "TO_DATE('" & v_strMaxDate & "','dd/mm/yyyy')")

                    v_strSQL = Replace(v_strSQL, "TT_", "")
                    v_strSQL = Replace(v_strSQL, "tt_", "")
                    v_strSQL = Replace(v_strSQL, "tT_", "")
                    v_strSQL = Replace(v_strSQL, "Tt_", "")

                    If v_trace_status = "1" And v_ds1.Tables(0).Rows(i)("TRACE") = 1 Then
                        Trace.WriteLine("[SI - " & v_strObjName & "] " & DateTime.Now & " :" & vbCrLf & " -o- Câu lệnh thứ #" & v_ds1.Tables(0).Rows(i)("ODRNUM") & "-o-" & vbCrLf & v_strSQL & vbCrLf)
                        If InStr(v_strSQL.ToUpper, "SELECT") > 0 Then
                            Trace.WriteLine("-o- Dữ liệu câu lệnh thứ #" & v_ds1.Tables(0).Rows(i)("ODRNUM") & "-o-")
                            v_strSQLTmp = v_strSQL.Substring(InStr(v_strSQL.ToUpper, "SELECT") - 1)
                            v_trace = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                            Trace.WriteLine(v_trace.GetXml & vbCrLf)
                        End If
                    End If

                    v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                    'Write Log
                    'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "CoreBusiness", "objMaster", "CoreHostInquery", "", "", mv_strTellerId, mv_strTellerName, "Excute SQL Command: " & v_strSQL)

                End If
            Next

            If v_lngError = ERR_SYSTEM_OK Then
                v_strSQL = v_strCmdInquiry

                'If Not CheckTranTable(v_strSQL) Then
                '    v_lngError = 1
                'Else
                If v_intCount >= 0 Then
                    For i = 0 To v_intCount
                        v_strhKey = v_arrField(i).Split("|")(0)
                        v_strhValue = "[!V_" & v_arrField(i).Split("|")(0) & "]"
                        If InStr(v_strSQL, v_strhValue, CompareMethod.Text) > 0 Then
                            v_strSQL = Replace(v_strSQL, "[!O_" & v_arrField(i).Split("|")(0) & "]", v_arrField(i).Split("|")(1))
                            v_strSQL = Replace(v_strSQL, "[!V_" & v_arrField(i).Split("|")(0) & "]", v_arrField(i).Split("|")(2))

                            If v_hCondition(v_strhKey) Is Nothing Then
                                v_hCondition.Add(v_strhKey, v_strhValue)
                            End If
                        End If
                    Next
                End If

                v_strSQL = DecodeSQL(v_strSQL)
                v_strSQL = Replace(v_strSQL, "TT_", "")
                v_strSQL = Replace(v_strSQL, "tt_", "")
                v_strSQL = Replace(v_strSQL, "tT_", "")
                v_strSQL = Replace(v_strSQL, "Tt_", "")
                Dim v_lngRowCount As Long

                Dim v_strWhere As String = ""
                'purpose : filter_group_x
                Dim v_strFILTERGROUP(10) As String
                Dim v_arrGroupID() As String
                For i = 1 To 9
                    v_strFILTERGROUP(i) = ""
                Next
                'end filter_group_x

                Dim v_strTmp, v_strTmpField, v_strTmpField1, v_strValue As String
                If v_strCondition <> "" Then
                    'purpose : filter_group_x
                    v_strSQLTmp = "select FIELDCODE,groupid from sisearchfld where not groupid is null and deleted=0 and SEARCHCODE='" & v_strObjName & "'"
                    v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                    'end filter_group_x

                    v_arrField = v_strCondition.Split("#")
                    v_intCount = v_arrField.Length - 2

                    For i = 0 To v_arrField.Length - 2
                        If v_hCondition(v_arrField(i).Split("|")(0)) Is Nothing Then
                            'v_strWhere &= " AND " & v_arrField(i).Split("|")(0) & " " & v_arrField(i).Split("|")(1) & " "
                            v_strValue = v_arrField(i).Split("|")(2)
                            Select Case v_arrField(i).Split("|")(3)
                                Case "D", "P"
                                    v_strTmpField = "TO_DATE(" & v_arrField(i).Split("|")(0) & ",'" & gc_FORMAT_DATE & "') " & v_arrField(i).Split("|")(1) & " "
                                    v_strTmpField1 = v_arrField(i).Split("|")(0) & " " & v_arrField(i).Split("|")(1) & " "
                                    v_strTmp = "TO_DATE('" & v_strValue & "', '" & gc_FORMAT_DATE & "')"
                                Case "N"
                                    v_strTmpField = v_arrField(i).Split("|")(0) & " " & v_arrField(i).Split("|")(1) & " "
                                    v_strTmpField1 = v_strTmpField
                                    If IsNumeric(v_strValue) Then
                                        v_strTmp = CDbl(v_strValue)
                                    Else
                                        v_strTmp = 0
                                    End If
                                Case Else
                                    v_strTmpField = "UPPER(" & v_arrField(i).Split("|")(0) & ") " & v_arrField(i).Split("|")(1) & " "
                                    v_strTmpField1 = v_strTmpField
                                    'bangpv: rao lai, khong hieu y nghia doan nay
                                    ' v_strValue = Trim(Replace(v_strValue, ".", String.Empty))
                                    'end bangpv 28/08/2012
                                    If v_arrField(i).Split("|")(1) = "LIKE" Then
                                        v_strTmp = "UPPER(N'%" & v_strValue & "%')"
                                    Else
                                        v_strTmp = "UPPER(N'" & v_strValue & "')"
                                    End If
                            End Select
                            'purpose : filter_group_x
                            If v_ds.Tables(0).Rows.Count > 0 Then
                                For j = 0 To v_ds.Tables(0).Rows.Count - 1
                                    If v_arrField(i).Split("|")(0) = v_ds.Tables(0).Rows(j)("FIELDCODE") Then
                                        v_arrGroupID = v_ds.Tables(0).Rows(j)("groupid").ToString.Split("|")
                                        For t = 0 To v_arrGroupID.Length - 1
                                            v_strFILTERGROUP(CInt(v_arrGroupID(t).Substring(1, 1))) &= "AND " _
                                                & v_strTmpField1.Replace("UPPER(", "").Replace(")", "").Replace(v_ds.Tables(0).Rows(j)("FIELDCODE"), _
                                                                        v_arrGroupID(t).Substring(0, 1) _
                                                                        & "." _
                                                                        & v_ds.Tables(0).Rows(j)("FIELDCODE")) _
                                                & v_strTmp
                                        Next
                                    End If
                                Next
                            End If
                            'end filter_group_x
                            v_strWhere &= " AND " & v_strTmpField & v_strTmp
                        End If
                    Next
                End If
                'purpose : filter_group_x
                Dim v_strForcedGroupArr() As String

                v_strSQLTmp = "select forced_group from sisearch where deleted=0 and SEARCHCODE='" & v_strObjName & "'"
                v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                If Not IsDBNull(v_ds.Tables(0).Rows(0)("forced_group")) Then
                    v_strForcedGroupArr = v_ds.Tables(0).Rows(0)("forced_group").ToString.Split("|")
                End If

                For i = 1 To 9
                    If Not IsDBNull(v_ds.Tables(0).Rows(0)("forced_group")) Then
                        For j = 0 To v_strForcedGroupArr.Length - 1
                            If v_strForcedGroupArr(j) = i Then
                                v_strFILTERGROUP(i) = IIf(v_strFILTERGROUP(i).Trim = "", " and 1=0 ", v_strFILTERGROUP(i))
                            End If
                        Next
                    End If
                    v_strSQL = Replace(v_strSQL, "?FITLER_GROUP_" & i, v_strFILTERGROUP(i))
                Next
                'end filter_group_x
                ' purpose : phi luu ky
                If InStr(v_strSQL, "[!MFNO_01]", CompareMethod.Text) Then
                    v_strSQLTmp = "select fomula from mftype where deleted=0 and status=0 and brid='" & v_strBRID & "' and mfno='01'"
                    v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                    If v_ds.Tables(0).Rows.Count > 0 Then
                        v_strSQL = Replace(v_strSQL, "[!MFNO_01]", v_ds.Tables(0).Rows(0)(0))
                    Else
                        v_strSQL = Replace(v_strSQL, "[!MFNO_01]", "0")
                    End If
                End If
                ' end phi luu ky
                If Trim(v_strWhere) <> "" Then
                    v_strSQL = "SELECT * FROM (" & v_strSQL & ") WHERE 1=1 " & v_strWhere
                End If
                v_strSQL = Replace(v_strSQL, "[!V_TXDATE]", Replace(v_strCurrDate, "_", "/"))
                v_strSQL = Replace(v_strSQL, "[!MINDATE]", "TO_DATE('" & v_strMinDate & "','dd/mm/yyyy')")
                v_strSQL = Replace(v_strSQL, "[!MAXDATE]", "TO_DATE('" & v_strMaxDate & "','dd/mm/yyyy')")
                v_strSQL = Replace(v_strSQL, "[!BRID]", v_strBRID)
                v_strSQL = Replace(v_strSQL, "[!T_T_3]", v_strT_T_3)
                v_strSQL = Replace(v_strSQL, "[!LMICODE]", v_lstMICODE)
                v_strSQL = Replace(v_strSQL, "[!LBRID]", v_lstBrID)

                If v_strSQL <> "" Then
                    'Tinh so ban ghi
                    v_strSQLTmp = "SELECT COUNT(1) FROM (" & v_strSQL & ")"
                    v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                    'Write Log
                    'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "CoreBusiness", "objMaster", "CoreHostInquery", "", "", mv_strTellerId, mv_strTellerName, "Excute SQL Command: " & v_strSQL)

                    v_lngRowCount = v_ds.Tables(0).Rows(0)(0)

                    If Not (v_attrColl.GetNamedItem(gc_AtributeFUNCNAME) Is Nothing) Then
                        pv_xmlDocument.DocumentElement.Attributes(gc_AtributeFUNCNAME).InnerXml = v_lngRowCount
                    Else
                        Dim v_attr As Xml.XmlAttribute
                        v_attr = pv_xmlDocument.CreateAttribute(gc_AtributeFUNCNAME)
                        v_attr.Value = v_lngRowCount
                        Dim dataElement As Xml.XmlElement
                        dataElement = pv_xmlDocument.DocumentElement
                        dataElement.Attributes.Append(v_attr)
                    End If

                    'Lay DL loc theo dong
                    Dim v_intFrom, v_intTo As Integer

                    If v_strClause <> "" Then
                        v_intFrom = CInt(v_strClause.Split("|")(0))
                        v_intTo = CInt(v_strClause.Split("|")(1))

                        v_strSQLTmp = "SELECT * FROM (SELECT T.*,rownum rn FROM (" & v_strSQL & ") T) WHERE RN BETWEEN " & v_intFrom & " AND " & v_intTo
                    Else
                        v_strSQLTmp = v_strSQL
                    End If

                    If v_trace_status = "1" Then
                        Trace.WriteLine("[SI - " & v_strObjName & "] " & DateTime.Now & " :" & vbCrLf & " -o- Ket qua:-o-" & vbCrLf & v_strSQL & vbCrLf)
                    End If

                    v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                    'Write Log
                    'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "CoreBusiness", "objMaster", "CoreHostInquery", "", "", mv_strTellerId, mv_strTellerName, "Excute SQL Command: " & v_strSQLTmp)
                End If
                'End If
            End If

            v_obj.Commit()
            'Write Log
            'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "CoreBusiness", "objMaster", "CoreHostExport", "", "", mv_strTellerId, mv_strTellerName, "Excute SQL Command: Commit")

            v_ds = ModifySearchResult(v_ds, v_strObjName)

            Dim v_strFilePath As String = v_strSearchResultRootPath & v_strSearchResultPath
            Dim v_strFolderPath As String = Mid(v_strFilePath, 1, v_strFilePath.LastIndexOf("\"))
            Dim v_strFileName As String = Mid(v_strFilePath, v_strFilePath.LastIndexOf("\") + 2, Len(v_strFilePath) - v_strFilePath.LastIndexOf("\") - 1)

            v_ds.WriteXml(v_strFilePath)

            Dim v_oZipEngine As New ZipEngine
            v_strFilePath = v_oZipEngine.ZipFile(v_strFolderPath, v_strFileName)

            Return v_strSearchResultRootPath & "|" & v_strFilePath & "|" & v_strServerAddress & "|" & v_strServerPort & "|" & v_strUserName & "|" & v_strPassword

        Catch ex As Exception
            'Write Log
            'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "CoreBusiness", "objMaster", "CoreHostExport", "", "", mv_strTellerId, mv_strTellerName, "Error: " & ex.ToString)

            v_obj.Commit()
            Throw ex
        Finally
            'Write Log
            'mv_lwLogWriter.StopWriteLog(gc_MODULE_HOST, "CoreBusiness", "objMaster", "CoreHostExport", "", "", mv_strTellerId, mv_strTellerName, "")
            If v_trace_status = "1" Then
                Trace.WriteLine("[Kết thúc: SI - " & v_strObjName & "] " & DateTime.Now & vbCrLf)
                tr2.Close()
                tr2.Dispose()
            End If
            v_obj.Dispose()
            If Not v_ds Is Nothing Then
                v_ds.Dispose()
            End If
            If Not v_ds1 Is Nothing Then
                v_ds1.Dispose()
            End If
            GC.Collect()
        End Try
    End Function

    'Private Function CoreHostExport(ByRef pv_xmlDocument As XmlDocumentEx) As String
    '    Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes

    '    Dim v_strClause As String
    '    Dim v_strLocal As String
    '    Dim v_strObjName As String
    '    Dim v_strCmdInquiry As String
    '    Dim v_strCondition As String
    '    Dim v_strBRID As String
    '    Dim v_strVsdBrid As String
    '    Dim v_obj As New DataAccess
    '    Dim v_arrField() As String

    '    Dim v_ds As DataSet
    '    Dim v_strSQL, v_strSQLTmp As String
    '    Dim tr2 As TextWriterTraceListener
    '    Dim v_trace_status, v_trace_path As String
    '    Dim v_strSearchResultRootPath, v_strSearchResultPath As String
    '    Dim v_strServerAddress, v_strServerPort, v_strUserName, v_strPassword As String
    '    Dim v_lstBrID As String
    '    Dim v_ds1 As DataSet
    '    Dim v_trace As DataSet
    '    Dim v_lngError As Long = ERR_SYSTEM_OK
    '    Try
    '        If Not (v_attrColl.GetNamedItem(gc_AtributeCMDINQUIRY) Is Nothing) Then
    '            v_strCmdInquiry = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCMDINQUIRY), Xml.XmlAttribute).Value)
    '        Else
    '            v_strCmdInquiry = String.Empty
    '        End If

    '        If Not (v_attrColl.GetNamedItem(gc_AtributeFUNCNAME) Is Nothing) Then
    '            v_strClause = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeFUNCNAME), Xml.XmlAttribute).Value)
    '        Else
    '            v_strClause = String.Empty
    '        End If

    '        If Not (v_attrColl.GetNamedItem(gc_AtributeLOCAL) Is Nothing) Then
    '            v_strLocal = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeLOCAL), Xml.XmlAttribute).Value)
    '        Else
    '            v_strLocal = String.Empty
    '        End If
    '        If Not (v_attrColl.GetNamedItem(gc_AtributeCLAUSE) Is Nothing) Then
    '            v_strCondition = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCLAUSE), Xml.XmlAttribute).Value)
    '        Else
    '            v_strCondition = String.Empty
    '        End If

    '        If Not (v_attrColl.GetNamedItem(gc_AtributeOBJNAME) Is Nothing) Then
    '            v_strObjName = Mid(CStr(CType(v_attrColl.GetNamedItem(gc_AtributeOBJNAME), Xml.XmlAttribute).Value), 4)
    '        Else
    '            v_strObjName = ATTR_TABLE
    '        End If
    '        Dim v_strPartition As String
    '        If Not (v_attrColl.GetNamedItem(gc_AtributeREFERENCE) Is Nothing) Then
    '            v_strPartition = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeREFERENCE), Xml.XmlAttribute).Value)
    '        Else
    '            v_strPartition = ""
    '        End If

    '        Dim v_strAutoID As String
    '        If Not (v_attrColl.GetNamedItem(gc_AtributeAUTOID) Is Nothing) Then
    '            v_strAutoID = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeAUTOID), Xml.XmlAttribute).Value)
    '            If Not IsNumeric(v_strAutoID) Then
    '                v_strAutoID = "0"
    '            End If
    '        Else
    '            v_strAutoID = "0"
    '        End If

    '        v_strBRID = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeBRID), Xml.XmlAttribute).Value)
    '        If Not (v_attrColl.GetNamedItem(gc_AtributeVSDBRID) Is Nothing) Then
    '            v_strVsdBrid = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeVSDBRID), Xml.XmlAttribute).Value)
    '        Else
    '            v_strVsdBrid = ""
    '        End If

    '        Dim v_strTellerID As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTLID), Xml.XmlAttribute).Value)
    '        Dim v_strTLName As String = v_obj.GetValue("TLPROFILES", "TLNAME", "TLID='" & v_strTellerID & "'")
    '        mv_strTellerId = v_strTellerID
    '        mv_strTellerName = v_strTLName

    '        Dim v_arrFilter As String
    '        v_arrFilter = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTXNUM), Xml.XmlAttribute).Value)

    '        'Write Log
    '        'mv_lwLogWriter.StartWriteLog(gc_MODULE_HOST, "CoreBusiness", "objMaster", "CoreHostExport", "", "", mv_strTellerId, mv_strTellerName, "")

    '        Dim v_blnSearch As Boolean = True
    '        Dim v_strCurrDate As String
    '        Dim v_strMaxDate, v_strMinDate As String
    '        v_trace_status = "0"

    '        If v_strLocal = gc_IsInQueryNotLocalMsg Then
    '            v_obj.NewDBInstance(gc_MODULE_INQUERY)
    '        Else
    '            v_obj.NewDBInstance(gc_MODULE_HOST)
    '        End If

    '        v_obj.BeginTran()

    '        v_obj.GetSysVar("SYSTEM", "TRACE_STATUS", v_trace_status)
    '        v_obj.GetSysVar("SYSTEM", "CURRDATE", v_strBRID, v_strCurrDate)

    '        v_obj.GetSysVar("SYSTEM", "FTP_SERVER_ADDRESS", v_strServerAddress)
    '        v_obj.GetSysVar("SYSTEM", "FTP_SERVER_PORT", v_strServerPort)
    '        v_obj.GetSysVar("SYSTEM", "FTP_USERNAME", v_strUserName)
    '        v_obj.GetSysVar("SYSTEM", "FTP_PASSWORD", v_strPassword)
    '        v_obj.GetSysVar("SYSTEM", "SEARCH_RESULT_PATH", v_strSearchResultRootPath)


    '        v_strCurrDate = Replace(v_strCurrDate, "/", "_")
    '        If v_strSearchResultRootPath = "" Then
    '            Dim v_app As New ApplicationServices.ApplicationBase
    '            v_strSearchResultRootPath = v_app.Info.DirectoryPath & "\VSD_SRC_RESULT\"
    '        End If

    '        v_strSearchResultPath = v_strCurrDate

    '        If Not System.IO.Directory.Exists(v_strSearchResultRootPath & v_strCurrDate) Then
    '            System.IO.Directory.CreateDirectory(v_strSearchResultRootPath & v_strCurrDate)
    '        End If

    '        v_strSearchResultPath &= "\Ket_Qua_Tim_Kiem_" & v_strBRID & "_" & v_strTLName & "_" & Replace(Replace(Now.ToShortTimeString, ":", "_"), " ", "_") & ".xls"

    '        ' date : 14/10/2008
    '        ' Purpose : get from  date T-3 to date T
    '        Dim v_strT_T_3 As String
    '        v_strSQL = "select to_char(GET_T_PLUS(to_date('" & v_strCurrDate & "', 'dd/mm/yyyy'),'" & v_strBRID & "',-1),'dd/mm/yyyy') txdate from dual" _
    '                & " union select to_char(GET_T_PLUS(to_date('" & v_strCurrDate & "', 'dd/mm/yyyy'),'" & v_strBRID & "',-2),'dd/mm/yyyy') txdate from dual" _
    '                & " union select to_char(GET_T_PLUS(to_date('" & v_strCurrDate & "', 'dd/mm/yyyy'),'" & v_strBRID & "',-3),'dd/mm/yyyy') txdate from dual"
    '        v_trace = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
    '        v_strT_T_3 = "to_date('" & v_strCurrDate & "', 'dd/mm/yyyy'),"
    '        For i = 0 To v_trace.Tables(0).Rows.Count - 1
    '            v_strT_T_3 = v_strT_T_3 & "to_date('" & v_trace.Tables(0).Rows(i)("txdate") & "', 'dd/mm/yyyy'),"
    '        Next
    '        v_strT_T_3 = "(" & Left(v_strT_T_3, v_strT_T_3.Length - 1) & ")"
    '        ' end 

    '        If v_trace_status = "1" Then
    '            v_obj.GetSysVar("SYSTEM", "TRACE_PATH", v_trace_path)
    '            v_strCurrDate = Replace(v_strCurrDate, "/", "_")
    '            If v_trace_path = "" Then
    '                Dim v_app As New ApplicationServices.ApplicationBase
    '                v_trace_path = v_app.Info.DirectoryPath & "\Log\" & v_strCurrDate
    '            Else
    '                v_trace_path = v_trace_path & v_strCurrDate
    '            End If

    '            If Not System.IO.Directory.Exists(v_trace_path) Then
    '                System.IO.Directory.CreateDirectory(v_trace_path)
    '            End If

    '            v_trace_path &= "\log_search_br" & v_strBRID & "_" & v_strTLName & ".txt"

    '            tr2 = New TextWriterTraceListener(System.IO.File.AppendText(v_trace_path))
    '            Trace.Listeners.Add(tr2)
    '            Trace.WriteLine("[Bắt đầu: SI - " & v_strObjName & "] " & DateTime.Now & vbCrLf)
    '        End If

    '        'Xu ly DL bang TRAN, TLLOG
    '        v_strSQL = "SELECT TLLOGTRAN FROM SISEARCH WHERE SEARCHCODE = '" & v_strObjName & "'"
    '        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
    '        'Write Log
    '        'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "CoreBusiness", "objMaster", "CoreHostExport", "", "", mv_strTellerId, mv_strTellerName, "Excute SQL Command: ")

    '        Dim v_strTran As String = "" 'IA|MA|CA|MF|RA|RG|SF|TLLOG
    '        Dim v_lstTran() As String
    '        Dim v_blnLoadAll As Boolean = False

    '        If v_ds.Tables(0).Rows.Count > 0 Then
    '            v_strTran = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("TLLOGTRAN")).ToUpper
    '        End If

    '        v_strMinDate = Replace(v_strCurrDate, "_", "/")
    '        v_strMaxDate = Replace(v_strCurrDate, "_", "/")
    '        If v_strPartition <> "" Then
    '            v_strMinDate = v_strPartition.Split("|")(0)
    '            v_strMaxDate = v_strPartition.Split("|")(1)
    '        End If
    '        v_blnSearch = CheckPassDate(v_strMaxDate, Replace(v_strCurrDate, "_", "/"))
    '        v_blnLoadAll = CheckPassDate(v_strMinDate, Replace(v_strCurrDate, "_", "/"))


    '        Dim v_strTllogWhere As String = ""
    '        If v_blnLoadAll Then
    '            v_strTllogWhere = " AND [#TXDATE [!O_TXDATE] TO_DATE('[!V_TXDATE]','" & gc_FORMAT_DATE & "')#] AND [#TXNUM [!O_TXNUM] '[!V_TXNUM]'#]"
    '            v_strTllogWhere &= " AND [#BUSDATE [!O_TXDATE] TO_DATE('[!V_BUSDATE]','" & gc_FORMAT_DATE & "')#] AND [#TLTXCD [!O_TLTXCD] '[!V_TLTXCD]'#]"
    '        End If

    '        If v_strTran <> "" Then
    '            v_lstTran = v_strTran.Split("|")
    '            v_strTran = ""
    '            For i As Integer = 0 To v_lstTran.Length - 1
    '                Select Case v_lstTran(i)
    '                    Case "TLLOG"
    '                        v_strTran &= ",'TLLOG'"

    '                        If v_blnLoadAll And v_strLocal = gc_IsInQueryNotLocalMsg Then
    '                            v_strTran &= ",'TLLOGALL'"
    '                        End If
    '                    Case "IA", "MA", "RA", "SF"
    '                        v_strTran &= ",'" & v_lstTran(i) & "TRAN'"
    '                        v_strTran &= ",'" & v_lstTran(i) & "MAST'"

    '                        If v_blnLoadAll Then
    '                            v_strTran &= ",'" & v_lstTran(i) & "TRANA'"
    '                        End If
    '                    Case "CA"
    '                        v_strTran &= ",'" & v_lstTran(i) & "TRAN'"
    '                        If v_strPartition <> "" Then
    '                            v_strTran &= ",'" & v_lstTran(i) & "TRANA'"
    '                            v_blnLoadAll = True
    '                        End If
    '                    Case "RG", "MF"
    '                        v_strTran &= ",'" & v_lstTran(i) & "TRAN'"
    '                        If v_blnLoadAll Then
    '                            v_strTran &= ",'" & v_lstTran(i) & "TRANA'"
    '                        End If
    '                End Select
    '            Next
    '            v_strTran = "(" & Mid(v_strTran, 2) & ")"
    '        End If

    '        Dim v_lstSICODE, v_lstMICODE As String

    '        v_lstMICODE = v_arrFilter.Split("|")(1)
    '        v_lstSICODE = v_arrFilter.Split("|")(0)
    '        Dim v_intCount As Integer = -1

    '        If v_strCondition <> "" Then
    '            v_arrField = v_strCondition.Split("#")
    '            v_intCount = v_arrField.Length - 2
    '        End If
    '        Dim v_hCondition As New Hashtable
    '        Dim v_strhKey, v_strhValue As String
    '        Dim v_blnReplace As Boolean

    '        If v_intCount >= 0 Then
    '            For j = 0 To v_intCount
    '                v_strhKey = v_arrField(j).Split("|")(0).ToUpper
    '                Select Case v_strhKey
    '                    Case "SICODE"
    '                        v_lstSICODE = "('" & v_arrField(j).Split("|")(2) & "')"
    '                    Case "MICODE"
    '                        v_lstMICODE = "('" & v_arrField(j).Split("|")(2) & "')"
    '                End Select
    '            Next
    '        End If
    '        'Lay phan quyen chi nhanh
    '        v_strSQL = "SELECT DISTINCT brid brid  FROM " _
    '                   & " (SELECT b.brid FROM tlbridauth a, brgrp b" _
    '                   & " WHERE ((AUTHID = '" & v_strTellerID & "' AND authtype = 'U') OR (AUTHID IN (SELECT a.grpid" _
    '                   & " FROM tlgrpusers a WHERE a.tlid = '" & v_strTellerID & "') AND authtype = 'G'))" _
    '                   & " And a.brid = b.brid And a.deleted = 0 And a.status = 0" _
    '                   & " And b.deleted = 0 And b.status = 0" _
    '                   & " UNION " _
    '                   & " SELECT b.brid FROM tlprofiles a, brgrp b" _
    '                   & " WHERE a.brid = b.brid AND a.tlid = '" & v_strTellerID & "'" _
    '                   & " AND a.deleted = 0 AND a.status = 0 AND b.deleted = 0 AND b.status = 0)"

    '        v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
    '        'Write Log
    '        'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "CoreBusiness", "objMaster", "CoreHostExport", "", "", mv_strTellerId, mv_strTellerName, "Excute SQL Command: " & v_strSQL)

    '        v_lstBrID = ""
    '        For i = 0 To v_ds.Tables(0).Rows.Count - 1
    '            v_lstBrID &= ",'" & v_ds.Tables(0).Rows(i)(0) & "'"
    '        Next
    '        v_lstBrID = "(" & Mid(v_lstBrID, 2) & ")"

    '        'Lay TVLK dc phan quyen cho user vao bang TMP_RGMI
    '        v_strSQL = "INSERT INTO tmp_rgmi " _
    '                   & " SELECT DISTINCT m.* FROM rgmi m WHERE m.deleted=0 AND m.status=0" _
    '                   & " AND m.MICODE IN " & v_lstMICODE

    '        v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
    '        'Write Log
    '        'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "CoreBusiness", "objMaster", "CoreHostExport", "", "", mv_strTellerId, mv_strTellerName, "Excute SQL Command: " & v_strSQL)


    '        'v_strSQL = "INSERT INTO TMP_RGMI(AUTOID, MICODE) SELECT '0' AUTOID, '000' MICODE FROM DUAL"
    '        'v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

    '        'Lay chung khoan dc phan quyen cho user vao bang TMP_RGSI
    '        ' ngay : 05092009 
    '        ' muc dich : tach chuoi v_lstSICODE khac phuc loi 
    '        '               ORA-01795: maximum number of expressions in a list is 1000
    '        Dim v_strCut, v_strCut1, v_strCut2, v_strCut3 As String
    '        Dim v_iCut As Integer
    '        v_iCut = (v_lstSICODE.Length \ 3)
    '        v_iCut = InStr(v_iCut, v_lstSICODE, ",")
    '        If v_iCut > 0 Then
    '            v_strCut1 = v_lstSICODE.Substring(0, v_iCut - 1) & ")"
    '            v_strSQL = "INSERT INTO tmp_rgsi(iscode, sicode, stock_name, part_value, TYPE," _
    '            & "stock_type, interest_rate, interest_period, " _
    '            & "bond_period, deleted, exchange_rate, note, " _
    '            & "bond_term, release_series, release_mode, isin, " _
    '            & "rarate, tmpid, paidmethod, npaiddate1, npaiddate2, " _
    '            & "npaiddate3, npaiddate4, int_release_mode, brid, status, public_date, currency_code) " _
    '            & " SELECT distinct m.iscode, m.sicode, m.stock_name, m.part_value, m.TYPE," _
    '            & "m.stock_type, m.interest_rate, m.interest_period," _
    '            & "m.bond_period, m.deleted, m.exchange_rate, m.note, " _
    '            & "m.bond_term, m.release_series, m.release_mode, m.isin, " _
    '            & "m.rarate, m.tmpid, m.paidmethod, m.npaiddate1, m.npaiddate2," _
    '            & "m.npaiddate3, m.npaiddate4, m.int_release_mode, m.brid, m.status, m.public_date, m.currency_code FROM rgsi m" _
    '            & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_strCut1
    '            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
    '            'Write Log
    '            'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "CoreBusiness", "objMaster", "CoreHostExport", "", "", mv_strTellerId, mv_strTellerName, "Excute SQL Command: " & v_strSQL)

    '            v_strCut = "(" & v_lstSICODE.Substring(v_iCut)
    '            v_iCut = InStr(v_iCut, v_strCut, ",")
    '            If v_iCut > 0 Then
    '                v_strCut2 = v_strCut.Substring(0, v_iCut - 1) & ")"
    '                v_strSQL = "INSERT INTO tmp_rgsi(iscode, sicode, stock_name, part_value, TYPE," _
    '                      & "stock_type, interest_rate, interest_period, " _
    '                      & "bond_period, deleted, exchange_rate, note, " _
    '                      & "bond_term, release_series, release_mode, isin, " _
    '                      & "rarate, tmpid, paidmethod, npaiddate1, npaiddate2, " _
    '                      & "npaiddate3, npaiddate4, int_release_mode, brid, status, public_date, currency_code) " _
    '                      & " SELECT distinct m.iscode, m.sicode, m.stock_name, m.part_value, m.TYPE," _
    '                      & "m.stock_type, m.interest_rate, m.interest_period," _
    '                      & "m.bond_period, m.deleted, m.exchange_rate, m.note, " _
    '                      & "m.bond_term, m.release_series, m.release_mode, m.isin, " _
    '                      & "m.rarate, m.tmpid, m.paidmethod, m.npaiddate1, m.npaiddate2," _
    '                      & "m.npaiddate3, m.npaiddate4, m.int_release_mode, m.brid, m.status, m.public_date, m.currency_code FROM rgsi m" _
    '                      & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_strCut2
    '                v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
    '                'Write Log
    '                'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "CoreBusiness", "objMaster", "CoreHostExport", "", "", mv_strTellerId, mv_strTellerName, "Excute SQL Command: " & v_strSQL)

    '                v_strCut3 = "(" & v_strCut.Substring(v_iCut)
    '                v_strSQL = "INSERT INTO tmp_rgsi(iscode, sicode, stock_name, part_value, TYPE," _
    '                        & "stock_type, interest_rate, interest_period, " _
    '                        & "bond_period, deleted, exchange_rate, note, " _
    '                        & "bond_term, release_series, release_mode, isin, " _
    '                        & "rarate, tmpid, paidmethod, npaiddate1, npaiddate2, " _
    '                        & "npaiddate3, npaiddate4, int_release_mode, brid, status, public_date, currency_code) " _
    '                        & " SELECT distinct m.iscode, m.sicode, m.stock_name, m.part_value, m.TYPE," _
    '                        & "m.stock_type, m.interest_rate, m.interest_period," _
    '                        & "m.bond_period, m.deleted, m.exchange_rate, m.note, " _
    '                        & "m.bond_term, m.release_series, m.release_mode, m.isin, " _
    '                        & "m.rarate, m.tmpid, m.paidmethod, m.npaiddate1, m.npaiddate2," _
    '                        & "m.npaiddate3, m.npaiddate4, m.int_release_mode, m.brid, m.status, m.public_date, m.currency_code FROM rgsi m" _
    '                        & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_strCut3
    '                v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
    '                'Write Log
    '                'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "CoreBusiness", "objMaster", "CoreHostExport", "", "", mv_strTellerId, mv_strTellerName, "Excute SQL Command: " & v_strSQL)

    '            Else
    '                v_strSQL = "INSERT INTO tmp_rgsi(iscode, sicode, stock_name, part_value, TYPE," _
    '                          & "stock_type, interest_rate, interest_period, " _
    '                          & "bond_period, deleted, exchange_rate, note, " _
    '                          & "bond_term, release_series, release_mode, isin, " _
    '                          & "rarate, tmpid, paidmethod, npaiddate1, npaiddate2, " _
    '                          & "npaiddate3, npaiddate4, int_release_mode, brid, status, public_date, currency_code) " _
    '                          & " SELECT distinct m.iscode, m.sicode, m.stock_name, m.part_value, m.TYPE," _
    '                          & "m.stock_type, m.interest_rate, m.interest_period," _
    '                          & "m.bond_period, m.deleted, m.exchange_rate, m.note, " _
    '                          & "m.bond_term, m.release_series, m.release_mode, m.isin, " _
    '                          & "m.rarate, m.tmpid, m.paidmethod, m.npaiddate1, m.npaiddate2," _
    '                          & "m.npaiddate3, m.npaiddate4, m.int_release_mode, m.brid, m.status, m.public_date, m.currency_code FROM rgsi m" _
    '                          & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_strCut
    '                v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
    '                'Write Log
    '                'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "CoreBusiness", "objMaster", "CoreHostExport", "", "", mv_strTellerId, mv_strTellerName, "Excute SQL Command: " & v_strSQL)

    '            End If
    '        Else
    '            v_strSQL = "INSERT INTO tmp_rgsi(iscode, sicode, stock_name, part_value, TYPE," _
    '                          & "stock_type, interest_rate, interest_period, " _
    '                          & "bond_period, deleted, exchange_rate, note, " _
    '                          & "bond_term, release_series, release_mode, isin, " _
    '                          & "rarate, tmpid, paidmethod, npaiddate1, npaiddate2, " _
    '                          & "npaiddate3, npaiddate4, int_release_mode, brid, status, public_date, currency_code) " _
    '                          & " SELECT distinct m.iscode, m.sicode, m.stock_name, m.part_value, m.TYPE," _
    '                          & "m.stock_type, m.interest_rate, m.interest_period," _
    '                          & "m.bond_period, m.deleted, m.exchange_rate, m.note, " _
    '                          & "m.bond_term, m.release_series, m.release_mode, m.isin, " _
    '                          & "m.rarate, m.tmpid, m.paidmethod, m.npaiddate1, m.npaiddate2," _
    '                          & "m.npaiddate3, m.npaiddate4, m.int_release_mode, m.brid, m.status, m.public_date, m.currency_code FROM rgsi m" _
    '                          & " WHERE m.deleted=0 AND m.BRID IN " & v_lstBrID & " AND m.SICODE IN " & v_lstSICODE
    '            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
    '            'Write Log
    '            'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "CoreBusiness", "objMaster", "CoreHostExport", "", "", mv_strTellerId, mv_strTellerName, "Excute SQL Command: " & v_strSQL)

    '        End If
    '        ' tuanta
    '        v_strSQL = "INSERT INTO TMP_RGSI(AUTOID, SICODE) SELECT 0 AUTOID, '000' SICODE FROM DUAL"
    '        v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
    '        'Write Log
    '        'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "CoreBusiness", "objMaster", "CoreHostExport", "", "", mv_strTellerId, mv_strTellerName, "Excute SQL Command: " & v_strSQL)


    '        'Lay cau xu ly chung
    '        Dim v_hSQL As New Hashtable
    '        Dim v_hORD As New Hashtable
    '        If v_strTran <> "" Then
    '            v_strSQL = "SELECT * FROM SISTORES WHERE SEARCHCODE IS NULL AND OVERWRITE IN " & v_strTran & " ORDER BY ODRNUM"

    '            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
    '            'Write Log
    '            'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "CoreBusiness", "objMaster", "CoreHostExport", "", "", mv_strTellerId, mv_strTellerName, "Excute SQL Command: " & v_strSQL)

    '            Dim v_strOverWrite As String
    '            If v_ds.Tables(0).Rows.Count > 0 Then
    '                For i As Integer = 0 To v_ds.Tables(0).Rows.Count - 1
    '                    v_strOverWrite = v_ds.Tables(0).Rows(i)("OVERWRITE")
    '                    v_hORD(i) = v_strOverWrite
    '                    v_hSQL(v_strOverWrite) = v_ds.Tables(0).Rows(i)("TRACE") & v_ds.Tables(0).Rows(i)("CMDSQL") & " " & v_ds.Tables(0).Rows(i)("CMDSQL1")
    '                Next
    '            End If

    '            'Lay cau xu ly chung da dc viet lai
    '            v_strSQL = "SELECT * FROM SISTORES WHERE SEARCHCODE='" & v_strObjName & "' AND OVERWRITE IN " & v_strTran & " ORDER BY ODRNUM"
    '            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
    '            'Write Log
    '            'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "CoreBusiness", "objMaster", "CoreHostExport", "", "", mv_strTellerId, mv_strTellerName, "Excute SQL Command: " & v_strSQL)

    '            If v_ds.Tables(0).Rows.Count > 0 Then
    '                For i As Integer = 0 To v_ds.Tables(0).Rows.Count - 1
    '                    v_strOverWrite = v_ds.Tables(0).Rows(i)("OVERWRITE")
    '                    If Not v_hSQL(v_strOverWrite) Is Nothing Then
    '                        v_hSQL(v_strOverWrite) = v_ds.Tables(0).Rows(i)("TRACE") & v_ds.Tables(0).Rows(i)("CMDSQL") & " " & v_ds.Tables(0).Rows(i)("CMDSQL1")
    '                    End If
    '                Next
    '            End If

    '            'Thuc hien cau xu ly chung
    '            Dim v_strPartitionSQL As String
    '            'Dim v_lstPartition As String
    '            For i As Integer = 0 To v_hORD.Count - 1
    '                v_strOverWrite = v_hORD(i)
    '                v_strSQL = Mid(v_hSQL(v_strOverWrite), 2)
    '                'v_strPartitionSQL = ""
    '                v_strSQL = Replace(v_strSQL, "[!BRID]", v_strBRID)
    '                v_strSQL = Replace(v_strSQL, "[!VSD_BRID]", v_strVsdBrid)
    '                v_strSQL = Replace(v_strSQL, "[!AUTOID]", v_strAutoID)
    '                v_strSQL = Replace(v_strSQL, "[!TLID]", v_strTellerID)
    '                v_strSQL = Replace(v_strSQL, "[!LBRID]", v_lstBrID)
    '                v_strSQL = Replace(v_strSQL, "[!LMICODE]", v_lstMICODE)
    '                v_strSQL = Replace(v_strSQL, "[!T_T_3]", v_strT_T_3)
    '                v_strSQL = Replace(v_strSQL, "[!MINDATE]", "TO_DATE('" & v_strMinDate & "','dd/mm/yyyy')")
    '                v_strSQL = Replace(v_strSQL, "[!MAXDATE]", "TO_DATE('" & v_strMaxDate & "','dd/mm/yyyy')")

    '                v_strSQL = Replace(v_strSQL, "TT_", "")
    '                v_strSQL = Replace(v_strSQL, "tt_", "")
    '                v_strSQL = Replace(v_strSQL, "tT_", "")
    '                v_strSQL = Replace(v_strSQL, "Tt_", "")

    '                v_strSQL = Replace(v_strSQL, "[!TLLOGALL]", "TLLOGALL_ALL")
    '                v_strSQL = Replace(v_strSQL, "[!CATRANA]", "CATRANA_ALL")
    '                v_strSQL = Replace(v_strSQL, "[!IATRANA]", "IATRANA_ALL")
    '                v_strSQL = Replace(v_strSQL, "[!MATRANA]", "MATRANA_ALL")
    '                v_strSQL = Replace(v_strSQL, "[!SFTRANA]", "SFTRANA_ALL")
    '                v_strSQL = Replace(v_strSQL, "[!MFTRANA]", "MFTRANA_ALL")
    '                v_strSQL = Replace(v_strSQL, "[!RATRANA]", "RATRANA_ALL")
    '                v_strSQL = Replace(v_strSQL, "[!RGTRANA]", "RGTRANA_ALL")

    '                If Right(v_strOverWrite, 3) = "ALL" Or Right(v_strOverWrite, 1) = "A" Then
    '                    'If Mid(v_strOverWrite, 1, 5).ToUpper = "TLLOG" Then
    '                    '    v_lstPartition = GetListPartition(v_strPartition)
    '                    'Else
    '                    '    v_lstPartition = GetListPartition(v_strMinDate & "|" & Replace(v_strCurrDate, "_", "/"))
    '                    'End If

    '                    'For j As Integer = 0 To v_lstPartition.Split("|").Length - 2
    '                    '    If v_strPartitionSQL <> "" Then
    '                    '        v_strPartitionSQL &= " UNION ALL "
    '                    '    End If

    '                    '    v_strPartitionSQL &= v_strSQL.Substring(InStr(v_strSQL.ToUpper, "SELECT") - 1)
    '                    'v_strPartitionSQL = Replace(v_strPartitionSQL, "[!TLLOGALL]", "TLLOGALL_ALL PARTITION(" & v_lstPartition.Split("|")(j) & ")")
    '                    'v_strPartitionSQL = Replace(v_strPartitionSQL, "[!CATRANA]", "CATRANA_ALL PARTITION(" & v_lstPartition.Split("|")(j) & ")")
    '                    'v_strPartitionSQL = Replace(v_strPartitionSQL, "[!IATRANA]", "IATRANA_ALL PARTITION(" & v_lstPartition.Split("|")(j) & ")")
    '                    'v_strPartitionSQL = Replace(v_strPartitionSQL, "[!MATRANA]", "MATRANA_ALL PARTITION(" & v_lstPartition.Split("|")(j) & ")")
    '                    'v_strPartitionSQL = Replace(v_strPartitionSQL, "[!SFTRANA]", "SFTRANA_ALL PARTITION(" & v_lstPartition.Split("|")(j) & ")")
    '                    'v_strPartitionSQL = Replace(v_strPartitionSQL, "[!MFTRANA]", "MFTRANA_ALL PARTITION(" & v_lstPartition.Split("|")(j) & ")")
    '                    'v_strPartitionSQL = Replace(v_strPartitionSQL, "[!RATRANA]", "RATRANA_ALL PARTITION(" & v_lstPartition.Split("|")(j) & ")")
    '                    'v_strPartitionSQL = Replace(v_strPartitionSQL, "[!RGTRANA]", "RGTRANA_ALL PARTITION(" & v_lstPartition.Split("|")(j) & ")")
    '                    'Next
    '                    If v_strOverWrite = "TLLOGALL" Then
    '                        v_strPartitionSQL = v_strSQL.Substring(InStr(v_strSQL.ToUpper, "SELECT") - 1)
    '                        v_strSQL = Mid(v_strSQL, 1, InStr(v_strSQL.ToUpper, "SELECT") - 1)
    '                        v_strPartitionSQL = " SELECT * FROM (" & v_strPartitionSQL & ") WHERE 1=1" & v_strTllogWhere
    '                        v_strSQL &= v_strPartitionSQL
    '                    End If

    '                    If v_intCount >= 0 Then
    '                        For j = 0 To v_intCount
    '                            v_strSQL = Replace(v_strSQL, "[!O_" & v_arrField(j).Split("|")(0) & "]", v_arrField(j).Split("|")(1))
    '                            v_strSQL = Replace(v_strSQL, "[!V_" & v_arrField(j).Split("|")(0) & "]", v_arrField(j).Split("|")(2))
    '                        Next
    '                    End If
    '                End If

    '                v_strSQL = Replace(v_strSQL, "[!V_TXDATE]", Replace(v_strCurrDate, "_", "/"))
    '                v_strSQL = DecodeSQL(v_strSQL)

    '                If v_trace_status = "1" And Mid(v_hSQL(v_strOverWrite).ToString, 1, 1) = "1" Then
    '                    Trace.WriteLine("[SI - " & v_strObjName & "] " & DateTime.Now & " :" & vbCrLf & " -o- Câu lệnh thứ xử lý chung thứ #" & i + 1 & "-o-" & vbCrLf & v_strSQL & vbCrLf)
    '                    If InStr(v_strSQL.ToUpper, "SELECT") > 0 Then
    '                        Trace.WriteLine("-o- Dữ liệu câu lệnh xử lý chung thứ #" & i + 1 & "-o-")
    '                        v_strSQLTmp = v_strSQL.Substring(InStr(v_strSQL.ToUpper, "SELECT") - 1)
    '                        v_trace = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
    '                        Trace.WriteLine(v_trace.GetXml & vbCrLf)
    '                    End If
    '                End If
    '                v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
    '                'Write Log
    '                'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "CoreBusiness", "objMaster", "CoreHostExport", "", "", mv_strTellerId, mv_strTellerName, "Excute SQL Command: " & v_strSQL)

    '            Next
    '        End If

    '        v_strSQL = "SELECT * FROM SISTORES WHERE SEARCHCODE='" & v_strObjName & "' AND OVERWRITE IS NULL AND DELETED=0 AND STATUS=0 ORDER BY ODRNUM"
    '        v_ds1 = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
    '        'Write Log
    '        'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "CoreBusiness", "objMaster", "CoreHostExport", "", "", mv_strTellerId, mv_strTellerName, "Excute SQL Command: " & v_strSQL)


    '        'Su ly cau lay SQL trong SISTORES
    '        For i = 0 To v_ds1.Tables(0).Rows.Count - 1
    '            v_strSQL = v_ds1.Tables(0).Rows(i)("CMDSQL").ToString.ToUpper & " " & v_ds1.Tables(0).Rows(i)("CMDSQL1").ToString.ToUpper
    '            v_blnReplace = False

    '            If v_intCount >= 0 Then
    '                For j = 0 To v_intCount
    '                    v_strhKey = v_arrField(j).Split("|")(0)
    '                    v_strhValue = "[!V_" & v_arrField(j).Split("|")(0) & "]"
    '                    If InStr(v_strSQL, v_strhValue, CompareMethod.Text) > 0 Then
    '                        v_strSQL = Replace(v_strSQL, "[!O_" & v_arrField(j).Split("|")(0) & "]", v_arrField(j).Split("|")(1))
    '                        v_strSQL = Replace(v_strSQL, "[!V_" & v_arrField(j).Split("|")(0) & "]", v_arrField(j).Split("|")(2))

    '                        If v_hCondition(v_strhKey) Is Nothing Then
    '                            v_hCondition.Add(v_strhKey, v_strhValue)
    '                        End If
    '                        v_blnReplace = True
    '                    End If
    '                Next
    '            End If

    '            If (Not v_blnReplace And v_ds1.Tables(0).Rows(i)("SITYPE") = 0) Or v_blnReplace Then
    '                v_strSQL = DecodeSQL(v_strSQL)
    '                'Neu khong su dung bang TMP thi thong bao loi
    '                'If Not CheckTranTable(v_strSQL) Then
    '                '    v_lngError = 1
    '                '    Exit For
    '                'End If

    '                v_strSQL = Replace(v_strSQL, "[!BRID]", v_strBRID)
    '                v_strSQL = Replace(v_strSQL, "[!VSD_BRID]", v_strVsdBrid)
    '                v_strSQL = Replace(v_strSQL, "[!AUTOID]", v_strAutoID)
    '                v_strSQL = Replace(v_strSQL, "[!TLID]", v_strTellerID)
    '                v_strSQL = Replace(v_strSQL, "[!LBRID]", v_lstBrID)
    '                v_strSQL = Replace(v_strSQL, "[!LMICODE]", v_lstMICODE)
    '                v_strSQL = Replace(v_strSQL, "[!T_T_3]", v_strT_T_3)
    '                v_strSQL = Replace(v_strSQL, "[!MINDATE]", "TO_DATE('" & v_strMinDate & "','dd/mm/yyyy')")
    '                v_strSQL = Replace(v_strSQL, "[!MAXDATE]", "TO_DATE('" & v_strMaxDate & "','dd/mm/yyyy')")

    '                v_strSQL = Replace(v_strSQL, "TT_", "")
    '                v_strSQL = Replace(v_strSQL, "tt_", "")
    '                v_strSQL = Replace(v_strSQL, "tT_", "")
    '                v_strSQL = Replace(v_strSQL, "Tt_", "")

    '                If v_trace_status = "1" And v_ds1.Tables(0).Rows(i)("TRACE") = 1 Then
    '                    Trace.WriteLine("[SI - " & v_strObjName & "] " & DateTime.Now & " :" & vbCrLf & " -o- Câu lệnh thứ #" & v_ds1.Tables(0).Rows(i)("ODRNUM") & "-o-" & vbCrLf & v_strSQL & vbCrLf)
    '                    If InStr(v_strSQL.ToUpper, "SELECT") > 0 Then
    '                        Trace.WriteLine("-o- Dữ liệu câu lệnh thứ #" & v_ds1.Tables(0).Rows(i)("ODRNUM") & "-o-")
    '                        v_strSQLTmp = v_strSQL.Substring(InStr(v_strSQL.ToUpper, "SELECT") - 1)
    '                        v_trace = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
    '                        Trace.WriteLine(v_trace.GetXml & vbCrLf)
    '                    End If
    '                End If

    '                v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
    '                'Write Log
    '                'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "CoreBusiness", "objMaster", "CoreHostExport", "", "", mv_strTellerId, mv_strTellerName, "Excute SQL Command: " & v_strSQL)

    '            End If
    '        Next

    '        If v_lngError = ERR_SYSTEM_OK Then
    '            v_strSQL = v_strCmdInquiry

    '            'If Not CheckTranTable(v_strSQL) Then
    '            '    v_lngError = 1
    '            'Else
    '            If v_intCount >= 0 Then
    '                For i = 0 To v_intCount
    '                    v_strhKey = v_arrField(i).Split("|")(0)
    '                    v_strhValue = "[!V_" & v_arrField(i).Split("|")(0) & "]"
    '                    If InStr(v_strSQL, v_strhValue, CompareMethod.Text) > 0 Then
    '                        v_strSQL = Replace(v_strSQL, "[!O_" & v_arrField(i).Split("|")(0) & "]", v_arrField(i).Split("|")(1))
    '                        v_strSQL = Replace(v_strSQL, "[!V_" & v_arrField(i).Split("|")(0) & "]", v_arrField(i).Split("|")(2))

    '                        If v_hCondition(v_strhKey) Is Nothing Then
    '                            v_hCondition.Add(v_strhKey, v_strhValue)
    '                        End If
    '                    End If
    '                Next
    '            End If

    '            v_strSQL = DecodeSQL(v_strSQL)
    '            v_strSQL = Replace(v_strSQL, "TT_", "")
    '            v_strSQL = Replace(v_strSQL, "tt_", "")
    '            v_strSQL = Replace(v_strSQL, "tT_", "")
    '            v_strSQL = Replace(v_strSQL, "Tt_", "")
    '            Dim v_lngRowCount As Long

    '            Dim v_strWhere As String = ""
    '            Dim v_strTmp, v_strTmpField, v_strValue As String
    '            If v_strCondition <> "" Then
    '                v_arrField = v_strCondition.Split("#")
    '                v_intCount = v_arrField.Length - 2

    '                For i = 0 To v_arrField.Length - 2
    '                    If v_hCondition(v_arrField(i).Split("|")(0)) Is Nothing Then
    '                        'v_strWhere &= " AND " & v_arrField(i).Split("|")(0) & " " & v_arrField(i).Split("|")(1) & " "
    '                        v_strValue = v_arrField(i).Split("|")(2)
    '                        Select Case v_arrField(i).Split("|")(3)
    '                            Case "D", "P"
    '                                v_strTmpField = "TO_DATE(" & v_arrField(i).Split("|")(0) & ",'" & gc_FORMAT_DATE & "') " & v_arrField(i).Split("|")(1) & " "
    '                                v_strTmp = "TO_DATE('" & v_strValue & "', '" & gc_FORMAT_DATE & "')"
    '                            Case "N"
    '                                v_strTmpField = v_arrField(i).Split("|")(0) & " " & v_arrField(i).Split("|")(1) & " "
    '                                If IsNumeric(v_strValue) Then
    '                                    v_strTmp = CDbl(v_strValue)
    '                                Else
    '                                    v_strTmp = 0
    '                                End If
    '                            Case Else
    '                                v_strTmpField = v_arrField(i).Split("|")(0) & " " & v_arrField(i).Split("|")(1) & " "

    '                                v_strValue = Trim(Replace(v_strValue, ".", String.Empty))
    '                                If v_arrField(i).Split("|")(1) = "LIKE" Then
    '                                    v_strTmp = "N'%" & v_strValue & "%'"
    '                                Else
    '                                    v_strTmp = "N'" & v_strValue & "'"
    '                                End If
    '                        End Select

    '                        v_strWhere &= " AND " & v_strTmpField & v_strTmp
    '                    End If
    '                Next
    '            End If

    '            If Trim(v_strWhere) <> "" Then
    '                v_strSQL = "SELECT * FROM (" & v_strSQL & ") WHERE 1=1 " & v_strWhere
    '            End If
    '            v_strSQL = Replace(v_strSQL, "[!V_TXDATE]", Replace(v_strCurrDate, "_", "/"))
    '            v_strSQL = Replace(v_strSQL, "[!MINDATE]", "TO_DATE('" & v_strMinDate & "','dd/mm/yyyy')")
    '            v_strSQL = Replace(v_strSQL, "[!MAXDATE]", "TO_DATE('" & v_strMaxDate & "','dd/mm/yyyy')")
    '            v_strSQL = Replace(v_strSQL, "[!BRID]", v_strBRID)
    '            v_strSQL = Replace(v_strSQL, "[!T_T_3]", v_strT_T_3)
    '            v_strSQL = Replace(v_strSQL, "[!LMICODE]", v_lstMICODE)

    '            If v_strSQL <> "" Then
    '                'Tinh so ban ghi
    '                v_strSQLTmp = "SELECT COUNT(1) FROM (" & v_strSQL & ")"
    '                v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
    '                v_lngRowCount = v_ds.Tables(0).Rows(0)(0)

    '                If Not (v_attrColl.GetNamedItem(gc_AtributeFUNCNAME) Is Nothing) Then
    '                    pv_xmlDocument.DocumentElement.Attributes(gc_AtributeFUNCNAME).InnerXml = v_lngRowCount
    '                Else
    '                    Dim v_attr As Xml.XmlAttribute
    '                    v_attr = pv_xmlDocument.CreateAttribute(gc_AtributeFUNCNAME)
    '                    v_attr.Value = v_lngRowCount
    '                    Dim dataElement As Xml.XmlElement
    '                    dataElement = pv_xmlDocument.DocumentElement
    '                    dataElement.Attributes.Append(v_attr)
    '                End If

    '                'Lay DL loc theo dong
    '                Dim v_intFrom, v_intTo As Integer

    '                If v_strClause <> "" Then
    '                    v_intFrom = CInt(v_strClause.Split("|")(0))
    '                    v_intTo = CInt(v_strClause.Split("|")(1))

    '                    v_strSQLTmp = "SELECT * FROM (SELECT T.*,rownum rn FROM (" & v_strSQL & ") T) WHERE RN BETWEEN " & v_intFrom & " AND " & v_intTo
    '                Else
    '                    v_strSQLTmp = v_strSQL
    '                End If

    '                If v_trace_status = "1" Then
    '                    Trace.WriteLine("[SI - " & v_strObjName & "] " & DateTime.Now & " :" & vbCrLf & " -o- Ket qua:-o-" & vbCrLf & v_strSQL & vbCrLf)
    '                End If

    '                v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
    '                'Write Log
    '                'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "CoreBusiness", "objMaster", "CoreHostExport", "", "", mv_strTellerId, mv_strTellerName, "Excute SQL Command: " & v_strSQLTmp)

    '            End If
    '            'End If
    '        End If

    '        v_obj.Commit()
    '        'Write Log
    '        'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "CoreBusiness", "objMaster", "CoreHostExport", "", "", mv_strTellerId, mv_strTellerName, "Excute SQL Command: Commit")

    '        v_ds = ModifySearchResult(v_ds, v_strObjName)

    '        Dim v_strFilePath As String = v_strSearchResultRootPath & v_strSearchResultPath
    '        Dim v_strFolderPath As String = Mid(v_strFilePath, 1, v_strFilePath.LastIndexOf("\"))
    '        Dim v_strFileName As String = Mid(v_strFilePath, v_strFilePath.LastIndexOf("\") + 2, Len(v_strFilePath) - v_strFilePath.LastIndexOf("\") - 1)

    '        v_ds.WriteXml(v_strFilePath)

    '        Dim v_oZipEngine As New ZipEngine
    '        v_strFilePath = v_oZipEngine.ZipFile(v_strFolderPath, v_strFileName)

    '        Return v_strSearchResultRootPath & "|" & v_strFilePath & "|" & v_strServerAddress & "|" & v_strServerPort & "|" & v_strUserName & "|" & v_strPassword

    '    Catch ex As Exception
    '        'Write Log
    '        'mv_lwLogWriter.WriteLog(gc_MODULE_HOST, "CoreBusiness", "objMaster", "CoreHostExport", "", "", mv_strTellerId, mv_strTellerName, "Error: " & ex.ToString)

    '        v_obj.Commit()
    '        Throw ex
    '    Finally
    '        'Write Log
    '        'mv_lwLogWriter.StopWriteLog(gc_MODULE_HOST, "CoreBusiness", "objMaster", "CoreHostExport", "", "", mv_strTellerId, mv_strTellerName, "")
    '        If v_trace_status = "1" Then
    '            Trace.WriteLine("[Kết thúc: SI - " & v_strObjName & "] " & DateTime.Now & vbCrLf)
    '            tr2.Close()
    '            tr2.Dispose()
    '        End If
    '        v_obj.Dispose()
    '        If Not v_ds Is Nothing Then
    '            v_ds.Dispose()
    '        End If
    '        If Not v_ds1 Is Nothing Then
    '            v_ds1.Dispose()
    '        End If
    '        GC.Collect()
    '    End Try
    'End Function

    Public Function ModifySearchResult(ByVal pv_ds As DataSet, ByVal pv_strObjname As String)
        Dim v_obj As New DataAccess
        Dim v_ds As DataSet
        Dim v_intSearchFldCount As Integer = 0
        Dim v_strSql As String
        Try
            v_strSql = "SELECT FIELDCODE, FIELDNAME, DISPLAY FROM SISEARCHFLD WHERE DELETED = 0 AND STATUS = 0 AND SEARCHCODE = '" & pv_strObjname & "'"
            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSql)
            v_intSearchFldCount = v_ds.Tables(0).Rows.Count

            Dim v_arrFieldCode(v_intSearchFldCount) As String
            Dim v_arrFileName(v_intSearchFldCount) As String
            Dim v_arrFileDisplay(v_intSearchFldCount) As String

            For i As Integer = 0 To v_intSearchFldCount - 1
                With v_ds.Tables(0)
                    v_arrFieldCode(i) = Trim(.Rows(i)("FIELDCODE"))
                    v_arrFileName(i) = Trim(.Rows(i)("FIELDNAME"))
                    v_arrFileDisplay(i) = Trim(.Rows(i)("DISPLAY"))
                End With
            Next

            For j As Integer = 0 To v_intSearchFldCount - 1
                If v_arrFileDisplay(j) = "N" Then
                    If Not pv_ds.Tables(0).Columns(v_arrFieldCode(j)) Is Nothing Then
                        pv_ds.Tables(0).Columns.Remove(v_arrFieldCode(j))
                    End If
                Else
                    If Not pv_ds.Tables(0).Columns.Contains(v_arrFileName(j)) Then
                        pv_ds.Tables(0).Columns(v_arrFieldCode(j)).ColumnName = v_arrFileName(j)
                    Else
                        pv_ds.Tables(0).Columns(v_arrFieldCode(j)).ColumnName = v_arrFileName(j) & "_1"
                    End If

                End If
            Next

            Return pv_ds
        Catch ex As Exception
            Return Nothing
            Throw ex
        Finally
            If Not v_obj Is Nothing Then
                v_obj.Dispose()
            End If
            If Not v_ds Is Nothing Then
                v_ds.Dispose()
            End If
        End Try
    End Function
#End Region

#Region " Implement functions - Must override "
    Overridable Function CheckBeforeAdd(ByVal pv_xmlDocument As XmlDocumentEx) As Long
        Return CheckPermition(pv_xmlDocument, gc_ActionAdd)
        'ContextUtil.SetComplete()
        'Return 0
    End Function

    Overridable Function CheckBeforeEdit(ByVal pv_xmlDocument As XmlDocumentEx) As Long
        Return CheckPermition(pv_xmlDocument, gc_ActionEdit)
        'ContextUtil.SetComplete()
        'Return 0
    End Function

    Overridable Function CheckBeforeDelete(ByVal pv_xmlDocument As XmlDocumentEx) As Long
        Return CheckPermition(pv_xmlDocument, gc_ActionDelete)
        'ContextUtil.SetComplete()
        'Return 0
    End Function

    Private Function CheckPermition(ByVal pv_xmlDocument As XmlDocumentEx, ByVal pv_strAction As String) As Long
        Dim v_lngError As Long = ERR_SYSTEM_OK
        Try
            Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes
            Dim v_strTellerID As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTLID), Xml.XmlAttribute).Value)
            Dim v_strBRID As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeBRID), Xml.XmlAttribute).Value)
            Dim v_obj As New DataAccess
            v_obj.NewDBInstance(gc_MODULE_HOST)
            Dim v_strSQL As String
            Dim v_strAuthStr As String
            v_strSQL = "SELECT * FROM cmdauth a WHERE ((a.AUTHID = '" & v_strTellerID & "' AND a.authtype = 'U')" _
                        & " OR (a.AUTHID IN (SELECT grpid FROM tlgrpusers WHERE tlid = '" & v_strTellerID & "') AND a.authtype = 'G'))" _
                        & " And a.deleted = 0 AND a.status = 0 AND a.cmdtype = 'M' AND a.brid='" & v_strBRID _
                        & "' AND a.cmdcode = (SELECT cmdid FROM cmdmenu WHERE objname = '" & ATTR_TABLE & "')"
            Dim v_ds As DataSet
            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

            Dim v_blnOK As Boolean = False
            If v_ds.Tables(0).Rows.Count > 0 Then
                For v_int As Integer = 0 To v_ds.Tables(0).Rows.Count - 1
                    v_strAuthStr = v_ds.Tables(0).Rows(v_int)("CMDALLOW") & v_ds.Tables(0).Rows(v_int)("STRAUTH")
                    Select Case pv_strAction
                        Case gc_ActionAdd
                            If Mid(v_strAuthStr, 3, 1) = "Y" Then
                                v_blnOK = True
                                Exit For
                            End If
                        Case gc_ActionDelete
                            If Mid(v_strAuthStr, 5, 1) = "Y" Then
                                v_blnOK = True
                                Exit For
                            End If
                        Case gc_ActionEdit
                            If Mid(v_strAuthStr, 4, 1) = "Y" Then
                                v_blnOK = True
                                Exit For
                            End If
                        Case gc_ActionInquiry
                            If Mid(v_strAuthStr, 2, 1) = "Y" Then
                                v_blnOK = True
                                Exit For
                            End If
                    End Select
                Next
            Else
                v_lngError = ERR_SYSTEM_START
            End If

            If Not v_blnOK Then
                v_lngError = ERR_SYSTEM_START
            End If

            v_ds.Dispose()
            v_obj.Dispose()

            Return v_lngError
        Catch ex As Exception
            Return ERR_SYSTEM_START
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
