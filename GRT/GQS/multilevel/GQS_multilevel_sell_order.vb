Public Class GQS_multilevel_sell_order

    Public Shared Function exe(
                              eoa As String,
                              na As String,
                              exchange_name As String,
                              seller_agency_domain As String,
                              seller_agency_ip As String,
                              seller_agency_port As Integer,
                              exchange_rate As Decimal,
                              days_span As Integer,
                              closing_time_utc_string As String,
                              exchange_fee_rate As Decimal,
                              idate_string As String) As String

        ' 수수료를 미리 수령한다. 취소시에도 가스비 중복이 일어나지 않는다.

        Dim pure_query As String

        pure_query = "USE bc_multilevel;" &
            "INSERT INTO sell_order(" &
            "`block_number`," &
            " `eoa`," &
            " `na`," &
            " `exchange_name`," &
            " `domain`," &
            " `ip`," &
            " `port`," &
            " `exchange_rate`," &
            " `days_span`," &
            " `closing_time`," &
            " `exchange_fee_rate`," &
            " `state`," &
            " `idate`)" &
            "VALUES(" &
            " 0," &
            " '" & Regex.Replace(eoa, "^0x", "") & "'," &
            " '" & Regex.Replace(na, "^0x", "") & "'," &
            " '" & exchange_name & "'," &
            " '" & seller_agency_domain & "'," &
            " '" & seller_agency_ip & "'," &
            " " & seller_agency_port & "," &
            " " & exchange_rate & "," &
            " " & days_span & "," &
            " '" & closing_time_utc_string & "'," &
            " " & exchange_fee_rate & "," &
            " 'alive'," &
            " '" & idate_string & "');"

        pure_query &=
            "USE bc;" &
            "UPDATE `account` SET `balance` = `balance` - " & (1 + exchange_fee_rate) & " WHERE `eoa` = '" & Regex.Replace(eoa, "^0x", "") & "' AND `coin_name` = 'guarantee';"

        pure_query &=
            "USE bc;" &
            "UPDATE `account` SET `balance` = `balance` + " & (1 + exchange_fee_rate) & ", `locked` = `locked` + 1 WHERE `eoa` = '" & Regex.Replace(na, "^0x", "") & "' AND `coin_name` = 'guarantee';" ' 리펀드 시에도 fee 는 반환되지 않는다.

        Return pure_query

    End Function

End Class
