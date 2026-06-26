<Serializable()> _
Public Class ICCFTX
#Region " Declaration "
    Private mv_strICRULE As String
    Private mv_strTXCD As String
    Private mv_strSORU As String
    Private mv_strDATATYPE As String
    Private mv_strREFNAME As String
    Private mv_strAMTEXP As String
    Private mv_strCMPCD As String
    Private mv_strOPERAND As String
    Private mv_strCMPEXP As String
    Private mv_dblVALNUMBER As Double
    Private mv_strVALCHAR As String
#End Region

#Region " Constructors and deconstructors "
    Public Sub New()
        mv_dblVALNUMBER = 0
        mv_strVALCHAR = String.Empty
        mv_strICRULE = String.Empty
        mv_strTXCD = String.Empty
        mv_strSORU = String.Empty
        mv_strDATATYPE = String.Empty
        mv_strREFNAME = String.Empty
        mv_strAMTEXP = String.Empty
        mv_strCMPCD = String.Empty
        mv_strOPERAND = String.Empty
        mv_strCMPEXP = String.Empty
    End Sub

    Public Overloads Sub Dispose()
        mv_dblVALNUMBER = 0
        mv_strVALCHAR = String.Empty
        mv_strICRULE = String.Empty
        mv_strTXCD = String.Empty
        mv_strSORU = String.Empty
        mv_strDATATYPE = String.Empty
        mv_strREFNAME = String.Empty
        mv_strAMTEXP = String.Empty
        mv_strCMPCD = String.Empty
        mv_strOPERAND = String.Empty
        mv_strCMPEXP = String.Empty
    End Sub
#End Region

#Region " Properties "
    Public Property OPERAND() As String
        Get
            Return mv_strOPERAND
        End Get
        Set(ByVal Value As String)
            mv_strOPERAND = Value
        End Set
    End Property

    Public Property CMPEXP() As String
        Get
            Return mv_strCMPEXP
        End Get
        Set(ByVal Value As String)
            mv_strCMPEXP = Value
        End Set
    End Property

    Public Property AMTEXP() As String
        Get
            Return mv_strAMTEXP
        End Get
        Set(ByVal Value As String)
            mv_strAMTEXP = Value
        End Set
    End Property

    Public Property CMPCD() As String
        Get
            Return mv_strCMPCD
        End Get
        Set(ByVal Value As String)
            mv_strCMPCD = Value
        End Set
    End Property

    Public Property DATATYPE() As String
        Get
            Return mv_strDATATYPE
        End Get
        Set(ByVal Value As String)
            mv_strDATATYPE = Value
        End Set
    End Property

    Public Property REFNAME() As String
        Get
            Return mv_strREFNAME
        End Get
        Set(ByVal Value As String)
            mv_strREFNAME = Value
        End Set
    End Property

    Public Property ICRULE() As String
        Get
            Return mv_strICRULE
        End Get
        Set(ByVal Value As String)
            mv_strICRULE = Value
        End Set
    End Property

    Public Property TXCD() As String
        Get
            Return mv_strTXCD
        End Get
        Set(ByVal Value As String)
            mv_strTXCD = Value
        End Set
    End Property

    Public Property SORU() As String
        Get
            Return mv_strSORU
        End Get
        Set(ByVal Value As String)
            mv_strSORU = Value
        End Set
    End Property

    Public Property VALNUMBER() As Double
        Get
            Return mv_dblVALNUMBER
        End Get
        Set(ByVal Value As Double)
            mv_dblVALNUMBER = Value
        End Set
    End Property

    Public Property VALCHAR() As String
        Get
            Return mv_strVALCHAR
        End Get
        Set(ByVal Value As String)
            mv_strVALCHAR = Value
        End Set
    End Property

#End Region
End Class
