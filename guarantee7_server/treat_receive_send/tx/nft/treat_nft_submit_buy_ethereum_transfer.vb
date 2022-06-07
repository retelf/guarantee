Imports System.Numerics
Imports Nethereum.Hex.HexTypes
Imports Nethereum.Web3
Imports Newtonsoft.Json.Linq

Public Class treat_nft_submit_buy_ethereum_transfer

    Public Shared Async Function exe(json As Newtonsoft.Json.Linq.JObject) As Task(Of String)

        Dim JRS As String
        Dim howto As String
        Dim signiture_data_for_ethereum_transfer(1) As String

        Dim transfer_ethereum_from_exchange_case As Boolean
        Dim nonce As HexBigInteger

        Dim receipt As New Nethereum.RPC.Eth.DTOs.TransactionReceipt()
        receipt.TransactionHash = "0x0"

        Dim command_key = json("key").ToString
        Dim sell_order_block_number = CLng(json("value")("sell_order_block_number"))
        Dim ethereum_transfer_eoa_from = json("value")("eoa_buyer").ToString
        Dim buyer_na = json("value")("na_buyer").ToString
        Dim exchange_name_buyer = GRT.GR.account.agency.exchange_name
        Dim seller_na = json("value")("na_seller").ToString
        Dim exchange_name_seller = CStr(json("value")("exchange_name_seller"))
        Dim price = CDec(json("value")("price"))
        Dim gasPrice = CDec(json("value")("gasPrice_Gwei"))
        Dim gasPrice_for_cancel = CDec(json("value")("gasPrice_for_cancel_Gwei"))
        Dim gasLimit = CDec(json("value")("gasLimit"))
        Dim signiture = json("value")("signiture").ToString
        Dim signiture_key = Regex.Match(signiture, "^0x.{64}").ToString

        signiture_data_for_ethereum_transfer(0) = json("value")("signiture_data_for_ethereum_transfer").ToString
        signiture_data_for_ethereum_transfer(1) = json("value")("signiture_data_for_ethereum_transfer_cancel").ToString
        transfer_ethereum_from_exchange_case = CBool(json("value")("transfer_ethereum_from_exchange_case"))
        nonce = New HexBigInteger(CType(json("value")("nonce_biginteger").ToString, BigInteger))

        Dim transfer_data As GRT.transfer_ethereum.st_data = Await send_to_socket_and_set_transfer_data.exe(json)

        If transfer_data.tem_transaction_success_initial Then ' 이곳으로 오는 에러는 오로지 tx_pending_so_long 뿐이다.

            If transfer_data.initial_success Then

                JRS = transfer_ethereum_result_treat.initial_success(signiture_key, command_key, json, transfer_data)

            Else

                If transfer_data.need_cancel_transfer Then

                    ' tx_pending_so_long

                    If transfer_data.tem_transaction_success_cancel Then

                        If transfer_data.cancel_success Then

                            JRS = Await transfer_ethereum_result_treat.cancel_success(sell_order_block_number, signiture_key, command_key, json, transfer_data)

                            howto = get_howto.nft_buy_transfer_fail_clear(price, gasPrice, gasLimit)

                            JRS = GRT.make_json_string.exe(
                                                            {{"key", command_key, "quot"},
                                                            {"success", "fail", "quot"}},
                                                            {{"transaction_hash_initial", transfer_data.transaction_hash_initial, "quot"},
                                                            {"transaction_hash_cancel", transfer_data.transaction_hash_cancel, "quot"},
                                                            {"reason", transfer_data.cancel_error_message, "quot"},
                                                            {"howto", howto, "quot"}}, False)

                        Else ' tx_pending_so_long_in_cancel

                            Call transfer_ethereum_result_treat.set_etc_info("cancel_unclear", json, transfer_data)

                            ' 이더리움이 나갔을 수도 있다. 언락은 보류해야 한다.

                            howto = get_howto.nft_buy_transfer_success_unclear(
                                    price,
                                    buyer_na,
                                    exchange_name_buyer,
                                    seller_na,
                                    exchange_name_seller,
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

                        Call transfer_ethereum_result_treat.set_etc_info("cancel_error", json, transfer_data)

                        ' tx_pending_so_long 상태에서 또 다시 발생한 것이므로 이더리움 인출 여부는 이곳에서 확인할 수 없게 된다.

                        howto = get_howto.nft_buy_transfer_success_unclear(
                                price,
                                buyer_na,
                                exchange_name_buyer,
                                seller_na,
                                exchange_name_seller,
                                transfer_data.transaction_hash_initial,
                                Nothing)

                        JRS = GRT.make_json_string.exe(
                                                            {{"key", command_key, "quot"},
                                                            {"success", "fail", "quot"}},
                                                            {{"transaction_hash_initial", transfer_data.transaction_hash_initial, "quot"},
                                                            {"reason", transfer_data.cancel_error_message, "quot"},
                                                            {"howto", howto, "quot"}}, False)

                    End If

                Else

                    ' 이유여하를 막론하고 NFT는 반납하여야 하므로 캔슬을 하지 않은 케이스이다.
                    ' 따라서 이더리움 송금이 실제로 되었는 지에 관하여 거래소에 문의하고 후속처리를 해야 하는 상황이다.

                    howto = get_howto.nft_recall_no_need_cancel(
                            price,
                            buyer_na,
                            exchange_name_buyer,
                            seller_na,
                            exchange_name_seller,
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

            Call transfer_ethereum_result_treat.set_etc_info("initial_error", json, transfer_data)

            Await Task.Run(Sub() request_lock.unlock(sell_order_block_number, signiture_key, "bc_nft", "sell_order")) ' 스스로 락을 걸었고 이더리움이 나가지 않았으므로 어떤 이유로든 언락을 해 주어야 한다.

            JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}},
                                            {{"transaction_hash_initial", "", "quot"},
                                            {"reason", transfer_data.initial_error_message, "quot"}}, False)

        End If

        Return JRS

    End Function

End Class
