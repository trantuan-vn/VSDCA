Imports Sats.CommonLibrary
Imports System.IO
Imports Sats.CoreBusiness
Imports Sats.DataAccessLayer
'Imports System.EnterpriseServices
Imports System.Runtime.InteropServices
Imports BkavCASign
Imports Sats.ServerCA

'<JustInTimeActivation(False), _
'Transaction(TransactionOption.Disabled), _
'ObjectPooling(Enabled:=True, MinPoolSize:=30)> _
Public Class CacheData
    Implements IDisposable
    'Inherits ServicedComponent
    Public st As New SYSTEMTIME
    <DllImport("kernel32.dll")> _
    Public Shared Sub SetLocalTime(ByRef lpSystemTime As SYSTEMTIME)
    End Sub

    <DllImport("kernel32.dll")> _
    Public Shared Sub GetLocalTime(ByRef lpSystemTime As SYSTEMTIME)
    End Sub

    Public Sub CreateCacheFile(ByVal pv_strFolderName As String)
        Dim v_obj As New DataAccess
        Dim v_ds As New DataSet
        Dim v_ds1 As New DataSet
        Dim v_strSQL As String
        Try
            v_obj.NewDBInstance(gc_MODULE_HOST)

            v_strSQL = "SELECT * FROM DEFCACHE ORDER BY CACHEORD"
            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            For i As Integer = 0 To v_ds.Tables(0).Rows.Count - 1
                If v_ds.Tables(0).Rows(i)("ISSYNC") = "Y" Then
                    v_strSQL = v_ds.Tables(0).Rows(i)("CACHESQL")
                    v_ds1 = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                    v_ds1.WriteXml(System.IO.Path.Combine(pv_strFolderName, v_ds.Tables(0).Rows(i)("TBLNAME") & ".xml"))
                End If
            Next
            'Tao file CacheVersion
            If v_ds.Tables(0).Rows.Count > 0 Then
                Dim sw As New System.IO.StreamWriter(pv_strFolderName & "\CacheVersion.txt")
                sw.Write(Guid.NewGuid)
                sw.Flush()
                sw.Close()
            End If
        Catch ex As Exception
            Throw ex
        Finally
            v_ds.Dispose()
            v_ds1.Dispose()
            v_obj.Dispose()
        End Try
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
