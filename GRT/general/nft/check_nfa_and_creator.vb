Public Class check_nfa_and_creator

    Public Shared Function exe(nfa As String) As String

        Dim Selectcmd As MySqlCommand
        Dim Adapter As MySqlDataAdapter
        Dim Dataset As DataSet

        Dim JRS, name, creator As String

        Dim Connection_mariadb_local_bc_nft As New MySqlConnection(GRT.GR.cString_mariadb_local_bc_nft)

        Connection_mariadb_local_bc_nft.Open()

        'CREATE PROCEDURE `up_select_account`
        '(IN p_nfa char(40))
        'BEGIN
        'SELECT * FROM account
        'WHERE `nfa` = p_nfa;
        'END

        Selectcmd = New MySqlCommand("up_select_account", Connection_mariadb_local_bc_nft)
        Selectcmd.CommandType = CommandType.StoredProcedure
        Selectcmd.Parameters.Add(New MySqlParameter("p_nfa", Regex.Replace(nfa, "^0x", "")))

        Adapter = New MySqlDataAdapter
        Adapter.SelectCommand = Selectcmd
        Dataset = New DataSet
        Adapter.Fill(Dataset)

        If Dataset.Tables(0).Rows.Count = 1 Then

            name = CStr(Dataset.Tables(0).Rows(0)("name"))
            creator = CStr(Dataset.Tables(0).Rows(0)("creator"))

            JRS = GRT.make_json_string.exe({{"key", "check_nfa_and_creator", "quot"}, {"success", "success", "quot"}}, {{"name", name, "quot"}, {"creator", creator, "quot"}}, False)

        Else

            JRS = GRT.make_json_string.exe({{"key", "check_nfa_and_creator", "quot"}, {"success", "fail", "quot"}}, {{"reason", "no_rows", "quot"}}, False)

        End If

        Return JRS

    End Function

End Class
