Imports Sats.CommonLibrary
Imports Sats.CoreBusiness
Imports Sats.DataAccessLayer
'Imports System.EnterpriseServices

'<JustInTimeActivation(False), _
'Transaction(TransactionOption.Disabled), _
'ObjectPooling(Enabled:=True, MinPoolSize:=30)> _
Public Class HOSTReport
    Implements IDisposable
    'Inherits ServicedComponent
    Public Function objTransfer(ByRef pv_strObjMsg As String) As Long
        Dim v_xmlDocument As New Xml.XmlDocument
        v_xmlDocument.LoadXml(pv_strObjMsg)
        Dim v_attrColl As Xml.XmlAttributeCollection = v_xmlDocument.DocumentElement.Attributes
        Dim v_strMSGTYPE As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeMSGTYPE), Xml.XmlAttribute).Value)
        Try
            Select Case v_strMSGTYPE
                Case gc_MsgTypeRpt
                    'Lay danh sach bao cao
                    CoreInquiry(v_xmlDocument)
                Case gc_MsgTypeProc
                    'Lay du lieu bao cao
                    'CoreRptInquiry(v_xmlDocument)
            End Select

            pv_strObjMsg = v_xmlDocument.InnerXml
            'ContextUtil.SetComplete()
        Catch ex As Exception
            'ContextUtil.SetAbort()
        End Try
    End Function

    Public Function CoreInquiry(ByRef pv_xmlDocument As Xml.XmlDocument) As Long
        Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes


        Dim v_strLocal As String
        Dim v_strCmdInquiry As String
        Dim v_obj As DataAccess = Nothing

        Try
            'Lay ra cac tham so tu message
            If Not (v_attrColl.GetNamedItem("LOCAL") Is Nothing) Then
                v_strLocal = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeLOCAL), Xml.XmlAttribute).Value)
            Else
                v_strLocal = String.Empty
            End If

            If Not (v_attrColl.GetNamedItem("VALUE2") Is Nothing) Then
                v_strCmdInquiry = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCMDINQUIRY), Xml.XmlAttribute).Value)
            Else
                v_strCmdInquiry = String.Empty
            End If


            If v_strLocal = "Y" Then
                v_obj = New DataAccess
            ElseIf v_strLocal = "N" Then
                v_obj = New DataAccess
                v_obj.NewDBInstance(gc_MODULE_HOST)
            End If

            Dim v_ds As DataSet

            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strCmdInquiry)

            'Create data
            Dim v_dataElement As Xml.XmlElement, v_entryNode As Xml.XmlNode
            Dim i As Integer, j As Integer
            Dim v_attrFLDNAME As Xml.XmlAttribute, v_attrFLDTYPE As Xml.XmlAttribute

            For i = 0 To v_ds.Tables(0).Rows.Count - 1
                v_dataElement = pv_xmlDocument.CreateElement(Xml.XmlNodeType.Element, "ObjData", "")
                For j = 0 To v_ds.Tables(0).Columns.Count - 1
                    'Append entry to data node
                    v_entryNode = pv_xmlDocument.CreateNode(Xml.XmlNodeType.Element, "entry", "")

                    'Add field name
                    v_attrFLDNAME = pv_xmlDocument.CreateAttribute("fldname")
                    v_attrFLDNAME.Value = v_ds.Tables(0).Columns(j).ColumnName
                    v_entryNode.Attributes.Append(v_attrFLDNAME)

                    'Add field type
                    v_attrFLDTYPE = pv_xmlDocument.CreateAttribute("fldtype")
                    v_attrFLDTYPE.Value = v_ds.Tables(0).Columns(j).DataType.ToString
                    v_entryNode.Attributes.Append(v_attrFLDTYPE)

                    'Set value
                    If IsDBNull((v_ds.Tables(0).Rows(i)(j))) Then
                        v_entryNode.InnerText = ""
                    Else
                        If v_ds.Tables(0).Rows(i)(j).GetType.Name = GetType(System.DateTime).Name Then
                            v_entryNode.InnerText = Format(v_ds.Tables(0).Rows(i)(j), gc_FORMAT_DATE)
                        Else
                            v_entryNode.InnerText = CStr(v_ds.Tables(0).Rows(i)(j))
                        End If
                    End If

                    v_dataElement.AppendChild(v_entryNode)
                Next
                pv_xmlDocument.DocumentElement.AppendChild(v_dataElement)
            Next

            v_ds.Dispose()
            'ContextUtil.SetComplete()
        Catch ex As Exception
            'ContextUtil.SetAbort()
        End Try
    End Function

    'Public Function CoreRptInquiry(ByRef pv_xmlDocument As Xml.XmlDocument) As Long
    '    Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes

    '    'Dim v_strClause As String
    '    Dim v_strLocal As String
    '    'Dim v_strCmdInquiry As String
    '    'Dim v_strObjMsg As String
    '    Dim v_strStoreProc As String
    '    Dim v_intNumOfParam As Integer
    '    Dim v_strName, v_strValue, v_strSize, v_strType As String
    '    Dim v_objRptParam As ReportParameters
    '    Dim v_arrRptPara() As ReportParameters
    '    Dim v_obj As DataAccess = Nothing


    '    Try
    '        'Lay ra cac tham so tu message
    '        If Not (v_attrColl.GetNamedItem(gc_AtributeLOCAL) Is Nothing) Then
    '            v_strLocal = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeLOCAL), Xml.XmlAttribute).Value)
    '        Else
    '            v_strLocal = String.Empty
    '        End If
    '        If Not (v_attrColl.GetNamedItem("STOREPROC") Is Nothing) Then
    '            v_strStoreProc = CStr(CType(v_attrColl.GetNamedItem("STOREPROC"), Xml.XmlAttribute).Value)
    '        Else
    '            v_strStoreProc = String.Empty
    '        End If
    '        If Not (v_attrColl.GetNamedItem("NUM_OF_PARAM") Is Nothing) Then
    '            v_intNumOfParam = CStr(CType(v_attrColl.GetNamedItem("NUM_OF_PARAM"), Xml.XmlAttribute).Value)
    '        Else
    '            v_intNumOfParam = String.Empty
    '        End If

    '        Dim v_index As Integer
    '        ReDim v_arrRptPara(v_intNumOfParam - 1)

    '        For v_index = 0 To v_intNumOfParam - 1
    '            If Not (v_attrColl.GetNamedItem("PARAMNAME" & v_index.ToString) Is Nothing) Then
    '                v_strName = CStr(CType(v_attrColl.GetNamedItem("PARAMNAME" & v_index.ToString), Xml.XmlAttribute).Value)
    '            Else
    '                v_strName = String.Empty
    '            End If
    '            If Not (v_attrColl.GetNamedItem("PARAMVALUE" & v_index.ToString) Is Nothing) Then
    '                v_strValue = CStr(CType(v_attrColl.GetNamedItem("PARAMVALUE" & v_index.ToString), Xml.XmlAttribute).Value)
    '            Else
    '                v_strValue = String.Empty
    '            End If
    '            If Not (v_attrColl.GetNamedItem("PARAMSIZE" & v_index.ToString) Is Nothing) Then
    '                v_strSize = CStr(CType(v_attrColl.GetNamedItem("PARAMSIZE" & v_index.ToString), Xml.XmlAttribute).Value)
    '            Else
    '                v_strSize = String.Empty
    '            End If
    '            If Not (v_attrColl.GetNamedItem("PARAMTYPE" & v_index.ToString) Is Nothing) Then
    '                v_strType = CStr(CType(v_attrColl.GetNamedItem("PARAMTYPE" & v_index.ToString), Xml.XmlAttribute).Value)
    '            Else
    '                v_strType = String.Empty
    '            End If
    '            v_objRptParam = New ReportParameters
    '            v_objRptParam.ParamName = v_strName
    '            v_objRptParam.ParamValue = v_strValue
    '            v_objRptParam.ParamSize = CInt(v_strSize)
    '            v_objRptParam.ParamType = v_strType
    '            v_arrRptPara(v_index) = v_objRptParam
    '        Next

    '        If v_strLocal = "Y" Then
    '            v_obj = New DataAccess
    '        ElseIf v_strLocal = "N" Then
    '            v_obj = New DataAccess
    '            v_obj.NewDBInstance(gc_MODULE_HOST)
    '        End If

    '        Dim v_ds As DataSet
    '        'Dim v_strSQL As String

    '        ' lay ra dataset
    '        v_ds = v_obj.ExecuteStoredReturnDataset(v_strStoreProc, v_arrRptPara)

    '        'Create data
    '        Dim v_dataElement As Xml.XmlElement, v_entryNode As Xml.XmlNode
    '        Dim i As Integer, j As Integer
    '        Dim v_attrFLDNAME As Xml.XmlAttribute, v_attrFLDTYPE As Xml.XmlAttribute

    '        For i = 0 To v_ds.Tables(0).Rows.Count - 1
    '            v_dataElement = pv_xmlDocument.CreateElement(Xml.XmlNodeType.Element, "ObjData", "")
    '            For j = 0 To v_ds.Tables(0).Columns.Count - 1
    '                'Append entry to data node
    '                v_entryNode = pv_xmlDocument.CreateNode(Xml.XmlNodeType.Element, "entry", "")

    '                'Add field name
    '                v_attrFLDNAME = pv_xmlDocument.CreateAttribute("fldname")
    '                v_attrFLDNAME.Value = v_ds.Tables(0).Columns(j).ColumnName
    '                v_entryNode.Attributes.Append(v_attrFLDNAME)

    '                'Add field type
    '                v_attrFLDTYPE = pv_xmlDocument.CreateAttribute("fldtype")
    '                v_attrFLDTYPE.Value = v_ds.Tables(0).Columns(j).DataType.ToString
    '                v_entryNode.Attributes.Append(v_attrFLDTYPE)

    '                'Set value
    '                If IsDBNull((v_ds.Tables(0).Rows(i)(j))) Then
    '                    v_entryNode.InnerText = ""
    '                Else
    '                    If v_ds.Tables(0).Rows(i)(j).GetType.Name = GetType(System.DateTime).Name Then
    '                        v_entryNode.InnerText = Format(v_ds.Tables(0).Rows(i)(j), gc_FORMAT_DATE)
    '                    Else
    '                        v_entryNode.InnerText = CStr(v_ds.Tables(0).Rows(i)(j))
    '                    End If
    '                End If

    '                v_dataElement.AppendChild(v_entryNode)
    '            Next
    '            pv_xmlDocument.DocumentElement.AppendChild(v_dataElement)
    '        Next

    '        v_ds.Dispose()
    '        ContextUtil.SetComplete()
    '    Catch ex As Exception
    '        ContextUtil.SetAbort()
    '    End Try
    'End Function
#Region " IDisposable Support "
    Private disposedValue As Boolean = False        ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: free other state (managed objects).
            End If

            ' TODO: free your own state (unmanaged objects).
            ' TODO: set large fields to null.
        End If
        Me.disposedValue = True
    End Sub
    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

#End Region
End Class