Public Class get_parent_hierarchy

    Public Shared Sub exe(closest_parent As String)

        Dim Connection_mariadb_local_bc_multilevel As New MySqlConnection(GRT.GR.cString_mariadb_local_bc_multilevel)

        Connection_mariadb_local_bc_multilevel.Open()

        Dim Selectcmd As MySqlCommand
        Dim Adapter As MySqlDataAdapter
        Dim Dataset As DataSet

        Dim hierarchy_i_number As Integer = 0
        Dim parent As String
        Dim level As Integer
        Dim ma As String = closest_parent

        ReDim assign_parent_ma.data.parent(0)

        assign_parent_ma.data.parent(0) = closest_parent

        While True

            'CREATE PROCEDURE `up_select_parent_ma_hierarchy`
            '(IN p_ma varchar(40))
            'BEGIN
            'SELECT parent, level
            'FROM account
            'WHERE ma = p_ma;
            'END

            Selectcmd = New MySqlCommand("up_select_parent_ma_hierarchy", Connection_mariadb_local_bc_multilevel)
            Selectcmd.CommandType = CommandType.StoredProcedure
            Selectcmd.Parameters.Add(New MySqlParameter("p_ma", ma))

            Adapter = New MySqlDataAdapter
            Selectcmd.CommandType = CommandType.StoredProcedure
            Adapter.SelectCommand = Selectcmd
            Dataset = New DataSet
            Adapter.Fill(Dataset)

            level = CInt(Dataset.Tables(0).Rows(0)("level"))

            If level > 0 Then

                parent = CStr(Dataset.Tables(0).Rows(0)("parent"))

                hierarchy_i_number += 1

                ReDim Preserve assign_parent_ma.data.parent(hierarchy_i_number)

                assign_parent_ma.data.parent(hierarchy_i_number) = parent

                ma = parent

            Else

                Exit While

            End If

        End While

        Connection_mariadb_local_bc_multilevel.Close()

        Array.Reverse(assign_parent_ma.data.parent)

    End Sub

End Class
