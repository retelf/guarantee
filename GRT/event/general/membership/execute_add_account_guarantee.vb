Imports System.ServiceProcess

Public Class execute_add_account_guarantee

    Public Shared Async Function exe(ecKey As Nethereum.Signer.EthECKey, password As String, node As String, whether_key_file_generate As Boolean, idate_string As String) As Task(Of String)

        'Dim JSS, JRS As String
        'Dim pure_query, public_key, private_key, signiture_representative, signiture_by_new_public_key As String

        'public_key = ecKey.GetPublicAddress
        'private_key = ecKey.GetPrivateKey

        '' 키파일을 만든다.

        'If whether_key_file_generate = True Then

        '    Call GRT.generate_key_file_old.exe("guarantee", password, ecKey)

        'End If

        '' pure_query 를 만든다.

        'pure_query = GRT.GQS_insert_account_pure_query.exe(public_key, GRT.GR.account.public_key, 0, node, idate_string)

        '' 사인 
        '' 메인 테이블 입력시에 eoa 를 GRT.GR.account.public_key 로 하게 된다.
        '' 따라서 그냥 private_key 로 하면 안된다. 나중에 relay 할 때 일관성 문제가 생긴다.

        'signiture_by_new_public_key = GRT.Security.Gsign.sign(pure_query, private_key)
        'signiture_representative = GRT.Security.Gsign.sign(pure_query, GRT.GR.account.private_key)

        '' 공개키를 등록한다.

        'JSS = GRT.make_json_string.exe(
        '            {{"key", "execute_add_account_guarantee", "quot"}},
        '            {
        '            {"block_hash", "initial", "quot"},
        '            {"public_key", public_key, "quot"},
        '            {"public_key_representative", GRT.GR.account.public_key, "quot"},
        '            {"coin_name", "guarantee", "quot"},
        '            {"node", node, "quot"},
        '            {"balance", CStr(0), "non_quot"},
        '            {"idate_string", idate_string, "quot"},
        '            {"signiture_by_new_public_key", signiture_by_new_public_key, "quot"},
        '            {"signiture_representative", signiture_representative, "quot"}
        '            }, True)

        'JRS = Await Task.Run(Function() GRT.socket_client.exe(GRT.GR.main_server_address_agency, GRT.GR.port_number_server_main_agency, GRT.GR.port_number_server_local, JSS))

        'Return JRS

    End Function

End Class
