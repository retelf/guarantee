Public Class treat_register_node

    Public Shared Async Function exe(json As Newtonsoft.Json.Linq.JObject, JSS As String) As Task(Of String)

        Dim block_number, port, port_nft As Integer
        Dim command_key, pure_query, database_name, table_name, query_type, contract_type As String
        Dim received_block_hash, previous_hash As String

        Dim eoa, na, exchange_name, type, domain, ip, idate_string As String
        Dim registered, verified As Boolean
        Dim signiture_for_register_node As String

        command_key = json("key").ToString

        eoa = json("value")("eoa").ToString

        registered = GRT.check_registered_eoa.exe(eoa, "guarantee")

        If registered Then

            ' 등록

            received_block_hash = json("value")("block_hash").ToString
            na = json("value")("na").ToString
            exchange_name = json("value")("exchange_name").ToString
            type = json("value")("type").ToString
            domain = json("value")("domain").ToString
            ip = json("value")("ip").ToString
            port = CInt(json("value")("port").ToString)
            port_nft = CInt(json("value")("port_nft").ToString)
            idate_string = json("value")("idate_string").ToString
            signiture_for_register_node = json("value")("signiture").ToString

            Call GRT.set_block_number_and_get_previous_block_hash.exe()

            pure_query = GRT.GQS_register_server.exe(eoa, na, exchange_name, type, domain, ip, port, port_nft, idate_string)

            block_number = GRT.set_block_number_and_get_previous_block_hash.data.block_number
            previous_hash = GRT.set_block_number_and_get_previous_block_hash.data.previous_hash
            database_name = "bc_manager"
            table_name = "node"
            query_type = "INSERT"
            contract_type = command_key

            verified = GRT.Security.Gverify.verify(pure_query, signiture_for_register_node, eoa)

            If verified Then

                Return Await self_input_and_relay.exe(command_key, block_number, received_block_hash, previous_hash, eoa, database_name, table_name, query_type, contract_type, pure_query, signiture_for_register_node, JSS)

            Else

                Dim error_message = "보내주신 자료가 변조되었습니다."

                Return "{""key"" : """ & command_key & """, ""success"" : ""fail"", ""value"" : {""reason"": """ & error_message & """} }"

            End If

        Else

            Return "{""key"" : """ & command_key & """, ""success"" : ""fail"", ""value"": {""publick_key"": """ & eoa & """, ""reason"": ""no_account""}}"

        End If

    End Function

End Class
