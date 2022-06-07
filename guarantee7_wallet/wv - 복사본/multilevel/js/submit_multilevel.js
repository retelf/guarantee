var class_submit_multilevel = function () {

    var Me; this.initialize = function () { Me = c_submit_multilevel; }

    this.button_selected;

    this.exe = function ($button) {

        let $tr_parent = $button.closest("tr");

        let btn_type = $button.prop('id').match(/buy|refund|recall|confirm/);

        this.button_selected = $button;

        if (btn_type == 'buy' && $('#div_info_outer').css('display') == 'none') {

            let explanation = '<br/>' +
                '사업자는 별도의 공개키와 비밀키를 부여받게 됩니다. 이 때 키파일을 발급받아 편리하게 비밀키를 보관하고자 한다면 비밀키 생성을 위한 패스워드가 필요합니다.' + '<br/>' +
                '만약 키파일 없이 그때그때마다 비밀키를 입력하는 방식으로 사용하고자 한다면 키파일 생성 안함을 클릭하시기 바랍니다.' + '<br/>' +
                '그리고 최종적으로 아래의 버튼을 클릭하시기 바랍니다.' + '<br/><br/>';

            $('#div_explanation').html(explanation);
            $('#div_info_outer').css('display', 'block');

            return;
        }

        let rb_checked_yes = $('#rb_key_file_generate_yes').prop('checked');
        let rb_checked_no = $('#rb_key_file_generate_no').prop('checked');

        if (btn_type=='buy' && !rb_checked_yes && !rb_checked_no) {
            alert("키파일 생성 여부를 체크해 주십시오.");
            return;
        }

        let jsonObject = {
            key: 'submit_' + btn_type,
            value:
            {
                password: $('#text_password').val(),
                whether_key_file_generate: rb_checked_yes,
                block_number: $tr_parent.find('.div_board_block_number:first').text(),
                eoa: $tr_parent.find('.div_board_eoa:first').text(),
                na: $tr_parent.find('.div_board_na:first').text(),
                domain: $tr_parent.find('.div_board_domain:first').text(),
                ip: $tr_parent.find('.div_board_ip:first').text(),
                port: $tr_parent.find('.div_board_port:first').text(),
                exchange_rate: $tr_parent.find('.div_board_exchange_rate:first').text(),
                days_span: $tr_parent.find('.div_board_days_span:first').text(),
                closing_time_local: $tr_parent.find('.div_board_closing_time_local:first').text(),
                closing_time_utc: $tr_parent.find('.div_board_closing_time_utc:first').text(),
                exchange_fee_rate: $tr_parent.find('.div_board_exchange_fee_rate:first').text()
            }
        }
        window.chrome.webview.postMessage(jsonObject);

    }

}; var c_submit_multilevel = new class_submit_multilevel();