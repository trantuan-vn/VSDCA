Imports Xceed.Grid

<Serializable()> _
Public Class GridEx
    Inherits GridControl

    Public Sub New(ByVal pv_strTable As String, ByVal pv_strResource As String, ByVal pv_strTellerID As String)
        MyBase.New()

        'License
        Licenser.LicenseKey = "GRD11-XU1NZ-BZ1HW-AKCA"

        'Thiết lập một số thuộc tính chung nhất của GRID
        _FormatGridBefore(Me, pv_strTellerID, pv_strTable, pv_strResource)
    End Sub

    Public Sub New()
        MyBase.New()

        'License
        Licenser.LicenseKey = "GRD11-XU1NZ-BZ1HW-AKCA"

        'Thiết lập một số thuộc tính chung nhất của GRID
        _FormatGridBefore(Me, "", , , False, False)
    End Sub
End Class
