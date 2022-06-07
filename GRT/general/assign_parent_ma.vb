Public Class assign_parent_ma
    Public Structure st_data

        Dim parent() As String
        Dim level As Integer
        Dim count_children As Integer
        Dim chois_type As String ' general, special_same_level, special_level_down

    End Structure : Public Shared data As st_data

    Public Shared Sub exe()

        ' 자식 카운트가 1~6 인 부모에 하나 자식 더 붙이는 경우
        ' 자식 카운트가 1~6 인 부모가 없어서 기존 레벨에서 0 인 부모를 하나 정하는 경우
        ' 자식 카운트가 1~6 인 부모가 없고 기존 레벨을 뛰어 넘는 경우

        Dim Connection_mariadb_local_bc_multilevel As New MySqlConnection(GRT.GR.cString_mariadb_local_bc_multilevel)

        Connection_mariadb_local_bc_multilevel.Open()

        Dim closest_parent As String

        Dim Selectcmd As MySqlCommand
        Dim Adapter As MySqlDataAdapter
        Dim Dataset As DataSet

        Dim count_result As Integer

        'CREATE PROCEDURE `up_select_parent_ma_general`
        '(IN p_multilevel_max_children_count int(11))
        'BEGIN
        'SELECT ma, level, count_children
        'FROM account
        'WHERE count_children > 0 AND count_children < p_multilevel_max_children_count
        'ORDER BY level DESC, count_children ASC
        'LIMIT 1;
        'END

        Selectcmd = New MySqlCommand("up_select_parent_ma_general", Connection_mariadb_local_bc_multilevel)
        Selectcmd.Parameters.Add(New MySqlParameter("p_multilevel_max_children_count", GRT.GR.multilevel.max_children_count))

        Adapter = New MySqlDataAdapter
        Selectcmd.CommandType = CommandType.StoredProcedure
        Adapter.SelectCommand = Selectcmd
        Dataset = New DataSet
        Adapter.Fill(Dataset)

        count_result = Dataset.Tables(0).Rows.Count

        If count_result = 0 Then

            'CREATE PROCEDURE `up_select_parent_ma_special_same_level`
            '()
            'BEGIN
            'SELECT ma, level, count_children
            'FROM account
            'WHERE count_children = 0
            'ORDER BY level ASC
            'LIMIT 1;
            'END

            Selectcmd = New MySqlCommand("up_select_parent_ma_special_same_level", Connection_mariadb_local_bc_multilevel)

            Adapter = New MySqlDataAdapter
            Selectcmd.CommandType = CommandType.StoredProcedure
            Adapter.SelectCommand = Selectcmd
            Dataset = New DataSet
            Adapter.Fill(Dataset)

            count_result = Dataset.Tables(0).Rows.Count

            If count_result = 0 Then ' 여기까지 오지 않는다. 일단은 남겨 놓는다.

                'CREATE PROCEDURE `up_select_parent_ma_special_level_down`
                '()
                'BEGIN
                'SELECT ma, level, count_children
                'FROM account
                'ORDER BY level DESC
                'LIMIT 1;
                'END

                Selectcmd = New MySqlCommand("up_select_parent_ma_special", Connection_mariadb_local_bc_multilevel)

                Adapter = New MySqlDataAdapter
                Selectcmd.CommandType = CommandType.StoredProcedure
                Adapter.SelectCommand = Selectcmd
                Dataset = New DataSet
                Adapter.Fill(Dataset)

                closest_parent = CStr(Dataset.Tables(0).Rows(0)("ma"))
                data.level = CInt(Dataset.Tables(0).Rows(0)("level"))
                data.count_children = CInt(Dataset.Tables(0).Rows(0)("count_children"))

            Else

                closest_parent = CStr(Dataset.Tables(0).Rows(0)("ma"))
                data.level = CInt(Dataset.Tables(0).Rows(0)("level"))
                data.count_children = CInt(Dataset.Tables(0).Rows(0)("count_children"))
                data.chois_type = "general"

            End If

        Else

            closest_parent = CStr(Dataset.Tables(0).Rows(0)("ma"))
            data.level = CInt(Dataset.Tables(0).Rows(0)("level"))
            data.count_children = CInt(Dataset.Tables(0).Rows(0)("count_children"))
            data.chois_type = "general"

        End If

        ' parent hierarchy 구하기

        Call get_parent_hierarchy.exe(closest_parent)

        Connection_mariadb_local_bc_multilevel.Close()

    End Sub

End Class
