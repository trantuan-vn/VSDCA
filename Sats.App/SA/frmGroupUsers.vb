Imports Sats.CommonLibrary

Public Class frmGroupUsers

    Dim v_result As DialogResult = Windows.Forms.DialogResult.Cancel
    Private mv_oProxy As BDSChannel.BDSDelivery

    Public Property Proxy() As BDSChannel.BDSDelivery
        Get
            Return mv_oProxy
        End Get
        Set(ByVal value As BDSChannel.BDSDelivery)
            mv_oProxy = value
        End Set
    End Property
#Region "Khai báo"
    Private mv_ResourceManager As Resources.ResourceManager
    Private mv_strLanguage As String
    Private mv_strTellerId As String
    Private mv_strBranchId As String
    Private mv_strGroupId As String
    Private mv_strGroupName As String
    Private mv_intExecFlag As Integer

    Const mc_strAdmin = "Admin"

    'Khai báo các bảng băm dùng chứa các thông tin NSD và nhóm
    Public hTlGroup As New Hashtable
    Public hTlidOutGrpFilter As New Hashtable
    Public hTlidInGrpFilter As New Hashtable
    Public hBridInGrpFilter As New Hashtable
    Public hBridOutGrpFilter As New Hashtable
    Private mv_strAuth As String

    Public v_intCountEnter As Integer = 0

    'Private mv_BDSDelivery As BDSChannel.BDSDelivery

#End Region

#Region "Khai báo thuộc tính"

    Public Property BranchId() As String
        Get
            Return mv_strBranchId
        End Get
        Set(ByVal Value As String)
            mv_strBranchId = Value
        End Set
    End Property

    Public Property STRAUTH() As String
        Get
            Return mv_strAuth
        End Get
        Set(ByVal Value As String)
            mv_strAuth = Value
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

    Public Property TellerId() As String
        Get
            Return mv_strTellerId
        End Get
        Set(ByVal Value As String)
            mv_strTellerId = Value
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

    Public Property ExeFlag() As Integer
        Get
            Return mv_intExecFlag
        End Get
        Set(ByVal Value As Integer)
            mv_intExecFlag = Value
        End Set
    End Property

#End Region


#Region "Method"

    Private Sub OnInit()
        mv_ResourceManager = New Resources.ResourceManager(gc_RootNamespace & ".frmGroupUsers_" & UserLanguage, System.Reflection.Assembly.GetExecutingAssembly())
        LoadUserInterface(Me)
        txtGroup.Focus()

        btnAdd.Enabled = (Mid(mv_strAuth, 3, 1) = "Y")
        btnAddAll.Enabled = (Mid(mv_strAuth, 3, 1) = "Y")
        btnRemove.Enabled = (Mid(mv_strAuth, 5, 1) = "Y")
        btnRemoveAll.Enabled = (Mid(mv_strAuth, 5, 1) = "Y")
        btnOk.Enabled = ((Mid(mv_strAuth, 3, 1) = "Y") Or (Mid(mv_strAuth, 4, 1) = "Y"))
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

            'Load caption của form, label caption
            Me.Text = mv_ResourceManager.GetString("frmGroupUsers")
            lbCaption.Text = mv_ResourceManager.GetString("lbCaption") & GroupName
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                           & "Error code: System error!" & vbNewLine _
                           & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Sub

    ''-----------------------------------------------------''
    ''-- Fill dữ liệu cho cboGROUP --''
    ''-- Hanm5
    ''-----------------------------------------------------''
    Private Sub FillCboData()
        Dim v_strCmdInquiryGroup, v_strOutGrpObjMsg As String
        Dim v_strValue As String
        Dim v_strFLDNAME As String
        Try
            v_strCmdInquiryGroup = "SELECT A.GRPID VALUE, A.GRPNAME DISPLAY FROM TLGROUPS A WHERE A.ACTIVE = 'Y'AND A.GRPID NOT IN (SELECT GRPID FROM TLGROUPS WHERE DELETED = 1)"
            v_strOutGrpObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLGROUPS, gc_ActionInquiry, v_strCmdInquiryGroup)

            'Dim v_wsOutGrp As New BDSDelivery.BDSDelivery
            Dim v_lngError As Long = Proxy.Message(v_strOutGrpObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox("Đã có lỗi xẩy ra, hãy liên hệ với quản tri hệ thống để được giúp đỡ", MsgBoxStyle.Critical, gc_ApplicationTitle)
                Exit Sub
            End If

            Dim v_xmlOutGrpDocument As New Xml.XmlDocument
            Dim v_nodeOutGrpList As Xml.XmlNodeList

            v_xmlOutGrpDocument.LoadXml(v_strOutGrpObjMsg)
            v_nodeOutGrpList = v_xmlOutGrpDocument.SelectNodes("/ObjectMessage/ObjData")

            Dim v_arrOutGRID(v_nodeOutGrpList.Count - 1) As String
            Dim v_arrOutGRNAME(v_nodeOutGrpList.Count - 1) As String

            For i As Integer = 0 To v_nodeOutGrpList.Count - 1
                For j As Integer = 0 To v_nodeOutGrpList.Item(i).ChildNodes.Count - 1
                    With v_nodeOutGrpList.Item(i).ChildNodes(j)
                        v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                        v_strValue = .InnerText.ToString
                    End With

                    Select Case Trim(v_strFLDNAME)
                        Case "VALUE"
                            v_arrOutGRID(i) = v_strValue
                        Case "DISPLAY"
                            v_arrOutGRNAME(i) = v_strValue
                    End Select
                Next
                hTlGroup.Add(v_arrOutGRID(i), v_arrOutGRNAME(i))
            Next

            Dim v_icGrpName As ICollection = hTlGroup.Values

            Dim v_arrGrpName As New ArrayList()

            v_arrGrpName.AddRange(v_icGrpName)
            'cboGROUP.DataSource = v_arrGrpName

        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                           & "Error code: System error!" & vbNewLine _
                           & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Sub

    '********************************************************************
    'Mục đích       	: Kiểm tra mã nhóm có tồn tại hay 
    'Tham số        	: 
    'Trả về         	: Int
    'Ngày tạo			: 14/03/2008
    'Người tạo		    : Nguyễn Mạnh Hà
    'Ngày cập nhật  	:
    'Người cập nhật 	:<Người cập nhật cuối cùng>
    '********************************************************************

    Public Function CheckExistGroup(ByVal p_strGroupId As String) As Long
        Dim v_strCmdCheckExistGroup As String
        Dim v_strValue As String
        Dim v_strFLDNAME As String
        Dim v_strGroupExist As String
        Dim v_intValueReturn As Integer

        Try
            v_strCmdCheckExistGroup = "SELECT COUNT(*) VALUE FROM TLGROUPS WHERE GRPID = '" & p_strGroupId & "' AND GRPID NOT IN (SELECT GRPID FROM TLGROUPS WHERE DELETED = 1)"
            v_strGroupExist = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLGROUPS, gc_ActionInquiry, v_strCmdCheckExistGroup)

            'Dim v_wsCheckGrp As New BDSDelivery.BDSDelivery
            Dim v_lngError As Long = Proxy.Message(v_strGroupExist)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox("Đã có lỗi xẩy ra, hãy liên hệ với quản tri hệ thống để được giúp đỡ", MsgBoxStyle.Critical, gc_ApplicationTitle)
                Exit Function
            End If

            Dim v_xmlCheckGrpDocument As New Xml.XmlDocument
            Dim v_nodeCheckGrp As Xml.XmlNodeList

            v_xmlCheckGrpDocument.LoadXml(v_strGroupExist)
            v_nodeCheckGrp = v_xmlCheckGrpDocument.SelectNodes("/ObjectMessage/ObjData")
            Dim v_arrCheckGrpNo(v_nodeCheckGrp.Count - 1) As String
            For i As Integer = 0 To v_nodeCheckGrp.Count - 1
                For j As Integer = 0 To v_nodeCheckGrp.Item(i).ChildNodes.Count - 1
                    With v_nodeCheckGrp.Item(i).ChildNodes(j)
                        v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                        v_strValue = .InnerText.ToString
                    End With
                    If Trim(v_strFLDNAME) = "VALUE" Then
                        v_intValueReturn = v_strValue
                    End If
                Next
            Next
            If v_intValueReturn = "0" Then
                Return ERR_SA_GROUP_NOT_EXIST
            Else
                Return ERR_SA_GROUP_EXIST
            End If
            Return 0
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    ''----------------------------------------------------------------''
    ''-- Thủ tục lấy dữ liệu điền vào các list:                  --''
    ''-- Cụ thể: Lấy thông tin NSD đã trong nhóm và chưa trong nhóm --''
    ''-- rồi điền vào các list tương ứng
    ''-- Hanm5 sửa vào ngày 11/3/2008                         --''
    ''-- thonm sửa vào ngày 22/10/2008                         --''
    ''----------------------------------------------------------------''
    Private Sub FillData()
        Dim v_strUsrOutGrpObjMsg, v_strUserInGroupObjMsg As String
        Dim v_strValue As String
        Dim v_strFLDNAME As String
        Dim v_strCmdInquiryUsrOutGrp, v_strCmdInquiryUserInGroup As String

        Try
            'Check trang thai cua Group
            v_strCmdInquiryUsrOutGrp = "SELECT STATUS FROM TLGROUPS WHERE GRPID='" & GroupId & "'"
            v_strUsrOutGrpObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLPROFILES, gc_ActionInquiry, v_strCmdInquiryUsrOutGrp)

            'Dim v_wsOutGrp As New BDSDelivery.BDSDelivery
            Dim v_lngError As Long = Proxy.Message(v_strUsrOutGrpObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox("Đã có lỗi xẩy ra, hãy liên hệ với quản tri hệ thống để được giúp đỡ", MsgBoxStyle.Critical, gc_ApplicationTitle)
                Exit Sub
            End If

            Dim v_xmlOutGrpDocument As New Xml.XmlDocument
            Dim v_nodeOutGrpList As Xml.XmlNodeList
            Dim v_strStatus As String

            v_xmlOutGrpDocument.LoadXml(v_strUsrOutGrpObjMsg)
            v_nodeOutGrpList = v_xmlOutGrpDocument.SelectNodes("/ObjectMessage/ObjData")
            For i As Integer = 0 To v_nodeOutGrpList.Count - 1
                For j As Integer = 0 To v_nodeOutGrpList.Item(i).ChildNodes.Count - 1
                    With v_nodeOutGrpList.Item(i).ChildNodes(j)
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
                MsgBox("Nhóm này đã ngừng hoạt động, bạn không thể gán NSD cho nhóm này!", MsgBoxStyle.Exclamation, "VSDS")
                txtGroup.Focus()
                Exit Sub
            End If

            'Clear list
            lstUserInGroup.Items.Clear()
            lstUserNoGroup.Items.Clear()
            'Clear Hashtable
            hTlidOutGrpFilter.Clear()
            hBridOutGrpFilter.Clear()
            'Select users' name that are not in group
            v_strCmdInquiryUsrOutGrp = "SELECT TLPROFILES.BRID, TLPROFILES.TLID VALUE, TLPROFILES.TLNAME DISPLAY FROM TLPROFILES" _
                                       & " WHERE TLPROFILES.TLID NOT IN (SELECT TLGRPUSERS.TLID FROM TLGRPUSERS WHERE TLGRPUSERS.GRPID='" & GroupId & "')" _
                                       & " AND TLPROFILES.DELETED=0 AND TLPROFILES.STATUS=0 AND TLPROFILES.TLID<>'" & TellerId & "' ORDER BY TLPROFILES.TLNAME"
            v_strUsrOutGrpObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLPROFILES, gc_ActionInquiry, v_strCmdInquiryUsrOutGrp)

            'Dim v_wsOutGrp As New BDSDelivery.BDSDelivery
            v_lngError = Proxy.Message(v_strUsrOutGrpObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox("Đã có lỗi xẩy ra, hãy liên hệ với quản tri hệ thống để được giúp đỡ", MsgBoxStyle.Critical, gc_ApplicationTitle)
                Exit Sub
            End If

            v_xmlOutGrpDocument.LoadXml(v_strUsrOutGrpObjMsg)
            v_nodeOutGrpList = v_xmlOutGrpDocument.SelectNodes("/ObjectMessage/ObjData")
            Dim v_arrOutGrpTLID(v_nodeOutGrpList.Count - 1) As String
            Dim v_arrOutGrpTLNAME(v_nodeOutGrpList.Count - 1) As String
            Dim v_arrOutGrpBRID(v_nodeOutGrpList.Count - 1) As String
            For i As Integer = 0 To v_nodeOutGrpList.Count - 1
                For j As Integer = 0 To v_nodeOutGrpList.Item(i).ChildNodes.Count - 1
                    With v_nodeOutGrpList.Item(i).ChildNodes(j)
                        v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                        v_strValue = .InnerText.ToString
                    End With
                    Select Case Trim(v_strFLDNAME)
                        Case "VALUE"
                            v_arrOutGrpTLID(i) = v_strValue
                        Case "DISPLAY"
                            v_arrOutGrpTLNAME(i) = v_strValue
                            If v_strValue <> mc_strAdmin Then
                                lstUserNoGroup.Items.Add(v_strValue)
                            End If
                        Case "BRID"
                            v_arrOutGrpBRID(i) = v_strValue
                    End Select
                Next
                hTlidOutGrpFilter.Add(v_arrOutGrpTLNAME(i), v_arrOutGrpTLID(i))
                hBridOutGrpFilter.Add(v_arrOutGrpTLNAME(i), v_arrOutGrpBRID(i))
            Next

            ''''==== Select users' name that are in group ====''''
            'Clear Hashtable
            hTlidInGrpFilter.Clear()
            hBridInGrpFilter.Clear()
            'Select users' name that are in group
            v_strCmdInquiryUserInGroup = "SELECT TLPROFILES.BRID, TLPROFILES.TLID VALUE, TLPROFILES.TLNAME DISPLAY " _
                                    & "FROM TLPROFILES, TLGRPUSERS, TLGROUPS " _
                                    & "WHERE TLPROFILES.TLID = TLGRPUSERS.TLID AND TLGRPUSERS.GRPID = TLGROUPS.GRPID AND TLGROUPS.GRPID = '" & GroupId & "' AND TLPROFILES.STATUS=0 ORDER BY TLPROFILES.TLNAME"
            v_strUserInGroupObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLPROFILES, gc_ActionInquiry, v_strCmdInquiryUserInGroup)

            'Dim v_wsIngrp As New BDSDelivery.BDSDelivery
            v_lngError = Proxy.Message(v_strUserInGroupObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox("Đã có lỗi xẩy ra, hãy liên hệ với quản tri hệ thống để được giúp đỡ", MsgBoxStyle.Critical, gc_ApplicationTitle)
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
                            v_arrInGrpTLID(i) = v_strValue
                        Case "DISPLAY"
                            v_arrInGrpTLNAME(i) = v_strValue
                            lstUserInGroup.Items.Add(v_strValue)
                        Case "BRID"
                            v_arrInGrpBRID(i) = v_strValue
                    End Select
                Next
                hTlidInGrpFilter.Add(v_arrInGrpTLNAME(i), v_arrInGrpTLID(i))
                hBridInGrpFilter.Add(v_arrInGrpTLNAME(i), v_arrInGrpBRID(i))
            Next

        Catch ex As Exception
            Throw ex
        End Try

    End Sub


    ''--------------------------------------------------------''
    ''-- Ghi các thông tin định nghĩa NSD cho nhóm vào CSDL --''
    ''--------------------------------------------------------''
    Private Sub OnSave()

        Dim v_strObjMsg As String
        'Dim v_strTLNAME, v_strValue As String
        Dim v_strClause As String

        Try

            'Get data from list box
            Dim v_strUserInGroup As String = ""
            Dim v_strBdsidInGrp As String = ""

            For i As Integer = 0 To lstUserInGroup.Items.Count - 1
                v_strUserInGroup &= hTlidInGrpFilter(lstUserInGroup.Items.Item(i)) & "|"
                v_strBdsidInGrp &= hBridInGrpFilter(lstUserInGroup.Items.Item(i)) & "|"
            Next

            'Build XML message
            v_strClause = GroupId & "#" & v_strUserInGroup & "#" & v_strBdsidInGrp

            v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsNotLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLGROUPS, gc_ActionAdhoc, , v_strClause, "AddUsersToGroup", gc_AutoIdUsed, )

            'Dim v_ws As New BDSDelivery.BDSDelivery
            Dim v_lngError As Long = Proxy.Message(v_strObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                Me.Cursor = Cursors.Default
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Exit Sub
            End If

            MsgBox(mv_ResourceManager.GetString("EditSuccessful"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
            'Me.OnClose()

        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                           & "Error code: System error!" & vbNewLine _
                           & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Sub

    Private Sub OnClose()
        Me.Close()
    End Sub


    ''----------------------------------------------''
    ''-- Thay đổi listbox khi thêm 1 NSD vào nhóm --''
    ''----------------------------------------------''
    Private Sub MoveOne()
        Try
            lstUserInGroup.Items.Add(lstUserNoGroup.SelectedItem)
            hTlidInGrpFilter.Add(lstUserNoGroup.SelectedItem, hTlidOutGrpFilter(lstUserNoGroup.SelectedItem))
            hBridInGrpFilter.Add(lstUserNoGroup.SelectedItem, hBridOutGrpFilter(lstUserNoGroup.SelectedItem))
            hTlidOutGrpFilter.Remove(lstUserNoGroup.SelectedItem)
            hBridOutGrpFilter.Remove(lstUserNoGroup.SelectedItem)
            lstUserNoGroup.Items.Remove(lstUserNoGroup.SelectedItem)

        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                           & "Error code: System error!" & vbNewLine _
                           & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Sub

    ''---------------------------------------------------''
    ''-- Thay đổi listbox khi thêm tất cả NSD vào nhóm --''
    ''---------------------------------------------------''
    Private Sub MoveAll()
        Try
            'Insert into list box
            lstUserInGroup.Items.AddRange(lstUserNoGroup.Items)
            'Insert values into "In group" hashtable and remove values from "Out group" hashtable
            For i As Integer = 0 To lstUserNoGroup.Items.Count - 1
                hTlidInGrpFilter.Add(lstUserNoGroup.Items.Item(i), hTlidOutGrpFilter(lstUserNoGroup.Items.Item(i)))
                hBridInGrpFilter.Add(lstUserNoGroup.Items.Item(i), hBridOutGrpFilter(lstUserNoGroup.Items.Item(i)))
                hTlidOutGrpFilter.Remove(lstUserNoGroup.Items.Item(i))
                hBridOutGrpFilter.Remove(lstUserNoGroup.Items.Item(i))
            Next
            'Clear list box
            lstUserNoGroup.Items.Clear()
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                           & "Error code: System error!" & vbNewLine _
                           & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Sub

    ''------------------------------------------------''
    ''-- Thay đổi listbox khi bỏ 1 NSD ra khỏi nhóm --''
    ''------------------------------------------------''
    Private Sub ReMoveOne()
        Try
            If CStr(hTlidInGrpFilter(lstUserInGroup.SelectedItem)) <> TellerId Then
                'Insert item to "Out group" list box
                lstUserNoGroup.Items.Add(lstUserInGroup.SelectedItem)
                'Insert/Remove values to/from hashtables
                hTlidOutGrpFilter.Add(lstUserInGroup.SelectedItem, hTlidInGrpFilter(lstUserInGroup.SelectedItem))
                hBridOutGrpFilter.Add(lstUserInGroup.SelectedItem, hBridInGrpFilter(lstUserInGroup.SelectedItem))
                hTlidInGrpFilter.Remove(lstUserInGroup.SelectedItem)
                hBridInGrpFilter.Remove(lstUserInGroup.SelectedItem)
                'Remove item from "In group" list box
                lstUserInGroup.Items.Remove(lstUserInGroup.SelectedItem)
            Else
                MsgBox(mv_ResourceManager.GetString("CurrentUser"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                Exit Sub
            End If
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                           & "Error code: System error!" & vbNewLine _
                           & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Sub

    ''-----------------------------------------------------''
    ''-- Thay đổi listbox khi bỏ tất cả NSD ra khỏi nhóm --''
    ''-----------------------------------------------------''
    Private Sub ReMoveAll()
        Try
            Dim v_strTellerName As String = String.Empty
            For i As Integer = 0 To lstUserInGroup.Items.Count - 1
                If CStr(hTlidInGrpFilter(lstUserInGroup.Items.Item(i))) <> TellerId Then
                    'Insert to "Out group" list box
                    lstUserNoGroup.Items.Add(lstUserInGroup.Items.Item(i))
                    'Insert/Remove values to/from hashtables
                    hTlidOutGrpFilter.Add(lstUserInGroup.Items.Item(i), hTlidInGrpFilter(lstUserInGroup.Items.Item(i)))
                    hBridOutGrpFilter.Add(lstUserInGroup.Items.Item(i), hBridInGrpFilter(lstUserInGroup.Items.Item(i)))
                    hTlidInGrpFilter.Remove(lstUserInGroup.Items.Item(i))
                    hBridInGrpFilter.Remove(lstUserInGroup.Items.Item(i))
                    'Remove item from "In group" list box
                    'lstUserInGroup.Items.Remove(lstUserInGroup.Items.Item(i))
                Else
                    v_strTellerName = CStr(lstUserInGroup.Items.Item(i))
                End If
            Next
            'Clear list box
            lstUserInGroup.Items.Clear()
            If v_strTellerName <> String.Empty Then
                lstUserInGroup.Items.Add(v_strTellerName)
                MsgBox(mv_ResourceManager.GetString("CurrentUser"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
            End If

        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                           & "Error code: System error!" & vbNewLine _
                           & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Sub


#End Region

#Region "Sự kiện Form"

    Private Sub frmGroupUsers_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        v_result = Windows.Forms.DialogResult.Cancel
    End Sub

    Private Sub frmGroupUsers_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'mv_BDSDelivery = New BDSChannel.BDSDelivery
        Me.OnInit()
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        If GroupId <> "" Then
            Me.OnSave()
        Else
            MsgBox(mv_ResourceManager.GetString("EmptyGroup"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
        End If

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.OnClose()
    End Sub

    Private Sub btnAddAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddAll.Click
        Me.MoveAll()
    End Sub

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If Not lstUserNoGroup.SelectedItems.Count < 1 Then
            Me.MoveOne()
        Else
            MsgBox(mv_ResourceManager.GetString("NothingSelected"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
        End If

    End Sub

    Private Sub btnRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemove.Click
        If Not lstUserInGroup.SelectedItems.Count < 1 Then
            Me.ReMoveOne()
        Else
            MsgBox(mv_ResourceManager.GetString("NothingSelected"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
        End If
    End Sub

    Private Sub btnRemoveAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveAll.Click
        Me.ReMoveAll()
    End Sub
    Private Sub lstUserNoGroup_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstUserNoGroup.DoubleClick
        Me.MoveOne()
    End Sub

    Private Sub lstUserInGroup_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstUserInGroup.DoubleClick
        Me.ReMoveOne()
    End Sub

#End Region

    

    Private Sub txtGroup_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtGroup.KeyUp
        Try
            Select Case e.KeyCode
                Case Keys.Enter
                    If v_result <> Windows.Forms.DialogResult.OK Then
                        If txtGroup.Text <> "" Then
                            If CheckExistGroup(txtGroup.Text) = ERR_SA_GROUP_NOT_EXIST Then
                                btnOk.Focus()
                                v_result = MsgBox(mv_ResourceManager.GetString("GroupNotExist"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                                Application.DoEvents()
                            Else
                                GroupId = txtGroup.Text
                                FillData()
                            End If
                        Else
                            MsgBox(mv_ResourceManager.GetString("EmptyGroup"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                        End If
                    End If
                Case Keys.F5
                    Dim frm As New Sats.AppCore.frmLookUp(m_BusLayer.AppLanguage)
                    Dim v_intPos As Integer ', ctl As Control
                    frm.SQLCMD = "SELECT grpid valuecd, grpid VALUE, grpname display, grpname en_display, description description FROM tlgroups WHERE status = 0 AND deleted=0 ORDER BY VALUE"
                    frm.TellerID = TellerId
                    frm.Proxy = Proxy
                    frm.ShowDialog()
                    If Not frm.RETURNDATA Is Nothing Then
                        v_intPos = InStr(frm.RETURNDATA, vbTab)
                        If v_intPos > 0 Then
                            Me.txtGroup.Text = Mid(frm.RETURNDATA, 1, v_intPos - 1)
                            GroupId = Me.txtGroup.Text
                            Me.txtGroup.Select()
                        End If
                        frm.Dispose()
                        FillData()
                    End If
            End Select
            Application.DoEvents()
            txtGroup.Focus()
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                           & "Error code: System error!" & vbNewLine _
                           & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Sub

    Private Sub txtGroup_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGroup.TextChanged
        v_result = Windows.Forms.DialogResult.Cancel
    End Sub
End Class