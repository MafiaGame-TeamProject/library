using ChatLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatLib.Handlers
{
  public class ClientRoomManager
  {
    private Dictionary<int, List<ClientHandler>> _roomHandlersDict = new();

    public int Add(ClientHandler clientHandler)
    {
      int roomId = clientHandler.InitialData!.RoomId;

      if (_roomHandlersDict.TryGetValue(roomId, out _)){
      // 이미 생성된 방에 사용자가 입장할 때

        if(_roomHandlersDict[roomId].Count == 4){
          // 가득 찼을 때 0 반환
          return 0;
        }

        _roomHandlersDict[roomId].Add(clientHandler);
        return _roomHandlersDict[roomId].Count;
      } else {
        //첫 방 생성(사용자가 사용하지 않는 방을 지정하고 입장할 때
        _roomHandlersDict[roomId] = new List<ClientHandler>() { clientHandler };
        return 1;
      }
    }

    public void Remove(ClientHandler clientHandler)
    {
      int roomId = clientHandler.InitialData!.RoomId;

      if (_roomHandlersDict.TryGetValue(roomId, out List<ClientHandler>? roomHandlers)) {
        _roomHandlersDict[roomId] = roomHandlers.FindAll(handler => !handler.Equals(clientHandler));
      }
    }

    public void SendToMyRoom(ChatHub hub)
    {
      if (_roomHandlersDict.TryGetValue(hub.RoomId, out List<ClientHandler>? roomHandlers))
      {
        roomHandlers.ForEach(handler => handler.Send(hub));
      }
    }
  }
}
