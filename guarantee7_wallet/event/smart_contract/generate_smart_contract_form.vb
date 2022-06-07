Imports System.IO
Imports Newtonsoft.Json
Imports System.Diagnostics.Debug
Imports System.Windows.Forms
Imports Microsoft.Web.WebView2.Core

Public Class generate_smart_contract_form

    Shared html_directory As String = Regex.Replace(Directory.GetCurrentDirectory, "guarantee7\\guarantee7.+", "guarantee7\guarantee7_wallet\wv\smart_contract\html")

    Public Shared Sub exe()

        If Not GRT.GR.account.login_state = "no_login" Then

            GR.control.wv_main.CoreWebView2.Navigate(html_directory & "\default.html")

            RemoveHandler GR.control.wv_main.CoreWebView2.WebMessageReceived, AddressOf UpdateContent
            AddHandler GR.control.wv_main.CoreWebView2.WebMessageReceived, AddressOf UpdateContent

            RemoveHandler GR.control.wv_sub.CoreWebView2.WebMessageReceived, AddressOf UpdateContent
            AddHandler GR.control.wv_sub.CoreWebView2.WebMessageReceived, AddressOf UpdateContent

        Else

            MessageBox.Show("먼저 로그인을 하시기 바랍니다.")

            Return

        End If

    End Sub
    Shared Async Sub UpdateContent(ByVal sender As Object, ByVal args As CoreWebView2WebMessageReceivedEventArgs)

        Dim json_message As String = args.WebMessageAsJson()

        Dim receipt, result As String

        Dim json_webview As Newtonsoft.Json.Linq.JObject = CType(JsonConvert.DeserializeObject(json_message), Linq.JObject)

        Select Case json_webview("key").ToString

            Case "load_smart_contract_page"

                Dim smart_contract_name As String
                Dim signiture As String
                Dim JSS, JRS As String
                Dim name, code, extention As String
                Dim files_count As Integer

                ' 메인으로부터 찾느냐 아니면 부모로부터 찾느냐 그것이 문제로다.
                ' 먼저 자신으로부터 찾고 없으면 메인으로 간다. 이것이 가장 기본이다.

                Dim server_listening, db_running As Boolean

                smart_contract_name = json_webview("value")("smart_contract_name").ToString

                signiture = GRT.Security.Gsign.sign("foo", GRT.GR.account.private_key)

                JSS = GRT.make_json_string.exe(
                    {{"key", "load_smart_contract", "quot"}},
                    {
                    {"block_hash", "initial", "quot"},
                    {"eoa", GRT.GR.account.public_key, "quot"},
                    {"smart_contract_name", smart_contract_name, "quot"},
                    {"signiture", signiture, "quot"}}, True)

                server_listening = GRT.check_process_running.exe("guarantee7_server")
                db_running = GRT.check_local_service_running.exe("MySql")

                If server_listening And db_running Then
                    JRS = Await GRT.socket_client.exe(GRT.GR.account.agency.domain_agency, GRT.GR.account.agency.port_agency, GRT.GR.temporary_self_client_socket_sender_number, JSS)
                Else
                    JRS = Await Task.Run(Function() GRT.socket_client.exe(GRT.GR.main_server_address_agency, GRT.GR.port_number_server_main_agency, GRT.GR.port_number_server_local, JSS))
                End If

                Dim json_receipt As Newtonsoft.Json.Linq.JObject = CType(JsonConvert.DeserializeObject(JRS), Linq.JObject)

                receipt = GRT.issue_receipt.exe(json_receipt.Root.ToString)

                If CStr(json_receipt("success")) = "success" Then

                    files_count = json_receipt("value").Count

                    For i = 0 To files_count - 1

                        name = CStr(json_receipt("value")(i)("name"))
                        code = CStr(json_receipt("value")(i)("code"))
                        'code = Regex.Replace(code, "\\""", """")
                        extention = CStr(json_receipt("value")(i)("extention"))

                    Next

                    '//////////////////////////////////////////////////////////////////////////////////////////////////// wv_main

                    'receipt = GRT.issue_receipt.exe(CStr(json_receipt("value")("receipt")("value")("signiture")))

                    receipt = Regex.Replace(receipt, "'", "\'")
                    receipt = Regex.Replace(receipt, vbCrLf, "<br />")

                    Dim scrollTop_code As String =
                        "$('html, body').animate({" &
                        "scrollTop: $('#div_public_key').offset().top" &
                        "}, 500)"

                    result = Await GR.control.wv_main.CoreWebView2.ExecuteScriptAsync(
                        $"$('.keys').show();
                        $('#div_explanation').html('{receipt}');
                        {scrollTop_code};")

                    '//////////////////////////////////////////////////////////////////////////////////////////////////// wv_sub

                    ' 여기서 작업이 필요하다.

                    Call GR.control.wv_sub.NavigateToString(code)

                    '////////////////////////////////////////////////////////////////////////////////////////////////////

                Else

                    result = Await GR.control.wv_main.CoreWebView2.ExecuteScriptAsync($"$('#div_explanation').html('{receipt}');")

                End If

                'Case "submit_exchange"

                '    Dim eoa, coin_name_from, coin_name_to, state As String
                '    Dim pure_query, signiture, signiture_for_get_balance As String
                '    Dim JSS, JRS As String
                '    Dim amount, exchange_rate, gasPrice As Decimal
                '    Dim idate_string = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.ffffff", Globalization.CultureInfo.InvariantCulture)

                '    eoa = json_webview("value")("eoa").ToString
                '    coin_name_from = json_webview("value")("coin_name_from").ToString
                '    coin_name_to = json_webview("value")("coin_name_to").ToString
                '    amount = CDec(json_webview("value")("amount"))

                '    If coin_name_from = "guarantee" Then
                '        gasPrice = 0
                '    Else
                '        gasPrice = CDec(json_webview("value")("gasPrice"))
                '    End If

                '    amount = CDec(json_webview("value")("amount"))
                '    exchange_rate = CDec(json_webview("value")("exchange_rate"))

                '    pure_query = GRT.GQS_exchange.exe(eoa, coin_name_from, coin_name_to, amount, exchange_rate, gasPrice, state, idate_string)

                '    signiture_for_get_balance = GRT.Security.Gsign.sign("foo", GRT.GR.account.private_key)

                '    signiture = GRT.Security.Gsign.sign(pure_query, GRT.GR.account.private_key)

                '    JSS = GRT.make_json_string.exe(
                '                                    {{"key", "submit_exchange", "quot"}},
                '                                    {
                '                                    {"eoa", eoa, "quot"},
                '                                    {"coin_name_from", coin_name_from, "quot"},
                '                                    {"coin_name_to", coin_name_to, "quot"},
                '                                    {"amount", CStr(amount), "non_quot"},
                '                                    {"exchange_rate", CStr(exchange_rate), "non_quot"},
                '                                    {"gasPrice", CStr(gasPrice), "non_quot"},
                '                                    {"state", state, "quot"},
                '                                    {"pure_query", pure_query, "quot"},
                '                                    {"signiture_for_get_balance", signiture_for_get_balance, "quot"},
                '                                    {"signiture", signiture, "quot"},
                '                                    {"idate_string", idate_string, "quot"}}, False)

                '    JRS = Await GRT.socket_client.exe(GRT.GR.main_server_address_agency, GRT.GR.port_number_server_main_agency, GRT.GR.port_number_server_local, JSS)

                '    Dim json_receipt As Newtonsoft.Json.Linq.JObject = CType(JsonConvert.DeserializeObject(JRS), Linq.JObject)

                '    receipt = GRT.issue_receipt.exe(json_receipt.Root.ToString)

                '    receipt = Regex.Replace(receipt, "'", "\'")
                '    receipt = Regex.Replace(receipt, vbCrLf, "<br />")

                '    receipt = GRT.issue_receipt.exe(json_receipt.Root.ToString)

                '    receipt = Regex.Replace(receipt, "'", "\'")
                '    receipt = Regex.Replace(receipt, vbCrLf, "<br />")

                '    result = Await GR.control.wv_main.CoreWebView2.ExecuteScriptAsync($"$('#div_explanation').html('{receipt}');")

        End Select

    End Sub

End Class
