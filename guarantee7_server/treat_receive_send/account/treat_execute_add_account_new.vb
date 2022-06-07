Imports Nethereum.Web3

Public Class treat_execute_add_account_new

    Public Shared Async Function exe(json As Newtonsoft.Json.Linq.JObject, JSS As String) As Task(Of String)

        Dim representative_from_dataset, eoa_new, eoa_representative, coin_name, node, idate_string As String
        Dim balance, balance_etc As Decimal
        Dim command_key, pure_query, query_signiture_by_new_public_key, query_signiture_representative, JRS As String

        Dim block_number As Long
        Dim database_name, table_name, query_type, contract_type As String
        Dim received_block_hash, previous_hash As String
        Dim idate As Date

        command_key = json("key").ToString

        received_block_hash = json("value")("block_hash").ToString
        eoa_new = json("value")("public_key").ToString
        eoa_representative = json("value")("public_key_representative").ToString
        coin_name = json("value")("coin_name").ToString
        node = json("value")("node").ToString
        balance = CDec(json("value")("balance"))
        balance_etc = CDec(json("value")("balance_etc"))
        idate_string = json("value")("idate_string").ToString
        query_signiture_by_new_public_key = json("value")("signiture_by_new_public_key").ToString
        query_signiture_representative = json("value")("signiture").ToString

        ' 먼저 검증

        pure_query = GRT.GQS_insert_account_pure_query.exe(eoa_new, eoa_representative, balance, {balance_etc}, node, idate_string)

        Dim verified_new As Boolean = GRT.Security.Gverify.verify(pure_query, query_signiture_by_new_public_key, eoa_new)
        Dim verified_representative As Boolean = GRT.Security.Gverify.verify(pure_query, query_signiture_representative, eoa_representative)

        If verified_new And verified_representative Then

            Dim Dataset As DataSet = GRT.check_representative.exe(eoa_representative)

            If Dataset.Tables(0).Rows.Count = 1 Then

                representative_from_dataset = CStr(Dataset.Tables(0).Rows(0)("representative"))

                If eoa_representative = "0x" & representative_from_dataset Then

                    ' server 자신의 bc 의 main 입력

                    Call GRT.set_block_number_and_get_previous_block_hash.exe()

                    block_number = GRT.set_block_number_and_get_previous_block_hash.data.block_number
                    previous_hash = GRT.set_block_number_and_get_previous_block_hash.data.previous_hash
                    database_name = "bc_manager, bc, bc"
                    table_name = "account, account, account"
                    query_type = "INSERT"
                    contract_type = command_key
                    idate = Date.Now

                    JRS = Await self_input_and_relay.exe(command_key, block_number, received_block_hash, previous_hash, eoa_representative, database_name, table_name, query_type, contract_type, pure_query, query_signiture_representative, JSS)

                Else

                    JRS = "{""key"" : """ & command_key & """, ""success"" : ""success"", ""value"": {""representative"": ""NO""}}"

                End If

            Else
                JRS = "{""key"" : """ & command_key & """, ""success"" : ""fail"", ""value"": {""eoa"": """ & eoa_representative & """, ""reason"": ""no_account""}}"
            End If

        Else

            If Not verified_new And Not verified_representative Then

                JRS = "{""key"" : """ & command_key & """, ""success"" : ""fail"", ""value"": {""eoa"": """ & eoa_representative & """, ""reason"": ""ineffective_account_and_non_representative""}}"

            ElseIf Not verified_new Then

                JRS = "{""key"" : """ & command_key & """, ""success"" : ""fail"", ""value"": {""eoa"": """ & eoa_representative & """, ""reason"": ""ineffective_account""}}"

            Else ' Not verified_representative

                JRS = "{""key"" : """ & command_key & """, ""success"" : ""fail"", ""value"": {""eoa"": """ & eoa_representative & """, ""reason"": ""non_representative""}}"

            End If

        End If

        Return JRS

    End Function

End Class
