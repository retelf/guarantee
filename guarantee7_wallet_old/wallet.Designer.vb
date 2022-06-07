<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class wallet
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
        Me.pnl_coin_login = New System.Windows.Forms.Panel()
        Me.pnl_open_information = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.rb_open_infomation = New System.Windows.Forms.RadioButton()
        Me.rb_not_open_infomation = New System.Windows.Forms.RadioButton()
        Me.cb_coin_name = New System.Windows.Forms.ComboBox()
        Me.pnl_coin_login_inner = New System.Windows.Forms.Panel()
        Me.lbl_coin_name = New System.Windows.Forms.Label()
        Me.rb_account_exist = New System.Windows.Forms.RadioButton()
        Me.rb_account_not_exist = New System.Windows.Forms.RadioButton()
        Me.lbl_private_key = New System.Windows.Forms.Label()
        Me.txt_private_key = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txt_coin_password = New System.Windows.Forms.TextBox()
        Me.lbl_public_key = New System.Windows.Forms.Label()
        Me.btn_coin_login = New System.Windows.Forms.Button()
        Me.txt_public_key = New System.Windows.Forms.TextBox()
        Me.pnl_purchase_coin = New System.Windows.Forms.Panel()
        Me.pnl_purchase_contract = New System.Windows.Forms.Panel()
        Me.pnl_refund = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lbl_login_state = New System.Windows.Forms.Label()
        Me.pnl_sell_membership = New System.Windows.Forms.Panel()
        Me.pnl_initial_login = New System.Windows.Forms.Panel()
        Me.lbl_initial_public_key = New System.Windows.Forms.Label()
        Me.txt_initial_public_key = New System.Windows.Forms.TextBox()
        Me.lbl_membership = New System.Windows.Forms.Label()
        Me.btn_initial_login = New System.Windows.Forms.Button()
        Me.wv = New Microsoft.Web.WebView2.WinForms.WebView2()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.lbl_message = New System.Windows.Forms.Label()
        Me.pnl_coin_login.SuspendLayout()
        Me.pnl_open_information.SuspendLayout()
        Me.pnl_coin_login_inner.SuspendLayout()
        Me.pnl_initial_login.SuspendLayout()
        CType(Me.wv, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnl_coin_login
        '
        Me.pnl_coin_login.Controls.Add(Me.pnl_open_information)
        Me.pnl_coin_login.Controls.Add(Me.cb_coin_name)
        Me.pnl_coin_login.Controls.Add(Me.pnl_coin_login_inner)
        Me.pnl_coin_login.Controls.Add(Me.lbl_private_key)
        Me.pnl_coin_login.Controls.Add(Me.txt_private_key)
        Me.pnl_coin_login.Controls.Add(Me.Label3)
        Me.pnl_coin_login.Controls.Add(Me.txt_coin_password)
        Me.pnl_coin_login.Controls.Add(Me.lbl_public_key)
        Me.pnl_coin_login.Controls.Add(Me.btn_coin_login)
        Me.pnl_coin_login.Controls.Add(Me.txt_public_key)
        Me.pnl_coin_login.Location = New System.Drawing.Point(12, 208)
        Me.pnl_coin_login.Name = "pnl_coin_login"
        Me.pnl_coin_login.Size = New System.Drawing.Size(687, 133)
        Me.pnl_coin_login.TabIndex = 0
        Me.pnl_coin_login.Visible = False
        '
        'pnl_open_information
        '
        Me.pnl_open_information.BackColor = System.Drawing.SystemColors.Control
        Me.pnl_open_information.Controls.Add(Me.Label2)
        Me.pnl_open_information.Controls.Add(Me.rb_open_infomation)
        Me.pnl_open_information.Controls.Add(Me.rb_not_open_infomation)
        Me.pnl_open_information.Location = New System.Drawing.Point(257, 42)
        Me.pnl_open_information.Name = "pnl_open_information"
        Me.pnl_open_information.Size = New System.Drawing.Size(410, 23)
        Me.pnl_open_information.TabIndex = 14
        Me.pnl_open_information.Visible = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(18, 3)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(273, 15)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "잔고나 거래내역 등의 정보 공개를 하시겠습니까?"
        '
        'rb_open_infomation
        '
        Me.rb_open_infomation.AutoSize = True
        Me.rb_open_infomation.Location = New System.Drawing.Point(297, 0)
        Me.rb_open_infomation.Name = "rb_open_infomation"
        Me.rb_open_infomation.Size = New System.Drawing.Size(37, 19)
        Me.rb_open_infomation.TabIndex = 7
        Me.rb_open_infomation.TabStop = True
        Me.rb_open_infomation.Text = "예"
        Me.rb_open_infomation.UseVisualStyleBackColor = True
        '
        'rb_not_open_infomation
        '
        Me.rb_not_open_infomation.AutoSize = True
        Me.rb_not_open_infomation.Location = New System.Drawing.Point(340, -1)
        Me.rb_not_open_infomation.Name = "rb_not_open_infomation"
        Me.rb_not_open_infomation.Size = New System.Drawing.Size(61, 19)
        Me.rb_not_open_infomation.TabIndex = 8
        Me.rb_not_open_infomation.TabStop = True
        Me.rb_not_open_infomation.Text = "아니오"
        Me.rb_not_open_infomation.UseVisualStyleBackColor = True
        '
        'cb_coin_name
        '
        Me.cb_coin_name.FormattingEnabled = True
        Me.cb_coin_name.Items.AddRange(New Object() {"guarantee", "ethereum"})
        Me.cb_coin_name.Location = New System.Drawing.Point(20, 13)
        Me.cb_coin_name.Name = "cb_coin_name"
        Me.cb_coin_name.Size = New System.Drawing.Size(121, 23)
        Me.cb_coin_name.TabIndex = 13
        '
        'pnl_coin_login_inner
        '
        Me.pnl_coin_login_inner.BackColor = System.Drawing.SystemColors.Control
        Me.pnl_coin_login_inner.Controls.Add(Me.lbl_coin_name)
        Me.pnl_coin_login_inner.Controls.Add(Me.rb_account_exist)
        Me.pnl_coin_login_inner.Controls.Add(Me.rb_account_not_exist)
        Me.pnl_coin_login_inner.Location = New System.Drawing.Point(148, 13)
        Me.pnl_coin_login_inner.Name = "pnl_coin_login_inner"
        Me.pnl_coin_login_inner.Size = New System.Drawing.Size(372, 23)
        Me.pnl_coin_login_inner.TabIndex = 11
        '
        'lbl_coin_name
        '
        Me.lbl_coin_name.AutoSize = True
        Me.lbl_coin_name.Location = New System.Drawing.Point(3, 4)
        Me.lbl_coin_name.Name = "lbl_coin_name"
        Me.lbl_coin_name.Size = New System.Drawing.Size(60, 15)
        Me.lbl_coin_name.TabIndex = 9
        Me.lbl_coin_name.Text = "guarantee"
        '
        'rb_account_exist
        '
        Me.rb_account_exist.AutoSize = True
        Me.rb_account_exist.Location = New System.Drawing.Point(76, 2)
        Me.rb_account_exist.Name = "rb_account_exist"
        Me.rb_account_exist.Size = New System.Drawing.Size(144, 19)
        Me.rb_account_exist.TabIndex = 7
        Me.rb_account_exist.TabStop = True
        Me.rb_account_exist.Text = "현재 계정이 있습니다."
        Me.rb_account_exist.UseVisualStyleBackColor = True
        '
        'rb_account_not_exist
        '
        Me.rb_account_not_exist.AutoSize = True
        Me.rb_account_not_exist.Location = New System.Drawing.Point(226, 1)
        Me.rb_account_not_exist.Name = "rb_account_not_exist"
        Me.rb_account_not_exist.Size = New System.Drawing.Size(144, 19)
        Me.rb_account_not_exist.TabIndex = 8
        Me.rb_account_not_exist.TabStop = True
        Me.rb_account_not_exist.Text = "현재 계정이 없습니다."
        Me.rb_account_not_exist.UseVisualStyleBackColor = True
        '
        'lbl_private_key
        '
        Me.lbl_private_key.AutoSize = True
        Me.lbl_private_key.Location = New System.Drawing.Point(22, 103)
        Me.lbl_private_key.Name = "lbl_private_key"
        Me.lbl_private_key.Size = New System.Drawing.Size(43, 15)
        Me.lbl_private_key.TabIndex = 10
        Me.lbl_private_key.Text = "비밀키"
        Me.lbl_private_key.Visible = False
        '
        'txt_private_key
        '
        Me.txt_private_key.Location = New System.Drawing.Point(71, 100)
        Me.txt_private_key.Name = "txt_private_key"
        Me.txt_private_key.Size = New System.Drawing.Size(594, 23)
        Me.txt_private_key.TabIndex = 9
        Me.txt_private_key.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txt_private_key.Visible = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(423, 74)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(95, 15)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "코인별 패스워드"
        '
        'txt_coin_password
        '
        Me.txt_coin_password.Enabled = False
        Me.txt_coin_password.Location = New System.Drawing.Point(522, 71)
        Me.txt_coin_password.Name = "txt_coin_password"
        Me.txt_coin_password.Size = New System.Drawing.Size(143, 23)
        Me.txt_coin_password.TabIndex = 4
        Me.txt_coin_password.Text = "숫자영문6~12자"
        Me.txt_coin_password.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lbl_public_key
        '
        Me.lbl_public_key.AutoSize = True
        Me.lbl_public_key.Location = New System.Drawing.Point(22, 74)
        Me.lbl_public_key.Name = "lbl_public_key"
        Me.lbl_public_key.Size = New System.Drawing.Size(43, 15)
        Me.lbl_public_key.TabIndex = 3
        Me.lbl_public_key.Text = "공개키"
        '
        'btn_coin_login
        '
        Me.btn_coin_login.Location = New System.Drawing.Point(524, 13)
        Me.btn_coin_login.Name = "btn_coin_login"
        Me.btn_coin_login.Size = New System.Drawing.Size(142, 23)
        Me.btn_coin_login.TabIndex = 2
        Me.btn_coin_login.Text = "로그인"
        Me.btn_coin_login.UseVisualStyleBackColor = True
        '
        'txt_public_key
        '
        Me.txt_public_key.Enabled = False
        Me.txt_public_key.Location = New System.Drawing.Point(71, 71)
        Me.txt_public_key.Name = "txt_public_key"
        Me.txt_public_key.Size = New System.Drawing.Size(346, 23)
        Me.txt_public_key.TabIndex = 0
        Me.txt_public_key.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'pnl_purchase_coin
        '
        Me.pnl_purchase_coin.Location = New System.Drawing.Point(12, 428)
        Me.pnl_purchase_coin.Name = "pnl_purchase_coin"
        Me.pnl_purchase_coin.Size = New System.Drawing.Size(687, 141)
        Me.pnl_purchase_coin.TabIndex = 1
        '
        'pnl_purchase_contract
        '
        Me.pnl_purchase_contract.Location = New System.Drawing.Point(12, 620)
        Me.pnl_purchase_contract.Name = "pnl_purchase_contract"
        Me.pnl_purchase_contract.Size = New System.Drawing.Size(687, 123)
        Me.pnl_purchase_contract.TabIndex = 2
        '
        'pnl_refund
        '
        Me.pnl_refund.Location = New System.Drawing.Point(12, 749)
        Me.pnl_refund.Name = "pnl_refund"
        Me.pnl_refund.Size = New System.Drawing.Size(687, 123)
        Me.pnl_refund.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("맑은 고딕", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.Label1.Location = New System.Drawing.Point(194, 18)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(352, 37)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "GUARANTEE COIN WALLET"
        '
        'lbl_login_state
        '
        Me.lbl_login_state.AutoSize = True
        Me.lbl_login_state.Location = New System.Drawing.Point(12, 186)
        Me.lbl_login_state.Name = "lbl_login_state"
        Me.lbl_login_state.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lbl_login_state.Size = New System.Drawing.Size(215, 15)
        Me.lbl_login_state.TabIndex = 14
        Me.lbl_login_state.Text = "개별 코인에 대한 로그인이 필요합니다"
        Me.lbl_login_state.Visible = False
        '
        'pnl_sell_membership
        '
        Me.pnl_sell_membership.Location = New System.Drawing.Point(12, 878)
        Me.pnl_sell_membership.Name = "pnl_sell_membership"
        Me.pnl_sell_membership.Size = New System.Drawing.Size(687, 123)
        Me.pnl_sell_membership.TabIndex = 4
        '
        'pnl_initial_login
        '
        Me.pnl_initial_login.Controls.Add(Me.lbl_initial_public_key)
        Me.pnl_initial_login.Controls.Add(Me.txt_initial_public_key)
        Me.pnl_initial_login.Controls.Add(Me.lbl_membership)
        Me.pnl_initial_login.Controls.Add(Me.btn_initial_login)
        Me.pnl_initial_login.Location = New System.Drawing.Point(12, 116)
        Me.pnl_initial_login.Name = "pnl_initial_login"
        Me.pnl_initial_login.Size = New System.Drawing.Size(687, 59)
        Me.pnl_initial_login.TabIndex = 2
        '
        'lbl_initial_public_key
        '
        Me.lbl_initial_public_key.AutoSize = True
        Me.lbl_initial_public_key.Location = New System.Drawing.Point(22, 23)
        Me.lbl_initial_public_key.Name = "lbl_initial_public_key"
        Me.lbl_initial_public_key.Size = New System.Drawing.Size(43, 15)
        Me.lbl_initial_public_key.TabIndex = 7
        Me.lbl_initial_public_key.Text = "공개키"
        '
        'txt_initial_public_key
        '
        Me.txt_initial_public_key.Location = New System.Drawing.Point(71, 20)
        Me.txt_initial_public_key.Name = "txt_initial_public_key"
        Me.txt_initial_public_key.Size = New System.Drawing.Size(346, 23)
        Me.txt_initial_public_key.TabIndex = 5
        '
        'lbl_membership
        '
        Me.lbl_membership.AutoSize = True
        Me.lbl_membership.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point)
        Me.lbl_membership.Location = New System.Drawing.Point(572, 27)
        Me.lbl_membership.Name = "lbl_membership"
        Me.lbl_membership.Size = New System.Drawing.Size(55, 15)
        Me.lbl_membership.TabIndex = 4
        Me.lbl_membership.Text = "회원가입"
        '
        'btn_initial_login
        '
        Me.btn_initial_login.Location = New System.Drawing.Point(443, 19)
        Me.btn_initial_login.Name = "btn_initial_login"
        Me.btn_initial_login.Size = New System.Drawing.Size(75, 23)
        Me.btn_initial_login.TabIndex = 3
        Me.btn_initial_login.Text = "로그인"
        Me.btn_initial_login.UseVisualStyleBackColor = True
        '
        'wv
        '
        Me.wv.CreationProperties = Nothing
        Me.wv.DefaultBackgroundColor = System.Drawing.Color.White
        Me.wv.Location = New System.Drawing.Point(725, 225)
        Me.wv.Name = "wv"
        Me.wv.Size = New System.Drawing.Size(681, 776)
        Me.wv.TabIndex = 15
        Me.wv.ZoomFactor = 1.0R
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.lbl_message)
        Me.Panel1.Location = New System.Drawing.Point(725, 116)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(681, 78)
        Me.Panel1.TabIndex = 16
        '
        'lbl_message
        '
        Me.lbl_message.AutoSize = True
        Me.lbl_message.Location = New System.Drawing.Point(13, 16)
        Me.lbl_message.Name = "lbl_message"
        Me.lbl_message.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lbl_message.Size = New System.Drawing.Size(53, 15)
        Me.lbl_message.TabIndex = 16
        Me.lbl_message.Text = "message"
        Me.lbl_message.Visible = False
        '
        'wallet
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1424, 1017)
        Me.Controls.Add(Me.lbl_login_state)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.wv)
        Me.Controls.Add(Me.pnl_initial_login)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.pnl_sell_membership)
        Me.Controls.Add(Me.pnl_refund)
        Me.Controls.Add(Me.pnl_purchase_contract)
        Me.Controls.Add(Me.pnl_purchase_coin)
        Me.Controls.Add(Me.pnl_coin_login)
        Me.Name = "wallet"
        Me.Text = "Wallet"
        Me.pnl_coin_login.ResumeLayout(False)
        Me.pnl_coin_login.PerformLayout()
        Me.pnl_open_information.ResumeLayout(False)
        Me.pnl_open_information.PerformLayout()
        Me.pnl_coin_login_inner.ResumeLayout(False)
        Me.pnl_coin_login_inner.PerformLayout()
        Me.pnl_initial_login.ResumeLayout(False)
        Me.pnl_initial_login.PerformLayout()
        CType(Me.wv, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents pnl_coin_login As Panel
    Friend WithEvents pnl_purchase_coin As Panel
    Friend WithEvents btn_coin_login As Button
    Friend WithEvents txt_public_key As TextBox
    Friend WithEvents pnl_purchase_contract As Panel
    Friend WithEvents pnl_refund As Panel
    Friend WithEvents Label3 As Label
    Friend WithEvents txt_coin_password As TextBox
    Friend WithEvents lbl_public_key As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents rb_account_not_exist As RadioButton
    Friend WithEvents rb_account_exist As RadioButton
    Friend WithEvents lbl_private_key As Label
    Friend WithEvents txt_private_key As TextBox
    Friend WithEvents pnl_coin_login_inner As Panel
    Friend WithEvents lbl_coin_name As Label
    Friend WithEvents cb_coin_name As ComboBox
    Friend WithEvents lbl_login_state As Label
    Friend WithEvents pnl_sell_membership As Panel
    Friend WithEvents pnl_initial_login As Panel
    Friend WithEvents lbl_initial_public_key As Label
    Friend WithEvents txt_initial_public_key As TextBox
    Friend WithEvents lbl_membership As Label
    Friend WithEvents btn_initial_login As Button
    Friend WithEvents wv As Microsoft.Web.WebView2.WinForms.WebView2
    Friend WithEvents Panel1 As Panel
    Friend WithEvents lbl_message As Label
    Friend WithEvents pnl_open_information As Panel
    Friend WithEvents Label2 As Label
    Friend WithEvents rb_open_infomation As RadioButton
    Friend WithEvents rb_not_open_infomation As RadioButton
End Class
