Public Class agent_record_old

    Public Shared Sub generate(signiture As String, eoa As String, contract_type As String, pure_query As String, idate_generated As DateTime)

        Dim signiture_key = Regex.Match(signiture, "^0x.{64}").ToString

        Dim Connection_mariadb_local_bc_manager As New MySqlConnection(GRT.GR.cString_mariadb_local_bc_manager)

        Connection_mariadb_local_bc_manager.Open()

        Dim Insertcmd As MySqlCommand

        'CREATE PROCEDURE up_insert_agent_record

        '(IN signiture_key char(64),
        'IN signiture text,
        'IN eoa text,
        'IN contract_type varchar(50),
        'IN query_string text,  
        'IN state varchar(50), 
        'IN idate_generated datetime)

        'BEGIN
        'INSERT INTO agent_record
        '(
        '`block_number`,
        '`signiture_key`,
        '`signiture`,
        '`eoa`,
        '`contract_type`,    
        '`query_string`,  
        '`state`, 
        '`idate_generated`
        ')
        'VALUES
        '(
        '0,
        'signiture_key,
        'signiture,
        'eoa,
        'contract_type,
        'query_string,
        'state,
        'idate_generated
        ');
        'End

        Insertcmd = New MySqlCommand("up_insert_agent_record", Connection_mariadb_local_bc_manager)
        Insertcmd.CommandType = CommandType.StoredProcedure

        Insertcmd.Parameters.Add(New MySqlParameter("signiture_key", Regex.Replace(signiture_key, "^0x", "")))
        Insertcmd.Parameters.Add(New MySqlParameter("signiture", signiture))
        Insertcmd.Parameters.Add(New MySqlParameter("eoa", Regex.Replace(eoa, "^0x", "")))
        Insertcmd.Parameters.Add(New MySqlParameter("contract_type", contract_type))
        Insertcmd.Parameters.Add(New MySqlParameter("query_string", pure_query))
        Insertcmd.Parameters.Add(New MySqlParameter("state", "unconfirmed"))
        Insertcmd.Parameters.Add(New MySqlParameter("idate_generated", idate_generated.ToString("yyyy/MM/dd HH:mm:ss")))

        Insertcmd.ExecuteNonQuery()

        Connection_mariadb_local_bc_manager.Close()

        ' 그리고 이곳에서 이 내용을 감독관 서버에 알린다.

    End Sub

    Public Shared Sub confirm(block_number As Long, signiture As String)

        Dim Selectcmd, Updatecmd As MySqlCommand
        Dim Adapter As MySqlDataAdapter
        Dim Dataset As DataSet
        Dim idate_generated, idate_confirmed As DateTime
        Dim time_taken As TimeSpan

        Dim signiture_key = Regex.Match(signiture, "^0x.{64}").ToString

        Dim Connection_mariadb_local_bc_manager As New MySqlConnection(GRT.GR.cString_mariadb_local_bc_manager)

        Connection_mariadb_local_bc_manager.Open()

        'CREATE PROCEDURE `up_select_idate_generated`
        '(IN p_signiture_key varchar(64))
        'BEGIN
        'SELECT idate_generated
        'FROM agent_record
        'WHERE
        '`signiture_key` = p_signiture_key;
        'END

        Selectcmd = New MySqlCommand("up_select_idate_generated", Connection_mariadb_local_bc_manager)
        Selectcmd.CommandType = CommandType.StoredProcedure
        Selectcmd.Parameters.Add(New MySqlParameter("p_signiture_key", Regex.Replace(signiture_key, "^0x", "")))

        Adapter = New MySqlDataAdapter
        Selectcmd.CommandType = CommandType.StoredProcedure
        Adapter.SelectCommand = Selectcmd
        Dataset = New DataSet
        Adapter.Fill(Dataset)

        If Dataset.Tables(0).Rows.Count = 1 Then

            idate_generated = CDate(Dataset.Tables(0).Rows(0)(0))

            idate_confirmed = DateTime.Now

            time_taken = idate_confirmed - idate_generated

            'CREATE PROCEDURE `up_confirm_agent_record`
            '(IN p_signiture_key varchar(64),
            'IN p_block_number bigint(20),
            'IN p_idate_confirmed datetime,
            'IN p_time_taken time)
            'BEGIN
            'UPDATE agent_record
            'SET state = 'confirmed', block_number = p_block_number, idate_confirmed = p_idate_confirmed, time_taken = p_time_taken
            'WHERE `signiture_key` = p_signiture_key;
            'END

            Updatecmd = New MySqlCommand("up_confirm_agent_record", Connection_mariadb_local_bc_manager)
            Updatecmd.CommandType = CommandType.StoredProcedure

            Updatecmd.Parameters.Add(New MySqlParameter("p_signiture_key", Regex.Replace(signiture_key, "^0x", "")))
            Updatecmd.Parameters.Add(New MySqlParameter("p_block_number", block_number))
            Updatecmd.Parameters.Add(New MySqlParameter("p_idate_confirmed", idate_confirmed.ToString("yyyy/MM/dd HH:mm:ss")))
            Updatecmd.Parameters.Add(New MySqlParameter("p_time_taken", Regex.Replace(time_taken.ToString(), "\.\d*$", "")))

            Updatecmd.ExecuteNonQuery()

            Connection_mariadb_local_bc_manager.Close()

            ' 그리고 이곳에서 이 모든 결과를 감독관 서버에 알린다.
            ' 감독관 서버는 자체적으로 내용을 분석해서 메인서버 교체 여부를 판단한다. 

        End If

    End Sub

End Class
