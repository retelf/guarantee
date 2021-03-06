Imports System.IO
Imports System.Numerics

Public Class treat_submit_nft_confirm

    Public Shared Async Function exe(json As Newtonsoft.Json.Linq.JObject, JSS As String) As Task(Of String)

        Dim initial_transfer = CStr(json("value")("initial_transfer"))
        Dim command_key = CStr(json("key"))
        Dim received_block_hash = CStr(json("value")("block_hash"))
        Dim owner_portion_block_number = CLng(json("value")("owner_portion_block_number"))
        Dim sell_order_block_number = CLng(json("value")("sell_order_block_number"))
        Dim owner_portion_parent_block_number = CLng(json("value")("owner_portion_parent_block_number"))
        Dim nfa = CStr(json("value")("nfa"))
        Dim token_id = CInt(json("value")("token_id"))
        Dim pieces_to_confirm = CInt(json("value")("pieces_to_confirm"))
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
        Dim eoa_confirmer = CStr(json("value")("eoa_confirmer"))
        Dim na_seller = CStr(json("value")("na_seller"))
        Dim exchange_name_seller = CStr(json("value")("exchange_name_seller"))
        Dim seller_agency_domain = CStr(json("value")("domain_seller"))
        Dim seller_agency_ip = CStr(json("value")("ip_seller"))
        Dim seller_agency_port = CInt(json("value")("port_seller"))
        Dim na_confirmer = CStr(json("value")("na_confirmer"))
        Dim exchange_name_confirmer = CStr(json("value")("exchange_name_confirmer"))
        Dim confirmer_agency_domain = CStr(json("value")("domain_confirmer"))
        Dim confirmer_agency_ip = CStr(json("value")("ip_confirmer"))
        Dim confirmer_agency_port = CInt(json("value")("port_confirmer"))
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
        Dim verified, eoa_exist As Boolean
        Dim json_JRS_lock, JRS_submit_info_verify As Newtonsoft.Json.Linq.JObject

        verified = GRT.Security.Gverify.verify(idate_string, eoa_idate_string_signature, eoa_confirmer)

        If verified Then

            eoa_exist = GRT.check_eoa_exist.exe(eoa_confirmer)

            If eoa_exist Then

                JRS_submit_info_verify = check_submit_info_match.nft_confirm(json)

                If JRS_submit_info_verify("success").ToString = "success" Then

                    If initial_transfer = "Y" Then

                        na_idate_string_signature = GRT.Security.Gsign.sign(idate_string, GRT.GR.server_private_key)
                        json("value")("na_idate_string_signature") = na_idate_string_signature

                        pure_query = GRT.GQS_nft_confirm.exe(command_key,
                                                            owner_portion_block_number,
                                                            sell_order_block_number,
                                                            owner_portion_parent_block_number,
                                                            eoa_seller,
                                                            price_sum,
                                                            currency,
                                                            pieces_to_confirm,
                                                            confirmed_type,
                                                            na_confirmer,
                                                            na_seller,
                                                            idate_string)

                        verified = GRT.Security.Gverify.verify(pure_query, signiture, eoa_confirmer)

                        If verified Then

                            ' deposit 방식으로 정산하기 때문에 이더리움 거래라 할 지라도 락을 걸 필요가 없다.
                            ' 다만 메인에서 전체거래 또는 수량이 부족한 만큼 거래를 불성립시키고 리턴하는 방식으로 처리해야 한다. 즉 If pieces_seized_for_check >= pieces_to_confirm Then 가 필요. 바로 아래에 그 구현이 존재한다.
                            ' 이렇게 하게 되면 recall 이나 confirm 의 경우 동일인이 여러 지갑에서 tx를 날린 경우에도 문제가 없게 된다.

                            Call GRT.agent_record.generate(signiture_key, idate_generated, json, JSS)
                            Call GRT.agent_record.state_update("no_lock_needed_case", "", signiture_key)

                            json("value")("initial_transfer") = "N"

                            ' 이더리움 송금은 필요하지 않다. deposit 로 처리하기 때문. 그러나 buy 의 경우는 즉각 송금처리하므로 이곳에서 이더리움 송금이 이루어져야 한다.
                            ' 물론 근본적으로 deposit 시스템을 도입하여 모든 거래를 deposit 기반으로 처리할 수도 있다.
                            ' 애당초 회원가입 시 deposit 를 미리 생성시키는 것이 바람직하다.
                            ' clear_house 의 경우는 미리부터 생성해서는 안된다. 어중이 거래소 하나 가입할 때마다 10만개의 로우가 형성되기 때문이다.

                            JSS = CType(JsonConvert.SerializeObject(json), String)

                            JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "success", "quot"}}, {{"reason", "send_success_at_least", "quot"}}, False)

                            Call send_main.exe(signiture_key, JSS) ' 메인으로 보낸다. 하등의 데이터베이스 처리 없이 보낸다.

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

                        If pieces_seized_for_check >= pieces_to_confirm Then ' 이 조건이 만족되지 않는 경우는 애당초 하위 서버로 릴레이가 되지 않고 리턴된다.

                            pure_query = GRT.GQS_nft_confirm.exe(command_key,
                                                                owner_portion_block_number,
                                                                sell_order_block_number,
                                                                owner_portion_parent_block_number,
                                                                eoa_seller,
                                                                price_sum,
                                                                currency,
                                                                pieces_to_confirm,
                                                                confirmed_type,
                                                                na_confirmer,
                                                                na_seller,
                                                                idate_string)

                            verified = GRT.Security.Gverify.verify(pure_query, signiture, eoa_confirmer)

                            If verified Then

                                Dim database_name As String
                                Dim table_name As String
                                Dim query_type As String
                                Dim contract_type As String

                                database_name = "" : table_name = "" : query_type = "UPDATE, UPDATE, INSERT, UPDATE" : contract_type = command_key

                                JRS = Await self_input_and_relay.exe(command_key, block_number, received_block_hash, previous_hash, eoa_confirmer, database_name, table_name, query_type, contract_type, pure_query, signiture, JSS) ' 부모서버로부터 릴레이 받은 경우임.  

                                Call agent_record_exe(JRS, block_number, signiture_key)

                            Else
                                JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", "데이터 변조", "quot"}}, False)
                            End If

                        Else
                            JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", "NFT 리콜 수량초과 또는 다른 주문에 의해 이미 처리된 상태입니다. 현재 리콜 가능 조각수는 " & pieces_seized_for_check & " 개 입니다.", "quot"}}, False)
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
