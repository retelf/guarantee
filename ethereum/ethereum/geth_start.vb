Imports System.IO
Imports System.Text.RegularExpressions

Public Class geth_start

    Sub New()

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.

        Dim startInfo As ProcessStartInfo
        Dim process As Process

        process = New Process()

        startInfo = New ProcessStartInfo()

        'MessageBox.Show(Directory.GetCurrentDirectory)

        startInfo.WorkingDirectory = Regex.Replace(Directory.GetCurrentDirectory, "guarantee7\\guarantee7.+", "guarantee7\ethereum\ethereum\bin\Debug\geth")

        startInfo.FileName = "startgeth.bat"

        'startInfo.CreateNoWindow = True

        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Minimized

        process.StartInfo = startInfo
        process.Start()

        'Dim startInfo As ProcessStartInfo = New ProcessStartInfo("geth\geth.exe")

        'startInfo.Verb = "runas"

        'Dim process = System.Diagnostics.Process.Start(startInfo)

    End Sub

End Class