Imports Sats.CommonLibrary
Imports Sats.DataAccessLayer
'Imports System.EnterpriseServices

'<JustInTimeActivation(False), _
'Transaction(TransactionOption.Supported), _
'ObjectPooling(Enabled:=True, MinPoolSize:=30)> _
Public Class TLGROUPS
    Inherits CoreBusiness.objMaster
    Implements CoreBusiness.IMaster

    Public Sub New()
        ATTR_TABLE = "TLGROUPS"
        'ContextUtil.SetComplete()
    End Sub

    Public Function Add(ByRef pv_xmlDocument As XmlDocumentEx) As Long Implements CoreBusiness.IMaster.Add
        Dim v_lngErrCode As Long

        Try
            v_lngErrCode = CoreAdd(pv_xmlDocument)
            If v_lngErrCode <> 0 Then
                Dim v_strErrorSource, v_strErrorMessage As String

                v_strErrorSource = "TLGROUPS.Add"
                v_strErrorMessage = String.Empty

                'LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
                '             & "Error code: " & v_lngErrCode.ToString() & vbNewLine _
                '             & "Error message: " & v_strErrorMessage, EventLogEntryType.Information)
                BuildXMLErrorException(pv_xmlDocument, v_strErrorSource, v_lngErrCode, v_strErrorMessage)
            End If

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

    Public Function Adhoc(ByRef pv_xmlDocument As XmlDocumentEx) As Long Implements CoreBusiness.IMaster.Adhoc
        Dim v_lngErrCode As Long = ERR_SYSTEM_OK
        Dim v_strFuncName As String
        Dim v_strObjMsg As String

        Try
            Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes
            v_strFuncName = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeFUNCNAME), Xml.XmlAttribute).Value)

            v_strObjMsg = pv_xmlDocument.InnerXml
            Select Case Trim(v_strFuncName)
                Case "AddUsersToGroup"
                    v_lngErrCode = AddUsersToGroup(pv_xmlDocument)
                Case "MIAssign"
                    v_lngErrCode = MIAssign(pv_xmlDocument)
                Case "CAAssign"
                    v_lngErrCode = CAAssign(pv_xmlDocument)
                Case "StockAssign"
                    v_lngErrCode = StockAssign(pv_xmlDocument)
                Case "BrIDAssign"
                    v_lngErrCode = BrIDAssign(pv_xmlDocument)
                Case "FunctionAssignment"
                    v_lngErrCode = FunctionAssignment(pv_xmlDocument)
                Case "ReportAssignment"
                    v_lngErrCode = ReportAssignment(pv_xmlDocument)
                Case "TransactionAssignment"
                    v_lngErrCode = TransactionAssignment(pv_xmlDocument)
                Case "UpdateSysCalendar"
                    v_lngErrCode = UpdateSystemCalendar(pv_xmlDocument)
                Case "UpdateSysVarValue"
                    v_lngErrCode = UpdateSysVarValue(pv_xmlDocument)
                Case "InitAllSysCalendar"
                    v_lngErrCode = InitAllSysCalendar(pv_xmlDocument)

            End Select
            pv_xmlDocument.LoadXml(v_strObjMsg)

            'ContextUtil.SetComplete()
            Return v_lngErrCode
        Catch ex As Exception
            'ContextUtil.SetAbort()
        End Try
    End Function

    Public Function Delete(ByRef pv_xmlDocument As XmlDocumentEx) As Long Implements CoreBusiness.IMaster.Delete
        Dim v_lngErrCode As Long
        Dim v_obj As DataAccess = Nothing
        Try
            v_lngErrCode = CoreDelete(pv_xmlDocument)
            If v_lngErrCode <> 0 Then
                Dim v_strErrorSource, v_strErrorMessage As String

                v_strErrorSource = "TLGROUPS.Delete"
                v_strErrorMessage = String.Empty

                'LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
                '             & "Error code: " & v_lngErrCode.ToString() & vbNewLine _
                '             & "Error message: " & v_strErrorMessage, EventLogEntryType.Information)
                BuildXMLErrorException(pv_xmlDocument, v_strErrorSource, v_lngErrCode, v_strErrorMessage)
            Else

                Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes
                Dim v_strClause, v_strLocal As String

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


                If v_strLocal = "Y" Then
                    v_obj = New DataAccess
                ElseIf v_strLocal = "N" Then
                    v_obj = New DataAccess
                    v_obj.NewDBInstance(gc_MODULE_HOST)
                End If

                Dim v_arrClause(), v_strGrpidDel As String
                v_arrClause = v_strClause.Split("=")
                v_strGrpidDel = v_arrClause(1)
                v_strGrpidDel = Replace(v_strGrpidDel, "'", "").Trim

                'Delete all from CMDAUTH and TLAUTH
                Dim v_strDelSQL As String
                'Delete from CMDAUTH
                v_strDelSQL = "DELETE FROM CMDAUTH WHERE AUTHID = '" & v_strGrpidDel & "' AND AUTHTYPE = 'G'"
                v_obj.ExecuteNonQuery(CommandType.Text, v_strDelSQL)
                'Delete from TLAUTH
                v_strDelSQL = "DELETE FROM TLAUTH WHERE AUTHID = '" & v_strGrpidDel & "' AND AUTHTYPE = 'G'"
                v_obj.ExecuteNonQuery(CommandType.Text, v_strDelSQL)
            End If

            'ContextUtil.SetComplete()
            Return v_lngErrCode
        Catch ex As Exception
            'ContextUtil.SetAbort()
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            '             & "Error code: System error!" & vbNewLine _
            '             & "Error message: " & ex.Message, EventLogEntryType.Error)
            Throw ex
        Finally
            If Not v_obj Is Nothing Then
                v_obj.Dispose()
            End If
        End Try
    End Function

    Public Function Edit(ByRef pv_xmlDocument As XmlDocumentEx) As Long Implements CoreBusiness.IMaster.Edit
        Dim v_lngErrCode As Long

        Try
            v_lngErrCode = CoreEdit(pv_xmlDocument)
            If v_lngErrCode <> 0 Then
                Dim v_strErrorSource, v_strErrorMessage As String

                v_strErrorSource = "TLGROUPS.Edit"
                v_strErrorMessage = String.Empty

                'LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
                '             & "Error code: " & v_lngErrCode.ToString() & vbNewLine _
                '             & "Error message: " & v_strErrorMessage, EventLogEntryType.Information)
                BuildXMLErrorException(pv_xmlDocument, v_strErrorSource, v_lngErrCode, v_strErrorMessage)
            End If

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

    Public Function Inquiry(ByRef pv_xmlDocument As XmlDocumentEx) As Long Implements CoreBusiness.IMaster.Inquiry
        Dim v_lngErrCode As Long

        Try
            v_lngErrCode = CoreInquiry(pv_xmlDocument)
            'If v_lngErrCode <> 0 Then
            '    Dim v_strErrorSource, v_strErrorMessage As String

            '    v_strErrorSource = "TLGROUPS.Inquiry"
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

#Region " Overrides functions "
    Overrides Function CheckBeforeAdd(ByVal pv_xmlDocument As XmlDocumentEx) As Long
        Dim v_ds As DataSet
        Dim v_nodeList As Xml.XmlNodeList

        Try
            Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes
            Dim v_strLocal As String, v_strGrpId As String = "", v_strGrpName As String
            Dim v_strFLDNAME, v_strVALUE As String
            Dim v_strSQL As String

            If Not (v_attrColl.GetNamedItem(gc_AtributeLOCAL) Is Nothing) Then
                v_strLocal = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeLOCAL), Xml.XmlAttribute).Value)
            Else
                v_strLocal = String.Empty
            End If

            v_nodeList = pv_xmlDocument.SelectNodes("/ObjectMessage/ObjData")
            For i As Integer = 0 To v_nodeList.Item(0).ChildNodes.Count - 1
                With v_nodeList.Item(0).ChildNodes(i)
                    v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                    v_strVALUE = .InnerText.ToString()

                    Select Case Trim(v_strFLDNAME)
                        Case "GRPID"
                            v_strGrpId = Trim(v_strVALUE)
                        Case "GRPNAME"
                            v_strGrpName = Trim(v_strVALUE)
                    End Select
                End With
            Next

            Dim v_obj As New DataAccess
            'If v_strLocal = "Y" Then
            '    v_obj.NewDBInstance(gc_MODULE_BDS)
            'ElseIf v_strLocal = "N" Then
            v_obj.NewDBInstance(gc_MODULE_HOST)
            'End If

            'Kiểm tra GRPID không được trùng
            v_strSQL = "SELECT COUNT(GRPID) FROM " & ATTR_TABLE & " WHERE GRPID = '" & v_strGrpId & "'"
            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            If v_ds.Tables(0).Rows.Count = 1 Then
                If v_ds.Tables(0).Rows(0)(0) > 0 Then
                    Return ERR_SA_GRPID_DUPLICATED
                End If
            End If
            'Kiểm tra GRPNAME không được trùng
            'v_strSQL = "SELECT COUNT(GRPNAME) FROM " & ATTR_TABLE & " WHERE GRPNAME = '" & v_strGrpName & "'  AND  CONCAT('0', DELETED) <> '01'"
            'v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            'If v_ds.Tables(0).Rows.Count = 1 Then
            '    If v_ds.Tables(0).Rows(0)(0) > 0 Then
            '        Return ERR_SA_GRPNAME_DUPLICATED
            '    End If
            'End If

            If Not (v_ds Is Nothing) Then
                v_ds.Dispose()
            End If

            'ContextUtil.SetComplete()
            Return 0
        Catch ex As Exception
            'ContextUtil.SetAbort()
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            '             & "Error code: System error!" & vbNewLine _
            '             & "Error message: " & ex.Message, EventLogEntryType.Error)
            Throw ex
        End Try
    End Function

    Overrides Function CheckBeforeEdit(ByVal pv_xmlDocument As XmlDocumentEx) As Long
        'ContextUtil.SetComplete()
        Return ERR_SYSTEM_OK
    End Function

    Overrides Function CheckBeforeDelete(ByVal pv_xmlDocument As XmlDocumentEx) As Long
        Try
            Dim v_ds As DataSet
            Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes
            Dim v_strClause As String = ""
            Dim v_strLocal As String
            Dim v_strSQL As String
            Dim v_strAutoid As String
            'Dim v_strFLDNAME, v_strFLDTYPE, v_strVALUE As String

            If Not (v_attrColl.GetNamedItem(gc_AtributeAUTOID) Is Nothing) Then
                v_strAutoid = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeAUTOID), Xml.XmlAttribute).Value)
            Else
                v_strAutoid = String.Empty
            End If

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

            Dim v_obj As DataAccess = Nothing
            If v_strLocal = "Y" Then
                v_obj = New DataAccess
            ElseIf v_strLocal = "N" Then
                v_obj = New DataAccess
                v_obj.NewDBInstance(gc_MODULE_HOST)
            End If

            'Không cho phép xoá nhóm người sử dụng không rỗng
            v_strSQL = "SELECT COUNT(GRPID) FROM TLGRPUSERS WHERE GRPID " & Mid(v_strClause, InStr(v_strClause, "="))
            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            If v_ds.Tables(0).Rows.Count = 1 Then
                If v_ds.Tables(0).Rows(0)(0) > 0 Then
                    Return ERR_SA_GRP_HAS_CHILD
                End If
            End If

            ''Không cho phép xoá nhóm NSD đã được phân quyền
            v_strSQL = "SELECT COUNT(AUTHID) FROM CMDAUTH WHERE AUTHTYPE = 'G' AND AUTHID " & Mid(v_strClause, InStr(v_strClause, "="))
            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            If v_ds.Tables(0).Rows.Count = 1 Then
                If v_ds.Tables(0).Rows(0)(0) > 0 Then
                    Return 1
                End If
            End If

            ''Không cho phép xoá nhóm NSD đã được cho phép thực hiện giao dịch
            'v_strSQL = "SELECT COUNT(AUTHID) FROM TLAUTH WHERE AUTHTYPE = 'G' AND AUTHID " & Mid(v_strClause, InStr(v_strClause, "="))
            'v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            'If v_ds.Tables(0).Rows.Count = 1 Then
            '    If v_ds.Tables(0).Rows(0)(0) > 0 Then
            '        Return ERR_SA_GRP_HAS_TLAUTH
            '    End If
            'End If

            If Not (v_ds Is Nothing) Then
                v_ds.Dispose()
            End If

            'ContextUtil.SetComplete()
            Return 0
        Catch ex As Exception
            'ContextUtil.SetAbort()
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            '             & "Error code: System error!" & vbNewLine _
            '             & "Error message: " & ex.Message, EventLogEntryType.Error)
            Throw ex
        End Try
    End Function


#End Region

#Region " Private methods "
    ''---------------------------------------------------------------------''
    ''-- + Mục đích: Ghi dữ liệu định nghĩa người dùng cho nhóm vào CSDL --'' 
    ''-- + Đầu vào: pv_xmlDocument: XmlDocument chứa dữ liệu cần thiết   --''
    ''-- + Đầu ra: N/A                                                   --''
    ''-- + Tác giả: Nguyễn Minh Thơ                                      --''
    ''-- + Ghi chú: N/A                                                  --''
    ''---------------------------------------------------------------------''
    Private Function AddUsersToGroup(ByRef pv_xmlDocument As XmlDocumentEx) As Long
        Dim v_obj As DataAccess = Nothing
        Try
            Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes
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

            Dim v_strGrpid As String
            Dim v_arrClause() As String
            Dim v_arrTlid() As String
            Dim v_arrBrid() As String

            v_arrClause = v_strClause.Split("#")
            v_strGrpid = v_arrClause(0)
            v_arrTlid = v_arrClause(1).Split("|")
            v_arrBrid = v_arrClause(2).Split("|")

            'Inquiry data

            If v_strLocal = "Y" Then
                v_obj = New DataAccess
            ElseIf v_strLocal = "N" Then
                v_obj = New DataAccess
                v_obj.NewDBInstance(gc_MODULE_HOST)
            End If

            'Delete data in database
            Dim v_strCmdDelSQL As String
            v_strCmdDelSQL = "DELETE FROM TLGRPUSERS WHERE GRPID = '" & v_strGrpid & "'"
            v_obj.ExecuteNonQuery(CommandType.Text, v_strCmdDelSQL)

            'Update database
            For i As Integer = 0 To v_arrTlid.Length - 2
                Dim v_strCmdInsertSQL As String
                v_strCmdInsertSQL = "INSERT INTO TLGRPUSERS(AUTOID, GRPID, BRID, TLID, DESCRIPTION) " _
                                    & "VALUES(SEQ_TLGRPUSERS.NEXTVAL, '" & v_strGrpid & "', '" & v_arrBrid(i) & "', '" & v_arrTlid(i) & "', '')"
                v_obj.ExecuteNonQuery(CommandType.Text, v_strCmdInsertSQL)
            Next

            Return ERR_SYSTEM_OK
        Catch ex As Exception
            'ContextUtil.SetAbort()
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            '                         & "Error code: System error!" & vbNewLine _
            '                         & "Error message: " & ex.Message, EventLogEntryType.Error)
            Throw ex
        Finally
            If Not v_obj Is Nothing Then
                v_obj.Dispose()
            End If
        End Try
    End Function


    ''---------------------------------------------------------------------''
    ''-- + Mục đích: Phan quyen TVLK                                     --'' 
    ''-- + Đầu vào: pv_xmlDocument: XmlDocument chứa dữ liệu cần thiết   --''
    ''-- + Đầu ra: N/A                                                   --''
    ''-- + Tác giả: Nguyễn Minh Thơ                                      --''
    ''-- + Ghi chú: N/A                                                  --''
    ''---------------------------------------------------------------------''
    Private Function MIAssign(ByRef pv_xmlDocument As XmlDocumentEx) As Long
        Dim v_obj As New DataAccess
        Try
            Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes
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

            Dim v_strGrpid As String
            Dim v_arrClause() As String
            Dim v_arrTlid() As String

            v_arrClause = v_strClause.Split("#")
            v_strGrpid = v_arrClause(0)
            v_arrTlid = v_arrClause(1).Split("|")

            'Inquiry data

            'If v_strLocal = "Y" Then
            '    v_obj.NewDBInstance(gc_MODULE_BDS)
            'ElseIf v_strLocal = "N" Then
            v_obj.NewDBInstance(gc_MODULE_HOST)
            'End If

            'Delete data in database
            Dim v_strCmdDelSQL As String
            v_strCmdDelSQL = "DELETE FROM TLMEMAUTH WHERE AUTHID = '" & v_strGrpid & "' AND AUTHTYPE='G'"
            v_obj.ExecuteNonQuery(CommandType.Text, v_strCmdDelSQL)

            'Update database
            For i As Integer = 0 To v_arrTlid.Length - 2
                Dim v_strCmdInsertSQL As String
                v_strCmdInsertSQL = "INSERT INTO TLMEMAUTH(AUTOID, AUTHID, AUTHTYPE, MICODE) " _
                                    & "VALUES(SEQ_TLMEMAUTH.NEXTVAL, '" & v_strGrpid & "', 'G', '" & v_arrTlid(i) & "')"
                v_obj.ExecuteNonQuery(CommandType.Text, v_strCmdInsertSQL)
            Next

            Return ERR_SYSTEM_OK
        Catch ex As Exception
            'ContextUtil.SetAbort()
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            '                         & "Error code: System error!" & vbNewLine _
            '                         & "Error message: " & ex.Message, EventLogEntryType.Error)
            'BuildXMLErrorException(pv_xmlDocument, ex.Source, 1, ex.Message)
            Throw ex
        Finally
            If Not v_obj Is Nothing Then
                v_obj.Dispose()
            End If
        End Try
    End Function
    ''---------------------------------------------------------------------''
    ''-- + Mục đích: Phan quyen ký số                                    --'' 
    ''-- + Đầu vào: pv_xmlDocument: XmlDocument chứa dữ liệu cần thiết   --''
    ''-- + Đầu ra: N/A                                                   --''
    ''-- + Tác giả: BằngPV                                               --''
    ''-- + Ghi chú: N/A                                                  --''
    ''---------------------------------------------------------------------''
    Private Function CAAssign(ByRef pv_xmlDocument As XmlDocumentEx) As Long
        Dim v_obj As New DataAccess
        Try
            Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes
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

            Dim v_strGrpid As String
            Dim v_arrClause() As String
            Dim v_arrTlid() As String

            v_arrClause = v_strClause.Split("#")
            v_strGrpid = v_arrClause(0)
            v_arrTlid = v_arrClause(1).Split("|")

            'Inquiry data

            'If v_strLocal = "Y" Then
            '    v_obj.NewDBInstance(gc_MODULE_BDS)
            'ElseIf v_strLocal = "N" Then
            v_obj.NewDBInstance(gc_MODULE_HOST)
            'End If

            'Delete data in database
            Dim v_strCmdDelSQL As String
            v_strCmdDelSQL = "DELETE FROM TLCAAUTH WHERE AUTHID = '" & v_strGrpid & "' AND AUTHTYPE='G'"
            v_obj.ExecuteNonQuery(CommandType.Text, v_strCmdDelSQL)

            'Update database
            For i As Integer = 0 To v_arrTlid.Length - 2
                Dim v_strCmdInsertSQL As String
                v_strCmdInsertSQL = "INSERT INTO TLCAAUTH(AUTOID, AUTHID, AUTHTYPE, TLTXCD) " _
                                    & "VALUES(SEQ_TLCAAUTH.NEXTVAL, '" & v_strGrpid & "', 'G', '" & v_arrTlid(i) & "')"
                v_obj.ExecuteNonQuery(CommandType.Text, v_strCmdInsertSQL)
            Next

            Return ERR_SYSTEM_OK
        Catch ex As Exception
            'ContextUtil.SetAbort()
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            '                         & "Error code: System error!" & vbNewLine _
            '                         & "Error message: " & ex.Message, EventLogEntryType.Error)
            'BuildXMLErrorException(pv_xmlDocument, ex.Source, 1, ex.Message)
            Throw ex
        Finally
            If Not v_obj Is Nothing Then
                v_obj.Dispose()
            End If
        End Try
    End Function
    ''---------------------------------------------------------------------''
    ''-- + Mục đích: Phan quyen chung khoan                                     --'' 
    ''-- + Đầu vào: pv_xmlDocument: XmlDocument chứa dữ liệu cần thiết   --''
    ''-- + Đầu ra: N/A                                                   --''
    ''-- + Tác giả: Nguyễn Minh Thơ                                      --''
    ''-- + Ghi chú: N/A                                                  --''
    ''---------------------------------------------------------------------''
    Private Function StockAssign(ByRef pv_xmlDocument As XmlDocumentEx) As Long
        Dim v_obj As New DataAccess
        Try
            Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes
            Dim v_strClause As String
            Dim v_strLocal As String
            Dim v_strAutoId As String
            Dim v_strBrId As String

            If Not (v_attrColl.GetNamedItem(gc_AtributeBRID) Is Nothing) Then
                v_strBrId = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeBRID), Xml.XmlAttribute).Value)
            Else
                v_strBrId = String.Empty
            End If

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

            Dim v_strGrpid As String
            Dim v_arrClause() As String
            Dim v_arrTlid() As String

            v_arrClause = v_strClause.Split("#")
            v_strGrpid = v_arrClause(0)
            v_arrTlid = v_arrClause(1).Split("|")

            'Inquiry data

            'If v_strLocal = "Y" Then
            '    v_obj.NewDBInstance(gc_MODULE_BDS)
            'ElseIf v_strLocal = "N" Then
            v_obj.NewDBInstance(gc_MODULE_HOST)
            'End If

            'Delete data in database
            Dim v_strCmdDelSQL As String
            v_strCmdDelSQL = "DELETE FROM TLSTOCKAUTH WHERE AUTHID = '" & v_strGrpid & "' AND AUTHTYPE='G' and sicode in (select sicode from rgsi where deleted=0 and status=0 and brid ='" & v_strBrId & "')"
            v_obj.ExecuteNonQuery(CommandType.Text, v_strCmdDelSQL)

            'Update database
            For i As Integer = 0 To v_arrTlid.Length - 2
                Dim v_strCmdInsertSQL As String
                v_strCmdInsertSQL = "INSERT INTO TLSTOCKAUTH(AUTOID, AUTHID, AUTHTYPE, SICODE) " _
                                    & "VALUES(SEQ_TLSTOCKAUTH.NEXTVAL, '" & v_strGrpid & "', 'G', '" & v_arrTlid(i) & "')"
                v_obj.ExecuteNonQuery(CommandType.Text, v_strCmdInsertSQL)
            Next

            Return ERR_SYSTEM_OK
        Catch ex As Exception
            'ContextUtil.SetAbort()
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            '                         & "Error code: System error!" & vbNewLine _
            '                         & "Error message: " & ex.Message, EventLogEntryType.Error)
            'BuildXMLErrorException(pv_xmlDocument, ex.Source, 1, ex.Message)
            Throw ex
        Finally
            If Not v_obj Is Nothing Then
                v_obj.Dispose()
            End If
        End Try
    End Function

    ''---------------------------------------------------------------------''
    ''-- + Mục đích: Phân quyền chi nhánh                                     --'' 
    ''-- + Đầu vào: pv_xmlDocument: XmlDocument chứa dữ liệu cần thiết   --''
    ''-- + Đầu ra: N/A                                                   --''
    ''-- + Tác giả: BangPV                                      --''
    ''-- + Ghi chú: N/A                                                  --''
    ''---------------------------------------------------------------------''
    Private Function BrIDAssign(ByRef pv_xmlDocument As XmlDocumentEx) As Long
        Dim v_obj As New DataAccess
        Try
            Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes
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

            Dim v_strGrpid As String
            Dim v_arrClause() As String
            Dim v_arrTlid() As String

            v_arrClause = v_strClause.Split("#")
            v_strGrpid = v_arrClause(0)
            v_arrTlid = v_arrClause(1).Split("|")

            'Inquiry data

            'If v_strLocal = "Y" Then
            '    v_obj.NewDBInstance(gc_MODULE_BDS)
            'ElseIf v_strLocal = "N" Then
            v_obj.NewDBInstance(gc_MODULE_HOST)
            ' End If

            'Delete data in database
            Dim v_strCmdDelSQL As String
            v_strCmdDelSQL = "DELETE FROM TLBRIDAUTH WHERE AUTHID = '" & v_strGrpid & "' AND AUTHTYPE='G'"
            v_obj.ExecuteNonQuery(CommandType.Text, v_strCmdDelSQL)

            'Update database
            For i As Integer = 0 To v_arrTlid.Length - 2
                Dim v_strCmdInsertSQL As String
                v_strCmdInsertSQL = "INSERT INTO TLBRIDAUTH(AUTOID, AUTHID, AUTHTYPE, BRID) " _
                                    & "VALUES(SEQ_TLBRIDAUTH.NEXTVAL, '" & v_strGrpid & "', 'G', '" & v_arrTlid(i) & "')"
                v_obj.ExecuteNonQuery(CommandType.Text, v_strCmdInsertSQL)
            Next

            Return ERR_SYSTEM_OK
        Catch ex As Exception
            'ContextUtil.SetAbort()
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            '                         & "Error code: System error!" & vbNewLine _
            '                         & "Error message: " & ex.Message, EventLogEntryType.Error)
            'BuildXMLErrorException(pv_xmlDocument, ex.Source, 1, ex.Message)
            Throw ex
        Finally
            If Not v_obj Is Nothing Then
                v_obj.Dispose()
            End If
        End Try
    End Function

    ''------------------------------------------------------------------------''
    ''-- + Mục đích: Cập nhật các dữ liệu phân quyền chức năng vào CSDL     --''
    ''-- + Đầu vào: pv_xmlDocument: XmlDocument chứa các dữ liệu phân quyền --''
    ''-- + Đầu ra: N/A                                                      --''
    ''-- + Tác giả: Nguyễn Minh Thơ                                         --''
    ''-- + Ghi chú: N/A                                                     --''
    ''------------------------------------------------------------------------''
    Private Function FunctionAssignment(ByRef pv_xmlDocument As XmlDocumentEx) As Long
        Dim v_obj As DataAccess = Nothing
        Try
            Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes
            Dim v_strClause As String
            Dim v_strLocal As String
            Dim v_strBrid As String
            Dim v_strGBrid As String

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
            ''bangpv: lấy mã chi nhánh
            If Not (v_attrColl.GetNamedItem(gc_AtributeBRID) Is Nothing) Then
                v_strGBrid = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeBRID), Xml.XmlAttribute).Value)
            Else
                v_strGBrid = String.Empty
            End If

            'Get Group information and strAuth
            Dim v_arrGrp(), v_arrStrAuth(), v_strGroupId, v_strLast As String
            v_arrGrp = v_strClause.Split("$")
            v_strGroupId = v_arrGrp(0)
            v_arrStrAuth = v_arrGrp(1).Split("#")
            'v_arrUsersId = v_arrGrp(2).Split("#")

            If v_strLocal = "Y" Then
                v_obj = New DataAccess
            ElseIf v_strLocal = "N" Then
                v_obj = New DataAccess
                v_obj.NewDBInstance(gc_MODULE_HOST)
            End If

            'Delete data in database
            Dim v_strCmdDelSQL As String
            'Delete Group information
            v_strCmdDelSQL = "DELETE FROM CMDAUTH WHERE AUTHID = '" & v_strGroupId & "' AND AUTHTYPE = 'G' AND CMDTYPE = 'M' AND BRID ='" & v_strGBrid & "'"
            v_obj.ExecuteNonQuery(CommandType.Text, v_strCmdDelSQL)

            'Insert data to CMDAUTH table
            Dim v_strCmdInsertSQL As String
            Dim v_strCMDCODE, v_strCMDALLOW, v_strAuth, v_arrMenuKey() As String

            'Insert group's auth info
            For i As Integer = 0 To v_arrStrAuth.Length - 2
                v_arrMenuKey = v_arrStrAuth(i).Split("|")
                v_strCMDCODE = v_arrMenuKey(0)
                v_strAuth = v_arrMenuKey(1)
                v_strLast = v_arrMenuKey(2)
                'bangpv
                v_strBrid = v_arrMenuKey(3)
                If v_strBrid = v_strGBrid Then
                    'end bangpv 
                    v_strCMDALLOW = Mid(v_strAuth, 1, 1)
                    v_strAuth = Mid(v_strAuth, 2, 6)

                    If v_strLast = "N" Then
                        v_strCmdInsertSQL = "SELECT CMDCODE FROM CMDAUTH WHERE CMDCODE = '" & Left(v_strCMDCODE, Len(v_strCMDCODE) - 4) & "' AND AUTHID='" & v_strGroupId & "' AND BRID='" & v_strBrid & "'"
                        Dim v_ds As DataSet = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strCmdInsertSQL)
                        If v_ds.Tables(0).Rows.Count <= 0 Then
                            'Insert data to CMDAUTH table
                            'bangpv
                            'them brid
                            v_strCmdInsertSQL = "INSERT INTO CMDAUTH(AUTOID, AUTHTYPE, AUTHID, CMDTYPE, CMDCODE, CMDALLOW, STRAUTH,brid) " _
                                                            & "VALUES(SEQ_CMDAUTH.NEXTVAL, 'G', '" & v_strGroupId & "', 'M', '" & Left(v_strCMDCODE, Len(v_strCMDCODE) - 4) & "', '" _
                                                            & v_strCMDALLOW & "', '" & v_strAuth & "', '" & v_strBrid & "')"
                            'them brid
                            v_obj.ExecuteNonQuery(CommandType.Text, v_strCmdInsertSQL)
                        End If
                    Else
                        'Insert data to CMDAUTH table
                        v_strCmdInsertSQL = "INSERT INTO CMDAUTH(AUTOID, AUTHTYPE, AUTHID, CMDTYPE, CMDCODE, CMDALLOW, STRAUTH, BRID) " _
                                        & "VALUES(SEQ_CMDAUTH.NEXTVAL, 'G', '" & v_strGroupId & "', 'M', '" & Left(v_strCMDCODE, Len(v_strCMDCODE) - 4) & "', '" _
                                        & v_strCMDALLOW & "', '" & v_strAuth & "','" & v_strBrid & "')"
                        v_obj.ExecuteNonQuery(CommandType.Text, v_strCmdInsertSQL)
                    End If
                End If
            Next

            Return ERR_SYSTEM_OK
        Catch ex As Exception
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            '                         & "Error code: System error!" & vbNewLine _
            '                         & "Error message: " & ex.Message, EventLogEntryType.Error)
            'BuildXMLErrorException(pv_xmlDocument, ex.Source, 1, ex.Message)
            Throw ex
        Finally
            If Not v_obj Is Nothing Then
                v_obj.Dispose()
            End If
            GC.Collect()
        End Try
    End Function

    ''------------------------------------------------------------------------''
    ''-- + Mục đích: Cập nhật các dữ liệu phân quyền báo cáo vào CSDL       --''
    ''-- + Đầu vào: pv_xmlDocument: XmlDocument chứa các dữ liệu phân quyền --''
    ''-- + Đầu ra: N/A                                                      --''
    ''-- + Tác giả: Nguyễn Minh Thơ                                         --''
    ''-- + Ghi chú: N/A                                                     --''
    ''------------------------------------------------------------------------''
    Private Function ReportAssignment(ByRef pv_xmlDocument As XmlDocumentEx) As Long
        Dim v_obj As New DataAccess
        Try
            Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes
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

            'Get Group information and strAuth
            Dim v_arrGrp(), v_arrStrAuth(), v_strGroupId As String
            v_arrGrp = v_strClause.Split("$")
            v_strGroupId = v_arrGrp(0)
            v_arrStrAuth = v_arrGrp(1).Split("#")

            'Inquiry data

            'If v_strLocal = "Y" Then
            '    v_obj.NewDBInstance(gc_MODULE_BDS)
            'ElseIf v_strLocal = "N" Then
            v_obj.NewDBInstance(gc_MODULE_HOST)
            'End If

            'Delete data in database
            Dim v_strCmdDelSQL As String
            v_strCmdDelSQL = "DELETE FROM CMDAUTH WHERE AUTHID = '" & v_strGroupId & "' AND AUTHTYPE = 'G' AND CMDTYPE = 'R'"
            v_obj.ExecuteNonQuery(CommandType.Text, v_strCmdDelSQL)

            'Insert data to CMDAUTH table
            Dim v_strCmdInsertSQL As String
            Dim v_strCMDCODE, v_strCMDALLOW, v_strAuth, v_arrMenuKey() As String
            'Insert group's auth info
            For i As Integer = 0 To v_arrStrAuth.Length - 2
                v_arrMenuKey = v_arrStrAuth(i).Split("|")
                v_strCMDCODE = v_arrMenuKey(0)
                v_strAuth = v_arrMenuKey(1)
                v_strCMDALLOW = Mid(v_strAuth, 1, 1)
                v_strAuth = Mid(v_strAuth, 2, 4)

                'Insert to database
                v_strCmdInsertSQL = "INSERT INTO CMDAUTH(AUTOID, AUTHTYPE, AUTHID, CMDTYPE, CMDCODE, CMDALLOW, STRAUTH) " _
                                                & "VALUES(SEQ_CMDAUTH.NEXTVAL, 'G', '" & v_strGroupId & "', 'R', '" & v_strCMDCODE & "', '" & v_strCMDALLOW & "', '" & v_strAuth & "')"
                v_obj.ExecuteNonQuery(CommandType.Text, v_strCmdInsertSQL)

            Next

            Return ERR_SYSTEM_OK
        Catch ex As Exception
            'ContextUtil.SetAbort()
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            '                         & "Error code: System error!" & vbNewLine _
            '                         & "Error message: " & ex.Message, EventLogEntryType.Error)
            'BuildXMLErrorException(pv_xmlDocument, ex.Source, 1, ex.Message)
            Throw ex
        Finally
            If Not v_obj Is Nothing Then
                v_obj.Dispose()
            End If
            GC.Collect()
        End Try
    End Function

    ''------------------------------------------------------------------------''
    ''-- + Mục đích: Cập nhật các dữ liệu phân quyền giao dịch vào CSDL     --''
    ''-- + Đầu vào: pv_xmlDocument: XmlDocument chứa các dữ liệu phân quyền --''
    ''-- + Đầu ra: N/A                                                      --''
    ''-- + Tác giả: Nguyễn Minh Thơ                                         --''
    ''-- + Ghi chú: N/A                                                     --''
    ''------------------------------------------------------------------------''
    Private Function TransactionAssignment(ByRef pv_xmlDocument As XmlDocumentEx) As Long
        Dim v_obj As New DataAccess
        Try
            Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes
            Dim v_strClause As String
            Dim v_strLocal As String
            'bangpv
            Dim v_strBrID As String
            Dim v_strGBrid As String

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
            'bangpv
            If Not (v_attrColl.GetNamedItem(gc_AtributeLOCAL) Is Nothing) Then
                v_strGBrid = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeBRID), Xml.XmlAttribute).Value)
            Else
                v_strGBrid = String.Empty
            End If

            'Get Group info and CMDAUTH's string and TLAUTH's string
            Dim v_arrGrp() As String
            Dim v_strCmdauthString, v_strTlauthString, v_strGroupId As String
            v_arrGrp = v_strClause.Split("$")
            v_strGroupId = v_arrGrp(0)
            v_strCmdauthString = v_arrGrp(2)
            v_strTlauthString = v_arrGrp(1)

            'Inquiry data

            'If v_strLocal = "Y" Then
            '    v_obj.NewDBInstance(gc_MODULE_BDS)
            'ElseIf v_strLocal = "N" Then
            v_obj.NewDBInstance(gc_MODULE_HOST)
            'End If

            'Delete data in database
            Dim v_strSQL As String
            'Thêm điều kiện chi nhánh
            v_strSQL = "DELETE FROM CMDAUTH WHERE AUTHID = '" & v_strGroupId & "' AND AUTHTYPE = 'G' AND CMDTYPE = 'T' AND BRID='" & v_strGBrid & "'"
            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

            'Delete data from TLAUTH table
            v_strSQL = "DELETE FROM TLAUTH WHERE AUTHID = '" & v_strGroupId & "' AND AUTHTYPE = 'G' AND BRID ='" & v_strGBrid & "'"
            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

            'Insert data to CMDAUTH table
            Dim v_arrCMDAUTH() As String
            Dim v_strCmdInsertSQL As String
            Dim v_arrMenuKey() As String
            Dim v_strTLTXCD, v_strCMDID, v_strLAST, v_strCMDALLOW, v_strAuth As String

            v_arrCMDAUTH = v_strCmdauthString.Split("#")

            'Insert group' informations
            For i As Integer = 0 To v_arrCMDAUTH.Length - 2
                'Get CMDID and CMDALLOW
                v_arrMenuKey = v_arrCMDAUTH(i).Split("|")
                v_strCMDID = v_arrMenuKey(0)
                v_strLAST = v_arrMenuKey(2)
                'v_strCMDALLOW = v_arrMenuKey(1)
                'bangpv 
                v_strBrID = v_arrMenuKey(3)
                If v_strBrID = v_strGBrid Then
                    'end bangpv 

                    v_strAuth = v_arrMenuKey(1)

                    If Len(Trim(v_strAuth)) = 1 Then
                        v_strCMDALLOW = v_strAuth
                        v_strAuth = String.Empty
                    Else
                        v_strCMDALLOW = Mid(v_strAuth, 1, 1)
                        v_strAuth = Mid(v_strAuth, 2, 3)
                    End If
                    'Neu v_strLAST = N
                    'Kiem tra xem da ton tai trong DB hay ko neu ko ton tai thi Insert
                    'bangpv: thêm điều kiện chi nhánh
                    If v_strLAST = "N" Then
                        v_strCmdInsertSQL = "SELECT CMDCODE FROM CMDAUTH WHERE CMDCODE = '" & Left(v_strCMDID, Len(v_strCMDID) - 4) & "' AND AUTHID='" & v_strGroupId & "' and brid ='" & v_strBrID & "'"
                        Dim v_ds As DataSet = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strCmdInsertSQL)
                        If v_ds.Tables(0).Rows.Count <= 0 Then
                            'Insert data to CMDAUTH table
                            v_strCmdInsertSQL = "INSERT INTO CMDAUTH(AUTOID, AUTHTYPE, AUTHID, CMDTYPE, CMDCODE, CMDALLOW, STRAUTH,BRID) " _
                                                            & "VALUES(SEQ_CMDAUTH.NEXTVAL, 'G', '" & v_strGroupId & "', 'T', '" & Left(v_strCMDID, Len(v_strCMDID) - 4) & "', '" _
                                                            & v_strCMDALLOW & "', '" & v_strAuth & "', '" & v_strBrID & "')"
                            v_obj.ExecuteNonQuery(CommandType.Text, v_strCmdInsertSQL)
                        End If
                    Else
                        'Insert data to CMDAUTH table
                        v_strCmdInsertSQL = "INSERT INTO CMDAUTH(AUTOID, AUTHTYPE, AUTHID, CMDTYPE, CMDCODE, CMDALLOW, STRAUTH, BRID) " _
                                                        & "VALUES(SEQ_CMDAUTH.NEXTVAL, 'G', '" & v_strGroupId & "', 'T', '" & Left(v_strCMDID, Len(v_strCMDID) - 4) & "', '" _
                                                        & v_strCMDALLOW & "', '" & v_strAuth & "', '" & v_strBrID & "')"
                        v_obj.ExecuteNonQuery(CommandType.Text, v_strCmdInsertSQL)

                    End If
                End If
                'end bangpv
            Next

            'Insert data to TLAUTH table
            Dim v_arrTLAUTH() As String
            Dim v_arrTransString() As String
            Dim v_strTLTYPE, v_strLIMIT As String
            Dim v_dblLIMIT As Double

            v_arrTLAUTH = v_strTlauthString.Split("#")
            For i As Integer = 0 To v_arrTLAUTH.Length - 2
                v_arrTransString = v_arrTLAUTH(i).Split("|")
                v_strTLTXCD = v_arrTransString(0)
                v_strTLTYPE = v_arrTransString(1)
                v_strLIMIT = v_arrTransString(2)
                v_strBrID = v_arrTransString(3)
                If v_strBrID = v_strGBrid Then
                    'v_strCMDID = v_arrTransString(3)
                    If v_strLIMIT = "" Then
                        v_strLIMIT = "0"
                    End If

                    v_dblLIMIT = CDbl(v_strLIMIT)

                    'Insert data to TLAUTH table
                    v_strSQL = "INSERT INTO TLAUTH(AUTOID, AUTHTYPE, AUTHID, TLTXCD, TLTYPE, TLLIMIT, BRID) " _
                                        & "VALUES(SEQ_TLAUTH.NEXTVAL, 'G', '" & v_strGroupId & "','" _
                                        & Left(v_strTLTXCD, Len(v_strTLTXCD) - 4) & "', '" & v_strTLTYPE & "', '" & v_dblLIMIT & "', '" & v_strBrID & "')"
                    v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                End If
            Next
            v_obj = Nothing
            Return ERR_SYSTEM_OK
        Catch ex As Exception
            'ContextUtil.SetAbort()
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            '                         & "Error code: System error!" & vbNewLine _
            '                         & "Error message: " & ex.Message, EventLogEntryType.Error)
            'BuildXMLErrorException(pv_xmlDocument, ex.Source, 1, ex.Message)
            Throw ex
        Finally
            If Not v_obj Is Nothing Then
                v_obj.Dispose()
            End If
            GC.Collect()
        End Try
    End Function
    ''------------------------------------------------------------------------''
    ''-- + Mục đích: Cập nhật các dữ liệu ngày nghỉ của hệ thống vào CSDL   --''
    ''-- + Đầu vào: pv_xmlDocument: XmlDocument chứa các dữ liệu            --''
    ''-- + Bảng dữ liệu: SYSCLDR                                             --''
    ''-- + Đầu ra: N/A                                                      --''
    ''-- + Tác giả: Nguyễn Mạnh Hà                                          --''
    ''-- + Ghi chú: N/A                                                     --''
    ''-- + Ngày tạo: 10/02/2009                                             --''
    ''------------------------------------------------------------------------''
    Private Function UpdateSystemCalendar(ByRef pv_xmlDocument As XmlDocumentEx) As Long
        Dim v_obj As New DataAccess
        Try
            Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes
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

            Dim v_arrData() As String
            Dim v_strDataInsert As String = "", v_strDataDelete As String = "", v_strBrId As String = ""
            Dim v_arrDataInsert() As String, v_arrDataDelete() As String

            v_arrData = v_strClause.Split("$")


            'If v_strLocal = "Y" Then
            '    v_obj.NewDBInstance(gc_MODULE_BDS)
            'ElseIf v_strLocal = "N" Then
            v_obj.NewDBInstance(gc_MODULE_HOST)
            'End If

            Dim v_strSql As String = ""

            For j As Integer = 0 To v_arrData.Length - 1
                Dim v_strBrData As String = v_arrData(j)
                Dim v_arrBrData() As String = v_strBrData.Split("#")

                v_strDataDelete = v_arrBrData(0)
                v_strDataInsert = v_arrBrData(1)
                v_strBrId = v_arrBrData(2)

                'Delete dữ liệu trong database
                If Trim(v_strDataDelete) <> "" Then
                    v_arrDataDelete = v_strDataDelete.Split("|")
                    For i As Integer = 0 To v_arrDataDelete.Length - 1
                        v_strSql = "UPDATE SYSCLDR SET DELETED = 1 WHERE TO_CHAR(SBDATE, 'dd/MM/yyyy') = '" & Trim(v_arrDataDelete(i)) & "'"
                        v_obj.ExecuteNonQuery(CommandType.Text, v_strSql)
                    Next
                End If
                'Insert dữ liệu vào database
                If Trim(v_strDataInsert) <> "" Then
                    v_arrDataInsert = v_strDataInsert.Split("|")
                    For i As Integer = 0 To v_arrDataInsert.Length - 1
                        v_strSql = "INSERT INTO SYSCLDR(AUTOID, SBDATE, STATUS, DELETED, BRID) " _
                                 & "VALUES(SEQ_SYSCLDR.NEXTVAL, TO_DATE('" & Trim(v_arrDataInsert(i)) & "','dd/MM/yyyy'), 0, 0, '" & Trim(v_strBrId) & "')"
                        v_obj.ExecuteNonQuery(CommandType.Text, v_strSql)
                    Next
                End If

            Next

            Return ERR_SYSTEM_OK
        Catch ex As Exception
            'ContextUtil.SetAbort()
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            '                         & "Error code: System error!" & vbNewLine _
            '                         & "Error message: " & ex.Message, EventLogEntryType.Error)
            'BuildXMLErrorException(pv_xmlDocument, ex.Source, 1, ex.Message)
            Throw ex
        Finally
            If Not v_obj Is Nothing Then
                v_obj.Dispose()
            End If
            GC.Collect()
        End Try
    End Function

    ''------------------------------------------------------------------------''
    ''-- + Mục đích: Khởi tạo dữ liệu ngày nghỉ cho cả năm của hệ thống vào CSDL   --''
    ''-- + Đầu vào: pv_xmlDocument: XmlDocument chứa các dữ liệu            --''
    ''-- + Bảng dữ liệu: SYSCLDR                                             --''
    ''-- + Đầu ra: N/A                                                      --''
    ''-- + Tác giả: Nguyễn Mạnh Hà                                          --''
    ''-- + Ghi chú: N/A                                                     --''
    ''-- + Ngày tạo: 10/02/2009                                             --''
    ''------------------------------------------------------------------------''
    Function InitAllSysCalendar(ByRef pv_xmlDocument As XmlDocumentEx) As Long
        Dim v_obj As New DataAccess
        Try
            Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes
            Dim v_strClause As String
            Dim v_strBRID As String
            Dim v_strLocal As String

            'Lấy câu dữ liệu gửi lên từ Client
            If Not (v_attrColl.GetNamedItem(gc_AtributeCLAUSE) Is Nothing) Then
                v_strClause = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCLAUSE), Xml.XmlAttribute).Value)
            Else
                v_strClause = String.Empty
            End If

            If Not (v_attrColl.GetNamedItem(gc_AtributeBRID) Is Nothing) Then
                v_strBRID = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeBRID), Xml.XmlAttribute).Value)
            Else
                v_strBRID = String.Empty
            End If

            If Not (v_attrColl.GetNamedItem(gc_AtributeLOCAL) Is Nothing) Then
                v_strLocal = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeLOCAL), Xml.XmlAttribute).Value)
            Else
                v_strLocal = String.Empty
            End If

            'Tạp đối tượng database access

            'If v_strLocal = "Y" Then
            '    v_obj.NewDBInstance(gc_MODULE_BDS)
            'ElseIf v_strLocal = "N" Then
            v_obj.NewDBInstance(gc_MODULE_HOST)
            'End If

            Dim v_strSQL As String = ""
            Dim v_dSaturday As Date
            Dim v_dSunday As Date
            Dim v_intFirstSaturday As Integer
            Dim v_dFirstDate As Date = CDate("01/01/" & v_strClause)
            For i As Integer = 1 To 7
                v_dFirstDate = CDate(DateAdd(DateInterval.Day, 1, v_dFirstDate))
                If v_dFirstDate.DayOfWeek = DayOfWeek.Saturday Then
                    v_dSaturday = v_dFirstDate
                End If
            Next

            'Xóa toàn bộ ngày cũ của năm
            v_strSQL = "DELETE FROM SYSCLDR WHERE TO_CHAR(SBDATE,'yyyy') = '" & Trim(v_strClause) & "' AND BRID = '" & Trim(v_strBRID) & "' AND DELETED = 0 AND STATUS = 0"
            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

            'Insert tất cả các thứ 7 và chủ nhật của năm vào Database
            v_dSunday = CDate(DateAdd(DateInterval.Day, 1, v_dSaturday))
            While v_dSunday.Year.ToString = v_strClause
                v_strSQL = "INSERT INTO SYSCLDR(AUTOID, SBDATE, STATUS, DELETED, BRID) " _
                                & "VALUES(SEQ_SYSCLDR.NEXTVAL, TO_DATE('" & CStr(Format(v_dSaturday, "dd/MM/yyyy")) & "','dd/MM/yyyy'), 0, 0, '" & v_strBRID & "')"
                v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

                v_strSQL = "INSERT INTO SYSCLDR(AUTOID, SBDATE, STATUS, DELETED, BRID) " _
                                 & "VALUES(SEQ_SYSCLDR.NEXTVAL, TO_DATE('" & CStr(Format(v_dSunday, "dd/MM/yyyy")) & "','dd/MM/yyyy'), 0, 0, '" & v_strBRID & "')"
                v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                'v_strSQL = "INSERT INTO SYSCLDR(AUTOID, SBDATE, STATUS, DELETED, BRID) " _
                '                & "VALUES(SEQ_SYSCLDR.NEXTVAL, TO_DATE('" & CStr(Format(v_dSaturday, "dd/MM/yyyy")) & "','dd/MM/yyyy'), 0, 0, '0001')"
                'v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                'v_strSQL = "INSERT INTO SYSCLDR(AUTOID, SBDATE, STATUS, DELETED, BRID) " _
                '                 & "VALUES(SEQ_SYSCLDR.NEXTVAL, TO_DATE('" & CStr(Format(v_dSaturday, "dd/MM/yyyy")) & "','dd/MM/yyyy'), 0, 0, '0002')"
                'v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                'v_strSQL = "INSERT INTO SYSCLDR(AUTOID, SBDATE, STATUS, DELETED, BRID) " _
                '                 & "VALUES(SEQ_SYSCLDR.NEXTVAL, TO_DATE('" & CStr(Format(v_dSaturday, "dd/MM/yyyy")) & "','dd/MM/yyyy'), 0, 0, '0003')"
                'v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

                'v_strSQL = "INSERT INTO SYSCLDR(AUTOID, SBDATE, STATUS, DELETED, BRID) " _
                '                 & "VALUES(SEQ_SYSCLDR.NEXTVAL, TO_DATE('" & CStr(Format(v_dSaturday, "dd/MM/yyyy")) & "','dd/MM/yyyy'), 0, 0, '0004')"
                'v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

                'v_strSQL = "INSERT INTO SYSCLDR(AUTOID, SBDATE, STATUS, DELETED, BRID) " _
                '                 & "VALUES(SEQ_SYSCLDR.NEXTVAL, TO_DATE('" & CStr(Format(v_dSaturday, "dd/MM/yyyy")) & "','dd/MM/yyyy'), 0, 0, '0005')"
                'v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                'bangpv
                'v_strSQL = "INSERT INTO SYSCLDR(AUTOID, SBDATE, STATUS, DELETED, BRID) " _
                '                 & "VALUES(SEQ_SYSCLDR.NEXTVAL, TO_DATE('" & CStr(Format(v_dSaturday, "dd/MM/yyyy")) & "','dd/MM/yyyy'), 0, 0, '0006')"
                'v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                'Thanglv9-25/12/2013-add them thi truong 0007
                'v_strSQL = "INSERT INTO SYSCLDR(AUTOID, SBDATE, STATUS, DELETED, BRID) " _
                '                 & "VALUES(SEQ_SYSCLDR.NEXTVAL, TO_DATE('" & CStr(Format(v_dSaturday, "dd/MM/yyyy")) & "','dd/MM/yyyy'), 0, 0, '0007')"
                'v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                'end


                'v_strSQL = "INSERT INTO SYSCLDR(AUTOID, SBDATE, STATUS, DELETED, BRID) " _
                '                 & "VALUES(SEQ_SYSCLDR.NEXTVAL, TO_DATE('" & CStr(Format(v_dSunday, "dd/MM/yyyy")) & "','dd/MM/yyyy'), 0, 0, '0001')"
                'v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                'v_strSQL = "INSERT INTO SYSCLDR(AUTOID, SBDATE, STATUS, DELETED, BRID) " _
                '                 & "VALUES(SEQ_SYSCLDR.NEXTVAL, TO_DATE('" & CStr(Format(v_dSunday, "dd/MM/yyyy")) & "','dd/MM/yyyy'), 0, 0, '0002')"
                'v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                'v_strSQL = "INSERT INTO SYSCLDR(AUTOID, SBDATE, STATUS, DELETED, BRID) " _
                '                 & "VALUES(SEQ_SYSCLDR.NEXTVAL, TO_DATE('" & CStr(Format(v_dSunday, "dd/MM/yyyy")) & "','dd/MM/yyyy'), 0, 0, '0003')"
                'v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                'v_strSQL = "INSERT INTO SYSCLDR(AUTOID, SBDATE, STATUS, DELETED, BRID) " _
                '                 & "VALUES(SEQ_SYSCLDR.NEXTVAL, TO_DATE('" & CStr(Format(v_dSunday, "dd/MM/yyyy")) & "','dd/MM/yyyy'), 0, 0, '0004')"
                'v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                'v_strSQL = "INSERT INTO SYSCLDR(AUTOID, SBDATE, STATUS, DELETED, BRID) " _
                '                 & "VALUES(SEQ_SYSCLDR.NEXTVAL, TO_DATE('" & CStr(Format(v_dSunday, "dd/MM/yyyy")) & "','dd/MM/yyyy'), 0, 0, '0005')"
                'v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

                'v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                'v_strSQL = "INSERT INTO SYSCLDR(AUTOID, SBDATE, STATUS, DELETED, BRID) " _
                '                 & "VALUES(SEQ_SYSCLDR.NEXTVAL, TO_DATE('" & CStr(Format(v_dSunday, "dd/MM/yyyy")) & "','dd/MM/yyyy'), 0, 0, '0006')"
                'v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                'Thanglv9-25/12/2013-add them thi truong 0007
                'v_strSQL = "INSERT INTO SYSCLDR(AUTOID, SBDATE, STATUS, DELETED, BRID) " _
                '                 & "VALUES(SEQ_SYSCLDR.NEXTVAL, TO_DATE('" & CStr(Format(v_dSunday, "dd/MM/yyyy")) & "','dd/MM/yyyy'), 0, 0, '0007')"
                'v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                'end

                v_dSaturday = CDate(DateAdd(DateInterval.Day, 7, v_dSaturday))
                v_dSunday = CDate(DateAdd(DateInterval.Day, 1, v_dSaturday))
            End While
            Return ERR_SYSTEM_OK
        Catch ex As Exception
            Throw ex
        Finally
            If Not v_obj Is Nothing Then
                v_obj.Dispose()
            End If
            GC.Collect()
        End Try
    End Function
    ''------------------------------------------------------------------------''
    ''-- + Mục đích: Cập nhật các dữ liệu tham số của hệ thống vào CSDL   --''
    ''-- + Đầu vào: pv_xmlDocument: XmlDocument chứa các dữ liệu            --''
    ''-- + Bảng dữ liệu: SYSVAR                                             --''
    ''-- + Đầu ra: N/A                                                      --''
    ''-- + Tác giả: Nguyễn Mạnh Hà                                          --''
    ''-- + Ghi chú: N/A                                                     --''
    ''-- + Ngày tạo: 10/02/2009                                             --''
    ''------------------------------------------------------------------------''
    Function UpdateSysVarValue(ByRef pv_xmlDocument As XmlDocumentEx) As Long
        Dim v_obj As New DataAccess
        Try
            Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes
            Dim v_strClause As String
            Dim v_strLocal As String

            'Lấy câu dữ liệu gửi lên từ Client
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

            Dim v_arrData() As String

            'Phân tách thành mảng dữ liệu
            v_arrData = v_strClause.Split("|")

            'Tạp đối tượng database access

            'If v_strLocal = "Y" Then
            '    v_obj.NewDBInstance(gc_MODULE_BDS)
            'ElseIf v_strLocal = "N" Then
            v_obj.NewDBInstance(gc_MODULE_HOST)
            'End If

            'Update dữ liệu vào CSDL
            For i As Integer = 0 To v_arrData.Length - 1
                If v_arrData(i) <> "" Then
                    Dim v_strSql As String = "UPDATE SYSVAR SET "
                    Dim v_arrSysVar() As String = Trim(v_arrData(i)).Split("=")
                    v_strSql &= "VARVALUE = " & v_arrSysVar(1)
                    v_strSql &= " WHERE VARNAME = '" & v_arrSysVar(0) & "' AND STATUS = 0 AND DELETED = 0"
                    v_obj.ExecuteNonQuery(CommandType.Text, v_strSql)
                End If
            Next
            Return ERR_SYSTEM_OK
        Catch ex As Exception
            'ContextUtil.SetAbort()
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            '                         & "Error code: System error!" & vbNewLine _
            '                         & "Error message: " & ex.Message, EventLogEntryType.Error)
            'BuildXMLErrorException(pv_xmlDocument, ex.Source, 1, ex.Message)
            Throw ex
        Finally
            If Not v_obj Is Nothing Then
                v_obj.Dispose()
            End If
        End Try
    End Function
#End Region

End Class

Public Class TLAUTH
    Inherits CoreBusiness.objMaster
    Implements CoreBusiness.IMaster

    Public Sub New()
        ATTR_TABLE = "TLAUTH"
        'ContextUtil.SetComplete()
    End Sub

    Public Function Add(ByRef pv_xmlDocument As XmlDocumentEx) As Long Implements CoreBusiness.IMaster.Add
       
    End Function

    Public Function Adhoc(ByRef pv_xmlDocument As XmlDocumentEx) As Long Implements CoreBusiness.IMaster.Adhoc
        
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

            '    v_strErrorSource = "TLAUTH.Inquiry"
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
 
End Class

Public Class CMDAUTH
    Inherits CoreBusiness.objMaster
    Implements CoreBusiness.IMaster

    Public Sub New()
        ATTR_TABLE = "CMDAUTH"
        'ContextUtil.SetComplete()
    End Sub

    Public Function Add(ByRef pv_xmlDocument As XmlDocumentEx) As Long Implements CoreBusiness.IMaster.Add
        
    End Function

    Public Function Adhoc(ByRef pv_xmlDocument As XmlDocumentEx) As Long Implements CoreBusiness.IMaster.Adhoc
        
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

            '    v_strErrorSource = "CMDAUTH.Inquiry"
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

End Class
