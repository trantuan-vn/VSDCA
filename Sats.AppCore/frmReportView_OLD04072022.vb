Imports System.Windows.Forms
Imports Xceed.SmartUI.Controls
Imports Sats.CommonLibrary
Imports System.Collections
Imports DataDynamics.ActiveReports.Toolbar
Imports System.Drawing
Imports System.IO
Imports DataDynamics.ActiveReports
Imports DataDynamics.ActiveReports.Export
Imports DataDynamics.ActiveReports.Export.Html
Imports DataDynamics.ActiveReports.Export.Pdf
Imports DataDynamics.ActiveReports.Export.Rtf
Imports DataDynamics.ActiveReports.Export.Text
Imports DataDynamics.ActiveReports.Export.Xls

Public Class frmReportView
#Region " Declare constant and variables "
    Const c_ResourceManager = "Sats.AppCore.frmReportView_"
    Private mv_ReportSetting As ReportSetting
    Private mv_DatasSet As DataSet
    Private mv_blnPrint As Boolean
    Private mv_blnExport As Boolean
    Private mv_ResourceManager As Resources.ResourceManager
    Private mv_strUserLanguage As String
    Private mv_blnOpen As Boolean
    Private mv_strObjMsg As String
    Private mv_oProxy As BDSChannel.BDSDelivery
    Private mv_strReportId As String
    Private mv_lstSICODE As String
    Private mv_strTellerName As String
#End Region

#Region " Properties "
    Public Property UserLanguage() As String
        Get
            Return mv_strUserLanguage
        End Get
        Set(ByVal value As String)
            mv_strUserLanguage = value
        End Set
    End Property
    Public Property ListSicode() As String
        Get
            Return mv_lstSICODE
        End Get
        Set(ByVal value As String)
            mv_lstSICODE = value
        End Set
    End Property

    Public Property ReportId() As String
        Get
            Return mv_strReportId
        End Get
        Set(ByVal value As String)
            mv_strReportId = value
        End Set
    End Property
    Public Property TellerName() As String
        Get
            Return mv_strTellerName
        End Get
        Set(ByVal value As String)
            mv_strTellerName = value
        End Set
    End Property

    Public Property ClientDataSet() As DataSet
        Get
            Return mv_DatasSet
        End Get
        Set(ByVal value As DataSet)
            mv_DatasSet = value
        End Set
    End Property

    Public Property ReportSetting() As ReportSetting
        Get
            Return mv_ReportSetting
        End Get
        Set(ByVal Value As ReportSetting)
            mv_ReportSetting = Value
        End Set
    End Property

    Public Property Print() As Boolean
        Get
            Return mv_blnPrint
        End Get
        Set(ByVal value As Boolean)
            mv_blnPrint = value
        End Set
    End Property
    Public Property Export() As Boolean
        Get
            Return mv_blnExport
        End Get
        Set(ByVal value As Boolean)
            mv_blnExport = value
        End Set
    End Property
    Public Property Proxy() As BDSChannel.BDSDelivery
        Get
            Return mv_oProxy
        End Get
        Set(ByVal value As BDSChannel.BDSDelivery)
            mv_oProxy = value
        End Set
    End Property
#End Region

#Region "Form Events"

    Private Sub frmReportView_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        'If e.CloseReason = CloseReason.UserClosing Then
        If Not mv_blnOpen Then
            If MsgBox(mv_ResourceManager.GetString("QSave"), MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, gc_ApplicationTitle) = MsgBoxResult.Yes Then
                SaveDocument()
            End If
        End If
    End Sub

    Private Sub frmReportView_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If mv_ReportSetting.UserLanguage = "VN" Then
                Me.Text = mv_ReportSetting.Title
            Else
                Me.Text = mv_ReportSetting.En_Title
            End If
        Catch ex As Exception

        End Try
        mv_ResourceManager = New Resources.ResourceManager(c_ResourceManager & UserLanguage, System.Reflection.Assembly.GetExecutingAssembly())
        'ShowReport()
        'arViewer.Toolbar.Visible = False
        arViewer.Toolbar.Tools(0).Visible = False
        arViewer.Toolbar.Tools(2).Visible = mv_blnPrint
        InitToolBar()
    End Sub
#End Region

#Region "Other method"

    Private Sub InitToolBar()
        arViewer.Toolbar.Images.Images.Add(imlTool.Images(0))
        arViewer.Toolbar.Images.Images.Add(imlTool.Images(1))
        arViewer.Toolbar.Tools(3).Enabled = Print

        'Add Save button to the toolbar
        Dim _btn As New DataDynamics.ActiveReports.Toolbar.Button
        _btn.ImageIndex = 14 ' new images were added to Toolbar.Images 
        _btn.ButtonStyle = DataDynamics.ActiveReports.Toolbar.ButtonStyle.Icon
        _btn.Caption = mv_ResourceManager.GetString("Save")
        _btn.Id = 5001 ' unique identifier for the new tool
        _btn.ToolTip = mv_ResourceManager.GetString("SaveTitle")
        _btn.Enabled = Export
        arViewer.Toolbar.Tools.Insert(3, CType(_btn, DataDynamics.ActiveReports.Toolbar.Tool))

        ''Add Export button to the toolbar
        _btn = New DataDynamics.ActiveReports.Toolbar.Button
        _btn.ImageIndex = 15
        _btn.Id = 5002
        _btn.ButtonStyle = DataDynamics.ActiveReports.Toolbar.ButtonStyle.Icon
        _btn.Caption = "Export"
        _btn.ToolTip = "Export Report Document"
        _btn.Enabled = Export
        arViewer.Toolbar.Tools.Insert(3, CType(_btn, DataDynamics.ActiveReports.Toolbar.Tool))
    End Sub

    Public Sub ShowReport(Optional ByVal v_strObjMsg As String = "")
        Try
            'Chuyển format cho Active Reports
            'Lay thong tin de export 

            mv_strObjMsg = v_strObjMsg

            'end Lay thong tin de export
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("vi-VN")
            Dim v_oRpt As DataDynamics.ActiveReports.ActiveReport3 = Nothing
            Select Case mv_ReportSetting.ReportType
                Case 1
                    v_oRpt = New rptReports(mv_ReportSetting, mv_DatasSet)
                Case 2
                    v_oRpt = New rptReportType2(mv_ReportSetting, mv_DatasSet)
                    'v_oRpt = New rptTextReport(mv_ReportSetting)
                Case 3
                    v_oRpt = New rptReportType3(mv_ReportSetting, mv_DatasSet)
                    arViewer.TableOfContents.Visible = True
                    Dim _btn As New DataDynamics.ActiveReports.Toolbar.Button
                    _btn.ImageIndex = 0 ' new images were added to Toolbar.Images 
                    _btn.ButtonStyle = DataDynamics.ActiveReports.Toolbar.ButtonStyle.Icon
                    '_btn.Caption = mv_ResourceManager.GetString("Content")
                    _btn.Id = 6001 ' unique identifier for the new tool
                    '_btn.ToolTip = mv_ResourceManager.GetString("Content")
                    arViewer.Toolbar.Tools.Insert(0, CType(_btn, DataDynamics.ActiveReports.Toolbar.Tool))
                Case 4
                    v_oRpt = New rptReportType4(mv_ReportSetting, mv_DatasSet)
                    arViewer.TableOfContents.Visible = True
                    Dim _btn As New DataDynamics.ActiveReports.Toolbar.Button
                    _btn.ImageIndex = 0 ' new images were added to Toolbar.Images 
                    _btn.ButtonStyle = DataDynamics.ActiveReports.Toolbar.ButtonStyle.Icon
                    '_btn.Caption = mv_ResourceManager.GetString("Content")
                    _btn.Id = 6001 ' unique identifier for the new tool
                    '_btn.ToolTip = mv_ResourceManager.GetString("Content")
                    arViewer.Toolbar.Tools.Insert(0, CType(_btn, DataDynamics.ActiveReports.Toolbar.Tool))
                Case 5
                    v_oRpt = New ActiveReport3
                    arViewer.TableOfContents.Visible = True
                    Dim v_oDir As New ApplicationServices.ApplicationBase
                    v_oRpt.LoadLayout(v_oDir.Info.DirectoryPath & "\Reports\Header\" & ReportSetting.ReportID & ".rpx")
                    Dim _btn As New DataDynamics.ActiveReports.Toolbar.Button
                    _btn.ImageIndex = 0 ' new images were added to Toolbar.Images 
                    _btn.ButtonStyle = DataDynamics.ActiveReports.Toolbar.ButtonStyle.Icon
                    '_btn.Caption = mv_ResourceManager.GetString("Content")
                    _btn.Id = 6001 ' unique identifier for the new tool
                    '_btn.ToolTip = mv_ResourceManager.GetString("Content")
                    arViewer.Toolbar.Tools.Insert(0, CType(_btn, DataDynamics.ActiveReports.Toolbar.Tool))
            End Select

            If Not ClientDataSet Is Nothing Then
                If ClientDataSet.Tables.IndexOf(ReportSetting.ReportID) >= 0 Then
                    v_oRpt.DataSource = ClientDataSet 'GetDataSet()
                    v_oRpt.DataMember = ReportSetting.ReportID '"RptData"
                End If
            End If

            v_oRpt.SetLicense("RGN,RGN Warez Group,DD-APN-30-C01339,944SHS949SWOM49HSHSF")

            v_oRpt.Run(True)
            arViewer.Document = v_oRpt.Document
            arViewer.Toolbar.Tools.Item(1).Enabled = True
            v_oRpt.Dispose()
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                                     & "Error code: System error!" & vbNewLine _
                                                     & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Sub

    Private Sub SaveDocument()
        Dim _dlgSave As New SaveFileDialog
        _dlgSave.Title = mv_ResourceManager.GetString("SaveTitle")
        _dlgSave.Filter = "Report Document Files (*.rdf)|*.rdf"
        _dlgSave.DefaultExt = "rdf"
        _dlgSave.AddExtension = True
        If _dlgSave.ShowDialog(Me) = DialogResult.OK Then
            If File.Exists(_dlgSave.FileName) Then
                If MessageBox.Show(Me, mv_ResourceManager.GetString("Overwrite"), "Overwrite", MessageBoxButtons.YesNo) <> DialogResult.Yes Then
                    Return
                End If
            End If
            arViewer.Document.Save(_dlgSave.FileName)
        End If
    End Sub

    'Private Sub ExportDocument()
    '    Dim v_oRpt As DataDynamics.ActiveReports.ActiveReport3 = Nothing
    '    Dim v_oProcess As New ProcessForm(Me)
    '    Dim v_xmlDocumentMessage As New XmlDocumentEx
    '    Dim v_strVSDBrid As String


    '    Try
    '        v_xmlDocumentMessage.LoadXml(mv_strObjMsg)

    '        'Get header message.

    '        Dim v_attrColl As Xml.XmlAttributeCollection = v_xmlDocumentMessage.DocumentElement.Attributes
    '        Dim v_strTLName As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTLNAME), Xml.XmlAttribute).Value)


    '        Dim v_strSQL As String = "SELECT vsd_brid FROM tlprofiles a WHERE a.tlname='" & v_strTLName & "'"

    '        Dim v_strObjMsg1 As String = BuildXMLObjMsg(, , , v_strTLName, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLGROUPS, gc_ActionInquiry, v_strSQL)

    '        Dim v_lngError As Long = Proxy.Message(v_strObjMsg1)
    '        Dim v_strFLDNAME As String
    '        Dim v_strValue As String
    '        v_xmlDocumentMessage.LoadXml(v_strObjMsg1)
    '        Dim v_nodeList As Xml.XmlNodeList = v_xmlDocumentMessage.SelectNodes("/ObjectMessage/ObjData")

    '        For i As Integer = 0 To v_nodeList.Count - 1
    '            For j As Integer = 0 To v_nodeList.Item(i).ChildNodes.Count - 1
    '                With v_nodeList.Item(i).ChildNodes(j)
    '                    v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
    '                    v_strValue = .InnerText.ToString()
    '                End With
    '                Select Case Trim(v_strFLDNAME)
    '                    Case "VSD_BRID"
    '                        v_strVSDBrid = Trim(v_strValue)

    '                End Select
    '            Next

    '        Next

    '        v_oProcess.ChangeCaption("Đang kết xuất dữ liệu...")
    '        v_oProcess.StartProcessForm()
    '        If v_strVSDBrid = "03" Or v_strVSDBrid = "04" Then
    '            Dim v_dlgExport As New SaveFileDialog
    '            Dim v_oExport As New Object
    '            v_dlgExport.Title = "Export"
    '            v_dlgExport.Filter = "Microsoft Excel (*.xls)|*.xls|Portable Document Format (*.pdf)|*.pdf|Rich Text Format (*.rtf)|*.rtf|HTML Format (*.htm)|*.htm|Text Format (*.txt)|*.txt"
    '            v_dlgExport.DefaultExt = "xsl"
    '            v_dlgExport.AddExtension = True

    '            If v_dlgExport.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
    '                Dim v_strFile As String = v_dlgExport.FileName
    '                Select Case Mid(v_strFile, Len(v_strFile) - 2).ToUpper
    '                    Case "HTM"
    '                        v_oExport = New HtmlExport
    '                    Case "PDF"
    '                        v_oExport = New PdfExport
    '                    Case "RTF"
    '                        v_oExport = New RtfExport
    '                    Case "TXT"
    '                        v_oExport = New TextExport
    '                    Case "XLS"
    '                        'v_oExport = New XlsExport

    '                        'v_oExport.DisplayGridLines = True
    '                        'v_oExport.RemoveVerticalSpace = True
    '                        'v_oExport.MinRowHeight = 0
    '                        'v_oExport.MinColumnWidth = 0.2
    '                        ''v_oExport.AutoRowHeight = True
    '                        'v_oExport.UseCellMerging = True
    '                        'v_oExport.FileFormat = FileFormat.Xls97Plus

    '                        v_oProcess.ChangeCaption("Đang kết xuất dữ liệu...")
    '                        v_oProcess.StartProcessForm()
    '                        ExportToExcel(mv_DatasSet, mv_ReportSetting, v_strFile)
    '                        v_oProcess.StopProcessForm()
    '                        MsgBox(IIf(Me.UserLanguage = "VN", "Kết xuất dữ liệu thành công.", "Export is successful!"), MsgBoxStyle.OkOnly, "Thông báo")
    '                        Exit Sub
    '                End Select

    '                v_oProcess.ChangeCaption("Đang kết xuất dữ liệu...")
    '                v_oProcess.StartProcessForm()
    '                Select Case mv_ReportSetting.ReportType
    '                    Case 1
    '                        v_oRpt = New rptReportsExport(mv_ReportSetting, mv_DatasSet)
    '                        v_oRpt.SetLicense("RGN,RGN Warez Group,DD-APN-30-C01339,944SHS949SWOM49HSHSF")

    '                        If Not ClientDataSet Is Nothing Then
    '                            If ClientDataSet.Tables.IndexOf(ReportSetting.ReportID) >= 0 Then
    '                                v_oRpt.DataSource = ClientDataSet 'GetDataSet()
    '                                v_oRpt.DataMember = ReportSetting.ReportID '"RptData"
    '                            End If
    '                        End If
    '                        v_oRpt.Run(True)
    '                        v_oExport.Export(v_oRpt.Document, v_strFile)
    '                        v_oRpt.Dispose()
    '                    Case 3
    '                        v_oRpt = New rptReportType3Export(mv_ReportSetting, mv_DatasSet)
    '                        v_oRpt.SetLicense("RGN,RGN Warez Group,DD-APN-30-C01339,944SHS949SWOM49HSHSF")

    '                        If Not ClientDataSet Is Nothing Then
    '                            If ClientDataSet.Tables.IndexOf(ReportSetting.ReportID) >= 0 Then
    '                                v_oRpt.DataSource = ClientDataSet 'GetDataSet()
    '                                v_oRpt.DataMember = ReportSetting.ReportID '"RptData"
    '                            End If
    '                        End If
    '                        v_oRpt.Run(True)
    '                        v_oExport.Export(v_oRpt.Document, v_strFile)
    '                        v_oRpt.Dispose()
    '                    Case Else
    '                        v_oExport.Export(arViewer.Document, v_strFile)
    '                End Select
    '            End If
    '            v_oProcess.StopProcessForm()
    '            MsgBox(IIf(Me.UserLanguage = "VN", "Kết xuất dữ liệu thành công.", "Export is successful!"), MsgBoxStyle.Information, "Thông báo")
    '        Else
    '            Dim v_lngErr As Long
    '            v_lngErr = Proxy.RptExpMessage(mv_strObjMsg)
    '            v_oProcess.StopProcessForm()
    '            If v_lngErr = 0 Then
    '                MsgBox(IIf(Me.UserLanguage = "VN", "Kết xuất dữ liệu thành công.", "Export is successful!"), MsgBoxStyle.Information, "Thông báo")
    '            Else
    '                MsgBox(IIf(Me.UserLanguage = "VN", "Kết xuất dữ liệu gặp lỗi.", "Export is Error!"), MsgBoxStyle.Critical, "Thông báo")
    '            End If
    '        End If

    '    Catch ex As Exception
    '        v_oProcess.StopProcessForm()
    '        LogError.Write("Error source: " & ex.Source & vbNewLine _
    '                                                 & "Error code: System error!" & vbNewLine _
    '                                                 & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
    '        MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
    '    Finally

    '    End Try
    'End Sub
    Private Sub ExportDocument()
        Dim v_oRpt As DataDynamics.ActiveReports.ActiveReport3 = Nothing
        Dim v_oProcess As New ProcessForm(Me)
        Dim v_lngError As Long
        Try
            'Kiem tra xem co export tren server hay khong
            Dim v_lstSICODEAllow As String = ""
            Dim v_intServerRptExp As Integer = Proxy.GetServerRptExp(ReportId, TellerName, ListSicode, v_lstSICODEAllow)
            If v_intServerRptExp > 0 Then
                v_oProcess.StopProcessForm()
                Dim v_lstTmp As String = ""
                If v_lstSICODEAllow <> ListSicode Then
                    Dim v_arrSICODE() As String = ListSicode.Substring(1, ListSicode.Length - 2).Split(",")
                    Dim v_arrSICODEAllow() As String = v_lstSICODEAllow.Split(",")

                    For Each v_strTmp In v_arrSICODE
                        If Not v_arrSICODEAllow.Contains(v_strTmp) Then
                            v_lstTmp = v_lstTmp & "," & v_strTmp
                        End If
                    Next
                End If
                If v_lstTmp <> "" Then
                    v_lstTmp = v_lstTmp.Substring(1)
                    MsgBox(String.Format("Bạn không có quyền kết xuất mã chứng khoán {0} trên máy chủ!", v_lstTmp), MsgBoxStyle.Exclamation, gc_ApplicationTitle)
                    Exit Sub
                End If
                If MsgBox("Báo cáo sẽ được kết xuất trên máy chủ và đẩy lên máy chủ FTP. Bạn có muốn tiếp tục không?", MsgBoxStyle.OkCancel, gc_ApplicationTitle) = MsgBoxResult.Ok Then
                    v_oProcess.StartProcessForm()
                    v_lngError = Proxy.RptExpMessage(mv_strObjMsg)
                    If v_lngError <> ERR_SYSTEM_OK Then
                        MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                        Exit Sub
                    Else
                        v_oProcess.StopProcessForm()
                        MsgBox("Báo cáo được kết xuất thành công!", MsgBoxStyle.Information, gc_ApplicationTitle)
                    End If
                Else
                    Exit Sub
                End If
            Else
                Dim v_dlgExport As New SaveFileDialog
                Dim v_oExport As New Object
                v_dlgExport.Title = "Export"
                v_dlgExport.Filter = "Microsoft Excel (*.xls)|*.xls|Portable Document Format (*.pdf)|*.pdf|Rich Text Format (*.rtf)|*.rtf|HTML Format (*.htm)|*.htm|Text Format (*.txt)|*.txt"
                v_dlgExport.DefaultExt = "xsl"
                v_dlgExport.AddExtension = True

                If v_dlgExport.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    Dim v_strFile As String = v_dlgExport.FileName
                    Select Case Mid(v_strFile, Len(v_strFile) - 2).ToUpper
                        Case "HTM"
                            v_oExport = New HtmlExport
                        Case "PDF"
                            v_oExport = New PdfExport
                        Case "RTF"
                            v_oExport = New RtfExport
                        Case "TXT"
                            v_oExport = New TextExport
                        Case "XLS"
                            'v_oExport = New XlsExport

                            'v_oExport.DisplayGridLines = True
                            'v_oExport.RemoveVerticalSpace = True
                            'v_oExport.MinRowHeight = 0
                            'v_oExport.MinColumnWidth = 0.2
                            ''v_oExport.AutoRowHeight = True
                            'v_oExport.UseCellMerging = True
                            'v_oExport.FileFormat = FileFormat.Xls97Plus

                            v_oProcess.ChangeCaption("Đang kết xuất dữ liệu...")
                            v_oProcess.StartProcessForm()
                            ExportToExcel(mv_DatasSet, mv_ReportSetting, v_strFile)
                            v_oProcess.StopProcessForm()
                            MsgBox(IIf(Me.UserLanguage = "VN", "Kết xuất dữ liệu thành công.", "Export is successful!"), MsgBoxStyle.OkOnly, "Thông báo")
                            Exit Sub
                    End Select

                    v_oProcess.ChangeCaption("Đang kết xuất dữ liệu...")
                    v_oProcess.StartProcessForm()
                    Select Case mv_ReportSetting.ReportType
                        Case 1
                            v_oRpt = New rptReportsExport(mv_ReportSetting, mv_DatasSet)
                            v_oRpt.SetLicense("RGN,RGN Warez Group,DD-APN-30-C01339,944SHS949SWOM49HSHSF")

                            If Not ClientDataSet Is Nothing Then
                                If ClientDataSet.Tables.IndexOf(ReportSetting.ReportID) >= 0 Then
                                    v_oRpt.DataSource = ClientDataSet 'GetDataSet()
                                    v_oRpt.DataMember = ReportSetting.ReportID '"RptData"
                                End If
                            End If
                            v_oRpt.Run(True)
                            v_oExport.Export(v_oRpt.Document, v_strFile)
                            v_oRpt.Dispose()
                        Case 3
                            v_oRpt = New rptReportType3Export(mv_ReportSetting, mv_DatasSet)
                            v_oRpt.SetLicense("RGN,RGN Warez Group,DD-APN-30-C01339,944SHS949SWOM49HSHSF")

                            If Not ClientDataSet Is Nothing Then
                                If ClientDataSet.Tables.IndexOf(ReportSetting.ReportID) >= 0 Then
                                    v_oRpt.DataSource = ClientDataSet 'GetDataSet()
                                    v_oRpt.DataMember = ReportSetting.ReportID '"RptData"
                                End If
                            End If
                            v_oRpt.Run(True)
                            v_oExport.Export(v_oRpt.Document, v_strFile)
                            v_oRpt.Dispose()
                        Case Else
                            v_oExport.Export(arViewer.Document, v_strFile)
                    End Select
                End If

                v_oProcess.StopProcessForm()
                MsgBox(IIf(Me.UserLanguage = "VN", "Kết xuất dữ liệu thành công.", "Export is successful!"), MsgBoxStyle.OkOnly, "Thông báo")
            End If
        Catch ex As Exception
            v_oProcess.StopProcessForm()
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                                     & "Error code: System error!" & vbNewLine _
                                                     & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        Finally

        End Try
    End Sub

    Public Sub OpenReport(ByVal pv_doc As Document.Document, ByVal pv_strFile As String)
        Try

            arViewer.Document = pv_doc
            arViewer.Document.Printer.PaperKind = Printing.PaperKind.A4

            Me.Text = pv_strFile
            mv_blnOpen = True
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                                     & "Error code: System error!" & vbNewLine _
                                                     & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Sub

    Public Sub PrintReport()
        If Me.PrintDialog1.ShowDialog() = DialogResult.OK Then
            'Option 1 :
            arViewer.Document.Printer.PrinterSettings = Me.PrintDialog1.PrinterSettings
            arViewer.Document.Printer.PrinterSettings.PrintRange = Me.PrintDialog1.PrinterSettings.PrintRange
            arViewer.Document.Print(False, False)
            'Option 2 :
            'Report1.Document.Printer.PrinterSettings = Me.PrintDialog1.PrinterSettings
            'Report1.Document.Print(False, False)
        End If
    End Sub
#End Region

    Private Sub arViewer_ToolClick(ByVal sender As Object, ByVal e As DataDynamics.ActiveReports.Toolbar.ToolClickEventArgs) Handles arViewer.ToolClick
        Try
            Select Case e.Tool.Id
                'Case 2 'Print button
                '    Me.PrintReport()
                Case 5001 'Save button
                    Me.SaveDocument()
                Case 5002 'Export button
                    Me.ExportDocument()
            End Select
        Catch ex As System.IO.IOException
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                                     & "Error code: System error!" & vbNewLine _
                                                     & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        mv_blnOpen = False
        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Public Sub ExportToExcel1(ByVal pv_oDataSet As DataSet, ByVal pv_ReportSetting As ReportSetting, _
                              ByVal pv_strFilePath As String)
        Dim v_strFieldName As String
        Dim v_int As Integer
        Dim v_intTblIndex As Integer = 0
        Dim v_oDataSet As New DataSet
        Try

            If pv_oDataSet.Tables.IndexOf(pv_ReportSetting.ReportID) >= 0 Then
                v_intTblIndex = pv_oDataSet.Tables.IndexOf(pv_ReportSetting.ReportID)
            End If
            Dim v_arrFieldName(pv_ReportSetting.ReportFld.Count) As String
            Dim v_arrFieldCaption(pv_ReportSetting.ReportFld.Count) As String
            Dim v_arrGroupName(pv_ReportSetting.ReportGrp.Count) As String
            Dim v_arrGroupCation(pv_ReportSetting.ReportGrp.Count) As String
            For v_int = 0 To pv_ReportSetting.ReportFld.Count - 1
                v_arrFieldName(v_int) = UCase(pv_ReportSetting.ReportFld(v_int).FieldName)
                v_arrFieldCaption(v_int) = UCase(pv_ReportSetting.ReportFld(v_int).Caption)
            Next
            For v_int = 0 To pv_ReportSetting.ReportGrp.Count - 1
                v_arrGroupName(v_int) = UCase(pv_ReportSetting.ReportGrp(v_int).FieldName)
                v_arrGroupCation(v_int) = UCase(pv_ReportSetting.ReportGrp(v_int).Caption)
            Next

            Dim v_intCount As Integer = pv_oDataSet.Tables(v_intTblIndex).Columns.Count
            Dim v_strRemoveColumn As String = ""
            v_oDataSet.Tables.Add(pv_oDataSet.Tables(v_intTblIndex).Copy)

            For i As Integer = 0 To v_intCount - 1
                If i < v_oDataSet.Tables(0).Columns.Count - 1 Then
                    v_strFieldName = Trim(v_oDataSet.Tables(0).Columns(i).ColumnName)
                    If v_arrFieldName.Contains(UCase(v_strFieldName)) Then
                        v_oDataSet.Tables(0).Columns(i).ColumnName = v_arrFieldCaption(v_arrFieldName.IndexOf(v_arrFieldName, UCase(v_strFieldName))) & "(" & i & ")"
                    ElseIf Not (v_arrGroupName.Contains(UCase(v_strFieldName)) Or v_arrGroupCation.Contains(UCase(v_strFieldName))) Then
                        v_strRemoveColumn &= v_strFieldName & "|"
                        'v_oDataSet.Tables(0).Columns.Remove(v_strFieldName)
                    Else
                        Select Case v_strFieldName
                            Case "MICODE"
                                v_oDataSet.Tables(0).Columns(i).ColumnName = "Mã TVLK"
                            Case "SICODE"
                                v_oDataSet.Tables(0).Columns(i).ColumnName = "Mã CK"
                            Case "IICODE"
                                v_oDataSet.Tables(0).Columns(i).ColumnName = "Mã PIN NĐT"
                            Case "MINAME"
                                v_oDataSet.Tables(0).Columns(i).ColumnName = "Tên TVLK"
                            Case "MI_NAME"
                                v_oDataSet.Tables(0).Columns(i).ColumnName = "Tên TVLK"
                            Case "STOCK_NAME"
                                v_oDataSet.Tables(0).Columns(i).ColumnName = "Tên CK"
                            Case "STOCKNAME"
                                v_oDataSet.Tables(0).Columns(i).ColumnName = "Tên CK"
                        End Select
                    End If
                End If
            Next
            If v_strRemoveColumn <> "" Then
                Dim v_arrRemoveColumn() As String = v_strRemoveColumn.Split("|")
                For j As Integer = 0 To v_arrRemoveColumn.Count - 1
                    If Trim(v_arrRemoveColumn(j)) <> "" Then
                        v_oDataSet.Tables(0).Columns.Remove(Trim(v_arrRemoveColumn(j)))
                    End If
                Next
            End If

            Dim Excel As Object = CreateObject("Excel.Application")
            If Excel Is Nothing Then
                MsgBox("Máy chưa được cài đặt ứng dụng Excel. Vui lòng cài đặt Microsoft Excel trước khi kết xuất dữ liệu!", MsgBoxStyle.Critical)
                Exit Sub
            End If
            'Export to Excel process
            With Excel
                .SheetsInNewWorkbook = 1
                .Workbooks.Add()
                .Worksheets(1).Select()

                Dim i As Integer = 1
                For v_col = 0 To v_oDataSet.Tables(0).Columns.Count - 1
                    .cells(1, i).value = v_oDataSet.Tables(0).Columns(v_col).ColumnName
                    .cells(1, i).EntireRow.Font.Bold = True
                    i += 1
                Next
                i = 2
                Dim k As Integer = 1
                For v_col = 0 To v_oDataSet.Tables(0).Columns.Count - 1
                    i = 2
                    For row = 0 To v_oDataSet.Tables(0).Rows.Count - 1
                        .Cells(i, k).Value = v_oDataSet.Tables(0).Rows(row).ItemArray(v_col)
                        i += 1
                    Next
                    k += 1
                Next
                .ActiveCell.Worksheet.SaveAs(pv_strFilePath)
            End With
            System.Runtime.InteropServices.Marshal.ReleaseComObject(Excel)
            Excel = Nothing
        Catch ex As Exception
            Throw ex
            MsgBox(ex.Message)
        Finally
            ' The excel is created and opened for insert value. We most close this excel using this system
            Dim v_arrPro() As Process = System.Diagnostics.Process.GetProcessesByName("EXCEL")
            For Each i As Process In v_arrPro
                i.Kill()
            Next
        End Try
    End Sub

    Public Sub ExportToExcel(ByVal pv_oDataSet As DataSet, ByVal pv_ReportSetting As ReportSetting, ByVal pv_strFilePath As String)
        Dim v_strFieldName As String
        Dim v_int As Integer
        Dim v_intTblIndex As Integer = 0
        Dim v_oDataSet As New DataSet
        Dim v_intGrpCount As Integer = 0
        Try

            If pv_oDataSet.Tables.IndexOf(pv_ReportSetting.ReportID) >= 0 Then
                v_intTblIndex = pv_oDataSet.Tables.IndexOf(pv_ReportSetting.ReportID)
            End If
            Dim v_arrFieldName(pv_ReportSetting.ReportFld.Count) As String
            Dim v_arrFieldCaption(pv_ReportSetting.ReportFld.Count) As String
            If Not pv_ReportSetting.ReportGrp Is Nothing Then
                v_intGrpCount = pv_ReportSetting.ReportGrp.Count
            Else
                v_intGrpCount = 0
            End If
            Dim v_arrGroupName(v_intGrpCount) As String
            Dim v_arrGroupCation(v_intGrpCount) As String

            For v_int = 0 To pv_ReportSetting.ReportFld.Count - 1
                v_arrFieldName(v_int) = UCase(pv_ReportSetting.ReportFld(v_int).FieldName)
                v_arrFieldCaption(v_int) = UCase(pv_ReportSetting.ReportFld(v_int).Caption)
            Next

            If Not pv_ReportSetting.ReportGrp Is Nothing Then
                For v_int = 0 To pv_ReportSetting.ReportGrp.Count - 1
                    v_arrGroupName(v_int) = UCase(pv_ReportSetting.ReportGrp(v_int).FieldName)
                    v_arrGroupCation(v_int) = UCase(pv_ReportSetting.ReportGrp(v_int).Caption)
                Next
            End If
            
            Dim v_intCount As Integer = pv_oDataSet.Tables(v_intTblIndex).Columns.Count
            Dim v_strRemoveColumn As String = ""
            v_oDataSet.Tables.Add(pv_oDataSet.Tables(v_intTblIndex).Copy)

            For i As Integer = 0 To v_intCount - 1
                If i < v_oDataSet.Tables(0).Columns.Count - 1 Then
                    v_strFieldName = Trim(v_oDataSet.Tables(0).Columns(i).ColumnName)
                    If v_arrFieldName.Contains(UCase(v_strFieldName)) Then
                        v_oDataSet.Tables(0).Columns(i).ColumnName = v_arrFieldCaption(v_arrFieldName.IndexOf(v_arrFieldName, UCase(v_strFieldName))) & "(" & i & ")"
                    ElseIf Not (v_arrGroupName.Contains(UCase(v_strFieldName)) Or v_arrGroupCation.Contains(UCase(v_strFieldName))) Then
                        v_strRemoveColumn &= v_strFieldName & "|"
                        'v_oDataSet.Tables(0).Columns.Remove(v_strFieldName)
                    Else
                        Select Case v_strFieldName
                            Case "MICODE"
                                v_oDataSet.Tables(0).Columns(i).ColumnName = "Mã TVLK"
                            Case "SICODE"
                                v_oDataSet.Tables(0).Columns(i).ColumnName = "Mã CK"
                            Case "IICODE"
                                v_oDataSet.Tables(0).Columns(i).ColumnName = "Mã PIN NĐT"
                            Case "IINAME"
                                v_oDataSet.Tables(0).Columns(i).ColumnName = "NĐT"
                            Case "MINAME"
                                v_oDataSet.Tables(0).Columns(i).ColumnName = "Tên TVLK"
                            Case "MI_NAME"
                                v_oDataSet.Tables(0).Columns(i).ColumnName = "Tên TVLK"
                            Case "STOCK_NAME"
                                v_oDataSet.Tables(0).Columns(i).ColumnName = "Tên CK"
                            Case "STOCKNAME"
                                v_oDataSet.Tables(0).Columns(i).ColumnName = "Tên CK"
                        End Select
                    End If
                End If
            Next
            If v_strRemoveColumn <> "" Then
                Dim v_arrRemoveColumn() As String = v_strRemoveColumn.Split("|")
                For j As Integer = 0 To v_arrRemoveColumn.Count - 1
                    If Trim(v_arrRemoveColumn(j)) <> "" Then
                        v_oDataSet.Tables(0).Columns.Remove(Trim(v_arrRemoveColumn(j)))
                    End If
                Next
            End If
            v_oDataSet.WriteXml(pv_strFilePath)
        Catch ex As Exception
            Throw ex
        Finally
            v_oDataSet.Dispose()
        End Try
    End Sub

End Class