<Serializable()> _
Public Class CFieldVal
#Region " Declaration "
    Private mv_intOdrNum As Integer
    Private mv_intFldMaster As Integer
    Private mv_strFldName As String
    Private mv_strObjName As String
    Private mv_strValType As String
    Private mv_strOperator As String
    Private mv_strValExp As String
    Private mv_strValExp2 As String
    Private mv_strErrMsg As String
    Private mv_strENErrMsg As String
#End Region

#Region " Constructors and deconstructors "
    Public Sub New()
        mv_intOdrNum = 0
        mv_strFldName = String.Empty
    End Sub

    Public Overloads Sub Dispose()
        mv_intOdrNum = 0
        mv_strFldName = String.Empty
    End Sub
#End Region

#Region " Properties "
    Public Property ORDNUM() As Integer
        Get
            Return mv_intOdrNum
        End Get
        Set(ByVal Value As Integer)
            mv_intOdrNum = Value
        End Set
    End Property

    Public Property IDXFLD() As Integer
        Get
            Return mv_intFldMaster
        End Get
        Set(ByVal Value As Integer)
            mv_intFldMaster = Value
        End Set
    End Property

    Public Property OBJNAME() As String
        Get
            Return mv_strObjName
        End Get
        Set(ByVal Value As String)
            mv_strObjName = Value
        End Set
    End Property

    Public Property FLDNAME() As String
        Get
            Return mv_strFldName
        End Get
        Set(ByVal Value As String)
            mv_strFldName = Value
        End Set
    End Property

    Public Property VALTYPE() As String
        Get
            Return mv_strValType
        End Get
        Set(ByVal Value As String)
            mv_strValType = Value
        End Set
    End Property

    Public Property mp_OPERATOR() As String
        Get
            Return mv_strOperator
        End Get
        Set(ByVal Value As String)
            mv_strOperator = Value
        End Set
    End Property

    Public Property VALEXP() As String
        Get
            Return mv_strValExp
        End Get
        Set(ByVal Value As String)
            mv_strValExp = Value
        End Set
    End Property

    Public Property VALEXP2() As String
        Get
            Return mv_strValExp2
        End Get
        Set(ByVal Value As String)
            mv_strValExp2 = Value
        End Set
    End Property

    Public Property ERRMSG() As String
        Get
            Return mv_strErrMsg
        End Get
        Set(ByVal Value As String)
            mv_strErrMsg = Value
        End Set
    End Property

    Public Property EN_ERRMSG() As String
        Get
            Return mv_strENErrMsg
        End Get
        Set(ByVal Value As String)
            mv_strENErrMsg = Value
        End Set
    End Property
#End Region
End Class
