Public Class cb_coin_name_SelectedIndexChanged
    Public Shared Sub exe()

        GR.control.rb_account_exist.Checked = False
        GR.control.rb_account_not_exist.Checked = False

        If GR.control.cb_coin_name.SelectedItem.ToString = "guarantee" Then
            If GRT.GR.account.login_state_guarantee Then
                GR.control.btn_coin_login.Text = "guarantee 로그아웃"
            Else
                GR.control.btn_coin_login.Text = "guarantee 로그인"
            End If
        ElseIf GR.control.cb_coin_name.SelectedItem.ToString = "ethereum" Then
            If GR.login_state_ethereum Then
                GR.control.btn_coin_login.Text = "ethereum 로그아웃"
            Else
                GR.control.btn_coin_login.Text = "ethereum 로그인"
            End If
        End If

        GR.control.txt_public_key.Text = ""
        GR.control.txt_coin_password.Text = ""

    End Sub

End Class
