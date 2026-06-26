Imports System
Imports System.Data
Imports System.Xml
Imports Microsoft.Win32
Imports System.Configuration
Imports System.IO
Imports System.EnterpriseServices
Imports Sats.CommonLibrary
Imports TimesTenHelper

<Transaction(TransactionOption.Disabled), _
ObjectPooling(Enabled:=True, MinPoolSize:=4, MaxPoolSize:=10000)> _
Public Class TTDataAccess
    Inherits ServicedComponent

    Private mv_strModule As String = gc_MODULE_BDS
    Private mv_strConnectionString As String
    Private mv_objTTConn As TTHelper
#Region " Hàm constructor "

    Public Sub New()
        _InitClass()
    End Sub
    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        mv_objTTConn.Destroy()
    End Sub

    Private Sub _InitClass()
        Dim v_strDataSource, v_strUsername, v_strPassword, v_strDBInit As String
        Dim pv_strModule As String = gc_MODULE_BDS
        Try
            If Not (pv_strModule Is Nothing) Then
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
            Else
                v_strDataSource = String.Empty
                v_strUsername = String.Empty
                v_strPassword = String.Empty
                v_strDBInit = String.Empty
            End If

            mv_strConnectionString = _BuildConnectionString(v_strDataSource, v_strUsername, v_strPassword)
            mv_objTTConn = New TTHelper(mv_strConnectionString)

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

#End Region

#Region " Helper "
    Private Function _BuildConnectionString(ByVal pv_strDataSource As String, _
                                            ByVal pv_strUserName As String, _
                                            ByVal pv_strPassword As String) As String
        Dim v_strConnectString As String = String.Empty

        If Not (pv_strDataSource = String.Empty) Then
            v_strConnectString &= "dsn=" & pv_strDataSource
        End If
        If Not (pv_strUserName = String.Empty) Then
            v_strConnectString &= ";uid=" & pv_strUserName
        End If
        If Not (pv_strPassword = String.Empty) Then
            v_strConnectString &= ";pwd=" & pv_strPassword
        End If

        Return v_strConnectString.ToLower
    End Function

#End Region

#Region " Các public method "

    Public Function ExecuteSQLReturnDataset(ByVal v_CommandType As CommandType, ByVal CommandText As String) As DataSet
        Try
            Return mv_objTTConn.ExecuteReturnDataSet(CommandText)
        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Public Function GetSysVar(ByVal pv_strGRNAME As String, ByVal pv_strVARNAME As String, ByRef pv_strVARVALUE As String) As Long
        Dim v_strSQL As String = ""
        Dim v_ds As DataSet

        Try
            v_strSQL = "SELECT VARVALUE, VARDESC, EN_VARDESC FROM SYSVAR WHERE TRIM(GRNAME) = '" & pv_strGRNAME & "' AND TRIM(VARNAME)='" & pv_strVARNAME & "'"
            v_ds = ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            If v_ds.Tables(0).Rows.Count = 0 Then
                Return ERR_SY_VARIABLE_NOTFOUND
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
            v_ds = ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            If v_ds.Tables(0).Rows.Count = 0 Then
                Return ERR_SY_VARIABLE_NOTFOUND
            Else
                pv_strVARVALUE = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(0)("VARVALUE")))
            End If
            Return ERR_SYSTEM_OK
        Catch ex As Exception
            'LogError.Write(v_strSQL & ControlChars.CrLf & ex.Message & vbNewLine & ex.StackTrace, EventLogEntryType.Error)
            Throw ex
        End Try
    End Function


    Public Sub ExecuteNonQueryTran(ByVal v_CommandType As CommandType, ByVal CommandText As String)

        Try
            mv_objTTConn.ExecuteNonQuery(CommandText, 1, True)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub ExecuteNonQuery(ByVal v_CommandType As CommandType, ByVal CommandText As String)

        Try
            mv_objTTConn.ExecuteNonQuery(CommandText)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub


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

#End Region

#Region "Transaction"
    Public Sub Rollback()
        mv_objTTConn.Rollback()
    End Sub
    Public Sub Commit()
        mv_objTTConn.Commit()
    End Sub
#End Region
End Class
