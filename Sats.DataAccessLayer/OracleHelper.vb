Imports System
Imports System.Data
Imports System.Xml
Imports System.IO
Imports Oracle.DataAccess.Client
Imports Oracle.DataAccess.Types
Imports Sats.CommonLibrary

Public Class OracleHelper
    Private mv_cn As OracleConnection
    Private mv_txn As OracleTransaction
    Private mv_strConnectionString As String

#Region " Các phương thức private và hàm constructor "
    Public Sub New()
        'Không làm gì cả
    End Sub

    Private Shared Sub AttachParameters(ByVal Command As OracleCommand, ByVal CommandParameter() As OracleParameter)
        Dim p As OracleParameter

        For Each p In CommandParameter
            If (p.Direction = ParameterDirection.InputOutput) And (p.Value Is Nothing) Then
                p.Value = Nothing
            End If
            Command.Parameters.Add(p)
        Next p
    End Sub

    Private Shared Sub AssignParameterValues(ByVal CommandParameters() As OracleParameter, ByVal ParameterValues() As Object)
        Dim i, j As Short

        If (CommandParameters Is Nothing) And (ParameterValues Is Nothing) Then
            'Không làm gì cả nếu chúng ta không có dữ liệu
            Return
        End If

        'Số lượng các tham số và số lượng các giá trị phải bằng nhau
        If CommandParameters.Length <> ParameterValues.Length Then
            Throw New ArgumentException("Số lượng giá trị các tham số không bằng số lượng các tham số!")
        End If

        'Gán giá trị cho tham số
        j = CommandParameters.Length - 1
        For i = 0 To j
            CommandParameters(i).Value = ParameterValues(i)
        Next
    End Sub

    Private Shared Sub PrepareCommand(ByRef Command As OracleCommand, _
                                      ByVal Connection As OracleConnection, _
                                      ByVal CommandType As CommandType, _
                                      ByVal CommandText As String, _
                                      ByVal CommandParameters() As OracleParameter)

        'Gán command text (là tên của stored procedure hoặc câu lệnh SQL)
        Command.CommandText = CommandText

        'Gán command type
        Command.CommandType = CommandType

        'Không có timeout
        Command.CommandTimeout = 0 '30 phút

        'Mở kết nối đến CSDL
        If Connection.State <> ConnectionState.Open Then
            Connection.Open()
        End If

        'Gán connection cho command
        Command.Connection = Connection


        'Attach các tham số cho câu lệnh
        If Not (CommandParameters Is Nothing) Then
            AttachParameters(Command, CommandParameters)
        End If

        Return
    End Sub
#End Region

#Region " ExecuteNonQuery "
    Public Overloads Shared Function ExecuteNonQuery(ByVal ConnectionString As String, _
                                                     ByVal CommandType As CommandType, _
                                                     ByVal CommandText As String) As Integer
        Return ExecuteNonQuery(ConnectionString, CommandType, CommandText, CType(Nothing, OracleParameter()))
    End Function

    Public Overloads Shared Function ExecuteNonQuery(ByVal ConnectionString As String, _
                                                         ByVal CommandType As CommandType, _
                                                         ByVal CommandText As String, _
                                                         ByVal ParamArray CommandParameters() As OracleParameter) As Integer
        'Tạo kết nối đến CSDL, dispose sau khi dùng xong
        Dim cn As New OracleConnection(ConnectionString)

        Try
            'cn.Open()

            'Chuyển cho hàm ExecuteNonQuery với Connection thay cho ConnectionString
            Return ExecuteNonQuery(cn, CommandType, CommandText, CommandParameters)
        Catch ex As Exception
            Throw ex
        Finally
            cn.Dispose()
        End Try
    End Function

    Public Overloads Shared Function ExecuteNonQuery(ByVal Connection As OracleConnection, _
                                                     ByVal CommandType As CommandType, _
                                                     ByVal CommandText As String, _
                                                     ByVal ParamArray CommandParameters() As OracleParameter) As Integer
        Dim cmd As New OracleCommand
        Dim retval As Integer
        Try
            'Tạo 1 command và chuẩn bị các tham số


            PrepareCommand(cmd, Connection, CommandType, CommandText, CommandParameters)

            'Execute the command
            retval = cmd.ExecuteNonQuery
            ' ChienCA add to debug INDEX :D
            'Dim path As String = "C:\SQLIndex\Temp.txt"
            'Dim sw As StreamWriter
            'sw = File.AppendText(path)
            'sw.WriteLine(Connection.ConnectionString.ToString + "--" + CommandText)
            'sw.Flush()
            'sw.Close()
            ' End ChienCA
            'Clear các tham số kh?i command �để có thể sử dụng lại v? sau
            cmd.Parameters.Clear()
            Return retval
        Catch ex As Exception
            LogError.Write(ex.Message & vbNewLine & CommandText, EventLogEntryType.Error, gc_MODULE_HOST)
            Throw ex
        Finally
            cmd.Dispose()
        End Try


    End Function

    Public Overloads Shared Function ExecuteNonQuery(ByVal ConnectionString As String, _
                                                     ByVal StoredProceduredName As String, _
                                                     ByVal ParameterNames() As Object, _
                                                     ByVal ParameterValues() As Object, _
                                                     ByVal ParameterTypes() As Object) As Integer
        Dim CommandParameters As OracleParameter()
        Dim cn As OracleConnection
        Try


            'N�ếu có giá trị của tham số
            If Not (ParameterValues Is Nothing) And ParameterValues.Length > 0 Then
                cn = New OracleConnection(ConnectionString)
                Dim cmd As New OracleCommand(StoredProceduredName)
                For i As Integer = 0 To ParameterNames.Length - 1
                    Dim v_Parameter As OracleParameter
                    cmd.CommandType = CommandType.StoredProcedure
                    v_Parameter = New OracleParameter(CStr(ParameterNames(i)), ParameterTypes(i))
                    v_Parameter.Direction = ParameterDirection.Input
                    v_Parameter.Value = ParameterValues(i)
                    cmd.Parameters.Add(v_Parameter)
                Next
                cn.Open()
                cmd.Connection = cn
                Return cmd.ExecuteNonQuery()
            Else 'Không có giá trị của tham số
                'Thực hiện command không có tham số
                Return ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, StoredProceduredName)
            End If
        Catch ex As Exception
            Throw ex
        Finally
            If Not cn Is Nothing Then
                cn.Dispose()
            End If
        End Try
    End Function
    ' ChienTD add for run Store, Value MAS 26/06/2008
    Public Overloads Shared Function ExecuteNonQuery(ByVal ConnectionString As String, _
                                                     ByVal StoredProceduredName As String, _
                                                     ByVal ParameterNames As ArrayList, _
                                                     ByVal ParameterValues As ArrayList, _
                                                     ByVal ParameterTypes As ArrayList) As Integer
        Dim CommandParameters As OracleParameter()
        Dim cn As OracleConnection
        Try


            'Nếu có giá trị của tham số
            If Not (ParameterValues Is Nothing) And ParameterValues.Count > 0 Then
                cn = New OracleConnection(ConnectionString)
                Dim cmd As New OracleCommand(StoredProceduredName)
                For i As Integer = 1 To ParameterNames.Count
                    Dim v_Parameter As OracleParameter
                    cmd.CommandType = CommandType.StoredProcedure
                    v_Parameter = New OracleParameter(CStr(ParameterNames(i - 1)), ParameterTypes(i - 1))
                    v_Parameter.Direction = ParameterDirection.Input
                    v_Parameter.Value = ParameterValues(i - 1)
                    cmd.Parameters.Add(v_Parameter)
                Next
                cn.Open()
                cmd.Connection = cn
                Return cmd.ExecuteNonQuery()
            Else 'Không có giá trị của tham số
                'Thực hiện command không có tham số
                Return ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, StoredProceduredName)
            End If
        Catch ex As Exception
            Throw ex
        Finally
            If cn Is Nothing Then
                cn.Dispose()
            End If
        End Try
    End Function

    Public Overloads Shared Function ExecuteNonQuery(ByVal ConnectionString As String, _
                                                  ByVal CommandText As String, _
                                                  ByVal ParameterNames As ArrayList, _
                                                  ByVal ParameterValues As ArrayList, _
                                                  ByVal ParameterTypes As ArrayList, _
                                                  ByVal CommandType As CommandType) As Integer
        Dim CommandParameters As OracleParameter()
        Dim cn As OracleConnection
        Try


            'Nếu có giá trị của tham số
            If Not (ParameterValues Is Nothing) And ParameterValues.Count > 0 Then
                cn = New OracleConnection(ConnectionString)
                Dim cmd As New OracleCommand(CommandText)
                For i As Integer = 1 To ParameterNames.Count
                    Dim v_Parameter As OracleParameter
                    cmd.CommandType = CommandType
                    v_Parameter = New OracleParameter(CStr(ParameterNames(i - 1)), ParameterTypes(i - 1))
                    v_Parameter.Direction = ParameterDirection.Input
                    v_Parameter.Value = ParameterValues(i - 1)
                    cmd.Parameters.Add(v_Parameter)
                Next
                cn.Open()
                cmd.Connection = cn
                Return cmd.ExecuteNonQuery()
            Else 'Không có giá trị của tham số
                'Thực hiện command không có tham số
                Return ExecuteNonQuery(ConnectionString, CommandType, CommandText)
            End If
        Catch ex As Exception
            Throw ex
        Finally
            If cn Is Nothing Then
                cn.Dispose()
            End If
        End Try
    End Function
    ' End ChienTD

    Public Overloads Shared Function ExecuteNonQuery(ByVal Connection As OracleConnection, _
                                                     ByVal CommandType As CommandType, _
                                                     ByVal CommandText As String) As Integer
        Return ExecuteNonQuery(Connection, CommandType, CommandText, CType(Nothing, OracleParameter()))
    End Function

    Public Overloads Shared Function ExecuteNonQuery(ByVal Connection As OracleConnection, _
                                                     ByVal StoredProceduredName As String, _
                                                     ByVal ParamArray parameterValues() As Object) As Integer
        Dim CommandParameters As OracleParameter()

        'Nếu có giá trị của tham số
        If Not (parameterValues Is Nothing) And parameterValues.Length > 0 Then
            'Lấy danh sách các tham số của Stored Procedured
            CommandParameters = OracleHelperParameterCache.GetSpParameterSet(Connection.ConnectionString, StoredProceduredName)

            'Gán giá trị cho các tham số
            AssignParameterValues(CommandParameters, parameterValues)

            'Thực hiện command với bộ tham số
            Return ExecuteNonQuery(Connection, CommandType.StoredProcedure, StoredProceduredName, CommandParameters)
        Else 'Không có giá trị của tham số
            'Thực hiện command không có tham số
            Return ExecuteNonQuery(Connection, CommandType.StoredProcedure, StoredProceduredName)
        End If
    End Function

#End Region

#Region " TranExecuteNonQuery "
    Private Sub TranPrepareCommand(ByRef Command As OracleCommand, _
                                     ByVal CommandType As CommandType, _
                                      ByVal CommandText As String, _
                                      ByVal CommandParameters() As OracleParameter)

        'Gán command text (là tên của stored procedure hoặc câu lệnh SQL)
        Command.CommandText = CommandText

        'Gán command type
        Command.CommandType = CommandType

        'Không có timeout
        Command.CommandTimeout = 0 '30 phút

        'Gán connection cho command
        Command.Connection = mv_cn

        'Attach các tham số cho câu lệnh
        If Not (CommandParameters Is Nothing) Then
            AttachParameters(Command, CommandParameters)
        End If

        Return
    End Sub

    Public Function TranExecuteNonQuery(ByVal CommandType As CommandType, _
                                                     ByVal CommandText As String) As Integer
        Return TranExecuteNonQuery(CommandType, CommandText, CType(Nothing, OracleParameter()))
    End Function

    Public Function TranExecuteNonQuery(ByVal CommandType As CommandType, _
                                                     ByVal CommandText As String, _
                                                     ByVal ParamArray CommandParameters() As OracleParameter) As Integer
        'Tạo 1 command và chuẩn bị các tham số
        Dim cmd As New OracleCommand
        Dim retval As Integer

        TranPrepareCommand(cmd, CommandType, CommandText, CommandParameters)

        'Execute the command
        retval = cmd.ExecuteNonQuery
        cmd.Parameters.Clear()

        Return retval
    End Function

    Public Function TranExecuteNonQuery(ByVal StoredProceduredName As String, _
                                                     ByVal ParameterNames() As Object, _
                                                     ByVal ParameterValues() As Object, _
                                                     ByVal ParameterTypes() As Object) As Integer
        Dim CommandParameters As OracleParameter()
        Try


            'Nếu có giá trị của tham số
            If Not (ParameterValues Is Nothing) And ParameterValues.Length > 0 Then
                Dim cmd As New OracleCommand(StoredProceduredName)
                For i As Integer = 0 To ParameterNames.Length - 1
                    Dim v_Parameter As OracleParameter
                    cmd.CommandType = CommandType.StoredProcedure
                    v_Parameter = New OracleParameter(CStr(ParameterNames(i)), ParameterTypes(i))
                    v_Parameter.Direction = ParameterDirection.Input
                    v_Parameter.Value = ParameterValues(i)
                    cmd.Parameters.Add(v_Parameter)
                Next
                cmd.Connection = mv_cn

                Return cmd.ExecuteNonQuery()
            Else 'Không có giá trị của tham số
                'Thực hiện command không có tham số
                Return TranExecuteNonQuery(CommandType.StoredProcedure, StoredProceduredName)
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function TranExecuteNonQuery(ByVal CommantText As String, _
                                                    ByVal ParameterNames() As Object, _
                                                    ByVal ParameterValues() As Object, _
                                                    ByVal ParameterTypes() As Object, _
                                                    ByVal CommandType As CommandType) As Integer
        Try
            'Nếu có giá trị của tham số
            If Not (ParameterValues Is Nothing) And ParameterValues.Length > 0 Then
                Dim cmd As New OracleCommand(CommantText)
                For i As Integer = 0 To ParameterNames.Length - 1
                    Dim v_Parameter As OracleParameter
                    cmd.CommandType = CommandType
                    v_Parameter = New OracleParameter(CStr(ParameterNames(i)), ParameterTypes(i))
                    v_Parameter.Direction = ParameterDirection.Input
                    v_Parameter.Value = ParameterValues(i)
                    cmd.Parameters.Add(v_Parameter)
                Next
                cmd.Connection = mv_cn

                Return cmd.ExecuteNonQuery()
            Else 'Không có giá trị của tham số
                'Thực hiện command không có tham số
                Return TranExecuteNonQuery(CommandType, CommantText)
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function TranExecuteNonQuery(ByVal CommantText As String, _
                                                   ByVal ParameterName As String, _
                                                   ByVal ParameterValue As String, _
                                                   ByVal ParameterType As OracleDbType, _
                                                   ByVal CommandType As CommandType) As Integer
        Try
            'Nếu có giá trị của tham số

            Dim cmd As New OracleCommand(CommantText)

            Dim v_Parameter As OracleParameter
            cmd.CommandType = CommandType
            v_Parameter = New OracleParameter(ParameterName, ParameterType)
            v_Parameter.Direction = ParameterDirection.Input
            v_Parameter.Value = ParameterValue
            cmd.Parameters.Add(v_Parameter)

            cmd.Connection = mv_cn

            Return cmd.ExecuteNonQuery()
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function TranExecuteNonQuery(ByVal CommantText As String, _
                                                    ByVal ParameterNames As ArrayList, _
                                                    ByVal ParameterValues As ArrayList, _
                                                    ByVal ParameterTypes As ArrayList, _
                                                    ByVal CommandType As CommandType) As Integer
        Try


            'Nếu có giá trị của tham số
            If Not (ParameterValues Is Nothing) And ParameterValues.Count > 0 Then
                Dim cmd As New OracleCommand(CommantText)
                For i As Integer = 1 To ParameterNames.Count
                    Dim v_Parameter As OracleParameter
                    cmd.CommandType = CommandType
                    v_Parameter = New OracleParameter(CStr(ParameterNames(i - 1)), CType(ParameterTypes(i - 1), OracleDbType))
                    v_Parameter.Direction = ParameterDirection.Input
                    v_Parameter.Value = ParameterValues(i - 1)
                    cmd.Parameters.Add(v_Parameter)
                Next
                cmd.Connection = mv_cn
                Return cmd.ExecuteNonQuery()
            Else 'Không có giá trị của tham số
                'Thực hiện command không có tham số
                Return TranExecuteNonQuery(CommandType, CommantText)
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function TranExecuteNonQuery(ByVal StoredProceduredName As String, _
                                                     ByVal ParameterNames As ArrayList, _
                                                     ByVal ParameterValues As ArrayList, _
                                                     ByVal ParameterTypes As ArrayList) As Integer
        Dim CommandParameters As OracleParameter()
        Try


            'Nếu có giá trị của tham số
            If Not (ParameterValues Is Nothing) And ParameterValues.Count > 0 Then
                Dim cmd As New OracleCommand(StoredProceduredName)
                For i As Integer = 1 To ParameterNames.Count
                    Dim v_Parameter As OracleParameter
                    cmd.CommandType = CommandType.StoredProcedure
                    v_Parameter = New OracleParameter(CStr(ParameterNames(i - 1)), ParameterTypes(i - 1))
                    v_Parameter.Direction = ParameterDirection.Input
                    v_Parameter.Value = ParameterValues(i - 1)
                    cmd.Parameters.Add(v_Parameter)
                Next
                cmd.Connection = mv_cn
                Return cmd.ExecuteNonQuery()
            Else 'Không có giá trị của tham số
                'Thực hiện command không có tham số
                Return TranExecuteNonQuery(CommandType.StoredProcedure, StoredProceduredName)
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function TranExecuteNonQuery(ByVal StoredProceduredName As String, _
                                                     ByVal ParamArray parameterValues() As Object) As Integer
        Dim CommandParameters As OracleParameter()

        'Nếu có giá trị của tham số
        If Not (parameterValues Is Nothing) And parameterValues.Length > 0 Then
            'Lấy danh sách các tham số của Stored Procedured
            CommandParameters = OracleHelperParameterCache.GetSpParameterSet(mv_strConnectionString, StoredProceduredName)

            'Gán giá trị cho các tham số
            AssignParameterValues(CommandParameters, parameterValues)

            'Thực hiện command với bộ tham số
            Return TranExecuteNonQuery(CommandType.StoredProcedure, StoredProceduredName, CommandParameters)
        Else 'Không có giá trị của tham số
            'Thực hiện command không có tham số
            Return TranExecuteNonQuery(CommandType.StoredProcedure, StoredProceduredName)
        End If
    End Function

#End Region

#Region " ExecuteDataset "
    Public Overloads Shared Function ExecuteDataset(ByVal ConnectionString As String, _
                                                    ByVal CommandType As CommandType, _
                                                    ByVal CommandText As String) As DataSet
        Return ExecuteDataset(ConnectionString, CommandType, CommandText, CType(Nothing, OracleParameter()))
    End Function

    Public Overloads Shared Function ExecuteDataset(ByVal ConnectionString As String, _
                                                    ByVal CommandType As CommandType, _
                                                    ByVal CommandText As String, _
                                                    ByVal ParamArray CommandParameters() As OracleParameter) As DataSet
        'Tạo kết nối đến CSDL, dispose sau khi sử dụng xong
        Dim cn As New OracleConnection(ConnectionString)

        Try
            cn.Open()

            Return ExecuteDataset(cn, CommandType, CommandText, CommandParameters)
        Catch ex As Exception
            Throw ex
        Finally
            cn.Dispose()
        End Try
    End Function

    Public Overloads Shared Function ExecuteDataset(ByVal Connection As OracleConnection, _
                                                    ByVal CommandType As CommandType, _
                                                    ByVal CommandText As String, _
                                                    ByVal ParamArray CommandParameters() As OracleParameter) As DataSet
        Dim cmd As New OracleCommand
        Dim ds As New DataSet
        Dim da As OracleDataAdapter
        Try
            'Tạo command và chuẩn bị các tham số


            PrepareCommand(cmd, Connection, CommandType, CommandText, CommandParameters)

            'Tạo DataAdapter và DataSet
            da = New OracleDataAdapter(cmd)

            'Fill dữ liệu vào DataSet
            da.Fill(ds)

            'Clear các tham số của command
            cmd.Parameters.Clear()

            'Trả v? DataSet
            Return ds
        Catch ex As Exception
            LogError.Write(ex.Message & vbNewLine & CommandText, EventLogEntryType.Error, gc_MODULE_HOST)
            Throw ex
        Finally
            ds.Dispose()
            cmd.Dispose()
            da.Dispose()
        End Try
    End Function

    Public Overloads Shared Function ExecuteDataset(ByVal ConnectionString As String, _
                                                    ByVal StoredProceduredName As String, _
                                                    ByVal ParamArray ParameterValues() As Object) As DataSet
        Dim CommandParameters As OracleParameter()

        'N�ếu có giá trị của tham số
        If Not (ParameterValues Is Nothing) And ParameterValues.Length > 0 Then
            'Lấy danh sách các tham số của Stored Procedured
            CommandParameters = OracleHelperParameterCache.GetSpParameterSet(ConnectionString, StoredProceduredName)

            'Gán giá trị cho các tham số
            AssignParameterValues(CommandParameters, ParameterValues)

            'Thực hiện command với bộ tham số
            Return ExecuteDataset(ConnectionString, CommandType.StoredProcedure, StoredProceduredName, CommandParameters)
        Else 'Không có giá trị của tham số
            'Thực hiện command không có tham số
            Return ExecuteDataset(ConnectionString, CommandType.StoredProcedure, StoredProceduredName)
        End If
    End Function

    Public Overloads Shared Function ExecuteDataset(ByVal Connection As OracleConnection, _
                                                    ByVal CommandType As CommandType, _
                                                    ByVal CommandText As String) As DataSet
        Return ExecuteDataset(Connection, CommandType, CommandText, CType(Nothing, OracleParameter()))
    End Function

    Public Overloads Shared Function ExecuteDataset(ByVal Connection As OracleConnection, _
                                                    ByVal StoredProceduredName As String, _
                                                    ByVal ParamArray ParameterValues() As Object) As DataSet
        Dim CommandParameters As OracleParameter()

        'Nếu có giá trị của tham số
        If Not (ParameterValues Is Nothing) And ParameterValues.Length > 0 Then
            'Lấy danh sách các tham số của Stored Procedured
            CommandParameters = OracleHelperParameterCache.GetSpParameterSet(Connection.ConnectionString, StoredProceduredName)

            'Gán giá trị cho các tham số
            AssignParameterValues(CommandParameters, ParameterValues)

            'Thực hiện command với bộ tham số
            Return ExecuteDataset(Connection, CommandType.StoredProcedure, StoredProceduredName, CommandParameters)
        Else 'Không có giá trị của tham số
            'Thực hiện command không có tham số
            Return ExecuteDataset(Connection, CommandType.StoredProcedure, StoredProceduredName)
        End If
    End Function

    Public Overloads Shared Function SaveUsingOracleBulkCopy(ByVal ConnectionString As String, _
                                                    ByVal pv_strTableName As String, _
                                                    ByVal pv_dt As DataTable) As Boolean
        'Tạo kết nối đến CSDL, dispose sau khi sử dụng xong
        Dim cn As New OracleConnection(ConnectionString)
        Try
            cn.Open()
            Dim bulkCopy As New OracleBulkCopy(cn, OracleBulkCopyOptions.Default)
            bulkCopy.DestinationTableName = pv_strTableName
            bulkCopy.BulkCopyTimeout = 3600
            bulkCopy.WriteToServer(pv_dt)
            bulkCopy.Dispose()
            Return True
        Catch ex As Exception
            Throw ex
        Finally
            cn.Dispose()
        End Try
    End Function
#End Region
    'bangpv : Lấy string từ CLOB data
#Region " TranExecuteString "
    Public Function TranExecuteString(ByVal CommandType As CommandType, _
                                                  ByVal CommandText As String) As String
        Try
            Return TranExecuteString(CommandType, CommandText, CType(Nothing, OracleParameter()))
        Catch ex As Exception
            LogError.Write(ex.Message & vbNewLine & CommandText, EventLogEntryType.Error, gc_MODULE_HOST)
            Throw ex
        End Try
    End Function


    Public Function TranExecuteString(ByVal CommandType As CommandType, _
                                                    ByVal CommandText As String, _
                                                    ByVal ParamArray CommandParameters() As OracleParameter) As String
        Dim cmd As OracleCommand
        Dim v_strOutput As String
        'Dim ds As New DataSet
        Dim dr As OracleDataReader
        Try
            'Tạo command và chuẩn bị các tham số
            TranPrepareCommand(cmd, CommandType, CommandText, CommandParameters)

            dr = cmd.ExecuteReader
            dr.Read()

            Dim Clob As OracleClob = dr.GetOracleClob(0)
            'Lấy dữ liệu string ra 
            v_strOutput = Clob.Value()
            Clob.Close()
            dr.Close()

            'Trả v? DataSet
            Return v_strOutput
        Catch ex As Exception
            Throw ex
        Finally
            GC.Collect()
            GC.GetTotalMemory(False)
            GC.Collect()
        End Try
    End Function

    Public Function TranExecuteString(ByVal StoredProceduredName As String, _
                                                    ByVal ParamArray ParameterValues() As Object) As String
        Dim CommandParameters As OracleParameter()

        'Nếu có giá trị của tham số
        If Not (ParameterValues Is Nothing) And ParameterValues.Length > 0 Then
            'Lấy danh sách các tham số của Stored Procedured
            CommandParameters = OracleHelperParameterCache.GetSpParameterSet(mv_strConnectionString, StoredProceduredName)

            'Gán giá trị cho các tham số
            AssignParameterValues(CommandParameters, ParameterValues)

            'Thực hiện command với bộ tham số
            Return TranExecuteString(CommandType.StoredProcedure, StoredProceduredName, CommandParameters)
        Else 'Không có giá trị của tham số
            'Thực hiện command không có tham số
            Return TranExecuteString(CommandType.StoredProcedure, StoredProceduredName)
        End If
    End Function
#End Region
    'end bangpv 
#Region " TranExecuteDataset "
    Public Function TranExecuteDataset(ByVal CommandType As CommandType, _
                                                    ByVal CommandText As String) As DataSet
        Try
            Return TranExecuteDataset(CommandType, CommandText, CType(Nothing, OracleParameter()))
        Catch ex As Exception
            LogError.Write(ex.Message & vbNewLine & CommandText, EventLogEntryType.Error, gc_MODULE_HOST)
            Throw ex
        End Try
    End Function


    Public Function TranExecuteDataset(ByVal CommandType As CommandType, _
                                                    ByVal CommandText As String, _
                                                    ByVal ParamArray CommandParameters() As OracleParameter) As DataSet
        Dim cmd As New OracleCommand
        Dim ds As New DataSet
        Dim da As OracleDataAdapter
        Try
            'Tạo command và chuẩn bị các tham số
            TranPrepareCommand(cmd, CommandType, CommandText, CommandParameters)

            'Tạo DataAdapter và DataSet
            da = New OracleDataAdapter(cmd)

            'Fill dữ liệu vào DataSet
            da.Fill(ds)

            'Clear các tham số của command
            cmd.Parameters.Clear()

            'Trả v? DataSet
            Return ds
        Catch ex As Exception
            Throw ex
        Finally
            If Not da Is Nothing Then
                da.Dispose()
            End If
            ds.Dispose()
        End Try
    End Function

    Public Function TranExecuteDataset(ByVal StoredProceduredName As String, _
                                                    ByVal ParamArray ParameterValues() As Object) As DataSet
        Dim CommandParameters As OracleParameter()

        'Nếu có giá trị của tham số
        If Not (ParameterValues Is Nothing) And ParameterValues.Length > 0 Then
            'Lấy danh sách các tham số của Stored Procedured
            CommandParameters = OracleHelperParameterCache.GetSpParameterSet(mv_strConnectionString, StoredProceduredName)

            'Gán giá trị cho các tham số
            AssignParameterValues(CommandParameters, ParameterValues)

            'Thực hiện command với bộ tham số
            Return TranExecuteDataset(CommandType.StoredProcedure, StoredProceduredName, CommandParameters)
        Else 'Không có giá trị của tham số
            'Thực hiện command không có tham số
            Return TranExecuteDataset(CommandType.StoredProcedure, StoredProceduredName)
        End If
    End Function
#End Region
#Region " ExecuteString"
    Public Overloads Shared Function ExecuteString(ByVal ConnectionString As String, _
                                                    ByVal CommandType As CommandType, _
                                                    ByVal CommandText As String) As String
        Return ExecuteString(ConnectionString, CommandType, CommandText, CType(Nothing, OracleParameter()))
    End Function

    Public Overloads Shared Function ExecuteString(ByVal ConnectionString As String, _
                                                    ByVal CommandType As CommandType, _
                                                    ByVal CommandText As String, _
                                                    ByVal ParamArray CommandParameters() As OracleParameter) As String
        'Tạo kết nối đến CSDL, dispose sau khi sử dụng xong
        Dim cn As New OracleConnection(ConnectionString)

        Try
            cn.Open()

            Return ExecuteString(cn, CommandType, CommandText, CommandParameters)
        Catch ex As Exception
            Throw ex
        Finally
            cn.Dispose()
        End Try
    End Function

    Public Overloads Shared Function ExecuteString(ByVal Connection As OracleConnection, _
                                                    ByVal CommandType As CommandType, _
                                                    ByVal CommandText As String, _
                                                    ByVal ParamArray CommandParameters() As OracleParameter) As String
        Dim cmd As New OracleCommand
        Dim v_strOutput As String

        Dim dr As OracleDataReader
        Try
            'Tạo command và chuẩn bị các tham số


            PrepareCommand(cmd, Connection, CommandType, CommandText, CommandParameters)

            dr = cmd.ExecuteReader
            dr.Read()

            Dim v_Clob As OracleClob = dr.GetOracleClob(0)
            'Lấy dữ liệu string ra 
            v_strOutput = v_Clob.Value()
            v_Clob.Close()
            dr.Close()

            'Trả v? DataSet
            Return v_strOutput
        Catch ex As Exception
            LogError.Write(ex.Message & vbNewLine & CommandText, EventLogEntryType.Error, gc_MODULE_HOST)
            Throw ex
        Finally
            GC.Collect()
        End Try
    End Function

    Public Overloads Shared Function ExecuteString(ByVal ConnectionString As String, _
                                                    ByVal StoredProceduredName As String, _
                                                    ByVal ParamArray ParameterValues() As Object) As String
        Dim CommandParameters As OracleParameter()

        'N�ếu có giá trị của tham số
        If Not (ParameterValues Is Nothing) And ParameterValues.Length > 0 Then
            'Lấy danh sách các tham số của Stored Procedured
            CommandParameters = OracleHelperParameterCache.GetSpParameterSet(ConnectionString, StoredProceduredName)

            'Gán giá trị cho các tham số
            AssignParameterValues(CommandParameters, ParameterValues)

            'Thực hiện command với bộ tham số
            Return ExecuteString(ConnectionString, CommandType.StoredProcedure, StoredProceduredName, CommandParameters)
        Else 'Không có giá trị của tham số
            'Thực hiện command không có tham số
            Return ExecuteString(ConnectionString, CommandType.StoredProcedure, StoredProceduredName)
        End If
    End Function

    Public Overloads Shared Function ExecuteString(ByVal Connection As OracleConnection, _
                                                    ByVal CommandType As CommandType, _
                                                    ByVal CommandText As String) As String
        Return ExecuteString(Connection, CommandType, CommandText, CType(Nothing, OracleParameter()))
    End Function

    Public Overloads Shared Function ExecuteString(ByVal Connection As OracleConnection, _
                                                    ByVal StoredProceduredName As String, _
                                                    ByVal ParamArray ParameterValues() As Object) As String
        Dim CommandParameters As OracleParameter()

        'Nếu có giá trị của tham số
        If Not (ParameterValues Is Nothing) And ParameterValues.Length > 0 Then
            'Lấy danh sách các tham số của Stored Procedured
            CommandParameters = OracleHelperParameterCache.GetSpParameterSet(Connection.ConnectionString, StoredProceduredName)

            'Gán giá trị cho các tham số
            AssignParameterValues(CommandParameters, ParameterValues)

            'Thực hiện command với bộ tham số
            Return ExecuteString(Connection, CommandType.StoredProcedure, StoredProceduredName, CommandParameters)
        Else 'Không có giá trị của tham số
            'Thực hiện command không có tham số
            Return ExecuteString(Connection, CommandType.StoredProcedure, StoredProceduredName)
        End If
    End Function
#End Region
#Region " ExecuteReader "
    Private Enum OracleConnectionOwnership
        'Connection is owned and managed by OracleHelper
        Internal
        'Connection is owned and managed by the caller
        [External]
    End Enum

    Private Overloads Shared Function ExecuteReader(ByVal Connection As OracleConnection, _
                                                    ByVal CommandType As CommandType, _
                                                    ByVal CommandText As String, _
                                                    ByVal CommandParameters() As OracleParameter, _
                                                    ByVal ConnectionOwnerShip As OracleConnectionOwnership) As OracleDataReader
        'Tạo command
        Dim cmd As New OracleCommand
        'Khai báo Reader
        Dim dr As OracleDataReader

        PrepareCommand(cmd, Connection, CommandType, CommandText, CommandParameters)

        'ExecuteReader with the appropriate CommandBehavior
        If ConnectionOwnerShip = OracleConnectionOwnership.External Then
            dr = cmd.ExecuteReader()
        Else
            dr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
        End If

        'Clear tham số của command
        cmd.Parameters.Clear()

        Return dr
    End Function

    Public Overloads Shared Function ExecuteReader(ByVal ConnectionString As String, _
                                                   ByVal CommandType As CommandType, _
                                                   ByVal CommandText As String) As OracleDataReader
        Return ExecuteReader(ConnectionString, CommandType, CommandText, CType(Nothing, OracleParameter()))
    End Function

    Public Overloads Shared Function ExecuteReader(ByVal ConnectionString As String, _
                                                   ByVal CommandType As CommandType, _
                                                   ByVal CommandText As String, _
                                                   ByVal ParamArray CommandParameters() As OracleParameter) As OracleDataReader
        'Tạo kết nối đến CSDL, dispose khi sử dụng xong
        Dim cn As New OracleConnection(ConnectionString)

        Try
            cn.Open()

            Return ExecuteReader(cn, CommandType, CommandText, CommandParameters, OracleConnectionOwnership.Internal)
        Catch ex As Exception
            Throw ex
        Finally
            cn.Dispose()
        End Try
    End Function

    Public Overloads Shared Function ExecuteReader(ByVal ConnectionString As String, _
                                                   ByVal StoredProceduredName As String, _
                                                   ByVal ParamArray ParameterValues() As Object) As OracleDataReader
        Dim CommandParameters As OracleParameter()

        'Nếu có giá trị của tham số
        If Not (ParameterValues Is Nothing) And ParameterValues.Length > 0 Then
            'Lấy danh sách các tham số của Stored Procedured
            CommandParameters = OracleHelperParameterCache.GetSpParameterSet(ConnectionString, StoredProceduredName)

            'Gán giá trị cho các tham số
            AssignParameterValues(CommandParameters, ParameterValues)

            'Thực hiện command với bộ tham số
            Return ExecuteReader(ConnectionString, CommandType.StoredProcedure, StoredProceduredName, CommandParameters)
        Else 'Không có giá trị của tham số
            'Thực hiện command không có tham số
            Return ExecuteReader(ConnectionString, CommandType.StoredProcedure, StoredProceduredName)
        End If
    End Function

    Public Overloads Shared Function ExecuteReader(ByVal Connection As OracleConnection, _
                                                   ByVal CommandType As CommandType, _
                                                   ByVal CommandText As String) As OracleDataReader
        Return ExecuteReader(Connection, CommandType, CommandText, CType(Nothing, OracleParameter))
    End Function

    Public Overloads Shared Function ExecuteReader(ByVal Connection As OracleConnection, _
                                                   ByVal CommandType As CommandType, _
                                                   ByVal CommandText As String, _
                                                   ByVal ParamArray CommandParameters() As OracleParameter) As OracleDataReader
        Return ExecuteReader(Connection, CommandType, CommandText, CommandParameters, OracleConnectionOwnership.External)
    End Function

    Public Overloads Shared Function ExecuteReader(ByVal Connection As OracleConnection, _
                                                   ByVal StoredProceduredName As String, _
                                                   ByVal ParamArray ParameterValues() As Object) As OracleDataReader
        Dim CommandParameters As OracleParameter()

        'Nếu có giá trị của tham số
        If Not (ParameterValues Is Nothing) And ParameterValues.Length > 0 Then
            'Lấy danh sách các tham số của Stored Procedured
            CommandParameters = OracleHelperParameterCache.GetSpParameterSet(Connection.ConnectionString, StoredProceduredName)

            'Gán giá trị cho các tham số
            AssignParameterValues(CommandParameters, ParameterValues)

            'Thực hiện command với bộ tham số
            Return ExecuteReader(Connection, CommandType.StoredProcedure, StoredProceduredName, CommandParameters)
        Else 'Không có giá trị của tham số
            'Thực hiện command không có tham số
            Return ExecuteReader(Connection, CommandType.StoredProcedure, StoredProceduredName)
        End If
    End Function
#End Region

#Region " ExecuteScalar "
    Public Overloads Shared Function ExecuteScalar(ByVal ConnectionString As String, _
                                                   ByVal CommandType As CommandType, _
                                                   ByVal CommandText As String) As Object
        Return ExecuteScalar(ConnectionString, CommandType, CommandText, CType(Nothing, OracleParameter()))
    End Function

    Public Overloads Shared Function ExecuteScalar(ByVal ConnectionString As String, _
                                                   ByVal CommandType As CommandType, _
                                                   ByVal CommandText As String, _
                                                   ByVal ParamArray CommandParameters() As OracleParameter) As Object
        'Tạo kết nối đến CSDL, dispose sau khi sử dụng xong
        Dim cn As New OracleConnection(ConnectionString)

        Try
            cn.Open()

            Return ExecuteScalar(cn, CommandType, CommandText, CommandParameters)
        Catch ex As Exception
            Throw ex
        Finally
            cn.Dispose()
        End Try
    End Function

    Public Overloads Shared Function ExecuteScalar(ByVal ConnectionString As String, _
                                                   ByVal StoredProceduredName As String, _
                                                   ByVal ParamArray ParameterValues() As Object) As Object
        Dim CommandParameters As OracleParameter()

        'Nếu có giá trị của tham số
        If Not (ParameterValues Is Nothing) And ParameterValues.Length > 0 Then
            'Lấy danh sách các tham số của Stored Procedured
            CommandParameters = OracleHelperParameterCache.GetSpParameterSet(ConnectionString, StoredProceduredName)

            'Gán giá trị cho các tham số
            AssignParameterValues(CommandParameters, ParameterValues)

            'Thực hiện command với bộ tham số
            Return ExecuteScalar(ConnectionString, CommandType.StoredProcedure, StoredProceduredName, CommandParameters)
        Else 'Không có giá trị của tham số
            'Thực hiện command không có tham số
            Return ExecuteScalar(ConnectionString, CommandType.StoredProcedure, StoredProceduredName)
        End If
    End Function

    Public Overloads Shared Function ExecuteScalar(ByVal Connection As OracleConnection, _
                                                   ByVal CommandType As CommandType, _
                                                   ByVal CommandText As String) As Object
        Return ExecuteScalar(Connection, CommandType, CommandText, CType(Nothing, OracleParameter()))
    End Function

    Public Overloads Shared Function ExecuteScalar(ByVal Connection As OracleConnection, _
                                                   ByVal CommandType As CommandType, _
                                                   ByVal CommandText As String, _
                                                   ByVal ParamArray CommandParameters() As OracleParameter) As Object
        'Tạo command
        Dim cmd As New OracleCommand
        Dim retval As Object

        PrepareCommand(cmd, Connection, CommandType, CommandText, CommandParameters)

        retval = cmd.ExecuteScalar()

        'Clear tham số của command
        cmd.Parameters.Clear()

        Return retval
    End Function

    Public Overloads Shared Function ExecuteScalar(ByVal Connection As OracleConnection, _
                                                   ByVal StoredProceduredName As String, _
                                                   ByVal ParamArray ParameterValues() As Object) As Object
        Dim CommandParameters As OracleParameter()

        'Nếu có giá trị của tham số
        If Not (ParameterValues Is Nothing) And ParameterValues.Length > 0 Then
            'Lấy danh sách các tham số của Stored Procedured
            CommandParameters = OracleHelperParameterCache.GetSpParameterSet(Connection.ConnectionString, StoredProceduredName)

            'Gán giá trị cho các tham số
            AssignParameterValues(CommandParameters, ParameterValues)

            'Thực hiện command với bộ tham số
            Return ExecuteScalar(Connection, CommandType.StoredProcedure, StoredProceduredName, CommandParameters)
        Else 'Không có giá trị của tham số
            'Thực hiện command không có tham số
            Return ExecuteScalar(Connection, CommandType.StoredProcedure, StoredProceduredName)
        End If
    End Function
#End Region

#Region " ExecuteXmlReader "
    Public Overloads Shared Function ExecuteXmlReader(ByVal Connection As OracleConnection, _
                                                      ByVal CommandType As CommandType, _
                                                      ByVal CommandText As String) As XmlReader
        Return ExecuteXmlReader(Connection, CommandType, CommandText, CType(Nothing, OracleParameter()))
    End Function

    Public Overloads Shared Function ExecuteXmlReader(ByVal Connection As OracleConnection, _
                                                      ByVal CommandType As CommandType, _
                                                      ByVal CommandText As String, _
                                                      ByVal ParamArray CommandParameters() As OracleParameter) As XmlReader
        'Tạo command
        Dim cmd As New OracleCommand
        Dim retval As XmlReader

        PrepareCommand(cmd, Connection, CommandType, CommandText, CommandParameters)

        retval = cmd.ExecuteXmlReader()

        'Clear tham số của command
        cmd.Parameters.Clear()

        Return retval
    End Function

    Public Overloads Shared Function ExecuteXmlReader(ByVal Connection As OracleConnection, _
                                                      ByVal StoredProceduredName As String, _
                                                      ByVal ParamArray ParameterValues() As Object) As XmlReader
        Dim CommandParameters As OracleParameter()

        'Nếu có giá trị của tham số
        If Not (ParameterValues Is Nothing) And ParameterValues.Length > 0 Then
            'Lấy danh sách các tham số của Stored Procedured
            CommandParameters = OracleHelperParameterCache.GetSpParameterSet(Connection.ConnectionString, StoredProceduredName)

            'Gán giá trị cho các tham số
            AssignParameterValues(CommandParameters, ParameterValues)

            'Thực hiện command với bộ tham số
            Return ExecuteXmlReader(Connection, CommandType.StoredProcedure, StoredProceduredName, CommandParameters)
        Else 'Không có giá trị của tham số
            'Thực hiện command không có tham số
            Return ExecuteXmlReader(Connection, CommandType.StoredProcedure, StoredProceduredName)
        End If
    End Function
#End Region

#Region "Transaction"
    Public Sub BeginTran(ByVal pv_strConnectionString As String)
        mv_strConnectionString = pv_strConnectionString
        mv_cn = New OracleConnection(mv_strConnectionString)
        mv_cn.Open()
        mv_txn = mv_cn.BeginTransaction()
    End Sub

    Public Sub Rollback()
        If Not IsDBNull(mv_txn) Then
            mv_txn.Rollback()
            mv_txn.Dispose()
            mv_cn.Close()
            mv_cn.Dispose()
        End If
    End Sub
    Public Sub Commit()
        If Not IsDBNull(mv_txn) Then
            mv_txn.Commit()
            mv_txn.Dispose()
            mv_cn.Close()
            mv_cn.Dispose()
        End If
    End Sub
#End Region

End Class

Public Class OracleHelperParameterCache

#Region " Các private method, khai báo biến và hàm constructor "
    Private Sub New()
        'Không làm gì cả
    End Sub

    Private Shared paramCache As Hashtable = Hashtable.Synchronized(New Hashtable)

    Private Shared Function DiscoverSpParameterSet(ByVal ConnectionString As String, _
                                                   ByVal StoredProceduredName As String, _
                                                   ByVal IncludeReturnValueParameter As Boolean, _
                                                   ByVal ParamArray ParameterValues() As Object) As OracleParameter()
        'Dim cn As New OracleConnection(ConnectionString)
        'Dim cmd As OracleCommand = New OracleCommand(StoredProceduredName, cn)
        'Dim DiscoveredParameters() As OracleParameter

        'Try
        '    cn.Open()
        '    cmd.CommandType = CommandType.StoredProcedure
        '    OracleCommandBuilder.DeriveParameters(cmd)
        '    If Not IncludeReturnValueParameter Then
        '        cmd.Parameters.RemoveAt(0)
        '    End If

        '    DiscoveredParameters = New OracleParameter(cmd.Parameters.Count - 1) {}
        '    cmd.Parameters.CopyTo(DiscoveredParameters, 0)
        'Catch ex As Exception
        '    Throw ex
        'Finally
        '    cmd.Dispose()
        '    cn.Dispose()
        'End Try

        'Return DiscoveredParameters
    End Function

    Private Shared Function CloneParameters(ByVal OriginalParameters() As OracleParameter) As OracleParameter()
        Dim i As Short
        Dim j As Short = OriginalParameters.Length - 1
        Dim ClonedParameters(j) As OracleParameter

        For i = 0 To j
            ClonedParameters(i) = CType(CType(OriginalParameters(i), ICloneable).Clone, OracleParameter)
        Next

        Return ClonedParameters
    End Function
#End Region

#Region " Caching functions "
    Public Shared Sub CacheParameterSet(ByVal ConnectionString As String, _
                                        ByVal CommandText As String, _
                                        ByVal ParamArray CommandParameters() As OracleParameter)
        Dim hashKey As String = ConnectionString + ":" + CommandText

        paramCache(hashKey) = CommandParameters
    End Sub

    Public Shared Function GetCachedParameterSet(ByVal ConnectionString As String, ByVal CommandText As String) As OracleParameter()
        Dim hashKey As String = ConnectionString + ":" + CommandText
        Dim CachedParameters As OracleParameter() = CType(paramCache(hashKey), OracleParameter())

        If CachedParameters Is Nothing Then
            Return Nothing
        Else
            Return CloneParameters(CachedParameters)
        End If
    End Function
#End Region

#Region " Parameter Discovery Functions "
    Public Overloads Shared Function GetSpParameterSet(ByVal ConnectionString As String, ByVal StoredProceduredName As String) As OracleParameter()
        Return GetSpParameterSet(ConnectionString, StoredProceduredName, False)
    End Function

    Public Overloads Shared Function GetSpParameterSet(ByVal ConnectionString As String, _
                                                       ByVal StoredProceduredName As String, _
                                                       ByVal IncludeReturnValueParameter As Boolean) As OracleParameter()
        Dim CachedParameters() As OracleParameter
        Dim hashKey As String

        hashKey = ConnectionString + ":" + StoredProceduredName + IIf(IncludeReturnValueParameter = True, ":include ReturnValue Parameter", "")

        CachedParameters = CType(paramCache(hashKey), OracleParameter())

        If (CachedParameters Is Nothing) Then
            paramCache(hashKey) = DiscoverSpParameterSet(ConnectionString, StoredProceduredName, IncludeReturnValueParameter)
            CachedParameters = CType(paramCache(hashKey), OracleParameter())
        End If

        Return CloneParameters(CachedParameters)
    End Function
#End Region

End Class
