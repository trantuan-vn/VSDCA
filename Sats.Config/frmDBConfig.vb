Imports Microsoft.VisualBasic
Imports System
Imports System.Text
Imports System.CodeDom.Compiler
Imports System.Reflection
Imports System.IO
Imports System.Diagnostics
Imports System.Windows.Forms
Imports Sats.CommonLibrary

Public Class frmDbConfig
    Private m_oCompilerErrors As CompilerErrorCollection
    Private m_oCompiledResult As CompilerResults
    Private sb As StringBuilder

    Public Sub DoComplile()
        Dim oCodeProvider As VBCodeProvider = New VBCodeProvider
        Dim oCParams As CompilerParameters = New CompilerParameters
        Dim oCResults As CompilerResults
        Try
            Dim strPath As String = GetCurrentDirectory()

            'Tao string chua code
            sb = New StringBuilder("")
            sb.Append("Imports Sats.CommonLibrary" & vbCrLf)
            sb.Append("Namespace Sats.DBConfig" & vbCrLf)
            sb.Append("Public Class DBConfig " & vbCrLf)
            sb.Append("Implements IDBConfig " & vbCrLf)
            sb.Append("Public Const VSDS_HOST_DSN As String = """ & EncryptString(gc_ENCRYPT_PASSWORD, txtOnlineDataSource.Text.Trim()) & """" & vbCrLf)
            sb.Append("Public Const VSDS_HOST_UID As String = """ & EncryptString(gc_ENCRYPT_PASSWORD, txtOnlineUser.Text.Trim()) & """" & vbCrLf)
            sb.Append("Public Const VSDS_HOST_PWD As String = """ & EncryptString(gc_ENCRYPT_PASSWORD, txtOnlinePassword.Text.Trim()) & """" & vbCrLf)
            sb.Append("Public Const VSDS_INQUERY_DSN As String = """ & EncryptString(gc_ENCRYPT_PASSWORD, txtInquiryDataSource.Text.Trim()) & """" & vbCrLf)
            sb.Append("Public Const VSDS_INQUERY_UID As String = """ & EncryptString(gc_ENCRYPT_PASSWORD, txtInquiryUser.Text.Trim()) & """" & vbCrLf)
            sb.Append("Public Const VSDS_INQUERY_PWD As String = """ & EncryptString(gc_ENCRYPT_PASSWORD, txtInquiryPassword.Text.Trim()) & """" & vbCrLf)
            sb.Append("Function GetHostConfig() As DBInfo Implements IDBConfig.GetHostConfig" & vbCrLf)
            sb.Append("Return New DBInfo(VSDS_HOST_UID, VSDS_HOST_PWD, VSDS_HOST_DSN)" & vbCrLf)
            sb.Append("End Function" & vbCrLf)
            sb.Append("Function GetInQueryConfig() As DBInfo Implements IDBConfig.GetInQueryConfig" & vbCrLf)
            sb.Append("Return New DBInfo(VSDS_INQUERY_UID, VSDS_INQUERY_PWD, VSDS_INQUERY_DSN)" & vbCrLf)
            sb.Append("End Function" & vbCrLf)
            sb.Append("Public Sub New()" & vbCrLf)
            sb.Append("End Sub" & vbCrLf)

            'Tao tham so compile
            oCParams.CompilerOptions = "/target:library /optimize"
            oCParams.GenerateInMemory = False
            oCParams.GenerateExecutable = False
            oCParams.IncludeDebugInformation = False
            oCParams.ReferencedAssemblies.Add("Sats.CommonLibrary.dll")
            oCParams.OutputAssembly = strPath & "\" & "Sats.DBConfig.dll"
            sb.Append("End Class " & vbCrLf)
            sb.Append("End Namespace" & vbCrLf)
            oCResults = oCodeProvider.CompileAssemblyFromSource(oCParams, sb.ToString)

            If oCResults.Errors.HasErrors Then
                Dim v_strErr As String = ""
                For i As Integer = 0 To oCResults.Errors.Count - 1
                    If Not oCResults.Errors(i).IsWarning Then
                        v_strErr &= oCResults.Errors(i).ErrorText & vbCrLf
                    End If
                Next
                MessageBox.Show(v_strErr)
            Else
                MessageBox.Show("Thực hiện thành công!")
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Function GetCurrentDirectory()
        Try
            Return (New FileInfo(Assembly.GetExecutingAssembly().Location)).Directory.FullName
        Catch ex As Exception
            Return Directory.GetCurrentDirectory()
        End Try
    End Function

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        DoComplile()
    End Sub
End Class