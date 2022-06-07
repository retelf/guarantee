var class_whether_key_file_generate = function () {

    this.exe = function (bool) {

        if (bool) {
            $('#rb_key_file_generate_no').prop('checked', false);
            $('#div_password_outer').css('display', 'block');
        } else {
            $('#rb_key_file_generate_yes').prop('checked', false);
            $('#div_password_outer').css('display', 'none');
        }
    }
}

var c_whether_key_file_generate = new class_whether_key_file_generate();
