<Serializable()> _
Public Class XmlDocumentEx
    Inherits Xml.XmlDocument

    Public Overrides Function CloneNode(ByVal deep As Boolean) As System.Xml.XmlNode
        MyBase.CloneNode(deep)
    End Function

    Public Overrides Sub WriteContentTo(ByVal w As System.Xml.XmlWriter)
        MyBase.WriteContentTo(w)
    End Sub

    Public Overrides Sub WriteTo(ByVal w As System.Xml.XmlWriter)
        MyBase.WriteTo(w)
    End Sub
End Class
