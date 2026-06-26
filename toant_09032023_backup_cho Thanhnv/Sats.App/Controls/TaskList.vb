Public Class TaskList

    Private Sub TaskList_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.TabText = m_ResourceManager.GetString("TaskListCaption")
    End Sub

    Private Sub TaskList_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        cbTaskList.Top = 5
        txtTaskContent.Top = cbTaskList.Height + 8

        cbTaskList.Left = 5
        txtTaskContent.Left = 5

        cbTaskList.Width = Me.Width - 10
        txtTaskContent.Width = Me.Width - 10

        txtTaskContent.Height = Me.Height - cbTaskList.Height - 10
    End Sub

End Class
