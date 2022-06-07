Imports System.IO

Public Class set_server_id_and_db_authority

    Public Shared Sub exe(password As String)

        ' 먼저 nerthereum 으로 root 의 계정과 비밀번호를 생성하고

        Dim ecKey = Nethereum.Signer.EthECKey.GenerateKey()

        Dim public_key = ecKey.GetPublicAddress()
        Dim private_key = ecKey.GetPrivateKey

        ' 데이터베이스에 계정(root)과 암호(public_key)를 설정한다. 먼저 데이터베이스 접속

        GRT.GR.cString_mariadb_local = "server=localhost;port=" & GRT.GR.port_number_database_local & ";database=mysql;uid=root;pwd=;CharSet=utf8;"
        GRT.GR.Connection_mariadb_local = New MySqlConnection(GRT.GR.cString_mariadb_local)

        GRT.GR.Connection_mariadb_local.Open()

        Dim sqlCommand As MySqlCommand

        sqlCommand = New MySqlCommand("GRANT ALL ON *.* TO 'guarantee7'@'%' IDENTIFIED by '" & public_key & "' WITH GRANT OPTION; FLUSH PRIVILEGES;", GRT.GR.Connection_mariadb_local)
        sqlCommand.CommandType = CommandType.Text

        sqlCommand.ExecuteNonQuery()

        ' root의 비밀번호도 마찬가지로 바꾼다.

        sqlCommand = New MySqlCommand("ALTER USER 'root'@'localhost' IDENTIFIED BY '" & public_key & "';", GRT.GR.Connection_mariadb_local)
        sqlCommand.CommandType = CommandType.Text

        sqlCommand.ExecuteNonQuery()

        GRT.GR.Connection_mariadb_local.Close()

        ' 암호를 다시 암호화한다.

        Dim hashed_public_key As String = GRT.Security.Gsign.sign(password, private_key)

        ' 암호화되지 않은 계정(root)과 암호화된 암호(hashed_public_key)를 server.ini 에 저장한다. 만약 기존 설정이 있다면 이는 하지 못한다.

        Dim current_directory As String = Regex.Replace(Directory.GetCurrentDirectory, "(guarantee7|test_guarantee)[^\\]*\\bin\\Debug\\net5.0-windows", "guarantee7_server\bin\Debug\net5.0-windows")

        Dim file_info = New FileInfo(current_directory & "\resource\server.ini")

        ecKey = Nethereum.Signer.EthECKey.GenerateKey()

        ' 서버아이디 생성

        Call GRT.modify_file_content.exe(file_info, {"server_id=[^\n]*", "password_hash=[^\n]*", "private_key=[^\n]*"}, {"server_id=" & ecKey.GetPublicAddress() & vbCrLf, "password_hash=" & hashed_public_key & vbCrLf, "private_key=" & ecKey.GetPrivateKey() & vbCrLf})

    End Sub

End Class
