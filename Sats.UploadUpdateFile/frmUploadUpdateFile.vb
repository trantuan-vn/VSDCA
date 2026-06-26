Imports System.Windows
Imports System.IO
Imports System.Xml
Imports System.Net
Imports Xceed.Zip
Imports Xceed.Compression
Imports Xceed.FileSystem

Public Class frmUpLoadUpdateFile

    Private mv_strReportFile As String = "Reports.zip"
    Private mv_strPath As String = "C:\Inetpub\wwwroot\AutoUpdate"
    Private mv_strAutoFile As String = "\AutoUpdate.xml"
    Private mv_strServer As String = "C:\Inetpub\wwwroot\AutoUpdate"

    Private Sub btnPath_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPath.Click
        Try
            Dim v_fbr As New FolderBrowserDialog
            v_fbr.SelectedPath = mv_strPath
            v_fbr.ShowDialog()
            txtPath.Text = v_fbr.SelectedPath
            GetFiles(txtPath.Text)
            mv_strPath = txtPath.Text
            btnOK.Enabled = True
            btnReport.Enabled = True
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub GetFiles(ByVal pv_strPath As String)
        Dim ofs As New DirectoryInfo(pv_strPath & "\")
        Dim oFile As FileInfo
        For Each oFile In ofs.GetFiles
            'If (Mid(oFile.Name, 1, 4).ToUpper = "SATS" And (oFile.Name.EndsWith(".dll") Or oFile.Name.EndsWith(".exe") Or oFile.Name.EndsWith(".config"))) Or oFile.Name.EndsWith(".sdf") Then
            chlFile.Items.Add(oFile.Name, True)
            'End If
        Next
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        CreateFileAutoUpdate()
    End Sub

    Private Sub frmUpLoadUpdateFile_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblProcess.Text = ""
        txtServer.Text = mv_strServer
        'txtPath.Text = mv_strPath
        'GetFiles(mv_strPath)
    End Sub

    Private Sub CreateFileAutoUpdate()
        Dim xmlDoc As New XmlDocument
        Dim oElmntRoot As XmlElement
        Dim v_strFileName As String
        Dim v_strTmpFileName As String
        Dim entryNode As Xml.XmlNode

        oElmntRoot = xmlDoc.CreateElement("update")
        xmlDoc.AppendChild(oElmntRoot)
        prc.Minimum = 0
        prc.Maximum = chlFile.CheckedItems.Count * 10
        Dim oZip As ZipArchive
        Dim oFile As DiskFile
        Dim fv As FileVersionInfo
        Dim v_attrFILENAME, v_attrFILEVERSION, v_attrFILEDATE As Xml.XmlAttribute

        For v_int As Integer = 0 To chlFile.CheckedItems.Count - 1
            lblProcess.Text = "Đang xủ lý tệp tin " & chlFile.Items(v_int)
            Application.DoEvents()

            entryNode = xmlDoc.CreateNode(Xml.XmlNodeType.Element, "Entry", "")

            fv = FileVersionInfo.GetVersionInfo(txtPath.Text & "\" & chlFile.Items(v_int))

            v_attrFILENAME = xmlDoc.CreateAttribute("filename")
            v_attrFILENAME.Value = chlFile.Items(v_int)
            entryNode.Attributes.Append(v_attrFILENAME)

            v_attrFILEVERSION = xmlDoc.CreateAttribute("version")
            v_attrFILEVERSION.Value = fv.FileMajorPart & "." & fv.FileMinorPart
            entryNode.Attributes.Append(v_attrFILEVERSION)

            v_attrFILEDATE = xmlDoc.CreateAttribute("date")
            v_attrFILEDATE.Value = File.GetLastWriteTime(fv.FileName)
            entryNode.Attributes.Append(v_attrFILEDATE)

            oElmntRoot.AppendChild(entryNode)
           
            v_strFileName = mv_strPath & "\" & chlFile.CheckedItems(v_int)
            If Not v_strFileName.EndsWith(".zip") Then
                If v_strFileName.EndsWith(".config") Then
                    v_strTmpFileName = mv_strServer & "\config.zip"
                Else
                    v_strTmpFileName = mv_strServer & "\" & Mid(chlFile.CheckedItems(v_int), 1, Len(chlFile.CheckedItems(v_int)) - 3) & "zip"
                End If

                NewZipFile(v_strTmpFileName, oZip)
                oZip.BeginUpdate()
                oZip.DefaultCompressionLevel = CompressionLevel.Highest
                oZip.DefaultCompressionMethod = CompressionMethod.Stored
                oZip.AllowSpanning = True
                oFile = New DiskFile(v_strFileName)
                oFile.CopyTo(oZip.RootFolder, True)
                oZip.EndUpdate()
                oZip = Nothing
                File.Delete(v_strFileName)
            End If
            prc.Increment(10)
        Next

        xmlDoc.Save(txtPath.Text & mv_strAutoFile)
        lblProcess.Text = "Cập nhật thành công!"
    End Sub

    Private Sub btnReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReport.Click
        Try
            Dim v_fbr As New FolderBrowserDialog
            v_fbr.SelectedPath = txtPath.Text & "\Reports\"
            v_fbr.ShowDialog()
            ZipFiles(v_fbr.SelectedPath)
            chlFile.Items.Add(mv_strReportFile, True)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub ZipFiles(ByVal pv_strPath As String)
        Dim ofs As New DirectoryInfo(pv_strPath)
        Dim oDir As DirectoryInfo
        Dim oFile As FileInfo
        Dim File As DiskFile
        Dim DestFolder As ZippedFolder
        Dim m_zipRoot As ZipArchive
        Cursor.Current = Cursors.WaitCursor
        Try
            NewZipFile(mv_strServer & "\" & mv_strReportFile, m_zipRoot)
            m_zipRoot.BeginUpdate()
            m_zipRoot.DefaultCompressionLevel = CompressionLevel.Highest
            m_zipRoot.DefaultCompressionMethod = CompressionMethod.Stored
            m_zipRoot.AllowSpanning = True
            Dim PathName As String
            Try
                prc.Maximum = GetFilesCount(pv_strPath) * 10

                For Each oFile In ofs.GetFiles
                    File = New DiskFile(oFile.FullName)
                    PathName = "Reports"
                    DestFolder = m_zipRoot.GetFolder(PathName)
                    File.CopyTo(DestFolder, False)

                    lblProcess.Text = "Đang nén tệp tin " & oFile.Name
                    prc.Increment(10)
                    Application.DoEvents()
                Next


                For Each oDir In ofs.GetDirectories
                    PathName = "Reports\" & oDir.Name
                    DestFolder = m_zipRoot.GetFolder(PathName)
                    ofs = New DirectoryInfo(pv_strPath & "\" & oDir.Name)
                    For Each oFile In ofs.GetFiles
                        File = New DiskFile(oFile.FullName)
                        File.CopyTo(DestFolder, False)
                        lblProcess.Text = "Đang nén tệp tin " & oFile.Name
                        prc.Increment(10)
                        Application.DoEvents()
                    Next
                Next
            Finally
                m_zipRoot.EndUpdate()
                m_zipRoot = Nothing
                lblProcess.Text = "Tạo tệp tin báo cáo thành công!"
            End Try
        Catch Except As Exception

        End Try
        Cursor.Current = Cursors.Default
    End Sub

    Private Function GetFilesCount(ByVal pv_strPath As String) As Integer
        Dim ofs As New DirectoryInfo(pv_strPath)
        Dim oDir As DirectoryInfo
        Dim v_intCount As Integer
        v_intCount = ofs.GetFiles.Length

        For Each oDir In ofs.GetDirectories
            ofs = New DirectoryInfo(pv_strPath & "\" & oDir.Name)
            v_intCount += ofs.GetFiles.Length
        Next
        Return v_intCount
    End Function

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

    Public Sub New()
        Xceed.Zip.Licenser.LicenseKey = "ZIN20-AU14Z-N81SH-BKCA"
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim v_fbr As New FolderBrowserDialog
        v_fbr.SelectedPath = mv_strServer
        v_fbr.ShowDialog()
        txtServer.Text = v_fbr.SelectedPath
    End Sub
End Class
