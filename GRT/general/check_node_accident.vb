Public Class check_node_accident

    Public Shared Function exe(na_confirmer As String) As Boolean

        Dim Selectcmd As MySqlCommand
        Dim Adapter As MySqlDataAdapter
        Dim Dataset As DataSet
        Dim accident As Boolean

        Dim Connection_mariadb_local_bc_manager As New MySqlConnection(GRT.GR.cString_mariadb_local_bc_manager)

        Connection_mariadb_local_bc_manager.Open()

        'CREATE PROCEDURE `up_select_node_info`
        '(IN p_na char(40))
        'BEGIN
        'SELECT * FROM node
        'WHERE na = p_na;
        'END

        Selectcmd = New MySqlCommand("up_select_node_info", Connection_mariadb_local_bc_manager)
        Selectcmd.Parameters.Add(New MySqlParameter("p_na", Regex.Replace(na_confirmer, "^0x", "")))

        Adapter = New MySqlDataAdapter
        Selectcmd.CommandType = CommandType.StoredProcedure
        Adapter.SelectCommand = Selectcmd
        Dataset = New DataSet
        Adapter.Fill(Dataset)

        accident = CBool(Dataset.Tables(0).Rows(0)("accident"))

        Return accident

    End Function

End Class
