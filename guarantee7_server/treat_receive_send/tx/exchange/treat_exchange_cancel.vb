Imports System.Net.Sockets
Imports System.Numerics
Imports System.Text

Public Class treat_exchange_cancel

    Public Shared Async Function exe(json As Newtonsoft.Json.Linq.JObject, JSS As String) As Task(Of String)

        Dim command_key, eoa, na, coin_name_from, coin_name_to, state, signiture, signiture_na, signiture_for_ethereum_transfer_cancel, signiture_key, signiture_for_get_balance, signiture_data_for_ethereum_transfer(1), JRS, idate_string As String
        Dim eoa_checked, na_checked, coin_name_from_checked As String
        Dim amount_checked, exchange_fee_rate_checked As Decimal
        Dim balance, amount, gasPrice, gasPrice_for_cancel, gasLimit, exchange_rate, exchange_fee_rate As Decimal
        Dim registered, verified, verified_eoa, verified_na As Boolean
        Dim block_number, cancel_block_number As Long
        Dim seller_agency_domain, seller_agency_ip As String
        Dim seller_agency_port As Integer
        Dim pure_query, pure_query_for_ethereum_transfer_cancel, database_name, table_name, query_type, contract_type As String
        Dim received_block_hash, previous_hash As String
        Dim initial_transfer As String
        Dim receipt As Nethereum.RPC.Eth.DTOs.TransactionReceipt
        Dim idate_generated As DateTime
        Dim json_JRS As Newtonsoft.Json.Linq.JObject
        Dim dataset_exchange_info As DataSet
        Dim gas_price_biginteger As BigInteger

        command_key = json("key").ToString

        received_block_hash = json("value")("block_hash").ToString
        cancel_block_number = CLng(json("value")("cancel_block_number"))
        eoa = json("value")("eoa").ToString
        na = json("value")("na").ToString
        seller_agency_domain = json("value")("seller_agency_domain").ToString
        seller_agency_ip = json("value")("seller_agency_ip").ToString
        seller_agency_port = CInt(json("value")("seller_agency_port"))
        coin_name_from = json("value")("coin_name_from").ToString
        coin_name_to = json("value")("coin_name_to").ToString

        registered = GRT.check_registered_eoa.exe(eoa, coin_name_from)

        If registered Then

            amount = CDec(json("value")("amount"))
            gasPrice = CDec(json("value")("gasPrice"))
            gasPrice_for_cancel = CDec(json("value")("gasPrice_for_cancel"))
            gasLimit = CDec(json("value")("gasLimit"))
            exchange_rate = CDec(json("value")("exchange_rate"))
            exchange_fee_rate = CDec(json("value")("exchange_fee_rate"))
            state = json("value")("state").ToString
            signiture_for_get_balance = json("value")("signiture_for_get_balance").ToString
            signiture_data_for_ethereum_transfer(0) = json("value")("signiture_data_for_ethereum_transfer").ToString
            signiture_data_for_ethereum_transfer(1) = json("value")("signiture_data_for_ethereum_transfer_cancel").ToString
            signiture = json("value")("signiture").ToString
            signiture_for_ethereum_transfer_cancel = json("value")("signiture_for_ethereum_transfer_cancel").ToString
            signiture_na = json("value")("signiture_na").ToString
            signiture_key = Regex.Match(signiture, "^0x.{64}").ToString

            initial_transfer = json("value")("initial_transfer").ToString
            idate_string = json("value")("idate_string").ToString

            idate_generated = DateTime.Now

            Dim JRS_submit_info_verify = check_submit_info_match.exchange_cancel(json) ' 자신의 데이터베이스를 직접 확인해야 한다.

            If JRS_submit_info_verify("success").ToString = "success" Then

                dataset_exchange_info = GRT.get_exchange_info_individual.exe(cancel_block_number)

                If dataset_exchange_info.Tables(0).Rows.Count = 1 Then

                    eoa_checked = CStr(dataset_exchange_info.Tables(0).Rows(0)("eoa"))
                    na_checked = CStr(dataset_exchange_info.Tables(0).Rows(0)("na"))
                    coin_name_from_checked = CStr(dataset_exchange_info.Tables(0).Rows(0)("coin_name_from"))
                    amount_checked = CDec(dataset_exchange_info.Tables(0).Rows(0)("amount"))
                    exchange_fee_rate_checked = CDec(dataset_exchange_info.Tables(0).Rows(0)("exchange_fee_rate"))

                    If Regex.Replace(eoa, "^0x", "") = eoa_checked And
                        Regex.Replace(na, "^0x", "") = na_checked And
                        coin_name_from = coin_name_from_checked And
                        amount = amount_checked And
                        exchange_fee_rate = exchange_fee_rate_checked Then

                        If coin_name_from = "ethereum" Then

                            If initial_transfer = "Y" Then

                                balance = Await GRT.get_balance.ethereum(na)

                                If balance >= amount + gasPrice * gasLimit / 1000000000 Then

                                    pure_query = GRT.GQS_submit_cancel.exe(cancel_block_number, eoa, na, coin_name_from, amount, exchange_fee_rate, gasPrice, gasLimit)

                                    verified = GRT.Security.Gverify.verify(pure_query, signiture, eoa) ' 1차검증

                                    If verified Then

                                        json("value")("initial_transfer") = "N"
                                        json("value")("signiture_for_get_balance") = GRT.Security.Gsign.sign("foo", GRT.GR.server_private_key)
                                        json("value")("signiture_na") = GRT.Security.Gsign.sign(pure_query, GRT.GR.server_private_key) ' 제2차검증자료 생성. 메인에서부터 이 두가지 모두를 검증한다.

                                        pure_query_for_ethereum_transfer_cancel = GRT.GQS_ethereum_transfer_cancel_treatment.exe(na, gasPrice_for_cancel, gasLimit, idate_string) ' 이것은 여기서 검증할 필요 없다. 스스로만 관계되어 있으므로.

                                        json("value")("signiture_for_ethereum_transfer_cancel") = GRT.Security.Gsign.sign(pure_query_for_ethereum_transfer_cancel, GRT.GR.server_private_key)

                                        gas_price_biginteger = CType(gasPrice * 1000000000, BigInteger)

                                        Dim unused = Await GRT.get_signiture_data_for_ethereum_transfer.exe("server", command_key, na, GRT.GR.server_private_key, eoa, amount * exchange_rate, exchange_fee_rate, gas_price_biginteger, CType(gasLimit, Numerics.BigInteger))

                                        signiture_data_for_ethereum_transfer = GRT.get_signiture_data_for_ethereum_transfer.info.signiture_data_for_ethereum_transfer

                                        json("value")("signiture_data_for_ethereum_transfer") = signiture_data_for_ethereum_transfer(0)
                                        json("value")("signiture_data_for_ethereum_transfer_cancel") = signiture_data_for_ethereum_transfer(1)
                                        json("value")("nonce_biginteger") = GRT.get_signiture_data_for_ethereum_transfer.info.nonce.Value.ToString

                                        'JSS = CType(JsonConvert.SerializeObject(json), String) 

                                        JRS = Await treat_ethereum_transfer_for_exchange_cancel.exe(json) ' 이더리움 송금처리

                                    Else
                                        JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", "데이터 변조", "quot"}}, False)
                                    End If

                                Else
                                    JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", "거래소(" & na & ") 의 " & coin_name_from & " 잔고가 부족합니다. 현재 잔고는 " & balance & " 입니다.", "quot"}}, False)
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

                                Dim ethereum_transaction_result As String = json("value")("ethereum_transaction_result").ToString
                                Dim transaction_hash_initial As String = json("value")("transaction_hash_initial").ToString
                                Dim transaction_hash_cancel As String = json("value")("transaction_hash_cancel").ToString
                                Dim tem_transaction_success_initial As Boolean = CBool(json("value")("tem_transaction_success_initial"))
                                Dim tem_transaction_success_cancel As Boolean = CBool(json("value")("tem_transaction_success_cancel"))

                                If ethereum_transaction_result = "initial_success" Or ethereum_transaction_result = "transfer_ethereum_from_exchange_case_proceeding" Then

                                    ' 1, 2차 검증 모두 실시해야 한다.

                                    pure_query = GRT.GQS_submit_cancel.exe(cancel_block_number, eoa, na, coin_name_from, amount, exchange_fee_rate, gasPrice, gasLimit)

                                    verified_eoa = GRT.Security.Gverify.verify(pure_query, signiture, eoa) ' 1차검증

                                    verified_na = GRT.Security.Gverify.verify(pure_query, signiture_na, na) ' 2차검증

                                    If verified_eoa And verified_na Then

                                        database_name = "bc, bc, bc"
                                        table_name = "account, account, exchange"
                                        query_type = "UPDATE, UPDATE, DELETE"
                                        contract_type = command_key

                                        JRS = Await self_input_and_relay.exe(command_key, block_number, received_block_hash, previous_hash, eoa, database_name, table_name, query_type, contract_type, pure_query, signiture, JSS)

                                        If Not GRT.GR.node_level = 0 Then

                                            Call GRT.agent_record.confirm(block_number, Regex.Match(signiture, "^0x.{64}").ToString)

                                        End If

                                    Else
                                        JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", "데이터 변조", "quot"}}, False)
                                    End If

                                Else ' cancel_success

                                    pure_query_for_ethereum_transfer_cancel = GRT.GQS_ethereum_transfer_cancel_treatment.exe(na, gasPrice_for_cancel, gasLimit, idate_string)

                                    verified = GRT.Security.Gverify.verify(pure_query_for_ethereum_transfer_cancel, signiture_for_ethereum_transfer_cancel, na)

                                    If verified Then

                                        database_name = "bc"
                                        table_name = "account"
                                        query_type = "UPDATE"
                                        contract_type = command_key

                                        JRS = Await self_input_and_relay.exe(command_key, block_number, received_block_hash, previous_hash, eoa, database_name, table_name, query_type, contract_type, pure_query_for_ethereum_transfer_cancel, signiture_for_ethereum_transfer_cancel, JSS) ' 부모서버로부터 릴레이 받은 경우임.  

                                        json_JRS = CType(JsonConvert.DeserializeObject(JRS), Linq.JObject)

                                        If json_JRS("success").ToString = "success" Then

                                            Call GRT.agent_record.confirm(block_number, signiture_key)

                                        End If

                                    Else
                                        JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", "데이터 변조", "quot"}}, False)
                                    End If

                                End If

                            End If

                        Else

                            balance = GRT.get_guarantee_balance_from_local_server.exe("foo", signiture_for_get_balance, na)

                            If balance >= amount Then

                                pure_query = GRT.GQS_submit_cancel.exe(cancel_block_number, eoa, na, coin_name_from, amount, exchange_fee_rate, gasPrice, gasLimit)

                                verified = GRT.Security.Gverify.verify(pure_query, signiture, eoa)

                                If verified Then

                                    If initial_transfer = "Y" Then

                                        json("value")("initial_transfer") = "N"

                                        json("value")("signiture_for_get_balance") = GRT.Security.Gsign.sign("foo", GRT.GR.server_private_key)

                                        JSS = CType(JsonConvert.SerializeObject(json), String)

                                        JRS = send_main.exe(signiture_key, JSS) ' 메인으로 보낸다. 하등의 데이터베이스 처리 없이 보낸다. 그러나 에이전트 레코드 기록은 남긴다.

                                        json_JRS = CType(JsonConvert.DeserializeObject(JRS), Linq.JObject)

                                        If json_JRS("success").ToString = "success" Then ' 에이전트 레코드 기록

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
                                        database_name = "bc, bc, bc"
                                        table_name = "account, account, exchange"
                                        query_type = "UPDATE, UPDATE, DELETE"
                                        contract_type = command_key

                                        JRS = Await self_input_and_relay.exe(command_key, block_number, received_block_hash, previous_hash, eoa, database_name, table_name, query_type, contract_type, pure_query, signiture, JSS) ' 부모서버로부터 릴레이 받은 경우임.  

                                        Call GRT.agent_record.confirm(block_number, Regex.Match(signiture, "^0x.{64}").ToString)

                                    End If

                                Else
                                    JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", "데이터 변조", "quot"}}, False)
                                End If

                            Else
                                JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", "거래소(" & na & ") 의 " & coin_name_from & " 잔고가 부족합니다. 현재 잔고는 " & balance & " 입니다.", "quot"}}, False)
                            End If

                        End If

                    Else
                        JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", "데이터 변조", "quot"}}, False)
                    End If

                Else
                    JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", "non_existing_block_number", "quot"}}, False)
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
