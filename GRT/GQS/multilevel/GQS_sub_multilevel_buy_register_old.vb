Public Class GQS_sub_multilevel_buy_register_old

    Public Shared Function exe(ma As String, sell_order_block_number As Long, eoa As String, idate_string As String) As String

        Dim pure_query As String

        pure_query = "USE bc_multilevel; " &
            "INSERT INTO account(" &
            "`block_number`," &
            " `ma`," &
            " `eoa`," &
            " `parent`," &
            " `count_children`," &
            " `level`," &
            " `point`," &
            " `running`," &
            " `idate`)" &
            "VALUES(" &
            " " & sell_order_block_number & "," &
            " '" & Regex.Replace(ma, "^0x", "") & "'," &
            " '" & Regex.Replace(eoa, "^0x", "") & "'," &
            " ''," &
            " 0," &
            " -1," &
            " 0," &
            " 1," &
            " '" & idate_string & "');"

        pure_query &= "USE bc; " &
            "INSERT INTO account(" &
            "`block_number`," &
            " `eoa`," &
            " `eoa_type`," &
            " `coin_name`," &
            " `balance`," &
            " `locked`," &
            " `idate`)" &
            "VALUES(" &
            " 0," &
            " '" & Regex.Replace(ma, "^0x", "") & "'," &
            " 'ma'," &
            " 'guarantee'," &
            " 0," &
            " 0," &
            " '" & idate_string & "');"

        pure_query &= "USE bc; " &
            "INSERT INTO account(" &
            "`block_number`," &
            " `eoa`," &
            " `eoa_type`," &
            " `coin_name`," &
            " `balance`," &
            " `locked`," &
            " `idate`)" &
            "VALUES(" &
            " 0," &
            " '" & Regex.Replace(ma, "^0x", "") & "'," &
            " 'ma'," &
            " 'ethereum'," &
            " 0," &
            " 0," &
            " '" & idate_string & "');"

        Return pure_query

    End Function

End Class
