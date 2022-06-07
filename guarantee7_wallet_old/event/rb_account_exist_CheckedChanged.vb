Public Class rb_account_exist_CheckedChanged
    Public Shared Sub exe()

        If GR.control.rb_account_exist.Checked Then

            GR.control.txt_public_key.Enabled = True
            GR.control.txt_coin_password.Enabled = True

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
                    GR.control.btn_coin_login.Text = "ethereum로그인"
                End If
            End If

            GR.control.pnl_open_information.Visible = False

            '===================================================================

            GR.control.txt_coin_password.Text = "a1025102"
            GR.control.txt_public_key.Text = "0xb79161d9DafBdCfee04aa2E2d87Db5863A45F277"

            '===================================================================

        Else
            GR.control.txt_public_key.Enabled = False
            If GR.control.cb_coin_name.SelectedItem.ToString = "guarantee" Then
                GR.control.btn_coin_login.Text = "guarantee 계정생성"
            Else
                GR.control.btn_coin_login.Text = "ethereum 계정생성"
            End If

            GR.control.pnl_open_information.Visible = True
        End If

    End Sub

End Class
