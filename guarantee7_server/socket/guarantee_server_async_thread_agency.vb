Imports System
Imports System.Net
Imports System.Net.Sockets
Imports System.ServiceProcess
Imports System.Text
Imports System.Threading

Public Class guarantee_server_async_thread_agency

    Shared allDone As ManualResetEvent = New ManualResetEvent(False)

    Public Shared Async Sub exe()

        Dim bytes As Byte() = New Byte(1023) {}
        Dim ipAddress As IPAddress

        Dim ipHostInfo As IPHostEntry = Dns.GetHostEntry(Dns.GetHostName())

        For i = 0 To ipHostInfo.AddressList.Length - 1

            If Regex.Match(ipHostInfo.AddressList(i).ToString, "^\d+\.\d+\.\d+\.\d+$").Success Then

                ipAddress = ipHostInfo.AddressList(i)

                Exit For

            End If

        Next

        Dim localEndPoint As IPEndPoint = New IPEndPoint(ipAddress, GRT.GR.port_number_server_local_agency)
        Dim listener As Socket = New Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp)

        Try
            listener.Bind(localEndPoint)
            listener.Listen(100)

            While True

                allDone.Reset()

                Callback_server.SetControl("txt_monitor", "write", vbCrLf & "Waiting for a connection from client ...", "")

                listener.BeginAccept(New AsyncCallback(AddressOf AcceptCallback), listener)

                allDone.WaitOne()

            End While

        Catch e As Exception
            Callback_server.SetControl("txt_monitor", "write", e.ToString(), "")
        End Try

    End Sub

    Shared Sub AcceptCallback(ByVal ar As IAsyncResult)

        allDone.[Set]()

        Dim listener As Socket = CType(ar.AsyncState, Socket)

        Dim handler As Socket = listener.EndAccept(ar)

        Dim state As StateObject = New StateObject()

        state.workSocket = handler

        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, New AsyncCallback(AddressOf ReadCallback), state)

    End Sub

    Shared Sub ReadCallback(ByVal ar As IAsyncResult)

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

                    Call ReadCallback_sub_exe(others_case, handler, content)

                Else
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, New AsyncCallback(AddressOf ReadCallback), state)
                End If

            Else

                others_case = True
                Call ReadCallback_sub_exe(others_case, handler, content)

            End If

        End If

    End Sub

    Shared Async Sub ReadCallback_sub_exe(others_case As Boolean, handler As Socket, json_str_message_received As String)

        Dim JSS, JRS As String
        Dim message_bytes_to_return As Byte()
        Dim json_JSS, message_to_return_json As Newtonsoft.Json.Linq.JObject

        If Not others_case Then

            Callback_server.SetControl("txt_monitor", "write", "received : """ & json_str_message_received, """ command")

            json_str_message_received = GRT.unicode_to_utf8.exe(json_str_message_received)

            JSS = Regex.Replace(json_str_message_received, "^<#BOF%>|<#EOF%>.*", "")

            json_JSS = CType(JsonConvert.DeserializeObject(JSS), Newtonsoft.Json.Linq.JObject)

            json_JSS("value")("remote_endpoint") = handler.RemoteEndPoint.ToString

            JSS = CType(JsonConvert.SerializeObject(json_JSS), String)

            JRS = Await execute_received_command.exe(JSS)

            message_bytes_to_return = Encoding.UTF8.GetBytes(JRS)

            message_to_return_json = CType(JsonConvert.DeserializeObject(JRS), Newtonsoft.Json.Linq.JObject)

            Call Send(handler, JRS)

            Call SetControl.exe(message_to_return_json)

        Else
            Callback_server.SetControl("txt_monitor", "write", "received : """ & json_str_message_received, """ command")
        End If

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
