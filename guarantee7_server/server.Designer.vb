<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class server
    Inherits System.Windows.Forms.Form

    'Form은 Dispose를 재정의하여 구성 요소 목록을 정리합니다.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows Form 디자이너에 필요합니다.
    Private components As System.ComponentModel.IContainer

    '참고: 다음 프로시저는 Windows Form 디자이너에 필요합니다.
    '수정하려면 Windows Form 디자이너를 사용하십시오.  
    '코드 편집기에서는 수정하지 마세요.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.txt_monitor = New System.Windows.Forms.TextBox()
        Me.btn_start = New System.Windows.Forms.Button()
        Me.txt_server_password = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.pnl_initial_login = New System.Windows.Forms.Panel()
        Me.lbl_login_state = New System.Windows.Forms.Label()
        Me.lbl_add_account = New System.Windows.Forms.Label()
        Me.lbl_login = New System.Windows.Forms.Label()
        Me.lbl_membership = New System.Windows.Forms.Label()
        Me.wv = New Microsoft.Web.WebView2.WinForms.WebView2()
        Me.pnl_initial_login.SuspendLayout()
        CType(Me.wv, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txt_monitor
        '
        Me.txt_monitor.Location = New System.Drawing.Point(649, 56)
        Me.txt_monitor.Multiline = True
        Me.txt_monitor.Name = "txt_monitor"
        Me.txt_monitor.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txt_monitor.Size = New System.Drawing.Size(690, 841)
        Me.txt_monitor.TabIndex = 3
        '
        'btn_start
        '
        Me.btn_start.Location = New System.Drawing.Point(1261, 12)
        Me.btn_start.Name = "btn_start"
        Me.btn_start.Size = New System.Drawing.Size(75, 23)
        Me.btn_start.TabIndex = 2
        Me.btn_start.Text = "start server"
        Me.btn_start.UseVisualStyleBackColor = True
        '
        'txt_server_password
        '
        Me.txt_server_password.Location = New System.Drawing.Point(757, 12)
        Me.txt_server_password.Name = "txt_server_password"
        Me.txt_server_password.Size = New System.Drawing.Size(100, 23)
        Me.txt_server_password.TabIndex = 4
        Me.txt_server_password.Text = "password"
        Me.txt_server_password.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(648, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(94, 15)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Server Password"
        '
        'pnl_initial_login
        '
        Me.pnl_initial_login.BackColor = System.Drawing.Color.White
        Me.pnl_initial_login.Controls.Add(Me.lbl_login_state)
        Me.pnl_initial_login.Controls.Add(Me.lbl_add_account)
        Me.pnl_initial_login.Controls.Add(Me.lbl_login)
        Me.pnl_initial_login.Controls.Add(Me.lbl_membership)
        Me.pnl_initial_login.Location = New System.Drawing.Point(19, 15)
        Me.pnl_initial_login.Name = "pnl_initial_login"
        Me.pnl_initial_login.Size = New System.Drawing.Size(615, 109)
        Me.pnl_initial_login.TabIndex = 18
        '
        'lbl_login_state
        '
        Me.lbl_login_state.AutoSize = True
        Me.lbl_login_state.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.lbl_login_state.Location = New System.Drawing.Point(83, 22)
        Me.lbl_login_state.Name = "lbl_login_state"
        Me.lbl_login_state.Size = New System.Drawing.Size(63, 15)
        Me.lbl_login_state.TabIndex = 24
        Me.lbl_login_state.Text = "login state"
        '
        'lbl_add_account
        '
        Me.lbl_add_account.AutoSize = True
        Me.lbl_add_account.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point)
        Me.lbl_add_account.Location = New System.Drawing.Point(463, 22)
        Me.lbl_add_account.Name = "lbl_add_account"
        Me.lbl_add_account.Size = New System.Drawing.Size(55, 15)
        Me.lbl_add_account.TabIndex = 22
        Me.lbl_add_account.Text = "계정추가"
        '
        'lbl_login
        '
        Me.lbl_login.AutoSize = True
        Me.lbl_login.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point)
        Me.lbl_login.Location = New System.Drawing.Point(24, 22)
        Me.lbl_login.Name = "lbl_login"
        Me.lbl_login.Size = New System.Drawing.Size(43, 15)
        Me.lbl_login.TabIndex = 21
        Me.lbl_login.Text = "로그인"
        '
        'lbl_membership
        '
        Me.lbl_membership.AutoSize = True
        Me.lbl_membership.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point)
        Me.lbl_membership.Location = New System.Drawing.Point(529, 22)
        Me.lbl_membership.Name = "lbl_membership"
        Me.lbl_membership.Size = New System.Drawing.Size(55, 15)
        Me.lbl_membership.TabIndex = 20
        Me.lbl_membership.Text = "회원가입"
        '
        'wv
        '
        Me.wv.CreationProperties = Nothing
        Me.wv.DefaultBackgroundColor = System.Drawing.Color.White
        Me.wv.Location = New System.Drawing.Point(19, 138)
        Me.wv.Name = "wv"
        Me.wv.Size = New System.Drawing.Size(615, 762)
        Me.wv.TabIndex = 19
        Me.wv.ZoomFactor = 1.0R
        '
        'server
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(1370, 913)
        Me.Controls.Add(Me.wv)
        Me.Controls.Add(Me.pnl_initial_login)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txt_monitor)
        Me.Controls.Add(Me.btn_start)
        Me.Controls.Add(Me.txt_server_password)
        Me.Name = "server"
        Me.Text = "main"
        Me.pnl_initial_login.ResumeLayout(False)
        Me.pnl_initial_login.PerformLayout()
        CType(Me.wv, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txt_monitor As TextBox
    Friend WithEvents btn_start As Button
    Friend WithEvents txt_server_password As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents pnl_initial_login As Panel
    Friend WithEvents lbl_login_state As Label
    Friend WithEvents lbl_add_account As Label
    Friend WithEvents lbl_login As Label
    Friend WithEvents lbl_membership As Label
    Friend WithEvents wv As Microsoft.Web.WebView2.WinForms.WebView2
End Class
