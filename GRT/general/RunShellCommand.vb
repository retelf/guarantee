Public Class RunShellCommand

    Public Shared Function exe(ByVal command As String, ByVal parms As String, ByRef stdout As String, ByRef stderr As String, ByVal Optional waitForCompletion As Boolean = True) As Integer

        Dim startInfo As ProcessStartInfo = New ProcessStartInfo(command)

        startInfo.RedirectStandardInput = True
        startInfo.RedirectStandardOutput = True
        startInfo.RedirectStandardError = True
        startInfo.UseShellExecute = False
        startInfo.WindowStyle = ProcessWindowStyle.Hidden
        startInfo.CreateNoWindow = True

        Dim proc As Process = Process.Start(startInfo)

        Dim sw As System.IO.StreamWriter = proc.StandardInput
        Dim sr As System.IO.StreamReader = proc.StandardOutput
        Dim se As System.IO.StreamReader = proc.StandardError

        sw.WriteLine(parms)
        sw.Close()

        stdout = sr.ReadToEnd()
        stderr = se.ReadToEnd()

        If waitForCompletion Then proc.WaitForExit()

        Return proc.ExitCode

    End Function

End Class
