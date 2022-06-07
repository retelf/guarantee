Imports System.Net.Sockets
Imports System.Numerics
Imports System.Text

Public Class treat_exchange
    Public Shared Async Function exe(json As Newtonsoft.Json.Linq.JObject, JSS As String) As Task(Of String)

        Dim command_key, eoa_seller, eoa_buyer, na, clickers_coin_name_to_buy, clickers_coin_name_to_sell,
            signiture, signiture_key, signiture_for_ethereum_transfer_cancel, signiture_for_get_balance, signiture_for_get_balance_na, signiture_data_for_ethereum_transfer(1), initial_transfer, JRS, idate_string As String
        Dim balance_clicker, balance_na, seller_amount, minimum_clicker_amount_for_compare, minimum_na_amount_for_compare, gasPrice, gasPrice_for_cancel, gasLimit, exchange_rate, exchange_fee_rate As Decimal
        Dim registered, verified As Boolean
        Dim block_number, exchange_block_number As Long
        Dim pure_query, pure_query_for_ethereum_transfer_cancel, database_name, table_name, query_type, contract_type, error_message As String
        Dim received_block_hash, previous_hash As String
        Dim idate_generated As DateTime
        Dim json_JRS, json_JRS_lock As Newtonsoft.Json.Linq.JObject
        Dim gas_price_biginteger As BigInteger

        command_key = json("key").ToString

        received_block_hash = json("value")("block_hash").ToString
        exchange_block_number = CLng(json("value")("exchange_block_number"))
        eoa_seller = json("value")("eoa_seller").ToString
        eoa_buyer = json("value")("eoa_buyer").ToString
        na = json("value")("na").ToString
        clickers_coin_name_to_buy = json("value")("clickers_coin_name_to_buy").ToString
        clickers_coin_name_to_sell = json("value")("clickers_coin_name_to_sell").ToString

        registered = GRT.check_registered_eoa.exe(eoa_buyer, clickers_coin_name_to_sell)

        If registered Then

            seller_amount = CDec(json("value")("amount"))
            gasPrice = CDec(json("value")("gasPrice"))
            gasLimit = CDec(json("value")("gasLimit"))
            exchange_rate = CDec(json("value")("exchange_rate"))
            exchange_fee_rate = CDec(json("value")("exchange_fee_rate"))
            pure_query = json("value")("pure_query").ToString
            pure_query_for_ethereum_transfer_cancel = json("value")("pure_query_for_ethereum_transfer_cancel").ToString
            signiture_for_get_balance = json("value")("signiture_for_get_balance").ToString
            signiture_for_get_balance_na = json("value")("signiture_for_get_balance_na").ToString
            signiture_data_for_ethereum_transfer(0) = json("value")("signiture_data_for_ethereum_transfer").ToString
            signiture_data_for_ethereum_transfer(1) = json("value")("signiture_data_for_ethereum_transfer_cancel").ToString
            signiture = json("value")("signiture").ToString
            signiture_for_ethereum_transfer_cancel = json("value")("signiture_for_ethereum_transfer_cancel").ToString
            signiture_key = Regex.Match(signiture, "^0x.{64}").ToString
            initial_transfer = json("value")("initial_transfer").ToString
            idate_string = json("value")("idate_string").ToString

            idate_generated = DateTime.Now

            Dim JRS_submit_info_verify = check_submit_info_match.exchange(json)

            If JRS_submit_info_verify("success").ToString = "success" Then

                If initial_transfer = "Y" Then

                    If clickers_coin_name_to_buy = "ethereum" Then

                        balance_clicker = GRT.get_guarantee_balance_from_local_server.exe("foo", signiture_for_get_balance, eoa_buyer)
                        balance_na = Await GRT.get_balance.ethereum(na)
                        gas_price_biginteger = CType(gasPrice * 1000000000, BigInteger)

                        gasPrice_for_cancel = gasPrice * 112 / 100

                        pure_query_for_ethereum_transfer_cancel = GRT.GQS_ethereum_transfer_cancel_treatment.exe(na, gasPrice_for_cancel, gasLimit, idate_string)
                        signiture_for_ethereum_transfer_cancel = GRT.Security.Gsign.sign(pure_query_for_ethereum_transfer_cancel, GRT.GR.server_private_key)

                        json("value")("pure_query_for_ethereum_transfer_cancel") = pure_query_for_ethereum_transfer_cancel
                        json("value")("signiture_for_ethereum_transfer_cancel") = signiture_for_ethereum_transfer_cancel

                        Dim unused = Await GRT.get_signiture_data_for_ethereum_transfer.exe("server", command_key, na, GRT.GR.server_private_key, eoa_buyer, seller_amount * exchange_rate, 0, gas_price_biginteger, CType(gasLimit, Numerics.BigInteger))

                        signiture_data_for_ethereum_transfer = GRT.get_signiture_data_for_ethereum_transfer.info.signiture_data_for_ethereum_transfer

                        json("value")("signiture_data_for_ethereum_transfer") = signiture_data_for_ethereum_transfer(0)
                        json("value")("signiture_data_for_ethereum_transfer_cancel") = signiture_data_for_ethereum_transfer(1)
                        json("value")("nonce_biginteger") = GRT.get_signiture_data_for_ethereum_transfer.info.nonce.Value.ToString

                    Else

                        balance_clicker = Await GRT.get_balance.ethereum(eoa_buyer)

                        signiture_for_get_balance_na = GRT.Security.Gsign.sign("foo", GRT.GR.server_private_key)
                        json("value")("signiture_for_get_balance_na") = signiture_for_get_balance_na
                        balance_na = GRT.get_guarantee_balance_from_local_server.exe("foo", signiture_for_get_balance_na, na)

                    End If

                    minimum_clicker_amount_for_compare = seller_amount * exchange_rate
                    minimum_na_amount_for_compare = seller_amount

                    If balance_clicker >= minimum_na_amount_for_compare + gasPrice * gasLimit / 1000000000 And
                        balance_na >= minimum_na_amount_for_compare + gasPrice * gasLimit / 1000000000 Then

                        pure_query = GRT.GQS_submit_exchange.exe(exchange_block_number, clickers_coin_name_to_buy, clickers_coin_name_to_sell, eoa_seller, eoa_buyer, na, seller_amount, exchange_rate, gasPrice, gasLimit, idate_string)

                        verified = GRT.Security.Gverify.verify(pure_query, signiture, eoa_buyer)

                        If verified Then

                            Call GRT.agent_record.generate(signiture_key, idate_generated, json, JSS)

                            Call GRT.agent_record.state_update("lock_requested", "", signiture_key)

                            json_JRS_lock = Await request_lock.lock(exchange_block_number, signiture_key, "bc", "exchange") ' 가장 먼저 락을 걸어준다.

                            If json_JRS_lock("success").ToString = "success" Then

                                Call GRT.agent_record.state_update("lock_request_response_received", "", signiture_key)

                                If Not json_JRS_lock("value")("lock").ToString = "no_rows" Then ' 로컬거래소에서는 있었는데 메인에서 없는 경우이다.

                                    Call GRT.agent_record.state_update("row_found", "", signiture_key)

                                    If json_JRS_lock("value")("lock").ToString = "alive" Then ' 메인에서 락처리가 됨.

                                        Call GRT.agent_record.state_update("lock_succeeded", "", signiture_key)

                                        json("value")("initial_transfer") = "N"

                                        JRS = Await treat_ethereum_transfer_for_submit_exchange.exe(json) ' 이더리움 송금처리

                                    Else ' 이미 locked 인 경우이다. 스스로 락을 걸지 않았으므로 언락 역시 필요없다.

                                        Call GRT.agent_record.state_update("already_locked", "", signiture_key)

                                        JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", "현재 다른 사용자가 먼저 선택한 상태입니다.", "quot"}}, False)

                                    End If

                                Else ' 애당초 락을 걸수도 없었으므로 언락 역시 필요없다.

                                    Call GRT.agent_record.state_update("no_rows", "", signiture_key)

                                    JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", "이미 삭제되었습니다.", "quot"}}, False)

                                End If

                            Else

                                error_message = json_JRS_lock("value")("reason").ToString

                                Call GRT.agent_record.state_update("lock_failed", error_message, signiture_key)

                                JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", error_message, "quot"}}, False)

                            End If

                        Else
                            JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", "데이터 변조", "quot"}}, False)
                        End If

                    Else

                        If Not balance_clicker >= seller_amount + gasPrice * gasLimit / 1000000000 Then

                            JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", eoa_buyer & " 의 잔고가 부족합니다. 현재 잔고는 " & balance_clicker & " 입니다.", "quot"}}, False)

                        ElseIf Not balance_na >= seller_amount + gasPrice * gasLimit / 1000000000 Then

                            JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", "거래소 " & na & " 의 잔고가 부족합니다. 현재 잔고는 " & balance_na & " 입니다.", "quot"}}, False)

                        End If

                    End If

                Else

                    ' 이더리움 잔고에 관해서는 하등의 문제는 없다. 이미 송금실시가 끝났기 때문이다.

                    If GRT.GR.node_level = 0 Then
                        Call GRT.agent_record.generate(signiture_key, idate_generated, json, JSS)
                    End If

                    Call GRT.set_block_number_and_get_previous_block_hash.exe()

                    If GRT.GR.node_level = 0 Then
                        Call GRT.agent_record.state_update("block_number_generated_" & block_number, "", signiture_key)
                    End If

                    database_name = "bc, bc, bc, bc, bc"
                    table_name = "account, account, account, account, exchange"
                    query_type = "UPDATE, UPDATE, UPDATE, UPDATE, DELETE"

                    contract_type = command_key

                    block_number = GRT.set_block_number_and_get_previous_block_hash.data.block_number
                    previous_hash = GRT.set_block_number_and_get_previous_block_hash.data.previous_hash

                    Dim ethereum_transaction_result As String = json("value")("ethereum_transaction_result").ToString
                    Dim transaction_hash_initial As String = json("value")("transaction_hash_initial").ToString
                    Dim transaction_hash_cancel As String = json("value")("transaction_hash_cancel").ToString
                    Dim tem_transaction_success_initial As Boolean = CBool(json("value")("tem_transaction_success_initial"))
                    Dim tem_transaction_success_cancel As Boolean = CBool(json("value")("tem_transaction_success_cancel"))

                    If ethereum_transaction_result = "initial_success" Or ethereum_transaction_result = "transfer_ethereum_from_exchange_case_proceeding" Then

                        ' 이미 에이전트 거래소에서는 성공적으로 잔고 확인을 마쳤다. 그러나 개별 서버 중 부정확한 잔고 데이터가 있다면 여기서 걸리게 된다.

                        If clickers_coin_name_to_buy = "ethereum" Then ' 거래소로부터 eoa_buyer 로의 이더리움 송금이 성공함. eoa_buyer 의 개런티 잔고가 문제된다.

                            minimum_clicker_amount_for_compare = seller_amount * exchange_rate

                            balance_clicker = GRT.get_guarantee_balance_from_local_server.exe("foo", signiture_for_get_balance, eoa_buyer)

                            If balance_clicker >= minimum_clicker_amount_for_compare Then

                                JRS = Await sub_exe(command_key,
                                            received_block_hash,
                                            signiture,
                                            signiture_key,
                                            exchange_block_number,
                                            clickers_coin_name_to_buy,
                                            clickers_coin_name_to_sell,
                                            eoa_seller,
                                            eoa_buyer,
                                            na,
                                            seller_amount,
                                            exchange_rate,
                                            gasPrice,
                                            gasLimit,
                                            JSS,
                                            idate_string)

                            Else
                                JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", eoa_buyer & " 의 잔고가 부족합니다. 현재 잔고는 " & balance_clicker & " 입니다.", "quot"}}, False)
                            End If

                        Else ' eoa_buyer 로부터 eoa_seller 에게로의 이더리움 송금이 성공했다. 거래소의 개런티 잔고가 문제된다.

                            minimum_na_amount_for_compare = seller_amount

                            balance_na = GRT.get_guarantee_balance_from_local_server.exe("foo", signiture_for_get_balance_na, na)

                            If balance_na >= minimum_na_amount_for_compare Then

                                JRS = Await sub_exe(command_key,
                                        received_block_hash,
                                        signiture,
                                        signiture_key,
                                        exchange_block_number,
                                        clickers_coin_name_to_buy,
                                        clickers_coin_name_to_sell,
                                        eoa_seller,
                                        eoa_buyer,
                                        na,
                                        seller_amount,
                                        exchange_rate,
                                        gasPrice,
                                        gasLimit,
                                        JSS,
                                        idate_string)

                            Else
                                JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", "거래소(" & na & ")의 잔고가 부족합니다. 현재 잔고는 " & balance_clicker & " 입니다.", "quot"}}, False)
                            End If

                        End If

                    Else ' cancel_success

                        Dim ethereum_transfer_eoa_from As String

                        If clickers_coin_name_to_buy = "ethereum" Then
                            ethereum_transfer_eoa_from = na
                        Else
                            ethereum_transfer_eoa_from = eoa_buyer
                        End If

                        gasPrice_for_cancel = gasPrice * 112 / 100

                        pure_query_for_ethereum_transfer_cancel = GRT.GQS_ethereum_transfer_cancel_treatment.exe(ethereum_transfer_eoa_from, gasPrice_for_cancel, gasLimit, idate_string)

                        verified = GRT.Security.Gverify.verify(pure_query_for_ethereum_transfer_cancel, signiture_for_ethereum_transfer_cancel, ethereum_transfer_eoa_from)

                        If verified Then

                            database_name = "bc"
                            table_name = "account"
                            query_type = "UPDATE"
                            contract_type = command_key

                            JRS = Await self_input_and_relay.exe(command_key, block_number, received_block_hash, previous_hash, ethereum_transfer_eoa_from, database_name, table_name, query_type, contract_type, pure_query_for_ethereum_transfer_cancel, signiture_for_ethereum_transfer_cancel, JSS) ' 부모서버로부터 릴레이 받은 경우임.  

                            json_JRS = CType(JsonConvert.DeserializeObject(JRS), Linq.JObject)

                            If json_JRS("success").ToString = "success" Then

                                Call GRT.agent_record.confirm(block_number, signiture_key) ' 에이전트 서버가 최초로 접수한 경우와 메인서버인 두 경우만 빼고는 아무런 변화를 일으키지 않게 된다.

                            End If

                        Else
                            JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", "데이터 변조", "quot"}}, False)
                        End If

                    End If

                End If

            Else
                JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", JRS_submit_info_verify("value")("reason").ToString, "quot"}}, False)
            End If

        Else
            JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", eoa_buyer & " 는 등록되지 않은 계정명입니다.", "quot"}}, False)
        End If

        Return JRS

    End Function

    Shared Async Function sub_exe(command_key As String,
                            received_block_hash As String,
                            signiture As String,
                            signiture_key As String,
                            exchange_block_number As Long,
                            clickers_coin_name_to_buy As String,
                            clickers_coin_name_to_sell As String,
                            eoa_seller As String,
                            eoa_buyer As String,
                            na As String, seller_amount As Decimal,
                            exchange_rate As Decimal,
                            gasPrice As Decimal,
                            gasLimit As Decimal,
                            JSS As String,
                            idate_string As String) As Task(Of String)

        Dim JRS As String

        Dim pure_query = GRT.GQS_submit_exchange.exe(exchange_block_number, clickers_coin_name_to_buy, clickers_coin_name_to_sell, eoa_seller, eoa_buyer, na, seller_amount, exchange_rate, gasPrice, gasLimit, idate_string)

        Dim verified = GRT.Security.Gverify.verify(pure_query, signiture, eoa_buyer)

        If verified Then

            Dim database_name = "bc, bc, bc, bc, bc"
            Dim table_name = "account, account, account, account, exchange"
            Dim query_type = "UPDATE, UPDATE, UPDATE, UPDATE, DELETE"

            Dim contract_type = command_key

            Dim block_number = GRT.set_block_number_and_get_previous_block_hash.data.block_number
            Dim previous_hash = GRT.set_block_number_and_get_previous_block_hash.data.previous_hash

            If GRT.GR.node_level = 0 Then
                Call GRT.agent_record.state_update("block_number_generated_" & block_number, "", signiture_key)
            End If

            JRS = Await self_input_and_relay.exe(command_key, block_number, received_block_hash, previous_hash, eoa_buyer, database_name, table_name, query_type, contract_type, pure_query, signiture, JSS)

            Dim json_JRS = CType(JsonConvert.DeserializeObject(JRS), Linq.JObject)

            If json_JRS("success").ToString = "success" Then

                Call GRT.agent_record.confirm(block_number, signiture_key) ' 에이전트 서버가 최초로 접수한 경우와 메인서버인 두 경우만 빼고는 아무런 변화를 일으키지 않게 된다.

            End If

        Else
            JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", "데이터 변조", "quot"}}, False)
        End If

        Return JRS

    End Function


End Class
