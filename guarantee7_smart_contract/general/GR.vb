Imports MySqlConnector

Public Class GR

    Public Shared txt_server_port_number As TextBox
    Public Shared txt_database_port_number As TextBox

    Structure st_control

        Dim wv_working, wv_test_launch As Microsoft.Web.WebView2.WinForms.WebView2

        Dim lbl_login As Label
        Dim lbl_login_state As Label
        Dim lbl_membership As Label

    End Structure : Public Shared control As st_control

End Class
