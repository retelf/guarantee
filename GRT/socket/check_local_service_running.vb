Imports System.ServiceProcess

Public Class check_local_service_running

    Public Shared Function exe(instance_name As String) As Boolean

        Dim service = New ServiceController(instance_name)

        If service.Status.Equals(ServiceControllerStatus.Running) Then
            Return True
        Else
            Return False
        End If

    End Function

End Class
