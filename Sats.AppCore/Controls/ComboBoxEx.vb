Imports System.Windows.Forms.ComboBox
Imports System.Windows.Forms

Public Class ComboBoxEx
    Inherits System.Windows.Forms.ComboBox

    Dim ds As New DataSet
    Dim dr As DataRow

    Public Const COMBOBOX_TABLE As String = "COMBOBOX"
    Public Const DISPLAY_ITEM As String = "DISPLAY"
    Public Const VALUE_ITEM As String = "VALUE"
    Private mv_intSelectedIndex As Integer
    Private mv_intOldSelectedIndex As Integer

    Public Sub New()
        MyBase.New()
        BuildDataTables()
        MyBase.Items.Clear()
        Me.BeginUpdate()
        Me.DataSource = ds.Tables(COMBOBOX_TABLE)
        Me.DisplayMember = DISPLAY_ITEM
        Me.ValueMember = VALUE_ITEM
        Me.EndUpdate()
    End Sub

    Public Sub Clears()
        ds.Tables(0).Clear()
    End Sub

    Public Sub AddItems(ByVal ItemDisplay As String, ByVal ItemValue As Object)
        dr = CType(DataSource, DataTable).NewRow ' ds.Tables(0).NewRow
        'ds.Tables(0).Rows.Add(dr)
        CType(DataSource, DataTable).Rows.Add(dr)
        dr(DISPLAY_ITEM) = ItemDisplay
        dr(VALUE_ITEM) = ItemValue
    End Sub

    Private Sub BuildDataTables()
        Dim table As DataTable = New DataTable(COMBOBOX_TABLE)
        With table.Columns
            .Add(DISPLAY_ITEM, GetType(System.String))
            .Add(VALUE_ITEM, GetType(System.Object))
        End With
        ds.Tables.Add(table)
    End Sub

    Private Sub ComboBoxEx_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.DropDown
        mv_intOldSelectedIndex = CType(sender, ComboBoxEx).SelectedIndex
        mv_intSelectedIndex = mv_intOldSelectedIndex
    End Sub

    Private Sub ComboBoxEx_DropDownClosed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.DropDownClosed
        If CType(sender, ComboBoxEx).SelectedIndex = mv_intOldSelectedIndex Then
            CType(sender, ComboBoxEx).SelectedIndex = mv_intSelectedIndex
        End If
    End Sub

    Private Sub ComboBoxAutoComplete_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        Dim Index As Integer
        Dim Actual As String
        Dim Found As String
        Dim MatchFound As Boolean

        Try
            'If the contents of the combo have been removed then select the first entry in the combo box list.
            Me.Text = Me.Text.Trim
            If Me.Text.Length = 0 Then
                Me.SelectedIndex = -1
                Me.SelectAll()
                Return
            End If

            'If the backspace key was pressed then remove the last character that was typed in and try to find a match.
            'Note that the selected text from the last character typed in to the end of the combo text field will also be deleted.
            If e.KeyCode = Keys.Back Then
                Me.Text = Me.Text.Substring(0, Me.Text.Length - 1)
            End If

            ' Do nothing for some keys such as navigation keys.
            If ((e.KeyCode = Keys.Left) Or _
                (e.KeyCode = Keys.Right) Or _
                (e.KeyCode = Keys.Up) Or _
                (e.KeyCode = Keys.Down) Or _
                (e.KeyCode = Keys.PageUp) Or _
                (e.KeyCode = Keys.PageDown) Or _
                (e.KeyCode = Keys.Home) Or _
                (e.KeyCode = Keys.End) Or _
                (e.KeyCode = Keys.Tab) Or _
                (e.KeyCode = Keys.Tab And e.Shift)) Then
                Return
            End If
            Do
                ' Store the actual text that has been typed.
                Actual = Me.Text

                ' Find the first match for the typed value.
                Index = Me.FindString(Actual)
                ' Get the text of the first match.
                'if index > -1 then a match was found.
                If (Index > -1) Then
                    Found = Me.Items(Index).ToString()

                    ' Select this item from the list.
                    Me.SelectedIndex = Index

                    ' Select the portion of the text that was automatically added so that any additional typing will replace it.
                    If Actual.Length = Found.Length Then
                        Me.SelectionStart = 1
                        Me.SelectionLength = Found.Length
                    Else
                        Me.SelectionStart = Actual.Length
                        Me.SelectionLength = Found.Length
                    End If

                    MatchFound = True
                Else
                    'If there isn't a match and the text typed in is only one character or nothing then just select the first
                    'entry in the combo box.
                    If Actual.Length = 1 Or Actual.Length = 0 Then
                        Me.SelectedIndex = -1
                        Me.SelectAll()
                        MatchFound = True
                    Else
                        'if there isn't a match for the text typed in then remove the last character of the text typed in
                        'and try to find a match.
                        Me.SelectionStart = Actual.Length - 1
                        Me.SelectionLength = Actual.Length - 1
                        Me.Text = Me.Text.Substring(0, Me.Text.Length - 1)

                    End If
                End If
            Loop Until MatchFound

            If e.KeyCode = Keys.Enter Then
                SendKeys.Send("{Tab}")
                e.Handled = True
            End If
            mv_intSelectedIndex = Me.SelectedIndex
        Catch ex As Exception

        End Try
    End Sub

End Class
