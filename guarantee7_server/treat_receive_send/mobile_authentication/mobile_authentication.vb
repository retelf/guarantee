Public Class mobile_authentication

    Public Shared Function generate() As Integer

        Dim random As New Random

        Dim authentication_number As Integer = random.Next(100000, 999999)

        Return authentication_number

    End Function

    Public Shared Async Function send(phone_number As String, authentication_number As Integer) As Task(Of Boolean)

        Dim JSS = GRT.make_json_string.exe({{"key", "send_mobile_authentication_number", "quot"}}, {{"phone_number", phone_number, "quot"}, {"authentication_number", CStr(authentication_number), "non_quot"}}, False)

        Dim JRS = Await GRT.socket_client.exe(GRT.GR.send_message_server_address, GRT.GR.port_number_server_send_message, GRT.GR.port_number_server_local, JSS)

        Dim json_JRS As Newtonsoft.Json.Linq.JObject = CType(JsonConvert.DeserializeObject(JRS), Linq.JObject)

        If json_JRS("success").ToString = "success" Then
            Return True
        Else
            Return False
        End If

    End Function

End Class
