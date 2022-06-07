Imports System.Net.Sockets
Imports System.Text

Public Class treat_multilevel_submit_refund

    Public Shared Async Function exe(json As Newtonsoft.Json.Linq.JObject, JSS As String) As Task(Of String)

        Dim command_key, eoa_guarantee_seller, seller_na, signiture, signiture_key, signiture_for_get_balance, JRS, JSS_lock, JRS_lock, idate_string, error_message As String
        Dim balance_na As Decimal ' 에이전시
        Dim registered, verified As Boolean
        Dim block_number, sell_order_block_number As Long
        Dim seller_agency_domain, seller_agency_ip As String
        Dim seller_agency_port As Integer
        Dim pure_query, database_name, table_name, query_type, contract_type As String
        Dim received_block_hash, previous_hash As String
        Dim initial_transfer As String
        Dim idate_generated As DateTime
        Dim json_JRS_lock, json_JRS As Linq.JObject

        command_key = json("key").ToString

        sell_order_block_number = CLng(json("value")("sell_order_block_number"))
        received_block_hash = json("value")("block_hash").ToString
        eoa_guarantee_seller = json("value")("eoa_guarantee_seller").ToString
        seller_na = json("value")("seller_na").ToString
        seller_agency_domain = json("value")("seller_agency_domain").ToString
        seller_agency_ip = json("value")("seller_agency_ip").ToString
        seller_agency_port = CInt(json("value")("seller_agency_port"))

        registered = GRT.check_registered_eoa.exe(eoa_guarantee_seller, "guarantee")

        If registered Then

            signiture = json("value")("signiture").ToString
            signiture_key = Regex.Match(signiture, "^0x.{64}").ToString
            initial_transfer = json("value")("initial_transfer").ToString
            idate_string = json("value")("idate_string").ToString

            idate_generated = DateTime.Now

            ' 가장 먼저 보내온 정보들이 진정한 정보인지 확인. 비록 eoa_guarantee_seller 스스로가 스스로에 관한 정보를 보낸 것이기는 하지만 자신이 아닌 자의 계정에서 금원을 빼 내고 자신의 계정의 금원을 증가시키는 쿼리이므로.

            Dim JRS_submit_info_verify = check_submit_info_match.multilevel_refund(json)

            If JRS_submit_info_verify("success").ToString = "success" Then

                If initial_transfer = "Y" Then

                    signiture_for_get_balance = GRT.Security.Gsign.sign("foo", GRT.GR.server_private_key)
                    ' 개런티 밸런스 확인을 위한 것이다. 개런티는 최초 셀러의 에이전시에 보관되며 지금 이 서버가 셀러에이전시 서버이다.
                    json("value")("signiture_for_get_balance") = signiture_for_get_balance

                Else
                    signiture_for_get_balance = json("value")("signiture_for_get_balance").ToString
                End If

                balance_na = GRT.get_guarantee_balance_from_local_server.exe("foo", signiture_for_get_balance, seller_na)

                If balance_na >= 1 Then

                    pure_query = GRT.GQS_multilevel_refund.exe(sell_order_block_number, eoa_guarantee_seller, seller_na, 1, idate_string)

                    verified = GRT.Security.Gverify.verify(pure_query, signiture, eoa_guarantee_seller)

                    If verified Then

                        If initial_transfer = "Y" Then ' 에이전트 서버가 최초로 접수한 경우. 

                            Call GRT.agent_record.generate(signiture_key, idate_generated, json, JSS)

                            Call GRT.agent_record.state_update("lock_requested", "", signiture_key)

                            json("value")("initial_transfer") = "N"
                            JSS = CType(JsonConvert.SerializeObject(json), String)

                            ' 가장 먼저 락을 걸어준다.

                            JSS_lock = GRT.make_json_string.exe(
                                    {{"key", "set_lock", "quot"}},
                                    {
                                    {"block_number", CStr(sell_order_block_number), "non_quot"},
                                    {"database", "bc_multilevel", "quot"},
                                    {"table", "sell_order", "quot"}}, False)

                            JRS_lock = Await GRT.socket_client.exe(GRT.GR.main_server_address_agency, GRT.GR.port_number_server_main_agency, GRT.GR.port_number_server_local, JSS_lock)

                            json_JRS_lock = CType(JsonConvert.DeserializeObject(JRS_lock), Linq.JObject)

                            If json_JRS_lock("success").ToString = "success" Then

                                Call GRT.agent_record.state_update("lock_request_response_received", "", signiture_key)

                                If Not json_JRS_lock("value")("lock").ToString = "no_rows" Then ' 로컬거래소에서는 있었는데 메인에서 없는 경우이다.

                                    Call GRT.agent_record.state_update("row_found", "", signiture_key)

                                    If json_JRS_lock("value")("lock").ToString = "alive" Then ' 메인에서 락처리가 됨.

                                        Call GRT.agent_record.state_update("lock_succeeded", "", signiture_key)

                                        JRS = send_main.exe(signiture_key, JSS) ' 메인으로 보낸다. 하등의 데이터베이스 처리 없이 보낸다. 그러나 에이전트 레코드 기록은 남긴다.

                                        Call GRT.agent_record.state_update("send_main_JRS_received", "", signiture_key)

                                        json_JRS = CType(JsonConvert.DeserializeObject(JRS), Linq.JObject)

                                        If json_JRS("success").ToString = "success" Then ' 락을 풀지 않더라도 기본적으로 UPDATE sell_order SET `state` = 'refunded' 가 되어 있으므로 특별한 문제는 없다..

                                            Call GRT.agent_record.state_update("send_main_succeeded", "", signiture_key)

                                        Else

                                            Call GRT.agent_record.state_update("send_main_fail", json_JRS("value")("reason").ToString, signiture_key)

                                            ' 이는 송신장애는 아니라는 뜻이다.
                                            ' 메인에서 문제가 발생한 경우 락이 걸린 상태에서 로우는 영구히 남게 된다.
                                            ' 이 경우를 깊이 생각해 보아야 한다.
                                            ' "문제가 생겼습니다. 거래소에 연락하시기 바랍니다." 멘트를 띄워 주어야 하는 것으로 보인다. 이런 경우가 발생하는 것은 어쩔 수 없다.

                                        End If

                                    Else

                                        Call GRT.agent_record.state_update("already_locked", "", signiture_key)

                                        JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", "현재 다른 사용자가 먼저 선택한 상태입니다.", "quot"}}, False)

                                    End If

                                Else

                                    Call GRT.agent_record.state_update("no_rows", "", signiture_key)

                                    JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", "이미 삭제되었습니다.", "quot"}}, False)

                                End If

                            Else

                                error_message = json_JRS_lock("value")("reason").ToString

                                Call GRT.agent_record.state_update("lock_failed", error_message, signiture_key)

                                JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", error_message, "quot"}}, False)

                            End If

                        Else ' 스스로 메인이거나 부모서버로부터 릴레이 받은 경우임. 

                            Call GRT.set_block_number_and_get_previous_block_hash.exe()

                            block_number = GRT.set_block_number_and_get_previous_block_hash.data.block_number
                            previous_hash = GRT.set_block_number_and_get_previous_block_hash.data.previous_hash
                            database_name = "bc, bc, bc_multilevel"
                            table_name = "account, account, sell_order"
                            query_type = "UPDATE, UPDATE, UPDATE"
                            contract_type = command_key

                            If GRT.GR.node_level = 0 Then
                                Call GRT.agent_record.state_update("block_number_generated_" & block_number, "", signiture_key)
                            End If

                            JRS = Await self_input_and_relay.exe(command_key, block_number, received_block_hash, previous_hash, eoa_guarantee_seller, database_name, table_name, query_type, contract_type, pure_query, signiture, JSS) ' 부모서버로부터 릴레이 받은 경우임.  

                            json_JRS = CType(JsonConvert.DeserializeObject(JRS), Linq.JObject)

                            If json_JRS("success").ToString = "success" Then

                                Call GRT.agent_record.confirm(block_number, signiture_key) ' 에이전트 서버가 최초로 접수한 경우와 메인서버인 두 경우만 빼고는 아무런 변화를 일으키지 않게 된다.

                            End If

                        End If

                    Else
                        JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", "데이터 변조", "quot"}}, False)
                    End If

                Else

                    JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", "거래소 " & seller_na & " 의 잔고가 부족합니다. 현재 잔고는 " & balance_na & " 입니다.", "quot"}}, False)

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
