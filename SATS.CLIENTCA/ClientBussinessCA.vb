Imports Microsoft.Win32
Imports BkavCASign
Imports System.IO
Imports System.Text
Imports Sats.CommonLibrary
Imports System.Globalization



Public Class ClientBussinessCA
    Public Const HEADER_ERROR As String = "CLIENT_ERROR_"
    Public Const UNDEFINED_ERROR = HEADER_ERROR + "UNDEFINED"
    Public Const BUILD_LOGIN_ERROR As String = HEADER_ERROR + "BUILD_LOGIN"
    Public Const COMBINE_ERROR As String = HEADER_ERROR + "COMBINE"
    Public Const DECOMBINE_ERROR As String = HEADER_ERROR + "DECOMBINE"
    Public Const XML_ERROR As String = HEADER_ERROR + "XML"
    Public Const ENCRYPT_ERROR As String = HEADER_ERROR + "ENCRYPT"
    Public Const BKAV_HEADER_ERROR As String = HEADER_ERROR + "BKAV_"
    Public Const OPEN_USB_ERROR As String = HEADER_ERROR + "OPEN_USB"
    Public Const CLOSE_USB_ERROR As String = HEADER_ERROR + "CLOSE_USB"
    Public Const OPEN_CERTIFICATE_CLIENT_ERROR As String = HEADER_ERROR + "OPEN_CERTIFICATE_CLIENT"

    Private Shared mv_oSignClient As SignClient
    Private Shared mv_oCertificateClient As CertificateClient

    Public Shared Function OpenUSB() As String
        Try
            If (mv_oSignClient Is Nothing) Then
                mv_oSignClient = New SignClient
            End If
            If Not (mv_oSignClient.IsLoaded) Then
                Dim v_intErr = mv_oSignClient.LoadCertificate()
                If (v_intErr <> 0) Then
                    Return OPEN_USB_ERROR
                End If
                Return ""
            End If
        Catch ex As Exception
            Return OPEN_USB_ERROR
        End Try
        Return ""
    End Function
    Public Shared Function getUSB() As SignClient
        Dim v_strErr As String = OpenUSB()
        If (v_strErr.StartsWith(HEADER_ERROR)) Then
            Return Nothing
        End If
        Return mv_oSignClient
    End Function
    Public Shared Function closeUSB() As String
        Try
            If Not (mv_oSignClient Is Nothing) Then
                mv_oSignClient = Nothing
                Return ""
            End If
        Catch ex As Exception
            Return CLOSE_USB_ERROR
        End Try
        Return ""
    End Function
    Public Shared Function OpenCertificateClient() As String
        Try
            If (mv_oCertificateClient Is Nothing) Then
                mv_oCertificateClient = New CertificateClient
            End If
            If Not (mv_oCertificateClient.IsLoaded) Then
                Dim v_intErr = mv_oCertificateClient.LoadCertificate()
                If (v_intErr <> 0) Then
                    Return OPEN_CERTIFICATE_CLIENT_ERROR
                End If
                Return ""
            End If
        Catch ex As Exception
            Return OPEN_CERTIFICATE_CLIENT_ERROR
        End Try
        Return ""
    End Function
    Public Shared Function AddAttribute(ByVal pv_strOrigXML As String, _
                                        ByVal pv_strAttributeName As String, _
                                        ByVal pv_strAttributeValue As String) As String
        Try
            Dim XMLDocumentMessage As New Xml.XmlDocument
            XMLDocumentMessage.LoadXml(pv_strOrigXML)

            'Dim dataElement As Xml.XmlElement

            Dim v_attr = XMLDocumentMessage.CreateAttribute(pv_strAttributeName)
            v_attr.Value = pv_strAttributeValue
            XMLDocumentMessage.DocumentElement.Attributes.Append(v_attr)

            'XMLDocumentMessage.AppendChild(dataElement)
            Return XMLDocumentMessage.InnerXml
        Catch ex As Exception
            Return ""
        End Try
    End Function
    Public Shared Function FileToString(ByVal pv_strFullName As String) As String
        Try
            Dim objReader As StreamReader = New StreamReader(pv_strFullName)
            Dim strContents As String = objReader.ReadToEnd()
            objReader.Close()
            Return strContents
        Catch Ex As Exception
            Return ""
        End Try
    End Function
    '***********************************************************************
    'Input Parameters     :pv_strDirPath String, pv_strFileName String
    'Ouput Parameters     :none
    'Returned value       :Boolean 
    'Purpose        	  :Check file exist 
    'Created date         :02/12/2010
    'Author               :bangpv
    'Last update date     :
    'Last modifying person:bangpv
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
    '***********************************************************************
    'Input Parameters     :pv_strSaveMessage: string, v_strFileName String, v_strLocalDir 
    'Ouput Parameters     :none
    'Returned value       :Long 
    'Purpose        	  :Save file client 
    'Created date         :02/12/2010
    'Author               :bangpv
    'Last update date     :
    'Last modifying person:bangpv
    '***********************************************************************
    Public Shared Function SaveFile(ByVal pv_strSaveMessage As String, ByVal v_strFileName As String, ByVal v_strLocalDir As String) As Long
        Dim v_oWriter As System.IO.StreamWriter
        Try
            Dim f As New IO.DirectoryInfo(v_strLocalDir)
            If Not f.Exists Then
                Directory.CreateDirectory(v_strLocalDir)
            End If
            If CheckExistFile(v_strLocalDir, v_strFileName & ".dat") Then
                File.Delete(v_strLocalDir & "\" & v_strFileName & ".dat")
            End If

            'Xóa file cũ nếu đã tạo 1 lần:
            Dim v_arrFileDel() As String
            v_arrFileDel = v_strFileName.Split("'")
            'Dim v_oWriter As System.IO.StreamWriter

            If File.Exists(v_strLocalDir & "\" & v_strFileName & ".bat") Then
                File.Delete(v_strLocalDir & "\" & v_strFileName & ".bat")
            End If

            v_oWriter = New StreamWriter(v_strLocalDir & "\" & v_strFileName & ".bat")
            v_oWriter.WriteLine("cd " & v_strLocalDir)
            v_oWriter.WriteLine(Left(v_strLocalDir, 2))
            v_oWriter.WriteLine("del " & " " & v_arrFileDel(0) & "'" & v_arrFileDel(1) & "'" & v_arrFileDel(2) & "*.dat")
            v_oWriter.WriteLine("exit ")
            v_oWriter.Close()

            Dim v_oProcess As Process
            v_oProcess = New Process

            v_oProcess.StartInfo.FileName = v_strLocalDir & "\" & v_strFileName & ".bat"
            v_oProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
            v_oProcess.StartInfo.CreateNoWindow = True
            v_oProcess.Start()
            'v_oProcess.WaitForExit()
            v_oProcess.Close()

            System.Threading.Thread.Sleep(3 * 1000)
            'xóa file .bat
            If File.Exists(v_strLocalDir & "\" & v_strFileName & ".bat") Then
                File.Delete(v_strLocalDir & "\" & v_strFileName & ".bat")
            End If

            v_oWriter = New StreamWriter(v_strLocalDir & "\" & v_strFileName & ".dat")
            v_oWriter.WriteLine(pv_strSaveMessage)
            v_oWriter.Close()
            Return 0
        Catch ex As Exception
            Return -1
        Finally
            GC.Collect()
            GC.GetTotalMemory(False)
            GC.Collect()
        End Try

    End Function

    '***********************************************************************
    'Input Parameters     :pv_strUserName: String, pv_strPassword: String
    '                      pv_strIPAddress: String
    'Ouput Parameters     :none
    'Returned value       :String
    'Purpose        	  :Build userName, password, ipaddress to a string
    'Created date         :15/11/2010
    'Author               :Myvq
    'Last update date     :15/11/2010
    'Last modifying person:Myvq
    '***********************************************************************
    Public Shared Function BuildLoginXML(ByVal pv_strUserName As String, _
                ByVal pv_strPassword As String, ByVal pv_strIPAddress As String) As String
        Dim v_strResult = ""
        Try
            v_strResult = "<LoginXML>" _
            & "<UserName>" & pv_strUserName & "</UserName>" _
            & "<Password>" & pv_strPassword & "</Password>" _
            & "<IPAddress>" & pv_strIPAddress & "</IPAddress>" _
            & "</LoginXML>"
            Return v_strResult
        Catch ex As Exception
            Return BUILD_LOGIN_ERROR
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
    Public Shared Function CombineData(ByVal pv_strOrigXML As String) As String ', Optional ByVal pv_strTLNAME As String = "", Optional ByVal pv_strSignCA As String = "") As String
        Dim v_strResult = ""
        Dim v_oXMLDocument = New Xml.XmlDocument
        Try
            'Dim v_oSignServer As New SignServer
            Dim v_intResult As Integer = 0
            'Dim v_oCertClient As New CertificateClient
            'Dim v_oCertServer As New CertificateServer
            'Dim v_oSessionKey As New SessionKey

            'init variable
            'Dim v_oSignClient As SignClient = New SignClient
            Dim v_intError As Integer = 0
            Dim v_strSignNature As String = ""

            ''sign data
            'v_intError = v_oSignClient.LoadCertificate
            'If (v_intError <> 0) Then
            '    Return BKAV_HEADER_ERROR + v_intError.ToString
            'End If
            Try
                OpenUSB()

                v_intError = mv_oSignClient.SignString(pv_strOrigXML, v_strSignNature)

                If (mv_oSignClient.ErrorStatus <> 0) Then
                    Return BKAV_HEADER_ERROR + v_intError.ToString
                End If
            Catch ex As Exception
                Return BKAV_HEADER_ERROR
                LogError.Write(ex.Message & vbCrLf & "Error connect to: " & mv_oSignClient.ErrorMessage & "|" & mv_oSignClient.ErrorStatus, EventLogEntryType.Error, _
                           "ServerbussinessCA.CombineData(String)")

            End Try

            ''combine pv_strOrigXML and its signature
            'v_strResult = "<DataXML>" _
            '& "<OrigXML>" & pv_strOrigXML & "</OrigXML>" _
            '& "<SignatureXML>" & v_strSignNature & "</SignatureXML>" _
            '& "</DataXML>"

            Dim v_oXMLElement As Xml.XmlElement = v_oXMLDocument.CreateElement("DataXML")

            Dim v_strOrigXML = v_oXMLDocument.CreateAttribute("OrigXML")
            v_strOrigXML.Value = pv_strOrigXML
            v_oXMLElement.Attributes.Append(v_strOrigXML)

            Dim v_strSignatureXML = v_oXMLDocument.CreateAttribute("SignatureXML")
            v_strSignatureXML.Value = v_strSignNature
            v_oXMLElement.Attributes.Append(v_strSignatureXML)

            'Dim v_strTLName = v_oXMLDocument.CreateAttribute("TLName")
            'v_strTLName.Value = pv_strTLNAME
            'v_oXMLElement.AppendChild(v_strTLName)

            'Dim v_strSignCA = v_oXMLDocument.CreateAttribute("SignCA")
            'v_strSignCA.Value = pv_strSignCA
            'v_oXMLElement.AppendChild(v_strSignCA)

            v_oXMLDocument.AppendChild(v_oXMLElement)

            'Return v_strResult
            Return v_oXMLDocument.InnerXml
        Catch ex As Exception
            Return COMBINE_ERROR
            LogError.Write(ex.Message & vbCrLf & "Error connect to: " & mv_oSignClient.ErrorMessage & "|" & mv_oSignClient.ErrorStatus, EventLogEntryType.Error, _
                        "ServerbussinessCA.CombineData(String)")
            Return ""
        End Try
    End Function
    Public Shared Function CombineData(ByVal pv_arrByteOrigXML As Byte()) As String ', Optional ByVal pv_strTLNAME As String = "", Optional ByVal pv_strSignCA As String = "") As String
        Dim v_strResult = ""
        Dim v_oXMLDocument = New Xml.XmlDocument
        Try
            'Dim v_oSignServer As New SignServer
            Dim v_intResult As Integer = 0
            'Dim v_oCertClient As New CertificateClient
            'Dim v_oCertServer As New CertificateServer
            'Dim v_oSessionKey As New SessionKey

            'init variable
            'Dim v_oSignClient As SignClient = New SignClient
            Dim v_intError As Integer = 0
            Dim v_strSignNature As String = ""
            'Dim v_arrByteSignature As String = ""

            ''sign data
            'v_intError = v_oSignClient.LoadCertificate
            'If (v_intError <> 0) Then
            '    Return BKAV_HEADER_ERROR + v_intError.ToString
            'End If
            Try
                OpenUSB()
                v_intError = mv_oSignClient.SignByte(pv_arrByteOrigXML, v_strSignNature)
                If (mv_oSignClient.ErrorStatus <> 0) Then
                    Return BKAV_HEADER_ERROR + v_intError.ToString
                End If
            Catch ex As Exception
                Return BKAV_HEADER_ERROR
                LogError.Write(ex.Message & vbCrLf & "Error connect to: ", EventLogEntryType.Error, _
                      "ServerbussinessCA.EncryptFile(String)")
                Return ""
            End Try


            ''combine pv_strOrigXML and its signature
            'v_strResult = "<DataXML>" _
            '& "<OrigXML>" & pv_strOrigXML & "</OrigXML>" _
            '& "<SignatureXML>" & v_strSignNature & "</SignatureXML>" _
            '& "</DataXML>"

            Dim v_oXMLElement As Xml.XmlElement = v_oXMLDocument.CreateElement("DataXML")

            Dim v_strOrigXML = v_oXMLDocument.CreateAttribute("OrigXML")
            v_strOrigXML.Value = System.Text.Encoding.Unicode.GetString(pv_arrByteOrigXML)
            v_oXMLElement.Attributes.Append(v_strOrigXML)

            Dim v_strSignatureXML = v_oXMLDocument.CreateAttribute("SignatureXML")
            v_strSignatureXML.Value = v_strSignNature
            v_oXMLElement.Attributes.Append(v_strSignatureXML)

            'Dim v_strTLName = v_oXMLDocument.CreateAttribute("TLName")
            'v_strTLName.Value = pv_strTLNAME
            'v_oXMLElement.AppendChild(v_strTLName)

            'Dim v_strSignCA = v_oXMLDocument.CreateAttribute("SignCA")
            'v_strSignCA.Value = pv_strSignCA
            'v_oXMLElement.AppendChild(v_strSignCA)

            v_oXMLDocument.AppendChild(v_oXMLElement)

            'Return v_strResult
            Return v_oXMLDocument.InnerXml
        Catch ex As Exception
            Return COMBINE_ERROR
            LogError.Write(ex.Message & vbCrLf & "Error connect to: ", EventLogEntryType.Error, _
                        "ServerbussinessCA.EncryptFile(String)")
            Return ""
        End Try
    End Function
    '***********************************************************************
    'Input Parameters     :pv_strUserName: String, pv_strIPAddress: String
    '                      pv_strPassword: String
    'Ouput Parameters     :pv_oSessionKey: SessionKey, pv_strEncryptString: String
    '                      pv_strEncryptedXML: String, 
    '                      pv_strEncryptedSessionKey: String
    'Returned value       :String
    'Purpose        	  :Combine pv_strUsername, pv_strPassword, pv_strIPAddress
    '                      to v_strOrigXML.
    '                      Sign v_strOrigXML with private key of USB, and we
    '                      have v_strSignature.
    '                      Combine v_strOrigXML and v_strSignature and retun
    '                      to pv_strEncryptedXML.
    '                      pv_strEncryptedXML will be encrypted with pv_oSessionKey
    '                      and return to pv_strEncryptedXML
    '                      pv_oSessionKey will be encrypted with public key of VSD
    '                      and return to pv_strEncryptedSessionKey
    'Created date         :15/11/2010
    'Author               :Myvq
    'Last update date     :15/11/2010
    'Last modifying person:Myvq
    '***********************************************************************
    Public Shared Function PrepareLoginCA(ByVal pv_strUserName As String, _
        ByVal pv_strPassword As String, ByVal pv_strIPAddress As String, _
        ByVal pv_oSessionKey As SessionKey, ByRef pv_strEncryptedXML As String, _
        ByRef pv_strEncryptedSessionKey As String) As String
        pv_strEncryptedXML = ""
        pv_strEncryptedSessionKey = ""
        Try
            'Build login XML
            Dim v_strOrigXML As String = ""
            v_strOrigXML = BuildLoginXML(pv_strUserName, pv_strPassword, pv_strIPAddress)
            If (v_strOrigXML.StartsWith(HEADER_ERROR)) Then
                Return v_strOrigXML
            End If

            'Build pv_strEncryptedXML
            Dim v_strCombinedData = CombineData(v_strOrigXML)
            If (v_strCombinedData.StartsWith(HEADER_ERROR)) Then
                Return v_strCombinedData
            End If

            'Encrypt Data
            Dim v_intError As Integer = _
                pv_oSessionKey.EncryptString(v_strCombinedData, pv_strEncryptedXML)
            If (v_intError <> 0) Then
                Return BKAV_HEADER_ERROR + v_intError.ToString
            End If

            'Dim v_oCertificateClient As CertificateClient = New CertificateClient
            'v_intError = v_oCertificateClient.LoadCertificate()
            'If (v_intError <> 0) Then
            '    Return BKAV_HEADER_ERROR + v_intError.ToString
            'End If
            OpenCertificateClient()
            v_intError = mv_oCertificateClient.EncryptSessionKey(pv_oSessionKey, pv_strEncryptedSessionKey)
            If (v_intError <> 0) Then
                Return BKAV_HEADER_ERROR + v_intError.ToString
            End If

            'will be deleted in future
            'pv_strEncryptedSessionKey = pv_oSessionKey.XmlKey

            'will be deleted in fututre
            'v_intError = pv_oSignServer.EncryptSessionKey( _
            '        pv_oSessionKey, pv_strEncryptedSessionKey)
            'If (v_intError <> 0) Then
            '    Return BKAV_HEADER_ERROR + v_intError.ToString
            'End If
            Return ""
        Catch ex As Exception
            Return UNDEFINED_ERROR
            LogError.Write(ex.Message & vbCrLf & "Error connect to: " & mv_oCertificateClient.ErrorMessage & "|" & mv_oCertificateClient.ErrorStatus, EventLogEntryType.Error, _
                     "ServerbussinessCA.PrepareLoginCA(String)")
        End Try
    End Function
    '***********************************************************************
    'Input Parameters     :pv_oSessionKey: SessionKey, pv_strSessionKey: String
    'Return               :String
    'Purpose        	  :Reset session key string of session key object
    '                      and return error code
    'Created date         :15/11/2010
    'Author               :Myvq
    'Last update date     :15/11/2010
    'Last modifying person:Myvq
    '***********************************************************************
    Public Shared Function UpdateSessionKey(ByVal pv_oSessionKey As SessionKey, _
                        ByVal pv_strSessionKey As String) As String
        Try
            Dim v_oSessionKey As SessionKey = New SessionKey
            'If (pv_oSessionKey.LoadSessionKey(pv_strSessionKey)) Then
            Dim v_intErr As Integer = mv_oSignClient.DecryptSessionKey(pv_strSessionKey, v_oSessionKey)
            If (v_intErr > 0) Then
                pv_oSessionKey.LoadSessionKey(v_oSessionKey.XmlKey)
                Return ""
            Else
                Return BKAV_HEADER_ERROR + "LOAD_SESSION_KEY"
            End If
        Catch ex As Exception
            Return UNDEFINED_ERROR
            LogError.Write(ex.Message & vbCrLf & "Error connect to: " & mv_oSignClient.ErrorMessage & "|" & mv_oSignClient.ErrorStatus, EventLogEntryType.Error, _
                       "ServerbussinessCA.UpdateSK(String)")
        End Try
    End Function
    '***********************************************************************
    'Input Parameters     :pv_strCombinedXML: String
    'Ouput Parameters     :pv_strOrigXML: String, pv_strSignatureXML: String
    'Return               :String
    'Purpose        	  :Decombine pv_strCombinedXML into pv_strOrigXML
    '                      and pv_strSignatureXML
    'Created date         :16/11/2010
    'Author               :Myvq
    'Last update date     :16/11/2010
    'Last modifying person:Myvq
    '***********************************************************************
    Public Shared Function DeCombineData(ByVal pv_strCombinedXML As String, _
        ByRef pv_strOrigXML As String, ByRef pv_strSignatureXML As String) As String
        Try
            pv_strOrigXML = ""
            pv_strSignatureXML = ""

            Dim v_oDocument As New Xml.XmlDocument

            v_oDocument.LoadXml(pv_strCombinedXML)
            Dim v_attrRColl As Xml.XmlAttributeCollection = v_oDocument.DocumentElement.Attributes

            pv_strOrigXML = CStr(CType(v_attrRColl.GetNamedItem("OrigXML"), Xml.XmlAttribute).Value)
            pv_strSignatureXML = CStr(CType(v_attrRColl.GetNamedItem("SignatureXML"), Xml.XmlAttribute).Value)
            'For Each v_oNode As Xml.XmlNode In v_oDocument.SelectNodes("DataXML") 
            'Dim v_strDocument As New System.Xml.XmlDocument
            'Try
            '    v_strDocument.LoadXml(pv_strCombinedXML)
            'Catch ex As Exception
            '    Return XML_ERROR
            'End Try

            'Try
            '    'Get original XML
            '    Dim v_oList = v_strDocument.GetElementsByTagName("OrigXML")
            '    For Each v_oItem As System.Xml.XmlElement In v_oList
            '        pv_strOrigXML = v_oItem.InnerText
            '    Next

            '    'Get signature XML
            '    v_oList = v_strDocument.GetElementsByTagName("SignatureXML")
            '    For Each v_oItem As System.Xml.XmlElement In v_oList
            '        pv_strSignatureXML = v_oItem.InnerText
            '    Next

            Return ""
        Catch ex As Exception
            Return DECOMBINE_ERROR
        End Try
    End Function
    Public Shared Function EncryptFile1113(ByVal pv_strRootDir As String, _
                                       ByVal pv_strBrid As String, _
                                       ByVal pv_strCurrDate As String, _
                                       ByVal pv_oSessionKey As SessionKey) As String
        Try
            'zip file
            Dim v_strCurrDate As String = pv_strCurrDate.Replace("/", "")
            Dim v_strPath As String = _
                pv_strRootDir & pv_strBrid '& "\" & v_strCurrDate
            If Not (Directory.Exists(v_strPath)) Then
                Return ""
                Exit Function
            End If
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
                    v_Prefix = "BIL_VND"
            End Select

            
            'End BangPV
            If (pv_strBrid <> "0002") Then
                Dim v_xZipEngine As New ZipEngine
                v_xZipEngine.UnzipFile(v_strPath & "\", v_Prefix & "_Trading_Result" & v_strCurrDate & ".zip", "*")
            End If

            Dim v_oDi As New IO.DirectoryInfo(v_strPath)
            Dim v_arrFi As IO.FileInfo()
            If (pv_strBrid = "0002") Then
                v_arrFi = v_oDi.GetFiles("*" & v_strCurrDate & ".txt")
            Else
                v_arrFi = v_oDi.GetFiles(v_Prefix & "_Trading_result" & v_strCurrDate & ".xml")
            End If
            'BangPV: Check xem da ton tai file hay chua
            If (v_arrFi Is Nothing Or v_arrFi.Length = 0) Then
                Return ""
                Exit Function
            End If
            'Dim v_oDocument As New Xml.XmlDocument
            'Dim v_oDataElement = v_oDocument.CreateElement("File")
            ''1

            'Dim v_oFi As IO.FileInfo
            'For Each v_oFi In v_arrFi
            '    Dim objReader As StreamReader = New StreamReader(v_oFi.FullName)
            '    Dim strContents As String = objReader.ReadToEnd()
            '    objReader.Close()

            '    Dim v_oAttribute = v_oDocument.CreateAttribute(v_oFi.Name)
            '    v_oAttribute.Value = strContents
            '    v_oDataElement.Attributes.Append(v_oAttribute)
            'Next
            'v_oDocument.AppendChild(v_oDataElement)

            'Dim v_strName As String = v_strPath & "\" & pv_strBrid & v_strCurrDate & ".xml"
            'Dim v_strEncryptedData As String = ""
            'pv_oSessionKey.EncryptString(v_oDocument.InnerXml, v_strEncryptedData)
            'Dim v_strEncryptedSessionKey As String = ""
            'mv_oCertificateClient.EncryptSessionKey(pv_oSessionKey, v_strEncryptedSessionKey)
            ''v_strEncryptedSessionKey = pv_oSessionKey.XmlKey

            'Dim v_oDocumentSave As New Xml.XmlDocument
            'Dim v_oElementSave = v_oDocumentSave.CreateElement("FileEncrypted")

            'Dim v_oEncrytedData = v_oDocumentSave.CreateAttribute("EncrytedData")
            'v_oEncrytedData.Value = v_strEncryptedData
            'v_oElementSave.Attributes.Append(v_oEncrytedData)

            'Dim v_oEncryptedSessionKey = v_oDocumentSave.CreateAttribute("EncryptedSessionKey")
            'v_oEncryptedSessionKey.Value = v_strEncryptedSessionKey
            'v_oElementSave.Attributes.Append(v_oEncryptedSessionKey)

            'v_oDocumentSave.AppendChild(v_oElementSave)
            'v_oDocumentSave.Save(v_strName)

            'Return ClientBussinessCA.EncryptXML(CombineData(strModified), pv_oSessionKey)
            Dim v_strFullEncyptedFileName As String = v_strPath & "\" & "1113" & pv_strBrid & v_strCurrDate & ".xml"
            Dim v_strErr As String = EncryptFile(v_strFullEncyptedFileName, v_arrFi, pv_oSessionKey, "1113")
            If (v_strErr = "") Then
                Return ""
                Exit Function
            End If
            Return v_strFullEncyptedFileName
        Catch ex As Exception
            LogError.Write(ex.Message & vbCrLf & "Error connect to: ", EventLogEntryType.Error, _
                           "ServerbussinessCA.EncryptFile(String)")
            Return ""
        End Try
    End Function
    Public Shared Function EncryptFile2130(ByVal pv_strRootDir As String, _
                                           ByVal pv_strBrid As String, _
                                           ByVal pv_strCurrDate As String, _
                                           ByVal pv_oSessionKey As SessionKey, _
                                           ByVal pv_strPriceType As String) As String
        Try
            Dim v_strCurrDate As String = pv_strCurrDate.Replace("/", "")
            Dim v_strPath As String = pv_strRootDir & pv_strBrid
            If Not (Directory.Exists(v_strPath)) Then
                Return ""
                Exit Function
            End If
            'If pv_strBrid <> "0002" Then
            '    Dim v_xZipEngine As New ZipEngine
            '    v_xZipEngine.UnzipFile(v_strPath & "\", "HNX_STOCKS_PRICE_" & DateTime.ParseExact(v_strCurrDate, "ddMMyyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd", CultureInfo.InvariantCulture) & ".zip", "*")
            '    v_xZipEngine.UnzipFile(v_strPath & "\", "HNX_IDX_STOCKS_" & DateTime.ParseExact(v_strCurrDate, "ddMMyyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd", CultureInfo.InvariantCulture) & ".zip", "*")
            '    v_xZipEngine.UnzipFile(v_strPath & "\", "HNX_BOND_PRICE_YC_" & DateTime.ParseExact(v_strCurrDate, "ddMMyyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd", CultureInfo.InvariantCulture) & ".zip", "*")
            'End If
            Dim v_oDi As New IO.DirectoryInfo(v_strPath)
            Dim v_arrFi As IO.FileInfo()
            If pv_strBrid = "0002" Then
                If pv_strPriceType = "1" Then
                    My.Computer.FileSystem.CopyFile(v_strPath & "\" & v_strCurrDate & "_CLOSE_PRICE_OF_LISTED_STOCKS.txt", v_strPath & "\" & "HSX" & v_strCurrDate & "_CLOSE_PRICE_OF_LISTED_STOCKS.txt")
                    v_arrFi = v_oDi.GetFiles("HSX" & v_strCurrDate & "_CLOSE_PRICE_OF_LISTED_STOCKS.txt")
                    ' bangpv edit: 20220307 add VN30 FSP
                ElseIf pv_strPriceType = "2" Then

                    My.Computer.FileSystem.CopyFile(v_strPath & "\" & v_strCurrDate & "_REFER_PRICE_OF_LISTED_STOCKS.txt", v_strPath & "\" & "HSX" & v_strCurrDate & "_REFER_PRICE_OF_LISTED_STOCKS.txt")
                    v_arrFi = v_oDi.GetFiles("HSX" & v_strCurrDate & "_REFER_PRICE_OF_LISTED_STOCKS.txt")
                    ' TruongNX41 edit: 20240610 add VN100 FSP
                ElseIf pv_strPriceType = "4" Then
                    My.Computer.FileSystem.CopyFile(v_strPath & "\" & v_strCurrDate & "_FSP_VN100.txt", v_strPath & "\" & "HSX" & v_strCurrDate & "_FSP_VN100.txt", True)
                    v_arrFi = v_oDi.GetFiles("HSX" & v_strCurrDate & "_FSP_VN100.txt")
                    ' TruongNX41 edit: 20240610 add VN100 FSP
                Else
                    My.Computer.FileSystem.CopyFile(v_strPath & "\" & v_strCurrDate & "_FSP_VN30.txt", v_strPath & "\" & "HSX" & v_strCurrDate & "_FSP_VN30.txt", True)
                    v_arrFi = v_oDi.GetFiles("HSX" & v_strCurrDate & "_FSP_VN30.txt")
                    ' end bangpv edit: 20220307 add VN30 FSP
                End If
                'ElseIf pv_strBrid = "0001" Then
                '    v_arrFi = v_oDi.GetFiles("HNX_*_" & DateTime.ParseExact(v_strCurrDate, "ddMMyyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd", CultureInfo.InvariantCulture) & ".xml")
            End If
            'Check xem da ton tai file hay chua
            If (v_arrFi Is Nothing Or v_arrFi.Length = 0) Then
                Return ""
                Exit Function
            End If
            '20221228 :Add price type to 0002 
            Dim v_strFullEncyptedFileName As String = v_strPath & "\" & "2130" & pv_strBrid & v_strCurrDate & "_" & pv_strPriceType & ".xml"
            'end BangPV 
            Dim v_strErr As String = EncryptFile(v_strFullEncyptedFileName, v_arrFi, pv_oSessionKey, "2130")
            If (v_strErr = "") Then
                Return ""
                Exit Function
            End If
            Return v_strFullEncyptedFileName
        Catch ex As Exception
            LogError.Write(ex.Message & vbCrLf & "Error connect to: ", EventLogEntryType.Error, _
                           "ClientBussinessCA.EncryptFile2130")
            Return ""
        End Try
    End Function

    Public Shared Function EncryptFileSblFtpBnk(ByVal pv_strFileName As String, _
                                           ByVal pv_strBrid As String, _
                                           ByVal pv_strCurrDate As String, _
                                           ByVal pv_oSessionKey As SessionKey, _
                                           ByVal pv_strTltxcd As String) As String
        Try
            Dim v_fi As New FileInfo(pv_strFileName)
            Dim v_arrFileInfo() As FileInfo = {v_fi}
            Dim v_strFullEncyptedFileName As String = Mid(pv_strFileName, 1, pv_strFileName.Length - 3) & "xml"
            Dim v_strErr As String = EncryptFile(v_strFullEncyptedFileName, v_arrFileInfo, pv_oSessionKey, pv_strTltxcd)
            If v_strErr = "" Then
                Return ""
                Exit Function
            End If
            Return v_strFullEncyptedFileName
        Catch ex As Exception
            LogError.Write(ex.Message & vbCrLf & "Error connect to: ", EventLogEntryType.Error, _
                           "ClientBussinessCA.EncryptFileSblFtpBnk")
            Return ""
        End Try
    End Function

    Public Shared Function EncryptFile1112(ByVal pv_strRootDir As String, _
                                       ByVal pv_strBrid As String, _
                                       ByVal pv_strCurrDate As String, _
                                       ByVal pv_oSessionKey As SessionKey, Optional ByVal v_strVSDSignal As String = "") As String
        Try
            'zip file
            Dim v_strCurrDate As String = pv_strCurrDate.Replace("/", "")
            Dim v_strPath As String = _
                pv_strRootDir & v_strCurrDate
            If Not (Directory.Exists(v_strPath)) Then
                Return ""
                Exit Function
            End If
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
                    'Add TPRL
                Case "0008"
                    v_Prefix = "HNX_BOND_CORP"
                    'end TPRL 
                Case "0009"
                    v_Prefix = "HNX_CACBON"
            End Select
            'add TPRL 
            Dim v_dtCurrDate As DateTime = DateTime.ParseExact(pv_strCurrDate, "dd/MM/yyyy", Globalization.CultureInfo.InvariantCulture)
            Dim v_strCurrDateTPRL As String = v_dtCurrDate.ToString("yyyyMMdd", Globalization.CultureInfo.InvariantCulture)
            'end TPRL 
            'add CacBon 
            Dim v_strCurrDateCacbon As String = v_dtCurrDate.ToString("yyyyMMdd", Globalization.CultureInfo.InvariantCulture)
            'end CacBon 
            'If (pv_strBrid <> "0002") Then
            '    Dim v_xZipEngine As New ZipEngine
            '    v_xZipEngine.UnzipFile(v_strPath & "\", v_Prefix & "_Trading_Result" & v_strCurrDate & ".zip", "*")
            'End If

            Dim di As New IO.DirectoryInfo(v_strPath)
            Dim v_arrFi As IO.FileInfo() = Nothing
            Dim v_strFullEncyptedFileName As String
            If (pv_strBrid = "0002") Then
                v_arrFi = di.GetFiles("FOREIGN_ROOM*" & v_strCurrDate & ".txt")

                v_strFullEncyptedFileName = v_strPath & "\" & "1112" & pv_strBrid & v_strCurrDate & ".xml"
                Dim v_strErr As String = EncryptFile(v_strFullEncyptedFileName, v_arrFi, pv_oSessionKey, "1112")
                If (v_strErr = "") Then
                    Return ""
                    Exit Function
                End If
                If (v_arrFi Is Nothing Or v_arrFi.Length = 0) Then
                    Return ""
                    Exit Function
                End If
            Else
                'TruongNX41 update 18/06/2026
                'TPRL
                If pv_strBrid = "0008" Then
                    v_strFullEncyptedFileName = v_strPath & "\" & v_Prefix & "_" & v_strCurrDateTPRL & ".zip"
                ElseIf pv_strBrid = "0009" Then
                    v_strFullEncyptedFileName = v_strPath & "\" & v_Prefix & "_" & v_strCurrDateCacbon & ".zip"
                Else
                    v_strFullEncyptedFileName = v_strPath & "\" & v_Prefix & v_strCurrDate & ".zip"
                End If
                'TPRL
                'CacBon
                'If pv_strBrid = "0009" Then
                '    v_strFullEncyptedFileName = v_strPath & "\" & v_Prefix & "_" & v_strCurrDateCacbon & ".zip"
                'Else
                '    v_strFullEncyptedFileName = v_strPath & "\" & v_Prefix & v_strCurrDate & ".zip"
                'End If
                'TruongNX41 update 18/06/2026 END
                'Cacbon
                Dim v_strErr As String = EncryptFile_Binary(v_strFullEncyptedFileName, pv_strRootDir, pv_oSessionKey, "1112", v_strVSDSignal)
                If v_strErr = "" Then
                    Return ""
                    Exit Function
                End If

            End If

            Return v_strFullEncyptedFileName
        Catch ex As Exception
            LogError.Write(ex.Message & vbCrLf & "Error connect to: ", EventLogEntryType.Error, _
                           "ServerbussinessCA.EncryptFile(String)")
            Return ""
        End Try
    End Function
    Public Shared Function EncryptFile1132(ByVal pv_strRootDir As String, _
                                      ByVal pv_strBrid As String, _
                                      ByVal pv_strCurrDate As String, _
                                      ByVal pv_oSessionKey As SessionKey, Optional ByVal v_strVSDSignal As String = "") As String
        Try
            'zip file
            Dim v_strCurrDate As String = pv_strCurrDate.Replace("/", "")
            Dim v_strPath As String = _
                pv_strRootDir & v_strCurrDate
            If Not (Directory.Exists(v_strPath)) Then
                Return ""
                Exit Function
            End If
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
            'If (pv_strBrid <> "0002") Then
            '    Dim v_xZipEngine As New ZipEngine
            '    v_xZipEngine.UnzipFile(v_strPath & "\", v_Prefix & "_Trading_Result" & v_strCurrDate & ".zip", "*")
            'End If

            Dim di As New IO.DirectoryInfo(v_strPath)
            Dim v_arrFi As IO.FileInfo() = Nothing
            Dim v_strFullEncyptedFileName As String
            If (pv_strBrid = "0002") Then
                v_arrFi = di.GetFiles("CW*" & v_strCurrDate & ".xml")

                v_strFullEncyptedFileName = v_strPath & "\" & "1132" & pv_strBrid & v_strCurrDate & ".xml"
                Dim v_strErr As String = EncryptFile(v_strFullEncyptedFileName, v_arrFi, pv_oSessionKey, "1132")
                If (v_strErr = "") Then
                    Return ""
                    Exit Function
                End If
                If (v_arrFi Is Nothing Or v_arrFi.Length = 0) Then
                    Return ""
                    Exit Function
                End If
            Else
                v_strFullEncyptedFileName = v_strPath & "\" & v_Prefix & v_strCurrDate & ".zip"
                Dim v_strErr As String = EncryptFile_Binary(v_strFullEncyptedFileName, pv_strRootDir, pv_oSessionKey, "1132", v_strVSDSignal)
                If v_strErr = "" Then
                    Return ""
                    Exit Function
                End If

            End If

            Return v_strFullEncyptedFileName
        Catch ex As Exception
            LogError.Write(ex.Message & vbCrLf & "Error connect to: ", EventLogEntryType.Error, _
                           "ServerbussinessCA.EncryptFile(String)")
            Return ""
        End Try
    End Function

    Public Shared Function EncryptFile1129_1130(ByVal pv_strFileName1 As String, ByVal pv_strFileName2 As String, _
                                       ByVal pv_strBrid As String, _
                                       ByVal pv_strCurrDate As String, _
                                       ByVal pv_oSessionKey As SessionKey, ByVal pv_strTLTXCD As String) As String
        Try
            'zip file
            Dim v_strCurrDate As String = pv_strCurrDate.Replace("/", "")
            'zip file
            Dim v_arrStrFileName1() As String = pv_strFileName1.Split("\")
            Dim v_arrStrFileName2() As String = pv_strFileName2.Split("\")
            Dim v_strFileName1, v_strFileName2 As String
            For i As Integer = 0 To v_arrStrFileName1.Length - 1
                v_strFileName1 = v_arrStrFileName1(i)
            Next

            For i As Integer = 0 To v_arrStrFileName2.Length - 1
                v_strFileName2 = v_arrStrFileName2(i)
            Next

            Dim v_strFileName As String
            If InStr(pv_strFileName1, "VSD") > 0 Then
                v_strFileName = Replace(v_strFileName1, ".xml", "")
            Else
                v_strFileName = Replace(v_strFileName2, ".xml", "")
            End If
            Dim mv_xZipEngine As New ZipEngine
            Dim v_strPath As String = System.AppDomain.CurrentDomain.BaseDirectory & "\Log"
            If Not (Directory.Exists(v_strPath)) Then
                Return ""
                Exit Function
            End If

            'Zip 2 file lai

            If mv_xZipEngine.Zip2FileNotDel(pv_strFileName1, _
                                          pv_strFileName2, v_strPath & "\" & v_strFileName & ".zip") = "" Then
                Return ""
                Exit Function
            End If
            'Dim v_strPath As String = System.AppDomain.CurrentDomain.BaseDirectory & "\Log"
            If Not (Directory.Exists(v_strPath)) Then
                Return ""
                Exit Function
            End If

            Dim v_strFullEncyptedFileName As String

            v_strFullEncyptedFileName = v_strPath & "\" & v_strFileName & ".zip"
            Dim v_strErr As String = EncryptFile_Binary(v_strFullEncyptedFileName, v_strPath, pv_oSessionKey, pv_strTLTXCD)
            If v_strErr = "" Then
                Return ""
                Exit Function
            End If

            Return v_strFullEncyptedFileName
        Catch ex As Exception
            LogError.Write(ex.Message & vbCrLf & "Error connect to: ", EventLogEntryType.Error, _
                           "ServerbussinessCA.EncryptFile(String)")
            Return ""
        End Try
    End Function

    Public Shared Function EncryptFile1114(ByVal pv_strRootDir As String, _
                                   ByVal pv_strBrid As String, _
                                   ByVal pv_strCurrDate As String, _
                                   ByVal pv_oSessionKey As SessionKey, _
                                   ByVal pv_strTltxcd As String, _
                                   Optional ByVal pv_intCsErrType As Integer = 0, _
                                   Optional ByVal pv_strSysDate As String = "01/01/2000") As String
        Try
            'zip file
            If pv_strTltxcd = "1114" Then
                Dim v_strCurrDate As String = pv_strCurrDate.Replace("/", "")
                Dim v_strPath As String = _
                    pv_strRootDir & v_strCurrDate
                If Not (Directory.Exists(v_strPath)) Then
                    Return ""
                    Exit Function
                End If
                Dim v_Prefix As String = ""
                Select Case pv_strBrid
                    Case "0001"
                        v_Prefix = "HNX"
                    Case "0002"
                        v_Prefix = "HOSE"
                    Case "0003"
                        v_Prefix = "UPCOM"
                    Case "0004"
                        v_Prefix = "BOND"
                    Case "0005"
                        v_Prefix = "USDBOND"
                    Case "0006"
                        v_Prefix = "BONDTP"
                    Case "1000"
                        v_Prefix = "HNX_HOSE"
                    Case "1001"
                        v_Prefix = "CP"
                    Case "1004"
                        v_Prefix = "TP"
                End Select
                'If (pv_strBrid <> "0002") Then
                '    Dim v_xZipEngine As New ZipEngine
                '    v_xZipEngine.UnzipFile(v_strPath & "\", v_Prefix & "_Trading_Result" & v_strCurrDate & ".zip", "*")
                'End If

                Dim di As New IO.DirectoryInfo(v_strPath)
                Dim v_arrFi As IO.FileInfo() = Nothing

                v_arrFi = di.GetFiles("NHCDTT_" & pv_strBrid & "_" & v_Prefix & "_*_" & v_strCurrDate & ".txt")

                If (v_arrFi Is Nothing Or v_arrFi.Length = 0) Then
                    Return ""
                    Exit Function
                End If
                Dim v_strFullEncyptedFileName As String = v_strPath & "\" & pv_strTltxcd & pv_strBrid & v_strCurrDate & ".xml"
                Dim v_strErr As String = EncryptFile(v_strFullEncyptedFileName, v_arrFi, pv_oSessionKey, "1114")
                If (v_strErr = "") Then
                    Return ""
                    Exit Function
                End If
                Return v_strFullEncyptedFileName
            ElseIf pv_strTltxcd = "1150" Then
                Dim v_strCurrDate As String = pv_strCurrDate.Replace("/", "")
                Dim v_strSysDate As String = pv_strSysDate.Replace("/", "")
                Dim v_strPath As String = _
                    pv_strRootDir & v_strCurrDate
                If Not (Directory.Exists(v_strPath)) Then
                    Return ""
                    Exit Function
                End If
                Dim v_Prefix As String = ""
                Select Case pv_strBrid
                    Case "0001"
                        v_Prefix = "HNX"
                    Case "0002"
                        v_Prefix = "HOSE"
                    Case "0003"
                        v_Prefix = "UPCOM"
                    Case "0004"
                        v_Prefix = "BOND"
                    Case "0005"
                        v_Prefix = "USDBOND"
                    Case "0006"
                        v_Prefix = "BONDTP"
                    Case "1000"
                        v_Prefix = "HNX_HOSE"
                    Case "1001"
                        v_Prefix = "CP"
                    Case "1004"
                        v_Prefix = "TP"
                End Select
                'If (pv_strBrid <> "0002") Then
                '    Dim v_xZipEngine As New ZipEngine
                '    v_xZipEngine.UnzipFile(v_strPath & "\", v_Prefix & "_Trading_Result" & v_strCurrDate & ".zip", "*")
                'End If

                Dim di As New IO.DirectoryInfo(v_strPath)
                Dim v_arrFi As IO.FileInfo() = Nothing
                If pv_intCsErrType = 1 Then
                    v_arrFi = di.GetFiles("NHCDTT_" & pv_strBrid & "_" & v_Prefix & "_*_" & v_strCurrDate & "_PT.txt")
                ElseIf pv_intCsErrType = 2 Then
                    v_arrFi = di.GetFiles("NHCDTT_" & pv_strBrid & "_" & v_Prefix & "_*_" & v_strCurrDate & "_*_" & v_strSysDate & "_TT.txt")
                ElseIf pv_intCsErrType = 3 Then
                    v_arrFi = di.GetFiles("NHCDTT_" & pv_strBrid & "_" & v_Prefix & "_*_" & v_strCurrDate & "_GT.txt")
                End If
                If (v_arrFi Is Nothing Or v_arrFi.Length = 0) Then
                    Return ""
                    Exit Function
                End If
                Dim v_strEndFile As String
                Select Case pv_intCsErrType
                    Case 1
                        v_strEndFile = "_PT"
                    Case 2
                        v_strEndFile = "_TT"
                    Case 3
                        v_strEndFile = "_GT"
                End Select
                Dim v_strFullEncyptedFileName As String = v_strPath & "\" & pv_strTltxcd & pv_strBrid & v_strCurrDate & v_strEndFile & ".xml"
                Dim v_strErr As String = EncryptFile(v_strFullEncyptedFileName, v_arrFi, pv_oSessionKey, "1150")
                If (v_strErr = "") Then
                    Return ""
                    Exit Function
                End If
                Return v_strFullEncyptedFileName
            Else
                Dim v_strCurrDate As String = pv_strCurrDate.Replace("/", "")
                Dim mv_xZipEngine As New ZipEngine
                Dim v_strPath As String = "C:\ExportFile\Log"
                If Not (Directory.Exists(v_strPath)) Then
                    Return ""
                    Exit Function
                End If

                'Zip 2 file lai

                If mv_xZipEngine.Zip2FileNotDel(pv_strRootDir, _
                                              "", v_strPath & "\1125" & pv_strBrid & v_strCurrDate & ".zip") = "" Then
                    Return ""
                    Exit Function
                End If
                Dim v_fInfo As New FileInfo(v_strPath & "\1125" & pv_strBrid & v_strCurrDate & ".zip")

                Dim v_fStream As System.IO.FileStream
                v_fStream = System.IO.File.OpenRead(v_strPath & "\1125" & pv_strBrid & v_strCurrDate & ".zip")
                Dim len As Long = v_fStream.Length

                Dim v_arrSource As Byte() = New Byte(len - 1) {}
                Dim v_arrTarget As Byte() = Nothing
                Dim intErr As Long = v_fStream.Read(v_arrSource, 0, len)
                'ma hoa 
                intErr = pv_oSessionKey.EncryptByte(v_arrSource, v_arrTarget)
                'ma hoa ssk

                Dim v_strEncryptedSSK As String = ""
                mv_oCertificateClient.EncryptSessionKey(pv_oSessionKey, v_strEncryptedSSK)

                Dim fs As System.IO.FileStream = System.IO.File.Create(v_strPath & "\1125" & pv_strBrid & v_strCurrDate & ".enc")

                fs.Write(v_arrTarget, 0, v_arrTarget.Length)

                fs.Close()

                fs = Nothing

                If CheckExistFile(v_strPath, "\1125" & pv_strBrid & v_strCurrDate & ".dat") Then
                    File.Delete(v_strPath & "\1125" & pv_strBrid & v_strCurrDate & ".dat")
                End If

                Dim v_oWriter = New StreamWriter(v_strPath & "\1125" & pv_strBrid & v_strCurrDate & ".dat")
                v_oWriter.WriteLine(v_strEncryptedSSK)
                v_oWriter.Close()
                'Zip file ma hoa va ssk duoc ma hoa 
                If mv_xZipEngine.Zip2FileNotDel(v_strPath & "\1125" & pv_strBrid & v_strCurrDate & ".enc", _
                                             v_strPath & "\1125" & pv_strBrid & v_strCurrDate & ".dat", _
                                             v_strPath & "\1125" & pv_strBrid & v_strCurrDate & "_enc.zip") = "" Then
                    Return ""
                    Exit Function
                End If

                Return v_strPath & "\1125" & pv_strBrid & v_strCurrDate & "_enc.zip"
            End If
        Catch ex As Exception
            LogError.Write(ex.Message & vbCrLf & "Error connect to: ", EventLogEntryType.Error, _
                           "ServerbussinessCA.EncryptFile1114(String,String,String,String)")
            Return ""
        End Try
    End Function
    'bangpv: 1123,1124
    Public Shared Function EncryptFile1123(ByVal pv_strFileName1 As String, ByVal pv_strFileName2 As String, _
                                       ByVal pv_strBrid As String, _
                                       ByVal pv_strCurrDate As String, _
                                       ByVal pv_oSessionKey As SessionKey) As String
        Try
            'zip file
            Dim v_strCurrDate As String = pv_strCurrDate.Replace("/", "")
            Dim mv_xZipEngine As New ZipEngine
            Dim v_strPath As String = System.AppDomain.CurrentDomain.BaseDirectory & "\Log"
            If Not (Directory.Exists(v_strPath)) Then
                Return ""
                Exit Function
            End If

            'Zip 2 file lai

            If mv_xZipEngine.Zip2FileNotDel(pv_strFileName1, _
                                          pv_strFileName2, v_strPath & "\1123" & pv_strBrid & v_strCurrDate & ".zip") = "" Then
                Return ""
                Exit Function
            End If

            Dim v_fInfo As New FileInfo(v_strPath & "\1123" & pv_strBrid & v_strCurrDate & ".zip")

            Dim v_fStream As System.IO.FileStream
            v_fStream = System.IO.File.OpenRead(v_strPath & "\1123" & pv_strBrid & v_strCurrDate & ".zip")
            Dim len As Long = v_fStream.Length

            Dim v_arrSource As Byte() = New Byte(len - 1) {}
            Dim v_arrTarget As Byte() = Nothing
            Dim intErr As Long = v_fStream.Read(v_arrSource, 0, len)
            'ma hoa 
            intErr = pv_oSessionKey.EncryptByte(v_arrSource, v_arrTarget)
            'ma hoa ssk

            Dim v_strEncryptedSSK As String = ""
            mv_oCertificateClient.EncryptSessionKey(pv_oSessionKey, v_strEncryptedSSK)

            Dim fs As System.IO.FileStream = System.IO.File.Create(v_strPath & "\1123" & pv_strBrid & v_strCurrDate & ".enc")

            fs.Write(v_arrTarget, 0, v_arrTarget.Length)

            fs.Close()

            fs = Nothing
            
            If CheckExistFile(v_strPath, "\1123" & pv_strBrid & v_strCurrDate & ".dat") Then
                File.Delete(v_strPath & "\1123" & pv_strBrid & v_strCurrDate & ".dat")
            End If

            Dim v_oWriter = New StreamWriter(v_strPath & "\1123" & pv_strBrid & v_strCurrDate & ".dat")
            v_oWriter.WriteLine(v_strEncryptedSSK)
            v_oWriter.Close()
            'Zip file ma hoa va ssk duoc ma hoa 
            If mv_xZipEngine.Zip2FileNotDel(v_strPath & "\1123" & pv_strBrid & v_strCurrDate & ".enc", _
                                         v_strPath & "\1123" & pv_strBrid & v_strCurrDate & ".dat", _
                                         v_strPath & "\1123" & pv_strBrid & v_strCurrDate & "_enc.zip") = "" Then
                Return ""
                Exit Function
            End If

            Return v_strPath & "\1123" & pv_strBrid & v_strCurrDate & "_enc.zip"

        Catch ex As Exception
            LogError.Write(ex.Message & vbCrLf & "Error connect to: ", EventLogEntryType.Error, _
                           "ServerbussinessCA.EncryptFile(String)")
            Return ""
        End Try
    End Function
    '1124
    Public Shared Function EncryptFile1124(ByVal pv_strFileName1 As String, ByVal pv_strFileName2 As String, _
                                       ByVal pv_strBrid As String, _
                                       ByVal pv_strCurrDate As String, _
                                       ByVal pv_oSessionKey As SessionKey) As String
        Try
            'zip file
            Dim v_strCurrDate As String = pv_strCurrDate.Replace("/", "")
            Dim mv_xZipEngine As New ZipEngine
            Dim v_strPath As String = System.AppDomain.CurrentDomain.BaseDirectory & "\Log"
            If Not (Directory.Exists(v_strPath)) Then
                Return ""
                Exit Function
            End If

            'Zip 2 file lai

            If mv_xZipEngine.Zip2FileNotDel(pv_strFileName1, _
                                          pv_strFileName2, v_strPath & "\1124" & pv_strBrid & v_strCurrDate & ".zip") = "" Then
                Return ""
                Exit Function
            End If

            Dim v_fInfo As New FileInfo(v_strPath & "\1124" & pv_strBrid & v_strCurrDate & ".zip")

            Dim v_fStream As System.IO.FileStream
            v_fStream = System.IO.File.OpenRead(v_strPath & "\1124" & pv_strBrid & v_strCurrDate & ".zip")
            Dim len As Long = v_fStream.Length

            Dim v_arrSource As Byte() = New Byte(len - 1) {}
            Dim v_arrTarget As Byte() = Nothing
            Dim intErr As Long = v_fStream.Read(v_arrSource, 0, len)
            'ma hoa 
            intErr = pv_oSessionKey.EncryptByte(v_arrSource, v_arrTarget)
            'ma hoa ssk

            Dim v_strEncryptedSSK As String = ""
            mv_oCertificateClient.EncryptSessionKey(pv_oSessionKey, v_strEncryptedSSK)

            Dim fs As System.IO.FileStream = System.IO.File.Create(v_strPath & "\1124" & pv_strBrid & v_strCurrDate & ".enc")

            fs.Write(v_arrTarget, 0, v_arrTarget.Length)

            fs.Close()

            fs = Nothing

            If CheckExistFile(v_strPath, "\1124" & pv_strBrid & v_strCurrDate & ".dat") Then
                File.Delete(v_strPath & "\1124" & pv_strBrid & v_strCurrDate & ".dat")
            End If

            Dim v_oWriter = New StreamWriter(v_strPath & "\1124" & pv_strBrid & v_strCurrDate & ".dat")
            v_oWriter.WriteLine(v_strEncryptedSSK)
            v_oWriter.Close()
            'Zip file ma hoa va ssk duoc ma hoa 
            If mv_xZipEngine.Zip2FileNotDel(v_strPath & "\1124" & pv_strBrid & v_strCurrDate & ".enc", _
                                         v_strPath & "\1124" & pv_strBrid & v_strCurrDate & ".dat", _
                                         v_strPath & "\1124" & pv_strBrid & v_strCurrDate & "_enc.zip") = "" Then
                Return ""
                Exit Function
            End If

            Return v_strPath & "\1124" & pv_strBrid & v_strCurrDate & "_enc.zip"

        Catch ex As Exception
            LogError.Write(ex.Message & vbCrLf & "Error connect to: ", EventLogEntryType.Error, _
                           "ServerbussinessCA.EncryptFile(String)")
            Return ""
        End Try
    End Function
    'end bangpv

    'Added by Thanglv9 - 13/04/2013
    Public Shared Function EncryptFileNHNN(ByVal pv_strFileName As String, _
                                       ByVal pv_strBrid As String, _
                                       ByVal pv_strCurrDate As String, _
                                       ByVal pv_strSysTime As String, _
                                       ByVal pv_oSessionKey As SessionKey) As String
        Try
            'zip file
            Dim v_strCurrDate As String = pv_strCurrDate.Replace("/", "")
            Dim mv_xZipEngine As New ZipEngine
            Dim v_strPath As String = System.AppDomain.CurrentDomain.BaseDirectory & "\Log"
            If Not (Directory.Exists(v_strPath)) Then
                Return ""
                Exit Function
            End If

            'Zip 2 file lai

            If mv_xZipEngine.Zip2FileNotDel(pv_strFileName, "", _
                                          v_strPath & "\1131" & pv_strBrid & v_strCurrDate & pv_strSysTime & ".zip") = "" Then
                Return ""
                Exit Function
            End If

            Dim v_fInfo As New FileInfo(v_strPath & "\1131" & pv_strBrid & v_strCurrDate & pv_strSysTime & ".zip")

            Dim v_fStream As System.IO.FileStream
            v_fStream = System.IO.File.OpenRead(v_strPath & "\1131" & pv_strBrid & v_strCurrDate & pv_strSysTime & ".zip")
            Dim len As Long = v_fStream.Length

            Dim v_arrSource As Byte() = New Byte(len - 1) {}
            Dim v_arrTarget As Byte() = Nothing
            Dim intErr As Long = v_fStream.Read(v_arrSource, 0, len)
            'ma hoa 
            intErr = pv_oSessionKey.EncryptByte(v_arrSource, v_arrTarget)
            'ma hoa ssk

            Dim v_strEncryptedSSK As String = ""
            mv_oCertificateClient.EncryptSessionKey(pv_oSessionKey, v_strEncryptedSSK)

            Dim fs As System.IO.FileStream = System.IO.File.Create(v_strPath & "\1131" & pv_strBrid & v_strCurrDate & pv_strSysTime & ".enc")

            fs.Write(v_arrTarget, 0, v_arrTarget.Length)

            fs.Close()

            fs = Nothing

            If CheckExistFile(v_strPath, "\1131" & pv_strBrid & v_strCurrDate & pv_strSysTime & ".dat") Then
                File.Delete(v_strPath & "\1131" & pv_strBrid & v_strCurrDate & pv_strSysTime & ".dat")
            End If

            Dim v_oWriter = New StreamWriter(v_strPath & "\1131" & pv_strBrid & v_strCurrDate & pv_strSysTime & ".dat")
            v_oWriter.WriteLine(v_strEncryptedSSK)
            v_oWriter.Close()
            'Zip file ma hoa va ssk duoc ma hoa 
            If mv_xZipEngine.Zip2FileNotDel(v_strPath & "\1131" & pv_strBrid & v_strCurrDate & pv_strSysTime & ".enc", _
                                         v_strPath & "\1131" & pv_strBrid & v_strCurrDate & pv_strSysTime & ".dat", _
                                         v_strPath & "\1131" & pv_strBrid & v_strCurrDate & pv_strSysTime & "_enc.zip") = "" Then
                Return ""
                Exit Function
            End If

            Return v_strPath & "\1131" & pv_strBrid & v_strCurrDate & pv_strSysTime & "_enc.zip"

        Catch ex As Exception
            LogError.Write(ex.Message & vbCrLf & "Error connect to: ", EventLogEntryType.Error, _
                           "ServerbussinessCA.EncryptFile(String)")
            Return ""
        End Try
    End Function
    'End Thanglv9

    Public Shared Function EncryptFile(ByVal pv_strFullEncyptedFileName As String, _
                                       ByVal pv_oFi As IO.FileInfo(), _
                                       ByVal pv_oSessionKey As SessionKey, _
                                       ByVal pv_strTltxcd As String) As String
        Try
            Dim v_oDocument As Xml.XmlDocument = New Xml.XmlDocument
            Dim v_oDataElement = v_oDocument.CreateElement("File")

            Dim v_oFi As IO.FileInfo
            For Each v_oFi In pv_oFi
                Dim v_oReader As StreamReader = New StreamReader(v_oFi.FullName)
                If (pv_strTltxcd = "1114" Or pv_strTltxcd = "1150") Then
                    v_oReader = New StreamReader(v_oFi.FullName, Encoding.Default)
                End If
                Dim v_strContents As String = v_oReader.ReadToEnd()
                v_oReader.Close()

                Dim v_oAttribute = v_oDocument.CreateAttribute(v_oFi.Name.Replace(" ", "abcxyz"))
                v_oAttribute.Value = v_strContents
                v_oDataElement.Attributes.Append(v_oAttribute)
            Next
            v_oDocument.AppendChild(v_oDataElement)

            Dim v_strEncryptedData As String = ""
            If pv_strTltxcd = "1114" Or pv_strTltxcd = "1150" Then
                pv_oSessionKey.EncryptString(v_oDocument.InnerXml, v_strEncryptedData, System.Text.Encoding.Default)
            Else
                pv_oSessionKey.EncryptString(v_oDocument.InnerXml, v_strEncryptedData)
            End If
            Dim v_strEncryptedSessionKey As String = ""
            mv_oCertificateClient.EncryptSessionKey(pv_oSessionKey, v_strEncryptedSessionKey)
            'v_strEncryptedSessionKey = pv_oSessionKey.XmlKey

            Dim v_oDocumentSave As New Xml.XmlDocument
            Dim v_oElementSave = v_oDocumentSave.CreateElement("FileEncrypted")

            Dim v_oEncrytedData = v_oDocumentSave.CreateAttribute("EncrytedData")
            v_oEncrytedData.Value = v_strEncryptedData
            v_oElementSave.Attributes.Append(v_oEncrytedData)

            Dim v_oEncryptedSessionKey = v_oDocumentSave.CreateAttribute("EncryptedSessionKey")
            v_oEncryptedSessionKey.Value = v_strEncryptedSessionKey
            v_oElementSave.Attributes.Append(v_oEncryptedSessionKey)

            v_oDocumentSave.AppendChild(v_oElementSave)
            v_oDocumentSave.Save(pv_strFullEncyptedFileName)
            Return pv_strFullEncyptedFileName
        Catch ex As Exception
            LogError.Write("Error source :EncryptFile " & ex.Source & vbNewLine _
                       & "Error code: System error!" & vbNewLine _
                       & "Error message: " & ex.Message & System.AppDomain.CurrentDomain.BaseDirectory & "\hnx.cer", EventLogEntryType.Error, gc_MODULE_HOST)
            Return ""
        End Try

    End Function
    'bangpv
    Public Shared Function EncryptFile_Binary(ByVal pv_strFullEncyptedFileName As String, _
                                              ByVal v_strRootDir As String, _
                                              ByVal pv_oSessionKey As SessionKey, _
                                              ByVal pv_strTltxcd As String, _
                                              Optional ByVal v_strVSDSignal As String = "") As String
        Try
            Dim v_fStream As System.IO.FileStream
            Dim v_strSignal As String = ""
            Dim v_zipEngine As New ZipEngine
            Dim v_strSignalFileName As String = Replace(pv_strFullEncyptedFileName, ".zip", "_Sign.xml")
            Dim v_strSSKEncrypted As String = ""

            v_fStream = System.IO.File.OpenRead(pv_strFullEncyptedFileName)
            Dim len As Long = v_fStream.Length

            Dim v_arrSource As Byte() = New Byte(len - 1) {}
            Dim v_arrTarget As Byte() = Nothing
            Dim intErr As Long = 0
            v_fStream.Read(v_arrSource, 0, len)
            'ký số 

            v_fStream.Close()
            'lấy file ký số
            'intErr = mv_oSignClient.SignByte(v_arrSource, v_strVSDSignal)
            v_strSignal = v_strVSDSignal
            'end bangpv 
            'If intErr <> 0 Then
            '    Return ""
            '    Exit Function
            'End If
            'Lưu file ký số
            Dim v_oWriter As New StreamWriter(v_strSignalFileName)

            v_oWriter.Write(v_strSignal)
            v_oWriter.Close()
            v_oWriter = Nothing

            'Zip file gốc và chữ ký
            If v_zipEngine.Zip2FileNotDel(pv_strFullEncyptedFileName, v_strSignalFileName, _
                                          Replace(pv_strFullEncyptedFileName, ".zip", "_Sign.zip")) = "" Then
                Return ""
                Exit Function
            End If
            'đọc file vừa zip, mã hóa
            v_fStream = System.IO.File.OpenRead(Replace(pv_strFullEncyptedFileName, ".zip", "_Sign.zip"))
            len = v_fStream.Length
            v_arrSource = New Byte(len - 1) {}
            intErr = v_fStream.Read(v_arrSource, 0, len)
            v_fStream.Close()
            'Mã hóa file vừa đọc vào 
            pv_oSessionKey.EncryptByte(v_arrSource, v_arrTarget)
            'ghi ra file 
            Dim fs As FileStream = System.IO.File.Create(Replace(pv_strFullEncyptedFileName, ".zip", "_Sign.zip.enc"))
            fs.Write(v_arrTarget, 0, v_arrTarget.Length)
            fs.Close()
            fs = Nothing
            'mã hóa sessionkey 
            'Load certificate của HNX 

            mv_oCertificateClient.LoadCertificateFromFile(System.AppDomain.CurrentDomain.BaseDirectory & "\hnx.cer")
            'mã hóa SessionKey bằng cert của HNX
            mv_oCertificateClient.EncryptSessionKey(pv_oSessionKey, v_strSSKEncrypted)

            'ghi ra file text
            v_oWriter = New StreamWriter(Replace(pv_strFullEncyptedFileName, ".zip", "_Sign.zip_SessionKey.xml"))

            v_oWriter.Write(v_strSSKEncrypted)
            v_oWriter.Close()
            v_oWriter = Nothing
            'Zip file lại 
            If File.Exists(pv_strFullEncyptedFileName) Then
                File.Delete(pv_strFullEncyptedFileName)
            End If
            v_zipEngine.Zip2FileNotDel(Replace(pv_strFullEncyptedFileName, ".zip", "_Sign.zip.enc"), _
                                       Replace(pv_strFullEncyptedFileName, ".zip", "_Sign.zip_SessionKey.xml"), pv_strFullEncyptedFileName)

            Return pv_strFullEncyptedFileName
        Catch ex As Exception
            LogError.Write("Error source :EncryptFile_Binary " & ex.Source & vbNewLine _
            & "Error code: System error!" & vbNewLine _
            & "Error message: " & ex.Message & System.AppDomain.CurrentDomain.BaseDirectory & "\hnx.cer", EventLogEntryType.Error, gc_MODULE_HOST)
            Return ""
        End Try

    End Function
    'end bangpv
    Public Shared Function ExtractFile1112(ByVal pv_strPath As String, _
                                       ByVal pv_strFullName As String, _
                                       ByVal pv_strBrid As String, _
                                       ByVal pv_strCurrDate As String) As Long
        Try
            If Not (Directory.Exists(pv_strPath)) Then
                Directory.CreateDirectory(pv_strPath)
            End If
            Dim v_oDocument As New Xml.XmlDocument
            v_oDocument.Load(pv_strFullName)

            Dim v_attrRColl As Xml.XmlAttributeCollection = v_oDocument.DocumentElement.Attributes

            Dim v_strEncryptedSessionKey = CStr(CType(v_attrRColl.GetNamedItem("EncryptedSessionKeyXML"), Xml.XmlAttribute).Value)
            Dim v_oEncrytedData = CStr(CType(v_attrRColl.GetNamedItem("EncryptedStringXML"), Xml.XmlAttribute).Value)

            Dim v_oSessionKey As New SessionKey
            v_oSessionKey.LoadSessionKey(v_strEncryptedSessionKey)

            Dim v_strData As String = ""
            Dim v_intErr As Long = v_oSessionKey.DecryptString(v_oEncrytedData, v_strData)
            If (v_intErr <> 0) Then
                Return v_intErr
                Exit Function
            End If

            Dim v_oFileDocument As New Xml.XmlDocument
            v_oFileDocument.LoadXml(v_strData)

            Dim v_attrFileRColl As Xml.XmlAttributeCollection = v_oFileDocument.DocumentElement.Attributes
            Dim v_oAttr As Xml.XmlAttribute
            For Each v_oAttr In v_attrFileRColl
                Dim v_strFullName As String = pv_strPath & "\" & v_oAttr.Name
                Dim v_strFile As String = v_oAttr.Value

                Dim v_oStreamWriter As New StreamWriter(v_strFullName)
                v_oStreamWriter.Write(v_strFile)
                v_oStreamWriter.Close()
            Next
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

            'zip file
            If (pv_strBrid <> "0002") Then
                Dim mv_xZipEngine As New ZipEngine
                mv_xZipEngine.ZipAllFileNotDel(pv_strPath & "\", v_Prefix & pv_strCurrDate & ".zip")
                Dim xmlList As String() = Directory.GetFiles(pv_strPath, "*.xml")
                For Each f As String In xmlList
                    File.Delete(f)
                Next

                'File.Delete(pv_strPath & "*.xml")
            End If
        Catch ex As Exception
            Return -1
        End Try
    End Function
    '1132
    Public Shared Function ExtractFile1132(ByVal pv_strPath As String, _
                                       ByVal pv_strFullName As String, _
                                       ByVal pv_strBrid As String, _
                                       ByVal pv_strCurrDate As String) As Long
        Try
            If Not (Directory.Exists(pv_strPath)) Then
                Directory.CreateDirectory(pv_strPath)
            End If
            Dim v_oDocument As New Xml.XmlDocument
            v_oDocument.Load(pv_strFullName)

            Dim v_attrRColl As Xml.XmlAttributeCollection = v_oDocument.DocumentElement.Attributes

            Dim v_strEncryptedSessionKey = CStr(CType(v_attrRColl.GetNamedItem("EncryptedSessionKeyXML"), Xml.XmlAttribute).Value)
            Dim v_oEncrytedData = CStr(CType(v_attrRColl.GetNamedItem("EncryptedStringXML"), Xml.XmlAttribute).Value)

            Dim v_oSessionKey As New SessionKey
            v_oSessionKey.LoadSessionKey(v_strEncryptedSessionKey)

            Dim v_strData As String = ""
            Dim v_intErr As Long = v_oSessionKey.DecryptString(v_oEncrytedData, v_strData)
            If (v_intErr <> 0) Then
                Return v_intErr
                Exit Function
            End If

            Dim v_oFileDocument As New Xml.XmlDocument
            v_oFileDocument.LoadXml(v_strData)

            Dim v_attrFileRColl As Xml.XmlAttributeCollection = v_oFileDocument.DocumentElement.Attributes
            Dim v_oAttr As Xml.XmlAttribute
            For Each v_oAttr In v_attrFileRColl
                Dim v_strFullName As String = pv_strPath & "\" & v_oAttr.Name
                Dim v_strFile As String = v_oAttr.Value

                Dim v_oStreamWriter As New StreamWriter(v_strFullName)
                v_oStreamWriter.Write(v_strFile)
                v_oStreamWriter.Close()
            Next
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

            'zip file
            If (pv_strBrid <> "0002") Then
                Dim mv_xZipEngine As New ZipEngine
                mv_xZipEngine.ZipAllFileNotDel(pv_strPath & "\", v_Prefix & pv_strCurrDate & ".zip")
                Dim xmlList As String() = Directory.GetFiles(pv_strPath, "*.xml")
                For Each f As String In xmlList
                    File.Delete(f)
                Next

                'File.Delete(pv_strPath & "*.xml")
            End If
        Catch ex As Exception
            Return -1
        End Try
    End Function
    Public Shared Function ExtractFile1114(ByVal pv_strPath As String, _
                                   ByVal pv_strFullName As String, _
                                   ByVal pv_strBrid As String, _
                                   ByVal pv_strCurrDate As String) As Long
        Try
            If Not (Directory.Exists(pv_strPath)) Then
                Directory.CreateDirectory(pv_strPath)
            End If
            'neu la giao dich 114

            Dim v_oDocument As New Xml.XmlDocument
            v_oDocument.Load(pv_strFullName)

            Dim v_attrRColl As Xml.XmlAttributeCollection = v_oDocument.DocumentElement.Attributes

            Dim v_strEncryptedSessionKey = CStr(CType(v_attrRColl.GetNamedItem("EncryptedSessionKeyXML"), Xml.XmlAttribute).Value)
            Dim v_oEncrytedData = CStr(CType(v_attrRColl.GetNamedItem("EncryptedStringXML"), Xml.XmlAttribute).Value)
            
            Dim v_oSessionKey As New SessionKey

            v_oSessionKey.LoadSessionKey(v_strEncryptedSessionKey)

            Dim v_strData As String = ""
            Dim v_intErr As Long = v_oSessionKey.DecryptString(v_oEncrytedData, v_strData, System.Text.Encoding.Default)
            If (v_intErr <> 0) Then
                Return v_intErr
                Exit Function
            End If

            Dim v_oFileDocument As New Xml.XmlDocument
            v_oFileDocument.LoadXml(v_strData)

            Dim v_attrFileRColl As Xml.XmlAttributeCollection = v_oFileDocument.DocumentElement.Attributes
            Dim v_oAttr As Xml.XmlAttribute
            For Each v_oAttr In v_attrFileRColl
                Dim v_strFullName As String = pv_strPath & "\" & v_oAttr.Name.Replace("abcxyz", " ")
                Dim v_strFile As String = v_oAttr.Value

                Dim v_oStreamWriter As New StreamWriter(v_strFullName, False, Encoding.Default)
                v_oStreamWriter.Write(v_strFile)
                v_oStreamWriter.Close()
            Next
            Return 0
        Catch ex As Exception
            Return -1
        End Try
    End Function

    Public Shared Function ExtractFile1125(ByVal pv_strPath As String, _
                                  ByVal pv_strFullName As String, _
                                  ByVal pv_strBrid As String, _
                                  ByVal pv_strCurrDate As String) As Long
        Try
            If Not (Directory.Exists(pv_strPath)) Then
                Directory.CreateDirectory(pv_strPath)
            End If
            'neu la giao dich 114
            Dim v_strPathGetFile As String = "C:\GetFileFTP\" & pv_strBrid & "\" & pv_strCurrDate

            Dim v_strDestFilename As String = "1125" & pv_strBrid & pv_strCurrDate & "_ENCRYPTED.zip"
            Dim v_strFileData As String = v_strPathGetFile & "\" & "1125" & pv_strBrid & pv_strCurrDate & "_ENCRYPTED.enc"
            Dim v_strFileSSK As String = v_strPathGetFile & "\" & "1125" & pv_strBrid & pv_strCurrDate & "_ENCRYPTED.dat"
            Dim v_ssk As New SessionKey


            Dim mv_xzipEngine As New ZipEngine
            'Giải nén file nhận về 
            mv_xzipEngine.UnzipFiles(v_strPathGetFile, v_strDestFilename)
            ' Lấy ssk trong file xml 

            Dim v_Stream As New StreamReader(v_strFileSSK)
            Dim v_strSSKEncrypt As String = v_Stream.ReadToEnd
            v_ssk = New SessionKey


            v_ssk.LoadSessionKey(v_strSSKEncrypt)
            'Giải mã ssk 
            Dim v_intErr As Long = 0
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
           
            'Ghi ra lấy file da ma hoa bang ssk cua user dang dang nhap 
            Dim fs As System.IO.FileStream = System.IO.File.Create(pv_strPath & "\" & "1125" & pv_strBrid & pv_strCurrDate & ".zip")
            fs.Write(v_arrTarget, 0, v_arrTarget.Length)
            fs.Close()
            fs = Nothing
            'giai nen 
            mv_xzipEngine.UnzipFiles(pv_strPath, "1125" & pv_strBrid & pv_strCurrDate & ".zip")
            File.Delete(pv_strPath & "\1125" & pv_strBrid & pv_strCurrDate & ".zip")
            'thong bao dau ra
            MsgBox("Tệp tin được lưu tại thư mục: " & pv_strPath, MsgBoxStyle.Information)

            Return True
        Catch ex As Exception
            Return -1
        End Try
    End Function

    Public Shared Function ExtractFile1123_1124(ByVal pv_strPath As String, _
                                 ByVal pv_strFullName As String, _
                                 ByVal pv_strBrid As String, _
                                 ByVal pv_strCurrDate As String, _
                                ByVal pv_strTLTXCD As String) As Long
        Try
            If Not (Directory.Exists(pv_strPath)) Then
                Directory.CreateDirectory(pv_strPath)
            End If
            'neu la giao dich 114
            Dim v_strPathGetFile As String = "C:\GetFileFTP\" & pv_strBrid & "\" & pv_strCurrDate

            Dim v_strDestFilename As String = pv_strTLTXCD & pv_strBrid & pv_strCurrDate & "_ENCRYPTED.zip"
            Dim v_strFileData As String = v_strPathGetFile & "\" & pv_strTLTXCD & pv_strBrid & pv_strCurrDate & "_ENCRYPTED.enc"
            Dim v_strFileSSK As String = v_strPathGetFile & "\" & pv_strTLTXCD & pv_strBrid & pv_strCurrDate & "_ENCRYPTED.dat"
            Dim v_ssk As New SessionKey


            Dim mv_xzipEngine As New ZipEngine
            'Giải nén file nhận về 
            mv_xzipEngine.UnzipFiles(v_strPathGetFile, v_strDestFilename)
            ' Lấy ssk trong file xml 

            Dim v_Stream As New StreamReader(v_strFileSSK)
            Dim v_strSSKEncrypt As String = v_Stream.ReadToEnd
            v_ssk = New SessionKey


            v_ssk.LoadSessionKey(v_strSSKEncrypt)
            'Giải mã ssk 
            Dim v_intErr As Long = 0
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

            'Ghi ra lấy file da ma hoa bang ssk cua user dang dang nhap 
            Dim fs As System.IO.FileStream = System.IO.File.Create(pv_strPath & "\" & pv_strTLTXCD & pv_strBrid & pv_strCurrDate & ".zip")
            fs.Write(v_arrTarget, 0, v_arrTarget.Length)
            fs.Close()
            fs = Nothing
            'giai nen 
            mv_xzipEngine.UnzipFiles(pv_strPath, pv_strTLTXCD & pv_strBrid & pv_strCurrDate & ".zip")
            File.Delete(pv_strPath & "\" & pv_strTLTXCD & pv_strBrid & pv_strCurrDate & ".zip")
            'thong bao dau ra
            MsgBox("Tệp tin được lưu tại thư mục: " & pv_strPath, MsgBoxStyle.Information)

            Return True
        Catch ex As Exception
            LogError.Write("Error source : " & ex.Source & vbNewLine _
             & "Error code: System error!" & vbNewLine _
             & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_HOST)
            Throw ex
            Return -1
        End Try
    End Function

    Public Shared Function EncryptXML(ByVal pv_strOrigXML As String, _
                            ByVal pv_oSessionKey As SessionKey, _
                            Optional ByVal pv_strTLName As String = "") As String
        Dim v_strResult As String = ""
        Dim v_oXMLDocument = New Xml.XmlDocument
        Try
            'Dim v_strCombinedData = CombineData(pv_strOrigXML)
            'If (v_strCombinedData.StartsWith(HEADER_ERROR)) Then
            '    Return v_strCombinedData
            'End If

            Dim v_intError As Integer = _
                 pv_oSessionKey.EncryptString(pv_strOrigXML, v_strResult)
            If (v_intError <> 0) Then
                Return BKAV_HEADER_ERROR + v_intError.ToString
            Else
                'Return v_strResult
                Dim v_oXMLElement = v_oXMLDocument.CreateElement("DataXML")
                If pv_strTLName <> "" Then
                    Dim v_oTLName = v_oXMLDocument.CreateAttribute("TLName")
                    v_oTLName.Value = pv_strTLName
                    v_oXMLElement.Attributes.Append(v_oTLName)
                End If

                'Dim v_oSignCA = v_oXMLDocument.CreateAttribute("SignCA")
                'v_oSignCA.Value = pv_strSignCA
                'v_oXMLElement.AppendChild(v_oSignCA)


                Dim v_oResult = v_oXMLDocument.CreateAttribute("EncryptedXML")
                v_oResult.Value = v_strResult
                v_oXMLElement.Attributes.Append(v_oResult)
                v_oXMLDocument.AppendChild(v_oXMLElement)
                Return v_oXMLDocument.InnerXml
            End If
        Catch ex As Exception
            Return ENCRYPT_ERROR
            LogError.Write(ex.Message & vbCrLf & "Error connect to: " & mv_oSignClient.ErrorMessage & "|" & mv_oSignClient.ErrorStatus, EventLogEntryType.Error, _
                       "ServerbussinessCA.CombineData(String)")
        Finally
            GC.Collect()
            GC.GetTotalMemory(False)
            GC.Collect()
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
        Try
            'Decrypt pv_strEncryptedXML with session key
            Dim v_strCombinedXML As String = ""
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

            'Verify original XML and its signature
            'Dim v_oCertClient As New CertificateClient
            'v_oCertClient.LoadCertificate()
            OpenCertificateClient()
            'Dim v_intVPN = System.Configuration.ConfigurationManager.AppSettings("IsMember")
            Dim bln_chkOCSP As Boolean
            If System.Configuration.ConfigurationSettings.AppSettings("CheckOCSP") = 1 Then
                bln_chkOCSP = True
            Else
                bln_chkOCSP = False
            End If
            If bln_chkOCSP = True Then
                v_intError = mv_oCertificateClient.VerifyString(v_strOrigXML, v_strSignature, bln_chkOCSP)
            Else
                v_intError = mv_oCertificateClient.VerifyString(v_strOrigXML, v_strSignature)
            End If
            If (v_intError <> 0) Then
                Return BKAV_HEADER_ERROR + v_intError.ToString
            End If

            Return v_strOrigXML
        Catch ex As Exception
            LogError.Write(ex.Message & vbCrLf & "Error connect to: " & mv_oSignClient.ErrorMessage & "|" & mv_oSignClient.ErrorStatus, EventLogEntryType.Error, _
                       "ServerbussinessCA.DEcryptxml(String)")
            Return ""
        End Try
    End Function

    Public Shared Function DecryptFile(ByVal pv_strFullFileName As String, _
                                       ByVal pv_strEncryptedFile As String, _
                                       ByVal pv_oSessionKey As SessionKey) As Long

        Dim v_strFile As String = DecryptXML(pv_strEncryptedFile, pv_oSessionKey)

        If (v_strFile = "") Then
            Return -1
            Exit Function
        End If

        If File.Exists(pv_strFullFileName) Then
            File.Delete(pv_strFullFileName)
        End If
        'write file
        Try
            Dim v_strOutputData As Byte() = _
                        System.Text.Encoding.Unicode.GetBytes(v_strFile)
            Dim v_oFileStream As System.IO.FileStream
            v_oFileStream = New System.IO.FileStream(pv_strFullFileName, System.IO.FileMode.Create)
            v_oFileStream.Write(v_strOutputData, 0, v_strOutputData.Length)
            v_oFileStream.Close()
            Return 0
        Catch ex As Exception
            Return -1
        End Try

    End Function

    '***********************************************************************
    'Input Parameters     :pv_strEncryptedSessionKey: String, 
    '                      pv_strResult: String
    'Output Parameters    :pv_strEncryptedSessionKey: String, 
    '                      pv_strResult: String
    'Purpose        	  :Get session key and update
    'Created date         :19/11/2010
    'Author               :Myvq
    'Last update date     :19/11/2010
    'Last modifying person:Myvq
    '***********************************************************************
    Public Shared Sub AfterLogin(ByVal pv_strEncryptedSessionKey As String, _
                                 ByRef pv_oSessionKey As SessionKey, _
                                 ByRef pv_strResult As String)
        Try
            'Dim v_oSignClient = New SignClient
            'v_oSignClient.LoadCertificate()
            If (pv_strResult <> Nothing And pv_strResult.StartsWith("SERVER_ERROR")) Then
                Return
            End If

            'Get session key
            OpenUSB()
            Dim v_intError = mv_oSignClient.DecryptSessionKey( _
                pv_strEncryptedSessionKey, pv_oSessionKey)
            If (v_intError <> 0) Then
                pv_strResult = "SERVER_ERROR_BKAV_" + v_intError.ToString
                closeUSB()
                Return
            End If

            'will be deleted infuture             
            'pv_oSessionKey.LoadSessionKey(pv_strEncryptedSessionKey)

            'Get result
            pv_strResult = DecryptXML(pv_strResult, pv_oSessionKey)

            ''update session key
            'Dim v_strErr As String = UpdateSessionKey(pv_oSessionKey, pv_strEncryptedSessionKey)
            'If (v_strErr.StartsWith(HEADER_ERROR)) Then
            '    pv_strResult = v_strErr
            'End If
        Catch ex As Exception
            pv_strResult = UNDEFINED_ERROR
            LogError.Write(ex.Message & vbCrLf & "Error connect to: " & mv_oSignClient.ErrorMessage & "|" & mv_oSignClient.ErrorStatus, EventLogEntryType.Error, _
                       "ServerbussinessCA.AfterLogin(String)")
        End Try
    End Sub


End Class
