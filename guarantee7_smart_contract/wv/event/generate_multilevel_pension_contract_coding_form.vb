Imports System.IO
Imports Newtonsoft.Json
Imports System.Diagnostics.Debug
Imports System.Windows.Forms
Imports Microsoft.Web.WebView2.Core

Public Class generate_multilevel_pension_contract_coding_form

    Shared directory_multilevel_pension_contract As String = Regex.Replace(Directory.GetCurrentDirectory, "guarantee7\\guarantee7.+", "guarantee7\guarantee7_smart_contract\wv\multilevel_pension_contract")

    Public Shared Sub exe()

        If Not GRT.GR.account.login_state = "no_login" Then

            GR.control.wv_working.CoreWebView2.Navigate(directory_multilevel_pension_contract & "\html\coding.html")

            RemoveHandler GR.control.wv_working.CoreWebView2.WebMessageReceived, AddressOf UpdateContent
            AddHandler GR.control.wv_working.CoreWebView2.WebMessageReceived, AddressOf UpdateContent

        Else

            MessageBox.Show("먼저 로그인을 하시기 바랍니다.")

            Return

        End If

    End Sub
    Shared Async Sub UpdateContent(ByVal sender As Object, ByVal args As CoreWebView2WebMessageReceivedEventArgs)

        Dim code, escaped_code, create_table_string, extention, file_name, code_json_string, smart_contract_name, industry_name As String
        Dim pure_query_for_signiture, pure_query_escaped, eoa, ca, signiture, idate_string As String
        Dim JSS, JRS As String
        Dim result As Task(Of String)

        Dim json_message As String = args.WebMessageAsJson()

        Dim json_webview As Newtonsoft.Json.Linq.JObject = CType(JsonConvert.DeserializeObject(json_message), Linq.JObject)

        Select Case json_webview("key").ToString

            Case "initialize_textarea_code_multilevel_pension_contract"

                ' html

                Dim file_info As New FileInfo(directory_multilevel_pension_contract & "\html\wallet_source.html")

                Dim fileContent = String.Empty
                Dim js_fileContent = String.Empty
                Dim css_fileContent = String.Empty
                Dim mysql_fileContent = String.Empty
                Dim fileStream As Stream

                fileStream = CType(file_info.OpenRead, Stream)

                Using StreamReader = New StreamReader(fileStream)

                    fileContent = StreamReader.ReadToEnd()

                End Using

                ' js

                Dim directory_info As New DirectoryInfo(directory_multilevel_pension_contract & "\js\wallet")

                For Each js_file_info In directory_info.GetFiles

                    fileStream = CType(js_file_info.OpenRead, Stream)

                    Using StreamReader = New StreamReader(fileStream)

                        js_fileContent &= StreamReader.ReadToEnd() & vbCrLf & vbCrLf

                    End Using

                Next

                fileContent = Regex.Replace(fileContent, "//script_to_be_replaced", js_fileContent)

                ' css

                directory_info = New DirectoryInfo(directory_multilevel_pension_contract & "\css\wallet")

                For Each css_file_info In directory_info.GetFiles

                    fileStream = CType(css_file_info.OpenRead, Stream)

                    Using StreamReader = New StreamReader(fileStream)

                        css_fileContent &= StreamReader.ReadToEnd() & vbCrLf & vbCrLf

                    End Using

                Next

                fileContent = Regex.Replace(fileContent, "/\*style_sheet_to_be_replaced\*/", css_fileContent)

                fileContent = Regex.Replace(fileContent, "'", "\'")
                fileContent = Regex.Replace(fileContent, vbCrLf, "\n")

                file_info = New FileInfo(directory_multilevel_pension_contract & "\etc\mysql_query.txt")

                fileStream = CType(file_info.OpenRead, Stream)

                Using StreamReader = New StreamReader(fileStream)

                    mysql_fileContent = StreamReader.ReadToEnd()

                End Using

                mysql_fileContent = Regex.Replace(mysql_fileContent, "'", "\'")
                mysql_fileContent = Regex.Replace(mysql_fileContent, vbCrLf, "\n")

                result = GR.control.wv_working.CoreWebView2.ExecuteScriptAsync(
                        $"$('#textarea_code').val('{fileContent}');
                        $('#textarea_create_table_string').val('{mysql_fileContent}');")

            Case "submit_wallet_source"

                code = json_webview("value")("code").ToString

                code = Regex.Replace(code, "\n", "")

                GR.control.wv_test_launch.NavigateToString(code)

            Case "submit_deploy"

                eoa = GRT.GR.account.public_key
                ca = Nethereum.Signer.EthECKey.GenerateKey().GetPublicAddress
                industry_name = json_webview("value")("industry_name").ToString
                smart_contract_name = json_webview("value")("smart_contract_name").ToString
                code = json_webview("value")("code").ToString
                create_table_string = json_webview("value")("create_table_string").ToString
                escaped_code = Regex.Replace(code, """", "_quot_double_")
                escaped_code = Regex.Replace(escaped_code, "'", "_quot_single_")
                extention = json_webview("value")("extention").ToString
                file_name = json_webview("value")("file_name").ToString

                code_json_string = GRT.make_json_string.exe(
                                                            {{"key", "code_json_string", "quot"}},
                                                            {
                                                            {"file_name", file_name, "quot"},
                                                            {"escaped_code", escaped_code, "quot"},
                                                            {"extention", extention, "quot"},
                                                            {"create_table_string", create_table_string, "quot"}
                                                            }, False)

                'Dim json_receipt2 As Newtonsoft.Json.Linq.JObject = CType(JsonConvert.DeserializeObject(code_json_string), Linq.JObject)

                idate_string = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.ffffff", Globalization.CultureInfo.InvariantCulture)

                pure_query_escaped = GRT.GQS_deploy_smart_contract_escaped.exe(
                    eoa, ca, industry_name, smart_contract_name, code_json_string, create_table_string, idate_string)

                pure_query_for_signiture = Regex.Replace(pure_query_escaped, "_quot_double_", "\""")
                pure_query_for_signiture = Regex.Replace(pure_query_for_signiture, "_quot_single_", "\'")

                signiture = GRT.Security.Gsign.sign(pure_query_for_signiture, GRT.GR.account.private_key)

                Dim header(,) As String = {{"key", "deploy_smart_contract", "quot"}}

                Dim value(,) As String =
                    {{"block_hash", "initial", "quot"},
                    {"eoa", eoa, "quot"},
                    {"ca", ca, "quot"},
                    {"industry_name", industry_name, "quot"},
                    {"smart_contract_name", smart_contract_name, "quot"},
                    {"code_json_string", "code_json_string_to_be_replaced", "quot"},
                    {"idate_string", idate_string, "quot"},
                    {"signiture", signiture, "quot"}}

                JSS = GRT.make_json_string.exe(header, value, False)

                'Dim json_receipt1 As Newtonsoft.Json.Linq.JObject = CType(JsonConvert.DeserializeObject(JSS), Linq.JObject)

                JSS = Regex.Replace(JSS, """code_json_string_to_be_replaced""", code_json_string)

                'Dim json_receipt3 As Newtonsoft.Json.Linq.JObject = CType(JsonConvert.DeserializeObject(JSS), Linq.JObject)

                JRS = Await Task.Run(Function() GRT.socket_client.exe(GRT.GR.main_server_address_agency, GRT.GR.port_number_server_main_agency, GRT.GR.port_number_server_local, JSS))

                Dim json_receipt As Newtonsoft.Json.Linq.JObject = CType(JsonConvert.DeserializeObject(JRS), Linq.JObject)

                Dim receipt As String

                If CStr(json_receipt("success")) = "success" Then

                    'receipt = GRT.issue_receipt.exe(CStr(json_receipt("value")("receipt")("value")("signiture")))
                    receipt = GRT.issue_receipt.exe(json_receipt.Root.ToString)

                    receipt = Regex.Replace(receipt, "'", "\'")
                    receipt = Regex.Replace(receipt, vbCrLf, "<br />")

                    Dim scrollTop_code As String =
                        "$('html, body').animate({" &
                        "scrollTop: $('#text_ca').offset().top" &
                        "}, 500)"

                    result = GR.control.wv_working.CoreWebView2.ExecuteScriptAsync(
                        $"$('.keys').show();
                        $('#div_explanation').html('{receipt}');
                        $('#text_ca').val('{ca}');
                        {scrollTop_code};")

                Else
                    MessageBox.Show(CStr(json_receipt("value")("reason")))
                End If

        End Select

        RemoveHandler GR.control.wv_working.CoreWebView2.WebMessageReceived, AddressOf UpdateContent
        AddHandler GR.control.wv_working.CoreWebView2.WebMessageReceived, AddressOf UpdateContent

    End Sub

End Class
