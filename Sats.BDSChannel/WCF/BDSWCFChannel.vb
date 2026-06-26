Imports System
Imports System.ServiceModel
Imports Sats.CommonLibrary
Imports Sats.BDSChannel.BDSService
Imports Sats.ClientCA
Imports BkavCASign
Imports System.IO

Public Class BDSDelivery 'BDSWCFChannel
    Implements Sats.BDSChannel.BDSService.IWCFCallback

    Public Event User_Join(ByVal pv_Client As String)
    Public Event User_Leave(ByVal pv_Client As String)
    Public Event Refresh_Clients(ByVal pv_Client() As String)
    Public Event ReceiveMessage(ByVal pv_oMessage As String)
    Public mv_oSessionKey As SessionKey
    Public mv_oSignClient As SignClient
    Public mv_blnCheckUSB As Boolean = False

    Private mv_oProxy As WCFClient
    Private mv_objInstanceContext As Object

    Public ReadOnly Property Stat() As CommunicationState
        Get
            Return mv_oProxy.State
        End Get
    End Property

#Region "IWCFCallback Member"
    Public Sub Receive(ByVal pv_oMessage As String) Implements BDSService.IWCFCallback.Receive

    End Sub

    Public Sub UserJoin(ByVal pv_oClient As CommonLibrary.Client) Implements BDSService.IWCFCallback.UserJoin

    End Sub

    Public Sub UserLeave(ByVal pv_oClient As CommonLibrary.Client) Implements BDSService.IWCFCallback.UserLeave

    End Sub
#End Region

#Region "Client function"
    Public Sub Connect()
        Dim v_strServer As String
        Try
            Dim v_oContent As New InstanceContext(Me)
            mv_oProxy = New WCFClient(v_oContent)
            v_strServer = mv_oProxy.Endpoint.Address.Uri.AbsoluteUri
            mv_oProxy.Endpoint.Address = New EndpointAddress(v_strServer)
            mv_oProxy.Open()

        Catch ex As Exception
            LogError.Write(ex.Message & vbCrLf & "Error connect to: " & v_strServer, EventLogEntryType.Error, gc_MODULE_CLIENT)
        End Try
    End Sub

    Public Sub UpdateInfoClient(ByVal pv_oClient As Client)
        If mv_oProxy.State <> CommunicationState.Opened Then
            Connect()
        End If
        mv_oProxy.ConnectAsync(pv_oClient)
    End Sub

    Public Sub SendAction(ByVal pv_oClient As Client)
        If mv_oProxy.State <> CommunicationState.Opened Then
            Connect()
        End If
        mv_oProxy.SendActionAsync(pv_oClient)
    End Sub

    Public Sub SendAsync(ByVal pv_arrByte() As Byte)
        mv_oProxy.SendAsync(pv_arrByte)
    End Sub

    Public Function Message(ByRef pv_strMessage As String) As Long
        If mv_oProxy.State <> CommunicationState.Opened Then
            Connect()
        End If
        Dim v_lngError As Long = ERR_SYSTEM_OK
        Try
            ''Thực hiện ký số với giao dịch có đánh dấu ký số
            'Dim v_xmlDocumentMessage As New XmlDocumentEx
            'v_xmlDocumentMessage.LoadXml(pv_strMessage)
            ''lấy tham số xem có ký CA hay không

            'Dim v_attrColl As Xml.XmlAttributeCollection = v_xmlDocumentMessage.DocumentElement.Attributes
            'Dim v_strIsSignCA As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeSIGNCA), Xml.XmlAttribute).Value)
            'If v_strIsSignCA = "1" Then
            '    pv_strMessage = ClientBussinessCA.CombineData(pv_strMessage)
            'End If
            'Mã hóa xml để gửi lên Server bằng SessionKey
            'pv_strMessage = ClientBussinessCA.EncryptXML(pv_strMessage, mv_oSessionKey, v_attrColl.GetNamedItem(gc_AtributeTLNAME).Value, v_strIsSignCA)
            Dim v_arrByte() As Byte = modCommond.Compression(pv_strMessage)
            v_lngError = mv_oProxy.Send(v_arrByte)
            pv_strMessage = Decompress(v_arrByte)
            Return v_lngError
        Catch ex As Exception
            Return ERR_SYSTEM_START
        End Try
    End Function

    Public Function MessageCA(ByRef pv_strMessage As String) As Long
        If mv_oProxy.State <> CommunicationState.Opened Then
            Connect()
        End If
        Dim v_lngError As Long = ERR_SYSTEM_OK
        Try
            ''Thực hiện ký số với giao dịch có đánh dấu ký số
            Dim v_xmlDocumentMessage As New XmlDocumentEx
            Dim v_attrColl As Xml.XmlAttributeCollection
            'Dim pv_strSaveMessage As String = pv_strMessage
            v_xmlDocumentMessage.LoadXml(pv_strMessage)
            v_attrColl = v_xmlDocumentMessage.DocumentElement.Attributes
            Dim v_strTLName As String
            Dim v_strStatus As String = v_attrColl.GetNamedItem(gc_AtributeSTATUS).Value
            'Them de lay ky so voi duyet giao dich
            v_strTLName = v_attrColl.GetNamedItem(gc_AtributeCFRNAME).Value
            If v_strTLName = "" Then
                v_strTLName = v_attrColl.GetNamedItem(gc_AtributeOFFNAME).Value

            End If
            If v_strTLName = "" Then
                v_strTLName = v_attrColl.GetNamedItem(gc_AtributeCHKNAME).Value

            End If
            If v_strTLName = "" Then
                v_strTLName = v_attrColl.GetNamedItem(gc_AtributeTLNAME).Value

            End If
            'bangpv add to del 
            LogError.Write("ky so lan 1: " & Format(Now, "HH:mm:ss.fff"), EventLogEntryType.Information, gc_MODULE_CLIENT)
            pv_strMessage = ClientBussinessCA.CombineData(pv_strMessage)
            LogError.Write("end ky so lan 1: " & Format(Now, "HH:mm:ss.fff"), EventLogEntryType.Information, gc_MODULE_CLIENT)
            'lấy chữ ký ở client 
            v_xmlDocumentMessage.LoadXml(pv_strMessage)
            v_attrColl = v_xmlDocumentMessage.DocumentElement.Attributes
            Dim v_strClientSignature As String = CStr(CType(v_attrColl.GetNamedItem("SignatureXML"), Xml.XmlAttribute).Value)
            'Mã hóa xml để gửi lên Server bằng SessionKey
            pv_strMessage = ClientBussinessCA.EncryptXML(pv_strMessage, mv_oSessionKey, v_strTLName)

            v_xmlDocumentMessage.LoadXml(pv_strMessage)
            v_attrColl = v_xmlDocumentMessage.DocumentElement.Attributes
            Dim pv_strSaveMessage As String = CStr(CType(v_attrColl.GetNamedItem("EncryptedXML"), Xml.XmlAttribute).Value)

            Dim v_arrByte() As Byte = modCommond.Compression(pv_strMessage)
            Dim v_arrByteCA() As Byte
            Dim v_strTLLOGIDCA As String
            Dim v_strVSDSignature As String
            'bangpv add to del
            LogError.Write("Gui du lieu len host xu ly: " & Format(Now, "HH:mm:ss.fff"), EventLogEntryType.Information, gc_MODULE_CLIENT)
            v_lngError = mv_oProxy.SendCA(v_arrByte, v_arrByteCA, v_strTLLOGIDCA, v_strVSDSignature)
            LogError.Write("End Gui du lieu len host xu ly: " & Format(Now, "HH:mm:ss.fff"), EventLogEntryType.Information, gc_MODULE_CLIENT)
            If v_lngError = ERR_SYSTEM_OK Then
                pv_strMessage = Decompress(v_arrByte)

                Dim v_strData As String = Decompress(v_arrByteCA)
                'Ký số ở client lên dữ liệu trả về
                LogError.Write("ky so lan 2: " & Format(Now, "HH:mm:ss.fff"), EventLogEntryType.Information, gc_MODULE_CLIENT)
                v_strData = ClientBussinessCA.CombineData(v_strData)
                LogError.Write("end ky so lan 2: " & Format(Now, "HH:mm:ss.fff"), EventLogEntryType.Information, gc_MODULE_CLIENT)
                v_xmlDocumentMessage.LoadXml(v_strData)
                v_attrColl = v_xmlDocumentMessage.DocumentElement.Attributes
                v_strClientSignature = CStr(CType(v_attrColl.GetNamedItem("SignatureXML"), Xml.XmlAttribute).Value)
                'encrypted
                v_strData = ClientBussinessCA.EncryptXML(v_strData, mv_oSessionKey, v_strTLName)
                v_xmlDocumentMessage.LoadXml(v_strData)
                v_attrColl = v_xmlDocumentMessage.DocumentElement.Attributes
                v_strData = CStr(CType(v_attrColl.GetNamedItem("EncryptedXML"), Xml.XmlAttribute).Value)
                'Gửi dữ liệu lên server để lưu file 
                v_arrByte = modCommond.Compression(v_strData)
                Dim v_strLocalDir, v_strFileName As String
                'v_strClientSignature = 
                LogError.Write("luu file tai server: " & Format(Now, "HH:mm:ss.fff"), EventLogEntryType.Information, gc_MODULE_CLIENT)
                v_lngError = mv_oProxy.SaveFileRptCA(v_arrByte, v_strVSDSignature, v_strClientSignature, v_strTLName, v_strTLLOGIDCA, v_strLocalDir, v_strFileName, v_strStatus)
                LogError.Write("end luu file tai server: " & Format(Now, "HH:mm:ss.fff"), EventLogEntryType.Information, gc_MODULE_CLIENT)
                Dim v_strAppPath As String = System.AppDomain.CurrentDomain.BaseDirectory
                v_strLocalDir = v_strAppPath & "\Log\" & v_strLocalDir
                'check existed Log folder
                Dim f As New IO.DirectoryInfo(v_strAppPath & "\Log")
                If Not f.Exists Then
                    Directory.CreateDirectory(v_strAppPath & "\Log")
                End If
                If v_lngError = ERR_SYSTEM_OK Then
                    Dim XMLDocumentMessage As New Xml.XmlDocument
                    Dim dataElement As Xml.XmlElement
                    Dim v_attrData, v_attrVSDSignature, v_attrClientSignature As Xml.XmlAttribute
                    dataElement = XMLDocumentMessage.CreateElement("RPData")

                    v_attrData = XMLDocumentMessage.CreateAttribute("DATA")
                    v_attrData.Value = v_strData 'ByteArrayToStr(v_arrByte)
                    dataElement.Attributes.Append(v_attrData)

                    v_attrVSDSignature = XMLDocumentMessage.CreateAttribute(gc_AtributeSignatureVSD)
                    v_attrVSDSignature.Value = v_strVSDSignature
                    dataElement.Attributes.Append(v_attrVSDSignature)

                    v_attrClientSignature = XMLDocumentMessage.CreateAttribute(gc_AtributeSignatureClient)
                    v_attrClientSignature.Value = v_strClientSignature
                    dataElement.Attributes.Append(v_attrClientSignature)

                    XMLDocumentMessage.AppendChild(dataElement)
                    'Dim pv_strMessageSave As String = "<DATA>" & ByteArrayToStr(v_arrByte) & "</DATA>" _
                    '                & "<" & gc_AtributeSignatureVSD & ">" & v_strVSDSignature & "<" & gc_AtributeSignatureVSD & ">" _
                    '                & "<" & gc_AtributeSignatureClient & ">" & v_strClientSignature & "<" & gc_AtributeSignatureClient & ">"
                    ClientBussinessCA.SaveFile(XMLDocumentMessage.InnerXml, v_strFileName, v_strLocalDir)
                End If
                'ClientBussinessCA.SaveFile(pv_strSaveMessage)
                'Ghep voi xml goc va chu ky cua TV de luu file 
                Return v_lngError
            Else
                pv_strMessage = Decompress(v_arrByte)
                Return v_lngError
            End If
        Catch ex As Exception
            Return ERR_SYSTEM_START
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                                    & "Error code: System error!" & vbNewLine _
                                                    & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
        End Try
    End Function

    Public Function FileCA(ByRef pv_strMessage As String, _
                           ByVal pv_strBrid As String, _
                           ByVal pv_strFileName As String, _
                           ByVal pv_strCurrDate As String) As String
        Try
            Return mv_oProxy.FileCA(pv_strMessage, pv_strBrid, pv_strFileName, pv_strCurrDate)
        Catch ex As Exception

        End Try

    End Function

    Public Function GetServerRptExp(ByVal pv_strRptId As String, ByVal pv_strTLName As String, ByVal pv_strSiCode As String, ByRef pv_strSiCodeAllow As String) As Integer
        If mv_oProxy.State <> CommunicationState.Opened Then
            Connect()
        End If
        Dim v_lngError As Long = ERR_SYSTEM_OK
        Dim v_intResult As Integer = 0
        v_lngError = mv_oProxy.GetServerRptExp(pv_strRptId, pv_strTLName, v_intResult, pv_strSiCode, pv_strSiCodeAllow)

        Return v_intResult
    End Function
    Public Function RptMessage(ByRef pv_strMsg As String, ByVal pv_intMaxTransferRows As Integer) As DataSet
        If mv_oProxy.State <> CommunicationState.Opened Then
            Connect()
        End If
        Dim v_lngError As Long = ERR_SYSTEM_OK
        Dim v_ds As DataSet
        Try
            Dim v_arrByte() As Byte = Nothing
            v_lngError = mv_oProxy.SendRpt(pv_strMsg, v_arrByte)
            v_ds = ZetaCompressionLibrary.CompressionHelper.DecompressDataSet(v_arrByte)

            'HaNM5 sua 23/12/2020
            Dim v_xmlDocumentMessage As New XmlDocumentEx
            v_xmlDocumentMessage.LoadXml(pv_strMsg)
            Dim v_strRptDataKey As String
            Dim v_intRptDataRowCount As Integer

            If Not v_xmlDocumentMessage.DocumentElement.Attributes.GetNamedItem(gc_AtributeRptDataKey) Is Nothing Then
                v_strRptDataKey = CStr(CType(v_xmlDocumentMessage.DocumentElement.Attributes.GetNamedItem(gc_AtributeRptDataKey), Xml.XmlAttribute).Value)
                v_intRptDataRowCount = Convert.ToInt32(CStr(CType(v_xmlDocumentMessage.DocumentElement.Attributes.GetNamedItem(gc_AtributeRptDataRowCount), Xml.XmlAttribute).Value))

                Dim v_intCurrDataRowCount As Integer = v_ds.Tables(0).Rows.Count
                Dim v_intRemainDataRowCount As Integer = v_intRptDataRowCount - v_intCurrDataRowCount
                Dim v_intFrom As Integer = v_intCurrDataRowCount
                Dim v_intTo As Integer = v_intCurrDataRowCount + pv_intMaxTransferRows

                While v_intRemainDataRowCount > 0
                    If v_intTo > v_intRptDataRowCount Then
                        v_intTo = v_intRptDataRowCount
                        v_intRemainDataRowCount = 0
                    End If
                    Dim v_dsTemp As DataSet = RptFetchData(v_strRptDataKey, v_intFrom, v_intTo)
                    If Not v_dsTemp Is Nothing Then
                        If v_dsTemp.Tables(0).Rows.Count > 0 Then
                            v_intRemainDataRowCount = v_intRemainDataRowCount - v_dsTemp.Tables(0).Rows.Count
                            v_ds.Tables(0).Merge(v_dsTemp.Tables(0), False, MissingSchemaAction.Add)
                        End If
                    End If
                    v_intFrom = v_intTo
                    v_intTo = v_intTo + pv_intMaxTransferRows
                End While
            End If

            Return v_ds
        Catch ex As Exception
            Return Nothing
        Finally
            If Not v_ds Is Nothing Then
                v_ds.Dispose()
            End If
        End Try
    End Function
    'bangpv
    Public Function RptExpMessage(ByRef pv_strMsg As String) As Long
        If mv_oProxy.State <> CommunicationState.Opened Then
            Connect()
        End If
        Dim v_lngError As Long = ERR_SYSTEM_OK

        Try

            v_lngError = mv_oProxy.SendExpRpt(pv_strMsg)

            Return v_lngError
        Catch ex As Exception
            Return Nothing
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                                                    & "Error code: System error!" & vbNewLine _
                                                    & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
        Finally

        End Try
    End Function
    'end bangpv 
    Public Function RptFetchData(ByVal pv_strRptDataKey As String, ByVal pv_intFrom As Integer, ByVal pv_intTo As Integer) As DataSet
        If mv_oProxy.State <> CommunicationState.Opened Then
            Connect()
        End If
        Dim v_lngError As Long = ERR_SYSTEM_OK
        Dim v_ds As DataSet
        Try
            Dim v_arrByte() As Byte = Nothing
            v_lngError = mv_oProxy.FetchRpt(pv_strRptDataKey, pv_intFrom, pv_intTo, v_arrByte)
            v_ds = ZetaCompressionLibrary.CompressionHelper.DecompressDataSet(v_arrByte)
            Return v_ds
        Catch ex As Exception
            Return Nothing
        Finally
            If Not v_ds Is Nothing Then
                v_ds.Dispose()
            End If
        End Try
    End Function
    Public Function RptMessageCA(ByRef pv_strMsg As String, ByVal pv_intMaxTransferRows As Integer) As DataSet
        If mv_oProxy.State <> CommunicationState.Opened Then
            Connect()
        End If
        Dim v_lngError As Long = ERR_SYSTEM_OK
        Dim v_ds As DataSet
        Dim v_strVSDSignature As String
        Dim v_strCAKey As String = ""
        Dim v_strDataHash As String = ""
        Try
            'Sign XML request 
            Dim v_xmlDocumentMessage As New XmlDocumentEx
            Dim v_attrColl As Xml.XmlAttributeCollection
            'Dim pv_strSaveMessage As String = pv_strMessage
            v_xmlDocumentMessage.LoadXml(pv_strMsg)
            v_attrColl = v_xmlDocumentMessage.DocumentElement.Attributes
            Dim v_strTLName As String = v_attrColl.GetNamedItem(gc_AtributeTLNAME).Value
            'Dim v_strRPTID As String = v_attrColl.GetNamedItem(gc_AtributeTLTXCD).Value
            Dim v_strClause As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCLAUSE), Xml.XmlAttribute).Value)
            Dim v_arrTemp() As String
            'Dim v_strFieldCode As String = ""
            v_arrTemp = v_strClause.Split("#")
            Dim v_strRptID = v_arrTemp(0)
            pv_strMsg = ClientBussinessCA.CombineData(pv_strMsg)
            'lấy chữ ký ở client 
            v_xmlDocumentMessage.LoadXml(pv_strMsg)
            v_attrColl = v_xmlDocumentMessage.DocumentElement.Attributes
            Dim v_strClientSignature As String = CStr(CType(v_attrColl.GetNamedItem("SignatureXML"), Xml.XmlAttribute).Value)
            'Mã hóa xml để gửi lên Server bằng SessionKey
            pv_strMsg = ClientBussinessCA.EncryptXML(pv_strMsg, mv_oSessionKey, v_strTLName)
            'end bangpv
            Dim v_arrByte() As Byte = Nothing

            v_lngError = mv_oProxy.SendRptCA(pv_strMsg, v_arrByte, v_strVSDSignature, v_strCAKey, v_strDataHash)
            If v_lngError = ERR_SYSTEM_OK Then
                v_ds = ZetaCompressionLibrary.CompressionHelper.DecompressDataSet(v_arrByte)
            End If
            'HaNM5 sua 23/12/2020
            v_xmlDocumentMessage = New XmlDocumentEx
            v_xmlDocumentMessage.LoadXml(pv_strMsg)
            Dim v_strRptDataKey As String
            Dim v_intRptDataRowCount As Integer

            If Not v_xmlDocumentMessage.DocumentElement.Attributes.GetNamedItem(gc_AtributeRptDataKey) Is Nothing Then
                v_strRptDataKey = CStr(CType(v_xmlDocumentMessage.DocumentElement.Attributes.GetNamedItem(gc_AtributeRptDataKey), Xml.XmlAttribute).Value)
                v_intRptDataRowCount = Convert.ToInt32(CStr(CType(v_xmlDocumentMessage.DocumentElement.Attributes.GetNamedItem(gc_AtributeRptDataRowCount), Xml.XmlAttribute).Value))

                Dim v_intCurrDataRowCount As Integer = v_ds.Tables(0).Rows.Count
                Dim v_intRemainDataRowCount As Integer = v_intRptDataRowCount - v_intCurrDataRowCount
                Dim v_intFrom As Integer = v_intCurrDataRowCount
                Dim v_intTo As Integer = v_intCurrDataRowCount + pv_intMaxTransferRows

                While v_intRemainDataRowCount > 0
                    If v_intTo > v_intRptDataRowCount Then
                        v_intTo = v_intRptDataRowCount
                        v_intRemainDataRowCount = 0
                    End If
                    Dim v_dsTemp As DataSet = RptFetchData(v_strRptDataKey, v_intFrom, v_intTo)
                    If Not v_dsTemp Is Nothing Then
                        If v_dsTemp.Tables(0).Rows.Count > 0 Then
                            v_intRemainDataRowCount = v_intRemainDataRowCount - v_dsTemp.Tables(0).Rows.Count
                            v_ds.Tables(0).Merge(v_dsTemp.Tables(0), False, MissingSchemaAction.Add)
                        End If
                    End If
                    v_intFrom = v_intTo
                    v_intTo = v_intTo + pv_intMaxTransferRows
                End While
            End If
            'Dim v_strTest As String = Convert.ToBase64String(v_arrByte)
            'Dim v_arrtest As Byte() = Convert.FromBase64String(v_strTest)
            ' ki so ds lay chu ki USB
            'Dim v_strDS As String = ByteArrayToStr(v_arrByte)
            If v_ds.Tables(0).Rows.Count > 0 Then
                'bangpv : sửa từ lấy getxml về chuyển byte thành strB64
                'Dim v_strDS As String = v_ds.GetXml()
                Dim v_strDS As String = ""
                'v_arrByte = ZetaCompressionLibrary.CompressionHelper.CompressDataSet(v_ds)
                'Dim v_strDS As String = Convert.ToBase64String(v_arrByte)
                ' tich hop pv_strMsg
                'ky so 
                'v_strDS = ClientBussinessCA.CombineData(v_strDS)
                v_strDS = ClientBussinessCA.CombineData(v_strDataHash)
                v_xmlDocumentMessage.LoadXml(v_strDS)
                v_attrColl = v_xmlDocumentMessage.DocumentElement.Attributes
                v_strClientSignature = CStr(CType(v_attrColl.GetNamedItem("SignatureXML"), Xml.XmlAttribute).Value)
                'encrypted
                'bangpv 
                'Thêm tên file có điều kiện tạo báo cáo 
                v_strRptID = v_strRptID & "'" & v_strCAKey
                'End bangpv
                v_strDS = ClientBussinessCA.EncryptXML(v_strDS, mv_oSessionKey, v_strTLName)
                v_xmlDocumentMessage.LoadXml(v_strDS)
                v_attrColl = v_xmlDocumentMessage.DocumentElement.Attributes
                v_strDS = CStr(CType(v_attrColl.GetNamedItem("EncryptedXML"), Xml.XmlAttribute).Value)

                v_arrByte = modCommond.Compression(v_strDS)
                Dim v_strLocalDir, v_strFileName As String
                Dim v_strAppPath As String = System.AppDomain.CurrentDomain.BaseDirectory
                'v_strClientSignature = 
                v_lngError = mv_oProxy.SaveFileRptCA(v_arrByte, v_strVSDSignature, v_strClientSignature, v_strTLName, v_strRptID, v_strLocalDir, v_strFileName, "")
                'check exist Log folder 
                v_strLocalDir = v_strAppPath & "\Log\" & v_strLocalDir
                Dim f As New IO.DirectoryInfo(v_strAppPath & "\Log")
                If Not f.Exists Then
                    Directory.CreateDirectory(v_strAppPath & "\Log")
                End If
                If v_lngError = ERR_SYSTEM_OK Then
                    Dim XMLDocumentMessage As New Xml.XmlDocument
                    Dim dataElement As Xml.XmlElement
                    Dim v_attrData, v_attrVSDSignature, v_attrClientSignature As Xml.XmlAttribute
                    dataElement = XMLDocumentMessage.CreateElement("RPData")

                    v_attrData = XMLDocumentMessage.CreateAttribute("DATA")
                    v_attrData.Value = v_strDS 'ByteArrayToStr(v_arrByte)
                    dataElement.Attributes.Append(v_attrData)

                    v_attrVSDSignature = XMLDocumentMessage.CreateAttribute(gc_AtributeSignatureVSD)
                    v_attrVSDSignature.Value = v_strVSDSignature
                    dataElement.Attributes.Append(v_attrVSDSignature)

                    v_attrClientSignature = XMLDocumentMessage.CreateAttribute(gc_AtributeSignatureClient)
                    v_attrClientSignature.Value = v_strClientSignature
                    dataElement.Attributes.Append(v_attrClientSignature)

                    XMLDocumentMessage.AppendChild(dataElement)
                    'Dim pv_strMessageSave As String = "<DATA>" & ByteArrayToStr(v_arrByte) & "</DATA>" _
                    '                & "<" & gc_AtributeSignatureVSD & ">" & v_strVSDSignature & "<" & gc_AtributeSignatureVSD & ">" _
                    '                & "<" & gc_AtributeSignatureClient & ">" & v_strClientSignature & "<" & gc_AtributeSignatureClient & ">"
                    ClientBussinessCA.SaveFile(XMLDocumentMessage.InnerXml, v_strFileName, v_strLocalDir)
                End If
            End If
            ' luu file giong server 
            Return v_ds
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                 & "Error code: System error!" & vbNewLine _
                 & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_CLIENT)
            Return Nothing
        Finally
            If Not v_ds Is Nothing Then
                v_ds.Dispose()
            End If
        End Try
    End Function

    Public Function Login(ByVal pv_strUserName As String, ByVal pv_strPassword As String, ByVal pv_strIPAddress As String) As String
        If mv_oProxy.State <> CommunicationState.Opened Then
            Connect()
        End If
        Try
            Return mv_oProxy.Login(pv_strUserName, pv_strPassword, pv_strIPAddress)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function LoginCA(ByVal pv_strUserName As String, ByVal pv_strPassword As String, ByVal pv_strIPAddress As String) As String
        If mv_oProxy.State <> CommunicationState.Opened Then
            Connect()
        End If
        Try
            'Generate session key
            If (mv_oSessionKey Is Nothing) Then
                mv_oSessionKey = New SessionKey
            End If
            mv_oSessionKey.Generate()

            mv_blnCheckUSB = False

            'Init other variables
            Dim v_strEncryptedXML As String = ""
            Dim v_strEncryptedSessionKey As String = ""
            Dim v_strResult As String = ""

            ''will be deleted in future
            'Dim v_oSignServer As New SignClient
            'v_oSignServer.LoadCertificate()

            'Prepare login
            Dim v_strErr As String = ClientBussinessCA.PrepareLoginCA(pv_strUserName, pv_strPassword, _
                pv_strIPAddress, mv_oSessionKey, v_strEncryptedXML, v_strEncryptedSessionKey) ', v_oSignServer)
            If (v_strErr.StartsWith(ClientBussinessCA.HEADER_ERROR)) Then
                'If (v_strErr.StartsWith(ClientBussinessCA.BKAV_HEADER_ERROR)) Then
                '    'MsgBox("Bạn chưa cắm token hoặc chưa chọn chứng thư số")
                '    Return v_strErr
                'End If
                Return v_strErr
            End If

            'Login CA
            mv_oProxy.LoginCA(v_strEncryptedXML, v_strEncryptedSessionKey, v_strResult)

            'Update information

            ClientBussinessCA.AfterLogin(v_strEncryptedSessionKey, mv_oSessionKey, v_strResult)

            If (v_strResult.Contains("ERROR")) Then
                mv_oSignClient = Nothing
                mv_blnCheckUSB = False
            Else
                mv_oSignClient = ClientBussinessCA.getUSB
                mv_blnCheckUSB = True
            End If

            Return v_strResult
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function ChangePassword(ByVal pv_strUserName As String, ByVal pv_strNewPassword As String) As Long
        Return mv_oProxy.ChangePassword(pv_strUserName, pv_strNewPassword)
    End Function

    Public Function GetTellerProfile(ByVal pv_strTiket As String) As WCFTellerProfile
        If mv_oProxy.State <> CommunicationState.Opened Then
            Connect()
        End If
        Try
            Return mv_oProxy.GetTellerProfile(pv_strTiket)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Sub Close(ByVal pv_oClient As Client)
        Try
            If mv_oProxy.State = CommunicationState.Opened Then
                mv_oProxy.Disconnect(pv_oClient)
                mv_oProxy.Close()
                mv_oProxy = Nothing
                ClientBussinessCA.closeUSB()
            End If
        Catch ex As Exception

        End Try
    End Sub
#End Region

    Public Function BeginReceive(ByVal pv_oMessage As String, ByVal callback As System.AsyncCallback, ByVal asyncState As Object) As System.IAsyncResult Implements BDSService.IWCFCallback.BeginReceive

    End Function

    Public Function BeginUserJoin(ByVal pv_oClient As CommonLibrary.Client, ByVal callback As System.AsyncCallback, ByVal asyncState As Object) As System.IAsyncResult Implements BDSService.IWCFCallback.BeginUserJoin

    End Function

    Public Function BeginUserLeave(ByVal pv_oClient As CommonLibrary.Client, ByVal callback As System.AsyncCallback, ByVal asyncState As Object) As System.IAsyncResult Implements BDSService.IWCFCallback.BeginUserLeave

    End Function

    Public Sub EndReceive(ByVal result As System.IAsyncResult) Implements BDSService.IWCFCallback.EndReceive

    End Sub

    Public Sub EndUserJoin(ByVal result As System.IAsyncResult) Implements BDSService.IWCFCallback.EndUserJoin

    End Sub

    Public Sub EndUserLeave(ByVal result As System.IAsyncResult) Implements BDSService.IWCFCallback.EndUserLeave

    End Sub

    Public Function BeginReceiveAction(ByVal pv_oDataSet As System.Data.DataSet, ByVal callback As System.AsyncCallback, ByVal asyncState As Object) As System.IAsyncResult Implements BDSService.IWCFCallback.BeginReceiveAction

    End Function

    Public Sub EndReceiveAction(ByVal result As System.IAsyncResult) Implements BDSService.IWCFCallback.EndReceiveAction

    End Sub

    Public Sub ReceiveAction(ByVal pv_oDataSet As System.Data.DataSet) Implements BDSService.IWCFCallback.ReceiveAction

    End Sub
End Class
