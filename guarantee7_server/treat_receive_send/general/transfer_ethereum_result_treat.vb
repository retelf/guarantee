Public Class transfer_ethereum_result_treat

    Public Shared Function initial_success(signiture_key As String, command_key As String, json As Newtonsoft.Json.Linq.JObject, transfer_data As GRT.transfer_ethereum.st_data) As String

        Dim JSS, JRS As String
        Dim json_JRS As Newtonsoft.Json.Linq.JObject

        json("value")("ethereum_transaction_result") = "initial_success"
        json("value")("tem_transaction_success_initial") = CInt(transfer_data.tem_transaction_success_initial)
        json("value")("transaction_hash_initial") = transfer_data.transaction_hash_initial
        json("value")("receipt_initial") = transfer_data.json_receipt_initial
        json("value")("usedGas") = transfer_data.json_receipt_initial("gasUsed")

        JSS = CType(JsonConvert.SerializeObject(json), String)

        JRS = send_main.exe(signiture_key, JSS) ' 메인으로 보낸다. 하등의 데이터베이스 처리 없이 보낸다. 그러나 에이전트 레코드 기록은 남긴다.

        Call GRT.agent_record.state_update("send_main_JRS_received", "", signiture_key)

        json_JRS = CType(JsonConvert.DeserializeObject(JRS), Linq.JObject)

        If json_JRS("success").ToString = "success" Then ' 락을 풀지 않더라도 기본적으로 UPDATE sell_order SET `state` = 'seized' 가 되어 있으므로 특별한 문제는 없다..

            Call GRT.agent_record.state_update("send_main_initial_succeeded", "", signiture_key)

        Else

            Call GRT.agent_record.state_update("send_main_initial_fail", json_JRS("value")("reason").ToString, signiture_key)

            ' 이는 송신장애는 아니라는 뜻이다.
            ' 메인에 도달하지 않거나 메인에서 실행되지 않은 쿼리를 다시 실행되도록 만들어 주어야 한다.

        End If

        Return JRS

    End Function

    Public Shared Async Function cancel_success(block_number As Long, signiture_key As String, command_key As String, json As Newtonsoft.Json.Linq.JObject, transfer_data As GRT.transfer_ethereum.st_data) As Task(Of String)

        Dim JSS, JRS As String
        Dim json_JRS As Newtonsoft.Json.Linq.JObject
        Dim database As String, table As String

        If Not (command_key = "submit_transfer" Or command_key = "submit_load_order" Or command_key = "submit_sell_order") Then '  submit_transfer 는 애당초 락을 걸지 않는다. 사실 이것은 없어도 된다.

            If command_key = "submit_cancel" Then
                database = "bc" : table = "exchange"
            ElseIf command_key = "submit_buy" Or command_key = "submit_refund" Or command_key = "submit_recall" Then
                database = "bc_multilevel" : table = "sell_order"
            End If

            Await Task.Run(Sub() request_lock.unlock(block_number, signiture_key, database, table)) ' 스스로 락을 걸었고 이더리움이 나가지 않았으므로 어떤 이유로든 언락을 해 주어야 한다.

        End If

        json("value")("ethereum_transaction_result") = "cancel_success"
        json("value")("tem_transaction_success_cancel") = CInt(transfer_data.tem_transaction_success_cancel)
        json("value")("transaction_hash_cancel") = transfer_data.transaction_hash_cancel
        json("value")("receipt_cancel") = transfer_data.json_receipt_cancel
        json("value")("usedGas") = transfer_data.json_receipt_cancel("gasUsed")

        JSS = CType(JsonConvert.SerializeObject(json), String)

        JRS = send_main.exe(signiture_key, JSS) ' 메인으로 보낸다. 하등의 데이터베이스 처리 없이 보낸다. 그러나 에이전트 레코드 기록은 남긴다. submit_recall 의 경우도 가스비 변경은 있으므로 보낸다. 거래소 이더 잔고의 변화.

        Call GRT.agent_record.state_update("send_main_JRS_received", "", signiture_key)

        json_JRS = CType(JsonConvert.DeserializeObject(JRS), Linq.JObject)

        If json_JRS("success").ToString = "success" Then

            Call GRT.agent_record.state_update("send_main_cancel_succeeded", "", signiture_key)

        Else

            Call GRT.agent_record.state_update("send_main_cancel_fail", json_JRS("value")("reason").ToString, signiture_key)

            ' 이는 송신장애는 아니라는 뜻이다.
            ' 메인에 도달하지 않거나 메인에서 실행되지 않은 쿼리를 다시 실행되도록 만들어 주어야 한다.

        End If

        Return JRS

    End Function

    Public Shared Function transfer_ethereum_from_exchange_case_proceeding(ethereum_transaction_result As String, signiture_key As String, command_key As String, json As Newtonsoft.Json.Linq.JObject, transfer_data As GRT.transfer_ethereum.st_data) As String

        Dim JSS, JRS As String
        Dim json_JRS As Newtonsoft.Json.Linq.JObject

        'json("value")("ethereum_transaction_result") = "transfer_ethereum_from_exchange_case_proceeding"
        json("value")("ethereum_transaction_result") = ethereum_transaction_result
        json("value")("tem_transaction_success_initial") = CInt(transfer_data.tem_transaction_success_initial)
        json("value")("tem_transaction_success_cancel") = CInt(transfer_data.tem_transaction_success_cancel)
        json("value")("transaction_hash_initial") = transfer_data.transaction_hash_initial
        json("value")("transaction_hash_cancel") = transfer_data.transaction_hash_cancel

        JSS = CType(JsonConvert.SerializeObject(json), String)

        JRS = send_main.exe(signiture_key, JSS) ' 메인으로 보낸다. 하등의 데이터베이스 처리 없이 보낸다. 그러나 에이전트 레코드 기록은 남긴다.

        Call GRT.agent_record.state_update("send_main_JRS_received", "", signiture_key)

        json_JRS = CType(JsonConvert.DeserializeObject(JRS), Linq.JObject)

        If json_JRS("success").ToString = "success" Then ' 락을 풀지 않더라도 기본적으로 UPDATE sell_order SET `state` = 'seized' 가 되어 있으므로 특별한 문제는 없다..

            Call GRT.agent_record.state_update("send_main_transfer_ethereum_from_exchange_case_proceeding_succeeded", "", signiture_key)

        Else

            Call GRT.agent_record.state_update("send_main_transfer_ethereum_from_exchange_case_proceeding_fail", json_JRS("value")("reason").ToString, signiture_key)

            ' 이는 송신장애는 아니라는 뜻이다.
            ' 메인에 도달하지 않거나 메인에서 실행되지 않은 쿼리를 다시 실행되도록 만들어 주어야 한다.

        End If

        Return JRS

    End Function

    Public Shared Function no_need_cancel_transfer(ethereum_transaction_result As String, signiture_key As String, command_key As String, json As Newtonsoft.Json.Linq.JObject, transfer_data As GRT.transfer_ethereum.st_data) As String

        Dim JSS, JRS As String
        Dim json_JRS As Newtonsoft.Json.Linq.JObject

        json("value")("ethereum_transaction_result") = ethereum_transaction_result
        json("value")("tem_transaction_success_initial") = CInt(transfer_data.tem_transaction_success_initial)
        json("value")("tem_transaction_success_cancel") = CInt(transfer_data.tem_transaction_success_cancel) ' no_need 하기야 initial 이 성공한 경우도 이것이 false 이기는 마찬가지임. 그래서 그대로 둔다. 일관성을 위해서.
        json("value")("transaction_hash_initial") = transfer_data.transaction_hash_initial
        json("value")("transaction_hash_cancel") = transfer_data.transaction_hash_cancel ' no_need

        JSS = CType(JsonConvert.SerializeObject(json), String)

        JRS = send_main.exe(signiture_key, JSS) ' 메인으로 보낸다. 하등의 데이터베이스 처리 없이 보낸다. 그러나 에이전트 레코드 기록은 남긴다.

        Call GRT.agent_record.state_update("send_main_JRS_received", "", signiture_key)

        json_JRS = CType(JsonConvert.DeserializeObject(JRS), Linq.JObject)

        If json_JRS("success").ToString = "success" Then ' 락을 풀지 않더라도 기본적으로 UPDATE sell_order SET `state` = 'seized' 가 되어 있으므로 특별한 문제는 없다..

            Call GRT.agent_record.state_update("send_main_no_need_cancel_transfer_succeeded", "", signiture_key)

        Else

            Call GRT.agent_record.state_update("send_main_no_need_cancel_transfer_fail", json_JRS("value")("reason").ToString, signiture_key)

            ' 이는 송신장애는 아니라는 뜻이다.
            ' 메인에 도달하지 않거나 메인에서 실행되지 않은 쿼리를 다시 실행되도록 만들어 주어야 한다.

        End If

        JRS = CType(JsonConvert.SerializeObject(json), String)

        Return JRS

    End Function

    Public Shared Sub set_etc_info(ethereum_transaction_result As String, json As Newtonsoft.Json.Linq.JObject, transfer_data As GRT.transfer_ethereum.st_data)

        json("value")("ethereum_transaction_result") = ethereum_transaction_result
        json("value")("tem_transaction_success_initial") = CInt(transfer_data.tem_transaction_success_initial)
        json("value")("tem_transaction_success_cancel") = CInt(transfer_data.tem_transaction_success_cancel)
        json("value")("transaction_hash_initial") = transfer_data.transaction_hash_initial
        json("value")("transaction_hash_cancel") = transfer_data.transaction_hash_cancel

    End Sub

End Class
