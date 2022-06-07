Public Class GQS_submit_exchange

    Public Shared Function exe(exchange_block_number As Long, clickers_coin_name_to_buy As String, clickers_coin_name_to_sell As String, eoa_seller As String, eoa_buyer As String, na As String, seller_amount As Decimal, exchange_rate As Decimal, gasPrice As Decimal, gasLimit As Decimal, idate_string As String) As String

        Dim pure_query As String
        Dim guarantee_amount, ethereum_amount_to_increase, ethereum_amount_to_reduce As Decimal

        If clickers_coin_name_to_buy = "ethereum" Then ' 개런티로 로그인 된 상태이다.

            ethereum_amount_to_increase = seller_amount
            ethereum_amount_to_reduce = seller_amount + (gasPrice * gasLimit) / 1000000000

            guarantee_amount = seller_amount * exchange_rate

            ' 바이어(clicker)의 이더리움 계좌 입금

            pure_query = "USE bc; UPDATE account SET `balance` = `balance` + " & ethereum_amount_to_increase & ", " & "idate = '" & idate_string & "' " &
                    "WHERE `eoa` = '" & Regex.Replace(eoa_buyer, "^0x", "") & "' AND `coin_name` = '" & "ethereum" & "';"

            ' 거래소의 이더리움 계좌 출금

            pure_query &= "USE bc; UPDATE account SET `balance` = `balance` - " & ethereum_amount_to_reduce & ", " & "idate = '" & idate_string & "' " &
                    "WHERE `eoa` = '" & Regex.Replace(na, "^0x", "") & "' AND `coin_name` = '" & "ethereum" & "';"

            ' 셀러(loader)의 개런티 계좌 입금

            pure_query &= "USE bc; UPDATE account SET `balance` = `balance` + " & guarantee_amount & ", " & "idate = '" & idate_string & "' " &
                    "WHERE `eoa` = '" & Regex.Replace(eoa_seller, "^0x", "") & "' AND `coin_name` = '" & "guarantee" & "';"

            ' 바이어(clicker)의 개런티 계좌 출금

            pure_query &= "USE bc; UPDATE account SET `balance` = `balance`- " & guarantee_amount & ", " & "idate = '" & idate_string & "' " &
                    "WHERE `eoa` = '" & Regex.Replace(eoa_buyer, "^0x", "") & "' AND `coin_name` = '" & "guarantee" & "';"

        Else ' 이더리움으로 로그인 된 상태이다.

            guarantee_amount = seller_amount

            ethereum_amount_to_increase = seller_amount * exchange_rate
            ethereum_amount_to_reduce = seller_amount * exchange_rate + (gasPrice * gasLimit) / 1000000000

            ' 바이어(clicker)의 개런티 계좌 입금

            pure_query = "USE bc; UPDATE account SET `balance` = `balance` + " & guarantee_amount & ", " & "idate = '" & idate_string & "' " &
                    "WHERE `eoa` = '" & Regex.Replace(eoa_buyer, "^0x", "") & "' AND `coin_name` = '" & "guarantee" & "';"

            ' 거래소의 개런티 계좌 출금

            pure_query &= "USE bc; UPDATE account SET `balance` = `balance` - " & guarantee_amount & ", " & "idate = '" & idate_string & "' " &
                    "WHERE `eoa` = '" & Regex.Replace(na, "^0x", "") & "' AND `coin_name` = '" & "guarantee" & "';"

            ' 셀러(loader)의 이더리움 계좌 입금

            pure_query &= "USE bc; UPDATE account SET `balance` = `balance` + " & ethereum_amount_to_increase & ", " & "idate = '" & idate_string & "' " &
                    "WHERE `eoa` = '" & Regex.Replace(eoa_seller, "^0x", "") & "' AND `coin_name` = '" & "ethereum" & "';"

            ' 바이어(clicker)의 이더리움 계좌 출금

            pure_query &= "USE bc; UPDATE account SET `balance` = `balance`- " & ethereum_amount_to_reduce & ", " & "idate = '" & idate_string & "' " &
                    "WHERE `eoa` = '" & Regex.Replace(eoa_buyer, "^0x", "") & "' AND `coin_name` = '" & "ethereum" & "';"

        End If

        ' exchange 에서 delete

        pure_query &= "USE bc; DELETE FROM exchange WHERE `block_number` = " & exchange_block_number & ";"

        Return pure_query

    End Function

End Class
