Imports SATS.CommonLibrary
Imports SATS.DataAccessLayer
Imports System.Configuration

'Imports System.EnterpriseServices
'<JustInTimeActivation(False), _
'Transaction(TransactionOption.Disabled), _
'ObjectPooling(Enabled:=True, MinPoolSize:=30)> _
Public Class SYNCODE
    Inherits CoreBusiness.objMaster
    Implements CoreBusiness.IMaster

    Public Sub New()
        ATTR_TABLE = "SYNCODE"
        'ContextUtil.SetComplete()
    End Sub
    Public Function Add(ByRef pv_xmlDocument As XmlDocumentEx) As Long Implements CoreBusiness.IMaster.Add
        'Dim v_nodeList As Xml.XmlNodeList, v_objBranch As New Branch
        'Dim v_strValue, v_strMODCODE, v_strOBJNAME As String
        ''Quét danh sách các bảng cần đồng bộ dữ liệu
        'Try
        '    v_nodeList = pv_xmlDocument.SelectNodes("/ObjectMessage/fields")
        '    For i As Integer = 0 To v_nodeList.Count - 1
        '        For j As Integer = 0 To v_nodeList.Item(i).ChildNodes.Count - 1
        '            With v_nodeList.Item(i).ChildNodes(j)
        '                'Xác định bảng dữ liệu cần đồng bộ
        '                v_strOBJNAME = Trim(.InnerText.ToString)
        '                v_strMODCODE = CStr(CType(.Attributes.GetNamedItem(gc_AtributeMODCODE), Xml.XmlAttribute).Value)
        '                'G�?i hàm để đồng bộ bảng tương ứng
        '                v_objBranch.RefreshCode(v_strMODCODE, v_strOBJNAME)

        '            End With
        '        Next
        '    Next
        'ContextUtil.SetComplete()
        'Catch ex As Exception
        'ContextUtil.SetAbort
        '   LogError.Write("Error source: " & ex.Source & vbNewLine _
        '                 & "Error code: System error!" & vbNewLine _
        '                 & "Error message: " & ex.Message, EventLogEntryType.Error)
        '    Throw ex
        'End Try
    End Function
    '-------------------------------------------------------------------------------
    'Hàm này xử lý đặc biệt để đồng bộ các bảng danh mục từ trên HOST v�?
    '-------------------------------------------------------------------------------
    Public Function Adhoc(ByRef pv_xmlDocument As XmlDocumentEx) As Long Implements CoreBusiness.IMaster.Adhoc
        Dim v_lngErrCode As Long = ERR_SYSTEM_OK
        Dim v_strFuncName As String

        Dim v_nodeList As Xml.XmlNodeList
        Dim v_strMODCODE, v_strOBJNAME, v_strSYNCMD, v_strBRID As String
        'Quét danh sách các bảng cần đồng bộ dữ liệu
        Try
            Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes

            v_strBRID = pv_xmlDocument.DocumentElement.Attributes(gc_AtributeBRID).Value.ToString()

            v_nodeList = pv_xmlDocument.SelectNodes("/ObjectMessage/fields")
            For i As Integer = 0 To v_nodeList.Count - 1
                For j As Integer = 0 To v_nodeList.Item(i).ChildNodes.Count - 1
                    With v_nodeList.Item(i).ChildNodes(j)
                        'Xác định bảng dữ liệu cần đồng bộ
                        v_strOBJNAME = Trim(.InnerText.ToString)
                        'v_strBRID = CStr(CType(.Attributes.GetNamedItem(gc_AtributeBRID), Xml.XmlAttribute).Value)
                        v_strMODCODE = CStr(CType(.Attributes.GetNamedItem(gc_AtributeMODCODE), Xml.XmlAttribute).Value)
                        v_strSYNCMD = CType(.Attributes.GetNamedItem(gc_AtributeCMDINQUIRY), Xml.XmlAttribute).Value.ToString().Replace("<$BRID>", v_strBRID)
                        v_strFuncName = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeFUNCNAME), Xml.XmlAttribute).Value)

                        'G�?i hàm để đồng bộ bảng tương ứng
                        Select Case Trim(v_strFuncName)
                            Case "RefreshCode"
                                v_lngErrCode = RefreshCode(v_strMODCODE, v_strOBJNAME, v_strSYNCMD)
                        End Select
                    End With
                Next
            Next
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

    Public Function Delete(ByRef pv_xmlDocument As XmlDocumentEx) As Long Implements CoreBusiness.IMaster.Delete
        'ContextUtil.SetComplete()
    End Function

    Public Function Edit(ByRef pv_xmlDocument As XmlDocumentEx) As Long Implements CoreBusiness.IMaster.Edit
        'ContextUtil.SetComplete()
    End Function

    Public Function Inquiry(ByRef pv_xmlDocument As XmlDocumentEx) As Long Implements CoreBusiness.IMaster.Inquiry
        Inquiry = CoreInquiry(pv_xmlDocument)
        'ContextUtil.SetComplete()
    End Function

#Region " Private methods "
    Private Function RefreshCode(ByVal pv_strObjCode As String, ByVal pv_strObjName As String, ByVal pv_strSYNCMD As String) As Long
        Dim v_xmlDocument As New Xml.XmlDocument, v_nodeList As Xml.XmlNodeList
        Dim v_lngErrorCode As Long, v_strObjMsg As String
        Dim v_strValue, v_strFLDNAME, v_strFLDTYPE As String
        'Dim v_ws As New HOSTDelivery.HOSTDelivery

        Dim v_strListOfFields As String = vbNullString
        Dim v_strListOfValues As String = vbNullString

        Dim v_strSQL As String, v_strNewSQL As String

        Dim v_objDataAccess As DataAccess
        v_objDataAccess = New DataAccess

        'Set the reference URL for webservice and time-out
        'v_ws.Url = GetHostDeliveryUrl()
        'v_ws.Timeout = gc_WEB_SERVICE_TIMEOUT

        Dim v_ds As DataSet
        Try
            'Lấy dữ liệu từ HOST v�? 
            v_strObjMsg = BuildXMLObjMsg(, , , , gc_IsLocalMsg, gc_MsgTypeObj, pv_strObjCode & "." & pv_strObjName, gc_ActionInquiry, pv_strSYNCMD)
            'v_ws.Message(v_strObjMsg)
            Dim v_obj As New Branch
            Dim v_lngErr As Long = v_obj.SendMessage2Host(v_strObjMsg)

            v_xmlDocument.LoadXml(v_strObjMsg)
            v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")

            'Xóa dữ liệu cũ của bảng cần đồng bộ
            If v_nodeList.Count > 0 Then
                v_strFLDNAME = CStr(CType(v_nodeList.Item(0).ChildNodes(0).Attributes.GetNamedItem(gc_AtributeFLDNAME), Xml.XmlAttribute).Value)
                v_strSQL = "SELECT COUNT(" & v_strFLDNAME & ") FROM " & pv_strObjName
                v_ds = v_objDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                If v_ds.Tables(0).Rows.Count = 1 Then
                    If CInt(v_ds.Tables(0).Rows(0)(0)) > 0 Then
                        v_strSQL = "DELETE FROM " & pv_strObjName
                        v_objDataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
                    End If
                End If

                'Insert dữ liệu đồng bộ từ HOST
                For i As Integer = 0 To v_nodeList.Count - 1
                    v_strListOfFields = vbNullString
                    v_strListOfValues = vbNullString
                    v_strNewSQL = "INSERT INTO " & pv_strObjName
                    For j As Integer = 0 To v_nodeList.Item(i).ChildNodes.Count - 1
                        With v_nodeList.Item(i).ChildNodes(j)
                            v_strValue = Trim(.InnerText.ToString)
                            v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem(gc_AtributeFLDNAME), Xml.XmlAttribute).Value)
                            v_strFLDTYPE = CStr(CType(.Attributes.GetNamedItem(gc_AtributeFLDTYPE), Xml.XmlAttribute).Value)

                            If Len(v_strValue) > 0 Then
                                If Len(v_strListOfFields) = 0 Then
                                    v_strListOfFields = "(" & v_strFLDNAME
                                    Select Case v_strFLDTYPE
                                        Case "System.String"
                                            v_strListOfValues = "('" & v_strValue & "'"
                                        Case "System.Date"
                                            v_strListOfValues = "('" & v_strValue & "'"
                                        Case Else
                                            v_strListOfValues = "(" & v_strValue
                                    End Select
                                Else
                                    v_strListOfFields = v_strListOfFields & "," & v_strFLDNAME
                                    Select Case v_strFLDTYPE
                                        Case "System.String"
                                            v_strListOfValues = v_strListOfValues & ",'" & v_strValue.Replace("'", "''") & "'"
                                        Case "System.DateTime"
                                            v_strListOfValues = v_strListOfValues & ",TO_DATE('" & v_strValue & "', '" & gc_FORMAT_DATE & "')"
                                        Case GetType(Double).Name
                                            v_strListOfValues = v_strListOfValues & "," & Replace(v_strValue, ",", "")
                                        Case Else
                                            v_strListOfValues = v_strListOfValues & "," & v_strValue
                                    End Select
                                End If
                            End If
                        End With
                    Next
                    If Len(v_strListOfFields) <> 0 Then
                        v_strListOfFields = v_strListOfFields & ")"
                        v_strListOfValues = v_strListOfValues & ")"
                        v_strNewSQL = v_strNewSQL & " " & v_strListOfFields & " VALUES " & v_strListOfValues
                        v_objDataAccess.ExecuteNonQuery(CommandType.Text, v_strNewSQL)
                    End If
                Next
            Else

            End If

            'Complete transaction
            'ContextUtil.SetComplete()
            Return v_lngErrorCode
        Catch ex As Exception
            'Abort transaction
            'ContextUtil.SetAbort()

            'LogError.Write(ex.Message & vbNewLine & ex.StackTrace, EventLogEntryType.Error)
            Throw ex
        End Try
    End Function

#End Region

End Class
