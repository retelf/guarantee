Public Class get_parent_info

    Public Shared Function exe(requesting_na As String) As String

        Dim parent_na As String
        Dim result As String

        Dim domain As String
        Dim domain_agency As String
        Dim domain_web As String
        Dim domain_ethereum As String
        Dim domain_management As String
        Dim domain_nft As String

        Dim ip As String
        Dim ip_agency As String
        Dim ip_web As String
        Dim ip_ethereum As String
        Dim ip_management As String
        Dim ip_nft As String

        Dim port As Integer
        Dim port_agency As Integer
        Dim port_web As Integer
        Dim port_ethereum As Integer
        Dim port_management As Integer
        Dim port_nft As Integer

        Dim Selectcmd As MySqlCommand
        Dim Adapter As MySqlDataAdapter
        Dim Dataset As DataSet

        Dim Connection_mariadb_local_bc_manager As New MySqlConnection(GRT.GR.cString_mariadb_local_bc_manager)

        Connection_mariadb_local_bc_manager.Open()

        'CREATE PROCEDURE `up_select_parent_na`
        '(IN p_requesting_na char(40))
        'BEGIN
        'SELECT parent
        'FROM node
        'WHERE na = p_requesting_na;
        'END

        Selectcmd = New MySqlCommand("up_select_parent_na", Connection_mariadb_local_bc_manager)
        Selectcmd.Parameters.Add(New MySqlParameter("p_requesting_na", Regex.Replace(requesting_na, "^0x", "")))

        Adapter = New MySqlDataAdapter
        Selectcmd.CommandType = CommandType.StoredProcedure
        Adapter.SelectCommand = Selectcmd
        Dataset = New DataSet
        Adapter.Fill(Dataset)

        If Dataset.Tables(0).Rows.Count = 1 Then

            parent_na = "0x" & CStr(Dataset.Tables(0).Rows(0)(0))

            'CREATE PROCEDURE `up_select_node_thread_info`
            '(IN p_na char(40))
            'BEGIN
            'SELECT * FROM node_thread
            'WHERE na = p_na;
            'END

            Selectcmd = New MySqlCommand("up_select_node_thread_info", Connection_mariadb_local_bc_manager)
            Selectcmd.Parameters.Add(New MySqlParameter("p_na", Regex.Replace(parent_na, "^0x", "")))

            Adapter = New MySqlDataAdapter
            Selectcmd.CommandType = CommandType.StoredProcedure
            Adapter.SelectCommand = Selectcmd
            Dataset = New DataSet
            Adapter.Fill(Dataset)

            If Dataset.Tables(0).Rows.Count = GR.server_thread_count Then

                For i = 0 To Dataset.Tables(0).Rows.Count - 1

                    Dim type = Dataset.Tables(0).Rows(i)("type").ToString

                    Select Case type

                        Case "default"

                            domain = CStr(Dataset.Tables(0).Rows(i)("domain"))
                            ip = CStr(Dataset.Tables(0).Rows(i)("ip"))
                            port = CInt(Dataset.Tables(0).Rows(i)("port"))

                        Case "agency"

                            domain_agency = CStr(Dataset.Tables(0).Rows(i)("domain"))
                            ip_agency = CStr(Dataset.Tables(0).Rows(i)("ip"))
                            port_agency = CInt(Dataset.Tables(0).Rows(i)("port"))

                        Case "web"

                            domain_web = CStr(Dataset.Tables(0).Rows(i)("domain"))
                            ip_web = CStr(Dataset.Tables(0).Rows(i)("ip"))
                            port_web = CInt(Dataset.Tables(0).Rows(i)("port"))

                        Case "ethereum"

                            domain_ethereum = CStr(Dataset.Tables(0).Rows(i)("domain"))
                            ip_ethereum = CStr(Dataset.Tables(0).Rows(i)("ip"))
                            port_ethereum = CInt(Dataset.Tables(0).Rows(i)("port"))

                        Case "management"

                            domain_management = CStr(Dataset.Tables(0).Rows(i)("domain"))
                            ip_management = CStr(Dataset.Tables(0).Rows(i)("ip"))
                            port_management = CInt(Dataset.Tables(0).Rows(i)("port"))

                        Case "nft"

                            domain_nft = CStr(Dataset.Tables(0).Rows(i)("domain"))
                            ip_nft = CStr(Dataset.Tables(0).Rows(i)("ip"))
                            port_nft = CInt(Dataset.Tables(0).Rows(i)("port"))

                    End Select

                Next

                result = GRT.make_json_string.exe(
                                {{"key", "parent_info_result", "quot"}, {"success", "success", "quot"}},
                                {
                                {"domain", domain, "quot"},
                                {"domain_agency", domain_agency, "quot"},
                                {"domain_web", domain_web, "quot"},
                                {"domain_ethereum", domain_ethereum, "quot"},
                                {"domain_management", domain_management, "quot"},
                                {"domain_nft", domain_nft, "quot"},
                                {"ip", ip, "quot"},
                                {"ip_agency", ip_agency, "quot"},
                                {"ip_web", ip_web, "quot"},
                                {"ip_ethereum", ip_ethereum, "quot"},
                                {"ip_management", ip_management, "quot"},
                                {"ip_nft", ip_nft, "quot"},
                                {"port", CStr(port), "non_quot"},
                                {"port_agency", CStr(port_agency), "non_quot"},
                                {"port_web", CStr(port_web), "non_quot"},
                                {"port_ethereum", CStr(port_ethereum), "non_quot"},
                                {"port_management", CStr(port_management), "non_quot"},
                                {"port_nft", CStr(port_nft), "non_quot"}}, False)

            Else
                result = "{""key"" : ""parent_info_result"", ""success"" : ""fail"", ""value"": {""reason"": ""database_error""}}"
            End If

        Else
            result = "{""key"" : ""parent_info_result"", ""success"" : ""fail"", ""value"": {""reason"": ""no_parent""}}"
        End If

        Connection_mariadb_local_bc_manager.Close()

        Return result

    End Function

End Class
