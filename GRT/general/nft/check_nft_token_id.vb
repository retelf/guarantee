Public Class check_nft_token_id

    Public Shared Function exe(nfa As String) As Integer

        Dim Selectcmd As MySqlCommand
        Dim Adapter As MySqlDataAdapter
        Dim Dataset As DataSet
        Dim token_id As Integer

        Dim Connection_mariadb_local_bc_nft As New MySqlConnection(GRT.GR.cString_mariadb_local_bc_nft)

        Connection_mariadb_local_bc_nft.Open()

        'CREATE PROCEDURE `up_select_nft_token_id`
        '(IN p_nfa char(40))
        'BEGIN
        'SELECT sub_count
        'FROM account
        'WHERE `nfa` = p_nfa;
        'END

        Selectcmd = New MySqlCommand("up_select_nft_token_id", Connection_mariadb_local_bc_nft)
        Selectcmd.CommandType = CommandType.StoredProcedure
        Selectcmd.Parameters.Add(New MySqlParameter("p_nfa", Regex.Replace(nfa, "^0x", "")))

        Adapter = New MySqlDataAdapter
        Adapter.SelectCommand = Selectcmd
        Dataset = New DataSet
        Adapter.Fill(Dataset)

        If Dataset.Tables(0).Rows.Count = 1 Then
            token_id = CInt(Dataset.Tables(0).Rows(0)(0))
        Else
            token_id = 0
        End If

        Return token_id

    End Function

End Class
