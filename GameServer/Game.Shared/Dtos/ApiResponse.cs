using MessagePack;

namespace Game.Shared.Dtos;

[MessagePackObject]
public class ApiResponse<T>
{
    [Key(0)] public bool Success { get; set; }
    [Key(1)] public string Message { get; set; }
    [Key(2)] public T Data { get; set; }
}