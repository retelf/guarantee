Public Class check_representative

    Public Shared Function exe(eoa As String) As DataSet

        Dim Connection_mariadb_local_bc_manager As New MySqlConnection(GRT.GR.cString_mariadb_local_bc_manager)

        Connection_mariadb_local_bc_manager.Open()

        Dim Selectcmd As MySqlCommand
        Dim Adapter As MySqlDataAdapter
        Dim Dataset As DataSet

        'CREATE PROCEDURE `up_select_representative`
        '(IN p_eoa varchar(40))
        'BEGIN
        'SELECT representative
        'FROM account
        'WHERE
        '`eoa` = p_eoa;
        'END

        Selectcmd = New MySqlCommand("up_select_for_check_main_key_login", Connection_mariadb_local_bc_manager)
        Selectcmd.CommandType = CommandType.StoredProcedure
        Selectcmd.Parameters.Add(New MySqlParameter("p_eoa", Regex.Replace(eoa, "^0x", "")))

        Adapter = New MySqlDataAdapter
        Selectcmd.CommandType = CommandType.StoredProcedure
        Adapter.SelectCommand = Selectcmd
        Dataset = New DataSet
        Adapter.Fill(Dataset)

        Connection_mariadb_local_bc_manager.Close()

        Return Dataset

    End Function

End Class
