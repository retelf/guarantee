Imports System.IO
Imports Newtonsoft.Json
Imports Microsoft.Web.WebView2.Core
Imports Nethereum.Hex.HexTypes

Public Class initialize_nft_form
    Public Shared Async Sub exe()

        Dim coin_name As String

        coin_name = GRT.GR.account.login_state

        Await GR.control.wv_main.CoreWebView2.ExecuteScriptAsync(
                                                $"$('#text_creator').val('{GRT.GR.account.public_key}');
                                                $('#text_sub_creator').val('{GRT.GR.account.public_key}');
                                                $('#text_personal_name').val('{GRT.GR.account.super_account.name_home_language}');
                                                $('#text_creator').prop('disabled', 'disabled');
                                                $('#text_sub_creator').prop('disabled', 'disabled');
                                                $('#text_personal_name').prop('disabled', 'disabled');")

        'Await GR.control.wv_main.CoreWebView2.ExecuteScriptAsync(
        '                                        $"$('#text_creator').val('{GRT.GR.account.public_key}');
        '                                        $('#text_sub_creator').val('{GRT.GR.account.public_key}');
        '                                        $('#text_personal_name').val('{GRT.GR.account.super_account.name_home_language}');")

    End Sub

End Class
