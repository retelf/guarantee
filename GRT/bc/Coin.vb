Imports System.Net.Sockets
Imports MySqlConnector
Imports System.Text
Imports System.ComponentModel
Imports Nethereum.Signer

Public Class Coin

    Public Class Gcoin

        Public Shared Async Function GetBalance(socket As Socket, publicKey As String, Optional password As String = "", Optional signiture As String = "") As Task(Of Decimal)

            Dim bytes As Byte() = New Byte(40000) {}

            Dim total_str As String = "coin" & "|*|" & "balance" & "|*|" & publicKey & "|*|" & password & "|*|" & signiture

            Dim byte_message_to_send As Byte() = Encoding.ASCII.GetBytes(total_str & "<#EOF%>")

            Dim async_exe = Task.Run(Sub() socket.Send(byte_message_to_send))

            Await async_exe

            ' 서버에서 할 일
            ' 먼저 public_open 을 확인한다.
            ' public_open 인 경우 곧바로 서버에 보내고
            ' public_open 이 아닌 경우 검증절차를 거친다.
            ' signiture 는 "password" 에 대한 signiture 이다.

            Dim bytesRec As Integer = socket.Receive(bytes)

            Return CDec(Encoding.ASCII.GetString(bytes, 0, bytesRec))

        End Function

    End Class

End Class
