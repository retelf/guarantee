<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class smart_contract
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
        Me.btn_coding = New System.Windows.Forms.Button()
        Me.btn_multilevel_pension_contract = New System.Windows.Forms.Button()
        Me.lbl_membership = New System.Windows.Forms.Label()
        Me.lbl_login = New System.Windows.Forms.Label()
        Me.lbl_add_account = New System.Windows.Forms.Label()
        Me.pnl_initial_login = New System.Windows.Forms.Panel()
        Me.lbl_login_state = New System.Windows.Forms.Label()
        Me.wv_working = New Microsoft.Web.WebView2.WinForms.WebView2()
        Me.wv_test_launch = New Microsoft.Web.WebView2.WinForms.WebView2()
        Me.btn_market = New System.Windows.Forms.Button()
        Me.pnl_initial_login.SuspendLayout()
        CType(Me.wv_working, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.wv_test_launch, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btn_coding
        '
        Me.btn_coding.Location = New System.Drawing.Point(23, 98)
        Me.btn_coding.Name = "btn_coding"
        Me.btn_coding.Size = New System.Drawing.Size(116, 23)
        Me.btn_coding.TabIndex = 26
        Me.btn_coding.Text = "coding"
        Me.btn_coding.UseVisualStyleBackColor = True
        '
        'btn_multilevel_pension_contract
        '
        Me.btn_multilevel_pension_contract.Location = New System.Drawing.Point(23, 156)
        Me.btn_multilevel_pension_contract.Name = "btn_multilevel_pension_contract"
        Me.btn_multilevel_pension_contract.Size = New System.Drawing.Size(116, 23)
        Me.btn_multilevel_pension_contract.TabIndex = 28
        Me.btn_multilevel_pension_contract.Text = "multilevel pension"
        Me.btn_multilevel_pension_contract.UseVisualStyleBackColor = True
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
        'pnl_initial_login
        '
        Me.pnl_initial_login.BackColor = System.Drawing.Color.White
        Me.pnl_initial_login.Controls.Add(Me.lbl_login_state)
        Me.pnl_initial_login.Controls.Add(Me.lbl_add_account)
        Me.pnl_initial_login.Controls.Add(Me.lbl_login)
        Me.pnl_initial_login.Controls.Add(Me.lbl_membership)
        Me.pnl_initial_login.Location = New System.Drawing.Point(23, 21)
        Me.pnl_initial_login.Name = "pnl_initial_login"
        Me.pnl_initial_login.Size = New System.Drawing.Size(747, 59)
        Me.pnl_initial_login.TabIndex = 19
        '
        'lbl_login_state
        '
        Me.lbl_login_state.AutoSize = True
        Me.lbl_login_state.Font = New System.Drawing.Font("맑은 고딕", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.lbl_login_state.Location = New System.Drawing.Point(106, 22)
        Me.lbl_login_state.Name = "lbl_login_state"
        Me.lbl_login_state.Size = New System.Drawing.Size(63, 15)
        Me.lbl_login_state.TabIndex = 23
        Me.lbl_login_state.Text = "login state"
        '
        'wv_working
        '
        Me.wv_working.CreationProperties = Nothing
        Me.wv_working.DefaultBackgroundColor = System.Drawing.Color.White
        Me.wv_working.Location = New System.Drawing.Point(176, 98)
        Me.wv_working.Name = "wv_working"
        Me.wv_working.Size = New System.Drawing.Size(594, 505)
        Me.wv_working.TabIndex = 29
        Me.wv_working.ZoomFactor = 1.0R
        '
        'wv_test_launch
        '
        Me.wv_test_launch.CreationProperties = Nothing
        Me.wv_test_launch.DefaultBackgroundColor = System.Drawing.Color.White
        Me.wv_test_launch.Location = New System.Drawing.Point(795, 98)
        Me.wv_test_launch.Name = "wv_test_launch"
        Me.wv_test_launch.Size = New System.Drawing.Size(594, 505)
        Me.wv_test_launch.TabIndex = 30
        Me.wv_test_launch.ZoomFactor = 1.0R
        '
        'btn_market
        '
        Me.btn_market.Location = New System.Drawing.Point(23, 127)
        Me.btn_market.Name = "btn_market"
        Me.btn_market.Size = New System.Drawing.Size(116, 23)
        Me.btn_market.TabIndex = 31
        Me.btn_market.Text = "market"
        Me.btn_market.UseVisualStyleBackColor = True
        '
        'smart_contract
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1414, 628)
        Me.Controls.Add(Me.btn_market)
        Me.Controls.Add(Me.wv_test_launch)
        Me.Controls.Add(Me.wv_working)
        Me.Controls.Add(Me.btn_multilevel_pension_contract)
        Me.Controls.Add(Me.btn_coding)
        Me.Controls.Add(Me.pnl_initial_login)
        Me.Name = "smart_contract"
        Me.Text = "smart_contract"
        Me.pnl_initial_login.ResumeLayout(False)
        Me.pnl_initial_login.PerformLayout()
        CType(Me.wv_working, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.wv_test_launch, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btn_load As Button
    Friend WithEvents btn_coding As Button
    Friend WithEvents wv_test_launch As Microsoft.Web.WebView2.WinForms.WebView2
    Friend WithEvents wv_working As Microsoft.Web.WebView2.WinForms.WebView2
    Friend WithEvents btn_multilevel_pension_contract As Button
    Friend WithEvents lbl_membership As Label
    Friend WithEvents lbl_login As Label
    Friend WithEvents lbl_add_account As Label
    Friend WithEvents pnl_initial_login As Panel
    Friend WithEvents lbl_login_state As Label
    Friend WithEvents WebView21 As Microsoft.Web.WebView2.WinForms.WebView2
    Friend WithEvents WebView22 As Microsoft.Web.WebView2.WinForms.WebView2
    Friend WithEvents btn_market As Button
End Class
