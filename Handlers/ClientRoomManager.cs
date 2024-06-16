using ChatLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ChatLib.Handlers
{
    public class ClientRoomManager
    {
        public Dictionary<int, List<ClientHandler>> _roomHandlersDict = new();

        //new List

        public void Add(ClientHandler clientHandler)
        {
            int roomId = clientHandler.InitialData!.RoomId;

            if (_roomHandlersDict.TryGetValue(roomId, out _)) // 이미 있는방 입장
            {
                _roomHandlersDict[roomId].Add(clientHandler);

                _roomHandlersDict[roomId].ForEach(x =>
                    Console.WriteLine(x.InitialData)
                    // List.Add(x.InitialData.roomId, x.InitialData.userName)
                ); // 해당 방 사용자 출력
            }
            else //최초 생성 입장
            {
                _roomHandlersDict[roomId] = new List<ClientHandler>() { clientHandler };
                // List.Add(x.InitialData.roomId, x.InitialData.userName)
                _roomHandlersDict[roomId].ForEach(x => Console.WriteLine(x.InitialData)); // 해당 방 사용자 출력
            }
        }

        // public void RoomUser(int Id)
        // {
        //     return newList
        // }

        public void Remove(ClientHandler clientHandler)
        {
            int roomId = clientHandler.InitialData!.RoomId;

            if (_roomHandlersDict.TryGetValue(roomId, out List<ClientHandler>? roomHandlers))
            {
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
