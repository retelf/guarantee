Imports Newtonsoft.Json

Public Class treat_display_board_multilevel_sell_order_bak

    Public Shared Function exe(json As Newtonsoft.Json.Linq.JObject) As String

        Dim eoa, coin_name, signiture, JRS, dataset_json_string As String
        Dim date_now_utc = GRT.GetNistTime.exe

        eoa = json("value")("eoa").ToString
        coin_name = json("value")("coin_name").ToString
        signiture = json("value")("signiture").ToString

        ' 먼저 검증

        Dim verified As Boolean = GRT.Security.Gverify.verify("", signiture, eoa)

        If verified Then

            Dim Connection_mariadb_local_bc_multilevel As New MySqlConnection(GRT.GR.cString_mariadb_local_bc_multilevel)

            Connection_mariadb_local_bc_multilevel.Open()

            Dim Selectcmd As MySqlCommand
            Dim Adapter As MySqlDataAdapter
            Dim Dataset As DataSet

            'CREATE PROCEDURE `up_select_board_sell_order_efficient`
            '(IN p_date_now_utc datetime)
            'BEGIN
            'SELECT block_number, eoa, na, exchange_name, domain, ip, port, exchange_rate, days_span, closing_time, state, exchange_fee_rate
            'FROM sell_order
            'WHERE (`closing_time` > p_date_now_utc AND `state` = 'seized') OR `state` = 'alive'
            'ORDER BY `exchange_rate`
            'LIMIT 20;
            'END

            Selectcmd = New MySqlCommand("up_select_board_sell_order_efficient", Connection_mariadb_local_bc_multilevel)
            Selectcmd.Parameters.Add(New MySqlParameter("p_date_now_utc", date_now_utc.ToString("yyyy/MM/dd HH:mm:ss")))

            Adapter = New MySqlDataAdapter
            Selectcmd.CommandType = CommandType.StoredProcedure
            Adapter.SelectCommand = Selectcmd
            Dataset = New DataSet
            Adapter.Fill(Dataset)

            Connection_mariadb_local_bc_multilevel.Close()

            dataset_json_string = JsonConvert.SerializeObject(Dataset)

            JRS = "{""key"" : """ & json("key").ToString & """, ""success"" : ""success"", " &
                        """value"": {""publick_key"": """ & eoa & """, ""dataset_json_string"": " & dataset_json_string & "}}"

        Else

            JRS = "{""key"" : ""display_board_sell_order"", ""success"" : ""fail"", ""value"": {""publick_key"": """ & eoa & """,""reason"": ""verification_fail""}}"

        End If

        Return JRS

    End Function

End Class
