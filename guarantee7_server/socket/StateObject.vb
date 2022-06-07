Imports System.Net.Sockets
Imports System.Text

Public Class StateObject

    Public Const BufferSize As Integer = 1024
    Public buffer As Byte() = New Byte(BufferSize - 1) {}
    Public sb As StringBuilder = New StringBuilder()
    Public workSocket As Socket = Nothing

End Class
