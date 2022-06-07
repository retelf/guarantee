Public Class set_initial_variables

    Public Shared Sub exe(local_password As String)

        GR.remote_server_address = "guarantee7.com"

        'GR.port_number_database_remote = 3306
        GR.port_number_database_remote = 30601
        GR.port_number_server_remote = 40701

        GR.port_number_database_local = set_initial_port_number.database
        GR.port_number_server_local = set_initial_port_number.server

        GR.cString_mariadb_remote = "server=" & GR.remote_server_address & ";port=" & GR.port_number_database_remote & ";database=mysql;uid=root;pwd=#cyndi$36%;CharSet=utf8;"
        GR.Connection_mariadb_remote = New MySqlConnection(GR.cString_mariadb_remote)
        GR.cString_mariadb_remote_bc = "server=" & GR.remote_server_address & ";port=" & GR.port_number_database_remote & ";database=bc;uid=root;pwd=#cyndi$36%;CharSet=utf8;"
        GR.Connection_mariadb_remote_bc = New MySqlConnection(GR.cString_mariadb_remote_bc)
        GR.cString_mariadb_remote_bc_manager = "server=" & GR.remote_server_address & ";port=" & GR.port_number_database_remote & ";database=bc_manager;uid=root;pwd=#cyndi$36%;CharSet=utf8;"
        GR.Connection_mariadb_remote_bc_manager = New MySqlConnection(GR.cString_mariadb_remote_bc_manager)
        GR.cString_mariadb_remote_bc_multilevel_contract = "server=" & GR.remote_server_address & ";port=" & GR.port_number_database_remote & ";database=bc_multilevel_contract;uid=root;pwd=#cyndi$36%;CharSet=utf8;"
        GR.Connection_mariadb_remote_bc_multilevel_contract = New MySqlConnection(GR.cString_mariadb_remote_bc_multilevel_contract)

        GR.cString_mariadb_local = "server=localhost;port=" & GR.port_number_database_local & ";database=mysql;uid=guarantee7;pwd=" & local_password & ";CharSet=utf8;"
        GR.Connection_mariadb_local = New MySqlConnection(GR.cString_mariadb_local)
        GR.cString_mariadb_local_bc = "server=localhost;port=" & GR.port_number_database_local & ";database=bc;uid=guarantee7;pwd=" & local_password & ";CharSet=utf8;"
        GR.Connection_mariadb_local_bc = New MySqlConnection(GR.cString_mariadb_local_bc)
        GR.cString_mariadb_local_bc_manager = "server=localhost;port=" & GR.port_number_database_local & ";database=bc_manager;uid=guarantee7;pwd=" & local_password & ";CharSet=utf8;"
        GR.Connection_mariadb_local_bc_manager = New MySqlConnection(GR.cString_mariadb_local_bc_manager)
        GR.cString_mariadb_local_bc_multilevel_contract = "server=localhost;port=" & GR.port_number_database_local & ";database=bc_multilevel_contract;uid=guarantee7;pwd=" & local_password & ";CharSet=utf8;"
        GR.Connection_mariadb_local_bc_multilevel_contract = New MySqlConnection(GR.cString_mariadb_local_bc_multilevel_contract)

    End Sub

    Public Shared Sub on_install(local_password As String)

        GR.remote_server_address = "guarantee7.com"

        'GR.port_number_database_remote = 3306
        GR.port_number_database_remote = 30601
        GR.port_number_server_remote = 40701

        GR.port_number_database_local = set_initial_port_number.database
        GR.port_number_server_local = set_initial_port_number.server

        GR.cString_mariadb_remote = "server=" & GR.remote_server_address & ";port=" & GR.port_number_database_remote & ";database=mysql;uid=root;pwd=#cyndi$36%;CharSet=utf8;"
        GR.Connection_mariadb_remote = New MySqlConnection(GR.cString_mariadb_remote)

        GR.cString_mariadb_local = "server=localhost;port=" & GR.port_number_database_local & ";database=mysql;uid=guarantee7;pwd=" & local_password & ";CharSet=utf8;"
        GR.Connection_mariadb_local = New MySqlConnection(GR.cString_mariadb_local)

    End Sub

End Class
