Imports Sats.CommonLibrary
Imports System.Windows.Forms
Imports TestBase64
Imports System.IO
Imports SendFiles
Imports ZetaCompressionLibrary
Imports Sats.ClientCA


Public Enum SaveButtonType
    OKButton = 0
    ApplyButton = 1
    CancelButton = 2
End Enum

Public Class frmMaintenance

#Region " Khai báo biến, hằng "
    Private mv_intExecFlag As Integer
    Private mv_strLanguage As String
    Private mv_resourceManager As Resources.ResourceManager

    Private mv_strModuleCode As String
    Private mv_strObjectName As String
    Private mv_strTableName As String

    Private mv_strKeyField As String
    Private mv_strKeyType As String
    Private mv_strKeyValue As String

    Protected mv_arrObjFields() As CFieldMaster
    Private mv_arrObjFldVals() As CFieldVal

    Private mv_strBranchId As String
    Private mv_strTellerId As String
    Private mv_strAuthString As String
    Private mv_strTellerType As String
    Private mv_strBusDate As String

    'Added by Thanglv9 - 12/12/2012
    Private mv_strBRCODE As String
    Private mv_strTellerName As String
    Private mv_strIpAddress As String
    Private mv_strWsName As String
    Private mv_strMSGAMT As String
    Private mv_strMICODE As String
    Private mv_strCOMICODE As String
    Private mv_strSignCA As String

    Private mv_strVsdBrid As String
    Private mv_strVsdBrid2 As String
    Private mv_strSICODE As String
    Private mv_strBACKDATE As String
    Private mv_strCHKID As String
    Private mv_strOFFID As String
    Private mv_strCFRID As String
    Private mv_strCOTLTXCD As String = ""
    Private mv_strCHILDTLTXCD As String = ""
    Private mv_strRange As String = ""
    Private mv_strTXNUM As String = ""
    Private mv_strTXDATE As String
    Private mv_strISPARENT As String = ""
    Private mv_intISBRID As Integer = 1
    Private mv_intVisible_Child As Integer = 1
    Private mv_strTxNote As String
    Private mv_strTblChk As String

    Private mv_IsParent As Long = -1
    Private mv_strPassDate As String
    Private mv_strObjType As String = "T"
    Private mv_strCaption As String
    'Private mv_strBRCODE As String
    'End

    Private mv_strLocalObject As String
    Private mv_strXMLFldMaster As String

    Protected mv_dsOldInput As DataSet
    Protected mv_dsInput As DataSet

    Private mv_saveButtonType As SaveButtonType

    Private mo_SearchForm As Sats.AppCore.frmSearch

    'Public mv_BDSDelivery As BDSChannel.BDSDelivery
    Private mv_oProxy As BDSChannel.BDSDelivery
#End Region

#Region " Properties "

    Public Property Proxy() As BDSChannel.BDSDelivery
        Get
            Return mv_oProxy
        End Get
        Set(ByVal value As BDSChannel.BDSDelivery)
            mv_oProxy = value
        End Set
    End Property

    Public Property SearchForm() As Sats.AppCore.frmSearch
        Get
            Return mo_SearchForm
        End Get
        Set(ByVal value As Sats.AppCore.frmSearch)
            mo_SearchForm = value
        End Set
    End Property

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

    Public Property ExeFlag() As Integer
        Get
            Return mv_intExecFlag
        End Get
        Set(ByVal Value As Integer)
            mv_intExecFlag = Value
        End Set
    End Property

    Public Property KeyFieldName() As String
        Get
            Return mv_strKeyField
        End Get
        Set(ByVal Value As String)
            mv_strKeyField = Value
        End Set
    End Property

    Public Property KeyFieldType() As String
        Get
            Return mv_strKeyType
        End Get
        Set(ByVal Value As String)
            mv_strKeyType = Value
        End Set
    End Property

    Public Property KeyFieldValue() As String
        Get
            Return mv_strKeyValue
        End Get
        Set(ByVal Value As String)
            mv_strKeyValue = Replace(Value, ".", "")
        End Set
    End Property

    Public Property BusDate() As String
        Get
            Return mv_strBusDate
        End Get
        Set(ByVal Value As String)
            mv_strBusDate = Value
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
    Public Property AuthString() As String
        Get
            Return mv_strAuthString
        End Get
        Set(ByVal Value As String)
            mv_strAuthString = Value
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
            Return mv_resourceManager
        End Get
        Set(ByVal Value As Resources.ResourceManager)
            mv_resourceManager = Value
        End Set
    End Property

    Public ReadOnly Property SaveButtonType() As SaveButtonType
        Get
            Return mv_saveButtonType
        End Get
    End Property

    Public Property TellerType() As String
        Get
            Return mv_strTellerType
        End Get
        Set(ByVal Value As String)
            mv_strTellerType = Value
        End Set
    End Property

    'Added by Thanglv9 - 12/12/2012
    Public Property FormCaption() As String
        Get
            Return mv_strCaption
        End Get
        Set(ByVal Value As String)
            mv_strCaption = Value
            Me.Text = mv_strCaption
        End Set
    End Property
    Public Property PassDate() As String
        Get
            Return mv_strPassDate
        End Get
        Set(ByVal value As String)
            mv_strPassDate = value
        End Set
    End Property

    Public Property ParentID() As String
        Get
            Return mv_IsParent
        End Get
        Set(ByVal Value As String)
            mv_IsParent = Value
        End Set
    End Property
    Public Property BRCODE() As String
        Get
            Return mv_strBRCODE
        End Get
        Set(ByVal Value As String)
            mv_strBRCODE = Value
        End Set
    End Property
    Public Property IpAddress() As String
        Get
            Return mv_strIpAddress
        End Get
        Set(ByVal Value As String)
            mv_strIpAddress = Value
        End Set
    End Property

    Public Property WsName() As String
        Get
            Return mv_strWsName
        End Get
        Set(ByVal Value As String)
            mv_strWsName = Value
        End Set
    End Property
    Public Property ObjectType() As String
        Get
            Return mv_strObjType
        End Get
        Set(ByVal value As String)
            mv_strObjType = value
        End Set
    End Property

    Public Property VSDBRID() As String
        Get
            Return mv_strVsdBrid
        End Get
        Set(ByVal value As String)
            mv_strVsdBrid = value
        End Set
    End Property
    Public Property VSDBRID2() As String
        Get
            Return mv_strVsdBrid2
        End Get
        Set(ByVal value As String)
            mv_strVsdBrid2 = value
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
    Public Property TXDate() As String
        Get
            Return mv_strTXDATE
        End Get
        Set(ByVal value As String)
            mv_strTXDATE = value
        End Set
    End Property
    'End
#End Region

#Region " Overridable Functions "
    Public Overridable Sub DoShowScreen()

    End Sub

    Public Overridable Sub FillData()
        Dim v_strFilter As String = ""
        Dim v_strFLDNAME, v_strValue As String
        Dim v_strFieldType As String = "", v_strDataType As String = ""
        Dim v_blnRiskFld As Boolean

        Try
            Select Case KeyFieldType
                Case "C"
                    v_strFilter = KeyFieldName & " = '" & KeyFieldValue & "'"
                Case "D"
                    v_strFilter = KeyFieldName & " = TO_DATE('" & KeyFieldValue & "', '" & gc_FORMAT_DATE & "')"
                Case "N"
                    v_strFilter = KeyFieldName & " = " & KeyFieldValue.ToString()
            End Select

            Dim v_strlstField As String
            v_strlstField = ""
            For j As Integer = 0 To UBound(mv_arrObjFields) - 1
                If mv_arrObjFields(j).FieldType = "D" Then
                    v_strlstField &= "TO_CHAR(" & mv_arrObjFields(j).FieldName & ",'dd/MM/yyyy') " & mv_arrObjFields(j).FieldName & ","
                Else
                    v_strlstField &= mv_arrObjFields(j).FieldName & ","
                End If
            Next

            v_strlstField = Mid(v_strlstField, 1, Len(v_strlstField) - 1)
            Dim v_strCmdInquiry As String = "SELECT " & v_strlstField & " FROM " & TableName & " WHERE " & v_strFilter & " AND DELETED=0 and status =0"
            Dim v_strObjMsg As String = BuildXMLObjMsg(, BranchId, , TellerId, gc_IsNotLocalMsg, gc_MsgTypeObj, ObjectName, gc_ActionInquiry, v_strCmdInquiry, , , , "('000')|('000')")

            'Dim v_ws As New BDSDelivery.BDSDelivery
            'Dim v_lngError As Long = mv_BDSDelivery.Message(v_strObjMsg)
            Dim v_lngError As Long = Proxy.Message(v_strObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Exit Sub
            End If

            Dim v_xmlDocument As New Xml.XmlDocument
            Dim v_nodeList As Xml.XmlNodeList

            v_xmlDocument.LoadXml(v_strObjMsg)
            v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")

            If v_nodeList.Count = 1 Then
                For i As Integer = 0 To v_nodeList.Item(0).ChildNodes.Count - 1
                    With v_nodeList.Item(0).ChildNodes(i)
                        v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                        v_strValue = .InnerText.ToString
                    End With

                    For j As Integer = 0 To UBound(mv_arrObjFields) - 1
                        If mv_arrObjFields(j).FieldName = Trim(v_strFLDNAME) Then
                            v_strFieldType = mv_arrObjFields(j).FieldType
                            v_strDataType = mv_arrObjFields(j).DataType
                            v_blnRiskFld = mv_arrObjFields(j).RiskField
                            Exit For
                        End If
                    Next

                    SetControlValue(Me, v_strFLDNAME, v_strFieldType, v_strValue, v_strDataType)
                    If Me.ExeFlag = ExecuteFlag.Edit Then
                        SetRiskField(Me, v_strFLDNAME, v_strFieldType, Not v_blnRiskFld)
                    End If
                Next
            End If
        Catch ex As Exception
            Throw ex
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            '                                         & "Error code: System error!" & vbNewLine _
            '                                         & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            'MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Sub

    Public Overridable Sub OnClose()
        Me.Close()
    End Sub

    Public Sub DoFillReturnValue(ByVal v_strGLGRP As String, ByVal v_strModCode As String, ByVal v_ctrlAccountEntries As System.Windows.Forms.ListView, Optional ByRef v_strCurrency As String = "")
        Dim v_strObjMsg, v_strCurrencyTem As String
        Dim v_strCmdInquiry As String
        Try
            v_strCurrencyTem = v_strCurrency
            v_strCmdInquiry = "SELECT ACNAME ,ACCTNO FROM GLREF WHERE APPTYPE='" & v_strModCode & "' AND GLGRP ='" & v_strGLGRP & "' ORDER BY ACNAME "
            v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_LOOKUP, gc_ActionInquiry, v_strCmdInquiry, )
            Dim v_strOldObjMsg As String = v_strObjMsg

            'Dim v_ws As New BDSDelivery.BDSDelivery
            'Dim v_lngError As Long = mv_BDSDelivery.Message(v_strObjMsg)
            Dim v_lngError As Long = Proxy.Message(v_strObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_GET_MSG_VN, gc_ERR_GET_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Exit Sub
            End If

            Dim v_xmlDocument As New Xml.XmlDocument
            v_xmlDocument.LoadXml(v_strObjMsg)

            'Hien thi header
            v_ctrlAccountEntries.Clear()

            Dim v_ACNAME As New System.Windows.Forms.ColumnHeader
            v_ACNAME.Text = "ACNAME"
            v_ACNAME.Width = 80
            'v_ColReport.Text.
            v_ctrlAccountEntries.Columns.Add(v_ACNAME)

            Dim v_ACCTNO As New System.Windows.Forms.ColumnHeader
            v_ACCTNO.Text = "ACCTNO"
            v_ACCTNO.Width = 120
            v_ctrlAccountEntries.Columns.Add(v_ACCTNO)

            'Hien thi du lieu
            Dim v_ListViewItem As ListViewItem
            Dim v_strValue As String = ""
            Dim v_strFLDNAME As String = ""
            Dim v_strTEXT As String = ""
            Dim v_strAcc As String = ""
            Dim v_strAccName As String = ""

            Dim v_nodeList As Xml.XmlNodeList = Nothing
            Dim v_arrStr(2) As String
            v_ctrlAccountEntries.Items.Clear()
            v_xmlDocument.LoadXml(v_strObjMsg)
            v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")
            v_ctrlAccountEntries.Items.Clear()
            For i As Integer = 0 To v_nodeList.Count - 1
                v_strTEXT = vbNullString
                For j As Integer = 0 To v_nodeList.Item(i).ChildNodes.Count - 1
                    With v_nodeList.Item(i).ChildNodes(j)
                        v_strValue = Trim(.InnerText.ToString)
                        v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                        Select Case Trim(v_strFLDNAME)
                            Case "ACNAME"
                                v_strAccName = v_strValue
                            Case "ACCTNO"
                                v_strAcc = v_strValue
                                If i = 0 Then
                                    v_strCurrency = Strings.Mid(v_strAcc, 5, 2)
                                End If
                            Case Else
                        End Select
                    End With
                Next

                'Fill du lieu vao grid
                If v_strCurrencyTem <> "" Then
                    If Strings.Mid(v_strAcc, 5, 2) = v_strCurrencyTem Then
                        v_arrStr(0) = v_strAccName
                        v_arrStr(1) = v_strAcc
                        v_ListViewItem = New ListViewItem(v_arrStr)
                        v_ctrlAccountEntries.Items.Add(v_ListViewItem)
                    End If
                Else
                    v_arrStr(0) = v_strAccName
                    v_arrStr(1) = v_strAcc
                    v_ListViewItem = New ListViewItem(v_arrStr)
                    v_ctrlAccountEntries.Items.Add(v_ListViewItem)
                End If
            Next
            v_strCurrency = v_strAcc
        Catch ex As Exception
            v_ctrlAccountEntries.Visible = True
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                                     & "Error code: System error!" & vbNewLine _
                                                     & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Sub

    Public Overridable Sub OnInit()
        Try
            'Set click event for buttons
            'mv_BDSDelivery = New BDSChannel.BDSDelivery
            AddHandler btnOk.Click, AddressOf Button_Click
            AddHandler btnApply.Click, AddressOf Button_Click
            AddHandler btnCancel.Click, AddressOf Button_Click
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US")
            LoadObjectFields()
            LoadObjectFieldValidateRules()
            'FormatObjectFields(Me)

            'If ExeFlag <> ExecuteFlag.AddNew Then
            '    DoDataExchange()
            'End If

        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                                     & "Error code: System error!" & vbNewLine _
                                                     & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Sub

    Public Overridable Sub OnSave()
        PrepareDataSet(mv_dsInput)
    End Sub

    Public Overridable Function DoDataExchange(Optional ByVal pv_blnSaved As Boolean = False) As Boolean
        Dim v_dr As DataRow
        Dim v_ctrl As Windows.Forms.Control = Nothing

        Try
            If pv_blnSaved Then
                v_dr = mv_dsInput.Tables(0).NewRow()
                GetControlValue(v_dr, mv_dsInput, Me)
                mv_dsInput.Tables(0).Rows.Add(v_dr)
            Else
                FillData()
                PrepareDataSet(mv_dsOldInput)

                v_dr = mv_dsOldInput.Tables(0).NewRow()
                GetControlValue(v_dr, mv_dsOldInput, Me)
                mv_dsOldInput.Tables(0).Rows.Add(v_dr)
            End If

            Return True
        Catch ex As Exception
            'LogError.Write("Error source: " & ModuleCode & "." & ObjectName & ".DoDataExchange" & vbNewLine _
            '             & "Error code: System error!" & vbNewLine _
            '             & "Error message: " & IIf(v_ctrl.Name Is Nothing, ".", v_ctrl.Name & ".") & ex.Message, EventLogEntryType.Error)
            Throw ex
            'Return False
        End Try
    End Function

    Public Overridable Function OnShowRiskView() As Boolean
        Try

        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Overridable Sub LoadUserInterface(ByRef pv_ctrl As Windows.Forms.Control)
        Dim v_ctrl As Windows.Forms.Control
        Dim v_str As String

        Try
            btnOK.Enabled = (ExeFlag <> ExecuteFlag.View)
            btnApply.Enabled = (ExeFlag <> ExecuteFlag.View)

            For Each v_ctrl In pv_ctrl.Controls
                v_str = ResourceManager.GetString(v_ctrl.Tag)
                If v_str <> String.Empty Then
                    If TypeOf (v_ctrl) Is Label Then
                        CType(v_ctrl, Label).Text = ResourceManager.GetString(v_ctrl.Tag)
                    ElseIf TypeOf (v_ctrl) Is Button Then
                        CType(v_ctrl, Button).Text = ResourceManager.GetString(v_ctrl.Tag)
                    ElseIf TypeOf (v_ctrl) Is CheckBox Then
                        CType(v_ctrl, CheckBox).Text = ResourceManager.GetString(v_ctrl.Tag)
                    ElseIf TypeOf (v_ctrl) Is Panel Then
                        LoadUserInterface(v_ctrl)
                    ElseIf TypeOf (v_ctrl) Is GroupBox Then
                        CType(v_ctrl, GroupBox).Text = ResourceManager.GetString(v_ctrl.Tag)
                        LoadUserInterface(v_ctrl)
                    ElseIf TypeOf (v_ctrl) Is TabControl Then
                        For Each v_ctrlTmp As Control In CType(v_ctrl, TabControl).TabPages
                            CType(v_ctrlTmp, TabPage).Text = ResourceManager.GetString(v_ctrlTmp.Tag)
                            LoadUserInterface(v_ctrlTmp)
                        Next
                        'LoadUserInterface(v_ctrl)
                    ElseIf TypeOf (v_ctrl) Is TabPage Then
                        v_ctrl.BackColor = System.Drawing.SystemColors.InactiveCaptionText
                        CType(v_ctrl, TabPage).Text = ResourceManager.GetString(v_ctrl.Tag)
                        LoadUserInterface(v_ctrl)

                    ElseIf TypeOf (v_ctrl) Is ComboBoxEx Then
                        If ExeFlag = ExecuteFlag.AddNew Then
                            If CType(v_ctrl, ComboBoxEx).Items.Count > 0 Then
                                CType(v_ctrl, ComboBoxEx).SelectedIndex = 0
                            End If
                        End If
                    ElseIf TypeOf (v_ctrl) Is DateTimePicker Then
                        If ExeFlag = ExecuteFlag.AddNew Then
                            CType(v_ctrl, DateTimePicker).Value = CDate(Me.BusDate)
                        End If
                    End If
                End If
            Next
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region

#Region " Protected Functions "
    Protected Sub SetControlValue(ByRef pv_ctrl As Windows.Forms.Control, _
                                ByVal pv_strFLDNAME As String, _
                                ByVal pv_strFLDTYPE As String, _
                                ByVal pv_strFLDVAL As String, ByVal pv_strDATATYPE As String)
        Dim v_ctrl As Windows.Forms.Control

        Try
            For Each v_ctrl In pv_ctrl.Controls
                If TypeOf (v_ctrl) Is FlexMaskEditBox Then
                    If (v_ctrl.Tag = Trim(pv_strFLDNAME)) And (pv_strFLDTYPE = "M") Then
                        CType(v_ctrl, FlexMaskEditBox).Text = Trim(pv_strFLDVAL)
                        Exit For
                    End If
                ElseIf TypeOf (v_ctrl) Is TextBox Then
                    If (v_ctrl.Tag = Trim(pv_strFLDNAME)) And (pv_strFLDTYPE = "T" Or pv_strFLDTYPE = "M") Then
                        CType(v_ctrl, TextBox).Text = Trim(pv_strFLDVAL)
                        If (pv_strDATATYPE = "N") Then
                            FormatNumericTextbox(CType(v_ctrl, TextBox))
                        End If
                        Exit For
                    End If
                ElseIf TypeOf (v_ctrl) Is DateTimePicker Then
                    If (v_ctrl.Tag = Trim(pv_strFLDNAME)) And (pv_strFLDTYPE = "D") Then
                        If (pv_strFLDVAL.Trim().Length > 0) And (pv_strFLDVAL.Trim() <> gc_NULL_DATE) Then
                            CType(v_ctrl, DateTimePicker).Checked = True
                            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("vi-VN")
                            CType(v_ctrl, DateTimePicker).Value = CDate(pv_strFLDVAL)
                            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US")
                        Else
                            CType(v_ctrl, DateTimePicker).Value = CDate(gc_NULL_DATE)
                            CType(v_ctrl, DateTimePicker).Checked = False
                        End If
                        Exit For
                    End If
                ElseIf TypeOf (v_ctrl) Is ComboBoxEx Then
                    If (v_ctrl.Tag = Trim(pv_strFLDNAME)) And (pv_strFLDTYPE = "C") Then
                        If Trim(pv_strFLDVAL) = "" Then
                            CType(v_ctrl, ComboBoxEx).SelectedIndex = 0
                        Else
                            CType(v_ctrl, ComboBoxEx).SelectedValue = Trim(pv_strFLDVAL)
                        End If
                        Exit For
                    End If
                    'ElseIf TypeOf (v_ctrl) Is CheckBox Then
                    '    If (v_ctrl.Tag = Trim(pv_strFLDNAME)) And (pv_strFLDTYPE = "C") Then
                    '        CType(v_ctrl, CheckBox).Text = Trim(pv_strFLDVAL)
                    '        Exit For
                    '    End If

                ElseIf TypeOf (v_ctrl) Is PictureBox Then
                    If (v_ctrl.Tag = Trim(pv_strFLDNAME)) And (pv_strFLDTYPE = "C") Then
                        CType(v_ctrl, PictureBox).Image = GetImageFromString(pv_strFLDVAL)
                        CType(v_ctrl, PictureBox).SizeMode = PictureBoxSizeMode.CenterImage
                        CType(v_ctrl, PictureBox).BorderStyle = BorderStyle.Fixed3D
                        Exit For
                    End If

                ElseIf TypeOf (v_ctrl) Is GroupBox Then
                    SetControlValue(v_ctrl, pv_strFLDNAME, pv_strFLDTYPE, pv_strFLDVAL, pv_strDATATYPE)
                ElseIf TypeOf (v_ctrl) Is TabControl Then
                    SetControlValue(v_ctrl, pv_strFLDNAME, pv_strFLDTYPE, pv_strFLDVAL, pv_strDATATYPE)
                ElseIf TypeOf (v_ctrl) Is TabPage Then
                    SetControlValue(v_ctrl, pv_strFLDNAME, pv_strFLDTYPE, pv_strFLDVAL, pv_strDATATYPE)
                ElseIf TypeOf (v_ctrl) Is Panel Then
                    SetControlValue(v_ctrl, pv_strFLDNAME, pv_strFLDTYPE, pv_strFLDVAL, pv_strDATATYPE)
                End If
            Next
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                         & "Error code: System error!" & vbNewLine _
                         & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.OKOnly, gc_ApplicationTitle)
        End Try
    End Sub

    Protected Sub SetRiskField(ByRef pv_ctrl As Windows.Forms.Control, _
                                ByVal pv_strFLDNAME As String, _
                                ByVal pv_strFLDTYPE As String, _
                                ByVal pv_BlnRickFld As Boolean)
        Dim v_ctrl As Windows.Forms.Control

        Try
            For Each v_ctrl In pv_ctrl.Controls
                If TypeOf (v_ctrl) Is FlexMaskEditBox Then
                    If (v_ctrl.Tag = Trim(pv_strFLDNAME)) And (pv_strFLDTYPE = "M") Then
                        CType(v_ctrl, FlexMaskEditBox).Enabled = pv_BlnRickFld
                        Exit For
                    End If
                ElseIf TypeOf (v_ctrl) Is TextBox Then
                    If (v_ctrl.Tag = Trim(pv_strFLDNAME)) And (pv_strFLDTYPE = "T" Or pv_strFLDTYPE = "M") Then
                        CType(v_ctrl, TextBox).Enabled = pv_BlnRickFld
                        Exit For
                    End If
                ElseIf TypeOf (v_ctrl) Is DateTimePicker Then
                    If (v_ctrl.Tag = Trim(pv_strFLDNAME)) And (pv_strFLDTYPE = "D") Then
                        CType(v_ctrl, DateTimePicker).Enabled = pv_BlnRickFld
                        Exit For
                    End If
                ElseIf TypeOf (v_ctrl) Is ComboBoxEx Then
                    If (v_ctrl.Tag = Trim(pv_strFLDNAME)) And (pv_strFLDTYPE = "C") Then
                        CType(v_ctrl, ComboBoxEx).Enabled = pv_BlnRickFld
                        Exit For
                    End If
                ElseIf TypeOf (v_ctrl) Is PictureBox Then
                    If (v_ctrl.Tag = Trim(pv_strFLDNAME)) And (pv_strFLDTYPE = "C") Then
                        CType(v_ctrl, PictureBox).Enabled = pv_BlnRickFld
                        Exit For
                    End If
                ElseIf TypeOf (v_ctrl) Is GroupBox Then
                    SetRiskField(v_ctrl, pv_strFLDNAME, pv_strFLDTYPE, pv_BlnRickFld)
                ElseIf TypeOf (v_ctrl) Is TabControl Then
                    SetRiskField(v_ctrl, pv_strFLDNAME, pv_strFLDTYPE, pv_BlnRickFld)
                ElseIf TypeOf (v_ctrl) Is TabPage Then
                    SetRiskField(v_ctrl, pv_strFLDNAME, pv_strFLDTYPE, pv_BlnRickFld)
                ElseIf TypeOf (v_ctrl) Is Panel Then
                    SetRiskField(v_ctrl, pv_strFLDNAME, pv_strFLDTYPE, pv_BlnRickFld)
                End If
            Next
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                         & "Error code: System error!" & vbNewLine _
                         & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.OKOnly, gc_ApplicationTitle)
        End Try
    End Sub
#End Region

#Region " Private Functions "
    Private Sub GetControlValue(ByRef pv_dr As DataRow, _
                                ByVal pv_ds As DataSet, _
                                ByVal pv_ctrl As Windows.Forms.Control)
        Dim v_dc As DataColumn
        Dim v_ctrl, v_ctrTabPage As Windows.Forms.Control
        Dim v_strFldType As String = "", v_strDataType As String = ""

        Try
            For Each v_ctrl In pv_ctrl.Controls
                If (TypeOf (v_ctrl) Is TabControl) Then
                    For Each v_ctrTabPage In v_ctrl.Controls
                        If (TypeOf (v_ctrTabPage) Is TabPage) Then
                            GetControlValue(pv_dr, pv_ds, v_ctrTabPage)
                        End If
                    Next
                Else
                    For Each v_dc In pv_ds.Tables(0).Columns
                        If (TypeOf (v_ctrl) Is TextBox) Then
                            If UCase(v_dc.ColumnName) = UCase(v_ctrl.Tag) Then
                                For i As Integer = 0 To UBound(mv_arrObjFields) - 1
                                    If (UCase(v_ctrl.Tag) = mv_arrObjFields(i).FieldName) Then
                                        v_strFldType = mv_arrObjFields(i).FieldType
                                        v_strDataType = mv_arrObjFields(i).DataType
                                        Exit For
                                    End If
                                Next
                                If (v_strFldType = "T") And (v_strDataType = "N") Then
                                    If Trim(CType(v_ctrl, TextBox).Text) = "" Then
                                        pv_dr(CType(v_ctrl, TextBox).Tag) = "0"
                                    Else
                                        pv_dr(CType(v_ctrl, TextBox).Tag) = Trim(CType(v_ctrl, TextBox).Text)
                                    End If
                                ElseIf (v_strFldType = "T") And (v_strDataType <> "N") Then
                                    pv_dr(CType(v_ctrl, TextBox).Tag) = CType(v_ctrl, TextBox).Text.Trim()
                                ElseIf (v_strFldType = "M") And (v_strDataType = "N") Then
                                    pv_dr(CType(v_ctrl, FlexMaskEditBox).Tag) = CType(v_ctrl, FlexMaskEditBox).Text.Trim()
                                ElseIf (v_strFldType = "M") Then
                                    pv_dr(CType(v_ctrl, FlexMaskEditBox).Tag) = CType(v_ctrl, FlexMaskEditBox).Text.Trim()
                                End If
                                Exit For
                            End If
                        ElseIf (TypeOf (v_ctrl) Is FlexMaskEditBox) Then
                            If UCase(v_dc.ColumnName) = UCase(v_ctrl.Tag) Then
                                pv_dr(CType(v_ctrl, FlexMaskEditBox).Tag) = Trim(CType(v_ctrl, FlexMaskEditBox).Text)
                                Exit For
                            End If
                        ElseIf (TypeOf (v_ctrl) Is DateTimePicker) Then
                            If UCase(v_dc.ColumnName) = UCase(v_ctrl.Tag) Then
                                If CType(v_ctrl, DateTimePicker).Checked Then
                                    pv_dr(CType(v_ctrl, DateTimePicker).Tag) = Trim(CStr(CType(v_ctrl, DateTimePicker).Value))
                                Else
                                    pv_dr(CType(v_ctrl, DateTimePicker).Tag) = CDate(gc_NULL_DATE)
                                End If
                                Exit For
                            End If
                        ElseIf (TypeOf (v_ctrl) Is ComboBoxEx) Then

                            If UCase(v_dc.ColumnName) = UCase(v_ctrl.Tag) Then
                                pv_dr(CType(v_ctrl, ComboBoxEx).Tag) = Trim(CType(v_ctrl, ComboBoxEx).SelectedValue)
                                Exit For
                            End If
                            'ElseIf (TypeOf (v_ctrl) Is CheckBox) Then
                            '    If UCase(v_dc.ColumnName) = UCase(v_ctrl.Tag) Then
                            '        pv_dr(CType(v_ctrl, CheckBox).Tag) = Trim(CType(v_ctrl, CheckBox).Text)
                            '        Exit For
                            '    End If

                        ElseIf (TypeOf (v_ctrl) Is PictureBox) Then
                            If UCase(v_dc.ColumnName) = UCase(v_ctrl.Tag) Then
                                pv_dr(CType(v_ctrl, PictureBox).Tag) = GetStringFromImage(CType(v_ctrl, PictureBox))
                                Exit For
                            End If
                        ElseIf (TypeOf (v_ctrl) Is GroupBox) Then
                            GetControlValue(pv_dr, pv_ds, v_ctrl)
                        End If
                    Next
                End If
            Next
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                         & "Error code: System error!" & vbNewLine _
                         & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.OKOnly, gc_ApplicationTitle)
        End Try
    End Sub

    Private Function GetStringFromImage(ByVal pv_PicBox As PictureBox) As String

        Dim v_mStream As New MemoryStream
        Dim v_Compressed As Byte()
        Dim v_Encoded As Char()
        Dim v_arrMyImage As Byte()
        Dim v_strBuilder As String = String.Empty
        'CType((v_ctrl), PictureBox)()
        If pv_PicBox.Image Is Nothing Then
            Return ""
        Else
            pv_PicBox.Image.Save(v_mStream, pv_PicBox.Image.RawFormat)
            v_arrMyImage = v_mStream.GetBuffer
            v_mStream.Close()
            v_Compressed = CompressionHelper.CompressBytes(v_arrMyImage)
            Dim v_BE As New Base64Encoder(v_Compressed)
            v_Encoded = v_BE.GetEncoded
            v_strBuilder &= v_Encoded
            Return v_strBuilder
        End If
    End Function

    Private Function GetImageFromString(ByVal pv_strFLDVAL) As System.Drawing.Bitmap
        If pv_strFLDVAL = "" Then
            Return Nothing
        Else
            Dim v_strCompress As String = Trim(pv_strFLDVAL)
            Dim v_Compression As Byte()
            Dim v_Base64Decoder As New Base64Decoder(v_strCompress)
            v_Compression = v_Base64Decoder.GetDecoded()
            Dim v_arrActualSignImage As Byte()
            v_arrActualSignImage = CompressionHelper.DecompressBytes(v_Compression)
            Dim tmpImage As System.Drawing.Bitmap = New System.Drawing.Bitmap(New MemoryStream(v_arrActualSignImage))
            Return tmpImage
        End If
    End Function

    Public Function GetControlValueByName(ByVal pv_strFLDNAME As String, ByVal pv_ctrl As Control) As String
        Dim v_strReturnValue As String = String.Empty

        Try
            For Each v_ctrl As Control In pv_ctrl.Controls
                If (TypeOf (v_ctrl) Is TabControl) Then
                    For Each v_ctrTabPage As Control In v_ctrl.Controls
                        If (TypeOf (v_ctrTabPage) Is TabPage) Then
                            v_strReturnValue = GetControlValueByName(pv_strFLDNAME, v_ctrTabPage)
                            If (v_strReturnValue <> String.Empty) Then
                                Exit For
                            End If
                        End If
                    Next
                ElseIf (TypeOf (v_ctrl) Is TextBox) Then
                    If (UCase(v_ctrl.Tag) = pv_strFLDNAME) Then
                        v_strReturnValue = CType(v_ctrl, TextBox).Text
                        Exit For
                    End If
                ElseIf (TypeOf (v_ctrl) Is FlexMaskEditBox) Then
                    If (UCase(v_ctrl.Tag) = pv_strFLDNAME) Then
                        v_strReturnValue = CType(v_ctrl, FlexMaskEditBox).Text
                        Exit For
                    End If
                ElseIf (TypeOf (v_ctrl) Is DateTimePicker) Then
                    If (UCase(v_ctrl.Tag) = pv_strFLDNAME) Then
                        If CType(v_ctrl, DateTimePicker).Checked Then
                            v_strReturnValue = Trim(CStr(CType(v_ctrl, DateTimePicker).Value))
                        Else
                            v_strReturnValue = CDate(gc_NULL_DATE)
                        End If
                        Exit For
                    End If
                ElseIf (TypeOf (v_ctrl) Is ComboBoxEx) Then
                    If (UCase(v_ctrl.Tag) = pv_strFLDNAME) Then
                        v_strReturnValue = CType(v_ctrl, ComboBoxEx).SelectedValue.ToString()
                        Exit For
                    End If
                    'ElseIf (TypeOf (v_ctrl) Is PictureBox) Then
                    '    If (UCase(v_ctrl.Tag) = pv_strFLDNAME) Then
                    '        v_strReturnValue = CType(v_ctrl, PictureBox).Image.
                    '    End If

                ElseIf (TypeOf (v_ctrl) Is GroupBox) Then
                    v_strReturnValue = GetControlValueByName(pv_strFLDNAME, v_ctrl)
                    If (v_strReturnValue <> String.Empty) Then
                        Exit For
                    End If
                End If
            Next
            Return v_strReturnValue
        Catch ex As Exception
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            '             & "Error code: System error!" & vbNewLine _
            '             & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            'MsgBox(ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
            'Return String.Empty
            Throw ex
        End Try
    End Function

    Private Function GetReferenceData(ByVal pv_xmlObjDataRef As String, ByVal pv_strFLDNAME As String) As String
        Dim v_xmlDocument As New Xml.XmlDocument, v_xmlRefDocument As New Xml.XmlDocument
        Dim v_strObjMsg As String = BuildXMLObjMsg(, , , , gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_FLDMASTER, gc_ActionInquiry)
        v_xmlRefDocument.LoadXml(pv_xmlObjDataRef)
        v_xmlDocument.LoadXml(v_strObjMsg)

        Dim v_dataElement As Xml.XmlElement, v_entryNode As Xml.XmlNode
        v_dataElement = v_xmlDocument.CreateElement(Xml.XmlNodeType.Element, "ObjData", "")
        Dim v_nodeListRef As Xml.XmlNodeList, i, j As Integer, v_strREFNAME As String
        v_nodeListRef = v_xmlRefDocument.SelectNodes("/ObjectMessage/ObjDataRef")
        For i = 0 To v_nodeListRef.Count - 1
            For j = 0 To v_nodeListRef.Item(i).ChildNodes.Count - 1
                With v_nodeListRef.Item(i).ChildNodes(j)
                    v_strREFNAME = CStr(CType(.Attributes.GetNamedItem("refname"), Xml.XmlAttribute).Value)
                    If Trim(v_strREFNAME) = (pv_strFLDNAME) Then
                        'Lấy nội dung của node để xử lý
                        v_entryNode = v_nodeListRef.Item(i).ChildNodes(j)
                        v_dataElement.AppendChild(v_entryNode)
                    End If
                End With
            Next
        Next
        v_xmlDocument.DocumentElement.AppendChild(v_dataElement)
        GetReferenceData = v_xmlDocument.InnerXml
    End Function

    Private Sub LoadObjectFields()
        Dim v_lngErrCode As Long = ERR_SYSTEM_OK
        Dim v_strErrorSource As String = Me.Name & ".Base.LoadObjectFields"

        Dim v_strFieldName As String = "", v_strDefName As String = "", v_strCaption As String = ""
        Dim v_strEnCaption As String = "", v_strFldType As String = "", v_strFldMask As String = ""
        Dim v_strFldFormat As String = "", v_strLList As String = "", v_strLChk As String = ""
        Dim v_strDefVal As String = "", v_strAmtExp As String = "", v_strValidTag As String = ""
        Dim v_strLookUp As String = "", v_strDataType As String = "", v_strLookupName As String = ""
        Dim v_strSearchCode As String = "", v_strSRMODCode As String = ""
        Dim v_intOdrNum, v_intFldLen As Integer
        Dim v_blnVisible, v_blnEnabled, v_blnMandatory, v_blnRiskField As Boolean
        Dim v_obj As SQLEngine.SQLDataAccessLayer
        Dim v_ds As DataSet
        Try
        
            v_obj = New SQLEngine.SQLDataAccessLayer(TellerId)
            Dim v_strSQL As String
            v_strSQL = "SELECT * FROM FLDMASTER WHERE MODCODE = '" & ModuleCode & "' AND OBJNAME = '" & ObjectName & "' ORDER BY ODRNUM"

            v_ds = v_obj.ExecuteReturnDataSet(v_strSQL)

            ReDim mv_arrObjFields(v_ds.Tables(0).Rows.Count)

            For i As Integer = 0 To v_ds.Tables(0).Rows.Count - 1
                With v_ds.Tables(0)
                    v_strFieldName = Trim(.Rows(i)("FLDNAME"))
                    v_strDefName = Trim(.Rows(i)("DEFNAME"))
                    v_strCaption = Trim(.Rows(i)("CAPTION"))
                    v_strEnCaption = Trim(.Rows(i)("EN_CAPTION"))
                    v_intOdrNum = CInt(.Rows(i)("ODRNUM"))
                    v_strFldType = Trim(.Rows(i)("FLDTYPE"))
                    v_strFldMask = Trim(.Rows(i)("FLDMASK"))
                    v_strFldFormat = Trim(.Rows(i)("FLDFORMAT"))
                    v_intFldLen = CInt(.Rows(i)("FLDLEN"))
                    v_strLList = Trim(.Rows(i)("LLIST"))
                    v_strLChk = Trim(.Rows(i)("LCHK"))
                    v_strDefVal = Trim(.Rows(i)("DEFVAL"))
                    v_blnVisible = (Trim(.Rows(i)("VISIBLE")) = "Y")
                    v_blnEnabled = (Trim(.Rows(i)("DISABLE")) = "N")
                    v_blnMandatory = (Trim(.Rows(i)("MANDATORY")) = "Y")
                    v_strAmtExp = Trim(.Rows(i)("AMTEXP"))
                    v_strValidTag = Trim(.Rows(i)("VALIDTAG"))
                    v_strLookUp = Trim(.Rows(i)("LOOKUP"))
                    v_strDataType = Trim(.Rows(i)("DATATYPE"))
                    v_strLookupName = Trim(.Rows(i)("LOOKUPNAME"))
                    v_strSearchCode = Trim(.Rows(i)("SEARCHCODE"))
                    v_strSRMODCode = Trim(.Rows(i)("SRMODCODE"))
                    v_blnRiskField = (Trim(.Rows(i)("RISKFLD")) = "Y")
                End With

                Dim v_objField As New CFieldMaster
                With v_objField
                    .FieldName = v_strFieldName
                    .ColumnName = v_strDefName
                    .Caption = v_strCaption
                    .EnCaption = v_strEnCaption
                    .DisplayOrder = v_intOdrNum
                    .FieldType = v_strFldType
                    .InputMask = v_strFldMask
                    .FieldFormat = v_strFldFormat
                    .FieldLength = v_intFldLen
                    .LookupList = v_strLList
                    .LookupCheck = v_strLChk
                    .DefaultValue = v_strDefVal
                    .Visible = v_blnVisible
                    .Enabled = v_blnEnabled
                    .Mandatory = v_blnMandatory
                    .AmtExp = v_strAmtExp
                    .ValidTag = v_strValidTag
                    .LookUp = v_strLookUp
                    .DataType = v_strDataType
                    .LookupName = v_strLookupName
                    .SearchCode = v_strSearchCode
                    .SrModCode = v_strSRMODCode
                    .RiskField = v_blnRiskField
                End With
                mv_arrObjFields(i) = v_objField
            Next

            ReDim Preserve mv_arrObjFields(v_ds.Tables(0).Rows.Count)
        Catch ex As Exception
            Throw ex
        Finally
            v_obj.CloseConnection()
            v_obj = Nothing
            v_ds.Dispose()
        End Try
    End Sub

    Private Sub LoadObjectFieldValidateRules()
        Dim v_strClause, v_strObjMsg, v_strValue, v_strFLDNAME As String
        Dim v_intIndex As Integer
        Dim v_objFieldVal As CFieldVal
        Dim v_obj As SQLEngine.SQLDataAccessLayer
        Dim v_ds As DataSet
        Try
            'Lấy các luật kiểm tra của các trường giao dịch
            v_strClause = "SELECT FLDNAME, VALTYPE, OPERATOR, VALEXP, VALEXP2, ERRMSG, EN_ERRMSG FROM FLDVAL " & _
                "WHERE upper(OBJNAME) = '" & ObjectName & "' ORDER BY VALTYPE, ODRNUM" 'Thứ tự order by là quan trọng không sửa
            v_obj = New SQLEngine.SQLDataAccessLayer(TellerId)
            v_ds = v_obj.ExecuteReturnDataSet(v_strClause)

            ReDim mv_arrObjFldVals(v_ds.Tables(0).Rows.Count)

            Dim v_strFieldVal_FldName As String = "", v_strFieldVal_ValType As String = "", v_strFieldVal_Operator As String = ""
            Dim v_strFieldVal_ValExp As String = "", v_strFieldVal_ValExp2 As String = "", v_strFieldVal_ErrMsg As String = "", v_strFieldVal_EnErrMsg As String = ""

            For i As Integer = 0 To v_ds.Tables(0).Rows.Count - 1
                With v_ds.Tables(0)
                    'Ghi nhận thuật toán để kiểm tra và tính toán cho từng trường của form
                    v_strFieldVal_FldName = Trim(.Rows(i)("FLDNAME"))
                    v_strFieldVal_ValType = Trim(.Rows(i)("VALTYPE"))
                    v_strFieldVal_Operator = Trim(.Rows(i)("OPERATOR"))
                    v_strFieldVal_ValExp = Trim(.Rows(i)("VALEXP"))
                    v_strFieldVal_ValExp2 = Trim(.Rows(i)("VALEXP2"))
                    v_strFieldVal_ErrMsg = Trim(.Rows(i)("ERRMSG"))
                    v_strFieldVal_EnErrMsg = Trim(.Rows(i)("EN_ERRMSG"))
                End With

                'Xác định index của mảng FldMaster
                For j As Integer = 0 To mv_arrObjFields.GetLength(0) - 1 Step 1
                    If Not mv_arrObjFields(j) Is Nothing Then
                        If Trim(mv_arrObjFields(j).FieldName) = Trim(v_strFieldVal_FldName) Then
                            v_intIndex = j
                            Exit For
                        End If
                    End If
                Next

                'Điều kiện xử lý
                v_objFieldVal = New CFieldVal
                With v_objFieldVal
                    .OBJNAME = ObjectName
                    .FLDNAME = v_strFieldVal_FldName
                    .VALTYPE = v_strFieldVal_ValType
                    .mp_OPERATOR = v_strFieldVal_Operator
                    .VALEXP = v_strFieldVal_ValExp
                    .VALEXP2 = v_strFieldVal_ValExp2
                    .ERRMSG = v_strFieldVal_ErrMsg
                    .EN_ERRMSG = v_strFieldVal_EnErrMsg
                    .IDXFLD = v_intIndex
                End With
                mv_arrObjFldVals(i) = v_objFieldVal
            Next

            ReDim Preserve mv_arrObjFldVals(v_ds.Tables(0).Rows.Count)
        Catch ex As Exception
            Throw ex
        Finally
            v_obj.CloseConnection()
            v_obj = Nothing
            v_ds.Dispose()
        End Try
    End Sub

    Protected Function VerifyRules() As Boolean
        'Dim v_intIndex As Integer
        Dim v_strFLDVALUE, v_strVALEXP As String

        Try
            'Verify các trường dữ liệu bắt buộc phải nhập và dữ liệu kiểu số
            For i As Integer = 0 To UBound(mv_arrObjFields) - 1
                If ((mv_arrObjFields(i).FieldType = "T") Or (mv_arrObjFields(i).FieldType = "M")) _
                    And (mv_arrObjFields(i).Mandatory) And (mv_arrObjFields(i).Enabled = True) Then
                    v_strFLDVALUE = GetControlValueByName(mv_arrObjFields(i).FieldName, Me).ToString().Trim()

                    If Not (v_strFLDVALUE.Length > 0) Then
                        If Me.UserLanguage = "EN" Then
                            MsgBox(Replace(gc_EN_MANDATORY, "@", mv_arrObjFields(i).EnCaption), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                        Else
                            MsgBox(Replace(gc_VN_MANDATORY, "@", mv_arrObjFields(i).Caption), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                        End If
                        SetControlFocus(Me, mv_arrObjFields(i).FieldName)
                        Return False
                    End If
                ElseIf (mv_arrObjFields(i).FieldType = "D") And (mv_arrObjFields(i).Mandatory) Then
                    v_strFLDVALUE = GetControlValueByName(mv_arrObjFields(i).FieldName, Me).ToString().Trim()
                    If (Not (v_strFLDVALUE.Length > 0)) Or (v_strFLDVALUE.Trim() = gc_NULL_DATE) Then
                        If Me.UserLanguage = "EN" Then
                            MsgBox(Replace(gc_EN_MANDATORY, "@", mv_arrObjFields(i).EnCaption), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                        Else
                            MsgBox(Replace(gc_VN_MANDATORY, "@", mv_arrObjFields(i).Caption), MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                        End If
                        SetControlFocus(Me, mv_arrObjFields(i).FieldName)
                        Return False
                    End If
                End If

                If ((mv_arrObjFields(i).FieldType = "T") Or (mv_arrObjFields(i).FieldType = "M")) _
                    And (mv_arrObjFields(i).DataType = "N") Then
                    v_strFLDVALUE = GetControlValueByName(mv_arrObjFields(i).FieldName, Me).ToString().Trim()

                    If v_strFLDVALUE.Length > 0 Then
                        If Not IsNumeric(v_strFLDVALUE) Then
                            If Me.UserLanguage = "EN" Then
                                MsgBox(Replace(ResourceManager.GetString("NumericDataType"), "@", mv_arrObjFields(i).EnCaption), MsgBoxStyle.Information + MsgBoxStyle.OKOnly, gc_ApplicationTitle)
                            Else
                                MsgBox(Replace(ResourceManager.GetString("NumericDataType"), "@", mv_arrObjFields(i).Caption), MsgBoxStyle.Information + MsgBoxStyle.OKOnly, gc_ApplicationTitle)
                            End If
                            SetControlFocus(Me, mv_arrObjFields(i).FieldName)
                            Return False
                        End If
                    End If
                End If
            Next

            'Duyệt mảng dữ liệu danh mục các điều kiện kiểm tra
            Dim v_intCount As Integer = mv_arrObjFldVals.GetLength(0)
            Dim v_objEval As New Evaluator

            If v_intCount > 0 Then
                For i As Integer = 0 To v_intCount - 1 Step 1
                    If Not mv_arrObjFldVals(i) Is Nothing Then
                        'Xử lý theo tham số đã cài đặt
                        With mv_arrObjFldVals(i)
                            Select Case GetFieldDataType(.FLDNAME)
                                Case "N"
                                    'Thực hiện xử lý cho từng phép toán
                                    If .VALTYPE = "E" Then
                                        'Do nothing
                                    ElseIf .VALTYPE = "V" Then
                                        'Lấy dữ liệu của trường cần validate
                                        v_strFLDVALUE = GetControlValueByName(.FLDNAME, Me)

                                        If (v_strFLDVALUE.Length > 0) Then      'Chỉ validate khi NSD nhập dữ liệu; ngược lại, bỏ qua
                                            Select Case .mp_OPERATOR
                                                Case ">>"
                                                    v_strVALEXP = BuildAMTEXP(.VALEXP)
                                                    If Not (CDbl(v_strFLDVALUE) > v_objEval.Eval(v_strVALEXP)) Then
                                                        If Me.UserLanguage = "EN" Then
                                                            MsgBox(.EN_ERRMSG, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                                                        Else
                                                            MsgBox(.ERRMSG, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                                                        End If
                                                        SetControlFocus(Me, mv_arrObjFldVals(i).FLDNAME)
                                                        Return False
                                                    End If
                                                Case ">="
                                                    v_strVALEXP = BuildAMTEXP(.VALEXP)
                                                    If Not (CDbl(v_strFLDVALUE) >= v_objEval.Eval(v_strVALEXP)) Then
                                                        If Me.UserLanguage = "EN" Then
                                                            MsgBox(.EN_ERRMSG, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                                                        Else
                                                            MsgBox(.ERRMSG, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                                                        End If
                                                        SetControlFocus(Me, mv_arrObjFldVals(i).FLDNAME)
                                                        Return False
                                                    End If
                                                Case "<<"
                                                    v_strVALEXP = BuildAMTEXP(.VALEXP)
                                                    If Not (CDbl(v_strFLDVALUE) < v_objEval.Eval(v_strVALEXP)) Then
                                                        If Me.UserLanguage = "EN" Then
                                                            MsgBox(.EN_ERRMSG, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                                                        Else
                                                            MsgBox(.ERRMSG, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                                                        End If
                                                        SetControlFocus(Me, mv_arrObjFldVals(i).FLDNAME)
                                                        Return False
                                                    End If
                                                Case "<="
                                                    v_strVALEXP = BuildAMTEXP(.VALEXP)
                                                    If Not (CDbl(v_strFLDVALUE) <= v_objEval.Eval(v_strVALEXP)) Then
                                                        If Me.UserLanguage = "EN" Then
                                                            MsgBox(.EN_ERRMSG, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                                                        Else
                                                            MsgBox(.ERRMSG, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                                                        End If
                                                        SetControlFocus(Me, mv_arrObjFldVals(i).FLDNAME)
                                                        Return False
                                                    End If
                                                Case "=="
                                                    v_strVALEXP = BuildAMTEXP(.VALEXP)
                                                    If Not (CDbl(v_strFLDVALUE) = v_objEval.Eval(v_strVALEXP)) Then
                                                        If Me.UserLanguage = "EN" Then
                                                            MsgBox(.EN_ERRMSG, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                                                        Else
                                                            MsgBox(.ERRMSG, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                                                        End If
                                                        SetControlFocus(Me, mv_arrObjFldVals(i).FLDNAME)
                                                        Return False
                                                    End If
                                                Case "<>"
                                                    v_strVALEXP = BuildAMTEXP(.VALEXP)
                                                    If Not (CDbl(v_strFLDVALUE) <> v_objEval.Eval(v_strVALEXP)) Then
                                                        If Me.UserLanguage = "EN" Then
                                                            MsgBox(.EN_ERRMSG, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                                                        Else
                                                            MsgBox(.ERRMSG, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                                                        End If
                                                        SetControlFocus(Me, mv_arrObjFldVals(i).FLDNAME)
                                                        Return False
                                                    End If
                                            End Select
                                        End If
                                    End If
                                Case "D"
                                    'Thực hiện xử lý cho từng phép toán
                                    If .VALTYPE = "E" Then
                                        'Do nothing
                                    ElseIf .VALTYPE = "V" Then
                                        'Lấy dữ liệu của trường cần validate
                                        v_strFLDVALUE = GetControlValueByName(.FLDNAME, Me)

                                        If (v_strFLDVALUE.Length > 0) Then      'Chỉ validate khi NSD nhập dữ liệu; ngược lại, bỏ qua
                                            Select Case .mp_OPERATOR
                                                Case ">>"
                                                    If Not (DDMMYYYY_SystemDate(v_strFLDVALUE) > BuildDATEEXP(.VALEXP)) Then
                                                        If Me.UserLanguage = "EN" Then
                                                            MsgBox(.EN_ERRMSG, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                                                        Else
                                                            MsgBox(.ERRMSG, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                                                        End If
                                                        SetControlFocus(Me, mv_arrObjFldVals(i).FLDNAME)
                                                        Return False
                                                    End If
                                                Case ">="
                                                    If Not (DDMMYYYY_SystemDate(v_strFLDVALUE) >= BuildDATEEXP(.VALEXP)) Then
                                                        If Me.UserLanguage = "EN" Then
                                                            MsgBox(.EN_ERRMSG, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                                                        Else
                                                            MsgBox(.ERRMSG, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                                                        End If
                                                        SetControlFocus(Me, mv_arrObjFldVals(i).FLDNAME)
                                                        Return False
                                                    End If
                                                Case "<<"
                                                    If Not (DDMMYYYY_SystemDate(v_strFLDVALUE) < BuildDATEEXP(.VALEXP)) Then
                                                        If Me.UserLanguage = "EN" Then
                                                            MsgBox(.EN_ERRMSG, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                                                        Else
                                                            MsgBox(.ERRMSG, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                                                        End If
                                                        SetControlFocus(Me, mv_arrObjFldVals(i).FLDNAME)
                                                        Return False
                                                    End If
                                                Case "<="
                                                    If Not (DDMMYYYY_SystemDate(v_strFLDVALUE) <= BuildDATEEXP(.VALEXP)) Then
                                                        If Me.UserLanguage = "EN" Then
                                                            MsgBox(.EN_ERRMSG, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                                                        Else
                                                            MsgBox(.ERRMSG, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                                                        End If
                                                        SetControlFocus(Me, mv_arrObjFldVals(i).FLDNAME)
                                                        Return False
                                                    End If
                                                Case "=="
                                                    If Not (DDMMYYYY_SystemDate(v_strFLDVALUE) = BuildDATEEXP(.VALEXP)) Then
                                                        If Me.UserLanguage = "EN" Then
                                                            MsgBox(.EN_ERRMSG, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                                                        Else
                                                            MsgBox(.ERRMSG, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                                                        End If
                                                        SetControlFocus(Me, mv_arrObjFldVals(i).FLDNAME)
                                                        Return False
                                                    End If
                                                Case "<>"
                                                    If Not (DDMMYYYY_SystemDate(v_strFLDVALUE) <> BuildDATEEXP(.VALEXP)) Then
                                                        If Me.UserLanguage = "EN" Then
                                                            MsgBox(.EN_ERRMSG, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                                                        Else
                                                            MsgBox(.ERRMSG, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, gc_ApplicationTitle)
                                                        End If
                                                        SetControlFocus(Me, mv_arrObjFldVals(i).FLDNAME)
                                                        Return False
                                                    End If
                                            End Select
                                        End If
                                    End If
                                Case "C"

                            End Select
                        End With
                    End If
                Next
            End If

            Return True
        Catch ex As Exception
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            '             & "Error code: System error!" & vbNewLine _
            '             & "Error message: " & ex.Message, EventLogEntryType.Error)
            'MsgBox(ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.OKOnly, gc_ApplicationTitle)
            Throw ex
        End Try
    End Function

    Private Function BuildAMTEXP(ByVal strAMTEXP As String) As String
        Dim v_strEvaluator, v_strElemenent, v_strValue, v_arrTemp() As String

        Try
            v_strEvaluator = vbNullString

            If Mid(strAMTEXP, 1, 1) = "@" Then
                v_strEvaluator = Mid(strAMTEXP, 2)
            Else
                v_arrTemp = strAMTEXP.Split("|")

                For i As Integer = 0 To v_arrTemp.Length - 1
                    v_strElemenent = v_arrTemp(i)

                    Select Case v_strElemenent
                        Case "++", "--", "**", "//", "((", "))"
                            'Operand
                            v_strEvaluator &= Mid(v_strElemenent, 1, 1)
                        Case Else
                            'Operator
                            v_strValue = CDbl(GetControlValueByName(v_strElemenent, Me)).ToString()
                            v_strEvaluator &= v_strValue
                    End Select
                Next
            End If

            Return v_strEvaluator

        Catch ex As Exception
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            '             & "Error code: System error!" & vbNewLine _
            '             & "Error message: " & ex.Message, EventLogEntryType.Error)
            'MsgBox(ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.OKOnly, gc_ApplicationTitle)
            Throw ex
        End Try
    End Function

    Public Sub FormatObjectFields(ByRef pv_ctrl As Windows.Forms.Control)
        Dim v_ctrl As Windows.Forms.Control = Nothing
        Try
            For Each v_ctrl In pv_ctrl.Controls
                If TypeOf (v_ctrl) Is TextBox Then
                    For i As Integer = 0 To UBound(mv_arrObjFields) - 1
                        With mv_arrObjFields(i)
                            If (v_ctrl.Tag = .FieldName) And (.FieldType = "T") Then
                                If .FieldLength > 0 Then
                                    CType(v_ctrl, TextBox).MaxLength = .FieldLength
                                End If
                                If .DataType = "N" Then
                                    CType(v_ctrl, TextBox).TextAlign = HorizontalAlignment.Right
                                    AddHandler CType(v_ctrl, TextBox).LostFocus, AddressOf NumericTextBox_LostFocus
                                Else
                                    CType(v_ctrl, TextBox).TextAlign = HorizontalAlignment.Left
                                End If

                                'Xử lí lookup
                                If (.LookUp = "Y") And (ExeFlag <> ExecuteFlag.View) Then
                                    'CType(v_ctrl, TextBox).BackColor = System.Drawing.Color.Khaki
                                    AddHandler CType(v_ctrl, TextBox).KeyUp, AddressOf TextBox_KeyUp
                                End If
                                If (Len(.SearchCode) > 0) And (ExeFlag <> ExecuteFlag.View) Then
                                    'CType(v_ctrl, TextBox).BackColor = System.Drawing.Color.GreenYellow
                                    AddHandler CType(v_ctrl, TextBox).KeyUp, AddressOf TextBox_KeyUp
                                End If

                                CType(v_ctrl, TextBox).Visible = .Visible
                                CType(v_ctrl, TextBox).Enabled = .Enabled

                                If ExeFlag = ExecuteFlag.View Then
                                    CType(v_ctrl, TextBox).ReadOnly = True
                                    CType(v_ctrl, TextBox).Enabled = False
                                End If

                                If ExeFlag = ExecuteFlag.AddNew Then
                                    If (.DefaultValue.Trim.Length > 0) Then
                                        CType(v_ctrl, TextBox).Text = .DefaultValue
                                    Else
                                        CType(v_ctrl, TextBox).Text = String.Empty
                                    End If
                                End If

                                Exit For
                            End If
                            If (v_ctrl.Tag = .FieldName) And (.FieldType = "M") Then
                                If .InputMask.Length > 0 Then
                                    CType(v_ctrl, FlexMaskEditBox).Mask = .InputMask
                                End If
                                If .FieldFormat.Length > 0 Then
                                    CType(v_ctrl, FlexMaskEditBox).PromptChar = .FieldFormat
                                End If
                                If .FieldLength > 0 Then
                                    CType(v_ctrl, FlexMaskEditBox).MaxLength = .FieldLength
                                End If
                                If .DataType = "N" Then
                                    CType(v_ctrl, FlexMaskEditBox).TextAlign = HorizontalAlignment.Right
                                    CType(v_ctrl, FlexMaskEditBox).FieldType = FlexMaskEditBox._FieldType.NUMERIC
                                Else
                                    CType(v_ctrl, FlexMaskEditBox).TextAlign = HorizontalAlignment.Left
                                    CType(v_ctrl, FlexMaskEditBox).FieldType = FlexMaskEditBox._FieldType.ALFA
                                End If
                                'Xử lí lookup
                                If (.LookUp = "Y") And (ExeFlag <> ExecuteFlag.View) Then
                                    'CType(v_ctrl, FlexMaskEditBox).BackColor = System.Drawing.Color.PeachPuff
                                    AddHandler CType(v_ctrl, FlexMaskEditBox).KeyUp, AddressOf TextBox_KeyUp
                                End If
                                If (Len(.SearchCode) > 0) And (ExeFlag <> ExecuteFlag.View) Then
                                    CType(v_ctrl, FlexMaskEditBox).BackColor = System.Drawing.Color.GreenYellow
                                    AddHandler CType(v_ctrl, FlexMaskEditBox).KeyUp, AddressOf TextBox_KeyUp
                                End If

                                CType(v_ctrl, FlexMaskEditBox).MaskCharInclude = False
                                CType(v_ctrl, FlexMaskEditBox).Visible = .Visible
                                CType(v_ctrl, FlexMaskEditBox).Enabled = .Enabled

                                If ExeFlag = ExecuteFlag.View Then
                                    CType(v_ctrl, FlexMaskEditBox).ReadOnly = True
                                    CType(v_ctrl, FlexMaskEditBox).Enabled = False
                                End If

                                If ExeFlag = ExecuteFlag.AddNew Then
                                    If (.DefaultValue.Trim.Length > 0) Then
                                        CType(v_ctrl, FlexMaskEditBox).Text = .DefaultValue
                                    Else
                                        CType(v_ctrl, FlexMaskEditBox).Text = String.Empty
                                    End If
                                End If

                                Exit For
                            End If

                        End With

                    Next
                    AddHandler CType(v_ctrl, TextBox).GotFocus, AddressOf TextBox_GotFocus
                ElseIf TypeOf (v_ctrl) Is ComboBoxEx Then
                    For i As Integer = 0 To UBound(mv_arrObjFields) - 1
                        With mv_arrObjFields(i)
                            If (v_ctrl.Tag = .FieldName) And (.FieldType = "C") And (.LookupList.Length > 0) Then
                                'Load from object reference data in object message
                                Dim v_strSQL As String
                                v_strSQL = mv_arrObjFields(i).LookupList
                                If .FieldName = "ISCODE" Then
                                    Dim v_strObjMsg As String
                                    v_strObjMsg = BuildXMLObjMsg(, BranchId, , TellerId, gc_IsLocalMsg, gc_MsgTypeObj, ObjectName, gc_ActionInquiry, v_strSQL)
                                    'mv_BDSDelivery.Message(v_strObjMsg)
                                    Proxy.Message(v_strObjMsg)
                                    Dim v_strFLDNAME, v_strVALUE As String
                                    Dim v_strCDVal, v_strCDContent As String
                                    Dim v_xmlDocument As New XmlDocumentEx
                                    Dim v_nodeList As Xml.XmlNodeList = Nothing
                                    v_xmlDocument.LoadXml(v_strObjMsg)
                                    v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")
                                    For v_i As Integer = 0 To v_nodeList.Count - 1
                                        For v_j As Integer = 0 To v_nodeList.Item(v_i).ChildNodes.Count - 1
                                            With v_nodeList.Item(v_i).ChildNodes(v_j)
                                                v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                                                v_strVALUE = .InnerText.ToString()
                                            End With
                                            Select Case Trim(v_strFLDNAME)
                                                Case "VALUE"
                                                    v_strCDVAL = Trim(v_strVALUE)
                                                Case "DISPLAY"
                                                    v_strCDCONTENT = Trim(v_strVALUE)
                                            End Select
                                        Next
                                        CType(v_ctrl, ComboBoxEx).AddItems(v_strCDContent, v_strCDVal)
                                    Next
                                Else
                                    Dim v_obj As New SQLEngine.SQLDataAccessLayer(TellerId)

                                    Dim v_ds As DataSet
                                    v_ds = v_obj.ExecuteReturnDataSet(v_strSQL)
                                    'FillComboExRefData(mv_strXMLFldMaster, CType(v_ctrl, ComboBoxEx), v_ctrl.Tag)
                                    CType(v_ctrl, ComboBoxEx).BeginUpdate()
                                    CType(v_ctrl, ComboBoxEx).DataSource = v_ds.Tables(0)
                                    CType(v_ctrl, ComboBoxEx).DisplayMember = "DISPLAY"
                                    CType(v_ctrl, ComboBoxEx).ValueMember = "VALUE"
                                    CType(v_ctrl, ComboBoxEx).EndUpdate()
                                    v_obj.CloseConnection()
                                    v_obj = Nothing
                                    v_ds.Dispose()
                                End If
                                If ExeFlag = ExecuteFlag.View Then
                                    CType(v_ctrl, ComboBoxEx).Enabled = False
                                End If
                                Exit For
                            End If
                        End With
                    Next
                ElseIf TypeOf (v_ctrl) Is FlexMaskEditBox Then
                    For i As Integer = 0 To UBound(mv_arrObjFields) - 1
                        With mv_arrObjFields(i)
                            If (v_ctrl.Tag = .FieldName) And (.FieldType = "M") Then
                                If .InputMask.Length > 0 Then
                                    CType(v_ctrl, FlexMaskEditBox).Mask = .InputMask
                                End If
                                If .FieldFormat.Length > 0 Then
                                    CType(v_ctrl, FlexMaskEditBox).PromptChar = .FieldFormat
                                End If
                                If .FieldLength > 0 Then
                                    CType(v_ctrl, FlexMaskEditBox).MaxLength = .FieldLength
                                End If
                                If .DataType = "N" Then
                                    CType(v_ctrl, FlexMaskEditBox).TextAlign = HorizontalAlignment.Right
                                Else
                                    CType(v_ctrl, FlexMaskEditBox).TextAlign = HorizontalAlignment.Left
                                End If
                                If ExeFlag = ExecuteFlag.View Then
                                    CType(v_ctrl, FlexMaskEditBox).ReadOnly = True
                                    CType(v_ctrl, FlexMaskEditBox).Enabled = False
                                End If
                                CType(v_ctrl, FlexMaskEditBox).Visible = .Visible
                                CType(v_ctrl, FlexMaskEditBox).Enabled = .Enabled

                                Exit For
                            End If
                        End With
                    Next
                ElseIf TypeOf (v_ctrl) Is DateTimePicker Then
                    For i As Integer = 0 To UBound(mv_arrObjFields) - 1
                        With mv_arrObjFields(i)
                            If (v_ctrl.Tag = .FieldName) And (.FieldType = "D") Then
                                'If .InputMask.Length > 0 Then

                                'End If
                                If .FieldFormat.Length > 0 Then
                                    CType(v_ctrl, DateTimePicker).CustomFormat = .FieldFormat
                                End If
                                'If .FieldLength > 0 Then

                                'End If
                                CType(v_ctrl, DateTimePicker).Visible = .Visible
                                CType(v_ctrl, DateTimePicker).Enabled = .Enabled
                                If ExeFlag = ExecuteFlag.View Then
                                    CType(v_ctrl, DateTimePicker).Enabled = False
                                End If
                                Exit For
                            End If
                        End With
                    Next
                ElseIf TypeOf (v_ctrl) Is GroupBox Then
                    FormatObjectFields(v_ctrl)
                ElseIf TypeOf (v_ctrl) Is TabControl Then
                    FormatObjectFields(v_ctrl)
                ElseIf TypeOf (v_ctrl) Is TabPage Then
                    FormatObjectFields(v_ctrl)
                ElseIf TypeOf (v_ctrl) Is Panel Then
                    FormatObjectFields(v_ctrl)
                End If
            Next
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub PrepareDataSet(ByRef pv_ds As DataSet)
        Dim v_dc As DataColumn

        Try
            If Not (pv_ds Is Nothing) Then
                pv_ds.Dispose()
            End If

            pv_ds = New DataSet("INPUT")
            pv_ds.Tables.Add(TableName)

            For i As Integer = 0 To UBound(mv_arrObjFields) - 1
                v_dc = New DataColumn(mv_arrObjFields(i).FieldName)
                Select Case mv_arrObjFields(i).DataType
                    Case "C"
                        v_dc.DataType = GetType(String)
                    Case "D"
                        v_dc.DataType = GetType(System.DateTime)
                    Case "N"
                        v_dc.DataType = GetType(Double)
                    Case Else
                        v_dc.DataType = GetType(String)
                End Select

                pv_ds.Tables(0).Columns.Add(v_dc)
            Next
        Catch ex As Exception
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            '             & "Error code: System error!" & vbNewLine _
            '             & "Error message: " & ex.Message, EventLogEntryType.Error)
            'MsgBox(ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.OKOnly, gc_ApplicationTitle)
            Throw ex
        End Try
    End Sub

    Private Sub FillLookupData(ByVal pv_strFLDNAME As String, ByVal pv_strVALUE As String, ByVal pv_strFULLDATA As String)
        Try
            Dim v_xmlDocument As New Xml.XmlDocument
            Dim v_nodeList As Xml.XmlNodeList
            Dim v_intNodeIndex As Integer
            Dim v_strLookupField As String

            v_xmlDocument.LoadXml(pv_strFULLDATA)
            Dim v_intCount As Integer = mv_arrObjFields.GetLength(0)

            If v_intCount > 0 Then
                'Xác định Node chứa dữ liệu
                v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")

                For i As Integer = 0 To v_nodeList.Count - 1
                    For j As Integer = 0 To v_nodeList.Item(i).ChildNodes.Count - 1
                        With v_nodeList.Item(i).ChildNodes(j)
                            If "VALUE" = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value) _
                                And pv_strVALUE = Trim(.InnerText.ToString) Then
                                v_intNodeIndex = i
                                Exit For
                            End If
                        End With
                    Next
                Next

                'Nạp dữ liệu Lookup cho các control có khai báo
                For i As Integer = 0 To v_intCount - 1 Step 1
                    If Not (mv_arrObjFields(i) Is Nothing) Then
                        If Trim(mv_arrObjFields(i).LookupName).Length > 0 Then
                            'Nếu có tham số lấy giá trị
                            Dim v_arrLookupName() As String = mv_arrObjFields(i).LookupName.Split("|")
                            If (v_arrLookupName.Length = 2) And (v_arrLookupName(0) = pv_strFLDNAME) Then
                                v_strLookupField = v_arrLookupName(1)
                                For j As Integer = 0 To v_nodeList.Item(v_intNodeIndex).ChildNodes.Count - 1
                                    With v_nodeList.Item(v_intNodeIndex).ChildNodes(j)
                                        If v_strLookupField = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value) Then
                                            'Gán giá trị cho contol tương ứng
                                            SetControlValue(Me, mv_arrObjFields(i).FieldName, mv_arrObjFields(i).FieldType, .InnerText.ToString().Trim(), mv_arrObjFields(i).DataType)
                                        End If
                                    End With
                                Next
                            End If
                        End If
                    End If
                Next
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub TextBox_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            If (TypeOf (sender) Is FlexMaskEditBox) Then
                CType(sender, FlexMaskEditBox).SelectionStart = 0
                CType(sender, FlexMaskEditBox).SelectionLength = CType(sender, FlexMaskEditBox).Mask.Length()
            ElseIf (TypeOf (sender) Is TextBox) Then
                CType(sender, TextBox).SelectionStart = 0
                CType(sender, TextBox).SelectionLength = CType(sender, TextBox).Text.Length()
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub FormatNumericTextbox(ByVal pv_ctrl As TextBox)
        Try
            Dim v_strFormat As String = ""
            Dim v_intDecimal As String

            For i As Integer = 0 To mv_arrObjFields.Length - 1
                If (mv_arrObjFields(i).FieldName = pv_ctrl.Tag) Then
                    v_strFormat = mv_arrObjFields(i).FieldFormat
                    Exit For
                End If
            Next

            If (v_strFormat.Length > 0) Then
                If (v_strFormat.IndexOf(".") <> -1) Then
                    v_intDecimal = Mid(v_strFormat, v_strFormat.IndexOf(".") + 2).Length()
                Else
                    v_intDecimal = 0
                End If
            Else
                v_intDecimal = 0
            End If

            If IsNumeric(pv_ctrl.Text) Then
                pv_ctrl.Text = FormatNumber(pv_ctrl.Text, v_intDecimal)
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub SetControlFocus(ByVal pv_ctrl As Control, ByVal pv_strFLDNAME As String)
        Dim v_ctrl As Control

        Try
            For Each v_ctrl In pv_ctrl.Controls
                If (v_ctrl.Enabled And v_ctrl.Visible) Then
                    If TypeOf (v_ctrl) Is FlexMaskEditBox Then
                        If (v_ctrl.Tag = Trim(pv_strFLDNAME)) Then
                            CType(v_ctrl, FlexMaskEditBox).Focus()
                            Exit For
                        End If
                    ElseIf TypeOf (v_ctrl) Is TextBox Then
                        If (v_ctrl.Tag = Trim(pv_strFLDNAME)) Then
                            CType(v_ctrl, TextBox).Focus()
                            Exit For
                        End If
                    ElseIf TypeOf (v_ctrl) Is DateTimePicker Then
                        If (v_ctrl.Tag = Trim(pv_strFLDNAME)) Then
                            CType(v_ctrl, DateTimePicker).Focus()
                            Exit For
                        End If
                    ElseIf TypeOf (v_ctrl) Is ComboBoxEx Then
                        If (v_ctrl.Tag = Trim(pv_strFLDNAME)) Then
                            CType(v_ctrl, ComboBoxEx).Focus()
                            Exit For
                        End If
                    ElseIf TypeOf (v_ctrl) Is PictureBox Then
                        If (v_ctrl.Tag = Trim(pv_strFLDNAME)) Then
                            CType(v_ctrl, PictureBox).Focus()
                            Exit For
                        End If
                    ElseIf TypeOf (v_ctrl) Is GroupBox Then
                        SetControlFocus(v_ctrl, pv_strFLDNAME)
                    ElseIf TypeOf (v_ctrl) Is TabControl Then
                        SetControlFocus(v_ctrl, pv_strFLDNAME)
                    ElseIf TypeOf (v_ctrl) Is TabPage Then
                        SetControlFocus(v_ctrl, pv_strFLDNAME)
                    ElseIf TypeOf (v_ctrl) Is Panel Then
                        SetControlFocus(v_ctrl, pv_strFLDNAME)
                    End If
                End If
            Next
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Function GetFieldDataType(ByVal pv_strFLDNAME As String) As String
        Try
            Dim v_strDataType As String = String.Empty

            For i As Integer = 0 To mv_arrObjFields.Length - 1
                If (mv_arrObjFields(i).FieldName = pv_strFLDNAME) Then
                    v_strDataType = mv_arrObjFields(i).DataType
                    Exit For
                End If
            Next

            Return v_strDataType
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function BuildDATEEXP(ByVal strDATEEXP As String) As Date
        Dim v_dtmRetVal As Date

        Try
            If Mid(strDATEEXP, 1, 1) = "@" Then
                v_dtmRetVal = DDMMYYYY_SystemDate(Mid(strDATEEXP, 2))
            Else
                v_dtmRetVal = DDMMYYYY_SystemDate(GetControlValueByName(strDATEEXP, Me))
            End If

            Return v_dtmRetVal
        Catch ex As Exception
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            '             & "Error code: System error!" & vbNewLine _
            '             & "Error message: " & ex.Message, EventLogEntryType.Error)
            'MsgBox(ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.OKOnly, gc_ApplicationTitle)
            Throw ex
        End Try
    End Function
#End Region

#Region " Form events "
    Private Sub frmMaintain_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'mv_BDSDelivery = New BDSChannel.BDSDelivery
        'OnInit()
        Try
            FormatObjectFields(Me)

            If ExeFlag <> ExecuteFlag.AddNew Then
                DoDataExchange()
            End If
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                                      & "Error code: System error!" & vbNewLine _
                                                      & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Sub

    Private Sub Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            If (sender Is btnOK) Then
                mv_saveButtonType = SaveButtonType.OKButton
                OnSave()
            ElseIf (sender Is btnApply) Then
                mv_saveButtonType = SaveButtonType.ApplyButton
                OnSave()
                SearchForm.mv_enuEditFormResult = mv_saveButtonType
            ElseIf (sender Is btnCancel) Then
                OnClose()
            End If
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                                     & "Error code: System error!" & vbNewLine _
                                                     & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Sub

    Private Sub frmMaintenance_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        Try
            Select Case e.KeyCode
                Case Keys.Enter
                    If (Me.ActiveControl Is btnOK) Then
                        OnSave()
                    Else
                        If TypeOf (Me.ActiveControl) Is TextBox Or TypeOf (Me.ActiveControl) Is ComboBox _
                            Or TypeOf (Me.ActiveControl) Is FlexMaskEditBox Or TypeOf (Me.ActiveControl) Is DateTimePicker Then
                            SendKeys.Send("{Tab}")
                            e.Handled = True
                        End If
                    End If
                Case Keys.Escape
                    OnClose()
            End Select
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                                     & "Error code: System error!" & vbNewLine _
                                                     & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Sub

    Private Sub TextBox_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        Dim v_strFLDNAME, v_strLookupSQL As String
        Dim v_intPos As Integer, v_intIndex As Integer
        Try
            Select Case e.KeyCode
                Case Keys.F5
                    'v_strFLDNAME = CType(sender, FlexMaskEditBox).Tag
                    v_strFLDNAME = CType(sender, Control).Tag
                    For i As Integer = 0 To UBound(mv_arrObjFields) - 1
                        If (mv_arrObjFields(i).FieldName = v_strFLDNAME) Then
                            v_intIndex = i
                            Exit For
                        End If
                    Next
                    If Len(mv_arrObjFields(v_intIndex).SearchCode) > 0 Then
                        Dim frm As New frmSearch(Me.UserLanguage)
                        frm.TableName = mv_arrObjFields(v_intIndex).SearchCode
                        frm.ModuleCode = mv_arrObjFields(v_intIndex).SrModCode
                        frm.AuthCode = "NNNNYYNNNN" 'Không cho phép thực hiện chức năng nào 
                        frm.IsLocalSearch = gc_IsLocalMsg
                        frm.IsLookup = "Y"
                        frm.SearchOnInit = False
                        frm.BranchId = Me.BranchId
                        frm.TellerId = Me.TellerId
                        frm.ShowDialog()
                        Me.ActiveControl.Text = frm.ReturnValue
                        frm.Dispose()
                    ElseIf mv_arrObjFields(v_intIndex).LookUp = "Y" Then

                        v_strLookupSQL = mv_arrObjFields(v_intIndex).LookupList
                        'Nếu là CFMAST

                        If Me.TableName = "CFMAST" Then
                            v_strLookupSQL = "SELECT TLID VALUE,TLID VALUECD,TLNAME DISPLAY,TLNAME EN_DISPLAY, TLNAME DESCRIPTION, TLNAME FROM tlprofiles where BRID='" & Me.BranchId & "'"
                        End If
                        Dim v_frm As New AppCore.frmLookUp(UserLanguage)
                        v_strLookupSQL = Replace(v_strLookupSQL, "<$BRID>", Me.BranchId)
                        v_strLookupSQL = Replace(v_strLookupSQL, "<$TLID>", Me.TellerId)
                        v_strLookupSQL = Replace(v_strLookupSQL, "<$BUSDATE>", Me.BusDate)
                        v_frm.SQLCMD = v_strLookupSQL
                        v_frm.ShowDialog()
                        v_intPos = InStr(v_frm.RETURNDATA, vbTab)
                        If v_intPos > 0 Then
                            CType(sender, FlexMaskEditBox).Text = Mid(v_frm.RETURNDATA, 1, v_intPos - 1)
                            'Nạp các giá trị tương ứng cho các trường khác
                            FillLookupData(v_strFLDNAME, Mid(v_frm.RETURNDATA, 1, v_intPos - 1), v_frm.FULLDATA)
                        End If
                        v_frm.Dispose()
                    End If
            End Select
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                                     & "Error code: System error!" & vbNewLine _
                                                     & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Sub
    Private Sub NumericTextBox_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            FormatNumericTextbox(CType(sender, TextBox))
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region
    
    
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
End Class