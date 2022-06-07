Public Class get_treated_query_register_node
    Public Shared Function exe(block_number As Long, pure_query As String) As String

        Dim treated_query As String
        Dim parent As String
        Dim level, count_children As Integer

        Call assign_parent_node.exe()

        parent = assign_parent_node.data.id
        level = assign_parent_node.data.level
        count_children = assign_parent_node.data.count_children

        treated_query = pure_query

        treated_query &= "USE bc;" & "UPDATE account SET `block_number` = " & block_number & " WHERE block_number = 0;"

        treated_query &= "USE bc_manager;" & "UPDATE node SET `parent` = '" & parent & "', `level` = " & level + 1 & " WHERE block_number = 0;"

        ' 해당 parent 의 자식 숫자를 하나 올려준다.

        treated_query &= "USE bc_manager;" & "UPDATE node SET `count_children` = " & count_children + 1 & " WHERE na = '" & parent & "';"

        treated_query &= "USE bc_manager;" & "UPDATE node SET `block_number` = " & block_number & " WHERE block_number = 0;"

        Return treated_query

    End Function

End Class
