Imports Sats.CommonLibrary

Public Class frmCAAssign
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
    Private mv_strAssignType As String
    Private mv_strMIInUser As String
    Private mv_strMIOutUser As String
    Private mv_strMIInGrp As String

    Const mc_strAdmin = "Admin"

    Private hTlidInGrpFilter As New Hashtable
    Private hTellerFilter As New Hashtable

    Public v_intCountEnter As Integer = 0

    'Private mv_BDSDelivery As BDSChannel.BDSDelivery

#End Region

#Region "Khai báo thuộc tính"

    Public Property AssignType() As String
        Get
            Return mv_strAssignType
        End Get
        Set(ByVal Value As String)
            mv_strAssignType = Value
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

    Public Sub OnInit()
        Try
            mv_ResourceManager = New Resources.ResourceManager(gc_RootNamespace & ".frmMIAssign_" & UserLanguage, System.Reflection.Assembly.GetExecutingAssembly())
            LoadUserInterface(Me)
            'mv_BDSDelivery = New BDSChannel.BDSDelivery

            FillTellerData()

            If AssignType = "Group" Then
                FillData()
            Else
                FillUserData()
            End If

            'Check trang thai
            Dim v_strSQL, v_strObjMsg As String

            v_strSQL = "SELECT STATUS FROM TLGROUPS WHERE GRPID='" & GroupId & "'"

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
            Me.Text = mv_ResourceManager.GetString("frmCAAssign")
            Dim v_strCaption As String
            If AssignType = "User" Then
                v_strCaption = Replace(mv_ResourceManager.GetString("lbCaption3"), "$", "NSD")
            Else
                v_strCaption = Replace(mv_ResourceManager.GetString("lbCaption3"), "$", " nhóm")
            End If
            lbCaption3.Text = v_strCaption & GroupName
            If ExeFlag = ExecuteFlag.View Then
                btnOk.Enabled = False
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    ''----------------------------------------------------------------''
    ''-- Thủ tục lấy dữ liệu điền vào các list:                  --''
    ''-- Cụ thể: Lấy thông tin TVLK đã trong nhóm và chưa trong nhóm --''
    ''-- rồi điền vào các list tương ứng
    ''-- Create by thonm                      --''
    ''----------------------------------------------------------------''
    Private Sub FillData()
        Dim v_strUsrOutGrpObjMsg, v_strUserInGroupObjMsg As String
        Dim v_strValue As String
        Dim v_strFLDNAME As String
        Dim v_strCmdInquiryUsrOutGrp, v_strCmdInquiryUserInGroup As String
        Dim v_strMICode, v_strDisplay As String
        Try
            'Clear list
            lstUserInGroup.Items.Clear()
            lstUserNoGroup.Items.Clear()

            mv_strMIOutUser = ""
            mv_strMIInUser = ""
            'Lay TVLK chua dc phan quyen cho nhom hoac NSD
            v_strCmdInquiryUsrOutGrp = "SELECT    VALUE,  display" _
                                            & " FROM ( " _
                                            & " select tltxcd value, tltxcd||'-'||txdesc display from tltx  " _
                                            & " where deleted =0 and issignca =1" _
                                            & " union all " _
                                            & " select rptid value, rptid || '-'||RPTTITLE display from rpreports " _
                                            & " where deleted =0 and issignca =1) " _
                                            & " WHERE VALUE NOT IN (SELECT TLTXCD FROM tlcaauth" _
                                            & " WHERE AUTHID = '" & GroupId & "' AND authtype = 'G')" _
                                            & " ORDER BY value"

            v_strUsrOutGrpObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLPROFILES, gc_ActionInquiry, v_strCmdInquiryUsrOutGrp)

            'Dim v_wsOutGrp As New BDSDelivery.BDSDelivery
            Dim v_lngError As Long = Proxy.Message(v_strUsrOutGrpObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Exit Sub
            End If

            Dim v_xmlOutGrpDocument As New Xml.XmlDocument
            Dim v_nodeOutGrpList As Xml.XmlNodeList

            v_xmlOutGrpDocument.LoadXml(v_strUsrOutGrpObjMsg)
            v_nodeOutGrpList = v_xmlOutGrpDocument.SelectNodes("/ObjectMessage/ObjData")

            For i As Integer = 0 To v_nodeOutGrpList.Count - 1
                For j As Integer = 0 To v_nodeOutGrpList.Item(i).ChildNodes.Count - 1
                    With v_nodeOutGrpList.Item(i).ChildNodes(j)
                        v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                        v_strValue = .InnerText.ToString
                    End With
                    Select Case Trim(v_strFLDNAME)
                        Case "VALUE"
                            v_strMICode = v_strValue
                        Case "DISPLAY"
                            v_strDisplay = v_strValue
                    End Select
                Next

                If Not hTellerFilter(v_strMICode) Is Nothing Then
                    mv_strMIOutUser &= v_strMICode & "|"
                    lstUserNoGroup.Items.Add(v_strDisplay)
                End If
            Next

            ''''==== Select users' name that are in group ====''''
            'Lay TVLK da phan quyen cho nhom hoac NSD
            v_strCmdInquiryUserInGroup = "SELECT    VALUE,  display" _
                                            & " FROM ( " _
                                            & " select tltxcd value, tltxcd||'-'||txdesc display from tltx  " _
                                            & " where deleted =0 and issignca =1" _
                                            & " union all " _
                                            & " select rptid value, rptid || '-'||RPTTITLE display from rpreports " _
                                            & " where deleted =0 and issignca =1) " _
                                            & " WHERE VALUE IN (SELECT TLTXCD FROM tlcaauth" _
                                            & " WHERE AUTHID = '" & GroupId & "' AND authtype = 'G')" _
                                            & "  ORDER BY value"
            v_strUserInGroupObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLPROFILES, gc_ActionInquiry, v_strCmdInquiryUserInGroup)

            'Dim v_wsIngrp As New BDSDelivery.BDSDelivery
            v_lngError = Proxy.Message(v_strUserInGroupObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
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
                            v_strMICode = v_strValue
                        Case "DISPLAY"
                            v_strDisplay = v_strValue
                    End Select
                Next

                If Not hTellerFilter(v_strMICode) Is Nothing Then
                    mv_strMIInUser &= v_strMICode & "|"
                    lstUserInGroup.Items.Add(v_strDisplay)
                End If
            Next

        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                          & "Error code: System error!" & vbNewLine _
                          & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try

    End Sub

    ''----------------------------------------------------------------''
    ''-- Thủ tục lấy dữ liệu điền vào các list:                  --''
    ''-- Cụ thể: Lấy thông tin TVLK đã trong nhóm và chưa trong nhóm --''
    ''-- rồi điền vào các list tương ứng
    ''-- Create by thonm                      --''
    ''----------------------------------------------------------------''
    Private Sub FillUserData()
        Dim v_strUsrOutGrpObjMsg, v_strUserInGroupObjMsg As String
        Dim v_strValue As String
        Dim v_strFLDNAME As String
        Dim v_strCmdInquiryUsrOutGrp, v_strCmdInquiryUserInGroup As String

        Try
            Dim v_strMICode, v_strDisplay As String
            'Clear list
            lstUserInGroup.Items.Clear()
            lstUserNoGroup.Items.Clear()

            mv_strMIOutUser = ""
            mv_strMIInUser = ""
            'Lay TVLK chua dc phan quyen cho nhom hoac NSD
            v_strCmdInquiryUsrOutGrp = "SELECT micode VALUE, micode || ' - ' || NAME display" _
                                            & " FROM rgmi WHERE micode NOT IN(" _
                                            & " SELECT  DISTINCT m.micode" _
                                            & " FROM rgmi m, tlmemauth a" _
                                            & " WHERE ((a.AUTHID = '" & GroupId & "' AND a.authtype = 'U')" _
                                            & " OR (a.AUTHID IN (SELECT grpid FROM tlgrpusers" _
                                            & " WHERE tlid = '" & GroupId & "') AND authtype = 'G'))" _
                                            & " And m.deleted = 0 And m.micode = a.micode) AND deleted=0" _
                                            & " ORDER BY VALUE"

            v_strUsrOutGrpObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLPROFILES, gc_ActionInquiry, v_strCmdInquiryUsrOutGrp)

            'Dim v_wsOutGrp As New BDSDelivery.BDSDelivery
            Dim v_lngError As Long = Proxy.Message(v_strUsrOutGrpObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Exit Sub
            End If

            Dim v_xmlOutGrpDocument As New Xml.XmlDocument
            Dim v_nodeOutGrpList As Xml.XmlNodeList

            v_xmlOutGrpDocument.LoadXml(v_strUsrOutGrpObjMsg)
            v_nodeOutGrpList = v_xmlOutGrpDocument.SelectNodes("/ObjectMessage/ObjData")

            For i As Integer = 0 To v_nodeOutGrpList.Count - 1
                For j As Integer = 0 To v_nodeOutGrpList.Item(i).ChildNodes.Count - 1
                    With v_nodeOutGrpList.Item(i).ChildNodes(j)
                        v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                        v_strValue = .InnerText.ToString
                    End With
                    Select Case Trim(v_strFLDNAME)
                        Case "VALUE"
                            v_strMICode = v_strValue
                        Case "DISPLAY"
                            v_strDisplay = v_strValue
                    End Select
                Next

                If Not hTellerFilter(v_strMICode) Is Nothing Then
                    mv_strMIOutUser &= v_strMICode & "|"
                    lstUserNoGroup.Items.Add(v_strDisplay)
                End If
            Next

            ''''==== Select users' name that are in group ====''''
            'Lay TVLK da phan quyen cho nhom hoac NSD
            v_strCmdInquiryUserInGroup = "SELECT  DISTINCT m.micode VALUE, m.micode || ' - ' || NAME display, a.authtype" _
                                        & " FROM rgmi m, tlmemauth a" _
                                        & " WHERE ((a.AUTHID = '" & GroupId & "' AND a.authtype = 'U')" _
                                        & " OR (a.AUTHID IN (SELECT grpid FROM tlgrpusers" _
                                        & " WHERE tlid = '" & GroupId & "') AND a.authtype = 'G'))" _
                                        & " And m.deleted = 0 And m.micode = a.micode" _
                                        & " ORDER BY M.micode"

            v_strUserInGroupObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLPROFILES, gc_ActionInquiry, v_strCmdInquiryUserInGroup)

            'Dim v_wsIngrp As New BDSDelivery.BDSDelivery
            v_lngError = Proxy.Message(v_strUserInGroupObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Exit Sub
            End If

            Dim v_xmlInGrpDocument As New Xml.XmlDocument
            Dim v_nodeInGrpList As Xml.XmlNodeList
            Dim v_strAuthType As String

            v_xmlInGrpDocument.LoadXml(v_strUserInGroupObjMsg)
            v_nodeInGrpList = v_xmlInGrpDocument.SelectNodes("/ObjectMessage/ObjData")
            Dim v_arrInGrpTLID(v_nodeInGrpList.Count - 1) As String
            Dim v_arrInGrpTLNAME(v_nodeInGrpList.Count - 1) As String
            Dim v_arrInGrpBRID(v_nodeInGrpList.Count - 1) As String

            hTlidInGrpFilter.Clear()
            Dim v_strtmpMICode As String = ""
            For i As Integer = 0 To v_nodeInGrpList.Count - 1
                For j As Integer = 0 To v_nodeInGrpList.Item(i).ChildNodes.Count - 1
                    With v_nodeInGrpList.Item(i).ChildNodes(j)
                        v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                        v_strValue = .InnerText.ToString
                    End With
                    Select Case Trim(v_strFLDNAME)
                        Case "VALUE"
                            v_strMICode = v_strValue
                        Case "DISPLAY"
                            v_strDisplay = v_strValue
                        Case "AUTHTYPE"
                            v_strAuthType = v_strValue
                    End Select
                Next

                If Not hTellerFilter(v_strMICode) Is Nothing Then
                    If v_strMICode <> v_strtmpMICode Then
                        lstUserInGroup.Items.Add(v_strDisplay)
                    End If
                    If v_strAuthType = "U" Then
                        mv_strMIInUser &= v_strMICode & "|"
                    Else
                        hTlidInGrpFilter.Add(v_strMICode, v_strDisplay)
                    End If
                End If
                v_strtmpMICode = v_strMICode
            Next

        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                          & "Error code: System error!" & vbNewLine _
                          & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try

    End Sub

    Private Sub FillTellerData()
        Dim v_strUsrOutGrpObjMsg As String
        Dim v_strValue As String
        Dim v_strFLDNAME As String

        Try
            'Lay TVLK chua dc phan quyen cho nhom hoac NSD
            'v_strCmdInquiryUsrOutGrp = "SELECT micode" _
            '                                & " FROM rgmi WHERE micode NOT IN(" _
            '                                & " SELECT  DISTINCT m.micode" _
            '                                & " FROM rgmi m, tlmemauth a" _
            '                                & " WHERE ((a.AUTHID = '" & GroupId & "' AND a.authtype = 'U')" _
            '                                & " OR (a.AUTHID IN (SELECT grpid FROM tlgrpusers" _
            '                                & " WHERE tlid = '" & GroupId & "') AND authtype = 'G'))" _
            '                                & " And m.deleted = 0 And m.micode = a.micode) AND deleted=0" _
            '                                & " ORDER BY micode"

            v_strUsrOutGrpObjMsg = BuildXMLObjMsg(Now.Date, m_BusLayer.CurrentTellerProfile.BranchId, Now.Date, _
                    m_BusLayer.CurrentTellerProfile.TellerId, CommonLibrary.gc_IsLocalMsg, CommonLibrary.gc_MsgTypeObj, _
                    OBJNAME_SY_AUTHENTICATION, CommonLibrary.gc_ActionInquiry, , m_BusLayer.CurrentTellerProfile.TellerId, "GetTellerCAData")

            'Dim v_wsOutGrp As New BDSDelivery.BDSDelivery
            Dim v_lngError As Long = Proxy.Message(v_strUsrOutGrpObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Exit Sub
            End If

            Dim v_xmlOutGrpDocument As New Xml.XmlDocument
            Dim v_nodeOutGrpList As Xml.XmlNodeList

            v_xmlOutGrpDocument.LoadXml(v_strUsrOutGrpObjMsg)
            v_nodeOutGrpList = v_xmlOutGrpDocument.SelectNodes("/ObjectMessage/ObjData")

            For i As Integer = 0 To v_nodeOutGrpList.Count - 1
                For j As Integer = 0 To v_nodeOutGrpList.Item(i).ChildNodes.Count - 1
                    With v_nodeOutGrpList.Item(i).ChildNodes(j)
                        v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                        v_strValue = .InnerText.ToString
                    End With
                    Select Case Trim(v_strFLDNAME)
                        Case "VALUE"
                            hTellerFilter.Add(v_strValue, v_strValue)
                    End Select
                Next
            Next
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                          & "Error code: System error!" & vbNewLine _
                          & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
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
            Cursor = Cursors.WaitCursor
            'Build XML message
            v_strClause = GroupId & "#" & mv_strMIInUser

            If AssignType = "Group" Then
                v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsNotLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLGROUPS, gc_ActionAdhoc, , v_strClause, "CAAssign")
            Else
                v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsNotLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLPROFILES, gc_ActionAdhoc, , v_strClause, "CAAssign")
            End If

            'Dim v_ws As New BDSDelivery.BDSDelivery
            Dim v_lngError As Long = Proxy.Message(v_strObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                Me.Cursor = Cursors.Default
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Exit Sub
            End If

            MsgBox(mv_ResourceManager.GetString("EditSuccessful"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
            Me.Close()
            Cursor = Cursors.Default
        Catch ex As Exception
            Cursor = Cursors.Default
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                                             & "Error code: System error!" & vbNewLine _
                                                             & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(mv_ResourceManager.GetString("SavingFailed"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
        End Try
    End Sub

    ''----------------------------------------------''
    ''-- Thay đổi listbox khi thêm 1 NSD vào nhóm --''
    ''----------------------------------------------''
    Private Sub MoveOne()
        Try
            Dim v_strSelect As String
            v_strSelect = lstUserNoGroup.SelectedItem.ToString.Split("-")(0).Trim

            mv_strMIOutUser = Replace(mv_strMIOutUser, v_strSelect & "|", "")
            mv_strMIInUser &= v_strSelect & "|"

            lstUserInGroup.Items.Add(lstUserNoGroup.SelectedItem)
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
                'hTlidInGrpFilter.Add(lstUserNoGroup.Items.Item(i), hTlidOutGrpFilter(lstUserNoGroup.Items.Item(i)))
                'hTlidOutGrpFilter.Remove(lstUserNoGroup.Items.Item(i))
                Dim v_strSelect As String
                v_strSelect = lstUserNoGroup.Items(i).ToString.Split("-")(0).Trim
                mv_strMIOutUser = Replace(mv_strMIOutUser, v_strSelect & "|", "")
                mv_strMIInUser &= v_strSelect & "|"
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
    ''-- Thay đổi listbox khi bỏ 1 TVLK ra khỏi nhóm hoac NSD --''
    ''------------------------------------------------''
    Private Sub ReMoveOne()
        Try
            'Insert item to "Out group" list box
            Dim v_strSelect As String
            v_strSelect = lstUserInGroup.SelectedItem.ToString.Split("-")(0).Trim

            'Kiem tra TVLK nay co phai da dc phan quyen cho Nhom hay ko?
            If Not hTlidInGrpFilter(v_strSelect) Is Nothing Then
                MsgBox(mv_ResourceManager.GetString("RemoveErr") & v_strSelect, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                Exit Sub
            End If
            mv_strMIInUser = Replace(mv_strMIInUser, v_strSelect & "|", "")
            mv_strMIOutUser &= v_strSelect & "|"

            lstUserNoGroup.Items.Add(lstUserInGroup.SelectedItem)
            lstUserInGroup.Items.Remove(lstUserInGroup.SelectedItem)

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
            Dim v_strMsg As String = ""
            For i As Integer = 0 To lstUserInGroup.Items.Count - 1
                'Insert to "Out group" list box
                Dim v_strSelect As String
                v_strSelect = lstUserInGroup.Items(i).ToString.Split("-")(0).Trim

                'Kiem tra TVLK nay co phai da dc phan quyen cho Nhom hay ko?
                If Not hTlidInGrpFilter(v_strSelect) Is Nothing Then
                    v_strMsg &= v_strSelect & ","
                Else
                    mv_strMIInUser = Replace(mv_strMIInUser, v_strSelect & "|", "")
                    mv_strMIOutUser &= v_strSelect & "|"
                    lstUserNoGroup.Items.Add(lstUserInGroup.Items.Item(i))
                    'lstUserInGroup.Items.Remove(lstUserInGroup.Items(i))
                End If
            Next

            lstUserInGroup.Items.Clear()
            If AssignType = "User" Then
                Dim v_arrTemp() As String
                v_arrTemp = v_strMsg.Split(",")
                For i As Integer = 0 To v_arrTemp.Count - 2
                    lstUserInGroup.Items.Add(hTlidInGrpFilter(v_arrTemp(i)))
                Next

                If v_strMsg <> "" Then
                    v_strMsg = Mid(v_strMsg, 1, Len(v_strMsg) - 1)
                    MsgBox(mv_ResourceManager.GetString("RemoveErr") & v_strMsg, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                End If
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

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        If GroupId <> "" Then
            Me.OnSave()
        Else
            MsgBox(mv_ResourceManager.GetString("EmptyGroup"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
        End If

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnAddAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddAll.Click
        Me.MoveAll()
    End Sub

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If Not lstUserNoGroup.SelectedItems.Count < 1 Then
            Me.MoveOne()
        Else
            MsgBox(mv_ResourceManager.GetString("NothingSelected3"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
        End If

    End Sub

    Private Sub btnRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemove.Click
        If Not lstUserInGroup.SelectedItems.Count < 1 Then
            Me.ReMoveOne()
        Else
            MsgBox(mv_ResourceManager.GetString("NothingSelected3"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
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

End Class