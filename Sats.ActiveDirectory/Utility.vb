Imports System.IO
Imports System.Security.Cryptography
Imports System.Security.Permissions ' For RegistryPermission
Imports System.Text
Imports System.Globalization
Imports System
Imports System.DirectoryServices
Imports System.Collections
Imports System.Data
Imports System.Data.SqlClient
Imports System.Security.Principal
Imports System.Configuration.ConfigurationManager

    Public Class Utility

#Region "public Constant Variables"
        Public Const DefaultDate As DateTime = #12/30/1899#
        Public Shared ADPath As String = AppSettings.Get("ADPAth")
        Public Shared ADUser As String = AppSettings.Get("ADUser")
        Public Shared ADPassword As String = AppSettings.Get("ADPassword")
        Public Shared ADUsersPath As String = AppSettings.Get("ADUsersPath")
#End Region

#Region "private functions"
    Friend Shared Function GetDirectoryObject(ByVal UserName As String, ByVal Password As String) As DirectoryEntry
        Dim oDE As DirectoryEntry
        oDE = New DirectoryEntry(ADPath, UserName, Password, AuthenticationTypes.Secure)
        Return oDE
    End Function
#End Region

#Region "Enums"
        Public Enum LoginResult
            LOGIN_OK = 0
            LOGIN_USER_DOESNT_EXIST
            LOGIN_USER_ACCOUNT_INACTIVE
        End Enum
        Friend Enum UserStatus
            Enable = 544
            Disable = 546
        End Enum
        Friend Enum GroupScope
            ADS_GROUP_TYPE_DOMAIN_LOCAL_GROUP = -2147483644
            ADS_GROUP_TYPE_GLOBAL_GROUP = -2147483646
            ADS_GROUP_TYPE_UNIVERSAL_GROUP = -2147483640
        End Enum
        Friend Enum ADAccountOptions
            UF_TEMP_DUPLICATE_ACCOUNT = 256
            UF_NORMAL_ACCOUNT = 512
            UF_INTERDOMAIN_TRUST_ACCOUNT = 2048
            UF_WORKSTATION_TRUST_ACCOUNT = 4096
            UF_SERVER_TRUST_ACCOUNT = 8192
            UF_DONT_EXPIRE_PASSWD = 65536
            UF_SCRIPT = 1
            UF_ACCOUNTDISABLE = 2
            UF_HOMEDIR_REQUIRED = 8
            UF_LOCKOUT = 16
            UF_PASSWD_NOTREQD = 32
            UF_PASSWD_CANT_CHANGE = 64
            UF_ACCOUNT_LOCKOUT = 16
            UF_ENCRYPTED_TEXT_PASSWORD_ALLOWED = 128
        End Enum

#End Region

#Region "Friend Functions"
        Friend Shared Function GetUser(ByVal UserName As String, ByVal Password As String) As DirectoryEntry
            Dim de As DirectoryEntry = GetDirectoryObject(UserName, Password)
            Dim deSearch As DirectorySearcher = New DirectorySearcher
            deSearch.SearchRoot = de
            deSearch.Filter = "(&(objectClass=user)(sAMAccountName=" + UserName + "))"
            deSearch.SearchScope = SearchScope.Subtree
            Dim results As SearchResult = deSearch.FindOne()
            If Not (results Is Nothing) Then
                de = New DirectoryEntry(results.Path, ADUser, ADPassword, AuthenticationTypes.Secure)
                Return de
            Else
                Return Nothing
            End If
        End Function
        Friend Shared Sub EnableUserAccount(ByVal oDE As DirectoryEntry)
            oDE.Properties("userAccountControl").Value = UserStatus.Enable
            oDE.CommitChanges()
            oDE.Close()
        End Sub
        Friend Shared Function GetGroup(ByVal Name As String) As DirectoryEntry
            Dim de As DirectoryEntry = Utility.GetDirectoryObject()
            Dim deSearch As DirectorySearcher = New DirectorySearcher
            deSearch.SearchRoot = de
            deSearch.Filter = "(&(objectClass=group)(cn=" + Name + "))"
            deSearch.SearchScope = SearchScope.Subtree
            Dim results As SearchResult = deSearch.FindOne()
            If Not (results Is Nothing) Then
                de = New DirectoryEntry(results.Path, Utility.ADUser, Utility.ADPassword, AuthenticationTypes.Secure)
                Return de
            Else
                Return Nothing
            End If
        End Function
       
        Friend Shared Sub DisableUserAccount(ByVal oDE As DirectoryEntry)
            oDE.Properties("userAccountControl").Value = UserStatus.Disable
            oDE.CommitChanges()
            oDE.Close()
        End Sub
        Friend Shared Function GetDirectoryObject(ByVal DomainReference As String) As DirectoryEntry
            Dim oDE As DirectoryEntry
            oDE = New DirectoryEntry(Utility.ADPath + DomainReference, Utility.ADUser, Utility.ADPassword, AuthenticationTypes.Secure)
            Return oDE
        End Function

        Friend Shared Sub SetUserPassword(ByVal oDE As DirectoryEntry, ByVal Password As String)
            oDE.Invoke("SetPassword", New Object() {Password})
        End Sub

        Friend Shared Function IsAccountActive(ByVal userAccountControl As Integer) As Boolean
            Dim userAccountControl_Disabled As Integer = Convert.ToInt32(ADAccountOptions.UF_ACCOUNTDISABLE)
            Dim flagExists As Integer = userAccountControl And userAccountControl_Disabled
            If flagExists > 0 Then
                Return False
            Else
                Return True
            End If
        End Function

        Friend Shared Function GetLDAPDomain() As String
            Dim LDAPDomain As StringBuilder = New StringBuilder
            Dim serverName As String = "k2mega.local"
            Dim LDAPDC As String() = serverName.Split(CType(".", Char))
            Dim index As Integer = 0
            While index < LDAPDC.GetUpperBound(0) + 1
                LDAPDomain.Append("DC=" + LDAPDC(index))
                If index < LDAPDC.GetUpperBound(0) Then
                    LDAPDomain.Append(",")
                End If
                index += 1
            End While
            Return LDAPDomain.ToString()
        End Function
        Friend Shared Function GetDirectoryObjectByDistinguishedName(ByVal ObjectPath As String) As DirectoryEntry
            Dim oDE As DirectoryEntry
            oDE = New DirectoryEntry(ObjectPath, Utility.ADUser, Utility.ADPassword, AuthenticationTypes.Secure)
            Return oDE
        End Function
        Friend Shared Sub SetProperty(ByVal oDE As DirectoryEntry, ByVal PropertyName As String, ByVal PropertyValue As String)
            If Not (PropertyValue = String.Empty) Then
                If oDE.Properties.Contains(PropertyName) Then
                    oDE.Properties(PropertyName)(0) = PropertyValue
                Else
                    oDE.Properties(PropertyName).Add(PropertyValue)
                End If
            End If
        End Sub
        Friend Shared Function GetDirectoryObject() As DirectoryEntry
            Dim oDE As DirectoryEntry
            oDE = New DirectoryEntry(Utility.ADPath, Utility.ADUser, Utility.ADPassword, AuthenticationTypes.Secure)
            Return oDE
        End Function
        Friend Shared Function GetProperty(ByVal oDE As DirectoryEntry, ByVal PropertyName As String) As String
            If oDE.Properties.Contains(PropertyName) Then
                Return oDE.Properties(PropertyName)(0).ToString()
            Else
                Return String.Empty
            End If
        End Function
#End Region

    End Class
