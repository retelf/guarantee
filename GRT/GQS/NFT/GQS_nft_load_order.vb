Public Class GQS_nft_load_order

    Public Shared Function exe(
                            owner_portion_block_number As Long,
                            nfa As String,
                            eoa As String,
                            price_piece As Decimal,
                            price_total As Decimal,
                            auto_recall As Boolean,
                            currency As String,
                            sell_pieces As Integer,
                            confirmed_type As String,
                            token_id As Integer,
                            na As String,
                            exchange_name As String,
                            seller_agency_domain As String,
                            seller_agency_ip As String,
                            seller_agency_port As Integer,
                            days_span As Integer,
                            closing_time_utc_string As String,
                            max_split As Integer,
                            idate_string As String) As String

        Dim pure_query As String =
            "USE bc_nft;" &
            "INSERT INTO sell_order(" &
            "`block_number`," &
            "`owner_portion_block_number`," &
            " `nfa`," &
            " `token_id`," &
            " `pieces_alive`," &
            " `pieces_seized`," &
            " `pieces_recalled`," &
            " `pieces_completed`," &
            " `total_pieces`," &
            " `confirmed_type`," &
            " `price_piece`," &
            " `price_total`," &
            " `auto_recall`," &
            " `currency`," &
            " `max_split`," &
            " `eoa`," &
            " `na`," &
            " `exchange_name`," &
            " `domain`," &
            " `ip`," &
            " `port`," &
            " `days_span`," &
            " `closing_time`," &
            " `transaction_fee_rate`," &
            " `state_open`," &
            " `idate`)" &
            "VALUES(" &
            " 0," &
            " " & owner_portion_block_number & "," &
            " '" & Regex.Replace(nfa, "^0x", "") & "'," &
            " " & token_id & "," &
            " " & sell_pieces & "," &
            " " & 0 & "," &
            " " & 0 & "," &
            " " & 0 & "," &
            " " & sell_pieces & "," &
            " '" & confirmed_type & "'," &
            " " & price_piece & "," &
            " " & price_total & "," &
            " " & auto_recall & "," &
            " '" & currency & "'," &
            " " & max_split & "," &
            " '" & Regex.Replace(eoa, "^0x", "") & "'," &
            " '" & Regex.Replace(na, "^0x", "") & "'," &
            " '" & exchange_name & "'," &
            " '" & seller_agency_domain & "'," &
            " '" & seller_agency_ip & "'," &
            " " & seller_agency_port & "," &
            " " & days_span & "," &
            " '" & closing_time_utc_string & "'," &
            " " & GRT.GR.nft_transaction_fee_rate & "," &
            " 1," &
            " '" & idate_string & "');"

        Dim pieces_type = "pieces_" & confirmed_type
        Dim pieces_ordered_type = "pieces_" & confirmed_type & "_ordered"

        pure_query &= "USE bc_nft; UPDATE owner_portion SET " &
            "`" & pieces_type & "` = `" & pieces_type & "` - " & sell_pieces & ", " &
            "`" & pieces_ordered_type & "` = `" & pieces_ordered_type & "` + " & sell_pieces & " " &
            "WHERE `block_number` = " & owner_portion_block_number & " AND `nfa` = '" & Regex.Replace(nfa, "^0x", "") & "' AND `eoa` = '" & Regex.Replace(eoa, "^0x", "") & "' AND `token_id` = " & token_id & ";"

        Return pure_query

    End Function

End Class




