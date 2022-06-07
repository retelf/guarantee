Imports System.ServiceProcess
Imports Newtonsoft.Json

Public Class execute_login

    Public Shared Async Function exe(coin_name As String, public_key As String, private_key As String, target As String, login_from As String) As Task(Of String)

        Dim JSS, JRS, JRS_nts_info As String
        Dim json_JRS_nts_info As Newtonsoft.Json.Linq.JObject

        JSS = GRT.make_json_string.exe({{"key", "execute_login", "quot"}}, {{"public_key", public_key, "quot"}, {"coin_name", coin_name, "quot"}, {"signiture", GRT.Security.Gsign.sign("foo", private_key), "quot"}}, True)

        ' 일단 스스로의 PC 에 서버(DB 서버와 소켓서버)가 설치되어 있는지 확인한다.
        ' DB 의 running 확인과 소켓 local 의 listening 확인

        Dim server_listening, db_running As Boolean

        Dim info As get_server_login_info.st_info

        server_listening = GRT.check_process_running.exe("guarantee7_server")
        db_running = GRT.check_local_service_running.exe("MySql")

        If login_from = "server" Then

            ' 이것은 완전히 성격이 다른 로그인이다.
            ' 서버에 로그인할 수 있는 자는 서버오너 뿐이다.
            ' 서버 정보와 클라이언트 에이전트 정보를 모두 가져와야 한다.

            If db_running Then

                info = get_server_login_info.exe(coin_name, public_key)

                JRS = Await get_info_execute_login_from_server_case.exe(public_key, GRT.Security.Gsign.sign("foo", private_key), info)

            Else
                JRS = "{""key"" : ""login_result"", ""success"" : ""fail"", ""value"": {""publick_key"": """ & public_key & """,""reason"": ""mysql_unrunning""}}"
            End If

        Else ' 에이전트 정보만 가져오면 될 것 같지만 agency 서버의 주소와 ip, port 모두 필요하므로 서버정보도 모두 가져온다.

            If target = "general" Then

                If server_listening And db_running Then

                    ' 서비스가 러닝 상태이면 신규 클라이언트를 만든다.

                    JRS = Await GRT.socket_client.exe(GRT.GR.address_server_local_agency, GRT.GR.port_number_server_local_agency, GRT.GR.temporary_self_client_socket_sender_number, JSS)

                Else

                    ' 없으면 wallet 케이스로 간주한다. 
                    ' 메인서버에 접속한다. 

                    JRS = Await Task.Run(Function() GRT.socket_client.exe(GRT.GR.main_server_address_agency, GRT.GR.port_number_server_main_agency, GRT.GR.port_number_server_local, JSS))

                End If

            Else ' main

                JRS = Await Task.Run(Function() GRT.socket_client.exe(GRT.GR.main_server_address_agency, GRT.GR.port_number_server_main_agency, GRT.GR.port_number_server_local, JSS))

            End If

        End If

        Return JRS

    End Function

End Class
