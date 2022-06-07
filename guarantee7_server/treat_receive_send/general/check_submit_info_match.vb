Public Class check_submit_info_match

    Public Shared Function multilevel_buy(json As Newtonsoft.Json.Linq.JObject) As Newtonsoft.Json.Linq.JObject

        Dim eoa_guarantee_seller, seller_na, JRS As String
        Dim exchange_rate, exchange_fee_rate As Decimal
        Dim sell_order_block_number As Long
        Dim seller_agency_domain, seller_agency_ip As String
        Dim seller_agency_port, days_span As Integer

        sell_order_block_number = CLng(json("value")("sell_order_block_number"))
        eoa_guarantee_seller = json("value")("eoa_guarantee_seller").ToString
        seller_na = json("value")("seller_na").ToString
        seller_agency_domain = json("value")("seller_agency_domain").ToString
        seller_agency_ip = json("value")("seller_agency_ip").ToString
        seller_agency_port = CInt(json("value")("seller_agency_port"))
        days_span = CInt(json("value")("days_span").ToString.Trim)
        exchange_rate = CDec(json("value")("exchange_rate"))
        exchange_fee_rate = CDec(json("value")("exchange_fee_rate"))

        Dim Connection_mariadb_local_bc_multilevel As New MySqlConnection(GRT.GR.cString_mariadb_local_bc_multilevel)

        Connection_mariadb_local_bc_multilevel.Open()

        Dim Selectcmd As MySqlCommand
        Dim Adapter As MySqlDataAdapter
        Dim Dataset As DataSet

        'CREATE PROCEDURE `up_select_board_sell_order_for_verify`
        '(IN `p_sell_order_block_number` bigint(20))
        'BEGIN
        'SELECT eoa, na, domain, ip, port, exchange_rate, days_span, closing_time, exchange_fee_rate
        'FROM sell_order
        'WHERE block_number = p_sell_order_block_number;
        'END

        Selectcmd = New MySqlCommand("up_select_board_sell_order_for_verify", Connection_mariadb_local_bc_multilevel)
        Selectcmd.CommandType = CommandType.StoredProcedure
        Selectcmd.Parameters.Add(New MySqlParameter("p_sell_order_block_number", sell_order_block_number))

        Adapter = New MySqlDataAdapter
        Adapter.SelectCommand = Selectcmd
        Dataset = New DataSet
        Adapter.Fill(Dataset)

        Connection_mariadb_local_bc_multilevel.Close()

        If Dataset.Tables(0).Rows.Count = 1 Then

            If Regex.Replace(eoa_guarantee_seller, "^0x", "") = CStr(Dataset.Tables(0).Rows(0)("eoa")) And
                Regex.Replace(seller_na, "^0x", "") = CStr(Dataset.Tables(0).Rows(0)("na")) And
                seller_agency_domain = CStr(Dataset.Tables(0).Rows(0)("domain")) And
                seller_agency_ip = CStr(Dataset.Tables(0).Rows(0)("ip")) And
                seller_agency_port = CInt(Dataset.Tables(0).Rows(0)("port")) And
                exchange_rate = CDec(Dataset.Tables(0).Rows(0)("exchange_rate")) And
                days_span = CInt(Dataset.Tables(0).Rows(0)("days_span")) And
                exchange_fee_rate = CDec(Dataset.Tables(0).Rows(0)("exchange_fee_rate")) Then

                JRS = GRT.make_json_string.exe({{"success", "success", "quot"}}, {{"reason", "", "quot"}}, False)

            Else

                JRS = GRT.make_json_string.exe({{"success", "fail", "quot"}}, {{"reason", "multilevel_submit_buy_info_not_match", "quot"}}, False)

            End If

        Else
            JRS = GRT.make_json_string.exe({{"success", "fail", "quot"}}, {{"reason", "multilevel_submit_buy_info_rows_not_exist", "quot"}}, False)
        End If

        Return CType(JsonConvert.DeserializeObject(JRS), Linq.JObject)

    End Function

    Public Shared Function multilevel_refund(json As Newtonsoft.Json.Linq.JObject) As Newtonsoft.Json.Linq.JObject

        Dim eoa_guarantee_seller, seller_na, JRS As String
        Dim sell_order_block_number As Long
        Dim seller_agency_domain, seller_agency_ip As String
        Dim seller_agency_port As Integer

        sell_order_block_number = CLng(json("value")("sell_order_block_number"))
        eoa_guarantee_seller = json("value")("eoa_guarantee_seller").ToString
        seller_na = json("value")("seller_na").ToString
        seller_agency_domain = json("value")("seller_agency_domain").ToString
        seller_agency_ip = json("value")("seller_agency_ip").ToString
        seller_agency_port = CInt(json("value")("seller_agency_port"))

        Dim Connection_mariadb_local_bc_multilevel As New MySqlConnection(GRT.GR.cString_mariadb_local_bc_multilevel)

        Connection_mariadb_local_bc_multilevel.Open()

        Dim Selectcmd As MySqlCommand
        Dim Adapter As MySqlDataAdapter
        Dim Dataset As DataSet

        'CREATE PROCEDURE `up_select_board_sell_order_for_verify`
        '(IN `p_sell_order_block_number` bigint(20))
        'BEGIN
        'SELECT eoa, na, domain, ip, port, exchange_rate, days_span, closing_time, exchange_fee_rate
        'FROM sell_order
        'WHERE block_number = p_sell_order_block_number;
        'END

        Selectcmd = New MySqlCommand("up_select_board_sell_order_for_verify", Connection_mariadb_local_bc_multilevel)
        Selectcmd.CommandType = CommandType.StoredProcedure
        Selectcmd.Parameters.Add(New MySqlParameter("p_sell_order_block_number", sell_order_block_number))

        Adapter = New MySqlDataAdapter
        Adapter.SelectCommand = Selectcmd
        Dataset = New DataSet
        Adapter.Fill(Dataset)

        Connection_mariadb_local_bc_multilevel.Close()

        If Dataset.Tables(0).Rows.Count = 1 Then

            If Regex.Replace(eoa_guarantee_seller, "^0x", "") = CStr(Dataset.Tables(0).Rows(0)("eoa")) And
                Regex.Replace(seller_na, "^0x", "") = CStr(Dataset.Tables(0).Rows(0)("na")) And
                seller_agency_domain = CStr(Dataset.Tables(0).Rows(0)("domain")) And
                seller_agency_ip = CStr(Dataset.Tables(0).Rows(0)("ip")) And
                seller_agency_port = CInt(Dataset.Tables(0).Rows(0)("port")) Then

                JRS = GRT.make_json_string.exe({{"success", "success", "quot"}}, {{"reason", "", "quot"}}, False)

            Else

                JRS = GRT.make_json_string.exe({{"success", "fail", "quot"}}, {{"reason", "multilevel_submit_refund_info_not_match", "quot"}}, False)

            End If

        Else
            JRS = GRT.make_json_string.exe({{"success", "fail", "quot"}}, {{"reason", "multilevel_submit_refund_info_rows_not_exist", "quot"}}, False)
        End If

        Return CType(JsonConvert.DeserializeObject(JRS), Linq.JObject)

    End Function

    Public Shared Function multilevel_recall(json As Newtonsoft.Json.Linq.JObject) As Newtonsoft.Json.Linq.JObject

        Dim eoa_guarantee_seller, eoa_signer, seller_na, ma, JRS As String
        Dim exchange_rate, exchange_fee_rate As Decimal
        Dim sell_order_block_number As Long
        Dim seller_agency_domain, seller_agency_ip As String
        Dim seller_agency_port, days_span As Integer

        sell_order_block_number = CLng(json("value")("sell_order_block_number"))
        eoa_guarantee_seller = json("value")("eoa_guarantee_seller").ToString
        eoa_signer = json("value")("eoa_signer").ToString
        seller_na = json("value")("seller_na").ToString
        ma = json("value")("ma").ToString
        seller_agency_domain = json("value")("seller_agency_domain").ToString
        seller_agency_ip = json("value")("seller_agency_ip").ToString
        seller_agency_port = CInt(json("value")("seller_agency_port"))
        days_span = CInt(json("value")("days_span").ToString.Trim)
        exchange_rate = CDec(json("value")("exchange_rate"))
        exchange_fee_rate = CDec(json("value")("exchange_fee_rate"))
        exchange_fee_rate = CDec(json("value")("exchange_fee_rate"))

        Dim Connection_mariadb_local_bc_multilevel As New MySqlConnection(GRT.GR.cString_mariadb_local_bc_multilevel)

        Connection_mariadb_local_bc_multilevel.Open()

        Dim Selectcmd As MySqlCommand
        Dim Adapter As MySqlDataAdapter
        Dim Dataset As DataSet

        'CREATE PROCEDURE `up_select_board_sell_order_efficient_for_verify`
        '(IN p_sell_order_block_number bigint(20))
        'BEGIN
        'SELECT block_number, eoa, na, buyer, ma, exchange_name, domain, ip, port, exchange_rate, days_span, closing_time, state, exchange_fee_rate
        'FROM sell_order_plus_account
        'WHERE block_number = p_sell_order_block_number;
        'END

        Selectcmd = New MySqlCommand("up_select_board_sell_order_efficient_for_verify", Connection_mariadb_local_bc_multilevel)
        Selectcmd.CommandType = CommandType.StoredProcedure
        Selectcmd.Parameters.Add(New MySqlParameter("p_sell_order_block_number", sell_order_block_number))

        Adapter = New MySqlDataAdapter
        Adapter.SelectCommand = Selectcmd
        Dataset = New DataSet
        Adapter.Fill(Dataset)

        Connection_mariadb_local_bc_multilevel.Close()

        If Dataset.Tables(0).Rows.Count = 1 Then

            If Regex.Replace(eoa_guarantee_seller, "^0x", "") = CStr(Dataset.Tables(0).Rows(0)("eoa")) And
                Regex.Replace(seller_na, "^0x", "") = CStr(Dataset.Tables(0).Rows(0)("na")) And
                Regex.Replace(eoa_signer, "^0x", "") = CStr(Dataset.Tables(0).Rows(0)("buyer")) And
                Regex.Replace(ma, "^0x", "") = CStr(Dataset.Tables(0).Rows(0)("ma")) And
                Regex.Replace(seller_na, "^0x", "") = CStr(Dataset.Tables(0).Rows(0)("na")) And
                seller_agency_domain = CStr(Dataset.Tables(0).Rows(0)("domain")) And
                seller_agency_ip = CStr(Dataset.Tables(0).Rows(0)("ip")) And
                seller_agency_port = CInt(Dataset.Tables(0).Rows(0)("port")) And
                exchange_rate = CDec(Dataset.Tables(0).Rows(0)("exchange_rate")) And
                days_span = CInt(Dataset.Tables(0).Rows(0)("days_span")) And
                exchange_fee_rate = CDec(Dataset.Tables(0).Rows(0)("exchange_fee_rate")) Then

                JRS = GRT.make_json_string.exe({{"success", "success", "quot"}}, {{"reason", "", "quot"}}, False)

            Else

                JRS = GRT.make_json_string.exe({{"success", "fail", "quot"}}, {{"reason", "multilevel_submit_buy_info_not_match", "quot"}}, False)

            End If

        Else
            JRS = GRT.make_json_string.exe({{"success", "fail", "quot"}}, {{"reason", "multilevel_submit_buy_info_rows_not_exist", "quot"}}, False)
        End If

        Return CType(JsonConvert.DeserializeObject(JRS), Linq.JObject)

    End Function

    Public Shared Function exchange(json As Newtonsoft.Json.Linq.JObject) As Newtonsoft.Json.Linq.JObject

        Dim command_key, eoa_seller, seller_na, clickers_coin_name_to_buy, clickers_coin_name_to_sell, JRS As String
        Dim seller_amount, exchange_rate, exchange_fee_rate As Decimal
        Dim exchange_block_number As Long

        command_key = json("key").ToString

        exchange_block_number = CLng(json("value")("exchange_block_number"))
        eoa_seller = json("value")("eoa_seller").ToString
        seller_na = json("value")("na").ToString
        clickers_coin_name_to_buy = json("value")("clickers_coin_name_to_buy").ToString
        clickers_coin_name_to_sell = json("value")("clickers_coin_name_to_sell").ToString
        seller_amount = CDec(json("value")("amount"))
        exchange_rate = CDec(json("value")("exchange_rate"))
        exchange_fee_rate = CDec(json("value")("exchange_fee_rate"))

        Dim Connection_mariadb_local_bc As New MySqlConnection(GRT.GR.cString_mariadb_local_bc)

        Connection_mariadb_local_bc.Open()

        Dim Selectcmd As MySqlCommand
        Dim Adapter As MySqlDataAdapter
        Dim Dataset As DataSet

        'CREATE PROCEDURE `up_select_board_exchange_efficient_for_verify`
        '(IN p_exchange_block_number bigint(20))
        'BEGIN
        'SELECT block_number, eoa, na, coin_name_from, coin_name_to, amount, exchange_rate, exchange_fee_rate, state
        'FROM exchange
        'WHERE block_number = p_exchange_block_number;
        'END

        Selectcmd = New MySqlCommand("up_select_board_exchange_efficient_for_verify", Connection_mariadb_local_bc)
        Selectcmd.CommandType = CommandType.StoredProcedure
        Selectcmd.Parameters.Add(New MySqlParameter("p_exchange_block_number", exchange_block_number))

        Adapter = New MySqlDataAdapter
        Adapter.SelectCommand = Selectcmd
        Dataset = New DataSet
        Adapter.Fill(Dataset)

        Connection_mariadb_local_bc.Close()

        If Dataset.Tables(0).Rows.Count = 1 Then

            If Regex.Replace(eoa_seller, "^0x", "") = CStr(Dataset.Tables(0).Rows(0)("eoa")) And
                Regex.Replace(seller_na, "^0x", "") = CStr(Dataset.Tables(0).Rows(0)("na")) And
                clickers_coin_name_to_sell = CStr(Dataset.Tables(0).Rows(0)("coin_name_to")) And
                clickers_coin_name_to_buy = CStr(Dataset.Tables(0).Rows(0)("coin_name_from")) And
                seller_amount = CDec(Dataset.Tables(0).Rows(0)("amount")) And
                exchange_rate = CDec(Dataset.Tables(0).Rows(0)("exchange_rate")) And
                exchange_fee_rate = CDec(Dataset.Tables(0).Rows(0)("exchange_fee_rate")) And
                "alive" = CStr(Dataset.Tables(0).Rows(0)("state")) Then

                JRS = GRT.make_json_string.exe({{"success", "success", "quot"}}, {{"reason", "", "quot"}}, False)

            Else

                JRS = GRT.make_json_string.exe({{"success", "fail", "quot"}}, {{"reason", "multilevel_submit_buy_info_not_match", "quot"}}, False)

            End If

        Else
            JRS = GRT.make_json_string.exe({{"success", "fail", "quot"}}, {{"reason", "multilevel_submit_buy_info_rows_not_exist", "quot"}}, False)
        End If

        Return CType(JsonConvert.DeserializeObject(JRS), Linq.JObject)

    End Function

    Public Shared Function exchange_cancel(json As Newtonsoft.Json.Linq.JObject) As Newtonsoft.Json.Linq.JObject

        Dim command_key, eoa, na, coin_name_from, coin_name_to, JRS As String
        Dim seller_amount, exchange_rate, exchange_fee_rate As Decimal
        Dim cancel_block_number As Long

        command_key = json("key").ToString

        cancel_block_number = CLng(json("value")("cancel_block_number"))
        eoa = json("value")("eoa").ToString
        na = json("value")("na").ToString
        coin_name_from = json("value")("coin_name_from").ToString
        coin_name_to = json("value")("coin_name_to").ToString
        seller_amount = CDec(json("value")("amount"))
        exchange_rate = CDec(json("value")("exchange_rate"))
        exchange_fee_rate = CDec(json("value")("exchange_fee_rate"))

        Dim Connection_mariadb_local_bc As New MySqlConnection(GRT.GR.cString_mariadb_local_bc)

        Connection_mariadb_local_bc.Open()

        Dim Selectcmd As MySqlCommand
        Dim Adapter As MySqlDataAdapter
        Dim Dataset As DataSet

        'CREATE PROCEDURE `up_select_board_exchange_efficient_for_verify`
        '(IN p_exchange_block_number bigint(20))
        'BEGIN
        'SELECT block_number, eoa, na, coin_name_from, coin_name_to, amount, exchange_rate, exchange_fee_rate, state
        'FROM exchange
        'WHERE block_number = p_exchange_block_number;
        'END

        Selectcmd = New MySqlCommand("up_select_board_exchange_efficient_for_verify", Connection_mariadb_local_bc)
        Selectcmd.CommandType = CommandType.StoredProcedure
        Selectcmd.Parameters.Add(New MySqlParameter("p_exchange_block_number", cancel_block_number))

        Adapter = New MySqlDataAdapter
        Adapter.SelectCommand = Selectcmd
        Dataset = New DataSet
        Adapter.Fill(Dataset)

        Connection_mariadb_local_bc.Close()

        If Dataset.Tables(0).Rows.Count = 1 Then

            If Regex.Replace(eoa, "^0x", "") = CStr(Dataset.Tables(0).Rows(0)("eoa")) And
                Regex.Replace(na, "^0x", "") = CStr(Dataset.Tables(0).Rows(0)("na")) And
                coin_name_from = CStr(Dataset.Tables(0).Rows(0)("coin_name_from")) And
                coin_name_to = CStr(Dataset.Tables(0).Rows(0)("coin_name_to")) And
                seller_amount = CDec(Dataset.Tables(0).Rows(0)("amount")) And
                exchange_rate = CDec(Dataset.Tables(0).Rows(0)("exchange_rate")) And
                exchange_fee_rate = CDec(Dataset.Tables(0).Rows(0)("exchange_fee_rate")) And
                "alive" = CStr(Dataset.Tables(0).Rows(0)("state")) Then

                JRS = GRT.make_json_string.exe({{"success", "success", "quot"}}, {{"reason", "", "quot"}}, False)

            Else

                JRS = GRT.make_json_string.exe({{"success", "fail", "quot"}}, {{"reason", "multilevel_submit_buy_info_not_match", "quot"}}, False)

            End If

        Else
            JRS = GRT.make_json_string.exe({{"success", "fail", "quot"}}, {{"reason", "multilevel_submit_buy_info_rows_not_exist", "quot"}}, False)
        End If

        Return CType(JsonConvert.DeserializeObject(JRS), Linq.JObject)

    End Function

    Public Shared Function nft_buy(json As Newtonsoft.Json.Linq.JObject) As Newtonsoft.Json.Linq.JObject

        Dim Selectcmd As MySqlCommand
        Dim Adapter As MySqlDataAdapter
        Dim Dataset_nft, Dataset_manager_node, Dataset_manager_node_thread As DataSet

        Dim JRS As String
        Dim sell_order_block_number = CLng(json("value")("sell_order_block_number"))
        Dim owner_portion_block_number = CLng(json("value")("owner_portion_block_number"))
        Dim nfa = CStr(json("value")("nfa"))
        Dim token_id = CInt(json("value")("token_id"))
        Dim pieces_to_buy = CInt(json("value")("pieces_to_buy"))
        Dim pieces_alive = CInt(json("value")("pieces_alive"))
        Dim confirmed_type = CStr(json("value")("confirmed_type"))
        Dim price_piece = CDec(json("value")("price_piece"))
        Dim currency = CStr(json("value")("currency"))
        Dim max_split = CInt(json("value")("max_split"))
        Dim eoa_seller = CStr(json("value")("eoa_seller"))
        Dim eoa_buyer = CStr(json("value")("eoa_buyer"))
        Dim na_seller = CStr(json("value")("na_seller"))
        Dim exchange_name_seller = CStr(json("value")("exchange_name_seller"))
        Dim seller_agency_domain = CStr(json("value")("domain_seller"))
        Dim seller_agency_ip = CStr(json("value")("ip_seller"))
        Dim seller_agency_port = CInt(json("value")("port_seller"))
        Dim na_buyer = CStr(json("value")("na_buyer"))
        Dim exchange_name_buyer = CStr(json("value")("exchange_name_buyer"))
        Dim buyer_agency_domain = CStr(json("value")("domain_buyer"))
        Dim buyer_agency_ip = CStr(json("value")("ip_buyer"))
        Dim buyer_agency_port = CInt(json("value")("port_buyer"))
        Dim days_span = CInt(json("value")("days_span"))
        Dim transaction_fee_rate = CDec(json("value")("transaction_fee_rate"))
        Dim state = CStr(json("value")("state"))

        Dim Connection_mariadb_local_bc_nft As New MySqlConnection(GRT.GR.cString_mariadb_local_bc_nft)

        Connection_mariadb_local_bc_nft.Open()

        'CREATE PROCEDURE `up_select_nft_sell_order_for_one_block_number`
        '(IN p_sell_order_block_number bigint(20))
        'BEGIN
        'SELECT * FROM sell_order
        'WHERE block_number = p_sell_order_block_number;
        'END

        Selectcmd = New MySqlCommand("up_select_nft_sell_order_for_one_block_number", Connection_mariadb_local_bc_nft)
        Selectcmd.CommandType = CommandType.StoredProcedure
        Selectcmd.Parameters.Add(New MySqlParameter("p_sell_order_block_number", sell_order_block_number))

        Adapter = New MySqlDataAdapter
        Adapter.SelectCommand = Selectcmd
        Dataset_nft = New DataSet
        Adapter.Fill(Dataset_nft)

        Connection_mariadb_local_bc_nft.Close()

        If Dataset_nft.Tables(0).Rows.Count = 1 Then

            Dim na_buyer_for_verify = GRT.get_node_from_eoa.exe(eoa_buyer)

            If Not na_buyer_for_verify = "" Then

                Dim Connection_mariadb_local_bc_manager As New MySqlConnection(GRT.GR.cString_mariadb_local_bc_manager)

                Connection_mariadb_local_bc_manager.Open()

                'CREATE PROCEDURE `up_select_node_info`
                '(IN p_na char(40))
                'BEGIN
                'SELECT * FROM node
                'WHERE na = p_na;
                'END

                Selectcmd = New MySqlCommand("up_select_node_info", Connection_mariadb_local_bc_manager)
                Selectcmd.Parameters.Add(New MySqlParameter("p_na", Regex.Replace(na_buyer_for_verify, "^0x", "")))

                Adapter = New MySqlDataAdapter
                Selectcmd.CommandType = CommandType.StoredProcedure
                Adapter.SelectCommand = Selectcmd
                Dataset_manager_node = New DataSet
                Adapter.Fill(Dataset_manager_node)

                'CREATE PROCEDURE `up_select_node_thread_agency_info`
                '(IN p_na char(40))
                'BEGIN
                'SELECT * FROM node_thread
                'WHERE `na` = p_na AND `type` = 'agency';
                'END

                Selectcmd = New MySqlCommand("up_select_node_thread_agency_info", Connection_mariadb_local_bc_manager)
                Selectcmd.Parameters.Add(New MySqlParameter("p_na", Regex.Replace(na_buyer_for_verify, "^0x", "")))

                Adapter = New MySqlDataAdapter
                Selectcmd.CommandType = CommandType.StoredProcedure
                Adapter.SelectCommand = Selectcmd
                Dataset_manager_node_thread = New DataSet
                Adapter.Fill(Dataset_manager_node_thread)

                If Dataset_manager_node.Tables(0).Rows.Count = 1 And Dataset_manager_node_thread.Tables(0).Rows.Count = 1 Then

                    If owner_portion_block_number = CLng(Dataset_nft.Tables(0).Rows(0)("owner_portion_block_number")) And
                        Regex.Replace(nfa, "^0x", "") = CStr(Dataset_nft.Tables(0).Rows(0)("nfa")) And
                        token_id = CInt(Dataset_nft.Tables(0).Rows(0)("token_id")) And
                        Regex.Replace(eoa_seller, "^0x", "") = CStr(Dataset_nft.Tables(0).Rows(0)("eoa")) And
                        pieces_alive = CInt(Dataset_nft.Tables(0).Rows(0)("pieces_alive")) And
                        price_piece = CDec(Dataset_nft.Tables(0).Rows(0)("price_piece")) And
                        currency = CStr(Dataset_nft.Tables(0).Rows(0)("currency")) And
                        confirmed_type = CStr(Dataset_nft.Tables(0).Rows(0)("confirmed_type")) And
                        Regex.Replace(na_seller, "^0x", "") = CStr(Dataset_nft.Tables(0).Rows(0)("na")) And
                        exchange_name_seller = CStr(Dataset_nft.Tables(0).Rows(0)("exchange_name")) And
                        seller_agency_domain = CStr(Dataset_nft.Tables(0).Rows(0)("domain")) And
                        seller_agency_ip = CStr(Dataset_nft.Tables(0).Rows(0)("ip")) And
                        seller_agency_port = CInt(Dataset_nft.Tables(0).Rows(0)("port")) And
                        Regex.Replace(na_buyer, "^0x", "") = CStr(Dataset_manager_node.Tables(0).Rows(0)("na")) And
                        exchange_name_buyer = CStr(Dataset_manager_node.Tables(0).Rows(0)("exchange_name")) And
                        buyer_agency_domain = CStr(Dataset_manager_node_thread.Tables(0).Rows(0)("domain")) And
                        buyer_agency_ip = CStr(Dataset_manager_node_thread.Tables(0).Rows(0)("ip")) And
                        buyer_agency_port = CInt(Dataset_manager_node_thread.Tables(0).Rows(0)("port")) And
                        days_span = CInt(Dataset_nft.Tables(0).Rows(0)("days_span")) And
                        max_split = CInt(Dataset_nft.Tables(0).Rows(0)("max_split")) Then

                        JRS = GRT.make_json_string.exe({{"success", "success", "quot"}}, {{"pieces_alive", CStr(pieces_alive), "non_quot"}}, False)

                    Else

                        JRS = GRT.make_json_string.exe({{"success", "fail", "quot"}}, {{"reason", "nft_submit_buy_info_not_match", "quot"}}, False)

                    End If

                Else
                    JRS = GRT.make_json_string.exe({{"success", "fail", "quot"}}, {{"reason", "nft_submit_buy_info_rows_node_not_exist", "quot"}}, False)
                End If

            Else
                JRS = GRT.make_json_string.exe({{"success", "fail", "quot"}}, {{"reason", "nft_submit_buy_info_node_not_found", "quot"}}, False)
            End If

        Else
            JRS = GRT.make_json_string.exe({{"success", "fail", "quot"}}, {{"reason", "nft_submit_buy_info_sell_order_not_found", "quot"}}, False)
        End If

        Return CType(JsonConvert.DeserializeObject(JRS), Linq.JObject)

    End Function

    Public Shared Function nft_recall(json As Newtonsoft.Json.Linq.JObject) As Newtonsoft.Json.Linq.JObject

        Dim Selectcmd As MySqlCommand
        Dim Adapter As MySqlDataAdapter
        Dim Dataset_nft_owner_portion_sell_order, Dataset_manager_node, Dataset_manager_node_thread As DataSet

        Dim JRS As String

        Dim owner_portion_block_number = CLng(json("value")("owner_portion_block_number"))
        Dim sell_order_block_number = CLng(json("value")("sell_order_block_number"))
        Dim owner_portion_parent_block_number = CLng(json("value")("owner_portion_parent_block_number"))
        Dim nfa = CStr(json("value")("nfa"))
        Dim token_id = CInt(json("value")("token_id"))
        Dim pieces_to_recall = CInt(json("value")("pieces_to_recall"))
        Dim pieces_seized = CInt(json("value")("pieces_seized"))
        Dim confirmed_type = CStr(json("value")("confirmed_type"))
        Dim price_piece = CDec(json("value")("price_piece"))
        Dim price_sum = CDec(json("value")("price_sum"))

        Dim nonce = CLng(json("value")("nonce"))
        Dim gasPrice_Gwei = CDec(json("value")("gasPrice_Gwei"))
        Dim gasLimit = CDec(json("value")("gasLimit"))

        Dim currency = CStr(json("value")("currency"))
        Dim max_split = CInt(json("value")("max_split"))
        Dim eoa_seller = CStr(json("value")("eoa_seller"))
        Dim eoa_recaller = CStr(json("value")("eoa_recaller"))
        Dim na_seller = CStr(json("value")("na_seller"))
        Dim exchange_name_seller = CStr(json("value")("exchange_name_seller"))
        Dim domain_seller = CStr(json("value")("domain_seller"))
        Dim ip_seller = CStr(json("value")("ip_seller"))
        Dim port_seller = CInt(json("value")("port_seller"))
        Dim na_recaller = CStr(json("value")("na_recaller"))
        Dim exchange_name_recaller = CStr(json("value")("exchange_name_recaller"))
        Dim domain_recaller = CStr(json("value")("domain_recaller"))
        Dim ip_recaller = CStr(json("value")("ip_recaller"))
        Dim port_recaller = CInt(json("value")("port_recaller"))
        Dim days_span = CInt(json("value")("days_span"))
        Dim closing_time = CDate(json("value")("closing_time"))
        Dim transaction_fee_rate = CSng(json("value")("transaction_fee_rate"))
        Dim state = CStr(json("value")("state"))

        Dim idate_string = CStr(json("value")("idate_string"))
        Dim signiture_data_for_ethereum_transfer = CStr(json("value")("signiture_data_for_ethereum_transfer"))
        Dim signiture_data_for_ethereum_transfer_cancel = CStr(json("value")("signiture_data_for_ethereum_transfer_cancel"))
        Dim transfer_ethereum_from_exchange_case = CBool(json("value")("transfer_ethereum_from_exchange_case"))
        Dim pure_query = CStr(json("value")("pure_query"))
        Dim signiture = CStr(json("value")("signiture"))
        Dim signiture_key = CStr(json("value")("signiture_key"))
        Dim pure_query_for_ethereum_transfer_not_succeeded_and_no_need_cancel_transfer = CStr(json("value")("pure_query_for_ethereum_transfer_not_succeeded_and_no_need_cancel_transfer"))
        Dim signiture_for_ethereum_transfer_not_succeeded_and_no_need_cancel_transfer = CStr(json("value")("signiture_for_ethereum_transfer_not_succeeded_and_no_need_cancel_transfer"))

        Dim Connection_mariadb_local_bc_nft As New MySqlConnection(GRT.GR.cString_mariadb_local_bc_nft)

        Connection_mariadb_local_bc_nft.Open()

        'CREATE PROCEDURE `up_select_nft_owner_portion_sell_order_for_one_block_number`
        '(IN p_owner_portion_sell_order_block_number bigint(20))
        'BEGIN
        'SELECT * FROM owner_portion_sell_order
        'WHERE owner_portion_block_number = p_owner_portion_sell_order_block_number;
        'END

        Selectcmd = New MySqlCommand("up_select_nft_owner_portion_sell_order_for_one_block_number", Connection_mariadb_local_bc_nft)
        Selectcmd.CommandType = CommandType.StoredProcedure
        Selectcmd.Parameters.Add(New MySqlParameter("p_owner_portion_sell_order_block_number", owner_portion_block_number))

        Adapter = New MySqlDataAdapter
        Adapter.SelectCommand = Selectcmd
        Dataset_nft_owner_portion_sell_order = New DataSet
        Adapter.Fill(Dataset_nft_owner_portion_sell_order)

        Connection_mariadb_local_bc_nft.Close()

        If Dataset_nft_owner_portion_sell_order.Tables(0).Rows.Count = 1 Then

            Dim na_buyer_for_verify = GRT.get_node_from_eoa.exe(eoa_recaller)

            If Not na_buyer_for_verify = "" Then

                Dim Connection_mariadb_local_bc_manager As New MySqlConnection(GRT.GR.cString_mariadb_local_bc_manager)

                Connection_mariadb_local_bc_manager.Open()

                'CREATE PROCEDURE `up_select_node_info`
                '(IN p_na char(40))
                'BEGIN
                'SELECT * FROM node
                'WHERE na = p_na;
                'END

                Selectcmd = New MySqlCommand("up_select_node_info", Connection_mariadb_local_bc_manager)
                Selectcmd.Parameters.Add(New MySqlParameter("p_na", Regex.Replace(na_buyer_for_verify, "^0x", "")))

                Adapter = New MySqlDataAdapter
                Selectcmd.CommandType = CommandType.StoredProcedure
                Adapter.SelectCommand = Selectcmd
                Dataset_manager_node = New DataSet
                Adapter.Fill(Dataset_manager_node)

                'CREATE PROCEDURE `up_select_node_thread_agency_info`
                '(IN p_na char(40))
                'BEGIN
                'SELECT * FROM node_thread
                'WHERE `na` = p_na AND `type` = 'agency';
                'END

                Selectcmd = New MySqlCommand("up_select_node_thread_agency_info", Connection_mariadb_local_bc_manager)
                Selectcmd.Parameters.Add(New MySqlParameter("p_na", Regex.Replace(na_buyer_for_verify, "^0x", "")))

                Adapter = New MySqlDataAdapter
                Selectcmd.CommandType = CommandType.StoredProcedure
                Adapter.SelectCommand = Selectcmd
                Dataset_manager_node_thread = New DataSet
                Adapter.Fill(Dataset_manager_node_thread)

                If Dataset_manager_node.Tables(0).Rows.Count = 1 And Dataset_manager_node_thread.Tables(0).Rows.Count = 1 Then

                    If sell_order_block_number = CLng(Dataset_nft_owner_portion_sell_order.Tables(0).Rows(0)("sell_order_block_number")) And
                        owner_portion_parent_block_number = CLng(Dataset_nft_owner_portion_sell_order.Tables(0).Rows(0)("owner_portion_parent_block_number")) And
                        Regex.Replace(nfa, "^0x", "") = CStr(Dataset_nft_owner_portion_sell_order.Tables(0).Rows(0)("owner_portion_nfa")) And
                        token_id = CInt(Dataset_nft_owner_portion_sell_order.Tables(0).Rows(0)("owner_portion_token_id")) And
                        pieces_seized = CInt(Dataset_nft_owner_portion_sell_order.Tables(0).Rows(0)("sell_order_pieces_seized")) And
                        confirmed_type = CStr(Dataset_nft_owner_portion_sell_order.Tables(0).Rows(0)("sell_order_confirmed_type")) And
                        price_piece = CDec(Dataset_nft_owner_portion_sell_order.Tables(0).Rows(0)("sell_order_price_piece")) And
                        currency = CStr(Dataset_nft_owner_portion_sell_order.Tables(0).Rows(0)("sell_order_currency")) And
                        max_split = CInt(Dataset_nft_owner_portion_sell_order.Tables(0).Rows(0)("owner_portion_max_split")) And
                        Regex.Replace(eoa_seller, "^0x", "") = CStr(Dataset_nft_owner_portion_sell_order.Tables(0).Rows(0)("sell_order_eoa")) And
                        Regex.Replace(eoa_recaller, "^0x", "") = CStr(Dataset_nft_owner_portion_sell_order.Tables(0).Rows(0)("owner_portion_eoa")) And
                        Regex.Replace(na_seller, "^0x", "") = CStr(Dataset_nft_owner_portion_sell_order.Tables(0).Rows(0)("sell_order_na")) And
                        exchange_name_seller = CStr(Dataset_nft_owner_portion_sell_order.Tables(0).Rows(0)("sell_order_exchange_name")) And
                        domain_seller = CStr(Dataset_nft_owner_portion_sell_order.Tables(0).Rows(0)("sell_order_domain")) And
                        ip_seller = CStr(Dataset_nft_owner_portion_sell_order.Tables(0).Rows(0)("sell_order_ip")) And
                        port_seller = CInt(Dataset_nft_owner_portion_sell_order.Tables(0).Rows(0)("sell_order_port")) And
                        Regex.Replace(na_recaller, "^0x", "") = CStr(Dataset_manager_node.Tables(0).Rows(0)("na")) And
                        exchange_name_recaller = CStr(Dataset_manager_node.Tables(0).Rows(0)("exchange_name")) And
                        domain_recaller = CStr(Dataset_manager_node_thread.Tables(0).Rows(0)("domain")) And
                        ip_recaller = CStr(Dataset_manager_node_thread.Tables(0).Rows(0)("ip")) And
                        port_recaller = CInt(Dataset_manager_node_thread.Tables(0).Rows(0)("port")) And
                        days_span = CInt(Dataset_nft_owner_portion_sell_order.Tables(0).Rows(0)("sell_order_days_span")) And
                        closing_time = CDate(CType(Dataset_nft_owner_portion_sell_order.Tables(0).Rows(0)("sell_order_closing_time"), MySqlDateTime)) And
                        transaction_fee_rate = CSng(Dataset_nft_owner_portion_sell_order.Tables(0).Rows(0)("sell_order_transaction_fee_rate")) Then

                        JRS = GRT.make_json_string.exe({{"success", "success", "quot"}}, {{"pieces_seized", CStr(pieces_seized), "non_quot"}}, False)

                    Else
                        JRS = GRT.make_json_string.exe({{"success", "fail", "quot"}}, {{"reason", "nft_submit_recall_info_not_match", "quot"}}, False)
                    End If

                Else
                    JRS = GRT.make_json_string.exe({{"success", "fail", "quot"}}, {{"reason", "nft_submit_buy_info_rows_node_not_exist", "quot"}}, False)
                End If

            Else
                JRS = GRT.make_json_string.exe({{"success", "fail", "quot"}}, {{"reason", "nft_submit_buy_info_node_not_found", "quot"}}, False)
            End If

        Else
            JRS = GRT.make_json_string.exe({{"success", "fail", "quot"}}, {{"reason", "nft_submit_recall_info_sell_order_not_found", "quot"}}, False)
        End If

        Return CType(JsonConvert.DeserializeObject(JRS), Linq.JObject)

    End Function

    Public Shared Function nft_confirm(json As Newtonsoft.Json.Linq.JObject) As Newtonsoft.Json.Linq.JObject

        Dim Selectcmd As MySqlCommand
        Dim Adapter As MySqlDataAdapter
        Dim Dataset_nft_owner_portion_sell_order, Dataset_manager_node, Dataset_manager_node_thread As DataSet

        Dim JRS As String

        Dim owner_portion_block_number = CLng(json("value")("owner_portion_block_number"))
        Dim sell_order_block_number = CLng(json("value")("sell_order_block_number"))
        Dim owner_portion_parent_block_number = CLng(json("value")("owner_portion_parent_block_number"))
        Dim nfa = CStr(json("value")("nfa"))
        Dim token_id = CInt(json("value")("token_id"))
        Dim pieces_to_confirm = CInt(json("value")("pieces_to_confirm"))
        Dim pieces_seized = CInt(json("value")("pieces_seized"))
        Dim confirmed_type = CStr(json("value")("confirmed_type"))
        Dim price_piece = CDec(json("value")("price_piece"))
        Dim price_sum = CDec(json("value")("price_sum"))

        Dim nonce = CLng(json("value")("nonce"))
        Dim gasPrice_Gwei = CDec(json("value")("gasPrice_Gwei"))
        Dim gasLimit = CDec(json("value")("gasLimit"))

        Dim currency = CStr(json("value")("currency"))
        Dim max_split = CInt(json("value")("max_split"))
        Dim eoa_seller = CStr(json("value")("eoa_seller"))
        Dim eoa_confirmer = CStr(json("value")("eoa_confirmer"))
        Dim na_seller = CStr(json("value")("na_seller"))
        Dim exchange_name_seller = CStr(json("value")("exchange_name_seller"))
        Dim domain_seller = CStr(json("value")("domain_seller"))
        Dim ip_seller = CStr(json("value")("ip_seller"))
        Dim port_seller = CInt(json("value")("port_seller"))
        Dim na_confirmer = CStr(json("value")("na_confirmer"))
        Dim exchange_name_confirmer = CStr(json("value")("exchange_name_confirmer"))
        Dim domain_confirmer = CStr(json("value")("domain_confirmer"))
        Dim ip_confirmer = CStr(json("value")("ip_confirmer"))
        Dim port_confirmer = CInt(json("value")("port_confirmer"))
        Dim days_span = CInt(json("value")("days_span"))
        Dim closing_time = CDate(json("value")("closing_time"))
        Dim transaction_fee_rate = CSng(json("value")("transaction_fee_rate"))
        Dim state = CStr(json("value")("state"))

        Dim idate_string = CStr(json("value")("idate_string"))
        Dim signiture_data_for_ethereum_transfer = CStr(json("value")("signiture_data_for_ethereum_transfer"))
        Dim signiture_data_for_ethereum_transfer_cancel = CStr(json("value")("signiture_data_for_ethereum_transfer_cancel"))
        Dim transfer_ethereum_from_exchange_case = CBool(json("value")("transfer_ethereum_from_exchange_case"))
        Dim pure_query = CStr(json("value")("pure_query"))
        Dim signiture = CStr(json("value")("signiture"))
        Dim signiture_key = CStr(json("value")("signiture_key"))
        Dim pure_query_for_ethereum_transfer_not_succeeded_and_no_need_cancel_transfer = CStr(json("value")("pure_query_for_ethereum_transfer_not_succeeded_and_no_need_cancel_transfer"))
        Dim signiture_for_ethereum_transfer_not_succeeded_and_no_need_cancel_transfer = CStr(json("value")("signiture_for_ethereum_transfer_not_succeeded_and_no_need_cancel_transfer"))

        Dim Connection_mariadb_local_bc_nft As New MySqlConnection(GRT.GR.cString_mariadb_local_bc_nft)

        Connection_mariadb_local_bc_nft.Open()

        'CREATE PROCEDURE `up_select_nft_owner_portion_sell_order_for_one_block_number`
        '(IN p_owner_portion_sell_order_block_number bigint(20))
        'BEGIN
        'SELECT * FROM owner_portion_sell_order
        'WHERE owner_portion_block_number = p_owner_portion_sell_order_block_number;
        'END

        Selectcmd = New MySqlCommand("up_select_nft_owner_portion_sell_order_for_one_block_number", Connection_mariadb_local_bc_nft)
        Selectcmd.CommandType = CommandType.StoredProcedure
        Selectcmd.Parameters.Add(New MySqlParameter("p_owner_portion_sell_order_block_number", owner_portion_block_number))

        Adapter = New MySqlDataAdapter
        Adapter.SelectCommand = Selectcmd
        Dataset_nft_owner_portion_sell_order = New DataSet
        Adapter.Fill(Dataset_nft_owner_portion_sell_order)

        Connection_mariadb_local_bc_nft.Close()

        If Dataset_nft_owner_portion_sell_order.Tables(0).Rows.Count = 1 Then

            Dim na_buyer_for_verify = GRT.get_node_from_eoa.exe(eoa_confirmer)

            If Not na_buyer_for_verify = "" Then

                Dim Connection_mariadb_local_bc_manager As New MySqlConnection(GRT.GR.cString_mariadb_local_bc_manager)

                Connection_mariadb_local_bc_manager.Open()

                'CREATE PROCEDURE `up_select_node_info`
                '(IN p_na char(40))
                'BEGIN
                'SELECT * FROM node
                'WHERE na = p_na;
                'END

                Selectcmd = New MySqlCommand("up_select_node_info", Connection_mariadb_local_bc_manager)
                Selectcmd.Parameters.Add(New MySqlParameter("p_na", Regex.Replace(na_buyer_for_verify, "^0x", "")))

                Adapter = New MySqlDataAdapter
                Selectcmd.CommandType = CommandType.StoredProcedure
                Adapter.SelectCommand = Selectcmd
                Dataset_manager_node = New DataSet
                Adapter.Fill(Dataset_manager_node)

                'CREATE PROCEDURE `up_select_node_thread_agency_info`
                '(IN p_na char(40))
                'BEGIN
                'SELECT * FROM node_thread
                'WHERE `na` = p_na AND `type` = 'agency';
                'END

                Selectcmd = New MySqlCommand("up_select_node_thread_agency_info", Connection_mariadb_local_bc_manager)
                Selectcmd.Parameters.Add(New MySqlParameter("p_na", Regex.Replace(na_buyer_for_verify, "^0x", "")))

                Adapter = New MySqlDataAdapter
                Selectcmd.CommandType = CommandType.StoredProcedure
                Adapter.SelectCommand = Selectcmd
                Dataset_manager_node_thread = New DataSet
                Adapter.Fill(Dataset_manager_node_thread)

                If Dataset_manager_node.Tables(0).Rows.Count = 1 And Dataset_manager_node_thread.Tables(0).Rows.Count = 1 Then

                    If sell_order_block_number = CLng(Dataset_nft_owner_portion_sell_order.Tables(0).Rows(0)("sell_order_block_number")) And
                        owner_portion_parent_block_number = CLng(Dataset_nft_owner_portion_sell_order.Tables(0).Rows(0)("owner_portion_parent_block_number")) And
                        Regex.Replace(nfa, "^0x", "") = CStr(Dataset_nft_owner_portion_sell_order.Tables(0).Rows(0)("owner_portion_nfa")) And
                        token_id = CInt(Dataset_nft_owner_portion_sell_order.Tables(0).Rows(0)("owner_portion_token_id")) And
                        pieces_seized = CInt(Dataset_nft_owner_portion_sell_order.Tables(0).Rows(0)("sell_order_pieces_seized")) And
                        confirmed_type = CStr(Dataset_nft_owner_portion_sell_order.Tables(0).Rows(0)("sell_order_confirmed_type")) And
                        price_piece = CDec(Dataset_nft_owner_portion_sell_order.Tables(0).Rows(0)("sell_order_price_piece")) And
                        currency = CStr(Dataset_nft_owner_portion_sell_order.Tables(0).Rows(0)("sell_order_currency")) And
                        max_split = CInt(Dataset_nft_owner_portion_sell_order.Tables(0).Rows(0)("owner_portion_max_split")) And
                        Regex.Replace(eoa_seller, "^0x", "") = CStr(Dataset_nft_owner_portion_sell_order.Tables(0).Rows(0)("sell_order_eoa")) And
                        Regex.Replace(eoa_confirmer, "^0x", "") = CStr(Dataset_nft_owner_portion_sell_order.Tables(0).Rows(0)("owner_portion_eoa")) And
                        Regex.Replace(na_seller, "^0x", "") = CStr(Dataset_nft_owner_portion_sell_order.Tables(0).Rows(0)("sell_order_na")) And
                        exchange_name_seller = CStr(Dataset_nft_owner_portion_sell_order.Tables(0).Rows(0)("sell_order_exchange_name")) And
                        domain_seller = CStr(Dataset_nft_owner_portion_sell_order.Tables(0).Rows(0)("sell_order_domain")) And
                        ip_seller = CStr(Dataset_nft_owner_portion_sell_order.Tables(0).Rows(0)("sell_order_ip")) And
                        port_seller = CInt(Dataset_nft_owner_portion_sell_order.Tables(0).Rows(0)("sell_order_port")) And
                        Regex.Replace(na_confirmer, "^0x", "") = CStr(Dataset_manager_node.Tables(0).Rows(0)("na")) And
                        exchange_name_confirmer = CStr(Dataset_manager_node.Tables(0).Rows(0)("exchange_name")) And
                        domain_confirmer = CStr(Dataset_manager_node_thread.Tables(0).Rows(0)("domain")) And
                        ip_confirmer = CStr(Dataset_manager_node_thread.Tables(0).Rows(0)("ip")) And
                        port_confirmer = CInt(Dataset_manager_node_thread.Tables(0).Rows(0)("port")) And
                        days_span = CInt(Dataset_nft_owner_portion_sell_order.Tables(0).Rows(0)("sell_order_days_span")) And
                        closing_time = CDate(CType(Dataset_nft_owner_portion_sell_order.Tables(0).Rows(0)("sell_order_closing_time"), MySqlDateTime)) And
                        transaction_fee_rate = CSng(Dataset_nft_owner_portion_sell_order.Tables(0).Rows(0)("sell_order_transaction_fee_rate")) Then

                        JRS = GRT.make_json_string.exe({{"success", "success", "quot"}}, {{"pieces_seized", CStr(pieces_seized), "non_quot"}}, False)

                    Else
                        JRS = GRT.make_json_string.exe({{"success", "fail", "quot"}}, {{"reason", "nft_submit_confirm_info_not_match", "quot"}}, False)
                    End If

                Else
                    JRS = GRT.make_json_string.exe({{"success", "fail", "quot"}}, {{"reason", "nft_submit_buy_info_rows_node_not_exist", "quot"}}, False)
                End If

            Else
                JRS = GRT.make_json_string.exe({{"success", "fail", "quot"}}, {{"reason", "nft_submit_buy_info_node_not_found", "quot"}}, False)
            End If

        Else
            JRS = GRT.make_json_string.exe({{"success", "fail", "quot"}}, {{"reason", "nft_submit_confirm_info_sell_order_not_found", "quot"}}, False)
        End If

        Return CType(JsonConvert.DeserializeObject(JRS), Linq.JObject)

    End Function

End Class
