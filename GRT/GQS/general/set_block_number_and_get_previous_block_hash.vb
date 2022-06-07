Public Class set_block_number_and_get_previous_block_hash
    Public Structure st_data

        Dim block_number As Integer
        Dim previous_hash As String

    End Structure : Public Shared data As st_data

    Public Shared Sub exe()

        Dim dataset_max_number_block_number_and_hash As DataSet
        Dim new_block_number, count As Integer
        Dim previous_hash As String

        dataset_max_number_block_number_and_hash = GRT.get_bc_main_info.max_number_block_number_and_hash

        count = dataset_max_number_block_number_and_hash.Tables(0).Rows.Count

        If count = 1 Then
            new_block_number = CInt(dataset_max_number_block_number_and_hash.Tables(0).Rows(0)("block_number")) + 1
        Else
            new_block_number = 1
        End If

        If new_block_number = 1 Then
            previous_hash = GRT.GR.genesis_block_hash
        Else
            previous_hash = CStr(dataset_max_number_block_number_and_hash.Tables(0).Rows(0)("block_hash"))
        End If

        data.block_number = new_block_number
        data.previous_hash = previous_hash

    End Sub

End Class
