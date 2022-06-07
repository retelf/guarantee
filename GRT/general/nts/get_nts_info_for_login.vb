Public Class get_nts_info_for_login

    Public Shared Async Function exe(eoa As String, signiture As String) As Task(Of String)

        Dim representative_eoa = execute_select_representative.exe(eoa)

        Dim Dataset As DataSet

        Dim JSS, JRS As String

        If GR.node_level = 0 Then ' 스스로 메인이라면 직접 nts 서버에 접속한다.

            Dataset = execute_select_nts_info.exe(representative_eoa)

            JRS = make_json_string.exe(
                        {{"key", "get_nts_info", "quot"}, {"success", "success", "quot"}},
                        {
                        {"email", CStr(Dataset.Tables(0).Rows(0)("email")), "quot"},
                        {"name_english", CStr(Dataset.Tables(0).Rows(0)("name_english")), "quot"},
                        {"name_home_language", CStr(Dataset.Tables(0).Rows(0)("name_home_language")), "quot"},
                        {"country", CStr(Dataset.Tables(0).Rows(0)("country")), "quot"},
                        {"phone_number", CStr(Dataset.Tables(0).Rows(0)("phone_number")), "quot"},
                        {"identity_number", CStr(Dataset.Tables(0).Rows(0)("identity_number")), "quot"}}, False)

        Else ' 만약 스스로 메인이 아니라면 메인서버(추후 nts 서버로 변경한다)에 송신하여 정보를 받아온다.

            JSS = GRT.make_json_string.exe({{"key", "get_nts_info", "quot"}}, {{"public_key", eoa, "quot"}, {"signiture", signiture, "quot"}}, True)

            JRS = Await GRT.socket_client.exe(GRT.GR.main_server_address_agency, GRT.GR.port_number_server_main_agency, GRT.GR.port_number_server_local, JSS)

        End If

        Return JRS

    End Function

End Class
