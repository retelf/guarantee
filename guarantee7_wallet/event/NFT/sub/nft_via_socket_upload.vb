Imports System.IO
Imports Newtonsoft.Json
Imports Microsoft.Web.WebView2.Core
Imports Nethereum.Hex.HexTypes

Public Class nft_via_socket_upload
    Public Shared Async Function exe(json_message As String) As Task(Of Linq.JObject)

        Dim key, coin_name As String
        Dim JSS_token_id, JRS, JRS_token_id As String
        Dim eoa, na, exchange_name, domain, ip As String
        Dim port As Integer

        Dim json_webview, json_JRS_token_id As Newtonsoft.Json.Linq.JObject

        json_webview = CType(JsonConvert.DeserializeObject(json_message), Linq.JObject)

        key = json_webview("key").ToString

        coin_name = GRT.GR.account.login_state

        eoa = GRT.GR.account.public_key
        na = GRT.GR.account.agency.node
        exchange_name = GRT.GR.account.agency.exchange_name
        domain = GRT.GR.account.agency.domain_agency
        ip = GRT.GR.account.agency.ip_agency
        port = GRT.GR.account.agency.port_agency

        Dim new_nfa As Boolean = CBool(json_webview("value")("new_nfa"))
        Dim source = CStr(json_webview("value")("source"))
        Dim file_length As Integer = CInt(json_webview("value")("file_length"))
        Dim nfa As String = CStr(json_webview("value")("nfa"))
        Dim nft_name As String = CStr(json_webview("value")("name"))
        Dim nft_sub_name As String = CStr(json_webview("value")("sub_name"))

        Dim token_id As Integer

        If Not new_nfa Then

            JSS_token_id = GRT.make_json_string.exe(
                                {{"key", "check_nft_token_id", "quot"}},
                                {
                                {"nfa", nfa, "quot"}}, False)

            JRS_token_id = Await GRT.socket_client.exe(GRT.GR.nft_server_address, GRT.GR.port_number_server_nft, GRT.GR.port_number_server_local, JSS_token_id)

            json_JRS_token_id = CType(JsonConvert.DeserializeObject(JRS_token_id), Linq.JObject)

            token_id = CInt(json_JRS_token_id("value")("token_id"))

        Else
            token_id = 0
        End If

        Dim name_and_extension = Regex.Match(source, "[^\\]+$").ToString
        Dim extension = Regex.Match(name_and_extension, "[^\.]+$").ToString

        Dim preBuffer As Byte()

        Dim data = nfa & "|" & token_id & "|" & nft_name & "|" & nft_sub_name & "|" & extension & "|" & file_length

        Dim signiture = GRT.Security.Gsign.sign(data, GRT.GR.account.private_key)

        data = data & "|" & eoa & "|" & signiture

        Using memoryStream = New MemoryStream()

            Using writer = New BinaryWriter(memoryStream)

                writer.Write(data)

            End Using

            preBuffer = memoryStream.ToArray()

            JRS = Await GRT.socket_client.send_file(GRT.GR.account.agency.ip, GRT.GR.account.agency.port_nft, GRT.GR.port_number_server_local, source, preBuffer)
            'JRS = Await GRT.socket_client.send_file(GRT.GR.main_server_address_agency, GRT.GR.port_number_server_main_agency + 10, GRT.GR.port_number_server_local, source, preBuffer)

        End Using

        Return CType(JsonConvert.DeserializeObject(JRS), Linq.JObject)

    End Function

End Class
