Imports System.Numerics
Imports Nethereum.Hex.HexTypes
Imports Nethereum.Web3
Imports Newtonsoft.Json.Linq

Public Class treat_nft_submit_recall_ethereum_transfer

    ' 아래는 완성된 상태가 아니므로 모두 다시 훑어 보아야 한다.

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
        Dim ethereum_transfer_eoa_from = json("value")("eoa_recaller").ToString
        Dim recaller_na = json("value")("na_recaller").ToString
        Dim exchange_name_recaller = GRT.GR.account.agency.exchange_name
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
        nonce = New HexBigInteger(CType(json("value")("nonce").ToString, BigInteger))

        Dim transfer_data As GRT.transfer_ethereum.st_data = Await send_to_socket_and_set_transfer_data.exe(json)

        If transfer_data.initial_success Then ' tem_transaction_success_initial 는 살펴볼 필요 없다.

            JRS = transfer_ethereum_result_treat.initial_success(signiture_key, command_key, json, transfer_data)

        Else ' 오로지 no_need_cancel_transfer 이 경우 뿐이다. 또한 아래에서 send_main 이 이루어지므로 언락 역시 필요없다.

            Call transfer_ethereum_result_treat.no_need_cancel_transfer("no_need_cancel_transfer", signiture_key, command_key, json, transfer_data)

            ' 이유여하를 막론하고 NFT는 반납하여야 하므로 캔슬을 하지 않은 케이스이다.
            ' 따라서 이더리움 송금이 실제로 되었는 지에 관하여 거래소에 문의하고 후속처리를 해야 하는 상황이다.

            howto = get_howto.nft_recall_no_need_cancel(
                    price,
                    recaller_na,
                    exchange_name_recaller,
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

        Return JRS

    End Function

End Class
