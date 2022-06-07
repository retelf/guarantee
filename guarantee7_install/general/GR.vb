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
        Dim txt_private_key As TextBox

        Dim btn_initial_login As Button
        Dim btn_coin_login As Button

        Dim lbl_login_state As Label
        Dim lbl_coin_name As Label
        Dim lbl_public_key As Label
        Dim lbl_private_key As Label
        Dim lbl_message As Label

        Dim lbl_login As Label
        Dim lbl_membership As Label

        Dim rb_account_exist As RadioButton
        Dim rb_account_not_exist As RadioButton
        Dim rb_open_infomation As RadioButton
        Dim rb_not_open_infomation As RadioButton

        Dim cb_coin_name As ComboBox

    End Structure : Public Shared control As st_control

    Public Shared login_state_guarantee As Boolean = False
    Public Shared login_state_ethereum As Boolean = False

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

    Public Shared fee As Decimal = 3000 ' 달러연동

    Public Structure st_authentication

        Dim id As String
        Dim password As String
        Dim name_english As String
        Dim name_home_language As String
        Dim country As String
        Dim phone_number As String
        Dim identity_number As String

    End Structure : Public Shared authentication As st_authentication

    Public Shared current_id As String
    Public Shared current_initial_password As String
    Public Shared current_guarantee_password As String
    Public Shared current_ethereum_password As String

End Class
