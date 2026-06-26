Imports System
Imports System.DirectoryServices
Imports System.Collections
Imports System.Data
Imports System.Data.SqlClient
Imports System.Text
Imports System.Security.Principal
Imports System.Xml.Serialization
Imports System.Reflection

<Serializable()> Public Class ADUser

#Region "private property variables"
    Private _FirstName As String  'givenName
    Private _MiddleInitial As String 'initials
    Private _LastName As String 'sn
    Private _DisplayName As String  'Name
    Private _UserPrincipalName As String 'userPrincipalName (e.g. user@domain.local)
    Private _PostalAddress As String
    Private _MailingAddress As String 'StreetAddress
    Private _ResidentialAddress As String 'HomePostalAddress
    Private _Title As String
    Private _HomePhone As String
    Private _OfficePhone As String  'TelephoneNumber
    Private _Mobile As String
    Private _Fax As String 'FacsimileTelephoneNumber
    Private _Email As String 'mail
    Private _Url As String
    Private _UserName As String  'sAMAccountName
    Private _Password As String
    Private _DistinguishedName As String
    Private _IsAccountActive As Boolean 'userAccountControl
    Private _Groups As ArrayList
#End Region

#Region "public Properties"
    Public Property FirstName() As String
        Get
            Return _FirstName
        End Get
        Set(ByVal Value As String)
            _FirstName = Value
        End Set
    End Property
    Public Property MiddleInitial() As String
        Get
            Return _MiddleInitial
        End Get
        Set(ByVal Value As String)
            If Value.Length > 6 Then
                Throw New Exception("MiddleInitial cannot be more than six characters")
            Else
                _MiddleInitial = Value
            End If
        End Set
    End Property
    Public Property LastName() As String
        Get
            Return _LastName
        End Get
        Set(ByVal Value As String)
            _LastName = Value
        End Set
    End Property

    Public ReadOnly Property DisplayName() As String
        Get
            _DisplayName = _FirstName & _MiddleInitial & "." & _LastName
            Return _DisplayName
        End Get

    End Property

    Public Property UserPrincipalName() As String
        Get
            Return _UserPrincipalName
        End Get
        Set(ByVal value As String)
            _UserPrincipalName = value
        End Set
    End Property

    Public Property PostalAddress() As String
        Get
            Return _PostalAddress
        End Get
        Set(ByVal Value As String)
            _PostalAddress = Value
        End Set
    End Property
    Public Property MailingAddress() As String
        Get
            Return _MailingAddress
        End Get
        Set(ByVal Value As String)
            _MailingAddress = Value
        End Set
    End Property

    Public Property ResidentialAddress() As String
        Get
            Return _ResidentialAddress
        End Get
        Set(ByVal Value As String)
            _ResidentialAddress = Value
        End Set
    End Property

    Public Property Title() As String
        Get
            Return _Title
        End Get
        Set(ByVal Value As String)
            _Title = Value
        End Set
    End Property
    Public Property HomePhone() As String
        Get
            Return _HomePhone
        End Get
        Set(ByVal Value As String)
            _HomePhone = Value
        End Set
    End Property

    Public Property OfficePhone() As String
        Get
            Return _OfficePhone
        End Get
        Set(ByVal Value As String)
            _OfficePhone = Value
        End Set
    End Property
    Public Property Mobile() As String
        Get
            Return _Mobile
        End Get
        Set(ByVal Value As String)
            _Mobile = Value
        End Set
    End Property
    Public Property Fax() As String
        Get
            Return _Fax
        End Get
        Set(ByVal Value As String)
            _Fax = Value
        End Set
    End Property
    Public Property Email() As String
        Get
            Return _Email
        End Get
        Set(ByVal Value As String)
            _Email = Value
        End Set
    End Property
    Public Property Url() As String
        Get
            Return _Url
        End Get
        Set(ByVal Value As String)
            _Url = Value
        End Set
    End Property
    Public Property UserName() As String
        Get
            Return _UserName
        End Get
        Set(ByVal Value As String)
            _UserName = Value
        End Set
    End Property
    Public Property Password() As String
        Get
            Return _Password
        End Get
        Set(ByVal Value As String)
            _Password = Value
        End Set
    End Property
    Public Property DistinguishedName() As String
        Get
            Return _DistinguishedName
        End Get
        Set(ByVal Value As String)
            _DistinguishedName = Value
        End Set
    End Property
    Public Property IsAccountActive() As Boolean
        Get
            Return _IsAccountActive
        End Get
        Set(ByVal Value As Boolean)
            _IsAccountActive = Value
        End Set
    End Property
    Public Property Groups() As ArrayList
        Get
            If _Groups Is Nothing Then
                _Groups = ADGroup.LoadByUser(DistinguishedName)
            End If
            Return _Groups
        End Get
        Set(ByVal Value As ArrayList)
            _Groups = Value
        End Set
    End Property
#End Region

#Region "friend Functions"
    Friend Shared Function LoadByGroup(ByVal DistinguishedName As String) As ArrayList
        Return GetUsers(DistinguishedName)
    End Function
#End Region

#Region "public Functions"
    Public Sub Update()
        Try
            Dim de As DirectoryEntry = GetDirectoyrObject(UserName)
            Utility.SetProperty(de, "givenName", FirstName)
            Utility.SetProperty(de, "initials", MiddleInitial)
            Utility.SetProperty(de, "sn", LastName)
            Utility.SetProperty(de, "UserPrincipalName", UserPrincipalName)
            Utility.SetProperty(de, "PostalAddress", PostalAddress)
            Utility.SetProperty(de, "StreetAddress", MailingAddress)
            Utility.SetProperty(de, "HomePostalAddress", ResidentialAddress)
            Utility.SetProperty(de, "Title", Title)
            Utility.SetProperty(de, "HomePhone", HomePhone)
            Utility.SetProperty(de, "TelephoneNumber", OfficePhone)
            Utility.SetProperty(de, "Mobile", Mobile)
            Utility.SetProperty(de, "FacsimileTelephoneNumber", Fax)
            Utility.SetProperty(de, "mail", Email)
            Utility.SetProperty(de, "Url", Url)
            If IsAccountActive = True Then
                de.Properties("userAccountControl")(0) = Utility.UserStatus.Enable
            Else
                de.Properties("userAccountControl")(0) = Utility.UserStatus.Disable
            End If
            de.CommitChanges()
        Catch ex As Exception
            Throw New Exception("User cannot be updated" & ex.Message)
        End Try
    End Sub
    Public Sub SetPassword(ByVal newPassword As String)
        Try
            Dim de As DirectoryEntry = GetDirectoyrObject(UserName)
            Utility.SetUserPassword(de, newPassword)
            de.CommitChanges()
        Catch ex As Exception
            Throw New Exception("User Password cannot be set" & ex.Message)
        End Try
    End Sub
#End Region

#Region "private Functions"
    Private Function GetDirectoyrObject(ByVal UserName As String) As DirectoryEntry
        Dim de As DirectoryEntry = Utility.GetDirectoryObject()
        Dim deSearch As DirectorySearcher = New DirectorySearcher
        deSearch.SearchRoot = de
        deSearch.Filter = "(&(objectClass=user)(sAMAccountName=" + UserName + "))"
        deSearch.SearchScope = SearchScope.Subtree
        Dim results As SearchResult = deSearch.FindOne()
        If Not (results Is Nothing) Then
            de = New DirectoryEntry(results.Path, Utility.ADUser, Utility.ADPassword, AuthenticationTypes.Secure)
            Return de
        Else
            Return Nothing
        End If
    End Function
    Private Shared Function GetUsers(ByVal DistinguishedName As String) As ArrayList
        Dim _de As DirectoryEntry = Utility.GetDirectoryObjectByDistinguishedName(DistinguishedName)
        Dim index As Integer
        Dim list As New ArrayList
        For index = 0 To _de.Properties("member").Count - 1
            list.Add(Load(Utility.GetDirectoryObjectByDistinguishedName(Utility.ADPath & "/" & _de.Properties("member")(index).ToString())))
        Next
        Return list
    End Function
    Private Shared Function Load(ByVal de As DirectoryEntry) As ADUser
        Dim ADUser As New ADUser
        ADUser.FirstName = Utility.GetProperty(de, "givenName")
        ADUser.MiddleInitial = Utility.GetProperty(de, "initials")
        ADUser.LastName = Utility.GetProperty(de, "sn")
        ADUser.UserPrincipalName = Utility.GetProperty(de, "UserPrincipalName")
        ADUser.PostalAddress = Utility.GetProperty(de, "PostalAddress")
        ADUser.MailingAddress = Utility.GetProperty(de, "StreetAddress")
        ADUser.ResidentialAddress = Utility.GetProperty(de, "HomePostalAddress")
        ADUser.Title = Utility.GetProperty(de, "Title")
        ADUser.HomePhone = Utility.GetProperty(de, "HomePhone")
        ADUser.OfficePhone = Utility.GetProperty(de, "TelephoneNumber")
        ADUser.Mobile = Utility.GetProperty(de, "Mobile")
        ADUser.Fax = Utility.GetProperty(de, "FacsimileTelephoneNumber")
        ADUser.Email = Utility.GetProperty(de, "mail")
        ADUser.Url = Utility.GetProperty(de, "Url")
        ADUser.UserName = Utility.GetProperty(de, "sAMAccountName")
        ADUser.DistinguishedName = Utility.ADPath & "/" & Utility.GetProperty(de, "DistinguishedName")
        ADUser.IsAccountActive = Utility.IsAccountActive(Convert.ToInt32(Utility.GetProperty(de, "userAccountControl")))
        Return ADUser
    End Function
    Private Function Load(ByVal deCollection As DirectoryEntries) As ArrayList
        Dim list As New ArrayList
        Dim de As DirectoryEntry

        For Each de In deCollection
            list.Add(Load(de))
        Next
        Return list
    End Function
#End Region

End Class

