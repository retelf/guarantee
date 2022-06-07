Imports System.Numerics
Imports Newtonsoft.Json

Public Class request_lock
    Public Shared Async Function lock(block_number As BigInteger, signiture_key As String, database As String, table As String) As Task(Of Newtonsoft.Json.Linq.JObject)

        Dim JSS_lock, JRS_lock As String
        Dim json_JRS_lock As Newtonsoft.Json.Linq.JObject

        Call GRT.agent_record.state_update("lock_requested", "", signiture_key)

        ' 가장 먼저 락을 걸어준다.

        JSS_lock = GRT.make_json_string.exe(
                                    {{"key", "set_lock", "quot"}},
                                    {
                                    {"block_number", block_number.ToString, "non_quot"},
                                    {"database", database, "quot"},
                                    {"table", table, "quot"}}, False)

        JRS_lock = Await GRT.socket_client.exe(GRT.GR.main_server_address_agency, GRT.GR.port_number_server_main_agency, GRT.GR.port_number_server_local, JSS_lock)

        json_JRS_lock = CType(JsonConvert.DeserializeObject(JRS_lock), Linq.JObject)

        Return json_JRS_lock

    End Function
    Public Shared Async Sub unlock(block_number As BigInteger, signiture_key As String, database As String, table As String)

        Dim JSS_lock, JRS_lock As String
        Dim json_JRS_lock As Newtonsoft.Json.Linq.JObject

        Call GRT.agent_record.state_update("unlock_requested", "", signiture_key)

        JSS_lock = GRT.make_json_string.exe(
                                            {{"key", "set_unlock", "quot"}},
                                            {
                                            {"block_number", block_number.ToString, "non_quot"},
                                            {"database", database, "quot"},
                                            {"table", table, "quot"}}, False)

        JRS_lock = Await GRT.socket_client.exe(GRT.GR.main_server_address_agency, GRT.GR.port_number_server_main_agency, GRT.GR.port_number_server_local, JSS_lock)

        json_JRS_lock = CType(JsonConvert.DeserializeObject(JRS_lock), Linq.JObject)

        If json_JRS_lock("success").ToString = "success" Then
            Call GRT.agent_record.state_update("unlock_succeeded", "", signiture_key)
        Else
            Call GRT.agent_record.state_update("unlock_failed", "", signiture_key) ' 이 경우가 문제다. 주기적 싱크로에서 계속 시도를 해 주는 것으로 처리한다.
        End If

    End Sub

End Class
