' This class is designed to resolve the parsed SQL input into a set of valid commands
' that can be used against the native ADO.Net methods.  This requires a lot of
' translation of clauses, operators, and keywords.

Public Class Resolver

    Private ds As DataSet
    Private sc As SelectCommand

    Private TableAlias As New Hashtable
    Private ColumnAlias As New ArrayList
    Private HasAsterick As Boolean

    Public Sub New(ByVal ds1 As DataSet, ByVal sc1 As SelectCommand)
        ds = ds1
        sc = sc1
    End Sub

    ' There is a checklist of things to do:
    ' 1) resolve any table aliases
    ' 2) check the table names against the DataSet (tweak if necessary)
    ' 3) fill in any missing table names
    ' 4) check the column names against the DataSet (tweak if necessary)
    ' 5) translate column names, operators, etc in expressions
    ' 6) translate a few things to meet the ADO.Net native features
    Public Sub ResolveSQL()
        Dim i, j As Integer
        Dim sl As SelectList
        Dim ts As TableSource
        Dim wh As SearchCondition
        Dim gb As GroupByExpression
        Dim buf, temp As String
        Dim found As Boolean

        ' let's create a default select list if we don't already have one
        If sc.SelectList.Count = 0 Then
            sl = New SelectList
            sl.ColumnName = "*"
            sc.SelectList.Add(sl)
        End If

        ' First we generate a mapping of the Table aliases to their real names for use
        ' in the next section
        For Each ts In sc.From
            If ts.RightAlias <> "" Then
                temp = ts.RightAlias.ToLower
                If Not TableAlias.Contains(temp) Then
                    TableAlias.Add(temp, ts.RightTable)
                End If
            End If
            If ts.LeftAlias <> "" Then
                temp = ts.LeftAlias.ToLower
                If Not TableAlias.Contains(temp) Then
                    TableAlias.Add(temp, ts.LeftTable)
                End If
            End If
        Next

        ' Build a similar list for the Column aliases.  This is just a simple list, 
        ' since we can't really "resolve" expression aliases.
        For Each sl In sc.SelectList
            If sl.ColumnAlias <> "" Then
                temp = sl.ColumnAlias.ToLower
                If Not ColumnAlias.Contains(temp) Then
                    ColumnAlias.Add(temp)
                End If
            End If
        Next

        ' Next we resolve Table information. We tweak the table name to exactly match the
        ' DataSet and also generate a list of unique TableNames that we use in the next
        ' steps.
        For i = 0 To sc.SelectList.Count - 1
            ProcessTables(sc.SelectList(i).TableName)
        Next
        For i = 0 To sc.From.Count - 1
            ProcessTables(sc.From(i).LeftTable)
            ProcessTables(sc.From(i).RightTable)
            ProcessTables(sc.From(i).LeftPredicateTable)
            ProcessTables(sc.From(i).RightPredicateTable)
        Next
        For i = 0 To sc.Where.Count - 1
            ProcessTables(sc.Where(i).TableName)
        Next
        For i = 0 To sc.GroupBy.Count - 1
            ProcessTables(sc.GroupBy(i).TableName)
        Next
        For i = 0 To sc.OrderBy.Count - 1
            ProcessTables(sc.OrderBy(i).TableName)
        Next

        ' Next we resolve the Column information.  We fill in any blank TableNames,
        ' verify and tweak the ColumnNames just like we did for the TableNames above.
        For i = 0 To sc.SelectList.Count - 1
            ProcessColumns(sc.SelectList(i).TableName, sc.SelectList(i).ColumnName, buf, False)
        Next
        For i = 0 To sc.From.Count - 1
            ProcessColumns(sc.From(i).LeftPredicateTable, sc.From(i).LeftPredicateColumn, buf, False)
            ProcessColumns(sc.From(i).RightPredicateTable, sc.From(i).RightPredicateColumn, buf, False)
        Next
        For i = 0 To sc.Where.Count - 1
            ProcessColumns(sc.Where(i).TableName, sc.Where(i).ColumnName, sc.Where(i).TempColumn, False)
        Next
        For i = 0 To sc.GroupBy.Count - 1
            ProcessColumns(sc.GroupBy(i).TableName, sc.GroupBy(i).ColumnName, sc.GroupBy(i).TempColumn, False)
        Next
        For i = 0 To sc.OrderBy.Count - 1
            ' ORDER BY is the only clause that supports column aliases.  So we attempt
            ' resolve the alias for non-function columns.
            ProcessColumns(sc.OrderBy(i).TableName, sc.OrderBy(i).ColumnName, buf, True)
        Next

        ' The next step is to parse, resolve, and tweak the table/column names inside
        ' the expressions.  The techniques used are the same as those found in the
        ' Parser class.
        For i = 0 To sc.SelectList.Count - 1
            ProcessExpression(sc.SelectList(i).Expression, sc.SelectList(i).DataType)
        Next
        For i = 0 To sc.Where.Count - 1
            ProcessExpression(sc.Where(i).Expression, sc.Where(i).DataType)
        Next
        For i = 0 To sc.GroupBy.Count - 1
            ProcessExpression(sc.GroupBy(i).Expression, sc.GroupBy(i).DataType)
        Next
        ProcessExpression(sc.Having, buf)

        ' For ease of mapping column names, we expand any "*" columns
        If HasAsterick Then
            Dim col As DataColumn
            Dim TableName As String

            ' use inserts to put the list in the proper location
            For i = 0 To sc.SelectList.Count - 1
                If sc.SelectList(i).ColumnName = "*" Then
                    Dim TempList As ArrayList
                    TableName = sc.SelectList(i).TableName
                    ' if you didn't specify a tablename, and there is more than one
                    ' table in the From list, then we need multiple '*' entries... one
                    ' for each table.
                    If TableName <> "" Then
                        TempList = New ArrayList
                        TempList.Add(TableName)
                    Else
                        TempList = sc.TableList
                    End If

                    For Each TableName In TempList
                        sc.SelectList.RemoveAt(i)
                        j = 0
                        For Each col In ds.Tables(TableName).Columns
                            sl = New SelectList
                            sl.ColumnName = col.ColumnName
                            sl.TableName = TableName
                            sl.IsAggregate = False
                            sc.SelectList.Insert(i + j, sl)
                            j += 1
                            If Not sc.ColumnList.Contains(sl.TableName & "." & sl.ColumnName) Then
                                sc.ColumnList.Add(sl.TableName & "." & sl.ColumnName)
                            End If
                        Next
                    Next
                End If
            Next
        End If

        ' Take a look at the list of Columns to see if we have any duplicates.  We use a
        ' set of arraylists to detect (and avoid) duplicates.
        Dim dupTabCol As New ArrayList
        Dim dupAlias As New ArrayList
        Dim dupCol As New ArrayList
        For Each sl In sc.SelectList
            ' find duplicate table/column names in TempTable
            If sl.ColumnName <> "" Then
                If dupTabCol.Contains(sl.TableName & "." & sl.ColumnName) Then
                    ' OK, we've got a duplicate.  Now we need convert this column into
                    ' an expression
                    i = 1
                    Do While True
                        If ColumnAlias.Contains(sl.ColumnName.ToLower & "_" & i) Then
                            i += 1
                        Else
                            ColumnAlias.Add(sl.ColumnName.ToLower & "_" & i)
                            sl.ColumnAlias = sl.ColumnName & "_" & i
                            sl.Expression = TabColName(sl.TableName, sl.ColumnName)
                            sl.DataType = ds.Tables(sl.TableName).Columns(sl.ColumnName).DataType.ToString
                            sl.TableName = ""
                            sl.ColumnName = ""
                            Exit Do
                        End If
                    Loop
                Else
                    dupTabCol.Add(sl.TableName & "." & sl.ColumnName)
                End If
            End If

            ' find duplicate aliases (both TempTable and ResultSet)
            If sl.ColumnAlias <> "" Then
                If dupAlias.Contains(sl.ColumnAlias) Then
                    ' Found one... We need to alter the alias
                    i = 1
                    Do While True
                        If ColumnAlias.Contains(sl.ColumnAlias.ToLower & "_" & i) Then
                            i += 1
                        Else
                            ColumnAlias.Add(sl.ColumnAlias.ToLower & "_" & i)
                            sl.ColumnAlias = sl.ColumnAlias & "_" & i
                            Exit Do
                        End If
                    Loop
                Else
                    dupAlias.Add(sl.ColumnAlias)
                End If
            End If

            ' find duplicates column names in the ResultSet
            If sl.ColumnName <> "" And sl.ColumnAlias = "" Then
                If dupCol.Contains(sl.ColumnName) Then
                    ' Found one... We need to add an alias
                    i = 1
                    Do While True
                        If ColumnAlias.Contains(sl.ColumnName.ToLower & "_" & i) Then
                            i += 1
                        Else
                            ColumnAlias.Add(sl.ColumnName.ToLower & "_" & i)
                            sl.ColumnAlias = sl.ColumnName & "_" & i
                            Exit Do
                        End If
                    Loop
                Else
                    dupCol.Add(sl.ColumnName)
                End If
            End If
        Next

        sc.TableList.TrimToSize()
        sc.ColumnList.TrimToSize()

        ' If GROUP BY is used, every item in the select list must be covered by a 
        ' corresponding item in the GROUP BY list or be a vector aggregate.
        If sc.GroupBy.Count > 0 Then
            For Each sl In sc.SelectList
                found = False
                For Each gb In sc.GroupBy
                    If (sl.TableName = gb.TableName And sl.ColumnName = gb.ColumnName) Or sl.Expression <> "" Then
                        found = True
                        Exit For
                    End If
                Next
                If Not found Then
                    Throw New ApplicationException("SQL syntax error: Items in SELECT list not covered by GROUP BY")
                End If
            Next
        Else
            ' A vector aggregate is a type of implied GROUP BY
            i = 0
            For Each sl In sc.SelectList
                If sl.IsAggregate Then
                    i += 1
                End If
            Next
            If i > 0 AndAlso sc.SelectList.Count > i Then
                Throw New ApplicationException("SQL syntax error: Items in SELECT list not covered by aggregate function")
            End If
        End If

        ' A quick SQL syntax check.  Expressions in the GROUP BY clause must match the
        ' expression in the SELECT clause
        For i = 0 To sc.GroupBy.Count - 1
            gb = sc.GroupBy(i)
            If gb.Expression <> "" Then
                ' go find the same expression in Select
                found = False
                For Each sl In sc.SelectList
                    If [String].Compare(sl.Expression, gb.Expression, True) = 0 Then
                        sc.GroupBy(i).ColumnName = sl.ColumnAlias
                        found = True
                        Exit For
                    End If
                Next
                If Not found Then
                    Throw New ApplicationException("SQL syntax error: No matching GROUP BY expression in SELECT")
                End If
            End If
        Next

        ' Translate a few things in the WHERE clause
        For i = 0 To sc.Where.Count - 1
            wh = sc.Where(i)
            ' Let's translate the unsupported "!=", "!<", and "!>" comparison operators
            Select Case wh.[Operator]
                Case "!="
                    sc.Where(i).[Operator] = "<>"
                Case "!<"
                    sc.Where(i).[Operator] = ">="
                Case "!>"
                    sc.Where(i).[Operator] = "<="
            End Select
        Next

        ' Next we translate the BETWEEN operator which is not supported by the native
        ' ADO.Net Select() method
        For Each wh In sc.Where
            If wh.[Operator] = "BETWEEN" Then
                TranslateBetween(sc.Where)
                Exit For
            End If
        Next

        ' process the join clauses...
        ProcessJoins(sc.From)
    End Sub

    ' Check the TableNames to make sure they match what's in the DataSet.  We tweak the
    ' name if necessary.  The name is passed by reference so this alters the source
    ' data directly.
    Private Sub ResolveTable(ByRef TableName As String)
        Dim buf As String
        Dim i As Integer
        Dim dt As DataTable

        ' I don't like double quotes... I prefer square brackets
        TableName = TableName.Trim(Chr(34))

        For i = 0 To ds.Tables.Count - 1
            dt = ds.Tables(i)
            If [String].Compare(dt.TableName, TableName, True) = 0 Then
                TableName = dt.TableName
                Exit Sub
            End If
        Next

        ' hummm... I wonder if we can find the name without the brackets. 
        If TableName.StartsWith("[") Then
            buf = TableName.TrimEnd("]").TrimStart("[")
            For i = 0 To ds.Tables.Count - 1
                dt = ds.Tables(i)
                If [String].Compare(dt.TableName, buf, True) = 0 Then
                    TableName = dt.TableName
                    Exit Sub
                End If
            Next
        End If

        ' OK, let's try it again *with* brackets
        If TableName.StartsWith("[") Then
            buf = "[" & TableName & "]"
            For i = 0 To ds.Tables.Count - 1
                dt = ds.Tables(i)
                If [String].Compare(dt.TableName, buf, True) = 0 Then
                    TableName = dt.TableName
                    Exit Sub
                End If
            Next
        End If

        Throw New ApplicationException("Input error: Table '" & TableName & "' not found")
    End Sub

    ' Fill in any missing TableName entries, verify and tweak the ColumnName
    Private Sub ResolveColumn(ByRef TableName As String, ByRef ColumnName As String, ByRef TempColumn As String)
        Dim Table, buf As String
        Dim i As Integer
        Dim GotIt As Boolean
        Dim dc As DataColumn

        ' I don't like double quotes... I prefer square brackets
        ColumnName = ColumnName.Trim(Chr(34))

        If TableName = "" Then
            ' A special case for the "*" column name
            If ColumnName = "*" Then
                If sc.TableList.Count = 1 Then
                    TableName = sc.TableList(0)
                End If
                HasAsterick = True
                Exit Sub
            End If

            ' OK, we've got to go poke around and find the appropriate table, then try
            ' to resolve the ColumnName
            GotIt = False
            For Each Table In sc.TableList
                For i = 0 To ds.Tables(Table).Columns.Count - 1
                    dc = ds.Tables(Table).Columns(i)
                    If [String].Compare(dc.ColumnName, ColumnName, True) = 0 Then
                        If GotIt Then
                            Throw New ApplicationException("Input error: Ambiguous column name '" & ColumnName & "'")
                        End If
                        TableName = Table
                        ColumnName = dc.ColumnName
                        TempColumn = TabColName(TableName, ColumnName)
                        GotIt = True
                    End If
                Next

                If ColumnName.StartsWith("[") Then
                    buf = ColumnName.TrimStart("[").TrimEnd("]")
                    For i = 0 To ds.Tables(Table).Columns.Count - 1
                        dc = ds.Tables(Table).Columns(i)
                        If [String].Compare(dc.ColumnName, buf, True) = 0 Then
                            If GotIt Then
                                Throw New ApplicationException("Input error: Ambiguous column name '" & ColumnName & "'")
                            End If
                            TableName = Table
                            ColumnName = dc.ColumnName
                            TempColumn = TabColName(TableName, ColumnName)
                            GotIt = True
                        End If
                    Next
                End If

                If Not ColumnName.StartsWith("[") Then
                    buf = "[" & ColumnName & "]"
                    For i = 0 To ds.Tables(Table).Columns.Count - 1
                        dc = ds.Tables(Table).Columns(i)
                        If [String].Compare(dc.ColumnName, buf, True) = 0 Then
                            If GotIt Then
                                Throw New ApplicationException("Input error: Ambiguous column name '" & ColumnName & "'")
                            End If
                            TableName = Table
                            ColumnName = dc.ColumnName
                            TempColumn = TabColName(TableName, ColumnName)
                            GotIt = True
                        End If
                    Next
                End If
            Next
            If Not GotIt Then
                Throw New ApplicationException("Input error: Invalid column name '" & ColumnName & "'")
            End If
            Exit Sub
        End If

        ' You provided the table name, so just resolve the column name
        If ColumnName = "*" Then
            HasAsterick = True
            Exit Sub
        End If

        For i = 0 To ds.Tables(TableName).Columns.Count - 1
            dc = ds.Tables(TableName).Columns(i)
            If [String].Compare(dc.ColumnName, ColumnName, True) = 0 Then
                ColumnName = dc.ColumnName
                TempColumn = TabColName(TableName, ColumnName)
                Exit Sub
            End If
        Next

        If ColumnName.StartsWith("[") Then
            buf = ColumnName.TrimStart("[").TrimEnd("]")
            For i = 0 To ds.Tables(TableName).Columns.Count - 1
                dc = ds.Tables(TableName).Columns(i)
                If [String].Compare(dc.ColumnName, buf, True) = 0 Then
                    ColumnName = dc.ColumnName
                    TempColumn = TabColName(TableName, ColumnName)
                    Exit Sub
                End If
            Next
        End If

        If Not ColumnName.StartsWith("[") Then
            buf = "[" & ColumnName & "]"
            For i = 0 To ds.Tables(TableName).Columns.Count - 1
                dc = ds.Tables(TableName).Columns(i)
                If [String].Compare(dc.ColumnName, buf, True) Then
                    ColumnName = dc.ColumnName
                    TempColumn = TabColName(TableName, ColumnName)
                    Exit Sub
                End If
            Next
        End If

        Throw New ApplicationException("Input error: Invalid column name '" & ColumnName & "'")
    End Sub

    ' The native DataTable.Select() method doesn't support the BETWEEN operator, so we
    ' translate it into a series of equivalent operations.
    Private Sub TranslateBetween(ByRef scc As SearchConditionCollection)
        Dim i As Integer
        Dim sc, temp_sc As SearchCondition

        ' do a "search and destroy" operation on the BEWTEEN operator
        For i = 0 To scc.Count - 1
            sc = scc(i)
            If sc.[Operator] = "BETWEEN" Then
                ' whack this entry
                scc.RemoveAt(i)

                ' add a phrase for the 1st half
                temp_sc = New SearchCondition
                temp_sc.TableName = sc.TableName
                temp_sc.ColumnName = sc.ColumnName
                temp_sc.TempColumn = TabColName(sc.TableName, sc.ColumnName)
                temp_sc.Level = sc.Level + 1
                temp_sc.[Operator] = ">"
                temp_sc.NumArgs = 2
                temp_sc.Arg1 = sc.Arg1
                scc.Insert(i, temp_sc)

                ' add a phrase for the AND operator
                temp_sc = New SearchCondition
                temp_sc.[Operator] = "AND"
                temp_sc.NumArgs = 0
                temp_sc.Level = sc.Level + 1
                scc.Insert(i + 1, temp_sc)

                ' add a phrase for the 2nd half
                temp_sc = New SearchCondition
                temp_sc.TableName = sc.TableName
                temp_sc.ColumnName = sc.ColumnName
                temp_sc.TempColumn = TabColName(sc.TableName, sc.ColumnName)
                temp_sc.Level = sc.Level + 1
                temp_sc.[Operator] = "<"
                temp_sc.NumArgs = 2
                temp_sc.Arg1 = sc.Arg2
                scc.Insert(i + 2, temp_sc)
            End If
        Next
    End Sub

    ' Process the Table names
    Private Sub ProcessTables(ByRef TableName As String)
        If TableName <> "" Then

            If TableAlias.Contains(TableName.ToLower) Then
                TableName = TableAlias(TableName.ToLower)
            End If

            ResolveTable(TableName)
            If Not sc.TableList.Contains(TableName) Then
                sc.TableList.Add(TableName)
            End If
        End If
    End Sub

    ' Process the Column names
    Private Sub ProcessColumns(ByRef TableName As String, ByRef ColumnName As String, ByRef TempColumn As String, ByVal SupportsAlias As Boolean)
        Dim sl As SelectList

        If ColumnName <> "" Then

            If SupportsAlias Then
                ' Let's see if this is a known column alias
                For Each sl In sc.SelectList
                    If [String].Compare(sl.ColumnAlias, ColumnName, True) = 0 Then
                        ' If the alias does not refer to an Expression, then resolve the
                        ' real table and column names
                        If sl.Expression = "" Then
                            TableName = sl.TableName
                            ColumnName = sl.ColumnName
                        End If
                        Exit Sub
                    End If
                Next
            End If

            ResolveColumn(TableName, ColumnName, TempColumn)
            If Not sc.ColumnList.Contains(TableName & "." & ColumnName) And ColumnName <> "*" Then
                sc.ColumnList.Add(TableName & "." & ColumnName)
            End If
        End If
    End Sub

    ' Process the Expression string.
    ' 1) Figure out the return type
    ' 2) Resolve table and column names
    ' 3) Do some translations
    Private Sub ProcessExpression(ByRef Expression As String, ByRef DataType As String)
        Dim words As ArrayList
        Dim buf, TableName, ColumnName, TempColumn As String
        Dim i, j, level As Integer

        If Expression <> "" Then
            ' Count(*) isn't supported by native ADO.Net, so we create an autonumber column
            ' in the TempTable and swap out the * for the new column name.
            If Expression.IndexOf("COUNT( * )") >= 0 Then
                Expression = Expression.Replace("COUNT( * )", "COUNT( __COUNT__ )")
                If Not sc.ColumnList.Contains("__COUNT__") Then
                    sc.ColumnList.Add("__COUNT__")
                End If
            End If
            words = Parser.SplitWords(Expression)

            For i = 0 To words.Count - 1
                buf = words(i)
                ' get the return type for functions
                Select Case buf.ToUpper
                    Case "COUNT(", "LEN("
                        If DataType = "" Then
                            DataType = "System.Int32"
                        End If
                    Case "AVG(", "STDEV(", "VAR("
                        If DataType = "" Then
                            DataType = "System.Double"
                        End If
                    Case "SUBSTRING(", "TRIM("
                        If DataType = "" Then
                            DataType = "System.String"
                        End If
                    Case "CONVERT("
                        ' TODO: What about nested CONVERTs?  Should we allow it?
                        ' What is the return type
                        If i < words.Count - 1 Then
                            Select Case words(i + 1).toupper
                                Case "BIGINT"
                                    DataType = "System.Int64"
                                Case "INT"
                                    DataType = "System.Int32"
                                Case "SMALLINT"
                                    DataType = "System.Int16"
                                Case "TINYINT"
                                    DataType = "System.Byte"
                                Case "BIT"
                                    DataType = "System.Boolean"
                                Case "DECIMAL", "NUMERIC", "REAL"
                                    DataType = "System.Single"
                                Case "FLOAT"
                                    DataType = "System.Double"
                                Case "MONEY", "SMALLMONEY"
                                    DataType = "System.Decimal"
                                Case "DATETIME", "SMALLDATETIME"
                                    DataType = "System.DateTime"
                                Case "CHAR", "VARCHAR", "NCHAR", "NVARCHAR"
                                    DataType = "System.Char"
                                Case "CHAR(", "VARCHAR(", "TEXT(", "NCHAR(", "NVARCHAR(", "NTEXT("
                                    DataType = "System.String"
                                    ' TODO: Find the closing parens instead guessing
                                    words.RemoveRange(i + 3, 2) ' size and closing paren
                                Case Else
                                    Throw New ApplicationException("SQL implementation limit: DataType '" & words(i).trimend("(") & "' is not supported")
                            End Select
                            words.RemoveRange(i + 1, 2) ' datatype and comma
                        End If
                        ' we need to find the closing parens
                        level = 1
                        For j = i + 1 To words.Count - 1
                            Select Case words(j)
                                Case "("
                                    level += 1
                                Case ")"
                                    level -= 1
                                    If level = 0 Then
                                        words.Insert(j, ",")
                                        words.Insert(j + 1, "'" & DataType & "'")
                                        Exit For
                                    End If
                                Case Else
                                    If words(j).endswith("(") Then
                                        level += 1
                                    End If
                            End Select
                        Next
                    Case "!="
                        ' Do a little translations
                        words(i) = "<>"
                    Case "!<"
                        words(i) = ">="
                    Case "!>"
                        words(i) = "<="
                        ' TODO: Translate the BETWEEN operators inside expressions
                    Case "IS", "IN", "LIKE", "NULL", "BETWEEN", "AND", "OR", "NOT"
                        ' just a list of keywords you're likely to come across in an
                        ' expression.
                    Case "SYSTEM.INT64", "SYSTEM.INT32", "SYSTEM.INT16", "SYSTEM.BYTE", "SYSTEM.BOOLEAN", "SYSTEM.SINGLE", "SYSTEM.DECIMAL", "SYSTEM.DOUBLE", "SYSTEM.DATETIME", "SYSTEM.CHAR", "SYSTEM.STRING"
                        ' list of datatypes (created by the CONVERT translation) to
                        ' ignore
                    Case Else
                        ' Process Column Names.  Valid column names either start with a
                        ' bracket or start with a letter but do not end with "("
                        If buf.StartsWith("[") Or buf.StartsWith(Chr(34)) Or ([Char].IsLetter(buf.Chars(0)) And Not buf.EndsWith("(")) Then

                            ' fix the table aliases, resolve the names
                            TableName = Parser.ParseNextToLastDot(buf)
                            If TableName <> "" Then
                                If TableAlias.Contains(TableName) Then
                                    TableName = TableAlias(TableName)
                                End If
                                ResolveTable(TableName)
                                If Not sc.TableList.Contains(TableName) Then
                                    sc.TableList.Add(TableName)
                                End If
                            End If

                            ' resolve the column
                            ColumnName = Parser.ParseLastDot(buf)
                            ResolveColumn(TableName, ColumnName, TempColumn)
                            If Not sc.ColumnList.Contains(TableName & "." & ColumnName) Then
                                sc.ColumnList.Add(TableName & "." & ColumnName)
                            End If

                            ' We need the return type for the expression.  This is
                            ' often tied to the datatype of one of the arguments.
                            ' So we look at the first column name that shows up.
                            If DataType = "" Then
                                DataType = ds.Tables(TableName).Columns(ColumnName).DataType.ToString
                            End If

                            ' Since we're using the native ADO.Net Expressions, we
                            ' have to strip the "." notation and use our own.
                            If buf <> TempColumn Then
                                words(i) = TempColumn
                            End If
                        End If
                End Select

            Next
            ' Some expressions don't have column names at all (it could be just a
            ' string, date, or numeric constant).  However, we still need to know the
            ' data type.
            If DataType = "" Then
                Dim c As Char
                c = words(0).Chars(0)
                Select Case c
                    Case "'"
                        DataType = "System.String"
                    Case "#"
                        DataType = "System.Date"
                    Case Else
                        DataType = "System.Double"
                End Select
            End If

            ' rebuild the expression string
            Expression = ""
            For i = 0 To words.Count - 1
                Expression &= words(i)
                If i < words.Count - 1 Then
                    Expression &= " "
                End If
            Next

        End If
    End Sub

    ' The SQL syntax allows for no particular order in the columns of the join
    ' predicate.  I don't particularly like that... so we sort them out here.
    Private Sub ProcessJoins(ByRef tsc As TableSourceCollection)
        Dim i As Integer
        Dim TempTable, TempColumn As String

        ' do we have any joins at all?
        If tsc.Count = 0 And tsc(0).JoinType = "" Then
            Exit Sub
        End If

        ' TODO: Verify that the predicates actually refer to tables/columns that
        ' already exist in the result set of any previous joins.

        For i = 0 To tsc.Count - 1
            ' swap 'em around so left is left... and right is right
            If tsc(i).LeftTable = tsc(i).RightPredicateTable Then
                TempTable = tsc(i).LeftPredicateTable
                TempColumn = tsc(i).LeftPredicateColumn
                tsc(i).LeftPredicateTable = tsc(i).RightPredicateTable
                tsc(i).LeftPredicateColumn = tsc(i).RightPredicateColumn
                tsc(i).RightPredicateTable = TempTable
                tsc(i).RightPredicateColumn = TempColumn
            End If
            If tsc(i).RightTable = tsc(i).LeftPredicateTable Then
                TempTable = tsc(i).RightPredicateTable
                TempColumn = tsc(i).RightPredicateColumn
                tsc(i).RightPredicateTable = tsc(i).LeftPredicateTable
                tsc(i).RightPredicateColumn = tsc(i).LeftPredicateColumn
                tsc(i).LeftPredicateTable = TempTable
                tsc(i).LeftPredicateColumn = TempColumn
            End If
        Next

    End Sub

    Friend Shared Function TabColName(ByVal TableName As String, ByVal ColumnName As String) As String
        ' Expressions don't allow for brackets "[]" or dots "." in column names, so we
        ' have to be somewhat creative.

        If IsNothing(TableName) Or IsNothing(ColumnName) Then
            Return ""
        End If

        ' TODO: I can envision a possible name collision here.
        Return (TableName.Replace(" ", "_").Trim("[]".ToCharArray) & "_" & ColumnName.Replace(" ", "_").Trim("[]".ToCharArray))
    End Function
End Class

