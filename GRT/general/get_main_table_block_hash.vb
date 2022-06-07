Imports System.Security.Cryptography
Imports System.Text

Public Class get_main_table_block_hash

    Public Shared Function exe(query As String, previous_hash As String, current_hash As String) As Boolean

        Dim signature As String = Security.Gsign.sign(query, previous_hash)

        If current_hash = Regex.Match(signature, "0x.{64}").ToString Then
            Return True
        Else
            Return False
        End If

    End Function

End Class
