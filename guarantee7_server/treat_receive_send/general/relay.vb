Public Class relay

    Public Shared Function exe(json As Newtonsoft.Json.Linq.JObject, json_message As String) As String

        Dim block_number As Integer
        Dim command_key, public_key, pure_query_for_verify, pure_query_escaped, database_name, table_name, database_name_array(), table_name_array(), query_type, contract_type As String
        Dim query_signiture_by_private_key, query_signiture_by_previous_hash As String
        Dim self_previous_hash, new_block_hash, received_block_hash As String
        Dim idate As Date
        Dim receipt As String
        Dim local_max_block_number As Long

        command_key = json("command_key").ToString

        block_number = CInt(json("value")("block_number"))
        received_block_hash = json("value")("block_hash").ToString
        public_key = json("value")("eoa").ToString
        database_name = json("value")("database_name").ToString
        table_name = json("value")("table_name").ToString
        query_type = json("value")("query_type").ToString
        contract_type = json("value")("contract_type").ToString
        pure_query_escaped = json("value")("query_string").ToString
        pure_query_for_verify = Regex.Replace(pure_query_escaped, "_quot_double_", "\""")
        pure_query_for_verify = Regex.Replace(pure_query_for_verify, "_quot_single_", "\'")
        query_signiture_by_private_key = json("value")("signiture").ToString
        idate = CDate(json("value")("idate"))

        database_name_array = Regex.Split(Regex.Replace(database_name, "\s*", ""), ",")
        table_name_array = Regex.Split(Regex.Replace(table_name, "\s*", ""), ",")

        local_max_block_number = GRT.get_local_main_max_block_number.exe

        self_previous_hash = GRT.get_local_block_hash.exe(local_max_block_number)

        query_signiture_by_previous_hash = GRT.Security.Gsign.sign(pure_query_for_verify, self_previous_hash)
        new_block_hash = Regex.Match(query_signiture_by_previous_hash, "0x.{64}").ToString
        new_block_hash = Regex.Replace(new_block_hash, "^0x", "")

        If Not block_number = local_max_block_number + 1 Then

            ' Call GRT.syncronize_main_p2p.exe("relay") ' 최초 설치시에 아직 스키마도 갖춰지지 않은 상태에서 실행되면 안된다. 이 때를 대비한 코드를 만들어 주어야 한다.

            Return "{""key"" : ""error"", ""success"" : ""fail"", ""receipt"" : ""블록넘버 건너뜀이 발생했습니다. 순서대로 체인을 구성하는 작업을 시작해야 합니다."" }"

        ElseIf Not new_block_hash = received_block_hash Then

            Return "{""key"" : ""error"", ""success"" : ""fail"", ""receipt"" : ""데이터 변조"" }"

        Else

            ' 일반 입력 - 이것을 먼저 해야 한다. 실패하면 bc 의 main 에 입력되어서는 안되므로

            Dim treated_query As String = GRT.get_treated_query_main.exe(command_key, block_number, database_name_array, table_name_array, pure_query_for_verify, query_type)

            Call GRT.execute_treated_query.exe(treated_query, database_name)

            ' server 자신의 bc 의 main 입력

            Call GRT.insert_bc_main.exe(block_number, new_block_hash, public_key, database_name, table_name, query_type, contract_type, pure_query_for_verify, query_signiture_by_private_key)

            ' 부여받은 역할에 따른 p2p 전송

            If json("key").ToString = "relay" Then

                Call send_p2p_relay.exe(json, json_message)

            End If

            ' 영수증 발급. (전달받은 거래를 스스로에게 입력시켜 분산블록 형성이 되었으므로)

            receipt = GRT.issue_receipt.exe(json_message)

            Return "{""key"" : ""relay_complete"", ""success"" : ""success"", ""receipt"" : " & receipt & "}"

        End If

    End Function

End Class
