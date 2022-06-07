Imports MySqlConnector

Public Class insert_sample_data
    Public Shared Sub node(Connection_to_insert As MySqlConnection, Connection_sample_database As MySqlConnection)

        Dim sString, iString, server_type As String
        Dim Selectcmd, Insertcmd As MySqlCommand
        Dim Adapter As MySqlDataAdapter
        Dim Dataset As DataSet

        Dim count As Integer

        Dim block_number As Long
        Dim na, eoa, type, domain, ip, parent, eoa_type, coin_name As String
        Dim port, level, count_children, point As Integer
        Dim balance, locked As Decimal
        Dim idate As Date
        Dim main, running As Boolean

        If Regex.Match(Connection_to_insert.ConnectionString, "server=192.168.0.251;port=3306;", RegexOptions.IgnoreCase).Success Then
            server_type = "nts"
        Else
            server_type = "general"
        End If

        If server_type = "nts" Then
            ' 아직 할 것이 없다.
        Else

            ' 일단은 USE bc_manager; SELECT * FROM node; 만 한다. 추후 증가될 수 있다.

            sString =
                "USE bc_manager; " &
                "SELECT * FROM node;"

            Selectcmd = New MySqlCommand(sString, Connection_sample_database)
            Selectcmd.CommandType = CommandType.Text

            Adapter = New MySqlDataAdapter
            Adapter.SelectCommand = Selectcmd
            Dataset = New DataSet
            Adapter.Fill(Dataset)

            count = Dataset.Tables(0).Rows.Count

            For i = 0 To count - 1

                block_number = CLng(Dataset.Tables(0).Rows(i)("block_number"))
                na = CStr(Dataset.Tables(0).Rows(i)("na"))
                eoa = CStr(Dataset.Tables(0).Rows(i)("eoa"))
                type = CStr(Dataset.Tables(0).Rows(i)("type"))
                main = CBool(Dataset.Tables(0).Rows(i)("main"))
                domain = CStr(Dataset.Tables(0).Rows(i)("domain"))
                ip = CStr(Dataset.Tables(0).Rows(i)("ip"))
                port = CInt(Dataset.Tables(0).Rows(i)("port"))
                parent = CStr(Dataset.Tables(0).Rows(i)("parent"))
                count_children = CInt(Dataset.Tables(0).Rows(i)("count_children"))
                level = CInt(Dataset.Tables(0).Rows(i)("level"))
                point = CInt(Dataset.Tables(0).Rows(i)("point"))
                running = CBool(Dataset.Tables(0).Rows(i)("running"))
                idate = CDate(Dataset.Tables(0).Rows(i)("idate"))

                iString =
                    "USE bc_manager; " &
                    "INSERT INTO node" &
                    "(" &
                    "`block_number`, " &
                    "`na`, " &
                    "`eoa`, " &
                    "`type`, " &
                    "`main`, " &
                    "`domain`, " &
                    "`ip`, " &
                    "`port`, " &
                    "`parent`, " &
                    "`count_children`, " &
                    "`level`, " &
                    "`point`, " &
                    "`running`, " &
                    "`idate`" &
                    ")" &
                    "VALUES" &
                    "(" &
                    "" & block_number & ", " &
                    "'" & na & "', " &
                    "'" & eoa & "', " &
                    "'" & type & "', " &
                    "" & main & ", " &
                    "'" & domain & "', " &
                    "'" & ip & "', " &
                    "" & port & ", " &
                    "'" & parent & "', " &
                    "" & count_children & ", " &
                    "" & level & ", " &
                    "" & point & ", " &
                    "" & running & ", " &
                    "'" & idate.ToString("yyyy/MM/dd HH:mm:ss") &
                    "')"

                Insertcmd = New MySqlCommand(iString, Connection_to_insert)
                Insertcmd.CommandType = CommandType.Text
                Insertcmd.ExecuteNonQuery()

            Next

            sString =
                "USE bc; " &
                "SELECT * FROM account;"

            Selectcmd = New MySqlCommand(sString, Connection_sample_database)
            Selectcmd.CommandType = CommandType.Text

            Adapter = New MySqlDataAdapter
            Adapter.SelectCommand = Selectcmd
            Dataset = New DataSet
            Adapter.Fill(Dataset)

            count = Dataset.Tables(0).Rows.Count

            For i = 0 To count - 1

                block_number = CLng(Dataset.Tables(0).Rows(i)("block_number"))
                eoa = CStr(Dataset.Tables(0).Rows(i)("eoa"))
                eoa_type = CStr(Dataset.Tables(0).Rows(i)("eoa_type"))
                coin_name = CStr(Dataset.Tables(0).Rows(i)("coin_name"))
                balance = CDec(Dataset.Tables(0).Rows(i)("balance"))
                locked = CDec(Dataset.Tables(0).Rows(i)("locked"))
                idate = CDate(Dataset.Tables(0).Rows(i)("idate"))

                iString =
                    "USE bc; " &
                    "INSERT INTO account" &
                    "(" &
                    "`block_number`, " &
                    "`eoa`, " &
                    "`eoa_type`, " &
                    "`coin_name`, " &
                    "`balance`, " &
                    "`locked`, " &
                    "`idate`" &
                    ")" &
                    "VALUES" &
                    "(" &
                    "" & block_number & ", " &
                    "'" & eoa & "', " &
                    "'" & eoa_type & "', " &
                    "'" & coin_name & "', " &
                    "" & balance & ", " &
                    "" & locked & ", " &
                    "'" & idate.ToString("yyyy/MM/dd HH:mm:ss") &
                    "')"

                Insertcmd = New MySqlCommand(iString, Connection_to_insert)
                Insertcmd.CommandType = CommandType.Text
                Insertcmd.ExecuteNonQuery()

            Next

        End If

    End Sub
    Public Shared Sub private_key(private_key As String, Connection_to_insert As MySqlConnection, Connection_sample_database As MySqlConnection)

        Dim iString As String
        Dim Insertcmd As MySqlCommand
        Dim idate As Date

        ' 일단은 USE bc_manager; SELECT * FROM node; 만 한다. 추후 증가될 수 있다.

        iString =
                    "USE bc; " &
                    "INSERT INTO key_store" &
                    "(" &
                    "`password`, " &
                    "`private_key`, " &
                    "`idate`" &
                    ")" &
                    "VALUES" &
                    "(" &
                    "'password', " &
                    "'" & Regex.Replace(private_key, "0x", "") & "', " &
                    "'" & Date.Now.ToString("yyyy/MM/dd HH:mm:ss") &
                    "')"

        Insertcmd = New MySqlCommand(iString, Connection_to_insert)
        Insertcmd.CommandType = CommandType.Text
        Insertcmd.ExecuteNonQuery()

    End Sub

End Class