Imports Sats.CommonLibrary
Imports Sats.AppCore
Imports Xceed.SmartUI.Controls.TreeView

Public Class frmTranAssign
    Private mv_oProxy As BDSChannel.BDSDelivery

    Public Property Proxy() As BDSChannel.BDSDelivery
        Get
            Return mv_oProxy
        End Get
        Set(ByVal value As BDSChannel.BDSDelivery)
            mv_oProxy = value
        End Set
    End Property
#Region " Declare constants and variables "
    Private mv_ResourceManager As Resources.ResourceManager
    Private mv_strLanguage As String

    Private mv_intExecFlag As Integer
    Private mv_strModuleCode As String
    Private mv_strObjectName As String
    Private mv_strTableName As String
    Private mv_strBranchId As String
    Private mv_strTellerId As String
    Private mv_strUserId As String
    Private mv_strUserName As String
    Private mv_strGroupId As String
    Private mv_strGroupName As String
    Private mv_strAssignType As String
    Private mv_strMIInUser As String
    Private mv_strMIOutUser As String
    Private mv_strBrid As String
    Private mv_blnBridStatus As Boolean

    Private mv_arrObjFields() As CFieldMaster

    Private mv_strTLAuthString As String
    Private mv_strTLGrpAuthString As String
    Private mv_strFuncAuthString As String
    Private mv_strFuncGrpAuthString As String

    'This node will be used as a temp node
    Private mv_node As New Node
    'Private mv_textbox As New System.Windows.Forms.TextBox

    Private mv_strLocalObject As String

    Dim hTLAuthFilter As New Hashtable
    Dim hTLTellerAuthFilter As New Hashtable
    Dim hTLGrpAuthFilter As New Hashtable
    Dim hFuncAuthFilter As New Hashtable
    Dim hFuncTellerAuthFilter As New Hashtable
    Dim hFuncGrpAuthFilter As New Hashtable


    'Private mv_BDSDelivery As BDSChannel.BDSDelivery
    'Private mv_node As Node
    Private mv_blnOK As Boolean

#End Region

    Private mv_arrNode As List(Of Node)

#Region " Properties "

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

#Region "Fill cây giao dịch"
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

    Public Sub GetTreeView(Optional ByVal IsShown As Boolean = True)
        Dim v_strObjMsg As String
        Dim v_nodeList As Xml.XmlNodeList
        Dim XmlDocument As New Xml.XmlDocument
        Dim v_int, v_intCount, v_intIndex As Integer
        Dim v_strFLDNAME As String = "", v_strValue As String = "", v_strPRID As String = ""
        Dim v_strCMDID As String = "", v_strCmdName As String = "", v_strCmdCode As String = ""
        Dim v_strLast, v_strLev, v_strPreLev As String
        Dim v_strMenuKey, v_strMenuType, v_strModCode, v_strAuthCode, v_strObjName, v_strAuthString As String
        Dim v_nodeParent, v_tempNode As Node
        Dim v_blnFirst As Boolean = True

        Try
            If IsShown Then
                'bangpv
                v_strObjMsg = CommonLibrary.BuildXMLObjMsg(Now.Date, mv_strBrid, Now.Date, _
                    m_BusLayer.CurrentTellerProfile.TellerId, CommonLibrary.gc_IsLocalMsg, CommonLibrary.gc_MsgTypeObj, _
                    OBJNAME_SY_AUTHENTICATION, CommonLibrary.gc_ActionInquiry, , m_BusLayer.CurrentTellerProfile.TellerId, "GetTranTreeMenu")
                m_BusLayer.BusSystemMessage(v_strObjMsg)
                'v_strObjMsg = CommonLibrary.BuildXMLObjMsg(Now.Date, m_BusLayer.CurrentTellerProfile.BranchId, Now.Date, _
                '    m_BusLayer.CurrentTellerProfile.TellerId, CommonLibrary.gc_IsLocalMsg, CommonLibrary.gc_MsgTypeObj, _
                '    OBJNAME_SY_AUTHENTICATION, CommonLibrary.gc_ActionInquiry, , m_BusLayer.CurrentTellerProfile.TellerId, "GetTranTreeMenu")
                'm_BusLayer.BusSystemMessage(v_strObjMsg)
                'end bangpv
                XmlDocument.LoadXml(v_strObjMsg)
                v_nodeList = XmlDocument.SelectNodes("/ObjectMessage/ObjData")
                'bangpv
                Me.stvTrans.Items(0).Items.Clear()
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
                                Case "MENUCODE"
                                    v_strCmdCode = Trim(v_strValue)
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
                                v_strTempAuth = v_tempNode.Key.Split("|")(5)
                                v_strAuthString = MergeAuth(v_strTempAuth, v_strAuthString)
                                v_strMenuKey = v_strLast & "|" & v_strLev & "|" & v_strCMDID & "|" & v_strCmdCode & "|" & v_strAuthCode & "|" & v_strAuthString
                                v_tempNode.Key = v_strMenuKey
                            End If
                        Else

                            Dim v_node As New Node(v_strCmdName, v_intIndex)

                            If v_strLast = "Y" Then
                                v_strMenuKey = v_strLast & "|" & v_strLev & "|" & v_strCMDID & "|" & v_strCmdCode & "|" & v_strAuthCode & "|" & v_strAuthString
                                If v_strPreLev = "3" And v_strLev = "2" Then
                                    v_tempNode = AddTreeNode(v_nodeParent.ParentItem, v_strMenuKey, v_strCmdCode & " - " & v_strCmdName, v_strLast, 3)
                                Else
                                    v_tempNode = AddTreeNode(v_nodeParent, v_strMenuKey, v_strCmdCode & " - " & v_strCmdName, v_strLast, 3)
                                End If
                            Else
                                v_strMenuKey = v_strLast & v_strCMDID & "|" & v_strPRID
                                If v_strLev = "1" Then
                                    v_node.Key = CStr(v_strMenuKey)
                                    v_node.ToolTipText = v_strCmdName
                                    v_node.Expanded = False
                                    Me.stvTrans.Items(0).Items.Add(v_node)
                                    v_nodeParent = Me.stvTrans.Items(0).Items(v_strMenuKey)
                                Else
                                    If v_strPreLev = "3" And v_strLev = "2" Then
                                        v_nodeParent = AddTreeNode(v_nodeParent.ParentItem, v_strMenuKey, v_strCmdCode & " - " & v_strCmdName, v_strLast, 0)
                                    Else
                                        v_nodeParent = AddTreeNode(v_nodeParent, v_strMenuKey, v_strCmdCode & " - " & v_strCmdName, v_strLast, 0)
                                    End If
                                End If
                                v_tempNode = v_nodeParent
                            End If
                            'AddHandler v_tempNode.Click, AddressOf Me.TreeView_Click
                            'AddHandler v_tempNode.KeyUp, AddressOf Me.TreeView_KeyUp
                            v_strPreLev = v_strLev
                        End If
                    Else
                        v_blnFirst = False
                        Dim v_node As New Node(v_strCmdName, v_intIndex)
                        Dim v_strName As String
                        If v_strLast = "Y" Then
                            v_strMenuKey = v_strLast & "|" & v_strLev & "|" & v_strCMDID & "|" & v_strCmdCode & "|" & v_strAuthCode & "|" & v_strAuthString

                            If v_strPreLev = "3" And v_strLev = "2" Then
                                AddTreeNode(v_nodeParent.ParentItem, v_strMenuKey, v_strCmdCode & " - " & v_strCmdName, v_strLast, 3)
                            Else
                                AddTreeNode(v_nodeParent, v_strMenuKey, v_strCmdCode & " - " & v_strCmdName, v_strLast, 3)
                            End If
                        Else
                            v_strMenuKey = v_strLast & v_strCMDID & "|" & v_strPRID
                            If v_strLev = "1" Then
                                v_node.Key = CStr(v_strMenuKey)
                                v_node.ToolTipText = v_strCmdName
                                v_node.Expanded = False
                                Me.stvTrans.Items(0).Items.Add(v_node)
                                v_nodeParent = Me.stvTrans.Items(0).Items(v_strMenuKey)
                            Else
                                If v_strPreLev = "3" And v_strLev = "2" Then
                                    v_nodeParent = AddTreeNode(v_nodeParent.ParentItem, v_strMenuKey, v_strCmdName, v_strLast, 0)
                                Else
                                    v_nodeParent = AddTreeNode(v_nodeParent, v_strMenuKey, v_strCmdName, v_strLast, 0)
                                End If
                            End If

                            v_tempNode = v_nodeParent
                        End If
                        'AddHandler v_tempNode.Click, AddressOf Me.TreeView_Click
                        'AddHandler v_tempNode.KeyUp, AddressOf Me.TreeView_KeyUp
                        v_strPreLev = v_strLev
                    End If
                    'Dim v_node As New Node(v_strCmdName, v_intIndex)

                    'If v_strLast = "Y" Then
                    '    v_strMenuKey = v_strLast & "|" & v_strLev & "|" & v_strCMDID & "|" & v_strCmdCode & "|" & v_strAuthCode & "|" & v_strAuthString
                    '    If v_strPreLev = "3" And v_strLev = "2" Then
                    '        AddTreeNode(v_nodeParent.ParentItem, v_strMenuKey, v_strCmdCode & " - " & v_strCmdName, v_strLast, 3)
                    '    Else
                    '        AddTreeNode(v_nodeParent, v_strMenuKey, v_strCmdCode & " - " & v_strCmdName, v_strLast, 3)
                    '    End If
                    'Else
                    '    v_strMenuKey = v_strLast & "|" & v_strLev & "|" & v_strCMDID
                    '    If v_strLev = "1" Then
                    '        v_node.Key = CStr(v_strMenuKey)
                    '        v_node.ToolTipText = v_strCmdName
                    '        v_node.Expanded = False
                    '        Me.stvTrans.Items(0).Items.Add(v_node)
                    '        v_nodeParent = Me.stvTrans.Items(0).Items(v_strMenuKey)
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
                Me.stvTrans.Items(0).Items.Clear()
            End If

        Catch ex As Exception
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            '            & "Error code: System error!" & vbNewLine _
            '            & "Error message: " & ex.Message, EventLogEntryType.Error, )
            'MsgBox(ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
            Throw ex
        End Try
    End Sub


    Private Sub FillUserData(ByVal v_strBrid As String)
        Try
            Dim v_strFLDNAME, v_strValue As String
            'Dim v_strCMDBrid As String = "CMDBRID"

            'Get Function access right of user
            Dim v_strCmdInquiryFunc As String, v_strFuncObjMsg As String = ""

            v_strCmdInquiryFunc = "SELECT   cmdid cmdid, LAST LAST, cmdallow cmdallow," _
                                    & " strauth strauth, authtype authtype" _
                                    & " FROM (SELECT m.cmdid cmdid, m.LAST LAST, a.cmdallow cmdallow," _
                                    & " a.strauth strauth, a.authtype" _
                                    & " FROM cmdmenu m, cmdauth a" _
                                    & " WHERE a.brid ='" & v_strBrid & "' AND m.cmdid = a.cmdcode" _
                                    & " AND ((a.AUTHID = '" & UserId & "' AND a.authtype = 'U')" _
                                    & " OR (a.AUTHID IN (SELECT grpid FROM tlgrpusers" _
                                    & " WHERE tlid = '" & UserId & "') AND a.authtype = 'G'))" _
                                    & " AND a.cmdtype = 'T'" _
                                    & " UNION " _
                                    & " SELECT m.tltxcd cmdid, 'Y' LAST, a.cmdallow cmdallow," _
                                    & " a.strauth strauth, a.authtype" _
                                    & " FROM tltx m, cmdauth a WHERE m.tltxcd = a.cmdcode" _
                                    & " AND a.BRID ='" & v_strBrid & "'" _
                                    & " AND ((a.AUTHID = '" & UserId & "' AND a.authtype = 'U')" _
                                    & " OR (a.AUTHID IN (SELECT grpid FROM tlgrpusers" _
                                    & " WHERE tlid = '" & UserId & "') AND a.authtype = 'G'))" _
                                    & " AND a.cmdtype = 'T')" _
                                    & " ORDER BY cmdid"
            v_strFuncObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLPROFILES, gc_ActionInquiry, v_strCmdInquiryFunc)

            Dim v_lngError As Long = Proxy.Message(v_strFuncObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Exit Sub
            End If

            Dim v_xmlFuncDocument As New Xml.XmlDocument
            Dim v_nodeFuncList As Xml.XmlNodeList

            v_xmlFuncDocument.LoadXml(v_strFuncObjMsg)
            v_nodeFuncList = v_xmlFuncDocument.SelectNodes("/ObjectMessage/ObjData")
            Dim v_strCMDID As String = "", v_strLAST As String = ""
            Dim v_strCMDALLOW As String = "", v_strAUTH As String = "", v_strHashValue As String = ""
            Dim v_strAuthType As String = ""
            'BangPV: Xóa dữ liệu trong các bảng tạm

            ' hFuncAuthFilter.Clear()
            'hFuncGrpAuthFilter.Clear()
            'hTLAuthFilter.Clear()
            'hTLGrpAuthFilter.Clear()
            'end bangpv
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
                    'bangpv: thêm chi nhánh vào hashtable 
                    'v_strHashValue = v_strCMDID & "|" & v_strCMDALLOW & v_strAUTH & "|" & v_strLAST
                    v_strHashValue = v_strCMDID & "|" & v_strCMDALLOW & v_strAUTH & "|" & v_strLAST & "|" & v_strBrid
                    hFuncAuthFilter.Add(v_strCMDID, v_strHashValue)
                    mv_strFuncAuthString &= v_strHashValue & "#"
                Else
                    If Not hFuncGrpAuthFilter(v_strCMDID) Is Nothing Then
                        Dim v_strOldHashVale As String
                        Dim v_strTempAuth As String
                        v_strOldHashVale = hFuncGrpAuthFilter(v_strCMDID)
                        hFuncGrpAuthFilter.Remove(v_strCMDID)
                        mv_strFuncGrpAuthString = Replace(mv_strFuncGrpAuthString, v_strOldHashVale & "#", "")
                        v_strTempAuth = v_strOldHashVale.Split("|")(1)
                        v_strAUTH = MergeAuth(v_strTempAuth, v_strCMDALLOW & v_strAUTH)
                        'bangpv: Thêm chi nhánh vào hashtable 
                        'v_strHashValue = v_strCMDID & "|" & v_strAUTH
                        v_strHashValue = v_strCMDID & "|" & v_strAUTH & "|" & v_strBrid
                        'end bangpv
                        mv_strFuncGrpAuthString &= v_strHashValue & "#"
                        hFuncGrpAuthFilter.Add(v_strCMDID, v_strHashValue)
                    Else
                        'bangpv: thêm chi nhánh vào hashtable 
                        'v_strHashValue = v_strCMDID & "|" & v_strCMDALLOW & v_strAUTH
                        v_strHashValue = v_strCMDID & "|" & v_strCMDALLOW & v_strAUTH & "|" & v_strBrid
                        'end bangpv
                        hFuncGrpAuthFilter.Add(v_strCMDID, v_strHashValue)
                        mv_strFuncGrpAuthString &= v_strHashValue & "#"
                    End If
                End If
            Next


            v_strCmdInquiryFunc = "SELECT tltxcd, tltype, tllimit, authtype" _
                                & " FROM tlauth WHERE brid ='" & v_strBrid & "' AND (authtype = 'U' AND AUTHID = '" & UserId & "')" _
                                & " OR (AUTHID IN (SELECT grpid FROM tlgrpusers WHERE tlid = '" & UserId & "') AND authtype = 'G')"
            v_strFuncObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLGROUPS, gc_ActionInquiry, v_strCmdInquiryFunc)

            Proxy.Message(v_strFuncObjMsg)

            Dim v_strTLTXCD, v_strTLTYPE, v_strTLLIMIT As String
            v_xmlFuncDocument.LoadXml(v_strFuncObjMsg)
            v_nodeFuncList = v_xmlFuncDocument.SelectNodes("/ObjectMessage/ObjData")

            For i As Integer = 0 To v_nodeFuncList.Count - 1
                For j As Integer = 0 To v_nodeFuncList.Item(i).ChildNodes.Count - 1
                    With v_nodeFuncList.Item(i).ChildNodes(j)
                        v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                        v_strValue = .InnerText.ToString
                    End With
                    Select Case Trim(v_strFLDNAME)
                        Case "TLTXCD"
                            v_strTLTXCD = v_strValue
                        Case "TLTYPE"
                            v_strTLTYPE = v_strValue
                        Case "TLLIMIT"
                            v_strTLLIMIT = v_strValue
                        Case "AUTHTYPE"
                            v_strAuthType = v_strValue
                    End Select
                Next
                v_strTLTXCD = v_strTLTXCD & v_strBrid
                'Fill to hashtable
                If v_strAuthType = "U" Then
                    'bangpv: thêm chi nhánh vào hashtable
                    'v_strHashValue = v_strTLTXCD & "|" & v_strTLTYPE & "|" & v_strTLLIMIT
                    v_strHashValue = v_strTLTXCD & "|" & v_strTLTYPE & "|" & v_strTLLIMIT & "|" & v_strBrid
                    'end bangpv
                    hTLAuthFilter.Add(v_strTLTXCD & v_strTLTYPE, v_strHashValue)
                    mv_strTLAuthString &= v_strHashValue & "#"
                Else
                    If Not hTLGrpAuthFilter(v_strTLTXCD & v_strTLTYPE) Is Nothing Then
                        Dim v_strOldHashVale As String
                        Dim v_strTempAuth As String
                        v_strOldHashVale = hTLGrpAuthFilter(v_strTLTXCD & v_strTLTYPE)
                        hTLGrpAuthFilter.Remove(v_strTLTXCD & v_strTLTYPE)
                        mv_strTLGrpAuthString = Replace(mv_strTLGrpAuthString, v_strOldHashVale & "#", "")
                        v_strTempAuth = v_strOldHashVale.Split("|")(2)
                        If v_strTLLIMIT < v_strTempAuth Then
                            v_strTLLIMIT = v_strTempAuth
                        End If
                        'v_strAUTH = MergeAuth(v_strTempAuth, v_strCMDALLOW & v_strAUTH)
                        'bangpv: Thêm chi nhánh vào hashtable 
                        'v_strHashValue = v_strTLTXCD & "|" & v_strTLTYPE & "|" & v_strTLLIMIT
                        v_strHashValue = v_strTLTXCD & "|" & v_strTLTYPE & "|" & v_strTLLIMIT & "|" & v_strBrid
                        'end bangpv
                        mv_strTLGrpAuthString &= v_strHashValue & "#"
                        hTLGrpAuthFilter.Add(v_strTLTXCD & v_strTLTYPE, v_strHashValue)
                    Else
                        'bangpv: Thêm chi nhánh vào hashtable 
                        'v_strHashValue = v_strTLTXCD & "|" & v_strTLTYPE & "|" & v_strTLLIMIT
                        v_strHashValue = v_strTLTXCD & "|" & v_strTLTYPE & "|" & v_strTLLIMIT & "|" & v_strBrid
                        'end bangpv
                        hTLGrpAuthFilter.Add(v_strTLTXCD & v_strTLTYPE, v_strHashValue)
                        mv_strTLGrpAuthString &= v_strHashValue & "#"
                    End If
                End If
            Next

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    'bangpv
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
    'end bangpv
    Private Sub FillTellerData(ByVal v_strBrid As String)
        Try
            Dim v_strFLDNAME, v_strValue As String

            'Get Function access right of user
            Dim v_strFuncObjMsg As String = ""

            v_strFuncObjMsg = BuildXMLObjMsg(Now.Date, v_strBrid, Now.Date, _
                    m_BusLayer.CurrentTellerProfile.TellerId, CommonLibrary.gc_IsLocalMsg, CommonLibrary.gc_MsgTypeObj, _
                    OBJNAME_SY_AUTHENTICATION, CommonLibrary.gc_ActionInquiry, , m_BusLayer.CurrentTellerProfile.TellerId, "GetTellerTranData")

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
            Dim v_strCMDALLOW As String, v_strAUTH As String, v_strHashValue As String
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
                v_strCMDID = v_strCMDID & v_strBrid
                'Fill to hashtable
                'v_strHashValue = v_strCMDID & "|" & v_strCMDALLOW & v_strAUTH & "|" & v_strLAST
                'hFuncTellerAuthFilter.Add(v_strCMDID, v_strHashValue)

                If Not hFuncTellerAuthFilter(v_strCMDID) Is Nothing Then
                    Dim v_strOldHashVale As String
                    Dim v_strTempAuth As String
                    v_strOldHashVale = hFuncTellerAuthFilter(v_strCMDID)
                    hFuncTellerAuthFilter.Remove(v_strCMDID)

                    v_strTempAuth = v_strOldHashVale.Split("|")(1)
                    v_strAUTH = MergeAuth(v_strTempAuth, v_strCMDALLOW & v_strAUTH)

                    v_strHashValue = v_strCMDID & "|" & v_strAUTH & "|" & v_strBrid
                    hFuncTellerAuthFilter.Add(v_strCMDID, v_strHashValue)
                Else
                    v_strHashValue = v_strCMDID & "|" & v_strCMDALLOW & v_strAUTH & "|" & v_strBrid
                    hFuncTellerAuthFilter.Add(v_strCMDID, v_strHashValue)
                End If
            Next

            v_strFuncObjMsg = BuildXMLObjMsg(Now.Date, v_strBrid, Now.Date, _
                    m_BusLayer.CurrentTellerProfile.TellerId, CommonLibrary.gc_IsLocalMsg, CommonLibrary.gc_MsgTypeObj, _
                    OBJNAME_SY_AUTHENTICATION, CommonLibrary.gc_ActionInquiry, , m_BusLayer.CurrentTellerProfile.TellerId, "GetTellerTranLimitData")
            v_lngError = Proxy.Message(v_strFuncObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Exit Sub
            End If

            Dim v_strTLTXCD, v_strTLTYPE, v_strTLLIMIT As String
            v_xmlFuncDocument.LoadXml(v_strFuncObjMsg)
            v_nodeFuncList = v_xmlFuncDocument.SelectNodes("/ObjectMessage/ObjData")

            For i As Integer = 0 To v_nodeFuncList.Count - 1
                For j As Integer = 0 To v_nodeFuncList.Item(i).ChildNodes.Count - 1
                    With v_nodeFuncList.Item(i).ChildNodes(j)
                        v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                        v_strValue = .InnerText.ToString
                    End With
                    Select Case Trim(v_strFLDNAME)
                        Case "TLTXCD"
                            v_strTLTXCD = v_strValue
                        Case "TLTYPE"
                            v_strTLTYPE = v_strValue
                        Case "TLLIMIT"
                            v_strTLLIMIT = v_strValue
                    End Select
                Next
                If Not v_strTLTXCD Is Nothing Then
                    v_strTLTXCD = v_strTLTXCD & v_strBrid

                End If
                'Fill to hashtable
                If Not hTLTellerAuthFilter(v_strTLTXCD & v_strTLTYPE) Is Nothing Then
                    Dim v_strOldHashVale As String
                    Dim v_strTempAuth As String
                    v_strOldHashVale = hTLTellerAuthFilter(v_strTLTXCD & v_strTLTYPE)
                    hTLTellerAuthFilter.Remove(v_strTLTXCD & v_strTLTYPE)

                    v_strTempAuth = v_strOldHashVale.Split("|")(2)
                    If v_strTLLIMIT < v_strTempAuth Then
                        v_strTLLIMIT = v_strTempAuth
                    End If
                    'BằngPV: Thêm chi nhánh vào
                    v_strHashValue = v_strTLTXCD & "|" & v_strTLTYPE & "|" & v_strTLLIMIT & "|" & v_strBrid
                    hTLTellerAuthFilter.Add(v_strTLTXCD & v_strTLTYPE, v_strHashValue)
                Else
                    v_strHashValue = v_strTLTXCD & "|" & v_strTLTYPE & "|" & v_strTLLIMIT & "|" & v_strBrid
                    hTLTellerAuthFilter.Add(v_strTLTXCD & v_strTLTYPE, v_strHashValue)
                End If
            Next
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region

#Region "Private methods"

    Public Sub OnInit()
        Try
        
            mv_ResourceManager = New Resources.ResourceManager("Sats.frmTransAssign_" & UserLanguage, System.Reflection.Assembly.GetExecutingAssembly())
            LoadUserInterface(Me)
            'mv_BDSDelivery = New BDSChannel.BDSDelivery

            'If AssignType = "Group" Then
            '    FillData(mv_strBrid)
            'Else
            '    FillUserData(mv_strBrid)
            'End If
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

            AddHandler txtGD.Validating, AddressOf txtLimit_Validating
            AddHandler txtDM1.Validating, AddressOf txtLimit_Validating
            AddHandler txtDM2.Validating, AddressOf txtLimit_Validating
            AddHandler txtDM3.Validating, AddressOf txtLimit_Validating

            'DisableTextLimit()
            DisableAssignment()

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
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Sub

    Private Sub DisableTextLimit()
        txtGD.Enabled = False
        txtDM1.Enabled = False
        txtDM2.Enabled = False
        txtDM3.Enabled = False
        txtGD.Text = ""
        txtDM1.Text = ""
        txtDM2.Text = ""
        txtDM3.Text = ""
    End Sub

    Private Sub FillData(ByVal v_strBrid As String)
        Try
            Dim v_strFLDNAME, v_strValue As String

            'Get Function access right of user
            Dim v_strCmdInquiryFunc As String, v_strFuncObjMsg As String = ""

            v_strCmdInquiryFunc = "SELECT cmdid cmdid,  LAST LAST,  cmdallow cmdallow,  strauth strauth FROM (" _
                                    & " SELECT m.cmdid cmdid, m.LAST LAST, a.cmdallow cmdallow, a.strauth strauth" _
                                    & " FROM cmdmenu m, cmdauth a WHERE m.cmdid = a.cmdcode AND a.brid='" & v_strBrid & "' " _
                                    & " AND a.cmdallow = 'Y' AND a.AUTHID = '" & GroupId & "' AND a.cmdtype = 'T'" _
                                    & " UNION " _
                                    & " SELECT m.tltxcd cmdid, 'Y' LAST, a.cmdallow cmdallow, a.strauth strauth" _
                                    & " FROM tltx m, cmdauth a WHERE m.tltxcd = a.cmdcode AND a.Brid = '" & v_strBrid & "'" _
                                    & " AND a.AUTHID = '" & GroupId & "' AND a.cmdtype = 'T'AND m.deleted = 0)" _
                                    & " ORDER BY cmdid"
            v_strFuncObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLGROUPS, gc_ActionInquiry, v_strCmdInquiryFunc)

            Dim v_lngError As Long = Proxy.Message(v_strFuncObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Exit Sub
            End If

            Dim v_xmlFuncDocument As New Xml.XmlDocument
            Dim v_nodeFuncList As Xml.XmlNodeList

            v_xmlFuncDocument.LoadXml(v_strFuncObjMsg)
            v_nodeFuncList = v_xmlFuncDocument.SelectNodes("/ObjectMessage/ObjData")
            mv_strFuncAuthString = ""
            Dim v_strCMDID As String, v_strLAST As String
            Dim v_strCMDALLOW As String, v_strAUTH As String, v_strHashValue As String
            'bangpv: xóa bảng tạm chứa quyền của nsd
            If hFuncAuthFilter.Count > 0 Then
                hFuncAuthFilter.Clear()
                mv_strFuncAuthString = ""
            End If
            'end bangpv
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
                'bangpv: Thêm chi nhánh
                v_strCMDID = v_strCMDID & v_strBrid
                'Fill to hashtable
                v_strHashValue = v_strCMDID & "|" & v_strCMDALLOW & v_strAUTH & "|" & v_strLAST & "|" & v_strBrid
                hFuncAuthFilter.Add(v_strCMDID, v_strHashValue)
                mv_strFuncAuthString &= v_strHashValue & "#"
            Next


            v_strCmdInquiryFunc = "SELECT TLTXCD, TLTYPE, TLLIMIT, AUTHTYPE FROM TLAUTH WHERE AUTHTYPE = 'G' AND AUTHID = '" & GroupId & "' AND BRID ='" & v_strBrid & "'"
            v_strFuncObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLGROUPS, gc_ActionInquiry, v_strCmdInquiryFunc)
            v_lngError = Proxy.Message(v_strFuncObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Exit Sub
            End If


            Dim v_strTLTXCD, v_strTLTYPE, v_strTLLIMIT As String
            v_xmlFuncDocument.LoadXml(v_strFuncObjMsg)
            v_nodeFuncList = v_xmlFuncDocument.SelectNodes("/ObjectMessage/ObjData")
            mv_strTLAuthString = ""
            'bangpv: xóa dữ liệu trong bảng tạm
            If hTLAuthFilter.Count > 0 Then
                hTLAuthFilter.Clear()
                mv_strTLAuthString = ""
            End If
            'end bangpv
            For i As Integer = 0 To v_nodeFuncList.Count - 1
                For j As Integer = 0 To v_nodeFuncList.Item(i).ChildNodes.Count - 1
                    With v_nodeFuncList.Item(i).ChildNodes(j)
                        v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                        v_strValue = .InnerText.ToString
                    End With
                    Select Case Trim(v_strFLDNAME)
                        Case "TLTXCD"
                            v_strTLTXCD = v_strValue
                        Case "TLTYPE"
                            v_strTLTYPE = v_strValue
                        Case "TLLIMIT"
                            v_strTLLIMIT = v_strValue
                    End Select
                Next
                'BằngPV: Thêm chi nhánh 
                v_strTLTXCD = v_strTLTXCD & v_strBrid
                'Fill to hashtable
                v_strHashValue = v_strTLTXCD & "|" & v_strTLTYPE & "|" & v_strTLLIMIT & "|" & v_strBrid
                hTLAuthFilter.Add(v_strTLTXCD & v_strTLTYPE, v_strHashValue)
                mv_strTLAuthString &= v_strHashValue & "#"
            Next
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                          & "Error code: System error!" & vbNewLine _
                                          & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Sub

    Private Sub OnSave()
        Dim v_strObjMsg As String = ""
        Dim v_strAllAuthString, v_strMenuKeyAuth, v_strGrpUsrId As String
        Me.Cursor = Cursors.WaitCursor
        btnOk.Enabled = False

        Try
            If AssignType = "User" Then
                'Get Auth string from menu

                v_strAllAuthString = UserId & "$" & mv_strTLAuthString & "$" & mv_strFuncAuthString

                'Buil XML message
                v_strObjMsg = BuildXMLObjMsg(Now.Date, mv_strBrid, , TellerId, gc_IsNotLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLPROFILES, gc_ActionAdhoc, , v_strAllAuthString, "TransactionAssignment")

            ElseIf AssignType = "Group" Then

                v_strAllAuthString = GroupId & "$" & mv_strTLAuthString & "$" & mv_strFuncAuthString

                'Buil XML message
                v_strObjMsg = BuildXMLObjMsg(Now.Date, mv_strBrid, , TellerId, gc_IsNotLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLGROUPS, gc_ActionAdhoc, , v_strAllAuthString, "TransactionAssignment")
            End If

            'Dim v_ws As New BDSDelivery.BDSDelivery
            Dim v_lngError As Long = Proxy.Message(v_strObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                Me.Cursor = Cursors.Default
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Exit Sub
            End If

            ''Check infomations and errors from message
            'Dim v_strErrorSource As String = "", v_strErrorMessage As String = ""
            'Dim v_lngErrorCode As Long

            'GetErrorFromMessage(v_strObjMsg, v_strErrorSource, v_lngErrorCode, v_strErrorMessage)
            'If v_lngErrorCode <> 0 Then
            '    'Update mouse pointer
            '    Cursor = Cursors.Default
            '    MsgBox(v_strErrorMessage, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, Me.Text)
            '    Exit Sub
            'End If
            Me.Cursor = Cursors.Default
            MsgBox(mv_ResourceManager.GetString("SavingSuccess"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
            btnOk.Enabled = True
            Me.Close()

        Catch ex As Exception
            Me.Cursor = Cursors.Default
            btnOk.Enabled = True
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                     & "Error code: System error!" & vbNewLine _
                                     & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(mv_ResourceManager.GetString("SavingFailed") & vbCrLf & ex.Message, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
        End Try
    End Sub


    Private Sub LoadUserInterface(ByRef pv_ctrl As Windows.Forms.Control)
        Dim v_ctrl As Windows.Forms.Control
        'Dim v_strTellerName As String

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
                ElseIf TypeOf (v_ctrl) Is ComboBox Then
                    If CType(v_ctrl, ComboBox).Items.Count > 0 Then
                        CType(v_ctrl, ComboBox).SelectedIndex = 0
                    End If
                End If
            Next

            'Load caption of toolbar
            btnOk.Text = mv_ResourceManager.GetString("btnOK1")
            btnCancel.Text = mv_ResourceManager.GetString("btnCancel")
            'Load caption of form, label caption
            Me.Text = mv_ResourceManager.GetString("frmTransAssign")
            If AssignType = "User" Then
                lbCaption.Text = mv_ResourceManager.GetString("lbCaption1") & UserName
            ElseIf AssignType = "Group" Then
                lbCaption.Text = mv_ResourceManager.GetString("lbCaption") & GroupName
            End If
            'Disable control if in view mode
            If (ExeFlag = ExecuteFlag.View) Then
                btnOk.Enabled = False
            End If
        Catch ex As Exception
            Throw ex
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
        Dim v_strPRID, v_strPRID1, v_strLEV, v_strLast, v_strCMDID, v_strAuth, v_arrMenuKey() As String

        Try
            Dim v_treeNodeParent As Node
            Dim v_strAuthStr As String = "YNNNNNN"
            Dim bln As Boolean
            Dim v_strHashValue As String
            'Dim v_strLast As String
            'v_treeNodeParent = pv_treeNode.ParentItem

            v_arrMenuKey = pv_treeNode.Key.Split("|")
            v_strCMDID = v_arrMenuKey(2)
            v_strLEV = v_arrMenuKey(1)
            v_strLast = v_arrMenuKey(0)
            'v_strCMDID = v_strCMDID & mv_strBrid
            For i As Integer = 0 To CInt(v_strLEV) - 2
                v_treeNodeParent = pv_treeNode.ParentItem
                bln = False
                For v_int As Integer = 0 To v_treeNodeParent.Items.Count - 1
                    v_strLast = Mid(v_treeNodeParent.Items(v_int).Key, 1, 1)
                    If v_strLast = "Y" Then
                        v_strCMDID = v_treeNodeParent.Items(v_int).Key.Split("|")(2)
                    Else
                        v_strLast = Mid(v_treeNodeParent.Items(v_int).Key.Split("|")(0), 2)
                    End If
                    v_strCMDID = v_strCMDID & mv_strBrid
                    If Not hFuncAuthFilter(v_strCMDID) Is Nothing Then
                        v_strAuth = hFuncAuthFilter(v_strCMDID).ToString.Split("|")(1)
                        bln = (Mid(v_strAuth, 1, 1) = "Y")
                        If bln Then Exit For
                    End If
                Next
                v_strPRID = Mid(v_treeNodeParent.Key.Split("|")(0), 2)
                v_strPRID1 = v_treeNodeParent.Key.Split("|")(1)
                v_strPRID = v_strPRID & mv_strBrid
                v_strPRID1 = v_strPRID1 & mv_strBrid
                If bln Then
                    If hFuncAuthFilter(v_strPRID) Is Nothing Then
                        v_strHashValue = v_strPRID & "|" & v_strAuthStr & "|" & v_strLast & "|" & mv_strBrid
                        hFuncAuthFilter.Add(v_strPRID, v_strHashValue)
                        mv_strFuncAuthString &= v_strHashValue & "#"
                    End If
                    If Trim(v_strPRID1) <> "" Then
                        If hFuncAuthFilter(v_strPRID1) Is Nothing Then
                            v_strHashValue = v_strPRID1 & "|" & v_strAuthStr & "|N" & "|" & mv_strBrid
                            hFuncAuthFilter.Add(v_strPRID1, v_strHashValue)
                            mv_strFuncAuthString &= v_strHashValue & "#"
                        End If
                    End If
                Else
                    If Not hFuncAuthFilter(v_strPRID) Is Nothing Then
                        hFuncAuthFilter.Remove(v_strPRID)
                        v_strHashValue = v_strPRID & "|" & v_strAuthStr & "|" & v_strLast & "|" & mv_strBrid
                        mv_strFuncAuthString = Replace(mv_strFuncAuthString, v_strHashValue & "#", "")
                    End If

                    If Trim(v_strPRID1) <> "" Then
                        If Not hFuncAuthFilter(v_strPRID1) Is Nothing Then
                            hFuncAuthFilter.Remove(v_strPRID1)
                            v_strHashValue = v_strPRID1 & "|" & v_strAuthStr & "|N" & "|" & mv_strBrid
                            mv_strFuncAuthString = Replace(mv_strFuncAuthString, v_strHashValue & "#", "")
                        End If
                    End If
                End If
                v_treeNodeParent = v_treeNodeParent.ParentItem
            Next

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    ''---------------------------------------''
    ''-- Thủ tục ẩn các control phân quyền --''
    ''---------------------------------------''
    Private Sub DisableAssignment()
        Try
            grbTransAssign.Enabled = False
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    ''---------------------------------------------''
    ''-- Thủ tục hiển thị các control phân quyền --''
    ''---------------------------------------------''
    Private Sub EnableAssignment()
        Try
            grbTransAssign.Enabled = True
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub DisableGroupAccessRight(ByVal v_strAuth As String)
        Dim v_strGD, v_strDM1, v_strDM2, v_strDM3 As String

        Try
            If v_strAuth <> Nothing Then
                v_strGD = Mid(v_strAuth, 1, 1)
                v_strDM1 = Mid(v_strAuth, 2, 1)
                v_strDM2 = Mid(v_strAuth, 3, 1)
                v_strDM3 = Mid(v_strAuth, 4, 1)

                'Enable Access right
                chkGD.Enabled = (v_strGD <> "Y")
                chkDM1.Enabled = (v_strDM1 <> "Y")
                chkDM2.Enabled = (v_strDM2 <> "Y")
                chkDM3.Enabled = (v_strDM3 <> "Y")
            End If
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                          & "Error code: System error!" & vbNewLine _
                                          & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Sub

    Private Sub DisableTellerAccessRight(ByVal v_strAuth As String)
        Dim v_strGD, v_strDM1, v_strDM2, v_strDM3 As String

        Try
            If v_strAuth <> Nothing Then
                v_strGD = Mid(v_strAuth, 1, 1)
                v_strDM1 = Mid(v_strAuth, 2, 1)
                v_strDM2 = Mid(v_strAuth, 3, 1)
                v_strDM3 = Mid(v_strAuth, 4, 1)

                'Enable Access right
                chkGD.Enabled = (v_strGD <> "N")
                chkDM1.Enabled = (v_strDM1 <> "N")
                chkDM2.Enabled = (v_strDM2 <> "N")
                chkDM3.Enabled = (v_strDM3 <> "N")
            End If
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                          & "Error code: System error!" & vbNewLine _
                                          & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Sub

    Private Sub ShowGrpLimitValue(ByVal v_strTLTXCD As String)
        Dim v_strLimit As String
        If chkGD.Checked Then
            If Not hTLGrpAuthFilter(v_strTLTXCD & "0") Is Nothing Then
                If hTLGrpAuthFilter(v_strTLTXCD & "0") <> "" Then
                    v_strLimit = CStr(hTLGrpAuthFilter(v_strTLTXCD & "0")).Split("|")(2)
                    txtGD.Enabled = True
                    txtGD.Text = v_strLimit
                End If
            End If
        End If

        If chkDM1.Checked Then
            If Not hTLGrpAuthFilter(v_strTLTXCD & "1") Is Nothing Then
                If hTLGrpAuthFilter(v_strTLTXCD & "1") <> "" Then
                    v_strLimit = CStr(hTLGrpAuthFilter(v_strTLTXCD & "1")).Split("|")(2)
                    txtDM1.Enabled = True
                    txtDM1.Text = v_strLimit
                End If
            End If
        End If

        If chkDM2.Checked Then
            If Not hTLGrpAuthFilter(v_strTLTXCD & "2") Is Nothing Then
                If hTLGrpAuthFilter(v_strTLTXCD & "2") <> "" Then
                    v_strLimit = CStr(hTLGrpAuthFilter(v_strTLTXCD & "2")).Split("|")(2)
                    txtDM2.Enabled = True
                    txtDM2.Text = v_strLimit
                End If
            End If
        End If

        If chkDM3.Checked Then
            If Not hTLGrpAuthFilter(v_strTLTXCD & "3") Is Nothing Then
                If hTLGrpAuthFilter(v_strTLTXCD & "3") <> "" Then
                    v_strLimit = CStr(hTLGrpAuthFilter(v_strTLTXCD & "3")).Split("|")(2)
                    txtDM3.Enabled = True
                    txtDM3.Text = v_strLimit
                End If
            End If
        End If
    End Sub

    ''-----------------------------------------------------''
    ''-- Thủ tục ẩn các control phân quyền (chế độ View) --''
    ''-----------------------------------------------------''
    Private Sub DisallowChange()
        Try
            chkGD.Enabled = False
            chkDM1.Enabled = False
            chkDM2.Enabled = False
            chkDM3.Enabled = False
        Catch ex As Exception

        End Try
    End Sub
    ''------------------------------''
    ''-- Hiển thị các quyền đã có --''
    ''------------------------------''
    Private Sub ShowAccessRight(ByVal pv_treeNode As Node)
        Dim v_arrMenuKey() As String
        Dim v_strGD, v_strDM1, v_strDM2, v_strDM3 As String
        Dim v_strAuth, v_strCMDID, v_strTLTXCD, v_arrAuth() As String
        Dim v_strGrpAuth As String
        Dim v_strTellerAuth As String
        Try

            If Mid(pv_treeNode.Key, 1, 1) = "Y" Then
                EnableAssignment()
                DisableTextLimit()
                v_arrMenuKey = pv_treeNode.Key.Split("|")
                v_strCMDID = v_arrMenuKey(2)
                v_strTLTXCD = Mid(v_arrMenuKey(3), 3)
                'bangpv
                v_strCMDID = v_strCMDID & mv_strBrid
                'end bangpv
                'CheckUserAccess(v_arrMenuKey(3))
                If Not hFuncAuthFilter(v_strCMDID) Is Nothing Then
                    v_arrAuth = CStr(hFuncAuthFilter(v_strCMDID)).Split("|")
                    v_strAuth = v_arrAuth(1)
                Else
                    v_strAuth = "NNNN"
                End If

                If Not hFuncTellerAuthFilter(v_strCMDID) Is Nothing Then
                    v_arrAuth = CStr(hFuncTellerAuthFilter(v_strCMDID)).Split("|")
                    v_strTellerAuth = v_arrAuth(1)
                Else
                    v_strTellerAuth = "NNNN"
                End If

                If AssignType = "User" Then
                    If Not hFuncGrpAuthFilter(v_strCMDID) Is Nothing Then
                        v_arrAuth = CStr(hFuncGrpAuthFilter(v_strCMDID)).Split("|")
                        v_strGrpAuth = v_arrAuth(1)
                    Else
                        v_strGrpAuth = "NNNN"
                    End If

                    v_strAuth = MergeAuth(v_strAuth, v_strGrpAuth)
                End If

                If v_strAuth <> Nothing Then
                    v_strGD = Mid(v_strAuth, 1, 1)
                    v_strDM1 = Mid(v_strAuth, 2, 1)
                    v_strDM2 = Mid(v_strAuth, 3, 1)
                    v_strDM3 = Mid(v_strAuth, 4, 1)

                    'Display Access right
                    chkGD.Checked = (v_strGD = "Y")
                    chkDM1.Checked = (v_strDM1 = "Y")
                    chkDM2.Checked = (v_strDM2 = "Y")
                    chkDM3.Checked = (v_strDM3 = "Y")
                End If

                If AssignType = "User" Then
                    v_strTellerAuth = MergeAuth1(v_strTellerAuth, v_strGrpAuth)
                    'DisableGroupAccessRight(v_strGrpAuth)
                    ShowGrpLimitValue(v_strTLTXCD)
                End If

                DisableTellerAccessRight(v_strTellerAuth)

                Dim v_strLimit As String
                If chkGD.Checked Then
                    If Not hTLAuthFilter(v_strTLTXCD & "0") Is Nothing Then
                        If hTLAuthFilter(v_strTLTXCD & "0") <> "" Then
                            v_strLimit = CStr(hTLAuthFilter(v_strTLTXCD & "0")).Split("|")(2)
                            txtGD.Enabled = True
                            txtGD.Text = v_strLimit
                        End If
                    End If
                End If

                If chkDM1.Checked Then
                    If Not hTLAuthFilter(v_strTLTXCD & "1") Is Nothing Then
                        If hTLAuthFilter(v_strTLTXCD & "1") <> "" Then
                            v_strLimit = CStr(hTLAuthFilter(v_strTLTXCD & "1")).Split("|")(2)
                            txtDM1.Enabled = True
                            txtDM1.Text = v_strLimit
                        End If
                    End If
                End If

                If chkDM2.Checked Then
                    If Not hTLAuthFilter(v_strTLTXCD & "2") Is Nothing Then
                        If hTLAuthFilter(v_strTLTXCD & "2") <> "" Then
                            v_strLimit = CStr(hTLAuthFilter(v_strTLTXCD & "2")).Split("|")(2)
                            txtDM2.Enabled = True
                            txtDM2.Text = v_strLimit
                        End If
                    End If
                End If

                If chkDM3.Checked Then
                    If Not hTLAuthFilter(v_strTLTXCD & "3") Is Nothing Then
                        If hTLAuthFilter(v_strTLTXCD & "3") <> "" Then
                            v_strLimit = CStr(hTLAuthFilter(v_strTLTXCD & "3")).Split("|")(2)
                            txtDM3.Enabled = True
                            txtDM3.Text = v_strLimit
                        End If
                    End If
                End If


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
        Return v_strAuth
    End Function

    Private Function MergeAuth1(ByVal pv_strTeller As String, ByVal pv_strGroup As String) As String
        Dim v_strAuth As String = ""
        v_strAuth &= IIf(Mid(pv_strGroup, 1, 1) = "Y", "N", Mid(pv_strTeller, 1, 1))
        v_strAuth &= IIf(Mid(pv_strGroup, 2, 1) = "Y", "N", Mid(pv_strTeller, 2, 1))
        v_strAuth &= IIf(Mid(pv_strGroup, 3, 1) = "Y", "N", Mid(pv_strTeller, 3, 1))
        v_strAuth &= IIf(Mid(pv_strGroup, 4, 1) = "Y", "N", Mid(pv_strTeller, 4, 1))
        Return v_strAuth
    End Function
#End Region


#Region "Event"
    Private Sub TreeView_Click(ByVal sender As Object, ByVal e As Xceed.SmartUI.SmartItemClickEventArgs)
        Dim pv_treeNode As Node
        Dim v_strLast As String
        'Update mouse pointer
        Cursor = Cursors.WaitCursor
        Try
            pv_treeNode = e.Item
            v_strLast = pv_treeNode.Key.Split("|")(0)
            If v_strLast = "Y" Then
                If pv_treeNode.Key <> "" Then
                    lbTrans.Text = pv_treeNode.Text
                    mv_node = pv_treeNode
                    ShowAccessRight(pv_treeNode)
                Else
                    lbTrans.Text = String.Empty
                    DisableAssignment()
                End If
            Else
                DisableAssignment()
            End If
        Catch ex As Exception
            Throw ex
        End Try

        'Update mouse pointer
        Cursor = Cursors.Default
    End Sub

    Private Sub TreeView_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        Dim pv_treeNode As Node
        Dim v_strLast As String

        pv_treeNode = CType(sender, Node)
        v_strLast = pv_treeNode.Key.Split("|")(0)

        Select Case e.KeyCode
            Case Keys.Up, Keys.Down
                If v_strLast = "Y" Then
                    If pv_treeNode.Key <> "" Then
                        lbTrans.Text = pv_treeNode.Text
                        mv_node = pv_treeNode
                        ShowAccessRight(pv_treeNode)
                    Else
                        lbTrans.Text = String.Empty
                        DisableAssignment()
                    End If
                Else
                    DisableAssignment()
                End If
        End Select
    End Sub

    Private Sub frmTranAssign_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'mv_BDSDelivery = New BDSChannel.BDSDelivery
        'OnInit()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub txtLimit_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
        Try
            Dim v_strHashKey As String
            Dim v_strHashValue As String
            Dim v_strOldValue As String
            Dim v_strTLType As String
            Dim v_strLimit As String
            Dim v_blnOK As Boolean = False
            If sender Is txtGD Then
                If chkGD.Checked Then
                    v_strLimit = txtGD.Text
                    v_strTLType = 0
                    v_blnOK = True
                End If
            ElseIf sender Is txtDM1 Then
                If chkDM1.Checked Then
                    v_strLimit = txtDM1.Text
                    v_strTLType = 1
                    v_blnOK = True
                End If
            ElseIf sender Is txtDM2 Then
                If chkDM2.Checked Then
                    v_strLimit = txtDM2.Text
                    v_strTLType = 2
                    v_blnOK = True
                End If
            ElseIf sender Is txtDM3 Then
                If chkDM3.Checked Then
                    v_strLimit = txtDM3.Text
                    v_strTLType = 3
                    v_blnOK = True
                End If
            End If

            v_strLimit = Replace(v_strLimit, ",", "").Trim
            If v_blnOK Then
                If CInt(v_strLimit) < 0 Then
                    MsgBox("Giá trị hạn mức phải lớn hơn 0!", MsgBoxStyle.Exclamation, "VSDS - Error")
                Else
                    Dim v_strTLTXID, v_strCMDID As String
                    If Not mv_node Is Nothing Then
                        mv_blnOK = False
                        v_strTLTXID = mv_node.Key.Split("|")(3)
                        v_strCMDID = mv_node.Key.Split("|")(2)

                        v_strCMDID = v_strCMDID & mv_strBrid
                        v_strTLTXID = v_strTLTXID & mv_strBrid
                        If v_strCMDID <> "" Then
                            v_strHashKey = Mid(v_strTLTXID, 3) & v_strTLType

                            Dim v_strTellerLimit As Double
                            If Not hTLTellerAuthFilter(v_strHashKey) Is Nothing Then
                                v_strOldValue = hTLTellerAuthFilter(v_strHashKey)
                                v_strTellerLimit = CDbl(v_strOldValue.Split("|")(2))
                                If CInt(v_strTellerLimit) > 0 Then
                                    If CDbl(v_strLimit) > v_strTellerLimit Then
                                        CType(sender, FlexMaskEditBox).Text = v_strTellerLimit
                                        MsgBox(Replace(mv_ResourceManager.GetString("LimitErr1"), "$", v_strTellerLimit), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                                        CType(sender, FlexMaskEditBox).Focus()
                                        Exit Sub
                                    End If
                                End If
                            End If
                            Dim v_strGrpLimit As Double = -1
                            If AssignType = "User" Then
                                If Not hTLGrpAuthFilter(v_strHashKey) Is Nothing Then
                                    v_strOldValue = hTLGrpAuthFilter(v_strHashKey)
                                    'Dim v_strGrpLimit As Double
                                    v_strGrpLimit = CDbl(v_strOldValue.Split("|")(2))
                                    If CDbl(v_strLimit) > 0 Then
                                        If CDbl(v_strLimit) < v_strGrpLimit Then
                                            CType(sender, FlexMaskEditBox).Text = v_strGrpLimit
                                            MsgBox(Replace(mv_ResourceManager.GetString("LimitErr"), "$", v_strGrpLimit), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                                            CType(sender, FlexMaskEditBox).Focus()
                                            Exit Sub
                                        End If
                                    End If
                                End If
                            End If

                            If CDbl(v_strLimit) <> CDbl(v_strGrpLimit) Then
                                v_strHashValue = Mid(v_strTLTXID, 3) & "|" & v_strTLType & "|" & v_strLimit & "|" & mv_strBrid

                                If Not hTLAuthFilter(v_strHashKey) Is Nothing Then
                                    v_strOldValue = hTLAuthFilter(v_strHashKey)
                                    mv_strTLAuthString = Replace(mv_strTLAuthString, v_strOldValue & "#", "")
                                    mv_strTLAuthString &= v_strHashValue & "#"
                                    hTLAuthFilter.Remove(v_strHashKey)
                                    hTLAuthFilter.Add(v_strHashKey, v_strHashValue)
                                Else
                                    mv_strTLAuthString &= v_strHashValue & "#"
                                    hTLAuthFilter.Add(v_strHashKey, v_strHashValue)
                                End If
                            End If
                        End If
                        mv_blnOK = True
                    End If
                    End If
            End If
            'If CInt(v_strLimit) < 0 Then
            '    MsgBox("Giá trị hạn mức phải lớn hơn 0!", MsgBoxStyle.Exclamation, "VSDS - Error")
            'Else
            '    Dim v_strTLTXID, v_strCMDID As String
            '    If Not mv_node Is Nothing Then
            '        mv_blnOK = False
            '        v_strTLTXID = mv_node.Key.Split("|")(3)
            '        v_strCMDID = mv_node.Key.Split("|")(2)

            '        If v_strCMDID <> "" And v_strLimit <> 0 Then
            '            v_strHashKey = Mid(v_strTLTXID, 3) & v_strTLType

            '            If AssignType = "User" Then
            '                If Not hTLGrpAuthFilter(v_strHashKey) Is Nothing Then
            '                    v_strOldValue = hTLGrpAuthFilter(v_strHashKey)
            '                    Dim v_strGrpLimit As Double
            '                    v_strGrpLimit = CDbl(v_strOldValue.Split("|")(2))
            '                    If CDbl(v_strLimit) < v_strGrpLimit Then
            '                        txtLimit.Text = v_strGrpLimit
            '                        MsgBox(Replace(mv_ResourceManager.GetString("LimitErr"), "$", v_strGrpLimit), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
            '                        txtLimit.Focus()
            '                        Exit Sub
            '                    End If
            '                End If
            '            End If

            '            v_strHashValue = Mid(v_strTLTXID, 3) & "|" & v_strTLType & "|" & v_strLimit

            '            If Not hTLAuthFilter(v_strHashKey) Is Nothing Then
            '                v_strOldValue = hTLAuthFilter(v_strHashKey)
            '                mv_strTLAuthString = Replace(mv_strTLAuthString, v_strOldValue & "#", "")
            '                mv_strTLAuthString &= v_strHashValue & "#"
            '                hTLAuthFilter.Remove(v_strHashKey)
            '                hTLAuthFilter.Add(v_strHashKey, v_strHashValue)
            '            Else
            '                mv_strTLAuthString &= v_strHashValue & "#"
            '                hTLAuthFilter.Add(v_strHashKey, v_strHashValue)
            '            End If
            '        End If

            '        If v_strTLType = 0 Then
            '            Dim v_strOldHashValue As String
            '            v_strHashValue = v_strCMDID & "|YNNNNN|" & mv_node.Key.Split("|")(0)
            '            If v_strLimit = "" Then
            '                If Not hFuncAuthFilter(v_strCMDID) Is Nothing Then
            '                    v_strOldHashValue = hFuncAuthFilter(v_strCMDID)
            '                    hFuncAuthFilter.Remove(v_strCMDID)
            '                    mv_strFuncAuthString = Replace(mv_strFuncAuthString, v_strOldHashValue & "#", "").Trim
            '                End If
            '            Else
            '                'Update new value to hash table
            '                If Not hFuncAuthFilter(v_strCMDID) Is Nothing Then
            '                    'Remove old value before add new value to hash table and auth's string
            '                    v_strOldHashValue = hFuncAuthFilter(v_strCMDID)
            '                    hFuncAuthFilter.Remove(v_strCMDID)
            '                    mv_strFuncAuthString = Replace(mv_strFuncAuthString, v_strOldHashValue & "#", "").Trim
            '                    hFuncAuthFilter.Add(v_strCMDID, v_strHashValue)
            '                    mv_strFuncAuthString &= v_strHashValue & "#"
            '                Else
            '                    'Add new value to hash table and auth's string
            '                    hFuncAuthFilter.Add(v_strCMDID, v_strHashValue)
            '                    mv_strFuncAuthString &= v_strHashValue & "#"
            '                End If
            '            End If
            '            SetParentNodeKey(mv_node)
            '        End If
            '        mv_blnOK = True
            '    End If
            'End If
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                          & "Error code: System error!" & vbNewLine _
                                          & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Sub

    Private Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        If mv_blnOK Then
            OnSave()
        End If
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
        Dim v_strGD, v_strDM1, v_strDM2, v_strDM3 As String
        Dim v_strCMDID, v_strTLTXCD, v_strAuth, v_strTLAuth, v_strHashValue, v_arrAuth(), v_arrTLAuth() As String
        Dim v_strKey As String, v_strLast As String
        Dim v_strOldValue As String
        Try
            mv_blnOK = True
            v_strKey = pv_treeNode.Key.ToString
            v_strCMDID = v_strKey.Split("|")(2)
            v_strTLTXCD = Mid(v_strKey.Split("|")(3), 3)
            v_strLast = v_strKey.Split("|")(0)
            'bangpv
            v_strCMDID = v_strCMDID & mv_strBrid
            v_strTLTXCD = v_strTLTXCD & mv_strBrid

            If Not hFuncAuthFilter(v_strCMDID) Is Nothing Then
                v_arrAuth = CStr(hFuncAuthFilter(v_strCMDID)).Split("|")
                v_strAuth = v_arrAuth(1)
            Else
                v_strAuth = "NNNN"
            End If

            If v_strAuth <> Nothing Then
                'If chkGD's value has changed
                If (sender Is chkGD) Then
                    If chkGD.Checked Then
                        v_strGD = "Y"
                        txtGD.Enabled = True
                        txtGD.Text = 0
                        txtGD.Focus()
                    Else
                        v_strGD = "N"
                        txtGD.Text = ""
                        txtGD.Enabled = False
                        If Not hTLAuthFilter(v_strTLTXCD & "0") Is Nothing Then
                            v_strOldValue = hTLAuthFilter(v_strTLTXCD & "0")
                            mv_strTLAuthString = Replace(mv_strTLAuthString, v_strOldValue & "#", "")
                            hTLAuthFilter.Remove(v_strTLTXCD & "0")
                        End If
                        If Not hFuncAuthFilter(v_strTLTXCD & "0") Is Nothing Then
                            hFuncAuthFilter.Remove(v_strTLTXCD & "0")
                        End If

                    End If
                    v_strAuth = v_strGD & Mid(v_strAuth, 2, 3)
                    'If chkDM1's value has changed
                ElseIf (sender Is chkDM1) Then
                    If chkDM1.Checked Then
                        v_strDM1 = "Y"
                        txtDM1.Enabled = True
                        txtDM1.Text = 0
                        txtDM1.Focus()
                    Else
                        v_strDM1 = "N"
                        txtDM1.Text = ""
                        txtDM1.Enabled = False
                        If Not hTLAuthFilter(v_strTLTXCD & "1") Is Nothing Then
                            v_strOldValue = hTLAuthFilter(v_strTLTXCD & "1")
                            mv_strTLAuthString = Replace(mv_strTLAuthString, v_strOldValue & "#", "")
                            hTLAuthFilter.Remove(v_strTLTXCD & "1")
                        End If
                        If Not hFuncAuthFilter(v_strTLTXCD & "1") Is Nothing Then
                            hFuncAuthFilter.Remove(v_strTLTXCD & "1")
                        End If
                    End If
                    v_strAuth = Mid(v_strAuth, 1, 1) & v_strDM1 & Mid(v_strAuth, 3, 2)

                    'If chkDM2's value has changed
                ElseIf (sender Is chkDM2) Then
                    If chkDM2.Checked Then
                        v_strDM2 = "Y"
                        txtDM2.Enabled = True
                        txtDM2.Text = 0
                        txtDM2.Focus()
                    Else
                        v_strDM2 = "N"
                        txtDM2.Text = ""
                        txtDM2.Enabled = False
                        If Not hTLAuthFilter(v_strTLTXCD & "2") Is Nothing Then
                            v_strOldValue = hTLAuthFilter(v_strTLTXCD & "2")
                            mv_strTLAuthString = Replace(mv_strTLAuthString, v_strOldValue & "#", "")
                            hTLAuthFilter.Remove(v_strTLTXCD & "2")
                        End If
                        If Not hFuncAuthFilter(v_strTLTXCD & "2") Is Nothing Then
                            hFuncAuthFilter.Remove(v_strTLTXCD & "2")
                        End If
                    End If
                    v_strAuth = Mid(v_strAuth, 1, 2) & v_strDM2 & Mid(v_strAuth, 4, 1)
                    'If chkDM3's value has changed
                ElseIf (sender Is chkDM3) Then
                    If chkDM3.Checked Then
                        v_strDM3 = "Y"
                        txtDM3.Enabled = True
                        txtDM3.Text = 0
                        txtDM3.Focus()
                    Else
                        v_strDM3 = "N"

                        txtDM3.Text = ""
                        txtDM3.Enabled = False
                        If Not hTLAuthFilter(v_strTLTXCD & "3") Is Nothing Then
                            v_strOldValue = hTLAuthFilter(v_strTLTXCD & "3")
                            mv_strTLAuthString = Replace(mv_strTLAuthString, v_strOldValue & "#", "")
                            hTLAuthFilter.Remove(v_strTLTXCD & "3")
                        End If
                        If Not hFuncAuthFilter(v_strTLTXCD & "3") Is Nothing Then
                            hFuncAuthFilter.Remove(v_strTLTXCD & "3")
                        End If
                    End If
                    v_strAuth = Mid(v_strAuth, 1, 3) & v_strDM3
                End If

                v_strHashValue = v_strCMDID & "|" & v_strAuth & "|" & v_strLast & "|" & mv_strBrid
                Dim v_strOldHashValue As String
                If v_strAuth = "NNNN" Then
                    If Not hFuncAuthFilter(v_strCMDID) Is Nothing Then
                        v_strOldHashValue = hFuncAuthFilter(v_strCMDID)
                        hFuncAuthFilter.Remove(v_strCMDID)
                        mv_strFuncAuthString = Replace(mv_strFuncAuthString, v_strOldHashValue & "#", "").Trim
                    End If
                Else
                    'Update new value to hash table
                    If Not hFuncAuthFilter(v_strCMDID) Is Nothing Then
                        'Remove old value before add new value to hash table and auth's string
                        v_strOldHashValue = hFuncAuthFilter(v_strCMDID)
                        hFuncAuthFilter.Remove(v_strCMDID)
                        mv_strFuncAuthString = Replace(mv_strFuncAuthString, v_strOldHashValue & "#", "").Trim
                        hFuncAuthFilter.Add(v_strCMDID, v_strHashValue)
                        mv_strFuncAuthString &= v_strHashValue & "#"
                    Else
                        'Add new value to hash table and auth's string
                        hFuncAuthFilter.Add(v_strCMDID, v_strHashValue)
                        mv_strFuncAuthString &= v_strHashValue & "#"
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

    Private Sub CheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkGD.Click, chkDM1.Click, chkDM2.Click, chkDM3.Click
        DoCheckboxChange(sender, stvTrans.SelectedItem)
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