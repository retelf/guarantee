var class_submit_open_file_dialog = function () {

    var Me; this.initialize = function () { Me = c_submit_open_file_dialog; }

    this.button_selected;

    this.exe = function () {

        let jsonObject = {
            key: "open_file_dialog",
            value:
            {
            }
        }
        window.chrome.webview.postMessage(jsonObject);

    }

}; var c_submit_open_file_dialog = new class_submit_open_file_dialog();