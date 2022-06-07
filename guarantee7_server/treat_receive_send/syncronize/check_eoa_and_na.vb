Public Class check_eoa_and_na

    Public Shared Function exe(received_eoa As String, received_na As String) As Boolean

        Dim eoa As String
        Dim bool As Boolean

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
        Selectcmd.Parameters.Add(New MySqlParameter("p_na", Regex.Replace(received_na, "^0x", "")))

        Adapter = New MySqlDataAdapter
        Selectcmd.CommandType = CommandType.StoredProcedure
        Adapter.SelectCommand = Selectcmd
        Dataset = New DataSet
        Adapter.Fill(Dataset)

        If Dataset.Tables(0).Rows.Count = 1 Then

            eoa = "0x" & CStr(Dataset.Tables(0).Rows(0)("eoa"))

            If eoa = received_eoa Then
                bool = True
            Else
                bool = False
            End If

        Else
            bool = False
        End If

        Connection_mariadb_local_bc_manager.Close()

        Return bool

    End Function

End Class
