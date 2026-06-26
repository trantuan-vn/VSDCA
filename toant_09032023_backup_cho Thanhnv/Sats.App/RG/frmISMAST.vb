Imports Sats.CommonLibrary

Public Class frmRGIS

#Region "Khai báo biến"
    Dim mv_lngISCODE As Long
    Dim ma_chrChar(0 To 15) As Char
#End Region

#Region "Khai báo thuộc tính"

    Public Property ISCODE() As Long
        Get
            Return mv_lngISCODE
        End Get
        Set(ByVal value As Long)
            mv_lngISCODE = value
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
            cbIsType.Focus()
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
            'txtCreateBy.Text = BranchId
            btnApply.Dispose()
            btnOk.Enabled = True
        ElseIf (ExeFlag = ExecuteFlag.View) Then
            btnApply.Dispose()
            btnOk.Enabled = False
        ElseIf (ExeFlag = ExecuteFlag.Edit) Then
            btnApply.Dispose()
            btnOk.Enabled = True
        End If
    End Sub

    Public Overloads Sub OnSave(ByVal sender As Object, ByVal pv_strClause As String)
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
                    v_strObjMsg = BuildXMLObjMsg(Me.BusDate, BranchId, , TellerId, gc_IsNotLocalMsg, gc_MsgTypeObj, ObjectName, gc_ActionAdd, , pv_strClause, , gc_AutoIdUsed, , , , VSDBRID, , , TellerName, IpAddress, WsName, TabText, BRCODE, Me.BusDate)
                    BuildXMLObjData(mv_dsInput, v_strObjMsg)

                    'mv_BDSDelivery.Message(v_strObjMsg)
                    Proxy.Message(v_strObjMsg)

                    'Kiểm tra thông tin và xử lý lỗi (nếu có) từ message trả về
                    Dim v_strErrorSource As String = "", v_strErrorMessage As String = ""
                    Dim v_lngErrorCode As Long

                    GetErrorFromMessage(v_strObjMsg, v_strErrorSource, v_lngErrorCode, v_strErrorMessage)
                    If v_lngErrorCode <> 0 Then
                        'Update mouse pointer
                        Cursor = Cursors.Default
                        If v_lngErrorCode = ERR_RG_IS_NAME_DUPLICATED Then
                            MsgBox(ResourceManager.GetString("ERR_RG_IS_NAME_DUPLICATED"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                            Cursor = Cursors.Default
                            Exit Sub
                        ElseIf v_lngErrorCode = ERR_RG_IS_SHORT_NAME_DUPLICATED Then
                            MsgBox(ResourceManager.GetString("ERR_RG_SHORT_NAME_DUPLICATED"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                            Cursor = Cursors.Default
                            Exit Sub
                        ElseIf v_lngErrorCode = ERR_RG_IS_CODE_DUPLICATED Then
                            MsgBox(ResourceManager.GetString("ERR_RG_CODE_DUPLICATED"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                            Cursor = Cursors.Default
                            Exit Sub
                        ElseIf v_lngErrorCode = ERR_RG_IS_BUSSINESSNO_DUPLICATED Then
                            MsgBox(Replace(ResourceManager.GetString("ERR_RG_IS_BUSSINESSNO_DUPLICATED"), "ISNAME", v_strErrorMessage), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                            Cursor = Cursors.Default
                            Exit Sub

                        ElseIf v_lngErrorCode = ERR_RG_IS_BUSSINESSNO_DUPLICATED_MI Then
                            Dim v_xml As New XmlDocumentEx
                            v_xml.LoadXml(v_strObjMsg)
                            Dim v_attrColl As Xml.XmlAttributeCollection = v_xml.DocumentElement.Attributes
                            If Not (v_attrColl.GetNamedItem(gc_AtributeFUNCNAME) Is Nothing) Then
                                v_strErrorMessage = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeFUNCNAME), Xml.XmlAttribute).Value)
                            Else
                                v_strErrorMessage = String.Empty
                            End If

                            Dim v_int = MsgBox(Replace(ResourceManager.GetString("ERR_RG_IS_BUSSINESSNO_DUPLICATED_MI"), "MINAME", v_strErrorMessage), MsgBoxStyle.Information + MsgBoxStyle.YesNo, gc_ApplicationTitle)
                            If v_int = vbYes Then
                                OnSave(btnOk, "Y")
                            Else
                                Exit Sub
                            End If

                        Else
                            MsgBox(v_strErrorMessage, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, Me.Text)
                            Cursor = Cursors.Default
                            Exit Sub
                        End If
                    Else
                        MsgBox(ResourceManager.GetString("AddnewSuccessful"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                        Cursor = Cursors.Default
                        If sender Is btnOk Then
                            'SearchForm.Activate()
                            'MsgBox("Add", "Add" + MsgBoxStyle.OkOnly, Me.Text)
                            MyBase.OnClose()
                        End If
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

                    'mv_BDSDelivery.Message(v_strObjMsg)
                    Proxy.Message(v_strObjMsg)

                    'Kiểm tra thông tin và xử lý lỗi (nếu có) từ message trả về
                    Dim v_strErrorSource As String = "", v_strErrorMessage As String = ""
                    Dim v_lngErrorCode As Long

                    GetErrorFromMessage(v_strObjMsg, v_strErrorSource, v_lngErrorCode, v_strErrorMessage)
                    If v_lngErrorCode <> 0 Then
                        'Update mouse pointer
                        Cursor = Cursors.Default
                        If v_lngErrorCode = ERR_RG_IS_NAME_DUPLICATED Then
                            MsgBox(ResourceManager.GetString("ERR_RG_IS_NAME_DUPLICATED"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                            Cursor = Cursors.Default
                            Exit Sub
                        ElseIf v_lngErrorCode = ERR_RG_IS_SHORT_NAME_DUPLICATED Then
                            MsgBox(ResourceManager.GetString("ERR_RG_SHORT_NAME_DUPLICATED"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                            Cursor = Cursors.Default
                            Exit Sub
                        ElseIf v_lngErrorCode = ERR_RG_IS_CODE_DUPLICATED Then
                            MsgBox(ResourceManager.GetString("ERR_RG_CODE_DUPLICATED"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                            Cursor = Cursors.Default
                            Exit Sub
                        Else
                            MsgBox(v_strErrorMessage, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, Me.Text)
                            Cursor = Cursors.Default
                            Exit Sub
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
            Cursor = Cursors.Default
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

#Region " Sự kiện form"
    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        OnSave(sender, "N")
    End Sub

    Private Sub txtCode_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtCode.KeyUp
        Dim v_chrChar As Char
        'v_chrChar = Convert.ToChar(e.KeyValue)
        If txtCode.Text.Length > 0 Then
            v_chrChar = txtCode.Text.Substring(txtCode.Text.Length - 1, 1)
            If Not check_IsCodeChar(v_chrChar) Then
                MsgBox(ResourceManager.GetString("KeyCodeNotComparetitive"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                txtCode.Text = txtCode.Text.Substring(0, txtCode.Text.Length - 1)
                txtCode.Focus()
            End If
        End If
    End Sub


    Private Sub txtBankAccount_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtBankAccount.KeyUp
        Dim v_chrChar As Char
        'v_chrChar = Convert.ToChar(e.KeyValue)
        If txtBankAccount.Text.Length > 0 Then
            v_chrChar = txtBankAccount.Text.Substring(txtBankAccount.Text.Length - 1, 1)
            If Not check_IsCodeChar(v_chrChar) Then
                MsgBox(ResourceManager.GetString("BankAccountNotComparetitive"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                txtBankAccount.Text = txtBankAccount.Text.Substring(0, txtBankAccount.Text.Length - 1)
                txtBankAccount.Focus()
            End If
        End If
    End Sub

    Private Sub txtCapitalRule_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        Dim v_chrChar As Char
        If txtCapitalRule.Text.Length > 1 Then
            v_chrChar = txtCapitalRule.Text.Substring(0, 1)
            If (v_chrChar = "0" Or v_chrChar = "-") Then
                MsgBox(ResourceManager.GetString("CapitalRuleNotComparetitive"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                'txtCapitalRule.Focus()
            End If
        End If
    End Sub
#End Region

#Region " Các hàm riêng"
    Public Function check_IsCodeChar(ByVal p_chrChar As Char)
        Dim v_blnCheck As Boolean = False
        Dim v_intAscci As Integer
        v_intAscci = Convert.ToInt32(p_chrChar)
        If ((v_intAscci < 58 And v_intAscci > 47) Or (v_intAscci < 91 And v_intAscci > 64) Or (v_intAscci < 123 And v_intAscci > 96) Or (v_intAscci < 32) Or (v_intAscci = 127)) Then
            v_blnCheck = True
        End If
        Return v_blnCheck
    End Function

#End Region

    Private Sub frmRGIS_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        cbIsType.Focus()
    End Sub
End Class
