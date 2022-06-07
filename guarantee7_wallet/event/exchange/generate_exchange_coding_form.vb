Imports System.IO
Imports Newtonsoft.Json
Imports Microsoft.Web.WebView2.Core
Imports Nethereum.Hex.HexTypes
Imports Newtonsoft.Json.Linq
Imports System.Numerics

Public Class generate_exchange_coding_form

    Shared html_directory As String = Regex.Replace(Directory.GetCurrentDirectory, "guarantee7\\guarantee7.+", "guarantee7\guarantee7_wallet\wv\exchange\html")

    Public Shared Async Sub exe()

        If Not GRT.GR.account.login_state = "no_login" Then

            GR.control.wv_main.CoreWebView2.Navigate(html_directory & "\default.html")
            GR.control.wv_sub.CoreWebView2.Navigate(html_directory & "\board_orders.html")

            RemoveHandler GR.control.wv_main.CoreWebView2.WebMessageReceived, AddressOf UpdateContent
            AddHandler GR.control.wv_main.CoreWebView2.WebMessageReceived, AddressOf UpdateContent

            RemoveHandler GR.control.wv_sub.CoreWebView2.WebMessageReceived, AddressOf UpdateContent
            AddHandler GR.control.wv_sub.CoreWebView2.WebMessageReceived, AddressOf UpdateContent

        Else

            MessageBox.Show("먼저 로그인을 하시기 바랍니다.")

            Return

        End If

    End Sub

    Shared initialized As Boolean = False
    Shared Async Sub UpdateContent(ByVal sender As Object, ByVal args As CoreWebView2WebMessageReceivedEventArgs)

        Dim key, coin_name As String

        Dim exchange_block_number, cancel_block_number As Long
        Dim eoa, eoa_seller, eoa_buyer, case_retored_eoa_from, na, coin_name_from, coin_name_to, clickers_coin_name_to_buy, clickers_coin_name_to_sell, state As String
        Dim pure_query, pure_query_for_ethereum_transfer_cancel, signiture, signiture_for_ethereum_transfer_cancel, signiture_for_get_balance, signiture_data_for_ethereum_transfer() As String
        Dim JSS, JRS, result As String
        Dim seller_agency_domain, seller_agency_ip As String
        Dim seller_agency_port As Integer
        Dim amount, exchange_rate, exchange_fee_rate, gasPrice, gasPrice_for_cancel, gasLimit As Decimal
        Dim gas_price_biginteger As HexBigInteger
        Dim idate_string = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.ffffff", Globalization.CultureInfo.InvariantCulture)
        Dim json_webview As Newtonsoft.Json.Linq.JObject
        Dim nonce_biginteger As BigInteger
        Dim transfer_ethereum_from_exchange_case As Boolean

        Dim json_message As String = args.WebMessageAsJson()

        json_webview = CType(JsonConvert.DeserializeObject(json_message), Linq.JObject)

        key = json_webview("key").ToString

        coin_name = GRT.GR.account.login_state

        Select Case key

            Case "initialize_exchange_form"

                If Not initialized Then

                    initialized = True

                    signiture_for_get_balance = GRT.Security.Gsign.sign("foo", GRT.GR.account.private_key)

                    Dim json_receipt As Newtonsoft.Json.Linq.JObject = Await GRT.get_balance_via_socket.exe(coin_name, GRT.GR.account.public_key, signiture_for_get_balance)

                    result = Await GR.control.wv_main.CoreWebView2.ExecuteScriptAsync($"$('#div_coin_name').html('{GRT.GR.account.login_state}');")

                    If CStr(json_receipt("success")) = "success" Then

                        GRT.GR.account.balance = CDec(json_receipt("value")("balance"))

                        result = Await GR.control.wv_main.CoreWebView2.ExecuteScriptAsync(
                            $"$('#div_coin_name').html('{coin_name}');
                        $('#div_balance').html({GRT.GR.account.balance});
                        $('#div_from').html('{GRT.GR.account.public_key}');")

                    Else

                        result = Await GR.control.wv_main.CoreWebView2.ExecuteScriptAsync(
                            $"$('#div_coin_name').html('{coin_name}');
                        $('#div_explanation').html('{CStr(json_receipt("value")("reason"))}');")

                    End If

                    Dim i_number_select_self_coin_name, i_number_select_counterpart_coin_name As Integer

                    If coin_name = "guarantee" Then
                        i_number_select_self_coin_name = 0
                        i_number_select_counterpart_coin_name = 1
                    Else
                        i_number_select_self_coin_name = 1
                        i_number_select_counterpart_coin_name = 0
                    End If

                    result = Await GR.control.wv_main.CoreWebView2.ExecuteScriptAsync(
                                    $"GR.login_state='{GRT.GR.account.login_state}';
                                GR.balance='{GRT.GR.account.balance}';
                                GR.eoa='{GRT.GR.account.public_key}';
                                GR.exchange_initializing();
                                $('#select_self_coin_name>option:eq({i_number_select_self_coin_name})').prop('selected', true);
                                $('#select_counterpart_coin_name>option:eq({i_number_select_counterpart_coin_name})').prop('selected', true);")

                    gas_price_biginteger = Await GRT.GR.ethereum.web3.Eth.GasPrice.SendRequestAsync

                    gasPrice = GRT.convert_hexbiginteger_decimal_unit_ethereum.exe(gas_price_biginteger)

                    Await GR.control.wv_main.CoreWebView2.ExecuteScriptAsync($"$('#text_gasPrice').val({gasPrice * 1000000000});")

                    Call display_board_exchange(json_webview)

                End If

            Case "display_board_exchange" ' 전광판

                Call display_board_exchange(json_webview)

            Case "submit_load_order"

                na = GRT.GR.account.agency.node
                seller_agency_domain = GRT.GR.account.agency.domain_agency
                seller_agency_ip = GRT.GR.account.agency.ip_agency
                seller_agency_port = GRT.GR.account.agency.port_agency

                eoa = json_webview("value")("eoa").ToString
                coin_name_from = json_webview("value")("coin_name_from").ToString
                coin_name_to = json_webview("value")("coin_name_to").ToString
                amount = CDec(json_webview("value")("amount"))
                exchange_rate = CDec(json_webview("value")("exchange_rate"))
                exchange_fee_rate = GRT.GR.exchange_fee_rate
                state = "alive"

                If coin_name_from = "guarantee" Then

                    gasPrice = 0
                    gasLimit = 0

                    pure_query = GRT.GQS_load_order.exe(eoa, na, seller_agency_domain, seller_agency_ip, seller_agency_port, coin_name_from, coin_name_to, amount, exchange_rate, GRT.GR.exchange_fee_rate, gasPrice, gasLimit, idate_string)

                    signiture_for_get_balance = GRT.Security.Gsign.sign("foo", GRT.GR.account.private_key)

                    signiture = GRT.Security.Gsign.sign(pure_query, GRT.GR.account.private_key)

                    JSS = GRT.make_json_string.exe(
                            {{"key", "submit_load_order", "quot"}},
                            {
                            {"block_hash", "initial", "quot"},
                            {"eoa", eoa, "quot"},
                            {"na", na, "quot"},
                            {"seller_agency_domain", seller_agency_domain, "quot"},
                            {"seller_agency_ip", seller_agency_ip, "quot"},
                            {"seller_agency_port", CStr(seller_agency_port), "non_quot"},
                            {"coin_name_from", coin_name_from, "quot"},
                            {"coin_name_to", coin_name_to, "quot"},
                            {"nonce_biginteger", CStr(-1), "non_quot"},
                            {"transfer_ethereum_from_exchange_case", CStr(0), "non_quot"},
                            {"amount", CStr(amount), "non_quot"},
                            {"exchange_rate", CStr(exchange_rate), "non_quot"},
                            {"exchange_fee_rate", CStr(exchange_fee_rate), "non_quot"},
                            {"gasPrice", CStr(gasPrice), "non_quot"},
                            {"gasPrice_for_cancel", CStr(gasPrice_for_cancel), "non_quot"},
                            {"gasLimit", CStr(gasLimit), "non_quot"},
                            {"state", state, "quot"},
                            {"pure_query", pure_query, "quot"},
                            {"signiture_for_get_balance", signiture_for_get_balance, "quot"},
                            {"signiture", signiture, "quot"},
                            {"signiture_for_ethereum_transfer_cancel", "", "quot"},
                            {"initial_transfer", "Y", "quot"},
                            {"idate_string", idate_string, "quot"}}, True)

                Else

                    gasPrice = CDec(json_webview("value")("gasPrice"))
                    gasLimit = CDec(json_webview("value")("gasLimit"))

                    gasPrice_for_cancel = gasPrice * 112 / 100

                    Select Case GRT.GR.account.ethereum_transaction_type

                        Case "signiture"

                            Dim unused = Await GRT.get_signiture_data_for_ethereum_transfer.exe("wallet", key, eoa, GRT.GR.account.private_key, na, amount, exchange_fee_rate, CType(gasPrice * 1000000000, Numerics.BigInteger), CType(gasLimit, Numerics.BigInteger))

                            signiture_data_for_ethereum_transfer = GRT.get_signiture_data_for_ethereum_transfer.info.signiture_data_for_ethereum_transfer

                            pure_query = GRT.GQS_load_order.exe(eoa, na, seller_agency_domain, seller_agency_ip, seller_agency_port, coin_name_from, coin_name_to, amount, exchange_rate, exchange_fee_rate, gasPrice, gasLimit, idate_string)

                            pure_query_for_ethereum_transfer_cancel = GRT.GQS_ethereum_transfer_cancel_treatment.exe(eoa, gasPrice_for_cancel, gasLimit, idate_string)

                            signiture_for_get_balance = GRT.Security.Gsign.sign("foo", GRT.GR.account.private_key)

                            signiture = GRT.Security.Gsign.sign(pure_query, GRT.GR.account.private_key)

                            signiture_for_ethereum_transfer_cancel = GRT.Security.Gsign.sign(pure_query_for_ethereum_transfer_cancel, GRT.GR.account.private_key)

                            case_retored_eoa_from = GRT.restore_ethereum_public_key.exe(eoa, GRT.GR.account.private_key)

                            JSS = GRT.make_json_string.exe(
                                {{"key", "submit_load_order", "quot"}},
                                {
                                {"case_retored_eoa_from", case_retored_eoa_from, "quot"},
                                {"block_hash", "initial", "quot"},
                                {"eoa", eoa, "quot"},
                                {"na", na, "quot"},
                                {"seller_agency_domain", seller_agency_domain, "quot"},
                                {"seller_agency_ip", seller_agency_ip, "quot"},
                                {"seller_agency_port", CStr(seller_agency_port), "non_quot"},
                                {"coin_name_from", coin_name_from, "quot"},
                                {"coin_name_to", coin_name_to, "quot"},
                                {"ethereum_transaction_type", GRT.GR.account.ethereum_transaction_type, "quot"},
                                {"nonce_biginteger", GRT.get_signiture_data_for_ethereum_transfer.info.nonce.Value.ToString, "non_quot"},
                                {"transfer_ethereum_from_exchange_case", CStr(0), "non_quot"},
                                {"amount", CStr(amount), "non_quot"},
                                {"exchange_rate", CStr(exchange_rate), "non_quot"},
                                {"exchange_fee_rate", CStr(exchange_fee_rate), "non_quot"},
                                {"gasPrice", CStr(gasPrice), "non_quot"},
                                {"gasPrice_for_cancel", CStr(gasPrice_for_cancel), "non_quot"},
                                {"gasLimit", CStr(gasLimit), "non_quot"},
                                {"state", state, "quot"},
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

                            pure_query = GRT.GQS_load_order.exe(eoa, na, seller_agency_domain, seller_agency_ip, seller_agency_port, coin_name_from, coin_name_to, amount, exchange_rate, exchange_fee_rate, gasPrice, gasLimit, idate_string)

                            signiture_for_get_balance = GRT.Security.Gsign.sign("foo", GRT.GR.account.private_key)

                            signiture = GRT.Security.Gsign.sign(pure_query, GRT.GR.account.private_key)

                            JSS = GRT.make_json_string.exe(
                                {{"key", "submit_load_order", "quot"}},
                                {
                                {"block_hash", "initial", "quot"},
                                {"eoa", eoa, "quot"},
                                {"na", na, "quot"},
                                {"seller_agency_domain", seller_agency_domain, "quot"},
                                {"seller_agency_ip", seller_agency_ip, "quot"},
                                {"seller_agency_port", CStr(seller_agency_port), "non_quot"},
                                {"coin_name_from", coin_name_from, "quot"},
                                {"coin_name_to", coin_name_to, "quot"},
                                {"ethereum_transaction_type", GRT.GR.account.ethereum_transaction_type, "quot"},
                                {"amount", CStr(amount), "non_quot"},
                                {"exchange_rate", CStr(exchange_rate), "non_quot"},
                                {"exchange_fee_rate", CStr(exchange_fee_rate), "non_quot"},
                                {"gasPrice", CStr(gasPrice), "non_quot"},
                                {"gasLimit", CStr(gasLimit), "non_quot"},
                                {"state", state, "quot"},
                                {"pure_query", pure_query, "quot"},
                                {"signiture_for_get_balance", signiture_for_get_balance, "quot"},
                                {"password", GRT.GR.account.password, "quot"},
                                {"signiture", signiture, "quot"},
                                {"initial_transfer", "Y", "quot"},
                                {"idate_string", idate_string, "quot"}}, False)

                        Case "private_key"

                            pure_query = GRT.GQS_load_order.exe(eoa, na, seller_agency_domain, seller_agency_ip, seller_agency_port, coin_name_from, coin_name_to, amount, exchange_rate, exchange_fee_rate, gasPrice, gasLimit, idate_string)

                            signiture_for_get_balance = GRT.Security.Gsign.sign("foo", GRT.GR.account.private_key)

                            signiture = GRT.Security.Gsign.sign(pure_query, GRT.GR.account.private_key)

                            JSS = GRT.make_json_string.exe(
                                {{"key", "submit_load_order", "quot"}},
                                {
                                {"block_hash", "initial", "quot"},
                                {"eoa", eoa, "quot"},
                                {"na", na, "quot"},
                                {"seller_agency_domain", seller_agency_domain, "quot"},
                                {"seller_agency_ip", seller_agency_ip, "quot"},
                                {"seller_agency_port", CStr(seller_agency_port), "non_quot"},
                                {"coin_name_from", coin_name_from, "quot"},
                                {"coin_name_to", coin_name_to, "quot"},
                                {"ethereum_transaction_type", GRT.GR.account.ethereum_transaction_type, "quot"},
                                {"amount", CStr(amount), "non_quot"},
                                {"exchange_rate", CStr(exchange_rate), "non_quot"},
                                {"exchange_fee_rate", CStr(exchange_fee_rate), "non_quot"},
                                {"gasPrice", CStr(gasPrice), "non_quot"},
                                {"gasLimit", CStr(gasLimit), "non_quot"},
                                {"state", state, "quot"},
                                {"pure_query", pure_query, "quot"},
                                {"signiture_for_get_balance", signiture_for_get_balance, "quot"},
                                {"private_key", GRT.GR.account.private_key, "quot"},
                                {"signiture", signiture, "quot"},
                                {"initial_transfer", "Y", "quot"},
                                {"idate_string", idate_string, "quot"}}, False)

                    End Select

                End If

                'JRS = Await GRT.socket_client.exe(GRT.GR.exchange_server_address, GRT.GR.port_number_server_exchange, GRT.GR.port_number_server_local, JSS)
                JRS = Await GRT.socket_client.exe(GRT.GR.account.agency.ip_agency, GRT.GR.account.agency.port_agency, GRT.GR.port_number_server_local, JSS)

                Call explanation_text(JRS)

            Case "submit_exchange"

                na = json_webview("value")("na").ToString
                seller_agency_domain = json_webview("value")("domain").ToString ' 이것은 그대로 쓰면 된다. 웹뷰의 타이틀이 아니라 데이터베이스로부터 온 리스트로부터 것이므로. 
                seller_agency_ip = json_webview("value")("ip").ToString
                seller_agency_port = CInt(json_webview("value")("port"))

                exchange_block_number = CLng(json_webview("value")("block_number"))
                eoa_seller = json_webview("value")("eoa").ToString ' clicker 의 상대방이다. eoa_buyer 가 clicker 이다.
                eoa_buyer = GRT.GR.account.public_key ' clickers 이다.
                clickers_coin_name_to_buy = json_webview("value")("coin_name_from").ToString
                clickers_coin_name_to_sell = GRT.GR.account.login_state
                amount = CDec(json_webview("value")("amount"))
                exchange_rate = CDec(json_webview("value")("exchange_rate"))
                exchange_fee_rate = CDec(json_webview("value")("exchange_fee_rate"))

                gas_price_biginteger = Await GRT.GR.ethereum.web3.Eth.GasPrice.SendRequestAsync
                gasPrice = CDec(CLng(gas_price_biginteger.ToString) / 1000000000)
                gasLimit = 21000

                gasPrice_for_cancel = gasPrice * 112 / 100

                pure_query = GRT.GQS_submit_exchange.exe(exchange_block_number, clickers_coin_name_to_buy, clickers_coin_name_to_sell, eoa_seller, GRT.GR.account.public_key, na, amount, exchange_rate, gasPrice, gasLimit, idate_string)

                If clickers_coin_name_to_buy = "ethereum" Then ' 개런티로 로그인 된 상태이다. 이더리움을 사는 경우이다. na 로부터 인출되는 것이므로 na 의 사인이 있어야 한다. 따라서 이곳에서는 만들 수 없다. 

                    pure_query_for_ethereum_transfer_cancel = ""
                    signiture_for_ethereum_transfer_cancel = ""

                Else ' 이더리움으로 로그인 된 상태이다.

                    pure_query_for_ethereum_transfer_cancel = GRT.GQS_ethereum_transfer_cancel_treatment.exe(eoa_buyer, gasPrice_for_cancel, gasLimit, idate_string)
                    signiture_for_ethereum_transfer_cancel = GRT.Security.Gsign.sign(pure_query_for_ethereum_transfer_cancel, GRT.GR.account.private_key)

                End If

                signiture_for_get_balance = GRT.Security.Gsign.sign("foo", GRT.GR.account.private_key)

                signiture = GRT.Security.Gsign.sign(pure_query, GRT.GR.account.private_key)

                Select Case GRT.GR.account.ethereum_transaction_type

                    Case "signiture"

                        If clickers_coin_name_to_sell = "ethereum" Then

                            Dim unused = Await GRT.get_signiture_data_for_ethereum_transfer.exe("wallet", key, GRT.GR.account.public_key, GRT.GR.account.private_key, eoa_seller, amount * exchange_rate, 0, gas_price_biginteger, CType(gasLimit, Numerics.BigInteger))

                            signiture_data_for_ethereum_transfer = GRT.get_signiture_data_for_ethereum_transfer.info.signiture_data_for_ethereum_transfer

                            nonce_biginteger = GRT.get_signiture_data_for_ethereum_transfer.info.nonce.Value

                            transfer_ethereum_from_exchange_case = False ' 거래소로부터 이더리움이 인출되는 경우가 아님.

                        Else

                            signiture_data_for_ethereum_transfer = {"", ""} ' 상대방의 na 로부터 직접 송금된다.

                            nonce_biginteger = -1

                            transfer_ethereum_from_exchange_case = True ' 거래소로부터 이더리움이 인출되는 경우임.

                        End If

                        JSS = GRT.make_json_string.exe(
                                        {{"key", "submit_exchange", "quot"}},
                                        {
                                        {"block_hash", "initial", "quot"},
                                        {"exchange_block_number", CStr(exchange_block_number), "non_quot"},
                                        {"eoa_buyer", GRT.GR.account.public_key, "quot"},
                                        {"eoa_seller", eoa_seller, "quot"},
                                        {"na", na, "quot"},
                                        {"clickers_coin_name_to_buy", clickers_coin_name_to_buy, "quot"},
                                        {"clickers_coin_name_to_sell", clickers_coin_name_to_sell, "quot"},
                                        {"ethereum_transaction_type", GRT.GR.account.ethereum_transaction_type, "quot"},
                                        {"nonce_biginteger", nonce_biginteger.ToString, "non_quot"},
                                        {"transfer_ethereum_from_exchange_case", CStr(CInt(transfer_ethereum_from_exchange_case)), "non_quot"},
                                        {"amount", CStr(amount), "non_quot"},
                                        {"exchange_rate", CStr(exchange_rate), "non_quot"},
                                        {"exchange_fee_rate", CStr(exchange_fee_rate), "non_quot"},
                                        {"gasPrice", CStr(gasPrice), "non_quot"},
                                        {"gasLimit", CStr(gasLimit), "non_quot"},
                                        {"pure_query", pure_query, "quot"},
                                        {"pure_query_for_ethereum_transfer_cancel", pure_query_for_ethereum_transfer_cancel, "quot"},
                                        {"signiture_for_get_balance", signiture_for_get_balance, "quot"},
                                        {"signiture_for_get_balance_na", "", "quot"},
                                        {"signiture_data_for_ethereum_transfer", signiture_data_for_ethereum_transfer(0), "quot"},
                                        {"signiture_data_for_ethereum_transfer_cancel", signiture_data_for_ethereum_transfer(1), "quot"},
                                        {"private_key", GRT.GR.account.private_key, "quot"},
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

                    Case "password" ' 원격 서버에 키파일이 저장되어 있는 경우만 가능

                    Case "private_key"

                End Select

                'JRS = Await GRT.socket_client.exe(GRT.GR.exchange_server_address, GRT.GR.port_number_server_exchange, GRT.GR.port_number_server_local, JSS)
                JRS = Await GRT.socket_client.exe(seller_agency_ip, seller_agency_port, GRT.GR.port_number_server_local, JSS)

                Call explanation_text(JRS)

            Case "submit_cancel"

                na = json_webview("value")("na").ToString
                seller_agency_domain = json_webview("value")("domain").ToString
                seller_agency_ip = json_webview("value")("ip").ToString
                seller_agency_port = CInt(json_webview("value")("port"))

                cancel_block_number = CLng(json_webview("value")("block_number"))
                eoa = json_webview("value")("eoa").ToString
                coin_name_from = json_webview("value")("coin_name_from").ToString
                coin_name_to = json_webview("value")("coin_name_to").ToString
                amount = CDec(json_webview("value")("amount"))
                exchange_rate = CDec(json_webview("value")("exchange_rate"))
                exchange_fee_rate = CDec(json_webview("value")("exchange_fee_rate"))
                state = ""

                If coin_name_from = "guarantee" Then

                    gasPrice = 0
                    gasLimit = 0
                    gasPrice_for_cancel = 0
                    transfer_ethereum_from_exchange_case = False

                Else

                    gas_price_biginteger = Await GRT.GR.ethereum.web3.Eth.GasPrice.SendRequestAsync
                    gasPrice = CDec(CLng(gas_price_biginteger.ToString) / 1000000000)
                    gasLimit = 21000
                    gasPrice_for_cancel = gasPrice * 112 / 100
                    transfer_ethereum_from_exchange_case = True

                End If

                pure_query = GRT.GQS_submit_cancel.exe(cancel_block_number, eoa, na, coin_name_from, amount, exchange_fee_rate, gasPrice, gasLimit)

                signiture = GRT.Security.Gsign.sign(pure_query, GRT.GR.account.private_key)

                JSS = GRT.make_json_string.exe(
                            {{"key", "submit_cancel", "quot"}},
                            {
                            {"block_hash", "initial", "quot"},
                            {"cancel_block_number", CStr(cancel_block_number), "non_quot"},
                            {"eoa", eoa, "quot"},
                            {"na", na, "quot"},
                            {"seller_agency_domain", seller_agency_domain, "quot"},
                            {"seller_agency_ip", seller_agency_ip, "quot"},
                            {"seller_agency_port", CStr(seller_agency_port), "non_quot"},
                            {"coin_name_from", coin_name_from, "quot"},
                            {"coin_name_to", coin_name_to, "quot"},
                            {"ethereum_transaction_type", GRT.GR.account.ethereum_transaction_type, "quot"},
                            {"nonce_biginteger", CStr(-1), "non_quot"},
                            {"transfer_ethereum_from_exchange_case", CStr(CInt(transfer_ethereum_from_exchange_case)), "non_quot"},
                            {"amount", CStr(amount), "non_quot"},
                            {"exchange_rate", CStr(exchange_rate), "non_quot"},
                            {"exchange_fee_rate", CStr(exchange_fee_rate), "non_quot"},
                            {"gasPrice", CStr(gasPrice), "non_quot"},
                            {"gasPrice_for_cancel", CStr(gasPrice_for_cancel), "non_quot"},
                            {"gasLimit", CStr(gasLimit), "non_quot"},
                            {"state", state, "quot"},
                            {"pure_query", pure_query, "quot"},
                            {"pure_query_for_ethereum_transfer_cancel", "", "quot"},
                            {"signiture_for_get_balance", "", "quot"}, ' 여기서는 공란일 수 밖에 없다. na 의 사인이 되어야 하므로.
                            {"signiture_data_for_ethereum_transfer", "", "quot"},
                            {"signiture_data_for_ethereum_transfer_cancel", "", "quot"},
                            {"signiture", signiture, "quot"},
                            {"signiture_na", "", "quot"},
                            {"signiture_for_ethereum_transfer_cancel", "", "quot"},
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

                'JRS = Await GRT.socket_client.exe(GRT.GR.exchange_server_address, GRT.GR.port_number_server_exchange, GRT.GR.port_number_server_local, JSS)
                JRS = Await GRT.socket_client.exe(seller_agency_ip, seller_agency_port, GRT.GR.port_number_server_local, JSS)

                Call explanation_text(JRS)

        End Select

    End Sub

    Shared Async Sub explanation_text(JRS As String)

        Dim result, receipt As String

        Dim json_receipt As Newtonsoft.Json.Linq.JObject = CType(JsonConvert.DeserializeObject(JRS), Linq.JObject)

        receipt = GRT.issue_receipt.exe(json_receipt.Root.ToString)

        receipt = Regex.Replace(receipt, "'", "\'")
        receipt = Regex.Replace(receipt, vbCrLf, "<br />")

        result = Await GR.control.wv_main.CoreWebView2.ExecuteScriptAsync($"$('#div_explanation').html('{receipt}');GR.set_interval_running=true;")

    End Sub

    Shared Async Sub display_board_exchange(json_webview As Newtonsoft.Json.Linq.JObject)

        Dim eoa, coin_name_from, coin_name_to As String
        Dim JSS, JRS As String

        eoa = GRT.GR.account.public_key

        coin_name_from = GRT.GR.account.login_state

        If coin_name_from = "guarantee" Then
            coin_name_to = "ethereum"
        Else
            coin_name_to = "guarantee"
        End If

        JSS = GRT.make_json_string.exe(
                    {{"key", "display_board_exchange", "quot"}},
                    {{"block_hash", "initial", "quot"},
                    {"eoa", eoa, "quot"},
                    {"coin_name_from", coin_name_from, "quot"},
                    {"coin_name_to", coin_name_to, "quot"},
                    {"signiture", GRT.Security.Gsign.sign("foo", GRT.GR.account.private_key), "quot"}}, True)

        'JRS = Await GRT.socket_client.exe(GRT.GR.account.agency.ip_agency, GRT.GR.account.agency.port_agency, GRT.GR.port_number_server_local, JSS) ' 자신의 거래소 서버에서만 가져온다. 안그러면 부담이 엄청 커진다.
        JRS = Await GRT.socket_client.exe(GRT.GR.exchange_server_address, GRT.GR.port_number_server_exchange, GRT.GR.port_number_server_local, JSS)

        Dim JRS_json = CType(JsonConvert.DeserializeObject(JRS), Linq.JObject)

        Dim DataSet = JsonConvert.DeserializeObject(Of DataSet)(JRS_json("value")("dataset_json_string").ToString)

        Dim board_exchange_script = generate_display_board_exchange_script.exe(DataSet)

        Await GR.control.wv_sub.CoreWebView2.ExecuteScriptAsync(
                $"$('#div_order').html('{Regex.Replace(board_exchange_script, "'", "\'")}');")

    End Sub

End Class
