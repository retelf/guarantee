Imports System.Collections.Generic
Imports System.Linq
Imports System.Numerics
Imports Nethereum.ABI
Imports Nethereum.Contracts.Services
Imports Nethereum.RLP
Imports Nethereum.Signer
Imports Xunit
Public Class FunctionEncodingDecoding

    Public Shared Sub exe()

        'Call AssertStringCollection(signiture_data_for_ethereum_transfer(0))

        'Dim stringType As New Nethereum.ABI.StringType()

        'Dim RLPElement = Nethereum.RLP.RLP.Decode(HexToByte(signiture_data_for_ethereum_transfer(0))).RLPData

        'Dim str = RLPElement.ToStringFromRLPDecoded()

        'Dim RLPCollection = CType(RLPElement, RLPCollection)

        'Dim arr(8) As String
        'Dim arr1(8) As String
        'Dim arr2(8) As String
        'Dim arr3(8) As String

        'Dim i_number As Integer = -1

        'For Each rpl In RLPCollection

        '    Try

        '        i_number += 1

        '        arr(i_number) = Encoding.Default.GetString(rpl.RLPData)
        '        arr1(i_number) = Encoding.Unicode.GetString(rpl.RLPData)
        '        arr2(i_number) = Encoding.UTF8.GetString(rpl.RLPData)
        '        arr3(i_number) = Encoding.ASCII.GetString(rpl.RLPData)

        '    Catch ex As Exception

        '        Dim aa = ""

        '    End Try

        'Next

        'Dim aa = OfflineTransactionSigner.VerifyTransaction(signiture_data_for_ethereum_transfer(0))

        'Dim result = Nethereum.ABI.ABIType.Decode(signiture_data_for_ethereum_transfer(0), Type.GetType("String"))

        'Dim decoder = New Nethereum.ABI.Decoders.StringTypeDecoder

        'Dim bytes As Byte() = HexToByte(signiture_data_for_ethereum_transfer(0))

        'Dim result = decoder.Decode(bytes)

        'Dim result = Nethereum.ABI.ABIType.Decode(bytes)

        'Dim ethApi As EthApiContractService = New EthApiContractService(Nothing)

    End Sub

    <Fact>
    Public Shared Sub ShouldDecodeInt()
        Dim abi = "[{""constant"":false,""inputs"":[{""name"":""a"",""type"":""uint256""}],""name"":""multiply"",""outputs"":[{""name"":""d"",""type"":""uint256""}],""type"":""function""}]"
        Dim ethApi = New EthApiContractService(Nothing)
        Dim contract = ethApi.GetContract(abi, "ContractAddress")
        Dim multiplyFunction = contract.GetFunction("multiply")
        Dim data = multiplyFunction.GetData(69)
        Dim decode = multiplyFunction.DecodeInput(data)
        Assert.Equal(69, CType(decode(0).Result, BigInteger))
    End Sub

    <Fact>
    Public Shared Sub ShouldDecodeMultipleParamsIncludingArray()
        Dim abi = "[{""constant"":false,""inputs"":[{""name"":""a"",""type"":""uint256""},{""name"":""b"",""type"":""string""},{""name"":""c"",""type"":""uint[3]""} ],""name"":""test"",""outputs"":[{""name"":""d"",""type"":""uint256""}],""type"":""function""}]"
        Dim ethApi = New EthApiContractService(Nothing)
        Dim contract = ethApi.GetContract(abi, "ContractAddress")
        Dim testFunction = contract.GetFunction("test")
        Dim array = {1, 2, 3}
        Dim str = "hello"
        Dim data = testFunction.GetData(69, str, array)
        'data = "0x67043cae0000000000000000000000005a9dac9315fdd1c3d13ef8af7fdfeb522db08f020000000000000000000000000000000000000000000000000000000058a20230000000000000000000000000000000000000000000000000000000000040293400000000000000000000000000000000000000000000000000000000000000a0f3df64775a2dfb6bc9e09dced96d0816ff5055bf95da13ce5b6c3f53b97071c800000000000000000000000000000000000000000000000000000000000000034254430000000000000000000000000000000000000000000000000000000000"
        Dim decode = testFunction.DecodeInput(data)
        Assert.Equal(69, CType(decode(0).Result, BigInteger))
        Assert.Equal(str, CStr(decode(1).Result))
        Dim listObjects = TryCast(decode(2).Result, List(Of Object))

        If listObjects IsNot Nothing Then
            Dim newArray = listObjects.[Select](Function(x) CInt(CType(x, BigInteger))).ToArray()
            Assert.Equal(array, newArray)
        End If
    End Sub
    <Fact>
    Public Overridable Sub ShouldDecodeNegativeIntString()
        Dim intType As IntType = New IntType("int")
        Dim result = intType.Decode(Of BigInteger)("0xffffffffffffffffffffffffffffffffffffffffffffffffffffffffffed2979")
        Assert.Equal(New BigInteger(-1234567), result)
    End Sub

    <Theory>
    <InlineData("-1000000000", "0xffffffffffffffffffffffffffffffffffffffffffffffffffffffffc4653600")>
    <InlineData("-124346577657532", "0xffffffffffffffffffffffffffffffffffffffffffffffffffff8ee84e68e144")>
    <InlineData("127979392992", "0x0000000000000000000000000000000000000000000000000000001dcc2a8fe0")>
    <InlineData("-37797598375987353", "0xffffffffffffffffffffffffffffffffffffffffffffffffff79b748d76fb767")>
    <InlineData("3457987492347979798742", "0x0000000000000000000000000000000000000000000000bb75377716692498d6")>
    Public Overridable Sub ShouldDecode(ByVal expected As String, ByVal hex As String)
        Dim intType As IntType = New IntType("int")
        Dim result = intType.Decode(Of BigInteger)(hex)
        Assert.Equal(expected, result.ToString())
    End Sub

    'Shared Async Function get_transaction_count(eoa_from As String) As Task(Of Nethereum.Hex.HexTypes.HexBigInteger)

    '    Dim web3 = New Web3(GR.account.ethereum_web3_url)

    '    Dim txCount = Await web3.Eth.Transactions.GetTransactionCount.SendRequestAsync(eoa_from)

    '    Return txCount

    'End Function

    Public Shared Function HexToByte(ByVal hex As String) As Byte()

        Return Enumerable.Range(0, hex.Length).Where(Function(x) x Mod 2 = 0).[Select](Function(x) Convert.ToByte(hex.Substring(x, 2), 16)).ToArray()

    End Function

    'Public Shared Sub AssertStringEncoding(ByVal test As String, ByVal expected As String)
    '    Dim testBytes As Byte() = test.ToBytesForRLPEncoding()
    '    Dim encoderesult As Byte() = RLP.EncodeElement(testBytes)
    '    'Assert.Equal(expected, encoderesult.ToHex())
    '    Dim decodeResult = RLP.Decode(encoderesult).RLPData
    '    Dim eee = decodeResult.ToStringFromRLPDecoded()
    '    'Assert.Equal(test, decodeResult.ToStringFromRLPDecoded())
    'End Sub

    Public Shared Sub RlpDecode(ByVal test As String)

        Dim decodedList = Nethereum.RLP.RLP.Decode(HexToByte(test))
        Dim decodedData = New List(Of Byte())()
        Dim decodedElements = CType(decodedList, RLPCollection)
        Dim Signature As EthECDSASignature

        Dim count = decodedElements.Count

        For i = 0 To count - 1
            decodedData.Add(decodedElements(i).RLPData)
        Next

        'If decodedElements(count - 1).RLPData IsNot Nothing Then
        '    Dim v = decodedElements(count - 1).RLPData(0)
        '    Dim r = decodedElements(count + 1).RLPData
        '    Dim s = decodedElements(count + 2).RLPData
        '    Signature = EthECDSASignatureFactory.FromComponents(r, s, v)
        'End If

        Dim Data = decodedData.ToArray()

        Dim decoded = True

    End Sub

    'Private Shared Sub AssertStringCollection(ByVal test As String)

    '    Dim testBytes As Byte() = test.ToBytesForRLPEncoding()
    '    Dim encoderesult As Byte() = RLP.EncodeList(RLP.EncodeElement(testBytes))
    '    'Assert.Equal(expected, encoderesult.ToHex())
    '    Dim decodeResult = TryCast(RLP.Decode(encoderesult), RLPCollection)

    '    Dim str As String

    '    For i As Integer = 0 To decodeResult.Count - 1
    '        str = decodeResult(i).RLPData.ToStringFromRLPDecoded()
    '    Next

    'End Sub

End Class
