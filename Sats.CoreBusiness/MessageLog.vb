Imports Sats.CommonLibrary
Imports Sats.DataAccessLayer
'Imports System.EnterpriseServices

'<JustInTimeActivation(False), _
'Transaction(TransactionOption.Disabled), _
'ObjectPooling(Enabled:=True, MinPoolSize:=30)> _
Public Class MessageLog
    Implements IDisposable
    'Inherits ServicedComponent

#Region " Khai báo hằng, biến "
    Private mv_strAttrRegion As String = gc_MODULE_HOST 'gc_MODULE_BDS
#End Region

#Region " Constructor "
    Public Sub New()
    End Sub
#End Region

#Region " Thuộc tính của lớp "

    Public Sub NewDBInstance(ByVal pv_strModule As String)
        mv_strAttrRegion = pv_strModule
    End Sub

    Public Property ATTR_REGION() As String
        Get
            Return mv_strAttrRegion
        End Get
        Set(ByVal Value As String)
            mv_strAttrRegion = Value
        End Set
    End Property
#End Region

#Region " Public methods "
    Public Function TransDelete(ByRef pv_xmlMessage As Xml.XmlDocument) As Long
        Dim v_lngErrorCode As Long = ERR_SYSTEM_OK
        Dim v_strErrorSource As String = "MessageLog.TransDelete", v_strErrorMessage As String = String.Empty
        Try

            Dim v_obj = New DataAccess
            v_obj.NewDBInstance(ATTR_REGION)



            'Update TLLOG: by set DELTD field
            Dim v_strTXNUM As String = pv_xmlMessage.DocumentElement.Attributes(gc_AtributeTXNUM).Value.ToString
            Dim v_strTXDATE As String = pv_xmlMessage.DocumentElement.Attributes(gc_AtributeTXDATE).Value.ToString
            Dim v_strTXSTATUS As String = pv_xmlMessage.DocumentElement.Attributes(gc_AtributeSTATUS).Value.ToString
            Dim v_strOFFID As String = pv_xmlMessage.DocumentElement.Attributes(gc_AtributeOFFID).Value.ToString
            Dim v_strDELTD As String = pv_xmlMessage.DocumentElement.Attributes(gc_AtributeDELTD).Value.ToString
            Dim v_blnReversal As Boolean = IIf(v_strDELTD = "Y", True, False)

            Dim v_strSQL As String = "UPDATE TLLOG SET DELTD='" & v_strDELTD & "',TXSTATUS='" & v_strTXSTATUS & "',OFFID='" & v_strOFFID & "' " _
                & "WHERE TXDATE = TO_DATE('" & v_strTXDATE & "', '" & gc_FORMAT_DATE & "') AND TXNUM = '" & v_strTXNUM & "'"
            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)

            If v_blnReversal Then
                'Xoa cac giao dich thu phi da nhap tuong ung voi giao dich
                v_strSQL = "UPDATE FEETRAN SET DELTD='Y' WHERE TXNUM='" & v_strTXNUM & "' AND TXDATE=TO_DATE('" & v_strTXDATE & "', '" & gc_FORMAT_DATE & "')"
                v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                'Xoa cac hoa don VAT da nhap tuong ung voi giao dich
                v_strSQL = "UPDATE VATTRAN SET DELTD='Y' WHERE TXNUM='" & v_strTXNUM & "' AND TXDATE=TO_DATE('" & v_strTXDATE & "', '" & gc_FORMAT_DATE & "')"
                v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                'Xoa cac tai san co dinh tuong ung voi giao dich
                v_strSQL = "UPDATE MITRAN SET DELTD='Y' WHERE TXNUM='" & v_strTXNUM & "' AND TXDATE=TO_DATE('" & v_strTXDATE & "', '" & gc_FORMAT_DATE & "')"
                v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
            End If
            If Not v_obj Is Nothing Then v_obj.Dispose()
            Return v_lngErrorCode
        Catch ex As Exception
            'LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
            '             & "Error code: " & v_lngErrorCode.ToString() & vbNewLine _
            '             & "Error message: " & v_strErrorMessage, EventLogEntryType.Information)
            Throw ex
        End Try
    End Function

    Public Function TransDetail(ByRef pv_xmlMessage As Xml.XmlDocument) As Long
        Dim v_lngErrCode As Long = ERR_SYSTEM_OK
        Dim v_strErrorSource As String = "MessageLog.TransDetail", v_strErrorMessage As String = String.Empty
        Dim v_obj As DataAccess
        Dim v_strTxMsg As String, v_strFLDNAME, v_strDATATYPE, v_strFLDVALUE As String
        Dim EntrySUBTXNO, EntryDORC, EntryCCYCD, EntryACCTNO, EntryAMOUNT As String
        Dim v_strVOUCHERNO, v_strVOUCHERTYPE, v_strSERIENO, v_strVOUCHERDATE, v_strCUSTID, v_strTAXCODE, v_strCUSTNAME, v_strADDRESS, v_strCONTENTS, v_strDESCRIPTION As String, v_dblQTTY, v_dblPRICE, v_dblVATRATE As Double
        Dim v_dblMISUBTXNO As Double, v_strMIDORC, v_strMICODE, v_strMICUSTID, v_strMICUSTNAME, v_strMITASKCD, v_strMIDEPTCD, v_strMIMICD, v_strMIDESCRIPTION As String
        Dim v_dataElement As Xml.XmlElement, v_entryNode As Xml.XmlNode
        Dim v_attrFLDNAME, v_attrDATATYPE As Xml.XmlAttribute
        Dim v_strSQL As String = "", v_ds As DataSet, i As Integer
        Dim v_dblAMT, v_dblVATAMT As Double

        Try

            v_obj = New DataAccess
            v_obj.NewDBInstance(ATTR_REGION)

            'Tạo message
            Dim v_attrColl As Xml.XmlAttributeCollection = pv_xmlMessage.DocumentElement.Attributes
            Dim v_strTXNUM As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTXNUM), Xml.XmlAttribute).Value)
            Dim v_strTXDATE As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeTXDATE), Xml.XmlAttribute).Value)

            v_strTxMsg = BuildXMLTxMsg(gc_MsgTypeTrans, gc_IsLocalMsg)
            pv_xmlMessage.LoadXml(v_strTxMsg)
            v_strSQL = "SELECT LG.*, TX.TXDESC TLTXDESC, TX.LOCAL, TX.TXTYPE, TX.NOSUBMIT, TX.DELALLOW FROM TLLOG LG, TLTX TX " & ControlChars.CrLf _
                & "WHERE TX.TLTXCD=LG.TLTXCD AND LG.TXNUM='" & v_strTXNUM & "' AND LG.TXDATE=TO_DATE('" & v_strTXDATE & "', '" & gc_FORMAT_DATE & "') "
            v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
            If v_ds.Tables(0).Rows.Count > 0 Then
                v_strErrorMessage = "Lấy phần header"
                'Lấy phần header
                pv_xmlMessage.DocumentElement.Attributes(gc_AtributeTXNUM).Value = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(0)("TXNUM")))
                pv_xmlMessage.DocumentElement.Attributes(gc_AtributeTXDATE).Value = Format(gf_CorrectDateField(v_ds.Tables(0).Rows(0)("TXDATE")), gc_FORMAT_DATE)
                pv_xmlMessage.DocumentElement.Attributes(gc_AtributeTXTIME).Value = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(0)("TXTIME")))
                pv_xmlMessage.DocumentElement.Attributes(gc_AtributeBRID).Value = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(0)("BRID")))
                pv_xmlMessage.DocumentElement.Attributes(gc_AtributeTLID).Value = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(0)("TLID")))
                pv_xmlMessage.DocumentElement.Attributes(gc_AtributeOFFID).Value = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(0)("OFFID")))
                pv_xmlMessage.DocumentElement.Attributes(gc_AtributeOVRRQS).Value = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(0)("OVRRQS")))
                pv_xmlMessage.DocumentElement.Attributes(gc_AtributeCHID).Value = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(0)("CHID")))
                pv_xmlMessage.DocumentElement.Attributes(gc_AtributeCHKID).Value = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(0)("CHKID")))
                pv_xmlMessage.DocumentElement.Attributes(gc_AtributeTLTXCD).Value = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(0)("TLTXCD")))
                pv_xmlMessage.DocumentElement.Attributes(gc_AtributeIBT).Value = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(0)("IBT")))
                pv_xmlMessage.DocumentElement.Attributes(gc_AtributeBRID2).Value = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(0)("BRID2")))
                pv_xmlMessage.DocumentElement.Attributes(gc_AtributeTLID2).Value = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(0)("TLID2")))
                pv_xmlMessage.DocumentElement.Attributes(gc_AtributeCCYUSAGE).Value = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(0)("CCYUSAGE")))
                pv_xmlMessage.DocumentElement.Attributes(gc_AtributeSTATUS).Value = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(0)("TXSTATUS")))
                pv_xmlMessage.DocumentElement.Attributes(gc_AtributeOFFLINE).Value = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(0)("OFF_LINE")))
                pv_xmlMessage.DocumentElement.Attributes(gc_AtributeDELTD).Value = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(0)("DELTD")))
                pv_xmlMessage.DocumentElement.Attributes(gc_AtributeBRDATE).Value = Format(gf_CorrectDateField(v_ds.Tables(0).Rows(0)("BRDATE")), gc_FORMAT_DATE)
                pv_xmlMessage.DocumentElement.Attributes(gc_AtributeBUSDATE).Value = Format(gf_CorrectDateField(v_ds.Tables(0).Rows(0)("BUSDATE")), gc_FORMAT_DATE)
                pv_xmlMessage.DocumentElement.Attributes(gc_AtributeMSGSTS).Value = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(0)("MSGSTS")))
                pv_xmlMessage.DocumentElement.Attributes(gc_AtributeOVRSTS).Value = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(0)("OVRSTS")))
                pv_xmlMessage.DocumentElement.Attributes(gc_AtributeIPADDRESS).Value = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(0)("IPADDRESS")))
                pv_xmlMessage.DocumentElement.Attributes(gc_AtributeWSNAME).Value = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(0)("WSNAME")))
                pv_xmlMessage.DocumentElement.Attributes(gc_AtributeBATCHNAME).Value = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(0)("BATCHNAME")))
                pv_xmlMessage.DocumentElement.Attributes(gc_AtributeTXDESC).Value = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(0)("TXDESC")))
                pv_xmlMessage.DocumentElement.Attributes(gc_AtributeLOCAL).Value = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(0)("LOCAL")))
                pv_xmlMessage.DocumentElement.Attributes(gc_AtributeTXTYPE).Value = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(0)("TXTYPE")))
                pv_xmlMessage.DocumentElement.Attributes(gc_AtributeNOSUBMIT).Value = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(0)("NOSUBMIT")))
                pv_xmlMessage.DocumentElement.Attributes(gc_AtributeMSGAMT).Value = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(0)("MSGAMT")))
                pv_xmlMessage.DocumentElement.Attributes(gc_AtributeMSGACCT).Value = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(0)("MSGACCT")))

                pv_xmlMessage.DocumentElement.Attributes(gc_AtributeCHKTIME).Value = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(0)("CHKTIME")))
                pv_xmlMessage.DocumentElement.Attributes(gc_AtributeOFFTIME).Value = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(0)("OFFTIME")))
                pv_xmlMessage.DocumentElement.Attributes(gc_AtributeDELALLOW).Value = Trim(gf_CorrectStringField(v_ds.Tables(0).Rows(0)("DELALLOW")))

                'Lấy phần nội dung giao dịch
                v_strSQL = "SELECT FLD.*,LGFLD.NVALUE, LGFLD.CVALUE " & ControlChars.CrLf _
                        & "FROM FLDMASTER FLD, TLLOGFLD LGFLD, TLLOG LG " & ControlChars.CrLf _
                        & "WHERE FLD.OBJNAME = LG.TLTXCD And LG.TXNUM = LGFLD.TXNUM And LG.TXDATE = LGFLD.TXDATE AND FLD.FLDNAME=LGFLD.FLDCD " & ControlChars.CrLf _
                        & "AND LG.TXNUM='" & v_strTXNUM & "' AND LG.TXDATE=TO_DATE('" & v_strTXDATE & "', '" & gc_FORMAT_DATE & "') " & ControlChars.CrLf _
                        & "ORDER BY ODRNUM"
                v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                v_strErrorMessage = "Lấy phần nội dung giao dịch"
                If v_ds.Tables(0).Rows.Count > 0 Then
                    v_dataElement = pv_xmlMessage.CreateElement(Xml.XmlNodeType.Element, "fields", "")
                    For i = 0 To v_ds.Tables(0).Rows.Count - 1 Step 1
                        'Xác định các trư�?ng dữ liệu
                        v_strFLDNAME = gf_CorrectStringField(v_ds.Tables(0).Rows(i)("FLDNAME"))
                        v_strDATATYPE = gf_CorrectStringField(v_ds.Tables(0).Rows(i)("DATATYPE"))
                        If v_strDATATYPE = "N" Then
                            v_strFLDVALUE = gf_CorrectStringField(v_ds.Tables(0).Rows(i)("NVALUE"))
                        Else
                            v_strFLDVALUE = gf_CorrectStringField(v_ds.Tables(0).Rows(i)("CVALUE"))
                        End If

                        'Tạo các trư�?ng dữ liệu
                        v_entryNode = pv_xmlMessage.CreateNode(Xml.XmlNodeType.Element, "entry", "")
                        v_attrFLDNAME = pv_xmlMessage.CreateAttribute(gc_AtributeFLDNAME)
                        v_attrFLDNAME.Value = v_strFLDNAME
                        v_entryNode.Attributes.Append(v_attrFLDNAME)
                        v_attrDATATYPE = pv_xmlMessage.CreateAttribute(gc_AtributeFLDTYPE)
                        v_attrDATATYPE.Value = v_strDATATYPE
                        v_entryNode.Attributes.Append(v_attrDATATYPE)
                        v_entryNode.InnerText = v_strFLDVALUE

                        v_dataElement.AppendChild(v_entryNode)
                    Next
                    pv_xmlMessage.DocumentElement.AppendChild(v_dataElement)
                End If

                'Lấy phần hạch toán
                v_strErrorMessage = "Lấy phần hạch toán"
                v_strSQL = "SELECT GL.* FROM TLLOG LG, GLTRAN GL " & ControlChars.CrLf _
                        & "WHERE LG.TXNUM=GL.TXNUM AND LG.TXDATE=GL.TXDATE " & ControlChars.CrLf _
                        & "AND LG.TXNUM='" & v_strTXNUM & "' AND LG.TXDATE=TO_DATE('" & v_strTXDATE & "', '" & gc_FORMAT_DATE & "') " & ControlChars.CrLf _
                        & "ORDER BY GL.SUBTXNO, GL.DORC DESC"
                v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                If v_ds.Tables(0).Rows.Count > 0 Then
                    v_dataElement = pv_xmlMessage.CreateElement(Xml.XmlNodeType.Element, "postmap", "")
                    For i = 0 To v_ds.Tables(0).Rows.Count - 1 Step 1
                        'Xác định các bút toán
                        EntrySUBTXNO = gf_CorrectStringField(v_ds.Tables(0).Rows(i)("SUBTXNO"))
                        EntryDORC = gf_CorrectStringField(v_ds.Tables(0).Rows(i)("DORC"))
                        EntryCCYCD = gf_CorrectStringField(v_ds.Tables(0).Rows(i)("CCYCD"))
                        EntryACCTNO = gf_CorrectStringField(v_ds.Tables(0).Rows(i)("ACCTNO"))
                        EntryAMOUNT = gf_CorrectStringField(v_ds.Tables(0).Rows(i)("AMT"))
                        'Thêm bút toán
                        v_entryNode = pv_xmlMessage.CreateNode(Xml.XmlNodeType.Element, "entry", "")
                        Dim v_attrSUBTXNO As Xml.XmlAttribute = pv_xmlMessage.CreateAttribute("subtxno")
                        v_attrSUBTXNO.Value = EntrySUBTXNO
                        v_entryNode.Attributes.Append(v_attrSUBTXNO)
                        Dim v_attrDORC As Xml.XmlAttribute = pv_xmlMessage.CreateAttribute("dorc")
                        v_attrDORC.Value = EntryDORC
                        v_entryNode.Attributes.Append(v_attrDORC)
                        Dim v_attrCCYCD As Xml.XmlAttribute = pv_xmlMessage.CreateAttribute("ccycd")
                        v_attrCCYCD.Value = EntryCCYCD
                        v_entryNode.Attributes.Append(v_attrCCYCD)
                        Dim v_attrACCTNO As Xml.XmlAttribute = pv_xmlMessage.CreateAttribute("acctno")
                        v_attrACCTNO.Value = EntryACCTNO
                        v_entryNode.Attributes.Append(v_attrACCTNO)
                        v_entryNode.InnerText = EntryAMOUNT
                        v_dataElement.AppendChild(v_entryNode)
                    Next
                    pv_xmlMessage.DocumentElement.AppendChild(v_dataElement)
                End If
                'Lấy phần hoá đơn VAT
                v_strErrorMessage = "Lấy phần hoá đơn VAT"
                v_strSQL = "SELECT * FROM VATTRAN VAT WHERE VAT.TXNUM='" & v_strTXNUM & "' AND VAT.TXDATE=TO_DATE('" & v_strTXDATE & "', '" & gc_FORMAT_DATE & "') ORDER BY VAT.VOUCHERNO"
                v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                If v_ds.Tables(0).Rows.Count > 0 Then
                    v_dataElement = pv_xmlMessage.CreateElement(Xml.XmlNodeType.Element, "vatvoucher", "")
                    For i = 0 To v_ds.Tables(0).Rows.Count - 1 Step 1
                        'Xác định các bút toán
                        v_strVOUCHERNO = gf_CorrectStringField(v_ds.Tables(0).Rows(i)("VOUCHERNO"))
                        v_strVOUCHERTYPE = gf_CorrectStringField(v_ds.Tables(0).Rows(i)("VOUCHERTYPE"))
                        v_strSERIENO = gf_CorrectStringField(v_ds.Tables(0).Rows(i)("SERIENO"))
                        v_strVOUCHERDATE = Format(gf_CorrectDateField(v_ds.Tables(0).Rows(i)("VOUCHERDATE")), gc_FORMAT_DATE)
                        v_strCUSTID = gf_CorrectStringField(v_ds.Tables(0).Rows(i)("CUSTID"))
                        v_strTAXCODE = gf_CorrectStringField(v_ds.Tables(0).Rows(i)("TAXCODE"))
                        v_strCUSTNAME = gf_CorrectStringField(v_ds.Tables(0).Rows(i)("CUSTNAME"))
                        v_strADDRESS = gf_CorrectStringField(v_ds.Tables(0).Rows(i)("ADDRESS"))
                        v_strCONTENTS = gf_CorrectStringField(v_ds.Tables(0).Rows(i)("CONTENTS"))
                        v_strDESCRIPTION = gf_CorrectStringField(v_ds.Tables(0).Rows(i)("DESCRIPTION"))
                        v_dblQTTY = gf_CorrectStringField(v_ds.Tables(0).Rows(i)("QTTY"))
                        v_dblPRICE = gf_CorrectStringField(v_ds.Tables(0).Rows(i)("PRICE"))
                        v_dblAMT = gf_CorrectStringField(v_ds.Tables(0).Rows(i)("AMT"))
                        v_dblVATRATE = gf_CorrectStringField(v_ds.Tables(0).Rows(i)("VATRATE"))
                        v_dblVATAMT = gf_CorrectStringField(v_ds.Tables(0).Rows(i)("VATAMT"))

                        'Thêm bút toán
                        v_entryNode = pv_xmlMessage.CreateNode(Xml.XmlNodeType.Element, "entry", "")
                        Dim v_attrVOUCHERNO As Xml.XmlAttribute = pv_xmlMessage.CreateAttribute("voucherno")
                        v_attrVOUCHERNO.Value = v_strVOUCHERNO
                        v_entryNode.Attributes.Append(v_attrVOUCHERNO)

                        Dim v_attrVOUCHERTYPE As Xml.XmlAttribute = pv_xmlMessage.CreateAttribute("vouchertype")
                        v_attrVOUCHERTYPE.Value = v_strVOUCHERTYPE
                        v_entryNode.Attributes.Append(v_attrVOUCHERTYPE)

                        Dim v_attrSERIENO As Xml.XmlAttribute = pv_xmlMessage.CreateAttribute("serieno")
                        v_attrSERIENO.Value = v_strSERIENO
                        v_entryNode.Attributes.Append(v_attrSERIENO)

                        Dim v_attrVOUCHERDATE As Xml.XmlAttribute = pv_xmlMessage.CreateAttribute("voucherdate")
                        v_attrVOUCHERDATE.Value = v_strVOUCHERDATE
                        v_entryNode.Attributes.Append(v_attrVOUCHERDATE)

                        Dim v_attrCUSTID As Xml.XmlAttribute = pv_xmlMessage.CreateAttribute("custid")
                        v_attrCUSTID.Value = v_strCUSTID
                        v_entryNode.Attributes.Append(v_attrCUSTID)

                        Dim v_attrTAXCODE As Xml.XmlAttribute = pv_xmlMessage.CreateAttribute("taxcode")
                        v_attrTAXCODE.Value = v_strTAXCODE
                        v_entryNode.Attributes.Append(v_attrTAXCODE)

                        Dim v_attrCUSTNAME As Xml.XmlAttribute = pv_xmlMessage.CreateAttribute("custname")
                        v_attrCUSTNAME.Value = v_strCUSTNAME
                        v_entryNode.Attributes.Append(v_attrCUSTNAME)

                        Dim v_attrADDRESS As Xml.XmlAttribute = pv_xmlMessage.CreateAttribute("address")
                        v_attrADDRESS.Value = v_strADDRESS
                        v_entryNode.Attributes.Append(v_attrADDRESS)

                        Dim v_attrCONTENTS As Xml.XmlAttribute = pv_xmlMessage.CreateAttribute("contents")
                        v_attrCONTENTS.Value = v_strCONTENTS
                        v_entryNode.Attributes.Append(v_attrCONTENTS)

                        Dim v_attrDESCRIPTION As Xml.XmlAttribute = pv_xmlMessage.CreateAttribute("description")
                        v_attrDESCRIPTION.Value = v_strDESCRIPTION
                        v_entryNode.Attributes.Append(v_attrDESCRIPTION)

                        Dim v_attrQTTY As Xml.XmlAttribute = pv_xmlMessage.CreateAttribute("qtty")
                        v_attrQTTY.Value = v_dblQTTY
                        v_entryNode.Attributes.Append(v_attrQTTY)

                        Dim v_attrPRICE As Xml.XmlAttribute = pv_xmlMessage.CreateAttribute("price")
                        v_attrPRICE.Value = v_dblPRICE
                        v_entryNode.Attributes.Append(v_attrPRICE)

                        Dim v_attrAMT As Xml.XmlAttribute = pv_xmlMessage.CreateAttribute("amt")
                        v_attrAMT.Value = v_dblAMT
                        v_entryNode.Attributes.Append(v_attrAMT)

                        Dim v_attrVATRATE As Xml.XmlAttribute = pv_xmlMessage.CreateAttribute("vatrate")
                        v_attrVATRATE.Value = v_dblVATRATE
                        v_entryNode.Attributes.Append(v_attrVATRATE)

                        Dim v_attrVATAMT As Xml.XmlAttribute = pv_xmlMessage.CreateAttribute("vatamt")
                        v_attrVATAMT.Value = v_dblVATAMT
                        v_entryNode.Attributes.Append(v_attrVATAMT)

                        v_entryNode.InnerText = v_dblVATRATE
                        v_dataElement.AppendChild(v_entryNode)
                    Next
                    pv_xmlMessage.DocumentElement.AppendChild(v_dataElement)
                End If

                'Lấy phần MITRAN
                v_strErrorMessage = "Lấy phần MITRAN"
                v_strSQL = "SELECT * FROM MITRAN MST WHERE MST.TXNUM='" & v_strTXNUM & "' AND MST.TXDATE=TO_DATE('" & v_strTXDATE & "', '" & gc_FORMAT_DATE & "') ORDER BY MST.ACCTNO"
                v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                If v_ds.Tables(0).Rows.Count > 0 Then
                    v_dataElement = pv_xmlMessage.CreateElement(Xml.XmlNodeType.Element, "mitran", "")
                    For i = 0 To v_ds.Tables(0).Rows.Count - 1 Step 1
                        'Xác định các bút toán
                        v_dblMISUBTXNO = gf_CorrectStringField(v_ds.Tables(0).Rows(i)("SUBTXNO"))
                        v_strMIDORC = gf_CorrectStringField(v_ds.Tables(0).Rows(i)("DORC"))
                        v_strMICODE = gf_CorrectStringField(v_ds.Tables(0).Rows(i)("ACCTNO"))
                        v_strMICUSTID = gf_CorrectStringField(v_ds.Tables(0).Rows(i)("CUSTID"))
                        v_strMICUSTNAME = gf_CorrectStringField(v_ds.Tables(0).Rows(i)("CUSTNAME"))
                        v_strMITASKCD = gf_CorrectStringField(v_ds.Tables(0).Rows(i)("TASKCD"))
                        v_strMIDEPTCD = gf_CorrectStringField(v_ds.Tables(0).Rows(i)("DEPTCD"))
                        v_strMIMICD = gf_CorrectStringField(v_ds.Tables(0).Rows(i)("MICD"))
                        v_strMIDESCRIPTION = gf_CorrectStringField(v_ds.Tables(0).Rows(i)("DESCRIPTION"))

                        'Tạo bút toán định khoản
                        v_entryNode = pv_xmlMessage.CreateNode(Xml.XmlNodeType.Element, "entry", "")

                        Dim v_attrSUBTXNO As Xml.XmlAttribute = pv_xmlMessage.CreateAttribute("subtxno")
                        v_attrSUBTXNO.Value = v_dblMISUBTXNO
                        v_entryNode.Attributes.Append(v_attrSUBTXNO)

                        Dim v_attrDORC As Xml.XmlAttribute = pv_xmlMessage.CreateAttribute("dorc")
                        v_attrDORC.Value = v_strMIDORC
                        v_entryNode.Attributes.Append(v_attrDORC)

                        Dim v_attrACCTNO As Xml.XmlAttribute = pv_xmlMessage.CreateAttribute("acctno")
                        v_attrACCTNO.Value = v_strMICODE
                        v_entryNode.Attributes.Append(v_attrACCTNO)

                        Dim v_attrCUSTID As Xml.XmlAttribute = pv_xmlMessage.CreateAttribute("custid")
                        v_attrCUSTID.Value = v_strMICUSTID
                        v_entryNode.Attributes.Append(v_attrCUSTID)

                        Dim v_attrCUSTNAME As Xml.XmlAttribute = pv_xmlMessage.CreateAttribute("custname")
                        v_attrCUSTNAME.Value = v_strMICUSTNAME
                        v_entryNode.Attributes.Append(v_attrCUSTNAME)

                        Dim v_attrTASKCD As Xml.XmlAttribute = pv_xmlMessage.CreateAttribute("taskcd")
                        v_attrTASKCD.Value = v_strMITASKCD
                        v_entryNode.Attributes.Append(v_attrTASKCD)

                        Dim v_attrDEPTCD As Xml.XmlAttribute = pv_xmlMessage.CreateAttribute("deptcd")
                        v_attrDEPTCD.Value = v_strMIDEPTCD
                        v_entryNode.Attributes.Append(v_attrDEPTCD)

                        Dim v_attrMICD As Xml.XmlAttribute = pv_xmlMessage.CreateAttribute("micd")
                        v_attrMICD.Value = v_strMIMICD
                        v_entryNode.Attributes.Append(v_attrMICD)

                        Dim v_attrDESCRIPTION As Xml.XmlAttribute = pv_xmlMessage.CreateAttribute("description")
                        v_attrDESCRIPTION.Value = v_strMIDESCRIPTION
                        v_entryNode.Attributes.Append(v_attrDESCRIPTION)

                        v_entryNode.InnerText = "MITRAN"
                        v_dataElement.AppendChild(v_entryNode)
                    Next
                    pv_xmlMessage.DocumentElement.AppendChild(v_dataElement)
                End If

                'Lấy phần FeeTrans
                Dim v_strFEECD, v_strGLACCTNO As String
                Dim v_dblFEEAMT, v_dblTXAMT, v_dblFEERATE, v_dblTOTALFEEAMT, v_dblTOTALVATAMT As Double
                v_dblTOTALFEEAMT = 0
                v_dblTOTALVATAMT = 0

                v_strErrorMessage = "Lấy phần FEETRAN"
                v_strSQL = "SELECT * FROM FEETRAN MST WHERE MST.TXNUM='" & v_strTXNUM & "' AND MST.TXDATE=TO_DATE('" & v_strTXDATE & "', '" & gc_FORMAT_DATE & "') "
                v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL)
                If v_ds.Tables(0).Rows.Count > 0 Then
                    v_dataElement = pv_xmlMessage.CreateElement(Xml.XmlNodeType.Element, "feemap", "")
                    For i = 0 To v_ds.Tables(0).Rows.Count - 1 Step 1
                        'Xác định các giao dịch thu phí
                        v_strFEECD = gf_CorrectStringField(v_ds.Tables(0).Rows(i)("FEECD"))
                        v_strGLACCTNO = gf_CorrectStringField(v_ds.Tables(0).Rows(i)("GLACCTNO"))
                        v_dblFEEAMT = gf_CorrectStringField(v_ds.Tables(0).Rows(i)("FEEAMT"))
                        v_dblVATAMT = gf_CorrectStringField(v_ds.Tables(0).Rows(i)("VATAMT"))
                        v_dblVATRATE = gf_CorrectStringField(v_ds.Tables(0).Rows(i)("VATRATE"))
                        v_dblTXAMT = gf_CorrectStringField(v_ds.Tables(0).Rows(i)("TXAMT"))
                        v_dblFEERATE = gf_CorrectStringField(v_ds.Tables(0).Rows(i)("FEERATE"))
                        v_dblTOTALFEEAMT = v_dblTOTALFEEAMT + v_dblFEEAMT
                        v_dblTOTALVATAMT = v_dblTOTALVATAMT + v_dblVATAMT

                        'Tạo entry v�? phí
                        v_entryNode = pv_xmlMessage.CreateNode(Xml.XmlNodeType.Element, "entry", "")
                        Dim v_attrFEECD As Xml.XmlAttribute = pv_xmlMessage.CreateAttribute("feecd")
                        v_attrFEECD.Value = v_strFEECD
                        v_entryNode.Attributes.Append(v_attrFEECD)

                        Dim v_attrGLACCTNO As Xml.XmlAttribute = pv_xmlMessage.CreateAttribute("glacctno")
                        v_attrGLACCTNO.Value = v_strGLACCTNO
                        v_entryNode.Attributes.Append(v_attrGLACCTNO)

                        Dim v_attrFEEAMT As Xml.XmlAttribute = pv_xmlMessage.CreateAttribute("feeamt")
                        v_attrFEEAMT.Value = v_dblFEEAMT
                        v_entryNode.Attributes.Append(v_attrFEEAMT)

                        Dim v_attrVATAMT As Xml.XmlAttribute = pv_xmlMessage.CreateAttribute("vatamt")
                        v_attrVATAMT.Value = v_dblVATAMT
                        v_entryNode.Attributes.Append(v_attrVATAMT)

                        Dim v_attrVATRATE As Xml.XmlAttribute = pv_xmlMessage.CreateAttribute("vatrate")
                        v_attrVATRATE.Value = v_dblVATRATE
                        v_entryNode.Attributes.Append(v_attrVATRATE)

                        Dim v_attrTXAMT As Xml.XmlAttribute = pv_xmlMessage.CreateAttribute("txamt")
                        v_attrTXAMT.Value = v_dblTXAMT
                        v_entryNode.Attributes.Append(v_attrTXAMT)

                        Dim v_attFEERATE As Xml.XmlAttribute = pv_xmlMessage.CreateAttribute("feerate")
                        v_attFEERATE.Value = v_dblFEERATE
                        v_entryNode.Attributes.Append(v_attFEERATE)

                        v_entryNode.InnerText = v_dblFEEAMT
                        v_dataElement.AppendChild(v_entryNode)
                    Next
                    pv_xmlMessage.DocumentElement.AppendChild(v_dataElement)
                End If
                pv_xmlMessage.DocumentElement.Attributes(gc_AtributeFEEAMT).Value = v_dblTOTALFEEAMT.ToString
                pv_xmlMessage.DocumentElement.Attributes(gc_AtributeVATAMT).Value = v_dblTOTALVATAMT.ToString

            Else
                v_lngErrCode = ERR_SY_TRANSACTION_NOTFOUND
                'LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
                '             & "Error code: " & v_strErrorMessage & vbNewLine _
                '             & "Error message: " & v_strSQL, EventLogEntryType.Information)
                BuildXMLErrorException(pv_xmlMessage, v_strErrorSource, v_lngErrCode, v_strErrorMessage)
                Return v_lngErrCode
            End If
            If Not v_obj Is Nothing Then v_obj.Dispose()
            Return v_lngErrCode
        Catch ex As Exception
            If Not v_obj Is Nothing Then v_obj.Dispose()
            'LogError.Write("Error source: " & v_strErrorSource & vbNewLine _
            '             & "Error code: " & v_strErrorMessage & vbNewLine _
            '             & "Error message: " & v_strSQL, EventLogEntryType.Information)
            Throw ex
        End Try
    End Function


    Public Function BuildAMTEXP(ByVal pv_xmlDocument As Xml.XmlDocument, ByVal strAMTEXP As String) As String
        Try
            Dim v_strEvaluator, v_strElemenent As String
            Dim v_lngIndex As Long
            Dim v_nodetxData As Xml.XmlNode
            Dim v_strFEEAMT As String = pv_xmlDocument.DocumentElement.Attributes(gc_AtributeFEEAMT).Value.ToString
            Dim v_strVATAMT As String = pv_xmlDocument.DocumentElement.Attributes(gc_AtributeVATAMT).Value.ToString

            v_strEvaluator = vbNullString
            v_lngIndex = 1

            While v_lngIndex < Len(strAMTEXP)
                'Get 02 charatacters in AMTEXP
                v_strElemenent = Mid$(strAMTEXP, v_lngIndex, 2)
                Select Case v_strElemenent
                    Case "FF"
                        'Fee amount
                        v_strEvaluator = v_strEvaluator & v_strFEEAMT
                    Case "VV"
                        'VAT amount
                        v_strEvaluator = v_strEvaluator & v_strVATAMT
                    Case "++", "--", "**", "//", "((", "))"
                        'Operand
                        v_strEvaluator = v_strEvaluator & Left$(v_strElemenent, 1)
                    Case Else
                        'Operator
                        v_nodetxData = pv_xmlDocument.SelectSingleNode("TransactMessage/fields/entry[@fldname='" & v_strElemenent & "']")
                        v_strEvaluator = v_strEvaluator & v_nodetxData.InnerText
                End Select
                v_lngIndex = v_lngIndex + 2
            End While
            Return v_strEvaluator
        Catch ex As Exception
            'ContextUtil.SetAbort()
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            '             & "Error code: System error!" & vbNewLine _
            '             & "Error message: " & ex.Message, EventLogEntryType.Error)
            Throw ex
        End Try
    End Function

    Public Function TransLog(ByRef pv_xmlMessage As Xml.XmlDocument) As Long
        Dim v_lngErrorCode As Long = ERR_SYSTEM_OK
        Dim v_nodeList As Xml.XmlNodeList
        Dim v_strSQLTmp As String = String.Empty, v_strSQL As String = String.Empty
        Dim v_strFLDCD As String, v_strFLDTYPE As String, v_strVALUE As String, v_dblVALUE As Double
        Dim v_objEval As New Evaluator
        Try
            Dim v_blnBEGINEND As Boolean = False
            Dim v_obj As New DataAccess
            v_obj.NewDBInstance(ATTR_REGION)

            'Insert into TLLOG
            Dim v_strTXNUM As String = pv_xmlMessage.DocumentElement.Attributes(gc_AtributeTXNUM).Value.ToString
            Dim v_strTXDATE As String = pv_xmlMessage.DocumentElement.Attributes(gc_AtributeTXDATE).Value.ToString
            Dim v_strTXTIME As String = pv_xmlMessage.DocumentElement.Attributes(gc_AtributeTXTIME).Value.ToString
            Dim v_strBRID As String = pv_xmlMessage.DocumentElement.Attributes(gc_AtributeBRID).Value.ToString
            Dim v_strTLID As String = pv_xmlMessage.DocumentElement.Attributes(gc_AtributeTLID).Value.ToString
            Dim v_strCHKID As String = pv_xmlMessage.DocumentElement.Attributes(gc_AtributeCHKID).Value.ToString
            Dim v_strOFFID As String = pv_xmlMessage.DocumentElement.Attributes(gc_AtributeOFFID).Value.ToString
            Dim v_strCFRID As String = pv_xmlMessage.DocumentElement.Attributes(gc_AtributeCFRID).Value.ToString
            Dim v_strTLTXCD As String = pv_xmlMessage.DocumentElement.Attributes(gc_AtributeTLTXCD).Value.ToString
            Dim v_strSTATUS As String = pv_xmlMessage.DocumentElement.Attributes(gc_AtributeSTATUS).Value.ToString
            Dim v_strDELTD As String = pv_xmlMessage.DocumentElement.Attributes(gc_AtributeDELTD).Value.ToString
            Dim v_strIPADDRESS As String = pv_xmlMessage.DocumentElement.Attributes(gc_AtributeIPADDRESS).Value.ToString
            Dim v_strWSNAME As String = pv_xmlMessage.DocumentElement.Attributes(gc_AtributeWSNAME).Value.ToString
            Dim v_strTXDESC As String = pv_xmlMessage.DocumentElement.Attributes(gc_AtributeTXDESC).Value.ToString
            Dim v_strBATCHNAME As String = pv_xmlMessage.DocumentElement.Attributes(gc_AtributeBATCHNAME).Value.ToString
            Dim v_strMICODE As String = pv_xmlMessage.DocumentElement.Attributes(gc_AtributeMICODE).Value.ToString
            Dim v_strMSGAMT As String = pv_xmlMessage.DocumentElement.Attributes(gc_AtributeMSGAMT).Value.ToString
            Dim v_blnReversal As Boolean = IIf(v_strDELTD = "Y", True, False)

            'Insert into TLLOGFLD
            v_nodeList = pv_xmlMessage.SelectNodes("/TransactMessage/fields")
            For i As Integer = 0 To v_nodeList.Count - 1
                For j As Integer = 0 To v_nodeList.Item(i).ChildNodes.Count - 1
                    With v_nodeList.Item(i).ChildNodes(j)
                        v_strFLDCD = .Attributes(gc_AtributeFLDNAME).Value.ToString
                        v_strFLDTYPE = .Attributes(gc_AtributeFLDTYPE).Value.ToString
                        If v_strFLDTYPE = "N" Then
                            v_strVALUE = vbNullString
                            v_dblVALUE = IIf(IsNumeric(.InnerText), .InnerText, 0)
                        Else
                            v_strVALUE = .InnerText
                            v_dblVALUE = 0
                        End If

                        If v_strFLDCD = gc_AtributeFLDTXDESC Then
                            v_strTXDESC = .InnerText
                        End If

                        v_strSQL = "INSERT INTO TLLOGFLD (AUTOID, TXNUM, TXDATE, FLDCD, CVALUE, NVALUE) " _
                            & "VALUES (SEQ_TLLOGFLD.NEXTVAL,'" & v_strTXNUM & "',TO_DATE('" & v_strTXDATE & "', '" & gc_FORMAT_DATE & "'),'" & v_strFLDCD & "','" & v_strVALUE & "'," & v_dblVALUE & ") "
                        v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
                    End With
                Next
            Next

            v_strSQL = "INSERT INTO TLLOG (AUTOID, TXNUM, TXDATE, TXTIME, BRID, " _
                                & "TLID, CHKID, OFFID, CFRID, TLTXCD, " _
                                & "TXSTATUS, " _
                                & "DELTD, IPADDRESS, WSNAME, BATCHNAME, TXDESC,MICODE, MSGAMT) VALUES (SEQ_TLLOG.NEXTVAL,'" _
                                & v_strTXNUM & "',TO_DATE('" & v_strTXDATE & "', '" & gc_FORMAT_DATE & "'),'" & v_strTXTIME & "','" & v_strBRID & "','" _
                                & v_strTLID & "','" & v_strCHKID & "','" & v_strOFFID & "','" & v_strCFRID & "','" & v_strTLTXCD & "','" _
                                & v_strSTATUS & "','" & v_strDELTD & "','" & v_strIPADDRESS & "','" & v_strWSNAME & "','" & v_strBATCHNAME & "','" & v_strTXDESC & "'," _
                                & "'" & v_strMICODE & "'," & v_strMSGAMT & ")"

            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
            If Not v_obj Is Nothing Then v_obj.Dispose()

            Return v_lngErrorCode
        Catch ex As Exception
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            '             & "Error code: System error!" & vbNewLine _
            '             & "Error message: " & ex.Message, EventLogEntryType.Error)
            Throw ex
        End Try
    End Function

    Public Function TransUpdateStatus(ByRef pv_xmlMessage As Xml.XmlDocument) As Long
        Dim v_lngErrorCode As Long = ERR_SYSTEM_OK

        Try
            Dim v_obj As New DataAccess
            v_obj.NewDBInstance(ATTR_REGION)


            'Update TLLOG: by set STATUS field
            Dim v_strTXNUM As String = pv_xmlMessage.DocumentElement.Attributes(gc_AtributeTXNUM).Value.ToString
            Dim v_strTXDATE As String = pv_xmlMessage.DocumentElement.Attributes(gc_AtributeTXDATE).Value.ToString
            Dim v_strSTATUS As String = pv_xmlMessage.DocumentElement.Attributes(gc_AtributeSTATUS).Value.ToString
            Dim v_strCHKID As String = pv_xmlMessage.DocumentElement.Attributes(gc_AtributeCHKID).Value.ToString
            Dim v_strOFFID As String = pv_xmlMessage.DocumentElement.Attributes(gc_AtributeOFFID).Value.ToString
            Dim v_strCFRID As String = pv_xmlMessage.DocumentElement.Attributes(gc_AtributeCFRID).Value.ToString
            Dim v_strDELTD As String = pv_xmlMessage.DocumentElement.Attributes(gc_AtributeDELTD).Value.ToString
            Dim v_strSQL As String = ""

            If v_strCHKID.Length > 1 Then
                v_strSQL &= " , CHKID = '" & v_strCHKID & "'"
            End If
            If v_strOFFID.Length > 1 Then
                v_strSQL &= " , OFFID = '" & v_strOFFID & "'"
            End If
            If v_strCFRID.Length > 1 Then
                v_strSQL &= " , CFRID = '" & v_strCFRID & "'"
            End If

            v_strSQL = "UPDATE TLLOG " & ControlChars.CrLf _
                            & " SET STATUS='" & v_strSTATUS & "' " & v_strSQL & " " & ControlChars.CrLf _
                            & " WHERE TXDATE = TO_DATE('" & v_strTXDATE & "','" & gc_FORMAT_DATE & "') AND TXNUM = '" & v_strTXNUM & "'"
            v_obj.ExecuteNonQuery(CommandType.Text, v_strSQL)
            If Not v_obj Is Nothing Then v_obj.Dispose()
            Return v_lngErrorCode
        Catch ex As Exception
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            '             & "Error code: System error!" & vbNewLine _
            '             & "Error message: " & ex.Message, EventLogEntryType.Error)
            Throw ex
        End Try
    End Function
    Public Function CreateAndLoadData(ByRef pv_xmlMessage As Xml.XmlDocument) As Long
        Dim v_lngErrorCode As Long = ERR_SYSTEM_OK

        Try
            Dim v_obj As New DataAccess
            v_obj.NewDBInstance(ATTR_REGION)

        Catch ex As Exception
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            '             & "Error code: System error!" & vbNewLine _
            '             & "Error message: " & ex.Message, EventLogEntryType.Error)
            Throw ex
        End Try
    End Function
#End Region
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
