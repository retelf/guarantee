Public Class GQS_multilevel_recall

    Public Shared Function exe(
                              sell_order_block_number As Long,
                              buyer As String,
                              eoa_guarantee_seller As String,
                              buyer_na As String,
                              ethereum_amount As Decimal,
                              gasPrice As Decimal,
                              gasLimit As Decimal,
                              idate_string As String) As String

        ' seller_na 로부터 buyer 에게 ethereum_amount 를 반환한다.
        ' recall 시에는 가스비를 공제한 전액을 돌려주고 confirm 시에는 수수료를 공제하되 가스비는 na 가 부담한다.
        ' ma 의 오너가 buyer 로부터 eoa_guarantee_seller 로 바뀐다.

        Dim pure_query As String

        Dim ethereum_amount_to_increase, ethereum_amount_to_reduce As Decimal

        ethereum_amount_to_increase = ethereum_amount - (gasPrice * gasLimit) / 1000000000
        ethereum_amount_to_reduce = ethereum_amount

        ' buyer 의 이더리움 계좌 입금

        pure_query = "USE bc; UPDATE account SET `balance` = `balance` + " & ethereum_amount_to_increase & ", " & "idate = '" & idate_string & "' " &
                    "WHERE `eoa` = '" & Regex.Replace(buyer, "^0x", "") & "' AND `coin_name` = '" & "ethereum" & "';"

        ' buyer_agency 거래소의 이더리움 계좌 출금 - 바이어가 자신의 이더리움을 맡기는 것은 자신의 에이전시이어야 하기 때문. 이 처리행위도 buyer_agency 에서 하게 된다.

        pure_query &= "USE bc; UPDATE account SET `balance` = `balance` - " & ethereum_amount_to_reduce & ", " & "idate = '" & idate_string & "' " &
                "WHERE `eoa` = '" & Regex.Replace(buyer_na, "^0x", "") & "' AND `coin_name` = '" & "ethereum" & "';"

        ' ma 의 오너 교체

        pure_query &= "USE bc_multilevel; UPDATE account SET `eoa` = '" & Regex.Replace(eoa_guarantee_seller, "^0x", "") & "' " &
                "WHERE `block_number` = " & sell_order_block_number & ";"

        ' sell_order 테이블 상태변경

        pure_query &= "USE bc_multilevel; UPDATE sell_order SET `state` = 'recalled' " &
                "WHERE `block_number` = " & sell_order_block_number & ";"

        Return pure_query

    End Function

    Public Shared Function exe_cancel(whether_clear As Boolean,
                                      sell_order_block_number As Long,
                                      eoa_guarantee_seller As String,
                                      ethereum_transfer_eoa_from As String,
                                      gasPrice_for_cancel As Decimal,
                                      gasLimit As Decimal,
                                      idate_string As String) As String

        ' 일단 정당한 자로부터 recall 이 들어온 이상 ma 의 오너 교체는 반드시 이루어져야 한다.
        ' 하지만 이더리움의 송금은 기록되지 말아야 한다.

        Dim pure_query As String

        ' ma 의 오너 교체

        pure_query = "USE bc_multilevel; UPDATE account SET `eoa` = '" & Regex.Replace(eoa_guarantee_seller, "^0x", "") & "' " &
                "WHERE `block_number` = " & sell_order_block_number & ";"

        ' sell_order 테이블 상태변경

        pure_query &= "USE bc_multilevel; UPDATE sell_order SET `state` = 'recalled' " &
                "WHERE `block_number` = " & sell_order_block_number & ";"

        Dim gas_amount = (gasPrice_for_cancel * gasLimit) / 1000000000

        If whether_clear Then

            ' ethereum_transfer_eoa_from 의 이더리움 계좌 가스비 출금

            pure_query &= "USE bc; UPDATE account SET `balance` = `balance`- " & gas_amount & ", " & "idate = '" & idate_string & "' " &
                    "WHERE `eoa` = '" & Regex.Replace(ethereum_transfer_eoa_from, "^0x", "") & "' AND `coin_name` = '" & "ethereum" & "';"

        End If

        Return pure_query

    End Function

End Class
