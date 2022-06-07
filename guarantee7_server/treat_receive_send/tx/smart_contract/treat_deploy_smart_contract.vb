Public Class treat_deploy_smart_contract

    Public Shared Async Function exe(json As Newtonsoft.Json.Linq.JObject, JSS As String) As Task(Of String)

        Dim block_number As Integer
        Dim pure_query, pure_query_escaped, database_name, table_name, query_type, contract_type As String
        Dim received_block_hash, previous_hash As String

        Dim idate As Date
        Dim command_key, eoa, ca, idate_string As String
        Dim code, escaped_code, extention, code_json_string, create_table_string, file_name, smart_contract_name, industry_name As String
        Dim registered, verified As Boolean
        Dim pure_query_signiture As String

        command_key = json("key").ToString
        eoa = json("value")("eoa").ToString

        registered = GRT.check_registered_eoa.exe(eoa, "guarantee")

        If registered Then

            ' deploy

            received_block_hash = json("value")("block_hash").ToString
            ca = json("value")("ca").ToString
            industry_name = json("value")("industry_name").ToString
            smart_contract_name = json("value")("smart_contract_name").ToString
            escaped_code = json("value")("code_json_string")("value")("escaped_code").ToString
            pure_query_signiture = json("value")("signiture").ToString
            create_table_string = json("value")("code_json_string")("value")("create_table_string").ToString
            extention = json("value")("code_json_string")("value")("extention").ToString
            file_name = json("value")("code_json_string")("value")("file_name").ToString

            code_json_string = GRT.make_json_string.exe(
                                                        {{"key", "code_json_string", "quot"}},
                                                        {
                                                        {"file_name", file_name, "quot"},
                                                        {"escaped_code", escaped_code, "quot"},
                                                        {"extention", extention, "quot"},
                                                        {"create_table_string", create_table_string, "quot"}
                                                        }, True)

            idate_string = json("value")("idate_string").ToString

            Call GRT.set_block_number_and_get_previous_block_hash.exe()

            pure_query_escaped = GRT.GQS_deploy_smart_contract_escaped.exe(eoa, ca, industry_name, smart_contract_name, code_json_string, create_table_string, idate_string)

            pure_query = Regex.Replace(pure_query_escaped, "_quot_double_", "\""")
            pure_query = Regex.Replace(pure_query, "_quot_single_", "\'")

            block_number = GRT.set_block_number_and_get_previous_block_hash.data.block_number
            previous_hash = GRT.set_block_number_and_get_previous_block_hash.data.previous_hash
            database_name = "bc_smart_contract"
            table_name = "contract"
            query_type = "INSERT"
            contract_type = command_key

            verified = GRT.Security.Gverify.verify(pure_query, pure_query_signiture, eoa)

            If verified Then

                Return Await self_input_and_relay.exe(command_key, block_number, received_block_hash, previous_hash, eoa, database_name, table_name, query_type, contract_type, pure_query, pure_query_signiture, JSS)

            Else

                Dim error_message = "보내주신 자료가 변조되었습니다."

                Return "{""key"" : """ & command_key & """, ""success"" : ""fail"", ""value"" : {""reason"": """ & error_message & """} }"

            End If

        Else

            Return "{""key"" : """ & command_key & """, ""success"" : ""fail"", ""value"": {""publick_key"": """ & eoa & """, ""reason"": ""no_account""}}"

        End If

    End Function

End Class
