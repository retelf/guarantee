Imports System.Numerics
Imports Nethereum.Hex.HexTypes
Imports Nethereum.RPC.Eth.DTOs
Imports Nethereum.Web3
Imports Nethereum.Web3.Accounts
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Public Class transfer_ethereum_sub_check_result

    Public Shared Async Function exe(mode As String, signiture_key As String, transaction_hash As String, transfer_data As transfer_ethereum.st_data) As Task(Of transfer_ethereum.st_data)

        Dim web3 = New Web3("http://localhost:8545")

        If mode = "initial" Then
            Call agent_record.transaction_hash_update(transaction_hash, signiture_key)
        Else
            Call agent_record.transaction_hash_cancel_update(transaction_hash, signiture_key)
        End If

        Call agent_record.state_update(mode & "_transaction_hash_generated", "", signiture_key)

        Dim rolling_times As Integer = 0

        While True

            If mode = "initial" Then

                transfer_data = Await check_filterChanges.exe(mode, web3, transaction_hash, transfer_data)

                If transfer_data.receipt_initial Is Nothing And Not transfer_data.nonce_too_low_in_initial_phase Then ' 현재 펜딩상태.

                    rolling_times += 1

                Else ' 영수증 객체 반환

                    If Not transfer_data.receipt_initial Is Nothing Then

                        transfer_data.initial_success = True

                        Call agent_record.state_update("receipt_received", "", signiture_key)

                        Call agent_record.save_receipt(CType(JsonConvert.SerializeObject(transfer_data.receipt_initial), String), signiture_key)

                    Else ' nonce_too_low_in_initial_phase

                        transfer_data.initial_error_message = "nonce_too_low_in_initial_phase"

                        Call agent_record.state_update("receipt_receive_failed", transfer_data.initial_error_message, signiture_key)

                    End If

                    Exit While

                End If

                If rolling_times > GRT.GR.ethereum_transaction_rolling_times Then

                    transfer_data = transfer_ethereum_makepending_so_long_data.exe(mode, signiture_key, transaction_hash, "tx_pending_so_long", "tx_pending_so_long", transfer_data)

                    Exit While

                Else

                    Threading.Thread.Sleep(5000)

                End If

            Else

                transfer_data = Await check_filterChanges.exe(mode, web3, transaction_hash, transfer_data)

                If transfer_data.receipt_initial Is Nothing And
                    transfer_data.receipt_cancel Is Nothing And
                    Not transfer_data.nonce_too_low_in_cancel_phase Then ' 현재 펜딩상태.

                    rolling_times += 1

                Else ' 영수증 객체 반환

                    If Not transfer_data.receipt_initial Is Nothing Then

                        transfer_data.initial_success = True

                        Call agent_record.state_update("initial_receipt_received", "", signiture_key)

                        Call agent_record.save_receipt(CType(JsonConvert.SerializeObject(transfer_data.receipt_initial), String), signiture_key)

                    ElseIf Not transfer_data.receipt_cancel Is Nothing Then

                        transfer_data.cancel_success = True

                        Call agent_record.state_update("cancel_receipt_received", "", signiture_key)

                        Call agent_record.save_receipt(CType(JsonConvert.SerializeObject(transfer_data.receipt_cancel), String), signiture_key)

                    ElseIf transfer_data.nonce_too_low_in_cancel_phase Then

                        transfer_data.cancel_error_message = "nonce_too_low_in_cancel_phase"

                        Call agent_record.state_update("receipt_receive_failed", transfer_data.cancel_error_message, signiture_key)

                    End If

                    Exit While

                End If

                If rolling_times > GRT.GR.ethereum_transaction_rolling_times_for_cancel Then

                    transfer_data = transfer_ethereum_makepending_so_long_data.exe(mode, signiture_key, transaction_hash, "tx_pending_so_long_in_cancel", "tx_pending_so_long_in_cancel", transfer_data)

                    Exit While

                Else

                    Threading.Thread.Sleep(5000)

                End If

            End If

        End While

        Return transfer_data

    End Function

End Class
