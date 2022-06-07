var class_set_interval_display_board = function () {

    var Me; this.initialize = function () { Me = c_set_interval_display_board; }

    this.exe = function (from) {

        if (GR.set_interval_running) {
            let jsonObject = {
                key: "display_board_" + from,
                value:
                {
                }
            }
            window.chrome.webview.postMessage(jsonObject);
        }
    }

}; var c_set_interval_display_board = new class_set_interval_display_board();