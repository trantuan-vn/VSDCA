Imports System
Imports System.Windows
Imports System.Windows.Forms

Public Class ProcessForm
    Private mv_frmProcessForm As frmProcess
    Private v_th As Threading.Thread
    Private ParentForm As Form
    Private mv_blnStart As Boolean

    Public Sub New(ByVal pv_ParentForm As Form)
        ParentForm = pv_ParentForm
    End Sub

    Public ReadOnly Property Status() As Boolean
        Get
            Return mv_blnStart
        End Get
    End Property

    Public Sub StartProcessForm()
        Try
            mv_frmProcessForm = New frmProcess
            mv_blnStart = False
            v_th = New Threading.Thread(AddressOf ShowForm)
            v_th.IsBackground = True
            v_th.Start()
            Threading.Thread.Sleep(100)
        Catch ex As Exception

        End Try
    End Sub

    Public Sub StopProcessForm()
        Try
            mv_frmProcessForm.Invoke(mv_frmProcessForm.mv_DeleClose)
            mv_frmProcessForm = Nothing
            mv_blnStart = True
            'v_th.Abort()
        Catch ex As Exception
          
        End Try
    End Sub

    Public Sub ChangeCaption(ByVal pv_strCaption As String)
        Try
            mv_frmProcessForm.Caption = pv_strCaption
            mv_frmProcessForm.Invoke(mv_frmProcessForm.mv_Delegate)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ShowForm()
        mv_frmProcessForm.ShowDialog()
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        If Not v_th Is Nothing Then
            v_th.Abort()
        End If
    End Sub
End Class
