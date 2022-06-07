Public Class get_button_string

    Public Shared Function exe(i_number As Integer, eoa As String, buyer As String, state As String) As String()

        Dim button_left_type, button_right_type, bootstrap_left_button_type, bootstrap_right_button_type, button_string(1) As String

        Select Case state

            Case "alive"

                button_left_type = "buy" : button_right_type = "refund"

                bootstrap_left_button_type = "primary" : bootstrap_right_button_type = "warning"

                If eoa = Regex.Replace(GRT.GR.account.public_key, "^0x", "") Then

                    button_string(0) = ""
                    button_string(1) = "<button id='btn_" & button_right_type & "_" & i_number & "' type='button' class='btn btn_" & button_right_type & " btn-" & bootstrap_right_button_type & " btn-sm' onclick='c_submit_multilevel.exe($(this))'>" & button_right_type & "</button>"

                Else

                    button_string(0) = "<button id='btn_" & button_left_type & "_" & i_number & "' type='button' class='btn btn_" & button_left_type & " btn-" & bootstrap_left_button_type & " btn-sm' onclick='c_submit_multilevel.exe($(this))'>" & button_left_type & "</button>"
                    button_string(1) = ""

                End If

            Case "seized"

                button_left_type = "confirm" : button_right_type = "recall"

                bootstrap_left_button_type = "success" : bootstrap_right_button_type = "danger"

                If buyer = Regex.Replace(GRT.GR.account.public_key, "^0x", "") Then

                    button_string(0) = "<button id='btn_" & button_left_type & "_" & i_number & "' type='button' class='btn btn_" & button_left_type & " btn-" & bootstrap_left_button_type & " btn-sm' onclick='c_submit_multilevel.exe($(this))'>" & button_left_type & "</button>"
                    button_string(1) = "<button id='btn_" & button_right_type & "_" & i_number & "' type='button' class='btn btn_" & button_right_type & " btn-" & bootstrap_right_button_type & " btn-sm' onclick='c_submit_multilevel.exe($(this))'>" & button_right_type & "</button>"

                Else

                    button_string(0) = ""
                    button_string(1) = ""

                End If

        End Select

        Return button_string

    End Function

End Class
