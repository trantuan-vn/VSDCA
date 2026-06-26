<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBatch
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmBatch))
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.lbCaption = New System.Windows.Forms.Label
        Me.grpSA = New System.Windows.Forms.GroupBox
        Me.dtpCurrDate = New System.Windows.Forms.DateTimePicker
        Me.lblCurrDate = New System.Windows.Forms.Label
        Me.cboBranchId = New Sats.AppCore.ComboBoxEx
        Me.lbBranchId = New System.Windows.Forms.Label
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnOk = New System.Windows.Forms.Button
        Me.Panel1.SuspendLayout()
        Me.grpSA.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.SystemColors.Desktop
        Me.Panel1.Controls.Add(Me.lbCaption)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(417, 56)
        Me.Panel1.TabIndex = 8
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
        'grpSA
        '
        Me.grpSA.Controls.Add(Me.dtpCurrDate)
        Me.grpSA.Controls.Add(Me.lblCurrDate)
        Me.grpSA.Controls.Add(Me.cboBranchId)
        Me.grpSA.Controls.Add(Me.lbBranchId)
        Me.grpSA.Location = New System.Drawing.Point(3, 57)
        Me.grpSA.Name = "grpSA"
        Me.grpSA.Size = New System.Drawing.Size(408, 80)
        Me.grpSA.TabIndex = 9
        Me.grpSA.TabStop = False
        Me.grpSA.Tag = "grpSA"
        Me.grpSA.Text = "grpSA"
        '
        'dtpCurrDate
        '
        Me.dtpCurrDate.Enabled = False
        Me.dtpCurrDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpCurrDate.Location = New System.Drawing.Point(119, 51)
        Me.dtpCurrDate.Name = "dtpCurrDate"
        Me.dtpCurrDate.Size = New System.Drawing.Size(95, 20)
        Me.dtpCurrDate.TabIndex = 3
        '
        'lblCurrDate
        '
        Me.lblCurrDate.AutoSize = True
        Me.lblCurrDate.Location = New System.Drawing.Point(32, 52)
        Me.lblCurrDate.Name = "lblCurrDate"
        Me.lblCurrDate.Size = New System.Drawing.Size(59, 13)
        Me.lblCurrDate.TabIndex = 2
        Me.lblCurrDate.Tag = "lblCurrDate"
        Me.lblCurrDate.Text = "lblCurrDate"
        '
        'cboBranchId
        '
        Me.cboBranchId.DisplayMember = "DISPLAY"
        Me.cboBranchId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBranchId.FormattingEnabled = True
        Me.cboBranchId.Location = New System.Drawing.Point(119, 17)
        Me.cboBranchId.Name = "cboBranchId"
        Me.cboBranchId.Size = New System.Drawing.Size(273, 21)
        Me.cboBranchId.TabIndex = 1
        Me.cboBranchId.Tag = "cboBranchId"
        Me.cboBranchId.ValueMember = "VALUE"
        '
        'lbBranchId
        '
        Me.lbBranchId.AutoSize = True
        Me.lbBranchId.Location = New System.Drawing.Point(33, 20)
        Me.lbBranchId.Name = "lbBranchId"
        Me.lbBranchId.Size = New System.Drawing.Size(58, 13)
        Me.lbBranchId.TabIndex = 0
        Me.lbBranchId.Tag = "lbBranchId"
        Me.lbBranchId.Text = "lbBranchId"
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(329, 147)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(82, 27)
        Me.btnCancel.TabIndex = 11
        Me.btnCancel.Tag = "btnCancel"
        Me.btnCancel.Text = "btnCancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(237, 147)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(86, 27)
        Me.btnOk.TabIndex = 10
        Me.btnOk.Tag = "btnOk"
        Me.btnOk.Text = "btnOk"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'frmBatch
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(417, 184)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.grpSA)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmBatch"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "frmBatch"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.grpSA.ResumeLayout(False)
        Me.grpSA.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents lbCaption As System.Windows.Forms.Label
    Friend WithEvents grpSA As System.Windows.Forms.GroupBox
    Friend WithEvents cboBranchId As Sats.AppCore.ComboBoxEx
    Friend WithEvents lbBranchId As System.Windows.Forms.Label
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents dtpCurrDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents lblCurrDate As System.Windows.Forms.Label
End Class
