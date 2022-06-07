Imports Newtonsoft.Json
Imports System.Diagnostics.Debug
Imports System.Windows.Forms
Imports Microsoft.Web.WebView2.Core

Public Class syncronize_main_p2p_for_middle_gap

    Public Shared Async Sub exe(replication_start_number As Long, replication_end_number As Long)

        Dim na, signiture, parent_address_agency, parent_domain_agency, parent_ip_agency As String
        Dim JSS, JRS As String
        Dim parent_port_agency As Integer
        Dim json_receipt, json_parent_info_address As Newtonsoft.Json.Linq.JObject

        na = get_local_server_id.exe

        ' 먼저 부모부터 찾아야 한다.

        json_parent_info_address = Await Task.Run(Function() GRT.get_parent_server_info.exe())

        parent_domain_agency = json_parent_info_address("value")("domain_agency").ToString
        parent_ip_agency = json_parent_info_address("value")("ip_agency").ToString
        parent_port_agency = CInt(json_parent_info_address("value")("port_agency"))

        If Not parent_domain_agency = "" Then
            parent_address_agency = parent_domain_agency
        Else
            parent_address_agency = parent_ip_agency
        End If

        ' 본격 복제

        signiture = GRT.Security.Gsign.sign("foo", GRT.GR.account.private_key)

        JSS = GRT.make_json_string.exe({{"key", "request_syncronize", "quot"}}, {{"na", na, "quot"}, {"eoa", GRT.GR.account.public_key, "quot"}, {"replication_start_number", CStr(replication_start_number), "non_quot"}, {"replication_end_number", CStr(replication_end_number), "non_quot"}, {"signiture", signiture, "quot"}}, True)

        JRS = Await Task.Run(Function() GRT.socket_client.exe(parent_address_agency, parent_port_agency, GRT.GR.port_number_server_local, JSS))

        json_receipt = CType(JsonConvert.DeserializeObject(JRS), Linq.JObject)

        Call syncronize_main_p2p_sub.exe(json_receipt)

    End Sub

End Class
