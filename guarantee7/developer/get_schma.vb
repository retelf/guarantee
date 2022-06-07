Imports MySqlConnector

Public Class get_schma

    Public Shared Function exe(command_type As String, database_name As String, table_name As String, procedure_name As String, create_sentence As String, Connection_mariadb_main_or_local As MySqlConnection) As DataSet

        Dim sqlCommand As MySqlCommand
        Dim Adapter As MySqlDataAdapter
        Dim Dataset As DataSet
        Dim sql As String

        Select Case command_type

            Case "database_name"

                sql = "SHOW DATABASES;"

            Case "show_create_database"

                sql = "SHOW CREATE DATABASE " & database_name & ";"

            Case "table_name"

                sql = "USE " & database_name & "; SHOW TABLES;"

            Case "show_create_table"

                sql = "USE " & database_name & "; SHOW CREATE TABLE " & table_name & ";"

            Case "procedure_name"

                sql = "USE " & database_name & "; SHOW PROCEDURE STATUS WHERE db = '" & database_name & "';"

            Case "show_create_procedure"

                sql = "USE " & database_name & "; SHOW CREATE PROCEDURE " & procedure_name & ";"

            Case "create_database", "create_table", "create_procedure"

                sql = create_sentence

        End Select

        Select Case command_type

            Case "create_database", "create_table", "create_procedure"

                Select Case command_type

                    Case "create_database"

                        sql = "DROP DATABASE IF EXISTS " & database_name & "; " & sql

                    Case "create_table"

                        sql = "USE " & database_name & "; DROP TABLE IF EXISTS " & table_name & "; " & sql

                    Case "create_procedure"

                        sql = "USE " & database_name & "; DROP PROCEDURE IF EXISTS " & procedure_name & "; " & sql

                End Select

                sqlCommand = New MySqlCommand(sql, Connection_mariadb_main_or_local) ' local
                sqlCommand.CommandType = CommandType.Text

                sqlCommand.ExecuteNonQuery()

            Case Else

                sqlCommand = New MySqlCommand(sql, Connection_mariadb_main_or_local) ' main
                sqlCommand.CommandType = CommandType.Text

                Adapter = New MySqlDataAdapter
                Adapter.SelectCommand = sqlCommand
                Dataset = New DataSet
                Adapter.Fill(Dataset)

                Return Dataset

        End Select

    End Function

End Class
