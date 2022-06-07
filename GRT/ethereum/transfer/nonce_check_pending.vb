Imports Nethereum.RPC.Eth.DTOs
Imports Nethereum.Hex.HexTypes
Imports System.Numerics
Imports Nethereum.RPC.NonceServices

Public Class nonce_check_pending

    Public Shared Async Function exe(public_key As String) As Task(Of Boolean)

        ' 짧은 시간 사이에 들어오는 논스중복 tx 는 그것이 아예 발생하지 않도록 튕겨준다.
        ' 즉 현재 동일인의 tx 가 펜딩 중이면 아예 거래를 하지 못하게 한다.
        ' 이 클래스는 추후 아무래도 안되는 경우에 사용한다.
        ' 가장 문제되는 것은 극도로 낮은 가스비 때문에 롤링하고 있는 tx 이다.

        Dim web3 = New Web3()

        Dim transaction As Transaction

        Dim filterChanges = Await web3.Eth.Filters.GetFilterChangesForBlockOrTransaction.SendRequestAsync(pending_filter_management.pendingFilter)

        Dim pending As Boolean = False

        For i = 0 To filterChanges.Length - 1

            transaction = Await web3.Eth.Transactions.GetTransactionByHash.SendRequestAsync(filterChanges(i))

            If transaction.From.ToLower = public_key.ToLower Then

                pending = True

                Exit For

            End If

        Next

        Return pending

    End Function

End Class
