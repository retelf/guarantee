Imports System.IO
Imports Newtonsoft.Json
Imports System.Diagnostics.Debug
Imports System.Windows.Forms
Imports Microsoft.Web.WebView2.Core
Imports System.ServiceProcess

Public Class get_balance_via_socket

    Public Shared Async Function exe(coin_name As String, eoa As String, signiture As String) As Task(Of Newtonsoft.Json.Linq.JObject)

        ' 메인으로부터 찾느냐 아니면 부모로부터 찾느냐 그것이 문제로다.
        ' 먼저 자신으로부터 찾고 없으면 메인으로 간다. 이것이 가장 기본이다.

        Dim JSS, JRS As String

        Dim server_listening, db_running As Boolean

        JSS = GRT.make_json_string.exe(
            {{"key", "get_balance", "quot"}},
            {
            {"eoa", eoa, "quot"},
            {"coin_name", coin_name, "quot"},
            {"signiture", signiture, "quot"}}, False)

        server_listening = GRT.check_process_running.exe("guarantee7_server")
        db_running = GRT.check_local_service_running.exe("MySql")

        If server_listening And db_running Then
            JRS = Await GRT.socket_client.exe(GRT.GR.account.agency.domain_agency, GRT.GR.account.agency.port_agency, GRT.GR.temporary_self_client_socket_sender_number, JSS)
        Else
            JRS = Await Task.Run(Function() GRT.socket_client.exe(GRT.GR.main_server_address_agency, GRT.GR.port_number_server_main_agency, GRT.GR.port_number_server_local, JSS))
        End If

        Dim json_receipt As Newtonsoft.Json.Linq.JObject = CType(JsonConvert.DeserializeObject(JRS), Linq.JObject)

        Return json_receipt

    End Function

End Class
