Module modSats
    Public Const gc_RegistryKey = "Software\FIS\SATS"
    Public Const gc_RootNamespace = "SATS"

#Region " Các hằng số Value trong registry "
    Public Const gc_REG_USERNAME = "UserName"
    Public Const gc_REG_PASSWORD = "Password"

    Public Const gc_REG_LANG = "Language"

    Public Const gc_REG_HEIGHT = "Height"
    Public Const gc_REG_WIDTH = "Width"
    Public Const gc_REG_WINSTATE = "WindowState"
#End Region

#Region " Các hằng số mã lỗi hệ thống "
    Public Const gc_SYSERR_BAD_URL = "SYS_000001"
    Public Const gc_SYSERR_SVR_ERROR = "SYS_000002"
    Public Const gc_SYSERR_CONTACT_NET_ADMIN = "SYS_000003"
    Public Const gc_SYSERR_SRV_UNREACHABLE = "SYS_000004"
    Public Const gc_SYSERR_CHECK_CONNECTION = "SYS_000005"
    Public Const gc_SYSERR_INCORRECT_USR_OR_PWD = "SYS_000006"
    Public Const gc_SYSERR_INCORRECT_IPADDRESS = "SYS_000010"
    Public Const gc_SYSERR_RE_TYPE = "SYS_000007"
    Public Const gc_SYSERR_UNKNOWN_ERROR = "SYS_000008"
    Public Const gc_SYSERR_CHECK_EVENT_LOG = "SYS_000009"
    Public Const gc_SYSERR_UNPLUGGED_USB = "SYS_000010"
    Public Const gc_SYSERR_WRONG_CERTIFICATE = "SYS_000011"
#End Region

End Module
