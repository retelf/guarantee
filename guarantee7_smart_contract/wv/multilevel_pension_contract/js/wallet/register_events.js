var class_register_events = function () {

    this.exe = function (supplier) {

        $("#btn_contract").bind("click", function () {

            if (supplier == "guarantee") {
                c_guarantee_supplier.exe();
            } else if (supplier == "ethereum") {
                c_ethereum_supplier.exe();
            }
        });
    }
}; var c_register_events = new class_register_events();