Public Class get_exchange_name

    Public Shared Function exe(na As String) As String

        Dim exchange_name As String

        Dim Selectcmd As MySqlCommand
        Dim Adapter As MySqlDataAdapter
        Dim Dataset As DataSet

        Dim Connection_mariadb_local_bc_manager As New MySqlConnection(GRT.GR.cString_mariadb_local_bc_manager)

        Connection_mariadb_local_bc_manager.Open()

        'CREATE PROCEDURE `up_select_node_info`
        '(IN p_na char(40))
        'BEGIN
        'SELECT *
        'FROM node
        'WHERE na = p_na;
        'END

        Selectcmd = New MySqlCommand("up_select_node_info", Connection_mariadb_local_bc_manager)
        Selectcmd.Parameters.Add(New MySqlParameter("p_na", Regex.Replace(na, "^0x", "")))

        Adapter = New MySqlDataAdapter
        Selectcmd.CommandType = CommandType.StoredProcedure
        Adapter.SelectCommand = Selectcmd
        Dataset = New DataSet
        Adapter.Fill(Dataset)

        Connection_mariadb_local_bc_manager.Close()

        exchange_name = "0x" & CStr(Dataset.Tables(0).Rows(0)("exchange_name"))

        Return exchange_name

    End Function

End Class
