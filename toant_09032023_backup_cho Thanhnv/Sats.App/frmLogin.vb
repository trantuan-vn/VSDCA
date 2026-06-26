Imports Microsoft.Win32
Imports BkavCAPlugIn
Imports BkavCASign
Imports Sats.CommonLibrary
Imports System.IO

Public Class frmLogin
    Private m_ResourceManager As Resources.ResourceManager
    Private blnKey As Boolean = True
    Private mv_blnBrid As Boolean = True
    Private mv_oProxy As BDSChannel.BDSDelivery
    Private mv_strIPAddress As String

    Public Property Proxy() As BDSChannel.BDSDelivery
        Get
            Return mv_oProxy
        End Get
        Set(ByVal value As BDSChannel.BDSDelivery)
            mv_oProxy = value
        End Set
    End Property

    Public Property IPAddress() As String
        Get
            Return mv_strIPAddress
        End Get
        Set(ByVal value As String)
            mv_strIPAddress = value
        End Set
    End Property

    Private Sub frmLogin_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If blnKey Then
            SendKey(e)
        Else
            blnKey = True
        End If
    End Sub

    Private Sub frmLogin_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim v_strUserName As String = String.Empty
        Dim v_strPassword As String = String.Empty

        Try
            LoadResource()
            Dim v_regKey As RegistryKey = Registry.CurrentUser.OpenSubKey(gc_RegistryKey)

            If Not v_regKey Is Nothing Then
                v_strUserName = CType(v_regKey.GetValue("UserName"), String)
                If v_strUserName Is Nothing Then
                    v_strUserName = ""
                End If
                v_strPassword = CType(v_regKey.GetValue("Password"), String)
                If v_strPassword Is Nothing Then
                    v_strPassword = ""
                End If
                If v_strPassword <> "" And v_strUserName <> "" Then
                    v_strPassword = DecryptString(v_strUserName, v_strPassword)
                End If
                v_regKey.Close()
            End If
            'reg new bkav cert 2015-2020 
           
        Catch ex As Exception
            LogError.Write(ex.Message & vbCrLf & ex.StackTrace, EventLogEntryType.Error, gc_MODULE_CLIENT)
        End Try

        If v_strUserName <> String.Empty AndAlso v_strPassword <> String.Empty Then
            txtUserName.Text = v_strUserName
            txtPassword.Text = v_strPassword
            cbRemPass.Checked = True
        End If

        txtUserName.Focus()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        blnLogin = False
        Me.Close()
    End Sub

    Private Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        DoConfirm()
    End Sub

    Private Sub txtUserName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtUserName.KeyUp
        'SendKey(e)
    End Sub

    Private Sub SendKey(ByVal e As System.Windows.Forms.KeyEventArgs)
        Try
            Select Case e.KeyCode
                Case Keys.Escape
                    Me.Close()
                Case Keys.Enter
                    If mv_blnBrid And m_BusLayer.CurrentTellerProfile.TellerId.Trim = "0000" Then
                        DoConfirm()
                    Else
                        If cboBRID.Visible = True Then
                            SelectBranch()
                        End If
                    End If
            End Select
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                 & "Error code: System error!" & vbNewLine _
                                 & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(m_BusLayer.AppLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)

        End Try
    End Sub

    Private Sub txtPassword_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPassword.KeyUp
        'SendKey(e)
    End Sub

    Private Sub LoadResource()
        m_ResourceManager = New Resources.ResourceManager("Sats.Resource_" & m_BusLayer.AppLanguage, System.Reflection.Assembly.GetExecutingAssembly())
        btnOk.Text = m_ResourceManager.GetString("frmLogin_btnLogin")
        btnCancel.Text = m_ResourceManager.GetString("frmLogin_btnCancel")
        btnBRID.Text = m_ResourceManager.GetString("btnBRID")
        Me.Text = m_ResourceManager.GetString("frmLogin_Caption")
        lbUserName.Text = m_ResourceManager.GetString("frmLogin_lbUserName")
        lbPassword.Text = m_ResourceManager.GetString("frmLogin_lbPassword")
        lblBRID.Text = m_ResourceManager.GetString("lblBRID")
        lbCaption.Text = m_ResourceManager.GetString("frmLogin_lbCap")
        cbRemPass.Text = m_ResourceManager.GetString("frmLogin_cbRemPass")

    End Sub
    Private Sub btnBRID_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBRID.Click
        SelectBranch()
    End Sub


    Private Sub SelectBranch()
        m_BusLayer.CurrentTellerProfile.BranchId = cboBRID.SelectedValue.ToString.Split("|")(0)
        m_BusLayer.CurrentTellerProfile.BranchName = cboBRID.Text

        m_BusLayer.CurrentTellerProfile.BusDate = cboBRID.SelectedValue.ToString.Split("|")(1)

        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

#Region "WCF"
    Private Sub DoConfirm()
        Dim v_intVPN As Integer
        Application.DoEvents()
        frmMDIMain.sbrStatus.Text = m_ResourceManager.GetString("STATUS_LOGIN")
        Application.DoEvents()
        frmMDIMain.sbrStatus.Text = m_ResourceManager.GetString("STATUS_LOGIN")


        If File.Exists(Application.StartupPath & "\" & "regcert.bat") Then
            File.Delete(Application.StartupPath & "\" & "regcert.bat")
        End If

        Dim v_oWriter = New StreamWriter(Application.StartupPath & "\" & "regcert.bat")
        'v_oWriter.WriteLine("@ftp -i -s:""%~f0""&GOTO:EOF")
        'v_oWriter.WriteLine("Certutil -addstore -f " & """Root""" & " " & """MIC.cer""")
        'v_oWriter.WriteLine("Certutil -addstore -f " & """CA""" & " " & """BkavCA.cer""")
        v_oWriter.WriteLine("Certutil -addstore -f " & """CA""" & " " & """BkavCA2.cer""")
        v_oWriter.WriteLine("certutilwinxp.exe -addstore -f " & """CA""" & " " & """BkavCA2.cer""")
        'certutilwinxp.exe
        'v_oWriter.WriteLine("pause")
        v_oWriter.Close() 'Register new bkav key 2015-2020
        'If File.Exists(Application.StartupPath & "\" & "trustMICWindows.bat") Then
        Dim v_oProcess = New Process
        v_oProcess.StartInfo.FileName = Application.StartupPath & "\" & "regcert.bat"
        v_oProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
        v_oProcess.StartInfo.CreateNoWindow = True
        v_oProcess.Start()
        'v_oProcess.WaitForExit()
        v_oProcess.Close()
        'MsgBox(v_oProcess.StartInfo.FileName, MsgBoxStyle.Information)

        'End If
        'If File.Exists(Application.StartupPath & "\" & "MIC.cer") Then
        '    Shell("certutil.exe -addstore -f " & "Root" & "MIC.cer")
        'End If

        'If File.Exists(Application.StartupPath & "\" & "MIC.cer") Then
        '    Shell("certutil.exe -addstore -f " & """Root""" & """MIC.cer""")
        'End If

        'If File.Exists(Application.StartupPath & "\" & "MIC.cer") Then
        '    Shell("certutil.exe -addstore -f " & """Root""" & """MIC.cer""")
        'End If
        'end bangpv  

        If txtUserName.Text.Trim.Equals("") And txtPassword.Text.Trim.Equals("") Then
            blnKey = False
            MsgBox(m_ResourceManager.GetString(gc_SYSERR_RE_TYPE), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
        Else
            IPAddress = GetIPAddress()
            MyBase.Cursor = Cursors.WaitCursor
            Dim v_intCheckCA As Integer = System.Configuration.ConfigurationManager.AppSettings("CheckOCSP")
            'Dim blResult As BusLayerResult = m_BusLayer.Login(txtUserName.Text, DataProtection.ProtectData(txtPassword.Text, GetMACAddress()))
            Dim blResult As BusLayerResult = m_BusLayer.Login(txtUserName.Text, txtPassword.Text, IPAddress, v_intCheckCA)
            MyBase.Cursor = Cursors.Arrow
            mv_blnBrid = False
            'bangpv:  chay lan dau voi user CA
            If File.Exists(Application.StartupPath & "\" & "vsd.cer") Then

                Dim v_oCerClient As CertificateClient = New CertificateClient
                Try
                    Dim v_intErr = v_oCerClient.LoadCertificate
                    v_intErr = v_oCerClient.UnregisterCertificate()
                Catch ex As Exception
                End Try
                v_oCerClient.LoadCertificateFromFile(Application.StartupPath & "\" & "vsd.cer")
                'v_oCerClient.LoadCertificateFromFile("D:\\vsd.cer")
                v_oCerClient.RegisterCertificate()
            End If

            If blResult = BusLayerResult.Success Then
                'Vào hệ thống thành công, lưu lại mã truy cập và mật khẩu nếu cần
                Try
                    'Thiết lập VNP : sửa ngày 09/03/2010
                    v_intVPN = System.Configuration.ConfigurationManager.AppSettings("IsMember")
                    If v_intVPN = 1 Then
                        Dim v_strCmd As String = ""
                        v_strCmd = "/c Vpnclient connect ""VSD"" user " & Trim(txtUserName.Text) _
                                    & " pwd " & txtPassword.Text

                        Dim v_procStartInfo As New System.Diagnostics.ProcessStartInfo
                        v_procStartInfo.FileName = "cmd.exe"
                        v_procStartInfo.Arguments = v_strCmd

                        v_procStartInfo.RedirectStandardOutput = True
                        v_procStartInfo.UseShellExecute = False
                        v_procStartInfo.CreateNoWindow = True

                        Process.Start(v_procStartInfo)
                    End If
                    'End VPN

                    Dim v_regKey As RegistryKey = Registry.CurrentUser.CreateSubKey(gc_RegistryKey)

                    If cbRemPass.Checked Then
                        Dim v_strUserName As String = txtUserName.Text
                        Dim v_strPassword As String = EncryptString(v_strUserName, txtPassword.Text)
                        v_regKey.SetValue(gc_REG_USERNAME, txtUserName.Text)
                        v_regKey.SetValue(gc_REG_PASSWORD, v_strPassword)
                    Else
                        v_regKey.DeleteValue(gc_REG_USERNAME, False)
                        v_regKey.DeleteValue(gc_REG_PASSWORD, False)
                    End If
                    v_regKey.Close()

                    Dim v_strSQL As String
                    Dim v_strObjMsg As String
                    v_strSQL = "SELECT DISTINCT brid || '|' || VARVALUE VALUE, brname display FROM " _
                                & " (SELECT a.BRID, a.BRNAME, b.VARVALUE FROM (SELECT b.brid, b.brname FROM tlbridauth a, brgrp b" _
                                & " WHERE ((AUTHID = '" & m_BusLayer.CurrentTellerProfile.TellerId & "' AND authtype = 'U') OR (AUTHID IN (SELECT a.grpid" _
                                & " FROM tlgrpusers a WHERE a.tlid = '" & m_BusLayer.CurrentTellerProfile.TellerId & "') AND authtype = 'G'))" _
                                & " And a.brid = b.brid AND a.deleted = 0 AND a.status = 0" _
                                & " AND b.deleted = 0 AND b.status = 0" _
                                & " UNION" _
                                & " SELECT b.brid, brname FROM tlprofiles a, brgrp b" _
                                & " WHERE a.brid = b.brid AND a.tlid = '" & m_BusLayer.CurrentTellerProfile.TellerId & "'" _
                                & " AND a.deleted = 0 AND a.status = 0 AND b.deleted = 0 AND b.status = 0) A," _
                                & " (SELECT VARVALUE, BRID FROM SYSVAR WHERE VARNAME='CURRDATE') B" _
                                & " WHERE A.BRID = B.BRID)" _
                                & " ORDER BY 1"

                    v_strObjMsg = BuildXMLObjMsg(, , , m_BusLayer.CurrentTellerProfile.TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLPROFILES, gc_ActionInquiry, v_strSQL)

                    Dim v_lngError As Long = pv_oProxy.Message(v_strObjMsg)
                    MyBase.Cursor = Cursors.Arrow
                    If v_lngError <> ERR_SYSTEM_OK Then
                        MsgBox(IIf(m_BusLayer.AppLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                        mv_blnBrid = True
                        Exit Sub
                    End If

                    Dim v_xml As New XmlDocumentEx
                    v_xml.LoadXml(v_strObjMsg)

                    Dim v_nodeList As Xml.XmlNodeList = Nothing
                    v_nodeList = v_xml.SelectNodes("/ObjectMessage/ObjData")

                    If v_nodeList.Count > 1 Then
                        cboBRID.Visible = True
                        lblBRID.Visible = True

                        AppCore.FillComboEx(v_strObjMsg, cboBRID)

                        txtUserName.Enabled = False
                        txtPassword.Enabled = False
                        btnOk.Visible = False
                        btnBRID.Visible = True
                        cboBRID.Focus()
                        'mv_blnBrid = False
                        Exit Sub
                    End If


                Catch ex As Exception
                    MyBase.Cursor = Cursors.Arrow
                    LogError.Write("Error source: " & ex.Source & vbNewLine _
                                         & "Error code: System error!" & vbNewLine _
                                         & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
                    MsgBox(IIf(m_BusLayer.AppLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                    mv_blnBrid = True
                End Try
                mv_blnBrid = True
                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()
            Else
                txtUserName.Focus()
                mv_blnBrid = True
                MyBase.Cursor = Cursors.Arrow
                If blResult = BusLayerResult.ServiceFailure Then
                    blnKey = False
                    MsgBox(m_ResourceManager.GetString(gc_SYSERR_SVR_ERROR) & " " & m_ResourceManager.GetString(gc_SYSERR_CONTACT_NET_ADMIN), _
                        MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)

                ElseIf blResult = BusLayerResult.ConnectionFailure Then
                    blnKey = False
                    MsgBox(m_ResourceManager.GetString(gc_SYSERR_SRV_UNREACHABLE) & " " & m_ResourceManager.GetString(gc_SYSERR_CHECK_CONNECTION), _
                        MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                ElseIf blResult = BusLayerResult.AuthenticationFailure Then
                    blnKey = False
                    MsgBox(m_ResourceManager.GetString(gc_SYSERR_INCORRECT_USR_OR_PWD), _
                        MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                ElseIf blResult = BusLayerResult.UnpluggedUSB Then
                    blnKey = False
                    MsgBox(m_ResourceManager.GetString(gc_SYSERR_UNPLUGGED_USB) & " " & m_ResourceManager.GetString(gc_SYSERR_RE_TYPE), _
                        MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                ElseIf blResult = BusLayerResult.WrongCertificate Then
                    blnKey = False
                    MsgBox(m_ResourceManager.GetString(gc_SYSERR_WRONG_CERTIFICATE), _
                        MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                Else
                    blnKey = False
                    MsgBox(m_ResourceManager.GetString(gc_SYSERR_UNKNOWN_ERROR) & " " & m_ResourceManager.GetString(gc_SYSERR_CHECK_EVENT_LOG), _
                        MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                End If
            End If

        End If
    End Sub
    Private Function GetIPAddress() As String
        Try
            Dim sHostName As String = System.Net.Dns.GetHostName()
            Dim ipE As System.Net.IPHostEntry = System.Net.Dns.GetHostByName(sHostName)
            Dim IpA() As System.Net.IPAddress = ipE.AddressList
            Dim sAddr As String

            sAddr = IpA(0).ToString

            Return sAddr
        Catch ex As Exception
            Throw ex
        End Try
    End Function
#End Region
End Class
