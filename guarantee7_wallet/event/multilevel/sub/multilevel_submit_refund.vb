Imports System.IO
Imports Newtonsoft.Json
Imports Microsoft.Web.WebView2.Core
Imports Nethereum.Hex.HexTypes

Public Class multilevel_submit_refund
    Public Shared Async Sub exe(json_message As String)

        Dim key As String
        Dim signiture As String
        Dim JSS, JRS, receipt, pure_query As String
        Dim sell_order_block_number As Long
        Dim eoa_guarantee_seller, seller_na, seller_agency_domain, seller_agency_ip, idate_string As String
        Dim seller_agency_port As Integer

        Dim json_webview As Newtonsoft.Json.Linq.JObject = CType(JsonConvert.DeserializeObject(json_message), Linq.JObject)

        key = json_webview("key").ToString

        sell_order_block_number = CLng(json_webview("value")("block_number"))
        eoa_guarantee_seller = CStr(json_webview("value")("eoa").ToString.Trim)
        seller_na = CStr(json_webview("value")("na").ToString.Trim)
        seller_agency_domain = json_webview("value")("domain").ToString
        seller_agency_ip = CStr(json_webview("value")("ip").ToString.Trim)
        seller_agency_port = CInt(json_webview("value")("port").ToString.Trim)

        idate_string = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.ffffff", Globalization.CultureInfo.InvariantCulture)

        pure_query = GRT.GQS_multilevel_refund.exe(sell_order_block_number, eoa_guarantee_seller, seller_na, 1, idate_string)

        signiture = GRT.Security.Gsign.sign(pure_query, GRT.GR.account.private_key)

        JSS = GRT.make_json_string.exe(
                    {{"key", key, "quot"}},
                    {
                    {"block_hash", "initial", "quot"},
                    {"sell_order_block_number", CStr(sell_order_block_number), "non_quot"},
                    {"eoa_guarantee_seller", eoa_guarantee_seller, "quot"}, ' eoa_buyer 이다.
                    {"seller_na", seller_na, "quot"},
                    {"seller_agency_domain", seller_agency_domain, "quot"},
                    {"seller_agency_ip", seller_agency_ip, "quot"},
                    {"seller_agency_port", CStr(seller_agency_port), "non_quot"},
                    {"pure_query", pure_query, "quot"},
                    {"signiture", signiture, "quot"},
                    {"initial_transfer", "Y", "quot"},
                    {"idate_string", idate_string, "quot"}}, True)

        JRS = Await GRT.socket_client.exe(seller_agency_ip, seller_agency_port, GRT.GR.port_number_server_local, JSS)

        Dim json_receipt As Newtonsoft.Json.Linq.JObject = CType(JsonConvert.DeserializeObject(JRS), Linq.JObject)

        receipt = GRT.issue_receipt.exe(json_receipt.Root.ToString)

        receipt = Regex.Replace(receipt, "'", "\'")
        receipt = Regex.Replace(receipt, vbCrLf, "<br />")

        Call GR.control.wv_main.CoreWebView2.ExecuteScriptAsync(
                    $"$('#div_explanation').html('{receipt}');")

    End Sub

End Class
