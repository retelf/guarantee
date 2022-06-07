Imports MySqlConnector

Public Class GR

    Public Shared txt_server_port_number As TextBox
    Public Shared txt_database_port_number As TextBox

    Structure st_control

        Dim wv As Microsoft.Web.WebView2.WinForms.WebView2

        Dim pnl_initial_login As Panel
        Dim pnl_coin_login As Panel
        Dim pnl_purchase_coin As Panel
        Dim pnl_purchase_contract As Panel
        Dim pnl_refund As Panel
        Dim pnl_sell_membership As Panel
        Dim pnl_coin_login_inner As Panel
        Dim pnl_open_information As Panel

        Dim txt_id As TextBox
        Dim txt_initial_password As TextBox
        Dim txt_coin_password As TextBox
        Dim txt_public_key As TextBox
        Dim txt_initial_public_key As TextBox
        Dim txt_private_key As TextBox

        Dim btn_initial_login As Button
        Dim btn_coin_login As Button

        Dim lbl_login_state As Label
        Dim lbl_coin_name As Label
        Dim lbl_public_key As Label
        Dim lbl_initial_public_key As Label
        Dim lbl_private_key As Label
        Dim lbl_message As Label

        Dim rb_account_exist As RadioButton
        Dim rb_account_not_exist As RadioButton
        Dim rb_open_infomation As RadioButton
        Dim rb_not_open_infomation As RadioButton

        Dim cb_coin_name As ComboBox

    End Structure : Public Shared control As st_control

    Public Shared login_state_guarantee As Boolean = False
    Public Shared login_state_ethereum As Boolean = False

    'PROJECT ID
    'a7a01158fe384d85b605cd5ba0b53bc3
    'PROJECT SECRET
    '0ace513a6617489f82d6fe7939c52d11
    'ENDPOINTS
    'Mainnet
    'https://mainnet.infura.io/v3/a7a01158fe384d85b605cd5ba0b53bc3
    'wss://mainnet.infura.io/ws/v3/a7a01158fe384d85b605cd5ba0b53bc3

    Public Shared ethereum_mainnet_connection_project_id As String = "a7a01158fe384d85b605cd5ba0b53bc3"
    Public Shared ethereum_foundation_public_key As String = "0xde0b295669a9fd93d5f28d9ec85e40f4cb697bae"

    Public Shared btn_execute As Button
    Public Shared txt_ethereum_address As TextBox
    Public Shared txt_password As TextBox
    Public Shared txt_public_key As TextBox
    Public Shared txt_private_key As TextBox
    Public Shared txt_transfer As TextBox
    Public Shared txt_source As TextBox
    Public Shared rb_ethereum_exist As RadioButton
    Public Shared rb_ethereum_not_exist As RadioButton
    Public Shared lbl_after_check_address As Label
    Public Shared lbl_password As Label
    Public Shared lbl_public_key As Label
    Public Shared lbl_private_key As Label
    Public Shared lbl_transfer As Label
    Public Shared cb_coin_types As ComboBox

    Public Shared price_goodwill_krw As Decimal = 500000

    'Public Shared remote_server_address As String = "filmasfilm.com"
    Public Shared remote_server_address As String = "guarantee7.com"

    Public Shared cString_mariadb_local As String = "server=localhost;port=3306;database=mysql;uid=root;pwd=#cyndi$36%;CharSet=utf8;"
    Public Shared Connection_mariadb_local As New MySqlConnection(cString_mariadb_local)
    Public Shared cString_mariadb_local_bc As String = "server=localhost;port=3306;database=bc;uid=root;pwd=#cyndi$36%;CharSet=utf8;"
    Public Shared Connection_mariadb_local_bc As New MySqlConnection(cString_mariadb_local_bc)
    Public Shared cString_mariadb_local_bc_manager As String = "server=localhost;port=3306;database=bc_manager;uid=root;pwd=#cyndi$36%;CharSet=utf8;"
    Public Shared Connection_mariadb_local_bc_manager As New MySqlConnection(cString_mariadb_local_bc_manager)
    Public Shared cString_mariadb_local_bc_multilevel_contract As String = "server=localhost;port=3306;database=bc_multilevel_contract;uid=root;pwd=#cyndi$36%;CharSet=utf8;"
    Public Shared Connection_mariadb_local_bc_multilevel_contract As New MySqlConnection(cString_mariadb_local_bc_multilevel_contract)

    Public Shared fee As Decimal = 3000 ' 달러연동

    Public Shared replication_unit As Integer = 30

    Public Shared current_id As String
    Public Shared current_initial_password As String
    Public Shared current_guarantee_password As String
    Public Shared current_ethereum_password As String

End Class
