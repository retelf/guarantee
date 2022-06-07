Imports Nethereum.Signer

Public Class verify
    Public Shared Function exe(original_data As String, signature As String, publicKey As String) As Boolean

        Dim signer As EthereumMessageSigner = New EthereumMessageSigner()

        Dim addressRec = signer.EncodeUTF8AndEcRecover(original_data, signature)

        If publicKey = addressRec Then
            Return True
        Else
            Return False
        End If

    End Function

End Class