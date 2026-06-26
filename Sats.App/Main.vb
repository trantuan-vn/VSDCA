Module Main
    Public blnLogin As Boolean
    Public m_ResourceManager As Resources.ResourceManager
    Public m_BusLayer As CBusLayer = Nothing
    Public tickCount As Decimal
    Public m_blnIsOnline As Boolean = True
    Public mv_strMsg As String
    Public frmTask As TaskList
    'WCF
    Public mv_oLocal As CommonLibrary.Client
    Public pv_oProxy As BDSChannel.BDSDelivery
End Module
