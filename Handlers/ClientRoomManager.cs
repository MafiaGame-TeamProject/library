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
        //new List

        public void Add(ClientHandler clientHandler)
        {
            int roomId = clientHandler.InitialData!.RoomId;

            if (_roomHandlersDict.TryGetValue(roomId, out _)) // 이미 있는방 입장
            {

                _roomHandlersDict[roomId].Add(clientHandler);
                Dictionary<int, List<ClientHandler>> dic = new();
                int i= 0;
                
                _roomHandlersDict[roomId].ForEach(x =>
                    {
                        Console.WriteLine(x.InitialData);
                        dic[roomId][i] = x;
                        userList[roomId] = dic;
                        i++;
                    }
                ); // 해당 방 사용자 출력
            }
            else //최초 생성 입장
            {
                _roomHandlersDict[roomId] = new List<ClientHandler>() { clientHandler };
                Dictionary<int, List<ClientHandler>> dic = new();
                dic[roomId][0] = clientHandler;
                userList[roomId] = dic;
                // List.Add(x.InitialData.roomId, x.InitialData.userName)
                _roomHandlersDict[roomId].ForEach(x => Console.WriteLine(x.InitialData)); // 해당 방 사용자 출력
            }
        }

        public List<ClientHandler> RoomUser(int Id)
        {
            // 서버에서 클라이언트 들어온 거 처리할 때 까지 기다리는 코드 넣어야 할듯
            return userList[Id][Id];
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
