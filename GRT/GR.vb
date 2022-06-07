Imports MySqlConnector

Public Class GR

    Public Shared remote_server_address As String

    Public Shared port_number_database_remote As Integer
    Public Shared port_number_server_remote As Integer

    Public Shared port_number_database_local As Integer
    Public Shared port_number_server_local As Integer

    Public Shared cString_mariadb_remote As String
    Public Shared Connection_mariadb_remote As MySqlConnection
    Public Shared cString_mariadb_remote_bc As String
    Public Shared Connection_mariadb_remote_bc As MySqlConnection
    Public Shared cString_mariadb_remote_bc_manager As String
    Public Shared Connection_mariadb_remote_bc_manager As MySqlConnection
    Public Shared cString_mariadb_remote_bc_multilevel_contract As String
    Public Shared Connection_mariadb_remote_bc_multilevel_contract As MySqlConnection

    Public Shared cString_mariadb_local As String
    Public Shared Connection_mariadb_local As MySqlConnection
    Public Shared cString_mariadb_local_bc As String
    Public Shared Connection_mariadb_local_bc As MySqlConnection
    Public Shared cString_mariadb_local_bc_manager As String
    Public Shared Connection_mariadb_local_bc_manager As MySqlConnection
    Public Shared cString_mariadb_local_bc_multilevel_contract As String
    Public Shared Connection_mariadb_local_bc_multilevel_contract As MySqlConnection

End Class
