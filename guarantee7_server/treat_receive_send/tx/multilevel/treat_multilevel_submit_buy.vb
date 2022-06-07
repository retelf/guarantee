Imports System.Net.Sockets
Imports System.Text

Public Class treat_multilevel_submit_buy

    Public Shared Async Function exe(json As Newtonsoft.Json.Linq.JObject, JSS As String) As Task(Of String)

        Dim command_key, eoa_signer, eoa_guarantee_seller, ethereum_transfer_eoa_from, ethereum_transfer_eoa_to,
            seller_na, buyer_na, ma, signiture, signiture_key, signiture_for_ethereum_transfer_cancel, signiture_for_get_balance,
            JRS, closing_time_utc_string, idate_string As String
        Dim balance_clicker, balance_na, amount, gasPrice, gasPrice_for_cancel, gasLimit, exchange_rate, exchange_fee_rate As Decimal
        Dim registered, verified As Boolean
        Dim block_number, sell_order_block_number As Long
        Dim seller_agency_domain, seller_agency_ip As String
        Dim seller_agency_port, days_span As Integer
        Dim pure_query, pure_query_for_ethereum_transfer_cancel, database_name, table_name, query_type, contract_type, error_message As String
        Dim received_block_hash, previous_hash As String
        Dim initial_transfer As String
        Dim idate_generated As DateTime
        Dim json_JRS, json_JRS_lock, JRS_submit_info_verify As Newtonsoft.Json.Linq.JObject

        command_key = json("key").ToString

        sell_order_block_number = CLng(json("value")("sell_order_block_number"))
        received_block_hash = json("value")("block_hash").ToString
        eoa_signer = json("value")("eoa_signer").ToString
        eoa_guarantee_seller = json("value")("eoa_guarantee_seller").ToString
        ethereum_transfer_eoa_from = json("value")("ethereum_transfer_eoa_from").ToString
        ethereum_transfer_eoa_to = json("value")("ethereum_transfer_eoa_to").ToString
        seller_na = json("value")("seller_na").ToString
        buyer_na = json("value")("buyer_na").ToString
        ma = json("value")("ma").ToString
        seller_agency_domain = json("value")("seller_agency_domain").ToString
        seller_agency_ip = json("value")("seller_agency_ip").ToString
        seller_agency_port = CInt(json("value")("seller_agency_port"))
        days_span = CInt(json("value")("days_span").ToString.Trim)
        closing_time_utc_string = CStr(json("value")("closing_time_utc_string").ToString)
        exchange_fee_rate = CDec(json("value")("exchange_fee_rate").ToString.Trim)

        registered = GRT.check_registered_eoa.exe_multi(ethereum_transfer_eoa_from, {"guarantee", "ethereum"})

        If registered Then

            amount = CDec(json("value")("amount"))
            gasPrice = CDec(json("value")("gasPrice"))
            gasPrice_for_cancel = CDec(json("value")("gasPrice_for_cancel"))
            gasLimit = CDec(json("value")("gasLimit"))
            exchange_rate = CDec(json("value")("exchange_rate"))
            exchange_fee_rate = CDec(json("value")("exchange_fee_rate"))
            signiture = json("value")("signiture").ToString
            signiture_key = Regex.Match(signiture, "^0x.{64}").ToString
            signiture_for_ethereum_transfer_cancel = json("value")("signiture_for_ethereum_transfer_cancel").ToString
            initial_transfer = json("value")("initial_transfer").ToString
            idate_string = json("value")("idate_string").ToString

            idate_generated = DateTime.Now

            ' 가장 먼저 보내온 정보들이 진정한 정보인지 확인.
            ' eoa_guarantee_seller 가 아닌 자, 즉 ethereum_transfer_eoa_from 가 eoa_guarantee_seller 가 만든 정보를 보낸 것이기 때문이지만
            ' 스스로 만든 정보라 할지라도 변조의 가능성이 있으므로 
            ' 이는 refund, exchange, exchange_cancel 등 모든 클릭 상황에서 발생하는 문제다.
            ' pure_query 마저도 조작해서 넘겨올 수 있기 때문이다.
            ' 그렇다고 서버에서의 데이타만 가지고 처리해서도 안된다. pure_query의 시그니처는 있어야 하기 때문이다.
            ' 그렇지 않으면 서버에서 그냥 무단으로 pure_query 를 남발해 버릴 수도 있다.

            JRS_submit_info_verify = check_submit_info_match.multilevel_buy(json)

            If JRS_submit_info_verify("success").ToString = "success" Then

                If initial_transfer = "Y" Then ' 에이전트 서버가 최초로 접수한 경우. If Not GRT.GR.node_level = 0 Then 조건은 여기서 충족된다.

                    signiture_for_get_balance = GRT.Security.Gsign.sign("foo", GRT.GR.server_private_key)
                    ' 개런티 밸런스 확인을 위한 것이다. 개런티는 최초 셀러의 에이전시에 보관되며 지금 이 서버가 셀러에이전시 서버이다.
                    json("value")("signiture_for_get_balance") = signiture_for_get_balance

                    balance_clicker = Await GRT.get_balance.ethereum(ethereum_transfer_eoa_from)
                    balance_na = GRT.get_guarantee_balance_from_local_server.exe("foo", signiture_for_get_balance, seller_na)

                    If balance_clicker >= amount + gasPrice * gasLimit / 1000000000 And
                        balance_na >= (amount / exchange_rate) Then ' 실제로는 balance_na >= 1 이다.

                        pure_query = GRT.GQS_multilevel_buy.exe(sell_order_block_number, ethereum_transfer_eoa_from, ma, seller_na, buyer_na, amount, gasPrice, gasLimit, closing_time_utc_string, idate_string)

                        verified = GRT.Security.Gverify.verify(pure_query, signiture, eoa_signer)

                        If verified Then

                            Call GRT.agent_record.generate(signiture_key, idate_generated, json, JSS)

                            json_JRS_lock = Await request_lock.lock(sell_order_block_number, signiture_key, "bc_multilevel", "sell_order") ' 가장 먼저 락을 걸어준다.

                            If json_JRS_lock("success").ToString = "success" Then

                                Call GRT.agent_record.state_update("lock_request_response_received", "", signiture_key)

                                If Not json_JRS_lock("value")("lock").ToString = "no_rows" Then ' 로컬거래소에서는 있었는데 메인에서 없는 경우이다.

                                    Call GRT.agent_record.state_update("row_found", "", signiture_key)

                                    If json_JRS_lock("value")("lock").ToString = "alive" Then ' 메인에서 락처리가 됨.

                                        Call GRT.agent_record.state_update("lock_succeeded", "", signiture_key)

                                        json("value")("initial_transfer") = "N"

                                        JRS = Await treat_multilevel_submit_buy_ethereum_transfer.exe(json) ' 이더리움 송금처리

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

                        If Not balance_clicker >= amount + gasPrice * gasLimit / 1000000000 Then

                            JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", ethereum_transfer_eoa_from & " 의 잔고가 부족합니다. 현재 잔고는 " & balance_clicker & " 입니다.", "quot"}}, False)

                        ElseIf Not balance_na >= (amount / exchange_rate) Then

                            JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", "거래소 " & seller_na & " 의 잔고가 부족합니다. 현재 잔고는 " & balance_na & " 입니다.", "quot"}}, False)

                        End If

                    End If

                Else
                    ' 스스로 메인이거나 부모서버로부터 릴레이 받은 경우임. 
                    ' 이 경우 이더리움 밸런스 부족 문제는 발생할 수가 없다. 이미 송금실시는 끝난 상태이기 때문이다.
                    ' 이미 amount 만큼 감소한 상태이므로 또 다시 밸런스 체킹을 하면 오류가 발생하게 된다.
                    ' 물론 이미 발생한 gasPrice_for_cancel 지출은 기록해 주어야 한다.

                    If GRT.GR.node_level = 0 Then
                        Call GRT.agent_record.generate(signiture_key, idate_generated, json, JSS)
                    End If

                    Call GRT.set_block_number_and_get_previous_block_hash.exe()

                    If GRT.GR.node_level = 0 Then
                        Call GRT.agent_record.state_update("block_number_generated_" & block_number, "", signiture_key)
                    End If

                    block_number = GRT.set_block_number_and_get_previous_block_hash.data.block_number
                    previous_hash = GRT.set_block_number_and_get_previous_block_hash.data.previous_hash

                    Dim ethereum_transaction_result As String = json("value")("ethereum_transaction_result").ToString
                    Dim transaction_hash_initial As String = json("value")("transaction_hash_initial").ToString
                    Dim transaction_hash_cancel As String = json("value")("transaction_hash_cancel").ToString
                    Dim tem_transaction_success_initial As Boolean = CBool(json("value")("tem_transaction_success_initial"))
                    Dim tem_transaction_success_cancel As Boolean = CBool(json("value")("tem_transaction_success_cancel"))

                    signiture_for_get_balance = json("value")("signiture_for_get_balance").ToString

                    'balance_clicker = Await GRT.get_balance.ethereum(ethereum_transfer_eoa_from) 
                    balance_na = GRT.get_guarantee_balance_from_local_server.exe("foo", signiture_for_get_balance, seller_na)

                    If ethereum_transaction_result = "initial_success" Or ethereum_transaction_result = "transfer_ethereum_from_exchange_case_proceeding" Then

                        If balance_na >= (amount / exchange_rate) Then ' 실제로는 balance_na >= 1 이다.
                            ' 만약 서버가 부정확한 데이터를 담고 있다면 문제가 될 수 있다. 개런티 잔고만이 문제된다.

                            pure_query = GRT.GQS_multilevel_buy.exe(sell_order_block_number, ethereum_transfer_eoa_from, ma, seller_na, buyer_na, amount, gasPrice, gasLimit, closing_time_utc_string, idate_string)

                            verified = GRT.Security.Gverify.verify(pure_query, signiture, eoa_signer)

                            If verified Then

                                database_name = ""
                                table_name = ""
                                query_type = "UPDATE, INSERT"
                                contract_type = command_key

                                JRS = Await self_input_and_relay.exe(command_key, block_number, received_block_hash, previous_hash, eoa_signer, database_name, table_name, query_type, contract_type, pure_query, signiture, JSS) ' 부모서버로부터 릴레이 받은 경우임.  

                                json_JRS = CType(JsonConvert.DeserializeObject(JRS), Linq.JObject)

                                If json_JRS("success").ToString = "success" Then

                                    Call GRT.agent_record.confirm(block_number, signiture_key) ' 에이전트 서버가 최초로 접수한 경우와 메인서버인 두 경우만 빼고는 아무런 변화를 일으키지 않게 된다.

                                End If

                            Else
                                JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", "데이터 변조", "quot"}}, False)
                            End If

                        Else

                            JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", "거래소 " & seller_na & " 의 개런티 잔고가 부족합니다. 현재 잔고는 " & balance_na & " 개런티 입니다.", "quot"}}, False)

                        End If

                    Else ' cancel_success

                        pure_query_for_ethereum_transfer_cancel = GRT.GQS_ethereum_transfer_cancel_treatment.exe(ethereum_transfer_eoa_from, gasPrice_for_cancel, gasLimit, idate_string)

                        verified = GRT.Security.Gverify.verify(pure_query_for_ethereum_transfer_cancel, signiture_for_ethereum_transfer_cancel, eoa_signer)

                        If verified Then

                            database_name = "bc"
                            table_name = "account"
                            query_type = "UPDATE"
                            contract_type = command_key

                            JRS = Await self_input_and_relay.exe(command_key, block_number, received_block_hash, previous_hash, eoa_signer, database_name, table_name, query_type, contract_type, pure_query_for_ethereum_transfer_cancel, signiture_for_ethereum_transfer_cancel, JSS) ' 부모서버로부터 릴레이 받은 경우임.  

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
            JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", "unregistered_account", "quot"}}, False)
        End If

        Return JRS

    End Function

End Class
