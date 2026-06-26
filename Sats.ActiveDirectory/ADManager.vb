Imports System
Imports System.DirectoryServices
Imports System.Collections
Imports System.Data
Imports System.Data.SqlClient
Imports System.Text
Imports System.Security.Principal

Public Class ADManager
    Private Shared _instance As ADManager

    Private Sub New()
    End Sub

    Public Shared ReadOnly Property Instance() As ADManager
        Get
            If _instance Is Nothing Then
                _instance = New ADManager
            End If
            Return _instance
        End Get
    End Property

#Region "private Properties"
    Private Function Load(ByVal deCollection As DirectoryEntries) As ArrayList
        Dim list As New ArrayList
        Dim de As DirectoryEntry

        For Each de In deCollection
            list.Add(Load(de))
        Next
        Return list
    End Function
    Private Function Load(ByVal seCollection As SearchResultCollection) As ArrayList
        Dim list As New ArrayList
        Dim se As SearchResult

        For Each se In seCollection
            list.Add(Load(New DirectoryEntry(se.Path, Utility.ADUser, Utility.ADPassword, AuthenticationTypes.Secure)))
        Next
        Return list
    End Function
    Private Function LoadGroup(ByVal seCollection As SearchResultCollection) As ArrayList
        Dim list As New ArrayList
        Dim se As SearchResult
        For Each se In seCollection
            list.Add(LoadGroup(New DirectoryEntry(se.Path, Utility.ADUser, Utility.ADPassword, AuthenticationTypes.Secure)))
        Next
        Return list
    End Function
    Private Function LoadGroup(ByVal de As DirectoryEntry) As ADGroup
        Dim _ADGroup As New ADGroup
        _ADGroup.Name = Utility.GetProperty(de, "cn")
        _ADGroup.DisplayName = Utility.GetProperty(de, "DisplayName")
        _ADGroup.DistinguishedName = Utility.ADPath & "/" & Utility.GetProperty(de, "DistinguishedName")
        _ADGroup.Description = Utility.GetProperty(de, "Description")
        Return _ADGroup
    End Function

    Private Shared Function GetUsers() As SearchResultCollection
        Dim de As DirectoryEntry = Utility.GetDirectoryObject()
        Dim deSearch As DirectorySearcher = New DirectorySearcher
        deSearch.SearchRoot = de
        deSearch.Filter = "(&(objectClass=user)(objectCategory=person))"
        deSearch.SearchScope = SearchScope.Subtree
        Return deSearch.FindAll()
    End Function
    Private Shared Function GetGroups() As SearchResultCollection
        Dim dsGroup As DataSet = New DataSet
        Dim de As DirectoryEntry = Utility.GetDirectoryObject()
        Dim deSearch As DirectorySearcher = New DirectorySearcher
        deSearch.SearchRoot = de
        deSearch.Filter = "(&(objectClass=group))"
        Return deSearch.FindAll()

    End Function
    Private Shared Function GetUser(ByVal UserName As String) As DirectoryEntry
        Dim de As DirectoryEntry = Utility.GetDirectoryObject()
        Dim deSearch As DirectorySearcher = New DirectorySearcher
        deSearch.SearchRoot = de
        deSearch.Filter = "(SAMAccountName=" + UserName + ")" '"(&(objectClass=user)(CN=" + UserName + "))"
        deSearch.SearchScope = SearchScope.Subtree
        Dim results As SearchResult = deSearch.FindOne()
        If Not (results Is Nothing) Then
            de = New DirectoryEntry(results.Path, Utility.ADUser, Utility.ADPassword, AuthenticationTypes.Secure)
            Return de
        Else
            Return Nothing
        End If
    End Function

    Private Function Load(ByVal de As DirectoryEntry) As ADUser
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
    Private Function AddGroup(ByVal Name As String, ByVal DisplayName As String, ByVal DistinguishedName As String, ByVal Description As String) As DirectoryEntry

        Dim RootDSE As String
        Dim DSESearcher As System.DirectoryServices.DirectorySearcher = New System.DirectoryServices.DirectorySearcher
        Try
            RootDSE = DSESearcher.SearchRoot.Path
            RootDSE = RootDSE.Insert(7, Utility.ADUsersPath)
            Dim myDE As System.DirectoryServices.DirectoryEntry = New DirectoryEntry(RootDSE)
            Dim myEntries As DirectoryEntries = myDE.Children
            Dim myDirectoryEntry As System.DirectoryServices.DirectoryEntry = myEntries.Add("CN=" + Name, "Group")
            Utility.SetProperty(myDirectoryEntry, "cn", Name)
            Utility.SetProperty(myDirectoryEntry, "DisplayName", DisplayName)
            Utility.SetProperty(myDirectoryEntry, "Description", Description)
            Utility.SetProperty(myDirectoryEntry, "sAMAccountName", Name)
            Utility.SetProperty(myDirectoryEntry, "groupType", CType(Utility.GroupScope.ADS_GROUP_TYPE_GLOBAL_GROUP, String))
            myDirectoryEntry.CommitChanges()
            myDirectoryEntry = Utility.GetGroup(Name)
            Return myDirectoryEntry
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function Add(ByVal FirstName As String, ByVal MiddleInitial As String, ByVal LastName As String, ByVal UserPrincipalName As String, ByVal PostalAddress As String, ByVal MailingAddress As String, ByVal ResidentialAddress As String, ByVal Title As String, ByVal HomePhone As String, ByVal OfficePhone As String, ByVal Mobile As String, ByVal Fax As String, ByVal Email As String, ByVal Url As String, ByVal UserName As String, ByVal Password As String, ByVal DistinguishedName As String, ByVal IsAccountActive As Boolean) As DirectoryEntry


        Dim RootDSE As String
        Dim DSESearcher As System.DirectoryServices.DirectorySearcher = New System.DirectoryServices.DirectorySearcher
        Try
            RootDSE = DSESearcher.SearchRoot.Path
            RootDSE = RootDSE.Insert(7, Utility.ADUsersPath)
            Dim myDE As System.DirectoryServices.DirectoryEntry = New DirectoryEntry(RootDSE)
            Dim myEntries As DirectoryEntries = myDE.Children
            Dim myDirectoryEntry As System.DirectoryServices.DirectoryEntry = myEntries.Add("CN=" + UserName, "user")
            Utility.SetProperty(myDirectoryEntry, "givenName", FirstName)
            Utility.SetProperty(myDirectoryEntry, "initials", MiddleInitial)
            Utility.SetProperty(myDirectoryEntry, "sn", LastName)
            If UserPrincipalName <> "" Then
                Utility.SetProperty(myDirectoryEntry, "UserPrincipalName", UserPrincipalName)
            Else
                Utility.SetProperty(myDirectoryEntry, "UserPrincipalName", UserName)
            End If
            Utility.SetProperty(myDirectoryEntry, "PostalAddress", PostalAddress)
            Utility.SetProperty(myDirectoryEntry, "StreetAddress", MailingAddress)
            Utility.SetProperty(myDirectoryEntry, "HomePostalAddress", ResidentialAddress)
            Utility.SetProperty(myDirectoryEntry, "Title", Title)
            Utility.SetProperty(myDirectoryEntry, "HomePhone", HomePhone)
            Utility.SetProperty(myDirectoryEntry, "TelephoneNumber", OfficePhone)
            Utility.SetProperty(myDirectoryEntry, "Mobile", Mobile)
            Utility.SetProperty(myDirectoryEntry, "FacsimileTelephoneNumber", Fax)
            Utility.SetProperty(myDirectoryEntry, "mail", Email)
            Utility.SetProperty(myDirectoryEntry, "Url", Url)
            Utility.SetProperty(myDirectoryEntry, "sAMAccountName", UserName)
            Utility.SetProperty(myDirectoryEntry, "UserPassword", Password)
            myDirectoryEntry.Properties("userAccountControl").Value = Utility.UserStatus.Enable
            myDirectoryEntry.CommitChanges()
            myDirectoryEntry = GetUser(UserName)
            Utility.SetUserPassword(myDirectoryEntry, Password)
            Return myDirectoryEntry
        Catch ex As Exception
            Throw ex
        End Try
    End Function

#End Region

#Region "public Properties"
    Public Function LoadUser(ByVal userName As String) As ADUser
        Return Load(GetUser(userName))
    End Function
    Public Function LoadGroup(ByVal GroupName As String) As ADGroup
        Return LoadGroup(Utility.GetGroup(GroupName))
    End Function
    Public Function LoadAllUsers() As ArrayList
        Return Load(GetUsers())
    End Function
    Public Function LoadAllGroups() As ArrayList
        Return Load(GetGroups())
    End Function
    Public Function CreateADUser(ByVal ADUser As ADUser) As ADUser
        Return Load(Add(ADUser.FirstName, ADUser.MiddleInitial, ADUser.LastName, ADUser.UserPrincipalName, ADUser.PostalAddress, ADUser.MailingAddress, ADUser.ResidentialAddress, ADUser.Title, ADUser.HomePhone, ADUser.OfficePhone, ADUser.Mobile, ADUser.Fax, ADUser.Email, ADUser.Url, ADUser.UserName, ADUser.Password, ADUser.DistinguishedName, ADUser.IsAccountActive))
    End Function
    Public Function CreateADGroup(ByVal ADGroup As ADGroup) As ADGroup
        Return LoadGroup(AddGroup(ADGroup.Name, ADGroup.DisplayName, ADGroup.DistinguishedName, ADGroup.Description))
    End Function
    Public Shared Sub AddUserToGroup(ByVal UserDistinguishedName As String, ByVal GroupDistinguishedName As String)
        Dim oGroup As DirectoryEntry = Utility.GetDirectoryObjectByDistinguishedName(GroupDistinguishedName)
        oGroup.Invoke("Add", New Object() {UserDistinguishedName})
        oGroup.Close()
    End Sub
    Public Shared Sub RemoveUserFromGroup(ByVal UserDistinguishedName As String, ByVal GroupDistinguishedName As String)
        Dim oGroup As DirectoryEntry = Utility.GetDirectoryObjectByDistinguishedName(GroupDistinguishedName)
        oGroup.Invoke("Remove", New Object() {UserDistinguishedName})
        oGroup.Close()
    End Sub
    Public Shared Sub DisableUserAccount(ByVal UserName As String)
        Utility.DisableUserAccount(GetUser(UserName))
    End Sub
    Public Shared Sub EnableUserAccount(ByVal UserName As String)
        Utility.EnableUserAccount(GetUser(UserName))
    End Sub
    Public Shared Sub DeleteUserAccount(ByVal UserName As String)

        Dim de As DirectoryEntry = GetUser(UserName)
        de.DeleteTree()
        de.CommitChanges()
    End Sub
    Public Shared Sub DeleteGroupAccount(ByVal GroupName As String)
        Dim de As DirectoryEntry = Utility.GetGroup(GroupName)
        de.DeleteTree()
        de.CommitChanges()
    End Sub
    Public Shared Function IsUserValid(ByVal UserName As String, ByVal Password As String) As Boolean
        Try
            Dim deUser As DirectoryEntry = Utility.GetUser(UserName, Password)
            deUser.Close()
            Return True
        Catch generatedExceptionVariable0 As Exception
            Return False
        End Try
    End Function
    Public Shared Function UserExists(ByVal UserName As String) As Boolean
        Dim de As DirectoryEntry = Utility.GetDirectoryObject()
        Dim deSearch As DirectorySearcher = New DirectorySearcher
        deSearch.SearchRoot = de
        deSearch.Filter = "(&(objectClass=user) (cn=" + UserName + "))"
        Dim results As SearchResultCollection = deSearch.FindAll()
        If results.Count = 0 Then
            Return False
        Else
            Return True
        End If
    End Function

    Public Shared Function GroupExists(ByVal GroupName As String) As Boolean
        Dim de As DirectoryEntry = Utility.GetDirectoryObject()
        Dim deSearch As DirectorySearcher = New DirectorySearcher
        deSearch.SearchRoot = de
        deSearch.Filter = "(&(objectClass=group) (cn=" + GroupName + "))"
        Dim results As SearchResultCollection = deSearch.FindAll()
        If results.Count = 0 Then
            Return False
        Else
            Return True
        End If
    End Function

    Public Shared Function Login(ByVal UserName As String, ByVal Password As String, ByVal pv_strIPAddress As String) As Utility.LoginResult
        If IsUserValid(UserName, Password) Then
            Dim de As DirectoryEntry = GetUser(UserName)
            If Not (de Is Nothing) Then
                Dim accountControl As Integer = Convert.ToInt32(de.Properties("userAccountControl")(0))
                If Not Utility.IsAccountActive(accountControl) Then
                    Return Utility.LoginResult.LOGIN_USER_ACCOUNT_INACTIVE
                Else
                    Dim v_strIPList As String = Utility.GetProperty(de, "StreetAddress")
                    Dim v_strIPAddress As String = ""
                    If v_strIPList <> "" Then
                        Dim v_arrIPList() As String = v_strIPList.Split(";")
                        For i As Integer = 0 To v_arrIPList.Count - 1
                            If Mid(v_arrIPList(i), v_arrIPList(i).LastIndexOf(".") + 2) = "*" Then
                                v_arrIPList(i) = Mid(v_arrIPList(i), 1, v_arrIPList(i).LastIndexOf(".") + 1) & Mid(pv_strIPAddress, pv_strIPAddress.LastIndexOf(".") + 2)
                            End If
                        Next
                        If v_arrIPList.Contains(pv_strIPAddress) Then
                            Return Utility.LoginResult.LOGIN_OK
                        Else
                            Return Utility.LoginResult.LOGIN_USER_DOESNT_EXIST
                        End If
                    Else
                        Return Utility.LoginResult.LOGIN_OK
                    End If
                End If
                de.Close()
            Else
                Return Utility.LoginResult.LOGIN_USER_DOESNT_EXIST
            End If
        Else
            Return Utility.LoginResult.LOGIN_USER_DOESNT_EXIST
        End If
    End Function

#End Region

End Class

