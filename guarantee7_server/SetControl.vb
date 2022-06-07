Public Class SetControl

    Public Shared Sub exe(json As Newtonsoft.Json.Linq.JObject)

        Select Case json("key").ToString

            Case "mobile_certification_number_send"

                Callback_server.SetControl(
                    "txt_monitor", "write", "returned : """ &
                    json("key").ToString &
                    " : " & json("success").ToString &
                    " : " & json("hash").ToString, """ command")

            Case "membership_result", "submit_load_nft"

                Callback_server.SetControl(
                    "txt_monitor", "write", "returned : """ &
                    json("key").ToString &
                    " : " & json("success").ToString, """ command")

            Case "relay_complete"

                Callback_server.SetControl(
                    "txt_monitor", "write", "returned : """ &
                    json("key").ToString &
                    " : " & json("success").ToString &
                    " : " & json("receipt").ToString, """ command")

        End Select

    End Sub

End Class
