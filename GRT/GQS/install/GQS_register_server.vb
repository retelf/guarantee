Public Class GQS_register_server
    Public Shared Function exe(eoa As String, na As String, exchange_name As String, type As String, domain As String, ip As String, port As Integer, port_nft As Integer, idate_string As String) As String

        Dim pure_query As String = ""

        For i = 0 To GRT.GR.coin_name.Length - 1

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
                " '" & Regex.Replace(na, "^0x", "") & "'," &
                " 'na'," &
                " '" & GRT.GR.coin_name(i) & "'," &
                " 0," &
                " 0," &
                " '" & idate_string & "');"

        Next

        pure_query &= "USE bc_manager; " &
            "INSERT INTO node(" &
            "`block_number`," &
            " `na`," &
            " `exchange_name`," &
            " `eoa`," &
            " `type`," &
            " `main`," &
            " `domain`," &
            " `ip`," &
            " `port`," &
            " `port_nft`," &
            " `parent`," &
            " `count_children`," &
            " `level`," &
            " `point`," &
            " `running`," &
            " `accident`," &
            " `idate`)" &
            "VALUES(" &
            " 0," &
            " '" & Regex.Replace(na, "^0x", "") & "'," &
            " '" & exchange_name & "'," &
            " '" & Regex.Replace(eoa, "^0x", "") & "'," &
            " '" & type & "'," &
            " 0," &
            " '" & domain & "'," &
            " '" & ip & "'," &
            " " & port & "," &
            " " & port_nft & "," &
            " ''," &
            " 0," &
            " -1," &
            " 0," &
            " 1," &
            " 0," &
            " '" & idate_string & "');"

        Return pure_query

    End Function

End Class
