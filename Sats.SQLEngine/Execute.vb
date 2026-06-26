' This class will build an "execution plan" for the select statement.   Well, OK... 
' that's a bit of an exaggeration.  The execution plan is NOT dynamic. Unlike execution
' plans in a *real* database, the same technique is always used.

Public Class Executer

    Private ds As DataSet
    Private sc As SelectCommand

    Public Sub New(ByVal ds1 As DataSet, ByVal sc1 As SelectCommand)
        ds = ds1
        sc = sc1
    End Sub

    ' Execution Strategy for the SELECT command
    Public Function Execute() As DataTable
        Dim dt_Temp, dt_ResultSet As DataTable

        ' Build a table to store intermediate results, perform the JOIN operations,
        ' do the initial fill the TempTable
        dt_Temp = Joins()

        ' perform the WHERE operations on the TempTable
        dt_Temp = FilterTable(dt_Temp)

        ' perform the GROUP BY/HAVING operations
        dt_Temp = Grouping(dt_Temp)

        ' perform the ORDER BY operations
        dt_Temp = Sort(dt_Temp)

        ' perform any TOP operations, and copy the data to the ResultSet
        dt_ResultSet = Limit(dt_Temp)

        Return dt_ResultSet
    End Function

    ' Apply the JOIN operations and do the initial fill of the TempTable
    Private Function Joins() As DataTable
        Dim dt_temp, Left_dt, Right_dt, dt_nested As DataTable
        Dim ts As TableSource
        Dim dr, dr_src As DataRow
        Dim TableName, ColumnName, Left_col, Right_col As String
        Dim ColumnList As ArrayList

        ' We have to process each join in the order in which it appears.  A separate
        ' DataTable is returned for nested join.  This is a bit tedious and slow but it
        ' is easier to follow than other potentially more efficient techniques.
        ' (Sometime simple is better, just because of that!)

        ' Do we have any joins at all?   If not, then just fill the TempTable and quit
        If sc.From.Count = 1 And sc.From(0).JoinType = "" Then
            TableName = sc.From(0).LeftTable
            dt_temp = BuildTempTable()
            ColumnList = GetTableColumns(TableName)
            For Each dr_src In ds.Tables(TableName).Rows
                dr = dt_temp.NewRow
                ' copy the columns into the temporary table
                For Each ColumnName In ColumnList
                    dr(Resolver.TabColName(TableName, ColumnName)) = dr_src(ColumnName)
                Next
                dt_temp.Rows.Add(dr)
            Next
            Return dt_temp
        End If

        '   LeftTable
        '   JOIN operator
        '       LeftTable        \
        '       JOIN operator     \  the product of this nested join becomes the new
        '       RightTable        /  RightTable for the first join
        '       ON condition     /
        '   ON condition

        ' The success of this section is highly dependent upon the proper order of the
        ' joins produced by the parser class.   For example, the first join must be 
        ' between two "real" tables in the DataSet.
        For Each ts In sc.From

            ' A "missing" left or right table is an indication of where the results of
            ' the previous join goes
            If ts.LeftTable = "" And ts.RightTable <> "" Then
                Left_dt = dt_temp
                Left_col = Resolver.TabColName(ts.LeftPredicateTable, ts.LeftPredicateColumn)
            Else
                Left_dt = ds.Tables(ts.LeftTable)
                Left_col = ts.LeftPredicateColumn
            End If
            If ts.RightTable = "" And ts.LeftTable <> "" Then
                Right_dt = dt_temp
                Right_col = Resolver.TabColName(ts.RightPredicateTable, ts.RightPredicateColumn)
            Else
                Right_dt = ds.Tables(ts.RightTable)
                Right_col = ts.RightPredicateColumn
            End If

            ' If both the left and the right are "missing", then use both the last and
            ' the previous nested table
            If ts.LeftTable = "" And ts.RightTable = "" Then
                Left_dt = dt_nested
                Left_col = Resolver.TabColName(ts.LeftPredicateTable, ts.LeftPredicateColumn)
                Right_dt = dt_temp
                Right_col = Resolver.TabColName(ts.RightPredicateTable, ts.RightPredicateColumn)
            End If
            If Not IsNothing(dt_temp) Then
                dt_nested = dt_temp.Copy
            End If
            dt_temp = JoinTables(Left_dt, Right_dt, Left_col, Right_col, ts.JoinType)
        Next

        If Not IsNothing(dt_nested) Then
            dt_nested.Dispose()
        End If
        Return dt_temp
    End Function

    ' Apply the WHERE Clause
    Private Function FilterTable(ByRef dt As DataTable) As DataTable
        Dim dt_temp As DataTable
        Dim dr_src, dr_dest As DataRow
        Dim WhereClause As String

        ' anything to do?
        If sc.Where.Count = 0 Then
            Return dt
        End If

        ' Make a Where Clause with Temp table column names
        WhereClause = MakeSearchCondition()

        ' copy the schema
        dt_temp = dt.Clone

        ' now apply the where condition
        For Each dr_src In dt.Select(WhereClause)
            dr_dest = dt_temp.NewRow
            dr_dest.ItemArray = dr_src.ItemArray
            dt_temp.Rows.Add(dr_dest)
            ' You might be tempted to use ImportRow here to speed things up a wee bit.
            ' However, the ImportRow method will not cause Expressions to re-evaluate,
            ' so that won't work here.
            'dt_temp.ImportRow(dr_src)
        Next

        Return dt_temp
    End Function

    ' Perform the GROUP BY and HAVING operations. 
    Private Function Grouping(ByRef dt As DataTable) As DataTable
        Dim dt_temp As DataTable
        Dim dr_src, dr_dest As DataRow
        Dim sl As SelectList
        Dim gb As GroupByExpression
        Dim GroupBySort, GroupByFilter, TempColumns(0), ColDataTypes(0) As String
        Dim Temp, TempVals(0) As Object
        Dim i, gb_cols, aggregates, count As Integer
        Dim sb As New System.Text.StringBuilder
        Dim Skip, Distinct As Boolean

        ' There is a lot going on here.  We have to test rows of data to see if they are
        ' unique.  If they are, we need to apply the aggregate operators using the
        ' native ADO.Net Compute method.

        ' A quick short cut
        If dt.Rows.Count = 0 Then
            Return dt
        End If

        ' How many Aggregate columns do we have?
        For Each sl In sc.SelectList
            If sl.IsAggregate Then
                aggregates += 1
            End If
        Next

        ' let's see if DISTINCT is being used.
        Distinct = False
        For i = 0 To UBound(sc.SelectArgs)
            If sc.SelectArgs(i) = "DISTINCT" Then
                ' if you mistakenly added DISTINCT when you already have aggregates, then
                ' just silently ignore the DISTINCT key word.
                If aggregates = 0 Then
                    Distinct = True
                End If
                Exit For
            End If
        Next

        ' We treat the DISTINCT keyword as a "super group by"...  So, we create a 
        ' GroupBy_Expression that contains all of the columns in the ResultSet
        If Distinct Then
            For i = 0 To sc.SelectList.Count - 1
                gb = New GroupByExpression
                If sc.SelectList(i).ColumnName = "" Then
                    gb.TempColumn = sc.SelectList(i).ColumnAlias
                    gb.DataType = sc.SelectList(i).DataType
                Else
                    gb.TableName = sc.SelectList(i).TableName
                    gb.ColumnName = sc.SelectList(i).ColumnName
                    gb.TempColumn = Resolver.TabColName(sc.SelectList(i).TableName, sc.SelectList(i).ColumnName)
                End If
                sc.GroupBy.Add(gb)
            Next
        End If

        ' Is there anything for us to do?
        If sc.GroupBy.Count = 0 And aggregates = 0 Then
            Return dt
        End If

        ' copy the schema
        dt_temp = dt.Clone

        ' At this point, the expressions in the TempTable must become normal values, so
        ' we alter the columns.expression property.  BTW: You can only do this when there
        ' are no rows present.
        For i = 0 To dt_temp.Columns.Count - 1
            dt_temp.Columns(i).Expression = ""
        Next

        ' An aggregate function without a GroupBy_Expression means there is an implied
        ' grouping.  So, just copy the first row and quit.
        If aggregates > 0 And sc.GroupBy.Count = 0 Then
            dr_dest = dt_temp.NewRow
            dr_dest.ItemArray = dt.Rows(0).ItemArray
            dt_temp.Rows.Add(dr_dest)
            Return dt_temp
        End If

        ' Do a temporary sort on the columns being grouped
        For Each gb In sc.GroupBy
            sb.Append(gb.TempColumn)
            sb.Append(",")
        Next
        GroupBySort = sb.ToString.TrimEnd(",")
        dt = Sort(dt, GroupBySort)

        ' Setup some temporary place holders for the grouped column values
        gb_cols = sc.GroupBy.Count - 1
        ReDim TempVals(gb_cols)
        ReDim TempColumns(gb_cols)
        ReDim ColDataTypes(gb_cols)
        For i = 0 To gb_cols
            If sc.GroupBy(i).Expression <> "" Then
                TempColumns(i) = sc.GroupBy(i).ColumnName
                ColDataTypes(i) = sc.GroupBy(i).DataType
            Else
                TempColumns(i) = sc.GroupBy(i).TempColumn
                ColDataTypes(i) = ds.Tables(sc.GroupBy(i).TableName).Columns(sc.GroupBy(i).ColumnName).DataType.ToString
            End If
        Next

        ' let's go...
        For Each dr_src In dt.Rows
            count = 0
            ' compare the values of the temporary place holders to the grouping columns
            For i = 0 To gb_cols
                Temp = dr_src(TempColumns(i))

                ' VB.Net can't really deal with comparing DBNull's
                If IsDBNull(Temp) Then
                    Temp = Nothing
                End If

                If Temp = TempVals(i) Then
                    count += 1
                End If
                TempVals(i) = Temp
            Next

            ' Is this row unique?  
            If count <> gb_cols + 1 Then
                ' let's start with just a copy of the unique row.  If there are aggregate
                ' functions, we overwrite those values below
                dr_dest = dt_temp.NewRow
                dr_dest.ItemArray = dr_src.ItemArray

                Skip = False
                If aggregates > 0 Then
                    ' Build a filter to use with the Compute() method below

                    sb.Length = 0
                    For i = 0 To gb_cols
                        Select Case ColDataTypes(i)
                            Case "System.String"
                                TempVals(i) = TempVals(i).ToString.Replace("'", "''")
                                sb.Append(TempColumns(i) & "='" & TempVals(i) & "'")
                            Case "System.DateTime"
                                ' TODO: Translate date formats?
                                sb.Append(TempColumns(i) & "=#" & TempVals(i) & "#")
                            Case Else
                                sb.Append(TempColumns(i) & "=" & TempVals(i))
                        End Select

                        If i < gb_cols Then
                            sb.Append(" AND ")
                        End If
                    Next
                    GroupByFilter = sb.ToString

                    ' Apply the HAVING filter here...
                    If sc.Having <> "" AndAlso CBool(dt.Compute(sc.Having, GroupByFilter)) = False Then
                        Skip = True
                    Else
                        ' Compute each of the aggregate values, plug the values into the
                        ' data row we're building
                        For Each sl In sc.SelectList
                            If sl.IsAggregate Then
                                dr_dest(sl.ColumnAlias) = dt.Compute(sl.Expression, GroupByFilter)
                            End If
                        Next
                    End If
                End If

                ' has this row been filtered out by a HAVING clause?
                If Not Skip Then
                    dt_temp.Rows.Add(dr_dest)
                End If
            End If
        Next
        Return dt_temp

    End Function

    ' Sort the DataTable
    Private Function Sort(ByRef dt As DataTable, Optional ByVal colnames As String = "") As DataTable
        Dim dt_temp As DataTable
        Dim dr As DataRow

        If dt.Rows.Count > 1 Then
            ' Did we specify a sort condition or use the one in the OrderBy_Expression?
            If colnames = "" Then
                colnames = MakeOrderBy(dt)
                ' Is there anything to do?
                If colnames = "" Then
                    Return dt
                End If
            End If

            ' copy the schema
            dt_temp = dt.Clone

            ' sort and import the data back
            For Each dr In dt.Select(Nothing, colnames)
                dt_temp.ImportRow(dr)
            Next
            Return dt_temp
        End If

        Return dt
    End Function

    ' Limit the number of rows in the result set (and also copy the data from the 
    ' TempTable to the ResultSet)
    Private Function Limit(ByVal dt As DataTable) As DataTable
        Dim dt_ResultSet As DataTable
        Dim dr As DataRow
        Dim row_limit, row, col As Integer
        Dim sa As String

        row_limit = dt.Rows.Count
        ' anything to do?
        If row_limit = 0 Then
            Return BuildResultSet()
        End If

        For Each sa In sc.SelectArgs
            Select Case sa
                Case "ALL", "TOP"
                    ' ignore these
                Case "DISTINCT"
                    ' this is handled by Grouping()
                Case "PERCENT"
                    row_limit = (dt.Rows.Count * row_limit) / 100.0
                Case Else
                    row_limit = CInt(sa)
            End Select
        Next

        ' some quick sanity checking
        If row_limit < 0 Then
            Throw New ApplicationException("SQL parameter error: Row Limit must be > 0")
        End If
        If row_limit > dt.Rows.Count Then
            row_limit = dt.Rows.Count
        End If

        ' copy the rows that we want to keep
        dt_ResultSet = BuildResultSet()
        For row = 0 To row_limit - 1
            ' Copy only the columns in the result set.  Remember, we built the TempTable
            ' so that the order of the columns exactly match the ResultSet.  That way
            ' we don't have to worry about temporary column names vs. the real column
            ' names.
            dr = dt_ResultSet.NewRow
            For col = 0 To dt_ResultSet.Columns.Count - 1
                dr(col) = dt.Rows(row)(col)
            Next
            dt_ResultSet.Rows.Add(dr)
        Next

        dt_ResultSet.AcceptChanges()
        Return dt_ResultSet
    End Function

    ' Build a temporary "TempTable" that contains all of the columns that are required
    ' by the select_list, the expressions, and the aggregation functions.  No column 
    ' aliases are used at this point (except for expressions, since they don't have any
    ' other names)
    Private Function BuildTempTable() As DataTable
        Dim dt As New DataTable
        Dim c, c_in As DataColumn
        Dim sl As SelectList
        Dim wh As SearchCondition
        Dim column, TableName, ColumnName, temp As String

        ' NOTE: We rely on the order of columns in the TempTable to exactly match the
        ' order of the columns in the ResultSet.  So we must start with the Select List.
        For Each sl In sc.SelectList
            If sl.Expression <> "" Then
                ' we can't populate the expression property just yet
                c = New DataColumn(sl.ColumnAlias)
                c.DataType = System.Type.GetType(sl.DataType)
            Else
                c_in = ds.Tables(sl.TableName).Columns(sl.ColumnName)
                c = New DataColumn(Resolver.TabColName(sl.TableName, sl.ColumnName))
                c.DataType = c_in.DataType
                c.MaxLength = c_in.MaxLength
            End If
            dt.Columns.Add(c)
        Next

        ' Next we tack on the extra columns that are required by the expressions (that
        ' don't show up in the ResultSet)
        For Each column In sc.ColumnList
            ' Special case for this name...  it signals the need for an autonumber
            If column = "__COUNT__" Then
                c = New DataColumn(column)
                c.DataType = GetType(Long)
                c.AutoIncrement = True
                c.AutoIncrementStep = 1
                c.AutoIncrementSeed = 0
                dt.Columns.Add(c)
            Else
                TableName = Parser.ParseNextToLastDot(column)
                ColumnName = Parser.ParseLastDot(column)
                temp = Resolver.TabColName(TableName, ColumnName)

                ' is it already in there?
                If Not dt.Columns.Contains(temp) Then
                    c_in = ds.Tables(TableName).Columns(ColumnName)
                    c = New DataColumn(temp)
                    c.DataType = c_in.DataType
                    c.MaxLength = c_in.MaxLength
                    dt.Columns.Add(c)
                End If
            End If
        Next

        ' Last, we have to add the expressions to the DataTable. The order of this is
        ' critical... expressions must appear after the columns they refer to.
        For Each sl In sc.SelectList
            If sl.Expression <> "" Then
                ' We're already built this column above, so now just add the expression
                ' property to the column (remember, you can't do this out of order, and
                ' I want the order of the TempTable to match that of the ResultSet)
                c = dt.Columns(sl.ColumnAlias)
                c.Expression = sl.Expression
            End If
        Next
        For Each wh In sc.Where
            If wh.Expression <> "" Then
                c = New DataColumn(wh.ExpAlias)
                c.DataType = System.Type.GetType(wh.DataType)
                c.Expression = wh.Expression
                dt.Columns.Add(c)
            End If
        Next
        ' By design, expressions that appear in the GROUP BY have to also appear in the
        ' select list.  So, that means they have already been added.

        ' NOTE: we let the normal error handling occur for things like invalid or 
        ' duplicate column names, etc.
        Return dt
    End Function

    ' Build the result set table based upon the information gathered in the SelectList
    ' This is what the final results will look like.
    Private Function BuildResultSet() As DataTable
        Dim dt As New DataTable
        Dim c, c_in As DataColumn
        Dim sl As SelectList

        For Each sl In sc.SelectList
            If sl.ColumnName = "" Then
                ' expressions are just regular columns now
                c = New DataColumn(sl.ColumnAlias)
                c.DataType = System.Type.GetType(sl.DataType)
                dt.Columns.Add(c)
            Else
                c_in = ds.Tables(sl.TableName).Columns(sl.ColumnName)
                If sl.ColumnAlias = "" Then
                    c = New DataColumn(sl.ColumnName)
                Else
                    c = New DataColumn(sl.ColumnAlias)
                End If
                c.DataType = c_in.DataType
                c.MaxLength = c_in.MaxLength
                dt.Columns.Add(c)
            End If
        Next

        ' NOTE: this is a "Result Set", so there is no primary key or any other extra
        ' DataTable attributes.  It is not part of the DataSet's Table collection.
        Return dt
    End Function

    ' Reconstitute the search_condition (from the Where clause) using the column names
    ' of the TempTable.
    Private Function MakeSearchCondition() As String
        Dim sb As New System.Text.StringBuilder
        Dim WhereClause As String
        Dim wh As SearchCondition
        Dim CurrentLevel As Integer

        ' anything to do?
        If sc.Where.Count = 0 Then
            Return ("")
        End If

        For Each wh In sc.Where
            If wh.Level > CurrentLevel Then
                sb.Append(New String("("c, wh.Level - CurrentLevel) & " ")
            End If
            If wh.Level < CurrentLevel Then
                sb.Append(New String(")"c, CurrentLevel - wh.Level) & " ")
            End If
            If wh.Expression <> "" Then
                sb.Append(wh.Expression & " ")
            Else
                Select Case wh.NumArgs
                    Case 0
                        sb.Append(wh.[Operator] & " ")
                    Case 1
                        sb.Append(wh.TempColumn & " " & wh.[Operator] & " ")
                    Case 2
                        sb.Append(wh.TempColumn & " " & wh.[Operator] & " " & wh.Arg1 & " ")
                    Case 3
                        sb.Append(wh.TempColumn & " " & wh.[Operator] & " " & wh.Arg1 & " AND " & wh.Arg2 & " ")
                End Select
            End If
            CurrentLevel = wh.Level
        Next

        If CurrentLevel > 0 Then
            sb.Append(New String(")"c, CurrentLevel))
        End If
        WhereClause = sb.ToString.TrimEnd

        Return WhereClause
    End Function

    ' Reconstitute the order_expression using the column names of the TempTable
    Private Function MakeOrderBy(ByVal dt As DataTable) As String
        Dim sb As New System.Text.StringBuilder
        Dim OrderByClause As String
        Dim oe As OrderByExpression

        ' anything to do?
        If sc.OrderBy.Count = 0 Then
            Return ("")
        End If

        For Each oe In sc.OrderBy
            If oe.ColumnName = "" Then
                ' Native ADO.Net doesn't support a sort by a column index, so we have 
                ' to substitute the column name in place of the number
                If oe.ColumnIndex < 1 Or oe.ColumnIndex > sc.SelectList.Count Then
                    Throw New ApplicationException("SQL parameter error: ORDER BY value out of range")
                End If
                sb.Append(dt.Columns(oe.ColumnIndex - 1).ColumnName)
            Else
                ' This is one of the few places where column aliases *are* allowed
                If oe.TableName = "" Then
                    sb.Append(oe.ColumnName)
                Else
                    sb.Append(Resolver.TabColName(oe.TableName, oe.ColumnName))
                End If
            End If
            If oe.SortOrder = "DESC" Then
                sb.Append(" " & oe.SortOrder)
            End If
            sb.Append(",")
        Next
        OrderByClause = sb.ToString.TrimEnd(",")

        Return OrderByClause
    End Function

    ' Return a list of columns being used from this table
    Private Function GetTableColumns(ByVal Table As String) As ArrayList
        Dim ans As New ArrayList
        Dim column, ColumnName, TableName As String

        For Each column In sc.ColumnList
            TableName = Parser.ParseNextToLastDot(column)
            ColumnName = Parser.ParseLastDot(column)

            If TableName = Table Then
                ans.Add(ColumnName)
            End If
        Next

        Return ans
    End Function

    ' Join two tables together
    Private Function JoinTables(ByVal LeftTable As DataTable, ByVal RightTable As DataTable, ByVal LeftColumn As String, ByVal RightColumn As String, ByVal JoinType As String) As DataTable
        Dim dr_left, dr_right, dr_dest, dr_temp, drs() As DataRow
        Dim dt, left_dt, right_dt As DataTable
        Dim right_col, left_col, column As String
        Dim RightColumnList, LeftColumnList As ArrayList
        Dim i As Integer

        ' Make sure both tables are in the DataSet.  You can't use Relationships unless
        ' both tables are in the same DataSet.  BTW: If we put 'em in here, we take 'em
        ' out at the end.
        If RightTable.TableName = "" Then
            RightTable.TableName = "__RIGHTTABLE__"
            ds.Tables.Add(RightTable)
        End If
        If LeftTable.TableName = "" Then
            LeftTable.TableName = "__LEFTTABLE__"
            ds.Tables.Add(LeftTable)
        End If

        ' build the detached TempTable that will be returned
        dt = BuildTempTable()
        left_dt = LeftTable
        right_dt = RightTable
        left_col = LeftColumn
        right_col = RightColumn

        ' what kind of join is this?
        Select Case JoinType
            Case "INNER"
            Case "LEFT"
            Case "RIGHT"
                ' We don't really do right joins... we just flip the arguments and
                ' do a left join instead.
                left_dt = RightTable
                right_dt = LeftTable
                left_col = RightColumn
                right_col = LeftColumn
                JoinType = "LEFT"
            Case "FULL"
                ' A full join is kinda complicated... we do a right join, then a left,
                ' then combine the two together.  The optimum execution plan for a full 
                ' join should be a "merge" operation... but let's not make things any
                ' more complicated than they already are.
                Dim temp As DataTable
                temp = JoinTables(RightTable, LeftTable, RightColumn, LeftColumn, "LEFT")
                dt = JoinTables(LeftTable, RightTable, LeftColumn, RightColumn, "LEFT")

                drs = temp.Select(Resolver.TabColName(LeftTable.TableName, LeftColumn) & " is null")
                For Each dr_right In drs
                    dt.ImportRow(dr_right)
                Next
                Return dt
            Case Else
                Throw New ApplicationException("SQL syntax error: Unknown Join type '" & JoinType & "'")
        End Select

        ' create a relationship between the two tables
        ds.Relations.Add(New DataRelation("__RELATIONSHIP__", right_dt.Columns(right_col), left_dt.Columns(left_col), False))

        ' let's go!
        dr_temp = dt.NewRow
        LeftColumnList = GetTableColumns(left_dt.TableName)
        RightColumnList = GetTableColumns(right_dt.TableName)
        For Each dr_left In left_dt.Rows

            ' Get the related rows from the "right" table
            drs = dr_left.GetParentRows("__RELATIONSHIP__")

            ' For inner joins, we don't record anything unless there is a matching row
            If UBound(drs) >= 0 Or JoinType <> "INNER" Then
                dr_dest = dt.NewRow

                ' Let's start by just copying the columns from the "left" table
                If left_dt.TableName = "__LEFTTABLE__" Then
                    dr_dest.ItemArray = dr_left.ItemArray
                Else
                    For Each column In LeftColumnList
                        dr_dest(Resolver.TabColName(left_dt.TableName, column)) = dr_left(column)
                    Next
                End If

                ' There are three possibilities... there are no matching rows, there is
                ' only one related row, there are many related rows.
                Select Case UBound(drs)
                    Case -1
                        ' Just record the row as it is now (with just the columns from
                        ' the left table).
                        dt.Rows.Add(dr_dest)
                    Case 0
                        dr_right = drs(0)
                        If right_dt.TableName = "__RIGHTTABLE__" Then
                            For i = 0 To right_dt.Columns.Count - 1
                                ' fill in the holes, but do not overwrite the data that
                                ' came from the left table
                                If IsDBNull(dr_dest(i)) Then
                                    dr_dest(i) = dr_right(i)
                                End If
                            Next
                        Else
                            For Each column In RightColumnList
                                dr_dest(Resolver.TabColName(right_dt.TableName, column)) = dr_right(column)
                            Next
                        End If
                        dt.Rows.Add(dr_dest)
                    Case Else
                        ' Make a copy of the prototype datarow that we already filled in
                        ' above.  It already has the column data from the left table.
                        dr_temp.ItemArray = dr_dest.ItemArray
                        For Each dr_right In drs
                            dr_dest = dt.NewRow

                            ' Copy prototype row (the left table's data)
                            dr_dest.ItemArray = dr_temp.ItemArray

                            ' Copy the columns from the related rows in the right table
                            If right_dt.TableName = "__RIGHTTABLE__" Then
                                For i = 0 To right_dt.Columns.Count - 1
                                    ' fill in the holes, but do not overwrite the data
                                    ' that came from the left table
                                    If IsDBNull(dr_dest(i)) Then
                                        dr_dest(i) = dr_right(i)
                                    End If
                                Next
                            Else
                                For Each column In RightColumnList
                                    dr_dest(Resolver.TabColName(right_dt.TableName, column)) = dr_right(column)
                                Next
                            End If
                            dt.Rows.Add(dr_dest)
                        Next
                End Select
            End If
        Next

        ' delete the temporary relationship we created above
        ds.Relations.Remove("__RELATIONSHIP__")

        ' remove the temporary DataTables from the DataSet
        If LeftTable.TableName = "__LEFTTABLE__" Then
            ds.Tables.Remove(LeftTable)
        End If
        If RightTable.TableName = "__RIGHTTABLE__" Then
            ds.Tables.Remove(RightTable)
        End If

        Return dt
    End Function
End Class
