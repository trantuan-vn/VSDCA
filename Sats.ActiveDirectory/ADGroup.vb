Imports System
Imports System.DirectoryServices
Imports System.Collections
Imports System.Data
Imports System.Data.SqlClient
Imports System.Text
Imports System.Security.Principal
Imports System.Xml.Serialization
Imports System.Reflection

    <Serializable()> Public Class ADGroup

#Region "private property variables"
        Private _Name As String 'cn
        Private _DisplayName As String
        Private _DistinguishedName As String
        Private _Description As String
        Private _Users As ArrayList
#End Region

#Region "public Properties"
        Public Property Name() As String
            Get
                Return _Name
            End Get
            Set(ByVal Value As String)
                _Name = Value
            End Set
        End Property

        Public Property DisplayName() As String
            Get
                Return _DisplayName
            End Get
            Set(ByVal Value As String)
                _DisplayName = Value
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

        Public Property Description() As String
            Get
                Return _Description
            End Get
            Set(ByVal Value As String)
                _Description = Value
            End Set
        End Property
        Public Property Users() As ArrayList
            Get
                If _Users Is Nothing Then
                    _Users = ADUser.LoadByGroup(DistinguishedName)
                End If
                Return _Users
            End Get
            Set(ByVal Value As ArrayList)
                _Users = Value
            End Set
        End Property
#End Region

#Region "friend Functions"
        Friend Shared Function LoadByUser(ByVal DistinguishedName As String) As ArrayList
            Return GetGroups(DistinguishedName)
        End Function
#End Region

#Region "public procedure"
        Public Sub Update()
            Try
                Dim de As DirectoryEntry = Utility.GetGroup(Name)
                Utility.SetProperty(de, "DisplayName", DisplayName)
                Utility.SetProperty(de, "Description", Description)
                de.CommitChanges()
            Catch ex As Exception
                Throw New Exception("User cannot be updated" & ex.Message)
            End Try
        End Sub
#End Region

#Region "private Functions"
        Private Shared Function GetGroups(ByVal DistinguishedName As String) As ArrayList
            Dim _de As DirectoryEntry = Utility.GetDirectoryObjectByDistinguishedName(DistinguishedName)
            Dim index As Integer
            Dim list As New ArrayList
            For index = 0 To _de.Properties("memberOf").Count - 1
                list.Add(Load(Utility.GetDirectoryObjectByDistinguishedName(Utility.ADPath & "/" & _de.Properties("memberOf")(index).ToString())))
            Next
            Return list
        End Function

        Private Shared Function Load(ByVal de As DirectoryEntry) As ADGroup
            Dim _ADGroup As New ADGroup
            _ADGroup.Name = Utility.GetProperty(de, "cn")
            _ADGroup.DisplayName = Utility.GetProperty(de, "DisplayName")
            _ADGroup.DistinguishedName = Utility.ADPath & "/" & Utility.GetProperty(de, "DistinguishedName")
            _ADGroup.Description = Utility.GetProperty(de, "Description")
            Return _ADGroup
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
