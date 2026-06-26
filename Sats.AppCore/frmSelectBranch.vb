Public Class frmSelectBranch
    Private mv_strBRID As String
    Private mv_oDataSet As DataSet

    Public Property oDataSet() As DataSet
        Get
            Return mv_oDataSet
        End Get
        Set(ByVal value As DataSet)
            mv_oDataSet = value
        End Set
    End Property

    Public Property BranchID() As String
        Get
            Return mv_strBRID
        End Get
        Set(ByVal value As String)
            mv_strBRID = value
        End Set
    End Property


    Public Sub OnInit()
        cboBranch.BeginUpdate()
        cboBranch.DataSource = mv_oDataSet.Tables(0)
        cboBranch.DisplayMember = "DISPLAY"
        cboBranch.ValueMember = "VALUE"
        cboBranch.EndUpdate()

        cboBranch.SelectedValue = BranchID
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        mv_strBRID = cboBranch.SelectedValue
        Me.Close()
    End Sub
End Class