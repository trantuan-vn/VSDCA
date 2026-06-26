Imports System.IO
Imports Microsoft.Win32
Imports System.Threading
Imports Xceed.SmartUI.Controls.TreeView
Imports Sats.CommonLibrary
Imports Sats.AppCore
Imports Sats.AppCore.GridEx
Imports Sats.AppCore.ComboBoxEx

Public Class frmReportAssign
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
    Dim mv_strFuncGrpAuth As String

    'Dim hParentsFilter As New Hashtable
    Dim hReportFilter As New Hashtable
    Dim hReportTellerFilter As New Hashtable
    Dim hReportGrpFilter As New Hashtable

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
            If pv_intImageIdx = 3 Then
                AddHandler v_node.Click, AddressOf Me.TreeView_Click
                AddHandler v_node.KeyUp, AddressOf Me.TreeView_KeyUp
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

    Private Sub FillTreeView(Optional ByVal IsShown As Boolean = True)
        Dim v_strObjMsg As String
        Dim v_nodeList As Xml.XmlNodeList
        Dim XmlDocument As New Xml.XmlDocument
        Dim v_int, v_intCount, v_intIndex As Integer
        Dim v_strMenuKey As String, v_strFLDNAME As String, v_strValue As String
        Dim v_strModCode As String, v_strCmdName As String, v_strTXCCODE, v_strCmdID As String
        Dim v_strSQL As String, v_strPrID As String
        Try

            v_strSQL = "SELECT a.modname, a.txcode, a.modcode, b.cmdid, b.prid" _
                        & " FROM appmodules a, cmdmenu b" _
                        & " WHERE a.modcode = b.modcode AND b.menutype = 'R' order by a.txcode"
            v_strObjMsg = BuildXMLObjMsg(Now.Date, m_BusLayer.CurrentTellerProfile.BranchId, , _
                              m_BusLayer.CurrentTellerProfile.TellerId & "|" & m_BusLayer.CurrentTellerProfile.TellerLevel, CommonLibrary.gc_IsLocalMsg, CommonLibrary.gc_MsgTypeObj, _
                               OBJNAME_SA_APPMODULES, CommonLibrary.gc_ActionInquiry, v_strSQL)
            m_BusLayer.BusSystemMessage(v_strObjMsg)

            XmlDocument.LoadXml(v_strObjMsg)
            v_nodeList = XmlDocument.SelectNodes("/ObjectMessage/ObjData")

            For v_intCount = 0 To v_nodeList.Count - 1
                For v_int = 0 To v_nodeList.Item(v_intCount).ChildNodes.Count - 1
                    With v_nodeList.Item(v_intCount).ChildNodes(v_int)
                        v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                        v_strValue = .InnerText.ToString

                        Select Case Trim(v_strFLDNAME)
                            Case "TXCODE"
                                v_strTXCCODE = Trim(v_strValue)
                            Case "MODCODE"
                                v_strModCode = Trim(v_strValue)
                            Case "MODNAME"
                                v_strCmdName = Trim(v_strValue)
                            Case "CMDID"
                                v_strCmdID = Trim(v_strValue)
                            Case "PRID"
                                v_strPrID = Trim(v_strValue)
                        End Select
                    End With
                Next v_int

                'If Len(Trim(v_strPRID)) = 0 Then

                v_intIndex = 0

                v_strMenuKey = v_strCmdID & "|" & v_strModCode & "|" & v_strTXCCODE & "|" & v_strPrID

                Dim v_node As New Node(v_strCmdName, v_intIndex)
                v_node.Key = CStr(v_strMenuKey)
                v_node.ToolTipText = v_strCmdName
                v_node.Expanded = False

                Me.stvTran.Items(0).Items.Add(v_node)
                AddReportMenu(stvTran.Items(0).Items(CStr(v_strMenuKey)), CStr(v_strModCode))

            Next v_intCount

        Catch ex As Exception
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            '             & "Error code: System error!" & vbNewLine _
            '             & "Error message: " & ex.Message, EventLogEntryType.Error)
            'MsgBox(ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
            Throw ex
        End Try
    End Sub


    Private Sub AddReportMenu(ByRef pv_nodeParent As Node, ByVal pv_strKey As String)
        Dim v_strObjMsg As String
        Dim v_nodeList As Xml.XmlNodeList
        Dim XmlDocument As New Xml.XmlDocument
        Dim v_int, v_intCount, v_intIndex As Integer
        Dim v_strFLDNAME As String, v_strValue As String
        Dim v_strRPID As String, v_strCmdName As String
        Dim v_strMenuKey As String
        Dim v_strModCode As String
        Dim v_NewNode As New Node
        Dim v_strSQL As String

        Try
            v_strSQL = "SELECT RPTTITLE, EN_RPTTITLE, RPTID, MODCODE FROM RPREPORTS WHERE MODCODE='" & pv_strKey & "' AND DELETED=0 AND STATUS=0 ORDER BY RPTID"
            v_strObjMsg = CommonLibrary.BuildXMLObjMsg(Now.Date, m_BusLayer.CurrentTellerProfile.BranchId, , _
                                m_BusLayer.CurrentTellerProfile.TellerId & "|" & m_BusLayer.CurrentTellerProfile.TellerLevel, CommonLibrary.gc_IsLocalMsg, CommonLibrary.gc_MsgTypeObj, _
                                 OBJNAME_RP_RPLIST, CommonLibrary.gc_ActionInquiry, v_strSQL)
            m_BusLayer.BusSystemMessage(v_strObjMsg)

            XmlDocument.LoadXml(v_strObjMsg)
            v_nodeList = XmlDocument.SelectNodes("/ObjectMessage/ObjData")

            If Not pv_nodeParent Is Nothing Then
                For v_intCount = 0 To v_nodeList.Count - 1
                    For v_int = 0 To v_nodeList.Item(v_intCount).ChildNodes.Count - 1
                        With v_nodeList.Item(v_intCount).ChildNodes(v_int)
                            v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                            v_strValue = .InnerText.ToString

                            Select Case Trim(v_strFLDNAME).ToUpper
                                Case "RPTID"
                                    v_strRPID = Trim(v_strValue)
                                Case "RPTTITLE"
                                    If UserLanguage <> "EN" Then
                                        v_strCmdName = Trim(v_strValue)
                                    End If
                                Case "EN_RPTTITLE"
                                    If UserLanguage = "EN" Then
                                        v_strCmdName = Trim(v_strValue)
                                    End If
                                Case "MODCODE"
                                    v_strModCode = Trim(v_strValue)
                            End Select
                        End With
                    Next v_int

                    v_intIndex = 3

                    'Set menu's key
                    v_strMenuKey = v_strRPID & "|" & v_strModCode
                    'Add new node to menu tree
                    v_NewNode = AddTreeNode(pv_nodeParent, v_strMenuKey, v_strRPID & " - " & v_strCmdName, "Y", v_intIndex)
                    'hParentsFilter.Add(v_strRPID, v_strRPID)
                Next v_intCount
            End If

        Catch ex As Exception
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            '             & "Error code: System error!" & vbNewLine _
            '             & "Error message: " & ex.Message, EventLogEntryType.Error)
            'MsgBox(ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
            Throw ex
        End Try
    End Sub

    Private Sub TreeView_Click(ByVal sender As Object, ByVal e As Xceed.SmartUI.SmartItemClickEventArgs)
        Dim pv_treeNode As Node
        'Update mouse pointer
        Cursor = Cursors.WaitCursor
        Try
            pv_treeNode = e.Item
            If pv_treeNode.Key <> "" Then
                lbFunc.Text = pv_treeNode.Text
                ShowAccessRight(pv_treeNode)
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
                    lbFunc.Text = v_node.Text
                    ShowAccessRight(v_node)
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
        Dim v_strCreate, v_strView, v_strPrint, v_strExport, v_strGrpAuth As String
        Dim v_strAuth, v_strCMDID, v_arrAuth(), v_strTellerAuth As String

        Try

            If pv_treeNode.Key <> "" Then
                EnableAssignment()
                v_arrMenuKey = pv_treeNode.Key.Split("|")
                v_strCMDID = v_arrMenuKey(0)
                If Not hReportFilter(v_strCMDID) Is Nothing Then
                    v_arrAuth = CStr(hReportFilter(v_strCMDID)).Split("|")
                    v_strAuth = v_arrAuth(1)
                Else
                    v_strAuth = "NNNNN"
                End If

                If AssignType = "User" Then

                    If Not hReportGrpFilter(v_strCMDID) Is Nothing Then
                        v_arrAuth = CStr(hReportGrpFilter(v_strCMDID)).Split("|")
                        v_strGrpAuth = v_arrAuth(1)
                    Else
                        v_strGrpAuth = "NNNNN"
                    End If

                    v_strAuth = MergeAuth(v_strAuth, v_strGrpAuth)
                End If

                If Not hReportTellerFilter(v_strCMDID) Is Nothing Then
                    v_arrAuth = CStr(hReportTellerFilter(v_strCMDID)).Split("|")
                    v_strTellerAuth = v_arrAuth(1)
                Else
                    v_strTellerAuth = "NNNNN"
                End If

                If v_strAuth <> Nothing Then
                    v_strCreate = Mid(v_strAuth, 2, 1)
                    v_strView = Mid(v_strAuth, 3, 1)
                    v_strPrint = Mid(v_strAuth, 4, 1)
                    v_strExport = Mid(v_strAuth, 5, 1)

                    'Display Access right
                    chkCreate.Checked = (v_strCreate = "Y")
                    chkView.Checked = (v_strView = "Y")
                    chkPrint.Checked = (v_strPrint = "Y")
                    chkExport.Checked = (v_strExport = "Y")
                End If
                If AssignType = "User" Then
                    v_strTellerAuth = MergeAuth1(v_strTellerAuth, v_strGrpAuth)
                    'DisableGroupAccessRight(v_strGrpAuth)
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
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                          & "Error code: System error!" & vbNewLine _
                          & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Sub

    Private Function MergeAuth(ByVal pv_strAuth1 As String, ByVal pv_strAuth2 As String) As String
        Dim v_strAuth As String
        v_strAuth = IIf(Mid(pv_strAuth1, 1, 1) = Mid(pv_strAuth2, 1, 1), Mid(pv_strAuth2, 1, 1), "Y")
        v_strAuth &= IIf(Mid(pv_strAuth1, 2, 1) = Mid(pv_strAuth2, 2, 1), Mid(pv_strAuth2, 2, 1), "Y")
        v_strAuth &= IIf(Mid(pv_strAuth1, 3, 1) = Mid(pv_strAuth2, 3, 1), Mid(pv_strAuth2, 3, 1), "Y")
        v_strAuth &= IIf(Mid(pv_strAuth1, 4, 1) = Mid(pv_strAuth2, 4, 1), Mid(pv_strAuth2, 4, 1), "Y")
        v_strAuth &= IIf(Mid(pv_strAuth1, 5, 1) = Mid(pv_strAuth2, 5, 1), Mid(pv_strAuth2, 5, 1), "Y")
        Return v_strAuth
    End Function

    Private Function MergeAuth1(ByVal pv_strTeller As String, ByVal pv_strGroup As String) As String
        Dim v_strAuth As String = ""
        v_strAuth &= IIf(Mid(pv_strGroup, 1, 1) = "Y", "N", Mid(pv_strTeller, 1, 1))
        v_strAuth &= IIf(Mid(pv_strGroup, 2, 1) = "Y", "N", Mid(pv_strTeller, 2, 1))
        v_strAuth &= IIf(Mid(pv_strGroup, 3, 1) = "Y", "N", Mid(pv_strTeller, 3, 1))
        v_strAuth &= IIf(Mid(pv_strGroup, 4, 1) = "Y", "N", Mid(pv_strTeller, 4, 1))
        v_strAuth &= IIf(Mid(pv_strGroup, 5, 1) = "Y", "N", Mid(pv_strTeller, 5, 1))
        Return v_strAuth
    End Function

    Private Sub DisableGroupAccessRight(ByVal v_strAuth As String)
        Dim v_strCreate, v_strPrint, v_strExport, v_strView As String

        Try


            If v_strAuth <> Nothing Then
                v_strCreate = Mid(v_strAuth, 2, 1)
                v_strView = Mid(v_strAuth, 3, 1)
                v_strPrint = Mid(v_strAuth, 4, 1)
                v_strExport = Mid(v_strAuth, 5, 1)

                'Enable Access right
                chkCreate.Enabled = (v_strCreate <> "Y")
                chkPrint.Enabled = (v_strPrint <> "Y")
                chkView.Enabled = (v_strView <> "Y")
                chkExport.Enabled = (v_strExport <> "Y")
            End If


        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                          & "Error code: System error!" & vbNewLine _
                          & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Sub

    Private Sub DisableTellerAccessRight(ByVal v_strAuth As String)
        Dim v_strCreate, v_strPrint, v_strExport, v_strView As String

        Try


            If v_strAuth <> Nothing Then
                v_strCreate = Mid(v_strAuth, 2, 1)
                v_strView = Mid(v_strAuth, 3, 1)
                v_strPrint = Mid(v_strAuth, 4, 1)
                v_strExport = Mid(v_strAuth, 5, 1)

                'Enable Access right
                chkCreate.Enabled = (v_strCreate <> "N")
                chkPrint.Enabled = (v_strPrint <> "N")
                chkView.Enabled = (v_strView <> "N")
                chkExport.Enabled = (v_strExport <> "N")
            End If


        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                          & "Error code: System error!" & vbNewLine _
                          & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
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
            chkCreate.Enabled = False
            chkPrint.Enabled = False
            chkView.Enabled = False
            chkExport.Enabled = False
        Catch ex As Exception

        End Try
    End Sub


    ''------------------------------------------------''
    ''-- Lấy các thông tin phân quyền đã có của NNSD --''
    ''------------------------------------------------''
    Private Sub FillData()
        Try
            Dim v_strFLDNAME, v_strValue As String

            'Get Function access right of user
            Dim v_strCmdInquiryFunc As String, v_strFuncObjMsg As String
            v_strCmdInquiryFunc = "SELECT a.rpttitle, a.en_rpttitle, a.objname, a.modcode, a.rptid, c.cmdallow," _
                                    & " c.strauth FROM rpreports a, cmdauth c" _
                                    & " WHERE a.rptid = c.cmdcode and a.deleted =0 and c.deleted =0 AND c.AUTHID = '" & GroupId & "'" _
                                    & " UNION " _
                                    & "SELECT a.cmdname rpttitle, a.en_cmdname en_rpttitle, a.objname, a.modcode," _
                                    & " a.cmdid rptid, c.cmdallow, c.strauth FROM cmdmenu a, cmdauth c" _
                                    & " WHERE a.cmdid = c.cmdcode and a.deleted =0 and c.deleted =0 AND c.AUTHID = '" & GroupId & "' AND a.menutype='R'"
            v_strFuncObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLGROUPS, gc_ActionInquiry, v_strCmdInquiryFunc)

            Dim v_lngError As Long = Proxy.Message(v_strFuncObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Exit Sub
            End If

            Dim v_xmlFuncDocument As New Xml.XmlDocument
            Dim v_nodeFuncList As Xml.XmlNodeList

            v_xmlFuncDocument.LoadXml(v_strFuncObjMsg)
            v_nodeFuncList = v_xmlFuncDocument.SelectNodes("/ObjectMessage/ObjData")
            Dim v_strCMDID As String, v_strCMDALLOW As String
            Dim v_strAUTH As String, v_strHashValue As String
            For i As Integer = 0 To v_nodeFuncList.Count - 1
                For j As Integer = 0 To v_nodeFuncList.Item(i).ChildNodes.Count - 1
                    With v_nodeFuncList.Item(i).ChildNodes(j)
                        v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                        v_strValue = .InnerText.ToString
                    End With
                    Select Case Trim(v_strFLDNAME)
                        Case "RPTID"
                            v_strCMDID = v_strValue
                        Case "CMDALLOW"
                            v_strCMDALLOW = v_strValue
                        Case "STRAUTH"
                            v_strAUTH = v_strValue
                    End Select
                Next
                'Fill to hashtable
                v_strHashValue = v_strCMDID & "|" & v_strCMDALLOW & v_strAUTH
                hReportFilter.Add(v_strCMDID, v_strHashValue)
                mv_strFuncAuth &= v_strHashValue & "#"
            Next

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    ''------------------------------------------------''
    ''-- Lấy các thông tin phân quyền đã có của NSD --''
    ''------------------------------------------------''
    Private Sub FillUserData()
        Try
            Dim v_strFLDNAME, v_strValue As String

            'Get Function access right of user
            Dim v_strCmdInquiryFunc As String, v_strFuncObjMsg As String
            v_strCmdInquiryFunc = "SELECT a.objname, a.modcode, a.rptid, c.cmdallow, c.strauth, c.authtype" _
                                & " FROM rpreports a, cmdauth c WHERE a.rptid = c.cmdcode" _
                                & " AND ((c.AUTHID = '" & UserId & "' AND c.authtype = 'U')" _
                                & " OR (c.AUTHID IN (SELECT b.grpid  FROM tlgrpusers b, tlgroups d" _
                                & " WHERE tlid = '" & UserId & "' AND b.grpid = d.grpid AND d.deleted = 0 AND d.status = 0)" _
                                & " AND c.authtype = 'G'))" _
                                & " UNION" _
                                & " SELECT a.objname, a.modcode, a.cmdid rptid, c.cmdallow, c.strauth, c.authtype" _
                                & " FROM cmdmenu a, cmdauth c WHERE (a.cmdid = c.cmdcode)" _
                                & " AND ((c.AUTHID = '" & UserId & "' AND c.authtype = 'U' AND c.cmdtype='R')" _
                                & " OR (c.AUTHID IN (SELECT b.grpid FROM tlgrpusers b, tlgroups d" _
                                & " WHERE tlid = '" & UserId & "' AND b.grpid = d.grpid AND d.deleted = 0 AND d.status = 0)" _
                                & " AND c.authtype = 'G' AND c.cmdtype='R'))"
            v_strFuncObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLGROUPS, gc_ActionInquiry, v_strCmdInquiryFunc)

            Dim v_lngError As Long = Proxy.Message(v_strFuncObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Exit Sub
            End If

            Dim v_xmlFuncDocument As New Xml.XmlDocument
            Dim v_nodeFuncList As Xml.XmlNodeList

            v_xmlFuncDocument.LoadXml(v_strFuncObjMsg)
            v_nodeFuncList = v_xmlFuncDocument.SelectNodes("/ObjectMessage/ObjData")
            Dim v_strCMDID As String, v_strCMDALLOW As String
            Dim v_strAUTH As String, v_strHashValue, v_strHashKey As String
            Dim v_strAuthType As String
            For i As Integer = 0 To v_nodeFuncList.Count - 1
                For j As Integer = 0 To v_nodeFuncList.Item(i).ChildNodes.Count - 1
                    With v_nodeFuncList.Item(i).ChildNodes(j)
                        v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                        v_strValue = .InnerText.ToString
                        End With
                    Select Case Trim(v_strFLDNAME)
                            Case "RPTID"
                                v_strCMDID = v_strValue
                            Case "CMDALLOW"
                                v_strCMDALLOW = v_strValue
                            Case "STRAUTH"
                                v_strAUTH = v_strValue
                            Case "AUTHTYPE"
                                v_strAuthType = v_strValue
                        End Select
                    Next
                    'Fill to hashtable
                    If v_strAuthType = "U" Then
                        v_strHashValue = v_strCMDID & "|" & v_strCMDALLOW & v_strAUTH
                        hReportFilter.Add(v_strCMDID, v_strHashValue)
                        mv_strFuncAuth &= v_strHashValue & "#"
                    Else
                        If Not hReportGrpFilter(v_strCMDID) Is Nothing Then
                            Dim v_strOldHashVale As String
                            Dim v_strTempAuth As String
                            v_strOldHashVale = hReportGrpFilter(v_strCMDID)
                            hReportGrpFilter.Remove(v_strCMDID)
                            mv_strFuncGrpAuth = Replace(mv_strFuncGrpAuth, v_strOldHashVale & "#", "")
                            v_strTempAuth = v_strOldHashVale.Split("|")(1)
                            v_strAUTH = MergeAuth(v_strTempAuth, v_strCMDALLOW & v_strAUTH)
                            v_strHashKey = v_strCMDID & "|" & v_strAUTH
                            mv_strFuncGrpAuth &= v_strHashKey & "#"
                            hReportGrpFilter.Add(v_strCMDID, v_strHashKey)
                        Else
                            v_strHashKey = v_strCMDID & "|" & v_strCMDALLOW & v_strAUTH
                            hReportGrpFilter.Add(v_strCMDID, v_strHashKey)
                            mv_strFuncGrpAuth &= v_strHashKey & "#"
                        End If
                    End If
                Next

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    ' ''------------------------------------------------''
    ' ''-- Lấy các thông tin phân quyền đã có của NSD đang đăng nhập --''
    ' ''------------------------------------------------''
    Private Sub FillTellerData()
        Try
            Dim v_strFLDNAME, v_strValue As String

            'Get Function access right of user
            Dim v_strCmdInquiryFunc As String, v_strFuncObjMsg As String
            'v_strCmdInquiryFunc = "SELECT a.objname, a.modcode, a.rptid, c.cmdallow, c.strauth, c.authtype" _
            '                        & " FROM rpreports a, cmdauth c WHERE (a.rptid = c.cmdcode)" _
            '                        & " AND ((c.AUTHID = '" & TellerId & "' AND c.AUTHTYPE='U')" _
            '                        & " OR (c.AUTHID IN (SELECT b.grpid FROM tlgrpusers b, tlgroups d WHERE tlid = '" & TellerId & "'" _
            '                        & " AND b.grpid=d.grpid AND d.deleted=0 AND d.status=0) AND c.AUTHTYPE='G'))"
            v_strFuncObjMsg = BuildXMLObjMsg(Now.Date, m_BusLayer.CurrentTellerProfile.BranchId, , _
                    m_BusLayer.CurrentTellerProfile.TellerId, CommonLibrary.gc_IsLocalMsg, CommonLibrary.gc_MsgTypeObj, _
                    OBJNAME_SY_AUTHENTICATION, CommonLibrary.gc_ActionInquiry, , m_BusLayer.CurrentTellerProfile.TellerId, "GetTellerReportData")

            Dim v_lngError As Long = Proxy.Message(v_strFuncObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Exit Sub
            End If

            Dim v_xmlFuncDocument As New Xml.XmlDocument
            Dim v_nodeFuncList As Xml.XmlNodeList

            v_xmlFuncDocument.LoadXml(v_strFuncObjMsg)
            v_nodeFuncList = v_xmlFuncDocument.SelectNodes("/ObjectMessage/ObjData")
            Dim v_strCMDID As String, v_strCMDALLOW As String
            Dim v_strAUTH As String, v_strHashValue, v_strHashKey As String
            Dim v_strAuthType As String
            For i As Integer = 0 To v_nodeFuncList.Count - 1
                For j As Integer = 0 To v_nodeFuncList.Item(i).ChildNodes.Count - 1
                    With v_nodeFuncList.Item(i).ChildNodes(j)
                        v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                        v_strValue = .InnerText.ToString
                    End With
                    Select Case Trim(v_strFLDNAME)
                        Case "RPTID"
                            v_strCMDID = v_strValue
                        Case "CMDALLOW"
                            v_strCMDALLOW = v_strValue
                        Case "STRAUTH"
                            v_strAUTH = v_strValue
                        Case "AUTHTYPE"
                            v_strAuthType = v_strValue
                    End Select
                Next
                'Fill to hashtable
                If Not hReportTellerFilter(v_strCMDID) Is Nothing Then
                    Dim v_strOldHashVale As String
                    Dim v_strTempAuth As String
                    v_strOldHashVale = hReportTellerFilter(v_strCMDID)
                    hReportTellerFilter.Remove(v_strCMDID)

                    v_strTempAuth = v_strOldHashVale.Split("|")(1)
                    v_strAUTH = MergeAuth(v_strTempAuth, v_strCMDALLOW & v_strAUTH)
                    v_strHashKey = v_strCMDID & "|" & v_strAUTH

                    hReportTellerFilter.Add(v_strCMDID, v_strHashKey)
                Else
                    v_strHashKey = v_strCMDID & "|" & v_strCMDALLOW & v_strAUTH
                    hReportTellerFilter.Add(v_strCMDID, v_strHashKey)
                End If

            Next

        Catch ex As Exception
            Throw ex
        End Try
    End Sub


    Private Sub OnSave()
        Dim v_strObjMsg As String = ""
        Dim v_strAllStrAuth, v_strGrpUsrId, v_strAuthString As String

        Cursor = Cursors.WaitCursor
        btnOk.Enabled = False
        Try

            If AssignType = "User" Then
                'Get strAuth from menu            
                'v_strAllStrAuth = GetAuthString(stvFuncMenu.Items(0))
                v_strAllStrAuth = UserId & "$" & mv_strFuncAuth

                'Build XML message
                v_strObjMsg = BuildXMLObjMsg(Now.Date, , , TellerId, gc_IsNotLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLPROFILES, gc_ActionAdhoc, , v_strAllStrAuth, "ReportAssignment")
            ElseIf AssignType = "Group" Then
               
                v_strAuthString = mv_strFuncAuth
                'Add Groupid and Usersid string to strAuth
                v_strAllStrAuth = GroupId & "$" & v_strAuthString

                'Build XML message
                v_strObjMsg = BuildXMLObjMsg(Now.Date, , , TellerId, gc_IsNotLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLGROUPS, gc_ActionAdhoc, , v_strAllStrAuth, "ReportAssignment")
            End If

            'Dim v_ws As New BDSDelivery.BDSDelivery
            Dim v_lngError As Long = Proxy.Message(v_strObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                Me.Cursor = Cursors.Default
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Exit Sub
            End If

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

            MessageBox.Show(mv_ResourceManager.GetString("SavingFailed") & vbCrLf & ex.Message, gc_ApplicationTitle, MessageBoxButtons.OK)
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
            mv_ResourceManager = New Resources.ResourceManager(gc_RootNamespace & ".frmReportAssign_" & UserLanguage, System.Reflection.Assembly.GetExecutingAssembly())
            LoadUserInterface(Me)
            'mv_BDSDelivery = New BDSChannel.BDSDelivery
            If AssignType = "Group" Then
                FillData()
            Else
                FillUserData()
            End If
            FillTreeView()
            FillTellerData()
            DisableAssignment()

            'Check trang thai
            Dim v_strSQL, v_strObjMsg As String
            If AssignType = "Group" Then
                v_strSQL = "SELECT STATUS FROM TLGROUPS WHERE GRPID='" & GroupId & "'"
            Else
                v_strSQL = "SELECT STATUS FROM TLPROFILES WHERE TLID='" & UserId & "'"
            End If
            v_strObjMsg = BuildXMLObjMsg(, , , , gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLPROFILES, gc_ActionInquiry, v_strSQL)

            'Dim v_wsOutGrp As New BDSDelivery.BDSDelivery
            Proxy.Message(v_strObjMsg)

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
            Me.Text = mv_ResourceManager.GetString("frmReportAssign")
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
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try

    End Sub

    Private Sub ChangeStatusCheckBox(ByVal v_bln As Boolean)
        Dim v_ctrl As Control
        For Each v_ctrl In Me.grbAccessRight.Controls
            If TypeOf (v_ctrl) Is CheckBox Then
                CType(v_ctrl, CheckBox).Checked = v_bln
            End If
        Next
    End Sub

    ''-------------------------------------------------------------''
    ''-- + Mục đích: Thay đổi và cập nhật các giá trị phân quyền --''
    ''--             lên key của node of menu tree               --''
    ''-- + Đầu vào: - sender: Checkbox có giá trị thay đổi       --''
    ''--            - pv_treeNode: Node hiện tại của menu tree   --''
    ''-- + Đầu ra: N/A                                           --''
    ''-- + Tác giả: Nguyễn Minh Thơ                            --''
    ''-- + Ghi chú: N/A                                          --''
    ''-------------------------------------------------------------''
    Private Sub DoCheckboxChange(ByVal sender As Object, ByVal pv_treeNode As Node)
        Dim v_strCMDALLOW As String, v_strCreate As String, v_strDelete As String
        Dim v_strExport As String, v_strView As String, v_strPrint As String = ""
        Dim v_strCMDID, v_strAuth, v_strHashValue, v_arrMenuKey(), v_arrAuth() As String

        Try

            'Get auth string and CMDID
            v_arrMenuKey = pv_treeNode.Key.Split("|")
            v_strCMDID = v_arrMenuKey(0)
            If Not hReportFilter(v_strCMDID) Is Nothing Then
                v_arrAuth = CStr(hReportFilter(v_strCMDID)).Split("|")
                v_strAuth = v_arrAuth(1)
            Else
                v_strAuth = "NNNNN"
            End If

            If v_strAuth <> Nothing Then
                'If ckbAll's value has changed
                If (sender Is chkCreate) Then
                    If chkCreate.Checked Then
                        v_strCMDALLOW = "Y"
                        v_strCreate = "Y"
                        v_strAuth = v_strCMDALLOW & v_strCreate & Mid(v_strAuth, 3, 3)
                    Else
                        v_strCreate = "N"
                        v_strAuth = Mid(v_strAuth, 1, 1) & v_strCreate & Mid(v_strAuth, 3, 3)
                    End If
                    'If ckbView's value has changed
                ElseIf (sender Is chkView) Then
                    If chkView.Checked Then
                        v_strView = "Y"
                        v_strCMDALLOW = "Y"
                        If chkCreate.Checked = False Then
                            chkCreate.Checked = True
                        End If
                        v_strCreate = "Y"
                        v_strAuth = v_strCMDALLOW & v_strCreate & v_strView & Mid(v_strAuth, 4, 2)
                    Else
                        v_strView = "N"
                        v_strAuth = Mid(v_strAuth, 1, 2) & v_strView & Mid(v_strAuth, 4, 2)
                    End If
                    'If ckbView's value has changed
                ElseIf (sender Is chkPrint) Then
                    If chkPrint.Checked Then
                        v_strPrint = "Y"
                        v_strCMDALLOW = "Y"

                        If chkCreate.Checked = False Then
                            chkCreate.Checked = True
                        End If
                        v_strCreate = "Y"

                        If chkView.Checked = False Then
                            chkView.Checked = True
                        End If
                        v_strView = "Y"

                        v_strAuth = v_strCMDALLOW & v_strCreate & v_strView & v_strPrint & Mid(v_strAuth, 5, 1)
                    Else
                        v_strPrint = "N"
                        v_strAuth = Mid(v_strAuth, 1, 3) & v_strPrint & Mid(v_strAuth, 5, 1)
                    End If
                    'If ckbExport value has changed
                ElseIf (sender Is chkExport) Then
                    If chkExport.Checked Then
                        v_strExport = "Y"
                        v_strCMDALLOW = "Y"

                        If chkCreate.Checked = False Then
                            chkCreate.Checked = True
                        End If
                        v_strCreate = "Y"

                        If chkView.Checked = False Then
                            chkView.Checked = True
                        End If
                        v_strView = "Y"

                        v_strAuth = v_strCMDALLOW & v_strCreate & v_strView & Mid(v_strAuth, 4, 1) & v_strExport
                    Else
                        v_strExport = "N"
                        v_strAuth = Mid(v_strAuth, 1, 4) & v_strExport
                    End If

                End If

                v_strHashValue = v_strCMDID & "|" & v_strAuth
                Dim v_strOldHashValue As String
                If (v_strAuth = "NNNNN") Or (v_strAuth = "YNNNN") Then
                    If Not hReportFilter(v_strCMDID) Is Nothing Then
                        v_strOldHashValue = hReportFilter(v_strCMDID)
                        hReportFilter.Remove(v_strCMDID)
                        mv_strFuncAuth = Replace(mv_strFuncAuth, v_strOldHashValue & "#", "").Trim
                    End If
                Else
                    'Update new value to hash table
                    If Not hReportFilter(v_strCMDID) Is Nothing Then
                        'Remove old value before add new value to hash table and auth's string
                        v_strOldHashValue = hReportFilter(v_strCMDID)
                        hReportFilter.Remove(v_strCMDID)
                        mv_strFuncAuth = Replace(mv_strFuncAuth, v_strOldHashValue & "#", "").Trim
                        hReportFilter.Add(v_strCMDID, v_strHashValue)
                        mv_strFuncAuth &= v_strHashValue & "#"
                    Else
                        'Add new value to hash table and auth's string
                        hReportFilter.Add(v_strCMDID, v_strHashValue)
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
        Dim v_strPRID, v_strPRID1, v_strCMDID, v_strAuth As String

        Try
            Dim v_treeNodeParent As Node
            Dim v_strAuthStr As String = "YYYYYY"
            Dim bln As Boolean
            Dim v_strHashValue As String

            v_treeNodeParent = pv_treeNode.ParentItem
            bln = False
            For v_int As Integer = 0 To v_treeNodeParent.Items.Count - 1
                v_strCMDID = v_treeNodeParent.Items(v_int).Key.Split("|")(0)
                If Not hReportFilter(v_strCMDID) Is Nothing Then
                    v_strAuth = hReportFilter(v_strCMDID).ToString.Split("|")(1)
                    bln = (Mid(v_strAuth, 1, 1) = "Y")
                    If bln Then Exit For
                End If
            Next
            v_strPRID = v_treeNodeParent.Key.Split("|")(0)
            v_strPRID1 = v_treeNodeParent.Key.Split("|")(3)
            If bln Then
                If hReportFilter(v_strPRID) Is Nothing Then
                    v_strHashValue = v_strPRID & "|" & v_strAuthStr
                    hReportFilter.Add(v_strPRID, v_strHashValue)
                    mv_strFuncAuth &= v_strHashValue & "#"
                End If
                If hReportFilter(v_strPRID1) Is Nothing Then
                    v_strHashValue = v_strPRID1 & "|" & v_strAuthStr
                    hReportFilter.Add(v_strPRID1, v_strHashValue)
                    mv_strFuncAuth &= v_strHashValue & "#"
                End If
            Else
                If Not hReportFilter(v_strPRID) Is Nothing Then
                    hReportFilter.Remove(v_strPRID)
                    v_strHashValue = v_strPRID & "|" & v_strAuthStr
                    mv_strFuncAuth = Replace(mv_strFuncAuth, v_strHashValue & "#", "")
                End If
                If Not hReportFilter(v_strPRID1) Is Nothing Then
                    hReportFilter.Remove(v_strPRID1)
                    v_strHashValue = v_strPRID1 & "|" & v_strAuthStr
                    mv_strFuncAuth = Replace(mv_strFuncAuth, v_strHashValue & "#", "")
                End If
            End If

        Catch ex As Exception
            Throw ex
        End Try

    End Sub
#End Region

#Region "Sự kiện Form"
    Private Sub frmFuncAssign_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'mv_BDSDelivery = New BDSChannel.BDSDelivery
        'Me.OnInit()
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        OnSave()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub CheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCreate.Click, chkPrint.Click, chkView.Click, chkExport.Click
        DoCheckboxChange(sender, stvTran.SelectedItem)
    End Sub

#End Region

End Class