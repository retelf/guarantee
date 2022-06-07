Public Class get_ma

    Public Shared Function exe(block_number As Long) As String

        Dim Connection_mariadb_local_bc_multilevel As New MySqlConnection(GRT.GR.cString_mariadb_local_bc_multilevel)

        Connection_mariadb_local_bc_multilevel.Open()

        Dim Selectcmd As MySqlCommand
        Dim Adapter As MySqlDataAdapter
        Dim Dataset As DataSet

        'CREATE PROCEDURE `up_select_ma`
        '(IN p_block_number bigint(20))
        'BEGIN
        'SELECT ma
        'FROM account
        'WHERE block_number = p_block_number;
        'END

        Selectcmd = New MySqlCommand("up_select_ma", Connection_mariadb_local_bc_multilevel)
        Selectcmd.Parameters.Add(New MySqlParameter("p_block_number", block_number))

        Adapter = New MySqlDataAdapter
        Selectcmd.CommandType = CommandType.StoredProcedure
        Adapter.SelectCommand = Selectcmd
        Dataset = New DataSet
        Adapter.Fill(Dataset)

        Return "0x" & CStr(Dataset.Tables(0).Rows(0)(0))

    End Function

End Class
