Public Class self_input_and_relay
    Public Shared Async Function exe(
                              command_key As String,
                              block_number As Long,
                              received_block_hash As String,
                              previous_hash As String,
                              public_key As String,
                              database_name As String,
                              table_name As String,
                              query_type As String,
                              contract_type As String,
                              pure_query As String,
                              signiture As String,
                              JSS As String) As Task(Of String)

        Dim new_block_hash, receipt, JRS, database_name_array(), table_name_array() As String

        Dim signiture_key = Regex.Match(signiture, "^0x.{64}").ToString

        Dim json_to_send As Linq.JObject = CType(JsonConvert.DeserializeObject(JSS), Linq.JObject)

        database_name_array = Regex.Split(Regex.Replace(database_name, "\s*", ""), ",")
        table_name_array = Regex.Split(Regex.Replace(table_name, "\s*", ""), ",")

        Dim treated_query As String = GRT.get_treated_query_main.exe(command_key, block_number, database_name_array, table_name_array, pure_query, query_type, json_to_send)

        new_block_hash = generate_new_block_hash.exe(treated_query, previous_hash)

        ' signiture_key 는 중복사용 체킹만을 위해 존재한다.
        ' block_hash 가 변조방지를 위한 것이다. 서로의 용도가 다르다.
        ' block_number 의 연속성 여부는 검증할 필요 없다.
        ' 연속이 되지 않으면 received_block_hash 가 new_block_hash 와 연속이 될 수 없기 때문이다.

        If new_block_hash = received_block_hash Or received_block_hash = "initial" Then

            If Not GRT.GR.node_level = 0 Then
                Call GRT.agent_record.state_update("received_block_hash_ok", "", signiture_key) ' 에이전트 서버가 최초로 접수한 경우만 빼고는 아무런 변화를 일으크지 않게 된다. 
            End If

            Dim signiture_key_duplicated = GRT.check_duplicated_signiture_key.exe(signiture)
            ' 이것은 매우 중요. 1초 내로 동일한 사인이 들어올 수는 있다. block_hash_duplicated 로는 대체 불가능이다. block_hash_duplicated 가 같을 경우는 없기 때문이다.

            If Not signiture_key_duplicated Then

                If Not GRT.GR.node_level = 0 Then
                    Call GRT.agent_record.state_update("signiture_key_not_duplicated", "", signiture_key) ' 에이전트 서버가 최초로 접수한 경우만 빼고는 아무런 변화를 일으크지 않게 된다. 
                End If

                Call GRT.execute_query.exe(block_number, new_block_hash, public_key, database_name, table_name, query_type, contract_type, pure_query, signiture, treated_query)

                If GRT.GR.node_level = 0 Then
                    Call GRT.agent_record.state_update("query_executed", "", signiture_key)
                End If

                ' p2p 전송

                If received_block_hash = "initial" Then ' 메인인 경우만 이에 해당한다.

                    json_to_send("value")("block_hash") = new_block_hash
                    JSS = CType(JsonConvert.SerializeObject(json_to_send), String)

                End If

                Call Task.Run(Sub() send_p2p_relay.exe(json_to_send, JSS))

                ' 영수증 전송

                receipt = GRT.issue_receipt.exe(JSS)

                JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "success", "quot"}}, {{"receipt", receipt, "quot"}}, True)

            Else

                JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", "duplicated_signiture_key", "quot"}}, False)

            End If

        Else

            ' 이로써 자신이 잘못된 것이라면 영원히 받지를 못하게 된다. 다시 설치해야 한다.
            ' 보내는 쪽에서 악의적인 경우였다면 이 다음부터는 정상적으로 받아들이게 된다.

            Dim reason As String = "received_block_hash_not_match"

            If Not GRT.GR.node_level = 0 Then
                Call GRT.agent_record.state_update(reason, "", signiture_key) ' 에이전트 서버가 최초로 접수한 경우만 빼고는 아무런 변화를 일으키지 않게 된다. 
            End If

            JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", reason, "quot"}}, False)

        End If

        Return JRS

    End Function

End Class
