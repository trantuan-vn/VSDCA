Imports Sats.CommonLibrary
Imports Sats.DataAccessLayer
'Imports System.EnterpriseServices

'<JustInTimeActivation(False), _
'Transaction(TransactionOption.Supported), _
'ObjectPooling(Enabled:=True, MinPoolSize:=30)> _
Public Class AllCode
    Inherits CoreBusiness.objMaster
    Implements CoreBusiness.IMaster

    Public Sub New()
        ATTR_TABLE = "ALLCODE"
    End Sub

    Public Function Add(ByRef pv_xmlDocument As XmlDocumentEx) As Long Implements CoreBusiness.IMaster.Add
        Dim v_lngErrCode As Long

        Try
            v_lngErrCode = CoreAdd(pv_xmlDocument)
            If v_lngErrCode <> 0 Then
                Dim v_strErrorSource, v_strErrorMessage As String

                v_strErrorSource = "ALLCODE.Add"
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
            ''LogError.Write("Error source: " & ex.Source & vbNewLine _
            '             & "Error code: System error!" & vbNewLine _
            '             & "Error message: " & ex.Message, EventLogEntryType.Error)
            Throw ex
        End Try
    End Function

    Public Function Adhoc(ByRef pv_xmlDocument As XmlDocumentEx) As Long Implements CoreBusiness.IMaster.Adhoc
        'ContextUtil.SetComplete()
    End Function

    Public Function Delete(ByRef pv_xmlDocument As XmlDocumentEx) As Long Implements CoreBusiness.IMaster.Delete
        Dim v_lngErrCode As Long

        Try
            v_lngErrCode = CoreDelete(pv_xmlDocument)
            'If v_lngErrCode <> 0 Then
            '    Dim v_strErrorSource, v_strErrorMessage As String

            '    v_strErrorSource = "ALLCODE.Delete"
            '    v_strErrorMessage = String.Empty

            '    LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
            '                 & "Error code: " & v_lngErrCode.ToString() & vbNewLine _
            '                 & "Error message: " & v_strErrorMessage, EventLogEntryType.Information)
            '    BuildXMLErrorException(pv_xmlDocument, v_strErrorSource, v_lngErrCode, v_strErrorMessage)
            'End If

            'ContextUtil.SetComplete()
            Return v_lngErrCode
        Catch ex As Exception
            ''ContextUtil.SetAbort()
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

                v_strErrorSource = "ALLCODE.Edit"
                v_strErrorMessage = String.Empty

                'LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
                '             & "Error code: " & v_lngErrCode.ToString() & vbNewLine _
                '             & "Error message: " & v_strErrorMessage, EventLogEntryType.Information)
                BuildXMLErrorException(pv_xmlDocument, v_strErrorSource, v_lngErrCode, v_strErrorMessage)
            End If

            ''ContextUtil.SetComplete()
            Return v_lngErrCode
        Catch ex As Exception
            ''ContextUtil.SetAbort()
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

            '    v_strErrorSource = "ALLCODE.Inquiry"
            '    v_strErrorMessage = String.Empty

            '    LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
            '                 & "Error code: " & v_lngErrCode.ToString() & vbNewLine _
            '                 & "Error message: " & v_strErrorMessage, EventLogEntryType.Information)
            '    BuildXMLErrorException(pv_xmlDocument, v_strErrorSource, v_lngErrCode, v_strErrorMessage)
            'End If

            ''ContextUtil.SetComplete()
            Return v_lngErrCode
        Catch ex As Exception
            ''ContextUtil.SetAbort()
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
            Dim v_strLocal As String = ""
            Dim v_strFLDNAME As String = "", v_strCDTYPE As String = "", v_strCDNAME As String = "", v_strCDVAL As String = "", v_strVALUE As String = ""
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
                        Case "CDTYPE"
                            v_strCDTYPE = Trim(v_strVALUE)
                        Case "CDNAME"
                            v_strCDNAME = Trim(v_strVALUE)
                        Case "CDVAL"
                            v_strCDVAL = Trim(v_strVALUE)
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

            v_strSQL = "SELECT COUNT(1) CDVAL FROM ALLCODE WHERE CDNAME ='" & v_strCDNAME & "'" _
                                                & " AND CDTYPE ='" & v_strCDTYPE & "'" _
                                                & " AND CDVAL ='" & v_strCDVAL & "'"

            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            If v_ds.Tables(0).Rows.Count > 0 Then
                If gf_CorrectNumericField(v_ds.Tables(0).Rows(0)(0)) > 0 Then
                    Return ERR_SA_CDVAL_DUPLICATED
                End If
            End If

            If Not (v_ds Is Nothing) Then
                v_ds.Dispose()
            End If

            'ContextUtil.SetComplete()
            Return 0
        Catch ex As Exception
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
        Return 0
    End Function
#End Region

End Class
