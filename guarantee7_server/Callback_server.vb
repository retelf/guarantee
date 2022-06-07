Imports System.Text.RegularExpressions
Imports System.Windows.Forms

Public Class Callback_server

    Private Delegate Sub SetControlCallback(ByVal object_type As String, ByVal order_type As String, ByVal arg_string As String, ByVal arg_string2 As String)

    Public Shared Function SetControl(ByVal object_type As String, ByVal order_type As String, ByVal arg_string As String, ByVal arg_string2 As String) As String

        Select Case object_type

            Case "txt_monitor"

                If GR.txt_monitor.InvokeRequired Then

                    Dim d As New SetControlCallback(AddressOf SetControl)

                    GR.txt_monitor.Invoke(d, New Object() {object_type, order_type, arg_string, arg_string2})

                Else

                    Select Case order_type

                        Case "write"

                            If GR.txt_monitor.Text.Length <= 10000 Then
                                GR.txt_monitor.Text &= arg_string & vbCrLf
                            Else
                                GR.txt_monitor.Text = Regex.Match(GR.txt_monitor.Text & vbCrLf & arg_string, ".{10000}$", RegexOptions.Singleline).ToString
                            End If

                            GR.txt_monitor.SelectionStart = GR.txt_monitor.TextLength

                            GR.txt_monitor.ScrollToCaret()

                    End Select

                End If

        End Select

    End Function

End Class