Imports System.ServiceProcess
Imports System.Windows.Forms

Public Class check_initial_install_case

    Public Shared Function exe() As String

        Dim initial_install_case As String

        ' 데이터베이스가 없거나 있더라도 초기화 전일 때, 즉 bc 데이터베이스가 없을 때는 설치시다.

        Try

            initial_install_case = "bc_database_exists"

            GR.Connection_mariadb_local.Open()

            Dim sqlCommand As MySqlCommand
            Dim Adapter As MySqlDataAdapter
            Dim Dataset As DataSet
            Dim database_name As String

            sqlCommand = New MySqlCommand("SHOW DATABASES;", GR.Connection_mariadb_local)
            sqlCommand.CommandType = CommandType.Text

            Adapter = New MySqlDataAdapter
            Adapter.SelectCommand = sqlCommand
            Dataset = New DataSet
            Adapter.Fill(Dataset)

            GR.Connection_mariadb_local.Close()

            For i = 0 To Dataset.Tables(0).Rows.Count - 1

                database_name = CStr(Dataset.Tables(0).Rows(i)(0))

                If database_name = "bc" Then

                    initial_install_case = "installed_but_no_bc_database"

                    Exit For

                End If

            Next

        Catch ex As Exception

            initial_install_case = "no_instance"

        End Try

        Return initial_install_case

    End Function

End Class
