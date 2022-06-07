Imports System.Text.RegularExpressions
Imports System.Windows.Forms

Public Class Callback

    Private Delegate Sub SetControlCallback(ByVal object_type As String, ByVal order_type As String, ByVal arg_string As String, ByVal arg_string2 As String)

    Public Shared Function SetControl(ByVal object_type As String, ByVal order_type As String, ByVal arg_string As String, ByVal arg_string2 As String) As String

        Select Case object_type

            Case "lbl_balance"

                If GR.control.lbl_message.InvokeRequired Then

                    Dim d As New SetControlCallback(AddressOf SetControl)

                    GR.control.lbl_message.Invoke(d, New Object() {object_type, order_type, arg_string, arg_string2})

                Else

                    Select Case order_type

                        Case "write"

                            GR.control.lbl_message.Text &= arg_string

                    End Select

                End If

        End Select

    End Function

End Class
