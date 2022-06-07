var class_register_events = function () {

    this.exe = function () {

        $("#btn_submit").bind("click", function () {
            c_submit_register_info.exe();
        });
    }
}

var c_register_events = new class_register_events();
