Public Class get_server_private_key

    Public Shared Function exe(password As String) As String

        Using Connection_mariadb_local_bc As New MySqlConnection(GR.cString_mariadb_local_bc)

            Connection_mariadb_local_bc.Open()

            Dim Selectcmd As MySqlCommand
            Dim Adapter As MySqlDataAdapter
            Dim Dataset As DataSet

            'CREATE PROCEDURE `up_select_server_private_key`
            '(IN p_password varchar(50))
            'BEGIN
            'SELECT private_key
            'FROM key_store
            'WHERE
            '`password` = p_password;
            'END

            Selectcmd = New MySqlCommand("up_select_server_private_key", Connection_mariadb_local_bc)
            Selectcmd.CommandType = CommandType.StoredProcedure
            Selectcmd.Parameters.Add(New MySqlParameter("p_password", password))

            Adapter = New MySqlDataAdapter
            Selectcmd.CommandType = CommandType.StoredProcedure
            Adapter.SelectCommand = Selectcmd
            Dataset = New DataSet
            Adapter.Fill(Dataset)

            Connection_mariadb_local_bc.Close()

            If Dataset.Tables(0).Rows.Count = 1 Then

                Return CStr(Dataset.Tables(0).Rows(0)(0))

            Else

                Return "password_not_match"

            End If

        End Using

    End Function

End Class
