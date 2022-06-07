Public Class verify_password

    Public Shared Function exe(coin_type As String, password As String, public_key As String) As Boolean

        ' 키파일로부터 private_key 를 추출하고

        Dim return_value = decrypt_keystore_file.exe(coin_type, password, public_key)

        If Regex.Match(return_value, "same mac").Success Then ' 이것 자체가 실패하거나

            Return False

        Else ' private_key 로부터 public_key 가 도출되지 않을 때 검증실패.

            ' 그러나 나중에 하기로 하고 일단

            Return True

        End If

    End Function

End Class
