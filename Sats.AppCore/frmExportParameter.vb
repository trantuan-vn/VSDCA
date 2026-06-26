Public Class frmExportParameter
    Dim mv_intExportType As Integer
    Dim mv_intRowFrom As Int32
    Dim mv_intRowTo As Int32
    Dim mv_intMaxRowSearch As Int32


    Public Property ExportType() As Integer
        Get
            Return mv_intExportType
        End Get
        Set(ByVal value As Integer)
            mv_intExportType = value
        End Set
    End Property
    Public Property RowFrom() As Int32
        Get
            Return mv_intRowFrom
        End Get
        Set(ByVal value As Int32)
            mv_intRowFrom = value
        End Set
    End Property

    Public Property RowTo() As Int32
        Get
            Return mv_intRowTo
        End Get
        Set(ByVal value As Int32)
            mv_intRowTo = value
        End Set
    End Property
    Public Property MaxRowSearch() As Int32
        Get
            Return mv_intMaxRowSearch
        End Get
        Set(ByVal value As Int32)
            mv_intMaxRowSearch = value
        End Set
    End Property

    Public Sub OnInit()
        Try
            rbGridExport.Select()
            txtRowFrom.Text = "1"
            txtRowTo.Text = MaxRowSearch.ToString
            grbInfo.Enabled = False
        Catch ex As Exception
            Throw ex
        End Try
    End Sub


    Private Sub frmExportParameter_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        OnInit()
    End Sub

    Private Sub rbDbExport_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbDbExport.CheckedChanged
        If rbDbExport.Checked Then
            grbInfo.Enabled = True
        End If
    End Sub

    Private Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        If rbGridExport.Checked Then
            ExportType = 0
            RowFrom = 0
            RowTo = 0
        Else
            ExportType = 1
            RowFrom = CInt(txtRowFrom.Text)
            RowTo = CInt(txtRowTo.Text)
            If RowTo - RowFrom < 0 Or RowTo - RowFrom > 65000 Then
                MsgBox("Số thứ tự đến dòng phải lớn hơn số thứ tự từ dòng và số dòng kết xuất phải nhỏ hơn hoặc bằng 65000 dòng!", MsgBoxStyle.OkOnly, "Thông báo")
                Exit Sub
            End If
        End If
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        ExportType = -1
        RowFrom = -1
        RowTo = -1
        Me.Close()
    End Sub

    Private Sub txtRowFrom_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtRowFrom.Validating
        If Trim(txtRowFrom.Text) = "" Then
            MsgBox("Số thứ tự dòng phải là số nguyên lớn hơn 0!", MsgBoxStyle.OkOnly, "Thông báo")
            txtRowFrom.Text = 1
            txtRowFrom.Focus()
            Exit Sub
        Else
            If IsNumeric(Trim(txtRowFrom.Text)) Then
                If CInt(txtRowFrom.Text) < 0 Then
                    MsgBox("Số thứ tự dòng phải là số nguyên lớn hơn 0!", MsgBoxStyle.OkOnly, "Thông báo")
                    txtRowFrom.Text = 1
                    txtRowFrom.Focus()
                    Exit Sub
                Else
                    RowFrom = CInt(Trim(txtRowFrom.Text))
                    Exit Sub
                End If
            Else
                MsgBox("Số thứ tự dòng phải là số nguyên lớn hơn 0!", MsgBoxStyle.OkOnly, "Thông báo")
                txtRowFrom.Text = 1
                txtRowFrom.Focus()
                Exit Sub
            End If
        End If
    End Sub

    Private Sub txtRowTo_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtRowTo.Validating
        If Trim(txtRowTo.Text) = "" Then
            MsgBox("Số thứ tự dòng phải là số nguyên lớn hơn 0!", MsgBoxStyle.OkOnly, "Thông báo")
            txtRowTo.Text = MaxRowSearch
            txtRowTo.Focus()
            Exit Sub
        Else
            If IsNumeric(Trim(txtRowTo.Text)) Then
                If CInt(txtRowTo.Text) < 0 Then
                    MsgBox("Số thứ tự dòng phải là số nguyên lớn hơn 0!", MsgBoxStyle.OkOnly, "Thông báo")
                    txtRowTo.Text = MaxRowSearch
                    txtRowTo.Focus()
                    Exit Sub
                Else
                    RowTo = CInt(Trim(txtRowFrom.Text))
                    Exit Sub
                End If
            Else
                MsgBox("Số thứ tự dòng phải là số nguyên lớn hơn 0!", MsgBoxStyle.OkOnly, "Thông báo")
                txtRowTo.Text = MaxRowSearch
                txtRowTo.Focus()
                Exit Sub
            End If
        End If
    End Sub
End Class