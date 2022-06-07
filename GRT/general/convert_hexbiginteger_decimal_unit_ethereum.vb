Public Class convert_hexbiginteger_decimal_unit_ethereum

    Public Shared Function exe(hex_big_integer As Nethereum.Hex.HexTypes.HexBigInteger) As Decimal

        Dim decimal_unit_ethereum As Decimal
        Dim hex_big_integer_str, decimal_str, need_zero_str As String
        Dim need_zero As Integer

        need_zero_str = ""

        hex_big_integer_str = hex_big_integer.ToString

        Dim length = hex_big_integer_str.Length

        If length <= 18 Then

            need_zero = 18 - length

            For i = 0 To need_zero - 1

                need_zero_str &= "0"

            Next

            decimal_str = "0." & need_zero_str & hex_big_integer_str

        Else

            Dim front, rear As String

            rear = Regex.Match(hex_big_integer_str, "\d{18}$").ToString

            front = Regex.Replace(hex_big_integer_str, "\d{18}$", "").ToString

            decimal_str = front & "." & rear

        End If

        decimal_unit_ethereum = CDec(decimal_str)

        Return decimal_unit_ethereum

    End Function

End Class
