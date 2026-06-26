Imports Sats.CommonLibrary
Public Class frmBRGRP

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
            If (ExeFlag = ExecuteFlag.AddNew) Then
                txtBRID.Focus()
            Else
                If (ExeFlag = ExecuteFlag.Edit) Then
                    txtBRNAME.Focus()
                End If
            End If

        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                         & "Error code: System error!" & vbNewLine _
                         & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(ResourceManager.GetString("InitDialogFailed"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
        End Try
    End Sub
    Public Overrides Sub LoadUserInterface(ByRef pv_ctrl As System.Windows.Forms.Control)
        MyBase.LoadUserInterface(pv_ctrl)
        
        If (ExeFlag = ExecuteFlag.View) Then
            btnOk.Enabled = False
        Else
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
                Cursor = Cursors.Default
                Exit Sub
            End If

            Select Case ExeFlag
                Case ExecuteFlag.AddNew
                    v_strObjMsg = BuildXMLObjMsg(, BranchId, , TellerId, LocalObject, gc_MsgTypeObj, ObjectName, gc_ActionAdd, , , , gc_AutoIdUnused)
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
                                MsgBox(v_strErrorMessage, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, Me.Text)
                        Exit Sub
                    End If

                    MsgBox(ResourceManager.GetString("AddnewSuccessful"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)

                    If sender Is btnOk Then
                        'SearchForm.Activate()
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

                    v_strObjMsg = BuildXMLObjMsg(, BranchId, , TellerId, LocalObject, gc_MsgTypeObj, ObjectName, gc_ActionEdit, , v_strClause)
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
                        If v_lngErrorCode = ERR_SA_BRID_DUPLICATED Then
                            MsgBox(ResourceManager.GetString("ERR_SA_BRID_DUPLICATED"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, Me.Text)
                        Else
                            If v_lngErrorCode = ERR_SA_BRNAME_DUPLICATED Then
                                MsgBox(ResourceManager.GetString("ERR_SA_BRNAME_DUPLICATED"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, Me.Text)
                            Else
                                MsgBox(v_strErrorMessage, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, Me.Text)
                                Exit Sub
                            End If
                        End If
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
#End Region
End Class
