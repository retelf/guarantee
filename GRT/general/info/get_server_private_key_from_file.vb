Imports System.ServiceProcess

Public Class get_server_private_key_from_file

    Public Shared Function exe() As String

        Dim current_directory As String = Regex.Replace(Directory.GetCurrentDirectory, "(guarantee7|test_guarantee)[^\\]*\\bin\\Debug\\net5.0-windows", "guarantee7_server\bin\Debug\net5.0-windows")

        Dim file_info = New FileInfo(current_directory & "\resource\server.ini")

        Dim fileContent = String.Empty
        Dim fileStream As Stream

        fileStream = CType(file_info.OpenRead, Stream)

        Using StreamReader = New StreamReader(fileStream)

            fileContent = StreamReader.ReadToEnd()

        End Using

        If Not Regex.Match(fileContent, "(?<=private_key=)\d+").ToString = "" Then

            Return CStr(Regex.Match(fileContent, "(?<=private_key=)\d+").ToString)

        Else

            Return "" ' 설치 중이다. 그냥 넘어가면 아무 문제 없다

        End If

    End Function

End Class
