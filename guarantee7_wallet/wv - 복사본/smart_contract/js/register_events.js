var class_register_events = function () {

    this.exe = function () {

        $("#btn_load").bind("click", function () {
            c_load.exe();
        });
    }
}

var c_register_events = new class_register_events();
