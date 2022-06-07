var class_register_events = function () {

    this.exe = function () {

        $("#btn_submit").bind("click", function () {
            c_submit_membership_info.exe();
        });

        $("#btn_submit_mobile_authentication_number").bind("click", function () {
            c_submit_mobile_authentication_number.exe();
        });

        $("#rb_key_file_generate_yes").bind("click", function () {
            c_whether_key_file_generate.exe(true);
        });

        $("#rb_key_file_generate_no").bind("click", function () {
            c_whether_key_file_generate.exe(false);
        });
    }
}

var c_register_events = new class_register_events();
