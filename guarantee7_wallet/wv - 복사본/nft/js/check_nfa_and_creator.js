var class_check_nfa_and_creator = function () {

    var Me; this.initialize = function () { Me = c_check_nfa_and_creator; }

    this.button_selected;

    this.exe = function () {

        let jsonObject = {
            key: "check_nfa_and_creator",
            value:
            {
                nfa: $('#text_nfa').val()
            }
        }
        window.chrome.webview.postMessage(jsonObject);

    }

}; var c_check_nfa_and_creator = new class_check_nfa_and_creator();