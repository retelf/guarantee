Imports System.ServiceProcess
Imports System.Windows.Forms

Public Class get_server_login_info

    Public Structure st_info

        Public Structure st_server

            Dim eoa As String
            Dim balance As Decimal
            Dim exchange_name As String

            Dim domain As String
            Dim domain_agency As String
            Dim domain_web As String
            Dim domain_ethereum As String
            Dim domain_management As String
            Dim domain_nft As String

            Dim ip As String
            Dim ip_agency As String
            Dim ip_web As String
            Dim ip_ethereum As String
            Dim ip_management As String
            Dim ip_nft As String

            Dim port As Integer
            Dim port_agency As Integer
            Dim port_web As Integer
            Dim port_ethereum As Integer
            Dim port_management As Integer
            Dim port_nft As Integer

            Dim is_main_server, registered_node, registered_account, server_owner_login As Boolean
            Dim message As String

        End Structure : Dim server As st_server

    End Structure : Shared local_server_info As st_info

    Public Shared Function exe(coin_name As String, public_key As String) As st_info

        ' 초기화

        local_server_info.server.balance = Nothing
        local_server_info.server.exchange_name = Nothing

        local_server_info.server.is_main_server = Nothing
        local_server_info.server.registered_node = Nothing
        local_server_info.server.registered_account = Nothing
        local_server_info.server.server_owner_login = Nothing
        local_server_info.server.message = Nothing

        local_server_info.server.domain = Nothing
        local_server_info.server.domain_agency = Nothing
        local_server_info.server.domain_web = Nothing
        local_server_info.server.domain_ethereum = Nothing
        local_server_info.server.domain_management = Nothing
        local_server_info.server.domain_nft = Nothing

        local_server_info.server.ip = Nothing
        local_server_info.server.ip_agency = Nothing
        local_server_info.server.ip_web = Nothing
        local_server_info.server.ip_ethereum = Nothing
        local_server_info.server.ip_management = Nothing
        local_server_info.server.ip_nft = Nothing

        local_server_info.server.port = Nothing
        local_server_info.server.port_agency = Nothing
        local_server_info.server.port_web = Nothing
        local_server_info.server.port_ethereum = Nothing
        local_server_info.server.port_management = Nothing
        local_server_info.server.port_nft = Nothing

        Dim Selectcmd As MySqlCommand
        Dim Adapter As MySqlDataAdapter
        Dim Dataset, Dataset_node_thread As DataSet

        Dim Connection_mariadb_local_bc_manager As New MySqlConnection(GRT.GR.cString_mariadb_local_bc_manager)

        Connection_mariadb_local_bc_manager.Open()

        'CREATE PROCEDURE `up_select_node_info`
        '(IN p_na char(40))
        'BEGIN
        'SELECT * FROM node
        'WHERE na = p_na;
        'END

        Selectcmd = New MySqlCommand("up_select_node_info", Connection_mariadb_local_bc_manager)
        Selectcmd.Parameters.Add(New MySqlParameter("p_na", Regex.Replace(GR.server_id, "^0x", "")))

        Adapter = New MySqlDataAdapter
        Selectcmd.CommandType = CommandType.StoredProcedure
        Adapter.SelectCommand = Selectcmd
        Dataset = New DataSet
        Adapter.Fill(Dataset)

        'CREATE PROCEDURE `up_select_node_thread_info`
        '(IN p_na char(40))
        'BEGIN
        'SELECT * FROM node_thread
        'WHERE na = p_na;
        'END

        Selectcmd = New MySqlCommand("up_select_node_thread_info", Connection_mariadb_local_bc_manager)
        Selectcmd.Parameters.Add(New MySqlParameter("p_na", Regex.Replace(GR.server_id, "^0x", "")))

        Adapter = New MySqlDataAdapter
        Selectcmd.CommandType = CommandType.StoredProcedure
        Adapter.SelectCommand = Selectcmd
        Dataset_node_thread = New DataSet
        Adapter.Fill(Dataset_node_thread)

        Connection_mariadb_local_bc_manager.Close()

        If Dataset.Tables(0).Rows.Count = 1 And Dataset_node_thread.Tables(0).Rows.Count = GR.server_thread_count Then

            local_server_info.server.registered_node = True
            local_server_info.server.is_main_server = CBool(Dataset.Tables(0).Rows(0)("main"))

            local_server_info.server.eoa = CStr(Dataset.Tables(0).Rows(0)("eoa"))
            local_server_info.server.exchange_name = CStr(Dataset.Tables(0).Rows(0)("exchange_name"))

            For i = 0 To Dataset_node_thread.Tables(0).Rows.Count - 1

                Dim type = Dataset_node_thread.Tables(0).Rows(i)("type").ToString

                Select Case type

                    Case "default"

                        local_server_info.server.domain = CStr(Dataset_node_thread.Tables(0).Rows(i)("domain"))
                        local_server_info.server.ip = CStr(Dataset_node_thread.Tables(0).Rows(i)("ip"))
                        local_server_info.server.port = CInt(Dataset_node_thread.Tables(0).Rows(i)("port"))

                    Case "agency"

                        local_server_info.server.domain_agency = CStr(Dataset_node_thread.Tables(0).Rows(i)("domain"))
                        local_server_info.server.ip_agency = CStr(Dataset_node_thread.Tables(0).Rows(i)("ip"))
                        local_server_info.server.port_agency = CInt(Dataset_node_thread.Tables(0).Rows(i)("port"))

                    Case "web"

                        local_server_info.server.domain_web = CStr(Dataset_node_thread.Tables(0).Rows(i)("domain"))
                        local_server_info.server.ip_web = CStr(Dataset_node_thread.Tables(0).Rows(i)("ip"))
                        local_server_info.server.port_web = CInt(Dataset_node_thread.Tables(0).Rows(i)("port"))

                    Case "ethereum"

                        local_server_info.server.domain_ethereum = CStr(Dataset_node_thread.Tables(0).Rows(i)("domain"))
                        local_server_info.server.ip_ethereum = CStr(Dataset_node_thread.Tables(0).Rows(i)("ip"))
                        local_server_info.server.port_ethereum = CInt(Dataset_node_thread.Tables(0).Rows(i)("port"))

                    Case "management"

                        local_server_info.server.domain_management = CStr(Dataset_node_thread.Tables(0).Rows(i)("domain"))
                        local_server_info.server.ip_management = CStr(Dataset_node_thread.Tables(0).Rows(i)("ip"))
                        local_server_info.server.port_management = CInt(Dataset_node_thread.Tables(0).Rows(i)("port"))

                    Case "nft"

                        local_server_info.server.domain_nft = CStr(Dataset_node_thread.Tables(0).Rows(i)("domain"))
                        local_server_info.server.ip_nft = CStr(Dataset_node_thread.Tables(0).Rows(i)("ip"))
                        local_server_info.server.port_nft = CInt(Dataset_node_thread.Tables(0).Rows(i)("port"))

                End Select

            Next

            If local_server_info.server.eoa = Regex.Replace(public_key, "^0x", "") Then

                local_server_info.server.server_owner_login = True

                Dim Connection_mariadb_local_bc As New MySqlConnection(GRT.GR.cString_mariadb_local_bc)

                Connection_mariadb_local_bc.Open()

                Dim Dataset_bc As DataSet

                'CREATE PROCEDURE `up_select_account_info`
                '(IN p_eoa  varchar(40),
                'IN p_coin_name  varchar(50))
                'BEGIN
                'SELECT * FROM account
                'WHERE
                '`eoa` = p_eoa AND
                '`coin_name` = p_coin_name;
                'END

                Selectcmd = New MySqlCommand("up_select_account_info", Connection_mariadb_local_bc)
                Selectcmd.CommandType = CommandType.StoredProcedure
                Selectcmd.Parameters.Add(New MySqlParameter("p_eoa", Regex.Replace(public_key, "^0x", "")))
                Selectcmd.Parameters.Add(New MySqlParameter("p_coin_name", coin_name))

                Adapter = New MySqlDataAdapter
                Adapter.SelectCommand = Selectcmd
                Dataset_bc = New DataSet
                Adapter.Fill(Dataset_bc)

                Connection_mariadb_local_bc.Close()

                If Dataset_bc.Tables(0).Rows.Count = 1 Then

                    local_server_info.server.registered_account = True

                    local_server_info.server.balance = CDec(Dataset_bc.Tables(0).Rows(0)("balance"))

                Else

                    local_server_info.server.registered_account = False

                End If

            Else

                local_server_info.server.server_owner_login = False

            End If

        Else

            local_server_info.server.registered_node = False

        End If

        Return local_server_info

    End Function

End Class
