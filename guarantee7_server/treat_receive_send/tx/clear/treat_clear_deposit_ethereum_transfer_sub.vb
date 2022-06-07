Imports System.Numerics
Imports Nethereum.Hex.HexTypes

Public Class treat_clear_deposit_ethereum_transfer_sub

    Public Shared Async Function exe(json As Newtonsoft.Json.Linq.JObject) As Task(Of String)

        Dim JSS, JRS As String
        Dim howto As String
        Dim na_depositor, depositor, exchange_name_na_depositor, coin_name, signiture, signiture_key, signiture_for_get_balance, signiture_for_get_balance_na_depositor, command_key As String
        Dim signiture_data_for_ethereum_transfer(1) As String
        Dim amount_to_clear, gasPrice, gasLimit As Decimal

        Dim transfer_ethereum_from_exchange_case As Boolean
        Dim nonce As HexBigInteger

        Dim receipt As New Nethereum.RPC.Eth.DTOs.TransactionReceipt()
        receipt.TransactionHash = "0x0"

        command_key = json("key").ToString
        na_depositor = json("value")("na").ToString
        exchange_name_na_depositor = json("value")("exchange_name_na_depositor").ToString
        coin_name = json("value")("coin_name").ToString
        depositor = json("value")("depositor").ToString
        amount_to_clear = CDec(json("value")("amount_to_clear"))
        gasPrice = CDec(json("value")("gasPrice"))
        gasLimit = CDec(json("value")("gasLimit"))
        signiture_for_get_balance = json("value")("signiture_for_get_balance").ToString ' depositor
        signiture_for_get_balance_na_depositor = json("value")("signiture_for_get_balance_na_depositor").ToString
        signiture = json("value")("signiture").ToString ' depositor
        signiture_key = Regex.Match(signiture, "^0x.{64}").ToString
        signiture_data_for_ethereum_transfer(0) = json("value")("signiture_data_for_ethereum_transfer").ToString
        signiture_data_for_ethereum_transfer(1) = json("value")("signiture_data_for_ethereum_transfer_cancel").ToString
        transfer_ethereum_from_exchange_case = CBool(json("value")("transfer_ethereum_from_exchange_case"))
        nonce = New HexBigInteger(CType(json("value")("nonce_biginteger").ToString, BigInteger))

        Dim efficient_data = GRT.check_offline_transaction_data.exe(depositor, amount_to_clear - (gasPrice * gasLimit / 1000000000), gasPrice, gasLimit, na_depositor, nonce, signiture_data_for_ethereum_transfer)

        If efficient_data Then

            Dim transfer_data As GRT.transfer_ethereum.st_data = Await send_to_socket_and_set_transfer_data.exe(json)

            If transfer_data.tem_transaction_success_initial Then ' 이곳으로 오는 에러는 tx_pending_so_long 과 nonce_too_low 뿐이다.

                If transfer_data.initial_success Then

                    JRS = transfer_ethereum_result_treat.initial_success(signiture_key, command_key, json, transfer_data)

                Else ' tx_pending_so_long, nonce_too_low_in_initial_phase

                    If transfer_data.nonce_too_low_in_initial_phase Then ' cancel_phase 로 가지 않는다.

                        howto = get_howto.nonce_too_low("initial", transfer_data)

                        JRS = GRT.make_json_string.exe(
                                                        {{"key", command_key, "quot"},
                                                        {"success", "fail", "quot"}},
                                                        {{"transaction_hash_initial", transfer_data.transaction_hash_initial, "quot"},
                                                        {"reason", transfer_data.initial_error_message, "quot"},
                                                        {"howto", howto, "quot"}}, False)

                    Else ' 오로지 no_need_cancel_transfer 이 경우 뿐이다. 

                        Call transfer_ethereum_result_treat.no_need_cancel_transfer("no_need_cancel_transfer", signiture_key, command_key, json, transfer_data)

                        ' 이유여하를 막론하고 NFT는 반납하여야 하므로 캔슬을 하지 않은 케이스이다.
                        ' 따라서 이더리움 송금이 실제로 되었는 지에 관하여 거래소에 문의하고 후속처리를 해야 하는 상황이다.

                        howto = get_howto.clear_deposit_no_need_cancel(
                                                                    amount_to_clear - (gasPrice * gasLimit / 1000000000),
                                                                    na_depositor,
                                                                    exchange_name_na_depositor,
                                                                    transfer_data.transaction_hash_initial)

                        JRS = GRT.make_json_string.exe(
                                        {{"key", command_key, "quot"},
                                        {"success", "fail", "quot"}},
                                        {{"transaction_hash_initial", transfer_data.transaction_hash_initial, "quot"},
                                        {"reason", transfer_data.cancel_error_message, "quot"},
                                        {"howto", howto, "quot"}}, False)

                    End If

                End If

            Else ' rpc 송신문제 등 확인불가 에러

                JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}},
                                            {{"transaction_hash_initial", "", "quot"},
                                            {"reason", transfer_data.initial_error_message, "quot"}}, False)

            End If

        Else

            JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", "데이터 변조", "quot"}}, False)

        End If

        Return JRS

    End Function

End Class
