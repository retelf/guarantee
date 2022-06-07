Imports System.Text

Public Class byte_to_hex

    Public Shared Function exe(bytes() As Byte) As String

        Dim hex As StringBuilder = New StringBuilder(bytes.Length * 2)

        For Each b As Byte In bytes
            hex.AppendFormat("{0:x2}", b)
        Next

        Return hex.ToString()

    End Function

End Class
