Imports System
Imports System.Net
Imports System.Net.Sockets
Imports System.ServiceProcess
Imports System.Text
Imports System.Threading

Public Class guarantee_server_async_thread_management

    Public Shared Async Sub exe()

        Callback_server.SetControl("txt_monitor", "write", vbCrLf & "Waiting for a management connection...", "")

        While True

            Call manage_confirm_nft.exe()

            Thread.Sleep(GRT.GR.management.confirm_time.general)

        End While

    End Sub

End Class
