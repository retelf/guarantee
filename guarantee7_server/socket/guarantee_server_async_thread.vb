Imports System.Net
Imports System.Net.Sockets
Imports System.ServiceProcess
Imports System.Text
Imports System.Threading

Public Class guarantee_server_async_thread

    Shared allDone As ManualResetEvent = New ManualResetEvent(False)

    Shared max_block_number_local As Long

    Public Shared Async Sub exe()

        ' 가장 먼저 싱크로 여부를 확인한다.
        ' *** 메인의 경우 - 메인에서는 할 일이 없는 것으로 보인다.
        ' 1. 바뀐 메인으로부터 이미 진행된 싱크로를 실시한다.
        ' 2. agent_record 에서 미완료 거래(DB입력은 되었으나 실시되지 않은 것 - 그런데 이런 것이 있을 수 있나? autocommit 도 아닌데)를 완료시킨다. 이는 자체에서 확인하는 것으로 족하다. sub 를 확인할 필요는 없다.
        '    락은 서브에서 풀도록 한다. 따라서 이 때까지는 계속 락이 걸려있게 된다.
        ' *** sub 의 경우
        ' 1. 기본적인 싱크로를 실시한다.
        ' 2. 이미 자체적으로 완료를 시킨 부분으로서
        '   agent_record 에서
        '   이미 무엇인가 실시는 되었으나
        '   메인으로 올리지 못한 부분 또는
        '   메인이 받지 못한 부분에 관하여
        '   (이것들은 메인으로부터 확인해야 한다.)
        '   메인에 송신한다. 그리고 락을 풀어준다.
        ' 이상은 listener.Listen 다음에서 처리해야 할 듯

        Dim service As ServiceController = New ServiceController("MySQL")

        While True
            service = New ServiceController("MySQL")
            If service.Status.Equals(ServiceControllerStatus.Stopped) Then
                service.Start()
                Exit While
            ElseIf service.Status.Equals(ServiceControllerStatus.Running) Then
                Exit While
            Else
                System.Threading.Thread.Sleep(100)
            End If
        End While

        Dim bytes As Byte() = New Byte(1023) {}
        Dim ipAddress As IPAddress

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
            listener.Bind(localEndPoint)
            listener.Listen(100) ' 아래의 fix 들은 반드시 이 다음에 실시해야 한다. 그래야 싱크로 도중 쌓아 둘 수 있다. 그렇지 않으면 싱크로 도중 메인에서 보낸 것이 날아가 버린다.

            '\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\ fix

            If Not GRT.GR.node_level = 0 Then

                Call upload_uncompleted_command.exe() ' 먼저 자체 미완료 부분을 올려준다.

                max_block_number_local = Await GRT.syncronize_main_p2p.exe("server") ' 전반적인 싱크로를 미리 한번 실시하는 것이 매우 효율적이다. 설치시의 싱크로는 이미 했으므로.

                ' 이제 싱크로 도중에 받은 것만이 남아 있게 된다.

            End If

            '\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\ fix

            While True

                allDone.Reset()

                Callback_server.SetControl("txt_monitor", "write", vbCrLf & "Waiting for a connection...", "")

                listener.BeginAccept(New AsyncCallback(AddressOf AcceptCallback), listener)

                allDone.WaitOne()

            End While

        Catch e As Exception
            Callback_server.SetControl("txt_monitor", "write", e.ToString(), "")
        End Try

    End Sub

    Public Shared Sub AcceptCallback(ByVal ar As IAsyncResult)

        allDone.[Set]()

        Dim listener As Socket = CType(ar.AsyncState, Socket)

        Dim handler As Socket = listener.EndAccept(ar)

        Dim state As StateObject = New StateObject()

        state.workSocket = handler

        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, New AsyncCallback(AddressOf ReadCallback), state)

    End Sub

    Public Shared Sub ReadCallback(ByVal ar As IAsyncResult)

        Dim others_case As Boolean

        Dim content As String = String.Empty
        Dim state As StateObject = CType(ar.AsyncState, StateObject)
        Dim handler As Socket = state.workSocket
        Dim bytesRead As Integer = handler.EndReceive(ar)

        If bytesRead > 0 Then

            state.sb.Append(Encoding.Unicode.GetString(state.buffer, 0, bytesRead))

            content = state.sb.ToString()

            If Regex.Match(content, "^<#BOF%>").Success Then

                others_case = False

                If content.IndexOf("<#EOF%>") > -1 Then

                    Call check_syncronize(others_case, handler, content)

                Else
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, New AsyncCallback(AddressOf ReadCallback), state)
                End If

            Else

                others_case = True
                Call check_syncronize(others_case, handler, content)

            End If

        End If

    End Sub

    Public Shared Async Sub check_syncronize(others_case As Boolean, handler As Socket, json_str_message_received As String)

        Dim received_block_number As Long
        Dim JSS As String
        Dim json_JSS As Newtonsoft.Json.Linq.JObject

        If Not others_case Then

            Callback_server.SetControl("txt_monitor", "write", "received : """ & json_str_message_received, """ command")

            json_str_message_received = GRT.unicode_to_utf8.exe(json_str_message_received)

            JSS = Regex.Replace(json_str_message_received, "^<#BOF%>|<#EOF%>.*", "")

            If GRT.GR.node_level = 0 Or Not GR.syncro_state Then

                Await Task.Run(Sub() exe_normal_process(handler, JSS))
                'Call exe_normal_process(handler, JSS)

            Else

                ' 싱크로 도중에 받은 것 실시.
                ' 부모로부터 릴레이 받은 것은 블록넘버 확인 후
                ' 부모로부터 받은 것이 아닌 경우에는 그대로 실시
                ' 위 두가지는 뒤죽박죽으로 들어와 있게 된다.

                ' 부모로부터 릴레이 받은 경우와 클라이언트로부터 받은 경우 구분한다.
                ' 릴레이를 받은 경우는 반드시 블록넘버가 있다.
                ' 클라이언트로부터 받은 경우는 블록넘버가 없다.

                json_JSS = CType(JsonConvert.DeserializeObject(JSS), Newtonsoft.Json.Linq.JObject)

                If Not json_JSS("value")("block_number") Is Nothing AndAlso CInt(json_JSS("value")("block_number")) > 0 Then ' 부모로부터 릴레이 받은 경우

                    received_block_number = CLng(json_JSS("value")("block_number"))

                    If max_block_number_local = received_block_number - 1 Then ' 싱크로 완료를 선언하고 정상실시한다.

                        GR.syncro_state = False

                        Await Task.Run(Sub() exe_normal_process(handler, JSS))

                    ElseIf max_block_number_local < received_block_number - 1 Then ' 중간부분을 실시한다. 한번이면 된다.

                        Await Task.Run(Sub() GRT.syncronize_main_p2p_for_middle_gap.exe(max_block_number_local + 1, received_block_number - 1))

                        GR.syncro_state = False

                        Await Task.Run(Sub() exe_normal_process(handler, JSS))

                    Else

                        '그냥 다음 핸들러로 넘어가 주어야 한다.
                        '따라서 아무것도 하지 않으면 그만이다.
                        '결국 위 If max_block_number_local = received_block_number - 1 Then 로 가게 된다.

                    End If

                Else ' 클라이언트로부터 받은 경우

                    Await Task.Run(Sub() exe_normal_process(handler, JSS))
                    'Call exe_normal_process(handler, JSS)

                End If

            End If

        Else
            Callback_server.SetControl("txt_monitor", "write", "received : """ & json_str_message_received, """ command")
        End If

    End Sub

    Private Shared Async Sub exe_normal_process(handler As Socket, JSS_received As String)

        Dim json_JSS = CType(JsonConvert.DeserializeObject(JSS_received), Newtonsoft.Json.Linq.JObject)

        json_JSS("value")("remote_endpoint") = handler.RemoteEndPoint.ToString

        Dim JSS = CType(JsonConvert.SerializeObject(json_JSS), String)

        Dim message_bytes_to_return As Byte()
        Dim message_to_return_json As Newtonsoft.Json.Linq.JObject

        Dim JRS = Await execute_received_command.exe(JSS)

        message_bytes_to_return = Encoding.UTF8.GetBytes(JRS)

        message_to_return_json = CType(JsonConvert.DeserializeObject(JRS), Newtonsoft.Json.Linq.JObject)

        Call Send(handler, JRS)

        Call SetControl.exe(message_to_return_json)

    End Sub

    Private Shared Sub Send(ByVal handler As Socket, ByVal data As String)

        Dim byteData As Byte() = Encoding.Unicode.GetBytes(data)

        handler.BeginSend(byteData, 0, byteData.Length, 0, New AsyncCallback(AddressOf SendCallback), handler)

    End Sub

    Private Shared Sub SendCallback(ByVal ar As IAsyncResult)

        Try

            Dim handler As Socket = CType(ar.AsyncState, Socket)

            Dim bytesSent As Integer = handler.EndSend(ar)

            handler.Shutdown(SocketShutdown.Both)

            handler.Close()

        Catch e As Exception

            Callback_server.SetControl("txt_monitor", "write", e.ToString(), "")

        End Try

    End Sub

End Class
