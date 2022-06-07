var class_register_events = function () {

    this.exe = function () {

        $("#btn_wallet_source").bind("click", function () {
            c_wallet_source.exe();
        });

        $("#btn_deploy").bind("click", function () {
            c_deploy.exe();
        });
    }
}

var c_register_events = new class_register_events();
