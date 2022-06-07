Public Class execute_nts_query

    Public Shared Sub exe(
                            block_number As Long,
                            block_hash As String,
                            eoa As String,
                            database_name As String,
                            table_name As String,
                            query_type As String,
                            contract_type As String,
                            query_string As String,
                            signiture As String,
                            algorithm As String,
                            individual_treated_query As String)

        Using Connection As New MySqlConnection(GR.cString_mariadb_bc_nts)

            Connection.Open()

            Dim Command As MySqlCommand

            Dim query As String = "SET AUTOCOMMIT = 0;"

            query &=
                "USE bc_nts;" &
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
                "algorithm," &
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
                "'" & Regex.Replace(query_string, "'", "\'") & "'," &
                "'" & signiture & "'," &
                "'" & algorithm & "'," &
                "'" & DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") & "');"

            query &= individual_treated_query

            query &= "COMMIT;"

            Command = New MySqlCommand(query, Connection)
            Command.CommandType = CommandType.Text

            Command.ExecuteNonQuery()

            Connection.Close()

        End Using

    End Sub

End Class
