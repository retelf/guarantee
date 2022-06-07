Imports Nethereum.Hex.HexTypes
Imports GRT.Security

Public Class coin_login_member

    Public Shared Async Sub exe()

        Dim coin_name, coin_password, public_key, private_key, key_pair(), signiture As String
        Dim balance_guarantee As Decimal
        Dim balance_ether As HexBigInteger

        coin_name = GR.control.cb_coin_name.SelectedItem.ToString
        coin_password = GR.control.txt_coin_password.Text

        If Regex.Match(GR.control.btn_coin_login.Text, "로그인$").Success Then

            coin_name = GR.control.cb_coin_name.SelectedItem.ToString
            public_key = GR.control.txt_public_key.Text.Trim
            private_key = GRT.decrypt_keystore_file.exe(coin_name, coin_password, public_key).ToLower

            If coin_login.exe() Then

                If coin_name = "guarantee" Then

                    signiture = Gsign.sign(coin_password, private_key)

                    balance_guarantee = Await GRT.get_balance.guarantee(public_key, signiture, GR.control.txt_coin_password.Text.Trim)

                    GR.control.lbl_message.Text = "guarantee 현재잔고 : " & balance_guarantee.ToString

                Else

                    balance_ether = Await GRT.get_balance.ethereum(public_key, GR.ethereum_mainnet_connection_project_id)

                    GR.control.lbl_message.Text = "ethereum 현재잔고 : " & balance_ether.ToString

                End If

                GR.control.lbl_message.Visible = True

            End If

        ElseIf Regex.Match(GR.control.btn_coin_login.Text, "계정생성$").Success Then

            ' 먼저 비밀번호 중복 체크

            If coin_password = GR.current_initial_password Then
                MessageBox.Show("회원가입에 사용한 비밀번호는 개별 코인 비밀번호로 사용할 수 없습니다.")
                Return
            End If

            If GR.control.rb_open_infomation.Checked = False And GR.control.rb_not_open_infomation.Checked = False Then
                MessageBox.Show("정보공개 여부를 선택해 주시기 바랍니다.")
                Return
            End If

            ' 키파일 생성

            Dim ecKey As Nethereum.Signer.EthECKey = Nethereum.Signer.EthECKey.GenerateKey()

            Call GRT.generate_key_file.exe(coin_name, coin_password, ecKey)

            key_pair = {ecKey.GetPublicAddress(), ecKey.GetPrivateKey}

            ' 계정 생성

            Call GRT.generate_eoa_account.exe(GR.current_id, GR.control.cb_coin_name.SelectedItem.ToString, key_pair(0), GR.control.rb_open_infomation.Checked)

            ' 최종적 표시

            GR.control.txt_public_key.Text = key_pair(0)
            GR.control.txt_public_key.Enabled = True
            GR.control.txt_private_key.Text = key_pair(1)
            GR.control.lbl_private_key.Visible = True
            GR.control.txt_private_key.Visible = True

        Else
            Call coin_logout.exe()
        End If

    End Sub

End Class
