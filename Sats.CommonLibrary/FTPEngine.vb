Imports XceedFtpLib
Public Class FTPEngine
    Dim mv_xftpXceedFtp As XceedFtp

    Dim mv_strServerAddress As String
    Dim mv_strServerPort As String
    Dim mv_strUserName As String
    Dim mv_strPassword As String

    Public Sub New()
    End Sub

    Public Function Connect(ByVal pv_strServerAddress As String, ByVal pv_strServerPort As String, _
                            ByVal pv_strUserName As String, ByVal pv_strPassword As String)
        Try
            mv_xftpXceedFtp = New XceedFtp()
            mv_xftpXceedFtp.ServerAddress = pv_strServerAddress
            mv_xftpXceedFtp.ServerPort = pv_strServerPort
            mv_xftpXceedFtp.UserName = pv_strUserName
            mv_xftpXceedFtp.Password = pv_strPassword
            mv_xftpXceedFtp.Connect()

            Return True

        Catch ex As Exception
            Return False
            Throw ex
        End Try
    End Function

    Public Sub Disconnect()
        Try
            If Not mv_xftpXceedFtp Is Nothing Then
                mv_xftpXceedFtp.Disconnect()
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function ReceiveFile(ByVal pv_strRemoteFilePath As String, ByVal pv_strLocalFilePath As String) As Boolean
        Try
            Dim v_strRootPath As String = mv_xftpXceedFtp.CurrentFolder

            mv_xftpXceedFtp.ReceiveFile(v_strRootPath & pv_strRemoteFilePath, 0, pv_strLocalFilePath)
            Return True

        Catch ex As Exception
            'Throw ex
            LogError.Write(ex.Message, EventLogEntryType.Error, gc_MODULE_HOST)
            Return False
        End Try
    End Function

    Public Function SendFile(ByVal pv_strRemoteFilePath As String, ByVal pv_strLocalFilePath As String) As Boolean
        Try
            mv_xftpXceedFtp.SendFile(pv_strLocalFilePath, 0, pv_strRemoteFilePath, False)
            Return True
        Catch ex As Exception
            'Throw ex
            LogError.Write(ex.Message, EventLogEntryType.Error, gc_MODULE_HOST)
            Return False
        End Try
    End Function
End Class
