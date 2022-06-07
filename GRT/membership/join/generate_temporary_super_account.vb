Public Class generate_temporary_super_account

    Public Shared Function exe(json As Newtonsoft.Json.Linq.JObject, mobile_authentication_number As Integer, pure_query As String, signiture As String, hash As String) As String

        Dim Connection_mariadb_bc_nts As New MySqlConnection(GR.cString_mariadb_bc_nts)

        Connection_mariadb_bc_nts.Open()

        Dim Insertcmd As MySqlCommand

        'CREATE PROCEDURE up_insert_temporary_super_account

        '(IN email varchar(50), 
        'IN name_english varchar(50), 
        'IN name_home_language varchar(100), 
        'IN country varchar(50), 
        'IN phone_number varchar(50), 
        'IN identity_number varchar(50), 
        'IN public_key char(40), 
        'IN mobile_authentication_number int, 
        'IN pure_query text, 
        'IN signiture text, 
        'IN hash char(40), 
        'IN idate datetime)

        'BEGIN
        'INSERT INTO temporary_super_account
        '(
        '`email`,
        '`name_english`,
        '`name_home_language`,
        '`country`,
        '`phone_number`,
        '`identity_number`,
        '`public_key`,
        '`mobile_authentication_number`,
        '`pure_query`,
        '`signiture`,
        '`hash`,
        '`idate`
        ')
        'VALUES
        '(
        'email,
        'name_english,
        'name_home_language,
        'country,
        'phone_number,
        'identity_number,
        'public_key,
        'mobile_authentication_number,
        'pure_query,
        'signiture,
        'hash,
        'idate
        ');
        'End

        Insertcmd = New MySqlCommand("up_insert_temporary_super_account", Connection_mariadb_bc_nts)
        Insertcmd.CommandType = CommandType.StoredProcedure

        Insertcmd.Parameters.Add(New MySqlParameter("email", json("value")("email").ToString))
        Insertcmd.Parameters.Add(New MySqlParameter("name_english", json("value")("name_english").ToString))
        Insertcmd.Parameters.Add(New MySqlParameter("name_home_language", json("value")("name_home_language").ToString))
        Insertcmd.Parameters.Add(New MySqlParameter("country", json("value")("country").ToString))
        Insertcmd.Parameters.Add(New MySqlParameter("phone_number", json("value")("phone_number").ToString))
        Insertcmd.Parameters.Add(New MySqlParameter("identity_number", json("value")("identity_number").ToString))
        Insertcmd.Parameters.Add(New MySqlParameter("public_key", Regex.Replace(json("value")("public_key").ToString, "^0x", "")))
        Insertcmd.Parameters.Add(New MySqlParameter("mobile_authentication_number", mobile_authentication_number))
        Insertcmd.Parameters.Add(New MySqlParameter("pure_query", pure_query))
        Insertcmd.Parameters.Add(New MySqlParameter("signiture", signiture))
        Insertcmd.Parameters.Add(New MySqlParameter("hash", Regex.Replace(hash, "^0x", "")))
        Insertcmd.Parameters.Add(New MySqlParameter("idate", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")))

        Insertcmd.ExecuteNonQuery()

        Connection_mariadb_bc_nts.Close()

    End Function

End Class
