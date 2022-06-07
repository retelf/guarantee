Imports Newtonsoft.Json
Imports Nethereum.Hex.HexTypes

Public Class multilevel_submit_buy
    Public Shared Async Sub exe(json_message As String)

        Dim ma, private_key, password As String
        Dim whether_key_file_generate As Boolean
        Dim ecKey As Nethereum.Signer.EthECKey
        Dim key As String
        Dim signiture, signiture_for_ethereum_transfer_cancel, signiture_for_get_balance, signiture_data_for_ethereum_transfer() As String
        Dim JSS, JRS, receipt, pure_query, pure_query_for_ethereum_transfer_cancel, closing_time_utc_string As String
        Dim days_span As Integer
        Dim sell_order_block_number As Long
        Dim case_retored_eoa_from, eoa_guarantee_seller, ethereum_transfer_eoa_from, ethereum_transfer_eoa_to,
            seller_na, buyer_na, seller_agency_domain, seller_agency_ip, idate_string As String
        Dim seller_agency_port As Integer
        Dim date_now_utc, closing_time_utc As DateTime
        Dim amount, exchange_rate, exchange_fee_rate As Decimal
        Dim gasPrice, gasPrice_for_cancel, gasLimit As Decimal
        Dim gas_price_biginteger As HexBigInteger

        Dim json_webview As Newtonsoft.Json.Linq.JObject = CType(JsonConvert.DeserializeObject(json_message), Linq.JObject)

        key = json_webview("key").ToString

        sell_order_block_number = CLng(json_webview("value")("block_number"))
        eoa_guarantee_seller = CStr(json_webview("value")("eoa").ToString.Trim)
        seller_na = CStr(json_webview("value")("na").ToString.Trim)
        buyer_na = GRT.GR.account.agency.node
        seller_agency_domain = json_webview("value")("domain").ToString ' 리스트로부터 온 것이므로 agency 가 맞다.
        seller_agency_ip = CStr(json_webview("value")("ip").ToString.Trim)
        seller_agency_port = CInt(json_webview("value")("port").ToString.Trim)
        exchange_rate = CDec(json_webview("value")("exchange_rate").ToString.Trim)
        days_span = CInt(json_webview("value")("days_span").ToString.Trim)
        exchange_fee_rate = CDec(json_webview("value")("exchange_fee_rate").ToString.Trim)

        ' 공개키와 비밀키를 스스로 발급한다.

        ecKey = Nethereum.Signer.EthECKey.GenerateKey()

        ma = ecKey.GetPublicAddress
        private_key = ecKey.GetPrivateKey
        password = json_webview("value")("password").ToString
        whether_key_file_generate = CBool(json_webview("value")("whether_key_file_generate"))
        idate_string = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.ffffff", Globalization.CultureInfo.InvariantCulture)

        ' 키파일을 만든다.

        If whether_key_file_generate = True Then

            Call GRT.generate_key_file.exe_new(password, ecKey)

        End If

        ethereum_transfer_eoa_from = GRT.GR.account.public_key
        ethereum_transfer_eoa_to = buyer_na

        amount = exchange_rate

        gas_price_biginteger = Await GRT.GR.ethereum.web3.Eth.GasPrice.SendRequestAsync
        gasPrice = CDec(CLng(gas_price_biginteger.ToString) / 1000000000) ' 결국 Gwei 가 된다.
        gasLimit = 21000

        gasPrice_for_cancel = gasPrice * 112 / 100

        date_now_utc = GRT.GetNistTime.exe

        closing_time_utc = date_now_utc.AddDays(days_span)

        closing_time_utc_string = closing_time_utc.ToString("yyyy/MM/dd HH:mm:ss")

        Dim unused = Await GRT.get_signiture_data_for_ethereum_transfer.exe("wallet", key, ethereum_transfer_eoa_from, GRT.GR.account.private_key, ethereum_transfer_eoa_to, amount, 0, ' recall 시에는 가스비를 공제한 전액을 돌려주고 confirm 시에는 수수료를 공제하되 가스비는 na 가 부담한다.
                                                                            CType(gasPrice * 1000000000, Numerics.BigInteger), CType(gasLimit, Numerics.BigInteger))

        signiture_data_for_ethereum_transfer = GRT.get_signiture_data_for_ethereum_transfer.info.signiture_data_for_ethereum_transfer

        pure_query = GRT.GQS_multilevel_buy.exe(sell_order_block_number, ethereum_transfer_eoa_from, ma, seller_na, buyer_na, amount, gasPrice, gasLimit, closing_time_utc_string, idate_string)

        pure_query_for_ethereum_transfer_cancel = GRT.GQS_ethereum_transfer_cancel_treatment.exe(ethereum_transfer_eoa_from, gasPrice_for_cancel, gasLimit, idate_string)

        signiture_for_get_balance = GRT.Security.Gsign.sign("foo", GRT.GR.account.private_key)

        signiture = GRT.Security.Gsign.sign(pure_query, GRT.GR.account.private_key)

        signiture_for_ethereum_transfer_cancel = GRT.Security.Gsign.sign(pure_query_for_ethereum_transfer_cancel, GRT.GR.account.private_key)

        case_retored_eoa_from = GRT.restore_ethereum_public_key.exe(ethereum_transfer_eoa_from, GRT.GR.account.private_key)

        JSS = GRT.make_json_string.exe(
                    {{"key", key, "quot"}},
                    {
                    {"case_retored_eoa_from", case_retored_eoa_from, "quot"},
                    {"block_hash", "initial", "quot"},
                    {"sell_order_block_number", CStr(sell_order_block_number), "non_quot"},
                    {"eoa_signer", GRT.GR.account.public_key, "quot"}, ' eoa_buyer 이다.
                    {"eoa_guarantee_seller", eoa_guarantee_seller, "quot"}, ' eoa_buyer 이다.
                    {"ethereum_transfer_eoa_from", ethereum_transfer_eoa_from, "quot"},
                    {"ethereum_transfer_eoa_to", ethereum_transfer_eoa_to, "quot"}, 'buyer_na 가 되어야 한다.
                    {"seller_na", seller_na, "quot"},
                    {"buyer_na", buyer_na, "quot"},
                    {"ma", ma, "quot"},
                    {"seller_agency_domain", seller_agency_domain, "quot"},
                    {"seller_agency_ip", seller_agency_ip, "quot"},
                    {"seller_agency_port", CStr(seller_agency_port), "non_quot"},
                    {"ethereum_transaction_type", GRT.GR.account.ethereum_transaction_type, "quot"},
                    {"exchange_rate", CStr(exchange_rate), "non_quot"},
                    {"nonce_biginteger", GRT.get_signiture_data_for_ethereum_transfer.info.nonce.Value.ToString, "non_quot"},
                    {"transfer_ethereum_from_exchange_case", CStr(0), "non_quot"},
                    {"amount", CStr(amount), "non_quot"},
                    {"gasPrice", CStr(gasPrice), "non_quot"},
                    {"gasPrice_for_cancel", CStr(gasPrice_for_cancel), "non_quot"},
                    {"gasLimit", CStr(gasLimit), "non_quot"},
                    {"days_span", CStr(days_span), "non_quot"},
                    {"closing_time_utc_string", closing_time_utc_string, "quot"},
                    {"exchange_fee_rate", CStr(GRT.GR.multilevel_exchange_fee_rate), "non_quot"},
                    {"pure_query", pure_query, "quot"},
                    {"pure_query_for_ethereum_transfer_cancel", pure_query_for_ethereum_transfer_cancel, "quot"},
                    {"signiture_for_get_balance", "", "quot"},
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
                    {"receipt_cancel", "", "quot"}}, True)

        JRS = Await GRT.socket_client.exe(seller_agency_ip, seller_agency_port, GRT.GR.port_number_server_local, JSS)
        'JRS = Await GRT.socket_client.exe(GRT.GR.account.agency.domain_agency, GRT.GR.account.agency.port_agency, GRT.GR.port_number_server_local, JSS)

        Dim json_receipt As Newtonsoft.Json.Linq.JObject = CType(JsonConvert.DeserializeObject(JRS), Linq.JObject)

        receipt = GRT.issue_receipt.exe(json_receipt.Root.ToString)

        receipt = Regex.Replace(receipt, "'", "\'")
        receipt = Regex.Replace(receipt, vbCrLf, "<br />")

        Call GR.control.wv_main.CoreWebView2.ExecuteScriptAsync(
                    $"$('#div_explanation').html('{receipt}');")

        Call GR.control.wv_sub.CoreWebView2.ExecuteScriptAsync(
                    $"$('#text_public_key').val('{ma}');
                    $('#text_private_key').val('{private_key}');
                    $('.keys').show();")

    End Sub

End Class
