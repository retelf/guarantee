Imports Newtonsoft.Json

Public Class GQS_deploy_smart_contract_escaped

    Public Shared Function exe(
                              eoa As String,
                              ca As String,
                              industry_name As String,
                              smart_contract_name As String,
                              code_json_string As String,
                              create_table_string As String,
                              idate_string As String) As String

        Dim pure_query As String
        Dim file_name, escaped_code, extention As String

        Dim code_json = CType(JsonConvert.DeserializeObject(code_json_string), Linq.JObject)

        pure_query = "USE bc_smart_contract; " &
                    "INSERT INTO `contract`(" &
                    "`block_number`," &
                    " `industry`," &
                    " `name`," &
                    " `eoa`," &
                    " `ca`," &
                    " `balance`," &
                    " `state`," &
                    " `idate`)" &
                    "VALUES(" &
                    " 0," &
                    " '" & industry_name & "'," &
                    " '" & smart_contract_name & "'," &
                    " '" & Regex.Replace(eoa, "^0x", "") & "'," &
                    " '" & Regex.Replace(ca, "^0x", "") & "'," &
                    " " & 0 & "," &
                    " 'running'," &
                    " '" & idate_string & "');"

        pure_query &= "USE mysql;"
        pure_query &= "CREATE DATABASE bc_sc_" & smart_contract_name & ";"
        pure_query &= "USE bc_sc_" & smart_contract_name & ";"
        pure_query &=
                    "CREATE TABLE `file` (" &
                    "  `block_number` bigint(20) NOT NULL," &
                    "  `ca` char(40) NOT NULL," &
                    "  `name` varchar(50) NOT NULL," &
                    "  `extention` varchar(50) NOT NULL," &
                    "  `code` text NOT NULL," &
                    "  `code_length` int(11) NOT NULL," &
                    "  `idate` datetime NOT NULL," &
                    "  PRIMARY KEY (`ca`,`name`,`extention`)," &
                    "  UNIQUE KEY `block_number` (`block_number`)" &
                    ") ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;"

        escaped_code = code_json("value")("escaped_code").ToString
        extention = code_json("value")("extention").ToString
        file_name = code_json("value")("file_name").ToString

        pure_query &= "INSERT INTO `file`(" &
                    "`block_number`," &
                    " `ca`," &
                    " `name`," &
                    " `extention`," &
                    " `code`," &
                    " `code_length`," &
                    " `idate`)" &
                    "VALUES(" &
                    " 0," &
                    " '" & Regex.Replace(ca, "^0x", "") & "'," &
                    " '" & file_name & "'," &
                    " '" & extention & "'," &
                    " '" & escaped_code & "'," &
                    " " & escaped_code.Length & "," &
                    " '" & idate_string & "');"

        pure_query &= create_table_string

        Return pure_query

    End Function

End Class
