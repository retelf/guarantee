Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports Newtonsoft.Json

Public Class guarantee_server_thread

    Shared max_block_number_local As Long

    Public Shared Async Sub exe()

        Dim JRS, json_str_message_received, JSS As String
        Dim message_to_return_json As Newtonsoft.Json.Linq.JObject
        Dim bytes As Byte() = New Byte(262144) {} ' 약 A4 25페이지
        Dim message_bytes_to_return As Byte()
        Dim hacking_case As Boolean
        Dim ipAddress As IPAddress

        ' Establish the local endpoint for the socket.  
        ' Dns.GetHostName returns the name of the host running the application. 
        Dim ipHostInfo As IPHostEntry = Dns.GetHostEntry(Dns.GetHostName())

        For i = 0 To ipHostInfo.AddressList.Length - 1

            If Regex.Match(ipHostInfo.AddressList(i).ToString, "^\d+\.\d+\.\d+\.\d+$").Success Then

                ipAddress = ipHostInfo.AddressList(i)

                Exit For

            End If

        Next

        Dim localEndPoint As IPEndPoint = New IPEndPoint(ipAddress, GRT.GR.port_number_server_local)
        Dim listener As Socket = New Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp)

        Try

            listener.Bind(localEndPoint) ' Bind 는 포트번호를 배타적으로 독점한다. 
            listener.Listen(100) ' 아래의 fix 들은 반드시 이 다음에 실시해야 한다. 그래야 싱크로 도중 쌓아 둘 수 있다. 그렇지 않으면 싱크로 도중 메인에서 보낸 것이 날아가 버린다.

            '\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\ fix

            If Not GRT.GR.node_level = 0 Then

                Call upload_uncompleted_command.exe() ' 먼저 자체 미완료 부분을 올려준다.

                max_block_number_local = Await GRT.syncronize_main_p2p.exe("server") ' 전반적인 싱크로를 미리 한번 실시하는 것이 매우 효율적이다. 설치시의 싱크로는 이미 했으므로.

                ' 이제 싱크로 도중에 받은 것만이 남아 있게 된다.

            End If

            '\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\ fix

            While True

                Callback_server.SetControl("txt_monitor", "write", vbCrLf & "Waiting for a connection from server...", "")

                Dim handler As Socket = listener.Accept()

                Dim bytes_length As Integer

                While True

                    Try

                        bytes_length = handler.Receive(bytes)

                    Catch ex As Exception

                        json_str_message_received = GRT.make_json_string.exe({{"key", "error", "quot"}, {"success", "fail", "quot"}}, {{"reason", ex.Message, "quot"}}, True)

                        Exit While

                    End Try

                    json_str_message_received = Encoding.Unicode.GetString(bytes, 0, bytes_length)

                    If json_str_message_received.IndexOf("<#EOF%>") > -1 Then

                        hacking_case = False

                    Else

                        hacking_case = True

                    End If

                    Exit While

                End While

                If Not hacking_case Then

                    Callback_server.SetControl("txt_monitor", "write", "received : """ & json_str_message_received, """ command")

                    JSS = Regex.Replace(json_str_message_received, "^\s*<#BOF%>\s*|<#EOF%>.*", "")

                    JRS = Await execute_received_command.exe(JSS)

                    message_bytes_to_return = Encoding.Unicode.GetBytes(JRS)

                    Try

                        'Await Task.Run(Function() handler.Send(message_bytes_to_return))
                        handler.Send(message_bytes_to_return)

                        message_to_return_json = CType(JsonConvert.DeserializeObject(JRS), Linq.JObject)

                        Call SetControl.exe(message_to_return_json)

                    Catch ex As Exception

                        Callback_server.SetControl("txt_monitor", "write", "received : """ & ex.Message, "quot")

                    End Try

                Else

                    Callback_server.SetControl("txt_monitor", "write", "received : """ & json_str_message_received, """ command")

                End If

                handler.Shutdown(SocketShutdown.Both)

                handler.Close()

            End While

        Catch e As Exception

            Callback_server.SetControl("txt_monitor", "write", e.ToString(), "")

        End Try

    End Sub

End Class

