Imports Sats.CommonLibrary
Imports System.IO
Public Class frmExportExcelForm
    Private mv_strFileName As String
    Private mv_strBranchId As String
    Private mv_strLanguage As String
    Private mv_strTellerId As String
    Private mv_strTltxcd As String
    Private mv_strModCode As String
    Private mv_strTransDesc As String

    Private mv_ResourceManager As Resources.ResourceManager
    'Private mv_BDSDelivery As New BDSChannel.BDSDelivery

    Private mv_arrObjFields() As CFieldMaster
    Dim mv_htbTLTXCD As New Hashtable
    Private mv_oProxy As BDSChannel.BDSDelivery

    Public Property Proxy() As BDSChannel.BDSDelivery
        Get
            Return mv_oProxy
        End Get
        Set(ByVal value As BDSChannel.BDSDelivery)
            mv_oProxy = value
        End Set
    End Property

#Region "Khai bao cac thuoc tinh"
    Public Property TransDesc() As String
        Get
            Return mv_strTransDesc
        End Get
        Set(ByVal value As String)
            mv_strTransDesc = value
        End Set
    End Property

    Public Property ModCode() As String
        Get
            Return mv_strModCode
        End Get
        Set(ByVal value As String)
            mv_strModCode = value
        End Set
    End Property
    Public Property TLTXCD() As String
        Get
            Return mv_strTltxcd
        End Get
        Set(ByVal value As String)
            mv_strTltxcd = value
        End Set
    End Property

    Public Property FileName() As String
        Get
            Return mv_strFileName
        End Get
        Set(ByVal value As String)
            mv_strFileName = value
        End Set
    End Property
    Public Property BranchId() As String
        Get
            Return mv_strBranchId
        End Get
        Set(ByVal Value As String)
            mv_strBranchId = Value
        End Set
    End Property

    Public Property UserLanguage() As String
        Get
            Return mv_strLanguage
        End Get
        Set(ByVal Value As String)
            mv_strLanguage = Value
        End Set
    End Property

    Public Property TellerId() As String
        Get
            Return mv_strTellerId
        End Get
        Set(ByVal Value As String)
            mv_strTellerId = Value
        End Set
    End Property
#End Region
#Region "Method"
    Private Sub OnInit()
        'Dim v_lwLogWriter As New LogWriter
        Try

            Dim v_strMethodName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
            'v_lwLogWriter.StartWriteLog(gc_MODULE_CLIENT, "Sats.App", "frmSearch", "Onsearch", _
            '"192.168.134.114", "Bank-Hanm5", "210", "Tuanpa", "abc")
            mv_ResourceManager = New Resources.ResourceManager(gc_RootNamespace & ".frmExportExcelForm_" & UserLanguage, System.Reflection.Assembly.GetExecutingAssembly())
            LoadUserInterface(Me)

            Dim v_strSql As String = "SELECT MODCODE VALUE, MODCODE || ' - ' || MODNAME DISPLAY FROM APPMODULES WHERE STATUS = 0 AND DELETED = 0 ORDER BY MODCODE"
            FillDataCombobox(v_strSql, cboSysPart)
            'v_lwLogWriter.WriteLog(gc_MODULE_CLIENT, "Sats.App", "frmSearch", "Onsearch", _
            '"192.168.134.114", "Bank-Hanm5", "210", "Tuanpa", "abc")
        Catch ex As Exception
            'v_lwLogWriter.WriteLog(gc_MODULE_CLIENT, "Sats.App", "frmSearch", "Onsearch", _
            '"192.168.134.114", "Bank-Hanm5", "210", "Tuanpa", ex.ToString)
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                           & "Error code: System error!" & vbNewLine _
                                           & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        Finally
            'v_lwLogWriter.StopWriteLog(gc_MODULE_CLIENT, "Sats.App", "frmSearch", "Onsearch", _
            '"192.168.134.114", "Bank-Hanm5", "210", "Tuanpa", "abc")
        End Try

    End Sub


    Private Sub LoadUserInterface(ByRef pv_ctrl As Windows.Forms.Control)
        Dim v_ctrl As Windows.Forms.Control
        Try
            For Each v_ctrl In pv_ctrl.Controls
                If TypeOf (v_ctrl) Is Panel Then
                    LoadUserInterface(v_ctrl)
                ElseIf TypeOf (v_ctrl) Is GroupBox Then
                    CType(v_ctrl, GroupBox).Text = mv_ResourceManager.GetString(v_ctrl.Tag)
                    LoadUserInterface(v_ctrl)
                ElseIf TypeOf (v_ctrl) Is Label Then
                    CType(v_ctrl, Label).Text = mv_ResourceManager.GetString(v_ctrl.Tag)
                ElseIf TypeOf (v_ctrl) Is Button Then
                    CType(v_ctrl, Button).Text = mv_ResourceManager.GetString(v_ctrl.Tag)
                End If
            Next

            'Load caption của form, label caption
            Me.Text = mv_ResourceManager.GetString("frmExportExcelForm")
            lbCaption.Text = mv_ResourceManager.GetString("lbCaption")
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub ExportExcelFile()
        Try
            Dim v_intIndex As Integer = 0
            Dim v_sbFile As New DataDynamics.SpreadBuilder.Workbook()
            v_sbFile.Sheets.AddNew()
            'Set up properties and values for columns, rows and cells as desired
            With v_sbFile.Sheets(0)
                .Name = TLTXCD

                'Tao header cho mau file import
                .Cell(0, 8).Merge(0, 2)
                .Cell(0, 8).SetValue("(Mẫu VSD/" & TLTXCD & ")")
                .Cell(0, 8).FontItalic = True
                .Cell(0, 8).FontBold = True
                .Cell(1, 0).Merge(0, 1)
                .Cell(1, 0).SetValue("Thành viên lưu ký:")
                .Cell(1, 0).FontBold = True
                .Cell(1, 0).Alignment = DataDynamics.SpreadBuilder.Style.HorzAlignments.Left

                .Cell(1, 4).Merge(0, 3)
                .Cell(1, 4).SetValue("CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM")
                .Cell(1, 4).FontBold = True
                .Cell(1, 4).Alignment = DataDynamics.SpreadBuilder.Style.HorzAlignments.Center

                .Cell(2, 0).Merge(0, 1)
                .Cell(2, 0).SetValue("Trụ sở chính")
                .Cell(2, 0).FontBold = False
                .Cell(2, 0).Alignment = DataDynamics.SpreadBuilder.Style.HorzAlignments.Left

                .Cell(2, 4).Merge(0, 3)
                .Cell(2, 4).SetValue("Độc lập - Tự do - Hạnh phúc")
                .Cell(2, 4).FontBold = True
                .Cell(2, 4).Alignment = DataDynamics.SpreadBuilder.Style.HorzAlignments.Center

                .Cell(3, 0).Merge(0, 1)
                .Cell(3, 0).SetValue("Số điện thoại")
                .Cell(3, 0).FontBold = False
                .Cell(3, 0).Alignment = DataDynamics.SpreadBuilder.Style.HorzAlignments.Left

                .Cell(4, 0).Merge(0, 1)
                .Cell(4, 0).SetValue("Số đăng ký TVLK")
                .Cell(4, 0).FontBold = False
                .Cell(4, 0).Alignment = DataDynamics.SpreadBuilder.Style.HorzAlignments.Left

                .Cell(5, 0).Merge(0, 10)
                .Cell(5, 0).SetValue("DANH SÁCH " & Mid(TransDesc, 8).ToUpper)
                .Cell(5, 0).FontSize = 16
                .Cell(5, 0).FontBold = True
                .Cell(5, 0).Alignment = DataDynamics.SpreadBuilder.Style.HorzAlignments.Center

                .Cell(7, 1).SetValue("Kính gửi:")
                .Cell(7, 1).FontBold = True
                .Cell(7, 1).FontItalic = True
                .Cell(7, 1).FontUnderlineStyle = DataDynamics.SpreadBuilder.Style.FontUnderlineStyles.Single
                .Cell(7, 1).Alignment = DataDynamics.SpreadBuilder.Style.HorzAlignments.Left

                .Cell(7, 2).Merge(0, 3)
                .Cell(7, 2).SetValue("Trung tâm Lưu ký Chứng khoán")
                .Cell(7, 2).FontBold = True
                .Cell(7, 2).Alignment = DataDynamics.SpreadBuilder.Style.HorzAlignments.Left

                .Cell(9, 0).Merge(0, 8)
                .Cell(9, 0).SetValue("Công ty chứng khoán ... gửi đến Trung tâm Lưu ký Chứng khoán danh sách " & Mid(TransDesc, 8) & " của chứng khoán sau :")
                .Cell(9, 0).FontBold = True
                .Cell(9, 0).WrapText = True
                .Cell(9, 0).Alignment = DataDynamics.SpreadBuilder.Style.HorzAlignments.Left

                'Footer của form import

                .Cell(24, 5).Merge(0, 3)
                .Cell(24, 5).SetValue("....., ngày ... tháng ... năm ....")
                .Cell(24, 5).FontBold = False
                .Cell(24, 5).Alignment = DataDynamics.SpreadBuilder.Style.HorzAlignments.Left

                .Cell(25, 5).Merge(0, 3)
                .Cell(25, 5).SetValue("Giám đốc")
                .Cell(25, 5).FontBold = True
                .Cell(25, 5).Alignment = DataDynamics.SpreadBuilder.Style.HorzAlignments.Center

                .Cell(25, 5).Merge(0, 3)

                'Cac cot du lieu de import
                .Columns(0).Width = 800
                .Cell(12, 0).SetValue("STT")
                .Cell(12, 0).FontBold = True
                .Cell(12, 0).FillColor = Color.Gray
                .Cell(12, 0).WrapText = True
                .Cell(12, 0).ForeColor = Color.Purple
                .Cell(12, 0).Alignment = DataDynamics.SpreadBuilder.Style.HorzAlignments.Center
                .Cell(12, 0).BorderBottomStyle = DataDynamics.SpreadBuilder.Style.BorderLineStyle.Dotted
                .Cell(12, 0).BorderBottomColor = Color.Black
                .Cell(12, 0).BorderLeftColor = Color.Black
                .Cell(12, 0).BorderLeftStyle = DataDynamics.SpreadBuilder.Style.BorderLineStyle.Dotted
                .Cell(12, 0).BorderRightColor = Color.Black
                .Cell(12, 0).BorderRightStyle = DataDynamics.SpreadBuilder.Style.BorderLineStyle.Dotted
                .Cell(12, 0).BorderTopColor = Color.Black
                .Cell(12, 0).BorderTopStyle = DataDynamics.SpreadBuilder.Style.BorderLineStyle.Dotted

                'Ghi chu
                .Cell(27, 1).SetValue("Ghi chú: ")
                .Cell(27, 1).FontBold = True
                .Cell(27, 1).FontItalic = True
                .Cell(27, 1).FontUnderlineStyle = DataDynamics.SpreadBuilder.Style.FontUnderlineStyles.Single
                .Cell(27, 1).WrapText = True
                .Cell(27, 1).Alignment = DataDynamics.SpreadBuilder.Style.HorzAlignments.Left

                For v_intI As Integer = 12 To 22
                    .Cell(v_intI, 0).Alignment = DataDynamics.SpreadBuilder.Style.HorzAlignments.Center
                    .Cell(v_intI, 0).BorderBottomColor = Color.Black
                    .Cell(v_intI, 0).BorderBottomStyle = DataDynamics.SpreadBuilder.Style.BorderLineStyle.Dotted
                    .Cell(v_intI, 0).BorderLeftColor = Color.Black
                    .Cell(v_intI, 0).BorderLeftStyle = DataDynamics.SpreadBuilder.Style.BorderLineStyle.Dotted
                    .Cell(v_intI, 0).BorderRightColor = Color.Black
                    .Cell(v_intI, 0).BorderRightStyle = DataDynamics.SpreadBuilder.Style.BorderLineStyle.Dotted
                    .Cell(v_intI, 0).BorderTopColor = Color.Black
                    .Cell(v_intI, 0).BorderTopStyle = DataDynamics.SpreadBuilder.Style.BorderLineStyle.Dotted
                    .Cell(v_intI, 0).WrapText = True
                    .Cell(v_intI, 0).NumberFormat = "[0-9]#,##0.00"
                Next

                Dim v_intArrLen As Integer = mv_arrObjFields.Count
                For i As Integer = 0 To mv_arrObjFields.Count - 2
                    Dim v_objField As CFieldMaster = mv_arrObjFields(i)
                    If v_objField.IsImported Then
                        v_intIndex = v_intIndex + 1
                        '.Columns(v_intIndex).Width = CInt(v_objField.Caption.Length) * 100
                        .Columns(v_intIndex).Width = 1440
                        With .Cell(12, v_intIndex)
                            .SetValue(v_objField.Caption)
                            .FontBold = True
                            .Alignment = DataDynamics.SpreadBuilder.Style.HorzAlignments.Center
                            .BorderBottomColor = Color.Black
                            .BorderBottomStyle = DataDynamics.SpreadBuilder.Style.BorderLineStyle.Dotted
                            .BorderLeftColor = Color.Black
                            .BorderLeftStyle = DataDynamics.SpreadBuilder.Style.BorderLineStyle.Dotted
                            .BorderRightColor = Color.Black
                            .BorderRightStyle = DataDynamics.SpreadBuilder.Style.BorderLineStyle.Dotted
                            .BorderTopColor = Color.Black
                            .BorderTopStyle = DataDynamics.SpreadBuilder.Style.BorderLineStyle.Dotted
                            .FillColor = Color.Gray
                            .WrapText = True
                            .ForeColor = Color.Purple
                        End With

                        For v_intI As Integer = 12 To 22
                            .Cell(v_intI, v_intIndex).Alignment = DataDynamics.SpreadBuilder.Style.HorzAlignments.Center
                            .Cell(v_intI, v_intIndex).BorderBottomColor = Color.Black
                            .Cell(v_intI, v_intIndex).BorderBottomStyle = DataDynamics.SpreadBuilder.Style.BorderLineStyle.Dotted
                            .Cell(v_intI, v_intIndex).BorderLeftColor = Color.Black
                            .Cell(v_intI, v_intIndex).BorderLeftStyle = DataDynamics.SpreadBuilder.Style.BorderLineStyle.Dotted
                            .Cell(v_intI, v_intIndex).BorderRightColor = Color.Black
                            .Cell(v_intI, v_intIndex).BorderRightStyle = DataDynamics.SpreadBuilder.Style.BorderLineStyle.Dotted
                            .Cell(v_intI, v_intIndex).BorderTopColor = Color.Black
                            .Cell(v_intI, v_intIndex).BorderTopStyle = DataDynamics.SpreadBuilder.Style.BorderLineStyle.Dotted
                            .Cell(v_intI, v_intIndex).WrapText = True
                            If v_objField.DataType = "C" Then
                                .Cell(v_intI, v_intIndex).NumberFormat = "@"
                            ElseIf v_objField.DataType = "D" Then
                                .Cell(v_intI, v_intIndex).NumberFormat = "dd/MM/yyyy"
                            End If
                        Next

                        If v_objField.LookupList <> "" Then
                            .Cell(28, v_intIndex).SetValue("* " & v_objField.Caption)
                            .Cell(28, v_intIndex).FontBold = True
                            .Cell(28, v_intIndex).WrapText = True
                            .Cell(28, v_intIndex).Alignment = DataDynamics.SpreadBuilder.Style.HorzAlignments.Left
                            Dim v_intRowCount As Integer = 0
                            Dim v_strLookUp As String = GetLookUpList(v_objField.LookupList)
                            If Trim(v_strLookUp) <> "" Then
                                Dim v_strLookUpList() As String

                                v_strLookUp = Mid(v_strLookUp, 1, v_strLookUp.Length - 1)
                                v_strLookUpList = v_strLookUp.Split(",")

                                v_intRowCount = v_strLookUpList.Length

                                If v_intRowCount > 0 Then
                                    For k As Integer = 0 To v_intRowCount - 1
                                        If TLTXCD = "3138" Then
                                            .Cell(29 + k, v_intIndex).SetValue(Replace(v_strLookUpList(k).ToString, "<<Tất cả>>", "-1(Tất cả)"))
                                        Else
                                            .Cell(29 + k, v_intIndex).SetValue(v_strLookUpList(k).ToString)
                                        End If
                                        .Cell(29 + k, v_intIndex).FontBold = False
                                        .Cell(29 + k, v_intIndex).WrapText = True
                                        .Cell(29 + k, v_intIndex).Alignment = DataDynamics.SpreadBuilder.Style.HorzAlignments.Left
                                    Next
                                End If
                            End If
                        End If
                    End If
                Next
            End With

            'Save the Workbook to an Excel file
            v_sbFile.Save(FileName)
            MessageBox.Show("Mẫu file Import được lưu tại địa chỉ: " & FileName)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub


    Private Function GetLookUpList(ByRef pv_strLookUpList As String) As String
        Dim v_obj As SQLEngine.SQLDataAccessLayer
        Dim v_ds As DataSet
        Try
            Dim v_strValue As String = "", v_strFLDNAME As String = ""
            Dim v_strCDVAL As String = "", v_strCDCONTENT As String = ""
            Dim v_strReturnList As String
            Dim v_strObjMsg As String
           
            v_obj = New SQLEngine.SQLDataAccessLayer(TellerId)
            v_ds = v_obj.ExecuteReturnDataSet(pv_strLookUpList)

            For i As Integer = 0 To v_ds.Tables(0).Rows.Count - 1
                v_strReturnList = v_strReturnList & v_ds.Tables(0).Rows(i)("DISPLAY") & ","
            Next
            Return v_strReturnList
        Catch ex As Exception
            Throw ex
        Finally
            v_obj.CloseConnection()
            v_obj = Nothing
            v_ds.Dispose()
        End Try
    End Function

    Private Sub FillDataCombobox(ByVal pv_strSql As String, ByRef pv_cboCombo As Sats.AppCore.ComboBoxEx)
        Try
            Dim v_xmlDocument As New Xml.XmlDocument
            Dim v_nodeList As Xml.XmlNodeList = Nothing
            Dim v_strValue As String = "", v_strFLDNAME As String = ""
            Dim v_strCDVAL As String = "", v_strCDCONTENT As String = ""
            Dim v_strChildTlTxcd As String = ""

            Dim v_strObjMsg As String
            'Dim strRow As String = "SELECT MODCODE VALUE, MODCODE || ' - ' || MODNAME DISPLAY FROM APPMODULES WHERE STATUS = 0 AND DELETED = 0 ORDER BY MODCODE"
            v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLGROUPS, gc_ActionInquiry, pv_strSql)
            Dim v_lngError As Long = Proxy.Message(v_strObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Exit Sub
            End If

            v_xmlDocument.LoadXml(v_strObjMsg)
            v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")

            pv_cboCombo.Clears()
            mv_htbTLTXCD.Clear()

            For i As Integer = 0 To v_nodeList.Count - 1
                For j As Integer = 0 To v_nodeList.Item(i).ChildNodes.Count - 1
                    With v_nodeList.Item(i).ChildNodes(j)
                        v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                        v_strValue = .InnerText.ToString()
                    End With
                    Select Case Trim(v_strFLDNAME)
                        Case "VALUE"
                            v_strCDVAL = Trim(v_strValue)
                        Case "DISPLAY"
                            v_strCDCONTENT = Trim(v_strValue)
                        Case "CHILDTLTXCD"
                            v_strChildTlTxcd = Trim(v_strValue)
                    End Select
                Next
                pv_cboCombo.AddItems(v_strCDCONTENT, v_strCDVAL)
                If Trim(pv_cboCombo.Name) = "cboTransCode" Then
                    If Not mv_htbTLTXCD.ContainsKey(v_strCDVAL) Then
                        mv_htbTLTXCD.Add(v_strCDVAL, v_strChildTlTxcd)
                    End If
                End If
            Next
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub LoadObjFields()
        Dim v_strClause, v_strObjMsg As String
        Dim v_xmlDocument As New Xml.XmlDocument, v_xmlDocumentData As New Xml.XmlDocument

        Dim v_nodeList As Xml.XmlNodeList
        Dim v_strValue, v_strFLDNAME As String
        Dim i, j As Integer
        Dim v_objField As CFieldMaster
        Dim v_strFieldName As String = "", v_strDefName As String = "", v_strCaption As String = ""
        Dim v_strFldType As String = "", v_strFldMask As String = "", v_strFldFormat As String = ""
        Dim v_strLList As String = "", v_strLChk As String = "", v_strDefVal As String = ""
        Dim v_strAmtExp As String = "", v_strValidTag As String = "", v_strLookUp As String = ""
        Dim v_strDataType As String = "", v_strControlType As String = "", v_strChainName As String = ""
        Dim v_strLookupName As String = "", v_strPrintInfo As String = "", v_strInvName As String = ""
        Dim v_strInvFormat As String = "", v_strFldSource As String = "", v_strFldDesc As String = ""
        Dim v_strSearchCode As String = "", v_strSrModCode As String = "", v_strMemberField As String = ""
        Dim v_strStockField As String = "", v_strIsDuplicated As String = "", v_strIsImported As String
        Dim v_intOdrNum, v_intCoOdrNum, v_intFldLen, v_intIndex As Integer
        Dim v_blnVisible, v_blnEnabled, v_blnMandatory As Boolean
        Dim v_obj As SQLEngine.SQLDataAccessLayer
        Dim v_ds As DataSet
        Try
            v_strClause = "SELECT * FROM FLDMASTER WHERE upper(OBJNAME) = '" & TLTXCD & "' and deleted =0 and status =0 ORDER BY COODRNUM"
            v_obj = New SQLEngine.SQLDataAccessLayer(TellerId)
            v_ds = v_obj.ExecuteReturnDataSet(v_strClause)
            ReDim mv_arrObjFields(v_ds.Tables(0).Rows.Count)

            For i = 0 To v_ds.Tables(0).Rows.Count - 1
                With v_ds.Tables(0)
                    v_strFieldName = Trim(.Rows(i)("FLDNAME"))
                    v_strDefName = Trim(.Rows(i)("DEFNAME"))
                    If UserLanguage <> "EN" Then
                        v_strCaption = Trim(.Rows(i)("CAPTION"))
                    Else
                        v_strCaption = Trim(.Rows(i)("EN_CAPTION"))
                    End If
                    v_intOdrNum = CInt(Trim(.Rows(i)("ODRNUM")))
                    v_strFldType = Trim(.Rows(i)("FLDTYPE"))
                    v_strFldMask = Trim(.Rows(i)("FLDMASK"))
                    v_strFldFormat = Trim(.Rows(i)("FLDFORMAT"))
                    v_intFldLen = CInt(Trim(.Rows(i)("FLDLEN")))
                    v_strLList = Trim(.Rows(i)("LLIST"))
                    If v_strLList <> "" Then
                        v_strLList = Replace(v_strLList, "?BRID", BranchId)
                    End If
                    v_strLChk = Trim(.Rows(i)("LCHK"))
                    v_strDefVal = Trim(.Rows(i)("DEFVAL"))
                    v_blnVisible = (Trim(.Rows(i)("VISIBLE")) = "Y")
                    v_blnEnabled = (Trim(.Rows(i)("DISABLE")) = "N")
                    v_blnMandatory = (Trim(.Rows(i)("MANDATORY")) = "Y")
                    v_strAmtExp = Trim(.Rows(i)("AMTEXP"))
                    v_strValidTag = Trim(.Rows(i)("VALIDTAG"))
                    v_strLookUp = Trim(.Rows(i)("LOOKUP"))
                    v_strDataType = Trim(.Rows(i)("DATATYPE"))
                    v_strControlType = Trim(.Rows(i)("CTLTYPE"))
                    v_strInvName = Trim(.Rows(i)("INVNAME"))
                    v_strInvFormat = Trim(.Rows(i)("INVFORMAT"))
                    v_strFldSource = Trim(.Rows(i)("FLDSOURCE"))
                    v_strFldDesc = Trim(.Rows(i)("FLDDESC"))
                    v_strChainName = Trim(.Rows(i)("CHAINNAME"))
                    v_strLookupName = Trim(.Rows(i)("LOOKUPNAME"))
                    v_strSearchCode = Trim(.Rows(i)("SEARCHCODE"))
                    v_strSrModCode = Trim(.Rows(i)("SRMODCODE"))
                    v_strPrintInfo = .Rows(i)("PRINTINFO") 'KhÃ´ng Ä‘Æ°á»£c trim vÃ¬ Ä‘á»™ dÃ i báº¯t buá»™c 10 kÃ½ tá»±
                    v_strMemberField = Trim(.Rows(i)("MEMBERFIELD"))
                    v_strStockField = Trim(.Rows(i)("STOCKFIELD"))
                    v_strIsImported = Trim(.Rows(i)("ISIMPORTED"))
                    v_intCoOdrNum = CInt(Trim(.Rows(i)("COODRNUM")))
                End With

                v_objField = New CFieldMaster
                With v_objField
                    .FieldName = v_strFieldName
                    .ColumnName = v_strDefName
                    .Caption = v_strCaption
                    .DisplayOrder = v_intOdrNum
                    .FieldType = v_strFldType
                    .InputMask = v_strFldMask
                    .FieldFormat = v_strFldFormat
                    .FieldLength = v_intFldLen
                    .LookupList = v_strLList
                    .LookupCheck = v_strLChk
                    .LookupName = v_strLookupName
                    .DefaultValue = v_strDefVal
                    .Visible = v_blnVisible
                    .Enabled = v_blnEnabled
                    .Mandatory = v_blnMandatory
                    .AmtExp = v_strAmtExp
                    .ValidTag = v_strValidTag
                    .LookUp = v_strLookUp
                    .DataType = v_strDataType
                    .ControlType = v_strControlType
                    .InvName = v_strInvName
                    .InvFormat = v_strInvFormat
                    .FldSource = v_strFldSource
                    .FldDesc = v_strFldDesc
                    .PrintInfo = v_strPrintInfo
                    .SearchCode = v_strSearchCode
                    .SrModCode = v_strSrModCode
                    .FieldValue = String.Empty
                    .MemberField = v_strMemberField
                    .StockField = v_strStockField
                    If v_strIsImported = "Y" Then
                        .IsImported = True
                    Else
                        .IsImported = False
                    End If
                    .CoOdrNum = v_intCoOdrNum
                End With
                mv_arrObjFields(i) = v_objField
            Next
            ReDim Preserve mv_arrObjFields(v_ds.Tables(0).Rows.Count)

        Catch ex As Exception
            Throw ex
        Finally
            v_obj.CloseConnection()
            v_obj = Nothing
            v_ds.Dispose()
        End Try
    End Sub

    Private Sub OnSubmit()
        Try
            LoadObjFields()
            ExportExcelFile()
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                          & "Error code: System error!" & vbNewLine _
                                          & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Sub
#End Region
    Private Sub cboSysPart_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboSysPart.SelectedIndexChanged
        Dim v_strSysPart As String
        Dim v_strSqlCmd As String
        Try
            v_strSysPart = cboSysPart.SelectedValue.ToString
            If Not v_strSysPart = "" Then
                v_strSqlCmd = "SELECT A.TLTXCD VALUE, A.TLTXCD || ' - ' || A.TXDESC DISPLAY, A.CHILDTLTXCD FROM TLTX A, APPMODULES B WHERE A.DELETED = 0 AND " _
                            & " A.STATUS = 0 AND SUBSTR(A.TLTXCD,1,2) = B.TXCODE AND B.MODCODE = '" & v_strSysPart & "' AND NOT A.CHILDTLTXCD IS NULL ORDER BY A.TLTXCD"
                FillDataCombobox(v_strSqlCmd, cboTransCode)
                ModCode = v_strSysPart
            End If

        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                          & "Error code: System error!" & vbNewLine _
                                          & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Sub


    Private Sub frmExportExcelForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        OnInit()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub cboTransCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboTransCode.SelectedIndexChanged

    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Try
            If cboTransCode.Items.Count = 0 Then
                MsgBox(mv_ResourceManager.GetString("NoTransaction"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                Cursor = Cursors.Default
                Exit Sub
            End If
            TLTXCD = cboTransCode.SelectedValue.ToString
            TransDesc = cboTransCode.GetItemText(cboTransCode.SelectedItem)
            Cursor = Cursors.WaitCursor
            If Trim(TLTXCD) = "" Then
                MsgBox(mv_ResourceManager.GetString("NotSelectTransaction"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                Cursor = Cursors.Default
                Exit Sub
            Else
                TLTXCD = mv_htbTLTXCD.Item(TLTXCD).ToString.Trim
                If Trim(TLTXCD) = "" Then
                    MsgBox(mv_ResourceManager.GetString("NotIsImportTransaction"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                    Cursor = Cursors.Default
                    Exit Sub
                Else
                    FileName = txtFileLocation.Text
                    If FileName <> "" Then
                        OnSubmit()
                        Cursor = Cursors.Default
                    Else
                        MsgBox(mv_ResourceManager.GetString("NotInputFileName"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                        Cursor = Cursors.Default
                        Exit Sub
                    End If
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
        Dim v_sfdSaveFile As New SaveFileDialog
        v_sfdSaveFile.Filter = "Excel(*.xls)|*.xls|File XML(*.xml)|*.xml|File Text(*.txt)|*.txt"
        v_sfdSaveFile.ShowDialog()
        Me.txtFileLocation.Text = v_sfdSaveFile.FileName
    End Sub
End Class