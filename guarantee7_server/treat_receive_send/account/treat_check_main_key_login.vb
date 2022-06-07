Public Class treat_check_main_key_login

    Public Shared Function exe(json As Newtonsoft.Json.Linq.JObject) As String

        Dim eoa, representative As String
        Dim query_signiture_by_private_key, JRS As String

        eoa = json("value")("public_key").ToString
        query_signiture_by_private_key = json("value")("signiture").ToString

        ' 먼저 검증

        Dim verified As Boolean = GRT.Security.Gverify.verify("", query_signiture_by_private_key, eoa)

        If verified Then

            Dim Dataset As DataSet = GRT.check_representative.exe(eoa)

            ' 영수증 발급. 

            If Dataset.Tables(0).Rows.Count = 1 Then

                representative = CStr(Dataset.Tables(0).Rows(0)("representative"))

                If eoa = "0x" & representative Then

                    JRS = "{""key"" : ""treat_check_main_key_login"", ""success"" : ""success"", ""value"": {""representative"": ""YES""}}"

                Else

                    JRS = "{""key"" : ""treat_check_main_key_login"", ""success"" : ""success"", ""value"": {""representative"": ""NO""}}"

                End If

            Else
                JRS = "{""key"" : ""treat_check_main_key_login"", ""success"" : ""fail"", ""value"": {""eoa"": """ & eoa & """, ""reason"": ""no_account""}}"
            End If

        Else

            Dim refusal_reason = "본인이 아닌 사람이 로그인을 하려고 하였습니다."

            JRS = "{""key"" : ""treat_check_main_key_login"", ""success"" : ""fail"", ""value"": {""eoa"": """ & eoa & """,""reason"": ""verification_fail""}}"

        End If

        Return JRS

    End Function

End Class
