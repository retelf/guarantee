Public Class execute_insert_query_bak

    Public Shared Function exe(block_number As Long, table_name As String, query_string As String, cString As String) As String

        Dim query_string_insert, query_string_update As String

        query_string_insert = query_string

        query_string_update = "UPDATE " & table_name & " SET `block_number` = " & block_number & " WHERE block_number = 0;"

        Dim Connection As New MySqlConnection(cString)

        Connection.Open()

        Dim Command As MySqlCommand

        Command = New MySqlCommand(query_string_insert & query_string_update, Connection)
        Command.CommandType = CommandType.Text

        Command.ExecuteNonQuery()

        Connection.Close()

    End Function

End Class

