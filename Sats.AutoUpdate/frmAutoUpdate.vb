Imports System.Net
Imports System.Xml
Imports System.IO
Imports Xceed.Zip
Imports Xceed.Compression
Imports Xceed.FileSystem
Imports System.Configuration.ConfigurationManager
Imports BkavCASign

Public Class frmAutoUpdate
    Private mv_blnUpdate As Boolean = False
    Dim mv_Count As Integer
    Private ManifestFile As String = "AutoUpdate.xml"
    Private RemoteUri As String = AppSettings.Get("RemoteUri") '  "http://192.168.134.94:80/AutoUpdate/"
    Private v_strCurVersion As String
    Private v_strVersion As String
    Private mv_strReportFile As String = "Reports.zip"
    Private mv_strExeFile As String = "Sats.exe"

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnLogin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLogin.Click
        System.Diagnostics.Process.Start(mv_strExeFile, "ZIN20-AU14Z-N81SH-BKCA")
        Me.Close()
    End Sub

    Private Sub frmAutoUpdate_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        File.Delete(Application.StartupPath & "\" & ManifestFile)
    End Sub

    Private Sub frmAutoUpdate_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.lblProccess.Text = "Đang kiểm tra phiên bản. Xin vui lòng chờ..."
        btnLogin.Enabled = False
        btnUpdate.Enabled = False

        If File.Exists(mv_strExeFile) Then
            Dim fv As FileVersionInfo = FileVersionInfo.GetVersionInfo(mv_strExeFile)
            lblVersion.Text = "Phiên bản: " & fv.FileMajorPart & "." & fv.FileMinorPart
        Else
            lblVersion.Text = "Phiên bản: 0.0"
        End If
        tmr.Enabled = True
    End Sub

    Private Sub tmr_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tmr.Tick
        mv_Count += 1
        If mv_Count = 2 Then
            If Not CheckVersion() Then
                btnLogin.Enabled = True
                btnUpdate.Enabled = True
                lblProccess.Text = "Bạn đang sử dụng phiên bản mới nhất!"
            Else
                btnLogin.Enabled = False
                btnUpdate.Enabled = True
            End If
            tmr.Enabled = False
        End If
        lblVersion.Text = "Phiên bản: " & v_strCurVersion
    End Sub

    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        ProcessUpdate()
    End Sub

    Private Sub ProcessUpdate()
        Dim myWebClient As New WebClient
        Dim RealFileName As String
        Me.Cursor = Cursors.WaitCursor
        btnUpdate.Enabled = False
        lblProccess.Text = "Đang kết nối máy chủ..."
        Application.DoEvents()
        Try
            Dim m_xmld As XmlDocument
            Dim m_nodelist As XmlNodeList
            'Dim m_node As XmlNode
            Dim fileNameValue As String
            'Create the XML Document
            m_xmld = New XmlDocument
            'Load the Xml file
            m_xmld.Load(Application.StartupPath & "\" & ManifestFile)
            'Get the list of name nodes 
            m_nodelist = m_xmld.SelectNodes("/update/Entry")

            InitProgress(m_nodelist.Count + GetFilesCount(Application.StartupPath & "\Reports"))
            For i As Integer = 0 To m_nodelist.Count - 1
                With m_nodelist.Item(i)
                    fileNameValue = CStr(CType(.Attributes.GetNamedItem("filename"), Xml.XmlAttribute).Value)
                    'v_strVersion = CStr(CType(.Attributes.GetNamedItem("version"), Xml.XmlAttribute).Value)
                    'v_strDate = CStr(CType(.Attributes.GetNamedItem("date"), Xml.XmlAttribute).Value)
                End With

                If fileNameValue.EndsWith(".config") Then
                    fileNameValue = "config.zip"
                Else
                    fileNameValue = Mid(fileNameValue, 1, Len(fileNameValue) - 3) & "zip"
                End If

                RealFileName = Application.StartupPath & "\" & fileNameValue
                myWebClient.DownloadFile(RemoteUri & fileNameValue, RealFileName)

                If File.Exists(Application.StartupPath & "\" & fileNameValue) Then
                    UnzipFiles(fileNameValue)
                    File.Delete(Application.StartupPath & "\" & fileNameValue)
                End If

                If File.Exists(Application.StartupPath & "\" & "vsd.zip") Then
                    If fileNameValue = "vsd.cer" Then
                        Dim v_oCerClient As CertificateClient = New CertificateClient
                        Dim v_intErr = v_oCerClient.LoadCertificate
                        v_intErr = v_oCerClient.UnregisterCertificate()

                        v_oCerClient.LoadCertificateFromFile(Application.StartupPath & "\" & "vsd.cer")
                        'v_oCerClient.LoadCertificateFromFile("D:\\vsd.cer")
                        v_intErr = v_oCerClient.RegisterCertificate()
                    End If
                End If
            Next

            'register new vsd public key
            

            lblProccess.Text = "Đã câp nhật thành công phiên bản " & v_strVersion
            Application.DoEvents()

            btnUpdate.Enabled = True
            btnLogin.Enabled = True

        Catch ex As Exception
            lblProccess.Text = "Không kết nối được với máy chủ! Xin vui lòng thử lại"
            Application.DoEvents()
            'MessageBox.Show("Không kết nối được với máy chủ!")
        Finally
            Me.Cursor = Cursors.Default
            btnUpdate.Enabled = True
        End Try
    End Sub

    Private Sub InitProgress(ByVal lMax As Long)
        prbUpdate.Value = 0
        prbUpdate.Maximum = lMax
        Application.DoEvents()
    End Sub

    Private Sub IncrementProgress()
        With prbUpdate
            If .Value < .Maximum Then .Value = .Value + 1
            Application.DoEvents()
        End With
    End Sub

    Private Function CheckVersion() As Boolean
        Dim myWebClient As New WebClient
        Dim isToUpgrade As Boolean = False
        Try
            Me.Cursor = Cursors.WaitCursor
            Application.DoEvents()

            myWebClient.DownloadFile(RemoteUri & ManifestFile, ManifestFile)
            v_strVersion = GetVersion()
            If File.Exists(Application.StartupPath & "\" & mv_strExeFile) Then
                Dim fv As FileVersionInfo = FileVersionInfo.GetVersionInfo(mv_strExeFile)
                v_strCurVersion = fv.FileMajorPart & "." & fv.FileMinorPart
                isToUpgrade = (v_strVersion <> v_strCurVersion)
            Else
                isToUpgrade = True
            End If
            lblProccess.Text = "Phiên bản hiện tại: " & v_strVersion
            Return isToUpgrade

        Catch ex As Exception
            lblProccess.Text = "Không kết nối được với máy chủ! Xin vui lòng thử lại"
            Application.DoEvents()
            Return True
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Function

    Private Sub UnzipFiles(ByVal pv_strFileName As String)
        Cursor.Current = Cursors.WaitCursor
        Try
            Dim DestFolder As DiskFolder = New DiskFolder(Application.StartupPath)
            Dim SubDestFolder As AbstractFolder
            Dim m_zipRoot As ZipArchive
            Dim ZipFile As DiskFile
            Dim oFile As AbstractFile

            ZipFile = New DiskFile(Application.StartupPath & "\" & pv_strFileName)
            m_zipRoot = New ZipArchive(ZipFile)

            Dim Files() As AbstractFile = m_zipRoot.GetFiles(True)

            For Each oFile In Files
                lblProccess.Text = "Đang cập nhật têp tin: " & oFile.Name & "..."
                If oFile.ParentFolder.IsRoot Then
                    oFile.CopyTo(DestFolder, True)
                Else
                    SubDestFolder = DestFolder.GetFolder(Mid(oFile.ParentFolder.FullName, 2))

                    If Not SubDestFolder.Exists Then
                        SubDestFolder = DestFolder.CreateFolder(Mid(oFile.ParentFolder.FullName, 2))
                    End If
                    oFile.CopyTo(SubDestFolder, True)
                End If
                IncrementProgress()
            Next
        Catch ex As Exception
            Throw ex
        End Try
        Cursor.Current = Cursors.Default
    End Sub

    Private Function GetFilesCount(ByVal pv_strPath As String) As Integer
        Try
            Dim ofs As New DirectoryInfo(pv_strPath)
            Dim oDir As DirectoryInfo
            Dim v_intCount As Integer
            v_intCount = ofs.GetFiles.Length

            For Each oDir In ofs.GetDirectories
                ofs = New DirectoryInfo(pv_strPath & "\" & oDir.Name)
                v_intCount += ofs.GetFiles.Length
            Next
            Return v_intCount
        Catch ex As Exception
            Return 0
        End Try
    End Function

    Public Sub New()
        Xceed.Zip.Licenser.LicenseKey = "ZIN20-AU14Z-N81SH-BKCA"
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Function GetVersion() As String
        Dim m_xmld As XmlDocument
        Dim m_nodelist As XmlNodeList
        m_xmld = New XmlDocument
        'Load the Xml file
        m_xmld.Load(Application.StartupPath & "\" & ManifestFile)
        m_nodelist = m_xmld.SelectNodes("/update/Entry")
        Dim m_node As XmlNode
        Dim v_strName, v_strVersion, v_strDate As String

        For i As Integer = 0 To m_nodelist.Count - 1
            v_strName = ""
            With m_nodelist.Item(i)
                v_strName = CStr(CType(.Attributes.GetNamedItem("filename"), Xml.XmlAttribute).Value)
                v_strVersion = CStr(CType(.Attributes.GetNamedItem("version"), Xml.XmlAttribute).Value)
                v_strDate = CStr(CType(.Attributes.GetNamedItem("date"), Xml.XmlAttribute).Value)
            End With
            If v_strName.ToUpper = mv_strExeFile.ToUpper Then
                Exit For
            End If
        Next

        If v_strName = "" Then
            v_strVersion = "0.0"
        End If

        Return v_strVersion
    End Function
End Class