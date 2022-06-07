Public Class install_webview2_runtime

    Public Shared Sub exe()

        Dim readValue = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\EdgeUpdate\Clients\{F3017226-FE2A-4295-8BDF-00C3A9A7E4C5}", "pv", Nothing)

        If readValue Is Nothing Then

            Dim path As String = Environment.CurrentDirectory & "\MicrosoftEdgeWebview2Setup.exe"

            Dim startInfo As ProcessStartInfo = New ProcessStartInfo(path)

            startInfo.Verb = "runas"

            System.Diagnostics.Process.Start(startInfo)

        End If

    End Sub

End Class
