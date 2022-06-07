Imports System.IO
Imports Microsoft.PowerShell.Commands
Imports Nethereum.Web3
Imports Newtonsoft.Json

Public Class Form1
    Private Sub btn_test_Click(sender As Object, e As EventArgs) Handles btn_test.Click

        Call GRT.set_initial_variables.exe(Me.[GetType]().Name)

        Dim private_key = GRT.decrypt_keystore_file.exe("guarantee", "#cyndi$36%", "0xD3aA97a3dA0D48b1774C9ff88a429B7217A061cB")

    End Sub

    Private Async Sub Run()

        Dim task1 = Task.Run(Function() LongCalcAsync(3))
        Dim sum As Integer = Await task1
        'Dim sum As Integer = Await Task.Run(Function() LongCalcAsync(3))

        MessageBox.Show(CStr(sum))

    End Sub

    Private Function LongCalcAsync(ByVal times As Integer) As Integer
        Dim result As Integer = 0

        For i As Integer = 0 To times - 1
            result += i
            Threading.Thread.Sleep(1000)
        Next

        Return result

    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Call plain_async()

        MessageBox.Show("ok0")

    End Sub

    Private Async Sub plain_async()

        Await Task.Run(Sub() dummy())

        Call plain()

    End Sub

    Private Sub plain()

        Dim result As Integer = 0

        For i As Integer = 0 To 2
            result += i
            Threading.Thread.Sleep(1000)
        Next

        MessageBox.Show("ok1")

    End Sub

    Private Sub dummy()

        Dim result As Integer = 0

        For i As Integer = 0 To 2
            result += i
            Threading.Thread.Sleep(1000)
        Next

        MessageBox.Show("ok2")

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        Dim header(,) As String = {{"www", "www", "quot"}, {"sss", "sss", "quot"}}

        Dim value1(,) As String = {{"name", "default", "quot"}, {"code", "zdfdfg sdfdfgdfg", "quot"}, {"extention", "html", "quot"}}

        Dim value2(,) As String = {{"name", "default", "quot"}, {"code", "sdcds dsd dfdfgdfg", "quot"}, {"extention", "js", "quot"}}

        Dim value3(,) As String = {{"name", "default", "quot"}, {"code", "ㅇㅌㅊㅇㅊ dfdfgdfg", "quot"}, {"extention", "css", "quot"}}

        Dim value = {value1, value2, value3}

        Dim json_str = GRT.make_json_string.exe_array(header, value, True)

        Dim json = CType(JsonConvert.DeserializeObject(json_str), Linq.JObject)

        Dim aaa = json("value")(1)("aaa").ToString

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        Dim json_str = "{""key"" : ""relay"", ""command_key"" : ""submit_mobile_authentication_number""}"

        Dim json = CType(JsonConvert.DeserializeObject(json_str), Linq.JObject)

        'json("key") = json("command_key")

        'json.Remove("command_key")

        json("key") = "no_realy"

        Dim jss = CType(JsonConvert.SerializeObject(json), String)

        Dim aa = ""

    End Sub

End Class
