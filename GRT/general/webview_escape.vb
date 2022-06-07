Public Class webview_escape

    Public Shared Function exe(content As String) As String

        content = Regex.Replace(content, "'", "\'")
        content = Regex.Replace(content, vbCrLf, "<br />")

        Return content

    End Function

End Class
