Imports Sats.CommonLibrary

Public Class frmSynData
    Private mv_strUserLangage As String
    Private mv_strTellerID As String
    Private mv_strAuthString As String
    Private mv_strAuthCode As String

    Private mv_strList As String = "TLPROFILES|TLGROUPS|TLAUTH|CMDAUTH|$ALLCODE|$RGSI|$RGIS|$RGMI|"

    Private mv_ResourceManager As Resources.ResourceManager
    Private mv_oProxy As BDSChannel.BDSDelivery

    Public Property Proxy() As BDSChannel.BDSDelivery
        Get
            Return mv_oProxy
        End Get
        Set(ByVal value As BDSChannel.BDSDelivery)
            mv_oProxy = value
        End Set
    End Property

    Public Property UserLanguage() As String
        Get
            Return mv_strUserLangage
        End Get
        Set(ByVal value As String)
            mv_strUserLangage = value
        End Set
    End Property

    Public Property TellerID() As String
        Get
            Return mv_strTellerID
        End Get
        Set(ByVal value As String)
            mv_strTellerID = value
        End Set
    End Property

    Public Property AuthCode() As String
        Get
            Return mv_strAuthCode
        End Get
        Set(ByVal value As String)
            mv_strAuthCode = value
        End Set
    End Property

    Public Property AuthString() As String
        Get
            Return mv_strAuthString
        End Get
        Set(ByVal value As String)
            mv_strAuthString = value
        End Set
    End Property

#Region "Private Method"
    Private Sub OnInit()
        mv_ResourceManager = New Resources.ResourceManager("Sats." & Me.Name & "_" & UserLanguage, System.Reflection.Assembly.GetExecutingAssembly())
        LoadUserInterface(Me)
        Me.Text = mv_ResourceManager.GetString("frmSynData")
        LoadList()
    End Sub

    Private Sub LoadList()
        Dim v_intCount = mv_strList.Split("$").Length - 1
        clbData.Items.Clear()
        For v_int As Integer = 0 To v_intCount
            clbData.Items.Add(mv_ResourceManager.GetString("List_" & v_int + 1))
        Next
    End Sub

    Private Sub LoadUserInterface(ByRef pv_ctrl As System.Windows.Forms.Control)
        Dim v_ctrl As Windows.Forms.Control
        Dim v_str As String

        Try
            'pv_ctrl.BackColor = System.Drawing.SystemColors.InactiveCaptionText
            For Each v_ctrl In pv_ctrl.Controls
                v_str = mv_ResourceManager.GetString(v_ctrl.Name)
                If v_str <> String.Empty Then
                    If TypeOf (v_ctrl) Is Label Then
                        CType(v_ctrl, Label).Text = mv_ResourceManager.GetString(v_ctrl.Name)
                    ElseIf TypeOf (v_ctrl) Is Button Then
                        CType(v_ctrl, Button).Text = mv_ResourceManager.GetString(v_ctrl.Name)
                    ElseIf TypeOf (v_ctrl) Is CheckBox Then
                        CType(v_ctrl, CheckBox).Text = mv_ResourceManager.GetString(v_ctrl.Name)
                    End If
                ElseIf TypeOf (v_ctrl) Is Panel Then
                    LoadUserInterface(v_ctrl)
                ElseIf TypeOf (v_ctrl) Is GroupBox Then
                    CType(v_ctrl, GroupBox).Text = mv_ResourceManager.GetString(v_ctrl.Name)
                    LoadUserInterface(v_ctrl)

                End If
            Next
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.OkOnly)
        End Try
    End Sub

    Private Sub OnSave()
        Dim v_strObjMsg, v_strClause As String
        Dim v_arrList() As String

        v_strClause = ""
        Try
            Cursor = Cursors.WaitCursor

            v_arrList = mv_strList.Split("$")

            For v_int As Integer = 0 To v_arrList.Length - 1
                If clbData.GetItemChecked(v_int) Then
                    v_strClause &= v_arrList(v_int)
                End If
            Next

            v_strObjMsg = BuildXMLObjMsg(Now.Date, , Now.Date, TellerID, gc_IsLocalMsg, gc_MsgTypeObj, _
                                OBJNAME_SA_SYNDATA, gc_ActionAdhoc, , v_strClause, "SynData")

            'Dim v_ws As New BDSDelivery.BDSDelivery
            'Dim v_ws As New BDSChannel.BDSDelivery
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
            Else
                MsgBox(mv_ResourceManager.GetString("Success"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, Me.Text)
            End If

            Cursor = Cursors.Default
        Catch ex As Exception
            Cursor = Cursors.Default
            MsgBox(mv_ResourceManager.GetString("Faild") & vbCrLf & ex.Message, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, Me.Text)
        End Try
    End Sub
#End Region

#Region "Form Event"
    Private Sub frmSynData_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        OnInit()
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        OnSave()
    End Sub
    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

#End Region

End Class