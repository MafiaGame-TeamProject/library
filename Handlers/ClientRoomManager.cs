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
        public List<Dictionary<int, List<ClientHandler>>> userList;

        public ClientRoomManager()
        {
            userList = new List<Dictionary<int, List<ClientHandler>>>(new Dictionary<int, List<ClientHandler>>[101]);

            for (int i = 0; i <= 100; i++)
            {
                userList[i] = new Dictionary<int, List<ClientHandler>>();
            }
        }

        public List<ClientHandler> Add(ClientHandler clientHandler)
        {
            int roomId = clientHandler.InitialData!.RoomId;

            if (_roomHandlersDict.TryGetValue(roomId, out _)) // 이미 있는방 입장
            {

                _roomHandlersDict[roomId].Add(clientHandler);
                Dictionary<int, List<ClientHandler>> dic = new();


                _roomHandlersDict[roomId].ForEach(x =>
                    {
                        Console.WriteLine(x.InitialData);
                        if (!dic.ContainsKey(roomId))
                        {
                            dic[roomId] = new List<ClientHandler>();
                        }
                        dic[roomId].Add(x);

                        userList[roomId] = dic;
                    }
                ); // 해당 방 사용자 출력
                 return userList[roomId][roomId];
            }
            else //최초 생성 입장
            {
                _roomHandlersDict[roomId] = new List<ClientHandler> { clientHandler };
                Dictionary<int, List<ClientHandler>> dic = new();
                dic[roomId] = new List<ClientHandler> { clientHandler };
                userList[roomId] = dic;
                // List.Add(x.InitialData.roomId, x.InitialData.userName)
                _roomHandlersDict[roomId].ForEach(x => Console.WriteLine(x.InitialData));
                return userList[roomId][roomId];
            }
        }

        public List<ClientHandler> GetRoomUsers(int roomId)
        {
            return userList[roomId][roomId];
        }



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
