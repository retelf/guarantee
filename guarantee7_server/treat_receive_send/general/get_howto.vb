Public Class get_howto

    Public Shared Function nonce_too_low(phase As String, transfer_data As GRT.transfer_ethereum.st_data) As String

        ' initial 인 경우 뿐만 아니라 cancel 모드인 경우라 할지라도 nonce_too_low 가 되었다는 것은 다른 곳에서의 거래가 먼저 완료되었다는 것을 의미하기 때문에 
        ' initial, cancel 모두가 동일 논스로서 실행되지 않았다는 것을 나타낸다.
        '
        Dim hash_phrase As String

        If Not transfer_data.receipt_cancel Is Nothing Then
            hash_phrase = "<BR/><BR/>최초 송금 해시 : " & transfer_data.transaction_hash_initial & "<BR/>송금 취소 해시 : " & transfer_data.transaction_hash_cancel & "<BR/><BR/>" & " 입니다. "
        Else
            hash_phrase = "<BR/><BR/>최초 송금 해시 : " & transfer_data.transaction_hash_initial & " 입니다. "
        End If

        Return "<BR/><BR/>" & "이더리움의 논스 중복이 발생하여 거래가 전면 취소되었습니다. 거래를 다시 시도하시기 바랍니다." & vbCrLf &
                "문제되는 이더리움 트랜젝션 해시값은 " & hash_phrase

    End Function

    Public Shared Function transfer_fail_clear(amount As Decimal, gasPrice As Decimal, gasLimit As Decimal) As String

        Return "<BR/><BR/>" & "송금이 실패하였습니다." & "<BR/>" & "<BR/>" &
                "거래는 취소되었습니다. 송금액(" & amount & " ethereum)은 인출되지 않았지만 취소를 위한 가스비(" & (gasPrice * 126 / 100) * gasLimit & "Gwei)는 지출되었습니다."

    End Function

    Public Shared Function load_order_fail_clear(amount As Decimal, gasPrice As Decimal, gasLimit As Decimal) As String

        Return "<BR/><BR/>" & "환전등록이 실패하였습니다." & "<BR/>" & "<BR/>" &
                "등록을 위한 송금은 취소되었습니다. 송금액(" & amount & " ethereum)은 인출되지 않았지만 취소를 위한 가스비(" & (gasPrice * 126 / 100) * gasLimit & "Gwei)는 지출되었습니다."

    End Function

    Public Shared Function exchange_fail_clear(eoa_seller As String, eoa_buyer As String, na As String, clickers_coin_name_to_buy As String, amount As Decimal, gasPrice As Decimal, gasLimit As Decimal) As String

        If clickers_coin_name_to_buy = "ethereum" Then

            Return "<BR/><BR/>" & "환전이 실패하였습니다." & "<BR/>" & "<BR/>" &
                    "환전를 위한 거래소(" & na & ")로부터 " & eoa_buyer & "로의 이더리움 송금은 취소되었습니다. " &
                    "다만 거래소(" & na & ")의 계정으로부터 취소를 위한 가스비(" & (gasPrice * 126 / 100) * gasLimit & "Gwei)는 지출되었습니다. 다시 한번 시도하시기 바랍니다."

        Else

            Return "<BR/><BR/>" & "환전이 실패하였습니다." & "<BR/>" & "<BR/>" &
                    "환전를 위한 " & eoa_buyer & " 으로부터 " & eoa_seller & " 로의 이더리움 송금은 취소되었습니다. " &
                    "다만 " & eoa_buyer & " 의 계정으로부터 취소를 위한 가스비(" & (gasPrice * 126 / 100) * gasLimit & "Gwei)는 지출되었습니다."

        End If

    End Function

    Public Shared Function exchange_cancel_fail_clear(eoa As String, na As String, amount As Decimal, gasPrice As Decimal, gasLimit As Decimal) As String

        Return "<BR/><BR/>" & "환불이 실패하였습니다." & "<BR/>" & "<BR/>" &
                "취소를 위한 거래소(" & na & ")로부터 " & eoa & "로의 송금은 취소되었습니다. " &
                "다만 거래소(" & na & ")의 계정으로부터 취소를 위한 가스비(" & (gasPrice * 126 / 100) * gasLimit & "Gwei)는 지출되었습니다. 다시 한번 시도하시기 바랍니다."

    End Function

    Public Shared Function multilevel_buy_transfer_fail_clear(amount As Decimal, gasPrice As Decimal, gasLimit As Decimal) As String

        Return "<BR/><BR/>" & "사업자 취득에 실패하였습니다." & "<BR/>" & "<BR/>" &
                "거래는 취소되었습니다. 취득비용(" & amount & " ethereum)은 인출되지 않았지만 취소를 위한 가스비(" & (gasPrice * 112 / 100) * gasLimit & "Gwei)는 지출되었습니다."

    End Function

    Public Shared Function multilevel_recall_transfer_fail_clear(amount As Decimal, na As String, exchange_name As String) As String

        Return "<BR/><BR/>" & "사업자 반납에는 성공하였습니다. 그러나 사업자 반납에 대응하는 이더리움(" & amount & " ethereum) 환불을 위한 송금에는 실패하였습니다. " & "<BR/>" & "<BR/>" &
                exchange_name & "거래소(" & na & ") 에 직접 반납을 요청하시기 바랍니다."

    End Function

    Public Shared Function transfer_success_unclear(amount As Decimal, eoa_to As String, transaction_hash_initial As String, transaction_hash_cancel As String) As String

        Dim hash_phrase As String

        If Not transaction_hash_cancel Is Nothing Then
            hash_phrase = "<BR/><BR/>최초 송금 해시 : " & transaction_hash_initial & "<BR/>송금 취소 해시 : " & transaction_hash_cancel & "<BR/><BR/>" & " 입니다. "
        Else
            hash_phrase = "<BR/><BR/>최초 송금 해시 : " & transaction_hash_initial & " 입니다. "
        End If

        Return "<BR/><BR/>" & "송금이 실패한 것으로 보이지만 확실치는 않습니다. " & "<BR/>" & "<BR/>" &
                "본 상황은 이더리움 플랫폼의 근본적인 불안정성 때문에 왕왕 발생하는 거래지연 내지 실패 상황입니다. " &
                "현재 이더리움이 인출되었는 지 아닌 지 확인하는 방법은 지금 이 시각부터 적당한 시간 - 대략 하루 정도 - 까지 수작업으로 확인하는 수 밖에 없습니다. " &
                "이더리움 플랫폼은 이런 경우 그 어떤 처리방법도 제공해 주지 않으며 모든 것을 클라이언트가 알아서 처리해야 한다는 약관을 규정하고 있습니다." & "<BR/>" &
                "결론적으로 클라이언트께서는 스스로 이더리움 네트워크에서 제공하는 사이트(메타마스크 등)에서 인출 여부를 확인하셔야 합니다. 문의를 위한 해시값은 " &
                hash_phrase &
                "만약 상대방(" & eoa_to & ")에게 " & amount & " 이더리움이 송금되었다면 오프라인에서 별도로 환급을 받으셔야 합니다. " &
                "그 외의 다른 방법은 없습니다. 이더리움 플랫폼의 근본적인 문제로서 이들과 연동하는 이상 필연적으로 겪을 수 밖에 없는 불편입니다. 이 점 양해를 부탁드립니다."

    End Function

    Public Shared Function load_order_success_unclear(amount As Decimal, na As String, exchange_name As String, transaction_hash_initial As String, transaction_hash_cancel As String) As String

        Dim hash_phrase As String

        If Not transaction_hash_cancel Is Nothing Then
            hash_phrase = "<BR/><BR/>최초 송금 해시 : " & transaction_hash_initial & "<BR/>송금 취소 해시 : " & transaction_hash_cancel & "<BR/><BR/>" & " 입니다. "
        Else
            hash_phrase = "<BR/><BR/>최초 송금 해시 : " & transaction_hash_initial & " 입니다. "
        End If

        Return "<BR/><BR/>" & "환전등록을 위한 이더리움 송금이 실패한 것으로 보이지만 확실치는 않습니다. " & "<BR/>" & "<BR/>" &
                "본 상황은 이더리움 플랫폼의 근본적인 불안정성 때문에 왕왕 발생하는 거래지연 내지 실패 상황입니다. " &
                "현재 이더리움이 인출되었는 지 아닌 지 확인하는 방법은 지금 이 시각부터 적당한 시간 - 대략 하루 정도 - 까지 수작업으로 확인하는 수 밖에 없습니다. " &
                "이더리움 플랫폼은 이런 경우 그 어떤 처리방법도 제공해 주지 않으며 모든 것을 클라이언트가 알아서 처리해야 한다는 약관을 규정하고 있습니다." & "<BR/>" &
                "결론적으로 클라이언트께서는 해당 거래소(" & exchange_name & ", 주소 : " & na & ")에 문의하셔야 하며 문의를 위한 해시값은 " &
                hash_phrase &
                "만약 " & amount & " 이더리움이 해당 거래소로 인출되었다면 오프라인에서 별도 송금을 통해 환급을 받으셔야 합니다. " &
                "그 외의 다른 방법은 없습니다. 이더리움 플랫폼의 근본적인 문제로서 이들과 연동하는 이상 필연적으로 겪을 수 밖에 없는 불편입니다. 이 점 양해를 부탁드립니다."

    End Function

    Public Shared Function exchange_success_unclear(amount As Decimal, eoa_seller As String, eoa_buyer As String, na As String, clickers_coin_name_to_buy As String, exchange_name As String, transaction_hash_initial As String, transaction_hash_cancel As String) As String

        Dim hash_phrase As String

        If Not transaction_hash_cancel Is Nothing Then
            hash_phrase = "<BR/><BR/>최초 송금 해시 : " & transaction_hash_initial & "<BR/>송금 취소 해시 : " & transaction_hash_cancel & "<BR/><BR/>" & " 입니다. "
        Else
            hash_phrase = "<BR/><BR/>최초 송금 해시 : " & transaction_hash_initial & " 입니다. "
        End If

        If clickers_coin_name_to_buy = "ethereum" Then

            Return "<BR/><BR/>" & "환전을 위한 거래소(" & na & ")로부터 " & eoa_buyer & "로의 " & amount & " 이더리움 송금이 실패한 것으로 보이지만 확실치는 않습니다. " & "<BR/>" & "<BR/>" &
                    "이로 인하여 " & eoa_buyer & "로부터 " & eoa_seller & " 로의 개런티 송금 역시 실시되지 않았습니다. 그러나 이더리움에 관해서는 아직은 결과가 불확실합니다." & "<BR/>" & "<BR/>" &
                    "본 상황은 이더리움 플랫폼의 근본적인 불안정성 때문에 왕왕 발생하는 거래지연 내지 실패 상황입니다. " &
                    "현재 이더리움이 환전되었는 지 아닌 지 확인하는 방법은 지금 이 시각부터 적당한 시간 - 대략 하루 정도 - 까지 수작업으로 확인하는 수 밖에 없습니다. " &
                    "이더리움 플랫폼은 이런 경우 그 어떤 처리방법도 제공해 주지 않으며 모든 것을 클라이언트가 알아서 처리해야 한다는 약관을 규정하고 있습니다." & "<BR/>" &
                    "결론적으로 클라이언트께서는 해당 거래소(" & exchange_name & ", 주소 : " & na & ")에 문의하셔야 하며 문의를 위한 해시값은 " &
                    hash_phrase &
                    "그 외의 다른 방법은 없습니다. 이더리움 플랫폼의 근본적인 문제로서 이들과 연동하는 이상 필연적으로 겪을 수 밖에 없는 불편입니다. 이 점 양해를 부탁드립니다."

        Else

            Return "<BR/><BR/>" & "환전을 위한 " & eoa_buyer & "로부터 " & eoa_seller & "로의 " & amount & " 이더리움 송금이 실패한 것으로 보이지만 확실치는 않습니다. " & "<BR/>" & "<BR/>" &
                    "이로 인하여 " & na & "로부터 " & eoa_buyer & " 로의 개런티 송금 역시 실시되지 않았습니다. 그러나 이더리움에 관해서는 아직은 결과가 불확실합니다." & "<BR/>" & "<BR/>" &
                    "본 상황은 이더리움 플랫폼의 근본적인 불안정성 때문에 왕왕 발생하는 거래지연 내지 실패 상황입니다. " &
                    "현재 이더리움이 환전되었는 지 아닌 지 확인하는 방법은 지금 이 시각부터 적당한 시간 - 대략 하루 정도 - 까지 수작업으로 확인하는 수 밖에 없습니다. " &
                    "이더리움 플랫폼은 이런 경우 그 어떤 처리방법도 제공해 주지 않으며 모든 것을 클라이언트가 알아서 처리해야 한다는 약관을 규정하고 있습니다." & "<BR/>" &
                    "본 거래에서의 이더리움 송금은 순수히 개인간 거래였으며 거래소를 경유하지 않습니다. 따라서 클라이언트께서는 스스로 이더리움 네트워크에서 제공하는 사이트(메타마스크 등)에서 인출 여부를 확인하셔야 합니다. 문의를 위한 해시값은 " &
                    hash_phrase &
                    "다만 해당 거래소(" & exchange_name & ", 주소 : " & na & ")에도 다소간의 자료는 남아 있으므로 문의가 가능합니다. " &
                    "그 외의 다른 방법은 없습니다. 이더리움 플랫폼의 근본적인 문제로서 이들과 연동하는 이상 필연적으로 겪을 수 밖에 없는 불편입니다. 이 점 양해를 부탁드립니다."

        End If

    End Function

    Public Shared Function exchange_cancel_success_unclear(amount As Decimal, eoa As String, na As String, exchange_name As String, transaction_hash_initial As String, transaction_hash_cancel As String) As String

        Dim hash_phrase As String

        If Not transaction_hash_cancel Is Nothing Then
            hash_phrase = "<BR/><BR/>최초 송금 해시 : " & transaction_hash_initial & "<BR/>송금 취소 해시 : " & transaction_hash_cancel & "<BR/><BR/>" & " 입니다. "
        Else
            hash_phrase = "<BR/><BR/>최초 송금 해시 : " & transaction_hash_initial & " 입니다. "
        End If

        Return "<BR/><BR/>" & "환불을 위한 거래소(" & na & ")로부터 " & eoa & "로의 " & amount & " 이더리움 송금이 실패한 것으로 보이지만 확실치는 않습니다. " & "<BR/>" & "<BR/>" &
                "본 상황은 이더리움 플랫폼의 근본적인 불안정성 때문에 왕왕 발생하는 거래지연 내지 실패 상황입니다. " &
                "현재 이더리움이 환불되었는 지 아닌 지 확인하는 방법은 지금 이 시각부터 적당한 시간 - 대략 하루 정도 - 까지 수작업으로 확인하는 수 밖에 없습니다. " &
                "이더리움 플랫폼은 이런 경우 그 어떤 처리방법도 제공해 주지 않으며 모든 것을 클라이언트가 알아서 처리해야 한다는 약관을 규정하고 있습니다." & "<BR/>" &
                "결론적으로 클라이언트께서는 해당 거래소(" & exchange_name & ", 주소 : " & na & ")에 문의하셔야 하며 문의를 위한 해시값은 " &
                hash_phrase &
                "그 외의 다른 방법은 없습니다. 이더리움 플랫폼의 근본적인 문제로서 이들과 연동하는 이상 필연적으로 겪을 수 밖에 없는 불편입니다. 이 점 양해를 부탁드립니다."

    End Function

    Public Shared Function multilevel_buy_transfer_success_unclear(amount As Decimal, na As String, exchange_name As String, transaction_hash_initial As String, transaction_hash_cancel As String) As String

        Dim hash_phrase As String

        If Not transaction_hash_cancel Is Nothing Then
            hash_phrase = "<BR/><BR/>최초 송금 해시 : " & transaction_hash_initial & "<BR/>송금 취소 해시 : " & transaction_hash_cancel & "<BR/><BR/>" & " 입니다. "
        Else
            hash_phrase = "<BR/><BR/>최초 송금 해시 : " & transaction_hash_initial & " 입니다. "
        End If

        Return "<BR/><BR/>" & "사업자 취득에 실패하였습니다." & "<BR/>" & "<BR/>" &
                "본 상황은 이더리움 플랫폼의 근본적인 불안정성 때문에 왕왕 발생하는 거래지연 내지 실패 상황입니다. " &
                "현재 이더리움이 인출되었는 지 아닌 지 확인하는 방법은 지금 이 시각부터 적당한 시간 - 대략 하루 정도 - 까지 수작업으로 확인하는 수 밖에 없습니다. " &
                "이더리움 플랫폼은 이런 경우 그 어떤 처리방법도 제공해 주지 않으며 모든 것을 클라이언트가 알아서 처리해야 한다는 약관을 규정하고 있습니다." & "<BR/>" &
                "결론적으로 클라이언트께서는 해당 거래소(" & exchange_name & ", 주소 : " & na & ")에 문의하셔야 하며 문의를 위한 해시값은 " &
                hash_phrase &
                "만약 " & amount & " 이더리움이 해당 거래소로 인출되었다면 오프라인에서 별도 송금을 통해 환급을 받으셔야 합니다. " &
                "그 외의 다른 방법은 없습니다. 이더리움 플랫폼의 근본적인 문제로서 이들과 연동하는 이상 필연적으로 겪을 수 밖에 없는 불편입니다. 이 점 양해를 부탁드립니다."

    End Function

    Public Shared Function multilevel_recall_transfer_success_unclear(amount As Decimal, na As String, exchange_name As String, transaction_hash_initial As String, transaction_hash_cancel As String) As String

        Dim hash_phrase As String

        If Not transaction_hash_cancel Is Nothing Then
            hash_phrase = "<BR/><BR/>최초 송금 해시 : " & transaction_hash_initial & "<BR/>송금 취소 해시 : " & transaction_hash_cancel & "<BR/><BR/>" & " 입니다. "
        Else
            hash_phrase = "<BR/><BR/>최초 송금 해시 : " & transaction_hash_initial & " 입니다. "
        End If

        Return "<BR/><BR/>" & "사업자 반납에는 성공하였습니다. 그러나 사업자 반납에 대응하는 이더리움(" & amount & " ethereum) 환불을 위한 송금이 제대로 이루어졌는 지는 명확하지 않습니다." & "<BR/>" & "<BR/>" &
                "본 상황은 이더리움 플랫폼의 근본적인 불안정성 때문에 왕왕 발생하는 거래지연 내지 실패 상황입니다. " &
                "현재 이더리움이 송금되었는 지 아닌 지 확인하는 방법은 지금 이 시각부터 적당한 시간 - 대략 하루 정도 - 까지 수작업으로 확인하는 수 밖에 없습니다. " &
                "이더리움 플랫폼은 이런 경우 그 어떤 처리방법도 제공해 주지 않으며 모든 것을 클라이언트가 알아서 처리해야 한다는 약관을 규정하고 있습니다." & "<BR/>" &
                "결론적으로 클라이언트께서는 해당 거래소(" & exchange_name & ", 주소 : " & na & ")에 문의하셔야 하며 문의를 위한 해시값은 " &
                hash_phrase &
                "만약 " & amount & " 이더리움이 해당 거래소로부터 인출되지 않았다면 귀하께서는 오프라인에서 별도 송금을 통해 환급을 받으셔야 합니다. " &
                "그 외의 다른 방법은 없습니다. 이더리움 플랫폼의 근본적인 문제로서 이들과 연동하는 이상 필연적으로 겪을 수 밖에 없는 불편입니다. 이 점 양해를 부탁드립니다."

    End Function

    Public Shared Function transfer_ethereum_from_exchange_case_proceeding_exchange(amount As Decimal, eoa_seller As String, eoa_buyer As String, na As String, clickers_coin_name_to_buy As String, exchange_name As String, transaction_hash_initial As String, transaction_hash_cancel As String) As String

        ' clickers_coin_name_to_buy = ethereum 인 경우만 이곳으로 온다.

        Dim hash_phrase As String

        If Not transaction_hash_cancel Is Nothing Then
            hash_phrase = "<BR/><BR/>최초 송금 해시 : " & transaction_hash_initial & "<BR/>송금 취소 해시 : " & transaction_hash_cancel & "<BR/><BR/>" & " 입니다. "
        Else
            hash_phrase = "<BR/><BR/>최초 송금 해시 : " & transaction_hash_initial & " 입니다. "
        End If

        Return "<BR/><BR/>" & "환전을 위한 거래소(" & na & ")로부터 " & eoa_buyer & "로의 " & amount & " 이더리움 송금 완료가 확인되지 않았지만 " & eoa_buyer & "로부터 " & eoa_seller & " 로의 개런티 송금은 실시되었습니다. " &
            "본 상황은 이더리움 플랫폼의 근본적인 불안정성 때문에 왕왕 발생하는 거래지연 내지 실패 상황입니다. " &
            "현재 이더리움이 환전되었는 지 아닌 지 확인하는 방법은 지금 이 시각부터 적당한 시간 - 대략 하루 정도 - 까지 수작업으로 확인하는 수 밖에 없습니다. " &
            "이더리움 플랫폼은 이런 경우 그 어떤 처리방법도 제공해 주지 않으며 모든 것을 클라이언트가 알아서 처리해야 한다는 약관을 규정하고 있습니다." & "<BR/>" &
            "결론적으로 클라이언트께서는 해당 거래소(" & exchange_name & ", 주소 : " & na & ")에 문의하셔야 하며 문의를 위한 해시값은 " &
            hash_phrase &
            "그 외의 다른 방법은 없습니다. 이더리움 플랫폼의 근본적인 문제로서 이들과 연동하는 이상 필연적으로 겪을 수 밖에 없는 불편입니다. 이 점 양해를 부탁드립니다."

    End Function

    Public Shared Function transfer_ethereum_from_exchange_case_proceeding_exchange_cancel(amount As Decimal, eoa As String, na As String, exchange_name As String, transaction_hash_initial As String, transaction_hash_cancel As String) As String

        Dim hash_phrase As String

        If Not transaction_hash_cancel Is Nothing Then
            hash_phrase = "<BR/><BR/>최초 송금 해시 : " & transaction_hash_initial & "<BR/>송금 취소 해시 : " & transaction_hash_cancel & "<BR/><BR/>" & " 입니다. "
        Else
            hash_phrase = "<BR/><BR/>최초 송금 해시 : " & transaction_hash_initial & " 입니다. "
        End If

        Return "<BR/><BR/>" & "환불을 위한 거래소(" & na & ")로부터 " & eoa & "로의 " & amount & " 이더리움 송금이 실패한 것으로 보이지만 확실치는 않습니다. " & "<BR/>" & "<BR/>" &
                "본 상황은 이더리움 플랫폼의 근본적인 불안정성 때문에 왕왕 발생하는 거래지연 내지 실패 상황입니다. " &
                "현재 이더리움이 환불되었는 지 아닌 지 확인하는 방법은 지금 이 시각부터 적당한 시간 - 대략 하루 정도 - 까지 수작업으로 확인하는 수 밖에 없습니다. " &
                "이더리움 플랫폼은 이런 경우 그 어떤 처리방법도 제공해 주지 않으며 모든 것을 클라이언트가 알아서 처리해야 한다는 약관을 규정하고 있습니다." & "<BR/>" &
                "결론적으로 클라이언트께서는 해당 거래소(" & exchange_name & ", 주소 : " & na & ")에 문의하셔야 하며 문의를 위한 해시값은 " &
                hash_phrase &
                "그 외의 다른 방법은 없습니다. 이더리움 플랫폼의 근본적인 문제로서 이들과 연동하는 이상 필연적으로 겪을 수 밖에 없는 불편입니다. 이 점 양해를 부탁드립니다."

    End Function
    Public Shared Function nft_buy_transfer_fail_clear(amount As Decimal, gasPrice As Decimal, gasLimit As Decimal) As String

        Return "<BR/><BR/>" & "NFT 매입에 실패하였습니다." & "<BR/>" & "<BR/>" &
                "거래는 취소되었습니다. 취득비용(" & amount & " ethereum)은 인출되지 않았지만 취소를 위한 가스비(" & (gasPrice * 112 / 100) * gasLimit & "Gwei)는 지출되었습니다."

    End Function

    Public Shared Function nft_buy_transfer_success_unclear(amount As Decimal, buyer_na As String, exchange_name_buyer As String, seller_na As String, exchange_name_seller As String, transaction_hash_initial As String, transaction_hash_cancel As String) As String

        Dim hash_phrase As String

        If Not transaction_hash_cancel Is Nothing Then
            hash_phrase = "<BR/><BR/>최초 송금 해시 : " & transaction_hash_initial & "<BR/>송금 취소 해시 : " & transaction_hash_cancel & "<BR/><BR/>" & " 입니다. "
        Else
            hash_phrase = "<BR/><BR/>최초 송금 해시 : " & transaction_hash_initial & " 입니다. "
        End If

        Return "<BR/><BR/>" & "NFT 매입에 실패하였습니다." & "<BR/>" & "<BR/>" &
                "본 상황은 이더리움 플랫폼의 근본적인 불안정성 때문에 왕왕 발생하는 거래지연 내지 실패 상황입니다. " &
                "현재 이더리움이 인출되었는 지 아닌 지 확인하는 방법은 지금 이 시각부터 적당한 시간 - 대략 하루 정도 - 까지 수작업으로 확인하는 수 밖에 없습니다. " &
                "이더리움 플랫폼은 이런 경우 그 어떤 처리방법도 제공해 주지 않으며 모든 것을 클라이언트가 알아서 처리해야 한다는 약관을 규정하고 있습니다." & "<BR/>" &
                "결론적으로 클라이언트께서는 해당 거래소(" & exchange_name_buyer & ", 주소 : " & buyer_na & ")에 문의하셔야 하며 문의를 위한 해시값은 " &
                hash_phrase &
                "만약 " & amount & " 이더리움이 해당 거래소로 인출되었다면 오프라인에서 별도 송금을 통해 환급을 받으셔야 합니다. " &
                "그 외의 다른 방법은 없습니다. 이더리움 플랫폼의 근본적인 문제로서 이들과 연동하는 이상 필연적으로 겪을 수 밖에 없는 불편입니다. 이 점 양해를 부탁드립니다."

    End Function

    Public Shared Function nft_recall_no_need_cancel(amount As Decimal, recaller_na As String, exchange_name_recaller As String, seller_na As String, exchange_name_seller As String, transaction_hash_initial As String) As String

        Dim hash_phrase As String = "<BR/><BR/>송금 해시 : " & transaction_hash_initial & " 입니다. "

        Return "<BR/><BR/>" & "이더리움 환불 성공 여부가 불확실합니다. 단 NFT 반납은 실시되었습니다." & "<BR/>" & "<BR/>" &
                "본 상황은 이더리움 플랫폼의 근본적인 불안정성 때문에 왕왕 발생하는 거래지연 내지 실패 상황입니다. " &
                "현재 이더리움이 환불되었는 지 아닌 지 확인하는 방법은 지금 이 시각부터 적당한 시간 - 대략 하루 정도 - 까지 수작업으로 확인하는 수 밖에 없습니다. " &
                "이더리움 플랫폼은 이런 경우 그 어떤 처리방법도 제공해 주지 않으며 모든 것을 클라이언트가 알아서 처리해야 한다는 약관을 규정하고 있습니다." & "<BR/>" &
                "결론적으로 클라이언트께서는 해당 거래소(" & exchange_name_recaller & ", 주소 : " & recaller_na & ")에 문의하셔야 하며 문의를 위한 해시값은 " &
                hash_phrase &
                "만약 " & amount & " 이더리움이 해당 거래소로부터 환급되지 않았다면 오프라인에서 별도 송금을 통해 환급을 받으셔야 합니다. " &
                "그 외의 다른 방법은 없습니다. 이더리움 플랫폼의 근본적인 문제로서 이들과 연동하는 이상 필연적으로 겪을 수 밖에 없는 불편입니다. 이 점 양해를 부탁드립니다."

    End Function

    Public Shared Function clear_deposit_no_need_cancel(amount As Decimal, na_depositor As String, exchange_name_na_depositor As String, transaction_hash_initial As String) As String

        Dim hash_phrase As String = "<BR/><BR/>송금 해시 : " & transaction_hash_initial & " 입니다. "

        Return "<BR/><BR/>" & "이더리움 송금 성공 여부가 불확실합니다. " & "<BR/>" & "<BR/>" &
                "본 상황은 이더리움 플랫폼의 근본적인 불안정성 때문에 왕왕 발생하는 거래지연 내지 실패 상황입니다. " &
                "현재 이더리움이 송금되었는 지 아닌 지 확인하는 방법은 지금 이 시각부터 적당한 시간 - 대략 하루 정도 - 까지 수작업으로 확인하는 수 밖에 없습니다. " &
                "이더리움 플랫폼은 이런 경우 그 어떤 처리방법도 제공해 주지 않으며 모든 것을 클라이언트가 알아서 처리해야 한다는 약관을 규정하고 있습니다." & "<BR/>" &
                "결론적으로 클라이언트께서는 해당 거래소(" & exchange_name_na_depositor & ", 주소 : " & na_depositor & ")에 문의하셔야 하며 문의를 위한 해시값은 " &
                hash_phrase &
                "만약 " & amount & " 이더리움이 해당 거래소로부터 환급되지 않았다면 오프라인에서 별도 송금을 통해 환급을 받으셔야 합니다. " &
                "그 외의 다른 방법은 없습니다. 이더리움 플랫폼의 근본적인 문제로서 이들과 연동하는 이상 필연적으로 겪을 수 밖에 없는 불편입니다. 이 점 양해를 부탁드립니다."

    End Function

End Class
