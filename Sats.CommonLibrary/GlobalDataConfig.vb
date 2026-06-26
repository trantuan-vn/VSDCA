Public Class GlobalDataConfig
    Public Shared mv_oHOST_DBCONFIG As DBInfo
    Private Shared mv_oINQUERY_DBCONFIG As DBInfo
    Public Shared Property HOST_DBCONFIG() As DBInfo
        Get
            Return mv_oHOST_DBCONFIG
        End Get
        Set(value As DBInfo)
            mv_oHOST_DBCONFIG = value
        End Set
    End Property

    Public Shared Property INQUERY_DBCONFIG() As DBInfo
        Get
            Return mv_oINQUERY_DBCONFIG
        End Get
        Set(value As DBInfo)
            mv_oINQUERY_DBCONFIG = value
        End Set
    End Property
End Class
