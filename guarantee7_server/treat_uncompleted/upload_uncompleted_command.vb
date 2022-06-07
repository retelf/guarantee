Imports Nethereum.RPC.Eth.DTOs
Imports Nethereum.Web3
Imports System.Net.Sockets
Imports System.Text

Public Class upload_uncompleted_command

    Public Shared Async Sub exe()

        ' 먼저 아직 컨펌이 안된 명령 추출 - signiture_key 로 확인

        Dim signiture_key, transaction_hash, contract_type, JSS, state, error_message As String
        Dim level As Integer
        Dim receipt As New TransactionReceipt
        Dim receipt_string As String

        Dim Connection_mariadb_local_bc_agent As New MySqlConnection(GRT.GR.cString_mariadb_local_bc_agent)

        Connection_mariadb_local_bc_agent.Open()

        Dim Selectcmd As MySqlCommand
        Dim Adapter As MySqlDataAdapter
        Dim Dataset As DataSet

        'CREATE PROCEDURE `up_select_uncompleted_command`
        '()
        'BEGIN
        'SELECT level, signiture_key, transaction_hash, contract_type, JSS, state, error_message
        'FROM agent_record
        'WHERE
        '`state` NOT LIKE '%|confirmed';
        'END

        Selectcmd = New MySqlCommand("up_select_uncompleted_command", Connection_mariadb_local_bc_agent)
        Selectcmd.CommandType = CommandType.StoredProcedure

        Adapter = New MySqlDataAdapter
        Selectcmd.CommandType = CommandType.StoredProcedure
        Adapter.SelectCommand = Selectcmd
        Dataset = New DataSet
        Adapter.Fill(Dataset)

        Connection_mariadb_local_bc_agent.Close()

        Dim count = Dataset.Tables(0).Rows.Count

        For i = 0 To count - 1

            level = CInt(Dataset.Tables(0).Rows(i)("level"))
            transaction_hash = CStr(Dataset.Tables(0).Rows(i)("transaction_hash"))
            signiture_key = CStr(Dataset.Tables(0).Rows(i)("signiture_key"))
            contract_type = CStr(Dataset.Tables(0).Rows(i)("contract_type"))
            JSS = CStr(Dataset.Tables(0).Rows(i)("JSS"))
            state = CStr(Dataset.Tables(0).Rows(i)("state"))
            error_message = CStr(Dataset.Tables(0).Rows(i)("error_message"))

            If Regex.Match(state, "\|transaction_hash_generated\|").Success Then

                If Regex.Match(state, "\|receipt_received\|").Success Then

                    ' 메인에서 명령이 실행되어 있나 확인할 필요 없이 무조건 JSS 를 올리기만 해도 된다.

                    Using gnet_client As Socket = GRT.Net.Gnet.net_sender(GRT.GR.main_server_address_agency, GRT.GR.port_number_server_main_agency, "localhost", GRT.GR.port_number_server_local)

                        Dim bytes As Byte() = New Byte(40000) {}

                        Dim byte_message_to_send As Byte() = Encoding.Unicode.GetBytes("<#BOF%>" & JSS & "<#EOF%>")

                        Call gnet_client.Send(byte_message_to_send)

                    End Using

                Else

                    '어쨌든 영수증을 다시 한 번 발급해 receipt 업데이트는 해 준다. 만약 발급이 된다면. (그 세부내용을 통하여 처리해야 할 지침을 남긴다. 이는 추후 할 일이다.)

                    Dim web3 = New Web3("http://localhost:8545")
                    receipt = Await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transaction_hash)
                    receipt_string = CType(JsonConvert.SerializeObject(receipt), String)

                    If Not receipt Is Nothing Then

                        Call GRT.agent_record.save_receipt(receipt_string, signiture_key)

                    End If

                    If Regex.Match(state, "\|tx_pending_so_long\|").Success Then

                        ' 로딩하지 말아야 한다. 가스비를 너무 낮게 책정한 본인 책임이다.
                        ' 추후 본인들이 확인을 한 후 외부에서 처리를 결정해야 한다.
                        ' 실행이 되는 지는 예측이 불가능하기 때문에 거래취소 역시 본인들에게 맡겨야 한다.

                    Else ' 컴퓨터가 꺼지거나 송신장애 등의 경우이다.

                        ' 해시를 확인한 후 영수증이 발급되면 로딩한다고 생각할 수도 있지만 ...
                        ' 역시 아무것도 하지 않아야 한다. 당사자들 사이에서 이미 해결을 했을 수도 있기 때문이다.
                        ' 영수증 발급만으로 족하다. 외부에서 처리하도록 해 주어야 한다.

                    End If

                    Call GRT.agent_record.state_update("need_offline_settlement", "", signiture_key)

                End If

            Else

                ' 일단은 아무 할 일이 없다.

                ' 특별한 생각이 없다면 이곳에서 지워 주는 것이 좋겠다.

            End If

            ' 여기서 반드시 block_number = 0  처리를 해 주어야 한다. 목적은 이 다음에 다시 반복하여 하지 않도록 하기 위함이다.
            ' 로우를 지울 필요는 없다.

            Call treat_agent_record_zero_block_number.exe(Dataset.Tables(0).Rows(i), receipt)

        Next

    End Sub

End Class
