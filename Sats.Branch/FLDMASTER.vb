Imports Sats.CommonLibrary
Imports Sats.DataAccessLayer
'Imports System.EnterpriseServices
'<JustInTimeActivation(False), _
'Transaction(TransactionOption.Disabled), _
'ObjectPooling(Enabled:=True, MinPoolSize:=30)> _
Public Class FLDMASTER
    Inherits CoreBusiness.objMaster
    Implements CoreBusiness.IMaster

    Public Sub New()
        ATTR_TABLE = "FLDMASTER"
        'ContextUtil.SetComplete()
    End Sub

    Public Function Add(ByRef pv_xmlDocument As XmlDocumentEx) As Long Implements CoreBusiness.IMaster.Add
        'ContextUtil.SetComplete()
    End Function

    Public Function Adhoc(ByRef pv_xmlDocument As XmlDocumentEx) As Long Implements CoreBusiness.IMaster.Adhoc
        'ContextUtil.SetComplete()
    End Function

    Public Function Delete(ByRef pv_xmlDocument As XmlDocumentEx) As Long Implements CoreBusiness.IMaster.Delete
        'ContextUtil.SetComplete()
    End Function

    Public Function Edit(ByRef pv_xmlDocument As XmlDocumentEx) As Long Implements CoreBusiness.IMaster.Edit
        'ContextUtil.SetComplete()
    End Function

    Public Function Inquiry(ByRef pv_xmlDocument As XmlDocumentEx) As Long Implements CoreBusiness.IMaster.Inquiry
        Inquiry = CoreInquiry(pv_xmlDocument)
        'Đối với FLDMASTER, sẽ xử lý nếu field hiển thị dạng combo thì sẽ lấy luôn danh sách các giá trị lựa chọn ở đây
        Inquiry = GetReferenceData(pv_xmlDocument)
        'ContextUtil.SetComplete()
    End Function

    Private Function GetReferenceData(ByRef pv_xmlDocument As XmlDocumentEx) As Long
        Dim v_nodeList As Xml.XmlNodeList, i, j, k, kk As Integer
        Dim v_dataElement As Xml.XmlElement, v_entryNode As Xml.XmlNode
        Dim v_attrREFFIELDNAME As Xml.XmlAttribute
        Dim v_attrFLDNAME As Xml.XmlAttribute, v_attrFLDTYPE As Xml.XmlAttribute, v_attrOLDVAL As Xml.XmlAttribute
        Dim v_strFLDNAME, v_strFIELDNAME, v_strVALUE, v_strFLDTYPE, v_strLLIST As String
        Dim v_strSQL As String, v_ds As DataSet, v_obj As New DataAccess
        v_obj.NewDBInstance(gc_MODULE_HOST)


        v_nodeList = pv_xmlDocument.SelectNodes("/ObjectMessage/ObjData")
        For i = 0 To v_nodeList.Count - 1
            v_strFIELDNAME = String.Empty
            v_strFLDTYPE = String.Empty
            v_strLLIST = String.Empty
            For j = 0 To v_nodeList.Item(i).ChildNodes.Count - 1
                With v_nodeList.Item(i).ChildNodes(j)
                    v_strVALUE = .InnerText.ToString
                    v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                    Select Case Trim(v_strFLDNAME)
                        Case "FLDNAME"
                            v_strFIELDNAME = Trim(v_strVALUE)
                        Case "FLDTYPE"
                            v_strFLDTYPE = Trim(v_strVALUE)
                        Case "LLIST"
                            v_strLLIST = Trim(v_strVALUE)
                    End Select
                End With
            Next

            'Xử lý nếu là lấy danh sách hiển thị lên combo
            If v_strFLDTYPE = "C" And v_strLLIST.Length > 0 Then
                v_strSQL = v_strLLIST
                v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                If v_ds.Tables(0).Rows.Count > 0 Then
                    For k = 0 To v_ds.Tables(0).Rows.Count - 1
                        v_dataElement = pv_xmlDocument.CreateElement(Xml.XmlNodeType.Element, "ObjDataRef", "")
                        For kk = 0 To v_ds.Tables(0).Columns.Count - 1
                            'Append entry to data node
                            v_entryNode = pv_xmlDocument.CreateNode(Xml.XmlNodeType.Element, "entry", "")

                            v_attrREFFIELDNAME = pv_xmlDocument.CreateAttribute("refname")
                            v_attrREFFIELDNAME.Value = v_strFIELDNAME
                            v_entryNode.Attributes.Append(v_attrREFFIELDNAME)

                            'Add field name
                            v_attrFLDNAME = pv_xmlDocument.CreateAttribute("fldname")
                            v_attrFLDNAME.Value = v_ds.Tables(0).Columns(kk).ColumnName
                            v_entryNode.Attributes.Append(v_attrFLDNAME)

                            'Add field type
                            v_attrFLDTYPE = pv_xmlDocument.CreateAttribute("fldtype")
                            v_attrFLDTYPE.Value = v_ds.Tables(0).Columns(kk).DataType.ToString
                            v_entryNode.Attributes.Append(v_attrFLDTYPE)

                            'Add current value
                            v_attrOLDVAL = pv_xmlDocument.CreateAttribute("oldval")
                            If IsDBNull((v_ds.Tables(0).Rows(k)(kk))) Then
                                If v_ds.Tables(0).Rows(k)(kk).GetType.Name = GetType(System.DateTime).Name _
                                    Or v_ds.Tables(0).Rows(k)(kk).GetType.Name = GetType(System.String).Name Then
                                    v_attrOLDVAL.Value = ""
                                Else
                                    v_attrOLDVAL.Value = "0"
                                End If

                            Else
                                If v_ds.Tables(0).Rows(k)(kk).GetType.Name = GetType(System.DateTime).Name Then
                                    v_attrOLDVAL.Value = Format(v_ds.Tables(0).Rows(k)(kk), gc_FORMAT_DATE)
                                Else
                                    v_attrOLDVAL.Value = CStr(v_ds.Tables(0).Rows(k)(kk))
                                End If
                            End If
                            v_entryNode.Attributes.Append(v_attrOLDVAL)

                            'Set value
                            If IsDBNull((v_ds.Tables(0).Rows(k)(kk))) Then
                                v_entryNode.InnerText = ""
                            Else
                                If v_ds.Tables(0).Rows(k)(kk).GetType.Name = GetType(System.DateTime).Name Then
                                    v_entryNode.InnerText = Format(v_ds.Tables(0).Rows(k)(kk), gc_FORMAT_DATE)
                                Else
                                    v_entryNode.InnerText = CStr(v_ds.Tables(0).Rows(k)(kk))
                                End If
                            End If

                            v_dataElement.AppendChild(v_entryNode)
                        Next
                        pv_xmlDocument.DocumentElement.AppendChild(v_dataElement)
                    Next
                End If

            End If
        Next
    End Function
End Class
