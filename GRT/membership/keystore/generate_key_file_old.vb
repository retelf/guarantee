Imports System.IO
Imports Nethereum.KeyStore
Imports Nethereum.KeyStore.Model

Public Class generate_key_file_old

    Public Shared Sub exe(coin_name As String, password As String, ecKey As Nethereum.Signer.EthECKey)

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

        'Dim ecKey = Nethereum.Signer.EthECKey.GenerateKey()

        Dim public_key = ecKey.GetPublicAddress()

        Dim keyStore = keyStoreService.EncryptAndGenerateKeyStore(password, ecKey.GetPrivateKeyAsBytes(), ecKey.GetPublicAddress(), scryptParams)
        Dim json = keyStoreService.SerializeKeyStoreToJson(keyStore)

        Dim service = New KeyStoreService()
        Dim fileName = service.GenerateUTCFileName(public_key)

        Dim current_directory As String = Regex.Replace(Directory.GetCurrentDirectory, "(guarantee7|test_guarantee)[^\\]*\\bin\\Debug\\net5.0-windows", "guarantee7_wallet\bin\Debug\net5.0-windows")

        Dim directory_info As New DirectoryInfo(current_directory & "\chain\keystore\" & folder_name)

        If Not directory_info.Exists Then
            Directory.CreateDirectory(current_directory & "\chain\keystore\" & folder_name)
        End If

        Using newfile = File.CreateText(Path.Combine(current_directory & "\chain\keystore\" & folder_name, fileName))

            newfile.Write(json)

            newfile.Flush()

        End Using

    End Sub

End Class
