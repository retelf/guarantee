Public Class get_treated_query_submit_load_nft
    Public Shared Function exe(block_number As Long, pure_query As String, sell_order_right_now As Boolean) As String

        Dim treated_query As String = pure_query

        treated_query &= "USE bc_nft;" & "UPDATE account SET `block_number` = " & block_number & " WHERE block_number = 0;"

        treated_query &= "USE bc_nft;" & "UPDATE sub_account SET `block_number` = " & block_number & " WHERE block_number = 0;"

        treated_query &= "USE bc_nft;" & "UPDATE owner_portion SET `block_number` = " & block_number & " WHERE block_number = 0;"

        treated_query &= "USE bc_nft;" & "UPDATE terms SET `block_number` = " & block_number & " WHERE block_number = 0;"

        If sell_order_right_now Then

            treated_query &= "USE bc_nft;" & "UPDATE sell_order SET `block_number` = " & block_number & " WHERE block_number = 0;"

        End If

        treated_query &= "USE bc_nft;" & "UPDATE history SET `block_number` = " & block_number & " WHERE block_number = 0;"

        Return treated_query

    End Function

End Class
