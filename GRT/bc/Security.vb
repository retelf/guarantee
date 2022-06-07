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

            ' 이더리움에서는 publicKey 가 언제나 소문자로만 온다.

            If original_data = "" Then
                original_data = "foo"
            End If

            Dim signer As EthereumMessageSigner = New EthereumMessageSigner()

            'Dim addressRec = signer.EncodeUTF8AndEcRecover(original_data, signature)

            Dim addressRec = signer.EncodeUTF8AndEcRecover(publicKey, signature)

            If String.Equals(publicKey, addressRec, StringComparison.OrdinalIgnoreCase) Then
                Return True
            Else
                Return False
            End If

        End Function

        'Public Shared Function verify(original_data As String, signature As String, publicKey As String) As Boolean

        '    ' 이더리움에서는 publicKey 가 언제나 소문자로만 온다.

        '    Dim addressRec, data As String

        '    If original_data = "" Then
        '        original_data = "foo"
        '    End If

        '    Dim signer As EthereumMessageSigner = New EthereumMessageSigner()

        '    data = signer.EncodeUTF8AndEcRecover(publicKey, signature)

        '    addressRec = signer.EncodeUTF8AndEcRecover(original_data, signature)

        '    If String.Equals(publicKey, addressRec, StringComparison.OrdinalIgnoreCase) Then
        '        Return True
        '    Else
        '        Return False
        '    End If

        '    data = signer.EncodeUTF8AndEcRecover(publicKey, signature)

        'End Function
        Public Shared Function public_key(publicKey As String, privateKey As String) As Boolean

            Dim signature As String = Gsign.sign("foo", privateKey)

            Return Gverify.verify("foo", signature, publicKey)

        End Function
        Public Shared Function get_public_key(privateKey As String) As String

            Dim signature As String = Gsign.sign("foo", privateKey)

            Dim signer As EthereumMessageSigner = New EthereumMessageSigner()

            Dim addressRec = signer.EncodeUTF8AndEcRecover("foo", signature)

            Return addressRec

        End Function

    End Class

End Class
