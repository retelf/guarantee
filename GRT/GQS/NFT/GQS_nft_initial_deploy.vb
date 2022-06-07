Public Class GQS_nft_initial_deploy

    Public Shared Function exe(
                            new_nfa As Boolean,
                            nfa As String,
                            eoa As String,
                            name As String,
                            sub_name As String,
                            sub_creator As String,
                            personal_name As String,
                            character As String,
                            nft_type As String,
                            price_piece As Decimal,
                            price_total As Decimal,
                            file_extension As String,
                            file_length As Long,
                            sell_order_right_now As Boolean,
                            pieces As Integer,
                            general_terms As String,
                            individual_terms As String,
                            token_id As Integer,
                            url As String,
                            na As String,
                            exchange_name As String,
                            seller_agency_domain As String,
                            seller_agency_ip As String,
                            seller_agency_port As Integer,
                            days_span As Integer,
                            closing_time_utc_string As String,
                            splitable As Boolean,
                            max_split As Integer,
                            file_copiable As Boolean,
                            materialable As Boolean,
                            max_material_count As Integer,
                            min_price As Decimal,
                            terms_modifiable As Boolean,
                            copyright_transfer As Boolean,
                            pollable As Boolean,
                            quorum_proposal As Single,
                            quorum_conference As Single,
                            quorum_resolution As Single,
                            poll_notice_days_span As Integer,
                            poll_days_span As Integer,
                            idate_string As String,
                            creator_profile As String,
                            sub_creator_profile As String,
                            main_description As String,
                            sub_description As String,
                            name_critic As String,
                            critic As String) As String

        Dim pure_query As String

        general_terms = Regex.Replace(general_terms, "'", "\'")
        individual_terms = Regex.Replace(individual_terms, "'", "\'")

        If new_nfa Then

            pure_query = "USE bc_nft;" &
                "INSERT INTO account(" &
                "`block_number`," &
                " `nfa`," &
                " `creator`," &
                " `profile`," &
                " `sub_count`," &
                " `name`," &
                " `description`," &
                " `terms`," &
                " `idate`)" &
                "VALUES(" &
                " 0," &
                " '" & Regex.Replace(nfa, "^0x", "") & "'," &
                " '" & Regex.Replace(eoa, "^0x", "") & "'," &
                " '" & creator_profile & "'," &
                " 1," &
                " '" & name & "'," &
                " '" & main_description & "'," &
                " '" & general_terms & "'," &
                " '" & idate_string & "');"

        Else

            pure_query = "USE bc_nft;" &
                "UPDATE account SET " &
                "`sub_count` = `sub_count` + 1 " &
                "WHERE `nfa` = '" & Regex.Replace(nfa, "^0x", "") & "'"

        End If

        pure_query &= "USE bc_nft;" &
            "INSERT INTO sub_account(" &
            "`block_number`," &
            " `nfa`," &
            " `token_id`," &
            " `url`," &
            " `sub_creator`," &
            " `personal_name`," &
            " `profile`," &
            " `sub_name`," &
            " `character`," &
            " `description`," &
            " `name_critic`," &
            " `critic`," &
            " `sub_terms`," &
            " `balance`," &
            " `nft_type`," & ' 일반(순수파일), 순수실물, 실물파일
            " `file_extension`," &
            " `file_length`," &
            " `idate`)" &
            "VALUES(" &
            " 0," &
            " '" & Regex.Replace(nfa, "^0x", "") & "'," &
            " " & token_id & "," &
            " '" & url & "'," &
            " '" & Regex.Replace(sub_creator, "^0x", "") & "'," &
            " '" & Regex.Replace(personal_name, "^0x", "") & "'," &
            " '" & sub_creator_profile & "'," &
            " '" & sub_name & "'," &
            " '" & character & "'," &
            " '" & sub_description & "'," &
            " '" & name_critic & "'," &
            " '" & critic & "'," &
            " '" & individual_terms & "'," &
            " 0," &
            " '" & nft_type & "'," &
            " '" & file_extension & "'," &
            " " & file_length & "," &
            " '" & idate_string & "');"

        pure_query &= "USE bc_nft;" &
            "INSERT INTO owner_portion(" &
            "`block_number`," &
            "`parent_block_number`," &
            "`sell_order_block_number`," &
            " `nfa`," &
            " `token_id`," &
            " `eoa`," &
            " `pieces_confirmed`," &
            " `pieces_confirmed_ordered`," &
            " `pieces_unconfirmed`," &
            " `pieces_unconfirmed_ordered`," &
            " `max_split`," &
            " `closing_time`," &
            " `idate`)" &
            "VALUES(" &
            " 0," &
            " -1," &
            " -1," &
            " '" & Regex.Replace(nfa, "^0x", "") & "'," &
            " " & token_id & "," &
            " '" & Regex.Replace(eoa, "^0x", "") & "'," &
            " " & max_split & "," &
            " " & 0 & "," &
            " " & 0 & "," &
            " " & 0 & "," &
            " " & max_split & "," &
            " '" & closing_time_utc_string & "'," &
            " '" & idate_string & "');"

        pure_query &= "USE bc_nft;" &
            "INSERT INTO terms(" &
            "`block_number`," &
            "`nfa`," &
            "`token_id`," &
            "`splitable`," &
            "`max_split`," &
            "`file_copiable`," &
            "`materialable`," &
            "`max_material_count`," &
            "`min_price`," &
            "`terms_modifiable`," &
            "`copyright_transfer`," &
            "`pollable`," &
            "`quorum_proposal`," &
            "`quorum_conference`," &
            "`quorum_resolution`," &
            "`poll_notice_days_span`," &
            "`poll_days_span`)" &
            "VALUES(" &
            " 0," &
            " '" & Regex.Replace(nfa, "^0x", "") & "'," &
            " " & token_id & "," &
            " " & splitable & "," &
            " " & max_split & "," &
            " " & file_copiable & "," &
            " " & materialable & "," &
            " " & max_material_count & "," &
            " " & min_price & "," &
            " " & terms_modifiable & "," &
            " " & copyright_transfer & "," &
            " " & pollable & "," &
            " " & quorum_proposal & "," &
            " " & quorum_conference & "," &
            " " & quorum_resolution & "," &
            " " & poll_notice_days_span & "," &
            " " & poll_days_span & ");"

        If sell_order_right_now Then

            pure_query &= "USE bc_nft;" &
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
                " -1," &
                " '" & Regex.Replace(nfa, "^0x", "") & "'," &
                " " & token_id & "," &
                " " & pieces & "," &
                " " & 0 & "," &
                " " & 0 & "," &
                " " & 0 & "," &
                " " & pieces & "," &
                " '" & "confirmed" & "'," &
                " " & price_piece & "," &
                " " & price_total & "," &
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

        End If

        pure_query &= "USE bc_nft;" &
            "INSERT INTO history(" &
            "`block_number`," &
            "`nfa`," &
            "`token_id`," &
            "`sender`," &
            "`recipient`," &
            "`pieces`," &
            "`max_split`," &
            "`price`," &
            "`sum`," &
            "`idate`)" &
            "VALUES(" &
            " 0," &
            " '" & Regex.Replace(nfa, "^0x", "") & "'," &
            " " & token_id & "," &
            " '" & Regex.Replace(eoa, "^0x", "") & "'," &
            " '" & Regex.Replace(eoa, "^0x", "") & "'," &
            " " & pieces & "," &
            " " & max_split & "," &
            " " & price_total & "," &
            " " & price_total & "," &
            " '" & idate_string & "');"

        Return pure_query

    End Function

End Class




