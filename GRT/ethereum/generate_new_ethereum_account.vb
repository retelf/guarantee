Imports Nethereum.Web3
Imports Nethereum.Web3.Accounts

Public Class generate_new_ethereum_account

    Public Shared Async Function exe(password As String) As Task(Of String())

        Dim ipcClient = New Nethereum.JsonRpc.IpcClient.IpcClient("./geth.ipc")

        Dim web3 = New Nethereum.Web3.Web3(ipcClient)

        Dim public_key = Await web3.Personal.NewAccount.SendRequestAsync("#cyndi$36%")

        Dim private_key = GRT.decrypt_keystore_file.exe("ethereum", password, public_key)

        Return {public_key, private_key}

    End Function

End Class
