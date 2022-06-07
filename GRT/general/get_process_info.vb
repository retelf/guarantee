Public Class get_process_info

    Public Shared Function exe(command As String, argument As String) As String

        Dim stdOutput As StreamReader
        Dim stdError As StreamReader
        Dim content As String
        Dim exitStatus As String

        Using p As Process = New Process()

            Dim ps As ProcessStartInfo = New ProcessStartInfo()

            ps.Arguments = argument
            ps.FileName = command
            ps.UseShellExecute = False
            ps.WindowStyle = ProcessWindowStyle.Hidden
            ps.RedirectStandardInput = True
            ps.RedirectStandardOutput = True
            ps.RedirectStandardError = True

            p.StartInfo = ps

            p.Start()

            stdOutput = p.StandardOutput
            stdError = p.StandardError
            content = stdOutput.ReadToEnd() + stdError.ReadToEnd()
            exitStatus = p.ExitCode.ToString()

        End Using

    End Function

End Class
