Imports Sats.CommonLibrary
Imports Sats.DataAccessLayer

Public Class SynData
    Inherits CoreBusiness.objMaster
    Implements CoreBusiness.IMaster

    Public Function Add(ByRef pv_xmlDocument As CommonLibrary.XmlDocumentEx) As Long Implements CoreBusiness.IMaster.Add

    End Function

    Public Function Adhoc(ByRef pv_xmlDocument As CommonLibrary.XmlDocumentEx) As Long Implements CoreBusiness.IMaster.Adhoc

        Dim v_xmlDocument As New Xml.XmlDocument, v_nodeList As Xml.XmlNodeList
        Dim v_lngErrorCode As Long, v_strObjMsg As String
        Dim v_strValue, v_strFLDNAME, v_strFLDTYPE As String
        Dim v_arrClause(), v_strListOfFields, v_strListOfValues As String

        Dim v_obj As New Branch
        Dim v_strSQL As String, v_strNewSQL As String
        Dim v_strTable As String, v_strObject As String = ""

        Try
            Dim v_objDataAccess As DataAccess
            v_objDataAccess = New DataAccess

            Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes
            Dim v_strClause As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCLAUSE), Xml.XmlAttribute).Value)

            v_arrClause = v_strClause.Split("|")

            For v_int As Integer = 0 To v_arrClause.Length - 2
                v_strTable = v_arrClause(v_int)
                v_strSQL = "SELECT * FROM " & v_strTable

                Select Case v_strTable
                    Case "TLPROFILES"
                        v_strObject = OBJNAME_SA_TLPROFILES
                    Case "TLGROUPS"
                        v_strObject = OBJNAME_SA_TLGROUPS
                    Case "CMDAUTH"
                        v_strObject = OBJNAME_SA_CMDAUTH
                    Case "TLAUTH"
                        v_strObject = OBJNAME_SA_TLAUTH
                    Case "ALLCODE"
                        v_strObject = OBJNAME_SA_ALLCODE
                    Case "RGIS"
                        v_strObject = OBJNAME_RG_RGIS
                    Case "RGSI"
                        v_strObject = OBJNAME_RG_RGSI
                    Case "RGMI"
                        v_strObject = OBJNAME_RG_RGMI

                End Select
                'Lay du lieu tu host ve
                v_strObjMsg = BuildXMLObjMsg(, , , , gc_IsLocalMsg, gc_MsgTypeObj, v_strObject, gc_ActionInquiry, v_strSQL)

                v_lngErrorCode = v_obj.SendMessage2Host(v_strObjMsg)

                If v_lngErrorCode <> ERR_SYSTEM_OK Then
                    Return v_lngErrorCode
                End If

                ' Doc du lieu lay dc tu HOST
                v_xmlDocument.LoadXml(v_strObjMsg)
                v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")

                v_objDataAccess.ExecuteNonQuery(CommandType.Text, "DELETE FROM " & v_strTable)

                If v_nodeList.Count > 0 Then
                    For i As Integer = 0 To v_nodeList.Count - 1
                        v_strListOfFields = vbNullString
                        v_strListOfValues = vbNullString
                        v_strNewSQL = "INSERT INTO " & v_strTable
                        For j As Integer = 0 To v_nodeList.Item(i).ChildNodes.Count - 1
                            With v_nodeList.Item(i).ChildNodes(j)
                                v_strValue = Trim(.InnerText.ToString)
                                v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem(gc_AtributeFLDNAME), Xml.XmlAttribute).Value)
                                v_strFLDTYPE = CStr(CType(.Attributes.GetNamedItem(gc_AtributeFLDTYPE), Xml.XmlAttribute).Value)
                            End With

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
                        Next
                        If Len(v_strListOfFields) <> 0 Then
                            v_strListOfFields = v_strListOfFields & ")"
                            v_strListOfValues = v_strListOfValues & ")"
                            v_strNewSQL = v_strNewSQL & " " & v_strListOfFields & " VALUES " & v_strListOfValues
                            v_objDataAccess.ExecuteNonQuery(CommandType.Text, v_strNewSQL)
                        End If
                    Next
                End If
            Next
            Return v_lngErrorCode
        Catch ex As Exception
            'LogError.Write(ex.Message & vbNewLine & ex.StackTrace, EventLogEntryType.Error)
            Throw ex
        End Try
    End Function

    Public Function Delete(ByRef pv_xmlDocument As CommonLibrary.XmlDocumentEx) As Long Implements CoreBusiness.IMaster.Delete

    End Function

    Public Function Edit(ByRef pv_xmlDocument As CommonLibrary.XmlDocumentEx) As Long Implements CoreBusiness.IMaster.Edit

    End Function

    Public Function Inquiry(ByRef pv_xmlDocument As CommonLibrary.XmlDocumentEx) As Long Implements CoreBusiness.IMaster.Inquiry

    End Function
End Class
