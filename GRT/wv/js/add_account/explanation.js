var class_explanation = function () {

    var Me; this.initialize = function () { Me = c_explanation; }

    this.exe = function (json_message) {

        json_object = JSON.parse(json_message);

        let explanation_content;

        switch (json_object.key) {

            case "no_key_file_on_login":

                explanation_content = "키파일이 존재하지 않습니다. 이 기기에는 " + json_object.value.publick_key + " 계정의 키파일이 저장된 바 없습니다. 일단은 비밀키(PrivateKey)를 입력해서 로그인 하시거나 아니면 본 기기에 키파일을 생성시키고 이를 이용하시기 바랍니다.";

                $('#div_subject').html("키파일 부존재");
                $('#div_explanation').html(explanation_content);

                $('#div_private_key_outer').css('display', 'block');

                $('#text_password').prop('disabled', true);break;

            case "ineffective_private_key":

                explanation_content = json_object.value.private_key + " 는 입력하신 공개키에 상응하는 비밀키가 아닙니다.";

                $('#div_subject').html("비밀키 오류");
                $('#div_explanation').html(explanation_content); break;

            case "login_result":

                if (json_object.success == "success") {

                    explanation_content = "로그인 되었습니다.";

                    $('#div_subject').html("로그인 성공");
                    $('#div_explanation').html(explanation_content); break;

                } else {

                    switch (json_object.value.reason) {

                        case "no_account":

                            if (json_object.value.coin_name == 'guarantee') {
                                explanation_content = json_object.value.publick_key + " 는 본 플랫폼에서 생성된 공개키가 아닙니다. 실명제를 위하여 이런 형식의 공개키는 추가가 불가능하며 반드시 이곳에서 별도의 신규키를 발급받으셔야 합니다.";
                            } else {
                                explanation_content = json_object.value.publick_key + " 에 관하여 아직 계정이 존재하지 않습니다. 계정을 만드시거나 추가하셔야 합니다.";
                            }

                            $('#div_subject').html("로그인 실패");
                            $('#div_explanation').html(explanation_content); break;

                        case "verification_fail": "본인이 아닌 자에 의한 로그인 시도입니다.";

                            $('#div_subject').html("로그인 실패");
                            $('#div_explanation').html(explanation_content); break;

                    }

                }

            case "check_registered_eoa", "execute_add_account_new", "execute_add_account_already":

                $('#div_explanation').html(json_message); break;
        }
    }
}

var c_explanation = new class_explanation(); c_explanation.initialize();
