using System.Text.Json;

namespace ChatLib.Models
{
  public class GameHub : GameDetail
  {
    public static GameHub? Parse(string json) => JsonSerializer.Deserialize<GameHub>(json)!;

    public ChatState State { get; set; }
    public string Message { get; set; } = string.Empty;

    public string ToJsonString() => JsonSerializer.Serialize(this);
  }
}
