var class_check_days_span = function () {

    this.exe = function (btn_type, $tr_parent) {

        let closing_time_string_original, closing_time_string_simple;
        let closing_time_utc, now_time_utc;

        closing_time_string_original = $tr_parent.find('.div_board_closing_time_utc:first').text();

        closing_time_utc = (new Date(closing_time_string_original)).getTime();

        now_time_utc = c_getCurrentTimeUTC.exe();

        if (closing_time_utc >= now_time_utc) {
            return true;
        } else {
            return false;
        }     
    }
}

var c_check_days_span = new class_check_days_span();
