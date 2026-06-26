Imports System
Imports Sats.CommonLibrary
Imports System.Windows.Forms
Imports Xceed.SmartUI.Controls.TreeView
Public Class DockTreeView

#Region "Các thuộc tính của Form"
    
#End Region

    Private Sub DockTreeView_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.TabText = m_ResourceManager.GetString("DockTreeViewCaption")
    End Sub


#Region "Hiển thị cây giao dịch"

    Public Sub GetTreeView(Optional ByVal IsShown As Boolean = True)
        Dim v_strObjMsg As String
        Dim v_nodeList As Xml.XmlNodeList
        Dim XmlDocument As New Xml.XmlDocument
        Dim v_int, v_intCount, v_intIndex As Integer
        Dim v_strFLDNAME As String = "", v_strValue As String = "", v_strPRID As String = ""
        Dim v_strCMDID As String, v_strCmdName As String = ""
        Dim v_strLast, v_strLev, v_strPreLev As String
        Dim v_strMenuKey, v_strMenuType, v_strMenuCode, v_strModCode, v_strAuthCode, v_strObjName, v_strAuthString As String
        Dim v_nodeParent As Node
        Dim v_tempNode As Node
        Dim v_blnFirst As Boolean = True

        Try
            If IsShown Then
                v_strObjMsg = CommonLibrary.BuildXMLObjMsg(Now.Date, m_BusLayer.CurrentTellerProfile.BranchId, Now.Date, _
                    m_BusLayer.CurrentTellerProfile.TellerId, CommonLibrary.gc_IsLocalMsg, CommonLibrary.gc_MsgTypeObj, _
                    OBJNAME_SY_AUTHENTICATION, CommonLibrary.gc_ActionInquiry, , m_BusLayer.CurrentTellerProfile.TellerId, "GetTreeMenu")

                pv_oProxy.Message(v_strObjMsg)


                XmlDocument.LoadXml(v_strObjMsg)
                v_nodeList = XmlDocument.SelectNodes("/ObjectMessage/ObjData")

                'stvMenu.Visible = False
                For v_intCount = 0 To v_nodeList.Count - 1
                    For v_int = 0 To v_nodeList.Item(v_intCount).ChildNodes.Count - 1
                        With v_nodeList.Item(v_intCount).ChildNodes(v_int)
                            v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                            v_strValue = .InnerText.ToString

                            Select Case Trim(v_strFLDNAME)
                                Case "PRID"
                                    v_strPRID = Trim(v_strValue)
                                Case "CMDNAME"
                                    If m_BusLayer.AppLanguage <> "EN" Then
                                        v_strCmdName = Trim(v_strValue)
                                    End If
                                Case "EN_CMDNAME"
                                    If m_BusLayer.AppLanguage = "EN" Then
                                        v_strCmdName = Trim(v_strValue)
                                    End If
                                Case "IMGINDEX"
                                    v_intIndex = CInt(Trim(v_strValue))
                                Case "CMDCODE"
                                    v_strCMDID = Trim(v_strValue)
                                Case "MENUCODE"
                                    v_strMenuCode = Trim(v_strValue)
                                Case "LEV"
                                    v_strLev = Trim(v_strValue)
                                Case "LAST"
                                    v_strLast = Trim(v_strValue)
                                Case "MODCODE"
                                    v_strModCode = Trim(v_strValue)
                                Case "OBJNAME"
                                    v_strObjName = Trim(v_strValue)
                                Case "MENUTYPE"
                                    v_strMenuType = Trim(v_strValue)
                                Case "AUTHCODE"
                                    v_strAuthCode = Trim(v_strValue)
                                Case "STRAUTH"
                                    v_strAuthString = Trim(v_strValue)
                            End Select
                        End With
                    Next v_int

                    If Not v_blnFirst Then
                        Dim v_strTempCMDID As String
                        Dim v_strTempLast As String
                        Dim v_strTempAuth As String
                        Dim v_strTempKey As String

                        v_strTempKey = v_tempNode.Key
                        v_strTempCMDID = Mid(v_strTempKey.Split("|")(0), 2)
                        v_strTempLast = Mid(v_strTempKey.Split("|")(0), 1, 1)

                        If v_strCMDID = v_strTempCMDID Then
                            If v_strTempLast = "Y" Then
                                v_strTempAuth = v_tempNode.Key.Split("|")(5)
                                v_strAuthString = MergeAuth(v_strTempAuth, v_strAuthString)
                                v_strMenuKey = v_strLast & v_strCMDID & "|" & v_strMenuType & "|" & v_strModCode & "|" & v_strObjName & "|" & v_strAuthCode & "|" & v_strAuthString & "|" & v_strMenuCode
                                v_tempNode.Key = v_strMenuKey
                            End If
                        Else
                            Dim v_node As New Node(v_strCmdName, v_intIndex)
                            Dim v_strName As String
                            If v_strLast = "Y" Then
                                v_strMenuKey = v_strLast & v_strCMDID & "|" & v_strMenuType & "|" & v_strModCode & "|" & v_strObjName & "|" & v_strAuthCode & "|" & v_strAuthString & "|" & v_strMenuCode
                                If v_strMenuType = "T" Then
                                    v_strName = v_strMenuCode & " - " & v_strCmdName
                                Else
                                    v_strName = v_strCmdName
                                End If
                                If v_strPreLev = "3" And v_strLev = "2" Then
                                    v_tempNode = AddTreeNode(v_nodeParent.ParentItem, v_strMenuKey, v_strName, v_strLast, 3)
                                Else
                                    If v_strPreLev = "2" And v_strMenuType = "R" Then
                                        v_tempNode = AddTreeNode(v_nodeParent.ParentItem, v_strMenuKey, v_strName, v_strLast, 3)
                                    Else
                                        v_tempNode = AddTreeNode(v_nodeParent, v_strMenuKey, v_strName, v_strLast, 3)
                                    End If
                                End If
                            Else
                                v_strMenuKey = v_strLast & v_strCMDID
                                If v_strLev = "1" Then
                                    v_node.Key = CStr(v_strMenuKey)
                                    v_node.ToolTipText = v_strCmdName
                                    v_node.Expanded = False
                                    Me.tvwTransact.Items(0).Items.Add(v_node)
                                    v_nodeParent = Me.tvwTransact.Items(0).Items(v_strMenuKey)
                                Else
                                    If v_strPreLev = "3" And v_strLev = "2" Then
                                        v_nodeParent = AddTreeNode(v_nodeParent.ParentItem, v_strMenuKey, v_strCmdName, v_strLast, 0)
                                    Else
                                        v_nodeParent = AddTreeNode(v_nodeParent, v_strMenuKey, v_strCmdName, v_strLast, 0)
                                    End If
                                End If
                                AddHandler v_nodeParent.DoubleClick, AddressOf Me.Treeview_Click
                                AddHandler v_nodeParent.KeyUp, AddressOf Me.Treeview_KeyUp
                                v_tempNode = v_nodeParent
                            End If

                            v_strPreLev = v_strLev
                        End If
                    Else
                        v_blnFirst = False
                        Dim v_node As New Node(v_strCmdName, v_intIndex)
                        Dim v_strName As String
                        If v_strLast = "Y" Then
                            v_strMenuKey = v_strLast & v_strCMDID & "|" & v_strMenuType & "|" & v_strModCode & "|" & v_strObjName & "|" & v_strAuthCode & "|" & v_strAuthString
                            If v_strMenuType = "T" Then
                                v_strName = v_strMenuCode & " - " & v_strCmdName
                            Else
                                v_strName = v_strCmdName
                            End If
                            If v_strPreLev = "3" And v_strLev = "2" Then
                                AddTreeNode(v_nodeParent.ParentItem, v_strMenuKey, v_strName, v_strLast, 3)
                            Else
                                AddTreeNode(v_nodeParent, v_strMenuKey, v_strName, v_strLast, 3)
                            End If
                        Else
                            v_strMenuKey = v_strLast & v_strCMDID
                            If v_strLev = "1" Then
                                v_node.Key = CStr(v_strMenuKey)
                                v_node.ToolTipText = v_strCmdName
                                v_node.Expanded = False
                                Me.tvwTransact.Items(0).Items.Add(v_node)
                                v_nodeParent = Me.tvwTransact.Items(0).Items(v_strMenuKey)
                            Else
                                If v_strPreLev = "3" And v_strLev = "2" Then
                                    v_nodeParent = AddTreeNode(v_nodeParent.ParentItem, v_strMenuKey, v_strCmdName, v_strLast, 0)
                                Else
                                    v_nodeParent = AddTreeNode(v_nodeParent, v_strMenuKey, v_strCmdName, v_strLast, 0)
                                End If
                            End If
                            AddHandler v_nodeParent.DoubleClick, AddressOf Me.Treeview_Click
                            AddHandler v_nodeParent.KeyUp, AddressOf Me.Treeview_KeyUp
                            v_tempNode = v_nodeParent
                        End If
                        v_strPreLev = v_strLev
                    End If
                Next v_intCount
                'stvMenu.Visible = True
            Else
                Me.tvwTransact.Items(0).Items.Clear()
            End If
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                        & "Error code: System error!" & vbNewLine _
                        & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(m_BusLayer.AppLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)

        End Try
    End Sub

    Private Function MergeAuth(ByVal pv_strAuth1 As String, ByVal pv_strAuth2 As String) As String
        Dim v_strAuth As String
        v_strAuth = IIf(Mid(pv_strAuth1, 1, 1) = Mid(pv_strAuth2, 1, 1), Mid(pv_strAuth2, 1, 1), "Y")
        v_strAuth &= IIf(Mid(pv_strAuth1, 2, 1) = Mid(pv_strAuth2, 2, 1), Mid(pv_strAuth2, 2, 1), "Y")
        v_strAuth &= IIf(Mid(pv_strAuth1, 3, 1) = Mid(pv_strAuth2, 3, 1), Mid(pv_strAuth2, 3, 1), "Y")
        v_strAuth &= IIf(Mid(pv_strAuth1, 4, 1) = Mid(pv_strAuth2, 4, 1), Mid(pv_strAuth2, 4, 1), "Y")
        v_strAuth &= IIf(Mid(pv_strAuth1, 5, 1) = Mid(pv_strAuth2, 5, 1), Mid(pv_strAuth2, 5, 1), "Y")
        v_strAuth &= IIf(Mid(pv_strAuth1, 6, 1) = Mid(pv_strAuth2, 6, 1), Mid(pv_strAuth2, 6, 1), "Y")
        v_strAuth &= IIf(Mid(pv_strAuth1, 7, 1) = Mid(pv_strAuth2, 7, 1), Mid(pv_strAuth2, 7, 1), "Y")
        Return v_strAuth
    End Function

    
    Private Function AddTreeNode(ByRef pv_nodeParent As Node, _
                                     ByVal pv_strKey As String, _
                                     ByVal pv_strName As String, _
                                     ByVal pv_strLast As String, _
                                     Optional ByVal pv_intImageIdx As Integer = 0) As Node
        'Create new node
        Dim v_node As New Node(pv_strName, pv_intImageIdx)

        v_node.Key = pv_strKey
        v_node.Text = pv_strName
        v_node.ToolTipText = pv_strName

        If pv_strLast = gc_IS_LAST_MENU Then
            AddHandler v_node.DoubleClick, AddressOf Me.Treeview_Click
            AddHandler v_node.KeyUp, AddressOf Me.Treeview_KeyUp
        Else
            v_node.Expanded = False
        End If

        pv_nodeParent.Items.Add(v_node)
        Return v_node
    End Function

#End Region

#Region "Sự kiện TreeView"

    Private Sub Treeview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim v_nodeKey As String
        Dim v_arrMenuKey() As String
        Dim v_strY As String
        v_nodeKey = CType(sender, Node).Key.ToString
        v_strY = Mid(v_nodeKey, 1, 1)
        If v_strY = "Y" Then
            v_arrMenuKey = Mid(v_nodeKey, 2).Split("|")
            ExecuteMenuFunction(v_arrMenuKey)
        Else
            CType(sender, Node).Expanded = Not CType(sender, Node).Expanded
            If CType(sender, Node).Expanded Then
                CType(sender, Node).ImageIndex = 1
            Else
                CType(sender, Node).ImageIndex = 0
            End If
        End If
    End Sub

    Private Sub Treeview_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        Dim v_nodeKey As String
        Dim v_arrMenuKey() As String
        Dim v_strY As String
        v_nodeKey = CType(sender, Node).Key.ToString
        v_strY = Mid(v_nodeKey, 1, 1)
        If e.KeyCode = Keys.Enter Then
            If v_strY = "Y" Then
                v_arrMenuKey = Mid(v_nodeKey, 2).Split("|")
                ExecuteMenuFunction(v_arrMenuKey)
            Else
                CType(sender, Node).Expanded = Not CType(sender, Node).Expanded
            End If
        End If
    End Sub


    Private Sub ExecuteMenuFunction(ByVal pv_arrMenuKey() As String)
        Dim v_strMenuType, v_strModCode, v_strObjName, v_strAuthCode, v_strAuthString, v_arrMenuKey() As String
        Dim v_strCMDID, v_strTLTXCD As String

        'Dim v_thread As New Threading.Thread(AddressOf ShowFormProcess)
        Dim v_oProcess As New Sats.AppCore.ProcessForm(Me)
        Try
            Dim v_strClause, v_strObjMsg As String
            v_oProcess.StartProcessForm()
            'Update mouse pointer
            Cursor = Cursors.WaitCursor

            v_arrMenuKey = pv_arrMenuKey
            If v_arrMenuKey.Length > 0 Then
                ' If node is not transaction
                v_strCMDID = v_arrMenuKey(0)
                v_strMenuType = v_arrMenuKey(1)
                v_strModCode = v_arrMenuKey(2)
                v_strObjName = v_arrMenuKey(3)
                v_strAuthCode = v_arrMenuKey(4)
                v_strAuthString = v_arrMenuKey(5)
                v_strTLTXCD = v_arrMenuKey(6)

                Select Case v_strMenuType
                    Case "R"    'Report

                        If CheckExitsForm(v_strObjName & v_strModCode) = False Then
                            Dim frm As New frmReportMaster(m_BusLayer.AppLanguage)
                            frm.Name = v_strObjName & v_strModCode
                            frm.BusDate = m_BusLayer.CurrentTellerProfile.BusDate
                            frm.TableName = "RPLIST"
                            frm.ModuleCode = v_strModCode
                            frm.AuthCode = v_strAuthCode
                            frm.AuthString = v_strAuthString
                            frm.IsLocalSearch = gc_IsInQueryNotLocalMsg
                            frm.SearchOnInit = True
                            frm.BranchId = m_BusLayer.CurrentTellerProfile.BranchId
                            frm.TellerId = m_BusLayer.CurrentTellerProfile.TellerId
                            frm.TellerType = m_BusLayer.CurrentTellerProfile.TellerType
                            frm.IpAddress = m_BusLayer.AppIpAddress
                            frm.WsName = m_BusLayer.AppWsName
                            frm.DockPanel = Me.DockPanel
                            frm.PassDate = m_BusLayer.CurrentTellerProfile.PassDate
                            frm.MemberFilter = m_BusLayer.CurrentTellerProfile.MemberFilter
                            frm.StockFilter = m_BusLayer.CurrentTellerProfile.StockFilter
                            frm.AllMember = m_BusLayer.CurrentTellerProfile.AllMember
                            frm.AllStock = m_BusLayer.CurrentTellerProfile.AllStock
                            frm.Client = mv_oLocal
                            frm.Proxy = pv_oProxy
                            'frm.ClientDataSet = m_BusLayer.mv_DataSet
                            'frm.Action = ExecuteFlag.Stoped
                            frm.InitDialog()
                            v_oProcess.StopProcessForm()
                            Application.DoEvents()
                            frm.Show()
                        End If
                    Case "M"    'Maintenance
                        If CheckExitsForm(v_strObjName) = False Then
                            If Mid(v_strObjName, 1, 6) = "TLLOG_" Then
                                v_strAuthCode = "YNNNYYN"
                                v_strAuthString = "NNNNY"
                                Dim frm As New frmTranMain(m_BusLayer.AppLanguage)
                                frm.Name = v_strObjName
                                frm.BusDate = m_BusLayer.CurrentTellerProfile.BusDate
                                frm.TableName = v_strObjName
                                frm.ModuleCode = v_strModCode
                                frm.AuthCode = v_strAuthCode
                                frm.AuthString = v_strAuthString
                                frm.IsLocalSearch = gc_IsInQueryNotLocalMsg
                                frm.SearchOnInit = False
                                frm.BranchId = m_BusLayer.CurrentTellerProfile.BranchId
                                frm.TellerId = m_BusLayer.CurrentTellerProfile.TellerId
                                frm.TellerType = m_BusLayer.CurrentTellerProfile.TellerType
                                frm.IpAddress = m_BusLayer.AppIpAddress
                                frm.WsName = m_BusLayer.AppWsName
                                frm.MemberFilter = m_BusLayer.CurrentTellerProfile.MemberFilter
                                frm.StockFilter = m_BusLayer.CurrentTellerProfile.StockFilter
                                frm.AllMember = m_BusLayer.CurrentTellerProfile.AllMember
                                frm.AllStock = m_BusLayer.CurrentTellerProfile.AllStock
                                frm.DockPanel = Me.DockPanel
                                frm.PassDate = m_BusLayer.CurrentTellerProfile.PassDate
                                frm.CaptionText = "Quản lý giao dịch phân hệ " & v_strModCode
                                frm.F_Task = frmTask
                                frm.Client = mv_oLocal
                                frm.Proxy = pv_oProxy
                                frm.InitDialog()
                                v_oProcess.StopProcessForm()
                                Application.DoEvents()
                                frm.Show()
                            Else
                                Dim frm As New frmSearchMaster(m_BusLayer.AppLanguage)
                                frm.Name = v_strObjName
                                frm.BusDate = m_BusLayer.CurrentTellerProfile.BusDate
                                frm.TableName = v_strObjName
                                frm.ModuleCode = v_strModCode
                                frm.AuthCode = v_strAuthCode
                                frm.AuthString = v_strAuthString
                                frm.IsLocalSearch = gc_IsInQueryNotLocalMsg
                                frm.SearchOnInit = False
                                frm.VsdBrid = m_BusLayer.CurrentTellerProfile.VsdBrid
                                frm.VsdBrid2 = m_BusLayer.CurrentTellerProfile.VsdBrid2
                                frm.BranchId = m_BusLayer.CurrentTellerProfile.BranchId
                                frm.TellerId = m_BusLayer.CurrentTellerProfile.TellerId
                                frm.TellerType = m_BusLayer.CurrentTellerProfile.TellerType
                                frm.IpAddress = m_BusLayer.AppIpAddress
                                frm.WsName = m_BusLayer.AppWsName
                                frm.MemberFilter = m_BusLayer.CurrentTellerProfile.MemberFilter
                                frm.StockFilter = m_BusLayer.CurrentTellerProfile.StockFilter
                                frm.AllMember = m_BusLayer.CurrentTellerProfile.AllMember
                                frm.AllStock = m_BusLayer.CurrentTellerProfile.AllStock
                                frm.DockPanel = Me.DockPanel
                                frm.PassDate = m_BusLayer.CurrentTellerProfile.PassDate
                                frm.Client = mv_oLocal
                                frm.Proxy = pv_oProxy
                                frm.InitDialog()
                                v_oProcess.StopProcessForm()
                                Application.DoEvents()
                                frm.Show()
                            End If
                        End If
                    Case "A"    'Special  
                        Select Case v_strObjName
                            Case "ENDOFDAY", "BEGINOFDAY"
                                ' tuanta 
                                ' Purpose : Running the end or the begin of day  
                                ' date : 13/02/2009
                                If Me.CheckExitsForm(v_strObjName) = False Then
                                    Dim frm As New frmBatch()
                                    frm.Name = v_strObjName
                                    frm.BEGINOREND = v_strObjName
                                    frm.BranchId = m_BusLayer.CurrentTellerProfile.BranchId
                                    frm.TellerId = m_BusLayer.CurrentTellerProfile.TellerId
                                    frm.UserLanguage = gc_LANG_VIETNAMESE
                                    frm.BusDate = m_BusLayer.CurrentTellerProfile.BusDate
                                    frm.Proxy = pv_oProxy
                                    v_oProcess.StopProcessForm()
                                    Application.DoEvents()
                                    frm.ShowDialog()
                                End If
                                ' end tuanta
                                'Hanm5 sửa:  bắt đầu từ đây
                            Case "EXPORTDATA"
                                If Me.CheckExitsForm(v_strObjName) = False Then
                                    Dim frm As New frmExportData()
                                    frm.Name = v_strObjName
                                    frm.UserLanguage = m_BusLayer.AppLanguage
                                    frm.BusDate = m_BusLayer.CurrentTellerProfile.BusDate
                                    'frm.TableName = v_strObjName
                                    'frm.ModuleCode = v_strModCode
                                    'frm.AuthCode = v_strAuthCode
                                    'frm.AuthString = v_strAuthString
                                    'frm.IsLocalSearch = gc_IsLocalMsg
                                    'frm.SearchOnInit = False
                                    frm.BranchId = m_BusLayer.CurrentTellerProfile.BranchId
                                    frm.TellerId = m_BusLayer.CurrentTellerProfile.TellerId
                                    'frm.TellerType = m_BusLayer.CurrentTellerProfile.TellerType
                                    'frm.IpAddress = m_BusLayer.AppIpAddress
                                    'frm.WsName = m_BusLayer.AppWsName
                                    'frm.DockPanel = Me.DockPanel
                                    'frm.STRAUTH = v_strAuthString
                                    v_oProcess.StopProcessForm()
                                    frm.Proxy = pv_oProxy
                                    Application.DoEvents()
                                    frm.ShowDialog()
                                    'Hanm5 sửa:  Kết thúc ở đây
                                    'Thonm: Dong bo du lieu tu HOST ve BDS

                                End If
                            Case "EXPORTEXCELFORM"
                                If Me.CheckExitsForm(v_strObjName) = False Then
                                    Dim frm As New frmExportExcelForm()
                                    frm.Name = v_strObjName
                                    frm.UserLanguage = m_BusLayer.AppLanguage
                                    'frm.BusDate = m_BusLayer.CurrentTellerProfile.BusDate
                                    'frm.TableName = v_strObjName
                                    'frm.ModuleCode = v_strModCode
                                    'frm.AuthCode = v_strAuthCode
                                    'frm.AuthString = v_strAuthString
                                    'frm.IsLocalSearch = gc_IsLocalMsg
                                    'frm.SearchOnInit = False
                                    frm.BranchId = m_BusLayer.CurrentTellerProfile.BranchId
                                    frm.TellerId = m_BusLayer.CurrentTellerProfile.TellerId
                                    frm.Proxy = pv_oProxy
                                    'frm.TellerType = m_BusLayer.CurrentTellerProfile.TellerType
                                    'frm.IpAddress = m_BusLayer.AppIpAddress
                                    'frm.WsName = m_BusLayer.AppWsName
                                    'frm.DockPanel = Me.DockPanel
                                    'frm.STRAUTH = v_strAuthString
                                    v_oProcess.StopProcessForm()
                                    Application.DoEvents()
                                    frm.ShowDialog()
                                End If
                            Case "SYSVARSETTING"
                                If Me.CheckExitsForm(v_strObjName) = False Then
                                    Dim frm As New frmSysVarSetting()
                                    frm.Name = v_strObjName
                                    frm.UserLanguage = m_BusLayer.AppLanguage
                                    'frm.BusDate = m_BusLayer.CurrentTellerProfile.BusDate
                                    'frm.TableName = v_strObjName
                                    'frm.ModuleCode = v_strModCode
                                    'frm.AuthCode = v_strAuthCode
                                    'frm.AuthString = v_strAuthString
                                    'frm.IsLocalSearch = gc_IsLocalMsg
                                    'frm.SearchOnInit = False
                                    frm.BranchId = m_BusLayer.CurrentTellerProfile.BranchId
                                    frm.TellerId = m_BusLayer.CurrentTellerProfile.TellerId
                                    frm.Proxy = pv_oProxy
                                    'frm.TellerType = m_BusLayer.CurrentTellerProfile.TellerType
                                    'frm.IpAddress = m_BusLayer.AppIpAddress
                                    'frm.WsName = m_BusLayer.AppWsName
                                    'frm.DockPanel = Me.DockPanel
                                    'frm.STRAUTH = v_strAuthString
                                    v_oProcess.StopProcessForm()
                                    Application.DoEvents()
                                    frm.ShowDialog()
                                End If
                                'Hanm5 sửa:  Kết thúc ở đây

                                'Thonm: Dong bo du lieu tu HOST ve BDS

                            Case "GROUPUSRE"
                                If Me.CheckExitsForm(v_strObjName) = False Then
                                    Dim frm As New frmGroupUsers()
                                    frm.Name = v_strObjName
                                    frm.UserLanguage = m_BusLayer.AppLanguage
                                    'frm.BusDate = m_BusLayer.CurrentTellerProfile.BusDate
                                    'frm.TableName = v_strObjName
                                    'frm.ModuleCode = v_strModCode
                                    'frm.AuthCode = v_strAuthCode
                                    'frm.AuthString = v_strAuthString
                                    'frm.IsLocalSearch = gc_IsLocalMsg
                                    'frm.SearchOnInit = False
                                    frm.BranchId = m_BusLayer.CurrentTellerProfile.BranchId
                                    frm.TellerId = m_BusLayer.CurrentTellerProfile.TellerId
                                    'frm.TellerType = m_BusLayer.CurrentTellerProfile.TellerType
                                    'frm.IpAddress = m_BusLayer.AppIpAddress
                                    'frm.WsName = m_BusLayer.AppWsName
                                    'frm.DockPanel = Me.DockPanel
                                    frm.STRAUTH = v_strAuthString
                                    frm.Proxy = pv_oProxy
                                    v_oProcess.StopProcessForm()
                                    Application.DoEvents()
                                    frm.ShowDialog()
                                    'Hanm5 sửa:  Kết thúc ở đây
                                    'Thonm: Dong bo du lieu tu HOST ve BDS
                                End If
                                'ADD by thonm
                            Case "IMPFILE"
                                Dim frm As New frmInQueryImpFile(m_BusLayer.AppLanguage)
                                frm.Name = v_strObjName
                                frm.TableName = v_strObjName
                                'frm.ObjectName = v_strObjName
                                frm.ModuleCode = v_strModCode
                                frm.AuthCode = v_strAuthCode
                                frm.AuthString = v_strAuthString
                                frm.BranchId = m_BusLayer.CurrentTellerProfile.BranchId
                                frm.TellerId = m_BusLayer.CurrentTellerProfile.TellerId
                                frm.DockPanel = Me.DockPanel
                                frm.UserLanguage = m_BusLayer.AppLanguage
                                frm.IsLocalSearch = gc_IsLocalMsg
                                frm.SearchOnInit = False
                                frm.Proxy = pv_oProxy
                                frm.InitDialog()
                                frm.Show()
                            Case "MFTLTXCD"
                                If Me.CheckExitsForm(v_strObjName) = False Then
                                    Dim frm As New frmAddMFToTLTX
                                    frm.Name = v_strObjName
                                    frm.UserLanguage = m_BusLayer.AppLanguage
                                    'frm.BusDate = m_BusLayer.CurrentTellerProfile.BusDate
                                    'frm.TableName = v_strObjName
                                    'frm.ModuleCode = v_strModCode
                                    'frm.AuthCode = v_strAuthCode
                                    'frm.AuthString = v_strAuthString
                                    'frm.IsLocalSearch = gc_IsLocalMsg
                                    'frm.SearchOnInit = False
                                    frm.BranchId = m_BusLayer.CurrentTellerProfile.BranchId
                                    frm.TellerId = m_BusLayer.CurrentTellerProfile.TellerId
                                    'frm.TellerType = m_BusLayer.CurrentTellerProfile.TellerType
                                    'frm.IpAddress = m_BusLayer.AppIpAddress
                                    'frm.WsName = m_BusLayer.AppWsName
                                    'frm.DockPanel = Me.DockPanel
                                    frm.STRAUTH = v_strAuthString
                                    frm.Proxy = pv_oProxy
                                    v_oProcess.StopProcessForm()
                                    Application.DoEvents()
                                    frm.ShowDialog()
                                    'Hanm5 sửa:  Kết thúc ở đây
                                    'Thonm: Dong bo du lieu tu HOST ve BDS
                                End If
                        End Select
                    Case "T"
                        v_oProcess.StopProcessForm()
                        Application.DoEvents()
                        ShowTransact(v_strTLTXCD)
                    Case Else

                End Select
            End If
        Catch ex As Exception
            v_oProcess.StopProcessForm()
            Application.DoEvents()
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                        & "Error code: System error!" & vbNewLine _
                        & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(m_BusLayer.AppLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        Finally
            v_oProcess.StopProcessForm()
            Application.DoEvents()
            'Update mouse pointer
            Cursor = Cursors.Default
        End Try
    End Sub


    Sub ShowTransact(ByVal pv_strTLTXCD As String)
        'Dim v_thread As New Threading.Thread(AddressOf ShowFormProcess)
        'v_thread.Start()
        Dim v_oProcess As New Sats.AppCore.ProcessForm(Me)
        Try
            v_oProcess.StartProcessForm()
            Dim v_strTLTXCD As String = "", v_strCmdInquiry As String = ""
            Dim v_strTXCODE As String = "", v_strModeCode As String = ""
            Dim v_lngErrCode As Long
            Dim frm As frmTransactionMaster
            Dim v_xmlDocument As New System.Xml.XmlDocument
            Dim v_nodeList As Xml.XmlNodeList
            Dim v_strAllowObjMsg As String
            Dim v_strObjMsg As String
            Dim v_strFLDNAME As String
            Dim v_strValue As String
            Dim v_strBrid As String

            'Check transaction allowed of current teller 
            'v_strTLTXCD = txtTrans.VirtualTextBox.Text
            v_strTLTXCD = Mid(pv_strTLTXCD, 3)
            If Len(Trim(v_strTLTXCD)) <> 4 Then
                v_oProcess.StopProcessForm()
                Application.DoEvents()
                Exit Sub
            End If

            Me.Cursor = Cursors.WaitCursor

            Dim v_strTlType As String
            v_strTlType = m_BusLayer.CurrentTellerProfile.TellerType
            'bangpv: thêm lọc theo chi nhánh 
            v_strBrid = m_BusLayer.CurrentTellerProfile.BranchId
            'end bangpv
            'v_strAllowObjMsg = BuildXMLObjMsg(, m_BusLayer.CurrentTellerProfile.BranchId, , m_BusLayer.CurrentTellerProfile.TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SY_AUTHENTICATION, gc_ActionInquiry, , v_strTlType & "|" & v_strTLTXCD, "CheckTransAllow", )

            'Dim v_bds As New BDSChannel.BDSDelivery
            'v_lngErrCode = v_bds.Message(v_strAllowObjMsg)
            If ERR_SYSTEM_OK = ERR_SYSTEM_OK Then
                v_strCmdInquiry = "SELECT c.modcode FROM TLTX a, " _
                & " ( " _
                & " select c.tltxcd from tlauth c where c.status =0 and c.deleted =0 and c.authtype = 'U' and c.authid = '" & m_BusLayer.CurrentTellerProfile.TellerId & "' and c.tltype = '0' " _
                & "AND c.BRID = '" & v_strBrid & "' " _
                & " union" _
                & " select c.tltxcd from tlauth c where c.status =0 and c.deleted =0 and c.authtype = 'G' AND c.brid ='" & v_strBrid & "' " _
                & " and c.authid in ( select grpid from tlgrpusers d where d.tlid = '" & m_BusLayer.CurrentTellerProfile.TellerId & "' and d.status =0 and d.deleted =0 ) and c.tltype = '0'" _
                & " ) b, appmodules c " _
                & " where a.status =0 and a.deleted =0 and a.tltxcd = b.tltxcd and substr(a.tltxcd,1,2) = c.txcode and c.deleted=0 and c.status = 0 " _
                & " and a.TLTXCD='" & v_strTLTXCD & "'"

                v_strObjMsg = BuildXMLObjMsg(, , , m_BusLayer.CurrentTellerProfile.TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SY_SEARCHFLD, gc_ActionInquiry, v_strCmdInquiry, , )

                Dim v_lngError As Long = pv_oProxy.Message(v_strObjMsg)
                If v_lngError <> ERR_SYSTEM_OK Then
                    v_oProcess.StopProcessForm()
                    Application.DoEvents()
                    MsgBox("Đã có lỗi xẩy ra, hãy liên hệ với quản tri hệ thống để được giúp đỡ", MsgBoxStyle.Critical, gc_ApplicationTitle)
                    Exit Sub
                End If
                v_xmlDocument.LoadXml(v_strObjMsg)
                v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")
                If v_nodeList.Count = 0 Then
                    'Neu khong tim thay ma giao dich
                    v_oProcess.StopProcessForm()
                    Application.DoEvents()
                    MessageBox.Show("Bạn không có quyền truy nhập giao dịch này !", gc_ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Me.Cursor = Cursors.Default
                    Exit Sub
                End If

                For i As Integer = 0 To v_nodeList.Count - 1
                    For j As Integer = 0 To v_nodeList.Item(i).ChildNodes.Count - 1
                        With v_nodeList.Item(i).ChildNodes(j)
                            v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                            v_strValue = .InnerText.ToString
                            Select Case Trim(v_strFLDNAME)
                                Case "MODCODE"
                                    v_strModeCode = Trim(v_strValue)
                                    Exit For
                            End Select
                        End With
                    Next
                Next

                If Len(Trim(v_strModeCode)) > 0 Then
                    If Not CheckExitsForm("frm" & v_strTLTXCD) Then
                        frm = New frmTransactionMaster
                        frm.Name = "frm" & v_strTLTXCD
                        frm.UserLanguage = m_BusLayer.AppLanguage
                        frm.ObjectName = v_strTLTXCD
                        frm.ModuleCode = v_strModeCode
                        frm.LocalObject = gc_IsNotLocalMsg
                        frm.BranchId = m_BusLayer.CurrentTellerProfile.BranchId
                        frm.TellerId = m_BusLayer.CurrentTellerProfile.TellerId
                        frm.IpAddress = m_BusLayer.AppIpAddress
                        frm.WsName = m_BusLayer.AppWsName
                        frm.BusDate = m_BusLayer.CurrentTellerProfile.BusDate
                        frm.MemberFilter = m_BusLayer.CurrentTellerProfile.MemberFilter
                        frm.StockFilter = m_BusLayer.CurrentTellerProfile.StockFilter
                        frm.AllMember = m_BusLayer.CurrentTellerProfile.AllMember
                        frm.AllStock = m_BusLayer.CurrentTellerProfile.AllStock
                        frm.BRCODE = m_BusLayer.CurrentTellerProfile.BranchName
                        frm.TellerName = m_BusLayer.CurrentTellerProfile.TellerName
                        frm.PassDate = m_BusLayer.CurrentTellerProfile.PassDate
                        frm.VSDBRID = m_BusLayer.CurrentTellerProfile.VsdBrid
                        frm.VSDBRID2 = m_BusLayer.CurrentTellerProfile.VsdBrid2
                        frm.Proxy = pv_oProxy
                        frm.Client = mv_oLocal
                        frm.Show(Me.DockPanel)
                        frm.Focus()
                    End If
                Else
                    'Thông báo giao dịch phải qua tra cứu
                    v_oProcess.StopProcessForm()
                    Application.DoEvents()
                    MsgBox(IIf(m_BusLayer.AppLanguage = "VN", "Đây không phải là mã giao dịch trực tiếp !", "This is not a direct transaction!"), MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, Me.Text)
                    Me.Cursor = Cursors.Default
                End If
            Else
                'Thông báo lỗi
                Dim v_strErrorSource As String = "", v_strErrorMessage As String = ""
                GetErrorFromMessage(v_strAllowObjMsg, v_strErrorSource, v_lngErrCode, v_strErrorMessage)
                Windows.Forms.Cursor.Current = Cursors.Default
                v_oProcess.StopProcessForm()
                Application.DoEvents()
                MsgBox(v_strErrorMessage, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, Me.Text)
                Exit Sub
            End If
            v_oProcess.StopProcessForm()
            Application.DoEvents()
            Me.Cursor = Cursors.Default
        Catch ex As Exception
            Windows.Forms.Cursor.Current = Cursors.Default
            v_oProcess.StopProcessForm()
            Application.DoEvents()
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                        & "Error code: System error!" & vbNewLine _
                        & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(m_BusLayer.AppLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        Finally
            v_oProcess = Nothing
        End Try
    End Sub
#End Region

    Private Function CheckExitsForm(ByVal v_strName) As Boolean
        Dim o_frm As Sats.WinFormsUI.Docking.DockContent
        Dim v_blnIsExits As Boolean = False

        For Each o_frm In Me.PanelPane.DockPanel.Documents
            If o_frm.Name = v_strName Then
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
