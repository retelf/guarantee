Public Class treat_check_registered_eoa

    Public Shared Function exe(json As Newtonsoft.Json.Linq.JObject) As String

        Dim public_key As String
        Dim query_signiture_by_private_key As String
        Dim bool As Boolean
        Dim JRS As String

        public_key = json("value")("public_key").ToString
        query_signiture_by_private_key = json("value")("signiture").ToString

        ' 먼저 검증

        Dim verified As Boolean = GRT.Security.Gverify.verify("", query_signiture_by_private_key, public_key)

        If verified Then

            bool = GRT.check_eoa_exist.exe(public_key)

            JRS = "{""key"" : ""check_registered_eoa"", ""success"" : ""success"", " &
                            """value"": {""registered"": """ & bool & """}}"

        Else

            JRS = "{""key"" : ""check_registered_eoa"", ""success"" : ""fail"", ""value"": {""reason"": ""verification_fail""}}"

        End If

        Return JRS

    End Function

End Class
