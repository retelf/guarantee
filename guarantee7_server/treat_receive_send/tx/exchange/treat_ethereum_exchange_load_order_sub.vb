Imports System.Numerics
Imports Nethereum.Hex.HexTypes
Imports Nethereum.Web3
Imports Nethereum.Web3.Accounts
Imports Newtonsoft.Json.Linq

Public Class treat_ethereum_exchange_load_order_sub

    Public Shared Async Function exe(json As Newtonsoft.Json.Linq.JObject) As Task(Of String)

        Dim JRS As String
        Dim howto As String
        Dim eoa, case_retored_eoa, na, exchange_name, signiture, signiture_key, command_key As String
        Dim signiture_data_for_ethereum_transfer(1) As String
        Dim amount, gasPrice, gasLimit As Decimal

        Dim transfer_ethereum_from_exchange_case As Boolean
        Dim nonce As HexBigInteger

        Dim receipt As New Nethereum.RPC.Eth.DTOs.TransactionReceipt()
        receipt.TransactionHash = "0x0"

        command_key = json("key").ToString
        eoa = json("value")("eoa").ToString

        na = json("value")("na").ToString
        exchange_name = GRT.get_exchange_name.exe(na)
        amount = CDec(json("value")("amount"))
        gasPrice = CDec(json("value")("gasPrice"))
        gasLimit = CDec(json("value")("gasLimit"))
        signiture = json("value")("signiture").ToString
        signiture_key = Regex.Match(signiture, "^0x.{64}").ToString

        case_retored_eoa = json("value")("case_retored_eoa_from").ToString
        signiture_data_for_ethereum_transfer(0) = json("value")("signiture_data_for_ethereum_transfer").ToString
        signiture_data_for_ethereum_transfer(1) = json("value")("signiture_data_for_ethereum_transfer_cancel").ToString
        transfer_ethereum_from_exchange_case = CBool(json("value")("transfer_ethereum_from_exchange_case"))
        nonce = New HexBigInteger(CType(json("value")("nonce_biginteger").ToString, BigInteger))

        If case_retored_eoa = Web3.OfflineTransactionSigner.GetSenderAddress(signiture_data_for_ethereum_transfer(0)) Then

            Dim transfer_data As GRT.transfer_ethereum.st_data = Await send_to_socket_and_set_transfer_data.exe(json)

            If transfer_data.tem_transaction_success_initial Then ' 이곳으로 오는 에러는 오로지 tx_pending_so_long 뿐이다.

                If transfer_data.initial_success Then

                    JRS = transfer_ethereum_result_treat.initial_success(signiture_key, command_key, json, transfer_data)

                Else

                    If transfer_data.nonce_too_low_in_initial_phase Then ' cancel_phase 로 가지 않는다.

                        ' 통상 언락을 하는 경우지만 이 곳은 락의 대상이 없기 때문에 언락하지 않는다.

                        howto = get_howto.nonce_too_low("initial", transfer_data)

                        JRS = GRT.make_json_string.exe(
                                                        {{"key", command_key, "quot"},
                                                        {"success", "fail", "quot"}},
                                                        {{"transaction_hash_initial", transfer_data.transaction_hash_initial, "quot"},
                                                        {"reason", transfer_data.initial_error_message, "quot"},
                                                        {"howto", howto, "quot"}}, False)

                    Else

                        If transfer_data.tem_transaction_success_cancel Then

                            If transfer_data.cancel_success Then

                                Dim unused As String = Await transfer_ethereum_result_treat.cancel_success(Nothing, signiture_key, command_key, json, transfer_data)

                                howto = get_howto.load_order_fail_clear(amount, gasPrice, gasLimit)

                                JRS = GRT.make_json_string.exe(
                                                            {{"key", command_key, "quot"},
                                                            {"success", "fail", "quot"}},
                                                            {{"transaction_hash_initial", transfer_data.transaction_hash_initial, "quot"},
                                                            {"transaction_hash_cancel", transfer_data.transaction_hash_cancel, "quot"},
                                                            {"reason", transfer_data.cancel_error_message, "quot"},
                                                            {"howto", howto, "quot"}}, False)

                            Else ' tx_pending_so_long_in_cancel

                                ' 이더리움이 나갔을 수도 있다. 언락은 보류해야 한다.

                                howto = get_howto.load_order_success_unclear(
                                    amount,
                                    na,
                                    exchange_name,
                                    transfer_data.transaction_hash_initial,
                                    transfer_data.transaction_hash_cancel)

                                JRS = GRT.make_json_string.exe(
                                                            {{"key", command_key, "quot"},
                                                            {"success", "fail", "quot"}},
                                                            {{"transaction_hash_initial", transfer_data.transaction_hash_initial, "quot"},
                                                            {"transaction_hash_cancel", transfer_data.transaction_hash_cancel, "quot"},
                                                            {"reason", transfer_data.cancel_error_message, "quot"},
                                                            {"howto", howto, "quot"}}, False)

                            End If

                        Else ' rpc 송신문제 등 확인불가 에러

                            ' tx_pending_so_long 상태에서 또 다시 발생한 것이므로 이더리움 인출 여부는 이곳에서 확인할 수 없게 된다.

                            howto = get_howto.load_order_success_unclear(
                                amount,
                                na,
                                exchange_name,
                                transfer_data.transaction_hash_initial,
                                Nothing)

                            JRS = GRT.make_json_string.exe(
                                                            {{"key", command_key, "quot"},
                                                            {"success", "fail", "quot"}},
                                                            {{"transaction_hash_initial", transfer_data.transaction_hash_initial, "quot"},
                                                            {"reason", transfer_data.cancel_error_message, "quot"},
                                                            {"howto", howto, "quot"}}, False)

                        End If

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
