Imports Sats.CommonLibrary
Imports System.IO
Imports System.Windows.Forms

Public Class frmFTPClient
    Private mv_strRemoteFilePath As String
    Private mv_strLocalFilePath As String
    Private mv_strServerAddress As String
    Private mv_strServerPort As String
    Private mv_strUserName As String
    Private mv_strPassword As String

    Dim v_oProcess As New ProcessForm(Me)

    Dim mv_xftpFTPEngine As New FTPEngine

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.


    End Sub

    Public Property RemotePath() As String
        Get
            Return mv_strRemoteFilePath
        End Get
        Set(ByVal value As String)
            mv_strRemoteFilePath = value
        End Set
    End Property

    Public Property LocalPath() As String
        Get
            Return mv_strLocalFilePath
        End Get
        Set(ByVal value As String)
            mv_strLocalFilePath = value
        End Set
    End Property
    Public Property ServerAddress() As String
        Get
            Return mv_strServerAddress
        End Get
        Set(ByVal value As String)
            mv_strServerAddress = value
        End Set
    End Property

    Public Property ServerPort() As String
        Get
            Return mv_strServerPort
        End Get
        Set(ByVal value As String)
            mv_strServerPort = value
        End Set
    End Property
    Public Property UserName() As String
        Get
            Return mv_strUserName
        End Get
        Set(ByVal value As String)
            mv_strUserName = value
        End Set
    End Property
    Public Property Password() As String
        Get
            Return mv_strPassword
        End Get
        Set(ByVal value As String)
            mv_strPassword = value
        End Set
    End Property
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Try
            If mv_xftpFTPEngine.Connect(ServerAddress, ServerPort, UserName, Password) Then
                LocalPath = txtFileLocation.Text
                If LocalPath <> "" Then
                    Dim v_strLocalDir As String
                    Dim v_arrLocalPath() As String = LocalPath.Split("\")
                    v_strLocalDir = Replace(LocalPath, "\" & v_arrLocalPath(v_arrLocalPath.Count - 1), "")

                    If Not System.IO.Directory.Exists(v_strLocalDir) Then
                        System.IO.Directory.CreateDirectory(v_strLocalDir)
                    End If
                    v_oProcess.StartProcessForm()
                    v_oProcess.ChangeCaption("Đang tải file")
                    Dim v_blnReceiveFileStatus As Boolean = False
                    v_blnReceiveFileStatus = mv_xftpFTPEngine.ReceiveFile(RemotePath, LocalPath)
                    If v_blnReceiveFileStatus Then
                        mv_xftpFTPEngine.Disconnect()
                        v_oProcess.StopProcessForm()
                        MsgBox("File đã được lưu tại địa chỉ " & LocalPath, MsgBoxStyle.OkOnly, "Thông báo lưu file")
                        Me.Close()
                    Else
                        v_oProcess.StopProcessForm()
                        MsgBox("Lỗi trong quá trình tải file từ máy chủ, Vui lòng xem lại đường dẫn lưu file trên máy trạm hoặc máy chủ!", MsgBoxStyle.OkOnly, "Thông báo lưu file")
                        Exit Sub
                    End If
                Else
                    MsgBox("Bạn chưa chọn đường dẫn để lưu file. Vui lòng kích vào Browse để chọn nơi lưu file", MsgBoxStyle.Critical, "Chọn đường dẫn lưu file về máy")
                    Exit Sub
                End If

            Else
                MsgBox("Không kết nối được tới máy chủ FTP, vui lòng kiểm tra lại cấu hình máy chủ FTP", MsgBoxStyle.Critical, "Lỗi kết nối máy chủ FTP")
                Exit Sub
            End If
        Catch ex As Exception
            If Not v_oProcess Is Nothing Then
                v_oProcess.StopProcessForm()
            End If
            Throw ex
        End Try
    End Sub

    Private Sub btnBrowse_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
        Dim v_sfdSaveFile As New SaveFileDialog
        'v_sfdSaveFile.Filter = "Excel(*.xls)|*.xls|File XML(*.xml)|*.xml|File Text(*.txt)|*.txt"
        v_sfdSaveFile.Filter = "Zip Files(*.zip)|*.zip"
        v_sfdSaveFile.ShowDialog()
        Me.txtFileLocation.Text = v_sfdSaveFile.FileName
    End Sub
End Class