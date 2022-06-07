Imports MySqlConnector
Imports Nethereum.Hex.HexTypes
Public Class GR

    Public Structure st_account

        Public login_state As String ' 코인네임이다.
        Public public_key As String
        Public private_key As String
        Public password As String

        Public Structure st_super_account

            Public email As String
            Public name_english As String
            Public name_home_language As String
            Public country As String
            Public phone_number As String
            Public identity_number As String

        End Structure : Dim super_account As st_super_account

        Public Structure st_agency

            Public node As String
            Public exchange_name As String

            Public domain As String
            Public domain_agency As String
            Public domain_web As String
            Public domain_ethereum As String
            Public domain_management As String
            Public domain_nft As String
            Public ip As String
            Public ip_agency As String
            Public ip_web As String
            Public ip_ethereum As String
            Public ip_management As String
            Public ip_nft As String
            Public port As Integer
            Public port_agency As Integer
            Public port_web As Integer
            Public port_ethereum As Integer
            Public port_management As Integer
            Public port_nft As Integer

        End Structure : Dim agency As st_agency

        Public balance As Decimal

        'Public login_state_guarantee As Boolean
        'Public login_state_ethereum As Boolean
        'Public public_key_ethereum As String
        'Public private_key_ethereum As String

        Public login_state_main_key As Boolean
        Public keyfile_folder As String
        'Public keyfile_folder_guarantee As String
        'Public keyfile_folder_ethereum As String
        Public ethereum_chain_id As Integer

        Public ethereum_transaction_type As String
        Public ethereum_connection_project_id As String

        Public ethereum_web3_network As String
        Public ethereum_web3_url As String

    End Structure : Public Shared account As st_account

    Public Structure management

        Public Structure confirm_time

            Public Shared general As Integer = 10000
            Public Shared multilevel As Integer = 10000
            Public Shared nft As Integer = 10000

        End Structure

    End Structure

    Public Structure st_foundation

        Dim public_key As String ' eoa

    End Structure : Public Shared foundation As st_foundation

    Public Structure st_multilevel

        Dim max_children_count As Integer
        Dim ma_foundation As String
        Dim first_parent_acquisition_ratio As Decimal

    End Structure : Public Shared multilevel As st_multilevel

    Public Structure st_nft

        Dim destination_folder As String

    End Structure : Public Shared nft As st_nft

    'PROJECT ID
    'a7a01158fe384d85b605cd5ba0b53bc3
    'PROJECT SECRET
    '0ace513a6617489f82d6fe7939c52d11
    'ENDPOINTS
    'Mainnet
    'https://mainnet.infura.io/v3/a7a01158fe384d85b605cd5ba0b53bc3
    'wss://mainnet.infura.io/ws/v3/a7a01158fe384d85b605cd5ba0b53bc3

    'Public Shared ethereum_connection_project_id As String = "a7a01158fe384d85b605cd5ba0b53bc3"
    'Public Shared ethereum_foundation_public_key As String = "0xde0b295669a9fd93d5f28d9ec85e40f4cb697bae"

    'Dim web3 = new Web3("https://mainnet.infura.io/v3/a7a01158fe384d85b605cd5ba0b53bc3");
    'Dim balance = await web3.Eth.GetBalance.SendRequestAsync("0xde0b295669a9fd93d5f28d9ec85e40f4cb697bae");

    Public Shared nts_server_address As String
    Public Shared port_number_server_nts As Integer

    Public Shared server_id As String ' node
    Public Shared server_private_key As String
    Public Shared node_level As Integer

    Public Shared server_thread_count As Integer = 6

    Public Shared main_server_address As String
    Public Shared main_server_address_agency As String
    Public Shared main_server_address_web As String
    Public Shared main_server_address_ethereum As String
    Public Shared main_server_address_nft As String
    Public Shared main_server_address_management As String

    Public Shared port_number_server_main As Integer
    Public Shared port_number_server_main_agency As Integer
    Public Shared port_number_server_main_nft As Integer
    Public Shared port_number_server_main_web As Integer
    Public Shared port_number_server_main_ethereum As Integer
    Public Shared port_number_server_main_management As Integer

    Public Shared port_number_database_main As Integer

    Public Shared ftp_server_address As String
    Public Shared port_number_server_ftp As Integer

    Public Shared nft_server_address As String
    Public Shared port_number_server_nft As Integer

    Public Shared qs_server_address As String
    Public Shared port_number_server_qs As Integer

    Public Shared balance_server_address As String
    Public Shared port_number_server_balance As Integer

    Public Shared exchange_server_address As String
    Public Shared port_number_server_exchange As Integer

    Public Shared authentification_server_address As String
    Public Shared port_number_server_authentification As Integer

    Public Shared send_message_server_address As String
    Public Shared port_number_server_send_message As Integer

    Public Shared temporary_self_client_socket_sender_number As Integer = 40702 ' 이것은 큰 의미가 없다. sender 의 connect 포트번호는 랜담 발급이기 때문이다.

    Public Shared address_server_local As String
    Public Shared address_server_local_agency As String
    Public Shared address_server_local_nft As String
    Public Shared address_server_local_web As String
    Public Shared address_server_local_ethereum As String
    Public Shared address_server_local_management As String

    Public Shared port_number_server_local As Integer
    Public Shared port_number_server_local_agency As Integer
    Public Shared port_number_server_local_nft As Integer
    Public Shared port_number_server_local_web As Integer
    Public Shared port_number_server_local_ethereum As Integer
    Public Shared port_number_server_local_management As Integer

    Public Shared port_number_database_local As Integer

    Public Shared cString_mariadb_bc_nts As String
    Public Shared Connection_mariadb_bc_nts As MySqlConnection

    Public Shared cString_mariadb_main As String
    Public Shared Connection_mariadb_main As MySqlConnection
    Public Shared cString_mariadb_main_bc As String
    Public Shared Connection_mariadb_main_bc As MySqlConnection
    Public Shared cString_mariadb_main_bc_manager As String
    Public Shared Connection_mariadb_main_bc_manager As MySqlConnection
    Public Shared cString_mariadb_main_bc_multilevel As String
    Public Shared Connection_mariadb_main_bc_multilevel As MySqlConnection
    Public Shared cString_mariadb_main_bc_smart_contract As String
    Public Shared Connection_mariadb_main_bc_smart_contract As MySqlConnection

    Public Shared cString_mariadb_local As String
    Public Shared Connection_mariadb_local As MySqlConnection
    Public Shared cString_mariadb_local_test As String
    Public Shared Connection_mariadb_local_test As MySqlConnection
    Public Shared cString_mariadb_local_bc As String
    Public Shared Connection_mariadb_local_bc As MySqlConnection
    Public Shared cString_mariadb_local_bc_manager As String
    Public Shared Connection_mariadb_local_bc_manager As MySqlConnection
    Public Shared cString_mariadb_local_bc_multilevel As String
    Public Shared Connection_mariadb_local_bc_multilevel As MySqlConnection
    Public Shared cString_mariadb_local_bc_smart_contract As String
    Public Shared Connection_mariadb_local_bc_smart_contract As MySqlConnection
    Public Shared cString_mariadb_local_bc_agent As String
    Public Shared Connection_mariadb_local_bc_agent As MySqlConnection
    Public Shared cString_mariadb_local_bc_nft As String
    Public Shared Connection_mariadb_local_bc_nft As MySqlConnection

    Public Shared genesis_block_hash As String = "0x0000000000000000000000000000000000000000000000000000000000000001"
    Public Shared algorithm As String = "Keccak-256-ethereum"
    Public Shared node_max_children_count As Integer = 10

    Public Shared ethereum_transaction_rolling_times As Integer = 36 ' 3분
    Public Shared ethereum_transaction_rolling_times_for_cancel As Integer = 36 ' 3분
    Public Shared replication_unit As Integer = 30
    Public Shared exchange_fee_rate As Decimal = CDec(0.002)
    Public Shared multilevel_exchange_fee_rate As Decimal = CDec(0.002)
    Public Shared nft_transaction_fee_rate As Decimal = CDec(0.002)

    Public Shared ftp_account_file_source As String = "C:\xampp\FileZillaFTP\FileZilla Server.xml"

    Public Structure st_ethereum

        Dim rpc_port_number As Integer
        Dim geth_running As Boolean
        Dim web3 As Web3

    End Structure : Public Shared ethereum As st_ethereum

    Public Shared coin_name() As String = {
        "guarantee", "ethereum", "bitcoin", "doge", "klayton", "eos", "ripple", "KRW", "USD", "CNY", "EUR", "JPY"}

End Class
