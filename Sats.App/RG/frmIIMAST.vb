Imports Sats.CommonLibrary
Public Class frmRGII
    Dim mv_lngIiCode As String
    Dim v_htbRepPerson As Hashtable

#Region "Khai báo thuộc tính"

    Public Property IiCode() As String
        Get
            Return mv_lngIiCode
        End Get
        Set(ByVal value As String)
            mv_lngIiCode = value
        End Set
    End Property
#End Region

#Region "Overrides method"

    Public Overrides Sub OnInit()
        Try

            MyBase.OnInit()

            'Load Resource Manager
            ResourceManager = New Resources.ResourceManager(gc_RootNamespace & "." & Me.Name & "_" & UserLanguage, System.Reflection.Assembly.GetExecutingAssembly())
            LoadUserInterface(Me)

            FillLstRepPerson(IiCode)

            Me.TabText = ResourceManager.GetString(Me.Name)
            Me.Text = ResourceManager.GetString(Me.Name)

            lbCaption.Text = ResourceManager.GetString(lbCaption.Tag & ExeFlag.ToString())

        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                         & "Error code: System error!" & vbNewLine _
                         & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(ResourceManager.GetString("InitDialogFailed"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
        End Try
    End Sub

    Public Overrides Sub LoadUserInterface(ByRef pv_ctrl As System.Windows.Forms.Control)
        MyBase.LoadUserInterface(pv_ctrl)

        If (ExeFlag = ExecuteFlag.AddNew) Then
            btnApply.Dispose()
            btnOk.Enabled = True
        ElseIf (ExeFlag = ExecuteFlag.View) Then
            btnApply.Dispose()
            btnOk.Enabled = False
        ElseIf (ExeFlag = ExecuteFlag.Edit) Then
            btnApply.Dispose()
            btnOk.Enabled = True
        End If
        If ExeFlag = ExecuteFlag.View Or ExeFlag = ExecuteFlag.Edit Then
            Dim v_intType = GetTypeValue()
            If v_intType = 1 Or v_intType = 3 Or v_intType = 4 Then
                SetTagForTpOrganization()
            Else
                SetTagForTpPersonal()
            End If
        End If
    End Sub

    Public Overloads Sub OnSave(ByVal sender As Object)
        Dim v_strObjMsg As String
        Try
            'Update mouse pointer
            Cursor = Cursors.WaitCursor

            MyBase.OnSave()
            If Not DoDataExchange(True) Then
                Exit Sub
            End If

            Select Case ExeFlag
                Case ExecuteFlag.AddNew
                    'Edited by Thanglv9 - 14/12/2012 - them tham so VsdBrid,TellerName,IpAddress,Wsname,Tabtext,BRCODE,BUSDATE
                    v_strObjMsg = BuildXMLObjMsg(Me.BusDate, BranchId, , TellerId, gc_IsNotLocalMsg, gc_MsgTypeObj, ObjectName, gc_ActionAdd, , , , gc_AutoIdUsed, , , , VSDBRID, , , TellerName, IpAddress, WsName, TabText, BRCODE, Me.BusDate)
                    BuildXMLObjData(mv_dsInput, v_strObjMsg)

                    'Dim v_ws As New BDSDelivery.BDSDelivery
                    'mv_BDSDelivery.Message(v_strObjMsg)
                    Proxy.Message(v_strObjMsg)

                    'Kiểm tra thông tin và xử lý lỗi (nếu có) từ message trả về
                    Dim v_strErrorSource As String = "", v_strErrorMessage As String = ""
                    Dim v_lngErrorCode As Long

                    GetErrorFromMessage(v_strObjMsg, v_strErrorSource, v_lngErrorCode, v_strErrorMessage)
                    If v_lngErrorCode <> 0 Then
                        'Update mouse pointer
                        Cursor = Cursors.Default
                        If v_lngErrorCode = ERR_RG_SI_CODE_DUPLICATED Then
                            MsgBox(ResourceManager.GetString("ERR_RG_II_CARD_DUPLICATED"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                            Exit Sub
                        Else
                            MsgBox(v_strErrorMessage, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, Me.Text)
                            Exit Sub
                        End If
                        Cursor = Cursors.Default
                    End If

                    If sender Is btnOk Then
                        'SearchForm.Activate()
                        MsgBox(ResourceManager.GetString("AddnewSuccessful"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                        MyBase.OnClose()
                    End If

                Case ExecuteFlag.Edit
                    Dim v_strClause As String = ""

                    Select Case KeyFieldType
                        Case "C"
                            v_strClause = KeyFieldName & " = '" & KeyFieldValue & "'"
                        Case "D"
                            v_strClause = KeyFieldName & " = TO_DATE('" & KeyFieldValue & "', '" & gc_FORMAT_DATE & "')"
                        Case "N"
                            v_strClause = KeyFieldName & " = " & KeyFieldValue.ToString()
                    End Select

                    'Editted by Thanglv9 - 16/12/2012
                    v_strObjMsg = BuildXMLObjMsg(Me.BusDate, BranchId, , TellerId, gc_IsNotLocalMsg, gc_MsgTypeObj, ObjectName, gc_ActionEdit, , v_strClause, , , , , , VSDBRID, , , TellerName, IpAddress, WsName, lbCaption.Text.ToString, BRCODE, Me.BusDate)
                    'End
                    BuildXMLObjData(mv_dsInput, v_strObjMsg, mv_dsOldInput, ExecuteFlag.Edit)

                    'Dim v_ws As New BDSDelivery.BDSDelivery
                    'mv_BDSDelivery.Message(v_strObjMsg)
                    Proxy.Message(v_strObjMsg)

                    'Kiểm tra thông tin và xử lý lỗi (nếu có) từ message trả về
                    Dim v_strErrorSource As String = "", v_strErrorMessage As String = ""
                    Dim v_lngErrorCode As Long

                    GetErrorFromMessage(v_strObjMsg, v_strErrorSource, v_lngErrorCode, v_strErrorMessage)
                    If v_lngErrorCode <> 0 Then
                        'Update mouse pointer
                        Cursor = Cursors.Default
                        MsgBox(v_strErrorMessage, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, Me.Text)
                        Exit Sub
                        Cursor = Cursors.Default
                    End If

                    MsgBox(ResourceManager.GetString("EditSuccessful"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                    If sender Is btnOk Then
                        'SearchForm.Activate()
                        MyBase.OnClose()
                    End If
            End Select
            'Me.DialogResult = DialogResult.OK
            'Update mouse pointer
            Cursor = Cursors.Default
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                         & "Error code: System error!" & vbNewLine _
                         & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(ResourceManager.GetString("SavingFailed"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
            Cursor = Cursors.Default
        End Try
    End Sub


    Public Overrides Function DoDataExchange(Optional ByVal pv_blnSaved As Boolean = False) As Boolean
        Try
            If Not ControlValidation(pv_blnSaved) Then
                Return False
            End If

            Return MyBase.DoDataExchange(pv_blnSaved)
        Catch ex As Exception
            Throw ex
            Return False
        End Try

    End Function

#End Region

#Region " Control validations "

    Private Function ControlValidation(Optional ByVal pv_blnSaved As Boolean = False) As Boolean
        Try
            If pv_blnSaved Then
                Return MyBase.VerifyRules()
            End If

            Return True
        Catch ex As Exception
            Throw ex
            Return False
        End Try
    End Function
#End Region
    Private Sub SetTagForTpOrganization()
        ' Personal tabpage
        tctRGII.SelectedIndex = 0
        cboType.Tag = "TYPE"
        txtFullName.Tag = "FULL_NAME"
        cboSex.Tag = "SEX"
        dtpBirthDate.Tag = "BIRTH_DATE"
        txtBirthPlace.Tag = "BIRTH_PLACE"
        txtCardNo.Tag = "CARDNO"
        txtCardIssue.Tag = "CARDISSUE"
        cboOriginalNationaly.Tag = "ORIGINAL_NATIONALY"
        cboCurrentNationaly.Tag = "CURRENT_NATIONALY"
        txtNation.Tag = "NATION"
        txtAddress.Tag = "ADDRESS"
        txtEducationLevel.Tag = "EDUCATION_LEVEL"
        txtOccupation.Tag = "OCCUPATION"
        txtPost.Tag = "POST"
        txtPhone.Tag = "PHONE"
        txtMobiphone.Tag = "MOBILEPHONE"
        txtFax.Tag = "FAX"
        txtEmail.Tag = "EMAIL"
        txtNote.Tag = "NOTE"
        txtForeignNo.Tag = "FOREIGNNO"

        'Organization tabpage

        cboOrgType.Tag = "TYPE_2"
        txtOrgFullName.Tag = "FULL_NAME_2"
        txtOrgTransName.Tag = "FIRST_NAME_2"
        txtOrgShortName.Tag = "LAST_NAME_2"

        txtOrgCardNo.Tag = "CARDNO_2"
        txtFoundationIssue.Tag = "BIRTH_PLACE_2"
        dtpFoundationDate.Tag = "BIRTH_DATE_2"
        lstOrgRepPerson.Tag = "REPNO_2"
        txtFoudationNo.Tag = "MOBILEPHONE_2"
        dtpOrgCardDate.Tag = "CARDDATE_2"
        txtOrgAddress.Tag = "ADDRESS_2"
        txtOrgPhone.Tag = "PHONE_2"
        txtOrgFax.Tag = "FAX_2"
        txtOrgFieldBusiness.Tag = "OCCUPATION_2"
        txtOrgDesc.Tag = "NOTE_2"
        txtOrgForeignNo.Tag = "FOREIGNNO_2"

        HideTabPage(tpOrganization)
    End Sub
    Private Sub SetTagForTpPersonal()
        ' Personal tabpage
        cboType.Tag = "TYPE_2"
        txtFullName.Tag = "FULL_NAME_2"
        cboSex.Tag = "SEX_2"
        dtpBirthDate.Tag = "BIRTH_DATE_2"
        txtBirthPlace.Tag = "BIRTH_PLACE_2"
        txtCardNo.Tag = "CARDNO_2"
        txtCardIssue.Tag = "CARDISSUE_2"
        cboOriginalNationaly.Tag = "ORIGINAL_NATIONALY_2"
        cboCurrentNationaly.Tag = "CURENT_NATIONALY_2"
        txtNation.Tag = "NATION_2"
        txtAddress.Tag = "ADDRESS_2"
        txtEducationLevel.Tag = "EDUCATION_LEVEL_2"
        txtOccupation.Tag = "OCCUPATION_2"
        txtPost.Tag = "POST_2"
        txtPhone.Tag = "PHONE_2"
        txtMobiphone.Tag = "MOBILEPHONE_2"
        txtFax.Tag = "FAX_2"
        txtEmail.Tag = "EMAIL_2"
        txtNote.Tag = "NOTE_2"
        txtForeignNo.Tag = "FOREIGNNO_2"
        'Organization tabpage

        tctRGII.SelectedIndex = 1
        cboOrgType.Tag = "TYPE"
        txtOrgFullName.Tag = "FULL_NAME"
        txtOrgTransName.Tag = "FIRST_NAME"
        txtOrgShortName.Tag = "LAST_NAME"
        txtOrgCardNo.Tag = "CARDNO"
        txtFoundationIssue.Tag = "BIRTH_PLACE"
        dtpFoundationDate.Tag = "BIRTH_DATE"
        lstOrgRepPerson.Tag = "REPNO"
        txtFoudationNo.Tag = "MOBILEPHONE"
        dtpOrgCardDate.Tag = "CARDDATE"
        txtOrgAddress.Tag = "ADDRESS"
        txtOrgPhone.Tag = "PHONE"
        txtOrgFax.Tag = "FAX"
        txtOrgFieldBusiness.Tag = "OCCUPATION"
        txtOrgDesc.Tag = "NOTE"
        txtOrgForeignNo.Tag = "FOREIGNNO"
        'cboCardType.SelectedIndex = 1

        HideTabPage(tpPersonal)
    End Sub

    Private Function GetTypeValue() As Integer
        Try
            Dim v_strKeyFieldType As String = Me.KeyFieldType
            Dim v_strKeyFieldName As String = Me.KeyFieldName
            Dim v_strKeyFieldValue As String = Me.KeyFieldValue

            Dim v_strFilter As String = ""
            Dim v_strObjMsg As String
            Dim v_strSqlInquiry As String = ""
            Dim v_xmlDocument As New Xml.XmlDocument
            Dim v_nodeList As Xml.XmlNodeList = Nothing
            Dim v_strValue As String = ""
            Dim v_strFLDNAME As String = ""
            Dim v_intType As Integer = 0
            Dim v_strCardNo As String

            Select Case KeyFieldType
                Case "C"
                    v_strFilter = KeyFieldName & " = '" & KeyFieldValue & "'"
                Case "D"
                    v_strFilter = KeyFieldName & " = TO_DATE('" & KeyFieldValue & "', '" & gc_FORMAT_DATE & "')"
                Case "N"
                    v_strFilter = KeyFieldName & " = " & KeyFieldValue.ToString()
            End Select

            v_strSqlInquiry = "SELECT IICODE, TYPE, CARDNO FROM RGII WHERE DELETED = 0 AND STATUS = 0 AND " & v_strFilter
            v_strObjMsg = BuildXMLObjMsg(, BranchId, , TellerId, gc_IsNotLocalMsg, gc_MsgTypeObj, OBJNAME_RG_RGII, gc_ActionInquiry, v_strSqlInquiry, , , , "('000')|('000')")

            'mv_BDSDelivery.Message(v_strObjMsg)
            Proxy.Message(v_strObjMsg)

            v_xmlDocument.LoadXml(v_strObjMsg)
            v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")

            For i As Integer = 0 To v_nodeList.Item(0).ChildNodes.Count - 1
                With v_nodeList.Item(0).ChildNodes(i)
                    v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                    v_strValue = .InnerText.ToString()
                    Select Case Trim(v_strFLDNAME)
                        Case "TYPE"
                            v_intType = CInt((v_strValue))
                        Case "CARDNO"
                            v_strCardNo = CStr(Trim(v_strValue))
                        Case "IICODE"
                            IiCode = CStr(Trim(v_strValue))
                    End Select
                End With
            Next
            Return v_intType
        Catch ex As Exception

        End Try
    End Function
    Private Sub GetListRepPerson(ByVal pv_strIiCode As String)

        Try
            Dim v_strSqlInquiry As String
            Dim v_strObjMsg As String
            Dim v_xmlDocument As New Xml.XmlDocument
            Dim v_nodeList As Xml.XmlNodeList = Nothing
            Dim v_strValue As String = ""
            Dim v_strFldName As String = ""
            Dim v_strIiCode, v_strRepName As String

            v_htbRepPerson = New Hashtable
            v_htbRepPerson.Clear()

            v_strSqlInquiry = "select b.IICODE, to_char(rownum) || '. ' || b.full_name || ' - ' || c.card_type_name || ' - ' || b.cardno || ' - ' || to_char(b.carddate,'dd/mm/yyyy') REPNAME " _
                            & " from rgiirep a, rgii b, " _
                            & " (SELECT CDVAL cardtype, CDCONTENT card_type_name FROM ALLCODE WHERE CDTYPE ='RG' AND CDNAME = 'CARDTYPE_IIMAST' AND DELETED = 0) c " _
                            & " where a.iicode = b.iicode And b.cardtype = c.cardtype And a.deleted = 0 And a.status = 0 and a.org_iicode = '" & pv_strIiCode & "'"

            v_strObjMsg = BuildXMLObjMsg(, BranchId, , TellerId, gc_IsNotLocalMsg, gc_MsgTypeObj, OBJNAME_RG_RGII, gc_ActionInquiry, v_strSqlInquiry, , , , "('000')|('000')")

            'Dim v_lngError As Long = mv_BDSDelivery.Message(v_strObjMsg)
            Dim v_lngError As Long = Proxy.Message(v_strObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Exit Sub
            End If

            v_xmlDocument.LoadXml(v_strObjMsg)
            v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")
            For j As Integer = 0 To v_nodeList.Count - 1
                For i As Integer = 0 To v_nodeList.Item(j).ChildNodes.Count - 1
                    With v_nodeList.Item(j).ChildNodes(i)
                        v_strFldName = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                        v_strValue = .InnerText.ToString()
                        Select Case Trim(v_strFldName)
                            Case "IICODE"
                                v_strIiCode = CStr((v_strValue))
                            Case "REPNAME"
                                v_strRepName = CStr(Trim(v_strValue))
                        End Select
                    End With
                Next
                If v_strIiCode <> "" And v_strRepName <> "" Then
                    v_htbRepPerson.Add(v_strIiCode, v_strRepName)
                    v_strIiCode = ""
                    v_strRepName = ""
                End If
            Next
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub FillLstRepPerson(ByVal pv_strIiCode As String)
        Try
            GetListRepPerson(pv_strIiCode)

            Dim v_icRepPerson As ICollection = v_htbRepPerson.Values
            Dim v_arrRepPerson As New ArrayList()

            v_arrRepPerson.AddRange(v_icRepPerson)
            lstOrgRepPerson.Items.Clear()
            If v_arrRepPerson.Count > 0 Then
                For i As Integer = 0 To v_arrRepPerson.Count - 1
                    lstOrgRepPerson.Items.Add(v_arrRepPerson(i))
                Next
            End If
            'lstOrgRepPerson.Enabled = False
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

#Region "Sự kiện form"
    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        OnSave(sender)
    End Sub

    Private Sub VerifyObjectField(ByRef pv_ctrl As Windows.Forms.Control)
        Try
            For Each v_ctrl As Control In pv_ctrl.Controls
                For i As Integer = 0 To UBound(mv_arrObjFields) - 1
                    With mv_arrObjFields(i)
                        Dim v = v_ctrl.Tag

                        If (v_ctrl.Tag = .FieldName) And (.RiskField = True) Then
                            v_ctrl.Enabled = False
                        End If
                    End With
                Next
            Next
        Catch ex As Exception
        End Try
    End Sub
#End Region
#Region "Sự kiện TabControl"
    Private Sub HideTabPage(ByVal pv_tpTabPage As TabPage)
        If tctRGII.TabPages.Contains(pv_tpTabPage) Then tctRGII.TabPages.Remove(pv_tpTabPage)
    End Sub

    Private Sub ShowTabPage(ByVal pv_tpTabPage As TabPage)
        ShowTabPage(pv_tpTabPage, tctRGII.TabPages.Count)
    End Sub

    Private Sub ShowTabPage(ByVal pv_tpTabPage As TabPage, ByVal index As Integer)
        If tctRGII.TabPages.Contains(pv_tpTabPage) Then Return
        InsertTabPage(pv_tpTabPage, index)
    End Sub

    Private Sub InsertTabPage(ByVal [pv_tpTabPage] As TabPage, ByVal [index] As Integer)
        If [index] < 0 Or [index] > tctRGII.TabCount Then
            Throw New ArgumentException("Index out of Range.")
        End If
        tctRGII.TabPages.Add([pv_tpTabPage])
        If [index] < tctRGII.TabCount - 1 Then
            Do While tctRGII.TabPages.IndexOf([pv_tpTabPage]) <> [index]
                SwapTabPages([pv_tpTabPage], (tctRGII.TabPages(tctRGII.TabPages.IndexOf([pv_tpTabPage]) - 1)))
            Loop
        End If
        tctRGII.SelectedTab = [pv_tpTabPage]
    End Sub

    Private Sub SwapTabPages(ByVal pv_tpTabPage1 As TabPage, ByVal pv_tpTabPage2 As TabPage)
        If tctRGII.TabPages.Contains(pv_tpTabPage1) = False Or tctRGII.TabPages.Contains(pv_tpTabPage2) = False Then
            Throw New ArgumentException("TabPages must be in the TabCotrols TabPageCollection.")
        End If
        Dim Index1 As Integer = tctRGII.TabPages.IndexOf(pv_tpTabPage1)
        Dim Index2 As Integer = tctRGII.TabPages.IndexOf(pv_tpTabPage2)
        tctRGII.TabPages(Index1) = pv_tpTabPage2
        tctRGII.TabPages(Index2) = pv_tpTabPage1
    End Sub
#End Region
End Class
