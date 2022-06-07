Public Class get_treated_query_submit_buy_old
    Public Shared Function exe(block_number As Long, sell_order_block_number As Long, pure_query As String) As String

        Dim treated_query As String
        Dim parent As String
        Dim level, count_children As Integer

        Call assign_parent_ma.exe()

        parent = assign_parent_ma.data.parent(assign_parent_ma.data.parent.Length - 1)
        level = assign_parent_ma.data.level
        count_children = assign_parent_ma.data.count_children

        treated_query = pure_query

        treated_query &= "USE bc;" & "UPDATE account SET `block_number` = " & block_number & " WHERE block_number = 0;"

        treated_query &= "USE bc_multilevel;" & "UPDATE account SET `parent` = '" & parent & "', `level` = " & level + 1 & " WHERE block_number = " & sell_order_block_number & ";"

        ' 해당 parent 의 자식 숫자를 하나 올려준다.

        treated_query &= "USE bc_multilevel;" & "UPDATE account SET `count_children` = " & count_children + 1 & " WHERE ma = '" & parent & "';"

        Return treated_query

    End Function

End Class
