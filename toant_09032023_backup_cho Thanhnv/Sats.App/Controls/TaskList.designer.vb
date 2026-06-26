<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TaskList
    Inherits Sats.WinFormsUI.Docking.DockContent
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
        Me.cbTaskList = New System.Windows.Forms.ComboBox
        Me.txtTaskContent = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'cbTaskList
        '
        Me.cbTaskList.FormattingEnabled = True
        Me.cbTaskList.Location = New System.Drawing.Point(-1, -1)
        Me.cbTaskList.Name = "cbTaskList"
        Me.cbTaskList.Size = New System.Drawing.Size(660, 21)
        Me.cbTaskList.TabIndex = 0
        '
        'txtTaskContent
        '
        Me.txtTaskContent.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtTaskContent.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTaskContent.Location = New System.Drawing.Point(0, 0)
        Me.txtTaskContent.Multiline = True
        Me.txtTaskContent.Name = "txtTaskContent"
        Me.txtTaskContent.ReadOnly = True
        Me.txtTaskContent.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtTaskContent.Size = New System.Drawing.Size(181, 411)
        Me.txtTaskContent.TabIndex = 1
        '
        'TaskList
        '
        Me.ClientSize = New System.Drawing.Size(181, 411)
        Me.Controls.Add(Me.txtTaskContent)
        Me.Controls.Add(Me.cbTaskList)
        Me.DockAreas = CType(((Sats.WinFormsUI.Docking.DockAreas.DockLeft Or Sats.WinFormsUI.Docking.DockAreas.DockRight) _
                    Or Sats.WinFormsUI.Docking.DockAreas.DockBottom), Sats.WinFormsUI.Docking.DockAreas)
        Me.HideOnClose = True
        Me.Name = "TaskList"
        Me.TabText = "frmTaskList"
        Me.Text = "Chi tiết giao dịch :  "
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cbTaskList As System.Windows.Forms.ComboBox
    Friend WithEvents txtTaskContent As System.Windows.Forms.TextBox

End Class
