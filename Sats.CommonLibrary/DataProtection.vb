Imports System
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Security

<Serializable()> _
Public NotInheritable Class DataProtection

    <Flags()> _
    Public Enum CryptProtectPromptFlags
        CRYPTPROTECT_PROMPT_ON_UNPROTECT = &H1
        CRYPTPROTECT_PROMPT_ON_PROTECT = &H2
        CRYPTPROTECT_PROMPT_RESERVED = &H4
        CRYPTPROTECT_PROMPT_STRONG = &H8
        CRYPTPROTECT_PROMPT_REQUIRE_STRONG = &H10
    End Enum

    <Flags()> _
    Public Enum CryptProtectDataFlags
        CRYPTPROTECT_UI_FORBIDDEN = &H1
        CRYPTPROTECT_LOCAL_MACHINE = &H4
        CRYPTPROTECT_CRED_SYNC = &H8
        CRYPTPROTECT_AUDIT = &H10
        CRYPTPROTECT_NO_RECOVERY = &H20
        CRYPTPROTECT_VERIFY_PROTECTION = &H40
        CRYPTPROTECT_CRED_REGENERATE = &H80
    End Enum

    Public Shared Function ProtectData(ByVal data As String, ByVal name As String) As String
        Return ProtectData(data, name, CryptProtectDataFlags.CRYPTPROTECT_UI_FORBIDDEN Or CryptProtectDataFlags.CRYPTPROTECT_LOCAL_MACHINE)
    End Function

    Public Shared Function ProtectData(ByVal data As Byte(), ByVal name As String) As Byte()
        Return ProtectData(data, name, CryptProtectDataFlags.CRYPTPROTECT_UI_FORBIDDEN Or CryptProtectDataFlags.CRYPTPROTECT_LOCAL_MACHINE)
    End Function

    Public Shared Function ProtectData(ByVal data As String, ByVal name As String, ByVal flags As CryptProtectDataFlags) As String
        Dim dataIn As Byte() = Encoding.Unicode.GetBytes(data)
        Dim dataOut As Byte() = ProtectData(dataIn, name, flags)

        If Not dataOut Is Nothing Then
            Return (Convert.ToBase64String(dataOut))
        Else
            Return Nothing
        End If
    End Function

    Private Shared Function ProtectData(ByVal data As Byte(), ByVal name As String, ByVal dwFlags As CryptProtectDataFlags) As Byte()
        Dim cipherText As Byte() = Nothing

        'copy data into unmanaged memory
        Dim din As New DPAPI.DATA_BLOB()
        din.cbData = data.Length
        din.pbData = Marshal.AllocHGlobal(din.cbData)

        If din.pbData.Equals(IntPtr.Zero) Then Throw New OutOfMemoryException("Unable to allocate memory for buffer.")

        Marshal.Copy(data, 0, din.pbData, din.cbData)

        Dim dout As New DPAPI.DATA_BLOB()

        Try
            Dim cryptoRetval As Boolean = DPAPI.CryptProtectData(din, name, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, dwFlags, dout)

            If cryptoRetval Then 'ERROR_SUCCESS?
                Dim startIndex As Integer = 0
                ReDim cipherText(dout.cbData)
                Marshal.Copy(dout.pbData, cipherText, startIndex, dout.cbData)
                DPAPI.LocalFree(dout.pbData)
            Else
                Dim errCode As Integer = Marshal.GetLastWin32Error()
                Dim buffer As New StringBuilder(256)
                Win32Error.FormatMessage(Win32Error.FormatMessageFlags.FORMAT_MESSAGE_FROM_SYSTEM, IntPtr.Zero, errCode, 0, buffer, buffer.Capacity, IntPtr.Zero)
            End If
        Finally
            'free the allocated memroy in use
            If Not din.pbData.Equals(IntPtr.Zero) Then Marshal.FreeHGlobal(din.pbData)
        End Try

        Return cipherText

    End Function

    Public Shared Function UnprotectData(ByVal data As Byte()) As Byte()
        Return UnprotectData(data, CryptProtectDataFlags.CRYPTPROTECT_UI_FORBIDDEN)
    End Function

    Public Shared Function UnprotectData(ByVal data As String) As String
        If Not data Is Nothing Then
            Dim dataIn As Byte() = Convert.FromBase64String(data)
            Dim dataOut As Byte() = UnprotectData(dataIn)

            If Not dataOut Is Nothing Then
                Return Encoding.Unicode.GetString(dataOut)
            Else
                Return Nothing
            End If
        Else
            Return Nothing
        End If
    End Function

    Friend Shared Function UnprotectData(ByVal data As Byte(), ByVal dwFlags As CryptProtectDataFlags) As Byte()
        Dim clearText As Byte() = Nothing

        'copy data into unmanaged memory
        Dim din As New DPAPI.DATA_BLOB
        din.cbData = data.Length
        din.pbData = Marshal.AllocHGlobal(din.cbData)

        If din.pbData.Equals(IntPtr.Zero) Then Throw New OutOfMemoryException("Unable to allocate memory for buffer.")

        Marshal.Copy(data, 0, din.pbData, din.cbData)

        Dim dout As New DPAPI.DATA_BLOB

        Try
            Dim cryptoRetval As Boolean = DPAPI.CryptUnprotectData(din, Nothing, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, dwFlags, dout)

            If cryptoRetval Then 'ERROR_SUCCESS?
                ReDim clearText(dout.cbData)
                Marshal.Copy(dout.pbData, clearText, 0, dout.cbData)
                DPAPI.LocalFree(dout.pbData)
            Else
                Dim errCode As Integer = Marshal.GetLastWin32Error()
                Dim buffer As New StringBuilder(256)
                Win32Error.FormatMessage(Win32Error.FormatMessageFlags.FORMAT_MESSAGE_FROM_SYSTEM, IntPtr.Zero, errCode, 0, buffer, buffer.Capacity, IntPtr.Zero)
            End If
        Finally
            'free the allocated memory in use
            If Not din.pbData.Equals(IntPtr.Zero) Then Marshal.FreeHGlobal(din.pbData)
        End Try

        Return clearText
    End Function

    Friend Shared Sub InitPromptstruct(ByRef ps As DPAPI.CRYPTPROTECT_PROMPTSTRUCT)
        ps.cbSize = Marshal.SizeOf(GetType(DPAPI.CRYPTPROTECT_PROMPTSTRUCT))
        ps.dwPromptFlags = 0
        ps.hwndApp = IntPtr.Zero
        ps.szPrompt = Nothing
    End Sub
End Class

<SuppressUnmanagedCodeSecurityAttribute()> _
Friend Class DPAPI
    <DllImport("crypt32")> _
    Public Shared Function CryptProtectData(ByRef dataIn As DATA_BLOB, ByVal szDataDescr As String, ByVal optionalEntropy As IntPtr, ByVal pvReserved As IntPtr, ByVal pPromptStruct As IntPtr, ByVal dwFlags As DataProtection.CryptProtectDataFlags, ByRef pDataOut As DATA_BLOB) As Boolean
    End Function

    <DllImport("crypt32")> _
    Public Shared Function CryptUnprotectData(ByRef dataIn As DATA_BLOB, ByVal ppszDataDescr As StringBuilder, ByVal optionalEntropy As IntPtr, ByVal pvReserved As IntPtr, ByVal pPromptStruct As IntPtr, ByVal dwFlags As DataProtection.CryptProtectDataFlags, ByRef pDataOut As DATA_BLOB) As Boolean
    End Function

    <DllImport("Kernel32.dll")> _
    Public Shared Function LocalFree(ByVal hMem As IntPtr) As IntPtr
    End Function

    <StructLayout(LayoutKind.Sequential)> _
    Public Structure DATA_BLOB
        Public cbData As Integer
        Public pbData As IntPtr
    End Structure

    <StructLayout(LayoutKind.Sequential)> _
    Public Structure CRYPTPROTECT_PROMPTSTRUCT
        Public cbSize As Integer '= Marshal.SizeOf(typeof(CRYPTPROTECT_PROMPTSTRUCT))
        Public dwPromptFlags As Integer '= 0
        Public hwndApp As IntPtr '= IntPtr.Zero
        Public szPrompt As String ' = nothing
    End Structure
End Class

Friend Class Win32Error
    <Flags()> _
    Public Enum FormatMessageFlags : int
        FORMAT_MESSAGE_ALLOCATE_BUFFER = &H100
        FORMAT_MESSAGE_IGNORE_INSERTS = &H200
        FORMAT_MESSAGE_FROM_STRING = &H400
        FORMAT_MESSAGE_FROM_HMODULE = &H800
        FORMAT_MESSAGE_FROM_SYSTEM = &H1000
        FORMAT_MESSAGE_ARGUMENT_ARRAY = &H2000
        FORMAT_MESSAGE_MAX_WIDTH_MASK = &HFF
    End Enum

    <DllImport("Kernel32.dll")> _
    Public Shared Function FormatMessage(ByVal flags As FormatMessageFlags, ByVal source As IntPtr, ByVal messageId As Integer, ByVal languageId As Integer, ByVal buffer As StringBuilder, ByVal size As Integer, ByVal arguments As IntPtr) As Integer
    End Function
End Class

