<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class test
    Inherits System.Windows.Forms.Form

    'Form은 Dispose를 재정의하여 구성 요소 목록을 정리합니다.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.btn_socket_listener = New System.Windows.Forms.Button()
        Me.btn_socket_sender = New System.Windows.Forms.Button()
        Me.btn_new_account = New System.Windows.Forms.Button()
        Me.btn_SendRawTransaction = New System.Windows.Forms.Button()
        Me.btn_get_balance = New System.Windows.Forms.Button()
        Me.btn_new_key = New System.Windows.Forms.Button()
        Me.btn_offline = New System.Windows.Forms.Button()
        Me.txt_value = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'btn_socket_listener
        '
        Me.btn_socket_listener.Location = New System.Drawing.Point(70, 55)
        Me.btn_socket_listener.Name = "btn_socket_listener"
        Me.btn_socket_listener.Size = New System.Drawing.Size(104, 23)
        Me.btn_socket_listener.TabIndex = 0
        Me.btn_socket_listener.Text = "socket_listener"
        Me.btn_socket_listener.UseVisualStyleBackColor = True
        '
        'btn_socket_sender
        '
        Me.btn_socket_sender.Location = New System.Drawing.Point(70, 109)
        Me.btn_socket_sender.Name = "btn_socket_sender"
        Me.btn_socket_sender.Size = New System.Drawing.Size(104, 23)
        Me.btn_socket_sender.TabIndex = 1
        Me.btn_socket_sender.Text = "socket_sender"
        Me.btn_socket_sender.UseVisualStyleBackColor = True
        '
        'btn_new_account
        '
        Me.btn_new_account.Location = New System.Drawing.Point(70, 187)
        Me.btn_new_account.Name = "btn_new_account"
        Me.btn_new_account.Size = New System.Drawing.Size(104, 23)
        Me.btn_new_account.TabIndex = 2
        Me.btn_new_account.Text = "new_account"
        Me.btn_new_account.UseVisualStyleBackColor = True
        '
        'btn_SendRawTransaction
        '
        Me.btn_SendRawTransaction.Location = New System.Drawing.Point(70, 268)
        Me.btn_SendRawTransaction.Name = "btn_SendRawTransaction"
        Me.btn_SendRawTransaction.Size = New System.Drawing.Size(104, 23)
        Me.btn_SendRawTransaction.TabIndex = 3
        Me.btn_SendRawTransaction.Text = "SendRawTransaction"
        Me.btn_SendRawTransaction.UseVisualStyleBackColor = True
        '
        'btn_get_balance
        '
        Me.btn_get_balance.Location = New System.Drawing.Point(70, 227)
        Me.btn_get_balance.Name = "btn_get_balance"
        Me.btn_get_balance.Size = New System.Drawing.Size(104, 23)
        Me.btn_get_balance.TabIndex = 4
        Me.btn_get_balance.Text = "get_balance"
        Me.btn_get_balance.UseVisualStyleBackColor = True
        '
        'btn_new_key
        '
        Me.btn_new_key.Location = New System.Drawing.Point(70, 149)
        Me.btn_new_key.Name = "btn_new_key"
        Me.btn_new_key.Size = New System.Drawing.Size(104, 23)
        Me.btn_new_key.TabIndex = 5
        Me.btn_new_key.Text = "new_key"
        Me.btn_new_key.UseVisualStyleBackColor = True
        '
        'btn_offline
        '
        Me.btn_offline.Location = New System.Drawing.Point(70, 307)
        Me.btn_offline.Name = "btn_offline"
        Me.btn_offline.Size = New System.Drawing.Size(104, 23)
        Me.btn_offline.TabIndex = 6
        Me.btn_offline.Text = "offline"
        Me.btn_offline.UseVisualStyleBackColor = True
        '
        'txt_value
        '
        Me.txt_value.Location = New System.Drawing.Point(630, 55)
        Me.txt_value.Name = "txt_value"
        Me.txt_value.Size = New System.Drawing.Size(100, 23)
        Me.txt_value.TabIndex = 7
        Me.txt_value.Text = "value"
        '
        'test
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.txt_value)
        Me.Controls.Add(Me.btn_offline)
        Me.Controls.Add(Me.btn_new_key)
        Me.Controls.Add(Me.btn_get_balance)
        Me.Controls.Add(Me.btn_SendRawTransaction)
        Me.Controls.Add(Me.btn_new_account)
        Me.Controls.Add(Me.btn_socket_sender)
        Me.Controls.Add(Me.btn_socket_listener)
        Me.Name = "test"
        Me.Text = "test"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btn_socket_listener As Button
    Friend WithEvents btn_socket_sender As Button
    Friend WithEvents btn_new_account As Button
    Friend WithEvents btn_SendRawTransaction As Button
    Friend WithEvents btn_get_balance As Button
    Friend WithEvents btn_new_key As Button
    Friend WithEvents btn_offline As Button
    Friend WithEvents txt_value As TextBox
End Class
