Imports System.ServiceProcess
Imports System.IO

Public Class set_port_number_install

    Public Shared Sub exe()

        Dim port_ok As Boolean

        Dim database_port_number As Integer = CInt(GR.txt_database_port_number.Text.Trim)
        Dim server_port_number As Integer = CInt(GR.txt_server_port_number.Text.Trim)

        GRT.GR.port_number_database_local = database_port_number
        GRT.GR.port_number_server_local = server_port_number

        Dim current_directory As String = Regex.Replace(Directory.GetCurrentDirectory, "(guarantee7|test_guarantee)[^\\]*\\bin\\Debug\\net5.0-windows", "guarantee7_server\bin\Debug\net5.0-windows")

        ' mariadb 의 포트넘버를 변경시킨다. my.cnf(my.ini) 의 port = database_port_number 로 하면 그만이다.

        port_ok = GRT.port.open("mariadb_server", database_port_number)

        If Not port_ok Then
            MessageBox.Show("데이터베이스용 포트넘버 " & database_port_number & " 는 사용할 수 없는 포트넘버입니다. 다른 번호를 사용하시기 바랍니다.")
            Return
        End If

        ' 먼저 mariadb 를 stop 시킨다.

        Dim service As ServiceController

        While True
            service = New ServiceController("MySQL")
            If service.Status.Equals(ServiceControllerStatus.Stopped) Then
                Exit While
            ElseIf service.Status.Equals(ServiceControllerStatus.Running) Then
                service.Stop()
            Else
                System.Threading.Thread.Sleep(100)
            End If
        End While

        'Dim default_file_source As String = find_file.exe("my.ini")

        Dim file_full_name As String = "C:\Program Files\MariaDB 10.2\data\my.ini"
        Dim file_info As New FileInfo(file_full_name)

        If Not file_info.Exists Then

            Using OpenFileDialog = New OpenFileDialog()

                OpenFileDialog.InitialDirectory = "c:\"
                OpenFileDialog.Filter = "ini files (*.ini)|*.ini"
                OpenFileDialog.FilterIndex = 2
                OpenFileDialog.RestoreDirectory = True

                If OpenFileDialog.ShowDialog() = DialogResult.OK Then

                    file_full_name = OpenFileDialog.FileName

                    file_info = New FileInfo(file_full_name)

                End If

            End Using

        End If

        Call GRT.modify_file_content.exe(file_info, {"port\s*=\s*\d+", "\[client\]"}, {"port=" & database_port_number, "skip-networking=0" & vbCrLf & "[client]"})

        file_info = New FileInfo(current_directory & "\resource\server.ini")

        Call GRT.modify_file_content.exe(file_info, {"path\.my\.ini=[^\n]*"}, {"path.my.ini=" & file_full_name & vbCrLf})

        While True
            service = New ServiceController("MySQL")
            If Not service.Status.Equals(ServiceControllerStatus.Stopped) Then
                System.Threading.Thread.Sleep(100)
            Else
                service.Start()
                Exit While
            End If
        End While

        ' server 의 포트넘버를 변경시킨다.

        Call GRT.port.open("guarantee_server", server_port_number)

        file_info = New FileInfo(current_directory & "\resource\server.ini")

        Call GRT.modify_file_content.exe(file_info, {"port=[^\n]*"}, {"port=" & server_port_number & vbCrLf})

        ' 공유기 메세지를 띄워준다.

        Callback.SetControl("wv_install", "write", "공유기를 사용하는 경우 공유기에서도 포트포워딩을 통하여 MariaDB의 포트번호(" & database_port_number & ")와 서버의 포트번호(" & server_port_number & ")를 열어 주어야 합니다.", "")

    End Sub

End Class
