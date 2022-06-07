Public Class execute_query

    Public Shared Sub exe(
                        block_number As Long,
                        block_hash As String,
                        eoa As String,
                        database_name As String,
                        table_name As String,
                        query_type As String,
                        contract_type As String,
                        pure_query As String,
                        signiture As String,
                        treated_query_string As String)

        Using Connection As New MySqlConnection(GR.cString_mariadb_local)

            Connection.Open()

            Dim Command As MySqlCommand

            Dim query As String = "SET AUTOCOMMIT = 0;"

            query &=
                "USE bc;" &
                "INSERT INTO main" &
                "(" &
                "block_number," &
                "block_hash," &
                "eoa," &
                "database_name," &
                "table_name," &
                "query_type," &
                "contract_type," &
                "query_string," &
                "signiture," &
                "treated_query_string," &
                "signiture_key," &
                "idate" &
                ")" &
                "VALUES" &
                "(" &
                "" & block_number & "," &
                "'" & Regex.Replace(block_hash, "^0x", "") & "'," &
                "'" & Regex.Replace(eoa, "^0x", "") & "'," &
                "'" & database_name & "'," &
                "'" & table_name & "'," &
                "'" & query_type & "'," &
                "'" & contract_type & "'," &
                "'" & Regex.Replace(pure_query, "'", "\'") & "'," &
                "'" & signiture & "'," &
                "'" & Regex.Replace(treated_query_string, "'", "\'") & "'," &
                "'" & Regex.Replace(Regex.Match(signiture, "^0x.{64}").ToString, "^0x", "") & "'," &
                "'" & DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") & "');"

            query &= treated_query_string

            query &= "COMMIT;"

            Command = New MySqlCommand(query, Connection)
            Command.CommandType = CommandType.Text

            Command.ExecuteNonQuery()

            Connection.Close()

        End Using

    End Sub

End Class
