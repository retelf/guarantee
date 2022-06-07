Imports System.IO
Imports System.Numerics
Imports Nethereum.RPC.NonceServices

Public Class treat_from_php

    Public Shared Async Function exe(json As Newtonsoft.Json.Linq.JObject, JSS As String) As Task(Of String)

        Dim JRS As String
        Dim command_key = CStr(json("key"))
        Dim sub_key = CStr(json("sub_key"))

        Dim reg_from_local_string As String = "^(localhost|10\.|::1/|fc00|fe80|127\.0|169\.254|172\.16|192\.168)"

        Dim remote_endpoint = CStr(json("value")("remote_endpoint"))

        If Regex.Match(remote_endpoint, reg_from_local_string).Success Then

            Select Case sub_key

                Case "general_account_info"

                    Dim public_key = CStr(json("value")("public_key"))
                    Dim coin_name = CStr(json("value")("coin_name"))
                    Dim signiture = CStr(json("value")("signiture"))

                    Dim verified = GRT.Security.Gverify.verify("foo", signiture, public_key)

                    If verified Then

                        JRS = Await treat_execute_login.exe(json)

                        Dim case_retored_eoa = GRT.restore_ethereum_public_key.exe_from_signiture("foo", signiture)

                        Dim json_JRS As Newtonsoft.Json.Linq.JObject = CType(JsonConvert.DeserializeObject(JRS), Linq.JObject)

                        json_JRS("value")("case_retored_eoa") = case_retored_eoa

                        JRS = CType(JsonConvert.SerializeObject(json_JRS), String)

                    Else

                        JRS = "{""key"" : ""error"", ""success"" : ""fail"", ""reason"" : ""데이터변조"" }"

                    End If

                Case "publick_key_recover_upper_case"

                    Dim password = CStr(json("value")("data"))
                    Dim password_signiture = CStr(json("value")("signiture"))

                    Dim restored_public_key = GRT.restore_ethereum_public_key.exe_from_signiture(password, password_signiture)

                    JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "success", "quot"}}, {{"restored_public_key", restored_public_key, "quot"}}, False)

                Case "request_additional_info"

                    JRS = GRT.make_json_string.exe(
                        {{"key", command_key, "quot"}, {"success", "success", "quot"}},
                        {
                        {"geth_network_id", CStr(GRT.GR.account.ethereum_chain_id), "non_quot"},
                        {"nft_transaction_fee_rate", CStr(GRT.GR.nft_transaction_fee_rate), "non_quot"}
                        }, False)

                Case "nonce_and_gasPrice", "only_gasPrice"

                    Dim public_key = CStr(json("value")("public_key"))

                    Dim signiture = CStr(json("value")("signiture"))

                    Dim verified = GRT.Security.Gverify.verify("foo", signiture, public_key)

                    If verified Then

                        Dim futureNonce As Nethereum.Hex.HexTypes.HexBigInteger
                        Dim NonceService As InMemoryNonceService
                        Dim nonce_string As String = "-1"

                        If sub_key = "nonce_and_gasPrice" Then

                            NonceService = New InMemoryNonceService(public_key, GRT.GR.ethereum.web3.Client)
                            futureNonce = Await GRT.nonce_management.exe(public_key)
                            nonce_string = futureNonce.Value.ToString

                        End If

                        Dim recommanded_gas_price_hex = Await GRT.GR.ethereum.web3.Eth.GasPrice.SendRequestAsync

                        Dim recommanded_gas_price_dec = GRT.convert_hexbiginteger_decimal_unit_ethereum.exe(recommanded_gas_price_hex)

                        JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "success", "quot"}}, {{"nonce", nonce_string, "non_quot"}, {"gasPrice", CStr(recommanded_gas_price_dec), "non_quot"}}, False)

                    Else

                        JRS = "{""key"" : ""error"", ""success"" : ""fail"", ""reason"" : ""데이터변조"" }"

                    End If

                Case "GetNistTime"

                    Dim public_key = CStr(json("value")("public_key"))

                    Dim signiture = CStr(json("value")("signiture"))

                    Dim verified = GRT.Security.Gverify.verify("foo", signiture, public_key)

                    If verified Then

                        Dim nist_time As DateTime = GRT.GetNistTime.exe

                        JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "success", "quot"}}, {{"nist_time_string", nist_time.ToString("yyyy/MM/dd HH:mm:ss"), "quot"}}, False)

                    Else

                        JRS = "{""key"" : ""error"", ""success"" : ""fail"", ""reason"" : ""데이터변조"" }"

                    End If

                Case "get_decimal_value"

                    Dim _operator = CStr(json("value")("operator"))
                    Dim number_a = CDec(json("value")("number_a"))
                    Dim number_b = CDec(json("value")("number_b"))

                    Dim return_decimal As Decimal

                    If _operator = "multiply" Then
                        return_decimal = number_a * number_b
                    ElseIf _operator = "plus" Then
                        return_decimal = number_a + number_b
                    ElseIf _operator = "divide" Then
                        return_decimal = number_a / number_b
                    End If

                    Dim dec_str = Regex.Replace(CStr(return_decimal), "(\.)?0+$", "")

                    If dec_str = "" Then
                        dec_str = "0"
                    End If

                    return_decimal = CDec(dec_str)

                    JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "success", "quot"}}, {{"return_decimal", CStr(return_decimal), "quot"}}, False)

            End Select

        Else

            JRS = "{""key"" : ""error"", ""success"" : ""fail"", ""reason"" : ""from_outside_enter"" }"

        End If

        Return JRS

    End Function

End Class
