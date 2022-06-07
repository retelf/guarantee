Public Class GR

    Public Shared txt_monitor As TextBox

    'Public Shared cString_mariadb As String = "server=filmasfilm.com;port=3306;database=bc_multilevel_contract;uid=retelf;pwd=#cyndi$36%;CharSet=utf8;"
    'Public Shared cString_mariadb_manager As String = "server=filmasfilm.com;port=3306;database=bc_manager;uid=retelf;pwd=#cyndi$36%;CharSet=utf8;"

    Public Shared cString_mariadb As String = "server=localhost;port=3306;database=bc_multilevel_contract;uid=retelf;pwd=#cyndi$36%;CharSet=utf8;"
    Public Shared cString_mariadb_manager As String = "server=localhost;port=3306;database=bc_manager;uid=retelf;pwd=#cyndi$36%;CharSet=utf8;"

    Public Shared Connection_mariadb As New MySqlConnection(cString_mariadb)
    Public Shared Connection_mariadb_manager As New MySqlConnection(cString_mariadb_manager)

    Public Shared syncro_state As Boolean

    Structure st_control

        Dim wv As Microsoft.Web.WebView2.WinForms.WebView2

        Dim lbl_login_state As Label
        Dim lbl_login As Label
        Dim lbl_membership As Label

    End Structure : Public Shared control As st_control

End Class
