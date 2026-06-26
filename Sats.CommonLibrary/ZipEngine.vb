Imports Xceed.Zip
Imports Xceed.Compression
Imports Xceed.FileSystem
Imports System.IO


Public Class ZipEngine

    Dim oZip As ZipArchive
    Dim oZipFolder As QuickZip
    Dim oFile As DiskFile
    Public Sub New()
        Xceed.Zip.Licenser.LicenseKey = "ZIN20-AU14Z-N81SH-BKCA"
    End Sub

    Public Function ZipFile(ByVal pv_strFolderPath As String, ByVal pv_strFileName As String) As String
        Dim v_strFileName As String
        Try
            If Not pv_strFileName.EndsWith(".zip") Then
                v_strFileName = pv_strFolderPath & "\" & Mid(pv_strFileName, 1, Len(pv_strFileName) - 3) & "zip"
            Else
                v_strFileName = pv_strFolderPath & "\" & pv_strFileName
            End If

            NewZipFile(v_strFileName, oZip)
            oZip.BeginUpdate()
            oZip.DefaultCompressionLevel = CompressionLevel.Highest
            oZip.DefaultCompressionMethod = CompressionMethod.Deflated
            oZip.AllowSpanning = True

            oFile = New DiskFile(pv_strFolderPath & "\" & pv_strFileName)
            oFile.CopyTo(oZip.RootFolder, True)
            oZip.EndUpdate()
            oZip = Nothing

            File.Delete(pv_strFolderPath & "\" & pv_strFileName)

            Return v_strFileName
        Catch ex As Exception
            Return pv_strFolderPath & "\" & pv_strFileName
            Throw ex
        Finally
            If Not oFile Is Nothing Then
                oFile = Nothing
            End If
        End Try
    End Function
    'BangPV: zip file ko deleted
    Public Function ZipFileNotDel(ByVal pv_strFolderPath As String, ByVal pv_strFileName As String) As String
        Dim v_strFileName As String
        Try
            If Not pv_strFileName.EndsWith(".zip") Then
                v_strFileName = pv_strFolderPath & "\" & Mid(pv_strFileName, 1, Len(pv_strFileName) - 3) & "zip"
            Else
                v_strFileName = pv_strFolderPath & "\" & pv_strFileName
            End If

            NewZipFile(v_strFileName, oZip)
            
            QuickZip.Zip(v_strFileName, True, True, False, pv_strFolderPath & "*.xml")
            ' File.Delete(pv_strFolderPath & "\" & pv_strFileName)

            Return v_strFileName
        Catch ex As Exception
            Return pv_strFolderPath & "\" & pv_strFileName
            Throw ex
        Finally
            If Not oFile Is Nothing Then
                oFile = Nothing
            End If
        End Try
    End Function
    Public Function ZipAllFileNotDel(ByVal pv_strFolderPath As String, ByVal pv_strFileName As String) As String
        Dim v_strFileName As String
        Try

            If Not pv_strFileName.EndsWith(".zip") Then
                v_strFileName = pv_strFolderPath & "\" & Mid(pv_strFileName, 1, Len(pv_strFileName) - 3) & "zip"
            Else
                v_strFileName = pv_strFolderPath & "\" & pv_strFileName
            End If

            NewZipFile(v_strFileName, oZip)

            QuickZip.Zip(v_strFileName, True, True, False, pv_strFolderPath & "\*.*")
            ' File.Delete(pv_strFolderPath & "\" & pv_strFileName)

            Return v_strFileName
        Catch ex As Exception
            Return pv_strFolderPath & "\" & pv_strFileName
            Throw ex
        Finally
            If Not oFile Is Nothing Then
                oFile = Nothing
            End If
        End Try
    End Function

    Public Function Zip2FileNotDel(ByVal v_strInput1 As String, ByVal v_strInput2 As String, ByVal pv_strFileName As String) As String
        'Dim v_strFileName As String
        Try

            

            NewZipFile(pv_strFileName, oZip)
            If v_strInput2 = "" Then
                QuickZip.Zip(pv_strFileName, True, True, False, v_strInput1)
            Else
                QuickZip.Zip(pv_strFileName, True, True, False, v_strInput1, v_strInput2)
            End If

            ' File.Delete(pv_strFolderPath & "\" & pv_strFileName)

            Return pv_strFileName
        Catch ex As Exception
            Return pv_strFileName
            Throw ex
        Finally
            If Not oFile Is Nothing Then
                oFile = Nothing
            End If
        End Try
    End Function
    'Unzip
    Public Function UnzipFile(ByVal pv_strFolderPath As String, ByVal pv_strFileName As String, ByVal pv_strDefFileName As String) As String
        Dim v_strFileName As String
        Try
            If Not pv_strFileName.EndsWith(".zip") Then
                v_strFileName = pv_strFolderPath & "\" & Mid(pv_strFileName, 1, Len(pv_strFileName) - 3) & "zip"
            Else
                v_strFileName = pv_strFolderPath & "\" & pv_strFileName
            End If
            QuickZip.Unzip(v_strFileName, pv_strFolderPath, pv_strDefFileName)
            Return v_strFileName
        Catch ex As Exception
            LogError.Write(ex.Message, EventLogEntryType.Error, gc_MODULE_HOST)
            'Return pv_strFolderPath & "\" & pv_strFileName
            Throw ex
        Finally
            'If Not oFile Is Nothing Then
            '    oFile = Nothing
            'End If
        End Try
    End Function
    Public Function UnzipFiles(ByVal pv_strFolderPath As String, ByVal pv_strFileName As String) As String
        Dim v_strFileName As String
        Try
            If Not pv_strFileName.EndsWith(".zip") Then
                v_strFileName = """" & pv_strFolderPath & "\" & pv_strFileName & ".zip" & """"
            Else
                v_strFileName = """" & pv_strFolderPath & "\" & pv_strFileName & """"
            End If

            'NewZipFile(v_strFileName, oZip)

            Dim zipFile As New DiskFile(pv_strFolderPath & "\" & pv_strFileName)

            'Create a ZipArchive object to access the zip file
            Dim zip As New ZipArchive(zipFile)

            'Create a DiskFolder object for the destination folder
            Dim destinationFolder As New DiskFolder(pv_strFolderPath)

            'Copy the contents of the zip file to the destination folder
            zip.CopyFilesTo(destinationFolder, True, True)
            ' File.Delete(pv_strFolderPath & "\" & pv_strFileName)

            Return v_strFileName
        Catch ex As Exception
            Return pv_strFolderPath & "\" & pv_strFileName
            Throw ex
        Finally
            If Not oFile Is Nothing Then
                oFile = Nothing
            End If
        End Try
    End Function
    'end bangpv

    Private Sub NewZipFile(ByVal ZipFilename As String, ByRef pv_zipRoot As ZipArchive)
        Dim ZipFile As DiskFile
        Try
            ZipFile = New DiskFile(ZipFilename)

            If ZipFile.Exists Then
                ZipFile.Delete()
            End If

            ZipFile.Create()
            pv_zipRoot = New ZipArchive(ZipFile)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
End Class
