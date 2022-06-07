Public Class GQS_nft_recall

    Public Shared Function exe(
                            owner_portion_block_number As Long,
                            sell_order_block_number As Long,
                            parent_block_number As Long,
                            nfa As String,
                            token_id As Integer,
                            seller As String,
                            recaller As String,
                            price_sum As Decimal,
                            auto_recall As Boolean,
                            currency As String,
                            gasPrice As Decimal,
                            gasLimit As Decimal,
                            pieces As Integer,
                            pieces_seized As Integer,
                            confirmed_type As String,
                            na_recaller As String,
                            na_seller As String,
                            days_span As Integer,
                            closing_time_utc_string As String,
                            transaction_fee_rate As Decimal,
                            max_split As Integer,
                            idate_string As String) As String

        ' 먼저 최초의 1개는 프로시저를 불러낼 것도 없이 pure_query 로만 실행한다.
        ' 하지만 그 다음부터는 while loop 로 처리한다.

        ' recaller 의 이더리움 Or 개런티 계좌 입금(수수료 없음). 단 가스비 발생. - 리콜러 random foo 사인을 바탕으로 리콜러 na 가 실시
        ' recall 시에는 가스비를 공제한 전액을 돌려줌.

        ' NFT 셀러의 ordered 를 confirmed 나 unconfirmed 로 복귀 - 이는 본래 셀러 na 가 해야 할 일이다.
        ' 그러나 그 자체만으로는 셀러의 이익으로 나타나는 것이기 때문에 그 인가를 얻을 필요는 없다. 오히려 손해를 입는 리콜러의 사인이 필요하다.

        Dim pure_query = ""

        Dim pieces_type = "pieces_" + confirmed_type
        Dim pieces_ordered_type = "pieces_" + confirmed_type + "_ordered"

        Dim price_to_increase, price_to_reduce As Decimal

        price_to_increase = price_sum ' recaller
        price_to_reduce = price_sum ' na_recaller

        If currency = "guarantee" Then

            pure_query &= "USE bc; UPDATE account SET `balance` = `balance` + " & price_to_increase & ", " & "idate = '" & idate_string & "' " &
            "WHERE `eoa` = '" & Regex.Replace(recaller, "^0x", "") & "' AND `coin_name` = '" & currency & "';"

            ' recaller_agency 거래소의 이더리움 Or 개런티 계좌 출금 - 리콜러가 자신의 이더리움 Or 개런티을 맡기는 것은 자신의 에이전시이어야 하기 때문. 이 처리행위도 recaller_agency 에서 하게 된다.

            pure_query &= "USE bc; UPDATE account SET `balance` = `balance` - " & price_to_reduce & ", `locked` = `locked` - " & price_to_reduce & " " &
            "WHERE `eoa` = '" & Regex.Replace(na_recaller, "^0x", "") & "' AND `coin_name` = '" & currency & "';"

        Else

            pure_query &= "USE bc; CALL up_check_and_insert_new_row_deposit('" & Regex.Replace(na_seller, "^0x", "") & "', '" & Regex.Replace(seller, "^0x", "") & "', '" & currency & "', '" & idate_string & "');"

            'CREATE PROCEDURE up_check_and_insert_new_row_deposit
            '(IN p_na char(40), 
            'IN p_depositor char(40), 
            'IN p_coin_name varchar(50), 
            'IN p_idate_string varchar(50))
            'BEGIN
            'DECLARE v_count int(11);
            'SELECT COUNT(*)
            'INTO v_count
            'FROM deposit
            'WHERE `na` = p_na AND `depositor` = p_depositor AND `coin_name` = p_coin_name;
            'IF v_count = 0 THEN
            '  INSERT INTO deposit (`block_number`, `na`, `depositor`, `coin_name`, `balance`, `idate`) VALUES (0, p_na, p_depositor, p_coin_name, 0, p_idate_string);
            'END IF;
            'END

            pure_query &= "USE bc; UPDATE deposit SET `balance` = `balance` + " & price_to_increase & ", " & "idate = '" & idate_string & "' " &
                "WHERE `na` = '" & Regex.Replace(na_recaller, "^0x", "") & "' AND `depositor` = '" & Regex.Replace(recaller, "^0x", "") & "' AND `coin_name` = '" & currency & "';"

        End If

        ' NFT 반납.

        ' owner_portion 에서 recaller 로우 변경

        pure_query &= "USE bc_nft; UPDATE owner_portion SET `pieces_unconfirmed` = `pieces_unconfirmed` - " & pieces & " " &
        "WHERE `block_number` = " & owner_portion_block_number & ";"

        ' seller 의 sell_order 에서 seized, recalled 변경.

        pure_query &= "USE bc_nft; UPDATE sell_order SET `pieces_seized` = `pieces_seized` - " & pieces & ", `pieces_recalled` = `pieces_recalled` + " & pieces & " WHERE `block_number` = " & sell_order_block_number & ";"

        ' seller 의 owner_portion 에서 pieces_type, ordered 변경.

        pure_query &= "USE bc_nft; UPDATE owner_portion SET `" & pieces_type & "` = `" & pieces_type & "` + " & pieces & ", `" & pieces_ordered_type & "` = `" & pieces_ordered_type & "` - " & pieces & " " &
        "WHERE `block_number` = " & parent_block_number & ";"

        ' auto_recall 일 때 연쇄

        If auto_recall Then

            pure_query &= "CALL up_loop_nft_recall(" & parent_block_number & ", " & pieces & ", '" & Regex.Replace(seller, "^0x", "") & "', '" & Regex.Replace(na_seller, "^0x", "") & "', '" & idate_string & "');"

            ' up_loop_nft_recall=================================================================================================================================================

            'CREATE PROCEDURE up_loop_nft_recall

            '(IN in_parent_block_number bigint(20),
            'IN in_pieces int(11),
            'IN in_recaller char(40), 
            'IN in_na_recaller char(40), 
            'IN in_idate_string varchar(50))

            'BEGIN

            'DECLARE out_parent_block_number bigint(20);
            'DECLARE out_auto_recall bool;
            'DECLARE out_currency varchar(50);
            'DECLARE out_seller char(40);
            'DECLARE out_na_seller char(40);

            'loop_nft_recall : WHILE True DO
            'CALL up_exe_nft_recall(in_parent_block_number, in_pieces, in_recaller, in_na_recaller, in_idate_string, out_parent_block_number, out_auto_recall, out_currency, out_seller, out_na_seller);

            '  SET in_parent_block_number = out_parent_block_number;
            '  SET in_recaller = out_seller;
            '  SET in_na_recaller = out_na_seller;
            'ELSE
            '  LEAVE loop_nft_recall;
            'END IF;
            'END WHILE loop_nft_recall;

            'END

            ' up_exe_nft_recall=================================================================================================================================================

            'CREATE PROCEDURE up_exe_nft_recall
            '(IN in_parent_block_number bigint(20),
            'IN in_pieces int(11),
            'IN in_recaller char(40),
            'IN in_na_recaller char(40),
            'IN in_idate_string varchar(50),
            'OUT out_parent_block_number bigint(20),
            'OUT out_auto_recall bool,
            'OUT out_currency varchar(50),
            'OUT out_seller char(40),
            'OUT out_na_seller char(40))

            'BEGIN
            'DECLARE v_owner_portion_block_number bigint(20);
            'DECLARE v_sell_order_block_number bigint(20);
            'DECLARE v_parent_block_number bigint(20);
            'DECLARE v_seller char(40);
            'DECLARE v_na_seller char(40);
            'DECLARE v_confirmed_type varchar(50);
            'DECLARE v_price_piece decimal(60,30);
            'DECLARE v_currency varchar(50);
            'DECLARE v_auto_recall bool;
            'DECLARE v_price_sum decimal(60,30);      

            'SELECT owner_portion_block_number, owner_portion_sell_order_block_number, owner_portion_parent_block_number, sell_order_confirmed_type, sell_order_price_piece, sell_order_currency, sell_order_auto_recall, sell_order_eoa, sell_order_na
            'INTO v_owner_portion_block_number, v_sell_order_block_number, v_parent_block_number, v_confirmed_type, v_price_piece, v_currency, v_auto_recall, v_seller, v_na_seller
            'FROM owner_portion_sell_order
            'WHERE `owner_portion_block_number` = in_parent_block_number;

            'SET v_price_sum = in_pieces * v_price_piece;

            'IF v_currency = 'guarantee' THEN

            '   UPDATE bc.account SET `balance` = `balance` + v_price_sum
            '   WHERE `eoa` = in_recaller And `coin_name` = v_currency;

            '   UPDATE bc.account SET `balance` = `balance` - v_price_sum, `locked` = `locked` - v_price_sum
            '   WHERE `eoa` = in_na_recaller And `coin_name` = v_currency;

            'ELSE

            '   CALL bc.up_check_and_insert_new_row_deposit(in_na_recaller, in_recaller, v_currency, in_idate_string);

            '   UPDATE bc.deposit SET `balance` = `balance` + v_price_sum, idate = in_idate_string 
            '   WHERE `na` = in_na_recaller AND `depositor` = in_recaller AND `coin_name` = v_currency;

            'END IF;

            'UPDATE bc_nft.owner_portion SET `pieces_unconfirmed` = `pieces_unconfirmed` - in_pieces
            'WHERE `block_number` = in_parent_block_number;

            'UPDATE bc_nft.sell_order SET `pieces_seized` = `pieces_seized` - in_pieces, `pieces_recalled` = `pieces_recalled` + in_pieces
            'WHERE `block_number` = v_sell_order_block_number;        

            'IF v_confirmed_type = 'confirmed' THEN

            '  UPDATE bc_nft.owner_portion SET `pieces_confirmed` = `pieces_confirmed` + in_pieces, `pieces_confirmed_ordered` = `pieces_confirmed_ordered` - in_pieces
            '  WHERE `block_number` = v_parent_block_number;

            'ELSE

            '  UPDATE bc_nft.owner_portion SET `pieces_unconfirmed` = `pieces_unconfirmed` + in_pieces, `pieces_unconfirmed_ordered` = `pieces_unconfirmed_ordered` - in_pieces
            '  WHERE `block_number` = v_parent_block_number;

            'END IF;

            'SET out_parent_block_number = v_parent_block_number;
            'SET out_auto_recall = v_auto_recall;
            'SET out_currency = v_currency;
            'SET out_seller = v_seller;
            'SET out_na_seller = v_na_seller;

            'END

            ' up_exe_nft_recall=================================================================================================================================================

        End If

        pure_query &= "USE bc_nft; UPDATE sell_order SET `state_open` = 1 WHERE `block_number` = " & sell_order_block_number & ";" ' 오직 최초의 경우만 언락한다. 락 역시 최초의 것만 했으므로.

        Return pure_query

    End Function

    Public Shared Function exe_ethereum_transfer_not_succeeded_and_no_need_cancel_transfer(
                                                                                            owner_portion_block_number As Long,
                                                                                            sell_order_block_number As Long,
                                                                                            parent_block_number As Long,
                                                                                            nfa As String,
                                                                                            token_id As Integer,
                                                                                            seller As String,
                                                                                            recaller As String,
                                                                                            price_sum As Decimal,
                                                                                            auto_recall As Boolean,
                                                                                            currency As String,
                                                                                            gasPrice As Decimal,
                                                                                            gasLimit As Decimal,
                                                                                            pieces As Integer,
                                                                                            pieces_seized As Integer,
                                                                                            confirmed_type As String,
                                                                                            na_recaller As String,
                                                                                            na_seller As String,
                                                                                            days_span As Integer,
                                                                                            closing_time_utc_string As String,
                                                                                            transaction_fee_rate As Decimal,
                                                                                            max_split As Integer,
                                                                                            idate_string As String) As String

        ' 이더리움 송금이 실패 내지 불분명한 경우인데 이 때 NFT 반납을 할 것인가의 문제이다. 또한 연쇄를 일으킬 것인가, 캔슬을 할 것인가 말 것인가.
        ' 이더리움은 리콜러의 거래소에 보관 중이기 때문에 리콜러에게는 안전하다.
        ' 나아가 auto_recall 연쇄는 반드시 일으켜 주어야 한다. 따라서 NFT 는 무조건 반납한다.
        ' 결국 추후 이더리움을 리콜러에게 별도 조사를 통하여 recaller_na 가 지급을 하는 부분만 제외하면 나머지는 똑같다.
        ' 즉 pure_query 에서 이더리움 송금처리만 하지 않으면 된다.
        ' 나아가 애당초 cancel 을 하지 말아야 한다. 그런데 이것은 분명하지 않다. 가스비 문제만 아니라면 캔슬을 해 주어 명확하게 하는 것이 좋다.
        ' 일반 송금의 경우는 캔슬이 맞다. 그러나 환전의 경우는 복잡해진다. 
        ' multilevel 의 경우 buy 의 경우 캔슬이 맞다. recall 의 경우는 캔슬할 이유가 없다.
        ' NFT의 경우 buy 시에는 캔슬이 필요하다. 리콜의 경우는 오토리콜일 때 절대 캔슬을 하면 안된다. 반면 오토리콜이 아니면 캔슬을 해 준다.
        ' 그러나 이상의 부분은 추후 다른 contract 형태까지 모두 전체적으로 다시 정리해야 하는 부분이다.

        Dim pure_query = ""

        Dim pieces_type = "pieces_" & confirmed_type
        Dim pieces_ordered_type = "pieces_" & confirmed_type & "_ordered"

        ' NFT 반납.

        ' owner_portion 에서 recaller 로우 변경

        pure_query &= "USE bc_nft; UPDATE owner_portion SET `pieces_unconfirmed` = `pieces_unconfirmed` - " & pieces & " " &
        "WHERE `block_number` = " & owner_portion_block_number & ";"

        ' seller 의 sell_order 에서 seized, recalled 변경.

        pure_query &= "USE bc_nft; UPDATE sell_order SET `pieces_seized` = `pieces_seized` - " & pieces & ", `pieces_recalled` = `pieces_recalled` + " & pieces & " WHERE `block_number` = " & sell_order_block_number & ";"

        ' seller 의 owner_portion 에서 pieces_type. ordered 변경.

        pure_query &= "USE bc_nft; UPDATE owner_portion SET `" & pieces_type & "` = `" & pieces_type & "` + " & pieces & ", `" & pieces_ordered_type & "` = `" & pieces_ordered_type & "` - " & pieces & " " &
        "WHERE `block_number` = " & parent_block_number & ";"

        pure_query &= "USE bc_nft; UPDATE sell_order SET `state_open` = 1 WHERE `block_number` = " & sell_order_block_number & ";"

        If auto_recall And currency = "guarantee" Then

            pure_query &= "CALL up_loop_nft_recall(" & parent_block_number & ", " & pieces & ", '" & Regex.Replace(seller, "^0x", "") & "', '" & Regex.Replace(na_seller, "^0x", "") & "', '" & idate_string & "');"

        End If

        Return pure_query

    End Function

End Class




