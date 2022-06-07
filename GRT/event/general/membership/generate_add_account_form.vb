Imports System.IO
Imports Newtonsoft.Json
Imports System.Diagnostics.Debug
Imports System.Windows.Forms
Imports Microsoft.Web.WebView2.Core

Public Class generate_add_account_form

    Shared wv As Microsoft.Web.WebView2.WinForms.WebView2
    Shared lbl_add_account As Label

    Public Shared Async Sub exe(wv_received As Microsoft.Web.WebView2.WinForms.WebView2, lbl_add_account_received As Label)

        If GRT.GR.account.login_state = "guarantee" Then

            Dim JRS = Await check_main_key_login.exe(GR.account.public_key, GR.account.private_key)

            Dim json = CType(JsonConvert.DeserializeObject(JRS), Linq.JObject)

            If json("success").ToString = "success" Then

                If json("value")("representative").ToString = "YES" Then

                    GR.account.login_state_main_key = True

                Else
                    MessageBox.Show("현재 로그인 되어있는 공개키는 메인키가 아닌 일반키입니다. 반드시 개런티코인의 메인공개키 계정으로 로그인 하셔야 합니다. 일단 새롭게 로그인 하시고 다시 시도하시기 바랍니다.")
                    Return
                End If

                wv = wv_received
                lbl_add_account = lbl_add_account_received

                Dim html_directory As String

                Await wv.EnsureCoreWebView2Async(Nothing)

                html_directory = Regex.Replace(Directory.GetCurrentDirectory, "guarantee7\\guarantee7.+", "guarantee7\GRT\wv\html")

                wv.CoreWebView2.Navigate(html_directory & "\add_account.html")

                RemoveHandler wv.CoreWebView2.WebMessageReceived, AddressOf UpdateContent
                AddHandler wv.CoreWebView2.WebMessageReceived, AddressOf UpdateContent

            Else
                MessageBox.Show("오류가 발생하였습니다.")
                Return
            End If

        Else
            MessageBox.Show("먼저 개런티코인의 메인공개키 계정으로 로그인 하셔야 합니다. 일단 새롭게 로그인 하시고 다시 시도하시기 바랍니다.")
            Return
        End If

    End Sub
    Shared Async Sub UpdateContent(ByVal sender As Object, ByVal args As CoreWebView2WebMessageReceivedEventArgs)

        Dim add_type, node, password, public_key, case_insensitive_public_key, private_key, idate_string As String
        Dim JSS, JRS, JRS_for_check, receipt As String
        Dim signiture As String
        Dim json_receipt As Linq.JObject
        Dim whether_key_file_generate As Boolean
        Dim result As Task
        Dim server_listening, db_running As Boolean
        Dim result_json_for_check, JRS_json As Linq.JObject

        Dim json_message As String = args.WebMessageAsJson()

        Dim json As Newtonsoft.Json.Linq.JObject = CType(JsonConvert.DeserializeObject(json_message), Linq.JObject)

        Select Case json("key").ToString

            Case "generate_node_list"

                Dim node_list_script = generate_node_list_script.exe

                node_list_script = Regex.Replace(node_list_script, "'", "\'")

                result = wv.CoreWebView2.ExecuteScriptAsync(
                        $"$('#div_node_outer').append($('{node_list_script}'));")

            Case "submit_add_account"

                add_type = json("value")("add_type").ToString
                password = json("value")("password").ToString
                node = json("value")("node").ToString
                whether_key_file_generate = CBool(json("value")("whether_key_file_generate"))
                idate_string = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.ffffff", Globalization.CultureInfo.InvariantCulture)

                Select Case add_type

                    Case "new"

                        ' 새롭게 키를 발급한다.

                        Dim ecKey As Nethereum.Signer.EthECKey

                        ecKey = Nethereum.Signer.EthECKey.GenerateKey()

                        public_key = ecKey.GetPublicAddress
                        private_key = ecKey.GetPrivateKey

                        JRS = Await execute_add_account_new.exe(ecKey, password, node, whether_key_file_generate, idate_string)

                        result = wv.CoreWebView2.ExecuteScriptAsync(
                                    $"$('#text_public_key').val('{public_key}');
                                    $('#text_private_key').val('{private_key}');
                                    $('#div_public_key_outer').css('display','block');
                                    $('#text_private_key').prop('type','text');
                                    $('#div_private_key_outer').css('display','block');")

                    Case "already"

                        case_insensitive_public_key = json("value")("public_key").ToString
                        private_key = json("value")("private_key").ToString

                        If Regex.Match(private_key, "^0x[\dabcdefABCDEF]{64}$").Success Then

                            ' 먼저 public_key 를 정상화 시킨다.

                            public_key = GRT.restore_ethereum_public_key.exe(case_insensitive_public_key, private_key)

                            If public_key = GRT.Security.Gverify.get_public_key(private_key) Then

                                signiture = GRT.Security.Gsign.sign("foo", private_key)

                                ' 이미 등록된 account 여부를 확인해야 한다.

                                JSS = GRT.make_json_string.exe(
                                        {{"key", "check_registered_eoa", "quot"}},
                                        {
                                        {"public_key", public_key, "quot"},
                                        {"signiture", signiture, "quot"}
                                        }, True)

                                server_listening = GRT.check_process_running.exe("guarantee7_server")
                                db_running = GRT.check_local_service_running.exe("MySql")

                                If server_listening And db_running Then
                                    JRS_for_check = Await GRT.socket_client.exe(GRT.GR.account.agency.domain_agency, GRT.GR.account.agency.port_agency, GRT.GR.temporary_self_client_socket_sender_number, JSS)
                                Else
                                    JRS_for_check = Await Task.Run(Function() GRT.socket_client.exe(GRT.GR.main_server_address_agency, GRT.GR.port_number_server_main_agency, GRT.GR.port_number_server_local, JSS))
                                End If

                                result_json_for_check = CType(JsonConvert.DeserializeObject(JRS_for_check), Linq.JObject)

                                If result_json_for_check("success").ToString = "success" Then

                                    If Not CBool(result_json_for_check("value")("registered").ToString) Then
                                        'If Not CBool(result_json_for_check("value")("registered").ToString) Then

                                        JRS = Await execute_add_account_already.exe(public_key, password, private_key, node, whether_key_file_generate, idate_string)

                                        result = wv.CoreWebView2.ExecuteScriptAsync(
                                            $"$('#text_private_key').prop('type','text');")

                                    Else

                                        JRS = JRS_for_check

                                    End If

                                Else

                                    JRS = JRS_for_check

                                End If

                            Else

                                JRS = "{""key"" : ""check_registered_eoa"", ""success"" : ""fail"", ""value"": {""reason"": ""public_key_private_key_not_match""}}"

                            End If

                        Else

                            JRS = "{""key"" : ""check_registered_eoa"", ""success"" : ""fail"", ""value"": {""reason"": ""inefficient_private_key""}}"

                        End If

                End Select

                json_receipt = CType(JsonConvert.DeserializeObject(JRS), Linq.JObject)

                receipt = GRT.issue_receipt.exe(json_receipt.Root.ToString)

                receipt = Regex.Replace(receipt, "'", "\'")
                receipt = Regex.Replace(receipt, vbCrLf, "<br />")

                Dim scrollTop_code As String =
                    "$('html, body').animate({" &
                    "scrollTop: $('#div_public_key').offset().top" &
                    "}, 500)"

                result = wv.CoreWebView2.ExecuteScriptAsync(
                    $"$('.keys').show();
                    $('#div_explanation').html('{receipt}');
                    {scrollTop_code};")

        End Select

    End Sub

End Class
