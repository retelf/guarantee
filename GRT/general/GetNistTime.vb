Imports System.Globalization
Imports System.Net.Cache

Public Class GetNistTime

    Public Shared Function exe() As DateTime

        ' 이는 자체적인 메인 시간 서버를 만들어서 그것을 써야 한다. 인 그러면 Nist 에서 부담이 엄청나진다.
        ' 미리 한 번 받아서 그것을 모든 서버에 나눠 주어야 하는 것이다.
        ' 만약 고장이 나면 그 때 다음을 사용한다.

        Dim utcDateTimeString As String

        Dim utc_time As Date

        Try

            Dim client = New TcpClient("time.nist.gov", 13)

            Using streamReader = New StreamReader(client.GetStream())

                Dim response = streamReader.ReadToEnd()
                utcDateTimeString = response.Substring(7, 17)
                'localDateTime = DateTime.ParseExact(utcDateTimeString, "yy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal)
                utc_time = DateTime.Parse(utcDateTimeString)

            End Using

        Catch ex As Exception

            Dim myHttpWebRequest = CType(WebRequest.Create("http://www.microsoft.com"), HttpWebRequest)
            Dim response = myHttpWebRequest.GetResponse()
            Dim todaysDates As String = response.Headers("date")
            utc_time = DateTime.ParseExact(todaysDates, "ddd, dd MMM yyyy HH:mm:ss 'GMT'", CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.AssumeUniversal)

        End Try

        Return utc_time

    End Function

End Class
