Imports System.Numerics
Imports Nethereum.Hex.HexTypes
Imports Nethereum.RPC.Eth.DTOs
Imports Nethereum.Web3
Imports Nethereum.Web3.Accounts
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Public Class transfer_ethereum

    Public Structure st_data

        Dim public_key As String
        Dim command_key As String
        Dim transfer_ethereum_from_exchange_case As Boolean
        Dim transaction_hash_initial As String
        Dim transaction_hash_cancel As String
        Dim receipt_initial As Nethereum.RPC.Eth.DTOs.TransactionReceipt
        Dim receipt_cancel As Nethereum.RPC.Eth.DTOs.TransactionReceipt
        Dim json_receipt_initial As Linq.JObject ' 이 두가지는 sender 소켓에서 입력된다.
        Dim json_receipt_cancel As Linq.JObject ' 이 두가지는 sender 소켓에서 입력된다.
        Dim tem_transaction_success_initial As Boolean
        Dim tem_transaction_success_cancel As Boolean
        Dim initial_success As Boolean
        Dim need_cancel_transfer As Boolean
        Dim cancel_success As Boolean
        Dim nonce_too_low_in_initial_phase As Boolean
        Dim nonce_too_low_in_cancel_phase As Boolean
        Dim initial_error_message As String
        Dim cancel_error_message As String

        Dim pendingFilter As HexBigInteger
        Dim filterChanges() As String ' pending_ethereum_tx_hashes 와 같은 의미이다. 단 10분 전부터 펜딩 상태이다.
        Dim filter_hash_saved As Boolean
        Dim FilterLogs() As FilterLog
        Dim nonce As HexBigInteger

        Dim my_other_hash() As String
        Dim my_transaction() As Transaction
        Dim my_receipt() As TransactionReceipt
        Dim my_length As Integer

    End Structure

    Public Shared Async Function SendRawTransaction_SendRequestAsync(
                                                                    public_key As String,
                                                                    signiture_key As String,
                                                                    signiture_data_for_ethereum_transfer() As String,
                                                                    command_key As String,
                                                                    transfer_ethereum_from_exchange_case As Boolean,
                                                                    nonce As HexBigInteger) As Task(Of st_data)

        Dim transfer_data As New st_data
        Dim web3 = New Web3()

        transfer_data.public_key = public_key
        transfer_data.command_key = command_key
        transfer_data.transfer_ethereum_from_exchange_case = transfer_ethereum_from_exchange_case
        transfer_data.nonce = nonce

        transfer_data.filter_hash_saved = False
        ReDim transfer_data.FilterLogs(-1)
        'transfer_data.pendingFilter = Await web3.Eth.Filters.NewPendingTransactionFilter.SendRequestAsync()
        transfer_data.pendingFilter = pending_filter_management.pendingFilter

        transfer_data.receipt_initial = Nothing
        transfer_data.receipt_cancel = Nothing
        transfer_data.json_receipt_initial = Nothing
        transfer_data.json_receipt_cancel = Nothing
        transfer_data.transaction_hash_initial = Nothing
        transfer_data.transaction_hash_cancel = Nothing
        transfer_data.tem_transaction_success_initial = False
        transfer_data.tem_transaction_success_cancel = False
        transfer_data.initial_success = False
        transfer_data.need_cancel_transfer = Nothing
        transfer_data.cancel_success = False
        transfer_data.initial_error_message = Nothing
        transfer_data.cancel_error_message = Nothing

        transfer_data.my_length = 0
        ReDim transfer_data.my_other_hash(-1)
        ReDim transfer_data.my_transaction(-1)
        ReDim transfer_data.my_receipt(-1)

        transfer_data.nonce_too_low_in_initial_phase = False
        transfer_data.nonce_too_low_in_cancel_phase = False

        Call agent_record.state_update("transaction_requested", "", signiture_key)

        Call GRT.nonce_management.exe_update(transfer_data.public_key, "last_received", transfer_data.nonce, Nothing)

        transfer_data = Await transfer_ethereum_sub_send.exe("initial", signiture_key, signiture_data_for_ethereum_transfer(0), transfer_data)

        If transfer_data.tem_transaction_success_initial Then

            transfer_data = Await transfer_ethereum_sub_check_result.exe("initial", signiture_key, transfer_data.transaction_hash_initial, transfer_data)

            If transfer_data.initial_success Then

                Call GRT.nonce_management.exe_update(public_key, "last_confirmed", nonce, transfer_data.transaction_hash_initial)

            Else

                If transfer_data.nonce_too_low_in_initial_phase Then

                    ' 여기서 처리하지 말고 treat_ethereum_transfer_sub 에서 처리하자.

                Else ' 여기서부터 cancel 모드이다. 일반 에러는 이리로 오지 않는다. 오로지 펜딩 상태만 온다.

                    ' 펜딩 상태라도 command_key 나 transfer_ethereum_from_exchange_case 의 상태에 따라서 캔슬을 하지 않아도 되는 경우를 살핀다.

                    Select Case command_key

                        Case "submit_transfer" : transfer_data.need_cancel_transfer = True

                        Case "submit_load_order" : transfer_data.need_cancel_transfer = True ' 이곳으로 왔다는 것은 이더리움 로딩이라는 뜻이므로

                        Case "submit_exchange" : transfer_data.need_cancel_transfer = True

                        ' 클라이언트가 클라이언트에게 보내는 경우에는 당연 캔슬
                        ' 거래소가 클라이언트에게 보내는 경우에는 캔슬을 실시하고 만약 그것도 실패하는 경우에는 그냥 개런티 실시를 해 버린다.

                        Case "submit_cancel" : transfer_data.need_cancel_transfer = True

                        Case "submit_buy" : transfer_data.need_cancel_transfer = True

                        Case "submit_recall" : transfer_data.need_cancel_transfer = True ' submit_exchange 와 같다.

                        Case "submit_nft_buy" : transfer_data.need_cancel_transfer = True

                        Case "submit_nft_recall" : transfer_data.need_cancel_transfer = False

                        Case Else

                            transfer_data.need_cancel_transfer = True

                    End Select

                    If transfer_data.need_cancel_transfer Then

                        transfer_data = Await transfer_ethereum_sub_send.exe("cancel", signiture_key, signiture_data_for_ethereum_transfer(1), transfer_data)

                        If transfer_data.tem_transaction_success_cancel Then

                            transfer_data = Await transfer_ethereum_sub_check_result.exe("cancel", signiture_key, transfer_data.transaction_hash_cancel, transfer_data)

                            If transfer_data.cancel_success Then

                                Call GRT.nonce_management.exe_update(public_key, "last_confirmed", nonce, transfer_data.transaction_hash_cancel)

                            Else ' 그냥 보내면 된다.

                            End If

                        Else
                            ' 그냥 보내면 된다. initial_send 에서 위 transfer_data 는 이미 완성되었다.
                        End If

                    Else
                        ' 일단 그냥 보낸다. 특별히 여기서 더 할 일은 없어 보인다.
                    End If

                End If

            End If

        Else
            ' 그냥 보내면 된다. initial_send 에서 위 transfer_data 는 이미 완성되었다.
        End If

        Return transfer_data

    End Function

End Class
