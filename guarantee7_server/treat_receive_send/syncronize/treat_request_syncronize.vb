Public Class treat_request_syncronize

    Public Shared Function exe(json As Newtonsoft.Json.Linq.JObject) As String

        Dim child_na, eoa As String
        Dim JRS, data_str, data_array_str As String
        Dim replication_start_number, replication_end_number As Long

        Dim Selectcmd As MySqlCommand
        Dim Adapter As MySqlDataAdapter
        Dim Dataset As DataSet

        Dim count As Integer
        Dim received_signiture_by_private_key As String

        Dim block_hash, database_name, table_name, contract_type, query_type, query_string, escaped_query_string, treated_query_string, escaped_treated_query_string, signiture As String
        Dim block_number As Long

        child_na = json("value")("na").ToString
        eoa = json("value")("eoa").ToString
        replication_start_number = CLng(json("value")("replication_start_number"))
        replication_end_number = CLng(json("value")("replication_end_number"))
        received_signiture_by_private_key = json("value")("signiture").ToString

        Dim Connection_mariadb_local_bc As MySqlConnection

        ' 먼저 eoa 진실성 검증

        Dim verified As Boolean = GRT.Security.Gverify.verify("", received_signiture_by_private_key, eoa)

        If verified Then

            ' 자식 서버 아이디가 맞는지와 그 소유자 검증

            verified = check_eoa_and_na.exe(eoa, child_na)

            If verified Then

                ' 데이터 추출 및 리턴

                Connection_mariadb_local_bc = New MySqlConnection(GRT.GR.cString_mariadb_local_bc)

                Connection_mariadb_local_bc.Open()

                'CREATE PROCEDURE `up_select_main_for_syncronize`
                '(IN p_replication_start_number  bigint(20),
                'IN p_replication_end_number  bigint(20))
                'BEGIN
                'SELECT *
                'FROM main
                'WHERE
                '`block_number` BETWEEN p_replication_start_number AND p_replication_end_number
                'ORDER BY `block_number`;
                'END

                Selectcmd = New MySqlCommand("up_select_main_for_syncronize", GRT.GR.Connection_mariadb_local_bc)
                Selectcmd.Parameters.Add(New MySqlParameter("p_replication_start_number", replication_start_number))
                Selectcmd.Parameters.Add(New MySqlParameter("p_replication_end_number", replication_end_number))

                Adapter = New MySqlDataAdapter
                Selectcmd.CommandType = CommandType.StoredProcedure
                Adapter.SelectCommand = Selectcmd
                Dataset = New DataSet
                Adapter.Fill(Dataset)

                Connection_mariadb_local_bc.Close()

                count = Dataset.Tables(0).Rows.Count

                JRS = "{""key"" : ""request_syncronize_result"", ""success"" : ""success"", ""value"": data_array_str}"

                data_array_str = "["

                For i = 0 To count - 1

                    block_number = CLng(Dataset.Tables(0).Rows(i)("block_number"))
                    block_hash = CStr(Dataset.Tables(0).Rows(i)("block_hash"))
                    eoa = CStr(Dataset.Tables(0).Rows(i)("eoa"))
                    database_name = CStr(Dataset.Tables(0).Rows(i)("database_name"))
                    table_name = CStr(Dataset.Tables(0).Rows(i)("table_name"))
                    contract_type = CStr(Dataset.Tables(0).Rows(i)("contract_type"))
                    query_type = CStr(Dataset.Tables(0).Rows(i)("query_type"))
                    query_string = CStr(Dataset.Tables(0).Rows(i)("query_string"))
                    escaped_query_string = Regex.Replace(query_string, "\\""", "_quot_double_")
                    escaped_query_string = Regex.Replace(escaped_query_string, "\\'", "_quot_single_")
                    treated_query_string = CStr(Dataset.Tables(0).Rows(i)("treated_query_string"))
                    escaped_treated_query_string = Regex.Replace(treated_query_string, "\\""", "_quot_double_")
                    escaped_treated_query_string = Regex.Replace(escaped_treated_query_string, "\\'", "_quot_single_")
                    signiture = CStr(Dataset.Tables(0).Rows(i)("signiture"))

                    data_str = GRT.make_json_string.exe(
                        {{"key", "data", "quot"}},
                        {{"block_number", CStr(block_number), "non_quot"},
                        {"block_hash", block_hash, "quot"},
                        {"eoa", eoa, "quot"},
                        {"database_name", database_name, "quot"},
                        {"table_name", table_name, "quot"},
                        {"contract_type", contract_type, "quot"},
                        {"query_type", query_type, "quot"},
                        {"query_string", escaped_query_string, "quot"},
                        {"treated_query_string", escaped_treated_query_string, "quot"},
                        {"signiture", signiture, "quot"}}, True)

                    data_array_str &= data_str & ","

                Next

                data_array_str = Regex.Replace(data_array_str, ",$", "")

                data_array_str &= "]"

                JRS = Regex.Replace(JRS, "data_array_str", data_array_str)

            Else
                JRS = "{""key"" : ""request_syncronize_result"", ""success"" : ""fail"", ""value"": {""reason"": ""verification_fail""}}"
            End If

        Else
            JRS = "{""key"" : ""request_syncronize_result"", ""success"" : ""fail"", ""value"": {""reason"": ""verification_fail""}}"
        End If

        Return JRS

    End Function

End Class
