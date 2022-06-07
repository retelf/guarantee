var class_select_event = function () {

    this.sell_order_right_now = function (bool) {

        if (bool) {
            $('#rb_sell_order_later').prop('checked', false);
        } else {
            $('#rb_sell_order_right_now').prop('checked', false);
        }
    }

    this.nft_type = function (rb_object) {

        if ($(rb_object).attr('id') =='rb_nft_type_pure_file') {
            $('#rb_nft_type_pure_real').prop('checked', false);
            $('#rb_nft_type_file_based_real').prop('checked', false);
        } else if ($(rb_object).attr('id') == 'rb_nft_type_pure_real') {
            $('#rb_nft_type_pure_file').prop('checked', false);
            $('#rb_nft_type_file_based_real').prop('checked', false);
        } else {
            $('#rb_nft_type_pure_file').prop('checked', false);
            $('#rb_nft_type_pure_real').prop('checked', false);
        }
    }
}

var c_select_event = new class_select_event();
