Imports Nethereum.Web3
Imports Nethereum.Web3.Accounts
Imports Newtonsoft.Json.Linq

Public Class ethereum_transfer_for_multilevel_seize

    Public Shared Async Function exe(signiture_key As String, json As Newtonsoft.Json.Linq.JObject) As Task(Of Nethereum.RPC.Eth.DTOs.TransactionReceipt)

        Dim eoa, case_retored_eoa_from, na, coin_name, signiture, signiture_for_get_balance, password, private_key, idate_string As String
        Dim signiture_data_for_ethereum_transfer(1) As String
        Dim amount As Decimal
        Dim ethereum_transaction_type As String
        Dim web3 As Web3

        Dim receipt As New Nethereum.RPC.Eth.DTOs.TransactionReceipt()

        receipt.TransactionHash = "0x0"

        eoa = json("value")("eoa_signer").ToString
        coin_name = "ethereum"

        na = json("value")("buyer_na").ToString
        amount = CDec(json("value")("amount"))
        signiture_for_get_balance = json("value")("signiture_for_get_balance").ToString
        signiture = json("value")("signiture").ToString
        ethereum_transaction_type = json("value")("ethereum_transaction_type").ToString
        idate_string = json("value")("idate_string").ToString

        Try

            Select Case ethereum_transaction_type

                Case "signiture" ' 자체검증.

                    case_retored_eoa_from = json("value")("case_retored_eoa_from").ToString
                    signiture_data_for_ethereum_transfer(0) = json("value")("signiture_data_for_ethereum_transfer").ToString
                    signiture_data_for_ethereum_transfer(1) = json("value")("signiture_data_for_ethereum_transfer_cancel").ToString

                    If case_retored_eoa_from = Web3.OfflineTransactionSigner.GetSenderAddress(signiture_data_for_ethereum_transfer(0)) Then

                        Await GRT.transfer_ethereum_old.SendRawTransaction_SendRequestAsync(signiture_key, signiture_data_for_ethereum_transfer(0))

                        receipt.Logs = JArray.Parse("[{""key"": ""verified""}]")

                    Else

                        receipt.Logs = JArray.Parse("[{""key"": ""error"", ""reason"": ""데이터 변조""}]")

                    End If

                Case "password" ' 서버에 키파일이 저장되어 있는 경우. 거래소 방식.

                    password = json("value")("password").ToString

                    private_key = GRT.decrypt_keystore_file.exe(coin_name, password, eoa).ToLower

                    Dim account = New Account(private_key, GRT.GR.account.ethereum_chain_id)

                    web3 = New Web3(account)

                    receipt = Await web3.Eth.GetEtherTransferService().TransferEtherAndWaitForReceiptAsync(na, amount)

                Case "private_key" ' private_key 를 직접 보낸 경우

                    private_key = json("value")("private_key").ToString

                    Dim account = New Account(private_key, GRT.GR.account.ethereum_chain_id)

                    web3 = New Web3(account)

                    receipt = Await web3.Eth.GetEtherTransferService().TransferEtherAndWaitForReceiptAsync(na, amount)

            End Select

        Catch ex As Exception

            receipt.HasErrors()
            receipt.Logs = JArray.Parse("[{""key"": ""error"", ""reason"": """ & ex.Message & """}]")

        End Try

        Return receipt

    End Function

End Class
