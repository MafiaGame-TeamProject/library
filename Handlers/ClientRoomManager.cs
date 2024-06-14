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

    public void Add(ClientHandler clientHandler)
    {
      int roomId = clientHandler.InitialData!.RoomId;

      if (_roomHandlersDict.TryGetValue(roomId, out _)){
         // 두번째 사용자 삽입부터
         if(_roomHandlersDict[roomId].Count >= 4) {
            Console.WriteLine("게임시작");
            //게임시작
         } else {
           _roomHandlersDict[roomId].Add(clientHandler);
         }
      }
      else
      {
        //첫 방 생성 뒤 사용자 삽입
        _roomHandlersDict[roomId] = new List<ClientHandler>() { clientHandler };
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
