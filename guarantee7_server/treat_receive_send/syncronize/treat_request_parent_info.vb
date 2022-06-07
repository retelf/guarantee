Public Class treat_request_parent_info

    Public Shared Function exe(json As Newtonsoft.Json.Linq.JObject) As String

        Dim public_key, requesting_na As String
        Dim domain, ip As String
        Dim port As Integer
        Dim query_signiture_by_private_key, JRS As String

        public_key = json("value")("eoa").ToString
        requesting_na = json("value")("na").ToString
        query_signiture_by_private_key = json("value")("signiture").ToString

        Dim Connection_mariadb_local_bc_manager As New MySqlConnection(GRT.GR.cString_mariadb_local_bc_manager)

        Connection_mariadb_local_bc_manager.Open()

        ' 먼저 검증

        Dim verified As Boolean = GRT.Security.Gverify.verify("", query_signiture_by_private_key, public_key)

        If verified Then

            JRS = GRT.get_parent_info.exe(requesting_na)

        Else
            JRS = "{""key"" : ""parent_info_result"", ""success"" : ""fail"", ""value"": {""reason"": ""verification_fail""}}"
        End If

        Connection_mariadb_local_bc_manager.Close()

        Return JRS

    End Function

End Class
