Imports System.IO

Public Class LinqEx
    Public Shared Function GetSiSearchFld(ByVal pv_strTable As String, ByVal pv_strFieldCode As String) As TSiSearchFld
        Try
            Dim v_lstSiSearchFld = (From f In CachedSiSearchFld.Descendants("Table") Where f.Element("SEARCHCODE").Value = pv_strTable And f.Element("FIELDCODE") = pv_strFieldCode Select f).ToList

            If v_lstSiSearchFld.Count = 0 Then
                Return Nothing
            End If
            Dim v_obj As New TSiSearchFld

            For Each f In v_lstSiSearchFld
                v_obj.POSITION = Element2String(f, "POSITION")
                v_obj.FIELDCODE = Element2String(f, "FIELDCODE")
                v_obj.FIELDNAME = Element2String(f, "FIELDNAME")
                v_obj.FIELDTYPE = Element2String(f, "FIELDTYPE")
                v_obj.SEARCHCODE = Element2String(f, "SEARCHCODE")
                v_obj.FIELDSIZE = Element2String(f, "FIELDSIZE")
                v_obj.MASK = Element2String(f, "MASK")
                v_obj.OPERATOR_ = Element2String(f, "OPERATOR")
                v_obj.FORMAT = Element2String(f, "FORMAT")
                v_obj.DISPLAY = Element2String(f, "DISPLAY")
                v_obj.SRCH = Element2String(f, "SRCH")
                v_obj.KEY = Element2String(f, "KEY")
                v_obj.WIDTH = Element2String(f, "WIDTH")
                v_obj.LOOKUPCMDSQL = Element2String(f, "LOOKUPCMDSQL")
                v_obj.EN_FIELDNAME = Element2String(f, "EN_FIELDNAME")
                v_obj.REFVALUE = Element2String(f, "REFVALUE")
                v_obj.FLDCD = Element2String(f, "FLDCD")
                v_obj.DEFVALUE = Element2String(f, "DEFVALUE")
                v_obj.AUTOID = Element2String(f, "AUTOID")
                v_obj.DELETED = Element2String(f, "DELETED")
                v_obj.STATUS = Element2String(f, "STATUS")
                v_obj.LST_TABLE = Element2String(f, "LST_TABLE")
                v_obj.GROUPID = Element2String(f, "GROUPID")
            Next

            Return v_obj
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Shared Function GetSiSearchFldList(ByVal pv_strTable As String) As TSiSearchFld()
        Try
            Dim v_lstSiSearchFld = (From f In CachedSiSearchFld.Descendants("Table") Where f.Element("SEARCHCODE").Value = pv_strTable Select f).ToList

            Dim list As New List(Of TSiSearchFld)

            For Each f In v_lstSiSearchFld
                Dim v_obj As New TSiSearchFld

                v_obj.POSITION = Element2String(f, "POSITION")
                v_obj.FIELDCODE = Element2String(f, "FIELDCODE")
                v_obj.FIELDNAME = Element2String(f, "FIELDNAME")
                v_obj.FIELDTYPE = Element2String(f, "FIELDTYPE")
                v_obj.SEARCHCODE = Element2String(f, "SEARCHCODE")
                v_obj.FIELDSIZE = Element2String(f, "FIELDSIZE")
                v_obj.MASK = Element2String(f, "MASK")
                v_obj.OPERATOR_ = Element2String(f, "OPERATOR")
                v_obj.FORMAT = Element2String(f, "FORMAT")
                v_obj.DISPLAY = Element2String(f, "DISPLAY")
                v_obj.SRCH = Element2String(f, "SRCH")
                v_obj.KEY = Element2String(f, "KEY")
                v_obj.WIDTH = Element2String(f, "WIDTH")
                v_obj.LOOKUPCMDSQL = Element2String(f, "LOOKUPCMDSQL")
                v_obj.EN_FIELDNAME = Element2String(f, "EN_FIELDNAME")
                v_obj.REFVALUE = Element2String(f, "REFVALUE")
                v_obj.FLDCD = Element2String(f, "FLDCD")
                v_obj.DEFVALUE = Element2String(f, "DEFVALUE")
                v_obj.AUTOID = Element2String(f, "AUTOID")
                v_obj.DELETED = Element2String(f, "DELETED")
                v_obj.STATUS = Element2String(f, "STATUS")
                v_obj.LST_TABLE = Element2String(f, "LST_TABLE")
                v_obj.GROUPID = Element2String(f, "GROUPID")

                list.Add(v_obj)
            Next

            Return list.OrderBy(Function(p) p.POSITION).ToArray
        Catch ex As Exception
            Return Nothing
        End Try
    End Function


    Public Shared Function GetFldMasterList(ByVal pv_strObjName As String) As TFldMaster()
        Try
            Dim v_lstFldMaster = (From f In CachedFldMaster.Descendants("Table") Where f.Element("OBJNAME") = pv_strObjName Select f).ToList

            Dim list As New List(Of TFldMaster)

            For Each f In v_lstFldMaster
                Dim v_obj As New TFldMaster
                v_obj.MODCODE = Element2String(f, "MODCODE")
                v_obj.FLDNAME = Element2String(f, "FLDNAME")
                v_obj.OBJNAME = Element2String(f, "OBJNAME")
                v_obj.DEFNAME = Element2String(f, "DEFNAME")
                v_obj.CAPTION = Element2String(f, "CAPTION")
                v_obj.EN_CAPTION = Element2String(f, "EN_CAPTION")
                v_obj.ODRNUM = Element2String(f, "ODRNUM")
                v_obj.FLDTYPE = Element2String(f, "FLDTYPE")
                v_obj.FLDMASK = Element2String(f, "FLDMASK")
                v_obj.FLDFORMAT = Element2String(f, "FLDFORMAT")
                v_obj.FLDLEN = Element2String(f, "FLDLEN")
                v_obj.LLIST = Element2String(f, "LLIST")
                v_obj.LCHK = Element2String(f, "LCHK")
                v_obj.DEFVAL = Element2String(f, "DEFVAL")
                v_obj.VISIBLE = Element2String(f, "VISIBLE")
                v_obj.DISABLE = Element2String(f, "DISABLE")
                v_obj.MANDATORY = Element2String(f, "MANDATORY")
                v_obj.AMTEXP = Element2String(f, "AMTEXP")
                v_obj.VALIDTAG = Element2String(f, "VALIDTAG")
                v_obj.LOOKUP = Element2String(f, "LOOKUP")
                v_obj.DATATYPE = Element2String(f, "DATATYPE")
                v_obj.INVNAME = Element2String(f, "INVNAME")
                v_obj.FLDSOURCE = Element2String(f, "FLDSOURCE")
                v_obj.FLDDESC = Element2String(f, "FLDDESC")
                v_obj.CHAINNAME = Element2String(f, "CHAINNAME")
                v_obj.PRINTINFO = Element2String(f, "PRINTINFO")
                v_obj.LOOKUPNAME = Element2String(f, "LOOKUPNAME")
                v_obj.SEARCHCODE = Element2String(f, "SEARCHCODE")
                v_obj.SRMODCODE = Element2String(f, "SRMODCODE")
                v_obj.INVFORMAT = Element2String(f, "INVFORMAT")
                v_obj.CTLTYPE = Element2String(f, "CTLTYPE")
                v_obj.RISKFLD = Element2String(f, "RISKFLD")
                v_obj.MEMBERFIELD = Element2String(f, "MEMBERFIELD")
                v_obj.AUTOID = Element2String(f, "AUTOID")
                v_obj.DELETED = Element2String(f, "DELETED")
                v_obj.STATUS = Element2String(f, "STATUS")
                v_obj.STOCKFIELD = Element2String(f, "STOCKFIELD")
                v_obj.ISIMPORTED = Element2String(f, "ISIMPORTED")
                v_obj.ISDUPLICATED = Element2String(f, "ISDUPLICATED")
                v_obj.LOADALL = Element2String(f, "LOADALL")
                v_obj.COODRNUM = Element2String(f, "COODRNUM")
                v_obj.MAXFLDLEN = Element2String(f, "MAXFLDLEN")
                list.Add(v_obj)
            Next

            Return list.OrderBy(Function(p) p.ODRNUM).ToArray
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Shared Function GetFldMasterImpList(ByVal pv_strObjName As String) As TFldMaster()
        Try
            Dim v_lstFldMaster = (From f In CachedFldMaster.Descendants("Table") Where f.Element("OBJNAME") = pv_strObjName And f.Element("ISIMPORT") = "Y" Select f).ToList

            Dim list As New List(Of TFldMaster)

            For Each f In v_lstFldMaster
                Dim v_obj As New TFldMaster
                v_obj.MODCODE = Element2String(f, "MODCODE")
                v_obj.FLDNAME = Element2String(f, "FLDNAME")
                v_obj.OBJNAME = Element2String(f, "OBJNAME")
                v_obj.DEFNAME = Element2String(f, "DEFNAME")
                v_obj.CAPTION = Element2String(f, "CAPTION")
                v_obj.EN_CAPTION = Element2String(f, "EN_CAPTION")
                v_obj.ODRNUM = Element2String(f, "ODRNUM")
                v_obj.FLDTYPE = Element2String(f, "FLDTYPE")
                v_obj.FLDMASK = Element2String(f, "FLDMASK")
                v_obj.FLDFORMAT = Element2String(f, "FLDFORMAT")
                v_obj.FLDLEN = Element2String(f, "FLDLEN")
                v_obj.LLIST = Element2String(f, "LLIST")
                v_obj.LCHK = Element2String(f, "LCHK")
                v_obj.DEFVAL = Element2String(f, "DEFVAL")
                v_obj.VISIBLE = Element2String(f, "VISIBLE")
                v_obj.DISABLE = Element2String(f, "DISABLE")
                v_obj.MANDATORY = Element2String(f, "MANDATORY")
                v_obj.AMTEXP = Element2String(f, "AMTEXP")
                v_obj.VALIDTAG = Element2String(f, "VALIDTAG")
                v_obj.LOOKUP = Element2String(f, "LOOKUP")
                v_obj.DATATYPE = Element2String(f, "DATATYPE")
                v_obj.INVNAME = Element2String(f, "INVNAME")
                v_obj.FLDSOURCE = Element2String(f, "FLDSOURCE")
                v_obj.FLDDESC = Element2String(f, "FLDDESC")
                v_obj.CHAINNAME = Element2String(f, "CHAINNAME")
                v_obj.PRINTINFO = Element2String(f, "PRINTINFO")
                v_obj.LOOKUPNAME = Element2String(f, "LOOKUPNAME")
                v_obj.SEARCHCODE = Element2String(f, "SEARCHCODE")
                v_obj.SRMODCODE = Element2String(f, "SRMODCODE")
                v_obj.INVFORMAT = Element2String(f, "INVFORMAT")
                v_obj.CTLTYPE = Element2String(f, "CTLTYPE")
                v_obj.RISKFLD = Element2String(f, "RISKFLD")
                v_obj.MEMBERFIELD = Element2String(f, "MEMBERFIELD")
                v_obj.AUTOID = Element2String(f, "AUTOID")
                v_obj.DELETED = Element2String(f, "DELETED")
                v_obj.STATUS = Element2String(f, "STATUS")
                v_obj.STOCKFIELD = Element2String(f, "STOCKFIELD")
                v_obj.ISIMPORTED = Element2String(f, "ISIMPORTED")
                v_obj.ISDUPLICATED = Element2String(f, "ISDUPLICATED")
                v_obj.LOADALL = Element2String(f, "LOADALL")
                v_obj.COODRNUM = Element2String(f, "COODRNUM")
                v_obj.MAXFLDLEN = Element2String(f, "MAXFLDLEN")
                list.Add(v_obj)
            Next
            Return list.OrderBy(Function(p) p.ODRNUM).ToArray
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Shared Function GetFldValList(ByVal pv_strObjName As String) As TFldVal()
        Try
            Dim v_lstFldVal = (From f In CachedFldVal.Descendants("Table") Where f.Element("OBJNAME").Value = pv_strObjName Select f).ToList

            Dim list As New List(Of TFldVal)

            For Each f In v_lstFldVal
                Dim v_obj As New TFldVal

                v_obj.FLDNAME = Element2String(f, "TXDESC")
                v_obj.OBJNAME = Element2String(f, "TXDESC")
                v_obj.ODRNUM = Element2String(f, "TXDESC")
                v_obj.VALTYPE = Element2String(f, "TXDESC")
                v_obj.OPERATOR_ = Element2String(f, "TXDESC")
                v_obj.VALEXP = Element2String(f, "TXDESC")
                v_obj.VALEXP2 = Element2String(f, "TXDESC")
                v_obj.ERRMSG = Element2String(f, "TXDESC")
                v_obj.EN_ERRMSG = Element2String(f, "TXDESC")
                v_obj.AUTOID = Element2String(f, "TXDESC")
                v_obj.DELETED = Element2String(f, "TXDESC")
                v_obj.STATUS = Element2String(f, "TXDESC")
                list.Add(v_obj)
            Next

            Return list.OrderBy(Function(p) p.ODRNUM).ToArray
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Shared Function GetReportFld(ByVal pv_strReportID As String) As TRpFld()
        Try
            Dim v_lstRpFld = (From f In CachedRpFld.Descendants("Table") Where f.Element("RPTID").Value = pv_strReportID Select f).ToList

            Dim list As New List(Of TRpFld)

            For Each f In v_lstRpFld
                Dim v_obj As New TRpFld
                v_obj.AUTOID = Element2String(f, "AUTOID")
                v_obj.RPTID = Element2String(f, "RPTID")
                v_obj.PARENT_ID = Element2String(f, "PARENT_ID")
                v_obj.POSITION = Element2String(f, "POSITION")
                v_obj.FIELDNAME = Element2String(f, "FIELDNAME")
                v_obj.FIELDTYPE = Element2String(f, "FIELDTYPE")
                v_obj.CAPTION = Element2String(f, "CAPTION")
                v_obj.EN_CAPTION = Element2String(f, "EN_CAPTION")
                v_obj.FORMAT = Element2String(f, "FORMAT")
                v_obj.DISPLAY = Element2String(f, "DISPLAY")
                v_obj.WIDTH = Element2String(f, "WIDTH")
                v_obj.ISDATAFIELD = Element2String(f, "ISDATAFIELD")
                v_obj.ISSUM = Element2String(f, "ISSUM")
                v_obj.ISPARENT = Element2String(f, "ISPARENT")
                v_obj.DELETED = Element2String(f, "DELETED")
                v_obj.STATUS = Element2String(f, "STATUS")
                v_obj.ALIGN = Element2String(f, "ALIGN")
                v_obj.HEIGHT = Element2String(f, "HEIGHT")
                v_obj.LEV = Element2String(f, "LEV")

                list.Add(v_obj)
            Next
            Return list.ToArray
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Shared Function GetAllCode(ByVal pv_strCDType As String, ByVal pv_strCDName As String) As DataSet
        Dim v_ds As New DataSet
        Try
            Dim v_lstAllCode = (From f In CachedAllCode.Descendants("Table") Where _
                                    f.Element("CDTYPE") = pv_strCDType And f.Element("CDNAME") = pv_strCDName Select f).ToList

            Dim list As New List(Of TAllCode)
            Dim arr() As TAllCode

            For Each f In v_lstAllCode
                Dim v_obj As New TAllCode
                v_obj.CDVAL = Element2String(f, "CDVAL")
                v_obj.CDCONTENT = Element2String(f, "CDCONTENT")
                v_obj.LSTODR = CInt("0" & Element2String(f, "LSTODR"))
                list.Add(v_obj)
            Next
            arr = list.OrderBy(Function(p) p.LSTODR).ToArray

            Dim v_dt As DataTable
            Dim v_oCol As DataColumn
            Dim v_oRow As DataRow
            v_dt = v_ds.Tables.Add("DATA_MESSAGE")

            v_oCol = New DataColumn("VALUE")
            v_oCol.DataType = GetType(System.String)
            v_oCol.ColumnName = "VALUE"
            v_dt.Columns.Add(v_oCol)

            v_oCol = New DataColumn("DISPLAY")
            v_oCol.DataType = GetType(System.String)
            v_oCol.ColumnName = "DISPLAY"
            v_dt.Columns.Add(v_oCol)

            For i As Integer = 0 To arr.Count - 1
                v_oRow = v_dt.NewRow
                v_oRow("VALUE") = arr(i).CDVAL
                v_oRow("DISPLAY") = arr(i).CDCONTENT
                v_dt.Rows.Add(v_oRow)
            Next

            Return v_ds
        Catch ex As Exception
            Return Nothing
        Finally
            v_ds.Dispose()
        End Try
    End Function

    Public Shared Function GetAllCodeText(ByVal pv_strLang As String, ByVal pv_strCDType As String, ByVal pv_strCDName As String, ByVal pv_strValue As String) As String
        Try
            Dim v_lstAllCode = (From f In CachedAllCode.Descendants("Table") Where f.Element("LANGCODE").Value = pv_strLang And f.Element("CDTYPE") = pv_strCDType And f.Element("CDNAME") = pv_strCDName And f.Element("CDVAL") = pv_strValue _
                               Select f).ToList

            Dim v_strValue As String = ""

            For Each f In v_lstAllCode
                v_strValue = Element2String(f, "CONTENT")
            Next
            Return v_strValue
        Catch ex As Exception
            Return String.Empty
        End Try

    End Function

    Public Shared Function GetRpReport(ByVal pv_strReportID As String) As TRpReports

        Try
            Dim v_lstRpReports = (From f In CachedRpReports.Descendants("Table") Where f.Element("RPTID").Value = pv_strReportID _
                              Select f).ToList

            Dim v_obj As New TRpReports
            For Each f In v_lstRpReports
                v_obj.RPTTITLE = Element2String(f, "RPTTITLE")
                v_obj.EN_RPTTITLE = Element2String(f, "EN_RPTTITLE")
                v_obj.RPTCMDSQL = Element2String(f, "RPTCMDSQL")
                v_obj.OBJNAME = Element2String(f, "OBJNAME")
                v_obj.ORDERBYCMDSQL = Element2String(f, "ORDERBYCMDSQL")
                v_obj.RPTTYPE = Element2String(f, "RPTTYPE")
                v_obj.ORIENTATION = Element2String(f, "ORIENTATION")
                v_obj.DSN = Element2String(f, "DSN")
                v_obj.CREATEBY = Element2String(f, "CREATEBY")
                v_obj.CREATEDATE = Element2String(f, "CREATEDATE")
                v_obj.MODCODE = Element2String(f, "MODCODE")
                v_obj.RPTID = Element2String(f, "RPTID")
                v_obj.TITLE_HEIGHT = Element2String(f, "TITLE_HEIGHT")
                v_obj.HEADER_HEIGHT = Element2String(f, "HEADER_HEIGHT")
                v_obj.FOOTER_HEIGHT = Element2String(f, "FOOTER_HEIGHT")
                v_obj.AUTOID = Element2String(f, "AUTOID")
                v_obj.DELETED = Element2String(f, "DELETED")
                v_obj.STATUS = Element2String(f, "STATUS")
                v_obj.TLLOGTRAN = Element2String(f, "TLLOGTRAN")
                v_obj.RPFONTSIZE = Element2String(f, "RPFONTSIZE")
                v_obj.RPPAPERSIZE = Element2String(f, "RPPAPERSIZE")
                v_obj.ISMEMBER = Element2String(f, "ISMEMBER")
                v_obj.RPTCMDSQL1 = Element2String(f, "RPTCMDSQL1")
                v_obj.RPTLIMIT = Element2String(f, "RPTLIMIT")
                v_obj.DATAROWHEIGHT = Element2String(f, "DATAROWHEIGHT")
                v_obj.ISSIGNCA = Element2String(f, "ISSIGNCA")
            Next
            Return v_obj
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Shared Function GetTltx(ByVal pv_strTltxcd As String) As TTlTx
        Dim v_strFullPath As String = ""
        Try
            Dim v_lstSearch = (From f In CachedTltx.Descendants("Table") Where f.Element("TLTXCD").Value = pv_strTltxcd _
                              Select f).ToList

            Dim v_obj As New TTltx
            For Each f In v_lstSearch
                v_obj.TXDESC = Element2String(f, "TXDESC")
                v_obj.EN_TXDESC = Element2String(f, "EN_TXDESC")
                v_obj.LOCAL = Element2String(f, "LOCAL")
                v_obj.BACKUP = Element2String(f, "BACKUP")
                v_obj.TXTYPE = Element2String(f, "TXTYPE")
                v_obj.CHKID = Element2String(f, "CHKID")
                v_obj.OFFID = Element2String(f, "OFFID")
                v_obj.CFRID = Element2String(f, "CFRID")
                v_obj.DELETED = Element2String(f, "DELETED")
                v_obj.CHAIN = Element2String(f, "CHAIN")
                v_obj.DIRECT = Element2String(f, "DIRECT")
                v_obj.MSG_AMT = Element2String(f, "MSG_AMT")
                v_obj.AUTOID = Element2String(f, "AUTOID")
                v_obj.MFNO = Element2String(f, "MFNO")
                v_obj.STATUS = Element2String(f, "STATUS")
                v_obj.MICODE = Element2String(f, "MICODE")
                v_obj.SICODE = Element2String(f, "SICODE")
                v_obj.BUSDATE = Element2String(f, "BUSDATE")
                v_obj.COTLTXCD = Element2String(f, "COTLTXCD")
                v_obj.FILENAME = Element2String(f, "FILENAME")
                v_obj.RANGE = Element2String(f, "RANGE")
                v_obj.TXNUM = Element2String(f, "TXNUM")
                v_obj.TXDATE = Element2String(f, "TXDATE")
                v_obj.ISPARENT = Element2String(f, "ISPARENT")
                v_obj.CHILDTLTXCD = Element2String(f, "CHILDTLTXCD")
                v_obj.ISBRID = Element2String(f, "ISBRID")
                v_obj.COMICODE = Element2String(f, "COMICODE")
                v_obj.VISIBLE_CHILD = Element2String(f, "VISIBLE_CHILD")
                v_obj.DELETE_TRAN = Element2String(f, "DELETE_TRAN")
                v_obj.TXNOTE = Element2String(f, "TXNOTE")
                v_obj.TBLCHK = Element2String(f, "TBLCHK")
                v_obj.ISMF = Element2String(f, "ISMF")
                v_obj.LSTRPT = Element2String(f, "LSTRPT")
                v_obj.MFFOR = Element2String(f, "MFFOR")
                v_obj.ISSIGNCA = Element2String(f, "ISSIGNCA")
            Next

            Return v_obj
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Shared Function GetSiSearch(ByVal pv_strSearchCode As String) As TSiSearch
        Try
            Dim v_lstSiSearch = (From f In CachedSiSearch.Descendants("Table") Where f.Element("SEARCHCODE").Value = pv_strSearchCode _
                              Select f).ToList
            Dim v_obj As New TSiSearch

            For Each f In v_lstSiSearch
                v_obj.SEARCHCODE = Element2String(f, "SEARCHCODE")
                v_obj.SEARCHTITLE= Element2String(f, "SEARCHTITLE")
                v_obj.EN_SEARCHTITLE= Element2String(f, "EN_SEARCHTITLE")
                v_obj.SEARCHCMDSQL= Element2String(f, "SEARCHCMDSQL")
                v_obj.OBJNAME= Element2String(f, "OBJNAME")
                v_obj.FRMNAME= Element2String(f, "FRMNAME")
                v_obj.ORDERBYCMDSQL= Element2String(f, "ORDERBYCMDSQL")
                v_obj.TLTXCD= Element2String(f, "TLTXCD")
                v_obj.AUTOID= Element2String(f, "AUTOID")
                v_obj.DELETED= Element2String(f, "DELETED")
                v_obj.STATUS= Element2String(f, "STATUS")
                v_obj.TLLOGTRAN= Element2String(f, "TLLOGTRAN")
                v_obj.LOADALL= Element2String(f, "LOADALL")
                v_obj.FLDCHK= Element2String(f, "FLDCHK")
                v_obj.FORCED_GROUP= Element2String(f, "FORCED_GROUP")
                v_obj.SEARCHCMDSQL1= Element2String(f, "SEARCHCMDSQL1")
            Next

            Return v_obj
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    'Public Shared Sub SaveLayout(ByVal pv_strObjname As String, ByVal pv_strLayout As String, ByVal pv_strFormSize As String)
    '    Dim v_lstSearch = (From f In CachedDefineModule.Descendants("Table") Where f.Element("AOBJNAME").Value = pv_strObjname _
    '                          Select f).ToList
    '    Dim v_objTltx As New TDefmod
    '    For Each f In v_lstSearch
    '        f.SetElementValue("FORMSIZE", pv_strFormSize)
    '        f.SetElementValue("LAYOUT", pv_strLayout)
    '    Next
    'End Sub

    'Public Shared Sub SaveReport(ByVal pv_strReportID As String, ByVal pv_strLayout As String)
    '    Dim v_lstReport = (From f In CachedReport.Descendants("Table") Where f.Element("RPTID").Value = pv_strReportID
    '                          Select f).ToList
    '    For Each f In v_lstReport
    '        f.SetElementValue("RPTLAYOUT", pv_strLayout)
    '    Next
    'End Sub

    Public Shared Function Element2String(ByVal obj As XElement, ByVal strname As String) As String
        Try
            If Not obj.Element(strname) Is Nothing Then
                Return obj.Element(strname).Value.ToString.Trim
            Else
                Return ""
            End If
        Catch ex As Exception
            Return ""
        End Try
    End Function
End Class

Public Class TSiSearch
    Implements IComparable

    Public SEARCHCODE As String
    Public SEARCHTITLE As String
    Public EN_SEARCHTITLE As String
    Public SEARCHCMDSQL As String
    Public OBJNAME As String
    Public FRMNAME As String
    Public ORDERBYCMDSQL As String
    Public TLTXCD As String
    Public AUTOID As String
    Public DELETED As String
    Public STATUS As String
    Public TLLOGTRAN As String
    Public LOADALL As String
    Public FLDCHK As String
    Public FORCED_GROUP As String
    Public SEARCHCMDSQL1 As String

    Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo
        Return Me.CompareTo(obj)
    End Function
End Class

Public Class TSiSearchFld
    Implements IComparable

    Public POSITION As String
    Public FIELDCODE As String
    Public FIELDNAME As String
    Public FIELDTYPE As String
    Public SEARCHCODE As String
    Public FIELDSIZE As String
    Public MASK As String
    Public OPERATOR_ As String
    Public FORMAT As String
    Public DISPLAY As String
    Public SRCH As String
    Public KEY As String
    Public WIDTH As String
    Public LOOKUPCMDSQL As String
    Public EN_FIELDNAME As String
    Public REFVALUE As String
    Public FLDCD As String
    Public DEFVALUE As String
    Public AUTOID As String
    Public DELETED As String
    Public STATUS As String
    Public LST_TABLE As String
    Public GROUPID As String

    Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo
        Return Me.CompareTo(obj)
    End Function
End Class

Public Class TFldMaster
    Implements IComparable
    Public MODCODE As String
    Public FLDNAME As String
    Public OBJNAME As String
    Public DEFNAME As String
    Public CAPTION As String
    Public EN_CAPTION As String
    Public ODRNUM As String
    Public FLDTYPE As String
    Public FLDMASK As String
    Public FLDFORMAT As String
    Public FLDLEN As String
    Public LLIST As String
    Public LCHK As String
    Public DEFVAL As String
    Public VISIBLE As String
    Public DISABLE As String
    Public MANDATORY As String
    Public AMTEXP As String
    Public VALIDTAG As String
    Public LOOKUP As String
    Public DATATYPE As String
    Public INVNAME As String
    Public FLDSOURCE As String
    Public FLDDESC As String
    Public CHAINNAME As String
    Public PRINTINFO As String
    Public LOOKUPNAME As String
    Public SEARCHCODE As String
    Public SRMODCODE As String
    Public INVFORMAT As String
    Public CTLTYPE As String
    Public RISKFLD As String
    Public MEMBERFIELD As String
    Public AUTOID As String
    Public DELETED As String
    Public STATUS As String
    Public STOCKFIELD As String
    Public ISIMPORTED As String
    Public ISDUPLICATED As String
    Public LOADALL As String
    Public COODRNUM As String
    Public MAXFLDLEN As String

    Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo
        Return Me.CompareTo(obj)
    End Function
End Class

Public Class TFldVal
    Implements IComparable

    Public FLDNAME As String
    Public OBJNAME As String
    Public ODRNUM As String
    Public VALTYPE As String
    Public OPERATOR_ As String
    Public VALEXP As String
    Public VALEXP2 As String
    Public ERRMSG As String
    Public EN_ERRMSG As String
    Public AUTOID As String
    Public DELETED As String
    Public STATUS As String

    Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo
        Return Me.CompareTo(obj)
    End Function
End Class

Public Class TAllCode
    Implements IComparable

    Public CDTYPE As String
    Public CDNAME As String
    Public CDVAL As String
    Public CDCONTENT As String
    Public LSTODR As String
    Public CDUSER As String
    Public AUTOID As String
    Public DELETED As String
    Public STATUS As String
    Public CDVALNO As String

    Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo
        Return Me.CompareTo(obj)
    End Function
End Class

Public Class TTlTx
    Implements IComparable
    Public TXDESC As String
    Public EN_TXDESC As String
    Public LOCAL As String
    Public BACKUP As String
    Public TXTYPE As String
    Public CHKID As String
    Public OFFID As String
    Public CFRID As String
    Public DELETED As String
    Public CHAIN As String
    Public DIRECT As String
    Public MSG_AMT As String
    Public AUTOID As String
    Public MFNO As String
    Public STATUS As String
    Public MICODE As String
    Public SICODE As String
    Public BUSDATE As String
    Public COTLTXCD As String
    Public FILENAME As String
    Public RANGE As String
    Public TXNUM As String
    Public TXDATE As String
    Public ISPARENT As String
    Public CHILDTLTXCD As String
    Public ISBRID As String
    Public COMICODE As String
    Public VISIBLE_CHILD As String
    Public DELETE_TRAN As String
    Public TXNOTE As String
    Public TBLCHK As String
    Public ISMF As String
    Public LSTRPT As String
    Public MFFOR As String
    Public ISSIGNCA As String
    Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo
        Return Me.CompareTo(obj)
    End Function
End Class

Public Class TRpReports
    Implements IComparable
    Public RPTTITLE As String
    Public EN_RPTTITLE As String
    Public RPTCMDSQL As String
    Public OBJNAME As String
    Public ORDERBYCMDSQL As String
    Public RPTTYPE As String
    Public ORIENTATION As String
    Public DSN As String
    Public CREATEBY As String
    Public CREATEDATE As String
    Public MODCODE As String
    Public RPTID As String
    Public TITLE_HEIGHT As String
    Public HEADER_HEIGHT As String
    Public FOOTER_HEIGHT As String
    Public AUTOID As String
    Public DELETED As String
    Public STATUS As String
    Public TLLOGTRAN As String
    Public RPFONTSIZE As String
    Public RPPAPERSIZE As String
    Public ISMEMBER As String
    Public RPTCMDSQL1 As String
    Public RPTLIMIT As String
    Public DATAROWHEIGHT As String
    Public ISSIGNCA As String
    Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo
        Return Me.CompareTo(obj)
    End Function
End Class

Public Class TRpFld
    Implements IComparable
    Public AUTOID As String
    Public RPTID As String
    Public PARENT_ID As String
    Public POSITION As String
    Public FIELDNAME As String
    Public FIELDTYPE As String
    Public CAPTION As String
    Public EN_CAPTION As String
    Public FORMAT As String
    Public DISPLAY As String
    Public WIDTH As String
    Public ISDATAFIELD As String
    Public ISSUM As String
    Public ISPARENT As String
    Public DELETED As String
    Public STATUS As String
    Public ALIGN As String
    Public HEIGHT As String
    Public LEV As String
    Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo
        Return Me.CompareTo(obj)
    End Function
End Class
Public Class TRpGrp
    Implements IComparable

    Public RPTID As String
    Public POSITION As String
    Public WIDTH As String
    Public FIELDNAME As String
    Public FIELDTYPE As String
    Public FORMAT As String
    Public CATION As String
    Public EN_CATION As String
    Public GRPFOOTER As String
    Public AUTOID As String
    Public DELETED As String
    Public STATUS As String

    Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo
        Return Me.CompareTo(obj)
    End Function
End Class

Public Class TDefNationality
    Implements IComparable

    Public AUTOID As String
    Public NAME As String
    Public CODE As String
    Public INTERNET As String
    Public CURRENCY As String

    Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo
        Return Me.CompareTo(obj)
    End Function
End Class

Public Class TCaType
    Implements IComparable

    Public AUTOID As String
    Public TYPENO As String
    Public STATUS As String
    Public DELETED As String
    Public NAME As String
    Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo
        Return Me.CompareTo(obj)
    End Function
End Class

Public Class TMaType
    Implements IComparable
    Public TYPENO As String
    Public GL_DEP_TYPE As String
    Public GL_TRADE_TYPE As String
    Public GL_ACCT_TYPE As String
    Public SEC_TYPE As String
    Public NAME As String
    Public AUTOID As String
    Public DELETED As String
    Public STATUS As String
    Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo
        Return Me.CompareTo(obj)
    End Function
End Class

Public Class TRaType
    Implements IComparable
    Public AUTOID As String
    Public RANO As String
    Public NAME As String
    Public DELETED As String
    Public STATUS As String
    Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo
        Return Me.CompareTo(obj)
    End Function
End Class

Public Class TSfType
    Implements IComparable
    Public AUTOID As String
    Public SFNO As String
    Public NAME As String
    Public DELETED As String
    Public STATUS As String
    Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo
        Return Me.CompareTo(obj)
    End Function
End Class

Public Class TAppModules
    Implements IComparable
    Public TXCODE As String
    Public MODCODE As String
    Public MODNAME As String
    Public CLASSNAME As String
    Public AUTOID As String
    Public DELETED As String
    Public STATUS As String
    Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo
        Return Me.CompareTo(obj)
    End Function
End Class

Public Class TCurrency
    Implements IComparable
    Public CURRENCY_CODE As String
    Public NAME As String
    Public AUTOID As String
    Public NOTE As String
    Public DELETED As String
    Public STATUS As String
    Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo
        Return Me.CompareTo(obj)
    End Function
End Class

Public Class TRgIs
    Implements IComparable

    Public AUTOID As String
    Public ISTYPE As String
    Public ISNAME As String
    Public SHORT_NAME As String
    Public ISCODE As String
    Public ADDRESS As String
    Public PHONE As String
    Public FAX As String
    Public BANK_ACCOUNT As String
    Public BANK_NAME As String
    Public BUSINESS_FIELD As String
    Public CAPITAL_RULE As String
    Public FOUNDATION_NO As String
    Public FOUNDATION_DATE As String
    Public FOUNDATION_ISSUERS As String
    Public LICENSE_NO As String
    Public LICENSE_DATE As String
    Public LICENSE_ISSUER As String
    Public DELETED As String
    Public NOTE As String
    Public STATUS As String
    Public TMPID As String
    Public NCOUPONDATEPERIOD As String
    Public BUSSINESS_NO As String
    Public BUSSINESS_DATE As String
    Public BUSSINESS_ISSUERS As String
    Public ISSUER_ID As String
    Public ISSUE_DATE As String
    Public CANCEL_DATE As String
    Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo
        Return Me.CompareTo(obj)
    End Function
End Class

Public Class TMfType
    Implements IComparable
    Public AUTOID As String
    Public MFNO As String
    Public NAME As String
    Public DELETED As String
    Public STATUS As String
    Public BRID As String
    Public FOMULA As String
    Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo
        Return Me.CompareTo(obj)
    End Function
End Class
'Public Class TGl_Code
'Implements IComparable
'Public autoid As Integer
'Public gl_acc_type As Integer
'Public gl_trans_type As Integer
'Public gl_deposit_type As Integer
'Public gl_acctype_lv2 As Integer
'Public gl_trade_type As Integer
'Public gl_sec_type As Integer
'Public gl_desc As String
'Public gl_typeno As String
'Public gl_acctno As String
'Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo
'Return Me.CompareTo(obj)
'End Function
'End Class
