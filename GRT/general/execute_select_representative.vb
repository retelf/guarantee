Public Class execute_select_representative

    Public Shared Function exe(eoa As String) As String

        Dim Selectcmd As MySqlCommand
        Dim Adapter As MySqlDataAdapter
        Dim Dataset As DataSet

        Using Connection As New MySqlConnection(GR.cString_mariadb_local_bc_manager)

            Connection.Open()

            'CREATE PROCEDURE `up_select_representative`
            '(IN p_eoa  char(40))
            'BEGIN
            'SELECT representative FROM account
            'WHERE
            '`eoa` = p_eoa;
            'END

            Selectcmd = New MySqlCommand("up_select_representative", Connection)
            Selectcmd.Parameters.Add(New MySqlParameter("p_eoa", Regex.Replace(eoa, "^0x", "")))

            Adapter = New MySqlDataAdapter
            Selectcmd.CommandType = CommandType.StoredProcedure
            Adapter.SelectCommand = Selectcmd
            Dataset = New DataSet

            Adapter.Fill(Dataset)

            Connection.Close()

            If Dataset.Tables(0).Rows.Count = 1 Then
                Return CStr(Dataset.Tables(0).Rows(0)(0))
            Else
                Return "no_account"
            End If

        End Using

    End Function

End Class
