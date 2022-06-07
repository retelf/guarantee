Public Class set_unlock

    Public Shared Sub exe(block_number As Long, database As String, table As String)

        Dim Connection As MySqlConnection
        Dim procedure_name As String = "up_set_unlock" & "_" & table

        Dim Updatecmd As MySqlCommand

        If database = "bc" Then

            Connection = New MySqlConnection(GRT.GR.cString_mariadb_local_bc)

            'CREATE PROCEDURE `up_set_unlock_exchange`
            '(IN p_block_number bigint(20))
            'BEGIN
            'UPDATE exchange SET state = 'alive' WHERE block_number = p_block_number;
            'END

        ElseIf database = "bc_multilevel" Then

            Connection = New MySqlConnection(GRT.GR.cString_mariadb_local_bc_multilevel)

            If table = "account" Then

                'CREATE PROCEDURE `up_set_unlock_account`
                '(IN p_block_number bigint(20))
                'BEGIN
                'UPDATE account SET state = 'alive' WHERE block_number = p_block_number;
                'END

            ElseIf table = "sell_order" Then

                'CREATE PROCEDURE `up_set_unlock_sell_order`
                '(IN p_block_number bigint(20))
                'BEGIN
                'UPDATE sell_order SET state = 'alive' WHERE block_number = p_block_number;
                'END

            End If

        ElseIf database = "bc_nft" Then

            Connection = New MySqlConnection(GRT.GR.cString_mariadb_local_bc_nft)

            If table = "sell_order" Then

                'CREATE PROCEDURE `up_set_unlock_sell_order`
                '(IN p_block_number bigint(20))
                'BEGIN
                'UPDATE sell_order SET state_open = 1 WHERE block_number = p_block_number;
                'END

            End If

        End If

        Connection.Open()

        Updatecmd = New MySqlCommand(procedure_name, Connection)
        Updatecmd.CommandType = CommandType.StoredProcedure
        Updatecmd.Parameters.Add(New MySqlParameter("p_block_number", block_number))
        Updatecmd.ExecuteNonQuery()

        Connection.Close()

    End Sub

End Class
