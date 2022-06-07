<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class guarantee7
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
        Me.btn_install = New System.Windows.Forms.Button()
        Me.btn_server = New System.Windows.Forms.Button()
        Me.btn_wallet = New System.Windows.Forms.Button()
        Me.pnl_initial_login = New System.Windows.Forms.Panel()
        Me.lbl_login_state = New System.Windows.Forms.Label()
        Me.lbl_add_account = New System.Windows.Forms.Label()
        Me.lbl_login = New System.Windows.Forms.Label()
        Me.lbl_membership = New System.Windows.Forms.Label()
        Me.btn_reset = New System.Windows.Forms.Button()
        Me.btn_reset_procedure = New System.Windows.Forms.Button()
        Me.btn_smart_contract = New System.Windows.Forms.Button()
        Me.btn_nft = New System.Windows.Forms.Button()
        Me.btn_reset_smart_contract = New System.Windows.Forms.Button()
        Me.wv = New Microsoft.Web.WebView2.WinForms.WebView2()
        Me.txt_block_number = New System.Windows.Forms.TextBox()
        Me.btn_balance = New System.Windows.Forms.Button()
        Me.pnl_initial_login.SuspendLayout()
        CType(Me.wv, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btn_install
        '
        Me.btn_install.Location = New System.Drawing.Point(45, 158)
        Me.btn_install.Name = "btn_install"
        Me.btn_install.Size = New System.Drawing.Size(75, 23)
        Me.btn_install.TabIndex = 0
        Me.btn_install.Text = "(un)install"
        Me.btn_install.UseVisualStyleBackColor = True
        '
        'btn_server
        '
        Me.btn_server.Location = New System.Drawing.Point(45, 187)
        Me.btn_server.Name = "btn_server"
        Me.btn_server.Size = New System.Drawing.Size(75, 23)
        Me.btn_server.TabIndex = 1
        Me.btn_server.Text = "server"
        Me.btn_server.UseVisualStyleBackColor = True
        '
        'btn_wallet
        '
        Me.btn_wallet.Location = New System.Drawing.Point(45, 274)
        Me.btn_wallet.Name = "btn_wallet"
        Me.btn_wallet.Size = New System.Drawing.Size(75, 23)
        Me.btn_wallet.TabIndex = 2
        Me.btn_wallet.Text = "wallet"
        Me.btn_wallet.UseVisualStyleBackColor = True
        '
        'pnl_initial_login
        '
        Me.pnl_initial_login.BackColor = System.Drawing.Color.White
        Me.pnl_initial_login.Controls.Add(Me.lbl_login_state)
        Me.pnl_initial_login.Controls.Add(Me.lbl_add_account)
        Me.pnl_initial_login.Controls.Add(Me.lbl_login)
        Me.pnl_initial_login.Controls.Add(Me.lbl_membership)
        Me.pnl_initial_login.Location = New System.Drawing.Point(45, 81)
        Me.pnl_initial_login.Name = "pnl_initial_login"
        Me.pnl_initial_login.Size = New System.Drawing.Size(723, 59)
        Me.pnl_initial_login.TabIndex = 16
        '
        'lbl_login_state
        '
        Me.lbl_login_state.AutoSize = True
        Me.lbl_login_state.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.lbl_login_state.Location = New System.Drawing.Point(100, 23)
        Me.lbl_login_state.Name = "lbl_login_state"
        Me.lbl_login_state.Size = New System.Drawing.Size(63, 15)
        Me.lbl_login_state.TabIndex = 20
        Me.lbl_login_state.Text = "login state"
        '
        'lbl_add_account
        '
        Me.lbl_add_account.AutoSize = True
        Me.lbl_add_account.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point)
        Me.lbl_add_account.Location = New System.Drawing.Point(577, 23)
        Me.lbl_add_account.Name = "lbl_add_account"
        Me.lbl_add_account.Size = New System.Drawing.Size(55, 15)
        Me.lbl_add_account.TabIndex = 19
        Me.lbl_add_account.Text = "계정추가"
        '
        'lbl_login
        '
        Me.lbl_login.AutoSize = True
        Me.lbl_login.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point)
        Me.lbl_login.Location = New System.Drawing.Point(32, 23)
        Me.lbl_login.Name = "lbl_login"
        Me.lbl_login.Size = New System.Drawing.Size(43, 15)
        Me.lbl_login.TabIndex = 18
        Me.lbl_login.Text = "로그인"
        '
        'lbl_membership
        '
        Me.lbl_membership.AutoSize = True
        Me.lbl_membership.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point)
        Me.lbl_membership.Location = New System.Drawing.Point(643, 23)
        Me.lbl_membership.Name = "lbl_membership"
        Me.lbl_membership.Size = New System.Drawing.Size(55, 15)
        Me.lbl_membership.TabIndex = 4
        Me.lbl_membership.Text = "회원가입"
        '
        'btn_reset
        '
        Me.btn_reset.Location = New System.Drawing.Point(45, 399)
        Me.btn_reset.Name = "btn_reset"
        Me.btn_reset.Size = New System.Drawing.Size(75, 23)
        Me.btn_reset.TabIndex = 17
        Me.btn_reset.Text = "reset all"
        Me.btn_reset.UseVisualStyleBackColor = True
        '
        'btn_reset_procedure
        '
        Me.btn_reset_procedure.Location = New System.Drawing.Point(45, 431)
        Me.btn_reset_procedure.Name = "btn_reset_procedure"
        Me.btn_reset_procedure.Size = New System.Drawing.Size(75, 23)
        Me.btn_reset_procedure.TabIndex = 18
        Me.btn_reset_procedure.Text = "reset proc"
        Me.btn_reset_procedure.UseVisualStyleBackColor = True
        '
        'btn_smart_contract
        '
        Me.btn_smart_contract.Location = New System.Drawing.Point(45, 216)
        Me.btn_smart_contract.Name = "btn_smart_contract"
        Me.btn_smart_contract.Size = New System.Drawing.Size(75, 23)
        Me.btn_smart_contract.TabIndex = 19
        Me.btn_smart_contract.Text = "SC"
        Me.btn_smart_contract.UseVisualStyleBackColor = True
        '
        'btn_nft
        '
        Me.btn_nft.Location = New System.Drawing.Point(45, 245)
        Me.btn_nft.Name = "btn_nft"
        Me.btn_nft.Size = New System.Drawing.Size(75, 23)
        Me.btn_nft.TabIndex = 20
        Me.btn_nft.Text = "NFT"
        Me.btn_nft.UseVisualStyleBackColor = True
        '
        'btn_reset_smart_contract
        '
        Me.btn_reset_smart_contract.Location = New System.Drawing.Point(45, 462)
        Me.btn_reset_smart_contract.Name = "btn_reset_smart_contract"
        Me.btn_reset_smart_contract.Size = New System.Drawing.Size(75, 23)
        Me.btn_reset_smart_contract.TabIndex = 22
        Me.btn_reset_smart_contract.Text = "reset smart"
        Me.btn_reset_smart_contract.UseVisualStyleBackColor = True
        '
        'wv
        '
        Me.wv.CreationProperties = Nothing
        Me.wv.DefaultBackgroundColor = System.Drawing.Color.White
        Me.wv.Location = New System.Drawing.Point(145, 158)
        Me.wv.Name = "wv"
        Me.wv.Size = New System.Drawing.Size(623, 397)
        Me.wv.TabIndex = 23
        Me.wv.ZoomFactor = 1.0R
        '
        'txt_block_number
        '
        Me.txt_block_number.Location = New System.Drawing.Point(45, 496)
        Me.txt_block_number.Name = "txt_block_number"
        Me.txt_block_number.Size = New System.Drawing.Size(75, 23)
        Me.txt_block_number.TabIndex = 24
        '
        'btn_balance
        '
        Me.btn_balance.Location = New System.Drawing.Point(45, 532)
        Me.btn_balance.Name = "btn_balance"
        Me.btn_balance.Size = New System.Drawing.Size(75, 23)
        Me.btn_balance.TabIndex = 25
        Me.btn_balance.Text = "balance"
        Me.btn_balance.UseVisualStyleBackColor = True
        '
        'guarantee7
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(786, 579)
        Me.Controls.Add(Me.btn_balance)
        Me.Controls.Add(Me.txt_block_number)
        Me.Controls.Add(Me.wv)
        Me.Controls.Add(Me.btn_reset_smart_contract)
        Me.Controls.Add(Me.btn_nft)
        Me.Controls.Add(Me.btn_smart_contract)
        Me.Controls.Add(Me.btn_reset_procedure)
        Me.Controls.Add(Me.btn_reset)
        Me.Controls.Add(Me.pnl_initial_login)
        Me.Controls.Add(Me.btn_wallet)
        Me.Controls.Add(Me.btn_server)
        Me.Controls.Add(Me.btn_install)
        Me.Name = "guarantee7"
        Me.Text = "guarantee7"
        Me.pnl_initial_login.ResumeLayout(False)
        Me.pnl_initial_login.PerformLayout()
        CType(Me.wv, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btn_install As Button
    Friend WithEvents btn_server As Button
    Friend WithEvents btn_wallet As Button
    Friend WithEvents pnl_initial_login As Panel
    Friend WithEvents lbl_membership As Label
    Friend WithEvents lbl_login As Label
    Friend WithEvents btn_reset As Button
    Friend WithEvents btn_reset_procedure As Button
    Friend WithEvents lbl_add_account As Label
    Friend WithEvents btn_smart_contract As Button
    Friend WithEvents btn_nft As Button
    Friend WithEvents btn_reset_smart_contract As Button
    Friend WithEvents lbl_login_state As Label
    Friend WithEvents wv As Microsoft.Web.WebView2.WinForms.WebView2
    Friend WithEvents txt_block_number As TextBox
    Friend WithEvents btn_balance As Button
End Class
