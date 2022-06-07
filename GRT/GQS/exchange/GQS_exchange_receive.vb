Public Class GQS_exchange_receive

    Public Shared Function guarantee(eoa As String, na As String, coin_name_from As String, coin_name_to As String, amount As Decimal, exchange_rate As Decimal, gasPrice As Decimal, gasLimit As Decimal, state As String, idate_string As String) As String

        Dim pure_query, pure_query_from, pure_query_to As String
        Dim amount_from As Decimal

        amount_from = amount / 1000000000

        ' 클라이언트의 계좌

        pure_query_from = "USE bc_manager; UPDATE account SET `balance` = `balance` - " & amount_from & ", " & "idate = '" & idate_string & "' " &
                "WHERE `eoa` = '" & Regex.Replace(eoa, "^0x", "") & "' AND `coin_name` = '" & coin_name_from & "';"

        ' 파운데이션의 계좌

        pure_query_to = "USE bc_manager; UPDATE account SET `balance` = `balance` + " & amount & ", " & "idate = '" & idate_string & "' " &
                "WHERE `eoa` = '" & Regex.Replace(GRT.GR.foundation.public_key, "^0x", "") & "' AND `coin_name` = '" & coin_name_to & "';"

        pure_query = pure_query_from & pure_query_to

        Return pure_query

    End Function

End Class
