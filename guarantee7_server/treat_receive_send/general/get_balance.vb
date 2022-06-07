Public Class get_balance

    Public Shared Async Function exe(json As Newtonsoft.Json.Linq.JObject) As Task(Of String)

        Dim eoa, coin_name, signiture, JRS As String
        Dim balance As Decimal

        eoa = json("value")("eoa").ToString
        coin_name = json("value")("coin_name").ToString
        signiture = json("value")("signiture").ToString

        Try

            If coin_name = "guarantee" Then

                balance = GRT.get_guarantee_balance_from_local_server.exe("foo", signiture, eoa)

            ElseIf coin_name = "ethereum" Then

                balance = Await GRT.get_balance.ethereum(eoa)

            End If

            JRS = GRT.make_json_string.exe({{"key", "get_balance", "quot"}, {"success", "success", "quot"}}, {{"balance", CStr(balance), "non_quot"}}, False)

        Catch ex As Exception

            JRS = GRT.make_json_string.exe({{"key", "get_balance", "quot"}, {"success", "fail", "quot"}}, {{"reason", CStr(ex.Message), "quot"}}, False)

        End Try

        Return JRS

    End Function

End Class
