<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmExportExcelForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmExportExcelForm))
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.lbCaption = New System.Windows.Forms.Label
        Me.grbInfo = New System.Windows.Forms.GroupBox
        Me.btnBrowse = New System.Windows.Forms.Button
        Me.txtFileLocation = New System.Windows.Forms.TextBox
        Me.lbFileLocation = New System.Windows.Forms.Label
        Me.cboTransCode = New Sats.AppCore.ComboBoxEx
        Me.lbTransCode = New System.Windows.Forms.Label
        Me.cboSysPart = New Sats.AppCore.ComboBoxEx
        Me.lbSysPart = New System.Windows.Forms.Label
        Me.btnOk = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.Panel1.SuspendLayout()
        Me.grbInfo.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.SystemColors.Desktop
        Me.Panel1.Controls.Add(Me.lbCaption)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(534, 56)
        Me.Panel1.TabIndex = 4
        Me.Panel1.Tag = ""
        '
        'lbCaption
        '
        Me.lbCaption.AutoSize = True
        Me.lbCaption.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbCaption.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lbCaption.Location = New System.Drawing.Point(23, 20)
        Me.lbCaption.Name = "lbCaption"
        Me.lbCaption.Size = New System.Drawing.Size(60, 13)
        Me.lbCaption.TabIndex = 0
        Me.lbCaption.Tag = "lbCaption"
        Me.lbCaption.Text = "lbCaption"
        '
        'grbInfo
        '
        Me.grbInfo.Controls.Add(Me.btnBrowse)
        Me.grbInfo.Controls.Add(Me.txtFileLocation)
        Me.grbInfo.Controls.Add(Me.lbFileLocation)
        Me.grbInfo.Controls.Add(Me.cboTransCode)
        Me.grbInfo.Controls.Add(Me.lbTransCode)
        Me.grbInfo.Controls.Add(Me.cboSysPart)
        Me.grbInfo.Controls.Add(Me.lbSysPart)
        Me.grbInfo.Location = New System.Drawing.Point(12, 62)
        Me.grbInfo.Name = "grbInfo"
        Me.grbInfo.Size = New System.Drawing.Size(510, 177)
        Me.grbInfo.TabIndex = 5
        Me.grbInfo.TabStop = False
        Me.grbInfo.Tag = "grbInfo"
        Me.grbInfo.Text = "grbInfo"
        '
        'btnBrowse
        '
        Me.btnBrowse.Location = New System.Drawing.Point(387, 131)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(75, 23)
        Me.btnBrowse.TabIndex = 6
        Me.btnBrowse.Tag = "btnBrowse"
        Me.btnBrowse.Text = "btnBrowse"
        Me.btnBrowse.UseVisualStyleBackColor = True
        '
        'txtFileLocation
        '
        Me.txtFileLocation.Location = New System.Drawing.Point(120, 103)
        Me.txtFileLocation.Name = "txtFileLocation"
        Me.txtFileLocation.Size = New System.Drawing.Size(342, 20)
        Me.txtFileLocation.TabIndex = 5
        Me.txtFileLocation.Tag = "txtFileLocation"
        '
        'lbFileLocation
        '
        Me.lbFileLocation.AutoSize = True
        Me.lbFileLocation.Location = New System.Drawing.Point(41, 106)
        Me.lbFileLocation.Name = "lbFileLocation"
        Me.lbFileLocation.Size = New System.Drawing.Size(72, 13)
        Me.lbFileLocation.TabIndex = 4
        Me.lbFileLocation.Tag = "lbFileLocation"
        Me.lbFileLocation.Text = "lbFileLocation"
        '
        'cboTransCode
        '
        Me.cboTransCode.DisplayMember = "DISPLAY"
        Me.cboTransCode.FormattingEnabled = True
        Me.cboTransCode.Location = New System.Drawing.Point(120, 62)
        Me.cboTransCode.Name = "cboTransCode"
        Me.cboTransCode.Size = New System.Drawing.Size(342, 21)
        Me.cboTransCode.TabIndex = 3
        Me.cboTransCode.Tag = "cboTransCode"
        Me.cboTransCode.ValueMember = "VALUE"
        '
        'lbTransCode
        '
        Me.lbTransCode.AutoSize = True
        Me.lbTransCode.Location = New System.Drawing.Point(41, 65)
        Me.lbTransCode.Name = "lbTransCode"
        Me.lbTransCode.Size = New System.Drawing.Size(67, 13)
        Me.lbTransCode.TabIndex = 2
        Me.lbTransCode.Tag = "lbTransCode"
        Me.lbTransCode.Text = "lbTransCode"
        '
        'cboSysPart
        '
        Me.cboSysPart.DisplayMember = "DISPLAY"
        Me.cboSysPart.FormattingEnabled = True
        Me.cboSysPart.Location = New System.Drawing.Point(120, 21)
        Me.cboSysPart.Name = "cboSysPart"
        Me.cboSysPart.Size = New System.Drawing.Size(342, 21)
        Me.cboSysPart.TabIndex = 1
        Me.cboSysPart.Tag = "cboSysPart"
        Me.cboSysPart.ValueMember = "VALUE"
        '
        'lbSysPart
        '
        Me.lbSysPart.AutoSize = True
        Me.lbSysPart.Location = New System.Drawing.Point(41, 24)
        Me.lbSysPart.Name = "lbSysPart"
        Me.lbSysPart.Size = New System.Drawing.Size(51, 13)
        Me.lbSysPart.TabIndex = 0
        Me.lbSysPart.Tag = "lbSysPart"
        Me.lbSysPart.Text = "lbSysPart"
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(190, 245)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(79, 35)
        Me.btnOk.TabIndex = 6
        Me.btnOk.Tag = "btnOk"
        Me.btnOk.Text = "btnOk"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(275, 245)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 35)
        Me.btnCancel.TabIndex = 7
        Me.btnCancel.Tag = "btnCancel"
        Me.btnCancel.Text = "btnCancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'frmExportExcelForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(534, 283)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.grbInfo)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "frmExportExcelForm"
        Me.Text = "frmExportExcelForm"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.grbInfo.ResumeLayout(False)
        Me.grbInfo.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents lbCaption As System.Windows.Forms.Label
    Friend WithEvents grbInfo As System.Windows.Forms.GroupBox
    Friend WithEvents lbFileLocation As System.Windows.Forms.Label
    Friend WithEvents cboTransCode As Sats.AppCore.ComboBoxEx
    Friend WithEvents lbTransCode As System.Windows.Forms.Label
    Friend WithEvents cboSysPart As Sats.AppCore.ComboBoxEx
    Friend WithEvents lbSysPart As System.Windows.Forms.Label
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents txtFileLocation As System.Windows.Forms.TextBox
    Friend WithEvents btnBrowse As System.Windows.Forms.Button
End Class
