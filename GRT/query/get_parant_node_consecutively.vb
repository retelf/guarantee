Public Class get_parant_node_consecutively

    Public Shared Function exe(public_key As String) As DataSet

        Dim Connection_mariadb_local As New MySqlConnection(GR.cString_mariadb_local_bc_manager)

        Connection_mariadb_local.Open()

        Dim Selectcmd As MySqlCommand
        Dim Adapter As MySqlDataAdapter
        Dim Dataset As DataSet
        Dim parent_eoa, parent_domain, parent_ip As String
        Dim parent_port As Integer
        Dim parent_main

        While True

            'CREATE PROCEDURE `up_select_parant_node`
            '(IN p_eoa  varchar(40))
            'BEGIN
            'SELECT *
            'FROM node
            'WHERE
            '`eoa` = p_eoa;
            'END

            Selectcmd = New MySqlCommand("up_select_parant_node", Connection_mariadb_local)
            Selectcmd.CommandType = CommandType.StoredProcedure
            Selectcmd.Parameters.Add(New MySqlParameter("p_eoa", Regex.Replace(public_key, "^0x", "")))

            Adapter = New MySqlDataAdapter
            Selectcmd.CommandType = CommandType.StoredProcedure
            Adapter.SelectCommand = Selectcmd
            Dataset = New DataSet
            Adapter.Fill(Dataset)

            If Dataset.Tables(0).Rows.Count = 1 Then

                ' 접속해 본다.

                parent_eoa = CStr(Dataset.Tables(0).Rows(0)("parent"))
                parent_domain = CStr(Dataset.Tables(0).Rows(0)("ip"))
                parent_ip = CStr(Dataset.Tables(0).Rows(0)("ip"))
                parent_port = CInt(Dataset.Tables(0).Rows(0)("ip"))
                parent_main = CBool(Dataset.Tables(0).Rows(0)("ip"))









                Exit While

            Else
                Exit While
            End If

        End While

        Connection_mariadb_local.Close()

        Return Dataset

    End Function

End Class
