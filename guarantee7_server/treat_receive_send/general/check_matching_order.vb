Public Class check_matching_order

    Public Shared Function exe(json As Newtonsoft.Json.Linq.JObject) As Decimal

        Dim coin_name_from, coin_name_to As String
        Dim exchange_rate As Decimal

        coin_name_from = json("value")("coin_name_from").ToString
        coin_name_to = json("value")("coin_name_to").ToString
        exchange_rate = CDec(json("value")("exchange_rate"))

        ' 거래 가능한 오더가 올라와 있는지를 확인한다.

        Dim Connection_mariadb_local As New MySqlConnection(GRT.GR.cString_mariadb_local_bc)

        Connection_mariadb_local.Open()

        Dim Selectcmd As MySqlCommand
        Dim Adapter As MySqlDataAdapter
        Dim Dataset As DataSet

        'CREATE PROCEDURE `up_select_board_exchange_check_matching_order`
        '(IN p_coin_name_sell varchar(50),
        'IN p_coin_name_buy varchar(50),
        'IN p_exchange_rate_reciprocal decimal(60,30))
        'BEGIN
        'SELECT SUM(amount)
        'FROM exchange
        'WHERE `coin_name_from` = p_coin_name_buy AND `coin_name_to` = p_coin_name_sell
        'AND `exchange_rate` <= p_exchange_rate_reciprocal;
        'END

        Selectcmd = New MySqlCommand("up_select_board_exchange_check_matching_order", Connection_mariadb_local)
        Selectcmd.Parameters.Add(New MySqlParameter("p_coin_name_sell", coin_name_from))
        Selectcmd.Parameters.Add(New MySqlParameter("p_coin_name_buy", coin_name_to))
        Selectcmd.Parameters.Add(New MySqlParameter("p_exchange_rate_reciprocal", 1 / exchange_rate))

        Adapter = New MySqlDataAdapter
        Selectcmd.CommandType = CommandType.StoredProcedure
        Adapter.SelectCommand = Selectcmd
        Dataset = New DataSet
        Adapter.Fill(Dataset)

        Connection_mariadb_local.Close()

        Return CDec(Dataset.Tables(0).Rows(0)(0))

    End Function

End Class
