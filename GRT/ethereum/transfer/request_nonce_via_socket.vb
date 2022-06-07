Imports System.Numerics
Imports Nethereum.Hex.HexTypes
Imports Nethereum.RPC.NonceServices
Imports Newtonsoft.Json

Public Class request_nonce_via_socket

    Public Shared Async Function exe(public_key As String) As Task(Of HexBigInteger)

        Dim futureNonce, geth_futureNonce As HexBigInteger
        Dim JSS, JRS As String

        Dim NonceService = New InMemoryNonceService(public_key, GRT.GR.ethereum.web3.Client)

        geth_futureNonce = Await NonceService.GetNextNonceAsync()

        JSS = GRT.make_json_string.exe(
                                {{"key", "nonce_request", "quot"}},
                                {
                                {"public_key", public_key, "quot"}}, False)

        JRS = Await GRT.socket_client.exe(GRT.GR.account.agency.ip_agency, GRT.GR.account.agency.port_agency, GRT.GR.port_number_server_local, JSS)

        Dim json_JRS As Newtonsoft.Json.Linq.JObject = CType(JsonConvert.DeserializeObject(JRS), Linq.JObject)

        futureNonce = New HexBigInteger(json_JRS("value")("futureNonce_HexValue").ToString)

        Return futureNonce

    End Function

End Class
