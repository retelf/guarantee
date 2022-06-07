Public Class make_json_str_bak

    Public Shared Function exe(key As String, value(,) As String) As String

        Dim json_str As String =
            "{""key"" : """ & key & """, " &
            """value"" : " &
            "   {" &
            "       value_str" &
            "   }" &
            "}"

        Dim value_str As String = ""

        For i = 0 To CInt(value.Length / 3) - 1

            If value(i, 2) = "quot" Then

                value_str &= "       """ & value(i, 0) & """ : """ & value(i, 1) & """, "

            Else

                value_str &= "       """ & value(i, 0) & """ : " & value(i, 1) & ", "

            End If

        Next

        value_str = Regex.Replace(value_str, ",\s*$", "")

        json_str = Regex.Replace(json_str, "value_str", value_str)

        Return json_str

    End Function

End Class
