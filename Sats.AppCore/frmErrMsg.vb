Imports Sats.CommonLibrary
Public Class frmErrMsg
    Private mv_strErrMsg As String
    Private mv_strWarningMsg As String
    Private mv_strErrCount As String
    Private mv_strWarningCount As String

    Public Property ErrMsg() As String
        Get
            Return mv_strErrMsg
        End Get
        Set(ByVal Value As String)
            mv_strErrMsg = Value
        End Set
    End Property
    Public Property WarningMsg() As String
        Get
            Return mv_strWarningMsg
        End Get
        Set(ByVal Value As String)
            mv_strWarningMsg = Value
        End Set
    End Property
    Public Property ErrCount() As String
        Get
            Return mv_strErrCount
        End Get
        Set(ByVal Value As String)
            mv_strErrCount = Value
        End Set
    End Property
    Public Property WarningCount() As String
        Get
            Return mv_strWarningCount
        End Get
        Set(ByVal Value As String)
            mv_strWarningCount = Value
        End Set
    End Property


    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        OnExport
    End Sub
    Protected Overridable Function OnExport() As Int32
        Try
            Dim v_dlgSave As New Windows.Forms.SaveFileDialog
            v_dlgSave.Filter = "Text files (*.txt)|*.txt|Excel files (*.xls)|*.xls|All files (*.*)|*.*"

            Dim v_res As Windows.Forms.DialogResult = v_dlgSave.ShowDialog(Me)
            If v_res = Windows.Forms.DialogResult.OK Then
                Dim v_strFileName As String = v_dlgSave.FileName
                Dim v_streamWriter As New System.IO.StreamWriter(v_strFileName, False, System.Text.Encoding.Unicode)

                v_streamWriter.WriteLine("STT" & vbTab & "Mô tả lỗi")

                For i As Integer = 0 To txtErrMsg.Lines.GetLength(0) - 1
                    v_streamWriter.WriteLine(i & vbTab & txtErrMsg.Lines(i))
                Next
                'Close StreamWriter
                v_streamWriter.Close()

            End If
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                                     & "Error code: System error!" & vbNewLine _
                                                     & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(gc_ERR_MSG_VN, MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Function

    Private Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        'If MsgBox("Bạn có muốn lưu kết quả lỗi ?", MsgBoxStyle.Information + MsgBoxStyle.OkCancel, "Thông báo") = MsgBoxResult.Ok Then
        'OnExport()
        'End If
        Me.Close()
    End Sub

    Private Sub frmErrMsg_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        txtErrMsg.Text = mv_strErrMsg
        txtWarningMsg.Text = mv_strWarningMsg
        tbMessages.TabPages(0).Text = mv_strErrCount & " lỗi"
        tbMessages.TabPages(1).Text = mv_strWarningCount & " cảnh báo"
        If mv_strWarningCount <> "0" Then
            tbMessages.TabPages(1).Select()
        End If
        If mv_strErrCount <> "0" Then
            tbMessages.TabPages(0).Select()
        End If
        btnOK.Enabled = (mv_strErrMsg = "")
        'btnOK.Enabled = (mv_strErrCount = "0")
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If MsgBox("Bạn đã chắc chắn muốn bỏ qua các cảnh báo trên để thực hiện giao dịch không ?", MsgBoxStyle.YesNo + MsgBoxStyle.Exclamation, "Cảnh báo") = MsgBoxResult.Yes Then
            Me.Close()
        Else
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
        End If
    End Sub
End Class