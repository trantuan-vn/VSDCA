Imports System
Imports Sats.CommonLibrary

Public Class BDSDelivery
    Inherits MarshalByRefObject
    Implements IBDSDelivery

    Public Function GetErrorMessage(ByVal pv_lngErrorCode As Long, ByVal pv_strLanguage As String) As String Implements CommonLibrary.IBDSDelivery.GetErrorMessage
        Dim v_strErrorMessage As String
        Dim v_obj As New Sats.Branch.Branch
        Dim v_lngError As Long

        Try
            Dim v_strMessage As String = String.Empty
            Dim v_strEnMessage As String = String.Empty
            Dim v_strClause As String = " ERRNUM = " & pv_lngErrorCode.ToString()
            Dim v_strObjMsg As String = BuildXMLObjMsg(, , , , gc_IsLocalMsg, gc_MsgTypeObj, OBJNAME_SY_DEFERROR, gc_ActionInquiry, , v_strClause)

            v_lngError = v_obj.objTransfer(v_strObjMsg)

            Dim v_xmlDocument As New XmlDocumentEx
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
                                v_strMessage = v_strValue
                            End If
                            If Trim(v_strFLDNAME) = "EN_ERRDESC" Then
                                v_strEnMessage = v_strValue
                            End If
                        End With
                    Next
                Next
                v_strErrorMessage = IIf(pv_strLanguage = "VN", v_strMessage, v_strEnMessage)
            Else
                v_strErrorMessage = "[" & pv_lngErrorCode.ToString() & IIf(pv_strLanguage = "VN", "]: Lỗi không xác định, vui lòng liên hệ với FPT ", "]: Undefined error, please contact FPT!")
            End If
            v_obj.Dispose()
            Return v_strErrorMessage
        Catch ex As Exception
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            '             & "Error code: System error!" & vbNewLine _
            '             & "Error message: " & ex.Message, EventLogEntryType.Error)
            Throw ex
        End Try
    End Function

    'Public Function Message(ByRef pv_strMessage As String) As Long Implements CommonLibrary.IBDSDelivery.Message
    '    Dim v_strErrorSource As String = "BDSDelivery.Message", v_strErrorMessage As String = ""
    '    Dim v_strResultData As String = vbNullString
    '    Dim v_lngErr As Long
    '    Dim v_obj As New Sats.Branch.Branch
    '    Try
    '        'Read transaction message 
    '        Dim v_xmlDocumentMessage As New XmlDocumentEx
    '        v_xmlDocumentMessage.LoadXml(pv_strMessage)
    '        'Get header message.
    '        Dim v_attrColl As Xml.XmlAttributeCollection = v_xmlDocumentMessage.DocumentElement.Attributes
    '        Dim v_strLOCAL As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeLOCAL), Xml.XmlAttribute).Value)
    '        Dim v_strMSGTYPE As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeMSGTYPE), Xml.XmlAttribute).Value)
    '        Dim v_strLanguage As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeLANGUAGE), Xml.XmlAttribute).Value)

    '        If v_strLOCAL = "Y" Or v_strMSGTYPE = gc_MsgTypeTrans Then
    '            Select Case v_strMSGTYPE
    '                Case gc_MsgTypeTrans
    '                    'Transaction message
    '                    v_lngErr = v_obj.txTransfer(pv_strMessage)
    '                Case gc_MsgTypeObj
    '                    'Object message
    '                    v_lngErr = v_obj.objTransfer(pv_strMessage)
    '            End Select

    '        ElseIf v_strLOCAL = "N" And v_strMSGTYPE = gc_MsgTypeObj Then
    '            v_lngErr = v_obj.SendMessage2Host(pv_strMessage)
    '        End If


    '        If v_lngErr <> ERR_SYSTEM_OK Then
    '            'Lấy thông báo lỗi từ bảng DEFERROR đã được replication xuống BDS
    '            If v_strErrorMessage.Length = 0 Then
    '                v_strErrorMessage = GetErrorMessage(v_lngErr, v_strLanguage)
    '            End If
    '            ReplaceXMLErrorException(pv_strMessage, v_strErrorSource, v_lngErr, v_strErrorMessage)
    '            LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
    '                         & "Error code: " & v_lngErr.ToString() & vbNewLine _
    '                         & "Error message: " & v_strErrorMessage, EventLogEntryType.Error)
    '        End If
    '        Return v_lngErr
    '    Catch ex As Exception
    '        LogError.Write("Error source: " & ex.Source & vbNewLine _
    '                     & "Error code: System error!" & vbNewLine _
    '                     & "Error message: " & ex.Message, EventLogEntryType.Error)
    '        Throw ex
    '    Finally
    '        v_obj.Dispose()
    '        GC.Collect()
    '        GC.GetTotalMemory(False)
    '        GC.Collect()
    '    End Try
    'End Function

    'Public Function GetFooterAndHeader(ByVal pv_strRptInfo As String, ByRef pv_bytHeader() As Byte, ByRef pv_bytFooter() As Byte, ByRef pv_bytPHeader() As Byte, ByRef pv_bytPFooter() As Byte) As Long Implements CommonLibrary.IBDSDelivery.GetFooterAndHeader
    '    Try
    '        Dim v_obj As New Branch.Branch
    '        Return v_obj.GetFooterAndHeader(pv_strRptInfo, pv_bytHeader, pv_bytFooter, pv_bytPHeader, pv_bytPFooter)
    '    Catch ex As Exception
    '        LogError.Write("Error source: " & ex.Source & vbNewLine _
    '                     & "Error code: System error!" & vbNewLine _
    '                     & "Error message: " & ex.Message, EventLogEntryType.Error)
    '        Throw ex
    '    End Try
    'End Function

    'Public Function GetFooterAndHeader(ByVal pv_strRptInfo As String, ByRef pv_bytHeader() As Byte, ByRef pv_bytPHeader() As Byte) As Long Implements CommonLibrary.IBDSDelivery.GetFooterAndHeader
    '    Try
    '        Dim v_obj As New Branch.Branch
    '        Return v_obj.GetFooterAndHeader(pv_strRptInfo, pv_bytHeader, pv_bytPHeader)
    '    Catch ex As Exception
    '        LogError.Write("Error source: " & ex.Source & vbNewLine _
    '                     & "Error code: System error!" & vbNewLine _
    '                     & "Error message: " & ex.Message, EventLogEntryType.Error)
    '        Throw ex
    '    End Try
    'End Function

    Public Function GetPublicKey(ByVal pv_strUserName As String, ByRef pv_arrBytePublicKey() As Byte) As Long Implements CommonLibrary.IBDSDelivery.GetPublicKey
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
            LogError.Write(ex.Message, EventLogEntryType.Error, "gc_MODULE_BDS")
            Return ERR_SYSTEM_START
        End Try
    End Function

    Public Function Message(ByVal pv_strTellerID As String, ByRef pv_arrByteMessage() As Byte) As Long Implements CommonLibrary.IBDSDelivery.Message

        Dim v_strErrorSource As String = "BDSDelivery.Message", v_strErrorMessage As String = ""
        Dim v_strResultData As String = vbNullString
        Dim v_lngErr As Long
        Dim v_strMessage As String
        Dim v_obj As New Sats.Branch.Branch
        Dim v_arrBytePrivateKey() As Byte
        Dim v_arrBytePublicKey() As Byte
        Try

            If hPrivateKey(pv_strTellerID) Is Nothing Then
                Return ERR_SYSTEM_START
            End If

            v_arrBytePrivateKey = hPrivateKey(pv_strTellerID)
            pv_arrByteMessage = DecryptMessage(v_arrBytePrivateKey, pv_arrByteMessage)
            v_strMessage = Decompress(pv_arrByteMessage)

            'Read transaction message 
            Dim v_xmlDocumentMessage As New XmlDocumentEx
            v_xmlDocumentMessage.LoadXml(v_strMessage)
            'Get header message.
            Dim v_attrColl As Xml.XmlAttributeCollection = v_xmlDocumentMessage.DocumentElement.Attributes
            Dim v_strLOCAL As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeLOCAL), Xml.XmlAttribute).Value)
            Dim v_strMSGTYPE As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeMSGTYPE), Xml.XmlAttribute).Value)
            Dim v_strLanguage As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeLANGUAGE), Xml.XmlAttribute).Value)

            v_arrBytePublicKey = hPublicKey(pv_strTellerID)
            'If v_strMSGTYPE = gc_MsgTypeRpt Then
            '    pv_arrByteMessage = v_obj.SendReport2Host(v_strMessage)
            '    pv_arrByteMessage = EncryptMessage(v_arrBytePublicKey, pv_arrByteMessage)
            'Else
            v_lngErr = v_obj.SendMessage2Host(v_strMessage)
            pv_arrByteMessage = Compression(v_strMessage)
            pv_arrByteMessage = EncryptMessage(v_arrBytePublicKey, pv_arrByteMessage)
            'End If

            hPublicKey.Remove(pv_strTellerID)
            hPrivateKey.Remove(pv_strTellerID)

            Return v_lngErr
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                         & "Error code: System error!" & vbNewLine _
                         & "Error message: " & ex.Message, EventLogEntryType.Error, "gc_MODULE_BDS")
            v_arrBytePublicKey = hPublicKey(pv_strTellerID)
            pv_arrByteMessage = Compression(v_strMessage)
            pv_arrByteMessage = EncryptMessage(v_arrBytePublicKey, pv_arrByteMessage)

            hPublicKey.Remove(pv_strTellerID)
            hPrivateKey.Remove(pv_strTellerID)
            Return ERR_SYSTEM_START
        Finally
            v_obj.Dispose()
            GC.Collect()
            GC.GetTotalMemory(False)
            GC.Collect()
        End Try
    End Function

    Public Function RptMessage(ByVal pv_strTellerID As String, ByRef pv_arrByteMessage() As Byte) As Long Implements CommonLibrary.IBDSDelivery.RptMessage
        Dim v_strErrorSource As String = "BDSDelivery.Message", v_strErrorMessage As String = ""
        Dim v_strResultData As String = vbNullString
        Dim v_lngErr As Long
        Dim v_strMessage As String
        Dim v_obj As New Sats.Branch.Branch
        Dim v_arrBytePrivateKey() As Byte
        Dim v_arrBytePublicKey() As Byte
        Try

            If hPrivateKey(pv_strTellerID) Is Nothing Then
                Return ERR_SYSTEM_START
            End If

            v_arrBytePrivateKey = hPrivateKey(pv_strTellerID)
            pv_arrByteMessage = DecryptMessage(v_arrBytePrivateKey, pv_arrByteMessage)
            v_strMessage = Decompress(pv_arrByteMessage)

            'Read transaction message 
            Dim v_xmlDocumentMessage As New XmlDocumentEx
            v_xmlDocumentMessage.LoadXml(v_strMessage)
            'Get header message.
            Dim v_attrColl As Xml.XmlAttributeCollection = v_xmlDocumentMessage.DocumentElement.Attributes
            Dim v_strLOCAL As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeLOCAL), Xml.XmlAttribute).Value)
            Dim v_strMSGTYPE As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeMSGTYPE), Xml.XmlAttribute).Value)
            Dim v_strLanguage As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeLANGUAGE), Xml.XmlAttribute).Value)

            v_arrBytePublicKey = hPublicKey(pv_strTellerID)

            pv_arrByteMessage = v_obj.SendReport2Host(v_strMessage)
            pv_arrByteMessage = EncryptMessage(v_arrBytePublicKey, pv_arrByteMessage)

            hPublicKey.Remove(pv_strTellerID)
            hPrivateKey.Remove(pv_strTellerID)

            Return v_lngErr
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                         & "Error code: System error!" & vbNewLine _
                         & "Error message: " & ex.Message, EventLogEntryType.Error, "gc_MODULE_BDS")
            v_arrBytePublicKey = hPublicKey(pv_strTellerID)
            pv_arrByteMessage = Compression(v_strMessage)
            pv_arrByteMessage = EncryptMessage(v_arrBytePublicKey, pv_arrByteMessage)

            hPublicKey.Remove(pv_strTellerID)
            hPrivateKey.Remove(pv_strTellerID)
            Return ERR_SYSTEM_START
        Finally
            v_obj.Dispose()
            GC.Collect()
            GC.GetTotalMemory(False)
            GC.Collect()
        End Try
    End Function

    Public Function LoadInterface(ByVal pv_strTellerID As String, ByRef pv_arrByteMessage() As Byte) As Long Implements CommonLibrary.IBDSDelivery.LoadInterface
        Dim v_strErrorSource As String = "BDSDelivery.Message", v_strErrorMessage As String = ""
        Dim v_strResultData As String = vbNullString
        Dim v_lngErr As Long
        Dim v_strMessage As String
        Dim v_obj As New Sats.Branch.Branch
        Dim v_arrBytePrivateKey() As Byte
        Dim v_arrBytePublicKey() As Byte
        Try

            If hPrivateKey(pv_strTellerID) Is Nothing Then
                Return ERR_SYSTEM_START
            End If

            v_arrBytePrivateKey = hPrivateKey(pv_strTellerID)
            pv_arrByteMessage = DecryptMessage(v_arrBytePrivateKey, pv_arrByteMessage)
            v_strMessage = Decompress(pv_arrByteMessage)

            'Read transaction message 
            Dim v_xmlDocumentMessage As New XmlDocumentEx
            v_xmlDocumentMessage.LoadXml(v_strMessage)
            'Get header message.
            Dim v_attrColl As Xml.XmlAttributeCollection = v_xmlDocumentMessage.DocumentElement.Attributes
            Dim v_strLOCAL As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeLOCAL), Xml.XmlAttribute).Value)
            Dim v_strMSGTYPE As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeMSGTYPE), Xml.XmlAttribute).Value)
            Dim v_strLanguage As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeLANGUAGE), Xml.XmlAttribute).Value)

            v_arrBytePublicKey = hPublicKey(pv_strTellerID)

            pv_arrByteMessage = v_obj.SendInterface2Host(v_strMessage)
            pv_arrByteMessage = EncryptMessage(v_arrBytePublicKey, pv_arrByteMessage)

            hPublicKey.Remove(pv_strTellerID)
            hPrivateKey.Remove(pv_strTellerID)

            Return v_lngErr
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                         & "Error code: System error!" & vbNewLine _
                         & "Error message: " & ex.Message, EventLogEntryType.Error, "gc_MODULE_BDS")
            v_arrBytePublicKey = hPublicKey(pv_strTellerID)
            pv_arrByteMessage = Compression(v_strMessage)
            pv_arrByteMessage = EncryptMessage(v_arrBytePublicKey, pv_arrByteMessage)

            hPublicKey.Remove(pv_strTellerID)
            hPrivateKey.Remove(pv_strTellerID)
            Return ERR_SYSTEM_START
        Finally
            v_obj.Dispose()
            GC.Collect()
            GC.GetTotalMemory(False)
            GC.Collect()
        End Try
    End Function
End Class
