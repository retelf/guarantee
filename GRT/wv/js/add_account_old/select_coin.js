var class_select_coin = function () {

    this.exe = function (coin_name) {

        if (coin_name=='guarantee') {
            $('#rb_ethereum_add_account').prop('checked', false);
            $('#div_public_key_outer').css('display', 'none');
            $('#div_password_outer').css('display', 'block');
            $('#div_private_key_outer').css('display', 'none');
            $('#div_whether_key_file_generate_outer').css('display', 'block');
        } else {
            $('#rb_guarantee_add_account').prop('checked', false);
            $('#div_public_key_outer').css('display', 'block');
            $('#div_password_outer').css('display', 'none');
            $('#div_private_key_outer').css('display', 'block');
            $('#div_whether_key_file_generate_outer').css('display', 'none');
        }
    }
}

var c_select_coin = new class_select_coin();
