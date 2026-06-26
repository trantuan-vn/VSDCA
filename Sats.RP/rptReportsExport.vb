Imports DataDynamics.ActiveReports
Imports DataDynamics.ActiveReports.Document
Imports Sats.BDSChannel
Imports Sats.CommonLibrary


Public Class rptReportsExport
    Inherits DataDynamics.ActiveReports.ActiveReport3

    Private WithEvents mv_rtpPageHearder As PageHeader
    Private WithEvents mv_rptPageFooter As PageFooter
    Private WithEvents mv_rptDetail As Detail
    Private WithEvents mv_rptReportHeader As ReportHeader
    Private WithEvents mv_rptReportFooter As ReportFooter
    Private WithEvents mv_rptGroupH As GroupHeader
    Private WithEvents mv_rptGroupF As GroupFooter
    Private mv_dblTop As Double = 0.0!

    Private mv_arrHeaderRow() As ReportHeaderRow
    Private mv_arrGroup() As ReportGroup
    Private mv_objPageSetting As ReportSetting
    Private mv_arrParam() As ReportParameters
    Private v_intRow As Integer
    Private mv_rtf As RichTextBox
    Private mv_DataSet As DataSet
    Private mv_dblPrintWidth As Double
    Private mv_dblLeft As Double
    Private mv_intRowNum As Integer = 0
    Private WithEvents lblRowNum As Label
    Private mv_strObjName As String
    Private mv_strFontSize As String
    Private mv_strPaperSize As String

    Private mv_strCompanyName As String
    Private mv_strAddress As String
    Private mv_strBranch As String

    Private mv_strMIName As String
    Private mv_strMIAddress As String
    Private mv_strMIPhone As String
    Private mv_strBUSINESS_NO As String
    Private mv_strIsSignCA As String
    Private mv_strWhereDate As String

    Private Const GC_CENTER = "P_CENTER"
    Private Const GC_COMPANY_NAME = "P_COMPANY_NAME"
    Private Const GC_ADDRESS = "P_COMPANY_ADDRESS"
    Private Const GC_BRANCH_NAME = "P_BRANCH_NAME"

    Private Const GC_MINAME = "P_MINAME"
    Private Const GC_MIADDRESS = "P_MIADDRESS"
    Private Const GC_MIPHONE = "P_MIPHONE"
    Private Const GC_BUSINESS_NO = "P_BUSINESS_NO"

    Private Const GC_WHERE_DATE = "P_WHERE_DATE"


    Public Sub New(ByVal v_ReportSetting As ReportSetting, ByVal pv_DataSet As DataSet)
        Try
            mv_objPageSetting = v_ReportSetting
            mv_arrHeaderRow = mv_objPageSetting.ReportFld
            mv_arrGroup = mv_objPageSetting.ReportGrp
            mv_DataSet = pv_DataSet
            mv_dblPrintWidth = SumWidth()
            mv_strObjName = mv_objPageSetting.ObjectName
            mv_strFontSize = v_ReportSetting.FontSize
            mv_strPaperSize = v_ReportSetting.PaperSize

            mv_strCompanyName = v_ReportSetting.CompanyName
            mv_strAddress = v_ReportSetting.Address
            mv_strBranch = v_ReportSetting.BranchName

            mv_strMIName = v_ReportSetting.MIName
            mv_strMIAddress = v_ReportSetting.MIAddress
            mv_strBUSINESS_NO = v_ReportSetting.BUSINESS_NO

            mv_strWhereDate = v_ReportSetting.WhereDate
            mv_strIsSignCA = v_ReportSetting.IsSignCA
            InitReport()
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                                     & "Error code: System error!" & vbNewLine _
                                                     & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(gc_ERR_MSG_VN, MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Sub

#Region "Private Function"

    Private Sub InitReport()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        mv_rtpPageHearder = New PageHeader
        mv_rptPageFooter = New PageFooter
        mv_rptReportHeader = New ReportHeader
        mv_rptReportFooter = New ReportFooter
        mv_rptDetail = New Detail

        PageSetting()

        Me.Sections.Add(Me.mv_rptReportHeader)
        Me.Sections.Add(Me.mv_rtpPageHearder)

        Dim v_intGroupCount As Integer
        'Dim v_grpH As GroupHeader
        'Dim v_grpF As GroupFooter
        Dim v_txt As TextBox
        Dim lbl As Label
        Dim ln As Line
        Dim v_dblLeft, v_dblPageWidth As Double

        If mv_objPageSetting.Orientation = 1 Then
            v_dblPageWidth = Me.PageSettings.PaperWidth
        Else
            v_dblPageWidth = Me.PageSettings.PaperHeight
        End If

        v_dblLeft = (v_dblPageWidth - mv_dblPrintWidth - 1) / 2
        v_dblLeft = System.Math.Round(v_dblLeft, 2) - 0.005
        mv_dblLeft = v_dblLeft

        mv_rptGroupH = New GroupHeader
        mv_rptGroupH.Name = "grpPublic"
        'mv_rptGroupH.DataField = .FieldName
        DrawHeader(mv_rptGroupH)
        DrowNumHeader(mv_objPageSetting.HeaderHeight, mv_rptGroupH)
        mv_rptGroupH.RepeatStyle = RepeatStyle.None
        mv_rptGroupH.Height = mv_objPageSetting.HeaderHeight + mv_objPageSetting.DataRowHeight
        Me.Sections.Add(mv_rptGroupH)

        If Not mv_arrGroup Is Nothing Then
            v_intGroupCount = mv_arrGroup.Count - 1
            For i As Integer = 0 To v_intGroupCount
                With mv_arrGroup(i)
                    mv_rptGroupH = New GroupHeader
                    mv_rptGroupH.Name = .FieldName
                    mv_rptGroupH.DataField = .FieldName
                    mv_rptGroupH.Tag = .Caption & "|" & .Footer
                    AddHandler mv_rptGroupH.Format, AddressOf mv_rptGroupH_Format
                    mv_rptGroupH.Height = mv_objPageSetting.DataRowHeight

                    v_txt = InitTextBox("grptxt" & .Caption, "", .Caption, .FldType, v_dblLeft + 0.0125, mv_objPageSetting.DataRowHeight, mv_dblPrintWidth - 0.025)

                    v_txt.Style = "ddo-char-set: 0; font-weight: bold;" _
                            & " font-size: " & mv_strFontSize & "pt; vertical-align: middle; font-style: italic; "
                    mv_rptGroupH.Controls.Add(v_txt)

                    ln = InitLine(v_dblLeft, mv_objPageSetting.DataRowHeight, 0.0, 0)
                    Me.mv_rptGroupH.Controls.Add(ln)

                    ln = InitLine(v_dblLeft + mv_dblPrintWidth, mv_objPageSetting.DataRowHeight, 0.0, 0)
                    Me.mv_rptGroupH.Controls.Add(ln)

                    ln = InitLine(v_dblLeft, 0.0, mv_objPageSetting.DataRowHeight, mv_dblPrintWidth)
                    Me.mv_rptGroupH.Controls.Add(ln)

                    Me.Sections.Add(mv_rptGroupH)
                End With
            Next
        End If
        Me.Sections.Add(Me.mv_rptDetail)
        Me.mv_rptDetail.Height = 0.0!
        If Not mv_arrGroup Is Nothing Then
            v_intGroupCount = mv_arrGroup.Count - 1
            For i As Integer = 0 To v_intGroupCount
                With mv_arrGroup(v_intGroupCount - i)
                    mv_rptGroupF = New GroupFooter
                    mv_rptGroupF.Name = .FieldName
                    If .Footer <> "" Then
                        mv_rptGroupF.Height = mv_objPageSetting.DataRowHeight

                        lbl = New Label
                        lbl.Left = v_dblLeft
                        lbl.Name = "grplbl" & .FieldName
                        'lbl.Text = .Footer
                        lbl.DataField = "F_" & .FieldName
                        lbl.Width = .CaptionWidth
                        lbl.Height = mv_objPageSetting.DataRowHeight
                        lbl.Style = "ddo-char-set: 0; font-weight: bold;" _
                            & " font-size: " & mv_strFontSize & "pt; vertical-align: middle; "

                        mv_rptGroupF.Controls.Add(lbl)

                        'v_txt = InitTextBox("grpftxt" & .Caption, "", .Caption, .FldType, v_dblLeft + .CaptionWidth, 0.25, mv_dblPrintWidth)
                        'mv_rptGroupF.Controls.Add(v_txt)

                        SumTextBoxInGroup(mv_rptGroupF, v_dblLeft)

                        ln = InitLine(mv_dblPrintWidth + v_dblLeft, mv_objPageSetting.DataRowHeight, 0, 0)
                        mv_rptGroupF.Controls.Add(ln)

                        ln = InitLine(v_dblLeft, 0, mv_objPageSetting.DataRowHeight, mv_dblPrintWidth)
                        mv_rptGroupF.Controls.Add(ln)

                        ln = InitLine(v_dblLeft, mv_objPageSetting.DataRowHeight, 0, 0)
                        mv_rptGroupF.Controls.Add(ln)
                    Else
                        mv_rptGroupF.Height = 0.0!
                    End If
                    Me.Sections.Add(mv_rptGroupF)
                End With
            Next
        End If

        mv_rptGroupF = New GroupFooter
        mv_rptGroupF.Name = "grpPublic"
        SumTextBoxInFooter(mv_rptGroupF, mv_dblLeft)
        Me.Sections.Add(mv_rptGroupF)

        mv_rptReportFooter.KeepTogether = True

        'Dim v_infPageNumber As ReportInfo
        'v_infPageNumber = New ReportInfo
        'v_infPageNumber.Left = mv_dblLeft
        'v_infPageNumber.Width = mv_dblPrintWidth
        'v_infPageNumber.Style = "ddo-char-set: 0; font-weight: bold;" _
        '                       & " font-size: 10pt; vertical-align: middle;"
        'v_infPageNumber.SummaryGroup = ""
        'v_infPageNumber.SummaryRunning = SummaryRunning.All
        'v_infPageNumber.FormatString = "Trang {PageNumber}/{PageCount}"
        'v_infPageNumber.Alignment = TextAlignment.Right
        'mv_rptPageFooter.Controls.Add(v_infPageNumber)

        Me.Sections.Add(Me.mv_rptPageFooter)
        Me.Sections.Add(Me.mv_rptReportFooter)
        InitHeader()
        ReportRowDetail()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
    End Sub

    Private Sub PageSetting()
        Me.MasterReport = False
        Me.PageSettings.DefaultPaperSize = True
        Me.PageSettings.Orientation = CInt(mv_objPageSetting.Orientation)

        '2023-04-04: KiemNH

        Me.Document.Printer.PrinterName = ""

        Select Case mv_strPaperSize
            Case "A4"
                Me.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4
            Case "A3"
                Me.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A3
        End Select

        'Select Case mv_strPaperSize
        '    Case "A4"
        '        Me.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Custom
        '        Me.PageSettings.PaperWidth = 8.26772!
        '        Me.PageSettings.PaperHeight = 11.69291!
        '    Case "A3"
        '        Me.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Custom
        '        Me.PageSettings.PaperWidth = 11.69291!
        '        Me.PageSettings.PaperHeight = 16.53543!
        'End Select

        '2023-04-04: KiemNH - End

        If mv_objPageSetting.Orientation = 1 Then
            Me.PrintWidth = Me.PageSettings.PaperWidth - 1
        Else
            Me.PrintWidth = Me.PageSettings.PaperHeight - 1.025
        End If
        Me.PageSettings.Margins.Bottom = 0.5!
        Me.PageSettings.Margins.Left = 0.5!
        Me.PageSettings.Margins.Top = 0.5!
        Me.PageSettings.Margins.Right = 0.5!
        Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial Unicode MS; font-style: normal; text-decoration: none; font-weight: norma" & _
                "l; font-size: " & mv_strFontSize & "pt; color: Black; ", "Normal"))
        'Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-style: normal; text-decoration: none; font-weight: norma" & _
        '        "l; font-size: " & mv_strFontSize & "pt; color: Black; ", "Normal"))
    End Sub

    Private Sub InitHeader()
        Try
            Dim v_dblPageWidth As Double
            'Dim v_streamRTF As System.IO.FileStream
            Dim v_strDir As String
            Dim v_oDir As New ApplicationServices.ApplicationBase
            v_strDir = v_oDir.Info.DirectoryPath

            If mv_objPageSetting.Orientation = 1 Then
                v_dblPageWidth = Me.PageSettings.PaperWidth - 1
            Else
                v_dblPageWidth = Me.PageSettings.PaperHeight - 1
            End If

            Dim rtf As RichTextBox

            'rtf = CreateRTF("rptHeader", mv_dblTop, 1.25, v_strDir & "\Reports\HPublic_" & mv_objPageSetting.Orientation & ".rtf")
            'mv_dblTop = mv_dblTop + 1.25
            'Me.mv_rptReportHeader.Controls.Add(rtf)
            rtf = CreateRTF("rptHeader", mv_dblTop, mv_objPageSetting.HHeight, v_strDir & "\Reports\Header\" & mv_objPageSetting.ReportID & ".rtf")
            rtf.AutoReplaceFields = True
            rtf.ReplaceField("Day", Mid(mv_objPageSetting.BusDate, 1, 2))
            rtf.ReplaceField("Month", Mid(mv_objPageSetting.BusDate, 4, 2))
            rtf.ReplaceField("Year", Mid(mv_objPageSetting.BusDate, 7, 4))
            rtf.ReplaceField("OBJNAME", mv_strObjName)

            rtf.ReplaceField(GC_CENTER, "TỔNG CÔNG TY LƯU KÝ")
            rtf.ReplaceField(GC_COMPANY_NAME, Mid(mv_strCompanyName, 10))
            If mv_strBranch = "" Then
                '    rtf.ReplaceField(GC_ADDRESS, "")
                rtf.ReplaceField(GC_BRANCH_NAME, "")
            Else
                'rtf.ReplaceField(GC_ADDRESS, mv_strAddress)
                rtf.ReplaceField(GC_BRANCH_NAME, mv_strBranch)
            End If

            rtf.ReplaceField(GC_MINAME, mv_strMIName)
            rtf.ReplaceField(GC_MIADDRESS, mv_strMIAddress)
            rtf.ReplaceField(GC_MIPHONE, mv_strMIPhone)
            rtf.ReplaceField(GC_BUSINESS_NO, mv_strBUSINESS_NO)
            rtf.ReplaceField(GC_WHERE_DATE, mv_strWhereDate)

            FillFormuleToRTF(rtf)
            Me.mv_rptReportHeader.Controls.Add(rtf)
            Me.mv_rptReportHeader.CanShrink = True
            Me.mv_rptReportHeader.CanGrow = True
            Me.mv_rptReportHeader.Height = mv_objPageSetting.HHeight + 1.25
            Me.mv_rtpPageHearder.Height = 0.0

            Dim v_dblTop As Double
            If Not mv_arrGroup Is Nothing Then
                If mv_arrGroup.Count > 0 Then
                    v_dblTop = mv_objPageSetting.DataRowHeight
                Else
                    v_dblTop = 0.0
                End If
            End If
            rtf = CreateRTF("rptFooter", v_dblTop, mv_objPageSetting.FHeight, v_strDir & "\Reports\Footer\" & mv_objPageSetting.ReportID & ".rtf")
            rtf.AutoReplaceFields = True

            Me.mv_rptPageFooter.Height = 0.0!
            rtf.ReplaceField("Day", Mid(mv_objPageSetting.BusDate, 1, 2))
            rtf.ReplaceField("Month", Mid(mv_objPageSetting.BusDate, 4, 2))
            rtf.ReplaceField("Year", Mid(mv_objPageSetting.BusDate, 7, 4))
            rtf.ReplaceField(GC_WHERE_DATE, mv_strWhereDate)
            FillFormuleToRTF(rtf)

            Me.mv_rptReportFooter.Controls.Add(rtf)
            Me.mv_rptReportFooter.Height = mv_objPageSetting.FHeight

            If Not mv_arrGroup Is Nothing Then
                If mv_arrGroup.Count > 0 Then
                    Me.mv_rptReportFooter.Height += mv_objPageSetting.DataRowHeight
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Function CreateRTF(ByVal pv_strName As String, ByVal pv_dblTop As Double, _
                              ByVal pv_dblHeight As Double, ByVal pv_strPath As String) As RichTextBox
        Try
            Dim v_rtf As New RichTextBox
            v_rtf.Left = mv_dblLeft
            v_rtf.Width = mv_dblPrintWidth
            v_rtf.Top = pv_dblTop
            v_rtf.Height = pv_dblHeight
            v_rtf.CanGrow = True
            v_rtf.CanShrink = False
            v_rtf.Clear()
            v_rtf.ForeColor = System.Drawing.Color.Black
            v_rtf.LoadFile(pv_strPath)
            v_rtf.MaxLength = 0
            v_rtf.Multiline = True
            Return v_rtf
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Private Function SumWidth() As Double
        Dim i As Integer
        Dim v_dblSum As Double
        v_dblSum = 0
        For i = 0 To mv_arrHeaderRow.Count - 1
            If mv_arrHeaderRow(i).IsDataField = "Y" Then
                v_dblSum = v_dblSum + CDbl(mv_arrHeaderRow(i).Width)
            End If
        Next
        Return v_dblSum
    End Function

    Private Sub FillFormuleToRTF(ByRef pv_RTF As RichTextBox)
        Dim v_dt As DataTable
        If Not mv_DataSet Is Nothing Then
            If mv_DataSet.Tables.IndexOf("F_" & mv_objPageSetting.ReportID) > -1 Then
                v_dt = mv_DataSet.Tables("F_" & mv_objPageSetting.ReportID)
                If v_dt.Rows.Count > 0 Then
                    For i As Integer = 0 To v_dt.Columns.Count - 1
                        pv_RTF.ReplaceField(v_dt.Columns(i).ColumnName, gf_CorrectStringField(v_dt.Rows(0)(i)))
                    Next
                End If
            End If
        End If
        Dim v_oField As Field
        For Each v_oField In Me.Fields
            pv_RTF.ReplaceField(v_oField.Name, v_oField.Value)
        Next
    End Sub

    Private Sub FillFormuleToRTFFooter(ByRef pv_RTF As RichTextBox)
        Dim v_dt As DataTable
        If Not mv_DataSet Is Nothing Then
            If mv_DataSet.Tables.IndexOf("F_" & mv_objPageSetting.ReportID) > -1 Then
                v_dt = mv_DataSet.Tables("F_" & mv_objPageSetting.ReportID)
                If v_dt.Rows.Count > 0 Then
                    Dim v_strValue As String
                    Dim v_arrValue() As String
                    For i As Integer = 0 To v_dt.Columns.Count - 1
                        v_strValue = gf_CorrectStringField(v_dt.Rows(0)(i))
                        v_arrValue = v_strValue.Split("-")
                        If v_arrValue.Count > 1 Then
                            pv_RTF.ReplaceField(v_dt.Columns(i).ColumnName & "_1", v_arrValue(0))
                            pv_RTF.ReplaceField(v_dt.Columns(i).ColumnName & "_2", v_arrValue(1))
                        Else
                            pv_RTF.ReplaceField(v_dt.Columns(i).ColumnName & "_1", v_arrValue(0))
                            pv_RTF.ReplaceField(v_dt.Columns(i).ColumnName & "_2", "")
                        End If
                        pv_RTF.ReplaceField(v_dt.Columns(i).ColumnName, gf_CorrectStringField(v_dt.Rows(0)(i)))
                    Next
                End If
            End If
            If mv_strIsSignCA = "1" Then
                pv_RTF.ReplaceField("SIGNCA", "* Thông báo/báo cáo đã được ký xác nhận bằng chữ ký số của người có thẩm quyền của VSDC")
                pv_RTF.ReplaceField("SIGNCA1", "")
            Else
                pv_RTF.ReplaceField("SIGNCA", " ")
                pv_RTF.ReplaceField("SIGNCA1", " ")

            End If
        End If
    End Sub

    Private Sub ReportRowHeader()
        Dim lbl As Label
        Dim v_dblLeft, v_dblHeight As Double
        Dim v_int As Integer
        Dim v_dblPageWidth As Double

        If mv_objPageSetting.Orientation = 1 Then
            v_dblPageWidth = Me.PageSettings.PaperWidth
        Else
            v_dblPageWidth = Me.PageSettings.PaperHeight
        End If

        v_dblLeft = (v_dblPageWidth - mv_dblPrintWidth - 1) / 2
        v_dblHeight = mv_objPageSetting.HeaderHeight
        For v_int = 0 To mv_arrHeaderRow.Count - 1
            With mv_arrHeaderRow(v_int)

                lbl = New Label
                lbl.Name = "lbl" & .FieldName
                If mv_objPageSetting.UserLanguage = "VN" Then
                    lbl.Text = .Caption
                Else
                    lbl.Text = .En_Caption
                End If

                lbl.Border.BottomColor = Drawing.Color.Black
                lbl.Border.BottomStyle = BorderLineStyle.Solid
                lbl.Border.TopColor = Drawing.Color.Black
                lbl.Border.TopStyle = BorderLineStyle.Solid
                lbl.Border.RightColor = Drawing.Color.Black
                lbl.Border.RightStyle = BorderLineStyle.Solid

                If v_int = 0 Then
                    lbl.Border.LeftColor = Drawing.Color.Black
                    lbl.Border.LeftStyle = BorderLineStyle.Solid
                End If

                lbl.Style = "ddo-char-set: 0; text-align: center; font-weight: bold;" _
                            & " font-size: " & mv_strFontSize & "pt; vertical-align: middle; "
                lbl.Left = v_dblLeft
                lbl.BackColor = Drawing.Color.LightGray
                'v_dblLeft = v_dblLeft + lbl.Width
                lbl.Width = .Width
                If .IsParent = "Y" Then
                    lbl.Height = mv_objPageSetting.DataRowHeight
                Else
                    If .ParentID <> 0 Then
                        lbl.Height = v_dblHeight - mv_objPageSetting.DataRowHeight
                    Else
                        lbl.Height = v_dblHeight
                    End If
                    v_dblLeft = v_dblLeft + lbl.Width
                End If

                If .ParentID <> 0 Then
                    lbl.Top = mv_objPageSetting.DataRowHeight 'mv_dblTop + 0.25!
                Else
                    lbl.Top = 0 'mv_dblTop
                End If

                Me.mv_rtpPageHearder.Controls.Add(lbl)

            End With
        Next

        Me.mv_rtpPageHearder.Height = v_dblHeight
    End Sub


    Private Sub ReportRowDetail()
        Dim txt As TextBox
        Dim v_dblLeft, v_dblTempLeft As Double
        Dim v_int As Integer
        Dim v_dblPageWidth As Double

        If mv_objPageSetting.Orientation = 1 Then
            v_dblPageWidth = Me.PageSettings.PaperWidth
        Else
            v_dblPageWidth = Me.PageSettings.PaperHeight
        End If

        v_dblLeft = (v_dblPageWidth - mv_dblPrintWidth - 1) / 2
        v_dblLeft = System.Math.Round(v_dblLeft, 2) - 0.005
        Dim ln As Line
        v_dblTempLeft = v_dblLeft
        ln = InitLine(v_dblLeft, mv_objPageSetting.DataRowHeight, 0.0, 0)
        ln.AnchorBottom = True
        Me.mv_rptDetail.Controls.Add(ln)

        For v_int = 0 To mv_arrHeaderRow.Count - 1
            With mv_arrHeaderRow(v_int)
                If .IsDataField = "Y" Then
                    If .FieldName = "STT" Then
                        lblRowNum = New Label
                        lblRowNum.Name = "lblSTT"
                        lblRowNum.Top = 0
                        lblRowNum.Left = v_dblLeft + 0.0125
                        lblRowNum.Width = .Width - 0.015
                        v_dblLeft = v_dblLeft + .Width
                        'lblRowNum.Alignment = TextAlignment.Center
                        'lblRowNum.VerticalAlignment = VerticalTextAlignment.Middle
                        lblRowNum.Style = "ddo-char-set: 0; text-align: center;" _
                           & " font-size: " & mv_strFontSize & "pt; vertical-align: middle; "
                        Me.mv_rptDetail.Controls.Add(lblRowNum)
                    Else
                        txt = New TextBox
                        txt.Name = "txt" & .FieldName
                        txt.DataField = .FieldName
                        txt.Top = 0.0!
                        txt.Left = v_dblLeft + 0.0125
                        txt.Width = .Width - 0.025
                        v_dblLeft = v_dblLeft + .Width

                        If .FieldType = "N" Or .FieldType = "D" Then
                            txt.OutputFormat = .Format
                        End If

                        If .TextAlign = "C" Then
                            'txt.Alignment = TextAlignment.Center
                            txt.Style = "ddo-char-set: 0; text-align: center;" _
                           & " font-size: " & mv_strFontSize & "pt; vertical-align: middle; "
                        ElseIf .TextAlign = "R" Then
                            'txt.Alignment = TextAlignment.Right
                            txt.Style = "ddo-char-set: 0; text-align: right;" _
                           & " font-size: " & mv_strFontSize & "pt; vertical-align: middle; "
                        ElseIf .TextAlign = "J" Then
                            'txt.Alignment = TextAlignment.Justify
                            txt.Style = "ddo-char-set: 0; text-align: justify;" _
                           & " font-size: " & mv_strFontSize & "pt; vertical-align: middle; "
                        Else
                            'txt.Alignment = TextAlignment.Left
                            txt.Style = "ddo-char-set: 0; text-align: left;" _
                           & " font-size: " & mv_strFontSize & "pt; vertical-align: middle; "
                        End If

                        txt.VerticalAlignment = VerticalTextAlignment.Middle
                        txt.Height = mv_objPageSetting.DataRowHeight
                        Me.mv_rptDetail.Controls.Add(txt)
                    End If
                    ln = InitLine(v_dblLeft, mv_objPageSetting.DataRowHeight, 0.0, 0)
                    ln.AnchorBottom = True
                    Me.mv_rptDetail.Controls.Add(ln)
                End If

            End With
        Next

        ln = InitLine(v_dblTempLeft, 0, mv_objPageSetting.DataRowHeight, mv_dblPrintWidth)
        ln.LineWeight = 1
        ln.Border.Style = BorderLineStyle.Dot
        Me.mv_rptDetail.Controls.Add(ln)
        'Me.PrintWidth = v_dblWidth
        'Me.mv_rptDetail.Height = 0.25!
        'hanm5 them tham so chieu cao cua dong du lieu
        Me.mv_rptDetail.Height = mv_objPageSetting.DataRowHeight

    End Sub

    Private Function InitLable(ByVal pv_strName As String, ByVal pv_strCaption As String, _
                               ByVal pv_dblLeft As Double, ByVal pv_dblHeight As Double, _
                               ByVal pv_dblWidth As Double) As Label
        Dim lbl As New Label
        lbl.Name = pv_strName
        lbl.Text = pv_strCaption
        lbl.Left = pv_dblLeft
        lbl.Height = pv_dblHeight
        lbl.Width = pv_dblWidth
        lbl.VerticalAlignment = VerticalTextAlignment.Middle

        Return lbl
    End Function

    Private Function InitTextBox(ByVal pv_strName As String, ByVal pv_strCaption As String, _
                                 ByVal pv_strFieldName As String, ByVal pv_strFieldType As String, _
                               ByVal pv_dblLeft As Double, ByVal pv_dblHeight As Double, _
                               ByVal pv_dblWidth As Double) As TextBox
        Dim txt As New TextBox
        txt.Name = pv_strName
        txt.DataField = pv_strFieldName
        txt.Text = pv_strCaption
        txt.Left = pv_dblLeft
        txt.Height = pv_dblHeight
        txt.Width = pv_dblWidth
        txt.VerticalAlignment = VerticalTextAlignment.Middle
        Select Case pv_strFieldType
            Case "N"
                txt.Alignment = TextAlignment.Right
            Case "D"
                txt.Alignment = TextAlignment.Center
            Case Else
                txt.Alignment = TextAlignment.Left
        End Select

        Return txt
    End Function

    Private Function InitLine(ByVal pv_dblLeft As Double, ByVal pv_dblHeight As Double, _
                              ByVal pv_dblTop As Double, ByVal pv_dblWidth As Double) As Line
        Dim v_ln As New Line

        v_ln.Top = pv_dblTop
        v_ln.Left = pv_dblLeft
        v_ln.Height = pv_dblHeight
        v_ln.Width = pv_dblWidth
        'v_ln.AnchorBottom = True
        v_ln.Border.Color = Drawing.Color.Black
        v_ln.LineWeight = 1
        v_ln.LineStyle = LineStyle.Solid

        Return v_ln
    End Function

    Private Sub SumTextBoxInGroup(ByRef pv_footer As GroupFooter, ByVal pv_dblLeft As Double)
        Dim v_int As Integer
        Dim txt As TextBox
        Dim ln As Line

        For v_int = 0 To mv_arrHeaderRow.Count - 1
            With mv_arrHeaderRow(v_int)
                If .IsDataField = "Y" Then
                    If .IsSum = "Y" Then
                        txt = InitTextBox("grpf" & .FieldName, "", .FieldName, .FieldType, pv_dblLeft + 0.0125, mv_objPageSetting.DataRowHeight, .Width - 0.025)
                        txt.SummaryFunc = SummaryFunc.Sum
                        txt.SummaryGroup = pv_footer.Name
                        txt.SummaryRunning = SummaryRunning.Group
                        txt.SummaryType = SummaryType.SubTotal
                        txt.BackColor = Drawing.Color.Transparent
                        txt.OutputFormat = .Format
                        txt.Style = "ddo-char-set: 0; text-align: right; font-weight: bold;" _
                                & " font-size: " & mv_strFontSize & "pt; vertical-align: middle; "
                        pv_footer.Controls.Add(txt)

                        ln = InitLine(pv_dblLeft, mv_objPageSetting.DataRowHeight, 0, 0)
                        pv_footer.Controls.Add(ln)

                        ln = InitLine(pv_dblLeft + .Width, mv_objPageSetting.DataRowHeight, 0, 0)
                        pv_footer.Controls.Add(ln)

                    End If
                    pv_dblLeft += .Width
                End If
            End With
        Next
    End Sub

    Private Sub SumTextBoxInFooter(ByRef pv_footer As GroupFooter, ByVal pv_dblLeft As Double)
        Dim v_int As Integer
        Dim txt As TextBox
        Dim ln As Line
        Dim v_bln As Boolean = False
        Dim v_dblLeft As Double = pv_dblLeft


        For v_int = 0 To mv_arrHeaderRow.Count - 1
            With mv_arrHeaderRow(v_int)
                If .IsDataField = "Y" Then
                    If .IsSum = "Y" Then
                        txt = InitTextBox("grpf" & .FieldName, "", .FieldName, .FieldType, pv_dblLeft, mv_objPageSetting.DataRowHeight, .Width)
                        txt.SummaryFunc = SummaryFunc.Sum
                        txt.SummaryRunning = SummaryRunning.None
                        txt.SummaryType = SummaryType.GrandTotal
                        txt.OutputFormat = .Format
                        txt.Style = "ddo-char-set: 0; text-align: right; font-weight: bold;" _
                                & " font-size: " & mv_strFontSize & "pt; vertical-align: middle; "
                        pv_footer.Controls.Add(txt)

                        ln = InitLine(pv_dblLeft + .Width, mv_objPageSetting.DataRowHeight, 0, 0)
                        pv_footer.Controls.Add(ln)
                        v_bln = True
                    End If
                    pv_dblLeft += .Width
                End If
            End With
        Next
        If v_bln Then
            ln = InitLine(v_dblLeft, mv_objPageSetting.DataRowHeight, 0, 0)
            pv_footer.Controls.Add(ln)
            ln = InitLine(mv_dblPrintWidth + v_dblLeft, mv_objPageSetting.DataRowHeight, 0, 0)
            pv_footer.Controls.Add(ln)

            ln = InitLine(v_dblLeft, 0, mv_objPageSetting.DataRowHeight, mv_dblPrintWidth)
            pv_footer.Controls.Add(ln)

            ln = InitLine(v_dblLeft, mv_objPageSetting.DataRowHeight, 0, 0)
            pv_footer.Controls.Add(ln)
            Dim lbl As Label
            lbl = New Label
            lbl.Left = v_dblLeft
            lbl.Name = "grplblF"
            lbl.Text = "Tổng cộng"
            lbl.Width = 2
            lbl.Height = mv_objPageSetting.DataRowHeight
            lbl.Style = "ddo-char-set: 0; font-weight: bold;" _
                & " font-size: " & mv_strFontSize & "pt; vertical-align: middle; "

            pv_footer.Controls.Add(lbl)
            pv_footer.Height = mv_objPageSetting.DataRowHeight
        Else
            pv_footer.Height = 0.0
        End If
    End Sub

    Private Sub DrawCell(ByRef pv_dblLeft As Double, _
                         ByVal pv_dblTop As Double, _
                         ByVal pv_intPos As Integer, _
                         ByVal pv_intLev As Integer, _
                         ByVal pv_objHeadRow As ReportHeaderRow, _
                         ByRef pv_rptGroupHeader As GroupHeader)
        Dim lbl As Label
        lbl = New Label
        With pv_objHeadRow
            lbl.Name = "lbl" & .FieldName
            If mv_objPageSetting.UserLanguage = "VN" Then
                lbl.Text = .Caption
            Else
                lbl.Text = .En_Caption
            End If

            lbl.Border.BottomColor = Drawing.Color.Black
            lbl.Border.BottomStyle = BorderLineStyle.Solid
            lbl.Border.TopColor = Drawing.Color.Black
            lbl.Border.TopStyle = BorderLineStyle.Solid
            lbl.Border.RightColor = Drawing.Color.Black
            lbl.Border.RightStyle = BorderLineStyle.Solid
            If pv_intPos = 0 Then
                lbl.Border.LeftColor = Drawing.Color.Black
                lbl.Border.LeftStyle = BorderLineStyle.Solid
            End If

            lbl.Style = "ddo-char-set: 0; text-align: center; font-weight: bold;" _
                        & " font-size: " & mv_strFontSize & "pt; vertical-align: middle; "
            lbl.Left = pv_dblLeft
            lbl.Top = pv_dblTop

            lbl.BackColor = Drawing.Color.LightGray
            'v_dblLeft = v_dblLeft + lbl.Width
            lbl.Width = .Width
            lbl.Height = .Height

            'Me.mv_rtpPageHearder.Controls.Add(lbl)
            pv_rptGroupHeader.Controls.Add(lbl)
            If .IsParent = "Y" Then
                pv_dblTop += .Height
                For v_int As Integer = 0 To mv_arrHeaderRow.Count - 1
                    If mv_arrHeaderRow(v_int).Lev = pv_intLev + 1 Then
                        If mv_arrHeaderRow(v_int).ParentID = .ID Then
                            DrawCell(pv_dblLeft, pv_dblTop, v_int, mv_arrHeaderRow(v_int).Lev, mv_arrHeaderRow(v_int), pv_rptGroupHeader)
                        End If
                    End If
                Next
            Else
                pv_dblLeft += .Width
            End If
        End With
    End Sub

    Private Sub DrawHeader(ByVal pv_rptGroupHeader As GroupHeader)
        Dim v_int As Integer
        Dim v_dblLeft As Double
        Dim pv_dblTop As Double
        pv_dblTop = 0.0
        v_dblLeft = mv_dblLeft
        For v_int = 0 To mv_arrHeaderRow.Count - 1
            If mv_arrHeaderRow(v_int).Lev = 1 Then
                DrawCell(v_dblLeft, pv_dblTop, v_int, mv_arrHeaderRow(v_int).Lev, mv_arrHeaderRow(v_int), pv_rptGroupHeader)
            End If
        Next
        'Me.mv_rptReportHeader.Height = pv_dblTop
    End Sub

    Private Sub DrowNumHeader(ByVal pv_dblTop As Double, ByRef pv_rptGroupHeader As GroupHeader)
        Dim v_int As Integer
        Dim v_dblLeft As Double
        Dim v_dblTop As Double
        v_dblTop = pv_dblTop
        v_dblLeft = mv_dblLeft
        Dim v_intPos As Integer = 0
        Dim lbl As Label
        For v_int = 0 To mv_arrHeaderRow.Count - 1
            If mv_arrHeaderRow(v_int).IsDataField = "Y" Then
                With mv_arrHeaderRow(v_int)
                    lbl = New Label
                    lbl.Name = "lbl_num" & .FieldName
                    lbl.Text = v_intPos + 1

                    lbl.Border.BottomColor = Drawing.Color.Black
                    lbl.Border.BottomStyle = BorderLineStyle.Solid
                    lbl.Border.TopColor = Drawing.Color.Black
                    lbl.Border.TopStyle = BorderLineStyle.Solid
                    lbl.Border.RightColor = Drawing.Color.Black
                    lbl.Border.RightStyle = BorderLineStyle.Solid
                    If v_intPos = 0 Then
                        lbl.Border.LeftColor = Drawing.Color.Black
                        lbl.Border.LeftStyle = BorderLineStyle.Solid
                    End If

                    lbl.Style = "ddo-char-set: 0; text-align: center; font-weight: bold;" _
                                & " font-size: " & mv_strFontSize & "pt; vertical-align: middle; "
                    lbl.Left = v_dblLeft
                    lbl.Top = v_dblTop

                    lbl.BackColor = Drawing.Color.LightGray
                    'v_dblLeft = v_dblLeft + lbl.Width
                    lbl.Width = .Width
                    lbl.Height = mv_objPageSetting.DataRowHeight
                    v_dblLeft += .Width
                    v_intPos += 1
                    pv_rptGroupHeader.Controls.Add(lbl)
                End With
            End If
        Next
    End Sub

#End Region

#Region "Report Events"
    Private Sub mv_rptDetail_Format(ByVal sender As Object, ByVal e As System.EventArgs) Handles mv_rptDetail.Format
        mv_intRowNum += 1
        If Not lblRowNum Is Nothing Then
            lblRowNum.Text = mv_intRowNum
        End If
    End Sub

    Private Sub rptReports_ReportStart(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.ReportStart
        Dim v_intGroupCount As Integer
        If Not mv_arrGroup Is Nothing Then
            v_intGroupCount = mv_arrGroup.Count - 1
            For i As Integer = 0 To v_intGroupCount
                With mv_arrGroup(i)
                    If .FldType <> "F" And .Footer <> "" Then
                        Dim v_oField As New Field
                        v_oField.Name = "F_" & .FieldName
                        Me.Fields.Add(v_oField)
                        v_oField.Dispose()
                    End If
                End With
            Next
        End If
    End Sub

    Private Sub mv_rptGroupH_Format(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            If Not Me.Fields("F_" & CType(sender, GroupHeader).Name) Is Nothing Then
                Dim v_strTmp As String
                v_strTmp = Me.Fields(CType(sender, GroupHeader).Tag.ToString.Split("|")(0)).Value.ToString
                v_strTmp = Trim(Mid(v_strTmp, InStr(v_strTmp, ".") + 1))
                Me.Fields("F_" & CType(sender, GroupHeader).Name).Value = CType(sender, GroupHeader).Tag.ToString.Split("|")(1) & " " & v_strTmp
            End If
        Catch ex As Exception

        End Try

    End Sub
#End Region
End Class
