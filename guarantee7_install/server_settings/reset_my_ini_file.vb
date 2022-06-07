Imports System.IO

Public Class reset_my_ini_file

    Public Shared Sub exe()

        Dim server_directory As String = Regex.Replace(Directory.GetCurrentDirectory, "(guarantee7|test_guarantee)[^\\]*\\bin\\Debug\\net5.0-windows", "guarantee7_server\bin\Debug\net5.0-windows")

        Dim file_info = New FileInfo(server_directory & "\resource\server.ini")

        Dim fileContent = "port=" & vbCrLf & "path.my.ini=" & vbCrLf & "server_id=" & vbCrLf & "password_hash=" & vbCrLf & "private_key=="

        File.WriteAllText(file_info.FullName, fileContent)

    End Sub

End Class
