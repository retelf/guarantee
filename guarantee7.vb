Imports System.IO
Imports GRT
Public Class guarantee7

    Shared folder As String = Environment.CurrentDirectory

    Sub New()

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.

        Call GRT.set_initial_variables.exe(Me.[GetType]().Name)

        ' 일단 먼저 webview2 를 설치한다.

        Call install_webview2_runtime.exe()

        GR.control.wv = Me.wv
        GR.control.lbl_login = Me.lbl_login
        GR.control.lbl_login_state = Me.lbl_login_state
        GR.control.lbl_add_account = Me.lbl_add_account
        GR.control.lbl_membership = Me.lbl_membership

        AddHandler Me.Resize, New System.EventHandler(AddressOf Form_Resize)

    End Sub
    Private Sub Form_Resize(ByVal sender As Object, ByVal e As EventArgs)
        wv.Size = Me.ClientSize - New System.Drawing.Size(wv.Location) - New Size(New Point(20, 20))
    End Sub
    Private Sub btn_wallet_Click(sender As Object, e As EventArgs) Handles btn_wallet.Click

        Dim path As String = Regex.Replace(folder, "(guarantee7|test_guarantee)[^\\]*\\bin\\Debug\\net5.0-windows", "guarantee7_wallet\bin\Debug\net5.0-windows") & "\guarantee7_wallet.exe"

        Dim startInfo As ProcessStartInfo = New ProcessStartInfo(path)

        startInfo.Verb = "runas"

        System.Diagnostics.Process.Start(startInfo)

    End Sub

    Private Sub btn_server_Click(sender As Object, e As EventArgs) Handles btn_server.Click

        Dim path As String = Regex.Replace(folder, "(guarantee7|test_guarantee)[^\\]*\\bin\\Debug\\net5.0-windows", "guarantee7_server\bin\Debug\net5.0-windows") & "\guarantee7_server.exe"

        Dim startInfo As ProcessStartInfo = New ProcessStartInfo(path)

        startInfo.Verb = "runas"

        System.Diagnostics.Process.Start(startInfo)

    End Sub

    Private Sub btn_install_Click(sender As Object, e As EventArgs) Handles btn_install.Click

        Dim path As String = Regex.Replace(folder, "(guarantee7|test_guarantee)[^\\]*\\bin\\Debug\\net5.0-windows", "guarantee7_install\bin\Debug\net5.0-windows") & "\guarantee7_install.exe"

        Dim startInfo As ProcessStartInfo = New ProcessStartInfo(path)

        startInfo.Verb = "runas"

        System.Diagnostics.Process.Start(startInfo)

    End Sub

    Private Sub btn_smart_contract_Click(sender As Object, e As EventArgs) Handles btn_smart_contract.Click

        Dim path As String = Regex.Replace(folder, "(guarantee7|test_guarantee)[^\\]*\\bin\\Debug\\net5.0-windows", "guarantee7_smart_contract\bin\Debug\net5.0-windows") & "\guarantee7_smart_contract.exe"

        Dim startInfo As ProcessStartInfo = New ProcessStartInfo(path)

        startInfo.Verb = "runas"

        System.Diagnostics.Process.Start(startInfo)

    End Sub

    Private Sub lbl_membership_Click(sender As Object, e As EventArgs) Handles lbl_membership.Click

        Call GRT.generate_super_account_membership_form.exe(wv, lbl_login, lbl_membership)

    End Sub

    Private Sub lbl_login_Click(sender As Object, e As EventArgs) Handles lbl_login.Click

        Call GRT.generate_login_form.exe("guarantee7", wv, lbl_login, lbl_login_state, lbl_membership)

    End Sub

    Private Sub lbl_add_account_Click(sender As Object, e As EventArgs) Handles lbl_add_account.Click

        Call GRT.generate_add_account_form.exe(wv, lbl_add_account)

    End Sub

    Private Sub btn_reset_Click(sender As Object, e As EventArgs) Handles btn_reset.Click

        Dim confirmResult = MessageBox.Show("Are you sure to reset all?", "Confirm reset all", MessageBoxButtons.YesNo)

        If confirmResult = DialogResult.Yes Then

            Call reset.exe("all")

        End If

    End Sub

    Private Sub btn_reset_procedure_Click(sender As Object, e As EventArgs) Handles btn_reset_procedure.Click

        Call reset.exe("procedure")

    End Sub

    Private Sub btn_reset_smart_contract_Click(sender As Object, e As EventArgs) Handles btn_reset_smart_contract.Click
        Call reset.exe_smart_contract(CLng(txt_block_number.Text.Trim))
    End Sub

    Private Sub btn_balance_Click(sender As Object, e As EventArgs) Handles btn_balance.Click
        Call reset.exe_balance()
    End Sub

End Class