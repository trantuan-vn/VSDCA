Imports System
Imports System.Data
Imports System.Xml
Imports Oracle.DataAccess.Client
Imports Oracle.DataAccess.Types
Imports Microsoft.Win32
Imports System.Configuration
Imports System.IO
'Imports System.EnterpriseServices
Imports Sats.CommonLibrary

Public Enum DBProvider
    OracleClient = 0
    SQLClient = 1
    ODBC = 2
    OLEDB = 3
    Access = 4
End Enum

'<Transaction(TransactionOption.Disabled), _
'ObjectPooling(Enabled:=True, MinPoolSize:=4, MaxPoolSize:=10000)> _
Public Class DataAccess
    Implements IDisposable
    'Inherits ServicedComponent

    Const c_LogCommandFilePath = "LogCommandFilePath"
    Private mv_strModule As String = gc_MODULE_HOST
    Private mv_strConnectionString As String
    Private mv_dbProvider As DBProvider
    Private mv_blnLogCommand As Boolean = False
    Private mv_strLogFileName As String
    Private mv_blnIsTransaction As Boolean = False
    Private mv_OracleHelper As OracleHelper
    Private mv_strModuleHost As String = ""
    Private mv_strModuleInquery As String = ""

#Region " Propertices"
    Public Property ModuleHost() As String
        Get
            Return mv_strModuleHost
        End Get
        Set(ByVal value As String)
            mv_strModuleHost = value
        End Set
    End Property
    Public Property ModuleInquery() As String
        Get
            Return mv_strModuleInquery
        End Get
        Set(ByVal value As String)
            mv_strModuleInquery = value
        End Set
    End Property
#End Region

#Region " Hàm constructor "

    Public Sub New()
        Try
            'If ConfigurationManager.AppSettings(gc_MODULE_HOST & ".DSN") <> Nothing Then
            '    mv_strModuleHost = ConfigurationManager.AppSettings(gc_MODULE_HOST & ".DSN").ToString().Trim()
            'End If
            'If ConfigurationManager.AppSettings(gc_MODULE_INQUERY & ".DSN") <> Nothing Then
            '    mv_strModuleInquery = ConfigurationManager.AppSettings(gc_MODULE_INQUERY & ".DSN").ToString().Trim()
            'End If
            '_InitClass(mv_strModule)
            _InitClassFromDLL(gc_MODULE_HOST)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub NewDBInstance(ByVal pv_strModule As String)
        '_InitClass(pv_strModule)
        _InitClassFromDLL(pv_strModule)
    End Sub

    Private Sub _InitClass(ByVal pv_strModule As String)
        Dim v_strDataSource, v_strUsername, v_strPassword, v_strDBInit As String
        Dim v_strMaxPoolSize, v_strMinPoolSize, v_strConnectionLifetime, v_strConnectionTimeout As String
        Dim v_strIncrPoolSize, v_strDecrPoolSize As String
        Try
            If Not (pv_strModule Is Nothing) Then
                If ConfigurationManager.AppSettings(pv_strModule & ".PRV") <> Nothing Then
                    ProvideType = CType(ConfigurationManager.AppSettings(pv_strModule & ".PRV").ToString().Trim(), DBProvider)
                End If
                If ConfigurationManager.AppSettings(pv_strModule & ".DSN") <> Nothing Then
                    v_strDataSource = ConfigurationManager.AppSettings(pv_strModule & ".DSN").ToString().Trim()
                Else
                    v_strDataSource = String.Empty
                End If
                If ConfigurationManager.AppSettings(pv_strModule & ".UID") <> Nothing Then
                    v_strUsername = ConfigurationManager.AppSettings(pv_strModule & ".UID").ToString().Trim()
                Else
                    v_strUsername = String.Empty
                End If
                If ConfigurationManager.AppSettings(pv_strModule & ".PWD") <> Nothing Then
                    v_strPassword = ConfigurationManager.AppSettings(pv_strModule & ".PWD").ToString().Trim()
                Else
                    v_strPassword = String.Empty
                End If
                If ConfigurationManager.AppSettings(pv_strModule & ".IDB") <> Nothing Then
                    v_strDBInit = ConfigurationManager.AppSettings(pv_strModule & ".IDB").ToString
                Else
                    v_strDBInit = String.Empty
                End If

                If ConfigurationManager.AppSettings(pv_strModule & ".PWD") <> Nothing Then
                    v_strPassword = ConfigurationManager.AppSettings(pv_strModule & ".PWD").ToString().Trim()
                Else
                    v_strPassword = String.Empty
                End If

                If ConfigurationManager.AppSettings(pv_strModule & ".MAX") <> Nothing Then
                    v_strMaxPoolSize = ConfigurationManager.AppSettings(pv_strModule & ".MAX").ToString().Trim()
                Else
                    v_strMaxPoolSize = String.Empty
                End If

                If ConfigurationManager.AppSettings(pv_strModule & ".MIN") <> Nothing Then
                    v_strMinPoolSize = ConfigurationManager.AppSettings(pv_strModule & ".MIN").ToString().Trim()
                Else
                    v_strMinPoolSize = String.Empty
                End If

                If ConfigurationManager.AppSettings(pv_strModule & ".TOUT") <> Nothing Then
                    v_strConnectionTimeout = ConfigurationManager.AppSettings(pv_strModule & ".TOUT").ToString().Trim()
                Else
                    v_strConnectionTimeout = String.Empty
                End If

                If ConfigurationManager.AppSettings(pv_strModule & ".TLIFE") <> Nothing Then
                    v_strConnectionLifetime = ConfigurationManager.AppSettings(pv_strModule & ".TLIFE").ToString().Trim()
                Else
                    v_strConnectionLifetime = String.Empty
                End If

                If ConfigurationManager.AppSettings(pv_strModule & ".INC") <> Nothing Then
                    v_strIncrPoolSize = ConfigurationManager.AppSettings(pv_strModule & ".INC").ToString().Trim()
                Else
                    v_strIncrPoolSize = String.Empty
                End If

                If ConfigurationManager.AppSettings(pv_strModule & ".DEC") <> Nothing Then
                    v_strDecrPoolSize = ConfigurationManager.AppSettings(pv_strModule & ".DEC").ToString().Trim()
                Else
                    v_strDecrPoolSize = String.Empty
                End If
            Else
                ProvideType = DBProvider.OracleClient   ' Oracle by default
                v_strDataSource = String.Empty
                v_strUsername = String.Empty
                v_strPassword = String.Empty
                v_strDBInit = String.Empty
            End If

            'mv_strConnectionString = _BuildConnectionString(v_strDataSource, v_strUsername, v_strPassword, v_strDBInit)
            mv_strConnectionString = _BuildConnectionString(v_strDataSource, v_strUsername, v_strPassword, _
                                                           v_strMinPoolSize, v_strMaxPoolSize, v_strConnectionLifetime, _
                                                           v_strConnectionTimeout, v_strIncrPoolSize, v_strDecrPoolSize, v_strDBInit)
            mv_strLogFileName = _BuildLogFileName()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub _InitClassFromDLL(ByVal pv_strModule As String)
        Dim v_strDataSource, v_strUsername, v_strPassword, v_strDBInit As String
        Dim v_strMaxPoolSize, v_strMinPoolSize, v_strConnectionLifetime, v_strConnectionTimeout As String
        Dim v_strIncrPoolSize, v_strDecrPoolSize As String
        Try
            If Not (pv_strModule Is Nothing) Then
                If pv_strModule = gc_MODULE_HOST Then
                    v_strDataSource = DecryptString(gc_ENCRYPT_PASSWORD, GlobalDataConfig.HOST_DBCONFIG.DataSource)
                    v_strUsername = DecryptString(gc_ENCRYPT_PASSWORD, GlobalDataConfig.HOST_DBCONFIG.UserName)
                    v_strPassword = DecryptString(gc_ENCRYPT_PASSWORD, GlobalDataConfig.HOST_DBCONFIG.Password)

                    v_strDBInit = String.Empty
                    'v_strMaxPoolSize = String.Empty
                    v_strMaxPoolSize = "400"
                    v_strMinPoolSize = String.Empty
                    v_strConnectionLifetime = String.Empty
                    v_strConnectionTimeout = String.Empty
                ElseIf pv_strModule = gc_MODULE_INQUERY Then
                    v_strDataSource = DecryptString(gc_ENCRYPT_PASSWORD, GlobalDataConfig.INQUERY_DBCONFIG.DataSource)
                    v_strUsername = DecryptString(gc_ENCRYPT_PASSWORD, GlobalDataConfig.INQUERY_DBCONFIG.UserName)
                    v_strPassword = DecryptString(gc_ENCRYPT_PASSWORD, GlobalDataConfig.INQUERY_DBCONFIG.Password)

                    v_strDBInit = String.Empty
                    'v_strMaxPoolSize = String.Empty
                    v_strMaxPoolSize = "400"
                    v_strMinPoolSize = String.Empty
                    v_strConnectionLifetime = String.Empty
                    v_strConnectionTimeout = String.Empty
                End If
            Else
                ProvideType = DBProvider.OracleClient   ' Oracle by default
                v_strDataSource = String.Empty
                v_strUsername = String.Empty
                v_strPassword = String.Empty
                v_strDBInit = String.Empty
            End If

            mv_strConnectionString = _BuildConnectionString(v_strDataSource, v_strUsername, v_strPassword, _
                                                           v_strMinPoolSize, v_strMaxPoolSize, v_strConnectionLifetime, _
                                                           v_strConnectionTimeout, v_strIncrPoolSize, v_strDecrPoolSize, v_strDBInit)

            'LogError.Write("ConnectionString =  " & ConnectionString, EventLogEntryType.Error, gc_MODULE_HOST)
            mv_strLogFileName = _BuildLogFileName()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region

#Region " Thuộc tính chỉ đọc ConnectionString "
    Public ReadOnly Property ConnectionString() As String
        Get
            Return mv_strConnectionString
        End Get
    End Property
#End Region

#Region " Các thuộc tính khác "
    Public Property strModule() As String
        Get
            Return mv_strModule
        End Get
        Set(ByVal Value As String)
            mv_strModule = Value
        End Set
    End Property

    Public Property ProvideType() As DBProvider
        Get
            Return mv_dbProvider
        End Get
        Set(ByVal Value As DBProvider)
            mv_dbProvider = Value
        End Set
    End Property

    Public Property LogCommand() As Boolean
        Get
            Return mv_blnLogCommand
        End Get
        Set(ByVal Value As Boolean)
            mv_blnLogCommand = Value
        End Set
    End Property
#End Region

#Region " Helper "
    Private Function _BuildConnectionString(ByVal pv_strDataSource As String, _
                                            ByVal pv_strUserName As String, _
                                            ByVal pv_strPassword As String, _
                                            ByVal pv_strMinPoolSize As String, _
                                            ByVal pv_strMaxPoolSize As String, _
                                            ByVal pv_strConnectionLifetime As String, _
                                            ByVal pv_strConnectionTimeout As String, _
                                            ByVal pv_strIncrPoolSize As String, _
                                            ByVal pv_strDecrPoolSize As String, _
                                            Optional ByVal pv_strDBInit As String = "") As String
        Dim v_strConnectString As String = String.Empty

        Select Case ProvideType
            Case DBProvider.OracleClient
                If Not (pv_strDataSource = String.Empty) Then
                    v_strConnectString &= "Data Source=" & pv_strDataSource
                End If
                If Not (pv_strUserName = String.Empty) Then
                    v_strConnectString &= ";User ID=" & pv_strUserName
                End If
                If Not (pv_strPassword = String.Empty) Then
                    v_strConnectString &= ";Password=" & pv_strPassword
                End If
                If Not (pv_strMinPoolSize = String.Empty) Then
                    v_strConnectString &= ";Min Pool Size=" & pv_strMinPoolSize
                End If
                If Not (pv_strMaxPoolSize = String.Empty) Then
                    v_strConnectString &= ";Max Pool Size=" & pv_strMaxPoolSize
                End If
                If Not (pv_strConnectionLifetime = String.Empty) Then
                    v_strConnectString &= ";Connection Lifetime=" & pv_strConnectionLifetime
                End If
                If Not (pv_strConnectionTimeout = String.Empty) Then
                    v_strConnectString &= ";Connection Timeout=" & pv_strConnectionTimeout
                End If
                If Not (pv_strIncrPoolSize = String.Empty) Then
                    v_strConnectString &= ";Incr Pool Size=" & pv_strIncrPoolSize
                End If
                If Not (pv_strDecrPoolSize = String.Empty) Then
                    v_strConnectString &= ";Decr Pool Size=" & pv_strDecrPoolSize
                End If
            Case DBProvider.SQLClient

            Case DBProvider.ODBC

            Case DBProvider.OLEDB

            Case DBProvider.Access

        End Select

        Return v_strConnectString
    End Function

    Private Function _BuildLogFileName() As String
        Dim v_strFileName As String

        If ConfigurationManager.AppSettings(c_LogCommandFilePath) <> Nothing Then
            v_strFileName = ConfigurationManager.AppSettings(c_LogCommandFilePath).ToString().Trim()
        Else
            v_strFileName = "C:"
        End If

        v_strFileName &= "\" & Now.ToString("ddMMMyyyy") & ".log"

        Return v_strFileName
    End Function

    Private Sub LogBusinessCommand(ByVal pv_strFileName As String, ByVal pv_bCommand As BusinessCommand)
        Dim v_streamWriter As New StreamWriter(pv_strFileName, True)

        With pv_bCommand
            v_streamWriter.WriteLine()
            v_streamWriter.WriteLine(.ExecuteUser & " execute @ " & .ExecuteDate.ToString("dd/MM/yyyy") & " " & .ExecuteTime)
            v_streamWriter.WriteLine(.SQLCommand)
        End With

        v_streamWriter.Flush()
        v_streamWriter.Close()
    End Sub
#End Region

#Region " Các public method "
    Public Function ExecuteSQLReturnDataset(ByVal pv_bCommand As BusinessCommand) As DataSet
        Try
            If LogCommand() Then
                'LogBusinessCommand(mv_strLogFileName, pv_bCommand)
            End If
            If mv_blnIsTransaction Then
                Return mv_OracleHelper.TranExecuteDataset(CommandType.Text, pv_bCommand.SQLCommand)
            Else
                Return OracleHelper.ExecuteDataset(ConnectionString(), CommandType.Text, pv_bCommand.SQLCommand)
            End If
        Catch ex As Exception
            LogError.Write(ex.Message & vbNewLine & CommandType.Text, EventLogEntryType.Error, gc_MODULE_HOST)
            Throw ex
        End Try
    End Function
    'bangpv
    Public Function ExecuteSQLReturnString(ByVal CommandType As CommandType, ByVal CommandText As String) As String
        Try
            If mv_blnIsTransaction Then
                Return mv_OracleHelper.TranExecuteString(CommandType, CommandText)
            Else
                Return mv_OracleHelper.ExecuteString(ConnectionString(), CommandType, CommandText)
            End If
        Catch ex As Exception
            LogError.Write(ex.Message & vbNewLine & CommandText, EventLogEntryType.Error, gc_MODULE_HOST)
            Throw ex
        End Try
    End Function
    'end bangpv

    Public Function ExecuteSQLReturnDataset(ByVal CommandType As CommandType, ByVal CommandText As String) As DataSet
        Try
            If mv_blnIsTransaction Then
                Return mv_OracleHelper.TranExecuteDataset(CommandType, CommandText)
            Else
                Return OracleHelper.ExecuteDataset(ConnectionString, CommandType, CommandText)
            End If
        Catch ex As Exception
            LogError.Write(ex.Message & vbNewLine & CommandText, EventLogEntryType.Error, gc_MODULE_HOST)
            Throw ex
        End Try
    End Function

    'Public Function ExecuteStoredReturnDataset(ByVal pv_strStoredName As String, ByVal pv_rptParameters() As ReportParameters) As DataSet
    '    Dim v_arrCommandParameters() As OracleParameter
    '    'Dim v_intParamCount As Integer
    '    Dim v_cmdParam As OracleParameter
    '    Dim i As Integer

    '    Try
    '        ReDim v_arrCommandParameters(pv_rptParameters.Length)

    '        v_cmdParam = New OracleParameter("p_ref_cursor", OracleDbType.RefCursor)
    '        v_cmdParam.Direction = ParameterDirection.InputOutput
    '        v_cmdParam.Value = Nothing
    '        v_arrCommandParameters(0) = v_cmdParam

    '        For i = 0 To pv_rptParameters.Length - 1
    '            If (pv_rptParameters(i).ParamName <> String.Empty) Then
    '                Select Case pv_rptParameters(i).ParamType
    '                    Case GetType(Double).Name
    '                        v_cmdParam = New OracleParameter(pv_rptParameters(i).ParamName, OracleDbType.Double, pv_rptParameters(i).ParamSize)
    '                    Case GetType(System.DateTime).Name
    '                        v_cmdParam = New OracleParameter(pv_rptParameters(i).ParamName, OracleDbType.Date, pv_rptParameters(i).ParamSize)
    '                    Case GetType(System.String).Name
    '                        v_cmdParam = New OracleParameter(pv_rptParameters(i).ParamName, OracleDbType.Varchar2, pv_rptParameters(i).ParamSize)
    '                    Case Else
    '                        v_cmdParam = New OracleParameter(pv_rptParameters(i).ParamName, OracleDbType.Varchar2, pv_rptParameters(i).ParamSize)
    '                End Select
    '                v_cmdParam.Direction = ParameterDirection.Input
    '                v_cmdParam.Value = pv_rptParameters(i).ParamValue

    '                v_arrCommandParameters(i + 1) = v_cmdParam
    '            End If
    '        Next
    '        ReDim Preserve v_arrCommandParameters(pv_rptParameters.Length)

    '        Return OracleHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, pv_strStoredName, v_arrCommandParameters)
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Function

    Public Function GetSysVar(ByVal pv_strGRNAME As String, ByVal pv_strVARNAME As String, ByRef pv_strVARVALUE As String) As Long
        Dim v_strSQL As String = ""
        Dim v_ds As DataSet

        Try
            v_strSQL = "SELECT VARVALUE, VARDESC, EN_VARDESC FROM SYSVAR WHERE TRIM(GRNAME) = '" _
                & pv_strGRNAME & "' AND TRIM(VARNAME)='" & pv_strVARNAME & "'"
            v_ds = OracleHelper.ExecuteDataset(ConnectionString, CommandType.Text, v_strSQL)
            If v_ds.Tables(0).Rows.Count = 0 Then
                Return ERR_SY_NO_DATAFOUND
            Else
                pv_strVARVALUE = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(0)("VARVALUE")))
            End If
            Return ERR_SYSTEM_OK
        Catch ex As Exception
            'LogError.Write(v_strSQL & ControlChars.CrLf & ex.Message & vbNewLine & ex.StackTrace, EventLogEntryType.Error)
            Throw ex
        End Try
    End Function

    Public Function GetSysVar(ByVal pv_strGRNAME As String, ByVal pv_strVARNAME As String, ByVal pv_strBRID As String, ByRef pv_strVARVALUE As String) As Long
        Dim v_strSQL As String = ""
        Dim v_ds As DataSet

        Try
            v_strSQL = "SELECT VARVALUE, VARDESC, EN_VARDESC FROM SYSVAR WHERE TRIM(GRNAME) = '" & pv_strGRNAME & "' AND TRIM(VARNAME)='" & pv_strVARNAME & "' and BRID = '" & pv_strBRID & "'"
            v_ds = OracleHelper.ExecuteDataset(ConnectionString, CommandType.Text, v_strSQL)
            If v_ds.Tables(0).Rows.Count = 0 Then
                Return ERR_SY_NO_DATAFOUND
            Else
                pv_strVARVALUE = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(0)("VARVALUE")))
            End If
            Return ERR_SYSTEM_OK
        Catch ex As Exception
            'LogError.Write(v_strSQL & ControlChars.CrLf & ex.Message & vbNewLine & ex.StackTrace, EventLogEntryType.Error)
            Throw ex
        End Try
    End Function

    Public Function SetSysVar(ByVal pv_strGRNAME As String, ByVal pv_strVARNAME As String, ByVal pv_strVARVALUE As String) As Long
        Dim v_strSQL As String
        Dim v_ds As DataSet

        Try
            v_strSQL = "SELECT VARVALUE, VARDESC, EN_VARDESC FROM SYSVAR WHERE TRIM(GRNAME)='" & pv_strGRNAME & "' AND TRIM(VARNAME)='" & pv_strVARNAME & "'"
            v_ds = OracleHelper.ExecuteDataset(ConnectionString, CommandType.Text, v_strSQL)
            If v_ds.Tables(0).Rows.Count = 0 Then
                Return ERR_SY_NO_DATAFOUND
            Else
                v_strSQL = "UPDATE SYSVAR SET VARVALUE='" & pv_strVARVALUE & "' WHERE TRIM(GRNAME)='" & pv_strGRNAME & "' AND TRIM(VARNAME)='" & pv_strVARNAME & "'"
                OracleHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, v_strSQL)
            End If
            Return ERR_SYSTEM_OK
        Catch ex As Exception
            'LogError.Write(ex.Message & vbNewLine & ex.StackTrace, EventLogEntryType.Error)
            Throw ex
        End Try
    End Function

    Public Function SetSysVar(ByVal pv_strGRNAME As String, ByVal pv_strVARNAME As String, ByVal pv_strBRID As String, ByVal pv_strVARVALUE As String) As Long
        Dim v_strSQL As String
        Dim v_ds As DataSet

        Try
            v_strSQL = "SELECT VARVALUE, VARDESC, EN_VARDESC FROM SYSVAR WHERE TRIM(GRNAME)='" & pv_strGRNAME & "' AND TRIM(VARNAME)='" & pv_strVARNAME & "' and BRID = '" & pv_strBRID & "'"
            v_ds = OracleHelper.ExecuteDataset(ConnectionString, CommandType.Text, v_strSQL)
            If v_ds.Tables(0).Rows.Count = 0 Then
                Return ERR_SY_NO_DATAFOUND
            Else
                v_strSQL = "UPDATE SYSVAR SET VARVALUE='" & pv_strVARVALUE & "' WHERE TRIM(GRNAME)='" & pv_strGRNAME & "' AND TRIM(VARNAME)='" & pv_strVARNAME & "' and BRID = '" & pv_strBRID & "'"
                OracleHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, v_strSQL)
            End If
            Return ERR_SYSTEM_OK
        Catch ex As Exception
            'LogError.Write(ex.Message & vbNewLine & ex.StackTrace, EventLogEntryType.Error)
            Throw ex
        End Try
    End Function

    Public Function GetValue(ByVal v_strTable As String, ByVal v_strFieldName As String, ByVal v_strWhere As String) As String
        Try
            Dim v_strSQL As String
            Dim v_ds As DataSet

            v_strSQL = "SELECT " & v_strFieldName & " FROM " & v_strTable & " WHERE 0=0 AND " & v_strWhere
            v_ds = ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            If v_ds.Tables(0).Rows.Count = 0 Then
                Return ""
            Else
                Return Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(0)(v_strFieldName)))
            End If
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Public Function ResetSequence(ByVal pv_strTable As String) As Long
        Dim v_strSQL As String
        Dim v_ds As DataSet

        Try
            'Kiểm tra Sequence đã tồn tại chưa
            v_strSQL = "SELECT * FROM USER_OBJECTS WHERE OBJECT_NAME = 'SEQ_" & pv_strTable & "'"
            v_ds = OracleHelper.ExecuteDataset(ConnectionString, CommandType.Text, v_strSQL)
            If v_ds.Tables(0).Rows.Count > 0 Then
                'Xoá Sequence cũ
                v_strSQL = "DROP SEQUENCE SEQ_" & pv_strTable
                OracleHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, v_strSQL)
            End If
            'Tạo mới Sequence 
            v_strSQL = "CREATE SEQUENCE SEQ_" & pv_strTable
            OracleHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, v_strSQL)

            Return ERR_SYSTEM_OK
        Catch ex As Exception
            'LogError.Write(ex.Message & vbNewLine & ex.StackTrace, EventLogEntryType.Error)
            Throw ex
        End Try
    End Function

    Public Function GetIDValue(ByVal pv_strTable As String) As Decimal
        Dim v_strSQL As String
        Dim v_ds As DataSet

        Try
            'Kiểm tra Sequence đã tồn tại chưa
            v_strSQL = "SELECT * FROM USER_OBJECTS WHERE OBJECT_NAME = 'SEQ_" & pv_strTable & "'"
            v_ds = OracleHelper.ExecuteDataset(ConnectionString, CommandType.Text, v_strSQL)
            If v_ds.Tables(0).Rows.Count = 0 Then
                'Tạo mới Sequence nếu chưa tồn tại
                v_strSQL = "CREATE SEQUENCE SEQ_" & pv_strTable
                OracleHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, v_strSQL)
            End If

            'Lấy IDValue
            v_strSQL = "Select SEQ_" & pv_strTable & ".NEXTVAL ID from DUAL"
            v_ds = OracleHelper.ExecuteDataset(ConnectionString, CommandType.Text, v_strSQL)

            If v_ds.Tables(0).Rows.Count > 0 Then
                Return v_ds.Tables(0).Rows(0)(0)
            End If
        Catch ex As Exception
            LogError.Write(ex.Message & vbNewLine & ex.StackTrace, EventLogEntryType.Error, gc_MODULE_HOST)
            Throw ex
        End Try
    End Function

    Public Function ExecuteNonQuery(ByVal CommandType As CommandType, _
                                    ByVal CommandText As String) As Integer
        Try
            If mv_blnIsTransaction Then
                Return mv_OracleHelper.TranExecuteNonQuery(CommandType, CommandText)
            Else
                Return OracleHelper.ExecuteNonQuery(ConnectionString, CommandType, CommandText)
            End If
        Catch ex As Exception
            LogError.Write(ex.Message & vbNewLine & CommandText, EventLogEntryType.Error, gc_MODULE_HOST)
            Throw ex
        End Try
    End Function


    Public Function ExecuteNonQuery(ByVal StoredProceduredName As String, ByVal ParameterNames() As Object, ByVal ParameterValues() As Object, ByVal ParameterTypes() As Object) As Integer
        Try
            If mv_blnIsTransaction Then
                Return mv_OracleHelper.TranExecuteNonQuery(StoredProceduredName, ParameterNames, ParameterValues, ParameterTypes)
            Else
                Return OracleHelper.ExecuteNonQuery(ConnectionString, StoredProceduredName, ParameterNames, ParameterValues, ParameterTypes)
            End If
        Catch ex As Exception
            LogError.Write(ex.Message & vbNewLine & StoredProceduredName, EventLogEntryType.Error, gc_MODULE_HOST)
            Throw ex
        End Try

    End Function
    '***********************************************************************
    'Input Parameters     :pv_strUserName: String, pv_strIPAddress: String
    '                      pv_strPassword: String
    'Ouput Parameters     :pv_oSessionKey: SessionKey, pv_strEncryptString: String
    '                      pv_strEncryptedXML: String, 
    '                      pv_strEncryptedSessionKey: String
    'Returned value       :String
    'Purpose        	  :Combine pv_strUsername, pv_strPassword, pv_strIPAddress
    '                      to v_strOrigXML.
    '                      Sign v_strOrigXML with private key of USB, and we
    '                      have v_strSignature.
    '                      Combine v_strOrigXML and v_strSignature and retun
    '                      to pv_strEncryptedXML.
    '                      pv_strEncryptedXML will be encrypted with pv_oSessionKey
    '                      and return to pv_strEncryptedXML
    '                      pv_oSessionKey will be encrypted with public key of VSD
    '                      and return to pv_strEncryptedSessionKey
    'Created date         :11/01/2011
    'Author               :Myvq
    'Last update date     :11/01/2011
    'Last modifying person:Myvq
    '***********************************************************************
    Public Function ExecuteNonQuery(ByVal SQLString As String, ByVal ParameterName() As Object, _
                                    ByVal ParameterValues() As Object, _
                                    ByVal ParameterTypes() As Object, ByVal CommandType As CommandType) As Integer
        Try
            If mv_blnIsTransaction Then
                Return mv_OracleHelper.TranExecuteNonQuery(SQLString, ParameterName, ParameterValues, ParameterTypes, CommandType)
            Else
                Return mv_OracleHelper.TranExecuteNonQuery(ConnectionString, SQLString, ParameterName, ParameterValues, ParameterTypes, CommandType)
            End If

        Catch ex As Exception
            LogError.Write(ex.Message & vbNewLine & SQLString, EventLogEntryType.Error, "DataAccess.ExecuteNonQuery")
            Throw ex
        End Try
    End Function
    Public Function ExecuteNonQuery(ByVal SQLString As String, ByVal ParameterNames As ArrayList, _
                                ByVal ParameterValues As ArrayList, _
                                ByVal ParameterTypes As ArrayList, ByVal CommandType As CommandType) As Integer
        Try
            mv_OracleHelper.TranExecuteNonQuery(SQLString, ParameterNames, ParameterValues, ParameterTypes, CommandType)
        Catch ex As Exception
            LogError.Write(ex.Message & vbNewLine & SQLString, EventLogEntryType.Error, "DataAccess.ExecuteNonQuery")
            Throw ex
        End Try

    End Function

    Public Function ExecuteNonQuery(ByVal SQLString As String, ByVal ParameterName As String, _
                                ByVal ParameterValue As String, _
                                 ByVal CommandType As CommandType) As Integer
        Try
            mv_OracleHelper.TranExecuteNonQuery(SQLString, ParameterName, ParameterValue, OracleDbType.Clob, CommandType)
        Catch ex As Exception
            LogError.Write(ex.Message & vbNewLine & SQLString, EventLogEntryType.Error, "DataAccess.ExecuteNonQuery")
            Throw ex
        End Try

    End Function

    Public Function CheckExitsField(ByVal v_strTable As String, Optional ByVal v_strFieldName As String = "DELETED") As Boolean
        Dim v_strSQL As String = "SELECT * FROM " & v_strTable & " WHERE 1=2"
        Dim test As New DataSet
        test = OracleHelper.ExecuteDataset(mv_strConnectionString, CommandType.Text, v_strSQL)

        If test.Tables(0).Columns.Contains(v_strFieldName) Then
            Return True
        Else
            Return False
        End If

        'If DataRow.Table.Columns.Contains("column") Then
        'MsgBox("YAY")
        'End If
        'Dim dr As OracleDataReader
        'dr = OracleHelper.ExecuteReader(mv_strConnectionString, CommandType.Text, v_strSQL)

        '        For i As Integer = 0 To dr.FieldCount - 1
        'If dr.GetName(i).ToUpper = v_strFieldName.ToUpper Then
        'Return True
        'End If
        'Next

        'Return False
    End Function

    'Public Function CheckUserAdmin(ByVal pv_strTellerID As String) As Boolean
    '    Dim v_strSQL As String
    '    Dim v_ds As DataSet

    '    Try
    '        v_strSQL = "SELECT ISADMIN FROM TLPROFILES WHERE TLID='" & pv_strTellerID & "'"
    '        v_ds = OracleHelper.ExecuteDataset(ConnectionString, CommandType.Text, v_strSQL)
    '        If v_ds.Tables(0).Rows.Count = 0 Then
    '            Return False
    '        Else
    '            If v_ds.Tables(0).Rows(0)(0) = "Y" Then
    '                Return True
    '            Else
    '                Return False
    '            End If
    '        End If
    '    Catch ex As Exception
    '        Return False
    '    End Try
    'End Function

    'HaNM5 them ham bulkcopy
    Public Function SaveUsingOracleBulkCopy(ByVal pv_strTableName As String, ByVal pv_dt As DataTable) As Boolean
        Try
            Return OracleHelper.SaveUsingOracleBulkCopy(ConnectionString, pv_strTableName, pv_dt)
        Catch ex As Exception
            LogError.Write(ex.Message & vbNewLine & pv_strTableName, EventLogEntryType.Error, "DataAccess.SaveUsingOracleBulkCopy")
            Throw ex
        End Try
    End Function
#End Region

#Region "Transaction"
    Public Sub BeginTran()
        mv_blnIsTransaction = True
        mv_OracleHelper = New OracleHelper
        mv_OracleHelper.BeginTran(mv_strConnectionString)
    End Sub

    Public Sub Rollback()
        mv_OracleHelper.Rollback()
    End Sub
    Public Sub Commit()
        mv_OracleHelper.Commit()
        mv_OracleHelper = Nothing
    End Sub
#End Region

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
#Region " IDisposable Support "
    Private disposedValue As Boolean = False        ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: free other state (managed objects).
            End If

            ' TODO: free your own state (unmanaged objects).
            ' TODO: set large fields to null.
        End If
        Me.disposedValue = True
    End Sub
    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

#End Region
End Class
