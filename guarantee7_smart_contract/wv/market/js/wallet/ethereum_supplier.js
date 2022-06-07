var class_ethereum_supplier = function () {

    this.exe = function () {

        // 먼저 개런티 유효 잔고를 찾는다.
        // 있으면 안내 멘트와 동시에 현재 하이어라키 상태를 알려 준다.
        // 엔테를 치면
        // ethereum_supplier 테이블에 확정거래 입력을 하게 된다.
        // 없으면 개런티 시장으로 간다.

        // 로그인 된 이더리움 eoa 를 찾는다.이더리움 잔액을 확인한다.
        // 있으면 드랍다운으로 디스플레이한다. 드랍다운과 금액을 동시에 디스플레이한다.
        // 없으면 

        let jsonObject = {
            key: "submit_multilevel_pension_ethereum_supplier",
            value:
            {
                ethereum_eoa: $('#text_ethereum_eoa').val(),
                ethereum_balance: $('#div_ethereum_balance').val()
            }
        }
        window.chrome.webview.postMessage(jsonObject);
    }
}; var c_ethereum_supplier = new class_ethereum_supplier();