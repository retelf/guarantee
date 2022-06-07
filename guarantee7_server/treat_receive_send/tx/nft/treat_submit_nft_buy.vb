Imports System.IO
Imports System.Numerics

Public Class treat_submit_nft_buy

    Public Shared Async Function exe(json As Newtonsoft.Json.Linq.JObject, JSS As String) As Task(Of String)

        Dim initial_transfer = CStr(json("value")("initial_transfer"))
        Dim command_key = CStr(json("key"))
        Dim received_block_hash = CStr(json("value")("block_hash"))
        Dim sell_order_block_number = CLng(json("value")("sell_order_block_number"))
        Dim owner_portion_block_number = CLng(json("value")("owner_portion_block_number"))
        Dim nfa = CStr(json("value")("nfa"))
        Dim token_id = CInt(json("value")("token_id"))
        Dim pieces_to_buy = CInt(json("value")("pieces_to_buy"))
        Dim pieces_alive = CInt(json("value")("pieces_alive"))
        Dim confirmed_type = CStr(json("value")("confirmed_type"))
        Dim price_piece = CDec(json("value")("price_piece"))
        Dim price = CDec(json("value")("price"))
        Dim nonce = CType(CLng(json("value")("nonce_biginteger")), BigInteger)
        Dim gasPrice = CDec(json("value")("gasPrice_Gwei"))
        Dim gasPrice_for_cancel = CDec(json("value")("gasPrice_for_cancel_Gwei"))
        Dim gasLimit = CDec(json("value")("gasLimit"))
        Dim currency = CStr(json("value")("currency"))
        Dim max_split = CInt(json("value")("max_split"))
        Dim eoa_seller = CStr(json("value")("eoa_seller"))
        Dim eoa_buyer = CStr(json("value")("eoa_buyer"))
        Dim na_seller = CStr(json("value")("na_seller"))
        Dim exchange_name_seller = CStr(json("value")("exchange_name_seller"))
        Dim seller_agency_domain = CStr(json("value")("domain_seller"))
        Dim seller_agency_ip = CStr(json("value")("ip_seller"))
        Dim seller_agency_port = CInt(json("value")("port_seller"))
        Dim na_buyer = CStr(json("value")("na_buyer"))
        Dim exchange_name_buyer = CStr(json("value")("exchange_name_buyer"))
        Dim buyer_agency_domain = CStr(json("value")("domain_buyer"))
        Dim buyer_agency_ip = CStr(json("value")("ip_buyer"))
        Dim buyer_agency_port = CInt(json("value")("port_buyer"))
        Dim days_span = CInt(json("value")("days_span"))
        Dim closing_time_utc_string = CStr(json("value")("closing_time"))
        Dim transaction_fee_rate = CDec(json("value")("transaction_fee_rate"))
        Dim state_open = CBool(CInt(json("value")("state_open")))
        Dim idate_string = CStr(json("value")("idate_string"))
        Dim eoa_idate_string_signature = CStr(json("value")("idate_string_signature"))
        Dim signiture_data_for_ethereum_transfer_0 = CStr(json("value")("signiture_data_for_ethereum_transfer_0"))
        Dim signiture_data_for_ethereum_transfer_1 = CStr(json("value")("signiture_data_for_ethereum_transfer_1"))
        Dim pure_query = CStr(json("value")("pure_query"))
        Dim signiture = CStr(json("value")("signiture"))
        Dim signiture_key = CStr(json("value")("signiture_key"))
        Dim pure_query_for_ethereum_transfer_cancel = CStr(json("value")("pure_query_for_ethereum_transfer_cancel"))
        Dim signiture_for_ethereum_transfer_cancel = CStr(json("value")("signiture_for_ethereum_transfer_cancel"))

        Dim idate_generated = DateTime.Now

        Dim pieces_alive_for_check As Integer
        Dim JRS, error_message, na_idate_string_signature As String
        Dim balance_buyer As Decimal
        Dim verified, eoa_exist As Boolean
        Dim json_JRS, json_JRS_lock, JRS_submit_info_verify As Newtonsoft.Json.Linq.JObject

        verified = GRT.Security.Gverify.verify(idate_string, eoa_idate_string_signature, eoa_buyer)

        If verified Then

            eoa_exist = GRT.check_eoa_exist.exe(eoa_buyer)

            If eoa_exist Then

                JRS_submit_info_verify = check_submit_info_match.nft_buy(json)

                If JRS_submit_info_verify("success").ToString = "success" Then

                    na_idate_string_signature = GRT.Security.Gsign.sign(idate_string, GRT.GR.server_private_key)
                    json("value")("na_idate_string_signature") = na_idate_string_signature

                    If initial_transfer = "Y" Then

                        If currency = "ethereum" Then
                            balance_buyer = Await GRT.get_balance.ethereum(eoa_buyer)
                        Else
                            balance_buyer = GRT.get_guarantee_balance_from_local_server.exe(idate_string, eoa_idate_string_signature, eoa_buyer)
                        End If

                        If balance_buyer >= price + gasPrice * gasLimit / 1000000000 Then

                            pieces_alive_for_check = CInt(JRS_submit_info_verify("value")("pieces_alive"))

                            If pieces_alive_for_check >= pieces_to_buy Then

                                pure_query = GRT.GQS_nft_buy.exe(
                                                                sell_order_block_number,
                                                                owner_portion_block_number,
                                                                nfa,
                                                                token_id,
                                                                eoa_seller,
                                                                eoa_buyer,
                                                                price,
                                                                currency,
                                                                gasPrice,
                                                                gasLimit,
                                                                pieces_to_buy,
                                                                confirmed_type,
                                                                na_buyer,
                                                                days_span,
                                                                closing_time_utc_string,
                                                                transaction_fee_rate,
                                                                max_split,
                                                                idate_string)

                                verified = GRT.Security.Gverify.verify(pure_query, signiture, eoa_buyer)

                                If verified Then

                                    Call GRT.agent_record.generate(signiture_key, idate_generated, json, JSS)

                                    ' deposit 방식으로 정산하기 때문에 이더리움 거래라 할 지라도 락을 걸 필요가 없다.
                                    ' 다만 메인에서 전체거래 또는 수량이 부족한 만큼 거래를 불성립시키고 리턴하는 방식으로 처리해야 한다.
                                    ' 이렇게 하게 되면 recall 이나 confirm 의 경우 동일인이 여러 지갑에서 tx를 날린 경우에도 문제가 없게 된다.

                                    json_JRS_lock = Await request_lock.lock(sell_order_block_number, signiture_key, "bc_nft", "sell_order") ' 가장 먼저 락을 걸어준다.

                                    If json_JRS_lock("success").ToString = "success" Then

                                        Call GRT.agent_record.state_update("lock_request_response_received", "", signiture_key)

                                        If Not json_JRS_lock("value")("lock").ToString = "no_rows" Then ' 로컬거래소에서는 있었는데 메인에서 없는 경우이다.

                                            Call GRT.agent_record.state_update("row_found", "", signiture_key)

                                            If json_JRS_lock("value")("lock").ToString = "open" Then ' 메인에서 락처리가 됨.

                                                Call GRT.agent_record.state_update("lock_succeeded", "", signiture_key)

                                                json("value")("initial_transfer") = "N"

                                                If currency = "ethereum" Then

                                                    JRS = Await treat_nft_submit_buy_ethereum_transfer.exe(json) ' 이더리움 송금처리. send_main.exe 은 이곳에서 이루어진다.

                                                Else

                                                    JSS = CType(JsonConvert.SerializeObject(json), String)

                                                    JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "success", "quot"}}, {{"reason", "send_success_at_least", "quot"}}, False)

                                                    Call send_main.exe(signiture_key, JSS) ' 메인으로 보낸다. 하등의 데이터베이스 처리 없이 보낸다.

                                                End If

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
                                JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", "NFT 주문 수량초과입니다. 현재 주문 가능 조각수는 " & pieces_alive_for_check & " 개 입니다.", "quot"}}, False)
                            End If

                        Else

                            JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", eoa_buyer & " 의 잔고가 부족합니다. 현재 잔고는 " & balance_buyer & " " & currency & " 입니다.", "quot"}}, False)

                        End If

                    Else
                        ' 스스로 메인이거나 부모서버로부터 릴레이 받은 경우임. 

                        If GRT.GR.node_level = 0 Then
                            Call GRT.agent_record.generate(signiture_key, idate_generated, json, JSS)
                        End If

                        Call GRT.set_block_number_and_get_previous_block_hash.exe()

                        Dim block_number = GRT.set_block_number_and_get_previous_block_hash.data.block_number
                        Dim previous_hash = GRT.set_block_number_and_get_previous_block_hash.data.previous_hash

                        If GRT.GR.node_level = 0 Then
                            Call GRT.agent_record.state_update("block_number_generated_" & block_number, "", signiture_key)
                        End If

                        pieces_alive_for_check = CInt(JRS_submit_info_verify("value")("pieces_alive"))

                        If pieces_alive_for_check >= pieces_to_buy Then ' 이렇게 하게 되면 동일인이 여러 지갑에서 동시에 tx 한 경우에도 처리가 가능하게 된다.

                            pure_query = GRT.GQS_nft_buy.exe(
                                    sell_order_block_number,
                                    owner_portion_block_number,
                                    nfa,
                                    token_id,
                                    eoa_seller,
                                    eoa_buyer,
                                    price,
                                    currency,
                                    gasPrice,
                                    gasLimit,
                                    pieces_to_buy,
                                    confirmed_type,
                                    na_buyer,
                                    days_span,
                                    closing_time_utc_string,
                                    transaction_fee_rate,
                                    max_split,
                                    idate_string)

                            verified = GRT.Security.Gverify.verify(pure_query, signiture, eoa_buyer)

                            If verified Then

                                Dim database_name As String
                                Dim table_name As String
                                Dim query_type As String
                                Dim contract_type As String

                                If currency = "ethereum" Then

                                    ' 이 경우 이더리움 밸런스 부족 문제는 발생할 수가 없다. 이미 송금실시는 끝난 상태이기 때문이다.
                                    ' 이미 amount 만큼 감소한 상태이므로 또 다시 밸런스 체킹을 하면 오류가 발생하게 된다.
                                    ' 물론 이미 발생한 gasPrice_for_cancel 지출은 기록해 주어야 한다.

                                    Dim ethereum_transaction_result As String = json("value")("ethereum_transaction_result").ToString
                                    Dim transaction_hash_initial As String = json("value")("transaction_hash_initial").ToString
                                    Dim transaction_hash_cancel As String = json("value")("transaction_hash_cancel").ToString
                                    Dim tem_transaction_success_initial As Boolean = CBool(json("value")("tem_transaction_success_initial"))
                                    Dim tem_transaction_success_cancel As Boolean = CBool(json("value")("tem_transaction_success_cancel"))

                                    If ethereum_transaction_result = "initial_success" Or ethereum_transaction_result = "transfer_ethereum_from_exchange_case_proceeding" Then

                                        database_name = "" : table_name = "" : query_type = "UPDATE, UPDATE, INSERT, UPDATE" : contract_type = command_key

                                        JRS = Await self_input_and_relay.exe(command_key, block_number, received_block_hash, previous_hash, eoa_buyer, database_name, table_name, query_type, contract_type, pure_query, signiture, JSS) ' 부모서버로부터 릴레이 받은 경우임.  

                                        Call agent_record_exe(JRS, block_number, signiture_key)

                                    Else ' cancel_success

                                        pure_query_for_ethereum_transfer_cancel = GRT.GQS_ethereum_transfer_cancel_treatment.exe(eoa_buyer, gasPrice_for_cancel, gasLimit, idate_string)

                                        verified = GRT.Security.Gverify.verify(pure_query_for_ethereum_transfer_cancel, signiture_for_ethereum_transfer_cancel, eoa_buyer)

                                        If verified Then

                                            database_name = "bc" : table_name = "account" : query_type = "UPDATE" : contract_type = command_key

                                            JRS = Await self_input_and_relay.exe(command_key, block_number, received_block_hash, previous_hash, eoa_buyer, database_name, table_name, query_type, contract_type, pure_query_for_ethereum_transfer_cancel, signiture_for_ethereum_transfer_cancel, JSS) ' 부모서버로부터 릴레이 받은 경우임.  

                                            Call agent_record_exe(JRS, block_number, signiture_key)

                                        Else
                                            JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", "데이터 변조", "quot"}}, False)
                                        End If

                                    End If

                                Else

                                    database_name = "" : table_name = "" : query_type = "UPDATE, UPDATE, INSERT, UPDATE" : contract_type = command_key

                                    JRS = Await self_input_and_relay.exe(command_key, block_number, received_block_hash, previous_hash, eoa_buyer, database_name, table_name, query_type, contract_type, pure_query, signiture, JSS) ' 부모서버로부터 릴레이 받은 경우임.  

                                    Call agent_record_exe(JRS, block_number, signiture_key)

                                End If

                            Else
                                JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", "데이터 변조", "quot"}}, False)
                            End If

                        Else
                            JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", "NFT 주문 수량초과입니다. 현재 주문 가능 조각수는 " & pieces_alive_for_check & " 개 입니다.", "quot"}}, False)
                        End If

                    End If

                Else
                    JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", JRS_submit_info_verify("value")("reason").ToString, "quot"}}, False)
                End If

            Else
                JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", "no_account", "quot"}}, False)
            End If

        Else

            JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", "idate_string 변조", "quot"}}, False)

        End If

        Return JRS

    End Function

    Shared Sub agent_record_exe(JRS As String, block_number As Long, signiture_key As String)

        Dim json_JRS = CType(JsonConvert.DeserializeObject(JRS), Linq.JObject)

        If json_JRS("success").ToString = "success" Then

            Call GRT.agent_record.confirm(block_number, signiture_key) ' 에이전트 서버가 최초로 접수한 경우와 메인서버인 두 경우만 빼고는 아무런 변화를 일으키지 않게 된다.

        End If

    End Sub

End Class
