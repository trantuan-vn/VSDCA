Imports Sats.CommonLibrary
Imports Sats.CoreBusiness
Imports Sats.DataAccessLayer

Public Class MF
    Inherits objMaster
    Implements IMaster

    Public Function Add(ByRef pv_xmlDocument As CommonLibrary.XmlDocumentEx) As Long Implements CoreBusiness.IMaster.Add

    End Function

    Public Function Adhoc(ByRef pv_xmlDocument As CommonLibrary.XmlDocumentEx) As Long Implements CoreBusiness.IMaster.Adhoc
        Dim v_lngErrCode As Long = ERR_SYSTEM_OK
        Dim v_strFuncName As String
        Dim v_strObjMsg As String

        Try
            Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes
            v_strFuncName = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeFUNCNAME), Xml.XmlAttribute).Value)

            v_strObjMsg = pv_xmlDocument.InnerXml
            Select Case Trim(v_strFuncName)
                Case "AddMFToTLTXCD"
                    v_lngErrCode = AddMFToTLTX(pv_xmlDocument)
            End Select
            pv_xmlDocument.LoadXml(v_strObjMsg)

            'ContextUtil.SetComplete()
            Return v_lngErrCode
        Catch ex As Exception
            'ContextUtil.SetAbort()
        End Try
    End Function

    Public Function Delete(ByRef pv_xmlDocument As CommonLibrary.XmlDocumentEx) As Long Implements CoreBusiness.IMaster.Delete

    End Function

    Public Function Edit(ByRef pv_xmlDocument As CommonLibrary.XmlDocumentEx) As Long Implements CoreBusiness.IMaster.Edit

    End Function

    Public Function Inquiry(ByRef pv_xmlDocument As CommonLibrary.XmlDocumentEx) As Long Implements CoreBusiness.IMaster.Inquiry

    End Function

    ''------------------------------------------------------------------------''
    ''-- + Mục đích: Gan muc phi cho giao dich                              --''
    ''-- + Đầu vào: pv_xmlDocument: XmlDocument chứa các dữ liệu phân quyền --''
    ''-- + Đầu ra: N/A                                                      --''
    ''-- + Tác giả: Nguyễn Minh Thơ                                         --''
    ''-- + Ghi chú: N/A                                                     --''
    ''------------------------------------------------------------------------''
    Private Function AddMFToTLTX(ByRef pv_xmlDocument As XmlDocumentEx) As Long
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
            Dim v_arrTLTXCD(), v_strTLTXCD, v_strMF As String
            v_arrTLTXCD = v_strClause.Split("#")
            v_strMF = v_arrTLTXCD(0)
            v_strTLTXCD = "('" & Replace(Mid(v_arrTLTXCD(1), 1, Len(v_arrTLTXCD(1)) - 1), "|", "','") & "')"

            'Inquiry data
            Dim v_obj As New DataAccess
            'Dim v_obj As DataAccess
            'If v_strLocal = "Y" Then
            '    v_obj.NewDBInstance(gc_MODULE_BDS)
            'ElseIf v_strLocal = "N" Then
            v_obj.NewDBInstance(gc_MODULE_HOST)
            'End If

            Dim v_strSQL As String
            'Remove muc phi da gan cho giao dich
            v_strSQL = "UPDATE TLTX SET MFNO=REPLACE(MFNO,'|" & v_strMF & "','')" _
                                    & " WHERE INSTR(MFNO,'" & v_strMF & "') > 0"
            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

            'Gan muc phi cho giao dich
            v_strSQL = "UPDATE TLTX SET MFNO=MFNO || '|" & v_strMF & "'" _
                        & " WHERE TLTXCD IN " & v_strTLTXCD & " AND (INSTR(MFNO,'" & v_strMF & "') = 0 OR NVL(MFNO,' ') = ' ')"
            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

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
End Class
