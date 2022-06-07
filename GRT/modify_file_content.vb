Imports System.IO

Public Class modify_file_content

    Public Shared Sub exe(file_info As FileInfo, replace_from() As String, replace_to() As String)

        Dim fileContent = String.Empty
        Dim fileStream As Stream

        fileStream = CType(file_info.OpenRead, Stream)

        Using StreamReader = New StreamReader(fileStream)

            fileContent = StreamReader.ReadToEnd()

        End Using

        fileContent = Regex.Replace(fileContent, "\S*skip-networking[^\n]*", "")

        For i = 0 To replace_from.Length - 1

            If Regex.Match(replace_to(i), "skip-networking").Success Then

                If Regex.Match(fileContent, "skip-networking").Success Then

                    fileContent = Regex.Replace(fileContent, "\S*skip-networking[^\n]*", replace_to(i))

                Else

                    fileContent = Regex.Replace(fileContent, replace_from(i), replace_to(i))

                End If

            Else

                fileContent = Regex.Replace(fileContent, replace_from(i), replace_to(i))

            End If

        Next

        File.WriteAllText(file_info.FullName, fileContent)

    End Sub

End Class
