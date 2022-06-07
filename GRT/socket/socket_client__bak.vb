Imports System.Text
Public Class socket_client_bak

    Public Shared Function exe(remote_server_address As String, port_number_server_remote As Integer, port_number_server_local As Integer, json_message As String) As String

        Dim remote_ipHostInfo As IPHostEntry = Dns.GetHostEntry(remote_server_address)
        Dim remote_ipAddress As IPAddress = remote_ipHostInfo.AddressList(remote_ipHostInfo.AddressList.Length - 1)
        Dim remote_EndPoint As IPEndPoint = New IPEndPoint(remote_ipAddress, port_number_server_remote)

        Dim local_ipHostInfo As IPHostEntry = Dns.GetHostEntry(Dns.GetHostName())
        Dim local_ipAddress As IPAddress = local_ipHostInfo.AddressList(1)
        Dim local_EndPoint As IPEndPoint = New IPEndPoint(local_ipAddress, port_number_server_local)

        Using gnet_client As Socket = New Socket(local_EndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp)

            gnet_client.Connect(remote_EndPoint)

            Dim bytes As Byte() = New Byte(40000) {}

            Dim byte_message_to_send As Byte() = Encoding.Unicode.GetBytes(json_message & "<#EOF%>")

            Call gnet_client.Send(byte_message_to_send)

            Dim bytesRec As Integer = gnet_client.Receive(bytes)

            Dim JRS = CStr(Encoding.Unicode.GetString(bytes, 0, bytesRec))

            Return JRS

        End Using

    End Function

End Class
