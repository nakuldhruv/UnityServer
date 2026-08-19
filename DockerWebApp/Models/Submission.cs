using System.ComponentModel.DataAnnotations;

namespace DockerWebApp.Models;

/// <summary>
/// 用户提交的姓名和邮箱记录。
/// </summary>
public class Submission
{
    public int Id { get; set; }

    [Required(ErrorMessage = "请填写姓名。")]
    [StringLength(100, ErrorMessage = "姓名不能超过 100 个字符。")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "请填写邮箱。")]
    [EmailAddress(ErrorMessage = "请输入有效的邮箱地址。")]
    [StringLength(200, ErrorMessage = "邮箱不能超过 200 个字符。")]
    public string Email { get; set; } = string.Empty;

    public DateTime SubmittedAt { get; set; } = DateTime.Now;
}
