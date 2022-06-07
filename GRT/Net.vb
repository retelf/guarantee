Public Class Net

    Public Class Gnet

        Public Shared Function net_sender(Optional domain As String = "localhost", Optional tcpPortNumber As Integer = -1) As Socket

            Dim remote_ipHostInfo As IPHostEntry = Dns.GetHostEntry("mainnet.guarantee7.com")
            Dim remote_ipAddress As IPAddress = remote_ipHostInfo.AddressList(0)
            Dim remote_EndPoint As IPEndPoint = New IPEndPoint(remote_ipAddress, 40701)

            Dim local_ipHostInfo As IPHostEntry = Dns.GetHostEntry("localhost")
            Dim local_ipAddress As IPAddress = local_ipHostInfo.AddressList(1)
            Dim local_EndPoint As IPEndPoint = New IPEndPoint(local_ipAddress, 40702)

            Dim sender As Socket = New Socket(local_EndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp)

            sender.Connect(remote_EndPoint)

            Return sender

        End Function

        Public Shared Function net_listener(Optional tcpPortNumber As Integer = 40701, Optional queue_limit As Integer = 10000) As Socket

            'Dim ipHostInfo As IPHostEntry = Dns.GetHostEntry(Dns.GetHostName())
            'Dim ipAddress As IPAddress = ipHostInfo.AddressList(1)
            'Dim localEndPoint As IPEndPoint = New IPEndPoint(ipAddress, 40707)
            'Dim listener As Socket = New Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp)

            'listener.Bind(localEndPoint)
            'listener.Listen(10000)

            Dim ipHostInfo As IPHostEntry = Dns.GetHostEntry(Dns.GetHostName())
            Dim ipAddress As IPAddress = ipHostInfo.AddressList(1)
            Dim localEndPoint As IPEndPoint = New IPEndPoint(ipAddress, tcpPortNumber)
            Dim listener As Socket = New Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp)

            listener.Bind(localEndPoint)
            listener.Listen(queue_limit)

            Return listener

        End Function

        'Public Shared Function net_sender(Optional domain As String = "mainnet.guarantee7.com", Optional tcpPortNumber As Integer = 40701) As Socket

        '    Dim remote_ipHostInfo As IPHostEntry = Dns.GetHostEntry(domain)
        '    Dim remote_ipAddress As IPAddress = remote_ipHostInfo.AddressList(0)
        '    Dim remote_EndPoint As IPEndPoint = New IPEndPoint(remote_ipAddress, tcpPortNumber)

        '    Dim local_ipHostInfo As IPHostEntry = Dns.GetHostEntry("localhost")
        '    Dim local_ipAddress As IPAddress = local_ipHostInfo.AddressList(1)
        '    Dim local_EndPoint As IPEndPoint = New IPEndPoint(local_ipAddress, tcpPortNumber)

        '    Dim sender As Socket = New Socket(local_EndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp)

        '    sender.Connect(remote_EndPoint)

        '    Return sender

        'End Function

        'Public Shared Function net_listener(Optional tcpPortNumber As Integer = 40701, Optional queue_limit As Integer = 10000) As Socket

        '    'Dim ipHostInfo As IPHostEntry = Dns.GetHostEntry(Dns.GetHostName())
        '    'Dim ipAddress As IPAddress = ipHostInfo.AddressList(1)
        '    'Dim localEndPoint As IPEndPoint = New IPEndPoint(ipAddress, 40707)
        '    'Dim listener As Socket = New Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp)

        '    'listener.Bind(localEndPoint)
        '    'listener.Listen(10000)

        '    Dim ipHostInfo As IPHostEntry = Dns.GetHostEntry(Dns.GetHostName())
        '    Dim ipAddress As IPAddress = ipHostInfo.AddressList(1)
        '    Dim localEndPoint As IPEndPoint = New IPEndPoint(ipAddress, tcpPortNumber)
        '    Dim listener As Socket = New Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp)

        '    listener.Bind(localEndPoint)
        '    listener.Listen(queue_limit)

        '    Return listener

        'End Function

    End Class

End Class
