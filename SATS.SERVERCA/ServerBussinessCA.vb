Imports BkavCASign
Imports System.DirectoryServices
Imports Sats.ActiveDirectory
Imports System.Configuration
Imports Sats.DataAccessLayer.DataAccess
Imports Sats.CommonLibrary
Imports Oracle.DataAccess.Client
Imports Oracle.DataAccess.Types
Imports System.IO


Public Class ServerBussinessCA
    Public Const HEADER_ERROR As String = "SERVER_ERROR_"
    Public Const UNDEFINED_ERROR = HEADER_ERROR + "UNDEFINED"
    Public Const BUILD_LOGIN_ERROR As String = HEADER_ERROR + "BUILD_LOGIN"
    Public Const GET_LOGIN_ERROR As String = HEADER_ERROR + "GET_LOGIN"
    Public Const COMBINE_ERROR As String = HEADER_ERROR + "COMBINE"
    Public Const DECOMBINE_ERROR As String = HEADER_ERROR + "DECOMBINE"
    Public Const XML_ERROR As String = HEADER_ERROR + "XML"
    Public Const ENCRYPT_ERROR As String = HEADER_ERROR + "ENCRYPT"
    Public Const DECRYPT_ERROR As String = HEADER_ERROR + "DECRYPT"
    Public Const GET_SESSIONKEY_ERROR As String = HEADER_ERROR + "GET_SESSIONKEY"
    Public Const VSD_USER_ERROR As String = HEADER_ERROR + "VSD_USER"
    Public Const ARTER_LOGIN_ERROR As String = HEADER_ERROR + "AFTER_LOGIN"
    Public Const SIGN_SERVER_LOGIN_ERROR As String = HEADER_ERROR + "SIGN_SERVER"
    Public Const INSERT_SESSIONKEY_ERROR As String = HEADER_ERROR + "INSERT_SESSIONKEY"
    Public Const OPEN_HSM_ERROR As String = HEADER_ERROR + "OPEN_HSM"
    Public Const BKAV_HEADER_ERROR As String = HEADER_ERROR + "BKAV_"
    Public Const BKAV_USER_ERROR As String = BKAV_HEADER_ERROR + "USER"
    Public Const BKAV_SIGN_SERVER_ERROR As String = BKAV_HEADER_ERROR + "SIGN_SERVER"
    Public Const BKAV_SESSIONKEY_ERROR As String = BKAV_HEADER_ERROR + "SESSIONKEY"

    Private Shared m_oSignServer As SignServer
    Private Shared m_oCertServer As CertificateServer

    '***********************************************************************
    'Input Parameters     :pv_strOrigXML: String
    'Ouput Parameters     :none
    'Returned value       :String
    'Purpose        	  :Sign pv_strOrigXML to the signature and combine it
    '                      with pv_strOrigXML
    'Created date         :15/11/2010
    'Author               :Myvq
    'Last update date     :15/11/2010
    'Last modifying person:Myvq
    '***********************************************************************
    Public Shared Function getHSM() As SignServer
        'OpenHSM(m_oSignServer)
        Return m_oSignServer
    End Function

    Public Shared Function CombineData(ByVal pv_strOrigXML As String) As String
        Dim v_strResult = ""
        Dim err_step As Integer
        Try
            'init variable            

            'If (m_oSignServer Is Nothing) Then
            ' OpenHSM(m_oSignServer)
            'End If
            Dim v_intError As Integer = 0
            Dim v_strSignNature As String = ""
            Dim v_oXMLDocument = New Xml.XmlDocument
            'sign data
            err_step = 1
            v_intError = m_oSignServer.SignString(pv_strOrigXML, v_strSignNature)
            If (v_intError <> 0) Then
                Return BKAV_HEADER_ERROR + v_intError.ToString
            End If
            err_step = 2

            ''will be deleted in future
            'v_strSignNature = "Signature"

            'combine pv_strOrigXML and its signature
            'v_strResult = "<DataXML>" _
            '& "<OrigXML>" & pv_strOrigXML & "</OrigXML>" _
            '& "<SignatureXML>" & v_strSignNature & "</SignatureXML>" _
            '& "</DataXML>"
            Dim v_oXMLElement = v_oXMLDocument.CreateElement("DataXML")
            Dim v_oOrigXML = v_oXMLDocument.CreateAttribute("OrigXML")
            v_oOrigXML.Value = pv_strOrigXML
            v_oXMLElement.Attributes.Append(v_oOrigXML)
            Dim v_oSignature = v_oXMLDocument.CreateAttribute("SignatureXML")
            v_oSignature.Value = v_strSignNature
            v_oXMLElement.Attributes.Append(v_oSignature)

            v_oXMLDocument.AppendChild(v_oXMLElement)

            v_strResult = v_oXMLDocument.InnerXml
            Return v_strResult

        Catch ex As Exception
            LogError.Write(ex.Message & vbCrLf & "Error connect to: " & err_step & "|" & m_oSignServer.ErrorMessage _
                           & "|" & m_oSignServer.ErrorStatus & "|" & m_oSignServer.LoadContent _
                           , _
                           EventLogEntryType.Error, "ServerbussinessCA.CombineData(String)")
            Return COMBINE_ERROR
        End Try
    End Function
    Public Shared Function SignByte(ByVal pv_strOrigXML As String, _
                                       ByVal pv_oSignServer As SignServer) As String
        Try
            Dim v_strReturn As String
            Dim v_arrDataServer As Byte() = Convert.FromBase64String(pv_strOrigXML)
            Dim v_intErr As Integer = pv_oSignServer.SignByte(v_arrDataServer, v_strReturn)
            'Dim v_intErr As Integer = pv_oSignServer.Sign(v_arrDataServer, v_strReturn)
            Dim v_strerr = "Data file string:" & pv_strOrigXML & "|" & m_oSignServer.LoadContent
            LogError.Write(v_strerr, EventLogEntryType.Error, "ServerbussinessCA.SignByte(String,SignServer)")
            Return v_strReturn
        Catch ex As Exception
            LogError.Write(ex.Message & vbCrLf & "Error connect to: " & "|" & m_oSignServer.ErrorMessage _
                           & "|" & m_oSignServer.ErrorStatus & "|" & m_oSignServer.LoadContent & "[" & "]" _
                           , _
                           EventLogEntryType.Error, "ServerbussinessCA.SignByte(String,SignServer)")
            Return COMBINE_ERROR
        End Try


    End Function
    '***********************************************************************
    'Input Parameters     :pv_strOrigXML: String
    'Ouput Parameters     :none
    'Returned value       :String
    'Purpose        	  :Sign pv_strOrigXML to the signature and combine it
    '                      with pv_strOrigXML
    'Created date         :15/11/2010
    'Author               :Myvq
    'Last update date     :15/11/2010
    'Last modifying person:Myvq
    '***********************************************************************
    Public Shared Function CombineData(ByVal pv_strOrigXML As String, _
                                       ByVal pv_oSignServer As SignServer) As String
        Dim v_strResult = ""
        Dim err_step As Integer
        Try
            'init variable            

            'Dim v_oSignServer As New SignServer
            Dim v_intError As Integer = 0
            Dim v_strSignNature As String = ""
            Dim v_oXMLDocument = New Xml.XmlDocument
            'sign data
            err_step = 1
            v_intError = pv_oSignServer.SignString(pv_strOrigXML, v_strSignNature)
            If (v_intError <> 0) Then
                Return BKAV_HEADER_ERROR + v_intError.ToString
            End If
            err_step = 2
            ''will be deleted in future
            'v_strSignNature = "Signature"

            'combine pv_strOrigXML and its signature
            'v_strResult = "<DataXML>" _
            '& "<OrigXML>" & pv_strOrigXML & "</OrigXML>" _
            '& "<SignatureXML>" & v_strSignNature & "</SignatureXML>" _
            '& "</DataXML>"
            Dim v_oXMLElement = v_oXMLDocument.CreateElement("DataXML")
            Dim v_oOrigXML = v_oXMLDocument.CreateAttribute("OrigXML")
            v_oOrigXML.Value = pv_strOrigXML
            v_oXMLElement.Attributes.Append(v_oOrigXML)
            Dim v_oSignature = v_oXMLDocument.CreateAttribute("SignatureXML")
            v_oSignature.Value = v_strSignNature
            v_oXMLElement.Attributes.Append(v_oSignature)

            v_oXMLDocument.AppendChild(v_oXMLElement)

            v_strResult = v_oXMLDocument.InnerXml
            Return v_strResult
        Catch ex As Exception
            LogError.Write(ex.Message & vbCrLf & "Error connect to: " & err_step & "|" & m_oSignServer.ErrorMessage _
                           & "|" & m_oSignServer.ErrorStatus & "|" & m_oSignServer.LoadContent & "[" & "]" _
                           , _
                           EventLogEntryType.Error, "ServerbussinessCA.CombineData(String)")
            Return COMBINE_ERROR
        End Try
    End Function
    '***********************************************************************
    'Input Parameters     :pv_strCombinedXML: String
    'Ouput Parameters     :pv_strOrigXML: String, pv_strSignatureXML: String
    'Return               :String
    'Purpose        	  :Decombine pv_strCombinedXML into pv_strOrigXML
    '                      and pv_strSignatureXML. Return error code
    'Created date         :16/11/2010
    'Author               :Myvq
    'Last update date     :16/11/2010
    'Last modifying person:Myvq
    '***********************************************************************
    Public Shared Function DeCombineData(ByVal pv_strCombinedXML As String, _
        ByRef pv_strOrigXML As String, ByRef pv_strSignatureXML As String) As String
        pv_strOrigXML = ""
        pv_strSignatureXML = ""
        Dim v_strDocument As New System.Xml.XmlDocument
        Try
            v_strDocument.LoadXml(pv_strCombinedXML)
        Catch ex As Exception
            Return XML_ERROR
        End Try

        Try
            ''Get original XML
            'Dim v_oList = v_strDocument.GetElementsByTagName("OrigXML")
            'For Each v_oItem As System.Xml.XmlElement In v_oList
            '    pv_strOrigXML = v_oItem.InnerText
            'Next

            ''Get signature XML
            'v_oList = v_strDocument.GetElementsByTagName("SignatureXML")
            'For Each v_oItem As System.Xml.XmlElement In v_oList
            '    pv_strSignatureXML = v_oItem.InnerText
            'Next
            'Dim v_xmlRootMessage As New XmlDocumentEx
            'v_xmlRootMessage.LoadXml(pv_strMessage)
            'Dim v_attrRColl As Xml.XmlAttributeCollection = v_xmlRootMessage.DocumentElement.Attributes

            Dim v_oDocument As New Xml.XmlDocument
            v_oDocument.LoadXml(pv_strCombinedXML)
            Dim v_attrRColl As Xml.XmlAttributeCollection = v_oDocument.DocumentElement.Attributes
            pv_strOrigXML = CStr(CType(v_attrRColl.GetNamedItem("OrigXML"), Xml.XmlAttribute).Value)
            pv_strSignatureXML = CStr(CType(v_attrRColl.GetNamedItem("SignatureXML"), Xml.XmlAttribute).Value)
            'For Each v_oNode As Xml.XmlNode In v_oDocument.SelectNodes("DataXML")
            '    pv_strOrigXML = v_oNode.Attributes("OrigXML").Value
            'Next

            'For Each v_oNode As Xml.XmlNode In v_oDocument.SelectNodes("DataXML")
            '    pv_strSignatureXML = v_oNode.Attributes("SignatureXML").Value
            'Next

            Return ""
        Catch ex As Exception
            LogError.Write(ex.Message & vbCrLf & "Error connect to: ", EventLogEntryType.Error, _
                           "ServerbussinessCA.DecombineData(String,String)")
            Return DECOMBINE_ERROR
        End Try
    End Function
    '***********************************************************************
    'Input Parameters     :pv_strOrigXML: String, pv_oSessionKey: SessionKey
    'Return               :String
    'Purpose        	  :Combine pv_strOrigXML with is signature and encrypt
    '                      result with session key
    'Created date         :16/11/2010
    'Author               :Myvq
    'Last update date     :16/11/2010
    'Last modifying person:Myvq
    '***********************************************************************
    Public Shared Function EncryptXML(ByVal pv_strOrigXML As String, _
                            ByVal pv_oSessionKey As SessionKey) As String
        Dim v_strResult As String = ""
        Try
            Dim v_strCombinedData = CombineData(pv_strOrigXML)
            If (v_strCombinedData.StartsWith(HEADER_ERROR)) Then
                Return v_strCombinedData
            End If

            Dim v_intError As Integer = _
                 pv_oSessionKey.EncryptString(v_strCombinedData, v_strResult)
            If (v_intError <> 0) Then
                Return BKAV_HEADER_ERROR + v_intError.ToString
            Else
                Return v_strResult
            End If
        Catch ex As Exception
            LogError.Write(ex.Message & vbCrLf & "Error connect to: ", EventLogEntryType.Error, _
                           "ServerbussinessCA.EncryptXML(String,SessionKey)")
            Return ENCRYPT_ERROR
        End Try
    End Function
    '***********************************************************************
    'Input Parameters     :pv_strOrigXML: String, pv_oSessionKey: SessionKey
    'Return               :String
    'Purpose        	  :Combine pv_strOrigXML with is signature and encrypt
    '                      result with session key
    'Created date         :16/11/2010
    'Author               :Myvq
    'Last update date     :16/11/2010
    'Last modifying person:Myvq
    '***********************************************************************
    Public Shared Function EncryptXML(ByVal pv_strOrigXML As String, _
                            ByVal pv_oSessionKey As SessionKey, _
                            ByVal pv_oSignServer As SignServer) As String
        Dim v_strResult As String = ""
        Try
            Dim v_strCombinedData = CombineData(pv_strOrigXML, pv_oSignServer)
            If (v_strCombinedData.StartsWith(HEADER_ERROR)) Then
                Return v_strCombinedData
            End If

            Dim v_intError As Integer = _
                 pv_oSessionKey.EncryptString(v_strCombinedData, v_strResult)
            If (v_intError <> 0) Then
                Return BKAV_HEADER_ERROR + v_intError.ToString
            Else
                Return v_strResult
            End If
        Catch ex As Exception
            LogError.Write(ex.Message & vbCrLf & "Error connect to: ", EventLogEntryType.Error, _
                           "ServerbussinessCA.EncryptXML(String,SessionKey,SignServer)")
            Return ENCRYPT_ERROR
        End Try
    End Function
    '***********************************************************************
    'Input Parameters     :pv_strOrigXML: String, pv_oSessionKey: SessionKey
    'Return               :String
    'Purpose        	  :Combine pv_strOrigXML with is signature and encrypt
    '                      result with session key
    'Created date         :16/11/2010
    'Author               :Myvq
    'Last update date     :16/11/2010
    'Last modifying person:Myvq
    '***********************************************************************
    Public Shared Function EncryptXML(ByVal pv_strOrigXML As String) As String
        Dim v_strResult As String = ""
        Try
            'OpenHSM(m_oSignServer)
            Dim v_strCombinedData = CombineData(pv_strOrigXML, m_oSignServer)
            If (v_strCombinedData.StartsWith(HEADER_ERROR)) Then
                Return v_strCombinedData
            End If

            Dim v_oSessionKey As SessionKey = New SessionKey
            v_oSessionKey.Generate()

            Dim v_strEncryptedString As String = ""
            Dim v_strEncryptedSessionKey As String = ""

            Dim v_intError As Integer = _
                 v_oSessionKey.EncryptString(v_strCombinedData, v_strEncryptedString)
            If (v_intError <> 0) Then
                Return BKAV_HEADER_ERROR + v_intError.ToString
            End If
            v_intError = m_oSignServer.EncryptSessionKey(v_oSessionKey, v_strEncryptedSessionKey)
            If (v_intError <> 0) Then
                Return BKAV_HEADER_ERROR + v_intError.ToString
            End If

            Dim v_oXMLDocument As Xml.XmlDocument = New Xml.XmlDocument
            Dim v_oXMLElement = v_oXMLDocument.CreateElement("DataXML")
            Dim v_oEncryptedString = v_oXMLDocument.CreateAttribute("EncryptedStringXML")
            v_oEncryptedString.Value = v_strEncryptedString
            v_oXMLElement.Attributes.Append(v_oEncryptedString)
            Dim v_oEncryptedSessionKey = v_oXMLDocument.CreateAttribute("EncryptedSessionKeyXML")
            v_oEncryptedSessionKey.Value = v_strEncryptedSessionKey
            v_oXMLElement.Attributes.Append(v_oEncryptedSessionKey)

            v_oXMLDocument.AppendChild(v_oXMLElement)

            'Dim v_oXMLReturnedDocument As Xml.XmlDocument = New Xml.XmlDocument
            'Dim v_oXMLReturnedElement = v_oXMLReturnedDocument.CreateElement("DataXML")
            'Dim v_oOrigXML = v_oXMLReturnedDocument.CreateAttribute("OrigXML")
            'v_oOrigXML.Value = pv_strOrigXML
            'v_oXMLReturnedElement.Attributes.Append(v_oOrigXML)
            'Dim v_oEncryptedData = v_oXMLReturnedDocument.CreateAttribute("SignatureXML")
            'v_oEncryptedData.Value = v_oXMLDocument.InnerXml
            'v_oXMLReturnedElement.Attributes.Append(v_oEncryptedData)

            'v_oXMLReturnedDocument.AppendChild(v_oXMLReturnedElement)

            v_strResult = v_oXMLDocument.InnerXml

            Return v_strResult

        Catch ex As Exception
            LogError.Write(ex.Message & vbCrLf & "Error connect to: ", EventLogEntryType.Error, _
                           "ServerbussinessCA.EncryptXML(String,SessionKey,SignServer)")
            Return ENCRYPT_ERROR
        End Try
    End Function
    '***********************************************************************
    'Input Parameters     :pv_strOrigXML: String
    'Return               :String
    'Purpose        	  :Combine pv_strOrigXML with is signature and encrypt
    '                      result with session key
    'Created date         :16/11/2010
    'Author               :Myvq
    'Last update date     :16/11/2010
    'Last modifying person:Myvq
    '***********************************************************************
    Public Shared Function EncryptXML(ByVal pv_strOrigXML As String, ByVal pv_strUserName As String) As String
        Dim v_strResult As String = ""
        'Get session key
        Dim v_strSessionKey As String
        Try
            v_strSessionKey = GetSessionKey(pv_strUserName)
        Catch ex As Exception
            Return GET_SESSIONKEY_ERROR
        End Try

        'Encrypt
        Try
            'bangpv: Chỉ encrypt string đã combine 
            'Dim v_strCombinedData = CombineData(pv_strOrigXML)
            'If (v_strCombinedData.StartsWith(HEADER_ERROR)) Then
            '    Return v_strCombinedData
            'End If

            Dim v_oSessionKey As New SessionKey
            v_oSessionKey.LoadSessionKey(v_strSessionKey)
            Dim v_intError As Integer = _
                 v_oSessionKey.EncryptString(pv_strOrigXML, v_strResult)
            If (v_intError <> 0) Then
                Return BKAV_HEADER_ERROR + v_intError.ToString
            Else
                Return v_strResult
            End If
        Catch ex As Exception
            LogError.Write(ex.Message & vbCrLf & "Error connect to: ", EventLogEntryType.Error, _
                           "ServerbussinessCA.EncryptXML(String,String)")
            Return ENCRYPT_ERROR
        End Try
    End Function
    '***********************************************************************
    'Input Parameters     :pv_strEncryptedXML: String, pv_oSessionKey: SessionKey
    'Return               :String
    'Purpose        	  :Get original XML from pv_strEncryptedXML
    'Created date         :17/11/2010
    'Author               :Myvq
    'Last update date     :17/11/2010
    'Last modifying person:Myvq
    '***********************************************************************
    Public Shared Function DecryptXML(ByVal pv_strEncryptedXML As String, _
                            ByVal pv_oSessionKey As SessionKey) As String
        Dim v_strOrigXML As String = ""
        Dim v_strCombinedXML As String = ""
        Try
            'Decrypt pv_strEncryptedXML with session key
            Dim v_oDocument = New Xml.XmlDocument
            v_oDocument.LoadXml(pv_strEncryptedXML)
            For Each v_oXMLNode As Xml.XmlNode In v_oDocument.SelectNodes("DataXML")
                v_strCombinedXML = v_oXMLNode.Attributes("EncryptedXML").Value()
            Next

            Dim v_intError As Integer = _
                 pv_oSessionKey.DecryptString(pv_strEncryptedXML, v_strCombinedXML)
            If (v_intError <> 0) Then
                Return BKAV_HEADER_ERROR + v_intError.ToString
            End If

            'Decombine original XML and its signature
            Dim v_strSignature As String = ""
            Dim v_strError = DeCombineData(v_strCombinedXML, v_strOrigXML, v_strSignature)
            If (v_strError.StartsWith(HEADER_ERROR)) Then
                Return v_strError
            End If

            'Get VSD username
            Dim v_strUserName = ""
            Try
                Dim v_strDocument As New Xml.XmlDocument
                v_strDocument.LoadXml(v_strOrigXML)
                Dim v_oList = v_strDocument.GetElementsByTagName("UserName")
                For Each v_oItem As System.Xml.XmlElement In v_oList
                    v_strUserName = v_oItem.InnerText
                Next
            Catch ex As Exception
                Return XML_ERROR
            End Try

            ''Get VSD user
            'Dim v_oVSDUser As DirectoryEntry = Nothing
            'Try
            '    v_oVSDUser = GetVSDUser(v_strUserName)
            '    If (v_oVSDUser Is Nothing) Then
            '        Return VSD_USER_ERROR
            '    End If
            'Catch ex As Exception
            '    Return VSD_USER_ERROR
            'End Try

            ''Get BKAV user
            'Dim v_oBKAVUser As CertificateServer
            'Try
            '    v_oBKAVUser = GetBKAVUser(v_oVSDUser)
            '    If (v_oBKAVUser Is Nothing) Then
            '        Return BKAV_USER_ERROR
            '    End If
            'Catch ex As Exception
            '    Return BKAV_USER_ERROR
            'End Try

            ''Verify original XML and its signature
            'v_intError = v_oBKAVUser.VerifyString(v_strOrigXML, v_strSignature)
            'If (v_intError <> 0) Then
            '    Return BKAV_HEADER_ERROR + v_intError.ToString
            'End If

            Return v_strOrigXML
        Catch ex As Exception
            LogError.Write(ex.Message & vbCrLf & "Error connect to: ", EventLogEntryType.Error, _
                           "ServerbussinessCA.DecryptXML(String,SessionKey)")
            Return ""
        End Try
    End Function
    Public Shared Function Check_Signature_client(ByVal v_strData As String, ByVal v_strSignatureClient As String, ByVal v_strVSDUsername As String, _
                                                   ByVal v_strBKAVPassword As String, ByVal v_strLDAPIP As String, ByVal v_strArrayOgarnization As String, _
                                                   ByVal v_strBKAVDomainComponent As String, ByVal v_strUsername As String) As Boolean
        Dim pv_oCertServer As CertificateServer
        Try
            pv_oCertServer = GetBKAVUser(v_strVSDUsername, v_strUsername, v_strBKAVPassword, _
                                                v_strLDAPIP, v_strArrayOgarnization, v_strBKAVDomainComponent)
            If (pv_oCertServer Is Nothing) Then
                Return BKAV_USER_ERROR
            End If
            Dim v_intError As Integer = pv_oCertServer.VerifyString(v_strData, v_strSignatureClient)
            If v_intError = 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            LogError.Write(ex.Message & vbCrLf & "Error connect to: ", EventLogEntryType.Error, _
                           "ServerbussinessCA.Check_Signature_client(String,String,String,String,String,String,String,String)")
            Throw ex
            Return False
        End Try

    End Function
    Public Shared Function Check_VSD_Signature(ByVal v_strData As String, ByVal v_strVerifyData As String, ByVal pv_oSignServer As SignServer) As Boolean
        'Dim pv_oSignServer As SignServer = New SignServer
        Dim v_strVSDSignature As String = ""
        Try
            'pv_oSignServer.SlotId = 0
            'pv_oSignServer.PIN = "12345678"
            'pv_oSignServer.PublicKeyName = "VSD"
            'pv_oSignServer.PrivateKeyName = "VSD"
            'pv_oSignServer.CertificateName = "VSD"
            'Dim int_chk As Integer = pv_oSignServer.OpenConnection("X", "Y", "Z")
            Dim int_chk = pv_oSignServer.SignString(v_strData, v_strVSDSignature)
            If int_chk = 0 Then
                int_chk = pv_oSignServer.VerifyString(v_strVerifyData, v_strVSDSignature)
                If int_chk = 0 Then
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
        Catch ex As Exception
            LogError.Write(ex.Message & vbCrLf & "Error connect to: ", EventLogEntryType.Error, _
                           "ServerbussinessCA.Check_VSD_Signature(String,String,SignServer)")

        End Try


    End Function

    '***********************************************************************
    'Input Parameters     :pv_strEncryptedXML: String, pv_strSessionKey: String
    'Return               :String
    'Purpose        	  :Get original XML from pv_strEncryptedXML
    'Created date         :18/11/2010
    'Author               :Myvq
    'Last update date     :18/11/2010
    'Last modifying person:Myvq
    '***********************************************************************
    Public Shared Function DecryptXML(ByVal pv_strEncryptedXML As String, _
                            ByVal pv_strEncryptedSessionKey As String, _
                            ByRef pv_oCertServer As CertificateServer, _
                            ByVal pv_oSignServer As SignServer, _
                            ByVal pv_strVSDUsername As String, _
                            ByVal pv_strPassword As String, _
                            ByVal pv_strLDAPIP As String, _
                            ByVal pv_strArrayOgarnization As String, _
                            ByVal pv_strBKAVDomainComponent As String) As String
        'ByVal pv_oSignSever As SignServer

        Dim v_strOrigXML As String = ""
        Try
            'Get session key object
            Dim v_oSessionKey As New BkavCASign.SessionKey
            Try
                ''Connect HSM
                'Dim v_oSignServer = New SignServer
                'Try
                '    Dim v_strHSMUserName = _
                '    System.Configuration.ConfigurationManager.AppSettings("HSMUserName")
                '    Dim v_strHSMPassword = _
                '        System.Configuration.ConfigurationManager.AppSettings("HSMPassword")
                '    Dim v_strHSMIPAddress = _
                '                        System.Configuration.ConfigurationManager.AppSettings("HSMIPAddress")
                '    v_oSignServer.OpenConnection(v_strHSMUserName, v_strHSMPassword, v_strHSMIPAddress)
                'Catch ex As Exception
                '    Return SIGN_SERVER_LOGIN_ERROR
                'End Try
                'đọc file vào 


                Dim v_intErr = pv_oSignServer.DecryptSessionKey( _
                   pv_strEncryptedSessionKey, v_oSessionKey)
                If (v_intErr <> 0) Then
                    Return BKAV_HEADER_ERROR + v_intErr.ToString
                End If
                If (v_oSessionKey Is Nothing) Then
                    Return BKAV_SESSIONKEY_ERROR
                End If

                ''will be deleted in future
                'v_oSessionKey.LoadSessionKey(pv_strEncryptedSessionKey)
            Catch ex As Exception
                Return BKAV_SIGN_SERVER_ERROR
            End Try

            'Decrypt pv_strEncryptedXML with session key
            Dim v_strCombinedXML As String = ""
            Dim v_intError As Integer = _
                 v_oSessionKey.DecryptString(pv_strEncryptedXML, v_strCombinedXML)
            If (v_intError <> 0) Then
                Return BKAV_HEADER_ERROR + v_intError.ToString
            End If

            'Decombine original XML and its signature
            Dim v_strSignature As String = ""
            Dim v_strError = DeCombineData(v_strCombinedXML, v_strOrigXML, v_strSignature)
            If (v_strError.StartsWith(HEADER_ERROR)) Then
                Return v_strError
            End If

            'Get VSD username and password  
            Dim v_strUserName = ""
            Dim v_strPassword = ""
            Try
                Dim v_strDocument As New System.Xml.XmlDocument
                v_strDocument.LoadXml("<DataXML>" + v_strOrigXML + "</DataXML>")
                Dim v_oList = v_strDocument.GetElementsByTagName("UserName")
                For Each v_oItem As System.Xml.XmlElement In v_oList
                    v_strUserName = v_oItem.InnerText
                Next
                v_oList = v_strDocument.GetElementsByTagName("Password")
                For Each v_oItem As System.Xml.XmlElement In v_oList
                    v_strPassword = v_oItem.InnerText
                Next
            Catch ex As Exception
                Return XML_ERROR
            End Try

            'Get VSD user
            Dim v_strAuthType As String = System.Configuration.ConfigurationManager.AppSettings("AuthType").ToString
            If v_strAuthType = gc_AUTHORIZATION_MODE_LDAP Then
                Dim v_oVSDUser As DirectoryEntry = Nothing
                Try
                    v_oVSDUser = GetVSDUser(v_strUserName, DecryptString(v_strUserName, v_strPassword))
                    If (v_oVSDUser Is Nothing) Then
                        Return VSD_USER_ERROR
                    End If
                Catch ex As Exception
                    Return VSD_USER_ERROR
                End Try
            End If

            'Get BKAV user
            Try
                pv_oCertServer = GetBKAVUser(pv_strVSDUsername, v_strUserName, pv_strPassword, _
                                             pv_strLDAPIP, pv_strArrayOgarnization, pv_strBKAVDomainComponent)
                If (pv_oCertServer Is Nothing) Then
                    Return BKAV_USER_ERROR
                End If
            Catch ex As Exception
                Return BKAV_USER_ERROR
            End Try

            'Verify original XML and its signature
            If System.Configuration.ConfigurationManager.AppSettings("CheckOCSP") = 1 Then
                Certificate.SetIntermediateCertificateAuthorities("61 0e f3 e1 00 00 00 00 00 05")
                v_intError = pv_oCertServer.VerifyString(v_strOrigXML, v_strSignature, True)
            Else
                v_intError = pv_oCertServer.VerifyString(v_strOrigXML, v_strSignature)
            End If
            If (v_intError <> 0) Then
                Return BKAV_HEADER_ERROR + v_intError.ToString
            End If

            Return v_strOrigXML
        Catch ex As Exception
            LogError.Write(ex.Message & vbCrLf & "Error connect to: ", EventLogEntryType.Error, _
                           "ServerbussinessCA.DecryptXML(String,String,CertificateServer,SignServer,String,String,String,String,String)")
            Return ""
        End Try
    End Function
    '***********************************************************************
    'Input Parameters     :pv_strOrigXML: String
    'Onput Parameters     :pv_strUserName: String, pv_strPassword: String
    '                     :pv_strIPAddress: String
    'Return               :String
    'Purpose        	  :Get infomation from pv_strOrigXML. Return error code
    'Created date         :18/11/2010
    'Author               :Myvq
    'Last update date     :18/11/2010
    'Last modifying person:Myvq
    '***********************************************************************
    Public Shared Function GetOrigXMLInfo(ByVal pv_strOrigXML As String, _
                            ByRef pv_strUserName As String, _
                            ByRef pv_strPassword As String, _
                            ByRef pv_strIPAddress As String) As String
        Dim v_strDocument As New System.Xml.XmlDocument
        Try
            v_strDocument.LoadXml(pv_strOrigXML)
        Catch ex As Exception
            Return XML_ERROR
        End Try

        Try
            'Get user name
            Dim v_oList = v_strDocument.GetElementsByTagName("UserName")
            For Each v_oItem As System.Xml.XmlElement In v_oList
                pv_strUserName = v_oItem.InnerText
            Next

            'Get user name
            v_oList = v_strDocument.GetElementsByTagName("Password")
            For Each v_oItem As System.Xml.XmlElement In v_oList
                pv_strPassword = v_oItem.InnerText
            Next

            'Get user name
            v_oList = v_strDocument.GetElementsByTagName("IPAddress")
            For Each v_oItem As System.Xml.XmlElement In v_oList
                pv_strIPAddress = v_oItem.InnerText
            Next

            Return ""
        Catch ex As Exception
            LogError.Write(ex.Message & vbCrLf & "Error connect to: ", EventLogEntryType.Error, _
                           "ServerbussinessCA.GetOrigXMLInfo(String,String,String,String)")
            Return GET_LOGIN_ERROR
        End Try
    End Function
    '***********************************************************************
    'Input Parameters     :pv_strEncryptedXML: String
    'Return               :String
    'Purpose        	  :Get original XML from pv_strEncryptedXML
    'Created date         :17/11/2010
    'Author               :Myvq
    'Last update date     :17/11/2010
    'Last modifying person:Myvq
    '***********************************************************************
    Public Shared Function DecryptXML(ByVal pv_strEncryptedXML As String, _
                                      ByVal pv_strUsername As String, _
                                      Optional ByRef pv_strSignature As String = "") As String
        Dim v_strOrigXML As String = ""

        'Get session key
        Dim v_strSessionKey As String
        Try
            v_strSessionKey = GetSessionKey(pv_strUsername)
            If (v_strSessionKey.StartsWith(HEADER_ERROR)) Then
                Return GET_SESSIONKEY_ERROR
            End If
        Catch ex As Exception
            Return UNDEFINED_ERROR
        End Try

        'Get original XML
        Try
            'Decrypt pv_strEncryptedXML with session key
            Dim v_strCombinedXML As String = ""
            Dim v_oSessionKey As New SessionKey
            v_oSessionKey.LoadSessionKey(v_strSessionKey)

            Dim v_intError As Integer = _
                 v_oSessionKey.DecryptString(pv_strEncryptedXML, v_strCombinedXML)
            If (v_intError <> 0) Then
                Return BKAV_HEADER_ERROR + v_intError.ToString
            End If

            'Decombine original XML and its signature
            Dim v_strSignature As String = ""
            Dim v_strError = DeCombineData(v_strCombinedXML, v_strOrigXML, v_strSignature)
            If (v_strError.StartsWith(HEADER_ERROR)) Then
                Return v_strError
            End If

            'Get VSD username
            'Dim v_strUserName = ""
            'Try
            '    Dim v_strDocument As New System.Xml.XmlDocument
            '    v_strDocument.LoadXml(v_strOrigXML)
            '    Dim v_oList = v_strDocument.GetElementsByTagName("UserName")
            '    For Each v_oItem As System.Xml.XmlElement In v_oList
            '        v_strUserName = v_oItem.InnerText
            '    Next
            'Catch ex As Exception
            '    Return XML_ERROR
            'End Try

            'Get VSD user
            'Dim v_oVSDUser As DirectoryEntry = Nothing
            'Try
            '    v_oVSDUser = GetVSDUser(v_strUserName)
            '    If (v_oVSDUser Is Nothing) Then
            '        Return VSD_USER_ERROR
            '    End If
            'Catch ex As Exception
            '    Return VSD_USER_ERROR
            'End Try

            ''Get BKAV user
            'Dim v_oBKAVUser As CertificateServer
            'Try
            '    v_oBKAVUser = GetBKAVUser(v_oVSDUser)
            '    If (v_oBKAVUser Is Nothing) Then
            '        Return BKAV_USER_ERROR
            '    End If
            'Catch ex As Exception
            '    Return BKAV_USER_ERROR
            'End Try

            ''Verify original XML and its signature
            'v_intError = v_oBKAVUser.VerifyString(v_strOrigXML, v_strSignature)
            'If (v_intError <> 0) Then
            '    Return BKAV_HEADER_ERROR + v_intError.ToString
            'End If

            'get signature 
            pv_strSignature = v_strSignature

            Return v_strOrigXML
        Catch ex As Exception
            LogError.Write(ex.Message & vbCrLf & "Error connect to: ", EventLogEntryType.Error, _
                           "ServerbussinessCA.DecryptXML(String,String,String)")
            Return DECRYPT_ERROR
        End Try
    End Function

    Public Shared Function DecryptXML_Exports(ByVal pv_strEncryptedXML As String, _
                                     ByVal pv_strUsername As String, ByVal pv_StrDate As String, _
                                     Optional ByRef pv_strSignature As String = "") As String
        Dim v_strOrigXML As String = ""

        'Get session key
        Dim v_strSessionKey As String
        Try
            v_strSessionKey = GetSessionKey_WithDate(pv_strUsername, pv_StrDate)
            If (v_strSessionKey.StartsWith(HEADER_ERROR)) Then
                Return GET_SESSIONKEY_ERROR
            End If
        Catch ex As Exception
            Return UNDEFINED_ERROR
        End Try

        'Get original XML
        Try
            'Decrypt pv_strEncryptedXML with session key
            Dim v_strCombinedXML As String = ""
            Dim v_oSessionKey As New SessionKey
            v_oSessionKey.LoadSessionKey(v_strSessionKey)

            Dim v_intError As Integer = _
                 v_oSessionKey.DecryptString(pv_strEncryptedXML, v_strCombinedXML)
            If (v_intError <> 0) Then
                Return BKAV_HEADER_ERROR + v_intError.ToString
            End If

            'Decombine original XML and its signature
            Dim v_strSignature As String = ""
            Dim v_strError = DeCombineData(v_strCombinedXML, v_strOrigXML, v_strSignature)
            If (v_strError.StartsWith(HEADER_ERROR)) Then
                Return v_strError
            End If

            'get signature 
            pv_strSignature = v_strSignature

            Return v_strOrigXML
        Catch ex As Exception
            LogError.Write(ex.Message & vbCrLf & "Error connect to: ", EventLogEntryType.Error, _
                           "ServerbussinessCA.DecryptXML(String,String,String)")
            Return DECRYPT_ERROR
        End Try
    End Function
    '***********************************************************************
    'Input Parameters     :none
    'Return               :String
    'Purpose        	  :Get session key from database if exits, otherwise
    '                      return ""
    'Created date         :17/11/2010
    'Author               :Myvq
    'Last update date     :17/11/2010
    'Last modifying person:Myvq
    '***********************************************************************
    Public Shared Function GetSessionKey(ByVal pv_strUserName As String) As String
        Try
            Dim v_strInquiry = "select tlid from tlprofiles where upper(tlname) = '" + UCase(pv_strUserName) + "'"
            Dim v_oDataAccess = New DataAccessLayer.DataAccess
            Dim v_oDataSet = v_oDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strInquiry)
            If (v_oDataSet.Tables(0).Rows.Count = 0) Then
                v_oDataAccess = Nothing
                Return ""
            ElseIf (v_oDataSet.Tables(0).Rows.Count > 1) Then
                v_oDataAccess = Nothing
                Return GET_SESSIONKEY_ERROR
            End If

            Dim v_intTLID As Integer = v_oDataSet.Tables(0).Rows(0).Item("TLID")
            v_strInquiry = "select sessionkey from tlsession where 1 = 1"
            v_strInquiry = v_strInquiry + "and tlid =to_char('" + v_intTLID.ToString + "') "
            v_strInquiry = v_strInquiry + "and type = 1 "
            v_strInquiry = v_strInquiry + "and txdate = ( select to_date(varvalue,'DD/MM/YYYY') from sysvar where varname ='CURRDATE' and brid ='0001') "

            v_oDataSet = v_oDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strInquiry)
            If (v_oDataSet.Tables(0).Rows.Count = 0) Then
                v_oDataAccess = Nothing
                Return ""
            ElseIf (v_oDataSet.Tables(0).Rows.Count = 1) Then
                Try
                    Dim v_strReturn As String = v_oDataSet.Tables(0).Rows(0).Item("SESSIONKEY")
                    v_oDataAccess = Nothing
                    Return v_strReturn
                Catch ex As Exception
                    Return GET_SESSIONKEY_ERROR
                End Try
            Else
                Return GET_SESSIONKEY_ERROR
            End If
        Catch ex As Exception
            LogError.Write(ex.Message & vbCrLf & "Error connect to: ", EventLogEntryType.Error, _
                           "ServerbussinessCA.GetSessionKey(String)")
            Return GET_SESSIONKEY_ERROR
        End Try
        Return ""
    End Function

    Public Shared Function GetSessionKey_WithDate(ByVal pv_strUserName As String, ByVal pv_strDate As String) As String
        Try
            Dim v_strInquiry = "select tlid from tlprofiles where upper(tlname) = '" + UCase(pv_strUserName) + "'"
            Dim v_oDataAccess = New DataAccessLayer.DataAccess
            Dim v_oDataSet = v_oDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strInquiry)
            Dim v_strTxdate = Mid(pv_strDate, 7, 2) & "/" & Mid(pv_strDate, 5, 2) & "/" & Mid(pv_strDate, 1, 4)
            If (v_oDataSet.Tables(0).Rows.Count = 0) Then
                v_oDataAccess = Nothing
                Return ""
            ElseIf (v_oDataSet.Tables(0).Rows.Count > 1) Then
                v_oDataAccess = Nothing
                Return GET_SESSIONKEY_ERROR
            End If

            Dim v_intTLID As Integer = v_oDataSet.Tables(0).Rows(0).Item("TLID")
            v_strInquiry = "select sessionkey from tlsession where 1 = 1"
            v_strInquiry = v_strInquiry + "and tlid =" + v_intTLID.ToString + " "
            v_strInquiry = v_strInquiry + "and type = 1 "
            v_strInquiry = v_strInquiry + "and txdate = to_date('" + v_strTxdate + "','DD/MM/YYYY')"

            v_oDataSet = v_oDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strInquiry)
            If (v_oDataSet.Tables(0).Rows.Count = 0) Then
                v_oDataAccess = Nothing
                Return ""
            ElseIf (v_oDataSet.Tables(0).Rows.Count = 1) Then
                Try
                    Dim v_strReturn As String = v_oDataSet.Tables(0).Rows(0).Item("SESSIONKEY")
                    v_oDataAccess = Nothing
                    Return v_strReturn
                Catch ex As Exception
                    Return GET_SESSIONKEY_ERROR
                End Try
            Else
                Return GET_SESSIONKEY_ERROR
            End If
        Catch ex As Exception
            LogError.Write(ex.Message & vbCrLf & "Error connect to: ", EventLogEntryType.Error, _
                           "ServerbussinessCA.GetSessionKey(String)")
            Return GET_SESSIONKEY_ERROR
        End Try
        Return ""
    End Function
    '***********************************************************************
    'Input Parameters     :none
    'Return               :String
    'Purpose        	  :Get session key from database if exits, otherwise
    '                      return ""
    'Created date         :17/11/2010
    'Author               :Myvq
    'Last update date     :17/11/2010
    'Last modifying person:Myvq
    '***********************************************************************
    Public Shared Function GetFileCAPath(ByVal pv_intBrid As String) As String
        Try
            Dim v_oDataAccess = New DataAccessLayer.DataAccess

            Dim v_strInquiry = "select * from sysvar where GRNAME = 'VSDFTPSVR' and VARNAME = 'RootPath' and BRID = " & pv_intBrid
            'v_strInquiry = v_strInquiry + pv_intBrid.ToString

            Dim v_strRootPath As String = ""
            Dim v_oDataSet = v_oDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strInquiry)
            v_strRootPath = v_oDataSet.Tables(0).Rows(0).Item("VARVALUE")

            v_strInquiry = "select * from sysvar where GRNAME = 'VSDFTPSVR' and VARNAME = 'RemotePath' and BRID = " & pv_intBrid
            'v_strInquiry = v_strInquiry + pv_intBrid.ToString

            Dim v_strRemotePath As String = ""
            v_oDataSet = v_oDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strInquiry)
            v_strRemotePath = v_oDataSet.Tables(0).Rows(0).Item("VARVALUE")

            Return v_strRootPath & "\" & v_strRemotePath
        Catch ex As Exception
            LogError.Write(ex.Message & vbCrLf & "Error connect to: ", EventLogEntryType.Error, _
                           "ServerbussinessCA.GetFileCAPath(Integer)")
            Return ""
        End Try
        Return ""
    End Function
    '***********************************************************************
    'Input Parameters     :none
    'Return               :String
    'Purpose        	  :Get session key from database if exits, otherwise
    '                      return ""
    'Created date         :11/09/2013
    'Author               :Thanglv
    '***********************************************************************
    Public Shared Function GetFileCAPathSBV(ByVal pv_intBrid As String) As String
        Try
            Dim v_oDataAccess = New DataAccessLayer.DataAccess

            Dim v_strInquiry = "select * from sysvar where GRNAME = 'VSDFTPSBV' and VARNAME = 'RootPath' and BRID = " & pv_intBrid
            'v_strInquiry = v_strInquiry + pv_intBrid.ToString

            Dim v_strRootPath As String = ""
            Dim v_oDataSet = v_oDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strInquiry)
            v_strRootPath = v_oDataSet.Tables(0).Rows(0).Item("VARVALUE")

            v_strInquiry = "select * from sysvar where GRNAME = 'VSDFTPSBV' and VARNAME = 'RemotePath' and BRID = " & pv_intBrid
            'v_strInquiry = v_strInquiry + pv_intBrid.ToString

            Dim v_strRemotePath As String = ""
            v_oDataSet = v_oDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strInquiry)
            v_strRemotePath = v_oDataSet.Tables(0).Rows(0).Item("VARVALUE")

            Return v_strRootPath & "\" & v_strRemotePath
        Catch ex As Exception
            LogError.Write(ex.Message & vbCrLf & "Error connect to: ", EventLogEntryType.Error, _
                           "ServerbussinessCA.GetFileCAPath(Integer)")
            Return ""
        End Try
        Return ""
    End Function
    '***********************************************************************
    'Input Parameters     :none
    'Return               :String
    'Purpose        	  :Get session key from database if exits, otherwise
    '                      return ""
    'Created date         :23/11/2010
    'Author               :Myvq
    'Last update date     :23/11/2010
    'Last modifying person:Myvq
    '***********************************************************************
    Public Shared Function InsertSessionKey(ByVal pv_strUserName As String, ByVal pv_strSessionKey As String) As String
        Try
            Dim v_strInquiry = "select tlid from tlprofiles where tlname = '" + pv_strUserName + "'"
            Dim v_oDataAccess = New DataAccessLayer.DataAccess
            Dim v_oDataSet = v_oDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strInquiry)
            If (v_oDataSet.Tables(0).Rows.Count <> 1) Then
                v_oDataAccess = Nothing
                Return ""
            End If
            Try
                Dim v_intTLID As Integer = v_oDataSet.Tables(0).Rows(0).Item("TLID")

                v_strInquiry = "select seq_tlsession.nextval NEXTVAL from dual "
                v_oDataSet = v_oDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strInquiry)
                Dim v_intAutoId As Integer = v_oDataSet.Tables(0).Rows(0).Item("NEXTVAL")

                v_strInquiry = "select varvalue from sysvar where varname ='CURRDATE' and brid ='0001'"
                v_oDataSet = v_oDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strInquiry)
                Dim v_strCurrDate As String = v_oDataSet.Tables(0).Rows(0).Item("VARVALUE")

                v_strInquiry = "insert into tlsession(autoid, txdate, tlid, sessionkey, type) "
                v_strInquiry = v_strInquiry + "values(" + v_intAutoId.ToString + ", to_date('" + v_strCurrDate + "','dd/MM/yyyy'), " + v_intTLID.ToString + ",'" + pv_strSessionKey + "', 1)"
                v_oDataAccess.BeginTran()
                Dim v_intError = v_oDataAccess.ExecuteNonQuery(CommandType.Text, v_strInquiry)
                If (v_intError <> 0) Then
                    v_oDataAccess.Commit()
                Else
                    v_oDataAccess.Rollback()
                End If
                v_oDataAccess = Nothing
            Catch ex As Exception
                v_oDataAccess.Rollback()
                v_oDataAccess = Nothing
            End Try
            Return ""
        Catch ex As Exception
            LogError.Write(ex.Message & vbCrLf & "Error connect to: ", EventLogEntryType.Error, _
                           "ServerbussinessCA.InsertSessionKey(String,String)")
            Return INSERT_SESSIONKEY_ERROR
        End Try

    End Function
    '***********************************************************************
    'Input Parameters     :pv_strUserName: String
    'Return               :String
    'Purpose        	  :Get user object from VSD with pv_strUserName
    '                      return ""
    'Created date         :18/11/2010
    'Author               :Myvq
    'Last update date     :18/11/2010
    'Last modifying person:Myvq
    '***********************************************************************
    Private Shared Function GetVSDUser(ByVal pv_strUserName As String, ByVal pv_strPassword As String) As DirectoryEntry
        Dim v_oDe As DirectoryEntry
        Try
            v_oDe = New DirectoryEntry(Utility.ADPath, pv_strUserName, pv_strPassword, AuthenticationTypes.Secure)
            Dim deSearch As DirectorySearcher = New DirectorySearcher
            deSearch.SearchRoot = v_oDe
            deSearch.Filter = "(&(objectClass=user)(sAMAccountName=" + pv_strUserName + "))"
            Dim results As SearchResult = deSearch.FindOne()
            If Not (results Is Nothing) Then
                v_oDe = New DirectoryEntry(results.Path, Utility.ADUser, Utility.ADPassword, AuthenticationTypes.Secure)
                Return v_oDe
            Else
                Return Nothing
            End If
        Catch ex As Exception
            LogError.Write(ex.Message & vbCrLf & "Error connect to: " & pv_strUserName & "|******", EventLogEntryType.Error, _
                           "ServerbussinessCA.GetVSDUser(String,String)")
            Return Nothing
        End Try
    End Function
    '***********************************************************************
    'Input Parameters     :none
    'Return               :CertificateServer
    'Purpose        	  :Get BKAV user from VSD user
    'Created date         :17/11/2010
    'Author               :Myvq
    'Last update date     :17/11/2010
    'Last modifying person:Myvq
    '***********************************************************************
    Public Shared Function GetBKAVUser(ByVal pv_strVSDUsername As String, _
                                       ByVal pv_strUsername As String, _
                                       ByVal pv_strPassword As String, _
                                       ByVal pv_strLDAPIP As String, _
                                       ByVal pv_strArrayOgarnization As String, _
                                       ByVal pv_strBKAVDomainComponent As String) As CertificateServer
        Try
            'will be modified in future
            Dim v_oCertServer As New CertificateServer
            Dim v_strDn As String = ""
            Dim v_arrOgarnization As String() = pv_strArrayOgarnization.Split(",")
            Dim v_intErr As Integer = -1

            For Each v_strOgarnization As String In v_arrOgarnization
                'bangpv
                'v_strDn = "cn=" + pv_strUsername + ",ou=" + v_strOgarnization + _
                '    "," + pv_strBKAVDomainComponent
                'end bangpv
                v_strDn = pv_strUsername + "@" + v_strOgarnization + "." + pv_strBKAVDomainComponent


                v_intErr = v_oCertServer.LoadCertificate(pv_strLDAPIP, _
                    pv_strVSDUsername, pv_strPassword, v_strDn)
                If (v_intErr = 0) Then
                    Exit For
                End If
            Next
            If (v_intErr <> 0) Then
                Return Nothing
            End If
            Return v_oCertServer
        Catch ex As Exception
            LogError.Write(ex.Message & vbCrLf & "Error connect to: ", EventLogEntryType.Error, _
                           "ServerbussinessCA.GetBKAVUser(String,String,String,String,String)")
            Return Nothing
        End Try
    End Function
    '***********************************************************************
    'Input Parameters     :pv_strEncryptedSessionKey: String,
    '                      pv_strResult: 
    'Onput Parameters     :pv_strEncryptedSessionKey: String,
    '                      pv_strResult: String
    'Purpose        	  :Get session key from database and error code to 
    '                      pv_result. Return error code
    'Created date         :19/11/2010
    'Author               :Myvq
    'Last update date     :19/11/2010
    'Last modifying person:Myvq
    '***********************************************************************
    Public Shared Sub AfterLogin(ByRef pv_strEncryptedSessionKey As String, _
            ByRef pv_strResult As String, ByVal pv_oCertServer As CertificateServer, _
            ByVal pv_strUserName As String, ByVal pv_oSignServer As SignServer, _
             ByVal pv_strVSDUsername As String, _
                            ByVal pv_strPassword As String, _
                            ByVal pv_strLDAPIP As String, _
                            ByVal pv_strArrayOgarnization As String, _
                            ByVal pv_strBKAVDomainComponent As String)
        Dim v_oSessionKey = New SessionKey
        'Dim err_step As Integer
        Try
            'Get original XML
            pv_oCertServer = GetBKAVUser(pv_strVSDUsername, pv_strUserName, pv_strPassword, _
                                             pv_strLDAPIP, pv_strArrayOgarnization, pv_strBKAVDomainComponent)
            Dim v_strSessionKey As String = GetSessionKey(pv_strUserName)
            If (v_strSessionKey <> "") Then
                v_oSessionKey.LoadSessionKey(v_strSessionKey)
            Else
                Try
                    Dim v_intError = m_oSignServer.DecryptSessionKey( _
                    pv_strEncryptedSessionKey, v_oSessionKey)
                    If (v_intError <> 0) Then
                        pv_strResult = BKAV_SIGN_SERVER_ERROR
                        Return
                    End If
                    ''will be deleted in future
                    'v_oSessionKey.LoadSessionKey(pv_strEncryptedSessionKey)

                    Dim v_strError = InsertSessionKey(pv_strUserName, v_oSessionKey.XmlKey)
                    If (v_strError.StartsWith(HEADER_ERROR)) Then
                        pv_strResult = v_strError
                        Return
                    End If
                Catch ex As Exception
                    pv_strResult = UNDEFINED_ERROR
                    Return
                End Try
            End If

            'Encrypt session key
            Dim v_intErr As Integer = pv_oCertServer.EncryptSessionKey( _
                v_oSessionKey, pv_strEncryptedSessionKey)
            If (v_intErr <> 0) Then
                pv_strResult = BKAV_HEADER_ERROR + v_intErr.ToString
            End If

            'Encrypt result
            pv_strResult = EncryptXML(pv_strResult, v_oSessionKey, m_oSignServer)

            'will be deleted infuture
            'pv_strEncryptedSessionKey = v_oSessionKey.XmlKey
        Catch ex As Exception
            LogError.Write(ex.Message & vbCrLf & "Error connect to: ", EventLogEntryType.Error, _
                           "ServerbussinessCA.AfterLogin(String,String,CertificatServer,String,SignServer)")
            pv_strResult = ARTER_LOGIN_ERROR
        End Try
    End Sub
    Public Shared Function SaveFile(ByVal pv_strFullFileName As String, _
                                   ByVal pv_strFile As String) As Long

        If File.Exists(pv_strFullFileName) Then
            File.Delete(pv_strFullFileName)
        End If
        'write file
        Try
            Dim objReader = New StreamWriter(pv_strFullFileName)
            objReader.Write(pv_strFile)
            objReader.Close()
            Return 0
        Catch ex As Exception
            Return -1
        End Try

    End Function
    'will be deleted in future
    Public Shared Sub OpenHSM(ByRef pv_oSignServer As SignServer, _
                              ByVal pv_intSlotId As Integer, _
                              ByVal pv_strPin As String, _
                              ByVal pv_strPublicKeyName As String, _
                              ByVal pv_strPrivateKeyName As String, _
                              ByVal pv_strCertificateName As String, _
                              ByVal pv_strHsmDllName As String)
        Dim int_err As Integer
        Dim str_err As String = ""
        Dim str_err1 As String = ""
        Try

            If (m_oSignServer Is Nothing) Then
                'will be deleted in future
                'System.Threading.Thread.Sleep(30 * 1000)

                'NEW CODE
                'm_oSignServer = New SignServer
                'm_oSignServer.SlotId = 0
                'm_oSignServer.PIN = "admin"
                'm_oSignServer.PublicKeyName = "VSDTest"
                'm_oSignServer.PrivateKeyName = "VSDTest"
                'm_oSignServer.CertificateName = "VSDTest"

                m_oSignServer = New SignServer
                m_oSignServer.SlotId = pv_intSlotId
                m_oSignServer.PIN = pv_strPin
                m_oSignServer.PublicKeyName = pv_strPublicKeyName
                m_oSignServer.PrivateKeyName = pv_strPrivateKeyName
                m_oSignServer.CertificateName = pv_strPrivateKeyName
                '2014111 bangpv  dinh nghia them duong dan cryptoki.dll
                m_oSignServer.HsmDllName = pv_strHsmDllName

                'end bangpv 
                int_err = m_oSignServer.OpenConnection("1", "2", "3")

                ''OLD CODE
                'm_oSignServer = New SignServer
                'm_oSignServer.SlotId = 0
                'm_oSignServer.PIN = "12345678"
                'm_oSignServer.PublicKeyName = "VSD"
                'm_oSignServer.PrivateKeyName = "VSD"
                'm_oSignServer.CertificateName = "VSD"
                'm_oSignServer.OpenConnection("X", "Y", "Z")
            End If
            pv_oSignServer = m_oSignServer
            str_err1 = pv_oSignServer.ErrorMessage
        Catch ex As Exception
            LogError.Write(ex.Message & vbCrLf & "Error connect to: bkavca", EventLogEntryType.Error, _
                           "ServerbussinessCA.OpenHSM(SignServer)")
            Throw ex
        End Try
    End Sub
    Public Shared Function EncryptFile(ByVal pv_strUsername As String, _
                                       ByVal pv_strPath As String) As String
        Try
            Dim v_oStreamBinary As New FileStream(pv_strPath, FileMode.Open)
            Dim v_oFileInfo As New FileInfo(pv_strPath)
            ' Create a binary stream reader object.
            Dim v_oReaderInput As BinaryReader = New BinaryReader(v_oStreamBinary)

            ' Determine the number of bytes to read.
            Dim v_intLengthFile As Integer = v_oFileInfo.Length

            ' Read the data in a byte array buffer.
            Dim v_arrInputData As Byte() = v_oReaderInput.ReadBytes(v_intLengthFile)

            ' Close the file.
            v_oStreamBinary.Close()
            v_oReaderInput.Close()

            Dim strModified As String = _
                    System.Text.Encoding.Unicode.GetString(v_arrInputData)
            Dim strCombinedData As String = _
                    ServerBussinessCA.CombineData(strModified)
            Return EncryptXML(strCombinedData, pv_strUsername)
        Catch ex As Exception
            LogError.Write(ex.Message & vbCrLf & "Error connect to: ", EventLogEntryType.Error, _
                           "ServerbussinessCA.EncryptFile(String)")
            Return ""
        End Try
    End Function
    Public Shared Function testEncrypt() As String
        Try
            Dim path As String = "C:\ExportFile\07012012\UPCOM07012012.zip"
            Dim path1 As String = "C:\ExportFile\07012012\A_UPCOM07012012.zip"
            ' Open the binary file.
            Dim streamBinary As New FileStream(path, FileMode.Open)
            Dim fileInfo As New FileInfo(path)
            ' Create a binary stream reader object.
            Dim readerInput As BinaryReader = New BinaryReader(streamBinary)

            ' Determine the number of bytes to read.
            Dim lengthFile As Integer = fileInfo.Length

            ' Read the data in a byte array buffer.
            Dim inputData As Byte() = readerInput.ReadBytes(lengthFile)

            ' Close the file.
            streamBinary.Close()
            readerInput.Close()

            Dim strModified As String = _
                    System.Text.Encoding.Unicode.GetString(inputData)

            Dim outputData As Byte() = _
                    System.Text.Encoding.Unicode.GetBytes(strModified)
            Dim oFileStream As System.IO.FileStream
            oFileStream = New System.IO.FileStream(path1, System.IO.FileMode.Create)
            oFileStream.Write(outputData, 0, outputData.Length)
            oFileStream.Close()

        Catch ex As Exception

        End Try
    End Function
    Public Shared Function CheckExistFile(ByVal pv_strDirPath As String, ByVal pv_strFileName As String)
        Dim v_blnCheck As Boolean = False
        Try
            For Each item As String In Directory.GetFiles(pv_strDirPath)
                If Trim(item) = Trim(pv_strFileName) Then
                    v_blnCheck = True
                End If
            Next
            Return v_blnCheck
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'bangpv: Giải mã file từ HNX
    Public Shared Function DecryptTrade_HNX(ByVal v_strFileName As String, ByVal v_strDestFilename As String) As Boolean
        Dim v_strFileSSK As String = v_strFileName & "_Sign.zip_SessionKey.xml"
        Dim v_strFileData As String = v_strFileName & "_Sign.zip.enc"
        Dim v_strFileSignal_HNX As String = v_strFileName & "_Sign.xml"
        'Dim v_strZipEncrypted As String = v_strFileName & "_Sign.zip"
        Try
            Dim v_strAppPath As String = System.AppDomain.CurrentDomain.BaseDirectory
            v_strAppPath = v_strAppPath & "\data"

            Dim v_sskHNX As New SessionKey


            Dim mv_xzipEngine As New ZipEngine
            'Giải nén file nhận về 
            mv_xzipEngine.UnzipFiles(v_strAppPath, v_strDestFilename & ".zip")
            ' Lấy ssk trong file xml 

            Dim v_Stream As New StreamReader(v_strFileSSK)
            Dim v_strSSKEncrypt As String = v_Stream.ReadToEnd
            v_Stream.Close()
            v_Stream = Nothing
            'Giải mã ssk 
            Dim v_intErr As Long = m_oSignServer.DecryptSessionKey(v_strSSKEncrypt, v_sskHNX)
            If v_intErr = -1 Then
                Return False
                Exit Function
            End If
            'đọc file vào 
            Dim v_fStream As System.IO.FileStream
            v_fStream = System.IO.File.OpenRead(v_strFileData)
            Dim len As Long = v_fStream.Length

            Dim v_arrSource As Byte() = New Byte(len - 1) {}
            Dim v_arrTarget As Byte() = Nothing
            Dim intErr As Long = v_fStream.Read(v_arrSource, 0, len)
            v_fStream.Close()
            v_fStream = Nothing

            v_intErr = v_sskHNX.DecryptByte(v_arrSource, v_arrTarget)
            If v_intErr = -1 Then
                Return False
                Exit Function
            End If
            'Verify 


            'Ghi ra lấy file nguyên thủy
            Dim fs As System.IO.FileStream = System.IO.File.Create(v_strAppPath & "\" & v_strDestFilename & ".zip")
            fs.Write(v_arrTarget, 0, v_arrTarget.Length)
            fs.Close()
            fs = Nothing
            'giai nen file 
            mv_xzipEngine.UnzipFiles(v_strAppPath, v_strDestFilename & ".zip")
            'lay chu ky 
            'v_fStream = System.IO.File.OpenRead(v_strFileSignal_HNX)
            v_Stream = New StreamReader(v_strFileSignal_HNX)
            Dim v_strSignal_HNX = v_Stream.ReadToEnd
            ' 
            v_Stream.Close()
            v_Stream = Nothing
            v_fStream = System.IO.File.OpenRead(v_strFileName & ".xml")
            len = v_fStream.Length
            v_arrSource = New Byte(len - 1) {}
            intErr = v_fStream.Read(v_arrSource, 0, len)
            v_fStream.Close()
            v_fStream = Nothing
            If (m_oCertServer Is Nothing) Then
                m_oCertServer = New CertificateServer
            End If

            m_oCertServer.LoadCertificateFromFile(System.AppDomain.CurrentDomain.BaseDirectory & "hnx.cer")
            'veryfile 
            intErr = m_oCertServer.VerifyByte(v_arrSource, v_strSignal_HNX)
            If intErr = -1 Then
                Return False
                Exit Function
            End If
            'delete file 
            If File.Exists(v_strFileName & ".xml") Then
                File.Delete(v_strFileName & ".xml")
            End If
            'If File.Exists(v_strDestFilename & ".zip") Then
            '    File.Delete(v_strDestFilename & ".zip")
            'End If
            If File.Exists(v_strFileSSK) Then
                File.Delete(v_strFileSSK)
            End If
            If File.Exists(v_strFileData) Then
                File.Delete(v_strFileData)
            End If
            If File.Exists(v_strFileSignal_HNX) Then
                File.Delete(v_strFileSignal_HNX)
            End If

            ''Hiện tại bkav chưa có hàm load cert trên server từ file hoặc tương tự nên chưa verify được trên server, mới test verify file này ở client 
            'mv_oCertificateClient.LoadCertificateFromFile(System.AppDomain.CurrentDomain.BaseDirectory & "\hnx.cer")
            'mv_oCertificateClient.VerifyByte ( v_arrTarget,v_strSignal_HNX) 
            Return True
        Catch ex As Exception
            ex.Source = "Host.ServerCA.DecryptPrice_HNX"
            LogError.Write("EXECUTE_TRAN - Error source: " & ex.Source & vbNewLine _
                         & "Error code: System error!" & vbNewLine _
                         & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_HOST)
            Return False
        End Try
    End Function
    'Hanm5: Giai ma file gia tu HNX
    Public Shared Function DecryptPrice_HNX(ByVal v_strFileName As String, ByVal v_strDestFilename As String) As Boolean
        Dim v_strFileSSK As String = v_strFileName & "_Sign.zip_SessionKey.xml"
        Dim v_strFileData As String = v_strFileName & "_Sign.zip.enc"
        Dim v_strFileSignal_HNX As String = v_strFileName & "_Sign.xml"
        'Dim v_strZipEncrypted As String = v_strFileName & "_Sign.zip"  & ".zip"
        Try
            Dim v_strAppPath As String = System.AppDomain.CurrentDomain.BaseDirectory
            v_strAppPath = v_strAppPath & "\price"
            Dim v_sskHNX As New SessionKey
            Dim mv_xzipEngine As New ZipEngine
            'Giải nén file nhận về 
            mv_xzipEngine.UnzipFiles(v_strAppPath, v_strDestFilename & ".zip ")
            ' Lấy ssk trong file xml 

            Dim v_Stream As New StreamReader(v_strFileSSK)
            Dim v_strSSKEncrypt As String = v_Stream.ReadToEnd
            v_Stream.Close()
            v_Stream = Nothing
            'Giải mã ssk 
            Dim v_intErr As Long = m_oSignServer.DecryptSessionKey(v_strSSKEncrypt, v_sskHNX)
            If v_intErr = -1 Then
                Return False
                Exit Function
            End If
            'đọc file vào 
            Dim v_fStream As System.IO.FileStream
            v_fStream = System.IO.File.OpenRead(v_strFileData)
            Dim len As Long = v_fStream.Length

            Dim v_arrSource As Byte() = New Byte(len - 1) {}
            Dim v_arrTarget As Byte() = Nothing
            Dim intErr As Long = v_fStream.Read(v_arrSource, 0, len)
            v_fStream.Close()
            v_fStream = Nothing

            v_intErr = v_sskHNX.DecryptByte(v_arrSource, v_arrTarget)
            If v_intErr = ERR_SYSTEM_START Then
                Return False
                Exit Function
            End If
            'Verify 
            'Ghi ra lấy file nguyên thủy
            Dim fs As System.IO.FileStream = System.IO.File.Create(v_strAppPath & "\" & v_strDestFilename & ".zip ")
            fs.Write(v_arrTarget, 0, v_arrTarget.Length)
            fs.Close()
            fs = Nothing
            'giai nen file 
            mv_xzipEngine.UnzipFiles(v_strAppPath, v_strDestFilename & ".zip ")
            'lay chu ky             
            v_Stream = New StreamReader(v_strFileSignal_HNX)
            Dim v_strSignal_HNX = v_Stream.ReadToEnd
            ' 
            v_Stream.Close()
            v_Stream = Nothing
            v_fStream = System.IO.File.OpenRead(v_strFileName & ".xml")
            len = v_fStream.Length
            v_arrSource = New Byte(len - 1) {}
            intErr = v_fStream.Read(v_arrSource, 0, len)
            v_fStream.Close()
            v_fStream = Nothing
            If (m_oCertServer Is Nothing) Then
                m_oCertServer = New CertificateServer
            End If

            m_oCertServer.LoadCertificateFromFile(System.AppDomain.CurrentDomain.BaseDirectory & "hnx.cer")
            'veryfile 
            intErr = m_oCertServer.VerifyByte(v_arrSource, v_strSignal_HNX)
            If intErr = -1 Then
                Return False
                Exit Function
            End If
            'delete(File)
            If File.Exists(v_strFileName & ".xml") Then
                File.Delete(v_strFileName & ".xml")
            End If
            If File.Exists(v_strFileSSK) Then
                File.Delete(v_strFileSSK)
            End If
            If File.Exists(v_strFileData) Then
                File.Delete(v_strFileData)
            End If
            If File.Exists(v_strFileSignal_HNX) Then
                File.Delete(v_strFileSignal_HNX)
            End If
            Return True
        Catch ex As Exception
            ex.Source = "Host.ServerCA.DecryptPrice_HNX"
            LogError.Write("EXECUTE_TRAN - Error source: " & ex.Source & vbNewLine _
                         & "Error code: System error!" & vbNewLine _
                         & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_HOST)
            Return False
        End Try
    End Function
    'bangpv: Giải mã file freefloat từ HNX
    Public Shared Function DecryptFreeFloat_HNX(ByVal v_strFileName As String, ByVal v_strDestFilename As String) As Boolean
        Dim v_strFileSSK As String = v_strFileName & "_Sign.zip_SessionKey.xml"
        Dim v_strFileData As String = v_strFileName & "_Sign.zip.enc"
        Dim v_strFileSignal_HNX As String = v_strFileName & "_Sign.xml"
        'Dim v_strZipEncrypted As String = v_strFileName & "_Sign.zip"
        Try
            Dim v_strAppPath As String = System.AppDomain.CurrentDomain.BaseDirectory
            v_strAppPath = v_strAppPath & "\data"

            Dim v_sskHNX As New SessionKey


            Dim mv_xzipEngine As New ZipEngine
            'Giải nén file nhận về 
            mv_xzipEngine.UnzipFiles(v_strAppPath, v_strDestFilename & ".zip")
            ' Lấy ssk trong file xml 

            Dim v_Stream As New StreamReader(v_strFileSSK)
            Dim v_strSSKEncrypt As String = v_Stream.ReadToEnd
            v_Stream.Close()
            v_Stream = Nothing
            'Giải mã ssk 
            Dim v_intErr As Long = m_oSignServer.DecryptSessionKey(v_strSSKEncrypt, v_sskHNX)
            If v_intErr = -1 Then
                Return False
                Exit Function
            End If
            'đọc file vào 
            Dim v_fStream As System.IO.FileStream
            v_fStream = System.IO.File.OpenRead(v_strFileData)
            Dim len As Long = v_fStream.Length

            Dim v_arrSource As Byte() = New Byte(len - 1) {}
            Dim v_arrTarget As Byte() = Nothing
            Dim intErr As Long = v_fStream.Read(v_arrSource, 0, len)
            v_fStream.Close()
            v_fStream = Nothing

            v_intErr = v_sskHNX.DecryptByte(v_arrSource, v_arrTarget)
            If v_intErr = -1 Then
                Return False
                Exit Function
            End If
            'Verify 


            'Ghi ra lấy file nguyên thủy
            Dim fs As System.IO.FileStream = System.IO.File.Create(v_strAppPath & "\" & v_strDestFilename & "_1.zip")
            fs.Write(v_arrTarget, 0, v_arrTarget.Length)
            fs.Close()
            fs = Nothing
            'giai nen file 
            mv_xzipEngine.UnzipFiles(v_strAppPath, v_strDestFilename & "_1.zip")
            'lay chu ky 
            'v_fStream = System.IO.File.OpenRead(v_strFileSignal_HNX)
            v_Stream = New StreamReader(v_strFileSignal_HNX)
            Dim v_strSignal_HNX = v_Stream.ReadToEnd
            ' 
            v_Stream.Close()
            v_Stream = Nothing
            v_fStream = System.IO.File.OpenRead(v_strFileName & ".xml")
            len = v_fStream.Length
            v_arrSource = New Byte(len - 1) {}
            intErr = v_fStream.Read(v_arrSource, 0, len)
            v_fStream.Close()
            v_fStream = Nothing
            If (m_oCertServer Is Nothing) Then
                m_oCertServer = New CertificateServer
            End If

            m_oCertServer.LoadCertificateFromFile(System.AppDomain.CurrentDomain.BaseDirectory & "hnx.cer")
            'veryfile 
            intErr = m_oCertServer.VerifyByte(v_arrSource, v_strSignal_HNX)
            If intErr = -1 Then
                Return False
                Exit Function
            End If
            'delete file 
            If File.Exists(v_strFileName & ".xml") Then
                File.Delete(v_strFileName & ".xml")
            End If
            'If File.Exists(v_strDestFilename & ".zip") Then
            '    File.Delete(v_strDestFilename & ".zip")
            'End If
            If File.Exists(v_strFileSSK) Then
                File.Delete(v_strFileSSK)
            End If
            If File.Exists(v_strFileData) Then
                File.Delete(v_strFileData)
            End If
            If File.Exists(v_strFileSignal_HNX) Then
                File.Delete(v_strFileSignal_HNX)
            End If

            ''Hiện tại bkav chưa có hàm load cert trên server từ file hoặc tương tự nên chưa verify được trên server, mới test verify file này ở client 
            'mv_oCertificateClient.LoadCertificateFromFile(System.AppDomain.CurrentDomain.BaseDirectory & "\hnx.cer")
            'mv_oCertificateClient.VerifyByte ( v_arrTarget,v_strSignal_HNX) 
            Return True
        Catch ex As Exception
            Return False
            ex.Source = "Host.ServerCA.DecryptFreeFloat_HNX"
            LogError.Write("EXECUTE_TRAN - Error source: " & ex.Source & vbNewLine _
                         & "Error code: System error!" & vbNewLine _
                         & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_HOST)
            Return ERR_SYSTEM_START
        End Try
    End Function
    'end bangpv
    Public Shared Function ExtractFile1113(ByVal pv_strPath As String, _
                                    ByVal pv_strFullName As String, _
                                    ByVal pv_strBrid As String, _
                                    ByRef pv_lstFileName As List(Of String)) As Long
        Try
            If Not (Directory.Exists(pv_strPath)) Then
                Directory.CreateDirectory(pv_strPath)
            End If
            Dim v_oDocument As New Xml.XmlDocument
            v_oDocument.Load(pv_strFullName)

            Dim v_attrRColl As Xml.XmlAttributeCollection = v_oDocument.DocumentElement.Attributes

            Dim v_strEncryptedSessionKey = CStr(CType(v_attrRColl.GetNamedItem("EncryptedSessionKey"), Xml.XmlAttribute).Value)
            Dim v_oEncrytedData = CStr(CType(v_attrRColl.GetNamedItem("EncrytedData"), Xml.XmlAttribute).Value)

            Dim v_oSessionKey As New SessionKey
            'v_oSessionKey.LoadSessionKey(v_strEncryptedSessionKey)
            'OpenHSM(m_oSignServer)

            Dim v_intErr As Long = m_oSignServer.DecryptSessionKey(v_strEncryptedSessionKey, v_oSessionKey)
            If (v_intErr <> 0) Then
                Return v_intErr
                Exit Function
            End If

            Dim v_strData As String = ""
            v_intErr = v_oSessionKey.DecryptString(v_oEncrytedData, v_strData)

            'Dim v_intErr As Long = v_oSessionKey.DecryptString(v_oEncrytedData, v_strData)
            If (v_intErr <> 0) Then
                Return v_intErr
                Exit Function
            End If

            Dim v_oFileDocument As New Xml.XmlDocument
            v_oFileDocument.LoadXml(v_strData)

            Dim v_attrFileRColl As Xml.XmlAttributeCollection = v_oFileDocument.DocumentElement.Attributes
            Dim v_oAttr As Xml.XmlAttribute
            pv_lstFileName = New List(Of String)
            For Each v_oAttr In v_attrFileRColl
                Dim v_strName As String = ""
                If (pv_strBrid = "0002") Then
                    v_strName = v_oAttr.Name '.Replace("astdl", "_ASTDL_")
                    v_strName = UCase(v_strName)
                    v_strName = Replace(v_strName, UCase("ASTDL"), "_ASTDL_")
                    v_strName = Replace(v_strName, "ASTPT", "_ASTPT_")
                    v_strName = pv_strBrid & v_strName
                Else
                    Dim v_Prefix As String = ""
                    Select Case pv_strBrid
                        Case "0001"
                            v_Prefix = "LISTED"
                        Case "0003"
                            v_Prefix = "UPCOM"
                        Case "0004"
                            v_Prefix = "BOND"
                        Case "0005"
                            v_Prefix = "USDBOND"
                        Case "0006"
                            v_Prefix = "BILL_VND"
                    End Select
                    v_strName = v_oAttr.Name '..Replace(v_Prefix & "_trading_Result", pv_strBrid & "_")
                    v_strName = UCase(v_strName)
                    v_strName = Replace(v_strName, v_Prefix & "_TRADING_RESULT", pv_strBrid & "_")
                End If
                Dim v_strFullName As String = pv_strPath & "\" & v_strName
                Dim v_strFile As String = v_oAttr.Value

                Dim v_oStreamWriter As New StreamWriter(v_strFullName)
                v_oStreamWriter.Write(v_strFile)
                v_oStreamWriter.Close()
                pv_lstFileName.Add(v_strFullName)
            Next
        Catch ex As Exception
            Return -1
        End Try
    End Function
    Public Shared Function ExtractFile2130(ByVal pv_strPath As String, _
                                  ByVal pv_strFullName As String, _
                                  ByVal pv_strBrid As String) As Long
        Try
            If Not (Directory.Exists(pv_strPath)) Then
                Directory.CreateDirectory(pv_strPath)
            End If
            Dim v_oDocument As New Xml.XmlDocument
            v_oDocument.Load(pv_strFullName)
            LogError.Write("ExtractFile2130 - Error source: " & "1:" & vbNewLine _
                                           & "Error code: System error!" & vbNewLine _
                                           & "Error message: " & "pv_strFullName=" & pv_strFullName, EventLogEntryType.Information, gc_MODULE_HOST)
            Dim v_attrRColl As Xml.XmlAttributeCollection = v_oDocument.DocumentElement.Attributes

            Dim v_strEncryptedSessionKey = CStr(CType(v_attrRColl.GetNamedItem("EncryptedSessionKey"), Xml.XmlAttribute).Value)
            Dim v_oEncrytedData = CStr(CType(v_attrRColl.GetNamedItem("EncrytedData"), Xml.XmlAttribute).Value)
            LogError.Write("ExtractFile2130 - Error source: " & "2:" & vbNewLine _
                                           & "Error code: System error!" & vbNewLine _
                                           & "Error message: " & "v_oEncrytedData=" & v_oEncrytedData, EventLogEntryType.Information, gc_MODULE_HOST)
            Dim v_oSessionKey As New SessionKey
            Dim v_intErr As Long = m_oSignServer.DecryptSessionKey(v_strEncryptedSessionKey, v_oSessionKey)
            If (v_intErr <> 0) Then
                Return v_intErr
                Exit Function
            End If

            Dim v_strData As String = ""
            v_intErr = v_oSessionKey.DecryptString(v_oEncrytedData, v_strData)
            If (v_intErr <> 0) Then
                Return v_intErr
                Exit Function
            End If
            LogError.Write("ExtractFile2130 - Error source: " & "3:" & vbNewLine _
                                           & "Error code: System error!" & vbNewLine _
                                           & "Error message: " & "v_strData=" & v_strData, EventLogEntryType.Information, gc_MODULE_HOST)
            Dim v_oFileDocument As New Xml.XmlDocument
            'If pv_strBrid = "0002" Then
            '    v_oFileDocument.LoadXml(v_strData.Replace("<FILE ", "<FILE HSX"))
            'Else
            '    v_oFileDocument.LoadXml(v_strData)
            'End If
            v_oFileDocument.LoadXml(v_strData)
            LogError.Write("ExtractFile2130 - Error source: " & "4:" & vbNewLine _
                                           & "Error code: System error!" & vbNewLine _
                                           & "Error message: " & "v_strData=" & v_strData, EventLogEntryType.Information, gc_MODULE_HOST)
            Dim v_attrFileRColl As Xml.XmlAttributeCollection = v_oFileDocument.DocumentElement.Attributes
            Dim v_oAttr As Xml.XmlAttribute
            For Each v_oAttr In v_attrFileRColl
                Dim v_strName As String = ""
                If pv_strBrid = "0002" Then
                    v_strName = v_oAttr.Name
                    v_strName = Mid(UCase(v_strName), 4)
                Else
                    v_strName = v_oAttr.Name
                    v_strName = UCase(v_strName)
                End If
                Dim v_strFullName As String = pv_strPath & "\" & v_strName.Replace("HSX", "")
                Dim v_strFile As String = v_oAttr.Value
                Dim v_oStreamWriter As New StreamWriter(v_strFullName)
                v_oStreamWriter.Write(v_strFile)
                v_oStreamWriter.Close()
            Next
        Catch ex As Exception
            LogError.Write("ExtractFile2130 - Error source: " & "2130:" & vbNewLine _
                                           & "Error code: System error!" & vbNewLine _
                                           & "Error message: " & ex.ToString(), EventLogEntryType.Information, gc_MODULE_HOST)
            Return -1
        End Try
    End Function
    'bangpv: VN30
    Public Shared Function ExtractFile1123(ByVal pv_strPath As String, _
                                   ByVal pv_strCurrDate As String, _
                                   ByVal pv_strBrid As String, ByVal pv_strTLTXCD As String, ByVal pv_strTLName As String) As Long

        Try
            Dim v_strAppPath As String = System.AppDomain.CurrentDomain.BaseDirectory
            v_strAppPath = v_strAppPath & "data"
            Dim v_strDestFilename As String = pv_strTLTXCD & pv_strBrid & pv_strCurrDate & "_enc.zip"
            Dim v_strFileData As String = v_strAppPath & "\" & pv_strTLTXCD & pv_strBrid & pv_strCurrDate & ".enc"
            Dim v_strFileSSK As String = v_strAppPath & "\" & pv_strTLTXCD & pv_strBrid & pv_strCurrDate & ".dat"
            Dim v_ssk As New SessionKey


            Dim mv_xzipEngine As New ZipEngine
            'Giải nén file nhận về 
            mv_xzipEngine.UnzipFiles(v_strAppPath, v_strDestFilename)
            ' Lấy ssk trong file xml 

            Dim v_Stream As New StreamReader(v_strFileSSK)
            Dim v_strSSKEncrypt As String = v_Stream.ReadToEnd

            'Giải mã ssk 
            Dim v_intErr As Long = m_oSignServer.DecryptSessionKey(v_strSSKEncrypt, v_ssk)
            If v_intErr = -1 Then
                Return False
                Exit Function
            End If
            'đọc file vào 
            Dim v_fStream As System.IO.FileStream
            v_fStream = System.IO.File.OpenRead(v_strFileData)
            Dim len As Long = v_fStream.Length

            Dim v_arrSource As Byte() = New Byte(len - 1) {}
            Dim v_arrTarget As Byte() = Nothing
            Dim intErr As Long = v_fStream.Read(v_arrSource, 0, len)
            v_intErr = v_ssk.DecryptByte(v_arrSource, v_arrTarget)
            If v_intErr = -1 Then
                Return False
                Exit Function
            End If
            'Verify 
            'v_fStream = System.IO.File.OpenRead(v_strFileSignal_HNX)
            'len = v_fStream.Length
            'v_arrSource = New Byte(len - 1) {}
            'lay chu ky 

            'Dim v_strSignal_HNX = v_Stream.ReadToEnd
            'm_oSignServer.VerifyByte(v_arrTarget, )

            v_ssk = New SessionKey

            Dim v_strClientSessionKey As String = _
                   ServerBussinessCA.GetSessionKey(pv_strTLName)
            v_ssk.LoadSessionKey(v_strClientSessionKey)
            v_arrSource = Nothing
            v_ssk.EncryptByte(v_arrTarget, v_arrSource)

            'Ghi ra lấy file da ma hoa bang ssk cua user dang dang nhap 
            Dim fs As System.IO.FileStream = System.IO.File.Create(pv_strPath & "\" & pv_strTLTXCD & pv_strBrid & pv_strCurrDate & "_ENCRYPTED.enc")
            fs.Write(v_arrSource, 0, v_arrSource.Length)
            fs.Close()
            fs = Nothing
            'Ghi lai sesssion key
            If CheckExistFile(pv_strPath, "\" & pv_strTLTXCD & pv_strBrid & pv_strCurrDate & "_ENCRYPTED.dat") Then
                File.Delete(pv_strPath & "\" & pv_strTLTXCD & pv_strBrid & pv_strCurrDate & "_ENCRYPTED.dat")
            End If

            Dim v_oWriter = New StreamWriter(pv_strPath & "\" & pv_strTLTXCD & pv_strBrid & pv_strCurrDate & "_ENCRYPTED.dat")
            v_oWriter.WriteLine(v_strClientSessionKey)
            v_oWriter.Close()

            If mv_xzipEngine.Zip2FileNotDel(pv_strPath & "\" & pv_strTLTXCD & pv_strBrid & pv_strCurrDate & "_ENCRYPTED.enc", _
                                         pv_strPath & "\" & pv_strTLTXCD & pv_strBrid & pv_strCurrDate & "_ENCRYPTED.dat", _
                                         pv_strPath & "\" & pv_strTLTXCD & pv_strBrid & pv_strCurrDate & "_ENCRYPTED.zip") = "" Then
                Return ""
                Exit Function
            End If
            If CheckExistFile(pv_strPath, pv_strTLTXCD & pv_strBrid & pv_strCurrDate & "_ENCRYPTED.dat") Then
                File.Delete(pv_strPath & "\" & pv_strTLTXCD & pv_strBrid & pv_strCurrDate & "_ENCRYPTED.dat")
            End If
            If CheckExistFile(pv_strPath, pv_strTLTXCD & pv_strBrid & pv_strCurrDate & "_ENCRYPTED.enc") Then
                File.Delete(pv_strPath & "\" & pv_strTLTXCD & pv_strBrid & pv_strCurrDate & "_ENCRYPTED.enc")
            End If
            Return True



        Catch ex As Exception
            Return -1
        End Try
    End Function
    'end bangpv

    Public Shared Function ExtractAndSaveFile(ByVal pv_strPath As String, _
                                    ByVal pv_strFullName As String, _
                                    ByVal pv_strTLName As String, _
                                    ByVal pv_strBrid As String, _
                                    ByVal pv_strCurrdate As String, _
                                    ByVal pv_strTltxcd As String) As Long
        Try
            If Not (Directory.Exists(pv_strPath)) Then
                Directory.CreateDirectory(pv_strPath)
            End If
            Dim v_oDocument As New Xml.XmlDocument
            v_oDocument.Load(pv_strFullName)

            Dim v_attrRColl As Xml.XmlAttributeCollection = v_oDocument.DocumentElement.Attributes

            Dim v_strEncryptedSessionKey = CStr(CType(v_attrRColl.GetNamedItem("EncryptedSessionKey"), Xml.XmlAttribute).Value)
            Dim v_oEncrytedData = CStr(CType(v_attrRColl.GetNamedItem("EncrytedData"), Xml.XmlAttribute).Value)

            Dim v_oSessionKey As New SessionKey
            'v_oSessionKey.LoadSessionKey(v_strEncryptedSessionKey)
            'OpenHSM(m_oSignServer)
            Dim v_intErr As Long = m_oSignServer.DecryptSessionKey(v_strEncryptedSessionKey, v_oSessionKey)
            If (v_intErr <> 0) Then
                Return v_intErr
                Exit Function
            End If

            Dim v_strData As String = ""
            If pv_strTltxcd = "1114" Or pv_strTltxcd = "1150" Then
                v_intErr = v_oSessionKey.DecryptString(v_oEncrytedData, v_strData, System.Text.Encoding.Default)
            Else
                v_intErr = v_oSessionKey.DecryptString(v_oEncrytedData, v_strData)
            End If
            'Dim v_intErr As Long = v_oSessionKey.DecryptString(v_oEncrytedData, v_strData)
            If (v_intErr <> 0) Then
                Return v_intErr
                Exit Function
            End If

            'Neu la giao dich 2151 thi luu 1 ban tren Server FTP de import
            If pv_strTltxcd = "2151" Then
                Dim v_oFileDocument As New Xml.XmlDocument
                v_oFileDocument.LoadXml(v_strData)

                Dim v_attrFileRColl As Xml.XmlAttributeCollection = v_oFileDocument.DocumentElement.Attributes
                Dim v_oAttr As Xml.XmlAttribute
                For Each v_oAttr In v_attrFileRColl
                    Dim v_strFullName As String = pv_strPath & "\" & v_oAttr.Name.Replace("abcxyz", " ")
                    Dim v_strFile As String = v_oAttr.Value

                    Dim v_oStreamWriter As New StreamWriter(v_strFullName, False, Text.Encoding.Default)
                    v_oStreamWriter.Write(v_strFile)
                    v_oStreamWriter.Close()
                Next
            End If

            Dim v_strClientSessionKey As String = ServerBussinessCA.GetSessionKey(pv_strTLName)
            v_oSessionKey.LoadSessionKey(v_strClientSessionKey)

            If pv_strTltxcd = "1114" Or pv_strTltxcd = "1150" Then
                v_intErr = v_oSessionKey.EncryptString(v_strData, v_oEncrytedData, System.Text.Encoding.Default)
            Else
                v_intErr = v_oSessionKey.EncryptString(v_strData, v_oEncrytedData)
            End If

            'Dim v_intErr As Long = v_oSessionKey.DecryptString(v_oEncrytedData, v_strData)
            If (v_intErr <> 0) Then
                Return v_intErr
                Exit Function
            End If
            Dim v_oXMLDocument As Xml.XmlDocument = New Xml.XmlDocument
            Dim v_oXMLElement = v_oXMLDocument.CreateElement("DataXML")
            Dim v_oEncryptedString = v_oXMLDocument.CreateAttribute("EncryptedStringXML")
            v_oEncryptedString.Value = v_oEncrytedData
            v_oXMLElement.Attributes.Append(v_oEncryptedString)
            Dim v_oEncryptedSessionKey = v_oXMLDocument.CreateAttribute("EncryptedSessionKeyXML")
            v_oEncryptedSessionKey.Value = v_strClientSessionKey
            v_oXMLElement.Attributes.Append(v_oEncryptedSessionKey)

            v_oXMLDocument.AppendChild(v_oXMLElement)
            If pv_strTltxcd = "2150" Or pv_strTltxcd = "2151" Or pv_strTltxcd = "1150" Then
                v_oXMLDocument.Save(Mid(pv_strFullName, 1, pv_strFullName.Length - 4) & "ENCRYPTED" & ".xml")
            Else
                v_oXMLDocument.Save(pv_strPath & "\" & pv_strTltxcd & pv_strBrid & pv_strCurrdate & "ENCRYPTED" & ".xml")
            End If
            'Dim v_oFileDocument As New Xml.XmlDocument
            'v_oFileDocument.LoadXml(v_strData)

            'Dim v_attrFileRColl As Xml.XmlAttributeCollection = v_oFileDocument.DocumentElement.Attributes
            'Dim v_oAttr As Xml.XmlAttribute
            'For Each v_oAttr In v_attrFileRColl
            '    Dim v_strName As String = v_oAttr.Name
            '    Dim v_strFullName As String = pv_strPath & "\" & v_strName
            '    Dim v_strFile As String = v_oAttr.Value

            '    Dim v_oStreamWriter As New StreamWriter(v_strFullName)
            '    v_oStreamWriter.Write(v_strFile)
            '    v_oStreamWriter.Close()
            'Next
        Catch ex As Exception
            Return -1
            LogError.Write(ex.Message & vbCrLf & "Error connect to: ", EventLogEntryType.Error, _
                          "ServerbussinessCA.ExtractAndSaveFile2150(String,SessionKey,SignServer)")
            Return ENCRYPT_ERROR
        End Try
    End Function

    'Phuc vu gui file tien cua SBL cho NHTT
    Public Shared Function ExtractAndSaveFile2150(ByVal pv_strPath As String, _
                                    ByVal pv_strFullName As String, _
                                    ByVal pv_strTLName As String, _
                                    ByVal pv_strBrid As String, _
                                    ByVal pv_strCurrdate As String, _
                                    ByVal pv_strTltxcd As String) As Long
        Try
            If Not (Directory.Exists(pv_strPath)) Then
                Directory.CreateDirectory(pv_strPath)
            End If
            Dim v_oDocument As New Xml.XmlDocument
            v_oDocument.Load(pv_strFullName)

            Dim v_attrRColl As Xml.XmlAttributeCollection = v_oDocument.DocumentElement.Attributes

            Dim v_strEncryptedSessionKey = CStr(CType(v_attrRColl.GetNamedItem("EncryptedSessionKey"), Xml.XmlAttribute).Value)
            Dim v_oEncrytedData = CStr(CType(v_attrRColl.GetNamedItem("EncrytedData"), Xml.XmlAttribute).Value)

            Dim v_oSessionKey As New SessionKey
            Dim v_intErr As Long = m_oSignServer.DecryptSessionKey(v_strEncryptedSessionKey, v_oSessionKey)
            If (v_intErr <> 0) Then
                Return v_intErr
                Exit Function
            End If

            Dim v_strData As String = ""
            v_intErr = v_oSessionKey.DecryptString(v_oEncrytedData, v_strData)
            If v_intErr <> 0 Then
                Return v_intErr
                Exit Function
            End If

            Dim v_strClientSessionKey As String = ServerBussinessCA.GetSessionKey(pv_strTLName)
            v_oSessionKey.LoadSessionKey(v_strClientSessionKey)

            v_intErr = v_oSessionKey.EncryptString(v_strData, v_oEncrytedData)
            If v_intErr <> 0 Then
                Return v_intErr
                Exit Function
            End If
            Dim v_oXMLDocument As Xml.XmlDocument = New Xml.XmlDocument
            Dim v_oXMLElement = v_oXMLDocument.CreateElement("DataXML")
            Dim v_oEncryptedString = v_oXMLDocument.CreateAttribute("EncryptedStringXML")
            v_oEncryptedString.Value = v_oEncrytedData
            v_oXMLElement.Attributes.Append(v_oEncryptedString)
            Dim v_oEncryptedSessionKey = v_oXMLDocument.CreateAttribute("EncryptedSessionKeyXML")
            v_oEncryptedSessionKey.Value = v_strClientSessionKey
            v_oXMLElement.Attributes.Append(v_oEncryptedSessionKey)

            v_oXMLDocument.AppendChild(v_oXMLElement)

            v_oXMLDocument.Save(pv_strFullName & "ENCRYPTED.xml")
        Catch ex As Exception
            Return -1
            LogError.Write(ex.Message & vbCrLf & "Error connect to: ", EventLogEntryType.Error, _
                          "ServerbussinessCA.ExtractAndSaveFile2150(String,SessionKey,SignServer)")
            Return ENCRYPT_ERROR
        End Try
    End Function
End Class
