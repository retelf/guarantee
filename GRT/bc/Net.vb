Public Class Net

    Public Class Gnet

        Public Shared Function net_sender(remote_domain As String, remote_port As Integer, local_domain As String, local_port As Integer) As Socket

            'If remote_domain = "movism.com" And remote_port = 40702 Then
            '    remote_domain = "192.168.0.248"
            'End If

            Dim remote_ipAddress, local_ipAddress As IPAddress
            Dim remote_ipHostInfo, local_ipHostInfo As IPHostEntry
            Dim remote_EndPoint, local_EndPoint As IPEndPoint

            Dim i_number As Integer

            If Not remote_domain = "127.0.0.1" And Regex.Match(remote_domain, "^\d+\.\d+\.\d+\.\d+$").Success Then

                remote_ipAddress = IPAddress.Parse(remote_domain)

            Else

                remote_ipHostInfo = Dns.GetHostEntry(remote_domain)
                'remote_ipHostInfo  = Dns.GetHostEntry("59.12.211.107")

                For i = 0 To remote_ipHostInfo.AddressList.Length - 1

                    If Not Regex.Match(remote_ipHostInfo.AddressList(i).ToString, ":").Success Then
                        i_number = i
                    End If

                Next

                remote_ipAddress = remote_ipHostInfo.AddressList(i_number)

            End If

            remote_EndPoint = New IPEndPoint(remote_ipAddress, remote_port)

            local_ipHostInfo = Dns.GetHostEntry("localhost")

            For i = 0 To local_ipHostInfo.AddressList.Length - 1

                If Not Regex.Match(local_ipHostInfo.AddressList(i).ToString, ":").Success Then
                    i_number = i
                End If

            Next

            local_ipAddress = local_ipHostInfo.AddressList(i_number)
            local_EndPoint = New IPEndPoint(local_ipAddress, local_port)

            Dim sender As Socket = New Socket(local_EndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp)

            sender.Connect(remote_EndPoint) ' Connect 는 포트를 쥐지 않고 임의 포트번호를 선택한다. Bind 와 다르다.

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

        'Public Shared Function net_sender(Optional domain As String = "qs.guarantee7.com", Optional tcpPortNumber As Integer = 40701) As Socket

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
