<Serializable()> _
Public Class DBInfo
    Private mv_strUserName As String = "HOST10"
    Private mv_strPassword As String = "HOST10"
    Private mv_strDataSource As String = "SATSHN"

    Public Sub New()
        mv_strUserName = ""
        mv_strPassword = ""
        mv_strDataSource = ""
    End Sub

    Public Sub New(ByVal pv_strUserName As String, ByVal pv_strPassword As String, ByVal pv_strDataSource As String)
        Me.UserName = pv_strUserName
        Me.Password = pv_strPassword
        Me.DataSource = pv_strDataSource
    End Sub

    Public Property UserName() As String
        Get
            Return mv_strUserName
        End Get
        Set(value As String)
            mv_strUserName = value
        End Set
    End Property
    Public Property Password() As String
        Get
            Return mv_strPassword
        End Get
        Set(value As String)
            mv_strPassword = value
        End Set
    End Property
    Public Property DataSource() As String
        Get
            Return mv_strDataSource
        End Get
        Set(value As String)
            mv_strDataSource = value
        End Set
    End Property
End Class
