Imports Sats.CommonLibrary
Imports System.IO
Imports DataDynamics.ActiveReports

Public Class frmReportMaster
    Private mv_strAuth As String

    Public Overrides Sub InitDialog()
        MyBase.InitDialog()
        btnAddNew.Text = mv_ResourceManager.GetString("btnAddNew_RP")
        btnView.Text = mv_ResourceManager.GetString("btnView_RP")
        btnEdit.Text = mv_ResourceManager.GetString("btnExecute_RP")
        btnExecute.Text = mv_ResourceManager.GetString("btnEdit_RP")
        btnAddNew.Enabled = False
        btnView.Enabled = False
        btnExecute.Enabled = False
        Me.btnDelete.Visible = False
        'Me.Text = Me.Text.Replace("@", ModuleCode)
    End Sub

    Protected Overrides Function ShowForm(ByVal pv_intExecFlag As Integer) As Boolean
        MyBase.ShowForm(pv_intExecFlag)
        Dim v_strRptID As String
        v_strRptID = Replace(Trim(CType(SearchGrid.CurrentRow, Xceed.Grid.DataRow).Cells(KeyColumn).Value), ".", String.Empty)
        If v_strRptID <> "" Then
            Select Case pv_intExecFlag
                Case ExecuteFlag.AddNew
                    CreateReport(v_strRptID)
                Case ExecuteFlag.View
                    'If m_BusLayer.mv_DataSet.Tables.IndexOf(v_strRptID) = -1 Then
                    '    CreateReport(v_strRptID)
                    'Else
                    '    ShowReport()
                    'End If
                Case ExecuteFlag.Edit
                    Dim v_dlgOpen As New OpenFileDialog
                    v_dlgOpen.Filter = "Reports Document Files (*.rdf)|*.rdf"
                    v_dlgOpen.FileName = ""
                    v_dlgOpen.CheckFileExists = True
                    v_dlgOpen.Title = "Open a Report Document File"

                    If v_dlgOpen.ShowDialog(Me) = DialogResult.OK Then
                        If File.Exists(v_dlgOpen.FileName) Then
                            ' Load the RDF file
                            Dim doc As New Document.Document
                            doc.Load(v_dlgOpen.FileName)

                            ' Open a new Preview Form
                            Dim v_frm As New Sats.AppCore.frmReportView
                            v_frm.UserLanguage = UserLanguage
                            v_frm.OpenReport(doc, v_dlgOpen.FileName)
                            v_frm.Show()
                        End If
                    End If
                Case ExecuteFlag.Execute

            End Select
        Else

        End If
    End Function

    Public Overrides Sub Grid_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'MyBase.Grid_Click(sender, e)

        mv_strAuth = CheckPermitionReport(Replace(Trim(CType(SearchGrid.CurrentRow, Xceed.Grid.DataRow).Cells(KeyColumn).Value), ".", String.Empty))

        btnAddNew.Enabled = ("Y" = Mid(mv_strAuth, 1, 1))
        btnView.Enabled = ("Y" = Mid(mv_strAuth, 2, 1))
        btnExecute.Enabled = ("Y" = Mid(mv_strAuth, 3, 1))
    End Sub

    Private Function CheckPermitionReport(ByVal v_strRptID As String) As String
        Try
            Dim v_strObjMsg As String
            Dim v_bln As String = False
            'v_strSQL = "Select a.strauth FROM cmdauth a WHERE a.deleted = 0 AND a.status = 0" _
            '                & " AND a.cmdtype = 'R' AND a.cmdcode='" & v_strRptID & "'" _
            '                & " AND ((a.AUTHID = '" & TellerId & "' AND a.authtype = 'U')" _
            '                & " OR (a.AUTHID IN (Select b.grpid FROM tlgrpusers b, tlgroups c" _
            '                & " WHERE b.grpid = c.grpid And c.deleted = 0 AND c.status = 0) AND a.authtype = 'G'))"
            v_strObjMsg = BuildXMLObjMsg(Now.Date, m_BusLayer.CurrentTellerProfile.BranchId, Now.Date, _
                m_BusLayer.CurrentTellerProfile.TellerId, CommonLibrary.gc_IsLocalMsg, CommonLibrary.gc_MsgTypeObj, _
                OBJNAME_SY_AUTHENTICATION, CommonLibrary.gc_ActionInquiry, , v_strRptID, "GetPermitReportData")
            'Dim v_ws As New BDSChannel.BDSDelivery
            Dim v_lngError As Long = Proxy.Message(v_strObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Return "NNNN"
            End If

            Dim v_xmlDocument As New XmlDocumentEx
            Dim v_nodeList As Xml.XmlNodeList
            v_xmlDocument.LoadXml(v_strObjMsg)
            v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")

            Dim i As Integer
            Dim v_strValue As String
            Dim v_strFldName As String
            Dim v_strAuth As String = "NNNN"
            Dim v_strTemp As String = "NNNN"
            For v_intCount As Integer = 0 To v_nodeList.Count - 1
                For i = 0 To v_nodeList.Item(v_intCount).ChildNodes.Count - 1
                    With v_nodeList.Item(v_intCount).ChildNodes(i)
                        v_strValue = .InnerText.ToString
                        v_strFldName = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                        Select Case Trim(v_strFldName)
                            Case "STRAUTH"
                                v_strAuth = MergeAuth(v_strValue, v_strTemp)
                        End Select
                    End With
                Next
                v_strTemp = v_strAuth
            Next
            Return v_strAuth

        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                         & "Error code: System error!" & vbNewLine _
                                         & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
            Return "NNNN"
        End Try
    End Function

    Private Sub ShowReport()
        'Dim v_thread As New Threading.Thread(AddressOf ShowFormProcess)
        Dim v_oProcess As New Sats.AppCore.ProcessForm(Me)
        Try

            v_oProcess.StartProcessForm()

            Dim v_objPageSetting As New ReportSetting
            Dim v_strsql As String
            Dim v_obj As New SQLEngine.SQLDataAccessLayer(TellerId)

            v_objPageSetting.ReportID = Replace(Trim(CType(SearchGrid.CurrentRow, Xceed.Grid.DataRow).Cells(KeyColumn).Value), ".", String.Empty)

            Dim v_ds As DataSet

            v_strsql = "SELECT * FROM RPREPORTS WHERE RPTID='" & v_objPageSetting.ReportID & "'"
            v_ds = v_obj.ExecuteReturnDataSet(v_strsql)

            Dim i As Integer
            Dim v_intIsMember As Integer

            For i = 0 To v_ds.Tables(0).Rows.Count - 1
                With v_ds.Tables(0)
                    v_objPageSetting.Title = .Rows(i)("RPTTITLE")
                    v_objPageSetting.En_Title = .Rows(i)("EN_RPTTITLE")
                    v_objPageSetting.ReportType = .Rows(i)("RPTTYPE")
                    v_objPageSetting.Orientation = .Rows(i)("ORIENTATION")
                    v_objPageSetting.DSN = .Rows(i)("DSN")
                    v_objPageSetting.ObjectName = .Rows(i)("OBJNAME")
                    v_objPageSetting.HeaderHeight = CDbl(.Rows(i)("TITLE_HEIGHT"))
                    v_objPageSetting.HHeight = CDbl(.Rows(i)("HEADER_HEIGHT"))
                    v_objPageSetting.FHeight = CDbl(.Rows(i)("FOOTER_HEIGHT"))
                    If .Rows(i)("RPFONTSIZE") = "" Then
                        v_objPageSetting.FontSize = "9.75"
                    Else
                        v_objPageSetting.FontSize = .Rows(i)("RPFONTSIZE")
                    End If

                    If .Rows(i)("RPPAPERSIZE") = "" Then
                        v_objPageSetting.PaperSize = "A4"
                    Else
                        v_objPageSetting.PaperSize = .Rows(i)("RPPAPERSIZE")
                    End If
                    v_objPageSetting.BusDate = Me.BusDate

                    v_intIsMember = .Rows(0)("ISMEMBER")
                End With
            Next
            v_objPageSetting.UserLanguage = Me.UserLanguage

            If v_intIsMember = 0 Then
                v_strsql = "SELECT * FROM BRGRP WHERE BRID='" & BranchId & "'"
                v_ds = v_obj.ExecuteReturnDataSet(v_strsql)

                v_objPageSetting.CompanyName = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("CompanyName")).ToUpper
                v_objPageSetting.Address = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("Address"))
                v_objPageSetting.BranchName = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("BranchName")).ToUpper
                v_objPageSetting.WhereDate = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("WhereDate"))
            Else
                v_strsql = "SELECT * FROM RGMI WHERE MICODE<>'000'"
                v_ds = v_obj.ExecuteReturnDataSet(v_strsql)

                v_objPageSetting.MIName = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("NAME")).ToUpper
                v_objPageSetting.MIAddress = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("ADDRESS"))
                v_objPageSetting.BUSINESS_NO = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("BUSINESS_NO"))
                v_objPageSetting.MIPhone = gf_CorrectStringField(v_ds.Tables(0).Rows(0)("PHONE"))
            End If
          
            'Lay cac truong bao cao
            v_strsql = "SELECT AUTOID,PARENT_ID,FIELDNAME,FIELDTYPE,CAPTION,EN_CAPTION,FORMAT,DISPLAY, " _
                    & "CONVERT(NUMERIC(5,2), WIDTH) AS WIDTH,ISDATAFIELD,ISSUM,ISPARENT,ALIGN,LEV,CONVERT(NUMERIC(5,2),HEIGHT) AS HEIGHT FROM RPFLD WHERE RPTID='" & v_objPageSetting.ReportID & "' ORDER BY POSITION"
            v_ds = v_obj.ExecuteReturnDataSet(v_strsql)
            Dim v_objTitleHeader As ReportHeaderRow
            Dim v_arrTitleHeader(v_ds.Tables(0).Rows.Count - 1) As ReportHeaderRow

            For v_int As Integer = 0 To v_ds.Tables(0).Rows.Count - 1
                v_objTitleHeader = New ReportHeaderRow
                With v_ds.Tables(0)
                    v_objTitleHeader.ID = .Rows(v_int)("AUTOID")
                    v_objTitleHeader.ParentID = .Rows(v_int)("PARENT_ID")
                    v_objTitleHeader.FieldName = .Rows(v_int)("FIELDNAME")
                    v_objTitleHeader.FieldType = .Rows(v_int)("FIELDTYPE")
                    v_objTitleHeader.Caption = .Rows(v_int)("CAPTION")
                    v_objTitleHeader.En_Caption = .Rows(v_int)("EN_CAPTION")
                    v_objTitleHeader.Format = .Rows(v_int)("FORMAT")
                    v_objTitleHeader.Display = .Rows(v_int)("DISPLAY")
                    v_objTitleHeader.Width = CDbl(.Rows(v_int)("WIDTH"))
                    v_objTitleHeader.IsDataField = .Rows(v_int)("ISDATAFIELD")
                    v_objTitleHeader.IsSum = .Rows(v_int)("ISSUM")
                    v_objTitleHeader.IsParent = .Rows(v_int)("ISPARENT")
                    v_objTitleHeader.TextAlign = .Rows(v_int)("ALIGN")
                    v_objTitleHeader.Lev = .Rows(v_int)("LEV")
                    v_objTitleHeader.Height = CDbl(.Rows(v_int)("HEIGHT"))
                End With
                v_arrTitleHeader(v_int) = v_objTitleHeader
            Next

            v_objPageSetting.ReportFld = v_arrTitleHeader

            'Lay group bao cao
            v_strsql = "SELECT * FROM RPGRP WHERE RPTID='" & v_objPageSetting.ReportID & "' ORDER BY POSITION"
            v_ds = v_obj.ExecuteReturnDataSet(v_strsql)
            If v_ds.Tables(0).Rows.Count > 0 Then
                Dim v_objGroup As ReportGroup
                Dim v_arrGroup(v_ds.Tables(0).Rows.Count - 1) As ReportGroup

                For v_int As Integer = 0 To v_ds.Tables(0).Rows.Count - 1
                    v_objGroup = New ReportGroup
                    With v_ds.Tables(0)
                        v_objGroup.FieldName = .Rows(v_int)("FIELDNAME")
                        v_objGroup.FldType = .Rows(v_int)("FIELDTYPE")
                        v_objGroup.Caption = .Rows(v_int)("CATION")
                        v_objGroup.En_Caption = .Rows(v_int)("EN_CATION")
                        v_objGroup.Format = .Rows(v_int)("FORMAT")
                        v_objGroup.CaptionWidth = CDbl(.Rows(v_int)("WIDTH"))
                        v_objGroup.Footer = .Rows(v_int)("GRPFOOTER")
                    End With
                    v_arrGroup(v_int) = v_objGroup
                Next
                v_objPageSetting.ReportGrp = v_arrGroup
            End If

            v_obj.CloseConnection()
            v_obj = Nothing
            Dim frm As New AppCore.frmReportView
            frm.ReportSetting = v_objPageSetting
            frm.ClientDataSet = m_BusLayer.mv_DataSet
            frm.UserLanguage = UserLanguage
            frm.Print = ("Y" = Mid(mv_strAuth, 3, 1))
            frm.ShowReport()
            frm.Show()
            v_oProcess.StopProcessForm()
        Catch ex As Exception
            v_oProcess.StopProcessForm()
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                          & "Error code: System error!" & vbNewLine _
                                          & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        Finally
            v_oProcess.StopProcessForm()
            v_oProcess = Nothing
        End Try
    End Sub

    Private Sub CreateReport(ByVal pv_strRPTID As String)
        'Dim v_thread As New Threading.Thread(AddressOf ShowFormProcess)
        Dim v_oProcess As New Sats.AppCore.ProcessForm(Me)
        Try

            v_oProcess.StartProcessForm()

            If Mid(mv_strAuth, 1, 1) <> "Y" Then
                v_oProcess.StopProcessForm()
                MsgBox(mv_ResourceManager.GetString("CreateReport"), MsgBoxStyle.Exclamation, gc_ApplicationTitle)
                Exit Sub
            End If
            Dim frm As AppCore.frmRPParameter
            If Not CheckExitsForm("frm" & pv_strRPTID) Then
                frm = New AppCore.frmRPParameter(m_BusLayer.mv_DataSet)
                frm.Name = "frm" & pv_strRPTID
                frm.UserLanguage = m_BusLayer.AppLanguage
                frm.ObjectName = pv_strRPTID
                frm.ModuleCode = ModuleCode
                frm.LocalObject = gc_IsInQueryNotLocalMsg
                frm.BranchId = m_BusLayer.CurrentTellerProfile.BranchId
                frm.TellerId = m_BusLayer.CurrentTellerProfile.TellerId
                frm.IpAddress = m_BusLayer.AppIpAddress
                frm.WsName = m_BusLayer.AppWsName
                frm.BusDate = m_BusLayer.CurrentTellerProfile.BusDate
                frm.MemberFilter = m_BusLayer.CurrentTellerProfile.MemberFilter
                frm.StockFilter = m_BusLayer.CurrentTellerProfile.StockFilter
                frm.AllMember = m_BusLayer.CurrentTellerProfile.AllMember
                frm.AllStock = m_BusLayer.CurrentTellerProfile.AllStock
                frm.BRCODE = m_BusLayer.CurrentTellerProfile.BranchName
                frm.TellerName = m_BusLayer.CurrentTellerProfile.TellerName
                Threading.Thread.Sleep(100)
                frm.Auth = mv_strAuth
                frm.ObjectType = "R"
                frm.PassDate = m_BusLayer.CurrentTellerProfile.PassDate
                frm.Proxy = Proxy
                frm.Client = Client
                frm.Show(Me.DockPanel)
            End If
            v_oProcess.StopProcessForm()
        Catch ex As Exception
            v_oProcess.StopProcessForm()
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                         & "Error code: System error!" & vbNewLine _
                                         & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        Finally
            v_oProcess = Nothing
        End Try
    End Sub

    Protected Overrides Function OnExecute() As Integer
        'Return MyBase.OnExecute()
        'Dim v_dlgOpen As New OpenFileDialog
        'v_dlgOpen.Filter = "Reports Document Files (*.rdf)|*.rdf"
        'v_dlgOpen.FileName = ""
        'v_dlgOpen.CheckFileExists = True
        'v_dlgOpen.Title = "Open a Report Document File"

        'If v_dlgOpen.ShowDialog(Me) = DialogResult.OK Then
        '    If File.Exists(v_dlgOpen.FileName) Then
        '        ' Load the RDF file
        '        Dim doc As New Document.Document
        '        doc.Load(v_dlgOpen.FileName)

        '        ' Open a new Preview Form
        '        Dim v_frm As New Sats.AppCore.frmReportView
        '        v_frm.OpenReport(doc, v_dlgOpen.FileName)
        '        v_frm.Show()
        '    End If
        'End If
    End Function

    Private Function CheckExitsForm(ByVal v_strName) As Boolean
        Dim o_frm As Sats.WinFormsUI.Docking.DockContent
        Dim v_blnIsExits As Boolean = False

        For Each o_frm In Me.DockPanel.Documents
            If o_frm.Name = v_strName Then
                v_blnIsExits = True
                o_frm.Activate()
                Exit For
            End If
        Next
        Return v_blnIsExits
    End Function

    Private Function MergeAuth(ByVal pv_strAuth1 As String, ByVal pv_strAuth2 As String) As String
        Dim v_strAuth As String
        v_strAuth = IIf(Mid(pv_strAuth1, 1, 1) = Mid(pv_strAuth2, 1, 1), Mid(pv_strAuth2, 1, 1), "Y")
        v_strAuth &= IIf(Mid(pv_strAuth1, 2, 1) = Mid(pv_strAuth2, 2, 1), Mid(pv_strAuth2, 2, 1), "Y")
        v_strAuth &= IIf(Mid(pv_strAuth1, 3, 1) = Mid(pv_strAuth2, 3, 1), Mid(pv_strAuth2, 3, 1), "Y")
        v_strAuth &= IIf(Mid(pv_strAuth1, 4, 1) = Mid(pv_strAuth2, 4, 1), Mid(pv_strAuth2, 4, 1), "Y")
        Return v_strAuth
    End Function

    'Private Function ReadFileToDataSet(ByVal pv_strRptID As String) As Boolean
    '    Dim v_ds As DataSet
    '    Dim v_bln As Boolean
    '    Dim v_Stream As System.IO.StreamReader
    '    Dim v_strFile As String
    '    Dim v_dLastWriteDate As Date
    '    Try
    '        If m_BusLayer.mv_DataSet.Tables.IndexOf(pv_strRptID) = -1 Then
    '            v_strFile = Application.LocalUserAppDataPath & "\" & pv_strRptID & ".xml"
    '            If System.IO.File.Exists(v_strFile) Then
    '                v_dLastWriteDate = System.IO.File.GetLastWriteTime(v_strFile)
    '                If FormatDateTime(v_dLastWriteDate.AddDays(m_BusLayer.CurrentTellerProfile.RptExprire), DateFormat.ShortDate) < FormatDateTime(m_BusLayer.CurrentTellerProfile.BusDate, DateFormat.ShortDate) Then
    '                    If MsgBox("Báo cáo này đã được tao cách đây 3 ngày. Bạn có muốn tạo lại báo cáo hay không?", MsgBoxStyle.YesNo, gc_ApplicationTitle) = MsgBoxResult.Yes Then
    '                        Return False
    '                    End If
    '                End If

    '                v_Stream = New System.IO.StreamReader(v_strFile)
    '                v_ds = New DataSet("RptDataSet")
    '                v_ds.ReadXml(v_Stream, XmlReadMode.ReadSchema)
    '                mv_DataSet = v_ds
    '                v_Stream.Close()
    '                v_Stream.Dispose()
    '                Return True
    '            Else
    '                MsgBox("Không tìm thấy dữ liệu báo cáo! Bạn phải tạo lại báo cáo!", gc_ApplicationTitle)
    '                Return False
    '            End If
    '        Else
    '            mv_DataSet = m_BusLayer.mv_DataSet
    '            Return True
    '        End If
    '    Catch ex As Exception
    '        Return False
    '        MsgBox("Lỗi đọc file báo cáo. Hãy liên hệ với Quản trị hệ thống để được giúp đỡ!", MsgBoxStyle.Exclamation, gc_ApplicationTitle)
    '    End Try
    'End Function

    Protected Overrides Function OnQuery() As Int32
        Dim v_strRptID As String
        v_strRptID = Replace(Trim(CType(SearchGrid.CurrentRow, Xceed.Grid.DataRow).Cells(KeyColumn).Value), ".", String.Empty)
        'If m_BusLayer.mv_DataSet.Tables.IndexOf(v_strRptID) = -1 Then
        'CreateReport(v_strRptID)
        'Else
        ShowReport()
        'End If
    End Function

End Class