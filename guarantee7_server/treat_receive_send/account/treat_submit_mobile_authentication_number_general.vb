Public Class treat_submit_mobile_authentication_number_general

    Public Shared Async Function exe(json As Newtonsoft.Json.Linq.JObject, JSS As String) As Task(Of String)

        Dim block_number As Integer
        Dim command_key, general_account_pure_query, database_name, table_name, query_type, contract_type As String
        Dim previous_hash, received_block_hash, node, signiture_for_general_account, public_key, idate_string As String
        Dim verified As Boolean

        command_key = json("key").ToString

        public_key = json("value")("public_key").ToString
        node = json("value")("node").ToString
        signiture_for_general_account = json("value")("signiture").ToString
        idate_string = json("value")("idate_string").ToString

        general_account_pure_query = GRT.GQS_insert_account_pure_query.exe(public_key, public_key, 0, {0}, node, idate_string)

        verified = GRT.Security.Gverify.verify(general_account_pure_query, signiture_for_general_account, public_key)

        If verified Then

            ' server 자신의 bc 의 main 입력

            Call GRT.set_block_number_and_get_previous_block_hash.exe()

            block_number = GRT.set_block_number_and_get_previous_block_hash.data.block_number
            received_block_hash = json("value")("block_hash").ToString
            previous_hash = GRT.set_block_number_and_get_previous_block_hash.data.previous_hash
            database_name = "bc, bc_manager"
            table_name = "account, account"
            query_type = "INSERT"
            contract_type = command_key

            Return Await self_input_and_relay.exe(command_key, block_number, received_block_hash, previous_hash, public_key, database_name, table_name, query_type, contract_type, general_account_pure_query, signiture_for_general_account, JSS)

        Else

            Dim error_message = "보내주신 자료가 변조되었습니다."

            Return "{""key"" : """ & command_key & """, ""success"" : ""fail"", ""value"" : {""reason"": """ & error_message & """} }"

        End If

    End Function

End Class
