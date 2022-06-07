Imports System.ServiceProcess

Public Class get_info_from_ini_file

    Public Shared Function database() As Integer

        Dim current_directory As String = Regex.Replace(Directory.GetCurrentDirectory, "(guarantee7|test_guarantee)[^\\]*\\bin\\Debug\\net5.0-windows", "guarantee7_server\bin\Debug\net5.0-windows")

        Dim file_info = New FileInfo(current_directory & "\resource\server.ini")

        Dim fileContent = String.Empty
        Dim fileStream As Stream

        fileStream = CType(file_info.OpenRead, Stream)

        Using StreamReader = New StreamReader(fileStream)

            fileContent = StreamReader.ReadToEnd()

        End Using

        ' 일단 my.ini 파일의 위치부터 찾아야 한다.

        Dim myini_location As String = Regex.Match(fileContent, "(?<=path\.my\.ini=)[^\n]+").ToString.Trim

        If Not myini_location = "" Then

            file_info = New FileInfo(myini_location)

            If file_info.Exists Then

                fileStream = CType(file_info.OpenRead, Stream)

                Using StreamReader = New StreamReader(fileStream)

                    fileContent = StreamReader.ReadToEnd()

                End Using

                Return CInt(Regex.Match(fileContent, "(?<=port=)\d+").ToString)

            Else ' mariadb 설치 중이다. 그냥 넘어가면 아무 문제 없다.

                Return 0

            End If

        Else

            Return 0

        End If

    End Function

    Public Shared Function server(request_str As String) As String

        Dim current_directory As String = Regex.Replace(Directory.GetCurrentDirectory, "(guarantee7|test_guarantee)[^\\]*\\bin\\Debug\\net5.0-windows", "guarantee7_server\bin\Debug\net5.0-windows")

        Dim file_info = New FileInfo(current_directory & "\resource\server.ini")

        Dim fileContent = String.Empty
        Dim fileStream As Stream

        fileStream = CType(file_info.OpenRead, Stream)

        Using StreamReader = New StreamReader(fileStream)

            fileContent = StreamReader.ReadToEnd()

        End Using

        If Not Regex.Match(fileContent, "(?<=" & request_str & "=)[\d\.]+").ToString = "" Then

            Return Regex.Match(fileContent, "(?<=" & request_str & "=)[\d\.]+").ToString

        Else

            Return "0" ' 설치 중이다. 그냥 넘어가면 아무 문제 없다

        End If

    End Function

End Class
