Public Class check_node_main

    Public Shared Function exe(requesting_na As String) As Boolean

        Dim main As Boolean

        Dim Selectcmd As MySqlCommand
        Dim Adapter As MySqlDataAdapter
        Dim Dataset As DataSet

        Dim Connection_mariadb_local_bc_manager As New MySqlConnection(GRT.GR.cString_mariadb_local_bc_manager)

        Connection_mariadb_local_bc_manager.Open()

        'CREATE PROCEDURE `up_check_node_main`
        '(IN p_requesting_na char(40))
        'BEGIN
        'SELECT main
        'FROM node
        'WHERE na = p_requesting_na;
        'END

        Selectcmd = New MySqlCommand("up_check_node_main", Connection_mariadb_local_bc_manager)
        Selectcmd.Parameters.Add(New MySqlParameter("p_requesting_na", Regex.Replace(requesting_na, "^0x", "")))

        Adapter = New MySqlDataAdapter
        Selectcmd.CommandType = CommandType.StoredProcedure
        Adapter.SelectCommand = Selectcmd
        Dataset = New DataSet
        Adapter.Fill(Dataset)

        main = CBool(Dataset.Tables(0).Rows(0)(0))

        Return main

    End Function

End Class
