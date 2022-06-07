Imports MySqlConnector

Public Class GR

    Public Shared txt_server_port_number As TextBox
    Public Shared txt_database_port_number As TextBox

    Structure st_control

        Dim wv As Microsoft.Web.WebView2.WinForms.WebView2

        Dim txt_public_key As TextBox
        Dim lbl_message As Label
        Dim lbl_login As Label
        Dim lbl_login_state As Label
        Dim lbl_add_account As Label
        Dim lbl_membership As Label

    End Structure : Public Shared control As st_control

    'Public Shared login_state_guarantee As Boolean = False
    'Public Shared login_state_ethereum As Boolean = False

    'Public Shared ethereum_connection_project_id As String = "a7a01158fe384d85b605cd5ba0b53bc3"
    'Public Shared ethereum_foundation_public_key As String = "0xde0b295669a9fd93d5f28d9ec85e40f4cb697bae"

    'Public Shared btn_execute As Button
    'Public Shared txt_ethereum_address As TextBox
    'Public Shared txt_password As TextBox
    'Public Shared txt_public_key As TextBox
    'Public Shared txt_private_key As TextBox
    'Public Shared txt_transfer As TextBox
    'Public Shared txt_source As TextBox
    'Public Shared rb_ethereum_exist As RadioButton
    'Public Shared rb_ethereum_not_exist As RadioButton
    'Public Shared lbl_after_check_address As Label
    'Public Shared lbl_password As Label
    'Public Shared lbl_public_key As Label
    'Public Shared lbl_private_key As Label
    'Public Shared lbl_transfer As Label
    'Public Shared cb_coin_types As ComboBox

    'Public Shared price_goodwill_krw As Decimal = 500000

    'Public Shared fee As Decimal = 3000 ' 달러연동

    'Public Shared replication_unit As Integer = 30

    'Public Shared current_id As String
    'Public Shared current_initial_password As String
    'Public Shared current_guarantee_password As String
    'Public Shared current_ethereum_password As String

End Class
