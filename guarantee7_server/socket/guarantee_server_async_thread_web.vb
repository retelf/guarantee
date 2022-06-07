Imports System.Net
Imports System.Net.Sockets
Imports System.ServiceProcess
Imports System.Text
Imports System.Threading

Public Class guarantee_server_async_thread_web

    Shared allDone As ManualResetEvent = New ManualResetEvent(False)

    Shared max_block_number_local As Long

    Public Shared Async Sub exe()

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

        Dim localEndPoint As IPEndPoint = New IPEndPoint(ipAddress, GRT.GR.port_number_server_local_web)
        Dim listener As Socket = New Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp)

        Try
            listener.Bind(localEndPoint)
            listener.Listen(100) ' 아래의 fix 들은 반드시 이 다음에 실시해야 한다. 그래야 싱크로 도중 쌓아 둘 수 있다. 그렇지 않으면 싱크로 도중 메인에서 보낸 것이 날아가 버린다.

            While True

                allDone.Reset()

                Callback_server.SetControl("txt_monitor", "write", vbCrLf & "Waiting for a web connection...", "")

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

            state.sb.Append(Encoding.UTF8.GetString(state.buffer, 0, bytesRead))

            content = state.sb.ToString()

            If Regex.Match(content, "^<#BOF%>").Success Then

                others_case = False

                If content.IndexOf("<#EOF%>") > -1 Then

                    content = Regex.Replace(content, "^<#BOF%>|<#EOF%>.*", "")

                    Call exe_normal_process(handler, content)

                Else
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, New AsyncCallback(AddressOf ReadCallback), state)
                End If

            Else

                others_case = True

                Callback_server.SetControl("txt_monitor", "write", "received : """ & content, """ command")

            End If

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

        Dim byteData As Byte() = Encoding.UTF8.GetBytes(data)

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
