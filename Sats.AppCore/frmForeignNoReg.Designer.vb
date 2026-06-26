<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmForeignNoReg
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
        Me.grpInfo = New System.Windows.Forms.GroupBox
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnOk = New System.Windows.Forms.Button
        Me.btnGenerate = New System.Windows.Forms.Button
        Me.txtForeignNo = New System.Windows.Forms.TextBox
        Me.lbForeignNo = New System.Windows.Forms.Label
        Me.grpInfo.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpInfo
        '
        Me.grpInfo.Controls.Add(Me.btnExit)
        Me.grpInfo.Controls.Add(Me.btnOk)
        Me.grpInfo.Controls.Add(Me.btnGenerate)
        Me.grpInfo.Controls.Add(Me.txtForeignNo)
        Me.grpInfo.Controls.Add(Me.lbForeignNo)
        Me.grpInfo.Location = New System.Drawing.Point(7, 4)
        Me.grpInfo.Name = "grpInfo"
        Me.grpInfo.Size = New System.Drawing.Size(399, 133)
        Me.grpInfo.TabIndex = 0
        Me.grpInfo.TabStop = False
        Me.grpInfo.Tag = "grpInfo"
        Me.grpInfo.Text = "grpInfo"
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(243, 84)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(75, 23)
        Me.btnExit.TabIndex = 4
        Me.btnExit.Tag = "btnExit"
        Me.btnExit.Text = "btnExit"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(158, 84)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(75, 23)
        Me.btnOk.TabIndex = 3
        Me.btnOk.Tag = "btnOk"
        Me.btnOk.Text = "btnOk"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'btnGenerate
        '
        Me.btnGenerate.Location = New System.Drawing.Point(74, 84)
        Me.btnGenerate.Name = "btnGenerate"
        Me.btnGenerate.Size = New System.Drawing.Size(75, 23)
        Me.btnGenerate.TabIndex = 2
        Me.btnGenerate.Tag = "btnGenerate"
        Me.btnGenerate.Text = "btnGenerate"
        Me.btnGenerate.UseVisualStyleBackColor = True
        '
        'txtForeignNo
        '
        Me.txtForeignNo.Location = New System.Drawing.Point(158, 33)
        Me.txtForeignNo.Name = "txtForeignNo"
        Me.txtForeignNo.Size = New System.Drawing.Size(175, 20)
        Me.txtForeignNo.TabIndex = 1
        Me.txtForeignNo.Tag = "FOREIGNNO"
        '
        'lbForeignNo
        '
        Me.lbForeignNo.AutoSize = True
        Me.lbForeignNo.Location = New System.Drawing.Point(51, 36)
        Me.lbForeignNo.Name = "lbForeignNo"
        Me.lbForeignNo.Size = New System.Drawing.Size(64, 13)
        Me.lbForeignNo.TabIndex = 0
        Me.lbForeignNo.Tag = "lbForeignNo"
        Me.lbForeignNo.Text = "lbForeignNo"
        '
        'frmForeignNoReg
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(414, 144)
        Me.Controls.Add(Me.grpInfo)
        Me.Name = "frmForeignNoReg"
        Me.Text = "frmForeignNoReg"
        Me.grpInfo.ResumeLayout(False)
        Me.grpInfo.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpInfo As System.Windows.Forms.GroupBox
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents btnGenerate As System.Windows.Forms.Button
    Friend WithEvents txtForeignNo As System.Windows.Forms.TextBox
    Friend WithEvents lbForeignNo As System.Windows.Forms.Label
End Class
