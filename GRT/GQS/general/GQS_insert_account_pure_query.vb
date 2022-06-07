Public Class GQS_insert_account_pure_query

    Public Shared Function exe(eoa As String, representative As String, balance_big_integer_decimal_guarantee As Decimal, balance_big_integer_decimal_etc() As Decimal, node As String, idate_string As String) As String

        Dim pure_query As String

        pure_query = "USE bc_manager; " &
            "INSERT IGNORE INTO account(" &
            "`block_number`," &
            " `eoa`," &
            " `representative`," &
            " `node`," &
            " `idate`)" &
            "VALUES(" &
            " 0," &
            " '" & Regex.Replace(eoa, "^0x", "") & "'," &
            " '" & Regex.Replace(representative, "^0x", "") & "'," &
            " '" & Regex.Replace(node, "^0x", "") & "'," &
            " '" & idate_string & "');"

        pure_query &= "USE bc; " &
            "INSERT IGNORE INTO account(" &
            "`block_number`," &
            " `eoa`," &
            " `eoa_type`," &
            " `coin_name`," &
            " `balance`," &
            " `locked`," &
            " `idate`)" &
            "VALUES(" &
            " 0," &
            " '" & Regex.Replace(eoa, "^0x", "") & "'," &
            " 'eoa'," &
            " 'guarantee'," &
            " " & balance_big_integer_decimal_guarantee / 1000000000000000000 & "," &
            " 0," &
            " '" & idate_string & "');"

        pure_query &= "USE bc; " &
            "INSERT IGNORE INTO account(" &
            "`block_number`," &
            " `eoa`," &
            " `eoa_type`," &
            " `coin_name`," &
            " `balance`," &
            " `locked`," &
            " `idate`)" &
            "VALUES(" &
            " 0," &
            " '" & Regex.Replace(eoa, "^0x", "") & "'," &
            " 'eoa'," &
            " 'ethereum'," &
            " " & balance_big_integer_decimal_etc(0) / 1000000000000000000 & "," &
            " 0," &
            " '" & idate_string & "');"

        Return pure_query

    End Function

End Class
