Public Class self_input_and_relay_old
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
        Dim json_to_send As Linq.JObject

        new_block_hash = generate_new_block_hash.exe(pure_query, previous_hash)

        ' block_number 의 연속성 여부는 검증할 필요 없다.
        ' 연속이 되지 않으면 received_block_hash 가 new_block_hash 와 연속이 될 수 없기 때문이다.

        If new_block_hash = received_block_hash Or received_block_hash = "initial" Then

            Dim block_hash_duplicated = GRT.check_duplicated_block_hash.exe(new_block_hash) ' 이것은 이제 필요없는 듯.

            Dim signiture_key_duplicated = GRT.check_duplicated_signiture_key.exe(signiture) ' 이것은 매우 중요. 1초 내로 동일한 사인이 들어올 수는 있다.

            If Not block_hash_duplicated And Not signiture_key_duplicated Then

                database_name_array = Regex.Split(Regex.Replace(database_name, "\s*", ""), ",")
                table_name_array = Regex.Split(Regex.Replace(table_name, "\s*", ""), ",")

                Dim treated_query As String = GRT.get_treated_query_main.exe(command_key, block_number, database_name_array, table_name_array, pure_query, query_type)

                Call GRT.execute_query.exe(block_number, new_block_hash, public_key, database_name, table_name, query_type, contract_type, pure_query, signiture, treated_query)

                ' p2p 전송

                json_to_send = CType(JsonConvert.DeserializeObject(JSS), Linq.JObject)

                If received_block_hash = "initial" Then ' 메인인 경우만 이에 해당한다.

                    json_to_send("value")("block_hash") = new_block_hash
                    JSS = CType(JsonConvert.SerializeObject(json_to_send), String)

                End If

                Call Task.Run(Sub() send_p2p_relay.exe(json_to_send, JSS))

                ' 영수증 전송

                receipt = GRT.issue_receipt.exe(JSS)

                JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "success", "quot"}}, {{"receipt", receipt, "quot"}}, True)

            Else

                If block_hash_duplicated Then ' 이것은 이제 필요없는 듯.

                    JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", "duplicated_block_hash", "quot"}}, False)

                Else

                    JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", "duplicated_signiture_key", "quot"}}, False)

                End If

            End If

        Else

            ' 이로써 자신이 잘못된 것이라면 영원히 받지를 못하게 된다. 다시 설치해야 한다.
            ' 보내는 쪽에서 악의적인 경우였다면 이 다음부터는 정상적으로 받아들이게 된다.

            JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", "block_hash_not_match", "quot"}}, False)

        End If

        Return JRS

    End Function

End Class
