var class_load_nft = function () {

    this.exe = function () {

        let new_nfa;

        if ($('#text_nfa').val() == 'NEW') {
            new_nfa = true;
        }
        else {
            new_nfa = false;
        }

        let jsonObject = {
            key: "submit_load_nft",
            value:
            {
                new_nfa: new_nfa,
                source: $('#div_source').html(),
                nfa: $('#text_nfa').val(),
                nft_type_pure_file: $('#rb_nft_type_pure_file').prop('checked'),
                nft_type_pure_real: $('#rb_nft_type_pure_real').prop('checked'),
                nft_type_file_based_real: $('#rb_nft_type_file_based_real').prop('checked'),
                name: $('#text_name').val(),
                main_description: $('#textarea_main_description').val().replace(/'/g, "_quot_single_").replace(/"/g, "_quot_double_"),
                sub_name: $('#text_sub_name').val(),
                sub_description: $('#textarea_sub_description').val().replace(/'/g, "_quot_single_").replace(/"/g, "_quot_double_"),
                name_critic: $('#text_name_critic').val(),
                critic: $('#textarea_critic').val().replace(/'/g, "_quot_single_").replace(/"/g, "_quot_double_"),
                character: $('#text_character').val().replace(/'/g, "_quot_single_").replace(/"/g, "_quot_double_"),
                creator: $('#text_creator').val(),
                creator_profile: $('#textarea_creator_profile').val().replace(/'/g, "_quot_single_").replace(/"/g, "_quot_double_"),
                sub_creator: $('#text_sub_creator').val(),
                personal_name: $('#text_personal_name').val(),
                sub_creator_profile: $('#textarea_sub_creator_profile').val().replace(/'/g, "_quot_single_").replace(/"/g, "_quot_double_"),
                price_piece: $('#text_price_piece').val(),
                price_total: $('#text_price_total').val(),
                splitable: $('#chk_splitable').prop('checked'),
                max_split: $('#text_max_split').val(),
                file_copiable: $('#chk_file_copiable').prop('checked'),
                materialable: $('#chk_materialable').prop('checked'),
                max_material_count: $('#text_max_material_count').val(),
                terms_modifiable: $('#chk_terms_modifiable').prop('checked'),
                copyright_transfer: $('#chk_copyright_transfer').prop('checked'),
                pollable: $('#chk_pollable').prop('checked'),
                min_price: $('#text_min_price').val(),
                quorum_proposal: $('#text_quorum_proposal').val() / 100,
                quorum_conference: $('#text_quorum_conference').val() / 100,
                quorum_resolution: $('#text_quorum_resolution').val() / 100,
                poll_notice_days_span: $('#text_poll_notice_days_span').val(),
                poll_days_span: $('#text_poll_days_span').val(),
                general_terms: $('#textarea_general_terms').val().replace(/'/g, "_quot_single_").replace(/"/g, "_quot_double_"),
                individual_terms: $('#textarea_individual_terms').val().replace(/'/g, "_quot_single_").replace(/"/g, "_quot_double_"),
                nft_recall_days_span: $('#text_nft_recall_days_span').val(),
                sell_order_right_now: $('#rb_sell_order_right_now').prop('checked')
            }
        }
        window.chrome.webview.postMessage(jsonObject);
    }
}

var c_load_nft = new class_load_nft();
