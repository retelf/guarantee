Imports Newtonsoft.Json

Public Class replicate_schema_from_p2p
    Public Shared Async Sub exe(parent_server_address_agency As String, port_number_server_parent_agency As Integer)

        Dim database_name, table_name, procedure_name, create_database_str, create_table_str, create_procedure_str As String
        Dim count, count_database As Integer
        Dim json_receipt_database_name, json_receipt_table_name, json_receipt_procedure_name As Newtonsoft.Json.Linq.JObject
        Dim json_receipt_show_create_database, json_receipt_show_create_table, json_receipt_show_create_procedure As Newtonsoft.Json.Linq.JObject

        ' 데이터베이스 명칭 가져오기

        Dim JSS, JRS As String

        Dim na = GRT.get_local_server_id.exe

        Dim Connection_mariadb_local As New MySqlConnection(GRT.GR.cString_mariadb_local)

        Connection_mariadb_local.Open()

        JSS = GRT.make_json_string.exe({{"key", "get_schma", "quot"}}, {{"na", na, "quot"}, {"eoa", GRT.GR.account.public_key, "quot"}, {"command_type", "database_name", "quot"}, {"database_name", "", "quot"}, {"table_name", "", "quot"}, {"procedure_name", "", "quot"}, {"signiture", GRT.Security.Gsign.sign("foo", GRT.GR.account.private_key), "quot"}}, True)

        JRS = Await Task.Run(Function() GRT.socket_client.exe(parent_server_address_agency, port_number_server_parent_agency, GRT.GR.port_number_server_local, JSS))

        json_receipt_database_name = CType(JsonConvert.DeserializeObject(JRS), Linq.JObject)

        count_database = json_receipt_database_name("value").Count

        For i = 0 To count_database - 1

            database_name = CStr(json_receipt_database_name("value")(i)("value")("database_name"))

            If (Regex.Match(database_name, "^bc$").Success Or Regex.Match(database_name, "^bc_").Success) And Not database_name = "bc_nts" Then

                JSS = GRT.make_json_string.exe({{"key", "get_schma", "quot"}}, {{"na", na, "quot"}, {"eoa", GRT.GR.account.public_key, "quot"}, {"command_type", "show_create_database", "quot"}, {"database_name", database_name, "quot"}, {"table_name", "", "quot"}, {"procedure_name", "", "quot"}, {"signiture", GRT.Security.Gsign.sign("foo", GRT.GR.account.private_key), "quot"}}, True)

                JRS = Await Task.Run(Function() GRT.socket_client.exe(parent_server_address_agency, port_number_server_parent_agency, GRT.GR.port_number_server_local, JSS))

                json_receipt_show_create_database = CType(JsonConvert.DeserializeObject(JRS), Linq.JObject)

                create_database_str = json_receipt_show_create_database("value")(0)("value")("create_database_str").ToString

                Call real_exe("create_database", database_name, Nothing, Nothing, create_database_str, Connection_mariadb_local)

                ' CREATE TABLE 

                JSS = GRT.make_json_string.exe({{"key", "get_schma", "quot"}}, {{"na", na, "quot"}, {"eoa", GRT.GR.account.public_key, "quot"}, {"command_type", "table_name", "quot"}, {"database_name", database_name, "quot"}, {"table_name", "", "quot"}, {"procedure_name", "", "quot"}, {"signiture", GRT.Security.Gsign.sign("foo", GRT.GR.account.private_key), "quot"}}, True)

                JRS = Await Task.Run(Function() GRT.socket_client.exe(parent_server_address_agency, port_number_server_parent_agency, GRT.GR.port_number_server_local, JSS))

                json_receipt_table_name = CType(JsonConvert.DeserializeObject(JRS), Linq.JObject)

                count = json_receipt_table_name("value").Count

                For j = 0 To count - 1

                    table_name = CStr(json_receipt_table_name("value")(j)("value")("table_name"))

                    JSS = GRT.make_json_string.exe({{"key", "get_schma", "quot"}}, {{"na", na, "quot"}, {"eoa", GRT.GR.account.public_key, "quot"}, {"command_type", "show_create_table", "quot"}, {"database_name", database_name, "quot"}, {"table_name", table_name, "quot"}, {"procedure_name", "", "quot"}, {"signiture", GRT.Security.Gsign.sign("foo", GRT.GR.account.private_key), "quot"}}, True)

                    JRS = Await Task.Run(Function() GRT.socket_client.exe(parent_server_address_agency, port_number_server_parent_agency, GRT.GR.port_number_server_local, JSS))

                    json_receipt_show_create_table = CType(JsonConvert.DeserializeObject(JRS), Linq.JObject)

                    create_table_str = json_receipt_show_create_table("value")(0)("value")("create_table_str").ToString

                    Call real_exe("create_table", database_name, table_name, Nothing, create_table_str, Connection_mariadb_local)

                Next

                ' CREATE PROCEDURE 

                JSS = GRT.make_json_string.exe({{"key", "get_schma", "quot"}}, {{"na", na, "quot"}, {"eoa", GRT.GR.account.public_key, "quot"}, {"command_type", "procedure_name", "quot"}, {"database_name", database_name, "quot"}, {"table_name", "", "quot"}, {"procedure_name", "", "quot"}, {"signiture", GRT.Security.Gsign.sign("foo", GRT.GR.account.private_key), "quot"}}, True)

                JRS = Await Task.Run(Function() GRT.socket_client.exe(parent_server_address_agency, port_number_server_parent_agency, GRT.GR.port_number_server_local, JSS))

                json_receipt_procedure_name = CType(JsonConvert.DeserializeObject(JRS), Linq.JObject)

                count = json_receipt_procedure_name("value").Count

                For j = 0 To count - 1

                    procedure_name = CStr(json_receipt_procedure_name("value")(j)("value")("procedure_name"))

                    JSS = GRT.make_json_string.exe({{"key", "get_schma", "quot"}}, {{"na", na, "quot"}, {"eoa", GRT.GR.account.public_key, "quot"}, {"command_type", "show_create_procedure", "quot"}, {"database_name", database_name, "quot"}, {"table_name", "", "quot"}, {"procedure_name", procedure_name, "quot"}, {"signiture", GRT.Security.Gsign.sign("foo", GRT.GR.account.private_key), "quot"}}, True)

                    JRS = Await Task.Run(Function() GRT.socket_client.exe(parent_server_address_agency, port_number_server_parent_agency, GRT.GR.port_number_server_local, JSS))

                    json_receipt_show_create_procedure = CType(JsonConvert.DeserializeObject(JRS), Linq.JObject)

                    create_procedure_str = json_receipt_show_create_procedure("value")(0)("value")("create_procedure_str").ToString

                    Call real_exe("create_procedure", database_name, Nothing, procedure_name, create_procedure_str, Connection_mariadb_local)

                Next

            End If

        Next

        Connection_mariadb_local.Close()

    End Sub

    Shared Sub real_exe(command_type As String, database_name As String, table_name As String, procedure_name As String, sql As String, Connection_mariadb_local As MySqlConnection)

        Dim sqlCommand As MySqlCommand

        Select Case command_type

            Case "create_database"

                sql = "DROP DATABASE IF EXISTS " & database_name & "; " & sql

            Case "create_table"

                sql = "USE " & database_name & "; DROP TABLE IF EXISTS " & table_name & "; " & sql

            Case "create_procedure"

                sql = "USE " & database_name & "; DROP PROCEDURE IF EXISTS " & procedure_name & "; " & sql

        End Select

        sqlCommand = New MySqlCommand(sql, Connection_mariadb_local)
        sqlCommand.CommandType = CommandType.Text

        sqlCommand.ExecuteNonQuery()

    End Sub

End Class
