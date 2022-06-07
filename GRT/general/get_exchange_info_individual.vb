Imports Newtonsoft.Json

Public Class get_exchange_info_individual

    Public Shared Function exe(block_number As Long) As DataSet

        Dim Selectcmd As MySqlCommand
        Dim Adapter As MySqlDataAdapter
        Dim Dataset As DataSet

        Dim Connection_mariadb_local As New MySqlConnection(GRT.GR.cString_mariadb_local_bc)

        Connection_mariadb_local.Open()

        'CREATE PROCEDURE `up_select_exchange_info_with_block_number`
        '(IN p_block_number bigint(20))
        'BEGIN
        'SELECT * FROM exchange
        'WHERE
        '`block_number` = p_block_number;
        'END

        Selectcmd = New MySqlCommand("up_select_exchange_info_with_block_number", Connection_mariadb_local)
        Selectcmd.Parameters.Add(New MySqlParameter("p_block_number", block_number))

        Adapter = New MySqlDataAdapter
        Selectcmd.CommandType = CommandType.StoredProcedure
        Adapter.SelectCommand = Selectcmd
        Dataset = New DataSet
        Adapter.Fill(Dataset)

        Connection_mariadb_local.Close()

        Return Dataset

    End Function

End Class
