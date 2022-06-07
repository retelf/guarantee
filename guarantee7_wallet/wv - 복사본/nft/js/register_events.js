var class_register_events = function () {

    this.exe = function () {

        $("#btn_load_nft").bind("click", function () {
            c_load_nft.exe();
        });

        $("#rb_sell_order_right_now").bind("click", function () {
            c_select_event.sell_order_right_now(true);
        });

        $("#rb_sell_order_later").bind("click", function () {
            c_select_event.sell_order_right_now(false);
        });

        $("#rb_nft_type_pure_file").bind("click", function () {
            c_select_event.nft_type(this);
        });

        $("#rb_nft_type_pure_real").bind("click", function () {
            c_select_event.nft_type(this);
        });

        $("#rb_nft_type_file_based_real").bind("click", function () {
            c_select_event.nft_type(this);
        });

        $("#btn_open_file_dialog").bind("click", function () {
            c_submit_open_file_dialog.exe();
        });

        $("#btn_check_nfa_and_creator").bind("click", function () {
            c_check_nfa_and_creator.exe();
        });
    }
}

var c_register_events = new class_register_events();
