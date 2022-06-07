Public Class set_database_connection

    Public Shared Sub open_general()

        GRT.GR.Connection_mariadb_bc_nts.Open()

        GRT.GR.Connection_mariadb_main.Open()
        GRT.GR.Connection_mariadb_main_bc.Open()
        GRT.GR.Connection_mariadb_main_bc_manager.Open()
        'GRT.GR.Connection_mariadb_main_bc_multilevel_contract.Open()

        GRT.GR.Connection_mariadb_local.Open()
        GRT.GR.Connection_mariadb_local_bc.Open()
        GRT.GR.Connection_mariadb_local_bc_manager.Open()
        'GRT.GR.Connection_mariadb_local_bc_multilevel_contract.Open()

    End Sub

    Public Shared Sub close_general()

        GRT.GR.Connection_mariadb_bc_nts.Close()

        GRT.GR.Connection_mariadb_main.Close()
        GRT.GR.Connection_mariadb_main_bc.Close()
        GRT.GR.Connection_mariadb_main_bc_manager.Close()
        'GRT.GR.Connection_mariadb_main_bc_multilevel_contract.Close()

        GRT.GR.Connection_mariadb_local.Close()
        GRT.GR.Connection_mariadb_local_bc.Close()
        GRT.GR.Connection_mariadb_local_bc_manager.Close()
        'GRT.GR.Connection_mariadb_local_bc_multilevel_contract.Close()

    End Sub

    Public Shared Sub open_install()

        'GRT.GR.Connection_mariadb_bc_nts.Open()

        GRT.GR.Connection_mariadb_main.Open()
        GRT.GR.Connection_mariadb_main_bc.Open()
        GRT.GR.Connection_mariadb_main_bc_manager.Open()
        'GRT.GR.Connection_mariadb_main_bc_multilevel_contract.Open()

        GRT.GR.Connection_mariadb_local.Open()

    End Sub

    Public Shared Sub close_install()

        'GRT.GR.Connection_mariadb_bc_nts.Close()

        GRT.GR.Connection_mariadb_main.Close()
        GRT.GR.Connection_mariadb_main_bc.Close()
        GRT.GR.Connection_mariadb_main_bc_manager.Close()
        'GRT.GR.Connection_mariadb_main_bc_multilevel_contract.Close()

        GRT.GR.Connection_mariadb_local.Close()

    End Sub

End Class
