Imports Newtonsoft.Json
Imports Nethereum.Hex.HexTypes

Public Class multilevel_submit_sell
    Public Shared Async Sub exe(json_message As String)

        Dim ma As String
        Dim key As String
        Dim signiture, signiture_for_ethereum_transfer_cancel_clear, signiture_for_ethereum_transfer_cancel_unclear As String
        Dim JSS, JRS, receipt, pure_query, pure_query_for_ethereum_transfer_cancel_clear, pure_query_for_ethereum_transfer_cancel_unclear, closing_time_utc_string, closing_time_local_string As String
        Dim days_span As Integer
        Dim sell_order_block_number As Long
        Dim eoa_guarantee_seller, ethereum_transfer_eoa_from, ethereum_transfer_eoa_to,
            seller_na, seller_agency_domain, seller_agency_ip, buyer_na, buyer_agency_domain, buyer_agency_ip, idate_string As String
        Dim seller_agency_port, buyer_agency_port As Integer
        Dim date_now_utc, closing_time_utc, closing_time_local As DateTime
        Dim amount, exchange_rate, exchange_fee_rate As Decimal
        Dim gasPrice, gasPrice_for_cancel, gasLimit As Decimal
        Dim gas_price_biginteger As HexBigInteger
        Dim transfer_ethereum_from_exchange_case As Boolean

        Dim json_webview As Newtonsoft.Json.Linq.JObject = CType(JsonConvert.DeserializeObject(json_message), Linq.JObject)

        key = json_webview("key").ToString

        sell_order_block_number = CLng(json_webview("value")("block_number"))
        eoa_guarantee_seller = CStr(json_webview("value")("eoa").ToString.Trim)
        buyer_na = GRT.GR.account.agency.node
        buyer_agency_domain = GRT.GR.account.agency.domain_agency
        buyer_agency_ip = GRT.GR.account.agency.ip_agency
        buyer_agency_port = GRT.GR.account.agency.port_agency
        seller_na = CStr(json_webview("value")("na").ToString.Trim)
        seller_agency_domain = json_webview("value")("domain").ToString
        seller_agency_ip = CStr(json_webview("value")("ip").ToString.Trim)
        seller_agency_port = CInt(json_webview("value")("port").ToString.Trim)
        exchange_rate = CDec(json_webview("value")("exchange_rate").ToString.Trim)
        days_span = CInt(json_webview("value")("days_span").ToString.Trim)
        closing_time_local_string = CStr(json_webview("value")("closing_time_local").ToString.Trim)
        closing_time_local = CDate(closing_time_local_string)
        closing_time_utc_string = CStr(json_webview("value")("closing_time_utc").ToString.Trim)
        closing_time_utc = CDate(closing_time_utc_string)
        exchange_fee_rate = CDec(json_webview("value")("exchange_fee_rate").ToString.Trim)

        ethereum_transfer_eoa_from = buyer_na
        ethereum_transfer_eoa_to = GRT.GR.account.public_key
        ma = GRT.get_ma.exe(sell_order_block_number)
        idate_string = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.ffffff", Globalization.CultureInfo.InvariantCulture)

        amount = exchange_rate ' 일단. 물론 실제 쿼리에는 가스비 공제를 해 준다.

        gas_price_biginteger = Await GRT.GR.ethereum.web3.Eth.GasPrice.SendRequestAsync
        gasPrice = CDec(CLng(gas_price_biginteger.ToString) / 1000000000) ' 결국 Gwei 가 된다.
        gasLimit = 21000

        gasPrice_for_cancel = gasPrice * 112 / 100 ' 이것은 일단 필요는 있다. 캔슬이 실패할 때 쿼리 실행을 한다 할지라도 캔슬이 성공하는 경우를 미리 배제할 필요는 없다.

        date_now_utc = GRT.GetNistTime.exe

        If closing_time_utc >= date_now_utc Then
            ' 사실 이는 메인서버에서 해 주어야 할 일이다.
            ' 그러나 여기서 한 번은 해 준다.
            ' 왜냐하면 이더리움 지급이 발생하기 때문이다.
            ' 반드시 메인서버에서 추가를 해 준다.
            ' 그러나 싱크로를 생각하면 좀 더 깊이 생각을 해야 한다.

            pure_query = GRT.GQS_multilevel_recall.exe(sell_order_block_number, ethereum_transfer_eoa_to, eoa_guarantee_seller, buyer_na, amount, gasPrice, gasLimit, idate_string)

            pure_query_for_ethereum_transfer_cancel_clear = GRT.GQS_multilevel_recall.exe_cancel(True, sell_order_block_number, eoa_guarantee_seller, buyer_na, gasPrice_for_cancel, gasLimit, idate_string)
            pure_query_for_ethereum_transfer_cancel_unclear = GRT.GQS_multilevel_recall.exe_cancel(False, sell_order_block_number, eoa_guarantee_seller, buyer_na, gasPrice_for_cancel, gasLimit, idate_string)

            signiture = GRT.Security.Gsign.sign(pure_query, GRT.GR.account.private_key)

            signiture_for_ethereum_transfer_cancel_clear = GRT.Security.Gsign.sign(pure_query_for_ethereum_transfer_cancel_clear, GRT.GR.account.private_key)
            signiture_for_ethereum_transfer_cancel_unclear = GRT.Security.Gsign.sign(pure_query_for_ethereum_transfer_cancel_unclear, GRT.GR.account.private_key)

            transfer_ethereum_from_exchange_case = True

            JSS = GRT.make_json_string.exe(
                        {{"key", key, "quot"}},
                        {
                        {"block_hash", "initial", "quot"},
                        {"sell_order_block_number", CStr(sell_order_block_number), "non_quot"},
                        {"eoa_signer", GRT.GR.account.public_key, "quot"}, ' eoa_buyer 이다.
                        {"eoa_guarantee_seller", eoa_guarantee_seller, "quot"}, ' eoa_buyer 이다.
                        {"ethereum_transfer_eoa_from", ethereum_transfer_eoa_from, "quot"},
                        {"ethereum_transfer_eoa_to", ethereum_transfer_eoa_to, "quot"}, 'buyer_na 가 되어야 한다.          
                        {"buyer_na", buyer_na, "quot"},
                        {"seller_na", seller_na, "quot"},
                        {"ma", ma, "quot"},
                        {"seller_agency_domain", seller_agency_domain, "quot"},
                        {"seller_agency_ip", seller_agency_ip, "quot"},
                        {"seller_agency_port", CStr(seller_agency_port), "non_quot"},
                        {"ethereum_transaction_type", GRT.GR.account.ethereum_transaction_type, "quot"},
                        {"exchange_rate", CStr(exchange_rate), "non_quot"},
                        {"nonce_biginteger", CStr(-1), "non_quot"},
                        {"transfer_ethereum_from_exchange_case", CStr(CInt(transfer_ethereum_from_exchange_case)), "non_quot"},
                        {"amount", CStr(amount), "non_quot"},
                        {"gasPrice", CStr(gasPrice), "non_quot"},
                        {"gasPrice_for_cancel", CStr(gasPrice_for_cancel), "non_quot"},
                        {"gasLimit", CStr(gasLimit), "non_quot"},
                        {"days_span", CStr(days_span), "non_quot"},
                        {"closing_time_utc_string", closing_time_utc_string, "quot"},
                        {"closing_time_utc_string", closing_time_utc_string, "quot"},
                        {"exchange_fee_rate", CStr(exchange_fee_rate), "non_quot"},
                        {"pure_query", pure_query, "quot"},
                        {"pure_query_for_ethereum_transfer_cancel_clear", pure_query_for_ethereum_transfer_cancel_clear, "quot"},
                        {"pure_query_for_ethereum_transfer_cancel_unclear", pure_query_for_ethereum_transfer_cancel_unclear, "quot"},
                        {"signiture_data_for_ethereum_transfer", "", "quot"},
                        {"signiture_data_for_ethereum_transfer_cancel", "", "quot"},
                        {"signiture", signiture, "quot"},
                        {"signiture_for_ethereum_transfer_cancel_clear", signiture_for_ethereum_transfer_cancel_clear, "quot"},
                        {"signiture_for_ethereum_transfer_cancel_unclear", signiture_for_ethereum_transfer_cancel_unclear, "quot"},
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

            JRS = Await GRT.socket_client.exe(buyer_agency_ip, buyer_agency_port, GRT.GR.port_number_server_local, JSS)
            'JRS = Await GRT.socket_client.exe(GRT.GR.account.agency.domain_agency, GRT.GR.account.agency.port_agency, GRT.GR.port_number_server_local, JSS)

        Else ' 로딩하고 상당한 시간이 경과한 다음에 클릭한 경우이다.

            JRS = GRT.make_json_string.exe({{"key", key, "quot"}, {"success", "fail", "quot"}}, {{"reason", "취소할 수 있는 기간이 지났습니다.", "quot"}}, False)

        End If

        Dim json_receipt As Newtonsoft.Json.Linq.JObject = CType(JsonConvert.DeserializeObject(JRS), Linq.JObject)

        receipt = GRT.issue_receipt.exe(json_receipt.Root.ToString)

        receipt = Regex.Replace(receipt, "'", "\'")
        receipt = Regex.Replace(receipt, vbCrLf, "<br />")

        Call GR.control.wv_main.CoreWebView2.ExecuteScriptAsync(
                        $"$('#div_explanation').html('{receipt}');")

    End Sub

End Class
