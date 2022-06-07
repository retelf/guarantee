Imports System.IO
Imports Newtonsoft.Json
Imports Microsoft.Web.WebView2.Core
Imports Nethereum.Hex.HexTypes

Public Class generate_multilevel_coding_form

    Shared html_directory As String = Regex.Replace(Directory.GetCurrentDirectory, "guarantee7\\guarantee7.+", "guarantee7\guarantee7_wallet\wv\multilevel\html")
    Public Shared Sub exe()

        If GRT.GR.account.login_state = "guarantee" Then

            GR.control.wv_main.CoreWebView2.Navigate(html_directory & "\default.html")
            GR.control.wv_sub.CoreWebView2.Navigate(html_directory & "\sell_orders.html")

            RemoveHandler GR.control.wv_main.CoreWebView2.WebMessageReceived, AddressOf UpdateContent
            AddHandler GR.control.wv_main.CoreWebView2.WebMessageReceived, AddressOf UpdateContent

            RemoveHandler GR.control.wv_sub.CoreWebView2.WebMessageReceived, AddressOf UpdateContent
            AddHandler GR.control.wv_sub.CoreWebView2.WebMessageReceived, AddressOf UpdateContent

        Else

            MessageBox.Show("먼저 개런티 계정으로 로그인을 하시기 바랍니다.")

            Return

        End If

    End Sub
    Shared Async Sub UpdateContent(ByVal sender As Object, ByVal args As CoreWebView2WebMessageReceivedEventArgs)

        Dim key, coin_name As String

        Dim json_message As String = args.WebMessageAsJson()

        Dim json_webview As Newtonsoft.Json.Linq.JObject = CType(JsonConvert.DeserializeObject(json_message), Linq.JObject)

        key = json_webview("key").ToString

        coin_name = GRT.GR.account.login_state

        Select Case key

            Case "initialize_multilevel_form"

                Call initialize_multilevel_form.exe()

            Case "display_board_sell_order" ' 전광판

                Call multilevel_display_board_sell_order.exe()

            Case "submit_sell_order"

                Call multilevel_submit_sell_order.exe(json_message)

            Case "submit_refund"

                Call multilevel_submit_refund.exe(json_message)

            Case "submit_buy"

                Call multilevel_submit_buy.exe(json_message)

            Case "submit_recall"

                Call multilevel_submit_recall.exe(json_message)

            Case "submit_confirm"

                Call multilevel_submit_confirm.exe(json_message)

        End Select

    End Sub

End Class
