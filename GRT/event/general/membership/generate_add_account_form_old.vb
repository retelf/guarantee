Imports System.IO
Imports Newtonsoft.Json
Imports System.Diagnostics.Debug
Imports System.Windows.Forms
Imports Microsoft.Web.WebView2.Core

Public Class generate_add_account_form_old

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

        Dim coin_name, node, password, public_key, private_key, idate_string As String
        Dim JRS, receipt As String
        Dim json_receipt As Linq.JObject
        Dim whether_key_file_generate As Boolean
        Dim result As Task

        Dim json_message As String = args.WebMessageAsJson()

        Dim json As Newtonsoft.Json.Linq.JObject = CType(JsonConvert.DeserializeObject(json_message), Linq.JObject)

        Select Case json("key").ToString

            Case "submit_add_account"

                coin_name = json("value")("coin_name").ToString
                node = json("value")("node").ToString
                whether_key_file_generate = CBool(json("value")("whether_key_file_generate"))
                idate_string = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.ffffff", Globalization.CultureInfo.InvariantCulture)

                Select Case coin_name

                    Case "guarantee"

                        ' 새롭게 키를 발급한다.

                        Dim ecKey As Nethereum.Signer.EthECKey

                        ecKey = Nethereum.Signer.EthECKey.GenerateKey()

                        public_key = ecKey.GetPublicAddress
                        private_key = ecKey.GetPrivateKey

                        password = json("value")("password").ToString

                        JRS = Await execute_add_account_guarantee.exe(ecKey, password, node, whether_key_file_generate, idate_string) ' 로그인 상태이므로 메인 eoa 정보이다.

                        result = wv.CoreWebView2.ExecuteScriptAsync(
                                $"$('#text_public_key').val('{public_key}');
                                $('#text_private_key').val('{private_key}');
                                $('#div_public_key_outer').css('display','block');
                                $('#text_private_key').prop('type','text');
                                $('#div_private_key_outer').css('display','block');")

                    Case "ethereum"

                        public_key = json("value")("public_key").ToString
                        private_key = json("value")("private_key").ToString

                        JRS = Await execute_add_account_ethereum.exe(public_key, private_key, node, idate_string) ' 로그인 상태이므로 메인 eoa 정보이다.

                End Select

                Await wv.CoreWebView2.ExecuteScriptAsync(
                            $"c_explanation.exe('{JRS}');")

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
