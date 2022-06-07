Imports Nethereum.Signer

Public Class get_local_server_id

    Public Shared Function exe() As String

        Dim server_id As String

        Dim server_directory As String = Regex.Replace(Directory.GetCurrentDirectory, "(guarantee7|test_guarantee)[^\\]*\\bin\\Debug\\net5.0-windows", "guarantee7_server\bin\Debug\net5.0-windows")

        Dim file_info = New FileInfo(server_directory & "\resource\server.ini")

        Dim fileContent = String.Empty
        Dim fileStream As Stream

        fileStream = CType(file_info.OpenRead, Stream)

        Using StreamReader = New StreamReader(fileStream)

            fileContent = StreamReader.ReadToEnd()

        End Using

        server_id = Regex.Match(fileContent, "(?<=server_id=)[^\n]+").ToString.Trim

        Return server_id

    End Function

End Class
