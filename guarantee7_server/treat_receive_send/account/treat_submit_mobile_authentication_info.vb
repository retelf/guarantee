Public Class treat_submit_mobile_authentication_info

    Public Shared Async Function exe(json As Newtonsoft.Json.Linq.JObject) As Task(Of String)

        Dim pure_query, public_key, phone_number, idate_string, signiture As String
        Dim verified, authentication_number_send_ok As Boolean

        ' 가장 먼저 검증

        public_key = json("value")("public_key").ToString
        idate_string = json("value")("idate_string").ToString
        phone_number = json("value")("phone_number").ToString

        signiture = json("value")("signiture").ToString

        pure_query = GRT.GQS_insert_super_account_pure_query.exe(public_key, idate_string, json)

        verified = GRT.Security.Gverify.verify(pure_query, signiture, public_key)

        If verified Then

            ' 먼저 들어온 데이터를 임시 DB에 저장한다. 반드시 pure_query 와 signiture 도 함께 저장한다.

            Dim mobile_authentication_number = mobile_authentication.generate

            ' 이 시점에서 인증회사가 해당 번호로 모바일 인증번호를 발송한다.

            authentication_number_send_ok = Await mobile_authentication.send(phone_number, mobile_authentication_number)

            Dim ecKey = Nethereum.Signer.EthECKey.GenerateKey()

            Dim hash = ecKey.GetPublicAddress() ' 좀 그렇긴 하지만 ...

            If authentication_number_send_ok Then

                Call GRT.generate_temporary_super_account.exe(json, mobile_authentication_number, pure_query, signiture, hash)

                Return "{""key"" : ""mobile_certification_number_send"", ""success"" : ""success"", ""hash"" : """ & hash & """}"

            Else

                Return "{""key"" : ""mobile_certification_number_send"", ""success"" : ""fail"", ""hash"" : """ & hash & """}"

            End If

        Else

            Dim error_message = "보내주신 자료가 변조되었습니다."

            Return "{""key"" : ""mobile_certification_number_send"", ""success"" : ""fail"", ""reason"" : """ & error_message & """}"

        End If

    End Function

End Class
