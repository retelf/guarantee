Imports System.Net.NetworkInformation
Imports System.ServiceProcess

Public Class port

    Public Shared Function open(rule_name As String, port_number As Integer) As Boolean

        If check_port_available(port_number) Then

            Call GRT.RunShellCommand.exe("netsh.exe", "advfirewall firewall add rule " & "name = " & rule_name & " dir = in action = allow protocol = TCP localport = " & port_number, "", "")
            Call GRT.RunShellCommand.exe("netsh.exe", "advfirewall firewall add rule " & "name = " & rule_name & " dir = out action = allow protocol = TCP localport = " & port_number, "", "")

            Return True

        Else
            Return False
        End If

    End Function

    Public Shared Sub close(rule_name As String, port_number As Integer)

        Call GRT.RunShellCommand.exe("netsh.exe", "advfirewall firewall delete rule " & "name = " & rule_name & " protocol = TCP localport = " & port_number, "", "")
        Call GRT.RunShellCommand.exe("netsh.exe", "advfirewall firewall delete rule " & "name = " & rule_name & " protocol = TCP localport = " & port_number, "", "")

    End Sub

    Public Shared Function check_port_available(port_number As Integer) As Boolean

        Dim isAvailable As Boolean = True

        Dim ip_str As String = ""

        Dim ipGlobalProperties As IPGlobalProperties = IPGlobalProperties.GetIPGlobalProperties()
        Dim tcpConnInfoArray As TcpConnectionInformation() = ipGlobalProperties.GetActiveTcpConnections()

        For Each tcpi As TcpConnectionInformation In tcpConnInfoArray

            ip_str &= tcpi.LocalEndPoint.Port & vbCrLf

            If tcpi.LocalEndPoint.Port = port_number Then
                isAvailable = False
                Exit For
            End If

        Next

        Return isAvailable

        'Dim service As ServiceController = ServiceController.GetServices().FirstOrDefault(Function(s) s.ServiceName = "MySQL")

        'If Not service Is Nothing Then
        '    While True
        '        If service.Status.Equals(ServiceControllerStatus.Stopped) Then
        '            Exit While
        '        ElseIf service.Status.Equals(ServiceControllerStatus.Running) Then
        '            service.Stop()
        '        Else
        '            System.Threading.Thread.Sleep(100)
        '        End If
        '    End While
        'End If

        'Dim isAvailable As Boolean = True

        'Dim ip_str As String = ""

        'Dim ipGlobalProperties As IPGlobalProperties = IPGlobalProperties.GetIPGlobalProperties()
        'Dim tcpConnInfoArray As TcpConnectionInformation() = ipGlobalProperties.GetActiveTcpConnections()

        'For Each tcpi As TcpConnectionInformation In tcpConnInfoArray

        '    ip_str &= tcpi.LocalEndPoint.Port & vbCrLf

        '    If tcpi.LocalEndPoint.Port = port_number Then

        '        isAvailable = False

        '        Exit For

        '    End If

        'Next

        'Return isAvailable

    End Function

    Public Shared Function search_open(rule_name As String, port_number As Integer) As Integer

        Dim temporary_port_number As Integer = port_number

        For i = 0 To 99

            If check_port_available(temporary_port_number) Then

                Call GRT.RunShellCommand.exe("netsh.exe", "advfirewall firewall add rule " & "name = " & rule_name & " dir = in action = allow protocol = TCP localport = " & temporary_port_number, "", "")
                Call GRT.RunShellCommand.exe("netsh.exe", "advfirewall firewall add rule " & "name = " & rule_name & " dir = out action = allow protocol = TCP localport = " & temporary_port_number, "", "")

                Return port_number

            Else

                temporary_port_number += 1

            End If

        Next

        Return -1

    End Function

End Class
