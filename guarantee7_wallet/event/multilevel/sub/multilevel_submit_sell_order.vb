Imports System.IO
Imports Newtonsoft.Json
Imports Microsoft.Web.WebView2.Core
Imports Nethereum.Hex.HexTypes

Public Class multilevel_submit_sell_order
    Public Shared Async Sub exe(json_message As String)

        Dim key, coin_name As String
        Dim signiture, signiture_for_get_balance As String
        Dim JSS, JRS, receipt, pure_query As String
        Dim date_now_utc As DateTime
        Dim days_span As Integer
        Dim eoa, na, exchange_name, domain, ip, idate_string, closing_time_utc_string As String
        Dim port As Integer
        Dim balance, exchange_rate As Decimal
        Dim result As Task(Of String)

        Dim json_webview As Newtonsoft.Json.Linq.JObject = CType(JsonConvert.DeserializeObject(json_message), Linq.JObject)

        key = json_webview("key").ToString

        coin_name = GRT.GR.account.login_state

        balance = CDec(json_webview("value")("balance").ToString.Trim)
        exchange_rate = CDec(json_webview("value")("exchange_rate").ToString.Trim)
        days_span = CInt(json_webview("value")("days_span").ToString.Trim)

        eoa = GRT.GR.account.public_key
        na = GRT.GR.account.agency.node
        exchange_name = GRT.GR.account.agency.exchange_name
        domain = GRT.GR.account.agency.domain_agency
        ip = GRT.GR.account.agency.ip_agency
        port = GRT.GR.account.agency.port_agency

        date_now_utc = GRT.GetNistTime.exe

        'closing_time_utc_string = date_now_utc.AddDays(days_span).ToString("yyyy/MM/dd HH:mm:ss")
        closing_time_utc_string = "0000-00-00 00:00:00"

        idate_string = date_now_utc.ToString("yyyy/MM/dd HH:mm:ss")

        pure_query = GRT.GQS_multilevel_sell_order.exe(eoa, na, exchange_name, domain, ip, port, exchange_rate, days_span, closing_time_utc_string, GRT.GR.multilevel_exchange_fee_rate, idate_string)

        signiture_for_get_balance = GRT.Security.Gsign.sign("foo", GRT.GR.account.private_key)

        signiture = GRT.Security.Gsign.sign(pure_query, GRT.GR.account.private_key)

        JSS = GRT.make_json_string.exe(
                    {{"key", "submit_sell_order", "quot"}},
                    {
                    {"block_hash", "initial", "quot"},
                    {"eoa", eoa, "quot"},
                    {"na", na, "quot"},
                    {"exchange_name", exchange_name, "quot"},
                    {"agency_domain", domain, "quot"},
                    {"agency_ip", ip, "quot"},
                    {"agency_port", CStr(port), "non_quot"},
                    {"coin_name", "guarantee", "quot"},
                    {"exchange_rate", CStr(exchange_rate), "non_quot"},
                    {"days_span", CStr(days_span), "non_quot"},
                    {"closing_time_utc_string", closing_time_utc_string, "quot"},
                    {"exchange_fee_rate", CStr(GRT.GR.multilevel_exchange_fee_rate), "non_quot"},
                    {"state", "alive", "quot"},
                    {"pure_query", pure_query, "quot"},
                    {"signiture_for_get_balance", signiture_for_get_balance, "quot"},
                    {"signiture", signiture, "quot"},
                    {"initial_transfer", "Y", "quot"},
                    {"idate_string", idate_string, "quot"}}, True)

        'JRS = Await GRT.socket_client.exe(GRT.GR.main_server_address_agency, GRT.GR.port_number_server_main_agency, GRT.GR.port_number_server_local, JSS)
        JRS = Await GRT.socket_client.exe(GRT.GR.account.agency.ip_agency, GRT.GR.account.agency.port_agency, GRT.GR.port_number_server_local, JSS)

        Dim json_receipt As Newtonsoft.Json.Linq.JObject = CType(JsonConvert.DeserializeObject(JRS), Linq.JObject)

        receipt = GRT.issue_receipt.exe(json_receipt.Root.ToString)

        receipt = Regex.Replace(receipt, "'", "\'")
        receipt = Regex.Replace(receipt, vbCrLf, "<br />")

        result = GR.control.wv_main.CoreWebView2.ExecuteScriptAsync(
                    $"$('#div_explanation').html('{receipt}');")

    End Sub

End Class
