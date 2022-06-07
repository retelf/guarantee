Imports System.Numerics
Imports Nethereum.Hex.HexTypes
Imports Nethereum.RPC.Eth.DTOs
Imports Nethereum.RPC.NonceServices
Imports Nethereum.RPC.TransactionManagers
Imports Nethereum.Signer
Imports Nethereum.ABI.Decoders
Imports Nethereum.ABI
Imports Nethereum.RLP
Imports System.Text
Imports Xunit
Imports Nethereum.Hex.HexConvertors.Extensions

Public Class get_signiture_data_for_ethereum_transfer

    Public Structure st_info

        Dim nonce As HexBigInteger
        Dim signiture_data_for_ethereum_transfer As String()

    End Structure : Public Shared info As st_info
    Public Shared Async Function exe(
                                    requested_from As String,
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

        If requested_from = "server" Then

            futureNonce = Await nonce_management.exe(eoa_from)

        Else

            futureNonce = Await request_nonce_via_socket.exe(eoa_from)

        End If

        info.nonce = futureNonce

        Dim OfflineTransactionSigner = New TransactionSigner()

        signiture_data_for_ethereum_transfer(0) = OfflineTransactionSigner.SignTransaction(private_key, CType(GRT.GR.account.ethereum_chain_id, BigInteger), eoa_to, amount_wei, futureNonce.Value, gasPrice_biginteger, gasLimit)

        signiture_data_for_ethereum_transfer(1) = OfflineTransactionSigner.SignTransaction(private_key, CType(GRT.GR.account.ethereum_chain_id, BigInteger), eoa_from, 0, futureNonce.Value, gasPrice_biginteger / 100 * 112, gasLimit)

        info.signiture_data_for_ethereum_transfer = signiture_data_for_ethereum_transfer

        Return True

    End Function

End Class


