Imports Nethereum.RPC.Eth.DTOs
Imports Newtonsoft.Json.Linq

Public Class transfer_ethereum_makepending_so_long_data

    Public Shared Function exe(mode As String, signiture_key As String, transaction_hash As String, state As String, reason As String, transfer_data As transfer_ethereum.st_data) As transfer_ethereum.st_data


        If mode = "initial" Then

            transfer_data.receipt_initial = New TransactionReceipt

            transfer_data.receipt_initial.TransactionHash = transfer_data.transaction_hash_initial

            transfer_data.receipt_initial.HasErrors()
            transfer_data.receipt_initial.Logs = JArray.Parse(
                            "[{""key"": ""error"", ""transaction_hash"": """ & transfer_data.transaction_hash_initial & """, ""reason"": """ & reason & """}]")

        Else

            transfer_data.receipt_cancel = New TransactionReceipt

            transfer_data.receipt_cancel.TransactionHash = transfer_data.transaction_hash_cancel

            transfer_data.receipt_cancel.HasErrors()
            transfer_data.receipt_cancel.Logs = JArray.Parse(
                            "[{""key"": ""error"", ""transaction_hash"": """ & transfer_data.transaction_hash_cancel & """, ""reason"": """ & reason & """}]")

        End If

        Call agent_record.state_update(state, reason, signiture_key)

        Return transfer_data

    End Function

End Class
