Imports System.ServiceProcess
Imports System.Windows.Forms

Public Class check_duplicated_signiture_key

    Public Shared Function exe(signiture As String) As Boolean

        Dim signiture_key = Regex.Match(signiture, "^0x.{64}").ToString

        Dim Selectcmd As MySqlCommand
        Dim Adapter As MySqlDataAdapter
        Dim Dataset As DataSet

        Dim Connection_local_bc As New MySqlConnection(GRT.GR.cString_mariadb_local_bc)

        Connection_local_bc.Open()

        'CREATE PROCEDURE `up_check_dupplicated_signiture_key`
        '(IN p_signiture_key varchar(64))
        'BEGIN
        'SELECT COUNT(*)
        'FROM main
        'WHERE
        '`signiture_key` = p_signiture_key;
        'END

        Selectcmd = New MySqlCommand("up_check_dupplicated_signiture_key", Connection_local_bc)
        Selectcmd.CommandType = CommandType.StoredProcedure
        Selectcmd.Parameters.Add(New MySqlParameter("p_signiture_key", Regex.Replace(signiture_key, "^0x", "")))

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
