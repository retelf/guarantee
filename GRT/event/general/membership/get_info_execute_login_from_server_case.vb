Imports System.ServiceProcess
Imports Newtonsoft.Json

Public Class get_info_execute_login_from_server_case

    Public Shared Async Function exe(public_key As String, foo_signature As String, info As get_server_login_info.st_info) As Task(Of String)

        Dim JRS, JRS_nts_info As String
        Dim json_JRS_nts_info As Newtonsoft.Json.Linq.JObject

        If info.server.registered_node Then

            If info.server.server_owner_login Then

                If info.server.registered_account Then

                    JRS_nts_info = Await get_nts_info_for_login.exe(public_key, foo_signature)

                    json_JRS_nts_info = CType(JsonConvert.DeserializeObject(JRS_nts_info), Linq.JObject)

                    JRS = make_json_string.exe(
                                {{"key", "login_result", "quot"}, {"success", "success", "quot"}},
                                {{"public_key", public_key, "quot"},
                                {"balance", CStr(info.server.balance), "non_quot"},
                                {"node", GR.server_id, "quot"},
                                {"exchange_name", info.server.exchange_name, "quot"},
                                {"domain", info.server.domain, "quot"},
                                {"domain_agency", info.server.domain_agency, "quot"},
                                {"domain_web", info.server.domain_web, "quot"},
                                {"domain_ethereum", info.server.domain_ethereum, "quot"},
                                {"domain_management", info.server.domain_management, "quot"},
                                {"domain_nft", info.server.domain_nft, "quot"},
                                {"ip", info.server.ip, "quot"},
                                {"ip_agency", info.server.ip_agency, "quot"},
                                {"ip_web", info.server.ip_web, "quot"},
                                {"ip_ethereum", info.server.ip_ethereum, "quot"},
                                {"ip_management", info.server.ip_management, "quot"},
                                {"ip_nft", info.server.ip_nft, "quot"},
                                {"port", CStr(info.server.port), "non_quot"},
                                {"port_agency", CStr(info.server.port_agency), "non_quot"},
                                {"port_web", CStr(info.server.port_web), "non_quot"},
                                {"port_ethereum", CStr(info.server.port_ethereum), "non_quot"},
                                {"port_management", CStr(info.server.port_management), "non_quot"},
                                {"port_nft", CStr(info.server.port_nft), "non_quot"},
                                {"email", json_JRS_nts_info("value")("email").ToString, "quot"},
                                {"name_english", json_JRS_nts_info("value")("name_english").ToString, "quot"},
                                {"name_home_language", json_JRS_nts_info("value")("name_home_language").ToString, "quot"},
                                {"country", json_JRS_nts_info("value")("country").ToString, "quot"},
                                {"phone_number", json_JRS_nts_info("value")("phone_number").ToString, "quot"},
                                {"identity_number", json_JRS_nts_info("value")("identity_number").ToString, "quot"}}, False)

                    'JRS = "{""key"" : ""login_result"", ""success"" : ""success"", " &
                    '    """value"": {""publick_key"": """ & public_key &
                    '    """, ""balance"": " & info.server.balance &
                    '    ", ""node"": """ & GR.server_id &
                    '    """, ""exchange_name"": """ & info.server.exchange_name &
                    '    """, ""domain"": """ & info.server.domain &
                    '    """, ""ip"": """ & info.server.ip &
                    '    """, ""port"": " & info.server.port &
                    '    ", ""port_nft"": " & info.server.port_nft & "}}"

                Else

                    Dim no_account_type As String

                    If info.server.is_main_server Then
                        no_account_type = "no_account_main_server"
                    Else
                        no_account_type = "no_account_child_server"
                    End If

                    JRS = "{""key"" : ""login_result"", ""success"" : ""fail"", ""value"": {""publick_key"": """ & public_key & """,""reason"": """ & no_account_type & """}}"

                End If

            Else
                JRS = "{""key"" : ""login_result"", ""success"" : ""fail"", ""value"": {""publick_key"": """ & public_key & """,""reason"": ""not_server_owner_login""}}"
            End If

        Else
            JRS = "{""key"" : ""login_result"", ""success"" : ""fail"", ""value"": {""publick_key"": """ & public_key & """,""reason"": ""unregistered_node or node_info_inefficient""}}"
        End If

        Return JRS

    End Function

End Class
