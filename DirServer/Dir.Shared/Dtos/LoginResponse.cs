namespace Dir.Shared.Dtos;

public class LoginResponse
{
    public bool IsSuccess { get; set; }
    public LoginResult Data { get; set; }
    public string ErrorMessage { get; set; }

    public static LoginResponse Ok(LoginResult data)
    {
        return new LoginResponse()
        {
            IsSuccess = true,
            Data = data,
        };
    }

    public static LoginResponse Fail(string errorMessage)
    {
        return new LoginResponse()
        {
            IsSuccess = false,
            ErrorMessage = errorMessage,
        };
    }
}