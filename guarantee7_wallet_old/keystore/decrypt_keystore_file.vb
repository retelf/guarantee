Imports System.IO
Imports Nethereum.KeyStore
Imports Nethereum.KeyStore.Model

Public Class decrypt_keystore_file

    Public Shared Function exe(coin_name As String, password As String, public_key As String) As String

        Try

            Dim current_directory As String = Regex.Replace(Directory.GetCurrentDirectory, "guarantee7[^\\]*\\bin\\Debug\\net5.0-windows", "guarantee7_wallet\bin\Debug\net5.0-windows")

            Dim directory_info As New DirectoryInfo(current_directory & "\chain\keystore\" & coin_name)

            Dim stream_reader As StreamReader

            Dim json As String

            Dim found As Boolean = False

            For Each file_info In directory_info.GetFiles

                If Regex.Match(file_info.FullName, Regex.Replace(public_key, "^0x", "") & "$").Success Then

                    stream_reader = New StreamReader(file_info.FullName, System.Text.Encoding.Default, True)

                    json = stream_reader.ReadToEnd()

                    found = True

                    Exit For

                End If

            Next

            If found Then

                Dim keyStoreService = New Nethereum.KeyStore.KeyStoreScryptService()

                Dim key As Byte() = keyStoreService.DecryptKeyStoreFromJson(password, json)

                Dim hexa_key As String = "0x" & Convert.ToHexString(key)

                Return hexa_key

            Else

                Return "키파일이 발견되지 않습니다."

            End If

        Catch ex As Exception

            Return ex.Message

        End Try

    End Function

End Class
