Public Class set_database_connection

    Public Shared Sub open()

        GRT.GR.Connection_mariadb_local.Open()
        GRT.GR.Connection_mariadb_local_bc.Open()
        GRT.GR.Connection_mariadb_local_bc_manager.Open()
        GRT.GR.Connection_mariadb_local_bc_multilevel_contract.Open()
        GRT.GR.Connection_mariadb_remote.Open()
        GRT.GR.Connection_mariadb_remote_bc.Open()
        GRT.GR.Connection_mariadb_remote_bc_manager.Open()
        GRT.GR.Connection_mariadb_remote_bc_multilevel_contract.Open()

    End Sub

    Public Shared Sub close()

        GRT.GR.Connection_mariadb_local.Close()
        GRT.GR.Connection_mariadb_local_bc.Close()
        GRT.GR.Connection_mariadb_local_bc_manager.Close()
        GRT.GR.Connection_mariadb_local_bc_multilevel_contract.Close()
        GRT.GR.Connection_mariadb_remote.Close()
        GRT.GR.Connection_mariadb_remote_bc.Close()
        GRT.GR.Connection_mariadb_remote_bc_manager.Close()
        GRT.GR.Connection_mariadb_remote_bc_multilevel_contract.Close()

    End Sub

    Public Shared Sub open_on_install()

        GRT.GR.Connection_mariadb_local.Open()

    End Sub

    Public Shared Sub close_on_install()

        GRT.GR.Connection_mariadb_local.Close()

    End Sub

End Class
