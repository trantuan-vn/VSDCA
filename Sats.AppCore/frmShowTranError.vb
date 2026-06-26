Imports System
Imports System.Drawing
Imports Sats.CommonLibrary
Imports Xceed.Grid

Public Class frmShowTranError
    Private mv_oErrMsg As Xml.XmlNodeList

    Private mv_strErrCount As String
    Private mv_strWarningCount As String
    Private mv_strPublicMsg As String
    Private v_hError, v_hWarning As Hashtable
    Private v_hTran As Hashtable
    Private mv_strTran As String

    Public ReadOnly Property TranMessage() As String
        Get
            Return mv_strTran
        End Get
    End Property

    Public Property ErrMsg() As Xml.XmlNodeList
        Get
            Return mv_oErrMsg
        End Get
        Set(ByVal value As Xml.XmlNodeList)
            mv_oErrMsg = value
        End Set
    End Property

    Public Property PublicMessage() As String
        Get
            Return mv_strPublicMsg
        End Get
        Set(ByVal value As String)
            mv_strPublicMsg = value
        End Set
    End Property

    Public Sub OnInit()
        Try
            Dim myFont As New Font("Arial", 9, FontStyle.Bold, GraphicsUnit.Point)
            Dim myColor As Color = Color.Black
            Dim v_arrMsg() As String = mv_strPublicMsg.Split("#")
            Dim v_strSuccMsg, v_strErrMsg, v_strMsg As String
            Dim v_lngPos As Long = 0
            v_strMsg = v_arrMsg(0)
            v_strSuccMsg = v_arrMsg(1)
            v_strErrMsg = v_arrMsg(2)

            If v_strErrMsg <> "" Then
                rtb.Select(v_lngPos, 0)
                rtb.SelectionFont = myFont
                rtb.SelectionColor = myColor
                rtb.SelectedText = v_strMsg
                v_lngPos += v_strMsg.Length
            End If

            If v_strSuccMsg <> "" Then
                myFont = New Font("Arial", 9, FontStyle.Regular, GraphicsUnit.Point)
                myColor = Color.Blue
                rtb.Select(v_lngPos, 0)
                rtb.SelectionFont = myFont
                rtb.SelectionColor = myColor
                rtb.SelectedText = v_strSuccMsg
                v_lngPos += v_strSuccMsg.Length
            End If

            If v_strErrMsg <> "" Then
                myFont = New Font("Arial", 9, FontStyle.Regular, GraphicsUnit.Point)
                myColor = Color.Red
                rtb.Select(v_lngPos, 0)
                rtb.SelectionFont = myFont
                rtb.SelectionColor = myColor
                rtb.SelectedText = v_strErrMsg
                v_lngPos += v_strErrMsg.Length
            End If
            FillList()
            btnOK.Enabled = False
        Catch ex As Exception

        End Try
    End Sub

    Private Sub FillList()
        Dim v_strCount As String
        Dim v_strErrMsg, v_strWarningMsg As String
        Dim v_intErr, v_intWarning As Integer
        Dim v_strTLTXCD, v_strTXNUM, v_strTXDATE As String
        Dim v_strValue, v_strFLDNAME As String

        v_strErrMsg = ""
        v_strWarningMsg = ""
        v_intErr = 0
        v_intWarning = 0
        v_hError = New Hashtable
        v_hWarning = New Hashtable
        v_hTran = New Hashtable

        For i = 0 To mv_oErrMsg.Count - 1
            v_strTLTXCD = CStr(CType(mv_oErrMsg.Item(i).Attributes.GetNamedItem(gc_AtributeTLTXCD), Xml.XmlAttribute).Value)
            v_strTXNUM = CStr(CType(mv_oErrMsg.Item(i).Attributes.GetNamedItem(gc_AtributeTXNUM), Xml.XmlAttribute).Value)
            v_strTXDATE = CStr(CType(mv_oErrMsg.Item(i).Attributes.GetNamedItem(gc_AtributeTXDATE), Xml.XmlAttribute).Value)
            v_strErrMsg = ""
            v_strWarningMsg = ""
            For j = 0 To mv_oErrMsg.Item(i).ChildNodes.Count - 1
                With mv_oErrMsg.Item(i).ChildNodes(j)
                    v_strValue = .InnerText.ToString
                    v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem(gc_AtributeFLDNAME), Xml.XmlAttribute).Value)
                    v_strCount = CStr(CType(.Attributes.GetNamedItem("COUNT"), Xml.XmlAttribute).Value)
                    If v_strValue.Trim <> "" Then
                        If v_strFLDNAME = "ERROR_MESSAGE" Then
                            v_strErrMsg &= .InnerText.ToString & vbCrLf
                            v_intErr += CInt(v_strCount)
                        ElseIf v_strFLDNAME = "WARNING_MESSAGE" Then
                            v_strWarningMsg &= .InnerText.ToString & vbCrLf
                            v_intWarning += CInt(v_strCount)
                        End If
                    End If
                End With
            Next
            If v_strErrMsg <> "" Then
                v_hError.Add(" + " & v_strTLTXCD & "(" & v_strTXNUM & " - " & v_strTXDATE & ")", v_strErrMsg)
                lstTranError.Items.Add(" + " & v_strTLTXCD & "(" & v_strTXNUM & " - " & v_strTXDATE & ")")
            End If

            If v_strWarningMsg <> "" Then
                v_hWarning.Add(" + " & v_strTLTXCD & "(" & v_strTXNUM & " - " & v_strTXDATE & ")", v_strWarningMsg)
                v_hTran.Add(" + " & v_strTLTXCD & "(" & v_strTXNUM & " - " & v_strTXDATE & ")", v_strTLTXCD & "|" & v_strTXNUM & "|" & v_strTXDATE)
                lstWarning.Items.Add(" + " & v_strTLTXCD & "(" & v_strTXNUM & " - " & v_strTXDATE & ")")
            End If
        Next
        tbErrors.Text = v_intErr & " lỗi"
        tbWarnings.Text = v_intWarning & " cảnh báo"
        If lstTranError.Items.Count > 0 Then
            lstTranError.SelectedIndex = 0
        End If
        If lstWarning.Items.Count > 0 Then
            lstWarning.SelectedIndex = 0
        End If
    End Sub

    Private Sub lstTranError_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstTranError.SelectedIndexChanged
        txtErrors.Clear()
        txtErrors.Text = v_hError(lstTranError.SelectedItem.ToString)
    End Sub

    Private Sub lstWarning_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstWarning.SelectedIndexChanged
        Try
            If lstWarning.CheckedItems.Count > 0 Then
                btnOK.Enabled = True
            Else
                btnOK.Enabled = False
            End If
            txtWarnings.Text = v_hWarning(lstWarning.SelectedItem.ToString)
        Catch ex As Exception

        End Try

    End Sub

    Private Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Try
            mv_strTran = ""
            Dim v_strWarning As String = "Bạn có thực sự muốn bỏ qua cảnh báo của giao dịch này hay không?"
            For v_int As Integer = 0 To lstWarning.CheckedItems.Count - 1
                mv_strTran &= v_hTran(lstWarning.CheckedItems(v_int).ToString) & "$"
                v_strWarning &= vbCrLf & lstWarning.CheckedItems(v_int).ToString
            Next

            If MsgBox(v_strWarning, MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, gc_ApplicationTitle) = MsgBoxResult.Yes Then
                Me.Close()
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class