using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;

class UdpChatServer
{
    private static int serverPort = 8080;
    private static UdpClient udpServer;
    private static Dictionary<IPEndPoint, string> connectedClients = new Dictionary<IPEndPoint, string>();  // 연결된 클라이언트와 닉네임

    public static async Task Main(string[] args)
    {
        udpServer = new UdpClient(serverPort);
        Console.WriteLine($"Server is listening on port {serverPort}...");

        while (true)
        {
            try
            {
                // 비동기로 클라이언트 메시지 수신
                UdpReceiveResult receivedResult = await udpServer.ReceiveAsync();
                IPEndPoint clientEndPoint = receivedResult.RemoteEndPoint;
                string receivedMessage = Encoding.UTF8.GetString(receivedResult.Buffer);

                // 새로운 클라이언트가 닉네임을 설정할 때
                if (!connectedClients.ContainsKey(clientEndPoint))
                {
                    connectedClients.Add(clientEndPoint, receivedMessage);  // 닉네임을 저장
                    Console.WriteLine($"New client connected: {clientEndPoint.Address}:{clientEndPoint.Port} with nickname '{receivedMessage}'");

                    // 다른 클라이언트들에게 새 클라이언트 접속 알림
                    await BroadcastMessageAsync($"{receivedMessage} has joined the chat.", clientEndPoint);
                    continue;  // 닉네임 설정 후 메시지 처리 종료
                }

                string clientNickname = connectedClients[clientEndPoint];
                Console.WriteLine($"{clientNickname} say - {receivedMessage}");

                // 다른 클라이언트에게 메시지 브로드캐스트
                if (receivedMessage == "disconnect")
                    await BroadcastMessageAsync($"{receivedMessage}", clientEndPoint);
                await BroadcastMessageAsync($"{clientNickname} : {receivedMessage}", clientEndPoint);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error : " + ex.Message);
            }
        }
    }

        private static async Task BroadcastMessageAsync(string message, IPEndPoint senderEndPoint)
        {
            // 메시지 수신자가 보낸 경우
            if (message == "disconnect")
            {
                connectedClients.Remove(senderEndPoint);

                string exitMessage = $"{connectedClients[senderEndPoint]} has left the chat.";
                byte[] exitMessageBytes = Encoding.UTF8.GetBytes(exitMessage);

                foreach (IPEndPoint client in connectedClients.Keys)
                {
                    await udpServer.SendAsync(exitMessageBytes, exitMessageBytes.Length, client);
                }

                
                return;
            }

            string clientNickname = connectedClients[senderEndPoint];
            string broadcastMessage = $"{message}";
            byte[] broadcastBytes = Encoding.UTF8.GetBytes(broadcastMessage);

            foreach (IPEndPoint client in connectedClients.Keys)
            {
                if (!client.Equals(senderEndPoint))  // 메시지를 보낸 클라이언트를 제외
                {
                    await udpServer.SendAsync(broadcastBytes, broadcastBytes.Length, client);
                }
            }
        }
}
