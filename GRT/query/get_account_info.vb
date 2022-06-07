Public Class get_account_info

    Public Shared Function max_number_block_info() As DataSet

        Dim Connection_mariadb_local_bc As New MySqlConnection(GRT.GR.cString_mariadb_local_bc)

        Connection_mariadb_local_bc.Open()

        Dim Selectcmd As MySqlCommand
        Dim Adapter As MySqlDataAdapter
        Dim Dataset As DataSet

        'CREATE PROCEDURE `up_select_max_number_block_info_main`
        '()
        'BEGIN
        'SELECT * FROM `main`
        'ORDER BY num_id DESC
        'LIMIT 1;
        'END

        Selectcmd = New MySqlCommand("up_select_max_number_block_info_main", Connection_mariadb_local_bc)
        Selectcmd.CommandType = CommandType.StoredProcedure

        Adapter = New MySqlDataAdapter
        Adapter.SelectCommand = Selectcmd
        Dataset = New DataSet
        Adapter.Fill(Dataset)

        Connection_mariadb_local_bc.Close()

        Return Dataset

    End Function

End Class
