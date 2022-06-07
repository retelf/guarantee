Public Class execute_treated_query

    Public Shared Sub exe(treated_query As String, database_name As String)

        Dim cString As String = get_cString.exe(database_name)

        Dim Connection As New MySqlConnection(cString)

        Connection.Open()

        Dim Command As MySqlCommand

        Command = New MySqlCommand(treated_query, Connection)
        Command.CommandType = CommandType.Text

        Command.ExecuteNonQuery()

        Connection.Close()

    End Sub

End Class
