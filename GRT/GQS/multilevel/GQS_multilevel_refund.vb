Public Class GQS_multilevel_refund

    Public Shared Function exe(
                              sell_order_block_number As Long,
                              eoa_guarantee_seller As String,
                              seller_na As String,
                              guarantee_amount As Decimal,
                              idate_string As String) As String

        Dim pure_query As String

        ' seller_agency 거래소의 개런티 계좌 출금

        pure_query = "USE bc; UPDATE account SET `balance` = `balance` - " & guarantee_amount & ", " & "idate = '" & idate_string & "' " &
                "WHERE `eoa` = '" & Regex.Replace(seller_na, "^0x", "") & "' AND `coin_name` = '" & "guarantee" & "';"

        ' eoa_guarantee_seller 의 개런티 계좌 입금

        pure_query &= "USE bc; UPDATE account SET `balance` = `balance` + " & guarantee_amount & ", " & "idate = '" & idate_string & "' " &
                "WHERE `eoa` = '" & Regex.Replace(eoa_guarantee_seller, "^0x", "") & "' AND `coin_name` = '" & "guarantee" & "';"

        ' sell_order 테이블 상태변경

        pure_query &= "USE bc_multilevel; UPDATE sell_order SET `state` = 'refunded' " &
                "WHERE `block_number` = " & sell_order_block_number & ";"

        Return pure_query

    End Function

End Class
