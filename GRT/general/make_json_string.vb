Public Class make_json_string

    Public Shared Function exe(header(,) As String, value(,) As String, execte_escape As Boolean) As String

        Dim header_str, value_str As String

        Dim json_str As String =
            "{header_str, " &
            """value"" : " &
            "   {" &
            "       value_str" &
            "   }" &
            "}"

        header_str = "" : value_str = ""

        For i = 0 To CInt(header.Length / 3) - 1

            If header(i, 2) = "quot" Then

                header_str &= "    """ & escape(header(i, 0), execte_escape) & """ : """ & escape(header(i, 1), execte_escape) & """, "

            Else

                header_str &= "    """ & escape(header(i, 0), execte_escape) & """ : " & escape(header(i, 1), execte_escape) & ", "

            End If

        Next

        header_str = Regex.Replace(header_str, ",\s*$", "")

        For i = 0 To CInt(value.Length / 3) - 1

            If value(i, 2) = "quot" Then

                value_str &= "       """ & escape(value(i, 0), execte_escape) & """ : """ & escape(value(i, 1), execte_escape) & """, "

            Else

                value_str &= "       """ & escape(value(i, 0), execte_escape) & """ : " & escape(value(i, 1), execte_escape) & ", "

            End If

        Next

        value_str = Regex.Replace(value_str, ",\s*$", "")

        json_str = Regex.Replace(json_str, "header_str", header_str)

        json_str = Regex.Replace(json_str, "value_str", value_str)

        Return json_str

    End Function

    Public Shared Function exe_array(header(,) As String, value()(,) As String, execte_escape As Boolean) As String

        Dim header_str, value_str As String

        Dim json_str As String =
            "{header_str, " &
            """value"" : " &
            "       value_str" &
            "}"

        header_str = ""

        For i = 0 To CInt(header.Length / 3) - 1

            If header(i, 2) = "quot" Then

                header_str &= "    """ & escape(header(i, 0), execte_escape) & """ : """ & escape(header(i, 1), execte_escape) & """, "

            Else

                header_str &= "    """ & escape(header(i, 0), execte_escape) & """ : " & escape(header(i, 1), execte_escape) & ", "

            End If

        Next

        header_str = Regex.Replace(header_str, ",\s*$", "")

        value_str = "["

        For i = 0 To value.Length - 1

            value_str &= "{"

            For j = 0 To CInt(value(i).Length / 3) - 1

                If value(i)(j, 2) = "quot" Then

                    value_str &= "       """ & escape(value(i)(j, 0), execte_escape) & """ : """ & escape(value(i)(j, 1), execte_escape) & """, "

                Else

                    value_str &= "       """ & escape(value(i)(j, 0), execte_escape) & """ : " & escape(value(i)(j, 1), execte_escape) & ", "

                End If

            Next

            value_str = Regex.Replace(value_str, ",\s*$", "")

            value_str &= "},"

        Next

        value_str = Regex.Replace(value_str, ",\s*$", "")

        value_str &= "]"

        json_str = Regex.Replace(json_str, "header_str", header_str)

        json_str = Regex.Replace(json_str, "value_str", value_str)

        Return json_str

    End Function

    Shared Function escape(data As String, execte_escape As Boolean) As String

        If execte_escape Then

            data = Regex.Replace(data, """", "\""")

        End If

        Return data

    End Function

End Class
