Imports System.IO
Imports System.Numerics

Public Class treat_submit_nft_recall

    Public Shared Async Function exe(json As Newtonsoft.Json.Linq.JObject, JSS As String) As Task(Of String)

        Dim initial_transfer = CStr(json("value")("initial_transfer"))
        Dim command_key = CStr(json("key"))
        Dim received_block_hash = CStr(json("value")("block_hash"))
        Dim owner_portion_block_number = CLng(json("value")("owner_portion_block_number"))
        Dim sell_order_block_number = CLng(json("value")("sell_order_block_number"))
        Dim owner_portion_parent_block_number = CLng(json("value")("owner_portion_parent_block_number"))
        Dim nfa = CStr(json("value")("nfa"))
        Dim token_id = CInt(json("value")("token_id"))
        Dim pieces_to_recall = CInt(json("value")("pieces_to_recall"))
        Dim pieces_seized = CInt(json("value")("pieces_seized"))
        Dim confirmed_type = CStr(json("value")("confirmed_type"))
        Dim price_piece = CDec(json("value")("price_piece"))
        Dim price_sum = CDec(json("value")("price_sum"))
        Dim auto_recall = CBool(json("value")("auto_recall"))
        Dim nonce = CType(CLng(json("value")("nonce")), BigInteger)
        Dim gasPrice = CDec(json("value")("gasPrice_Gwei"))
        Dim gasLimit = CDec(json("value")("gasLimit"))
        Dim currency = CStr(json("value")("currency"))
        Dim max_split = CInt(json("value")("max_split"))
        Dim eoa_seller = CStr(json("value")("eoa_seller"))
        Dim eoa_recaller = CStr(json("value")("eoa_recaller"))
        Dim na_seller = CStr(json("value")("na_seller"))
        Dim exchange_name_seller = CStr(json("value")("exchange_name_seller"))
        Dim seller_agency_domain = CStr(json("value")("domain_seller"))
        Dim seller_agency_ip = CStr(json("value")("ip_seller"))
        Dim seller_agency_port = CInt(json("value")("port_seller"))
        Dim na_recaller = CStr(json("value")("na_recaller"))
        Dim exchange_name_recaller = CStr(json("value")("exchange_name_recaller"))
        Dim recaller_agency_domain = CStr(json("value")("domain_recaller"))
        Dim recaller_agency_ip = CStr(json("value")("ip_recaller"))
        Dim recaller_agency_port = CInt(json("value")("port_recaller"))
        Dim days_span = CInt(json("value")("days_span"))
        Dim closing_time_utc_string = CStr(json("value")("closing_time"))
        Dim transaction_fee_rate = CDec(json("value")("transaction_fee_rate"))
        Dim state = CStr(json("value")("state"))
        Dim idate_string = CStr(json("value")("idate_string"))
        Dim eoa_idate_string_signature = CStr(json("value")("idate_string_signature"))
        Dim signiture_data_for_ethereum_transfer_0 = CStr(json("value")("signiture_data_for_ethereum_transfer_0"))
        Dim signiture_data_for_ethereum_transfer_1 = CStr(json("value")("signiture_data_for_ethereum_transfer_1"))
        Dim pure_query = CStr(json("value")("pure_query"))
        Dim signiture = CStr(json("value")("signiture"))
        Dim signiture_key = CStr(json("value")("signiture_key"))
        'Dim pure_query_for_ethereum_transfer_not_succeeded_and_no_need_cancel_transfer = CStr(json("value")("pure_query_for_ethereum_transfer_not_succeeded_and_no_need_cancel_transfer"))
        'Dim signiture_for_ethereum_transfer_not_succeeded_and_no_need_cancel_transfer = CStr(json("value")("signiture_for_ethereum_transfer_not_succeeded_and_no_need_cancel_transfer"))

        Dim idate_generated = DateTime.Now

        Dim pieces_seized_for_check As Integer
        Dim JRS, error_message, na_idate_string_signature As String
        Dim balance_na_recaller As Decimal
        Dim verified, eoa_exist As Boolean
        Dim json_JRS, json_JRS_lock, JRS_submit_info_verify As Newtonsoft.Json.Linq.JObject

        verified = GRT.Security.Gverify.verify(idate_string, eoa_idate_string_signature, eoa_recaller)

        If verified Then

            eoa_exist = GRT.check_eoa_exist.exe(eoa_recaller)

            If eoa_exist Then

                JRS_submit_info_verify = check_submit_info_match.nft_recall(json)

                If JRS_submit_info_verify("success").ToString = "success" Then

                    If initial_transfer = "Y" Then

                        na_idate_string_signature = GRT.Security.Gsign.sign(idate_string, GRT.GR.server_private_key)
                        json("value")("na_idate_string_signature") = na_idate_string_signature

                        pure_query = GRT.GQS_nft_recall.exe(
                                                            owner_portion_block_number,
                                                            sell_order_block_number,
                                                            owner_portion_parent_block_number,
                                                            nfa,
                                                            token_id,
                                                            eoa_seller,
                                                            eoa_recaller,
                                                            price_sum,
                                                            auto_recall,
                                                            currency,
                                                            gasPrice,
                                                            gasLimit,
                                                            pieces_to_recall,
                                                            pieces_seized,
                                                            confirmed_type,
                                                            na_recaller,
                                                            na_seller,
                                                            days_span,
                                                            closing_time_utc_string,
                                                            transaction_fee_rate,
                                                            max_split,
                                                            idate_string)

                        verified = GRT.Security.Gverify.verify(pure_query, signiture, eoa_recaller)

                        If verified Then

                            Call GRT.agent_record.generate(signiture_key, idate_generated, json, JSS)

                            ' 가장 먼저 락을 걸어준다.
                            ' 연쇄적으로 리콜이 되는 경우라도 가장 첫번째만 락을 걸어주면 된다. 하위 수치들은 이미 안전하게 격리되어 있기 때문이다.
                            ' 또한 디폴트 서버는 철저히 동기 프로세스이므로 그 외의 다른 간섭도 없다.
                            json_JRS_lock = Await request_lock.lock(sell_order_block_number, signiture_key, "bc_nft", "sell_order")

                            If json_JRS_lock("success").ToString = "success" Then

                                Call GRT.agent_record.state_update("lock_request_response_received", "", signiture_key)

                                If Not json_JRS_lock("value")("lock").ToString = "no_rows" Then ' 로컬거래소에서는 있었는데 메인에서 없는 경우이다.

                                    Call GRT.agent_record.state_update("row_found", "", signiture_key)

                                    If json_JRS_lock("value")("lock").ToString = "open" Then ' 메인에서 락처리가 되고 state_open 을 0 으로 변경 후 "open"을 반환함.

                                        Call GRT.agent_record.state_update("lock_succeeded", "", signiture_key)

                                        json("value")("initial_transfer") = "N"

                                        ' 이더리움 송금은 필요하지 않다. deposit 로 처리하기 때문. 그러나 buy 의 경우는 즉각 송금처리하므로 이곳에서 이더리움 송금이 이루어져야 한다.
                                        ' 물론 근본적으로 deposit 시스템을 도입하여 모든 거래를 deposit 기반으로 처리할 수도 있다.
                                        ' 애당초 회원가입 시 deposit 를 미리 생성시키는 것이 바람직하다.
                                        ' clear_house 의 경우는 미리부터 생성해서는 안된다. 어중이 거래소 하나 가입할 때마다 10만개의 로우가 형성되기 때문이다.

                                        JSS = CType(JsonConvert.SerializeObject(json), String)

                                        JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "success", "quot"}}, {{"reason", "send_success_at_least", "quot"}}, False)

                                        Call send_main.exe(signiture_key, JSS) ' 메인으로 보낸다. 하등의 데이터베이스 처리 없이 보낸다.

                                    Else ' 이미 locked 인 경우이다. 스스로 락을 걸지 않았으므로 언락 역시 필요없다.

                                        Call GRT.agent_record.state_update("already_locked", "", signiture_key)

                                        JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", "현재 다른 사용자가 먼저 선택한 상태입니다.", "quot"}}, False)

                                    End If

                                Else ' 애당초 락을 걸수도 없었으므로 언락 역시 필요없다.

                                    Call GRT.agent_record.state_update("no_rows", "", signiture_key)

                                    JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", "이미 삭제되었습니다.", "quot"}}, False)

                                End If

                            Else ' 락이 실패했으므로 언락 역시 필요없다.

                                error_message = json_JRS_lock("value")("reason").ToString

                                Call GRT.agent_record.state_update("lock_failed", error_message, signiture_key)

                                JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", error_message, "quot"}}, False)

                            End If

                        Else
                            JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", "데이터 변조", "quot"}}, False)
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

                        pieces_seized_for_check = CInt(JRS_submit_info_verify("value")("pieces_seized"))

                        If pieces_seized_for_check >= pieces_to_recall Then

                            pure_query = GRT.GQS_nft_recall.exe(
                                                            owner_portion_block_number,
                                                            sell_order_block_number,
                                                            owner_portion_parent_block_number,
                                                            nfa,
                                                            token_id,
                                                            eoa_seller,
                                                            eoa_recaller,
                                                            price_sum,
                                                            auto_recall,
                                                            currency,
                                                            gasPrice,
                                                            gasLimit,
                                                            pieces_to_recall,
                                                            pieces_seized,
                                                            confirmed_type,
                                                            na_recaller,
                                                            na_seller,
                                                            days_span,
                                                            closing_time_utc_string,
                                                            transaction_fee_rate,
                                                            max_split,
                                                            idate_string)

                            verified = GRT.Security.Gverify.verify(pure_query, signiture, eoa_recaller)

                            If verified Then

                                Dim database_name As String
                                Dim table_name As String
                                Dim query_type As String
                                Dim contract_type As String

                                ' recall 과 confirm 의 경우는 밸런스 체킹을 하지 않는다.
                                ' 개런티의 경우는 미리 락을 걸어 놓으면 밸런스 체킹이 필요없다.
                                ' 이더리움 계정도 락을 설정하고 주기적으로 밸런스 체킹을 한다. 하지만 확실한 방법은 아니다.
                                ' 이더리움의 경우는 추후 deposit call 이나 clear 처리 시 이더리움 게스에서 지불불능이 될 수 있다.
                                ' 이 경우 deposit 나 clear 계정은 부족한 만큼 변경이 일어나지 않고 가능한 범위 내에서만 이행된다.
                                ' 이행되지 못한 부분은 언맷 필드에 기록된다.  이것이 존재하는 거래소는 이더리움 관련 신규 거래정지를 맞게 된다. 개런티는 애당초 이런 일이 생기지 않는다.
                                ' 빨간불 거래소의 경우는 컨펌 시에 deposit 를 형성시켜 주지 않는다. 반면 자신의 직전 거래소에 대해서는 채무부담을 하고 따라서 직전 거래소는 deposit 를 형성시킬 수 있게 된다.
                                ' 문제가 생긴 부분들은 각자가 알아서 처리하는 것으로 한다.
                                ' 그래도 큰 문제가 되지 않는 이유는 무수한 개인과 거래소들이 시시각각 call 을 하기 때문에 이더리움을 일시에 빼돌리기 위한 준비를 하기는 그리 쉽지 않다.

                                database_name = "" : table_name = "" : query_type = "UPDATE, UPDATE, INSERT, UPDATE" : contract_type = command_key

                                JRS = Await self_input_and_relay.exe(command_key, block_number, received_block_hash, previous_hash, eoa_recaller, database_name, table_name, query_type, contract_type, pure_query, signiture, JSS) ' 부모서버로부터 릴레이 받은 경우임.  

                                Call agent_record_exe(JRS, block_number, signiture_key)

                            Else
                                JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", "데이터 변조", "quot"}}, False)
                            End If

                        Else
                            JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", "NFT 리콜 수량초과입니다. 현재 리콜 가능 조각수는 " & pieces_seized_for_check & " 개 입니다.", "quot"}}, False)
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
