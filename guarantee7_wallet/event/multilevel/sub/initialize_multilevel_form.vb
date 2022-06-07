Imports System.IO
Imports Newtonsoft.Json
Imports Microsoft.Web.WebView2.Core
Imports Nethereum.Hex.HexTypes

Public Class initialize_multilevel_form
    Public Shared Async Sub exe()

        Dim coin_name As String
        Dim signiture_for_get_balance As String
        Dim balance As Decimal
        Dim result As Task(Of String)

        coin_name = GRT.GR.account.login_state

        signiture_for_get_balance = GRT.Security.Gsign.sign("foo", GRT.GR.account.private_key)

        Dim json_receipt As Newtonsoft.Json.Linq.JObject = Await GRT.get_balance_via_socket.exe(coin_name, GRT.GR.account.public_key, signiture_for_get_balance)

        result = GR.control.wv_main.CoreWebView2.ExecuteScriptAsync($"$('#div_coin_name').html('{GRT.GR.account.login_state}');")

        If CStr(json_receipt("success")) = "success" Then

            balance = CDec(json_receipt("value")("balance"))

            result = GR.control.wv_main.CoreWebView2.ExecuteScriptAsync(
                        $"$('#div_balance').html({balance});")

        Else

            result = GR.control.wv_main.CoreWebView2.ExecuteScriptAsync(
                        $"$('#div_explanation').html('{CStr(json_receipt("value")("reason"))}');")

        End If

    End Sub

End Class
