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
        Me.wv_main = New Microsoft.Web.WebView2.WinForms.WebView2()
        Me.wv_sub = New Microsoft.Web.WebView2.WinForms.WebView2()
        Me.pnl_initial_login = New System.Windows.Forms.Panel()
        Me.lbl_login_state = New System.Windows.Forms.Label()
        Me.lbl_add_account = New System.Windows.Forms.Label()
        Me.lbl_login = New System.Windows.Forms.Label()
        Me.lbl_membership = New System.Windows.Forms.Label()
        Me.btn_smart_contract = New System.Windows.Forms.Button()
        Me.btn_transfer = New System.Windows.Forms.Button()
        Me.btn_nft = New System.Windows.Forms.Button()
        Me.btn_exchange = New System.Windows.Forms.Button()
        Me.btn_multilevel = New System.Windows.Forms.Button()
        Me.btn_clear = New System.Windows.Forms.Button()
        CType(Me.wv_main, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.wv_sub, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnl_initial_login.SuspendLayout()
        Me.SuspendLayout()
        '
        'wv_main
        '
        Me.wv_main.CreationProperties = Nothing
        Me.wv_main.DefaultBackgroundColor = System.Drawing.Color.White
        Me.wv_main.Location = New System.Drawing.Point(139, 141)
        Me.wv_main.Name = "wv_main"
        Me.wv_main.Size = New System.Drawing.Size(637, 478)
        Me.wv_main.TabIndex = 32
        Me.wv_main.ZoomFactor = 1.0R
        '
        'wv_sub
        '
        Me.wv_sub.CreationProperties = Nothing
        Me.wv_sub.DefaultBackgroundColor = System.Drawing.Color.White
        Me.wv_sub.Location = New System.Drawing.Point(797, 21)
        Me.wv_sub.Name = "wv_sub"
        Me.wv_sub.Size = New System.Drawing.Size(752, 598)
        Me.wv_sub.TabIndex = 36
        Me.wv_sub.ZoomFactor = 1.0R
        '
        'pnl_initial_login
        '
        Me.pnl_initial_login.BackColor = System.Drawing.Color.White
        Me.pnl_initial_login.Controls.Add(Me.lbl_login_state)
        Me.pnl_initial_login.Controls.Add(Me.lbl_add_account)
        Me.pnl_initial_login.Controls.Add(Me.lbl_login)
        Me.pnl_initial_login.Controls.Add(Me.lbl_membership)
        Me.pnl_initial_login.Location = New System.Drawing.Point(29, 21)
        Me.pnl_initial_login.Name = "pnl_initial_login"
        Me.pnl_initial_login.Size = New System.Drawing.Size(747, 105)
        Me.pnl_initial_login.TabIndex = 27
        '
        'lbl_login_state
        '
        Me.lbl_login_state.AutoSize = True
        Me.lbl_login_state.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.lbl_login_state.Location = New System.Drawing.Point(91, 22)
        Me.lbl_login_state.Name = "lbl_login_state"
        Me.lbl_login_state.Size = New System.Drawing.Size(63, 15)
        Me.lbl_login_state.TabIndex = 23
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
        'btn_smart_contract
        '
        Me.btn_smart_contract.Location = New System.Drawing.Point(29, 286)
        Me.btn_smart_contract.Name = "btn_smart_contract"
        Me.btn_smart_contract.Size = New System.Drawing.Size(75, 23)
        Me.btn_smart_contract.TabIndex = 33
        Me.btn_smart_contract.Text = "smartC"
        Me.btn_smart_contract.UseVisualStyleBackColor = True
        '
        'btn_transfer
        '
        Me.btn_transfer.Location = New System.Drawing.Point(29, 141)
        Me.btn_transfer.Name = "btn_transfer"
        Me.btn_transfer.Size = New System.Drawing.Size(75, 23)
        Me.btn_transfer.TabIndex = 34
        Me.btn_transfer.Text = "transfer"
        Me.btn_transfer.UseVisualStyleBackColor = True
        '
        'btn_nft
        '
        Me.btn_nft.Location = New System.Drawing.Point(31, 257)
        Me.btn_nft.Name = "btn_nft"
        Me.btn_nft.Size = New System.Drawing.Size(73, 23)
        Me.btn_nft.TabIndex = 35
        Me.btn_nft.Text = "NFT"
        Me.btn_nft.UseVisualStyleBackColor = True
        '
        'btn_exchange
        '
        Me.btn_exchange.Location = New System.Drawing.Point(29, 199)
        Me.btn_exchange.Name = "btn_exchange"
        Me.btn_exchange.Size = New System.Drawing.Size(75, 23)
        Me.btn_exchange.TabIndex = 37
        Me.btn_exchange.Text = "exchange"
        Me.btn_exchange.UseVisualStyleBackColor = True
        '
        'btn_multilevel
        '
        Me.btn_multilevel.Location = New System.Drawing.Point(31, 228)
        Me.btn_multilevel.Name = "btn_multilevel"
        Me.btn_multilevel.Size = New System.Drawing.Size(73, 23)
        Me.btn_multilevel.TabIndex = 38
        Me.btn_multilevel.Text = "multilevel"
        Me.btn_multilevel.UseVisualStyleBackColor = True
        '
        'btn_clear
        '
        Me.btn_clear.Location = New System.Drawing.Point(29, 170)
        Me.btn_clear.Name = "btn_clear"
        Me.btn_clear.Size = New System.Drawing.Size(75, 23)
        Me.btn_clear.TabIndex = 39
        Me.btn_clear.Text = "clear"
        Me.btn_clear.UseVisualStyleBackColor = True
        '
        'wallet
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1570, 642)
        Me.Controls.Add(Me.btn_clear)
        Me.Controls.Add(Me.btn_multilevel)
        Me.Controls.Add(Me.btn_exchange)
        Me.Controls.Add(Me.btn_nft)
        Me.Controls.Add(Me.btn_transfer)
        Me.Controls.Add(Me.btn_smart_contract)
        Me.Controls.Add(Me.pnl_initial_login)
        Me.Controls.Add(Me.wv_main)
        Me.Controls.Add(Me.wv_sub)
        Me.Location = New System.Drawing.Point(200, 200)
        Me.Name = "wallet"
        Me.Text = "wallet"
        CType(Me.wv_main, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.wv_sub, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnl_initial_login.ResumeLayout(False)
        Me.pnl_initial_login.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pnl_initial_login As Panel
    Friend WithEvents lbl_add_account As Label
    Friend WithEvents lbl_login As Label
    Friend WithEvents lbl_membership As Label
    Friend WithEvents wv_main As Microsoft.Web.WebView2.WinForms.WebView2
    Friend WithEvents btn_smart_contract As Button
    Friend WithEvents btn_transfer As Button
    Friend WithEvents btn_nft As Button
    Friend WithEvents wv_sub As Microsoft.Web.WebView2.WinForms.WebView2
    Friend WithEvents lbl_login_state As Label
    Friend WithEvents btn_exchange As Button
    Friend WithEvents btn_multilevel As Button
    Friend WithEvents btn_clear As Button
End Class
