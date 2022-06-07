Public Class treat_test

    Public Shared Async Function exe(json As Newtonsoft.Json.Linq.JObject, JSS As String) As Task(Of String)

        Dim Updatecmd As MySqlCommand

        Dim sleeping_time = CInt(json("value")("sleeping_time"))
        Dim value = json("value")("value").ToString

        Dim unused = Await Task.Run(Function() sleep(sleeping_time))

        Dim Connection_mariadb_local_test As New MySqlConnection(GRT.GR.cString_mariadb_local_test)

        Dim uString = "UPDATE test SET `value` = " & value

        Connection_mariadb_local_test.Open()

        Updatecmd = New MySqlCommand(uString, Connection_mariadb_local_test)

        Updatecmd.CommandType = CommandType.Text

        Updatecmd.ExecuteNonQuery()

        Connection_mariadb_local_test.Close()

        Dim JRS = GRT.make_json_string.exe({{"key", "test", "quot"}, {"success", "success", "quot"}}, {}, False)

        Return JRS

    End Function

    Shared Function sleep(sleeping_time As Integer) As Boolean

        Threading.Thread.Sleep(sleeping_time * 1000)

        Return True

    End Function

End Class
