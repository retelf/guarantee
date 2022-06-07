Imports System.IO
Imports System.Net
Imports System.Text.RegularExpressions

Public Class ftp_upload

    Shared _destination_file_short_name As String

    Public Shared Sub check_folder(
                                  domain As String,
                                  port As Integer,
                                  destination_folder_full_name As String,
                                  destination_file_short_name As String,
                                  original_file_full_Name As String,
                                  user_name As String,
                                  password As String,
                                  folder_ok As Boolean,
                                  file_ok As Boolean)

        _destination_file_short_name = destination_file_short_name

        ' folder_ok 와 file_ok 는 외부에서 면밀히 준비해야 한다.

        Dim request As FtpWebRequest
        ' 처음부터 폴더를 체크하면서 만들어 나간다.

        Dim folder() As String = Regex.Split(destination_folder_full_name, "\\")

        Dim current_folder As String = ""
        Dim tem_folder As String = ""

        For i = 0 To folder.Length - 1

            tem_folder &= folder(i) & "\"

            current_folder = tem_folder.TrimEnd(CChar("\"))

            request = CType(WebRequest.Create(New Uri(String.Format("ftp://{0}:{1}/{2}/", domain, CStr(port), current_folder))), FtpWebRequest)

            request.Method = WebRequestMethods.Ftp.MakeDirectory

            request.Credentials = New NetworkCredential(user_name, password)
            request.UseBinary = True
            request.UsePassive = False
            request.KeepAlive = False

            Dim response As FtpWebResponse

            Try

                response = CType(request.GetResponse(), FtpWebResponse)

                If i = folder.Length - 1 Then

                    ' 최종 폴더가 지금 만들어졌으므로 곧바로 파일 전송

                    Call file_finally(domain, current_folder, original_file_full_Name, user_name, password)

                    response.Close()

                    Exit For

                Else
                    ' 그 다음 폴더를 만들어야 하므로 다시 루프회전
                End If

            Catch ex As WebException

                response = CType(ex.Response, FtpWebResponse)

                If response.StatusCode = FtpStatusCode.ActionNotTakenFileUnavailable Then

                    If i = folder.Length - 1 Then

                        ' 최종폴더 존재. 파일 존재 확인 단계로 넘어감

                        Call check_file(domain, current_folder, original_file_full_Name, user_name, password)

                        Exit For

                    Else

                        ' 아직 하위 폴더를 더 체크해야 하므로 루프회전

                    End If

                Else ' 의외의 에러임

                    Try
                        Err.Raise(513 + vbObjectError)
                    Catch ex_1 As Exception
                        Err.Raise(513 + vbObjectError, , ex.Message & vbCrLf & " ex.StackTrace 요약 : " & Regex.Match(ex.StackTrace, "[\dA-z_]+\.vb:줄 [\d]+").ToString)
                    End Try

                End If

                response.Close()

            End Try

        Next

    End Sub

    Public Shared Sub file_finally(domain As String, destination_folder_full_name As String, original_file_full_Name As String, user_name As String, password As String)

        ' 만약 파일이 존재하는 경우 이 코드는 그대로 덮어쓰기가 된다.

        Dim request As FtpWebRequest
        Dim requestStream As Stream
        'Dim destination_folder_full_name As String = "mnt"
        'Dim fileName As String = "\\WIN-CJ8KUTRMN2J\auction_court_base\files\2018\남원\1\0\958\court_image\남원1-0-2018-958-0-10.jpg"
        'Dim absoluteFileName As String = Path.GetFileName(original_file_full_Name)

        request = CType(WebRequest.Create(New Uri(String.Format("ftp://{0}/{1}/{2}", domain, destination_folder_full_name, _destination_file_short_name))), FtpWebRequest)
        request.Method = WebRequestMethods.Ftp.UploadFile

        request.UseBinary = True
        request.UsePassive = False
        request.KeepAlive = False
        request.Credentials = New NetworkCredential(user_name, password)
        'request.ConnectionGroupName = "group"

        Dim fs As FileStream

        fs = File.OpenRead(original_file_full_Name)

        Dim buffer As Byte() = New Byte(CInt(fs.Length - 1)) {}
        fs.Read(buffer, 0, buffer.Length)
        fs.Close()

        requestStream = request.GetRequestStream()
        requestStream.Write(buffer, 0, buffer.Length)
        requestStream.Flush()
        requestStream.Close()

    End Sub

    Public Shared Sub check_file(domain As String, destination_folder_full_name As String, original_file_full_Name As String, user_name As String, password As String)

        Dim request As FtpWebRequest
        Dim absoluteFileName As String = Path.GetFileName(original_file_full_Name)

        request = CType(WebRequest.Create(New Uri(String.Format("ftp://{0}/{1}/{2}", domain, destination_folder_full_name, absoluteFileName))), FtpWebRequest)
        request.Method = WebRequestMethods.Ftp.GetFileSize

        request.UseBinary = True
        request.UsePassive = False
        request.KeepAlive = False
        request.Credentials = New NetworkCredential(user_name, password)
        'request.ConnectionGroupName = "group"

        Dim response As FtpWebResponse

        Try

            response = CType(request.GetResponse(), FtpWebResponse)

            'Call file_finally(domain, destination_folder_full_name, original_file_full_Name, user_name, password)

            ' 파일이 존재하므로 할 일이 없음.

        Catch ex As WebException

            response = CType(ex.Response, FtpWebResponse)

            If response.StatusCode = FtpStatusCode.ActionNotTakenFileUnavailable Then ' 파일 부존재

                Call file_finally(domain, destination_folder_full_name, original_file_full_Name, user_name, password)

            Else ' 의외의 에러임

                Try
                    Err.Raise(513 + vbObjectError)
                Catch ex_1 As Exception
                    Err.Raise(513 + vbObjectError, , ex.Message & vbCrLf & " ex.StackTrace 요약 : " & Regex.Match(ex.StackTrace, "[\dA-z_]+\.vb:줄 [\d]+").ToString)
                End Try

            End If

            response.Close()

        End Try

    End Sub

    Public Shared Function folder_exist(
                                  domain As String,
                                  destination_folder_full_name As String,
                                  user_name As String,
                                  password As String) As Boolean

        ' 이 과정에서 폴더가 만들어진다. 따라서 직후에 반드시 사용할 것이 아니라면 테스트를 삼가야 한다.

        Dim request As FtpWebRequest

        Dim folder() As String = Regex.Split(destination_folder_full_name, "\\")

        Dim current_folder As String = ""
        Dim tem_folder As String = ""

        For i = 0 To folder.Length - 1

            tem_folder &= folder(i) & "\"

            current_folder = tem_folder.TrimEnd(CChar("\"))

            request = CType(WebRequest.Create(New Uri(String.Format("ftp://{0}/{1}/", domain, current_folder))), FtpWebRequest)

            request.Credentials = New NetworkCredential(user_name, password)
            request.UseBinary = True
            request.UsePassive = False
            request.KeepAlive = False

            request.Method = WebRequestMethods.Ftp.MakeDirectory

            Dim response As FtpWebResponse

            Try

                response = CType(request.GetResponse(), FtpWebResponse)

                If Not i = folder.Length - 1 Then

                    ' 최종 폴더가 존재하지 않으며 최종폴더 이전의 테스트폴더가 만들어졌으니 이를 삭제해야 한다. 
                    ' 기존의 직전 폴더까지는 이곳으로 오지 않으므로 지울 필요는 없으며 바로 이 폴더만 지원주면 된다.

                    Call ftp_upload.remove_directory(domain, current_folder, user_name, password)

                    response.Close()

                    Return False

                Else

                    ' 최종 폴더가 지금 만들어졌다. 어쨌든 원칙적으로 삭제해야 한다.
                    ' 만약 남기고 싶다면 별도의 함수를 만들고 이를 호출하는 것이 안전하다.

                    Call ftp_upload.remove_directory(domain, current_folder, user_name, password)

                    response.Close()

                    Return False

                End If

            Catch ex As WebException

                response = CType(ex.Response, FtpWebResponse)

                If response.StatusCode = FtpStatusCode.ActionNotTakenFileUnavailable Then

                    If i = folder.Length - 1 Then

                        ' 최종폴더가 이미 존재하고 있었다.

                        response.Close()

                        Return True

                    Else

                        ' 아직 하위 폴더를 더 체크해야 하므로 루프회전

                        response.Close()

                    End If

                Else ' 의외의 에러임

                    Try
                        Err.Raise(513 + vbObjectError)
                    Catch ex_1 As Exception
                        Err.Raise(513 + vbObjectError, , ex.Message & vbCrLf & " ex.StackTrace 요약 : " & Regex.Match(ex.StackTrace, "[\dA-z_]+\.vb:줄 [\d]+").ToString)
                    End Try

                End If

            End Try

        Next

    End Function

    Public Shared Function remove_directory(
                                  domain As String,
                                  destination_folder_full_name As String,
                                  user_name As String,
                                  password As String) As String

        Dim request As FtpWebRequest

        request = CType(WebRequest.Create(New Uri(String.Format("ftp://{0}/{1}/", domain, destination_folder_full_name))), FtpWebRequest)

        request.Credentials = New NetworkCredential(user_name, password)
        request.UseBinary = True
        request.UsePassive = False
        request.KeepAlive = False

        request.Method = WebRequestMethods.Ftp.RemoveDirectory

        Dim response As FtpWebResponse

        Try

            response = CType(request.GetResponse(), FtpWebResponse)

        Catch ex As Exception

            Try
                Err.Raise(513 + vbObjectError)
            Catch ex_1 As Exception
                Err.Raise(513 + vbObjectError, , ex.Message & vbCrLf & " ex.StackTrace 요약 : " & Regex.Match(ex.StackTrace, "[\dA-z_]+\.vb:줄 [\d]+").ToString)
            End Try

        End Try

        response.Close()

    End Function

End Class
