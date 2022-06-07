Public Class get_treated_query_submit_buy
    Public Shared Function exe(block_number As Long, pure_query As String) As String

        Dim treated_query As String = pure_query

        treated_query &= "USE bc;" & "UPDATE account SET `block_number` = " & block_number & " WHERE block_number = 0;"

        Return treated_query

    End Function

End Class
