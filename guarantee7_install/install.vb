Imports System.ServiceProcess
Imports System.Runtime.InteropServices
Imports System.Net
Imports System.IO
Imports Microsoft.Web.WebView2.Core
Imports Newtonsoft.Json

Public Class install
    Private Sub Form_Resize(ByVal sender As Object, ByVal e As EventArgs)
        wv.Size = Me.ClientSize - New System.Drawing.Size(wv.Location) - New Size(New Point(20, 20))
    End Sub

    Public Sub New()

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.

        GR.txt_server_port_number = Me.txt_server_port_number
        GR.txt_database_port_number = Me.txt_database_port_number

        GR.control.wv = Me.wv
        GR.control.lbl_login = Me.lbl_login
        GR.control.lbl_login_state = Me.lbl_login_state
        GR.control.lbl_membership = Me.lbl_membership

        AddHandler Me.Resize, New System.EventHandler(AddressOf Form_Resize)

        Call Initialize_webview() ' 반드시 필요하다.

        Call GRT.set_initial_variables.exe(Me.[GetType]().Name)

    End Sub

    Private Async Sub Initialize_webview()

        Await wv.EnsureCoreWebView2Async(Nothing)

    End Sub

    Private Sub btn_mariadb_download_Click(sender As Object, e As EventArgs) Handles btn_mariadb_download.Click

        wv.Source = New System.Uri("https://downloads.mariadb.org/interstitial/mariadb-10.2.40/winx64-packages/mariadb-10.2.40-winx64.msi")

    End Sub

    Private Shared FolderDownloads As Guid = New Guid("374DE290-123F-4565-9164-39C4925E467B")

    <DllImport("shell32.dll", CharSet:=CharSet.Auto)>
    Private Shared Function SHGetKnownFolderPath(ByRef id As Guid, ByVal flags As Integer, ByVal token As IntPtr, <Out> ByRef path As IntPtr) As Integer
    End Function
    Public Shared Function GetDownloadsPath() As String
        If Environment.OSVersion.Version.Major < 6 Then Throw New NotSupportedException()
        Dim pathPtr As IntPtr = IntPtr.Zero

        Try
            SHGetKnownFolderPath(FolderDownloads, 0, IntPtr.Zero, pathPtr)
            Return Marshal.PtrToStringUni(pathPtr)
        Finally
            Marshal.FreeCoTaskMem(pathPtr)
        End Try
    End Function

    Private Sub btn_install_Click(sender As Object, e As EventArgs) Handles btn_mariadb_install.Click

        ' 먼저 포트 검사

        Dim port_ok As Boolean

        Dim database_port_number As Integer = CInt(GR.txt_database_port_number.Text.Trim)
        Dim server_port_number As Integer = CInt(GR.txt_server_port_number.Text.Trim)

        port_ok = GRT.port.check_port_available(database_port_number)

        If Not port_ok Then
            MessageBox.Show("데이터베이스용 포트넘버 " & database_port_number & " 는 사용할 수 없는 포트넘버입니다. 다른 번호를 사용하시기 바랍니다.")
            Return
        End If

        port_ok = GRT.port.check_port_available(server_port_number)

        If Not port_ok Then
            MessageBox.Show("개런티서버용 포트넘버 " & server_port_number & " 는 사용할 수 없는 포트넘버입니다. 다른 번호를 사용하시기 바랍니다.")
            Return
        End If

        ' 설치시작

        Dim mariadb_install_state As String

        Dim file_full_name As String = "C:\Program Files\MariaDB 10.2\data\my.ini"
        Dim file_info As New FileInfo(file_full_name)

        If file_info.Exists Then
            mariadb_install_state = "uninstall"
        Else
            mariadb_install_state = "install"
        End If

        Dim process As New Process

        Dim startInfo As New ProcessStartInfo

        startInfo.FileName = GetDownloadsPath() & "\mariadb-10.2.40-winx64.msi"

        startInfo.UseShellExecute = True

        process.StartInfo = startInfo

        process.Start()

        process.WaitForExit()

        Call reset_my_ini_file.exe()

        If mariadb_install_state = "install" Then ' 설치 직후다.

            ' 먼저 mariadb 접속해 본다.

            GRT.GR.cString_mariadb_local = "server=localhost;port=3306;database=mysql;uid=root;pwd=;CharSet=utf8;"
            GRT.GR.Connection_mariadb_local = New MySqlConnection(GRT.GR.cString_mariadb_local)

            GRT.GR.Connection_mariadb_local.Open()

            ' 포트넘버 세팅

            Call set_port_number_install.exe() ' my.ini server.ini 파일 변경

            ' 권한설정 세팅

            Call set_server_id_and_db_authority.exe(txt_server_password.Text.Trim) ' root 사용금지, guarantee7 계정설정 및 비밀번호 server.ini 에 저장

            MessageBox.Show("ok")

            GRT.GR.Connection_mariadb_local.Close()

            'Try

            '    ' 먼저 mariadb 접속해 본다.

            '    GRT.GR.cString_mariadb_local = "server=localhost;port=3306;database=mysql;uid=root;pwd=;CharSet=utf8;"
            '    GRT.GR.Connection_mariadb_local = New MySqlConnection(GRT.GR.cString_mariadb_local)

            '    GRT.GR.Connection_mariadb_local.Open()

            '    ' 포트넘버 세팅

            '    Call set_port_number_install.exe() ' my.ini server.ini 파일 변경

            '    ' 권한설정 세팅

            '    Call set_server_id_and_db_authority.exe(txt_server_password.Text.Trim) ' root 사용금지, guarantee7 계정설정 및 비밀번호 server.ini 에 저장

            '    MessageBox.Show("ok")

            '    GRT.GR.Connection_mariadb_local.Close()

            'Catch ex As Exception

            '    MessageBox.Show(ex.Message & vbCrLf & vbCrLf & "데이터베이스 설치가 잘못되었습니다. 데이터베이스를 언인스톨하고 다시 설치하시기 바랍니다. 에러메세지 : " & ex.Message)

            '    Return

            'End Try

        Else ' DB 제거 직후다. 일단 수작업으로 설치폴더를 제거하도록 시킨다.

            MessageBox.Show("제거가 완료되었습니다. 다시 데이터베이스를 설치하시기 바랍니다.")

        End If

    End Sub

    Private Sub btn_register_server_Click(sender As Object, e As EventArgs) Handles btn_register_server.Click

        Call generate_register_node_form.exe()

    End Sub

    Private Async Sub btn_get_schma_Click(sender As Object, e As EventArgs) Handles btn_get_schma.Click

        Dim signiture, parent_address_agency, parent_domain_agency, parent_ip_agency As String
        Dim JSS, JRS As String
        Dim parent_port_agency, count As Integer
        Dim replication_start_number, replication_end_number, max_block_number_parent, max_block_number_local As Long
        Dim json_receipt, json_parent_info_address, json_parent_info_max_block_number As Newtonsoft.Json.Linq.JObject
        Dim block_hash, eoa, contract_type, query_type, query_string As String
        Dim block_number As Long

        ' 먼저 부모부터 찾아야 한다.

        json_parent_info_address = Await Task.Run(Function() GRT.get_parent_server_info.exe())

        parent_domain_agency = json_parent_info_address("value")("domain_agency").ToString
        parent_ip_agency = json_parent_info_address("value")("ip_agency").ToString
        parent_port_agency = CInt(json_parent_info_address("value")("port_agency"))

        If Not parent_domain_agency = "" Then
            parent_address_agency = parent_domain_agency
        Else
            parent_address_agency = parent_ip_agency
        End If

        Await Task.Run(Sub() GRT.replicate_schema_from_p2p.exe(parent_address_agency, parent_port_agency))

        '' mariadb 시작시키고

        'Dim service As ServiceController = New ServiceController("MySQL")

        'If (service.Status.Equals(ServiceControllerStatus.Stopped)) OrElse (service.Status.Equals(ServiceControllerStatus.StopPending)) Then
        '    service.Start()
        'End If

        'Call GRT.set_initial_variables.exe(Me.[GetType]().Name)

        'Call GRT.set_database_connection.open_install()

        '' 데이터베이스 복제한다. 오로지 p2p로만 복제한다.

        ''Call replicate_schema.exe()
        'Call replicate_schema_from_p2p.exe()

        'Call GRT.set_database_connection.close_install()

        'MessageBox.Show("success")

    End Sub

    'Private Sub btn_webview2_download_Click(sender As Object, e As EventArgs) Handles btn_webview2_download.Click

    '    Using client = New WebClient()
    '        client.DownloadFile("http://example.com/file/song/a.mpeg", "a.mpeg")
    '    End Using

    'End Sub
    Private Async Sub btn_syncronize_Click(sender As Object, e As EventArgs) Handles btn_syncronize.Click

        If GRT.GR.account.login_state = "guarantee" Then

            Dim password As String = GRT.get_local_database_password.exe

            ' mariadb 시작시키고

            Dim service As ServiceController = New ServiceController("MySQL")

            While True
                service = New ServiceController("MySQL")
                If service.Status.Equals(ServiceControllerStatus.Stopped) Then
                    service.Start()
                    Exit While
                ElseIf service.Status.Equals(ServiceControllerStatus.Running) Then
                    Exit While
                Else
                    System.Threading.Thread.Sleep(100)
                End If
            End While

            Call GRT.set_initial_variables.exe(Me.[GetType]().Name) : Call GRT.set_database_connection.open_general()

            ' 데이터베이스 복제한다. 오로지 부모로부터만 복제한다.

            Await Task.Run(Sub() GRT.syncronize_main_p2p.exe("install"))

            Call GRT.set_database_connection.close_general()

            MessageBox.Show("success")

        Else

            MessageBox.Show("먼저 로그인을 하시기 바랍니다.")

            Return

        End If

    End Sub

    Private Sub btn_get_key_Click(sender As Object, e As EventArgs) Handles btn_get_key.Click
        txt_key.Text = GRT.get_local_database_password.exe
    End Sub

    Private Sub lbl_login_Click(sender As Object, e As EventArgs) Handles lbl_login.Click

        Call GRT.generate_login_form.exe("install", wv, lbl_login, lbl_login_state, lbl_membership)

    End Sub

    Private Sub lbl_add_account_Click(sender As Object, e As EventArgs) Handles lbl_add_account.Click

        Call GRT.generate_add_account_form.exe(wv, lbl_add_account)

    End Sub

    Private Sub lbl_membership_Click(sender As Object, e As EventArgs) Handles lbl_membership.Click

        Call GRT.generate_super_account_membership_form.exe(wv, lbl_login, lbl_membership)

    End Sub
End Class