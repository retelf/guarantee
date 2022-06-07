Public Class treat_load_smart_contract

    Public Shared Async Function exe(json As Newtonsoft.Json.Linq.JObject, JSS As String) As Task(Of String)

        Dim command_key, eoa As String
        Dim smart_contract_name As String
        Dim registered As Boolean
        Dim foo_signiture As String
        Dim JRS As String
        Dim sString, cString_smart_contract As String
        Dim Connection_mariadb_local_bc_sc_smart_contract As MySqlConnection
        Dim local_password As String
        Dim count As Integer

        command_key = json("key").ToString
        eoa = json("value")("eoa").ToString

        registered = GRT.check_registered_eoa.exe(eoa, "guarantee")

        If registered Then

            smart_contract_name = json("value")("smart_contract_name").ToString
            foo_signiture = json("value")("signiture").ToString

            local_password = GRT.get_local_database_password.exe

            cString_smart_contract = "server=localhost;port=" & GRT.GR.port_number_database_local & ";database=bc_sc_" & smart_contract_name & ";uid=guarantee7;pwd=" & local_password & ";CharSet=utf8;"
            Connection_mariadb_local_bc_sc_smart_contract = New MySqlConnection(cString_smart_contract)

            Connection_mariadb_local_bc_sc_smart_contract.Open()

            Dim Selectcmd As MySqlCommand
            Dim Adapter As MySqlDataAdapter
            Dim Dataset As DataSet

            sString = "SELECT name, code, extention FROM file"

            Selectcmd = New MySqlCommand(sString, Connection_mariadb_local_bc_sc_smart_contract)
            Selectcmd.CommandType = CommandType.Text
            Adapter = New MySqlDataAdapter
            Adapter.SelectCommand = Selectcmd
            Dataset = New DataSet
            Adapter.Fill(Dataset)

            Connection_mariadb_local_bc_sc_smart_contract.Close()

            If Dataset.Tables(0).Rows.Count > 0 Then

                Dim name, code, extention As String
                Dim header(,), value(-1)(,) As String

                count = Dataset.Tables(0).Rows.Count

                header = {{"key", command_key, "quot"}, {"success", "success", "quot"}}

                For i = 0 To count - 1

                    ReDim Preserve value(i)

                    name = CStr(Dataset.Tables(0).Rows(i)("name"))
                    code = CStr(Dataset.Tables(0).Rows(i)("code"))
                    code = Regex.Replace(code, """", "\""")
                    extention = CStr(Dataset.Tables(0).Rows(i)("extention"))

                    value(i) = {{"name", name, "quot"}, {"code", code, "quot"}, {"extention", extention, "quot"}}

                Next

                JRS = GRT.make_json_string.exe_array(header, value, False)

            Else
                JRS = "{""key"" : ""treat_check_main_key_login"", ""success"" : ""fail"", ""value"": {""reason"": ""no_files""}}"
            End If

        Else

            JRS = "{""key"" : """ & command_key & """, ""success"" : ""fail"", ""value"": {""publick_key"": """ & eoa & """, ""reason"": ""no_account""}}"

        End If

        Return JRS

    End Function

End Class
