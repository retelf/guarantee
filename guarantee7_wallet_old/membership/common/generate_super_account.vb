Public Class generate_super_account

    Public Shared Sub exe()

        'Dim Connection_mariadb_main As New MySqlConnection(GRT.GR.cString_mariadb_main)

        'Connection_mariadb_main.Open()

        'Dim Insertcmd As MySqlCommand

        ''CREATE PROCEDURE up_insert_super_account

        ''(IN id varchar(50),    
        ''IN password varchar(50),     
        ''IN name_english varchar(50), 
        ''IN name_home_language varchar(100), 
        ''IN country varchar(50), 
        ''IN phone_number varchar(50), 
        ''IN identity_number varchar(50), 
        ''IN idate datetime)

        ''BEGIN
        ''INSERT INTO bc_manager.super_account
        ''(
        ''`id`,
        ''`password`,
        ''`name_english`,
        ''`name_home_language`,
        ''`country`,
        ''`phone_number`,
        ''`identity_number`,
        ''`idate`
        '')
        ''VALUES
        ''(
        ''id,
        ''password,
        ''name_english,
        ''name_home_language,
        ''country,
        ''phone_number,
        ''identity_number,
        ''idate
        '');
        ''End

        'Insertcmd = New MySqlCommand("up_insert_super_account", Connection_mariadb_main)
        'Insertcmd.CommandType = CommandType.StoredProcedure

        'Insertcmd.Parameters.Add(New MySqlParameter("id", GR.authentication.id))
        'Insertcmd.Parameters.Add(New MySqlParameter("password", GR.authentication.password))
        'Insertcmd.Parameters.Add(New MySqlParameter("name_english", GR.authentication.name_english))
        'Insertcmd.Parameters.Add(New MySqlParameter("name_home_language", GR.authentication.name_home_language))
        'Insertcmd.Parameters.Add(New MySqlParameter("country", GR.authentication.country))
        'Insertcmd.Parameters.Add(New MySqlParameter("phone_number", GR.authentication.phone_number))
        'Insertcmd.Parameters.Add(New MySqlParameter("identity_number", GR.authentication.identity_number))
        'Insertcmd.Parameters.Add(New MySqlParameter("idate", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")))

        'Insertcmd.ExecuteNonQuery()

        'Connection_mariadb_main.Close()

        'MessageBox.Show("회원가입에 성공하였습니다.")

    End Sub

End Class
