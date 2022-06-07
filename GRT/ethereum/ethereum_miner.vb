Public Class ethereum_miner

    Public Shared Async Function _start() As Task

        Dim Web3Geth = New Web3Geth("http://localhost:" & GR.ethereum.rpc_port_number)

        Dim mineResult = Await Web3Geth.Miner.Start.SendRequestAsync(2)

    End Function
    Public Shared Async Function _stop() As Task

        Dim Web3Geth = New Web3Geth("http://localhost:" & GR.ethereum.rpc_port_number)

        Dim mineResult = Await Web3Geth.Miner.Stop.SendRequestAsync()

    End Function

End Class
