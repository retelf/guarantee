Imports Nethereum.Hex.HexTypes
Imports GRT.Coin
Imports GRT.Net

Public Class get_balance

    Public Shared Async Function guarantee(public_key As String, signiture As String, password As String) As Task(Of Decimal)

        Dim balance_guarantee As Decimal

        ' 이 경우는 로그인 단계이므로 아래를 거칠 필요 없다.
        Dim balance_open_case() As Boolean = GRT.caseOpen.checkOpen.balance(public_key)

        If balance_open_case(0) Then

            Using gnet_client As Socket = Gnet.net_sender()

                If balance_open_case(1) Then

                    balance_guarantee = Await Gcoin.GetBalance(gnet_client, public_key)

                Else

                    balance_guarantee = Await Gcoin.GetBalance(gnet_client, public_key, password, signiture)

                End If

            End Using

        Else

            'MessageBox.Show("계정이 존재하지 않습니다.")

            balance_guarantee = -1

        End If

        Return balance_guarantee

    End Function

    Public Shared Async Function ethereum(public_key As String, ethereum_mainnet_connection_project_id As String) As Task(Of HexBigInteger)

        Dim web3 As Web3 = New Web3("https://mainnet.infura.io/v3/" & ethereum_mainnet_connection_project_id)

        Dim balance_ether As HexBigInteger = Await web3.Eth.GetBalance.SendRequestAsync(public_key)

        Return balance_ether

    End Function

End Class
