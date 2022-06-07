Public Class treat_submit_mobile_authentication_number_main

    Public Shared Async Function exe(json As Newtonsoft.Json.Linq.JObject, JSS As String) As Task(Of String)

        Dim block_number, mobile_authentication_number As Integer
        Dim command_key, super_account_pure_query, general_account_pure_query, database_name, table_name, database_name_array(), table_name_array(), query_type, contract_type As String
        Dim received_block_hash, previous_hash, new_block_hash, algorithm, signiture_for_super_account, node, signiture_for_general_account, query_signiture_by_previous_hash, public_key, idate_string As String
        Dim idate As Date
        Dim verified As Boolean

        Dim dataset As DataSet = GRT.check_mobile_authentication_number.exe(json)

        command_key = json("key").ToString

        If dataset.Tables(0).Rows.Count = 1 Then

            ' 인증번호 일치여부 확인

            mobile_authentication_number = CInt(dataset.Tables(0).Rows(0)("mobile_authentication_number"))

            If CInt(json("value")("mobile_authentication_number")) = mobile_authentication_number Then

                received_block_hash = json("value")("block_hash").ToString
                public_key = json("value")("public_key").ToString
                node = json("value")("node").ToString
                signiture_for_general_account = json("value")("signiture").ToString
                idate_string = json("value")("idate_string").ToString

                super_account_pure_query = CStr(dataset.Tables(0).Rows(0)("pure_query")) ' 여기서 시큐러티가 필요하다.

                general_account_pure_query = GRT.GQS_insert_account_pure_query.exe(public_key, public_key, 0, {0}, node, idate_string)

                verified = GRT.Security.Gverify.verify(general_account_pure_query, signiture_for_general_account, public_key)

                If verified Then

                    signiture_for_super_account = CStr(dataset.Tables(0).Rows(0)("signiture"))

                    Call set_additional_data_for_mobile_authentication.exe("bc_nts", "super_account", "INSERT", "register_super_account")

                    block_number = set_additional_data_for_mobile_authentication.data.block_number
                    previous_hash = set_additional_data_for_mobile_authentication.data.previous_hash
                    database_name = set_additional_data_for_mobile_authentication.data.database_name
                    table_name = set_additional_data_for_mobile_authentication.data.table_name
                    query_type = set_additional_data_for_mobile_authentication.data.query_type
                    contract_type = set_additional_data_for_mobile_authentication.data.contract_type
                    algorithm = set_additional_data_for_mobile_authentication.data.algorithm
                    idate = set_additional_data_for_mobile_authentication.data.idate

                    query_signiture_by_previous_hash = GRT.Security.Gsign.sign(super_account_pure_query, previous_hash)
                    new_block_hash = Regex.Match(query_signiture_by_previous_hash, "0x.{64}").ToString
                    new_block_hash = Regex.Replace(new_block_hash, "^0x", "")

                    database_name_array = Regex.Split(Regex.Replace(database_name, "\s*", ""), ",")
                    table_name_array = Regex.Split(Regex.Replace(table_name, "\s*", ""), ",")

                    ' bc_nts 의 super_account 입력 ' 이것을 먼저 해야 한다.

                    Dim treated_query As String = GRT.get_treated_query_main.exe("insert_super_account", block_number, database_name_array, table_name_array, super_account_pure_query, query_type)

                    Call GRT.execute_nts_query.exe(block_number, new_block_hash, public_key, database_name, table_name, query_type, contract_type, super_account_pure_query, signiture_for_super_account, algorithm, treated_query)

                    ' server 자신의 bc 의 main 입력

                    Call GRT.set_block_number_and_get_previous_block_hash.exe()

                    block_number = GRT.set_block_number_and_get_previous_block_hash.data.block_number
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

            Else

                Return "{""key"" : """ & command_key & """, ""success"" : ""fail"", ""value"" : {""reason"": ""인증번호가 일치하지 않습니다..""} }"

            End If

        Else

            Return "{""key"" : """ & command_key & """, ""success"" : ""fail"", ""value"" : {""reason"": ""회원등록에 실패하였습니다.""} }"

        End If

    End Function

End Class
