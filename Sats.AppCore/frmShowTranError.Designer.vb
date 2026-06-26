<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmShowTranError
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
        Me.btnOK = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnExport = New System.Windows.Forms.Button
        Me.Spc = New System.Windows.Forms.SplitContainer
        Me.grbMessage = New System.Windows.Forms.GroupBox
        Me.rtb = New System.Windows.Forms.RichTextBox
        Me.tbMesages = New System.Windows.Forms.TabControl
        Me.tbErrors = New System.Windows.Forms.TabPage
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer
        Me.lstTranError = New System.Windows.Forms.ListBox
        Me.txtErrors = New System.Windows.Forms.TextBox
        Me.tbWarnings = New System.Windows.Forms.TabPage
        Me.SplitContainer3 = New System.Windows.Forms.SplitContainer
        Me.lstWarning = New System.Windows.Forms.CheckedListBox
        Me.txtWarnings = New System.Windows.Forms.TextBox
        Me.Spc.Panel1.SuspendLayout()
        Me.Spc.Panel2.SuspendLayout()
        Me.Spc.SuspendLayout()
        Me.grbMessage.SuspendLayout()
        Me.tbMesages.SuspendLayout()
        Me.tbErrors.SuspendLayout()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        Me.tbWarnings.SuspendLayout()
        Me.SplitContainer3.Panel1.SuspendLayout()
        Me.SplitContainer3.Panel2.SuspendLayout()
        Me.SplitContainer3.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnOK
        '
        Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOK.Location = New System.Drawing.Point(394, 485)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 31)
        Me.btnOK.TabIndex = 7
        Me.btnOK.Text = "Chấp nhận"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnExit.Location = New System.Drawing.Point(475, 485)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(75, 31)
        Me.btnExit.TabIndex = 6
        Me.btnExit.Text = "Thoát"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(314, 485)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(74, 31)
        Me.btnExport.TabIndex = 5
        Me.btnExport.Text = "Kết xuất"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'Spc
        '
        Me.Spc.Dock = System.Windows.Forms.DockStyle.Top
        Me.Spc.Location = New System.Drawing.Point(0, 0)
        Me.Spc.Name = "Spc"
        Me.Spc.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'Spc.Panel1
        '
        Me.Spc.Panel1.Controls.Add(Me.grbMessage)
        '
        'Spc.Panel2
        '
        Me.Spc.Panel2.Controls.Add(Me.tbMesages)
        Me.Spc.Size = New System.Drawing.Size(557, 483)
        Me.Spc.SplitterDistance = 148
        Me.Spc.TabIndex = 8
        '
        'grbMessage
        '
        Me.grbMessage.Controls.Add(Me.rtb)
        Me.grbMessage.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grbMessage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grbMessage.Location = New System.Drawing.Point(0, 0)
        Me.grbMessage.Name = "grbMessage"
        Me.grbMessage.Size = New System.Drawing.Size(557, 148)
        Me.grbMessage.TabIndex = 0
        Me.grbMessage.TabStop = False
        Me.grbMessage.Text = "Thông báo"
        '
        'rtb
        '
        Me.rtb.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rtb.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rtb.Location = New System.Drawing.Point(3, 16)
        Me.rtb.Name = "rtb"
        Me.rtb.ReadOnly = True
        Me.rtb.Size = New System.Drawing.Size(551, 129)
        Me.rtb.TabIndex = 1
        Me.rtb.Text = ""
        '
        'tbMesages
        '
        Me.tbMesages.Controls.Add(Me.tbErrors)
        Me.tbMesages.Controls.Add(Me.tbWarnings)
        Me.tbMesages.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tbMesages.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbMesages.Location = New System.Drawing.Point(0, 0)
        Me.tbMesages.Name = "tbMesages"
        Me.tbMesages.SelectedIndex = 0
        Me.tbMesages.Size = New System.Drawing.Size(557, 331)
        Me.tbMesages.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
        Me.tbMesages.TabIndex = 5
        '
        'tbErrors
        '
        Me.tbErrors.Controls.Add(Me.SplitContainer2)
        Me.tbErrors.Location = New System.Drawing.Point(4, 22)
        Me.tbErrors.Name = "tbErrors"
        Me.tbErrors.Padding = New System.Windows.Forms.Padding(3)
        Me.tbErrors.Size = New System.Drawing.Size(549, 305)
        Me.tbErrors.TabIndex = 2
        Me.tbErrors.Text = "0 lỗi"
        Me.tbErrors.UseVisualStyleBackColor = True
        '
        'SplitContainer2
        '
        Me.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer2.Location = New System.Drawing.Point(3, 3)
        Me.SplitContainer2.Name = "SplitContainer2"
        '
        'SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.Controls.Add(Me.lstTranError)
        '
        'SplitContainer2.Panel2
        '
        Me.SplitContainer2.Panel2.Controls.Add(Me.txtErrors)
        Me.SplitContainer2.Size = New System.Drawing.Size(543, 299)
        Me.SplitContainer2.SplitterDistance = 154
        Me.SplitContainer2.TabIndex = 0
        '
        'lstTranError
        '
        Me.lstTranError.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstTranError.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstTranError.FormattingEnabled = True
        Me.lstTranError.Location = New System.Drawing.Point(0, 0)
        Me.lstTranError.Name = "lstTranError"
        Me.lstTranError.Size = New System.Drawing.Size(154, 290)
        Me.lstTranError.TabIndex = 1
        '
        'txtErrors
        '
        Me.txtErrors.BackColor = System.Drawing.SystemColors.Window
        Me.txtErrors.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtErrors.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtErrors.Location = New System.Drawing.Point(0, 0)
        Me.txtErrors.Multiline = True
        Me.txtErrors.Name = "txtErrors"
        Me.txtErrors.ReadOnly = True
        Me.txtErrors.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtErrors.Size = New System.Drawing.Size(385, 299)
        Me.txtErrors.TabIndex = 5
        '
        'tbWarnings
        '
        Me.tbWarnings.Controls.Add(Me.SplitContainer3)
        Me.tbWarnings.Location = New System.Drawing.Point(4, 22)
        Me.tbWarnings.Name = "tbWarnings"
        Me.tbWarnings.Padding = New System.Windows.Forms.Padding(3)
        Me.tbWarnings.Size = New System.Drawing.Size(549, 305)
        Me.tbWarnings.TabIndex = 1
        Me.tbWarnings.Text = "0 cảnh báo"
        Me.tbWarnings.UseVisualStyleBackColor = True
        '
        'SplitContainer3
        '
        Me.SplitContainer3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer3.Location = New System.Drawing.Point(3, 3)
        Me.SplitContainer3.Name = "SplitContainer3"
        '
        'SplitContainer3.Panel1
        '
        Me.SplitContainer3.Panel1.Controls.Add(Me.lstWarning)
        '
        'SplitContainer3.Panel2
        '
        Me.SplitContainer3.Panel2.Controls.Add(Me.txtWarnings)
        Me.SplitContainer3.Size = New System.Drawing.Size(543, 299)
        Me.SplitContainer3.SplitterDistance = 154
        Me.SplitContainer3.TabIndex = 1
        '
        'lstWarning
        '
        Me.lstWarning.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstWarning.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstWarning.FormattingEnabled = True
        Me.lstWarning.Location = New System.Drawing.Point(0, 0)
        Me.lstWarning.Name = "lstWarning"
        Me.lstWarning.Size = New System.Drawing.Size(154, 289)
        Me.lstWarning.TabIndex = 0
        '
        'txtWarnings
        '
        Me.txtWarnings.BackColor = System.Drawing.SystemColors.Window
        Me.txtWarnings.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtWarnings.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtWarnings.Location = New System.Drawing.Point(0, 0)
        Me.txtWarnings.Multiline = True
        Me.txtWarnings.Name = "txtWarnings"
        Me.txtWarnings.ReadOnly = True
        Me.txtWarnings.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtWarnings.Size = New System.Drawing.Size(385, 299)
        Me.txtWarnings.TabIndex = 5
        '
        'frmShowTranError
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(557, 520)
        Me.Controls.Add(Me.Spc)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.btnExport)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmShowTranError"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Thông báo duyệt giao dịch"
        Me.Spc.Panel1.ResumeLayout(False)
        Me.Spc.Panel2.ResumeLayout(False)
        Me.Spc.ResumeLayout(False)
        Me.grbMessage.ResumeLayout(False)
        Me.tbMesages.ResumeLayout(False)
        Me.tbErrors.ResumeLayout(False)
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        Me.SplitContainer2.Panel2.PerformLayout()
        Me.SplitContainer2.ResumeLayout(False)
        Me.tbWarnings.ResumeLayout(False)
        Me.SplitContainer3.Panel1.ResumeLayout(False)
        Me.SplitContainer3.Panel2.ResumeLayout(False)
        Me.SplitContainer3.Panel2.PerformLayout()
        Me.SplitContainer3.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents Spc As System.Windows.Forms.SplitContainer
    Friend WithEvents grbMessage As System.Windows.Forms.GroupBox
    Friend WithEvents rtb As System.Windows.Forms.RichTextBox
    Friend WithEvents tbMesages As System.Windows.Forms.TabControl
    Friend WithEvents tbErrors As System.Windows.Forms.TabPage
    Friend WithEvents SplitContainer2 As System.Windows.Forms.SplitContainer
    Friend WithEvents txtErrors As System.Windows.Forms.TextBox
    Friend WithEvents tbWarnings As System.Windows.Forms.TabPage
    Friend WithEvents SplitContainer3 As System.Windows.Forms.SplitContainer
    Friend WithEvents lstWarning As System.Windows.Forms.CheckedListBox
    Friend WithEvents txtWarnings As System.Windows.Forms.TextBox
    Friend WithEvents lstTranError As System.Windows.Forms.ListBox
End Class
