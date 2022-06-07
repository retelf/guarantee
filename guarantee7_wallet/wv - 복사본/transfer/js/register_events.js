var class_register_events = function () {

    this.exe = function () {

        $('#select_self_coin_name').change(function () {

            let self_coin_name = $("#select_self_coin_name option:selected").text();
            let transfer_amount = $('#text_amount').val();

                $('#div_details').text(self_coin_name + ' 코인 ' + transfer_amount + ' 개를 ' + $('#text_to').val() + ' 님에게 송금합니다.');

        });

        $("#btn_transfer").bind("click", function () {
            c_transfer.exe();
        });
    }
}

var c_register_events = new class_register_events();
