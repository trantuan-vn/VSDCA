<Serializable()> _
Public Class ReportSetting
    Private mv_strRptType As String
    Private mv_strRptOrientation As String
    Private mv_strSQL As String
    Private mv_strTitle As String
    Private mv_strEnTitle As String
    Private mv_strDSN As String
    Private mv_strUserLanguage As String
    Private mv_strReportID As String
    Private mv_strCreateBy As String
    Private mv_strCreateDate As String
    Private mv_strParam As String
    Private mv_dblHeight As Double
    Private mv_dblFHeight As Double
    Private mv_dblHHeight As Double
    Private mv_objRptFld() As ReportHeaderRow
    Private mv_objRptgrp() As ReportGroup
    Private mv_strObjName As String
    Private mv_strRPFontSize As String
    Private mv_strRPPaperSize As String
    Private mv_strBusDate As String
    Private mv_strCompanyName As String
    Private mv_strAddress As String
    Private mv_strBranchName As String

    Private mv_strMIName As String
    Private mv_strMIAddress As String
    Private mv_strMIPhone As String
    Private mv_strBUSINESS_NO As String

    Private mv_strWhereDate As String
    Private mv_dblDataRowHeight As Double
    'BangPV: Them setting cho ký số
    Private mv_strIsSignCA As String

    Public Property IsSignCA() As String
        Get
            Return mv_strIsSignCA
        End Get
        Set(ByVal value As String)
            mv_strIsSignCA = value
        End Set
    End Property
    'end bangpv

    Public Property DataRowHeight() As Double
        Get
            Return mv_dblDataRowHeight
        End Get
        Set(ByVal value As Double)
            mv_dblDataRowHeight = value
        End Set
    End Property

    Public Property WhereDate() As String
        Get
            Return mv_strWhereDate
        End Get
        Set(ByVal value As String)
            mv_strWhereDate = value
        End Set
    End Property

    Public Property MIName() As String
        Get
            Return mv_strMIName
        End Get
        Set(ByVal value As String)
            mv_strMIName = value
        End Set
    End Property

    Public Property MIAddress() As String
        Get
            Return mv_strMIAddress
        End Get
        Set(ByVal value As String)
            mv_strMIAddress = value
        End Set
    End Property

    Public Property MIPhone() As String
        Get
            Return mv_strMIPhone
        End Get
        Set(ByVal value As String)
            mv_strMIPhone = value
        End Set
    End Property

    Public Property BUSINESS_NO() As String
        Get
            Return mv_strBUSINESS_NO
        End Get
        Set(ByVal value As String)
            mv_strBUSINESS_NO = value
        End Set
    End Property

    Public Property CompanyName() As String
        Get
            Return mv_strCompanyName
        End Get
        Set(ByVal value As String)
            mv_strCompanyName = value
        End Set
    End Property

    Public Property Address() As String
        Get
            Return mv_strAddress
        End Get
        Set(ByVal value As String)
            mv_strAddress = value
        End Set
    End Property

    Public Property BranchName() As String
        Get
            Return mv_strBranchName
        End Get
        Set(ByVal value As String)
            mv_strBranchName = value
        End Set
    End Property

    Public Property BusDate() As String
        Get
            Return mv_strBusDate
        End Get
        Set(ByVal value As String)
            mv_strBusDate = value
        End Set
    End Property

    Public Property FontSize() As String
        Get
            Return mv_strRPFontSize
        End Get
        Set(ByVal value As String)
            mv_strRPFontSize = value
        End Set
    End Property

    Public Property PaperSize() As String
        Get
            Return mv_strRPPaperSize
        End Get
        Set(ByVal value As String)
            mv_strRPPaperSize = value
        End Set
    End Property

    Public Property CreateBy() As String
        Get
            Return mv_strCreateBy
        End Get
        Set(ByVal value As String)
            mv_strCreateBy = value
        End Set
    End Property

    Public Property CreateDate() As String
        Get
            Return mv_strCreateDate
        End Get
        Set(ByVal value As String)
            mv_strCreateDate = value
        End Set
    End Property

    Public Property Param() As String
        Get
            Return mv_strParam
        End Get
        Set(ByVal value As String)
            mv_strParam = value
        End Set
    End Property

    Public Property ReportID() As String
        Get
            Return mv_strReportID
        End Get
        Set(ByVal value As String)
            mv_strReportID = value
        End Set
    End Property

    Public Property UserLanguage() As String
        Get
            Return mv_strUserLanguage
        End Get
        Set(ByVal value As String)
            mv_strUserLanguage = value
        End Set
    End Property

    Public Property ReportType() As String
        Get
            Return mv_strRptType
        End Get
        Set(ByVal value As String)
            mv_strRptType = value
        End Set
    End Property

    Public Property Title() As String
        Get
            Return mv_strTitle
        End Get
        Set(ByVal value As String)
            mv_strTitle = value
        End Set
    End Property

    Public Property En_Title() As String
        Get
            Return mv_strEnTitle
        End Get
        Set(ByVal value As String)
            mv_strEnTitle = value
        End Set
    End Property

    Public Property SQLCommand() As String
        Get
            Return mv_strSQL
        End Get
        Set(ByVal value As String)
            mv_strSQL = value
        End Set
    End Property

    Public Property Orientation() As String
        Get
            Return mv_strRptOrientation
        End Get
        Set(ByVal value As String)
            mv_strRptOrientation = value
        End Set
    End Property

    Public Property DSN() As String
        Get
            Return mv_strDSN
        End Get
        Set(ByVal value As String)
            mv_strDSN = value
        End Set
    End Property

    Public Property HeaderHeight() As String
        Get
            Return mv_dblHeight
        End Get
        Set(ByVal value As String)
            mv_dblHeight = value
        End Set
    End Property

    Public Property FHeight() As Double
        Get
            Return mv_dblFHeight
        End Get
        Set(ByVal value As Double)
            mv_dblFHeight = value
        End Set
    End Property

    Public Property HHeight() As Double
        Get
            Return mv_dblHHeight
        End Get
        Set(ByVal value As Double)
            mv_dblHHeight = value
        End Set
    End Property

    Public Property ReportFld() As ReportHeaderRow()
        Get
            Return mv_objRptFld
        End Get
        Set(ByVal value() As ReportHeaderRow)
            mv_objRptFld = value
        End Set
    End Property

    Public Property ReportGrp() As ReportGroup()
        Get
            Return mv_objRptgrp
        End Get
        Set(ByVal value() As ReportGroup)
            mv_objRptgrp = value
        End Set
    End Property

    Public Property ObjectName() As String
        Get
            Return mv_strObjName
        End Get
        Set(ByVal value As String)
            mv_strObjName = value
        End Set
    End Property
End Class

<Serializable()> _
Public Class ReportHeaderRow
    Private mv_intID As Integer
    Private mv_intParentID As Integer
    Private mv_strCaption As String
    Private mv_strEnCaption As String
    Private mv_dblWidth As Double
    Private mv_strFldName As String
    Private mv_strFldType As String
    Private mv_strFormat As String
    Private mv_strIsDataField As String
    Private mv_strDisplay As String
    Private mv_strIsSum As String
    Private mv_strIsParent As String
    Private mv_strTextAlign As String
    Private mv_intLev As Integer
    Private mv_dblHeight As Double

    Public Property ID() As Integer
        Get
            Return mv_intID
        End Get
        Set(ByVal value As Integer)
            mv_intID = value
        End Set
    End Property

    Public Property ParentID() As Integer
        Get
            Return mv_intParentID
        End Get
        Set(ByVal value As Integer)
            mv_intParentID = value
        End Set
    End Property

    Public Property Lev() As Integer
        Get
            Return mv_intLev
        End Get
        Set(ByVal value As Integer)
            mv_intLev = value
        End Set
    End Property

    Public Property Height() As Double
        Get
            Return mv_dblHeight
        End Get
        Set(ByVal value As Double)
            mv_dblHeight = value
        End Set
    End Property

    Public Property Caption() As String
        Get
            Return mv_strCaption
        End Get
        Set(ByVal value As String)
            mv_strCaption = value
        End Set
    End Property

    Public Property En_Caption() As String
        Get
            Return mv_strEnCaption
        End Get
        Set(ByVal value As String)
            mv_strEnCaption = value
        End Set
    End Property

    Public Property Width() As Double
        Get
            Return mv_dblWidth
        End Get
        Set(ByVal value As Double)
            mv_dblWidth = value
        End Set
    End Property

    Public Property Display() As String
        Get
            Return mv_strDisplay
        End Get
        Set(ByVal value As String)
            mv_strDisplay = value
        End Set
    End Property

    Public Property IsSum() As String
        Get
            Return mv_strIsSum
        End Get
        Set(ByVal value As String)
            mv_strIsSum = value
        End Set
    End Property

    Public Property IsDataField() As String
        Get
            Return mv_strIsDataField
        End Get
        Set(ByVal value As String)
            mv_strIsDataField = value
        End Set
    End Property

    Public Property FieldName() As String
        Get
            Return mv_strFldName
        End Get
        Set(ByVal value As String)
            mv_strFldName = value
        End Set
    End Property

    Public Property FieldType() As String
        Get
            Return mv_strFldType
        End Get
        Set(ByVal value As String)
            mv_strFldType = value
        End Set
    End Property


    Public Property Format() As String
        Get
            Return mv_strFormat
        End Get
        Set(ByVal value As String)
            mv_strFormat = value
        End Set
    End Property

    Public Property IsParent() As String
        Get
            Return mv_strIsParent
        End Get
        Set(ByVal value As String)
            mv_strIsParent = value
        End Set
    End Property

    Public Property TextAlign() As String
        Get
            Return mv_strTextAlign
        End Get
        Set(ByVal value As String)
            mv_strTextAlign = value
        End Set
    End Property

End Class

<Serializable()> _
Public Class ReportGroup
    Private mv_strFldName As String
    Private mv_strFldType As String
    Private mv_strCaption As String
    Private mv_strEnCaption As String
    Private mv_dblCaptionWidth As Double
    Private mv_strFormat As String
    Private mv_strFooter As String

    Public Property FieldName() As String
        Get
            Return mv_strFldName
        End Get
        Set(ByVal value As String)
            mv_strFldName = value
        End Set
    End Property

    Public Property FldType() As String
        Get
            Return mv_strFldType
        End Get
        Set(ByVal value As String)
            mv_strFldType = value
        End Set
    End Property

    Public Property En_Caption() As String
        Get
            Return mv_strEnCaption
        End Get
        Set(ByVal value As String)
            mv_strEnCaption = value
        End Set
    End Property

    Public Property Caption() As String
        Get
            Return mv_strCaption
        End Get
        Set(ByVal value As String)
            mv_strCaption = value
        End Set
    End Property

    Public Property CaptionWidth() As Double
        Get
            Return mv_dblCaptionWidth
        End Get
        Set(ByVal value As Double)
            mv_dblCaptionWidth = value
        End Set
    End Property

    Public Property Format() As String
        Get
            Return mv_strFormat
        End Get
        Set(ByVal value As String)
            mv_strFormat = value
        End Set
    End Property

    Public Property Footer() As String
        Get
            Return mv_strFooter
        End Get
        Set(ByVal value As String)
            mv_strFooter = value
        End Set
    End Property

End Class

