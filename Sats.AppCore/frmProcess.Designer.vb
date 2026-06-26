<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmProcess
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
        Me.lbl = New System.Windows.Forms.Label
        Me.prb = New System.Windows.Forms.ProgressBar
        Me.SuspendLayout()
        '
        'lbl
        '
        Me.lbl.AutoSize = True
        Me.lbl.Location = New System.Drawing.Point(12, 9)
        Me.lbl.Name = "lbl"
        Me.lbl.Size = New System.Drawing.Size(147, 13)
        Me.lbl.TabIndex = 0
        Me.lbl.Text = "Đang lấy dữ liệu từ máy chủ..."
        '
        'prb
        '
        Me.prb.Location = New System.Drawing.Point(12, 32)
        Me.prb.Name = "prb"
        Me.prb.Size = New System.Drawing.Size(326, 23)
        Me.prb.Step = 5
        Me.prb.Style = System.Windows.Forms.ProgressBarStyle.Marquee
        Me.prb.TabIndex = 1
        '
        'frmProcess
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Gainsboro
        Me.ClientSize = New System.Drawing.Size(350, 67)
        Me.ControlBox = False
        Me.Controls.Add(Me.prb)
        Me.Controls.Add(Me.lbl)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmProcess"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lbl As System.Windows.Forms.Label
    Friend WithEvents prb As System.Windows.Forms.ProgressBar

End Class
