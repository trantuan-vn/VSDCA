Imports Sats.CommonLibrary
Imports Sats.DataAccessLayer
Public Class TLTXUSERAUTH
    Inherits CoreBusiness.objMaster
    Implements CoreBusiness.IMaster

    Public Sub New()
        ATTR_TABLE = "TLTXUSERAUTH"
        'ContextUtil.SetComplete()
    End Sub

    Public Function Add(ByRef pv_xmlDocument As CommonLibrary.XmlDocumentEx) As Long Implements CoreBusiness.IMaster.Add
        Dim v_lngErrCode As Long

        Try
            v_lngErrCode = CoreAdd(pv_xmlDocument)
            If v_lngErrCode <> 0 Then
                Dim v_strErrorSource, v_strErrorMessage As String

                v_strErrorSource = "TLTXUSERAUTH.Add"
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

    Public Function Adhoc(ByRef pv_xmlDocument As CommonLibrary.XmlDocumentEx) As Long Implements CoreBusiness.IMaster.Adhoc

    End Function

    Public Function Delete(ByRef pv_xmlDocument As CommonLibrary.XmlDocumentEx) As Long Implements CoreBusiness.IMaster.Delete
        Dim v_lngErrCode As Long

        Try
            v_lngErrCode = CoreDelete(pv_xmlDocument)
            If v_lngErrCode <> 0 Then
                Dim v_strErrorSource, v_strErrorMessage As String

                v_strErrorSource = "TLTXUSERAUTH.Delete"
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

    Public Function Edit(ByRef pv_xmlDocument As CommonLibrary.XmlDocumentEx) As Long Implements CoreBusiness.IMaster.Edit
        Dim v_lngErrCode As Long

        Try
            v_lngErrCode = CoreEdit(pv_xmlDocument)
            If v_lngErrCode <> 0 Then
                Dim v_strErrorSource, v_strErrorMessage As String

                v_strErrorSource = "TLTXUSERAUTH.Edit"
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

    Public Function Inquiry(ByRef pv_xmlDocument As CommonLibrary.XmlDocumentEx) As Long Implements CoreBusiness.IMaster.Inquiry
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
            Dim v_strLocal As String
            Dim v_strAuthId As String = "", v_strAuthType As String = "", v_strTLTXCD As String = ""
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
                        Case "AUTHID"
                            v_strAuthId = Trim(v_strVALUE)
                        Case "AUTHTYPE"
                            v_strAuthType = Trim(v_strVALUE)
                        Case "TLTXCD"
                            v_strTLTXCD = Trim(v_strVALUE)
                    End Select
                End With
            Next

            Dim v_obj As New DataAccess
            'If v_strLocal = "Y" Then
            '    v_obj.NewDBInstance(gc_MODULE_BDS)
            'ElseIf v_strLocal = "N" Then
            v_obj.NewDBInstance(gc_MODULE_HOST)
            'End If

            'Kiểm tra AUTHID, AUTHTYPE, TLTXCD không được trùng
            v_strSQL = "SELECT COUNT(AUTHID) FROM " & ATTR_TABLE & " WHERE AUTHID = '" & v_strAuthId & "' AND AUTHTYPE = '" & v_strAuthType & "'" _
                        & " AND TLTXCD = '" & v_strTLTXCD & "' AND DELETED = 0 AND STATUS = 0"
            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            If v_ds.Tables(0).Rows.Count = 1 Then
                If v_ds.Tables(0).Rows(0)(0) > 0 Then
                    Return ERR_SA_TLXUSERAUTH_DUPLICATED
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
End Class
