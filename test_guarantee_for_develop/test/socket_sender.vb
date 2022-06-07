Imports System.Net.Sockets
Imports System.Text
Imports Newtonsoft.Json

Public Class socket_sender

    Public Shared Async Sub exe()

        Dim JRS As String

        Dim pure_query = GRT.make_json_string.exe(
                        {{"chainId", "321456", "non_quot"},
                        {"nonce", CStr(100), "non_quot"},
                        {"from", "0xe58a43B5b46b91184467FD2e5b594B4441682126", "quot"},
                        {"to", "0x7c045ee35f3cb49d9d390a55fad3baf64a33220c", "quot"},
                        {"gas", "0x" & CStr(3), "quot"},
                        {"id", CStr(1), "non_quot"},
                        {"gasPrice", "0x" & CStr(3), "quot"},
                        {"jsonrpc", "2.0", "quot"},
                        {"method", "send_raw_transaction", "quot"}},
                        {}, False)

        Dim signiture = GRT.Security.Gsign.sign(pure_query, "0xbebd07320925652d273a1b25ed9cc5b81d7f28831be83b11c9c1a203c7461c0b")

        Dim JSS = GRT.make_json_string.exe(
                        {{"chainId", "321456", "non_quot"},
                        {"nonce", CStr(100), "non_quot"},
                        {"from", "0xe58a43B5b46b91184467FD2e5b594B4441682126", "quot"},
                        {"to", "0x7c045ee35f3cb49d9d390a55fad3baf64a33220c", "quot"},
                        {"gas", "0x" & CStr(3), "quot"},
                        {"id", CStr(1), "non_quot"},
                        {"gasPrice", "0x" & CStr(3), "quot"},
                        {"jsonrpc", "2.0", "quot"},
                        {"method", "send_raw_transaction", "quot"},
                        {"data", pure_query, "non_quot"},
                        {"params", "[" & signiture & "]", "quot"}},
                        {}, False)

        Dim deJSS = CType(JsonConvert.DeserializeObject(JSS), Linq.JObject)

        Using gnet_client As Socket = GRT.Net.Gnet.net_sender("localhost", 8545, "localhost", 40700)

            Dim bytes As Byte() = New Byte(40000) {}

            Dim byte_message_to_send As Byte() = Encoding.Unicode.GetBytes(JSS)

            Await Task.Run(Sub() gnet_client.Send(byte_message_to_send))

            Dim bytesRec As Integer = Await Task.Run(Function() gnet_client.Receive(bytes))

            JRS = CStr(Encoding.Unicode.GetString(bytes, 0, bytesRec))

        End Using

    End Sub

End Class
