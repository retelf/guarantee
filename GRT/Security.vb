Imports System.Net.Sockets
Imports MySqlConnector
Imports System.Text
Imports System.ComponentModel
Imports Nethereum.Signer

Public Class Security

    Public Class Gsign
        Public Shared Function sign(data As String, private_key As String) As String

            Dim signer As EthereumMessageSigner = New EthereumMessageSigner()

            Dim signature As String = signer.EncodeUTF8AndSign(data, New EthECKey(private_key))

            Return signature

        End Function

    End Class

    Public Class Gverify
        Public Shared Function verify(original_data As String, signature As String, publicKey As String) As Boolean

            Dim signer As EthereumMessageSigner = New EthereumMessageSigner()

            Dim addressRec = signer.EncodeUTF8AndEcRecover(original_data, signature)

            If publicKey = addressRec Then
                Return True
            Else
                Return False
            End If

        End Function

    End Class

End Class
