' This is a set of classes that define the structure of the intermediate output of the
' parser and resolver classes.  It also contains the type-safe collection classes.

'   TableSource - the parsed output of the FROM clause
'   SelectList - the parsed output of the SELECT clause
'   SearchCondition - the parsed output of the WHERE clause
'   GroupByExpression - the parsed output of the GROUP BY clause
'   OrderByExpression - the parsed output of the ORDER By clause

Public Class TableSource
    ' This holds the parts of tables that are used in the FROM clause.  It includes a
    ' place to record the details of the join parameters.  Example:
    '
    ' LeftTable AS LeftAlias
    ' INNER JOIN
    ' RightTable AS RightAlias
    ' ON LPTable.LPColumn = RPTable.RPColumn

    Private _JoinType As String             ' the type of join (INNER, LEFT, etc)
    Private _LeftTable As String
    Private _LeftAlias As String
    Private _RightTable As String
    Private _RightAlias As String
    Private _LPTable As String              ' 4 parts of the join "predicate"
    Private _LPColumn As String             ' LP = Left Predicate
    Private _RPTable As String              ' RP = Right Predicate
    Private _RPColumn As String

    Public Property JoinType() As String
        Get
            Return _JoinType
        End Get
        Set(ByVal Value As String)
            _JoinType = Value
        End Set
    End Property

    Public Property LeftTable() As String
        Get
            Return _LeftTable
        End Get
        Set(ByVal Value As String)
            _LeftTable = Value
        End Set
    End Property

    Public Property LeftAlias() As String
        Get
            Return _LeftAlias
        End Get
        Set(ByVal Value As String)
            _LeftAlias = Value
        End Set
    End Property

    Public Property RightTable() As String
        Get
            Return _RightTable
        End Get
        Set(ByVal Value As String)
            _RightTable = Value
        End Set
    End Property

    Public Property RightAlias() As String
        Get
            Return _RightAlias
        End Get
        Set(ByVal Value As String)
            _RightAlias = Value
        End Set
    End Property

    Public Property LeftPredicateTable() As String
        Get
            Return _LPTable
        End Get
        Set(ByVal Value As String)
            _LPTable = Value
        End Set
    End Property

    Public Property LeftPredicateColumn() As String
        Get
            Return _LPColumn
        End Get
        Set(ByVal Value As String)
            _LPColumn = Value
        End Set
    End Property

    Public Property RightPredicateTable() As String
        Get
            Return _RPTable
        End Get
        Set(ByVal Value As String)
            _RPTable = Value
        End Set
    End Property

    Public Property RightPredicateColumn() As String
        Get
            Return _RPColumn
        End Get
        Set(ByVal Value As String)
            _RPColumn = Value
        End Set
    End Property
End Class

Public Class TableSourceCollection
    Inherits CollectionBase

    Default Public Property Item(ByVal index As Integer) As TableSource
        Get
            Return CType(List(index), TableSource)
        End Get
        Set(ByVal Value As TableSource)
            List(index) = Value
        End Set
    End Property

    Public Function IndexOf(ByVal value As TableSource) As Integer
        Return List.IndexOf(value)
    End Function

    Public Sub Add(ByVal ts As TableSource)
        list.Add(ts)
    End Sub

    Public Sub Insert(ByVal index As Integer, ByVal value As TableSource)
        List.Insert(index, value)
    End Sub

    Public Sub Remove(ByVal value As TableSource)
        List.Remove(value)
    End Sub
End Class

Public Class SelectList
    ' This holds the list of columns that will appear in the final output.  Example:
    '
    ' TableName.ColumnName AS ColumnAlias

    Private _TableName As String
    Private _ColumnName As String
    Private _ColumnAlias As String
    Private _Expression As String       ' stash the expressions here
    Private _IsAggregate As Boolean     ' is this a "vector aggregate" expression
    Private _DataType As String         ' expressions must have a return type

    Public Property TableName() As String
        Get
            Return _TableName
        End Get
        Set(ByVal Value As String)
            _TableName = Value
        End Set
    End Property
    Public Property ColumnName() As String
        Get
            Return _ColumnName
        End Get
        Set(ByVal Value As String)
            _ColumnName = Value
        End Set
    End Property
    Public Property ColumnAlias() As String
        Get
            Return _ColumnAlias
        End Get
        Set(ByVal Value As String)
            _ColumnAlias = Value
        End Set
    End Property
    Public Property Expression() As String
        Get
            Return _Expression
        End Get
        Set(ByVal Value As String)
            _Expression = Value
        End Set
    End Property
    Public Property IsAggregate() As Boolean
        Get
            Return _IsAggregate
        End Get
        Set(ByVal Value As Boolean)
            _IsAggregate = Value
        End Set
    End Property
    Public Property DataType() As String
        Get
            Return _DataType
        End Get
        Set(ByVal Value As String)
            _DataType = Value
        End Set
    End Property
End Class

Public Class SelectListCollection
    Inherits CollectionBase

    Default Public Property Item(ByVal index As Integer) As SelectList
        Get
            Return CType(List(index), SelectList)
        End Get
        Set(ByVal Value As SelectList)
            List(index) = Value
        End Set
    End Property

    Public Function IndexOf(ByVal value As SelectList) As Integer
        Return List.IndexOf(value)
    End Function

    Public Sub Add(ByVal ts As SelectList)
        list.Add(ts)
    End Sub

    Public Sub Insert(ByVal index As Integer, ByVal value As SelectList)
        List.Insert(index, value)
    End Sub

    Public Sub Remove(ByVal value As SelectList)
        List.Remove(value)
    End Sub
End Class

Public Class SearchCondition
    ' This holds the parts of the search conditions found in the WHERE clause.  Example:
    '
    ' TableName.ColumnName > 12

    Private _Level As Integer                ' level of parentheses
    Private _Operator As String              ' the comparison operator (i.e., "=")
    Private _Expression As String
    Private _ExpAlias As String              ' made up alias if you didn't provide one
    Private _DataType As String              ' if an expression, we need the data type
    Private _NumArgs As Integer              ' the number of arguments expected
    Private _TableName As String
    Private _ColumnName As String            ' one of the arguments is a column
    Private _TempColumn As String            ' pre-formatted column name for TempTable
    Private _Arg1 As String                  ' the 2nd argument, etc
    Private _Arg2 As String

    Public Property Level() As Integer
        Get
            Return _Level
        End Get
        Set(ByVal Value As Integer)
            _Level = Value
        End Set
    End Property

    Public Property [Operator]() As String
        Get
            Return _Operator
        End Get
        Set(ByVal Value As String)
            _Operator = Value
        End Set
    End Property

    Public Property Expression() As String
        Get
            Return _Expression
        End Get
        Set(ByVal Value As String)
            _Expression = Value
        End Set
    End Property

    Public Property ExpAlias() As String
        Get
            Return _ExpAlias
        End Get
        Set(ByVal Value As String)
            _ExpAlias = Value
        End Set
    End Property

    Public Property DataType() As String
        Get
            Return _DataType
        End Get
        Set(ByVal Value As String)
            _DataType = Value
        End Set
    End Property

    Public Property NumArgs() As Integer
        Get
            Return _NumArgs
        End Get
        Set(ByVal Value As Integer)
            _NumArgs = Value
        End Set
    End Property

    Public Property TableName() As String
        Get
            Return _TableName
        End Get
        Set(ByVal Value As String)
            _TableName = Value
        End Set
    End Property

    Public Property ColumnName() As String
        Get
            Return _ColumnName
        End Get
        Set(ByVal Value As String)
            _ColumnName = Value
        End Set
    End Property

    Public Property TempColumn() As String
        Get
            Return _TempColumn
        End Get
        Set(ByVal Value As String)
            _TempColumn = Value
        End Set
    End Property

    Public Property Arg1() As String
        Get
            Return _Arg1
        End Get
        Set(ByVal Value As String)
            _Arg1 = Value
        End Set
    End Property

    Public Property Arg2() As String
        Get
            Return _Arg2
        End Get
        Set(ByVal Value As String)
            _Arg2 = Value
        End Set
    End Property
End Class

Public Class SearchConditionCollection
    Inherits CollectionBase

    Default Public Property Item(ByVal index As Integer) As SearchCondition
        Get
            Return CType(List(index), SearchCondition)
        End Get
        Set(ByVal Value As SearchCondition)
            List(index) = Value
        End Set
    End Property

    Public Function IndexOf(ByVal value As SearchCondition) As Integer
        Return List.IndexOf(value)
    End Function

    Public Sub Add(ByVal ts As SearchCondition)
        list.Add(ts)
    End Sub

    Public Sub Insert(ByVal index As Integer, ByVal value As SearchCondition)
        List.Insert(index, value)
    End Sub

    Public Sub Remove(ByVal value As SearchCondition)
        List.Remove(value)
    End Sub
End Class

Public Class GroupByExpression
    ' This holds the parts of the GROUP BY clause.  Example:
    '
    ' TableName.ColumnName

    Private _TableName As String
    Private _ColumnName As String
    Private _TempColumn As String            ' pre-formatted column name for TempTable
    Private _Expression As String            ' could be an expression
    Private _DataType As String              ' if an expression, we need the data type

    Public Property TableName() As String
        Get
            Return _TableName
        End Get
        Set(ByVal Value As String)
            _TableName = Value
        End Set
    End Property

    Public Property ColumnName() As String
        Get
            Return _ColumnName
        End Get
        Set(ByVal Value As String)
            _ColumnName = Value
        End Set
    End Property

    Public Property TempColumn() As String
        Get
            Return _TempColumn
        End Get
        Set(ByVal Value As String)
            _TempColumn = Value
        End Set
    End Property

    Public Property Expression() As String
        Get
            Return _Expression
        End Get
        Set(ByVal Value As String)
            _Expression = Value
        End Set
    End Property

    Public Property DataType() As String
        Get
            Return _DataType
        End Get
        Set(ByVal Value As String)
            _DataType = Value
        End Set
    End Property
End Class

Public Class GroupByExpressionCollection
    Inherits CollectionBase

    Default Public Property Item(ByVal index As Integer) As GroupByExpression
        Get
            Return CType(List(index), GroupByExpression)
        End Get
        Set(ByVal Value As GroupByExpression)
            List(index) = Value
        End Set
    End Property

    Public Function IndexOf(ByVal value As GroupByExpression) As Integer
        Return List.IndexOf(value)
    End Function

    Public Sub Add(ByVal ts As GroupByExpression)
        list.Add(ts)
    End Sub

    Public Sub Insert(ByVal index As Integer, ByVal value As GroupByExpression)
        List.Insert(index, value)
    End Sub

    Public Sub Remove(ByVal value As GroupByExpression)
        List.Remove(value)
    End Sub
End Class

Public Class OrderByExpression
    ' This holds the parts of the ORDER BY clause.  Example:
    '
    ' TableName.ColumnName ASC

    Private _TableName As String
    Private _ColumnName As String
    Private _SortOrder As String             ' ASC, DESC
    Private _ColumnIndex As Integer          ' column number (1 based)

    Public Property TableName() As String
        Get
            Return _TableName
        End Get
        Set(ByVal Value As String)
            _TableName = Value
        End Set
    End Property

    Public Property ColumnName() As String
        Get
            Return _ColumnName
        End Get
        Set(ByVal Value As String)
            _ColumnName = Value
        End Set
    End Property

    Public Property SortOrder() As String
        Get
            Return _SortOrder
        End Get
        Set(ByVal Value As String)
            _SortOrder = Value
        End Set
    End Property

    Public Property ColumnIndex() As Integer
        Get
            Return _ColumnIndex
        End Get
        Set(ByVal Value As Integer)
            _ColumnIndex = Value
        End Set
    End Property
End Class

Public Class OrderByExpressionCollection
    Inherits CollectionBase

    Default Public Property Item(ByVal index As Integer) As OrderByExpression
        Get
            Return CType(List(index), OrderByExpression)
        End Get
        Set(ByVal Value As OrderByExpression)
            List(index) = Value
        End Set
    End Property

    Public Function IndexOf(ByVal value As OrderByExpression) As Integer
        Return List.IndexOf(value)
    End Function

    Public Sub Add(ByVal ts As OrderByExpression)
        list.Add(ts)
    End Sub

    Public Sub Insert(ByVal index As Integer, ByVal value As OrderByExpression)
        List.Insert(index, value)
    End Sub

    Public Sub Remove(ByVal value As OrderByExpression)
        List.Remove(value)
    End Sub
End Class