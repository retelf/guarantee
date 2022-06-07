Imports Newtonsoft.Json
Public Class set_additional_data_for_mobile_authentication
    Public Structure st_data

        Dim block_number As Integer
        Dim previous_hash As String
        Dim database_name As String
        Dim table_name As String
        Dim query_type As String
        Dim contract_type As String
        Dim algorithm As String
        Dim idate As Date

    End Structure : Public Shared data As st_data

    Public Shared Sub exe(database_name As String, table_name As String, query_type As String, contract_type As String)

        Dim dataset_max_number_block_info As DataSet
        Dim new_block_number, count As Integer
        Dim algorithm As String
        Dim previous_hash As String
        Dim idate As Date

        dataset_max_number_block_info = GRT.get_nts_main_info.max_number_block_info

        count = dataset_max_number_block_info.Tables(0).Rows.Count

        If count = 1 Then
            new_block_number = CInt(dataset_max_number_block_info.Tables(0).Rows(0)("block_number")) + 1
        Else
            new_block_number = 1
        End If

        If new_block_number = 1 Then
            previous_hash = GRT.GR.genesis_block_hash
        Else
            previous_hash = CStr(dataset_max_number_block_info.Tables(0).Rows(0)("block_hash"))
        End If

        algorithm = GRT.GR.algorithm
        idate = DateTime.Now

        data.block_number = new_block_number
        data.previous_hash = previous_hash
        data.database_name = database_name
        data.table_name = table_name
        data.query_type = query_type
        data.contract_type = contract_type
        data.algorithm = algorithm
        data.idate = idate

    End Sub

End Class
