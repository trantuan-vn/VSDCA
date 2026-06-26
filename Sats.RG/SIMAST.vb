Imports Sats.CommonLibrary
Imports Sats.DataAccessLayer


Public Class RGSI
    Inherits CoreBusiness.objMaster
    Implements CoreBusiness.IMaster

    Public Sub New()
        ATTR_TABLE = "RGSI"
        'ContextUtil.SetComplete()
    End Sub

    Public Function Add(ByRef pv_xmlDocument As XmlDocumentEx) As Long Implements CoreBusiness.IMaster.Add
        Dim v_lngErrCode As Long
        Try
            v_lngErrCode = CoreAddtoLOG(pv_xmlDocument)
            If v_lngErrCode = 0 Then
                v_lngErrCode = CoreAdd(pv_xmlDocument)
                If v_lngErrCode = 0 Then
                    v_lngErrCode = UpdateToRGII(pv_xmlDocument)
                End If
            End If
            If v_lngErrCode <> 0 Then
                Dim v_strErrorSource, v_strErrorMessage As String

                v_strErrorSource = "RGSI.Add"
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

    End Function

    Public Function Delete(ByRef pv_xmlDocument As XmlDocumentEx) As Long Implements CoreBusiness.IMaster.Delete
        Dim v_lngErrCode As Long

        Try
            'v_lngErrCode = CoreDeleteToLogAfterDel(pv_xmlDocument)
            'If v_lngErrCode = 0 Then
            v_lngErrCode = CoreDelete(pv_xmlDocument)
            'End If
            If v_lngErrCode = 0 Then
                v_lngErrCode = CoreDeleteToLogAfterDel(pv_xmlDocument)
            End If
            If v_lngErrCode <> 0 Then
                Dim v_strErrorSource, v_strErrorMessage As String

                v_strErrorSource = "RGSI.Delete"
                v_strErrorMessage = String.Empty

                'LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
                '             & "Error code: " & v_lngErrCode.ToString() & vbNewLine _
                '             & "Error message: " & v_strErrorMessage, EventLogEntryType.Information)
                BuildXMLErrorException(pv_xmlDocument, v_strErrorSource, v_lngErrCode, v_strErrorMessage)
            End If
            'ContextUtil.SetComplete()
            Return v_lngErrCode
        Catch ex As Exception
            Return -1
            'ContextUtil.SetAbort()
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            '             & "Error code: System error!" & vbNewLine _
            '             & "Error message: " & ex.Message, EventLogEntryType.Error)
            LogError.Write("Delete_RGSI - Error source: " & ex.Source & vbNewLine _
                     & "Error code: System error!" & vbNewLine _
                     & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_HOST)
            Throw ex
        End Try
    End Function

    Public Function Edit(ByRef pv_xmlDocument As XmlDocumentEx) As Long Implements CoreBusiness.IMaster.Edit
        Dim v_lngErrCode As Long

        Try
            v_lngErrCode = CoreInSertToLogAfterEdit(pv_xmlDocument)
            'Added by Thanglv9 - 16/12/2012
            If v_lngErrCode = 0 Then
                v_lngErrCode = CoreEdit(pv_xmlDocument)
            End If
            'End
            If v_lngErrCode <> 0 Then
                Dim v_strErrorSource, v_strErrorMessage As String

                v_strErrorSource = "RGSI.Edit"
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

            '    v_strErrorSource = "RGSI.Inquiry"
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
    '********************************************************************
    'Mục đích       	: Kiểm tra dữ liệu trước khi thêm vào dữ liệu vào bảng RGSI
    'Tham số        	: XMLDocumentEx
    'Trả về         	: Int
    'Ngày tạo			: 29/01/2008
    'Người tạo		    : Nguyen Manh Ha
    'Ngày cập nhật  	:<15/12/2008>
    'Người cập nhật 	:<Hanm5>
    '********************************************************************


    Overrides Function CheckBeforeAdd(ByVal pv_xmlDocument As XmlDocumentEx) As Long
        Dim v_ds As DataSet
        Dim v_nodeList As Xml.XmlNodeList
        Try
            Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes
            Dim v_strLocal As String, v_strSICODE As String = ""
            Dim v_strIsCode As String = ""
            Dim v_intIsType As Integer = 0
            Dim v_intType As Integer = 0
            Dim v_intStockType As Integer = 0
            'Add by ThangLV 20170731 - dinh nghia menh gia
            Dim v_Partvalue As Int64 = 0

            'Add by HoaLX3 20230514 - dinh nghia thoi han TP
            Dim v_BondPeriod As Integer = 0

            'end
            Dim v_strFldName, v_strValue As String
            Dim v_strSQL As String

            If Not (v_attrColl.GetNamedItem(gc_AtributeLOCAL) Is Nothing) Then
                v_strLocal = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeLOCAL), Xml.XmlAttribute).Value)
            Else
                v_strLocal = String.Empty
            End If

            v_nodeList = pv_xmlDocument.SelectNodes("/ObjectMessage/ObjData")
            For i As Integer = 0 To v_nodeList.Item(0).ChildNodes.Count - 1
                With v_nodeList.Item(0).ChildNodes(i)
                    v_strFldName = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                    v_strValue = .InnerText.ToString()

                    Select Case Trim(v_strFLDNAME)
                        Case "SICODE"
                            v_strSICODE = Trim(v_strVALUE)
                        Case "ISCODE"
                            v_strISCODE = Trim(v_strVALUE)
                        Case "TYPE"
                            v_intType = CInt(Trim(v_strValue))
                        Case "STOCK_TYPE"
                            If v_strValue <> "" Then
                                v_intStockType = CInt(Trim(v_strValue))
                            Else
                                v_intStockType = 0
                            End If
                        Case "PART_VALUE" 'Add by ThangLV 20170731- lay gia tri menh gia
                            If v_strValue <> "" Then
                                v_Partvalue = Convert.ToInt64(Trim(v_strValue))
                            Else
                                v_Partvalue = 0
                            End If
                        Case "BOND_PERIOD" 'Add by HoaLX3 20230514- lay gia Thoi han TP
                            If v_strValue <> "" Then
                                v_BondPeriod = CInt(Trim(v_strValue))
                            Else
                                v_BondPeriod = 0
                            End If
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

            'Kiểm tra SICODE không được trùng
            v_strSQL = "SELECT COUNT(AUTOID) FROM " & ATTR_TABLE & " WHERE UPPER(SICODE) = UPPER('" & v_strSICODE & "') AND DELETED = 0"
            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            If v_ds.Tables(0).Rows.Count = 1 Then
                If v_ds.Tables(0).Rows(0)(0) > 0 Then
                    Return ERR_RG_SI_CODE_DUPLICATED
                End If
            End If

            '---Added by ThangLV 2017/07/19 - Kiem tra Ma CW 
            If v_intType = "4" Then
                'Ma CW phai co 8 ky tu
                If (Len(v_strSICODE) <> 8) Then
                    Return ERR_RG_SI_CW_CODE_2
                End If
                'Ma CW co ky tu dau phai la C or P
                If (v_strSICODE.Substring(0, 1) <> "C" And v_strSICODE.Substring(0, 1) <> "P") Then
                    Return ERR_RG_SI_CW_CODE_1
                End If
                'Ma CW co ky 4 ky tu cuoi phai la so

                If Not (IsNumeric(v_strSICODE.Substring(4, 4))) Then
                    Return ERR_RG_SI_CW_CODE_3
                End If
            End If

            v_strSQL = "SELECT ISTYPE FROM RGIS WHERE ISCODE = '" & v_strIsCode & "' AND DELETED=0"
            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            If v_ds.Tables(0).Rows.Count = 1 Then
                v_intIsType = CInt(v_ds.Tables(0).Rows(0)(0))
            End If
            Select Case v_intIsType
                Case 1
                    If v_intType <> 1 And v_intType <> 2 And v_intType <> 4 Then
                        Return ERR_RG_SI_ISTYPE11
                    ElseIf v_intType = 1 Then
                        If v_intStockType <> 5 And v_intStockType <> 6 And v_intStockType <> 7 And v_intStockType <> 9 Then
                            Return ERR_RG_SI_ISTYPE12
                        End If
                    End If
                Case 2
                    If v_intType <> 1 Then
                        Return ERR_RG_SI_ISTYPE21
                    Else
                        If v_intStockType <> 1 And v_intStockType <> 2 And v_intStockType <> 4 And v_intStockType <> 8 Then
                            Return ERR_RG_SI_ISTYPE22
                        End If
                    End If
                Case 3
                    If v_intType <> 1 And v_intStockType <> 3 Then
                        Return ERR_RG_SI_ISTYPE3
                    End If
                Case 4
                    If v_intType <> 3 Then
                        Return ERR_RG_SI_ISTYPE4
                    End If
                    'Case 5
                    'If v_intType <> 4 Then
                    'Return ERR_RG_SI_ISTYPE5
                    'End If
            End Select

            'Kiểm tra mot TCPH chi dang ky duy nhat mot ma chung khoan
            If v_intType = "2" Then
                v_strSQL = "SELECT COUNT(AUTOID) FROM " & ATTR_TABLE & " WHERE UPPER(ISCODE) = UPPER('" & v_strIsCode & "') AND TYPE = 2 AND DELETED = 0"
                v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                If v_ds.Tables(0).Rows.Count = 1 Then
                    If v_ds.Tables(0).Rows(0)(0) > 0 Then
                        Return ERR_RG_IS_HAS_STOCK
                    End If
                End If
            End If

            'Add by ThangLV - Check menh gia > 0 voi Type in (1,2,3)
            If v_intType = "1" Or v_intType = "2" Or v_intType = "3" Then
                If v_Partvalue = 0 Then
                    Return ERR_RG_SI_PARTVALUE_1
                End If
            End If

            'Add by HoaLX - Check thoi han TP > 0, Type = 1 - Trai Phieu
            If v_BondPeriod = 0 And v_intType = "1" Then
                Return ERR_RG_SI_BONDPERIOD_1
            End If

            If Not (v_ds Is Nothing) Then
                v_ds.Dispose()
            End If

            'ContextUtil.SetComplete()
            Return 0

        Catch ex As Exception
            'ContextUtil.SetAbort()
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            '& "Error code: System error!" & vbNewLine _
            '& "Error message: " & ex.Message, EventLogEntryType.Error)
            Throw ex
        End Try
    End Function

    '********************************************************************
    'Mục đích       	: Kiểm tra dữ liệu trước khi UPDATE vào dữ liệu vào bảng RGSI
    'Tham số        	: XMLDocumentEx
    'Trả về         	: Int
    'Ngày tạo			: 29/01/2008
    'Người tạo		    : Nguyen Manh Ha
    'Ngày cập nhật  	:<15/12/2008>
    'Người cập nhật 	:<Hanm5>
    '********************************************************************
    Overrides Function CheckBeforeEdit(ByVal pv_xmlDocument As XmlDocumentEx) As Long
        Dim v_ds As DataSet
        Dim v_nodeList As Xml.XmlNodeList
        Try
            Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes
            Dim v_strLocal As String, v_strSICODE As String = ""
            Dim v_strIsCode As String = ""
            Dim v_intIsType As Integer = 0
            Dim v_intType As Integer = 0
            Dim v_intStockType As Integer = 0
            Dim v_strFldName, v_strValue As String
            Dim v_strSQL As String
            'Add by ThangLV 20170731 - dinh nghia menh gia
            Dim v_Partvalue As Int64 = 0

            'Add by HoaLX3 20230514 - dinh nghia thoi han TP
            Dim v_BondPeriod As Integer = 0

            If Not (v_attrColl.GetNamedItem(gc_AtributeLOCAL) Is Nothing) Then
                v_strLocal = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeLOCAL), Xml.XmlAttribute).Value)
            Else
                v_strLocal = String.Empty
            End If

            v_nodeList = pv_xmlDocument.SelectNodes("/ObjectMessage/ObjData")
            For i As Integer = 0 To v_nodeList.Item(0).ChildNodes.Count - 1
                With v_nodeList.Item(0).ChildNodes(i)
                    v_strFldName = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                    v_strValue = .InnerText.ToString()

                    Select Case Trim(v_strFldName)
                        Case "SICODE"
                            v_strSICODE = Trim(v_strValue)
                        Case "ISCODE"
                            v_strIsCode = Trim(v_strValue)
                        Case "TYPE"
                            v_intType = CInt(Trim(v_strValue))
                        Case "STOCK_TYPE"
                            If v_strValue <> "" Then
                                v_intStockType = CInt(Trim(v_strValue))
                            Else
                                v_intStockType = 0
                            End If
                        Case "PART_VALUE" 'Add by ThangLV 20170731- lay gia tri menh gia
                            If v_strValue <> "" Then
                                v_Partvalue = Convert.ToInt64(Trim(v_strValue))
                            Else
                                v_Partvalue = 0
                            End If
                        Case "BOND_PERIOD" 'Add by HoaLX3 20230514- lay gia Thoi han TP
                            If v_strValue <> "" Then
                                v_BondPeriod = CInt(Trim(v_strValue))
                            Else
                                v_BondPeriod = 0
                            End If
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

            v_strSQL = "SELECT ISTYPE FROM RGIS WHERE ISCODE = '" & v_strIsCode & "' AND DELETED=0"
            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            If v_ds.Tables(0).Rows.Count = 1 Then
                v_intIsType = CInt(v_ds.Tables(0).Rows(0)(0))
            End If
            Select Case v_intIsType
                Case 1
                    If v_intType <> 1 And v_intType <> 2 And v_intType <> 4 Then
                        Return ERR_RG_SI_ISTYPE11
                    ElseIf v_intType = 1 Then
                        If v_intStockType <> 5 And v_intStockType <> 6 And v_intStockType <> 9 Then
                            Return ERR_RG_SI_ISTYPE12
                        End If
                    End If
                Case 2
                    If v_intType <> 1 Then
                        Return ERR_RG_SI_ISTYPE21
                    Else
                        If v_intStockType <> 1 And v_intStockType <> 2 And v_intStockType <> 4 And v_intStockType <> 8 Then
                            Return ERR_RG_SI_ISTYPE22
                        End If
                    End If
                Case 3
                    If v_intType <> 1 And v_intStockType <> 3 Then
                        Return ERR_RG_SI_ISTYPE3
                    End If
                Case 4
                    If v_intType <> 3 Then
                        Return ERR_RG_SI_ISTYPE4
                    End If
            End Select
            If Not (v_ds Is Nothing) Then
                v_ds.Dispose()
            End If
            'Add by ThangLV - Check menh gia > 0 voi Type in (1,2,3)
            If v_intType = "1" Or v_intType = "2" Or v_intType = "3" Then
                If v_Partvalue = 0 Then
                    Return ERR_RG_SI_PARTVALUE_1
                End If
            End If

            'Add by HoaLX - Check thoi han TP > 0, Type = 1 - Trai Phieu
            If v_BondPeriod = 0 And v_intType = "1" Then
                Return ERR_RG_SI_BONDPERIOD_1
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
    '********************************************************************
    'Mục đích       	: Kiểm tra dữ liệu trước khi xóa dữ liệu trong bảng RGSI
    'Tham số        	: XMLDocumentEx
    'Trả về         	: Int
    'Ngày tạo			: 01/11/2008
    'Người tạo		    : Nguyen Manh Ha
    'Ngày cập nhật  	:<15/12/2008>
    'Người cập nhật 	:<Hanm5>
    '********************************************************************

    Overrides Function CheckBeforeDelete(ByVal pv_xmlDocument As XmlDocumentEx) As Long
        Try
            Dim v_ds As DataSet = Nothing
            Dim v_strSQL As String = ""
            Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes
            Dim v_strClause As String = ""
            Dim v_strLocal As String = ""
            Dim v_strAutoid As String = ""

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

            Dim v_obj As DataAccess
            If v_strLocal = "Y" Then
                v_obj = New DataAccess
            ElseIf v_strLocal = "N" Then
                v_obj = New DataAccess
                v_obj.NewDBInstance(gc_MODULE_HOST)
            End If

            'Không cho phép xóa chứng khoán khi có giao dich bên bảng TLLOG
            v_strSQL = "SELECT COUNT(A.AUTOID) FROM TLLOG A WHERE A.DELETED = 0 AND A.SICODE " & Mid(v_strClause, InStr(v_strClause, "="))
            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            If v_ds.Tables(0).Rows.Count = 1 Then
                If v_ds.Tables(0).Rows(0)(0) > 0 Then
                    Return ERR_RG_SI_HAS_TRANS
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
#End Region
#Region "Private Function"
    Private Function UpdateToRGII(ByVal pv_xmlDocument As XmlDocumentEx) As Long
        Try
            Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes
            Dim v_nodeList As Xml.XmlNodeList = Nothing
            Dim v_strLocal As String
            Dim v_strValue As String = ""
            Dim v_strFLDNAME As String = ""

            Dim v_strIsCode As String
            Dim v_strSiCode As String

            If Not (v_attrColl.GetNamedItem(gc_AtributeLOCAL) Is Nothing) Then
                v_strLocal = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeLOCAL), Xml.XmlAttribute).Value)
            Else
                v_strLocal = String.Empty
            End If

            v_nodeList = pv_xmlDocument.SelectNodes("/ObjectMessage/ObjData")
            For i As Integer = 0 To v_nodeList.Item(0).ChildNodes.Count - 1
                With v_nodeList.Item(0).ChildNodes(i)
                    v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                    v_strValue = .InnerText.ToString()

                    Select Case Trim(v_strFLDNAME)
                        Case "ISCODE"
                            v_strIsCode = CStr(Trim(v_strValue))
                        Case "SICODE"
                            v_strSiCode = CStr(Trim(v_strValue))
                    End Select
                End With
            Next

            Dim v_strSql As String = ""
            v_strSql = "MERGE INTO RGII A USING RGIS B ON (A.CARDNO = B.BUSSINESS_NO AND A.CARDDATE = B.BUSSINESS_DATE AND B.ISCODE = '" & v_strIsCode & "' " _
                       & " AND A.DELETED = 0 AND A.STATUS = 0 AND B.DELETED = 0 AND B.STATUS = 0 )" _
                       & " WHEN MATCHED THEN UPDATE SET A.SICODE = A.SICODE || '" & v_strSiCode & "|'"

            Dim v_obj As DataAccess = Nothing
            If v_strLocal = "Y" Then
                v_obj = New DataAccess
            ElseIf v_strLocal = "N" Then
                v_obj = New DataAccess
                v_obj.NewDBInstance(gc_MODULE_HOST)
            End If

            v_obj.ExecuteNonQuery(CommandType.Text, v_strSql)
            'ContextUtil.SetComplete()
            Return ERR_SYSTEM_OK

        Catch ex As Exception
            Return ERR_SYSTEM_START
            Throw ex
        End Try
    End Function
#End Region

End Class
