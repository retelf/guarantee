Public Class get_node_level

    Public Shared Function exe(requesting_na As String) As Integer

        Dim level As Integer = -1

        Dim Selectcmd As MySqlCommand
        Dim Adapter As MySqlDataAdapter
        Dim Dataset As DataSet

        Dim Connection_mariadb_local_bc_manager As New MySqlConnection(GRT.GR.cString_mariadb_local_bc_manager)

        Connection_mariadb_local_bc_manager.Open()

        'CREATE PROCEDURE `up_select_node_level`
        '(IN p_requesting_na char(40))
        'BEGIN
        'SELECT level
        'FROM node
        'WHERE na = p_requesting_na;
        'END

        Selectcmd = New MySqlCommand("up_select_node_level", Connection_mariadb_local_bc_manager)
        Selectcmd.Parameters.Add(New MySqlParameter("p_requesting_na", Regex.Replace(requesting_na, "^0x", "")))

        Adapter = New MySqlDataAdapter
        Selectcmd.CommandType = CommandType.StoredProcedure
        Adapter.SelectCommand = Selectcmd
        Dataset = New DataSet
        Adapter.Fill(Dataset)

        If Dataset.Tables(0).Rows.Count = 1 Then

            level = CInt(Dataset.Tables(0).Rows(0)(0))

        End If

        Return level

    End Function

End Class
