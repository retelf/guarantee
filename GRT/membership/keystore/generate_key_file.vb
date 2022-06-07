Imports System.IO
Imports Nethereum.KeyStore
Imports Nethereum.KeyStore.Model

Public Class generate_key_file

    Public Shared Sub exe_new(password As String, ecKey As Nethereum.Signer.EthECKey)

        Dim keyStoreService = New Nethereum.KeyStore.KeyStoreScryptService()

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

        Dim directory_info As New DirectoryInfo(current_directory & "\chain\keystore")

        If Not directory_info.Exists Then
            Directory.CreateDirectory(current_directory & "\chain\keystore")
        End If

        Dim file_info As New FileInfo(Path.Combine(current_directory & "\chain\keystore", fileName))

        If Not file_info.Exists Then

            Using newfile = File.CreateText(Path.Combine(current_directory & "\chain\keystore", fileName))

                newfile.Write(json)

                newfile.Flush()

            End Using

        End If

    End Sub

    Public Shared Sub exe_already(password As String, private_key As String)

        Dim public_key = GRT.Security.Gverify.get_public_key(private_key)

        Dim net_private_key = Regex.Replace(private_key, "^0x", "")

        Dim PrivateKeyAsBytes = Enumerable.Range(0, net_private_key.Length).Where(Function(x) x Mod 2 = 0).[Select](Function(x) Convert.ToByte(net_private_key.Substring(x, 2), 16)).ToArray()

        Dim keyStoreService = New Nethereum.KeyStore.KeyStoreScryptService()

        Dim scryptParams = New ScryptParams With {
            .Dklen = 32,
            .N = 262144,
            .R = 1,
            .P = 8
        }

        'Dim ecKey = Nethereum.Signer.EthECKey.GenerateKey()

        Dim keyStore = keyStoreService.EncryptAndGenerateKeyStore(password, PrivateKeyAsBytes, public_key, scryptParams)
        Dim json = keyStoreService.SerializeKeyStoreToJson(keyStore)

        Dim service = New KeyStoreService()
        Dim fileName = service.GenerateUTCFileName(public_key)

        Dim current_directory As String = Regex.Replace(Directory.GetCurrentDirectory, "(guarantee7|test_guarantee)[^\\]*\\bin\\Debug\\net5.0-windows", "guarantee7_wallet\bin\Debug\net5.0-windows")

        Dim directory_info As New DirectoryInfo(current_directory & "\chain\keystore")

        If Not directory_info.Exists Then
            Directory.CreateDirectory(current_directory & "\chain\keystore")
        End If

        Dim file_info As New FileInfo(Path.Combine(current_directory & "\chain\keystore", fileName))

        If Not file_info.Exists Then

            Using newfile = File.CreateText(Path.Combine(current_directory & "\chain\keystore", fileName))

                newfile.Write(json)

                newfile.Flush()

            End Using

        End If

    End Sub

End Class
