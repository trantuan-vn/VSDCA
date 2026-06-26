Imports Sats.CommonLibrary
Public Class frmBatchMsg
    Private mv_strBusDate As String = String.Empty
    Private mv_strBranchId As String
    Public Property BusDate() As String
        Get
            Return mv_strBusDate
        End Get
        Set(ByVal Value As String)
            mv_strBusDate = Value
        End Set
    End Property
    Public Property BranchId() As String
        Get
            Return mv_strBranchId
        End Get
        Set(ByVal Value As String)
            mv_strBranchId = Value
        End Set
    End Property
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        If MsgBox("Bạn có muốn lưu kết quả các đợt phát hành ?", MsgBoxStyle.Information + MsgBoxStyle.OkCancel, "Thông báo") = MsgBoxResult.Ok Then
            OnExport()
        End If
        Me.Close()
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        OnExport()
    End Sub
    Protected Overridable Function OnExport() As Int32
        Try
            Dim v_dlgSave As New Windows.Forms.SaveFileDialog
            v_dlgSave.Filter = "Excel files (*.xls)|*.xls|Text files (*.txt)|*.txt|All files (*.*)|*.*"

            Dim v_res As Windows.Forms.DialogResult = v_dlgSave.ShowDialog(Me)
            If v_res = Windows.Forms.DialogResult.OK Then
                Dim v_strFileName As String = v_dlgSave.FileName
                Dim v_streamWriter As New System.IO.StreamWriter(v_strFileName, False, System.Text.Encoding.Unicode)

                v_streamWriter.WriteLine("Bắt đầu ngày " & mv_strBusDate & " - thị trường " & mv_strBranchId & vbCrLf)

                For i As Integer = 1 To txtErrMsg.Lines.GetLength(0) - 1
                    v_streamWriter.WriteLine(txtErrMsg.Lines(i))
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


    Private Sub frmBatchMsg_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If MsgBox("Bạn có muốn lưu kết quả các đợt phát hành ?", MsgBoxStyle.Information + MsgBoxStyle.OkCancel, "Thông báo") = MsgBoxResult.Ok Then
            OnExport()
        End If
    End Sub
End Class