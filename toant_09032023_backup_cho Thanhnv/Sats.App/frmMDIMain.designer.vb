<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMDIMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub


    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMDIMain))
        Me.StatusStrip = New System.Windows.Forms.StatusStrip
        Me.sbrBranch = New System.Windows.Forms.ToolStripStatusLabel
        Me.sbrUser = New System.Windows.Forms.ToolStripStatusLabel
        Me.sbrStatus = New System.Windows.Forms.ToolStripStatusLabel
        Me.sbrTime = New System.Windows.Forms.ToolStripStatusLabel
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.ctrDockPanel = New Sats.WinFormsUI.Docking.DockPanel
        Me.MenuStrip = New System.Windows.Forms.MenuStrip
        Me.mnuSYS = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuSysLogin = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuSysLogout = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuSysChangePassword = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuSysSap = New System.Windows.Forms.ToolStripSeparator
        Me.mnuSysExit = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuTool = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuToolLanguages = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuToolLangVN = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuToolLangEN = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuToolFavorits = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuToolDockTreeView = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuToolTaskList = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuToolOption = New System.Windows.Forms.ToolStripMenuItem
        Me.nmuCustomizeMenu = New System.Windows.Forms.ToolStripMenuItem
        Me.WindowsMenu = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuWinCloseAll = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuHelp = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuHelpContent = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuHelpIndex = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator8 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuAbout = New System.Windows.Forms.ToolStripMenuItem
        Me.tmrMain = New System.Windows.Forms.Timer(Me.components)
        Me.stbTool = New System.Windows.Forms.ToolStrip
        Me.btnLogin = New System.Windows.Forms.ToolStripLabel
        Me.btnLogout = New System.Windows.Forms.ToolStripLabel
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator
        Me.ToolStripLabel1 = New System.Windows.Forms.ToolStripLabel
        Me.txtTrans = New System.Windows.Forms.ToolStripTextBox
        Me.bgw = New System.ComponentModel.BackgroundWorker
        Me.ntfIcon = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.ctmNotify = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ShowMessagesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.StatusStrip.SuspendLayout()
        Me.MenuStrip.SuspendLayout()
        Me.stbTool.SuspendLayout()
        Me.ctmNotify.SuspendLayout()
        Me.SuspendLayout()
        '
        'StatusStrip
        '
        Me.StatusStrip.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.StatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.sbrBranch, Me.sbrUser, Me.sbrStatus, Me.sbrTime})
        Me.StatusStrip.Location = New System.Drawing.Point(0, 514)
        Me.StatusStrip.Name = "StatusStrip"
        Me.StatusStrip.Size = New System.Drawing.Size(757, 29)
        Me.StatusStrip.TabIndex = 7
        Me.StatusStrip.Text = "StatusStrip"
        '
        'sbrBranch
        '
        Me.sbrBranch.Image = CType(resources.GetObject("sbrBranch.Image"), System.Drawing.Image)
        Me.sbrBranch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.sbrBranch.Name = "sbrBranch"
        Me.sbrBranch.Size = New System.Drawing.Size(185, 24)
        Me.sbrBranch.Spring = True
        Me.sbrBranch.Text = "Branch"
        Me.sbrBranch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'sbrUser
        '
        Me.sbrUser.Image = CType(resources.GetObject("sbrUser.Image"), System.Drawing.Image)
        Me.sbrUser.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.sbrUser.Name = "sbrUser"
        Me.sbrUser.Size = New System.Drawing.Size(185, 24)
        Me.sbrUser.Spring = True
        Me.sbrUser.Text = "User"
        Me.sbrUser.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'sbrStatus
        '
        Me.sbrStatus.Image = CType(resources.GetObject("sbrStatus.Image"), System.Drawing.Image)
        Me.sbrStatus.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.sbrStatus.Name = "sbrStatus"
        Me.sbrStatus.Size = New System.Drawing.Size(185, 24)
        Me.sbrStatus.Spring = True
        Me.sbrStatus.Text = "Status"
        Me.sbrStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'sbrTime
        '
        Me.sbrTime.Image = CType(resources.GetObject("sbrTime.Image"), System.Drawing.Image)
        Me.sbrTime.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.sbrTime.Name = "sbrTime"
        Me.sbrTime.Size = New System.Drawing.Size(185, 24)
        Me.sbrTime.Spring = True
        Me.sbrTime.Text = "Time"
        Me.sbrTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ctrDockPanel
        '
        Me.ctrDockPanel.ActiveAutoHideContent = Nothing
        Me.ctrDockPanel.AutoSize = True
        Me.ctrDockPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ctrDockPanel.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World)
        Me.ctrDockPanel.Location = New System.Drawing.Point(0, 0)
        Me.ctrDockPanel.Name = "ctrDockPanel"
        Me.ctrDockPanel.Size = New System.Drawing.Size(757, 514)
        Me.ctrDockPanel.TabIndex = 9
        '
        'MenuStrip
        '
        Me.MenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuSYS, Me.mnuTool, Me.WindowsMenu, Me.mnuHelp})
        Me.MenuStrip.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip.MdiWindowListItem = Me.WindowsMenu
        Me.MenuStrip.Name = "MenuStrip"
        Me.MenuStrip.Size = New System.Drawing.Size(757, 24)
        Me.MenuStrip.TabIndex = 12
        Me.MenuStrip.Text = "MenuStrip"
        '
        'mnuSYS
        '
        Me.mnuSYS.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuSysLogin, Me.mnuSysLogout, Me.ToolStripSeparator3, Me.mnuSysChangePassword, Me.mnuSysSap, Me.mnuSysExit})
        Me.mnuSYS.ImageTransparentColor = System.Drawing.SystemColors.ActiveBorder
        Me.mnuSYS.Name = "mnuSYS"
        Me.mnuSYS.Size = New System.Drawing.Size(63, 20)
        Me.mnuSYS.Text = "Hệ thống"
        '
        'mnuSysLogin
        '
        Me.mnuSysLogin.Image = CType(resources.GetObject("mnuSysLogin.Image"), System.Drawing.Image)
        Me.mnuSysLogin.ImageTransparentColor = System.Drawing.Color.Black
        Me.mnuSysLogin.Name = "mnuSysLogin"
        Me.mnuSysLogin.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.I), System.Windows.Forms.Keys)
        Me.mnuSysLogin.Size = New System.Drawing.Size(221, 22)
        Me.mnuSysLogin.Text = "Đăng nhập hệ thống"
        '
        'mnuSysLogout
        '
        Me.mnuSysLogout.ImageTransparentColor = System.Drawing.Color.Black
        Me.mnuSysLogout.Name = "mnuSysLogout"
        Me.mnuSysLogout.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
        Me.mnuSysLogout.Size = New System.Drawing.Size(221, 22)
        Me.mnuSysLogout.Text = "Thoát khỏi hệ thống"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(218, 6)
        '
        'mnuSysChangePassword
        '
        Me.mnuSysChangePassword.ImageTransparentColor = System.Drawing.Color.Black
        Me.mnuSysChangePassword.Name = "mnuSysChangePassword"
        Me.mnuSysChangePassword.Size = New System.Drawing.Size(221, 22)
        Me.mnuSysChangePassword.Text = "Đổi mật khẩu"
        '
        'mnuSysSap
        '
        Me.mnuSysSap.Name = "mnuSysSap"
        Me.mnuSysSap.Size = New System.Drawing.Size(218, 6)
        '
        'mnuSysExit
        '
        Me.mnuSysExit.Name = "mnuSysExit"
        Me.mnuSysExit.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.F4), System.Windows.Forms.Keys)
        Me.mnuSysExit.Size = New System.Drawing.Size(221, 22)
        Me.mnuSysExit.Text = "E&xit"
        '
        'mnuTool
        '
        Me.mnuTool.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuToolLanguages, Me.ToolStripSeparator1, Me.mnuToolFavorits, Me.mnuToolDockTreeView, Me.mnuToolTaskList, Me.ToolStripSeparator2, Me.mnuToolOption, Me.nmuCustomizeMenu})
        Me.mnuTool.Name = "mnuTool"
        Me.mnuTool.Size = New System.Drawing.Size(58, 20)
        Me.mnuTool.Text = "Công cụ"
        '
        'mnuToolLanguages
        '
        Me.mnuToolLanguages.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuToolLangVN, Me.mnuToolLangEN})
        Me.mnuToolLanguages.Name = "mnuToolLanguages"
        Me.mnuToolLanguages.Size = New System.Drawing.Size(219, 22)
        Me.mnuToolLanguages.Text = "Ngôn ngữ"
        '
        'mnuToolLangVN
        '
        Me.mnuToolLangVN.Name = "mnuToolLangVN"
        Me.mnuToolLangVN.Size = New System.Drawing.Size(133, 22)
        Me.mnuToolLangVN.Text = "Tiếng Việt"
        '
        'mnuToolLangEN
        '
        Me.mnuToolLangEN.Name = "mnuToolLangEN"
        Me.mnuToolLangEN.Size = New System.Drawing.Size(133, 22)
        Me.mnuToolLangEN.Text = "Tiếng Anh"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(216, 6)
        '
        'mnuToolFavorits
        '
        Me.mnuToolFavorits.Name = "mnuToolFavorits"
        Me.mnuToolFavorits.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.D1), System.Windows.Forms.Keys)
        Me.mnuToolFavorits.Size = New System.Drawing.Size(219, 22)
        Me.mnuToolFavorits.Text = "mnuToolFavorits"
        '
        'mnuToolDockTreeView
        '
        Me.mnuToolDockTreeView.Name = "mnuToolDockTreeView"
        Me.mnuToolDockTreeView.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.D2), System.Windows.Forms.Keys)
        Me.mnuToolDockTreeView.Size = New System.Drawing.Size(219, 22)
        Me.mnuToolDockTreeView.Text = "ToolStripMenuItem2"
        '
        'mnuToolTaskList
        '
        Me.mnuToolTaskList.Name = "mnuToolTaskList"
        Me.mnuToolTaskList.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.D3), System.Windows.Forms.Keys)
        Me.mnuToolTaskList.Size = New System.Drawing.Size(219, 22)
        Me.mnuToolTaskList.Text = "ToolStripMenuItem3"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(216, 6)
        '
        'mnuToolOption
        '
        Me.mnuToolOption.Name = "mnuToolOption"
        Me.mnuToolOption.Size = New System.Drawing.Size(219, 22)
        Me.mnuToolOption.Text = "Tùy chọn"
        '
        'nmuCustomizeMenu
        '
        Me.nmuCustomizeMenu.Name = "nmuCustomizeMenu"
        Me.nmuCustomizeMenu.Size = New System.Drawing.Size(219, 22)
        Me.nmuCustomizeMenu.Text = "Điều chỉnh menu"
        '
        'WindowsMenu
        '
        Me.WindowsMenu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuWinCloseAll, Me.ToolStripSeparator6})
        Me.WindowsMenu.Name = "WindowsMenu"
        Me.WindowsMenu.Size = New System.Drawing.Size(53, 20)
        Me.WindowsMenu.Text = "Cửa sổ"
        '
        'mnuWinCloseAll
        '
        Me.mnuWinCloseAll.Name = "mnuWinCloseAll"
        Me.mnuWinCloseAll.Size = New System.Drawing.Size(142, 22)
        Me.mnuWinCloseAll.Text = "Đóng tất cả"
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(139, 6)
        '
        'mnuHelp
        '
        Me.mnuHelp.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuHelpContent, Me.mnuHelpIndex, Me.ToolStripSeparator8, Me.mnuAbout})
        Me.mnuHelp.Name = "mnuHelp"
        Me.mnuHelp.Size = New System.Drawing.Size(40, 20)
        Me.mnuHelp.Text = "&Help"
        '
        'mnuHelpContent
        '
        Me.mnuHelpContent.Name = "mnuHelpContent"
        Me.mnuHelpContent.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F1), System.Windows.Forms.Keys)
        Me.mnuHelpContent.Size = New System.Drawing.Size(173, 22)
        Me.mnuHelpContent.Text = "&Contents"
        '
        'mnuHelpIndex
        '
        Me.mnuHelpIndex.Image = CType(resources.GetObject("mnuHelpIndex.Image"), System.Drawing.Image)
        Me.mnuHelpIndex.ImageTransparentColor = System.Drawing.Color.Black
        Me.mnuHelpIndex.Name = "mnuHelpIndex"
        Me.mnuHelpIndex.Size = New System.Drawing.Size(173, 22)
        Me.mnuHelpIndex.Text = "&Index"
        '
        'ToolStripSeparator8
        '
        Me.ToolStripSeparator8.Name = "ToolStripSeparator8"
        Me.ToolStripSeparator8.Size = New System.Drawing.Size(170, 6)
        '
        'mnuAbout
        '
        Me.mnuAbout.Name = "mnuAbout"
        Me.mnuAbout.Size = New System.Drawing.Size(173, 22)
        Me.mnuAbout.Text = "&About ..."
        '
        'tmrMain
        '
        Me.tmrMain.Interval = 1000
        '
        'stbTool
        '
        Me.stbTool.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnLogin, Me.btnLogout, Me.ToolStripSeparator4, Me.ToolStripLabel1, Me.txtTrans})
        Me.stbTool.Location = New System.Drawing.Point(0, 24)
        Me.stbTool.Name = "stbTool"
        Me.stbTool.Size = New System.Drawing.Size(757, 25)
        Me.stbTool.TabIndex = 18
        Me.stbTool.Text = "ToolStrip1"
        '
        'btnLogin
        '
        Me.btnLogin.Image = Global.Sats.My.Resources.Resources.msobmain_202
        Me.btnLogin.Name = "btnLogin"
        Me.btnLogin.Size = New System.Drawing.Size(122, 22)
        Me.btnLogin.Text = "Đăng nhập hệ thống"
        '
        'btnLogout
        '
        Me.btnLogout.Image = Global.Sats.My.Resources.Resources.wmploc_1171
        Me.btnLogout.Name = "btnLogout"
        Me.btnLogout.Size = New System.Drawing.Size(119, 22)
        Me.btnLogout.Text = "Thoát khỏi hệ thống"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(6, 25)
        '
        'ToolStripLabel1
        '
        Me.ToolStripLabel1.Name = "ToolStripLabel1"
        Me.ToolStripLabel1.Size = New System.Drawing.Size(107, 22)
        Me.ToolStripLabel1.Text = "Nhập mã giao dịch:   "
        '
        'txtTrans
        '
        Me.txtTrans.Name = "txtTrans"
        Me.txtTrans.Size = New System.Drawing.Size(100, 25)
        '
        'bgw
        '
        '
        'ntfIcon
        '
        Me.ntfIcon.ContextMenuStrip = Me.ctmNotify
        Me.ntfIcon.Icon = CType(resources.GetObject("ntfIcon.Icon"), System.Drawing.Icon)
        '
        'ctmNotify
        '
        Me.ctmNotify.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ShowMessagesToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ctmNotify.Name = "ctmNotify"
        Me.ctmNotify.Size = New System.Drawing.Size(178, 48)
        '
        'ShowMessagesToolStripMenuItem
        '
        Me.ShowMessagesToolStripMenuItem.Name = "ShowMessagesToolStripMenuItem"
        Me.ShowMessagesToolStripMenuItem.Size = New System.Drawing.Size(177, 22)
        Me.ShowMessagesToolStripMenuItem.Text = "Show messages"
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(177, 22)
        Me.ExitToolStripMenuItem.Text = "&Thoát chương trình"
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.Interval = 3000
        '
        'frmMDIMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(757, 543)
        Me.Controls.Add(Me.stbTool)
        Me.Controls.Add(Me.MenuStrip)
        Me.Controls.Add(Me.ctrDockPanel)
        Me.Controls.Add(Me.StatusStrip)
        Me.IsMdiContainer = True
        Me.KeyPreview = True
        Me.Name = "frmMDIMain"
        Me.Text = "frmMDIMain"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.StatusStrip.ResumeLayout(False)
        Me.StatusStrip.PerformLayout()
        Me.MenuStrip.ResumeLayout(False)
        Me.MenuStrip.PerformLayout()
        Me.stbTool.ResumeLayout(False)
        Me.stbTool.PerformLayout()
        Me.ctmNotify.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents sbrBranch As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents StatusStrip As System.Windows.Forms.StatusStrip
    Friend WithEvents ctrDockPanel As Sats.WinFormsUI.Docking.DockPanel
    Friend WithEvents sbrUser As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents sbrTime As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents MenuStrip As System.Windows.Forms.MenuStrip
    Friend WithEvents mnuSYS As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuSysLogin As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuSysLogout As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuSysChangePassword As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuSysSap As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuSysExit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuTool As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuToolLanguages As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents WindowsMenu As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuHelp As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuHelpContent As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuHelpIndex As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator8 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuAbout As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuToolLangVN As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuToolLangEN As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuToolFavorits As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuToolDockTreeView As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuToolTaskList As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuToolOption As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuWinCloseAll As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tmrMain As System.Windows.Forms.Timer
    Friend WithEvents sbrStatus As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents stbTool As System.Windows.Forms.ToolStrip
    Friend WithEvents ToolStripLabel1 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents txtTrans As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents btnLogin As System.Windows.Forms.ToolStripLabel
    Friend WithEvents btnLogout As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents bgw As System.ComponentModel.BackgroundWorker
    Friend WithEvents ntfIcon As System.Windows.Forms.NotifyIcon
    Friend WithEvents ctmNotify As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ShowMessagesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents nmuCustomizeMenu As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Timer1 As System.Windows.Forms.Timer

End Class
