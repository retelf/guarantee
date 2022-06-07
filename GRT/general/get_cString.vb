Public Class get_cString

    Public Shared Function exe(database_name As String) As String

        Dim cString As String

        Select Case database_name
            Case "bc_nts"
                cString = GR.cString_mariadb_bc_nts
            Case "bc"
                cString = GR.cString_mariadb_local_bc
            Case "bc_manager"
                cString = GR.cString_mariadb_local_bc_manager
            Case "bc_multilevel"
                cString = GR.cString_mariadb_local_bc_multilevel
            Case "bc_smart_contract"
                cString = GR.cString_mariadb_local_bc_smart_contract
        End Select

        Return cString

    End Function

End Class
