Imports Nethereum.RPC.Eth.DTOs
Imports Nethereum.Hex.HexTypes
Imports System.Numerics
Imports Nethereum.RPC.NonceServices

Public Class check_filterChanges

    Public Shared Async Function exe(mode As String, web3 As Web3, transaction_hash As String, transfer_data As transfer_ethereum.st_data) As Task(Of transfer_ethereum.st_data)

        ' 이것은 특수한 경우만을 위한 것이다. 결과적으로는. 이해하기 힘든 pendingFilter 현상이다.
        ' 이는 게스 안을 들여다 볼 수 없으므로 어쩔 수 없다.
        ' pendingFilter 가 발생하기 전의 tx 까지 잡히는 현상이 있는데 그 경우에만 유용하다.
        ' 그래도 없는 것보다는 낫다.
        ' pendingFilter 를 미리 1시간 전 쯤에 만들어 놓고 그것을 사용하면 된다.

        Dim transaction As Transaction

        If mode = "initial" Then

            transfer_data.receipt_initial = Await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transaction_hash) ' It will return null for pending transactions and an object if the transaction is successful.

            If transfer_data.receipt_initial Is Nothing Then

                If Not transfer_data.filter_hash_saved Then

                    transfer_data.filterChanges = Await web3.Eth.Filters.GetFilterChangesForBlockOrTransaction.SendRequestAsync(transfer_data.pendingFilter)

                    For i = 0 To transfer_data.filterChanges.Length - 1

                        If Not transfer_data.filterChanges(i) = transfer_data.transaction_hash_initial Then

                            transaction = Await web3.Eth.Transactions.GetTransactionByHash.SendRequestAsync(transfer_data.filterChanges(i))

                            If Not transaction Is Nothing Then

                                If transaction.From.ToLower = transfer_data.public_key.ToLower Then

                                    transfer_data.my_length += 1

                                    ReDim Preserve transfer_data.my_other_hash(transfer_data.my_length - 1)

                                    transfer_data.my_other_hash(transfer_data.my_length - 1) = transfer_data.filterChanges(i)

                                End If

                            End If

                        End If

                    Next

                    ReDim transfer_data.my_transaction(transfer_data.my_length - 1)
                    ReDim transfer_data.my_receipt(transfer_data.my_length - 1)

                    transfer_data.filter_hash_saved = True

                End If

                transfer_data = Await check_nonce_too_low(mode, web3, transfer_data)

            Else
                ' 할일이 끝났다.
            End If

        Else

            ' 논스 중복이 없는 상태에서 이리로 들어왔다. 즉 상당한 시간이 경과한 후에 이리로 들어선 것이다. 그래도 혹시나 receipt_initial 은 끝까지 확인한다.

            transfer_data.receipt_initial = Await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transfer_data.transaction_hash_initial)
            transfer_data.receipt_cancel = Await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transfer_data.transaction_hash_cancel) ' It will return null for pending transactions and an object if the transaction is successful.

            If Not transfer_data.receipt_initial Is Nothing Or Not transfer_data.receipt_cancel Is Nothing Then

                ' 할일이 끝났다.

            Else

                transfer_data = Await check_nonce_too_low(mode, web3, transfer_data)

            End If

        End If

        Return transfer_data

    End Function

    Shared Async Function check_nonce_too_low(mode As String, web3 As Web3, transfer_data As transfer_ethereum.st_data) As Task(Of transfer_ethereum.st_data)

        For i = 0 To transfer_data.my_length - 1

            transfer_data.my_transaction(i) = Await web3.Eth.Transactions.GetTransactionByHash.SendRequestAsync(transfer_data.my_other_hash(i))

            transfer_data.my_receipt(i) = Await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transfer_data.my_other_hash(i))

            If Not transfer_data.my_receipt(i) Is Nothing Then

                If transfer_data.my_transaction(i).Nonce.Value >= transfer_data.nonce.Value Then

                    If mode = "initial" Then

                        transfer_data.nonce_too_low_in_initial_phase = True

                    Else

                        transfer_data.nonce_too_low_in_cancel_phase = True

                    End If

                    Exit For

                End If

            End If

        Next

        '' 스스로의 논스도 다이렉트로 확인한다. 그런데 이것은 리시트가 나오기 전에도 작동하므로 제대로 된 tx 마저도 취소처리시켜 버린다.

        'If Not nonce_too_low_my_transaction Then

        '    NonceService = New InMemoryNonceService(transfer_data.public_key, GRT.GR.ethereum.web3.Client)

        '    futureNonce = Await NonceService.GetNextNonceAsync()

        '    If futureNonce.Value > transfer_data.nonce.Value Then

        '        If mode = "initial" Then

        '            transfer_data.nonce_too_low_in_initial_phase = True

        '        Else

        '            transfer_data.nonce_too_low_in_cancel_phase = True

        '        End If

        '    End If

        'End If

        Return transfer_data

    End Function

End Class
