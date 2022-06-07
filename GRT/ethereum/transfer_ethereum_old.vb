Imports System.Numerics
Imports Nethereum.Hex.HexTypes
Imports Nethereum.RPC.Eth.DTOs
Imports Nethereum.Web3
Imports Nethereum.Web3.Accounts
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Public Class transfer_ethereum_old

    Public Shared Async Function TransferEtherAndWaitForReceiptAsync(private_key As String, to_address As String, etherAmount As Decimal, Optional gasPriceGwei As Decimal = Nothing, Optional gas As BigInteger = Nothing) As Task(Of Nethereum.RPC.Eth.DTOs.TransactionReceipt)

        Dim account = New Account(private_key, GRT.GR.account.ethereum_chain_id)

        Dim web3 = New Web3(account)

        Dim receipt = Await web3.Eth.GetEtherTransferService().TransferEtherAndWaitForReceiptAsync(to_address, etherAmount, gasPriceGwei, gas)

        Return receipt

    End Function

    Public Shared Async Function SendRawTransaction_SendRequestAsync(signiture_key As String, data As String) As Task(Of Nethereum.RPC.Eth.DTOs.TransactionReceipt)

        Dim web3 = New Web3("http://localhost:8545")

        Call agent_record.state_update("transaction_requested", "", signiture_key)

        Dim transaction_hash As String
        Dim tem_transaction_success As Boolean
        Dim receipt As New TransactionReceipt
        Dim reason, howto As String

        Dim pendingFilter As HexBigInteger = Await web3.Eth.Filters.NewPendingTransactionFilter.SendRequestAsync()

        Dim filterChanges() As String

        Try

            transaction_hash = Await web3.Eth.Transactions.SendRawTransaction.SendRequestAsync(data(0))

            filterChanges = Await web3.Eth.Filters.GetFilterChangesForBlockOrTransaction.SendRequestAsync(pendingFilter)

            tem_transaction_success = False

            For i = 0 To filterChanges.Length - 1

                If filterChanges(i) = transaction_hash Then

                    tem_transaction_success = True

                    Exit For

                End If

            Next

            If tem_transaction_success = False Then

                Call agent_record.state_update("transaction_error", "transaction_not_mined", signiture_key)

                receipt.HasErrors()
                receipt.Logs = JArray.Parse("[{""key"": ""error"", ""transaction_hash"": """ & transaction_hash & """, ""reason"": ""transaction_not_mined""}]")

            End If

        Catch ex As Exception

            tem_transaction_success = False

            Call agent_record.state_update("transaction_error", ex.Message, signiture_key)

            receipt.HasErrors()
            receipt.Logs = JArray.Parse("[{""key"": ""error"", ""transaction_hash"": """", ""reason"": """ & ex.Message & """}]")

        End Try

        If tem_transaction_success Then

            Call agent_record.transaction_hash_update(transaction_hash, signiture_key)

            Call agent_record.state_update("transaction_hash_generated", "", signiture_key)

            Dim rolling_times As Integer = 0

            While True

                receipt = Await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transaction_hash) ' It will return null for pending transactions and an object if the transaction is successful.

                If receipt Is Nothing Then ' 현재 펜딩상태.

                    rolling_times += 1

                    Threading.Thread.Sleep(5000)

                Else ' 영수증 객체 반환

                    Call agent_record.state_update("receipt_received", "", signiture_key)

                    Call agent_record.save_receipt(CType(JsonConvert.SerializeObject(receipt), String), signiture_key)

                    Exit While

                End If

                If rolling_times >= 10 Then

                    reason = "tx_pending_so_long" ' 2.5분
                    howto = "<BR/>" & "본 상황은 이더리움 플랫폼의 근본적인 불안정성 때문에 간간히 발생하는 거래지연 상태입니다. " &
                        "본 거래는 추후 정상적으로 완료될 수도 있거나 영원히 미완성 상태로 남아있을 수도 있게 됩니다." & "<BR/>" &
                        "현재 이더리움이 인출되었는 지 아닌 지 확인하는 방법은 적당한 시간 - 대략 하루 정도 - 후에 수작업으로 확인하는 수 밖에 없습니다. " &
                        "이더리움 플랫폼은 이런 경우 그 어떤 처리방법을 알려주지 않으며 모든 것을 사용자의 책임으로 떠미루고 있습니다." & "<BR/>" &
                        "결론적으로 클라이언트께서는 해당 거래소에 문의하셔서 코인이 인출되었으나 그 댓가를 받지 못한 경우 외부에서 별도 송금이나 잔여 처리를 하셔야 합니다. " &
                        "그 외의 다른 방법은 없습니다. 이더리움 플랫폼의 근본적인 문제로서 이들과 연동하는 이상 필연적으로 겪을 수 밖에 없는 불편입니다. 이 점 양해를 부탁드립니다."

                    receipt = New TransactionReceipt

                    receipt.TransactionHash = transaction_hash

                    receipt.HasErrors()
                    receipt.Logs = JArray.Parse(
                        "[{""key"": ""error"", ""transaction_hash"": """ & transaction_hash & """, ""reason"": """ & reason & """, ""howto"": """ & howto & """}]")

                    Call agent_record.state_update("tx_pending_so_long", reason, signiture_key)

                    Exit While

                End If

            End While

        End If

        Return receipt

    End Function

End Class
