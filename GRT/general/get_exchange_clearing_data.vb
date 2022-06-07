Public Class get_exchange_clearing_data

    Public Structure data

        Dim exchange_left As String
        Dim exchange_right As String
        Dim price_to_increase_decrease As Decimal

    End Structure

    Public Shared Function exe(na_seller As String, na_confirmer As String, price_sum As Decimal) As data

        Dim data As New data

        ' na_confirmer 가 na_seller 에게 빚을 져야 한다. 즉 na_seller 의 밸런스가 price_to_increase_decrease 만큼 증가해야 한다.

        ' 동일 거래소인 경우 아무런 변화가 없게 된다. 단지 클라이언트간의 변동이기 때문.

        If Not na_seller = na_confirmer Then

            If Regex.Replace(na_seller, "^0x", "") < Regex.Replace(na_confirmer, "^0x", "") Then

                data.exchange_left = na_seller
                data.exchange_right = na_confirmer
                data.price_to_increase_decrease = price_sum

            Else

                data.exchange_left = na_confirmer
                data.exchange_right = na_seller
                data.price_to_increase_decrease = -price_sum

            End If

            Return data

        Else

            Return Nothing

        End If

    End Function

End Class
