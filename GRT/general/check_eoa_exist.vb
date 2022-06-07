Public Class check_eoa_exist

    Public Shared Function exe(eoa As String) As Boolean

        Dim Connection_mariadb_local_bc_manager As New MySqlConnection(GRT.GR.cString_mariadb_local_bc_manager)

        Connection_mariadb_local_bc_manager.Open()

        Dim Selectcmd As MySqlCommand
        Dim Adapter As MySqlDataAdapter
        Dim Dataset As DataSet

        'CREATE PROCEDURE `up_check_eoa_exist`
        '(IN p_eoa varchar(40))
        'BEGIN
        'SELECT COUNT(*)
        'FROM account
        'WHERE
        '`eoa` = p_eoa;
        'END

        Selectcmd = New MySqlCommand("up_check_eoa_exist", Connection_mariadb_local_bc_manager)
        Selectcmd.CommandType = CommandType.StoredProcedure
        Selectcmd.Parameters.Add(New MySqlParameter("p_eoa", Regex.Replace(eoa, "^0x", "")))

        Adapter = New MySqlDataAdapter
        Adapter.SelectCommand = Selectcmd
        Dataset = New DataSet
        Adapter.Fill(Dataset)

        Connection_mariadb_local_bc_manager.Close()

        If CInt(Dataset.Tables(0).Rows(0)(0)) = 1 Then
            Return True
        Else
            Return False
        End If

    End Function

End Class
