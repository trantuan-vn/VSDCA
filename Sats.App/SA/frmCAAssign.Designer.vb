<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCAAssign
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
        Me.btnOk = New System.Windows.Forms.Button
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.lbCaption3 = New System.Windows.Forms.Label
        Me.btnCancel = New System.Windows.Forms.Button
        Me.grbGroupUsers3 = New System.Windows.Forms.GroupBox
        Me.btnRemoveAll = New System.Windows.Forms.Button
        Me.btnRemove = New System.Windows.Forms.Button
        Me.btnAdd = New System.Windows.Forms.Button
        Me.btnAddAll = New System.Windows.Forms.Button
        Me.lstUserInGroup = New System.Windows.Forms.ListBox
        Me.lstUserNoGroup = New System.Windows.Forms.ListBox
        Me.lbUsersInGroup3 = New System.Windows.Forms.Label
        Me.lbUsersNoGroup3 = New System.Windows.Forms.Label
        Me.Panel1.SuspendLayout()
        Me.grbGroupUsers3.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(324, 397)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(86, 30)
        Me.btnOk.TabIndex = 13
        Me.btnOk.Tag = "btnOk"
        Me.btnOk.Text = "btnOk"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.SystemColors.Desktop
        Me.Panel1.Controls.Add(Me.lbCaption3)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(500, 56)
        Me.Panel1.TabIndex = 11
        '
        'lbCaption3
        '
        Me.lbCaption3.AutoSize = True
        Me.lbCaption3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbCaption3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lbCaption3.Location = New System.Drawing.Point(23, 20)
        Me.lbCaption3.Name = "lbCaption3"
        Me.lbCaption3.Size = New System.Drawing.Size(60, 13)
        Me.lbCaption3.TabIndex = 0
        Me.lbCaption3.Tag = "lbCaption3"
        Me.lbCaption3.Text = "lbCaption"
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(416, 397)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(82, 30)
        Me.btnCancel.TabIndex = 14
        Me.btnCancel.Tag = "btnCancel"
        Me.btnCancel.Text = "btnCancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'grbGroupUsers3
        '
        Me.grbGroupUsers3.Controls.Add(Me.btnRemoveAll)
        Me.grbGroupUsers3.Controls.Add(Me.btnRemove)
        Me.grbGroupUsers3.Controls.Add(Me.btnAdd)
        Me.grbGroupUsers3.Controls.Add(Me.btnAddAll)
        Me.grbGroupUsers3.Controls.Add(Me.lstUserInGroup)
        Me.grbGroupUsers3.Controls.Add(Me.lstUserNoGroup)
        Me.grbGroupUsers3.Controls.Add(Me.lbUsersInGroup3)
        Me.grbGroupUsers3.Controls.Add(Me.lbUsersNoGroup3)
        Me.grbGroupUsers3.Location = New System.Drawing.Point(2, 66)
        Me.grbGroupUsers3.Name = "grbGroupUsers3"
        Me.grbGroupUsers3.Size = New System.Drawing.Size(496, 320)
        Me.grbGroupUsers3.TabIndex = 12
        Me.grbGroupUsers3.TabStop = False
        Me.grbGroupUsers3.Tag = "grbGroupUsers3"
        Me.grbGroupUsers3.Text = "grbGroupUsers3"
        '
        'btnRemoveAll
        '
        Me.btnRemoveAll.Font = New System.Drawing.Font("Webdings", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.btnRemoveAll.Location = New System.Drawing.Point(241, 190)
        Me.btnRemoveAll.Name = "btnRemoveAll"
        Me.btnRemoveAll.Size = New System.Drawing.Size(30, 30)
        Me.btnRemoveAll.TabIndex = 7
        Me.btnRemoveAll.Tag = "btnRemoveAll"
        Me.btnRemoveAll.Text = "7"
        Me.btnRemoveAll.UseVisualStyleBackColor = True
        '
        'btnRemove
        '
        Me.btnRemove.Font = New System.Drawing.Font("Webdings", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.btnRemove.Location = New System.Drawing.Point(241, 153)
        Me.btnRemove.Name = "btnRemove"
        Me.btnRemove.Size = New System.Drawing.Size(30, 30)
        Me.btnRemove.TabIndex = 6
        Me.btnRemove.Text = "3"
        Me.btnRemove.UseVisualStyleBackColor = True
        '
        'btnAdd
        '
        Me.btnAdd.Font = New System.Drawing.Font("Webdings", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.btnAdd.Location = New System.Drawing.Point(241, 116)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(30, 30)
        Me.btnAdd.TabIndex = 5
        Me.btnAdd.Tag = "btnAdd"
        Me.btnAdd.Text = "4"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'btnAddAll
        '
        Me.btnAddAll.Font = New System.Drawing.Font("Webdings", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.btnAddAll.Location = New System.Drawing.Point(241, 79)
        Me.btnAddAll.Name = "btnAddAll"
        Me.btnAddAll.Size = New System.Drawing.Size(30, 30)
        Me.btnAddAll.TabIndex = 4
        Me.btnAddAll.Text = "8"
        Me.btnAddAll.UseVisualStyleBackColor = True
        '
        'lstUserInGroup
        '
        Me.lstUserInGroup.FormattingEnabled = True
        Me.lstUserInGroup.Location = New System.Drawing.Point(277, 44)
        Me.lstUserInGroup.Name = "lstUserInGroup"
        Me.lstUserInGroup.Size = New System.Drawing.Size(213, 264)
        Me.lstUserInGroup.TabIndex = 3
        Me.lstUserInGroup.Tag = "lbUserInGroup"
        '
        'lstUserNoGroup
        '
        Me.lstUserNoGroup.FormattingEnabled = True
        Me.lstUserNoGroup.Location = New System.Drawing.Point(14, 44)
        Me.lstUserNoGroup.Name = "lstUserNoGroup"
        Me.lstUserNoGroup.Size = New System.Drawing.Size(221, 264)
        Me.lstUserNoGroup.TabIndex = 2
        Me.lstUserNoGroup.Tag = "lbUserNoGroup"
        '
        'lbUsersInGroup3
        '
        Me.lbUsersInGroup3.AllowDrop = True
        Me.lbUsersInGroup3.AutoSize = True
        Me.lbUsersInGroup3.Location = New System.Drawing.Point(274, 28)
        Me.lbUsersInGroup3.Name = "lbUsersInGroup3"
        Me.lbUsersInGroup3.Size = New System.Drawing.Size(80, 13)
        Me.lbUsersInGroup3.TabIndex = 1
        Me.lbUsersInGroup3.Tag = "lbUsersInGroup3"
        Me.lbUsersInGroup3.Text = "lbUsersInGroup"
        '
        'lbUsersNoGroup3
        '
        Me.lbUsersNoGroup3.AutoSize = True
        Me.lbUsersNoGroup3.Location = New System.Drawing.Point(11, 28)
        Me.lbUsersNoGroup3.Name = "lbUsersNoGroup3"
        Me.lbUsersNoGroup3.Size = New System.Drawing.Size(85, 13)
        Me.lbUsersNoGroup3.TabIndex = 0
        Me.lbUsersNoGroup3.Tag = "lbUsersNoGroup3"
        Me.lbUsersNoGroup3.Text = "lbUsersNoGroup"
        '
        'frmCAAssign
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(500, 431)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.grbGroupUsers3)
        Me.Name = "frmCAAssign"
        Me.Text = "frmCAAssign"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.grbGroupUsers3.ResumeLayout(False)
        Me.grbGroupUsers3.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents lbCaption3 As System.Windows.Forms.Label
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents grbGroupUsers3 As System.Windows.Forms.GroupBox
    Friend WithEvents btnRemoveAll As System.Windows.Forms.Button
    Friend WithEvents btnRemove As System.Windows.Forms.Button
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents btnAddAll As System.Windows.Forms.Button
    Friend WithEvents lstUserInGroup As System.Windows.Forms.ListBox
    Friend WithEvents lstUserNoGroup As System.Windows.Forms.ListBox
    Friend WithEvents lbUsersInGroup3 As System.Windows.Forms.Label
    Friend WithEvents lbUsersNoGroup3 As System.Windows.Forms.Label
End Class
