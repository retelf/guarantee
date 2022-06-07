Imports System.Threading
Imports Nethereum.Web3
Imports Nethereum.Web3.Accounts
Imports Nethereum.Geth
Imports Nethereum.Quorum
Imports Nethereum.Contracts
Imports Nethereum.Signer
Imports Nethereum.Signer.TransactionFactory
Imports Nethereum.Web3.Accounts.Managed
Imports Nethereum.Hex.HexConvertors.Extensions
Imports Nethereum.KeyStore
Imports Nethereum.Hex.HexConvertors
Imports Nethereum.Hex.HexTypes
Imports Nethereum.RPC.NonceServices
Imports Nethereum.RPC.Web3
Imports Nethereum.RPC.TransactionReceipts
Imports System.Threading.Tasks
Imports Nethereum.RPC.Eth.Transactions
Imports Nethereum.RPC.Eth.DTOs
Imports Nethereum.Contracts.TransactionHandlers
Imports Newtonsoft.Json.Linq
Imports System.Numerics
Imports Nethereum.RPC.TransactionManagers
Imports MySqlConnector
Imports Newtonsoft.Json
Imports System.Text.RegularExpressions
Imports System.Text
Imports Nethereum.ABI.ABIType

Public Class test

    Sub New()

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.

        Call GRT.set_initial_variables.exe(Me.[GetType]().Name)

    End Sub
    Private Sub btn_socket_listener_Click(sender As Object, e As EventArgs) Handles btn_socket_listener.Click

        'Dim a = "USE bc_nft;INSERT INTO sell_order(`block_number`,`owner_portion_block_number`, `nfa`, `token_id`, `pieces`, `confirmed_type`, `price`, `auto_recall`, `currency`, `max_split`, `eoa`, `na`, `exchange_name`, `domain`, `ip`, `port`, `days_span`, `closing_time`, `transaction_fee_rate`, `state`, `idate`)VALUES( 0, 206, '0BeBF09EEd7F722B304ABa9160ecd375E4580d45', 0, 999000, 'confirmed', 1, True, 'guarantee', 1000000, '12c98cbe86B5e541391e58674398bEb529C3A1Bf', '962921D70e02e6c5970112579f6e6F2e9d0b1b94', 'bithumb', 'movism.com', '221.150.221.16', 40700, 1, '0000-00-00 00:00:00', 0.002, 'alive', '2022-02-15 02:17:29.495624');USE bc_nft; UPDATE owner_portion SET `pieces_confirmed` = `pieces_confirmed` - 999000, `pieces_confirmed_ordered` = `pieces_confirmed_ordered` + 999000 WHERE `nfa` = '0BeBF09EEd7F722B304ABa9160ecd375E4580d45' AND `eoa` = '12c98cbe86B5e541391e58674398bEb529C3A1Bf' AND `token_id` = 0;"
        'Dim b = "USE bc_nft;INSERT INTO sell_order(`block_number`,`owner_portion_block_number`, `nfa`, `token_id`, `pieces`, `confirmed_type`, `price`, `auto_recall`, `currency`, `max_split`, `eoa`, `na`, `exchange_name`, `domain`, `ip`, `port`, `days_span`, `closing_time`, `transaction_fee_rate`, `state`, `idate`)VALUES( 0, 206, '0BeBF09EEd7F722B304ABa9160ecd375E4580d45', 0, 999000, 'confirmed', 1, True, 'guarantee', 1000000, '12c98cbe86B5e541391e58674398bEb529C3A1Bf', '962921D70e02e6c5970112579f6e6F2e9d0b1b94', 'bithumb', 'movism.com', '221.150.221.16', 40700, 1, '0000-00-00 00:00:00', 0.002, 'alive', '2022-02-15 02:17:29.495624');USE bc_nft; UPDATE owner_portion SET `pieces_confirmed` = `pieces_confirmed` - 999000, `pieces_confirmed_ordered` = `pieces_confirmed_ordered` + 999000 WHERE `nfa` = '0BeBF09EEd7F722B304ABa9160ecd375E4580d45' AND `eoa` = '12c98cbe86B5e541391e58674398bEb529C3A1Bf' AND `token_id` = 0;"

        'If a = b Then
        '    Dim aa = ""
        'Else
        '    Dim bb = ""
        'End If

        Call FunctionEncodingDecoding.RlpDecode("f86c82020e8459682f008252089412c98cbe86b5e541391e58674398beb529c3a1bf87038d7ea4c68000802ca01f72e5a955f7e868b9f54a2f97144b2ec5d04e7a13a4a0463246f61b297380b5a003b7b95aaa4821fc9a22908db7d8f19a0910b4a2d7f03518ff845de72fa322f0")

    End Sub

    Private Sub btn_socket_sender_Click(sender As Object, e As EventArgs) Handles btn_socket_sender.Click

        Dim JSS = GRT.make_json_string.exe(
                        {{"key", "test", "quot"}},
                        {
                        {"sleeping_time", CStr(10), "non_quot"},
                        {"value", "0", "quot"}}, True)

        Dim JRS = GRT.socket_client.exe(GRT.GR.address_server_local_agency, GRT.GR.port_number_server_local_agency, GRT.GR.port_number_server_local, JSS)

        JSS = GRT.make_json_string.exe(
                        {{"key", "test", "quot"}},
                        {
                        {"sleeping_time", CStr(0), "non_quot"},
                        {"value", "1", "quot"}}, True)

        JRS = GRT.socket_client.exe(GRT.GR.address_server_local_agency, GRT.GR.port_number_server_local_agency, GRT.GR.port_number_server_local, JSS)

    End Sub

    Private Async Sub btn_new_account_Click(sender As Object, e As EventArgs) Handles btn_new_account.Click

        Dim data = "abc"
        Dim public_key = "0x20f3F16B3FB97Acd0352ae1Ef3aBc131BC0C8132"
        Dim private_key = "0x9699f2889d4c9d95b439c6931d02de060b6924925a3071caa63fb3806105e92d"
        Dim signature = GRT.Security.Gsign.sign(data, public_key)
        Dim bool = GRT.Security.Gverify.verify(data, signature, private_key)
        'Dim bool = GRT.Security.Gverify.verify(data, signature, public_key)
        'Dim bool = GRT.Security.Gverify.verify(Regex.Replace(public_key, "^0x", ""), signature, data)

    End Sub

    Public Shared Function shared_test() As String

        Thread.Sleep(1000)

        MessageBox.Show("x")

    End Function

    Private Async Sub btn_SendRawTransaction_Click(sender As Object, e As EventArgs) Handles btn_SendRawTransaction.Click

        Dim web3 = New Web3()
        Dim filterChanges() As String

        Dim pendingFilter As HexBigInteger = Await Web3.Eth.Filters.NewPendingTransactionFilter.SendRequestAsync()

        Threading.Thread.Sleep(10000)

        filterChanges = Await web3.Eth.Filters.GetFilterChangesForBlockOrTransaction.SendRequestAsync(pendingFilter)

    End Sub

    Private Async Sub btn_get_balance_Click(sender As Object, e As EventArgs) Handles btn_get_balance.Click

        Dim balance_mainnet_foundation, balance_mainnet_me, balance_mainnet_new_me, balance_mainnet_new2_me, balance_mainnet_new3_me, balance_mainnet_change,
            balance_rinkbey_me, balance_rinkbey_new_me, balance_rinkbey_new2_me, balance_rinkbey_foundation, balance_rinkbey_new3_me, balance_rinkbey_change,
            balance_local_foundation, balance_local_me, balance_local_new_me, balance_local_new2_me, balance_local_new3_me, balance_local_change As HexBigInteger

        Dim web3 = New Web3()

        web3 = New Web3("https://mainnet.infura.io/v3/a7a01158fe384d85b605cd5ba0b53bc3")

        balance_mainnet_foundation = Await web3.Eth.GetBalance.SendRequestAsync("0xde0b295669a9fd93d5f28d9ec85e40f4cb697bae") ' 메인넷 파운데이션

        balance_mainnet_me = Await web3.Eth.GetBalance.SendRequestAsync("0xe58a43B5b46b91184467FD2e5b594B4441682126")

        balance_mainnet_new_me = Await web3.Eth.GetBalance.SendRequestAsync("0xbd3058ffab6c7afb83b9ca99ee9e1cd843af060a")

        balance_mainnet_new2_me = Await web3.Eth.GetBalance.SendRequestAsync("0x653b54cC2eF7bFB08956951FeF4646bE9DDAaa02")

        balance_mainnet_new3_me = Await web3.Eth.GetBalance.SendRequestAsync("0x5487fbb990ae0043e8a451661195e7806dd500a4")

        balance_mainnet_change = Await web3.Eth.GetBalance.SendRequestAsync("0x0ca19fd64c08f3dfc5fc3080597d85b04cbfbf93")

        web3 = New Web3("https://rinkeby.infura.io/v3/9aa3d95b3bc440fa88ea12eaa4456161")

        balance_rinkbey_foundation = Await web3.Eth.GetBalance.SendRequestAsync("0xde0b295669a9fd93d5f28d9ec85e40f4cb697bae") ' 링케비 파운데이션 0x00631df1b81e097c8d2dc16bc0d07775b8e73473

        balance_rinkbey_me = Await web3.Eth.GetBalance.SendRequestAsync("0xe58a43B5b46b91184467FD2e5b594B4441682126")

        balance_rinkbey_new_me = Await web3.Eth.GetBalance.SendRequestAsync("0xbd3058ffab6c7afb83b9ca99ee9e1cd843af060a")

        balance_rinkbey_new2_me = Await web3.Eth.GetBalance.SendRequestAsync("0x653b54cC2eF7bFB08956951FeF4646bE9DDAaa02")

        balance_rinkbey_new3_me = Await web3.Eth.GetBalance.SendRequestAsync("0x5487fbb990ae0043e8a451661195e7806dd500a4")

        balance_local_new3_me = Await web3.Eth.GetBalance.SendRequestAsync("0x653b54cC2eF7bFB08956951FeF4646bE9DDAaa02")

        balance_rinkbey_change = Await web3.Eth.GetBalance.SendRequestAsync("0x0ca19fd64c08f3dfc5fc3080597d85b04cbfbf93")

        balance_rinkbey_me = Await web3.Eth.GetBalance.SendRequestAsync("0x00631df1b81e097c8d2dc16bc0d07775b8e73473")

        web3 = New Web3()

        balance_local_foundation = Await web3.Eth.GetBalance.SendRequestAsync("0xde0b295669a9fd93d5f28d9ec85e40f4cb697bae") ' 로컬 파운데이션

        balance_local_me = Await web3.Eth.GetBalance.SendRequestAsync("0xe58a43B5b46b91184467FD2e5b594B4441682126")

        balance_local_new_me = Await web3.Eth.GetBalance.SendRequestAsync("0xbd3058ffab6c7afb83b9ca99ee9e1cd843af060a")

        balance_local_new2_me = Await web3.Eth.GetBalance.SendRequestAsync("0x653b54cC2eF7bFB08956951FeF4646bE9DDAaa02") ' 마구잡이

        balance_local_new3_me = Await web3.Eth.GetBalance.SendRequestAsync("0x5487fbb990ae0043e8a451661195e7806dd500a4")

        balance_local_change = Await web3.Eth.GetBalance.SendRequestAsync("0x0ca19fd64c08f3dfc5fc3080597d85b04cbfbf93")

        Dim aa = ""

    End Sub

    Private Async Sub btn_new_key_Click(sender As Object, e As EventArgs) Handles btn_new_key.Click

        Dim node() As String = {
        "0x5487fbb990ae0043e8a451661195e7806dd500a4",
        "0xd233120B00c7D2AcDb17A03aF08D70D1Fe31e6cb", ' 233
        "0xceBd5A5b38477f3BF84CE636a40bdfc2163c6201", ' 246
        "0xfB489D2410068608A46a2cDda8950817dF55D16b"} ' 248

        Dim block_number() As Long = {-4, -3, -2, -1}

        Dim cString_0 As String = "server=filmasfilm.com;port=30601;database=bc;uid=guarantee7;pwd=0x7aF87Ad373C2e7b9875CaADF44c0Cef78D069105;CharSet=utf8;"
        Dim cString_1 As String = "server=192.168.0.253;port=30601;database=bc;uid=guarantee7;pwd=0x24bCaCBC32338a253fA62BE4D062FcC7A1660A87;CharSet=utf8;"
        Dim cString_2 As String = "server=filmasfilm.com;port=30602;database=bc;uid=guarantee7;pwd=0xafD2D6DdD5Bb60f121f20Bf598B777578a486066;CharSet=utf8;" ' 이것은 이제 안된다. server=filmasfilm.com 으로 바꾸어 주어야 하는데 포트가 중복된다.
        Dim cString_3 As String = "server=192.168.0.248;port=30601;database=bc;uid=guarantee7;pwd=0x84A9f63A3dc2eBADC76dE566b5F58b021C072631;CharSet=utf8;"

        Dim cString() As String = {cString_0, cString_1, cString_2, cString_3}

        For h = 0 To cString.Length - 1

            Dim Connection_sample_database = New MySqlConnection(cString(h))

            Dim Insertcmd As MySqlCommand

            Connection_sample_database.Open()

            ''''''' Call make_sample_database.exe(Connection_sample_database, cString(1), cString(0)) ' 이것은 앞으로 더 이상 필요없을 것이다.

            Dim pure_query As String

            Dim idate_string = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.ffffff", Globalization.CultureInfo.InvariantCulture)

            For i = 0 To node.Length - 1

                For j = 0 To GRT.GR.coin_name.Length - 1

                    pure_query = "INSERT IGNORE INTO account(" &
                        "`block_number`," &
                        " `eoa`," &
                        " `eoa_type`," &
                        " `coin_name`," &
                        " `balance`," &
                        " `locked`," &
                        " `idate`)" &
                        "VALUES(" &
                        " " & block_number(i) & "," &
                        " '" & Regex.Replace(node(i), "^0x", "") & "'," &
                        " 'na'," &
                        " '" & GRT.GR.coin_name(j) & "'," &
                        " 0," &
                        " 0," &
                        " '" & idate_string & "');"

                    Insertcmd = New MySqlCommand(pure_query, Connection_sample_database)
                    Insertcmd.CommandType = CommandType.Text

                    Insertcmd.ExecuteNonQuery()

                Next

            Next

        Next

        MessageBox.Show("ok")

    End Sub

    Private Async Sub btn_offline_Click(sender As Object, e As EventArgs) Handles btn_offline.Click

        '' 메인넷
        'Dim eoa_from = "0x5487fbb990ae0043e8a451661195e7806dd500a4"
        'Dim private_key = "0xba7c9fc1cb05b53bc1699d94072e2438e433edb4301a48db10e1054116cc4411"
        'Dim eoa_to = "0xe58a43B5b46b91184467FD2e5b594B4441682126"

        '링케비
        Dim eoa_from = "0x5487fbb990ae0043e8a451661195e7806dd500a4"
        Dim private_key = "0xba7c9fc1cb05b53bc1699d94072e2438e433edb4301a48db10e1054116cc4411"
        Dim eoa_to = "0xe58a43B5b46b91184467FD2e5b594B4441682126"

        Dim amount, gasPrice, gas As BigInteger
        Dim amount_eth As Decimal

        amount_eth = 0.00008

        Dim account = New Account(private_key, GRT.GR.account.ethereum_chain_id)

        Dim callInput = CType(EtherTransferTransactionInputBuilder.CreateTransactionInput(eoa_from, eoa_to, amount_eth), CallInput)
        amount = amount_eth * 1000000000000000000
        gasPrice = Await GRT.GR.ethereum.web3.Eth.GasPrice.SendRequestAsync
        'gas = GRT.GR.ethereum.web3.Eth.TransactionManager.EstimateGasAsync(callInput).Result

        account.NonceService = New InMemoryNonceService(account.Address, GRT.GR.ethereum.web3.Client)
        Dim currentNonce = Await GRT.GR.ethereum.web3.Eth.Transactions.GetTransactionCount.SendRequestAsync(account.Address, BlockParameter.CreatePending())
        Dim futureNonce = Await account.NonceService.GetNextNonceAsync()

        Dim OfflineTransactionSigner = New TransactionSigner()

        Dim encoded = OfflineTransactionSigner.SignTransaction(private_key, GRT.GR.account.ethereum_chain_id, eoa_to, amount, futureNonce, gasPrice, 21000)

        'web3 = New Web3("http://localhost:8545")

        'Dim transaction_hash = Await web3.Eth.Transactions.SendRawTransaction.SendRequestAsync("0x" & encoded)

        Dim aa = ""

    End Sub
End Class
