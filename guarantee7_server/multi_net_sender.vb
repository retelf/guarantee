Imports System.Net.Sockets
Imports System.Text

Public Class multi_net_sender

    Public Shared Async Sub exe(json As Newtonsoft.Json.Linq.JObject, i_number As Integer, Dataset_node As DataSet, JSS As String)

        Dim domain, ip, rep_adress As String
        Dim port As Integer
        Dim string_received As String
        Dim bytes_received As Integer

        domain = CStr(Dataset_node.Tables(0).Rows(i_number)("domain"))
        ip = CStr(Dataset_node.Tables(0).Rows(i_number)("ip"))
        port = CInt(Dataset_node.Tables(0).Rows(i_number)("port"))

        If Not domain = "" Then
            rep_adress = domain
        Else
            rep_adress = ip
        End If

        Using gnet_client As Socket = GRT.Net.Gnet.net_sender(rep_adress, port, "localhost", GRT.GR.port_number_server_local)

            Dim bytes As Byte() = New Byte(40000) {}

            Dim byte_message_to_send As Byte() = Encoding.Unicode.GetBytes("<#BOF%>" & JSS & "<#EOF%>")

            gnet_client.Send(byte_message_to_send)

            bytes_received = gnet_client.Receive(bytes)

            string_received = CStr(Encoding.Unicode.GetString(bytes, 0, bytes_received))

            ' 데이터베이스에 교신 내용을 기록한다.
            ' 그리고 리턴을 받아서 기여도 자료를 확보한다.
            ' 등등

            If json("key").ToString = "relay" Then ' 스스로가 전달받은 서버이다.

            End If

        End Using

    End Sub

End Class
