Imports Sats.CommonLibrary
Imports System.IO
Imports Sats.AppCore

Public Class frmSysVarSetting
    Private mv_ResourceManager As Resources.ResourceManager
    'Private mv_BDSDelivery As New BDSChannel.BDSDelivery

    Private Const VSD_BRANCH As String = "0001"
    Private Const HCM_BRANCH As String = "0002"
    Private Const OTC_BRANCH As String = "0003"
    Private Const BOND_BRANCH As String = "0004"
    Private Const BONDUSD_BRANCH As String = "0005"
    Private Const BONDTP_BRANCH As String = "0006"
    Private Const DLCN_BRANCH As String = "0007"

    'Khai bao thuoc tinh Form
    Private mv_strBranchId As String
    Private mv_strLanguage As String
    Private mv_strTellerId As String
    Private mv_strBrid As String
    Private mv_intMonth As Integer = Now.Month
    Private mv_intYear As Integer = Now.Year

    'Khai bao cac bien tham so he thong
    'Phan he SA
    'Phan he CS
    'Phan he SF
    Dim v_strMaxAnnualFund As String
    Dim v_strInterestRateLevel2 As String
    Dim v_strDateLimitLevel2 As String
    Dim v_strAnnualFundRate As String
    Dim v_strInterestRateLevel1 As String
    'Phan he RA

    'Cac bien khac
    Dim mv_htbVarList As Hashtable

    Dim mv_htbListDateVSD As New Hashtable
    Dim mv_htbListDateHCM As New Hashtable
    Dim mv_htbListDateOTC As New Hashtable
    Dim mv_htbListDateBOND As New Hashtable
    Dim mv_htbListDateBONDUSD As New Hashtable
    Dim mv_htbListDateBONDTP As New Hashtable
    'Thanglv9-add them 0007
    Dim mv_htbListDateDLCN As New Hashtable
    'end

    Dim mv_lstListDateVSD As New List(Of String)
    Dim mv_lstListDateHCM As New List(Of String)
    Dim mv_lstListDateOTC As New List(Of String)
    Dim mv_lstListDateBOND As New List(Of String)
    Dim mv_lstListDateBONDUSD As New List(Of String)
    Dim mv_lstListDateBONDTP As New List(Of String)
    'Thanglv9-add them 0007
    Dim mv_lstListDateDLCN As New List(Of String)
    'end
    Private mv_oProxy As BDSChannel.BDSDelivery

    Public Property Proxy() As BDSChannel.BDSDelivery
        Get
            Return mv_oProxy
        End Get
        Set(ByVal value As BDSChannel.BDSDelivery)
            mv_oProxy = value
        End Set
    End Property

#Region "Khai bao cac thuoc tinh"
    Public Property BRID() As String
        Get
            Return mv_strBrid
        End Get
        Set(ByVal Value As String)
            mv_strBrid = Value
        End Set
    End Property

    Public Property Year() As Integer
        Get
            Return mv_intYear
        End Get
        Set(ByVal Value As Integer)
            mv_intYear = Value
        End Set
    End Property

    Public Property Month() As Integer
        Get
            Return mv_intMonth
        End Get
        Set(ByVal Value As Integer)
            mv_intMonth = Value
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
#End Region
#Region "Method"
    Private Sub OnInit()
        mv_ResourceManager = New Resources.ResourceManager(gc_RootNamespace & ".frmSysVarSetting_" & UserLanguage, System.Reflection.Assembly.GetExecutingAssembly())
        'System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("vi-VN")
        LoadUserInterface(Me)
        LoadDataScreen()
        GetListDate()
        FillDataCombobox()
        tpRA.Dispose()
        'System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US")
    End Sub


    Private Sub LoadUserInterface(ByRef pv_ctrl As Windows.Forms.Control)
        Dim v_ctrl As Windows.Forms.Control
        Try
            For Each v_ctrl In pv_ctrl.Controls
                If TypeOf (v_ctrl) Is Panel Then
                    LoadUserInterface(v_ctrl)
                ElseIf TypeOf (v_ctrl) Is GroupBox Then
                    CType(v_ctrl, GroupBox).Text = mv_ResourceManager.GetString(v_ctrl.Tag)
                    LoadUserInterface(v_ctrl)
                ElseIf TypeOf (v_ctrl) Is Label Then
                    CType(v_ctrl, Label).Text = mv_ResourceManager.GetString(v_ctrl.Tag)
                ElseIf TypeOf (v_ctrl) Is Button Then
                    CType(v_ctrl, Button).Text = mv_ResourceManager.GetString(v_ctrl.Tag)
                ElseIf TypeOf (v_ctrl) Is TabControl Then
                    For Each v_ctrlTmp As Control In CType(v_ctrl, TabControl).TabPages
                        CType(v_ctrlTmp, TabPage).Text = mv_ResourceManager.GetString(v_ctrlTmp.Tag)
                        LoadUserInterface(v_ctrlTmp)
                    Next
                ElseIf TypeOf (v_ctrl) Is TabPage Then
                    v_ctrl.BackColor = System.Drawing.SystemColors.InactiveCaptionText
                    CType(v_ctrl, TabPage).Text = mv_ResourceManager.GetString(v_ctrl.Tag)
                    LoadUserInterface(v_ctrl)
                End If
            Next

            'Load caption của form, label caption
            Me.Text = mv_ResourceManager.GetString("frmSysVarSetting")
            lbCaption.Text = mv_ResourceManager.GetString("lbCaption")
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub LoadDataScreen()
        Try
            Dim v_xmlDocument As New Xml.XmlDocument
            Dim v_nodeList As Xml.XmlNodeList = Nothing

            Dim v_strValue As String = "", v_strFldName As String = ""
            Dim v_strGrName As String = "", v_strVarName As String = "", v_strVarValue As String = ""

            Dim v_strObjMsg As String
            Dim v_strSql As String = ""

            v_strSql = "SELECT GRNAME, VARNAME, VARVALUE FROM SYSVAR WHERE GRNAME in ('SA','SYSTEM','SF','RA') AND STATUS = 0 AND DELETED = 0"

            v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLGROUPS, gc_ActionInquiry, v_strSql)
            Proxy.Message(v_strObjMsg)

            v_xmlDocument.LoadXml(v_strObjMsg)
            v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")


            For i As Integer = 0 To v_nodeList.Count - 1
                For j As Integer = 0 To v_nodeList.Item(i).ChildNodes.Count - 1
                    With v_nodeList.Item(i).ChildNodes(j)
                        v_strFldName = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                        v_strValue = .InnerText.ToString()
                    End With
                    Select Case Trim(v_strFLDNAME)
                        Case "GRNAME"
                            v_strGrName = Trim(v_strValue)
                        Case "VARNAME"
                            v_strVarName = Trim(v_strValue)
                        Case "VARVALUE"
                            v_strVarValue = Trim(v_strValue)
                    End Select
                Next
                Select Case Trim(v_strGrName)
                    Case "SA"
                        SetControlValue(tpSA, v_strVarName, v_strVarValue)
                    Case "SYSTEM"
                        SetControlValue(tpSYSTEM, v_strVarName, v_strVarValue)
                    Case "SF"
                        SetControlValue(tpSF, v_strVarName, v_strVarValue)
                    Case "RA"
                        SetControlValue(tpRA, v_strVarName, v_strVarValue)
                End Select
            Next
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub SetControlValue(ByRef pv_ctrl As Control, ByVal pv_strCtrllTag As String, ByVal pv_strCtrlValue As String)
        Try
            For Each v_ctrl In pv_ctrl.Controls
                If TypeOf (v_ctrl) Is TextBox Then
                    If v_ctrl.Tag = pv_strCtrllTag Then
                        CType(v_ctrl, TextBox).Text = pv_strCtrlValue
                    End If
                ElseIf TypeOf (v_ctrl) Is Sats.AppCore.ComboBoxEx Then
                    If v_ctrl.Tag = pv_strCtrllTag Then
                        CType(v_ctrl, Sats.AppCore.ComboBoxEx).SelectedValue = pv_strCtrlValue
                    End If
                ElseIf TypeOf (v_ctrl) Is GroupBox Then
                    SetControlValue(v_ctrl, pv_strCtrllTag, pv_strCtrlValue)
                End If
            Next
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Function GetDataUpdate(ByRef pv_ctrl As Control) As String
        Try
            Dim v_strSqlUpdate As String = ""
            For Each v_ctrl In pv_ctrl.Controls
                If TypeOf (v_ctrl) Is TextBox Then
                    v_strSqlUpdate = v_strSqlUpdate & Trim(CType(v_ctrl, TextBox).Tag) & "= '" & Trim(CType(v_ctrl, TextBox).Text) & "'|"
                ElseIf TypeOf (v_ctrl) Is Sats.AppCore.ComboBoxEx Then
                    v_strSqlUpdate = v_strSqlUpdate & Trim(CType(v_ctrl, Sats.AppCore.ComboBoxEx).Tag) & " = '" & Trim(CType(v_ctrl, Sats.AppCore.ComboBoxEx).SelectedValue) & "'|"
                ElseIf TypeOf (v_ctrl) Is GroupBox Then
                    v_strSqlUpdate = v_strSqlUpdate & GetDataUpdate(v_ctrl)
                End If
            Next
            Return v_strSqlUpdate
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Sub FillDataCombobox()
        Try
            Dim v_strObjMsg As String
            Dim strRow As String = "SELECT BRID VALUE, BRID || ' - ' || BRNAME DISPLAY FROM BRGRP WHERE STATUS = 0 AND DELETED = 0 AND BRID IN ('0001','0002','0003', '0004', '0005','0006','0007') ORDER BY BRID"
            v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLGROUPS, gc_ActionInquiry, strRow)
            Proxy.Message(v_strObjMsg)

            cboBranchId.Clears()

            FillComboEx(v_strObjMsg, cboBranchId)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub GetListDate()
        Try
            Dim v_xmlDocument As New Xml.XmlDocument
            Dim v_nodeList As Xml.XmlNodeList = Nothing
            Dim v_strValue As String = "", v_strFLDNAME As String = ""
            Dim v_strAutoId As String = "", v_strSbDate As String = "", v_strBrId As String

            Dim v_strObjMsg As String
            Dim strRow As String = "SELECT AUTOID, TO_CHAR(SBDATE, 'dd/MM/yyyy') SBDATE, BRID FROM SYSCLDR WHERE STATUS = 0 AND DELETED = 0"
            v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLGROUPS, gc_ActionInquiry, strRow)
            Proxy.Message(v_strObjMsg)

            v_xmlDocument.LoadXml(v_strObjMsg)
            v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")

            mv_htbListDateVSD.Clear()
            mv_htbListDateHCM.Clear()
            mv_htbListDateOTC.Clear()
            mv_htbListDateBOND.Clear()
            mv_htbListDateBONDUSD.Clear()
            mv_htbListDateBONDTP.Clear()
            'Thanglv9-add them thi truong 0007
            mv_htbListDateDLCN.Clear()
            'End

            mv_lstListDateVSD.Clear()
            mv_lstListDateHCM.Clear()
            mv_lstListDateOTC.Clear()
            mv_lstListDateBOND.Clear()
            mv_lstListDateBONDUSD.Clear()
            mv_lstListDateBONDTP.Clear()
            'Thanglv9-add them thi truong 0007
            mv_lstListDateDLCN.Clear()
            'End

            For i As Integer = 0 To v_nodeList.Count - 1
                For j As Integer = 0 To v_nodeList.Item(i).ChildNodes.Count - 1
                    With v_nodeList.Item(i).ChildNodes(j)
                        v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                        v_strValue = .InnerText.ToString()
                    End With
                    Select Case Trim(v_strFLDNAME)
                        Case "AUTOID"
                            v_strAutoId = Trim(v_strValue)
                        Case "SBDATE"
                            v_strSbDate = Trim(v_strValue)
                        Case "BRID"
                            v_strBrId = Trim(v_strValue)
                    End Select
                Next
                Select Case v_strBrId
                    Case VSD_BRANCH
                        mv_htbListDateVSD.Add(v_strSbDate, v_strAutoId)
                        mv_lstListDateVSD.Add(v_strSbDate)
                    Case HCM_BRANCH
                        mv_htbListDateHCM.Add(v_strSbDate, v_strAutoId)
                        mv_lstListDateHCM.Add(v_strSbDate)
                    Case OTC_BRANCH
                        mv_htbListDateOTC.Add(v_strSbDate, v_strAutoId)
                        mv_lstListDateOTC.Add(v_strSbDate)
                    Case BOND_BRANCH
                        mv_htbListDateBOND.Add(v_strSbDate, v_strAutoId)
                        mv_lstListDateBOND.Add(v_strSbDate)
                    Case BONDUSD_BRANCH
                        mv_htbListDateBONDUSD.Add(v_strSbDate, v_strAutoId)
                        mv_lstListDateBONDUSD.Add(v_strSbDate)
                    Case BONDTP_BRANCH
                        mv_htbListDateBONDTP.Add(v_strSbDate, v_strAutoId)
                        mv_lstListDateBONDTP.Add(v_strSbDate)
                        'Thanglv9-add them thi truong 0007
                    Case DLCN_BRANCH
                        mv_htbListDateDLCN.Add(v_strSbDate, v_strAutoId)
                        mv_lstListDateDLCN.Add(v_strSbDate)
                        'end
                End Select
            Next
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub FillListDate(ByRef pv_lstListDate As List(Of String), ByRef pv_intMonth As Integer, ByRef pv_intYear As Integer)
        Try
            lstListDate.Items.Clear()
            For i As Integer = 0 To pv_lstListDate.Count - 1
                If CDate(pv_lstListDate.Item(i)).Month = pv_intMonth And CDate(pv_lstListDate.Item(i)).Year = pv_intYear Then
                    lstListDate.Items.Add(pv_lstListDate.Item(i))
                End If
            Next
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Function GetStrUpdateSA(ByRef pv_htbListDate As Hashtable, ByRef pv_lstListDate As List(Of String), ByVal pv_strBrId As String) As String
        Try
            Dim v_strDateListInsert As String = ""
            Dim v_strDateListDelete As String = ""

            For i As Integer = 0 To pv_htbListDate.Count - 1
                If Not pv_lstListDate.Contains(pv_htbListDate.Keys(i)) Then
                    v_strDateListDelete &= pv_htbListDate.Keys(i).ToString & "|"
                End If
            Next
            For j As Integer = 0 To pv_lstListDate.Count - 1
                If Not pv_htbListDate.ContainsKey(pv_lstListDate.Item(j)) Then
                    v_strDateListInsert &= pv_lstListDate.Item(j).ToString & "|"
                End If
            Next
            pv_htbListDate.Clear()
            For j As Integer = 0 To pv_lstListDate.Count - 1
                pv_htbListDate.Add(pv_lstListDate.Item(j).ToString, "")
            Next
            'Build XML message

            If v_strDateListDelete <> "" Then
                v_strDateListDelete = Mid(v_strDateListDelete, 1, Len(v_strDateListDelete) - 1)
            End If

            If v_strDateListInsert <> "" Then
                v_strDateListInsert = Mid(v_strDateListInsert, 1, Len(v_strDateListInsert) - 1)
            End If

            Return v_strDateListDelete & "#" & v_strDateListInsert & "#" & pv_strBrId
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Private Sub UpdateSA()

        Dim v_strObjMsg As String
        Dim v_strClause As String = ""
        Try
            v_strClause &= GetStrUpdateSA(mv_htbListDateVSD, mv_lstListDateVSD, VSD_BRANCH) & "$"
            v_strClause &= GetStrUpdateSA(mv_htbListDateHCM, mv_lstListDateHCM, HCM_BRANCH) & "$"
            v_strClause &= GetStrUpdateSA(mv_htbListDateOTC, mv_lstListDateOTC, OTC_BRANCH) & "$"
            v_strClause &= GetStrUpdateSA(mv_htbListDateBOND, mv_lstListDateBOND, BOND_BRANCH) & "$"
            v_strClause &= GetStrUpdateSA(mv_htbListDateBONDTP, mv_lstListDateBOND, BONDTP_BRANCH) & "$"
            v_strClause &= GetStrUpdateSA(mv_htbListDateBONDUSD, mv_lstListDateBONDUSD, BONDUSD_BRANCH) & "$"
            'Thanglv9-Add them thi truong 0007
            v_strClause &= GetStrUpdateSA(mv_htbListDateDLCN, mv_lstListDateDLCN, DLCN_BRANCH)
            'end

            v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsNotLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLGROUPS, gc_ActionAdhoc, , v_strClause, "UpdateSysCalendar", gc_AutoIdUsed, )
            Proxy.Message(v_strObjMsg)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub InitAllYear(ByVal pv_strYear As String, ByVal pv_bridSelected As String)
        Try
            Dim v_strObjMsg As String
            Dim v_strClause As String = Trim(pv_strYear)
            Dim v_strBrid As String = Trim(pv_bridSelected)

            v_strObjMsg = BuildXMLObjMsg(, v_strBrid, , TellerId, gc_IsNotLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLGROUPS, gc_ActionAdhoc, , v_strClause, "InitAllSysCalendar", gc_AutoIdUsed, )

            'Dim v_ws As New BDSDelivery.BDSDelivery
            Proxy.Message(v_strObjMsg)

            GetListDate()
            FillDataCombobox()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub UpdateSysVarValue()
        Try
            Dim v_strVarListUpdate As String = ""
            v_strVarListUpdate = GetDataUpdate(tpSYSTEM) & "|"
            v_strVarListUpdate &= GetDataUpdate(tpSF) & "|"
            v_strVarListUpdate &= GetDataUpdate(tpRA)

            Dim v_strObjMsg As String

            v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsNotLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLGROUPS, gc_ActionAdhoc, , v_strVarListUpdate, "UpdateSysVarValue", gc_AutoIdUsed, )

            'Dim v_ws As New BDSDelivery.BDSDelivery
            Proxy.Message(v_strObjMsg)

            'MsgBox(mv_ResourceManager.GetString("EditSuccessful"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
            'Me.OnClose()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub RemoveAll()
        Try
            Select Case BRID
                Case VSD_BRANCH
                    mv_lstListDateVSD.Clear()
                    lstListDate.Items.Clear()
                Case HCM_BRANCH
                    mv_lstListDateHCM.Clear()
                    lstListDate.Items.Clear()
                Case OTC_BRANCH
                    mv_lstListDateOTC.Clear()
                    lstListDate.Items.Clear()
                Case BOND_BRANCH
                    mv_lstListDateBOND.Clear()
                    lstListDate.Items.Clear()
                Case BONDUSD_BRANCH
                    mv_lstListDateBONDUSD.Clear()
                    lstListDate.Items.Clear()
                Case BONDTP_BRANCH
                    mv_lstListDateBONDTP.Clear()
                    lstListDate.Items.Clear()
                    'Thanglv9-add them thi truong 0007
                Case DLCN_BRANCH
                    mv_lstListDateDLCN.Clear()
                    lstListDate.Items.Clear()
                    'end
            End Select
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub RemoveOne()
        Try
            Select Case BRID
                Case VSD_BRANCH
                    mv_lstListDateVSD.Remove(lstListDate.SelectedItem)
                    lstListDate.Items.Remove(lstListDate.SelectedItem)
                Case HCM_BRANCH
                    mv_lstListDateHCM.Remove(lstListDate.SelectedItem)
                    lstListDate.Items.Remove(lstListDate.SelectedItem)
                Case OTC_BRANCH
                    mv_lstListDateOTC.Remove(lstListDate.SelectedItem)
                    lstListDate.Items.Remove(lstListDate.SelectedItem)
                Case BOND_BRANCH
                    mv_lstListDateBOND.Remove(lstListDate.SelectedItem)
                    lstListDate.Items.Remove(lstListDate.SelectedItem)
                Case BONDUSD_BRANCH
                    mv_lstListDateBONDUSD.Remove(lstListDate.SelectedItem)
                    lstListDate.Items.Remove(lstListDate.SelectedItem)
                Case BONDUSD_BRANCH
                    mv_lstListDateBONDTP.Remove(lstListDate.SelectedItem)
                    lstListDate.Items.Remove(lstListDate.SelectedItem)
                    'Thanglv9-add them thi truong 0007
                Case DLCN_BRANCH
                    mv_lstListDateDLCN.Remove(lstListDate.SelectedItem)
                    lstListDate.Items.Remove(lstListDate.SelectedItem)
                    'end
            End Select
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub AddOne()
        Try
            Dim v_dDate As String = CStr(Format(mcdCalendar.SelectionStart.Date, "dd/MM/yyyy"))

            Select Case BRID
                Case VSD_BRANCH
                    'If mv_htbListDateVSD.ContainsKey(Trim(v_dDate.ToString)) Then
                    '    MsgBox(mv_ResourceManager.GetString("DateExist"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                    'Else
                    '    mv_lstListDateVSD.Add(v_dDate.ToString)
                    '    lstListDate.Items.Add(Trim(v_dDate.ToString))
                    'End If
                    If mv_lstListDateVSD.Contains(Trim(v_dDate.ToString)) Then
                        MsgBox(mv_ResourceManager.GetString("DateExist"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                    Else
                        mv_lstListDateVSD.Add(v_dDate.ToString)
                        lstListDate.Items.Add(Trim(v_dDate.ToString))
                    End If
                Case HCM_BRANCH
                    'If mv_htbListDateHCM.ContainsKey(Trim(v_dDate.ToString)) Then
                    '    MsgBox(mv_ResourceManager.GetString("DateExist"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                    'Else
                    '    mv_lstListDateHCM.Add(v_dDate.ToString)
                    '    lstListDate.Items.Add(Trim(v_dDate.ToString))
                    'End If
                    If mv_lstListDateHCM.Contains(Trim(v_dDate.ToString)) Then
                        MsgBox(mv_ResourceManager.GetString("DateExist"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                    Else
                        mv_lstListDateHCM.Add(v_dDate.ToString)
                        lstListDate.Items.Add(Trim(v_dDate.ToString))
                    End If
                Case OTC_BRANCH
                    'If mv_htbListDateOTC.ContainsKey(Trim(v_dDate.ToString)) Then
                    '    MsgBox(mv_ResourceManager.GetString("DateExist"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                    'Else
                    '    mv_lstListDateOTC.Add(v_dDate.ToString)
                    '    lstListDate.Items.Add(Trim(v_dDate.ToString))
                    'End If
                    If mv_lstListDateOTC.Contains(Trim(v_dDate.ToString)) Then
                        MsgBox(mv_ResourceManager.GetString("DateExist"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                    Else
                        mv_lstListDateOTC.Add(v_dDate.ToString)
                        lstListDate.Items.Add(Trim(v_dDate.ToString))
                    End If
                Case BOND_BRANCH
                    'If mv_htbListDateOTC.ContainsKey(Trim(v_dDate.ToString)) Then
                    '    MsgBox(mv_ResourceManager.GetString("DateExist"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                    'Else
                    '    mv_lstListDateOTC.Add(v_dDate.ToString)
                    '    lstListDate.Items.Add(Trim(v_dDate.ToString))
                    'End If
                    If mv_lstListDateBOND.Contains(Trim(v_dDate.ToString)) Then
                        MsgBox(mv_ResourceManager.GetString("DateExist"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                    Else
                        mv_lstListDateBOND.Add(v_dDate.ToString)
                        lstListDate.Items.Add(Trim(v_dDate.ToString))
                    End If
                Case BONDUSD_BRANCH
                    'If mv_htbListDateOTC.ContainsKey(Trim(v_dDate.ToString)) Then
                    '    MsgBox(mv_ResourceManager.GetString("DateExist"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                    'Else
                    '    mv_lstListDateOTC.Add(v_dDate.ToString)
                    '    lstListDate.Items.Add(Trim(v_dDate.ToString))
                    'End If
                    If mv_lstListDateBONDUSD.Contains(Trim(v_dDate.ToString)) Then
                        MsgBox(mv_ResourceManager.GetString("DateExist"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                    Else
                        mv_lstListDateBONDUSD.Add(v_dDate.ToString)
                        lstListDate.Items.Add(Trim(v_dDate.ToString))
                    End If
                Case BONDTP_BRANCH
                    'If mv_htbListDateOTC.ContainsKey(Trim(v_dDate.ToString)) Then
                    '    MsgBox(mv_ResourceManager.GetString("DateExist"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                    'Else
                    '    mv_lstListDateOTC.Add(v_dDate.ToString)
                    '    lstListDate.Items.Add(Trim(v_dDate.ToString))
                    'End If
                    If mv_lstListDateBONDTP.Contains(Trim(v_dDate.ToString)) Then
                        MsgBox(mv_ResourceManager.GetString("DateExist"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                    Else
                        mv_lstListDateBONDTP.Add(v_dDate.ToString)
                        lstListDate.Items.Add(Trim(v_dDate.ToString))
                    End If
                    'Thanglv9-add them thi truong 0007
                Case DLCN_BRANCH
                    'If mv_htbListDateOTC.ContainsKey(Trim(v_dDate.ToString)) Then
                    '    MsgBox(mv_ResourceManager.GetString("DateExist"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                    'Else
                    '    mv_lstListDateOTC.Add(v_dDate.ToString)
                    '    lstListDate.Items.Add(Trim(v_dDate.ToString))
                    'End If
                    If mv_lstListDateDLCN.Contains(Trim(v_dDate.ToString)) Then
                        MsgBox(mv_ResourceManager.GetString("DateExist"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                    Else
                        mv_lstListDateDLCN.Add(v_dDate.ToString)
                        lstListDate.Items.Add(Trim(v_dDate.ToString))
                    End If
                    'end
            End Select
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub OnSubmit()
        Try
            UpdateSA()
            UpdateSysVarValue()
            MsgBox(mv_ResourceManager.GetString("EditSuccessful"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region

#Region "Event"
    Private Sub frmSysVarSetting_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        OnInit()
    End Sub


    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        OnSubmit()
    End Sub
    Private Sub btnRemoveAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveAll.Click
        If lstListDate.Items.Count = 0 Then
            MsgBox(mv_ResourceManager.GetString("EmptyListBox"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
        Else
            Dim v_intAns As Integer = MsgBox(mv_ResourceManager.GetString("ConfirmRemoveAll"), MsgBoxStyle.Information + MsgBoxStyle.YesNo, gc_ApplicationTitle)
            If v_intAns = vbYes Then
                RemoveAll()
            Else
                Return
            End If
        End If
    End Sub

    Private Sub btnRemoveOne_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveOne.Click
        If lstListDate.Items.Count = 0 Then
            MsgBox(mv_ResourceManager.GetString("EmptyListBox"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
        Else
            Dim v_intAns As Integer = MsgBox(mv_ResourceManager.GetString("ConfirmRemoveOne"), MsgBoxStyle.Information + MsgBoxStyle.YesNo, gc_ApplicationTitle)
            If v_intAns = vbYes Then
                RemoveOne()
            Else
                Return
            End If
        End If
    End Sub

    Private Sub btnAddOne_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddOne.Click
        Dim v_intAns As Integer = MsgBox(mv_ResourceManager.GetString("ConfirmAddOne"), MsgBoxStyle.Information + MsgBoxStyle.YesNo, gc_ApplicationTitle)
        If v_intAns = vbYes Then
            AddOne()
        Else
            Return
        End If
    End Sub
#End Region

    Private Sub cboBranchId_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboBranchId.SelectedIndexChanged
        System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("vi-VN")
        If Not cboBranchId.SelectedValue Is Nothing Then
            BRID = cboBranchId.SelectedValue.ToString
            Select Case BRID
                Case VSD_BRANCH
                    FillListDate(mv_lstListDateVSD, Month, Year)
                Case HCM_BRANCH
                    FillListDate(mv_lstListDateHCM, Month, Year)
                Case OTC_BRANCH
                    FillListDate(mv_lstListDateOTC, Month, Year)
                Case BOND_BRANCH
                    FillListDate(mv_lstListDateBOND, Month, Year)
                Case BONDUSD_BRANCH
                    FillListDate(mv_lstListDateBONDUSD, Month, Year)
                Case BONDTP_BRANCH
                    FillListDate(mv_lstListDateBONDTP, Month, Year)
                    'Thanglv9-add them thi truong 0007
                Case DLCN_BRANCH
                    FillListDate(mv_lstListDateDLCN, Month, Year)
                    'end
            End Select
        End If
        System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US")
    End Sub
    Private Sub btnInitAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInitAll.Click
        Dim v_strYear As String = InputBox("Vui lòng nhập năm muốn khởi tạo(Mặc định là năm hiện hành)", "Nhập năm khởi tạo", Now.Year.ToString)
        Dim v_bridSelected = cboBranchId.SelectedValue.ToString
        If IsNumeric(v_strYear) Then
            Dim v_intYear As Integer = CInt(v_strYear)
            If v_intYear >= 2000 And v_intYear <= 2020 Then
                InitAllYear(v_strYear, v_bridSelected)
            Else
                MsgBox(mv_ResourceManager.GetString("InitAllErrorRange"), MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                Exit Sub
            End If
        Else
            MsgBox(mv_ResourceManager.GetString("InitAllError"), MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
            Exit Sub
        End If
        MsgBox(mv_ResourceManager.GetString("InitAllSuccessfull"), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
    End Sub

    Private Sub mcdCalendar_DateChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DateRangeEventArgs) Handles mcdCalendar.DateChanged
        Try
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("vi-VN")
            Dim v_intMonth As Integer = mcdCalendar.SelectionStart.Date.Month
            Dim v_intYear As Integer = mcdCalendar.SelectionStart.Date.Year
            Month = v_intMonth
            Year = v_intYear
            Select Case BRID
                Case VSD_BRANCH
                    FillListDate(mv_lstListDateVSD, Month, Year)
                Case HCM_BRANCH
                    FillListDate(mv_lstListDateHCM, Month, Year)
                Case OTC_BRANCH
                    FillListDate(mv_lstListDateOTC, Month, Year)
                Case BOND_BRANCH
                    FillListDate(mv_lstListDateBOND, Month, Year)
                Case BONDUSD_BRANCH
                    FillListDate(mv_lstListDateBONDUSD, Month, Year)
                Case BONDTP_BRANCH
                    FillListDate(mv_lstListDateBONDTP, Month, Year)
                    'Thanglv9-add them thi truong 0007
                Case DLCN_BRANCH
                    FillListDate(mv_lstListDateDLCN, Month, Year)
                    'end
            End Select
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US")
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
End Class