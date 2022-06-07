Public Class get_guarantee_balance_from_local_server

    Public Shared Function exe(data As String, signature As String, public_key As String) As Decimal

        Dim verified As Boolean
        Dim balance_guarantee As Decimal

        If public_key = GRT.GR.server_id Then ' 스스로의 서버가 자신의 balance 를 확인하는 경우다.
            verified = True
        Else
            verified = Security.Gverify.verify(data, signature, public_key)
        End If

        If verified Then

            ' 임의의 다른 서버의 밸런스 확인을 추가한다. 만약 일치하지 않으면 마이너스 값을 반환한다.
            ' 또한 순간적으로 여러번 누르는 경우 이를 추가적으로 차감한 값을 리턴한다.
            ' na 의 경우에는 여러 곳에서 동시적으로 환전이 발생하는 경우가 있다.

            Dim Connection_mariadb_local_bc As New MySqlConnection(GRT.GR.cString_mariadb_local_bc)

            Connection_mariadb_local_bc.Open()

            Dim Selectcmd As MySqlCommand
            Dim Adapter As MySqlDataAdapter
            Dim Dataset As DataSet

            'CREATE PROCEDURE `up_select_balance`
            '(IN p_eoa  varchar(40),
            'IN p_coin_name  varchar(20))
            'BEGIN
            'SELECT balance FROM account
            'WHERE
            '`eoa` = p_eoa AND
            '`coin_name` = p_coin_name;
            'END

            Selectcmd = New MySqlCommand("up_select_balance", Connection_mariadb_local_bc)
            Selectcmd.Parameters.Add(New MySqlParameter("p_eoa", Regex.Replace(public_key, "^0x", "")))
            Selectcmd.Parameters.Add(New MySqlParameter("p_coin_name", "guarantee"))

            Adapter = New MySqlDataAdapter
            Selectcmd.CommandType = CommandType.StoredProcedure
            Adapter.SelectCommand = Selectcmd
            Dataset = New DataSet

            Adapter.Fill(Dataset)

            balance_guarantee = CDec(Dataset.Tables(0).Rows(0)((0)))

            Connection_mariadb_local_bc.Close()

        Else
            balance_guarantee = -1
        End If

        Return balance_guarantee

    End Function

End Class
