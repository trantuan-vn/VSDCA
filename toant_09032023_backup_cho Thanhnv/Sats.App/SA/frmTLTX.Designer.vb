<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTLTX
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTLTX))
        Me.lbTLTXCD = New System.Windows.Forms.Label
        Me.txtTltxcd = New System.Windows.Forms.TextBox
        Me.txtTxDESC = New System.Windows.Forms.TextBox
        Me.lbTxDesc = New System.Windows.Forms.Label
        Me.lbLevel1 = New System.Windows.Forms.Label
        Me.lbLevel2 = New System.Windows.Forms.Label
        Me.lbLevel3 = New System.Windows.Forms.Label
        Me.cboLevel1 = New Sats.AppCore.ComboBoxEx
        Me.cboLevel2 = New Sats.AppCore.ComboBoxEx
        Me.cboLevel3 = New Sats.AppCore.ComboBoxEx
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Size = New System.Drawing.Size(448, 42)
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(123, 204)
        Me.btnOk.TabIndex = 6
        Me.btnOk.Text = "&Chấp nhận"
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(237, 204)
        Me.btnCancel.TabIndex = 7
        '
        'lbTLTXCD
        '
        Me.lbTLTXCD.AutoSize = True
        Me.lbTLTXCD.Location = New System.Drawing.Point(12, 64)
        Me.lbTLTXCD.Name = "lbTLTXCD"
        Me.lbTLTXCD.Size = New System.Drawing.Size(57, 13)
        Me.lbTLTXCD.TabIndex = 5
        Me.lbTLTXCD.Tag = "lbTLTXCD"
        Me.lbTLTXCD.Text = "lbTLTXCD"
        '
        'txtTltxcd
        '
        Me.txtTltxcd.Location = New System.Drawing.Point(115, 57)
        Me.txtTltxcd.Name = "txtTltxcd"
        Me.txtTltxcd.Size = New System.Drawing.Size(107, 20)
        Me.txtTltxcd.TabIndex = 1
        Me.txtTltxcd.Tag = "TLTXCD"
        '
        'txtTxDESC
        '
        Me.txtTxDESC.Location = New System.Drawing.Point(115, 84)
        Me.txtTxDESC.Name = "txtTxDESC"
        Me.txtTxDESC.Size = New System.Drawing.Size(331, 20)
        Me.txtTxDESC.TabIndex = 2
        Me.txtTxDESC.Tag = "TXDESC"
        '
        'lbTxDesc
        '
        Me.lbTxDesc.AutoSize = True
        Me.lbTxDesc.Location = New System.Drawing.Point(12, 91)
        Me.lbTxDesc.Name = "lbTxDesc"
        Me.lbTxDesc.Size = New System.Drawing.Size(58, 13)
        Me.lbTxDesc.TabIndex = 10
        Me.lbTxDesc.Tag = "lbTXDESC"
        Me.lbTxDesc.Text = "lbTXDESC"
        '
        'lbLevel1
        '
        Me.lbLevel1.AutoSize = True
        Me.lbLevel1.Location = New System.Drawing.Point(12, 119)
        Me.lbLevel1.Name = "lbLevel1"
        Me.lbLevel1.Size = New System.Drawing.Size(47, 13)
        Me.lbLevel1.TabIndex = 11
        Me.lbLevel1.Tag = "lbLevel1"
        Me.lbLevel1.Text = "lbLevel1"
        '
        'lbLevel2
        '
        Me.lbLevel2.AutoSize = True
        Me.lbLevel2.Location = New System.Drawing.Point(12, 147)
        Me.lbLevel2.Name = "lbLevel2"
        Me.lbLevel2.Size = New System.Drawing.Size(47, 13)
        Me.lbLevel2.TabIndex = 12
        Me.lbLevel2.Tag = "lbLevel2"
        Me.lbLevel2.Text = "lbLevel2"
        '
        'lbLevel3
        '
        Me.lbLevel3.AutoSize = True
        Me.lbLevel3.Location = New System.Drawing.Point(12, 175)
        Me.lbLevel3.Name = "lbLevel3"
        Me.lbLevel3.Size = New System.Drawing.Size(47, 13)
        Me.lbLevel3.TabIndex = 13
        Me.lbLevel3.Tag = "lbLevel3"
        Me.lbLevel3.Text = "lbLevel3"
        '
        'cboLevel1
        '
        Me.cboLevel1.DisplayMember = "DISPLAY"
        Me.cboLevel1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboLevel1.FormattingEnabled = True
        Me.cboLevel1.Location = New System.Drawing.Point(115, 111)
        Me.cboLevel1.Name = "cboLevel1"
        Me.cboLevel1.Size = New System.Drawing.Size(179, 21)
        Me.cboLevel1.TabIndex = 3
        Me.cboLevel1.Tag = "CHKID"
        Me.cboLevel1.ValueMember = "VALUE"
        '
        'cboLevel2
        '
        Me.cboLevel2.DisplayMember = "DISPLAY"
        Me.cboLevel2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboLevel2.FormattingEnabled = True
        Me.cboLevel2.Location = New System.Drawing.Point(115, 139)
        Me.cboLevel2.Name = "cboLevel2"
        Me.cboLevel2.Size = New System.Drawing.Size(179, 21)
        Me.cboLevel2.TabIndex = 4
        Me.cboLevel2.Tag = "OFFID"
        Me.cboLevel2.ValueMember = "VALUE"
        '
        'cboLevel3
        '
        Me.cboLevel3.DisplayMember = "DISPLAY"
        Me.cboLevel3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboLevel3.FormattingEnabled = True
        Me.cboLevel3.Location = New System.Drawing.Point(115, 167)
        Me.cboLevel3.Name = "cboLevel3"
        Me.cboLevel3.Size = New System.Drawing.Size(179, 21)
        Me.cboLevel3.TabIndex = 5
        Me.cboLevel3.Tag = "CFRID"
        Me.cboLevel3.ValueMember = "VALUE"
        '
        'frmTLTX
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.ClientSize = New System.Drawing.Size(448, 241)
        Me.Controls.Add(Me.cboLevel3)
        Me.Controls.Add(Me.cboLevel2)
        Me.Controls.Add(Me.cboLevel1)
        Me.Controls.Add(Me.lbLevel3)
        Me.Controls.Add(Me.lbLevel2)
        Me.Controls.Add(Me.lbLevel1)
        Me.Controls.Add(Me.lbTxDesc)
        Me.Controls.Add(Me.txtTxDESC)
        Me.Controls.Add(Me.txtTltxcd)
        Me.Controls.Add(Me.lbTLTXCD)
        Me.Name = "frmTLTX"
        Me.Controls.SetChildIndex(Me.lbTLTXCD, 0)
        Me.Controls.SetChildIndex(Me.txtTltxcd, 0)
        Me.Controls.SetChildIndex(Me.btnCancel, 0)
        Me.Controls.SetChildIndex(Me.btnOk, 0)
        Me.Controls.SetChildIndex(Me.Panel1, 0)
        Me.Controls.SetChildIndex(Me.btnApply, 0)
        Me.Controls.SetChildIndex(Me.txtTxDESC, 0)
        Me.Controls.SetChildIndex(Me.lbTxDesc, 0)
        Me.Controls.SetChildIndex(Me.lbLevel1, 0)
        Me.Controls.SetChildIndex(Me.lbLevel2, 0)
        Me.Controls.SetChildIndex(Me.lbLevel3, 0)
        Me.Controls.SetChildIndex(Me.cboLevel1, 0)
        Me.Controls.SetChildIndex(Me.cboLevel2, 0)
        Me.Controls.SetChildIndex(Me.cboLevel3, 0)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lbTLTXCD As System.Windows.Forms.Label
    Friend WithEvents txtTltxcd As System.Windows.Forms.TextBox
    Friend WithEvents txtTxDESC As System.Windows.Forms.TextBox
    Friend WithEvents lbTxDesc As System.Windows.Forms.Label
    Friend WithEvents lbLevel1 As System.Windows.Forms.Label
    Friend WithEvents lbLevel2 As System.Windows.Forms.Label
    Friend WithEvents lbLevel3 As System.Windows.Forms.Label
    Friend WithEvents cboLevel1 As Sats.AppCore.ComboBoxEx
    Friend WithEvents cboLevel2 As Sats.AppCore.ComboBoxEx
    Friend WithEvents cboLevel3 As Sats.AppCore.ComboBoxEx

End Class
