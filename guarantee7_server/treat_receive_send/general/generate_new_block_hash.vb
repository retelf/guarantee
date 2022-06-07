Public Class generate_new_block_hash

    Public Shared Function exe(pure_query As String, previous_hash As String) As String

        Dim query_signiture_by_previous_hash, new_block_hash As String

        query_signiture_by_previous_hash = GRT.Security.Gsign.sign(pure_query, previous_hash)

        new_block_hash = Regex.Match(query_signiture_by_previous_hash, "0x.{64}").ToString

        new_block_hash = Regex.Replace(new_block_hash, "^0x", "")

        Return new_block_hash

    End Function

End Class
