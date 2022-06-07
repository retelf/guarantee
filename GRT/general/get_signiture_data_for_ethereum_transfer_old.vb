Imports System.Numerics
Imports Nethereum.Hex.HexTypes
Imports Nethereum.RPC.Eth.DTOs
Imports Nethereum.RPC.NonceServices
Imports Nethereum.RPC.TransactionManagers
Imports Nethereum.Signer
Imports Nethereum.Web3.Accounts

Public Class get_signiture_data_for_ethereum_transfer_old

    Public Structure st_info

        Dim nonce As HexBigInteger
        Dim signiture_data_for_ethereum_transfer As String()

    End Structure : Public Shared info As st_info
    Public Shared Async Function exe(
                                    command_key As String,
                                    eoa_from As String,
                                    private_key As String,
                                    eoa_to As String,
                                    amount_eth As Decimal,
                                    fee_rate As Decimal,
                                    Optional gasPrice_biginteger As BigInteger = Nothing,
                                    Optional gasLimit As BigInteger = Nothing
                                    ) As Task(Of Boolean)

        ' na, ma, sa 등으로부터 오는 경우가 실제상 문제된다. eoa 로부터 오는 경우는 지갑을 별도로 동시적으로 사용하지 않는 이상 논스중복은 발생하지 않기 때문이다.
        ' 애당초 중복논스가 발생하지 않도록 

        Dim signiture_data_for_ethereum_transfer(1) As String

        Dim futureNonce As HexBigInteger

        Dim account = New Account(private_key, GRT.GR.account.ethereum_chain_id)

        Dim amount_wei As BigInteger = CType(amount_eth * (1 + fee_rate) * 1000000000000000000, BigInteger)

        Dim callInput = CType(EtherTransferTransactionInputBuilder.CreateTransactionInput(eoa_from, eoa_to, amount_eth), CallInput)

        If gasPrice_biginteger = 0 Then
            gasPrice_biginteger = Await GRT.GR.ethereum.web3.Eth.GasPrice.SendRequestAsync
        End If

        If gasLimit = 0 Then
            gasLimit = GRT.GR.ethereum.web3.Eth.TransactionManager.EstimateGasAsync(callInput).Result
        End If

        If command_key = "submit_cancel" Or command_key = "submit_recall" Then

            amount_wei -= gasPrice_biginteger * gasLimit

        End If

        account.NonceService = New InMemoryNonceService(account.Address, GRT.GR.ethereum.web3.Client)

        futureNonce = Await account.NonceService.GetNextNonceAsync()
        'Dim currentNonce = Await GRT.GR.ethereum.web3.Eth.Transactions.GetTransactionCount.SendRequestAsync(account.Address, BlockParameter.CreatePending())

        info.nonce = futureNonce

        Dim OfflineTransactionSigner = New TransactionSigner()

        signiture_data_for_ethereum_transfer(0) = OfflineTransactionSigner.SignTransaction(private_key, CType(GRT.GR.account.ethereum_chain_id, BigInteger), eoa_to, amount_wei, futureNonce.Value, gasPrice_biginteger, gasLimit)

        signiture_data_for_ethereum_transfer(1) = OfflineTransactionSigner.SignTransaction(private_key, CType(GRT.GR.account.ethereum_chain_id, BigInteger), eoa_from, 0, futureNonce.Value, gasPrice_biginteger / 100 * 125, gasLimit)

        info.signiture_data_for_ethereum_transfer = signiture_data_for_ethereum_transfer

        Return True

    End Function

    Shared Async Function get_transaction_count(eoa_from As String) As Task(Of Nethereum.Hex.HexTypes.HexBigInteger)

        Dim web3 = New Web3(GR.account.ethereum_web3_url)

        Dim txCount = Await web3.Eth.Transactions.GetTransactionCount.SendRequestAsync(eoa_from)

        Return txCount

    End Function

End Class
