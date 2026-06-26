Imports Sats.CommonLibrary
Imports Sats.AppCore

Public Class frmTLTXUserAuth
    'Private mv_oProxy As BDSChannel.BDSDelivery

    'Public Property Proxy() As BDSChannel.BDSDelivery
    '    Get
    '        Return mv_oProxy
    '    End Get
    '    Set(ByVal value As BDSChannel.BDSDelivery)
    '        mv_oProxy = value
    '    End Set
    'End Property
#Region "Khai bao"
    Private mv_ResourceManager As Resources.ResourceManager
    Private mv_strLanguage As String
    Private mv_strTellerId As String
    Private mv_strBranchId As String
    Private mv_strAuthId As String
    Private mv_strAuthName As String
    Private mv_intExecFlag As Integer
    Private mv_strAuthType As String

    Dim v_result As DialogResult = Windows.Forms.DialogResult.Cancel
    'Private mv_BDSDelivery As New BDSChannel.BDSDelivery

#End Region
#Region "Thuoc tinh"
    Public Property AuthType() As String
        Get
            Return mv_strAuthType
        End Get
        Set(ByVal value As String)
            mv_strAuthType = value
        End Set
    End Property
    Public Property AuthId() As String
        Get
            Return mv_strAuthId
        End Get
        Set(ByVal Value As String)
            mv_strAuthId = Value
        End Set
    End Property

    Public Property AuthName() As String
        Get
            Return mv_strAuthName
        End Get
        Set(ByVal Value As String)
            mv_strAuthName = Value
        End Set
    End Property
#End Region
#Region "Method Override"
    Public Overrides Sub OnInit()
        Try
            MyBase.OnInit()
            'Load Resource Manager
            ResourceManager = New Resources.ResourceManager(gc_RootNamespace & "." & Me.Name & "_" & MyBase.UserLanguage, System.Reflection.Assembly.GetExecutingAssembly())
            LoadUserInterface(Me)

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
            txtAuthName.Enabled = False
        ElseIf (ExeFlag = ExecuteFlag.View) Then
            btnApply.Dispose()
            btnOk.Enabled = False
        ElseIf (ExeFlag = ExecuteFlag.Edit) Then
            btnApply.Dispose()
            btnOk.Enabled = True
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
                    v_strObjMsg = BuildXMLObjMsg(, BranchId, , TellerId, gc_IsNotLocalMsg, gc_MsgTypeObj, ObjectName, gc_ActionAdd, , , , gc_AutoIdUsed)
                    BuildXMLObjData(mv_dsInput, v_strObjMsg)

                    'Dim v_ws As New BDSDelivery.BDSDelivery
                    Proxy.Message(v_strObjMsg)

                    'Kiểm tra thông tin và xử lý lỗi (nếu có) từ message trả về
                    Dim v_strErrorSource As String = "", v_strErrorMessage As String = ""
                    Dim v_lngErrorCode As Long

                    GetErrorFromMessage(v_strObjMsg, v_strErrorSource, v_lngErrorCode, v_strErrorMessage)
                    If v_lngErrorCode <> 0 Then
                        'Update mouse pointer
                        Cursor = Cursors.Default
                        Select Case v_lngErrorCode
                            Case ERR_SA_TLXUSERAUTH_DUPLICATED
                                MsgBox(ResourceManager.GetString("ERR_SA_TLXUSERAUTH_DUPLICATED"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                                Exit Sub
                            Case Else
                                MsgBox(v_strErrorMessage, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, Me.Text)
                                Exit Sub
                        End Select
                    End If

                    If sender Is btnOk Then
                        'SearchForm.Activate()
                        MsgBox(ResourceManager.GetString("AddnewSuccessful"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                        MyBase.OnClose()
                    End If
                    'comment

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

                    v_strObjMsg = BuildXMLObjMsg(, BranchId, , TellerId, gc_IsNotLocalMsg, gc_MsgTypeObj, ObjectName, gc_ActionEdit, , v_strClause)
                    BuildXMLObjData(mv_dsInput, v_strObjMsg, mv_dsOldInput, ExecuteFlag.Edit)

                    'Dim v_ws As New BDSDelivery.BDSDelivery
                    Proxy.Message(v_strObjMsg)

                    'Kiểm tra thông tin và xử lý lỗi (nếu có) từ message trả về
                    Dim v_strErrorSource As String = "", v_strErrorMessage As String = ""
                    Dim v_lngErrorCode As Long

                    GetErrorFromMessage(v_strObjMsg, v_strErrorSource, v_lngErrorCode, v_strErrorMessage)
                    If v_lngErrorCode <> 0 Then
                        'Update mouse pointer
                        Cursor = Cursors.Default
                        Select Case v_lngErrorCode
                            Case Else
                                MsgBox(v_strErrorMessage, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, Me.Text)
                                Exit Sub
                        End Select
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
        Finally
            Me.Cursor = Cursors.Default
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
#Region "Cac ham rieng"
    '********************************************************************
    'Mục đích       	: Kiểm tra mã nhóm có tồn tại hay 
    'Tham số        	: 
    'Trả về         	: Int
    'Ngày tạo			: 14/03/2008
    'Người tạo		    : Nguyễn Mạnh Hà
    'Ngày cập nhật  	:
    'Người cập nhật 	:<Người cập nhật cuối cùng>
    '********************************************************************


    Public Function CheckExistAuthId(ByVal p_strAuthId As String) As Long
        Dim v_strCmdCheck As String
        Dim v_strValue As String
        Dim v_strFLDNAME As String
        Dim v_strAuthIdExist As String
        Dim v_intCount As Integer

        Try
            Select Case AuthType
                Case "U"
                    v_strCmdCheck = "SELECT COUNT(*) VALUE, MAX(TLFULLNAME) AUTHNAME FROM TLPROFILES WHERE TLID = '" & p_strAuthId & "' AND DELETED = 0"
                Case "G"
                    v_strCmdCheck = "SELECT COUNT(*) VALUE, MAX(GRPNAME) AUTHNAME FROM TLGROUPS WHERE GRPID = '" & p_strAuthId & "' AND DELETED = 0"
            End Select

            v_strAuthIdExist = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLGROUPS, gc_ActionInquiry, v_strCmdCheck)

            'Dim v_wsCheckGrp As New BDSDelivery.BDSDelivery
            Dim v_lngError As Long = Proxy.Message(v_strAuthIdExist)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox("Đã có lỗi xẩy ra, hãy liên hệ với quản tri hệ thống để được giúp đỡ", MsgBoxStyle.Critical, gc_ApplicationTitle)
                Exit Function
            End If

            Dim v_xmlDocument As New Xml.XmlDocument
            Dim v_nodeList As Xml.XmlNodeList

            v_xmlDocument.LoadXml(v_strAuthIdExist)
            v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")
            v_intCount = CInt(v_nodeList.Item(0).ChildNodes(0).InnerText.ToString)
            AuthName = v_nodeList.Item(0).ChildNodes(1).InnerText.ToString
            'Dim v_arrCheckGrpNo(v_nodeCheckGrp.Count - 1) As String
            'For i As Integer = 0 To v_nodeCheckGrp.Count - 1
            '    For j As Integer = 0 To v_nodeCheckGrp.Item(i).ChildNodes.Count - 1
            '        With v_nodeCheckGrp.Item(i).ChildNodes(j)
            '            v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
            '            v_strValue = .InnerText.ToString
            '        End With
            '        If Trim(v_strFLDNAME) = "VALUE" Then
            '            v_intValueReturn = v_strValue
            '        End If
            '    Next
            'Next
            If v_intCount = 0 Then
                Return ERR_SA_AUTHID_NOT_EXIST
            Else
                Return ERR_SA_AUTHID_EXIST
            End If
            Return ERR_SYSTEM_OK
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Sub FillDataCombobox(ByVal pv_strSql As String, ByRef pv_cboCombo As Sats.AppCore.ComboBoxEx)
        Try
            Dim v_strObjMsg As String = ""

            v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLGROUPS, gc_ActionInquiry, pv_strSql)
            Dim v_lngError As Long = Proxy.Message(v_strObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Exit Sub
            End If
            pv_cboCombo.Clears()
            FillComboEx(v_strObjMsg, pv_cboCombo)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region

    Private Sub cboModCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboModCode.SelectedIndexChanged
        Try
            If Not cboModCode.SelectedValue Is Nothing Then
                Dim v_strModCode As String = cboModCode.SelectedValue.ToString
                Dim v_strSql As String
                v_strSql = "SELECT A.TLTXCD VALUE, A.TLTXCD || ' - ' || A.TXDESC DISPLAY FROM TLTX A, APPMODULES B WHERE SUBSTR(A.TLTXCD,1,2) = B.TXCODE AND B.MODCODE = '" & v_strModCode & "' ORDER BY 1"
                FillDataCombobox(v_strSql, cboTLTX)
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub frmTLTXUserAuth_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            OnInit()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub cboAuthType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboAuthType.SelectedIndexChanged
        Try
            If Not cboAuthType.SelectedIndex.ToString Is Nothing Then
                If cboAuthType.SelectedIndex = 0 Then
                    AuthType = "G"
                Else
                    AuthType = "U"
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        OnSave(sender)
    End Sub

    Private Sub txtAuthId_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtAuthId.KeyUp
        Try
            Select Case e.KeyCode
                Case Keys.Enter
                    If v_result <> Windows.Forms.DialogResult.OK Then
                        If txtAuthId.Text <> "" Then
                            If CheckExistAuthId(txtAuthId.Text) = ERR_SA_AUTHID_NOT_EXIST Then
                                btnOk.Focus()
                                v_result = MsgBox(mv_ResourceManager.GetString("AuthIdNotExist"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                                Application.DoEvents()
                            Else
                                AuthId = txtAuthId.Text
                                txtAuthName.Text = AuthName
                            End If
                        Else
                            MsgBox(mv_ResourceManager.GetString("EmptyAuthId"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                        End If
                    End If
                Case Keys.F5
                    Dim frm As New Sats.AppCore.frmLookUp(m_BusLayer.AppLanguage)
                    Dim v_intPos As Integer
                    Select Case AuthType
                        Case "U"
                            frm.SQLCMD = "SELECT TLID VALUE, TLFULLNAME DISPLAY, TLFULLNAME EN_DISPLAY, TLFULLNAME DESCRIPTION FROM TLPROFILES WHERE STATUS = 0 AND DELETED = 0  ORDER BY 1"
                        Case "G"
                            frm.SQLCMD = "SELECT GRPID VALUE, GRPNAME DISPLAY, GRPNAME EN_DISPLAY, GRPNAME DESCRIPTION FROM TLGROUPS WHERE STATUS = 0 AND DELETED = 0 ORDER BY 1"
                    End Select
                    frm.TellerID = TellerId
                    frm.Proxy = Proxy
                    frm.ShowDialog()
                    If Not frm.RETURNDATA Is Nothing Then
                        v_intPos = InStr(frm.RETURNDATA, vbTab)
                        If v_intPos > 0 Then
                            Me.txtAuthId.Text = Mid(frm.RETURNDATA, 1, v_intPos - 1)
                            Me.txtAuthName.Text = frm.RETURNDISPLAY
                            AuthId = Me.txtAuthId.Text
                            AuthName = Me.txtAuthName.Text
                            Me.txtAuthId.Select()
                        End If
                        frm.Dispose()
                    End If
            End Select
            Application.DoEvents()
            txtAuthId.Focus()
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                           & "Error code: System error!" & vbNewLine _
                           & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Sub

    Private Sub txtAuthId_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAuthId.Leave
        If txtAuthId.Text <> "" Then
            If CheckExistAuthId(txtAuthId.Text) = ERR_SA_AUTHID_NOT_EXIST Then
                MsgBox(ResourceManager.GetString("AuthIdNotExist"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                txtAuthId.Focus()
                Application.DoEvents()
            Else
                AuthId = txtAuthId.Text
                txtAuthName.Text = AuthName
            End If
        Else
            MsgBox(mv_ResourceManager.GetString("EmptyAuthId"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
        End If
    End Sub
End Class
