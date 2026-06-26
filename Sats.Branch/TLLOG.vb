Imports Sats.CommonLibrary
Imports Sats.DataAccessLayer

'Imports System.EnterpriseServices
'<JustInTimeActivation(False), _
'Transaction(TransactionOption.Disabled), _
'ObjectPooling(Enabled:=True, MinPoolSize:=30)> _
Public Class TLLOG
    Inherits CoreBusiness.objMaster
    Implements CoreBusiness.IMaster

    Public Sub New()
        ATTR_TABLE = "TLLOG"
        'ContextUtil.SetComplete()
    End Sub


    Public Function Add(ByRef pv_xmlDocument As XmlDocumentEx) As Long Implements CoreBusiness.IMaster.Add
        'ContextUtil.SetComplete()
    End Function

    Public Function Adhoc(ByRef pv_xmlDocument As XmlDocumentEx) As Long Implements CoreBusiness.IMaster.Adhoc
        'ContextUtil.SetComplete()
    End Function

    Public Function Delete(ByRef pv_xmlDocument As XmlDocumentEx) As Long Implements CoreBusiness.IMaster.Delete
        'ContextUtil.SetComplete()
    End Function

    Public Function Edit(ByRef pv_xmlDocument As XmlDocumentEx) As Long Implements CoreBusiness.IMaster.Edit
        'ContextUtil.SetComplete()
    End Function

    Public Function Inquiry(ByRef pv_xmlDocument As XmlDocumentEx) As Long Implements CoreBusiness.IMaster.Inquiry
        Dim v_lngErrCode As Long

        Try
            v_lngErrCode = CoreInquiry(pv_xmlDocument)
            'If v_lngErrCode <> 0 Then
            '    Dim v_strErrorSource, v_strErrorMessage As String

            '    v_strErrorSource = "TLLOG.Inquiry"
            '    v_strErrorMessage = String.Empty

            '    LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
            '                 & "Error code: " & v_lngErrCode.ToString() & vbNewLine _
            '                 & "Error message: " & v_strErrorMessage, EventLogEntryType.Information)
            '    BuildXMLErrorException(pv_xmlDocument, v_strErrorSource, v_lngErrCode, v_strErrorMessage)
            'End If

            'ContextUtil.SetComplete()
            Return v_lngErrCode
        Catch ex As Exception
            'ContextUtil.SetAbort()
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            '             & "Error code: System error!" & vbNewLine _
            '             & "Error message: " & ex.Message, EventLogEntryType.Error)
            Throw ex
        End Try
    End Function

End Class
