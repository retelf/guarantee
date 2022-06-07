Imports Nethereum.Signer

Public Class get_password

    Public Shared Function exe() As String

        Dim password, encrypted As String

        Dim signer As EthereumMessageSigner = New EthereumMessageSigner()

        Dim server_directory As String = Regex.Replace(Directory.GetCurrentDirectory, "guarantee7[^\\]*\\bin\\Debug\\net5.0-windows", "guarantee7_server\bin\Debug\net5.0-windows")

        Dim file_info = New FileInfo(server_directory & "\resource\server.ini")

        Dim fileContent = String.Empty
        Dim fileStream As Stream

        fileStream = CType(file_info.OpenRead, Stream)

        Using StreamReader = New StreamReader(fileStream)

            fileContent = StreamReader.ReadToEnd()

        End Using

        encrypted = Regex.Match(fileContent, "(?<=password_hash=)[^\n]+").ToString

        password = signer.EncodeUTF8AndEcRecover("password", encrypted)

        Return password

    End Function

End Class
