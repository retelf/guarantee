Public Class GJS_for_initial_distribute

    Public Shared Function exe(command_key As String, block_number As Long, block_hash As String, eoa As String, database_name As String, table_name As String, contract_type As String, query_type As String, pure_query As String, signiture As String, idate_string As String) As String

        Dim escaped_pure_query = Regex.Replace(pure_query, "\\""", "_quot_double_")
        escaped_pure_query = Regex.Replace(escaped_pure_query, "\\'", "_quot_single_")

        Dim json_message As String =
                    "{""relay"" : ""yes"", " &
                    """key"": """ & command_key & """, " &
                    """database"": ""bc"", " &
                    """table"": ""main"", " &
                    """value"" : {" &
                    """block_number"" : " & block_number & ", " &
                    """block_hash"" : """ & block_hash & """, " &
                    """eoa"" : """ & Regex.Replace(eoa, "^0x", "") & """, " &
                    """database_name"" : """ & database_name & """, " &
                    """table_name"" : """ & table_name & """, " &
                    """contract_type"" : """ & contract_type & """, " &
                    """query_type"" : """ & query_type & """, " &
                    """query_string"" : """ & escaped_pure_query & """, " & ' json 전송을 위해
                    """signiture"" : """ & signiture & """, " &
                    """idate_string"" : """ & idate_string & """}}"

        Return json_message

    End Function

End Class
