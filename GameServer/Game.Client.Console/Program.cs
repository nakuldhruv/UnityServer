using Game.Client.Services;

GameClientService client = new GameClientService("http://localhost:5018");

try
{
    Console.WriteLine("用户名：");
    var username = Console.ReadLine();
    Console.WriteLine("密码：");
    var password = Console.ReadLine();
    var loginResponse = await client.UserService.LoginAsync(username, password);
    Console.WriteLine($"{loginResponse.Message}");

    Console.WriteLine("新名称：");
    var newName = Console.ReadLine();
    var renameResponse = await client.UserService.RenameAsync(loginResponse.Data.UserId, newName);
    Console.WriteLine($"{renameResponse.Message}");

    Console.WriteLine("旧密码：");
    var oldPassword = Console.ReadLine();
    Console.WriteLine("新密码：");
    var newPassword = Console.ReadLine();
    var changePasswordResponse = await client.UserService.ChangePasswordAsync(loginResponse.Data.UserId, oldPassword, newPassword);
    Console.WriteLine($"{changePasswordResponse.Message}");
}
catch (Exception ex)
{
    Console.WriteLine($"登录失败: {ex.Message}");
}

Console.WriteLine("按任意键退出...");
Console.ReadKey();