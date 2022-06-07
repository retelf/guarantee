Imports System.Net.Sockets
Imports System.Text

Public Class treat_transfer

    Public Shared Async Function exe(json As Newtonsoft.Json.Linq.JObject, JSS As String) As Task(Of String)

        Dim command_key, eoa_from, eoa_to, coin_name, signiture, signiture_key, signiture_for_ethereum_transfer_cancel, signiture_for_get_balance, JRS, idate_string As String
        Dim balance, amount, gasPrice, gasPrice_for_cancel, gasLimit As Decimal
        Dim registered, verified As Boolean
        Dim block_number As Long
        Dim pure_query, pure_query_for_ethereum_transfer_cancel, database_name, table_name, query_type, contract_type As String
        Dim received_block_hash, previous_hash As String
        Dim initial_transfer As String
        Dim idate_generated As DateTime
        Dim json_JRS As Newtonsoft.Json.Linq.JObject

        command_key = json("key").ToString

        received_block_hash = json("value")("block_hash").ToString
        eoa_from = json("value")("eoa_from").ToString
        coin_name = json("value")("coin_name").ToString

        registered = GRT.check_registered_eoa.exe(eoa_from, coin_name)

        If registered Then

            idate_generated = DateTime.Now

            eoa_to = json("value")("eoa_to").ToString
            amount = CDec(json("value")("amount"))
            gasPrice = CDec(json("value")("gasPrice")) ' 기위로 온다.
            gasPrice_for_cancel = CDec(json("value")("gasPrice_for_cancel"))
            gasLimit = CDec(json("value")("gasLimit"))
            signiture_for_get_balance = json("value")("signiture_for_get_balance").ToString
            signiture = json("value")("signiture").ToString
            signiture_key = Regex.Match(signiture, "^0x.{64}").ToString
            signiture_for_ethereum_transfer_cancel = json("value")("signiture_for_ethereum_transfer_cancel").ToString
            initial_transfer = json("value")("initial_transfer").ToString
            idate_string = json("value")("idate_string").ToString

            If coin_name = "ethereum" Then

                If initial_transfer = "Y" Then

                    balance = Await GRT.get_balance.ethereum(eoa_from)

                    If balance > amount + gasPrice * gasLimit / 1000000000 Then

                        pure_query = GRT.GQS_clear_deposit.exe(coin_name, eoa_from, eoa_to, amount, gasPrice, gasLimit, idate_string)

                        verified = GRT.Security.Gverify.verify(pure_query, signiture, eoa_from)

                        If verified Then

                            Call GRT.agent_record.generate(signiture_key, idate_generated, json, JSS)

                            json("value")("initial_transfer") = "N"

                            JRS = Await treat_ethereum_transfer_sub.exe(json) ' 이더리움 송금처리

                        Else
                            JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", "데이터 변조", "quot"}}, False)
                        End If

                    Else
                        JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", eoa_from & " 의 잔고가 부족합니다. 현재 잔고는 " & balance & " 입니다.", "quot"}}, False)
                    End If

                Else
                    ' 스스로 메인이거나 부모서버로부터 릴레이 받은 경우임.
                    ' 이미 이더리움 balance_for_cancel 여부의 체킹과 그에 기초한 실시가 끝났으므로 이곳에서 밸런스 부족 문제는 전혀 발생할 수가 없다.
                    ' 물론 단순 송금이 아닌 케이스의 경우 개런티 부분에 관해서는 부족 문제가 정상적으로 발생할 수 있다. 거래소의 개런티 잔고를 확인할 필요가 있기 때문이다.
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

                    If ethereum_transaction_result = "initial_success" Or ethereum_transaction_result = "transfer_ethereum_from_exchange_case_proceeding" Then

                        pure_query = GRT.GQS_clear_deposit.exe(coin_name, eoa_from, eoa_to, amount, gasPrice, gasLimit, idate_string)

                        verified = GRT.Security.Gverify.verify(pure_query, signiture, eoa_from)

                        If verified Then

                            database_name = "bc_manager"
                            table_name = "account"
                            query_type = "UPDATE"
                            contract_type = command_key

                            JRS = Await self_input_and_relay.exe(command_key, block_number, received_block_hash, previous_hash, eoa_from, database_name, table_name, query_type, contract_type, pure_query, signiture, JSS)

                            json_JRS = CType(JsonConvert.DeserializeObject(JRS), Linq.JObject)

                            If json_JRS("success").ToString = "success" Then

                                Call GRT.agent_record.confirm(block_number, signiture_key) ' 에이전트 서버가 최초로 접수한 경우와 메인서버인 두 경우만 빼고는 아무런 변화를 일으키지 않게 된다.

                            End If

                        Else
                            JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", "데이터 변조", "quot"}}, False)
                        End If

                    Else ' cancel_success

                        pure_query_for_ethereum_transfer_cancel = GRT.GQS_ethereum_transfer_cancel_treatment.exe(eoa_from, gasPrice_for_cancel, gasLimit, idate_string)

                        verified = GRT.Security.Gverify.verify(pure_query_for_ethereum_transfer_cancel, signiture_for_ethereum_transfer_cancel, eoa_from)

                        If verified Then

                            database_name = "bc"
                            table_name = "account"
                            query_type = "UPDATE"
                            contract_type = command_key

                            JRS = Await self_input_and_relay.exe(command_key, block_number, received_block_hash, previous_hash, eoa_from, database_name, table_name, query_type, contract_type, pure_query_for_ethereum_transfer_cancel, signiture_for_ethereum_transfer_cancel, JSS) ' 부모서버로부터 릴레이 받은 경우임.  

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

                balance = GRT.get_guarantee_balance_from_local_server.exe("foo", signiture_for_get_balance, eoa_from)

                If balance >= amount Then

                    pure_query = GRT.GQS_clear_deposit.exe(coin_name, eoa_from, eoa_to, amount, gasPrice, gasLimit, idate_string)

                    verified = GRT.Security.Gverify.verify(pure_query, signiture, eoa_from)

                    If verified Then

                        If initial_transfer = "Y" Then ' 에이전트 서버가 최초로 접수한 경우

                            json("value")("initial_transfer") = "N"

                            JSS = CType(JsonConvert.SerializeObject(json), String)

                            JRS = send_main.exe(signiture_key, JSS) ' 메인으로 보낸다. 하등의 데이터베이스 처리 없이 보낸다. 그러나 에이전트 레코드 기록은 남긴다.

                            json_JRS = CType(JsonConvert.DeserializeObject(JRS), Linq.JObject)

                            If json_JRS("success").ToString = "success" Then

                                Call GRT.agent_record.generate(Regex.Match(signiture, "^0x.{64}").ToString, idate_generated, json, JSS)

                            End If

                        Else

                            If GRT.GR.node_level = 0 Then
                                Call GRT.agent_record.generate(signiture_key, idate_generated, json, JSS)
                            End If

                            Call GRT.set_block_number_and_get_previous_block_hash.exe()

                            If GRT.GR.node_level = 0 Then
                                Call GRT.agent_record.state_update("block_number_generated_" & block_number, "", signiture_key)
                            End If

                            block_number = GRT.set_block_number_and_get_previous_block_hash.data.block_number
                            previous_hash = GRT.set_block_number_and_get_previous_block_hash.data.previous_hash
                            database_name = "bc_manager"
                            table_name = "account"
                            query_type = "UPDATE"
                            contract_type = command_key

                            JRS = Await self_input_and_relay.exe(command_key, block_number, received_block_hash, previous_hash, eoa_from, database_name, table_name, query_type, contract_type, pure_query, signiture, JSS) ' 부모서버로부터 릴레이 받은 경우임.  

                            Call GRT.agent_record.confirm(block_number, Regex.Match(signiture, "^0x.{64}").ToString)

                        End If

                    Else
                        JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", "데이터 변조", "quot"}}, False)
                    End If

                Else
                    JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", eoa_from & " 의 잔고가 부족합니다. 현재 잔고는 " & balance & " 입니다.", "quot"}}, False)
                End If

            End If

        Else
            JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", "no_account", "quot"}}, False)
        End If

        Return JRS

    End Function

End Class
