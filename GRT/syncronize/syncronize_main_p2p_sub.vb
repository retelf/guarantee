Imports System.Windows.Forms

Public Class syncronize_main_p2p_sub

    Public Shared Async Sub exe(json_receipt As Newtonsoft.Json.Linq.JObject)

        Dim signiture As String
        Dim count As Integer
        Dim block_hash, eoa, database_name, table_name, database_name_array(), table_name_array(), contract_type, query_type, pure_query, treated_query As String
        Dim block_number As Long
        Dim previous_hash As String
        Dim hash_verified, sign_verified As Boolean

        count = json_receipt("value").Count

        For i = 0 To count - 1

            block_number = CLng(json_receipt("value")(i)("value")("block_number"))
            block_hash = CStr(json_receipt("value")(i)("value")("block_hash"))
            eoa = CStr(json_receipt("value")(i)("value")("eoa"))
            database_name = CStr(json_receipt("value")(i)("value")("database_name"))
            table_name = CStr(json_receipt("value")(i)("value")("table_name"))
            contract_type = CStr(json_receipt("value")(i)("value")("contract_type"))
            query_type = CStr(json_receipt("value")(i)("value")("query_type"))
            pure_query = CStr(json_receipt("value")(i)("value")("query_string"))
            pure_query = Regex.Replace(pure_query, "_quot_double_", "\""")
            pure_query = Regex.Replace(pure_query, "_quot_single_", "\'")
            treated_query = CStr(json_receipt("value")(i)("value")("treated_query_string"))
            treated_query = Regex.Replace(treated_query, "_quot_double_", "\""")
            treated_query = Regex.Replace(treated_query, "_quot_single_", "\'")
            signiture = CStr(json_receipt("value")(i)("value")("signiture"))

            database_name_array = Regex.Split(Regex.Replace(database_name, "\s*", ""), ",")
            table_name_array = Regex.Split(Regex.Replace(table_name, "\s*", ""), ",")

            ' 이들에 대해 하나씩

            ' verify 실시 

            If block_number = 1 Then
                previous_hash = GRT.GR.genesis_block_hash
            Else
                previous_hash = get_local_block_hash.exe(block_number - 1)
            End If

            hash_verified = GRT.get_main_table_block_hash.exe(treated_query, previous_hash, "0x" & block_hash)

            sign_verified = GRT.Security.Gverify.verify(pure_query, signiture, "0x" & eoa)

            If hash_verified And sign_verified Then

                ' 해당 명령실행

                'Dim treated_query As String = get_treated_query_main.exe(contract_type, block_number, database_name_array, table_name_array, pure_query, query_type)

                Call GRT.execute_query.exe(block_number, block_hash, eoa, database_name, table_name, query_type, contract_type, pure_query, signiture, treated_query)

            Else

                Dim message As String

                If Not hash_verified And sign_verified Then

                    message = "central server block chain cut off at block number : " & block_number

                ElseIf Not sign_verified And hash_verified Then

                    message = "central server block chain query """ & pure_query & """(block number : " & block_number & ") is failed in verification"

                Else

                    message = "central server block chain cut off at block number : " & block_number &
                            " and also " &
                            "central server block chain query """ & pure_query & """(block number : " & block_number & ") is failed in verification"

                End If

                MessageBox.Show(message)

            End If

            previous_hash = "0x" & block_hash

        Next

    End Sub

End Class
