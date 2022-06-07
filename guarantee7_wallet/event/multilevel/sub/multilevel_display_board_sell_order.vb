Imports System.IO
Imports Newtonsoft.Json
Imports Microsoft.Web.WebView2.Core
Imports Nethereum.Hex.HexTypes

Public Class multilevel_display_board_sell_order
    Public Shared Async Sub exe()

        Dim eoa, coin_name As String
        Dim JSS, JRS As String

        eoa = GRT.GR.account.public_key

        coin_name = GRT.GR.account.login_state

        JSS = GRT.make_json_string.exe(
                    {{"key", "display_board_sell_order", "quot"}},
                    {{"block_hash", "initial", "quot"},
                    {"eoa", eoa, "quot"},
                    {"coin_name", coin_name, "quot"},
                    {"signiture", GRT.Security.Gsign.sign("foo", GRT.GR.account.private_key), "quot"}}, True)

        JRS = Await GRT.socket_client.exe(GRT.GR.main_server_address_agency, GRT.GR.port_number_server_main_agency, GRT.GR.port_number_server_local, JSS)
        'JRS = Await GRT.socket_client.exe(GRT.GR.account.agency.domain_agency, GRT.GR.account.agency.port_agency, GRT.GR.port_number_server_local, JSS)

        Dim JRS_json = CType(JsonConvert.DeserializeObject(JRS), Linq.JObject)

        Dim DataSet = JsonConvert.DeserializeObject(Of DataSet)(JRS_json("value")("dataset_json_string").ToString)

        Dim board_exchange_script = generate_display_board_sell_order_script.exe(DataSet)

        Await GR.control.wv_sub.CoreWebView2.ExecuteScriptAsync(
                $"$('#div_order').html('{Regex.Replace(board_exchange_script, "'", "\'")}');")

    End Sub

End Class
