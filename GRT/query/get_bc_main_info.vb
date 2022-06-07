Public Class get_bc_main_info

    Public Shared Function max_number_block_number_and_hash() As DataSet

        Dim Connection_mariadb_local_bc As New MySqlConnection(GRT.GR.cString_mariadb_local_bc)

        Connection_mariadb_local_bc.Open()

        Dim Selectcmd As MySqlCommand
        Dim Adapter As MySqlDataAdapter
        Dim Dataset As DataSet

        'CREATE PROCEDURE `up_select_max_number_block_number_and_hash_bc_main`
        '()
        'BEGIN
        'SELECT block_number, block_hash FROM `main`
        'ORDER BY block_number DESC
        'LIMIT 1;
        'END

        Selectcmd = New MySqlCommand("up_select_max_number_block_number_and_hash_bc_main", Connection_mariadb_local_bc)
        Selectcmd.CommandType = CommandType.StoredProcedure

        Adapter = New MySqlDataAdapter
        Selectcmd.CommandType = CommandType.StoredProcedure
        Adapter.SelectCommand = Selectcmd
        Dataset = New DataSet
        Adapter.Fill(Dataset)

        Connection_mariadb_local_bc.Close()

        Return Dataset

    End Function

End Class
