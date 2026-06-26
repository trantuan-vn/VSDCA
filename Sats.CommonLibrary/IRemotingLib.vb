Imports System
Imports System.ServiceModel
Imports System.Collections.Generic
Imports System.Runtime.Serialization


Public Enum LoginStatus
    Success = 1
    Failure = 2
End Enum

Public Interface IAuthServiceLib
    Function GetAuthorizationTicket(ByVal pv_strUserName As String, ByVal pv_strPassword As String) As String
    Function GetTellerProfile(ByVal ticket As String) As CTellerProfile
    Function Message(ByVal pv_strTellerID As String, ByRef pv_arrByteMessage() As Byte) As Long
    Function ChangePassword(ByVal pv_strUserName As String, ByVal pv_strNewPassword As String) As Long
    Function GetPublicKey(ByVal pv_strUserName As String, ByRef v_arrBytePublicKey() As Byte) As Long
End Interface

Public Interface IBDSDelivery
    Function Message(ByVal pv_strTellerID As String, ByRef pv_arrByteMessage() As Byte) As Long
    Function RptMessage(ByVal pv_strTellerID As String, ByRef pv_arrByteMessage() As Byte) As Long
    Function LoadInterface(ByVal pv_strTellerID As String, ByRef pv_arrByteMessage() As Byte) As Long
    Function GetErrorMessage(ByVal pv_lngErrorCode As Long, ByVal pv_strLanguage As String) As String
    Function GetPublicKey(ByVal pv_strUserName As String, ByRef v_arrBytePublicKey() As Byte) As Long
End Interface

Public Interface IReomoteLib
    Function Message(ByRef pv_strMessage As String) As Long
    Function Message(ByRef pv_arrByteMessage() As Byte) As Long
    Function RptMessage(ByRef pv_arrByteMessage() As Byte) As Long
    Function LoadInterface(ByRef pv_arrByteMessage() As Byte) As Long
    Function GetPublicKey(ByVal pv_strUserName As String, ByRef v_arrBytePublicKey() As Byte) As Long
    Function GetAuthorizationTicket(ByVal pv_strUserName As String, ByVal pv_strPassword As String) As String
    Function GetTellerProfile(ByVal ticket As String) As CTellerProfile
End Interface

Public Interface IRemoteReport
    Function Message(ByRef pv_arrByteMessage() As Byte) As Long
    Function Message(ByRef pv_arrByteMessage() As Byte, ByVal pv_arrBytePublicKey() As Byte) As Long
    Function GetPublicKey(ByVal pv_strUserName As String, ByRef v_arrBytePublicKey() As Byte) As Long
End Interface


'Interface WCF
Public Interface IWCFCallback
    <OperationContract(IsOneWay:=True)> _
    Sub Receive(ByVal pv_oMessage As String)

    <OperationContract(IsOneWay:=True)> _
    Sub ReceiveAction(ByVal pv_oDataSet As DataSet)

    <OperationContract(IsOneWay:=True)> _
    Sub UserJoin(ByVal pv_oClient As Client)

    <OperationContract(IsOneWay:=True)> _
    Sub UserLeave(ByVal pv_oClient As Client)
End Interface

<ServiceContract(CallbackContract:=GetType(IWCFCallback), SessionMode:=SessionMode.Required)> _
Public Interface IWCF
    <OperationContract(IsInitiating:=True)> _
    Function Connect(ByVal pv_oClient As Client) As Boolean

    <OperationContract(IsInitiating:=True)> _
    Function Send(ByRef pv_arrByte() As Byte) As Long

    <OperationContract(IsInitiating:=True)> _
    Function SendCA(ByRef pv_arrByte() As Byte, ByRef v_arrByteCA() As Byte, ByRef v_strTLLOGIDCA As String, ByRef v_strVSDSignature As String) As Long

    <OperationContract(IsInitiating:=True)> _
    Function SendAction(ByVal pv_oClient As Client) As Long

    <OperationContract(IsInitiating:=True)> _
    Function GetServerRptExp(ByVal pv_strRptId As String, ByVal pv_strTLName As String, ByRef pv_intServerRptExp As Integer, ByVal pv_strSiCode As String, ByRef pv_strSiCodeAllow As String) As Long
    <OperationContract(IsInitiating:=True)> _
    Function SendRpt(ByRef pv_strMessage As String, ByRef pv_arrByte() As Byte) As Long
    <OperationContract(IsInitiating:=True)> _
    Function SendExpRpt(ByVal pv_strMessage As String) As Long
    <OperationContract(IsInitiating:=True)> _
    Function FetchRpt(ByVal pv_strRptDataKey As String, ByVal pv_intFrom As Integer, ByVal pv_intTo As Integer, ByRef pv_arrByte() As Byte) As Long

    <OperationContract(IsInitiating:=True)> _
    Function SendRptCA(ByRef pv_strMessage As String, ByRef pv_arrByte() As Byte, ByRef v_strVSDSignature As String, ByRef v_strCAKey As String, ByRef pv_strDataHash As String) As Long

    <OperationContract(IsInitiating:=True)> _
        Function SaveFileRptCA(ByVal pv_arrByte() As Byte, ByVal v_strVSDSignature As String, ByVal v_strClientVSDSignature As String, _
                               ByVal v_strTLname As String, ByVal v_strRptId As String, ByRef v_strLocalDir As String, _
                               ByRef v_strFileName As String, ByVal v_strStatus As String) As Long

    <OperationContract(IsInitiating:=True)> _
    Sub SynSend(ByRef pv_arrByte() As Byte)

    <OperationContract(IsInitiating:=True)> _
    Function Login(ByVal pv_strUserName As String, Optional ByVal pv_strPassword As String = "", Optional ByVal pv_strIPAddress As String = "") As String
    <OperationContract(IsInitiating:=True)> _
    Function LoginCA(ByVal pv_strEncryptedXML As String, _
        ByRef pv_strEncryptedSessionKey As String, ByRef pv_strResult As String) As String
    <OperationContract(IsInitiating:=True)> _
    Function FileCA(ByRef pv_strMessage As String, ByVal pv_strBrid As String, _
                    ByVal pv_strFileName As String, ByVal pv_strCurrDate As String) As String

    <OperationContract(IsInitiating:=True)> _
    Function AfterLoginCA(ByRef pv_strEncryptedSessionKey As String, _
            ByRef pv_strResult As String, _
            ByVal pv_strUserName As String) As String

    <OperationContract(IsInitiating:=True)> _
   Function ChangePassword(ByVal pv_strUserName As String, ByVal pv_strNewPassword As String) As Long

    <OperationContract(IsInitiating:=True)> _
   Function GetTellerProfile(ByVal pv_strUserName As String) As WCFTellerProfile

    <OperationContract(IsInitiating:=True)> _
    Sub Disconnect(ByVal pv_oClient As Client)
End Interface
<ServiceContract(CallbackContract:=GetType(IWCFCallback), SessionMode:=SessionMode.Required)> _
Public Interface IReportWCF
    <OperationContract(IsInitiating:=True)> _
    Function Connect(ByVal pv_oClient As Client) As Boolean
    <OperationContract(IsInitiating:=True)> _
    Function SendRpt(ByRef pv_strMessage As String, ByRef pv_arrByte() As Byte) As Long
    <OperationContract(IsInitiating:=True)> _
    Function SendExpRpt(ByVal pv_strMessage As String) As Long
    <OperationContract(IsInitiating:=True)> _
    Function FetchRpt(ByVal pv_strRptDataKey As String, ByVal pv_intFrom As Integer, ByVal pv_intTo As Integer, ByRef pv_arrByte() As Byte) As Long
    <OperationContract(IsInitiating:=True)> _
    Function SendRptCA(ByRef pv_strMessage As String, ByRef pv_arrByte() As Byte, ByRef v_strVSDSignature As String, ByRef v_strCAKey As String, ByRef pv_strDataHash As String) As Long

    <OperationContract(IsInitiating:=True)> _
    Function SaveFileRptCA(ByVal pv_arrByte() As Byte, ByVal v_strVSDSignature As String, ByVal v_strClientVSDSignature As String, _
                           ByVal v_strTLname As String, ByVal v_strRptId As String, ByRef v_strLocalDir As String, _
                           ByRef v_strFileName As String, ByVal v_strStatus As String) As Long
    <OperationContract(IsInitiating:=True)> _
    Sub Disconnect(ByVal pv_oClient As Client)
End Interface

<DataContract()> _
Public Class Client
    Private mv_strIPAddress As String
    Private mv_strWSName As String
    Private mv_strTLName As String
    Private mv_LoginTime As DateTime
    Private mv_strAction As String
    Private mv_blnActive As Boolean = False

    <DataMember()> _
    Public Property IPAddress() As String
        Get
            Return mv_strIPAddress
        End Get
        Set(ByVal value As String)
            mv_strIPAddress = value
        End Set
    End Property

    <DataMember()> _
    Public Property WSName() As String
        Get
            Return mv_strWSName
        End Get
        Set(ByVal value As String)
            mv_strWSName = value
        End Set
    End Property

    <DataMember()> _
   Public Property TLNAME() As String
        Get
            Return mv_strTLName
        End Get
        Set(ByVal value As String)
            mv_strTLName = value
        End Set
    End Property

    <DataMember()> _
   Public Property LoginTime() As DateTime
        Get
            Return mv_LoginTime
        End Get
        Set(ByVal value As DateTime)
            mv_LoginTime = value
        End Set
    End Property

    <DataMember()> _
   Public Property Action() As String
        Get
            Return mv_strAction
        End Get
        Set(ByVal value As String)
            mv_strAction = value
        End Set
    End Property

    <DataMember()> _
  Public Property isActive() As Boolean
        Get
            Return mv_blnActive
        End Get
        Set(ByVal value As Boolean)
            mv_blnActive = value
        End Set
    End Property
End Class

<DataContract()> _
Public Class WCFTellerProfile
    Private _TellerId As String
    Private _TellerName As String
    Private _TellerFullName As String
    Private _Password As String
    Private _TellerLevel As Integer
    Private _BranchId As String
    Private _BranchName As String
    Private _BusDate As String
    Private _Interval As String
    Private _TellerTitle As String
    Private _TellerPrinterName As String
    Private _TellerGroup As String
    Private _TellerType As String
    Private _Description As String
    Private _LoginTime As String
    Private _MemberFilter As String
    Private _StockFilter As String
    Private _AllStock As Integer
    Private _AllMember As Integer
    Private _MaxValue As Integer
    Private _RptExprire As Integer
    Private _PassDate As String
    Private _VsdBrid As String
    Private _VsdBrid2 As String

    <DataMember()> _
    Public Property TellerID() As String
        Get
            Return _TellerId
        End Get
        Set(ByVal value As String)
            _TellerId = value
        End Set
    End Property

    <DataMember()> _
    Public Property TellerName() As String
        Get
            Return _TellerName
        End Get
        Set(ByVal value As String)
            _TellerName = value
        End Set
    End Property

    <DataMember()> _
    Public Property TellerFullName() As String
        Get
            Return _TellerFullName
        End Get
        Set(ByVal value As String)
            _TellerFullName = value
        End Set
    End Property

    <DataMember()> _
    Public Property Password() As String
        Get
            Return _Password
        End Get
        Set(ByVal value As String)
            _Password = value
        End Set
    End Property

    <DataMember()> _
    Public Property TellerLevel() As Integer
        Get
            Return _TellerLevel
        End Get
        Set(ByVal value As Integer)
            _TellerLevel = value
        End Set
    End Property

    <DataMember()> _
    Public Property BranchId() As String
        Get
            Return _BranchId
        End Get
        Set(ByVal value As String)
            _BranchId = value
        End Set
    End Property

    <DataMember()> _
    Public Property BranchName() As String
        Get
            Return _BranchName
        End Get
        Set(ByVal value As String)
            _BranchName = value
        End Set
    End Property

    <DataMember()> _
    Public Property BusDate() As String
        Get
            Return _BusDate
        End Get
        Set(ByVal value As String)
            _BusDate = value
        End Set
    End Property

    <DataMember()> _
    Public Property Interval() As String
        Get
            Return _Interval
        End Get
        Set(ByVal value As String)
            _Interval = value
        End Set
    End Property

    <DataMember()> _
    Public Property TellerTitle() As String
        Get
            Return _TellerTitle
        End Get
        Set(ByVal value As String)
            _TellerTitle = value
        End Set
    End Property

    <DataMember()> _
    Public Property TellerPrinterName() As String
        Get
            Return _TellerPrinterName
        End Get
        Set(ByVal value As String)
            _TellerPrinterName = value
        End Set
    End Property

    <DataMember()> _
    Public Property TellerGroup() As String
        Get
            Return _TellerGroup
        End Get
        Set(ByVal value As String)
            _TellerGroup = value
        End Set
    End Property

    <DataMember()> _
    Public Property TellerType() As String
        Get
            Return _TellerType
        End Get
        Set(ByVal value As String)
            _TellerType = value
        End Set
    End Property

    <DataMember()> _
    Public Property Description() As String
        Get
            Return _Description
        End Get
        Set(ByVal value As String)
            _Description = value
        End Set
    End Property

    <DataMember()> _
    Public Property LoginTime() As String
        Get
            Return _LoginTime
        End Get
        Set(ByVal value As String)
            _LoginTime = value
        End Set
    End Property

    <DataMember()> _
    Public Property MemberFilter() As String
        Get
            Return _MemberFilter
        End Get
        Set(ByVal value As String)
            _MemberFilter = value
        End Set
    End Property

    <DataMember()> _
        Public Property StockFilter() As String
        Get
            Return _StockFilter
        End Get
        Set(ByVal value As String)
            _StockFilter = value
        End Set
    End Property

    <DataMember()> _
    Public Property AllStock() As String
        Get
            Return _AllStock
        End Get
        Set(ByVal value As String)
            _AllStock = value
        End Set
    End Property

    <DataMember()> _
    Public Property AllMember() As String
        Get
            Return _AllMember
        End Get
        Set(ByVal value As String)
            _AllMember = value
        End Set
    End Property

    <DataMember()> _
    Public Property MaxValue() As String
        Get
            Return _MaxValue
        End Get
        Set(ByVal value As String)
            _MaxValue = value
        End Set
    End Property

    <DataMember()> _
    Public Property RptExprire() As String
        Get
            Return _RptExprire
        End Get
        Set(ByVal value As String)
            _RptExprire = value
        End Set
    End Property

    <DataMember()> _
    Public Property PassDate() As String
        Get
            Return _PassDate
        End Get
        Set(ByVal value As String)
            _PassDate = value
        End Set
    End Property

    <DataMember()> _
        Public Property VsdBrid() As String
        Get
            Return _VsdBrid
        End Get
        Set(ByVal value As String)
            _VsdBrid = value
        End Set
    End Property
    <DataMember()> _
        Public Property VsdBrid2() As String
        Get
            Return _VsdBrid2
        End Get
        Set(ByVal value As String)
            _VsdBrid2 = value
        End Set
    End Property
End Class