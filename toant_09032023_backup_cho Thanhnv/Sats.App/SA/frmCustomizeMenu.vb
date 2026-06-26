Imports System
Imports Sats.CommonLibrary
Imports System.Windows.Forms
Imports Xceed.SmartUI.Controls.TreeView

Public Class frmCustomizeMenu

    Private mv_SelectNode As Node
    Private mv_strKey, mv_strDisplay As String
    Private mv_intKey As Integer
    Private mv_oProxy As BDSChannel.BDSDelivery

    Public Property Proxy() As BDSChannel.BDSDelivery
        Get
            Return mv_oProxy
        End Get
        Set(ByVal value As BDSChannel.BDSDelivery)
            mv_oProxy = value
        End Set
    End Property

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
                Proxy.Message(v_strObjMsg)

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

    Public Sub GetTreeFavView(Optional ByVal pv_strTLID As String = "0000")
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
            v_strObjMsg = CommonLibrary.BuildXMLObjMsg(Now.Date, m_BusLayer.CurrentTellerProfile.BranchId, Now.Date, _
                    m_BusLayer.CurrentTellerProfile.TellerId, CommonLibrary.gc_IsLocalMsg, CommonLibrary.gc_MsgTypeObj, _
                    OBJNAME_SY_AUTHENTICATION, CommonLibrary.gc_ActionInquiry, , pv_strTLID, "GetTreeFavMenu")
            Proxy.Message(v_strObjMsg)

            XmlDocument.LoadXml(v_strObjMsg)
            v_nodeList = XmlDocument.SelectNodes("/ObjectMessage/ObjData")

            'stvMenu.Visible = False
            Dim v_hParentNode As New Hashtable
            v_hParentNode.Add("Lev_0", stvFavmenu.Items(0))
            mv_intKey = 0
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

                'v_nodeParent = v_hParentNode("Lev_" & CInt(v_strLev) - 1)
                'v_tempNode = AddTreeNode1(v_nodeParent, "Key_" & v_intCount, v_strCmdName, v_strLast, v_intIndex)

                'If v_hParentNode("Lev_" & v_strLev) Is Nothing Then
                '    v_hParentNode.Add("Lev_" & v_strLev, v_tempNode)
                'Else
                '    v_hParentNode("Lev_" & v_strLev) = v_tempNode
                'End If

                v_nodeParent = v_hParentNode("Lev_" & CInt(v_strLev) - 1)

                If v_strLast = "Y" Then
                    v_strMenuKey = v_strLast & v_strCMDID & "|" & v_strMenuType & "|" & v_strModCode & "|" & v_strObjName & "|" & v_strAuthCode & "|" & v_strAuthString & "|" & v_strMenuCode & "|" & v_intCount

                    AddTreeNode1(v_nodeParent, v_strMenuKey, v_strCmdName, v_strLast, v_intIndex)
                Else
                    v_tempNode = AddTreeNode1(v_nodeParent, "Key_" & v_intCount, v_strCmdName, v_strLast, v_intIndex)

                    If v_hParentNode("Lev_" & v_strLev) Is Nothing Then
                        v_hParentNode.Add("Lev_" & v_strLev, v_tempNode)
                    Else
                        v_hParentNode("Lev_" & v_strLev) = v_tempNode
                    End If
                End If


            Next v_intCount
            'stvMenu.Visible = True
            mv_intKey = v_intCount + 1

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
            AddHandler v_node.Click, AddressOf Me.Treeview_Click
            AddHandler v_node.KeyUp, AddressOf Me.Treeview_KeyUp
        Else
            v_node.Expanded = False
        End If

        pv_nodeParent.Items.Add(v_node)
        Return v_node
    End Function

    Private Function AddTreeNode1(ByRef pv_nodeParent As Node, _
                                    ByVal pv_strKey As String, _
                                    ByVal pv_strName As String, _
                                    ByVal pv_strLast As String, _
                                    Optional ByVal pv_intImageIdx As Integer = 0) As Node
        'Create new node
        Dim v_node As New Node(pv_strName, pv_intImageIdx)

        v_node.Key = pv_strKey
        v_node.Text = pv_strName
        v_node.ToolTipText = pv_strName

        AddHandler v_node.Click, AddressOf Treeview_Click1

        pv_nodeParent.Items.Add(v_node)
        Return v_node
    End Function

#End Region

    Private Sub Treeview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim v_nodeKey As String
        Dim v_arrMenuKey() As String
        Dim v_strY As String
        v_nodeKey = CType(sender, Node).Key.ToString
        v_strY = Mid(v_nodeKey, 1, 1)
        If v_strY = "Y" Then
            mv_strKey = v_nodeKey
            mv_strDisplay = CType(sender, Node).Text
            'ExecuteMenuFunction(v_arrMenuKey)
        Else
            CType(sender, Node).Expanded = Not CType(sender, Node).Expanded
            If CType(sender, Node).Expanded Then
                CType(sender, Node).ImageIndex = 1
            Else
                CType(sender, Node).ImageIndex = 0
            End If
        End If
    End Sub

    Private Sub Treeview_Click1(ByVal sender As System.Object, ByVal e As System.EventArgs)
        mv_SelectNode = stvFavmenu.SelectedItem
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
                'ExecuteMenuFunction(v_arrMenuKey)
            Else
                CType(sender, Node).Expanded = Not CType(sender, Node).Expanded
            End If
        End If
    End Sub

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        Dim v_strName As String
        Dim v_node As Node

        If mv_SelectNode Is Nothing Then
            MsgBox("Bạn phải chọn một mục", MsgBoxStyle.Information, Me.Text)
        Else
            If mv_SelectNode.ImageIndex = 3 Then
                MsgBox("Bạn phải chọn một mục cha", MsgBoxStyle.Information, Me.Text)
            Else
                v_strName = InputBox("Nhập tên mục", gc_ApplicationTitle)
                If v_strName.Trim <> "" Then
                    v_node = New Node(v_strName)

                    v_node.Key = "Key_" & mv_intKey
                    v_node.ImageIndex = 1
                    mv_intKey += 1
                    AddHandler v_node.Click, AddressOf Treeview_Click1

                    mv_SelectNode.Items.Add(v_node)
                    mv_SelectNode.Expanded = True
                End If
            End If
        End If
    End Sub

    Private Sub cmdDel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDel.Click
        Dim v_node As Node
        If Not mv_SelectNode Is Nothing Then
            If MsgBox("Bạn có chắc chắn xóa mục này hay không?", MsgBoxStyle.Question + MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                v_node = mv_SelectNode.ParentItem
                v_node.Items.Remove(mv_SelectNode)
            End If
        End If
    End Sub

    Private Sub btnLoadDefault_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoadDefault.Click
        stvFavmenu.Items(0).Items.Clear()
        GetTreeFavView()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        OnSave()
    End Sub

    Private Sub OnSave()
        Try
            Dim v_node As Node
            Dim v_intKey As Integer = 1
            Dim v_intLev As Integer = 1
            Dim v_ds As New DataSet()
            Dim v_oDataColumn As DataColumn

            v_ds.Tables.Add("FAVMENU")
            For i As Integer = 0 To 11
                v_oDataColumn = New DataColumn("COL_" & i)
                v_oDataColumn.ColumnName = "COL_" & i
                v_oDataColumn.DataType = GetType(System.String)
                v_ds.Tables(0).Columns.Add(v_oDataColumn)
            Next

            For v_int As Integer = 0 To stvFavmenu.Items(0).Items.Count - 1
                GetDataNode(stvFavmenu.Items(0).Items(v_int), v_intKey, v_intLev, v_ds)
                v_intKey += 1
            Next

            Dim v_strObjMsg As String
            v_strObjMsg = BuildXMLObjMsg(, , , m_BusLayer.CurrentTellerProfile.TellerId, gc_IsNotLocalMsg, gc_MsgTypeObj, OBJNAME_SY_AUTHENTICATION, gc_ActionInquiry, , , "FAVMENU")
            BuildXMLObjData(v_ds, v_strObjMsg)
            'Dim v_ws As New BDSChannel.BDSDelivery
            Dim v_lngErr As Long = ERR_SYSTEM_OK

            v_lngErr = Proxy.Message(v_strObjMsg)

            If v_lngErr <> ERR_SYSTEM_OK Then
                MsgBox(IIf(m_BusLayer.AppLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
            Else
                MsgBox("Chấp nhận không thành công!", MsgBoxStyle.Critical, Me.Text)
            End If
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                           & "Error code: System error!" & vbNewLine _
                           & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(m_BusLayer.AppLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Sub

    Private Sub GetDataNode(ByVal pv_node As Node, ByRef pv_intOrd As Integer, ByVal pv_intLev As Integer, ByRef pv_ds As DataSet)
        Dim v_intOrd As Integer = pv_intOrd
        
        Dim v_oDataRow As DataRow
        Dim v_strKey As String = pv_node.Key
        Dim v_strDisplay As String = pv_node.Text
        Dim v_strCMDID, v_strPRID, v_strMenuType, v_strLast, v_strAUTHCODE, v_strImgIndex, v_strObjName, v_strModeCode, v_strCMDCODE As String

        v_strImgIndex = pv_node.ImageIndex

        If Mid(v_strKey, 1, 3).ToUpper = "KEY" Then
            v_strCMDID = "K_" & v_intOrd
            v_strPRID = ""
            v_strLast = "N"
            v_strAUTHCODE = "NNNNNN"
            v_strObjName = ""
            v_strModeCode = ""
            v_strCMDCODE = ""
            v_strMenuType = "P"
        Else
            Dim v_arrMenuKey() As String
            v_arrMenuKey = Mid(v_strKey, 2).Split("|")

            v_strLast = "Y"
            v_strCMDID = v_arrMenuKey(0)
            v_strMenuType = v_arrMenuKey(1)
            v_strModeCode = v_arrMenuKey(2)
            v_strObjName = v_arrMenuKey(3)
            v_strAUTHCODE = v_arrMenuKey(4)
            v_strPRID = ""
        End If

        v_oDataRow = pv_ds.Tables(0).NewRow()

        v_oDataRow(0) = v_strCMDID
        v_oDataRow(1) = v_strPRID
        v_oDataRow(2) = pv_intLev
        v_oDataRow(3) = v_strImgIndex
        v_oDataRow(4) = v_strDisplay
        v_oDataRow(5) = v_strAUTHCODE
        v_oDataRow(6) = pv_intOrd

        v_oDataRow(7) = v_strObjName
        v_oDataRow(8) = v_strModeCode
        v_oDataRow(9) = v_strCMDCODE
        v_oDataRow(10) = v_strLast
        v_oDataRow(11) = v_strMenuType

        pv_ds.Tables(0).Rows.Add(v_oDataRow)

        If pv_node.Items.Count > 0 Then
            For v_int = 0 To pv_node.Items.Count - 1
                pv_intOrd += 1
                GetDataNode(pv_node.Items(v_int), pv_intOrd, pv_intLev + 1, pv_ds)
            Next
        End If
    End Sub

    Private Sub AddNodeToFav()
        Dim v_node As Node
        If Not mv_SelectNode Is Nothing Then
            If Mid(mv_SelectNode.Key, 1, 3).ToUpper <> "KEY" Then
                MsgBox("Không thể thêm vào mục này!", MsgBoxStyle.Information, Me.Text)
            Else
                If mv_strKey <> "" Then
                    v_node = New Node(mv_strDisplay)
                    v_node.Key = mv_strKey & "|" & mv_intKey
                    v_node.ImageIndex = 3
                    AddHandler v_node.Click, AddressOf Treeview_Click1
                    mv_SelectNode.Items.Add(v_node)
                    mv_strKey = ""
                    mv_strDisplay = ""
                    mv_intKey += 1
                    mv_SelectNode.Expanded = True
                End If
            End If
        Else
            MsgBox("Bạn phải chọn một mục", MsgBoxStyle.Information, Me.Text)
        End If
    End Sub

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        AddNodeToFav()
    End Sub

    Private Sub btnReLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReLoad.Click
        stvFavmenu.Items(0).Items.Clear()
        GetTreeFavView(m_BusLayer.CurrentTellerProfile.TellerId)
    End Sub

    Private Sub cmdEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdEdit.Click
        Dim v_strName As String

        If mv_SelectNode Is Nothing Then
            MsgBox("Bạn phải chọn một mục", MsgBoxStyle.Information, Me.Text)
        Else
            v_strName = InputBox("Nhập tên mục", gc_ApplicationTitle, mv_SelectNode.Text)
            If v_strName.Trim <> "" Then
                mv_SelectNode.Text = v_strName
            End If
        End If
    End Sub

    Private Sub frmCustomizeMenu_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        AddHandler Node2.Click, AddressOf Treeview_Click1
    End Sub
End Class