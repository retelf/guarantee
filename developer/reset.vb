Imports MySqlConnector

Public Class reset

    Shared cString() As String = {
        "server=192.168.0.251;port=3306;database=mysql;uid=guarantee7;pwd=#cyndi$36%;CharSet=utf8;", ' 251 - nts
        "server=filmasfilm.com;port=30601;database=mysql;uid=guarantee7;pwd=0x7aF87Ad373C2e7b9875CaADF44c0Cef78D069105;CharSet=utf8;", ' 233
        "server=192.168.0.253;port=30601;database=mysql;uid=guarantee7;pwd=0x24bCaCBC32338a253fA62BE4D062FcC7A1660A87;CharSet=utf8;", ' 253
        "server=filmasfilm.com;port=30602;database=mysql;uid=guarantee7;pwd=0xafD2D6DdD5Bb60f121f20Bf598B777578a486066;CharSet=utf8;", ' 246
        "server=192.168.0.248;port=30601;database=mysql;uid=guarantee7;pwd=0x84A9f63A3dc2eBADC76dE566b5F58b021C072631;CharSet=utf8;"} ' 248

    Shared private_key() As String = {
        "0xba3b353e9de09e64fd5bb8d4d373dea9aa76593a6fa63b6071a759b044a19511", ' 233
        "0x2af5a3ed36bea9a808a300db453a91f13329998ebe3c964e4500909118e77278", ' 253
        "0xfa30e2b5e7f79d912522e6b4758320339ee209577b81fcfc0786ecffbdb36f32", ' 246
        "0xf5b2d6f7db979ee4b2f1c9109c876dc1a0dafa7c44f3800c13a60044702b3a0a"} ' 248

    Shared node() As String = {
        "0xd233120B00c7D2AcDb17A03aF08D70D1Fe31e6cb", ' 233
        "0x962921D70e02e6c5970112579f6e6F2e9d0b1b94", ' 253
        "0xceBd5A5b38477f3BF84CE636a40bdfc2163c6201", ' 246
        "0xfB489D2410068608A46a2cDda8950817dF55D16b"} ' 248

    'Shared cString() As String = {
    '    "server=filmasfilm.com;port=30601;database=mysql;uid=guarantee7;pwd=0xd7162956E3d3B2F3DC0d090c153E8F42387A5830;"} ' 233

    Public Shared Sub exe(replicate_type As String)

        ' 일단 sample_database 부터 만든다. 이것은 이 작업 후 바로 주석처리한다.

        Dim Connection_sample_database = New MySqlConnection("server=filmasfilm.com;port=3306;database=mysql;uid=retelf;pwd=#cyndi$36%;CharSet=utf8;")

        Connection_sample_database.Open()

        ''''''' Call make_sample_database.exe(Connection_sample_database, cString(1), cString(0)) ' 이것은 앞으로 더 이상 필요없을 것이다.

        Dim Connection_local_for_save As MySqlConnection

        For i = 0 To cString.Length - 1

            Connection_local_for_save = New MySqlConnection(cString(i))

            Connection_local_for_save.Open()

            ' 모든 스키마를 제거 및 생성하고 

            Call replicate_schema.exe(replicate_type, Connection_sample_database, Connection_local_for_save)

            If replicate_type = "all" Then

                ' 모든 샘플 데이터를 입력한다(일단은 USE bc_manager; SELECT * FROM node; 만 한다. 추후 증가될 수 있다.)

                Call insert_sample_data.node(Connection_local_for_save, Connection_sample_database)

                If i > 0 Then

                    Call insert_sample_data.private_key(private_key(i - 1), Connection_local_for_save, Connection_sample_database)

                End If

            End If

            Connection_local_for_save.Close()

        Next

        Connection_sample_database.Close()

        MessageBox.Show("ok")

    End Sub

    Public Shared Sub exe_smart_contract(block_number As Long)

        Dim Connection As MySqlConnection
        Dim Command As MySqlCommand

        Dim reset_query As String

        For i = 1 To cString.Length - 1

            Connection = New MySqlConnection(cString(i))

            Connection.Open()

            'reset_query = "USE bc; DELETE FROM main WHERE `block_number` = 6;USE bc_smart_contract; DELETE FROM contract WHERE `block_number` = 6;DROP DATABASE IF EXISTS bc_sc_multilevel_pension_contract;"
            'reset_query = "USE bc; DELETE FROM main WHERE `block_number` > 12;DELETE FROM orders WHERE `block_number` > 12;USE bc_smart_contract; DELETE FROM contract WHERE `block_number` > 12;DROP DATABASE IF EXISTS bc_sc_market;"
            reset_query =
                "USE bc; DELETE FROM main WHERE `block_number` >= " & block_number & ";" &
                "USE bc; DELETE FROM account WHERE `block_number` >= " & block_number & ";" &
                "USE bc; DELETE FROM exchange WHERE `block_number` >= " & block_number & ";" &
                "USE bc_agent; DELETE FROM agent_record WHERE `block_number` >= " & block_number & " OR `block_number` = 0; " &
                "USE bc_manager; DELETE FROM account WHERE `block_number` >= " & block_number & "; " &
                "USE bc_manager; DELETE FROM node WHERE `block_number` >= " & block_number & ";" &
                "USE bc_multilevel; DELETE FROM sell_order WHERE `block_number` >= " & block_number & " OR `block_number` = 0; " &
                "USE bc_multilevel; DELETE FROM account WHERE `block_number` >= " & block_number & " OR `block_number` = 0; "

            Command = New MySqlCommand(reset_query, Connection)
            Command.CommandType = CommandType.Text

            Command.ExecuteNonQuery()

            Connection.Close()

        Next

        MessageBox.Show("ok")

    End Sub

    Public Shared Sub exe_balance()

        Dim Connection As MySqlConnection
        Dim Command As MySqlCommand

        Dim reset_query As String

        For i = 1 To cString.Length - 1

            Connection = New MySqlConnection(cString(i))

            Connection.Open()

            reset_query = "USE bc; UPDATE account SET balance = 0;"

            reset_query &= "USE bc; UPDATE account SET balance = 100 WHERE coin_name = 'guarantee';"

            reset_query &= "USE bc; UPDATE account SET balance = 0 WHERE eoa_type = 'ma';"

            reset_query &= "USE bc; UPDATE account SET balance = 1000 WHERE (coin_name = 'guarantee' OR coin_name = 'ethereum') AND (eoa = '962921D70e02e6c5970112579f6e6F2e9d0b1b94' OR eoa = '12c98cbe86B5e541391e58674398bEb529C3A1Bf' OR eoa = 'e58a43B5b46b91184467FD2e5b594B4441682126');"

            reset_query &= "USE bc; UPDATE account SET balance = 7777777 WHERE eoa = '12c98cbe86B5e541391e58674398bEb529C3A1Bf' AND coin_name = 'guarantee';"

            Command = New MySqlCommand(reset_query, Connection)

            Command.CommandType = CommandType.Text

            Command.ExecuteNonQuery()

            Connection.Close()

        Next

        MessageBox.Show("ok")

    End Sub

End Class
