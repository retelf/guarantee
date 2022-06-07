Public Class treat_execute_login

    Public Shared Async Function exe(json As Newtonsoft.Json.Linq.JObject) As Task(Of String)

        Dim public_key, coin_name, node, exchange_name As String
        Dim balance As Decimal
        Dim query_signiture_by_private_key As String
        Dim JSS, JRS, JRS_nts_info As String
        Dim json_JRS_nts_info As Newtonsoft.Json.Linq.JObject

        public_key = json("value")("public_key").ToString
        coin_name = json("value")("coin_name").ToString
        query_signiture_by_private_key = json("value")("signiture").ToString

        ' 먼저 검증

        Dim verified As Boolean = GRT.Security.Gverify.verify("", query_signiture_by_private_key, public_key)

        If verified Then

            Dim Connection_mariadb_local_bc As New MySqlConnection(GRT.GR.cString_mariadb_local_bc)

            Connection_mariadb_local_bc.Open()

            Dim Selectcmd As MySqlCommand
            Dim Adapter As MySqlDataAdapter
            Dim Dataset_bc, Dataset_bc_manager, Dataset, Dataset_node_thread As DataSet

            'CREATE PROCEDURE `up_select_account_info`
            '(IN p_eoa  varchar(40),
            'IN p_coin_name  varchar(50))
            'BEGIN
            'SELECT * FROM account
            'WHERE
            '`eoa` = p_eoa AND
            '`coin_name` = p_coin_name;
            'END

            Selectcmd = New MySqlCommand("up_select_account_info", Connection_mariadb_local_bc)
            Selectcmd.CommandType = CommandType.StoredProcedure
            Selectcmd.Parameters.Add(New MySqlParameter("p_eoa", Regex.Replace(public_key, "^0x", "")))
            Selectcmd.Parameters.Add(New MySqlParameter("p_coin_name", coin_name))

            Adapter = New MySqlDataAdapter
            Adapter.SelectCommand = Selectcmd
            Dataset_bc = New DataSet
            Adapter.Fill(Dataset_bc)

            Connection_mariadb_local_bc.Close()

            Dim Connection_mariadb_local_bc_manager As New MySqlConnection(GRT.GR.cString_mariadb_local_bc_manager)

            Connection_mariadb_local_bc_manager.Open()

            'CREATE PROCEDURE `up_select_account_info`
            '(IN p_eoa char(40))
            'BEGIN
            'SELECT * FROM account
            'WHERE
            '`eoa` = p_eoa;
            'END

            Selectcmd = New MySqlCommand("up_select_account_info", Connection_mariadb_local_bc_manager)
            Selectcmd.CommandType = CommandType.StoredProcedure
            Selectcmd.Parameters.Add(New MySqlParameter("p_eoa", Regex.Replace(public_key, "^0x", "")))
            Selectcmd.Parameters.Add(New MySqlParameter("p_coin_name", coin_name))

            Adapter = New MySqlDataAdapter
            Selectcmd.CommandType = CommandType.StoredProcedure
            Adapter.SelectCommand = Selectcmd
            Dataset_bc_manager = New DataSet
            Adapter.Fill(Dataset_bc_manager)

            If Dataset_bc.Tables(0).Rows.Count = 1 And Dataset_bc_manager.Tables(0).Rows.Count = 1 Then

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

                balance = CDec(Dataset_bc.Tables(0).Rows(0)("balance"))
                node = "0x" & CStr(Dataset_bc_manager.Tables(0).Rows(0)("node"))

                'CREATE PROCEDURE `up_select_node_info`
                '(IN p_na char(40))
                'BEGIN
                'SELECT * FROM node
                'WHERE na = p_na;
                'END

                Selectcmd = New MySqlCommand("up_select_node_info", Connection_mariadb_local_bc_manager)
                Selectcmd.Parameters.Add(New MySqlParameter("p_na", Regex.Replace(node, "^0x", "")))

                Adapter = New MySqlDataAdapter
                Selectcmd.CommandType = CommandType.StoredProcedure
                Adapter.SelectCommand = Selectcmd
                Dataset = New DataSet
                Adapter.Fill(Dataset)

                'CREATE PROCEDURE `up_select_node_thread_info`
                '(IN p_na char(40))
                'BEGIN
                'SELECT * FROM node_thread
                'WHERE na = p_na;
                'END

                Selectcmd = New MySqlCommand("up_select_node_thread_info", Connection_mariadb_local_bc_manager)
                Selectcmd.Parameters.Add(New MySqlParameter("p_na", Regex.Replace(node, "^0x", "")))

                Adapter = New MySqlDataAdapter
                Selectcmd.CommandType = CommandType.StoredProcedure
                Adapter.SelectCommand = Selectcmd
                Dataset_node_thread = New DataSet
                Adapter.Fill(Dataset_node_thread)

                If Dataset.Tables(0).Rows.Count = 1 And Dataset_node_thread.Tables(0).Rows.Count = GRT.GR.server_thread_count Then ' 이미 노드가 등록되어 있는 경우

                    exchange_name = CStr(Dataset.Tables(0).Rows(0)("exchange_name"))

                    For i = 0 To Dataset_node_thread.Tables(0).Rows.Count - 1

                        Dim type = Dataset_node_thread.Tables(0).Rows(i)("type").ToString

                        Select Case type

                            Case "default"

                                domain = CStr(Dataset_node_thread.Tables(0).Rows(i)("domain"))
                                ip = CStr(Dataset_node_thread.Tables(0).Rows(i)("ip"))
                                port = CInt(Dataset_node_thread.Tables(0).Rows(i)("port"))

                            Case "agency"

                                domain_agency = CStr(Dataset_node_thread.Tables(0).Rows(i)("domain"))
                                ip_agency = CStr(Dataset_node_thread.Tables(0).Rows(i)("ip"))
                                port_agency = CInt(Dataset_node_thread.Tables(0).Rows(i)("port"))

                            Case "web"

                                domain_web = CStr(Dataset_node_thread.Tables(0).Rows(i)("domain"))
                                ip_web = CStr(Dataset_node_thread.Tables(0).Rows(i)("ip"))
                                port_web = CInt(Dataset_node_thread.Tables(0).Rows(i)("port"))

                            Case "ethereum"

                                domain_ethereum = CStr(Dataset_node_thread.Tables(0).Rows(i)("domain"))
                                ip_ethereum = CStr(Dataset_node_thread.Tables(0).Rows(i)("ip"))
                                port_ethereum = CInt(Dataset_node_thread.Tables(0).Rows(i)("port"))

                            Case "management"

                                domain_management = CStr(Dataset_node_thread.Tables(0).Rows(i)("domain"))
                                ip_management = CStr(Dataset_node_thread.Tables(0).Rows(i)("ip"))
                                port_management = CInt(Dataset_node_thread.Tables(0).Rows(i)("port"))

                            Case "nft"

                                domain_nft = CStr(Dataset_node_thread.Tables(0).Rows(i)("domain"))
                                ip_nft = CStr(Dataset_node_thread.Tables(0).Rows(i)("ip"))
                                port_nft = CInt(Dataset_node_thread.Tables(0).Rows(i)("port"))

                        End Select

                    Next

                    JRS_nts_info = Await GRT.get_nts_info_for_login.exe(public_key, query_signiture_by_private_key)

                    json_JRS_nts_info = CType(JsonConvert.DeserializeObject(JRS_nts_info), Linq.JObject)

                    JRS = GRT.make_json_string.exe(
                                {{"key", "login_result", "quot"}, {"success", "success", "quot"}},
                                {{"public_key", public_key, "quot"},
                                {"balance", CStr(balance), "non_quot"},
                                {"node", node, "quot"},
                                {"exchange_name", exchange_name, "quot"},
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
                                {"port_nft", CStr(port_nft), "non_quot"},
                                {"email", json_JRS_nts_info("value")("email").ToString, "quot"},
                                {"name_english", json_JRS_nts_info("value")("name_english").ToString, "quot"},
                                {"name_home_language", json_JRS_nts_info("value")("name_home_language").ToString, "quot"},
                                {"country", json_JRS_nts_info("value")("country").ToString, "quot"},
                                {"phone_number", json_JRS_nts_info("value")("phone_number").ToString, "quot"},
                                {"identity_number", json_JRS_nts_info("value")("identity_number").ToString, "quot"}}, False)

                    Return JRS

                Else
                    ' 노드 등록을 위한 로그인이다. 이미 다른 곳에 노드 등록을 한 경우는 위에서 처리된다. 이 경우 account 에는 노드가 표시되어 있으나 노드 그 자체는 아직 등록되어 있지 않은 상태이다.

                    JRS = GRT.make_json_string.exe(
                                {{"key", "login_result", "quot"}, {"success", "success", "quot"}},
                                {{"public_key", public_key, "quot"},
                                {"balance", CStr(balance), "non_quot"},
                                {"node", node, "quot"},
                                {"exchange_name", "", "quot"},
                                {"domain", "", "quot"},
                                {"domain_agency", "", "quot"},
                                {"domain_web", "", "quot"},
                                {"domain_ethereum", "", "quot"},
                                {"domain_management", "", "quot"},
                                {"domain_nft", "", "quot"},
                                {"ip", "", "quot"},
                                {"ip_agency", "", "quot"},
                                {"ip_web", "", "quot"},
                                {"ip_ethereum", "", "quot"},
                                {"ip_management", "", "quot"},
                                {"ip_nft", ip_nft, "quot"},
                                {"port", CStr(-1), "non_quot"},
                                {"port_agency", CStr(-1), "non_quot"},
                                {"port_web", CStr(-1), "non_quot"},
                                {"port_ethereum", CStr(-1), "non_quot"},
                                {"port_management", CStr(-1), "non_quot"},
                                {"port_nft", CStr(-1), "non_quot"},
                                {"email", "", "quot"},
                                {"name_english", "", "quot"},
                                {"name_home_language", "", "quot"},
                                {"country", "", "quot"},
                                {"phone_number", "", "quot"},
                                {"identity_number", "", "quot"}}, False)

                    Return JRS

                    Return "{""key"" : ""login_result"", ""success"" : ""success"", " &
                        """value"": {""publick_key"": """ & public_key & """, ""balance"": " & balance & ", ""node"": """ & node & """, ""exchange_name"": """", ""domain"": """ & "" & """, ""ip"": """ & "" & """, ""port"": " & -1 & ", ""port_nft"": " & -1 & "}}"

                End If

                Connection_mariadb_local_bc_manager.Close()

            Else

                Return "{""key"" : ""login_result"", ""success"" : ""fail"", ""value"": {""publick_key"": """ & public_key & """, ""reason"": ""no_account""}}"

            End If

        Else

            Return "{""key"" : ""login_result"", ""success"" : ""fail"", ""value"": {""publick_key"": """ & public_key & """,""reason"": ""verification_fail""}}"

        End If

    End Function

End Class
