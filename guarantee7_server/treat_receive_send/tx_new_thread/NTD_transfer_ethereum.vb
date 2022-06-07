Imports System.Numerics
Imports Nethereum.Hex.HexTypes
Imports Nethereum.RPC.Eth.DTOs
Imports Nethereum.Web3
Imports Nethereum.Web3.Accounts
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Public Class NTD_transfer_ethereum

    Public Shared Async Function exe(json As Newtonsoft.Json.Linq.JObject, JSS As String) As Task(Of String)

        Dim tem_JRS, JRS As String
        Dim json_JRS As Newtonsoft.Json.Linq.JObject

        Dim eoa_from As String
        Dim signiture As String
        Dim signiture_key As String
        Dim command_key As String
        Dim signiture_data_for_ethereum_transfer(1) As String
        Dim transfer_ethereum_from_exchange_case As Boolean
        Dim nonce As HexBigInteger

        command_key = json("key").ToString
        eoa_from = json("value")("eoa_from").ToString
        signiture_key = json("value")("signiture_key").ToString
        signiture_data_for_ethereum_transfer(0) = json("value")("signiture_data_for_ethereum_transfer").ToString
        signiture_data_for_ethereum_transfer(1) = json("value")("signiture_data_for_ethereum_transfer_cancel").ToString
        transfer_ethereum_from_exchange_case = CBool(json("value")("transfer_ethereum_from_exchange_case"))
        nonce = New HexBigInteger(CType(json("value")("nonce_biginteger").ToString, BigInteger))

        Dim transfer_data As GRT.transfer_ethereum.st_data =
                Await GRT.transfer_ethereum.SendRawTransaction_SendRequestAsync(
                eoa_from,
                signiture_key,
                signiture_data_for_ethereum_transfer,
                command_key,
                transfer_ethereum_from_exchange_case,
                nonce)

        tem_JRS = GRT.make_json_string.exe(
                        {{"key", command_key, "quot"}},
                        {
                        {"public_key", eoa_from, "quot"},
                        {"command_key", command_key, "quot"},
                        {"transfer_ethereum_from_exchange_case", CStr(CInt(transfer_ethereum_from_exchange_case)), "non_quot"},
                        {"transaction_hash_initial", transfer_data.transaction_hash_initial, "quot"},
                        {"transaction_hash_cancel", transfer_data.transaction_hash_cancel, "quot"},
                        {"tem_transaction_success_initial", CStr(CInt(transfer_data.tem_transaction_success_initial)), "non_quot"},
                        {"tem_transaction_success_cancel", CStr(CInt(transfer_data.tem_transaction_success_cancel)), "non_quot"},
                        {"initial_success", CStr(CInt(transfer_data.initial_success)), "non_quot"},
                        {"need_cancel_transfer", CStr(CInt(transfer_data.need_cancel_transfer)), "non_quot"},
                        {"cancel_success", CStr(CInt(transfer_data.cancel_success)), "non_quot"},
                        {"nonce_too_low_in_initial_phase", CStr(CInt(transfer_data.nonce_too_low_in_initial_phase)), "non_quot"},
                        {"nonce_too_low_in_cancel_phase", CStr(CInt(transfer_data.nonce_too_low_in_cancel_phase)), "non_quot"},
                        {"initial_error_message", transfer_data.initial_error_message, "quot"},
                        {"cancel_error_message", transfer_data.cancel_error_message, "quot"}}, False)

        json_JRS = CType(JsonConvert.DeserializeObject(tem_JRS), Linq.JObject)

        json_JRS("value")("receipt_initial") = CType(JsonConvert.DeserializeObject(JsonConvert.SerializeObject(transfer_data.receipt_initial)), Linq.JObject)
        json_JRS("value")("receipt_cancel") = CType(JsonConvert.DeserializeObject(JsonConvert.SerializeObject(transfer_data.receipt_cancel)), Linq.JObject)

        If transfer_data.initial_success Then
            json_JRS("value")("usedGas") = transfer_data.receipt_initial.GasUsed.ToString
        ElseIf transfer_data.cancel_success Then
            json_JRS("value")("usedGas") = transfer_data.receipt_cancel.GasUsed.ToString
        End If

        JRS = CType(JsonConvert.SerializeObject(json_JRS), String)

        Return JRS

    End Function

End Class