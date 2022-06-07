Public Class get_treated_query_main

    Public Shared Function exe(command_key As String, block_number As Long, database_name() As String, table_name() As String, pure_query As String, query_type As String, Optional json As Newtonsoft.Json.Linq.JObject = Nothing) As String

        Dim treated_query As String

        Select Case command_key

            Case "submit_load_order"

                treated_query =
                    pure_query &
                    "USE bc;" &
                    "UPDATE exchange SET `block_number` = " & block_number & " WHERE block_number = 0;"

            Case "submit_sell_order"

                treated_query = pure_query & "USE bc_multilevel;" & "UPDATE sell_order SET `block_number` = " & block_number & " WHERE block_number = 0;"

            Case "register_node" : treated_query = get_treated_query_register_node.exe(block_number, pure_query)

            Case "submit_exchange" : treated_query = get_treated_query_submit_exchange.exe(block_number, pure_query)

            Case "submit_buy" : treated_query = get_treated_query_submit_buy.exe(block_number, pure_query)

            Case "deploy_smart_contract"

                treated_query = pure_query & "USE bc_smart_contract; UPDATE " & table_name(0) & " SET `block_number` = " & block_number & " WHERE block_number = 0;"

                Dim sc_database_name As String = Regex.Match(pure_query, "(?<=CREATE DATABASE\s+)bc_sc_.+?(?=;)").ToString

                treated_query &= "USE " & sc_database_name & ";" & "UPDATE `file` SET `block_number` = " & block_number & " WHERE block_number = 0;"

            Case "submit_load_nft" : treated_query = get_treated_query_submit_load_nft.exe(block_number, pure_query, CBool(json("value")("sell_order_right_now")))

            Case "submit_nft_load_order"

                treated_query = pure_query & "USE bc_nft;" & "UPDATE sell_order SET `block_number` = " & block_number & " WHERE block_number = 0;"

            Case "submit_nft_buy"

                treated_query = pure_query & "USE bc_nft;" & "UPDATE owner_portion SET `block_number` = " & block_number & " WHERE block_number = 0;"

            Case "submit_nft_confirm"

                treated_query = pure_query & "USE bc;" & "UPDATE deposit SET `block_number` = " & block_number & " WHERE block_number = 0;"
                treated_query = treated_query & "USE bc;" & "UPDATE clearing_house SET `block_number` = " & block_number & " WHERE block_number = 0;"
                treated_query = treated_query & "USE bc_manager;" & "UPDATE node_accident SET `block_number` = " & block_number & " WHERE block_number = 0;"

            Case Else

                Select Case query_type

                    Case "INSERT"

                        treated_query = pure_query

                        For i = 0 To database_name.Length - 1

                            treated_query &= "USE " & database_name(i) & ";" & "UPDATE " & table_name(i) & " SET `block_number` = " & block_number & " WHERE block_number = 0;"

                        Next

                    Case Else

                        treated_query = pure_query

                End Select

        End Select

        Return treated_query

    End Function

End Class
