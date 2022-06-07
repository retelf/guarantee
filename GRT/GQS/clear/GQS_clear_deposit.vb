Public Class GQS_clear_deposit

    Public Shared Function exe(coin_name As String, na As String, depositor As String, amount_to_clear As Decimal, gasPrice As Decimal, gasLimit As Decimal, idate_string As String) As String

        ' 사실상 이더리움 clear_deposit 이 아니면 이리로 오지 않는다.

        Dim pure_query As String
        Dim amount_to_increase, amount_to_reduce As Decimal

        Dim gas_fee = gasPrice * gasLimit / 1000000000

        amount_to_increase = amount_to_clear - gas_fee ' depositor
        amount_to_reduce = amount_to_clear ' na

        'na
        pure_query = "USE bc; UPDATE account SET `balance` = `balance` - " & amount_to_reduce & ", " & "`idate` = '" & idate_string & "' " &
                "WHERE `eoa` = '" & Regex.Replace(na, "^0x", "") & "' AND `coin_name` = '" & coin_name & "';"

        'depositor
        pure_query &= "USE bc; UPDATE account SET `balance` = `balance` + " & amount_to_increase & ", " & "`idate` = '" & idate_string & "' " &
                "WHERE `eoa` = '" & Regex.Replace(depositor, "^0x", "") & "' AND `coin_name` = '" & coin_name & "';"

        'deposit 테이블
        pure_query &= "USE bc; UPDATE deposit SET `balance` = `balance` - " & amount_to_reduce & ", " & "`idate` = '" & idate_string & "' " &
                "WHERE `na` = '" & Regex.Replace(na, "^0x", "") & "' AND `depositor` = '" & Regex.Replace(depositor, "^0x", "") & "' AND `coin_name` = '" & coin_name & "';"

        ' lock 처리 문제
        ' 이더리움의 경우 lock 감축은 하지 않는다. 여전히 자신의 앞의 na에게 채무는 남아 있으며 또한 자신의 뒤의 na 에 대해서도 채권이 그대로 잔존하기 때문이다.
        ' 이는 이미 lock 가 eoa 에 대한 것이 아니라 na 에 대한 것으로 변질되었기 때문이다.
        ' 결국 lock 감축은 차후 clear_exchange 를 하는 시점에서 행해지게 된다.
        ' guarantee 의 경우는 컨펌시에 직접 depositor 에게 크로싱 송금이 이루어지기 때문에 곧바로 락을 감축하게 된다. 
        ' 즉 buy 와 그 후속행위인 confirm 이나 recall 의 시점에서 곧바로 송금과 deposit 감축 그리고 락 해제가 동시에 이루어지기 때문에 추후 별도로 이곳에서 lock 해제를 해야 할 재원이 발생하지 않는다.

        Return pure_query

    End Function

End Class
