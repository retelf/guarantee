Public Class execute_coin_login

    Public Shared Function exe(eoa As String, coin_name As String) As Boolean

        Dim Connection_mariadb_local As New MySqlConnection(GR.cString_mariadb_local_bc_manager)

        Connection_mariadb_local.Open()

        Dim Selectcmd As MySqlCommand
        Dim Adapter As MySqlDataAdapter
        Dim Dataset As DataSet

        'CREATE PROCEDURE `up_select_eoa`
        '(IN p_eoa  varchar(40),
        'IN p_coin_name  varchar(50))
        'BEGIN
        'SELECT COUNT(*)
        'FROM bc_manager.account
        'WHERE
        '`eoa` = p_eoa AND
        '`coin_name` = p_coin_name;
        'END

        Selectcmd = New MySqlCommand("up_select_eoa", Connection_mariadb_local)
        Selectcmd.CommandType = CommandType.StoredProcedure
        Selectcmd.Parameters.Add(New MySqlParameter("p_eoa", Regex.Replace(eoa, "^0x", "")))
        Selectcmd.Parameters.Add(New MySqlParameter("p_coin_name", coin_name))

        Adapter = New MySqlDataAdapter
        Selectcmd.CommandType = CommandType.StoredProcedure
        Adapter.SelectCommand = Selectcmd
        Dataset = New DataSet
        Adapter.Fill(Dataset)

        Connection_mariadb_local.Close()

        If CInt(Dataset.Tables(0).Rows(0)((0))) > 0 Then
            Return True
        Else
            Return False
        End If

    End Function

End Class
