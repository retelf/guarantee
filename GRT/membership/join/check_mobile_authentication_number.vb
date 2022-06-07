Public Class check_mobile_authentication_number

    Public Shared Function exe(json As Newtonsoft.Json.Linq.JObject) As DataSet

        Dim hash As String = json("value")("hash").ToString

        Dim Connection_mariadb_bc_nts As New MySqlConnection(GR.cString_mariadb_bc_nts)

        Connection_mariadb_bc_nts.Open()

        Dim Selectcmd As MySqlCommand
        Dim Adapter As MySqlDataAdapter
        Dim Dataset As DataSet

        'CREATE PROCEDURE `up_select_temporary_super_account`
        '(IN p_hash  char(40))
        'BEGIN
        'SELECT mobile_authentication_number, pure_query, signiture
        'FROM temporary_super_account
        'WHERE
        '`hash` = p_hash;
        'END

        Selectcmd = New MySqlCommand("up_select_temporary_super_account", Connection_mariadb_bc_nts)
        Selectcmd.CommandType = CommandType.StoredProcedure
        Selectcmd.Parameters.Add(New MySqlParameter("p_hash", Regex.Replace(hash, "^0x", "")))

        Adapter = New MySqlDataAdapter
        Selectcmd.CommandType = CommandType.StoredProcedure
        Adapter.SelectCommand = Selectcmd
        Dataset = New DataSet
        Adapter.Fill(Dataset)

        Connection_mariadb_bc_nts.Close()

        Return dataset

    End Function

End Class
