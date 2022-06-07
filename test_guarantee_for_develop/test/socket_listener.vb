Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.Text.RegularExpressions
Imports Newtonsoft.Json

Public Class socket_listener

    Public Shared data As String = Nothing

    Public Shared Async Sub exe()

        Dim json_str_message_received, JSS As String
        Dim bytes As Byte() = New Byte(262144) {} ' 약 A4 25페이지
        Dim net_bytes() As Byte

        ' Establish the local endpoint for the socket.  
        ' Dns.GetHostName returns the name of the host running the application. 
        Dim ipHostInfo As IPHostEntry = Dns.GetHostEntry(Dns.GetHostName())
        Dim ipAddress As IPAddress = ipHostInfo.AddressList(1)
        Dim localEndPoint As IPEndPoint = New IPEndPoint(ipAddress, 40700)
        Dim listener As Socket = New Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp)

        listener.Bind(localEndPoint) ' Bind 는 포트번호를 배타적으로 독점한다. 
        listener.Listen(10000)

        While True

            Dim handler As Socket = listener.Accept()

            json_str_message_received = Nothing

            Dim bytes_length As Integer

            While True

                bytes_length = handler.Receive(bytes)

                json_str_message_received &= Encoding.Unicode.GetString(bytes, 0, bytes_length)

                If json_str_message_received.IndexOf("<#EOF%>") > -1 Then

                    ReDim net_bytes(bytes_length - 6)

                    Array.Copy(bytes, 0, net_bytes, 0, bytes_length - 5)

                    Exit While

                End If

                If Regex.Match(json_str_message_received, "test").Success Then

                End If

            End While

            ' 명령처리

            ' 서버에서 할 일
            ' 먼저 public_open 을 확인한다.
            ' public_open 인 경우 곧바로 서버에 잔고를 보내고
            ' public_open 이 아닌 경우 검증절차를 거친다.
            ' signature 는 "password" 에 대한 signature 이다.

            JSS = Regex.Replace(json_str_message_received, "<#EOF%>.*", "")

            handler.Shutdown(SocketShutdown.Both)

            handler.Close()

        End While

    End Sub

End Class

