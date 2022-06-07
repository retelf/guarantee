Public Class GQS_multilevel_buy

    Public Shared Function exe(
                              sell_order_block_number As Long,
                              ethereum_transfer_eoa_from As String,
                              ma As String,
                              seller_na As String,
                              buyer_na As String,
                              ethereum_amount As Decimal,
                              gasPrice As Decimal,
                              gasLimit As Decimal,
                              closing_time_utc_string As String,
                              idate_string As String) As String

        ' default.html 에서 css 처리
        ' Public Class multilevel_submit_buy 에서 애당초 ma 공개키, 비밀키 발급
        ' 이리로 ma 가 전달된다.
        ' 추후 리펀드 시 ma 제거한다. 

        Dim pure_query As String

        Dim guarantee_amount, ethereum_amount_to_increase, ethereum_amount_to_reduce As Decimal

        guarantee_amount = 1

        ethereum_amount_to_increase = ethereum_amount
        ethereum_amount_to_reduce = ethereum_amount + (gasPrice * gasLimit) / 1000000000

        ' 바이어(clicker)의 이더리움 계좌 출금

        pure_query = "USE bc; UPDATE account SET `balance` = `balance`- " & ethereum_amount_to_reduce & ", " & "idate = '" & idate_string & "' " &
                "WHERE `eoa` = '" & Regex.Replace(ethereum_transfer_eoa_from, "^0x", "") & "' AND `coin_name` = '" & "ethereum" & "';"

        ' buyer_agency 거래소의 이더리움 계좌 입금 - 바이어가 자신의 이더리움을 맡기는 것은 자신의 에이전시이어야 하기 때문. 그러나 이 처리행위(바이어 거래소로 이더리움 송금행위)는 seller_agency 가 직접 하게 된다.

        pure_query &= "USE bc; UPDATE account SET `balance` = `balance` + " & ethereum_amount_to_increase & ", `locked` = `locked` + " & ethereum_amount_to_increase & ", " & "idate = '" & idate_string & "' " &
                    "WHERE `eoa` = '" & Regex.Replace(buyer_na, "^0x", "") & "' AND `coin_name` = '" & "ethereum" & "';"

        ' seller_agency 거래소의 개런티 계좌 출금

        pure_query &= "USE bc; UPDATE account SET `balance` = `balance` - " & guarantee_amount & ", `locked` = `locked` - " & guarantee_amount & ", " & "idate = '" & idate_string & "' " &
                "WHERE `eoa` = '" & Regex.Replace(seller_na, "^0x", "") & "' AND `coin_name` = '" & "guarantee" & "';"

        ' 신규 ma 발급과 바이어(clicker)의 사업자 오너 등록

        pure_query &= GQS_sub_multilevel_buy_register.exe(ma, sell_order_block_number, ethereum_transfer_eoa_from, idate_string)

        ' 다단계 개런티 분배 - 이것은 treat query 에서 해 주어야 할 듯.

        pure_query &= GQS_sub_multilevel_buy_distribute.exe(guarantee_amount, idate_string)

        ' sell_order 테이블 상태변경 - 이것은 이제 바꾸어야 한다.

        pure_query &= "USE bc_multilevel; UPDATE sell_order SET `state` = 'seized', `closing_time` = '" & closing_time_utc_string & "' " &
                "WHERE `block_number` = " & sell_order_block_number & ";"

        Return pure_query

    End Function

End Class
