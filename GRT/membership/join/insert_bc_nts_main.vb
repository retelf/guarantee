Public Class insert_bc_nts_main

    Public Shared Function exe(
                              block_number As Integer,
                              block_hash As String,
                              eoa As String,
                              database_name As String,
                              table_name As String,
                              query_type As String,
                              contract_type As String,
                              query_string As String,
                              signiture As String,
                              algorithm As String,
                              idate As Date) As String

        Dim Connection_mariadb_bc_nts As New MySqlConnection(GR.cString_mariadb_bc_nts)

        Connection_mariadb_bc_nts.Open()

        Dim Insertcmd As MySqlCommand

        'CREATE PROCEDURE up_insert_nts_main

        '(IN block_number bigint(20),
        'IN block_hash char(64),
        'IN eoa char(40), 
        'IN database_name varchar(50),
        'IN table_name varchar(50),
        'IN contract_type varchar(50),
        'IN query_type varchar(50),
        'IN query_string text,
        'IN signiture text,
        'IN algorithm text,
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
        '`algorithm`,
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
        'algorithm,
        'idate
        ');
        'End

        Insertcmd = New MySqlCommand("up_insert_nts_main", Connection_mariadb_bc_nts)
        Insertcmd.CommandType = CommandType.StoredProcedure

        Insertcmd.Parameters.Add(New MySqlParameter("block_number", block_number))
        Insertcmd.Parameters.Add(New MySqlParameter("block_hash", block_hash))
        Insertcmd.Parameters.Add(New MySqlParameter("eoa", Regex.Replace(eoa, "^0x", "")))
        Insertcmd.Parameters.Add(New MySqlParameter("database_name", database_name))
        Insertcmd.Parameters.Add(New MySqlParameter("table_name", table_name))
        Insertcmd.Parameters.Add(New MySqlParameter("query_type", query_type))
        Insertcmd.Parameters.Add(New MySqlParameter("contract_type", contract_type))
        Insertcmd.Parameters.Add(New MySqlParameter("query_string", query_string))
        Insertcmd.Parameters.Add(New MySqlParameter("signiture", signiture))
        Insertcmd.Parameters.Add(New MySqlParameter("algorithm", algorithm))
        Insertcmd.Parameters.Add(New MySqlParameter("idate", idate))

        Insertcmd.ExecuteNonQuery()

        Connection_mariadb_bc_nts.Close()

    End Function

End Class
