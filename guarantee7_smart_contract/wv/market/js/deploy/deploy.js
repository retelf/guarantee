var class_deploy = function () {

    this.exe = function () {

        // 일단 validation 중에서 급한 것만

        let jsonObject = {
            key: "submit_deploy",
            value:
            {
                industry_name: $('#text_industry_name').val(),
                smart_contract_name: $('#text_smart_contract_name').val(),
                code: $('#textarea_code').val(),
                extention: $('#text_extention').val(),
                create_table_string: $('#textarea_create_table_string').val(),
                file_name: $('#text_file_name').val()
            }
        }

        window.chrome.webview.postMessage(jsonObject);
    }
}

var c_deploy = new class_deploy();
