Imports Newtonsoft.Json

Public Class treat_display_board_exchange

    Public Shared Function exe(json As Newtonsoft.Json.Linq.JObject) As String

        Dim eoa, coin_name_from, coin_name_to, signiture, JRS, dataset_json_string As String

        eoa = json("value")("eoa").ToString
        coin_name_from = json("value")("coin_name_from").ToString
        coin_name_to = json("value")("coin_name_to").ToString
        signiture = json("value")("signiture").ToString

        ' 먼저 검증

        Dim verified As Boolean = GRT.Security.Gverify.verify("", signiture, eoa)

        If verified Then

            Dim Connection_mariadb_local As New MySqlConnection(GRT.GR.cString_mariadb_local_bc)

            Connection_mariadb_local.Open()

            Dim Selectcmd As MySqlCommand
            Dim Adapter As MySqlDataAdapter
            Dim Dataset As DataSet

            'CREATE PROCEDURE `up_select_board_exchange`
            '(IN p_coin_name_sell varchar(50),
            'IN p_coin_name_buy varchar(50))
            'BEGIN
            'SELECT block_number, eoa, na, domain, ip, port, coin_name_from, coin_name_to, amount, exchange_rate, exchange_fee_rate
            'FROM exchange
            'WHERE
            '`coin_name_from` = p_coin_name_buy AND `coin_name_to` = p_coin_name_sell
            'ORDER BY exchange_rate DESC
            'LIMIT 10;
            'SELECT block_number, eoa, na, domain, ip, port, coin_name_from, coin_name_to, amount, exchange_rate, exchange_fee_rate
            'FROM exchange
            'WHERE
            '`coin_name_from` = p_coin_name_sell AND `coin_name_to` = p_coin_name_buy
            'ORDER BY exchange_rate ASC
            'LIMIT 10;
            'END

            Selectcmd = New MySqlCommand("up_select_board_exchange", Connection_mariadb_local)
            Selectcmd.Parameters.Add(New MySqlParameter("p_coin_name_sell", coin_name_from))
            Selectcmd.Parameters.Add(New MySqlParameter("p_coin_name_buy", coin_name_to))

            Adapter = New MySqlDataAdapter
            Selectcmd.CommandType = CommandType.StoredProcedure
            Adapter.SelectCommand = Selectcmd
            Dataset = New DataSet
            Adapter.Fill(Dataset)

            Connection_mariadb_local.Close()

            dataset_json_string = JsonConvert.SerializeObject(Dataset)

            JRS = "{""key"" : ""display_board_exchange"", ""success"" : ""success"", " &
                        """value"": {""publick_key"": """ & eoa & """, ""dataset_json_string"": " & dataset_json_string & "}}"

        Else

            JRS = "{""key"" : ""display_board_exchange"", ""success"" : ""fail"", ""value"": {""publick_key"": """ & eoa & """,""reason"": ""verification_fail""}}"

        End If

        Return JRS

    End Function

End Class
