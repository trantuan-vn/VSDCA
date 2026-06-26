<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLookUpRGII
    Inherits System.Windows.Forms.Form

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmLookUpRGII))
        Me.lstAllPerson = New System.Windows.Forms.ListBox
        Me.lstRepPerson = New System.Windows.Forms.ListBox
        Me.btnAddAll = New System.Windows.Forms.Button
        Me.btnAdd = New System.Windows.Forms.Button
        Me.btnRemove = New System.Windows.Forms.Button
        Me.btnRemoveAll = New System.Windows.Forms.Button
        Me.lbCardType = New System.Windows.Forms.Label
        Me.cboCardType = New Sats.AppCore.ComboBoxEx
        Me.lbCardNo = New System.Windows.Forms.Label
        Me.txtCardNo = New System.Windows.Forms.TextBox
        Me.btnSearch = New System.Windows.Forms.Button
        Me.grbLookUp = New System.Windows.Forms.GroupBox
        Me.lbRepPerson = New System.Windows.Forms.Label
        Me.lbAllPerson = New System.Windows.Forms.Label
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.lbCaption = New System.Windows.Forms.Label
        Me.btnCancel = New System.Windows.Forms.Button
        Me.grbLookUp.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'lstAllPerson
        '
        Me.lstAllPerson.FormattingEnabled = True
        Me.lstAllPerson.Location = New System.Drawing.Point(8, 32)
        Me.lstAllPerson.Name = "lstAllPerson"
        Me.lstAllPerson.Size = New System.Drawing.Size(231, 264)
        Me.lstAllPerson.TabIndex = 0
        '
        'lstRepPerson
        '
        Me.lstRepPerson.FormattingEnabled = True
        Me.lstRepPerson.Location = New System.Drawing.Point(338, 32)
        Me.lstRepPerson.Name = "lstRepPerson"
        Me.lstRepPerson.Size = New System.Drawing.Size(226, 264)
        Me.lstRepPerson.TabIndex = 1
        '
        'btnAddAll
        '
        Me.btnAddAll.Font = New System.Drawing.Font("Webdings", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.btnAddAll.Location = New System.Drawing.Point(267, 62)
        Me.btnAddAll.Name = "btnAddAll"
        Me.btnAddAll.Size = New System.Drawing.Size(42, 35)
        Me.btnAddAll.TabIndex = 2
        Me.btnAddAll.Tag = "AddAll"
        Me.btnAddAll.Text = "8"
        Me.btnAddAll.UseVisualStyleBackColor = True
        '
        'btnAdd
        '
        Me.btnAdd.Font = New System.Drawing.Font("Webdings", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.btnAdd.Location = New System.Drawing.Point(267, 112)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(42, 34)
        Me.btnAdd.TabIndex = 3
        Me.btnAdd.Tag = "Add"
        Me.btnAdd.Text = "4"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'btnRemove
        '
        Me.btnRemove.Font = New System.Drawing.Font("Webdings", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.btnRemove.Location = New System.Drawing.Point(267, 161)
        Me.btnRemove.Name = "btnRemove"
        Me.btnRemove.Size = New System.Drawing.Size(42, 36)
        Me.btnRemove.TabIndex = 4
        Me.btnRemove.Tag = "Remove"
        Me.btnRemove.Text = "3"
        Me.btnRemove.UseVisualStyleBackColor = True
        '
        'btnRemoveAll
        '
        Me.btnRemoveAll.Font = New System.Drawing.Font("Webdings", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.btnRemoveAll.Location = New System.Drawing.Point(267, 215)
        Me.btnRemoveAll.Name = "btnRemoveAll"
        Me.btnRemoveAll.Size = New System.Drawing.Size(42, 36)
        Me.btnRemoveAll.TabIndex = 5
        Me.btnRemoveAll.Tag = "RemoveAll"
        Me.btnRemoveAll.Text = "7"
        Me.btnRemoveAll.UseVisualStyleBackColor = True
        '
        'lbCardType
        '
        Me.lbCardType.AutoSize = True
        Me.lbCardType.Location = New System.Drawing.Point(1, 396)
        Me.lbCardType.Name = "lbCardType"
        Me.lbCardType.Size = New System.Drawing.Size(61, 13)
        Me.lbCardType.TabIndex = 6
        Me.lbCardType.Tag = "lbCardType"
        Me.lbCardType.Text = "lbCardType"
        '
        'cboCardType
        '
        Me.cboCardType.DisplayMember = "DISPLAY"
        Me.cboCardType.FormattingEnabled = True
        Me.cboCardType.Location = New System.Drawing.Point(59, 391)
        Me.cboCardType.Name = "cboCardType"
        Me.cboCardType.Size = New System.Drawing.Size(142, 21)
        Me.cboCardType.TabIndex = 7
        Me.cboCardType.Tag = "CARDTYPE"
        Me.cboCardType.ValueMember = "VALUE"
        '
        'lbCardNo
        '
        Me.lbCardNo.AutoSize = True
        Me.lbCardNo.Location = New System.Drawing.Point(207, 399)
        Me.lbCardNo.Name = "lbCardNo"
        Me.lbCardNo.Size = New System.Drawing.Size(51, 13)
        Me.lbCardNo.TabIndex = 8
        Me.lbCardNo.Tag = "lbCardNo"
        Me.lbCardNo.Text = "lbCardNo"
        '
        'txtCardNo
        '
        Me.txtCardNo.Location = New System.Drawing.Point(263, 392)
        Me.txtCardNo.Name = "txtCardNo"
        Me.txtCardNo.Size = New System.Drawing.Size(144, 20)
        Me.txtCardNo.TabIndex = 9
        Me.txtCardNo.Tag = "CARDNO"
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(422, 392)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(75, 23)
        Me.btnSearch.TabIndex = 10
        Me.btnSearch.Tag = "SEARCH"
        Me.btnSearch.Text = "btnSearch"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'grbLookUp
        '
        Me.grbLookUp.Controls.Add(Me.lbRepPerson)
        Me.grbLookUp.Controls.Add(Me.lbAllPerson)
        Me.grbLookUp.Controls.Add(Me.lstAllPerson)
        Me.grbLookUp.Controls.Add(Me.lstRepPerson)
        Me.grbLookUp.Controls.Add(Me.btnAdd)
        Me.grbLookUp.Controls.Add(Me.btnAddAll)
        Me.grbLookUp.Controls.Add(Me.btnRemove)
        Me.grbLookUp.Controls.Add(Me.btnRemoveAll)
        Me.grbLookUp.Location = New System.Drawing.Point(4, 61)
        Me.grbLookUp.Name = "grbLookUp"
        Me.grbLookUp.Size = New System.Drawing.Size(574, 310)
        Me.grbLookUp.TabIndex = 11
        Me.grbLookUp.TabStop = False
        Me.grbLookUp.Tag = "grbLookUp"
        Me.grbLookUp.Text = "grbLookUp"
        '
        'lbRepPerson
        '
        Me.lbRepPerson.AutoSize = True
        Me.lbRepPerson.Location = New System.Drawing.Point(335, 16)
        Me.lbRepPerson.Name = "lbRepPerson"
        Me.lbRepPerson.Size = New System.Drawing.Size(68, 13)
        Me.lbRepPerson.TabIndex = 7
        Me.lbRepPerson.Tag = "lbRepPerson"
        Me.lbRepPerson.Text = "lbRepPerson"
        '
        'lbAllPerson
        '
        Me.lbAllPerson.AutoSize = True
        Me.lbAllPerson.Location = New System.Drawing.Point(6, 16)
        Me.lbAllPerson.Name = "lbAllPerson"
        Me.lbAllPerson.Size = New System.Drawing.Size(59, 13)
        Me.lbAllPerson.TabIndex = 6
        Me.lbAllPerson.Tag = "lbAllPerson"
        Me.lbAllPerson.Text = "lbAllPerson"
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.SystemColors.Desktop
        Me.Panel1.Controls.Add(Me.lbCaption)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(583, 56)
        Me.Panel1.TabIndex = 12
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
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(503, 391)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 13
        Me.btnCancel.Tag = "btnCancel"
        Me.btnCancel.Text = "btnCancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'frmLookUpRGII
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(583, 426)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.grbLookUp)
        Me.Controls.Add(Me.btnSearch)
        Me.Controls.Add(Me.txtCardNo)
        Me.Controls.Add(Me.lbCardNo)
        Me.Controls.Add(Me.cboCardType)
        Me.Controls.Add(Me.lbCardType)
        Me.Name = "frmLookUpRGII"
        Me.Text = "frmLookUpRGII"
        Me.grbLookUp.ResumeLayout(False)
        Me.grbLookUp.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lstAllPerson As System.Windows.Forms.ListBox
    Friend WithEvents lstRepPerson As System.Windows.Forms.ListBox
    Friend WithEvents btnAddAll As System.Windows.Forms.Button
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents btnRemove As System.Windows.Forms.Button
    Friend WithEvents btnRemoveAll As System.Windows.Forms.Button
    Friend WithEvents lbCardType As System.Windows.Forms.Label
    Friend WithEvents cboCardType As Sats.AppCore.ComboBoxEx
    Friend WithEvents lbCardNo As System.Windows.Forms.Label
    Friend WithEvents txtCardNo As System.Windows.Forms.TextBox
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents grbLookUp As System.Windows.Forms.GroupBox
    Friend WithEvents lbRepPerson As System.Windows.Forms.Label
    Friend WithEvents lbAllPerson As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents lbCaption As System.Windows.Forms.Label
    Friend WithEvents btnCancel As System.Windows.Forms.Button
End Class
