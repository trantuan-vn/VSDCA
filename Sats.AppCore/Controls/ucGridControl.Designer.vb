<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ucGridControl
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ucGridControl))
        Me.pnlSearch = New System.Windows.Forms.Panel
        Me.pnlFilter = New System.Windows.Forms.Panel
        Me.gbFilter = New System.Windows.Forms.GroupBox
        Me.txtValue = New System.Windows.Forms.Control
        Me.btnFilter = New System.Windows.Forms.Button
        Me.lblValue = New System.Windows.Forms.Label
        Me.cboOperator = New Sats.AppCore.ComboBoxEx
        Me.lblOperator = New System.Windows.Forms.Label
        Me.cboField = New Sats.AppCore.ComboBoxEx
        Me.lblField = New System.Windows.Forms.Label
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.pnlFilter.SuspendLayout()
        Me.gbFilter.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlSearch
        '
        Me.pnlSearch.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlSearch.Location = New System.Drawing.Point(3, 88)
        Me.pnlSearch.Name = "pnlSearch"
        Me.pnlSearch.Size = New System.Drawing.Size(794, 409)
        Me.pnlSearch.TabIndex = 0
        '
        'pnlFilter
        '
        Me.pnlFilter.Controls.Add(Me.gbFilter)
        Me.pnlFilter.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlFilter.Location = New System.Drawing.Point(3, 16)
        Me.pnlFilter.Name = "pnlFilter"
        Me.pnlFilter.Size = New System.Drawing.Size(794, 66)
        Me.pnlFilter.TabIndex = 1
        '
        'gbFilter
        '
        Me.gbFilter.Controls.Add(Me.txtValue)
        Me.gbFilter.Controls.Add(Me.btnFilter)
        Me.gbFilter.Controls.Add(Me.lblValue)
        Me.gbFilter.Controls.Add(Me.cboOperator)
        Me.gbFilter.Controls.Add(Me.lblOperator)
        Me.gbFilter.Controls.Add(Me.cboField)
        Me.gbFilter.Controls.Add(Me.lblField)
        Me.gbFilter.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbFilter.Location = New System.Drawing.Point(0, 0)
        Me.gbFilter.Name = "gbFilter"
        Me.gbFilter.Size = New System.Drawing.Size(794, 66)
        Me.gbFilter.TabIndex = 0
        Me.gbFilter.TabStop = False
        Me.gbFilter.Text = "Lọc dữ liệu"
        '
        'txtValue
        '
        Me.txtValue.Location = New System.Drawing.Point(104, 76)
        Me.txtValue.Name = "txtValue"
        Me.txtValue.Size = New System.Drawing.Size(179, 20)
        Me.txtValue.TabIndex = 5
        '
        'btnFilter
        '
        Me.btnFilter.Location = New System.Drawing.Point(704, 21)
        Me.btnFilter.Name = "btnFilter"
        Me.btnFilter.Size = New System.Drawing.Size(75, 23)
        Me.btnFilter.TabIndex = 5
        Me.btnFilter.Text = "Lọc"
        Me.btnFilter.UseVisualStyleBackColor = True
        '
        'lblValue
        '
        Me.lblValue.AutoSize = True
        Me.lblValue.Location = New System.Drawing.Point(392, 27)
        Me.lblValue.Name = "lblValue"
        Me.lblValue.Size = New System.Drawing.Size(34, 13)
        Me.lblValue.TabIndex = 4
        Me.lblValue.Text = "Giá trị"
        '
        'cboOperator
        '
        Me.cboOperator.DisplayMember = "DISPLAY"
        Me.cboOperator.FormattingEnabled = True
        Me.cboOperator.Location = New System.Drawing.Point(276, 21)
        Me.cboOperator.Name = "cboOperator"
        Me.cboOperator.Size = New System.Drawing.Size(100, 21)
        Me.cboOperator.TabIndex = 3
        Me.cboOperator.ValueMember = "VALUE"
        '
        'lblOperator
        '
        Me.lblOperator.AutoSize = True
        Me.lblOperator.Location = New System.Drawing.Point(218, 27)
        Me.lblOperator.Name = "lblOperator"
        Me.lblOperator.Size = New System.Drawing.Size(52, 13)
        Me.lblOperator.TabIndex = 2
        Me.lblOperator.Text = "Điều kiện"
        '
        'cboField
        '
        Me.cboField.DisplayMember = "DISPLAY"
        Me.cboField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboField.FormattingEnabled = True
        Me.cboField.Location = New System.Drawing.Point(62, 21)
        Me.cboField.Name = "cboField"
        Me.cboField.Size = New System.Drawing.Size(150, 21)
        Me.cboField.TabIndex = 1
        Me.cboField.ValueMember = "VALUE"
        '
        'lblField
        '
        Me.lblField.AutoSize = True
        Me.lblField.Location = New System.Drawing.Point(9, 24)
        Me.lblField.Name = "lblField"
        Me.lblField.Size = New System.Drawing.Size(47, 13)
        Me.lblField.TabIndex = 0
        Me.lblField.Text = "Tiêu chí"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.pnlFilter)
        Me.GroupBox1.Controls.Add(Me.pnlSearch)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(800, 500)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        '
        'ucGridControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "ucGridControl"
        Me.Size = New System.Drawing.Size(800, 500)
        Me.pnlFilter.ResumeLayout(False)
        Me.gbFilter.ResumeLayout(False)
        Me.gbFilter.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnlSearch As System.Windows.Forms.Panel
    Friend WithEvents gbFilter As System.Windows.Forms.GroupBox
    Friend WithEvents lblValue As System.Windows.Forms.Label
    Friend WithEvents cboOperator As Sats.AppCore.ComboBoxEx
    Friend WithEvents lblOperator As System.Windows.Forms.Label
    Friend WithEvents cboField As Sats.AppCore.ComboBoxEx
    Friend WithEvents lblField As System.Windows.Forms.Label
    Friend WithEvents btnFilter As System.Windows.Forms.Button
    Friend WithEvents pnlFilter As System.Windows.Forms.Panel
    Friend WithEvents txtValue As System.Windows.Forms.Control
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox

End Class
