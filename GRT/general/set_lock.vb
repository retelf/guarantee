Imports System.Numerics

Public Class set_lock

    Public Shared Function exe(block_number As Long, database As String, table As String) As String

        Dim Connection As MySqlConnection

        Dim Selectcmd As MySqlCommand
        Dim Adapter As MySqlDataAdapter
        Dim Dataset As DataSet
        Dim procedure_name As String = "up_set_lock" & "_" & table

        If database = "bc" And table = "exchange" Then

            Connection = New MySqlConnection(GRT.GR.cString_mariadb_local_bc)

        ElseIf database = "bc_multilevel" Then

            Connection = New MySqlConnection(GRT.GR.cString_mariadb_local_bc_multilevel)

        ElseIf database = "bc_nft" Then

            Connection = New MySqlConnection(GRT.GR.cString_mariadb_local_bc_nft)

        End If

        Connection.Open()

        ' exchange =======================================

        'CREATE DEFINER =`root`@`localhost` PROCEDURE `up_set_lock_exchange`(IN p_block_number bigint(20))
        'BEGIN
        '    Declare v_state varchar(50);
        '    Declare v_count int;()
        '    Set v_count = (Select count(*) FROM exchange WHERE block_number = p_block_number);
        '    If v_count > 0 Then
        '        BEGIN
        '        Select Case state
        '        INTO v_state
        '        FROM exchange
        '        WHERE block_number = p_block_number;
        '        If (v_state = 'alive') THEN
        '         UPDATE exchange SET state = 'locked' WHERE block_number = p_block_number;
        '        Else
        '    Set v_state = 'locked';
        '        End If;
        '        End;
        '    Else
        '    Set v_state = 'no_rows';
        '    End If;

        'Select Case v_state;

        'End

        ' multilevel =======================================

        'CREATE PROCEDURE `up_set_lock_sell_order`
        '(IN p_block_number bigint(20))
        'BEGIN
        '    DECLARE v_state varchar(50);
        '    DECLARE v_count int;
        '    SET v_count = (SELECT count(*) FROM sell_order WHERE block_number = p_block_number);
        '    IF v_count > 0 THEN
        '        BEGIN
        '        SELECT state
        '        INTO v_state
        '        FROM sell_order
        '        WHERE block_number = p_block_number;
        '        IF (v_state = 'alive' OR v_state = 'seized') THEN
        '         UPDATE sell_order SET state = 'locked' WHERE block_number = p_block_number;
        '        ELSE 
        '         SET v_state = 'locked';
        '        END IF;
        '        END;
        '    ELSE
        '        SET v_state = 'no_rows';
        '    END IF;

        'SELECT v_state;

        'END

        ' NFT =======================================

        'CREATE PROCEDURE `up_set_lock_sell_order`
        '(IN p_block_number bigint(20))
        'BEGIN
        '    DECLARE v_state_open bit(1);
        '    DECLARE v_state_open_str vachar(50);
        '    DECLARE v_count int;
        '    SET v_count = (SELECT count(*) FROM sell_order WHERE block_number = p_block_number);
        '    IF v_count > 0 THEN
        '        BEGIN
        '        SELECT state_open
        '        INTO v_state_open
        '        FROM sell_order
        '        WHERE block_number = p_block_number;
        '        IF v_state_open = 1 THEN
        '         UPDATE sell_order SET state_open = 0 WHERE block_number = p_block_number;
        '         SET v_state_open_str = 'open';
        '        ELSE 
        '         SET v_state_open_str = 'locked';
        '        END IF;
        '        END;
        '    ELSE
        '        SET v_state_open_str = 'no_rows';
        '    END IF;

        'SELECT v_state_open_str;

        'END

        Selectcmd = New MySqlCommand(procedure_name, Connection)
        Selectcmd.CommandType = CommandType.StoredProcedure
        Selectcmd.Parameters.Add(New MySqlParameter("p_block_number", block_number))

        Adapter = New MySqlDataAdapter
        Selectcmd.CommandType = CommandType.StoredProcedure
        Adapter.SelectCommand = Selectcmd
        Dataset = New DataSet
        Adapter.Fill(Dataset)

        Connection.Close()

        Return CStr(Dataset.Tables(0).Rows(0)(0))

    End Function

End Class
