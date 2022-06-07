Imports System.IO
Imports Newtonsoft.Json
Imports System.Diagnostics.Debug
Imports System.Windows.Forms
Imports Microsoft.Web.WebView2.Core

Public Class get_parent_server_info_max_block_number

    Public Shared Async Function exe(parent_server_address_agency As String, port_number_server_parent_agency As Integer) As Task(Of Newtonsoft.Json.Linq.JObject)

        Dim server_id, signiture As String
        Dim JSS, JRS As String
        Dim json_receipt As Newtonsoft.Json.Linq.JObject

        server_id = get_local_server_id.exe

        ' 부모서버에 접속해서 부모의 데이터를 받아온다.

        signiture = GRT.Security.Gsign.sign("foo", GRT.GR.account.private_key)

        JSS = GRT.make_json_string.exe({{"key", "request_max_block_number", "quot"}}, {{"na", server_id, "quot"}, {"eoa", GRT.GR.account.public_key, "quot"}, {"signiture", signiture, "quot"}}, True)

        JRS = Await Task.Run(Function() GRT.socket_client.exe(parent_server_address_agency, port_number_server_parent_agency, GRT.GR.port_number_server_local, JSS))

        json_receipt = CType(JsonConvert.DeserializeObject(JRS), Linq.JObject)

        Return json_receipt

    End Function

End Class
