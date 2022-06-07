Public Class set_node_private_key

    Public Shared Sub exe(password As String, private_key As String)

        Dim Connection_mariadb_local_bc As New MySqlConnection(GR.cString_mariadb_local_bc)

        Connection_mariadb_local_bc.Open()

        Dim Insertcmd As MySqlCommand

        'CREATE PROCEDURE up_set_node_private_key

        '(IN password varchar(50),
        'IN private_key char(64),
        'IN idate datetime)

        'BEGIN
        'INSERT INTO key_store
        '(
        '`password`,
        '`private_key`,
        '`idate`
        ')
        'VALUES
        '(
        'password,
        'private_key,
        'idate
        ');
        'End

        Insertcmd = New MySqlCommand("up_set_node_private_key", Connection_mariadb_local_bc)
        Insertcmd.CommandType = CommandType.StoredProcedure

        Insertcmd.Parameters.Add(New MySqlParameter("password", password))
        Insertcmd.Parameters.Add(New MySqlParameter("private_key", Regex.Replace(private_key, "^0x", "")))
        Insertcmd.Parameters.Add(New MySqlParameter("idate", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")))

        Insertcmd.ExecuteNonQuery()

        Connection_mariadb_local_bc.Close()

    End Sub

End Class
