<Serializable()> _
Public Class BusinessCommand
    Private mv_strUser As String
    Private mv_dtmExecuteDate As Date
    Private mv_strExecuteTime As String
    Private mv_strSQL As String
    Private mv_blnHasTransaction As Boolean

    Public Sub New()
        mv_strUser = String.Empty
        mv_dtmExecuteDate = Now.Date
        mv_strExecuteTime = Format(Now.Hour, "00") & ":" & Format(Now.Minute, "00") & ":" & Format(Now.Second, "00")
        mv_strSQL = String.Empty
        mv_blnHasTransaction = False
    End Sub

    Public Property ExecuteUser() As String
        Get
            Return mv_strUser
        End Get
        Set(ByVal Value As String)
            mv_strUser = Value
        End Set
    End Property

    Public Property ExecuteDate() As Date
        Get
            Return mv_dtmExecuteDate
        End Get
        Set(ByVal Value As Date)
            mv_dtmExecuteDate = Value
        End Set
    End Property

    Public Property ExecuteTime() As String
        Get
            Return mv_strExecuteTime
        End Get
        Set(ByVal Value As String)
            mv_strExecuteTime = Value
        End Set
    End Property

    Public Property SQLCommand() As String
        Get
            Return mv_strSQL
        End Get
        Set(ByVal Value As String)
            mv_strSQL = Value
        End Set
    End Property

    Public Property HasTransaction() As Boolean
        Get
            Return mv_blnHasTransaction
        End Get
        Set(ByVal Value As Boolean)
            mv_blnHasTransaction = Value
        End Set
    End Property
End Class
