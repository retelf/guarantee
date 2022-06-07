Imports MySqlConnector

Public Class make_sample_database

    Public Shared Sub exe(Connection_sample_database As MySqlConnection, cString_main As String, cString_nts As String)

        Dim Connection_main = New MySqlConnection(cString_main)
        Dim Connection_nts = New MySqlConnection(cString_nts)

        Connection_main.Open()
        Connection_nts.Open()

        ' ntf schema 복제

        Call replicate_schema.exe("all", Connection_nts, Connection_sample_database)

        ' main schema 복제

        Call replicate_schema.exe("all", Connection_main, Connection_sample_database)

        ' main의 node 데이터 가져오기

        Call insert_sample_data.node(Connection_sample_database, Connection_main)

        Connection_main.Close()
        Connection_nts.Close()

    End Sub

End Class
