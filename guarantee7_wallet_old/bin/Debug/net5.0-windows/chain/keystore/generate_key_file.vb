Imports System.IO
Imports Nethereum.KeyStore
Imports Nethereum.KeyStore.Model

Public Class generate_key_file

    Public Shared Function exe(coin_name As String, password As String) As String()

        Dim keyStoreService = New Nethereum.KeyStore.KeyStoreScryptService()

        Dim folder_name As String

        If coin_name = "guarantee" Then
            folder_name = "guarantee"
        Else
            folder_name = "ethereum"
        End If

        Dim scryptParams = New ScryptParams With {
            .Dklen = 32,
            .N = 262144,
            .R = 1,
            .P = 8
        }

        Dim ecKey = Nethereum.Signer.EthECKey.GenerateKey()

        Dim public_key = ecKey.GetPublicAddress()
        Dim private_key = ecKey.GetPrivateKey

        Dim keyStore = keyStoreService.EncryptAndGenerateKeyStore(password, ecKey.GetPrivateKeyAsBytes(), ecKey.GetPublicAddress(), scryptParams)
        Dim json = keyStoreService.SerializeKeyStoreToJson(keyStore)

        Dim service = New KeyStoreService()
        Dim fileName = service.GenerateUTCFileName(public_key)

        Dim current_directory As String = Directory.GetCurrentDirectory

        Dim directory_info As New DirectoryInfo(current_directory & "\chain\keystore\" & folder_name)

        If Not directory_info.Exists Then
            Directory.CreateDirectory(Directory.GetCurrentDirectory & "\chain\keystore\" & folder_name)
        End If

        Using newfile = File.CreateText(Path.Combine(current_directory & "\chain\keystore\" & folder_name, fileName))

            newfile.Write(json)

            newfile.Flush()

        End Using

        Return {public_key, private_key}

    End Function

End Class
