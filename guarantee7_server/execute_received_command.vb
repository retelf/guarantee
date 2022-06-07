Imports System.Text.RegularExpressions
Imports Newtonsoft.Json
Imports Nethereum.Signer.EthECKey

Public Class execute_received_command

    Public Shared Async Function exe(JSS As String) As Task(Of String)

        Dim json_received As Newtonsoft.Json.Linq.JObject = CType(JsonConvert.DeserializeObject(JSS), Linq.JObject)

        Dim JRS As String

        Dim key As String = CStr(json_received("key"))

        Select Case key

            Case "relay" : JRS = relay.exe(json_received, JSS)

            Case "new_policy" : JRS = treat_new_policy.exe(json_received, JSS)

            Case "submit_mobile_authentication_info" : JRS = Await treat_submit_mobile_authentication_info.exe(json_received)

            Case "submit_mobile_authentication_number"

                If GRT.check_node_main.exe(GRT.GR.server_id) Then
                    JRS = Await treat_submit_mobile_authentication_number_main.exe(json_received, JSS)
                Else
                    JRS = Await treat_submit_mobile_authentication_number_general.exe(json_received, JSS)
                End If

            Case "register_node" : JRS = Await treat_register_node.exe(json_received, JSS)

            Case "execute_login" : JRS = Await treat_execute_login.exe(json_received)

            Case "get_nts_info" : JRS = Await treat_get_nts_info.exe(json_received, JSS)

            Case "check_registered_eoa" : JRS = treat_check_registered_eoa.exe(json_received)

            Case "request_parent_info" : JRS = treat_request_parent_info.exe(json_received)

            Case "request_max_block_number" : JRS = treat_request_max_block_number.exe(json_received)

            Case "request_syncronize" : JRS = treat_request_syncronize.exe(json_received)

            Case "get_schma" : JRS = treat_get_schma.exe(json_received)

            Case "check_main_key_login" : JRS = treat_check_main_key_login.exe(json_received)

            Case "execute_add_account_new" : JRS = Await treat_execute_add_account_new.exe(json_received, JSS)

            Case "execute_add_account_already" : JRS = Await treat_execute_add_account_already.exe(json_received, JSS)

            'Case "execute_add_account_guarantee" : JRS = Await treat_execute_add_account_guarantee.exe(json_received, JSS)

            'Case "execute_add_account_ethereum" : JRS = Await treat_execute_add_account_ethereum.exe(json_received, JSS)

            Case "deploy_smart_contract" : JRS = Await treat_deploy_smart_contract.exe(json_received, JSS)

            Case "load_smart_contract" : JRS = Await treat_load_smart_contract.exe(json_received, JSS)

            Case "get_balance" : JRS = Await get_balance.exe(json_received)

            Case "nonce_request" : JRS = Await GRT.nonce_management.exe_trom_wallet(json_received)

            Case "submit_transfer" : JRS = Await treat_transfer.exe(json_received, JSS)

            Case "submit_load_order" : JRS = Await treat_exchang_load_order.exe(json_received, JSS)

            Case "submit_exchange" : JRS = Await treat_exchange.exe(json_received, JSS)

            Case "submit_cancel" : JRS = Await treat_exchange_cancel.exe(json_received, JSS)

            Case "display_board_exchange" : JRS = treat_display_board_exchange.exe(json_received)

            Case "submit_sell_order" : JRS = Await treat_multilevel_sell_order.exe(json_received, JSS)

            Case "submit_refund" : JRS = Await treat_multilevel_submit_refund.exe(json_received, JSS)

            Case "submit_buy" : JRS = Await treat_multilevel_submit_buy.exe(json_received, JSS)

            Case "submit_recall" : JRS = Await treat_multilevel_submit_recall.exe(json_received, JSS)

            Case "submit_sell" : JRS = Await treat_multilevel_submit_sell.exe(json_received, JSS)

            Case "set_lock", "set_unlock" : JRS = treat_set_lock.exe(json_received) ' 이곳이 메인 또는 exchange_server_address 임. 하지만 relay 는 되지 않는다. 너무 부하가 크게 된다.

            Case "display_board_sell_order" : JRS = treat_display_board_multilevel_sell_order.exe(json_received)

            Case "check_nft_token_id"

                Dim token_id = GRT.check_nft_token_id.exe(CStr(json_received("value")("nfa")))

                JRS = GRT.make_json_string.exe({{"key", key, "quot"}, {"success", "success", "quot"}}, {{"token_id", CStr(token_id), "non_quot"}}, False)

            Case "check_nfa_and_creator" : JRS = GRT.check_nfa_and_creator.exe(CStr(json_received("value")("nfa")))

            Case "submit_load_nft" : JRS = Await treat_submit_load_nft.exe(json_received, JSS)

            Case "submit_nft_load_order" : JRS = Await treat_submit_nft_load_order.exe(json_received, JSS)

            Case "submit_nft_buy" : JRS = Await treat_submit_nft_buy.exe(json_received, JSS)

            Case "submit_nft_recall" : JRS = Await treat_submit_nft_recall.exe(json_received, JSS)

            Case "submit_nft_confirm" : JRS = Await treat_submit_nft_confirm.exe(json_received, JSS)

            Case "submit_clear_deposit" : JRS = Await treat_submit_clear_deposit.exe(json_received, JSS)

            Case "from_php" : JRS = Await treat_from_php.exe(json_received, JSS)

            Case "test" : JRS = Await treat_test.exe(json_received, JSS)

                ' new thread ethereum

            Case "submit_NTD_transfer" : JRS = Await NTD_transfer_ethereum.exe(json_received, JSS)

                ' new thread nft

                ' new thread management

            Case Else

                JRS = "{""key"" : ""error_command"", ""success"" : ""fail"", ""receipt"" : ""error_command"" }"

        End Select

        Return JRS

    End Function

End Class
