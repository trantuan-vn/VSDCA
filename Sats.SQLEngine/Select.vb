' This class supports a minimal set of SQL SELECT command statements against a group of
' ADO.Net DataTables contained in a disconnected DataSet.  The purpose is to prevent a
' round trip to the server to accomplish simple things like what is available with the
' JOIN and GROUP BY clauses of the SELECT command.
' 
' This is *NOT* a complete SQL Engine... in fact, it's not even a complete implementation
' of just the SELECT command.  Consequently, there is no support for any DDL statements
' (ALTER, CREATE, or DROP) nor any other DML statements (INSERT, UPDATE, or DELETE) nor
' any DCL statements (GRANT, DENY, or REVOKE).
'
' The syntax for the SELECT command is designed around the SQL Server 2000 flavor of the
' SQL language.  There is certainly no claim of compliance with any SQL standard.  The
' most significant limitation is the rather poor support for SQL Server 2000's huge list
' of functions.  The tiny list functions that *are* supported here are shown below.
'
' Operators, Functions, and Expressions are limited to what is natively supported (or
' easily translated) by ADO.Net, namely:
'
'   Comparison: <, >, <=, >=, <>, =, IN, LIKE, IS NULL
'   Logical: AND, OR, NOT
'   Math: +, -, *, /, %
'   String: +
'   Wildcards: *, %, []
'   Aggregation: SUM, AVG, MIN, MAX, COUNT, STDEV, VAR
'   Functions: LEN, ISNULL, IIF, TRIM, SUBSTRING, CONVERT
'
' The supported (and unsupported) syntax for each of the SQL clauses is documented in the
' Parse.vb file at each of the "clause parser" methods.

Public Class SelectCommand

    ' SELECT select_list
    ' FROM table_source 
    ' [ WHERE search_condition ] 
    ' [ GROUP BY group_by_expression ] 
    ' [ HAVING search_condition ] 
    ' [ ORDER BY order_expression ] 

    Private ds As DataSet
    Friend TableList As ArrayList
    Friend ColumnList As ArrayList

    Private _Command As String = ""
    Private _SelectArgs(-1) As String
    Private _slc As SelectListCollection
    Private _tsc As TableSourceCollection
    Private _scc As SearchConditionCollection
    Private _gbc As GroupByExpressionCollection
    Private _Having As String = ""
    Private _obc As OrderByExpressionCollection

    Public Property Command() As String
        Get
            Return _Command
        End Get
        Set(ByVal Value As String)
            _Command = Value
        End Set
    End Property

    Public Property SelectArgs() As String()
        Get
            Return _SelectArgs
        End Get
        Set(ByVal Value As String())
            _SelectArgs = Value
        End Set
    End Property

    Public Property SelectList() As SelectListCollection
        Get
            Return _slc
        End Get
        Set(ByVal Value As SelectListCollection)
            _slc = Value
        End Set
    End Property

    Public Property From() As TableSourceCollection
        Get
            Return _tsc
        End Get
        Set(ByVal Value As TableSourceCollection)
            _tsc = Value
        End Set
    End Property

    Public Property Where() As SearchConditionCollection
        Get
            Return _scc
        End Get
        Set(ByVal Value As SearchConditionCollection)
            _scc = Value
        End Set
    End Property

    Public Property GroupBy() As GroupByExpressionCollection
        Get
            Return _gbc
        End Get
        Set(ByVal Value As GroupByExpressionCollection)
            _gbc = Value
        End Set
    End Property

    Public Property Having() As String
        Get
            Return _Having
        End Get
        Set(ByVal Value As String)
            _Having = Value
        End Set
    End Property

    Public Property OrderBy() As OrderByExpressionCollection
        Get
            Return _obc
        End Get
        Set(ByVal Value As OrderByExpressionCollection)
            _obc = Value
        End Set
    End Property

    Public Sub New(ByVal ds1 As DataSet)
        ds = ds1
        TableList = New ArrayList
        ColumnList = New ArrayList
        _tsc = New TableSourceCollection
        _slc = New SelectListCollection
        _scc = New SearchConditionCollection
        _gbc = New GroupByExpressionCollection
        _obc = New OrderByExpressionCollection
    End Sub

    'Do what we came here to do... execute the Select Command 
    'Supported List :  
    'Comparison: <, >, <=, >=, <>, =, IN, LIKE, IS NULL
    'Logical: AND, OR, NOT
    'Math: +, -, *, /, %
    'String: +
    'Wildcards: *, %, []
    'Aggregation: SUM, AVG, MIN, MAX, COUNT, STDEV, VAR
    'Functions: LEN, ISNULL, IIF, TRIM, SUBSTRING, CONVERT
    Public Function Execute(ByVal SQL As String) As DataTable
        Dim dt As DataTable
        Dim p As New Parser(Me)
        Dim r As New Resolver(ds, Me)
        Dim e As New Executer(ds, Me)

        ' some quick sanity checking...
        If IsNothing(ds) Then
            Throw New ApplicationException("Input error: Can't execute the SQL statement, not a valid DataSet")
        End If

        ' Let's parse the input...
        p.ParseSQL(SQL)

        ' Check it against the DataSet, fix ambiguities, fill in blanks, etc.
        r.ResolveSQL()

        ' Execute the SQL command
        dt = e.Execute()

        Return dt
    End Function

End Class


