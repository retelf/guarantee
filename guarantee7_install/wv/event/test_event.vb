Imports System.IO
Imports Newtonsoft.Json

Public Class test_event

    Public Shared Async Sub exe()

        ' 인스턴스 생성

        Await GR.control.wv.EnsureCoreWebView2Async(Nothing)

        ' 기본 html 생성

        Dim html_directory As String = Regex.Replace(Directory.GetCurrentDirectory, "guarantee7\\guarantee7.+", "guarantee7\guarantee7_install\wv\html")

        GR.control.wv.CoreWebView2.Navigate(html_directory & "\register_server.html")

        ' 스크립트 추가

        Dim script As String = ""

        script &=
            "document.addEventListener('click', function (event)" &
            "{" &
            "let elem = event.target;" &
            "let jsonObject =" &
            " {" &
            "   Key: 'click'," &
            "   Id: elem.id," &
            "   Value: elem.value" &
            " };" &
            "window.chrome.webview.postMessage(jsonObject);" &
            "});"

        Await GR.control.wv.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync($"" & script)

        ' WebMessageReceivedEvent 추가

        AddHandler GR.control.wv.CoreWebView2.WebMessageReceived, AddressOf UpdateContent

    End Sub
    Shared Sub UpdateContent(ByVal sender As Object, ByVal args As CoreWebView2WebMessageReceivedEventArgs)

        'Dim json_message As String = args.WebMessageAsJson()

        'Dim json = JsonConvert.DeserializeObject(json_message)

        'Dim Id As String = json("Id")
        'Dim Key As String = json("Key")

        'MessageBox.Show(Id)

        'GR.control.wv.CoreWebView2.ExecuteScriptAsync($"$('#{Id}').html('{Key}')")

    End Sub

End Class
