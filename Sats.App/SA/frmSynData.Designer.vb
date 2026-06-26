<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSynData
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
        Me.grbSynData = New System.Windows.Forms.GroupBox
        Me.clbData = New System.Windows.Forms.CheckedListBox
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnOK = New System.Windows.Forms.Button
        Me.grbSynData.SuspendLayout()
        Me.SuspendLayout()
        '
        'grbSynData
        '
        Me.grbSynData.Controls.Add(Me.clbData)
        Me.grbSynData.Location = New System.Drawing.Point(7, 4)
        Me.grbSynData.Name = "grbSynData"
        Me.grbSynData.Size = New System.Drawing.Size(266, 298)
        Me.grbSynData.TabIndex = 8
        Me.grbSynData.TabStop = False
        Me.grbSynData.Tag = "grbSynData"
        Me.grbSynData.Text = "grbSynData"
        '
        'clbData
        '
        Me.clbData.CheckOnClick = True
        Me.clbData.FormattingEnabled = True
        Me.clbData.Location = New System.Drawing.Point(6, 14)
        Me.clbData.Name = "clbData"
        Me.clbData.Size = New System.Drawing.Size(254, 274)
        Me.clbData.TabIndex = 5
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(202, 308)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(71, 28)
        Me.btnCancel.TabIndex = 7
        Me.btnCancel.Text = "btnCancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(121, 308)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 28)
        Me.btnOK.TabIndex = 6
        Me.btnOK.Text = "btnOk"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'frmSynData
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(279, 340)
        Me.Controls.Add(Me.grbSynData)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmSynData"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "frmSynData"
        Me.grbSynData.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grbSynData As System.Windows.Forms.GroupBox
    Friend WithEvents clbData As System.Windows.Forms.CheckedListBox
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
End Class
