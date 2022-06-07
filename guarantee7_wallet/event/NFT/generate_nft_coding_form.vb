Imports System.IO
Imports Newtonsoft.Json
Imports Microsoft.Web.WebView2.Core
Imports Nethereum.Hex.HexTypes

Public Class generate_nft_coding_form

    Shared html_directory As String = Regex.Replace(Directory.GetCurrentDirectory, "guarantee7\\guarantee7.+", "guarantee7\guarantee7_wallet\wv\nft\html")

    Shared open_file_dialog As New OpenFileDialog
    Public Shared Sub exe()

        If GRT.GR.account.login_state = "guarantee" Then

            GR.control.wv_main.CoreWebView2.Navigate(html_directory & "\default.html")
            GR.control.wv_sub.CoreWebView2.Navigate(html_directory & "\sell_orders.html")

            RemoveHandler GR.control.wv_main.CoreWebView2.WebMessageReceived, AddressOf UpdateContent
            AddHandler GR.control.wv_main.CoreWebView2.WebMessageReceived, AddressOf UpdateContent

            RemoveHandler GR.control.wv_sub.CoreWebView2.WebMessageReceived, AddressOf UpdateContent
            AddHandler GR.control.wv_sub.CoreWebView2.WebMessageReceived, AddressOf UpdateContent

        Else

            MessageBox.Show("먼저 개런티 계정으로 로그인을 하시기 바랍니다.")

            Return

        End If

    End Sub
    Shared Async Sub UpdateContent(ByVal sender As Object, ByVal args As CoreWebView2WebMessageReceivedEventArgs)

        Dim key, coin_name, file_source, html_script As String

        Dim json_JSS As String = args.WebMessageAsJson()

        Dim json_webview, json_nft_via_socket_upload As Newtonsoft.Json.Linq.JObject

        Dim JSS_nfa, JRS_nfa, JSS_token_id, JRS_token_id, name, creator As String
        Dim json_JRS_nfa, json_JRS_token_id As Newtonsoft.Json.Linq.JObject

        json_webview = CType(JsonConvert.DeserializeObject(json_JSS), Linq.JObject)

        key = json_webview("key").ToString

        coin_name = GRT.GR.account.login_state

        Select Case key

            Case "initialize_nft_form"

                Call initialize_nft_form.exe()

            Case "open_file_dialog"

                Dim dialog_result = open_file_dialog.ShowDialog()

                file_source = open_file_dialog.FileName

                Await GR.control.wv_main.CoreWebView2.ExecuteScriptAsync(
                        $"$('#div_source').html('{Regex.Replace(file_source, "\\", "\\")}');
                        $('#div_media').html('{"<object data=\'" & Regex.Replace(file_source, "\\", "\\") & "\'>이 브라우저는 본 객체를 지원하지 않습니다</object>"}');")

            Case "check_nfa_and_creator"

                json_webview = CType(JsonConvert.DeserializeObject(json_JSS), Linq.JObject)

                key = json_webview("key").ToString

                Dim nfa As String = CStr(json_webview("value")("nfa"))

                If Regex.Match(nfa, "^0x.{40}$").Success Then

                    JSS_nfa = GRT.make_json_string.exe(
                                {{"key", key, "quot"}},
                                {
                                {"nfa", nfa, "quot"}}, False)

                    JRS_nfa = Await GRT.socket_client.exe(GRT.GR.nft_server_address, GRT.GR.port_number_server_nft, GRT.GR.port_number_server_local, JSS_nfa)

                    json_JRS_nfa = CType(JsonConvert.DeserializeObject(JRS_nfa), Linq.JObject)

                    If json_JRS_nfa("success").ToString = "success" Then

                        name = json_JRS_nfa("value")("name").ToString
                        creator = json_JRS_nfa("value")("creator").ToString

                        Await GR.control.wv_main.CoreWebView2.ExecuteScriptAsync(
                                                $"$('#text_name').val('{name}');
                                                $('#text_creator').val('{creator}');
                                                $('#text_name').prop('disabled', 'disabled');
                                                $('#text_creator').prop('disabled', 'disabled');")

                    Else

                        Dim reason = json_JRS_nfa("value")("reason").ToString

                        If reason = "no_rows" Then

                            Dim comment = "기존에 존재하는 NFT의 nfa 가 발견되지 않습니다. " &
                                "이는 부정확한 nfa 를 입력했기 때문입니다. 정확한 nfa 를 기입하시기 바랍니다. " &
                                "만약 새로운 nfa 를 생성하고 그 하위 token_id 를 형성시키려 하신다면 NEW 를 입력하시면 됩니다. " &
                                "이 경우 token_id 는 0번부터 시작하게 됩니다."

                            Await GR.control.wv_main.CoreWebView2.ExecuteScriptAsync(
                                $"$('#div_explanation').html('{comment}');
                                $('#text_nfa').val('{"NEW"}');")

                        Else

                            Await GR.control.wv_main.CoreWebView2.ExecuteScriptAsync(
                                $"$('#div_explanation').html('{reason}');
                            $('#text_nfa').val('{"NEW"}');")

                        End If

                    End If

                Else
                    MessageBox.Show("nfa 기재에 오류가 있습니다.")
                End If

            Case "submit_load_nft"

                Dim new_nfa As Boolean = CBool(json_webview("value")("new_nfa"))

                Dim source = CStr(json_webview("value")("source"))

                Dim file_info As New FileInfo(source)

                json_webview("value")("file_length") = file_info.Length

                Dim nfa As String = CStr(json_webview("value")("nfa"))

                '\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

                ' nfa, token_id, extension 결정

                If new_nfa Then

                    Dim ecKey = Nethereum.Signer.EthECKey.GenerateKey()

                    json_webview("value")("new_nfa") = True
                    json_webview("value")("nfa") = ecKey.GetPublicAddress
                    json_webview("value")("token_id") = 0

                Else

                    json_webview("value")("new_nfa") = False

                    JSS_token_id = GRT.make_json_string.exe(
                                        {{"key", "check_nft_token_id", "quot"}},
                                        {
                                        {"nfa", nfa, "quot"}}, False)

                    JRS_token_id = Await GRT.socket_client.exe(GRT.GR.nft_server_address, GRT.GR.port_number_server_nft, GRT.GR.port_number_server_local, JSS_token_id)

                    json_JRS_token_id = CType(JsonConvert.DeserializeObject(JRS_token_id), Linq.JObject)

                    json_webview("value")("token_id") = CInt(json_JRS_token_id("value")("token_id"))

                End If

                Dim filename_and_extension = Regex.Match(source, "[^\\]+$").ToString

                Dim extension = Regex.Match(filename_and_extension, "[^\.]+$").ToString

                json_webview("value")("extension") = extension

                '\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\ 파일 전송

                json_JSS = CType(JsonConvert.SerializeObject(json_webview), String)

                json_nft_via_socket_upload = Await nft_via_socket_upload.exe(json_JSS)

                '\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\ 블록체인 deploy

                If json_nft_via_socket_upload("success").ToString = "success" Then

                    Call submit_nft_data.exe(json_JSS)

                Else

                    Await GR.control.wv_main.CoreWebView2.ExecuteScriptAsync($"$('#div_explanation').html('{json_nft_via_socket_upload("value")("reason").ToString}');")

                End If

            Case "display_board_sell_order" ' 전광판

        End Select

    End Sub

End Class