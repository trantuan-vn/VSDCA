<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSelectBranch
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSelectBranch))
        Me.grbBranch = New System.Windows.Forms.GroupBox
        Me.lblBranch = New System.Windows.Forms.Label
        Me.cboBranch = New Sats.AppCore.ComboBoxEx
        Me.btnOK = New System.Windows.Forms.Button
        Me.grbBranch.SuspendLayout()
        Me.SuspendLayout()
        '
        'grbBranch
        '
        Me.grbBranch.Controls.Add(Me.btnOK)
        Me.grbBranch.Controls.Add(Me.cboBranch)
        Me.grbBranch.Controls.Add(Me.lblBranch)
        Me.grbBranch.Location = New System.Drawing.Point(12, 12)
        Me.grbBranch.Name = "grbBranch"
        Me.grbBranch.Size = New System.Drawing.Size(391, 72)
        Me.grbBranch.TabIndex = 0
        Me.grbBranch.TabStop = False
        Me.grbBranch.Text = "Chi nhánh in báo cáo"
        '
        'lblBranch
        '
        Me.lblBranch.AutoSize = True
        Me.lblBranch.Location = New System.Drawing.Point(6, 33)
        Me.lblBranch.Name = "lblBranch"
        Me.lblBranch.Size = New System.Drawing.Size(55, 13)
        Me.lblBranch.TabIndex = 0
        Me.lblBranch.Text = "Chi nhánh"
        '
        'cboBranch
        '
        Me.cboBranch.DisplayMember = "DISPLAY"
        Me.cboBranch.FormattingEnabled = True
        Me.cboBranch.Location = New System.Drawing.Point(102, 30)
        Me.cboBranch.Name = "cboBranch"
        Me.cboBranch.Size = New System.Drawing.Size(198, 21)
        Me.cboBranch.TabIndex = 1
        Me.cboBranch.ValueMember = "VALUE"
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(306, 26)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 26)
        Me.btnOK.TabIndex = 2
        Me.btnOK.Text = "Chấp nhận"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'frmSelectBranch
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(412, 96)
        Me.Controls.Add(Me.grbBranch)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmSelectBranch"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Chọn chi nhánh in báo cáo"
        Me.grbBranch.ResumeLayout(False)
        Me.grbBranch.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grbBranch As System.Windows.Forms.GroupBox
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents cboBranch As Sats.AppCore.ComboBoxEx
    Friend WithEvents lblBranch As System.Windows.Forms.Label
End Class
