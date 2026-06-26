Imports Xceed.SmartUI.Controls.TreeView
Imports Sats.CommonLibrary
Imports System
Imports System.Windows.Forms


Public Class frmFunction
    Private mv_intStart As Integer
    Private mv_strReturnValue As String
    Private mv_ResourceManager As Resources.ResourceManager
    Private mv_strLanguage As String
    Private mv_strTellerID As String
    Private mv_oProxy As BDSChannel.BDSDelivery

    Public Property Proxy() As BDSChannel.BDSDelivery
        Get
            Return mv_oProxy
        End Get
        Set(ByVal value As BDSChannel.BDSDelivery)
            mv_oProxy = value
        End Set
    End Property

    Public Property UserLanguage() As String
        Get
            Return mv_strLanguage
        End Get
        Set(ByVal value As String)
            mv_strLanguage = value
        End Set
    End Property

    Public Property ReturnValue() As String
        Get
            Return mv_strReturnValue
        End Get
        Set(ByVal value As String)
            mv_strReturnValue = value
        End Set
    End Property

    Public Property TellerID() As String
        Get
            Return mv_strTellerID
        End Get
        Set(ByVal value As String)
            mv_strTellerID = value
        End Set
    End Property

    Private Sub OnInit()
        mv_ResourceManager = New Resources.ResourceManager("Sats.AppCore.frmFunction_" & UserLanguage, System.Reflection.Assembly.GetExecutingAssembly())
        LoadUserInterface(Me)

        lstValue.BeginUpdate()

        lstValue.Items.Add("Số lượng Chứng khoán(X)")
        lstValue.Items.Add("Số lượng Quyền(Y)")
        lstValue.Items.Add("Loại cổ phần(Z)")
        lstValue.Items.Add("Loại Chứng khoán(T)")

        lstValue.EndUpdate()
        GetTreeFunc()
        txtEditer.Text = mv_strReturnValue
    End Sub

    Private Sub GetTreeFunc()
        Try
            'Dim v_oBds As New BDSChannel.BDSDelivery
            Dim v_strSQL, v_strObjMsg As String
            Dim v_nodeList As Xml.XmlNodeList
            Dim XmlDocument As New Xml.XmlDocument
            Dim v_strFLDNAME, v_strValue As String
            Dim v_strFUNCNAME, v_strFUNCDESC, v_strLev, v_strMenuKey, v_strPreLev As String
            Dim v_blnFirst As Boolean = True

            v_strSQL = "SELECT * FROM CAFUNC WHERE STATUS=0 AND DELETED=0 ORDER BY FGROUP, FORDER"
            v_strObjMsg = BuildXMLObjMsg(, , , TellerID, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_CA_START, gc_ActionInquiry, v_strSQL)

            Dim v_lngError As Long = proxy.Message(v_strObjMsg)

            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Exit Sub
            End If
            XmlDocument.LoadXml(v_strObjMsg)
            v_nodeList = XmlDocument.SelectNodes("/ObjectMessage/ObjData")

            'stvMenu.Visible = False
            For v_intCount = 0 To v_nodeList.Count - 1
                For v_int = 0 To v_nodeList.Item(v_intCount).ChildNodes.Count - 1
                    With v_nodeList.Item(v_intCount).ChildNodes(v_int)
                        v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                        v_strValue = .InnerText.ToString

                        Select Case Trim(v_strFLDNAME)
                            Case "FUNCNAME"
                                v_strFUNCNAME = Trim(v_strValue)
                            Case "FUNCDESC"
                                v_strFUNCDESC = Trim(v_strValue)
                            Case "LEV"
                                v_strLev = Trim(v_strValue)

                        End Select
                    End With
                Next v_int

                Dim v_node As New Node
                Dim v_nodeParent As Node

                If v_blnFirst Then
                    v_node = New Node(v_strFUNCNAME)
                    v_strMenuKey = v_strLev & "|" & v_strFUNCNAME & "|" & v_strFUNCDESC

                    v_node.Key = v_strMenuKey
                    stvFunc.Items(0).Items.Add(v_node)
                    v_nodeParent = stvFunc.Items(0).Items(v_strMenuKey)
                    v_blnFirst = False
                Else
                    v_strMenuKey = v_strLev & "|" & v_strFUNCNAME & "|" & v_strFUNCDESC
                    If v_strLev = "1" Then
                        v_nodeParent = AddTreeNode(v_nodeParent.ParentItem, v_strMenuKey, v_strFUNCNAME, v_strFUNCDESC)
                    Else
                        AddTreeNode(v_nodeParent, v_strMenuKey, v_strFUNCNAME, v_strFUNCDESC)
                    End If
                    AddHandler v_nodeParent.DoubleClick, AddressOf Me.Tree_DoubleClick
                End If
            Next v_intCount

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Function AddTreeNode(ByRef pv_nodeParent As Node, _
                                        ByVal pv_strKey As String, _
                                        ByVal pv_strName As String, _
                                        ByVal pv_strDesc As String) As Node
        Try
            'Create new node
            Dim v_node As New Node(pv_strName)

            v_node.Key = pv_strKey
            v_node.Text = pv_strName
            v_node.ToolTipText = pv_strDesc
            AddHandler v_node.DoubleClick, AddressOf Me.Tree_DoubleClick
            pv_nodeParent.Items.Add(v_node)
            Return v_node
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Sub frmFunction_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        OnInit()
    End Sub

    Private Sub Tree_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim v_strKey As String
        Dim v_strLev, v_strDesc As String
        Dim v_strSelText As String
        v_strKey = CType(sender, Node).Key

        v_strLev = v_strKey.Split("|")(0)
        v_strDesc = v_strKey.Split("|")(2)

        If v_strLev = "2" Then
            v_strSelText = txtEditer.SelectedText
            If v_strSelText <> "" Then
                txtEditer.Text = txtEditer.Text.Replace(txtEditer.SelectedText, v_strDesc)
            Else
                txtEditer.Text = txtEditer.Text.Insert(mv_intStart, v_strDesc)
            End If
            txtEditer.Focus()
            txtEditer.SelectionStart = mv_intStart + Len(v_strDesc)
        End If
    End Sub

    Private Sub txtEditer_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtEditer.LostFocus
        mv_intStart = txtEditer.SelectionStart
    End Sub
    
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If CheckFunction(txtEditer.Text) <> ERR_SYSTEM_OK Then
            MsgBox(mv_ResourceManager.GetString("CheckErr"), MsgBoxStyle.Exclamation, gc_ApplicationTitle)
            Exit Sub
        End If
        mv_strReturnValue = Trim(txtEditer.Text).ToUpper
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        mv_strReturnValue = ""
        Me.Close()
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
            Me.Text = mv_ResourceManager.GetString("frmFunction")
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub btnCheck_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCheck.Click
        If CheckFunction(txtEditer.Text) <> ERR_SYSTEM_OK Then
            MsgBox(mv_ResourceManager.GetString("CheckErr"), MsgBoxStyle.Exclamation, gc_ApplicationTitle)
        Else
            MsgBox(mv_ResourceManager.GetString("CheckOK"), MsgBoxStyle.Exclamation, gc_ApplicationTitle)
        End If
    End Sub

    Private Function CheckFunction(ByVal pv_strValue As String) As Long
        Try
            'Dim v_oBds As New BDSChannel.BDSDelivery
            Dim v_strSQL, v_strObjMsg As String
            Dim v_lngError As Long

            v_strSQL = "SELECT " & pv_strValue & " CT FROM" _
                         & " (SELECT 2 X, 3 Y, 2 Z, 1 T FROM DUAL)"
            v_strObjMsg = BuildXMLObjMsg(, , , TellerID, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_CA_START, gc_ActionInquiry, v_strSQL)

            v_lngError = Proxy.Message(v_strObjMsg)

            Return v_lngError

        Catch ex As Exception
            Return 1
        End Try
    End Function

    Private Sub lstValue_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstValue.DoubleClick
        Dim v_strSelText As String
        Dim v_strValue As String
        If CType(sender, ListBox).SelectedIndex = 0 Then
            v_strValue = "X"
        ElseIf CType(sender, ListBox).SelectedIndex = 1 Then
            v_strValue = "Y"
        ElseIf CType(sender, ListBox).SelectedIndex = 2 Then
            v_strValue = "Z"
        Else
            v_strValue = "T"
        End If

        v_strSelText = txtEditer.SelectedText
        If v_strSelText <> "" Then
            txtEditer.Text = txtEditer.Text.Replace(txtEditer.SelectedText, v_strValue)
        Else
            txtEditer.Text = txtEditer.Text.Insert(mv_intStart, v_strValue)
        End If
        txtEditer.Focus()
        txtEditer.SelectionStart = mv_intStart + Len(v_strValue)
    End Sub
   
End Class