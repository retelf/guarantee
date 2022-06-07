Public Class treat_set_lock

    Public Shared Function exe(json As Newtonsoft.Json.Linq.JObject) As String

        ' multilevel 이나 exchange 의 경우에는 지분 개념이 없다. 따라서 state(alive, seized, recalled) 와 lock 두 개의 필드가 있게 된다.
        ' 반면 nft 의 경우는 지분개념이 있어서 state 가 다시 그 수량만큼 세부 표현이 필요하다. 따라서 alive, seized, recalled 세 필드와 lock 필드를 필요로 한다.
        ' nft recall lock의 경우는 연쇄적이므로 모든 연쇄에 대하여 lock 을 먼저 건 후 본작업을 실시하고 다시 연쇄적으로 lock 를 푼다. 만약 lock 중 lock를 만나면 uncommit 를 해 주게 된다. 

        Dim block_number As Long
        Dim key, database, table, lock, JRS As String

        key = json("key").ToString

        block_number = CLng(json("value")("block_number"))
        database = json("value")("database").ToString
        table = json("value")("table").ToString

        If key = "set_lock" Then

            Try

                lock = GRT.set_lock.exe(block_number, database, table)

                JRS = GRT.make_json_string.exe({{"key", key, "quot"}, {"success", "success", "quot"}}, {{"lock", CStr(lock), "quot"}}, False)

            Catch ex As Exception

                JRS = GRT.make_json_string.exe({{"key", json("key").ToString, "quot"}, {"success", "fail", "quot"}}, {{"reason", CStr(ex.Message), "quot"}}, False)

            End Try

        Else

            Call GRT.set_unlock.exe(block_number, database, table)

            JRS = GRT.make_json_string.exe({{"key", key, "quot"}, {"success", "success", "quot"}}, {{"lock", "unlock_tried", "quot"}}, False)

        End If

        Return JRS

    End Function

End Class
