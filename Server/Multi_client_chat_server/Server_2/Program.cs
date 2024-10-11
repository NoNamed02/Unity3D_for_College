//Step2.Server - 닉네임 관리 추가

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

    // 클라이언트 정보 저장 (IPEndPoint, 닉네임)
    private static Dictionary<IPEndPoint, string> connectedClients = new Dictionary<IPEndPoint, string>();

    public static async Task Main(string[] args)
    {
        udpServer = new UdpClient(serverPort);
        Console.WriteLine($"Server is listening on port {serverPort}...");

        while (true)
        {
            try
            {
                // 클라이언트로부터 메시지 수신
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

                // 이미 연결된 클라이언트로부터 받은 메시지
                string senderNickname = connectedClients[clientEndPoint];
                Console.WriteLine($"{senderNickname} ({clientEndPoint.Address}:{clientEndPoint.Port}) says: {receivedMessage}");

                // 모든 클라이언트에게 메시지 브로드캐스트
                await BroadcastMessageAsync($"{senderNickname}: {receivedMessage}", clientEndPoint);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error receiving message: " + ex.Message);
            }
        }
    }

    // 모든 클라이언트에게 메시지 브로드캐스트
    private static async Task BroadcastMessageAsync(string message, IPEndPoint senderEndPoint)
    {
        byte[] broadcastBytes = Encoding.UTF8.GetBytes(message);

        foreach (var client in connectedClients)
        {
            // 메시지를 보낸 클라이언트에게는 전송하지 않음
            if (!client.Key.Equals(senderEndPoint))
            {
                try
                {
                    await udpServer.SendAsync(broadcastBytes, broadcastBytes.Length, client.Key);
                    Console.WriteLine($"Sent to {client.Key.Address}:{client.Key.Port}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending message to {client.Key.Address}:{client.Key.Port}: {ex.Message}");
                }
            }
        }
    }
}
