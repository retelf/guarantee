Public Class GQS_sub_multilevel_buy_distribute

    Public Shared Function exe(guarantee_amount As Decimal, idate_string As String) As String

        Dim pure_query As String = ""
        Dim first_parent_acquisition_sum, residual, other_parents_acquisition_sum, accumulated_sum As Decimal

        Dim hierarachy_length As Integer = assign_parent_ma.data.parent.Length

        Dim parents_sum(hierarachy_length - 1) As Decimal

        ' 분배율 결정

        If hierarachy_length = 1 Then

            first_parent_acquisition_sum = guarantee_amount
            other_parents_acquisition_sum = 0

        Else

            first_parent_acquisition_sum = GR.multilevel.first_parent_acquisition_ratio * guarantee_amount
            residual = guarantee_amount - first_parent_acquisition_sum
            other_parents_acquisition_sum = residual / (hierarachy_length - 1)

            other_parents_acquisition_sum = Math.Round(other_parents_acquisition_sum, 28)

        End If

        parents_sum(0) = first_parent_acquisition_sum

        accumulated_sum = parents_sum(0)

        For i = 1 To hierarachy_length - 1

            If i < hierarachy_length - 1 Then

                parents_sum(i) = other_parents_acquisition_sum

                accumulated_sum += parents_sum(i)

            Else

                parents_sum(hierarachy_length - 1) = guarantee_amount - accumulated_sum

            End If

        Next

        ' 분배

        For i = 0 To hierarachy_length - 1

            pure_query &= "USE bc; UPDATE account SET `balance` = `balance` + " & parents_sum(i) & ", " & "idate = '" & idate_string & "' " &
                    "WHERE `eoa` = '" & Regex.Replace(assign_parent_ma.data.parent(i), "^0x", "") & "' AND `coin_name` = '" & "guarantee" & "';"

        Next

        Return pure_query

    End Function

End Class
