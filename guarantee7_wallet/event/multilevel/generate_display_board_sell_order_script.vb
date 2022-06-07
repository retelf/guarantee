Public Class generate_display_board_sell_order_script

    Public Shared Function exe(Dataset As DataSet) As String

        Dim table_string As String
        Dim block_number As Long
        Dim eoa, na, buyer, ma, exchange_name, domain, ip As String
        Dim port, days_span As Integer
        Dim exchange_rate, exchange_fee_rate As Decimal
        Dim state As String
        Dim closing_time_utc, closing_time_local As DateTime
        Dim button_string() As String

        table_string = "<table id='table_board' class='table_board'>"

        table_string &= "<tr id='tr_title'>"

        table_string &= "<td id='td_board_sell_order' class='td_board td_board_sell_order'><div class='div_title div_title_exchange'>" & "" & "</div></td>"
        table_string &= "<td id='td_board_block_number' class='td_board td_board_block_number'><div class='div_title div_title_block_number'>" & "BN" & "</div></td>"
        table_string &= "<td id='td_board_eoa' class='td_board td_board_eoa'><div class='div_title div_title_eoa'>" & "eoa" & "</div></td>"
        table_string &= "<td id='td_board_na' class='td_board td_board_na'><div class='div_title div_title_na'>" & "na" & "</div></td>"
        table_string &= "<td id='td_board_buyer' class='td_board td_board_buyer'><div class='div_title div_title_buyer'>" & "buyer" & "</div></td>"
        table_string &= "<td id='td_board_ma' class='td_board td_board_ma'><div class='div_title div_title_ma'>" & "ma" & "</div></td>"
        table_string &= "<td id='td_board_exchange_name' class='td_board td_board_exchange_name'><div class='div_title div_title_exchange_name'>" & "exchange_name" & "</div></td>"
        table_string &= "<td id='td_board_domain' class='td_board td_board_domain'><div class='div_title div_title_domain'>" & "domain" & "</div></td>"
        table_string &= "<td id='td_board_ip' class='td_board td_board_ip'><div class='div_title div_title_ip'>" & "ip" & "</div></td>"
        table_string &= "<td id='td_board_port' class='td_board td_board_port'><div class='div_title div_title_port'>" & "port" & "</div></td>"
        table_string &= "<td id='td_board_exchange_rate' class='td_board td_board_exchange_rate'><div class='div_title div_title_exchange_rate'>" & "ethereum" & "</div></td>"
        table_string &= "<td id='td_board_days_span' class='td_board td_board_days_span'><div class='div_title div_title_days_span'>" & "days_span" & "</div></td>"
        table_string &= "<td id='td_board_state' class='td_board td_board_state'><div class='div_title div_title_state'>" & "state" & "</div></td>"
        table_string &= "<td id='td_board_closing_time_local' class='td_board td_board_closing_time_local'><div class='div_title div_title_closing_time_local'>" & "closing_time_local" & "</div></td>"
        table_string &= "<td id='td_board_closing_time_utc' class='td_board td_board_closing_time_utc'><div class='div_title div_title_closing_time_utc'>" & "closing_time_utc" & "</div></td>"
        table_string &= "<td id='td_board_exchange_fee_rate' class='td_board td_board_exchange_fee_rate'><div class='div_title div_title_exchange_fee_rate'>" & "fee" & "</div></td>"
        table_string &= "<td id='td_board_refund' class='td_board td_board_refund'><div class='div_title div_title_refund'>" & "" & "</div></td>"

        For i = 0 To Dataset.Tables(0).Rows.Count - 1

            block_number = CLng(Dataset.Tables(0).Rows(i)("block_number"))
            eoa = "0x" & CStr(Dataset.Tables(0).Rows(i)("eoa"))
            na = "0x" & CStr(Dataset.Tables(0).Rows(i)("na"))

            If Not Dataset.Tables(0).Rows(i).IsNull("buyer") Then
                buyer = "0x" & CStr(Dataset.Tables(0).Rows(i)("buyer"))
            Else
                buyer = "/"
            End If

            If Not Dataset.Tables(0).Rows(i).IsNull("ma") Then
                ma = "0x" & CStr(Dataset.Tables(0).Rows(i)("ma"))
            Else
                ma = "/"
            End If

            exchange_name = CStr(Dataset.Tables(0).Rows(i)("exchange_name"))
            domain = CStr(Dataset.Tables(0).Rows(i)("domain"))
            ip = CStr(Dataset.Tables(0).Rows(i)("ip"))
            port = CInt(Dataset.Tables(0).Rows(i)("port"))
            exchange_rate = CDec(Dataset.Tables(0).Rows(i)("exchange_rate"))
            days_span = CInt(Dataset.Tables(0).Rows(i)("days_span"))
            state = CStr(Dataset.Tables(0).Rows(i)("state"))

            If Not Regex.Match(Dataset.Tables(0).Rows(i)("closing_time").ToString, "^0000").Success Then
                closing_time_utc = CDate(Dataset.Tables(0).Rows(i)("closing_time"))
                closing_time_local = closing_time_utc.ToLocalTime
            End If

            exchange_fee_rate = CDec(Dataset.Tables(0).Rows(i)("exchange_fee_rate"))

            button_string = get_button_string.exe(i, Regex.Replace(eoa, "^0x", ""), Regex.Replace(buyer, "^0x", ""), state)

            table_string &= "<tr id='tr_board_sell_" & i & "' class='tr_board tr_board_odd_even_" & i Mod 2 & " tr_board_sell'>"

            table_string &= "<td id='td_board_sell_buy' class='td_board td_board_buy'><div class='div_board div_board_buy'>" & button_string(0) & "</div></td>"
            table_string &= "<td id='td_board_sell_block_number' class='td_board td_board_block_number'><div class='div_board div_board_block_number'>" & block_number & "</div></td>"
            table_string &= "<td id='td_board_sell_eoa' class='td_board td_board_eoa'><div class='div_board div_board_eoa'>" & eoa & "</div></td>"
            table_string &= "<td id='td_board_sell_na' class='td_board td_board_na'><div class='div_board div_board_na'>" & na & "</div></td>"
            table_string &= "<td id='td_board_sell_buyer' class='td_board td_board_buyer'><div class='div_board div_board_buyer'>" & buyer & "</div></td>"
            table_string &= "<td id='td_board_sell_ma' class='td_board td_board_ma'><div class='div_board div_board_ma'>" & ma & "</div></td>"
            table_string &= "<td id='td_board_sell_exchange_name' class='td_board td_board_exchange_name'><div class='div_board div_board_exchange_name'>" & exchange_name & "</div></td>"
            table_string &= "<td id='td_board_sell_domain' class='td_board td_board_na'><div class='div_board div_board_domain'>" & domain & "</div></td>"
            table_string &= "<td id='td_board_sell_ip' class='td_board td_board_ip'><div class='div_board div_board_ip'>" & ip & "</div></td>"
            table_string &= "<td id='td_board_sell_port' class='td_board td_board_port'><div class='div_board div_board_port'>" & port & "</div></td>"
            table_string &= "<td id='td_board_sell_exchange_rate' class='td_board td_board_exchange_rate'><div class='div_board div_board_exchange_rate'>" & exchange_rate & "</div></td>"
            table_string &= "<td id='td_board_sell_days_span' class='td_board td_board_days_span'><div class='div_board div_board_days_span'>" & days_span & "</div></td>"
            table_string &= "<td id='td_board_sell_state' class='td_board td_board_state'><div class='div_board div_board_state'>" & state & "</div></td>"

            If Not Regex.Match(Dataset.Tables(0).Rows(i)("closing_time").ToString, "^0000").Success Then
                table_string &= "<td id='td_board_sell_closing_time_local' class='td_board td_board_closing_time_local'><div class='div_board div_board_closing_time_local'>" & closing_time_local.ToString("yyyy/MM/dd HH:mm:ss") & "</div></td>"
                table_string &= "<td id='td_board_sell_closing_time_utc' class='td_board td_board_closing_time_utc'><div class='div_board div_board_closing_time_utc'>" & closing_time_utc.ToString("yyyy/MM/dd HH:mm:ss") & "</div></td>"
            Else
                table_string &= "<td id='td_board_sell_closing_time_local' class='td_board td_board_closing_time_local'><div class='div_board div_board_closing_time_local'>/</div></td>"
                table_string &= "<td id='td_board_sell_closing_time_utc' class='td_board td_board_closing_time_utc'><div class='div_board div_board_closing_time_utc'>/</div></td>"
            End If

            table_string &= "<td id='td_board_sell_exchange_fee_rate' class='td_board td_board_exchange_fee_rate'><div class='div_board div_board_exchange_fee_rate'>" & exchange_fee_rate & "</div></td>"
            table_string &= "<td id='td_board_sell_refund' class='td_board td_board_refund'><div class='div_board div_board_refund'>" & button_string(1) & "</div></td>"

            table_string &= "</tr>"

        Next

        table_string &= "</table>"

        Return table_string

    End Function

End Class
