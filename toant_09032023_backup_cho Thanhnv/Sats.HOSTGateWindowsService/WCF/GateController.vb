Imports Sats.DataAccessLayer
Imports Sats.CommonLibrary
Imports Sats.Interface4Gate
Imports Sats.Utils

Public Class GateController
    Implements IWCF4Gate

    Public Function SendMessage(ByRef message As String) As Long Implements IWCF4Gate.SendMessage
        Dim v_obj As Host.txRouter
        Try
            v_obj = New Host.txRouter
            Dim v_xmlDocumentMessage As New XmlDocumentEx()
            v_xmlDocumentMessage.LoadXml(message)
            Dim v_strBRID As String = v_xmlDocumentMessage.DocumentElement.Attributes(gc_AtributeBRID).Value.ToString
            v_xmlDocumentMessage.DocumentElement.Attributes(gc_AtributeTXDATE).InnerText = GetBusdate(v_strBRID)
            Dim lngErr As Long = v_obj.Transact(v_xmlDocumentMessage, mv_oSignServer)
            message = v_xmlDocumentMessage.InnerXml
            Return lngErr
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                & "Error code: System error!" & vbNewLine _
                & "Error message: " & ex.Message, EventLogEntryType.Error, gc_MODULE_HOST)
            Return ERR_SYSTEM_START
        Finally
            If Not v_obj Is Nothing Then
                v_obj.Dispose()
            End If
            GC.Collect()
            GC.GetTotalMemory(False)
            GC.Collect()
        End Try
    End Function

    Public Function GetData(ByVal sql As String) As DataContainer Implements IWCF4Gate.GetData
        Try
            Dim container As New DataContainer()
            Dim v_obj As New DataAccess
            v_obj.NewDBInstance(gc_MODULE_HOST)
            Dim ds As DataSet = v_obj.ExecuteSQLReturnDataset(CommandType.Text, sql)
            container.DataSet = ds
            Return container
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                & "Error code: System error!" & vbNewLine _
                & "Error message: " & ex.Message, EventLogEntryType.Error, "STP")
            Throw ex
        End Try
    End Function

    Public Function GetSearch(ByVal message As String) As DataContainer Implements IWCF4Gate.GetSearch
        Try
            Dim xml As New XmlDocumentEx
            Dim obj As New Host.objRouter
            xml.LoadXml(message)
            Dim ds As DataSet
            ds = obj.GetSearch4STP(xml)
            Dim container As New DataContainer
            container.DataSet = ds
            Return container
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                & "Error code: System error!" & vbNewLine _
                & "Error message: " & ex.Message, EventLogEntryType.Error, "STP")
            Throw ex
        End Try
    End Function

    Public Function CreateReport(ByVal message As String) As DataContainer Implements IWCF4Gate.CreateReport
        Try
            Dim xml As New XmlDocumentEx
            xml.LoadXml(message)
            Dim v_obj As RP.Report = New RP.Report
            Dim v_strReportDataKey As String = String.Empty
            Dim v_ds As DataSet = v_obj.CreateReport(xml, v_strReportDataKey)
            Dim container As New DataContainer
            container.DataSet = v_ds
            Return container
        Catch ex As Exception
            LogError.Write("Error source: " & ex.Source & vbNewLine _
                & "Error code: System error!" & vbNewLine _
                & "Error message: " & ex.Message, EventLogEntryType.Error, "STP")
            Throw ex
        End Try
    End Function

    Public Function GetBusdate(ByVal pv_strBRID As String) As String
        Dim v_obj As New DataAccess
        Dim v_strCurrDate As String = ""
        v_obj.NewDBInstance(gc_MODULE_HOST)
        Dim v_lngErrCode As Long = v_obj.GetSysVar("SYSTEM", "CURRDATE", pv_strBRID, v_strCurrDate)
        Return v_strCurrDate
    End Function
End Class
