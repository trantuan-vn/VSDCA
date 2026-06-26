Imports SATS.CommonLibrary
Imports SATS.CoreBusiness
'Imports System.EnterpriseServices

'<JustInTimeActivation(False), _
'Transaction(TransactionOption.Disabled), _
'ObjectPooling(Enabled:=True, MinPoolSize:=30)> _
Public Class FLDVAL
    Inherits CoreBusiness.objMaster
    Implements CoreBusiness.IMaster

    Public Sub New()
        ATTR_TABLE = "FLDVAL"
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
        Inquiry = CoreInquiry(pv_xmlDocument)
        'ContextUtil.SetComplete()
    End Function
End Class
