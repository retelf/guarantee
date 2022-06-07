Imports Nethereum.Web3

Public Class treat_execute_add_account_already

    Public Shared Async Function exe(json As Newtonsoft.Json.Linq.JObject, JSS As String) As Task(Of String)

        Dim command_key, representative_for_check, eoa_representative, eoa_etc_coin, coin_name, node, idate_string As String
        Dim balance, balance_etc As Decimal
        Dim balance_wei_for_checking As Nethereum.Hex.HexTypes.HexBigInteger
        Dim pure_query, query_signiture_by_private_key, query_signiture_by_private_key_etc_coin, JRS As String

        Dim block_number As Long
        Dim database_name, table_name, query_type, contract_type As String
        Dim received_block_hash, previous_hash As String
        Dim idate As Date

        command_key = json("key").ToString

        received_block_hash = json("value")("block_hash").ToString
        eoa_representative = json("value")("eoa_representative").ToString
        eoa_etc_coin = json("value")("public_key_etc_coin").ToString
        coin_name = json("value")("coin_name").ToString
        node = json("value")("node").ToString
        balance = CDec(json("value")("balance"))
        balance_etc = CDec(json("value")("balance_etc"))
        idate_string = json("value")("idate_string").ToString
        query_signiture_by_private_key = json("value")("signiture").ToString
        query_signiture_by_private_key_etc_coin = json("value")("signiture_etc_coin").ToString

        ' 먼저 검증

        pure_query = GRT.GQS_insert_account_pure_query.exe(eoa_etc_coin, eoa_representative, balance, {balance_etc}, node, idate_string)

        Dim verified As Boolean = GRT.Security.Gverify.verify(pure_query, query_signiture_by_private_key, eoa_representative)
        Dim verified_etc_coin As Boolean = GRT.Security.Gverify.verify(pure_query, query_signiture_by_private_key_etc_coin, eoa_etc_coin)

        If verified And verified_etc_coin Then

            ' 밸런스를 다시 한 번 확인하고

            Dim web3 = New Web3(GRT.GR.account.ethereum_web3_url)
            balance_wei_for_checking = Await web3.Eth.GetBalance.SendRequestAsync(eoa_etc_coin)
            'Dim balance_decimal_for_checking As Decimal = GRT.convert_hexbiginteger_decimal_unit_ethereum.exe(balance_wei_for_checking)
            Dim balance_decimal_for_checking As Decimal = CDec(balance_wei_for_checking.Value)

            If balance_etc = balance_decimal_for_checking Then

                Dim Dataset As DataSet = GRT.check_representative.exe(eoa_representative)

                If Dataset.Tables(0).Rows.Count = 1 Then

                    representative_for_check = CStr(Dataset.Tables(0).Rows(0)("representative")) ' 이 장면에서 일반적인 eoa 의 존재도 확인된다.

                    If eoa_representative = "0x" & representative_for_check Then

                        ' server 자신의 bc 의 main 입력

                        Call GRT.set_block_number_and_get_previous_block_hash.exe()

                        block_number = GRT.set_block_number_and_get_previous_block_hash.data.block_number
                        previous_hash = GRT.set_block_number_and_get_previous_block_hash.data.previous_hash
                        database_name = "bc_manager, bc, bc"
                        table_name = "account, account, account"
                        query_type = "INSERT"
                        contract_type = command_key
                        idate = Date.Now

                        JRS = Await self_input_and_relay.exe(command_key, block_number, received_block_hash, previous_hash, eoa_representative, database_name, table_name, query_type, contract_type, pure_query, query_signiture_by_private_key, JSS)

                    Else

                        JRS = "{""key"" : """ & command_key & """, ""success"" : ""success"", ""value"": {""representative"": ""NO""}}"

                    End If

                Else
                    JRS = "{""key"" : """ & command_key & """, ""success"" : ""fail"", ""value"": {""eoa"": """ & eoa_representative & """, ""reason"": ""no_account""}}"
                End If

            Else
                JRS = "{""key"" : """ & command_key & """, ""success"" : ""fail"", ""value"": {""eoa"": """ & eoa_representative & """, ""reason"": ""balance_not_match""}}"
            End If

        Else

            JRS = "{""key"" : """ & command_key & """, ""success"" : ""fail"", ""value"": {""eoa"": """ & eoa_representative & """, ""reason"": ""데이터 변조""}}"

        End If

        Return JRS

    End Function

End Class
