var class_whether_key_file_generate = function () {

    this.exe = function (bool) {

        if (bool) {
            $('#rb_key_file_generate_no').prop('checked', false);
        } else {
            $('#rb_key_file_generate_yes').prop('checked', false);
        }
    }
}

var c_whether_key_file_generate = new class_whether_key_file_generate();
