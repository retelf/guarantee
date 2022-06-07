Public Class generate_node_list_script

    Public Shared Function exe() As String

        Dim select_string, na As String

        Dim Connection_mariadb_local As New MySqlConnection(GR.cString_mariadb_local_bc_manager)

        Connection_mariadb_local.Open()

        Dim Selectcmd As MySqlCommand
        Dim Adapter As MySqlDataAdapter
        Dim Dataset As DataSet

        'CREATE PROCEDURE `up_select_node_list`
        '()
        'BEGIN
        'SELECT na
        'FROM node
        'WHERE
        '`running` = 1
        'ORDER BY
        '`level` DESC, `point` DESC;
        'END

        Selectcmd = New MySqlCommand("up_select_node_list", Connection_mariadb_local)
        Selectcmd.CommandType = CommandType.StoredProcedure

        Adapter = New MySqlDataAdapter
        Selectcmd.CommandType = CommandType.StoredProcedure
        Adapter.SelectCommand = Selectcmd
        Dataset = New DataSet
        Adapter.Fill(Dataset)

        Connection_mariadb_local.Close()

        select_string =
            "<select id='select_node'>" &
            "<option value='select_exchange' selected>거래소 선택</option>"

        For i = 0 To Dataset.Tables(0).Rows.Count - 1

            na = CStr(Dataset.Tables(0).Rows(i)(0))

            select_string &= "<option value='0x" & na & "'>" & "0x" & na & "</option>"

        Next

        select_string &= "</select>"

        Return select_string

    End Function

End Class
