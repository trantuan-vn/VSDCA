' This class is designed to split the incoming SQL string into words, then pass those
' words off to be processed by the specialized "clause parsers"

Public Class Parser

    Private sc As SelectCommand

    Public Sub New(ByVal sc1 As SelectCommand)
        sc = sc1
    End Sub

    ' Prepare the SQL statement by breaking it up into words and feeding the words to
    ' the 6 "clause parsers"
    Public Sub ParseSQL(ByVal SQL As String)
        Dim bufs As ArrayList
        Dim i, j, from_index, where_index, groupby_index, having_index As Integer
        Dim orderby_index As Integer

        If IsNothing(SQL) OrElse SQL.trim = "" Then
            Throw New ApplicationException("Input error: Null SQL string")
        End If

        ' Split the SQL string into space-separated words taking into consideration
        ' the use of the "[", "]", and "'" characters
        bufs = SplitWords(SQL.toupper)

        ' Get some keyword locations in the SQL statement
        from_index = bufs.IndexOf("FROM")
        where_index = bufs.IndexOf("WHERE")
        groupby_index = bufs.IndexOf("GROUP")
        having_index = bufs.IndexOf("HAVING")
        orderby_index = bufs.IndexOf("ORDER")

        ' TODO: Consider adding a "hyper parser" that will break down nested select 
        ' statements for processing.

        ' A quick set of sanity checks... Anything missing?
        If bufs.Count < 0 OrElse bufs(0) <> "SELECT" Then
            Throw New ApplicationException("SQL syntax error: SELECT statement must be first")
        End If
        If bufs.LastIndexOf("SELECT") <> 0 Then
            Throw New ApplicationException("SQL implementation limit: Duplicate SELECT statements not supported")
        End If
        If from_index < 0 Then
            Throw New ApplicationException("SQL syntax error: Missing FROM statement")
        End If
        If having_index > 0 And groupby_index < 0 Then
            Throw New ApplicationException("SQL syntax error: Missing GROUP BY statement")
        End If

        ' Check to see if things are in the correct order
        If where_index > 0 And where_index < from_index Then
            Throw New ApplicationException("SQL syntax error: WHERE statement must follow FROM statement")
        End If
        If groupby_index > 0 And groupby_index < from_index Then
            Throw New ApplicationException("SQL syntax error: GROUP BY statement must follow FROM statement")
        End If
        If groupby_index > 0 And groupby_index < where_index Then
            Throw New ApplicationException("SQL syntax error: GROUP BY statement must follow WHERE statement")
        End If
        If having_index > 0 And having_index < groupby_index Then
            Throw New ApplicationException("SQL syntax error: HAVING statement must follow GROUP BY statement")
        End If
        If orderby_index > 0 And orderby_index < from_index Then
            Throw New ApplicationException("SQL syntax error: ORDER By statement must follow FROM statement")
        End If
        If orderby_index > 0 And orderby_index < groupby_index Then
            Throw New ApplicationException("SQL syntax error: ORDER By statement must follow GROUP BY statement")
        End If
        If orderby_index > 0 And orderby_index < having_index Then
            Throw New ApplicationException("SQL syntax error: ORDER By statement must follow HAVING statement")
        End If

        ' check for duplicate keywords
        If from_index <> bufs.LastIndexOf("FROM") Then
            Throw New ApplicationException("SQL syntax error: Duplicate FROM statements")
        End If
        If where_index <> bufs.LastIndexOf("WHERE") Then
            Throw New ApplicationException("SQL syntax error: Duplicate WHERE statements")
        End If
        If groupby_index <> bufs.LastIndexOf("GROUP") Then
            Throw New ApplicationException("SQL syntax error: Duplicate GROUP BY statements")
        End If
        If having_index <> bufs.LastIndexOf("HAVING") Then
            Throw New ApplicationException("SQL syntax error: Duplicate HAVING statements")
        End If
        If orderby_index <> bufs.LastIndexOf("ORDER") Then
            Throw New ApplicationException("SQL syntax error: Duplicate ORDER BY statements")
        End If

        ' Now we pass the SQL 'words' to the parsing routine for each clause.  We start
        ' by re-splitting the original string without the ToUpper
        bufs = SplitWords(SQL)
        sc.Command = "SELECT"

        ' Parse the first half of the SELECT Statement
        sc.SelectArgs = ParseSelectArgs(bufs.GetRange(1, from_index - 1))

        ' Parse the second half of the SELECT Statement.  We need the word count from
        ' SelectArgs to know where start passing words for the 2nd half. 
        i = UBound(sc.SelectArgs) + 1
        sc.SelectList = ParseSelect(bufs.GetRange(i + 1, from_index - 1 - i))

        ' Parse the FROM statement
        ' we stop at WHERE, GROUP BY, ORDER BY, or the end of the string
        i = IIf(where_index > 0, where_index - 1, bufs.Count - 1)
        j = IIf(groupby_index > 0, groupby_index - 1, bufs.Count - 1)
        i = IIf(i < j, i, j)
        j = IIf(orderby_index > 0, orderby_index - 1, bufs.Count - 1)
        i = IIf(i < j, i, j)
        sc.From = ParseFrom(bufs.GetRange(from_index + 1, i - from_index))

        ' Parse the WHERE statement
        If where_index > 0 Then
            ' we stop at GROUP BY, ORDER BY, or the end of the string
            i = IIf(groupby_index > 0, groupby_index - 1, bufs.Count - 1)
            j = IIf(orderby_index > 0, orderby_index - 1, bufs.Count - 1)
            i = IIf(i < j, i, j)
            sc.Where = ParseWhere(bufs.GetRange(where_index + 1, i - where_index))
        End If

        ' Parse the GROUP BY statement
        If groupby_index > 0 Then
            ' we stop at HAVING, ORDER BY, or the end of the string
            i = IIf(having_index > 0, having_index - 1, bufs.Count - 1)
            j = IIf(orderby_index > 0, orderby_index - 1, bufs.Count - 1)
            i = IIf(i < j, i, j)
            sc.GroupBy = ParseGroupBy(bufs.GetRange(groupby_index + 1, i - groupby_index))
        End If

        ' Parse the HAVING statement
        If having_index > 0 Then
            ' we stop at ORDER BY or the end of the string
            i = IIf(orderby_index > 0, orderby_index - 1, bufs.Count - 1)
            sc.Having = ParseHaving(bufs.GetRange(having_index + 1, i - having_index))
        End If

        ' Parse the ORDER BY statement
        If orderby_index > 0 Then
            ' we stop at the end of the string
            sc.OrderBy = ParseOrderBy(bufs.GetRange(orderby_index + 1, bufs.Count - 1 - orderby_index))
        End If
    End Sub

    ' This is similar to the Split method but it splits things into words based upon
    ' white space and operators... and it takes "escape characters" (brackets, single
    ' quotes, and #) into consideration.  Marked as shared... is used in Resolver
    Friend Shared Function SplitWords(ByVal SQL As String) As ArrayList
        Dim buf As String
        Dim IsInBrackets, IsInQuotes, TwoCharacter As Boolean
        Dim ans As New ArrayList
        Dim i, last_token As Integer
        Dim RightSide() As String = {"(", ",", "=", "<", "<=", ">", ">=", "*", "/", "+", "-"}
        Dim c As Char

        If IsNothing(SQL) Then
            Return ans
        End If

        ' Let's clean it up a wee bit before we start
        SQL = SQL.Trim.TrimEnd(";").Trim

        ' Okey dokey, let's poke thru hunting for white space and operators
        IsInBrackets = False
        IsInQuotes = False
        last_token = 0
        For i = 0 To SQL.Length - 1
            c = SQL.Chars(i)

            Select Case c
                Case "["
                    IsInBrackets = True
                Case "]"
                    IsInBrackets = False
                Case "'", "#", Chr(34)
                    ' Two single quotes means record just one
                    If buf <> "'" Or (i > 0 AndAlso SQL.Chars(i - 1) <> "'") Then
                        IsInQuotes = Not IsInQuotes
                    End If
                Case "+", "-", "/", "%", "|", "&", "^", "~", ")", ","
                    ' We parse them all here as separate words, even if we don't support
                    ' them all.
                    If Not IsInBrackets And Not IsInQuotes Then
                        ' Check to see if + and - is either positive/negative or as part
                        ' of scientific notation
                        If buf = "+" Or buf = "-" Then
                            If i + 1 <= SQL.Length - 1 AndAlso [Char].IsDigit(SQL.Chars(i + 1)) Then
                                If ans.Count > 0 AndAlso [Array].IndexOf(RightSide, Right(ans(ans.Count - 1), 1)) >= 0 Then
                                    ' OK, it's just a positive/negative sign
                                    Exit Select
                                End If
                                If i > 1 AndAlso [String].Compare(SQL.Chars(i - 1).ToString, "E", True) = 0 And [Char].IsDigit(SQL.Chars(i - 2)) Then
                                    ' OK, it's part of scientific notation
                                    Exit Select
                                End If
                            End If
                        End If

                        ' do we have a word that hasn't yet been saved?
                        If i - last_token > 0 Then
                            buf = SQL.Substring(last_token, i - last_token)
                            ans.Add(buf)
                        End If

                        ' add the token itself as a word
                        ans.Add(c.ToString)
                        last_token = i + 1
                    End If
                Case "*"
                    If Not IsInBrackets And Not IsInQuotes Then
                        ' the "*" could be a wildcard to the right of a dot
                        If i > 0 AndAlso SQL.Chars(i - 1) = "." Then
                            Exit Select
                        End If

                        ' do we have a word that hasn't yet been saved?
                        If i - last_token > 0 Then
                            buf = SQL.Substring(last_token, i - last_token)
                            ans.Add(buf)
                        End If

                        ' check for *=
                        If i + 1 < SQL.Length - 1 AndAlso SQL.Chars(i + 1) = "=" Then
                            ans.Add("*=")
                            i += 1
                        Else
                            ' add the token itself as a word
                            ans.Add(c.ToString)
                        End If
                        last_token = i + 1
                    End If
                Case "("
                    ' This is a bit different.  In order to detect functions we see if
                    ' the previous character was a letter.  If it is, we tack the "("
                    ' onto the end of the word
                    If Not IsInBrackets And Not IsInQuotes Then
                        If i > 0 AndAlso [Char].IsLetter(SQL.Chars(i - 1)) Then
                            buf = SQL.Substring(last_token, i - last_token + 1)
                            ' we make one exception to this rule for the IN operator
                            If buf = "IN(" Then
                                ans.Add("IN")
                                ans.Add("(")
                            Else
                                ans.Add(buf)
                            End If
                        Else
                            ' otherwise, just add the "(" as word itself
                            ans.Add(c.ToString)
                        End If
                        last_token = i + 1
                    End If
                Case "<", ">", "!", "="
                    ' Is this a two-character operator?
                    If Not IsInBrackets And Not IsInQuotes Then
                        ' do we have a word that hasn't yet been saved?
                        If i - last_token > 0 Then
                            buf = SQL.Substring(last_token, i - last_token)
                            ans.Add(buf)
                        End If

                        ' take a peek at the next character so we can distinguish
                        ' the two-character operators
                        TwoCharacter = False
                        If i + 1 < SQL.Length - 1 Then
                            buf = c & SQL.Chars(i + 1)
                            Select Case buf
                                Case "<>", "<=", ">=", "!=", "!<", "!>", "=*"
                                    ans.Add(buf)
                                    i += 1
                                    TwoCharacter = True
                            End Select
                        End If

                        If Not TwoCharacter Then
                            ' add the token itself as a word
                            ans.Add(c.ToString)
                        End If

                        last_token = i + 1
                    End If
                Case " ", vbTab, vbCr, vbLf
                    If Not IsInBrackets And Not IsInQuotes Then
                        buf = SQL.Substring(last_token, i - last_token)
                        ' this eliminates duplicate blanks
                        If buf <> "" Then
                            ans.Add(buf)
                        End If
                        last_token = i + 1
                    End If
            End Select

            ' are we there yet?
            If i = SQL.Length - 1 Then
                If IsInBrackets Then
                    Throw New ApplicationException("SQL syntax error: Mismatched ']'")
                End If
                If IsInQuotes Then
                    Throw New ApplicationException("SQL syntax error: Mismatched single quote")
                End If
                buf = SQL.Substring(last_token)
                ' this eliminates duplicate blanks
                If buf <> "" Then
                    ans.Add(buf)
                End If
            End If
        Next

        Return ans
    End Function

    ' Parse the first half of the Select clause

    ' SELECT [ ALL | DISTINCT ] 
    '    [ TOP n [ PERCENT ] ] 
    'ALL
    '   Specifies that duplicate rows can appear in the result set. ALL is the default.
    'DISTINCT
    '   Specifies that only unique rows can appear in the result set. Null values are
    '   considered equal for the purposes of the DISTINCT keyword.
    'TOP n [PERCENT]
    '   Specifies that only the first n rows are to be output from the query result set.
    '   n is an integer between 0 and 4294967295. If PERCENT is also specified, only the
    '   first n percent of the rows are output from the result set. When specified with
    '   PERCENT, n must be an integer between 0 and 100.

    '   If the query includes an ORDER BY clause, the first n rows (or n percent of rows)
    '   ordered by the ORDER BY clause are output. If the query has no ORDER BY clause,
    '   the order of the rows is arbitrary.

    ' Not Supported: "WITH TIES"

    Private Function ParseSelectArgs(ByVal SelectWords As ArrayList) As String()
        Dim i As Integer
        Dim ans(-1), buf As String
        Dim al As New ArrayList

        If IsNothing(SelectWords) OrElse SelectWords.Count = 0 Then
            Return ans
        End If

        ' Look for: Keywords only
        For i = 0 To SelectWords.Count - 1
            buf = SelectWords(i).ToUpper
            Select Case buf
                Case "ALL", "DISTINCT"
                    ' if used, it must be first in the list
                    If al.Count <> 0 Then
                        Throw New ApplicationException("SQL syntax error: Parsing error near 'DISTINCT'")
                    End If
                    al.Add(buf)
                Case "TOP"
                    al.Add("TOP")
                    ' get the number of rows (or percent)
                    If i + 1 <= SelectWords.Count - 1 AndAlso IsNumeric(SelectWords(i + 1)) Then
                        al.Add(SelectWords(i + 1))
                        i += 1
                    Else
                        Throw New ApplicationException("SQL syntax error: Parsing error after '" & buf & "' in SELECT")
                    End If
                    ' get the optional "PERCENT" options
                    If i + 1 <= SelectWords.Count - 1 AndAlso SelectWords(i + 1).ToUpper = "PERCENT" Then
                        al.Add("PERCENT")
                        i += 1
                    End If
                Case "WITH", "TIES"
                    Throw New ApplicationException("SQL implementation limit: Keyword WITH TIES is not supported")
                Case Else
                    ' OK, this must be the beginning of the selection list
                    Exit For
            End Select
        Next

        ' copy the answers
        If al.Count > 0 Then
            ans = al.ToArray(GetType(String))
        End If

        Return ans
    End Function

    ' Parse the 2nd half of the Select clause

    'SELECT < select_list > 
    '< select_list > ::= 
    '   { * 
    '   | { table_name | table_alias }.* 
    '   | { column_name | expression } [ [ AS ] column_alias ] 
    '   } [ ,...n ] 
    '
    '< select_list > 
    '   The columns to be selected for the result set. The select list is a series of
    '   expressions separated by commas. 
    '* 
    '   Specifies that all columns from all tables in the FROM clause should be returned.
    '   The columns are returned by table, as specified in the FROM clause, and in the
    '   order in which they exist in the table. 
    'table_name | table_alias.* 
    '   Limits the scope of the * to the specified table. Requires that all columns from
    '   the specified table in the FROM clause should be returned. The columns are
    '   returned in the order in which they exist in the table. If a table has an alias
    '   specified in the FROM clause, the alias must be used, and the use of the table
    '   name is not valid. 
    'column_name 
    '   Is the name of a column to return. Qualify column_name to prevent an ambiguous
    '   reference, such as occurs when two tables in the FROM clause have columns with
    '   duplicate names. For example, both the Customers and Orders tables in the
    '   Northwind database have a column named ColumnID. If the two tables are joined in
    '   a query, the customer ID can be specified in the select list as
    '   Customers.CustomerID. If a table alias is provided, the table alias must be used
    '   to qualify the column name. Otherwise, the table name should be used. 
    'expression 
    '   Is a column name, constant, function, or any valid combination of column names,
    '   constants, and functions connected by one or more operators. 
    'column_alias 
    '   Is an alternative name to replace the column name in the query results set. For
    '   example, an alias such as "Quantity", or "Quantity to Date", or "Qty" can be
    '   specified for a column named quantity. Aliases are used also to specify names
    '   for the results of expressions, for example: 

    '      SELECT AVG(UnitPrice) AS 'Average Price' FROM [Order Details]

    '   column_alias can be used in an ORDER BY clause, but it cannot be used in a
    '   WHERE, GROUP BY, or HAVING clause. 

    ' Not Supported: "|", "&", "^", "~", "INTO", "IDENTITYCOL", "ROWGUIDCOL", "CASE",
    '   "WHEN", "THEN", "ELSE", "END", "GROUPING", "COALESCE"
    ' Not Supported: Column aliases enclosed in quotes.

    Private Function ParseSelect(ByVal SelectWords As ArrayList) As SelectListCollection
        Dim buf, ExpressionBuf As String
        Dim i, level, ExpressionLevel, ExprCount, AggregateStart As Integer
        Dim IsInExpression, IsAggregate, IsAlias As Boolean
        Dim ans As New SelectListCollection
        Dim sl As SelectList

        If IsNothing(SelectWords) OrElse SelectWords.Count = 0 Then
            Return ans
        End If

        ExpressionLevel = 0
        IsInExpression = False
        IsAggregate = False
        IsAlias = False
        ExpressionBuf = ""
        level = 0
        ExprCount = 1
        sl = New SelectList

        ' Look for: Columns, Constants, Scalars, Aggregates, and Operators
        For i = 0 To SelectWords.Count - 1
            buf = SelectWords(i)

            Select Case buf.ToUpper
                Case "AS"
                    ' The next word is an alias
                    IsAlias = True
                    IsInExpression = False
                Case "("
                    level += 1
                    ExpressionBuf &= " " & buf
                    IsInExpression = True
                Case ")"
                    level -= 1
                    ' There is a limitation in the native ADO.Net handling of vector
                    ' aggregates.  These functions must have only a single column as an
                    ' argument.
                    If IsAggregate And i - AggregateStart > 2 Then
                        Throw New ApplicationException("SQL implementation limit: Expressions not allowed inside aggregation functions")
                    End If
                    ' OK, did we just finished an expression?
                    If level = ExpressionLevel Then
                        IsInExpression = False
                    End If
                    ExpressionBuf &= " " & buf
                Case "+", "-", "*", "/", "%"
                    ' Special case for the "*" character... it could be the wildcard
                    ' character for "all columns"
                    If buf = "*" AndAlso IsAlone(i, SelectWords) Then
                        sl.ColumnName = buf
                    Else
                        If ExpressionBuf = "" And i > 0 Then
                            ' Yikes, that previous word was part of the expression
                            ExpressionBuf = SelectWords(i - 1) & " " & buf
                            sl.TableName = ""
                            sl.ColumnName = ""
                            ExpressionLevel = level
                        Else
                            ExpressionBuf &= " " & buf
                        End If
                        IsInExpression = True
                    End If
                Case "SUM(", "AVG(", "MAX(", "MIN(", "COUNT(", "STDEV(", "VAR("
                    ' These are the only aggregation functions we support

                    ' is this the start of a new expression?
                    If Not IsInExpression Then
                        ExpressionLevel = level
                    End If
                    level += 1
                    IsInExpression = True
                    IsAggregate = True
                    AggregateStart = i
                    ExpressionBuf &= " " & buf.ToUpper
                Case "LEN(", "ISNULL(", "IIF(", "TRIM(", "SUBSTRING(", "CONVERT("
                    ' These are the only scalar functions that we support

                    ' NOTE: The arguments for the SQL CONVERT and the ADO.Net CONVERT
                    ' backwards. CONVERT arguments are translated in Resolver

                    ' is this the start of a new expression?
                    If Not IsInExpression Then
                        ExpressionLevel = level
                    End If
                    level += 1
                    IsInExpression = True
                    ExpressionBuf &= " " & buf.ToUpper
                Case ","
                    If IsInExpression And IsInsideFunction(ExpressionBuf) Then
                        ExpressionBuf &= " " & buf
                    Else
                        sl.Expression = ExpressionBuf.Trim
                        sl.IsAggregate = IsAggregate

                        ' If we don't have a alias for expression, we make one.
                        If sl.ColumnAlias = "" And sl.Expression <> "" Then
                            sl.ColumnAlias = "Expr" & ExprCount
                            ExprCount += 1
                        End If

                        ExpressionBuf = ""
                        IsAggregate = False
                        If sl.ColumnName = "" And sl.Expression = "" Then
                            Throw New ApplicationException("SQL syntax error: Misplaced ',' in SELECT")
                        End If

                        ans.Add(sl)
                        ' clear out the select_list structure so we can go at it again
                        sl = New SelectList
                    End If
                Case "|", "&", "^", "~"
                    Throw New ApplicationException("SQL implementation limit: '" & buf & "' bit-wise operator is not supported")
                Case "INTO", "IDENTITYCOL", "ROWGUIDCOL", "CASE", "WHEN", "THEN", "ELSE", "END", "GROUPING", "COALESCE"
                    Throw New ApplicationException("SQL implementation limit: Keyword " & buf.ToUpper & " is not supported")
                Case Else
                    ' is it an unhandled scalar function?
                    If buf.EndsWith("(") Then
                        Throw New ApplicationException("SQL implementation limit: Function " & buf.ToUpper.TrimEnd("(") & " is not supported")
                    End If

                    ' Is it a string, date, or numeric constant?  If so, then this must
                    ' be the start of an expression.
                    If IsConstant(buf) Then
                        If Not IsInExpression Then
                            ExpressionLevel = level
                        End If
                        IsInExpression = True
                    End If

                    If IsInExpression Then
                        ExpressionBuf &= " " & buf
                    Else
                        ' was this an explicit or implied alias?
                        If IsAlias Or sl.ColumnName <> "" Then
                            sl.ColumnAlias = buf
                            IsAlias = False
                        Else
                            sl.TableName = ParseNextToLastDot(buf)
                            sl.ColumnName = ParseLastDot(buf)

                            ' if using the dot notation, both fields must be populated
                            If sl.TableName = "" And sl.ColumnName = "" Then
                                Throw New ApplicationException("SQL syntax error: Misplaced '.' in SELECT")
                            End If
                        End If
                    End If
            End Select

            ' are we there yet?
            If i = SelectWords.Count - 1 Then
                sl.Expression = ExpressionBuf.Trim
                sl.IsAggregate = IsAggregate

                ' If we don't have a alias for expression, we make one.
                If sl.ColumnAlias = "" And sl.Expression <> "" Then
                    sl.ColumnAlias = "Expr" & ExprCount
                    ExprCount += 1
                End If

                ' we'd better check to see if somebody put in a few extra commas
                If sl.ColumnName = "" And sl.Expression = "" Then
                    Throw New ApplicationException("SQL syntax error: Misplaced ',' in SELECT")
                End If
                If level <> 0 Then
                    Throw New ApplicationException("SQL syntax error: Unmatched ')' in SELECT")
                End If

                ans.Add(sl)
            End If
        Next

        ' could be Null due to a SelectArg
        Return ans
    End Function

    ' Parse the From clause

    'FROM { < table_source > } [ ,...n ]  
    '< table_source > ::= 
    '   table_name [ [ AS ] table_alias ] 
    '   | < joined_table > 
    '< joined_table > ::= 
    '   < table_source > < join_type > < table_source > ON < search_condition > 
    '   | ( < joined_table > )
    '< join_type > ::= 
    '   [ INNER | { { LEFT | RIGHT | FULL } [ OUTER ] } ] JOIN 
    '
    '< table_source > 
    '   Specifies the tables and joined tables for the SELECT statement. 
    'table_name [ [ AS ] table_alias ] 
    '   Specifies the name of a table and an optional alias. 
    '< joined_table > 
    '   Is a result set that is the join of two or more tables. 
    '   For multiple joins, you can use parentheses to specify the order of the joins. 

    '< join_type > 
    '   Specifies the type of join operation. 
    'INNER 
    '   Specifies that all matching pairs of rows are returned. Discards unmatched rows
    '   from both tables. This is the default if no join type is specified. 
    'LEFT [ OUTER ] 
    '   Specifies that all rows from the left table that are not meeting the specified
    '   condition are included in the result set in addition to all rows returned by the
    '   inner join. Output columns from the left table are set to NULL. 
    'RIGHT [ OUTER ] 
    '   Specifies that all rows from the right table that are not meeting the specified
    '   condition are included in the result set in addition to all rows returned by the
    '   inner join. Output columns from the right table are set to NULL. 
    'FULL [ OUTER ] 
    '   If a row from either the left or right table does not match the selection
    '   criteria, specifies the row be included in the result set, and output columns
    '   that correspond to the other table be set to NULL. This is in addition to all
    '   rows usually returned by the inner join.
    'JOIN 
    '   Indicates that the specified tables should be joined. 
    'ON < search_condition > 
    '   Specifies the condition on which the join is based. The condition can specify
    '   any valid predicate, although columns and comparison operators are often used. 

    ' Not Supported: "OPENXML", "WITH", "CROSS"
    ' Not Supported: pre-SQL92 syntax for the join clause (implied cross join, "*=", "=*",
    '   join predicates in the Where clause, etc)
    ' Not Supported: ON search_condition with multiple predicates, expressions, or join
    '   operators other than "="
    ' Not Supported: Table aliases enclosed in double quotes
    '
    ' Note: The ON search_condition must be a single predicate with just two column
    '   names and the "=" operator.  No other syntax is supported.  Example:
    '       Customers.CustomerID = Orders.CustomerID

    Private Function ParseFrom(ByVal FromWords As ArrayList) As TableSourceCollection
        Dim buf, RealTableName, NextWord, LastOperator As String
        Dim TempTable, TempAlias As String
        Dim i As Integer
        Dim Joins As New Stack
        Dim ans As New TableSourceCollection
        Dim ts As New TableSource

        If IsNothing(FromWords) OrElse FromWords.Count = 0 Then
            Throw New ApplicationException("SQL syntax error: Missing FROM criteria")
        End If

        ' Joins have 4 parts:
        '
        '   LeftTable
        '   JOIN operator
        '   RightTable
        '   ON condition
        '
        ' The problem is that either the LeftTable or the RightTable could be another
        ' nested join:
        '
        '   LeftTable
        '   JOIN operator
        '       LeftTable        \
        '       JOIN operator     \  the product of this nested join becomes the new
        '       RightTable        /  RightTable for the first join
        '       ON condition     /
        '   ON condition
        '
        ' This makes parsing nested joins rather difficult.  We have to push incomplete
        ' structures onto a stack and then later pop them off the stack to fill in the
        ' missing parts.  Using the example above, I won't know the ON condition of the
        ' first join until after I've processed the entire second (nested) join.

        ' Look for: Tables and Columns
        For i = 0 To FromWords.Count - 1
            buf = FromWords(i)

            Select Case buf.ToUpper
                Case "AS"
                    RealTableName = ""
                    If i > 0 Then
                        RealTableName = FromWords(i - 1)
                    End If
                    ' The next word is an alias
                    NextWord = "Alias"
                Case "INNER", "LEFT", "RIGHT", "FULL"
                    ts.JoinType = buf.ToUpper
                Case "OUTER"
                    ' ignore
                Case "JOIN"
                    ' Joins are recorded in a stack because of the horrible SQL syntax
                    Select Case LastOperator
                        Case "JOIN"
                            ' we mistakenly recorded a RightTable
                            TempTable = ts.RightTable
                            TempAlias = ts.RightAlias
                            ts.RightTable = ""
                            ts.RightAlias = ""
                            ' push this incomplete structure onto a stack, then start a
                            ' new structure
                            Joins.Push(ts)
                            ts = New TableSource
                            ts.LeftTable = TempTable
                            ts.LeftAlias = TempAlias
                        Case "ON"
                            ' previous nested join is complete, so just start a new one
                            Joins.Push(ts)
                            ts = New TableSource
                        Case ""
                            ' this means it's the first join... (no stack operations)
                    End Select
                    If ts.JoinType = "" Then
                        ts.JoinType = "INNER"
                    End If
                    LastOperator = "JOIN"
                Case "ON"
                    ' The next word is a Left Column predicate
                    NextWord = "LeftColumn"
                    LastOperator = "ON"
                Case "="
                    ' The next word is a Right Column predicate
                    NextWord = "RightColumn"
                Case "<>", ">", ">=", "<", "<=", "!=", "!>", "!<", "LIKE", "IS", "IN", "NULL"
                    Throw New ApplicationException("SQL implementation limit: Join operator '" & buf & "' is not supported")
                Case "+", "-", "*", "/", "%", "|", "&", "^", "~"
                    Throw New ApplicationException("SQL implementation limit: Operators are not supported in FROM clause")
                Case ","
                    Throw New ApplicationException("SQL implementation limit: Implied CROSS JOIN is not supported")
                Case "OPENXML", "WITH", "CROSS"
                    Throw New ApplicationException("SQL implementation limit: Keyword " & buf.ToUpper & " is not supported")
                Case "(", ")"
                    ' ignore
                Case Else
                    ' constants are not supported here
                    If IsConstant(buf) Then
                        Throw New ApplicationException("SQL implementation limit: Constants are not supported in FROM clause")
                    End If
                    ' functions are not supported here
                    If buf.Length > 1 And buf.EndsWith("(") Then
                        Throw New ApplicationException("SQL implementation limit: Functions are not supported in FROM clause")
                    End If

                    ' We've got 4 possibilities... It's a left column, a right column,
                    ' an alias, or a table name
                    Select Case NextWord
                        Case "LeftColumn"
                            ts.LeftPredicateTable = ParseNextToLastDot(buf)
                            ts.LeftPredicateColumn = ParseLastDot(buf)
                            If ts.LeftPredicateTable = "" And ts.LeftPredicateColumn = "" Then
                                Throw New ApplicationException("SQL syntax error: Misplaced '.' in FROM")
                            End If
                        Case "RightColumn"
                            ts.RightPredicateTable = ParseNextToLastDot(buf)
                            ts.RightPredicateColumn = ParseLastDot(buf)
                            If ts.RightPredicateTable = "" And ts.RightPredicateColumn = "" Then
                                Throw New ApplicationException("SQL syntax error: Misplaced '.' in FROM")
                            End If
                            ' add this to our collection
                            ans.Add(ts)

                            If Joins.Count > 0 Then
                                ts = Joins.Pop
                            End If
                            RealTableName = ""
                        Case "Alias"
                            If ts.RightTable = "" Then
                                ts.LeftAlias = buf
                                ts.LeftTable = RealTableName
                            Else
                                ts.RightAlias = buf
                                ts.RightTable = RealTableName
                            End If
                        Case ""
                            If ts.JoinType = "" Then
                                If ts.LeftTable <> "" Then
                                    ts.LeftAlias = ParseLastDot(buf)
                                Else
                                    ts.LeftTable = ParseLastDot(buf)
                                End If
                            Else
                                If ts.RightTable <> "" Then
                                    ts.RightAlias = ParseLastDot(buf)
                                Else
                                    ts.RightTable = ParseLastDot(buf)
                                End If
                            End If
                    End Select
                    NextWord = ""
            End Select

            ' are we there yet?
            If i = FromWords.Count - 1 Then
                ' could be a table without any joins
                If ts.JoinType = "" Then
                    ans.Add(ts)
                End If
            End If
        Next

        If ans.Count = 0 Then
            Throw New ApplicationException("SQL syntax error: Missing FROM list")
        End If

        Return ans
    End Function

    ' Parse the Where clause

    '[ WHERE < search_condition >]

    '< search_condition > ::= 
    '    {    [ NOT ] < predicate > | ( < search_condition > ) } 
    '        [ { AND | OR } [ NOT ] { < predicate > | ( < search_condition > ) } ] 
    '    }    [ ,...n ] 

    '< predicate > ::= 
    '    {    expression { = | <> | != | > | >= | !> | < | <= | !< } expression 
    '        | string_expression [ NOT ] LIKE string_expression 
    '        | expression [ NOT ] BETWEEN expression AND expression 
    '        | expression IS [ NOT ] NULL 
    '        | expression [ NOT ] IN ( expression [ ,...n ] ) 
    '    } 

    '< search_condition > 
    '   Restricts the rows returned in the result set through the use of predicates.
    '   There is no limit to the number of predicates, separated by an AND clause, that
    '   can be included in a search condition.

    ' Not Supported: "ESCAPE", "CONTAINS", "FREETEXT", "ALL", "SOME", "ANY", "EXIST"
    ' Note:  The LIKE operator supports only the "*" and "%" wildcards and wildcards 
    '   must appear at the beginning or end of the string (it's a ADO.Net thing).

    Private Function ParseWhere(ByVal WhereWords As ArrayList) As SearchConditionCollection
        Dim buf, temp, IsBuf, InBuf, Operators(), ExpressionBuf As String
        Dim i, level, ExpressionLevel, ExpCount As Integer
        Dim IsInIn, IsInBetween, IsInIs, IsInExpression As Boolean
        Dim ans As New SearchConditionCollection
        Dim wh As New SearchCondition

        ' They can only be killed with silver bullets  :)
        If IsNothing(WhereWords) OrElse WhereWords.Count = 0 Then
            Throw New ApplicationException("SQL syntax error: Missing selection criteria in WHERE")
        End If

        Operators = New String() {"=", "<>", ">", ">=", "<", "<=", "LIKE", "BETWEEN", "IN"}
        level = 0
        ExpressionLevel = 0
        ExpCount = 0
        IsInIn = False
        IsInIs = False
        IsInBetween = False
        IsInExpression = False
        IsBuf = ""
        InBuf = ""
        ExpressionBuf = ""

        ' Look for: Columns, Constants, Scalar Functions, and Operators
        For i = 0 To WhereWords.Count - 1
            buf = WhereWords(i)

            Select Case buf.ToUpper
                Case "("
                    level += 1
                    If IsInExpression Then
                        ExpressionBuf &= " " & buf
                    End If
                Case ")"
                    ' some folks nest things for no darn reason
                    level -= 1
                    If IsInExpression Then
                        If IsInExpression Then
                            ExpressionBuf &= " " & buf
                        End If
                    Else
                        If IsInIn Then
                            wh.Arg1 = "( " & InBuf & " )"
                            ans.Add(wh)
                            wh = New SearchCondition
                            InBuf = ""
                            IsInIn = False
                        End If
                    End If
                Case "=", "<>", ">", ">=", "<", "<=", "LIKE", "!=", "!>", "!<"
                    ' NOTE: Native ADO.Net doesn't support the !=, !>, or !< operators,
                    ' but we record them here and translate them in Resolver
                    If IsInExpression Then
                        ExpressionBuf &= " " & buf
                    Else
                        ' do we already have an operator?
                        If wh.[Operator] = "NOT" Then
                            wh.[Operator] = "NOT " & buf.ToUpper
                        Else
                            wh.[Operator] = buf.ToUpper
                        End If
                        wh.NumArgs = 2
                        wh.Level = level
                    End If
                Case "+", "-", "*", "/", "%"
                    If ExpressionBuf = "" And i > 0 Then
                        ' Yikes, that previous word was part of the expression
                        ExpressionBuf = WhereWords(i - 1) & " " & buf
                        wh.TableName = ""
                        wh.ColumnName = ""
                        ExpressionLevel = level
                    Else
                        ExpressionBuf &= " " & buf
                    End If
                    IsInExpression = True
                Case "AND"
                    ' "AND" can appear in 2 locations: as part of the BETWEEEN operator,
                    ' or as a stand alone logical operator.

                    If IsInExpression And ExpressionLevel <> level Then
                        ExpressionBuf &= " " & buf
                    Else
                        ' If this is a BETWEEN operator, let's peek ahead for the second half
                        If IsInBetween And i + 1 <= WhereWords.Count - 1 Then
                            wh.Arg2 = WhereWords(i + 1)
                            wh.NumArgs = 3
                            wh.Level = level
                            ans.Add(wh)
                            wh = New SearchCondition
                            i += 1
                            IsInBetween = False
                        Else
                            If IsInExpression And ExpressionLevel = level Then
                                wh.Expression = ExpressionBuf
                                wh.ExpAlias = "__WHERE" & ExpCount & "__"
                                ExpCount += 1
                                wh.NumArgs = 0
                                wh.ColumnName = ""
                                wh.TableName = ""
                                wh.[Operator] = ""
                                wh.Level = level
                                ans.Add(wh)
                                wh = New SearchCondition
                                IsInExpression = False
                                ExpressionBuf = ""

                            End If
                            wh.NumArgs = 0
                            wh.[Operator] = buf.ToUpper
                            wh.Level = level
                            ans.Add(wh)
                            wh = New SearchCondition
                        End If
                    End If
                Case "OR"
                    If IsInExpression And ExpressionLevel <> level Then
                        ExpressionBuf &= " " & buf
                    Else
                        If IsInExpression And ExpressionLevel = level Then
                            wh.Expression = ExpressionBuf
                            wh.ExpAlias = "__WHERE" & ExpCount & "__"
                            ExpCount += 1
                            wh.NumArgs = 0
                            wh.ColumnName = ""
                            wh.TableName = ""
                            wh.[Operator] = ""
                            wh.Level = level
                            ans.Add(wh)
                            wh = New SearchCondition
                            IsInExpression = False
                            ExpressionBuf = ""
                        End If
                        wh.NumArgs = 0
                        wh.[Operator] = buf.ToUpper
                        wh.Level = level
                        ans.Add(wh)
                        wh = New SearchCondition
                    End If
                Case "NOT"
                    ' "NOT" can appear in 3 locations:  As part of the IS NULL function,
                    ' as a modifier to an operator, or as a stand alone logical operator
                    If IsInExpression And ExpressionLevel <> level Then
                        ExpressionBuf &= " " & buf
                    Else
                        If IsInIs Then
                            IsBuf = "IS NOT"
                        Else
                            ' Let's peek ahead to see if this is a modifier to an operator
                            If i + 1 <= WhereWords.Count - 1 Then
                                temp = WhereWords(i + 1).ToUpper
                                If [Array].IndexOf(Operators, temp) > 0 Then
                                    wh.[Operator] = "NOT"
                                End If
                            Else
                                ' OK, this is just a logical operator
                                If IsInExpression And ExpressionLevel = level Then
                                    wh.Expression = ExpressionBuf
                                    wh.ExpAlias = "__WHERE" & ExpCount & "__"
                                    ExpCount += 1
                                    wh.NumArgs = 0
                                    wh.ColumnName = ""
                                    wh.TableName = ""
                                    wh.[Operator] = ""
                                    wh.Level = level
                                    ans.Add(wh)
                                    wh = New SearchCondition
                                    IsInExpression = False
                                    ExpressionBuf = ""
                                End If
                                wh.NumArgs = 0
                                wh.[Operator] = buf.ToUpper
                                wh.Level = level
                                ans.Add(wh)
                                wh = New SearchCondition
                            End If
                        End If
                    End If
                Case "BETWEEN"
                    ' NOTE: The BETWEEN operator isn't supported by native ADO.Net, but
                    ' we record it here and translate it later into equivalent commands

                    If IsInExpression Then
                        ExpressionBuf &= " " & buf
                    Else
                        ' Let's peek ahead to get the first half
                        If i + 1 <= WhereWords.Count - 1 Then
                            wh.Arg1 = WhereWords(i + 1)
                            i += 1
                        Else
                            Throw New ApplicationException("SQL syntax error: Parsing error near 'BETWEEN' in WHERE")
                        End If
                        IsInBetween = True

                        ' do we already have an operator?
                        If wh.[Operator] = "NOT" Then
                            wh.[Operator] = "NOT " & buf.ToUpper
                        Else
                            wh.[Operator] = buf.ToUpper
                        End If
                        wh.NumArgs = 3
                        wh.Level = level
                    End If
                Case "IN"
                    If IsInExpression Then
                        ExpressionBuf &= " " & buf
                    Else
                        IsInIn = True
                        ' do we already have an operator?
                        If wh.[Operator] = "NOT" Then
                            wh.[Operator] = "NOT " & buf.ToUpper
                        Else
                            wh.[Operator] = buf.ToUpper
                        End If
                        wh.NumArgs = 2
                        wh.Level = level
                    End If
                Case "IS"
                    If IsInExpression Then
                        ExpressionBuf &= " " & buf
                    Else
                        IsInIs = True
                        wh.[Operator] = buf.ToUpper
                        wh.Level = level
                        IsBuf = "IS"
                    End If
                Case "NULL"
                    If IsInExpression Then
                        ExpressionBuf &= " " & buf
                    Else
                        ' sanity check...
                        If IsBuf.Length >= 2 AndAlso IsBuf.StartsWith("IS") Then
                            IsBuf &= " NULL"
                            IsInIs = False

                            wh.[Operator] = IsBuf
                            wh.Level = level

                            ' unary operator, so we process it here
                            wh.NumArgs = 1
                            ans.Add(wh)
                            wh = New SearchCondition
                            IsBuf = ""
                        Else
                            Throw New ApplicationException("SQL syntax error: Parsing error near 'NULL' in WHERE")
                        End If
                    End If
                Case "LEN(", "ISNULL(", "IIF(", "TRIM(", "SUBSTRING(", "CONVERT("
                    ' is this the start of a new expression?
                    If Not IsInExpression Then
                        ExpressionLevel = level
                    End If
                    level += 1
                    IsInExpression = True
                    ExpressionBuf &= " " & buf.ToUpper
                Case "SUM(", "AVG(", "MAX(", "MIN(", "COUNT(", "STDEV(", "VAR("
                    Throw New ApplicationException("SQL syntax error:  Aggregate functions are not allowed in WHERE clause")
                Case "ESCAPE", "CONTAINS", "FREETEXT", "ALL", "SOME", "ANY", "EXIST"
                    Throw New ApplicationException("SQL implementation limit: Keyword " & buf.ToUpper & " is not supported in WHERE")
                Case Else
                    ' is it an unhandled scalar function?
                    If buf.EndsWith("(") Then
                        Throw New ApplicationException("SQL implementation limit: Function " & buf.ToUpper.TrimEnd("(") & " is not supported")
                    End If

                    If IsInExpression Then
                        ExpressionBuf &= " " & buf
                    Else
                        If IsInIn Then
                            InBuf &= buf
                        Else
                            ' This is either a table/column or a filter depending on if we
                            ' already have an operator
                            If wh.[Operator] = "" Then
                                wh.TableName = ParseNextToLastDot(buf)
                                wh.ColumnName = ParseLastDot(buf)
                                If wh.TableName = "" And wh.ColumnName = "" Then
                                    Throw New ApplicationException("SQL syntax error: Misplaced '.' in WHERE")
                                End If
                            Else
                                wh.Arg1 = buf
                            End If

                            ' Do we have all 3 parts of a search condition(column, operator, and
                            ' filter)? Unary and 4 parts search conditions are handled separately
                            If Not IsInBetween And (wh.ColumnName <> "" And wh.[Operator] <> "" And wh.Arg1 <> "") Then
                                ans.Add(wh)
                                wh = New SearchCondition
                            End If
                        End If
                    End If
            End Select

            ' are we there yet?
            If i = WhereWords.Count - 1 Then
                If IsInExpression And ExpressionBuf <> "" Then
                    wh.Expression = ExpressionBuf
                    wh.ExpAlias = "__WHERE" & ExpCount & "__"
                    wh.NumArgs = 0
                    wh.ColumnName = ""
                    wh.TableName = ""
                    wh.[Operator] = ""
                    wh.Level = level
                    ans.Add(wh)
                Else
                    If wh.ColumnName <> "" Then
                        ans.Add(wh)
                    End If
                End If
            End If
        Next

        If ans.Count = 0 Then
            Throw New ApplicationException("SQL syntax error: Missing WHERE list")
        End If

        Return ans
    End Function
    ' Parse the Group By clause

    '[ GROUP BY group_by_expression [ ,...n ] ] 

    'group_by_expression 
    '   Is an expression on which grouping is performed. group_by_expression is also
    '   known as a grouping column. group_by_expression can be a column or a nonaggregate
    '   expression that references a column. A column alias that is defined in the select
    '   list cannot be used to specify a grouping column. Aggregate expressions cannot be
    '   specified in a group_by_expression. 

    ' Not Supported: "ALL", "WITH", "CUBE", "ROLLUP"

    Private Function ParseGroupBy(ByVal GroupByWords As ArrayList) As GroupByExpressionCollection
        Dim buf As String
        Dim i, Level, ExpressionLevel As Integer
        Dim ExpressionBuf As String
        Dim IsInExpression As Boolean
        Dim ans As New GroupByExpressionCollection
        Dim gb As New GroupByExpression

        If IsNothing(GroupByWords) OrElse GroupByWords.Count = 0 Then
            Throw New ApplicationException("SQL syntax error: Missing GROUP BY criteria")
        End If
        If GroupByWords(0).ToUpper <> "BY" Then
            Throw New ApplicationException("SQL syntax error: Parsing error after GROUP")
        End If

        IsInExpression = False
        ExpressionBuf = ""

        ' Look for: Columns, Constants, Scalar Functions, and Operators
        For i = 1 To GroupByWords.Count - 1
            buf = GroupByWords(i)

            Select Case buf.ToUpper
                Case "("
                    Level += 1
                Case ")"
                    Level -= 1
                    ' OK, did we just finished an expression?
                    If Level = ExpressionLevel Then
                        IsInExpression = False
                    End If
                    ExpressionBuf &= " " & buf
                Case "+", "-", "*", "/"
                    ' binary operators signal that an expression has already begun
                    If Not IsInExpression Then
                        ' Oops! we have to go back and add the previous word as part of
                        ' the expression.
                        If i > 0 Then
                            ExpressionBuf = GroupByWords(i - 1) & " " & buf
                            ExpressionLevel = Level
                            IsInExpression = True
                        Else
                            Throw New ApplicationException("SQL syntax error: Misplaced '" & buf & "' in GROUP BY")
                        End If
                    End If
                    ExpressionBuf &= " " & buf
                Case "LEN(", "ISNULL(", "IIF(", "TRIM(", "SUBSTRING(", "CONVERT("
                    ' is this the start of a new expression?
                    If Not IsInExpression Then
                        ExpressionLevel = Level
                        IsInExpression = True
                    End If
                    Level += 1
                    ExpressionBuf &= " " & buf.ToUpper
                Case ","
                    If IsInExpression And IsInsideFunction(ExpressionBuf) Then
                        ExpressionBuf &= " " & buf
                    Else
                        gb.Expression = ExpressionBuf.Trim
                        ans.Add(gb)
                        gb = New GroupByExpression
                        ExpressionBuf = ""
                    End If
                Case "SUM(", "AVG(", "MAX(", "MIN(", "COUNT(", "STDEV(", "VAR("
                    Throw New ApplicationException("SQL syntax error: Aggregate functions are not allowed in GROUP BY clause")
                Case "ALL", "WITH", "CUBE", "ROLLUP"
                    Throw New ApplicationException("SQL implementation limit: Keyword " & buf.ToUpper & " is not supported")
                Case Else
                    ' is it an unsupported scalar function?
                    If buf.EndsWith("(") Then
                        Throw New ApplicationException("SQL implementation limit: Function " & buf.ToUpper.TrimEnd("(") & " is not supported")
                    End If

                    If IsInExpression Then
                        ExpressionBuf &= " " & buf
                    Else
                        ' is it a constant? If so then we start an expression
                        If IsConstant(buf) Then
                            ExpressionLevel = Level
                            IsInExpression = True
                            ExpressionBuf &= " " & buf
                        Else
                            gb.TableName = ParseNextToLastDot(buf)
                            gb.ColumnName = ParseLastDot(buf)
                            ' if you're using the dot notation, both fields must be populated
                            If gb.TableName = "" And gb.ColumnName = "" Then
                                Throw New ApplicationException("SQL syntax error: Misplaced '.' in GROUP BY")
                            End If
                        End If
                    End If
            End Select

            ' are we there yet?
            If i = GroupByWords.Count - 1 Then
                gb.Expression = ExpressionBuf.Trim
                ans.Add(gb)
            End If
        Next

        If ans.Count = 0 Then
            Throw New ApplicationException("SQL syntax error: Missing GROUP BY list")
        End If

        Return ans
    End Function
    ' Parse the Having clause

    '[ HAVING < search_condition > ]

    '< search_condition > ::= 
    '    {    [ NOT ] < predicate > | ( < search_condition > ) } 
    '        [ { AND | OR } [ NOT ] { < predicate > | ( < search_condition > ) } ] 
    '    }    [ ,...n ] 

    '< predicate > ::= 
    '    {    expression { = | <> | != | > | >= | !> | < | <= | !< } expression 
    '        | string_expression [ NOT ] LIKE string_expression 
    '        | expression [ NOT ] BETWEEN expression AND expression 
    '        | expression IS [ NOT ] NULL 
    '        | expression [ NOT ] IN ( expression [ ,...n ] ) 
    '    } 

    '< search_condition > 
    '   Specifies the search condition for the group to meet. The search condition can
    '   use aggregate expressions and nonaggregate expressions. The only columns that
    '   can be used in the nonaggregate expressions are those specified as grouping
    '   columns in the GROUP BY clause. This is because the group-by columns represent
    '   common properties for the entire group. Likewise, the aggregate expressions
    '   represent a common property for the entire group. The HAVING clause search
    '   condition is expressing a predicate over the properties of the group. 

    ' Not Supported: Group By columns and scalar functions

    Private Function ParseHaving(ByVal HavingWords As ArrayList) As String
        Dim buf, ans As String

        If IsNothing(HavingWords) OrElse HavingWords.Count = 0 Then
            Throw New ApplicationException("SQL syntax error: Missing HAVING criteria")
        End If
        ' Since the HAVING clause is always an Aggregate function (or an expression with
        ' aggregates) there really isn't much to do here...

        ' Look for: Constants, Aggregate Functions, and Operators
        For Each buf In HavingWords
            Select Case buf.ToUpper
                Case "SUM(", "AVG(", "MAX(", "MIN(", "COUNT(", "STDEV(", "VAR("
                    ans &= buf.ToUpper & " "
                Case "LEN(", "ISNULL(", "IIF(", "TRIM(", "SUBSTRING(", "CONVERT("
                    Throw New ApplicationException("SQL syntax error: Scalar functions are not allowed in HAVING clause")
                Case Else
                    ' unsupported function?
                    If buf.Length > 1 And buf.EndsWith("(") Then
                        Throw New ApplicationException("SQL implementation limit: Function " & buf.ToUpper.TrimEnd("(") & " is not supported")
                    End If
                    ' everything else is OK
                    ans &= buf & " "
            End Select
        Next

        Return ans.Trim
    End Function
    ' Parse the Order By clause

    '[ ORDER BY { order_by_expression [ ASC | DESC ] }     [ ,...n] ] 

    'order_by_expression 
    '   Specifies a column on which to sort. A sort column can be specified as a name or
    '   column alias (which can be qualified by the table name) or an expression.
    '   Multiple sort columns can be specified. The sequence of the sort columns in the
    '   ORDER BY clause defines the organization of the sorted result set. 
    '
    '   The ORDER BY clause can include items not appearing in the select list. 

    Private Function ParseOrderBy(ByVal OrderByWords As ArrayList) As OrderByExpressionCollection
        Dim buf As String
        Dim i As Integer
        Dim Operators() As String = {"+", "-", "*", "/", "%", "|", "&", "^", "~"}
        Dim ans As New OrderByExpressionCollection
        Dim ob As New OrderByExpression

        ' some initial sanity checking...
        If IsNothing(OrderByWords) OrElse OrderByWords.Count = 0 Then
            Throw New ApplicationException("SQL syntax error: Missing ORDER BY criteria")
        End If
        If OrderByWords(0).ToUpper <> "BY" Then
            Throw New ApplicationException("SQL syntax error: Parsing error after ORDER")
        End If

        ob.SortOrder = "ASC"

        ' Look for: Columns, Aliases, and Constants (integer only)
        For i = 1 To OrderByWords.Count - 1
            buf = OrderByWords(i)

            Select Case buf.ToUpper
                Case "DESC", "ASC"
                    ob.SortOrder = buf.ToUpper
                Case ","
                    ans.Add(ob)
                    ob = New OrderByExpression
                    ob.SortOrder = "ASC"
                Case "(", ")"
                    ' ignore...
                Case Else
                    ' TODO: Allow functions here, and then use resolver to match them
                    ' to their column ordinals

                    ' functions are not supported here
                    If (buf.Length > 1 And buf.EndsWith("(")) Or [Array].IndexOf(Operators, buf) >= 0 Then
                        Throw New ApplicationException("SQL implementation limit: Expressions are not supported in ORDER BY")
                    End If

                    ' constants are not supported here
                    If Left(buf, 1) = "'" Or Left(buf, 1) = "#" Then
                        Throw New ApplicationException("SQL implementation limit: Constants are not supported in ORDER BY")
                    End If

                    If IsNumeric(buf) Then
                        ob.ColumnIndex = CInt(buf)
                    Else
                        ' column aliases *are* allowed here
                        ob.TableName = ParseNextToLastDot(buf)
                        ob.ColumnName = ParseLastDot(buf)
                        ' if you're using the dot notation, both fields must be populated
                        If ob.TableName = "" And ob.ColumnName = "" Then
                            Throw New ApplicationException("SQL syntax error: Misplaced '.' in ORDER BY")
                        End If
                    End If
            End Select

            ' are we there yet?
            If i = OrderByWords.Count - 1 Then
                ans.Add(ob)
            End If
        Next

        If ans.Count = 0 Then
            Throw New ApplicationException("SQL syntax error: Missing ORDER BY list")
        End If

        Return ans
    End Function

    ' Given a Server.Owner.Database.Table.Column notation, return the Table.  Marked as 
    ' shared cause it's used a lot in the other classes
    Friend Shared Function ParseNextToLastDot(ByVal buf As String) As String
        Dim bufs(), ans As String

        If Not IsNothing(buf) Then
            If buf.IndexOf(".") > 0 Then
                bufs = buf.Split(".")
                ans = bufs(UBound(bufs) - 1)
            End If
        End If

        Return ans
    End Function

    ' Given a Server.Owner.Database.Table.Column notation, return the Column.  Marked as 
    ' shared cause it's used a lot in the other classes
    Friend Shared Function ParseLastDot(ByVal buf As String) As String
        Dim bufs(), ans As String

        If Not IsNothing(buf) Then
            If buf.IndexOf(".") > 0 Then
                bufs = buf.Split(".")
                ans = bufs(UBound(bufs))
            Else
                ans = buf
            End If
        End If

        Return ans
    End Function

    ' Is this word all alone? (poor thing...)
    Private Function IsAlone(ByVal i As Integer, ByVal words As ArrayList) As Boolean

        ' Is this there only one word?
        If words.Count = 1 Then
            Return True
        End If

        ' Is the next word a comma?
        If i + 1 <= words.Count - 1 AndAlso words(i + 1) = "," Then
            Return True
        End If

        ' Was the previous word a comma?
        If i - 1 >= 0 AndAlso words(i - 1) = "," Then
            Return True
        End If

        Return False
    End Function

    ' Is this a string, date, or numeric constant?
    Private Function IsConstant(ByVal buf As String) As Boolean
        Dim c As Char

        If buf = "" Then
            Return False
        End If

        c = buf.Chars(0)

        ' string, date, or number
        If c = "'" Or c = "#" Or [Char].IsDigit(c) Then
            Return True
        End If

        ' a positive/negative sign
        If c = "+" Or c = "-" Then
            If buf.Length > 1 AndAlso [Char].IsDigit(buf.Chars(1)) Then
                Return True
            End If
        End If

        ' I guess not...
        Return False
    End Function

    ' Is this an incomplete function?  We can tell by a mismatched set of parentheses
    Private Function IsInsideFunction(ByVal Expression As String) As Boolean
        Dim buf() As Char
        Dim i, count As Integer

        buf = Expression.ToCharArray
        For i = 0 To UBound(buf)
            If buf(i) = "(" Then
                count += 1
            End If
            If buf(i) = ")" Then
                count -= 1
            End If
        Next

        Return CBool(count)
    End Function

End Class
