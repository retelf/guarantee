Public Class GQS_submit_cancel

    Public Shared Function exe(block_number As Long, eoa As String, na As String, coin_name_from As String, amount As Decimal, exchange_fee_rate As Decimal, gasPrice As Decimal, gasLimit As Decimal) As String

        Dim pure_query As String

        pure_query =
            "USE bc;" &
            "UPDATE `account` SET `balance` = `balance` + " & amount * (1 + exchange_fee_rate) - (gasPrice * gasLimit) / 1000000000 & " WHERE `eoa` = '" & Regex.Replace(eoa, "^0x", "") & "' AND `coin_name` = '" & coin_name_from & "';"

        pure_query &=
            "USE bc;" &
            "UPDATE `account` SET `balance` = `balance` - " & amount * (1 + exchange_fee_rate) & " WHERE `eoa` = '" & Regex.Replace(na, "^0x", "") & "' AND `coin_name` = '" & coin_name_from & "';"

        ' exchange 에서 delete

        pure_query &= "USE bc; DELETE FROM exchange WHERE `block_number` = " & block_number & ";"

        Return pure_query

    End Function

End Class
