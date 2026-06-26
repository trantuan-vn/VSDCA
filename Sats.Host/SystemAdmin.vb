Imports Sats.CommonLibrary
Imports System.IO
Imports Sats.CoreBusiness
Imports Sats.DataAccessLayer
'Imports System.EnterpriseServices
Imports System.Runtime.InteropServices
Imports BkavCASign
Imports Sats.ServerCA



Public Structure SYSTEMTIME
    Public wYear As UInt16
    Public wMonth As UInt16
    Public wDayOfWeek As UInt16
    Public wDay As UInt16
    Public wHour As UInt16
    Public wMinute As UInt16
    Public wSecond As UInt16
    Public wMilliseconds As UInt16
End Structure

'<JustInTimeActivation(False), _
'Transaction(TransactionOption.Disabled), _
'ObjectPooling(Enabled:=True, MinPoolSize:=30)> _
Public Class SystemAdmin
    Implements IDisposable
    'Inherits ServicedComponent

    Public st As New SYSTEMTIME
    <DllImport("kernel32.dll")> _
    Public Shared Sub SetLocalTime(ByRef lpSystemTime As SYSTEMTIME)
    End Sub

    <DllImport("kernel32.dll")> _
    Public Shared Sub GetLocalTime(ByRef lpSystemTime As SYSTEMTIME)
    End Sub

    Public Function BranchExecute(ByRef pv_strObjMsg As String) As Long
        Dim v_lngErrCode As Long = ERR_SYSTEM_OK
        Dim v_strErrorSource As String = "Host.SystemAdmin.BranchExecute", v_strErrorMessage As String = ""
        Dim v_strSQL As String, v_DataAccess As New DataAccess, v_obj As txRouter
        Dim v_nodeList As Xml.XmlNodeList, v_xmlDocument As New Xml.XmlDocument
        Dim v_strBCHMDL, v_strAPPTYPE, v_strBATCHNAME As String, i, j, intPos As Integer

        Try
            v_DataAccess.NewDBInstance(gc_MODULE_HOST)
            v_xmlDocument.LoadXml(pv_strObjMsg)
            Dim v_strFillter As String = Trim(v_xmlDocument.DocumentElement.Attributes(gc_AtributeCLAUSE).Value.ToString)
            Dim v_attrColl As Xml.XmlAttributeCollection = v_xmlDocument.DocumentElement.Attributes
            Dim v_IntCurrRow As Integer = CInt(Trim(v_xmlDocument.DocumentElement.Attributes(gc_AtributeREFERENCE).Value.ToString))
            'Kiá»ƒm tra chá»‰ cho phÃ©p cháº¡y Batch náº¿u HOST Ä‘ang á»Ÿ tráº¡ng thÃ¡i OPERATION_INACTIVE
            Dim v_strSYSVAR As String = ""
            v_lngErrCode = v_DataAccess.GetSysVar("SYSTEM", "HOSTATUS", v_strSYSVAR)
            If v_lngErrCode <> ERR_SYSTEM_OK Then
                'ContextUtil.SetAbort()
                Return v_lngErrCode
            End If
            If v_strSYSVAR <> OPERATION_INACTIVE Then
                'ContextUtil.SetAbort()
                Return ERR_SA_HOST_OPERATION_STILLACTIVE
            Else
                'Thá»±c hiá»‡n cÃ¡c bÆ°á»›c cháº¡y Batch theo danh sÃ¡ch Ä‘Æ°á»£c gá»­i

                Dim v_intMaxRow As Integer = 0
                Dim v_objDataAccess As New DataAccess
                v_objDataAccess.NewDBInstance(gc_MODULE_HOST)
                v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")
                '-->Kiem tra
                'Tao bien chay batch va cap nhat vao Sysvar: batchstatus
                'Neu: batchstatus=1: dang chay batch
                'Neu: batchstatus=0: hoan tat chay batch
                v_lngErrCode = v_DataAccess.SetSysVar("SYSTEM", "BATCHSTATUS", OPERATION_ACTIVE)
                If v_nodeList.Count > 0 Then
                    For i = 0 To v_nodeList.Count - 1
                        For j = 0 To v_nodeList.Item(i).ChildNodes.Count - 1
                            With v_nodeList.Item(i).ChildNodes(j)
                                'Láº¥y tÃªn BATCHNAME cáº§n cháº¡y
                                v_strBATCHNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                                intPos = InStr(v_strBATCHNAME, ".")
                                v_strAPPTYPE = Mid(v_strBATCHNAME, 1, intPos - 1)
                                v_strBCHMDL = Mid(v_strBATCHNAME, intPos + 1)

                                'Ä?Ã¡nh dáº¥u xoÃ¡ cÃ¡c giao dá»‹ch cÅ©
                                v_strSQL = "UPDATE TLLOG SET DELTD='Y' WHERE TXSTATUS <> '1' AND RTRIM(BATCHNAME)='" & v_strBCHMDL.Trim & "'"
                                v_DataAccess.NewDBInstance(gc_MODULE_HOST)
                                v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
                                'Thá»±c hiá»‡n cháº¡y Batch
                                v_obj = New txRouter
                                v_lngErrCode = v_obj.Batch(v_strAPPTYPE, v_strBCHMDL, v_strFillter, v_intMaxRow)
                                v_xmlDocument.DocumentElement.Attributes(gc_AtributeREFERENCE).Value = v_intMaxRow.ToString
                                pv_strObjMsg = v_xmlDocument.InnerXml
                                If v_lngErrCode <> ERR_SYSTEM_OK Then
                                    'ContextUtil.SetAbort()
                                    'Neu chay batch co loi thi phai update lai trang thai cua batchstatus la 0
                                    v_lngErrCode = v_DataAccess.SetSysVar("SYSTEM", "BATCHSTATUS", OPERATION_INACTIVE)
                                    Return v_lngErrCode
                                Else
                                    'Cáº­p nháº­t bÆ°á»›c cháº¡y batch thÃ nh cÃ´ng
                                    If v_IntCurrRow = -1 Then
                                        'KhÃ´ng phÃ¢n trang
                                        v_strSQL = "UPDATE SBBATCHSTS SET BCHSTS = 'Y', CMPLTIME = SYSDATE,BCHSUCPAGE=" & v_IntCurrRow & " WHERE UPPER(BCHMDL) = '" & v_strBCHMDL & "' AND BCHDATE=(SELECT MAX(BCHDATE) FROM SBBATCHSTS) "
                                        v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
                                    Else
                                        'PhÃ¢n trang
                                        If v_IntCurrRow >= v_intMaxRow Then
                                            'LÃ  trang cuá»‘i
                                            v_strSQL = "UPDATE SBBATCHSTS SET BCHSTS = 'Y', CMPLTIME = SYSDATE,BCHSUCPAGE=" & v_IntCurrRow & " WHERE UPPER(BCHMDL) = '" & v_strBCHMDL & "' AND BCHDATE=(SELECT MAX(BCHDATE) FROM SBBATCHSTS) "
                                            v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
                                        Else
                                            'KhÃ´ng lÃ  trang cuá»‘i
                                            v_strSQL = "UPDATE SBBATCHSTS SET BCHSUCPAGE=" & v_IntCurrRow & " WHERE UPPER(BCHMDL) = '" & v_strBCHMDL & "' AND BCHDATE=(SELECT MAX(BCHDATE) FROM SBBATCHSTS) "
                                            v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
                                        End If
                                    End If
                                End If
                            End With
                        Next
                    Next
                End If
                'Neu ko thuc hien buoc batch nao thi cung cap nhat lai batchstatus=0
                v_lngErrCode = v_DataAccess.SetSysVar("SYSTEM", "BATCHSTATUS", OPERATION_INACTIVE)
            End If
            'ContextUtil.SetComplete()
            Return v_lngErrCode
        Catch ex As Exception
            'ContextUtil.SetAbort()
            'LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
            '             & "Error code: " & v_lngErrCode.ToString() & vbNewLine _
            '             & "Error message: " & v_strErrorMessage, EventLogEntryType.Information)
            'Neu ko thuc hien buoc batch nao thi cung cap nhat lai batchstatus=0
            v_lngErrCode = v_DataAccess.SetSysVar("SYSTEM", "BATCHSTATUS", OPERATION_INACTIVE)
            Throw ex
        End Try
    End Function
    Public Function BranchDeActive(ByRef pv_strObjMsg As String) As Long
        Dim v_lngErrCode As Long = ERR_SYSTEM_OK
        Dim v_strErrorSource As String = "Host.SystemAdmin.BranchDeActive", v_strErrorMessage As String = ""
        Dim v_strSQL As String, v_DataAccess As New DataAccess
        Dim v_xmlDocument As New Xml.XmlDocument
        Try
            v_DataAccess.NewDBInstance(gc_MODULE_HOST)
            v_xmlDocument.LoadXml(pv_strObjMsg)
            Dim v_attrColl As Xml.XmlAttributeCollection = v_xmlDocument.DocumentElement.Attributes
            Dim v_strBRID As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeBRID), Xml.XmlAttribute).Value)
            v_strSQL = "UPDATE BRGRP SET STATUS='" & BRGRP_CLOSED & "' WHERE TRIM(BRID)='" & Trim(v_strBRID) & "'"
            v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
            'ContextUtil.SetComplete()
            Return v_lngErrCode
        Catch ex As Exception
            'ContextUtil.SetAbort()
            'LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
            '             & "Error code: " & v_lngErrCode.ToString() & vbNewLine _
            '             & "Error message: " & v_strErrorMessage, EventLogEntryType.Information)
            Throw ex
        End Try
    End Function

    Public Function BranchActive(ByRef pv_strObjMsg As String) As Long
        Dim v_lngErrCode As Long = ERR_SYSTEM_OK
        Dim v_strErrorSource As String = "Host.SystemAdmin.BranchActive", v_strErrorMessage As String = ""
        Dim v_strSQL As String, v_DataAccess As New DataAccess
        Dim v_xmlDocument As New Xml.XmlDocument
        Try
            v_DataAccess.NewDBInstance(gc_MODULE_HOST)
            v_xmlDocument.LoadXml(pv_strObjMsg)
            Dim v_attrColl As Xml.XmlAttributeCollection = v_xmlDocument.DocumentElement.Attributes
            Dim v_strBRID As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeBRID), Xml.XmlAttribute).Value)
            Dim v_strSYSVAR As String = ""

            'Kiá»ƒm tra chá»‰ cho phÃ©p Active branch náº¿u HOST Ä‘Ã£ Active
            v_lngErrCode = v_DataAccess.GetSysVar("SYSTEM", "HOSTATUS", v_strSYSVAR)
            If v_lngErrCode <> ERR_SYSTEM_OK Then
                'ContextUtil.SetAbort()
                Return v_lngErrCode
            End If
            If v_strSYSVAR = OPERATION_INACTIVE Then
                'ContextUtil.SetAbort()
                Return ERR_SY_HOST_OPERATION_ISINACTIVE
            Else
                'Cáº­p nháº­t láº¡i tráº¡ng thÃ¡i cá»§a chi nhÃ¡nh
                v_strSQL = "UPDATE BRGRP SET STATUS='" & BRGRP_ACTIVE & "' WHERE TRIM(BRID)='" & Trim(v_strBRID) & "'"
                v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
                'Tráº£ vá»? ngÃ y lÃ m viá»‡c hiá»‡n táº¡i cá»§a há»‡ thá»‘ng
                v_lngErrCode = v_DataAccess.GetSysVar("SYSTEM", "CURRDATE", v_strSYSVAR)
                If v_lngErrCode <> ERR_SYSTEM_OK Then
                    'ContextUtil.SetAbort()
                    Return v_lngErrCode
                End If
                CType(v_attrColl.GetNamedItem(gc_AtributeREFERENCE), Xml.XmlAttribute).Value = v_strSYSVAR
                pv_strObjMsg = v_xmlDocument.InnerXml
            End If

            'ContextUtil.SetComplete()
            Return v_lngErrCode
        Catch ex As Exception
            'ContextUtil.SetAbort()
            'LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
            '             & "Error code: " & v_lngErrCode.ToString() & vbNewLine _
            '             & "Error message: " & v_strErrorMessage, EventLogEntryType.Information)
            Throw ex
        End Try

    End Function

    Public Function HostDeActive(ByRef pv_strObjMsg As String) As Long
        Dim v_lngErrCode As Long = ERR_SYSTEM_OK
        Dim v_strErrorSource As String = "Host.SystemAdmin.HostDeActive", v_strErrorMessage As String = ""
        Dim v_strSQL, v_strREFERENCE As String, v_ds As DataSet, v_DataAccess As New DataAccess

        Try
            v_DataAccess.NewDBInstance(gc_MODULE_HOST)
            Dim v_xmlDocument As New Xml.XmlDocument
            v_xmlDocument.LoadXml(pv_strObjMsg)
            Dim v_attrColl As Xml.XmlAttributeCollection = v_xmlDocument.DocumentElement.Attributes

            'Kiá»ƒm tra khÃ´ng cho phÃ©p DeActive HeadOffice náº¿u cÃ¡c Branch váº«n chÆ°a DeActive háº¿t
            v_strSQL = "SELECT BRID, BRNAME FROM BRGRP WHERE TRIM(STATUS)='" & BRGRP_ACTIVE & "'"
            v_ds = v_DataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            If v_ds.Tables(0).Rows.Count > 0 Then
                v_strREFERENCE = vbNullString
                For i As Integer = 0 To v_ds.Tables(0).Rows.Count - 1 Step 1
                    v_strREFERENCE = v_strREFERENCE & "@" & Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(i)("BRID")))
                Next
                CType(v_attrColl.GetNamedItem(gc_AtributeREFERENCE), Xml.XmlAttribute).Value = v_strREFERENCE
                pv_strObjMsg = v_xmlDocument.InnerXml
                'ContextUtil.SetAbort()
                Return ERR_SY_STILLHAS_BRGRP_ACTIVE
            End If
            'Thá»±c hiá»‡n DeActive
            Dim v_strSYSVAR As String = ""
            v_lngErrCode = v_DataAccess.GetSysVar("SYSTEM", "HOSTATUS", v_strSYSVAR)
            If v_lngErrCode <> ERR_SYSTEM_OK Then Return v_lngErrCode
            If v_strSYSVAR = OPERATION_INACTIVE Then
                'ContextUtil.SetAbort()
                Return ERR_SY_HOST_OPERATION_ISINACTIVE
            Else
                'Cáº­p nháº­t tráº¡ng thÃ¡i á»Ÿ BDS
                v_lngErrCode = v_DataAccess.SetSysVar("SYSTEM", "HOSTATUS", OPERATION_INACTIVE)
            End If
            If v_lngErrCode <> ERR_SYSTEM_OK Then
                'ContextUtil.SetAbort()
            Else
                'ContextUtil.SetComplete()
            End If
            Return v_lngErrCode
        Catch ex As Exception
            'ContextUtil.SetAbort()
            'LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
            '             & "Error code: " & v_lngErrCode.ToString() & vbNewLine _
            '             & "Error message: " & v_strErrorMessage, EventLogEntryType.Information)
            Throw ex
        End Try
    End Function

    Public Function HostActive(ByRef pv_strObjMsg As String) As Long
        Dim v_lngErrCode As Long = ERR_SYSTEM_OK
        Dim v_strErrorSource As String = "Host.SystemAdmin.HostActive", v_strErrorMessage As String = ""
        Dim v_DataAccess As New DataAccess

        Try
            v_DataAccess.NewDBInstance(gc_MODULE_HOST)
            Dim v_strSYSVAR As String = ""
            v_lngErrCode = v_DataAccess.GetSysVar("SYSTEM", "HOSTATUS", v_strSYSVAR)
            If v_lngErrCode <> ERR_SYSTEM_OK Then
                'ContextUtil.SetAbort()
                Return v_lngErrCode
            End If
            If v_strSYSVAR = OPERATION_ACTIVE Then
                'ContextUtil.SetAbort()
                Return ERR_SY_HOST_OPERATION_STILLACTIVE
            Else
                'Cáº­p nháº­t tráº¡ng thÃ¡i á»Ÿ HeadOffice
                v_lngErrCode = v_DataAccess.SetSysVar("SYSTEM", "HOSTATUS", OPERATION_ACTIVE)

                'Cáº­p nháº­t láº¡i tráº¡ng thÃ¡i cho cac bÆ°á»›c Batch khÃ´ng Ä‘Æ°á»£c cháº¡y
                Dim v_strCURRDATE As String = ""
                v_lngErrCode = v_DataAccess.GetSysVar("SYSTEM", "CURRDATE", v_strCURRDATE)
                If v_lngErrCode <> ERR_SYSTEM_OK Then
                    'ContextUtil.SetAbort()
                    Return v_lngErrCode
                End If
                Dim v_obj As New DataAccess
                v_obj.NewDBInstance(gc_MODULE_HOST)
                Dim v_strSQL As String
                v_strSQL = "UPDATE SBBATCHSTS SET BCHSTS='N' WHERE BCHSTS = ' ' AND BCHDATE < TO_DATE('" & v_strCURRDATE & "','" & gc_FORMAT_DATE & "')"
                v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

            End If
            If v_lngErrCode <> ERR_SYSTEM_OK Then
                'ContextUtil.SetAbort()
            Else
                'ContextUtil.SetComplete()
            End If
            Return v_lngErrCode
        Catch ex As Exception
            'ContextUtil.SetAbort()
            'LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
            '             & "Error code: " & v_lngErrCode.ToString() & vbNewLine _
            '             & "Error message: " & v_strErrorMessage, EventLogEntryType.Information)
            Throw ex
        End Try
    End Function

    Public Function GetInventory(ByRef pv_strObjMsg As String) As Long
        Dim v_lngErrCode As Long = ERR_SYSTEM_OK
        Dim v_strErrorSource As String = "Host.SystemAdmin.GetInventory"
        Dim v_xmlMessage As New Xml.XmlDocument
        Dim v_strSQL As String = "", XMLDocument As New Xml.XmlDocument
        Dim v_DataAccess As New DataAccess, v_ds As DataSet
        Dim v_strSYSVAR As String = ""

        Try
            v_DataAccess.NewDBInstance(gc_MODULE_HOST)
            v_lngErrCode = v_DataAccess.GetSysVar("SYSTEM", "COMPANYCD", v_strSYSVAR)

            XMLDocument.LoadXml(pv_strObjMsg)
            Dim v_attrColl As Xml.XmlAttributeCollection = XMLDocument.DocumentElement.Attributes
            'Ä?Ã¢y lÃ  tÃªn cá»§a Inventory
            Dim v_strBRID As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeBRID), Xml.XmlAttribute).Value)
            Dim v_strClause As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCLAUSE), Xml.XmlAttribute).Value)

            Select Case v_strClause
                Case "CUSTID"
                    'Láº¥y mÃ£ khÃ¡ch hÃ ng cÃ²n cÃ³ thá»ƒ sá»­ dá»¥ng cá»§a tá»«ng BRID
                    v_strSQL = "SELECT SUBSTR(INVACCT,1,4), MAX(ODR)+1 AUTOINV FROM " & ControlChars.CrLf _
                        & "(SELECT ROWNUM ODR, INVACCT " & ControlChars.CrLf _
                        & "FROM (SELECT CUSTID INVACCT FROM CFMAST WHERE SUBSTR(CUSTID,1,4)='" & v_strBRID & "' ORDER BY CUSTID) DAT " & ControlChars.CrLf _
                        & "WHERE TO_NUMBER(SUBSTR(INVACCT,5,6))=ROWNUM) INVTAB " & ControlChars.CrLf _
                        & "GROUP BY SUBSTR(INVACCT,1,4)"

                Case "CUSTODYCD"
                    'Láº¥y sá»‘ tÃ i khoáº£n lÆ°u kÃ½ cÃ²n cÃ³ thá»ƒ sá»­ dá»¥ng theo mÃ£ cá»§a cÃ´ng ty
                    v_strSQL = "SELECT SUBSTR(INVACCT,1,4), MAX(ODR)+1 AUTOINV FROM " & ControlChars.CrLf _
                        & "(SELECT ROWNUM ODR, INVACCT " & ControlChars.CrLf _
                        & "FROM (SELECT CUSTODYCD INVACCT FROM CFMAST WHERE SUBSTR(CUSTODYCD,1,3)='" & v_strSYSVAR & "' AND TRIM(TO_CHAR(TRANSLATE(SUBSTR(CUSTODYCD,5,6),'0123456789',' '))) IS NULL ORDER BY CUSTODYCD) DAT " & ControlChars.CrLf _
                        & "WHERE TO_NUMBER(SUBSTR(INVACCT,5,6))=ROWNUM) INVTAB " & ControlChars.CrLf _
                        & "GROUP BY SUBSTR(INVACCT,1,4)"

                Case "AFACCTNO"
                    'Láº¥y sá»‘ há»£p Ä‘á»“ng cÃ²n cÃ³ thá»ƒ sá»­ dá»¥ng cá»§a tá»«ng BRID
                    v_strSQL = "SELECT SUBSTR(INVACCT,1,4), MAX(ODR)+1 AUTOINV FROM " & ControlChars.CrLf _
                        & "(SELECT ROWNUM ODR, INVACCT " & ControlChars.CrLf _
                        & "FROM (SELECT ACCTNO INVACCT FROM AFMAST WHERE SUBSTR(ACCTNO,1,4)='" & v_strBRID & "' ORDER BY ACCTNO) DAT " & ControlChars.CrLf _
                        & "WHERE TO_NUMBER(SUBSTR(INVACCT,5,6))=ROWNUM) INVTAB " & ControlChars.CrLf _
                        & "GROUP BY SUBSTR(INVACCT,1,4)"

            End Select

            'Kiá»ƒm tra Sequence Ä‘Ã£ tá»“n táº¡i chÆ°a
            v_ds = v_DataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            If v_ds.Tables(0).Rows.Count = 0 Then
                CType(v_attrColl.GetNamedItem(gc_AtributeCLAUSE), Xml.XmlAttribute).Value = "0"
            Else
                CType(v_attrColl.GetNamedItem(gc_AtributeCLAUSE), Xml.XmlAttribute).Value = CStr(v_ds.Tables(0).Rows(0)("AUTOINV"))
            End If

            'Tráº£ vá»? káº¿t quáº£
            pv_strObjMsg = XMLDocument.InnerXml
            'ContextUtil.SetComplete()
            Return 0
        Catch ex As Exception
            'ContextUtil.SetAbort()
            'LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
            '             & "Error code: " & v_strSQL & vbNewLine _
            '             & "Error message: " & ex.Message, EventLogEntryType.Error)
            Throw ex
        End Try
    End Function

    Public Function GetSystemTime(ByRef pv_strObjMsg As String) As Long
        Dim v_lngErrCode As Long = ERR_SYSTEM_OK
        Dim v_strErrorSource As String = "Host.SystemAdmin.GetSystemTime", v_strErrorMessage As String = ""
        Dim v_strSQL As String, v_DataAccess As New DataAccess, v_ds As DataSet
        Dim v_xmlDocument As New Xml.XmlDocument
        Try

            'GetLocalTime(st)
            v_DataAccess.NewDBInstance(gc_MODULE_HOST)
            v_strSQL = "SELECT TO_CHAR(SYSDATE,'HH:MI:SS') TXTIME FROM DUAL"
            v_ds = v_DataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

            v_xmlDocument.LoadXml(pv_strObjMsg)

            If v_ds.Tables(0).Rows.Count > 0 Then
                v_xmlDocument.DocumentElement.Attributes(gc_AtributeCLAUSE).Value = v_ds.Tables(0).Rows(0)("TXTIME")
            Else
                v_xmlDocument.DocumentElement.Attributes(gc_AtributeCLAUSE).Value = "00:00:00"
            End If

            pv_strObjMsg = v_xmlDocument.InnerXml

            'ContextUtil.SetComplete()
            Return v_lngErrCode
        Catch ex As Exception
            'ContextUtil.SetAbort()
            'LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
            '             & "Error code: " & v_lngErrCode.ToString() & vbNewLine _
            '             & "Error message: " & v_strErrorMessage, EventLogEntryType.Information)
            Throw ex
        End Try
    End Function
    Public Function UPDATE_ROOM_STATUS(ByRef pv_strObjMsg As String) As Long
        Dim v_lngErrCode As Long = ERR_SYSTEM_OK
        Dim v_xmlDocument As New Xml.XmlDocument
        Dim v_DataAccess As New DataAccess
        Try
            v_DataAccess.NewDBInstance(gc_MODULE_HOST)
            v_xmlDocument.LoadXml(pv_strObjMsg)
            Dim v_strBRID As String = Trim(v_xmlDocument.DocumentElement.Attributes(gc_AtributeBRID).Value.ToString)
            v_DataAccess.SetSysVar("SYSTEM", "room_status", v_strBRID, OPERATION_ACTIVE)
            Return 0
        Catch ex As Exception
            v_DataAccess.Rollback()
            Return v_lngErrCode
            Throw ex
        End Try
    End Function
    Public Function UPDATE_ROOM_STATUS1(ByRef pv_strObjMsg As String) As Long
        Dim v_lngErrCode As Long = ERR_SYSTEM_OK
        Dim v_xmlDocument As New Xml.XmlDocument
        Dim v_DataAccess As New DataAccess
        Try
            v_DataAccess.NewDBInstance(gc_MODULE_HOST)
            v_xmlDocument.LoadXml(pv_strObjMsg)
            Dim v_strBRID As String = Trim(v_xmlDocument.DocumentElement.Attributes(gc_AtributeBRID).Value.ToString)
            v_DataAccess.SetSysVar("SYSTEM", "room_status", v_strBRID, OPERATION_STANDBY)
            Return 0
        Catch ex As Exception
            v_DataAccess.Rollback()
            Return v_lngErrCode
            Throw ex
        End Try
    End Function
    Public Function UPDATE_ROOM_STATUS2(ByRef pv_strObjMsg As String) As Long
        Dim v_lngErrCode As Long = ERR_SYSTEM_OK
        Dim v_xmlDocument As New Xml.XmlDocument
        Dim v_DataAccess As New DataAccess
        Try
            v_DataAccess.NewDBInstance(gc_MODULE_HOST)
            v_xmlDocument.LoadXml(pv_strObjMsg)
            Dim v_strBRID As String = Trim(v_xmlDocument.DocumentElement.Attributes(gc_AtributeBRID).Value.ToString)
            v_DataAccess.SetSysVar("SYSTEM", "room_status", v_strBRID, OPERATION_ACTIVE)
            Dim v_oSignServer As SignServer = New SignServer
            'ServerCA.ServerBussinessCA.OpenHSM(v_oSignServer)
            Dim v_intSlotId = Integer.Parse(System.Configuration.ConfigurationManager.AppSettings("HSMSlotId"))
            Dim v_strPIN = System.Configuration.ConfigurationManager.AppSettings("HSMPin")
            Dim v_strPublicKeyName = System.Configuration.ConfigurationManager.AppSettings("HSMPublicKeyName")
            Dim v_strPrivateKeyName = System.Configuration.ConfigurationManager.AppSettings("HSMPrivateKeyName")
            Dim v_strCertificateName = System.Configuration.ConfigurationManager.AppSettings("HSMCertificateName")
            Dim v_strHsmDllName = System.Configuration.ConfigurationManager.AppSettings("HSMDllName")

            ServerBussinessCA.OpenHSM(v_oSignServer, v_intSlotId, v_strPIN, _
                                      v_strPublicKeyName, v_strPrivateKeyName, _
                                      v_strCertificateName, v_strHsmDllName)
            pv_strObjMsg = ServerCA.ServerBussinessCA.CombineData(pv_strObjMsg, v_oSignServer)
            Return 0
        Catch ex As Exception
            v_DataAccess.Rollback()
            Return v_lngErrCode
            Throw ex
        End Try
    End Function
    Public Function UPDATE_BANK_STATUS(ByRef pv_strObjMsg As String) As Long
        Dim v_lngErrCode As Long = ERR_SYSTEM_OK
        Dim v_xmlDocument As New Xml.XmlDocument
        Dim v_DataAccess As New DataAccess
        Try
            v_DataAccess.NewDBInstance(gc_MODULE_HOST)
            v_xmlDocument.LoadXml(pv_strObjMsg)
            Dim v_strBRID As String = Trim(v_xmlDocument.DocumentElement.Attributes(gc_AtributeBRID).Value.ToString)
            Dim v_str_intFrequence As String = Trim(v_xmlDocument.DocumentElement.Attributes(gc_AtributeCLAUSE).Value.ToString)
            Dim v_str_bank_status As String
            Select Case CInt(v_str_intFrequence)
                Case 1
                    v_str_bank_status = "bank_status_t3"
                Case 2
                    v_str_bank_status = "bank_status_t1"
                Case Else
                    v_str_bank_status = "bank_status_td"
            End Select
            v_DataAccess.SetSysVar("SYSTEM", v_str_bank_status, v_strBRID, OPERATION_ACTIVE)
            Return 0
        Catch ex As Exception
            v_DataAccess.Rollback()
            Return v_lngErrCode
            Throw ex
        End Try
    End Function
    Public Function ENDOFDAY(ByRef pv_strObjMsg As String) As Long
        Dim v_lngErrCode As Long = ERR_SYSTEM_OK
        Dim v_strErrorSource As String = "Host.SystemAdmin.ENDOFDAY", v_strErrorMessage As String = ""
        Dim v_strSQL As String
        Dim v_DataAccess As New DataAccess
        Dim v_xmlDocument As New Xml.XmlDocument
        Dim blnTran As Boolean = False
        Dim v_dsMF, v_dsMF1 As DataSet
        Dim v_strFOMULA01, v_strFOMULA09 As String
        Dim v_strBatchType As String = "1"
        Dim v_strBRID As String
        Try
            v_DataAccess.NewDBInstance(gc_MODULE_INQUERY)


            v_xmlDocument.LoadXml(pv_strObjMsg)

            v_strBRID = Trim(v_xmlDocument.DocumentElement.Attributes(gc_AtributeBRID).Value.ToString)
            Dim v_strCURRDATE As String = Trim(v_xmlDocument.DocumentElement.Attributes(gc_AtributeTXDATE).Value.ToString)
            Dim v_strTLID As String = Trim(v_xmlDocument.DocumentElement.Attributes(gc_AtributeTLID).Value.ToString)
            'tuanta
            v_strSQL = "SELECT FOMULA FROM MFTYPE " _
              & " WHERE MFNO='01' AND DELETED=0 AND STATUS=0"
            v_dsMF = v_DataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            If v_dsMF.Tables(0).Rows.Count > 0 Then
                v_strFOMULA01 = v_dsMF.Tables(0).Rows(0)(0)
            Else
                Return -9001
            End If
            'end tuanta
            ' Change status of branch
            v_DataAccess.SetSysVar("SYSTEM", "BRSTATUS", v_strBRID, OPERATION_INACTIVE)
            'System.Threading.Thread.Sleep(30 * 60 * 100)
            blnTran = True
            v_DataAccess.BeginTran()
            Dim v_strNumDay As String = "30"
            Dim v_strPassDate As String
            ' check constraint 
            v_strSQL = "insert into approve_batch(txdate, brid) select TO_DATE('" & v_strCURRDATE & "','dd/mm/yyyy'), '" & v_strBRID & "' from dual"
            v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)

            'v_strSQL = "insert into iamast_all select /*+ parallel(a,4) */ TO_DATE('" & v_strCURRDATE & "','dd/mm/yyyy') ,a.* from iamast a where brid = '" & v_strBRID & "' and deleted=0"
            'v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)

            'v_strSQL = "insert into mamast_all select /*+ parallel(a,4) */ TO_DATE('" & v_strCURRDATE & "','dd/mm/yyyy') ,a.* from mamast a where brid = '" & v_strBRID & "' and deleted=0"
            'v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)

            v_lngErrCode = v_DataAccess.GetSysVar("SYSTEM", "NUMDAY", v_strNumDay)

            'Lay ngay qua khu
            v_strSQL = "SELECT TO_CHAR(TO_DATE('" & v_strCURRDATE & "','dd/mm/yyyy')-" & v_strNumDay & ",'dd/mm/yyyy') PASSDATE FROM DUAL"
            v_dsMF = v_DataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            v_strPassDate = v_dsMF.Tables(0).Rows(0)(0)

            'Cap nhat ngay qua khu
            v_strSQL = "UPDATE SYSVAR SET VARVALUE = '" & v_strPassDate & "'" _
                        & " WHERE GRNAME='SYSTEM' AND VARNAME='PASSDATE' AND BRID='" & v_strBRID & "'"
            v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)

            'Xoa du lieu cac bang trong ngay
            'v_lngErrCode = BatchTranTruncate(v_strBRID, v_DataAccess, v_strDBLink)

            'RA
            v_lngErrCode = BatchTranInsert(v_strBRID, v_strCURRDATE, v_strPassDate, "RA", v_DataAccess)
            ' CS
            v_lngErrCode = BatchTranInsert(v_strBRID, v_strCURRDATE, v_strPassDate, "CS", v_DataAccess)
            'SF
            v_lngErrCode = BatchTranInsert(v_strBRID, v_strCURRDATE, v_strPassDate, "SF", v_DataAccess)
            ' MA
            v_lngErrCode = BatchTranInsert(v_strBRID, v_strCURRDATE, v_strPassDate, "MA", v_DataAccess)
            'IA
            'v_lngErrCode = BatchTranInsert(v_strBRID, v_strCURRDATE, v_strPassDate, "IA", v_DataAccess)
            'v_strSQL = "insert into iatrana_all select a.* from iatran a where a.brid = '" & v_strBRID & "' and not nvl(a.issue_autoid,0) in (select autoid from saissue where deleted=0 and status<>2)"
            ' #Begin VSDC
            ' date 30/08/2024
            ' purpose: disable insert iatrana_all
            ' author : VSDC

            'Rao ngay 10/09/2024 de pass ket thuc ngay - TruongNX41
            'v_strSQL = "insert into iatrana_all select a.* from iatran a where a.brid = '" & v_strBRID & "'"
            'End TruongNX41 10/09/2024

            ' #end VSDC
            'bangpv 28/08/2012: Them cho phan he BA
            'BA
            v_lngErrCode = BatchTranInsert(v_strBRID, v_strCURRDATE, v_strPassDate, "BA", v_DataAccess)
            'end bangpv 28/08/2012

            'Rao ngay 10/09/2024 de pass ket thuc ngay - TruongNX41
            'v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
            'End TruongNX41 10/09/2024

            'RG
            v_lngErrCode = BatchTranInsert(v_strBRID, v_strCURRDATE, v_strPassDate, "RG", v_DataAccess)
            ' TLLOG
            v_strSQL = "insert into tllogall_all select a.* from tllog a " _
                    & " where a.status in (3,4) and a.brid = '" & v_strBRID _
                    & "' and not a.autoid in (select autoid from saissue where deleted=0 and status<>2) " _
                    & "  and not nvl(a.parentid,0) in (select autoid from saissue where deleted=0 and status<>2) " _
                    & " and not tltxcd like '60%'"
            v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)

            '' delete CA
            'v_strSQL = "insert into tllogall_all select a.* from tllog a " _
            '        & " where a.deleted<>0 and a.brid = '" & v_strBRID & "'"
            'v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
            'v_strSQL = "delete from tllog a " _
            '        & " where a.deleted<>0 and a.brid = '" & v_strBRID & "'"
            'v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)

            'v_strSQL = "insert into catran_1 select a.* from catran a " _
            '        & " where a.deleted<>0 and a.brid = '" & v_strBRID & "'"
            'v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
            'v_strSQL = "delete from catran a " _
            '        & " where a.deleted<>0 and a.brid = '" & v_strBRID & "'"
            'v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)

            'v_strSQL = "insert into camast_1 select a.* from camast a " _
            '        & " where a.deleted<>0 and a.brid = '" & v_strBRID & "'"
            'v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
            'v_strSQL = "delete from camast a " _
            '        & " where a.deleted<>0 and a.brid = '" & v_strBRID & "'"
            'v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
            '' end CA

            ' MF
            ' MF.1 : batch
            v_strSQL = "insert into mftrana_all select * from mftran where brid = '" & v_strBRID & "'"
            v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
            ' end
            'Delete du lieu o Database HN
            'RA
            v_lngErrCode = BatchTranDelete(v_strBRID, v_strCURRDATE, v_strPassDate, "RA", v_DataAccess)
            'CS
            v_lngErrCode = BatchTranDelete(v_strBRID, v_strCURRDATE, v_strPassDate, "CS", v_DataAccess)
            'SF
            v_lngErrCode = BatchTranDelete(v_strBRID, v_strCURRDATE, v_strPassDate, "SF", v_DataAccess)
            'MA
            v_lngErrCode = BatchTranDelete(v_strBRID, v_strCURRDATE, v_strPassDate, "MA", v_DataAccess)
            'IA
            'v_lngErrCode = BatchTranDelete(v_strBRID, v_strCURRDATE, v_strPassDate, "IA", v_DataAccess)
            'v_strSQL = "delete from iatran a where a.brid = '" & v_strBRID & "' and not nvl(a.issue_autoid,0) in (select autoid from saissue where deleted=0 and status<>2)"
            ' #Begin VSDC
            ' date 30/08/2024
            ' purpose: disable insert iatrana_all
            ' author : VSDC

            'Rao ngay 10/09/2024 de pass ket thuc ngay - TruongNX41
            'v_strSQL = "delete from iatran a where a.brid = '" & v_strBRID & "'"
            ' end VSDC
            'v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
            'End TruongNX41 10/09/2024

            'RG
            'bangpv 28/08/2012: Them cho phan he BA
            'BA
            v_lngErrCode = BatchTranDelete(v_strBRID, v_strCURRDATE, v_strPassDate, "BA", v_DataAccess)
            'end bangpv 28/08/2012
            v_lngErrCode = BatchTranDelete(v_strBRID, v_strCURRDATE, v_strPassDate, "RG", v_DataAccess)
            'Xoa bang stpnotify va stpqueue 
            'STPQUEUE 
            v_strSQL = " INSERT INTO STPQUEUE_HIS " _
                        & " SELECT * FROM STPQUEUE WHERE created_date <= to_date('" & v_strCURRDATE & "','DD/MM/YYYY')-7"
            v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)

            v_strSQL = " DELETE STPQUEUE WHERE created_date <= to_date('" & v_strCURRDATE & "','DD/MM/YYYY') -7"
            v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)

            'start STPQUEUE_SATS 20221229
            'HoaLX 2023/08/03 - Chi chay o san hnx 0001
            If v_strBRID = "0001" Then
                v_strSQL = " INSERT INTO STPQUEUE_SATS_HIS " _
                            & " SELECT * FROM STPQUEUE_SATS WHERE created_date <= to_date('" & v_strCURRDATE & "','DD/MM/YYYY')-7"
                v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)

                v_strSQL = " DELETE STPQUEUE_SATS WHERE created_date <= to_date('" & v_strCURRDATE & "','DD/MM/YYYY') -7"
                v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
            End If

            'end: STPQUEUE_SATS 20221229
            'stpnotify 
            v_strSQL = " INSERT INTO STPNOTIFY_HIS " _
                        & " SELECT * FROM STPNOTIFY WHERE to_date(busdate,'DD/MM/YYYY') <= to_date('" & v_strCURRDATE & "','DD/MM/YYYY') -7"
            v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)

            v_strSQL = " DELETE STPNOTIFY WHERE to_date(busdate,'DD/MM/YYYY') <= to_date('" & v_strCURRDATE & "','DD/MM/YYYY') -7"
            v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
            'End Xoa bang stpnotify va stpqueue 


            'TLLOG
            v_strSQL = "delete from tllog a " _
                    & " where a.status in (3,4) and a.brid = '" & v_strBRID _
                    & "' and not a.autoid in (select autoid from saissue where deleted=0 and status<>2) " _
                    & "  and not nvl(a.parentid,0) in (select autoid from saissue where deleted=0 and status<>2) " _
                    & " and not tltxcd like '60%'"
            v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
            'MF
            v_strSQL = "delete from mftran where brid = '" & v_strBRID & "'"
            v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
            ' tuanta 
            ' MF.2 : charge in depository of member
            v_strSQL = "Select GET_TRADE_DATE(TO_DATE('" & v_strCURRDATE & "','DD/MM/YYYY')+1,'" & v_strBRID & "','+')" _
                    & " -TO_DATE('" & v_strCURRDATE & "','DD/MM/YYYY')  DIFFNO from dual"
            v_dsMF1 = v_DataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            Dim v_intDIFFNO, i As Integer
            If v_dsMF1.Tables(0).Rows.Count > 0 Then
                v_intDIFFNO = Math.Abs(v_dsMF1.Tables(0).Rows(0)("DIFFNO"))
            Else
                i = i Mod 0
            End If
            For i = 1 To v_intDIFFNO
                v_strSQL = "insert into TMP_TLLOG(AUTOID) select HOST10.SEQ_TLLOG.NEXTVAL from dual"
                v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)

                If i = 1 Then
                    v_strSQL = "insert into tmp_matran(micode, tmpid, sicode, typeno, namt)" _
                        & " select a.MICODE M, b.type T, a.SICODE S, a.TYPENO N, a.balance X" _
                        & " from mamast a, rgsi b " _
                        & " where a.sicode=b.sicode and b.deleted =0 and b.status=0 " _
                        & " and not (a.micode ='501' and substr(a.typeno,5,1) ='1') " _
                        & " and a.status = 0 and a.deleted = 0 and a.brid = '" & v_strBRID _
                        & "' and b.brid = '" & v_strBRID & "' and a.micode<>'000' " _
                    & " AND A.SICODE NOT IN ( " _
                    & " SELECT SICODE FROM TLLOGALL_ALL A WHERE " _
                    & " A.AUTOID IN (SELECT A.AUTOID FROM SAISSUE A, (SELECT VARVALUE FROM SYSVAR WHERE varname='CURRDATE' AND BRID = '" _
                    & v_strBRID & "' AND DELETED=0 AND STATUS=0) B " _
                    & " WHERE GET_TRADE_DATE(A.DEDATE-1,'" & v_strBRID & "','-') = TO_DATE(B.varvalue,'dd/mm/yyyy') " _
                    & " AND A.DELETED=0 and a.apptype='SD' )" _
                    & ")"
                    v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
                    v_strSQL = "insert into TMP_TLLOG1(AUTOID) select autoid from TMP_TLLOG"
                    v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
                    'bangpv: loai bo cac giao dich phat sinh do hoan doi co phieu, trai phieu chuyen doi 
                    v_strSQL = "INSERT INTO tmp_matran1(micode, tmpid, sicode, typeno, namt)" _
                            & "SELECT a.micode,b.TYPE, a.sicode, a.typeno, sum(a.namt) namt  FROM passed.iatrana_all  a, rgsi b " _
                            & "WHERE issue_autoid IN ( " _
                            & "SELECT a.autoid FROM tllog a, saissue b " _
                            & "WHERE a.tltxcd ='6043' AND a.col_value05 IN ('8','9') " _
                            & "AND a.autoid = b.autoid " _
                            & "AND GET_TRADE_DATE(b.DEDATE-1,'" & v_strBRID & "','-')=to_date('" & v_strCURRDATE & "','DD/MM/YYYY' )) " _
                            & "AND OPERATOR ='+' AND a.sicode = b.sicode " _
                            & "AND a.brid = b.brid AND a.status =0 AND a.deleted =0  AND b.status =0 AND b.deleted =0 " _
                            & "AND a.brid ='" & v_strBRID & "' and a.micode <>'000' " _
                            & "GROUP BY a.micode,b.TYPE, a.sicode, a.typeno"

                    v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
                    v_strSQL = "MERGE INTO tmp_matran a " _
                            & "USING tmp_matran1 b  " _
                            & "ON (a.micode= b.micode AND a.tmpid = b.tmpid " _
                            & "AND a.sicode = b.sicode AND b.typeno = a.typeno) " _
                            & " WHEN matched THEN " _
                            & "UPDATE SET a.namt = a.namt - b.namt "
                    v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
                    'end bangpv 20160719
                End If

                v_strSQL = "insert into TLLOGALL_ALL(AUTOID, TXNUM, TXDATE, busdate, BRID,TLID, TLTXCD, status,deleted, txname, brcode, isparent, parent_text " _
                        & ", parentid, tlname, offname, chkname, cfrname, status_text, chkid, offid, cfrid, " _
                        & " col_value01, col_type01, col_desc01, col_value02, col_type02, col_desc02)" _
                        & " select a.AUTOID,generate_tllog_code(a.autoid), TO_DATE('" & v_strCURRDATE & "','DD/MM/YYYY')+ " & CStr(i - 1) & ",TO_DATE('" & v_strCURRDATE & "','DD/MM/YYYY')+ " & CStr(i - 1) & "," _
                        & " '" & v_strBRID & "','" & v_strTLID & "', '1099',3,0, 'Giao dịch chạy batch cuối ngày', b.DCNAME,1, 'Giao dịch lẻ'  " _
                        & " ,0, c.TLNAME, c.TLNAME,c.TLNAME,c.TLNAME, 'Duyệt cấp trưởng VSD','" & v_strTLID & "','" & v_strTLID & "','" & v_strTLID & "'" _
                        & " , '" & v_strBRID & "','C','" & v_strBRID & "',to_char(TO_DATE('" & v_strCURRDATE & "','DD/MM/YYYY')+ " & CStr(i - 1) & ",'dd/mm/yyyy'),'D',to_char(TO_DATE('" & v_strCURRDATE & "','DD/MM/YYYY')+ " & CStr(i - 1) & ",'dd/mm/yyyy')" _
                        & " from TMP_TLLOG a, brgrp b, tlprofiles c where b.brid = '" & v_strBRID & "' and c.tlid = '" & v_strTLID & "' and c.deleted=0 and c.status=0"
                v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)

                v_strSQL = "insert into mftrana_all(txnum, txdate, acctno, FIELD, namt, deleted, autoid, status, brid, OPERATOR,ref, tltxcd, msgamt, fomula, micode, mfno, typeno, sicode)" _
                        & " SELECT generate_tllog_code(b.autoid), TO_DATE('" & v_strCURRDATE & "','DD/MM/YYYY')+ " & CStr(i - 1) & ", a.micode || '.01', 'BALANCE' ," _
                        & " a.namt, 0, HOST10.seq_mftran.nextval, 0, '" & v_strBRID & "', '+' " _
                        & " , 'Giao dịch chạy batch cuối ngày(' || generate_tllog_code(b.autoid) || ',' || to_char(TO_DATE('" & v_strCURRDATE & "','DD/MM/YYYY')+ " & CStr(i - 1) & ",'dd/mm/yyyy') || ')', '1099', a.X , '" & v_strFOMULA01 & "',a.micode, '01', a.typeno, a.sicode " _
                        & " FROM " _
                        & " ( select a.M micode, a.S sicode, a.N typeno, sum(a.X) X, nvl(sum(nvl(" & v_strFOMULA01 & ",0)),0) namt " _
                        & "   from  (select a.MICODE M, a.tmpid T, a.SICODE S, a.TYPENO N, a.namt X, '" & v_strBRID & "' B, TO_DATE('" & v_strCURRDATE & "','DD/MM/YYYY')+ " & CStr(i - 1) & " D " _
                        & "          from tmp_matran a where a.micode <'800') a " _
                        & "    group by a.M,a.N, a.S" _
                        & "    having sum(a.X)>0 " _
                        & " ) a, tmp_tllog b"
                v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)

                v_strSQL = "delete from TMP_TLLOG"
                v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)

            Next



            If v_strBRID = "0001" Then
                v_strSQL = "Select EXTRACT(MONTH FROM GET_TRADE_DATE(TO_DATE('" & v_strCURRDATE & "','DD/MM/YYYY')+1,'" & v_strBRID & "','+'))" _
                        & " -EXTRACT(MONTH FROM TO_DATE('" & v_strCURRDATE & "','DD/MM/YYYY'))  DIFFNO from dual"
                v_dsMF1 = v_DataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                If v_dsMF1.Tables(0).Rows(0)("DIFFNO") <> 0 Then
                    v_strSQL = "SELECT FOMULA FROM MFTYPE " _
                              & " WHERE MFNO='09' AND DELETED=0 AND STATUS=0"
                    v_dsMF = v_DataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                    If v_dsMF.Tables(0).Rows.Count > 0 Then
                        v_strFOMULA09 = v_dsMF.Tables(0).Rows(0)(0)
                    Else
                        i = i Mod 0
                    End If
                    v_strSQL = "insert into mftrana_all(txnum, txdate, acctno, FIELD, namt, deleted, autoid, status, brid, OPERATOR,ref, tltxcd, msgamt, fomula, micode, mfno, typeno, sicode)" _
                            & " SELECT generate_tllog_code(b.autoid), TO_DATE('" & v_strCURRDATE & "','dd/mm/yyyy'), a.micode || '.09', 'BALANCE' ," _
                            & " a.namt, 0, HOST10.seq_mftran.nextval, 0, '" & v_strBRID & "', '+' " _
                            & " , 'Giao dịch chạy batch cuối ngày(' || generate_tllog_code(b.autoid) || ',' || '" & v_strCURRDATE & "' || ')', '1099', NULL , '" & v_strFOMULA09 & "',a.micode, '09', NULL, NULL " _
                            & " FROM " _
                            & " ( select a.micode, " & v_strFOMULA09 & " namt " _
                            & "   from RGMI A WHERE a.DELETED=0 AND A.STATUS=0 and micode<>'000' and micode <'800'" _
                            & " ) a, TMP_TLLOG1 b"
                    v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
                End If
            End If

            'end tuanta 
            'bangpv
            'Thay đổi trạng thái TTBT
            'v_DataAccess.SetSysVar("SYSTEM", "sett_t3", v_strBRID, OPERATION_INACTIVE)
            'v_DataAccess.SetSysVar("SYSTEM", "sett_t1", v_strBRID, OPERATION_INACTIVE)
            'v_DataAccess.SetSysVar("SYSTEM", "sett_td", v_strBRID, OPERATION_INACTIVE)
            'v_strSQL = "UPDATE SYSVAR SET VARVALUE = '0'" _
            '            & " WHERE GRNAME='SYSTEM' AND VARNAME='room_status' AND BRID='" & v_strBRID & "'"
            'v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
            v_strSQL = "UPDATE SYSVAR SET VARVALUE = '0'" _
                        & " WHERE GRNAME='SYSTEM' AND VARNAME='bank_status_t3' AND BRID='" & v_strBRID & "'"
            v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
            v_strSQL = "UPDATE SYSVAR SET VARVALUE = '0'" _
                        & " WHERE GRNAME='SYSTEM' AND VARNAME='bank_status_t1' AND BRID='" & v_strBRID & "'"
            v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
            v_strSQL = "UPDATE SYSVAR SET VARVALUE = '0'" _
                        & " WHERE GRNAME='SYSTEM' AND VARNAME='bank_status_td' AND BRID='" & v_strBRID & "'"
            v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
            'BangPV
            'Bỏ chạy Job cuối ngày
            'v_strSQL = "UPDATE SYSVAR SET VARVALUE = '1'" _
            '            & " WHERE GRNAME='SYSTEM' AND VARNAME='BALANCE_JOB' AND BRID='" & v_strBRID & "'"
            'v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
            'End BangPV
            'Cap nhat trang thai nha dau tu chuyen nghiep 
            'HoaLX - 2023/08/03 - chi cap nhat voi san 0008 - TPRL
            If v_strBRID = "0008" Then
                v_strSQL = "MERGE INTO RGIIIA_CB A " _
                            & " USING " _
                            & " (SELECT * FROM rgiiia_cb a " _
                            & " WHERE a.deleted =0 and (a.pro_enddate =TO_DATE ('" & v_strCURRDATE & "','DD/MM/YYYY') " _
                            & " OR (a.pro_enddate>TO_DATE ('" & v_strCURRDATE & "','DD/MM/YYYY') " _
                            & " AND a.pro_enddate <GET_T_PLUS(TO_DATE('" & v_strCURRDATE & "','DD/MM/YYYY'),'" & v_strBRID & "',1)) " _
                            & ")) b " _
                            & " ON (a.autoid = b.autoid)   " _
                            & " WHEN MATCHED THEN " _
                            & " UPDATE SET a.IS_PRO_INVESTOR=0"
                v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)

                v_strSQL = "MERGE INTO RGII_CB A " _
                            & " USING " _
                            & " (SELECT * FROM rgii_cb a " _
                            & " WHERE a.deleted =0 and (a.pro_enddate =TO_DATE ('" & v_strCURRDATE & "','DD/MM/YYYY') " _
                            & " OR (a.pro_enddate>TO_DATE ('" & v_strCURRDATE & "','DD/MM/YYYY') " _
                            & " AND a.pro_enddate <GET_T_PLUS(TO_DATE('" & v_strCURRDATE & "','DD/MM/YYYY'),'" & v_strBRID & "',1)) " _
                            & ")) b " _
                            & " ON (a.autoid = b.autoid)   " _
                            & " WHEN MATCHED THEN " _
                            & " UPDATE SET a.IS_PRO_INVESTOR=0"
                v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)

            End If


            v_strSQL = "UPDATE SYSVAR SET VARVALUE = '1'" _
                        & " WHERE GRNAME='SYSTEM' AND VARNAME='DATA_BATCH' AND BRID='" & v_strBRID & "'"
            v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
            'v_DataAccess.SetSysVar("SYSTEM", "room_status", v_strBRID, OPERATION_INACTIVE)
            'v_DataAccess.SetSysVar("SYSTEM", "bank_status_t3", v_strBRID, OPERATION_INACTIVE)
            'v_DataAccess.SetSysVar("SYSTEM", "bank_status_t1", v_strBRID, OPERATION_INACTIVE)
            'v_DataAccess.SetSysVar("SYSTEM", "bank_status_td", v_strBRID, OPERATION_INACTIVE)
            'end bangpv 
            'Chay batch CA
            v_lngErrCode = BatchCA(v_strBRID, v_strTLID, v_strCURRDATE, v_DataAccess, "END")
            v_lngErrCode = BatchRG(v_strBRID, v_strTLID, v_strCURRDATE, v_DataAccess, "END")
            ' commit
            'BangPV 20141112: Them chay batch chuyen san
            v_lngErrCode = BatchSD(v_strBRID, v_strTLID, v_strCURRDATE, v_DataAccess, "END")

            'HaNM5 them phan batch cho phan dieu chinh chuc nang thanh toan TPCP
            v_lngErrCode = BatchCS(v_strBRID, v_strTLID, v_strCURRDATE, v_DataAccess, "END")
            ' commit
            v_lngErrCode = BackUp_DB(v_strBRID, v_strCURRDATE)

            If v_lngErrCode = -1 Then
                v_DataAccess.Rollback()
                v_DataAccess.SetSysVar("SYSTEM", "BRSTATUS", v_strBRID, OPERATION_ACTIVE)
            Else
                v_DataAccess.Commit()
            End If


            Return v_lngErrCode

        Catch ex As Exception
            If blnTran Then
                v_DataAccess.Rollback()
                v_DataAccess.SetSysVar("SYSTEM", "BRSTATUS", v_strBRID, OPERATION_ACTIVE)
            End If
            LogError.Write(ex.Message, EventLogEntryType.Error, gc_MODULE_HOST)
            Throw ex
        Finally
            If Not v_dsMF Is Nothing Then
                v_dsMF.Dispose()
            End If
            If Not v_DataAccess Is Nothing Then
                v_DataAccess.Dispose()
            End If
        End Try
    End Function
    Public Function BackUp_DB(ByVal v_strBrid As String, ByVal v_strTxdate As String) As Long
        Dim v_lngErrCode As Long = ERR_SYSTEM_OK
        Dim v_DataAccess As New DataAccess
        Dim v_strRootPath As String
        Dim v_strLocalFileName As String
        Dim v_strUsername As String
        Dim v_strPassword As String
        Dim v_strDBInstance As String
        'Dim v_strLocalDir As String
        Try
            v_strTxdate = Replace(v_strTxdate, "\", "")
            v_strTxdate = Replace(v_strTxdate, "/", "")

            v_DataAccess.GetSysVar("BK_DB", "RootPath", v_strBrid, v_strRootPath)
            'v_DataAccess.GetSysVar("BK_DB", "UserName", v_strBrid, v_strUsername)
            'v_DataAccess.GetSysVar("BK_DB", "Password", v_strBrid, v_strPassword)
            'v_DataAccess.GetSysVar("BK_DB", "DBInstance", v_strBrid, v_strDBInstance)
            'bangpv: sua lay user, password tu DB sang file config
            v_strDBInstance = DecryptString(gc_ENCRYPT_PASSWORD, GlobalDataConfig.HOST_DBCONFIG.DataSource)
            v_strUsername = DecryptString(gc_ENCRYPT_PASSWORD, GlobalDataConfig.HOST_DBCONFIG.UserName)
            v_strPassword = DecryptString(gc_ENCRYPT_PASSWORD, GlobalDataConfig.HOST_DBCONFIG.Password)
            v_strLocalFileName = v_strBrid & v_strTxdate

            Dim v_oWriter As System.IO.StreamWriter

            If File.Exists(v_strRootPath & "\" & Mid(v_strLocalFileName, 1, v_strLocalFileName.Length - 4) & ".bat") Then
                File.Delete(v_strRootPath & "\" & Mid(v_strLocalFileName, 1, v_strLocalFileName.Length - 4) & ".bat")
            End If
            If File.Exists(v_strRootPath & "\" & v_strBrid & "_" & v_strLocalFileName & ".dmp ") Then
                File.Delete(v_strRootPath & "\" & v_strBrid & "_" & v_strLocalFileName & ".dmp ")
            End If

            v_oWriter = New StreamWriter(v_strRootPath & "\" & Mid(v_strLocalFileName, 1, v_strLocalFileName.Length - 4) & ".bat")
            v_oWriter.WriteLine("@echo off")

            v_oWriter.WriteLine("cd " & v_strRootPath)
            v_oWriter.WriteLine("expdp " & v_strUsername & "/" & v_strPassword & "@" & v_strDBInstance & " DIRECTORY=export_dir DUMPFILE=" & v_strBrid & "_" & v_strTxdate & ".dmp " _
            & " TABLES=(IAMAST:BR" & v_strBrid & ", MAMAST:BR" & v_strBrid & ") CONTENT=DATA_ONLY ")
            'v_oWriter.WriteLine("binary")
            'v_oWriter.WriteLine("put " & v_strLocalFileName & " " & v_strLocalFileName)
            v_oWriter.WriteLine("bye" & vbCrLf)

            v_oWriter.Close()

            Dim v_oProcess As Process
            v_oProcess = New Process

            v_oProcess.StartInfo.FileName = v_strRootPath & "\" & Mid(v_strLocalFileName, 1, v_strLocalFileName.Length - 4) & ".bat"
            v_oProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
            v_oProcess.StartInfo.CreateNoWindow = True
            v_oProcess.Start()
            'v_oProcess.WaitForExit()
            v_oProcess.Close()
            Return v_lngErrCode
        Catch ex As Exception
            LogError.Write(ex.Message, EventLogEntryType.Error, gc_MODULE_HOST)
            Return -1
        End Try
    End Function
    'Public Function ENDOFDAY1(ByRef pv_strObjMsg As String) As Long
    '    Dim v_lngErrCode As Long = ERR_SYSTEM_OK
    '    Dim v_strErrorSource As String = "Host.SystemAdmin.ENDOFDAY", v_strErrorMessage As String = ""
    '    Dim v_strSQL As String
    '    Dim v_DataAccess As New DataAccess, v_HcmDataAccess As New DataAccess
    '    'Dim v_HostDataAccess As New DataAccess
    '    Dim v_xmlDocument As New Xml.XmlDocument
    '    Dim blnTran As Boolean = False
    '    Dim v_dsMF As DataSet
    '    Dim v_strFOMULA As String
    '    Dim v_strBatchType As String = "1"

    '    Try
    '        v_DataAccess.NewDBInstance(gc_MODULE_INQUERY)
    '        'v_HostDataAccess.NewDBInstance(gc_MODULE_HOST)
    '        v_lngErrCode = v_DataAccess.GetSysVar("SYSTEM", "BATCH_TYPE", v_strBatchType)
    '        If v_strBatchType <> "1" Then
    '            v_HcmDataAccess.NewDBInstance(gc_MODULE_BDS)
    '        End If
    '        v_xmlDocument.LoadXml(pv_strObjMsg)
    '        Dim v_strBRID As String = Trim(v_xmlDocument.DocumentElement.Attributes(gc_AtributeBRID).Value.ToString)
    '        Dim v_strCURRDATE As String = Trim(v_xmlDocument.DocumentElement.Attributes(gc_AtributeTXDATE).Value.ToString)
    '        Dim v_strTLID As String = Trim(v_xmlDocument.DocumentElement.Attributes(gc_AtributeTLID).Value.ToString)
    '        v_strSQL = "SELECT FOMULA FROM MFTYPE " _
    '          & " WHERE MFNO='01' AND BRID='" & v_strBRID & "' AND DELETED=0 AND STATUS=0"
    '        v_dsMF = v_DataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
    '        If v_dsMF.Tables(0).Rows.Count > 0 Then
    '            v_strFOMULA = v_dsMF.Tables(0).Rows(0)(0)
    '        Else
    '            Return -9001
    '        End If

    '        ' Change status of branch
    '        v_DataAccess.SetSysVar("SYSTEM", "BRSTATUS", v_strBRID, OPERATION_INACTIVE)

    '        blnTran = True
    '        v_DataAccess.BeginTran()
    '        If v_strBatchType <> "1" Then
    '            v_HcmDataAccess.BeginTran()
    '        End If

    '        Dim v_strNumDay As String = "30"
    '        Dim v_strPassDate As String

    '        v_lngErrCode = v_DataAccess.GetSysVar("SYSTEM", "NUMDAY", v_strNumDay)

    '        'Lay ngay qua khu
    '        v_strSQL = "SELECT TO_CHAR(TO_DATE('" & v_strCURRDATE & "','dd/mm/yyyy')-" & v_strNumDay & ",'dd/mm/yyyy') PASSDATE FROM DUAL"
    '        v_dsMF = v_DataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
    '        v_strPassDate = v_dsMF.Tables(0).Rows(0)(0)

    '        'Cap nhat ngay qua khu
    '        v_strSQL = "UPDATE SYSVAR SET VARVALUE = '" & v_strPassDate & "'" _
    '                    & " WHERE GRNAME='SYSTEM' AND VARNAME='PASSDATE' AND BRID='" & v_strBRID & "'"
    '        v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)

    '        ' CA
    '        'v_strSQL = "insert into catrana select * from catran where brid = '" & v_strBRID & "'"
    '        'v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
    '        'v_strSQL = "delete from catran where brid = '" & v_strBRID & "'"
    '        'v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
    '        'v_lngErrCode = BatchTran(v_strBRID, v_strCURRDATE, v_strPassDate, "CA", v_DataAccess)

    '        ' RA
    '        'v_strSQL = "insert into ratrana_all select * from ratran where brid = '" & v_strBRID & "'"
    '        'v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
    '        'v_strSQL = "delete from ratran where brid = '" & v_strBRID & "'"
    '        'v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
    '        v_lngErrCode = BatchTranInsert(v_strBRID, v_strCURRDATE, v_strPassDate, "RA", v_DataAccess)
    '        If v_strBatchType <> "1" Then
    '            v_lngErrCode = BatchTranInsert(v_strBRID, v_strCURRDATE, v_strPassDate, "RA", v_HcmDataAccess)
    '        End If
    '        ' CS
    '        v_lngErrCode = BatchTranInsert(v_strBRID, v_strCURRDATE, v_strPassDate, "CS", v_DataAccess)
    '        If v_strBatchType <> "1" Then
    '            v_lngErrCode = BatchTranInsert(v_strBRID, v_strCURRDATE, v_strPassDate, "CS", v_HcmDataAccess)
    '        End If
    '        ' SF
    '        'v_strSQL = "insert into sftrana_all select * from sftran where brid = '" & v_strBRID & "'"
    '        'v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
    '        'v_strSQL = "delete from sftran where brid = '" & v_strBRID & "'"
    '        'v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
    '        v_lngErrCode = BatchTranInsert(v_strBRID, v_strCURRDATE, v_strPassDate, "SF", v_DataAccess)
    '        If v_strBatchType <> "1" Then
    '            v_lngErrCode = BatchTranInsert(v_strBRID, v_strCURRDATE, v_strPassDate, "SF", v_HcmDataAccess)
    '        End If
    '        ' MA
    '        'v_strSQL = "insert into matrana_all select * from matran where brid = '" & v_strBRID & "'"
    '        'v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
    '        'v_strSQL = "delete from matran where brid = '" & v_strBRID & "'"
    '        'v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
    '        v_lngErrCode = BatchTranInsert(v_strBRID, v_strCURRDATE, v_strPassDate, "MA", v_DataAccess)
    '        If v_strBatchType <> "1" Then
    '            v_lngErrCode = BatchTranInsert(v_strBRID, v_strCURRDATE, v_strPassDate, "MA", v_HcmDataAccess)
    '        End If
    '        ' IA
    '        'v_strSQL = "insert into iatrana_all select * from iatran where brid = '" & v_strBRID & "'"
    '        'v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
    '        'v_strSQL = "delete from iatran where brid = '" & v_strBRID & "'"
    '        'v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
    '        v_lngErrCode = BatchTranInsert(v_strBRID, v_strCURRDATE, v_strPassDate, "IA", v_DataAccess)
    '        If v_strBatchType <> "1" Then
    '            v_lngErrCode = BatchTranInsert(v_strBRID, v_strCURRDATE, v_strPassDate, "IA", v_HcmDataAccess)
    '        End If
    '        ' RE
    '        'v_strSQL = "insert into retrana select * from retran where brid = '" & v_strBRID & "'"
    '        'v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
    '        'v_strSQL = "delete from retran where brid = '" & v_strBRID & "'"
    '        'v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)

    '        ' RG
    '        'v_strSQL = "insert into rgtrana_all select * from rgtran where brid = '" & v_strBRID & "'"
    '        'v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
    '        'v_strSQL = "delete from rgtran where brid = '" & v_strBRID & "'"
    '        'v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
    '        v_lngErrCode = BatchTranInsert(v_strBRID, v_strCURRDATE, v_strPassDate, "RG", v_DataAccess)
    '        If v_strBatchType <> "1" Then
    '            v_lngErrCode = BatchTranInsert(v_strBRID, v_strCURRDATE, v_strPassDate, "RG", v_HcmDataAccess)
    '        End If
    '        ' TLLOG
    '        v_strSQL = "insert into tllogall_all select a.* from tllog a " _
    '                & " where a.status in (3,4) and a.brid = '" & v_strBRID & "' AND a.tltxcd not LIKE '60%'"
    '        v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)

    '        If v_strBatchType <> "1" Then
    '            v_strSQL = "insert into tllogall_all select a.* from tllog a " _
    '                & " where a.status in (3,4) and a.brid = '" & v_strBRID & "' AND a.tltxcd not LIKE '60%'"
    '            v_HcmDataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
    '        End If

    '        'v_strSQL = "insert into tllogall select a.* from tllog a " _
    '        '        & " where a.status in (3,4) and a.brid = '" & v_strBRID & "' AND a.tltxcd not LIKE '60%'"
    '        'v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)

    '        ''Lay ngay min
    '        'Dim v_strMinDate As String
    '        'v_strSQL = "SELECT TO_CHAR(MIN(BUSDATE),'dd/mm/yyyy') FROM TLLOGALL"
    '        'v_dsMF = v_DataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
    '        'If v_dsMF.Tables(0).Rows(0)(0).ToString.Trim <> "" Then
    '        '    v_strMinDate = v_dsMF.Tables(0).Rows(0)(0)
    '        'Else
    '        '    v_strMinDate = v_strCURRDATE
    '        'End If

    '        ''Neu mindate < passdate xoa dl thua trong bang all
    '        'If CheckPassDate(v_strMinDate, v_strPassDate) Then
    '        '    v_strSQL = "DELETE FROM TLLOGALL WHERE BUSDATE<TO_DATE('" & v_strPassDate & "','dd/mm/yyyy')"
    '        '    v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
    '        'End If
    '        ''Neu mindate > passdate them dl tu bang all_all vao bang all
    '        'If Not CheckPassDate(v_strMinDate, v_strPassDate) Then
    '        '    v_strSQL = "INSERT INTO TLLOGALL SELECT * FROM TLLOGALL_ALL" _
    '        '                & " WHERE BUSDATE>=TO_DATE('" & v_strPassDate & "','dd/mm/yyyy')" _
    '        '                & " AND BUSDATE<TO_DATE('" & v_strMinDate & "','dd/mm/yyyy')" _
    '        '                & " AND BRID='" & v_strBRID & "' AND STATUS IN (3,4)"
    '        '    v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
    '        'End If

    '        'v_strSQL = "delete from tllog a " _
    '        '        & " where a.status in (3,4) and a.brid = '" & v_strBRID & "' AND a.tltxcd not LIKE '60%'"
    '        'v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)


    '        ' MF
    '        ' MF.1 : batch
    '        v_strSQL = "insert into mftrana_all select * from mftran where brid = '" & v_strBRID & "'"
    '        v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
    '        'If v_strBatchType <> "1" Then
    '        '    v_strSQL = "insert into mftrana_all select * from mftran where brid = '" & v_strBRID & "'"
    '        '    v_HcmDataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
    '        'End If
    '        ' MF.2 : charge in depository of member
    '        v_strSQL = "insert into TMP_TLLOG(AUTOID) select SEQ_TLLOG.NEXTVAL from dual"
    '        v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
    '        'v_HostDataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
    '        'v_strSQL = "insert into TLLOGALL(AUTOID, TXNUM, TXDATE, busdate, BRID,TLID, TLTXCD, status,deleted, txname, brcode, isparent, parent_text " _
    '        '        & ", parentid, tlname, offname, chkname, cfrname, status_text, chkid, offid, cfrid, " _
    '        '        & " col_value01, col_type01, col_desc01, col_value02, col_type02, col_desc02)" _
    '        '        & " select a.AUTOID,generate_tllog_code(a.autoid), TO_DATE('" & v_strCURRDATE & "','dd/mm/yyyy'),TO_DATE('" & v_strCURRDATE & "','dd/mm/yyyy')," _
    '        '        & " '" & v_strBRID & "','" & v_strTLID & "', '1099',3,0, 'Giao dịch chạy batch cuối ngày', b.DCNAME,1, 'Giao dịch lẻ'  " _
    '        '        & " ,0, c.TLNAME, c.TLNAME,c.TLNAME,c.TLNAME, 'Duyệt cấp trưởng VSD','" & v_strTLID & "','" & v_strTLID & "','" & v_strTLID & "'" _
    '        '        & " , '" & v_strBRID & "','C','" & v_strBRID & "','" & v_strCURRDATE & "','D','" & v_strCURRDATE & "'" _
    '        '        & " from TMP_TLLOG a, brgrp b, tlprofiles c where b.brid = '" & v_strBRID & "' and c.tlid = '" & v_strTLID & "' and c.deleted=0 and c.status=0"
    '        'v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)

    '        v_strSQL = "insert into TLLOGALL_ALL(AUTOID, TXNUM, TXDATE, busdate, BRID,TLID, TLTXCD, status,deleted, txname, brcode, isparent, parent_text " _
    '                & ", parentid, tlname, offname, chkname, cfrname, status_text, chkid, offid, cfrid, " _
    '                & " col_value01, col_type01, col_desc01, col_value02, col_type02, col_desc02)" _
    '                & " select a.AUTOID,generate_tllog_code(a.autoid), TO_DATE('" & v_strCURRDATE & "','dd/mm/yyyy'),TO_DATE('" & v_strCURRDATE & "','dd/mm/yyyy')," _
    '                & " '" & v_strBRID & "','" & v_strTLID & "', '1099',3,0, 'Giao dịch chạy batch cuối ngày', b.DCNAME,1, 'Giao dịch lẻ'  " _
    '                & " ,0, c.TLNAME, c.TLNAME,c.TLNAME,c.TLNAME, 'Duyệt cấp trưởng VSD','" & v_strTLID & "','" & v_strTLID & "','" & v_strTLID & "'" _
    '                & " , '" & v_strBRID & "','C','" & v_strBRID & "','" & v_strCURRDATE & "','D','" & v_strCURRDATE & "'" _
    '                & " from TMP_TLLOG a, brgrp b, tlprofiles c where b.brid = '" & v_strBRID & "' and c.tlid = '" & v_strTLID & "' and c.deleted=0 and c.status=0"

    '        v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
    '        'v_HostDataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)

    '        v_strSQL = "insert into mftrana_all(txnum, txdate, acctno, FIELD, namt, deleted, autoid, status, brid, OPERATOR,ref)" _
    '                & " SELECT generate_tllog_code(b.autoid), TO_DATE('" & v_strCURRDATE & "','dd/mm/yyyy'), a.micode || '.01', 'BALANCE' ," _
    '                & " " & v_strFOMULA & ", 0, seq_mftran.nextval, 0, '" & v_strBRID & "', '+' " _
    '                & ", 'Giao dịch chạy batch cuối ngày(' || generate_tllog_code(b.autoid) || ',' || '" & v_strCURRDATE & "' || ')'" _
    '                & " FROM " _
    '                & " (select substr(maacctno,1,3) micode, nvl(sum(nvl(balance,0)),0) X " _
    '                & "  from mamast where status = 0 and deleted = 0 and brid = '" & v_strBRID & "' group by substr(maacctno,1,3)) a" _
    '                & " , tmp_tllog b"

    '        v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
    '        'v_HostDataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)

    '        'If v_strBatchType <> "1" Then
    '        '    v_strSQL = "insert into TMP_TLLOG(AUTOID) select SEQ_TLLOG.NEXTVAL from dual"
    '        '    v_HcmDataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
    '        '    'v_strSQL = "insert into TLLOGALL(AUTOID, TXNUM, TXDATE, busdate, BRID,TLID, TLTXCD, status,deleted, txname, brcode, isparent, parent_text " _
    '        '    '        & ", parentid, tlname, offname, chkname, cfrname, status_text, chkid, offid, cfrid, " _
    '        '    '        & " col_value01, col_type01, col_desc01, col_value02, col_type02, col_desc02)" _
    '        '    '        & " select a.AUTOID,generate_tllog_code(a.autoid), TO_DATE('" & v_strCURRDATE & "','dd/mm/yyyy'),TO_DATE('" & v_strCURRDATE & "','dd/mm/yyyy')," _
    '        '    '        & " '" & v_strBRID & "','" & v_strTLID & "', '1099',3,0, 'Giao dịch chạy batch cuối ngày', b.DCNAME,1, 'Giao dịch lẻ'  " _
    '        '    '        & " ,0, c.TLNAME, c.TLNAME,c.TLNAME,c.TLNAME, 'Duyệt cấp trưởng VSD','" & v_strTLID & "','" & v_strTLID & "','" & v_strTLID & "'" _
    '        '    '        & " , '" & v_strBRID & "','C','" & v_strBRID & "','" & v_strCURRDATE & "','D','" & v_strCURRDATE & "'" _
    '        '    '        & " from TMP_TLLOG a, brgrp b, tlprofiles c where b.brid = '" & v_strBRID & "' and c.tlid = '" & v_strTLID & "' and c.deleted=0 and c.status=0"
    '        '    'v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)

    '        '    v_strSQL = "insert into TLLOGALL_ALL(AUTOID, TXNUM, TXDATE, busdate, BRID,TLID, TLTXCD, status,deleted, txname, brcode, isparent, parent_text " _
    '        '            & ", parentid, tlname, offname, chkname, cfrname, status_text, chkid, offid, cfrid, " _
    '        '            & " col_value01, col_type01, col_desc01, col_value02, col_type02, col_desc02)" _
    '        '            & " select a.AUTOID,generate_tllog_code(a.autoid), TO_DATE('" & v_strCURRDATE & "','dd/mm/yyyy'),TO_DATE('" & v_strCURRDATE & "','dd/mm/yyyy')," _
    '        '            & " '" & v_strBRID & "','" & v_strTLID & "', '1099',3,0, 'Giao dịch chạy batch cuối ngày', b.DCNAME,1, 'Giao dịch lẻ'  " _
    '        '            & " ,0, c.TLNAME, c.TLNAME,c.TLNAME,c.TLNAME, 'Duyệt cấp trưởng VSD','" & v_strTLID & "','" & v_strTLID & "','" & v_strTLID & "'" _
    '        '            & " , '" & v_strBRID & "','C','" & v_strBRID & "','" & v_strCURRDATE & "','D','" & v_strCURRDATE & "'" _
    '        '            & " from TMP_TLLOG a, brgrp b, tlprofiles c where b.brid = '" & v_strBRID & "' and c.tlid = '" & v_strTLID & "' and c.deleted=0 and c.status=0"
    '        '    v_HcmDataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)

    '        '    v_strSQL = "insert into mftrana_all(txnum, txdate, acctno, FIELD, namt, deleted, autoid, status, brid, OPERATOR,ref)" _
    '        '            & " SELECT generate_tllog_code(b.autoid), TO_DATE('" & v_strCURRDATE & "','dd/mm/yyyy'), a.micode || '.01', 'BALANCE' ," _
    '        '            & " " & v_strFOMULA & ", 0, seq_mftran.nextval, 0, '" & v_strBRID & "', '+' " _
    '        '            & ", 'Giao dịch chạy batch cuối ngày(' || generate_tllog_code(b.autoid) || ',' || '" & v_strCURRDATE & "' || ')'" _
    '        '            & " FROM " _
    '        '            & " (select substr(maacctno,1,3) micode, nvl(sum(nvl(balance,0)),0) X " _
    '        '            & "  from mamast where status = 0 and deleted = 0 and brid = '" & v_strBRID & "' group by substr(maacctno,1,3)) a" _
    '        '            & " , tmp_tllog b"
    '        '    v_HcmDataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
    '        'End If

    '        'v_strSQL = "insert into mftrana(txnum, txdate, acctno, FIELD, namt, deleted, autoid, status, brid, OPERATOR,ref)" _
    '        '        & " SELECT generate_tllog_code(b.autoid), TO_DATE('" & v_strCURRDATE & "','dd/mm/yyyy'), a.micode || '.01', 'BALANCE' ," _
    '        '        & " " & v_strFOMULA & ", 0, seq_mftran.nextval, 0, '" & v_strBRID & "', '+' " _
    '        '        & ", 'Giao dịch chạy batch cuối ngày(' || generate_tllog_code(b.autoid) || ',' || '" & v_strCURRDATE & "' || ')'" _
    '        '        & " FROM " _
    '        '        & " (select substr(maacctno,1,3) micode, nvl(sum(nvl(balance,0)),0) X " _
    '        '        & "  from mamast where status = 0 and deleted = 0 and brid = '" & v_strBRID & "' group by substr(maacctno,1,3)) a" _
    '        '        & " , tmp_tllog b"
    '        'v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)


    '        'v_strSQL = "SELECT TO_CHAR(MIN(TXDATE),'dd/mm/yyyy') FROM MFTRANA"
    '        'v_dsMF = v_DataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
    '        'If v_dsMF.Tables(0).Rows(0)(0).ToString.Trim <> "" Then
    '        '    v_strMinDate = v_dsMF.Tables(0).Rows(0)(0)
    '        'Else
    '        '    v_strMinDate = v_strCURRDATE
    '        'End If

    '        ''Neu mindate < passdate xoa dl thua trong bang all
    '        'If CheckPassDate(v_strMinDate, v_strPassDate) Then
    '        '    v_strSQL = "DELETE FROM MFTRANA WHERE TXDATE<TO_DATE('" & v_strPassDate & "','dd/mm/yyyy')"
    '        '    v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
    '        'End If
    '        ''Neu mindate > passdate them dl tu bang all_all vao bang all
    '        'If Not CheckPassDate(v_strMinDate, v_strPassDate) Then
    '        '    v_strSQL = "INSERT INTO MFTRANA SELECT * FROM MFTRANA_ALL" _
    '        '                & " WHERE TXDATE>=TO_DATE('" & v_strPassDate & "','dd/mm/yyyy')" _
    '        '                & " AND TXDATE<TO_DATE('" & v_strMinDate & "','dd/mm/yyyy')" _
    '        '                & " AND BRID='" & v_strBRID & "'"
    '        '    v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
    '        'End If

    '        'v_strSQL = "delete from mftran where brid = '" & v_strBRID & "'"
    '        'v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)


    '        'bangpv
    '        'Thay đổi trạng thái TTBT
    '        v_DataAccess.SetSysVar("SYSTEM", "sett_t3", v_strBRID, OPERATION_INACTIVE)
    '        v_DataAccess.SetSysVar("SYSTEM", "sett_t1", v_strBRID, OPERATION_INACTIVE)
    '        v_DataAccess.SetSysVar("SYSTEM", "sett_td", v_strBRID, OPERATION_INACTIVE)
    '        v_DataAccess.SetSysVar("SYSTEM", "room_status", v_strBRID, OPERATION_INACTIVE)
    '        v_DataAccess.SetSysVar("SYSTEM", "bank_status_t3", v_strBRID, OPERATION_INACTIVE)
    '        v_DataAccess.SetSysVar("SYSTEM", "bank_status_t1", v_strBRID, OPERATION_INACTIVE)
    '        v_DataAccess.SetSysVar("SYSTEM", "bank_status_td", v_strBRID, OPERATION_INACTIVE)
    '        'end bangpv 


    '        'Delete du lieu o Database HN

    '        'RA
    '        v_lngErrCode = BatchTranDelete(v_strBRID, v_strCURRDATE, v_strPassDate, "RA", v_DataAccess)
    '        'CS
    '        v_lngErrCode = BatchTranDelete(v_strBRID, v_strCURRDATE, v_strPassDate, "CS", v_DataAccess)
    '        'SF
    '        v_lngErrCode = BatchTranDelete(v_strBRID, v_strCURRDATE, v_strPassDate, "SF", v_DataAccess)
    '        'MA
    '        v_lngErrCode = BatchTranDelete(v_strBRID, v_strCURRDATE, v_strPassDate, "MA", v_DataAccess)
    '        'IA
    '        v_lngErrCode = BatchTranDelete(v_strBRID, v_strCURRDATE, v_strPassDate, "IA", v_DataAccess)
    '        'RG
    '        v_lngErrCode = BatchTranDelete(v_strBRID, v_strCURRDATE, v_strPassDate, "RG", v_DataAccess)
    '        'TLLOG
    '        v_strSQL = "delete from tllog a " _
    '                & " where a.status in (3,4) and a.brid = '" & v_strBRID & "' AND a.tltxcd not LIKE '60%'"
    '        v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
    '        'MF
    '        v_strSQL = "delete from mftran where brid = '" & v_strBRID & "'"
    '        v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)

    '        'Chay batch CA
    '        v_lngErrCode = BatchCA(v_strBRID, v_strTLID, v_strCURRDATE, v_DataAccess)
    '        'v_lngErrCode = BatchCA(v_strBRID, v_strTLID, v_strCURRDATE, v_HostDataAccess)
    '        'If v_strBatchType <> "1" Then
    '        '    v_lngErrCode = BatchCA(v_strBRID, v_strTLID, v_strCURRDATE, v_HcmDataAccess)
    '        'End If

    '        ' commit

    '        If v_strBatchType <> "1" Then
    '            v_HcmDataAccess.Commit()
    '        End If
    '        v_DataAccess.Commit()
    '        'v_HostDataAccess.Commit()
    '        Return v_lngErrCode

    '    Catch ex As Exception
    '        If blnTran Then
    '            v_DataAccess.Rollback()
    '            'v_HostDataAccess.Rollback()
    '            If v_strBatchType <> "1" Then
    '                v_HcmDataAccess.Rollback()
    '            End If
    '        End If
    '        'LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
    '        '             & "Error code: " & v_lngErrCode.ToString() & vbNewLine _
    '        '             & "Error message: " & v_strErrorMessage, EventLogEntryType.Information)
    '        Throw ex
    '    Finally
    '        If Not v_dsMF Is Nothing Then
    '            v_dsMF.Dispose()
    '        End If
    '        If Not v_DataAccess Is Nothing Then
    '            v_DataAccess.Dispose()
    '        End If
    '        If Not v_HcmDataAccess Is Nothing Then
    '            v_HcmDataAccess.Dispose()
    '        End If
    '        'If Not v_HostDataAccess Is Nothing Then
    '        '    v_HostDataAccess.Dispose()
    '        'End If
    '    End Try
    'End Function
    'bangpv: save file CA
    Public Function SaveFileCA(ByVal pv_strSaveMessage As String, ByVal v_strTLName As String, _
                               ByVal v_strTLTXCD As String, ByRef v_strLocalDir As String, ByRef v_strFileName As String, ByVal v_strStatus As String) As Long
        Dim v_oWriter As System.IO.StreamWriter
        Dim v_DataAccess As New DataAccess
        Try
            Dim v_strAppPath As String = System.AppDomain.CurrentDomain.BaseDirectory
            'Doan nay dung de tro toi thu muc share chung 
            'Dim v_intErr As Integer = v_DataAccess.GetSysVar("CA", "APP_LOCATION", v_strAppPath)
            'v_DataAccess.GetSysVar("CA", "VSD_CA", v_strLocalDir)
            'v_strLocalDir = v_strLocalDir & "\Server"
            'bangpv_ save file to ftpserver 

            Dim v_strSQL = "select to_char(to_date(varvalue,'dd/mm/yyyy'),'YYYYMMDD') date_, to_char(to_date(varvalue,'dd/mm/yyyy'),'YYYYMMDD')||''''||to_char(sysdate,'hh24miss') time from sysvar where varname ='CURRDATE' and brid ='0008'"
            Dim v_trace As System.Data.DataSet = v_DataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            Dim v_strTime As String = v_trace.Tables(0).Rows(0)("time")
            v_strLocalDir = v_trace.Tables(0).Rows(0)("date_")

            Dim f As New IO.DirectoryInfo(v_strAppPath & "\Log")
            If Not f.Exists Then
                Directory.CreateDirectory(v_strAppPath & "\Log")
            End If

            v_strAppPath = v_strAppPath & "\Log\" & v_strLocalDir

            f = New IO.DirectoryInfo(v_strAppPath)
            If Not f.Exists Then
                Directory.CreateDirectory(v_strAppPath)
            End If
            'Xóa file cũ nếu đã tạo 1 lần:
            Dim v_arrFileDel() As String
            v_arrFileDel = v_strTLTXCD.Split("'")
            'Dim v_oWriter As System.IO.StreamWriter

            If File.Exists(v_strAppPath & "\" & v_strTLName & "'" & v_strTLTXCD & "'" & v_strTime & "'" & v_strStatus & ".bat") Then
                File.Delete(v_strAppPath & "\" & v_strTLName & "'" & v_strTLTXCD & "'" & v_strTime & "'" & v_strStatus & ".bat")
            End If

            'v_oWriter = New StreamWriter(v_strAppPath & "\" & v_strTLName & "'" & v_strTLTXCD & "'" & v_strTime & ".bat")
            v_oWriter = New StreamWriter(v_strAppPath & "\" & v_strTLName & "'" & v_strTLTXCD & "'" & v_strTime & "'" & v_strStatus & ".bat")
            v_oWriter.WriteLine("cd " & v_strAppPath)
            v_oWriter.WriteLine(Left(v_strAppPath, 2))
            v_oWriter.WriteLine("del " & " " & v_strTLName & "'" & v_arrFileDel(0) & "'" & v_arrFileDel(1) & "*.dat")
            v_oWriter.WriteLine("exit ")
            v_oWriter.Close()

            Dim v_oProcess As Process
            v_oProcess = New Process

            v_oProcess.StartInfo.FileName = v_strAppPath & "\" & v_strTLName & "'" & v_strTLTXCD & "'" & v_strTime & "'" & v_strStatus & ".bat"
            v_oProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
            v_oProcess.StartInfo.CreateNoWindow = True
            v_oProcess.Start()
            'v_oProcess.WaitForExit()
            v_oProcess.Close()

            System.Threading.Thread.Sleep(3 * 1000)
            'xóa file .bat
            If File.Exists(v_strAppPath & "\" & v_strTLName & "'" & v_strTLTXCD & "'" & v_strTime & "'" & v_strStatus & ".bat") Then
                File.Delete(v_strAppPath & "\" & v_strTLName & "'" & v_strTLTXCD & "'" & v_strTime & "'" & v_strStatus & ".bat")
            End If

            'File.Delete(v_strAppPath & "\" & v_strTLName & (0) & v_strTLName(1) & "*.dat")
            v_strFileName = v_strTLName & "'" & v_strTLTXCD & "'" & v_strTime & "'" & v_strStatus
            If File.Exists(v_strAppPath & "\" & v_strFileName & ".dat") Then
                File.Delete(v_strAppPath & "\" & v_strFileName & ".dat")
            End If

            v_oWriter = New StreamWriter(v_strAppPath & "\" & v_strFileName & ".dat")
            v_oWriter.WriteLine(pv_strSaveMessage)
            v_oWriter.Close()
            ' Đẩy file lên FTP server
            Dim v_strServerAddress, v_strServerPort, v_strUsername, v_strPassword, v_strRemotePath, v_strRootPath As String
            v_DataAccess.GetSysVar("FILEFTPSVR", "ServerAddress", "0001", v_strServerAddress)
            v_DataAccess.GetSysVar("FILEFTPSVR", "ServerPort", "0001", v_strServerPort)
            v_DataAccess.GetSysVar("FILEFTPSVR", "Username", "0001", v_strUsername)
            v_DataAccess.GetSysVar("FILEFTPSVR", "Password", "0001", v_strPassword)
            v_DataAccess.GetSysVar("FILEFTPSVR", "RemotePath", "0001", v_strRemotePath)
            v_DataAccess.GetSysVar("FILEFTPSVR", "RootPath", "0001", v_strRootPath)

            v_oWriter = New StreamWriter(v_strAppPath & "\" & v_strFileName & ".bat")
            v_oWriter.WriteLine("@ftp -i -s:""%~f0""&GOTO:EOF")
            v_oWriter.WriteLine("open " & v_strServerAddress)
            v_oWriter.WriteLine(v_strUsername)
            v_oWriter.WriteLine(v_strPassword)
            v_oWriter.WriteLine("lcd " & """" & v_strAppPath & """")
            v_oWriter.WriteLine("cd " & v_strRemotePath)
            v_oWriter.WriteLine("binary")
            v_oWriter.WriteLine("put " & v_strFileName & ".dat" & " " & v_strFileName & ".dat")
            v_oWriter.WriteLine("bye" & vbCrLf)

            v_oWriter.Close()

            'Dim v_oProcess As Process
            v_oProcess = New Process

            v_oProcess.StartInfo.FileName = v_strAppPath & "\" & v_strFileName & ".bat"
            v_oProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
            v_oProcess.StartInfo.CreateNoWindow = True
            v_oProcess.Start()
            'v_oProcess.WaitForExit()
            v_oProcess.Close()


            'If File.Exists(v_strAppPath & "\" & v_strFileName & ".bat") Then
            '    File.Delete(v_strAppPath & "\" & v_strFileName & ".bat")
            'End If
            Return 0
        Catch ex As Exception
            LogError.Write(ex.Message, EventLogEntryType.Error, gc_MODULE_HOST)
            Return -1
        End Try
        GC.Collect()
    End Function
    'end bangpv 
    'bangpv: Function nhận file FTP
    Public Function GETFTP(ByRef pv_strObjMsg As String) As Long
        Dim v_lngErrCode As Long = ERR_SYSTEM_OK
        Dim v_ServerAddress As String
        Dim v_ServerPort As String
        Dim v_Username As String
        Dim v_Password As String
        Dim v_RemotePath As String
        Dim v_xmlDocument As New Xml.XmlDocument
        Dim mv_xftpFTPEngine As New FTPEngine
        Dim mv_xzipEngine As New ZipEngine
        Dim v_Prefix As String
        'Dim v_strSQL As String, v_DataAccess As New DataAccess
        'Dim v_xmlDocument As New Xml.XmlDocument


        Try
            v_xmlDocument.LoadXml(pv_strObjMsg)
            Dim v_strBrid As String = Trim(v_xmlDocument.DocumentElement.Attributes(gc_AtributeBRID).Value.ToString)
            Dim v_strClause As String = Trim(v_xmlDocument.DocumentElement.Attributes(gc_AtributeCLAUSE).Value.ToString)
            Dim v_strTxdate As String = Trim(v_xmlDocument.DocumentElement.Attributes(gc_AtributeTXDATE).Value.ToString)
            Dim v_Clause() = v_strClause.Split(":")
            v_ServerAddress = v_Clause(0)
            v_ServerPort = v_Clause(1)
            v_Username = v_Clause(2)
            v_Password = v_Clause(3)
            v_RemotePath = v_Clause(4)
            Dim v_strAppPath As String
            v_strAppPath = System.AppDomain.CurrentDomain.BaseDirectory
            Select Case v_strBrid
                Case "0001"
                    v_Prefix = "LISTED"
                Case "0003"
                    v_Prefix = "UPCOM"
                Case "0004"
                    v_Prefix = "BOND"
                Case "0005"
                    v_Prefix = "USDBOND"
                Case "0006"
                    v_Prefix = "BILL_VND"
            End Select

            'Hanm5 Edit
            Dim v_oWriter As System.IO.StreamWriter

            If File.Exists(v_strAppPath & "data\" & v_strBrid & "_" & v_strTxdate & ".bat") Then
                File.Delete(v_strAppPath & "data\" & v_strBrid & "_" & v_strTxdate & ".bat")
            End If


            v_oWriter = New StreamWriter(v_strAppPath & "data\" & v_strBrid & "_" & v_strTxdate & ".bat")
            v_oWriter.WriteLine("@ftp -i -s:""%~f0""&GOTO:EOF")
            v_oWriter.WriteLine("open " & v_ServerAddress)
            v_oWriter.WriteLine(v_Username)
            v_oWriter.WriteLine(v_Password)
            v_oWriter.WriteLine("lcd " & """" & v_strAppPath & "data" & """")
            v_oWriter.WriteLine("cd " & v_RemotePath)
            v_oWriter.WriteLine("binary")
            If v_strBrid = "0002" Then
                v_oWriter.WriteLine("get " & "astdl" & v_strTxdate & ".txt" & " " & v_strBrid & "_ASTDL_" & v_strTxdate & ".txt")
                v_oWriter.WriteLine("get " & "astpt" & v_strTxdate & ".txt" & " " & v_strBrid & "_ASTPT_" & v_strTxdate & ".txt")
            Else
                'bangpv: Sửa để lấy file ký số
                v_oWriter.WriteLine("get " & v_Prefix & "_TRADING_RESULT" & v_strTxdate & ".zip " & v_Prefix & "_TRADING_RESULT" & v_strTxdate & ".zip")
                'v_oWriter.WriteLine("get " & v_Prefix & "_TRADING_RESULT" & v_strTxdate & "zip.enc " & v_Prefix & "_TRADING_RESULT" & v_strTxdate & "_Sign.zip.enc")
                'v_oWriter.WriteLine("get " & v_Prefix & "_TRADING_RESULT" & v_strTxdate & "_Sign.zip_SessionKey.enc " & v_Prefix & "_TRADING_RESULT" & v_strTxdate & "_Sign.zip_SessionKey.enc")
            End If
            v_oWriter.WriteLine("bye" & vbCrLf)
            v_oWriter.Close()
            Dim v_oProcess As Process
            v_oProcess = New Process

            v_oProcess.StartInfo.FileName = v_strAppPath & "data\" & v_strBrid & "_" & v_strTxdate & ".bat" 'v_strAppPath & "data\" & v_strBrid & "_" & v_strTxdate & ".bat"
            v_oProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
            v_oProcess.StartInfo.CreateNoWindow = True
            v_oProcess.Start()
            'v_oProcess.WaitForExit()
            v_oProcess.Close()

            System.Threading.Thread.Sleep(30 * 1000)

            'Giải mã ssk, giải mã file mã hóa, lấy file dữ liệu gốc
            If v_strBrid <> "0002" Then
                If Not ServerBussinessCA.DecryptTrade_HNX(v_strAppPath & "data\" & v_Prefix & "_TRADING_RESULT" & v_strTxdate, v_Prefix & "_TRADING_RESULT" & v_strTxdate) Then
                    Return -1
                    Exit Function

                End If
            End If
            If v_strBrid = "0002" Then
                If File.Exists(v_strAppPath & "data\" & v_strBrid & "_ASTDL_" & v_strTxdate & ".txt") _
                              And File.Exists(v_strAppPath & "data\" & v_strBrid & "_ASTPT_" & v_strTxdate & ".txt") Then
                    Return 0
                Else
                    Return -1
                    Exit Function
                End If
            Else

                If File.Exists(v_strAppPath & "data\" & v_Prefix & "_TRADING_RESULT" & v_strTxdate & ".zip") Then
                    mv_xzipEngine.UnzipFile(v_strAppPath & "data", v_Prefix & "_TRADING_RESULT" & v_strTxdate & ".zip", v_Prefix & "_TRADING_RESULT" & v_strTxdate & ".xml")
                    Dim f As New IO.FileInfo(v_strAppPath & "data\" & v_strBrid & "_" & v_strTxdate & ".xml")
                    If f.Exists Then
                        f.Delete()
                    End If
                    Rename(v_strAppPath & "data\" & v_Prefix & "_TRADING_RESULT" & v_strTxdate & ".xml", v_strAppPath & "data\" & v_strBrid & "_" & v_strTxdate & ".xml")
                    Return 0

                Else
                    Return -1

                    Exit Function
                End If
            End If
            'End Hanm5 Edit

        Catch ex As Exception
            LogError.Write(ex.Message, EventLogEntryType.Error, gc_MODULE_HOST)
            Return -1
        End Try
    End Function

    Public Function SendFTPtoHNX(ByRef pv_strObjMsg As String) As Long
        Dim v_lngErrCode As Long = ERR_SYSTEM_OK
        Dim v_strServerAddress As String
        Dim v_strServerPort As String
        Dim v_strUsername As String
        Dim v_strPassword As String
        Dim v_strRemotePath As String
        Dim v_xmlDocument As New XmlDocumentEx
        Dim v_strSQL As String, v_DataAccess As New DataAccess
        Dim v_strRootPath, v_strLocalDir, v_strLocalFileName As String

        Try
            v_xmlDocument.LoadXml(pv_strObjMsg)
            Dim v_strBrid As String = Trim(v_xmlDocument.DocumentElement.Attributes(gc_AtributeBRID).Value.ToString)
            Dim v_strClause As String = Trim(v_xmlDocument.DocumentElement.Attributes(gc_AtributeCLAUSE).Value.ToString)
            Dim v_strTxdate As String = Trim(v_xmlDocument.DocumentElement.Attributes(gc_AtributeTXDATE).Value.ToString)

            v_DataAccess.GetSysVar("EXPORT", "ServerAddress", v_strBrid, v_strServerAddress)
            v_DataAccess.GetSysVar("EXPORT", "ServerPort", v_strBrid, v_strServerPort)
            v_DataAccess.GetSysVar("EXPORT", "Username", v_strBrid, v_strUsername)
            v_DataAccess.GetSysVar("EXPORT", "Password", v_strBrid, v_strPassword)
            v_DataAccess.GetSysVar("EXPORT", "RemotePath", v_strBrid, v_strRemotePath)
            v_DataAccess.GetSysVar("VSDFTPSVR", "RootPath", v_strBrid, v_strRootPath)

            v_strLocalDir = v_strRootPath & v_strClause.Split(":")(0)
            v_strLocalFileName = v_strClause.Split(":")(1)

            Dim v_oWriter As System.IO.StreamWriter

            If File.Exists(v_strLocalDir & "\" & Mid(v_strLocalFileName, 1, v_strLocalFileName.Length - 4) & ".bat") Then
                File.Delete(v_strLocalDir & "\" & Mid(v_strLocalFileName, 1, v_strLocalFileName.Length - 4) & ".bat")
            End If

            v_oWriter = New StreamWriter(v_strLocalDir & "\" & Mid(v_strLocalFileName, 1, v_strLocalFileName.Length - 4) & ".bat")
            v_oWriter.WriteLine("@ftp -i -s:""%~f0""&GOTO:EOF")
            v_oWriter.WriteLine("open " & v_strServerAddress)
            v_oWriter.WriteLine(v_strUsername)
            v_oWriter.WriteLine(v_strPassword)
            v_oWriter.WriteLine("lcd " & v_strLocalDir)
            v_oWriter.WriteLine("cd " & v_strRemotePath)
            v_oWriter.WriteLine("binary")
            v_oWriter.WriteLine("put " & v_strLocalFileName & " " & v_strLocalFileName)
            v_oWriter.WriteLine("bye" & vbCrLf)

            v_oWriter.Close()

            Dim v_oProcess As Process
            v_oProcess = New Process

            v_oProcess.StartInfo.FileName = v_strLocalDir & "\" & Mid(v_strLocalFileName, 1, v_strLocalFileName.Length - 4) & ".bat"
            v_oProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
            v_oProcess.StartInfo.CreateNoWindow = True
            v_oProcess.Start()
            'Dim v_stranc As String = v_oProcess.StartTime
            'v_oProcess.WaitForExit()
            v_oProcess.Close()

            'If File.Exists(v_strLocalDir & "\" & Mid(v_strLocalFileName, 1, v_strLocalFileName.Length - 4) & ".bat") Then
            '    File.Delete(v_strLocalDir & "\" & Mid(v_strLocalFileName, 1, v_strLocalFileName.Length - 4) & ".bat")
            'End If
            Return 0
        Catch ex As Exception
            LogError.Write(ex.Message, EventLogEntryType.Error, gc_MODULE_HOST)
            Return -1
        Finally
            v_DataAccess.Dispose()
        End Try
    End Function
    'end bangpv
    Public Function BEGINOFDAY(ByRef pv_strObjMsg As String) As Long
        Dim v_lngErrCode As Long = ERR_SYSTEM_OK
        Dim v_strErrorSource As String = "Host.SystemAdmin.BEGINOFDAY", v_strErrorMessage As String = ""
        Dim v_strSQL As String, v_DataAccess As New DataAccess
        Dim v_xmlDocument As New Xml.XmlDocument
        Dim blnTran As Boolean = False


        Try
            v_DataAccess.NewDBInstance(gc_MODULE_HOST)
            v_xmlDocument.LoadXml(pv_strObjMsg)
            Dim v_strBRID As String = Trim(v_xmlDocument.DocumentElement.Attributes(gc_AtributeBRID).Value.ToString)
            Dim v_strCURRDATE As String = Trim(v_xmlDocument.DocumentElement.Attributes(gc_AtributeTXDATE).Value.ToString)
            Dim v_strTLID As String = Trim(v_xmlDocument.DocumentElement.Attributes(gc_AtributeTLID).Value.ToString)
            blnTran = True
            v_DataAccess.BeginTran()
            ' Change status of branch
            'v_DataAccess.SetSysVar("SYSTEM", "BRSTATUS", v_strBRID, OPERATION_ACTIVE)
            'set BRSTATUS
            v_strSQL = "UPDATE SYSVAR SET VARVALUE = '1'" _
                       & " WHERE GRNAME='SYSTEM' AND VARNAME='BRSTATUS' AND BRID='" & v_strBRID & "'"
            v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
            'set CURRDATE
            v_strSQL = "UPDATE SYSVAR SET VARVALUE = '" & v_strCURRDATE & "'" _
                       & " WHERE GRNAME='SYSTEM' AND VARNAME='CURRDATE' AND BRID='" & v_strBRID & "'"
            v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)


            v_strSQL = "UPDATE SYSVAR SET VARVALUE = '0'" _
                        & " WHERE GRNAME='SYSTEM' AND VARNAME='DATA_BATCH' AND BRID='" & v_strBRID & "'"
            v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
            'v_DataAccess.SetSysVar("SYSTEM", "CURRDATE", v_strBRID, v_strCURRDATE)
            'Chay batch CA
            v_lngErrCode = BatchCA(v_strBRID, v_strTLID, v_strCURRDATE, v_DataAccess, "BEGIN")
            v_lngErrCode = BatchRG(v_strBRID, v_strTLID, v_strCURRDATE, v_DataAccess, "BEGIN")
            'set room_status
            v_strSQL = "UPDATE SYSVAR SET VARVALUE = '0'" _
                       & " WHERE GRNAME='SYSTEM' AND VARNAME='room_status' AND BRID='" & v_strBRID & "'"
            v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
            'HaNM5(25/05/2021): Cap nhat trang thai nhan file HoSE
            If v_strBRID = "0002" Then
                v_strSQL = "UPDATE SYSVAR SET VARVALUE = '0'" _
                       & " WHERE GRNAME='SYSTEM' AND VARNAME IN ('SETT_UPD_ASTDL_STATUS', 'SETT_UPD_ASTPT_STATUS') AND BRID='" & v_strBRID & "'"
                v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
            End If
            ' commit
            v_DataAccess.Commit()
            Return v_lngErrCode
        Catch ex As Exception
            If blnTran Then
                v_DataAccess.Rollback()
            End If
            'LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
            '             & "Error code: " & v_lngErrCode.ToString() & vbNewLine _
            '             & "Error message: " & v_strErrorMessage, EventLogEntryType.Information)
            Throw ex
        End Try
    End Function

    'Add by thonm
    'Cap nhat thong tin user dang nhap vao hethong
    'Public Function WriteSessionIn(ByRef pv_strObjMsg As String) As Long
    '    Dim v_lngErrCode As Long = ERR_SYSTEM_OK
    '    Dim v_strErrorSource As String = "Host.SystemAdmin.WriteSessionIn", v_strErrorMessage As String = ""
    '    Dim v_strSQL As String, v_DataAccess As New DataAccess
    '    Dim v_xmlDocument As New Xml.XmlDocument
    '    Dim blnTran As Boolean = False

    '    Try
    '        v_DataAccess.NewDBInstance(gc_MODULE_HOST)
    '        v_xmlDocument.LoadXml(pv_strObjMsg)
    '        Dim v_strBRID As String = Trim(v_xmlDocument.DocumentElement.Attributes(gc_AtributeBRID).Value.ToString)
    '        Dim v_strCURRDATE As String = Trim(v_xmlDocument.DocumentElement.Attributes(gc_AtributeTXDATE).Value.ToString)
    '        Dim v_strLoginTime As String = Trim(v_xmlDocument.DocumentElement.Attributes(gc_AtributeTXTIME).Value.ToString)
    '        Dim v_strTLID As String = Trim(v_xmlDocument.DocumentElement.Attributes(gc_AtributeTLID).Value.ToString)
    '        Dim v_strClause As String = Trim(v_xmlDocument.DocumentElement.Attributes(gc_AtributeCLAUSE).Value.ToString)
    '        Dim v_strPCName, v_strIPAddress As String
    '        Dim v_strBDSPCName, v_strBDSIPAddress As String
    '        Dim v_strBDS, v_strClient As String

    '        v_strBDS = v_strClause.Split("#")(1)
    '        v_strBDSPCName = v_strBDS.Split("|")(0)
    '        v_strBDSIPAddress = v_strBDS.Split("|")(1)

    '        v_strClient = v_strClause.Split("#")(0)
    '        v_strPCName = v_strClient.Split("|")(0)
    '        v_strIPAddress = v_strClient.Split("|")(1)

    '        blnTran = True
    '        v_DataAccess.BeginTran()
    '        v_strSQL = "SELECT * FROM TLSESSION WHERE TLID='" & v_strTLID & "'"
    '        Dim v_ds As DataSet

    '        v_ds = v_DataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
    '        If v_ds.Tables(0).Rows.Count > 0 Then
    '            v_strSQL = "UPDATE TLSESSION SET TIMECHECK='" & Now.Hour & ":" & Now.Minute & ":" & Now.Second & "' WHERE TLID='" & v_strTLID & "'"
    '        Else
    '            v_strSQL = "INSERT INTO TLSESSION(AUTOID, TLID, DATEIN, PCNAME, IPADDRESS, BRID, TIMEIN, IPBDS, PCBDS,TIMECHECK)" _
    '                        & " VALUES(seq_tlsession.nextval,'" & v_strTLID & "'," _
    '                        & "TO_DATE('" & v_strCURRDATE & "','dd/mm/yyyy'),'" & v_strPCName & "'," _
    '                        & "'" & v_strIPAddress & "','" & v_strBRID & "','" & v_strLoginTime & "'," _
    '                        & "'" & v_strBDSPCName & "','" & v_strBDSIPAddress & "','" & Now.Hour & ":" & Now.Minute & ":" & Now.Second & "')"

    '        End If
    '        v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)

    '        v_DataAccess.Commit()
    '        Return v_lngErrCode
    '    Catch ex As Exception
    '        If blnTran Then
    '            v_DataAccess.Rollback()
    '        End If
    '        'LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
    '        '             & "Error code: " & v_lngErrCode.ToString() & vbNewLine _
    '        '             & "Error message: " & v_strErrorMessage, EventLogEntryType.Information)
    '        Throw ex
    '    End Try
    'End Function

    'Add by thonm
    'Cap nhat thong tin user thoat khoi he thong
    'Public Function WriteSessionOut(ByRef pv_strObjMsg As String) As Long
    '    Dim v_lngErrCode As Long = ERR_SYSTEM_OK
    '    Dim v_strErrorSource As String = "Host.SystemAdmin.WriteSessionOut", v_strErrorMessage As String = ""
    '    Dim v_strSQL As String, v_DataAccess As New DataAccess
    '    Dim v_xmlDocument As New Xml.XmlDocument
    '    Dim blnTran As Boolean = False

    '    Try
    '        v_DataAccess.NewDBInstance(gc_MODULE_HOST)
    '        v_xmlDocument.LoadXml(pv_strObjMsg)
    '        Dim v_strBRID As String = Trim(v_xmlDocument.DocumentElement.Attributes(gc_AtributeBRID).Value.ToString)
    '        Dim v_strTLID As String = Trim(v_xmlDocument.DocumentElement.Attributes(gc_AtributeTLID).Value.ToString)
    '        Dim v_strCURRDATE As String = Trim(v_xmlDocument.DocumentElement.Attributes(gc_AtributeTXDATE).Value.ToString)
    '        blnTran = True
    '        v_DataAccess.BeginTran()

    '        v_strSQL = "INSERT INTO TLSESSIONALL(AUTOID, TLID, DATEIN, PCNAME, IPADDRESS, BRID,DATEOUT, TIMEIN,TIMEOUT, IPBDS, PCBDS)" _
    '                    & " SELECT seq_tlsessionall.nextval,TLID, DATEIN, PCNAME, IPADDRESS, BRID,TO_DATE('" & v_strCURRDATE & "','dd/mm/yyyy'),TIMEIN,'" & Now.Hour & ":" & Now.Minute & ":" & Now.Second & "'," _
    '                    & " IPBDS, PCBDS" _
    '                    & " FROM TLSESSION WHERE TLID='" & v_strTLID & "'"
    '        v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)

    '        v_strSQL = "DELETE FROM TLSESSION WHERE TLID='" & v_strTLID & "'"
    '        v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)

    '        v_DataAccess.Commit()
    '        Return v_lngErrCode
    '    Catch ex As Exception
    '        If blnTran Then
    '            v_DataAccess.Rollback()
    '        End If
    '        'LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
    '        '             & "Error code: " & v_lngErrCode.ToString() & vbNewLine _
    '        '             & "Error message: " & v_strErrorMessage, EventLogEntryType.Information)
    '        Throw ex
    '    End Try
    'End Function

    'Add by thonm
    'Cap nhat status user
    'Public Function UpdateUserAccess(ByRef pv_strObjMsg As String) As Long
    '    Dim v_lngErrCode As Long = ERR_SYSTEM_OK
    '    Dim v_strErrorSource As String = "Host.SystemAdmin.WriteSessionOut", v_strErrorMessage As String = ""
    '    Dim v_strSQL As String, v_DataAccess As New DataAccess
    '    Dim v_xmlDocument As New Xml.XmlDocument
    '    Dim blnTran As Boolean = False

    '    Try
    '        v_DataAccess.NewDBInstance(gc_MODULE_HOST)
    '        v_xmlDocument.LoadXml(pv_strObjMsg)
    '        Dim v_ds As DataSet
    '        Dim v_strTLID As String = Trim(v_xmlDocument.DocumentElement.Attributes(gc_AtributeTLID).Value.ToString)
    '        blnTran = True
    '        v_strSQL = "SELECT * FROM TLSESSION WHERE TLID='" & v_strTLID & "'"
    '        v_ds = v_DataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

    '        If v_ds.Tables(0).Rows.Count > 0 Then
    '            v_DataAccess.BeginTran()
    '            v_strSQL = "UPDATE TLSESSION SET TIMECHECK='" & Now.Hour & ":" & Now.Minute & ":" & Now.Second & "' WHERE TLID='" & v_strTLID & "'"
    '            v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
    '            v_strSQL = "DELETE FROM TLMESSAGES WHERE MSGSTATUS=1"
    '            v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
    '            v_DataAccess.Commit()
    '        Else
    '            v_lngErrCode = WriteSessionIn(pv_strObjMsg)
    '        End If

    '        Return v_lngErrCode
    '    Catch ex As Exception
    '        If blnTran Then
    '            v_DataAccess.Rollback()
    '        End If
    '        'LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
    '        '             & "Error code: " & v_lngErrCode.ToString() & vbNewLine _
    '        '             & "Error message: " & v_strErrorMessage, EventLogEntryType.Information)
    '        Throw ex
    '    End Try
    'End Function

    Public Sub UpdateUserNotUse()
        Dim v_obj As New DataAccess
        Try
            v_obj.NewDBInstance(gc_MODULE_HOST)
            Dim v_strSQL As String
            Dim v_ds As DataSet
            Dim v_strTime, v_strTLID, v_strBusDate As String
            v_obj.BeginTran()
            v_strSQL = "SELECT * FROM TLSESSION"
            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

            With v_ds.Tables(0)
                If .Rows.Count > 0 Then
                    For v_int As Integer = 0 To .Rows.Count - 1
                        v_strTime = .Rows(v_int)("TIMECHECK")
                        v_strTLID = .Rows(v_int)("TLID")
                        'v_obj.GetSysVar("SYSTEM", "CURRDATE", .Rows(v_int)("BRID"), v_strBusDate)
                        If TimeAdd(v_strTime) > 60 Then
                            v_strSQL = "INSERT INTO TLSESSIONALL(AUTOID, TLID, DATEIN, PCNAME, IPADDRESS, BRID,DATEOUT, TIMEIN,TIMEOUT, IPBDS, PCBDS)" _
                                        & " SELECT seq_tlsessionall.nextval,TLID, DATEIN, PCNAME, IPADDRESS, BRID,DATEIN,TIMEIN,'" & Now.Hour & ":" & Now.Minute & ":" & Now.Second & "'," _
                                        & " IPBDS, PCBDS" _
                                        & " FROM TLSESSION WHERE TLID='" & v_strTLID & "'"
                            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

                            v_strSQL = "DELETE FROM TLSESSION WHERE TLID='" & v_strTLID & "'"
                            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                            'LogError.Write("Update: " & v_strTLID, EventLogEntryType.SuccessAudit)
                        End If
                    Next
                End If
            End With
            v_obj.Commit()
        Catch ex As Exception
            v_obj.Rollback()
            'Throw ex
        Finally
            v_obj.Dispose()
        End Try
    End Sub

    'Add by thonm
    'Cap nhat thong tin user menu
    Public Function FavMenu(ByRef pv_strObjMsg As String) As Long
        Dim v_lngErrCode As Long = ERR_SYSTEM_OK
        Dim v_strErrorSource As String = "Host.SystemAdmin.Favmenu", v_strErrorMessage As String = ""
        Dim v_strSQL As String = "", v_DataAccess As New DataAccess
        Dim v_xmlDocument As New Xml.XmlDocument
        Dim blnTran As Boolean = False
        Dim v_nodeList As Xml.XmlNodeList

        Try
            v_DataAccess.NewDBInstance(gc_MODULE_HOST)
            v_xmlDocument.LoadXml(pv_strObjMsg)
            Dim v_strTLID As String = Trim(v_xmlDocument.DocumentElement.Attributes(gc_AtributeTLID).Value.ToString)

            blnTran = True
            v_DataAccess.BeginTran()

            v_strSQL = "DELETE FROM FAVMENU WHERE TLID='" & v_strTLID & "'"
            v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)

            v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ObjData")
            Dim v_strCMDID, v_strPRID, v_strMenuType, v_strLast, v_strAUTHCODE, v_strImgIndex, v_strObjName, v_strModeCode, v_strCMDCODE As String
            Dim v_strFLDNAME, v_strValue, v_strLev, v_strOrd, v_strCMDNAME As String
            For i As Integer = 0 To v_nodeList.Count - 1
                For j As Integer = 0 To v_nodeList.Item(i).ChildNodes.Count - 1
                    With v_nodeList.Item(i).ChildNodes(j)
                        v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                        v_strValue = .InnerText.ToString

                        Select Case Trim(v_strFLDNAME)
                            Case "COL_0"
                                v_strCMDID = Trim(v_strValue)
                            Case "COL_1"
                                v_strPRID = Trim(v_strValue)
                            Case "COL_2"
                                v_strLev = Trim(v_strValue)
                            Case "COL_3"
                                v_strImgIndex = Trim(v_strValue)
                            Case "COL_4"
                                v_strCMDNAME = Trim(v_strValue)
                            Case "COL_5"
                                v_strAUTHCODE = Trim(v_strValue)
                            Case "COL_6"
                                v_strOrd = Trim(v_strValue)
                            Case "COL_7"
                                v_strObjName = Trim(v_strValue)
                            Case "COL_8"
                                v_strModeCode = Trim(v_strValue)
                            Case "COL_9"
                                v_strCMDCODE = Trim(v_strValue)
                            Case "COL_10"
                                v_strLast = Trim(v_strValue)
                            Case "COL_11"
                                v_strMenuType = Trim(v_strValue)
                        End Select
                    End With
                Next

                v_strSQL = "INSERT INTO FAVMENU(AUTOID, TLID, CMDID, PRID, LEV, IMGINDEX, MENUTYPE," _
                            & " MODCODE, OBJNAME, CMDNAME, AUTHCODE, CMDORD, STATUS, DELETED, LAST)" _
                            & " VALUES(SEQ_FAVMENU.NEXTVAL, '" & v_strTLID & "', '" & v_strCMDID & "'," _
                            & "'" & v_strPRID & "'," & v_strLev & "," & v_strImgIndex & ",'" & v_strMenuType & "'," _
                            & "'" & v_strModeCode & "','" & v_strObjName & "','" & v_strCMDNAME & "','" & v_strAUTHCODE & "'," _
                            & v_strOrd & ",0,0,'" & v_strLast & "')"

                v_DataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)

            Next
            v_DataAccess.Commit()
            Return v_lngErrCode
        Catch ex As Exception
            If blnTran Then
                v_DataAccess.Rollback()
            End If
            'LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
            '             & "Error code: " & v_lngErrCode.ToString() & vbNewLine _
            '             & "Error message: " & v_strErrorMessage, EventLogEntryType.Information)
            Throw ex
        End Try
    End Function

    Private Function TimeAdd(ByVal pv_strTime As String) As Integer
        Return (Now.Hour * 3600 + Now.Minute * 60 + Now.Second) - Hour(pv_strTime) * 3600 + Minute(pv_strTime) * 60 + Second(pv_strTime)
    End Function

    Public Function BatchCA(ByVal v_strBRID As String, ByVal v_strTellerID As String, ByVal v_strCURRDATE As String, ByRef v_TranDataAccess As DataAccess, ByVal v_str_BEGINOREND As String) As Long
        Dim v_lngErr As Long = ERR_SYSTEM_OK
        Dim v_ds As DataSet
        Dim v_strSQL, v_strSQLTmp As String
        Dim v_int, v_inttmp As Integer

        Dim tr2 As TextWriterTraceListener
        Dim v_trace_status, v_trace_path As String
        Dim v_trace, v_tllog As DataSet
        Try
            'BangPV:  Lay lai curvar tren server 
            'v_lngErr = v_TranDataAccess.GetSysVar("SYSTEM", "CURRDATE", v_strBRID, v_strCURRDATE)
            'End BangPV:  Lay lai curvar tren server 
            v_trace_status = "0"
            v_lngErr = v_TranDataAccess.GetSysVar("SYSTEM", "TRACE_STATUS", v_trace_status)
            'v_trace_path = "C:\log_batch_sql_data.txt"
            Dim v_strTempDate As String
            If v_trace_status = "1" Then
                v_TranDataAccess.GetSysVar("SYSTEM", "TRACE_PATH", v_trace_path)
                v_strTempDate = Replace(v_strCURRDATE, "/", "_")

                If v_trace_path = "" Then
                    Dim v_app As New ApplicationServices.ApplicationBase
                    v_trace_path = v_app.Info.DirectoryPath & "\Log\" & v_strTempDate
                Else
                    v_trace_path = v_trace_path & v_strTempDate
                End If

                If Not System.IO.Directory.Exists(v_trace_path) Then
                    System.IO.Directory.CreateDirectory(v_trace_path)
                End If

                v_trace_path &= "\log_batch_br" & v_strBRID & "_tl" & v_strTellerID & ".txt"

                tr2 = New TextWriterTraceListener(System.IO.File.AppendText(v_trace_path))
                Trace.Listeners.Add(tr2)
                Trace.WriteLine("[Bắt đầu: BATCH - CA] " & DateTime.Now & vbCrLf)
            End If
            Dim v_strBUSDATE As String
            If v_str_BEGINOREND = "BEGIN" Then
                v_strBUSDATE = v_strCURRDATE
                ' luu ky
                v_strSQL = "SELECT A.* FROM TLLOG A WHERE A.BRID='" & v_strBRID & "' AND A.AUTOID IN (SELECT A.AUTOID FROM SAISSUE A, (SELECT VARVALUE FROM SYSVAR WHERE varname='CURRDATE' AND BRID = '" & v_strBRID & "' AND DELETED=0 AND STATUS=0) B " _
                        & " WHERE GET_TRADE_DATE(A.DEDATE-1,'" & v_strBRID & "','-') = TO_DATE(B.varvalue,'dd/mm/yyyy') " _
                        & " AND A.DELETED=0 AND A.STATUS=0 and a.apptype='CA' )"
                v_tllog = v_TranDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                For v_inttmp = 0 To v_tllog.Tables(0).Rows.Count - 1
                    v_strSQL = "SELECT * FROM APPDML WHERE TLTXCD = '6090' AND TXSTATUS = 3 AND DELETED=0 AND STATUS=0 and ORDNUM in (14,15,8,9,4,5, 18,19,20,21) ORDER BY TLTXCD, ORDNUM"
                    v_ds = v_TranDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

                    For v_int = 0 To v_ds.Tables(0).Rows.Count - 1
                        ' bangpv: them vao de ghep cau lenh sql dai qua : & " " & v_ds.Tables(0).Rows(v_int)("DMLSQL1")
                        v_strSQL = v_ds.Tables(0).Rows(v_int)("DMLSQL") & " " & v_ds.Tables(0).Rows(v_int)("DMLSQL1")
                        'end bangpv 
                        v_strSQL = Replace(v_strSQL, "?BRID", "'" & v_strBRID & "'")
                        v_strSQL = Replace(v_strSQL, "?BUSDATE", "to_date('" & v_strBUSDATE & "','dd/mm/yyyy')")
                        v_strSQL = Replace(v_strSQL, "?AUTOID", v_tllog.Tables(0).Rows(v_inttmp)("AUTOID"))
                        v_strSQL = Replace(v_strSQL, "?SICODE", "'" & v_tllog.Tables(0).Rows(v_inttmp)("SICODE") & "'")
                        v_strSQL = Replace(v_strSQL, "?TXNUM", "'" & v_tllog.Tables(0).Rows(v_inttmp)("TXNUM") & "'")
                        v_strSQL = Replace(v_strSQL, "?TXNAME", "'" & v_tllog.Tables(0).Rows(v_inttmp)("TXNAME") & "'")
                        v_strSQL = Replace(v_strSQL, "?TXDATE", "TO_DATE('" & CDate(v_tllog.Tables(0).Rows(v_inttmp)("TXDATE")).ToString("dd/MM/yyyy") & "','dd/mm/yyyy')")
                        v_strSQL = Replace(v_strSQL, "?TLTXCD", "'" & v_tllog.Tables(0).Rows(v_inttmp)("TLTXCD") & "'")
                        v_strSQL = Replace(v_strSQL, "?ISSUE_TYPE", "'" & v_tllog.Tables(0).Rows(v_inttmp)("COL_VALUE07") & "'")

                        If v_trace_status = "1" And v_ds.Tables(0).Rows(v_int)("TRACE") = 1 Then
                            Trace.WriteLine("[BATCH - CA] " & DateTime.Now & " :" & vbCrLf & " -o- Câu lệnh thứ #" & v_ds.Tables(0).Rows(v_int)("ORDNUM") & "-o-" & vbCrLf & v_strSQL & vbCrLf)
                            If InStr(v_strSQL.ToUpper, "SELECT") > 0 Then
                                Trace.WriteLine("-o- Dữ liệu câu lệnh thứ #" & v_ds.Tables(0).Rows(v_int)("ORDNUM") & "-o-")
                                v_strSQLTmp = v_strSQL.Substring(InStr(v_strSQL.ToUpper, "SELECT") - 1)
                                v_trace = v_TranDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                                Trace.WriteLine(v_trace.GetXml & vbCrLf)
                            End If
                        End If

                        v_TranDataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
                    Next

                Next
            End If
            If v_str_BEGINOREND = "END" Then
                v_strSQL = "select GET_TRADE_DATE(to_date('" & v_strCURRDATE & "','dd/mm/yyyy')+1,'" & v_strBRID & "','+') from dual"
                v_tllog = v_TranDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                If v_tllog.Tables(0).Rows.Count > 0 Then
                    v_strBUSDATE = CDate(v_tllog.Tables(0).Rows(0)(0)).ToString("dd/MM/yyyy")
                Else
                    v_strBUSDATE = v_strCURRDATE
                End If
                ' luu ky
                v_strSQL = "SELECT A.* FROM TLLOG A WHERE A.BRID='" & v_strBRID & "' AND A.AUTOID IN (SELECT A.AUTOID FROM SAISSUE A, (SELECT VARVALUE FROM SYSVAR WHERE varname='CURRDATE' AND BRID = '" & v_strBRID & "' AND DELETED=0 AND STATUS=0) B " _
                        & " WHERE GET_TRADE_DATE(A.DEDATE-1,'" & v_strBRID & "','-') = TO_DATE(B.varvalue,'dd/mm/yyyy') " _
                        & " AND A.DELETED=0 AND A.STATUS=0 and a.apptype='CA' )"
                v_tllog = v_TranDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                For v_inttmp = 0 To v_tllog.Tables(0).Rows.Count - 1
                    v_strSQL = "SELECT * FROM APPDML WHERE TLTXCD = '6090' AND TXSTATUS = 3 AND DELETED=0 AND STATUS=0 and not ORDNUM in (18,19,20,21) ORDER BY TLTXCD, ORDNUM"
                    v_ds = v_TranDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

                    For v_int = 0 To v_ds.Tables(0).Rows.Count - 1
                        ' bangpv: them vao de ghep cau lenh sql dai qua : & " " & v_ds.Tables(0).Rows(v_int)("DMLSQL1")
                        v_strSQL = v_ds.Tables(0).Rows(v_int)("DMLSQL") & " " & v_ds.Tables(0).Rows(v_int)("DMLSQL1")
                        'end bangpv 
                        v_strSQL = Replace(v_strSQL, "?BRID", "'" & v_strBRID & "'")
                        v_strSQL = Replace(v_strSQL, "?BUSDATE", "to_date('" & v_strBUSDATE & "','dd/mm/yyyy')")
                        v_strSQL = Replace(v_strSQL, "?AUTOID", v_tllog.Tables(0).Rows(v_inttmp)("AUTOID"))
                        v_strSQL = Replace(v_strSQL, "?SICODE", "'" & v_tllog.Tables(0).Rows(v_inttmp)("SICODE") & "'")
                        v_strSQL = Replace(v_strSQL, "?TXNUM", "'" & v_tllog.Tables(0).Rows(v_inttmp)("TXNUM") & "'")
                        v_strSQL = Replace(v_strSQL, "?TXNAME", "'" & v_tllog.Tables(0).Rows(v_inttmp)("TXNAME") & "'")
                        v_strSQL = Replace(v_strSQL, "?TXDATE", "TO_DATE('" & CDate(v_tllog.Tables(0).Rows(v_inttmp)("TXDATE")).ToString("dd/MM/yyyy") & "','dd/mm/yyyy')")
                        v_strSQL = Replace(v_strSQL, "?TLTXCD", "'" & v_tllog.Tables(0).Rows(v_inttmp)("TLTXCD") & "'")
                        v_strSQL = Replace(v_strSQL, "?ISSUE_TYPE", "'" & v_tllog.Tables(0).Rows(v_inttmp)("COL_VALUE07") & "'")

                        If v_trace_status = "1" And v_ds.Tables(0).Rows(v_int)("TRACE") = 1 Then
                            Trace.WriteLine("[BATCH - CA] " & DateTime.Now & " :" & vbCrLf & " -o- Câu lệnh thứ #" & v_ds.Tables(0).Rows(v_int)("ORDNUM") & "-o-" & vbCrLf & v_strSQL & vbCrLf)
                            If InStr(v_strSQL.ToUpper, "SELECT") > 0 Then
                                Trace.WriteLine("-o- Dữ liệu câu lệnh thứ #" & v_ds.Tables(0).Rows(v_int)("ORDNUM") & "-o-")
                                v_strSQLTmp = v_strSQL.Substring(InStr(v_strSQL.ToUpper, "SELECT") - 1)
                                v_trace = v_TranDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                                Trace.WriteLine(v_trace.GetXml & vbCrLf)
                            End If
                        End If

                        v_TranDataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
                    Next

                Next

                ' giao dich
                v_strSQL = "SELECT A.* FROM TLLOG A WHERE A.BRID='" & v_strBRID & "' AND A.AUTOID IN (SELECT A.AUTOID FROM SAISSUE A, (SELECT VARVALUE FROM SYSVAR WHERE varname='CURRDATE' AND BRID = '" & v_strBRID & "' AND DELETED=0 AND STATUS=0) B " _
                        & " WHERE GET_TRADE_DATE(A.TRANDATE-1,'" & v_strBRID & "','-') = TO_DATE(B.varvalue,'dd/mm/yyyy') " _
                        & " AND A.DELETED=0 AND A.STATUS=1 and a.apptype='CA' )"
                v_tllog = v_TranDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                For v_inttmp = 0 To v_tllog.Tables(0).Rows.Count - 1
                    v_strSQL = "SELECT * FROM APPDML WHERE TLTXCD = '2091' AND TXSTATUS = 3 AND DELETED=0 AND STATUS=0 ORDER BY TLTXCD, ORDNUM"
                    v_ds = v_TranDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

                    For v_int = 0 To v_ds.Tables(0).Rows.Count - 1
                        ' bangpv: them vao de ghep cau lenh sql dai qua : & " " & v_ds.Tables(0).Rows(v_int)("DMLSQL1")
                        v_strSQL = v_ds.Tables(0).Rows(v_int)("DMLSQL") & " " & v_ds.Tables(0).Rows(v_int)("DMLSQL1")
                        'end bangpv
                        v_strSQL = Replace(v_strSQL, "?BRID", "'" & v_strBRID & "'")
                        v_strSQL = Replace(v_strSQL, "?BUSDATE", "to_date('" & v_strBUSDATE & "','dd/mm/yyyy')")
                        v_strSQL = Replace(v_strSQL, "?AUTOID", v_tllog.Tables(0).Rows(v_inttmp)("AUTOID"))
                        v_strSQL = Replace(v_strSQL, "?SICODE", "'" & v_tllog.Tables(0).Rows(v_inttmp)("SICODE") & "'")
                        v_strSQL = Replace(v_strSQL, "?TXNUM", "'" & v_tllog.Tables(0).Rows(v_inttmp)("TXNUM") & "'")
                        v_strSQL = Replace(v_strSQL, "?TXNAME", "'" & v_tllog.Tables(0).Rows(v_inttmp)("TXNAME") & "'")
                        v_strSQL = Replace(v_strSQL, "?TXDATE", "TO_DATE('" & CDate(v_tllog.Tables(0).Rows(v_inttmp)("TXDATE")).ToString("dd/MM/yyyy") & "','dd/mm/yyyy')")
                        v_strSQL = Replace(v_strSQL, "?TLTXCD", "'" & v_tllog.Tables(0).Rows(v_inttmp)("TLTXCD") & "'")
                        v_strSQL = Replace(v_strSQL, "?ISSUE_TYPE", "'" & v_tllog.Tables(0).Rows(v_inttmp)("COL_VALUE07") & "'")
                        v_strSQL = Replace(v_strSQL, "?TXNOTE", "'" & v_tllog.Tables(0).Rows(v_inttmp)("TXNOTE") & "'")

                        If v_trace_status = "1" And v_ds.Tables(0).Rows(v_int)("TRACE") = 1 Then
                            Trace.WriteLine("[BATCH - CA] " & DateTime.Now & " :" & vbCrLf & " -o- Câu lệnh thứ #" & v_ds.Tables(0).Rows(v_int)("ORDNUM") & "-o-" & vbCrLf & v_strSQL & vbCrLf)
                            If InStr(v_strSQL.ToUpper, "SELECT") > 0 Then
                                Trace.WriteLine("-o- Dữ liệu câu lệnh thứ #" & v_ds.Tables(0).Rows(v_int)("ORDNUM") & "-o-")
                                v_strSQLTmp = v_strSQL.Substring(InStr(v_strSQL.ToUpper, "SELECT") - 1)
                                v_trace = v_TranDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                                Trace.WriteLine(v_trace.GetXml & vbCrLf)
                            End If
                        End If

                        v_TranDataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
                    Next

                Next
            End If



            If v_trace_status = "1" Then
                tr2.Flush()
            End If
            Return v_lngErr
        Catch ex As Exception
            'ex.Source = "CA.BATCH"
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            '             & "Error code: System error!" & vbNewLine _
            '             & "Error message: " & ex.Message, EventLogEntryType.Error)
            Throw ex
        Finally
            If v_trace_status = "1" Then
                Trace.WriteLine("[Kết thúc: BATCH - CA] " & DateTime.Now & vbCrLf)
                tr2.Close()
                tr2.Dispose()
            End If
            GC.Collect()
        End Try
    End Function
    Public Function BatchRG(ByVal v_strBRID As String, ByVal v_strTellerID As String, ByVal v_strCURRDATE As String, ByRef v_TranDataAccess As DataAccess, ByVal v_str_BEGINOREND As String) As Long
        Dim v_lngErr As Long = ERR_SYSTEM_OK
        Dim v_ds As DataSet
        Dim v_strSQL, v_strSQLTmp As String
        Dim v_int, v_inttmp As Integer

        Dim tr2 As TextWriterTraceListener
        Dim v_trace_status, v_trace_path As String
        Dim v_trace, v_tllog As DataSet
        Try
            'BangPV:  Lay lai curvar tren server 
            'v_lngErr = v_TranDataAccess.GetSysVar("SYSTEM", "CURRDATE", v_strBRID, v_strCURRDATE)
            'End BangPV:  Lay lai curvar tren server 
            v_trace_status = "0"
            v_lngErr = v_TranDataAccess.GetSysVar("SYSTEM", "TRACE_STATUS", v_trace_status)
            'v_trace_path = "C:\log_batch_sql_data.txt"
            Dim v_strTempDate As String
            If v_trace_status = "1" Then
                v_TranDataAccess.GetSysVar("SYSTEM", "TRACE_PATH", v_trace_path)
                v_strTempDate = Replace(v_strCURRDATE, "/", "_")

                If v_trace_path = "" Then
                    Dim v_app As New ApplicationServices.ApplicationBase
                    v_trace_path = v_app.Info.DirectoryPath & "\Log\" & v_strTempDate
                Else
                    v_trace_path = v_trace_path & v_strTempDate
                End If

                If Not System.IO.Directory.Exists(v_trace_path) Then
                    System.IO.Directory.CreateDirectory(v_trace_path)
                End If

                v_trace_path &= "\log_batch_br" & v_strBRID & "_tl" & v_strTellerID & ".txt"

                tr2 = New TextWriterTraceListener(System.IO.File.AppendText(v_trace_path))
                Trace.Listeners.Add(tr2)
                Trace.WriteLine("[Bắt đầu: BATCH - CA] " & DateTime.Now & vbCrLf)
            End If
            Dim v_strBUSDATE As String

            If v_str_BEGINOREND = "BEGIN" Then
                v_strBUSDATE = v_strCURRDATE
                ' luu ky
                v_strSQL = "SELECT A.* FROM TLLOG A WHERE A.BRID='" & v_strBRID & "' AND A.AUTOID IN (SELECT A.AUTOID FROM SAISSUE A, (SELECT VARVALUE FROM SYSVAR WHERE varname='CURRDATE' AND BRID = '" & v_strBRID & "' AND DELETED=0 AND STATUS=0) B " _
                        & " WHERE GET_TRADE_DATE(A.DEDATE-1,'" & v_strBRID & "','-') = TO_DATE(B.varvalue,'dd/mm/yyyy') " _
                        & " AND A.DELETED=0 AND A.STATUS=0 and a.apptype='RG' )"
                v_tllog = v_TranDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                For v_inttmp = 0 To v_tllog.Tables(0).Rows.Count - 1
                    v_strSQL = "SELECT * FROM APPDML WHERE TLTXCD = '2090' AND TXSTATUS = 3 AND DELETED=0 AND STATUS=0 and ORDNUM in (4,5,25,26,27,28) ORDER BY TLTXCD, ORDNUM"
                    v_ds = v_TranDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

                    For v_int = 0 To v_ds.Tables(0).Rows.Count - 1
                        'bangpv : & " " & v_ds.Tables(0).Rows(v_int)("DMLSQL1")
                        v_strSQL = v_ds.Tables(0).Rows(v_int)("DMLSQL") & " " & v_ds.Tables(0).Rows(v_int)("DMLSQL1")
                        'end bangpv 
                        v_strSQL = Replace(v_strSQL, "?BRID", "'" & v_strBRID & "'")
                        v_strSQL = Replace(v_strSQL, "?BUSDATE", "to_date('" & v_strBUSDATE & "','dd/mm/yyyy')")
                        v_strSQL = Replace(v_strSQL, "?AUTOID", v_tllog.Tables(0).Rows(v_inttmp)("AUTOID"))
                        v_strSQL = Replace(v_strSQL, "?SICODE", "'" & v_tllog.Tables(0).Rows(v_inttmp)("SICODE") & "'")
                        v_strSQL = Replace(v_strSQL, "?TXNUM", "'" & v_tllog.Tables(0).Rows(v_inttmp)("TXNUM") & "'")
                        v_strSQL = Replace(v_strSQL, "?TXNAME", "'" & v_tllog.Tables(0).Rows(v_inttmp)("TXNAME") & "'")
                        v_strSQL = Replace(v_strSQL, "?TXDATE", "TO_DATE('" & CDate(v_tllog.Tables(0).Rows(v_inttmp)("TXDATE")).ToString("dd/MM/yyyy") & "','dd/mm/yyyy')")
                        v_strSQL = Replace(v_strSQL, "?TLTXCD", "'" & v_tllog.Tables(0).Rows(v_inttmp)("TLTXCD") & "'")
                        v_strSQL = Replace(v_strSQL, "?ISSUE_TYPE", "'" & v_tllog.Tables(0).Rows(v_inttmp)("COL_VALUE07") & "'")

                        If v_trace_status = "1" And v_ds.Tables(0).Rows(v_int)("TRACE") = 1 Then
                            Trace.WriteLine("[BATCH - CA] " & DateTime.Now & " :" & vbCrLf & " -o- Câu lệnh thứ #" & v_ds.Tables(0).Rows(v_int)("ORDNUM") & "-o-" & vbCrLf & v_strSQL & vbCrLf)
                            If InStr(v_strSQL.ToUpper, "SELECT") > 0 Then
                                Trace.WriteLine("-o- Dữ liệu câu lệnh thứ #" & v_ds.Tables(0).Rows(v_int)("ORDNUM") & "-o-")
                                v_strSQLTmp = v_strSQL.Substring(InStr(v_strSQL.ToUpper, "SELECT") - 1)
                                v_trace = v_TranDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                                Trace.WriteLine(v_trace.GetXml & vbCrLf)
                            End If
                        End If

                        v_TranDataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
                    Next

                Next
            End If
            If v_str_BEGINOREND = "END" Then
                v_strSQL = "select GET_TRADE_DATE(to_date('" & v_strCURRDATE & "','dd/mm/yyyy')+1,'" & v_strBRID & "','+') from dual"
                v_tllog = v_TranDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                If v_tllog.Tables(0).Rows.Count > 0 Then
                    v_strBUSDATE = CDate(v_tllog.Tables(0).Rows(0)(0)).ToString("dd/MM/yyyy")
                Else
                    v_strBUSDATE = v_strCURRDATE
                End If
                ' luu ky
                v_strSQL = "SELECT A.* FROM TLLOG A WHERE A.BRID='" & v_strBRID & "' AND A.AUTOID IN (SELECT A.AUTOID FROM SAISSUE A, (SELECT VARVALUE FROM SYSVAR WHERE varname='CURRDATE' AND BRID = '" & v_strBRID & "' AND DELETED=0 AND STATUS=0) B " _
                        & " WHERE GET_TRADE_DATE(A.DEDATE-1,'" & v_strBRID & "','-') = TO_DATE(B.varvalue,'dd/mm/yyyy') " _
                        & " AND A.DELETED=0 AND A.STATUS=0 and a.apptype='RG' )"
                v_tllog = v_TranDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                For v_inttmp = 0 To v_tllog.Tables(0).Rows.Count - 1
                    v_strSQL = "SELECT * FROM APPDML WHERE TLTXCD = '2090' AND TXSTATUS = 3 AND DELETED=0 AND STATUS=0 and not ORDNUM in (25,26,27,28) ORDER BY TLTXCD, ORDNUM"
                    v_ds = v_TranDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

                    For v_int = 0 To v_ds.Tables(0).Rows.Count - 1
                        'bangpv  & " " & v_ds.Tables(0).Rows(v_int)("DMLSQL1")
                        v_strSQL = v_ds.Tables(0).Rows(v_int)("DMLSQL") & " " & v_ds.Tables(0).Rows(v_int)("DMLSQL1")
                        'END bangpv 
                        v_strSQL = Replace(v_strSQL, "?BRID", "'" & v_strBRID & "'")
                        v_strSQL = Replace(v_strSQL, "?BUSDATE", "to_date('" & v_strBUSDATE & "','dd/mm/yyyy')")
                        v_strSQL = Replace(v_strSQL, "?AUTOID", v_tllog.Tables(0).Rows(v_inttmp)("AUTOID"))
                        v_strSQL = Replace(v_strSQL, "?SICODE", "'" & v_tllog.Tables(0).Rows(v_inttmp)("SICODE") & "'")
                        v_strSQL = Replace(v_strSQL, "?TXNUM", "'" & v_tllog.Tables(0).Rows(v_inttmp)("TXNUM") & "'")
                        v_strSQL = Replace(v_strSQL, "?TXNAME", "'" & v_tllog.Tables(0).Rows(v_inttmp)("TXNAME") & "'")
                        v_strSQL = Replace(v_strSQL, "?TXDATE", "TO_DATE('" & CDate(v_tllog.Tables(0).Rows(v_inttmp)("TXDATE")).ToString("dd/MM/yyyy") & "','dd/mm/yyyy')")
                        v_strSQL = Replace(v_strSQL, "?TLTXCD", "'" & v_tllog.Tables(0).Rows(v_inttmp)("TLTXCD") & "'")
                        v_strSQL = Replace(v_strSQL, "?ISSUE_TYPE", "'" & v_tllog.Tables(0).Rows(v_inttmp)("COL_VALUE07") & "'")

                        If v_trace_status = "1" And v_ds.Tables(0).Rows(v_int)("TRACE") = 1 Then
                            Trace.WriteLine("[BATCH - CA] " & DateTime.Now & " :" & vbCrLf & " -o- Câu lệnh thứ #" & v_ds.Tables(0).Rows(v_int)("ORDNUM") & "-o-" & vbCrLf & v_strSQL & vbCrLf)
                            If InStr(v_strSQL.ToUpper, "SELECT") > 0 Then
                                Trace.WriteLine("-o- Dữ liệu câu lệnh thứ #" & v_ds.Tables(0).Rows(v_int)("ORDNUM") & "-o-")
                                v_strSQLTmp = v_strSQL.Substring(InStr(v_strSQL.ToUpper, "SELECT") - 1)
                                v_trace = v_TranDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                                Trace.WriteLine(v_trace.GetXml & vbCrLf)
                            End If
                        End If

                        v_TranDataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
                    Next

                Next

                ' giao dich
                v_strSQL = "SELECT A.* FROM TLLOG A WHERE A.BRID='" & v_strBRID & "' AND A.AUTOID IN (SELECT A.AUTOID FROM SAISSUE A, (SELECT VARVALUE FROM SYSVAR WHERE varname='CURRDATE' AND BRID = '" & v_strBRID & "' AND DELETED=0 AND STATUS=0) B " _
                        & " WHERE GET_TRADE_DATE(A.TRANDATE-1,'" & v_strBRID & "','-') = TO_DATE(B.varvalue,'dd/mm/yyyy') " _
                        & " AND A.DELETED=0 AND A.STATUS=1 and a.apptype='RG' )"
                v_tllog = v_TranDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                For v_inttmp = 0 To v_tllog.Tables(0).Rows.Count - 1
                    v_strSQL = "SELECT * FROM APPDML WHERE TLTXCD = '2091' AND TXSTATUS = 3 AND DELETED=0 AND STATUS=0 ORDER BY TLTXCD, ORDNUM"
                    v_ds = v_TranDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

                    For v_int = 0 To v_ds.Tables(0).Rows.Count - 1
                        'bangpv:  & " " & v_ds.Tables(0).Rows(v_int)("DMLSQL1")
                        v_strSQL = v_ds.Tables(0).Rows(v_int)("DMLSQL") & " " & v_ds.Tables(0).Rows(v_int)("DMLSQL1")
                        'end bangpv
                        v_strSQL = Replace(v_strSQL, "?BRID", "'" & v_strBRID & "'")
                        v_strSQL = Replace(v_strSQL, "?BUSDATE", "to_date('" & v_strBUSDATE & "','dd/mm/yyyy')")
                        v_strSQL = Replace(v_strSQL, "?AUTOID", v_tllog.Tables(0).Rows(v_inttmp)("AUTOID"))
                        v_strSQL = Replace(v_strSQL, "?SICODE", "'" & v_tllog.Tables(0).Rows(v_inttmp)("SICODE") & "'")
                        v_strSQL = Replace(v_strSQL, "?TXNUM", "'" & v_tllog.Tables(0).Rows(v_inttmp)("TXNUM") & "'")
                        v_strSQL = Replace(v_strSQL, "?TXNAME", "'" & v_tllog.Tables(0).Rows(v_inttmp)("TXNAME") & "'")
                        v_strSQL = Replace(v_strSQL, "?TXDATE", "TO_DATE('" & CDate(v_tllog.Tables(0).Rows(v_inttmp)("TXDATE")).ToString("dd/MM/yyyy") & "','dd/mm/yyyy')")
                        v_strSQL = Replace(v_strSQL, "?TLTXCD", "'" & v_tllog.Tables(0).Rows(v_inttmp)("TLTXCD") & "'")
                        v_strSQL = Replace(v_strSQL, "?ISSUE_TYPE", "'" & v_tllog.Tables(0).Rows(v_inttmp)("COL_VALUE07") & "'")
                        v_strSQL = Replace(v_strSQL, "?TXNOTE", "'" & v_tllog.Tables(0).Rows(v_inttmp)("TXNOTE") & "'")

                        If v_trace_status = "1" And v_ds.Tables(0).Rows(v_int)("TRACE") = 1 Then
                            Trace.WriteLine("[BATCH - CA] " & DateTime.Now & " :" & vbCrLf & " -o- Câu lệnh thứ #" & v_ds.Tables(0).Rows(v_int)("ORDNUM") & "-o-" & vbCrLf & v_strSQL & vbCrLf)
                            If InStr(v_strSQL.ToUpper, "SELECT") > 0 Then
                                Trace.WriteLine("-o- Dữ liệu câu lệnh thứ #" & v_ds.Tables(0).Rows(v_int)("ORDNUM") & "-o-")
                                v_strSQLTmp = v_strSQL.Substring(InStr(v_strSQL.ToUpper, "SELECT") - 1)
                                v_trace = v_TranDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                                Trace.WriteLine(v_trace.GetXml & vbCrLf)
                            End If
                        End If

                        v_TranDataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
                    Next

                Next
            End If

            If v_trace_status = "1" Then
                tr2.Flush()
            End If
            Return v_lngErr
        Catch ex As Exception
            'ex.Source = "CA.BATCH"
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            '             & "Error code: System error!" & vbNewLine _
            '             & "Error message: " & ex.Message, EventLogEntryType.Error)
            Throw ex
        Finally
            If v_trace_status = "1" Then
                Trace.WriteLine("[Kết thúc: BATCH - CA] " & DateTime.Now & vbCrLf)
                tr2.Close()
                tr2.Dispose()
            End If
            GC.Collect()
        End Try
    End Function
    'bangpv 20141112
    Public Function BatchSD(ByVal v_strBRID As String, ByVal v_strTellerID As String, ByVal v_strCURRDATE As String, ByRef v_TranDataAccess As DataAccess, ByVal v_str_BEGINOREND As String) As Long
        Dim v_lngErr As Long = ERR_SYSTEM_OK
        Dim v_ds As DataSet
        Dim v_strSQL, v_strSQLTmp As String
        Dim v_int, v_inttmp As Integer

        Dim tr2 As TextWriterTraceListener
        Dim v_trace_status, v_trace_path As String
        Dim v_trace, v_tllog As DataSet
        Try
            'BangPV:  Lay lai curvar tren server 
            'v_lngErr = v_TranDataAccess.GetSysVar("SYSTEM", "CURRDATE", v_strBRID, v_strCURRDATE)
            'End BangPV:  Lay lai curvar tren server 
            v_trace_status = "0"
            v_lngErr = v_TranDataAccess.GetSysVar("SYSTEM", "TRACE_STATUS", v_trace_status)
            'v_trace_path = "C:\log_batch_sql_data.txt"
            Dim v_strTempDate As String
            If v_trace_status = "1" Then
                v_TranDataAccess.GetSysVar("SYSTEM", "TRACE_PATH", v_trace_path)
                v_strTempDate = Replace(v_strCURRDATE, "/", "_")

                If v_trace_path = "" Then
                    Dim v_app As New ApplicationServices.ApplicationBase
                    v_trace_path = v_app.Info.DirectoryPath & "\Log\" & v_strTempDate
                Else
                    v_trace_path = v_trace_path & v_strTempDate
                End If

                If Not System.IO.Directory.Exists(v_trace_path) Then
                    System.IO.Directory.CreateDirectory(v_trace_path)
                End If

                v_trace_path &= "\log_batch_br" & v_strBRID & "_tl" & v_strTellerID & ".txt"

                tr2 = New TextWriterTraceListener(System.IO.File.AppendText(v_trace_path))
                Trace.Listeners.Add(tr2)
                Trace.WriteLine("[Bắt đầu: BATCH - SD] " & DateTime.Now & vbCrLf)
            End If
            Dim v_strBUSDATE As String
            'If v_str_BEGINOREND = "BEGIN" Then
            '    v_strBUSDATE = v_strCURRDATE
            '    ' luu ky
            '    v_strSQL = "SELECT A.* FROM TLLOG A WHERE A.BRID='" & v_strBRID _
            '           & "' AND A.AUTOID IN (SELECT A.AUTOID FROM SAISSUE A, (SELECT VARVALUE FROM SYSVAR WHERE varname='CURRDATE' AND BRID = '" _
            '           & v_strBRID & "' AND DELETED=0 AND STATUS=0) B " _
            '           & " WHERE GET_TRADE_DATE(A.TRANDATE-1,'" & v_strBRID & "','-') = TO_DATE(B.varvalue,'dd/mm/yyyy') " _
            '           & " AND A.DELETED=0 and a.apptype='SD' )"
            '    v_tllog = v_TranDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            '    For v_inttmp = 0 To v_tllog.Tables(0).Rows.Count - 1
            '        v_strSQL = "SELECT * FROM APPDML WHERE TLTXCD = '1101' AND TXSTATUS = 5 AND DELETED=0 AND STATUS=0 and ordnum in (4,13,14,17,18,20,21) ORDER BY TLTXCD, ORDNUM"
            '        v_ds = v_TranDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

            '        For v_int = 0 To v_ds.Tables(0).Rows.Count - 1
            '            ' bangpv: them vao de ghep cau lenh sql dai qua : & " " & v_ds.Tables(0).Rows(v_int)("DMLSQL1")
            '            v_strSQL = v_ds.Tables(0).Rows(v_int)("DMLSQL") & " " & v_ds.Tables(0).Rows(v_int)("DMLSQL1")
            '            'end bangpv 
            '            v_strSQL = Replace(v_strSQL, "?BRID", "'" & v_strBRID & "'")
            '            v_strSQL = Replace(v_strSQL, "?BUSDATE", "to_date('" & v_strBUSDATE & "','dd/mm/yyyy')")
            '            v_strSQL = Replace(v_strSQL, "?AUTOID", v_tllog.Tables(0).Rows(v_inttmp)("AUTOID"))
            '            v_strSQL = Replace(v_strSQL, "?SICODE", "'" & v_tllog.Tables(0).Rows(v_inttmp)("SICODE") & "'")
            '            v_strSQL = Replace(v_strSQL, "?TXNUM", "'" & v_tllog.Tables(0).Rows(v_inttmp)("TXNUM") & "'")
            '            v_strSQL = Replace(v_strSQL, "?TXNAME", "'" & v_tllog.Tables(0).Rows(v_inttmp)("TXNAME") & "'")
            '            v_strSQL = Replace(v_strSQL, "?TXDATE", "TO_DATE('" & CDate(v_tllog.Tables(0).Rows(v_inttmp)("TXDATE")).ToString("dd/MM/yyyy") & "','dd/mm/yyyy')")
            '            v_strSQL = Replace(v_strSQL, "?TLTXCD", "'" & v_tllog.Tables(0).Rows(v_inttmp)("TLTXCD") & "'")
            '            v_strSQL = Replace(v_strSQL, "?ISSUE_TYPE", "'" & v_tllog.Tables(0).Rows(v_inttmp)("COL_VALUE07") & "'")
            '            v_strSQL = Replace(v_strSQL, "?TXNOTE", "'" & v_tllog.Tables(0).Rows(v_inttmp)("TXNOTE") & "'")

            '            If v_trace_status = "1" And v_ds.Tables(0).Rows(v_int)("TRACE") = 1 Then
            '                Trace.WriteLine("[BATCH - SD] " & DateTime.Now & " :" & vbCrLf & " -o- Câu lệnh thứ #" & v_ds.Tables(0).Rows(v_int)("ORDNUM") & "-o-" & vbCrLf & v_strSQL & vbCrLf)
            '                If InStr(v_strSQL.ToUpper, "SELECT") > 0 Then
            '                    Trace.WriteLine("-o- Dữ liệu câu lệnh thứ #" & v_ds.Tables(0).Rows(v_int)("ORDNUM") & "-o-")
            '                    v_strSQLTmp = v_strSQL.Substring(InStr(v_strSQL.ToUpper, "SELECT") - 1)
            '                    v_trace = v_TranDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
            '                    Trace.WriteLine(v_trace.GetXml & vbCrLf)
            '                End If
            '            End If

            '            v_TranDataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
            '        Next

            '    Next
            'End If

            If v_str_BEGINOREND = "END" Then
                ' giao dich
                v_strBUSDATE = v_strCURRDATE
                v_strSQL = "SELECT A.* FROM TLLOG A WHERE A.BRID='" & v_strBRID _
                        & "' AND A.AUTOID IN (SELECT A.AUTOID FROM SAISSUE A, (SELECT VARVALUE FROM SYSVAR WHERE varname='CURRDATE' AND BRID = '" _
                        & v_strBRID & "' AND DELETED=0 AND STATUS=0) B " _
                        & " WHERE GET_TRADE_DATE(A.DEDATE-1,'" & v_strBRID & "','-') = TO_DATE(B.varvalue,'dd/mm/yyyy') " _
                        & " AND A.DELETED=0 and a.apptype='SD' )"
                v_tllog = v_TranDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                For v_inttmp = 0 To v_tllog.Tables(0).Rows.Count - 1
                    v_strSQL = "SELECT * FROM APPDML WHERE TLTXCD = '1101' AND TXSTATUS = '5' AND DELETED=0 AND STATUS=0  /*and ordnum not in (13,14,17,18,20,21)*/ ORDER BY TLTXCD, ORDNUM"
                    v_ds = v_TranDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)

                    For v_int = 0 To v_ds.Tables(0).Rows.Count - 1
                        'bangpv:  & " " & v_ds.Tables(0).Rows(v_int)("DMLSQL1")
                        v_strSQL = v_ds.Tables(0).Rows(v_int)("DMLSQL") & " " & v_ds.Tables(0).Rows(v_int)("DMLSQL1")
                        'end bangpv
                        v_strSQL = Replace(v_strSQL, "?BRID", "'" & v_strBRID & "'")
                        'v_strSQL = Replace(v_strSQL, "?BUSDATE", "to_date('" & v_strBUSDATE & "','dd/mm/yyyy')")
                        v_strSQL = Replace(v_strSQL, "?BUSDATE", "to_date('" & v_tllog.Tables(0).Rows(v_inttmp)("COL_VALUE06") & "','dd/mm/yyyy')")
                        v_strSQL = Replace(v_strSQL, "?AUTOID", v_tllog.Tables(0).Rows(v_inttmp)("AUTOID"))
                        v_strSQL = Replace(v_strSQL, "?SICODE", "'" & v_tllog.Tables(0).Rows(v_inttmp)("SICODE") & "'")
                        v_strSQL = Replace(v_strSQL, "?TXNUM", "'" & v_tllog.Tables(0).Rows(v_inttmp)("TXNUM") & "'")
                        v_strSQL = Replace(v_strSQL, "?TXNAME", "'" & v_tllog.Tables(0).Rows(v_inttmp)("TXNAME") & "'")
                        v_strSQL = Replace(v_strSQL, "?TXDATE", "TO_DATE('" & CDate(v_tllog.Tables(0).Rows(v_inttmp)("TXDATE")).ToString("dd/MM/yyyy") & "','dd/mm/yyyy')")
                        v_strSQL = Replace(v_strSQL, "?TLTXCD", "'" & v_tllog.Tables(0).Rows(v_inttmp)("TLTXCD") & "'")
                        v_strSQL = Replace(v_strSQL, "?ISSUE_TYPE", "'" & v_tllog.Tables(0).Rows(v_inttmp)("COL_VALUE07") & "'")
                        v_strSQL = Replace(v_strSQL, "?TXNOTE", "'" & v_tllog.Tables(0).Rows(v_inttmp)("TXNOTE") & "'")

                        If v_trace_status = "1" And v_ds.Tables(0).Rows(v_int)("TRACE") = 1 Then
                            Trace.WriteLine("[BATCH - SD] " & DateTime.Now & " :" & vbCrLf & " -o- Câu lệnh thứ #" & v_ds.Tables(0).Rows(v_int)("ORDNUM") & "-o-" & vbCrLf & v_strSQL & vbCrLf)
                            If InStr(v_strSQL.ToUpper, "SELECT") > 0 Then
                                Trace.WriteLine("-o- Dữ liệu câu lệnh thứ #" & v_ds.Tables(0).Rows(v_int)("ORDNUM") & "-o-")
                                v_strSQLTmp = v_strSQL.Substring(InStr(v_strSQL.ToUpper, "SELECT") - 1)
                                v_trace = v_TranDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQLTmp)
                                Trace.WriteLine(v_trace.GetXml & vbCrLf)
                            End If
                        End If

                        v_TranDataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
                    Next

                Next
            End If

            If v_trace_status = "1" Then
                tr2.Flush()
            End If
            Return v_lngErr
        Catch ex As Exception
            ex.Source = "HOST.SD.BATCH"
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            '             & "Error code: System error!" & vbNewLine _
            '             & "Error message: " & ex.Message, EventLogEntryType.Error)
            LogError.Write(ex.Message, EventLogEntryType.Error, ex.Source)
            Throw ex
        Finally
            If v_trace_status = "1" Then
                Trace.WriteLine("[Kết thúc: BATCH - SD] " & DateTime.Now & vbCrLf)
                tr2.Close()
                tr2.Dispose()
            End If
            GC.Collect()
        End Try
    End Function
    'end bangpv 20141112 
    'hanm5 20161015
    Public Function BatchCS(ByVal v_strBRID As String, ByVal v_strTellerID As String, ByVal v_strCURRDATE As String, ByRef v_TranDataAccess As DataAccess, ByVal v_str_BEGINOREND As String) As Long
        Dim v_lngErr As Long = ERR_SYSTEM_OK
        Dim v_strSQL As String
        Try
            v_strSQL = "BEGIN PROC_CS_BATCH(?TXDATE, ?BRID, ?TLID, ?BEGINOREND); END;"
            v_strSQL = Replace(v_strSQL, "?BRID", "'" & v_strBRID & "'")
            v_strSQL = Replace(v_strSQL, "?TXDATE", "'" & v_strCURRDATE & "'")
            v_strSQL = Replace(v_strSQL, "?TLID", "'" & v_strTellerID & "'")
            v_strSQL = Replace(v_strSQL, "?BEGINOREND", "'" & v_str_BEGINOREND & "'")

            v_TranDataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
            Return v_lngErr
        Catch ex As Exception
            ex.Source = "HOST.CS.BATCH"
            LogError.Write(ex.Message, EventLogEntryType.Error, ex.Source)
            Throw ex
        Finally
            GC.Collect()
        End Try
    End Function
    'end hanm5 20161015
    Private Function BatchTranInsert(ByVal v_strBRID As String, ByVal v_strCURRDATE As String, _
                               ByVal v_strPassDate As String, ByVal v_strTran As String, _
                               ByRef v_TranDataAccess As DataAccess) As Long
        Dim v_lngErr As Long = ERR_SYSTEM_OK
        Dim v_ds As DataSet
        Try
            Dim v_strMinDate As String
            Dim v_strSQL As String
            ' Can sua them doan nay de tranh day vao qua khu thua 20170706
            v_strSQL = "insert into " & v_strTran & "trana_all select * from " & v_strTran & "tran where brid = '" & v_strBRID & "'" _
                        & " and txdate <= to_date('" & v_strCURRDATE & " 23:59:00', 'DD/MM/YYYY hh24:mi:ss')"

            v_TranDataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
            'If v_strTran = "RG" Then
            '    If v_strDBLink <> "" Then
            '        v_strSQL = "insert into rgii select * from rgii" & v_strDBLink
            '        v_TranDataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)

            '        v_strSQL = "insert into rgiiia select * from rgiiia" & v_strDBLink
            '        v_TranDataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)

            '        v_strSQL = "insert into rgiirep select * from rgiirep" & v_strDBLink
            '        v_TranDataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)

            '        v_strSQL = "insert into rgiiinfo select * from rgiiinfo" & v_strDBLink
            '        v_TranDataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
            '    End If
            'ElseIf v_strTran = "SF" Then
            '    If v_strDBLink <> "" Then
            '        v_strSQL = "insert into " & v_strTran & "mast select * from " & v_strTran & "mast" & v_strDBLink
            '        v_TranDataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
            '    End If
            'Else
            '    If v_strTran <> "CS" Then
            '        If v_strDBLink <> "" Then
            '            v_strSQL = "insert into " & v_strTran & "mast select * from " & v_strTran & "mast" & v_strDBLink & " where brid = '" & v_strBRID & "'"
            '            v_TranDataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
            '        End If
            '    End If
            'End If

            'v_strSQL = "insert into " & v_strTran & "trana select * from " & v_strTran & "tran where brid = '" & v_strBRID & "'"
            'v_TranDataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
            ''Lay ngay min
            'v_strSQL = "SELECT TO_CHAR(MIN(TXDATE),'dd/mm/yyyy') FROM " & v_strTran & "TRANA"
            'v_ds = v_TranDataAccess.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            'If v_ds.Tables(0).Rows(0)(0).ToString.Trim <> "" Then
            '    v_strMinDate = v_ds.Tables(0).Rows(0)(0)
            'Else
            '    v_strMinDate = v_strCURRDATE
            'End If

            ''Neu mindate < passdate xoa dl thua trong bang all
            'If CheckPassDate(v_strMinDate, v_strPassDate) Then
            '    v_strSQL = "DELETE FROM " & v_strTran & "TRANA WHERE TXDATE<TO_DATE('" & v_strPassDate & "','dd/mm/yyyy')"
            '    v_TranDataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
            'End If
            ''Neu mindate > passdate them dl tu bang all_all vao bang all
            'If Not CheckPassDate(v_strMinDate, v_strPassDate) Then
            '    v_strSQL = "INSERT INTO " & v_strTran & "TRANA SELECT * FROM " & v_strTran & "TRANA_ALL" _
            '                & " WHERE TXDATE>=TO_DATE('" & v_strPassDate & "','dd/mm/yyyy')" _
            '                & " AND TXDATE<TO_DATE('" & v_strMinDate & "','dd/mm/yyyy')" _
            '                & " AND BRID='" & v_strBRID & "'"
            '    v_TranDataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
            'End If

            'v_strSQL = "delete from " & v_strTran & "tran where brid = '" & v_strBRID & "'"
            'v_TranDataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
            'v_ds.Dispose()
            Return v_lngErr
        Catch ex As Exception
            Throw ex
        Finally
            If Not v_ds Is Nothing Then
                v_ds.Dispose()
            End If
        End Try
    End Function
    Private Function BatchTranDelete(ByVal v_strBRID As String, ByVal v_strCURRDATE As String, _
                              ByVal v_strPassDate As String, ByVal v_strTran As String, _
                              ByRef v_TranDataAccess As DataAccess) As Long
        Dim v_lngErr As Long = ERR_SYSTEM_OK
        Try
            Dim v_strSQL As String
            v_strSQL = "delete from " & v_strTran & "tran where brid = '" & v_strBRID & "'" _
                        & " and txdate <= to_date('" & v_strCURRDATE & " 23:59:00', 'DD/MM/YYYY hh24:mi:ss')"
            v_TranDataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
            Return v_lngErr
        Catch ex As Exception
            Throw ex
        Finally
            GC.Collect()
        End Try
    End Function

    Private Function BatchTranTruncate(ByVal v_strBRID As String, _
                              ByRef v_TranDataAccess As DataAccess, ByVal v_strDBLink As String) As Long
        Dim v_lngErr As Long = ERR_SYSTEM_OK
        Dim v_arrTrans() As String = {"CA", "IA", "MA", "MF", "SF", "RA", "RG", "TLLOG"}
        Dim v_strSQL As String
        Try
            If v_strDBLink <> "" Then
                For i As Integer = 0 To v_arrTrans.Length - 1
                    Select Case v_arrTrans(i)
                        Case "RG"
                            v_strSQL = "delete from RGII"
                            v_TranDataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
                            v_strSQL = "delete from RGIIIA"
                            v_TranDataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
                            v_strSQL = "delete from RGIIREP"
                            v_TranDataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
                            v_strSQL = "delete from RGIIINFO"
                            v_TranDataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
                        Case "TLLOG"
                            v_strSQL = "delete from TLLOG where brid = '" & v_strBRID & "'"
                            v_TranDataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
                        Case "SF"
                            v_strSQL = "delete from SFMAST"
                            v_TranDataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
                        Case Else
                            v_strSQL = "delete from " & v_arrTrans(i) & "MAST where brid = '" & v_strBRID & "'"
                            v_TranDataAccess.ExecuteNonQuery(CommandType.Text, v_strSQL)
                    End Select
                Next
            End If
            Return v_lngErr
        Catch ex As Exception
            Throw ex
            Return ERR_SYSTEM_START
        Finally
            GC.Collect()
        End Try
    End Function

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
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
