<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLookUp
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmLookUp))
        Me.pnLookup = New System.Windows.Forms.Panel
        Me.lblSearch = New System.Windows.Forms.Label
        Me.txtSearch = New System.Windows.Forms.TextBox
        Me.btnOK = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.cboSearchOption = New Sats.AppCore.ComboBoxEx
        Me.SuspendLayout()
        '
        'pnLookup
        '
        Me.pnLookup.Location = New System.Drawing.Point(2, 4)
        Me.pnLookup.Name = "pnLookup"
        Me.pnLookup.Size = New System.Drawing.Size(578, 332)
        Me.pnLookup.TabIndex = 0
        '
        'lblSearch
        '
        Me.lblSearch.AutoSize = True
        Me.lblSearch.Location = New System.Drawing.Point(5, 349)
        Me.lblSearch.Name = "lblSearch"
        Me.lblSearch.Size = New System.Drawing.Size(64, 13)
        Me.lblSearch.TabIndex = 1
        Me.lblSearch.Text = "Search text:"
        '
        'txtSearch
        '
        Me.txtSearch.Location = New System.Drawing.Point(227, 347)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(353, 20)
        Me.txtSearch.TabIndex = 2
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(394, 373)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(90, 28)
        Me.btnOK.TabIndex = 3
        Me.btnOK.Text = "btnOK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(490, 373)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(90, 28)
        Me.btnCancel.TabIndex = 4
        Me.btnCancel.Text = "btnCancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'cboSearchOption
        '
        Me.cboSearchOption.DisplayMember = "DISPLAY"
        Me.cboSearchOption.FormattingEnabled = True
        Me.cboSearchOption.Location = New System.Drawing.Point(100, 347)
        Me.cboSearchOption.Name = "cboSearchOption"
        Me.cboSearchOption.Size = New System.Drawing.Size(121, 21)
        Me.cboSearchOption.TabIndex = 1
        Me.cboSearchOption.ValueMember = "VALUE"
        '
        'frmLookUp
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(585, 404)
        Me.Controls.Add(Me.cboSearchOption)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.txtSearch)
        Me.Controls.Add(Me.lblSearch)
        Me.Controls.Add(Me.pnLookup)
        Me.KeyPreview = True
        Me.Name = "frmLookUp"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "frmLookUp"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pnLookup As System.Windows.Forms.Panel
    Friend WithEvents lblSearch As System.Windows.Forms.Label
    Friend WithEvents txtSearch As System.Windows.Forms.TextBox
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button

    Public Sub New(ByVal pv_strLanguage As String)
        MyBase.New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        mv_strLanguage = pv_strLanguage
    End Sub
    Friend WithEvents cboSearchOption As AppCore.ComboBoxEx
End Class
