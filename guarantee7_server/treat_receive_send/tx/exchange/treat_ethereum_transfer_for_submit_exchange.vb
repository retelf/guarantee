Imports System.Numerics
Imports Nethereum.Hex.HexTypes
Imports Nethereum.Web3
Imports Nethereum.Web3.Accounts
Imports Newtonsoft.Json.Linq

Public Class treat_ethereum_transfer_for_submit_exchange

    Public Shared Async Function exe(json As Newtonsoft.Json.Linq.JObject) As Task(Of String)

        Dim JRS As String
        Dim howto As String
        Dim eoa_seller, eoa_buyer, na, eoa_ethereum_from, exchange_name, signiture, signiture_key, signiture_data_for_ethereum_transfer(1), command_key As String
        Dim ethereum_from_is_na As Boolean
        Dim clickers_coin_name_to_buy, clickers_coin_name_to_sell As String
        Dim seller_amount, gasPrice, gasLimit As Decimal
        Dim exchange_block_number As Long

        Dim transfer_ethereum_from_exchange_case As Boolean
        Dim nonce As HexBigInteger

        Dim receipt As New Nethereum.RPC.Eth.DTOs.TransactionReceipt()

        receipt.TransactionHash = "0x0"

        command_key = json("key").ToString

        exchange_block_number = CLng(json("value")("exchange_block_number"))
        eoa_seller = json("value")("eoa_seller").ToString
        eoa_buyer = json("value")("eoa_buyer").ToString
        na = json("value")("na").ToString
        clickers_coin_name_to_buy = json("value")("clickers_coin_name_to_buy").ToString
        clickers_coin_name_to_sell = json("value")("clickers_coin_name_to_sell").ToString

        If clickers_coin_name_to_buy = "ethereum" Then
            ethereum_from_is_na = True
            eoa_ethereum_from = na
        Else
            ethereum_from_is_na = False
            eoa_ethereum_from = eoa_buyer
        End If

        exchange_name = GRT.get_exchange_name.exe(na)
        seller_amount = CDec(json("value")("amount"))
        signiture = json("value")("signiture").ToString
        signiture_key = Regex.Match(signiture, "^0x.{64}").ToString

        signiture_data_for_ethereum_transfer(0) = json("value")("signiture_data_for_ethereum_transfer").ToString
        signiture_data_for_ethereum_transfer(1) = json("value")("signiture_data_for_ethereum_transfer_cancel").ToString
        transfer_ethereum_from_exchange_case = CBool(json("value")("transfer_ethereum_from_exchange_case"))
        nonce = New HexBigInteger(CType(json("value")("nonce_biginteger").ToString, BigInteger))

        Dim transfer_data As GRT.transfer_ethereum.st_data = Await send_to_socket_and_set_transfer_data.exe(json)

        If transfer_data.tem_transaction_success_initial Then ' 이곳으로 오는 에러는 tx_pending_so_long 과 nonce_too_low 뿐이다.

            If transfer_data.initial_success Then

                JRS = transfer_ethereum_result_treat.initial_success(signiture_key, command_key, json, transfer_data)

            Else ' tx_pending_so_long, nonce_too_low_in_initial_phase

                If transfer_data.nonce_too_low_in_initial_phase Then ' cancel_phase 로 가지 않는다.

                    Await Task.Run(Sub() request_lock.unlock(exchange_block_number, signiture_key, "bc", "exchange")) ' 스스로 락을 걸었고 이더리움이 나가지 않았으므로 어떤 이유로든 언락을 해 주어야 한다.

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

                            howto = get_howto.exchange_fail_clear(eoa_seller, eoa_buyer, na, clickers_coin_name_to_buy, seller_amount, gasPrice, gasLimit)

                            JRS = GRT.make_json_string.exe(
                                                            {{"key", command_key, "quot"},
                                                            {"success", "fail", "quot"}},
                                                            {{"transaction_hash_initial", transfer_data.transaction_hash_initial, "quot"},
                                                            {"transaction_hash_cancel", transfer_data.transaction_hash_cancel, "quot"},
                                                            {"reason", transfer_data.cancel_error_message, "quot"},
                                                            {"howto", howto, "quot"}}, False)

                        Else

                            If transfer_ethereum_from_exchange_case Then ' 쿼리 일단 실행. submit_exchange, submit_cancel, submit_recall 의 경우만 적용된다.

                                JRS = transfer_ethereum_result_treat.transfer_ethereum_from_exchange_case_proceeding("transfer_ethereum_from_exchange_case_proceeding", signiture_key, command_key, json, transfer_data)

                                howto = get_howto.transfer_ethereum_from_exchange_case_proceeding_exchange(
                                    seller_amount,
                                    eoa_seller,
                                    eoa_buyer,
                                    na,
                                    clickers_coin_name_to_buy,
                                    exchange_name,
                                    transfer_data.transaction_hash_initial,
                                    transfer_data.transaction_hash_cancel)

                                JRS = GRT.make_json_string.exe(
                                                            {{"key", command_key, "quot"},
                                                            {"success", "success", "quot"}},
                                                            {{"transaction_hash_initial", transfer_data.transaction_hash_initial, "quot"},
                                                            {"transaction_hash_cancel", transfer_data.transaction_hash_cancel, "quot"},
                                                            {"reason", "transfer_ethereum_from_exchange_case_proceeding", "quot"},
                                                            {"howto", howto, "quot"}}, False)

                            Else ' 퀴리 실행 중지

                                ' 이더리움이 나갔을 수도 있다. 언락은 보류해야 한다.

                                howto = get_howto.exchange_success_unclear(
                                        seller_amount,
                                        eoa_seller,
                                        eoa_buyer,
                                        na,
                                        clickers_coin_name_to_buy,
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

                        End If

                    Else ' rpc 송신문제 등 확인불가 에러

                        ' tx_pending_so_long 상태에서 또 다시 발생한 것이므로 이더리움 인출 여부는 이곳에서 확인할 수 없게 된다.

                        howto = get_howto.exchange_success_unclear(
                                seller_amount,
                                eoa_seller,
                                eoa_buyer,
                                na,
                                clickers_coin_name_to_buy,
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

        Return JRS

    End Function

End Class
