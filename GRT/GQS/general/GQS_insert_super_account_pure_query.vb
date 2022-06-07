Public Class GQS_insert_super_account_pure_query

    Public Shared Function exe(public_key As String, idate_string As String, json As Newtonsoft.Json.Linq.JObject) As String

        Dim pure_query, email, name_english, name_home_language, country, phone_number, identity_number As String

        email = json("value")("email").ToString
        name_english = json("value")("name_english").ToString
        name_home_language = json("value")("name_home_language").ToString
        country = json("value")("country").ToString
        phone_number = json("value")("phone_number").ToString
        identity_number = json("value")("identity_number").ToString

        pure_query = "USE bc_nts; " &
            "INSERT INTO super_account(" &
            " `block_number`," &
            " `public_key`," &
            "`email`," &
            "`name_english`," &
            " `name_home_language`," &
            " `country`," &
            " `phone_number`," &
            " `identity_number`," &
            " `idate`)" &
            "VALUES(" &
            " 0," &
            " '" & Regex.Replace(public_key, "^0x", "") & "'," &
            " '" & email & "'," &
            " '" & name_english & "'," &
            " '" & name_home_language & "'," &
            " '" & country & "'," &
            " '" & phone_number & "'," &
            " '" & identity_number & "'," &
            " '" & idate_string & "');"

        Return pure_query

    End Function

End Class
