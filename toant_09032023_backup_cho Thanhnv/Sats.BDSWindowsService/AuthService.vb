Imports System.DirectoryServices
Imports Sats.CommonLibrary
Imports Sats.ActiveDirectory

Public Class AuthService
    Inherits MarshalByRefObject
    Implements IAuthServiceLib

    Public Function GetAuthorizationTicket(ByVal pv_strUserName As String, ByVal pv_strPassword As String) As String Implements CommonLibrary.IAuthServiceLib.GetAuthorizationTicket
        'Dim v_strUserId As String
        '' Dim v_obj As New Sats.Branch.Branch


        'Dim v_strAuthType As String = Configuration.ConfigurationManager.AppSettings("AuthType").ToString
        'Dim v_eLoginResult As Utility.LoginResult = Utility.LoginResult.LOGIN_OK

        'If v_strAuthType = gc_AUTHORIZATION_MODE_LDAP Then
        '    v_eLoginResult = ADManager.Login(pv_strUserName, DecryptString(pv_strUserName, pv_strPassword))
        'End If

        'If v_eLoginResult = Utility.LoginResult.LOGIN_OK Then
        '    Try
        '        Dim v_ws As New HOSTChannel.HOSTDelivery
        '        'v_strUserId = v_obj.GetAuthorizationTicket(pv_strUserName)
        '        v_strUserId = v_ws.GetAuthorizationTicket(pv_strUserName, pv_strPassword)
        '        'Kiem tra user da dc dang nhap hay chua
        '        'If v_obj.CheckUserInUse(v_strUserId.Split("|")(1)) Then
        '        '    v_obj.Dispose()
        '        '    Return "UserInUse"
        '        'End If
        '    Catch ex As Exception
        '        Throw ex
        '        Return Nothing
        '    End Try

        '    'If Not (v_strUserId Is Nothing) Then
        '    '    'v_strUserId &= "|" & DataProtection.UnprotectData(pv_strPassword)
        '    '    v_strUserId &= "|" & pv_strPassword
        '    'End If

        '    'If v_strUserId Is Nothing Then
        '    '    'NSD hoặc mật khẩu không đúng
        '    '    Return Nothing
        '    'End If


        '    'create the ticket
        '    'Dim ticket As New FormsAuthenticationTicket(v_strUserId, False, 1)
        '    'Dim encryptedTicket As String = FormsAuthentication.Encrypt(ticket)

        '    'get the ticket timeout in minutes
        '    'Dim timeout As Integer = CInt(configurationAppSettings.GetValue("AuthenticationTicket.Timeout", GetType(Integer)))
        '    'v_obj.Dispose()
        '    Return v_strUserId
        'Else
        '    Return Nothing
        'End If
    End Function

    Public Function GetTellerProfile(ByVal ticket As String) As CommonLibrary.CTellerProfile Implements CommonLibrary.IAuthServiceLib.GetTellerProfile
        'Dim v_strBranchId As String = String.Empty
        'Dim v_strTellerId As String = String.Empty

        ''Dim v_str As String = FormsAuthentication.Decrypt(ticket).Name
        'Dim v_strArray() As String = ticket.Split("|") 'v_str.Split("|")

        'If v_strArray.Length = 3 Then
        '    v_strBranchId = v_strArray(0)
        '    v_strTellerId = v_strArray(1)
        'End If

        ''Lấy thông tin người sử dụng
        'Dim v_obj As New Sats.Branch.Branch
        'Dim tlProfile As CTellerProfile = v_obj.GetTellerProfile(v_strBranchId, v_strTellerId)
        'v_obj.Dispose()
        'Return tlProfile
        Dim v_ws As New HOSTChannel.HOSTDelivery
        Return v_ws.GetTellerProfile(ticket)
    End Function

    Private Function objMessage(ByRef pv_strObjMessage As String) As Long
        Dim v_obj As New Sats.Branch.Branch
        Try
            Return v_obj.objTransfer(pv_strObjMessage)
        Catch ex As Exception
            Throw ex
        Finally
            v_obj.Dispose()
        End Try
    End Function

    Private Function txMessage(ByRef pv_strTxMessage As String) As Long
        Return 0
    End Function

    Private Function GetErrorMessage(ByVal pv_lngErrorCode As Long) As String
        Dim v_strErrorMessage As String = String.Empty
        Dim v_obj As New Sats.Branch.Branch
        Dim v_lngError As Long

        Try
            Dim v_strClause As String = " ERRNUM = " & pv_lngErrorCode.ToString()
            Dim v_strObjMsg As String = BuildXMLObjMsg(, , , , gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SY_DEFERROR, gc_ActionInquiry, , v_strClause)

            v_lngError = v_obj.objTransfer(v_strObjMsg)

            Dim v_xmlDocument As New Xml.XmlDocument
            Dim v_nodeList As Xml.XmlNodeList
            Dim v_strValue, v_strFLDNAME As String

            'Read object message 
            v_xmlDocument.LoadXml(v_strObjMsg)
            v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")
            If v_nodeList.Count = 1 Then
                For i As Integer = 0 To v_nodeList.Count - 1
                    For j As Integer = 0 To v_nodeList.Item(i).ChildNodes.Count - 1
                        With v_nodeList.Item(i).ChildNodes(j)
                            v_strValue = .InnerText.ToString
                            v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)

                            If Trim(v_strFLDNAME) = "ERRDESC" Then
                                v_strErrorMessage = v_strValue
                            End If
                        End With
                    Next
                Next
            Else
                v_strErrorMessage = "Undefined error!"
            End If

            Return v_strErrorMessage
        Catch ex As Exception
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            '             & "Error code: System error!" & vbNewLine _
            '             & "Error message: " & ex.Message, EventLogEntryType.Error)
            Throw ex
        Finally
            v_obj.Dispose()
        End Try
    End Function

    Public Function ChangePassword(ByVal pv_strUserName As String, ByVal pv_strNewPassword As String) As Long Implements CommonLibrary.IAuthServiceLib.ChangePassword
        Try
            Dim v_oUser As New ADUser
            v_oUser = ADManager.Instance.LoadUser(pv_strUserName)
            v_oUser.SetPassword(DecryptString(pv_strUserName, pv_strNewPassword))
            Return ERR_SYSTEM_OK
        Catch ex As Exception
            Return -1
        End Try
    End Function

    Public Function GetPublicKey(ByVal pv_strUserName As String, ByRef pv_arrBytePublicKey() As Byte) As Long Implements CommonLibrary.IAuthServiceLib.GetPublicKey
        Try
            Dim v_arrBytePrivateKey() As Byte
            Dim v_arrBytePublicKey() As Byte

            GetKey(pv_strUserName, v_arrBytePublicKey, v_arrBytePrivateKey)
            If Not hPublicKey(pv_strUserName) Is Nothing Then
                hPublicKey(pv_strUserName) = pv_arrBytePublicKey
                hPrivateKey(pv_strUserName) = v_arrBytePrivateKey
            Else
                hPublicKey.Add(pv_strUserName, pv_arrBytePublicKey)
                hPrivateKey.Add(pv_strUserName, v_arrBytePrivateKey)
            End If

            pv_arrBytePublicKey = v_arrBytePublicKey

            Return ERR_SYSTEM_OK
        Catch ex As Exception
            LogError.Write(ex.Message, EventLogEntryType.Error, "BDS_SERVICE")
            Return ERR_SYSTEM_START
        End Try
    End Function

    Public Function Message(ByVal pv_strTellerID As String, ByRef pv_arrByteMessage() As Byte) As Long Implements CommonLibrary.IAuthServiceLib.Message
        Dim v_strMessage As String
        Dim v_arrBytePrivateKey() As Byte
        Dim v_arrBytePublicKey() As Byte
        Try
            Dim XMLDocumentMessage As New Xml.XmlDocument
            Dim v_lngErr As Long = ERR_SYSTEM_OK

            If hPrivateKey(pv_strTellerID) Is Nothing Then
                Return ERR_SYSTEM_START
            End If

            v_arrBytePrivateKey = hPrivateKey(pv_strTellerID)
            pv_arrByteMessage = DecryptMessage(v_arrBytePrivateKey, pv_arrByteMessage)
            v_strMessage = Decompress(pv_arrByteMessage)
            'Read transaction message 
            XMLDocumentMessage.LoadXml(v_strMessage)

            'Get message's header
            Dim v_attrColl As Xml.XmlAttributeCollection = XMLDocumentMessage.DocumentElement.Attributes
            Dim v_strLOCAL As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeLOCAL), Xml.XmlAttribute).Value)
            Dim v_strMSGTYPE As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeMSGTYPE), Xml.XmlAttribute).Value)

            'If v_strLOCAL = gc_IsLocalMsg Then
            '    Select Case v_strMSGTYPE
            '        Case gc_MsgTypeTrans
            '            'Transaction message
            '            v_lngErr = txMessage(v_strMessage)
            '        Case gc_MsgTypeObj
            '            'Object message
            '            v_lngErr = objMessage(v_strMessage)
            '    End Select
            'ElseIf v_strLOCAL = gc_IsLocalMsg Then
            '    'Call Host WebService 
            'End If
            Dim v_obj As New Sats.Branch.Branch
            If v_strMSGTYPE = gc_MsgTypeTrans Then
                v_lngErr = v_obj.txTransfer(v_strMessage)
            Else
                'If v_strLOCAL = "Y" Or v_strLOCAL = "IY" Then
                '    v_lngErr = v_obj.objTransfer(v_strMessage)
                'ElseIf v_strLOCAL = "N" Or v_strLOCAL = "IN" Then
                v_lngErr = v_obj.SendMessage2Host(v_strMessage)
                'End If
            End If

            v_arrBytePublicKey = hPublicKey(pv_strTellerID)
            pv_arrByteMessage = Compression(v_strMessage)
            pv_arrByteMessage = EncryptMessage(v_arrBytePublicKey, pv_arrByteMessage)

            hPublicKey.Remove(pv_strTellerID)
            hPrivateKey.Remove(pv_strTellerID)

            Return v_lngErr
        Catch ex As Exception
            LogError.Write(ex.Message, EventLogEntryType.Error, "gc_MODULE_BDS")
            v_arrBytePublicKey = hPublicKey(pv_strTellerID)
            pv_arrByteMessage = Compression(v_strMessage)
            pv_arrByteMessage = EncryptMessage(v_arrBytePublicKey, pv_arrByteMessage)

            hPublicKey.Remove(pv_strTellerID)
            hPrivateKey.Remove(pv_strTellerID)
            Return ERR_SYSTEM_START
        Finally
            GC.Collect()
            GC.GetTotalMemory(False)
            GC.Collect()
        End Try
    End Function


#Region "WCF"
    Public Function Login(ByVal pv_strUserName As String, ByVal pv_strPassword As String, ByVal pv_strIPAddress As String) As LoginStatus
        Dim v_strAuthType As String = Configuration.ConfigurationManager.AppSettings("AuthType").ToString
        Dim v_eLoginResult As Utility.LoginResult = Utility.LoginResult.LOGIN_OK

        If v_strAuthType = gc_AUTHORIZATION_MODE_LDAP Then
            v_eLoginResult = ADManager.Login(pv_strUserName, DecryptString(pv_strUserName, pv_strPassword), pv_strIPAddress)
        End If

        If v_eLoginResult = Utility.LoginResult.LOGIN_OK Then
            Try
                Dim v_ws As New HOSTChannel.HostWCFChannel
                If v_strAuthType = gc_AUTHORIZATION_MODE_LDAP Then
                    Return v_ws.Login(pv_strUserName, "", "")
                Else
                    Return v_ws.Login(pv_strUserName, pv_strPassword, pv_strIPAddress)
                End If

                v_ws.Close()
            Catch ex As Exception
                Throw ex
                Return LoginStatus.Failure
            End Try
        Else
            Return LoginStatus.Failure
        End If
    End Function
#End Region
End Class
