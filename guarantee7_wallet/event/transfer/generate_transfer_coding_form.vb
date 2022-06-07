Imports System.IO
Imports Newtonsoft.Json
Imports Microsoft.Web.WebView2.Core
Imports Nethereum.Hex.HexTypes
Imports System.Globalization

Public Class generate_transfer_coding_form

    Shared html_directory As String = Regex.Replace(Directory.GetCurrentDirectory, "guarantee7\\guarantee7.+", "guarantee7\guarantee7_wallet\wv\transfer\html")

    Public Shared Sub exe()

        If Not GRT.GR.account.login_state = "no_login" Then

            GR.control.wv_main.CoreWebView2.Navigate(html_directory & "\default.html")

            RemoveHandler GR.control.wv_main.CoreWebView2.WebMessageReceived, AddressOf UpdateContent
            AddHandler GR.control.wv_main.CoreWebView2.WebMessageReceived, AddressOf UpdateContent

        Else

            MessageBox.Show("먼저 로그인을 하시기 바랍니다.")

            Return

        End If

    End Sub
    Shared Async Sub UpdateContent(ByVal sender As Object, ByVal args As CoreWebView2WebMessageReceivedEventArgs)

        Dim key, coin_name As String
        Dim signiture, signiture_for_ethereum_transfer_cancel, signiture_for_get_balance, signiture_data_for_ethereum_transfer() As String
        Dim JSS, JRS, receipt, pure_query, pure_query_for_ethereum_transfer_cancel As String
        Dim eoa_from, case_retored_eoa_from, eoa_to, unit, idate_string As String
        Dim balance, amount, gasPrice, gasPrice_for_cancel, gasLimit As Decimal
        Dim recommanded_gas_price As HexBigInteger
        Dim transfer_ethereum_from_exchange_case As Boolean ' 거래소 계좌로부터 이더리움이 인출되는 경우인가
        Dim result As Task(Of String)

        Dim json_message As String = args.WebMessageAsJson()

        Dim json_webview As Newtonsoft.Json.Linq.JObject = CType(JsonConvert.DeserializeObject(json_message), Linq.JObject)

        key = json_webview("key").ToString

        coin_name = GRT.GR.account.login_state

        Select Case key

            Case "initialize_transfer_form"

                signiture_for_get_balance = GRT.Security.Gsign.sign("foo", GRT.GR.account.private_key)

                Dim json_receipt As Newtonsoft.Json.Linq.JObject = Await GRT.get_balance_via_socket.exe(coin_name, GRT.GR.account.public_key, signiture_for_get_balance)

                result = GR.control.wv_main.CoreWebView2.ExecuteScriptAsync($"$('#div_coin_name').html('{GRT.GR.account.login_state}');")

                If CStr(json_receipt("success")) = "success" Then

                    balance = CDec(json_receipt("value")("balance"))

                    result = GR.control.wv_main.CoreWebView2.ExecuteScriptAsync(
                        $"$('#div_coin_name').html('{coin_name}');
                        $('#div_balance').html({balance});
                        $('#div_from').html('{GRT.GR.account.public_key}');")

                Else

                    result = GR.control.wv_main.CoreWebView2.ExecuteScriptAsync(
                        $"$('#div_coin_name').html('{coin_name}');
                        $('#div_explanation').html('{CStr(json_receipt("value")("reason"))}');")

                End If

                unit = "Unit : " & coin_name

                If coin_name = "guarantee" Then

                    result = GR.control.wv_main.CoreWebView2.ExecuteScriptAsync(
                        $"$('#div_gasPrice_outer').hide();$('#label_amount_unit').html('{unit}');
                        $('#select_self_coin_name>option:eq(0)').prop('selected', true);")

                Else

                    recommanded_gas_price = Await GRT.GR.ethereum.web3.Eth.GasPrice.SendRequestAsync

                    gasPrice = GRT.convert_hexbiginteger_decimal_unit_ethereum.exe(recommanded_gas_price)

                    result = GR.control.wv_main.CoreWebView2.ExecuteScriptAsync(
                        $"$('#div_gasPrice_outer').show();$('#label_amount_unit').html('{unit}');
                        $('#text_gasPrice').val({gasPrice * 1000000000});
                        $('#select_self_coin_name>option:eq(1)').prop('selected', true);")

                End If

            Case "submit_transfer"

                eoa_from = json_webview("value")("eoa_from").ToString
                eoa_to = json_webview("value")("eoa_to").ToString
                balance = CDec(json_webview("value")("balance").ToString.Trim)
                amount = CDec(json_webview("value")("amount").ToString.Trim)

                idate_string = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.ffffff", Globalization.CultureInfo.InvariantCulture)

                If coin_name = "guarantee" Then

                    gasPrice = 0
                    gasLimit = 0

                    pure_query = GRT.GQS_clear_deposit.exe(coin_name, eoa_from, eoa_to, amount, gasPrice, gasLimit, idate_string)

                    signiture_for_get_balance = GRT.Security.Gsign.sign("foo", GRT.GR.account.private_key)

                    signiture = GRT.Security.Gsign.sign(pure_query, GRT.GR.account.private_key)

                    JSS = GRT.make_json_string.exe(
                        {{"key", "submit_transfer", "quot"}},
                        {
                        {"block_hash", "initial", "quot"},
                        {"eoa_from", eoa_from, "quot"},
                        {"eoa_to", eoa_to, "quot"},
                        {"coin_name", coin_name, "quot"},
                        {"na", GRT.GR.account.agency.node, "quot"},
                        {"exchange_name", GRT.GR.account.agency.exchange_name, "quot"},
                        {"nonce_biginteger", CStr(-1), "non_quot"},
                        {"amount", CStr(amount), "non_quot"},
                        {"gasPrice", CStr(gasPrice), "non_quot"},
                        {"gasPrice_for_cancel", CStr(0), "non_quot"},
                        {"gasLimit", CStr(gasLimit), "non_quot"},
                        {"transfer_ethereum_from_exchange_case", CStr(0), "non_quot"},
                        {"pure_query", pure_query, "quot"},
                        {"signiture_for_get_balance", signiture_for_get_balance, "quot"},
                        {"signiture", signiture, "quot"},
                        {"signiture_for_ethereum_transfer_cancel", "", "quot"},
                        {"initial_transfer", "Y", "quot"},
                        {"idate_string", idate_string, "quot"}}, False)

                Else

                    gasPrice = CDec(json_webview("value")("gasPrice").ToString.Trim) ' 기위로 온다.
                    gasLimit = 21000

                    gasPrice_for_cancel = gasPrice * 112 / 100

                    Select Case GRT.GR.account.ethereum_transaction_type

                        Case "signiture"

                            Dim unused = Await GRT.get_signiture_data_for_ethereum_transfer.exe("wallet", key, eoa_from, GRT.GR.account.private_key, eoa_to, amount, 0, CType(gasPrice * 1000000000, Numerics.BigInteger), CType(gasLimit, Numerics.BigInteger))

                            signiture_data_for_ethereum_transfer = GRT.get_signiture_data_for_ethereum_transfer.info.signiture_data_for_ethereum_transfer

                            pure_query = GRT.GQS_clear_deposit.exe(coin_name, eoa_from, eoa_to, amount, gasPrice, gasLimit, idate_string)

                            pure_query_for_ethereum_transfer_cancel = GRT.GQS_ethereum_transfer_cancel_treatment.exe(eoa_from, gasPrice_for_cancel, gasLimit, idate_string)

                            signiture_for_get_balance = GRT.Security.Gsign.sign("foo", GRT.GR.account.private_key)

                            signiture = GRT.Security.Gsign.sign(pure_query, GRT.GR.account.private_key)

                            signiture_for_ethereum_transfer_cancel = GRT.Security.Gsign.sign(pure_query_for_ethereum_transfer_cancel, GRT.GR.account.private_key)

                            case_retored_eoa_from = GRT.restore_ethereum_public_key.exe(eoa_from, GRT.GR.account.private_key)

                            transfer_ethereum_from_exchange_case = False

                            JSS = GRT.make_json_string.exe(
                                {{"key", "submit_transfer", "quot"}},
                                {
                                {"case_retored_eoa_from", case_retored_eoa_from, "quot"},
                                {"block_hash", "initial", "quot"},
                                {"eoa_from", eoa_from, "quot"},
                                {"eoa_to", eoa_to, "quot"},
                                {"coin_name", coin_name, "quot"},
                                {"na", GRT.GR.account.agency.node, "quot"},
                                {"exchange_name", GRT.GR.account.agency.exchange_name, "quot"},
                                {"ethereum_transaction_type", GRT.GR.account.ethereum_transaction_type, "quot"},
                                {"nonce_biginteger", CStr(GRT.get_signiture_data_for_ethereum_transfer.info.nonce.Value.ToString), "non_quot"},
                                {"amount", CStr(amount), "non_quot"},
                                {"gasPrice", CStr(gasPrice), "non_quot"},
                                {"gasPrice_for_cancel", CStr(gasPrice_for_cancel), "non_quot"},
                                {"gasLimit", CStr(gasLimit), "non_quot"},
                                {"transfer_ethereum_from_exchange_case", CStr(CInt(transfer_ethereum_from_exchange_case)), "non_quot"},
                                {"pure_query", pure_query, "quot"},
                                {"pure_query_for_ethereum_transfer_cancel", pure_query_for_ethereum_transfer_cancel, "quot"},
                                {"signiture_for_get_balance", signiture_for_get_balance, "quot"},
                                {"signiture_data_for_ethereum_transfer", signiture_data_for_ethereum_transfer(0), "quot"},
                                {"signiture_data_for_ethereum_transfer_cancel", signiture_data_for_ethereum_transfer(1), "quot"},
                                {"signiture", signiture, "quot"},
                                {"signiture_for_ethereum_transfer_cancel", signiture_for_ethereum_transfer_cancel, "quot"},
                                {"initial_transfer", "Y", "quot"},
                                {"idate_string", idate_string, "quot"},
                                {"ethereum_transaction_result", "", "quot"},
                                {"transaction_hash_initial", "", "quot"},
                                {"transaction_hash_cancel", "", "quot"},
                                {"tem_transaction_success_initial", CStr(0), "non_quot"},
                                {"tem_transaction_success_cancel", CStr(0), "non_quot"},
                                {"usedGas", CStr(0), "non_quot"},
                                {"receipt_initial", "", "quot"},
                                {"receipt_cancel", "", "quot"}}, False)

                        Case "password" ' 원격 서버에 키파일이 저장되어 있는 경우만 가능.

                            pure_query = GRT.GQS_clear_deposit.exe(coin_name, eoa_from, eoa_to, amount, gasPrice, gasLimit, idate_string)

                            signiture_for_get_balance = GRT.Security.Gsign.sign("foo", GRT.GR.account.private_key)

                            signiture = GRT.Security.Gsign.sign(pure_query, GRT.GR.account.private_key)

                            JSS = GRT.make_json_string.exe(
                                {{"key", "submit_transfer", "quot"}},
                                {
                                {"block_hash", "initial", "quot"},
                                {"eoa_from", eoa_from, "quot"},
                                {"eoa_to", eoa_to, "quot"},
                                {"coin_name", coin_name, "quot"},
                                {"ethereum_transaction_type", GRT.GR.account.ethereum_transaction_type, "quot"},
                                {"amount", CStr(amount), "non_quot"},
                                {"gasPrice", CStr(gasPrice), "non_quot"},
                                {"gasLimit", CStr(gasLimit), "non_quot"},
                                {"pure_query", pure_query, "quot"},
                                {"signiture_for_get_balance", signiture_for_get_balance, "quot"},
                                {"password", GRT.GR.account.password, "quot"},
                                {"signiture", signiture, "quot"},
                                {"initial_transfer", "Y", "quot"},
                                {"idate_string", idate_string, "quot"},
                                {"ethereum_transaction_result", "", "quot"},
                                {"transaction_hash_initial", "", "quot"},
                                {"transaction_hash_cancel", "", "quot"},
                                {"usedGas", CStr(0), "non_quot"},
                                {"receipt_initial", "", "quot"},
                                {"receipt_cancel", "", "quot"}}, False)

                        Case "private_key"

                            pure_query = GRT.GQS_clear_deposit.exe(coin_name, eoa_from, eoa_to, amount, gasPrice, gasLimit, idate_string)

                            signiture_for_get_balance = GRT.Security.Gsign.sign("foo", GRT.GR.account.private_key)

                            signiture = GRT.Security.Gsign.sign(pure_query, GRT.GR.account.private_key)

                            JSS = GRT.make_json_string.exe(
                                {{"key", "submit_transfer", "quot"}},
                                {
                                {"block_hash", "initial", "quot"},
                                {"eoa_from", eoa_from, "quot"},
                                {"eoa_to", eoa_to, "quot"},
                                {"coin_name", coin_name, "quot"},
                                {"ethereum_transaction_type", GRT.GR.account.ethereum_transaction_type, "quot"},
                                {"amount", CStr(amount), "non_quot"},
                                {"gasPrice", CStr(gasPrice), "non_quot"},
                                {"gasLimit", CStr(gasLimit), "non_quot"},
                                {"pure_query", pure_query, "quot"},
                                {"signiture_for_get_balance", signiture_for_get_balance, "quot"},
                                {"private_key", GRT.GR.account.private_key, "quot"},
                                {"signiture", signiture, "quot"},
                                {"initial_transfer", "Y", "quot"},
                                {"idate_string", idate_string, "quot"},
                                {"ethereum_transaction_result", "", "quot"},
                                {"transaction_hash_initial", "", "quot"},
                                {"transaction_hash_cancel", "", "quot"},
                                {"usedGas", CStr(0), "non_quot"},
                                {"receipt_initial", "", "quot"},
                                {"receipt_cancel", "", "quot"}}, False)

                    End Select

                End If

                JRS = Await GRT.socket_client.exe(GRT.GR.account.agency.ip_agency, GRT.GR.account.agency.port_agency, GRT.GR.port_number_server_local, JSS)

                Dim json_receipt As Newtonsoft.Json.Linq.JObject = CType(JsonConvert.DeserializeObject(JRS), Linq.JObject)

                receipt = GRT.issue_receipt.exe(json_receipt.Root.ToString)

                receipt = Regex.Replace(receipt, "'", "\'")
                receipt = Regex.Replace(receipt, vbCrLf, "<br />")

                result = GR.control.wv_main.CoreWebView2.ExecuteScriptAsync(
                    $"$('#div_explanation').html('{receipt}');")

        End Select

    End Sub

End Class
