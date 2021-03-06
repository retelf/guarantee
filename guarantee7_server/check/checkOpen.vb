Public Class checkOpen

    Public Shared Function balance(public_key As String) As Boolean()

        Dim Connection_mariadb_manager As New MySqlConnection(GR.cString_mariadb_manager)

        Connection_mariadb_manager.Open()

        Dim Selectcmd As MySqlCommand
        Dim Adapter As MySqlDataAdapter
        Dim Dataset As DataSet

        'CREATE PROCEDURE `up_select_case_open_balance`
        '(IN p_eoa  varchar(40))
        'BEGIN
        'SELECT o_balance
        'FROM security_coin
        'WHERE
        '`eoa` = p_eoa;
        'END

        Selectcmd = New MySqlCommand("up_select_case_open_balance", Connection_mariadb_manager)
        Selectcmd.CommandType = CommandType.StoredProcedure
        Selectcmd.Parameters.Add(New MySqlParameter("p_eoa", Regex.Replace(public_key, "^0x", "")))

        Adapter = New MySqlDataAdapter
        Selectcmd.CommandType = CommandType.StoredProcedure
        Adapter.SelectCommand = Selectcmd
        Dataset = New DataSet
        Adapter.Fill(Dataset)

        If Dataset.Tables(0).Rows.Count > 0 Then
            Return {True, CBool(Dataset.Tables(0).Rows(0)((0)))}
        Else
            Return {False, False}
        End If

        Connection_mariadb_manager.Close()

    End Function

End Class