Public Class check_main_key_login

    Public Shared Async Function exe(public_key As String, private_key As String) As Task(Of string)

        Dim JSS, JRS As String

        JSS = GRT.make_json_string.exe({{"key", "check_main_key_login", "quot"}}, {{"public_key", public_key, "quot"}, {"signiture", GRT.Security.Gsign.sign("foo", private_key), "quot"}}, True)

        ' 일단 스스로의 PC 에 서버(DB 서버와 소켓서버)가 설치되어 있는지 확인한다.
        ' DB 의 running 확인과 소켓 local 의 listening 확인

        Dim server_listening, db_running As Boolean

        server_listening = GRT.check_process_running.exe("guarantee7_server")
        db_running = GRT.check_local_service_running.exe("MySql")

        If server_listening And db_running Then

            ' 서비스가 러닝 상태이면 신규 클라이언트를 만든다.

            JRS = Await GRT.socket_client.exe(GRT.GR.account.agency.domain_agency, GRT.GR.account.agency.port_agency, GRT.GR.temporary_self_client_socket_sender_number, JSS)

        Else

            ' 없으면 wallet 케이스로 간주한다. 
            ' 메인서버에 접속한다. 

            JRS = Await Task.Run(Function() GRT.socket_client.exe(GRT.GR.main_server_address_agency, GRT.GR.port_number_server_main_agency, GRT.GR.port_number_server_local, JSS))

        End If

        Return JRS

    End Function

End Class
