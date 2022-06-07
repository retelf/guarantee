Public Class check_process_running

    Public Shared Function exe(process_name As String) As Boolean

        ' 이것 보다는 프로세스 PID 를 확인하는 것이 정확하다.

        Dim count As Integer

        Dim Processes As Process() = Process.GetProcessesByName(process_name)
        'Dim Processes As Process() = Process.GetProcessesByName("guarantee7")

        If Processes.Length = 0 Then

            Return False

        Else

            For i = 0 To Processes.Length - 1

                count = Processes(i).Threads.Count

                If count > 0 Then

                    For j = 0 To count - 1

                        If Processes(i).Threads(j).ThreadState = ThreadState.Running Or Processes(i).Threads(j).ThreadState = ThreadState.Wait Then

                            GRT.GR.port_number_server_local = CInt(GRT.get_info_from_ini_file.server("port_default"))

                            Return True

                        End If

                    Next

                End If

            Next

            Return False

        End If

    End Function

End Class
