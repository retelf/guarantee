Imports Nethereum.Hex.HexTypes
Imports System.Threading

Public Class pending_filter_management

    Public Shared pendingFilter As HexBigInteger
    Public Shared prepare_next_filter_minute As Integer = 1

    Public Shared Sub exe()

        Dim thread_refresh_filter As Thread = New Thread(AddressOf refresh_filter)

        thread_refresh_filter.Start()

    End Sub

    Shared Async Sub refresh_filter() ' 언제나 5분 전 이전 것으로 유지.

        Dim web3 = New Web3()

        Dim next_filter As HexBigInteger

        pendingFilter = Await web3.Eth.Filters.NewPendingTransactionFilter.SendRequestAsync()

        While True

            Thread.Sleep(1000 * 120 * prepare_next_filter_minute)

            next_filter = Await web3.Eth.Filters.NewPendingTransactionFilter.SendRequestAsync() ' 2분 경과 후 준비

            Thread.Sleep(1000 * 60 * prepare_next_filter_minute)

            pendingFilter = next_filter ' 2분 경과 후 1분 전의 것으로 교체

        End While

    End Sub

End Class
