Public Class execute_initial_login

    Public Shared Function exe(public_key As String) As Boolean

        public_key = Regex.Replace(public_key, "^0x", "")

        Dim Connection_mariadb_bc_nts As New MySqlConnection(GRT.GR.cString_mariadb_bc_nts)

        Connection_mariadb_bc_nts.Open()

        Dim Selectcmd As MySqlCommand
        Dim Adapter As MySqlDataAdapter
        Dim Dataset As DataSet

        'CREATE PROCEDURE `up_select_count_public_key`
        '(IN p_public_key char(40))
        'BEGIN
        'SELECT COUNT(*)
        'FROM bc_nts.super_account
        'WHERE
        '`public_key` = p_public_key;
        'END

        Selectcmd = New MySqlCommand("up_select_count_public_key", Connection_mariadb_bc_nts)
        Selectcmd.CommandType = CommandType.StoredProcedure
        Selectcmd.Parameters.Add(New MySqlParameter("p_public_key", public_key))

        Adapter = New MySqlDataAdapter
        Selectcmd.CommandType = CommandType.StoredProcedure
        Adapter.SelectCommand = Selectcmd
        Dataset = New DataSet
        Adapter.Fill(Dataset)

        If CInt(Dataset.Tables(0).Rows(0)(0)) > 0 Then

            Return True

        Else
            Return False
        End If

        Connection_mariadb_bc_nts.Close()

    End Function

End Class
