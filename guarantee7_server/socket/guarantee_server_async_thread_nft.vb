Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.Threading

Public Class FILE_StateObject
    Public Const BufferSize As Integer = 1024
    Public buffer As Byte() = New Byte(BufferSize - 1) {}
    Public workSocket As Socket = Nothing
End Class

Public Class guarantee_server_async_thread_nft

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

        Dim localEndPoint As IPEndPoint = New IPEndPoint(ipAddress, GRT.GR.port_number_server_local_nft)
        Dim listener As Socket = New Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp)

        Try
            listener.Bind(localEndPoint)
            listener.Listen(100)

            While True

                allDone.Reset()

                Callback_server.SetControl("txt_monitor", "write", vbCrLf & "Waiting for a file upload connection...", "")

                listener.BeginAccept(New AsyncCallback(AddressOf AcceptCallback), listener)

                allDone.WaitOne()

            End While

        Catch e As Exception
            Callback_server.SetControl("txt_monitor", "write", e.ToString(), "")
        End Try

    End Sub

    Public Shared Sub AcceptCallback(ByVal ar As IAsyncResult)

        allDone.Set()

        Dim listener As Socket = CType(ar.AsyncState, Socket)

        Dim handler As Socket = listener.EndAccept(ar)

        Dim state As FILE_StateObject = New FILE_StateObject()

        state.workSocket = handler

        initial_ReadCallback = True

        handler.BeginReceive(state.buffer, 0, FILE_StateObject.BufferSize, 0, New AsyncCallback(AddressOf ReadCallback), state)

    End Sub

    Shared initial_ReadCallback As Boolean

    Public Shared Sub ReadCallback(ByVal ar As IAsyncResult)

        Dim state As FILE_StateObject = CType(ar.AsyncState, FILE_StateObject)
        Dim handler As Socket = state.workSocket

        Dim JRS As String

        Static total_data_string, original_data, data(), nfa, nft_name, nft_sub_name, extension, eoa, signiture As String
        Static token_id As Integer
        Static file_length As Long

        Dim start_index As Integer
        Dim length As Long
        Static verified, eoa_exist As Boolean
        Static directory_checked As Boolean

        Dim bytesRead As Integer = handler.EndReceive(ar)

        If bytesRead > 0 Then

            Using memoryStream = New MemoryStream(state.buffer)

                If initial_ReadCallback Then

                    directory_checked = False

                    Using memorystream_reader = New BinaryReader(memoryStream)

                        total_data_string = memorystream_reader.ReadString()

                        If Regex.Match(total_data_string, "^0x[abcdef\d]{40}", RegexOptions.IgnoreCase).Success Then

                            data = Regex.Split(total_data_string, "\|")

                            nfa = data(0)
                            token_id = CInt(data(1))
                            nft_name = data(2)
                            nft_sub_name = data(3)
                            extension = data(4)
                            file_length = CLng(data(5))
                            eoa = data(6)
                            signiture = data(7)

                            eoa_exist = GRT.check_eoa_exist.exe(eoa)

                            If eoa_exist Then

                                original_data = Regex.Replace(total_data_string, "\|[^\|]+\|[^\|]+$", "")

                                verified = GRT.Security.Gverify.verify(original_data, signiture, eoa)

                                If verified Then

                                    length = memoryStream.Length - memoryStream.Position

                                    start_index = CInt(state.buffer.Length - length)
                                    initial_ReadCallback = False

                                Else

                                    JRS = GRT.make_json_string.exe({{"key", "submit_load_nft", "quot"}, {"success", "fail", "quot"}}, {{"reason", "데이터 변조", "quot"}}, False)
                                    Call Send(handler, JRS)

                                End If

                            Else

                                JRS = GRT.make_json_string.exe({{"key", "submit_load_nft", "quot"}, {"success", "fail", "quot"}}, {{"reason", "no_account", "quot"}}, False)
                                Call Send(handler, JRS)

                            End If

                        Else

                            Return

                        End If

                    End Using

                Else

                    length = bytesRead
                    start_index = 0

                End If

            End Using

            If verified And eoa_exist Then

                If Not directory_checked Then

                    If Not Directory.Exists(GRT.GR.nft.destination_folder & "\" & nfa) Then

                        Directory.CreateDirectory(GRT.GR.nft.destination_folder & "\" & nfa)

                    End If

                    directory_checked = True

                End If

                Dim full_name = GRT.GR.nft.destination_folder & "\" & nfa & "\" & nfa & "_" & token_id & "_" & nft_name & "_" & nft_sub_name & "." & extension

                Using writer = New BinaryWriter(File.Open(full_name, FileMode.Append))

                    writer.Write(state.buffer, start_index, CInt(length))

                End Using

                handler.BeginReceive(state.buffer, 0, FILE_StateObject.BufferSize, 0, New AsyncCallback(AddressOf ReadCallback), state)

            End If

        Else

            JRS = GRT.make_json_string.exe({{"key", "submit_load_nft", "quot"}, {"success", "success", "quot"}}, {}, False)

            Call Send(handler, JRS)

            Call SetControl.exe(CType(JsonConvert.DeserializeObject(JRS), Newtonsoft.Json.Linq.JObject))

        End If

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
