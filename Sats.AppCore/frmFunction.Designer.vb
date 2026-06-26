<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmFunction
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
        Me.components = New System.ComponentModel.Container
        Me.grpValue = New System.Windows.Forms.GroupBox
        Me.lstValue = New System.Windows.Forms.ListBox
        Me.txtEditer = New System.Windows.Forms.TextBox
        Me.btnCancel = New System.Windows.Forms.Button
        Me.grpEdit = New System.Windows.Forms.GroupBox
        Me.grpFunc = New System.Windows.Forms.GroupBox
        Me.stvFunc = New Xceed.SmartUI.Controls.TreeView.SmartTreeView(Me.components)
        Me.Node1 = New Xceed.SmartUI.Controls.TreeView.Node("Function")
        Me.btnOK = New System.Windows.Forms.Button
        Me.btnCheck = New System.Windows.Forms.Button
        Me.grpValue.SuspendLayout()
        Me.grpEdit.SuspendLayout()
        Me.grpFunc.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpValue
        '
        Me.grpValue.Controls.Add(Me.lstValue)
        Me.grpValue.Location = New System.Drawing.Point(6, 9)
        Me.grpValue.Name = "grpValue"
        Me.grpValue.Size = New System.Drawing.Size(222, 187)
        Me.grpValue.TabIndex = 16
        Me.grpValue.TabStop = False
        Me.grpValue.Text = "grpValue"
        '
        'lstValue
        '
        Me.lstValue.FormattingEnabled = True
        Me.lstValue.Location = New System.Drawing.Point(6, 21)
        Me.lstValue.Name = "lstValue"
        Me.lstValue.Size = New System.Drawing.Size(210, 160)
        Me.lstValue.TabIndex = 0
        '
        'txtEditer
        '
        Me.txtEditer.Location = New System.Drawing.Point(6, 19)
        Me.txtEditer.Multiline = True
        Me.txtEditer.Name = "txtEditer"
        Me.txtEditer.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtEditer.Size = New System.Drawing.Size(591, 119)
        Me.txtEditer.TabIndex = 0
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(615, 82)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(88, 29)
        Me.btnCancel.TabIndex = 15
        Me.btnCancel.Text = "btnCancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'grpEdit
        '
        Me.grpEdit.Controls.Add(Me.txtEditer)
        Me.grpEdit.Location = New System.Drawing.Point(6, 202)
        Me.grpEdit.Name = "grpEdit"
        Me.grpEdit.Size = New System.Drawing.Size(603, 144)
        Me.grpEdit.TabIndex = 13
        Me.grpEdit.TabStop = False
        Me.grpEdit.Text = "grpEdit"
        '
        'grpFunc
        '
        Me.grpFunc.Controls.Add(Me.stvFunc)
        Me.grpFunc.Location = New System.Drawing.Point(234, 9)
        Me.grpFunc.Name = "grpFunc"
        Me.grpFunc.Size = New System.Drawing.Size(375, 187)
        Me.grpFunc.TabIndex = 11
        Me.grpFunc.TabStop = False
        Me.grpFunc.Text = "grpFunc"
        '
        'stvFunc
        '
        Me.stvFunc.Items.AddRange(New Xceed.SmartUI.SmartItem() {Me.Node1})
        Me.stvFunc.Location = New System.Drawing.Point(6, 19)
        Me.stvFunc.Name = "stvFunc"
        Me.stvFunc.Size = New System.Drawing.Size(363, 162)
        Me.stvFunc.TabIndex = 0
        Me.stvFunc.Text = "stvFunc"
        '
        'Node1
        '
        Me.Node1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Node1.Text = "Function"
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(615, 12)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(88, 29)
        Me.btnOK.TabIndex = 10
        Me.btnOK.Text = "btnOK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnCheck
        '
        Me.btnCheck.Location = New System.Drawing.Point(615, 47)
        Me.btnCheck.Name = "btnCheck"
        Me.btnCheck.Size = New System.Drawing.Size(88, 29)
        Me.btnCheck.TabIndex = 17
        Me.btnCheck.Text = "btnCheck"
        Me.btnCheck.UseVisualStyleBackColor = True
        '
        'frmFunction
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.ClientSize = New System.Drawing.Size(706, 352)
        Me.Controls.Add(Me.btnCheck)
        Me.Controls.Add(Me.grpValue)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.grpEdit)
        Me.Controls.Add(Me.grpFunc)
        Me.Controls.Add(Me.btnOK)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmFunction"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "frmFunction"
        Me.grpValue.ResumeLayout(False)
        Me.grpEdit.ResumeLayout(False)
        Me.grpEdit.PerformLayout()
        Me.grpFunc.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpValue As System.Windows.Forms.GroupBox
    Friend WithEvents lstValue As System.Windows.Forms.ListBox
    Friend WithEvents txtEditer As System.Windows.Forms.TextBox
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents grpEdit As System.Windows.Forms.GroupBox
    Friend WithEvents grpFunc As System.Windows.Forms.GroupBox
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents stvFunc As Xceed.SmartUI.Controls.TreeView.SmartTreeView
    Friend WithEvents Node1 As Xceed.SmartUI.Controls.TreeView.Node
    Friend WithEvents btnCheck As System.Windows.Forms.Button
End Class
