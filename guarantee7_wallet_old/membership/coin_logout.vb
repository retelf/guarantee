Public Class coin_logout

    Public Shared Sub exe()

        If GR.control.cb_coin_name.SelectedItem.ToString = "guarantee" Then
            GRT.GR.account.login_state_guarantee = False
            GR.control.btn_coin_login.Text = "guarantee 로그인"
            GR.current_guarantee_password = ""
        ElseIf GR.control.cb_coin_name.SelectedItem.ToString = "ethereum" Then
            GR.login_state_ethereum = False
            GR.control.btn_coin_login.Text = "ethereum 로그인"
            GR.current_ethereum_password = ""
        End If

        If GRT.GR.account.login_state_guarantee = True Then

            GR.control.lbl_login_state.Text = "현재 guarantee에만 로그인 되어 있습니다"

        ElseIf GR.login_state_ethereum = True Then

            GR.control.lbl_login_state.Text = "현재 ethereum에만 로그인 되어 있습니다"

        Else

            GR.control.lbl_login_state.Text = "로그인이 필요합니다"

        End If

        GR.control.txt_public_key.Text = ""
        GR.control.txt_coin_password.Text = ""

    End Sub

End Class
