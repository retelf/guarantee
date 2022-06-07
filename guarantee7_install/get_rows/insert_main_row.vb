Public Class insert_main_row

    Public Shared Function exe(block_number As Long, block_hash As String, eoa As String, contract_type As String, query_type As String, query_string As String, signiture As String, idate As Date) As String

        Dim Insertcmd As MySqlCommand

        'CREATE PROCEDURE up_insert_main

        '(IN block_number bigint(20),
        'IN block_hash char(64),
        'IN eoa char(40), 
        'IN contract_type varchar(50),
        'IN query_type varchar(50),
        'IN query_string text,
        'IN signiture text,
        'IN idate datetime)

        'BEGIN
        'INSERT INTO main
        '(
        '`block_number`,
        '`block_hash`,
        '`eoa`,
        '`contract_type`,
        '`query_type`,
        '`query_string`,
        '`signiture`,
        '`idate`
        ')
        'VALUES
        '(
        'block_number,
        'block_hash,
        'eoa,
        'contract_type,
        'query_type,
        'query_string,
        'signiture,
        'idate
        ');
        'End

        Insertcmd = New MySqlCommand("up_insert_main",GRT.GR.Connection_mariadb_local_bc)
        Insertcmd.CommandType = CommandType.StoredProcedure

        Insertcmd.Parameters.Add(New MySqlParameter("block_number", block_number))
        Insertcmd.Parameters.Add(New MySqlParameter("block_hash", block_hash))
        Insertcmd.Parameters.Add(New MySqlParameter("eoa", eoa))
        Insertcmd.Parameters.Add(New MySqlParameter("contract_type", contract_type))
        Insertcmd.Parameters.Add(New MySqlParameter("query_type", query_type))
        Insertcmd.Parameters.Add(New MySqlParameter("query_string", query_string))
        Insertcmd.Parameters.Add(New MySqlParameter("signiture", signiture))
        Insertcmd.Parameters.Add(New MySqlParameter("idate", idate))

        Insertcmd.ExecuteNonQuery()

    End Function

End Class
