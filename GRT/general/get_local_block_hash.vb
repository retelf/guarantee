Public Class get_local_block_hash

    Public Shared Function exe(block_number As Long) As String

        Dim Selectcmd As MySqlCommand
        Dim Adapter As MySqlDataAdapter
        Dim block_hash As String
        Dim dataset As DataSet

        'CREATE PROCEDURE `up_select_block_hash`
        '(IN p_block_number bigint)
        'BEGIN
        'SELECT block_hash
        'FROM main
        'WHERE block_number = p_block_number;
        'END

        Selectcmd = New MySqlCommand("up_select_block_hash", GRT.GR.Connection_mariadb_local_bc)
        Selectcmd.CommandType = CommandType.StoredProcedure
        Selectcmd.Parameters.Add(New MySqlParameter("p_block_number", block_number))

        Adapter = New MySqlDataAdapter
        Selectcmd.CommandType = CommandType.StoredProcedure
        Adapter.SelectCommand = Selectcmd
        dataset = New DataSet
        Adapter.Fill(dataset)

        If dataset.Tables(0).Rows.Count > 0 Then

            block_hash = CStr(dataset.Tables(0).Rows(0)(0))

        Else

            block_hash = GRT.GR.genesis_block_hash

        End If

        Return block_hash

    End Function

End Class
