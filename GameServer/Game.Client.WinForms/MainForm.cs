using Game.Client.Services;
using Game.Shared.Dtos;

namespace Game.Client;

public partial class MainForm : Form
{
    private GameClientService _client;
    private UserDto _user;

    public MainForm()
    {
        InitializeComponent();
        _client = new GameClientService("http://localhost:5018");
    }

    private async void LoginButtonClick(object sender, EventArgs e)
    {
        var response = await _client.UserService.LoginAsync(LoginNameTextBox.Text, LoginPasswordTextBox.Text);
        _user = response.Data;
        UserIdLabel.Text = _user.UserId.ToString();
        UserNameLabel.Text = _user.Name;
        StatusLabel.Text = response.Message;
    }

    private async void CpwButtonClick(object sender, EventArgs e)
    {
        var response = await _client.UserService.ChangePasswordAsync(_user.UserId, CpwOldPwTextBox.Text, CpwNewPwTextBox.Text);
        StatusLabel.Text = response.Message;
    }

    private async void RenameButtonClick(object? sender, EventArgs e)
    {
        var response = await _client.UserService.RenameAsync(_user.UserId, RenameTextBox.Text);
        StatusLabel.Text = response.Message;
    }
}