namespace ChatLib.Models
{
  public class GameDetail
  {
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public int RoomId { get; set; }
    public int liarId { get; set; }
    public string word1 { get; set; }
    public string word2 { get; set; }


    public override string ToString()
    {
      return $"RoomId: {RoomId}, UserName: {UserName}";
    }
  }
}
