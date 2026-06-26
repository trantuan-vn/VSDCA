Imports System
Imports System.Data
Imports System.Data.SqlServerCe
Imports Sats.CommonLibrary

Public Class SQLDataAccessLayer
    Private mv_strConnectionString As String
    Private mv_oConn As SqlCeConnection
    Private mv_strTLName As String
    Private mv_strFileName As String


    Public Sub New(ByVal pv_strTLName As String)
        Dim v_oApp As New ApplicationServices.ApplicationBase
        Dim v_strPath As String = v_oApp.Info.DirectoryPath
        mv_strTLName = pv_strTLName
        mv_strFileName = EncryptString(mv_strTLName, DATA_FILE)
        mv_strFileName = Replace(mv_strFileName, "/", "")
        mv_strFileName = Replace(mv_strFileName, "\", "")
        mv_strFileName = Replace(mv_strFileName, "+", "")
        mv_strFileName = Replace(mv_strFileName, "-", "")
        mv_strFileName = Replace(mv_strFileName, "*", "")
        mv_strFileName = Replace(mv_strFileName, "=", "")
        mv_strFileName = Mid(mv_strFileName, 1, 8)
        BuildConnStr()
        DecryptFile(v_strPath & "\UserData\" & mv_strFileName & ".enc", v_strPath & "\UserData\" & mv_strFileName & ".sdf", mv_strTLName)
        CreateConnection()
    End Sub

    Private Sub BuildConnStr()
        mv_strConnectionString = "Data Source=|DataDirectory|\UserData\" & mv_strFileName & ".sdf" _
                                & ";Password=" & DATA_PASSWORD & ";Persist Security Info=True"
    End Sub

    Private Sub CreateConnection()
        Try
            'Dim v_oConn As New SqlCeConnection
           
            mv_oConn = New SqlCeConnection
            mv_oConn.ConnectionString = mv_strConnectionString
            mv_oConn.Open()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function UpdateData(ByVal pv_strMessage As String, ByVal pv_strTable As String) As Long
        Try
            Dim v_xmlDocument As New XmlDocumentEx
            Dim v_nodeList As Xml.XmlNodeList
            v_xmlDocument.LoadXml(pv_strMessage)
            v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/" & pv_strTable)

            Dim v_strSQL As String

            v_strSQL = "DELETE FROM " & pv_strTable
            ExecuteNonQuery(v_strSQL)

            v_strSQL = "SELECT * FROM " & pv_strTable
            Dim v_oDA As New SqlCeDataAdapter(v_strSQL, mv_oConn)

            Dim v_ds As New DataSet("VSDS")
            v_oDA.FillSchema(v_ds, SchemaType.Source, pv_strTable)
            v_oDA.Fill(v_ds, pv_strTable)

            Dim v_tbl As DataTable
            v_tbl = v_ds.Tables(pv_strTable)

            Dim v_oRow As DataRow
            Dim v_strValue, v_strFldName, v_strFldType As String
            For i As Integer = 0 To v_nodeList.Count - 1
                v_oRow = v_tbl.NewRow()
                For j As Integer = 0 To v_nodeList.Item(i).ChildNodes.Count - 1
                    With v_nodeList.Item(i).ChildNodes(j)
                        v_strValue = .InnerText.ToString
                        v_strFldName = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                        v_strFldType = CStr(CType(.Attributes.GetNamedItem("fldtype"), Xml.XmlAttribute).Value)
                    End With

                    If pv_strTable.ToUpper = "RPREPORTS" And v_strFldName.ToUpper = "RPTCMDSQL" Then
                    Else
                        If v_strFldType = "System.String" Or v_strFldType = "System.DateTime" Then
                            v_oRow(v_strFldName) = gf_CorrectStringField(v_strValue)
                        Else
                            If v_strValue = "" Then v_strValue = "0"
                            v_oRow(v_strFldName) = gf_CorrectNumericField(v_strValue)
                        End If
                    End If
                Next
                v_tbl.Rows.Add(v_oRow)
            Next
            Dim objCommandBuilder As New SqlCeCommandBuilder(v_oDA)
            v_oDA.Update(v_ds, pv_strTable)

            v_ds.Dispose()
            v_oDA.Dispose()
            Return ERR_SYSTEM_OK
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ExecuteReturnDataSet(ByVal pv_strSQL As String) As DataSet
        Try
            Dim v_oDA As New SqlCeDataAdapter(pv_strSQL, mv_oConn)
            Dim v_ds As New DataSet
            v_oDA.Fill(v_ds)
            v_oDA.Dispose()
            Return v_ds
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ExecuteNonQuery(ByVal pv_strSQL As String) As Integer
        Try
            Dim v_int As Integer
            Dim v_oCmd As SqlCeCommand
            v_oCmd = New SqlCeCommand(pv_strSQL, mv_oConn)
            v_int = v_oCmd.ExecuteNonQuery()
            v_oCmd.Dispose()
            Return v_int
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Sub UpdateVersion(ByVal pv_strTable As String, ByVal pv_strVerstion As String)
        Try
            Dim v_strSQL As String
            v_strSQL = "UPDATE TBLVERSION SET TBLVERSION = '" & pv_strVerstion & "' WHERE TBLNAME='" & pv_strTable & "'"
            ExecuteNonQuery(v_strSQL)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Public Sub CloseConnection()
        Try
            mv_oConn.Close()
            mv_oConn = Nothing
            Dim v_oApp As New ApplicationServices.ApplicationBase
            Dim v_strPath As String = v_oApp.Info.DirectoryPath
            EncryptFile(v_strPath & "\UserData\" & mv_strFileName & ".sdf", v_strPath & "\UserData\" & mv_strFileName & ".enc", mv_strTLName)
            System.IO.File.Delete(v_strPath & "\UserData\" & mv_strFileName & ".sdf")
        Catch ex As Exception

        End Try
    End Sub
End Class
