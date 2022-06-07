Imports System.Numerics
Imports Nethereum.Hex.HexTypes
Imports Nethereum.Web3

Public Class send_to_socket_and_set_transfer_data

    Public Shared Async Function exe(json As Linq.JObject) As Task(Of GRT.transfer_ethereum.st_data)

        Dim JSS, JRS As String
        Dim eoa_from, signiture, signiture_key As String
        Dim signiture_data_for_ethereum_transfer(1) As String
        Dim json_receipt As Linq.JObject

        Dim transfer_ethereum_from_exchange_case As Boolean
        Dim nonce As HexBigInteger

        signiture = json("value")("signiture").ToString
        signiture_key = Regex.Match(signiture, "^0x.{64}").ToString
        signiture_data_for_ethereum_transfer(0) = json("value")("signiture_data_for_ethereum_transfer").ToString
        signiture_data_for_ethereum_transfer(1) = json("value")("signiture_data_for_ethereum_transfer_cancel").ToString
        transfer_ethereum_from_exchange_case = CBool(json("value")("transfer_ethereum_from_exchange_case"))
        nonce = New HexBigInteger(CType(json("value")("nonce_biginteger").ToString, BigInteger))

        eoa_from = Web3.OfflineTransactionSigner.GetSenderAddress(signiture_data_for_ethereum_transfer(0))

        JSS = GRT.make_json_string.exe(
                                    {{"key", "submit_NTD_transfer", "quot"}},
                                    {
                                    {"block_hash", "initial", "quot"},
                                    {"eoa_from", eoa_from, "quot"},
                                    {"signiture_key", signiture_key, "quot"},
                                    {"signiture_data_for_ethereum_transfer", signiture_data_for_ethereum_transfer(0), "quot"},
                                    {"signiture_data_for_ethereum_transfer_cancel", signiture_data_for_ethereum_transfer(1), "quot"},
                                    {"transfer_ethereum_from_exchange_case", CStr(CInt(transfer_ethereum_from_exchange_case)), "non_quot"},
                                    {"nonce_biginteger", CStr(nonce.Value.ToString), "non_quot"}}, False)

        JRS = Await GRT.socket_client.exe(GRT.GR.address_server_local_ethereum, GRT.GR.port_number_server_local_ethereum, GRT.GR.temporary_self_client_socket_sender_number, JSS)

        json_receipt = CType(JsonConvert.DeserializeObject(JRS), Linq.JObject)

        Dim transfer_data As New GRT.transfer_ethereum.st_data

        transfer_data.public_key = json_receipt("value")("public_key").ToString
        transfer_data.command_key = json_receipt("value")("command_key").ToString
        transfer_data.transfer_ethereum_from_exchange_case = CBool(json_receipt("value")("transfer_ethereum_from_exchange_case"))
        transfer_data.transaction_hash_initial = json_receipt("value")("transaction_hash_initial").ToString
        transfer_data.transaction_hash_cancel = json_receipt("value")("transaction_hash_cancel").ToString
        transfer_data.json_receipt_initial = CType(JsonConvert.DeserializeObject(json_receipt("value")("receipt_initial").ToString), Linq.JObject)
        transfer_data.json_receipt_cancel = CType(JsonConvert.DeserializeObject(json_receipt("value")("receipt_cancel").ToString), Linq.JObject)
        transfer_data.tem_transaction_success_initial = CBool(json_receipt("value")("tem_transaction_success_initial"))
        transfer_data.tem_transaction_success_cancel = CBool(json_receipt("value")("tem_transaction_success_cancel"))
        transfer_data.initial_success = CBool(json_receipt("value")("initial_success"))
        transfer_data.need_cancel_transfer = CBool(json_receipt("value")("need_cancel_transfer"))
        transfer_data.cancel_success = CBool(json_receipt("value")("cancel_success"))
        transfer_data.nonce_too_low_in_initial_phase = CBool(json_receipt("value")("nonce_too_low_in_initial_phase"))
        transfer_data.nonce_too_low_in_cancel_phase = CBool(json_receipt("value")("nonce_too_low_in_cancel_phase"))
        transfer_data.initial_error_message = json_receipt("value")("initial_error_message").ToString
        transfer_data.cancel_error_message = json_receipt("value")("cancel_error_message").ToString

        Return transfer_data

    End Function

End Class
