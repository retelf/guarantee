Imports Nethereum.Signer

Public Class restore_ethereum_public_key

    Public Shared Function exe(publicKey_ethereum As String, private_key_ethereum As String) As String

        Dim signature As String = GRT.Security.Gsign.sign("foo", private_key_ethereum)

        Dim signer As EthereumMessageSigner = New EthereumMessageSigner()

        Dim case_sensitive_publicKey = signer.EncodeUTF8AndEcRecover("foo", signature)

        Return case_sensitive_publicKey

    End Function

    Public Shared Function exe_from_signiture(password As String, password_signiture As String) As String

        Dim signer As EthereumMessageSigner = New EthereumMessageSigner()

        Dim case_sensitive_publicKey = signer.EncodeUTF8AndEcRecover(password, password_signiture)

        Return case_sensitive_publicKey

    End Function

End Class
