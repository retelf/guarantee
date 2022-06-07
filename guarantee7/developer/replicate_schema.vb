Imports MySqlConnector

Public Class replicate_schema
    Public Shared Sub exe(replicate_type As String, Connection_target_for_get As MySqlConnection, Connection_local_for_save As MySqlConnection)

        Dim Dataset_database_name As DataSet
        Dim database_name, server_type As String

        If Regex.Match(Connection_local_for_save.ConnectionString, "server=192.168.0.251;port=3306;", RegexOptions.IgnoreCase).Success Then
            server_type = "nts"
        Else
            server_type = "general"
        End If

        ' 데이터베이스 명칭 가져오기

        Dataset_database_name = get_schma.exe("database_name", Nothing, Nothing, Nothing, Nothing, Connection_target_for_get)

        ' SHOW CREATE DATABASE 명령 실행

        For i = 0 To Dataset_database_name.Tables(0).Rows.Count - 1

            database_name = CStr(Dataset_database_name.Tables(0).Rows(i)(0))

            If server_type = "nts" Then

                If (Regex.Match(database_name, "^bc_nts$").Success) Then

                    Call exe_sub(replicate_type, database_name, Connection_target_for_get, Connection_local_for_save)

                End If

            Else

                If (Regex.Match(database_name, "^bc$").Success Or Regex.Match(database_name, "^bc_").Success) Then

                    Call exe_sub(replicate_type, database_name, Connection_target_for_get, Connection_local_for_save)

                End If

            End If

        Next

    End Sub

    Shared Sub exe_sub(replicate_type As String, database_name As String, Connection_target_for_get As MySqlConnection, Connection_local_for_save As MySqlConnection)

        Dim Dataset_table_name, Dataset_procedure_name As DataSet
        Dim table_name, procedure_name, create_database_str, create_table_str, create_procedure_str As String

        If replicate_type = "all" Then

            create_database_str = CStr(get_schma.exe("show_create_database", database_name, Nothing, Nothing, Nothing, Connection_target_for_get).Tables(0).Rows(0)(1))

            Call get_schma.exe("create_database", database_name, Nothing, Nothing, create_database_str, Connection_local_for_save)

            ' CREATE TABLE 

            Dataset_table_name = get_schma.exe("table_name", database_name, Nothing, Nothing, Nothing, Connection_target_for_get)

            For j = 0 To Dataset_table_name.Tables(0).Rows.Count - 1

                table_name = CStr(Dataset_table_name.Tables(0).Rows(j)(0))

                create_table_str = CStr(get_schma.exe("show_create_table", database_name, table_name, Nothing, Nothing, Connection_target_for_get).Tables(0).Rows(0)(1))

                Call get_schma.exe("create_table", database_name, table_name, Nothing, create_table_str, Connection_local_for_save)

            Next

        End If

        ' CREATE PROCEDURE 

        Dataset_procedure_name = get_schma.exe("procedure_name", database_name, Nothing, Nothing, Nothing, Connection_target_for_get)

        For j = 0 To Dataset_procedure_name.Tables(0).Rows.Count - 1

            procedure_name = CStr(Dataset_procedure_name.Tables(0).Rows(j)("Name"))

            create_procedure_str = CStr(get_schma.exe("show_create_procedure", database_name, Nothing, procedure_name, Nothing, Connection_target_for_get).Tables(0).Rows(0)("Create Procedure"))

            Call get_schma.exe("create_procedure", database_name, Nothing, procedure_name, create_procedure_str, Connection_local_for_save)

        Next

    End Sub

End Class
