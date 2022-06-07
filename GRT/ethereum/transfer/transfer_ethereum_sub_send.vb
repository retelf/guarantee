Imports System.Numerics
Imports Nethereum.Hex.HexTypes
Imports Nethereum.RPC.Eth.DTOs
Imports Nethereum.Web3
Imports Nethereum.Web3.Accounts
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Public Class transfer_ethereum_sub_send

    Public Shared Async Function exe(mode As String, signiture_key As String, signiture_data_for_ethereum_transfer As String, transfer_data As transfer_ethereum.st_data) As Task(Of transfer_ethereum.st_data)

        Dim web3 = New Web3("http://localhost:8545")

        Call agent_record.state_update(mode & "_transaction_requested", "", signiture_key)

        Dim receipt As New TransactionReceipt

        Try

            If mode = "initial" Then

                transfer_data.transaction_hash_initial = Await web3.Eth.Transactions.SendRawTransaction.SendRequestAsync(signiture_data_for_ethereum_transfer)

                transfer_data.tem_transaction_success_initial = True

            Else

                transfer_data.transaction_hash_cancel = Await web3.Eth.Transactions.SendRawTransaction.SendRequestAsync(signiture_data_for_ethereum_transfer)

                transfer_data.tem_transaction_success_cancel = True

            End If

            Call GRT.nonce_management.exe_update(transfer_data.public_key, "last_input", transfer_data.nonce, Nothing)

        Catch ex As Exception

            ' transaction_hash 자체를 확보하지 못한 경우이다. 이 경우도 실제 이더리움이 건너가는 경우도 있긴 하다. 하지만 그것은 일단 무시한다. 추후 다시 검토하고 수정한다.
            ' 게스가 꺼진 경우, 송신문제, 가스비, nonce too low 등 - 상당히 명확한 거래실패의 경우이다. 설령 성공을 했더라도 거래를 중지시킨다.
            ' 거래소가 이더리움을 보유하고 있었던 경우라 하더라도 개런티 거래는 진행시키지 않는다. 추후 이의제기가 들어오면 그 때 record 를 기초로 처리한다.

            'If Regex.Match(ex.Message, "insufficient\s*funds", RegexOptions.IgnoreCase).Success Then

            'ElseIf Regex.Match(ex.Message, "rpc", RegexOptions.IgnoreCase).Success And
            '    Regex.Match(ex.Message, "error", RegexOptions.IgnoreCase).Success Then

            'ElseIf Regex.Match(ex.Message, "nonce", RegexOptions.IgnoreCase).Success Then

            'ElseIf Regex.Match(ex.Message, "Rpc\s*timeout", RegexOptions.IgnoreCase).Success Then

            'End If

            If mode = "initial" Then

                transfer_data.tem_transaction_success_initial = False
                transfer_data.initial_error_message = ex.Message

            Else ' 펜딩 상태에서 cancel 에러가 발생한 경우이다.

                transfer_data.tem_transaction_success_cancel = False
                transfer_data.cancel_error_message = ex.Message

            End If

            Call agent_record.state_update(mode & "_transaction_error", ex.Message, signiture_key)

        End Try

        Return transfer_data

    End Function

End Class
