<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmExportParameter
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
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnOk = New System.Windows.Forms.Button
        Me.grbInfo = New System.Windows.Forms.GroupBox
        Me.txtRowFrom = New System.Windows.Forms.TextBox
        Me.lbRowFrom = New System.Windows.Forms.Label
        Me.btnBrowse = New System.Windows.Forms.Button
        Me.txtRowTo = New System.Windows.Forms.TextBox
        Me.lbRowTo = New System.Windows.Forms.Label
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.lbCaption = New System.Windows.Forms.Label
        Me.grbExportType = New System.Windows.Forms.GroupBox
        Me.rbDbExport = New System.Windows.Forms.RadioButton
        Me.rbGridExport = New System.Windows.Forms.RadioButton
        Me.grbInfo.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.grbExportType.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(239, 239)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 35)
        Me.btnCancel.TabIndex = 15
        Me.btnCancel.Tag = "btnCancel"
        Me.btnCancel.Text = "Thoát"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(154, 239)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(79, 35)
        Me.btnOk.TabIndex = 14
        Me.btnOk.Tag = "btnOk"
        Me.btnOk.Text = "Kết xuất"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'grbInfo
        '
        Me.grbInfo.Controls.Add(Me.txtRowFrom)
        Me.grbInfo.Controls.Add(Me.lbRowFrom)
        Me.grbInfo.Controls.Add(Me.btnBrowse)
        Me.grbInfo.Controls.Add(Me.txtRowTo)
        Me.grbInfo.Controls.Add(Me.lbRowTo)
        Me.grbInfo.Location = New System.Drawing.Point(14, 69)
        Me.grbInfo.Name = "grbInfo"
        Me.grbInfo.Size = New System.Drawing.Size(415, 89)
        Me.grbInfo.TabIndex = 13
        Me.grbInfo.TabStop = False
        Me.grbInfo.Text = "Tham số kết xuất dữ liệu"
        '
        'txtRowFrom
        '
        Me.txtRowFrom.Location = New System.Drawing.Point(184, 19)
        Me.txtRowFrom.Name = "txtRowFrom"
        Me.txtRowFrom.Size = New System.Drawing.Size(124, 20)
        Me.txtRowFrom.TabIndex = 8
        Me.txtRowFrom.Tag = "txtRowFrom"
        '
        'lbRowFrom
        '
        Me.lbRowFrom.AutoSize = True
        Me.lbRowFrom.Location = New System.Drawing.Point(105, 22)
        Me.lbRowFrom.Name = "lbRowFrom"
        Me.lbRowFrom.Size = New System.Drawing.Size(47, 13)
        Me.lbRowFrom.TabIndex = 7
        Me.lbRowFrom.Tag = "lbRowFrom"
        Me.lbRowFrom.Text = "Từ dòng"
        '
        'btnBrowse
        '
        Me.btnBrowse.Location = New System.Drawing.Point(157, 122)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(75, 23)
        Me.btnBrowse.TabIndex = 6
        Me.btnBrowse.Tag = "btnBrowse"
        Me.btnBrowse.Text = "Browse"
        Me.btnBrowse.UseVisualStyleBackColor = True
        '
        'txtRowTo
        '
        Me.txtRowTo.Location = New System.Drawing.Point(184, 47)
        Me.txtRowTo.Name = "txtRowTo"
        Me.txtRowTo.Size = New System.Drawing.Size(124, 20)
        Me.txtRowTo.TabIndex = 5
        Me.txtRowTo.Tag = "txtRowTo"
        '
        'lbRowTo
        '
        Me.lbRowTo.AutoSize = True
        Me.lbRowTo.Location = New System.Drawing.Point(106, 50)
        Me.lbRowTo.Name = "lbRowTo"
        Me.lbRowTo.Size = New System.Drawing.Size(54, 13)
        Me.lbRowTo.TabIndex = 4
        Me.lbRowTo.Tag = "lbRowTo"
        Me.lbRowTo.Text = "Đến dòng"
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.SystemColors.Desktop
        Me.Panel1.Controls.Add(Me.lbCaption)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(469, 56)
        Me.Panel1.TabIndex = 12
        '
        'lbCaption
        '
        Me.lbCaption.AutoSize = True
        Me.lbCaption.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbCaption.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lbCaption.Location = New System.Drawing.Point(23, 20)
        Me.lbCaption.Name = "lbCaption"
        Me.lbCaption.Size = New System.Drawing.Size(96, 13)
        Me.lbCaption.TabIndex = 0
        Me.lbCaption.Tag = "lbCaption"
        Me.lbCaption.Text = "Kết xuất dữ liệu"
        '
        'grbExportType
        '
        Me.grbExportType.Controls.Add(Me.rbDbExport)
        Me.grbExportType.Controls.Add(Me.rbGridExport)
        Me.grbExportType.Controls.Add(Me.grbInfo)
        Me.grbExportType.Location = New System.Drawing.Point(12, 62)
        Me.grbExportType.Name = "grbExportType"
        Me.grbExportType.Size = New System.Drawing.Size(443, 171)
        Me.grbExportType.TabIndex = 16
        Me.grbExportType.TabStop = False
        Me.grbExportType.Tag = "grbExportType"
        Me.grbExportType.Text = "Chọn hình thức kết xuất"
        '
        'rbDbExport
        '
        Me.rbDbExport.AutoSize = True
        Me.rbDbExport.Location = New System.Drawing.Point(14, 46)
        Me.rbDbExport.Name = "rbDbExport"
        Me.rbDbExport.Size = New System.Drawing.Size(139, 17)
        Me.rbDbExport.TabIndex = 15
        Me.rbDbExport.TabStop = True
        Me.rbDbExport.Tag = "rbDbExport"
        Me.rbDbExport.Text = "Kết xuất từ cơ sở dữ liệu"
        Me.rbDbExport.UseVisualStyleBackColor = True
        '
        'rbGridExport
        '
        Me.rbGridExport.AutoSize = True
        Me.rbGridExport.Location = New System.Drawing.Point(14, 23)
        Me.rbGridExport.Name = "rbGridExport"
        Me.rbGridExport.Size = New System.Drawing.Size(208, 17)
        Me.rbGridExport.TabIndex = 14
        Me.rbGridExport.TabStop = True
        Me.rbGridExport.Tag = "rbGridExport"
        Me.rbGridExport.Text = "Kết xuất từ lưới dữ liệu tìm kiếm hiện có"
        Me.rbGridExport.UseVisualStyleBackColor = True
        '
        'frmExportParameter
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(469, 280)
        Me.Controls.Add(Me.grbExportType)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "frmExportParameter"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Kết xuất dữ liệu"
        Me.grbInfo.ResumeLayout(False)
        Me.grbInfo.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.grbExportType.ResumeLayout(False)
        Me.grbExportType.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents grbInfo As System.Windows.Forms.GroupBox
    Friend WithEvents btnBrowse As System.Windows.Forms.Button
    Friend WithEvents txtRowTo As System.Windows.Forms.TextBox
    Friend WithEvents lbRowTo As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents lbCaption As System.Windows.Forms.Label
    Friend WithEvents txtRowFrom As System.Windows.Forms.TextBox
    Friend WithEvents lbRowFrom As System.Windows.Forms.Label
    Friend WithEvents grbExportType As System.Windows.Forms.GroupBox
    Friend WithEvents rbDbExport As System.Windows.Forms.RadioButton
    Friend WithEvents rbGridExport As System.Windows.Forms.RadioButton
End Class
