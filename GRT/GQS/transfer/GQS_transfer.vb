Public Class GQS_transfer

    Public Shared Function exe(coin_name As String, eoa_from As String, eoa_to As String, amount As Decimal, gasPrice As Decimal, gasLimit As Decimal, idate_string As String) As String

        Dim pure_query As String
        Dim amount_from As Decimal

        amount_from = amount + (gasPrice * gasLimit) / 1000000000

        pure_query = "USE bc; UPDATE account SET `balance` = `balance` - " & amount_from & ", " & "`idate` = '" & idate_string & "' " &
                "WHERE `eoa` = '" & Regex.Replace(eoa_from, "^0x", "") & "' AND `coin_name` = '" & coin_name & "';"

        pure_query &= "USE bc; UPDATE account SET `balance` = `balance` + " & amount & ", " & "`idate` = '" & idate_string & "' " &
                "WHERE `eoa` = '" & Regex.Replace(eoa_to, "^0x", "") & "' AND `coin_name` = '" & coin_name & "';"

        Return pure_query

    End Function

End Class
