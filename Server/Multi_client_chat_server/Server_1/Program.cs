//Step1. 다중클라이언트 지원 서버 - 주어진 클라이언트와 통신 (다수)

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
    private static List<IPEndPoint> connectedClients = new List<IPEndPoint>();  // 연결된 클라이언트 목록

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

                // 새로운 클라이언트가 연결되면 리스트에 추가
                if (!connectedClients.Contains(clientEndPoint))
                {
                    connectedClients.Add(clientEndPoint);
                    Console.WriteLine($"New client connected: {clientEndPoint.Address}:{clientEndPoint.Port}");
                }

                // 클라이언트로부터 받은 메시지 출력
                Console.WriteLine($"Client {clientEndPoint.Address}:{clientEndPoint.Port} says: {receivedMessage}");

                // 모든 클라이언트에게 메시지 브로드캐스트
                await BroadcastMessageAsync(receivedMessage, clientEndPoint);
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
        string broadcastMessage = $"{senderEndPoint.Address}:{senderEndPoint.Port} says: {message}";
        byte[] broadcastBytes = Encoding.UTF8.GetBytes(broadcastMessage);

        foreach (IPEndPoint client in connectedClients)
        {
            // 메시지를 보낸 클라이언트에게는 전송하지 않음
            if (!client.Equals(senderEndPoint))
            {
                try
                {
                    await udpServer.SendAsync(broadcastBytes, broadcastBytes.Length, client);
                    Console.WriteLine($"Sent to {client.Address}:{client.Port}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending message to {client.Address}:{client.Port}: {ex.Message}");
                }
            }
        }
    }
}
