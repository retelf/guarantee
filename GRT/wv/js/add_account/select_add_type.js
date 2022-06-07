var class_select_add_type = function () {

    this.exe = function (add_type) {

        if (add_type=='new') {
            $('#rb_add_account_already').prop('checked', false);
            $('#div_public_key_outer').css('display', 'none');
            $('#div_private_key_outer').css('display', 'none');
        } else {
            $('#rb_add_account_new').prop('checked', false);
            $('#div_public_key_outer').css('display', 'block');
            $('#div_private_key_outer').css('display', 'block');
        }
    }
}

var c_select_add_type = new class_select_add_type();
