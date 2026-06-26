'Imports Sats.CommonLibrary

'Public Class BDSDelivery
'    Private mv_obj As IBDSDelivery

'    Public Sub New()
'        Try
'            Dim v_intPort As Integer
'            Dim v_strServer As String
'            Dim v_strChannel As String

'            v_intPort = System.Configuration.ConfigurationManager.AppSettings("RemotePort")
'            v_strServer = System.Configuration.ConfigurationManager.AppSettings("RemoteServer")
'            v_strChannel = System.Configuration.ConfigurationManager.AppSettings("RemoteChannel")

'            If v_strServer = "" Then v_strServer = "localhost"
'            If v_intPort = 0 Then v_intPort = 8100
'            If v_strChannel = "" Then v_strChannel = "tcp"

'            mv_obj = CType(Activator.GetObject(GetType(IBDSDelivery), v_strChannel & "://" & v_strServer & ":" & v_intPort & "/BDSDelivery"), IBDSDelivery)
'        Catch ex As Exception
'            Throw ex
'        Finally
'            GC.Collect()
'        End Try
'    End Sub

'    Public Function Message(ByRef pv_strMessage As String) As Long
'        Dim v_lngErr As Long = ERR_SYSTEM_OK
'        Try
'            Dim v_xmlDocument As New XmlDocumentEx
'            Dim v_strTellerID As String
'            v_xmlDocument.LoadXml(pv_strMessage)
'            Dim v_attrColl As Xml.XmlAttributeCollection = v_xmlDocument.DocumentElement.Attributes
'            If Not v_attrColl.GetNamedItem(gc_AtributeTLID) Is Nothing Then
'                v_strTellerID = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTLID), Xml.XmlAttribute).Value)
'            Else
'                v_strTellerID = String.Empty
'            End If

'            If v_strTellerID = String.Empty Then
'                Return ERR_SYSTEM_START
'            End If

'            Dim v_arrBytePublicKey() As Byte
'            Dim v_arrBytePrivateKey() As Byte
'            Dim v_strIPaddress As String = GetIPAddress()


'            GetKey(v_strTellerID, v_arrBytePublicKey, v_arrBytePrivateKey)

'            v_lngErr = mv_obj.GetPublicKey(v_strTellerID & v_strIPaddress, v_arrBytePublicKey)

'            Dim v_arrByteObjMsg() As Byte = Compression(pv_strMessage)
'            Dim v_arrByteMessage() As Byte = EncryptMessage(v_arrBytePublicKey, v_arrByteObjMsg)

'            'Return mv_obj.Message(pv_strMessage)
'            v_lngErr = mv_obj.Message(v_strTellerID & v_strIPaddress, v_arrByteMessage)

'            v_arrByteObjMsg = DecryptMessage(v_arrBytePrivateKey, v_arrByteMessage)
'            pv_strMessage = Decompress(v_arrByteObjMsg)

'            Return v_lngErr
'        Catch ex As Exception
'            LogError.Write("Error source: " & ex.Source & vbNewLine _
'                                                     & "Error code: System error!" & vbNewLine _
'                                                     & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
'            Return ERR_SYSTEM_START
'        End Try
'    End Function

'    Public Function RptMessage(ByRef pv_strMessage As String) As DataSet
'        Dim v_lngErr As Long = ERR_SYSTEM_OK
'        Try
'            Dim v_xmlDocument As New XmlDocumentEx
'            Dim v_strTellerID As String
'            v_xmlDocument.LoadXml(pv_strMessage)
'            Dim v_attrColl As Xml.XmlAttributeCollection = v_xmlDocument.DocumentElement.Attributes
'            If Not v_attrColl.GetNamedItem(gc_AtributeTLID) Is Nothing Then
'                v_strTellerID = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTLID), Xml.XmlAttribute).Value)
'            Else
'                v_strTellerID = String.Empty
'            End If

'            If v_strTellerID = String.Empty Then
'                Return Nothing
'            End If

'            Dim v_arrBytePublicKey() As Byte
'            Dim v_arrBytePrivateKey() As Byte
'            Dim v_strIPaddress As String = GetIPAddress()


'            GetKey(v_strTellerID, v_arrBytePublicKey, v_arrBytePrivateKey)

'            v_lngErr = mv_obj.GetPublicKey(v_strTellerID & v_strIPaddress, v_arrBytePublicKey)

'            Dim v_arrByteObjMsg() As Byte = Compression(pv_strMessage)
'            Dim v_arrByteMessage() As Byte = EncryptMessage(v_arrBytePublicKey, v_arrByteObjMsg)

'            'Return mv_obj.Message(pv_strMessage)
'            v_lngErr = mv_obj.RptMessage(v_strTellerID & v_strIPaddress, v_arrByteMessage)

'            v_arrByteObjMsg = DecryptMessage(v_arrBytePrivateKey, v_arrByteMessage)
'            Dim v_ds As DataSet
'            v_ds = ZetaCompressionLibrary.CompressionHelper.DecompressDataSet(v_arrByteObjMsg)
'            Return v_ds
'        Catch ex As Exception
'            LogError.Write("Error source: " & ex.Source & vbNewLine _
'                                                     & "Error code: System error!" & vbNewLine _
'                                                     & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
'            Return Nothing
'        End Try
'    End Function

'    Public Function LoadInterface(ByRef pv_strMessage As String) As DataSet
'        Dim v_lngErr As Long = ERR_SYSTEM_OK
'        Try
'            Dim v_xmlDocument As New XmlDocumentEx
'            Dim v_strTellerID As String
'            v_xmlDocument.LoadXml(pv_strMessage)
'            Dim v_attrColl As Xml.XmlAttributeCollection = v_xmlDocument.DocumentElement.Attributes
'            If Not v_attrColl.GetNamedItem(gc_AtributeTLID) Is Nothing Then
'                v_strTellerID = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTLID), Xml.XmlAttribute).Value)
'            Else
'                v_strTellerID = String.Empty
'            End If

'            If v_strTellerID = String.Empty Then
'                Return Nothing
'            End If

'            Dim v_arrBytePublicKey() As Byte
'            Dim v_arrBytePrivateKey() As Byte
'            Dim v_strIPaddress As String = GetIPAddress()


'            GetKey(v_strTellerID, v_arrBytePublicKey, v_arrBytePrivateKey)

'            v_lngErr = mv_obj.GetPublicKey(v_strTellerID & v_strIPaddress, v_arrBytePublicKey)

'            Dim v_arrByteObjMsg() As Byte = Compression(pv_strMessage)
'            Dim v_arrByteMessage() As Byte = EncryptMessage(v_arrBytePublicKey, v_arrByteObjMsg)

'            'Return mv_obj.Message(pv_strMessage)
'            v_lngErr = mv_obj.LoadInterface(v_strTellerID & v_strIPaddress, v_arrByteMessage)

'            v_arrByteObjMsg = DecryptMessage(v_arrBytePrivateKey, v_arrByteMessage)
'            Dim v_ds As DataSet
'            v_ds = ZetaCompressionLibrary.CompressionHelper.DecompressDataSet(v_arrByteObjMsg)
'            Return v_ds
'        Catch ex As Exception
'            LogError.Write("Error source: " & ex.Source & vbNewLine _
'                                                     & "Error code: System error!" & vbNewLine _
'                                                     & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
'            Return Nothing
'        End Try
'    End Function

'    Public Function GetErrorMessage(ByVal pv_lngErrorCode As Long, ByVal pv_strLanguage As String) As String
'        Return mv_obj.GetErrorMessage(pv_lngErrorCode, pv_strLanguage)
'    End Function

'    Private Function GetIPAddress() As String
'        Try
'            Dim sHostName As String = System.Net.Dns.GetHostName()
'            Dim ipE As System.Net.IPHostEntry = System.Net.Dns.GetHostByName(sHostName)
'            Dim IpA() As System.Net.IPAddress = ipE.AddressList
'            Dim sAddr As String

'            sAddr = IpA(0).ToString
'            Return sAddr
'        Catch ex As Exception
'            Throw ex
'        End Try
'    End Function
'End Class
