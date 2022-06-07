Public Class register_ethereum_client_accounts

    Public Shared Sub exe(keycoin_address As String, amount As Decimal)

        Dim Connection_mariadb As New MySqlConnection(GR.cString_mariadb_local)

        Connection_mariadb.Open()

        Dim Insertcmd As MySqlCommand

        'CREATE PROCEDURE up_insert_temporary_accounts

        '(IN keycoin_address varchar(40),    
        'IN keycoin_password varchar(50),   
        'IN other_address varchar(40),     
        'IN coin_name varchar(10), 
        'IN amount decimal(60,30),
        'IN idate datetime)

        'BEGIN
        'INSERT INTO temporary_accounts
        '(
        '`keycoin_address`,
        '`keycoin_password`,
        '`other_address`,
        '`coin_name`,    
        '`amount`, 
        '`idate`
        ')
        'VALUES
        '(
        'keycoin_address,
        'keycoin_password,
        'other_address,
        'coin_name,
        'amount,
        'idate
        ');
        'End

        Insertcmd = New MySqlCommand("up_insert_temporary_accounts", Connection_mariadb)
        Insertcmd.CommandType = CommandType.StoredProcedure

        Insertcmd.Parameters.Add(New MySqlParameter("keycoin_address", Regex.Replace(keycoin_address, "^0x", "")))
        Insertcmd.Parameters.Add(New MySqlParameter("keycoin_password", GR.txt_password.Text.Trim))
        Insertcmd.Parameters.Add(New MySqlParameter("other_address", Regex.Replace(GR.txt_ethereum_address.Text.Trim, "^0x", "")))
        Insertcmd.Parameters.Add(New MySqlParameter("coin_name", GR.cb_coin_types.SelectedItem.ToString))
        Insertcmd.Parameters.Add(New MySqlParameter("amount", amount))
        Insertcmd.Parameters.Add(New MySqlParameter("idate", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")))

        Insertcmd.CommandTimeout = 600

        Insertcmd.ExecuteNonQuery()

        Connection_mariadb.Close()

    End Sub

End Class
