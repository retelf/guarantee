Public Class get_node_from_eoa

    Public Shared Function exe(eoa As String) As String

        Dim node As String = ""

        Dim Selectcmd As MySqlCommand
        Dim Adapter As MySqlDataAdapter
        Dim Dataset As DataSet

        Dim Connection_mariadb_local_bc_manager As New MySqlConnection(GRT.GR.cString_mariadb_local_bc_manager)

        Connection_mariadb_local_bc_manager.Open()

        'CREATE PROCEDURE `up_select_node_from_eoa`
        '(IN p_eoa char(40))
        'BEGIN
        'SELECT node
        'FROM account
        'WHERE eoa = p_eoa;
        'END

        Selectcmd = New MySqlCommand("up_select_node_from_eoa", Connection_mariadb_local_bc_manager)
        Selectcmd.Parameters.Add(New MySqlParameter("p_eoa", Regex.Replace(eoa, "^0x", "")))

        Adapter = New MySqlDataAdapter
        Selectcmd.CommandType = CommandType.StoredProcedure
        Adapter.SelectCommand = Selectcmd
        Dataset = New DataSet
        Adapter.Fill(Dataset)

        Connection_mariadb_local_bc_manager.Close()

        If Dataset.Tables(0).Rows.Count = 1 Then
            node = CStr(Dataset.Tables(0).Rows(0)(0))
        End If

        Return node

    End Function

End Class
