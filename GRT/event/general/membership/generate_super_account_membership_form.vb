Imports System.IO
Imports Newtonsoft.Json
Imports System.Diagnostics.Debug
Imports System.Windows.Forms
Imports Microsoft.Web.WebView2.Core

Public Class generate_super_account_membership_form

    Shared wv As Microsoft.Web.WebView2.WinForms.WebView2
    Shared lbl_login, lbl_membership As Label

    Public Shared Async Sub exe(wv_received As Microsoft.Web.WebView2.WinForms.WebView2, lbl_login_received As Label, lbl_membership_received As Label)

        wv = wv_received

        Dim html_directory As String

        Await wv.EnsureCoreWebView2Async(Nothing)

        html_directory = Regex.Replace(Directory.GetCurrentDirectory, "guarantee7\\guarantee7.+", "guarantee7\GRT\wv\html")

        wv.CoreWebView2.Navigate(html_directory & "\membership.html")

        RemoveHandler wv.CoreWebView2.WebMessageReceived, AddressOf UpdateContent
        AddHandler wv.CoreWebView2.WebMessageReceived, AddressOf UpdateContent

    End Sub
    Shared Async Sub UpdateContent(ByVal sender As Object, ByVal args As CoreWebView2WebMessageReceivedEventArgs)

        Dim pure_query, public_key, private_key, password, node, signiture, idate_string, hash As String
        Dim whether_key_file_generate As Boolean
        Dim ecKey As Nethereum.Signer.EthECKey
        Dim receipt_string As String
        Dim json_receipt As Newtonsoft.Json.Linq.JObject

        GRT.GR.port_number_server_local = CInt(GRT.get_info_from_ini_file.server("port_default"))

        Dim json_message As String = args.WebMessageAsJson()

        Dim json_webview As Newtonsoft.Json.Linq.JObject = CType(JsonConvert.DeserializeObject(json_message), Linq.JObject)

        Select Case json_webview("key").ToString

            Case "submit_membership_info"

                ' 공개키와 비밀키를 스스로 발급한다.

                ecKey = Nethereum.Signer.EthECKey.GenerateKey()

                public_key = ecKey.GetPublicAddress
                private_key = ecKey.GetPrivateKey
                password = json_webview("value")("password").ToString
                whether_key_file_generate = CBool(json_webview("value")("whether_key_file_generate"))
                idate_string = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.ffffff", Globalization.CultureInfo.InvariantCulture)

                ' 키파일을 만든다.

                If whether_key_file_generate = True Then

                    Call GRT.generate_key_file.exe_new(password, ecKey)

                End If

                ' pure_query 를 만든다.

                pure_query = GRT.GQS_insert_super_account_pure_query.exe(public_key, idate_string, json_webview)

                signiture = GRT.Security.Gsign.sign(pure_query, private_key)

                Dim result = wv.CoreWebView2.ExecuteScriptAsync(
                        $"$('#text_public_key').val('{public_key}');
                        $('#text_private_key').val('{private_key}');")

                json_receipt = Await GRT.mobile_authentication.submit_membership_info(json_webview, public_key, idate_string, signiture)

                ' 리턴 처리

                If CStr(json_receipt("success")) = "success" Then

                    Await wv.CoreWebView2.ExecuteScriptAsync(
                        $"$('.mobile_authentication').show();
                        $('#div_hidden_hash').html('{json_receipt("hash").ToString}');")

                Else

                    MessageBox.Show(CStr(json_receipt("value")("reason")))

                End If

            Case "submit_mobile_authentication_number"

                ' pure_query 를 만든다. 그런데 이 때 public_key 는 html 의 text_public_key 에서 찾는다.

                public_key = json_webview("value")("public_key").ToString
                private_key = json_webview("value")("private_key").ToString
                node = json_webview("value")("node").ToString
                hash = json_webview("value")("hash").ToString
                idate_string = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.ffffff", Globalization.CultureInfo.InvariantCulture)

                pure_query = GRT.GQS_insert_account_pure_query.exe(public_key, public_key, 0, {0}, node, idate_string)

                signiture = GRT.Security.Gsign.sign(pure_query, private_key)

                json_receipt = Await GRT.mobile_authentication.submit_mobile_authentication_number(json_webview, idate_string, signiture)

                ' 리턴 처리

                If CStr(json_receipt("success")) = "success" Then

                    'receipt = GRT.issue_receipt.exe(CStr(json_receipt("value")("receipt")("value")("signiture")))
                    receipt_string = GRT.issue_receipt.exe(json_receipt.Root.ToString)

                    receipt_string = Regex.Replace(receipt_string, "'", "\'")
                    receipt_string = Regex.Replace(receipt_string, vbCrLf, "<br />")

                    Dim scrollTop_code As String =
                        "$('html, body').animate({" &
                        "scrollTop: $('#div_public_key').offset().top" &
                        "}, 500)"

                    Dim result = wv.CoreWebView2.ExecuteScriptAsync(
                        $"$('.keys').show();
                        $('#div_explanation').html('{receipt_string}');
                        {scrollTop_code};")

                Else

                    MessageBox.Show(CStr(json_receipt("value")("reason")))

                End If

        End Select

    End Sub

End Class
