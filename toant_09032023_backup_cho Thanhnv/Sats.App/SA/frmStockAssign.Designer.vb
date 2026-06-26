<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmStockAssign
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
        Me.btnRemove = New System.Windows.Forms.Button
        Me.btnAddAll = New System.Windows.Forms.Button
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.lbCaption = New System.Windows.Forms.Label
        Me.lstUserNoGroup = New System.Windows.Forms.ListBox
        Me.btnOk = New System.Windows.Forms.Button
        Me.lstUserInGroup = New System.Windows.Forms.ListBox
        Me.lbUsersInGroup1 = New System.Windows.Forms.Label
        Me.btnAdd = New System.Windows.Forms.Button
        Me.btnRemoveAll = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.grbGroupUsers1 = New System.Windows.Forms.GroupBox
        Me.lbUsersNoGroup1 = New System.Windows.Forms.Label
        Me.lstBrID = New System.Windows.Forms.ListBox
        Me.lbBrID = New System.Windows.Forms.Label
        Me.Panel1.SuspendLayout()
        Me.grbGroupUsers1.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnRemove
        '
        Me.btnRemove.Font = New System.Drawing.Font("Webdings", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.btnRemove.Location = New System.Drawing.Point(448, 148)
        Me.btnRemove.Name = "btnRemove"
        Me.btnRemove.Size = New System.Drawing.Size(30, 30)
        Me.btnRemove.TabIndex = 6
        Me.btnRemove.Text = "3"
        Me.btnRemove.UseVisualStyleBackColor = True
        '
        'btnAddAll
        '
        Me.btnAddAll.Font = New System.Drawing.Font("Webdings", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.btnAddAll.Location = New System.Drawing.Point(448, 74)
        Me.btnAddAll.Name = "btnAddAll"
        Me.btnAddAll.Size = New System.Drawing.Size(30, 30)
        Me.btnAddAll.TabIndex = 4
        Me.btnAddAll.Text = "8"
        Me.btnAddAll.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.SystemColors.Desktop
        Me.Panel1.Controls.Add(Me.lbCaption)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(717, 49)
        Me.Panel1.TabIndex = 11
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
        'lstUserNoGroup
        '
        Me.lstUserNoGroup.FormattingEnabled = True
        Me.lstUserNoGroup.Location = New System.Drawing.Point(221, 39)
        Me.lstUserNoGroup.Name = "lstUserNoGroup"
        Me.lstUserNoGroup.Size = New System.Drawing.Size(221, 264)
        Me.lstUserNoGroup.TabIndex = 2
        Me.lstUserNoGroup.Tag = "lbUserNoGroup"
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(320, 381)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(86, 30)
        Me.btnOk.TabIndex = 13
        Me.btnOk.Tag = "btnOk"
        Me.btnOk.Text = "btnOk"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'lstUserInGroup
        '
        Me.lstUserInGroup.FormattingEnabled = True
        Me.lstUserInGroup.Location = New System.Drawing.Point(484, 39)
        Me.lstUserInGroup.Name = "lstUserInGroup"
        Me.lstUserInGroup.Size = New System.Drawing.Size(213, 264)
        Me.lstUserInGroup.TabIndex = 3
        Me.lstUserInGroup.Tag = "lbUserInGroup"
        '
        'lbUsersInGroup1
        '
        Me.lbUsersInGroup1.AllowDrop = True
        Me.lbUsersInGroup1.AutoSize = True
        Me.lbUsersInGroup1.Location = New System.Drawing.Point(481, 23)
        Me.lbUsersInGroup1.Name = "lbUsersInGroup1"
        Me.lbUsersInGroup1.Size = New System.Drawing.Size(80, 13)
        Me.lbUsersInGroup1.TabIndex = 1
        Me.lbUsersInGroup1.Tag = "lbUsersInGroup1"
        Me.lbUsersInGroup1.Text = "lbUsersInGroup"
        '
        'btnAdd
        '
        Me.btnAdd.Font = New System.Drawing.Font("Webdings", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.btnAdd.Location = New System.Drawing.Point(448, 111)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(30, 30)
        Me.btnAdd.TabIndex = 5
        Me.btnAdd.Tag = "btnAdd"
        Me.btnAdd.Text = "4"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'btnRemoveAll
        '
        Me.btnRemoveAll.Font = New System.Drawing.Font("Webdings", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.btnRemoveAll.Location = New System.Drawing.Point(448, 185)
        Me.btnRemoveAll.Name = "btnRemoveAll"
        Me.btnRemoveAll.Size = New System.Drawing.Size(30, 30)
        Me.btnRemoveAll.TabIndex = 7
        Me.btnRemoveAll.Tag = "btnRemoveAll"
        Me.btnRemoveAll.Text = "7"
        Me.btnRemoveAll.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(412, 381)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(82, 30)
        Me.btnCancel.TabIndex = 14
        Me.btnCancel.Tag = "btnCancel"
        Me.btnCancel.Text = "btnCancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'grbGroupUsers1
        '
        Me.grbGroupUsers1.Controls.Add(Me.lbBrID)
        Me.grbGroupUsers1.Controls.Add(Me.lstBrID)
        Me.grbGroupUsers1.Controls.Add(Me.btnRemoveAll)
        Me.grbGroupUsers1.Controls.Add(Me.btnRemove)
        Me.grbGroupUsers1.Controls.Add(Me.btnAdd)
        Me.grbGroupUsers1.Controls.Add(Me.btnAddAll)
        Me.grbGroupUsers1.Controls.Add(Me.lstUserInGroup)
        Me.grbGroupUsers1.Controls.Add(Me.lstUserNoGroup)
        Me.grbGroupUsers1.Controls.Add(Me.lbUsersInGroup1)
        Me.grbGroupUsers1.Controls.Add(Me.lbUsersNoGroup1)
        Me.grbGroupUsers1.Location = New System.Drawing.Point(2, 55)
        Me.grbGroupUsers1.Name = "grbGroupUsers1"
        Me.grbGroupUsers1.Size = New System.Drawing.Size(703, 320)
        Me.grbGroupUsers1.TabIndex = 12
        Me.grbGroupUsers1.TabStop = False
        Me.grbGroupUsers1.Tag = "grbGroupUsers1"
        Me.grbGroupUsers1.Text = "grbGroupUsers"
        '
        'lbUsersNoGroup1
        '
        Me.lbUsersNoGroup1.AutoSize = True
        Me.lbUsersNoGroup1.Location = New System.Drawing.Point(218, 23)
        Me.lbUsersNoGroup1.Name = "lbUsersNoGroup1"
        Me.lbUsersNoGroup1.Size = New System.Drawing.Size(85, 13)
        Me.lbUsersNoGroup1.TabIndex = 0
        Me.lbUsersNoGroup1.Tag = "lbUsersNoGroup1"
        Me.lbUsersNoGroup1.Text = "lbUsersNoGroup"
        '
        'lstBrID
        '
        Me.lstBrID.FormattingEnabled = True
        Me.lstBrID.Location = New System.Drawing.Point(6, 39)
        Me.lstBrID.Name = "lstBrID"
        Me.lstBrID.Size = New System.Drawing.Size(209, 264)
        Me.lstBrID.TabIndex = 1
        Me.lstBrID.Tag = "lbBrID"
        '
        'lbBrID
        '
        Me.lbBrID.AutoSize = True
        Me.lbBrID.Location = New System.Drawing.Point(6, 23)
        Me.lbBrID.Name = "lbBrID"
        Me.lbBrID.Size = New System.Drawing.Size(36, 13)
        Me.lbBrID.TabIndex = 9
        Me.lbBrID.Tag = "lbBrID"
        Me.lbBrID.Text = "lbBrID"
        '
        'frmStockAssign
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(717, 415)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.grbGroupUsers1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmStockAssign"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "frmStockAssign"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.grbGroupUsers1.ResumeLayout(False)
        Me.grbGroupUsers1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnRemove As System.Windows.Forms.Button
    Friend WithEvents btnAddAll As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents lbCaption As System.Windows.Forms.Label
    Friend WithEvents lstUserNoGroup As System.Windows.Forms.ListBox
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents lstUserInGroup As System.Windows.Forms.ListBox
    Friend WithEvents lbUsersInGroup1 As System.Windows.Forms.Label
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents btnRemoveAll As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents grbGroupUsers1 As System.Windows.Forms.GroupBox
    Friend WithEvents lbUsersNoGroup1 As System.Windows.Forms.Label
    Friend WithEvents lstBrID As System.Windows.Forms.ListBox
    Friend WithEvents lbBrID As System.Windows.Forms.Label
End Class
