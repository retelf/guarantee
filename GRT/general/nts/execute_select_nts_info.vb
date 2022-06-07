Public Class execute_select_nts_info

    Public Shared Function exe(eoa As String) As DataSet

        Dim Selectcmd As MySqlCommand
        Dim Adapter As MySqlDataAdapter
        Dim Dataset As DataSet

        Using Connection As New MySqlConnection(GR.cString_mariadb_bc_nts)

            Connection.Open()

            'CREATE PROCEDURE `up_select_super_account`
            '(IN p_eoa  char(40))
            'BEGIN
            'SELECT * FROM super_account
            'WHERE
            '`public_key` = p_eoa;
            'END

            Selectcmd = New MySqlCommand("up_select_super_account", Connection)
            Selectcmd.Parameters.Add(New MySqlParameter("p_eoa", Regex.Replace(eoa, "^0x", "")))

            Adapter = New MySqlDataAdapter
            Selectcmd.CommandType = CommandType.StoredProcedure
            Adapter.SelectCommand = Selectcmd
            Dataset = New DataSet

            Adapter.Fill(Dataset)

            Connection.Close()

            Return Dataset

        End Using

    End Function

End Class
