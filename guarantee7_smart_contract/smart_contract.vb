Imports System.ServiceProcess
Imports System.Runtime.InteropServices
Imports System.Net
Imports System.IO
Imports Microsoft.Web.WebView2.Core
Imports Newtonsoft.Json

Public Class smart_contract
    Private Sub Form_Resize(ByVal sender As Object, ByVal e As EventArgs)
        wv_working.Height = Me.ClientSize.Height - wv_working.Top - 20
        wv_test_launch.Height = Me.ClientSize.Height - wv_test_launch.Top - 20
    End Sub

    Public Sub New()

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.

        GR.control.wv_working = Me.wv_working
        GR.control.wv_test_launch = Me.wv_test_launch

        GR.control.lbl_login = Me.lbl_login
        GR.control.lbl_login_state = Me.lbl_login_state
        GR.control.lbl_membership = Me.lbl_membership

        AddHandler Me.Resize, New System.EventHandler(AddressOf Form_Resize)

        Call Initialize_webview() ' 반드시 필요하다.

        Call GRT.set_initial_variables.exe(Me.[GetType]().Name)

    End Sub

    Private Async Sub Initialize_webview()

        Await wv_working.EnsureCoreWebView2Async(Nothing)
        Await wv_test_launch.EnsureCoreWebView2Async(Nothing)

    End Sub
    Private Sub lbl_login_Click(sender As Object, e As EventArgs) Handles lbl_login.Click

        Call GRT.generate_login_form.exe("smart_contract", wv_working, lbl_login, lbl_login_state, lbl_membership)

    End Sub

    Private Sub lbl_add_account_Click(sender As Object, e As EventArgs) Handles lbl_add_account.Click

        Call GRT.generate_add_account_form.exe(wv_working, lbl_add_account)

    End Sub

    Private Sub lbl_membership_Click(sender As Object, e As EventArgs) Handles lbl_membership.Click

        Call GRT.generate_super_account_membership_form.exe(wv_working, lbl_login, lbl_membership)

    End Sub

    Private Sub btn_coding_Click(sender As Object, e As EventArgs) Handles btn_coding.Click

        Call generate_coding_form.exe()

    End Sub

    Private Sub btn_market_Click(sender As Object, e As EventArgs) Handles btn_market.Click

        Call generate_market_coding_form.exe()

    End Sub

    Private Sub btn_multilevel_pension_contract_Click(sender As Object, e As EventArgs) Handles btn_multilevel_pension_contract.Click

        Call generate_multilevel_pension_contract_coding_form.exe()

    End Sub
End Class