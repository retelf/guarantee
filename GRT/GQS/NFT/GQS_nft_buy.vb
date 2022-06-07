Public Class GQS_nft_buy

    Public Shared Function exe(
                            sell_order_block_number As Long,
                            parent_block_number As Long,
                            nfa As String,
                            token_id As Integer,
                            seller As String,
                            buyer As String,
                            price As Decimal,
                            currency As String,
                            gasPrice As Decimal,
                            gasLimit As Decimal,
                            pieces_to_buy As Integer,
                            confirmed_type As String,
                            na_buyer As String,
                            days_span As Integer,
                            closing_time_utc_string As String,
                            transaction_fee_rate As Decimal,
                            max_split As Integer,
                            idate_string As String) As String

        Dim pure_query = ""

        Dim pieces_type = "pieces_" + confirmed_type
        Dim pieces_ordered_type = "pieces_" + confirmed_type + "_ordered"

        Dim transaction_fee = CDec(CDbl(price * transaction_fee_rate))

        Dim price_to_increase, price_to_reduce As Decimal

        If currency = "guarantee" Then
            price_to_increase = price + transaction_fee
            price_to_reduce = price + transaction_fee
        Else
            price_to_increase = price + transaction_fee
            price_to_reduce = price + transaction_fee + (gasPrice * gasLimit) / 1000000000
        End If

        ' 바이어(clicker)의 계좌 출금

        pure_query &= "USE bc; UPDATE account SET `balance` = `balance` - " & price_to_reduce & ", " & "idate = '" & idate_string & "' " &
            "WHERE `eoa` = '" & Regex.Replace(buyer, "^0x", "") & "' AND `coin_name` = '" & currency & "';"

        ' 바이어의 거래소 계좌에 입금

        pure_query &= "USE bc; UPDATE account SET `balance` = `balance` + " & price_to_increase & ", `locked` = `locked` + " & price_to_increase & ", " & "idate = '" & idate_string & "' " &
            "WHERE `eoa` = '" & Regex.Replace(na_buyer, "^0x", "") & "' AND `coin_name` = '" & currency & "';"

        ' NFT 이전

        ' 바이어  

        ' 새로운 owner_portion 로우 생성. 이는 무조건이다.
        ' 일단 uncomfirmed 로 생성시킨다. 이 상태에서 order 를 하지 않고 추후 확정이 되면 그 때 comfirmed 로 이전한다.
        ' uncomfirmed 상태에서 다시 order 를 하면 uncomfirmed 를 감소시키고 uncomfirmed_ordered 를 증가시킨다.
        ' 추후 recall 이 되면 이 경우 가장 최초의 pieces 만 comfirmed 이며 그 이후는 uncomfirmed 이므로 그에 일치하게 원본으로 복귀한다. 
        ' recall 은 자동연쇄방식과 비연쇄방식이 있다.
        ' recall 가능기간은 각 order 때마다 예비시간을 포함하여 스스로의 책임으로 책정한다.

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
            " " & parent_block_number & "," &
            " " & sell_order_block_number & "," &
            " '" & Regex.Replace(nfa, "^0x", "") & "'," &
            " " & token_id & "," &
            " '" & Regex.Replace(buyer, "^0x", "") & "'," &
            " " & 0 & "," &
            " " & 0 & "," &
            " " & pieces_to_buy & "," &
            " " & 0 & "," &
            " " & max_split & "," &
            " '" & closing_time_utc_string & "'," &
            " '" & idate_string & "');"

        ' sell_order 테이블 상태변경

        pure_query &= "USE bc_nft; UPDATE sell_order SET `pieces_alive` = `pieces_alive` - " & pieces_to_buy & ", `pieces_seized` = `pieces_seized` + " & pieces_to_buy & " WHERE `block_number` = " & sell_order_block_number & ";"

        pure_query &= "USE bc_nft; UPDATE sell_order SET `state_open` = 1 WHERE `block_number` = " & sell_order_block_number & ";"

        Return pure_query

    End Function

End Class




