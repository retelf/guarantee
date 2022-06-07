var class_select_coin = function () {

    this.exe = function (coin_name) {

        if (coin_name=='guarantee') {
            $('#rb_ethereum_login').prop('checked', false);
            $('#div_transaction_type').css('display', 'none');
            $('#text_public_key').val('0xB9A25d241d2EF3F3cEac31c0C29Bee046C87CA49');
            $('#text_password').val('');
        } else {
            $('#rb_guarantee_login').prop('checked', false);
            $('#div_transaction_type').css('display', 'block');
            $('#text_public_key').val('0xe58a43B5b46b91184467FD2e5b594B4441682126');
            $('#text_password').val('#cyndi$36%');
            $('#text_private_key').val('0x569640918523a2698f2b0f47e8d2720efb1aeecda271524611c7320538eab8af');
            //$('#rb_guarantee_login').prop('checked', false);
            //$('#div_transaction_type').css('display', 'block');
            //$('#text_public_key').val('0x12c98cbe86B5e541391e58674398bEb529C3A1Bf');
            //$('#text_password').val('#cyndi$36%');
            //$('#text_private_key').val('0xd01bef55142cb72227e9edf3c90ea000877d71dbf8e4fd178ff84557a0650c77');
        }

        this.ethereum_transaction_type = function (rb) {

            $('.rb_ethereum_transaction_type').prop('checked', false);

            rb.prop('checked', true);
            if ($('#rb_signiture').prop('checked') || $('#rb_password').prop('checked')) {
                $('#div_password_outer').css('display', 'block');
                $('#div_private_key_outer').css('display', 'none');
            } else if ($('#rb_private_key').prop('checked')) {
                $('#div_private_key_outer').css('display', 'block');
                $('#div_password_outer').css('display', 'none');
            }
        }
    }
}

var c_select_coin = new class_select_coin();
