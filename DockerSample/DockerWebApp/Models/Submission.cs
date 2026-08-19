using System.ComponentModel.DataAnnotations;

namespace DockerWebApp.Models;

/// <summary>
/// 用户提交的邮箱记录。
/// </summary>
public class Submission
{
    public int Id { get; set; }

    [Required(ErrorMessage = "请填写邮箱。")]
    [EmailAddress(ErrorMessage = "请输入有效的邮箱地址。")]
    [StringLength(200, ErrorMessage = "邮箱不能超过 200 个字符。")]
    public string Email { get; set; } = string.Empty;

    public DateTime SubmittedAt { get; set; } = DateTime.Now;
}
