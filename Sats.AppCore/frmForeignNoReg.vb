
Imports Xceed.SmartUI.Controls.TreeView
Imports Sats.CommonLibrary
Imports System
Imports System.Windows.Forms


Public Class frmForeignNoReg
    Private mv_strTellerId As String
    Private mv_intStart As Integer
    Private mv_strIaType As String
    Private mv_strCardNo As String
    Private mv_strCardDate As String
    Private mv_strReturnValue As String
    Private mv_ResourceManager As Resources.ResourceManager
    Private mv_strLanguage As String
    Private mv_oProxy As BDSChannel.BDSDelivery

    Public Property Proxy() As BDSChannel.BDSDelivery
        Get
            Return mv_oProxy
        End Get
        Set(ByVal value As BDSChannel.BDSDelivery)
            mv_oProxy = value
        End Set
    End Property

    Public Property TellerId() As String
        Get
            Return mv_strTellerId
        End Get
        Set(ByVal value As String)
            mv_strTellerId = value
        End Set
    End Property

    Public Property IaType() As String
        Get
            Return mv_strIaType
        End Get
        Set(ByVal value As String)
            mv_strIaType = value
        End Set
    End Property
    Public Property CardNo() As String
        Get
            Return mv_strCardNo
        End Get
        Set(ByVal value As String)
            mv_strCardNo = value
        End Set
    End Property
    Public Property CardDate() As String
        Get
            Return mv_strCardDate
        End Get
        Set(ByVal value As String)
            mv_strCardDate = value
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

    Private Sub OnInit()
        mv_ResourceManager = New Resources.ResourceManager("Sats.AppCore.frmForeignNoReg_" & UserLanguage, System.Reflection.Assembly.GetExecutingAssembly())
        LoadUserInterface(Me)
        Me.Text = mv_ResourceManager.GetString(Me.Name)

    End Sub
    Private Sub frmFunction_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        OnInit()
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

    Private Function GenerateForeignNo(ByVal pv_strCardNo As String, ByVal pv_strCardDate As String, ByVal pv_strIaType As String) As String
        Dim v_strForeignNo As String = ""
        Try
            'Dim v_oBds As New BDSChannel.BDSDelivery
            Dim v_strSQL, v_strObjMsg As String
            Dim v_nodeList As Xml.XmlNodeList
            Dim XmlDocument As New Xml.XmlDocument
            Dim v_strFLDNAME, v_strValue As String

            v_strSQL = "select generate_foreign_no('" & pv_strCardNo & "', '" & pv_strCardDate & "', '" & pv_strIaType & "') FOREIGNNO from dual "
            v_strObjMsg = BuildXMLObjMsg(, , , TellerId, gc_IsNotLocalMsg, gc_MsgTypeObj, OBJNAME_RG_RGII, gc_ActionAdhoc, v_strSQL, , "Get_Foreign_No")
            Proxy.Message(v_strObjMsg)

            XmlDocument.LoadXml(v_strObjMsg)
            v_nodeList = XmlDocument.SelectNodes("/ObjectMessage/ObjData")
            For i As Integer = 0 To v_nodeList.Item(0).ChildNodes.Count - 1
                With v_nodeList.Item(0).ChildNodes(i)
                    v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                    v_strValue = .InnerText.ToString()

                    If v_strFLDNAME = "FOREIGNNO" Then
                        v_strForeignNo = Trim(v_strValue)
                    End If
                End With
            Next
            Return v_strForeignNo
        Catch ex As Exception

        End Try
    End Function
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        mv_strReturnValue = ""
        Me.Close()
    End Sub

    Private Sub btnGenerate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenerate.Click
        If IaType <> "" Then
            If IaType <> "4" And IaType <> "6" Then
                MsgBox(mv_ResourceManager.GetString("IaTypeNotCorrect"), MsgBoxStyle.Exclamation, gc_ApplicationTitle)
                Me.Close()
            Else
                txtForeignNo.Text = GenerateForeignNo(CardNo, CardDate, IaType)
            End If
        Else
            txtForeignNo.Text = GenerateForeignNo(CardNo, CardDate, IaType)
        End If
    End Sub

    Private Function CheckExistForeignNo(ByVal pv_strForeignNo As String) As Boolean
        Dim v_blnCheck As Boolean = False
        Try
            'Dim v_oBds As New BDSChannel.BDSDelivery
            Dim v_strSQL, v_strObjMsg As String
            Dim v_nodeList As Xml.XmlNodeList
            Dim XmlDocument As New Xml.XmlDocument
            Dim v_strFLDNAME, v_strValue As String
            Dim v_intCount As Integer = 0

            v_strSQL = "select (a.count + b.count) count from (select count(*) count from rgii where foreignno = '" & pv_strForeignNo & "' and status = 0 and deleted = 0) a, " _
                        & " (select count(*) count from tllog where col_value10 = '" & pv_strForeignNo & "' and tltxcd = '2016' and status < 3 and deleted = 0) b"
            v_strObjMsg = BuildXMLObjMsg(, , , , gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_RG_RGII, gc_ActionInquiry, v_strSQL)
            Proxy.Message(v_strObjMsg)

            XmlDocument.LoadXml(v_strObjMsg)
            v_nodeList = XmlDocument.SelectNodes("/ObjectMessage/ObjData")
            For i As Integer = 0 To v_nodeList.Item(0).ChildNodes.Count - 1
                With v_nodeList.Item(0).ChildNodes(i)
                    v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                    v_strValue = .InnerText.ToString()

                    If v_strFLDNAME = "COUNT" Then
                        v_intCount = CInt(Trim(v_strValue))
                    End If
                End With
            Next
            If v_intCount > 0 Then
                v_blnCheck = True
            End If
            Return v_blnCheck
        Catch ex As Exception

        End Try
    End Function

    Private Function CheckForeignNoFormat(ByVal pv_strForeignNo As String) As Boolean
        Dim v_blnCheck As Boolean = False
        Dim v_chrChar As Char
        Dim v_strNumber As String
        Try
            If pv_strForeignNo.Length <> 6 Then
                v_blnCheck = True
            Else
                v_chrChar = pv_strForeignNo.Substring(0, 1)
                If v_chrChar <> "I" And v_chrChar <> "C" Then
                    v_blnCheck = True
                Else
                    v_strNumber = pv_strForeignNo.Substring(2, 4)
                    If Not IsNumeric(v_strNumber) Then
                        v_blnCheck = True
                    End If
                End If
            End If
            Return v_blnCheck
        Catch ex As Exception

        End Try
    End Function

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        If txtForeignNo.Text = "" Then
            MsgBox(mv_ResourceManager.GetString("ForeignNoEmpty"), MsgBoxStyle.Exclamation, gc_ApplicationTitle)
            Exit Sub
        Else
            If CheckForeignNoFormat(Trim(txtForeignNo.Text)) Then
                MsgBox(mv_ResourceManager.GetString("ForeignNoFormatError"), MsgBoxStyle.Exclamation, gc_ApplicationTitle)
                Exit Sub
            Else
                If Not CheckExistForeignNo(txtForeignNo.Text) Then
                    mv_strReturnValue = Trim(txtForeignNo.Text)
                Else
                    MsgBox(mv_ResourceManager.GetString("ForeignNoDuplicated"), MsgBoxStyle.Exclamation, gc_ApplicationTitle)
                    Exit Sub
                End If
            End If
        End If
        Me.Close()
    End Sub
End Class