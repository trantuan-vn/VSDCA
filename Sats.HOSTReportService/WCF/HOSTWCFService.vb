Imports System
Imports System.IO
Imports System.ServiceModel
Imports System.Collections.Generic
Imports System.Runtime.Serialization
Imports Sats.CommonLibrary
Imports System.EnterpriseServices
Imports Sats.ServerCA
Imports BkavCASign
Imports log4net
Imports System.Security.Cryptography


<ServiceBehavior(InstanceContextMode:=InstanceContextMode.Single, _
                 ConcurrencyMode:=ConcurrencyMode.Multiple, _
                 UseSynchronizationContext:=False)> _
Public Class HOSTWCFService
    Implements IReportWCF

    Private mv_lstClients As New Dictionary(Of Client, IWCFCallback)
    Private mv_ClientList As New List(Of Client)
    Private mv_oSyncObj As New Object
    'Private mv_lwLogWriter As New LogWriter

    Public ReadOnly Property CurrentCallback() As IWCFCallback
        Get
            Return OperationContext.Current.GetCallbackChannel(Of IWCFCallback)()
        End Get
    End Property

#Region "IWCF Member"
    Public Function Connect(ByVal pv_oClient As Client) As Boolean Implements IReportWCF.Connect
        If Not mv_lstClients.ContainsValue(CurrentCallback) Then
            SyncLock (mv_oSyncObj)
                mv_lstClients.Add(pv_oClient, CurrentCallback)
                mv_ClientList.Add(pv_oClient)
            End SyncLock
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub Disconnect(ByVal pv_oClient As Client) Implements IReportWCF.Disconnect
        For Each v_oClient As Client In mv_lstClients.Keys
            If v_oClient.IPAddress = pv_oClient.IPAddress Then
                SyncLock (mv_oSyncObj)
                    mv_lstClients.Remove(v_oClient)
                    mv_ClientList.Remove(v_oClient)
                End SyncLock
                Exit Sub
            End If
        Next
    End Sub
    'Public Function SendRpt(ByRef pv_strMessage As String, ByRef pv_arrByte() As Byte) As Long Implements CommonLibrary.IReportWCF.SendRpt
    '    Dim v_strErrorSource As String = "HOSTDelivery.Message(Byte)", v_strErrorMessage As String = ""
    '    Dim v_strResultData As String = vbNullString
    '    Dim v_lngErr As Long
    '    Dim v_obj As RP.Report
    '    Dim v_ds As DataSet
    '    Try
    '        'Read transaction message 
    '        Dim v_xmlDocumentMessage As New XmlDocumentEx
    '        v_xmlDocumentMessage.LoadXml(pv_strMessage)
    '        Dim v_strCAKey As String = ""
    '        'HaNM5: 08/12/2020
    '        HOSTReportChanel.AppLogger.Info(pv_strMessage)

    '        'Get header message.

    '        Dim v_attrColl As Xml.XmlAttributeCollection = v_xmlDocumentMessage.DocumentElement.Attributes
    '        Dim v_strLOCAL As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeLOCAL), Xml.XmlAttribute).Value)
    '        Dim v_strMSGTYPE As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeMSGTYPE), Xml.XmlAttribute).Value)
    '        Dim v_strTLName As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTLNAME), Xml.XmlAttribute).Value)

    '        Dim v_strClause As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCLAUSE), Xml.XmlAttribute).Value)
    '        Dim v_arrTemp() As String = v_strClause.Split("#")
    '        Dim v_strSICODE As String = ""
    '        Dim v_strRptID = v_arrTemp(0)
    '        Dim v_arrField = v_arrTemp(1).Split("$")

    '        For l As Integer = 0 To v_arrField.Count - 2
    '            'bangpv: CA
    '            If v_arrField(l).Split("|")(2) = "SICODE" Then
    '                v_strSICODE = v_arrField(l).Split("|")(3)
    '            End If
    '            'end bangpv
    '        Next

    '        Dim v_strReportDataKey As String = String.Empty
    '        v_obj = New RP.Report
    '        v_ds = v_obj.CreateReport(v_xmlDocumentMessage, v_strReportDataKey)
    '        'Kiem tra xem bao cao can in phia client hay export phia server
    '        Dim vServerRptExp As Integer = v_obj.GetServerRptExport(v_strTLName)
    '        If vServerRptExp > 0 Then
    '            v_lngErr = v_obj.SaveRPTFile(v_ds, v_strTLName, v_strRptID, v_strSICODE, v_strCAKey)
    '        Else
    '            'Compress message
    '            pv_arrByte = ZetaCompressionLibrary.CompressionHelper.CompressDataSet(v_ds)
    '            pv_strMessage = v_xmlDocumentMessage.InnerXml
    '        End If

    '        Return v_lngErr
    '    Catch ex As Exception
    '        LogError.Write("Error source: " & ex.Source & vbNewLine _
    '            & "Error code: System error!" & vbNewLine _
    '            & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_HOST)
    '        Return ERR_SYSTEM_START
    '    Finally
    '        If Not v_obj Is Nothing Then
    '            v_obj.Dispose()
    '        End If
    '        If Not v_ds Is Nothing Then
    '            v_ds.Dispose()
    '        End If
    '        GC.Collect()
    '        GC.GetTotalMemory(False)
    '        GC.Collect()
    '    End Try
    'End Function

    Public Function SendRpt(ByRef pv_strMessage As String, ByRef pv_arrByte() As Byte) As Long Implements CommonLibrary.IReportWCF.SendRpt
        Dim v_strErrorSource As String = "HOSTDelivery.Message(Byte)", v_strErrorMessage As String = ""
        Dim v_strResultData As String = vbNullString
        Dim v_lngErr As Long
        Dim v_obj As RP.Report
        Dim v_ds As DataSet
        Try
            'Read transaction message 
            Dim v_xmlDocumentMessage As New XmlDocumentEx
            v_xmlDocumentMessage.LoadXml(pv_strMessage)

            'HaNM5: 08/12/2020
            HOSTReportChanel.AppLogger.Info(pv_strMessage)

            'Get header message.

            Dim v_attrColl As Xml.XmlAttributeCollection = v_xmlDocumentMessage.DocumentElement.Attributes
            Dim v_strLOCAL As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeLOCAL), Xml.XmlAttribute).Value)
            Dim v_strMSGTYPE As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeMSGTYPE), Xml.XmlAttribute).Value)
            Dim v_strReportDataKey As String = String.Empty
            v_obj = New RP.Report
            v_ds = v_obj.CreateReport(v_xmlDocumentMessage, v_strReportDataKey)
            'Compress message
            pv_arrByte = ZetaCompressionLibrary.CompressionHelper.CompressDataSet(v_ds)
            pv_strMessage = v_xmlDocumentMessage.InnerXml
            Return v_lngErr
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                & "Error code: System error!" & vbNewLine _
                & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_HOST)
            Return ERR_SYSTEM_START
        Finally
            If Not v_obj Is Nothing Then
                v_obj.Dispose()
            End If
            If Not v_ds Is Nothing Then
                v_ds.Dispose()
            End If
            GC.Collect()
            GC.GetTotalMemory(False)
            GC.Collect()
        End Try
    End Function

    Public Function SendExpRpt(ByVal pv_strMessage As String) As Long Implements CommonLibrary.IReportWCF.SendExpRpt
        Dim v_strErrorSource As String = "HOSTDelivery.Message(Byte)", v_strErrorMessage As String = ""
        Dim v_strResultData As String = vbNullString
        Dim v_lngErr As Long
        Dim v_obj As RP.Report
        Dim v_ds As DataSet
        'Dim v_strFileName As String
        Try
            'Read transaction message 
            Dim v_xmlDocumentMessage As New XmlDocumentEx
            v_xmlDocumentMessage.LoadXml(pv_strMessage)
            Dim v_strCAKey As String = ""
            Dim v_strReportDataKey As String = ""
            'Get header message.

            Dim v_attrColl As Xml.XmlAttributeCollection = v_xmlDocumentMessage.DocumentElement.Attributes
            Dim v_strLOCAL As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeLOCAL), Xml.XmlAttribute).Value)
            Dim v_strMSGTYPE As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeMSGTYPE), Xml.XmlAttribute).Value)
            Dim v_strTLName As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTLNAME), Xml.XmlAttribute).Value)

            Dim v_strClause As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCLAUSE), Xml.XmlAttribute).Value)

            'bangpv add 20230219 
            Dim v_strClause1 As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTXTIME), Xml.XmlAttribute).Value)
            ' End bangpv add 20230219 

            Dim v_arrTemp() As String
            Dim v_strSICODE As String = ""
            'Dim v_strFieldCode As String = ""
            v_arrTemp = v_strClause.Split("#")
            Dim v_strRptID = v_arrTemp(0)
            Dim v_arrField = v_arrTemp(1).Split("$")

            For l As Integer = 0 To v_arrField.Count - 2
                'v_strSQL = Replace(v_strSQL, "[!" & v_arrField(l).Split("|")(0) & "]", v_arrField(l).Split("|")(1))
                'bangpv: CA
                If v_arrField(l).Split("|")(2) = "SICODE" Then
                    v_strSICODE = v_arrField(l).Split("|")(3)
                End If
                'end bangpv
            Next

            v_obj = New RP.Report
            v_ds = v_obj.CreateReport(v_xmlDocumentMessage, v_strReportDataKey, v_strCAKey)
            If v_strReportDataKey <> String.Empty Then
                v_ds = CoreBusiness.objMaster.ReportDataSets(v_strReportDataKey)
            End If
            'Compress message
            'pv_arrByte = ZetaCompressionLibrary.CompressionHelper.CompressDataSet(v_ds)
            '20230219: add param to function SaveRPTFile 
            v_lngErr = v_obj.SaveRPTFile(v_ds, v_strTLName, v_strRptID, v_strSICODE, v_strCAKey, v_strClause1)
            '20230219: add param to function SaveRPTFile 
            Return v_lngErr
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                & "Error code: System error!" & vbNewLine _
                & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_HOST)
            Return ERR_SYSTEM_START
        Finally
            If Not v_obj Is Nothing Then
                v_obj.Dispose()
            End If
            If Not v_ds Is Nothing Then
                v_ds.Dispose()
            End If
            GC.Collect()
            GC.GetTotalMemory(False)
            GC.Collect()
        End Try
    End Function
    'bANGPV SUA 09032023
    Function FetchRpt(ByVal pv_strRptDataKey As String, ByVal pv_intFrom As Integer, ByVal pv_intTo As Integer, ByRef pv_arrByte() As Byte) As Long Implements CommonLibrary.IReportWCF.FetchRpt
        Dim v_strErrorSource As String = "HOSTDelivery.Message(Byte)", v_strErrorMessage As String = ""
        Dim v_strResultData As String = vbNullString
        Dim v_lngErr As Long
        Dim v_obj As RP.Report
        Dim v_ds As DataSet
        Try
            v_obj = New RP.Report
            v_ds = v_obj.FetchRpt(pv_strRptDataKey, pv_intFrom, pv_intTo)
            'Compress message
            pv_arrByte = ZetaCompressionLibrary.CompressionHelper.CompressDataSet(v_ds)

            Return v_lngErr
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                & "Error code: System error!" & vbNewLine _
                & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_HOST)
            Return ERR_SYSTEM_START
        Finally
            If Not v_obj Is Nothing Then
                v_obj.Dispose()
            End If
            If Not v_ds Is Nothing Then
                v_ds.Dispose()
            End If
            GC.Collect()
            GC.GetTotalMemory(False)
            GC.Collect()
        End Try
    End Function

    Public Function SendRptCA(ByRef pv_strMessage As String, ByRef pv_arrByte() As Byte, ByRef v_strVSDSignature As String, ByRef v_strCAKey As String, ByRef pv_strDataHash As String) As Long Implements CommonLibrary.IReportWCF.SendRptCA
        Dim v_strErrorSource As String = "HOSTDelivery.Message(Byte)", v_strErrorMessage As String = ""
        Dim v_strResultData As String = vbNullString
        Dim v_strClientSignature As String
        Dim v_lngErr As Long
        Dim v_obj As RP.Report
        Dim v_ds As DataSet

        Try
            'Read transaction message 
            Dim v_xmlRootMessage As New XmlDocumentEx
            v_xmlRootMessage.LoadXml(pv_strMessage)
            Dim v_attrRColl As Xml.XmlAttributeCollection = v_xmlRootMessage.DocumentElement.Attributes
            'lấy xml mã hóa
            pv_strMessage = CStr(CType(v_attrRColl.GetNamedItem("EncryptedXML"), Xml.XmlAttribute).Value)
            'lưu chuỗi xml mã hóa đã lấy ra 
            Dim pv_strMessageSave = pv_strMessage
            'lấy tlname truyền lên 
            ' giai ma bang HSM
            Dim v_strTLname = CStr(CType(v_attrRColl.GetNamedItem("TLName"), Xml.XmlAttribute).Value)
            'giải mã encrypted xml để lấy về xmd gốc 
            pv_strMessage = ServerBussinessCA.DecryptXML(pv_strMessage, v_strTLname, v_strClientSignature)

            Dim v_xmlDocumentMessage As New XmlDocumentEx
            v_xmlDocumentMessage.LoadXml(pv_strMessage)

            'HaNM5: 08/12/2020
            HOSTReportChanel.AppLogger.Info(pv_strMessage)
            'Get header message.

            Dim v_attrColl As Xml.XmlAttributeCollection = v_xmlDocumentMessage.DocumentElement.Attributes
            Dim v_strLOCAL As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeLOCAL), Xml.XmlAttribute).Value)
            Dim v_strMSGTYPE As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeMSGTYPE), Xml.XmlAttribute).Value)
            Dim v_strReportDataKey As String = String.Empty

            v_obj = New RP.Report
            v_ds = v_obj.CreateReport(v_xmlDocumentMessage, v_strReportDataKey, v_strCAKey)

            'Compress message
            pv_arrByte = ZetaCompressionLibrary.CompressionHelper.CompressDataSet(v_ds)
            'Dim pv_strResult As String = ByteArrayToStr(pv_arrByte)
            'Dim pv_strResult As String = ZetaCompressionLibrary.CompressionHelper.DecompressString(pv_arrByte)

            'ký số trên string trả về: 
            'Dim v_strData As String = v_ds.GetXml
            'bangpv: sửa về ký và lưu trên StringB64
            'Dim v_strData As String
            'If v_strReportDataKey <> String.Empty Then
            '    Dim v_arrByteTmp() As Byte = ZetaCompressionLibrary.CompressionHelper.CompressDataSet(CoreBusiness.objMaster.ReportDataSets(v_strReportDataKey))
            '    v_strData = Convert.ToBase64String(v_arrByteTmp)
            'Else
            '    v_strData = Convert.ToBase64String(pv_arrByte)
            'End If

            'v_strVSDSignature = ServerBussinessCA.CombineData(v_strData, mv_oSignServer)

            '05072021: HaNM5 sua ve ky tren chuoi Hash
            Dim v_sha = New SHA1CryptoServiceProvider
            If v_strReportDataKey <> String.Empty Then
                Dim v_arrByteTmp() As Byte = ZetaCompressionLibrary.CompressionHelper.CompressDataSet(CoreBusiness.objMaster.ReportDataSets(v_strReportDataKey))
                pv_strDataHash = Convert.ToBase64String(v_sha.ComputeHash(v_arrByteTmp))
            Else
                pv_strDataHash = Convert.ToBase64String(v_sha.ComputeHash(pv_arrByte))
            End If
            v_strVSDSignature = ServerBussinessCA.CombineData(pv_strDataHash, mv_oSignServer)

            'pv_strMessage = v_strCAKey
            pv_strMessage = v_xmlDocumentMessage.InnerXml
            Return v_lngErr
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                & "Error code: System error!" & vbNewLine _
                & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_HOST)
            Return ERR_SYSTEM_START
        Finally
            If Not v_obj Is Nothing Then
                v_obj.Dispose()
            End If
            If Not v_ds Is Nothing Then
                v_ds.Dispose()
            End If
            GC.Collect()
            GC.GetTotalMemory(False)
            GC.Collect()
        End Try
    End Function

    Public Function SaveFileRptCA(ByVal v_arrByte() As Byte, ByVal v_strVSDSignature As String, ByVal v_strClientVSDSignature As String, ByVal v_strTLName As String, ByVal v_strRptId As String, ByRef v_strLocalDir As String, ByRef v_strFileName As String, ByVal v_strStatus As String) As Long Implements CommonLibrary.IReportWCF.SaveFileRptCA
        Dim v_strErrorSource As String = "HOSTDelivery.Message(Byte)", v_strErrorMessage As String = ""
        Dim lngErr As Long = ERR_SYSTEM_START
        Dim v_obj As Object = Nothing

        Try
            'HaNM5: 08/12/2020
            HOSTReportChanel.AppLogger.Info("SaveFileRptCA")

            Dim v_strDS As String = Decompress(v_arrByte)
            v_obj = New Host.SystemAdmin
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
            v_attrClientSignature.Value = v_strClientVSDSignature
            dataElement.Attributes.Append(v_attrClientSignature)

            XMLDocumentMessage.AppendChild(dataElement)
            'Dim pv_strMessageSave As String = "<DATA>" & ByteArrayToStr(v_arrByte) & "</DATA>" _
            '                    & "<" & gc_AtributeSignatureVSD & ">" & v_strVSDSignature & "<" & gc_AtributeSignatureVSD & ">" _
            '                    & "<" & gc_AtributeSignatureClient & ">" & v_strClientVSDSignature & "<" & gc_AtributeSignatureClient & ">"
            lngErr = v_obj.SaveFileCA(XMLDocumentMessage.InnerXml, v_strTLName, v_strRptId, v_strLocalDir, v_strFileName, v_strStatus)

            Return lngErr
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                & "Error code: System error!" & vbNewLine _
                & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_HOST)
            Return lngErr
        Finally
            GC.Collect()
            GC.GetTotalMemory(False)
            GC.Collect()
        End Try
    End Function
#End Region
End Class