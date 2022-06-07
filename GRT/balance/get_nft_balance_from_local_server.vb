Public Class get_nft_balance_from_local_server

    Public Shared Function exe(nfa As String, token_id As Integer, confirmed_type As String, data As String, signature As String, public_key As String) As Integer

        Dim verified As Boolean
        Dim balance_nft As Integer

        If public_key = GRT.GR.server_id Then ' 스스로의 서버가 자신의 balance 를 확인하는 경우다.
            verified = True
        Else
            verified = Security.Gverify.verify(data, signature, public_key)
        End If

        If verified Then

            Dim Connection_mariadb_local_bc_nft As New MySqlConnection(GRT.GR.cString_mariadb_local_bc_nft)

            Connection_mariadb_local_bc_nft.Open()

            Dim Selectcmd As MySqlCommand
            Dim Adapter As MySqlDataAdapter
            Dim Dataset As DataSet

            If confirmed_type = "confirmed" Then

                'CREATE PROCEDURE `up_select_balance_nft_confirmed`
                '(IN p_eoa varchar(40), p_nfa varchar(40), p_token_id int(11))
                'BEGIN
                'SELECT pieces_confirmed_ordered FROM owner_portion
                'WHERE
                '`eoa` = p_eoa AND `nfa` = p_nfa AND `token_id` = p_token_id;
                'END

                Selectcmd = New MySqlCommand("up_select_balance_nft_confirmed", Connection_mariadb_local_bc_nft)

            Else

                'CREATE PROCEDURE `up_select_balance_nft_unconfirmed`
                '(IN p_eoa varchar(40), p_nfa varchar(40), p_token_id int(11))
                'BEGIN
                'SELECT pieces_unconfirmed_ordered FROM owner_portion
                'WHERE
                '`eoa` = p_eoa AND `nfa` = p_nfa AND `token_id` = p_token_id;
                'END

                Selectcmd = New MySqlCommand("up_select_balance_nft_unconfirmed", Connection_mariadb_local_bc_nft)

            End If

            Selectcmd.Parameters.Add(New MySqlParameter("p_eoa", Regex.Replace(public_key, "^0x", "")))
            Selectcmd.Parameters.Add(New MySqlParameter("p_nfa", Regex.Replace(nfa, "^0x", "")))
            Selectcmd.Parameters.Add(New MySqlParameter("p_token_id", token_id))

            Adapter = New MySqlDataAdapter
            Selectcmd.CommandType = CommandType.StoredProcedure
            Adapter.SelectCommand = Selectcmd
            Dataset = New DataSet

            Adapter.Fill(Dataset)

            balance_nft = CInt(Dataset.Tables(0).Rows(0)((0)))

            Connection_mariadb_local_bc_nft.Close()

        Else
            balance_nft = -1
        End If

        Return balance_nft

    End Function

End Class
