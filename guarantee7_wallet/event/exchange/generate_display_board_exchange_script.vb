Public Class generate_display_board_exchange_script

    Public Shared Function exe(Dataset As DataSet) As String

        Dim table_string As String
        Dim block_number As Long
        Dim eoa_orderer, na, domain, ip, coin_name_from, coin_name_to As String
        Dim port As Integer
        Dim amount, exchange_rate, exchange_fee_rate As Decimal
        Dim button_exchange_string, button_cancel_string As String

        table_string = "<table id='table_board' class='table_board'>"

        For i = 0 To Dataset.Tables(0).Rows.Count - 1

            block_number = CLng(Dataset.Tables(0).Rows(i)("block_number"))
            eoa_orderer = CStr(Dataset.Tables(0).Rows(i)("eoa"))
            na = CStr(Dataset.Tables(0).Rows(i)("na"))
            domain = CStr(Dataset.Tables(0).Rows(i)("domain"))
            ip = CStr(Dataset.Tables(0).Rows(i)("ip"))
            port = CInt(Dataset.Tables(0).Rows(i)("port"))
            coin_name_from = CStr(Dataset.Tables(0).Rows(i)("coin_name_from"))
            coin_name_to = CStr(Dataset.Tables(0).Rows(i)("coin_name_to"))
            amount = CDec(Dataset.Tables(0).Rows(i)("amount"))
            exchange_rate = CDec(Dataset.Tables(0).Rows(i)("exchange_rate"))
            exchange_fee_rate = CDec(Dataset.Tables(0).Rows(i)("exchange_fee_rate"))

            table_string &= "<tr id='tr_board_sell_" & i & "' class='tr_board tr_board_odd_even_" & i Mod 2 & " tr_board_sell'>"

            button_exchange_string = "<button id='btn_exchange_" & i & "' type='button' class='btn btn-primary btn-sm' onclick='c_submit_exchange.exe($(this))'>exchange</button>"
            button_cancel_string = ""

            table_string &= "<td id='td_board_sell_exchange' class='td_board td_board_exchange'><div class='div_board div_board_exchange'>" & button_exchange_string & "</div></td>"
            table_string &= "<td id='td_board_sell_block_number' class='td_board td_board_block_number'><div class='div_board div_board_block_number'>" & block_number & "</div></td>"
            table_string &= "<td id='td_board_sell_eoa' class='td_board td_board_eoa'><div class='div_board div_board_eoa'>" & "0x" & eoa_orderer & "</div></td>"
            table_string &= "<td id='td_board_sell_na' class='td_board td_board_na'><div class='div_board div_board_na'>" & "0x" & na & "</div></td>"
            table_string &= "<td id='td_board_sell_domain' class='td_board td_board_na'><div class='div_board div_board_domain'>" & domain & "</div></td>"
            table_string &= "<td id='td_board_sell_ip' class='td_board td_board_ip'><div class='div_board div_board_ip'>" & ip & "</div></td>"
            table_string &= "<td id='td_board_sell_port' class='td_board td_board_port'><div class='div_board div_board_port'>" & port & "</div></td>"
            table_string &= "<td id='td_board_sell_coin_name_from' class='td_board td_board_coin_name_from'><div class='div_board div_board_coin_name_from'>" & coin_name_from & "</div></td>"
            table_string &= "<td id='td_board_sell_coin_name_to' class='td_board td_board_coin_name_to'><div class='div_board div_board_coin_name_to'>" & coin_name_to & "</div></td>"
            table_string &= "<td id='td_board_sell_amount' class='td_board td_board_amount'><div class='div_board div_board_amount'>" & amount & "</div></td>"
            table_string &= "<td id='td_board_sell_exchange_rate' class='td_board td_board_exchange_rate'><div class='div_board div_board_exchange_rate'>" & exchange_rate & "</div></td>"
            table_string &= "<td id='td_board_sell_exchange_fee_rate' class='td_board td_board_exchange_fee_rate'><div class='div_board div_board_exchange_fee_rate'>" & exchange_fee_rate & "</div></td>"
            table_string &= "<td id='td_board_sell_cancel' class='td_board td_board_cancel'><div class='div_board div_board_cancel'>" & button_cancel_string & "</div></td>"

            table_string &= "</tr>"

        Next

        table_string &= "<tr id='tr_title'>"

        table_string &= "<td id='td_board_exchange' class='td_board td_board_exchange'><div class='div_title div_title_exchange'>" & "" & "</div></td>"
        table_string &= "<td id='td_board_block_number' class='td_board td_board_block_number'><div class='div_title div_title_block_number'>" & "BN" & "</div></td>"
        table_string &= "<td id='td_board_eoa' class='td_board td_board_eoa'><div class='div_title div_title_eoa'>" & "eoa" & "</div></td>"
        table_string &= "<td id='td_board_na' class='td_board td_board_na'><div class='div_title div_title_na'>" & "na" & "</div></td>"
        table_string &= "<td id='td_board_domain' class='td_board td_board_domain'><div class='div_title div_title_na'>" & "domain" & "</div></td>"
        table_string &= "<td id='td_board_ip' class='td_board td_board_ip'><div class='div_title div_title_ip'>" & "ip" & "</div></td>"
        table_string &= "<td id='td_board_port' class='td_board td_board_port'><div class='div_title div_title_port'>" & "port" & "</div></td>"
        table_string &= "<td id='td_board_coin_name_from' class='td_board td_board_coin_name_from'><div class='div_title div_title_coin_name_from'>" & "coin_name_from" & "</div></td>"
        table_string &= "<td id='td_board_coin_name_to' class='td_board td_board_coin_name_to'><div class='div_title div_title_coin_name_to'>" & "coin_name_to" & "</div></td>"
        table_string &= "<td id='td_board_amount' class='td_board td_board_amount'><div class='div_title div_title_amount'>" & "amount" & "</div></td>"
        table_string &= "<td id='td_board_exchange_rate' class='td_board td_board_exchange_rate'><div class='div_title div_title_exchange_rate'>" & "rate" & "</div></td>"
        table_string &= "<td id='td_board_exchange_fee_rate' class='td_board td_board_exchange_fee_rate'><div class='div_title div_title_exchange_fee_rate'>" & "fee" & "</div></td>"
        table_string &= "<td id='td_board_cancel' class='td_board td_board_cancel'><div class='div_title div_title_cancel'>" & "" & "</div></td>"

        table_string &= "</tr>"

        For i = 0 To Dataset.Tables(1).Rows.Count - 1

            block_number = CLng(Dataset.Tables(1).Rows(i)("block_number"))
            eoa_orderer = CStr(Dataset.Tables(1).Rows(i)("eoa"))
            na = CStr(Dataset.Tables(1).Rows(i)("na"))
            domain = CStr(Dataset.Tables(1).Rows(i)("domain"))
            ip = CStr(Dataset.Tables(1).Rows(i)("ip"))
            port = CInt(Dataset.Tables(1).Rows(i)("port"))
            coin_name_from = CStr(Dataset.Tables(1).Rows(i)("coin_name_from"))
            coin_name_to = CStr(Dataset.Tables(1).Rows(i)("coin_name_to"))
            amount = CDec(Dataset.Tables(1).Rows(i)("amount"))
            exchange_rate = CDec(Dataset.Tables(1).Rows(i)("exchange_rate"))
            exchange_fee_rate = CDec(Dataset.Tables(1).Rows(i)("exchange_fee_rate"))

            table_string &= "<tr id='tr_board_buy_" & i & "' class='tr_board tr_board_odd_even_" & i Mod 2 & " tr_board_buy'>"

            button_exchange_string = ""

            If eoa_orderer = Regex.Replace(GRT.GR.account.public_key, "^0x", "") Then
                button_cancel_string = "<button id='btn_cancel_" & i & "' type='button' class='btn btn-warning btn-sm' onclick='c_submit_exchange.exe($(this))'>cancel</button>"
            Else
                button_cancel_string = ""
            End If

            table_string &= "<td id='td_board_sell_exchange' class='td_board td_board_exchange'><div class='div_board div_board_exchange'>" & button_exchange_string & "</div></td>"
            table_string &= "<td id='td_board_buy_block_number' class='td_board td_board_block_number'><div class='div_board div_board_block_number'>" & block_number & "</div></td>"
            table_string &= "<td id='td_board_buy_eoa' class='td_board td_board_eoa'><div class='div_board div_board_eoa'>" & "0x" & eoa_orderer & "</div></td>"
            table_string &= "<td id='td_board_buy_na' class='td_board td_board_na'><div class='div_board div_board_na'>" & "0x" & na & "</div></td>"
            table_string &= "<td id='td_board_buy_domain' class='td_board td_board_na'><div class='div_board div_board_domain'>" & domain & "</div></td>"
            table_string &= "<td id='td_board_buy_ip' class='td_board td_board_ip'><div class='div_board div_board_ip'>" & ip & "</div></td>"
            table_string &= "<td id='td_board_buy_port' class='td_board td_board_port'><div class='div_board div_board_port'>" & port & "</div></td>"
            table_string &= "<td id='td_board_buy_coin_name_from' class='td_board td_board_coin_name_from'><div class='div_board div_board_coin_name_from'>" & coin_name_from & "</div></td>"
            table_string &= "<td id='td_board_buy_coin_name_to' class='td_board td_board_coin_name_to'><div class='div_board div_board_coin_name_to'>" & coin_name_to & "</div></td>"
            table_string &= "<td id='td_board_buy_amount' class='td_board td_board_amount'><div class='div_board div_board_amount'>" & amount & "</div></td>"
            table_string &= "<td id='td_board_buy_exchange_rate' class='td_board td_board_exchange_rate'><div class='div_board div_board_exchange_rate'>" & exchange_rate & "</div></td>"
            table_string &= "<td id='td_board_buy_exchange_fee_rate' class='td_board td_board_exchange_fee_rate'><div class='div_board div_board_exchange_fee_rate'>" & exchange_fee_rate & "</div></td>"
            table_string &= "<td id='td_board_sell_cancel' class='td_board td_board_cancel'><div class='div_board div_board_cancel'>" & button_cancel_string & "</div></td>"

            table_string &= "</tr>"

        Next

        table_string &= "</table>"

        Return table_string

    End Function

End Class
