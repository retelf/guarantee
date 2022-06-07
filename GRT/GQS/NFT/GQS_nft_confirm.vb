Public Class GQS_nft_confirm

    Public Shared Function exe(command_key As String,
                            owner_portion_block_number As Long,
                            sell_order_block_number As Long,
                            parent_block_number As Long,
                            seller As String,
                            price_sum As Decimal,
                            currency As String,
                            pieces As Integer,
                            confirmed_type As String,
                            na_confirmer As String,
                            na_seller As String,
                            idate_string As String) As String

        Dim exchange_left_right As GRT.get_exchange_clearing_data.data
        Dim pure_query = ""

        Dim pieces_type = "pieces_" + confirmed_type
        Dim pieces_ordered_type = "pieces_" + confirmed_type + "_ordered"

        Dim price_to_increase, price_to_reduce As Decimal

        price_to_increase = price_sum  ' confirmer
        price_to_reduce = price_sum ' na_confirmer

        If currency = "guarantee" Then

            ' 셀러의 개런티 계좌 입금.

            pure_query &= "USE bc; UPDATE account SET `balance` = `balance` + " & price_to_increase & ", " & "idate = '" & idate_string & "' " &
                "WHERE `eoa` = '" & Regex.Replace(seller, "^0x", "") & "' AND `coin_name` = '" & currency & "';"

            ' confirmer_agency 거래소의 개런티 계좌 출금 - 컨퍼머가 자신의 개런티를 맡기는 것은 자신의 에이전시이어야 하기 때문. 이 처리행위도 confirmer_agency 에서 하게 된다.

            pure_query &= "USE bc; UPDATE account SET `balance` = `balance` - " & price_to_reduce & ", `locked` = `locked` - " & price_to_reduce & " " &
                "WHERE `eoa` = '" & Regex.Replace(na_confirmer, "^0x", "") & "' AND `coin_name` = '" & currency & "';"

        Else

            ' 셀러의 이더리움 디퍼짓 계좌 입금. 이 처리행위도 confirmer_agency 에서 하게 되지만 입금은 seller 거래소에 하게 된다.
            ' 그런데 실제로 이더리움이 이전한 것은 아니므로 괴리가 발생하게 된다. 즉 na_confirmer 는 na_seller 에게 이더리움 빚을 지고 있는 셈이 된다.
            ' 이를 위해 어음교환소가 필요하게 된다. 어음교환소 계정에서 na_seller 에 대한 na_confirmer 의 이더리움 채무 표시를 해 준다.
            ' 이후 언제라도 na_seller 는 해당 액수 만큼의 출금을 할 수 있다.
            ' 자동으로 출금하는 manager 스레드를 사용하게 될 것이다.

            ' 가장 먼저 na_confirmer 의 사고 여부를 확인한다.

            Dim accident = check_node_accident.exe(na_confirmer)

            If Not accident Then

                ' 추후 사고가 나더라도 현재는 정상인 상태이므로 추후 clear 시점에서 사고가 나더라도 현 거래에 관한 한 책임은 seller_agency 가 진다. 따라서 지금 확정적으로 deposit 을 해준다.
                ' na_seller = na_confirmer 는 관계없다.

                pure_query &= "USE bc; CALL up_check_and_insert_new_row_deposit('" & Regex.Replace(na_seller, "^0x", "") & "', '" & Regex.Replace(seller, "^0x", "") & "', '" & currency & "', '" & idate_string & "');"

                pure_query &= "USE bc; UPDATE deposit SET `balance` = `balance` + " & price_to_increase & ", " & "idate = '" & idate_string & "' " &
                    "WHERE `na` = '" & Regex.Replace(na_seller, "^0x", "") & "' AND `depositor` = '" & Regex.Replace(seller, "^0x", "") & "' AND `coin_name` = '" & currency & "';"

                If Not Regex.Replace(na_seller, "^0x", "") = Regex.Replace(na_confirmer, "^0x", "") Then

                    exchange_left_right = GRT.get_exchange_clearing_data.exe(na_seller, na_confirmer, price_sum)

                    pure_query &= "USE bc; CALL up_check_and_insert_new_row_clearing_house('" & Regex.Replace(exchange_left_right.exchange_left, "^0x", "") & "', '" & Regex.Replace(exchange_left_right.exchange_right, "^0x", "") & "', '" & currency & "', '" & idate_string & "');"

                    'CREATE PROCEDURE `up_check_and_insert_new_row_clearing_house`
                    '(IN `p_exchange_left` CHAR(40),
                    'IN `p_exchange_right` CHAR(40),
                    'IN `p_coin_name` VARCHAR(50),
                    'IN `p_idate_string` VARCHAR(50))
                    'BEGIN
                    'DECLARE v_count int(11);
                    'SELECT COUNT(*)
                    'INTO v_count
                    'FROM clearing_house
                    'WHERE `exchange_left` = p_exchange_left And `exchange_right` = p_exchange_right And `coin_name` = p_coin_name;
                    'If v_count = 0 Then
                    'INSERT INTO clearing_house (`block_number`, `exchange_left`, `exchange_right`, `coin_name`, `balance`, `idate_last_transaction`, `idate_last_clearing`) VALUES (0, p_exchange_left, p_exchange_right, p_coin_name, 0, p_idate_string, p_idate_string);
                    'End If;
                    'End

                    pure_query &= "USE bc; UPDATE clearing_house SET `balance` = `balance` + (" & exchange_left_right.price_to_increase_decrease & "), `idate_last_transaction` = '" & idate_string & "' " &
                    "WHERE `exchange_left` = '" & Regex.Replace(exchange_left_right.exchange_left, "^0x", "") & "' AND `exchange_right` = '" & Regex.Replace(exchange_left_right.exchange_right, "^0x", "") & "' AND `coin_name` = '" & currency & "';"

                End If

                ' 이상의 처리를 통하여 confirmer 에 대한 lock 가 직전 na 에 대한 lock 로 그 성질이 변하게 된다. 이는 clear_exchange 시점에서 풀리게 된다.

            Else

                ' 이미 사고가 존재하는 상황이므로 deposit 를 일으키지 않는다. 동시에 clear 테이블에서도 하등의 작업을 하지 않는다. 책임은 그 책임을 발생시킨 na_confirmer 가 지게 된다.
                ' 이 자리에서도 사고 금액의 증가는 발생하는 것이므로 node_accident 테이블에 신규사고 등록을 시킨다. 
                ' 이것을 정상적인 deposit 방식으로 처리하지 않는 것은 deposit 테이블의 row 갯수의 천문학적 증가를 방지하기 위함이다.

                ' node_accident 테이블에 신규사고 등록. 이 신규사고 등록이 deposit 를 대신한다. 즉 na_confirmer 의 seller 에 대한 deposit 가 된다.

                pure_query &= "USE bc_manager;" &
                    "INSERT INTO node_accident(" &
                    " `block_number`," &
                    " `contract_type`," &
                    " `na`," &
                    " `eoa`," &
                    " `coin_name`," &
                    " `amount`," &
                    " `amount_resolved`," &
                    " `idate_occured`)" &
                    "VALUES(" &
                    " 0," &
                    " '" & command_key & "'," &
                    " '" & Regex.Replace(na_confirmer, "^0x", "") & "'," &
                    " '" & Regex.Replace(seller, "^0x", "") & "'," &
                    " '" & currency & "'," &
                    " " & price_sum & "," &
                    " " & 0 & "," &
                    " '" & idate_string & "');"

                ' node 테이블에 사고 등록은 이미 되어 있으므로 별도로 사고등록은 필요없다.

            End If

        End If

        ' NFT 관련.

        ' owner_portion 에서 confirmer 로우 변경

        pure_query &= "USE bc_nft; UPDATE owner_portion SET `pieces_unconfirmed` = `pieces_unconfirmed` - " & pieces & ", `pieces_confirmed` = `pieces_confirmed` + " & pieces & " " &
        "WHERE `block_number` = " & owner_portion_block_number & ";"

        If confirmed_type = "confirmed" Then

            pure_query &= "USE bc_nft; UPDATE owner_portion SET `pieces_confirmed_ordered` = `pieces_confirmed_ordered` - " & pieces & " " &
            "WHERE `block_number` = " & parent_block_number & ";"

        End If

        ' seller 의 sell_order 에서 seized, completed 변경.

        pure_query &= "USE bc_nft; UPDATE sell_order SET `pieces_seized` = `pieces_seized` - " & pieces & ", `pieces_completed` = `pieces_completed` + " & pieces & " WHERE `block_number` = " & sell_order_block_number & ";"

        'confirmed_type 이 unconfirmed 일 때 연쇄

        If confirmed_type = "unconfirmed" Then

            pure_query &= "CALL up_loop_nft_confirm('" & command_key & "', " & parent_block_number & ", " & pieces & ", '" & Regex.Replace(seller, "^0x", "") & "', '" & Regex.Replace(na_seller, "^0x", "") & "', '" & idate_string & "');"

            ' up_loop_nft_confirm=================================================================================================================================================

            'CREATE PROCEDURE up_loop_nft_confirm

            '(IN in_command_key varchar(50),
            'IN in_parent_block_number bigint(20),
            'IN in_pieces int(11),
            'IN in_confirmer char(40), 
            'IN in_na_confirmer char(40), 
            'IN in_idate_string varchar(50))

            'BEGIN

            'DECLARE out_parent_block_number bigint(20);
            'DECLARE out_currency varchar(50);
            'DECLARE out_confirmed_type varchar(50);
            'DECLARE out_seller char(40);
            'DECLARE out_na_seller char(40);

            'loop_nft_confirm : WHILE True DO
            'CALL up_exe_nft_confirm(in_command_key, in_parent_block_number, in_pieces, in_confirmer, in_na_confirmer, in_idate_string, out_parent_block_number, out_currency, out_confirmed_type, out_seller, out_na_seller);
            'IF out_confirmed_type = 'unconfirmed' THEN
            '  SET in_parent_block_number = out_parent_block_number;
            '  SET in_confirmer = out_seller;
            '  SET in_na_confirmer = out_na_seller;
            'ELSE
            '  LEAVE loop_nft_confirm;
            'END IF;
            'END WHILE loop_nft_confirm;
            'END

            ' up_exe_nft_confirm=================================================================================================================================================

            'CREATE PROCEDURE up_exe_nft_confirm
            '(IN in_command_key varchar(50),
            'IN in_parent_block_number bigint(20),
            'IN in_pieces int(11),
            'IN in_confirmer char(40),
            'IN in_na_confirmer char(40),
            'IN in_idate_string varchar(50),
            'OUT out_parent_block_number bigint(20),
            'OUT out_currency varchar(50),
            'OUT out_confirmed_type varchar(50),
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
            'DECLARE v_price_sum decimal(60,30);   
            'DECLARE v_exchange_left char(40);
            'DECLARE v_exchange_right char(40);
            'DECLARE v_price_to_increase_decrease decimal(60,30);
            'DECLARE v_accident bit(1);     

            'SELECT owner_portion_block_number, owner_portion_sell_order_block_number, owner_portion_parent_block_number, sell_order_confirmed_type, sell_order_price_piece, sell_order_currency, sell_order_eoa, sell_order_na
            'INTO v_owner_portion_block_number, v_sell_order_block_number, v_parent_block_number, v_confirmed_type, v_price_piece, v_currency, v_seller, v_na_seller
            'FROM owner_portion_sell_order
            'WHERE `owner_portion_block_number` = in_parent_block_number;

            'SET v_price_sum = in_pieces * v_price_piece;

            'IF v_currency = 'guarantee' THEN

            '  UPDATE bc.account SET `balance` = `balance` + v_price_sum
            '  WHERE `eoa` = v_seller And `coin_name` = v_currency;

            '  UPDATE bc.account SET `balance` = `balance` - v_price_sum
            '  WHERE `eoa` = in_na_confirmer And `coin_name` = v_currency;

            'ELSE

            '  SET v_accident = 0;

            '  SELECT accident
            '  INTO v_accident
            '  FROM bc_manager.node
            '  WHERE `na` = in_na_confirmer;

            '    IF NOT v_accident THEN

            '       CALL bc.up_check_and_insert_new_row_deposit(v_na_seller, v_seller, v_currency, in_idate_string);

            '       UPDATE bc.deposit SET `balance` = `balance` + v_price_sum, idate = in_idate_string 
            '       WHERE `na` = v_na_seller AND `depositor` = v_seller AND `coin_name` = v_currency;

            '       IF NOT v_na_seller = in_na_confirmer THEN

            '           IF v_na_seller < in_na_confirmer THEN
            '               SET v_exchange_left = v_na_seller;
            '               SET v_exchange_right = in_na_confirmer;
            '               SET v_price_to_increase_decrease = v_price_sum;
            '           ELSE
            '               SET v_exchange_left = in_na_confirmer;
            '               SET v_exchange_right = v_na_seller;
            '               SET v_price_to_increase_decrease = (0 - v_price_sum);
            '           END IF;

            '           CALL bc.up_check_and_insert_new_row_clearing_house(v_exchange_left, v_exchange_right, v_currency, in_idate_string);

            '           UPDATE bc.clearing_house SET `balance` = `balance` + v_price_to_increase_decrease, `idate_last_transaction` = in_idate_string 
            '           WHERE `exchange_left` = v_exchange_left AND `exchange_right` = v_exchange_right AND `coin_name` = v_currency;

            '       END IF;

            '    ELSE

            '       INSERT INTO bc_manager.node_accident(
            '       `block_number`,
            '        `contract_type`,
            '        `na`,
            '        `eoa`,
            '        `coin_name`,
            '        `amount`,
            '        `amount_resolved`,
            '        `idate_occured`)
            '       VALUES(
            '        0,
            '        in_command_key,
            '        in_na_confirmer,
            '        v_seller,
            '        v_currency,
            '        v_price_sum,
            '        0,
            '        in_idate_string);

            '  END IF;

            'END IF;

            'UPDATE bc_nft.owner_portion SET `pieces_unconfirmed_ordered` = `pieces_unconfirmed_ordered` - in_pieces, `pieces_confirmed` = `pieces_confirmed` + in_pieces
            'WHERE `block_number` = v_owner_portion_block_number;

            'If v_confirmed_type = 'confirmed' Then

            '  UPDATE bc_nft.owner_portion SET `pieces_confirmed_ordered` = `pieces_confirmed_ordered` - in_pieces
            '  WHERE `block_number` = v_parent_block_number;

            'End If;

            'UPDATE bc_nft.sell_order SET `pieces_seized` = `pieces_seized` - in_pieces, `pieces_completed` = `pieces_completed` + in_pieces
            'WHERE `block_number` = v_sell_order_block_number;     

            'SET out_parent_block_number = v_parent_block_number;
            'SET out_currency = v_currency;
            'SET out_confirmed_type = v_confirmed_type;
            'SET out_seller = v_seller;
            'SET out_na_seller = v_na_seller;

            'END

            ' up_exe_nft_confirm=================================================================================================================================================

        End If

        'pure_query &= "USE bc_nft; UPDATE sell_order SET `state_open` = 1 WHERE `block_number` = " & sell_order_block_number & ";" ' 불필요. 락이 없다.

        Return pure_query

    End Function

    'Public Shared Function exe_ethereum_transfer_not_succeeded_and_no_need_cancel_transfer(
    '                                                                                        owner_portion_block_number As Long,
    '                                                                                        sell_order_block_number As Long,
    '                                                                                        parent_block_number As Long,
    '                                                                                        nfa As String,
    '                                                                                        token_id As Integer,
    '                                                                                        seller As String,
    '                                                                                        confirmer As String,
    '                                                                                        price_sum As Decimal,
    '                                                                                        auto_recall As Boolean,
    '                                                                                        currency As String,
    '                                                                                        gasPrice As Decimal,
    '                                                                                        gasLimit As Decimal,
    '                                                                                        pieces As Integer,
    '                                                                                        pieces_seized As Integer,
    '                                                                                        confirmed_type As String,
    '                                                                                        na_confirmer As String,
    '                                                                                        na_seller As String,
    '                                                                                        days_span As Integer,
    '                                                                                        closing_time_utc_string As String,
    '                                                                                        transaction_fee_rate As Decimal,
    '                                                                                        max_split As Integer,
    '                                                                                        idate_string As String) As String

    '    ' 이더리움 송금이 실패 내지 불분명한 경우인데 이 때 NFT 반납을 할 것인가의 문제이다. 또한 연쇄를 일으킬 것인가, 캔슬을 할 것인가 말 것인가.
    '    ' 이더리움은 컨퍼머의 거래소에 보관 중이기 때문에 컨퍼머에게는 안전하다.
    '    ' 나아가 auto_recall 연쇄는 반드시 일으켜 주어야 한다. 따라서 NFT 는 무조건 반납한다.
    '    ' 결국 추후 이더리움을 컨퍼머에게 별도 조사를 통하여 confirmer_na 가 지급을 하는 부분만 제외하면 나머지는 똑같다.
    '    ' 즉 pure_query 에서 이더리움 송금처리만 하지 않으면 된다.
    '    ' 나아가 애당초 cancel 을 하지 말아야 한다. 그런데 이것은 분명하지 않다. 가스비 문제만 아니라면 캔슬을 해 주어 명확하게 하는 것이 좋다.
    '    ' 일반 송금의 경우는 캔슬이 맞다. 그러나 환전의 경우는 복잡해진다. 
    '    ' multilevel 의 경우 buy 의 경우 캔슬이 맞다. confirm 의 경우는 캔슬할 이유가 없다.
    '    ' NFT의 경우 buy 시에는 캔슬이 필요하다. 컨펌의 경우는 오토컨펌일 때 절대 캔슬을 하면 안된다. 반면 오토컨펌이 아니면 캔슬을 해 준다.
    '    ' 그러나 이상의 부분은 추후 다른 contract 형태까지 모두 전체적으로 다시 정리해야 하는 부분이다.

    '    Dim pure_query = ""

    '    Dim pieces_type = "pieces_" & confirmed_type
    '    Dim pieces_ordered_type = "pieces_" & confirmed_type & "_ordered"

    '    ' NFT 반납.

    '    ' owner_portion 에서 confirmer 로우 변경

    '    pure_query &= "USE bc_nft; UPDATE owner_portion SET `pieces_unconfirmed` = `pieces_unconfirmed` - " & pieces & " " &
    '    "WHERE `block_number` = " & owner_portion_block_number & ";"

    '    ' seller 의 sell_order 에서 seized, confirmed 변경.

    '    pure_query &= "USE bc_nft; UPDATE sell_order SET `pieces_seized` = `pieces_seized` - " & pieces & ", `pieces_confirmed` = `pieces_confirmed` + " & pieces & " WHERE `block_number` = " & sell_order_block_number & ";"

    '    ' seller 의 owner_portion 에서 pieces_type. ordered 변경.

    '    pure_query &= "USE bc_nft; UPDATE owner_portion SET `" & pieces_type & "` = `" & pieces_type & "` + " & pieces & ", `" & pieces_ordered_type & "` = `" & pieces_ordered_type & "` - " & pieces & " " &
    '    "WHERE `block_number` = " & parent_block_number & ";"

    '    pure_query &= "USE bc_nft; UPDATE sell_order SET `state_open` = 1 WHERE `block_number` = " & sell_order_block_number & ";"

    '    If auto_recall And currency = "guarantee" Then

    '        pure_query &= "CALL up_loop_nft_confirm(" & parent_block_number & ", " & pieces & ", '" & Regex.Replace(seller, "^0x", "") & "', '" & Regex.Replace(na_seller, "^0x", "") & "', '" & idate_string & "');"

    '    End If

    '    Return pure_query

    'End Function

End Class



