Imports System
'Imports System.Data
'Imports System.Data.OleDb
Imports System.Resources
Imports System.Windows.Forms
Imports System.Reflection
Imports Sats.CommonLibrary
Imports System.IO
Imports Microsoft.Win32
Imports System.Threading
Imports Sats.SQLEngine
Imports Sats.ClientCA

Public Class frmMDIMain

    Private rm As ResourceManager
    'Private mv_BDSDelivery As BDSChannel.BDSDelivery
    Private v_blnKey As Boolean = True
    Private v_blnOk As Boolean = False
    'Public WithEvents btn As Button
    Private frmProcess As Sats.AppCore.frmProcess
    Private frmDocTreeView As DockTreeView
    Private frmMain As frmTranMain
    '    Private frmTask As TaskList
    Private frmFave As Favorits
    Private WithEvents mv_wb As System.ComponentModel.BackgroundWorker

#Region " Các sự kiện mở và đóng form chính "

    Private Sub frmMDIMain_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        SaveRegistrySettings()
        SaveRegistryFormPosition()
        Me.ntfIcon.Visible = False
        Me.ntfIcon.Dispose()
        ChangeOnlineStatus(False)
        UnLoadComponets()
    End Sub

    Private Sub frmMDIMain_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        e.Cancel = False
    End Sub

    Private Sub frmMDIMain_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Dim param As String = Microsoft.VisualBasic.Command().Trim
        'If param <> "ZIN20-AU14Z-N81SH-BKCA" Then
        '    Environment.Exit(1)
        'End If

        ' mv_BDSDelivery = New BDSChannel.BDSDelivery
        OnMDILoad()
    End Sub
#End Region

#Region " Helper methods "
    Private Sub SaveRegistrySettings()
        Try
            Dim v_regKey As RegistryKey = Registry.CurrentUser.CreateSubKey(gc_RegistryKey)

            If MyBase.WindowState <> FormWindowState.Minimized Then
                v_regKey.SetValue("WindowState", Convert.ToInt32(MyBase.WindowState))
                v_regKey.SetValue("Height", MyBase.Height)
                v_regKey.SetValue("Width", MyBase.Width)
            End If

            v_regKey.Close()
        Catch ex As Exception
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            '                              & "Error code: System error!" & vbNewLine _
            '                              & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            'MsgBox(IIf(m_BusLayer.AppLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Sub

    Private Sub LoadRegistrySettings()
        Dim v_regKey As RegistryKey = Registry.CurrentUser.OpenSubKey(gc_RegistryKey)
        If v_regKey Is Nothing Then Return

        'Get some ui settings
        If Not v_regKey.GetValue("WindowState") Is Nothing Then
            MyBase.WindowState = CType(v_regKey.GetValue("WindowState"), FormWindowState)
        End If
        If Not v_regKey.GetValue("Height") Is Nothing Then
            MyBase.Height = CType(v_regKey.GetValue("Height"), Integer)
        End If
        If Not v_regKey.GetValue("Width") Is Nothing Then
            MyBase.Width = CType(v_regKey.GetValue("Width"), Integer)
        End If

        'Close the key
        v_regKey.Close()
    End Sub

    'Hanm them ham

    Private Sub SaveRegistryFormPosition()
        Try
            Dim v_regDocTreeViewKey As RegistryKey = Registry.CurrentUser.CreateSubKey(gc_RegistryKey & "\UIPosition\DocTreeView")
            If frmDocTreeView.Visible = True Then
                v_regDocTreeViewKey.SetValue("DocTreeViewState", "True")
                v_regDocTreeViewKey.SetValue("DocTreeViewHeight", frmDocTreeView.Height)
                v_regDocTreeViewKey.SetValue("DocTreeViewWidth", frmDocTreeView.Width)
                v_regDocTreeViewKey.SetValue("DockArea", frmDocTreeView.DockAreas.ToString)
                v_regDocTreeViewKey.SetValue("DockState", frmDocTreeView.DockState.ToString)
            Else
                v_regDocTreeViewKey.SetValue("DocTreeViewState", "False")
            End If

            Dim o_frm As Windows.Forms.Form
            Dim v_regChildrenForm As RegistryKey = Registry.CurrentUser.CreateSubKey(gc_RegistryKey & "\ChildrenForm")
            v_regChildrenForm.DeleteSubKeyTree("ChildrenForm")
            Dim v_strFormName As String = ""
            For Each o_frm In Me.MdiChildren
                v_strFormName = CType(o_frm.Name, String)
                If Not v_strFormName = "" Then
                    Dim v_regSubKey As RegistryKey = v_regChildrenForm.CreateSubKey(v_strFormName)

                    v_regSubKey.SetValue("FormName", CType(o_frm.Name, String))
                    v_regSubKey.SetValue("DockArea", CType(o_frm.Dock, String))
                End If
            Next

        Catch ex As Exception
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            '                             & "Error code: System error!" & vbNewLine _
            '                             & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            'MsgBox(IIf(m_BusLayer.AppLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Sub

    Private Sub LoadRegistryFormPosition()
        Dim v_regDocTreeViewKey As RegistryKey = Registry.CurrentUser.OpenSubKey(gc_RegistryKey & "\UIPosition\DocTreeView")
        If v_regDocTreeViewKey Is Nothing Then Return
        If Not v_regDocTreeViewKey.GetValue("DocTreeViewState") Is Nothing Then
            If v_regDocTreeViewKey.GetValue("DocTreeViewState") = True Then
                frmDocTreeView.Visible = True
                If Not v_regDocTreeViewKey.GetValue("DocTreeViewHeight") Is Nothing Then
                    frmDocTreeView.Height = CType(v_regDocTreeViewKey.GetValue("DocTreeViewHeight"), Integer)
                End If
                If Not v_regDocTreeViewKey.GetValue("DocTreeViewWidth") Is Nothing Then
                    frmDocTreeView.Width = CType(v_regDocTreeViewKey.GetValue("DocTreeViewWidth"), Integer)
                End If
                If Not v_regDocTreeViewKey.GetValue("DockArea") Is Nothing Then
                    If CType(v_regDocTreeViewKey.GetValue("DockArea"), String) = "DockRight" Then
                        frmDocTreeView.DockHandler.DockAreas = WinFormsUI.Docking.DockAreas.DockRight
                        frmDocTreeView.DockHandler.DockState = WinFormsUI.Docking.DockAreas.DockRight
                    Else : frmDocTreeView.DockHandler.DockAreas = WinFormsUI.Docking.DockAreas.DockLeft
                        'frmDocTreeView.DockHandler.DockState = WinFormsUI.Docking.DockAreas.DockLeft
                    End If

                End If
            Else : frmDocTreeView.Visible = False
            End If
        End If
    End Sub

    'Hanm ket thuc

    Private Function DisplayLoginForm() As DialogResult
        Dim frm As New frmLogin()
        Dim frmResult As DialogResult = frm.ShowDialog()
        Me.Refresh()
        Return frmResult
    End Function

    Private Sub ChangeOnlineStatus(ByVal newState As Boolean)
        m_blnIsOnline = newState

        If m_blnIsOnline Then
            Me.tmrMain.Enabled = True
            Dim str As String
            str = m_BusLayer.CurrentTellerProfile.Interval

            sbrBranch.Text = m_BusLayer.CurrentTellerProfile.BranchId _
               & IIf(m_BusLayer.CurrentTellerProfile.BranchName <> String.Empty, " - " & m_BusLayer.CurrentTellerProfile.BranchName, String.Empty)
            sbrStatus.Text = m_ResourceManager.GetString("STATUS_WORKING")
            sbrUser.Text = m_BusLayer.CurrentTellerProfile.TellerName
            If m_BusLayer.CurrentTellerProfile.Description <> String.Empty Then
                sbrUser.Text &= " (" & m_BusLayer.CurrentTellerProfile.Description & ")"
            End If
            sbrTime.Text = m_BusLayer.CurrentTellerProfile.BusDate & " - " _
               & Format$(Now.Hour, "00") & ":" & Format$(Now.Minute, "00") & ":" & Format$(Now.Second, "00")

            'mnuSysLogin.Visible = False
            'mnuSysLogin.Enabled = False
            'mnuSysLogout.Visible = True
            'mnuSysLogout.Enabled = True
            'mnuSysChangePassword.Visible = True
            'mnuSysChangePassword.Enabled = True
            'mnuSysSap.Visible = True
        Else
            Me.tmrMain.Enabled = False

            sbrBranch.Text = String.Empty
            sbrStatus.Text = String.Empty
            sbrUser.Text = String.Empty
            sbrTime.Text = String.Empty


            'mnuSysLogin.Visible = True
            'mnuSysLogin.Enabled = True
            'mnuSysLogout.Visible = False
            'mnuSysLogout.Enabled = False
            'mnuSysChangePassword.Visible = False
            'mnuSysChangePassword.Enabled = False
            'mnuSysSap.Visible = False
        End If

    End Sub

    Private Sub LoadUserInterface()

        Dim obj_mnu As ToolStripMenuItem
        Dim obj_mnu1 As Object
        rm = New Resources.ResourceManager("Sats.frmMDIMain_" & m_BusLayer.AppLanguage, System.Reflection.Assembly.GetExecutingAssembly())
        For Each obj_mnu In MenuStrip.Items
            obj_mnu.Text = rm.GetString(obj_mnu.Name)
            For Each obj_mnu1 In obj_mnu.DropDownItems
                obj_mnu1.Text = rm.GetString(obj_mnu1.Name)
            Next
        Next

        Me.Text = rm.GetString("Caption")

        'mnuHelp.Text = m_ResourceManager.GetString("frmMDIMain.mnuHelp")
        'mnuHelpUserManual.Text = m_ResourceManager.GetString("frmMDIMain.mnuHelpUserManual")
        'mnuHelpAbout.Text = m_ResourceManager.GetString("frmMDIMain.mnuHelpAbout")

        'tbtTransaction.Text = m_ResourceManager.GetString("TRANSACTION_CODE")
        'tbtTransaction.VirtualTextBox.ForeColor = Color.Chocolate

        Select Case m_BusLayer.AppLanguage
            Case gc_LANG_VIETNAMESE
                mnuToolLangVN.Checked = True
                mnuToolLangEN.Checked = False
            Case gc_LANG_ENGLISH
                mnuToolLangVN.Checked = False
                mnuToolLangEN.Checked = True
        End Select

        'sbrBranch.ToolTipText = m_ResourceManager.GetString("frmMDIMain.sbrPanelBranch")
        'sbrUser.ToolTipText = m_ResourceManager.GetString("frmMDIMain.sbrPanelUser")
        'sbrPanelStatus.ToolTipText = m_ResourceManager.GetString("frmMDIMain.sbrPanelStatus")
        'sbrPanelDateTime.ToolTipText = m_ResourceManager.GetString("frmMDIMain.sbrPanelDateTime")
    End Sub


    Private Sub ComputeUpTime()
        Dim nTicks As Double
        Dim nHours As Integer
        Dim nMin As Integer
        Dim nSec As Integer

        nTicks = tickCount
        nTicks = nTicks / 1000
        'nDays = Int(nTicks / (3600 * 24))
        nTicks = nTicks - (Int(nTicks / (3600 * 24)) * (3600 * 24))
        nHours = Int(nTicks / 3600)
        nTicks = nTicks - (Int(nTicks / 3600) * 3600)
        nMin = Int(nTicks / 60)
        nTicks = nTicks - (Int(nTicks / 60) * 60)
        nSec = nTicks
        'Label6.Text = Environment.TickCount
        sbrTime.Text = m_BusLayer.CurrentTellerProfile.BusDate & " - " _
        & Format$(nHours, "00") & ":" & Format$(nMin, "00") & ":" & Format$(nSec, "00")
        tickCount += 1000
    End Sub

#End Region


#Region " Other Mothod "
    Private Sub OnMDILoad()

        'Load Splash form
        Me.Cursor = Cursors.WaitCursor
        mnuSysLogin.Visible = True
        mnuSysLogin.Enabled = True
        mnuSysLogout.Visible = False
        mnuSysLogout.Enabled = False
        mnuSysChangePassword.Visible = False
        mnuSysChangePassword.Enabled = False
        mnuSysSap.Visible = False
        mnuTool.Visible = False
        'Dim v_frmSplash As New frmSplash

        'v_frmSplash.Show()
        Application.DoEvents()

        'Khởi tạo các tham số cho hệ thống
        Try
            'Tạo một instance của lớp ứng dụng
            m_BusLayer = New CBusLayer
            m_ResourceManager = New Resources.ResourceManager("Sats.Resource_" & m_BusLayer.AppLanguage, System.Reflection.Assembly.GetExecutingAssembly())
            'Load user interface depend on system language
            LoadUserInterface()
        Catch ex As UriFormatException
            'Close splash form
            'v_frmSplash.Close()
            MsgBox(m_ResourceManager.GetString(gc_SYSERR_BAD_URL), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
            Me.Close()
            Return
        End Try

        LoadRegistrySettings()

        Favorits.HideOnClose = True
        'Favorits.Show(ctrDockPanel)

        TaskList.HideOnClose = True
        'TaskList.Show(ctrDockPanel)

        DockTreeView.HideOnClose = True
        'DockTreeView.Show(ctrDockPanel)

        'ShowMainTran()

        Application.DoEvents()
        'v_frmSplash.Close()

        Me.Cursor = Cursors.Default

        'If m_blnIsOnline Then
        '    If DisplayLoginForm() = Windows.Forms.DialogResult.Cancel Then
        '        UnLoadComponets()
        '        btnLogin.Enabled = True
        '        btnLogout.Enabled = False
        '    Else
        '        'LoadComponets()
        '        ShowProcess()
        '        btnLogin.Enabled = False
        '        btnLogout.Enabled = True
        '    End If

        'End If
        Login()
    End Sub

#End Region

    Private Sub tmrMain_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrMain.Tick
        ComputeUpTime()
    End Sub

    Private Sub ShowMainTran()
        Dim v_strObjName, v_strModCode, v_strAuthCode, v_strAuthString As String
        ' fix tam
        v_strObjName = "TLLOG"
        v_strModCode = "SY"
        v_strAuthCode = "YYYYYYY"
        v_strAuthString = "NNNNN"
        ' end fix
        'Dim frmMain As New frmTranMain(m_BusLayer.AppLanguage)
        frmMain.Name = v_strObjName
        frmMain.BusDate = m_BusLayer.CurrentTellerProfile.BusDate
        frmMain.TableName = v_strObjName
        frmMain.ModuleCode = v_strModCode
        frmMain.AuthCode = v_strAuthCode
        frmMain.AuthString = v_strAuthString
        frmMain.IsLocalSearch = gc_IsNotLocalMsg
        frmMain.SearchOnInit = False
        frmMain.BranchId = m_BusLayer.CurrentTellerProfile.BranchId
        frmMain.VsdBrid = m_BusLayer.CurrentTellerProfile.VsdBrid
        frmMain.VsdBrid2 = m_BusLayer.CurrentTellerProfile.VsdBrid2
        frmMain.TellerId = m_BusLayer.CurrentTellerProfile.TellerId
        'Added by Thanglv9 -14/12/2012
        frmMain.TellerName = m_BusLayer.CurrentTellerProfile.TellerName
        frmMain.BRCODE = m_BusLayer.CurrentTellerProfile.BranchName
        'End
        frmMain.TellerType = m_BusLayer.CurrentTellerProfile.TellerType
        frmMain.IpAddress = m_BusLayer.AppIpAddress
        frmMain.WsName = m_BusLayer.AppWsName
        frmMain.MemberFilter = m_BusLayer.CurrentTellerProfile.MemberFilter
        frmMain.StockFilter = m_BusLayer.CurrentTellerProfile.StockFilter
        frmMain.AllMember = m_BusLayer.CurrentTellerProfile.AllMember
        frmMain.AllStock = m_BusLayer.CurrentTellerProfile.AllStock
        frmMain.PassDate = m_BusLayer.CurrentTellerProfile.PassDate
        frmMain.CaptionText = "Quản lý giao dịch"
        frmMain.CloseStatus = False
        frmMain.F_Task = frmTask
        frmMain.Client = mv_oLocal
        frmMain.Proxy = pv_oProxy
        frmMain.InitDialog()
        'Return frm
    End Sub

#Region "Xử lý sự kiên Click menu"

    Private Sub SYS_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuSysLogout.Click, mnuSysLogin.Click, mnuSysExit.Click, mnuSysChangePassword.Click
        Dim v_strMenuName As String
        v_strMenuName = Mid(CType(sender, ToolStripMenuItem).Name, 7).ToUpper
        Select Case v_strMenuName
            Case "LOGIN"
                Login()
            Case "LOGOUT"
                UnLoadComponets()
                btnLogin.Enabled = True
                btnLogout.Enabled = False
            Case "CHANGEPASSWORD"
                Dim v_frm As New frmChangePassword
                v_frm.TellerId = m_BusLayer.CurrentTellerProfile.TellerId
                v_frm.UserName = m_BusLayer.CurrentTellerProfile.TellerName
                v_frm.UserLanguage = m_BusLayer.AppLanguage
                v_frm.IPAddress = mv_oLocal.IPAddress
                v_frm.ShowDialog()
            Case "EXIT"
                UnLoadComponets()
                End
        End Select
    End Sub

    Private Sub Tool_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuToolTaskList.Click, mnuToolOption.Click, mnuToolLangVN.Click, mnuToolLangEN.Click, mnuToolFavorits.Click, mnuToolDockTreeView.Click
        Dim v_strMenuName As String

        v_strMenuName = Mid(CType(sender, ToolStripMenuItem).Name, 8).ToUpper

        Select Case v_strMenuName
            Case "FAVORITS"
                If frmFave.IsHidden Then
                    frmFave.Show(ctrDockPanel)
                Else
                    frmFave.Activate()
                End If
            Case "DOCKTREEVIEW"
                If frmDocTreeView.IsHidden Then
                    frmDocTreeView.Show(ctrDockPanel)
                Else
                    frmDocTreeView.Activate()
                End If
            Case "TASKLIST"
                If frmTask.IsHidden Then
                    frmTask.Show(ctrDockPanel)
                Else
                    frmTask.Activate()
                End If
        End Select

    End Sub

    Private Sub Window_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuWinCloseAll.Click
        Dim v_obj As Form
        For Each v_obj In Me.MdiChildren
            If v_obj.Name <> "frmMain" Then
                v_obj.Close()
            End If
        Next
    End Sub

    Private Sub Help_Click(ByVal sender As Object, ByVal e As EventArgs)

    End Sub
#End Region

    Private Sub LoadComponets()
        'Chuyen con tro chuot sang hinh dong ho cat
        'MyBase.Cursor = Cursors.WaitCursor
        v_blnOk = False
        ChangeOnlineStatus(True)

        'Lay DL ve client: hanm5 sua ngay 05/03/2014: chuyen phan load giao dien sang ham login de bat su kien neu load giao dien loi thi ko cho vao he thong
        'sbrStatus.Text = m_ResourceManager.GetString("STATUS_LOADING_INTERFACE")
        'frmProcess.Caption = m_ResourceManager.GetString("STATUS_LOADING_INTERFACE")
        'frmProcess.Invoke(frmProcess.mv_Delegate)
        'Application.DoEvents()

        'mv_oLocal.isActive = True
        'mv_oLocal.Action = "Begin load interface"
        'pv_oProxy.SendAction(mv_oLocal)
        'GetDataFromBDSToClient()
        'mv_oLocal.isActive = False
        'mv_oLocal.Action = "End load interface"
        'pv_oProxy.SendAction(mv_oLocal)

        frmDocTreeView = New DockTreeView
        frmTask = New TaskList
        frmFave = New Favorits
        frmMain = New frmTranMain(m_BusLayer.AppLanguage)
        'Load tree giao dich
        sbrStatus.Text = m_ResourceManager.GetString("STATUS_LOADING_TREE")
        frmProcess.Caption = m_ResourceManager.GetString("STATUS_LOADING_TREE")
        frmProcess.Invoke(frmProcess.mv_Delegate)
        Application.DoEvents()

        frmDocTreeView.GetTreeView()
        'Load tree favorits
        frmFave.GetTreeFavView(m_BusLayer.CurrentTellerProfile.TellerId)

        sbrStatus.Text = m_ResourceManager.GetString("STATUS_LOADING_FAVORITS")
        frmProcess.Caption = m_ResourceManager.GetString("STATUS_LOADING_FAVORITS")
        frmProcess.Invoke(frmProcess.mv_Delegate)
        'Load form giao dịch chính
        sbrStatus.Text = m_ResourceManager.GetString("STATUS_LOADING_PARAMS")
        frmProcess.Caption = m_ResourceManager.GetString("STATUS_LOADING_PARAMS")
        frmProcess.Invoke(frmProcess.mv_Delegate)
        Application.DoEvents()

        'Dim frm As Sats.WinFormsUI.Docking.DockContent
        'frm = ShowMainTran()
        ShowMainTran()

        Application.DoEvents()

        sbrStatus.Text = m_ResourceManager.GetString("STATUS_WORKING")

        'hanm
        'LoadRegistryFormPosition()

        'Chuyen con tro chuot ve hinh dang binh thuong
        'MyBase.Cursor = Cursors.Default

        'Tickcount for count time
        tickCount = CDec(Hour(m_BusLayer.CurrentTellerProfile.LoginTime)) * 3600
        tickCount += CDec(Minute(m_BusLayer.CurrentTellerProfile.LoginTime)) * 60
        tickCount += CDec(Second(m_BusLayer.CurrentTellerProfile.LoginTime))
        tickCount *= 1000
        tmrMain.Start()
        v_blnOk = True
        'mv_wb = New System.ComponentModel.BackgroundWorker
        'mv_wb.RunWorkerAsync()

    End Sub

    Private Sub UnLoadComponets()
        'Chuyen con tro chuot sang hinh dong ho cat
        MyBase.Cursor = Cursors.WaitCursor

        'Cap nhat thong tin user dang nhap vao he thong
        If v_blnOk Then
            'Dim v_ws As New BDSChannel.BDSDelivery
            'Dim v_strObjMsg As String

            'v_strObjMsg = BuildXMLObjMsg(m_BusLayer.CurrentTellerProfile.BusDate, m_BusLayer.CurrentTellerProfile.BranchId, m_BusLayer.CurrentTellerProfile.LoginTime, m_BusLayer.CurrentTellerProfile.TellerId, gc_IsNotLocalMsg, gc_MsgTypeObj, OBJNAME_SY_AUTHENTICATION, gc_ActionInquiry, , , "WriteSessionOut")
            'v_ws.Message(v_strObjMsg)
        End If

        ChangeOnlineStatus(False)
        If Not frmDocTreeView Is Nothing Then
            frmDocTreeView.Close()
            frmDocTreeView = Nothing
        End If

        If Not frmTask Is Nothing Then
            frmTask.Close()
            frmTask = Nothing
        End If

        If Not frmFave Is Nothing Then
            frmFave.Close()
            frmFave = Nothing
        End If

        If Not frmMain Is Nothing Then
            frmMain.Close()
            frmMain = Nothing
        End If

        CloseAllForm()

        txtTrans.Enabled = False

        sbrStatus.Text = m_ResourceManager.GetString("STATUS_NOT_WORKING")

        tmrMain.Stop()
        v_blnOk = False
        m_blnIsOnline = True
        'm_BusLayer.mv_DataSet.Tables.Clear()
        m_BusLayer.CurrentTellerProfile.TellerId = "0000"
        If Not pv_oProxy Is Nothing Then
            pv_oProxy.Close(mv_oLocal)
        End If
        'Chuyen con tro chuot ve hinh dang binh thuong
        MyBase.Cursor = Cursors.Default

        If Not (pv_oProxy Is Nothing) Then
            pv_oProxy.mv_blnCheckUSB = False
            ClientBussinessCA.closeUSB()
        End If
    End Sub

    Private Sub CloseAllForm()
        Try
            Dim o_frm As Windows.Forms.Form
            For Each o_frm In Me.MdiChildren
                CType(o_frm, Sats.WinFormsUI.Docking.DockContent).HideOnClose = False
                CType(o_frm, Sats.WinFormsUI.Docking.DockContent).Hide()
            Next
        Catch ex As Exception

        End Try

    End Sub

    Private Function CheckExitsForm(ByVal v_strName) As Boolean
        Dim o_frm As Sats.WinFormsUI.Docking.DockContent
        Dim v_blnIsExits As Boolean = False

        For Each o_frm In Me.ctrDockPanel.Documents
            If o_frm.Name = v_strName Then
                v_blnIsExits = True
                o_frm.Activate()
                Exit For
            End If
        Next
        Return v_blnIsExits
    End Function

#Region "Load business function"
    Sub ShowTransact()
        'Dim v_thread As New Threading.Thread(AddressOf ShowFormProcess)
        Dim v_oProcess As New Sats.AppCore.ProcessForm(Me)
        Try
            v_oProcess.StartProcessForm()
            Dim v_strTLTXCD As String = "", v_strCmdInquiry As String = ""
            Dim v_strTXCODE As String = "", v_strModeCode As String = ""
            Dim v_lngErrCode As Long
            Dim frm As frmTransactionMaster
            Dim v_xmlDocument As New System.Xml.XmlDocument
            Dim v_nodeList As Xml.XmlNodeList
            Dim v_strAllowObjMsg As String
            Dim v_strObjMsg As String
            Dim v_strFLDNAME As String
            Dim v_strValue As String
            Dim v_strBrID As String

            'v_strCmdInquiry = "SELECT a.VARVALUE FROM sysvar a where a.GRNAME='SYSTEM' and a.VARNAME= 'BRSTATUS' and BRID='" & m_BusLayer.CurrentTellerProfile.BranchId & "'"
            'v_strObjMsg = BuildXMLObjMsg(, , , , gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLTX, gc_ActionInquiry, v_strCmdInquiry)
            'mv_BDSDelivery.Message(v_strObjMsg)
            'v_xmlDocument.LoadXml(v_strObjMsg)
            'If v_xmlDocument.SelectSingleNode("/ObjectMessage/ObjData/entry[@fldname='VARVALUE']").InnerText = OPERATION_INACTIVE Then
            '    MsgBox(rm.GetString("ErrorMsg"), MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, rm.GetString("Title"))
            '    Exit Sub
            'End If

            'Check transaction allowed of current teller 
            'v_strTLTXCD = txtTrans.VirtualTextBox.Text
            v_strTLTXCD = txtTrans.Text
            If Len(Trim(v_strTLTXCD)) <> 4 Then
                v_oProcess.StopProcessForm()
                Application.DoEvents()
                Exit Sub
            End If

            Me.Cursor = Cursors.WaitCursor

            Dim v_strTlType As String
            v_strTlType = m_BusLayer.CurrentTellerProfile.TellerType
            v_strBrID = m_BusLayer.CurrentTellerProfile.BranchId

            'v_strAllowObjMsg = BuildXMLObjMsg(, m_BusLayer.CurrentTellerProfile.BranchId, , m_BusLayer.CurrentTellerProfile.TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SY_AUTHENTICATION, gc_ActionInquiry, , v_strTlType & "|" & v_strTLTXCD, "CheckTransAllow", )

            'v_lngErrCode = mv_BDSDelivery.Message(v_strAllowObjMsg)
            Application.DoEvents()

            v_strCmdInquiry = "SELECT c.modcode FROM TLTX a, " _
                & " ( " _
                & " select c.tltxcd from tlauth c where c.status =0 and c.deleted =0 and c.authtype = 'U' and c.authid = '" & m_BusLayer.CurrentTellerProfile.TellerId & "' and c.tltype = '0' " _
                & " and c.brid ='" & v_strBrID & "'" _
                & " union" _
                & " select c.tltxcd from tlauth c where c.status =0 and c.deleted =0 and c.authtype = 'G' and c.brid ='" & v_strBrID & "'" _
                & " and c.authid in ( select grpid from tlgrpusers d where d.tlid = '" & m_BusLayer.CurrentTellerProfile.TellerId & "' and d.status =0 and d.deleted =0 ) and c.tltype = '0'" _
                & " ) b, appmodules c " _
                & " where a.status =0 and a.deleted =0 and a.tltxcd = b.tltxcd and substr(a.tltxcd,1,2) = c.txcode and c.deleted=0 and c.status = 0 " _
                & " and a.TLTXCD='" & v_strTLTXCD & "'"

            v_strObjMsg = BuildXMLObjMsg(, , , m_BusLayer.CurrentTellerProfile.TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SY_SEARCHFLD, gc_ActionInquiry, v_strCmdInquiry, , )

            Dim v_lngError As Long = pv_oProxy.Message(v_strObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(m_BusLayer.AppLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Exit Sub
            End If

            v_xmlDocument.LoadXml(v_strObjMsg)
            v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")
            If v_nodeList.Count = 0 Then
                'Neu khong tim thay ma giao dich
                v_oProcess.StopProcessForm()
                Application.DoEvents()
                MessageBox.Show("Bạn không có quyền truy nhập giao dịch này !", gc_ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Information)
                v_blnKey = False
                Me.Cursor = Cursors.Default
                Exit Sub
            End If

            For i As Integer = 0 To v_nodeList.Count - 1
                For j As Integer = 0 To v_nodeList.Item(i).ChildNodes.Count - 1
                    With v_nodeList.Item(i).ChildNodes(j)
                        v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                        v_strValue = .InnerText.ToString
                        Select Case Trim(v_strFLDNAME)
                            Case "MODCODE"
                                v_strModeCode = Trim(v_strValue)
                                Exit For
                        End Select
                    End With
                Next
            Next

            If Len(Trim(v_strModeCode)) > 0 Then
                If Not CheckExitsForm("frm" & v_strTLTXCD) Then
                    frm = New frmTransactionMaster
                    frm.Name = "frm" & v_strTLTXCD
                    frm.UserLanguage = m_BusLayer.AppLanguage
                    frm.ObjectName = v_strTLTXCD
                    frm.ModuleCode = v_strModeCode
                    frm.LocalObject = gc_IsNotLocalMsg
                    frm.BranchId = m_BusLayer.CurrentTellerProfile.BranchId
                    frm.TellerId = m_BusLayer.CurrentTellerProfile.TellerId
                    frm.VSDBRID = m_BusLayer.CurrentTellerProfile.VsdBrid
                    frm.IpAddress = m_BusLayer.AppIpAddress
                    frm.WsName = m_BusLayer.AppWsName
                    frm.BusDate = m_BusLayer.CurrentTellerProfile.BusDate
                    frm.MemberFilter = m_BusLayer.CurrentTellerProfile.MemberFilter
                    frm.StockFilter = m_BusLayer.CurrentTellerProfile.StockFilter
                    frm.AllMember = m_BusLayer.CurrentTellerProfile.AllMember
                    frm.AllStock = m_BusLayer.CurrentTellerProfile.AllStock
                    frm.BRCODE = m_BusLayer.CurrentTellerProfile.BranchName
                    frm.TellerName = m_BusLayer.CurrentTellerProfile.TellerName
                    frm.PassDate = m_BusLayer.CurrentTellerProfile.PassDate
                    frm.Proxy = pv_oProxy
                    frm.Client = mv_oLocal
                    frm.Show(ctrDockPanel)
                End If
            Else
                'Thông báo giao dịch phải qua tra cứu
                v_oProcess.StopProcessForm()
                Application.DoEvents()
                MsgBox(IIf(m_BusLayer.AppLanguage = "VN", "Đây không phải là mã giao dịch trực tiếp !", "This is not a direct transaction!"), MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, Me.Text)
                v_blnKey = False
                Me.Cursor = Cursors.Default
            End If
           
            Me.Cursor = Cursors.Default
            v_oProcess.StopProcessForm()
            Application.DoEvents()
        Catch ex As Exception
            Windows.Forms.Cursor.Current = Cursors.Default
            v_oProcess.StopProcessForm()
            Application.DoEvents()
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                         & "Error code: System error!" & vbNewLine _
                                         & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(m_BusLayer.AppLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
            v_blnKey = False
        Finally
            v_oProcess = Nothing
        End Try

    End Sub
#End Region

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        Xceed.SmartUI.Licenser.LicenseKey = "SUN31-241NZ-YZDPW-JKBA"

        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub txtTrans_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtTrans.KeyUp
        Select Case e.KeyCode
            Case Keys.Enter
                If v_blnKey Then
                    ShowTransact()
                Else
                    v_blnKey = True
                End If

            Case Keys.F5
                Dim frm As New Sats.AppCore.frmLookUp(m_BusLayer.AppLanguage)
                Dim v_intPos As Integer ', ctl As Control
                frm.SQLCMD = "SELECT a.TLTXCD VALUE, a.TLTXCD VALUECD,  a.TXDESC DISPLAY, a.EN_TXDESC EN_DISPLAY, c.MODNAME DESCRIPTION FROM TLTX a, " _
                            & " ( " _
                            & " select c.tltxcd from tlauth c where c.status =0 and c.deleted =0 and c.authtype = 'U' and c.authid = '" & m_BusLayer.CurrentTellerProfile.TellerId & "' and c.tltype = '0' " _
                            & " union" _
                            & " select c.tltxcd from tlauth c where c.status =0 and c.deleted =0 and c.authtype = 'G'" _
                            & " and c.authid in ( select grpid from tlgrpusers d where d.tlid = '" & m_BusLayer.CurrentTellerProfile.TellerId & "' and d.status =0 and d.deleted =0 ) and c.tltype = '0'" _
                            & " ) b, appmodules c " _
                            & " where a.status =0 and a.deleted =0 and a.tltxcd = b.tltxcd and substr(a.tltxcd,1,2) = c.txcode and c.deleted=0 and c.status = 0 "
                frm.TellerID = m_BusLayer.CurrentTellerProfile.TellerId
                frm.Proxy = pv_oProxy
                frm.ShowDialog()

                If Not frm.RETURNDATA Is Nothing Then
                    v_intPos = InStr(frm.RETURNDATA, vbTab)
                    If v_intPos > 0 Then
                        Me.txtTrans.Text = Mid(frm.RETURNDATA, 1, v_intPos - 1)
                        Me.txtTrans.Select()
                    End If
                    frm.Dispose()
                End If
        End Select
    End Sub

    Private Sub txtTrans_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTrans.TextChanged
        v_blnKey = True
    End Sub

    Private Sub btnLogout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLogout.Click
        UnLoadComponets()
        btnLogin.Enabled = True
        btnLogout.Enabled = False
        m_blnIsOnline = True
        mnuSysLogin.Visible = True
        mnuSysLogin.Enabled = True
        mnuSysLogout.Visible = False
        mnuSysLogout.Enabled = False
        mnuSysChangePassword.Visible = False
        mnuSysChangePassword.Enabled = False
        mnuSysSap.Visible = False
        mnuTool.Visible = False

        'Ngắt kết nối VPN
        Dim v_strCmd As String = ""
        v_strCmd = "/c Vpnclient disconnect"

        Dim v_procStartInfo As New System.Diagnostics.ProcessStartInfo
        v_procStartInfo.FileName = "cmd.exe"
        v_procStartInfo.Arguments = v_strCmd

        v_procStartInfo.RedirectStandardOutput = True
        v_procStartInfo.UseShellExecute = False
        v_procStartInfo.CreateNoWindow = True

        ClientBussinessCA.closeUSB()

        Process.Start(v_procStartInfo)



    End Sub

    Private Sub btnLogin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLogin.Click
        Login()
    End Sub

    Private Sub bgw_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bgw.DoWork
        If Not v_blnOk Then
            LoadComponets()
        Else
            e.Cancel = True
        End If
    End Sub

    Private Sub mv_wb_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles mv_wb.DoWork
        While blnLogin
            GetMessageFromBDS()
            Thread.Sleep(45000)
        End While
    End Sub

    Private Sub ShowProcess()
        frmProcess = New Sats.AppCore.frmProcess
        bgw.RunWorkerAsync(True)
        frmProcess.ShowDialog()
        frmProcess = Nothing
    End Sub

    Private Sub bgw_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bgw.RunWorkerCompleted
        If Not frmProcess Is Nothing Then
            frmProcess.Close()
            frmProcess = Nothing
        End If

        If v_blnOk Then
            frmDocTreeView.Show(ctrDockPanel)
            frmTask.Show(ctrDockPanel)
            frmFave.Show(ctrDockPanel)
            frmMain.Show(ctrDockPanel)
            frmTask.DockHandler.DockState = WinFormsUI.Docking.DockState.DockLeft
            frmTask.DockHandler.DockAreas = WinFormsUI.Docking.DockAreas.DockLeft
            If frmFave.stvFavmenu.Items(0).Items.Count > 0 Then
                frmFave.Activate()
            Else
                frmDocTreeView.Activate()
            End If
            txtTrans.Enabled = True
        End If
    End Sub

    Private Sub Login()
        If m_blnIsOnline Then
            If DisplayLoginForm() = Windows.Forms.DialogResult.Cancel Then
                UnLoadComponets()
                btnLogin.Enabled = True
                btnLogout.Enabled = False
            Else
                'LoadComponets()
                'Hanm5-Sua ngay 05/03/2014: Day phan load giao dien len truoc, neu co loi thi khong cho vao he thong
                mv_oLocal.isActive = True
                mv_oLocal.Action = "Begin load interface"
                pv_oProxy.SendAction(mv_oLocal)
                If Not GetDataFromBDSToClient() Then
                    Login()
                End If
                mv_oLocal.isActive = False
                mv_oLocal.Action = "End load interface"
                pv_oProxy.SendAction(mv_oLocal)
                'End Hanm5 Sua

                ShowProcess()
                btnLogin.Enabled = False
                btnLogout.Enabled = True
                Me.ntfIcon.Visible = True
                Me.ntfIcon.Text = gc_ApplicationTitle & " - " & m_BusLayer.CurrentTellerProfile.TellerName.ToUpper
                mnuSysLogin.Visible = False
                mnuSysLogin.Enabled = False
                mnuSysLogout.Visible = True
                mnuSysLogout.Enabled = True
                mnuSysChangePassword.Visible = True
                mnuSysChangePassword.Enabled = True
                mnuSysSap.Visible = True
                mnuTool.Visible = True
            End If

        End If
    End Sub

    Private Sub GetMessageFromBDS()
        Try
            Dim v_nodeList As Xml.XmlNodeList
            Dim v_strObjMsg, v_strValue, v_strFLDNAME As String
            v_strObjMsg = BuildXMLObjMsg(m_BusLayer.CurrentTellerProfile.BusDate, m_BusLayer.CurrentTellerProfile.BranchId, _
                                         m_BusLayer.CurrentTellerProfile.LoginTime, m_BusLayer.CurrentTellerProfile.TellerId, _
                                         gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SY_AUTHENTICATION, gc_ActionInquiry, , m_BusLayer.AppWsName & "|" & m_BusLayer.AppIpAddress, "GetMessages")
            Dim v_lngError As Long = pv_oProxy.Message(v_strObjMsg)

            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(m_BusLayer.AppLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Exit Sub
            End If

            'Dim v_xmlDocument As New XmlDocumentEx

            'v_xmlDocument.LoadXml(v_strObjMsg)

            'v_xmlDocument.LoadXml(v_strObjMsg)
            'v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")
            'Dim v_strMsg As String = ""

            'Dim v_strTLTXCD, v_strTXDESC, v_strTXNUM As String
            'For i = 0 To v_nodeList.Count - 1
            '    For j = 0 To v_nodeList.Item(i).ChildNodes.Count - 1
            '        With v_nodeList.Item(i).ChildNodes(j)
            '            v_strValue = .InnerText.ToString
            '            v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
            '            Select Case Trim(v_strFLDNAME)
            '                Case "TLTXCD"
            '                    v_strTLTXCD = v_strValue
            '                Case "TXDESC"
            '                    v_strTXDESC = v_strValue
            '                Case "TXNUM"
            '                    v_strTXNUM = v_strValue
            '            End Select
            '        End With
            '    Next
            '    v_strMsg &= v_strTLTXCD & " - " & v_strTXDESC & vbCrLf
            'Next

            'Me.ntfIcon.ShowBalloonTip(30, gc_ApplicationTitle, v_strMsg, ToolTipIcon.Warning)
            'mv_strMsg = v_strObjMsg

        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                         & "Error code: System error!" & vbNewLine _
                                         & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(m_BusLayer.AppLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Sub

    'Tao database
    Private Sub CreateData(ByVal v_strFilename As String)
        Try
            If Not Directory.Exists(Application.StartupPath & "\UserData") Then
                Directory.CreateDirectory(Application.StartupPath & "\UserData")
            End If
            If File.Exists(Application.StartupPath & "\UserData\" & v_strFilename & ".enc") Then
                File.Delete(Application.StartupPath & "\UserData\" & v_strFilename & ".enc")
            End If
            EncryptFile(Application.StartupPath & "\" & DATA_FILE, Application.StartupPath & "\UserData\" & v_strFilename & ".enc", m_BusLayer.CurrentTellerProfile.TellerId)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    'Lay DS tu BDS ve client
    Private Function GetDataFromBDSToClient() As Boolean
        Dim v_strObjMsg As String
        Dim v_strFileName As String
        Dim v_oProccess As Sats.AppCore.ProcessForm
        Dim v_obj As SQLEngine.SQLDataAccessLayer
        Try
            v_oProccess = New Sats.AppCore.ProcessForm(Me)
            v_oProccess.ChangeCaption(m_ResourceManager.GetString("STATUS_LOADING_INTERFACE"))
            v_oProccess.StartProcessForm()

            v_strFileName = EncryptString(m_BusLayer.CurrentTellerProfile.TellerId, DATA_FILE)
            v_strFileName = Replace(v_strFileName, "/", "")
            v_strFileName = Replace(v_strFileName, "\", "")
            v_strFileName = Replace(v_strFileName, "+", "")
            v_strFileName = Replace(v_strFileName, "-", "")
            v_strFileName = Replace(v_strFileName, "*", "")
            v_strFileName = Replace(v_strFileName, "=", "")
            v_strFileName = Mid(v_strFileName, 1, 8)

            CreateData(v_strFileName)

            Dim v_strLstTable, v_strsql As String
            v_obj = New SQLEngine.SQLDataAccessLayer(m_BusLayer.CurrentTellerProfile.TellerId)
            v_strsql = "SELECT * FROM TBLVERSION"
            Dim v_ds As DataSet = v_obj.ExecuteReturnDataSet(v_strsql)

            v_strLstTable = ""
            For i As Integer = 0 To v_ds.Tables(0).Rows.Count - 1
                v_strLstTable &= v_ds.Tables(0).Rows(i)("TBLNAME") & "|" & v_ds.Tables(0).Rows(i)("TBLVERSION") & "#"
            Next

            v_strObjMsg = BuildXMLObjMsg(m_BusLayer.CurrentTellerProfile.BusDate, m_BusLayer.CurrentTellerProfile.BranchId, _
                                         m_BusLayer.CurrentTellerProfile.LoginTime, m_BusLayer.CurrentTellerProfile.TellerId, _
                                         gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SY_AUTHENTICATION, gc_ActionInquiry, , v_strLstTable, "GetDataFromBDSToClient")
            Dim v_lngError As Long = pv_oProxy.Message(v_strObjMsg)

            If v_lngError <> ERR_SYSTEM_OK Then
                v_oProccess.StopProcessForm()
                MsgBox(IIf(m_BusLayer.AppLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_GET_INTERFACE_VN, gc_ERR_MSG_GET_INTERFACE_VN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Return False
                Exit Function
            End If

            Dim v_oXML As New XmlDocumentEx
            v_oXML.LoadXml(v_strObjMsg)
            Dim v_attrColl As Xml.XmlAttributeCollection = v_oXML.DocumentElement.Attributes
            v_strLstTable = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCLAUSE), Xml.XmlAttribute).Value)
            'Rao de lay dung version so voi vsd 
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US")
            For i As Integer = 0 To v_strLstTable.Split("#").Count - 2
                v_obj.UpdateData(v_strObjMsg, v_strLstTable.Split("#")(i).Split("|")(0))
                v_obj.UpdateVersion(v_strLstTable.Split("#")(i).Split("|")(0), v_strLstTable.Split("#")(i).Split("|")(1))
            Next

            v_obj.CloseConnection()

            DecryptFile(Application.StartupPath & "\UserData\" & v_strFileName & ".enc", Application.StartupPath & "\" & DATA_FILE, m_BusLayer.CurrentTellerProfile.TellerId)

            v_obj = New SQLDataAccessLayer(m_BusLayer.CurrentTellerProfile.TellerId)
            v_obj.UpdateData(v_strObjMsg, "RGSI")
            v_obj.UpdateData(v_strObjMsg, "RGMI")
            v_obj.UpdateData(v_strObjMsg, "CO_RGMI")
            v_obj.UpdateData(v_strObjMsg, "RGMI_CCP_MAP")
            v_obj.UpdateData(v_strObjMsg, "BRGRP")
            v_obj.UpdateData(v_strObjMsg, "TLPROFILES")
            v_obj.UpdateData(v_strObjMsg, "TLTXUSERAUTH")
            v_obj.CloseConnection()
            v_obj = Nothing
            v_oProccess.StopProcessForm()
            Return True
        Catch ex As Exception
            If Not v_obj Is Nothing Then
                v_obj.CloseConnection()
                v_obj = Nothing
            End If
            v_oProccess.StopProcessForm()
            v_oProccess = Nothing
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                         & "Error code: System error!" & vbNewLine _
                                         & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(m_BusLayer.AppLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
            'Return False
        End Try
    End Function


    Private Sub ShowMessagesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowMessagesToolStripMenuItem.Click
        'frmMessages.InitTreeView()
        frmMessages.Show()
    End Sub

    Private Sub nmuCustomizeMenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nmuCustomizeMenu.Click
        Dim frm As New frmCustomizeMenu
        frm.GetTreeView()
        frm.GetTreeFavView(m_BusLayer.CurrentTellerProfile.TellerId)
        frm.ShowDialog()
    End Sub
    Protected Overrides Sub Finalize()
        pv_oProxy = Nothing
        MyBase.Finalize()
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Try
            If Not (pv_oProxy Is Nothing) Then
                If pv_oProxy.mv_blnCheckUSB Then
                    If (pv_oProxy.mv_oSignClient Is Nothing) Then
                        UnLoadComponets()
                        btnLogin.Enabled = True
                        btnLogout.Enabled = False
                        m_blnIsOnline = True
                        mnuSysLogin.Visible = True
                        mnuSysLogin.Enabled = True
                        mnuSysLogout.Visible = False
                        mnuSysLogout.Enabled = False
                        mnuSysChangePassword.Visible = False
                        mnuSysChangePassword.Enabled = False
                        mnuSysSap.Visible = False
                        mnuTool.Visible = False

                        'Ngắt kết nối VPN
                        Dim v_strCmd As String = ""
                        v_strCmd = "/c Vpnclient disconnect"

                        Dim v_procStartInfo As New System.Diagnostics.ProcessStartInfo
                        v_procStartInfo.FileName = "cmd.exe"
                        v_procStartInfo.Arguments = v_strCmd

                        v_procStartInfo.RedirectStandardOutput = True
                        v_procStartInfo.UseShellExecute = False
                        v_procStartInfo.CreateNoWindow = True

                        ClientBussinessCA.closeUSB()

                        Process.Start(v_procStartInfo)
                    End If
                    If Not (pv_oProxy.mv_oSignClient.CheckCertificateExist) Then
                        UnLoadComponets()
                        btnLogin.Enabled = True
                        btnLogout.Enabled = False
                        m_blnIsOnline = True
                        mnuSysLogin.Visible = True
                        mnuSysLogin.Enabled = True
                        mnuSysLogout.Visible = False
                        mnuSysLogout.Enabled = False
                        mnuSysChangePassword.Visible = False
                        mnuSysChangePassword.Enabled = False
                        mnuSysSap.Visible = False
                        mnuTool.Visible = False

                        'Ngắt kết nối VPN
                        Dim v_strCmd As String = ""
                        v_strCmd = "/c Vpnclient disconnect"

                        Dim v_procStartInfo As New System.Diagnostics.ProcessStartInfo
                        v_procStartInfo.FileName = "cmd.exe"
                        v_procStartInfo.Arguments = v_strCmd

                        v_procStartInfo.RedirectStandardOutput = True
                        v_procStartInfo.UseShellExecute = False
                        v_procStartInfo.CreateNoWindow = True

                        ClientBussinessCA.closeUSB()

                        Process.Start(v_procStartInfo)
                    End If
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub
End Class
