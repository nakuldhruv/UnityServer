namespace Game.Client;

partial class MainForm
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        LoginButton = new System.Windows.Forms.Button();
        CpwButton = new System.Windows.Forms.Button();
        RenameButton = new System.Windows.Forms.Button();
        UserIdLabel = new System.Windows.Forms.Label();
        UserNameLabel = new System.Windows.Forms.Label();
        LoginNameTextBox = new System.Windows.Forms.TextBox();
        LoginPasswordTextBox = new System.Windows.Forms.TextBox();
        RenameTextBox = new System.Windows.Forms.TextBox();
        CpwOldPwTextBox = new System.Windows.Forms.TextBox();
        CpwNewPwTextBox = new System.Windows.Forms.TextBox();
        StatusLabel = new System.Windows.Forms.Label();
        SuspendLayout();
        // 
        // LoginButton
        // 
        LoginButton.Location = new System.Drawing.Point(125, 222);
        LoginButton.Name = "LoginButton";
        LoginButton.Size = new System.Drawing.Size(100, 31);
        LoginButton.TabIndex = 1;
        LoginButton.Text = "Login";
        LoginButton.UseVisualStyleBackColor = true;
        LoginButton.Click += LoginButtonClick;
        // 
        // CpwButton
        // 
        CpwButton.Location = new System.Drawing.Point(547, 214);
        CpwButton.Name = "CpwButton";
        CpwButton.Size = new System.Drawing.Size(146, 39);
        CpwButton.TabIndex = 2;
        CpwButton.Text = "ChangePassword";
        CpwButton.UseVisualStyleBackColor = true;
        CpwButton.Click += CpwButtonClick;
        // 
        // RenameButton
        // 
        RenameButton.Location = new System.Drawing.Point(361, 216);
        RenameButton.Name = "RenameButton";
        RenameButton.Size = new System.Drawing.Size(100, 37);
        RenameButton.TabIndex = 3;
        RenameButton.Text = "Rename";
        RenameButton.UseVisualStyleBackColor = true;
        RenameButton.Click += RenameButtonClick;
        // 
        // UserIdLabel
        // 
        UserIdLabel.Location = new System.Drawing.Point(38, 41);
        UserIdLabel.Name = "UserIdLabel";
        UserIdLabel.Size = new System.Drawing.Size(100, 23);
        UserIdLabel.TabIndex = 4;
        UserIdLabel.Text = "UserId";
        // 
        // UserNameLabel
        // 
        UserNameLabel.Location = new System.Drawing.Point(232, 41);
        UserNameLabel.Name = "UserNameLabel";
        UserNameLabel.Size = new System.Drawing.Size(100, 23);
        UserNameLabel.TabIndex = 5;
        UserNameLabel.Text = "UserName";
        // 
        // LoginNameTextBox
        // 
        LoginNameTextBox.Location = new System.Drawing.Point(81, 115);
        LoginNameTextBox.Name = "LoginNameTextBox";
        LoginNameTextBox.Size = new System.Drawing.Size(189, 27);
        LoginNameTextBox.TabIndex = 6;
        // 
        // LoginPasswordTextBox
        // 
        LoginPasswordTextBox.Location = new System.Drawing.Point(81, 173);
        LoginPasswordTextBox.Name = "LoginPasswordTextBox";
        LoginPasswordTextBox.Size = new System.Drawing.Size(189, 27);
        LoginPasswordTextBox.TabIndex = 7;
        // 
        // RenameTextBox
        // 
        RenameTextBox.Location = new System.Drawing.Point(316, 115);
        RenameTextBox.Name = "RenameTextBox";
        RenameTextBox.Size = new System.Drawing.Size(189, 27);
        RenameTextBox.TabIndex = 8;
        // 
        // CpwOldPwTextBox
        // 
        CpwOldPwTextBox.Location = new System.Drawing.Point(526, 115);
        CpwOldPwTextBox.Name = "CpwOldPwTextBox";
        CpwOldPwTextBox.Size = new System.Drawing.Size(189, 27);
        CpwOldPwTextBox.TabIndex = 9;
        // 
        // CpwNewPwTextBox
        // 
        CpwNewPwTextBox.Location = new System.Drawing.Point(526, 173);
        CpwNewPwTextBox.Name = "CpwNewPwTextBox";
        CpwNewPwTextBox.Size = new System.Drawing.Size(189, 27);
        CpwNewPwTextBox.TabIndex = 10;
        // 
        // StatusLable
        // 
        StatusLabel.Location = new System.Drawing.Point(379, 41);
        StatusLabel.Name = "StatusLable";
        StatusLabel.Size = new System.Drawing.Size(138, 23);
        StatusLabel.TabIndex = 11;
        StatusLabel.Text = "Status";
        // 
        // MainForm
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(800, 450);
        Controls.Add(StatusLabel);
        Controls.Add(CpwNewPwTextBox);
        Controls.Add(CpwOldPwTextBox);
        Controls.Add(RenameTextBox);
        Controls.Add(LoginPasswordTextBox);
        Controls.Add(LoginNameTextBox);
        Controls.Add(UserNameLabel);
        Controls.Add(UserIdLabel);
        Controls.Add(RenameButton);
        Controls.Add(CpwButton);
        Controls.Add(LoginButton);
        Text = "Form1";
        ResumeLayout(false);
        PerformLayout();
    }

    private System.Windows.Forms.Label StatusLabel;

    private System.Windows.Forms.TextBox CpwOldPwTextBox;
    private System.Windows.Forms.TextBox CpwNewPwTextBox;

    private System.Windows.Forms.TextBox LoginNameTextBox;
    private System.Windows.Forms.TextBox LoginPasswordTextBox;
    private System.Windows.Forms.TextBox RenameTextBox;

    private System.Windows.Forms.Label UserIdLabel;
    private System.Windows.Forms.Label UserNameLabel;

    private System.Windows.Forms.Button CpwButton;
    private System.Windows.Forms.Button LoginButton;
    private System.Windows.Forms.Button RenameButton;

    #endregion
}