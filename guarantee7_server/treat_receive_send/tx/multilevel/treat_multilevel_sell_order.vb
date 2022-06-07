Imports System.Net.Sockets
Imports System.Text

Public Class treat_multilevel_sell_order

    Public Shared Async Function exe(json As Newtonsoft.Json.Linq.JObject, JSS As String) As Task(Of String)

        Dim command_key, eoa, na, exchange_name, coin_name, state, signiture, signiture_key, signiture_for_get_balance, JRS, closing_time_utc_string, idate_string As String
        Dim balance, amount, gasPrice, gasLimit, exchange_rate, exchange_fee_rate As Decimal
        Dim registered, verified As Boolean
        Dim block_number As Long
        Dim agency_domain, agency_ip As String
        Dim agency_port, days_span As Integer
        Dim pure_query, database_name, table_name, query_type, contract_type As String
        Dim received_block_hash, previous_hash As String
        Dim initial_transfer As String
        Dim idate_generated As DateTime
        Dim json_JRS As Newtonsoft.Json.Linq.JObject

        command_key = json("key").ToString

        received_block_hash = json("value")("block_hash").ToString
        eoa = json("value")("eoa").ToString
        na = json("value")("na").ToString
        exchange_name = json("value")("exchange_name").ToString
        agency_domain = json("value")("agency_domain").ToString
        agency_ip = json("value")("agency_ip").ToString
        agency_port = CInt(json("value")("agency_port"))
        coin_name = json("value")("coin_name").ToString

        registered = GRT.check_registered_eoa.exe(eoa, coin_name)

        If registered Then

            exchange_rate = CDec(json("value")("exchange_rate"))
            days_span = CInt(json("value")("days_span"))
            closing_time_utc_string = json("value")("closing_time_utc_string").ToString
            exchange_fee_rate = CDec(json("value")("exchange_fee_rate"))
            state = json("value")("state").ToString
            signiture_for_get_balance = json("value")("signiture_for_get_balance").ToString
            signiture = json("value")("signiture").ToString
            signiture_key = Regex.Match(signiture, "^0x.{64}").ToString
            initial_transfer = json("value")("initial_transfer").ToString
            idate_string = json("value")("idate_string").ToString

            balance = GRT.get_guarantee_balance_from_local_server.exe("foo", signiture_for_get_balance, eoa)

            idate_generated = DateTime.Now

            If balance > 0 And amount + gasPrice * gasLimit / 1000000000 <= balance Then

                pure_query = GRT.GQS_multilevel_sell_order.exe(eoa, na, exchange_name, agency_domain, agency_ip, agency_port, exchange_rate, days_span, closing_time_utc_string, exchange_fee_rate, idate_string)

                verified = GRT.Security.Gverify.verify(pure_query, signiture, eoa)

                If verified Then

                    If initial_transfer = "Y" Then
                        ' 사실 이것은 좀 불분명하다. 수정해 주어야 할 듯. 실제로 이곳에서 트랜스퍼를 하지 않기 때문.
                        ' 또한 Y 를 변경시켜도 안된다.

                        If Not GRT.GR.node_level = 0 Then ' 에이전트 서버가 최초로 접수한 경우

                            JRS = send_main.exe(signiture_key, JSS) ' 메인으로 보낸다. 하등의 데이터베이스 처리 없이 보낸다. 그러나 에이전트 레코드 기록은 남긴다.

                            json_JRS = CType(JsonConvert.DeserializeObject(JRS), Linq.JObject)

                            If json_JRS("success").ToString = "success" Then ' 에이전트 레코드 기록

                                Call GRT.agent_record.generate(Regex.Match(signiture, "^0x.{64}").ToString, idate_generated, json, JSS)

                            End If

                        Else ' 메인서버가 에이전트로부터 보고 받음. 이 경우는 개런티 실행은 메인서버에서 해 준다.

                            Call GRT.set_block_number_and_get_previous_block_hash.exe()

                            block_number = GRT.set_block_number_and_get_previous_block_hash.data.block_number
                            previous_hash = GRT.set_block_number_and_get_previous_block_hash.data.previous_hash
                            database_name = "bc_multilevel, bc, bc"
                            table_name = "sell_order, account, account"
                            query_type = "INSERT, UPDATE, UPDATE"
                            contract_type = command_key

                            json("value")("initial_transfer") = "N"
                            JSS = CType(JsonConvert.SerializeObject(json), String)

                            JRS = Await self_input_and_relay.exe(command_key, block_number, received_block_hash, previous_hash, eoa, database_name, table_name, query_type, contract_type, pure_query, signiture, JSS) ' 부모서버로부터 릴레이 받은 경우임.  

                        End If

                    Else ' 부모서버로부터 릴레이 받은 경우임. 에이전트가 다시 릴레이 받은 경우도 이리로 옴.

                        Call GRT.set_block_number_and_get_previous_block_hash.exe()

                        block_number = GRT.set_block_number_and_get_previous_block_hash.data.block_number
                        previous_hash = GRT.set_block_number_and_get_previous_block_hash.data.previous_hash
                        database_name = "bc_multilevel, bc, bc"
                        table_name = "sell_order, account, account"
                        query_type = "INSERT, UPDATE, UPDATE"
                        contract_type = command_key

                        JRS = Await self_input_and_relay.exe(command_key, block_number, received_block_hash, previous_hash, eoa, database_name, table_name, query_type, contract_type, pure_query, signiture, JSS) ' 부모서버로부터 릴레이 받은 경우임.  

                        Call GRT.agent_record.confirm(block_number, Regex.Match(signiture, "^0x.{64}").ToString)

                    End If

                Else
                    JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", "데이터 변조", "quot"}}, False)
                End If

            Else
                JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "fail", "quot"}}, {{"reason", "insufficient_balance", "quot"}}, False)
            End If

        End If

        Return JRS

    End Function

End Class
