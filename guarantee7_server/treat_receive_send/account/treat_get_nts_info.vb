Public Class treat_get_nts_info

    Public Shared Async Function exe(json As Newtonsoft.Json.Linq.JObject, JSS As String) As Task(Of String) ' 스스로 메인서버인 경우에만 사용된다. 

        Dim command_key, public_key, signiture As String
        Dim verified As Boolean
        Dim JRS As String
        Dim Dataset As DataSet

        command_key = json("key").ToString
        public_key = json("value")("public_key").ToString
        signiture = json("value")("signiture").ToString

        verified = GRT.Security.Gverify.verify("foo", signiture, public_key)

        If verified Then

            Dim representative_eoa = GRT.execute_select_representative.exe(public_key)

            Dataset = GRT.execute_select_nts_info.exe(representative_eoa)

            If Dataset.Tables(0).Rows.Count > 0 Then

                JRS = GRT.make_json_string.exe(
                            {{"key", "get_nts_info", "quot"}, {"success", "success", "quot"}},
                            {
                            {"email", CStr(Dataset.Tables(0).Rows(0)("email")), "quot"},
                            {"name_english", CStr(Dataset.Tables(0).Rows(0)("name_english")), "quot"},
                            {"name_home_language", CStr(Dataset.Tables(0).Rows(0)("name_home_language")), "quot"},
                            {"country", CStr(Dataset.Tables(0).Rows(0)("country")), "quot"},
                            {"phone_number", CStr(Dataset.Tables(0).Rows(0)("phone_number")), "quot"},
                            {"identity_number", CStr(Dataset.Tables(0).Rows(0)("identity_number")), "quot"}}, False)

            Else

                JRS = "{""key"" : """ & command_key & """, ""success"" : ""fail"", ""value"" : {""reason"": """ & "no_account" & """} }"

            End If

        Else

            Dim error_message = "보내주신 자료가 변조되었습니다."

            Return "{""key"" : """ & command_key & """, ""success"" : ""fail"", ""value"" : {""reason"": """ & error_message & """} }"

        End If

        Return JRS

    End Function

End Class
