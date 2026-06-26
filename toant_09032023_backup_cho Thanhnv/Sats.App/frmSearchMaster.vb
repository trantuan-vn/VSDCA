Imports Sats.CommonLibrary

Public Class frmSearchMaster
    Private v_frm As Sats.AppCore.frmMaintenance
    Public Sub New(ByVal pv_strLanguage As String)

        ' This call is required by the Windows Form Designer.
        MyBase.New(pv_strLanguage)
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Public Overrides Sub InitDialog()
        MyBase.InitDialog()
        Me.btnAddNew.Text = ResourceManager.GetString("btnAddNew")
        Me.btnAddNew.Shortcut = Shortcut.CtrlN
        Me.btnView.Text = ResourceManager.GetString("btnView")
        Me.btnView.Shortcut = Shortcut.CtrlV

        Me.btnEdit.Text = ResourceManager.GetString("btnEdit")
        Me.btnEdit.Shortcut = Shortcut.CtrlE

        Me.btnDelete.Text = ResourceManager.GetString("btnDelete")
        Me.btnDelete.Shortcut = Shortcut.CtrlD

        Me.btnExecute.Visible = False
    End Sub

    Protected Overrides Function ShowForm(ByVal pv_intExecFlag As Integer) As Boolean
        'Dim v_thread As New Threading.Thread(AddressOf ShowFormProcess)
        Dim v_oProcess As New Sats.AppCore.ProcessForm(Me)
        v_oProcess.StartProcessForm()
        Dim v_strFullObjName As String
        MyBase.ShowForm(pv_intExecFlag)

        Try
            If pv_intExecFlag <> ExecuteFlag.Delete Then

                
                If pv_intExecFlag <> ExecuteFlag.AddNew Then
                    If (SearchGrid.CurrentRow Is Nothing) Then
                        v_oProcess.StopProcessForm()
                        MsgBox(ResourceManager.GetString("NotSelected"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, Me.Text)
                        Exit Function
                    End If
                    If (SearchGrid.CurrentRow Is SearchGrid.FixedFooterRows.Item(0)) Then
                        v_oProcess.StopProcessForm()
                        MsgBox(ResourceManager.GetString("Footer"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, Me.Text)
                        Exit Function
                    End If
                End If
                'Dim v_frm As Sats.AppCore.frmMaintenance = Nothing

                v_strFullObjName = ModuleCode & "." & ObjectName
                If Not CheckExitsForm("frm" & ObjectName) Then
                    Select Case v_strFullObjName
                        Case OBJNAME_SA_ALLCODE
                            v_frm = New frmAllCode
                        Case OBJNAME_SA_TLGROUPS
                            v_frm = New frmTLGroups
                        Case OBJNAME_SA_TLPROFILES
                            v_frm = New frmTLPROFILES
                        Case OBJNAME_RG_RGIS
                            v_frm = New frmRGIS
                        Case OBJNAME_RG_RGMI
                            v_frm = New frmRGMI
                        Case OBJNAME_RG_RGSI
                            v_frm = New frmRGSI
                        Case OBJNAME_SA_BRGRP
                            v_frm = New frmBRGRP
                        Case OBJNAME_RG_RGII
                            v_frm = New frmRGII
                        Case OBJNAME_SA_TLTX
                            v_frm = New frmTLTX
                        Case OBJNAME_SA_TLTXUSERAUTH
                            v_frm = New frmTLTXUserAuth
                    End Select
                    v_frm.Name = "frm" & ObjectName
                    v_frm.ExeFlag = pv_intExecFlag
                    v_frm.UserLanguage = UserLanguage
                    v_frm.ModuleCode = ModuleCode
                    v_frm.ObjectName = v_strFullObjName
                    v_frm.TableName = ObjectName
                    v_frm.LocalObject = IsLocalSearch
                    'v_frm.Text = FormCaption
                    v_frm.TellerId = TellerId
                    v_frm.TellerType = TellerType
                    v_frm.AuthString = AuthString
                    v_frm.BranchId = BranchId
                    v_frm.BusDate = Me.BusDate
                    v_frm.KeyFieldName = KeyColumn
                    v_frm.KeyFieldType = KeyFieldType
                    v_frm.Proxy = Proxy
                    'Added by Thanglv9 - 12/12/2012
                    v_frm.IpAddress = IpAddress
                    v_frm.WsName = WsName
                    v_frm.VSDBRID = VsdBrid
                    v_frm.TabText = TabText
                    v_frm.TellerName = m_BusLayer.CurrentTellerProfile.TellerName
                    v_frm.BRCODE = m_BusLayer.CurrentTellerProfile.BranchName
                    v_frm.FormCaption = FormCaption
                    'End
                    'v_frm.DockPanel = Me.DockPanel
                    'v_frm.SearchForm = Me

                    If pv_intExecFlag <> ExecuteFlag.AddNew Then
                        v_frm.KeyFieldValue = Replace(Trim(CType(SearchGrid.CurrentRow, Xceed.Grid.DataRow).Cells(KeyColumn).Value), ".", String.Empty)
                    End If
                    v_frm.OnInit()
                    v_oProcess.StopProcessForm()
                    v_frm.ShowDialog(Me.DockPanel)
                End If
            End If

        Catch ex As Exception
            v_oProcess.StopProcessForm()
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                                     & "Error code: System error!" & vbNewLine _
                                                     & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        Finally
            v_oProcess.StopProcessForm()
        End Try
    End Function


    Private Function CheckExitsForm(ByVal v_strFormName As String) As Boolean
        Dim o_frm As Sats.WinFormsUI.Docking.DockContent
        Dim v_blnIsExits As Boolean = False

        For Each o_frm In Me.PanelPane.DockPanel.Documents
            If o_frm.Name = v_strFormName Then
                v_blnIsExits = True
                o_frm.Activate()
                Exit For
            End If
        Next
        Return v_blnIsExits
    End Function

    'Private Sub ShowFormProcess()
    '    Dim v_frm As New Sats.AppCore.frmProcess
    '    v_frm.ShowDialog()
    'End Sub
End Class
