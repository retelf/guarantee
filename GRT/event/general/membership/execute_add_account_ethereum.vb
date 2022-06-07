Imports System.ServiceProcess

Public Class execute_add_account_ethereum

    Public Shared Async Function exe(case_insensitive_public_key_ethereum As String, private_key_ethereum As String, node As String, idate_string As String) As Task(Of String)

        'Dim JSS, JRS, pure_query As String

        '' 먼저 public_key_ethereum 를 정상화 시킨다.

        'Dim public_key_ethereum = GRT.restore_ethereum_public_key.exe(case_insensitive_public_key_ethereum, private_key_ethereum)

        '' 입력된 키를 검증하고

        'Dim verified = GRT.Security.Gverify.public_key(public_key_ethereum, private_key_ethereum)

        'If verified Then

        '    ' 밸런스를 확인하고

        '    Dim web3 = New Web3(GR.account.ethereum_web3_url)

        '    Dim balance_wei = Await web3.Eth.GetBalance.SendRequestAsync(public_key_ethereum)

        '    Dim balance_decimal As Decimal = CDec(balance_wei.Value)

        '    'Dim balance_decimal As Decimal = convert_hexbiginteger_decimal_unit_ethereum.exe(balance_wei)
        '    'Dim balance_decimal As Decimal = 100

        '    If Not balance_decimal = 0 Then

        '        pure_query = GRT.GQS_insert_account_pure_query.exe(public_key_ethereum, GR.account.public_key, balance_decimal, node, idate_string)

        '        ' 공개키를 등록한다.

        '        JSS = GRT.make_json_string.exe(
        '            {{"key", "execute_add_account_ethereum", "quot"}},
        '            {
        '            {"block_hash", "initial", "quot"},
        '            {"public_key", GR.account.public_key, "quot"},
        '            {"public_key_ethereum", public_key_ethereum, "quot"},
        '            {"coin_name", "ethereum", "quot"},
        '            {"node", node, "quot"},
        '            {"balance_wei", CStr(balance_decimal), "non_quot"},
        '            {"idate_string", idate_string, "quot"},
        '            {"signiture", GRT.Security.Gsign.sign(pure_query, GR.account.private_key), "quot"},
        '            {"signiture_ethereum", GRT.Security.Gsign.sign(pure_query, private_key_ethereum), "quot"}
        '            }, True)

        '        JRS = Await Task.Run(Function() GRT.socket_client.exe(GRT.GR.main_server_address_agency, GRT.GR.port_number_server_main_agency, GRT.GR.port_number_server_local, JSS))

        '    Else

        '        JRS = "{""key"" : ""execute_add_account_ethereum"", ""success"" : ""fail"", ""reason"" : ""ineffective_or_zero_balance_ethereum_account"" }"

        '    End If

        'Else

        '    JRS = "{""key"" : ""execute_add_account_ethereum"", ""success"" : ""fail"", ""reason"" : ""verification_fail"" }"

        'End If

        'Return JRS

    End Function

End Class
