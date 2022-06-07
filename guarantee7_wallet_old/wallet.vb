Imports System.IO
Imports Microsoft.Web.WebView2.Core
Imports Nethereum.KeyStore.Model

Public Class wallet

    '1. 로그인(회원가입)
    ' 
    '2. guarantee 보유량 확인과 구입
    '	a. 중앙으로부터 guarantee 구입
    '	b. eoa, 구입량, 구입대가 기록
    '3. 사업자 가입
    '	a. 상위컨트렉트 선정(또는 자동선정)
    '	b. 계약실행
    '		i. guarantee을 상위사업자에게 송금
    '		ii. 하위컨트렉트 계정 발급
    '4. 리펀드
    '	a. 사업자 계정 중앙으로 양도
    '	b. ethereum 리펀드
    '5. 사업자 양도
    '	a. 사업자 거래 마켓 
    '	b. eoa 만 존재하면 가능
    '	c. 양도방식
    '		i. ethereum
    '		ii. guarantee
    '		iii. 현금

    Shared contract_address As String
    Shared coin_amount As Decimal = 0
    Private Sub Form_Resize(ByVal sender As Object, ByVal e As EventArgs)
        wv.Size = Me.ClientSize - New System.Drawing.Size(wv.Location) - New Size(New Point(20, 20))
    End Sub

    Public Sub New()

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        GR.control.wv = Me.wv
        GR.control.pnl_initial_login = Me.pnl_initial_login
        GR.control.pnl_coin_login = Me.pnl_coin_login
        GR.control.pnl_purchase_coin = Me.pnl_purchase_coin
        GR.control.pnl_purchase_contract = Me.pnl_purchase_contract
        GR.control.pnl_refund = Me.pnl_refund
        GR.control.pnl_sell_membership = Me.pnl_sell_membership
        GR.control.pnl_coin_login_inner = Me.pnl_coin_login_inner
        GR.control.pnl_open_information = Me.pnl_open_information
        GR.control.txt_initial_public_key = Me.txt_initial_public_key
        GR.control.txt_coin_password = Me.txt_coin_password
        GR.control.txt_public_key = Me.txt_public_key
        GR.control.txt_private_key = Me.txt_private_key
        GR.control.btn_initial_login = Me.btn_initial_login
        GR.control.btn_coin_login = Me.btn_coin_login
        GR.control.lbl_login_state = Me.lbl_login_state
        GR.control.lbl_coin_name = Me.lbl_coin_name
        GR.control.lbl_public_key = Me.lbl_public_key
        GR.control.lbl_private_key = Me.lbl_private_key
        GR.control.lbl_message = Me.lbl_message
        GR.control.rb_account_exist = Me.rb_account_exist
        GR.control.rb_account_not_exist = Me.rb_account_not_exist
        GR.control.rb_open_infomation = Me.rb_open_infomation
        GR.control.rb_not_open_infomation = Me.rb_not_open_infomation
        GR.control.cb_coin_name = Me.cb_coin_name

        cb_coin_name.SelectedIndex = 0

        AddHandler Me.Resize, New System.EventHandler(AddressOf Form_Resize)

        Call Initialize_webview()

        Call GRT.set_initial_variables.exe()

    End Sub

    Private Async Sub Initialize_webview()

        Await wv.EnsureCoreWebView2Async(Nothing)

        Dim folder As String = Regex.Replace(Directory.GetCurrentDirectory, "guarantee7[^\\]*\\bin\\Debug\\net5.0-windows", "guarantee7_wallet\bin\Debug\net5.0-windows")

        Dim file_stream As String = get_file_streame.exe(folder & "\resource\html\default.html")

        wv.NavigateToString(file_stream)

    End Sub

    Private Sub _rb_account_exist_CheckedChanged(sender As Object, e As EventArgs) Handles rb_account_exist.CheckedChanged

        Call rb_account_exist_CheckedChanged.exe()

    End Sub

    Private Sub _rb_account_not_exist_Changed(sender As Object, e As EventArgs) Handles rb_account_not_exist.CheckedChanged

        Call rb_account_not_exist_Changed.exe()

    End Sub

    Private Sub _cb_coin_name_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cb_coin_name.SelectedIndexChanged

        Call cb_coin_name_SelectedIndexChanged.exe()

    End Sub

    Private Sub btn_login_Click(sender As Object, e As EventArgs) Handles btn_coin_login.Click

        Call coin_login_member.exe()

    End Sub

    Private Sub txt_password_Enter(sender As Object, e As EventArgs) Handles txt_coin_password.Enter
        txt_coin_password.Text = ""
        txt_coin_password.PasswordChar = CChar("*")
    End Sub

    Private Sub txt_password_Leave(sender As Object, e As EventArgs) Handles txt_coin_password.Leave
        If txt_coin_password.Text = "" Then
            txt_coin_password.Text = "숫자영문6~12자"
            txt_coin_password.PasswordChar = CChar("")
        End If
    End Sub

    Private Sub lbl_membership_Click(sender As Object, e As EventArgs) Handles lbl_membership.Click

        'MessageBox.Show(membership.exe())

    End Sub

    Private Sub btn_initial_login_Click(sender As Object, e As EventArgs) Handles btn_initial_login.Click

        Call initial_login.exe()

    End Sub

End Class