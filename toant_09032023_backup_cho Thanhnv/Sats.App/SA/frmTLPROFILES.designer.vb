<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTLPROFILES
    Inherits Sats.AppCore.frmMaintenance

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTLPROFILES))
        Me.grbInfo = New System.Windows.Forms.GroupBox
        Me.cboVsd_Brid2 = New Sats.AppCore.ComboBoxEx
        Me.lbVsd_Brid2 = New System.Windows.Forms.Label
        Me.cboVsd_Brid = New Sats.AppCore.ComboBoxEx
        Me.lbVSD_BRID = New System.Windows.Forms.Label
        Me.txtGroup = New System.Windows.Forms.TextBox
        Me.cboStatus = New Sats.AppCore.ComboBoxEx
        Me.lbStatus = New System.Windows.Forms.Label
        Me.lbTLTitle = New System.Windows.Forms.Label
        Me.txtTLTITLE = New System.Windows.Forms.TextBox
        Me.txtUserName = New System.Windows.Forms.TextBox
        Me.txtDesc = New System.Windows.Forms.TextBox
        Me.txtUserFullName = New System.Windows.Forms.TextBox
        Me.txtRePin = New System.Windows.Forms.TextBox
        Me.txtPIN = New System.Windows.Forms.TextBox
        Me.lbDesc = New System.Windows.Forms.Label
        Me.lbTVLK = New System.Windows.Forms.Label
        Me.cboBRID = New Sats.AppCore.ComboBoxEx
        Me.lbDepr = New System.Windows.Forms.Label
        Me.lbRePassword = New System.Windows.Forms.Label
        Me.lbPassword = New System.Windows.Forms.Label
        Me.lbUserFullName = New System.Windows.Forms.Label
        Me.lbUserName = New System.Windows.Forms.Label
        Me.grbPermit = New System.Windows.Forms.GroupBox
        Me.btnBrID = New System.Windows.Forms.Button
        Me.btnStock = New System.Windows.Forms.Button
        Me.btnTVLK = New System.Windows.Forms.Button
        Me.btnReportAssign = New System.Windows.Forms.Button
        Me.btnTransAssign = New System.Windows.Forms.Button
        Me.btnFuncAssgin = New System.Windows.Forms.Button
        Me.txtTLID = New System.Windows.Forms.TextBox
        Me.Panel1.SuspendLayout()
        Me.grbInfo.SuspendLayout()
        Me.grbPermit.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.SystemColors.Highlight
        Me.Panel1.Size = New System.Drawing.Size(646, 42)
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(554, 240)
        Me.btnOk.TabIndex = 11
        '
        'btnApply
        '
        Me.btnApply.Location = New System.Drawing.Point(365, 436)
        Me.btnApply.Size = New System.Drawing.Size(80, 31)
        Me.btnApply.Visible = False
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(554, 271)
        Me.btnCancel.TabIndex = 12
        '
        'grbInfo
        '
        Me.grbInfo.Controls.Add(Me.cboVsd_Brid2)
        Me.grbInfo.Controls.Add(Me.lbVsd_Brid2)
        Me.grbInfo.Controls.Add(Me.cboVsd_Brid)
        Me.grbInfo.Controls.Add(Me.lbVSD_BRID)
        Me.grbInfo.Controls.Add(Me.txtGroup)
        Me.grbInfo.Controls.Add(Me.cboStatus)
        Me.grbInfo.Controls.Add(Me.lbStatus)
        Me.grbInfo.Controls.Add(Me.lbTLTitle)
        Me.grbInfo.Controls.Add(Me.txtTLTITLE)
        Me.grbInfo.Controls.Add(Me.txtUserName)
        Me.grbInfo.Controls.Add(Me.txtDesc)
        Me.grbInfo.Controls.Add(Me.txtUserFullName)
        Me.grbInfo.Controls.Add(Me.txtRePin)
        Me.grbInfo.Controls.Add(Me.txtPIN)
        Me.grbInfo.Controls.Add(Me.lbDesc)
        Me.grbInfo.Controls.Add(Me.lbTVLK)
        Me.grbInfo.Controls.Add(Me.cboBRID)
        Me.grbInfo.Controls.Add(Me.lbDepr)
        Me.grbInfo.Controls.Add(Me.lbRePassword)
        Me.grbInfo.Controls.Add(Me.lbPassword)
        Me.grbInfo.Controls.Add(Me.lbUserFullName)
        Me.grbInfo.Controls.Add(Me.lbUserName)
        Me.grbInfo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grbInfo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.grbInfo.Location = New System.Drawing.Point(4, 48)
        Me.grbInfo.Name = "grbInfo"
        Me.grbInfo.Size = New System.Drawing.Size(630, 178)
        Me.grbInfo.TabIndex = 4
        Me.grbInfo.TabStop = False
        Me.grbInfo.Tag = "grbInfo"
        Me.grbInfo.Text = "grbInfo"
        '
        'cboVsd_Brid2
        '
        Me.cboVsd_Brid2.DisplayMember = "DISPLAY"
        Me.cboVsd_Brid2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboVsd_Brid2.FormattingEnabled = True
        Me.cboVsd_Brid2.Location = New System.Drawing.Point(461, 120)
        Me.cboVsd_Brid2.Name = "cboVsd_Brid2"
        Me.cboVsd_Brid2.Size = New System.Drawing.Size(162, 21)
        Me.cboVsd_Brid2.TabIndex = 11
        Me.cboVsd_Brid2.Tag = "VSD_BRID2"
        Me.cboVsd_Brid2.ValueMember = "VALUE"
        '
        'lbVsd_Brid2
        '
        Me.lbVsd_Brid2.AutoSize = True
        Me.lbVsd_Brid2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbVsd_Brid2.Location = New System.Drawing.Point(318, 122)
        Me.lbVsd_Brid2.Name = "lbVsd_Brid2"
        Me.lbVsd_Brid2.Size = New System.Drawing.Size(63, 13)
        Me.lbVsd_Brid2.TabIndex = 23
        Me.lbVsd_Brid2.Tag = "lbVSD_BRID2"
        Me.lbVsd_Brid2.Text = "lbVsd_Brid2"
        '
        'cboVsd_Brid
        '
        Me.cboVsd_Brid.DisplayMember = "DISPLAY"
        Me.cboVsd_Brid.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboVsd_Brid.FormattingEnabled = True
        Me.cboVsd_Brid.Location = New System.Drawing.Point(462, 96)
        Me.cboVsd_Brid.Name = "cboVsd_Brid"
        Me.cboVsd_Brid.Size = New System.Drawing.Size(162, 21)
        Me.cboVsd_Brid.TabIndex = 9
        Me.cboVsd_Brid.Tag = "VSD_BRID"
        Me.cboVsd_Brid.ValueMember = "VALUE"
        '
        'lbVSD_BRID
        '
        Me.lbVSD_BRID.AutoSize = True
        Me.lbVSD_BRID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbVSD_BRID.Location = New System.Drawing.Point(319, 98)
        Me.lbVSD_BRID.Name = "lbVSD_BRID"
        Me.lbVSD_BRID.Size = New System.Drawing.Size(57, 13)
        Me.lbVSD_BRID.TabIndex = 21
        Me.lbVSD_BRID.Tag = "lbVSD_BRID"
        Me.lbVSD_BRID.Text = "lbVsd_Brid"
        '
        'txtGroup
        '
        Me.txtGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtGroup.Location = New System.Drawing.Point(462, 67)
        Me.txtGroup.Name = "txtGroup"
        Me.txtGroup.Size = New System.Drawing.Size(161, 20)
        Me.txtGroup.TabIndex = 7
        Me.txtGroup.Tag = "TLGROUP"
        '
        'cboStatus
        '
        Me.cboStatus.DisplayMember = "DISPLAY"
        Me.cboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboStatus.FormattingEnabled = True
        Me.cboStatus.Location = New System.Drawing.Point(95, 95)
        Me.cboStatus.Name = "cboStatus"
        Me.cboStatus.Size = New System.Drawing.Size(179, 21)
        Me.cboStatus.TabIndex = 8
        Me.cboStatus.Tag = "STATUS"
        Me.cboStatus.ValueMember = "VALUE"
        '
        'lbStatus
        '
        Me.lbStatus.AutoSize = True
        Me.lbStatus.Location = New System.Drawing.Point(4, 98)
        Me.lbStatus.Name = "lbStatus"
        Me.lbStatus.Size = New System.Drawing.Size(45, 13)
        Me.lbStatus.TabIndex = 20
        Me.lbStatus.Tag = "lbStatus"
        Me.lbStatus.Text = "lbStatus"
        '
        'lbTLTitle
        '
        Me.lbTLTitle.AutoSize = True
        Me.lbTLTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbTLTitle.Location = New System.Drawing.Point(4, 70)
        Me.lbTLTitle.Name = "lbTLTitle"
        Me.lbTLTitle.Size = New System.Drawing.Size(48, 13)
        Me.lbTLTitle.TabIndex = 19
        Me.lbTLTitle.Tag = "lbTLTitle"
        Me.lbTLTitle.Text = "lbTLTitle"
        '
        'txtTLTITLE
        '
        Me.txtTLTITLE.Location = New System.Drawing.Point(95, 67)
        Me.txtTLTITLE.Name = "txtTLTITLE"
        Me.txtTLTITLE.Size = New System.Drawing.Size(179, 20)
        Me.txtTLTITLE.TabIndex = 6
        Me.txtTLTITLE.Tag = "TLTITLE"
        '
        'txtUserName
        '
        Me.txtUserName.Location = New System.Drawing.Point(95, 15)
        Me.txtUserName.Name = "txtUserName"
        Me.txtUserName.Size = New System.Drawing.Size(179, 20)
        Me.txtUserName.TabIndex = 2
        Me.txtUserName.Tag = "TLNAME"
        '
        'txtDesc
        '
        Me.txtDesc.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDesc.Location = New System.Drawing.Point(95, 152)
        Me.txtDesc.Name = "txtDesc"
        Me.txtDesc.Size = New System.Drawing.Size(529, 20)
        Me.txtDesc.TabIndex = 12
        Me.txtDesc.Tag = "DESCRIPTION"
        '
        'txtUserFullName
        '
        Me.txtUserFullName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtUserFullName.Location = New System.Drawing.Point(95, 41)
        Me.txtUserFullName.Name = "txtUserFullName"
        Me.txtUserFullName.Size = New System.Drawing.Size(179, 20)
        Me.txtUserFullName.TabIndex = 3
        Me.txtUserFullName.Tag = "TLFULLNAME"
        '
        'txtRePin
        '
        Me.txtRePin.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRePin.Location = New System.Drawing.Point(462, 41)
        Me.txtRePin.Name = "txtRePin"
        Me.txtRePin.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtRePin.Size = New System.Drawing.Size(161, 20)
        Me.txtRePin.TabIndex = 5
        Me.txtRePin.Tag = "PIN2"
        '
        'txtPIN
        '
        Me.txtPIN.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPIN.Location = New System.Drawing.Point(462, 15)
        Me.txtPIN.Name = "txtPIN"
        Me.txtPIN.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPIN.Size = New System.Drawing.Size(162, 20)
        Me.txtPIN.TabIndex = 4
        Me.txtPIN.Tag = "PIN"
        '
        'lbDesc
        '
        Me.lbDesc.AutoSize = True
        Me.lbDesc.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbDesc.Location = New System.Drawing.Point(4, 155)
        Me.lbDesc.Name = "lbDesc"
        Me.lbDesc.Size = New System.Drawing.Size(40, 13)
        Me.lbDesc.TabIndex = 16
        Me.lbDesc.Tag = "lbDesc"
        Me.lbDesc.Text = "lbDesc"
        '
        'lbTVLK
        '
        Me.lbTVLK.AutoSize = True
        Me.lbTVLK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbTVLK.Location = New System.Drawing.Point(2, 128)
        Me.lbTVLK.Name = "lbTVLK"
        Me.lbTVLK.Size = New System.Drawing.Size(42, 13)
        Me.lbTVLK.TabIndex = 14
        Me.lbTVLK.Tag = "lbTVLK"
        Me.lbTVLK.Text = "lbTVLK"
        '
        'cboBRID
        '
        Me.cboBRID.DisplayMember = "DISPLAY"
        Me.cboBRID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBRID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboBRID.FormattingEnabled = True
        Me.cboBRID.Location = New System.Drawing.Point(95, 125)
        Me.cboBRID.Name = "cboBRID"
        Me.cboBRID.Size = New System.Drawing.Size(179, 21)
        Me.cboBRID.TabIndex = 10
        Me.cboBRID.Tag = "BRID"
        Me.cboBRID.ValueMember = "VALUE"
        '
        'lbDepr
        '
        Me.lbDepr.AutoSize = True
        Me.lbDepr.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbDepr.Location = New System.Drawing.Point(319, 70)
        Me.lbDepr.Name = "lbDepr"
        Me.lbDepr.Size = New System.Drawing.Size(38, 13)
        Me.lbDepr.TabIndex = 12
        Me.lbDepr.Tag = "lbDepr"
        Me.lbDepr.Text = "lbDepr"
        '
        'lbRePassword
        '
        Me.lbRePassword.AutoSize = True
        Me.lbRePassword.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbRePassword.Location = New System.Drawing.Point(319, 44)
        Me.lbRePassword.Name = "lbRePassword"
        Me.lbRePassword.Size = New System.Drawing.Size(75, 13)
        Me.lbRePassword.TabIndex = 9
        Me.lbRePassword.Tag = "lbRePassword"
        Me.lbRePassword.Text = "lbRePassword"
        '
        'lbPassword
        '
        Me.lbPassword.AutoSize = True
        Me.lbPassword.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbPassword.Location = New System.Drawing.Point(319, 18)
        Me.lbPassword.Name = "lbPassword"
        Me.lbPassword.Size = New System.Drawing.Size(61, 13)
        Me.lbPassword.TabIndex = 5
        Me.lbPassword.Tag = "lbPassword"
        Me.lbPassword.Text = "lbPassword"
        '
        'lbUserFullName
        '
        Me.lbUserFullName.AutoSize = True
        Me.lbUserFullName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbUserFullName.Location = New System.Drawing.Point(4, 48)
        Me.lbUserFullName.Name = "lbUserFullName"
        Me.lbUserFullName.Size = New System.Drawing.Size(81, 13)
        Me.lbUserFullName.TabIndex = 7
        Me.lbUserFullName.Tag = "lbUserFullName"
        Me.lbUserFullName.Text = "lbUserFullName"
        '
        'lbUserName
        '
        Me.lbUserName.AutoSize = True
        Me.lbUserName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbUserName.Location = New System.Drawing.Point(4, 22)
        Me.lbUserName.Name = "lbUserName"
        Me.lbUserName.Size = New System.Drawing.Size(65, 13)
        Me.lbUserName.TabIndex = 2
        Me.lbUserName.Tag = "lbUserName"
        Me.lbUserName.Text = "lbUserName"
        '
        'grbPermit
        '
        Me.grbPermit.Controls.Add(Me.btnBrID)
        Me.grbPermit.Controls.Add(Me.btnStock)
        Me.grbPermit.Controls.Add(Me.btnTVLK)
        Me.grbPermit.Controls.Add(Me.btnReportAssign)
        Me.grbPermit.Controls.Add(Me.btnTransAssign)
        Me.grbPermit.Controls.Add(Me.btnFuncAssgin)
        Me.grbPermit.Location = New System.Drawing.Point(7, 234)
        Me.grbPermit.Name = "grbPermit"
        Me.grbPermit.Size = New System.Drawing.Size(541, 63)
        Me.grbPermit.TabIndex = 5
        Me.grbPermit.TabStop = False
        Me.grbPermit.Tag = "grbPermit"
        Me.grbPermit.Text = "grbPermit"
        '
        'btnBrID
        '
        Me.btnBrID.Location = New System.Drawing.Point(442, 27)
        Me.btnBrID.Name = "btnBrID"
        Me.btnBrID.Size = New System.Drawing.Size(93, 25)
        Me.btnBrID.TabIndex = 18
        Me.btnBrID.Tag = "btnBrID"
        Me.btnBrID.Text = "btnBrID"
        Me.btnBrID.UseVisualStyleBackColor = True
        '
        'btnStock
        '
        Me.btnStock.Location = New System.Drawing.Point(329, 27)
        Me.btnStock.Name = "btnStock"
        Me.btnStock.Size = New System.Drawing.Size(112, 25)
        Me.btnStock.TabIndex = 17
        Me.btnStock.Tag = "btnStock"
        Me.btnStock.Text = "btnStock"
        Me.btnStock.UseVisualStyleBackColor = True
        '
        'btnTVLK
        '
        Me.btnTVLK.Location = New System.Drawing.Point(249, 27)
        Me.btnTVLK.Name = "btnTVLK"
        Me.btnTVLK.Size = New System.Drawing.Size(79, 25)
        Me.btnTVLK.TabIndex = 16
        Me.btnTVLK.Tag = "btnTVLK"
        Me.btnTVLK.Text = "btnTVLK"
        Me.btnTVLK.UseVisualStyleBackColor = True
        '
        'btnReportAssign
        '
        Me.btnReportAssign.Location = New System.Drawing.Point(169, 27)
        Me.btnReportAssign.Name = "btnReportAssign"
        Me.btnReportAssign.Size = New System.Drawing.Size(79, 25)
        Me.btnReportAssign.TabIndex = 15
        Me.btnReportAssign.Tag = "btnReportAssign"
        Me.btnReportAssign.Text = "btnReportAssign"
        Me.btnReportAssign.UseVisualStyleBackColor = True
        '
        'btnTransAssign
        '
        Me.btnTransAssign.Location = New System.Drawing.Point(88, 27)
        Me.btnTransAssign.Name = "btnTransAssign"
        Me.btnTransAssign.Size = New System.Drawing.Size(79, 25)
        Me.btnTransAssign.TabIndex = 14
        Me.btnTransAssign.Tag = "btnTransAssign"
        Me.btnTransAssign.Text = "btnTransAssign"
        Me.btnTransAssign.UseVisualStyleBackColor = True
        '
        'btnFuncAssgin
        '
        Me.btnFuncAssgin.Location = New System.Drawing.Point(7, 27)
        Me.btnFuncAssgin.Name = "btnFuncAssgin"
        Me.btnFuncAssgin.Size = New System.Drawing.Size(79, 25)
        Me.btnFuncAssgin.TabIndex = 13
        Me.btnFuncAssgin.Tag = "btnFuncAssgin"
        Me.btnFuncAssgin.Text = "btnFuncAssgin"
        Me.btnFuncAssgin.UseVisualStyleBackColor = True
        '
        'txtTLID
        '
        Me.txtTLID.Location = New System.Drawing.Point(278, 400)
        Me.txtTLID.Name = "txtTLID"
        Me.txtTLID.Size = New System.Drawing.Size(100, 20)
        Me.txtTLID.TabIndex = 6
        Me.txtTLID.Tag = "TLID"
        '
        'frmTLPROFILES
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.ClientSize = New System.Drawing.Size(646, 308)
        Me.Controls.Add(Me.txtTLID)
        Me.Controls.Add(Me.grbInfo)
        Me.Controls.Add(Me.grbPermit)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "frmTLPROFILES"
        Me.Controls.SetChildIndex(Me.btnCancel, 0)
        Me.Controls.SetChildIndex(Me.btnOk, 0)
        Me.Controls.SetChildIndex(Me.grbPermit, 0)
        Me.Controls.SetChildIndex(Me.btnApply, 0)
        Me.Controls.SetChildIndex(Me.grbInfo, 0)
        Me.Controls.SetChildIndex(Me.txtTLID, 0)
        Me.Controls.SetChildIndex(Me.Panel1, 0)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.grbInfo.ResumeLayout(False)
        Me.grbInfo.PerformLayout()
        Me.grbPermit.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtDesc As System.Windows.Forms.TextBox
    Friend WithEvents txtUserFullName As System.Windows.Forms.TextBox
    Friend WithEvents txtRePin As System.Windows.Forms.TextBox
    Friend WithEvents txtPIN As System.Windows.Forms.TextBox
    Friend WithEvents lbDesc As System.Windows.Forms.Label
    Friend WithEvents lbTVLK As System.Windows.Forms.Label
    Friend WithEvents cboBRID As Sats.AppCore.ComboBoxEx
    Friend WithEvents lbDepr As System.Windows.Forms.Label
    Friend WithEvents lbRePassword As System.Windows.Forms.Label
    Friend WithEvents lbPassword As System.Windows.Forms.Label
    Friend WithEvents lbUserFullName As System.Windows.Forms.Label
    Friend WithEvents lbUserName As System.Windows.Forms.Label
    Friend WithEvents grbPermit As System.Windows.Forms.GroupBox
    Friend WithEvents btnReportAssign As System.Windows.Forms.Button
    Friend WithEvents btnTransAssign As System.Windows.Forms.Button
    Friend WithEvents btnFuncAssgin As System.Windows.Forms.Button
    Friend WithEvents grbInfo As System.Windows.Forms.GroupBox
    Friend WithEvents txtUserName As System.Windows.Forms.TextBox
    Friend WithEvents btnTVLK As System.Windows.Forms.Button
    Friend WithEvents lbTLTitle As System.Windows.Forms.Label
    Friend WithEvents txtTLTITLE As System.Windows.Forms.TextBox
    Friend WithEvents cboStatus As Sats.AppCore.ComboBoxEx
    Friend WithEvents lbStatus As System.Windows.Forms.Label
    Friend WithEvents btnStock As System.Windows.Forms.Button
    Friend WithEvents txtTLID As System.Windows.Forms.TextBox
    Friend WithEvents txtGroup As System.Windows.Forms.TextBox
    Friend WithEvents btnBrID As System.Windows.Forms.Button
    Friend WithEvents cboVsd_Brid As Sats.AppCore.ComboBoxEx
    Friend WithEvents lbVSD_BRID As System.Windows.Forms.Label
    Friend WithEvents cboVsd_Brid2 As Sats.AppCore.ComboBoxEx
    Friend WithEvents lbVsd_Brid2 As System.Windows.Forms.Label

End Class
