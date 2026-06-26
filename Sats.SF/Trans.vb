Imports SATS.CommonLibrary
Imports SATS.CoreBusiness
Imports SATS.DataAccessLayer
Public Class Trans
    Inherits CoreBusiness.txMaster
    Public Sub New()
        ATTR_MODULE = "SF"
    End Sub
#Region " Implement functions"
    Overrides Function txImpUpdate(ByRef pv_xmlDocument As Xml.XmlDocument) As Long
        'Xác định mã giao dịch tương ứng
        Dim v_lngErrorCode As Long
        Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes
        Dim v_strTLTXCD As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTLTXCD), Xml.XmlAttribute).Value)
        Select Case v_strTLTXCD
            Case gc_DE_DEPOSIT_SECURITIES
                v_lngErrorCode = DepositSecurities(pv_xmlDocument)
        End Select
        'Trả v�? mã lỗi
        Return v_lngErrorCode
    End Function

    Overrides Function txImpCheck(ByRef pv_xmlDocument As Xml.XmlDocument) As Long
        Dim v_lngErrorCode As Long
        Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes
        Dim v_strTLTXCD As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTLTXCD), Xml.XmlAttribute).Value)
        Select Case v_strTLTXCD
            Case gc_DE_DEPOSIT_SECURITIES
                'v_lngErrorCode = CheckTLGRPUSERS(pv_xmlDocument)
        End Select
        Return v_lngErrorCode

    End Function

    Overrides Function txImpMisc(ByRef pv_xmlDocument As Xml.XmlDocument) As Long
        'Xác định mã giao dịch tương ứng
        Dim v_lngErrorCode As Long
        Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlDocument.DocumentElement.Attributes
        Dim v_strTLTXCD As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTLTXCD), Xml.XmlAttribute).Value)
        Select Case v_strTLTXCD

        End Select
        'Trả v�? mã lỗi
        Return v_lngErrorCode
    End Function
#End Region
#Region " Private functions"
    Private Function DepositSecurities(ByRef pv_xmlDocument As Xml.XmlDocument) As Long
        Dim v_lngErrCode As Long = ERR_SYSTEM_OK
        Dim v_strErrorSource As String = "SE.Trans.DepositSecurities", v_strErrorMessage As String = ""
        Dim v_obj As New DataAccess, v_ds As DataSet
        Dim v_strSQL As String = String.Empty, i As Integer
        Try
            Dim v_nodeList As Xml.XmlNodeList
            Dim v_strSQLTmp As String = String.Empty
            Dim v_strFLDCD As String, v_strFLDTYPE As String, v_strVALUE As String, v_dblVALUE As Double
            Dim v_strSEACCTNO As String = "", v_strDESC As String
            Dim v_dblDEPOSITQTTY, v_dblDEPOSITPRICE As Double
            Dim v_strTXDATE As String = pv_xmlDocument.DocumentElement.Attributes(gc_AtributeTXDATE).Value
            Dim v_strBUSDATE As String = pv_xmlDocument.DocumentElement.Attributes(gc_AtributeBUSDATE).Value ' QuyetLT -> Chan Posting date
            Dim v_strTXNUM As String = pv_xmlDocument.DocumentElement.Attributes(gc_AtributeTXNUM).Value
            Dim v_strTXDESC As String = pv_xmlDocument.DocumentElement.Attributes(gc_AtributeTXDESC).Value
            Dim v_strOVRRQD As String = pv_xmlDocument.DocumentElement.Attributes(gc_AtributeOVRRQS).Value
            Dim v_strCHKID As String = pv_xmlDocument.DocumentElement.Attributes(gc_AtributeCHKID).Value
            Dim v_strOFFID As String = pv_xmlDocument.DocumentElement.Attributes(gc_AtributeOFFID).Value
            Dim v_strDELTD As String = pv_xmlDocument.DocumentElement.Attributes(gc_AtributeDELTD).Value
            Dim v_blnReversal As Boolean = IIf(v_strDELTD = "Y", True, False), v_blnApproval As Boolean = False
            v_obj.NewDBInstance(gc_MODULE_HOST)
            '�?�?c nội dung giao dịch
            v_nodeList = pv_xmlDocument.SelectNodes("/TransactMessage/fields/entry")
            For i = 0 To v_nodeList.Count - 1
                With v_nodeList.Item(i)
                    v_strFLDCD = Trim(.Attributes(gc_AtributeFLDNAME).Value.ToString)
                    v_strFLDTYPE = Trim(.Attributes(gc_AtributeFLDTYPE).Value.ToString)
                    If v_strFLDTYPE = "N" Then
                        v_strVALUE = vbNullString
                        v_dblVALUE = IIf(IsNumeric(.InnerText), CDbl(.InnerText), 0)
                    Else
                        v_strVALUE = Trim(.InnerText)
                        v_dblVALUE = 0
                    End If

                    Select Case v_strFLDCD
                        Case "03" 'SEACCTNO
                            v_strSEACCTNO = v_strVALUE
                        Case "10" 'DEPOSITQTTY
                            v_dblDEPOSITQTTY = v_dblVALUE
                        Case "09" 'PRICE
                            v_dblDEPOSITPRICE = v_dblVALUE
                        Case "30" 'DESC
                            v_strDESC = v_strVALUE
                    End Select
                End With
            Next

            If Not v_blnReversal Then
                'ghi nhan luu ky
                'v_strSQL = "INSERT INTO SEDEPOSIT (AUTOID,ACCTNO,TXNUM,TXDATE,DEPOSITPRICE,DEPOSITQTTY,STATUS,DELTD,DESCRIPTION) VALUES (SEQ_SEDEPOSIT.NEXTVAL,'" & v_strSEACCTNO & "','" & v_strTXNUM & "',TO_DATE('" & v_strTXDATE & "','DD/MM/YYYY')," & v_dblDEPOSITPRICE & "," & v_dblDEPOSITQTTY & ",'D','N','" & v_strTXDESC & "')"

                ' QuyetLT -> Chan Posting date
                v_strSQL = "INSERT INTO SEDEPOSIT (AUTOID,ACCTNO,TXNUM,TXDATE,DEPOSITPRICE,DEPOSITQTTY,STATUS,DELTD,DESCRIPTION,BUSDATE) VALUES (SEQ_SEDEPOSIT.NEXTVAL,'" & v_strSEACCTNO & "','" & v_strTXNUM & "',TO_DATE('" & v_strTXDATE & "','DD/MM/YYYY')," & v_dblDEPOSITPRICE & "," & v_dblDEPOSITQTTY & ",'D','N','" & v_strTXDESC & "',TO_DATE('" & v_strBUSDATE & "','DD/MM/YYYY'))"
                v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
            Else
                v_strSQL = " SELECT * FROM SEMAST WHERE ACCTNO='" & v_strSEACCTNO & "' AND DEPOSIT >=" & v_dblDEPOSITQTTY & ""
                v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                If Not v_ds.Tables(0).Rows.Count > 0 Then

                    'v_lngErrCode = ERR_SE_CANNOT_DELETE_DEPOSITED
                    'LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
                    '             & "Error code: " & v_lngErrCode.ToString() & vbNewLine _
                    '             & "Error message: " & v_strErrorMessage, EventLogEntryType.Information)
                    Return v_lngErrCode
                End If
                'Xoa luu ky
                v_strSQL = "UPDATE SEDEPOSIT SET DELTD='Y' WHERE TXNUM='" & v_strTXNUM & "' AND TXDATE=TO_DATE('" & v_strTXDATE & "','DD/MM/YYYY')"
                v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
            End If

            Return v_lngErrCode
        Catch ex As Exception
            'LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
            '             & "Error code: System error!" & vbNewLine _
            '             & "Error message: " & v_strSQL & ControlChars.CrLf & ex.Message, EventLogEntryType.Error)
            Throw ex
        End Try
    End Function
#End Region
End Class
