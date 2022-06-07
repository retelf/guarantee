Public Class execute_query

    Public Shared Function exe(query_string As String) As String

        Dim Insertcmd As MySqlCommand

        Insertcmd = New MySqlCommand(query_string, GR.Connection_mariadb_local)
        Insertcmd.CommandType = CommandType.Text

        Insertcmd.ExecuteNonQuery()

    End Function

End Class
