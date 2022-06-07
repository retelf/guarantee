Imports System.Numerics
Imports Nethereum.Hex.HexTypes
Imports Nethereum.RLP

Public Class check_offline_transaction_data

    Public Shared Function exe(eoa_to As String,
                               amount As Decimal,
                               gasPrice As Decimal,
                               gasLimit As Decimal,
                               case_retored_eoa_from As String,
                               nonce As HexBigInteger,
                               signiture_data_for_ethereum_transfer() As String
                               ) As Boolean

        Dim nonce_decoded As HexBigInteger
        Dim gasPrice_decoded, gasLimit_decoded, amount_decoded As Decimal
        Dim eoa_to_decoded As String

        Dim receipt As New Nethereum.RPC.Eth.DTOs.TransactionReceipt()
        receipt.TransactionHash = "0x0"

        Dim case_retored_eoa_from_for_check = Web3.OfflineTransactionSigner.GetSenderAddress(signiture_data_for_ethereum_transfer(0))

        If case_retored_eoa_from = case_retored_eoa_from_for_check Then

            'https://medium.com/codechain-kr/%EC%9B%B9%EC%9C%BC%EB%A1%9C-rlp-%EB%94%94%EB%B2%84%EA%B9%85%ED%95%98%EA%B8%B0-d36048d4b556
            'https://eth.wiki/en/fundamentals/rlp
            'https://codechain-io.github.io/rlp-debugger/

            Dim decodedList = Nethereum.RLP.RLP.Decode(HexToBytes(signiture_data_for_ethereum_transfer(0)))
            Dim decodedData = New List(Of Byte())()
            Dim decodedElements = CType(decodedList, RLPCollection)

            Dim count = decodedElements.Count

            For i = 0 To count - 1
                decodedData.Add(decodedElements(i).RLPData)
            Next

            Dim decodedData_array = decodedData.ToArray()

            nonce_decoded = New HexBigInteger(CType(ByteTolong(decodedData_array(0)), BigInteger))
            gasPrice_decoded = ByteToDecimal(decodedData_array(1)) / 1000000000
            gasLimit_decoded = ByteToDecimal(decodedData_array(2))
            eoa_to_decoded = "0x" & Regex.Replace(BitConverter.ToString(decodedData_array(3)), "-", "")
            amount_decoded = ByteToDecimal(decodedData_array(4)) / 1000000000000000000

            If nonce = nonce_decoded And
                gasPrice = gasPrice_decoded And
                gasLimit = gasLimit_decoded And
                eoa_to.ToLower = eoa_to_decoded.ToLower And
                amount = amount_decoded Then

                Return True

            Else
                Return False
            End If

        Else
            Return False
        End If

    End Function

    Shared Function HexToBytes(ByVal hex As String) As Byte()

        Return Enumerable.Range(0, hex.Length).Where(Function(x) x Mod 2 = 0).[Select](Function(x) Convert.ToByte(hex.Substring(x, 2), 16)).ToArray()

    End Function

    Shared Function ByteTolong(ByVal bytes As Byte()) As Long

        Dim hex = BitConverter.ToString(bytes)
        Dim lng = Convert.ToInt64(Regex.Replace(hex, "-", ""), 16)

        Return lng

    End Function

    Shared Function ByteToDecimal(ByVal bytes As Byte()) As Decimal

        Dim hex = BitConverter.ToString(bytes)
        Dim lng = Convert.ToInt64(Regex.Replace(hex, "-", ""), 16)
        Dim dec = Convert.ToDecimal(lng)

        Return dec

    End Function

End Class
