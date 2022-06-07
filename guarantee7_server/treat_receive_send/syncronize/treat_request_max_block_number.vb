Public Class treat_request_max_block_number

    Public Shared Function exe(json As Newtonsoft.Json.Linq.JObject) As String

        Dim public_key, requesting_na As String
        Dim max_block_number As Long
        Dim query_signiture_by_private_key, JRS As String

        public_key = json("value")("eoa").ToString
        requesting_na = json("value")("na").ToString
        query_signiture_by_private_key = json("value")("signiture").ToString

        Dim Connection_mariadb_local_bc_manager As New MySqlConnection(GRT.GR.cString_mariadb_local_bc_manager)

        Connection_mariadb_local_bc_manager.Open()

        ' 먼저 검증

        Dim verified As Boolean = GRT.Security.Gverify.verify("", query_signiture_by_private_key, public_key)

        If verified Then

            max_block_number = GRT.get_local_main_max_block_number.exe

            JRS = GRT.make_json_string.exe(
                        {{"key", json("key").ToString, "quot"}, {"success", "success", "quot"}},
                        {{"max_block_number", CStr(max_block_number), "non_quot"}}, True)

        Else

            Dim error_message As String = "verification_fail"

            JRS = GRT.make_json_string.exe(
                        {{"key", json("key").ToString, "quot"}, {"success", "fail", "quot"}},
                        {{"reason", error_message, "quot"}}, True)
        End If

        Connection_mariadb_local_bc_manager.Close()

        Return JRS

    End Function

End Class
