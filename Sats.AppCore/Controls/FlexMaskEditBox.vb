'Written by Ted Schopenhouer 
'email ted.schopenhouer@hccnet.nl 
'
'This source code is freeware
'I hope, when you modify this source code in a positive way, you'll
'send me a copy of it per email.
'thanxs in advance.

Option Strict On
Option Explicit On

#Region " Imports    "

Imports System.Windows.Forms
Imports System.ComponentModel
Imports System.Drawing
Imports System.Text
Imports System.Globalization

'Imports System.ComponentModel
Imports System.ComponentModel.Design.Serialization

#End Region

'<ToolboxBitmap("..\FlexMaskEditBox.ico")> _ (picture is only balast!)
Public Class FlexMaskEditBox
    Inherits System.Windows.Forms.TextBox

#Region " Enums    "
    Public Enum _FieldType
        ALFA
        NUMERIC
        DATE_
    End Enum

    Protected Enum Status
        NOVALIDPOS
        UpperCase
        LowerCase
        NoCase
    End Enum

    Public Enum SelectTxt
        Never
        Always
        Once
    End Enum

    Protected Enum flxMask   ' "&AaCc?#9"
        Ampersand
        UpperA
        LowerA
        UpperC
        LowerC
        QuestionMark
        NumberSign
        NineSign
        NoPos
        DecPoint
        GroupSep
        DateSep
    End Enum
#End Region

#Region " Vars, events   "
    Protected Const _CONSTsMaskChars As String = "&AaCc?#9"
    Protected Const _CONSTiKeyDecPoint As Integer = 190
    Protected Const _CONSTiAsci_iDecPoint As Integer = 46
    Protected Const _CONSTsPromptChar As String = " "

    Shadows Event KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
    Shadows Event KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
    Shadows Event Enter(ByVal sender As Object, ByVal e As System.EventArgs)
    Shadows Event Leave(ByVal sender As Object, ByVal e As System.EventArgs)

    Protected _sDecimalSeperator As String = NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator
    Protected _sGroupSeperator As String = NumberFormatInfo.CurrentInfo.CurrencyGroupSeparator
    Protected _sDateSeperator As String = DateTimeFormatInfo.CurrentInfo.DateSeparator
    Protected _iDecimalSeperator As Integer = Asc(_sDecimalSeperator)
    Protected _bNoReturn As Boolean = False
    Protected _bMaskCharInclude As Boolean = True
    Protected _iDecPoint As Integer = 0
    Protected _iMaxLen As Integer = 0
    Protected _sSpecialChars As String = ""
    Protected _bBeepOnError As Boolean = False
    Protected _sMask As String = ""
    Protected _iFieldType As _FieldType = _FieldType.ALFA
    Protected _Text As String
    Protected _textCopy As String = ""
    Protected _bMaskUsed As Boolean = False
    Protected _txtPosStatus() As Status
    Protected _MaskSigns() As flxMask
    Protected _sShadowEmptyText As String = ""
    Protected _NoTab As Boolean = False
    Protected _bModified As Boolean = False
    Protected _sOriginalValue As String = ""
    Protected _SelTxtAllowed As Boolean = False
    Protected _SelectTxt As SelectTxt

    Protected ErrorProvider1 As ErrorProvider = New ErrorProvider()
    Protected ToolTip1 As ToolTip = New ToolTip()

    Protected WithEvents ContextMenu1 As ContextMenu = New ContextMenu()
    Protected WithEvents menuCopy As New MenuItem("Copy")
    Protected WithEvents menuPaste As New MenuItem("Paste")
    Protected WithEvents menuDelete As New MenuItem("Delete")

    Protected _PromptChar As Char = CChar(_CONSTsPromptChar)
    Protected _iPromptChar As Integer = Asc(_PromptChar)

    Protected _bInsertOn As Boolean = False
    Protected _bDelete As Boolean = False
    Protected _sFormatString As String = ""
    Protected _sErrorTxt As String = ""

    Protected _ErrorForeColor As Color = Color.Red

    Protected _FocusBackColor As Color = MyBase.BackColor
    Protected _FocusForeColor As Color = MyBase.ForeColor

    Protected _BackColor As Color = MyBase.BackColor
    Protected _ForeColor As Color = MyBase.ForeColor


#End Region

    Public Sub New()
        MyBase.New()

        MyBase.MaxLength = 50

        'add menu for copy/paste/delete
        Me.ContextMenu = ContextMenu1

        ContextMenu1.MenuItems.Clear()
        ContextMenu1.MenuItems.Add(menuCopy)
        ContextMenu1.MenuItems.Add(menuPaste)
        ContextMenu1.MenuItems.Add(menuDelete)
    End Sub


    Protected Sub BuildMask()
        Dim i, iLenMask As Integer
        Dim bBackslash As Boolean
        Dim tmpChar As Char
        Dim StatusNextChar As Status = Status.NoCase
        Dim curPosStatus As Status = Status.NoCase
        Dim j As Integer = 1

        _Text = Nothing
        _sShadowEmptyText = Nothing
        _iDecPoint = 0
        _bMaskUsed = False

        iLenMask = _sMask.Length
        ReDim _txtPosStatus(iLenMask)
        ReDim _MaskSigns(iLenMask)

        Dim sMaskTxt As StringBuilder = New StringBuilder(iLenMask)

        For i = 1 To iLenMask

            tmpChar = _sMask.Chars(i - 1)

            If tmpChar = "\" AndAlso Not bBackslash Then  'Next Char is NO mask Char
                bBackslash = True

            ElseIf _CONSTsMaskChars.IndexOf(tmpChar) > -1 AndAlso Not bBackslash Then

                _MaskSigns(j) = CType(_CONSTsMaskChars.IndexOf(tmpChar), flxMask)
                sMaskTxt.Append(_PromptChar)

                If StatusNextChar <> Status.NoCase Then

                    _txtPosStatus(j) = StatusNextChar
                    StatusNextChar = Status.NoCase

                Else
                    _txtPosStatus(j) = curPosStatus

                End If

                j += 1
            ElseIf tmpChar = "|" AndAlso Not bBackslash Then
                curPosStatus = Status.NoCase

            ElseIf tmpChar = ">" AndAlso Not bBackslash Then 'UPPERCASE
                curPosStatus = Status.UpperCase

            ElseIf tmpChar = "}" AndAlso Not bBackslash Then
                StatusNextChar = Status.UpperCase

            ElseIf tmpChar = "{" AndAlso Not bBackslash Then
                StatusNextChar = Status.LowerCase

            ElseIf tmpChar = "<" AndAlso Not bBackslash Then  'LOWERCASE
                curPosStatus = Status.LowerCase

            ElseIf Char.ToUpper(tmpChar) = "D" AndAlso Not bBackslash Then

                _MaskSigns(j) = flxMask.DecPoint
                sMaskTxt.Append(_sDecimalSeperator)
                _txtPosStatus(j) = Status.NOVALIDPOS

                If _iFieldType = _FieldType.NUMERIC Then
                    _iDecPoint = j

                End If

                j += 1

            ElseIf Char.ToUpper(tmpChar) = "G" AndAlso Not bBackslash Then

                _MaskSigns(j) = flxMask.GroupSep
                sMaskTxt.Append(_sGroupSeperator)
                _txtPosStatus(j) = Status.NOVALIDPOS
                j += 1

            ElseIf Char.ToUpper(tmpChar) = "S" AndAlso Not bBackslash Then

                _MaskSigns(j) = flxMask.DateSep
                sMaskTxt.Append(_sDateSeperator)
                _txtPosStatus(j) = Status.NOVALIDPOS
                j += 1

            Else

                _MaskSigns(j) = flxMask.NoPos
                sMaskTxt.Append(tmpChar)
                bBackslash = False
                curPosStatus = Status.NoCase
                _txtPosStatus(j) = Status.NOVALIDPOS
                j += 1

            End If

        Next

        _bMaskUsed = True
        _iMaxLen = sMaskTxt.Length
        _Text = sMaskTxt.ToString
        _sShadowEmptyText = _Text

        If Me.DesignMode Then
            Me.Disp(_sMask)
        Else
            Me.Disp(_Text)
        End If

        MyBase.MaxLength = _iMaxLen

    End Sub


    'All the exceptions in a numeric field (that are many more then you can think of)
    Protected Sub DelNul(Optional ByVal HasFocus As Boolean = False, Optional ByVal NewText As String = "")
        Dim tmpText() As Char, tmpChar As Char
        Dim sTmpToken, sOldText As String
        Dim i, iCursorPos, iLen As Integer
        Dim MinusSign, bNumDetect As Boolean

        If _iFieldType = _FieldType.NUMERIC Then

            iCursorPos = MyBase.SelectionStart
            sOldText = _Text
            If Not NewText = Nothing Then

                tmpText = NewText.ToCharArray
            Else

                tmpText = _Text.ToCharArray

                For i = 1 To _iMaxLen

                    If _txtPosStatus(i) = Status.NOVALIDPOS AndAlso i <> _iDecPoint Then

                        tmpText(i - 1) = _PromptChar

                    End If

                Next

            End If

            iLen = tmpText.Length - 1

            For i = 0 To iLen Step 1

                tmpChar = tmpText(i)

                If tmpChar <> _PromptChar Then

                    If tmpChar = "0"c Then

                        'Xử lý nếu chỉ có duy nhất 1 số 0 thì vẫn hiển thị
                        If sOldText.Replace(_PromptChar, "").Replace(",", "").Replace(".", "").Trim.Length = 1 Then
                            MinusSign = True
                        End If

                        If MinusSign Then

                            'If i < iLen Then

                            '    If tmpText(i + 1) <> _sDecimalSeperator OrElse HasFocus Then
                            '        tmpText(i) = _PromptChar

                            '    End If

                            'End If

                            MinusSign = False

                        Else

                            tmpText(i) = _PromptChar

                        End If
                    ElseIf tmpChar = "-"c Then
                        MinusSign = True

                    Else
                        Exit For

                    End If

                End If

            Next

            If _iDecPoint > 0 AndAlso _Text.IndexOf(_sDecimalSeperator) > -1 Then

                For i = iLen To 0 Step -1

                    tmpChar = tmpText(i)

                    If tmpChar <> _PromptChar Then

                        If tmpChar = "0"c Then
                            tmpText(i) = _PromptChar

                        Else
                            Exit For

                        End If

                    End If

                Next

            End If

            tmpText = Replace(tmpText, _PromptChar, "").ToCharArray

            If HasFocus Then

                For i = 0 To tmpText.Length - 1

                    If IsNumeric(tmpText(i)) Then
                        bNumDetect = True

                        Exit For

                    End If
                Next

                If Not bNumDetect Then
                    tmpText = Nothing

                End If

            Else

                If _iDecPoint > 0 Then

                    If tmpText.Length = 0 OrElse tmpText(0) = _sDecimalSeperator Then
                        tmpText = ("0" & tmpText).ToCharArray

                    ElseIf tmpText(0) = "-"c AndAlso tmpText(1) = _sDecimalSeperator Then
                        tmpText = ("-0" & tmpText.ToString.Substring(1)).ToCharArray

                    End If

                    If Array.IndexOf(tmpText, CChar(_sDecimalSeperator)) + 1 = tmpText.Length Then
                        tmpText = (tmpText & "0"c).ToCharArray

                    End If

                End If

            End If

            _Text = _sShadowEmptyText

            sTmpToken = Me.Token(tmpText, 0, _sDecimalSeperator)

            If _iDecPoint > 0 Then

                MyBase.SelectionStart = _iDecPoint - 1
                For i = 0 To sTmpToken.Length - 1
                    Me.InsertCharToLeft(Asc(sTmpToken.Chars(i)))

                Next

                sTmpToken = Me.Token(tmpText, 1, _sDecimalSeperator)

                MyBase.SelectionStart = _iDecPoint
                For i = 0 To sTmpToken.Length - 1
                    Me.InsertChar(Asc(sTmpToken.Chars(i)))

                Next

            Else

                If sTmpToken.Trim <> "0" Then
                    If CStr(Val(sTmpToken)) = "0" Then
                        sTmpToken = ""
                        _Text = _sShadowEmptyText
                        Disp(_Text)
                        Exit Sub
                    Else
                        sTmpToken = CStr(Val(sTmpToken))
                    End If
                End If

                'If MyBase.TextAlign = HorizontalAlignment.Left Then
                '    MyBase.SelectionStart = 0
                'ElseIf MyBase.TextAlign = HorizontalAlignment.Right Then
                Dim intTextLength, intInvalid As Integer
                intTextLength = sTmpToken.Length
                i = _iMaxLen + 1
                intTextLength = 0
                intInvalid = 0
                While intTextLength < sTmpToken.Length
                    i -= 1
                    If _txtPosStatus(i) <> Status.NOVALIDPOS Then
                        intTextLength += 1
                    Else
                        intInvalid += 1
                    End If
                End While
                MyBase.SelectionStart = _iMaxLen - (intTextLength + intInvalid)
                'End If
                For i = 0 To sTmpToken.Length - 1 Step 1
                    Me.InsertChar(Asc(sTmpToken.Chars(i)))

                Next

            End If

        End If

    End Sub


    Protected Sub FitInMask(ByVal sNewText As String)
        Dim i, j As Integer
        Dim TmpChar As Char, validChar As Char

        _sOriginalValue = sNewText
        _bModified = False

        If sNewText = Nothing Then

            _Text = _sShadowEmptyText.ToCharArray
            _textCopy = Nothing
            Exit Sub

        End If

        If _iFieldType = _FieldType.NUMERIC Then

            Me.DelNul(False, sNewText)
            _textCopy = _Text

            Exit Sub

        End If

        For i = 0 To sNewText.Length - 1

            TmpChar = sNewText.Chars(i)

            For j = j + 1 To _iMaxLen

                If _txtPosStatus(j) <> Status.NOVALIDPOS Then

                    validChar = Me.ValidChar(Asc(TmpChar), j)

                    If Not validChar = Nothing Then
                        _Text = _Text.Remove(j - 1, 1).Insert(j - 1, validChar)

                    Else
                        j -= 1

                    End If

                    Exit For

                ElseIf _sShadowEmptyText.Chars(j - 1) = TmpChar Then

                    Exit For

                End If

            Next

        Next

        _textCopy = _Text

    End Sub


    Protected Function ValidChar(ByVal KeyAscii As Integer, ByVal iCursorPos As Integer) As Char
        If KeyAscii = _iPromptChar Then
            If _txtPosStatus(iCursorPos) = Status.NOVALIDPOS Then
                Return Nothing
            End If

            Return _PromptChar

        ElseIf Not _sSpecialChars = Nothing Then

            If _sSpecialChars.IndexOf(Chr(KeyAscii)) > -1 Then
                If _txtPosStatus(iCursorPos) = Status.NOVALIDPOS Then
                    Return Nothing
                End If

                Return Chr(KeyAscii)
            End If

        End If

        Select Case _MaskSigns(iCursorPos)

            Case flxMask.NumberSign
                If KeyAscii > 47 AndAlso KeyAscii < 58 Then
                    Return Chr(KeyAscii)
                End If

            Case flxMask.NineSign
                If KeyAscii > 47 AndAlso KeyAscii < 58 Then
                    Return Chr(KeyAscii)

                ElseIf KeyAscii = 45 Then ' - sign

                    If _iFieldType = _FieldType.NUMERIC Then

                        If Not _bDelete Then

                            If _iDecPoint > 0 Then

                                If _iDecPoint < iCursorPos Then Return Nothing

                            End If

                            Dim i As Integer
                            For i = 1 To iCursorPos
                                If _Text.Chars(i - 1) <> _PromptChar AndAlso _
                                   _txtPosStatus(i) <> Status.NOVALIDPOS Then Return Nothing

                            Next

                        End If

                    End If

                    Return Chr(KeyAscii)

                End If

            Case flxMask.QuestionMark

                Select Case KeyAscii

                    Case 65 To 90, 97 To 122
                        Return Me.xCase(Chr(KeyAscii), _txtPosStatus(iCursorPos))

                End Select

            Case flxMask.UpperA, flxMask.UpperC

                Select Case KeyAscii

                    Case 65 To 90, 97 To 122, 48 To 57
                        Return Me.xCase(Chr(KeyAscii), _txtPosStatus(iCursorPos))

                End Select

            Case flxMask.Ampersand

                Select Case KeyAscii

                    Case 32 To 126, 128 To 255
                        Return Me.xCase(Chr(KeyAscii), _txtPosStatus(iCursorPos))

                End Select

            Case flxMask.LowerA, flxMask.LowerC

                Select Case KeyAscii

                    Case 65 To 90, 32, 97 To 122, 48 To 57
                        Return Me.xCase(Chr(KeyAscii), _txtPosStatus(iCursorPos))

                End Select

        End Select

    End Function


    Private Function xCase(ByVal tmpChar As Char, ByVal xAction As Status) As Char

        Select Case xAction

            Case Status.NoCase
                Return tmpChar

            Case Status.UpperCase
                Return Char.ToUpper(tmpChar)

            Case Status.LowerCase
                Return Char.ToLower(tmpChar)

        End Select

    End Function


    Protected Function ShiftBackSpace() As Boolean
        Dim i As Integer, iCursorPos As Integer = MyBase.SelectionStart

        For i = iCursorPos To 1 Step -1

            If _txtPosStatus(i) <> Status.NOVALIDPOS Then

                _Text = _Text.Remove(i - 1, 1).Insert(i - 1, _PromptChar)
                Me.Disp(_Text)
                MyBase.SelectionStart = i - 1

                Return True

            End If

        Next

    End Function

    Protected Function OverWriteChar(ByVal KeyAscii As Integer) As Boolean
        Dim validChar As Char, i As Integer
        Dim iCursorPos As Integer = Me.GetValidPosRight(MyBase.SelectionStart) ' + 1

        For i = iCursorPos To _iMaxLen

            validChar = Me.ValidChar(KeyAscii, i)

            If Not validChar = Nothing Then

                _Text = _Text.Remove(i - 1, 1).Insert(i - 1, validChar)

                Me.Disp(_Text)
                MyBase.SelectionStart = i

                Return True

            End If

        Next

    End Function


    Protected Function InsertCharToLeft(ByVal KeyAscii As Integer) As Boolean
        Dim tmpText() As Char, TmpChar As Char, validChar As Char, validChar2 As Char
        Dim i As Integer
        Dim iCursorPos As Integer = MyBase.SelectionStart
        Dim iPosOld As Integer = iCursorPos

        For i = iCursorPos + 1 To _iMaxLen Step 1

            If i = _iDecPoint OrElse (_txtPosStatus(i) <> Status.NOVALIDPOS AndAlso _Text.Chars(i - 1) <> _PromptChar) Then
                Exit For

            Else
                iCursorPos += 1

            End If

        Next

        For i = iCursorPos To 1 Step -1

            If _txtPosStatus(i) = Status.NOVALIDPOS Then
                iCursorPos -= 1

            Else
                Exit For

            End If

        Next

        If iCursorPos < 2 Then

            If _iFieldType = _FieldType.NUMERIC Then

                If _iDecPoint < 3 Then

                    If _Text.Chars(0) = _PromptChar Then
                        MyBase.SelectionStart = 0

                    End If

                End If

            End If

            Return Me.InsertChar(KeyAscii)

        End If

        validChar = Me.ValidChar(KeyAscii, iCursorPos)

        If Not validChar = Nothing Then

            tmpText = _Text.ToCharArray

            For i = iCursorPos To 2 Step -1

                If _txtPosStatus(i) <> Status.NOVALIDPOS AndAlso TmpChar = Nothing Then

                    TmpChar = _Text.Chars(i - 1)

                End If

                If _txtPosStatus(i - 1) <> Status.NOVALIDPOS AndAlso (Not TmpChar = Nothing) Then

                    validChar2 = Me.ValidChar(Asc(TmpChar), i - 1)

                    If Not validChar2 = Nothing Then

                        If TmpChar = _PromptChar Then
                            Exit For

                        ElseIf i = 2 AndAlso _Text.Chars(0) <> _PromptChar Then
                            If (iCursorPos + 1 <> _iDecPoint) Then
                                Return Me.InsertChar(KeyAscii)
                            Else
                                Return False
                            End If

                        Else
                            tmpText(i - 2) = TmpChar
                            TmpChar = Nothing

                        End If

                    Else
                        Exit For

                    End If
                ElseIf TmpChar = _PromptChar AndAlso (Not validChar2 = Nothing) Then
                    tmpText(i - 1) = validChar2

                    Exit For

                ElseIf i = 2 Then
                    Return False

                End If

            Next

            tmpText(iCursorPos - 1) = validChar
            _Text = tmpText

            If FieldType = _FieldType.NUMERIC AndAlso iPosOld < iCursorPos AndAlso _Text.IndexOf(_PromptChar, iPosOld, iCursorPos - iPosOld) > -1 Then

                For i = 1 To iPosOld

                    If _txtPosStatus(i) <> Status.NOVALIDPOS Then

                        If _Text.Chars(i - 1) <> _PromptChar Then
                            Me.DelNul(True)
                        End If

                    End If

                Next

            End If

            Me.Disp(_Text)
            MyBase.SelectionStart = iCursorPos

        Else
            Return Me.InsertChar(KeyAscii)

        End If

        Return True
    End Function


    Protected Sub Disp(ByVal s As String)
        _bNoReturn = True : MyBase.Text = s : _bNoReturn = False
    End Sub


    Protected Function InsertChar(ByVal KeyAscii As Integer) As Boolean
        Dim validChar, tmpChar As Char
        Dim i, j As Integer
        Dim iCursorPos As Integer = Me.GetValidPosRight(MyBase.SelectionStart)

        If iCursorPos = MyBase.SelectionStart Then Exit Function
        If iCursorPos = _iMaxLen AndAlso _Text.Chars(_iMaxLen - 1) <> _PromptChar Then Return False

        validChar = Me.ValidChar(KeyAscii, iCursorPos)

        If validChar = Nothing Then Exit Function

        Dim tmpText() As Char = _Text.ToCharArray

        tmpText(iCursorPos - 1) = validChar

        For i = iCursorPos To _iMaxLen Step 1

            validChar = _Text.Chars(i - 1)

            If validChar = _PromptChar Then
                Exit For
            End If

            For j = i + 1 To _iMaxLen

                tmpChar = _Text.Chars(j - 1)
                If _iFieldType = _FieldType.NUMERIC AndAlso _iDecPoint > 0 AndAlso j = _iDecPoint Then
                    Return False
                End If

                If j = _iMaxLen AndAlso ((_iFieldType = _FieldType.NUMERIC AndAlso ("0" & _PromptChar).IndexOf(tmpChar) = -1) OrElse (_iFieldType <> _FieldType.NUMERIC AndAlso tmpChar <> _PromptChar)) Then
                    Return False

                ElseIf _txtPosStatus(j) = Status.NOVALIDPOS Then
                    i += 1

                Else
                    validChar = Me.ValidChar(Asc(validChar), j)

                    If Not validChar = Nothing Then
                        tmpText(j - 1) = validChar
                    Else
                        Return False
                    End If

                    Exit For

                End If

            Next

        Next

        _Text = tmpText

        If _iFieldType = _FieldType.NUMERIC Then
            If _iDecPoint > 0 AndAlso iCursorPos > _iDecPoint Then
                For i = _iDecPoint + 1 To iCursorPos
                    If _txtPosStatus(i) <> Status.NOVALIDPOS AndAlso _Text.Chars(i - 1) = _PromptChar Then
                        Me.DelNul(True)
                        For j = _iDecPoint + 1 To _iMaxLen
                            If _txtPosStatus(j) <> Status.NOVALIDPOS AndAlso _Text.Chars(j - 1) = _PromptChar Then
                                iCursorPos = j - 1
                                Exit For
                            End If
                        Next
                        Exit For
                    End If
                Next
            End If
        End If

        Me.Disp(_Text)
        j = GetValidPosRight(iCursorPos)

        If iCursorPos = j Then
            MyBase.SelectionStart = iCursorPos
        Else
            MyBase.SelectionStart = j - 1
        End If

        Return True
    End Function


    Protected Sub ToDecPoint()
        Dim iCursorPos, i, j As Integer
        iCursorPos = MyBase.SelectionStart
        If iCursorPos < _iDecPoint Then
            For i = iCursorPos To _iDecPoint
                If _txtPosStatus(i) <> Status.NOVALIDPOS AndAlso _Text.Chars(i - 1) = _PromptChar Then
                    Me.DelNul(True)
                    For j = iCursorPos + 1 To _iDecPoint
                        If _txtPosStatus(j) <> Status.NOVALIDPOS AndAlso _Text.Chars(j - 1) = _PromptChar Then
                            iCursorPos = j - 1
                            Exit For
                        End If
                    Next
                    Exit For
                End If
            Next
        End If

        If iCursorPos < _iDecPoint - 1 Then
            MyBase.SelectionStart = _iDecPoint - 1
        ElseIf iCursorPos > _iDecPoint Then
            MyBase.SelectionStart = _iDecPoint
        ElseIf iCursorPos = _iDecPoint Then
            MyBase.SelectionStart = _iDecPoint - 1
        Else
            MyBase.SelectionStart = _iDecPoint
        End If
    End Sub


    Protected Function DelLeftFromCursor() As Boolean
        Dim i, j, k, iCursorPos As Integer
        Dim validChar As Char

        For k = 1 To _iMaxLen
            If _txtPosStatus(k) <> Status.NOVALIDPOS Then Exit For

        Next

        iCursorPos = MyBase.SelectionStart

        If iCursorPos < k Then Return False

        Dim tmpText() As Char = _Text.ToCharArray

        For i = iCursorPos To k Step -1

            If _txtPosStatus(i) <> Status.NOVALIDPOS Then

                For j = i To 2 Step -1

                    If _txtPosStatus(j - 1) <> Status.NOVALIDPOS Then

                        validChar = Me.ValidChar(Asc(tmpText(j - 2)), i)

                        If Not validChar = Nothing Then
                            tmpText(i - 1) = validChar

                        Else
                            Return False

                        End If

                        Exit For

                    End If

                Next

            End If

        Next

        tmpText(k - 1) = _PromptChar
        _Text = tmpText
        Me.Disp(_Text)
        MyBase.SelectionStart = iCursorPos

        Return True
    End Function


    Protected Function Delete() As Boolean
        Dim i, j, k, iCursorPos As Integer
        Dim validChar As Char

        For k = _iMaxLen To 1 Step -1
            If _txtPosStatus(k) <> Status.NOVALIDPOS Then Exit For
        Next

        iCursorPos = MyBase.SelectionStart + 1

        If iCursorPos > k Then Exit Function

        Dim tmpText() As Char = _Text.ToCharArray

        For i = iCursorPos To k - 1 Step 1

            If _txtPosStatus(i) <> Status.NOVALIDPOS Then

                For j = i To k - 1 Step 1

                    If _txtPosStatus(j + 1) <> Status.NOVALIDPOS Then

                        validChar = Me.ValidChar(Asc(tmpText(j)), i)

                        If Not validChar = Nothing Then
                            tmpText(i - 1) = validChar
                        Else
                            Return False
                        End If

                        Exit For

                    End If

                Next

            End If
        Next

        tmpText(k - 1) = _PromptChar
        _Text = tmpText

        Me.Disp(_Text)
        MyBase.SelectionStart = iCursorPos - 1

        Return True
    End Function


    Protected Function BackSpace() As Boolean
        Dim i, j, k, iCursorPos As Integer
        Dim validChar As Char

        iCursorPos = MyBase.SelectionStart

        For i = iCursorPos To 1 Step -1

            If _txtPosStatus(i) <> Status.NOVALIDPOS Then

                iCursorPos = i
                Exit For

            End If
        Next

        If i <> iCursorPos Then Return False

        If FieldType = _FieldType.NUMERIC AndAlso iCursorPos < _iDecPoint Then

            For k = _iDecPoint To 1 Step -1

                If _txtPosStatus(k) <> Status.NOVALIDPOS Then Exit For

            Next

        Else

            For k = _iMaxLen To 1 Step -1

                If _txtPosStatus(k) <> Status.NOVALIDPOS Then Exit For

            Next

        End If

        If iCursorPos = 0 OrElse k = 0 Then Return False

        Dim TmpText() As Char = _Text.ToCharArray

        For i = iCursorPos To k - 1 Step 1

            If _txtPosStatus(i) <> Status.NOVALIDPOS Then

                For j = i To k - 1 Step 1

                    If _txtPosStatus(j + 1) <> Status.NOVALIDPOS Then

                        validChar = Me.ValidChar(Asc(TmpText(j)), i)

                        If Not validChar = Nothing Then
                            TmpText(i - 1) = validChar

                        Else

                            Return False
                        End If

                        Exit For

                    End If

                Next

            End If

        Next

        TmpText(k - 1) = _PromptChar

        _Text = TmpText

        Me.Disp(_Text)
        MyBase.SelectionStart = iCursorPos - 1

        Return True

    End Function


    Protected Sub TextBox_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        RaiseEvent KeyDown(sender, e)
        Dim iSelStart As Integer

        If e.Shift Then

            If e.KeyCode = Keys.Left Or e.KeyCode = Keys.Right Then
                Exit Sub
            End If

        ElseIf e.Control Then

            If e.KeyCode = Keys.C Then

                Me.CopyClipBoard()
                e.Handled = True
                Exit Sub

            ElseIf e.KeyCode = Keys.V Then

                Me.PasteClipBoard()
                e.Handled = True
                Exit Sub

            End If

        End If

        Select Case e.KeyCode

            Case Keys.Insert
                _bInsertOn = Not _bInsertOn

            Case Keys.Back

                If MyBase.ReadOnly Then
                    If _bBeepOnError Then Beep()

                ElseIf e.Shift Then
                    If (Not Me.ShiftBackSpace()) AndAlso _bBeepOnError Then Beep()

                Else
                    If (Not Me.BackSpace()) AndAlso _bBeepOnError Then Beep()

                End If

            Case Keys.Delete

                If MyBase.ReadOnly Then
                    If _bBeepOnError Then Beep()

                ElseIf MyBase.SelectionLength > 0 Then
                    Me.DeleteSelTxt()

                ElseIf _iFieldType = _FieldType.NUMERIC AndAlso (Not e.Control) Then

                    If _iDecPoint > 0 AndAlso MyBase.SelectionStart < _iDecPoint Then

                        _bDelete = True
                        If Not e.Shift Then
                            MyBase.SelectionStart = Math.Min(MyBase.SelectionStart, _iDecPoint - 1)

                        End If

                        If (Not Me.DelLeftFromCursor()) AndAlso _bBeepOnError Then Beep()
                        _bDelete = False

                    Else

                        If (Not Me.Delete()) AndAlso _bBeepOnError Then Beep()

                    End If

                ElseIf e.Shift Then

                    If (Not Me.DelLeftFromCursor()) AndAlso _bBeepOnError Then Beep()

                ElseIf e.Control Then


                    iSelStart = MyBase.SelectionStart

                    If iSelStart < _iMaxLen Then

                        _Text = _Text.Remove(iSelStart, 1).Insert(iSelStart, _sShadowEmptyText.Chars(iSelStart))
                        Me.Disp(_Text)
                        MyBase.SelectionStart = iSelStart

                    ElseIf _bBeepOnError Then
                        Beep()

                    End If

                Else

                    If (Not Me.Delete()) AndAlso _bBeepOnError Then Beep()

                End If

            Case Keys.Left
                MyBase.SelectionStart = Me.GetValidPosLeft(MyBase.SelectionStart + 1) - 1

            Case Keys.Right
                MyBase.SelectionStart = Me.GetValidPosRight(MyBase.SelectionStart)

            Case CType(_CONSTiKeyDecPoint, Keys), Keys.Decimal

                If (_iFieldType = _FieldType.NUMERIC AndAlso _iDecPoint > 0) Then   ' . pressed

                    Me.ToDecPoint()

                End If

            Case Keys.Home
                iSelStart = MyBase.SelectionStart
                MyBase.SelectionStart = 0
                If e.Shift Then
                    MyBase.SelectionLength = iSelStart
                Else
                    MyBase.SelectionLength = 0
                End If

            Case Keys.End
                'MyBase.SelectionStart = _iMaxLen
                If e.Shift Then
                    MyBase.SelectionLength = Len(Trim(MyBase.Text))
                Else
                    MyBase.SelectionStart = _iMaxLen
                End If

        End Select

        e.Handled = True

    End Sub


    Protected Function GetValidPosRight(ByVal iCursorPos As Integer) As Integer
        Dim i As Integer

        For i = iCursorPos + 1 To _iMaxLen

            If _txtPosStatus(i) <> Status.NOVALIDPOS Then
                Return i
            End If

        Next

        Return iCursorPos

    End Function


    Protected Function GetValidPosLeft(ByVal iOldPos As Integer) As Integer
        Dim i As Integer

        For i = iOldPos - 1 To 1 Step -1

            If _txtPosStatus(i) <> Status.NOVALIDPOS Then
                Return i
            End If

        Next

        Return iOldPos

    End Function


    Private Sub TextBox_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        RaiseEvent KeyPress(sender, e)

        If e.KeyChar = vbCr Then
            '   SendKeys.Send("{Tab}")
            'e.Handled = True

        ElseIf _bMaskUsed Then

            Dim KeyInt As Integer = Asc(e.KeyChar)

            If KeyInt >= 32 Then

                If Me.ReadOnly Then

                    If _bBeepOnError Then Beep()
                    e.Handled = True
                    Exit Sub

                ElseIf Me.SelectionLength > 0 Then
                    Me.DeleteSelTxt()

                End If
                If KeyInt = 45 AndAlso _iFieldType = _FieldType.NUMERIC AndAlso NegValue() > -1 Then
                    If _bBeepOnError Then Beep()

                ElseIf KeyInt > 47 AndAlso KeyInt < 58 AndAlso _iFieldType = _FieldType.NUMERIC AndAlso Me.SelectionStart <= NegValue() Then
                    If _bBeepOnError Then Beep()

                ElseIf (KeyInt = _iDecimalSeperator) AndAlso (_iFieldType = _FieldType.NUMERIC AndAlso _iDecPoint > 0) Then     ' . pressed
                    Me.ToDecPoint()

                ElseIf Not (KeyInt = _CONSTiAsci_iDecPoint AndAlso (_iFieldType = _FieldType.NUMERIC AndAlso _iDecPoint > 0)) Then

                    If _bInsertOn Then

                        If Not Me.OverWriteChar(KeyInt) AndAlso _bBeepOnError Then Beep()

                    Else

                        If _iFieldType = _FieldType.NUMERIC AndAlso Me.SelectionStart < _iDecPoint Then

                            If Not Me.InsertCharToLeft(KeyInt) AndAlso _bBeepOnError Then Beep()

                        Else

                            If Me.InsertChar(KeyInt) Then

                            ElseIf _bBeepOnError Then
                                Beep()

                            End If

                        End If

                    End If

                End If

            End If

            e.Handled = True

        End If

    End Sub

    'there may be "-" in then Maskchars  ->  "(???) - (9999d##)"
    Protected Function NegValue() As Integer
        Dim i As Integer
        If _Text.IndexOf("-") > -1 Then
            For i = 0 To _Text.Length - 1
                If _Text.Chars(i) = "-"c Then
                    If _txtPosStatus(i + 1) <> Status.NOVALIDPOS Then
                        Return i
                    End If
                End If
            Next
        End If
        Return -1
    End Function

    Protected Sub GotoFirstPrompChar()
        Dim i As Integer

        If _iFieldType = _FieldType.NUMERIC AndAlso _iDecPoint > 0 Then

            MyBase.SelectionStart = _iDecPoint - 1

        Else
            i = _Text.IndexOf(_PromptChar)

            If i = -1 Then
                i = _sShadowEmptyText.IndexOf(_PromptChar)

            End If
            MyBase.SelectionStart = Math.Max(i, 0)

        End If

    End Sub


    Protected Sub DeleteSelTxt()
        Dim i, CursorPos As Integer

        If _Text = Nothing OrElse MyBase.ReadOnly Then
            Exit Sub
        End If

        CursorPos = MyBase.SelectionStart
        For i = CursorPos + 1 To CursorPos + MyBase.SelectionLength

            If _txtPosStatus(i) <> Status.NOVALIDPOS Then
                _Text = _Text.Remove(i - 1, 1).Insert(i - 1, PromptChar)

            End If

        Next

        Me.Disp(_Text)
        MyBase.SelectionStart = CursorPos

    End Sub


    Protected Function Token(ByVal SeekStr As String, Optional ByVal Index As Integer = 0, Optional ByVal Delimiter As String = "^", Optional ByRef StartPos As Integer = 0) As String
        Dim iCursorPos, iIndexLoop As Integer
        If SeekStr = Nothing OrElse StartPos < 0 OrElse StartPos >= SeekStr.Length Then
            Return ""

        End If

        Do While iIndexLoop <> Index

            StartPos = SeekStr.IndexOf(Delimiter, StartPos)

            If StartPos = -1 Then
                Return ""

            End If

            iIndexLoop += 1
            StartPos += Delimiter.Length

        Loop

        iCursorPos = SeekStr.IndexOf(Delimiter, StartPos)

        If iCursorPos = -1 Then

            If StartPos = 0 Then
                Return SeekStr ' "" 

            End If

            Token = SeekStr.Substring(StartPos)
            StartPos = SeekStr.Length

        Else
            Token = SeekStr.Substring(StartPos, iCursorPos - StartPos)
            StartPos = iCursorPos + 1

        End If

    End Function


    Protected Function FormatData(ByVal sStr2Format As String, Optional ByVal sFormatStr As String = Nothing) As String

        If sFormatStr = Nothing Then
            sFormatStr = _sFormatString

        End If

        If Not sFormatStr = Nothing Then

            If _iFieldType = _FieldType.NUMERIC Then

                Try
                    Return Decimal.Parse(sStr2Format).ToString(sFormatStr)

                Catch
                    Return sStr2Format

                End Try

            ElseIf _iFieldType = _FieldType.DATE_ Then

                Try
                    Return DateTime.Parse(sStr2Format).ToString(sFormatStr)

                Catch
                    Return sStr2Format

                End Try

            Else

                Try
                    Return String.Format(_sFormatString, sFormatStr)

                Catch
                    Return sStr2Format

                End Try

            End If

        Else
            Return sStr2Format

        End If

    End Function


    Protected Function GetText() As String

        If _bMaskUsed Then

            Dim Value As StringBuilder = New StringBuilder(_iMaxLen)
            Dim i As Integer
            Dim s As Char

            For i = 1 To _iMaxLen

                s = _Text.Chars(i - 1)

                If _txtPosStatus(i) <> Status.NOVALIDPOS Then

                    If s = _PromptChar Then
                        Value.Append(" ")

                    Else
                        Value.Append(s)

                    End If

                ElseIf _bMaskCharInclude OrElse _
                                  i = _iDecPoint OrElse _
                                  _MaskSigns(i) = flxMask.DateSep Then

                    Value.Append(s)

                End If

            Next

            If _iFieldType = _FieldType.NUMERIC Then

                Try
                    Return Decimal.Parse(Value.ToString.Replace(" ", "")).ToString

                Catch

                    Try
                        Return Decimal.Parse(("0" & Value.ToString.Replace(" ", ""))).ToString

                    Catch
                    End Try

                End Try

            End If

            Return Value.ToString.TrimEnd

        Else
            Return _Text & ""

        End If

    End Function

    'Create by ChienTD: Tạo hàm GetText riêng 
    Protected Function GetTextPrivate() As String

        If _bMaskUsed Then

            Dim Value As StringBuilder = New StringBuilder(_iMaxLen)
            Dim i As Integer
            Dim s As Char

            For i = 1 To _iMaxLen

                s = _Text.Chars(i - 1)

                If _txtPosStatus(i) <> Status.NOVALIDPOS Then

                    If s = _PromptChar Then
                        Value.Append(" ")

                    Else
                        Value.Append(s)

                    End If

                ElseIf _bMaskCharInclude OrElse _
                                  i = _iDecPoint OrElse _
                                  _MaskSigns(i) = flxMask.DateSep Then

                    Value.Append(s)

                End If

            Next

            Return Value.ToString.TrimEnd

        Else
            Return _Text.TrimEnd

        End If

    End Function

    Protected Function SetText(ByVal Value As String) As String
        Dim sNewValue As String = ""

        _sOriginalValue = Value

        If (Not _Text = Nothing) AndAlso _bMaskUsed Then
            _Text = _sShadowEmptyText

        Else
            _bModified = False
            Return ""

        End If

        If _iFieldType = _FieldType.NUMERIC AndAlso (Not Value = Nothing) Then

            sNewValue = Value.Replace(_PromptChar, "")

            If _iDecPoint = 0 AndAlso sNewValue.IndexOf(_sDecimalSeperator) > -1 Then

                Try
                    sNewValue = CLng(sNewValue).ToString

                Catch
                    sNewValue = sNewValue.Substring(0, sNewValue.IndexOf(_sDecimalSeperator)).Trim

                End Try

            Else

                Try
                    sNewValue = Decimal.Parse(sNewValue).ToString

                Catch
                    sNewValue = Value

                End Try

            End If

        Else
            sNewValue = Value

        End If

        Me.FitInMask(sNewValue)

        Return _Text

    End Function


    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean

        If keyData = Keys.Tab AndAlso _NoTab Then
            'tab is not allowed in datagrid, it will done double
            Return True

        ElseIf keyData = Keys.End Then
            Dim i As Integer
            For i = _iMaxLen To 1 Step -1
                If _txtPosStatus(i) <> Status.NOVALIDPOS Then
                    MyBase.SelectionStart = i
                    Exit For
                End If
            Next

            Return True

        ElseIf keyData = Keys.Home Then
            Dim i As Integer
            For i = 1 To _iMaxLen Step 1
                If _txtPosStatus(i) <> Status.NOVALIDPOS Then
                    MyBase.SelectionStart = i - 1
                    Exit For
                End If
            Next
            Return True

        ElseIf keyData = Keys.Left Or keyData = Keys.Right Then
            TextBox_KeyDown(Me, New System.Windows.Forms.KeyEventArgs(keyData))
            Return True

            'ElseIf keyData = Keys.Enter Then
            '   SendKeys.Send("{Tab}")
            '   Return True

        End If
        Return MyBase.ProcessCmdKey(msg, keyData)

    End Function

    Public Sub RollBack()
        Me.SetText(_textCopy)
    End Sub

    Protected Sub FlexMaskEditBox_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Enter
        RaiseEvent Enter(sender, e)

        'no mask is given, so we make one by our own
        If (Not _bMaskUsed) Then Me.Mask = _sMask

        'do not change !
        Me.Disp(_Text)

        If _iFieldType = _FieldType.NUMERIC Then
            Me.DelNul(True)
            Me.Disp(_Text)

        End If

        Me.GotoFirstPrompChar()

        MyBase.ForeColor = _FocusForeColor
        MyBase.BackColor = _FocusBackColor

        If _SelectTxt <> SelectTxt.Never OrElse _SelTxtAllowed Then
            If _SelTxtAllowed Then MyBase.SelectAll()
            _SelTxtAllowed = (_SelectTxt = SelectTxt.Always)
        End If
    End Sub


    Protected Sub FlexMaskEditBox_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Leave
        RaiseEvent Leave(sender, e)

        If _iFieldType = _FieldType.NUMERIC Then
            Me.DelNul(False)

        End If

        If Not _sFormatString = Nothing Then
            _bNoReturn = True : MyBase.Text = Me.FormatData(Me.GetText()) : _bNoReturn = False

        End If

    End Sub


    Protected Overrides Sub OnLostFocus(ByVal e As System.EventArgs)
        If _sErrorTxt = Nothing Then
            MyBase.ForeColor = _ForeColor

        Else
            MyBase.ForeColor = _ErrorForeColor

        End If

        MyBase.BackColor = _BackColor

    End Sub


    Protected Sub CopyClipBoard()
        Try
            Clipboard.SetDataObject(_Text.Substring(MyBase.SelectionStart, MyBase.SelectionLength).Replace(_PromptChar, " "))
        Catch
        End Try

    End Sub


    Protected Sub PasteClipBoard()
        Dim iData As IDataObject = Clipboard.GetDataObject()
        Dim s As String = CType(iData.GetData(DataFormats.Text), String)
        Dim i As Integer

        If Not s = Nothing Then
            Me.DeleteSelTxt()

            For i = 0 To Math.Min(_iMaxLen, Math.Max(s.Length - 1, 0))
                Me.InsertChar(Asc(s.Chars(i)))

            Next

        End If
    End Sub

    Private Sub MenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
                               Handles menuCopy.Click, menuDelete.Click, menuPaste.Click

        If sender Is menuDelete Then
            Me.DeleteSelTxt()

        ElseIf sender Is menuCopy Then
            Me.CopyClipBoard()

        ElseIf sender Is menuPaste Then
            Me.PasteClipBoard()

        End If

    End Sub


    Private Sub ContextMenu1_Popup(ByVal sender As Object, ByVal e As System.EventArgs) Handles ContextMenu1.Popup
        Dim iData As IDataObject = Clipboard.GetDataObject()

        menuCopy.Enabled = (MyBase.SelectionLength > 0) AndAlso (Not MyBase.ReadOnly)

        menuPaste.Enabled = (Not iData.GetDataPresent(DataFormats.Text) = Nothing) AndAlso (Not MyBase.ReadOnly)

        menuDelete.Enabled = menuCopy.Enabled

    End Sub

    Private Sub FlexMaskEditBox_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseUp
        If Me.SelectionStart = _iMaxLen Then Me.GotoFirstPrompChar()

    End Sub

    Public Overloads Sub SelectAll()
        MyBase.SelectAll()
        _SelTxtAllowed = True
    End Sub
    Public Overloads Sub Clear()
        _Text = _sShadowEmptyText
        Me.Disp(_Text)
    End Sub
    Public Overloads Sub Cut()
        Me.CopyClipBoard()
        Me.DeleteSelTxt()
    End Sub
    Public Overloads Sub ResetText()
        Me.SetText(_sOriginalValue)
        Me.Disp(_Text)
    End Sub
    Public Overloads Sub Paste()
        Me.PasteClipBoard()
    End Sub
    Public Overloads Sub AppendText(ByVal s As String)
        Me.Text = _sOriginalValue & s
    End Sub

    Public Overloads Sub SelectText()
        Dim sText As String
        Dim intTextLength, intInvalid, i As Integer
        If _iFieldType = _FieldType.NUMERIC Then
            sText = GetTextPrivate().Trim
        Else
            sText = GetTextPrivate()
        End If

        intTextLength = sText.Length
        If sText.Length = 0 Then
            MyBase.SelectionStart = 0
            Exit Sub
        End If
        If _iFieldType = _FieldType.NUMERIC Then
            i = _iMaxLen + 1
            intTextLength = 0
            intInvalid = 0
            While intTextLength < sText.Length
                i -= 1
                If _txtPosStatus(i) <> Status.NOVALIDPOS Then
                    intTextLength += 1
                Else
                    intInvalid += 1
                End If
            End While
            MyBase.SelectionStart = _iMaxLen - (intTextLength + intInvalid)
            MyBase.SelectionLength = intTextLength + intInvalid
        Else
            i = 0
            intTextLength = 0
            intInvalid = 0
            While intTextLength < sText.Length
                i += 1
                If _txtPosStatus(i) <> Status.NOVALIDPOS Then
                    intTextLength += 1
                Else
                    intInvalid += 1
                End If
            End While
            MyBase.SelectionStart = 0
            MyBase.SelectionLength = intTextLength + intInvalid
        End If
        _SelTxtAllowed = True
    End Sub
#Region " Public Property's "
    Public ReadOnly Property Version() As String
        Get
            With System.Reflection.Assembly.GetAssembly(Me.GetType()).GetName().Version
                Return .Major.ToString & "." & .Minor.ToString & "." _
                     & .Build.ToString & "." & .Revision().ToString()
            End With
        End Get
    End Property

    Public ReadOnly Property GetFormatedText(Optional ByVal sFormatStr As String = Nothing) As String
        Get
            If (Not _sFormatString = Nothing) OrElse (Not sFormatStr = Nothing) Then
                Return Me.FormatData(Me.GetText(), sFormatStr)
            End If
            Return String.Empty
        End Get
    End Property

    <CategoryAttribute("FlexMaskedit Property"), _
       Browsable(False), _
       [ReadOnly](False), _
       BindableAttribute(False), _
       DefaultValueAttribute(False), _
       DesignOnly(True), _
       DescriptionAttribute("Returned The State Of Text (Modified Or Not) ")> _
    Public Shadows Property Modified() As Boolean
        Get
            Return _sOriginalValue.Trim <> Me.GetText().Trim OrElse _bModified
        End Get
        Set(ByVal Value As Boolean)
            _bModified = Value
        End Set
    End Property

    <CategoryAttribute("FlexMaskedit Property"), _
    DefaultValueAttribute(""), _
    DescriptionAttribute("Tooltip Displayed When MousePointer Enter Text")> _
   Public Property ToolTip() As String
        Get
            Return CType(Me.ToolTip1.GetToolTip(Me), String)
        End Get
        Set(ByVal Value As String)
            Me.ToolTip1.SetToolTip(Me, Value)
        End Set
    End Property

    <CategoryAttribute("FlexMaskedit Property"), _
    DefaultValueAttribute(""), _
    DescriptionAttribute("Mask For Filtering Keyboard Input (See ReadMe.Txt)")> _
    Public Property Mask() As String
        Get
            Return _sMask
        End Get
        Set(ByVal Value As String)
            _bMaskUsed = False
            _sMask = Value
            If (Not Me.DesignMode) Then
                If _sMask = Nothing Then _sMask = Strings.StrDup(50, "&")
                Me.BuildMask()
                If Not _sOriginalValue = Nothing Then
                    Me.Text = _sOriginalValue
                End If
            End If
        End Set
    End Property

    <CategoryAttribute("FlexMaskedit Property"), _
    DefaultValueAttribute(True), _
    DescriptionAttribute("Includes Mask Chars In Returned Text")> _
      Public Property MaskCharInclude() As Boolean
        Get
            Return _bMaskCharInclude
        End Get
        Set(ByVal Value As Boolean)
            _bMaskCharInclude = Value
        End Set
    End Property

    <CategoryAttribute("FlexMaskedit Property"), _
    DefaultValueAttribute(GetType(_FieldType), "0"), _
    DescriptionAttribute("FieldType Numeric/AlfaNumeric (Change Input Behavior)")> _
      Public Property FieldType() As _FieldType
        Get
            Return _iFieldType
        End Get
        Set(ByVal Value As _FieldType)
            _iFieldType = Value
            If _bMaskUsed AndAlso (Not Me.DesignMode) Then Me.Mask = _sMask
        End Set
    End Property


    <CategoryAttribute("FlexMaskedit Property"), _
       DefaultValueAttribute("  (Space)"), _
       DescriptionAttribute("Display Prompt Chars On Valid Input Places")> _
   Public Property PromptChar() As String
        Get
            If Me.DesignMode Then
                If _PromptChar = " "c Then
                    Return CStr(_PromptChar) & " (Space)"
                End If
            End If
            Return CStr(_PromptChar)
        End Get
        Set(ByVal Value As String)
            If Value = Nothing Then Exit Property
            _PromptChar = CType(Value, Char)
            _iPromptChar = Asc(_PromptChar)
            If _bMaskUsed AndAlso (Not Me.DesignMode) Then Me.Mask = _sMask
        End Set
    End Property
    <CategoryAttribute("FlexMaskedit Property"), _
            Browsable(False), _
            [ReadOnly](False), _
            BindableAttribute(False), _
            DefaultValue(False), _
            DesignOnly(True), _
      DescriptionAttribute("Only Used With A DataGrid Control, To Eliminate Double Tabs")> _
     Public Property BeepOnError() As Boolean
        Get
            Return _bBeepOnError
        End Get
        Set(ByVal Value As Boolean)
            _bBeepOnError = Value
        End Set
    End Property

    <CategoryAttribute("FlexMaskedit Property"), DefaultValueAttribute(""), _
    DescriptionAttribute("Valid Keyboard Chars, Who Will Always Pass The Mask, Independend Mask Settings")> _
   Public Property SpecialChars() As String
        Get
            Return _sSpecialChars
        End Get
        Set(ByVal Value As String)
            _sSpecialChars = Value
        End Set
    End Property

    <CategoryAttribute("FlexMaskedit Property"), _
       DefaultValueAttribute(""), _
       DescriptionAttribute("Format Text On Exit Focus, FOR DISPLAY PURPOSE ONLY! else use GetFormatedText()")> _
    Public Property SetFormatString() As String
        Get
            Return _sFormatString
        End Get
        Set(ByVal Value As String)
            _sFormatString = Value
        End Set
    End Property

    <CategoryAttribute("FlexMaskedit Property"), _
       Browsable(False), _
       [ReadOnly](False), _
       BindableAttribute(True), _
       DefaultValueAttribute(""), _
       DesignOnly(True), _
       DescriptionAttribute("Error Text, Indicating To The End User An Input Error Occurs")> _
    Public Property ErrorTxt() As String
        Get
            Return _sErrorTxt
        End Get
        Set(ByVal Value As String)
            _sErrorTxt = Value
            Me.ErrorProvider1.SetError(Me, _sErrorTxt)
            If _sErrorTxt = Nothing Then
                If Me.Focused Then
                    MyBase.ForeColor = _FocusForeColor
                Else
                    MyBase.ForeColor = _ForeColor
                End If
            Else
                MyBase.ForeColor = _ErrorForeColor
            End If
        End Set
    End Property

    <CategoryAttribute("FlexMaskedit Property"), _
          Browsable(False), _
          [ReadOnly](False), _
          BindableAttribute(False), _
          DefaultValue(False), _
          DesignOnly(True), _
    DescriptionAttribute("Only Used With A DataGrid Control, To Eliminate Double Tabs")> _
 Public Property NoTab() As Boolean
        Get
            Return _NoTab
        End Get
        Set(ByVal Value As Boolean)
            _NoTab = Value
        End Set
    End Property

    <CategoryAttribute("FlexMaskedit Property"), _
 DescriptionAttribute("BackColor For TextBox When NO Focus")> _
 Public Overrides Property BackColor() As System.Drawing.Color
        Get
            Return MyBase.BackColor
        End Get
        Set(ByVal Value As System.Drawing.Color)
            _BackColor = Value
            MyBase.BackColor = Value
        End Set
    End Property

    <CategoryAttribute("FlexMaskedit Property"), _
          DescriptionAttribute("ForeColor For TextBox When NO Focus")> _
       Public Overrides Property ForeColor() As System.Drawing.Color
        Get
            Return MyBase.ForeColor
        End Get
        Set(ByVal Value As System.Drawing.Color)
            _ForeColor = Value
            MyBase.ForeColor = Value
        End Set
    End Property

    <CategoryAttribute("FlexMaskedit Property"), _
  DefaultValue(GetType(Color), "Window"), _
  DescriptionAttribute("BackColor For TextBox When Control Has Focus")> _
 Public Property FocusBackColor() As System.Drawing.Color
        Get
            Return _FocusBackColor
        End Get
        Set(ByVal Value As System.Drawing.Color)
            _FocusBackColor = Value
        End Set
    End Property

    <CategoryAttribute("FlexMaskedit Property"), _
    DefaultValue(GetType(Color), "WindowText"), _
       DescriptionAttribute("ForeColor For TextBox When Control Has Focus")> _
    Public Property FocusForeColor() As Color
        Get
            Return _FocusForeColor
        End Get
        Set(ByVal Value As Color)
            _FocusForeColor = Value
        End Set
    End Property

    <CategoryAttribute("FlexMaskedit Property"), _
    DefaultValue(GetType(Color), "Red"), _
       DescriptionAttribute("Error ForeColor BackColor When A Error Occurs")> _
       Public Property ErrorForeColor() As Color
        Get
            Return _ErrorForeColor
        End Get
        Set(ByVal Value As Color)
            _ErrorForeColor = Value
        End Set
    End Property

    <CategoryAttribute("FlexMaskedit Property"), _
          Browsable(True), _
          [ReadOnly](False), _
          BindableAttribute(True), _
          DefaultValue(""), _
          DesignOnly(False), _
          DescriptionAttribute("Text contained in the control")> _
 Public Overrides Property Text() As String
        Get
            If Me.DesignMode Then
                If (Not _sOriginalValue = Nothing) AndAlso _sOriginalValue.Length > 14 AndAlso _sOriginalValue.Substring(0, 15) = "FlexMaskEditBox" Then
                    Return ""
                End If
                Return _sOriginalValue
            ElseIf _bNoReturn Then
                Return ""
            End If
            Return Me.GetText()
        End Get
        Set(ByVal Value As String)
            If Me.DesignMode Then
                _sOriginalValue = Value
                Exit Property
            End If
            If Not _bMaskUsed Then Mask = _sMask
            _bNoReturn = True : MyBase.Text = Me.SetText(Value) : _bNoReturn = False
            If Not Me.Focused Then
                If Not _sFormatString = Nothing Then
                    _bNoReturn = True : MyBase.Text = Me.FormatData(Me.GetText()) : _bNoReturn = False
                End If
            Else
                If Not _Text = Nothing Then Me.GotoFirstPrompChar()
            End If
        End Set
    End Property

    <CategoryAttribute("FlexMaskedit Property"), _
             Browsable(False), _
             [ReadOnly](False), _
             BindableAttribute(False), _
             DefaultValue(50), _
             DesignOnly(True), _
             DescriptionAttribute("NOT Available, Flex Will Auto Adjust Length.")> _
       Public Overrides Property MaxLength() As Integer
        Get
            Return MyBase.MaxLength
        End Get
        Set(ByVal Value As Integer)
            MyBase.MaxLength = Value
        End Set
    End Property

    <CategoryAttribute("FlexMaskedit Property"), _
       DefaultValueAttribute(GetType(SelectTxt), "0"), _
       DescriptionAttribute("Select All The text On Focus")> _
       Public Property SelTxtOnEnter() As SelectTxt
        Get
            Return _SelectTxt
        End Get
        Set(ByVal Value As SelectTxt)
            _SelectTxt = Value
            _SelTxtAllowed = (Value <> SelectTxt.Never)
        End Set
    End Property

#End Region

End Class

