Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document
Imports Sats.BDSChannel
Imports Sats.CommonLibrary

Public Class rptMF001
    Inherits DataDynamics.ActiveReports.ActiveReport3
    Private WithEvents mv_rptReportHeader As ReportHeader
    Private mv_oRTF As RichTextBox
    Private mv_objPageSetting As ReportSetting


    Public Sub New(ByVal v_ReportSetting As ReportSetting)

        ' This call is required by the Windows Form Designer.
        Dim v_oDir As New ApplicationServices.ApplicationBase

        'InitializeComponent()
        Me.LoadLayout((v_oDir.Info.DirectoryPath & "\Reports\Header\" & v_ReportSetting.ReportID & ".rpx"))

        ' Add any initialization after the InitializeComponent() call.
        mv_objPageSetting = v_ReportSetting
        'OnInit()

    End Sub

    'Private Sub OnInit()
    '    'CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
    '    'mv_rptReportHeader = New ReportHeader
    '    Try
    '        'mv_rptReportHeader = New ReportHeader
    '        'Dim v_oDir As New ApplicationServices.ApplicationBase
    '        'CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()

    '        'Me.LoadLayout(v_oDir.Info.DirectoryPath & "\Reports\Header\" & mv_objPageSetting.ReportID & ".rpx")
    '        'Me.Sections.Add(Me.mv_rptReportHeader)
    '        'CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()

    '        'AddHandler Me.Detail1.Format, AddressOf Header_Format
    '        'CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

    '        ' mv_rptDetail.KeepTogether = True
    '        'Me.Sections.Add(Me.mv_rptReportFooter)
    '        'v_oRpt = New ActiveReport3
    '        'CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Sub

    'Public Sub Header_Format(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Try
    '        Dim v_oField As Field
    '        Dim v_strFieldName As String
    '        For Each v_oField In Me.Fields
    '            If Mid(v_oField.Name, 1, 3).ToUpper = "RM_" Then
    '                v_strFieldName = "_" & v_oField.Name
    '                mv_oRTF.ReplaceField(v_strFieldName, ReadMoney(v_oField.Value.ToString))
    '                mv_oRTF.ReplaceField(v_oField.Name, FormatNumber(v_oField.Value.ToString, 0))
    '            ElseIf Mid(v_oField.Name, 1, 3).ToUpper = "RN_" Then
    '                v_strFieldName = "_" & v_oField.Name
    '                mv_oRTF.ReplaceField(v_strFieldName, ReadNumber(v_oField.Value.ToString))
    '                mv_oRTF.ReplaceField(v_oField.Name, FormatNumber(v_oField.Value.ToString, 0))
    '            Else
    '                mv_oRTF.ReplaceField(v_oField.Name, gf_CorrectStringField(v_oField.Value))
    '            End If

    '        Next
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Sub

    Private Sub GroupHeader1_Format(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            GroupHeader1.AddBookmark(txtMINAME.Text)
        Catch ex As Exception

        End Try
    End Sub

   
    Private Sub GroupHeader1_Format_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GroupHeader1.Format

    End Sub

    Private Sub rptMF001_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart

    End Sub
End Class

