Imports System.IO
Imports Newtonsoft.Json
Imports Microsoft.Web.WebView2.Core
Imports Nethereum.Hex.HexTypes

Public Class submit_nft_data
    Public Shared Async Sub exe(json_message As String)

        Dim key, coin_name As String
        Dim signiture, signiture_key As String
        Dim JSS, JRS, receipt, pure_query As String
        Dim date_now_utc As DateTime
        Dim eoa, na, exchange_name, domain, ip, idate_string, closing_time_utc_string As String
        Dim port, port_nft As Integer
        Dim result As Task(Of String)

        Dim json_webview = CType(JsonConvert.DeserializeObject(json_message), Linq.JObject)

        key = json_webview("key").ToString

        coin_name = GRT.GR.account.login_state

        eoa = GRT.GR.account.public_key
        na = GRT.GR.account.agency.node
        exchange_name = GRT.GR.account.agency.exchange_name
        domain = GRT.GR.account.agency.domain_agency
        ip = GRT.GR.account.agency.ip_agency
        port = GRT.GR.account.agency.port_agency
        port_nft = GRT.GR.account.agency.port_nft

        Dim new_nfa = CBool(json_webview("value")("new_nfa"))
        Dim source = CStr(json_webview("value")("source"))
        Dim nfa As String = CStr(json_webview("value")("nfa"))

        Dim nft_type_pure_file = CBool(json_webview("value")("nft_type_pure_file"))
        Dim nft_type_pure_real = CBool(json_webview("value")("nft_type_pure_real"))
        Dim nft_type_file_based_real = CBool(json_webview("value")("nft_type_file_based_real"))

        Dim nft_type As String

        If nft_type_pure_file Then
            nft_type = "nft_type_pure_file"
        ElseIf nft_type_pure_real Then
            nft_type = "nft_type_pure_real"
        Else
            nft_type = "nft_type_file_based_real"
        End If

        Dim extension = CStr(json_webview("value")("extension"))
        Dim file_length = CLng(json_webview("value")("file_length"))
        Dim name = CStr(json_webview("value")("name"))
        Dim sub_name = CStr(json_webview("value")("sub_name"))
        Dim character = CStr(json_webview("value")("character"))
        Dim creator = CStr(json_webview("value")("creator"))
        Dim sub_creator = CStr(json_webview("value")("sub_creator"))
        Dim personal_name = CStr(json_webview("value")("personal_name"))
        Dim token_id = CInt(json_webview("value")("token_id"))
        Dim price_piece = CDec(json_webview("value")("price_piece"))
        Dim price_total = CDec(json_webview("value")("price_total"))
        Dim splitable = CBool(json_webview("value")("splitable"))
        Dim max_split = CInt(json_webview("value")("max_split"))
        Dim file_copiable = CBool(json_webview("value")("file_copiable"))
        Dim materialable = CBool(json_webview("value")("materialable"))
        Dim max_material_count = CInt(json_webview("value")("max_material_count"))
        Dim copyright_transfer = CBool(json_webview("value")("copyright_transfer"))
        Dim pollable = CBool(json_webview("value")("pollable"))
        Dim min_price = CDec(json_webview("value")("min_price"))
        Dim terms_modifiable = CBool(json_webview("value")("terms_modifiable"))
        Dim quorum_proposal = CSng(json_webview("value")("quorum_proposal"))
        Dim quorum_conference = CSng(json_webview("value")("quorum_conference"))
        Dim quorum_resolution = CSng(json_webview("value")("quorum_resolution"))
        Dim poll_notice_days_span = CInt(json_webview("value")("poll_notice_days_span"))
        Dim poll_days_span = CInt(json_webview("value")("poll_days_span"))
        Dim general_terms = CStr(json_webview("value")("general_terms"))
        Dim individual_terms = CStr(json_webview("value")("individual_terms"))
        Dim nft_recall_days_span = CInt(json_webview("value")("nft_recall_days_span"))
        Dim sell_order_right_now = CBool(json_webview("value")("sell_order_right_now"))

        Dim creator_profile = CStr(json_webview("value")("creator_profile"))
        Dim sub_creator_profile = CStr(json_webview("value")("sub_creator_profile"))
        Dim main_description = CStr(json_webview("value")("main_description"))
        Dim sub_description = CStr(json_webview("value")("sub_description"))
        Dim name_critic = CStr(json_webview("value")("name_critic"))
        Dim critic = CStr(json_webview("value")("critic"))

        Dim url = "nft.ipfs.guarantee7.com" & "/" & nfa & "_" & token_id & "_" & name & "_" & sub_name & "." & extension

        date_now_utc = GRT.GetNistTime.exe

        'closing_time_utc_string = date_now_utc.AddDays(days_span).ToString("yyyy/MM/dd HH:mm:ss")
        closing_time_utc_string = "0000-00-00 00:00:00"

        idate_string = date_now_utc.ToString("yyyy/MM/dd HH:mm:ss")

        pure_query = GRT.GQS_nft_initial_deploy.exe(
                                                new_nfa,
                                                nfa,
                                                eoa,
                                                name,
                                                sub_name,
                                                sub_creator,
                                                personal_name,
                                                character,
                                                nft_type,
                                                price_piece,
                                                price_total,
                                                extension,
                                                file_length,
                                                sell_order_right_now,
                                                max_split,
                                                general_terms,
                                                individual_terms,
                                                token_id,
                                                url,
                                                na,
                                                exchange_name,
                                                domain,
                                                ip,
                                                port,
                                                nft_recall_days_span,
                                                closing_time_utc_string,
                                                splitable,
                                                max_split,
                                                file_copiable,
                                                materialable,
                                                max_material_count,
                                                min_price,
                                                terms_modifiable,
                                                copyright_transfer,
                                                pollable,
                                                quorum_proposal,
                                                quorum_conference,
                                                quorum_resolution,
                                                poll_notice_days_span,
                                                poll_days_span,
                                                idate_string,
                                                creator_profile,
                                                sub_creator_profile,
                                                main_description,
                                                sub_description,
                                                name_critic,
                                                critic)

        signiture = GRT.Security.Gsign.sign(pure_query, GRT.GR.account.private_key)
        signiture_key = Regex.Match(signiture, "^0x.{64}").ToString

        JSS = GRT.make_json_string.exe(
                {{"key", key, "quot"}},
                {
                {"block_hash", "initial", "quot"},
                {"source", Regex.Replace(source, "\\", "\\"), "quot"},
                {"new_nfa", CStr(CInt(new_nfa)), "non_quot"},
                {"nfa", nfa, "quot"},
                {"token_id", CStr(token_id), "non_quot"},
                {"name", name, "quot"},
                {"sub_name", sub_name, "quot"},
                {"character", character, "quot"},
                {"nft_type", nft_type, "quot"},
                {"url", url, "quot"},
                {"creator", creator, "quot"},
                {"sub_creator", sub_creator, "quot"},
                {"personal_name", personal_name, "quot"},
                {"extension", extension, "quot"},
                {"file_length", CStr(file_length), "non_quot"},
                {"sell_order_right_now", CStr(CInt(sell_order_right_now)), "non_quot"},
                {"price_piece", CStr(price_piece), "non_quot"},
                {"price_total", CStr(price_total), "non_quot"},
                {"splitable", CStr(CInt(splitable)), "non_quot"},
                {"max_split", CStr(max_split), "non_quot"},
                {"file_copiable", CStr(CInt(file_copiable)), "non_quot"},
                {"materialable", CStr(CInt(materialable)), "non_quot"},
                {"max_material_count", CStr(max_material_count), "non_quot"},
                {"terms_modifiable", CStr(CInt(terms_modifiable)), "non_quot"},
                {"copyright_transfer", CStr(CInt(copyright_transfer)), "non_quot"},
                {"pollable", CStr(CInt(pollable)), "non_quot"},
                {"min_price", CStr(min_price), "non_quot"},
                {"quorum_proposal", CStr(quorum_proposal), "non_quot"},
                {"quorum_conference", CStr(quorum_conference), "non_quot"},
                {"quorum_resolution", CStr(quorum_resolution), "non_quot"},
                {"poll_notice_days_span", CStr(poll_notice_days_span), "non_quot"},
                {"poll_days_span", CStr(poll_days_span), "non_quot"},
                {"general_terms", general_terms, "quot"},
                {"individual_terms", individual_terms, "quot"},
                {"nft_recall_days_span", CStr(nft_recall_days_span), "non_quot"},
                {"closing_time_utc_string", closing_time_utc_string, "quot"},
                {"eoa", eoa, "quot"},
                {"na", na, "quot"},
                {"exchange_name", exchange_name, "quot"},
                {"agency_domain", domain, "quot"},
                {"agency_ip", ip, "quot"},
                {"agency_port", CStr(port), "non_quot"},
                {"agency_port_nft", CStr(port_nft), "non_quot"},
                {"pure_query", pure_query, "quot"},
                {"signiture", signiture, "quot"},
                {"signiture_key", signiture_key, "quot"},
                {"initial_transfer", "Y", "quot"},
                {"idate_string", idate_string, "quot"},
                {"creator_profile", creator_profile, "quot"},
                {"sub_creator_profile", sub_creator_profile, "quot"},
                {"main_description", main_description, "quot"},
                {"sub_description", sub_description, "quot"},
                {"name_critic", name_critic, "quot"},
                {"critic", critic, "quot"}}, False)

        'JRS = Await GRT.socket_client.exe(GRT.GR.main_server_address_agency, GRT.GR.port_number_server_main_agency, GRT.GR.port_number_server_local, JSS)
        JRS = Await GRT.socket_client.exe(GRT.GR.account.agency.ip_agency, GRT.GR.account.agency.port_agency, GRT.GR.port_number_server_local, JSS) ' 에이전시에서는 파일이 제대로 올라와 있느냐만 확인한다.

        Dim json_JRS As Newtonsoft.Json.Linq.JObject = CType(JsonConvert.DeserializeObject(JRS), Linq.JObject)

        receipt = GRT.issue_receipt.exe(json_JRS.Root.ToString)

        receipt = Regex.Replace(receipt, "'", "\'")
        receipt = Regex.Replace(receipt, vbCrLf, "<br />")

        result = GR.control.wv_main.CoreWebView2.ExecuteScriptAsync(
                    $"$('#div_explanation').html('{receipt}');")

    End Sub

End Class