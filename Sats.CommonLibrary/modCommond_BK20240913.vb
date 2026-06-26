Imports System.Net
Imports System.Runtime.InteropServices
Imports System
Imports System.Text
Imports System.Xml.Linq


<Serializable()> _
Public Module modCommond

#Region " Các hằng số ngôn ngữ "
    Public Const gc_LANG_VIETNAMESE = "VN"
    Public Const gc_LANG_ENGLISH = "EN"
#End Region
#Region " Các hằng số ma hoa "
    Public Const gc_PREFIX = "vsd@123"
    Public Const gc_ENCRYPT_PASSWORD = "562673c7ae24d6295abf6d8e72f3d039"
#End Region

#Region " Authorization Mode constants "
    Public Const gc_AUTHORIZATION_MODE_LDAP = "LDAP"
    Public Const gc_AUTHORIZATION_MODE_DB = "DB"
#End Region

#Region " Common constants "

    Public Const ADMIN_ID = "0001"
    Public Const HO_BRID = "0001"
    Public Const gc_ApplicationTitle = "VSDS 1.0"


    'Thông báo trường không được bỏ trống
    Public Const gc_VN_MANDATORY = "Thông tin '@' không được bỏ trống!"
    Public Const gc_EN_MANDATORY = "Thông tin '@' không được bỏ trống!"

    'Các hằng số l
    Public Const gc_MODULE_CLIENT = "VSDS_CLIENT"
    Public Const gc_MODULE_BDS = "VSDS_BDS"
    Public Const gc_MODULE_HOST = "VSDS_HOST"
    Public Const gc_MODULE_INQUERY = "VSDS_INQUERY"
    Public Const gc_WEB_SERVICE_TIMEOUT = 3600000
    Public Const DAILY_TRANSACTION = "DAY"
    Public Const MSGTRANS_DELETED = "Y"
    Public Const MSGTRANS_UNDELETE = "N"
    Public Const HOST_BRID = "0001"
    Public Const OPERATION_INACTIVE = "0"
    Public Const OPERATION_ACTIVE = "1"
    Public Const OPERATION_STANDBY = "2"
    Public Const BRGRP_ACTIVE = "A"
    Public Const BRGRP_OFFLINE = "O"
    Public Const BRGRP_CLOSED = "C"
    Public Const BATCH_PREFIXED = "99"

    'Hằng số nguyên nhân duyệt giao dịch
    Public Const gc_OFFID_OVRRQS = "@00"
    Public Const OVRRQS_CHECKER_CONTROL = "@00"
    Public Const OVRRQS_TRANSACTIONLIMIT = "@01"
    Public Const OVRRQS_CASHIERLIMIT = "@02"
    Public Const OVRRQS_APPROVELIMIT = "@03"
    Public Const OVRRQS_CUSTOMERLIMIT = "@04"
    Public Const OVRRQS_INTERBRANCH = "@05"
    Public Const OVRRQS_DELETETRANSACTION = "@06"
    Public Const OVRRQS_CONTRACTLIMIT = "@07"
    Public Const OVRRQS_SECURERATIO = "@08"
    Public Const OVRRQS_SPECIAL = "@09"
    Public Const OVRRQS_OVERMARGINLIMIT = "@10"
    Public Const OVRRQS_ORDERTRADELIMIT = "@11"
    Public Const OVRRQS_ORDERSECURERATIO = "@12"
    Public Const OVRRQS_AFTRADELIMIT = "@13"
    Public Const OVRRQS_MARGINLIMIT = "@14"
    Public Const OVRRQS_TRADELIMIT = "@15"
    Public Const OVRRQS_ADVANCELIMIT = "@16"
    Public Const OVRRQS_REPOLIMIT = "@17"
    Public Const OVRRQS_DEPOSITLIMIT = "@18"

    'Atribute ObjectMessage & TransactMessage
    Public Const gc_AtributeTXDATE = "TXDATE"
    Public Const gc_AtributeTXTIME = "TXTIME"
    Public Const gc_AtributeBRDATE = "BRDATE"
    Public Const gc_AtributeBUSDATE = "BUSDATE"
    Public Const gc_AtributeTXNUM = "TXNUM"
    Public Const gc_AtributeLOCAL = "LOCAL"
    Public Const gc_AtributePRETRAN = "PRETRAN"
    Public Const gc_AtributeSTOREPROC = "STOREPROC"
    Public Const gc_AtributePARAM_NAME = "PARAMNAME"
    Public Const gc_AtributePARAM_VALUE = "PARAMVALUE"
    Public Const gc_AtributePARAM_SIZE = "PARAMSIZE"
    Public Const gc_AtributePARAM_TYPE = "PARAMTYPE"
    Public Const gc_AtributeNUM_OF_PARAM = "NUM_OF_PARAM"
    Public Const gc_AtributeTLID = "TLID"
    Public Const gc_AtributeTLNAME = "TLNAME"
    Public Const gc_AtributeGRPID = "GRPID"
    Public Const gc_AtributeBRID = "BRID"
    Public Const gc_AtributeBRCODE = "BRCODE"
    Public Const gc_AtributeIPADDRESS = "IPADDRESS"
    Public Const gc_AtributeWSNAME = "WSNAME"
    Public Const gc_AtributeOFFID = "OFFID"
    Public Const gc_AtributeOFFNAME = "OFFNAME"
    Public Const gc_AtributeCHKID = "CHKID"
    Public Const gc_AtributeCHKNAME = "CHKNAME"
    Public Const gc_AtributeCHID = "CHID"
    Public Const gc_AtributeCFRID = "CFRID"
    Public Const gc_AtributeCFRNAME = "CFRNAME"
    Public Const gc_AtributeMICODE = "MICODE"
    Public Const gc_AtributeSICODE = "SICODE"
    Public Const gc_AtributeISPARENT = "ISPARENT"
    Public Const gc_AtributePARENTID = "PARENTID"
    Public Const gc_AtributePARENT_TEXT = "PARENT_TEXT"
    Public Const gc_AtributeCHILDTLTXCD = "CHILDTLTXCD"
    Public Const gc_AtributeISBRID = "ISBRID"
    Public Const gc_AtributeCOMICODE = "COMICODE"
    Public Const gc_AtributeREASON = "REASON"
    Public Const gc_AtributeMISSING_WARNING = "MISSING_WARNING"
    Public Const gc_AtributeOLDSTATUS = "OLDSTATUS"
    Public Const gc_AtributeMSGAMT = "MSGAMT"
    Public Const gc_AtributeDELTDTXNUM = "DELTDTXNUM"
    Public Const gc_AtributeTXNAME = "TXNAME"
    Public Const gc_AtributeSignatureVSD = "VSDSignature"
    Public Const gc_AtributeSignatureClient = "ClientSignature"
    Public Const gc_AtributeLocalDirCA = "LocalDirCA"
    Public Const gc_AtributeFileNameCA = "FileNameCA"
    Public Const gc_TRANSACTION_ZERO = "0"
    Public Const gc_IMP_TRAN_UNIT = 500

    'Hanm5 them
    Public Const gc_AtributeTXNOTE = "TXNOTE"
    Public Const gc_AtributeVSDBRID = "VSD_BRID"
    Public Const gc_AtributeVSDBRID2 = "VSD_BRID2"
    Public Const gc_AtributeTBLCHK = "TBLCHK"
    Public Const gc_AtributePARENT_TLTXCD = "PARENT_TLTXCD"
    Public Const gc_AtributePARENT_TXNUM = "PARENT_TXNUM"
    Public Const gc_AtributePARENT_TXDATE = "PARENT_TXDATE"
    Public Const gc_AtributePARENT_BUSDATE = "PARENT_BUSDATE"
    'bangpv
    Public Const gc_AtributeSIGNCA = "SIGNCA"
    Public Const gc_AtributeFileNameCACHK = "FILENAMECACHK"
    Public Const gc_AtributeSignatureCACHK = "SignatureCACHK"
    Public Const gc_AtributeStateFile = "StateFile"

    'Add by Thanglv - 24/09/2013
    Public Const gc_AtributeFileName = "FILENAME"

    'Add by HaNM5 23/12/2020
    Public Const gc_AtributeRptDataKey = "RPTDATAKEY"
    Public Const gc_AtributeRptDataRowCount = "RPTDATAROWCOUNT"

    Public Const gc_AtributeTLID2 = "TLID2"
    Public Const gc_AtributeBRID2 = "BRID2"
    Public Const gc_AtributeIBT = "IBT"
    Public Const gc_AtributeMSGACCT = "MSGACCT"
    Public Const gc_AtributeCHKTIME = "CHKTIME"
    Public Const gc_AtributeOFFTIME = "OFFTIME"


    Public Const gc_AtributeUPDATEMODE = "UPDATEMODE"
    Public Const gc_AtributeOVRRQS = "OVRRQD"
    Public Const gc_AtributeDELTD = "DELTD"
    Public Const gc_AtributeSTATUS = "STATUS"
    Public Const gc_AtributeSTATUSTEXT = "STATUS_TEXT"
    Public Const gc_AtributeBATCHNAME = "BATCHNAME"
    Public Const gc_AtributeTLTXCD = "TLTXCD"
    Public Const gc_AtributeNOSUBMIT = "NOSUBMIT"
    Public Const gc_AtributeDELALLOW = "DELALLOW"
    Public Const gc_AtributeTXTYPE = "TXTYPE"
    Public Const gc_AtributeCCYUSAGE = "CCYUSAGE"
    Public Const gc_AtributeOFFLINE = "OFFLINE"
    Public Const gc_AtributeMSGSTS = "MSGSTS"
    Public Const gc_AtributeOVRSTS = "OVRSTS"
    Public Const gc_AtributeFEEAMT = "FEEAMT"
    Public Const gc_AtributeVATAMT = "VATAMT"
    Public Const gc_AtributeVOUCHER = "VOUCHER"
    Public Const gc_AtributeTXDESC = "TXDESC"
    Public Const gc_AtributeLANGUAGE = "LANGUAGE"

    Public Const gc_AtributeMSGTYPE = "MSGTYPE"
    Public Const gc_AtributeOBJNAME = "OBJNAME"
    Public Const gc_AtributeFUNCNAME = "FUNCTIONNAME"
    Public Const gc_AtributeACTFLAG = "ACTIONFLAG"
    Public Const gc_AtributeCLAUSE = "CLAUSE"
    Public Const gc_AtributeCMDINQUIRY = "CMDINQUIRY"
    Public Const gc_AtributeAUTOID = "AUTOID"
    Public Const gc_AtributeREFERENCE = "REFERENCE"

    Public Const gc_AtributeFLDTXDESC = "30"
    Public Const gc_AtributeFLDNAME = "fldname"
    Public Const gc_AtributeFLDTYPE = "fldtype"

    Public Const gc_AtributeMODCODE = "MODCODE"

    'MessagePrintInfo
    Public Const gc_PrintInfoCUSTNAME = "CUSTNAME"
    Public Const gc_PrintInfoADDRESS = "ADDRESS"
    Public Const gc_PrintInfoLICENSE = "LICENSE"

    'ActionFlag
    Public Const gc_ActionDelete = "DELETE"
    Public Const gc_ActionInquiry = "INQUIRY"
    Public Const gc_ActionEdit = "EDIT"
    Public Const gc_ActionAdd = "ADD"
    Public Const gc_ActionAdhoc = "ADHOC"

    'MessageType
    Public Const gc_MsgTypeObj = "O"
    Public Const gc_MsgTypeTrans = "T"
    Public Const gc_MsgTypeRpt = "R"
    Public Const gc_MsgTypeProc = "P"

    'MessageLocation
    Public Const gc_IsLocalMsg = "Y"
    Public Const gc_IsNotLocalMsg = "N"
    Public Const gc_IsInQueryNotLocalMsg = "IN"
    Public Const gc_IsInQueryLocalMsg = "IY"
    Public Const gc_TLTX_BDS = "Y"
    Public Const gc_TLTX_HOST = "N"

    'OrderStatus
    Public Const gc_ORDER_OPEN = "1"
    Public Const gc_ORDER_SENT = "2"
    Public Const gc_ORDER_CANCEL = "3"
    Public Const gc_ORDER_EXECUTED = "4"
    Public Const gc_ORDER_EXPIRED = "5"
    Public Const gc_ORDER_REJECTED = "6"
    Public Const gc_ORDER_COMPLETED = "7"
    Public Const gc_ORDER_PENDING = "8"

    'Hằng số qui định sử dụng AUTOID
    Public Const gc_AutoIdUsed = "Y"
    Public Const gc_AutoIdUnused = "N"

    'Hằng số mã giao dịch dịch

    Public Const gc_XcryLicense = "CRY11-BU1NZ-AZDHH-AKBA"
    Public Const gc_ZipLicense = "SCO11-BU16Z-BZDYH-AKBA"
    Public Const gc_ERR_MSG_GET_INTERFACE_VN = "Việc cập nhật dữ liệu không thành công, bạn vui lòng chạy file AutoUpdate để cập nhật lại hoặc liên hệ quản trị hệ thống để được giúp đỡ"
    Public Const gc_ERR_MSG_VN = "Hệ thống xảy ra lỗi, hãy cập nhật phiên bản mới nhất hoặc liên hệ với Quản trị hệ thống để được giúp đỡ!"
    Public Const gc_ERR_MSG_EN = "Hệ thống xảy ra lỗi, hãy cập nhật phiên bản mới nhất hoặc liên hệ với Quản trị hệ thống để được giúp đỡ!"
    Public Const gc_ERR_GET_MSG_VN = "Lỗi lấy dữ liệu từ máy chủ, hãy liên hệ với Quản trị hệ thống để được giúp đỡ!"
    Public Const gc_ERR_GET_MSG_EN = "Lỗi lấy dữ liệu từ máy chủ, hãy liên hệ với Quản trị hệ thống để được giúp đỡ!"
    Public Const gc_ERR_DAY_NOT_STARTED_VN = "Hệ thống chưa bắt đầu ngày nên bạn không thực hiện được thao tác này!"
    Public Const gc_ERR_DAY_NOT_STARTED_EN = "Hệ thống chưa bắt đầu ngày nên bạn không thực hiện được thao tác này!"
    Public Const gc_ERR_DAY_INVALID_VN = "Hệ thống đã thay đổi ngày nên bạn không thực hiện được thao tác này! Vui lòng thoát ra và đăng nhập lại."
    Public Const gc_ERR_DAY_INVALID_EN = "Hệ thống đã thay đổi ngày nên bạn không thực hiện được thao tác này! Vui lòng thoát ra và đăng nhập lại."

#End Region
#Region " Object combobox "
    Public Declare Function SendMessage Lib "user32" Alias "SendMessageA" (ByVal hwnd As Long, ByVal wMsg As Long, ByVal wParam As Long, ByVal lParam As String) As Long
#End Region

#Region " Object name constants "

    Public Const OBJNAME_RG_START = "RG."
    Public Const OBJNAME_RG_RGIS = OBJNAME_RG_START & "RGIS"
    Public Const OBJNAME_RG_RGMI = OBJNAME_RG_START & "RGMI"
    Public Const OBJNAME_RG_RGSI = OBJNAME_RG_START & "RGSI"
    Public Const OBJNAME_RG_RGII = OBJNAME_RG_START & "RGII"

    Public Const OBJNAME_CS_START = "CS."
    Public Const OBJNAME_CS_NETSTOCK = OBJNAME_CS_START & "NETSTOCK"
    Public Const OBJNAME_CS_CSTRADE = OBJNAME_CS_START & "CSTRADE"
    Public Const OBJNAME_CS_VIEWBALANCE = OBJNAME_CS_START & "VIEWBALANCE"
    Public Const OBJNAME_CS_VIEWNETBANK = OBJNAME_CS_START & "VIEWNETBANK"

    Public Const OBJNAME_SY_START = "SY."
    Public Const OBJNAME_SY_AUTHENTICATION = OBJNAME_SY_START & "AUTH"
    Public Const OBJNAME_SY_BDSSYSTEM = OBJNAME_SY_START & "BDSSYSTEM"
    Public Const OBJNAME_SY_SEARCHFLD = OBJNAME_SY_START & "SEARCHFLD"
    Public Const OBJNAME_SY_DEFERROR = OBJNAME_SY_START & "DEFERROR"
    Public Const OBJNAME_SY_TLLOG = OBJNAME_SY_START & "TLLOG"

    Public Const OBJNAME_SA_START = "SA."
    Public Const OBJNAME_SA_SYNDATA = OBJNAME_SA_START & "SYNDATA"
    Public Const OBJNAME_SA_BRGRP = OBJNAME_SA_START & "BRGRP"
    Public Const OBJNAME_SA_TLPROFILES = OBJNAME_SA_START & "   "
    Public Const OBJNAME_SA_TLGROUPS = OBJNAME_SA_START & "TLGROUPS"
    Public Const OBJNAME_SA_BEGINOFDAY = OBJNAME_SA_START & "BEGINOFDAY"
    Public Const OBJNAME_SA_ENDOFDAY = OBJNAME_SA_START & "ENDOFDAY"
    Public Const OBJNAME_SA_TLTX = OBJNAME_SA_START & "TLTX"
    Public Const OBJNAME_SA_TLAUTH = OBJNAME_SA_START & "TLAUTH"
    Public Const OBJNAME_SA_CMDAUTH = OBJNAME_SA_START & "CMDAUTH"
    Public Const OBJNAME_SA_FLDMASTER = OBJNAME_SA_START & "FLDMASTER"
    Public Const OBJNAME_SA_FLDVAL = OBJNAME_SA_START & "FLDVAL"
    Public Const OBJNAME_SA_APPMODULES = OBJNAME_SA_START & "APPMODULES"
    Public Const OBJNAME_SA_SYSVAR = OBJNAME_SA_START & "SYSVAR"
    Public Const OBJNAME_SA_SYNCODE = OBJNAME_SA_START & "SYNCODE"
    Public Const OBJNAME_SA_ALLCODE = OBJNAME_SA_START & "ALLCODE"
    Public Const OBJNAME_SA_LOOKUP = OBJNAME_SA_START & "LOOKUP"
    Public Const OBJNAME_SA_FUNCASSIGN = OBJNAME_SA_START & "FUNCASSIGN"
    Public Const OBJNAME_SA_TRANSASSIGN = OBJNAME_SA_START & "TRANSASSIGN"
    Public Const OBJNAME_SA_RPTASSIGN = OBJNAME_SA_START & "RPTASSIGN"
    Public Const OBJNAME_SA_TLTXUSERAUTH = OBJNAME_SA_START & "TLTXUSERAUTH"

    Public Const OBJNAME_CA_START = "CA."
    Public Const OBJNAME_CA_CAMAST = OBJNAME_CA_START & "CAMAST"

    Public Const OBJNAME_MF_START = "MF."
    Public Const OBJNAME_MF_ADDMFTOTLTX = OBJNAME_CA_START & "ADDMFTOTLTX"

    Public Const OBJNAME_RP_START = "RP."
    Public Const OBJNAME_RP_RPLIST = OBJNAME_RP_START & "RPLIST"
    Public Const OBJNAME_RP_RPREPORT = OBJNAME_RP_START & "RPREPORT"
    Public Const OBJNAME_RP_RPSTORE = OBJNAME_RP_START & "RPSTORE"

#End Region

#Region " Các hằng số format dữ liệu "
    Public Const gc_FORMAT_DATE = "dd/MM/yyyy"
    Public Const gc_FORMAT_TIME = "hh:mm:ss"
    Public Const gc_FORMAT_TXNUM = "0000000000"
    Public Const gc_FORMAT_BATCHTXNUM = "0000000000"
    Public Const gc_FORMAT_NUMBER = "#,##0.0000"
    Public Const gc_FORMAT_NUMBER_2 = "#,##0.00"
    Public Const gc_FORMAT_GLACCTNO = "####.##.#####.####"
    Public Const gc_FORMAT_ODDATE = "DDMMYYYY"
    Public Const gc_FORMAT_ODAUTOID = "000000"
    Public Const gc_FORMAT_RPAUTOID = "000000"
    Public Const MaxNumber = "100000000000000"
    Public Const gc_NULL_DATE = "01/01/1753"
#End Region

#Region " Các hằng số định nghĩa mã phân hệ nghiệp vụ "
    Public Const SUB_SYSTEM_SY = "SY"
    Public Const SUB_SYSTEM_SA = "SA"
    Public Const SUB_SYSTEM_RG = "RG"
    Public Const SUB_SYSTEM_DE = "DE"
    Public Const SUB_SYSTEM_MF = "MF"
    Public Const SUB_SYSTEM_CS = "CS"
    Public Const SUB_SYSTEM_SF = "SF"
    Public Const SUB_SYSTEM_CA = "CA"
    Public Const SUB_SYSTEM_TA = "TA"
    Public Const SUB_SYSTEM_FI = "FI"
    Public Const SUB_SYSTEM_SD = "SD"
    Public Const SUB_SYSTEM_RP = "RP"
#End Region

#Region " Error code constants "
    Public Const ERR_SYSTEM_OK = 0
    Public Const ERR_SYSTEM_START = ERR_SYSTEM_OK - 1
    ' Se xoa di
    Public Const ERR_SA_CHECKER1_OVR = ERR_SA_START - 10
    Public Const ERR_SA_CHECKER2_OVR = ERR_SA_START - 11
    Public Enum TransactStatus1
        Logged = 0              'Logged
        Completed = 1           'Completed
        ErrorOccured = 2        'Error
        Cashier = 3             'Unsetted
        Pending = 4             'Pending to approve
        Rejected = 5            'Rejected
        MsgRequired = 6         'SWIFT missing
        Deleting = 7            'Pending to delete
        Refuse = 8             'Refuse
        Deleted = 9            'Deleted
        Remittance = 10          'Remittance
    End Enum
    Public Const gc_OD_CISEND = "8821"
    Public Const gc_OD_CIRECEIVE = "8822"
    Public Const gc_OD_SESEND = "8823"
    Public Const gc_OD_SERECEIVE = "8824"
    Public Const gc_OD_BATCH_CISEND_FEE = "8855"
    Public Const gc_OD_BATCH_CIRECEIVE_FEE = "8856"
    Public Const gc_OD_BATCH_CISEND = "8865"
    Public Const gc_OD_BATCH_CIRECEIVE = "8866"
    Public Const gc_OD_BATCH_SESEND = "8867"
    Public Const gc_OD_BATCH_SERECEIVE = "8868"
    Public Const gc_OD_BATCH_RLS_ADVANCED = "8861"
    Public Const gc_OD_MATCHORDER = "8804"
    Public Const gc_OD_MANUAL_MATCHORDER = "8809"
    Public Const gc_OD_RELEASEBUYORDER = "8862"
    Public Const gc_OD_RELEASESELLORDER = "8863"
    Public Const gc_OD_FINISHORDER = "8864"
    Public Const gc_OD_BATCH_DEPOSIT = "8869"
    Public Const gc_CI_CASHDEPOSIT = "1140"
    Public Const gc_CI_GL_TRF_CI = "1141"
    Public Const gc_CI_RCV_BRROWED = "1142"
    Public Const gc_CI_OD_ADVANCED = "1143"
    Public Const gc_CI_UNBLOCK_AMOUNT = "1145"
    Public Const gc_CI_DEALING_GL_TRF_CI = "1146"
    Public Const gc_CI_TRF_GLTOCI = "1147"
    Public Const gc_CI_DEVIDION_ALLOCATION = "1149"
    Public Const gc_CI_INT_TO_PRINCIPLE = "1150"
    Public Const gc_CI_TRANSFER2BANK = "1101"
    Public Const gc_CI_PAIDADVANCEDPAYMENT = "1103"
    Public Const gc_CI_ORDERADVANCEDPAYMENT = "1143"
    Public Const gc_CI_COMPLETETRANSFER2BANK = "1104"
    Public Const gc_CI_CRINTACR = "1160"
    Public Const gc_CI_ODINTACR = "1161"
    Public Const gc_CI_CRINTPRINCIPAL = "1162"
    Public Const gc_CI_ODINTPRINCIPAL = "1163"
    Public Const gc_CI_OPENACCOUNT = "1170"
    Public Const gc_CI_ACCOUNTINQUIRY = "1171"
    Public Const gc_CI_ACCOUNTHISTORY = "1172"
    Public Const gc_CI_GETINTTRANS = "1175"
    Public Const gc_CI_ContractCloseRequest = "0088"
    Public Const ERR_SA_PRINTINFO_ACCTNOTFOUND = ERR_SA_START - 15

    Public Const ERR_SA_BUSDATE_BRANCHDATE_PLZLOGIN_OUT = ERR_SA_START - 83
    Public Const ERR_SA_HOST_OPERATION_STILLACTIVE = ERR_SA_START - 22
    Public Const ERR_CF_CUSTOM_NOTFOUND = ERR_CF_START - 2
    Public Const ERR_CF_START = ERR_SYSTEM_OK - 200000
    Public Const gc_GL_CASHTRF = "9901"

    ' Giao dịch trên HOST
    Public Const ERR_SA_HOST_OPERATION_ISINACTIVE = ERR_SA_START - 23
    Public Const ERR_SA_HOST_TRANSACTION_MICODE = ERR_SA_START - 24

    '--System SY
    Public Const ERR_SY_STATUS_IS_BLOCKED = ERR_SYSTEM_OK - 2
    Public Const ERR_SY_STATUS_IS_SENT = ERR_SYSTEM_OK - 3
    Public Const ERR_SY_HOST_OPERATION_ISINACTIVE = ERR_SYSTEM_OK - 4
    Public Const ERR_SY_HOST_OPERATION_STILLACTIVE = ERR_SYSTEM_OK - 5
    Public Const ERR_SY_STILLHAS_BRGRP_ACTIVE = ERR_SYSTEM_OK - 6
    Public Const ERR_SY_NO_DATAFOUND = ERR_SYSTEM_OK - 7
    Public Const ERR_SY_APPCHK_ACCTNO_NOTFOUND = ERR_SYSTEM_OK - 8
    Public Const ERR_SY_CHECKER1_OVR = ERR_SYSTEM_OK - 9
    Public Const ERR_SY_CHECKER2_OVR = ERR_SYSTEM_OK - 10
    Public Const ERR_SY_CRTUSR_NOTTELLER = ERR_SYSTEM_OK - 11
    Public Const ERR_SY_TRANSACT_CMDALLOW = ERR_SYSTEM_OK - 12
    Public Const ERR_SY_VARIABLE_NOTFOUND = ERR_SYSTEM_OK - 13
    Public Const ERR_SY_TRANSACTION_NOTFOUND = ERR_SYSTEM_OK - 14
    Public Const ERR_SY_TLTXCD_NOTFOUND = ERR_SYSTEM_OK - 15
    Public Const ERR_SY_PERMITION = ERR_SYSTEM_OK - 16

    '--Phan he SA
    Public Const ERR_SA_START = ERR_SYSTEM_OK - 100000
    Public Const ERR_SA_BRID_DUPLICATED = ERR_SA_START - 1
    Public Const ERR_SA_BR_HAS_USER = ERR_SA_START - 2
    Public Const ERR_SA_BRNAME_DUPLICATED = ERR_SA_START - 3
    Public Const ERR_SA_BDS_OPERATION_ISINACTIVE = ERR_SA_START - 21
    Public Const ERR_SA_GROUP_NOT_EXIST = ERR_SA_START - 31
    Public Const ERR_SA_GROUP_EXIST = ERR_SA_START - 32
    Public Const ERR_SA_BUSDATE_DIF_TXDATE = ERR_SA_START - 92
    Public Const ERR_SA_TRANSACT_CMDALLOW = ERR_SA_START - 47
    Public Const ERR_SA_TRANSACT_TRANSOVRLIMIT = ERR_SA_START - 51
    Public Const ERR_SA_TRANSACT_TELLERLIMIT_NOTDEFINED = ERR_SA_START - 70
    '----AllCode----
    Public Const ERR_SA_CDVAL_DUPLICATED = ERR_SA_START - 2

    '----TLGROUPS
    Public Const ERR_SA_GRPID_DUPLICATED = ERR_SA_START - 3
    Public Const ERR_SA_GRPNAME_DUPLICATED = ERR_SA_START - 4
    Public Const ERR_SA_GRP_HAS_CHILD = ERR_SA_START - 5

    '----TLPROFILES
    Public Const ERR_SA_TL_IN_SYS = ERR_SA_START - 6
    Public Const ERR_SA_TL_EDIT_CURRENT_USR = ERR_SA_START - 7
    Public Const ERR_SA_TL_HAS_TLAUTH = ERR_SA_START - 8
    Public Const ERR_SA_TL_HAS_CHILD = ERR_SA_START - 9
    '----BRGRP
    Public Const ERR_SA_TLID_DUPLICATED = ERR_SA_START - 10
    Public Const ERR_SA_TLNAME_DUPLICATED = ERR_SA_START - 11
    '----TLTXUSERAUTH
    Public Const ERR_SA_TLXUSERAUTH_DUPLICATED = ERR_SA_START - 12
    Public Const ERR_SA_AUTHID_NOT_EXIST = ERR_SA_START - 13
    Public Const ERR_SA_AUTHID_EXIST = ERR_SA_START - 14

    'Phân hệ RG
    '----IS
    Public Const ERR_RG_START = ERR_SYSTEM_OK - 200000
    Public Const ERR_RG_ISNAME_DUPLICATED = ERR_RG_START - 1
    Public Const ERR_RG_IS_SHORT_NAME_DUPLICATED = ERR_RG_START - 2
    Public Const ERR_RG_IS_NAME_DUPLICATED = ERR_RG_START - 3
    Public Const ERR_RG_IS_CODE_DUPLICATED = ERR_RG_START - 4
    Public Const ERR_RG_IS_HAS_SI = ERR_RG_START - 5
    Public Const ERR_RG_IS_BUSSINESSNO_DUPLICATED = ERR_RG_START - 43
    Public Const ERR_RG_IS_INSERT_TO_RGII = ERR_RG_START - 44
    Public Const ERR_RG_IS_BUSSINESSNO_DUPLICATED_MI = ERR_RG_START - 45

    '----MI
    Public Const ERR_RG_MI_CODE_DUPLICATED = ERR_RG_START - 6
    Public Const ERR_RG_MI_CODETRADE_DUPLICATED = ERR_RG_START - 7
    Public Const ERR_RG_MI_BUSSINESSNO_DUPLICATED = ERR_RG_START - 40
    Public Const ERR_RG_MI_BUSSINESSNO_DUPLICATED_IS = ERR_RG_START - 41
    Public Const ERR_RG_MI_INSERT_TO_RGII = ERR_RG_START - 42
    Public Const ERR_RG_MI_ACCTNO_DUPLICATED = ERR_RG_START - 8
    Public Const ERR_RG_MI_NAME_DUPLICATED = ERR_RG_START - 9
    Public Const ERR_RG_MI_HAS_TRANS = ERR_RG_START - 10
    '----SI
    Public Const ERR_RG_SI_CODE_DUPLICATED = ERR_RG_START - 10
    Public Const ERR_RG_IS_HAS_STOCK = ERR_RG_START - 12
    Public Const ERR_RG_SI_ISTYPE11 = ERR_RG_START - 20
    Public Const ERR_RG_SI_ISTYPE12 = ERR_RG_START - 21
    Public Const ERR_RG_SI_ISTYPE21 = ERR_RG_START - 22
    Public Const ERR_RG_SI_ISTYPE22 = ERR_RG_START - 23
    Public Const ERR_RG_SI_ISTYPE3 = ERR_RG_START - 24
    Public Const ERR_RG_SI_ISTYPE4 = ERR_RG_START - 25
    Public Const ERR_RG_SI_ISTYPE5 = ERR_RG_START - 26
    Public Const ERR_RG_SI_HAS_TRANS = ERR_RG_START - 30
    'Add by ThangLV9 2017/07/19 - Check
    Public Const ERR_RG_SI_CW_CODE_1 = ERR_RG_START - 50
    Public Const ERR_RG_SI_CW_CODE_2 = ERR_RG_START - 51
    Public Const ERR_RG_SI_CW_CODE_3 = ERR_RG_START - 52
    Public Const ERR_RG_SI_CW_CODE_4 = ERR_RG_START - 53
    Public Const ERR_RG_SI_CW_CODE_5 = ERR_RG_START - 54
    Public Const ERR_RG_SI_PARTVALUE_1 = ERR_RG_START - 60
    'Added by HoaLX3 20230514
    Public Const ERR_RG_SI_BONDPERIOD_1 = ERR_RG_START - 61


    '---II
    Public Const ERR_RG_II_CARD_DUPLICATED = ERR_RG_START - 11

    'Phân hệ DE
    Public Const ERR_DE_START = ERR_SYSTEM_OK - 300000
    Public Const gc_DE_DEPOSIT_SECURITIES = "3001"
    'Phân hệ MF
    Public Const ERR_MF_START = ERR_SYSTEM_OK - 400000

    'Phân hệ CS
    Public Const ERR_CS_START = ERR_SYSTEM_OK - 500000

    'Phân hệ SF
    Public Const ERR_SF_START = ERR_SYSTEM_OK - 600000

    'Phân hệ CA
    Public Const ERR_CA_START = ERR_SYSTEM_OK - 700000

    'Phân hệ TA
    Public Const ERR_TA_START = ERR_SYSTEM_OK - 800000

    'Phân hệ SD
    Public Const ERR_SD_START = ERR_SYSTEM_OK - 900000

    'Phân hệ FI
    Public Const ERR_FI_START = ERR_SYSTEM_OK - 110000

    'Phân hệ RP
    Public Const ERR_RP_START = ERR_SYSTEM_OK - 120000

#End Region

#Region " Define constants for currency "
    Public Const BASED_CCYCD = "00"
    Public Const POS_CCYCD = 8
    Public Const BASED_CCYCD_DECIMAL = "2"
#End Region

#Region " Constants for report "
    'Hằng số quy ước phạm vi tạo báo cáo
    Public Const gc_REPORT_AREA_ALL = "A"           'Báo cáo toàn công ty
    Public Const gc_REPORT_AREA_BRANCH = "B"        'Báo cáo chi nhánh
    Public Const gc_REPORT_AREA_AGENT = "S"         'Báo cáo đại lý nhận lệnh

    'Constants for report formular name
    Public Const gc_RPT_FORMULAR_START = "F_"
    Public Const gc_RPT_FORMULAR_COMPANY_NAME = gc_RPT_FORMULAR_START & "COMPANY_NAME"
    Public Const gc_RPT_FORMULAR_ADDRESS = gc_RPT_FORMULAR_START & "ADDRESS"
    Public Const gc_RPT_FORMULAR_PHONE_FAX = gc_RPT_FORMULAR_START & "PHONE_FAX"
    Public Const gc_RPT_FORMULAR_REPORT_TITLE = gc_RPT_FORMULAR_START & "REPORT_TITLE"
    Public Const gc_RPT_FORMULAR_CREATED_DATE = gc_RPT_FORMULAR_START & "CREATED_DATE"
    Public Const gc_RPT_FORMULAR_CREATED_BY = gc_RPT_FORMULAR_START & "CREATED_BY"

    Public Const gc_RPT_FORMULAR_FROM_DATE = gc_RPT_FORMULAR_START & "FROM_DATE"
    Public Const gc_RPT_FORMULAR_TO_DATE = gc_RPT_FORMULAR_START & "TO_DATE"
    Public Const gc_RPT_FORMULAR_REPORT_CRITERIAS = gc_RPT_FORMULAR_START & "REPORT_CRITERIAS"

    Public Const DATA_PASSWORD As String = "123abc"
    Public Const DATA_FILE As String = "VSDClient.sdf"
    Public Const DATA_ENCRYPT_FILE As String = "VSDClient.enc"
#End Region

#Region " Common functions & variables "

    'Các kiểu tác động tới CSDL của form
    Public Enum ExecuteFlag
        View = 0
        AddNew = 1
        Edit = 2
        Delete = 3
        Execute = 4
        Stoped = 5
        Returned = 6
        Executed = 7
    End Enum

    Public Enum TransactStatus
        LOG_MEMBER_STAFF = 0         'Created by Member Staff 
        APPROVED_MEMBER_MANAGER = 1         'Approved by Member Manager
        APPROVED_VDS_STAFF = 2         'Approved by VSD Staff
        CONFIRMED_VSD_MANAGER = 3         'Confirmed by VSD Manager
        REJECTED_LOG = 4         'Rejected log
    End Enum

    Public Const LOG_MEMBER_STAFF_TEXT As String = "Chờ duyệt"         'Created by Member Staff 
    Public Const APPROVED_MEMBER_MANAGER_TEXT As String = "Duyệt cấp trưởng TVLK" 'Approved by Member Manager
    Public Const APPROVED_VDS_STAFF_TEXT As String = "Duyệt cấp cán bộ VSD"  'Approved by VSD Staff
    Public Const CONFIRMED_VSD_MANAGER_TEXT As String = "Duyệt cấp lãnh đạo VSD" 'Confirmed by VSD Manager
    Public Const REJECTED_LOG_TEXT As String = "Từ chối duyệt" 'Rejected log
    Public Const DELETED_TRANS_TEXT As String = "Đã xóa" 'Delete transaction - BằngPV



    Public Enum ProcessType
        BatchProcess = 0
        ReportProcess = 1
    End Enum

    <DllImport("iphlpapi.dll", ExactSpelling:=True)> _
    Public Function SendARP(ByVal DestIP As Integer, ByVal SrcIP As Integer, ByVal pMacAddr() As Byte, ByRef PhyAddrLen As Integer) As Integer

    End Function
    ' tuanta
    ' convert string to string literal
    Public Function ToLiteral(ByVal input As String) As String
        Dim arrBytes As Byte() = Encoding.UTF8.GetBytes(input)
        Dim strReturn As String = ""
        For i = 0 To UBound(arrBytes)
            strReturn = strReturn & Chr(arrBytes(i)).ToString
        Next
        Return strReturn
    End Function
    ' tuanta
    ' convert string to string literal
    Public Function ToNVarchar(ByVal input As String) As String
        Dim arrBytes As Byte() = Encoding.UTF8.GetBytes(input)
        Dim strReturn As String = ""
        For i = 0 To UBound(arrBytes)
            strReturn = strReturn & Chr(arrBytes(i)).ToString
        Next
        Return strReturn
    End Function
    ' end tuanta
    'bangpv: dataset to xml 
    Public Function Dataset_to_CSV(ByVal ds As DataSet, ByVal v_strRPTID As String) As String
        Dim str As New StringBuilder
        '
        For Each dr As DataRow In ds.Tables(0).Rows
            For Each field As Object In dr.ItemArray
                str.Append(field.ToString & ",")
            Next
            str.Replace(",", vbNewLine, str.Length - 1, 1)
        Next
        Try
            If Not System.IO.Directory.Exists("C:\SatsReport\Csv") Then
                System.IO.Directory.CreateDirectory("C:\SatsReport\Csv")
            End If            
            My.Computer.FileSystem.WriteAllText("C:\SatsReport\Csv\" & v_strRPTID & ".csv", str.ToString, False)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function gf_Numberic(ByVal v_str As String) As Boolean
        If IsNumeric(v_str) Then
            Return True
        Else
            Dim v_strLeft, v_strRight As String
            v_strLeft = v_str
            v_strRight = "0"
            For i As Integer = 0 To v_str.Length - 1
                If v_str.Chars(i) = "/" Then
                    v_strLeft = v_str.Substring(0, i)
                    v_strRight = v_str.Substring(i + 1, v_str.Length - i - 1)
                    Exit For
                End If
            Next
            If IsNumeric(Trim(v_strLeft)) And Not IsDecimal(Trim(v_strLeft)) And IsNumeric(Trim(v_strRight)) And Not IsDecimal(Trim(v_strRight)) And Trim(v_strRight) <> "0" Then
                Return True
            Else
                Return False
            End If
        End If
    End Function
    Public Function gf_FormatNumberToSring(ByVal v_str As String, ByVal v_int As Integer) As String
        Dim v_strLeft, v_strRight As String
        v_strLeft = String.Empty
        v_strRight = String.Empty
        If Len(v_str) > 0 Then
            If IsNumeric(v_str) Then
                v_strLeft = v_str
                v_strRight = "1"
            Else
                If gf_Numberic(v_str) Then
                    For i As Integer = 0 To v_str.Length - 1
                        If v_str.Chars(i) = "/" Then
                            v_strLeft = v_str.Substring(0, i)
                            v_strRight = v_str.Substring(i + 1, v_str.Length - i - 1)
                            Exit For
                        End If
                    Next
                End If
            End If
        End If
        If v_int = 0 Then
            Return v_strLeft
        Else
            Return v_strRight
        End If
    End Function
    Public Function IsDecimal(ByVal v_str As String) As Boolean
        Dim t As String
        For i As Integer = 0 To v_str.Length - 1
            t = v_str.Substring(i, 1)
            If t = "." Then Return True
        Next
        Return False
    End Function

    Public Function gf_Cdbl(ByVal v_str As String) As Double
        If IsNumeric(v_str) Then
            Return CDbl(v_str)
        Else
            Dim i As Integer
            Dim v_strLeft, v_strRight As String
            v_strLeft = v_str
            v_strRight = "0"
            For i = 0 To v_str.Length - 1
                If v_str.Chars(i) = "/" Then
                    v_strLeft = v_str.Substring(0, i)
                    v_strRight = v_str.Substring(i + 1, v_str.Length - i - 1)
                    Exit For
                End If
            Next
            If IsNumeric(Trim(v_strLeft)) And IsNumeric(Trim(v_strRight)) And Trim(v_strRight) <> "0" Then
                Return CDbl(CDbl(Trim(v_strLeft)) / CDbl(Trim(v_strRight)))
            End If
        End If
    End Function
    Public Function gf_GeneratePIN() As String
        Try
            Dim v_strPIN As String
            v_strPIN = CType(Rnd(), String).ToString
            If Len(v_strPIN) > 50 Then v_strPIN = Left(v_strPIN, 50)
            Return v_strPIN
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function gf_CorrectStringField(ByVal data As Object) As String
        Try
            Return IIf(IsDBNull(data), String.Empty, data)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function gf_CorrectNumericField(ByVal data As Object) As Decimal
        Try
            Return IIf(IsDBNull(data), 0, data)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function gf_CorrectDateField(ByVal data As Object) As Date
        Try

            Return IIf(IsDBNull(data), Now.Date, data)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function gf_CorrectBooleanField(ByVal data As Object) As Boolean
        Try
            Return IIf(IsDBNull(data), False, data)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function LogXmlMessage(ByVal v_strXmlMsg As String, ByVal v_strPathFileName As String) As Boolean
        Try
            Dim writer As Xml.XmlTextWriter = New Xml.XmlTextWriter(v_strPathFileName, Nothing)
            Dim v_xmlLogMessage As New Xml.XmlDocument
            writer.Formatting = Xml.Formatting.Indented
            v_xmlLogMessage.LoadXml(v_strXmlMsg)
            v_xmlLogMessage.Save(writer)
            writer.Close()
        Catch ex As Exception
            Throw ex
        End Try
    End Function


    'Public Function BuildXMLTxMsg(Optional ByVal pv_strMSGTYPE As String = "T", _
    '                                Optional ByVal pv_strLOCAL As String = "", _
    '                                Optional ByVal pv_strTLTXCD As String = "", _
    '                                Optional ByVal pv_strBRID As String = "", _
    '                                Optional ByVal pv_strTLID As String = "", _
    '                                Optional ByVal pv_strIPADDRESS As String = "", _
    '                                Optional ByVal pv_strWSNAME As String = "", _
    '                                Optional ByVal pv_strTXTYPE As String = "", _
    '                                Optional ByVal pv_strNOSUBMIT As String = "", _
    '                                Optional ByVal pv_strSTATUS As String = "0", _
    '                                Optional ByVal pv_strDELTD As String = "N", _
    '                                Optional ByVal pv_strOVRRQS As String = "", _
    '                                Optional ByVal pv_strUPDATEMODE As String = "", _
    '                                Optional ByVal pv_strOFFID As String = "", _
    '                                Optional ByVal pv_strCHKID As String = "", _
    '                                Optional ByVal pv_strCFRID As String = "", _
    '                                Optional ByVal pv_strCHID As String = "", _
    '                                Optional ByVal pv_strIBT As String = "", _
    '                                Optional ByVal pv_strBRID2 As String = "", _
    '                                Optional ByVal pv_strTLID2 As String = "", _
    '                                Optional ByVal pv_strTXDATE As String = "", _
    '                                Optional ByVal pv_strTXTIME As String = "", _
    '                                Optional ByVal pv_strTXNUM As String = "", _
    '                                Optional ByVal pv_strBRDATE As String = "", _
    '                                Optional ByVal pv_strBUSDATE As String = "", _
    '                                Optional ByVal pv_strCCYUSAGE As String = "00", _
    '                                Optional ByVal pv_strOFFLINE As String = "N", _
    '                                Optional ByVal pv_strMSGSTS As String = "0", _
    '                                Optional ByVal pv_strOVRSTS As String = "0", _
    '                                Optional ByVal pv_strPRETRAN As String = "Y", _
    '                                Optional ByVal pv_strDELALLOW As String = "Y", _
    '                                Optional ByVal pv_strTXDESC As String = "", _
    '                                Optional ByVal pv_strBATCHNAME As String = DAILY_TRANSACTION) As String
    '    Try
    '        Dim XMLDocumentMessage As New Xml.XmlDocument
    '        Dim dataElement As Xml.XmlElement
    '        Dim v_attrMSGTYPE, v_attrTLTXCD, v_attrTLID, v_attrBRID, v_attrIPADDRESS, v_attrWSNAME, v_attrDELALLOW, _
    '            v_attrTXTYPE, v_attrNOSUBMIT, v_attrSTATUS, v_attrDELTD, _
    '            v_attrOVRRQS, v_attrUPDATEMODE, v_attrLOCAL, v_attrOFFID, _
    '            v_attrCHKID, v_attrCHID, v_attrCFRID, v_attrIBT, v_attrBRID2, _
    '            v_attrTLID2, v_attrTXDATE, v_attrTXTIME, v_attrTXNUM, _
    '            v_attrBRDATE, v_attrBUSDATE, v_attrCCYUSAGE, v_attrOFFLINE, _
    '            v_attrMSGSTS, v_attrOVRSTS, v_attrPRETRAN, v_attrTXDESC, v_attrBATCHNAME, v_attrFEEAMT, v_attrVATAMT, v_attrVOUCHER As Xml.XmlAttribute
    '        Dim v_attrMSGAMT, v_attrMSGACCT, v_attrCHKTIME, v_attrOFFTIME As Xml.XmlAttribute

    '        dataElement = XMLDocumentMessage.CreateElement("TransactMessage")



    '        v_attrMSGTYPE = XMLDocumentMessage.CreateAttribute(gc_AtributeMSGTYPE)
    '        v_attrMSGTYPE.Value = pv_strMSGTYPE
    '        dataElement.Attributes.Append(v_attrMSGTYPE)

    '        v_attrTLTXCD = XMLDocumentMessage.CreateAttribute(gc_AtributeTLTXCD)
    '        v_attrTLTXCD.Value = pv_strTLTXCD
    '        dataElement.Attributes.Append(v_attrTLTXCD)

    '        v_attrBRID = XMLDocumentMessage.CreateAttribute(gc_AtributeBRID)
    '        v_attrBRID.Value = pv_strBRID
    '        dataElement.Attributes.Append(v_attrBRID)

    '        v_attrTLID = XMLDocumentMessage.CreateAttribute(gc_AtributeTLID)
    '        v_attrTLID.Value = pv_strTLID
    '        dataElement.Attributes.Append(v_attrTLID)

    '        v_attrIPADDRESS = XMLDocumentMessage.CreateAttribute(gc_AtributeIPADDRESS)
    '        v_attrIPADDRESS.Value = pv_strIPADDRESS
    '        dataElement.Attributes.Append(v_attrIPADDRESS)

    '        v_attrWSNAME = XMLDocumentMessage.CreateAttribute(gc_AtributeWSNAME)
    '        v_attrWSNAME.Value = pv_strWSNAME
    '        dataElement.Attributes.Append(v_attrWSNAME)

    '        v_attrTXTYPE = XMLDocumentMessage.CreateAttribute(gc_AtributeTXTYPE)
    '        v_attrTXTYPE.Value = pv_strTXTYPE
    '        dataElement.Attributes.Append(v_attrTXTYPE)

    '        v_attrNOSUBMIT = XMLDocumentMessage.CreateAttribute(gc_AtributeNOSUBMIT)
    '        v_attrNOSUBMIT.Value = pv_strNOSUBMIT
    '        dataElement.Attributes.Append(v_attrNOSUBMIT)

    '        v_attrSTATUS = XMLDocumentMessage.CreateAttribute(gc_AtributeSTATUS)
    '        v_attrSTATUS.Value = pv_strSTATUS
    '        dataElement.Attributes.Append(v_attrSTATUS)

    '        v_attrDELTD = XMLDocumentMessage.CreateAttribute(gc_AtributeDELTD)
    '        v_attrDELTD.Value = pv_strDELTD
    '        dataElement.Attributes.Append(v_attrDELTD)

    '        v_attrDELALLOW = XMLDocumentMessage.CreateAttribute(gc_AtributeDELALLOW)
    '        v_attrDELALLOW.Value = pv_strDELALLOW
    '        dataElement.Attributes.Append(v_attrDELALLOW)

    '        v_attrOVRRQS = XMLDocumentMessage.CreateAttribute(gc_AtributeOVRRQS)
    '        v_attrOVRRQS.Value = pv_strOVRRQS
    '        dataElement.Attributes.Append(v_attrOVRRQS)

    '        v_attrUPDATEMODE = XMLDocumentMessage.CreateAttribute(gc_AtributeUPDATEMODE)
    '        v_attrUPDATEMODE.Value = pv_strUPDATEMODE
    '        dataElement.Attributes.Append(v_attrUPDATEMODE)

    '        v_attrLOCAL = XMLDocumentMessage.CreateAttribute(gc_AtributeLOCAL)
    '        v_attrLOCAL.Value = pv_strLOCAL
    '        dataElement.Attributes.Append(v_attrLOCAL)

    '        v_attrOFFID = XMLDocumentMessage.CreateAttribute(gc_AtributeOFFID)
    '        v_attrOFFID.Value = pv_strOFFID
    '        dataElement.Attributes.Append(v_attrOFFID)

    '        v_attrCHKID = XMLDocumentMessage.CreateAttribute(gc_AtributeCHKID)
    '        v_attrCHKID.Value = pv_strCHKID
    '        dataElement.Attributes.Append(v_attrCHKID)

    '        v_attrCFRID = XMLDocumentMessage.CreateAttribute(gc_AtributeCFRID)
    '        v_attrCFRID.Value = pv_strCFRID
    '        dataElement.Attributes.Append(v_attrCFRID)

    '        v_attrCHID = XMLDocumentMessage.CreateAttribute(gc_AtributeCHID)
    '        v_attrCHID.Value = pv_strCHID
    '        dataElement.Attributes.Append(v_attrCHID)

    '        v_attrIBT = XMLDocumentMessage.CreateAttribute(gc_AtributeIBT)
    '        If Len(pv_strIBT) = 0 Then pv_strIBT = "0"
    '        v_attrIBT.Value = pv_strIBT
    '        dataElement.Attributes.Append(v_attrIBT)

    '        v_attrBRID2 = XMLDocumentMessage.CreateAttribute(gc_AtributeBRID2)
    '        v_attrBRID2.Value = pv_strBRID2
    '        dataElement.Attributes.Append(v_attrBRID2)

    '        v_attrTLID2 = XMLDocumentMessage.CreateAttribute(gc_AtributeTLID2)
    '        v_attrTLID2.Value = pv_strTLID2
    '        dataElement.Attributes.Append(v_attrTLID2)

    '        v_attrTXDATE = XMLDocumentMessage.CreateAttribute(gc_AtributeTXDATE)
    '        v_attrTXDATE.Value = pv_strTXDATE
    '        dataElement.Attributes.Append(v_attrTXDATE)

    '        v_attrTXTIME = XMLDocumentMessage.CreateAttribute(gc_AtributeTXTIME)
    '        v_attrTXTIME.Value = pv_strTXTIME
    '        dataElement.Attributes.Append(v_attrTXTIME)

    '        v_attrTXNUM = XMLDocumentMessage.CreateAttribute(gc_AtributeTXNUM)
    '        v_attrTXNUM.Value = pv_strTXNUM
    '        dataElement.Attributes.Append(v_attrTXNUM)

    '        v_attrBRDATE = XMLDocumentMessage.CreateAttribute(gc_AtributeBRDATE)
    '        v_attrBRDATE.Value = pv_strBRDATE
    '        dataElement.Attributes.Append(v_attrBRDATE)

    '        v_attrBUSDATE = XMLDocumentMessage.CreateAttribute(gc_AtributeBUSDATE)
    '        v_attrBUSDATE.Value = pv_strBUSDATE
    '        dataElement.Attributes.Append(v_attrBUSDATE)

    '        v_attrCCYUSAGE = XMLDocumentMessage.CreateAttribute(gc_AtributeCCYUSAGE)
    '        v_attrCCYUSAGE.Value = pv_strCCYUSAGE
    '        dataElement.Attributes.Append(v_attrCCYUSAGE)

    '        v_attrOFFLINE = XMLDocumentMessage.CreateAttribute(gc_AtributeOFFLINE)
    '        v_attrOFFLINE.Value = pv_strOFFLINE
    '        dataElement.Attributes.Append(v_attrOFFLINE)

    '        v_attrMSGSTS = XMLDocumentMessage.CreateAttribute(gc_AtributeMSGSTS)
    '        v_attrMSGSTS.Value = pv_strMSGSTS
    '        dataElement.Attributes.Append(v_attrMSGSTS)

    '        v_attrOVRSTS = XMLDocumentMessage.CreateAttribute(gc_AtributeOVRSTS)
    '        v_attrOVRSTS.Value = pv_strOVRSTS
    '        dataElement.Attributes.Append(v_attrOVRSTS)

    '        v_attrPRETRAN = XMLDocumentMessage.CreateAttribute(gc_AtributePRETRAN)
    '        v_attrPRETRAN.Value = pv_strPRETRAN
    '        dataElement.Attributes.Append(v_attrPRETRAN)

    '        v_attrBATCHNAME = XMLDocumentMessage.CreateAttribute(gc_AtributeBATCHNAME)
    '        v_attrBATCHNAME.Value = pv_strBATCHNAME
    '        dataElement.Attributes.Append(v_attrBATCHNAME)

    '        v_attrMSGAMT = XMLDocumentMessage.CreateAttribute(gc_AtributeMSGAMT)
    '        v_attrMSGAMT.Value = ""
    '        dataElement.Attributes.Append(v_attrMSGAMT)

    '        v_attrMSGACCT = XMLDocumentMessage.CreateAttribute(gc_AtributeMSGACCT)
    '        v_attrMSGACCT.Value = ""
    '        dataElement.Attributes.Append(v_attrMSGACCT)

    '        v_attrCHKTIME = XMLDocumentMessage.CreateAttribute(gc_AtributeCHKTIME)
    '        v_attrCHKTIME.Value = ""
    '        dataElement.Attributes.Append(v_attrCHKTIME)

    '        v_attrOFFTIME = XMLDocumentMessage.CreateAttribute(gc_AtributeOFFTIME)
    '        v_attrOFFTIME.Value = ""
    '        dataElement.Attributes.Append(v_attrOFFTIME)

    '        v_attrTXDESC = XMLDocumentMessage.CreateAttribute(gc_AtributeTXDESC)
    '        v_attrTXDESC.Value = pv_strTXDESC
    '        dataElement.Attributes.Append(v_attrTXDESC)

    '        v_attrFEEAMT = XMLDocumentMessage.CreateAttribute(gc_AtributeFEEAMT)
    '        v_attrFEEAMT.Value = "0"
    '        dataElement.Attributes.Append(v_attrFEEAMT)

    '        v_attrVATAMT = XMLDocumentMessage.CreateAttribute(gc_AtributeVATAMT)
    '        v_attrVATAMT.Value = "0"
    '        dataElement.Attributes.Append(v_attrVATAMT)

    '        v_attrVOUCHER = XMLDocumentMessage.CreateAttribute(gc_AtributeVOUCHER)
    '        v_attrVATAMT.Value = ""
    '        dataElement.Attributes.Append(v_attrVOUCHER)

    '        XMLDocumentMessage.AppendChild(dataElement)
    '        Return XMLDocumentMessage.InnerXml
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Function
    Public Function BuildXMLTxMsg(Optional ByVal pv_strMSGTYPE As String = "T", _
                                    Optional ByVal pv_strLOCAL As String = "", _
                                    Optional ByVal pv_strTLTXCD As String = "", _
                                    Optional ByVal pv_strBRID As String = "", _
                                    Optional ByVal pv_strTLID As String = "", _
                                    Optional ByVal pv_strIPADDRESS As String = "", _
                                    Optional ByVal pv_strWSNAME As String = "", _
                                    Optional ByVal pv_strSTATUS As String = "0", _
                                    Optional ByVal pv_strDELTD As String = "N", _
                                    Optional ByVal pv_strOFFID As String = "", _
                                    Optional ByVal pv_strCHKID As String = "", _
                                    Optional ByVal pv_strCFRID As String = "", _
                                    Optional ByVal pv_strTXDATE As String = "", _
                                    Optional ByVal pv_strTXTIME As String = "", _
                                    Optional ByVal pv_strTXNUM As String = "", _
                                    Optional ByVal pv_strOFFLINE As String = "N", _
                                    Optional ByVal pv_strTXDESC As String = "", _
                                    Optional ByVal pv_strMSGAMT As String = "", _
                                    Optional ByVal pv_strMICODE As String = "", _
                                    Optional ByVal pv_strDELTDTXNUM As String = "", _
                                    Optional ByVal pv_strBATCHNAME As String = DAILY_TRANSACTION, _
                                    Optional ByVal pv_strLanguage As String = "VN", _
                                    Optional ByVal pv_strBUSDATE As String = "", _
                                    Optional ByVal pv_strSICODE As String = "", _
                                    Optional ByVal pv_strCHILDTLTXCD As String = "", _
                                    Optional ByVal pv_strISPARENT As String = "", _
                                    Optional ByVal pv_strPARENTID As String = "", _
                                    Optional ByVal pv_strBRCODE As String = "", _
                                    Optional ByVal pv_strTLNAME As String = "", _
                                    Optional ByVal pv_strOFFNAME As String = "", _
                                    Optional ByVal pv_strCHKNAME As String = "", _
                                    Optional ByVal pv_strCFRNAME As String = "", _
                                    Optional ByVal pv_strTXNAME As String = "", _
                                    Optional ByVal pv_strSTATUS_TEXT As String = "", _
                                    Optional ByVal pv_strPARENT_TEXT As String = "", _
                                    Optional ByVal pv_strAUTOID As String = "", _
                                    Optional ByVal pv_strTXTYPE As String = "", _
                                    Optional ByVal pv_strISBRID As String = "", _
                                    Optional ByVal pv_strCOMICODE As String = "", _
                                    Optional ByVal pv_strREASON As String = "", _
                                    Optional ByVal pv_strMISSING_WARNING As String = "0", _
                                    Optional ByVal pv_strMFNO As String = "", _
                                    Optional ByVal pv_strOLDSTATUS As String = "", _
                                    Optional ByVal pv_strTXNOTE As String = "", _
                                    Optional ByVal pv_strVSDBRID As String = "", _
                                    Optional ByVal pv_strTBLCHK As String = "", _
                                    Optional ByVal pv_strPARENT_TLTXCD As String = "", _
                                    Optional ByVal pv_strPARENT_TXNUM As String = "", _
                                    Optional ByVal pv_strPARENT_TXDATE As String = "", _
                                    Optional ByVal pv_strPARENT_BUSDATE As String = "", _
                                    Optional ByVal pv_strSignCA As String = "0", _
                                    Optional ByVal pv_strFileNameCA As String = "", _
                                    Optional ByVal pv_strSignatureCHK As String = "", _
                                    Optional ByVal pv_strStateFile As String = "") As String
        Try
            Dim XMLDocumentMessage As New Xml.XmlDocument
            Dim dataElement As Xml.XmlElement
            Dim v_attrMSGTYPE, v_attrTLTXCD, v_attrTLID, v_attrBRID, v_attrIPADDRESS, v_attrWSNAME, _
                v_attrSTATUS, v_attrDELTD, _
                v_attrLOCAL, v_attrOFFID, _
                v_attrCHKID, v_attrCFRID, v_attrTXDATE, _
                v_attrTXTIME, v_attrTXNUM, v_attrOFFLINE, _
                v_attrTXDESC, v_attrBATCHNAME, v_attrLanguage, _
                v_attrMSGAMT, v_attrMICODE, v_attrDELTDTXNUM, v_attrSICODE, v_attrBUSDATE, _
                v_attrCHILDTLTXCD, v_attrISPARENT, v_attrPARENTID, v_attrBRCODE, _
                v_attrTLNAME, v_attrCHKNAME, v_attrOFFNAME, v_attrCFRNAME, v_attrTXNAME, v_attrSTATUS_TEXT, v_attrMFNO, _
                v_attrPARENT_TEXT, v_attrAUTOID, v_attrTXTYPE, v_attrOLDSTATUS, _
                v_attrISBRID, v_attrCOMICODE, v_attrREASON, v_attrMISSING_WARNING, v_attrTXNOTE, _
                v_attrVSDBRID, v_attrTBLCHK, v_attrPARENT_TLTXCD, v_attrPARENT_TXNUM, v_attrPARENT_TXDATE, _
                v_attrPARENT_BUSDATE, v_attrSignCA, v_attrFileNameCA, v_attrSignatureCHK, v_attrStateFile As Xml.XmlAttribute

            dataElement = XMLDocumentMessage.CreateElement("TransactMessage")
            '1
            v_attrMSGTYPE = XMLDocumentMessage.CreateAttribute(gc_AtributeMSGTYPE)
            v_attrMSGTYPE.Value = pv_strMSGTYPE
            dataElement.Attributes.Append(v_attrMSGTYPE)
            '2
            v_attrLOCAL = XMLDocumentMessage.CreateAttribute(gc_AtributeLOCAL)
            v_attrLOCAL.Value = pv_strLOCAL
            dataElement.Attributes.Append(v_attrLOCAL)
            '3
            v_attrTLTXCD = XMLDocumentMessage.CreateAttribute(gc_AtributeTLTXCD)
            v_attrTLTXCD.Value = pv_strTLTXCD
            dataElement.Attributes.Append(v_attrTLTXCD)
            '4
            v_attrBRID = XMLDocumentMessage.CreateAttribute(gc_AtributeBRID)
            v_attrBRID.Value = pv_strBRID
            dataElement.Attributes.Append(v_attrBRID)
            '5
            v_attrTLID = XMLDocumentMessage.CreateAttribute(gc_AtributeTLID)
            v_attrTLID.Value = pv_strTLID
            dataElement.Attributes.Append(v_attrTLID)
            '6
            v_attrIPADDRESS = XMLDocumentMessage.CreateAttribute(gc_AtributeIPADDRESS)
            v_attrIPADDRESS.Value = pv_strIPADDRESS
            dataElement.Attributes.Append(v_attrIPADDRESS)
            '7
            v_attrWSNAME = XMLDocumentMessage.CreateAttribute(gc_AtributeWSNAME)
            v_attrWSNAME.Value = pv_strWSNAME
            dataElement.Attributes.Append(v_attrWSNAME)
            '8
            v_attrSTATUS = XMLDocumentMessage.CreateAttribute(gc_AtributeSTATUS)
            v_attrSTATUS.Value = pv_strSTATUS
            dataElement.Attributes.Append(v_attrSTATUS)
            '9
            v_attrDELTD = XMLDocumentMessage.CreateAttribute(gc_AtributeDELTD)
            v_attrDELTD.Value = pv_strDELTD
            dataElement.Attributes.Append(v_attrDELTD)
            '10
            v_attrCHKID = XMLDocumentMessage.CreateAttribute(gc_AtributeCHKID)
            v_attrCHKID.Value = pv_strCHKID
            dataElement.Attributes.Append(v_attrCHKID)
            '11
            v_attrOFFID = XMLDocumentMessage.CreateAttribute(gc_AtributeOFFID)
            v_attrOFFID.Value = pv_strOFFID
            dataElement.Attributes.Append(v_attrOFFID)
            '12
            v_attrCFRID = XMLDocumentMessage.CreateAttribute(gc_AtributeCFRID)
            v_attrCFRID.Value = pv_strCFRID
            dataElement.Attributes.Append(v_attrCFRID)
            '13
            v_attrTXDATE = XMLDocumentMessage.CreateAttribute(gc_AtributeTXDATE)
            v_attrTXDATE.Value = pv_strTXDATE
            dataElement.Attributes.Append(v_attrTXDATE)
            '14
            v_attrTXTIME = XMLDocumentMessage.CreateAttribute(gc_AtributeTXTIME)
            v_attrTXTIME.Value = pv_strTXTIME
            dataElement.Attributes.Append(v_attrTXTIME)
            '15
            v_attrTXNUM = XMLDocumentMessage.CreateAttribute(gc_AtributeTXNUM)
            v_attrTXNUM.Value = pv_strTXNUM
            dataElement.Attributes.Append(v_attrTXNUM)
            '16
            v_attrOFFLINE = XMLDocumentMessage.CreateAttribute(gc_AtributeOFFLINE)
            v_attrOFFLINE.Value = pv_strOFFLINE
            dataElement.Attributes.Append(v_attrOFFLINE)
            '17
            v_attrTXDESC = XMLDocumentMessage.CreateAttribute(gc_AtributeTXDESC)
            v_attrTXDESC.Value = pv_strTXDESC
            dataElement.Attributes.Append(v_attrTXDESC)
            '18 
            v_attrMSGAMT = XMLDocumentMessage.CreateAttribute(gc_AtributeMSGAMT)
            v_attrMSGAMT.Value = pv_strMSGAMT
            dataElement.Attributes.Append(v_attrMSGAMT)
            '19
            v_attrMICODE = XMLDocumentMessage.CreateAttribute(gc_AtributeMICODE)
            v_attrMICODE.Value = pv_strMICODE
            dataElement.Attributes.Append(v_attrMICODE)
            '20
            v_attrDELTDTXNUM = XMLDocumentMessage.CreateAttribute(gc_AtributeDELTDTXNUM)
            v_attrDELTDTXNUM.Value = pv_strDELTDTXNUM
            dataElement.Attributes.Append(v_attrDELTDTXNUM)
            '21
            v_attrBATCHNAME = XMLDocumentMessage.CreateAttribute(gc_AtributeBATCHNAME)
            v_attrBATCHNAME.Value = pv_strBATCHNAME
            dataElement.Attributes.Append(v_attrBATCHNAME)
            '22
            v_attrLanguage = XMLDocumentMessage.CreateAttribute(gc_AtributeLANGUAGE)
            v_attrLanguage.Value = pv_strLanguage
            dataElement.Attributes.Append(v_attrLanguage)
            '23
            v_attrBUSDATE = XMLDocumentMessage.CreateAttribute(gc_AtributeBUSDATE)
            v_attrBUSDATE.Value = pv_strBUSDATE
            dataElement.Attributes.Append(v_attrBUSDATE)
            '24
            v_attrSICODE = XMLDocumentMessage.CreateAttribute(gc_AtributeSICODE)
            v_attrSICODE.Value = pv_strSICODE
            dataElement.Attributes.Append(v_attrSICODE)
            '25
            v_attrCHILDTLTXCD = XMLDocumentMessage.CreateAttribute(gc_AtributeCHILDTLTXCD)
            v_attrCHILDTLTXCD.Value = pv_strCHILDTLTXCD
            dataElement.Attributes.Append(v_attrCHILDTLTXCD)
            '26
            v_attrISPARENT = XMLDocumentMessage.CreateAttribute(gc_AtributeISPARENT)
            v_attrISPARENT.Value = pv_strISPARENT
            dataElement.Attributes.Append(v_attrISPARENT)
            '27
            v_attrPARENTID = XMLDocumentMessage.CreateAttribute(gc_AtributePARENTID)
            v_attrPARENTID.Value = pv_strPARENTID
            dataElement.Attributes.Append(v_attrPARENTID)
            '28
            v_attrBRCODE = XMLDocumentMessage.CreateAttribute(gc_AtributeBRCODE)
            v_attrBRCODE.Value = pv_strBRCODE
            dataElement.Attributes.Append(v_attrBRCODE)
            '29
            v_attrTLNAME = XMLDocumentMessage.CreateAttribute(gc_AtributeTLNAME)
            v_attrTLNAME.Value = pv_strTLNAME
            dataElement.Attributes.Append(v_attrTLNAME)
            '30
            v_attrCHKNAME = XMLDocumentMessage.CreateAttribute(gc_AtributeCHKNAME)
            v_attrCHKNAME.Value = pv_strCHKNAME
            dataElement.Attributes.Append(v_attrCHKNAME)
            '31
            v_attrTXNAME = XMLDocumentMessage.CreateAttribute(gc_AtributeTXNAME)
            v_attrTXNAME.Value = pv_strTXNAME
            dataElement.Attributes.Append(v_attrTXNAME)
            '32
            v_attrSTATUS_TEXT = XMLDocumentMessage.CreateAttribute(gc_AtributeSTATUSTEXT)
            v_attrSTATUS_TEXT.Value = pv_strSTATUS_TEXT
            dataElement.Attributes.Append(v_attrSTATUS_TEXT)
            '33
            v_attrPARENT_TEXT = XMLDocumentMessage.CreateAttribute(gc_AtributePARENT_TEXT)
            v_attrPARENT_TEXT.Value = pv_strPARENT_TEXT
            dataElement.Attributes.Append(v_attrPARENT_TEXT)
            '34
            v_attrCFRNAME = XMLDocumentMessage.CreateAttribute(gc_AtributeCFRNAME)
            v_attrCFRNAME.Value = pv_strCFRNAME
            dataElement.Attributes.Append(v_attrCFRNAME)

            '35
            v_attrAUTOID = XMLDocumentMessage.CreateAttribute(gc_AtributeAUTOID)
            v_attrAUTOID.Value = pv_strAUTOID
            dataElement.Attributes.Append(v_attrAUTOID)



            '36
            v_attrOFFNAME = XMLDocumentMessage.CreateAttribute(gc_AtributeOFFNAME)
            v_attrOFFNAME.Value = pv_strOFFNAME
            dataElement.Attributes.Append(v_attrOFFNAME)


            '37
            v_attrTXTYPE = XMLDocumentMessage.CreateAttribute(gc_AtributeTXTYPE)
            v_attrTXTYPE.Value = pv_strTXTYPE
            dataElement.Attributes.Append(v_attrTXTYPE)
            ' 38
            v_attrISBRID = XMLDocumentMessage.CreateAttribute(gc_AtributeISBRID)
            v_attrISBRID.Value = pv_strISBRID
            dataElement.Attributes.Append(v_attrISBRID)
            ' 39 
            v_attrCOMICODE = XMLDocumentMessage.CreateAttribute(gc_AtributeCOMICODE)
            v_attrCOMICODE.Value = pv_strCOMICODE
            dataElement.Attributes.Append(v_attrCOMICODE)
            ' 40
            v_attrREASON = XMLDocumentMessage.CreateAttribute(gc_AtributeREASON)
            v_attrREASON.Value = pv_strREASON
            dataElement.Attributes.Append(v_attrREASON)
            '41
            v_attrMISSING_WARNING = XMLDocumentMessage.CreateAttribute(gc_AtributeMISSING_WARNING)
            v_attrMISSING_WARNING.Value = pv_strMISSING_WARNING
            dataElement.Attributes.Append(v_attrMISSING_WARNING)
            '42
            v_attrOLDSTATUS = XMLDocumentMessage.CreateAttribute(gc_AtributeOLDSTATUS)
            v_attrOLDSTATUS.Value = pv_strOLDSTATUS
            dataElement.Attributes.Append(v_attrOLDSTATUS)
            '43
            v_attrTXNOTE = XMLDocumentMessage.CreateAttribute(gc_AtributeTXNOTE)
            v_attrTXNOTE.Value = pv_strTXNOTE
            dataElement.Attributes.Append(v_attrTXNOTE)
            '44
            v_attrVSDBRID = XMLDocumentMessage.CreateAttribute(gc_AtributeVSDBRID)
            v_attrVSDBRID.Value = pv_strVSDBRID
            dataElement.Attributes.Append(v_attrVSDBRID)

            '45
            v_attrTBLCHK = XMLDocumentMessage.CreateAttribute(gc_AtributeTBLCHK)
            v_attrTBLCHK.Value = pv_strTBLCHK
            dataElement.Attributes.Append(v_attrTBLCHK)
            '46
            v_attrPARENT_TLTXCD = XMLDocumentMessage.CreateAttribute(gc_AtributePARENT_TLTXCD)
            v_attrPARENT_TLTXCD.Value = pv_strPARENT_TLTXCD
            dataElement.Attributes.Append(v_attrPARENT_TLTXCD)
            '47
            v_attrPARENT_TXNUM = XMLDocumentMessage.CreateAttribute(gc_AtributePARENT_TXNUM)
            v_attrPARENT_TXNUM.Value = pv_strPARENT_TXNUM
            dataElement.Attributes.Append(v_attrPARENT_TXNUM)
            '48
            v_attrPARENT_TXDATE = XMLDocumentMessage.CreateAttribute(gc_AtributePARENT_TXDATE)
            v_attrPARENT_TXDATE.Value = pv_strPARENT_TXDATE
            dataElement.Attributes.Append(v_attrPARENT_TXDATE)
            '49
            v_attrPARENT_BUSDATE = XMLDocumentMessage.CreateAttribute(gc_AtributePARENT_BUSDATE)
            v_attrPARENT_BUSDATE.Value = pv_strPARENT_BUSDATE
            dataElement.Attributes.Append(v_attrPARENT_BUSDATE)
            'bangpv: Thêm thuộc tính xác định giao dịch có được ký chữ ký số hay không 
            '50
            ' Public Const gc_AtributeFileNameCACHK = "FILENAMECACHK"
            'Public Const gc_AtributeSignatureCACHK = "SignatureCACHK"
            v_attrSignCA = XMLDocumentMessage.CreateAttribute(gc_AtributeSIGNCA)
            v_attrSignCA.Value = pv_strSignCA
            dataElement.Attributes.Append(v_attrSignCA)
            '51
            v_attrFileNameCA = XMLDocumentMessage.CreateAttribute(gc_AtributeFileNameCACHK)
            v_attrFileNameCA.Value = pv_strFileNameCA
            dataElement.Attributes.Append(v_attrFileNameCA)
            '52
            v_attrSignatureCHK = XMLDocumentMessage.CreateAttribute(gc_AtributeSignatureCACHK)
            v_attrSignatureCHK.Value = pv_strSignatureCHK
            dataElement.Attributes.Append(v_attrSignatureCHK)
            'end bangpv
            v_attrStateFile = XMLDocumentMessage.CreateAttribute(gc_AtributeStateFile)
            v_attrStateFile.Value = pv_strStateFile
            dataElement.Attributes.Append(v_attrStateFile)
            'tuanta

            'end tuanta
            XMLDocumentMessage.AppendChild(dataElement)
            Return XMLDocumentMessage.InnerXml
        Catch ex As Exception
            Throw ex
        End Try
    End Function


    'Public Function BuildXMLRptMsg(Optional ByVal pv_strLocal As String = "", _
    '                               Optional ByVal pv_strStoreProc As String = "", _
    '                               Optional ByVal pv_strMsgType As String = "", _
    '                               Optional ByVal pv_arrRptParam() As ReportParameters = Nothing, _
    '                               Optional ByVal pv_intNumOfParam As Integer = 0) As String

    '    Try
    '        Dim XMLDocumentMessage As New XmlDocumentEx
    '        Dim dataElement As Xml.XmlElement
    '        Dim v_attrLocal As Xml.XmlAttribute, v_attrStoreProc As Xml.XmlAttribute
    '        Dim v_attrName As Xml.XmlAttribute, v_attrSize As Xml.XmlAttribute, v_attrType As Xml.XmlAttribute, v_attrValue As Xml.XmlAttribute
    '        Dim v_attrNumOfParam As Xml.XmlAttribute, v_attrMsgType As Xml.XmlAttribute

    '        dataElement = XMLDocumentMessage.CreateElement("ObjectMessage")

    '        If Len(pv_strLocal) > 0 Then
    '            v_attrLocal = XMLDocumentMessage.CreateAttribute(gc_AtributeLOCAL)
    '            v_attrLocal.Value = pv_strLocal
    '            dataElement.Attributes.Append(v_attrLocal)
    '        End If
    '        If Len(pv_strStoreProc) > 0 Then
    '            v_attrStoreProc = XMLDocumentMessage.CreateAttribute(gc_AtributeSTOREPROC)
    '            v_attrStoreProc.Value = pv_strStoreProc
    '            dataElement.Attributes.Append(v_attrStoreProc)
    '        End If
    '        If Len(pv_strMsgType) > 0 Then
    '            v_attrMsgType = XMLDocumentMessage.CreateAttribute(gc_AtributeMSGTYPE)
    '            v_attrMsgType.Value = pv_strMsgType
    '            dataElement.Attributes.Append(v_attrMsgType)
    '        End If
    '        Dim i As Integer
    '        For i = 0 To pv_arrRptParam.Length - 1
    '            If Len(pv_arrRptParam(i).ParamName) > 0 Then
    '                v_attrName = XMLDocumentMessage.CreateAttribute(gc_AtributePARAM_NAME & i.ToString)
    '                v_attrName.Value = pv_arrRptParam(i).ParamName
    '                dataElement.Attributes.Append(v_attrName)
    '            End If
    '            If Len(pv_arrRptParam(i).ParamValue) > 0 Then
    '                v_attrValue = XMLDocumentMessage.CreateAttribute(gc_AtributePARAM_VALUE & i.ToString)
    '                v_attrValue.Value = pv_arrRptParam(i).ParamValue
    '                dataElement.Attributes.Append(v_attrValue)
    '            End If
    '            If Len(pv_arrRptParam(i).ParamSize) > 0 Then
    '                v_attrSize = XMLDocumentMessage.CreateAttribute(gc_AtributePARAM_SIZE & i.ToString)
    '                v_attrSize.Value = pv_arrRptParam(i).ParamSize
    '                dataElement.Attributes.Append(v_attrSize)
    '            End If
    '            If Len(pv_arrRptParam(i).ParamType) > 0 Then
    '                v_attrType = XMLDocumentMessage.CreateAttribute(gc_AtributePARAM_TYPE & i.ToString)
    '                v_attrType.Value = pv_arrRptParam(i).ParamType
    '                dataElement.Attributes.Append(v_attrType)
    '            End If
    '        Next

    '        If Len(pv_intNumOfParam) > 0 Then
    '            v_attrNumOfParam = XMLDocumentMessage.CreateAttribute(gc_AtributeNUM_OF_PARAM)
    '            v_attrNumOfParam.Value = pv_intNumOfParam
    '            dataElement.Attributes.Append(v_attrNumOfParam)
    '        End If

    '        XMLDocumentMessage.AppendChild(dataElement)
    '        Return XMLDocumentMessage.InnerXml
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Function

    'Edited by Thanglv9 - 13/12/2012 - Them Ipaddress,wsname,tabtext
    Public Function BuildXMLObjMsg(Optional ByVal pv_strTxDate As String = "", _
                                   Optional ByVal pv_strBranchId As String = "", _
                                   Optional ByVal pv_strTxTime As String = "", _
                                   Optional ByVal pv_strTellerId As String = "", _
                                   Optional ByVal pv_strLocal As String = "", _
                                   Optional ByVal pv_strMsgType As String = "", _
                                   Optional ByVal pv_strObjName As String = "", _
                                   Optional ByVal pv_strActionFlag As String = "", _
                                   Optional ByVal pv_strCmdInquiry As String = "", _
                                   Optional ByVal pv_strClause As String = "", _
                                   Optional ByVal pv_strFuncName As String = "", _
                                   Optional ByVal pv_strAutoId As String = "", _
                                   Optional ByVal pv_strTxNum As String = "", _
                                   Optional ByVal pv_strReference As String = "", _
                                    Optional ByVal pv_strLanguage As String = "VN", _
                                    Optional ByVal pv_strVsdBrid As String = "", _
                                    Optional ByVal pv_strVsdBrid2 As String = "", _
                                    Optional ByVal pv_strSignCA As String = "", _
                                    Optional ByVal pv_strTLName As String = "", _
                                    Optional ByVal pv_strIPADD As String = "", _
                                    Optional ByVal pv_strWSNAME As String = "", _
                                    Optional ByVal pv_strTXNAME As String = "", _
                                    Optional ByVal pv_strBRCODE As String = "", _
                                    Optional ByVal pv_strBusDate As String = "", _
                                    Optional ByVal pv_strFileNameNHNN As String = "") As String
        Try
            Dim XMLDocumentMessage As New Xml.XmlDocument
            Dim dataElement As Xml.XmlElement
            Dim v_attrTxDate As Xml.XmlAttribute, v_attrTxTime As Xml.XmlAttribute, v_attrTLID As Xml.XmlAttribute, v_attrBRID As Xml.XmlAttribute
            Dim v_attrLocal As Xml.XmlAttribute, v_attrMsgType As Xml.XmlAttribute, v_attrObjName As Xml.XmlAttribute, v_attrActFlag As Xml.XmlAttribute
            Dim v_attrCmdInquiry As Xml.XmlAttribute, v_attrClause As Xml.XmlAttribute, v_attrFuncName As Xml.XmlAttribute, v_attrTxNum As Xml.XmlAttribute
            Dim v_attrAutoId As Xml.XmlAttribute, v_attrReference, v_attrLanguage, v_attrVsdBrid, v_attrVsdBrid2, v_attrSignCA, v_attrTLName As Xml.XmlAttribute
            'Added by Thanglv9
            Dim v_attrIPADD As Xml.XmlAttribute, v_attrWSNAME As Xml.XmlAttribute, v_attrTXNAME As Xml.XmlAttribute, v_attrBRCODE As Xml.XmlAttribute, v_attrBusDate, v_attrFileNameNHNN As Xml.XmlAttribute
            'End
            dataElement = XMLDocumentMessage.CreateElement("ObjectMessage")

            If Len(pv_strTxDate) > 0 Then
                v_attrTxDate = XMLDocumentMessage.CreateAttribute(gc_AtributeTXDATE)
                v_attrTxDate.Value = pv_strTxDate
                dataElement.Attributes.Append(v_attrTxDate)
            End If

            If Len(pv_strTxNum) > 0 Then
                v_attrTxNum = XMLDocumentMessage.CreateAttribute(gc_AtributeTXNUM)
                v_attrTxNum.Value = pv_strTxNum
                dataElement.Attributes.Append(v_attrTxNum)
            End If

            If Len(pv_strTxTime) > 0 Then
                v_attrTxTime = XMLDocumentMessage.CreateAttribute(gc_AtributeTXTIME)
                v_attrTxTime.Value = pv_strTxTime
                dataElement.Attributes.Append(v_attrTxTime)
            End If

            If Len(pv_strTellerId) > 0 Then
                v_attrTLID = XMLDocumentMessage.CreateAttribute(gc_AtributeTLID)
                v_attrTLID.Value = pv_strTellerId
                dataElement.Attributes.Append(v_attrTLID)
            End If

            If Len(pv_strBranchId) > 0 Then
                v_attrBRID = XMLDocumentMessage.CreateAttribute(gc_AtributeBRID)
                v_attrBRID.Value = pv_strBranchId
                dataElement.Attributes.Append(v_attrBRID)
            End If

            If Len(pv_strLocal) > 0 Then
                v_attrLocal = XMLDocumentMessage.CreateAttribute(gc_AtributeLOCAL)
                v_attrLocal.Value = pv_strLocal
                dataElement.Attributes.Append(v_attrLocal)
            End If

            If Len(pv_strMsgType) > 0 Then
                v_attrMsgType = XMLDocumentMessage.CreateAttribute(gc_AtributeMSGTYPE)
                v_attrMsgType.Value = pv_strMsgType
                dataElement.Attributes.Append(v_attrMsgType)
            End If

            If Len(pv_strObjName) > 0 Then
                v_attrObjName = XMLDocumentMessage.CreateAttribute(gc_AtributeOBJNAME)
                v_attrObjName.Value = pv_strObjName
                dataElement.Attributes.Append(v_attrObjName)
            End If

            If Len(pv_strActionFlag) > 0 Then
                v_attrActFlag = XMLDocumentMessage.CreateAttribute(gc_AtributeACTFLAG)
                v_attrActFlag.Value = pv_strActionFlag
                dataElement.Attributes.Append(v_attrActFlag)
            End If

            If Len(pv_strCmdInquiry) > 0 Then
                v_attrCmdInquiry = XMLDocumentMessage.CreateAttribute(gc_AtributeCMDINQUIRY)
                v_attrCmdInquiry.Value = pv_strCmdInquiry
                dataElement.Attributes.Append(v_attrCmdInquiry)
            End If

            If Len(pv_strClause) > 0 Then
                v_attrClause = XMLDocumentMessage.CreateAttribute(gc_AtributeCLAUSE)
                v_attrClause.Value = pv_strClause
                dataElement.Attributes.Append(v_attrClause)
            End If

            If Len(pv_strFuncName) > 0 Then
                v_attrFuncName = XMLDocumentMessage.CreateAttribute(gc_AtributeFUNCNAME)
                v_attrFuncName.Value = pv_strFuncName
                dataElement.Attributes.Append(v_attrFuncName)
            End If

            If Len(pv_strAutoId) > 0 Then
                v_attrAutoId = XMLDocumentMessage.CreateAttribute(gc_AtributeAUTOID)
                v_attrAutoId.Value = pv_strAutoId
                dataElement.Attributes.Append(v_attrAutoId)
            End If

            If Len(pv_strReference) > 0 Then
                v_attrReference = XMLDocumentMessage.CreateAttribute(gc_AtributeREFERENCE)
                v_attrReference.Value = pv_strReference
                dataElement.Attributes.Append(v_attrReference)
            End If

            v_attrLanguage = XMLDocumentMessage.CreateAttribute(gc_AtributeLANGUAGE)
            v_attrLanguage.Value = pv_strLanguage
            dataElement.Attributes.Append(v_attrLanguage)
            'bangpv CA
            v_attrSignCA = XMLDocumentMessage.CreateAttribute(gc_AtributeSIGNCA)
            v_attrSignCA.Value = pv_strSignCA
            dataElement.Attributes.Append(v_attrSignCA)

            v_attrTLName = XMLDocumentMessage.CreateAttribute(gc_AtributeTLNAME)
            v_attrTLName.Value = pv_strTLName
            dataElement.Attributes.Append(v_attrTLName)
            'end bangpv

            'Added by Thanglv9 - 13/12/2012
            v_attrIPADD = XMLDocumentMessage.CreateAttribute(gc_AtributeIPADDRESS)
            v_attrIPADD.Value = pv_strIPADD
            dataElement.Attributes.Append(v_attrIPADD)

            v_attrWSNAME = XMLDocumentMessage.CreateAttribute(gc_AtributeWSNAME)
            v_attrWSNAME.Value = pv_strWSNAME
            dataElement.Attributes.Append(v_attrWSNAME)

            v_attrTXNAME = XMLDocumentMessage.CreateAttribute(gc_AtributeTXNAME)
            v_attrTXNAME.Value = pv_strTXNAME
            dataElement.Attributes.Append(v_attrTXNAME)

            v_attrBRCODE = XMLDocumentMessage.CreateAttribute(gc_AtributeBRCODE)
            v_attrBRCODE.Value = pv_strBRCODE
            dataElement.Attributes.Append(v_attrBRCODE)

            v_attrBusDate = XMLDocumentMessage.CreateAttribute(gc_AtributeBUSDATE)
            v_attrBusDate.Value = pv_strBusDate
            dataElement.Attributes.Append(v_attrBusDate)
            'End
            If Len(pv_strVsdBrid) > 0 Then
                v_attrVsdBrid = XMLDocumentMessage.CreateAttribute(gc_AtributeVSDBRID)
                v_attrVsdBrid.Value = pv_strVsdBrid
                dataElement.Attributes.Append(v_attrVsdBrid)
            End If

            If Len(pv_strVsdBrid2) > 0 Then
                v_attrVsdBrid2 = XMLDocumentMessage.CreateAttribute(gc_AtributeVSDBRID2)
                v_attrVsdBrid2.Value = pv_strVsdBrid2
                dataElement.Attributes.Append(v_attrVsdBrid2)
            End If
            'Added by Thanglv9 - 24/09/2013
            If Len(pv_strFileNameNHNN) > 0 Then
                v_attrFileNameNHNN = XMLDocumentMessage.CreateAttribute(gc_AtributeFileName)
                v_attrFileNameNHNN.Value = pv_strFileNameNHNN
                dataElement.Attributes.Append(v_attrFileNameNHNN)
            End If
            'end thanglv

            XMLDocumentMessage.AppendChild(dataElement)
            Return XMLDocumentMessage.InnerXml
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function BuildXMLObjData(ByVal pv_ds As DataSet, _
                                    ByRef pv_strObjectMessage As String, _
                                    Optional ByVal pv_dsOldInput As DataSet = Nothing, _
                                    Optional ByVal pv_intFlag As Integer = 1) As Long
        Dim v_XmlDocument As New Xml.XmlDocument
        Dim dataElement As Xml.XmlElement
        Dim entryNode As Xml.XmlNode
        Dim v_attrFLDNAME, v_attrFLDTYPE, v_attrOLDVAL As Xml.XmlAttribute


        v_XmlDocument.LoadXml(pv_strObjectMessage)

        Try
            If pv_intFlag = ExecuteFlag.AddNew Then
                If pv_ds.Tables(0).Rows.Count > 0 Then
                    For v_int As Integer = 0 To pv_ds.Tables(0).Rows.Count - 1
                        dataElement = v_XmlDocument.CreateElement(Xml.XmlNodeType.Element, "ObjData", "")

                        For v_intColumn As Integer = 0 To pv_ds.Tables(0).Columns.Count - 1
                            entryNode = v_XmlDocument.CreateNode(Xml.XmlNodeType.Element, "Entry", "")

                            'Add field name
                            v_attrFLDNAME = v_XmlDocument.CreateAttribute("fldname")
                            v_attrFLDNAME.Value = pv_ds.Tables(0).Columns(v_intColumn).ColumnName
                            entryNode.Attributes.Append(v_attrFLDNAME)

                            'Add field type
                            v_attrFLDTYPE = v_XmlDocument.CreateAttribute("fldtype")
                            v_attrFLDTYPE.Value = pv_ds.Tables(0).Columns(v_intColumn).DataType.ToString
                            entryNode.Attributes.Append(v_attrFLDTYPE)

                            'Add current value
                            v_attrOLDVAL = v_XmlDocument.CreateAttribute("oldval")
                            If Not IsDBNull(pv_ds.Tables(0).Rows(v_int)(v_intColumn)) Then
                                If pv_ds.Tables(0).Rows(v_int)(v_intColumn).GetType.Name = GetType(System.DateTime).Name Then
                                    v_attrOLDVAL.Value = Format(pv_ds.Tables(0).Rows(v_int)(v_intColumn), gc_FORMAT_DATE)
                                Else
                                    v_attrOLDVAL.Value = CStr(pv_ds.Tables(0).Rows(v_int)(v_intColumn))
                                End If
                            End If
                            entryNode.Attributes.Append(v_attrOLDVAL)

                            'Set value
                            If Not IsDBNull(pv_ds.Tables(0).Rows(v_int)(v_intColumn)) Then
                                If pv_ds.Tables(0).Rows(v_int)(v_intColumn).GetType.Name = GetType(System.DateTime).Name Then
                                    entryNode.InnerText = Format(pv_ds.Tables(0).Rows(v_int)(v_intColumn), gc_FORMAT_DATE)
                                Else
                                    entryNode.InnerText = CStr(pv_ds.Tables(0).Rows(v_int)(v_intColumn))
                                End If
                            End If

                            dataElement.AppendChild(entryNode)
                        Next v_intColumn

                        v_XmlDocument.DocumentElement.AppendChild(dataElement)
                    Next v_int
                End If
            ElseIf pv_intFlag = ExecuteFlag.Edit Then
                If pv_dsOldInput.Tables(0).Rows.Count > 0 Then
                    For v_int As Integer = 0 To pv_dsOldInput.Tables(0).Rows.Count - 1
                        dataElement = v_XmlDocument.CreateElement(Xml.XmlNodeType.Element, "ObjData", "")

                        For v_intColumn As Integer = 0 To pv_dsOldInput.Tables(0).Columns.Count - 1
                            entryNode = v_XmlDocument.CreateNode(Xml.XmlNodeType.Element, "Entry", "")

                            'Add field name
                            v_attrFLDNAME = v_XmlDocument.CreateAttribute("fldname")
                            v_attrFLDNAME.Value = pv_dsOldInput.Tables(0).Columns(v_intColumn).ColumnName
                            entryNode.Attributes.Append(v_attrFLDNAME)

                            'Add field type
                            v_attrFLDTYPE = v_XmlDocument.CreateAttribute("fldtype")
                            v_attrFLDTYPE.Value = pv_dsOldInput.Tables(0).Columns(v_intColumn).DataType.ToString
                            entryNode.Attributes.Append(v_attrFLDTYPE)

                            'Add current value
                            v_attrOLDVAL = v_XmlDocument.CreateAttribute("oldval")
                            'Edited by Thanglv9 - 16/12/2012
                            If Not IsDBNull(pv_dsOldInput.Tables(0).Rows(v_int)(v_intColumn)) Then
                                If pv_dsOldInput.Tables(0).Rows(v_int)(v_intColumn).GetType.Name = GetType(System.DateTime).Name Then
                                    v_attrOLDVAL.Value = Format(pv_dsOldInput.Tables(0).Rows(v_int)(v_intColumn), gc_FORMAT_DATE)
                                Else
                                    v_attrOLDVAL.Value = CStr(pv_dsOldInput.Tables(0).Rows(v_int)(v_intColumn))
                                End If
                            End If
                            'End
                            entryNode.Attributes.Append(v_attrOLDVAL)

                            'Set value
                            If Not IsDBNull(pv_ds.Tables(0).Rows(v_int)(v_intColumn)) Then
                                If pv_ds.Tables(0).Rows(v_int)(v_intColumn).GetType.Name = GetType(System.DateTime).Name Then
                                    entryNode.InnerText = Format(pv_ds.Tables(0).Rows(v_int)(v_intColumn), gc_FORMAT_DATE)
                                Else
                                    entryNode.InnerText = CStr(pv_ds.Tables(0).Rows(v_int)(v_intColumn))
                                End If
                            End If



                            dataElement.AppendChild(entryNode)
                        Next v_intColumn

                        v_XmlDocument.DocumentElement.AppendChild(dataElement)
                    Next v_int
                End If
            Else
                If pv_ds.Tables(0).Rows.Count > 0 Then
                    For v_intRow As Integer = 0 To pv_ds.Tables(0).Rows.Count - 1
                        dataElement = v_XmlDocument.CreateElement(Xml.XmlNodeType.Element, "ObjData", "")

                        For v_intColumn As Integer = 0 To pv_ds.Tables(0).Columns.Count - 1
                            entryNode = v_XmlDocument.CreateNode(Xml.XmlNodeType.Element, "Entry", "")

                            'Add field name
                            v_attrFLDNAME = v_XmlDocument.CreateAttribute("fldname")
                            v_attrFLDNAME.Value = pv_ds.Tables(0).Columns(v_intColumn).ColumnName
                            entryNode.Attributes.Append(v_attrFLDNAME)

                            'Add field type
                            v_attrFLDTYPE = v_XmlDocument.CreateAttribute("fldtype")
                            v_attrFLDTYPE.Value = pv_ds.Tables(0).Columns(v_intColumn).DataType.ToString
                            entryNode.Attributes.Append(v_attrFLDTYPE)

                            'Add current value
                            v_attrOLDVAL = v_XmlDocument.CreateAttribute("oldval")
                            If Not IsDBNull(pv_ds.Tables(0).Rows(v_intRow)(v_intColumn)) Then
                                v_attrOLDVAL.Value = CStr(pv_ds.Tables(0).Rows(v_intRow)(v_intColumn))
                            End If
                            entryNode.Attributes.Append(v_attrOLDVAL)

                            'Set value
                            If pv_ds.Tables(0).Rows(v_intRow)(v_intColumn).GetType.Name = GetType(System.DateTime).Name Then
                                entryNode.InnerText = Format(pv_ds.Tables(0).Rows(v_intRow)(v_intColumn), gc_FORMAT_DATE)
                            Else
                                entryNode.InnerText = CStr(pv_ds.Tables(0).Rows(v_intRow)(v_intColumn))
                            End If
                            'If Not IsDBNull(pv_ds.Tables(0).Rows(v_intRow)(v_intColumn)) Then
                            '    entryNode.InnerText = CStr(pv_ds.Tables(0).Rows(v_intRow)(v_intColumn))
                            'End If

                            dataElement.AppendChild(entryNode)
                        Next

                        v_XmlDocument.DocumentElement.AppendChild(dataElement)
                    Next v_intRow
                End If
            End If

            pv_strObjectMessage = v_XmlDocument.InnerXml
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function BuildXMLRptData(ByVal pv_ds As DataSet, _
                                    ByRef pv_strObjectMessage As String, _
                                    Optional ByVal pv_dsOldInput As DataSet = Nothing, _
                                    Optional ByVal pv_strElement As String = "ObjData") As Long
        Dim v_XmlDocument As New Xml.XmlDocument
        Dim dataElement As Xml.XmlElement
        Dim entryNode As Xml.XmlNode
        Dim v_attrFLDNAME, v_attrFLDTYPE, v_attrOLDVAL As Xml.XmlAttribute


        v_XmlDocument.LoadXml(pv_strObjectMessage)

        Try
            If pv_ds.Tables(0).Rows.Count > 0 Then
                For v_int As Integer = 0 To pv_ds.Tables(0).Rows.Count - 1
                    dataElement = v_XmlDocument.CreateElement(Xml.XmlNodeType.Element, pv_strElement, "")

                    For v_intColumn As Integer = 0 To pv_ds.Tables(0).Columns.Count - 1
                        entryNode = v_XmlDocument.CreateNode(Xml.XmlNodeType.Element, "Entry", "")

                        'Add field name
                        v_attrFLDNAME = v_XmlDocument.CreateAttribute("fldname")
                        v_attrFLDNAME.Value = pv_ds.Tables(0).Columns(v_intColumn).ColumnName
                        entryNode.Attributes.Append(v_attrFLDNAME)

                        'Add field type
                        v_attrFLDTYPE = v_XmlDocument.CreateAttribute("fldtype")
                        v_attrFLDTYPE.Value = pv_ds.Tables(0).Columns(v_intColumn).DataType.ToString
                        entryNode.Attributes.Append(v_attrFLDTYPE)

                        'Add current value
                        v_attrOLDVAL = v_XmlDocument.CreateAttribute("oldval")
                        If Not IsDBNull(pv_ds.Tables(0).Rows(v_int)(v_intColumn)) Then
                            If pv_ds.Tables(0).Rows(v_int)(v_intColumn).GetType.Name = GetType(System.DateTime).Name Then
                                v_attrOLDVAL.Value = Format(pv_ds.Tables(0).Rows(v_int)(v_intColumn), gc_FORMAT_DATE)
                            Else
                                v_attrOLDVAL.Value = CStr(pv_ds.Tables(0).Rows(v_int)(v_intColumn))
                            End If
                        End If
                        entryNode.Attributes.Append(v_attrOLDVAL)

                        'Set value
                        If Not IsDBNull(pv_ds.Tables(0).Rows(v_int)(v_intColumn)) Then
                            If pv_ds.Tables(0).Rows(v_int)(v_intColumn).GetType.Name = GetType(System.DateTime).Name Then
                                entryNode.InnerText = Format(pv_ds.Tables(0).Rows(v_int)(v_intColumn), gc_FORMAT_DATE)
                            Else
                                entryNode.InnerText = CStr(pv_ds.Tables(0).Rows(v_int)(v_intColumn))
                            End If
                        End If

                        dataElement.AppendChild(entryNode)
                    Next v_intColumn

                    v_XmlDocument.DocumentElement.AppendChild(dataElement)
                Next v_int
            End If

            pv_strObjectMessage = v_XmlDocument.InnerXml
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'Hanm5 thêm hanm duoi nay
    Public Function BuildXMLExportData(ByVal pv_ds As DataSet, _
                                    ByRef pv_strObjectMessage As String, _
                                    ByRef pv_strElement As String, _
                                    Optional ByVal pv_dsOldInput As DataSet = Nothing _
                                    ) As Long
        Dim v_XmlDocument As New Xml.XmlDocument
        Dim dataElement As Xml.XmlElement
        Dim FieldNode As Xml.XmlNode
        Dim v_strFLDNAME As String


        v_XmlDocument.LoadXml(pv_strObjectMessage)

        Try
            If pv_ds.Tables(0).Rows.Count > 0 Then
                For v_int As Integer = 0 To pv_ds.Tables(0).Rows.Count - 1
                    dataElement = v_XmlDocument.CreateElement(Xml.XmlNodeType.Element, pv_strElement, "")

                    For v_intColumn As Integer = 0 To pv_ds.Tables(0).Columns.Count - 1

                        v_strFLDNAME = CStr(pv_ds.Tables(0).Columns(v_intColumn).ColumnName)
                        FieldNode = v_XmlDocument.CreateNode(Xml.XmlNodeType.Element, v_strFLDNAME, "")

                        'Set value
                        If Not IsDBNull(pv_ds.Tables(0).Rows(v_int)(v_intColumn)) Then
                            If pv_ds.Tables(0).Rows(v_int)(v_intColumn).GetType.Name = GetType(System.DateTime).Name Then
                                FieldNode.InnerText = Format(pv_ds.Tables(0).Rows(v_int)(v_intColumn), gc_FORMAT_DATE)
                            Else
                                FieldNode.InnerText = CStr(pv_ds.Tables(0).Rows(v_int)(v_intColumn))
                            End If
                        End If
                        dataElement.AppendChild(FieldNode)
                    Next v_intColumn
                    v_XmlDocument.DocumentElement.AppendChild(dataElement)
                Next v_int
            End If
            pv_strObjectMessage = v_XmlDocument.InnerXml
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    'Hanm5

    Public Function BuildXMLErrorException(ByRef pv_xmlDocument As Xml.XmlDocument, _
                                           ByVal pv_strErrorSource As String, _
                                           ByVal pv_lngErrorCode As Long, _
                                           ByVal pv_strErrorMessage As String) As Long
        Dim dataElement As Xml.XmlElement
        Dim entryNode As Xml.XmlNode
        Dim v_attrFLDNAME, v_attrFLDTYPE, v_attrOLDVAL As Xml.XmlAttribute

        Try
            If pv_strErrorSource Is Nothing Then pv_strErrorSource = String.Empty
            If pv_strErrorMessage Is Nothing Then pv_strErrorMessage = String.Empty

            dataElement = pv_xmlDocument.CreateElement(Xml.XmlNodeType.Element, "ErrorException", "")

            entryNode = pv_xmlDocument.CreateNode(Xml.XmlNodeType.Element, "Entry", "")
            'Add field name
            v_attrFLDNAME = pv_xmlDocument.CreateAttribute("fldname")
            v_attrFLDNAME.Value = "ERRSOURCE"
            entryNode.Attributes.Append(v_attrFLDNAME)
            'Add field type
            v_attrFLDTYPE = pv_xmlDocument.CreateAttribute("fldtype")
            v_attrFLDTYPE.Value = pv_strErrorSource.GetType().ToString()
            entryNode.Attributes.Append(v_attrFLDTYPE)
            'Add current value
            v_attrOLDVAL = pv_xmlDocument.CreateAttribute("oldval")
            If pv_strErrorSource.Length > 0 Then
                v_attrOLDVAL.Value = pv_strErrorSource
            End If
            entryNode.Attributes.Append(v_attrOLDVAL)
            'Set value
            If pv_strErrorSource.Length > 0 Then
                entryNode.InnerText = pv_strErrorSource
            End If
            dataElement.AppendChild(entryNode)

            entryNode = pv_xmlDocument.CreateNode(Xml.XmlNodeType.Element, "Entry", "")
            'Add field name
            v_attrFLDNAME = pv_xmlDocument.CreateAttribute("fldname")
            v_attrFLDNAME.Value = "ERRCODE"
            entryNode.Attributes.Append(v_attrFLDNAME)
            'Add field type
            v_attrFLDTYPE = pv_xmlDocument.CreateAttribute("fldtype")
            v_attrFLDTYPE.Value = pv_lngErrorCode.GetType().ToString()
            entryNode.Attributes.Append(v_attrFLDTYPE)
            'Add current value
            v_attrOLDVAL = pv_xmlDocument.CreateAttribute("oldval")
            If pv_lngErrorCode <> 0 Then
                v_attrOLDVAL.Value = pv_lngErrorCode.ToString()
            End If
            entryNode.Attributes.Append(v_attrOLDVAL)
            'Set value
            If pv_lngErrorCode <> 0 Then
                entryNode.InnerText = pv_lngErrorCode.ToString()
            End If
            dataElement.AppendChild(entryNode)

            entryNode = pv_xmlDocument.CreateNode(Xml.XmlNodeType.Element, "Entry", "")
            'Add field name
            v_attrFLDNAME = pv_xmlDocument.CreateAttribute("fldname")
            v_attrFLDNAME.Value = "ERRMSG"
            entryNode.Attributes.Append(v_attrFLDNAME)
            'Add field type
            v_attrFLDTYPE = pv_xmlDocument.CreateAttribute("fldtype")
            v_attrFLDTYPE.Value = pv_strErrorMessage.GetType().ToString()
            entryNode.Attributes.Append(v_attrFLDTYPE)
            'Add current value
            v_attrOLDVAL = pv_xmlDocument.CreateAttribute("oldval")
            If pv_strErrorMessage.Length > 0 Then
                v_attrOLDVAL.Value = pv_strErrorMessage
            End If
            entryNode.Attributes.Append(v_attrOLDVAL)
            'Set value
            If pv_strErrorMessage.Length > 0 Then
                entryNode.InnerText = pv_strErrorMessage
            End If
            dataElement.AppendChild(entryNode)

            pv_xmlDocument.DocumentElement.AppendChild(dataElement)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ReplaceXMLErrorException(ByRef pv_strMessage As String, _
                                             ByVal pv_strErrorSource As String, _
                                             ByVal pv_lngErrorCode As Long, _
                                             ByVal pv_strErrorMessage As String) As Long
        Dim v_xmlDocument As New Xml.XmlDocument
        Dim v_oldElement, dataElement As Xml.XmlElement
        Dim entryNode As Xml.XmlNode
        Dim v_attrFLDNAME, v_attrFLDTYPE, v_attrOLDVAL As Xml.XmlAttribute

        Try
            If pv_strErrorSource Is Nothing Then pv_strErrorSource = String.Empty
            If pv_strErrorMessage Is Nothing Then pv_strErrorMessage = String.Empty

            v_xmlDocument.LoadXml(pv_strMessage)
            v_oldElement = v_xmlDocument.DocumentElement.GetElementsByTagName("ErrorException").Item(0)
            If v_oldElement Is Nothing Then
                'Tạo mới node ErrorExeption
                v_oldElement = v_xmlDocument.CreateElement(Xml.XmlNodeType.Element, "ErrorException", "")
                v_xmlDocument.DocumentElement.AppendChild(v_oldElement)
            End If

            dataElement = v_xmlDocument.CreateElement(Xml.XmlNodeType.Element, "ErrorException", "")

            entryNode = v_xmlDocument.CreateNode(Xml.XmlNodeType.Element, "Entry", "")
            'Add field name
            v_attrFLDNAME = v_xmlDocument.CreateAttribute("fldname")
            v_attrFLDNAME.Value = "ERRSOURCE"
            entryNode.Attributes.Append(v_attrFLDNAME)
            'Add field type
            v_attrFLDTYPE = v_xmlDocument.CreateAttribute("fldtype")
            v_attrFLDTYPE.Value = pv_strErrorSource.GetType().ToString()
            entryNode.Attributes.Append(v_attrFLDTYPE)
            'Add current value
            v_attrOLDVAL = v_xmlDocument.CreateAttribute("oldval")
            If pv_strErrorSource.Length > 0 Then
                v_attrOLDVAL.Value = pv_strErrorSource
            End If
            entryNode.Attributes.Append(v_attrOLDVAL)
            'Set value
            If pv_strErrorSource.Length > 0 Then
                entryNode.InnerText = pv_strErrorSource
            End If
            dataElement.AppendChild(entryNode)

            entryNode = v_xmlDocument.CreateNode(Xml.XmlNodeType.Element, "Entry", "")
            'Add field name
            v_attrFLDNAME = v_xmlDocument.CreateAttribute("fldname")
            v_attrFLDNAME.Value = "ERRCODE"
            entryNode.Attributes.Append(v_attrFLDNAME)
            'Add field type
            v_attrFLDTYPE = v_xmlDocument.CreateAttribute("fldtype")
            v_attrFLDTYPE.Value = pv_lngErrorCode.GetType().ToString()
            entryNode.Attributes.Append(v_attrFLDTYPE)
            'Add current value
            v_attrOLDVAL = v_xmlDocument.CreateAttribute("oldval")
            If pv_lngErrorCode <> 0 Then
                v_attrOLDVAL.Value = pv_lngErrorCode.ToString()
            End If
            entryNode.Attributes.Append(v_attrOLDVAL)
            'Set value
            If pv_lngErrorCode <> 0 Then
                entryNode.InnerText = pv_lngErrorCode.ToString()
            End If
            dataElement.AppendChild(entryNode)

            entryNode = v_xmlDocument.CreateNode(Xml.XmlNodeType.Element, "Entry", "")
            'Add field name
            v_attrFLDNAME = v_xmlDocument.CreateAttribute("fldname")
            v_attrFLDNAME.Value = "ERRMSG"
            entryNode.Attributes.Append(v_attrFLDNAME)
            'Add field type
            v_attrFLDTYPE = v_xmlDocument.CreateAttribute("fldtype")
            v_attrFLDTYPE.Value = pv_strErrorMessage.GetType().ToString()
            entryNode.Attributes.Append(v_attrFLDTYPE)
            'Add current value
            v_attrOLDVAL = v_xmlDocument.CreateAttribute("oldval")
            If pv_strErrorMessage.Length > 0 Then
                v_attrOLDVAL.Value = pv_strErrorMessage
            End If
            entryNode.Attributes.Append(v_attrOLDVAL)
            'Set value
            If pv_strErrorMessage.Length > 0 Then
                entryNode.InnerText = pv_strErrorMessage
            End If
            dataElement.AppendChild(entryNode)

            'Thay node ErrorExeption cũ
            v_xmlDocument.DocumentElement.ReplaceChild(dataElement, v_oldElement)

            pv_strMessage = v_xmlDocument.InnerXml
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    '----------------------------------------------------------------------------------------------
    ' + Mục đích:   Lấy thông tin lỗi từ message trả v?
    ' + �?�ầu vào:    
    '       - pv_strMessage:        Message trả v?
    ' + �?�ầu ra:
    '       - pv_strErrorSource:    Nguồn phát sinh lỗi, = "" nếu không có lỗi
    '       - pv_lngErrorCode:      Mã lỗi, = 0 nếu không có lỗi
    '       - pv_strErrorMessage:   Thông báo lỗi, = "" nếu không có lỗi
    ' + Trả v?:     NA
    ' + T�ác giả:    Trần Ki?u Minh
    ' + Ghi ch�ú:    N/A
    '----------------------------------------------------------------------------------------------
    Public Function GetErrorFromMessage(ByVal pv_strMessage As String, _
                                        ByRef pv_strErrorSource As String, _
                                        ByRef pv_lngErrorCode As Long, _
                                        ByRef pv_strErrorMessage As String) As Long
        Dim v_xmlDocument As New Xml.XmlDocument
        Dim v_nodeList As Xml.XmlNodeList = Nothing
        Dim v_strFLDNAME As String = ""
        Dim v_strValue As String = ""

        Try
            v_xmlDocument.LoadXml(pv_strMessage)
            Dim v_attrColl As Xml.XmlAttributeCollection = v_xmlDocument.DocumentElement.Attributes
            Dim v_strMSGTYPE As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeMSGTYPE), Xml.XmlAttribute).Value)

            'Xác định loại message
            If (v_strMSGTYPE = gc_MsgTypeObj) Or (v_strMSGTYPE = gc_MsgTypeRpt) Or (v_strMSGTYPE = gc_MsgTypeProc) Then
                v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/ErrorException")
            ElseIf v_strMSGTYPE = gc_MsgTypeTrans Then
                v_nodeList = v_xmlDocument.SelectNodes("/TransactMessage/ErrorException")
            End If

            'Lấy thông tin lỗi
            If v_nodeList.Count > 0 Then 'Có thông tin lỗi
                For i As Integer = 0 To v_nodeList.Count - 1
                    For j As Integer = 0 To v_nodeList.Item(i).ChildNodes.Count - 1
                        With v_nodeList.Item(i).ChildNodes(j)
                            v_strFLDNAME = CStr(CType(.Attributes.GetNamedItem("fldname"), Xml.XmlAttribute).Value)
                            v_strValue = .InnerText.ToString

                            Select Case Trim(v_strFLDNAME)
                                Case "ERRSOURCE"
                                    pv_strErrorSource = Trim(v_strValue)
                                Case "ERRCODE"
                                    pv_lngErrorCode = CLng(Trim(v_strValue))
                                Case "ERRMSG"
                                    pv_strErrorMessage = Trim(v_strValue)
                            End Select
                        End With
                    Next
                Next
            Else
                pv_strErrorSource = String.Empty
                pv_strErrorMessage = String.Empty
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    '----------------------------------------------------------------------------------------------
    ' + Mục đích:   Lấy nguyên nhân duyệt từ message trả v?
    ' + �?�ầu vào:    
    '       - pv_strMessage:        Message trả v?
    ' + �?�ầu ra:
    '       - pv_strErrorMessage:   Thông báo lỗi, = "" nếu không có lỗi
    ' + Trả v?:     NA
    ' + T�ác giả:    Trần Ki?u Minh
    ' + Ghi ch�ú:    N/A
    '----------------------------------------------------------------------------------------------
    Public Function GetReasonFromMessage(ByVal pv_strMessage As String, _
                                        ByRef pv_strErrorMessage As String) As Long
        Dim v_xmlDocument As New Xml.XmlDocument
        Dim v_nodeList As Xml.XmlNodeList = Nothing

        Try
            v_xmlDocument.LoadXml(pv_strMessage)
            Dim v_attrColl As Xml.XmlAttributeCollection = v_xmlDocument.DocumentElement.Attributes
            Dim v_strMSGTYPE As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeMSGTYPE), Xml.XmlAttribute).Value)

            'Xác định loại message
            If v_strMSGTYPE = gc_MsgTypeObj Then
                v_nodeList = v_xmlDocument.SelectNodes("/ObjectMessage/checker")
            ElseIf v_strMSGTYPE = gc_MsgTypeTrans Then
                v_nodeList = v_xmlDocument.SelectNodes("/TransactMessage/checker")
            End If

            'Lấy thông tin nguyên nhân duyệt
            If v_nodeList.Count > 0 Then 'Có nguyên nhân duyệt đi kèm
                For i As Integer = 0 To v_nodeList.Count - 1
                    For j As Integer = 0 To v_nodeList.Item(i).ChildNodes.Count - 1
                        pv_strErrorMessage = pv_strErrorMessage & ControlChars.CrLf & v_nodeList.Item(i).ChildNodes(j).InnerXml
                    Next
                Next
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function DDMMYYYY_SystemDate(ByVal v_strDate As String) As Date
        Try
            Dim v_dtREFDATE As Date
            v_dtREFDATE = CDate("01/01/" & GetDateValue(v_strDate, "Y")) 'Lấy năm hiện tại
            v_dtREFDATE = DateAdd(DateInterval.Month, GetDateValue(v_strDate, "M") - 1, v_dtREFDATE) 'Lấy tháng hiện tại
            v_dtREFDATE = DateAdd(DateInterval.Day, GetDateValue(v_strDate, "D") - 1, v_dtREFDATE)  'Lấy ngày hiện tại
            Return v_dtREFDATE
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetDateValue(ByVal v_strDate As String, ByVal v_strType As String) As Integer
        'Format là DD/MM/YYYY
        Dim i, j As Integer
        i = InStr(1, v_strDate, "/")
        If i > 0 Then j = InStr(i + 1, v_strDate, "/")
        If i > 0 And j > 0 Then
            Select Case Trim(v_strType)
                Case "D"
                    Return CInt(Mid(v_strDate, 1, i - 1))
                Case "M"
                    Return CInt(Mid(v_strDate, i + 1, j - i - 1))
                Case "Y"
                    Return CInt(Mid(v_strDate, j + 1, 4))
            End Select
        End If
    End Function

    Function IsDateValue(ByVal v_strDate As String) As Boolean
        Try
            'Format là DD/MM/YYYY
            Dim intDay, intMonth, intYear As Integer, v_dtValue As Date = Now
            intDay = GetDateValue(v_strDate, "D")
            intMonth = GetDateValue(v_strDate, "M")
            intYear = GetDateValue(v_strDate, "Y")

            'Kiểm tra giá trị năm và tháng phải hợp lệ
            If intYear < 1900 And intYear > 2099 Then
                Return False
            End If
            If intMonth < 1 And intMonth > 12 Then
                Return False
            End If

            'Kiểm tra giá trị ngày phải hợp lệ
            If intDay < 1 Or intDay > Date.DaysInMonth(intYear, intMonth) Then
                Return False
            End If
            Return True
        Catch
            Return False
        End Try
    End Function

    Function GetMax(ByVal v_dblNumber1 As Double, ByVal v_dblNumber2 As Double) As Double
        Return IIf(v_dblNumber1 > v_dblNumber2, v_dblNumber1, v_dblNumber2)
    End Function

    Function GetMin(ByVal v_dblNumber1 As Double, ByVal v_dblNumber2 As Double) As Double
        Return IIf(v_dblNumber1 > v_dblNumber2, v_dblNumber2, v_dblNumber1)
    End Function

    Private Function GetIPAddress() As IPAddress
        Try
            Dim sHostName As String = System.Net.Dns.GetHostName()
            Dim ipE As System.Net.IPHostEntry = Dns.GetHostByName(sHostName)
            Dim IpA() As System.Net.IPAddress = ipE.AddressList

            Return IpA(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    'Hàm sử dụng để lấy địa chỉ MAC của card mạng
    Public Function GetMACAddress() As String
        Try
            'Dim tempAddress As IPAddress = GetIPAddress()

            'Dim ab() As Byte
            'ReDim ab(5)
            'Dim len As Int32 = ab.Length()
            ''Dim i As Long = tempAddress.Address
            'Dim r As Integer = SendARP(Integer.Parse(tempAddress.Address), 0, ab, len)
            'Dim mac As String = BitConverter.ToString(ab, 0, 6)

            'Return mac
            Return "FPT Software Solutions"
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    'Cac ham doc tu lieu tu file 
    Public Function ReadFileASTPT(ByVal pv_strFile As String) As DataTable
        Try
            Dim v_Stream As New System.IO.StreamReader(pv_strFile)
            Dim v_strLine As String
            Dim v_dt As New DataTable("ASTPT")
            Dim v_strConfirmNo, v_strMatch_Date, v_strMatch_Time, v_strSec_Code, v_strSET_TYPE As String
            Dim v_strQty, v_strPrice, v_strB_ACC_NO, v_strS_ACC_NO, v_strBlock_Tran As String
            Dim v_strB_CODE_TRADE, v_strS_CODE_TRADE, v_strB_ORDER_NO, v_strS_ORDER_NO, v_strB_PC_PLAG, v_strS_PC_PLAG As String

            v_strBlock_Tran = "1"
            v_strSET_TYPE = "3"

            'Tao table
            Dim v_oDataColumn As DataColumn
            Dim v_oDataRow As DataRow

            For i As Integer = 0 To 15
                v_oDataColumn = New DataColumn("Col" & i + 1)
                v_oDataColumn.ColumnName = "Col" & i + 1
                v_oDataColumn.DataType = GetType(System.String)
                v_dt.Columns.Add(v_oDataColumn)
            Next

            While Not v_Stream.EndOfStream
                v_oDataRow = v_dt.NewRow

                v_strLine = v_Stream.ReadLine
                If v_strLine <> "" Then
                    v_strConfirmNo = Mid(v_strLine, 1, 6)
                    v_oDataRow(0) = String.Format("{0:900000}", CInt(v_strConfirmNo))

                    v_strMatch_Time = Mid(v_strLine, 7, 8)
                    v_oDataRow(1) = v_strMatch_Time.Trim

                    v_strMatch_Date = Mid(v_strLine, 15, 10)
                    v_oDataRow(2) = v_strMatch_Date.Trim

                    v_strSec_Code = Mid(v_strLine, 42, 8)
                    v_oDataRow(3) = v_strSec_Code.Trim

                    v_strPrice = Mid(v_strLine, 50, 13)
                    v_oDataRow(4) = CStr(CDbl(v_strPrice.Trim) * 1000)

                    If CDbl("0" & Mid(v_strLine, 96, 8).Trim) > 0 Then
                        v_strQty = Mid(v_strLine, 96, 8)
                        v_strB_PC_PLAG = "P"
                    ElseIf CDbl("0" & Mid(v_strLine, 104, 8).Trim) > 0 Then
                        v_strQty = Mid(v_strLine, 104, 8)
                        v_strB_PC_PLAG = "C"
                    ElseIf CDbl("0" & Mid(v_strLine, 112, 8).Trim) > 0 Then
                        v_strQty = Mid(v_strLine, 112, 8)
                        v_strB_PC_PLAG = "M"
                    ElseIf CDbl("0" & Mid(v_strLine, 120, 8).Trim) > 0 Then
                        v_strQty = Mid(v_strLine, 120, 8)
                        v_strB_PC_PLAG = "F"
                    End If

                    If CDbl("0" & Mid(v_strLine, 160, 8).Trim) > 0 Then
                        v_strS_PC_PLAG = "P"
                    ElseIf CDbl("0" & Mid(v_strLine, 168, 8).Trim) > 0 Then
                        v_strS_PC_PLAG = "C"
                    ElseIf CDbl("0" & Mid(v_strLine, 176, 8).Trim) > 0 Then
                        v_strS_PC_PLAG = "M"
                    ElseIf CDbl("0" & Mid(v_strLine, 184, 8).Trim) > 0 Then
                        v_strS_PC_PLAG = "F"
                    End If

                    v_oDataRow(5) = v_strQty.Trim

                    v_strB_ACC_NO = Mid(v_strLine, 63, 10)
                    v_oDataRow(6) = v_strB_ACC_NO.Trim

                    v_strS_ACC_NO = Mid(v_strLine, 73, 10)
                    v_oDataRow(7) = v_strS_ACC_NO.Trim

                    v_oDataRow(8) = v_strSET_TYPE
                    v_oDataRow(9) = v_strBlock_Tran

                    v_strB_CODE_TRADE = Mid(v_strLine, 33, 3)
                    v_oDataRow(10) = v_strB_CODE_TRADE

                    v_strS_CODE_TRADE = Mid(v_strLine, 36, 3)
                    v_oDataRow(11) = v_strS_CODE_TRADE

                    v_strB_ORDER_NO = v_strConfirmNo
                    v_oDataRow(12) = v_strB_ORDER_NO

                    v_strS_ORDER_NO = v_strConfirmNo
                    v_oDataRow(13) = v_strS_ORDER_NO


                    v_oDataRow(14) = v_strB_PC_PLAG

                    v_oDataRow(15) = v_strS_PC_PLAG

                    v_dt.Rows.Add(v_oDataRow)
                End If
            End While
            Return v_dt
        Catch ex As Exception
            MsgBox("File dữ liệu không hợp lệ!", MsgBoxStyle.Exclamation, gc_ApplicationTitle)
            Return Nothing
        End Try
    End Function

    Public Function ReadFileFullASTPT(ByVal pv_strFile As String) As DataSet
        Try
            Dim v_Stream As New System.IO.StreamReader(pv_strFile)
            Dim v_strLine As String
            Dim v_ds As New DataSet("ASTDT")
            Dim v_strValue As String

            'Tao table
            Dim v_oDataColumn As DataColumn
            Dim v_oDataRow As DataRow

            v_ds.Tables.Add("IMPFILE")

            v_oDataColumn = New DataColumn("Col0")
            v_oDataColumn.ColumnName = "Col0"
            v_oDataColumn.DataType = GetType(System.Double)
            v_ds.Tables(0).Columns.Add(v_oDataColumn)

            For i As Integer = 1 To 34
                v_oDataColumn = New DataColumn("Col" & i)
                v_oDataColumn.ColumnName = "Col" & i
                v_oDataColumn.DataType = GetType(System.String)
                v_ds.Tables(0).Columns.Add(v_oDataColumn)
            Next
            Dim v_int As Long = 0
            While Not v_Stream.EndOfStream
                v_oDataRow = v_ds.Tables(0).NewRow

                v_strLine = v_Stream.ReadLine
                If v_strLine <> "" Then
                    v_int += 1
                    'STT
                    v_oDataRow(0) = v_int
                    'Confirm No
                    v_strValue = Mid(v_strLine, 1, 6)
                    v_oDataRow(1) = v_strValue.Trim
                    'Put Through Deal Time
                    v_strValue = Mid(v_strLine, 7, 8)
                    v_oDataRow(2) = v_strValue.Trim
                    'Put Through Deal Time
                    v_strValue = Mid(v_strLine, 15, 10)
                    v_oDataRow(3) = v_strValue.Trim
                    'Buyer Trader ID
                    v_strValue = Mid(v_strLine, 25, 4)
                    v_oDataRow(4) = v_strValue.Trim
                    'Seller Trader ID
                    v_strValue = Mid(v_strLine, 29, 4)
                    v_oDataRow(5) = v_strValue.Trim
                    'Buyer Firm No
                    v_strValue = Mid(v_strLine, 33, 3)
                    v_oDataRow(6) = v_strValue.Trim
                    'Seller Firm No
                    v_strValue = Mid(v_strLine, 36, 3)
                    v_oDataRow(7) = v_strValue.Trim
                    'Board Type
                    v_strValue = Mid(v_strLine, 39, 1)
                    v_oDataRow(8) = v_strValue.Trim
                    'Status
                    v_strValue = Mid(v_strLine, 40, 2)
                    v_oDataRow(9) = v_strValue.Trim
                    'Security
                    v_strValue = Mid(v_strLine, 42, 8)
                    v_oDataRow(10) = v_strValue.Trim
                    'Price
                    v_strValue = Mid(v_strLine, 50, 13)
                    v_oDataRow(11) = v_strValue.Trim
                    'Buyer Customer ID
                    v_strValue = Mid(v_strLine, 63, 10)
                    v_oDataRow(12) = v_strValue.Trim
                    'Seller Customer ID
                    v_strValue = Mid(v_strLine, 73, 10)
                    v_oDataRow(13) = v_strValue.Trim
                    'PT Deal ID
                    v_strValue = Mid(v_strLine, 83, 5)
                    v_oDataRow(14) = v_strValue.Trim
                    'Fillter
                    v_strValue = Mid(v_strLine, 88, 8)
                    v_oDataRow(15) = v_strValue.Trim
                    'Buyer Portfolio Volume
                    v_strValue = Mid(v_strLine, 96, 8)
                    v_oDataRow(16) = v_strValue
                    'Buyer Customer Volume
                    v_strValue = Mid(v_strLine, 104, 8)
                    v_oDataRow(17) = v_strValue
                    'Buyer Mutual Fund Volume
                    v_strValue = Mid(v_strLine, 112, 8)
                    v_oDataRow(18) = v_strValue
                    'Buyer Foreign Volume
                    v_strValue = Mid(v_strLine, 120, 8)
                    v_oDataRow(19) = v_strValue
                    'Reserved 1
                    v_strValue = Mid(v_strLine, 128, 8)
                    v_oDataRow(20) = v_strValue.Trim
                    'Reserved 2
                    v_strValue = Mid(v_strLine, 136, 8)
                    v_oDataRow(21) = v_strValue.Trim
                    'Reserved 3
                    v_strValue = Mid(v_strLine, 144, 8)
                    v_oDataRow(22) = v_strValue.Trim
                    'Reserved 4
                    v_strValue = Mid(v_strLine, 152, 8)
                    v_oDataRow(23) = v_strValue.Trim

                    'Seller Portfolio Volume
                    v_strValue = Mid(v_strLine, 160, 8)
                    v_oDataRow(24) = v_strValue
                    'Seller Customer Volume
                    v_strValue = Mid(v_strLine, 168, 8)
                    v_oDataRow(25) = v_strValue
                    'Seller Mutual Fund Volume
                    v_strValue = Mid(v_strLine, 176, 8)
                    v_oDataRow(26) = v_strValue
                    'Seller Foreign Volume
                    v_strValue = Mid(v_strLine, 184, 8)
                    v_oDataRow(27) = v_strValue
                    'Reserved 1
                    v_strValue = Mid(v_strLine, 192, 8)
                    v_oDataRow(28) = v_strValue.Trim
                    'Reserved 2
                    v_strValue = Mid(v_strLine, 200, 8)
                    v_oDataRow(29) = v_strValue.Trim
                    'Reserved 3
                    v_strValue = Mid(v_strLine, 208, 8)
                    v_oDataRow(30) = v_strValue.Trim
                    'Reserved 4
                    v_strValue = Mid(v_strLine, 216, 8)
                    v_oDataRow(31) = v_strValue.Trim

                    'Filter
                    v_strValue = Mid(v_strLine, 224, 8)
                    v_oDataRow(32) = v_strValue.Trim
                    'Buyer Approved Time
                    v_strValue = Mid(v_strLine, 232, 8)
                    v_oDataRow(33) = v_strValue.Trim
                    'Filter
                    v_strValue = Mid(v_strLine, 240, 22)
                    v_oDataRow(34) = v_strValue.Trim


                    v_ds.Tables(0).Rows.Add(v_oDataRow)
                End If
            End While

            Return v_ds
        Catch ex As Exception
            MsgBox("File dữ liệu không hợp lệ!", MsgBoxStyle.Exclamation, gc_ApplicationTitle)
            Return Nothing
        End Try
    End Function

    Public Function ReadFileASTDT(ByVal pv_strFile As String) As DataTable
        Try
            Dim v_Stream As New System.IO.StreamReader(pv_strFile)
            Dim v_strLine As String
            Dim v_dt As New DataTable("ASTDT")
            Dim v_strConfirmNo, v_strMatch_Date, v_strMatch_Time, v_strSec_Code, v_strSET_TYPE As String
            Dim v_strQty, v_strPrice, v_strB_ACC_NO, v_strS_ACC_NO, v_strBlock_Tran As String
            Dim v_strB_CODE_TRADE, v_strS_CODE_TRADE, v_strB_ORDER_NO, v_strS_ORDER_NO, v_strB_PC_PLAG, v_strS_PC_PLAG As String

            v_strBlock_Tran = "0"
            v_strSET_TYPE = "3"

            'Tao table
            Dim v_oDataColumn As DataColumn
            Dim v_oDataRow As DataRow

            For i As Integer = 0 To 15
                v_oDataColumn = New DataColumn("Col" & i + 1)
                v_oDataColumn.ColumnName = "Col" & i + 1
                v_oDataColumn.DataType = GetType(System.String)
                v_dt.Columns.Add(v_oDataColumn)
            Next

            While Not v_Stream.EndOfStream
                v_oDataRow = v_dt.NewRow

                v_strLine = v_Stream.ReadLine
                If v_strLine <> "" Then
                    v_strConfirmNo = Mid(v_strLine, 1, 6)
                    v_oDataRow(0) = String.Format("{0:000000}", CInt(v_strConfirmNo))

                    v_strMatch_Time = Mid(v_strLine, 55, 8)
                    v_oDataRow(1) = v_strMatch_Time.Trim

                    v_strMatch_Date = Mid(v_strLine, 63, 10)
                    v_oDataRow(2) = v_strMatch_Date.Trim

                    v_strSec_Code = Mid(v_strLine, 92, 8)
                    v_oDataRow(3) = v_strSec_Code.Trim

                    v_strPrice = Mid(v_strLine, 108, 9)
                    v_oDataRow(4) = CStr(CDbl(v_strPrice.Trim) * 1000)

                    v_strQty = Mid(v_strLine, 100, 8)
                    v_oDataRow(5) = v_strQty.Trim

                    v_strB_ACC_NO = Mid(v_strLine, 117, 10)
                    v_oDataRow(6) = v_strB_ACC_NO.Trim

                    v_strS_ACC_NO = Mid(v_strLine, 127, 10)
                    v_oDataRow(7) = v_strS_ACC_NO.Trim

                    v_oDataRow(8) = v_strSET_TYPE
                    v_oDataRow(9) = v_strBlock_Tran

                    v_strB_CODE_TRADE = Mid(v_strLine, 83, 3)
                    v_oDataRow(10) = v_strB_CODE_TRADE

                    v_strS_CODE_TRADE = Mid(v_strLine, 86, 3)
                    v_oDataRow(11) = v_strS_CODE_TRADE

                    v_strB_ORDER_NO = Mid(v_strLine, 7, 8)
                    v_oDataRow(12) = v_strB_ORDER_NO

                    v_strS_ORDER_NO = Mid(v_strLine, 25, 8)
                    v_oDataRow(13) = v_strS_ORDER_NO

                    v_strB_PC_PLAG = Mid(v_strLine, 81, 1)
                    v_oDataRow(14) = v_strB_PC_PLAG

                    v_strS_PC_PLAG = Mid(v_strLine, 82, 1)
                    v_oDataRow(15) = v_strS_PC_PLAG

                    v_dt.Rows.Add(v_oDataRow)
                End If
            End While

            Return v_dt
        Catch ex As Exception
            MsgBox("File dữ liệu không hợp lệ!", MsgBoxStyle.Exclamation, gc_ApplicationTitle)
            Return Nothing
        End Try
    End Function


    Public Function ReadFileFullASTDT(ByVal pv_strFile As String) As DataSet
        Try
            Dim v_Stream As New System.IO.StreamReader(pv_strFile)
            Dim v_strLine As String
            Dim v_ds As New DataSet("ASTDT")
            Dim v_strValue As String

            'Tao table
            Dim v_oDataColumn As DataColumn
            Dim v_oDataRow As DataRow

            v_ds.Tables.Add("IMPFILE")

            v_oDataColumn = New DataColumn("Col0")
            v_oDataColumn.ColumnName = "Col0"
            v_oDataColumn.DataType = GetType(System.Double)
            v_ds.Tables(0).Columns.Add(v_oDataColumn)

            For i As Integer = 1 To 26
                v_oDataColumn = New DataColumn("Col" & i)
                v_oDataColumn.ColumnName = "Col" & i
                v_oDataColumn.DataType = GetType(System.String)
                v_ds.Tables(0).Columns.Add(v_oDataColumn)
            Next
            Dim v_int As Long = 0
            While Not v_Stream.EndOfStream
                v_oDataRow = v_ds.Tables(0).NewRow

                v_strLine = v_Stream.ReadLine
                If v_strLine <> "" Then
                    v_int += 1
                    'STT
                    v_oDataRow(0) = v_int
                    'Confirm No
                    v_strValue = Mid(v_strLine, 1, 6)
                    v_oDataRow(1) = v_strValue.Trim
                    'Buyer Order No
                    v_strValue = Mid(v_strLine, 7, 8)
                    v_oDataRow(2) = v_strValue.Trim
                    'Buyer Order Date
                    v_strValue = Mid(v_strLine, 15, 10)
                    v_oDataRow(3) = v_strValue.Trim
                    'Seller Order No
                    v_strValue = Mid(v_strLine, 25, 8)
                    v_oDataRow(4) = v_strValue.Trim
                    'Seller Order Date
                    v_strValue = Mid(v_strLine, 33, 10)
                    v_oDataRow(5) = v_strValue.Trim
                    'Buyer Pointer
                    v_strValue = Mid(v_strLine, 43, 6)
                    v_oDataRow(6) = v_strValue.Trim
                    'Seller Pointer
                    v_strValue = Mid(v_strLine, 49, 6)
                    v_oDataRow(7) = v_strValue.Trim
                    'Deal time
                    v_strValue = Mid(v_strLine, 55, 8)
                    v_oDataRow(8) = v_strValue.Trim
                    'Deal date
                    v_strValue = Mid(v_strLine, 63, 10)
                    v_oDataRow(9) = v_strValue.Trim
                    'Buyer Trader ID
                    v_strValue = Mid(v_strLine, 73, 4)
                    v_oDataRow(10) = v_strValue.Trim
                    'Seller Trader ID
                    v_strValue = Mid(v_strLine, 77, 4)
                    v_oDataRow(11) = v_strValue.Trim
                    'Buyer PC Flag
                    v_strValue = Mid(v_strLine, 81, 1)
                    v_oDataRow(12) = v_strValue.Trim
                    'Seller PC Flag
                    v_strValue = Mid(v_strLine, 82, 1)
                    v_oDataRow(13) = v_strValue.Trim
                    'Buyer Confirm No
                    v_strValue = Mid(v_strLine, 83, 3)
                    v_oDataRow(14) = v_strValue.Trim
                    'Seller Confirm No
                    v_strValue = Mid(v_strLine, 86, 3)
                    v_oDataRow(15) = v_strValue.Trim
                    'Board Type
                    v_strValue = Mid(v_strLine, 89, 1)
                    v_oDataRow(16) = v_strValue.Trim
                    'Status
                    v_strValue = Mid(v_strLine, 90, 2)
                    v_oDataRow(17) = v_strValue.Trim
                    'Security
                    v_strValue = Mid(v_strLine, 92, 8)
                    v_oDataRow(18) = v_strValue.Trim
                    'Volume
                    v_strValue = Mid(v_strLine, 100, 8)
                    v_oDataRow(19) = v_strValue
                    'Price
                    v_strValue = Mid(v_strLine, 108, 9)
                    v_oDataRow(20) = v_strValue.Trim
                    'Buyer Customer ID
                    v_strValue = Mid(v_strLine, 117, 10)
                    v_oDataRow(21) = v_strValue.Trim
                    'Seller Customer ID
                    v_strValue = Mid(v_strLine, 127, 10)
                    v_oDataRow(22) = v_strValue.Trim
                    'Fillter
                    v_strValue = Mid(v_strLine, 137, 8)
                    v_oDataRow(23) = v_strValue.Trim
                    'Buyer Order Time
                    v_strValue = Mid(v_strLine, 145, 8)
                    v_oDataRow(24) = v_strValue.Trim
                    'Seller Order Time
                    v_strValue = Mid(v_strLine, 153, 8)
                    v_oDataRow(25) = v_strValue.Trim
                    'Filter
                    v_strValue = Mid(v_strLine, 161, 4)
                    v_oDataRow(26) = v_strValue.Trim

                    v_ds.Tables(0).Rows.Add(v_oDataRow)
                End If
            End While

            Return v_ds
        Catch ex As Exception
            MsgBox("File dữ liệu không hợp lệ!", MsgBoxStyle.Exclamation, gc_ApplicationTitle)
            Return Nothing
        End Try
    End Function

    Public Function ReadFileXML(ByVal pv_strFile As String) As DataTable
        Try
            Dim v_oDocument As New Xml.XmlDocument
            Dim v_dt As New DataTable("XMLHN")
            v_oDocument.Load(pv_strFile)
            v_oDocument.InnerXml = v_oDocument.InnerXml.Replace(" xmlns=""http://tempuri.org/SATS_TRADING_RESULT.xsd""", "")
            'Tao table
            Dim v_oDataColumn As DataColumn
            Dim v_oDataRow As DataRow
            Dim v_int As Integer

            For i As Integer = 0 To 15
                v_oDataColumn = New DataColumn("Col" & i + 1)
                v_oDataColumn.ColumnName = "Col" & i + 1
                v_oDataColumn.DataType = GetType(System.String)
                v_dt.Columns.Add(v_oDataColumn)
            Next

            Dim v_nodeList As Xml.XmlNodeList
            v_nodeList = v_oDocument.SelectNodes("/SATS_TRADING_RESULT/TRADING_RESULT")

            Dim v_strField, v_strValue As String

            For v_int = 0 To v_nodeList.Count - 1
                v_oDataRow = v_dt.NewRow
                For j As Integer = 0 To v_nodeList(v_int).ChildNodes.Count - 1
                    v_strField = v_nodeList(v_int).ChildNodes(j).Name
                    v_strValue = v_nodeList(v_int).ChildNodes(j).InnerText
                    Select Case v_strField.ToUpper
                        Case "CONFIRM_NO"
                            v_oDataRow(0) = v_strValue
                        Case "MATCH_TIME"
                            v_oDataRow(1) = v_strValue
                        Case "MATCH_DATE"
                            v_oDataRow(2) = Mid(v_strValue, 9, 2) & "/" & Mid(v_strValue, 6, 2) & "/" & Mid(v_strValue, 1, 4)
                        Case "SEC_CODE"
                            v_oDataRow(3) = v_strValue
                        Case "PRICE"
                            v_oDataRow(4) = v_strValue
                        Case "QUANTITY"
                            v_oDataRow(5) = v_strValue
                        Case "B_ACCOUNT_NO"
                            v_oDataRow(6) = v_strValue
                        Case "S_ACCOUNT_NO"
                            v_oDataRow(7) = v_strValue
                        Case "SETT_TYPE"
                            v_oDataRow(8) = v_strValue
                        Case "BLOCK_TRANS"
                            v_oDataRow(9) = v_strValue
                        Case "B_CODE_TRADE"
                            v_oDataRow(10) = IIf(v_strValue.Length = 2, "0" & v_strValue, v_strValue)
                        Case "S_CODE_TRADE"
                            v_oDataRow(11) = IIf(v_strValue.Length = 2, "0" & v_strValue, v_strValue)
                        Case "B_ORDER_NO"
                            v_oDataRow(12) = v_strValue
                        Case "S_ORDER_NO"
                            v_oDataRow(13) = v_strValue
                        Case "B_PC_FLAG"
                            v_oDataRow(14) = v_strValue
                        Case "S_PC_FLAG"
                            v_oDataRow(15) = v_strValue
                    End Select
                Next
                v_dt.Rows.Add(v_oDataRow)
            Next
            Return v_dt
        Catch ex As Exception
            MsgBox("File dữ liệu không hợp lệ!", MsgBoxStyle.Exclamation, gc_ApplicationTitle)
            Return Nothing
        End Try
    End Function

    Public Function ReadFileFullXML(ByVal pv_strFile As String) As DataSet
        Try
            Dim v_oDocument As New Xml.XmlDocument
            Dim v_ds As New DataSet("XMLHN")
            v_oDocument.Load(pv_strFile)
            v_oDocument.InnerXml = v_oDocument.InnerXml.Replace(" xmlns=""http://tempuri.org/SATS_TRADING_RESULT.xsd""", "")
            'Tao table
            Dim v_oDataColumn As DataColumn
            Dim v_oDataRow As DataRow
            Dim v_int As Integer

            v_ds.Tables.Add("IMPFILE")

            v_oDataColumn = New DataColumn("Col0")
            v_oDataColumn.ColumnName = "Col0"
            v_oDataColumn.DataType = GetType(System.String)
            v_ds.Tables(0).Columns.Add(v_oDataColumn)

            For i As Integer = 1 To 25
                v_oDataColumn = New DataColumn("Col" & i)
                v_oDataColumn.ColumnName = "Col" & i
                v_oDataColumn.DataType = GetType(System.String)
                v_ds.Tables(0).Columns.Add(v_oDataColumn)
            Next

            Dim v_nodeList As Xml.XmlNodeList
            v_nodeList = v_oDocument.SelectNodes("/SATS_TRADING_RESULT/TRADING_RESULT")

            Dim v_strField, v_strValue As String

            For v_int = 0 To v_nodeList.Count - 1
                v_oDataRow = v_ds.Tables(0).NewRow
                v_oDataRow(0) = v_int + 1
                For j As Integer = 0 To v_nodeList(v_int).ChildNodes.Count - 1
                    v_strField = v_nodeList(v_int).ChildNodes(j).Name
                    v_strValue = v_nodeList(v_int).ChildNodes(j).InnerText
                    Select Case v_strField.ToUpper
                        Case "CONFIRM_NO"
                            v_oDataRow(1) = v_strValue
                        Case "B_ORDER_NO"
                            v_oDataRow(2) = v_strValue
                        Case "B_ORDER_DATE"
                            v_oDataRow(3) = v_strValue
                        Case "S_ORDER_NO"
                            v_oDataRow(4) = v_strValue
                        Case "S_ORDER_DATE"
                            v_oDataRow(5) = v_strValue
                        Case "B_NEXT_CNFRM"
                            v_oDataRow(6) = v_strValue
                        Case "S_NEXT_CNFRM"
                            v_oDataRow(7) = v_strValue
                        Case "MATCH_TIME"
                            v_oDataRow(8) = Mid(v_strValue, 9, 2) & "/" & Mid(v_strValue, 6, 2) & "/" & Mid(v_strValue, 1, 4)
                        Case "MATCH_DATE"
                            v_oDataRow(9) = v_strValue
                        Case "B_TRADING_ID"
                            v_oDataRow(10) = v_strValue
                        Case "S_TRADING_ID"
                            v_oDataRow(11) = v_strValue
                        Case "B_PC_FLAG"
                            v_oDataRow(12) = v_strValue
                        Case "S_PC_FLAG"
                            v_oDataRow(13) = v_strValue
                        Case "B_CODE_TRADE"
                            v_oDataRow(14) = IIf(v_strValue.Length = 2, "0" & v_strValue, v_strValue)
                        Case "S_CODE_TRADE"
                            v_oDataRow(15) = IIf(v_strValue.Length = 2, "0" & v_strValue, v_strValue)
                        Case "STATUS"
                            v_oDataRow(16) = v_strValue
                        Case "SEC_CODE"
                            v_oDataRow(17) = v_strValue
                        Case "QUANTITY"
                            v_oDataRow(18) = v_strValue
                        Case "PRICE"
                            v_oDataRow(19) = v_strValue
                        Case "B_ACCOUNT_NO"
                            v_oDataRow(20) = v_strValue
                        Case "S_ACCOUNT_NO"
                            v_oDataRow(21) = v_strValue
                        Case "SETT_TYPE"
                            v_oDataRow(22) = v_strValue
                        Case "SETT_DAY"
                            v_oDataRow(23) = v_strValue
                        Case "TRADING_DATE"
                            v_oDataRow(24) = Mid(v_strValue, 9, 2) & "/" & Mid(v_strValue, 6, 2) & "/" & Mid(v_strValue, 1, 4)
                        Case "BLOCK_TRANS"
                            v_oDataRow(25) = v_strValue
                    End Select
                Next
                v_ds.Tables(0).Rows.Add(v_oDataRow)
            Next
            Return v_ds
        Catch ex As Exception
            MsgBox("File dữ liệu không hợp lệ!", MsgBoxStyle.Exclamation, gc_ApplicationTitle)
            Return Nothing
        End Try
    End Function

    Public Function GetDataTableFromFile(ByVal pv_strFile As String, Optional ByVal pv_strType As String = "") As DataTable
        Dim v_strEx As String
        Dim v_strFile As String
        Dim v_dt As DataTable
        Dim v_arr As Array
        v_strFile = Trim(pv_strFile)
        If System.IO.File.Exists(v_strFile) Then
            v_strEx = Mid(v_strFile, Len(v_strFile) - 2, 3)
            If v_strEx.ToUpper = "TXT" Then
                If pv_strType = "D" Then
                    v_dt = ReadFileASTDT(v_strFile)
                Else
                    v_dt = ReadFileASTPT(v_strFile)
                End If
            ElseIf v_strEx.ToUpper = "XML" Then
                v_dt = ReadFileXML(v_strFile)
            End If
        End If
        Return v_dt
    End Function

    Public Function CheckPassDate(ByVal pv_strDate As String, ByVal pv_strPassDate As String) As Boolean
        Dim v_Date, v_Passdate As Date
        v_Date = DDMMYYYY_SystemDate(pv_strDate)
        v_Passdate = DDMMYYYY_SystemDate(pv_strPassDate)
        Return v_Date < v_Passdate
    End Function

    Private Function GetQ(ByVal v_intMonth As Integer) As Integer
        Select Case v_intMonth
            Case 1, 2, 3
                Return 1
            Case 4, 5, 6
                Return 2
            Case 7, 8, 9
                Return 3
            Case 10, 11, 12
                Return 4
        End Select
    End Function

    Public Function GetListPartition(ByVal pv_strDate As String) As String
        Dim v_intMaxYear, v_intMinYear As Integer
        Dim v_intQMin, v_intQMax As Integer
        Dim v_strPartion As String = ""
        Dim v_strKey As String = ""

        v_intMaxYear = CInt(Mid(pv_strDate.Split("|")(1), 7, 4))
        v_intMinYear = CInt(Mid(pv_strDate.Split("|")(0), 7, 4))
        v_intQMin = GetQ(CInt(Mid(pv_strDate.Split("|")(0), 4, 2)))
        v_intQMax = GetQ(CInt(Mid(pv_strDate.Split("|")(1), 4, 2)))

        Dim v_blnStart As Boolean = True
        If v_intMinYear = v_intMaxYear Then
            While (v_intQMin <= v_intQMax)
                v_strPartion &= "Q" & v_intQMin & "Y" & v_intMinYear & "|"
                v_intQMin += 1
            End While
        Else
            For i As Integer = v_intMinYear To v_intMaxYear
                If v_blnStart Then
                    While v_intQMin <= 4
                        v_strPartion &= "Q" & v_intQMin & "Y" & i & "|"
                        v_intQMin += 1
                        v_blnStart = False
                    End While
                Else
                    v_intQMin = 1
                    If i = v_intMaxYear Then
                        While v_intQMin <= v_intQMax
                            v_strPartion &= "Q" & v_intQMin & "Y" & i & "|"
                            v_intQMin += 1
                        End While
                    Else
                        'v_intQMax = 4
                        While v_intQMin <= 4
                            v_strPartion &= "Q" & v_intQMin & "Y" & i & "|"
                            v_intQMin += 1
                        End While
                    End If
                End If
            Next
        End If
        Return v_strPartion
    End Function
#End Region

#Region "Ma hoa Message"
    Public Function Compression(ByVal pv_strMessage As String) As Byte()
        'Dim compressor As New XceedStreamingCompressionLib.XceedStreamingCompressionClass()
        'compressor.License(gc_ZipLicense)
        'compressor.CompressionFormat = New XceedStreamingCompressionLib.XceedBZip2CompressionFormatClass()
        'Try
        '    Dim compressedData As Object = compressor.Compress(pv_strMessage, True)
        '    Return CType(compressedData, Byte())
        'Catch except As System.Runtime.InteropServices.COMException
        '    Throw except
        'End Try
        Try
            Return ZetaCompressionLibrary.CompressionHelper.CompressString(pv_strMessage)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function Decompress(ByVal pv_bytMessage() As Byte) As String
        'Dim compressor As New XceedStreamingCompressionLib.XceedStreamingCompressionClass()
        'compressor.License(gc_ZipLicense)
        'compressor.CompressionFormat = New XceedStreamingCompressionLib.XceedBZip2CompressionFormatClass()
        'Try
        '    Dim decompressedData As Object = compressor.Decompress(pv_bytMessage, True)
        '    Return System.Text.Encoding.Unicode.GetString(CType(decompressedData, Byte()))
        'Catch except As System.Runtime.InteropServices.COMException
        '    Throw except
        'End Try
        Try
            Return ZetaCompressionLibrary.CompressionHelper.DecompressString(pv_bytMessage)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function EncryptString(ByVal pv_strUser As String, ByVal pv_strMessage As String) As String
        'Dim encrypt As New XceedEncryptionLib.XceedEncryptionClass()
        'encrypt.License(gc_XcryLicense)
        'Dim rijndael As New XceedEncryptionLib.XceedRijndaelEncryptionMethodClass()
        'Dim source As Object = pv_strMessage
        'Try
        '    rijndael.SetSecretKeyFromPassPhrase(pv_strUser, 128)
        '    encrypt.EncryptionMethod = rijndael
        '    Dim encryptedData As Object = encrypt.Encrypt(source, True)
        '    Return Convert.ToBase64String(CType(encryptedData, Byte()))
        'Catch except As System.Runtime.InteropServices.COMException
        '    Throw except
        'End Try
        Try
            Dim obj As New CryptoEngine
            Return obj.AES_Encrypt(pv_strMessage, pv_strUser)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function DecryptString(ByVal pv_strUser As String, ByVal pv_strMessage As String) As String
        'Dim encrypt As New XceedEncryptionLib.XceedEncryptionClass()
        'encrypt.License(gc_XcryLicense)
        'Dim rijndael As New XceedEncryptionLib.XceedRijndaelEncryptionMethodClass()
        'Try
        '    rijndael.SetSecretKeyFromPassPhrase(pv_strUser, 128)
        '    encrypt.EncryptionMethod = rijndael
        '    Dim v_objMessage As Object = Convert.FromBase64String(pv_strMessage)
        '    Return System.Text.Encoding.Unicode.GetString(CType(encrypt.Decrypt(v_objMessage, True), Byte()))
        'Catch except As System.Runtime.InteropServices.COMException
        '    Throw except
        'End Try
        Try
            Dim obj As New CryptoEngine
            Return obj.AES_Decrypt(pv_strMessage, pv_strUser)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function EncryptMessage(ByVal pv_strPublicKey() As Byte, ByRef pv_arrByteObjMsg() As Byte) As Byte()
        Dim encrypt As New XceedEncryptionLib.XceedEncryptionClass()
        'Dim v_seed As Object = vbNull
        encrypt.License(gc_XcryLicense)
        Dim v_RSA As New XceedEncryptionLib.XceedRSAEncryptionMethod
        Try
            'RSA.SetRandomKeyPair(1024, v_seed)
            v_RSA.PublicKey = pv_strPublicKey
            encrypt.EncryptionMethod = v_RSA

            Dim encryptedData As Object = encrypt.Encrypt(pv_arrByteObjMsg, True)
            Return CType(encryptedData, Byte())
        Catch except As System.Runtime.InteropServices.COMException
            Return Nothing
        End Try
    End Function

    Public Function DecryptMessage(ByVal pv_strPrivateKey() As Byte, ByVal v_arrByteObjMsg() As Byte) As Byte()
        Dim encrypt As New XceedEncryptionLib.XceedEncryptionClass()

        encrypt.License(gc_XcryLicense)
        Dim v_RSA As New XceedEncryptionLib.XceedRSAEncryptionMethod
        Dim v_seed As Object = vbNull
        Try
            v_RSA.PrivateKey = pv_strPrivateKey
            encrypt.EncryptionMethod = v_RSA
            'Dim v_encryptedData As Object = Convert.FromBase64String(v_strObjMsg)

            Dim decryptedData As Object = encrypt.Decrypt(v_arrByteObjMsg, True)
            Return CType(decryptedData, Byte())
        Catch except As System.Runtime.InteropServices.COMException
            Return Nothing
        End Try
    End Function

    Public Sub GetKey(ByVal pv_strTellerID As String, ByRef pv_arrBytePublicKey() As Byte, _
                           ByRef pv_arrBytePrivateKey() As Byte)
        Dim v_seed As Object = pv_strTellerID & Rnd()
        Dim v_RSA As New XceedEncryptionLib.XceedRSAEncryptionMethod
        Dim encrypt As New XceedEncryptionLib.XceedEncryptionClass()

        encrypt.License(gc_XcryLicense)
        v_RSA.SetRandomKeyPair(1024, v_seed)

        pv_arrBytePublicKey = CType(v_RSA.PublicKey, Byte())
        pv_arrBytePrivateKey = CType(v_RSA.PrivateKey, Byte())
    End Sub

    Public Sub EncryptFile(ByVal pv_strInFile As String, ByVal pv_strOutFile As String, ByVal pv_strTLName As String)
        'Dim encrypt As New XceedEncryptionLib.XceedEncryptionClass()
        'encrypt.License(gc_XcryLicense)
        'Dim rijndael As New XceedEncryptionLib.XceedRijndaelEncryptionMethodClass()
        'Try
        '    rijndael.SetSecretKeyFromPassPhrase(pv_strTLName, 128)
        '    encrypt.EncryptionMethod = rijndael
        '    Dim bytesRead As Object = Nothing
        '    encrypt.ProcessFile(pv_strInFile, 0, 0, XceedEncryptionLib.EXEFileProcessing.efpEncrypt, True, pv_strOutFile, False, bytesRead)
        'Catch ex As System.Runtime.InteropServices.COMException
        '    'Throw ex
        'End Try
        Try
            Dim obj As New CryptoEngine
            obj.EncryptOrDecryptFile(pv_strInFile, pv_strOutFile, pv_strTLName, CryptoAction.ActionEncrypt)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub DecryptFile(ByVal pv_strInFile As String, ByVal pv_strOutFile As String, ByVal pv_strTLName As String)
        'Dim encrypt As New XceedEncryptionLib.XceedEncryptionClass()
        'encrypt.License(gc_XcryLicense)
        'Dim rijndael As New XceedEncryptionLib.XceedRijndaelEncryptionMethodClass()
        'Try
        '    rijndael.SetSecretKeyFromPassPhrase(pv_strTLName, 128)
        '    encrypt.EncryptionMethod = rijndael
        '    Dim bytesRead As Object = Nothing
        '    encrypt.ProcessFile(pv_strInFile, 0, 0, XceedEncryptionLib.EXEFileProcessing.efpDecrypt, True, pv_strOutFile, False, bytesRead)
        'Catch ex As System.Runtime.InteropServices.COMException
        '    'Throw ex
        'End Try
        Try
            Dim obj As New CryptoEngine
            obj.EncryptOrDecryptFile(pv_strInFile, pv_strOutFile, pv_strTLName, CryptoAction.ActionDecrypt)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region
#Region "Convert type"
    ' BangPV: to convert a string to a byte array
    Public Function StrToByteArray(ByVal str As String) As Byte()
        Dim encoding As New System.Text.UTF8Encoding()
        Return encoding.GetBytes(str)
    End Function 'StrToByteArray


    ' BangPV: to convert a byte array to a string:
    Public Function ByteArrayToStr(ByVal dBytes As Byte()) As String
        Dim str As String
        Dim enc As New System.Text.UTF8Encoding()
        str = enc.GetString(dBytes)
        Return str
    End Function



#End Region
#Region "Chuyen so thanh chu"
    Public Function ReadMoney(ByVal sSoTien As String) As String
        Dim DonVi() As String = {"", "nghìn ", "triệu ", "tỷ ", "nghìn ", "triệu "}
        Dim so As String
        Dim chuoi As String = ""
        Dim temp As String
        Dim id As Byte

        Do While (Not sSoTien.Equals(""))
            If sSoTien.Length <> 0 Then
                so = getNum(sSoTien)
                sSoTien = Left(sSoTien, sSoTien.Length - so.Length)
                temp = setNum(so)
                so = temp
                If Not so.Equals("") Then
                    temp = temp + DonVi(id)
                    chuoi = temp + chuoi
                End If
                id = id + 1
            End If
        Loop
        temp = UCase(Left(chuoi, 1))
        Return temp & Right(chuoi, Len(chuoi) - 1) & "đồng."
    End Function
    Public Function ReadNumber(ByVal sSoTien As String) As String
        Dim DonVi() As String = {"", "nghìn ", "triệu ", "tỷ ", "nghìn ", "triệu "}
        Dim so As String
        Dim chuoi As String = ""
        Dim temp As String
        Dim id As Byte

        Do While (Not sSoTien.Equals(""))
            If sSoTien.Length <> 0 Then
                so = getNum(sSoTien)
                sSoTien = Left(sSoTien, sSoTien.Length - so.Length)
                temp = setNum(so)
                so = temp
                If Not so.Equals("") Then
                    temp = temp + DonVi(id)
                    chuoi = temp + chuoi
                End If
                id = id + 1
            End If
        Loop
        temp = UCase(Left(chuoi, 1))
        Return temp & Right(chuoi, Len(chuoi) - 1)
    End Function
    Private Function getNum(ByVal sSoTien As String) As String
        Dim so As String

        If sSoTien.Length >= 3 Then
            so = Right(sSoTien, 3)
        Else
            so = Right(sSoTien, sSoTien.Length)
        End If
        Return so
    End Function

    Private Function setNum(ByVal sSoTien As String) As String
        Dim chuoi As String = ""
        Dim flag0 As Boolean
        Dim flag1 As Boolean
        Dim temp As String

        temp = sSoTien
        Dim kyso() As String = {"không ", "một ", "hai ", "ba ", "bốn ", "năm ", "sáu ", "bảy ", "tám ", "chín "}
        'Xet hang tram
        If sSoTien.Length = 3 Then
            If Not (Left(sSoTien, 1) = 0 And Left(Right(sSoTien, 2), 1) = 0 And Right(sSoTien, 1) = 0) Then
                chuoi = kyso(Left(sSoTien, 1)) + "trăm "
            End If
            sSoTien = Right(sSoTien, 2)
        End If
        'Xet hang chuc
        If sSoTien.Length = 2 Then
            If Left(sSoTien, 1) = 0 Then
                If Right(sSoTien, 1) <> 0 Then
                    chuoi = chuoi + "linh "
                End If
                flag0 = True
            Else
                If Left(sSoTien, 1) = 1 Then
                    chuoi = chuoi + "mười "
                Else
                    chuoi = chuoi + kyso(Left(sSoTien, 1)) + "mươi "
                    flag1 = True
                End If
            End If
            sSoTien = Right(sSoTien, 1)
        End If
        'Xet hang don vi
        If Right(sSoTien, 1) <> 0 Then
            If Left(sSoTien, 1) = 5 And Not flag0 Then
                If temp.Length = 1 Then
                    chuoi = chuoi + "năm "
                Else
                    chuoi = chuoi + "lăm "
                End If
            Else
                If Left(sSoTien, 1) = 1 And Not (Not flag1 Or flag0) And chuoi <> "" Then
                    chuoi = chuoi + "mốt "
                Else
                    chuoi = chuoi + kyso(Left(sSoTien, 1)) + ""
                End If
            End If
        Else
        End If
        Return chuoi
    End Function
#End Region
#Region "Cache data"
    Public CachedTltx As XDocument
    Public CachedFldMaster As XDocument
    Public CachedSiSearch As XDocument
    Public CachedSiSearchFld As XDocument
    Public CachedFldVal As XDocument
    Public CachedRpReports As XDocument
    Public CachedRpFld As XDocument
    Public CachedRpGrp As XDocument
    Public CachedAllCode As XDocument
    Public CachedDefnationality As XDocument
    Public CachedCaType As XDocument
    Public CachedMaType As XDocument
    Public CachedRaType As XDocument
    Public CachedSfType As XDocument
    Public CachedAppModules As XDocument
    Public CachedCurrency As XDocument
    Public CachedRgIs As XDocument
    Public CachedMfType As XDocument
    'Public CachedGl_Code As XDocument
#End Region

End Module
