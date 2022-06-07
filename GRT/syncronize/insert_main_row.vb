Public Class insert_main_row

    Public Shared Function exe(block_number As Long,
                               block_hash As String,
                               eoa As String,
                               database_name As String,
                               table_name As String,
                               contract_type As String,
                               query_type As String,
                               query_string As String,
                               signiture As String,
                               idate As Date,
                               Connection_mariadb_local_bc As MySqlConnection) As String

        Dim Insertcmd As MySqlCommand

        'CREATE PROCEDURE up_insert_bc_main

        '(IN block_number bigint(20),
        'IN block_hash char(64),
        'IN eoa char(40), 
        'IN database_name varchar(50),
        'IN table_name varchar(50),
        'IN contract_type varchar(50),
        'IN query_type varchar(50),
        'IN query_string text,
        'IN signiture text,
        'IN signiture_key char(64),
        'IN idate datetime)

        'BEGIN
        'INSERT INTO main
        '(
        '`block_number`,
        '`block_hash`,
        '`eoa`,
        '`database_name`,
        '`table_name`,
        '`contract_type`,
        '`query_type`,
        '`query_string`,
        '`signiture`,
        '`signiture_key`,
        '`idate`
        ')
        'VALUES
        '(
        'block_number,
        'block_hash,
        'eoa,
        'database_name,
        'table_name,
        'contract_type,
        'query_type,
        'query_string,
        'signiture,
        'signiture_key,
        'idate
        ');
        'End

        Insertcmd = New MySqlCommand("up_insert_bc_main", Connection_mariadb_local_bc)
        Insertcmd.CommandType = CommandType.StoredProcedure

        Insertcmd.Parameters.Add(New MySqlParameter("block_number", block_number))
        Insertcmd.Parameters.Add(New MySqlParameter("block_hash", block_hash))
        Insertcmd.Parameters.Add(New MySqlParameter("eoa", eoa))
        Insertcmd.Parameters.Add(New MySqlParameter("database_name", database_name))
        Insertcmd.Parameters.Add(New MySqlParameter("table_name", table_name))
        Insertcmd.Parameters.Add(New MySqlParameter("query_type", query_type))
        Insertcmd.Parameters.Add(New MySqlParameter("contract_type", contract_type))
        Insertcmd.Parameters.Add(New MySqlParameter("query_string", query_string))
        Insertcmd.Parameters.Add(New MySqlParameter("signiture", signiture))
        Insertcmd.Parameters.Add(New MySqlParameter("signiture_key", Regex.Replace(Regex.Match(signiture, "^0x.{64}").ToString, "^0x", "")))
        Insertcmd.Parameters.Add(New MySqlParameter("idate", DateTime.Today))

        Insertcmd.ExecuteNonQuery()

    End Function

End Class
