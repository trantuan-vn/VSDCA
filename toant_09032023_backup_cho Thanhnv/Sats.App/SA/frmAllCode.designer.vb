<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAllCode
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAllCode))
        Me.grbAllCode = New System.Windows.Forms.GroupBox
        Me.txtLSTODR = New Sats.AppCore.FlexMaskEditBox
        Me.lbCDContent = New System.Windows.Forms.Label
        Me.txtCDContent = New System.Windows.Forms.TextBox
        Me.cboCDUser = New Sats.AppCore.ComboBoxEx
        Me.lbCDUser = New System.Windows.Forms.Label
        Me.lbLSTORD = New System.Windows.Forms.Label
        Me.txtCDName = New System.Windows.Forms.TextBox
        Me.cboCDType = New Sats.AppCore.ComboBoxEx
        Me.lbCDType = New System.Windows.Forms.Label
        Me.txtCDVal = New Sats.AppCore.FlexMaskEditBox
        Me.lbCDVal = New System.Windows.Forms.Label
        Me.lbCDName = New System.Windows.Forms.Label
        Me.txtAUTOID = New System.Windows.Forms.TextBox
        Me.Panel1.SuspendLayout()
        Me.grbAllCode.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.Panel1.Size = New System.Drawing.Size(599, 42)
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(343, 174)
        '
        'btnApply
        '
        Me.btnApply.Location = New System.Drawing.Point(425, 174)
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(507, 174)
        '
        'grbAllCode
        '
        Me.grbAllCode.Controls.Add(Me.txtLSTODR)
        Me.grbAllCode.Controls.Add(Me.lbCDContent)
        Me.grbAllCode.Controls.Add(Me.txtCDContent)
        Me.grbAllCode.Controls.Add(Me.cboCDUser)
        Me.grbAllCode.Controls.Add(Me.lbCDUser)
        Me.grbAllCode.Controls.Add(Me.lbLSTORD)
        Me.grbAllCode.Controls.Add(Me.txtCDName)
        Me.grbAllCode.Controls.Add(Me.cboCDType)
        Me.grbAllCode.Controls.Add(Me.lbCDType)
        Me.grbAllCode.Controls.Add(Me.txtCDVal)
        Me.grbAllCode.Controls.Add(Me.lbCDVal)
        Me.grbAllCode.Controls.Add(Me.lbCDName)
        Me.grbAllCode.Location = New System.Drawing.Point(5, 42)
        Me.grbAllCode.Name = "grbAllCode"
        Me.grbAllCode.Size = New System.Drawing.Size(581, 125)
        Me.grbAllCode.TabIndex = 4
        Me.grbAllCode.TabStop = False
        Me.grbAllCode.Tag = "grbAllCode"
        Me.grbAllCode.Text = "grbAllCode"
        '
        'txtLSTODR
        '
        Me.txtLSTODR.Location = New System.Drawing.Point(129, 94)
        Me.txtLSTODR.Name = "txtLSTODR"
        Me.txtLSTODR.Size = New System.Drawing.Size(172, 20)
        Me.txtLSTODR.TabIndex = 12
        Me.txtLSTODR.Tag = "LSTODR"
        '
        'lbCDContent
        '
        Me.lbCDContent.AutoSize = True
        Me.lbCDContent.Location = New System.Drawing.Point(9, 72)
        Me.lbCDContent.Name = "lbCDContent"
        Me.lbCDContent.Size = New System.Drawing.Size(82, 13)
        Me.lbCDContent.TabIndex = 11
        Me.lbCDContent.Tag = "lbCDCONTENT"
        Me.lbCDContent.Text = "lbCDCONTENT"
        '
        'txtCDContent
        '
        Me.txtCDContent.Location = New System.Drawing.Point(129, 69)
        Me.txtCDContent.Name = "txtCDContent"
        Me.txtCDContent.Size = New System.Drawing.Size(446, 20)
        Me.txtCDContent.TabIndex = 10
        Me.txtCDContent.Tag = "CDCONTENT"
        '
        'cboCDUser
        '
        Me.cboCDUser.DisplayMember = "DISPLAY"
        Me.cboCDUser.FormattingEnabled = True
        Me.cboCDUser.Location = New System.Drawing.Point(437, 94)
        Me.cboCDUser.Name = "cboCDUser"
        Me.cboCDUser.Size = New System.Drawing.Size(138, 21)
        Me.cboCDUser.TabIndex = 9
        Me.cboCDUser.Tag = "CDUSER"
        Me.cboCDUser.ValueMember = "VALUE"
        '
        'lbCDUser
        '
        Me.lbCDUser.AutoSize = True
        Me.lbCDUser.Location = New System.Drawing.Point(327, 97)
        Me.lbCDUser.Name = "lbCDUser"
        Me.lbCDUser.Size = New System.Drawing.Size(60, 13)
        Me.lbCDUser.TabIndex = 8
        Me.lbCDUser.Tag = "lbCDUSER"
        Me.lbCDUser.Text = "lbCDUSER"
        '
        'lbLSTORD
        '
        Me.lbLSTORD.AutoSize = True
        Me.lbLSTORD.Location = New System.Drawing.Point(9, 97)
        Me.lbLSTORD.Name = "lbLSTORD"
        Me.lbLSTORD.Size = New System.Drawing.Size(59, 13)
        Me.lbLSTORD.TabIndex = 7
        Me.lbLSTORD.Tag = "lbLSTORD"
        Me.lbLSTORD.Text = "lbLSTORD"
        '
        'txtCDName
        '
        Me.txtCDName.Location = New System.Drawing.Point(130, 43)
        Me.txtCDName.Name = "txtCDName"
        Me.txtCDName.Size = New System.Drawing.Size(171, 20)
        Me.txtCDName.TabIndex = 5
        Me.txtCDName.Tag = "CDNAME"
        '
        'cboCDType
        '
        Me.cboCDType.DisplayMember = "DISPLAY"
        Me.cboCDType.FormattingEnabled = True
        Me.cboCDType.Location = New System.Drawing.Point(130, 16)
        Me.cboCDType.Name = "cboCDType"
        Me.cboCDType.Size = New System.Drawing.Size(444, 21)
        Me.cboCDType.TabIndex = 4
        Me.cboCDType.Tag = "CDTYPE"
        Me.cboCDType.ValueMember = "VALUE"
        '
        'lbCDType
        '
        Me.lbCDType.AutoSize = True
        Me.lbCDType.Location = New System.Drawing.Point(9, 19)
        Me.lbCDType.Name = "lbCDType"
        Me.lbCDType.Size = New System.Drawing.Size(58, 13)
        Me.lbCDType.TabIndex = 3
        Me.lbCDType.Tag = "lbCDTYPE"
        Me.lbCDType.Text = "lbCDTYPE"
        '
        'txtCDVal
        '
        Me.txtCDVal.Location = New System.Drawing.Point(437, 43)
        Me.txtCDVal.Name = "txtCDVal"
        Me.txtCDVal.Size = New System.Drawing.Size(137, 20)
        Me.txtCDVal.TabIndex = 2
        Me.txtCDVal.Tag = "CDVAL"
        '
        'lbCDVal
        '
        Me.lbCDVal.AutoSize = True
        Me.lbCDVal.Location = New System.Drawing.Point(327, 46)
        Me.lbCDVal.Name = "lbCDVal"
        Me.lbCDVal.Size = New System.Drawing.Size(50, 13)
        Me.lbCDVal.TabIndex = 1
        Me.lbCDVal.Tag = "lbCDVAL"
        Me.lbCDVal.Text = "lbCDVAL"
        '
        'lbCDName
        '
        Me.lbCDName.AutoSize = True
        Me.lbCDName.Location = New System.Drawing.Point(9, 46)
        Me.lbCDName.Name = "lbCDName"
        Me.lbCDName.Size = New System.Drawing.Size(61, 13)
        Me.lbCDName.TabIndex = 0
        Me.lbCDName.Tag = "lbCDNAME"
        Me.lbCDName.Text = "lbCDNAME"
        '
        'txtAUTOID
        '
        Me.txtAUTOID.Location = New System.Drawing.Point(5, 173)
        Me.txtAUTOID.Name = "txtAUTOID"
        Me.txtAUTOID.Size = New System.Drawing.Size(125, 20)
        Me.txtAUTOID.TabIndex = 5
        Me.txtAUTOID.Tag = "AUTOID"
        Me.txtAUTOID.Visible = False
        '
        'frmAllCode
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.ClientSize = New System.Drawing.Size(599, 207)
        Me.Controls.Add(Me.txtAUTOID)
        Me.Controls.Add(Me.grbAllCode)
        Me.Name = "frmAllCode"
        Me.Controls.SetChildIndex(Me.Panel1, 0)
        Me.Controls.SetChildIndex(Me.btnOk, 0)
        Me.Controls.SetChildIndex(Me.btnApply, 0)
        Me.Controls.SetChildIndex(Me.btnCancel, 0)
        Me.Controls.SetChildIndex(Me.grbAllCode, 0)
        Me.Controls.SetChildIndex(Me.txtAUTOID, 0)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.grbAllCode.ResumeLayout(False)
        Me.grbAllCode.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents grbAllCode As System.Windows.Forms.GroupBox
    Friend WithEvents lbCDVal As System.Windows.Forms.Label
    Friend WithEvents lbCDName As System.Windows.Forms.Label

    Public Sub New()
        MyBase.New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Friend WithEvents txtCDVal As Sats.AppCore.FlexMaskEditBox
    Friend WithEvents cboCDType As Sats.AppCore.ComboBoxEx
    Friend WithEvents lbCDType As System.Windows.Forms.Label
    Friend WithEvents lbCDContent As System.Windows.Forms.Label
    Friend WithEvents txtCDContent As System.Windows.Forms.TextBox
    Friend WithEvents cboCDUser As Sats.AppCore.ComboBoxEx
    Friend WithEvents lbCDUser As System.Windows.Forms.Label
    Friend WithEvents lbLSTORD As System.Windows.Forms.Label
    Friend WithEvents txtCDName As System.Windows.Forms.TextBox
    Friend WithEvents txtAUTOID As System.Windows.Forms.TextBox
    Friend WithEvents txtLSTODR As Sats.AppCore.FlexMaskEditBox
End Class
