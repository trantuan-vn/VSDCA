Imports Sats.CommonLibrary
Imports Sats.DataAccessLayer
Imports System.Reflection

Public Class Form1
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Try
            Dim v_Stream As New System.IO.StreamReader(txtFilePath.Text)
            Dim v_strLine As String
            Dim v_strConfirmNo, v_strMatch_Date, v_strMatch_Time, v_strSec_Code, v_strSET_TYPE As String
            Dim v_strQty, v_strPrice, v_strB_ACC_NO, v_strS_ACC_NO, v_strBlock_Tran As String
            Dim v_strB_CODE_TRADE, v_strS_CODE_TRADE, v_strB_ORDER_NO, v_strS_ORDER_NO, v_strB_PC_PLAG, v_strS_PC_PLAG As String
            Dim v_strStatus As String

            Dim strRows As String = ""

            v_strBlock_Tran = "0"
            v_strSET_TYPE = "3"
            Dim v_obj As New DataAccess
            v_obj.NewDBInstance(gc_MODULE_HOST)
            v_obj.ExecuteNonQuery(CommandType.Text, "TRUNCATE TABLE txfields_astdl")
            Dim v_ds As DataSet = v_obj.ExecuteSQLReturnDataset(CommandType.Text, "SELECT * FROM txfields_astdl WHERE 0=1")
            Dim v_int As Integer = 0
            While Not v_Stream.EndOfStream
                Dim v_row As DataRow = v_ds.Tables(0).NewRow()
                v_row("COL_VALUE01") = "00000000"
                v_row("COL_TYPE01") = "C"
                v_row("COL_DESC01") = "00000000"
                v_row("COL_VALUE02") = "17/05/2021"
                v_row("COL_TYPE02") = "D"
                v_row("COL_DESC02") = "17/05/2021"
                v_row("COL_VALUE03") = "17/05/2021"
                v_row("COL_TYPE03") = "D"
                v_row("COL_DESC03") = "17/05/2021"

                v_strLine = v_Stream.ReadLine
                If v_strLine <> "" Then
                    v_strStatus = Trim(Mid(v_strLine, 90, 2))
                    If (v_strStatus <> "XC") And (v_strStatus <> "XS") And (v_strStatus <> "") Then
                    Else
                        v_strConfirmNo = String.Format("{0:000000}", CInt(Mid(v_strLine, 1, 6)))
                        v_row("COL_VALUE04") = v_strConfirmNo.Trim
                        v_row("COL_TYPE04") = "C"
                        v_row("COL_DESC04") = v_strConfirmNo.Trim

                        v_strMatch_Time = Mid(v_strLine, 55, 8)
                        v_row("COL_VALUE05") = v_strMatch_Time.Trim
                        v_row("COL_TYPE05") = "C"
                        v_row("COL_DESC05") = v_strMatch_Time.Trim

                        v_strMatch_Date = Mid(v_strLine, 63, 10)
                        v_row("COL_VALUE06") = v_strMatch_Date
                        v_row("COL_TYPE06") = "C"
                        v_row("COL_DESC06") = v_strMatch_Date

                        v_strSec_Code = Mid(v_strLine, 92, 8)
                        v_row("COL_VALUE07") = v_strSec_Code.Trim
                        v_row("COL_TYPE07") = "C"
                        v_row("COL_DESC07") = v_strSec_Code.Trim

                        v_strPrice = CStr(CDbl(Mid(v_strLine, 108, 9)) * 1000)
                        v_row("COL_VALUE08") = v_strPrice.Trim
                        v_row("COL_TYPE08") = "N"
                        v_row("COL_DESC08") = v_strPrice.Trim

                        v_strQty = Mid(v_strLine, 100, 8)
                        v_row("COL_VALUE09") = v_strQty.Trim
                        v_row("COL_TYPE09") = "N"
                        v_row("COL_DESC09") = v_strQty.Trim

                        v_strB_ACC_NO = Mid(v_strLine, 117, 10)
                        v_row("COL_VALUE10") = v_strB_ACC_NO.Trim
                        v_row("COL_TYPE10") = "C"
                        v_row("COL_DESC10") = v_strB_ACC_NO.Trim

                        v_strS_ACC_NO = Mid(v_strLine, 127, 10)
                        v_row("COL_VALUE11") = v_strS_ACC_NO.Trim
                        v_row("COL_TYPE11") = "C"
                        v_row("COL_DESC11") = v_strS_ACC_NO.Trim

                        v_strB_CODE_TRADE = Mid(v_strLine, 83, 3)
                        v_row("COL_VALUE14") = v_strB_CODE_TRADE.Trim
                        v_row("COL_TYPE14") = "C"
                        v_row("COL_DESC14") = v_strB_CODE_TRADE.Trim

                        v_strS_CODE_TRADE = Mid(v_strLine, 86, 3)
                        v_row("COL_VALUE15") = v_strS_CODE_TRADE.Trim
                        v_row("COL_TYPE15") = "C"
                        v_row("COL_DESC15") = v_strS_CODE_TRADE.Trim

                        v_strB_ORDER_NO = Mid(v_strLine, 7, 8)
                        v_row("COL_VALUE16") = v_strB_ORDER_NO.Trim
                        v_row("COL_TYPE16") = "C"
                        v_row("COL_DESC16") = v_strB_ORDER_NO.Trim

                        v_strS_ORDER_NO = Mid(v_strLine, 25, 8)
                        v_row("COL_VALUE17") = v_strS_ORDER_NO.Trim
                        v_row("COL_TYPE17") = "C"
                        v_row("COL_DESC17") = v_strS_ORDER_NO.Trim

                        v_strB_PC_PLAG = Mid(v_strLine, 81, 1)
                        v_row("COL_VALUE18") = v_strB_PC_PLAG.Trim
                        v_row("COL_TYPE18") = "C"
                        v_row("COL_DESC18") = v_strB_PC_PLAG.Trim

                        v_strS_PC_PLAG = Mid(v_strLine, 82, 1)
                        v_row("COL_VALUE19") = v_strS_PC_PLAG.Trim
                        v_row("COL_TYPE19") = "C"
                        v_row("COL_DESC19") = v_strS_PC_PLAG.Trim

                        v_row("COL_VALUE12") = v_strSET_TYPE.Trim
                        v_row("COL_TYPE12") = "N"
                        v_row("COL_DESC12") = v_strSET_TYPE.Trim

                        v_row("COL_VALUE13") = v_strBlock_Tran.Trim
                        v_row("COL_TYPE13") = "N"
                        v_row("COL_DESC13") = v_strBlock_Tran.Trim

                        v_row("TLTXCD") = "4084"
                        v_row("REAL_ROW") = v_int

                        v_ds.Tables(0).Rows.Add(v_row)
                        v_int = v_int + 1
                    End If
                End If
            End While
            If v_ds.Tables(0).Rows.Count > 0 And v_obj.SaveUsingOracleBulkCopy("txfields_astdl", v_ds.Tables(0)) Then
                MessageBox.Show("Read file successfull.")
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Lay tham so database
        Dim asb As Assembly
        Dim assamblyName As AssemblyName = AssemblyName.GetAssemblyName(AppDomain.CurrentDomain.BaseDirectory & "Sats.DBConfig.dll")
        Dim myDomain As AppDomain = AppDomain.CreateDomain("HOST")
        asb = myDomain.Load(assamblyName)
        Dim dbConfig As IDBConfig = CType(asb.CreateInstance("Sats.DBConfig.DBConfig"), IDBConfig)
        GlobalDataConfig.HOST_DBCONFIG = dbConfig.GetHostConfig()
        GlobalDataConfig.INQUERY_DBCONFIG = dbConfig.GetInQueryConfig()
        AppDomain.Unload(myDomain)
    End Sub
End Class
