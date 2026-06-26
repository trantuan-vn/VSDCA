<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAddMFToTLTX
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
        Me.btnOk = New System.Windows.Forms.Button
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.lbCaption = New System.Windows.Forms.Label
        Me.btnRemoveAll = New System.Windows.Forms.Button
        Me.lbMF = New System.Windows.Forms.Label
        Me.btnAdd = New System.Windows.Forms.Button
        Me.txtMF = New System.Windows.Forms.TextBox
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnAddAll = New System.Windows.Forms.Button
        Me.grbMFToTLTXCD = New System.Windows.Forms.GroupBox
        Me.lstTLTXCDInMF = New System.Windows.Forms.ListBox
        Me.lstTLTXCDNoMF = New System.Windows.Forms.ListBox
        Me.lbTLTXCDInMF = New System.Windows.Forms.Label
        Me.lbTLTXCDNoMF = New System.Windows.Forms.Label
        Me.lbNote = New System.Windows.Forms.Label
        Me.Panel1.SuspendLayout()
        Me.grbMFToTLTXCD.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnRemove
        '
        Me.btnRemove.Font = New System.Drawing.Font("Webdings", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.btnRemove.Location = New System.Drawing.Point(313, 214)
        Me.btnRemove.Name = "btnRemove"
        Me.btnRemove.Size = New System.Drawing.Size(30, 30)
        Me.btnRemove.TabIndex = 6
        Me.btnRemove.Text = "3"
        Me.btnRemove.UseVisualStyleBackColor = True
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(500, 475)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(86, 30)
        Me.btnOk.TabIndex = 9
        Me.btnOk.Tag = "btnOk"
        Me.btnOk.Text = "btnOk"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.SystemColors.Desktop
        Me.Panel1.Controls.Add(Me.lbCaption)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(681, 56)
        Me.Panel1.TabIndex = 7
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
        'btnRemoveAll
        '
        Me.btnRemoveAll.Font = New System.Drawing.Font("Webdings", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.btnRemoveAll.Location = New System.Drawing.Point(313, 251)
        Me.btnRemoveAll.Name = "btnRemoveAll"
        Me.btnRemoveAll.Size = New System.Drawing.Size(30, 30)
        Me.btnRemoveAll.TabIndex = 7
        Me.btnRemoveAll.Tag = "btnRemoveAll"
        Me.btnRemoveAll.Text = "7"
        Me.btnRemoveAll.UseVisualStyleBackColor = True
        '
        'lbMF
        '
        Me.lbMF.AutoSize = True
        Me.lbMF.Location = New System.Drawing.Point(23, 66)
        Me.lbMF.Name = "lbMF"
        Me.lbMF.Size = New System.Drawing.Size(30, 13)
        Me.lbMF.TabIndex = 11
        Me.lbMF.Tag = "lbMF"
        Me.lbMF.Text = "lbMF"
        '
        'btnAdd
        '
        Me.btnAdd.Font = New System.Drawing.Font("Webdings", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.btnAdd.Location = New System.Drawing.Point(313, 177)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(30, 30)
        Me.btnAdd.TabIndex = 5
        Me.btnAdd.Tag = "btnAdd"
        Me.btnAdd.Text = "4"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'txtMF
        '
        Me.txtMF.Location = New System.Drawing.Point(123, 63)
        Me.txtMF.Name = "txtMF"
        Me.txtMF.Size = New System.Drawing.Size(99, 20)
        Me.txtMF.TabIndex = 12
        Me.txtMF.Tag = "GROUP"
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(588, 475)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(82, 30)
        Me.btnCancel.TabIndex = 10
        Me.btnCancel.Tag = "btnCancel"
        Me.btnCancel.Text = "btnCancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnAddAll
        '
        Me.btnAddAll.Font = New System.Drawing.Font("Webdings", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.btnAddAll.Location = New System.Drawing.Point(313, 140)
        Me.btnAddAll.Name = "btnAddAll"
        Me.btnAddAll.Size = New System.Drawing.Size(30, 30)
        Me.btnAddAll.TabIndex = 4
        Me.btnAddAll.Text = "8"
        Me.btnAddAll.UseVisualStyleBackColor = True
        '
        'grbMFToTLTXCD
        '
        Me.grbMFToTLTXCD.Controls.Add(Me.btnRemoveAll)
        Me.grbMFToTLTXCD.Controls.Add(Me.btnRemove)
        Me.grbMFToTLTXCD.Controls.Add(Me.btnAdd)
        Me.grbMFToTLTXCD.Controls.Add(Me.btnAddAll)
        Me.grbMFToTLTXCD.Controls.Add(Me.lstTLTXCDInMF)
        Me.grbMFToTLTXCD.Controls.Add(Me.lstTLTXCDNoMF)
        Me.grbMFToTLTXCD.Controls.Add(Me.lbTLTXCDInMF)
        Me.grbMFToTLTXCD.Controls.Add(Me.lbTLTXCDNoMF)
        Me.grbMFToTLTXCD.Location = New System.Drawing.Point(12, 89)
        Me.grbMFToTLTXCD.Name = "grbMFToTLTXCD"
        Me.grbMFToTLTXCD.Size = New System.Drawing.Size(658, 380)
        Me.grbMFToTLTXCD.TabIndex = 8
        Me.grbMFToTLTXCD.TabStop = False
        Me.grbMFToTLTXCD.Tag = "grbMFToTLTXCD"
        Me.grbMFToTLTXCD.Text = "grbMFToTLTXCD"
        '
        'lstTLTXCDInMF
        '
        Me.lstTLTXCDInMF.FormattingEnabled = True
        Me.lstTLTXCDInMF.Location = New System.Drawing.Point(359, 44)
        Me.lstTLTXCDInMF.Name = "lstTLTXCDInMF"
        Me.lstTLTXCDInMF.Size = New System.Drawing.Size(286, 329)
        Me.lstTLTXCDInMF.TabIndex = 3
        Me.lstTLTXCDInMF.Tag = "lstUserInGroup"
        '
        'lstTLTXCDNoMF
        '
        Me.lstTLTXCDNoMF.FormattingEnabled = True
        Me.lstTLTXCDNoMF.Location = New System.Drawing.Point(14, 44)
        Me.lstTLTXCDNoMF.Name = "lstTLTXCDNoMF"
        Me.lstTLTXCDNoMF.Size = New System.Drawing.Size(283, 329)
        Me.lstTLTXCDNoMF.TabIndex = 2
        Me.lstTLTXCDNoMF.Tag = "lstTLTXCDNoMF"
        '
        'lbTLTXCDInMF
        '
        Me.lbTLTXCDInMF.AllowDrop = True
        Me.lbTLTXCDInMF.AutoSize = True
        Me.lbTLTXCDInMF.Location = New System.Drawing.Point(356, 28)
        Me.lbTLTXCDInMF.Name = "lbTLTXCDInMF"
        Me.lbTLTXCDInMF.Size = New System.Drawing.Size(81, 13)
        Me.lbTLTXCDInMF.TabIndex = 1
        Me.lbTLTXCDInMF.Tag = "lbTLTXCDInMF"
        Me.lbTLTXCDInMF.Text = "lbTLTXCDInMF"
        '
        'lbTLTXCDNoMF
        '
        Me.lbTLTXCDNoMF.AutoSize = True
        Me.lbTLTXCDNoMF.Location = New System.Drawing.Point(11, 28)
        Me.lbTLTXCDNoMF.Name = "lbTLTXCDNoMF"
        Me.lbTLTXCDNoMF.Size = New System.Drawing.Size(92, 13)
        Me.lbTLTXCDNoMF.TabIndex = 0
        Me.lbTLTXCDNoMF.Tag = "lbTLTXCDNoMF"
        Me.lbTLTXCDNoMF.Text = "lbTLTXCDNoMFp"
        '
        'lbNote
        '
        Me.lbNote.AutoSize = True
        Me.lbNote.ForeColor = System.Drawing.SystemColors.Highlight
        Me.lbNote.Location = New System.Drawing.Point(228, 66)
        Me.lbNote.Name = "lbNote"
        Me.lbNote.Size = New System.Drawing.Size(38, 13)
        Me.lbNote.TabIndex = 13
        Me.lbNote.Tag = "lbNote"
        Me.lbNote.Text = "lbNote"
        '
        'frmAddMFToTLTX
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(681, 512)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.lbMF)
        Me.Controls.Add(Me.txtMF)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.grbMFToTLTXCD)
        Me.Controls.Add(Me.lbNote)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmAddMFToTLTX"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "frmAddMFToTLTX"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.grbMFToTLTXCD.ResumeLayout(False)
        Me.grbMFToTLTXCD.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnRemove As System.Windows.Forms.Button
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents lbCaption As System.Windows.Forms.Label
    Friend WithEvents btnRemoveAll As System.Windows.Forms.Button
    Friend WithEvents lbMF As System.Windows.Forms.Label
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents txtMF As System.Windows.Forms.TextBox
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnAddAll As System.Windows.Forms.Button
    Friend WithEvents grbMFToTLTXCD As System.Windows.Forms.GroupBox
    Friend WithEvents lstTLTXCDInMF As System.Windows.Forms.ListBox
    Friend WithEvents lstTLTXCDNoMF As System.Windows.Forms.ListBox
    Friend WithEvents lbTLTXCDInMF As System.Windows.Forms.Label
    Friend WithEvents lbTLTXCDNoMF As System.Windows.Forms.Label
    Friend WithEvents lbNote As System.Windows.Forms.Label
End Class
