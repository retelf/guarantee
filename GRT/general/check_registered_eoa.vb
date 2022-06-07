Public Class check_registered_eoa

    Public Shared Function exe(eoa As String, coin_name As String) As Boolean

        Dim Selectcmd As MySqlCommand
        Dim Adapter As MySqlDataAdapter
        Dim Dataset As DataSet

        Dim Connection_mariadb_local_bc As New MySqlConnection(GRT.GR.cString_mariadb_local_bc)

        Connection_mariadb_local_bc.Open()

        'CREATE PROCEDURE `up_select_balance`
        '(IN p_eoa  varchar(40),
        'IN p_coin_name  varchar(20))
        'BEGIN
        'SELECT balance
        'FROM account
        'WHERE
        '`eoa` = p_eoa AND
        '`coin_name` = p_coin_name;
        'END

        Selectcmd = New MySqlCommand("up_select_balance", Connection_mariadb_local_bc)
        Selectcmd.CommandType = CommandType.StoredProcedure
        Selectcmd.Parameters.Add(New MySqlParameter("p_eoa", Regex.Replace(eoa, "^0x", "")))
        Selectcmd.Parameters.Add(New MySqlParameter("p_coin_name", coin_name))

        Adapter = New MySqlDataAdapter
        Selectcmd.CommandType = CommandType.StoredProcedure
        Adapter.SelectCommand = Selectcmd
        Dataset = New DataSet
        Adapter.Fill(Dataset)

        If Dataset.Tables(0).Rows.Count = 1 Then
            Return True
        Else
            Return False
        End If

    End Function

    Public Shared Function exe_multi(eoa As String, coin_name() As String) As Boolean

        Dim registered As Boolean = True

        For i = 0 To coin_name.Length - 1

            If Not exe(eoa, coin_name(i)) Then

                registered = False

                Exit For

            End If

        Next

        Return registered

    End Function

End Class
