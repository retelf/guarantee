Imports System.Text

Public Class unicode_to_utf8
    Public Shared Function exe(unicode_string As String) As String

        Dim unicode As Encoding = Encoding.Unicode

        Dim utf8 As Encoding = Encoding.UTF8

        Dim unicodeBytes As Byte() = unicode.GetBytes(unicode_string)

        Dim utf8Bytes As Byte() = Encoding.Convert(unicode, utf8, unicodeBytes)

        Return CStr(Encoding.UTF8.GetString(utf8Bytes, 0, utf8Bytes.Length))

    End Function

End Class
