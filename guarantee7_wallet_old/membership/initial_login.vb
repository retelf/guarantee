Public Class initial_login

    Public Shared Sub exe()

        Dim login_success As Boolean = execute_initial_login.exe(GR.control.txt_initial_public_key.Text.Trim)

        If login_success Then

            GR.control.lbl_login_state.Visible = True

            GR.control.pnl_coin_login.Visible = True

        Else

            MessageBox.Show("계정이 존재하지 않거나 비밀번호가 올바르지 않습니다.")

            Return

        End If

    End Sub

End Class
