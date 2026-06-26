<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTLGroups
    Inherits Sats.AppCore.frmMaintenance

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTLGroups))
        Me.grbGroups = New System.Windows.Forms.GroupBox
        Me.txtGroupID = New Sats.AppCore.FlexMaskEditBox
        Me.cboGroupActive = New Sats.AppCore.ComboBoxEx
        Me.txtGroupDesc = New System.Windows.Forms.TextBox
        Me.txtGroupName = New System.Windows.Forms.TextBox
        Me.lbGroupDesc = New System.Windows.Forms.Label
        Me.lbGroupActive = New System.Windows.Forms.Label
        Me.lbGroupName = New System.Windows.Forms.Label
        Me.lbGroupID = New System.Windows.Forms.Label
        Me.grbPermit = New System.Windows.Forms.GroupBox
        Me.btnBrID = New System.Windows.Forms.Button
        Me.btnStock = New System.Windows.Forms.Button
        Me.btnTVLK = New System.Windows.Forms.Button
        Me.btnReport = New System.Windows.Forms.Button
        Me.btnTran = New System.Windows.Forms.Button
        Me.btnFun = New System.Windows.Forms.Button
        Me.btnCA = New System.Windows.Forms.Button
        Me.Panel1.SuspendLayout()
        Me.grbGroups.SuspendLayout()
        Me.grbPermit.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Size = New System.Drawing.Size(671, 42)
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(577, 182)
        Me.btnOk.TabIndex = 4
        '
        'btnApply
        '
        Me.btnApply.Visible = False
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(577, 213)
        Me.btnCancel.TabIndex = 5
        '
        'grbGroups
        '
        Me.grbGroups.Controls.Add(Me.txtGroupID)
        Me.grbGroups.Controls.Add(Me.cboGroupActive)
        Me.grbGroups.Controls.Add(Me.txtGroupDesc)
        Me.grbGroups.Controls.Add(Me.txtGroupName)
        Me.grbGroups.Controls.Add(Me.lbGroupDesc)
        Me.grbGroups.Controls.Add(Me.lbGroupActive)
        Me.grbGroups.Controls.Add(Me.lbGroupName)
        Me.grbGroups.Controls.Add(Me.lbGroupID)
        Me.grbGroups.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grbGroups.Location = New System.Drawing.Point(6, 48)
        Me.grbGroups.Name = "grbGroups"
        Me.grbGroups.Size = New System.Drawing.Size(653, 128)
        Me.grbGroups.TabIndex = 4
        Me.grbGroups.TabStop = False
        Me.grbGroups.Tag = "grbGroups"
        Me.grbGroups.Text = "grbGroups"
        '
        'txtGroupID
        '
        Me.txtGroupID.Location = New System.Drawing.Point(116, 19)
        Me.txtGroupID.Name = "txtGroupID"
        Me.txtGroupID.Size = New System.Drawing.Size(123, 20)
        Me.txtGroupID.TabIndex = 0
        Me.txtGroupID.Tag = "GRPID"
        '
        'cboGroupActive
        '
        Me.cboGroupActive.DisplayMember = "DISPLAY"
        Me.cboGroupActive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboGroupActive.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboGroupActive.FormattingEnabled = True
        Me.cboGroupActive.Location = New System.Drawing.Point(116, 71)
        Me.cboGroupActive.Name = "cboGroupActive"
        Me.cboGroupActive.Size = New System.Drawing.Size(123, 21)
        Me.cboGroupActive.TabIndex = 2
        Me.cboGroupActive.Tag = "STATUS"
        Me.cboGroupActive.ValueMember = "VALUE"
        '
        'txtGroupDesc
        '
        Me.txtGroupDesc.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtGroupDesc.Location = New System.Drawing.Point(116, 98)
        Me.txtGroupDesc.Name = "txtGroupDesc"
        Me.txtGroupDesc.Size = New System.Drawing.Size(294, 20)
        Me.txtGroupDesc.TabIndex = 3
        Me.txtGroupDesc.Tag = "DESCRIPTION"
        '
        'txtGroupName
        '
        Me.txtGroupName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtGroupName.Location = New System.Drawing.Point(116, 45)
        Me.txtGroupName.Name = "txtGroupName"
        Me.txtGroupName.Size = New System.Drawing.Size(294, 20)
        Me.txtGroupName.TabIndex = 1
        Me.txtGroupName.Tag = "GRPNAME"
        '
        'lbGroupDesc
        '
        Me.lbGroupDesc.AutoSize = True
        Me.lbGroupDesc.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbGroupDesc.Location = New System.Drawing.Point(14, 101)
        Me.lbGroupDesc.Name = "lbGroupDesc"
        Me.lbGroupDesc.Size = New System.Drawing.Size(69, 13)
        Me.lbGroupDesc.TabIndex = 3
        Me.lbGroupDesc.Tag = "GROUPDESC"
        Me.lbGroupDesc.Text = "lbGroupDesc"
        '
        'lbGroupActive
        '
        Me.lbGroupActive.AutoSize = True
        Me.lbGroupActive.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbGroupActive.Location = New System.Drawing.Point(11, 74)
        Me.lbGroupActive.Name = "lbGroupActive"
        Me.lbGroupActive.Size = New System.Drawing.Size(74, 13)
        Me.lbGroupActive.TabIndex = 2
        Me.lbGroupActive.Tag = "GROUPACTIVE"
        Me.lbGroupActive.Text = "lbGroupActive"
        '
        'lbGroupName
        '
        Me.lbGroupName.AutoSize = True
        Me.lbGroupName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbGroupName.Location = New System.Drawing.Point(11, 48)
        Me.lbGroupName.Name = "lbGroupName"
        Me.lbGroupName.Size = New System.Drawing.Size(72, 13)
        Me.lbGroupName.TabIndex = 1
        Me.lbGroupName.Tag = "GROUPNAME"
        Me.lbGroupName.Text = "lbGroupName"
        '
        'lbGroupID
        '
        Me.lbGroupID.AutoSize = True
        Me.lbGroupID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbGroupID.Location = New System.Drawing.Point(11, 22)
        Me.lbGroupID.Name = "lbGroupID"
        Me.lbGroupID.Size = New System.Drawing.Size(55, 13)
        Me.lbGroupID.TabIndex = 0
        Me.lbGroupID.Tag = "GROUPID"
        Me.lbGroupID.Text = "lbGroupID"
        '
        'grbPermit
        '
        Me.grbPermit.Controls.Add(Me.btnCA)
        Me.grbPermit.Controls.Add(Me.btnBrID)
        Me.grbPermit.Controls.Add(Me.btnStock)
        Me.grbPermit.Controls.Add(Me.btnTVLK)
        Me.grbPermit.Controls.Add(Me.btnReport)
        Me.grbPermit.Controls.Add(Me.btnTran)
        Me.grbPermit.Controls.Add(Me.btnFun)
        Me.grbPermit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grbPermit.Location = New System.Drawing.Point(6, 182)
        Me.grbPermit.Name = "grbPermit"
        Me.grbPermit.Size = New System.Drawing.Size(565, 56)
        Me.grbPermit.TabIndex = 5
        Me.grbPermit.TabStop = False
        Me.grbPermit.Tag = "grbPermit"
        Me.grbPermit.Text = "grbPermit"
        '
        'btnBrID
        '
        Me.btnBrID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBrID.Location = New System.Drawing.Point(401, 22)
        Me.btnBrID.Name = "btnBrID"
        Me.btnBrID.Size = New System.Drawing.Size(84, 25)
        Me.btnBrID.TabIndex = 11
        Me.btnBrID.Tag = "btnBrID"
        Me.btnBrID.Text = "btnBrID"
        Me.btnBrID.UseVisualStyleBackColor = True
        '
        'btnStock
        '
        Me.btnStock.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnStock.Location = New System.Drawing.Point(314, 22)
        Me.btnStock.Name = "btnStock"
        Me.btnStock.Size = New System.Drawing.Size(84, 25)
        Me.btnStock.TabIndex = 10
        Me.btnStock.Tag = "btnStock"
        Me.btnStock.Text = "btnStock"
        Me.btnStock.UseVisualStyleBackColor = True
        '
        'btnTVLK
        '
        Me.btnTVLK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnTVLK.Location = New System.Drawing.Point(242, 22)
        Me.btnTVLK.Name = "btnTVLK"
        Me.btnTVLK.Size = New System.Drawing.Size(69, 25)
        Me.btnTVLK.TabIndex = 9
        Me.btnTVLK.Tag = "btnTVLK"
        Me.btnTVLK.Text = "btnTVLK"
        Me.btnTVLK.UseVisualStyleBackColor = True
        '
        'btnReport
        '
        Me.btnReport.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnReport.Location = New System.Drawing.Point(166, 22)
        Me.btnReport.Name = "btnReport"
        Me.btnReport.Size = New System.Drawing.Size(73, 25)
        Me.btnReport.TabIndex = 8
        Me.btnReport.Tag = "btnReport"
        Me.btnReport.Text = "btnReport"
        Me.btnReport.UseVisualStyleBackColor = True
        '
        'btnTran
        '
        Me.btnTran.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnTran.Location = New System.Drawing.Point(85, 22)
        Me.btnTran.Name = "btnTran"
        Me.btnTran.Size = New System.Drawing.Size(79, 25)
        Me.btnTran.TabIndex = 7
        Me.btnTran.Tag = "btnTran"
        Me.btnTran.Text = "btnTran"
        Me.btnTran.UseVisualStyleBackColor = True
        '
        'btnFun
        '
        Me.btnFun.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFun.Location = New System.Drawing.Point(6, 22)
        Me.btnFun.Name = "btnFun"
        Me.btnFun.Size = New System.Drawing.Size(77, 25)
        Me.btnFun.TabIndex = 6
        Me.btnFun.Tag = "btnFun"
        Me.btnFun.Text = "btnFun"
        Me.btnFun.UseVisualStyleBackColor = True
        '
        'btnCA
        '
        Me.btnCA.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCA.Location = New System.Drawing.Point(491, 22)
        Me.btnCA.Name = "btnCA"
        Me.btnCA.Size = New System.Drawing.Size(68, 25)
        Me.btnCA.TabIndex = 12
        Me.btnCA.Tag = "btnCA"
        Me.btnCA.Text = "btnCA"
        Me.btnCA.UseVisualStyleBackColor = True
        '
        'frmTLGroups
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(671, 255)
        Me.Controls.Add(Me.grbGroups)
        Me.Controls.Add(Me.grbPermit)
        Me.Name = "frmTLGroups"
        Me.TabText = "frmTLGroups"
        Me.Text = "frmTLGroups"
        Me.Controls.SetChildIndex(Me.Panel1, 0)
        Me.Controls.SetChildIndex(Me.btnOk, 0)
        Me.Controls.SetChildIndex(Me.btnApply, 0)
        Me.Controls.SetChildIndex(Me.btnCancel, 0)
        Me.Controls.SetChildIndex(Me.grbPermit, 0)
        Me.Controls.SetChildIndex(Me.grbGroups, 0)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.grbGroups.ResumeLayout(False)
        Me.grbGroups.PerformLayout()
        Me.grbPermit.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grbGroups As System.Windows.Forms.GroupBox
    Friend WithEvents txtGroupID As Sats.AppCore.FlexMaskEditBox
    Friend WithEvents cboGroupActive As Sats.AppCore.ComboBoxEx
    Friend WithEvents txtGroupDesc As System.Windows.Forms.TextBox
    Friend WithEvents txtGroupName As System.Windows.Forms.TextBox
    Friend WithEvents lbGroupDesc As System.Windows.Forms.Label
    Friend WithEvents lbGroupActive As System.Windows.Forms.Label
    Friend WithEvents lbGroupName As System.Windows.Forms.Label
    Friend WithEvents lbGroupID As System.Windows.Forms.Label
    Friend WithEvents grbPermit As System.Windows.Forms.GroupBox
    Friend WithEvents btnTVLK As System.Windows.Forms.Button
    Friend WithEvents btnReport As System.Windows.Forms.Button
    Friend WithEvents btnTran As System.Windows.Forms.Button
    Friend WithEvents btnFun As System.Windows.Forms.Button
    Friend WithEvents btnBrID As System.Windows.Forms.Button
    Friend WithEvents btnStock As System.Windows.Forms.Button
    Friend WithEvents btnCA As System.Windows.Forms.Button
End Class
