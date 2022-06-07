Public Class smart_contract_common_initializing

    Public Shared Async Sub exe()

        ' 로그인 코인, 잔고

        Dim result = Await GR.control.wv_sub.CoreWebView2.ExecuteScriptAsync(
                        $"GR.login_state='{GRT.GR.account.login_state}';
                        GR.balance='{GRT.GR.account.balance}';
                        GR.eoa='{GRT.GR.account.public_key}';
                        GR.smart_contract_common_initializing();")

    End Sub

End Class
