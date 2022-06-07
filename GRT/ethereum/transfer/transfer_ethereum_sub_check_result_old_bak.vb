Imports System.Numerics
Imports Nethereum.Hex.HexTypes
Imports Nethereum.RPC.Eth.DTOs
Imports Nethereum.Web3
Imports Nethereum.Web3.Accounts
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Public Class transfer_ethereum_sub_check_result_old_bak

    Shared Async Function exe(mode As String, signiture_key As String, transaction_hash As String, transfer_data As transfer_ethereum.st_data) As Task(Of transfer_ethereum.st_data)

        Dim web3 = New Web3("http://localhost:8545")

        Dim reason As String

        If mode = "initial" Then
            Call agent_record.transaction_hash_update(transaction_hash, signiture_key)
        Else
            Call agent_record.transaction_hash_cancel_update(transaction_hash, signiture_key)
        End If

        Call agent_record.state_update(mode & "_transaction_hash_generated", "", signiture_key)

        Dim rolling_times As Integer = 0

        While True

            If mode = "initial" Then

                transfer_data.receipt_initial = Await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transaction_hash) ' It will return null for pending transactions and an object if the transaction is successful.

                If transfer_data.receipt_initial Is Nothing Then ' 현재 펜딩상태.

                    rolling_times += 1

                    Threading.Thread.Sleep(5000)

                Else ' 영수증 객체 반환

                    transfer_data.initial_success = True

                    Call agent_record.state_update("receipt_received", "", signiture_key)

                    Call agent_record.save_receipt(CType(JsonConvert.SerializeObject(transfer_data.receipt_initial), String), signiture_key)

                    Exit While

                End If

                If rolling_times > GRT.GR.ethereum_transaction_rolling_times Then

                    ' 거래소가 이더리움을 보유하고 있었던 경우에는 나중이 보장되므로 개런티 거래는 진행시킨다.
                    ' 즉 불명확할 때의 방식은
                    '   거래소가 이더리움을 먼저 보내는 경우는 무조건 상대발 거래 진행시키고
                    '   거래소가 개런티를 보유하고 있었던 경우는 일단은 거래를 보류시킨다.
                    '   나머지는 외부에서 처리한다.

                    ' 다만 여기서 지금의 논스값을 확인해 본다.
                    ' 만약 논스값이 올라갔다면 어쨌든 실행은 된 것이다. 이 경우는 캔슬로 가지 않고 기다리라는 말만 하고 거래를 진행시킨다.
                    ' 만약 논스값이 그대로라면 이는 가스비나 그 외 특단의 문제가 존재하는 것이다. 이 경우 동일 논스 캔슬 트랜젝션을 날려준다.

                    ' 또한 cancel 체크 과정에서 이니셜과 캔슬 해쉬(영수증) 함께 체킹한다.

                    ' 논스값, initial 해시, cancel 해시를 번갈아가면서 상태를 확인한다.


                    reason = "tx_pending_so_long"

                    transfer_data.receipt_initial = New TransactionReceipt

                    transfer_data.receipt_initial.TransactionHash = transfer_data.transaction_hash_initial

                    transfer_data.receipt_initial.HasErrors()
                    transfer_data.receipt_initial.Logs = JArray.Parse(
                        "[{""key"": ""error"", ""transaction_hash"": """ & transfer_data.transaction_hash_initial & """, ""reason"": """ & reason & """}]")

                    Call agent_record.state_update("tx_pending_so_long", reason, signiture_key)

                    Exit While

                End If

            Else

                transfer_data.receipt_cancel = Await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transaction_hash) ' It will return null for pending transactions and an object if the transaction is successful.

                If transfer_data.receipt_cancel Is Nothing Then ' 현재 펜딩상태.

                    rolling_times += 1

                    Threading.Thread.Sleep(5000)

                Else ' 영수증 객체 반환

                    transfer_data.cancel_success = True

                    Call agent_record.state_update("cancel_receipt_received", "", signiture_key)

                    Call agent_record.save_receipt_cancel(CType(JsonConvert.SerializeObject(transfer_data.receipt_cancel), String), signiture_key)

                    Exit While

                End If

                If rolling_times > GRT.GR.ethereum_transaction_rolling_times_for_cancel Then

                    reason = "tx_pending_so_long_in_cancel"

                    transfer_data.receipt_cancel = New TransactionReceipt

                    transfer_data.receipt_cancel.TransactionHash = transfer_data.transaction_hash_cancel

                    transfer_data.receipt_cancel.HasErrors()
                    transfer_data.receipt_cancel.Logs = JArray.Parse(
                        "[{""key"": ""error"", ""transaction_hash"": """ & transfer_data.transaction_hash_cancel & """, ""reason"": """ & reason & """}]")

                    Call agent_record.state_update("tx_pending_so_long_in_cancel", reason, signiture_key)

                    Exit While

                End If

            End If

        End While

        Return transfer_data

    End Function

End Class
