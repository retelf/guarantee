Imports System.IO
Imports System.Numerics

Public Class treat_submit_nft_load_order

    Public Shared Async Function exe(json As Newtonsoft.Json.Linq.JObject, JSS As String) As Task(Of String)

        Dim JRS As String
        Dim initial_transfer = CStr(json("value")("initial_transfer"))
        Dim command_key = CStr(json("key"))
        Dim received_block_hash = CStr(json("value")("block_hash"))
        Dim owner_portion_block_number = CLng(json("value")("owner_portion_block_number"))
        Dim nfa = CStr(json("value")("nfa"))
        Dim token_id = CInt(json("value")("token_id"))
        Dim eoa = CStr(json("value")("eoa"))
        Dim price_piece = CDec(json("value")("price_piece"))
        Dim price_total = CDec(json("value")("price_total"))
        Dim auto_recall = CBool(json("value")("auto_recall"))
        Dim currency = CStr(json("value")("currency"))
        Dim sell_pieces = CInt(json("value")("sell_pieces"))
        Dim confirmed_type = CStr(json("value")("confirmed_type"))
        Dim max_split = CInt(json("value")("max_split"))
        Dim days_span = CInt(json("value")("days_span"))
        Dim closing_time_utc_string = CStr(json("value")("closing_time_utc_string"))
        Dim na = CStr(json("value")("na"))
        Dim exchange_name = CStr(json("value")("exchange_name"))
        Dim seller_agency_domain = CStr(json("value")("seller_agency_domain"))
        Dim seller_agency_ip = CStr(json("value")("seller_agency_ip"))
        Dim seller_agency_port = CInt(json("value")("seller_agency_port"))
        Dim pure_query = CStr(json("value")("pure_query"))
        Dim signiture = CStr(json("value")("signiture"))
        Dim signiture_key = CStr(json("value")("signiture_key"))
        Dim idate_string = CStr(json("value")("idate_string"))

        Dim idate_generated = DateTime.Now

        Dim eoa_exist = GRT.check_eoa_exist.exe(eoa)

        If eoa_exist Then

            If initial_transfer = "Y" Then ' 에이전트 서버가 최초로 접수한 경우. eoa 진정성과 파일 확인만 한다.

                ' send_main 의 전초적인 작업만 하는 것이므로 이곳에서 검증은 필요없다. 
                ' 스스로 메인이거나 부모서버로부터 릴레이 받은 경우에만 검증이 필요할 뿐이다.

                json("value")("initial_transfer") = "N" : JSS = CType(JsonConvert.SerializeObject(json), String)

                JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "success", "quot"}}, {{"reason", "send_success_at_least", "quot"}}, False)

                Call send_main.exe(signiture_key, JSS) ' 메인으로 보낸다. 하등의 데이터베이스 처리 없이 보낸다.

            Else
                ' 스스로 메인이거나 부모서버로부터 릴레이 받은 경우임. 

                Call GRT.set_block_number_and_get_previous_block_hash.exe()

                Dim block_number = GRT.set_block_number_and_get_previous_block_hash.data.block_number
                Dim previous_hash = GRT.set_block_number_and_get_previous_block_hash.data.previous_hash

                pure_query = GRT.GQS_nft_load_order.exe(
                                                        owner_portion_block_number,
                                                        nfa,
                                                        eoa,
                                                        price_piece,
                                                        price_total,
                                                        auto_recall,
                                                        currency,
                                                        sell_pieces,
                                                        confirmed_type,
                                                        token_id,
                                                        na,
                                                        exchange_name,
                                                        seller_agency_domain,
                                                        seller_agency_ip,
                                                        seller_agency_port,
                                                        days_span,
                                                        closing_time_utc_string,
                                                        max_split,
                                                        idate_string)

                Dim verified = GRT.Security.Gverify.verify(pure_query, signiture, eoa)

                If verified Then

                    Dim database_name = ""
                    Dim table_name = ""
                    Dim query_type = "INSERT, UPDATE"
                    Dim contract_type = command_key

                    JRS = Await self_input_and_relay.exe(command_key, block_number, received_block_hash, previous_hash, eoa, database_name, table_name, query_type, contract_type, pure_query, signiture, JSS) ' 부모서버로부터 릴레이 받은 경우임.  

                    'Dim json_JRS = CType(JsonConvert.DeserializeObject(JRS), Linq.JObject)

                    'If json_JRS("success").ToString = "success" Then

                    '    Call GRT.agent_record.confirm(block_number, signiture_key) ' 에이전트 서버가 최초로 접수한 경우와 메인서버인 두 경우만 빼고는 아무런 변화를 일으키지 않게 된다.

                    'End If

                Else
                    JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", "데이터 변조", "quot"}}, False)
                End If

            End If

        Else
            JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", "no_account", "quot"}}, False)
        End If

        Return JRS

    End Function

End Class
