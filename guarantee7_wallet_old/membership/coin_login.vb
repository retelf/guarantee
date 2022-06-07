Imports Nethereum.Hex.HexTypes

Public Class coin_login

    Public Shared Function exe() As Boolean

        Dim public_key, password, coin_name As String
        Dim eoa_exist, verified As Boolean

        coin_name = GR.control.cb_coin_name.SelectedItem.ToString
        public_key = GR.control.txt_public_key.Text.Trim
        password = GR.control.txt_coin_password.Text.Trim

        eoa_exist = execute_coin_login.exe(public_key, coin_name)

        If eoa_exist Then

            verified = GRT.verify_password.exe(coin_name, password, public_key) ' password 검증

            If Not verified Then
                MessageBox.Show("계정은 존재하지만 비밀번호가 올바르지 않습니다.")
                Return False
            End If

            If coin_name = "guarantee" Then
                GRT.GR.account.login_state_guarantee = True
                GR.current_guarantee_password = password
            ElseIf coin_name = "ethereum" Then
                GR.login_state_ethereum = True
                GR.current_ethereum_password = password
            End If

            If GRT.GR.account.login_state_guarantee = True And GR.login_state_ethereum = True Then
                GR.control.lbl_login_state.Text = "현재 guarantee과 ethereum 모두에 로그인 되어 있습니다"
            ElseIf GRT.GR.account.login_state_guarantee = True Then
                GR.control.lbl_login_state.Text = "현재 guarantee에만 로그인 되어 있습니다"
            ElseIf GR.login_state_ethereum = True Then
                GR.control.lbl_login_state.Text = "현재 ethereum에만 로그인 되어 있습니다"
            End If

            If coin_name = "guarantee" Then
                GR.control.btn_coin_login.Text = "guarantee 로그아웃"
            Else
                GR.control.btn_coin_login.Text = "ethereum 로그아웃"
            End If

            Return True

        Else

            MessageBox.Show(coin_name & " 계정이 존재하지 않습니다. 신규계정을 생성하시기 바랍니다.")
            Return False

        End If

    End Function

End Class
