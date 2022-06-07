Public Class GQS_load_order

    Public Shared Function exe(eoa As String, na As String, seller_agency_domain As String, seller_agency_ip As String, seller_agency_port As Integer, coin_name_from As String, coin_name_to As String, amount As Decimal, exchange_rate As Decimal, exchange_fee_rate As Decimal, gasPrice As Decimal, gasLimit As Decimal, idate_string As String) As String

        ' 수수료를 미리 수령한다. 취소시에도 가스비 중복이 일어나지 않는다.

        Dim pure_query As String

        pure_query = "USE bc;" &
            "INSERT INTO exchange(" &
            "`block_number`," &
            " `eoa`," &
            " `na`," &
            " `domain`," &
            " `ip`," &
            " `port`," &
            " `coin_name_from`," &
            " `coin_name_to`," &
            " `amount`," &
            " `exchange_rate`," &
            " `exchange_fee_rate`," &
            " `gasPrice`," &
            " `gasLimit`," &
            " `state`," &
            " `idate`)" &
            "VALUES(" &
            " 0," &
            " '" & Regex.Replace(eoa, "^0x", "") & "'," &
            " '" & Regex.Replace(na, "^0x", "") & "'," &
            " '" & seller_agency_domain & "'," &
            " '" & seller_agency_ip & "'," &
            " " & seller_agency_port & "," &
            " '" & coin_name_from & "'," &
            " '" & coin_name_to & "'," &
            " " & amount & "," &
            " " & exchange_rate & "," &
            " " & exchange_fee_rate & "," &
            " " & gasPrice & "," &
            " " & gasLimit & "," &
            " 'alive'," &
            " '" & idate_string & "');"

        pure_query &=
            "USE bc;" &
            "UPDATE `account` SET `balance` = `balance` - " & amount * (1 + exchange_fee_rate) & " WHERE `eoa` = '" & Regex.Replace(eoa, "^0x", "") & "' AND `coin_name` = '" & coin_name_from & "';"

        pure_query &=
            "USE bc;" &
            "UPDATE `account` SET `balance` = `balance` + " & amount * (1 + exchange_fee_rate) & " WHERE `eoa` = '" & Regex.Replace(na, "^0x", "") & "' AND `coin_name` = '" & coin_name_from & "';"

        Return pure_query

    End Function

End Class
