Imports Sats.CommonLibrary
'Imports DevExpress.XtraTreeList
'Imports DevExpress.XtraTreeList.Nodes

Public Class frmMessages

    'Public Sub InitTreeView()
    '    Dim v_nodeList As Xml.XmlNodeList
    '    Dim v_strValue, v_strFLDNAME As String
    '    Dim v_xmlDocument As New XmlDocumentEx
    '    If Trim(mv_strMsg) <> "" Then
    '        v_xmlDocument.LoadXml(mv_strMsg)

    '        v_xmlDocument.LoadXml(mv_strMsg)
    '        v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")
    '        Dim v_strMsg As String = ""

    '        tlvTLTX.BeginUnboundLoad()
    '        Dim node As TreeListNode
    '        Dim pnode As TreeListNode = Nothing
    '        tlvTLTX.Nodes.Clear()

    '        Dim v_strTLTXCD, v_strTXDESC, v_strTXNUM As String
    '        For i = 0 To v_nodeList.Count - 1
    '            For j = 0 To v_nodeList.Item(i).ChildNodes.Count - 1
    '                With v_nodeList.Item(i).ChildNodes(j)
    '                    v_strValue = .InnerText.ToString
    '                    v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
    '                    Select Case Trim(v_strFLDNAME)
    '                        Case "TLTXCD"
    '                            v_strTLTXCD = v_strValue
    '                        Case "TXDESC"
    '                            v_strTXDESC = v_strValue
    '                        Case "TXNUM"
    '                            v_strTXNUM = v_strValue
    '                    End Select
    '                End With
    '            Next

    '            node = tlvTLTX.AppendNode(New Object() {CheckState.Unchecked, v_strTXNUM, v_strTLTXCD & " - " & v_strTXDESC}, pnode)
    '            node.HasChildren = False
    '        Next
    '        tlvTLTX.EndUnboundLoad()
    '    End If
    'End Sub

    'Private Sub tlvTLTX_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Dim v_strTLTXCD As String = Mid(tlvTLTX.Selection.Item(0)(2), 1, 4)
    '    Dim v_strTXNUM As String = tlvTLTX.Selection.Item(0)(1)
    '    ShowTran(v_strTXNUM, v_strTLTXCD)
    'End Sub

    'Private Sub tlvTLTX_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs)

    'End Sub

    'Private Sub ShowTran(ByVal pv_strTXNUM As String, ByVal v_strTlTXCD As String)
    '    Dim v_ws As New Sats.BDSChannel.BDSDelivery
    '    Try
    '        Dim v_strSQL As String
    '        Dim v_strValue, v_strFLDNAME As String
    '        Dim v_strObjMsg As String
    '        Dim v_strObjMsg1 As String
    '        Dim v_xmlDocument As New XmlDocumentEx
    '        Dim v_nodeList As Xml.XmlNodeList
    '        Dim v_strField, v_strCaption As String
    '        Dim v_strDetail As String = ""

    '        v_strSQL = "SELECT * FROM FLDMASTER a where a.deleted = 0 and a.status = 0 and a.OBJNAME = '" & v_strTlTXCD & "' ORDER BY a.ODRNUM"
    '        v_strObjMsg = BuildXMLObjMsg(, , , , gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLTX, gc_ActionInquiry, v_strSQL)
    '        v_ws.Message(v_strObjMsg)

    '        v_xmlDocument.LoadXml(v_strObjMsg)
    '        v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")

    '        If v_nodeList.Count > 0 Then
    '            For i As Integer = 0 To v_nodeList.Count - 1
    '                For j As Integer = 0 To v_nodeList.Item(i).ChildNodes.Count - 1
    '                    With v_nodeList.Item(i).ChildNodes(j)
    '                        v_strValue = Trim(.InnerText.ToString)
    '                        v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
    '                        Select Case Trim(v_strFLDNAME)
    '                            Case "FLDNAME"
    '                                v_strField = v_strValue
    '                            Case "CAPTION"
    '                                v_strCaption = v_strValue
    '                        End Select
    '                    End With
    '                Next
    '                v_strDetail &= ", '#" & v_strField & ": ' || COL_DESC" & v_strField & " COL_" & v_strField
    '            Next
    '        End If

    '        v_strSQL = "SELECT * FROM (SELECT " & Mid(v_strDetail, 2) & " FROM TLLOG WHERE TXNUM='" & pv_strTXNUM & "')"
    '        v_strObjMsg1 = BuildXMLObjMsg(, , , , gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLTX, gc_ActionInquiry, v_strSQL)
    '        v_ws.Message(v_strObjMsg1)

    '        v_xmlDocument.LoadXml(v_strObjMsg1)
    '        v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")

    '        v_strDetail = ""

    '        If v_nodeList.Count > 0 Then
    '            For i As Integer = 0 To v_nodeList.Count - 1
    '                For j As Integer = 0 To v_nodeList.Item(i).ChildNodes.Count - 1
    '                    With v_nodeList.Item(i).ChildNodes(j)
    '                        v_strValue = Trim(.InnerText.ToString)
    '                        v_strDetail &= v_strValue & vbCrLf
    '                    End With
    '                Next
    '            Next
    '        End If

    '        v_xmlDocument.LoadXml(v_strObjMsg)
    '        v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")

    '        If v_nodeList.Count > 0 Then
    '            For i As Integer = 0 To v_nodeList.Count - 1
    '                For j As Integer = 0 To v_nodeList.Item(i).ChildNodes.Count - 1
    '                    With v_nodeList.Item(i).ChildNodes(j)
    '                        v_strValue = Trim(.InnerText.ToString)
    '                        v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
    '                        Select Case Trim(v_strFLDNAME)
    '                            Case "FLDNAME"
    '                                v_strField = v_strValue
    '                            Case "CAPTION"
    '                                v_strCaption = v_strValue
    '                        End Select
    '                    End With
    '                Next
    '                v_strDetail = Replace(v_strDetail, "#" & v_strField, v_strCaption)
    '            Next
    '        End If

    '        TextBox1.Text = v_strDetail
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Sub

End Class