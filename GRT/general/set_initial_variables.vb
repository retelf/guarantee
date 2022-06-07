Imports System.ServiceProcess

Public Class set_initial_variables

    Public Shared Sub exe(mode As String)

        GR.foundation.public_key = "0xD3aA97a3dA0D48b1774C9ff88a429B7217A061cB"
        GR.multilevel.ma_foundation = "0x2c0E3DE43055d32543Ae68F593937a0f958Ec5BE"
        GR.multilevel.first_parent_acquisition_ratio = CDec(0.15)

        GRT.GR.account.login_state = "no_login"

        Dim local_database_password As String = get_local_database_password.exe
        GR.port_number_database_local = get_info_from_ini_file.database

        GR.address_server_local = get_info_from_ini_file.server("address_default")
        GR.address_server_local_agency = get_info_from_ini_file.server("address_agency")
        GR.address_server_local_web = get_info_from_ini_file.server("address_web")
        GR.address_server_local_ethereum = get_info_from_ini_file.server("address_ethereum")
        GR.address_server_local_management = get_info_from_ini_file.server("address_management")
        GR.address_server_local_nft = get_info_from_ini_file.server("address_nft")

        GR.port_number_server_local = CInt(get_info_from_ini_file.server("port_default"))
        GR.port_number_server_local_agency = CInt(get_info_from_ini_file.server("port_agency"))
        GR.port_number_server_local_web = CInt(get_info_from_ini_file.server("port_web"))
        GR.port_number_server_local_ethereum = CInt(get_info_from_ini_file.server("port_ethereum"))
        GR.port_number_server_local_management = CInt(get_info_from_ini_file.server("port_management"))
        GR.port_number_server_local_nft = CInt(get_info_from_ini_file.server("port_nft"))

        GR.main_server_address = "mainnet.guarantee7.com"
        GR.main_server_address_agency = "mainnet.guarantee7.com"
        GR.main_server_address_web = "mainnet.guarantee7.com"
        GR.main_server_address_ethereum = "mainnet.guarantee7.com"
        GR.main_server_address_management = "mainnet.guarantee7.com"
        GR.main_server_address_nft = "mainnet.guarantee7.com"

        GR.port_number_server_main = 40701
        GR.port_number_server_main_agency = 40711
        GR.port_number_server_main_web = 40721
        GR.port_number_server_main_ethereum = 40731
        GR.port_number_server_main_management = 40741
        GR.port_number_server_main_nft = 40751

        GR.qs_server_address = "qs.guarantee7.com"
        GR.port_number_server_qs = 40711

        GR.balance_server_address = "balance.guarantee7.com"
        GR.port_number_server_balance = 40711

        GR.exchange_server_address = "exchange.guarantee7.com"
        GR.port_number_server_exchange = 40711

        GR.authentification_server_address = "authentification.guarantee7.com"
        GR.port_number_server_authentification = 40711

        GR.send_message_server_address = "movism.com"
        GR.port_number_server_send_message = 40707

        GR.ftp_server_address = "ftp.guarantee7.com"
        GR.port_number_server_ftp = 14146

        GR.nft_server_address = "nft.guarantee7.com"
        GR.port_number_server_nft = 40701 ' 이는 데이터를 위한 것이다. 파일은 언제나 에이전시에 저장할 뿐이다.

        'GR.account.ethereum_web3_network = "mainnet"
        'GR.account.ethereum_chain_id = 1
        'GR.account.ethereum_connection_project_id = "a7a01158fe384d85b605cd5ba0b53bc3"
        'GR.account.ethereum_web3_url = "https://" & GR.account.ethereum_web3_network & ".infura.io/v3/" & GR.account.ethereum_connection_project_id

        GR.account.ethereum_web3_network = "rinkeby"
        GR.account.ethereum_chain_id = 4
        GR.account.ethereum_connection_project_id = "9aa3d95b3bc440fa88ea12eaa4456161"
        GR.account.ethereum_web3_url = "https://" & GR.account.ethereum_web3_network & ".infura.io/v3/" & GR.account.ethereum_connection_project_id

        GR.account.keyfile_folder = Regex.Replace(Directory.GetCurrentDirectory, "(guarantee7|test_guarantee)[^\\]*\\bin\\Debug\\net5.0-windows", "guarantee7_wallet\bin\Debug\net5.0-windows") & "\chain\keystore"

        'GR.nft.destination_folder = "C:\xampp\virtual\ipfs\NFT"

        'GR.account.keyfile_folder_guarantee = Regex.Replace(Directory.GetCurrentDirectory, "(guarantee7|test_guarantee)[^\\]*\\bin\\Debug\\net5.0-windows", "guarantee7_wallet\bin\Debug\net5.0-windows") & "\chain\keystore"
        'Select Case GR.account.ethereum_chain_id
        '    Case 1
        '        GR.account.keyfile_folder_ethereum = System.Environment.GetEnvironmentVariable("USERPROFILE") & "\AppData\Roaming\Ethereum\keystore"
        '    Case 4
        '        GR.account.keyfile_folder_ethereum = System.Environment.GetEnvironmentVariable("USERPROFILE") & "\AppData\Roaming\Ethereum\rinkeby\keystore"
        'End Select

        GRT.GR.ethereum.geth_running = GRT.check_process_running.exe("geth")

        If GRT.GR.ethereum.geth_running Then
            GRT.GR.ethereum.web3 = New Web3
        Else
            GRT.GR.ethereum.web3 = New Web3(GR.account.ethereum_web3_url)
        End If

        GRT.GR.multilevel.max_children_count = 7

        'GR.nts_server_address = "nts.guarantee7.com"
        'GR.port_number_server_nts = 40701

        Dim nts_password = "#cyndi$36%"
        'GR.cString_mariadb_bc_nts = "server=movism.com;port=3306;database=bc_nts;uid=root;pwd=" & nts_password & ";CharSet=utf8;Allow Zero Datetime=True;"
        GR.cString_mariadb_bc_nts = "server=192.168.0.251;port=3306;database=mysql;uid=root;pwd=" & nts_password & ";CharSet=utf8;Allow Zero Datetime=True;"
        GR.Connection_mariadb_bc_nts = New MySqlConnection(GR.cString_mariadb_bc_nts)

        GR.cString_mariadb_local = "server=localhost;port=" & GR.port_number_database_local & ";database=mysql;uid=guarantee7;pwd=" & local_database_password & ";CharSet=utf8;Allow Zero Datetime=True;"
        GR.Connection_mariadb_local = New MySqlConnection(GR.cString_mariadb_local)
        GR.cString_mariadb_local_test = "server=localhost;port=" & GR.port_number_database_local & ";database=test;uid=guarantee7;pwd=" & local_database_password & ";CharSet=utf8;Allow Zero Datetime=True;"
        GR.Connection_mariadb_local_test = New MySqlConnection(GR.cString_mariadb_local_test)
        GR.cString_mariadb_local_bc = "server=localhost;port=" & GR.port_number_database_local & ";database=bc;uid=guarantee7;pwd=" & local_database_password & ";CharSet=utf8;Allow Zero Datetime=True;"
        GR.Connection_mariadb_local_bc = New MySqlConnection(GR.cString_mariadb_local_bc)
        GR.cString_mariadb_local_bc_manager = "server=localhost;port=" & GR.port_number_database_local & ";database=bc_manager;uid=guarantee7;pwd=" & local_database_password & ";CharSet=utf8;Allow Zero Datetime=True;"
        GR.Connection_mariadb_local_bc_manager = New MySqlConnection(GR.cString_mariadb_local_bc_manager)
        GR.cString_mariadb_local_bc_multilevel = "server=localhost;port=" & GR.port_number_database_local & ";database=bc_multilevel;uid=guarantee7;pwd=" & local_database_password & ";CharSet=utf8;Allow Zero Datetime=True;"
        GR.Connection_mariadb_local_bc_multilevel = New MySqlConnection(GR.cString_mariadb_local_bc_multilevel)
        GR.cString_mariadb_local_bc_smart_contract = "server=localhost;port=" & GR.port_number_database_local & ";database=bc_smart_contract;uid=guarantee7;pwd=" & local_database_password & ";CharSet=utf8;Allow Zero Datetime=True;"
        GR.Connection_mariadb_local_bc_smart_contract = New MySqlConnection(GR.cString_mariadb_local_bc_smart_contract)
        GR.cString_mariadb_local_bc_agent = "server=localhost;port=" & GR.port_number_database_local & ";database=bc_agent;uid=guarantee7;pwd=" & local_database_password & ";CharSet=utf8;Allow Zero Datetime=True;"
        GR.Connection_mariadb_local_bc_agent = New MySqlConnection(GR.cString_mariadb_local_bc_agent)
        GR.cString_mariadb_local_bc_nft = "server=localhost;port=" & GR.port_number_database_local & ";database=bc_nft;uid=guarantee7;pwd=" & local_database_password & ";CharSet=utf8;Allow Zero Datetime=True;"
        GR.Connection_mariadb_local_bc_nft = New MySqlConnection(GR.cString_mariadb_local_bc_nft)

        GR.ethereum.rpc_port_number = 8545

        ' 이하는 개발자 기간 중 필요한 부분. 나중에는 제거한다.

        Dim main_password As String = "#cyndi$36%"
        GR.port_number_database_main = 3306

        GR.cString_mariadb_main = "server=" & GR.main_server_address & ";port=" & GR.port_number_database_main & ";database=mysql;uid=retelf;pwd=" & main_password & ";CharSet=utf8;Allow Zero Datetime=True;"
        GR.Connection_mariadb_main = New MySqlConnection(GR.cString_mariadb_main)
        GR.cString_mariadb_main_bc = "server=" & GR.main_server_address & ";port=" & GR.port_number_database_main & ";database=bc;uid=retelf;pwd=" & main_password & ";CharSet=utf8;Allow Zero Datetime=True;"
        GR.Connection_mariadb_main_bc = New MySqlConnection(GR.cString_mariadb_main_bc)
        GR.cString_mariadb_main_bc_manager = "server=" & GR.main_server_address & ";port=" & GR.port_number_database_main & ";database=bc_manager;uid=retelf;pwd=" & main_password & ";CharSet=utf8;Allow Zero Datetime=True;"
        GR.Connection_mariadb_main_bc_manager = New MySqlConnection(GR.cString_mariadb_main_bc_manager)
        GR.cString_mariadb_main_bc_multilevel = "server=" & GR.main_server_address & ";port=" & GR.port_number_database_main & ";database=bc_multilevel;uid=retelf;pwd=" & main_password & ";CharSet=utf8;Allow Zero Datetime=True;"
        GR.Connection_mariadb_main_bc_multilevel = New MySqlConnection(GR.cString_mariadb_main_bc_multilevel)
        GR.cString_mariadb_main_bc_smart_contract = "server=" & GR.main_server_address & ";port=" & GR.port_number_database_main & ";database=bc_smart_contract;uid=retelf;pwd=" & main_password & ";CharSet=utf8;Allow Zero Datetime=True;"
        GR.Connection_mariadb_main_bc_smart_contract = New MySqlConnection(GR.cString_mariadb_main_bc_smart_contract)

        GR.server_id = get_local_server_id.exe

        If Not mode = "install" And Not mode = "guarantee7" Then
            GR.node_level = get_node_level.exe(GR.server_id)
        End If

        'Dim database_install_state As String = check_initial_install_case.exe ' bc 데이터베이스 존재 여부 확인

        'If database_install_state = "bc_database_exists" Then



        '    '' 일단은 샘플서버가 아닌 filmasfilm.com 192.168.0.233 에서 main 을 담당한다. <- ' 아예 필요가 없다.

        '    'Dim main_password As String = "0x6af1F9b61fc380dF629a07a00Af3D756881f8338"
        '    'GR.port_number_database_main = 30601

        '    'GR.cString_mariadb_main = "server=" & GR.main_server_address & ";port=" & GR.port_number_database_main & ";database=mysql;uid=guarantee7;pwd=" & main_password & ";CharSet=utf8;Allow Zero Datetime=True;"
        '    'GR.Connection_mariadb_main = New MySqlConnection(GR.cString_mariadb_main)
        '    'GR.cString_mariadb_main_bc = "server=" & GR.main_server_address & ";port=" & GR.port_number_database_main & ";database=bc;uid=guarantee7;pwd=" & main_password & ";CharSet=utf8;Allow Zero Datetime=True;"
        '    'GR.Connection_mariadb_main_bc = New MySqlConnection(GR.cString_mariadb_main_bc)
        '    'GR.cString_mariadb_main_bc_manager = "server=" & GR.main_server_address & ";port=" & GR.port_number_database_main & ";database=bc_manager;uid=guarantee7;pwd=" & main_password & ";CharSet=utf8;Allow Zero Datetime=True;"
        '    'GR.Connection_mariadb_main_bc_manager = New MySqlConnection(GR.cString_mariadb_main_bc_manager)
        '    'GR.cString_mariadb_main_bc_multilevel = "server=" & GR.main_server_address & ";port=" & GR.port_number_database_main & ";database=bc_multilevel;uid=guarantee7;pwd=" & main_password & ";CharSet=utf8;Allow Zero Datetime=True;"
        '    'GR.Connection_mariadb_main_bc_multilevel = New MySqlConnection(GR.cString_mariadb_main_bc_multilevel)

        'ElseIf database_install_state = "installed_but_no_bc_database" Then

        'Else ' no_instance 

        'End If

    End Sub

End Class
