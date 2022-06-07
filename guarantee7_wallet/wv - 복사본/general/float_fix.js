var class_float_fix = function () {

    this.exe = function (float_a, float_b) {

        let str_after_comma_a, str_after_comma_b;
        let int_a, int_b;
        let zeros = 1;
        let final_zeros;
        let count_after_comma_a = 0;
        let count_after_comma_b = 0;

        str_after_comma_a = (float_a + '').match(/\.\d*$/) + '';

        if (str_after_comma_a !== null) {

            count_after_comma_a = str_after_comma_a.length - 1;

            for (let i = 0; i < count_after_comma_a; i++) {
                zeros = zeros * 10;
            }

            int_a = float_a * zeros;

            int_a = int_a.toFixed(0)

        } else {
            int_a = float_a;
        }

        zeros = 1;

        str_after_comma_b = (float_b + '').match(/\.\d*$/) + '';

        if (str_after_comma_b !== null) {

            count_after_comma_b = str_after_comma_b.length - 1;

            for (let i = 0; i < count_after_comma_b; i++) {
                zeros = zeros * 10;
            }

            int_b = float_b * zeros;

            int_b = int_b.toFixed(0)

        } else {
            int_b = float_b;
        }

        final_zeros = int_a * int_b;

        zeros = 1;

        for (let i = 0; i < count_after_comma_a + count_after_comma_b; i++) {
            zeros = zeros * 10;
        }

        final_zeros = final_zeros / zeros;

        return final_zeros;

    }

}; var c_float_fix = new class_float_fix();