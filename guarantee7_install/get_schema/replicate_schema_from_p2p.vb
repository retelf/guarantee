Public Class replicate_schema_from_p2p
    Public Shared Sub exe()

        Dim Dataset_database_name, Dataset_table_name, Dataset_procedure_name As DataSet
        Dim database_name, table_name, procedure_name, create_database_str, create_table_str, create_procedure_str As String

        ' 데이터베이스 명칭 가져오기

        Dataset_database_name = get_schma.exe("database_name", Nothing, Nothing, Nothing, Nothing, GRT.GR.Connection_mariadb_main)

        ' SHOW CREATE DATABASE 명령 실행

        For i = 0 To Dataset_database_name.Tables(0).Rows.Count - 1

            database_name = CStr(Dataset_database_name.Tables(0).Rows(i)(0))

            If (Regex.Match(database_name, "^bc$").Success Or Regex.Match(database_name, "^bc_").Success) And Not database_name = "bc_nts" Then

                create_database_str = CStr(get_schma.exe("show_create_database", database_name, Nothing, Nothing, Nothing, GRT.GR.Connection_mariadb_main).Tables(0).Rows(0)(1))

                Call get_schma.exe("create_database", database_name, Nothing, Nothing, create_database_str, GRT.GR.Connection_mariadb_local)

                ' CREATE TABLE 

                Dataset_table_name = get_schma.exe("table_name", database_name, Nothing, Nothing, Nothing, GRT.GR.Connection_mariadb_main)

                For j = 0 To Dataset_table_name.Tables(0).Rows.Count - 1

                    table_name = CStr(Dataset_table_name.Tables(0).Rows(j)(0))

                    create_table_str = CStr(get_schma.exe("show_create_table", database_name, table_name, Nothing, Nothing, GRT.GR.Connection_mariadb_main).Tables(0).Rows(0)(1))

                    Call get_schma.exe("create_table", database_name, table_name, Nothing, create_table_str, GRT.GR.Connection_mariadb_local)

                Next

                ' CREATE PROCEDURE 

                Dataset_procedure_name = get_schma.exe("procedure_name", database_name, Nothing, Nothing, Nothing, GRT.GR.Connection_mariadb_main)

                For j = 0 To Dataset_procedure_name.Tables(0).Rows.Count - 1

                    procedure_name = CStr(Dataset_procedure_name.Tables(0).Rows(j)("Name"))

                    create_procedure_str = CStr(get_schma.exe("show_create_procedure", database_name, Nothing, procedure_name, Nothing, GRT.GR.Connection_mariadb_main).Tables(0).Rows(0)("Create Procedure"))

                    Call get_schma.exe("create_procedure", database_name, Nothing, procedure_name, create_procedure_str, GRT.GR.Connection_mariadb_local)

                Next

            End If

        Next

    End Sub

End Class
