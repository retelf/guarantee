Imports System.ServiceProcess
Imports System.Windows.Forms

Public Class check_duplicated_block_hash

    Public Shared Function exe(block_hash As String) As Boolean

        Dim Selectcmd As MySqlCommand
        Dim Adapter As MySqlDataAdapter
        Dim Dataset As DataSet

        Dim Connection_local_bc As New MySqlConnection(GRT.GR.cString_mariadb_local_bc)

        Connection_local_bc.Open()

        'CREATE PROCEDURE `up_check_dupplicated_block_hash`
        '(IN p_block_hash varchar(64))
        'BEGIN
        'SELECT COUNT(*)
        'FROM main
        'WHERE
        '`block_hash` = p_block_hash;
        'END

        Selectcmd = New MySqlCommand("up_check_dupplicated_block_hash", Connection_local_bc)
        Selectcmd.CommandType = CommandType.StoredProcedure
        Selectcmd.Parameters.Add(New MySqlParameter("p_block_hash", Regex.Replace(block_hash, "^0x", "")))

        Adapter = New MySqlDataAdapter
        Selectcmd.CommandType = CommandType.StoredProcedure
        Adapter.SelectCommand = Selectcmd
        Dataset = New DataSet
        Adapter.Fill(Dataset)

        If CInt(Dataset.Tables(0).Rows(0)(0)) > 0 Then
            Return True
        Else
            Return False
        End If

    End Function

End Class
