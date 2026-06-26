<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBRGRP
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmBRGRP))
        Me.grbGroups = New System.Windows.Forms.GroupBox
        Me.txtDESCRIPTION = New System.Windows.Forms.TextBox
        Me.cbSTATUS = New Sats.AppCore.ComboBoxEx
        Me.txtCDNAME = New System.Windows.Forms.TextBox
        Me.cbPRBRID = New Sats.AppCore.ComboBoxEx
        Me.txtBRNAME = New System.Windows.Forms.TextBox
        Me.txtBRID = New Sats.AppCore.FlexMaskEditBox
        Me.lbDesc = New System.Windows.Forms.Label
        Me.lbStatus = New System.Windows.Forms.Label
        Me.lbDCName = New System.Windows.Forms.Label
        Me.lbPRBRID = New System.Windows.Forms.Label
        Me.lbBrName = New System.Windows.Forms.Label
        Me.lbBrid = New System.Windows.Forms.Label
        Me.Panel1.SuspendLayout()
        Me.grbGroups.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Size = New System.Drawing.Size(623, 42)
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(434, 274)
        '
        'btnApply
        '
        Me.btnApply.Location = New System.Drawing.Point(396, 361)
        Me.btnApply.Visible = False
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(520, 274)
        '
        'grbGroups
        '
        Me.grbGroups.Controls.Add(Me.txtDESCRIPTION)
        Me.grbGroups.Controls.Add(Me.cbSTATUS)
        Me.grbGroups.Controls.Add(Me.txtCDNAME)
        Me.grbGroups.Controls.Add(Me.cbPRBRID)
        Me.grbGroups.Controls.Add(Me.txtBRNAME)
        Me.grbGroups.Controls.Add(Me.txtBRID)
        Me.grbGroups.Controls.Add(Me.lbDesc)
        Me.grbGroups.Controls.Add(Me.lbStatus)
        Me.grbGroups.Controls.Add(Me.lbDCName)
        Me.grbGroups.Controls.Add(Me.lbPRBRID)
        Me.grbGroups.Controls.Add(Me.lbBrName)
        Me.grbGroups.Controls.Add(Me.lbBrid)
        Me.grbGroups.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grbGroups.ForeColor = System.Drawing.SystemColors.ActiveCaption
        Me.grbGroups.Location = New System.Drawing.Point(5, 60)
        Me.grbGroups.Name = "grbGroups"
        Me.grbGroups.Size = New System.Drawing.Size(612, 196)
        Me.grbGroups.TabIndex = 4
        Me.grbGroups.TabStop = False
        Me.grbGroups.Tag = "grbGroups"
        Me.grbGroups.Text = "grbGroups"
        '
        'txtDESCRIPTION
        '
        Me.txtDESCRIPTION.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDESCRIPTION.Location = New System.Drawing.Point(141, 160)
        Me.txtDESCRIPTION.Name = "txtDESCRIPTION"
        Me.txtDESCRIPTION.Size = New System.Drawing.Size(385, 20)
        Me.txtDESCRIPTION.TabIndex = 11
        Me.txtDESCRIPTION.Tag = "DESCRIPTION"
        '
        'cbSTATUS
        '
        Me.cbSTATUS.DisplayMember = "DISPLAY"
        Me.cbSTATUS.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbSTATUS.FormattingEnabled = True
        Me.cbSTATUS.Location = New System.Drawing.Point(141, 130)
        Me.cbSTATUS.Name = "cbSTATUS"
        Me.cbSTATUS.Size = New System.Drawing.Size(192, 21)
        Me.cbSTATUS.TabIndex = 10
        Me.cbSTATUS.Tag = "STATUS"
        Me.cbSTATUS.ValueMember = "VALUE"
        '
        'txtCDNAME
        '
        Me.txtCDNAME.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCDNAME.Location = New System.Drawing.Point(141, 104)
        Me.txtCDNAME.Name = "txtCDNAME"
        Me.txtCDNAME.Size = New System.Drawing.Size(385, 20)
        Me.txtCDNAME.TabIndex = 9
        Me.txtCDNAME.Tag = "DCNAME"
        '
        'cbPRBRID
        '
        Me.cbPRBRID.DisplayMember = "DISPLAY"
        Me.cbPRBRID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbPRBRID.FormattingEnabled = True
        Me.cbPRBRID.Location = New System.Drawing.Point(141, 78)
        Me.cbPRBRID.Name = "cbPRBRID"
        Me.cbPRBRID.Size = New System.Drawing.Size(385, 21)
        Me.cbPRBRID.TabIndex = 8
        Me.cbPRBRID.Tag = "PRBRID"
        Me.cbPRBRID.ValueMember = "VALUE"
        '
        'txtBRNAME
        '
        Me.txtBRNAME.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBRNAME.Location = New System.Drawing.Point(141, 52)
        Me.txtBRNAME.Name = "txtBRNAME"
        Me.txtBRNAME.Size = New System.Drawing.Size(385, 20)
        Me.txtBRNAME.TabIndex = 7
        Me.txtBRNAME.Tag = "BRNAME"
        '
        'txtBRID
        '
        Me.txtBRID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBRID.Location = New System.Drawing.Point(141, 26)
        Me.txtBRID.Name = "txtBRID"
        Me.txtBRID.Size = New System.Drawing.Size(100, 20)
        Me.txtBRID.TabIndex = 6
        Me.txtBRID.Tag = "BRID"
        '
        'lbDesc
        '
        Me.lbDesc.AutoSize = True
        Me.lbDesc.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbDesc.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lbDesc.Location = New System.Drawing.Point(7, 167)
        Me.lbDesc.Name = "lbDesc"
        Me.lbDesc.Size = New System.Drawing.Size(40, 13)
        Me.lbDesc.TabIndex = 5
        Me.lbDesc.Tag = "lbDesc"
        Me.lbDesc.Text = "lbDesc"
        '
        'lbStatus
        '
        Me.lbStatus.AutoSize = True
        Me.lbStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lbStatus.Location = New System.Drawing.Point(7, 140)
        Me.lbStatus.Name = "lbStatus"
        Me.lbStatus.Size = New System.Drawing.Size(45, 13)
        Me.lbStatus.TabIndex = 4
        Me.lbStatus.Tag = "lbStatus"
        Me.lbStatus.Text = "lbStatus"
        '
        'lbDCName
        '
        Me.lbDCName.AutoSize = True
        Me.lbDCName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbDCName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lbDCName.Location = New System.Drawing.Point(7, 111)
        Me.lbDCName.Name = "lbDCName"
        Me.lbDCName.Size = New System.Drawing.Size(58, 13)
        Me.lbDCName.TabIndex = 3
        Me.lbDCName.Tag = "lbDCName"
        Me.lbDCName.Text = "lbDCName"
        '
        'lbPRBRID
        '
        Me.lbPRBRID.AutoSize = True
        Me.lbPRBRID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbPRBRID.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lbPRBRID.Location = New System.Drawing.Point(7, 84)
        Me.lbPRBRID.Name = "lbPRBRID"
        Me.lbPRBRID.Size = New System.Drawing.Size(56, 13)
        Me.lbPRBRID.TabIndex = 2
        Me.lbPRBRID.Tag = "lbPRBRID"
        Me.lbPRBRID.Text = "lbPRBRID"
        '
        'lbBrName
        '
        Me.lbBrName.AutoSize = True
        Me.lbBrName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbBrName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lbBrName.Location = New System.Drawing.Point(6, 55)
        Me.lbBrName.Name = "lbBrName"
        Me.lbBrName.Size = New System.Drawing.Size(53, 13)
        Me.lbBrName.TabIndex = 1
        Me.lbBrName.Tag = "lbBrName"
        Me.lbBrName.Text = "lbBrName"
        '
        'lbBrid
        '
        Me.lbBrid.AutoSize = True
        Me.lbBrid.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbBrid.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lbBrid.Location = New System.Drawing.Point(6, 29)
        Me.lbBrid.Name = "lbBrid"
        Me.lbBrid.Size = New System.Drawing.Size(33, 13)
        Me.lbBrid.TabIndex = 0
        Me.lbBrid.Tag = "lbBrid"
        Me.lbBrid.Text = "lbBrid"
        '
        'frmBRGRP
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.ClientSize = New System.Drawing.Size(623, 306)
        Me.Controls.Add(Me.grbGroups)
        Me.Name = "frmBRGRP"
        Me.Controls.SetChildIndex(Me.btnCancel, 0)
        Me.Controls.SetChildIndex(Me.btnOk, 0)
        Me.Controls.SetChildIndex(Me.btnApply, 0)
        Me.Controls.SetChildIndex(Me.grbGroups, 0)
        Me.Controls.SetChildIndex(Me.Panel1, 0)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.grbGroups.ResumeLayout(False)
        Me.grbGroups.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grbGroups As System.Windows.Forms.GroupBox
    Friend WithEvents lbDCName As System.Windows.Forms.Label
    Friend WithEvents lbPRBRID As System.Windows.Forms.Label
    Friend WithEvents lbBrName As System.Windows.Forms.Label
    Friend WithEvents lbBrid As System.Windows.Forms.Label
    Friend WithEvents txtCDNAME As System.Windows.Forms.TextBox
    Friend WithEvents cbPRBRID As Sats.AppCore.ComboBoxEx
    Friend WithEvents txtBRNAME As System.Windows.Forms.TextBox
    Friend WithEvents txtBRID As Sats.AppCore.FlexMaskEditBox
    Friend WithEvents lbDesc As System.Windows.Forms.Label
    Friend WithEvents lbStatus As System.Windows.Forms.Label
    Friend WithEvents txtDESCRIPTION As System.Windows.Forms.TextBox
    Friend WithEvents cbSTATUS As Sats.AppCore.ComboBoxEx

End Class
