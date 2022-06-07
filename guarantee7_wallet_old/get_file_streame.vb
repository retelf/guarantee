Imports System.IO

Public Class get_file_streame

    Public Shared Function exe(source As String) As String

        Dim file_info As New FileInfo(source)

        Dim stream_reader = New StreamReader(file_info.FullName, System.Text.Encoding.Default, True)

        Return stream_reader.ReadToEnd()

    End Function

End Class
