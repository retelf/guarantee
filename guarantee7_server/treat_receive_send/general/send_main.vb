Imports System.Net.Sockets
Imports System.Text

Public Class send_main

    Public Shared Function exe(signiture_key As String, JSS As String) As String

        Dim JRS As String
        Dim bytes_received As Integer

        Call GRT.agent_record.state_update("send_main_socket_connect_requested", "", signiture_key)

        Using gnet_client As Socket = GRT.Net.Gnet.net_sender(GRT.GR.main_server_address, GRT.GR.port_number_server_main, "localhost", GRT.GR.port_number_server_local)

            Call GRT.agent_record.state_update("send_main_socket_connected", "", signiture_key)

            Dim bytes As Byte() = New Byte(40000) {}

            Dim byte_message_to_send As Byte() = Encoding.Unicode.GetBytes("<#BOF%>" & JSS & "<#EOF%>")

            gnet_client.Send(byte_message_to_send)

            Call GRT.agent_record.state_update("send_main_socket_sended", "", signiture_key)

            ' 여기서 끝나는 경우가 오류의 거의 90% 이다. 자체 송신장애인 경우, 메인서버의 수신장애인 경우

            bytes_received = gnet_client.Receive(bytes)

            Call GRT.agent_record.state_update("send_main_socket_received", "", signiture_key)

            JRS = CStr(Encoding.Unicode.GetString(bytes, 0, bytes_received))

            Return JRS

        End Using

    End Function

End Class
