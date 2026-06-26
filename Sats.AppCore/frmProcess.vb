Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Drawing
Imports System.Threading
Imports System.Windows.Forms


Public NotInheritable Class frmProcess

    Delegate Sub ShowMessage()
    Delegate Sub FormClose()
    Public mv_Delegate As ShowMessage
    Public mv_DeleClose As FormClose
    Private mv_bln As Boolean
    Private mv_intCount As Integer = 0
    Private mv_strCaption As String

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        mv_Delegate = New ShowMessage(AddressOf ShowMsg)
        mv_DeleClose = New FormClose(AddressOf CloseForm)
    End Sub

    Public Property ProcessOK() As Boolean
        Get
            Return mv_bln
        End Get
        Set(ByVal value As Boolean)
            mv_bln = value
        End Set
    End Property

    Public Property Caption() As String
        Get
            Return mv_strCaption
        End Get
        Set(ByVal value As String)
            mv_strCaption = value
        End Set
    End Property

    Private Sub ShowMsg()
        Try
            lbl.Text = Caption
        Catch ex As Exception

        End Try
    End Sub

    Private Sub CloseForm()
        Try
            Me.Close()
        Catch ex As Exception

        End Try
    End Sub
  
End Class
