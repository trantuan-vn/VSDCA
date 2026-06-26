Imports Sats.CommonLibrary
Public Class frmSelectVsdBrid
    Private mv_strVsdBrid As String
    Private mv_strBRID As String
    Private mv_strSELECTED As String = ""
    Private mv_strRETURN As String = ""
    'Private mv_BDSDelivery As New BDSChannel.BDSDelivery
    Private mv_ResourceManager As Resources.ResourceManager
    Private mv_strTellerID As String
    Private mv_strUserLanguage As String
    Private mv_oProxy As BDSChannel.BDSDelivery

    Public Property Proxy() As BDSChannel.BDSDelivery
        Get
            Return mv_oProxy
        End Get
        Set(ByVal value As BDSChannel.BDSDelivery)
            mv_oProxy = value
        End Set
    End Property

    Public Property BRID() As String
        Get
            Return mv_strBRID
        End Get
        Set(ByVal Value As String)
            mv_strBRID = Value
        End Set
    End Property

    Public Property VSDBRID() As String
        Get
            Return mv_strVsdBrid
        End Get
        Set(ByVal Value As String)
            mv_strVsdBrid = Value
        End Set
    End Property
    Public Property SELECTED() As String
        Get
            Return mv_strSELECTED
        End Get
        Set(ByVal Value As String)
            mv_strSELECTED = Value
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

    Public Property UserLanguage() As String
        Get
            Return mv_strUserLanguage
        End Get
        Set(ByVal value As String)
            mv_strUserLanguage = value
        End Set
    End Property

    Private Sub frmSelectTLTX_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'OnInit()
    End Sub
    Public Function OnInit() As Boolean
        Return FillDataCombobox()
    End Function
    Private Function FillDataCombobox() As Boolean
        Try
            Dim v_xmlDocument As New Xml.XmlDocument
            Dim v_nodeList As Xml.XmlNodeList = Nothing
            Dim v_strValue As String = "", v_strFLDNAME As String = ""
            Dim v_strCDVAL As String = "", v_strCDCONTENT As String = ""

            Dim v_strObjMsg As String
            Dim strRow As String = "Select cdval value, cdval || ' - ' || cdcontent display from allcode where cdtype = 'SA' and cdname = 'VSD_BRID' and cdvalno < 3"
            v_strObjMsg = BuildXMLObjMsg(, , , TellerID, gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SA_TLGROUPS, gc_ActionInquiry, strRow)
            Dim v_lngError As Long = Proxy.Message(v_strObjMsg)
            If v_lngError <> ERR_SYSTEM_OK Then
                MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
                Return False
            End If

            v_xmlDocument.LoadXml(v_strObjMsg)
            v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")

            For i As Integer = 0 To v_nodeList.Count - 1
                For j As Integer = 0 To v_nodeList.Item(i).ChildNodes.Count - 1
                    With v_nodeList.Item(i).ChildNodes(j)
                        v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                        v_strValue = .InnerText.ToString()
                    End With
                    Select Case Trim(v_strFLDNAME)
                        Case "VALUE"
                            v_strCDVAL = Trim(v_strValue)
                        Case "DISPLAY"
                            v_strCDCONTENT = Trim(v_strValue)
                    End Select
                Next
                cboVsdBrid.AddItems(v_strCDCONTENT, v_strCDVAL)
            Next
            If v_nodeList.Count = 1 Then
                SELECTED = v_strCDVAL
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                                     & "Error code: System error!" & vbNewLine _
                                                     & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            MsgBox(IIf(UserLanguage = gc_LANG_VIETNAMESE, gc_ERR_MSG_VN, gc_ERR_MSG_EN), MsgBoxStyle.Critical, gc_ApplicationTitle)
        End Try
    End Function

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        SELECTED = ""
        Me.Close()
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        SELECTED = cboVsdBrid.SelectedValue
        Me.Close()
    End Sub
End Class