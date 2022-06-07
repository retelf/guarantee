Imports System.IO
Imports System.Numerics

Public Class treat_submit_load_nft

    Public Shared Async Function exe(json As Newtonsoft.Json.Linq.JObject, JSS As String) As Task(Of String)

        Dim JRS As String
        Dim command_key = CStr(json("key"))
        Dim received_block_hash = CStr(json("value")("block_hash"))
        Dim source = CStr(json("value")("source"))
        Dim new_nfa = CBool(json("value")("new_nfa"))
        Dim nfa = CStr(json("value")("nfa"))
        Dim token_id = CInt(json("value")("token_id"))
        Dim name = CStr(json("value")("name"))
        Dim sub_name = CStr(json("value")("sub_name"))
        Dim character = CStr(json("value")("character"))
        Dim nft_type = CStr(json("value")("nft_type"))
        Dim url = CStr(json("value")("url"))
        Dim creator = CStr(json("value")("creator"))
        Dim sub_creator = CStr(json("value")("sub_creator"))
        Dim personal_name = CStr(json("value")("personal_name"))
        Dim extension = CStr(json("value")("extension"))
        Dim file_length = CLng(json("value")("file_length"))
        Dim sell_order_right_now = CBool(json("value")("sell_order_right_now"))
        Dim price_piece = CDec(json("value")("price_piece"))
        Dim price_total = CDec(json("value")("price_total"))
        Dim splitable = CBool(json("value")("splitable"))
        Dim max_split = CInt(json("value")("max_split"))
        Dim file_copiable = CBool(json("value")("file_copiable"))
        Dim materialable = CBool(json("value")("materialable"))
        Dim max_material_count = CInt(json("value")("max_material_count"))
        Dim terms_modifiable = CBool(json("value")("terms_modifiable"))
        Dim copyright_transfer = CBool(json("value")("copyright_transfer"))
        Dim pollable = CBool(json("value")("pollable"))
        Dim min_price = CDec(json("value")("min_price"))
        Dim quorum_proposal = CSng(json("value")("quorum_proposal"))
        Dim quorum_conference = CSng(json("value")("quorum_conference"))
        Dim quorum_resolution = CSng(json("value")("quorum_resolution"))
        Dim poll_notice_days_span = CInt(json("value")("poll_notice_days_span"))
        Dim poll_days_span = CInt(json("value")("poll_days_span"))
        Dim general_terms = CStr(json("value")("general_terms"))
        Dim individual_terms = CStr(json("value")("individual_terms"))
        Dim nft_recall_days_span = CInt(json("value")("nft_recall_days_span"))
        Dim closing_time_utc_string = CStr(json("value")("closing_time_utc_string"))
        Dim eoa = CStr(json("value")("eoa"))
        Dim na = CStr(json("value")("na"))
        Dim exchange_name = CStr(json("value")("exchange_name"))
        Dim agency_domain = CStr(json("value")("agency_domain"))
        Dim agency_ip = CStr(json("value")("agency_ip"))
        Dim agency_port = CInt(json("value")("agency_port"))
        Dim agency_port_nft = CStr(json("value")("agency_port_nft"))
        Dim pure_query = CStr(json("value")("pure_query"))
        Dim signiture = CStr(json("value")("signiture"))
        Dim signiture_key = CStr(json("value")("signiture_key"))
        Dim initial_transfer = CStr(json("value")("initial_transfer"))
        Dim idate_string = CStr(json("value")("idate_string"))

        Dim creator_profile = CStr(json("value")("creator_profile"))
        Dim sub_creator_profile = CStr(json("value")("sub_creator_profile"))
        Dim main_description = CStr(json("value")("main_description"))
        Dim sub_description = CStr(json("value")("sub_description"))
        Dim name_critic = CStr(json("value")("name_critic"))
        Dim critic = CStr(json("value")("critic"))

        Dim eoa_exist = GRT.check_eoa_exist.exe(eoa)

        If eoa_exist Then

            If initial_transfer = "Y" Then ' 에이전트 서버가 최초로 접수한 경우. eoa 진정성과 파일 확인만 한다.

                ' send_main 의 전초적인 작업만 하는 것이므로 이곳에서 검증은 필요없다. 
                ' 스스로 메인이거나 부모서버로부터 릴레이 받은 경우에만 검증이 필요할 뿐이다.

                ' 파일 존재 확인

                Dim full_name = GRT.GR.nft.destination_folder & "\" & nfa & "\" & nfa & "_" & token_id & "_" & name & "_" & sub_name & "." & extension

                Dim file_info As New FileInfo(full_name)

                Dim count As Integer = 0
                Dim result As String

                While True

                    If file_info.Exists Then

                        If file_info.Length = file_length Then

                            result = "send_success_at_least"

                        Else

                            result = "file_brocken"

                            count += 1

                        End If

                    Else

                        result = "file_not_found"

                        count += 1

                    End If

                    If result = "file_brocken" Or result = "file_not_found" Then

                        If count < 100 Then

                            Threading.Thread.Sleep(100)

                        Else

                            JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", result, "quot"}}, False)

                            Exit While

                        End If

                    Else

                        json("value")("initial_transfer") = "N" : JSS = CType(JsonConvert.SerializeObject(json), String)

                        JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "success", "quot"}}, {{"reason", result, "quot"}}, False)

                        Call send_main.exe(signiture_key, JSS) ' 메인으로 보낸다. 하등의 데이터베이스 처리 없이 보낸다.

                        Exit While

                    End If

                End While

            Else
                ' 스스로 메인이거나 부모서버로부터 릴레이 받은 경우임. 

                Call GRT.set_block_number_and_get_previous_block_hash.exe()

                Dim block_number = GRT.set_block_number_and_get_previous_block_hash.data.block_number
                Dim previous_hash = GRT.set_block_number_and_get_previous_block_hash.data.previous_hash

                pure_query = GRT.GQS_nft_initial_deploy.exe(
                                                new_nfa,
                                                nfa,
                                                eoa,
                                                name,
                                                sub_name,
                                                sub_creator,
                                                personal_name,
                                                character,
                                                nft_type,
                                                price_piece,
                                                price_total,
                                                extension,
                                                file_length,
                                                sell_order_right_now,
                                                max_split,
                                                general_terms,
                                                individual_terms,
                                                token_id,
                                                url,
                                                na,
                                                exchange_name,
                                                agency_domain,
                                                agency_ip,
                                                agency_port,
                                                nft_recall_days_span,
                                                closing_time_utc_string,
                                                splitable,
                                                max_split,
                                                file_copiable,
                                                materialable,
                                                max_material_count,
                                                min_price,
                                                terms_modifiable,
                                                copyright_transfer,
                                                pollable,
                                                quorum_proposal,
                                                quorum_conference,
                                                quorum_resolution,
                                                poll_notice_days_span,
                                                poll_days_span,
                                                idate_string,
                                                creator_profile,
                                                sub_creator_profile,
                                                main_description,
                                                sub_description,
                                                name_critic,
                                                critic)

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
