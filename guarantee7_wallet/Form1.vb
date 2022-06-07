Public Class Form1
    Private Async Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim key, coin_name As String
        Dim signiture, signiture_for_get_balance As String
        Dim JSS, JRS, receipt, pure_query As String
        Dim eoa_from, eoa_to, unit, idate_string As String
        Dim balance, amount, gas As Decimal
        Dim result As Task(Of String)

        Dim process = GRT.geth._start

        'Dim web3 = New Web3("http://movism.com:8545")
        Dim web3 = New Web3("http://localhost:8545")
        'Dim web3 = New Web3("http://192.168.0.253:8545")

        Dim balance_wei = Await web3.Eth.GetBalance.SendRequestAsync("0xe58a43B5b46b91184467FD2e5b594B4441682126")

        Await GRT.ethereum_miner._stop()

        Await GRT.ethereum_miner._start()

        gas = CDec(100)

        Dim transaction = Await web3.Eth.GetEtherTransferService().TransferEtherAndWaitForReceiptAsync(eoa_to, amount, gas)

    End Sub
End Class
