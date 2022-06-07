Imports Nethereum.RPC.Eth.DTOs
Imports Nethereum.Hex.HexTypes
Imports System.Numerics
Imports Nethereum.RPC.NonceServices

Public Class check_filterChanges_test

    Shared count_share As Integer = 0
    Shared record As String = ""

    Public Shared Async Function exe(mode As String, web3 As Web3, transaction_hash As String, transfer_data As transfer_ethereum.st_data) As Task(Of transfer_ethereum.st_data)

        Dim transaction As Transaction

        Dim str_filterChanges As String

        If mode = "initial" Then

            transfer_data.receipt_initial = Await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transaction_hash) ' It will return null for pending transactions and an object if the transaction is successful.

            If transfer_data.receipt_initial Is Nothing Then

                If Not transfer_data.filter_hash_saved Then

                    count_share += 1

                    Dim count = count_share

                    transfer_data.filterChanges = Await web3.Eth.Filters.GetFilterChangesForBlockOrTransaction.SendRequestAsync(transfer_data.pendingFilter)
                    'transfer_data.filterChanges = Await web3.Eth.Filters.GetFilterChangesForBlockOrTransaction.SendRequestAsync(New HexBigInteger("0x7184787bad2360798fc487fc7b77561"))

                    '==============================================================================================================================================================

                    str_filterChanges = ""
                    For i = 0 To transfer_data.filterChanges.Length - 1
                        str_filterChanges &= transfer_data.filterChanges(i) & " "
                    Next
                    record &= count & " : " & transfer_data.transaction_hash_initial & ", " & transfer_data.pendingFilter.Value.ToString & ", " & str_filterChanges & vbCrLf

                    '==============================================================================================================================================================

                    For i = 0 To transfer_data.filterChanges.Length - 1

                        If Not transfer_data.filterChanges(i) = transfer_data.transaction_hash_initial Then

                            transaction = Await web3.Eth.Transactions.GetTransactionByHash.SendRequestAsync(transfer_data.filterChanges(i))

                            If transaction.From.ToLower = transfer_data.public_key.ToLower Then

                                transfer_data.my_length += 1

                                ReDim Preserve transfer_data.my_other_hash(transfer_data.my_length - 1)

                                transfer_data.my_other_hash(transfer_data.my_length - 1) = transfer_data.filterChanges(i)

                            End If

                        End If

                    Next

                    '==============================================================================================================================================================

                    str_filterChanges = ""
                    For i = 0 To transfer_data.filterChanges.Length - 1
                        str_filterChanges &= transfer_data.filterChanges(i) & " "
                    Next
                    record &= count & " : " & transfer_data.transaction_hash_initial & ", " & transfer_data.pendingFilter.Value.ToString & ", " & str_filterChanges & vbCrLf

                    '==============================================================================================================================================================

                    ReDim transfer_data.my_transaction(transfer_data.my_length - 1)
                    ReDim transfer_data.my_receipt(transfer_data.my_length - 1)

                    transfer_data.filter_hash_saved = True

                    'If transfer_data.filterChanges.Length > 1 AndAlso transfer_data.filterChanges(0) = transaction_hash Then
                    '    Dim aa = ""
                    'End If

                    If count = 1 Then

                        '==============================================================================================================================================================

                        str_filterChanges = ""
                        For i = 0 To transfer_data.filterChanges.Length - 1
                            str_filterChanges &= transfer_data.filterChanges(i) & " "
                        Next
                        record &= count & " : " & transfer_data.transaction_hash_initial & ", " & transfer_data.pendingFilter.Value.ToString & ", " & str_filterChanges & vbCrLf

                        '==============================================================================================================================================================

                    Else

                        '==============================================================================================================================================================

                        str_filterChanges = ""
                        For i = 0 To transfer_data.filterChanges.Length - 1
                            str_filterChanges &= transfer_data.filterChanges(i) & " "
                        Next
                        record &= count & " : " & transfer_data.transaction_hash_initial & ", " & transfer_data.pendingFilter.Value.ToString & ", " & str_filterChanges & vbCrLf

                        '==============================================================================================================================================================

                        Dim aa = ""

                    End If

                End If

                transfer_data = Await check_nonce(mode, web3, transfer_data)

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

                transfer_data = Await check_nonce(mode, web3, transfer_data)

            End If

        End If

        Return transfer_data

    End Function

    Shared Async Function check_nonce(mode As String, web3 As Web3, transfer_data As transfer_ethereum.st_data) As Task(Of transfer_ethereum.st_data)

        Dim nonce_too_low_my_transaction As Boolean
        Dim NonceService As InMemoryNonceService
        Dim futureNonce As HexBigInteger

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

                    nonce_too_low_my_transaction = True

                    Exit For

                End If

            End If

        Next

        ' 스스로의 논스도 다이렉트로 확인한다.

        If Not nonce_too_low_my_transaction Then

            NonceService = New InMemoryNonceService(transfer_data.public_key, GRT.GR.ethereum.web3.Client)

            futureNonce = Await NonceService.GetNextNonceAsync()

            If futureNonce.Value > transfer_data.nonce.Value Then

                If mode = "initial" Then

                    transfer_data.nonce_too_low_in_initial_phase = True

                Else

                    transfer_data.nonce_too_low_in_cancel_phase = True

                End If

            End If

        End If

        Return transfer_data

    End Function

End Class
