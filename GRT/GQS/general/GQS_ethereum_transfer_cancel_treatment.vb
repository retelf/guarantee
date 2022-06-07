Public Class GQS_ethereum_transfer_cancel_treatment

    Public Shared Function exe(
                              ethereum_transfer_eoa_from As String,
                              gasPrice_for_cancel As Decimal,
                              gasLimit As Decimal,
                              idate_string As String) As String

        ' default.html 에서 css 처리
        ' Public Class multilevel_submit_buy 에서 애당초 ma 공개키, 비밀키 발급
        ' 이리로 ma 가 전달된다.
        ' 추후 리펀드 시 ma 제거한다. 

        Dim pure_query As String

        Dim gas_amount = (gasPrice_for_cancel * gasLimit) / 1000000000

        ' 바이어(clicker)의 이더리움 계좌 출금

        pure_query = "USE bc; UPDATE account SET `balance` = `balance`- " & gas_amount & ", " & "idate = '" & idate_string & "' " &
                "WHERE `eoa` = '" & Regex.Replace(ethereum_transfer_eoa_from, "^0x", "") & "' AND `coin_name` = '" & "ethereum" & "';"

        Return pure_query

    End Function

End Class
