Imports System.IO
Imports Newtonsoft.Json
Imports System.Windows.Forms
Imports Microsoft.Web.WebView2.Core

Public Class generate_login_form

    Shared wv As Microsoft.Web.WebView2.WinForms.WebView2
    Shared lbl_login, lbl_login_state, lbl_membership As Label
    Shared login_from As String

    Public Shared Async Sub exe(from As String, wv_received As Microsoft.Web.WebView2.WinForms.WebView2, lbl_login_received As Label, lbl_login_state_received As Label, lbl_membership_received As Label)

        wv = wv_received
        lbl_login = lbl_login_received
        lbl_login_state = lbl_login_state_received
        lbl_membership = lbl_membership_received
        login_from = from

        If lbl_login.Text = "로그인" Then

            Dim html_directory As String

            Await wv.EnsureCoreWebView2Async(Nothing)

            html_directory = Regex.Replace(Directory.GetCurrentDirectory, "guarantee7\\guarantee7.+", "guarantee7\GRT\wv\html")

            wv.CoreWebView2.Navigate(html_directory & "\login.html")

            RemoveHandler wv.CoreWebView2.WebMessageReceived, AddressOf UpdateContent
            AddHandler wv.CoreWebView2.WebMessageReceived, AddressOf UpdateContent

        Else

            GR.account.private_key = ""
            GR.account.public_key = ""
            GR.account.login_state = "no_login"
            lbl_login.Text = "로그인"
            lbl_login_state.Text = "no_login"
            lbl_membership.Enabled = True

            Dim result = GRT.make_json_string.exe({{"key", "login_result", "quot"}, {"success", "success_logout", "quot"}}, {}, True)

            Await wv.CoreWebView2.ExecuteScriptAsync(
                                    $"$('#td_data_input').html('');
                                    c_explanation.exe('{result}');")

        End If

    End Sub
    Shared Async Sub UpdateContent(ByVal sender As Object, ByVal args As CoreWebView2WebMessageReceivedEventArgs)

        Dim coin_name, password, public_key, private_key As String
        Dim effective_public_key As Boolean
        Dim result As String
        Dim result_json As Linq.JObject

        Dim json_message As String = args.WebMessageAsJson()

        Dim json As Newtonsoft.Json.Linq.JObject = CType(JsonConvert.DeserializeObject(json_message), Linq.JObject)

        Dim current_directory As String = Regex.Replace(Directory.GetCurrentDirectory, "(guarantee7|test_guarantee)[^\\]*\\bin\\Debug\\net5.0-windows", "guarantee7_wallet\bin\Debug\net5.0-windows")

        Select Case json("key").ToString

            Case "submit_login"

                coin_name = json("value")("coin_name").ToString
                password = json("value")("password").ToString
                public_key = json("value")("public_key").ToString
                private_key = json("value")("private_key").ToString

                GR.account.password = password
                GR.account.ethereum_transaction_type = json("value")("ethereum_transaction_type").ToString

                If private_key = "" Then

                    ' 먼저 키파일의 존재를 확인한다.

                    private_key = GRT.decrypt_keystore_file.exe(coin_name, password, public_key).ToLower

                    If Regex.Match(private_key, "^0x").Success Then ' 존재하면 그대로 로그인 진행한다.

                        result = Await execute_login.exe(coin_name, public_key, private_key, "general", login_from)

                        GR.account.private_key = private_key

                    Else ' 없으면 private 키를 입력시킨다.

                        result = GRT.make_json_string.exe({{"key", "login_result", "quot"}, {"success", "fail", "quot"}}, {{"public_key", public_key, "quot"}, {"reason", "no_key_file_on_login", "quot"}}, True)

                        GR.account.private_key = ""

                    End If

                Else

                    ' private_key 로 public_key 가 유효한 지 확인. 유효하면 key_file_login_case = False

                    If Not Regex.Match(private_key, "^0x").Success Then
                        private_key = "0x" & private_key
                    End If

                    effective_public_key = GRT.Security.Gverify.public_key(public_key, private_key)

                    If effective_public_key Then

                        result = Await execute_login.exe(coin_name, public_key, private_key, "general", login_from)

                        GR.account.private_key = private_key

                    Else

                        result = GRT.make_json_string.exe({{"key", "login_result", "quot"}, {"success", "fail", "quot"}}, {{"public_key", public_key, "quot"}, {"reason", "ineffective_private_key", "quot"}}, True)

                        GR.account.private_key = ""

                    End If

                End If

                result_json = CType(JsonConvert.DeserializeObject(result), Linq.JObject)

                If result_json("success").ToString = "success" Then

                    Await wv.CoreWebView2.ExecuteScriptAsync(
                                $"c_explanation.exe('{result}');")

                    GR.account.public_key = public_key
                    GR.account.login_state = coin_name
                    GR.account.agency.node = result_json("value")("node").ToString
                    GR.account.agency.exchange_name = result_json("value")("exchange_name").ToString

                    GR.account.agency.domain = result_json("value")("domain").ToString
                    GR.account.agency.domain_agency = result_json("value")("domain_agency").ToString
                    GR.account.agency.domain_web = result_json("value")("domain_web").ToString
                    GR.account.agency.domain_ethereum = result_json("value")("domain_ethereum").ToString
                    GR.account.agency.domain_management = result_json("value")("domain_management").ToString
                    GR.account.agency.domain_nft = result_json("value")("domain_nft").ToString
                    GR.account.agency.ip = result_json("value")("ip").ToString
                    GR.account.agency.ip_agency = result_json("value")("ip_agency").ToString
                    GR.account.agency.ip_web = result_json("value")("ip_web").ToString
                    GR.account.agency.ip_ethereum = result_json("value")("ip_ethereum").ToString
                    GR.account.agency.ip_management = result_json("value")("ip_management").ToString
                    GR.account.agency.ip_nft = result_json("value")("ip_nft").ToString
                    GR.account.agency.port = CInt(result_json("value")("port"))
                    GR.account.agency.port_agency = CInt(result_json("value")("port_agency"))
                    GR.account.agency.port_web = CInt(result_json("value")("port_web"))
                    GR.account.agency.port_ethereum = CInt(result_json("value")("port_ethereum"))
                    GR.account.agency.port_management = CInt(result_json("value")("port_management"))
                    GR.account.agency.port_nft = CInt(result_json("value")("port_nft"))

                    GR.nft.destination_folder = "C:\xampp\virtual\" & GR.account.agency.exchange_name & ".exchange.guarantee7.com\ipfs\NFT"

                    GR.account.super_account.email = result_json("value")("email").ToString
                    GR.account.super_account.name_english = result_json("value")("name_english").ToString
                    GR.account.super_account.name_home_language = result_json("value")("name_home_language").ToString
                    GR.account.super_account.country = result_json("value")("country").ToString
                    GR.account.super_account.phone_number = result_json("value")("phone_number").ToString
                    GR.account.super_account.identity_number = result_json("value")("identity_number").ToString

                    If login_from = "server" Then

                        lbl_login_state.Text =
                            GR.account.super_account.name_english & "(" & GR.account.super_account.name_home_language & ") " & GR.account.super_account.country & vbCrLf &
                            coin_name & " : " & GR.account.public_key & vbCrLf &
                            "node" & " : " & result_json("value")("node").ToString & vbCrLf &
                            "exchange_name" & " : " & result_json("value")("exchange_name").ToString & vbCrLf &
                            "ip" & " : " & result_json("value")("ip").ToString & " " &
                            "port" & " : " & result_json("value")("port").ToString

                    Else

                        lbl_login_state.Text =
                            GR.account.super_account.name_english & "(" & GR.account.super_account.name_home_language & ") " & GR.account.super_account.country & vbCrLf &
                            coin_name & " : " & GR.account.public_key & vbCrLf &
                            "node" & " : " & result_json("value")("node").ToString & vbCrLf &
                            "exchange_name" & " : " & result_json("value")("exchange_name").ToString & vbCrLf &
                            "ip_agency" & " : " & result_json("value")("ip_agency").ToString & " " &
                            "port_agency" & " : " & result_json("value")("port_agency").ToString

                    End If

                    lbl_login.Text = "로그아웃"
                    lbl_membership.Enabled = False

                    '/////////////////////////////////////////////////////////////////////////////////////////

                    ' 이것은 wallet 에서는 하면 안된다. geth 는 노드에서 수동으로 돌려야지 자동으로 하면 안된다.
                    ' wallet 에 게스 운영을 강제하는 것 밖에는 안된다.
                    ' 설치의 경우 역시 선택을 하도록 해야 한다.
                    ' 오로지 서버를 돌릴 때만 게스를 자동으로 스타팅 되도록 해야 한다.

                    'If coin_name = "ethereum" And Not GRT.check_process_running.exe("geth") Then
                    '    Call GRT.geth._start()
                    'End If

                    '/////////////////////////////////////////////////////////////////////////////////////////

                Else

                    If result_json("value")("reason").ToString = "no_account" Or
                        result_json("value")("reason").ToString = "no_account_child_server" Then

                        ' main 서버에서 다시 한번 확인한다.

                        result = Await execute_login.exe(coin_name, public_key, private_key, "main", login_from)

                        result_json = CType(JsonConvert.DeserializeObject(result), Linq.JObject)

                        If result_json("success").ToString = "success" Then

                            GR.account.private_key = private_key

                            GR.account.public_key = public_key
                            GR.account.login_state = coin_name
                            GR.account.agency.node = result_json("value")("node").ToString
                            GR.account.agency.exchange_name = result_json("value")("exchange_name").ToString

                            'GR.account.agency.domain = "http://" & result_json("value")("domain").ToString
                            'GR.account.agency.ip = "http://" & result_json("value")("ip").ToString
                            'GR.account.agency.port = CInt(result_json("value")("port"))
                            'GR.account.agency.port_nft = CInt(result_json("value")("port_nft"))

                            GR.account.agency.domain = result_json("value")("domain").ToString
                            GR.account.agency.domain_agency = result_json("value")("domain_agency").ToString
                            GR.account.agency.domain_web = result_json("value")("domain_web").ToString
                            GR.account.agency.domain_ethereum = result_json("value")("domain_ethereum").ToString
                            GR.account.agency.domain_management = result_json("value")("domain_management").ToString
                            GR.account.agency.domain_nft = result_json("value")("domain_nft").ToString
                            GR.account.agency.ip = result_json("value")("ip").ToString
                            GR.account.agency.ip_agency = result_json("value")("ip_agency").ToString
                            GR.account.agency.ip_web = result_json("value")("ip_web").ToString
                            GR.account.agency.ip_ethereum = result_json("value")("ip_ethereum").ToString
                            GR.account.agency.ip_management = result_json("value")("ip_management").ToString
                            GR.account.agency.ip_nft = result_json("value")("ip_nft").ToString
                            GR.account.agency.port = CInt(result_json("value")("port"))
                            GR.account.agency.port_agency = CInt(result_json("value")("port_agency"))
                            GR.account.agency.port_web = CInt(result_json("value")("port_web"))
                            GR.account.agency.port_ethereum = CInt(result_json("value")("port_ethereum"))
                            GR.account.agency.port_management = CInt(result_json("value")("port_management"))
                            GR.account.agency.port_nft = CInt(result_json("value")("port_nft"))

                            GR.account.balance = CDec(result_json("value")("balance"))

                            GR.nft.destination_folder = "C:\xampp\virtual\" & GR.account.agency.exchange_name & ".exchange.guarantee7.com\ipfs\NFT"

                            GR.account.super_account.email = result_json("value")("email").ToString
                            GR.account.super_account.name_english = result_json("value")("name_english").ToString
                            GR.account.super_account.name_home_language = result_json("value")("name_home_language").ToString
                            GR.account.super_account.country = result_json("value")("country").ToString
                            GR.account.super_account.phone_number = result_json("value")("phone_number").ToString
                            GR.account.super_account.identity_number = result_json("value")("identity_number").ToString

                            lbl_login_state.Text =
                                GR.account.super_account.name_english & "(" & GR.account.super_account.name_home_language & ") " & GR.account.super_account.country & vbCrLf &
                                coin_name & " : " & GR.account.public_key & vbCrLf &
                                "node" & " : " & result_json("value")("node").ToString & vbCrLf &
                                "exchange_name" & " : " & result_json("value")("exchange_name").ToString & vbCrLf &
                                "ip" & " : " & result_json("value")("ip").ToString & " " &
                                "port" & " : " & result_json("value")("port").ToString & " " &
                                "port_agency" & " : " & result_json("value")("port_agency").ToString
                            lbl_login.Text = "로그아웃"
                            lbl_membership.Enabled = False

                        Else

                            GR.account.private_key = ""

                            result_json("value")("coin_name") = coin_name
                            result = CType(JsonConvert.SerializeObject(result_json), String)

                            GR.account.public_key = ""
                            GR.account.login_state = "no_login"
                            lbl_login.Text = "로그인"
                            lbl_login_state.Text = coin_name & " : " & result_json("value")("reason").ToString
                            lbl_membership.Enabled = True

                        End If

                        Await wv.CoreWebView2.ExecuteScriptAsync(
                                        $"c_explanation.exe('{result}');")

                    Else

                        GR.account.login_state = "no_login"
                        lbl_login_state.Text = coin_name & " : " & result_json("value")("reason").ToString

                        Await wv.CoreWebView2.ExecuteScriptAsync(
                                    $"c_explanation.exe('{result}');")

                    End If

                End If

        End Select

    End Sub

End Class
