Imports System.Threading
Imports Nethereum.Web3
Imports Nethereum
Imports Nethereum.Hex.HexTypes

Public Class server
    'Private Sub Form_Resize(ByVal sender As Object, ByVal e As EventArgs)
    '    wv.Size = Me.ClientSize - New System.Drawing.Size(wv.Location) - New Size(New Point(20, 20))
    'End Sub

    Public Sub New()

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.

        GR.txt_monitor = Me.txt_monitor

        GR.control.wv = Me.wv
        GR.control.lbl_login = Me.lbl_login
        GR.control.lbl_login_state = Me.lbl_login_state
        GR.control.lbl_membership = Me.lbl_membership

        'AddHandler Me.Resize, New System.EventHandler(AddressOf Form_Resize)

        Call Initialize_webview() ' 반드시 필요하다.

        Call GRT.set_initial_variables.exe(Me.[GetType]().Name)

        Call GRT.pending_filter_management.exe()

    End Sub

    Private Async Sub Initialize_webview()

        Await wv.EnsureCoreWebView2Async(Nothing)

    End Sub

    Private Sub btn_start_Click(sender As Object, e As EventArgs) Handles btn_start.Click

        If GRT.GR.account.login_state = "guarantee" Then

            'Call GRT.set_initial_variables.exe(Me.[GetType]().Name)

            GRT.GR.server_private_key = "0x" & GRT.get_server_private_key.exe(txt_server_password.Text.Trim)

            If Not GRT.GR.server_private_key = "password_not_match" Then

                GR.syncro_state = True ' 일단은 싱크로 체킹을 해야 한다.

                'Dim Thread_guarantee_server As Thread = New Thread(AddressOf guarantee_server_thread.exe) : Thread_guarantee_server.Start()
                Dim Thread_guarantee_server As Thread = New Thread(AddressOf guarantee_server_async_thread.exe) : Thread_guarantee_server.Start()
                Dim Thread_guarantee_agency As Thread = New Thread(AddressOf guarantee_server_async_thread_agency.exe) : Thread_guarantee_agency.Start()

                Dim Thread_guarantee_ethereum As Thread = New Thread(AddressOf guarantee_server_async_thread_ethereum.exe) : Thread_guarantee_ethereum.Start()
                Dim Thread_guarantee_management As Thread = New Thread(AddressOf guarantee_server_async_thread_management.exe) : Thread_guarantee_management.Start()
                Dim Thread_guarantee_nft As Thread = New Thread(AddressOf guarantee_server_async_thread_nft.exe) : Thread_guarantee_nft.Start()

                'Dim Thread_guarantee_web As Thread = New Thread(AddressOf guarantee_server_async_thread_web.exe) : Thread_guarantee_web.Start()

            Else

                MessageBox.Show("password_not_match")

            End If

        Else

            MessageBox.Show("먼저 개런티 계정으로 로그인을 하시기 바랍니다.")

            Return

        End If

    End Sub

    Private Sub lbl_login_Click(sender As Object, e As EventArgs) Handles lbl_login.Click

        Call GRT.generate_login_form.exe("server", wv, lbl_login, lbl_login_state, lbl_membership)

    End Sub

    Private Sub lbl_add_account_Click(sender As Object, e As EventArgs) Handles lbl_add_account.Click

        Call GRT.generate_add_account_form.exe(wv, lbl_add_account)

    End Sub

    Private Sub lbl_membership_Click(sender As Object, e As EventArgs) Handles lbl_membership.Click

        Call GRT.generate_super_account_membership_form.exe(wv, lbl_login, lbl_membership)

    End Sub

End Class