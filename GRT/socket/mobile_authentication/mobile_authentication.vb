Imports System.Diagnostics.Debug
Imports Newtonsoft.Json

Public Class mobile_authentication

    Public Shared Async Function submit_membership_info(json As Newtonsoft.Json.Linq.JObject, public_key As String, idate_string As String, signiture As String) As Task(Of Newtonsoft.Json.Linq.JObject)

        Dim json_message =
            "{""key"": ""submit_mobile_authentication_info""," &
            """value"":" &
            "{""email"":""" & json("value")("email").ToString &
            """,""name_english"":""" & json("value")("name_english").ToString &
            """,""name_home_language"":""" & json("value")("name_home_language").ToString &
            """,""country"":""" & json("value")("country").ToString &
            """,""phone_number"":""" & json("value")("phone_number").ToString &
            """,""identity_number"":""" & json("value")("identity_number").ToString &
            """,""public_key"":""" & public_key &
            """,""signiture"":""" & signiture &
            """,""idate_string"":""" & idate_string &
            """}" &
            "}"

        Dim json_return_str = Await Task.Run(Function() socket_client.exe(GRT.GR.authentification_server_address, GRT.GR.port_number_server_authentification, GRT.GR.port_number_server_local, json_message))

        Return CType(JsonConvert.DeserializeObject(json_return_str), Linq.JObject)

    End Function

    Public Shared Async Function submit_mobile_authentication_number(json As Newtonsoft.Json.Linq.JObject, idate_string As String, signiture As String) As Task(Of Newtonsoft.Json.Linq.JObject)

        ' 만약 외국 거주자인 경우 외국 은행계좌에 1원을 입금하고 인증확인한다.

        Dim json_message =
            "{""key"": ""submit_mobile_authentication_number""," &
            """value"":" &
            "{""public_key"":""" & json("value")("public_key").ToString &
            """,""node"":""" & json("value")("node").ToString &
            """,""mobile_authentication_number"":""" & json("value")("mobile_authentication_number").ToString &
            """,""signiture"":""" & signiture &
            """,""block_hash"":""" & "initial" &
            """,""hash"":""" & json("value")("hash").ToString &
            """,""idate_string"":""" & idate_string &
            """}" &
            "}"

        Dim json_return_str = Await Task.Run(Function() socket_client.exe(GRT.GR.authentification_server_address, GRT.GR.port_number_server_authentification, GRT.GR.port_number_server_local, json_message))

        ' 이 사이 메인서버에서 공개, 비밀키가 발급되어 리턴된다.

        Dim json_keys As Newtonsoft.Json.Linq.JObject = CType(JsonConvert.DeserializeObject(json_return_str), Linq.JObject)

        Return json_keys

    End Function

End Class
