Imports Sats.CommonLibrary
Imports Sats.DataAccessLayer
'Imports System.EnterpriseServices

'<JustInTimeActivation(False), _
'Transaction(TransactionOption.Supported), _
'ObjectPooling(Enabled:=True, MinPoolSize:=30)> _
Public Class TLPROFILES
    Inherits CoreBusiness.objMaster
    Implements CoreBusiness.IMaster

    Public Sub New()
        ATTR_TABLE = "TLPROFILES"
        'ContextUtil.SetComplete()
    End Sub

    Public Function Add(ByRef pv_xmlDocument As XmlDocumentEx) As Long Implements CoreBusiness.IMaster.Add
        Dim v_lngErrCode As Long

        Try
            'v_lngErrCode = CoreAdd(pv_xmlDocument)
            v_lngErrCode = AddNewUser(pv_xmlDocument)
            If v_lngErrCode <> 0 Then
                Dim v_strErrorSource, v_strErrorMessage As String

                v_strErrorSource = "TLPROFILES.Add"
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
                Case "AddNewUser"
                    v_lngErrCode = AddNewUser(pv_xmlDocument)
                Case "EditUser"
                    v_lngErrCode = EditUser(pv_xmlDocument)
                Case "FunctionAssignment"
                    v_lngErrCode = FunctionAssignment(pv_xmlDocument)
                Case "ReportAssignment"
                    v_lngErrCode = ReportAssignment(pv_xmlDocument)
                Case "TransactionAssignment"
                    v_lngErrCode = TransactionAssignment(pv_xmlDocument)
                Case "StockAssign"
                    v_lngErrCode = StockAssign(pv_xmlDocument)
                Case "MIAssign"
                    v_lngErrCode = MIAssign(pv_xmlDocument)
                Case "BrIDAssign"
                    v_lngErrCode = BrIDAssign(pv_xmlDocument)
            End Select
            pv_xmlDocument.LoadXml(v_strObjMsg)

            Return v_lngErrCode
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function Delete(ByRef pv_xmlDocument As XmlDocumentEx) As Long Implements CoreBusiness.IMaster.Delete
        Dim v_lngErrCode As Long

        Try
            v_lngErrCode = CoreDelete(pv_xmlDocument)
            If v_lngErrCode <> 0 Then
                Dim v_strErrorSource, v_strErrorMessage As String

                v_strErrorSource = "TLPROFILES.Delete"
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

                Dim v_obj As DataAccess = Nothing
                If v_strLocal = "Y" Then
                    v_obj = New DataAccess
                ElseIf v_strLocal = "N" Then
                    v_obj = New DataAccess
                    v_obj.NewDBInstance(gc_MODULE_HOST)
                End If

                Dim v_arrClause(), v_strTlidDel As String
                v_arrClause = v_strClause.Split("=")
                v_strTlidDel = v_arrClause(1)
                v_strTlidDel = Replace(v_strTlidDel, "'", "").Trim

                'Delete all from CMDAUTH and TLAUTH
                Dim v_strDelSQL As String
                'Delete from CMDAUTH
                v_strDelSQL = "DELETE FROM CMDAUTH WHERE AUTHID = '" & v_strTlidDel & "' AND AUTHTYPE = 'U'"
                v_obj.ExecuteNonQuery(CommandType.Text, v_strDelSQL)
                'Delete from TLAUTH
                v_strDelSQL = "DELETE FROM TLAUTH WHERE AUTHID = '" & v_strTlidDel & "' AND AUTHTYPE = 'U'"
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
        End Try
    End Function

    Public Function Edit(ByRef pv_xmlDocument As XmlDocumentEx) As Long Implements CoreBusiness.IMaster.Edit
        Dim v_lngErrCode As Long

        Try
            v_lngErrCode = CoreEdit(pv_xmlDocument)
            If v_lngErrCode <> 0 Then
                Dim v_strErrorSource, v_strErrorMessage As String

                v_strErrorSource = "TLPROFILES.Edit"
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

            '    v_strErrorSource = "TLPROFILES.Inquiry"
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
            Dim v_strLocal As String, v_strTlId As String = "", v_strTlname As String
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
                        Case "TLID"
                            v_strTlId = Trim(v_strVALUE)
                        Case "TLNAME"
                            v_strTlname = Trim(v_strVALUE)
                    End Select
                End With
            Next

            Dim v_obj As DataAccess = Nothing
            If v_strLocal = "Y" Then
                v_obj = New DataAccess
            ElseIf v_strLocal = "N" Then
                v_obj = New DataAccess
                v_obj.NewDBInstance(gc_MODULE_HOST)
            End If

            'Kiểm tra TLID không được trùng
            v_strSQL = "SELECT COUNT(TLID) FROM " & ATTR_TABLE & " WHERE TLID = '" & v_strTlId & "'"
            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            If v_ds.Tables(0).Rows.Count = 1 Then
                If v_ds.Tables(0).Rows(0)(0) > 0 Then
                    Return ERR_SA_TLID_DUPLICATED
                End If
            End If
            'Kiểm tra TLNAME không được trùng
            v_strSQL = "SELECT COUNT(TLNAME) FROM " & ATTR_TABLE & " WHERE TLNAME = '" & v_strTlname & "' AND DELETED = 0"
            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            If v_ds.Tables(0).Rows.Count = 1 Then
                If v_ds.Tables(0).Rows(0)(0) > 0 Then
                    Return ERR_SA_TLNAME_DUPLICATED
                End If
            End If

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
        Return 0
    End Function

    Overrides Function CheckBeforeDelete(ByVal pv_xmlDocument As XmlDocumentEx) As Long
        Try
            Dim v_ds As DataSet
            Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes
            Dim v_strClause, v_strCurrentTlid As String
            Dim v_strLocal As String
            Dim v_strSQL As String
            Dim v_strAutoid As String

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
            If Not (v_attrColl.GetNamedItem(gc_AtributeTLID) Is Nothing) Then
                v_strCurrentTlid = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTLID), Xml.XmlAttribute).Value)
            Else
                v_strCurrentTlid = String.Empty
            End If

            Dim v_obj As DataAccess = Nothing
            If v_strLocal = "Y" Then
                v_obj = New DataAccess
            ElseIf v_strLocal = "N" Then
                v_obj = New DataAccess
                v_obj.NewDBInstance(gc_MODULE_HOST)
            End If

            'Không cho phép xóa chính NSD đang sử dụng hệ thống
            Dim v_arrClause(), v_strTlidDel As String
            v_arrClause = v_strClause.Split("=")
            v_strTlidDel = v_arrClause(1)
            v_strTlidDel = Replace(v_strTlidDel, "'", "").Trim
            If v_strTlidDel = v_strCurrentTlid Then
                Return ERR_SA_TL_IN_SYS
            End If

            'Không cho phép xoá NSD đã có trong 1 nhóm cụ thể
            v_strSQL = "SELECT COUNT(TLID) FROM TLGRPUSERS WHERE TLID " & Mid(v_strClause, InStr(v_strClause, "="))
            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            If v_ds.Tables(0).Rows.Count = 1 Then
                If v_ds.Tables(0).Rows(0)(0) > 0 Then
                    Return ERR_SA_TL_HAS_CHILD
                End If
            End If

            'Không cho phép xoá NSD đã có trong danh sách thực hiện GD trong ngày
            v_strSQL = "SELECT COUNT(TLID) FROM TLLOG WHERE TLID " & Mid(v_strClause, InStr(v_strClause, "="))
            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            If v_ds.Tables(0).Rows.Count = 1 Then
                If v_ds.Tables(0).Rows(0)(0) > 0 Then
                    Return ERR_SA_TL_HAS_TLAUTH
                End If
            End If

            'Không cho phép xoá NSD đã có trong danh sách thực hiện GD
            v_strSQL = "SELECT COUNT(TLID) FROM TLLOGALL WHERE TLID " & Mid(v_strClause, InStr(v_strClause, "="))
            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            If v_ds.Tables(0).Rows.Count = 1 Then
                If v_ds.Tables(0).Rows(0)(0) > 0 Then
                    Return ERR_SA_TL_HAS_TLAUTH
                End If
            End If

            ''Không cho phép xoá NSD đã có trong DS phân quyền
            'v_strSQL = "SELECT COUNT(AUTHID) FROM CMDAUTH WHERE AUTHTYPE = 'U' AND AUTHID " & Mid(v_strClause, InStr(v_strClause, "="))
            'v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            'If v_ds.Tables(0).Rows.Count = 1 Then
            '    If v_ds.Tables(0).Rows(0)(0) > 0 Then
            '        Return ERR_SA_TL_HAS_CMDAUTH
            '    End If
            'End If

            ''Không cho phép xoá NSD đã có trong DS thực hiện giao dịch
            'v_strSQL = "SELECT COUNT(AUTHID) FROM TLAUTH WHERE AUTHTYPE = 'U' AND AUTHID " & Mid(v_strClause, InStr(v_strClause, "="))
            'v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            'If v_ds.Tables(0).Rows.Count = 1 Then
            '    If v_ds.Tables(0).Rows(0)(0) > 0 Then
            '        Return ERR_SA_TL_HAS_TLAUTH
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

    Private Function AddNewUser(ByRef pv_xmlDocument As XmlDocumentEx) As Long
        Try
            Dim v_lngErrorCode As Long

            v_lngErrorCode = CheckBeforeAdd(pv_xmlDocument)
            If v_lngErrorCode <> 0 Then
                Return v_lngErrorCode
                Exit Function
            End If

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

            'Inquiry data
            Dim v_obj As New DataAccess
            'If v_strLocal = "Y" Then
            '    v_obj.NewDBInstance(gc_MODULE_BDS)
            'ElseIf v_strLocal = "N" Then
            v_obj.NewDBInstance(gc_MODULE_HOST)
            'End If

            Dim v_strSQL As String = "INSERT INTO " & ATTR_TABLE
            Dim v_strListOfFields As String = vbNullString
            Dim v_strListOfValues As String = vbNullString
            Dim v_strSignature As String = String.Empty
            Dim v_strCustID As String = String.Empty


            Dim v_decID As Decimal
            'bangpv: Rao lai de lay TLID trung 
            Dim i As Integer
            Dim v_strSQLTLID As String
            Dim v_dsTLID As DataSet
            For i = 1000 To 9999
                v_strSQLTLID = "SELECT tlid FROM TLPROFILES WHERE TLID =LPAD(" & i & ",4,'0')"
                v_dsTLID = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTLID)
                If v_dsTLID.Tables(0).Rows.Count = 0 Then
                    v_decID = i
                    Exit For
                End If
            Next
            'v_decID = v_obj.GetIDValue(ATTR_TABLE)
            v_strListOfFields = "(AUTOID"
            v_strListOfValues = "(" & v_decID


            'Cập nhật vào CSDL
            Dim v_nodeList As Xml.XmlNodeList ' , i As Integer
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
                        v_strListOfFields = v_strListOfFields & "," & v_strFLDNAME
                        If v_strFLDNAME = "TLID" Then
                            v_strListOfValues = v_strListOfValues & ",'" & v_decID & "'"
                        Else
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

            Return 0
        Catch ex As Exception
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            '               & "Error code: System error!" & vbNewLine _
            '               & "Error message: " & ex.Message, EventLogEntryType.Error)
            Throw ex
        End Try
    End Function

    Private Function EditUser(ByRef pv_xmlDocument As XmlDocumentEx) As Long
        Try
            Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes
            Dim v_strClause As String
            Dim v_strLocal As String
            Dim v_strTellerId As String

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

            If Not (v_attrColl.GetNamedItem(gc_AtributeTLID) Is Nothing) Then
                v_strTellerId = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTLID), Xml.XmlAttribute).Value)
            Else
                v_strTellerId = String.Empty
            End If

            Dim v_strTlid As String = "", v_strBrid As String = "", v_strTlgroup As String = ""
            Dim v_strTlname As String = "", v_strTlpassword As String = "", v_strFullname As String = ""
            Dim v_strTltitle As String = "", v_strTlprn As String = "", v_strTldescription As String = "", v_strTlType As String = ""
            Dim v_intTlLev As Integer
            Dim v_arrTlProfile() As String = Nothing

            If v_strClause <> String.Empty Then
                v_arrTlProfile = v_strClause.Split("|")
                If v_arrTlProfile.Length = 11 Then
                    v_strTlid = v_arrTlProfile(0)
                    v_strBrid = v_arrTlProfile(1)
                    v_strTlgroup = v_arrTlProfile(2)
                    v_strTlname = v_arrTlProfile(3)
                    v_strFullname = v_arrTlProfile(4)
                    v_strTltitle = v_arrTlProfile(5)
                    v_strTlprn = v_arrTlProfile(6)
                    v_strTldescription = v_arrTlProfile(7)
                    v_strTlpassword = v_arrTlProfile(8)
                    v_intTlLev = CInt(v_arrTlProfile(9))
                    v_strTlType = v_arrTlProfile(10)
                End If
            End If

            Dim v_obj As DataAccess = Nothing
            If v_strLocal = "Y" Then
                v_obj = New DataAccess
            ElseIf v_strLocal = "N" Then
                v_obj = New DataAccess
                v_obj.NewDBInstance(gc_MODULE_HOST)
            End If

            'Check before Edit
            'Check to sure that the user to edit is not the current user
            If Trim(v_strTellerId) = Trim(v_strTlid) Then
                Return ERR_SA_TL_EDIT_CURRENT_USR
            End If

            'Add data to database
            Dim v_strSQL As String
            v_strSQL = "UPDATE TLPROFILES SET TLNAME = '" & v_strTlname & "', TLFULLNAME = '" & v_strFullname & "', TLLEV = '" & v_intTlLev & "', BRID = '" & v_strBrid & "', TLTITLE = '" & v_strTltitle & "', " _
                                           & "TLPRN = '" & v_strTlprn & "', TLGROUP = '" & v_strTlgroup & "', PIN = '" & v_strTlpassword & "', DESCRIPTION = '" & v_strTldescription & "', TLTYPE = '" & v_strTlType & "' " _
                                           & "WHERE TLID = '" & v_strTlid & "'"
            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

            'Update limit of right of user
            Dim v_strTeller, v_strCashier, v_strOfficer, v_strChecker As String
            If v_strTlType <> String.Empty Then
                v_strTeller = Mid(v_strTlType, 1, 1)
                v_strCashier = Mid(v_strTlType, 2, 1)
                v_strOfficer = Mid(v_strTlType, 3, 1)
                v_strChecker = Mid(v_strTlType, 4, 1)

                'Delete limit of right
                If v_strTeller = "N" Then
                    v_strSQL = "DELETE FROM TLAUTH WHERE AUTHID = '" & v_strTlid & "' AND AUTHTYPE = 'U' AND TLTYPE = 'T'"
                    v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                End If
                If v_strCashier = "N" Then
                    v_strSQL = "DELETE FROM TLAUTH WHERE AUTHID = '" & v_strTlid & "' AND AUTHTYPE = 'U' AND TLTYPE = 'C'"
                    v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                End If
                If v_strOfficer = "N" Then
                    v_strSQL = "DELETE FROM TLAUTH WHERE AUTHID = '" & v_strTlid & "' AND AUTHTYPE = 'U' AND TLTYPE = 'A'"
                    v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                End If
                If v_strChecker = "N" Then
                    v_strSQL = "DELETE FROM TLAUTH WHERE AUTHID = '" & v_strTlid & "' AND AUTHTYPE = 'U' AND TLTYPE = 'R'"
                    v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                End If
            End If

            Return ERR_SYSTEM_OK
        Catch ex As Exception
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            '               & "Error code: System error!" & vbNewLine _
            '               & "Error message: " & ex.Message, EventLogEntryType.Error)
            Throw ex
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
            Dim v_obj As New DataAccess
            'If v_strLocal = "Y" Then
            '    v_obj.NewDBInstance(gc_MODULE_BDS)
            'ElseIf v_strLocal = "N" Then
            v_obj.NewDBInstance(gc_MODULE_HOST)
            'End If

            'Delete data in database
            Dim v_strCmdDelSQL As String
            v_strCmdDelSQL = "DELETE FROM TLMEMAUTH WHERE AUTHID = '" & v_strGrpid & "' AND AUTHTYPE='U'"
            v_obj.ExecuteNonQuery(CommandType.Text, v_strCmdDelSQL)

            'Update database
            For i As Integer = 0 To v_arrTlid.Length - 2
                Dim v_strCmdInsertSQL As String
                v_strCmdInsertSQL = "INSERT INTO TLMEMAUTH(AUTOID, AUTHID, AUTHTYPE, MICODE) " _
                                    & "VALUES(SEQ_TLMEMAUTH.NEXTVAL, '" & v_strGrpid & "', 'U', '" & v_arrTlid(i) & "')"
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
            Dim v_obj As New DataAccess
            'If v_strLocal = "Y" Then
            '    v_obj.NewDBInstance(gc_MODULE_BDS)
            'ElseIf v_strLocal = "N" Then
            v_obj.NewDBInstance(gc_MODULE_HOST)
            'End If

            'Delete data in database
            Dim v_strCmdDelSQL As String
            v_strCmdDelSQL = "DELETE FROM TLSTOCKAUTH WHERE AUTHID = '" & v_strGrpid & "' AND AUTHTYPE='U' and sicode in (select sicode from rgsi where deleted=0 and status=0 and brid ='" & v_strBrId & "')"
            v_obj.ExecuteNonQuery(CommandType.Text, v_strCmdDelSQL)

            'Update database
            For i As Integer = 0 To v_arrTlid.Length - 2
                Dim v_strCmdInsertSQL As String
                v_strCmdInsertSQL = "INSERT INTO TLSTOCKAUTH(AUTOID, AUTHID, AUTHTYPE, SICODE) " _
                                    & "VALUES(SEQ_TLSTOCKAUTH.NEXTVAL, '" & v_strGrpid & "', 'U', '" & v_arrTlid(i) & "')"
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
        End Try
    End Function

    ''--------------------------------------------------------------------------''
    ''-- + Mục đích: Phân quyền chi nhánh                                     --'' 
    ''-- + Đầu vào: pv_xmlDocument: XmlDocument chứa dữ liệu cần thiết        --''
    ''-- + Đầu ra: N/A                                                        --''
    ''-- + Tác giả: BằngPV                                                    --''
    ''-- + Ghi chú: N/A                                                       --''
    ''--------------------------------------------------------------------------''
    Private Function BrIDAssign(ByRef pv_xmlDocument As XmlDocumentEx) As Long
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
            Dim v_obj As New DataAccess
            'If v_strLocal = "Y" Then
            '    v_obj.NewDBInstance(gc_MODULE_BDS)
            'ElseIf v_strLocal = "N" Then
            v_obj.NewDBInstance(gc_MODULE_HOST)
            'End If

            'Delete data in database
            Dim v_strCmdDelSQL As String
            v_strCmdDelSQL = "DELETE FROM TLBRIDAUTH WHERE AUTHID = '" & v_strGrpid & "' AND AUTHTYPE='U'"
            v_obj.ExecuteNonQuery(CommandType.Text, v_strCmdDelSQL)

            'Update database
            For i As Integer = 0 To v_arrTlid.Length - 2
                Dim v_strCmdInsertSQL As String
                v_strCmdInsertSQL = "INSERT INTO TLBRIDAUTH(AUTOID, AUTHID, AUTHTYPE, BRID) " _
                                    & "VALUES(SEQ_TLBRIDAUTH.NEXTVAL, '" & v_strGrpid & "', 'U', '" & v_arrTlid(i) & "')"
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
        Try
            Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes
            Dim v_strClause As String
            Dim v_strLocal As String
            Dim v_strTellerId As String
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

            If Not (v_attrColl.GetNamedItem(gc_AtributeTLID) Is Nothing) Then
                v_strTellerId = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTLID), Xml.XmlAttribute).Value)
            Else
                v_strTellerId = String.Empty
            End If
            'bangpv: lấy mã chi nhánh
            If Not (v_attrColl.GetNamedItem(gc_AtributeBRID) Is Nothing) Then
                v_strGBrid = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeBRID), Xml.XmlAttribute).Value)
            Else
                v_strGBrid = String.Empty
            End If

            'Get Group information and strAuth
            Dim v_arrGrp(), v_arrStrAuth(), v_strUserId, v_strLast As String
            v_arrGrp = v_strClause.Split("$")
            v_strUserId = v_arrGrp(0)
            v_arrStrAuth = v_arrGrp(1).Split("#")

            Dim v_obj As DataAccess = Nothing
            If v_strLocal = "Y" Then
                v_obj = New DataAccess
            ElseIf v_strLocal = "N" Then
                v_obj = New DataAccess
                v_obj.NewDBInstance(gc_MODULE_HOST)
            End If

            'Delete data in database
            Dim v_strCmdDelSQL As String
            'Delete Group information
            v_strCmdDelSQL = "DELETE FROM CMDAUTH WHERE AUTHID = '" & v_strUserId & "' AND AUTHTYPE = 'U' AND CMDTYPE = 'M'  AND BRID='" & v_strGBrid & "'"
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
                        v_strCmdInsertSQL = "SELECT CMDCODE FROM CMDAUTH WHERE CMDCODE = '" & Left(v_strCMDCODE, Len(v_strCMDCODE) - 4) & "' AND AUTHID='" & v_strUserId & "' AND BRID ='" & v_strBrid & "'"
                        Dim v_ds As DataSet = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strCmdInsertSQL)
                        If v_ds.Tables(0).Rows.Count <= 0 Then
                            'Insert data to CMDAUTH table
                            v_strCmdInsertSQL = "INSERT INTO CMDAUTH(AUTOID, AUTHTYPE, AUTHID, CMDTYPE, CMDCODE, CMDALLOW, STRAUTH,BRID) " _
                                                            & "VALUES(SEQ_CMDAUTH.NEXTVAL, 'U', '" & v_strUserId & "', 'M', '" & Left(v_strCMDCODE, Len(v_strCMDCODE) - 4) & "', '" _
                                                            & v_strCMDALLOW & "', '" & v_strAuth & "', '" & v_strBrid & "')"
                            v_obj.ExecuteNonQuery(CommandType.Text, v_strCmdInsertSQL)
                        End If
                    Else
                        'Insert data to CMDAUTH table
                        v_strCmdInsertSQL = "INSERT INTO CMDAUTH(AUTOID, AUTHTYPE, AUTHID, CMDTYPE, CMDCODE, CMDALLOW, STRAUTH,BRID) " _
                                        & "VALUES(SEQ_CMDAUTH.NEXTVAL, 'U', '" & v_strUserId & "', 'M', '" & Left(v_strCMDCODE, Len(v_strCMDCODE) - 4) & "', '" _
                                        & v_strCMDALLOW & "', '" & v_strAuth & "', '" & v_strBrid & "')"
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
            Dim v_obj As New DataAccess
            'If v_strLocal = "Y" Then
            '    v_obj.NewDBInstance(gc_MODULE_BDS)
            'ElseIf v_strLocal = "N" Then
            v_obj.NewDBInstance(gc_MODULE_HOST)
            'End If

            'Delete data in database
            Dim v_strCmdDelSQL As String
            v_strCmdDelSQL = "DELETE FROM CMDAUTH WHERE AUTHID = '" & v_strGroupId & "' AND AUTHTYPE = 'U' AND CMDTYPE = 'R'"
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
                                                & "VALUES(SEQ_CMDAUTH.NEXTVAL, 'U', '" & v_strGroupId & "', 'R', '" & v_strCMDCODE & "', '" & v_strCMDALLOW & "', '" & v_strAuth & "')"
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
            If Not (v_attrColl.GetNamedItem(gc_AtributeBRID) Is Nothing) Then
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
            Dim v_obj As New DataAccess
            'If v_strLocal = "Y" Then
            '    v_obj.NewDBInstance(gc_MODULE_BDS)
            'ElseIf v_strLocal = "N" Then
            v_obj.NewDBInstance(gc_MODULE_HOST)
            'End If

            'Delete data in database
            Dim v_strSQL As String

            v_strSQL = "DELETE FROM CMDAUTH WHERE AUTHID = '" & v_strGroupId & "' AND AUTHTYPE = 'U' AND CMDTYPE = 'T'  AND BRID ='" & v_strGBrid & "'"
            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

            'Delete data from TLAUTH table
            v_strSQL = "DELETE FROM TLAUTH WHERE AUTHID = '" & v_strGroupId & "' AND AUTHTYPE = 'U' AND BRID ='" & v_strGBrid & "'"
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
                'bangpv
                v_strBrid = v_arrMenuKey(3)
                'v_strCMDALLOW = v_arrMenuKey(1)
                If v_strBrid = v_strGBrid Then
                    v_strAuth = v_arrMenuKey(1)

                    If Len(Trim(v_strAuth)) = 1 Then
                        v_strCMDALLOW = v_strAuth
                        v_strAuth = String.Empty
                    Else
                        v_strCMDALLOW = Mid(v_strAuth, 1, 1)
                        v_strAuth = Mid(v_strAuth, 2, 4)
                    End If
                    'Neu v_strLAST = N
                    'Kiem tra xem da ton tai trong DB hay ko neu ko ton tai thi Insert
                    If v_strLAST = "N" Then
                        v_strCmdInsertSQL = "SELECT CMDCODE FROM CMDAUTH WHERE CMDCODE = '" & Left(v_strCMDID, Len(v_strCMDID) - 4) & "' AND AUTHID='" & v_strGroupId & "' AND BRID='" & v_strBrid & "'"
                        Dim v_ds As DataSet = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strCmdInsertSQL)
                        If v_ds.Tables(0).Rows.Count <= 0 Then
                            'Insert data to CMDAUTH table
                            v_strCmdInsertSQL = "INSERT INTO CMDAUTH(AUTOID, AUTHTYPE, AUTHID, CMDTYPE, CMDCODE, CMDALLOW, STRAUTH, BRID) " _
                                                            & "VALUES(SEQ_CMDAUTH.NEXTVAL, 'U', '" & v_strGroupId & "', 'T', '" & Left(v_strCMDID, Len(v_strCMDID) - 4) _
                                                            & "', '" & v_strCMDALLOW & "', '" & v_strAuth & "', '" & v_strBrid & "')"
                            v_obj.ExecuteNonQuery(CommandType.Text, v_strCmdInsertSQL)
                        End If
                    Else
                        'Insert data to CMDAUTH table
                        v_strCmdInsertSQL = "INSERT INTO CMDAUTH(AUTOID, AUTHTYPE, AUTHID, CMDTYPE, CMDCODE, CMDALLOW, STRAUTH, BRID) " _
                                                        & "VALUES(SEQ_CMDAUTH.NEXTVAL, 'U', '" & v_strGroupId & "', 'T', '" & Left(v_strCMDID, Len(v_strCMDID) - 4) _
                                                        & "', '" & v_strCMDALLOW & "', '" & v_strAuth & "', '" & v_strBrid & "')"
                        v_obj.ExecuteNonQuery(CommandType.Text, v_strCmdInsertSQL)
                    End If
                End If
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
                'bangpv 
                v_strBrid = v_arrTransString(3)
                If v_strBrid = v_strGBrid Then
                    'v_strCMDID = v_arrTransString(3)
                    If v_strLIMIT = "" Then
                        v_strLIMIT = "0"
                    End If

                    v_dblLIMIT = CDbl(v_strLIMIT)

                    'Insert data to TLAUTH table
                    v_strSQL = "INSERT INTO TLAUTH(AUTOID, AUTHTYPE, AUTHID, TLTXCD, TLTYPE, TLLIMIT, BRID ) " _
                                        & "VALUES(SEQ_TLAUTH.NEXTVAL, 'U', '" & v_strGroupId & "','" & Left(v_strTLTXCD, Len(v_strTLTXCD) - 4) _
                                        & "', '" & v_strTLTYPE & "', '" & v_dblLIMIT & "', '" & v_strBrid & "')"
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
        End Try
    End Function

#End Region

End Class
