Public Class generate_eoa_account

    Public Shared Sub exe(id As String, coin_name As String, public_key As String, open As Boolean)

        Dim Connection_mariadb_local_bc_manager As New MySqlConnection(GR.cString_mariadb_local_bc_manager)

        Connection_mariadb_local_bc_manager.Open()

        Dim Insertcmd As MySqlCommand

        ' account

        'CREATE PROCEDURE up_insert_account

        '(IN id varchar(40),
        'IN eoa varchar(40),
        'IN coin_name varchar(10), 
        'IN balance decimal(60,30),
        'IN idate datetime)

        'BEGIN
        'INSERT INTO account
        '(
        '`id`,
        '`eoa`,
        '`coin_name`,    
        '`balance`, 
        '`idate`
        ')
        'VALUES
        '(
        'id,
        'eoa,
        'coin_name,
        'balance,
        'idate
        ');
        'End

        Insertcmd = New MySqlCommand("up_insert_account", Connection_mariadb_local_bc_manager)
        Insertcmd.CommandType = CommandType.StoredProcedure

        Insertcmd.Parameters.Add(New MySqlParameter("id", id))
        Insertcmd.Parameters.Add(New MySqlParameter("eoa", Regex.Replace(public_key, "^0x", "")))
        Insertcmd.Parameters.Add(New MySqlParameter("coin_name", coin_name))
        Insertcmd.Parameters.Add(New MySqlParameter("balance", 0))
        Insertcmd.Parameters.Add(New MySqlParameter("idate", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")))

        Insertcmd.ExecuteNonQuery()

        ' security 

        'CREATE PROCEDURE up_insert_security_smart_contracts

        '(IN eoa varchar(40),
        'IN o_balance bit(1), 
        'IN o_idate bit(1), 
        'IN idate datetime)

        'BEGIN
        'INSERT INTO security_smart_contracts
        '(
        '`eoa`,
        '`o_balance`,    
        '`o_idate`, 
        '`idate`
        ')
        'VALUES
        '(
        'eoa,
        'o_balance,
        'o_idate,
        'idate
        ');
        'End

        Insertcmd = New MySqlCommand("up_insert_security_smart_contracts", Connection_mariadb_local_bc_manager)
        Insertcmd.CommandType = CommandType.StoredProcedure

        Insertcmd.Parameters.Add(New MySqlParameter("eoa", Regex.Replace(public_key, "^0x", "")))
        Insertcmd.Parameters.Add(New MySqlParameter("o_balance", open))
        Insertcmd.Parameters.Add(New MySqlParameter("o_idate", open))
        Insertcmd.Parameters.Add(New MySqlParameter("idate", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")))

        Insertcmd.ExecuteNonQuery()

        Connection_mariadb_local_bc_manager.Close()

    End Sub

End Class
