Imports Microsoft.Web.WebView2.Core

Public Class wallet
    Private Sub Form_Resize(ByVal sender As Object, ByVal e As EventArgs)
        wv_main.Height = Me.ClientSize.Height - wv_main.Top - 20
    End Sub

    Public Sub New()

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.

        GR.control.wv_main = Me.wv_main
        GR.control.wv_sub = Me.wv_sub

        GR.control.lbl_login = Me.lbl_login
        GR.control.lbl_login_state = Me.lbl_login_state
        GR.control.lbl_membership = Me.lbl_membership

        AddHandler Me.Resize, New System.EventHandler(AddressOf Form_Resize)

        Call Initialize_webview() ' 반드시 필요하다.

        Call GRT.set_initial_variables.exe(Me.[GetType]().Name)

    End Sub

    Private Async Sub Initialize_webview()

        Await wv_main.EnsureCoreWebView2Async(Nothing)
        Await wv_sub.EnsureCoreWebView2Async(Nothing)

    End Sub

    Private Sub btn_smart_contract_Click(sender As Object, e As EventArgs) Handles btn_smart_contract.Click

        Call generate_smart_contract_form.exe()

    End Sub
    Private Sub wv_sub_NavigationCompleted(sender As Object, e As CoreWebView2NavigationCompletedEventArgs)

        Call smart_contract_common_initializing.exe()

    End Sub
    Private Sub lbl_login_Click(sender As Object, e As EventArgs) Handles lbl_login.Click

        Call GRT.generate_login_form.exe("wallet", wv_main, lbl_login, lbl_login_state, lbl_membership)

    End Sub

    Private Sub lbl_add_account_Click(sender As Object, e As EventArgs) Handles lbl_add_account.Click

        Call GRT.generate_add_account_form.exe(wv_main, lbl_add_account)

    End Sub

    Private Sub lbl_membership_Click(sender As Object, e As EventArgs) Handles lbl_membership.Click

        Call GRT.generate_super_account_membership_form.exe(wv_main, lbl_login, lbl_membership)

    End Sub

    Private Sub btn_transfer_Click(sender As Object, e As EventArgs) Handles btn_transfer.Click

        Call generate_transfer_coding_form.exe()

    End Sub

    Private Sub btn_clear_Click(sender As Object, e As EventArgs) Handles btn_clear.Click

    End Sub

    Private Sub btn_exchange_Click(sender As Object, e As EventArgs) Handles btn_exchange.Click

        Call generate_exchange_coding_form.exe()

    End Sub

    Private Sub btn_multilevel_Click(sender As Object, e As EventArgs) Handles btn_multilevel.Click

        Call generate_multilevel_coding_form.exe()

    End Sub

    Private Sub btn_nft_Click(sender As Object, e As EventArgs) Handles btn_nft.Click

        Call generate_nft_coding_form.exe()

    End Sub
End Class