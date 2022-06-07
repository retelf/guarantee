Public Class GQS_sub_multilevel_buy_register

    Public Shared Function exe(ma As String, sell_order_block_number As Long, eoa As String, idate_string As String) As String

        ' 다단계 ma 추출

        Dim parent As String
        Dim level, count_children As Integer

        Call assign_parent_ma.exe()

        parent = assign_parent_ma.data.parent(assign_parent_ma.data.parent.Length - 1)
        level = assign_parent_ma.data.level
        count_children = assign_parent_ma.data.count_children

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
            " '" & Regex.Replace(parent, "^0x", "") & "'," &
            " 0," &
            " " & level + 1 & "," &
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

        ' 해당 parent 의 자식 숫자를 하나 올려준다.

        pure_query &=
            "USE bc_multilevel;" &
            "UPDATE account SET `count_children` = " & count_children + 1 & " WHERE ma = '" & parent & "';"

        Return pure_query

    End Function

End Class
