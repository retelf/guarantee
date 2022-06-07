Public Class manage_confirm_nft

    Public Shared Sub exe()

        'Dim Selectcmd, Updatecmd As MySqlCommand
        'Dim Adapter As MySqlDataAdapter
        'Dim Dataset As DataSet
        'Dim idate_generated, idate_confirmed As DateTime
        'Dim time_taken As TimeSpan

        'Dim Connection_mariadb_local_bc_agent As New MySqlConnection(GRT.GR.cString_mariadb_local_bc_agent)

        'Connection_mariadb_local_bc_agent.Open()

        ''CREATE PROCEDURE `up_select_idate_generated`
        ''(IN p_signiture_key char(64))
        ''BEGIN
        ''SELECT idate_generated
        ''FROM agent_record
        ''WHERE
        ''`signiture_key` = p_signiture_key;
        ''END

        'Selectcmd = New MySqlCommand("up_select_idate_generated", Connection_mariadb_local_bc_agent)
        'Selectcmd.CommandType = CommandType.StoredProcedure
        'Selectcmd.Parameters.Add(New MySqlParameter("p_signiture_key", Regex.Replace(signiture_key, "^0x", "")))

        'Adapter = New MySqlDataAdapter
        'Selectcmd.CommandType = CommandType.StoredProcedure
        'Adapter.SelectCommand = Selectcmd
        'Dataset = New DataSet
        'Adapter.Fill(Dataset)

    End Sub

End Class
