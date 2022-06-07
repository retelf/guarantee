Public Class get_local_main_max_block_number

    Public Shared Function exe() As Long

        Dim Selectcmd As MySqlCommand
        Dim Adapter As MySqlDataAdapter
        Dim max_block_number As Long
        Dim dataset As DataSet

        'CREATE PROCEDURE `up_select_max_block_number`()
        'BEGIN
        'SELECT MAX(block_number)
        'FROM main;
        'END

        Selectcmd = New MySqlCommand("up_select_max_block_number", GRT.GR.Connection_mariadb_local_bc)
        Selectcmd.CommandType = CommandType.StoredProcedure

        Adapter = New MySqlDataAdapter
        Selectcmd.CommandType = CommandType.StoredProcedure
        Adapter.SelectCommand = Selectcmd
        dataset = New DataSet
        Adapter.Fill(dataset)

        If Not dataset.Tables(0).Rows(0)(0) Is DBNull.Value Then

            max_block_number = CInt(dataset.Tables(0).Rows(0)(0))

        Else

            max_block_number = 0

        End If

        Return max_block_number

    End Function

End Class
