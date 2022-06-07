<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class install
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
        Me.btn_syncronize = New System.Windows.Forms.Button()
        Me.btn_mariadb_install = New System.Windows.Forms.Button()
        Me.btn_mariadb_download = New System.Windows.Forms.Button()
        Me.btn_get_schma = New System.Windows.Forms.Button()
        Me.txt_database_port_number = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txt_server_port_number = New System.Windows.Forms.TextBox()
        Me.btn_register_server = New System.Windows.Forms.Button()
        Me.btn_get_key = New System.Windows.Forms.Button()
        Me.txt_key = New System.Windows.Forms.TextBox()
        Me.pnl_initial_login = New System.Windows.Forms.Panel()
        Me.lbl_login_state = New System.Windows.Forms.Label()
        Me.lbl_add_account = New System.Windows.Forms.Label()
        Me.lbl_login = New System.Windows.Forms.Label()
        Me.lbl_membership = New System.Windows.Forms.Label()
        Me.wv = New Microsoft.Web.WebView2.WinForms.WebView2()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txt_server_password = New System.Windows.Forms.TextBox()
        Me.pnl_initial_login.SuspendLayout()
        CType(Me.wv, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btn_syncronize
        '
        Me.btn_syncronize.Location = New System.Drawing.Point(23, 459)
        Me.btn_syncronize.Name = "btn_syncronize"
        Me.btn_syncronize.Size = New System.Drawing.Size(181, 23)
        Me.btn_syncronize.TabIndex = 0
        Me.btn_syncronize.Text = "Syncronize"
        Me.btn_syncronize.UseVisualStyleBackColor = True
        '
        'btn_mariadb_install
        '
        Me.btn_mariadb_install.Location = New System.Drawing.Point(23, 254)
        Me.btn_mariadb_install.Name = "btn_mariadb_install"
        Me.btn_mariadb_install.Size = New System.Drawing.Size(181, 23)
        Me.btn_mariadb_install.TabIndex = 2
        Me.btn_mariadb_install.Text = "Database Install(uninstall)"
        Me.btn_mariadb_install.UseVisualStyleBackColor = True
        '
        'btn_mariadb_download
        '
        Me.btn_mariadb_download.Location = New System.Drawing.Point(23, 112)
        Me.btn_mariadb_download.Name = "btn_mariadb_download"
        Me.btn_mariadb_download.Size = New System.Drawing.Size(181, 23)
        Me.btn_mariadb_download.TabIndex = 3
        Me.btn_mariadb_download.Text = "Database(mariadb) Download"
        Me.btn_mariadb_download.UseVisualStyleBackColor = True
        '
        'btn_get_schma
        '
        Me.btn_get_schma.Location = New System.Drawing.Point(23, 421)
        Me.btn_get_schma.Name = "btn_get_schma"
        Me.btn_get_schma.Size = New System.Drawing.Size(181, 23)
        Me.btn_get_schma.TabIndex = 4
        Me.btn_get_schma.Text = "Get Database Schema"
        Me.btn_get_schma.UseVisualStyleBackColor = True
        '
        'txt_database_port_number
        '
        Me.txt_database_port_number.Location = New System.Drawing.Point(154, 213)
        Me.txt_database_port_number.Name = "txt_database_port_number"
        Me.txt_database_port_number.Size = New System.Drawing.Size(50, 23)
        Me.txt_database_port_number.TabIndex = 5
        Me.txt_database_port_number.Text = "30601"
        Me.txt_database_port_number.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(23, 216)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(130, 15)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Database Port Number"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(23, 147)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(114, 15)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Server Port Number"
        '
        'txt_server_port_number
        '
        Me.txt_server_port_number.Location = New System.Drawing.Point(154, 144)
        Me.txt_server_port_number.Name = "txt_server_port_number"
        Me.txt_server_port_number.Size = New System.Drawing.Size(50, 23)
        Me.txt_server_port_number.TabIndex = 7
        Me.txt_server_port_number.Text = "40701"
        Me.txt_server_port_number.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'btn_register_server
        '
        Me.btn_register_server.Location = New System.Drawing.Point(23, 292)
        Me.btn_register_server.Name = "btn_register_server"
        Me.btn_register_server.Size = New System.Drawing.Size(181, 23)
        Me.btn_register_server.TabIndex = 10
        Me.btn_register_server.Text = "Register Server"
        Me.btn_register_server.UseVisualStyleBackColor = True
        '
        'btn_get_key
        '
        Me.btn_get_key.Location = New System.Drawing.Point(23, 492)
        Me.btn_get_key.Name = "btn_get_key"
        Me.btn_get_key.Size = New System.Drawing.Size(181, 23)
        Me.btn_get_key.TabIndex = 11
        Me.btn_get_key.Text = "get_key"
        Me.btn_get_key.UseVisualStyleBackColor = True
        '
        'txt_key
        '
        Me.txt_key.Location = New System.Drawing.Point(230, 493)
        Me.txt_key.Name = "txt_key"
        Me.txt_key.Size = New System.Drawing.Size(540, 23)
        Me.txt_key.TabIndex = 12
        '
        'pnl_initial_login
        '
        Me.pnl_initial_login.BackColor = System.Drawing.Color.White
        Me.pnl_initial_login.Controls.Add(Me.lbl_login_state)
        Me.pnl_initial_login.Controls.Add(Me.lbl_add_account)
        Me.pnl_initial_login.Controls.Add(Me.lbl_login)
        Me.pnl_initial_login.Controls.Add(Me.lbl_membership)
        Me.pnl_initial_login.Location = New System.Drawing.Point(23, 26)
        Me.pnl_initial_login.Name = "pnl_initial_login"
        Me.pnl_initial_login.Size = New System.Drawing.Size(747, 59)
        Me.pnl_initial_login.TabIndex = 17
        '
        'lbl_login_state
        '
        Me.lbl_login_state.AutoSize = True
        Me.lbl_login_state.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.lbl_login_state.Location = New System.Drawing.Point(98, 22)
        Me.lbl_login_state.Name = "lbl_login_state"
        Me.lbl_login_state.Size = New System.Drawing.Size(63, 15)
        Me.lbl_login_state.TabIndex = 24
        Me.lbl_login_state.Text = "login state"
        '
        'lbl_add_account
        '
        Me.lbl_add_account.AutoSize = True
        Me.lbl_add_account.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point)
        Me.lbl_add_account.Location = New System.Drawing.Point(569, 22)
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
        Me.lbl_membership.Location = New System.Drawing.Point(635, 22)
        Me.lbl_membership.Name = "lbl_membership"
        Me.lbl_membership.Size = New System.Drawing.Size(55, 15)
        Me.lbl_membership.TabIndex = 20
        Me.lbl_membership.Text = "회원가입"
        '
        'wv
        '
        Me.wv.CreationProperties = Nothing
        Me.wv.DefaultBackgroundColor = System.Drawing.Color.White
        Me.wv.Location = New System.Drawing.Point(240, 112)
        Me.wv.Name = "wv"
        Me.wv.Size = New System.Drawing.Size(530, 370)
        Me.wv.TabIndex = 18
        Me.wv.ZoomFactor = 1.0R
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(23, 182)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(94, 15)
        Me.Label3.TabIndex = 20
        Me.Label3.Text = "Server Password"
        '
        'txt_server_password
        '
        Me.txt_server_password.Location = New System.Drawing.Point(135, 179)
        Me.txt_server_password.Name = "txt_server_password"
        Me.txt_server_password.Size = New System.Drawing.Size(69, 23)
        Me.txt_server_password.TabIndex = 19
        Me.txt_server_password.Text = "password"
        Me.txt_server_password.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'install
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 538)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txt_server_password)
        Me.Controls.Add(Me.wv)
        Me.Controls.Add(Me.pnl_initial_login)
        Me.Controls.Add(Me.txt_key)
        Me.Controls.Add(Me.btn_get_key)
        Me.Controls.Add(Me.btn_register_server)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txt_server_port_number)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txt_database_port_number)
        Me.Controls.Add(Me.btn_get_schma)
        Me.Controls.Add(Me.btn_mariadb_download)
        Me.Controls.Add(Me.btn_mariadb_install)
        Me.Controls.Add(Me.btn_syncronize)
        Me.Name = "install"
        Me.Text = "install"
        Me.pnl_initial_login.ResumeLayout(False)
        Me.pnl_initial_login.PerformLayout()
        CType(Me.wv, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btn_syncronize As Button
    Friend WithEvents btn_mariadb_install As Button
    Friend WithEvents btn_mariadb_download As Button
    Friend WithEvents btn_get_schma As Button
    Friend WithEvents txt_database_port_number As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents txt_server_port_number As TextBox
    Friend WithEvents btn_register_server As Button
    Friend WithEvents btn_get_key As Button
    Friend WithEvents txt_key As TextBox
    Friend WithEvents pnl_initial_login As Panel
    Friend WithEvents lbl_add_account As Label
    Friend WithEvents lbl_login As Label
    Friend WithEvents lbl_membership As Label
    Friend WithEvents lbl_login_state As Label
    Friend WithEvents wv As Microsoft.Web.WebView2.WinForms.WebView2
    Friend WithEvents Label3 As Label
    Friend WithEvents txt_server_password As TextBox
End Class
