Imports System.IO
Imports Microsoft.Win32
Imports System.Threading
Imports Xceed.SmartUI.Controls.TreeView
Imports Sats.CommonLibrary
Imports Sats.AppCore
Imports Sats.AppCore.GridEx
Imports Sats.AppCore.ComboBoxEx

Public Class frmFuncAssign
    Private mv_oProxy As BDSChannel.BDSDelivery

    Public Property Proxy() As BDSChannel.BDSDelivery
        Get
            Return mv_oProxy
        End Get
        Set(ByVal value As BDSChannel.BDSDelivery)
            mv_oProxy = value
        End Set
    End Property
#Region " Khai báo hằng & biến "
    Private mv_ResourceManager As Resources.ResourceManager
    Private mv_strLanguage As String
    Private mv_intExecFlag As Integer
    Private mv_strModuleCode As String
    Private mv_strObjectName As String
    Private mv_strTableName As String
    Private mv_strBranchId As String
    Private mv_strTellerId As String
    Private mv_strTellerName As String
    Private mv_strUserId As String
    Private mv_strUserName As String
    Private mv_strGroupId As String
    Private mv_strGroupName As String
    Private mv_strAssignType As String
    Private mv_strLocalObject As String
    Private mv_strFuncAuth As String
    Private mv_strFuncGrpAuth As String
    Private mv_strMIInUser As String
    Private mv_strMIOutUser As String
    Private mv_strBrid As String
    Private mv_blnBridStatus As Boolean
    Dim hParentsFilter As New Hashtable
    Dim hFunctionFilter As New Hashtable
    Dim hFunctionGrpFilter As New Hashtable
    Dim hFunctionTellerFilter As New Hashtable

    'Private mv_BDSDelivery As BDSChannel.BDSDelivery

#End Region

#Region " Khai báo thuộc tính "

    Public Property ModuleCode() As String
        Get
            Return mv_strModuleCode
        End Get
        Set(ByVal Value As String)
            mv_strModuleCode = Value
        End Set
    End Property

    Public Property ObjectName() As String
        Get
            Return mv_strObjectName
        End Get
        Set(ByVal Value As String)
            mv_strObjectName = Value
        End Set
    End Property

    Public Property TableName() As String
        Get
            Return mv_strTableName
        End Get
        Set(ByVal Value As String)
            mv_strTableName = Value
        End Set
    End Property

    Public Property BranchId() As String
        Get
            Return mv_strBranchId
        End Get
        Set(ByVal Value As String)
            mv_strBranchId = Value
        End Set
    End Property

    Public Property TellerId() As String
        Get
            Return mv_strTellerId
        End Get
        Set(ByVal Value As String)
            mv_strTellerId = Value
        End Set
    End Property

    Public Property TellerName() As String
        Get
            Return mv_strTellerName
        End Get
        Set(ByVal Value As String)
            mv_strTellerName = Value
        End Set
    End Property

    Public Property UserId() As String
        Get
            Return mv_strUserId
        End Get
        Set(ByVal Value As String)
            mv_strUserId = Value
        End Set
    End Property

    Public Property UserName() As String
        Get
            Return mv_strUserName
        End Get
        Set(ByVal Value As String)
            mv_strUserName = Value
        End Set
    End Property

    Public Property GroupId() As String
        Get
            Return mv_strGroupId
        End Get
        Set(ByVal Value As String)
            mv_strGroupId = Value
        End Set
    End Property

    Public Property GroupName() As String
        Get
            Return mv_strGroupName
        End Get
        Set(ByVal Value As String)
            mv_strGroupName = Value
        End Set
    End Property

    Public Property LocalObject() As String
        Get
            Return mv_strLocalObject
        End Get
        Set(ByVal Value As String)
            mv_strLocalObject = Value
        End Set
    End Property

    Public Property UserLanguage() As String
        Get
            Return mv_strLanguage
        End Get
        Set(ByVal Value As String)
            mv_strLanguage = Value
        End Set
    End Property

    Public Property ResourceManager() As Resources.ResourceManager
        Get
            Return mv_ResourceManager
        End Get
        Set(ByVal Value As Resources.ResourceManager)
            mv_ResourceManager = Value
        End Set
    End Property

    Public Property ExeFlag() As Integer
        Get
            Return mv_intExecFlag
        End Get
        Set(ByVal Value As Integer)
            mv_intExecFlag = Value
        End Set
    End Property

    Public Property AssignType() As String
        Get
            Return mv_strAssignType
        End Get
        Set(ByVal Value As String)
            mv_strAssignType = Value
        End Set
    End Property
#End Region

#Region "Hiển thị cây chức năng"
    Private Function AddTreeNode(ByRef pv_nodeParent As Node, _
                                         ByVal pv_strKey As String, _
                                         ByVal pv_strName As String, _
                                         ByVal pv_strLast As String, _
                                         Optional ByVal pv_intImageIdx As Integer = 0) As Node
        Try
            'Create new node
            Dim v_node As New Node(pv_strName, pv_intImageIdx)

            v_node.Key = pv_strKey
            v_node.Text = pv_strName
            v_node.ToolTipText = pv_strName
            If pv_strLast = gc_IS_LAST_MENU Then
                AddHandler v_node.Click, AddressOf Me.TreeView_Click
                AddHandler v_node.KeyUp, AddressOf Me.Treeview_KeyUp
            Else
                v_node.Expanded = False
            End If
            'Add node to menu tree
            pv_nodeParent.Items.Add(v_node)
            Return v_node
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Sub GetTreeView(Optional ByVal IsShown As Boolean = True)
        Dim v_strObjMsg As String
        Dim v_nodeList As Xml.XmlNodeList
        Dim XmlDocument As New Xml.XmlDocument
        Dim v_int, v_intCount, v_intIndex As Integer
        Dim v_strFLDNAME As String = "", v_strValue As String = "", v_strPRID As String = ""
        Dim v_strCMDID As String = "", v_strCmdName As String = ""
        Dim v_strLast, v_strLev, v_strPreLev As String
        Dim v_strMenuKey, v_strMenuType, v_strModCode, v_strAuthCode, v_strObjName, v_strAuthString As String
        Dim v_nodeParent, v_tempNode As Node
        Dim v_blnFirst As Boolean = True

        Try
            If IsShown Then
                'bangpv: lấy brid của người sử dụng
                'v_strObjMsg = CommonLibrary.BuildXMLObjMsg(Now.Date, m_BusLayer.CurrentTellerProfile.BranchId, Now.Date, _
                '    m_BusLayer.CurrentTellerProfile.TellerId, CommonLibrary.gc_IsLocalMsg, CommonLibrary.gc_MsgTypeObj, _
                '    OBJNAME_SY_AUTHENTICATION, CommonLibrary.gc_ActionInquiry, , m_BusLayer.CurrentTellerProfile.TellerId, "GetFunctionTreeMenu")
                'm_BusLayer.BusSystemMessage(v_strObjMsg)
                v_strObjMsg = CommonLibrary.BuildXMLObjMsg(Now.Date, mv_strBrid, Now.Date, _
                  m_BusLayer.CurrentTellerProfile.TellerId, CommonLibrary.gc_IsLocalMsg, CommonLibrary.gc_MsgTypeObj, _
                  OBJNAME_SY_AUTHENTICATION, CommonLibrary.gc_ActionInquiry, , m_BusLayer.CurrentTellerProfile.TellerId, "GetFunctionTreeMenu")
                m_BusLayer.BusSystemMessage(v_strObjMsg)

                'end bangpv
                XmlDocument.LoadXml(v_strObjMsg)
                v_nodeList = XmlDocument.SelectNodes("/ObjectMessage/ObjData")
                'bangpv
                Me.stvTran.Items(0).Items.Clear()
                'end bangpv
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

                        v_strTempLast = Mid(v_strTempKey.Split("|")(0), 1, 1)
                        If v_strTempLast = "Y" Then
                            v_strTempCMDID = v_strTempKey.Split("|")(2)
                        Else
                            v_strTempCMDID = Mid(v_strTempKey.Split("|")(0), 2)
                        End If
                        If v_strCMDID = v_strTempCMDID Then
                            If v_strTempLast = "Y" Then
                                v_strTempAuth = v_tempNode.Key.Split("|")(4)
                                v_strAuthString = MergeAuth(v_strTempAuth, v_strAuthString)
                                v_strMenuKey = v_strLast & "|" & v_strLev & "|" & v_strCMDID & "|" & v_strAuthCode & "|" & v_strAuthString
                                v_tempNode.Key = v_strMenuKey
                            End If
                        Else

                            Dim v_node As New Node(v_strCmdName, v_intIndex)

                            If v_strLast = "Y" Then
                                v_strMenuKey = v_strLast & "|" & v_strLev & "|" & v_strCMDID & "|" & v_strAuthCode & "|" & v_strAuthString
                                If v_strPreLev = "3" And v_strLev = "2" Then
                                    v_tempNode = AddTreeNode(v_nodeParent.ParentItem, v_strMenuKey, v_strCmdName, v_strLast, 3)
                                Else
                                    v_tempNode = AddTreeNode(v_nodeParent, v_strMenuKey, v_strCmdName, v_strLast, 3)
                                End If
                            Else
                                v_strMenuKey = v_strLast & v_strCMDID
                                If v_strLev = "1" Then
                                    v_node.Key = CStr(v_strMenuKey)
                                    v_node.ToolTipText = v_strCmdName
                                    v_node.Expanded = False
                                    Me.stvTran.Items(0).Items.Add(v_node)
                                    v_nodeParent = Me.stvTran.Items(0).Items(v_strMenuKey)
                                Else
                                    If v_strPreLev = "3" And v_strLev = "2" Then
                                        v_nodeParent = AddTreeNode(v_nodeParent.ParentItem, v_strMenuKey, v_strCmdName, v_strLast, 0)
                                    Else
                                        v_nodeParent = AddTreeNode(v_nodeParent, v_strMenuKey, v_strCmdName, v_strLast, 0)
                                    End If
                                End If
                                AddHandler v_nodeParent.Click, AddressOf Me.TreeView_Click
                                AddHandler v_nodeParent.KeyUp, AddressOf Me.TreeView_KeyUp
                                v_tempNode = v_nodeParent
                            End If

                            v_strPreLev = v_strLev
                        End If
                    Else
                        v_blnFirst = False
                        Dim v_node As New Node(v_strCmdName, v_intIndex)
                        Dim v_strName As String
                        If v_strLast = "Y" Then
                            v_strMenuKey = v_strLast & "|" & v_strLev & "|" & v_strCMDID & "|" & v_strAuthCode & "|" & v_strAuthString

                            If v_strPreLev = "3" And v_strLev = "2" Then
                                AddTreeNode(v_nodeParent.ParentItem, v_strMenuKey, v_strCmdName, v_strLast, 3)
                            Else
                                AddTreeNode(v_nodeParent, v_strMenuKey, v_strCmdName, v_strLast, 3)
                            End If
                        Else
                            v_strMenuKey = v_strLast & v_strCMDID
                            If v_strLev = "1" Then
                                v_node.Key = CStr(v_strMenuKey)
                                v_node.ToolTipText = v_strCmdName
                                v_node.Expanded = False

                                Me.stvTran.Items(0).Items.Add(v_node)
                                v_nodeParent = Me.stvTran.Items(0).Items(v_strMenuKey)
                            Else
                                If v_strPreLev = "3" And v_strLev = "2" Then
                                    v_nodeParent = AddTreeNode(v_nodeParent.ParentItem, v_strMenuKey, v_strCmdName, v_strLast, 0)
                                Else
                                    v_nodeParent = AddTreeNode(v_nodeParent, v_strMenuKey, v_strCmdName, v_strLast, 0)
                                End If
                            End If
                            AddHandler v_nodeParent.Click, AddressOf Me.TreeView_Click
                            AddHandler v_nodeParent.KeyUp, AddressOf Me.TreeView_KeyUp
                            v_tempNode = v_nodeParent
                        End If
                        v_strPreLev = v_strLev
                    End If
                    'Dim v_node As New Node(v_strCmdName, v_intIndex)

                    'If v_strLast = "Y" Then
                    '    v_strMenuKey = v_strLast & "|" & v_strLev & "|" & v_strCMDID & "|" & v_strAuthCode & "|" & v_strAuthString
                    '    If v_strPreLev = "3" And v_strLev = "2" Then
                    '        AddTreeNode(v_nodeParent.ParentItem, v_strMenuKey, v_strCmdName, v_strLast, 3)
                    '    Else
                    '        AddTreeNode(v_nodeParent, v_strMenuKey, v_strCmdName, v_strLast, 3)
                    '    End If
                    'Else
                    '    v_strMenuKey = v_strLast & "|" & v_strLev & "|" & v_strCMDID
                    '    If v_strLev = "1" Then
                    '        v_node.Key = CStr(v_strMenuKey)
                    '        v_node.ToolTipText = v_strCmdName
                    '        v_node.Expanded = False
                    '        Me.stvTran.Items(0).Items.Add(v_node)
                    '        v_nodeParent = Me.stvTran.Items(0).Items(v_strMenuKey)
                    '    Else
                    '        If v_strPreLev = "3" And v_strLev = "2" Then
                    '            v_nodeParent = AddTreeNode(v_nodeParent.ParentItem, v_strMenuKey, v_strCmdName, v_strLast, 0)
                    '        Else
                    '            v_nodeParent = AddTreeNode(v_nodeParent, v_strMenuKey, v_strCmdName, v_strLast, 0)
                    '        End If
                    '    End If
                    '    AddHandler v_nodeParent.Click, AddressOf Me.TreeView_Click
                    '    AddHandler v_nodeParent.KeyUp, AddressOf Me.TreeView_KeyUp
                    'End If

                    'v_strPreLev = v_strLev

                Next v_intCount
                'stvMenu.Visible = True
            Else
                Me.stvTran.Items(0).Items.Clear()
            End If
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                           & "Error code: System error!" & vbNewLine _
                           & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Sub

    Private Sub TreeView_Click(ByVal sender As Object, ByVal e As Xceed.SmartUI.SmartItemClickEventArgs)
        Dim pv_treeNode As Node
        'Update mouse pointer
        Cursor = Cursors.WaitCursor
        Try
            pv_treeNode = e.Item
            If pv_treeNode.Key <> "" Then
                If pv_treeNode.Key.Split("|")(0) = "Y" Then
                    lbFunc.Text = pv_treeNode.Text
                    ShowAccessRight(pv_treeNode)
                Else
                    DisableAssignment()
                End If
            Else
                lbFunc.Text = String.Empty
                DisableAssignment()
            End If

        Catch ex As Exception
            Throw ex
        End Try

        'Update mouse pointer
        Cursor = Cursors.Default
    End Sub

    Private Sub TreeView_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        Dim v_node As Node
        v_node = CType(sender, Node)

        Select Case e.KeyCode
            Case Keys.Up, Keys.Down
                If v_node.Key <> "" Then
                    If v_node.Key.Split("|")(0) = "Y" Then
                        lbFunc.Text = v_node.Text
                        ShowAccessRight(v_node)
                    Else
                        DisableAssignment()
                    End If
                Else
                    lbFunc.Text = String.Empty
                    DisableAssignment()
                End If
        End Select
    End Sub

#End Region

#Region "Các hàm và thử tục khác"
    ''------------------------------''
    ''-- Hiển thị các quyền đã có --''
    ''------------------------------''
    Private Sub ShowAccessRight(ByVal pv_treeNode As Node)
        Dim v_arrMenuKey() As String
        Dim v_strAccess, v_strInquiry, v_strAdd, v_strEdit, v_strDelete, v_strExport, v_strView As String
        Dim v_strAuth, v_strCMDID, v_arrAuth() As String
        Dim v_strGrpAuth As String
        Dim v_strTellerAuth As String
        Try

            If pv_treeNode.Key <> "" Then
                EnableAssignment()
                v_arrMenuKey = pv_treeNode.Key.Split("|")
                v_strCMDID = v_arrMenuKey(2)
                v_strCMDID = v_strCMDID & mv_strBrid
                'CheckUserAccess(v_arrMenuKey(3))
                If Not hFunctionFilter(v_strCMDID) Is Nothing Then
                    v_arrAuth = CStr(hFunctionFilter(v_strCMDID)).Split("|")
                    v_strAuth = v_arrAuth(1)
                Else
                    v_strAuth = "NNNNNNN"
                End If

                If Not hFunctionTellerFilter(v_strCMDID) Is Nothing Then
                    v_arrAuth = CStr(hFunctionTellerFilter(v_strCMDID)).Split("|")
                    v_strTellerAuth = v_arrAuth(1)
                Else
                    v_strTellerAuth = "NNNNNNN"
                End If

                If AssignType = "User" Then

                    If Not hFunctionGrpFilter(v_strCMDID) Is Nothing Then
                        v_arrAuth = CStr(hFunctionGrpFilter(v_strCMDID)).Split("|")
                        v_strGrpAuth = v_arrAuth(1)
                    Else
                        v_strGrpAuth = "NNNNNNN"
                    End If

                    v_strAuth = MergeAuth(v_strAuth, v_strGrpAuth)
                End If

                If v_strAuth <> Nothing Then
                    v_strAccess = Mid(v_strAuth, 1, 1)
                    v_strInquiry = Mid(v_strAuth, 2, 1)
                    v_strAdd = Mid(v_strAuth, 3, 1)
                    v_strEdit = Mid(v_strAuth, 4, 1)
                    v_strDelete = Mid(v_strAuth, 5, 1)
                    v_strView = Mid(v_strAuth, 6, 1)
                    v_strExport = Mid(v_strAuth, 7, 1)

                    'Display Access right
                    chkAccess.Checked = (v_strAccess = "Y")
                    chkSearch.Checked = (v_strInquiry = "Y")
                    chkAddNew.Checked = (v_strAdd = "Y")
                    chkEdit.Checked = (v_strEdit = "Y")
                    chkDelete.Checked = (v_strDelete = "Y")
                    chkView.Checked = (v_strView = "Y")
                    chkExport.Checked = (v_strExport = "Y")
                End If

                If AssignType = "User" Then
                    'DisableGroupAccessRight(v_strGrpAuth)
                    v_strTellerAuth = MergeAuth1(v_strTellerAuth, v_strGrpAuth)
                End If

                DisableTellerAccessRight(v_strTellerAuth)
                'Allow assign in edit mode only
                If (ExeFlag = ExecuteFlag.View) Then
                    DisallowChange()
                End If
            Else
                DisableAssignment()
            End If
        Catch ex As Exception
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            '                         & "Error code: System error!" & vbNewLine _
            '                         & "Error message: " & ex.Message, EventLogEntryType.Error)
            'MsgBox(ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
            Throw ex
        End Try
    End Sub

    Private Sub DisableGroupAccessRight(ByVal v_strAuth As String)
        Dim v_strAccess, v_strInquiry, v_strAdd, v_strEdit, v_strDelete, v_strExport, v_strView As String

        Try
            If v_strAuth <> Nothing Then
                v_strAccess = Mid(v_strAuth, 1, 1)
                v_strInquiry = Mid(v_strAuth, 2, 1)
                v_strAdd = Mid(v_strAuth, 3, 1)
                v_strEdit = Mid(v_strAuth, 4, 1)
                v_strDelete = Mid(v_strAuth, 5, 1)
                v_strView = Mid(v_strAuth, 6, 1)
                v_strExport = Mid(v_strAuth, 7, 1)

                'Enable Access right
                chkAccess.Enabled = (v_strAccess <> "Y")
                chkSearch.Enabled = (v_strInquiry <> "Y")
                chkAddNew.Enabled = (v_strAdd <> "Y")
                chkEdit.Enabled = (v_strEdit <> "Y")
                chkDelete.Enabled = (v_strDelete <> "Y")
                chkView.Enabled = (v_strView <> "Y")
                chkExport.Enabled = (v_strExport <> "Y")
            End If


        Catch ex As Exception
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            '                         & "Error code: System error!" & vbNewLine _
            '                         & "Error message: " & ex.Message, EventLogEntryType.Error)
            'MsgBox(ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
            Throw ex
        End Try
    End Sub

    Private Sub DisableTellerAccessRight(ByVal v_strAuth As String)
        Dim v_strAccess, v_strInquiry, v_strAdd, v_strEdit, v_strDelete, v_strExport, v_strView As String

        Try
            If v_strAuth <> Nothing Then
                v_strAccess = Mid(v_strAuth, 1, 1)
                v_strInquiry = Mid(v_strAuth, 2, 1)
                v_strAdd = Mid(v_strAuth, 3, 1)
                v_strEdit = Mid(v_strAuth, 4, 1)
                v_strDelete = Mid(v_strAuth, 5, 1)
                v_strView = Mid(v_strAuth, 6, 1)
                v_strExport = Mid(v_strAuth, 7, 1)

                'Enable Access right
                chkAccess.Enabled = (v_strAccess <> "N")
                chkSearch.Enabled = (v_strInquiry <> "N")
                chkAddNew.Enabled = (v_strAdd <> "N")
                chkEdit.Enabled = (v_strEdit <> "N")
                chkDelete.Enabled = (v_strDelete <> "N")
                chkView.Enabled = (v_strView <> "N")
                chkExport.Enabled = (v_strExport <> "N")
            End If


        Catch ex As Exception
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            '                         & "Error code: System error!" & vbNewLine _
            '                         & "Error message: " & ex.Message, EventLogEntryType.Error)
            'MsgBox(ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
            Throw ex
        End Try
    End Sub


    Private Sub CheckUserAccess(ByVal pv_strAuthString As String)
        Try
            chkAccess.Enabled = (Mid(pv_strAuthString, 1, 1) = "Y")
            chkAddNew.Enabled = (Mid(pv_strAuthString, 3, 1) = "Y")
            chkEdit.Enabled = (Mid(pv_strAuthString, 4, 1) = "Y")
            chkDelete.Enabled = (Mid(pv_strAuthString, 5, 1) = "Y")
            chkSearch.Enabled = (Mid(pv_strAuthString, 2, 1) = "Y")
            chkView.Enabled = (Mid(pv_strAuthString, 6, 1) = "Y")
            chkExport.Enabled = (Mid(pv_strAuthString, 7, 1) = "Y")
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    ''---------------------------------------''
    ''-- Thủ tục ẩn các control phân quyền --''
    ''---------------------------------------''
    Private Sub DisableAssignment()
        Try
            grbAccessRight.Enabled = False
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    ''---------------------------------------------''
    ''-- Thủ tục hiển thị các control phân quyền --''
    ''---------------------------------------------''
    Private Sub EnableAssignment()
        Try
            grbAccessRight.Enabled = True
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    ''-----------------------------------------------------''
    ''-- Thủ tục ẩn các control phân quyền (chế độ View) --''
    ''-----------------------------------------------------''
    Private Sub DisallowChange()
        Try
            chkAccess.Enabled = False
            chkSearch.Enabled = False
            chkAddNew.Enabled = False
            chkEdit.Enabled = False
            chkDelete.Enabled = False
            chkView.Enabled = False
            chkExport.Enabled = False
        Catch ex As Exception

        End Try
    End Sub
    'Lay thong tin ve chi nhanh duoc phan quyen cua user/group 
    Private Sub FillBrIDData()
        Dim v_strUserInGroupObjMsg As String
        Dim v_strValue As String
        Dim v_strFLDNAME As String
        Dim v_strCmdInquiryUserInGroup As String
        Dim v_strSICode, v_strDisplay As String

        Try
            'Clear list
            lstBrID.Items.Clear()
            mv_blnBridStatus = True
            mv_strMIOutUser = ""
            mv_strMIInUser = ""
            'Lay TVLK chua dc phan quyen cho nhom hoac NSD
            'bangpv
            'Thêm: lọc hiển thị chứng khoán theo chi nhánh mà nhóm có quyền
            If AssignType = "Group" Then
                v_strCmdInquiryUserInGroup = "SELECT   brid VALUE, brid || ' - ' || brname display" _
                                                & " FROM brgrp WHERE brid in (SELECT brid FROM tlbridauth" _
                                                & " WHERE AUTHID = '" & GroupId & "')" _
                                                & " And deleted = 0 ORDER BY brid"
            Else
                v_strCmdInquiryUserInGroup = "SELECT   brid VALUE, brid || ' - ' || brname display" _
                                                & " FROM brgrp WHERE brid in (SELECT a.brid FROM tlbridauth a, tlgrpusers b " _
                                                & " WHERE a.AUTHID = '" & UserId & "' " _
                                                & " OR (b.tlid = '" & UserId & "' and b.grpid =a.authid)" _
                                                & " )" _
                                                & " And deleted = 0 ORDER BY brid"

            End If
            v_strUserInGroupObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLPROFILES, gc_ActionInquiry, v_strCmdInquiryUserInGroup)

            'Dim v_wsIngrp As New BDSDelivery.BDSDelivery
            Dim v_lngErr As Long = Proxy.Message(v_strUserInGroupObjMsg)
            If v_lngErr <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Exit Sub
            End If

            Dim v_xmlInGrpDocument As New Xml.XmlDocument
            Dim v_nodeInGrpList As Xml.XmlNodeList

            v_xmlInGrpDocument.LoadXml(v_strUserInGroupObjMsg)
            v_nodeInGrpList = v_xmlInGrpDocument.SelectNodes("/ObjectMessage/ObjData")
            Dim v_arrInGrpTLID(v_nodeInGrpList.Count - 1) As String
            Dim v_arrInGrpTLNAME(v_nodeInGrpList.Count - 1) As String
            Dim v_arrInGrpBRID(v_nodeInGrpList.Count - 1) As String
            For i As Integer = 0 To v_nodeInGrpList.Count - 1
                For j As Integer = 0 To v_nodeInGrpList.Item(i).ChildNodes.Count - 1
                    With v_nodeInGrpList.Item(i).ChildNodes(j)
                        v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                        v_strValue = .InnerText.ToString
                    End With
                    Select Case Trim(v_strFLDNAME)
                        Case "VALUE"
                            v_strSICode = v_strValue
                        Case "DISPLAY"
                            v_strDisplay = v_strValue
                    End Select
                Next
                'If Not hTellerFilter(v_strSICode) Is Nothing Then
                mv_strMIInUser &= v_strSICode & "|"
                lstBrID.Items.Add(v_strDisplay)
                ' End If

            Next
            If Not v_strSICode Is Nothing Then
                mv_strBrid = v_strSICode
                FillTellerData(mv_strBrid)
                'If AssignType = "Group" Then
                '    FillData(v_strSICode)
                'Else
                '    FillUserData(v_strSICode)
                'End If
                lstBrID.Focus()
                lstBrID.SelectedIndex = 0
                mv_blnBridStatus = True
            Else
                mv_blnBridStatus = False
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    ''------------------------------------------------''
    ''-- Lấy các thông tin phân quyền đã có của NSD --''
    ''------------------------------------------------''
    Private Sub FillData(ByVal v_strBrID As String)
        Try
            Dim v_strFLDNAME, v_strValue As String

            'Get Function access right of user
            Dim v_strCmdInquiryFunc As String, v_strFuncObjMsg As String = ""
            'modified by bangpv
            'thêm điều kiện chi nhánh
            'v_strCmdInquiryFunc = "Select M.CMDID CMDID, M.LAST LAST, A.CMDALLOW CMDALLOW, A.STRAUTH STRAUTH " _
            '                    & "from CMDMENU M, CMDAUTH A " _
            '                    & "where M.CMDID = A.CMDCODE and A.CMDALLOW = 'Y' " _
            '                    & "and A.AUTHID = '" & GroupId & "' and (A.CMDTYPE = 'M' OR A.CMDTYPE = 'A')" _
            '                    & "order by M.CMDID"
            v_strCmdInquiryFunc = "Select M.CMDID CMDID, M.LAST LAST, A.CMDALLOW CMDALLOW, A.STRAUTH STRAUTH " _
                                & "from CMDMENU M, CMDAUTH A " _
                                & "where M.CMDID = A.CMDCODE and A.CMDALLOW = 'Y' " _
                                & "and A.AUTHID = '" & GroupId & "' and (A.CMDTYPE = 'M' OR A.CMDTYPE = 'A') " _
                                & "and A.BRID='" & v_strBrID & "' " _
                                & "order by M.CMDID"
            v_strFuncObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLGROUPS, gc_ActionInquiry, v_strCmdInquiryFunc)
            'end bangpv

            Dim v_lngErr As Long = Proxy.Message(v_strFuncObjMsg)
            If v_lngErr <> ERR_SYSTEM_OK Then
                MsgBox("Lỗi khi lấy dữ liệu quyền chức năng của nhóm", MsgBoxStyle.Critical)
                Exit Sub
            End If
            Dim v_xmlFuncDocument As New Xml.XmlDocument
            Dim v_nodeFuncList As Xml.XmlNodeList

            v_xmlFuncDocument.LoadXml(v_strFuncObjMsg)
            v_nodeFuncList = v_xmlFuncDocument.SelectNodes("/ObjectMessage/ObjData")

            Dim v_strCMDID As String, v_strLAST As String
            Dim v_strCMDALLOW As String, v_strAUTH As String, v_strHashKey As String
            'hFunctionFilter.Clear()
            'hFunctionGrpFilter.Clear()
            For i As Integer = 0 To v_nodeFuncList.Count - 1
                For j As Integer = 0 To v_nodeFuncList.Item(i).ChildNodes.Count - 1
                    With v_nodeFuncList.Item(i).ChildNodes(j)
                        v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                        v_strValue = .InnerText.ToString
                    End With
                    Select Case Trim(v_strFLDNAME)
                        Case "CMDID"
                            v_strCMDID = v_strValue
                        Case "LAST"
                            v_strLAST = v_strValue
                        Case "CMDALLOW"
                            v_strCMDALLOW = v_strValue
                        Case "STRAUTH"
                            v_strAUTH = v_strValue
                    End Select
                Next
                v_strCMDID = v_strCMDID & v_strBrID
                'Fill to hashtable
                v_strHashKey = v_strCMDID & "|" & v_strCMDALLOW & v_strAUTH & "|" & v_strLAST & "|" & v_strBrID
                If hFunctionFilter(v_strCMDID) Is Nothing Then
                    hFunctionFilter.Add(v_strCMDID, v_strHashKey)
                    mv_strFuncAuth &= v_strHashKey & "#"
                End If
            Next

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    ''------------------------------------------------''
    ''-- Lấy các thông tin phân quyền đã có của NSD đang đăng nhập --''
    ''------------------------------------------------''
    Private Sub FillTellerData(ByVal v_strBrid As String)
        Try
            Dim v_strFLDNAME, v_strValue As String

            'Get Function access right of user
            Dim v_strFuncObjMsg As String

            v_strFuncObjMsg = BuildXMLObjMsg(Now.Date, v_strBrid, Now.Date, _
                    m_BusLayer.CurrentTellerProfile.TellerId, CommonLibrary.gc_IsLocalMsg, CommonLibrary.gc_MsgTypeObj, _
                    OBJNAME_SY_AUTHENTICATION, CommonLibrary.gc_ActionInquiry, , m_BusLayer.CurrentTellerProfile.TellerId, "GetTellerFunctionData")


            Dim v_lngError As Long = Proxy.Message(v_strFuncObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Exit Sub
            End If

            Dim v_xmlFuncDocument As New Xml.XmlDocument
            Dim v_nodeFuncList As Xml.XmlNodeList

            v_xmlFuncDocument.LoadXml(v_strFuncObjMsg)
            v_nodeFuncList = v_xmlFuncDocument.SelectNodes("/ObjectMessage/ObjData")

            Dim v_strCMDID As String, v_strLAST As String
            Dim v_strCMDALLOW As String, v_strAUTH As String, v_strHashKey As String
            For i As Integer = 0 To v_nodeFuncList.Count - 1
                For j As Integer = 0 To v_nodeFuncList.Item(i).ChildNodes.Count - 1
                    With v_nodeFuncList.Item(i).ChildNodes(j)
                        v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                        v_strValue = .InnerText.ToString
                    End With
                    Select Case Trim(v_strFLDNAME)
                        Case "CMDID"
                            v_strCMDID = v_strValue
                        Case "LAST"
                            v_strLAST = v_strValue
                        Case "CMDALLOW"
                            v_strCMDALLOW = v_strValue
                        Case "STRAUTH"
                            v_strAUTH = v_strValue
                    End Select
                Next
                'Fill to hashtable
                v_strCMDID = v_strCMDID & v_strBrid
                If Not hFunctionTellerFilter(v_strCMDID) Is Nothing Then
                    Dim v_strOldHashVale As String
                    Dim v_strTempAuth As String
                    v_strOldHashVale = hFunctionTellerFilter(v_strCMDID)
                    hFunctionTellerFilter.Remove(v_strCMDID)

                    v_strTempAuth = v_strOldHashVale.Split("|")(1)
                    v_strAUTH = MergeAuth(v_strTempAuth, v_strCMDALLOW & v_strAUTH)

                    v_strHashKey = v_strCMDID & "|" & v_strAUTH & "|" & v_strLAST & "|" & v_strBrid
                    hFunctionTellerFilter.Add(v_strCMDID, v_strHashKey)
                Else
                    v_strHashKey = v_strCMDID & "|" & v_strCMDALLOW & v_strAUTH & "|" & v_strLAST & "|" & v_strBrid
                    hFunctionTellerFilter.Add(v_strCMDID, v_strHashKey)
                End If
            Next

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub FillUserData(ByVal v_strBrid As String)
        Try
            Dim v_strFLDNAME, v_strValue As String

            'Get Function access right of user
            Dim v_strCmdInquiryFunc As String, v_strFuncObjMsg As String = ""
            'bangpv
            'thêm điều kiện chi nhánh
            'v_strCmdInquiryFunc = "SELECT m.cmdid cmdid, m.last last, a.cmdallow cmdallow, a.strauth strauth, a.authtype" _
            '                        & " FROM cmdmenu m, cmdauth a" _
            '                        & " WHERE(m.cmdid = a.cmdcode)" _
            '                        & " AND a.cmdallow = 'Y'" _
            '                        & " AND (a.AUTHID = '" & UserId & "' OR a.authid IN (SELECT grpid FROM tlgrpusers WHERE tlid='" & UserId & "'))" _
            '                        & " AND (a.cmdtype = 'M' OR a.cmdtype = 'A')" _
            '                        & " ORDER BY m.cmdid"
            'hFunctionGrpFilter = Nothing
            v_strCmdInquiryFunc = "SELECT m.cmdid cmdid, m.last last, a.cmdallow cmdallow, a.strauth strauth, a.authtype" _
                                    & " FROM cmdmenu m, cmdauth a" _
                                    & " WHERE(m.cmdid = a.cmdcode)" _
                                    & " AND a.cmdallow = 'Y'" _
                                    & "AND a.brid ='" & v_strBrid & "' " _
                                    & " AND (a.AUTHID = '" & UserId & "' OR a.authid IN (SELECT grpid FROM tlgrpusers WHERE tlid='" & UserId & "'))" _
                                    & " AND (a.cmdtype = 'M' OR a.cmdtype = 'A')" _
                                    & " ORDER BY m.cmdid"
            'end bangpv
            v_strFuncObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLPROFILES, gc_ActionInquiry, v_strCmdInquiryFunc)

            Dim v_lngError As Long = Proxy.Message(v_strFuncObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Exit Sub
            End If

            Dim v_xmlFuncDocument As New Xml.XmlDocument
            Dim v_nodeFuncList As Xml.XmlNodeList
            'hFunctionFilter.Clear()
            'hFunctionGrpFilter.Clear()
            v_xmlFuncDocument.LoadXml(v_strFuncObjMsg)
            v_nodeFuncList = v_xmlFuncDocument.SelectNodes("/ObjectMessage/ObjData")
            Dim v_strCMDID As String = "", v_strLAST As String = ""
            Dim v_strCMDALLOW As String = "", v_strAUTH As String = "", v_strHashKey As String = ""
            Dim v_strAuthType As String = ""

            For i As Integer = 0 To v_nodeFuncList.Count - 1
                For j As Integer = 0 To v_nodeFuncList.Item(i).ChildNodes.Count - 1
                    With v_nodeFuncList.Item(i).ChildNodes(j)
                        v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                        v_strValue = .InnerText.ToString
                    End With
                    Select Case Trim(v_strFLDNAME)
                        Case "CMDID"
                            v_strCMDID = v_strValue
                        Case "LAST"
                            v_strLAST = v_strValue
                        Case "CMDALLOW"
                            v_strCMDALLOW = v_strValue
                        Case "STRAUTH"
                            v_strAUTH = v_strValue
                        Case "AUTHTYPE"
                            v_strAuthType = v_strValue
                    End Select
                Next
                'Fill to hashtable
                v_strCMDID = v_strCMDID & v_strBrid
                If v_strAuthType = "U" Then
                    v_strHashKey = v_strCMDID & "|" & v_strCMDALLOW & v_strAUTH & "|" & v_strLAST & "|" & v_strBrid
                    If hFunctionFilter(v_strCMDID) Is Nothing Then
                        hFunctionFilter.Add(v_strCMDID, v_strHashKey)
                        mv_strFuncAuth &= v_strHashKey & "#"
                    End If
                Else
                    If Not hFunctionGrpFilter(v_strCMDID) Is Nothing Then
                        Dim v_strOldHashVale As String
                        Dim v_strTempAuth As String
                        v_strOldHashVale = hFunctionGrpFilter(v_strCMDID)
                        hFunctionGrpFilter.Remove(v_strCMDID)
                        mv_strFuncGrpAuth = Replace(mv_strFuncGrpAuth, v_strOldHashVale & "#", "")
                        v_strTempAuth = v_strOldHashVale.Split("|")(1)
                        v_strAUTH = MergeAuth(v_strTempAuth, v_strCMDALLOW & v_strAUTH)
                        v_strHashKey = v_strCMDID & "|" & v_strAUTH & "|" & v_strBrid
                        mv_strFuncGrpAuth &= v_strHashKey & "#"
                        hFunctionGrpFilter.Add(v_strCMDID, v_strHashKey)
                    Else
                        v_strHashKey = v_strCMDID & "|" & v_strCMDALLOW & v_strAUTH & "|" & v_strLAST & "|" & v_strBrid
                        hFunctionGrpFilter.Add(v_strCMDID, v_strHashKey)
                        mv_strFuncGrpAuth &= v_strHashKey & "#"
                    End If
                End If
            Next
        Catch ex As Exception
            Throw ex
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

    Private Function MergeAuth1(ByVal pv_strTeller As String, ByVal pv_strGroup As String) As String
        Dim v_strAuth As String = ""
        v_strAuth &= IIf(Mid(pv_strGroup, 1, 1) = "Y", "N", Mid(pv_strTeller, 1, 1))
        v_strAuth &= IIf(Mid(pv_strGroup, 2, 1) = "Y", "N", Mid(pv_strTeller, 2, 1))
        v_strAuth &= IIf(Mid(pv_strGroup, 3, 1) = "Y", "N", Mid(pv_strTeller, 3, 1))
        v_strAuth &= IIf(Mid(pv_strGroup, 4, 1) = "Y", "N", Mid(pv_strTeller, 4, 1))
        v_strAuth &= IIf(Mid(pv_strGroup, 5, 1) = "Y", "N", Mid(pv_strTeller, 5, 1))
        v_strAuth &= IIf(Mid(pv_strGroup, 6, 1) = "Y", "N", Mid(pv_strTeller, 6, 1))
        v_strAuth &= IIf(Mid(pv_strGroup, 7, 1) = "Y", "N", Mid(pv_strTeller, 7, 1))
        Return v_strAuth
    End Function

    Private Sub OnSave()
        Dim v_strObjMsg As String = ""
        Dim v_strAllStrAuth, v_strAuthString As String

        Cursor = Cursors.WaitCursor
        btnOk.Enabled = False
        Try
            If AssignType = "User" Then
                
                v_strAllStrAuth = UserId & "$" & mv_strFuncAuth

                'Build XML message
                v_strObjMsg = BuildXMLObjMsg(Now.Date, mv_strBrid, , TellerId, gc_IsNotLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLPROFILES, gc_ActionAdhoc, , v_strAllStrAuth, "FunctionAssignment")
            ElseIf AssignType = "Group" Then
                v_strAuthString = mv_strFuncAuth
                'Add Groupid and Usersid string to strAuth
                v_strAllStrAuth = GroupId & "$" & v_strAuthString ' & "$" & v_strGrpUsrId

                'Build XML message
                v_strObjMsg = BuildXMLObjMsg(Now.Date, mv_strBrid, , TellerId, gc_IsNotLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLGROUPS, gc_ActionAdhoc, , v_strAllStrAuth, "FunctionAssignment")
            End If

            'Dim v_ws As New BDSDelivery.BDSDelivery
            Dim v_lngErrMsg As Long = Proxy.Message(v_strObjMsg)


            If v_lngErrMsg <> ERR_SYSTEM_OK Then
                Me.Cursor = Cursors.Default
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Exit Sub
            End If
            ''Kiểm tra thông tin và xử lý lỗi (nếu có) từ message trả về
            'Dim v_strErrorSource As String = "", v_strErrorMessage As String = ""
            'Dim v_lngErrorCode As Long

            'GetErrorFromMessage(v_strObjMsg, v_strErrorSource, v_lngErrorCode, v_strErrorMessage)
            'If v_lngErrorCode <> 0 Then
            '    'Update mouse pointer
            '    Cursor = Cursors.Default
            '    MsgBox(v_strErrorMessage, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, Me.Text)
            '    Exit Sub
            'End If

            'MsgBox(mv_ResourceManager.GetString("SavingSuccess"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, Me.Text)

            MessageBox.Show(mv_ResourceManager.GetString("SavingSuccess"), gc_ApplicationTitle, MessageBoxButtons.OK)

            Cursor = Cursors.Default
            btnOk.Enabled = True
            Me.Close()

        Catch ex As Exception
            Cursor = Cursors.Default
            btnOk.Enabled = True
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                                 & "Error code: System error!" & vbNewLine _
                                                 & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)

            MessageBox.Show(mv_ResourceManager.GetString("SavingFailed"), gc_ApplicationTitle, MessageBoxButtons.OK)
            'MsgBox(mv_ResourceManager.GetString("SavingFailed"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, Me.Text)
        End Try
    End Sub

    Private Function GetGrpUsrId(ByVal pv_strGroupId As String) As String
        Dim v_strUsersId As String = String.Empty
        Dim v_strUsrInGrpSql, v_strFLDNAME, v_strValue As String
        Dim v_strObjMsg As String

        Try

            ''''==== Select users' name that are in group ====''''
            v_strUsrInGrpSql = "SELECT TLPROFILES.TLID VALUE, TLPROFILES.TLNAME DISPLAY " _
                                    & "FROM TLPROFILES, TLGRPUSERS, TLGROUPS " _
                                    & "WHERE TLPROFILES.TLID = TLGRPUSERS.TLID AND TLGRPUSERS.GRPID = TLGROUPS.GRPID AND TLGROUPS.GRPID = '" & pv_strGroupId & "' ORDER BY TLPROFILES.TLNAME"
            v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLPROFILES, gc_ActionInquiry, v_strUsrInGrpSql)

            'Dim v_wsIngrp As New BDSDelivery.BDSDelivery

            Dim v_lngError As Long = Proxy.Message(v_strObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Return String.Empty
            End If

            Dim v_xmlDocument As New Xml.XmlDocument
            Dim v_nodeList As Xml.XmlNodeList

            v_xmlDocument.LoadXml(v_strObjMsg)
            v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")

            For i As Integer = 0 To v_nodeList.Count - 1
                For j As Integer = 0 To v_nodeList.Item(i).ChildNodes.Count - 1
                    With v_nodeList.Item(i).ChildNodes(j)
                        v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                        v_strValue = .InnerText.ToString
                    End With
                    Select Case Trim(v_strFLDNAME)
                        Case "VALUE"
                            v_strUsersId &= v_strValue & "#"
                    End Select
                Next
            Next

            Return v_strUsersId
        Catch ex As Exception
            Throw ex
        End Try
    End Function
#End Region

#Region "Hàm & thủ tục xử lý Form"
    Public Sub OnInit()
        Try
            'Khởi tạo kích thước form và load resource
            DisableAssignment()
            mv_ResourceManager = New Resources.ResourceManager(gc_RootNamespace & ".frmFuncAssign_" & UserLanguage, System.Reflection.Assembly.GetExecutingAssembly())
            LoadUserInterface(Me)
            ' mv_BDSDelivery = New BDSChannel.BDSDelivery
            'remove by bangpv
            'If AssignType.ToUpper = "GROUP" Then
            '    FillData()
            'Else
            '    FillUserData()
            'End If
            'end bangpv
            FillBrIDData()
            If mv_blnBridStatus = False Then
                If AssignType = "Group" Then
                    MsgBox("Nhóm chưa được phân quyền chi nhánh", MsgBoxStyle.Critical)
                    Me.Close()
                    Exit Sub
                Else
                    MsgBox("Người sử dụng chưa được phân quyền chi nhánh", MsgBoxStyle.Critical)
                    Me.Close()
                    Exit Sub
                End If

            End If

            GetTreeView()
            'Check trang thai
            Dim v_strSQL, v_strObjMsg As String
            If AssignType = "Group" Then
                v_strSQL = "SELECT STATUS FROM TLGROUPS WHERE GRPID='" & GroupId & "'"
            Else
                v_strSQL = "SELECT STATUS FROM TLPROFILES WHERE TLID='" & UserId & "'"
            End If
            v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLPROFILES, gc_ActionInquiry, v_strSQL)

            'Dim v_wsOutGrp As New BDSDelivery.BDSDelivery
            Dim v_lngError As Long = Proxy.Message(v_strObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Exit Sub
            End If

            Dim v_xmlDocument As New Xml.XmlDocument
            Dim v_node As Xml.XmlNodeList
            Dim v_strStatus, v_strFLDNAME, v_strValue As String

            v_xmlDocument.LoadXml(v_strObjMsg)
            v_node = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")
            For i As Integer = 0 To v_node.Count - 1
                For j As Integer = 0 To v_node.Item(i).ChildNodes.Count - 1
                    With v_node.Item(i).ChildNodes(j)
                        v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                        v_strValue = .InnerText.ToString
                    End With
                    Select Case Trim(v_strFLDNAME)
                        Case "STATUS"
                            v_strStatus = v_strValue
                    End Select
                Next
            Next

            If CInt("0" & v_strStatus) <> 0 Then
                btnOk.Enabled = False
            End If
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                         & "Error code: System error!" & vbNewLine _
                         & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(ResourceManager.GetString("InitDialogFailed"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
        End Try
    End Sub

    Private Sub LoadUserInterface(ByRef pv_ctrl As Windows.Forms.Control)
        Dim v_ctrl As Windows.Forms.Control
        Try
            For Each v_ctrl In pv_ctrl.Controls
                If TypeOf (v_ctrl) Is Panel Then
                    LoadUserInterface(v_ctrl)
                ElseIf TypeOf (v_ctrl) Is GroupBox Then
                    CType(v_ctrl, GroupBox).Text = mv_ResourceManager.GetString(v_ctrl.Name)
                    LoadUserInterface(v_ctrl)
                ElseIf TypeOf (v_ctrl) Is Label Then
                    CType(v_ctrl, Label).Text = mv_ResourceManager.GetString(v_ctrl.Name)
                ElseIf TypeOf (v_ctrl) Is Button Then
                    CType(v_ctrl, Button).Text = mv_ResourceManager.GetString(v_ctrl.Name)
                ElseIf TypeOf (v_ctrl) Is CheckBox Then
                    CType(v_ctrl, CheckBox).Text = mv_ResourceManager.GetString(v_ctrl.Name)
                End If
            Next

            'Load caption of form, label caption
            Me.Text = mv_ResourceManager.GetString("frmFuncAssign")
            If AssignType = "User" Then
                lbCaption.Text = mv_ResourceManager.GetString("lbCaption0") & UserName
            ElseIf AssignType = "Group" Then
                lbCaption.Text = mv_ResourceManager.GetString("lbCaption1") & GroupName
            End If

            'Disable control if in view mode
            If (ExeFlag = ExecuteFlag.View) Then
                btnOk.Enabled = False
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    ''---------------------------------------------------------------''
    ''-- Thay đổi các giá trị khi thay đổi trạng thái của checkbox --''
    ''---------------------------------------------------------------''
    Private Sub Checkbox_CheckedChanged(ByVal sender As Object, ByVal pv_treeNode As Node)

        Try

            If pv_treeNode.Items.Count > 0 Then
                DoCheckboxChange(sender, pv_treeNode)
                Dim v_node As Node
                For Each v_node In pv_treeNode.Items
                    If v_node.Items.Count > 0 Then
                        Checkbox_CheckedChanged(sender, v_node)
                    Else
                        DoCheckboxChange(sender, v_node)
                    End If
                Next
            Else
                DoCheckboxChange(sender, pv_treeNode)
            End If

        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                     & "Error code: System error!" & vbNewLine _
                                     & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
        End Try

    End Sub

    ''-------------------------------------------------------------''
    ''-- + Mục đích: Thay đổi và cập nhật các giá trị phân quyền --''
    ''--             lên key của node of menu tree               --''
    ''-- + Đầu vào: - sender: Checkbox có giá trị thay đổi       --''
    ''--            - pv_treeNode: Node hiện tại của menu tree   --''
    ''-- + Đầu ra: N/A                                           --''
    ''-- + Tác giả: Nguyen Minh Tho                              --''
    ''-- + Ghi chú: N/A                                          --''
    ''-------------------------------------------------------------''
    Private Sub DoCheckboxChange(ByVal sender As Object, ByVal pv_treeNode As Node)
        Dim v_strCMDALLOW As String = "", v_strInquiry As String = "", v_strAdd As String = ""
        Dim v_strEdit As String = "", v_strDelete As String = "", v_strExport As String = "", v_strView As String = ""
        Dim v_strCMDID, v_strAuth, v_strHashValue, v_arrAuth() As String
        Dim v_strKey As String, v_strLast As String
        Try
            v_strKey = pv_treeNode.Key.ToString
            v_strCMDID = v_strKey.Split("|")(2)
            v_strLast = v_strKey.Split("|")(0)
            v_strCMDID = v_strCMDID & mv_strBrid
            If Not hFunctionFilter(v_strCMDID) Is Nothing Then
                v_arrAuth = CStr(hFunctionFilter(v_strCMDID)).Split("|")
                v_strAuth = v_arrAuth(1)
            Else
                v_strAuth = "NNNNNNN"
            End If
            If v_strAuth <> Nothing Then
                'If ckbACCESS's value has changed
                If (sender Is chkAccess) Then
                    If chkAccess.Checked Then
                        v_strCMDALLOW = "Y"
                        v_strAuth = v_strCMDALLOW & Mid(v_strAuth, 2, 6)
                    Else
                        v_strCMDALLOW = "N"
                        chkSearch.Checked = False
                        chkAddNew.Checked = False
                        chkEdit.Checked = False
                        chkDelete.Checked = False
                        chkView.Checked = False
                        chkExport.Checked = False

                        v_strInquiry = "N"
                        v_strAdd = "N"
                        v_strEdit = "N"
                        v_strDelete = "N"
                        v_strView = "N"
                        v_strExport = "N"
                        v_strAuth = v_strCMDALLOW & v_strView & v_strAdd & v_strEdit & v_strDelete & v_strInquiry & v_strExport
                    End If

                    'If ckbINQUIRY's value has changed
                ElseIf (sender Is chkSearch) Then
                    If chkSearch.Checked Then
                        v_strInquiry = "Y"
                        If chkAccess.Checked = False Then
                            chkAccess.Checked = True
                        End If
                        v_strCMDALLOW = "Y"
                        v_strAuth = v_strCMDALLOW & Mid(v_strAuth, 2, 4) & v_strInquiry & Mid(v_strAuth, 6, 1)
                    Else
                        v_strInquiry = "N"
                        chkAddNew.Checked = False
                        chkEdit.Checked = False
                        chkDelete.Checked = False
                        chkView.Checked = False
                        chkExport.Checked = False
                        v_strAuth = Mid(v_strAuth, 1, 1) & "NNNNNN"
                    End If

                    'If ckbADD's value has changed
                ElseIf (sender Is chkAddNew) Then
                    If chkAddNew.Checked Then
                        v_strAdd = "Y"
                        If chkAccess.Checked = False Then
                            chkAccess.Checked = True
                        End If
                        v_strCMDALLOW = "Y"
                        If chkSearch.Checked = False Then
                            chkSearch.Checked = True
                        End If
                        v_strInquiry = "Y"
                        v_strAuth = v_strCMDALLOW & Mid(v_strAuth, 2, 1) & v_strAdd & Mid(v_strAuth, 4, 2) & v_strInquiry & Mid(v_strAuth, 6, 1)
                    Else
                        v_strAdd = "N"
                        v_strAuth = Mid(v_strAuth, 1, 2) & v_strAdd & Mid(v_strAuth, 4, 2)
                    End If

                    'If ckbEDIT's value has changed
                ElseIf (sender Is chkEdit) Then
                    If chkEdit.Checked Then
                        v_strEdit = "Y"
                        If chkAccess.Checked = False Then
                            chkAccess.Checked = True
                        End If
                        v_strCMDALLOW = "Y"
                        If chkSearch.Checked = False Then
                            chkSearch.Checked = True
                        End If
                        v_strInquiry = "Y"
                        v_strAuth = v_strCMDALLOW & Mid(v_strAuth, 2, 2) & v_strEdit & Mid(v_strAuth, 5, 2)
                    Else
                        v_strEdit = "N"
                        v_strAuth = Mid(v_strAuth, 1, 3) & v_strEdit & Mid(v_strAuth, 5, 2)
                    End If

                    'If ckbDELETE's value has changed
                ElseIf (sender Is chkDelete) Then
                    If chkDelete.Checked Then
                        v_strDelete = "Y"
                        If chkAccess.Checked = False Then
                            chkAccess.Checked = True
                        End If
                        v_strCMDALLOW = "Y"
                        If chkSearch.Checked = False Then
                            chkSearch.Checked = True
                        End If
                        v_strInquiry = "Y"
                        v_strAuth = v_strCMDALLOW & Mid(v_strAuth, 2, 3) & v_strDelete & v_strInquiry & Mid(v_strAuth, 6, 1)
                    Else
                        v_strDelete = "N"
                        v_strAuth = Mid(v_strAuth, 1, 4) & v_strDelete & Mid(v_strAuth, 5, 2)
                    End If

                    'If ckbView's value has changed
                ElseIf (sender Is chkView) Then
                    If chkView.Checked Then
                        v_strView = "Y"
                        If chkAccess.Checked = False Then
                            chkAccess.Checked = True
                        End If
                        v_strCMDALLOW = "Y"
                        If chkSearch.Checked = False Then
                            chkSearch.Checked = True
                        End If
                        v_strInquiry = "Y"
                        v_strAuth = v_strCMDALLOW & v_strView & Mid(v_strAuth, 3, 3) & v_strInquiry & Mid(v_strAuth, 6, 1)
                    Else
                        v_strView = "N"
                        v_strAuth = Mid(v_strAuth, 1, 1) & v_strView & Mid(v_strAuth, 3, 4)
                    End If

                    'If ckbExport value has changed
                ElseIf (sender Is chkExport) Then
                    If chkExport.Checked Then
                        v_strExport = "Y"
                        If chkAccess.Checked = False Then
                            chkAccess.Checked = True
                        End If
                        v_strCMDALLOW = "Y"
                        If chkSearch.Checked = False Then
                            chkSearch.Checked = True
                        End If
                        v_strInquiry = "Y"
                        v_strAuth = v_strCMDALLOW & Mid(v_strAuth, 2, 4) & v_strInquiry & v_strExport
                    Else
                        v_strExport = "N"
                        v_strAuth = Mid(v_strAuth, 1, 6) & v_strExport
                    End If

                End If

                v_strHashValue = v_strCMDID & "|" & v_strAuth & "|" & v_strLast & "|" & mv_strBrid
                Dim v_strOldHashValue As String
                If v_strAuth = "NNNNNNN" Then
                    If Not hFunctionFilter(v_strCMDID) Is Nothing Then
                        v_strOldHashValue = hFunctionFilter(v_strCMDID)
                        hFunctionFilter.Remove(v_strCMDID)
                        mv_strFuncAuth = Replace(mv_strFuncAuth, v_strOldHashValue & "#", "").Trim
                    End If
                Else
                    'Update new value to hash table
                    If Not hFunctionFilter(v_strCMDID) Is Nothing Then
                        'Remove old value before add new value to hash table and auth's string
                        v_strOldHashValue = hFunctionFilter(v_strCMDID)
                        hFunctionFilter.Remove(v_strCMDID)
                        mv_strFuncAuth = Replace(mv_strFuncAuth, v_strOldHashValue & "#", "").Trim
                        hFunctionFilter.Add(v_strCMDID, v_strHashValue)
                        mv_strFuncAuth &= v_strHashValue & "#"
                    Else
                        'Add new value to hash table and auth's string
                        hFunctionFilter.Add(v_strCMDID, v_strHashValue)
                        mv_strFuncAuth &= v_strHashValue & "#"
                    End If
                End If
                SetParentNodeKey(pv_treeNode)
            End If

        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                           & "Error code: System error!" & vbNewLine _
                           & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Sub

    ' ''--------------------------------------------------------------''
    ' ''-- + Mục đích: Set lại key của các node cha của 1 node      --''
    ' ''-- + Đầu vào: pv_treeNode: node có các node cha cần set key --''
    ' ''-- + Đầu ra: N/A                                            --''
    ' ''-- + Tác giả: Nguyen Minh Tho                               --''
    ' ''-- + Ghi chú: N/A                                           --''
    ' ''--------------------------------------------------------------''
    Private Sub SetParentNodeKey(ByVal pv_treeNode As Node)
        Dim v_strPRID, v_strLEV, v_strLast, v_strCMDID, v_strAuth, v_arrMenuKey() As String

        Try
            Dim v_treeNodeParent As Node
            Dim v_strAuthStr As String = "YNNNNNN"
            Dim bln As Boolean
            Dim v_strHashValue As String
            'v_treeNodeParent = pv_treeNode.ParentItem

            v_arrMenuKey = pv_treeNode.Key.Split("|")
            v_strCMDID = v_arrMenuKey(2)
            v_strLEV = v_arrMenuKey(1)
            v_strLast = v_arrMenuKey(0)

            For i As Integer = 0 To CInt(v_strLEV) - 2
                v_treeNodeParent = pv_treeNode.ParentItem
                bln = False
                For v_int As Integer = 0 To v_treeNodeParent.Items.Count - 1
                    v_strLast = Mid(v_treeNodeParent.Items(v_int).Key, 1, 1)
                    If v_strLast = "Y" Then
                        v_strCMDID = v_treeNodeParent.Items(v_int).Key.Split("|")(2)
                        v_strCMDID = v_strCMDID & mv_strBrid
                    Else
                        v_strCMDID = Mid(v_treeNodeParent.Items(v_int).Key, 2)
                        v_strCMDID = v_strCMDID & mv_strBrid
                    End If
                    If Not hFunctionFilter(v_strCMDID) Is Nothing Then
                        v_strAuth = hFunctionFilter(v_strCMDID).ToString.Split("|")(1)
                        bln = (Mid(v_strAuth, 1, 1) = "Y")
                        If bln Then Exit For
                    End If
                Next
                v_strPRID = Mid(v_treeNodeParent.Key, 2)
                v_strPRID = v_strPRID & mv_strBrid
                If bln Then
                    If hFunctionFilter(v_strPRID) Is Nothing Then
                        v_strHashValue = v_strPRID & "|" & v_strAuthStr & "|" & v_strLast & "|" & mv_strBrid
                        hFunctionFilter.Add(v_strPRID, v_strHashValue)
                        mv_strFuncAuth &= v_strHashValue & "#"
                    End If
                Else
                    If Not hFunctionFilter(v_strPRID) Is Nothing Then
                        hFunctionFilter.Remove(v_strPRID)
                        v_strHashValue = v_strPRID & "|" & v_strAuthStr & "|" & v_strLast & "|" & mv_strBrid
                        mv_strFuncAuth = Replace(mv_strFuncAuth, v_strHashValue & "#", "")
                    End If
                End If
                v_treeNodeParent = v_treeNodeParent.ParentItem
            Next

        Catch ex As Exception
            Throw ex
        End Try

    End Sub
#End Region

#Region "Sự kiện Form"
    Private Sub frmFuncAssign_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Me.OnInit()
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        OnSave()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub CheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAddNew.Click, chkAccess.Click, chkEdit.Click, chkDelete.Click, chkSearch.Click, chkView.Click, chkExport.Click
        DoCheckboxChange(sender, stvTran.SelectedItem)
    End Sub

#End Region

    

    Private Sub lstBrID_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstBrID.SelectedIndexChanged
        Try
            Dim v_lstbrid_value As String
            v_lstbrid_value = lstBrID.SelectedItem.ToString.Split("-")(0).Trim
            mv_strBrid = v_lstbrid_value
            'hFunctionGrpFilter = Nothing
            FillTellerData(mv_strBrid)
            If AssignType = "Group" Then
                FillData(mv_strBrid)
            Else
                FillUserData(mv_strBrid)
            End If
            GetTreeView()
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                           & "Error code: System error!" & vbNewLine _
                           & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Sub
End Class