Public Class get_nts_main_info

    Public Shared Function max_number_block_info() As DataSet

        Dim Connection_mariadb_bc_nts As New MySqlConnection(GRT.GR.cString_mariadb_bc_nts)

        Connection_mariadb_bc_nts.Open()

        Dim Selectcmd As MySqlCommand
        Dim Adapter As MySqlDataAdapter
        Dim Dataset As DataSet

        'CREATE PROCEDURE `up_select_max_number_block_info_nts_main`
        '()
        'BEGIN
        'SELECT * FROM `main`
        'ORDER BY block_number DESC
        'LIMIT 1;
        'END

        Selectcmd = New MySqlCommand("up_select_max_number_block_info_nts_main", Connection_mariadb_bc_nts)
        Selectcmd.CommandType = CommandType.StoredProcedure

        Adapter = New MySqlDataAdapter
        Selectcmd.CommandType = CommandType.StoredProcedure
        Adapter.SelectCommand = Selectcmd
        Dataset = New DataSet
        Adapter.Fill(Dataset)

        Connection_mariadb_bc_nts.Close()

        Return Dataset

    End Function

End Class
