Imports Nethereum.Web3

Public Class treat_execute_add_account_ethereum

    Public Shared Async Function exe(json As Newtonsoft.Json.Linq.JObject, JSS As String) As Task(Of String)

        'Dim command_key, representative, eoa, eoa_ethereum, coin_name, node, idate_string As String
        'Dim balance_wei As Decimal
        'Dim balance_wei_for_checking As Nethereum.Hex.HexTypes.HexBigInteger
        'Dim pure_query, query_signiture_by_private_key, query_signiture_by_private_key_ethereum, JRS As String

        'Dim block_number As Long
        'Dim database_name, table_name, query_type, contract_type As String
        'Dim received_block_hash, previous_hash As String
        'Dim idate As Date

        'command_key = json("key").ToString

        'received_block_hash = json("value")("block_hash").ToString
        'eoa = json("value")("public_key").ToString
        'eoa_ethereum = json("value")("public_key_ethereum").ToString
        'coin_name = json("value")("coin_name").ToString
        'node = json("value")("node").ToString
        'balance_wei = CDec(json("value")("balance_wei"))
        'idate_string = json("value")("idate_string").ToString
        'query_signiture_by_private_key = json("value")("signiture").ToString
        'query_signiture_by_private_key_ethereum = json("value")("signiture_ethereum").ToString

        '' 먼저 검증

        'pure_query = GRT.GQS_insert_account_pure_query.exe(eoa_ethereum, eoa, balance_wei, node, idate_string)

        'Dim verified As Boolean = GRT.Security.Gverify.verify(pure_query, query_signiture_by_private_key, eoa)
        'Dim verified_ethereum As Boolean = GRT.Security.Gverify.verify(pure_query, query_signiture_by_private_key_ethereum, eoa_ethereum)

        'If verified And verified_ethereum Then

        '    ' 밸런스를 다시 한 번 확인하고

        '    Dim web3 = New Web3(GRT.GR.account.ethereum_web3_url)

        '    balance_wei_for_checking = Await web3.Eth.GetBalance.SendRequestAsync(eoa_ethereum)

        '    Dim balance_decimal_for_checking As Decimal = GRT.convert_hexbiginteger_decimal_unit_ethereum.exe(balance_wei_for_checking)

        '    If balance_wei = balance_decimal_for_checking * 1000000000000000000 Then

        '        Dim Dataset As DataSet = GRT.check_representative.exe(eoa)

        '        If Dataset.Tables(0).Rows.Count = 1 Then

        '            representative = CStr(Dataset.Tables(0).Rows(0)("representative")) ' 이 장면에서 일반적인 eoa 의 존재도 확인된다.

        '            If eoa = "0x" & representative Then

        '                ' server 자신의 bc 의 main 입력

        '                Call GRT.set_block_number_and_get_previous_block_hash.exe()

        '                block_number = GRT.set_block_number_and_get_previous_block_hash.data.block_number
        '                previous_hash = GRT.set_block_number_and_get_previous_block_hash.data.previous_hash
        '                database_name = "bc, bc_manager"
        '                table_name = "account, account"
        '                query_type = "INSERT"
        '                contract_type = command_key
        '                idate = Date.Now

        '                JRS = Await self_input_and_relay.exe(command_key, block_number, received_block_hash, previous_hash, eoa, database_name, table_name, query_type, contract_type, pure_query, query_signiture_by_private_key, JSS)

        '            Else

        '                JRS = "{""key"" : """ & command_key & """, ""success"" : ""success"", ""value"": {""representative"": ""NO""}}"

        '            End If

        '        Else
        '            JRS = "{""key"" : """ & command_key & """, ""success"" : ""fail"", ""value"": {""eoa"": """ & eoa & """, ""reason"": ""no_account""}}"
        '        End If

        '    Else
        '        JRS = "{""key"" : """ & command_key & """, ""success"" : ""fail"", ""value"": {""eoa"": """ & eoa & """, ""reason"": ""balance_not_match""}}"
        '    End If

        'Else

        '    JRS = "{""key"" : """ & command_key & """, ""success"" : ""fail"", ""value"": {""eoa"": """ & eoa & """, ""reason"": ""데이터 변조""}}"

        'End If

        'Return JRS

    End Function

End Class
