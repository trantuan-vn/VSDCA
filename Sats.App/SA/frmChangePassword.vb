Public Class frmChangePassword
    Private mv_ResourceManager As Resources.ResourceManager
    Private mv_strLanguage As String
    Private mv_strTellerId As String
    Private mv_strUserName As String
    Private mv_strIPAddress As String

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

    Public Property UserName() As String
        Get
            Return mv_strUserName
        End Get
        Set(ByVal Value As String)
            mv_strUserName = Value
        End Set
    End Property

    Public Property IPAddress() As String
        Get
            Return mv_strIPAddress
        End Get
        Set(ByVal Value As String)
            mv_strIPAddress = Value
        End Set
    End Property

    Private Sub LoadUserInterface(ByRef pv_ctrl As Windows.Forms.Control)
        Dim v_ctrl As Windows.Forms.Control

        Try

            For Each v_ctrl In pv_ctrl.Controls
                If TypeOf (v_ctrl) Is Panel Then
                    LoadUserInterface(v_ctrl)
                ElseIf TypeOf (v_ctrl) Is GroupBox Then
                    CType(v_ctrl, GroupBox).Text = mv_ResourceManager.GetString(v_ctrl.Name)
                    LoadUserInterface(v_ctrl)
                ElseIf TypeOf (v_ctrl) Is Label Then
                    CType(v_ctrl, Label).Text = mv_ResourceManager.GetString(v_ctrl.Name)
                ElseIf TypeOf (v_ctrl) Is Button Then
                    CType(v_ctrl, Button).Text = mv_ResourceManager.GetString(v_ctrl.Name)
                ElseIf TypeOf (v_ctrl) Is CheckBox Then
                    CType(v_ctrl, CheckBox).Text = mv_ResourceManager.GetString(v_ctrl.Name)
                End If
            Next

            'Load caption của form, label caption
            Me.Text = mv_ResourceManager.GetString("frmChangePassword")
            lbCaption.Text = mv_ResourceManager.GetString("lbCaption") & UserName
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub OnInit()
        mv_ResourceManager = New Resources.ResourceManager(gc_RootNamespace & ".frmChangePassword_" & UserLanguage, System.Reflection.Assembly.GetExecutingAssembly())
        LoadUserInterface(Me)
    End Sub

    Private Sub frmChangePassword_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        OnInit()
        txtUserName.Text = UserName
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub OnSubmit()
        If m_BusLayer.Login(UserName, txtOldPassword.Text, IPAddress) = BusLayerResult.Success Then
            If txtNewPassword.Text = "" Then
                MsgBox(mv_ResourceManager.GetString("PasswordNotEmpty"), MsgBoxStyle.Information, Me.Text)
            Else
                If txtNewPassword.Text <> txtReType.Text Then
                    MsgBox(mv_ResourceManager.GetString("Password"), MsgBoxStyle.Information, Me.Text)
                Else
                    If m_BusLayer.ChangePassword(UserName, txtNewPassword.Text) = 0 Then
                        MsgBox(mv_ResourceManager.GetString("Success"), MsgBoxStyle.Information, Me.Text)
                    Else
                        MsgBox(mv_ResourceManager.GetString("SavingFailed"), MsgBoxStyle.Exclamation, Me.Text)
                    End If
                End If
            End If
        Else
            MsgBox(mv_ResourceManager.GetString("NoFindPassword"), MsgBoxStyle.Information, Me.Text)
        End If
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        OnSubmit()
    End Sub
End Class