Imports System.Net.Sockets
Imports System.Text

Public Class send_p2p_relay

    Public Shared Async Sub exe(json As Newtonsoft.Json.Linq.JObject, JSS As String)

        ' p2p 자식노드를 가져온다.

        Dim server_id As String = GRT.get_local_server_id.exe
        Dim signiture_key = Regex.Match(json("value")("signiture").ToString, "^0x.{64}").ToString

        Dim Connection_mariadb_local_bc_manager As New MySqlConnection(GRT.GR.cString_mariadb_local_bc_manager)

        Connection_mariadb_local_bc_manager.Open()

        Dim Selectcmd As MySqlCommand
        Dim Adapter As MySqlDataAdapter
        Dim Dataset As DataSet

        'CREATE PROCEDURE `up_select_node_children`
        '(IN p_na  varchar(40))
        'BEGIN
        'SELECT na, domain, ip, port
        'FROM node
        'WHERE parent = p_na;
        'END

        Selectcmd = New MySqlCommand("up_select_node_children", Connection_mariadb_local_bc_manager)
        Selectcmd.CommandType = CommandType.StoredProcedure
        Selectcmd.Parameters.Add(New MySqlParameter("p_na", Regex.Replace(server_id, "^0x", "")))

        Adapter = New MySqlDataAdapter
        Adapter.SelectCommand = Selectcmd
        Dataset = New DataSet
        Adapter.Fill(Dataset)

        Connection_mariadb_local_bc_manager.Close()

        Dim count As Integer = Dataset.Tables(0).Rows.Count

        If GRT.GR.node_level = 0 Then
            Call GRT.agent_record.state_update("relay_started", "", signiture_key)
        End If

        For i = 0 To count - 1

            Try

                Call multi_net_sender.exe(json, i, Dataset, JSS)
                'Call (New multi_net_sender).exe(json, i, Dataset, JSS) ' async 로 진행하기 위해서다. 하지만 위의 것도 되는 듯.

            Catch ex As Exception

                If GRT.GR.node_level = 0 Then
                    Call GRT.agent_record.state_update("relay_error", ex.Message, signiture_key)
                End If

                If Regex.Match(ex.Message, "응답|disconnected ").Success Then
                    ' 그냥 넘어간다. 이는 해당 서버에서 스스로 추후 싱크로를 해야 하는 문제이다.
                Else
                    Err.Raise(513)
                End If

            End Try

        Next

        If GRT.GR.node_level = 0 Then
            Call GRT.agent_record.state_update("relay_succeeded", "", signiture_key)
        End If

    End Sub

End Class
