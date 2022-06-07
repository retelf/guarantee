Public Class geth

    Public Shared Function _start() As Process

        Dim path As String =
            Regex.Replace(Directory.GetCurrentDirectory, "guarantee7\\guarantee7.+", "guarantee7\ethereum\ethereum\bin\Debug\ethereum.exe")

        Dim startInfo As ProcessStartInfo = New ProcessStartInfo(path)

        startInfo.Verb = "runas"

        Dim process = System.Diagnostics.Process.Start(startInfo)

        GRT.GR.ethereum.web3 = New Web3

        Return process

    End Function
    Public Shared Async Function _stop() As Task

        Dim Web3Geth = New Web3Geth("http://localhost:" & GR.ethereum.rpc_port_number)

        Dim mineResult = Await Web3Geth.Miner.Stop.SendRequestAsync()

    End Function

End Class
