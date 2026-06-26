Imports log4net
Public Class LogWriter
    Public Shared mv_l4nLogger As ILog

    Public Const LOG_CLIENT As String = "Sats.Client"
    Public Const LOG_BDS As String = "Sats.BDS"
    Public Const LOG_HOST As String = "Sats.HOST"
    Public Const LOG_START As Integer = 0
    Public Const LOG_DEBUG As Integer = 1
    Public Const LOG_STOP As Integer = 2

    Public Sub WriteLog(ByVal pv_strModuleLayer As String, _
                        ByVal pv_strModuleCode As String, _
                       ByVal pv_strClassName As String, _
                       ByVal pv_strFunctionName As String, _
                       ByVal pv_strIpAddress As String, _
                       ByVal pv_strHostName As String, _
                       ByVal pv_strTellerId As String, _
                       ByVal pv_strTellerName As String, _
                       ByVal pv_strMessageLog As String)

        Try

            Select Case pv_strModuleLayer
                Case gc_MODULE_CLIENT
                    WriteLogToFile(LOG_CLIENT, pv_strModuleCode, pv_strClassName, pv_strFunctionName, pv_strIpAddress, pv_strHostName, _
                                   pv_strTellerId, pv_strTellerName, pv_strMessageLog, LOG_DEBUG)
                Case gc_MODULE_BDS
                    WriteLogToFile(LOG_BDS, pv_strModuleCode, pv_strClassName, pv_strFunctionName, pv_strIpAddress, pv_strHostName, _
                                   pv_strTellerId, pv_strTellerName, pv_strMessageLog, LOG_DEBUG)
                Case gc_MODULE_HOST
                    WriteLogToFile(LOG_HOST, pv_strModuleCode, pv_strClassName, pv_strFunctionName, pv_strIpAddress, pv_strHostName, _
                                   pv_strTellerId, pv_strTellerName, pv_strMessageLog, LOG_DEBUG)
            End Select
        Catch ex As Exception
            Throw ex
        Finally
            GC.Collect()
        End Try
    End Sub
    Public Sub StopWriteLog(ByVal pv_strModuleLayer As String, _
                        ByVal pv_strModuleCode As String, _
                       ByVal pv_strClassName As String, _
                       ByVal pv_strFunctionName As String, _
                       ByVal pv_strIpAddress As String, _
                       ByVal pv_strHostName As String, _
                       ByVal pv_strTellerId As String, _
                       ByVal pv_strTellerName As String, _
                       ByVal pv_strMessageLog As String)

        Try

            Select Case pv_strModuleLayer
                Case gc_MODULE_CLIENT
                    WriteLogToFile(LOG_CLIENT, pv_strModuleCode, pv_strClassName, pv_strFunctionName, pv_strIpAddress, pv_strHostName, _
                                   pv_strTellerId, pv_strTellerName, pv_strMessageLog, LOG_STOP)
                Case gc_MODULE_BDS
                    WriteLogToFile(LOG_BDS, pv_strModuleCode, pv_strClassName, pv_strFunctionName, pv_strIpAddress, pv_strHostName, _
                                   pv_strTellerId, pv_strTellerName, pv_strMessageLog, LOG_STOP)
                Case gc_MODULE_HOST
                    WriteLogToFile(LOG_HOST, pv_strModuleCode, pv_strClassName, pv_strFunctionName, pv_strIpAddress, pv_strHostName, _
                                   pv_strTellerId, pv_strTellerName, pv_strMessageLog, LOG_STOP)
            End Select
        Catch ex As Exception
            Throw ex
        Finally
            GC.Collect()
        End Try
    End Sub
    Public Sub StartWriteLog(ByVal pv_strModuleLayer As String, _
                        ByVal pv_strModuleCode As String, _
                       ByVal pv_strClassName As String, _
                       ByVal pv_strFunctionName As String, _
                       ByVal pv_strIpAddress As String, _
                       ByVal pv_strHostName As String, _
                       ByVal pv_strTellerId As String, _
                       ByVal pv_strTellerName As String, _
                       ByVal pv_strMessageLog As String)

        Try

            Select Case pv_strModuleLayer
                Case gc_MODULE_CLIENT
                    WriteLogToFile(LOG_CLIENT, pv_strModuleCode, pv_strClassName, pv_strFunctionName, pv_strIpAddress, pv_strHostName, _
                                   pv_strTellerId, pv_strTellerName, pv_strMessageLog, LOG_START)
                Case gc_MODULE_BDS
                    WriteLogToFile(LOG_BDS, pv_strModuleCode, pv_strClassName, pv_strFunctionName, pv_strIpAddress, pv_strHostName, _
                                   pv_strTellerId, pv_strTellerName, pv_strMessageLog, LOG_START)
                Case gc_MODULE_HOST
                    WriteLogToFile(LOG_HOST, pv_strModuleCode, pv_strClassName, pv_strFunctionName, pv_strIpAddress, pv_strHostName, _
                                   pv_strTellerId, pv_strTellerName, pv_strMessageLog, LOG_START)
            End Select
        Catch ex As Exception
            Throw ex
        Finally
            GC.Collect()
        End Try
    End Sub
    Private Sub WriteLogToFile(ByVal pv_strLoggerName As String, _
                              ByVal pv_strModuleCode As String, _
                       ByVal pv_strClassName As String, _
                       ByVal pv_strFunctionName As String, _
                       ByVal pv_strIpAddress As String, _
                       ByVal pv_strHostName As String, _
                       ByVal pv_strTellerId As String, _
                       ByVal pv_strTellerName As String, _
                       ByVal pv_strMessageLog As String, _
                       ByVal pv_intLogType As Integer)
        Dim v_strInfo As String = ""
        Try
            mv_l4nLogger = log4net.LogManager.GetLogger(pv_strLoggerName)
            v_strInfo = pv_strLoggerName & "." & pv_strModuleCode & "." _
                        & pv_strClassName & "." & pv_strFunctionName & "." & pv_strIpAddress & "." & pv_strTellerName
            Select Case pv_intLogType
                Case LOG_START
                    mv_l4nLogger.Info(v_strInfo & " - Start()")
                Case LOG_DEBUG
                    mv_l4nLogger.Debug(v_strInfo & " - " & pv_strMessageLog)
                Case LOG_STOP
                    mv_l4nLogger.Info(v_strInfo & " - " & " - Finish()")
            End Select
        Catch ex As Exception
            mv_l4nLogger.Error(pv_strLoggerName & " - " & ex.Message)
            Throw ex
        Finally
            GC.Collect()
        End Try
    End Sub
End Class
