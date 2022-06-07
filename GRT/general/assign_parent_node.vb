Public Class assign_parent_node
    Public Structure st_data

        Dim id As String
        Dim level As Integer
        Dim count_children As Integer

    End Structure : Public Shared data As st_data

    Public Shared Sub exe()

        Dim Connection_mariadb_local_bc_manager As New MySqlConnection(GRT.GR.cString_mariadb_local_bc_manager)

        Connection_mariadb_local_bc_manager.Open()

        Dim Selectcmd As MySqlCommand
        Dim Adapter As MySqlDataAdapter
        Dim Dataset As DataSet

        Dim count_result As Integer

        'CREATE PROCEDURE `up_select_parent_server_address_general`
        '(IN p_node_max_children_count int(11))
        'BEGIN
        'SELECT na, level, count_children
        'FROM node
        'WHERE count_children > 0 AND count_children < p_node_max_children_count
        'ORDER BY level DESC, point DESC, count_children ASC
        'LIMIT 1;
        'END

        Selectcmd = New MySqlCommand("up_select_parent_server_address_general", Connection_mariadb_local_bc_manager)
        Selectcmd.CommandType = CommandType.StoredProcedure
        Selectcmd.Parameters.Add(New MySqlParameter("p_node_max_children_count", GRT.GR.node_max_children_count))

        Adapter = New MySqlDataAdapter
        Selectcmd.CommandType = CommandType.StoredProcedure
        Adapter.SelectCommand = Selectcmd
        Dataset = New DataSet
        Adapter.Fill(Dataset)

        count_result = Dataset.Tables(0).Rows.Count

        If count_result = 0 Then

            'CREATE PROCEDURE `up_select_parent_server_address_special`
            '()
            'BEGIN
            'SELECT na, level, count_children
            'FROM node
            'ORDER BY level DESC, point DESC
            'LIMIT 1;
            'END

            Selectcmd = New MySqlCommand("up_select_parent_server_address_special", Connection_mariadb_local_bc_manager)
            Selectcmd.CommandType = CommandType.StoredProcedure

            Adapter = New MySqlDataAdapter
            Selectcmd.CommandType = CommandType.StoredProcedure
            Adapter.SelectCommand = Selectcmd
            Dataset = New DataSet
            Adapter.Fill(Dataset)

        End If

        Connection_mariadb_local_bc_manager.Close()

        data.id = CStr(Dataset.Tables(0).Rows(0)("na"))
        data.level = CInt(Dataset.Tables(0).Rows(0)("level"))
        data.count_children = CInt(Dataset.Tables(0).Rows(0)("count_children"))

    End Sub

End Class
