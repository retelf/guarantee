Public Class agent_record

    Public Shared Sub generate(signiture_key As String, idate_generated As DateTime, json As Newtonsoft.Json.Linq.JObject, JSS As String)

        Dim block_number_this_contract_type As Long
        Dim contract_type = json("key").ToString

        Select Case contract_type
            Case "submit_transfer", "submit_load_order"
                block_number_this_contract_type = CLng(-1)
            Case "submit_exchange"
                block_number_this_contract_type = CLng(json("value")("exchange_block_number"))
            Case "submit_buy", "submit_refund", "submit_recall", "submit_confirm", "submit_nft_buy"
                block_number_this_contract_type = CLng(json("value")("sell_order_block_number"))
            Case "submit_cancel"
                block_number_this_contract_type = CLng(json("value")("cancel_block_number"))
        End Select

        Dim Connection_mariadb_local_bc_agent As New MySqlConnection(GRT.GR.cString_mariadb_local_bc_agent)

        Connection_mariadb_local_bc_agent.Open()

        Dim Insertcmd As MySqlCommand

        'CREATE PROCEDURE up_insert_agent_record
        '(
        'IN p_block_number bigint(20),
        'IN p_block_number_this_contract_type bigint(20),
        'IN p_level int,
        'IN p_signiture_key char(64),
        'IN p_transaction_hash char(64),
        'IN p_transaction_hash_cancel char(64),
        'IN p_contract_type varchar(50),
        'IN p_receipt text,
        'IN p_receipt_cancel text,
        'IN p_JSS text,
        'IN p_state varchar(1000),
        'IN p_idate_generated datetime,
        'IN p_idate_confirmed datetime,
        'IN p_time_taken time,
        'IN p_error_message text
        ')
        'BEGIN
        'INSERT INTO agent_record
        '(
        '`block_number`,
        '`block_number_this_contract_type`,
        '`level`,
        '`signiture_key`,
        '`transaction_hash`,
        '`transaction_hash_cancel`,
        '`contract_type`,
        '`receipt`,
        '`receipt_cancel`,
        '`JSS`,
        '`state`,
        '`idate_generated`,
        '`idate_confirmed`,
        '`time_taken`,
        '`error_message`
        ')
        'VALUES
        '(
        'p_block_number,
        'p_block_number_this_contract_type,
        'p_level,
        'p_signiture_key,
        'p_transaction_hash,
        'p_transaction_hash_cancel,
        'p_contract_type,
        'p_receipt,
        'p_receipt_cancel,
        'p_JSS,
        'p_state,
        'p_idate_generated,
        'p_idate_confirmed,
        'p_time_taken,
        'p_error_message
        ');
        'End

        Insertcmd = New MySqlCommand("up_insert_agent_record", Connection_mariadb_local_bc_agent)
        Insertcmd.CommandType = CommandType.StoredProcedure

        Insertcmd.Parameters.Add(New MySqlParameter("p_block_number", 0))
        Insertcmd.Parameters.Add(New MySqlParameter("p_block_number_this_contract_type", block_number_this_contract_type))
        Insertcmd.Parameters.Add(New MySqlParameter("p_level", GR.node_level))
        Insertcmd.Parameters.Add(New MySqlParameter("p_signiture_key", Regex.Replace(signiture_key, "^0x", "")))
        Insertcmd.Parameters.Add(New MySqlParameter("p_transaction_hash", ""))
        Insertcmd.Parameters.Add(New MySqlParameter("p_transaction_hash_cancel", ""))
        Insertcmd.Parameters.Add(New MySqlParameter("p_contract_type", contract_type))
        Insertcmd.Parameters.Add(New MySqlParameter("p_receipt", ""))
        Insertcmd.Parameters.Add(New MySqlParameter("p_receipt_cancel", ""))
        Insertcmd.Parameters.Add(New MySqlParameter("p_JSS", JSS))
        Insertcmd.Parameters.Add(New MySqlParameter("p_state", "|start|"))
        Insertcmd.Parameters.Add(New MySqlParameter("p_idate_generated", idate_generated.ToString("yyyy/MM/dd HH:mm:ss")))
        Insertcmd.Parameters.Add(New MySqlParameter("p_idate_confirmed", "0000-00-00 00:00:00"))
        Insertcmd.Parameters.Add(New MySqlParameter("p_time_taken", "00:00:00"))
        Insertcmd.Parameters.Add(New MySqlParameter("p_error_message", "|"))

        Insertcmd.ExecuteNonQuery()

        Connection_mariadb_local_bc_agent.Close()

    End Sub

    Public Shared Sub state_update(state As String, error_message As String, signiture_key As String)

        Dim Updatecmd As MySqlCommand

        Dim Connection_mariadb_local_bc_agent As New MySqlConnection(GRT.GR.cString_mariadb_local_bc_agent)

        Connection_mariadb_local_bc_agent.Open()

        'CREATE PROCEDURE `up_agent_record_state_update`
        '(IN p_signiture_key char(64),
        'IN p_state varchar(50),
        'IN p_error_message text)
        'BEGIN
        'UPDATE agent_record
        'SET state = CONCAT(state, p_state , '|' ), error_message = CONCAT(error_message , p_error_message , '|')
        'WHERE `signiture_key` = p_signiture_key;
        'END

        Updatecmd = New MySqlCommand("up_agent_record_state_update", Connection_mariadb_local_bc_agent)
        Updatecmd.CommandType = CommandType.StoredProcedure

        Updatecmd.Parameters.Add(New MySqlParameter("p_signiture_key", Regex.Replace(signiture_key, "^0x", "")))
        Updatecmd.Parameters.Add(New MySqlParameter("p_state", state))
        Updatecmd.Parameters.Add(New MySqlParameter("p_error_message", error_message))

        Updatecmd.ExecuteNonQuery()

        Connection_mariadb_local_bc_agent.Close()

    End Sub

    Public Shared Sub transaction_hash_update(transaction_hash As String, signiture_key As String)

        Dim Updatecmd As MySqlCommand

        Dim Connection_mariadb_local_bc_agent As New MySqlConnection(GRT.GR.cString_mariadb_local_bc_agent)

        Connection_mariadb_local_bc_agent.Open()

        'CREATE PROCEDURE `up_agent_record_transaction_hash_update`
        '(IN p_signiture_key char(64),
        'IN p_transaction_hash char(64))
        'BEGIN
        'UPDATE agent_record
        'SET transaction_hash = p_transaction_hash
        'WHERE `signiture_key` = p_signiture_key;
        'END

        Updatecmd = New MySqlCommand("up_agent_record_transaction_hash_update", Connection_mariadb_local_bc_agent)
        Updatecmd.CommandType = CommandType.StoredProcedure

        Updatecmd.Parameters.Add(New MySqlParameter("p_signiture_key", Regex.Replace(signiture_key, "^0x", "")))
        Updatecmd.Parameters.Add(New MySqlParameter("p_transaction_hash", Regex.Replace(transaction_hash, "^0x", "")))

        Updatecmd.ExecuteNonQuery()

        Connection_mariadb_local_bc_agent.Close()

    End Sub

    Public Shared Sub transaction_hash_cancel_update(transaction_hash_cancel As String, signiture_key As String)

        Dim Updatecmd As MySqlCommand

        Dim Connection_mariadb_local_bc_agent As New MySqlConnection(GRT.GR.cString_mariadb_local_bc_agent)

        Connection_mariadb_local_bc_agent.Open()

        'CREATE PROCEDURE `up_agent_record_transaction_hash_cancel_update`
        '(IN p_signiture_key char(64),
        'IN p_transaction_hash_cancel char(64))
        'BEGIN
        'UPDATE agent_record
        'SET transaction_hash_cancel = p_transaction_hash_cancel
        'WHERE `signiture_key` = p_signiture_key;
        'END

        Updatecmd = New MySqlCommand("up_agent_record_transaction_hash_cancel_update", Connection_mariadb_local_bc_agent)
        Updatecmd.CommandType = CommandType.StoredProcedure

        Updatecmd.Parameters.Add(New MySqlParameter("p_signiture_key", Regex.Replace(signiture_key, "^0x", "")))
        Updatecmd.Parameters.Add(New MySqlParameter("p_transaction_hash_cancel", Regex.Replace(transaction_hash_cancel, "^0x", "")))

        Updatecmd.ExecuteNonQuery()

        Connection_mariadb_local_bc_agent.Close()

    End Sub

    Public Shared Sub save_receipt(receipt_string As String, signiture_key As String)

        Dim Updatecmd As MySqlCommand

        Dim Connection_mariadb_local_bc_agent As New MySqlConnection(GRT.GR.cString_mariadb_local_bc_agent)

        Connection_mariadb_local_bc_agent.Open()

        'CREATE PROCEDURE `up_agent_record_save_receipt`
        '(IN p_signiture_key char(64),
        'IN p_receipt text)
        'BEGIN
        'UPDATE agent_record
        'SET receipt = p_receipt
        'WHERE `signiture_key` = p_signiture_key;
        'END

        Updatecmd = New MySqlCommand("up_agent_record_save_receipt", Connection_mariadb_local_bc_agent)
        Updatecmd.CommandType = CommandType.StoredProcedure

        Updatecmd.Parameters.Add(New MySqlParameter("p_signiture_key", Regex.Replace(signiture_key, "^0x", "")))
        Updatecmd.Parameters.Add(New MySqlParameter("p_receipt", receipt_string))

        Updatecmd.ExecuteNonQuery()

        Connection_mariadb_local_bc_agent.Close()

    End Sub

    Public Shared Sub save_receipt_cancel(receipt_cancel_string As String, signiture_key As String)

        Dim Updatecmd As MySqlCommand

        Dim Connection_mariadb_local_bc_agent As New MySqlConnection(GRT.GR.cString_mariadb_local_bc_agent)

        Connection_mariadb_local_bc_agent.Open()

        'CREATE PROCEDURE `up_agent_record_save_receipt_cancel`
        '(IN p_signiture_key char(64),
        'IN p_receipt_cancel text)
        'BEGIN
        'UPDATE agent_record
        'SET receipt_cancel = p_receipt_cancel
        'WHERE `signiture_key` = p_signiture_key;
        'END

        Updatecmd = New MySqlCommand("up_agent_record_save_receipt_cancel", Connection_mariadb_local_bc_agent)
        Updatecmd.CommandType = CommandType.StoredProcedure

        Updatecmd.Parameters.Add(New MySqlParameter("p_signiture_key", Regex.Replace(signiture_key, "^0x", "")))
        Updatecmd.Parameters.Add(New MySqlParameter("p_receipt_cancel", receipt_cancel_string))

        Updatecmd.ExecuteNonQuery()

        Connection_mariadb_local_bc_agent.Close()

    End Sub

    Public Shared Sub confirm(block_number As Long, signiture_key As String)

        Dim Selectcmd, Updatecmd As MySqlCommand
        Dim Adapter As MySqlDataAdapter
        Dim Dataset As DataSet
        Dim idate_generated, idate_confirmed As DateTime
        Dim time_taken As TimeSpan

        Dim Connection_mariadb_local_bc_agent As New MySqlConnection(GRT.GR.cString_mariadb_local_bc_agent)

        Connection_mariadb_local_bc_agent.Open()

        'CREATE PROCEDURE `up_select_idate_generated`
        '(IN p_signiture_key char(64))
        'BEGIN
        'SELECT idate_generated
        'FROM agent_record
        'WHERE
        '`signiture_key` = p_signiture_key;
        'END

        Selectcmd = New MySqlCommand("up_select_idate_generated", Connection_mariadb_local_bc_agent)
        Selectcmd.CommandType = CommandType.StoredProcedure
        Selectcmd.Parameters.Add(New MySqlParameter("p_signiture_key", Regex.Replace(signiture_key, "^0x", "")))

        Adapter = New MySqlDataAdapter
        Selectcmd.CommandType = CommandType.StoredProcedure
        Adapter.SelectCommand = Selectcmd
        Dataset = New DataSet
        Adapter.Fill(Dataset)

        If Dataset.Tables(0).Rows.Count = 1 Then

            idate_generated = CDate(CType(Dataset.Tables(0).Rows(0)(0), MySqlDateTime))

            idate_confirmed = DateTime.Now

            time_taken = idate_confirmed - idate_generated

            'CREATE PROCEDURE `up_confirm_agent_record`
            '(IN p_signiture_key char(64),
            'IN p_block_number bigint(20),
            'IN p_idate_confirmed datetime,
            'IN p_time_taken time)
            'BEGIN
            'UPDATE agent_record
            'SET state = CONCAT(state , '|' , 'confirmed'), block_number = p_block_number, idate_confirmed = p_idate_confirmed, time_taken = p_time_taken
            'WHERE `signiture_key` = p_signiture_key;
            'END

            Updatecmd = New MySqlCommand("up_confirm_agent_record", Connection_mariadb_local_bc_agent)
            Updatecmd.CommandType = CommandType.StoredProcedure

            Updatecmd.Parameters.Add(New MySqlParameter("p_signiture_key", Regex.Replace(signiture_key, "^0x", "")))
            Updatecmd.Parameters.Add(New MySqlParameter("p_block_number", block_number))
            Updatecmd.Parameters.Add(New MySqlParameter("p_idate_confirmed", idate_confirmed.ToString("yyyy/MM/dd HH:mm:ss")))
            Updatecmd.Parameters.Add(New MySqlParameter("p_time_taken", Regex.Replace(time_taken.ToString(), "\.\d*$", "")))

            Updatecmd.ExecuteNonQuery()

            ' 그리고 이곳에서 이 모든 결과를 감독관 서버에 알린다.
            ' 감독관 서버는 자체적으로 내용을 분석해서 메인서버 교체 여부를 판단한다. 

        End If

        Connection_mariadb_local_bc_agent.Close()

    End Sub

End Class
