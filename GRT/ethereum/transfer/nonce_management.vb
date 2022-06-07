Imports System.Numerics
Imports Nethereum.Hex.HexTypes
Imports Nethereum.RPC.NonceServices

Public Class nonce_management

    Public Shared Async Function exe_trom_wallet(json As Newtonsoft.Json.Linq.JObject) As Task(Of String)

        Dim command_key As String = json("key").ToString
        Dim public_key As String = json("value")("public_key").ToString

        Dim futureNonce As HexBigInteger = Await exe(public_key)

        Dim JRS = GRT.make_json_string.exe({{"key", command_key, "quot"}, {"success", "success", "quot"}}, {{"futureNonce_HexValue", futureNonce.HexValue, "quot"}}, False)

        Return JRS

    End Function

    Public Shared Async Function exe(public_key As String) As Task(Of HexBigInteger)

        Dim Selectcmd As MySqlCommand
        Dim Adapter As MySqlDataAdapter
        Dim Dataset As DataSet
        Dim futureNonce, geth_futureNonce As HexBigInteger
        Dim server_Nonce As Long
        Dim future_nonce_value As Long

        Dim NonceService = New InMemoryNonceService(public_key, GRT.GR.ethereum.web3.Client)

        geth_futureNonce = Await NonceService.GetNextNonceAsync()

        Dim Connection_mariadb_local_bc_manager As New MySqlConnection(GRT.GR.cString_mariadb_local_bc_manager)

        Connection_mariadb_local_bc_manager.Open()

        'CREATE PROCEDURE `up_get_server_nonce`
        '(IN p_public_key char(64))
        'BEGIN
        'SELECT state, transaction_hash, last_requested, last_received, last_input, last_confirmed, last_canceled
        'FROM ethereum_nonce
        'WHERE
        '`public_key` = p_public_key;
        'END

        Selectcmd = New MySqlCommand("up_get_server_nonce", Connection_mariadb_local_bc_manager)
        Selectcmd.Parameters.Add(New MySqlParameter("p_public_key", Regex.Replace(public_key, "^0x", "")))

        Adapter = New MySqlDataAdapter
        Selectcmd.CommandType = CommandType.StoredProcedure
        Adapter.SelectCommand = Selectcmd
        Dataset = New DataSet
        Adapter.Fill(Dataset)

        If Dataset.Tables(0).Rows.Count = 1 Then

            '\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\ 이곳에서 다양한 변형을 만들 수 있다.

            server_Nonce = CLng(Dataset.Tables(0).Rows(0)("last_input")) ' 클릭착오 방지, 에러발생시 논스 증가 무시를 위해.

            'If geth_futureNonce.Value = 0 Then ' 싱크로 이전인 경우이다. 이 처리도 필요하다.

            'End If

            If CLng(geth_futureNonce.Value) = server_Nonce + 1 Then ' 이 경우는 확실한 것이다.

                future_nonce_value = CLng(geth_futureNonce.Value)

            Else

                If CLng(geth_futureNonce.Value) > server_Nonce + 1 Then

                    ' 게스 논스가 서버논스보다 큰 경우
                    '   당연히 게스논스를 따른다.

                    future_nonce_value = CLng(geth_futureNonce.Value)

                Else

                    ' 게스 논스가 서버논스보다 작은 경우

                    ' 여러번 동시적으로 누른 경우 - 이 경우는 서버논스를 따른다. 그런데 last_input 이 되기 전에 새로 누른 경우 동일한 논스가 발급되는 현상이 발생하게 된다.

                    ' 가장 문제되는 경우는 거래소 계정으로부터 자금이 인출되는 경우이다. 동시에 여러 곳으로부터 이러한 요청이 들어올 수 있다. 이 경우를 집중적으로 검토해야 한다.

                    ' last_requested, last_received 와 last_input 이 다른 경우가 문제된다. 이런 경우가 발생하지 않도록 해 주면 될 듯, 더 이상은 불가능한 것으로 보인다.

                    future_nonce_value = server_Nonce + 1 ' 일단은 임시적으로

                End If

            End If

            futureNonce = New HexBigInteger(future_nonce_value)

            '\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

            Call exe_update(public_key, "last_requested", futureNonce, Nothing)

        Else

            futureNonce = geth_futureNonce

            Call exe_insert(public_key, geth_futureNonce)

        End If

        Return futureNonce

    End Function

    Shared Sub exe_insert(public_key As String, nonce As HexBigInteger)

        Dim Connection_mariadb_local_bc_manager As New MySqlConnection(GRT.GR.cString_mariadb_local_bc_manager)

        Connection_mariadb_local_bc_manager.Open()

        Dim Insertcmd As MySqlCommand

        'CREATE PROCEDURE up_insert_server_nonce
        '(
        'IN p_public_key char(40),
        'IN p_transaction_hash char(64),
        'IN p_state varchar(800),
        'IN p_last_requested bigint(20),
        'IN p_last_received bigint(20),
        'IN p_last_input bigint(20),
        'IN p_last_confirmed bigint(20),
        'IN p_last_canceled bigint(20),
        'IN p_idate datetime
        ')
        'BEGIN
        'INSERT INTO ethereum_nonce
        '(
        '`public_key`,
        '`transaction_hash`,
        '`state`,
        '`last_requested`,
        '`last_received`,
        '`last_input`,
        '`last_confirmed`,
        '`last_canceled`,
        '`idate`
        ')
        'VALUES
        '(
        'p_public_key,
        'p_transaction_hash,
        'p_state,
        'p_last_requested,
        'p_last_received,
        'p_last_input,
        'p_last_confirmed,
        'p_last_canceled,
        'p_idate
        ');
        'End

        Insertcmd = New MySqlCommand("up_insert_server_nonce", Connection_mariadb_local_bc_manager)
        Insertcmd.CommandType = CommandType.StoredProcedure

        Insertcmd.Parameters.Add(New MySqlParameter("p_public_key", Regex.Replace(public_key, "^0x", "")))
        Insertcmd.Parameters.Add(New MySqlParameter("p_transaction_hash", ""))
        Insertcmd.Parameters.Add(New MySqlParameter("p_state", ""))
        Insertcmd.Parameters.Add(New MySqlParameter("p_last_requested", CLng(nonce.Value)))
        Insertcmd.Parameters.Add(New MySqlParameter("p_last_received", -1))
        Insertcmd.Parameters.Add(New MySqlParameter("p_last_input", -1))
        Insertcmd.Parameters.Add(New MySqlParameter("p_last_confirmed", -1))
        Insertcmd.Parameters.Add(New MySqlParameter("p_last_canceled", -1))
        Insertcmd.Parameters.Add(New MySqlParameter("p_idate", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")))

        Insertcmd.ExecuteNonQuery()

        Connection_mariadb_local_bc_manager.Close()

    End Sub

    Shared Sub exe_update(public_key As String, last_type As String, nonce As HexBigInteger, transaction_hash As String)

        Dim Updatecmd As MySqlCommand

        Dim Connection_mariadb_local_bc_manager As New MySqlConnection(GRT.GR.cString_mariadb_local_bc_manager)

        Connection_mariadb_local_bc_manager.Open()

        If Not transaction_hash Is Nothing Then

            'CREATE PROCEDURE up_update_server_nonce_last_requested
            '(
            'IN p_public_key char(40),
            'IN p_nonce bigint(20),
            'IN p_transaction_hash char(64)
            ')
            'BEGIN
            'UPDATE ethereum_nonce
            'SET
            '`last_requested` = GREATEST(p_nonce, `last_requested`),
            '`transaction_hash` = p_transaction_hash
            'WHERE `public_key` = p_public_key;
            'End

            'CREATE PROCEDURE up_update_server_nonce_last_received
            '(
            'IN p_public_key char(40),
            'IN p_nonce bigint(20),
            'IN p_transaction_hash char(64)
            ')
            'BEGIN
            'UPDATE ethereum_nonce
            'SET
            '`last_received` = GREATEST(p_nonce, `last_received`),
            '`transaction_hash` = p_transaction_hash
            'WHERE `public_key` = p_public_key;
            'End

            'CREATE PROCEDURE up_update_server_nonce_last_input
            '(
            'IN p_public_key char(40),
            'IN p_nonce bigint(20),
            'IN p_transaction_hash char(64)
            ')
            'BEGIN
            'UPDATE ethereum_nonce
            'SET
            '`last_input` = GREATEST(p_nonce, `last_input`),
            '`transaction_hash` = p_transaction_hash
            'WHERE `public_key` = p_public_key;
            'End

            'CREATE PROCEDURE up_update_server_nonce_last_confirmed
            '(
            'IN p_public_key char(40),
            'IN p_nonce bigint(20),
            'IN p_transaction_hash char(64)
            ')
            'BEGIN
            'UPDATE ethereum_nonce
            'SET
            '`last_confirmed` = GREATEST(p_nonce, `last_confirmed`),
            '`transaction_hash` = p_transaction_hash
            'WHERE `public_key` = p_public_key;
            'End

            'CREATE PROCEDURE up_update_server_nonce_last_canceled
            '(
            'IN p_public_key char(40),
            'IN p_nonce bigint(20),
            'IN p_transaction_hash char(64)
            ')
            'BEGIN
            'UPDATE ethereum_nonce
            'SET
            '`last_canceled` = GREATEST(p_nonce, `last_canceled`),
            '`transaction_hash` = p_transaction_hash
            'WHERE `public_key` = p_public_key;
            'End

            Updatecmd = New MySqlCommand("up_update_server_nonce_" & last_type, Connection_mariadb_local_bc_manager)

            Updatecmd.Parameters.Add(New MySqlParameter("p_transaction_hash", Regex.Replace(transaction_hash, "^0x", "")))

        Else

            'CREATE PROCEDURE up_update_server_nonce_last_requested_only_nonce
            '(
            'IN p_public_key char(40),
            'IN p_nonce bigint(20)
            ')
            'BEGIN
            'UPDATE ethereum_nonce
            'SET
            '`last_requested` = GREATEST(p_nonce, `last_requested`)
            'WHERE `public_key` = p_public_key;
            'End

            'CREATE PROCEDURE up_update_server_nonce_last_received_only_nonce
            '(
            'IN p_public_key char(40),
            'IN p_nonce bigint(20)
            ')
            'BEGIN
            'UPDATE ethereum_nonce
            'SET
            '`last_received` = GREATEST(p_nonce, `last_received`)
            'WHERE `public_key` = p_public_key;
            'End

            'CREATE PROCEDURE up_update_server_nonce_last_input_only_nonce
            '(
            'IN p_public_key char(40),
            'IN p_nonce bigint(20)
            ')
            'BEGIN
            'UPDATE ethereum_nonce
            'SET
            '`last_input` = GREATEST(p_nonce, `last_input`)
            'WHERE `public_key` = p_public_key;
            'End

            'CREATE PROCEDURE up_update_server_nonce_last_confirmed_only_nonce
            '(
            'IN p_public_key char(40),
            'IN p_nonce bigint(20)
            ')
            'BEGIN
            'UPDATE ethereum_nonce
            'SET
            '`last_confirmed` = GREATEST(p_nonce, ``)
            'WHERE `public_key` = p_public_key;
            'End

            'CREATE PROCEDURE up_update_server_nonce_last_canceled_only_nonce
            '(
            'IN p_public_key char(40),
            'IN p_nonce bigint(20)
            ')
            'BEGIN
            'UPDATE ethereum_nonce
            'SET
            '`last_canceled` = GREATEST(p_nonce, `last_canceled`)
            'WHERE `public_key` = p_public_key;
            'End

            Updatecmd = New MySqlCommand("up_update_server_nonce_" & last_type & "_only_nonce", Connection_mariadb_local_bc_manager)

        End If

        Updatecmd.CommandType = CommandType.StoredProcedure
        Updatecmd.Parameters.Add(New MySqlParameter("p_public_key", Regex.Replace(public_key, "^0x", "")))
        Updatecmd.Parameters.Add(New MySqlParameter("p_nonce", CLng(nonce.Value)))

        Updatecmd.ExecuteNonQuery()

        Connection_mariadb_local_bc_manager.Close()

    End Sub

End Class
