Imports System.IO
Imports System.Net
Imports System.Net.Sockets

Public Class guarantee_receive_file_thread_test

    Public Shared temporary_folder_file_name() As String

    Public Shared Sub exe()

        Dim destination_folder, file_name, extension As String

        Dim listener = New TcpListener(IPAddress.Loopback, 11000)

        listener.Start()

        While True

            Using client = listener.AcceptTcpClient()

                If Not temporary_folder_file_name Is Nothing Then

                    destination_folder = temporary_folder_file_name(0)
                    file_name = temporary_folder_file_name(1)
                    extension = temporary_folder_file_name(2)

                    Using stream = client.GetStream()

                        Using output = File.Create(destination_folder & "\" & file_name & "." & extension)

                            'Console.WriteLine("Client connected. Starting to receive the file")

                            Dim buffer = New Byte(1023) {}

                            Dim bytesRead As Integer

                            While True

                                bytesRead = stream.Read(buffer, 0, buffer.Length)

                                If bytesRead > 0 Then

                                    output.Write(buffer, 0, bytesRead)

                                Else

                                    Exit While

                                End If

                            End While

                        End Using

                    End Using

                    temporary_folder_file_name = Nothing

                Else
                    ' 그냥 넘어가면 된다. 흘려 보내기.
                End If

            End Using

        End While

    End Sub

End Class
