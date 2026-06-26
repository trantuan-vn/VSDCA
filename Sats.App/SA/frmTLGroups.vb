Imports Sats.CommonLibrary

Public Class frmTLGroups
#Region "Khai báo biến"
    Private mv_strGroupID As String
#End Region

#Region "Khai báo thuộc tính"

    Public Property GroupID() As String
        Get
            Return mv_strGroupID
        End Get
        Set(ByVal value As String)
            mv_strGroupID = value
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
            grbPermit.Enabled = False

        ElseIf (ExeFlag = ExecuteFlag.View) Then
            btnApply.Enabled = False
            btnOk.Enabled = False
            grbPermit.Enabled = True
        ElseIf (ExeFlag = ExecuteFlag.Edit) Then
            grbPermit.Enabled = False
            If cboGroupActive.SelectedValue = "N" Then
                grbPermit.Enabled = False
            Else
                grbPermit.Enabled = True
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
                Cursor = Cursors.Default
                Exit Sub
            End If

            Select Case ExeFlag
                Case ExecuteFlag.AddNew
                    v_strObjMsg = BuildXMLObjMsg(, BranchId, , TellerId, gc_IsNotLocalMsg, gc_MsgTypeObj, ObjectName, gc_ActionAdd, , , , gc_AutoIdUsed)
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
                        If v_lngErrorCode = ERR_SA_GRPID_DUPLICATED Then
                            MsgBox(ResourceManager.GetString("ERR_SA_GRPID_DUPLICATED"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                            Exit Sub
                        Else
                            If v_lngErrorCode = ERR_SA_GRPNAME_DUPLICATED Then
                                MsgBox(ResourceManager.GetString("ERR_SA_GRPNAME_DUPLICATED"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                                Exit Sub
                            Else
                                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                                Exit Sub
                            End If
                        End If
                    End If

                    MsgBox(ResourceManager.GetString("AddnewSuccessful"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)

                    If sender Is btnOk Then
                        'SearchForm.Activate()
                        MyBase.OnClose()
                    ElseIf sender Is btnApply Then
                        ExeFlag = ExecuteFlag.Edit
                        txtGroupID.Enabled = False
                        grbPermit.Enabled = True
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

                    v_strObjMsg = BuildXMLObjMsg(, BranchId, , TellerId, gc_IsNotLocalMsg, gc_MsgTypeObj, ObjectName, gc_ActionEdit, , v_strClause)
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
                        MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                        Exit Sub
                    End If

                    MsgBox(ResourceManager.GetString("EditSuccessful"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                    If sender Is btnOk Then
                        'SearchForm.Activate()
                        MyBase.OnClose()
                    ElseIf sender Is btnApply Then
                        grbPermit.Enabled = True
                    End If
            End Select

            'Me.DialogResult = DialogResult.OK
            'Update mouse pointer
            Cursor = Cursors.Default
        Catch ex As Exception
            Cursor = Cursors.Default
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                         & "Error code: System error!" & vbNewLine _
                         & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(ResourceManager.GetString("SavingFailed"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
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

#Region "Sự kiện Form"

    Private Sub btn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click, btnApply.Click
        OnSave(sender)
    End Sub

    Private Sub btnUser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Cursor = Cursors.WaitCursor
        Try
            Dim v_frm As New frmGroupUsers
            v_frm.UserLanguage = Me.UserLanguage
            v_frm.GroupId = txtGroupID.Text
            v_frm.GroupName = txtGroupName.Text
            v_frm.TellerId = Me.TellerId
            v_frm.ExeFlag = Me.ExeFlag
            v_frm.Proxy = Proxy
            v_frm.ShowDialog()
        Catch ex As Exception

        End Try
        Cursor = Cursors.Default
    End Sub

    Private Sub btnFun_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFun.Click
        Cursor = Cursors.WaitCursor

        Dim v_frm As New frmFuncAssign
        'v_frm.Owner = Me.FindForm
        v_frm.UserLanguage = Me.UserLanguage
        v_frm.GroupId = txtGroupID.Text
        v_frm.GroupName = txtGroupName.Text
        v_frm.TellerId = Me.TellerId
        v_frm.ExeFlag = Me.ExeFlag
        v_frm.AssignType = "Group"
        v_frm.Proxy = Proxy
        v_frm.OnInit()
        v_frm.ShowDialog()

        Cursor = Cursors.Default
    End Sub

    Private Sub btnTran_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTran.Click
        Cursor = Cursors.WaitCursor
        Dim v_frm As New frmTranAssign
        v_frm.UserLanguage = Me.UserLanguage
        v_frm.GroupId = txtGroupID.Text
        v_frm.GroupName = txtGroupName.Text
        v_frm.TellerId = Me.TellerId
        v_frm.ExeFlag = Me.ExeFlag
        v_frm.AssignType = "Group"
        v_frm.Proxy = Proxy
        v_frm.OnInit()
        v_frm.ShowDialog()

        Cursor = Cursors.Default
    End Sub

    Private Sub btnReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReport.Click
        Cursor = Cursors.WaitCursor
        Dim v_frm As New frmReportAssign
        v_frm.UserLanguage = Me.UserLanguage
        v_frm.GroupId = txtGroupID.Text
        v_frm.GroupName = txtGroupName.Text
        v_frm.TellerId = Me.TellerId
        v_frm.ExeFlag = Me.ExeFlag
        v_frm.AssignType = "Group"
        v_frm.Proxy = Proxy
        v_frm.OnInit()
        v_frm.ShowDialog()

        Cursor = Cursors.Default
    End Sub

    Private Sub btnTVLK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTVLK.Click
        Cursor = Cursors.WaitCursor
        Dim v_frm As New frmMIAssign
        v_frm.UserLanguage = Me.UserLanguage
        v_frm.GroupId = txtGroupID.Text
        v_frm.GroupName = txtGroupName.Text
        v_frm.TellerId = Me.TellerId
        v_frm.ExeFlag = Me.ExeFlag
        v_frm.AssignType = "Group"
        v_frm.Proxy = Proxy
        v_frm.OnInit()
        v_frm.ShowDialog()

        Cursor = Cursors.Default
    End Sub

    Private Sub btnStock_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStock.Click
        Cursor = Cursors.WaitCursor
        Dim v_frm As New frmStockAssign
        v_frm.UserLanguage = Me.UserLanguage
        v_frm.GroupId = txtGroupID.Text
        v_frm.GroupName = txtGroupName.Text
        v_frm.TellerId = Me.TellerId
        v_frm.ExeFlag = Me.ExeFlag
        v_frm.AssignType = "Group"
        v_frm.Proxy = Proxy
        v_frm.OnInit()
        v_frm.ShowDialog()

        Cursor = Cursors.Default
    End Sub

#End Region
    
    
    Private Sub frmTLGroups_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'OnInit()
        txtGroupID.Focus()
    End Sub
   
   
    Private Sub btnBrID_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrID.Click
        Cursor = Cursors.WaitCursor
        Dim v_frm As New frmBrAssign
        v_frm.UserLanguage = Me.UserLanguage
        v_frm.GroupId = txtGroupID.Text
        v_frm.GroupName = txtGroupName.Text
        v_frm.TellerId = Me.TellerId
        v_frm.ExeFlag = Me.ExeFlag
        v_frm.AssignType = "Group"
        v_frm.Proxy = Proxy
        v_frm.OnInit()
        v_frm.ShowDialog()

        Cursor = Cursors.Default
    End Sub

    Private Sub btnCA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCA.Click
        Cursor = Cursors.WaitCursor
        Dim v_frm As New frmCAAssign
        v_frm.UserLanguage = Me.UserLanguage
        v_frm.GroupId = txtGroupID.Text
        v_frm.GroupName = txtGroupName.Text
        v_frm.TellerId = Me.TellerId
        v_frm.ExeFlag = Me.ExeFlag
        v_frm.AssignType = "Group"
        v_frm.Proxy = Proxy
        v_frm.OnInit()
        v_frm.ShowDialog()

        Cursor = Cursors.Default
    End Sub
End Class