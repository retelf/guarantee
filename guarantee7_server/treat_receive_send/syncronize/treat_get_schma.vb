Public Class treat_get_schma

    Public Shared Function exe(json As Newtonsoft.Json.Linq.JObject) As String

        Dim child_na, eoa As String
        Dim JRS, data_str, data_array_str, command_type, sql As String
        Dim database_name, table_name, procedure_name, received_database_name, received_table_name, received_procedure_name As String
        Dim create_database_str, create_table_str, create_procedure_str As String

        Dim Selectcmd As MySqlCommand
        Dim Adapter As MySqlDataAdapter
        Dim Dataset As DataSet

        Dim count As Integer
        Dim received_signiture_by_private_key As String

        child_na = json("value")("na").ToString
        eoa = json("value")("eoa").ToString
        command_type = json("value")("command_type").ToString
        received_database_name = json("value")("database_name").ToString
        received_table_name = json("value")("table_name").ToString
        received_procedure_name = json("value")("procedure_name").ToString
        received_signiture_by_private_key = json("value")("signiture").ToString

        Dim Connection_mariadb_local As New MySqlConnection(GRT.GR.cString_mariadb_local)

        Connection_mariadb_local.Open()

        ' 먼저 eoa 진실성 검증

        Dim verified As Boolean = GRT.Security.Gverify.verify("", received_signiture_by_private_key, eoa)

        If verified Then

            ' 자식 서버 아이디가 맞는지와 그 소유자 검증

            verified = check_eoa_and_na.exe(eoa, child_na)

            If verified Then

                Select Case command_type

                    Case "database_name"

                        sql = "SHOW DATABASES;"

                    Case "show_create_database"

                        sql = "SHOW CREATE DATABASE " & received_database_name & ";"

                    Case "table_name"

                        sql = "USE " & received_database_name & "; SHOW TABLES;"

                    Case "show_create_table"

                        sql = "USE " & received_database_name & "; SHOW CREATE TABLE " & received_table_name & ";"

                    Case "procedure_name"

                        sql = "USE " & received_database_name & "; SHOW PROCEDURE STATUS WHERE db = '" & received_database_name & "';"

                    Case "show_create_procedure"

                        sql = "USE " & received_database_name & "; SHOW CREATE PROCEDURE " & received_procedure_name & ";"

                End Select

                Selectcmd = New MySqlCommand(sql, Connection_mariadb_local) ' main
                Selectcmd.CommandType = CommandType.Text

                Adapter = New MySqlDataAdapter
                Adapter.SelectCommand = Selectcmd
                Dataset = New DataSet
                Adapter.Fill(Dataset)

                Connection_mariadb_local.Close()

                Select Case command_type

                    Case "database_name"

                        count = Dataset.Tables(0).Rows.Count

                        JRS = "{""key"" : ""database_name"", ""success"" : ""success"", ""value"": data_array_str}"

                        data_array_str = "["

                        For i = 0 To count - 1

                            database_name = CStr(Dataset.Tables(0).Rows(i)(0))

                            data_str = GRT.make_json_string.exe(
                                                            {{"key", "data", "quot"}},
                                                            {
                                                            {"database_name", database_name, "quot"}
                                                            }, True)

                            data_array_str &= data_str & ","

                        Next

                        data_array_str = Regex.Replace(data_array_str, ",$", "")

                        data_array_str &= "]"

                        JRS = Regex.Replace(JRS, "data_array_str", data_array_str)

                    Case "show_create_database"

                        JRS = "{""key"" : ""show_create_database"", ""success"" : ""success"", ""value"": data_array_str}"

                        data_array_str = "["

                        create_database_str = CStr(Dataset.Tables(0).Rows(0)(1))

                        data_str = GRT.make_json_string.exe(
                                                        {{"key", "data", "quot"}},
                                                        {
                                                        {"create_database_str", create_database_str, "quot"}
                                                        }, True)

                        data_array_str &= data_str

                        data_array_str &= "]"

                        JRS = Regex.Replace(JRS, "data_array_str", data_array_str)

                    Case "table_name"

                        count = Dataset.Tables(0).Rows.Count

                        JRS = "{""key"" : ""table_name"", ""success"" : ""success"", ""value"": data_array_str}"

                        data_array_str = "["

                        For i = 0 To count - 1

                            table_name = CStr(Dataset.Tables(0).Rows(i)(0))

                            data_str = GRT.make_json_string.exe(
                                                            {{"key", "data", "quot"}},
                                                            {
                                                            {"table_name", table_name, "quot"}
                                                            }, True)

                            data_array_str &= data_str & ","

                        Next

                        data_array_str = Regex.Replace(data_array_str, ",$", "")

                        data_array_str &= "]"

                        JRS = Regex.Replace(JRS, "data_array_str", data_array_str)

                    Case "show_create_table"

                        JRS = "{""key"" : ""show_create_table"", ""success"" : ""success"", ""value"": data_array_str}"

                        data_array_str = "["

                        create_table_str = CStr(Dataset.Tables(0).Rows(0)(1))

                        data_str = GRT.make_json_string.exe(
                                                        {{"key", "data", "quot"}},
                                                        {
                                                        {"create_table_str", create_table_str, "quot"}
                                                        }, True)

                        data_array_str &= data_str

                        data_array_str &= "]"

                        JRS = Regex.Replace(JRS, "data_array_str", data_array_str)

                    Case "procedure_name"

                        count = Dataset.Tables(0).Rows.Count

                        JRS = "{""key"" : ""procedure_name"", ""success"" : ""success"", ""value"": data_array_str}"

                        data_array_str = "["

                        For i = 0 To count - 1

                            procedure_name = CStr(Dataset.Tables(0).Rows(i)("Name"))

                            data_str = GRT.make_json_string.exe(
                                                            {{"key", "data", "quot"}},
                                                            {
                                                            {"procedure_name", procedure_name, "quot"}
                                                            }, True)

                            data_array_str &= data_str & ","

                        Next

                        data_array_str = Regex.Replace(data_array_str, ",$", "")

                        data_array_str &= "]"

                        JRS = Regex.Replace(JRS, "data_array_str", data_array_str)

                    Case "show_create_procedure"

                        If Dataset.Tables.Count > 0 Then

                            JRS = "{""key"" : ""show_create_procedure"", ""success"" : ""success"", ""value"": data_array_str}"

                            data_array_str = "["

                            create_procedure_str = CStr(Dataset.Tables(0).Rows(0)("Create Procedure"))

                            data_str = GRT.make_json_string.exe(
                                                            {{"key", "data", "quot"}},
                                                            {
                                                            {"create_procedure_str", create_procedure_str, "quot"}
                                                            }, True)

                            data_array_str &= data_str

                            data_array_str &= "]"

                            JRS = Regex.Replace(JRS, "data_array_str", data_array_str)

                        Else
                            JRS = "{""key"" : ""show_create_procedure"", ""success"" : ""success"", ""value"": []}"
                        End If

                End Select

            Else
                JRS = "{""key"" : """ & command_type & """, ""success"" : ""fail"", ""value"": {""reason"": ""verification_fail""}}"
            End If

        Else
            JRS = "{""key"" : """ & command_type & """, ""success"" : ""fail"", ""value"": {""reason"": ""verification_fail""}}"
        End If

        Return JRS

    End Function

End Class
