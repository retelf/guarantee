Imports System.ServiceProcess

Public Class execute_add_account_already

    Public Shared Async Function exe(public_key As String, password As String, private_key As String, node As String, whether_key_file_generate As Boolean, idate_string As String) As Task(Of String)

        Dim JSS, JRS, pure_query As String

        ' 이더리움 밸런스를 확인하고

        Dim web3 = New Web3(GR.account.ethereum_web3_url)
        Dim balance_wei = Await web3.Eth.GetBalance.SendRequestAsync(public_key)
        Dim balance_decimal As Decimal = CDec(balance_wei.Value)

        If Not balance_decimal = 0 Then

            ' 키파일을 만든다.

            If whether_key_file_generate = True Then

                Call GRT.generate_key_file.exe_already(password, private_key)

            End If

            pure_query = GRT.GQS_insert_account_pure_query.exe(public_key, GR.account.public_key, 0, {balance_decimal}, node, idate_string)

            ' 공개키를 등록한다.

            JSS = GRT.make_json_string.exe(
                    {{"key", "execute_add_account_already", "quot"}},
                    {
                    {"block_hash", "initial", "quot"},
                    {"eoa_representative", GR.account.public_key, "quot"},
                    {"public_key_etc_coin", public_key, "quot"},
                    {"coin_name", "ethereum", "quot"},
                    {"node", node, "quot"},
                    {"balance", CStr(0), "non_quot"},
                    {"balance_etc", CStr(balance_decimal), "non_quot"},
                    {"idate_string", idate_string, "quot"},
                    {"signiture", GRT.Security.Gsign.sign(pure_query, GR.account.private_key), "quot"},
                    {"signiture_etc_coin", GRT.Security.Gsign.sign(pure_query, private_key), "quot"}
                    }, True)

            JRS = Await Task.Run(Function() GRT.socket_client.exe(GRT.GR.main_server_address_agency, GRT.GR.port_number_server_main_agency, GRT.GR.port_number_server_local, JSS))

        Else

            JRS = "{""key"" : ""execute_add_account_ethereum"", ""success"" : ""fail"", ""reason"" : ""ineffective_or_zero_balance_ethereum_account"" }"

        End If

        Return JRS

    End Function

End Class
