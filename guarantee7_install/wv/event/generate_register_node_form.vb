Imports System.IO
Imports Newtonsoft.Json
Imports System.Diagnostics.Debug
Imports System.Windows.Forms
Imports Microsoft.Web.WebView2.Core

Public Class generate_register_node_form

    Public Shared Async Sub exe()

        If GRT.GR.account.login_state = "guarantee" Then

            Dim html_directory As String

            Await GR.control.wv.EnsureCoreWebView2Async(Nothing)

            html_directory = Regex.Replace(Directory.GetCurrentDirectory, "guarantee7\\guarantee7.+", "guarantee7\guarantee7_install\wv\html")

            GR.control.wv.CoreWebView2.Navigate(html_directory & "\register_node.html")

            AddHandler GR.control.wv.CoreWebView2.WebMessageReceived, AddressOf UpdateContent

        Else

            MessageBox.Show("먼저 개런티 계정으로 로그인을 하시기 바랍니다.")

            Return

        End If

    End Sub
    Shared Async Sub UpdateContent(ByVal sender As Object, ByVal args As CoreWebView2WebMessageReceivedEventArgs)

        Dim pure_query, eoa, na, exchange_name, domain, ip, type, signiture, idate_string As String
        Dim port, port_nft As Integer
        Dim JSS, JRS As String

        GRT.GR.port_number_server_local = CInt(GRT.get_info_from_ini_file.server("port_default"))

        Dim json_message As String = args.WebMessageAsJson()

        Dim json_webview As Newtonsoft.Json.Linq.JObject = CType(JsonConvert.DeserializeObject(json_message), Linq.JObject)

        Dim script As String = ""

        Select Case json_webview("key").ToString

            Case "initialize_register_info"

                script &=
                    "$('#text_eoa').val('" & GRT.GR.account.public_key & "').prop('disabled', 'disabled');" &
                    "$('#text_na').val('" & GRT.get_local_server_id.exe & "').prop('disabled', 'disabled');" &
                    "$('#text_port').val('" & GRT.get_info_from_ini_file.server("port_default") & "').prop('disabled', 'disabled');" &
                    "$('#text_port_nft').val('" & GRT.get_info_from_ini_file.server("port_nft") & "').prop('disabled', 'disabled');"

                Await GR.control.wv.CoreWebView2.ExecuteScriptAsync($"" & script)

            Case "submit_register_info"

                ' pure_query 를 만든다. 그런데 이 때 public_key 는 html 의 text_public_key 에서 찾는다.

                eoa = json_webview("value")("eoa").ToString
                na = json_webview("value")("na").ToString.Trim
                exchange_name = json_webview("value")("exchange_name").ToString.Trim
                domain = json_webview("value")("domain").ToString.Trim
                ip = json_webview("value")("ip").ToString.Trim
                port = CInt(json_webview("value")("port").ToString.Trim)
                port_nft = CInt(json_webview("value")("port_nft").ToString.Trim)
                type = json_webview("value")("type").ToString.Trim

                idate_string = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.ffffff", Globalization.CultureInfo.InvariantCulture)
                pure_query = GRT.GQS_register_server.exe(eoa, na, exchange_name, type, domain, ip, port, port_nft, idate_string)

                signiture = GRT.Security.Gsign.sign(pure_query, GRT.GR.account.private_key)

                JSS = GRT.make_json_string.exe({{"key", "register_node", "quot"}},
                                               {{"block_hash", "initial", "quot"}, {"na", na, "quot"}, {"exchange_name", exchange_name, "quot"}, {"eoa", eoa, "quot"}, {"type", type, "quot"}, {"domain", domain, "quot"}, {"ip", ip, "quot"}, {"port", CStr(port), "non_quot"}, {"idate_string", idate_string, "quot"}, {"signiture", signiture, "quot"}}, True)

                JRS = Await GRT.socket_client.exe(GRT.GR.main_server_address_agency, GRT.GR.port_number_server_main_agency, GRT.GR.port_number_server_local, JSS)

                Dim json_receipt As Newtonsoft.Json.Linq.JObject = CType(JsonConvert.DeserializeObject(JRS), Linq.JObject)            

                Dim receipt As String

                If CStr(json_receipt("success")) = "success" Then

                    'receipt = GRT.issue_receipt.exe(CStr(json_receipt("value")("receipt")("value")("signiture")))
                    receipt = GRT.issue_receipt.exe(json_receipt.Root.ToString)

                    receipt = Regex.Replace(receipt, "'", "\'")
                    receipt = Regex.Replace(receipt, vbCrLf, "<br />")

                    Dim scrollTop_code As String =
                        "$('html, body').animate({" &
                        "scrollTop: $('#div_eoa').offset().top" &
                        "}, 500)"

                    Dim result = GR.control.wv.CoreWebView2.ExecuteScriptAsync(
                        $"$('.keys').show();
                        $('#div_explanation').html('{receipt}');
                        {scrollTop_code};")

                Else

                    MessageBox.Show(CStr(json_receipt("value")("reason")))

                End If

        End Select

    End Sub

End Class
