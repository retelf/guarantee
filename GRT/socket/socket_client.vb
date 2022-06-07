Imports System.Text
Public Class socket_client

    Public Shared Async Function exe(remote_server_address As String, port_number_server_remote As Integer, port_number_server_local As Integer, JSS As String) As Task(Of String)

        Using gnet_client As Socket = GRT.Net.Gnet.net_sender(remote_server_address, port_number_server_remote, "localhost", port_number_server_local)

            Dim bytes As Byte() = New Byte(40000) {}

            Dim byte_message_to_send As Byte() = Encoding.Unicode.GetBytes("<#BOF%>" & JSS & "<#EOF%>")

            Await Task.Run(Sub() gnet_client.Send(byte_message_to_send))

            Dim bytesRec As Integer = Await Task.Run(Function() gnet_client.Receive(bytes))

            Dim JRS = CStr(Encoding.Unicode.GetString(bytes, 0, bytesRec))

            Return JRS

        End Using

    End Function

    Public Shared Async Function send_file(remote_server_address As String, port_number_server_remote As Integer, port_number_server_local As Integer, file_source As String, preBuffer As Byte()) As Task(Of String)

        Using gnet_client As Socket = GRT.Net.Gnet.net_sender(remote_server_address, port_number_server_remote, "localhost", port_number_server_local)

            Try

                Await Task.Run(Sub() gnet_client.SendFile(file_source, preBuffer, Nothing, TransmitFileOptions.UseDefaultWorkerThread))

                Return GRT.make_json_string.exe({{"key", "send_file", "quot"}, {"success", "success", "quot"}}, {}, False)

            Catch ex As Exception

                Return GRT.make_json_string.exe({{"key", "send_file", "quot"}, {"success", "fail", "quot"}}, {{"reason", ex.Message, "quot"}}, False)

            End Try

        End Using

    End Function

End Class
